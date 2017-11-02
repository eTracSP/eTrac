using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel.ApiModel;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class PassengerTrackingManager : IPassengerTracking
    {
        /// <summary>
        /// Created By Bhushan Dod
        /// Dated : Oct-12-2017
        /// For Saving and editing passenger route.
        /// </summary>
        /// <param name="objeFleetPassengerTrackingModel"></param>
        /// <returns></returns>
        public Result SavePassengerTrackingRoute(eFleetPassengerTrackingModel objeFleetPassengerTrackingModel)
        {
            Result obj;
            try
            {                
                var objeFleetPassengerTrackingRoute = new eFleetPassengerTrackingRoute();
                var objeFleetPassengerTrackingRepository = new eFleetPassengerTrackingRepository();
                var objeTracLoginModel = new eTracLoginModel();

                if (objeFleetPassengerTrackingModel.RouteID == 0)
                {
                    AutoMapper.Mapper.CreateMap<eFleetPassengerTrackingRoute, eFleetPassengerTrackingModel>();
                    var objfleetMaintenanceMapper = AutoMapper.Mapper.Map(objeFleetPassengerTrackingModel, objeFleetPassengerTrackingRoute);
                    objeFleetPassengerTrackingRepository.Add(objfleetMaintenanceMapper);                  
                    objeFleetPassengerTrackingRepository.SaveChanges();
                    obj = Result.Completed;                    
                }
                //edit Data
                else
                {
                    var RouteData = objeFleetPassengerTrackingRepository.GetAll(v => v.IsDeleted == false && v.RouteID == objeFleetPassengerTrackingModel.RouteID).SingleOrDefault();
                    AutoMapper.Mapper.CreateMap<eFleetPassengerTrackingModel, eFleetPassengerTrackingRoute>();
                    var objfleetDriverMapper = AutoMapper.Mapper.Map(objeFleetPassengerTrackingModel, RouteData);
                    objeFleetPassengerTrackingRepository.SaveChanges();
                    obj = Result.UpdatedSuccessfully;                  
                }
                return obj;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "Result SavePassengerTrackingRoute(eFleetPassengerTrackingModel objeFleetPassengerTrackingModel)", "Exception While saving efleet passenger route.", objeFleetPassengerTrackingModel);
                throw;
            }
        }
        /// <summary>
        /// Created By Ashwajit Bansod Date : 12 - Oct - 2017
        /// For Fetching the List of Route table according to service type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<eFleetPassengerTrackingRouteModel> GetAllPassengerTrackingRouteDetails(eFleetPassengerTrackingRouteServiceModel obj)
        {
            var objeFleetPassengerTrackingRouteModel = new eFleetPassengerTrackingRouteModel();
            DateTime TodaysDate = DateTime.UtcNow;
            try
            {
                var db = new workorderEMSEntities();
                long serviceType = (obj.ServiceType == (long)eFleetEnum.Regular) ? (long)eFleetEnum.Regular : (long)eFleetEnum.Event;
                var Results = db.eFleetPassengerTrackingRoutes.Where(a => a.IsDeleted == false && a.ServiceType == serviceType
                                                                                         && (DbFunctions.TruncateTime(a.StartDate) <= TodaysDate.Date)
                                                                                         && (DbFunctions.TruncateTime(a.EndDate) >= TodaysDate.Date))
                                                                .Select(a => new eFleetPassengerTrackingRouteModel()
                                                                {
                                                                    RouteID = a.RouteID,
                                                                    ServiceType = a.GlobalCode.GlobalCodeId,
                                                                    StartDate = a.StartDate,
                                                                    EndDate = a.EndDate,
                                                                    RouteName = a.RouteName
                                                                }).ToList<eFleetPassengerTrackingRouteModel>();

                return Results;
            }
                                                                                           
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<eFleetPassengerTrackingRouteModel> GetAllPassengerTrackingRouteDetails(eFleetPassengerTrackingRouteServiceModel obj)", "Exception While Listing Route detail.", obj.UserId);
                throw;
            }
        }
        /// <summary>
        /// Created By Ashwajit Bansod Dated : 12 - Oct - 2017
        /// For Saving the Passenger Tracking Count into database
        /// </summary>
        /// <param name="objModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> InsertPassengerTrackingCount(eFleetPassengerTrackingCountModelForService objModel)
        {
            var objReturnModel = new ServiceResponseModel<string>();
            try
            {
                var objPassengerTrackingCountRepository = new PassengerTrackingCountRepository();
                eFleetPassengerTrackingCount Obj = new eFleetPassengerTrackingCount();
                AutoMapper.Mapper.CreateMap<eFleetPassengerTrackingCountModelForService, eFleetPassengerTrackingCount>();
                Obj = AutoMapper.Mapper.Map(objModel, Obj);
                Obj.CreatedBy = objModel.UserId;
                Obj.CreatedDate = DateTime.UtcNow;
                objPassengerTrackingCountRepository.Add(Obj);
                if (Obj.PassengerCountID > 0)
                { 
                    var Data = objPassengerTrackingCountRepository.GetAll(pm => pm.PassengerCountID == objModel.PassengerCountID && pm.IsDeleted == false).FirstOrDefault();
                    objReturnModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.InvariantCulture);
                    objReturnModel.Message = CommonMessage.Successful();
                }
                else
                {
                    objReturnModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.InvariantCulture);
                    objReturnModel.Message = CommonMessage.NoRecordMessage();
                }
            }
            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ServiceResponseModel<string> InsertPassengerTrackingCount(eFleetPassengerTrackingCountModelForService objModel)", "while insert Passenger Tracking Count", objModel);
                objReturnModel.Message = ex.Message;
                objReturnModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                objReturnModel.Data = null;
            }
            return objReturnModel;
        }
        /// <summary>
        /// Created By Ashwajit Bansod
        /// Dated : Oct/13/2017
        /// Fetching the data for edit
        /// </summary>
        /// <param name="RouteId"></param>
        /// <returns></returns>
        public eFleetPassengerTrackingModel GeteFleetPassengerTrackingDetailsById(long RouteId)
        {
            try
            {
                var objeFleetPassengerTrackingRepository = new eFleetPassengerTrackingRepository();
                var editeFleetPassengerTrackingDetails = new eFleetPassengerTrackingModel();
                var PassengerTrackingDetails = objeFleetPassengerTrackingRepository.GetSingleOrDefault(u => u.RouteID == RouteId);
                if (PassengerTrackingDetails.RouteID > 0) // PmID in Ashwajit created Table
                {
                    AutoMapper.Mapper.CreateMap<eFleetPassengerTrackingRoute, eFleetPassengerTrackingModel>();
                    editeFleetPassengerTrackingDetails.RouteID = PassengerTrackingDetails.RouteID;
                    var objfleetPassengerTackMapper = AutoMapper.Mapper.Map(PassengerTrackingDetails, editeFleetPassengerTrackingDetails);
                }
                return editeFleetPassengerTrackingDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public eFleetPassengerTrackingModel GeteFleetPassengerTrackingDetailsById(long RouteId)", "Exception While Editing Passenger Tracking Route.", null);
                throw;
            }
        }
        /// <summary>
        /// Created By Ashwajit Bansod
        /// Dated : Oct/13/2017
        /// For deleting the Passenger Tracking Route
        /// </summary>
        /// <param name="passengerId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public Result DeleteeFleetPassengerTracking(long passengerId, long loggedInUserId, string location)
        {
            var objDAR = new DARModel();
            try
            {
                if (passengerId > 0)
                {
                    var objeFleetPassengerTrackingRepository = new eFleetPassengerTrackingRepository();
                    var data = objeFleetPassengerTrackingRepository.GetSingleOrDefault(v => v.RouteID == passengerId && v.IsDeleted == false); // PmID in Ashwajit Created Table
                    if (data != null)
                    {
                        data.IsDeleted = true;
                        data.DeletedBy = loggedInUserId;
                        data.DeletedDate = DateTime.UtcNow;
                        objeFleetPassengerTrackingRepository.Update(data);
                        objeFleetPassengerTrackingRepository.SaveChanges();

                        //objDAR.ActivityDetails = DarMessage.DeleteFleetPM(location);
                        //objDAR.TaskType = (long)TaskTypeCategory.DeletePreventativeMaintenance;

                        //#region Save DAR
                        //objDAR.LocationId = data.LocationID;
                        //objDAR.UserId = loggedInUserId;
                        //objDAR.DeletedBy = data.DeletedBy;
                        //objDAR.DeletedOn = DateTime.UtcNow;
                        //result = _ICommonMethod.SaveDAR(objDAR);
                        //#endregion Save DAR
                        return Result.Delete;
                    }
                }
                else { return Result.DoesNotExist; }
                return Result.Delete;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public Result DeleteeFleetPassengerTracking(long passengerId, long loggedInUserId, string location)", "Exception While Deleting eFleet Passenger Tracking.", null);
                throw;
            }
        }
    }
}
