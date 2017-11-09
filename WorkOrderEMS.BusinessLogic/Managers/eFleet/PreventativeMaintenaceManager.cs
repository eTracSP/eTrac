using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Data;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.BusinessLogic.Managers.eFleet
{
    public class PreventativeMaintenaceManager : IEfleetPM
    {
        IPreventativeMaintenanceRepository _IPreventativeMaintenanceRepository;
        CommonMethodManager _ICommonMethod = new CommonMethodManager();
        /// <summary>
        /// Created By Ashwajit Bansod Dated 08/29/2017
        /// Get all vehicle number from eFleetVehicle
        /// </summary>
        /// <returns></returns>
        public List<eFleetVehicleModel> GetAllVehicleNumber()
        {
            try
            {
                var objeFleetPreventativeMaintenanceRepository = new eFleetPreventativeMaintenanceRepository();
                return objeFleetPreventativeMaintenanceRepository.GetVehicleNumber();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<eFleetVehicleModel> GetVehicleNumber()", "Exception While Fetching all vehicle Number.", null);
                throw;
            }
        }

        /// <summary>Save eFleet Preventative Maintenance
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedFor>SaveeFleetPreventativeMaintenance</CreatedFor>
        /// <CreatedOn>August-29-2017</CreatedOn>
        /// </summary>
        /// <param name="eFleetDamageTireModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> InsertPreventativeMaintenance(eFleetPreventaticeMaintenanceModel objModel)
        {
            var objReturnModel = new ServiceResponseModel<string>();
            try
            {
                var objeFleetPreventativeMaintenanceRepository = new eFleetPreventativeMaintenanceRepository();
                eFleetPreventativeMaintenance Obj = new eFleetPreventativeMaintenance();
                AutoMapper.Mapper.CreateMap<eFleetPreventaticeMaintenanceModel, eFleetPreventativeMaintenance>();
                Obj = AutoMapper.Mapper.Map(objModel, Obj);
                Obj.CreatedBy = objModel.UserId;
                Obj.CreatedDate = DateTime.UtcNow;
                objeFleetPreventativeMaintenanceRepository.Add(Obj);
                if (Obj.ID > 0)
                {
                    #region Save DAR
                    DARModel objDAR = new DARModel();
                    objDAR.ActivityDetails = DarMessage.NeweFleetPMCreated(objModel.LocationName);
                    objDAR.LocationId = objModel.LocationID;
                    objDAR.UserId = objModel.UserId;
                    objDAR.CreatedBy = objModel.UserId;
                    objDAR.CreatedOn = DateTime.UtcNow;
                    objDAR.TaskType = (long)TaskTypeCategory.PreventativeMaintenanceSubmission;
                    Result result = _ICommonMethod.SaveDAR(objDAR);
                    #endregion Save DAR

                    #region Email
                    var objEmailLogRepository = new EmailLogRepository();
                    var objEmailReturn = new List<EmailToManagerModel>();
                    var objListEmailog = new List<EmailLog>();
                    var objTemplateModel = new TemplateModel();
                    if (result == Result.Completed)
                    {
                        objEmailReturn = objEmailLogRepository.SendEmailToManagerForeFleetInspection(objModel.LocationID, objModel.UserId).Result;
                    }

                    if (objEmailReturn.Count > 0 && result == Result.Completed)
                    {
                        foreach (var item in objEmailReturn)
                        {
                            bool IsSent = false;
                            var objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = item.ManagerEmail;
                            objEmailHelper.ManagerName = item.ManagerName;
                            objEmailHelper.RemiderMetric = Convert.ToString(objModel.ReminderMetric);
                            objEmailHelper.Meter = Convert.ToString(objModel.Meter);
                            objEmailHelper.VehicleNumber = objModel.VehicleNumber;
                            objEmailHelper.LocationName = objModel.LocationName;
                            objEmailHelper.UserName = item.UserName;
                            objEmailHelper.QrCodeId = objModel.QrCodeId;
                            objEmailHelper.ServiceDueDate = objModel.ServiceDueDate.ToString();
                            //objEmailHelper.InfractionStatus = obj.Status;
                            objEmailHelper.MailType = "PreventativeMaintenance";
                            objEmailHelper.SentBy = item.RequestBy;
                            objEmailHelper.LocationID = item.LocationID;
                            objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();

                            IsSent = objEmailHelper.SendEmailWithTemplate();

                            //Push Notification
                            string message = PushNotificationMessages.eFleetPreventativeMaintenanceReported(objModel.LocationName, objModel.QrCodeId, objModel.VehicleNumber);
                            PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
                            if (IsSent == true)
                            {
                                var objEmailog = new EmailLog();
                                try
                                {
                                    objEmailog.CreatedBy = item.RequestBy;
                                    objEmailog.CreatedDate = DateTime.UtcNow;
                                    objEmailog.DeletedBy = null;
                                    objEmailog.DeletedOn = null;
                                    objEmailog.LocationId = item.LocationID;
                                    objEmailog.ModifiedBy = null;
                                    objEmailog.ModifiedOn = null;
                                    objEmailog.SentBy = item.RequestBy;
                                    objEmailog.SentEmail = item.ManagerEmail;
                                    objEmailog.Subject = objEmailHelper.Subject;
                                    objEmailog.SentTo = item.ManagerUserId;
                                    objListEmailog.Add(objEmailog);
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }

                        }
                        using (var context = new workorderEMSEntities())
                        {
                            context.EmailLogs.AddRange(objListEmailog);
                            context.SaveChanges(); ;
                        }
                        //    //var x = EmailLogRepository.InsertEntitiesNew("EmailLog", objListEmailog);
                        //    //Task<bool> x = null;
                        //    //foreach (var i in objListEmailog)
                        //    //{
                        //    //    x = objEmailLogRepository.SaveEmailLogAsync(i);
                        //    //}
                        //}


                        #endregion Email
                        objReturnModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.InvariantCulture);
                        objReturnModel.Message = CommonMessage.Successful();
                    }
                }
            }


            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "ServiceResponseModel<string> InsertPreventativeMaintenance(eFleetPreventaticeMaintenanceModel objModel)", "while insert preventative maintenance", objModel);
                objReturnModel.Message = ex.Message;
                objReturnModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                objReturnModel.Data = null;
            }
            return objReturnModel;
        }


        /// <summary>
        /// Created by Ashwajit Bansod Dated 08/29/2017
        /// Get all Miles Value from eFleetMeter
        /// </summary>
        /// <returns></returns>
        public List<eFleetMeterModel> GetAllMilesValue()
        {
            try
            {
                var objeFleetPreventativeMaintenanceRepository = new eFleetPreventativeMaintenanceRepository();
                return objeFleetPreventativeMaintenanceRepository.GetAllMeterValue();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<eFleetMeterModel> GetAllMeterValue()", "Exception While Fetching all Meter Values.", null);
                throw;
            }
        }

        /// <summary>
        /// Created by Ashwajit Bansod Dated 08/29/2017
        /// Get all Meter List from Global codes
        /// </summary>
        /// <returns></returns>
        public List<GlobalCodeModel> GetAllMeterList()
        {
            try
            {
                var objeFleetPreventativeMaintenanceRepository = new eFleetPreventativeMaintenanceRepository();
                return objeFleetPreventativeMaintenanceRepository.GetAllMeterList();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public List<GlobalCodeModel> GetAllMeterList()", "Exception While Fetching all Meter List.", null);
                throw;
            }
        }
        /// <summary>
        /// Created By Ashwajit for Fetching all the Category Values From Database
        /// </summary>
        /// <returns></returns>
        public List<GlobalCodeModelDDL> GetAllCategory()
        {
            try
            {
                var objeFleetPreventativeMaintenanceRepository = new eFleetPreventativeMaintenanceRepository();
                return objeFleetPreventativeMaintenanceRepository.GetAllCategory();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<GlobalCodeModel> GetAllCategory()", "Exception While Fetching all Category request.", null);
                throw;
            }
        }
        /// <summary>
        /// Created By Ashwajit Bansod Dated 08/29/2017
        /// Save and edit Preventative maintenance Data
        /// </summary>
        /// <param name="objeFleetPMModel"></param>
        /// <returns></returns>
        public eFleetPMModel SaveEfleetPreventativeMaintenance(eFleetPMModel objeFleetPMModel)
        {
            try
            {
                workorderEMSEntities db = new workorderEMSEntities();
                var objLocationMaster = new LocationMaster();
                var objeFleetPreventativeMaintenance = new eFleetPreventativeMaintenance();
                var objeFleetVehicleModel = new eFleetVehicleModel();
                var meterval = Convert.ToInt64(eFleetEnum.Hours);
                var objeFleetPreventativeMaintenanceRepository = new eFleetPreventativeMaintenanceRepository();
                var objeTracLoginModel = new eTracLoginModel();
                if (objeFleetPMModel.ID == 0)
                {
                    AutoMapper.Mapper.CreateMap<eFleetPMModel, eFleetPreventativeMaintenance>();
                    if (objeFleetPMModel.Meter == meterval)
                    {
                        objeFleetPMModel.ReminderMetric = objeFleetPMModel.HoursValue;
                    }
                    var objfleetPMMapper = AutoMapper.Mapper.Map(objeFleetPMModel, objeFleetPreventativeMaintenance);
                    objeFleetPreventativeMaintenanceRepository.Add(objfleetPMMapper);
                    objeFleetPreventativeMaintenanceRepository.SaveChanges();
                    objeFleetPMModel.Result = Result.Completed;
                    if (objeFleetPMModel.Result == Result.Completed)
                    {
                        #region Save DAR
                        DARModel objDAR = new DARModel();
                        objDAR.ActivityDetails = DarMessage.NeweFleetPMCreated(objeFleetPMModel.LocationName);
                        objDAR.LocationId = objeFleetPMModel.LocationID;
                        objDAR.UserId = objeFleetPMModel.UserId;
                        objDAR.CreatedBy = objeFleetPMModel.UserId;
                        objDAR.CreatedOn = DateTime.UtcNow;
                        objDAR.TaskType = (long)TaskTypeCategory.PreventativeMaintenanceSubmission;
                        Result result = _ICommonMethod.SaveDAR(objDAR);
                        #endregion Save DAR

                        //Created By Ashwajit Bansod Date: Oct-04-2017 for sending a mail regarding Prevenatative maintenance to the manager
                        #region Email
                        var objEmailLogRepository = new EmailLogRepository();
                        var objEmailReturn = new List<EmailToManagerModel>();
                        var objListEmailog = new List<EmailLog>();
                        var objTemplateModel = new TemplateModel();
                        if (result == Result.Completed)
                        {
                            objEmailReturn = objEmailLogRepository.SendEmailToManagerForeFleetInspection(objeFleetPMModel.LocationID, objeFleetPMModel.UserId).Result;
                        }

                        if (objEmailReturn.Count > 0 && result == Result.Completed)
                        {
                            foreach (var item in objEmailReturn)
                            {
                                bool IsSent = false;
                                var objEmailHelper = new EmailHelper();
                                objEmailHelper.emailid = item.ManagerEmail;
                                objEmailHelper.ManagerName = item.ManagerName;
                                if (objeFleetPMModel.ReminderMetric == null)
                                {
                                    objEmailHelper.RemiderMetric = "N/A";
                                }
                                else
                                {
                                    objEmailHelper.RemiderMetric = (from em in db.eFleetMeters where em.ID == objeFleetPMModel.ReminderMetric select em.MeterValue).FirstOrDefault();

                                }
                                objEmailHelper.Meter = (from gc in db.GlobalCodes where gc.GlobalCodeId == objeFleetPMModel.Meter select gc.CodeName).FirstOrDefault();
                                objEmailHelper.Category = (from gc in db.GlobalCodes where gc.GlobalCodeId == objeFleetPMModel.Category select gc.CodeName).FirstOrDefault(); ;
                                objEmailHelper.VehicleNumber = objeFleetPMModel.VehicleNumber;
                                objEmailHelper.LocationName = objeFleetPMModel.LocationName;
                                objEmailHelper.UserName = item.UserName;
                                objEmailHelper.QrCodeId = objeFleetPMModel.QRCodeID;
                                objEmailHelper.ServiceDueDate = objeFleetPMModel.ServiceDueDate.ToString();
                                objEmailHelper.PMMetric = objeFleetPMModel.ReminderMetricDesc;
                                //objEmailHelper.InfractionStatus = obj.Status;
                                objEmailHelper.MailType = "PreventativeMaintenance";
                                objEmailHelper.SentBy = item.RequestBy;
                                objEmailHelper.LocationID = item.LocationID;
                                objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                                IsSent = objEmailHelper.SendEmailWithTemplate();
                                //Push Notification
                                string message = PushNotificationMessages.eFleetPreventativeMaintenanceReported(objeFleetPMModel.LocationName, objeFleetPMModel.QRCodeID, objeFleetPMModel.VehicleNumber);
                                PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
                                if (IsSent == true)
                                {
                                    var objEmailog = new EmailLog();
                                    try
                                    {
                                        objEmailog.CreatedBy = item.RequestBy;
                                        objEmailog.CreatedDate = DateTime.UtcNow;
                                        objEmailog.DeletedBy = null;
                                        objEmailog.DeletedOn = null;
                                        objEmailog.LocationId = item.LocationID;
                                        objEmailog.ModifiedBy = null;
                                        objEmailog.ModifiedOn = null;
                                        objEmailog.SentBy = item.RequestBy;
                                        objEmailog.SentEmail = item.ManagerEmail;
                                        objEmailog.Subject = objEmailHelper.Subject;
                                        objEmailog.SentTo = item.ManagerUserId;
                                        objListEmailog.Add(objEmailog);
                                    }
                                    catch (Exception)
                                    {
                                        throw;
                                    }
                                }
                            }
                            using (var context = new workorderEMSEntities())
                            {
                                context.EmailLogs.AddRange(objListEmailog);
                                context.SaveChanges(); ;
                            }
                            //    //var x = EmailLogRepository.InsertEntitiesNew("EmailLog", objListEmailog);
                            //    //Task<bool> x = null;
                            //    //foreach (var i in objListEmailog)
                            //    //{
                            //    //    x = objEmailLogRepository.SaveEmailLogAsync(i);
                            //    //}
                            //}
                            #endregion Email
                        }
                    }
                }
                //edit Data
                else
                {
                    var PreventativeData = objeFleetPreventativeMaintenanceRepository.GetAll(v => v.IsDeleted == false && v.ID == objeFleetPMModel.PmID).SingleOrDefault(); //PmID = ID PmID in Ashwajit Table
                                                                                                                                                                            //objeFleetPMModel.DriverImage = DriverData.DriverImage;//== null ? "" : HostingPrefix + ProfilePicPath.Replace("~", "") + DriverData.DriverImage;
                    AutoMapper.Mapper.CreateMap<eFleetPMModel, eFleetPreventativeMaintenance>();
                    if (objeFleetPMModel.Meter == meterval)
                    {
                        objeFleetPMModel.ReminderMetric = objeFleetPMModel.HoursValue;
                    }
                    var objfleetDriverMapper = AutoMapper.Mapper.Map(objeFleetPMModel, PreventativeData);
                    objeFleetPreventativeMaintenanceRepository.SaveChanges();
                    objeFleetPMModel.Result = Result.UpdatedSuccessfully;
                    if (objeFleetPMModel.Result == Result.UpdatedSuccessfully)
                    {
                        #region Save DAR
                        DARModel objDAR = new DARModel();
                        objDAR.ActivityDetails = DarMessage.NeweFleetPMUpdated(objeFleetPMModel.LocationName);
                        objDAR.LocationId = objeFleetPMModel.LocationID;
                        objDAR.UserId = objeFleetPMModel.UserId;
                        objDAR.ModifiedBy = objeFleetPMModel.UserId;
                        objDAR.ModifiedOn = DateTime.UtcNow;
                        objDAR.TaskType = (long)TaskTypeCategory.UpdatePreventativeMaintenance;
                        Result result = _ICommonMethod.SaveDAR(objDAR);
                        #endregion Save DAR
                    }
                }
                
                return objeFleetPMModel;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public eFleetPMModel SaveEfleetPreventativeMaintenance(eFleetPMModel objeFleetPMModel)", "Exception While saving Preventative Maintenence request.", objeFleetPMModel);
                throw;
            }
        }
        /// <summary>
        /// Created By Ashwajit Bansod Dated:08/31/2017
        /// eFleet Preventative Maintanance List Display in a Grid form
        /// </summary>
        /// <param name="objeFleetPMModelList"></param>
        /// <returns></returns>
        public eFleetPMModel GetAlleFleetPMList(eFleetPMModel objeFleetPMModelList)
        {
            return objeFleetPMModelList;
        }
        /// <summary>
        /// Get all Vehicle Data from Database in a List Form
        /// created By Ashwajit Bansod
        /// Date : 08/12/2017
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public eDetailsPM GetListeFleetPMDetails(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)
        {
            try
            {
                workorderEMSEntities db = new workorderEMSEntities();
                var objeDetailsPM = new eDetailsPM();
                var objeFleetMeter = new eFleetMeter();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                eFleetPMModel objeFleetPMModel = new eFleetPMModel();
                var Results = db.eFleetPreventativeMaintenances.Where(a => a.IsDeleted == false).Select(a => new eFleetPMModel()
                {
                    PmID = a.ID,//PmID in Ashwajit Tale of PM
                    VehicleID = a.VehicleID,
                    QRCodeID = a.QRCodeID,
                    VehicleNumber = a.VehicleNumber,
                    ListMeter = a.GlobalCode.CodeName,
                    HoursValue = a.ReminderMetric,
                    ListReminderMetric = (from em in db.eFleetMeters where em.ID == a.ReminderMetric select em.MeterValue).FirstOrDefault(),//db.eFleetMeters.Where(rm => rm.ID == a.ReminderMetric).FirstOrDefault().MeterValue,                                                                                                                                       // (from rm in db.eFleetMeters where rm.ID == a.ReminderMetric select rm.MeterValue).FirstOrDefault(),
                    PMCategoryList = a.GlobalCode1.CodeName,
                    // Category = a.GlobalCode1.GlobalCodeId,      //(from gc in db.GlobalCodes where gc.GlobalCodeId == a.Category select gc.CodeName).FirstOrDefault(),
                    OtherMilesComment = a.OtherMilesComment,
                    ReminderMetricDesc = a.ReminderMetricDesc,
                    ServiceDueDate = a.ServiceDueDate
                });
                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                Results = Results.OrderByDescending(s => s.PmID);
                objeDetailsPM.pageindex = pageindex;
                objeDetailsPM.total = totalPages;
                objeDetailsPM.records = totRecords;
                objeDetailsPM.rows = Results.ToList();
                return objeDetailsPM;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "GetListeFleetPMDetails(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)", "Exception While Listing Preventative Maintenence.", null);
                throw;
            }
        }
        /// <summary>
        /// Created By ashwajit Bansod for edit vehicle by VehicleID
        /// </summary>
        /// <param name="VehicleID"></param>
        /// <returns></returns>
        public eFleetPMModel GeteFleetPMDetailsById(long pmId)
        {
            try
            {
                var db = new workorderEMSEntities();
                var ObjeFleetPreventativeMaintenanceRepository = new eFleetPreventativeMaintenanceRepository();
                var editeFleetPMDetails = new eFleetPMModel();
                var meterval = Convert.ToInt64(eFleetEnum.Hours);
                var objeFleetPreventativeMaintenance = new eFleetPreventativeMaintenance();
                var efleetDetails = ObjeFleetPreventativeMaintenanceRepository.GetSingleOrDefault(u => u.ID == pmId);
                if (efleetDetails.ID > 0) // PmID in Ashwajit created Table
                {
                    AutoMapper.Mapper.CreateMap<eFleetPreventativeMaintenance, eFleetPMModel>();
                    editeFleetPMDetails.PmID = efleetDetails.ID;
                    var objfleetVehicleMapper = AutoMapper.Mapper.Map(efleetDetails, editeFleetPMDetails);
                    if (editeFleetPMDetails.Meter == meterval)
                    {
                        editeFleetPMDetails.HoursValue = efleetDetails.ReminderMetric;
                    }
                }
                return editeFleetPMDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public eFleetPMModel GeteFleetPMDetailsById(long pmId)", "Exception While Editing Preventative Maintenence.", null);
                throw;
            }
        }
        /// <summary>
        /// created by Ashwajit Bansod dated : 09/01/2017
        /// For Deleting eFleet Preventative Maintenance
        /// </summary>
        /// <param name="VehicleId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public Result DeleteeFleetPM(long VehicleId, long loggedInUserId, string location)
        {
            var objDAR = new DARModel();
            try
            {
                Result result;
                if (VehicleId > 0)
                {
                    var objeFleetPreventativeMaintenanceRepository = new eFleetPreventativeMaintenanceRepository();
                    var data = objeFleetPreventativeMaintenanceRepository.GetSingleOrDefault(v => v.ID == VehicleId && v.IsDeleted == false); // PmID in Ashwajit Created Table
                    if (data != null)
                    {
                        data.IsDeleted = true;
                        data.DeletedBy = loggedInUserId;
                        data.DeletedDate = DateTime.UtcNow;
                        objeFleetPreventativeMaintenanceRepository.Update(data);

                        objeFleetPreventativeMaintenanceRepository.SaveChanges();

                        objDAR.ActivityDetails = DarMessage.DeleteFleetPM(location);
                        objDAR.TaskType = (long)TaskTypeCategory.DeletePreventativeMaintenance;

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

                else { return Result.DoesNotExist; }
                return Result.Delete;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public Result DeleteeFleetPM(long VehicleId, long loggedInUserId)", "Exception While Deleting Preventative Maintenence.", null);
                throw;
            }
        }

        public List<PendingPM> GetAllPendingPMReminderDescription(long LocationID, long VehicleID)
        {
            var lstReminderDescription = new List<PendingPM>();
            try
            {
                var db = new workorderEMSEntities();
                var todaysDate = DateTime.UtcNow.Date;
                lstReminderDescription = db.eFleetPreventativeMaintenances.Where(a => a.IsCompleted == null && a.LocationID == LocationID && a.IsDeleted == false
                                                                                                        && a.ServiceDueDate <= todaysDate && a.VehicleID == VehicleID).Select(s => new PendingPM()
                                                                                                        {
                                                                                                            PmID = s.ID,
                                                                                                            ReminderMetricDesc = s.ReminderMetricDesc,
                                                                                                            ServiceDueDate = s.ServiceDueDate
                                                                                                        }).ToList();
                return lstReminderDescription;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<PendingPM> GetAllPendingPMReminderDescription(long LocationID, long VehicleID)", "Exception While Fetching all Reminder Metric Description for service.", null);
                throw;
            }
        }
    }
}