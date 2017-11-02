using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.BusinessLogic.Interfaces.eFleet;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.BusinessLogic.Managers.eFleet;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Infrastructure;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;
using WorkOrderEMS.Models.ServiceModel.ApiModel;

namespace WorkOrderEMS.Controllers.Services
{
    public class ServiceApiController : ApiController
    {
        private readonly IeFleetFuelingManager _IFuelingManager;
        private readonly IEfleetPM _IEfleetPM;
        private readonly IEfleetVehicle _IEfleetVehicle;
        private readonly IDARManager _IDARManager;
        private readonly IEfleetVehicleIncidentReport _IEfleetVehicleIncidentReport;
        private readonly IEfleetMaintenance _IEfleetMaintenance;
        private readonly IPassengerTracking _IPassengerTracking;
        private readonly IHoursOfServices _IHoursOfServices;
        //private readonly IEfleetPM _IEfleetPM;
        public ServiceApiController()
        {
        }
        public ServiceApiController(IEfleetPM _IEfleetPM, IeFleetFuelingManager _IFuelingManager, IEfleetVehicle _IEfleetVehicle, IDARManager _IDARManager, IEfleetVehicleIncidentReport _IEfleetVehicleIncidentReport, IEfleetMaintenance _IEfleetMaintenance, IPassengerTracking _IPassengerTracking, IHoursOfServices _IHoursOfServices)
        {
            this._IFuelingManager = _IFuelingManager;
            this._IEfleetPM = _IEfleetPM;
            this._IEfleetVehicle = _IEfleetVehicle;
            this._IDARManager = _IDARManager;
            this._IEfleetVehicleIncidentReport = _IEfleetVehicleIncidentReport;
            this._IEfleetMaintenance = _IEfleetMaintenance;
            this._IPassengerTracking = _IPassengerTracking;
            this._IHoursOfServices = _IHoursOfServices;
        }
        // GET: api/ServiceApi
        public IHttpActionResult Get()
        {
            return Ok("Hello");
        }

        /// <summary>Get eFleetVehicleID Details 
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>GeteFleetVehicleIDdetails</CreatedFor>
        /// <CreatedOn>Sept-06-2017</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceEfleetModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GeteFleetVehicleIDdetails(VehicleScanModel objServiceVehicleModel)
        {
            // var ObjeFleetCommonManager = new eFleetCommonManager();
            ServiceResponseModel<VehicleScanModel> objServiceResponseModel = new ServiceResponseModel<VehicleScanModel>();
            VehicleScanModel ObjVehicleScanModel = new VehicleScanModel();
            try
            {
                if (objServiceVehicleModel.ServiceAuthKey != null && objServiceVehicleModel.LocationID > 0 && objServiceVehicleModel.QRCodeID != null)
                {

                    objServiceResponseModel = _IFuelingManager.GeteFleetVehicleDetailsByID(objServiceVehicleModel);
                }
                else
                {
                    objServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    objServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception ex)
            {
                objServiceResponseModel.Message = ex.Message;
                objServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                objServiceResponseModel.Data = null;
            }

            return Ok(objServiceResponseModel);
        }

        [HttpPost]
        public IHttpActionResult SaveeFleetDamageTireInspection(eFleetDamageTireModel objeFleetDamageTireModel)
        {
            // var ObjVehicleManager = new VehicleManager();
            var ObjServiceResponseModel = new ServiceResponseModel<UpdateEfleetInspectionTypeXML>();
            try
            {
                if (objeFleetDamageTireModel != null && objeFleetDamageTireModel.ServiceAuthKey != null && objeFleetDamageTireModel.UserId > 0)
                {
                    var ObjRespnse = _IEfleetVehicle.SaveeFleetDamageTireInspectionDetails(objeFleetDamageTireModel);
                    ObjRespnse.Data.DamageTireDetails = null;
                    ObjRespnse.Data.InteriorMileageDriverDetails = null;
                    ObjRespnse.Data.EngineExteriorDetails = null;
                    ObjRespnse.Data.EmergencyAccessoriesDetails = null;
                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult SaveeFleetInteriorMileageInspection(eFleetInteriorMileageDriverModel objeFleetInteriorMileageModel)
        {
            //var ObjVehicleManager = new VehicleManager();
            var ObjServiceResponseModel = new ServiceResponseModel<UpdateEfleetInspectionTypeXML>();
            try
            {
                if (objeFleetInteriorMileageModel != null && objeFleetInteriorMileageModel.ServiceAuthKey != null && objeFleetInteriorMileageModel.UserId > 0)
                {
                    var ObjRespnse = _IEfleetVehicle.SaveeFleetInteriorMileageInspectionDetails(objeFleetInteriorMileageModel);
                    ObjRespnse.Data.DamageTireDetails = null;
                    ObjRespnse.Data.InteriorMileageDriverDetails = null;
                    ObjRespnse.Data.EngineExteriorDetails = null;
                    ObjRespnse.Data.EmergencyAccessoriesDetails = null;
                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult SaveeFleetEngineExteriorInspection(eFleetEngineExteriorModel objeFleetEngineExteriorModel)
        {
            // var ObjVehicleManager = new VehicleManager();
            var ObjServiceResponseModel = new ServiceResponseModel<UpdateEfleetInspectionTypeXML>();
            try
            {
                if (objeFleetEngineExteriorModel != null && objeFleetEngineExteriorModel.ServiceAuthKey != null && objeFleetEngineExteriorModel.UserId > 0)
                {
                    var ObjRespnse = _IEfleetVehicle.SaveeFleetEngineExteriorInspectionDetails(objeFleetEngineExteriorModel);
                    if (ObjRespnse.Data != null)
                    {
                        ObjRespnse.Data.DamageTireDetails = null;
                        ObjRespnse.Data.InteriorMileageDriverDetails = null;
                        ObjRespnse.Data.EngineExteriorDetails = null;
                        ObjRespnse.Data.EmergencyAccessoriesDetails = null;
                    }

                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult SaveeFleetEmergencyAccessoriesInspection(eFleetEmergencyAccessoriesModel objeFleetEmergencyAccessoriesModel)
        {
            //var ObjVehicleManager = new VehicleManager();
            var ObjServiceResponseModel = new ServiceResponseModel<UpdateEfleetInspectionTypeXML>();
            try
            {
                if (objeFleetEmergencyAccessoriesModel != null && objeFleetEmergencyAccessoriesModel.ServiceAuthKey != null && objeFleetEmergencyAccessoriesModel.UserId > 0)
                {
                    var ObjRespnse = _IEfleetVehicle.SaveeFleetEmergencyAccessoriesInspectionDetails(objeFleetEmergencyAccessoriesModel);
                    ObjRespnse.Data.DamageTireDetails = null;
                    ObjRespnse.Data.InteriorMileageDriverDetails = null;
                    ObjRespnse.Data.EngineExteriorDetails = null;
                    ObjRespnse.Data.EmergencyAccessoriesDetails = null;
                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult ChangeInspectionCheckOutInStatus(ChangeInspectionStatusModel objChangeInspectionStatusModel)
        {
            // var ObjVehicleManager = new VehicleManager();
            var ObjServiceResponseModel = new ServiceResponseModel<UpdateEfleetInspectionTypeXML>();
            try
            {
                if (objChangeInspectionStatusModel != null && objChangeInspectionStatusModel.ServiceAuthKey != null && objChangeInspectionStatusModel.UserId > 0)
                {
                    var ObjRespnse = _IEfleetVehicle.ChangingStatusOfInsection(objChangeInspectionStatusModel);
                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult SaveeFleetPreventativeMaintenance(eFleetPreventaticeMaintenanceModel objeFleetPreventaticeMaintenanceModel)
        {
            // var ObjPreventativeMaintenaceManager = new PreventativeMaintenaceManager();
            var ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (objeFleetPreventaticeMaintenanceModel != null && objeFleetPreventaticeMaintenanceModel.ServiceAuthKey != null && objeFleetPreventaticeMaintenanceModel.UserId > 0)
                {
                    var ObjRespnse = _IEfleetPM.InsertPreventativeMaintenance(objeFleetPreventaticeMaintenanceModel);
                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetMeterMilesHoursValueList(MeterModel objMeterModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<eFleetMeterModel>>();
            try
            {
                if (objMeterModel != null && objMeterModel.ServiceAuthKey != null && objMeterModel.UserId > 0)
                {
                    // var ObjPreventativeMaintenaceManager = new PreventativeMaintenaceManager();
                    var meterList = _IEfleetPM.GetAllMilesValue();
                    if (meterList.Count > 0)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = meterList;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;

                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult SaveeFleetFueling(eFleetFuelingModelForService objeFleetFuelingModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                // var objeFleetCommonManager = new eFleetCommonManager();
                if (objeFleetFuelingModel != null && objeFleetFuelingModel.ServiceAuthKey != null && objeFleetFuelingModel.UserId > 0)
                {
                    var ObjRespnse = _IFuelingManager.InserteFleetFueling(objeFleetFuelingModel);
                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.InnerException.ToString();
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;

                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult StartFuelingTimer(ServiceDARModel objeFleetDARModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<long>();
            try
            {
                //var objDarManager = new DARManager();
                if (objeFleetDARModel != null && objeFleetDARModel.ServiceAuthKey != null && objeFleetDARModel.UserId > 0)
                {
                    long DARId = _IDARManager.SaveeFleetDAR(objeFleetDARModel);
                    if (DARId > 0)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.InvariantCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = DARId;
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                //ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult SaveeFleetIncident(eFleetIncidentModel objeeFleetIncidentModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                // var objVehicleIncidentManager = new VehicleIncidentManager();
                if (objeeFleetIncidentModel != null && objeeFleetIncidentModel.ServiceAuthKey != null && objeeFleetIncidentModel.UserId > 0)
                {
                    var ObjRespnse = _IEfleetVehicleIncidentReport.InsertVehicleIncident(objeeFleetIncidentModel);
                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;

                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetAllVehicleList(ServiceBaseModel obj)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<VehicleDetailsModel>>();
            try
            {
                if (obj != null && obj.ServiceAuthKey != null && obj.UserId > 0 && obj.LocationID > 0)
                {
                    //  var ObjVehicleManager = new VehicleManager();
                    var vehicleList = _IEfleetVehicle.GetAllVehicleListDetails(obj);
                    if (vehicleList.Count > 0)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = vehicleList;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;

                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult SaveeFleetMaintenance(eFleetMaintenanceModelForApiService objeeFleetMaintenanceModel)
        {
            //var ObjMaintenanceManager = new MaintenanceManager();
            var ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (objeeFleetMaintenanceModel != null && objeeFleetMaintenanceModel.ServiceAuthKey != null && objeeFleetMaintenanceModel.UserId > 0)
                {
                    var ObjRespnse = _IEfleetMaintenance.InsertMaintenance(objeeFleetMaintenanceModel);
                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult GetAllPendingPreventativeMaintenanceList(ServiceBaseModel obj)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<PendingPM>>();
            try
            {
                if (obj != null && obj.ServiceAuthKey != null && obj.UserId > 0 && obj.LocationID > 0)
                {
                    //var ObjPreventativeMaintenaceManager = new PreventativeMaintenaceManager();
                    var PMList = _IEfleetPM.GetAllPendingPMReminderDescription(obj.LocationID);
                    if (PMList.Count > 0)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = PMList;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;

                return Ok(ObjServiceResponseModel);
            }
        }

        public IHttpActionResult UploadFiles()
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<string>>();
            try
            {
                var uploadFolderPath = HostingEnvironment.MapPath("~/Content/eFleetDocs/VehicleIncident/");

                //#region CleaningUpPreviousFiles.InDevelopmentOnly
                //DirectoryInfo directoryInfo = new DirectoryInfo(uploadFolderPath);
                //foreach (FileInfo fileInfo in directoryInfo.GetFiles())
                //	fileInfo.Delete();
                //#endregion

                if (Request.Content.IsMimeMultipartContent())
                {
                    var streamProvider = new WithExtensionMultipartFormDataStreamProvider(uploadFolderPath);
                    var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith<List<string>>(t =>
                    {
                        if (t.IsFaulted || t.IsCanceled)
                        {
                            throw new HttpResponseException(HttpStatusCode.InternalServerError);
                        }

                        var fileInfo = streamProvider.FileData.Select(i =>
                        {
                            var info = new FileInfo(i.LocalFileName);
                            return info.Name;
                        });
                        return fileInfo.ToList();
                    });
                    ObjServiceResponseModel.Data = task.Result.ToList();
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.InvariantCulture);
                    ObjServiceResponseModel.Message = CommonMessage.Successful();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidEntry();
                    ObjServiceResponseModel.Data = null;
                }
                //return Ok(ObjServiceResponseModel);
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                // return Ok(ObjServiceResponseModel);
            }
            return Ok(ObjServiceResponseModel);
        }

        [HttpPost]
        public IHttpActionResult GetAlleFleetPassengerTrackingRouteList(eFleetPassengerTrackingRouteServiceModel obj)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<List<eFleetPassengerTrackingRouteModel>>();
            try
            {
                if (obj != null && obj.ServiceAuthKey != null && obj.UserId > 0 && obj.ServiceType > 0)
                {
                    //var ObjPassengerTrackingManager = new PassengerTrackingManager();
                    var routeList = _IPassengerTracking.GetAllPassengerTrackingRouteDetails(obj);
                    if (routeList.Count > 0)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = routeList;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                    return Ok(ObjServiceResponseModel);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;

                return Ok(ObjServiceResponseModel);
            }
        }

        [HttpPost]
        public IHttpActionResult SaveeFleetPassengerTrackingCount(eFleetPassengerTrackingCountModelForService obj)
        {
            //  var ObjPassengerTrackingManager = new PassengerTrackingManager();
            var ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (obj != null && obj.ServiceAuthKey != null && obj.UserId > 0)
                {
                    var ObjRespnse = _IPassengerTracking.InsertPassengerTrackingCount(obj);
                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : Oct-25-2017
        /// Created for: Saving Hours of services
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveeFleetHourseOfServices(HoursOfServicesModel obj)
        {
           // var ObjHoursOfServicesManager = new HoursOfServicesManager();
            var ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (obj != null && obj.ServiceAuthKey != null && obj.UserId > 0)
                {

                    var ObjRespnse = _IHoursOfServices.InsertHoursOfServices(obj);
                    return Ok(ObjRespnse);
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                    return Ok(ObjServiceResponseModel);
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
                return Ok(ObjServiceResponseModel);
            }
        }
    }
}
