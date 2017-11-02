using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helper.SerializationHelper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.BusinessLogic.Managers.eFleet
{
    public class eFleetCommonManager : IeFleetFuelingManager
    {
        /// <summary>Save eFleet Fueling
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedFor>InserteFleetFueling</CreatedFor>
        /// <CreatedOn>September-04-2017</CreatedOn>
        /// </summary>
        /// <param name="eFleetDamageTireModel"></param>
        /// <returns></returns>
        CommonMethodManager _ICommonMethod = new CommonMethodManager();
        public ServiceResponseModel<string> InserteFleetFueling(eFleetFuelingModelForService objModel)
        {
            var objReturnModel = new ServiceResponseModel<string>();
            try
            {
                var objFuelingRepository = new FuelingRepository();
                var objDARRepository = new DARRepository();
                var objDARDetail = new DARDetail();
                var objDAR = new DARModel();
                var Obj = new eFleetFueling();
                AutoMapper.Mapper.CreateMap<eFleetFuelingModelForService, eFleetFueling>();
                Obj = AutoMapper.Mapper.Map(objModel, Obj);
                Obj.CreatedBy = objModel.UserId;           
                Obj.CreatedDate = DateTime.UtcNow;
                objFuelingRepository.Add(Obj);
                if (Obj.FuelID > 0)
                {
                    objDARDetail.ActivityDetails = DarMessage.RegisterNeweFleetFueling(objModel.LocationName);
                    //objDAR.ActivityDetails = objModel.ActivityDetails;
                    objDARDetail.LocationId = objModel.LocationID;
                    objDARDetail.TaskType = (long)TaskTypeCategory.EfleetFuelingSubmission;
                    objDARDetail.CreatedBy = objModel.UserId;
                    objDARDetail.CreatedOn = DateTime.UtcNow;
                    objDARDetail.DeletedBy = null;
                    objDARDetail.DeletedOn = null;
                    objDARDetail.IsDeleted = false;
                    objDARDetail.IsManual = false;
                    objDARDetail.ModifiedBy = null;
                    objDARDetail.ModifiedOn = null;
                    
                    objDARDetail.UserId = objModel.UserId;
                    objDARDetail.StartTime = objModel.FuelingDate;
                    objDARDetail.EndTime = DateTime.UtcNow;
                    objDARRepository.Add(objDARDetail);
                  //Result result = _ICommonMethod.SaveDAR(objDARDetail);

                    #region Email
                    var objEmailLogRepository = new EmailLogRepository();
                    var objEmailReturn = new List<EmailToManagerModel>();
                    var objListEmailog = new List<EmailLog>();
                    var objTemplateModel = new TemplateModel();
                    Result result;
                    workorderEMSEntities db = new workorderEMSEntities();
                    if (objDARDetail.DARId > 0)
                    {
                        objEmailReturn = objEmailLogRepository.SendEmailToManagerForeFleetInspection(objModel.LocationID, objModel.UserId).Result;
                    }

                    if (objEmailReturn.Count > 0 && objDARDetail.DARId > 0)
                    {
                        foreach (var item in objEmailReturn)
                        {
                            bool IsSent = false;
                            var objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = item.ManagerEmail;
                            objEmailHelper.ManagerName = item.ManagerName;
                            objEmailHelper.DriverNameforFueling = objModel.DriverName;
                            objEmailHelper.FuelType = (from gc in db.GlobalCodes where gc.GlobalCodeId == objModel.FuelType select gc.CodeName).FirstOrDefault();
                            objEmailHelper.GasStatioName = objModel.GasStatioName;
                            objEmailHelper.Mileage = objModel.Mileage;
                            objEmailHelper.CurrentFuel = objModel.CurrentFuel;
                            objEmailHelper.Total = objModel.Total.ToString();
                            objEmailHelper.VehicleNumber = objModel.VehicleNumber;
                            objEmailHelper.LocationName = objModel.LocationName;
                            objEmailHelper.UserName = item.UserName;
                            objEmailHelper.QrCodeId = objModel.QRCodeID;
                            objEmailHelper.FuelingDate = objModel.FuelingDate.ToString();
                            //objEmailHelper.InfractionStatus = obj.Status;
                            objEmailHelper.MailType = "EfleetFueling";
                            objEmailHelper.SentBy = item.RequestBy;
                            objEmailHelper.LocationID = item.LocationID;
                            objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();

                            IsSent = objEmailHelper.SendEmailWithTemplate();

                            //Push Notification
                            string message = PushNotificationMessages.eFleetFuelingReported(objModel.LocationName, objModel.QRCodeID, objModel.VehicleNumber);
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
                            context.SaveChanges();
                        }
                        //    //var x = EmailLogRepository.InsertEntitiesNew("EmailLog", objListEmailog);
                        //    //Task<bool> x = null;
                        //    //foreach (var i in objListEmailog)
                        //    //{
                        //    //    x = objEmailLogRepository.SaveEmailLogAsync(i);
                        //    //}
                        //}


                        #endregion Email

                        if (objDARDetail.DARId > 0)
                        {
                            objReturnModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.InvariantCulture);
                            objReturnModel.Message = CommonMessage.Successful();
                        }
                        else
                        {
                            objReturnModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                            objReturnModel.Message = CommonMessage.FailureMessage();
                        }
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

        /// <summary>Get eFleetVehicle Details
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>GeteFleetVehicleDetailsByID</CreatedFor>
        /// <CreatedOn>Sept-06-2017</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCElevatorModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<VehicleScanModel> GeteFleetVehicleDetailsByID(VehicleScanModel ObjServiceVehicleModel)
        {
            var ObjeFleetVehicleRepository = new eFleetVehicleRepository();
            var objeFleetVehicleScanLogRepository = new eFleetVehicleScanLogRepository();
            var ObjUserRepository = new UserRepository();
            var objDARRepository = new DARRepository();
            ServiceDARModel obj = new ServiceDARModel();
            ServiceResponseModel<VehicleScanModel> ObjServiceResponseModel = new ServiceResponseModel<VehicleScanModel>();
            try
            {               
                    var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == ObjServiceVehicleModel.ServiceAuthKey && x.IsDeleted == false);
                    if (authuser != null && authuser.UserId > 0)
                    {
                        var result = GeteFleetVehicleById(ObjServiceVehicleModel.QRCodeID, ObjServiceVehicleModel.LocationID);

                        ObjServiceResponseModel.Message = (result != null && result.VehicleID > 0) ? CommonMessage.Successful() : CommonMessage.DoesNotExistsRecordMessage();
                        ObjServiceResponseModel.Response = (result != null && result.VehicleID > 0) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Data = result;
                        if (result.VehicleID > 0)
                        {
                            //For Scan log maintian
                            long VehScanLogId = objeFleetVehicleScanLogRepository.SaveeFleetVehicleScanLog(result.VehicleID, Convert.ToInt64(eFleetEnum.VehicleScan), authuser.UserId, Convert.ToInt64(result.LocationID));
                            ObjServiceResponseModel.Data.VehicleScanLogId = VehScanLogId;

                            //For DAR log maintian
                            obj.CreatedBy = authuser.UserId;
                            obj.ActivityDetails = DarMessage.VehicleScanMessage((authuser.FirstName + " " + authuser.LastName), result.VehicleNumber, result.QRCodeID);
                            obj.LocationId = Convert.ToInt64(result.LocationID);
                            obj.UserId = authuser.UserId;
                            obj.TaskType = (long)eFleetEnum.VehicleScan;
                            long DarId = objDARRepository.SaveDARDetails(obj);
                            ObjServiceResponseModel.Data.DarID = DarId;
                        }
                    }
                    else
                    {

                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.InvalidSessionResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    }
               
            }
            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "ServiceResponseModel<VehicleScanModel> GeteFleetVehicleDetailsByID(VehicleScanModel ObjServiceVehicleModel)", "while fetching GeteFleetVehicleDetailsByID", ObjServiceVehicleModel);
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }
            return ObjServiceResponseModel;
        }

        /// <summary>
        /// CreatedBy   :   Bhushan Dod
        /// CreatedOn   :   Sep-08-2017
        /// CreatedFor  :   Get eFleet Vehicle by ID
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public VehicleScanModel GeteFleetVehicleById(string QRCodeID, long locationId)
        {
            try
            {
                var ObjReturn = new VehicleScanModel();

                //ServiceQrcShuttleBusModel objReturnShuttle = new ServiceQrcShuttleBusModel();
                var ObjeFleetVehicleRepository = new eFleetVehicleRepository();
                var ObjUserRepository = new UserRepository();
                var ObjVehicleDetail = ObjeFleetVehicleRepository.GetSingleOrDefault(q => q.QRCodeID == QRCodeID && q.LocationID == locationId && q.IsDeleted == false);
                if (ObjVehicleDetail == null)
                { throw new Exception("Record not found."); }
                AutoMapper.Mapper.CreateMap<eFleetVehicle, VehicleScanModel>();
                ObjReturn = AutoMapper.Mapper.Map(ObjVehicleDetail, ObjReturn);

                string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);

                string AssetImgURLPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["VehicleImage"], CultureInfo.InvariantCulture);
                ObjReturn.VehicleImage = HostingPrefix + AssetImgURLPrefix.Replace("~", "") + ObjVehicleDetail.VehicleImage;

                //Added By Bhushan Dod on 06/09/2017 for Damage in mobile app
                if (ObjVehicleDetail.DamageTireDetails != null)
                {
                    var ObjReturnDamageTire = new eFleetDamageTireModel();
                    ObjReturnDamageTire = GenericDataContractSerializer<eFleetDamageTireModel>.DeserializeXml(ObjVehicleDetail.DamageTireDetails);
                    //ObjReturn.DamageTire = ObjReturnDamageTire;
                    //ObjReturn.IsDamage = ObjReturnDamageTire.Damage.IsDamage;
                    //ObjReturn.DarID = ObjReturnDamageTire.Damage.DamageDarId;
                }
                //Added By Bhushan Dod on 06/09/2017 for Damage in mobile app
                if (ObjVehicleDetail.InteriorMileageDriverDetails != null)
                {
                    var objInteriorMileageDriverDetails = new eFleetInteriorMileageDriverModel();
                    objInteriorMileageDriverDetails = GenericDataContractSerializer<eFleetInteriorMileageDriverModel>.DeserializeXml(ObjVehicleDetail.InteriorMileageDriverDetails);
                    ObjReturn.ChShDescription = objInteriorMileageDriverDetails.Mileage.ChShDescription;
                    ObjReturn.OldChShDescription = objInteriorMileageDriverDetails.Mileage.OldChShDescription;
                   
                    //ObjReturn.InteriorMileageDriver = objInteriorMileageDriverDetails;
                }
                //Added By Bhushan Dod on 06/09/2017 for Damage in mobile app
                if (ObjVehicleDetail.EngineExteriorDetails != null)
                {
                    var objEngineExteriorDetails = new eFleetEngineExteriorModel();
                    objEngineExteriorDetails = GenericDataContractSerializer<eFleetEngineExteriorModel>.DeserializeXml(ObjVehicleDetail.EngineExteriorDetails);
                    //ObjReturn.EngineExterior = objEngineExteriorDetails;
                }
                //Added By Bhushan Dod on 06/09/2017 for Damage in mobile app
                if (ObjVehicleDetail.EmergencyAccessoriesDetails != null)
                {
                    var objEmergencyAccessoriesDetails = new eFleetEmergencyAccessoriesModel();
                    objEmergencyAccessoriesDetails = GenericDataContractSerializer<eFleetEmergencyAccessoriesModel>.DeserializeXml(ObjVehicleDetail.EmergencyAccessoriesDetails);
                    //ObjReturn.EmergencyAccessories = objEmergencyAccessoriesDetails;
                }
                return ObjReturn;
            }
            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "VehicleScanModel GeteFleetVehicleById(long vehicleId, long locationId)", "while fetching GeteFleetVehicleById", QRCodeID);
                return null;
            }
        }
    }
}
