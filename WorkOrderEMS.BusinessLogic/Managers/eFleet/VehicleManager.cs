using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helper.SerializationHelper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.BusinessLogic.Managers.eFleet
{
    public class VehicleManager : IEfleetVehicle
    {
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string VehicleImagePath = Convert.ToString(ConfigurationManager.AppSettings["VehicleImage"], CultureInfo.InvariantCulture);
        private string ProfileFilePath = ConfigurationManager.AppSettings["AttachmentOfInsurance"];

        CommonMethodManager _ICommonMethod = new CommonMethodManager();
        /// <summary>
        /// Save eFLeet vehicle details
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveeFleetInspectionRequestDetails</CreatedFor>
        /// <CreatedOn>August-29-2017</CreatedOn>
        /// </summary>
        /// <param name="objeFleetVehicleModel"></param>
        /// <returns></returns>
        public eFleetVehicleModel SaveEfleetVehicle(eFleetVehicleModel objeFleetVehicleModel)
        {
            try
            {
                var objeFleetVehicle = new eFleetVehicle();
                var objeFleetVehicleRepository = new eFleetVehicleRepository();
                var objeTracLoginModel = new eTracLoginModel();
                if (objeFleetVehicleModel.VehicleID == 0)
                {
                    objeFleetVehicleModel.QRCDefaultSize = 155;
                    AutoMapper.Mapper.CreateMap<eFleetVehicleModel, eFleetVehicle>();
                    objeFleetVehicle.FuelType = Convert.ToInt32(objeFleetVehicleModel.FuelType);
                    var objfleetVehicleMapper = AutoMapper.Mapper.Map(objeFleetVehicleModel, objeFleetVehicle);
                    objeFleetVehicleRepository.Add(objfleetVehicleMapper);
                    objeFleetVehicle.QRCodeID = objeFleetVehicleModel.QRCodeID + "EF" + (objeFleetVehicle.VehicleID + 100).ToString();
                    objeFleetVehicleModel.QRCodeID = objeFleetVehicle.QRCodeID;
                    // objeFleetVehicleModel.ListFuelType = objeFleetVehicleModel.FuelType.ToString();
                    //objeFleetVehicleModel.LocationName = objeFleetVehicle.LocationMaster.LocationName;
                    objeFleetVehicleRepository.SaveChanges();
                    objeFleetVehicleModel.Result = Result.Completed;
                    if (objeFleetVehicleModel.Result == Result.Completed)
                    {
                        #region Save DAR
                        DARModel objDAR = new DARModel();
                        objDAR.ActivityDetails = DarMessage.RegisterNeweFleetVehicle(objeTracLoginModel.LocationNames);
                        objDAR.LocationId = objeFleetVehicleModel.LocationID;
                        objDAR.UserId = objeFleetVehicleModel.UserID;
                        objDAR.CreatedBy = objeFleetVehicleModel.UserID;
                        objDAR.CreatedOn = DateTime.UtcNow;
                        objDAR.TaskType = (long)TaskTypeCategory.eFleetVehicleSubmission;
                        Result result = _ICommonMethod.SaveDAR(objDAR);
                        #endregion Save DAR
                    }
                }
                else
                {
                    var vehicleData = objeFleetVehicleRepository.GetAll(v => v.IsDeleted == false && v.VehicleID == objeFleetVehicleModel.VehicleID).SingleOrDefault();
                    if (objeFleetVehicleModel.VehicleImageFile != null)
                    {
                    }
                    else
                    {
                        objeFleetVehicleModel.VehicleImage = vehicleData.VehicleImage;
                    }
                    if (objeFleetVehicleModel.AttachmentOfInsuranceFile != null)
                    {
                    }
                    else
                    {
                        objeFleetVehicleModel.AttachmentOfInsurance = vehicleData.AttachmentOfInsurance;
                    }
                    if (objeFleetVehicleModel.AttachmentOfRegistrationFile != null)
                    {
                    }
                    else
                    {
                        objeFleetVehicleModel.AttachmentOfRegistration = vehicleData.AttachmentOfRegistration;
                    }
                    objeFleetVehicleModel.QRCDefaultSize = 155;
                    AutoMapper.Mapper.CreateMap<eFleetVehicleModel, eFleetVehicle>();
                    objeFleetVehicle.FuelType = Convert.ToInt32(objeFleetVehicleModel.FuelType);
                    var objfleetVehicleMapper = AutoMapper.Mapper.Map(objeFleetVehicleModel, vehicleData);
                    objeFleetVehicleModel.QRCodeID = vehicleData.QRCodeID;
                    //objeFleetVehicleModel.LocationName = objeFleetVehicle.LocationMaster.LocationName;
                    objeFleetVehicleRepository.SaveChanges();
                    objeFleetVehicleModel.Result = Result.UpdatedSuccessfully;
                    if (objeFleetVehicleModel.Result == Result.UpdatedSuccessfully)
                    {
                        #region Save DAR
                        DARModel objDAR = new DARModel();
                        objDAR.ActivityDetails = DarMessage.UpdateeFleetVehicle(objeFleetVehicleModel.LocationName);
                        objDAR.LocationId = objeFleetVehicleModel.LocationID;
                        objDAR.UserId = objeFleetVehicleModel.UserID;
                        objDAR.ModifiedBy = objeFleetVehicleModel.UserID;
                        objDAR.ModifiedOn = DateTime.UtcNow;
                        objDAR.TaskType = (long)TaskTypeCategory.UpdateeFleetVehicle;
                        Result result = _ICommonMethod.SaveDAR(objDAR);
                        #endregion Save DAR
                    }
                }
                return objeFleetVehicleModel;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public eFleetVehicleModel SaveEfleetVehicle(eFleetVehicleModel objeFleetVehicleModel)", "Exception While saving vehicle request.", objeFleetVehicleModel);
                throw;
            }
        }

        /// <summary>Save eFleet Inspection Request
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveeFleetInspectionRequestDetails</CreatedFor>
        /// <CreatedOn>May-09-2017</CreatedOn>
        /// </summary>
        /// <param name="eFleetDamageTireModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<UpdateEfleetInspectionTypeXML> SaveeFleetDamageTireInspectionDetails(eFleetDamageTireModel obj)
        {
            var ObjQRCMasterRepository = new QRCMasterRepository();
            var ObjServiceResponseModel = new ServiceResponseModel<UpdateEfleetInspectionTypeXML>();
            try
            {
                if (obj.ServiceAuthKey != null)
                {
                    string ToXml;
                    ToXml = GenericDataContractSerializer<eFleetDamageTireModel>.SerializeObject(obj);
                    var result = ObjQRCMasterRepository.eFLeetSave(obj.ServiceAuthKey, obj.UserId, obj.VehicleID, ToXml, obj.Action, obj.DamageStatus, obj.UserName);
                    if (result.Data.VehicleID > 0 &&
                        result.Data.DamageTireStatus == true &&
                        result.Data.EmergencyAccessoriesStatus == true &&
                        result.Data.EngineExteriorStatus == true &&
                        result.Data.InteriorMileageDriverStatus == true &&
                        obj.Status == eFleetCheckInOut.PreTrip ||
                        obj.Status == eFleetCheckInOut.PostTrip)
                    {
                        string status = BusinessHelpers.InspectionPDFCreator<eFleetDamageTireModel>.PDFCreationDAREMailforInspection(obj, result);
                        result.Data.InspectionStatusFile = status;
                    }
                    ObjServiceResponseModel.Response = (result.Data.Response != Convert.ToInt32(ServiceResponse.InvalidSessionResponse)) ? Convert.ToInt64(result.Data.Response, CultureInfo.InvariantCulture) : Convert.ToInt32(ServiceResponse.InvalidSessionResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = (result.Data.Response != null) ? (result.Data.ResponseMessage).ToString(CultureInfo.InvariantCulture) : CommonMessage.NoRecordMessage();//CommonMessage.MessageLogout();
                    ObjServiceResponseModel.Data = (result.Data.Response == Convert.ToInt32(ServiceResponse.SuccessResponse)) ? result.Data : null;
                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "ServiceResponseModel<UpdateEfleetInspectionTypeXML> SaveeFleetInspectionRequestDetails(eFleetDamageTireModel obj)", "while changing status", obj);
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }
            return ObjServiceResponseModel;
        }

        /// <summary>Save eFleet Inspection Request
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveeFleetInspectionRequestDetails</CreatedFor>
        /// <CreatedOn>May-09-2017</CreatedOn>
        /// </summary>
        /// <param name="eFleetDamageTireModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<UpdateEfleetInspectionTypeXML> SaveeFleetInteriorMileageInspectionDetails(eFleetInteriorMileageDriverModel obj)
        {
            var ObjQRCMasterRepository = new QRCMasterRepository();
            var ObjServiceResponseModel = new ServiceResponseModel<UpdateEfleetInspectionTypeXML>();
            var objEmailReturn = new List<EmailToManagerModel>();
            var objListEmailog = new List<EmailLog>();
            try
            {
                if (obj.ServiceAuthKey != null)
                {
                    string ToXml;
                    ToXml = GenericDataContractSerializer<eFleetInteriorMileageDriverModel>.SerializeObject(obj);
                    var result = ObjQRCMasterRepository.eFLeetSave(obj.ServiceAuthKey, obj.UserId, obj.VehicleID, ToXml, obj.Action, obj.DamageStatus, obj.UserName);
                    if (result.Data.VehicleID > 0)
                    {
                        string status = BusinessHelpers.InspectionPDFCreator<eFleetInteriorMileageDriverModel>.PDFCreationDAREMailforInspection(obj, result);
                        result.Data.InspectionStatusFile = status;
                    }
                    ObjServiceResponseModel.Response = (result.Data.Response != Convert.ToInt32(ServiceResponse.InvalidSessionResponse)) ? Convert.ToInt64(result.Data.Response, CultureInfo.InvariantCulture) : Convert.ToInt32(ServiceResponse.InvalidSessionResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = (result.Data.Response != null) ? (result.Data.ResponseMessage).ToString(CultureInfo.InvariantCulture) : CommonMessage.NoRecordMessage();//CommonMessage.MessageLogout();
                    ObjServiceResponseModel.Data = (result.Data.Response == Convert.ToInt32(ServiceResponse.SuccessResponse)) ? result.Data : null;
                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "ServiceResponseModel<UpdateEfleetInspectionTypeXML> SaveeFleetInspectionRequestDetails(eFleetDamageTireModel obj)", "while changing status", obj);
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }
            return ObjServiceResponseModel;
        }

        /// <summary>Save eFleet Inspection Request
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveeFleetInspectionRequestDetails</CreatedFor>
        /// <CreatedOn>May-09-2017</CreatedOn>
        /// </summary>
        /// <param name="eFleetDamageTireModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<UpdateEfleetInspectionTypeXML> SaveeFleetEngineExteriorInspectionDetails(eFleetEngineExteriorModel obj)
        {
            var ObjQRCMasterRepository = new QRCMasterRepository();
            var ObjServiceResponseModel = new ServiceResponseModel<UpdateEfleetInspectionTypeXML>();
            var objEmailReturn = new List<EmailToManagerModel>();
            var objListEmailog = new List<EmailLog>();
            try
            {
                if (obj.ServiceAuthKey != null)
                {
                    string ToXml;
                    ToXml = GenericDataContractSerializer<eFleetEngineExteriorModel>.SerializeObject(obj);
                    var result = ObjQRCMasterRepository.eFLeetSave(obj.ServiceAuthKey, obj.UserId, obj.VehicleID, ToXml, obj.Action, obj.DamageStatus, obj.UserName);
                    if (result.Data.VehicleID > 0)
                    {
                        string status = BusinessHelpers.InspectionPDFCreator<eFleetEngineExteriorModel>.PDFCreationDAREMailforInspection(obj, result);
                        result.Data.InspectionStatusFile = status;
                    }
                    ObjServiceResponseModel.Response = (result.Data.Response != -2) ? Convert.ToInt64(result.Data.Response, CultureInfo.InvariantCulture) : Convert.ToInt32(ServiceResponse.InvalidSessionResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = (result.Data.Response != null) ? (result.Data.ResponseMessage).ToString(CultureInfo.InvariantCulture) : CommonMessage.NoRecordMessage();//CommonMessage.MessageLogout();
                    ObjServiceResponseModel.Data = (result.Data.Response == Convert.ToInt32(ServiceResponse.SuccessResponse)) ? result.Data : null;

                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "ServiceResponseModel<UpdateEfleetInspectionTypeXML> SaveeFleetInspectionRequestDetails(eFleetDamageTireModel obj)", "while changing status", obj);
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }
            return ObjServiceResponseModel;
        }

        /// <summary>Save eFleet Inspection Request
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveeFleetInspectionRequestDetails</CreatedFor>
        /// <CreatedOn>May-09-2017</CreatedOn>
        /// </summary>
        /// <param name="eFleetDamageTireModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<UpdateEfleetInspectionTypeXML> SaveeFleetEmergencyAccessoriesInspectionDetails(eFleetEmergencyAccessoriesModel obj)
        {
            var ObjQRCMasterRepository = new QRCMasterRepository();
            var ObjServiceResponseModel = new ServiceResponseModel<UpdateEfleetInspectionTypeXML>();
            var objEmailReturn = new List<EmailToManagerModel>();
            var objListEmailog = new List<EmailLog>();
            try
            {
                if (obj.ServiceAuthKey != null)
                {
                    string ToXml;
                    ToXml = GenericDataContractSerializer<eFleetEmergencyAccessoriesModel>.SerializeObject(obj);
                    var result = ObjQRCMasterRepository.eFLeetSave(obj.ServiceAuthKey, obj.UserId, obj.VehicleID, ToXml, obj.Action, obj.DamageStatus, obj.UserName);
                    if (result.Data.VehicleID > 0)
                    {
                        string status = BusinessHelpers.InspectionPDFCreator<eFleetEmergencyAccessoriesModel>.PDFCreationDAREMailforInspection(obj, result);
                        result.Data.InspectionStatusFile = status;
                    }
                    ObjServiceResponseModel.Response = (result.Data.Response != -2) ? Convert.ToInt64(result.Data.Response, CultureInfo.InvariantCulture) : Convert.ToInt32(ServiceResponse.InvalidSessionResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = (result.Data.Response != null) ? (result.Data.ResponseMessage).ToString(CultureInfo.InvariantCulture) : CommonMessage.NoRecordMessage();//CommonMessage.MessageLogout();
                    ObjServiceResponseModel.Data = (result.Data.Response == Convert.ToInt32(ServiceResponse.SuccessResponse)) ? result.Data : null;
                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "ServiceResponseModel<UpdateEfleetInspectionTypeXML> SaveeFleetInspectionRequestDetails(eFleetDamageTireModel obj)", "while changing status", obj);
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }
            return ObjServiceResponseModel;
        }

        public ServiceResponseModel<string> ChangingStatusOfInsection(ChangeInspectionStatusModel objChangeInspectionStatusModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<ChangeInspectionStatusModel>();
            eFleetVehicle objeFleetVehicle;
            eFleetVehicleRepository ObjeFleetVehicleRepository = new eFleetVehicleRepository();
            var objDARRepository = new DARRepository();
            var objResponseModel = new ServiceResponseModel<string>();
            try
            {
                // var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == objChangeInspectionStatusModel.ServiceAuthKey && x.IsDeleted == false);

                objeFleetVehicle = ObjeFleetVehicleRepository.GetAll(x => x.IsDeleted == false && x.VehicleID == objChangeInspectionStatusModel.VehicleID && x.LocationID == objChangeInspectionStatusModel.LocationId).FirstOrDefault();
                if (objeFleetVehicle != null)
                {
                    objeFleetVehicle.CheckOutStatus = objChangeInspectionStatusModel.CheckOutStatus;
                    objeFleetVehicle.DamageTireStatus = false;
                    objeFleetVehicle.InteriorMileageDriverStatus = false;
                    objeFleetVehicle.EngineExteriorStatus = false;
                    objeFleetVehicle.EmergencyAccessoriesStatus = false;
                    objeFleetVehicle.DummyField = objChangeInspectionStatusModel.InspectionStatusFile;
                    ObjeFleetVehicleRepository.Update(objeFleetVehicle);

                    // var managerlist =  SendeFleetCheckOutInNotificaitonToAllManager(objChangeInspectionStatusModel.LocationId, objChangeInspectionStatusModel.UserId);

                    //foreach (var item in managerlist)
                    //{

                    //}
                    objResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                    objResponseModel.Message = CommonMessage.SaveSuccessMessage();
                }
                else
                {
                    objResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                    objResponseModel.Message = CommonMessage.NoRecordMessage();
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "ServiceResponseModel<string> ChangingStatusOfInsection(ChangeInspectionStatusModel objChangeInspectionStatusModel)", "while changing status", objChangeInspectionStatusModel);
                objResponseModel.Message = ex.Message;
                objResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                objResponseModel.Data = null;
            }
            return objResponseModel;
        }

        /// <summary>
        /// Created by Bhushan Dod on 24 August 2017
        /// This function is using for sending notification to all Manager after call inspection done by employee.
        /// </summary>
        /// <returns></returns>
        public List<listForEmployeeDevice> SendeFleetCheckOutInNotificaitonToAllManager(long LocationId, long UserId)
        {
            using (workorderEMSEntities Context = new workorderEMSEntities())
            {
                try
                {
                    List<listForEmployeeDevice> a = (from pd in Context.PermissionDetails
                                                     join ur in Context.UserRegistrations
                                                            on pd.UserId equals ur.UserId
                                                     join elm in Context.ManagerLocationMappings
                                                            on ur.UserId equals elm.ManagerUserId
                                                     join lm in Context.LocationMasters
                                                            on elm.LocationId equals lm.LocationId

                                                     where ur.UserType == Convert.ToInt64(UserType.Manager) // employee type
                                                     && ur.IsDeleted == false
                                                     && elm.LocationId == LocationId
                                                     && (pd.PermissionId == 10 || pd.PermissionId == 428) //very nice
                                                     && pd.LocationId == LocationId
                                                     select new listForEmployeeDevice
                                                     {
                                                         PermissionDetailId = pd.PermissionDetailId,
                                                         DeviceId = ur.DeviceId,
                                                         UserId = ur.UserId,
                                                         LocationName = lm.LocationName
                                                     }).Distinct().OrderBy(x => x.PermissionDetailId).ToList();
                    return a;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        //public string PDFCreationofInspectionDone(eFleetVehicle objeFleetVehicle)
        //{
        //    string filename = HttpContext.Current.Server.MapPath("~/Content/eMaintenance/DisclaimerDownload/" + objeFleetVehicle.QRCodeID + ".pdf");
        //    //----------------------------
        //    Document doc = new Document(PageSize.A4, 30f, 30f, 40f, 30f);
        //    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(filename, FileMode.CreateNew));
        //    doc.Open();
        //    try
        //    {
        //        //var content = writer.DirectContent;
        //        //var pageBorderRect = new iTextSharp.text.Rectangle(doc.PageSize);
        //        //pageBorderRect.Left += doc.LeftMargin;
        //        //pageBorderRect.Right -= doc.RightMargin;
        //        //pageBorderRect.Top -= doc.TopMargin;
        //        //pageBorderRect.Bottom += doc.BottomMargin;
        //        //content.SetColorStroke(BaseColor.BLACK);
        //        //content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
        //        //content.Stroke();

        //        //Logo
        //        string imageURL = HttpContext.Current.Server.MapPath("~/Images/logo-etrac.png");
        //        iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
        //        jpg.Alignment = 3;
        //        jpg.SpacingBefore = 30f;
        //        jpg.ScaleToFit(100f, 80f);
        //        jpg.SpacingBefore = 10f;
        //        jpg.SpacingAfter = 1f;
        //        doc.Add(jpg);

        //        foreach (IElement element in HTMLWorker.ParseToList(
        //        new StringReader(htmlData), null))
        //        {
        //            doc.Add(element);
        //        }

        //        PdfPTable table = new PdfPTable(2);

        //        table.WidthPercentage = 96;
        //        BaseFont bf = BaseFont.CreateFont(
        //                    BaseFont.TIMES_ROMAN,
        //                    BaseFont.CP1252,
        //                    BaseFont.EMBEDDED);
        //        iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 11);

        //        //PdfPCell cell = new PdfPCell(new Phrase("Start Time: " + imgupload.StartTime, font));
        //        //cell.Colspan = 1;
        //        //cell.Border = 0;
        //        //cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        //table.AddCell(cell);

        //        //cell = new PdfPCell(new Phrase("End Time: " + imgupload.EndTime, font));
        //        //cell.Colspan = 1;
        //        //cell.Border = 0;
        //        //cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        //table.AddCell(cell);

        //        PdfPCell cell = new PdfPCell(new Phrase("Manager or Employee Name: " + objData.AssignedFirstName + ' ' + objData.AssignedLastName, font));
        //        cell.Colspan = 1;
        //        cell.Border = 0;
        //        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Drivers Licence Number: " + objData.DriverLicenseNo, font));
        //        cell.Colspan = 1;
        //        cell.Border = 0;
        //        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Customer Name: " + objData.CustomerName, font));
        //        cell.Colspan = 1;
        //        cell.Border = 0;
        //        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Customer Address: " + objData.Address, font));
        //        cell.Colspan = 1;
        //        cell.Border = 0;
        //        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Customer Telephone: " + objData.CustomerContact, font));
        //        cell.Colspan = 1;
        //        cell.Border = 0;
        //        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(" ", font));
        //        cell.Colspan = 1;
        //        cell.Border = 0;
        //        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(" ", font));
        //        cell.Colspan = 1;
        //        cell.Border = 0;
        //        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell);
        //        cell = new PdfPCell(new Phrase(" ", font));
        //        cell.Colspan = 1;
        //        cell.Border = 0;
        //        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell);
        //        cell = new PdfPCell(new Phrase(" ", font));
        //        cell.Colspan = 1;
        //        cell.Border = 0;
        //        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell);
        //        cell = new PdfPCell(new Phrase(" ", font));
        //        cell.Colspan = 1;
        //        cell.Border = 0;
        //        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell);

        //        imageURL = HttpContext.Current.Server.MapPath("~/Content/Images/FRSignature/" + imgupload.Image);
        //        jpg = iTextSharp.text.Image.GetInstance(imageURL);
        //        jpg.ScaleToFit(140f, 120f);
        //        jpg.SpacingBefore = 10f;
        //        jpg.SpacingAfter = 1f;

        //        cell = new PdfPCell(new Phrase(" ", font));
        //        cell.Colspan = 1;
        //        cell.Border = 0;
        //        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

        //        cell.AddElement(jpg);
        //        table.AddCell(cell);

        //        imageURL = HttpContext.Current.Server.MapPath("~/Content/Images/FRSignature/" + imgupload.ImageEmp);
        //        jpg = iTextSharp.text.Image.GetInstance(imageURL);
        //        jpg.ScaleToFit(140f, 120f);
        //        jpg.SpacingBefore = 10f;
        //        jpg.SpacingAfter = 1f;

        //        cell = new PdfPCell(new Phrase(" ", font));
        //        cell.Colspan = 1;
        //        cell.Border = 0;
        //        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

        //        cell.AddElement(jpg);
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(objData.CustomerName, font));
        //        cell.Colspan = 1;
        //        cell.Border = 0;
        //        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(objData.AssignedFirstName + ' ' + objData.AssignedLastName, font));
        //        cell.Colspan = 1;
        //        cell.Border = 0;
        //        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(" ", font));
        //        cell.Colspan = 1;
        //        cell.Border = 0;
        //        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(" ", font));
        //        cell.Colspan = 1;
        //        cell.Border = 0;
        //        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Signature of Customer", font));
        //        cell.Colspan = 1;
        //        cell.Border = 0;
        //        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Signature of Manager or Employee", font));
        //        cell.Colspan = 1;
        //        cell.Border = 0;
        //        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell);

        //        doc.Add(table);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        doc.Close();
        //        // Watermark code            
        //        string watermarkLoc = System.Web.HttpContext.Current.Server.MapPath("~/Images/eTrac380-light.png");
        //        PdfReader pdfReader = new PdfReader(filename);
        //        PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(filename.Replace(EncryptWorkId + ".pdf", waterMarkDisclaimerFilename), FileMode.Create));

        //        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(watermarkLoc);
        //        var pageSize = pdfReader.GetPageSizeWithRotation(1);

        //        var x = pageSize.Width / 2 - img.ScaledWidth / 2;
        //        var y = pageSize.Height / 2 - img.ScaledHeight / 2;
        //        img.SetAbsolutePosition(x, y);
        //        //img.ScaleToFit(100f,120f);
        //        PdfContentByte waterMark;
        //        for (int page = 1; page <= pdfReader.NumberOfPages; page++)
        //        {
        //            waterMark = pdfStamper.GetUnderContent(page);
        //            waterMark.AddImage(img);
        //        }
        //        pdfStamper.FormFlattening = true;
        //        pdfStamper.Close();
        //        pdfReader.Close();
        //        //delete old file. No more need of that file.
        //        System.IO.File.Delete(filename);

        //    }

        //    return "";
        //}

        public List<GlobalCodeModel> GetAllFuelType()
        {
            try
            {
                var objeeFleetVehicleRepository = new eFleetVehicleRepository();
                return objeeFleetVehicleRepository.GetAllFuelType();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<GlobalCodeModel> GetAllFuelType()", "Exception While Fetching all Fuel type request.", null);
                throw;
            }
        }

        /// <summary>
        /// Vehicle List Display in a Grid form
        /// created by Ashwajit Bansod
        /// date:08/12/2017
        /// </summary>
        /// <param name="objeFleetVehicleList"></param>
        /// <returns></returns>
        public eFleetVehicleModel GetAllVehicleList(eFleetVehicleModel objeFleetVehicleList)
        {
            return objeFleetVehicleList;
        }

        /// <summary>
        /// Get all Vehicle Data from Database in a List Form
        /// created By Ashwajit Bansod
        /// Date : 08/12/2017
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public eDetails GetListVehicleDetails(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)
        {
            try
            {
                var db = new workorderEMSEntities();
                var objeDetails = new eDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                var Results = db.eFleetVehicles.Where(a => a.IsDeleted == false).Select(a => new eFleetVehicleModel()
                {
                    VehicleID = a.VehicleID,
                    QRCodeID = a.QRCodeID,
                    VehicleIdentificationNo = a.VehicleIdentificationNo,
                    VehicleNumber = a.VehicleNumber,
                    Make = a.Make,
                    ListFuelType = (from gc in db.GlobalCodes where gc.GlobalCodeId == a.FuelType select gc.CodeName).FirstOrDefault(),
                    Model = a.Model,
                    GVWR = a.GVWR,
                    Year = a.Year,
                    StorageAddress = a.StorageAddress,
                    VehicleImage = a.VehicleImage,
                    DummyField = a.DummyField
                }).OrderByDescending(s => s.QRCodeID).ToList();
                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
               //Commented by Bhushan for code optimization.
                //Results = Results.OrderByDescending(s => s.QRCodeID).ToList();

                objeDetails.pageindex = pageindex;
                objeDetails.total = totalPages;
                objeDetails.records = totRecords;
                objeDetails.rows = Results.ToList();
                return objeDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public eDetails GetListVahicleDetails(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)", "Exception While Listing vehicle request.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By ashwajit Bansod for edit vehicle by VehicleID
        /// </summary>
        /// <param name="VehicleID"></param>
        /// <returns></returns>
        public eFleetVehicleModel GetVehicleDetailsById(long VehicleId)
        {
            try
            {
                var db = new workorderEMSEntities();
                var ObjeFleetVehicleRepository = new eFleetVehicleRepository();
                var editVehicleDetails = new eFleetVehicleModel();
                var vehicleDetails = ObjeFleetVehicleRepository.GetSingleOrDefault(u => u.VehicleID == VehicleId);
                if (vehicleDetails.VehicleID > 0)
                {
                    AutoMapper.Mapper.CreateMap<eFleetVehicle, eFleetVehicleModel>();
                    var objfleetVehicleMapper = AutoMapper.Mapper.Map(vehicleDetails, editVehicleDetails);
                    //editVehicleDetails.FuelType = (from gc in db.GlobalCodes where gc.GlobalCodeId == vehicleDetails.FuelType select gc.CodeName).FirstOrDefault();
                    editVehicleDetails.ExpirationDate = vehicleDetails.ExpirationDate;
                    editVehicleDetails.AttachmentOfInsurance = vehicleDetails.AttachmentOfInsurance;
                    editVehicleDetails.AttachmentOfRegistration = vehicleDetails.AttachmentOfRegistration;
                    editVehicleDetails.FuelType = editVehicleDetails.FuelType;
                    editVehicleDetails.DummyField = editVehicleDetails.DummyField;
                    ////  //editVehicleDetails.AttachmentOfInsurance = vehicleDetails.AttachmentOfInsurance == null ? "" : HostingPrefix + ProfilePicPath.Replace("~", "") + vehicleDetails.AttachmentOfInsurance;
                    ////  //editVehicleDetails.AttachmentOfRegistration = vehicleDetails.AttachmentOfRegistration  == null ? "" : HostingPrefix + ProfilePicPath.Replace("~", "") + vehicleDetails.AttachmentOfRegistration;
                    ////  // editVehicleDetails.AttachmentOfRegistration = HostingPrefix + ProfileFilePath.Replace("~", "") + vehicleDetails.AttachmentOfRegistration;
                    editVehicleDetails.VehicleImage = vehicleDetails.VehicleImage == null ? HostingPrefix + VehicleImagePath.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + VehicleImagePath.Replace("~", "") + vehicleDetails.VehicleImage;
                    //editVehicleDetails.AttachmentOfInsuranceFile = vehicleDetails.AttachmentOfInsurance;                 
                }
                return editVehicleDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public eFleetVehicleModel GetVehicleDetailsById(long VehicleId)", "Exception While Editing vehicle request.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By Ashwajit Bansod for Deleting the Vehicle Dated: 08/18/2017
        /// </summary>
        /// <param name="VehicleId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public Result DeleteeFleetVehicle(long VehicleId, long loggedInUserId, string location)
        {
            var objDAR = new DARModel();
            try
            {
                Result result;
                if (VehicleId > 0)
                {
                    eFleetVehicleRepository objeFleetVehicleRepository = new eFleetVehicleRepository();
                    var data = objeFleetVehicleRepository.GetSingleOrDefault(v => v.VehicleID == VehicleId && v.IsDeleted == false);
                    if (data != null)
                    {
                        data.IsDeleted = true;
                        data.DeletedBy = loggedInUserId;
                        data.DeletedDate = DateTime.UtcNow;
                        objeFleetVehicleRepository.Update(data);
                        objeFleetVehicleRepository.SaveChanges();

                        objDAR.ActivityDetails = DarMessage.DeleteFleetVehicle(location);
                        objDAR.TaskType = (long)TaskTypeCategory.DeleteeFleetVehicle;

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

        /// <summary>
        /// Created By - Bhushan Dod
        /// Created Date - 22/Sep/2017
        /// To get the list of vehicle.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<VehicleDetailsModel> GetAllVehicleListDetails(ServiceBaseModel obj)
        {
            try
            {
                var db = new workorderEMSEntities();
                var Results = db.eFleetVehicles.Where(a => a.IsDeleted == false && a.LocationID == obj.LocationID).Select(a => new VehicleDetailsModel()
                {
                    VehicleID = a.VehicleID,
                    QRCodeID = a.QRCodeID,
                    VehicleIdentificationNo = a.VehicleIdentificationNo,
                    VehicleNumber = a.VehicleNumber
                }).ToList<VehicleDetailsModel>();

                return Results;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<eFleetVehicleModel> GetAllVehicleListDetails(ServiceBaseModel obj)", "Exception While Listing vehicle detail.", obj.UserId);
                throw;
            }
        }

        public bool IsVehicleExist(string VehicleNumber)
        {
            var objeFleetVehicle = new eFleetVehicleModel();
            var db = new workorderEMSEntities();
            var VehicleExist = (!db.eFleetVehicles.Any(x => x.VehicleNumber == VehicleNumber));
            return VehicleExist;
        }

        public static string SaveInspectionPDF(string QRCodeID, string htmlData)
        {

            var EncryptVehicleId = Cryptography.GetEncryptedData(QRCodeID.ToString(), true) + DateTime.Now.ToString("yyyyMMddHHmmsstt");
            string filename = HttpContext.Current.Server.MapPath("~/Content/eFleetDocs/Inspection/" + EncryptVehicleId + ".pdf");
            //----------------------------
            Document doc = new Document(PageSize.A4, 30f, 30f, 40f, 30f);
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(filename, FileMode.CreateNew));
            doc.Open();
            try
            {
                BaseFont bf = BaseFont.CreateFont(
            BaseFont.TIMES_ROMAN,
            BaseFont.CP1252,
            BaseFont.EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 11);
                //var content = writer.DirectContent;
                //var pageBorderRect = new iTextSharp.text.Rectangle(doc.PageSize);
                //pageBorderRect.Left += doc.LeftMargin;
                //pageBorderRect.Right -= doc.RightMargin;
                //pageBorderRect.Top -= doc.TopMargin;
                //pageBorderRect.Bottom += doc.BottomMargin;
                //content.SetColorStroke(BaseColor.BLACK);
                //content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
                //content.Stroke();

                //Logo
                string imageURL = HttpContext.Current.Server.MapPath("~/Images/logo-etrac.png");
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                jpg.Alignment = 3;
                jpg.SpacingBefore = 30f;
                jpg.ScaleToFit(100f, 80f);
                jpg.SpacingBefore = 10f;
                jpg.SpacingAfter = 1f;
                doc.Add(jpg);

                //PdfPTable table = new PdfPTable(2);
                //table.WidthPercentage = 96;

                //PdfPCell cell = new PdfPCell(new Phrase("Inspection Date: " + UserName, font));
                //cell.Colspan = 1;
                //cell.Border = 0;
                //cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell);

                //cell = new PdfPCell(new Phrase("Done By: " + UserName, font));
                //cell.Colspan = 1;
                //cell.Border = 0;
                //cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell);
                StyleSheet styles = new StyleSheet();
                styles.LoadStyle("headingalign", "margin-left", "80px");
                // string stylesheet = System.Web.HttpContext.Current.Server.MapPath("~/Content/eFleetDocs/Inspection/Form/InspectionPdfStyle.css");
                foreach (IElement element in HTMLWorker.ParseToList(new StringReader(htmlData), null))
                {
                    doc.Add(element);
                }

                //doc.Add(table);

                return "eTrac" + EncryptVehicleId + ".pdf";
            }
            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public void SaveInspectionPDF(string htmlData, string QRCodeID)", "while creating and saving inspection PDF", QRCodeID);
                throw;
            }
            finally
            {
                doc.Close();
                // Watermark code            
                string watermarkLoc = System.Web.HttpContext.Current.Server.MapPath("~/Images/eTrac380-light.png");
                PdfReader pdfReader = new PdfReader(filename);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(filename.Replace(EncryptVehicleId + ".pdf", "eTrac" + EncryptVehicleId + ".pdf"), FileMode.Create));

                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(watermarkLoc);
                var pageSize = pdfReader.GetPageSizeWithRotation(1);

                var x = pageSize.Width / 2 - img.ScaledWidth / 2;
                var y = pageSize.Height / 2 - img.ScaledHeight / 2;
                img.SetAbsolutePosition(x, y);
                //img.ScaleToFit(100f,120f);
                PdfContentByte waterMark;
                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    waterMark = pdfStamper.GetUnderContent(page);
                    waterMark.AddImage(img);
                }
                pdfStamper.FormFlattening = true;
                pdfStamper.Close();
                pdfReader.Close();
                //delete old file. No more need of that file.
                System.IO.File.Delete(filename);

            }
        }

        //private bool PDFCreationDAREMailforInspection(eFleetDamageTireModel obj, ServiceResponseModel<UpdateEfleetInspectionTypeXML> result)
        //{
        //    try
        //    {
        //        var objEmailLogRepository = new EmailLogRepository();
        //        var objEmailReturn = new List<EmailToManagerModel>();
        //        var objListEmailog = new List<EmailLog>();
        //        if (result.Data.Response == 1)
        //        {
        //            objEmailReturn = objEmailLogRepository.SendEmailToManagerForeFleetInspection(obj.LocationId, obj.UserId).Result;
        //        }

        //        if (result.Data.DamageTireStatus == true && result.Data.EmergencyAccessoriesStatus == true
        //            && result.Data.EngineExteriorStatus == true && result.Data.InteriorMileageDriverStatus == true
        //            && obj.Status == eFleetCheckInOut.PreTrip || obj.Status == eFleetCheckInOut.PostTrip)
        //        {
        //            var objTemplateModel = new TemplateModel();
        //            //Added By Bhushan Dod on 06/09/2017 for Damage in mobile app
        //            if (result.Data.DamageTireDetails != null)
        //            {
        //                var ObjReturnDamageTire = new eFleetDamageTireModel();
        //                ObjReturnDamageTire = GenericDataContractSerializer<eFleetDamageTireModel>.DeserializeXml(result.Data.DamageTireDetails);
        //                objTemplateModel.DamageTire = ObjReturnDamageTire;
        //            }
        //            //Added By Bhushan Dod on 06/09/2017 for Damage in mobile app
        //            if (result.Data.InteriorMileageDriverDetails != null)
        //            {
        //                var objInteriorMileageDriverDetails = new eFleetInteriorMileageDriverModel();
        //                objInteriorMileageDriverDetails = GenericDataContractSerializer<eFleetInteriorMileageDriverModel>.DeserializeXml(result.Data.InteriorMileageDriverDetails);
        //                objTemplateModel.InteriorMileageDriver = objInteriorMileageDriverDetails;
        //            }
        //            //Added By Bhushan Dod on 06/09/2017 for Damage in mobile app
        //            if (result.Data.EngineExteriorDetails != null)
        //            {
        //                var objEngineExteriorDetails = new eFleetEngineExteriorModel();
        //                objEngineExteriorDetails = GenericDataContractSerializer<eFleetEngineExteriorModel>.DeserializeXml(result.Data.EngineExteriorDetails);
        //                objTemplateModel.EngineExterior = objEngineExteriorDetails;
        //            }
        //            //Added By Bhushan Dod on 06/09/2017 for Damage in mobile app
        //            if (result.Data.EmergencyAccessoriesDetails != null)
        //            {
        //                var objEmergencyAccessoriesDetails = new eFleetEmergencyAccessoriesModel();
        //                objEmergencyAccessoriesDetails = GenericDataContractSerializer<eFleetEmergencyAccessoriesModel>.DeserializeXml(result.Data.EmergencyAccessoriesDetails);
        //                objTemplateModel.EmergencyAccessories = objEmergencyAccessoriesDetails;
        //            }
        //            objTemplateModel.Status = obj.Status;
        //            objTemplateModel.UserName = obj.UserName;
        //            objTemplateModel.VehicleNumber = result.Data.VehicleNumber;
        //            objTemplateModel.QRCodeID = result.Data.QRCodeID;
        //            objTemplateModel.TimeZoneName = obj.TimeZoneName;
        //            objTemplateModel.TimeZoneOffset = obj.TimeZoneOffset;
        //            objTemplateModel.IsTimeZoneinDaylight = obj.IsTimeZoneinDaylight;
        //            string htmlData = TemplateDesigner.eFleetTemplate(objTemplateModel);
        //            string returnPath = SaveInspectionPDF(obj.QrcodeId, htmlData);

        //            if (objEmailReturn.Count > 0 && result.Data.Response == 1)
        //            {
        //                foreach (var item in objEmailReturn)
        //                {
        //                    bool IsSent = false;
        //                    var objEmailHelper = new EmailHelper();
        //                    objEmailHelper.emailid = item.ManagerEmail;
        //                    objEmailHelper.ManagerName = item.ManagerName;
        //                    objEmailHelper.VehicleMake = result.Data.Make;
        //                    objEmailHelper.VehicleModel = result.Data.Model;
        //                    objEmailHelper.VehicleIdentificationNumber = result.Data.VehicleNumber;
        //                    objEmailHelper.LocationName = result.Data.LocationName;
        //                    objEmailHelper.UserName = item.UserName;
        //                    objEmailHelper.QrCodeId = obj.QrcodeId;
        //                    objEmailHelper.InfractionStatus = obj.Status;
        //                    objEmailHelper.MailType = "EFLEETINSPECTIONREPORT";
        //                    objEmailHelper.SentBy = item.RequestBy;
        //                    objEmailHelper.LocationID = item.LocationID;
        //                    objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();

        //                    string[] attachFiles = new string[1];
        //                    for (var i = 0; i < attachFiles.Count(); i++)
        //                    {
        //                        attachFiles[i] = HttpContext.Current.Server.MapPath("~/Content/eFleetDocs/Inspection/" + returnPath);
        //                    }
        //                    IsSent = objEmailHelper.SendEmailWithTemplate(attachFiles);

        //                    //Push Notification
        //                    string message = PushNotificationMessages.eFleetInspectionReported(result.Data.LocationName, result.Data.QRCodeID, result.Data.VehicleNumber);
        //                    PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
        //                    if (IsSent == true)
        //                    {
        //                        var objEmailog = new EmailLog();
        //                        try
        //                        {
        //                            objEmailog.CreatedBy = item.RequestBy;
        //                            objEmailog.CreatedDate = DateTime.UtcNow;
        //                            objEmailog.DeletedBy = null;
        //                            objEmailog.DeletedOn = null;
        //                            objEmailog.LocationId = item.LocationID;
        //                            objEmailog.ModifiedBy = null;
        //                            objEmailog.ModifiedOn = null;
        //                            objEmailog.SentBy = item.RequestBy;
        //                            objEmailog.SentEmail = item.ManagerEmail;
        //                            objEmailog.Subject = objEmailHelper.Subject;
        //                            objEmailog.SentTo = item.ManagerUserId;
        //                            objListEmailog.Add(objEmailog);
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            return false;
        //                        }
        //                    }
        //                }
        //                using (var context = new workorderEMSEntities())
        //                {
        //                    context.EmailLogs.AddRange(objListEmailog);
        //                    context.SaveChanges(); ;
        //                }
        //                //var x = EmailLogRepository.InsertEntitiesNew("EmailLog", objListEmailog);
        //                //Task<bool> x = null;
        //                //foreach (var i in objListEmailog)
        //                //{
        //                //    x = objEmailLogRepository.SaveEmailLogAsync(i);
        //                //}
        //            }
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Exception_B.Exception_B.exceptionHandel_Runtime(ex, "ServiceResponseModel<UpdateEfleetInspectionTypeXML> SaveeFleetInspectionRequestDetails(eFleetDamageTireModel obj)", "while changing status", obj);

        //        return false;
        //    }



        //}


    }
}
