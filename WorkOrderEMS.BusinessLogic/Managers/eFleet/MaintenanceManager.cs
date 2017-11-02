using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces.eFleet;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel.ApiModel;

namespace WorkOrderEMS.BusinessLogic.Managers.eFleet
{
    public class MaintenanceManager : IEfleetMaintenance
    {
        CommonMethodManager _ICommonMethod = new CommonMethodManager();
        workorderEMSEntities objworkorderEMSEntities = new workorderEMSEntities();
        /// <summary>
        /// Created By Ashwajit Bansod  Dated:Sept-20-2017
        /// for fetching all vehicle number stored in eFleetVehicle
        /// </summary>
        /// <returns></returns>
        public List<eFleetVehicleModel> GetVehicleNumber(long LocationID)
        {
            try
            {
                var objeFleetMaintenanceRepository = new eFleetMaintenanceRepository();
                return objeFleetMaintenanceRepository.GetVehicleNumber(LocationID);
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<eFleetVehicleModel> GetVehicleNumber()", "Exception While Fetching all vehicle Number.", null);
                throw;
            }
        }
        /// <summary>
        /// Created By Ashwajit Bansod Dated : Sept-20-2017
        /// For Getting all Maintenance Type from Global Codes table
        /// </summary>
        /// <returns></returns>
        public List<GlobalCodeModel> GetAllMaintenanceType()
        {
            try
            {
                var objeFleetMaintenanceRepository = new eFleetMaintenanceRepository();
                return objeFleetMaintenanceRepository.GetAllMaintenanceType();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public List<GlobalCodeModel> GetAllMeterList()", "Exception While Fetching all Meter List.", null);
                throw;
            }
        }
        /// <summary>
        /// Created By Ashwajit Bansod Dated : Sept-22-2017
        /// For Fetching Pending Preventative Maintenance Reminder Description 
        /// </summary>
        /// <param name="LocationID"></param>
        /// <returns></returns>
       public List<PendingPM> GetAllPendingPMReminderDescription(long LocationID)
        {         
                List<PendingPM> lstReminderDescription = new List<PendingPM>();
                try
                {
                    lstReminderDescription = objworkorderEMSEntities.eFleetPreventativeMaintenances.Where(a => a.IsCompleted == null && a.LocationID == LocationID && a.IsDeleted == false).Select(s => new PendingPM()
                    {
                        PmID = s.ID,
                        ReminderMetricDesc = s.ReminderMetricDesc

                    }).ToList();
                    return lstReminderDescription;
                }           
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public List<eFleetPMModel> GetAllReminderDescription()", "Exception While Fetching all Reminder Metric Description.", null);
                throw;
            }
        }
        /// <summary>
        /// Created By Ashwajit Bansod Dated : Sept-22-2017
        /// For Saving and editing Maintenance Report
        /// </summary>
        /// <param name="objeFleetMaintenanceModel"></param>
        /// <returns></returns>
        public eFleetMaintenanceModel SaveEfleetMaintenance(eFleetMaintenanceModel objeFleetMaintenanceModel)
        {
            try
            {
                LocationMaster objLocationMaster = new LocationMaster();
                var objeFleetMaintenance = new eFleetMaintenance();
                var objeFleetMaintenanceRepository = new eFleetMaintenanceRepository();
                var objeTracLoginModel = new eTracLoginModel();

                

                if (objeFleetMaintenanceModel.MaintenanceID == 0)
                {
                    AutoMapper.Mapper.CreateMap<eFleetMaintenanceModel, eFleetMaintenance>();
                    var objfleetMaintenanceMapper = AutoMapper.Mapper.Map(objeFleetMaintenanceModel, objeFleetMaintenance);
                    objeFleetMaintenanceRepository.Add(objfleetMaintenanceMapper);
                    //objeFleetDriver.QRCCodeID = objeFleetMaintenanceModel.QRCCodeID + "EFD" + (objeFleetDriver.DriverID + 100).ToString();
                    objeFleetMaintenanceRepository.SaveChanges();
                    objeFleetMaintenanceModel.Result = Result.Completed;
                    if (objeFleetMaintenanceModel.Result == Result.Completed)
                    {
                        #region Save DAR
                        DARModel objDAR = new DARModel();
                        objDAR.ActivityDetails = DarMessage.RegisterNeweFleetMaintenance(objeTracLoginModel.LocationNames);
                        objDAR.LocationId = objeFleetMaintenanceModel.LocationID;
                        objDAR.UserId = objeFleetMaintenanceModel.UserID;
                        objDAR.CreatedBy = objeFleetMaintenanceModel.UserID;
                        objDAR.CreatedOn = DateTime.UtcNow;
                        objDAR.TaskType = (long)TaskTypeCategory.eFleetDriverSubmission;
                        Result result = _ICommonMethod.SaveDAR(objDAR);
                        #endregion Save DAR
                    }
                }
                //edit Data
                else
                {
                    var MaintenanceData = objeFleetMaintenanceRepository.GetAll(v => v.IsDeleted == false && v.MaintenanceID == objeFleetMaintenanceModel.MaintenanceID && v.LocationID == objeFleetMaintenanceModel.LocationID).SingleOrDefault();
                    //objeFleetDriverModel.QRCCodeID = MaintenanceData.QRCCodeID;
                    //objeFleetDriverModel.DriverImage = MaintenanceData.DriverImage;//== null ? "" : HostingPrefix + ProfilePicPath.Replace("~", "") + DriverData.DriverImage;
                    AutoMapper.Mapper.CreateMap<eFleetMaintenanceModel, eFleetMaintenance>();
                    var objfleetDriverMapper = AutoMapper.Mapper.Map(objeFleetMaintenanceModel, MaintenanceData);
                    //objeFleetDriverModel.Passwordforedit = DriverData.Password;
                    objeFleetMaintenanceRepository.SaveChanges();
                    objeFleetMaintenanceModel.Result = Result.UpdatedSuccessfully;

                    if (objeFleetMaintenanceModel.Result == Result.UpdatedSuccessfully)
                    {
                        #region Save DAR
                        DARModel objDAR = new DARModel();
                        objDAR.ActivityDetails = DarMessage.RegisterNeweFleetMaintenance(objeFleetMaintenanceModel.LocationName);
                        objDAR.LocationId = objeFleetMaintenanceModel.LocationID;
                        objDAR.UserId = objeFleetMaintenanceModel.UserID;
                        objDAR.ModifiedBy = objeFleetMaintenanceModel.UserID;
                        objDAR.ModifiedOn = DateTime.UtcNow;
                        objDAR.TaskType = (long)TaskTypeCategory.UpdateeFleetMaintenance;
                        Result result = _ICommonMethod.SaveDAR(objDAR);
                        #endregion Save DAR
                    }
                }
                return objeFleetMaintenanceModel;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public eFleetMaintenanceModel SaveEfleetMaintenance(eFleetMaintenanceModel objeFleetMaintenanceModel)", "Exception While saving Maintenance request.", objeFleetMaintenanceModel);
                throw;
            }
        }
        /// <summary>
        /// Created By Ashwajit Bansod Dated : Sept-22-2017
        /// For Creating JQ Grid List 
        /// </summary>
        /// <param name="objeFleetVehicleList"></param>
        /// <returns></returns>
        public eFleetMaintenanceModel GetAllMaintenanceList(eFleetMaintenanceModel objeFleetMaintenanceModelList)
        {
            return objeFleetMaintenanceModelList;
        }
        /// <summary>
        /// Created By Ashwajit Bansod : Dated Sept-22-2017
        /// For Fetching all required data from database and Display in JQ Grid Listing
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
        public eDetailsMaintenance GetListeFleetMaintenanceDetails(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)
        {
            try
            {
                workorderEMSEntities db = new workorderEMSEntities();
                var objeDetailsMaintenance = new eDetailsMaintenance();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                var objeFleetMaintenanceModel = new eFleetMaintenanceModel();
                var Results = db.eFleetMaintenances.Where(a => a.IsDeleted == false && a.LocationID == locationId).Select(a => new eFleetMaintenanceModel()
                {
                    PmID = a.PmID,
                    VehicleID = a.VehicleID,
                    MaintenanceID = a.MaintenanceID,
                    //QRCodeID = a.QRCodeID,
                    VehicleNumber = a.VehicleNumber,
                    MaintenanceTypeList = a.GlobalCode.CodeName,
                    DriverName = a.DriverName,
                    //ListReminderMetric = (from em in db.eFleetMeters where em.ID == a.ReminderMetric select em.MeterValue).FirstOrDefault(),//db.eFleetMeters.Where(rm => rm.ID == a.ReminderMetric).FirstOrDefault().MeterValue,                                                                                                                                       // (from rm in db.eFleetMeters where rm.ID == a.ReminderMetric select rm.MeterValue).FirstOrDefault(),
                    //ScheduledPM = a.ScheduledPM,
                    // Category = a.GlobalCode1.GlobalCodeId,      //(from gc in db.GlobalCodes where gc.GlobalCodeId == a.Category select gc.CodeName).FirstOrDefault(),
                    DaysOutOfService = a.DaysOutOfService,
                    ReminderMetricDesc = a.ReminderMetricDesc,
                    MaintenanceDate = a.MaintenanceDate,
                    TotalCost = a.TotalCost,
                    Miles = a.Miles,
                    Note = a.Note,
                    LabourCost = a.LabourCost,
                    PartsCost = a.PartsCost,
                });
                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                Results = Results.OrderByDescending(s => s.MaintenanceID);
                objeDetailsMaintenance.pageindex = pageindex;
                objeDetailsMaintenance.total = totalPages;
                objeDetailsMaintenance.records = totRecords;
                objeDetailsMaintenance.rows = Results.ToList();
                return objeDetailsMaintenance;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "GetListeFleetMaintenanceDetails(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)", "Exception While Listing Maintenence.", null);
                throw;
            }
        }
        /// <summary>
        /// Created By Ashwajit Bansod Dated : Sept-22-2017
        /// for fetching the Data from Database for editing
        /// </summary>
        /// <param name="maintenanceId"></param>
        /// <returns></returns>
        public eFleetMaintenanceModel GeteFleetMaintenanceDetailsById(long maintenanceId)
        {
            try
            {
                var db = new workorderEMSEntities();
                var ObjeFleetMaintenanceRepository = new eFleetMaintenanceRepository();
                var editeFleetMaintenanceDetails = new eFleetMaintenanceModel();
                var objeFleetPreventativeMaintenance = new eFleetPreventativeMaintenance();
                var efleetmaintenanceDetails = ObjeFleetMaintenanceRepository.GetSingleOrDefault(u => u.MaintenanceID == maintenanceId);
                if (efleetmaintenanceDetails.MaintenanceID > 0)
                {
                    AutoMapper.Mapper.CreateMap<eFleetMaintenance, eFleetMaintenanceModel>();
                    var objfleetVehicleMapper = AutoMapper.Mapper.Map(efleetmaintenanceDetails, editeFleetMaintenanceDetails);
                    //if (editeFleetPMDetails.Meter == 423)
                    //{
                    //    editeFleetPMDetails.HoursValue = efleetDetails.ReminderMetric;
                    //}
                }
                return editeFleetMaintenanceDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public eFleetMaintenanceModel GeteFleetPMDetailsById(long maintenanceId)", "Exception While Editing Maintenence.", null);
                throw;
            }
        }
        /// <summary>
        /// Created By Ashwajit Bansod Dated : Sept-22-2017
        /// </summary>
        /// <param name="VehicleId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public Result DeleteeFleetMaintenance(long maintenanceId, long loggedInUserId, string location)
        {
            var objDAR = new DARModel();
            try
            {
                Result result;
                if (maintenanceId > 0)
                {
                    if (true)
                    {
                        var objeFleetMaintenanceRepository = new eFleetMaintenanceRepository();
                        var data = objeFleetMaintenanceRepository.GetSingleOrDefault(v => v.MaintenanceID == maintenanceId && v.IsDeleted == false);
                        if (data != null)
                        {
                            data.IsDeleted = true;
                            data.DeletedBy = loggedInUserId;
                            data.DeletedDate = DateTime.UtcNow;
                            objeFleetMaintenanceRepository.Update(data);
                            objeFleetMaintenanceRepository.SaveChanges();

                            objDAR.ActivityDetails = DarMessage.DeleteFleetMaintenance(location);
                            objDAR.TaskType = (long)TaskTypeCategory.DeleteeFleetMaintenance;

                            #region Save DAR
                            objDAR.LocationId = data.LocationID;
                            objDAR.UserId = loggedInUserId;
                            objDAR.DeletedBy = data.DeletedBy;
                            objDAR.DeletedOn = DateTime.UtcNow;
                            result = _ICommonMethod.SaveDAR(objDAR);
                            #endregion Save DAR
                            return Result.Delete;
                        }
                    }
                    else
                    { return Result.Failed; }
                }
                else { return Result.DoesNotExist; }
                return Result.Delete;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public Result DeleteeFleetMaintenance(long maintenanceId, long loggedInUserId)", "Exception While Deleting Preventative Maintenence.", null);
                throw;
            }
        }
        /// <summary>Save eFleet  Maintenance
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedFor>SaveeFleetMaintenance</CreatedFor>
        /// <CreatedOn>September-20-2017</CreatedOn>
        /// </summary>
        /// <param name="objModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> InsertMaintenance(eFleetMaintenanceModelForApiService objModel)
        {
            var objReturnModel = new ServiceResponseModel<string>();
            try
            {
                var objeFleetMaintenanceRepository = new eFleetMaintenanceRepository();
                eFleetMaintenance Obj = new eFleetMaintenance();
                AutoMapper.Mapper.CreateMap<eFleetMaintenanceModelForApiService, eFleetMaintenance>();
                Obj = AutoMapper.Mapper.Map(objModel, Obj);
                Obj.CreatedBy = objModel.UserId;
                Obj.CreatedDate = DateTime.UtcNow;
                objeFleetMaintenanceRepository.Add(Obj);
                if (Obj.MaintenanceID > 0)
                {
                    if (Obj.MaintenanceType == 445 && objModel.PmID != null && objModel.PmID > 0)
                    {
                        var objeFleetPreventativeMaintenanceRepository = new eFleetPreventativeMaintenanceRepository();
                        var pmData = objeFleetPreventativeMaintenanceRepository.GetAll(pm => pm.ID == objModel.PmID && pm.LocationID == objModel.LocationID && pm.IsDeleted == false).FirstOrDefault();
                        if (pmData != null && pmData.ID > 0)
                        {
                            pmData.IsCompleted = true;
                            pmData.CompletedBy = objModel.UserId;
                            pmData.CompletedOn = DateTime.UtcNow;

                            objeFleetPreventativeMaintenanceRepository.Update(pmData);
                            objReturnModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.InvariantCulture);
                            objReturnModel.Message = CommonMessage.Successful();
                        }
                        else
                        {
                            objReturnModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.InvariantCulture);
                            objReturnModel.Message = CommonMessage.NoRecordMessage();
                        }
                    }
                    else
                    {
                        objReturnModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.InvariantCulture);
                        objReturnModel.Message = CommonMessage.Successful();
                    }
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

    }
}
