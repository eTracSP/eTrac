using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.BusinessLogic.Managers.eFleet
{
    public class VehicleIncidentManager : IEfleetVehicleIncidentReport
    {
        CommonMethodManager _ICommonMethod = new CommonMethodManager();
        //private readonly IEfleetVehicleIncidentReport _IEfleetVehicleIncidentReport;
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string IncidentImagePath = ConfigurationManager.AppSettings["IncidentImage"];
        public eFleetVehicleIncidentModel SaveEfleetVehicleIncident(eFleetVehicleIncidentModel objeFleetVehicleIncidentModel)
        {
            try
            {
                LocationMaster objLocationMaster = new LocationMaster();
                var objeFleetVehicleIncident = new eFleetVehicleIncident();
                var objeFleetVehicleIncidentRepository = new eFleetVehicleIncidentRepository();
                var objeTracLoginModel = new eTracLoginModel();
              
                if (objeFleetVehicleIncidentModel.IncidentID == 0)
                {
                    AutoMapper.Mapper.CreateMap<eFleetVehicleIncidentModel, eFleetVehicleIncident>();
                    var objfleetVehicleIncidentMapper = AutoMapper.Mapper.Map(objeFleetVehicleIncidentModel, objeFleetVehicleIncident);
                    objeFleetVehicleIncidentRepository.Add(objfleetVehicleIncidentMapper);
                    // objeFleetVehicle.QRCodeID = objeFleetVehicleModel.QRCodeID + "EFV" + (objeFleetVehicle.VehicleID + 100).ToString();
                    objeFleetVehicleIncidentRepository.SaveChanges();
                    objeFleetVehicleIncidentModel.Result = Result.Completed;
                    if (objeFleetVehicleIncidentModel.Result == Result.Completed)
                    {
                        #region Save DAR
                        DARModel objDAR = new DARModel();
                        objDAR.ActivityDetails = DarMessage.RegisterNeweFleetIncidentVehicle(objeFleetVehicleIncidentModel.LocationName);
                        objDAR.LocationId = objeFleetVehicleIncidentModel.LocationID;
                        objDAR.UserId = objeFleetVehicleIncidentModel.UserID;
                        objDAR.CreatedBy = objeFleetVehicleIncidentModel.UserID;
                        objDAR.CreatedOn = DateTime.UtcNow;
                        objDAR.TaskType = (long)TaskTypeCategory.eFleetVehicleIncidentSubmission;
                        Result result = _ICommonMethod.SaveDAR(objDAR);
                        #endregion Save DAR
                        
                        //Created By Ashwajit Bansod for Sending the Mail of Vehicle Incident to the Manager
                        #region Email
                        var objEmailLogRepository = new EmailLogRepository();
                        var objEmailReturn = new List<EmailToManagerModel>();
                        var objListEmailog = new List<EmailLog>();
                        var objTemplateModel = new TemplateModel();
                        if (result == Result.Completed)
                        {
                            objEmailReturn = objEmailLogRepository.SendEmailToManagerForeFleetInspection(objeFleetVehicleIncidentModel.LocationID, objeFleetVehicleIncidentModel.UserID).Result;
                        }

                        if (objEmailReturn.Count > 0 && result == Result.Completed)
                        {
                            foreach (var item in objEmailReturn)
                            {
                                bool IsSent = false;
                                var objEmailHelper = new EmailHelper();
                                objEmailHelper.emailid = item.ManagerEmail;
                                objEmailHelper.ManagerName = item.ManagerName;                               
                                objEmailHelper.VehicleNumber = objeFleetVehicleIncidentModel.VehicleNumber;
                                objEmailHelper.LocationName = objeFleetVehicleIncidentModel.LocationName;
                                objEmailHelper.UserName = item.UserName;
                                objEmailHelper.QrCodeId = objeFleetVehicleIncidentModel.QRCodeID;
                                objEmailHelper.AccidentDate = objeFleetVehicleIncidentModel.AccidentDate.ToString();
                                objEmailHelper.City = objeFleetVehicleIncidentModel.City;
                                objEmailHelper.NumberOfInjuries = objeFleetVehicleIncidentModel.NumberOfInjuries;
                                objEmailHelper.DriverNameForVehicleIncident = objeFleetVehicleIncidentModel.DriverName;
                                objEmailHelper.IncidentDescription = objeFleetVehicleIncidentModel.Description;
                                if (objeFleetVehicleIncidentModel.Preventability == true)
                                {
                                    objEmailHelper.Prevenatability = "Yes";
                                }
                                else
                                {
                                    objEmailHelper.Prevenatability = "No";
                                }
                                //objEmailHelper.InfractionStatus = obj.Status;
                                objEmailHelper.MailType = "VehicleIncident";
                                objEmailHelper.SentBy = item.RequestBy;
                                objEmailHelper.LocationID = item.LocationID;
                                objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();

                                IsSent = objEmailHelper.SendEmailWithTemplate();

                                //Push Notification
                                string message = PushNotificationMessages.eFleetIncidentForServiceReported(objeFleetVehicleIncidentModel.LocationName, objeFleetVehicleIncidentModel.QRCodeID, objeFleetVehicleIncidentModel.VehicleNumber);
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
                else
                {
                    var vehicleIncidentData = objeFleetVehicleIncidentRepository.GetAll(v => v.IsDeleted == false && v.LocationID == objeFleetVehicleIncidentModel.LocationID && v.IncidentID == objeFleetVehicleIncidentModel.IncidentID).SingleOrDefault();
                    objeFleetVehicleIncidentModel.IncidentImage = vehicleIncidentData.IncidentImage;
                    AutoMapper.Mapper.CreateMap<eFleetVehicleIncidentModel, eFleetVehicleIncident>();
                    // objeFleetVehicleIncident.FuelType = Convert.ToInt32(objeFleetVehicleIncidentModel.FuelType);
                    var objfleetVehicleMapper = AutoMapper.Mapper.Map(objeFleetVehicleIncidentModel, vehicleIncidentData);
                    objeFleetVehicleIncidentRepository.SaveChanges();
                    objeFleetVehicleIncidentModel.Result = Result.UpdatedSuccessfully;
                    if (objeFleetVehicleIncidentModel.Result == Result.UpdatedSuccessfully)
                    {
                        #region Save DAR
                        DARModel objDAR = new DARModel();
                        objDAR.ActivityDetails = DarMessage.UpdateeFleetVehicleIncident(objeFleetVehicleIncidentModel.LocationName);
                        objDAR.LocationId = objeFleetVehicleIncidentModel.LocationID;
                        objDAR.UserId = objeFleetVehicleIncidentModel.UserID;
                        objDAR.ModifiedBy = objeFleetVehicleIncidentModel.UserID;
                        objDAR.ModifiedOn = DateTime.UtcNow;
                        objDAR.TaskType = (long)TaskTypeCategory.UpdateeFleetIncidentVehicle;
                        Result result = _ICommonMethod.SaveDAR(objDAR);
                        #endregion Save DAR
                    }
                }
                return objeFleetVehicleIncidentModel;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public eFleetVehicleIncidentModel SaveEfleetVehicleIncident(eFleetVehicleIncidentModel objeFleetVehicleIncidentModel)", "Exception While saving vehicle Incident request.", objeFleetVehicleIncidentModel);
                throw;
            }
        }
        //Get all State from MasterState Table
        public List<StateModel> GetStateID()
        {
            try
            {
                var objeFleetVehicleIncidentRepository = new eFleetVehicleIncidentRepository();
                return objeFleetVehicleIncidentRepository.GetStateID();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public List<StateModel> GetStateID()", "Exception While Getting All States.", null);
                throw;
            }
        }
        /// <summary>
        /// Creayed By Ashwajit Bansod Dated : Sep-13-2017
        /// </summary>
        /// <returns></returns>
        public List<eFleetVehicleModel> GetVehicleNumber()
        {
            try
            {
                var objeFleetVehicleIncidentRepository = new eFleetVehicleIncidentRepository();
                return objeFleetVehicleIncidentRepository.GetAllVehicleNumber();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<eFleetVehicleModel> GetVehicleNumber()", "Exception While Fetching all vehicle Number.", null);
                throw;
            }
        }
        /// <summary>
        /// Created By Ashwajit Bansod Dated: Sept-16-2017
        /// for Displying JQGrid List
        /// </summary>
        /// <param name="objeFleetVehicleList"></param>
        /// <returns></returns>
        public eFleetVehicleIncidentModel GetAllVehicleIncidentList(eFleetVehicleIncidentModel objeFleetVehicleIncidentModel)
        {
            return objeFleetVehicleIncidentModel;
        }
        /// <summary>
        /// Created By Ashwajit Bansod dated:Sept-16-2017
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
        public eFleetIncidentDetails GetListVahicleListDetails(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)
        {
            try
            {
                workorderEMSEntities db = new workorderEMSEntities();
                eFleetIncidentDetails objeFleetIncidentDetails = new eFleetIncidentDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                var Results = db.eFleetVehicleIncidents.Where(a => a.IsDeleted == false && a.LocationID == locationId).Select(a => new eFleetVehicleIncidentModel()
                {
                    VehicleID = a.VehicleID,
                    QRCodeID = a.QRCodeID,
                    IncidentID = a.IncidentID,
                    AccidentDate = a.AccidentDate,
                    VehicleNumber = a.VehicleNumber,
                    DriverName = a.DriverName,
                    Preventability =(bool)a.Preventability,
                    NumberOfInjuries = a.NumberOfInjuries,
                    // ListFuelType = (from gc in db.GlobalCodes where gc.GlobalCodeId == a.FuelType select gc.CodeName).FirstOrDefault(),
                    Description = a.Description,
                    IncidentImage = a.IncidentImage

                });
                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                //if (sortOrderBy.ToLower() == "DESC")
                //{
                //    Results = Results.OrderByDescending(s => s.QRCodeID);
                //    Results = Results.Skip(pageindex * pageSize).Take(pageSize);
                //}
                //else
                //{
                Results = Results.OrderByDescending(s => s.IncidentID);
                // Results = Results.Skip(pageindex * pageSize).Take(pageSize);
                //}
                objeFleetIncidentDetails.pageindex = pageindex;
                objeFleetIncidentDetails.total = totalPages;
                objeFleetIncidentDetails.records = totRecords;
                objeFleetIncidentDetails.rows = Results.ToList();
                return objeFleetIncidentDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public eFleetIncidentDetails GetListVahicleListDetails(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)", "Exception While Listing vehicle request.", null);
                throw;
            }
        }
        /// <summary>
        /// Created By Ashwajit Bansod Dated:Sept-16-2017
        /// for fetching the data from database and display in the incident for editing 
        /// </summary>
        /// <param name="VehicleId"></param>
        /// <returns></returns>
        public eFleetVehicleIncidentModel GetIncidentDetailsById(long IncidentId)
        {
            try
            {
                workorderEMSEntities db = new workorderEMSEntities();
                var ObjeeFleetVehicleIncidentRepository = new eFleetVehicleIncidentRepository();
                var editVehicleIncidentDetails = new eFleetVehicleIncidentModel();
                var vehicleIncidentDetails = ObjeeFleetVehicleIncidentRepository.GetSingleOrDefault(u => u.IncidentID == IncidentId);
                if (vehicleIncidentDetails.IncidentID > 0)
                {
                    AutoMapper.Mapper.CreateMap<eFleetVehicleIncident, eFleetVehicleIncidentModel>();
                    var objfleetVehicleMapper = AutoMapper.Mapper.Map(vehicleIncidentDetails, editVehicleIncidentDetails);
                    //editVehicleDetails.FuelType = (from gc in db.GlobalCodes where gc.GlobalCodeId == vehicleDetails.FuelType select gc.CodeName).FirstOrDefault();
                    //// editVehicleIncidentDetails.VehicleNumber = vehicleIncidentDetails.VehicleNumber;          
                    //editVehicleIncidentDetails.DriverName = vehicleIncidentDetails.DriverName;
                    // editVehicleIncidentDetails.Description = vehicleIncidentDetails.Description;
                    //editVehicleIncidentDetails.StateId = Convert.ToInt32(vehicleIncidentDetails.StateId);
                    //editVehicleIncidentDetails.AccidentDate = vehicleIncidentDetails.AccidentDate;
                    //editVehicleIncidentDetails.Preventability = (bool)vehicleIncidentDetails.Preventability;
                    ////  //editVehicleDetails.AttachmentOfInsurance = vehicleDetails.AttachmentOfInsurance == null ? "" : HostingPrefix + ProfilePicPath.Replace("~", "") + vehicleDetails.AttachmentOfInsurance;
                    ////  //editVehicleDetails.AttachmentOfRegistration = vehicleDetails.AttachmentOfRegistration  == null ? "" : HostingPrefix + ProfilePicPath.Replace("~", "") + vehicleDetails.AttachmentOfRegistration;
                    ////  // editVehicleDetails.AttachmentOfRegistration = HostingPrefix + ProfileFilePath.Replace("~", "") + vehicleDetails.AttachmentOfRegistration;
                    editVehicleIncidentDetails.IncidentImage = vehicleIncidentDetails.IncidentImage == null ? "" : HostingPrefix + IncidentImagePath.Replace("~", "") + vehicleIncidentDetails.IncidentImage;
                    //editVehicleDetails.AttachmentOfInsuranceFile = vehicleDetails.AttachmentOfInsurance;                 
                }
                return editVehicleIncidentDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public eFleetVehicleIncidentModel GetIncidentDetailsById(long VehicleId)", "Exception While Editing vehicle request.", null);
                throw;
            }
        }

        public Result DeleteeFleetIncidentVehicle(long IncidentId, long loggedInUserId, string location)
        {
            var objDAR = new DARModel();
            try
            {
                Result result;
                if (IncidentId > 0)
                {
                    if (true)
                    {
                        var objeFleetVehicleIncidentRepository = new eFleetVehicleIncidentRepository();
                        var data = objeFleetVehicleIncidentRepository.GetSingleOrDefault(v => v.IncidentID == IncidentId && v.IsDeleted == false);
                        if (data != null)
                        {
                            data.IsDeleted = true;
                            data.DeletedBy = loggedInUserId;
                            data.DeletedDate = DateTime.UtcNow;
                            objeFleetVehicleIncidentRepository.Update(data);
                            objeFleetVehicleIncidentRepository.SaveChanges();

                            objDAR.ActivityDetails = DarMessage.DeleteFleetVehicleIncident(location);
                            objDAR.TaskType = (long)TaskTypeCategory.DeleteeFleetVehicleIncident;

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
                else
                {
                    return Result.DoesNotExist;
                }
                return Result.Delete;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public Result DeleteeFleetVehicle(long VehicleId, long loggedInUserId)", "Exception While Deleting vehicle request.", null);
                throw;
            }
        }
        /// <summary>Save eFleet Incident
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedFor>InsertVehicleIncident</CreatedFor>
        /// <CreatedOn>September-15-2017</CreatedOn>
        /// </summary>
        /// <param name="eFleetIncidentModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> InsertVehicleIncident(eFleetIncidentModel objModel)
        {
            var objReturnModel = new ServiceResponseModel<string>();
            try
            {
                var objeFleetVehicleIncidentRepository = new eFleetVehicleIncidentRepository();
                var Obj = new eFleetVehicleIncident();
                var objDAR = new DARModel();
                AutoMapper.Mapper.CreateMap<eFleetIncidentModel, eFleetVehicleIncident>();
                Obj = AutoMapper.Mapper.Map(objModel, Obj);
                Obj.CreatedBy = objModel.UserId;
                Obj.CreatedDate = DateTime.UtcNow;
                objeFleetVehicleIncidentRepository.Add(Obj);
                if (Obj.IncidentID > 0)
                {

                   // objDAR.ActivityDetails = objModel.ActivityDetails;
                    objDAR.ActivityDetails = DarMessage.RegisterNeweFleetIncidentVehicle(objModel.LocationName);
                    objDAR.LocationId = objModel.LocationID;
                    // objDAR.TaskType = objModel.TaskType;
                    objDAR.TaskType = (long)TaskTypeCategory.EfleetIncidentSubmission;
                    objDAR.CreatedBy = objModel.UserId;
                    objDAR.CreatedOn = DateTime.UtcNow;
                    objDAR.DeletedBy = null;
                    objDAR.DeletedOn = null;
                    objDAR.IsDeleted = false;
                    objDAR.IsManual = false;
                    objDAR.ModifiedBy = null;
                    objDAR.ModifiedOn = null;
                    objDAR.UserId = objModel.UserId;
                    objDAR.StartTime = objModel.AccidentDate.ToString();
                    objDAR.EndTime = DateTime.UtcNow.ToString();
                    // objDARRepository.Add(objDAR);
                    Result result = _ICommonMethod.SaveDAR(objDAR);

                    #region Email
                    var objEmailLogRepository = new EmailLogRepository();
                    var objEmailReturn = new List<EmailToManagerModel>();
                    var objListEmailog = new List<EmailLog>();
                    var objTemplateModel = new TemplateModel();
                    workorderEMSEntities db = new workorderEMSEntities();
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
                            //objEmailHelper.DriverNameforFueling = objModel.DriverName;
                           /// objEmailHelper.FuelType = (from gc in db.GlobalCodes where gc.GlobalCodeId == objModel.FuelType select gc.CodeName).FirstOrDefault();
                            objEmailHelper.IncidentDescription = objModel.Description;
                            if (objModel.Preventability == true)
                            {
                                objEmailHelper.Prevenatability = "Yes";
                            }
                            else
                            {
                                objEmailHelper.Prevenatability = "No";
                            }
                            objEmailHelper.NumberOfInjuries = objModel.NumberOfInjuries;
                            objEmailHelper.City = objModel.City;
                            objEmailHelper.VehicleNumber = objModel.VehicleNumber;
                            objEmailHelper.LocationName = objModel.LocationName;
                            objEmailHelper.UserName = item.UserName;
                            objEmailHelper.QrCodeId = objModel.QRCodeID;
                            objEmailHelper.AccidentDate = objModel.AccidentDate.ToString();
                            //objEmailHelper.InfractionStatus = obj.Status;
                            objEmailHelper.MailType = "EfleetIncidentForService";
                            objEmailHelper.SentBy = item.RequestBy;
                            objEmailHelper.LocationID = item.LocationID;
                            objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();

                            IsSent = objEmailHelper.SendEmailWithTemplate();

                            //Push Notification
                            string message = PushNotificationMessages.eFleetIncidentForServiceReported(objModel.LocationName, objModel.QRCodeID, objModel.VehicleNumber);
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
                    if (Obj.IncidentID > 0)
                    {
                        objReturnModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.InvariantCulture);
                        objReturnModel.Message = CommonMessage.Successful();
                    }
                }
            }
            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "ServiceResponseModel<string> InsertVehicleIncident(eFleetIncidentModel objModel)", "while insert eFleet vehicle incident", objModel);
                objReturnModel.Message = ex.Message;
                objReturnModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                objReturnModel.Data = null;
            }
            return objReturnModel;
        }
    }
}
