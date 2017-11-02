using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WorkOrderEMS.BusinessLogic.Managers.eFleet;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helper.SerializationHelper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.BusinessLogic.BusinessHelpers
{
    public static class InspectionPDFCreator<T>
    {
        public static string PDFCreationDAREMailforInspection(T obj1, ServiceResponseModel<UpdateEfleetInspectionTypeXML> result)
        {
            try
            {
                string returnPath = string.Empty;
                dynamic obj = obj1;
                var objEmailLogRepository = new EmailLogRepository();
                var objEmailReturn = new List<EmailToManagerModel>();
                var objListEmailog = new List<EmailLog>();
                if (result.Data.Response == 1)
                {
                    objEmailReturn = objEmailLogRepository.SendEmailToManagerForeFleetInspection(obj.LocationId, obj.UserId).Result;
                }
                if (result.Data.DamageTireStatus == true && result.Data.EmergencyAccessoriesStatus == true
                    && result.Data.EngineExteriorStatus == true && result.Data.InteriorMileageDriverStatus == true
                    && obj.Status == eFleetCheckInOut.PreTrip || obj.Status == eFleetCheckInOut.PostTrip)
                {
                    var objTemplateModel = new TemplateModel();
                    //Added By Bhushan Dod on 06/09/2017 for Damage in mobile app
                    if (result.Data.DamageTireDetails != null)
                    {
                        var ObjReturnDamageTire = new eFleetDamageTireModel();
                        ObjReturnDamageTire = GenericDataContractSerializer<eFleetDamageTireModel>.DeserializeXml(result.Data.DamageTireDetails);
                        objTemplateModel.DamageTire = ObjReturnDamageTire;
                    }
                    //Added By Bhushan Dod on 06/09/2017 for Damage in mobile app
                    if (result.Data.InteriorMileageDriverDetails != null)
                    {
                        var objInteriorMileageDriverDetails = new eFleetInteriorMileageDriverModel();
                        objInteriorMileageDriverDetails = GenericDataContractSerializer<eFleetInteriorMileageDriverModel>.DeserializeXml(result.Data.InteriorMileageDriverDetails);
                        objTemplateModel.InteriorMileageDriver = objInteriorMileageDriverDetails;
                    }
                    //Added By Bhushan Dod on 06/09/2017 for Damage in mobile app
                    if (result.Data.EngineExteriorDetails != null)
                    {
                        var objEngineExteriorDetails = new eFleetEngineExteriorModel();
                        objEngineExteriorDetails = GenericDataContractSerializer<eFleetEngineExteriorModel>.DeserializeXml(result.Data.EngineExteriorDetails);
                        objTemplateModel.EngineExterior = objEngineExteriorDetails;
                    }
                    //Added By Bhushan Dod on 06/09/2017 for Damage in mobile app
                    if (result.Data.EmergencyAccessoriesDetails != null)
                    {
                        var objEmergencyAccessoriesDetails = new eFleetEmergencyAccessoriesModel();
                        objEmergencyAccessoriesDetails = GenericDataContractSerializer<eFleetEmergencyAccessoriesModel>.DeserializeXml(result.Data.EmergencyAccessoriesDetails);
                        objTemplateModel.EmergencyAccessories = objEmergencyAccessoriesDetails;
                    }
                    objTemplateModel.Status = obj.Status;
                    objTemplateModel.UserName = obj.UserName;
                    objTemplateModel.VehicleNumber = result.Data.VehicleNumber;
                    objTemplateModel.QRCodeID = result.Data.QRCodeID;
                    objTemplateModel.TimeZoneName = obj.TimeZoneName;
                    objTemplateModel.TimeZoneOffset = obj.TimeZoneOffset;
                    objTemplateModel.IsTimeZoneinDaylight = obj.IsTimeZoneinDaylight;

                    string htmlData = TemplateDesigner.eFleetTemplate(objTemplateModel);

                    returnPath = VehicleManager.SaveInspectionPDF(obj.QrcodeId, htmlData);

                    if (objEmailReturn.Count > 0 && result.Data.Response == 1)
                    {
                        foreach (var item in objEmailReturn)
                        {
                            bool IsSent = false;
                            var objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = item.ManagerEmail;
                            objEmailHelper.ManagerName = item.ManagerName;
                            objEmailHelper.VehicleMake = result.Data.Make;
                            objEmailHelper.VehicleModel = result.Data.Model;
                            objEmailHelper.VehicleIdentificationNumber = result.Data.VehicleNumber;
                            objEmailHelper.LocationName = result.Data.LocationName;
                            objEmailHelper.UserName = item.UserName;
                            objEmailHelper.QrCodeId = obj.QrcodeId;
                            objEmailHelper.InfractionStatus = obj.Status;
                            objEmailHelper.MailType = "EFLEETINSPECTIONREPORT";
                            objEmailHelper.SentBy = item.RequestBy;
                            objEmailHelper.LocationID = item.LocationID;
                            objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();

                            string[] attachFiles = new string[1];
                            for (var i = 0; i < attachFiles.Count(); i++)
                            {
                                attachFiles[i] = HttpContext.Current.Server.MapPath("~/Content/eFleetDocs/Inspection/" + returnPath);
                            }
                            IsSent = objEmailHelper.SendEmailWithTemplate(attachFiles);

                            //Push Notification
                            string message = PushNotificationMessages.eFleetInspectionReported(result.Data.LocationName, result.Data.QRCodeID, result.Data.VehicleNumber);
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
                                catch (Exception ex)
                                {
                                    return returnPath;
                                }
                            }
                        }
                        using (var context = new workorderEMSEntities())
                        {
                            context.EmailLogs.AddRange(objListEmailog);
                            context.SaveChanges(); ;
                        }
                        //var x = EmailLogRepository.InsertEntitiesNew("EmailLog", objListEmailog);
                        //Task<bool> x = null;
                        //foreach (var i in objListEmailog)
                        //{
                        //    x = objEmailLogRepository.SaveEmailLogAsync(i);
                        //}
                    }
                }

                return returnPath;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "static bool PDFCreationDAREMailforInspection(T obj1, ServiceResponseModel<UpdateEfleetInspectionTypeXML> result)", "while creating PDF", result.Data.VehicleID);

                return null;
            }
        }
    }
}
