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
using WorkOrderEMS.Models.CommonModels;
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
        /// 
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
                    //code to insert new record if user added new route while edit
                    if (( objeFleetPassengerTrackingModel.PickupList != null || objeFleetPassengerTrackingModel.PickupList != "") &&
                       (objeFleetPassengerTrackingModel.DropList != null || objeFleetPassengerTrackingModel.DropList != ""))
                    {
                        objeFleetPassengerTrackingModel.PickupList = objeFleetPassengerTrackingModel.PickupList.Remove(objeFleetPassengerTrackingModel.PickupList.ToString().LastIndexOf(','), 1);
                        objeFleetPassengerTrackingModel.DropList = objeFleetPassengerTrackingModel.DropList.Remove(objeFleetPassengerTrackingModel.DropList.ToString().LastIndexOf(','), 1);

                        var picklist = objeFleetPassengerTrackingModel.PickupList.Split(',').ToList();
                        var droplist = objeFleetPassengerTrackingModel.DropList.Split(',').ToList();

                        //AutoMapper.Mapper.CreateMap<eFleetPassengerTrackingModel, eFleetPassengerTrackingRoute>();
                        if (picklist.Count == droplist.Count)
                        {
                            var listPassengerRoutemodel1 = new List<eFleetPassengerTrackingRoute>();
                            for (int i = 0; i < picklist.Count; i++)
                            {
                                var objfleetPTMapperLoop = new eFleetPassengerTrackingRoute();
                                //AutoMapper.Mapper.CreateMap<eFleetPassengerTrackingModel, eFleetPassengerTrackingRoute>();
                                // objfleetPTMapperLoop = AutoMapper.Mapper.Map(objeFleetPassengerTrackingModel, objeFleetPassengerTrackingRoute);
                                objfleetPTMapperLoop.PickUpPoint = picklist[i];
                                objfleetPTMapperLoop.DropPoint = droplist[i];
                                objfleetPTMapperLoop.CreatedBy = objeFleetPassengerTrackingModel.CreatedBy;
                                objfleetPTMapperLoop.CreatedDate = DateTime.UtcNow;
                                objfleetPTMapperLoop.EndDate = objeFleetPassengerTrackingModel.EndDate;
                                objfleetPTMapperLoop.StartDate = objeFleetPassengerTrackingModel.StartDate;
                                objfleetPTMapperLoop.ServiceType = objeFleetPassengerTrackingModel.ServiceType;
                                objfleetPTMapperLoop.RouteName = objeFleetPassengerTrackingModel.RouteName;
                                // objfleetPTMapperLoop. = objeFleetPassengerTrackingModel.RouteName;

                                listPassengerRoutemodel1.Add(objfleetPTMapperLoop);

                            }
                            using (var context = new workorderEMSEntities())
                            {
                                context.eFleetPassengerTrackingRoutes.AddRange(listPassengerRoutemodel1);
                                context.SaveChanges();
                            }
                            //objeFleetPassengerTrackingRepository.BulkAdd(listPassengerRoutemodel);
                        }
                    }

                    obj = Result.Completed;
                }
                //edit Data
                else
                {
                    var RouteData = objeFleetPassengerTrackingRepository.GetAll(v => v.IsDeleted == false && v.RouteID == objeFleetPassengerTrackingModel.RouteID).SingleOrDefault();
                    RouteData.ModifiedBy = objeFleetPassengerTrackingModel.ModifiedBy;
                    RouteData.ModifiedDate = objeFleetPassengerTrackingModel.ModifiedDate;
                    RouteData.DropPoint = objeFleetPassengerTrackingModel.DropPoint;
                    RouteData.PickUpPoint = objeFleetPassengerTrackingModel.PickUpPoint;
                    RouteData.RouteName = objeFleetPassengerTrackingModel.RouteName;
                    RouteData.ServiceType = objeFleetPassengerTrackingModel.ServiceType;
                    RouteData.StartDate = objeFleetPassengerTrackingModel.StartDate;
                    RouteData.EndDate = objeFleetPassengerTrackingModel.EndDate;
                    objeFleetPassengerTrackingRepository.Update(RouteData);

                    //code to insert new record if user added new route while edit
                    if ((objeFleetPassengerTrackingModel.PickupList != null && objeFleetPassengerTrackingModel.PickupList != "") &&
                       (objeFleetPassengerTrackingModel.DropList != null && objeFleetPassengerTrackingModel.DropList != ""))
                    {
                        objeFleetPassengerTrackingModel.PickupList = objeFleetPassengerTrackingModel.PickupList.Remove(objeFleetPassengerTrackingModel.PickupList.ToString().LastIndexOf(','), 1);
                        objeFleetPassengerTrackingModel.DropList = objeFleetPassengerTrackingModel.DropList.Remove(objeFleetPassengerTrackingModel.DropList.ToString().LastIndexOf(','), 1);

                        var picklist = objeFleetPassengerTrackingModel.PickupList.Split(',').ToList();
                        var droplist = objeFleetPassengerTrackingModel.DropList.Split(',').ToList();
                        picklist.RemoveAt(0); //for no need to first entry to insert as above code already updating
                        droplist.RemoveAt(0);
                        //AutoMapper.Mapper.CreateMap<eFleetPassengerTrackingModel, eFleetPassengerTrackingRoute>();
                        if (picklist.Count == droplist.Count)
                        {
                            var listPassengerRoutemodel11 = new List<eFleetPassengerTrackingRoute>();
                            for (int i = 0; i < picklist.Count; i++)
                            {
                                var objfleetPTMapperLoop = new eFleetPassengerTrackingRoute();
                                //AutoMapper.Mapper.CreateMap<eFleetPassengerTrackingModel, eFleetPassengerTrackingRoute>();
                               // objfleetPTMapperLoop = AutoMapper.Mapper.Map(objeFleetPassengerTrackingModel, objeFleetPassengerTrackingRoute);
                                objfleetPTMapperLoop.PickUpPoint = picklist[i];
                                objfleetPTMapperLoop.DropPoint = droplist[i];
                                objfleetPTMapperLoop.CreatedBy = objeFleetPassengerTrackingModel.ModifiedBy ?? RouteData.CreatedBy;
                                objfleetPTMapperLoop.CreatedDate = DateTime.UtcNow;
                                objfleetPTMapperLoop.EndDate = objeFleetPassengerTrackingModel.EndDate;
                                objfleetPTMapperLoop.StartDate = objeFleetPassengerTrackingModel.StartDate;
                                objfleetPTMapperLoop.ServiceType = objeFleetPassengerTrackingModel.ServiceType;
                                objfleetPTMapperLoop.RouteName = objeFleetPassengerTrackingModel.RouteName;
                               // objfleetPTMapperLoop. = objeFleetPassengerTrackingModel.RouteName;

                                listPassengerRoutemodel11.Add(objfleetPTMapperLoop);

                            }
                            using (var context = new workorderEMSEntities())
                            {
                                context.eFleetPassengerTrackingRoutes.AddRange(listPassengerRoutemodel11);
                                context.SaveChanges();
                            }
                            //objeFleetPassengerTrackingRepository.BulkAdd(listPassengerRoutemodel);
                        }
                    }
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
        /// 
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
        /// s
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

                    //if (Data != null && Data.PassengerCountID > 0)
                    //{
                    //    objPassengerTrackingCountRepository.Update(Data);
                    //    objReturnModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.InvariantCulture);
                    //    objReturnModel.Message = CommonMessage.Successful();
                    //}
                    //else
                    //{
                    //    objReturnModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.InvariantCulture);
                    //    objReturnModel.Message = CommonMessage.NoRecordMessage();
                    //}
                }
                else
                {
                    objReturnModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.InvariantCulture);
                    objReturnModel.Message = CommonMessage.NoRecordMessage();
                }
            }
            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "ServiceResponseModel<string> InsertMaintenance(eFleetMaintenanceModel objModel)", "while insert maintenance", objModel);
                objReturnModel.Message = ex.Message;
                objReturnModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                objReturnModel.Data = null;
            }
            return objReturnModel;
        }

        /// <summary>
        /// Created By: Bhushan Dod 
        /// Created Date: Oct-13-2017
        /// List of Passenger tracking route according to service type in jqgrid list.
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="locationId"></param>
        /// <param name="textSearch"></param>
        /// <param name="statusType"></param>
        /// <returns></returns>
        public JQGridModel<eFleetPassengerTrackingModel> GetListeFleetPassengerRoutewithJQGridDetails(int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, long? statusType)
        {
            try
            {
                workorderEMSEntities db = new workorderEMSEntities();
                var objeFleetPassengerTrackingModel = new JQGridModel<eFleetPassengerTrackingModel>();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                var objeFleetRoute = new eFleetPassengerTrackingModel();
                var Results = db.eFleetPassengerTrackingRoutes.Where(a => (a.IsDeleted == false)
                            && (((statusType == 0) ? null : statusType) == null || a.ServiceType == statusType)).Select(a => new eFleetPassengerTrackingModel()
                            {
                                DropPoint = a.DropPoint,
                                EndDate = a.EndDate,
                                PickUpPoint = a.PickUpPoint,
                                ServiceTypeName = a.GlobalCode.CodeName,
                                StartDate = a.StartDate,
                                RouteID = a.RouteID,
                                RouteName = a.RouteName,
                                CreatedDate = a.CreatedDate
                            }).OrderByDescending(x => x.CreatedDate).ToList();
                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                objeFleetPassengerTrackingModel.pageindex = pageindex;
                objeFleetPassengerTrackingModel.total = totalPages;
                objeFleetPassengerTrackingModel.records = totRecords;
                objeFleetPassengerTrackingModel.rows = Results.ToList();
                return objeFleetPassengerTrackingModel;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "JQGridModel<eFleetPassengerTrackingModel> GetListeFleetPassengerRoutewithJQGridDetails(int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, long? statusType)", "Exception While fetching ", statusType);
                throw;
            }
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
                var db = new workorderEMSEntities();
                var objeFleetPassengerTrackingRepository = new eFleetPassengerTrackingRepository();
                var editeFleetPassengerTrackingDetails = new eFleetPassengerTrackingModel();
                var PassengerTrackingDetails = objeFleetPassengerTrackingRepository.GetSingleOrDefault(u => u.RouteID == RouteId);
                if (PassengerTrackingDetails.RouteID > 0) // PmID in Ashwajit created Table
                {
                    AutoMapper.Mapper.CreateMap<eFleetPassengerTrackingRoute, eFleetPassengerTrackingModel>();
                    //editeFleetPassengerTrackingDetails.RouteID = PassengerTrackingDetails.RouteID;
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
        public Result DeleteeFleetPassengerTracking(long passengerId, long loggedInUserId)
        {
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
                        return Result.Delete;
                    }
                }
                else { return Result.DoesNotExist; }
                return Result.Delete;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public Result DeleteeFleetPM(long VehicleId, long loggedInUserId)", "Exception While Deleting Preventative Maintenence.", null);
                throw;
            }
        }
    }
}
