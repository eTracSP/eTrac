using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService" in both code and config file together.
    [ServiceContract]
    public interface IService
    {
        #region For Login

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<eTracLoginModel> ValidateLogin(eTracLoginModel objLogIn);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<eTracLoginModel> LoginLog(eTracLoginModel objLogIn);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> ServiceLogout(eTracLoginModel objLogOut);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> ForgotPassword(eTracLoginModel objETracLoginModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> ChangePassword(eTracLoginModel objETracLoginModel);


        #endregion For Login

        #region for dashboard

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<List<ServiceWorkAssignmentModel>> GetEmployeeTaskList(ServiceDurationModel objServiceDurationModel);
        //ServiceResponseModel GetEmployeeTaskList(ServiceDurationModel ObjServiceDurationModel);

        //NotDelivered 
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<List<UserModel>> GetEmployeeList(UserModel objUserModel);

        //Not Delivered To Mob Team
        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> UpdateTaskStatus(ServiceWorkStatusModel objWorkStatusModel);

        //Not Delivered To Mob Team
        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<List<ServiceWorkAssignmentModel>> GetClientRequestTaskList(ServiceDurationModel objServiceDurationModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<ServiceDashboardModel> DashboardCount(ServiceDashboardModel objServiceDashboardModel);


        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<List<ServiceWorkAssignmentModel>> GetEmployeeContinuousTaskList(ServiceDurationModel objServiceDurationModel);
        #endregion for dashboard

        #region for E-scan QRC Master
        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> QrcElevatorRequestDetails(ServiceQrcElevatorModel objServiceQrcElevatorModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> QrcBathroomRequestDetails(ServiceQrcBathroomModel objServiceQrcBathroomModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> QrcEquipmentRequestDetails(ServiceQrcEquipmentModel objServiceQrcEquipmentModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> QrcCellphoneRequestDetails(ServiceQrcCellphoneModel objServiceQrcCellphoneModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> QrcEscalatorsRequestDetails(ServiceQrcEscalatorsModel objServiceQrcEscalatorsModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> QrcMovingWalkwayRequestDetails(ServiceQrcMovingWalkwayModel objServiceQrcMovingWalkwayModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> QrcParkingRequestDetails(ServiceQrcParkingModel objServiceQrcParkingModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> QrcTrashCanRequestDetails(ServiceQrcTrashcanModel objServiceQrcTrashcanModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> QrcGateArmRequestDetails(ServiceQrcGateArmModel objServiceQrcGateArmModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> QrcTicketSplitterRequestDetails(ServiceQrcTicketSplitterModel objServiceQrcTicketSplitterModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> QrcBusStationRequestDetails(ServiceQrcBusStationModel objServiceQrcBusStationModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> QrcPhoneSystemRequestDetails(ServiceQrcPhoneSystemModel objServiceQrcPhoneSystemModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<QRCModel> GetQrcIdDetails(ServiceQrcModel objServiceQrcModel);


        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> QrcVehicleRequestDetails(ServiceQrcVehicleModel objServiceQrcVehicleModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> CheckoutEmail(ServiceQrcModel objServiceQrcModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> QrcShuttleRequestDetails(ServiceQrcShuttleBusModel objServiceQrcShuttleBusModel);

        #endregion for E-scan QRC Master.

        #region For Generate Work Order Request

        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<ServiceWorkAssignmentModel> SaveWorkRequestOrder(ServiceWorkAssignmentModel objServiceWorkAssignmentModel);

        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<ServiceWorkAssignmentModel> AcceptUrgentWorkRequest(ServiceDARModel objServiceDARModel);

        #endregion For Generate Work Order Request

        #region For Client
        //NotDelivered and also client not sure about client role in android app
        //[WebInvoke(Method = "POST",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        //ServiceResponseModel<string> SaveNewClient(UserModel objUserModel);



        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<List<ServiceUserModel>> GetAllClientList(ServiceUserModel objUserModel);

        #endregion For Client

        #region For DAR

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<ServiceDARModel> SaveDarDetails(ServiceDARModel objServiceDARModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<ServiceDARModel> InsertDarDetailsTracking(ServiceDARModel objServiceDARModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<List<ServiceDARListModel>> GetListofAllDarDetails(ServiceDARListModel objServiceDARListModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> UpdateDarTaskStatus(ServiceDARModel objServiceDARModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<long> DarDisclaimerForm(ServiceDisclaimerModel objServiceDisclaimerModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> UpdateDisclaimerEndTimeStatus(ServiceDARModel objServiceDARModel);
        #endregion For DAR

        #region For Image Upload

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<ServiceImageUpload> ImageUpload(ServiceImageUpload objServiceImageUpload);

        #endregion For Image Upload

        #region For Push Notification

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> SendPushNotification(ServicePushModel objServicePushModel);

        #endregion For Push Notification

        #region for Rule Master

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<List<RuleMasterModelList>> GetListofRuleByLocation(RuleMasterModelList objRuleMasterModelList);

        #endregion for Rule Master

        //Dummy Web Service For testing
        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<ServiceQrcVehicleModel> TestingXmltoObj(ServiceQrcVehicleModel objServiceQrcTrashcanModel);


        #region for eMaintenance

        //[OperationContract]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        //ServiceResponseModel<WorkOrderEMS.Models.eMaintenance_M.eMaintenance_M> GetHTMLForEmaintenance(WorkOrderEMS.Models.eMaintenance_M.eMaintenance_M obj);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<ServiceImageUpload> FacilitySignatureUpload(ServiceImageUpload objServiceImageUpload);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> FeedbackSurvey(ServiceFedbackModel objServiceFedbackModel);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> FacilityRequestTimer(ServiceWorkStatusModel objServiceWorkStatusModel);


        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> CRAlertToEmployee(ServiceWorkStatusModel obj);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> WorkOrderPauseResume(ServiceWorkOrderAcceptanceModel obj);

        #endregion for eMaintenance

        #region for Tracking Employee Idle

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> EmployeeIdleStatus(ServiceWorkStatusModel obj);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> UpdateEmployeeIdleTimeLimit(ServiceWorkStatusModel obj);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare)]
        ServiceResponseModel<string> EquipmentCheckInNotDone(ServiceWorkStatusModel obj);

        #endregion for Tracking Employee Idle
    }
}
