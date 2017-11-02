using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Exception_B;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;
using WorkOrderEMS.Models.eMaintenance_M;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service.svc or Service.svc.cs at the Solution Explorer and start debugging.
    public class Service : IService
    {
        //private readonly ILogin _ILogin;

        //public Service(ILogin _ILogin)
        //{ this._ILogin = _ILogin; }

        #region For Login
        /// <summary>ValidateLogin
        /// <CreatedFor>Validate Service Login</CreatedFor>
        /// <CreatedBy>Bhushan & Nagendra</CreatedBy>
        /// <CreatedOn>Jan-09-2015</CreatedOn>
        /// </summary>
        /// <param name="Login"></param>
        /// <returns></returns>
        public ServiceResponseModel<eTracLoginModel> ValidateLogin(eTracLoginModel objLogIn)
        {
            ServiceResponseModel<eTracLoginModel> serviceresponse = new ServiceResponseModel<eTracLoginModel>();
            LoginManager _ILogin = new LoginManager();
            try
            {
                if (objLogIn != null && objLogIn.UserName != null && objLogIn.Password != null)
                {
                    eTracLoginModel result = _ILogin.AuthenticateUser(objLogIn);

                    // This condition for invalid user
                    // Added By Bhushan Dod on Jan 12 2015
                    serviceresponse.Message = (result != null && !string.IsNullOrEmpty(result.ResponseMessage)) ? result.ResponseMessage : CommonMessage.DoesNotExistsRecordMessage();
                    serviceresponse.Response = (result != null) ? result.Response : 0;
                    serviceresponse.Data = result;
                }
                else
                {

                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = -1;
                serviceresponse.Data = null;
            }
            return serviceresponse;
        }

        /// <summary>ServiceLogout
        /// <CreatedFor>Validate Service Logout</CreatedFor>
        /// <CreatedBy>Bhushan </CreatedBy>
        /// <CreatedOn>Jan-12-2015</CreatedOn>
        /// </summary>
        /// <param name="Login"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> ServiceLogout(eTracLoginModel objLogOut)
        {
            ServiceResponseModel<string> serviceresponse = new ServiceResponseModel<string>();
            LoginManager _ILogin = new LoginManager();
            try
            {
                if (objLogOut != null && objLogOut.ServiceAuthKey != null)
                {

                    eTracLoginModel result = _ILogin.Logout(objLogOut);

                    // This condition for invalid user
                    // Added By Bhushan Dod on Jan 12 2015                
                    serviceresponse.Message = (result != null && !string.IsNullOrEmpty(result.ResponseMessage)) ? result.ResponseMessage : CommonMessage.DoesNotExistsRecordMessage();
                    serviceresponse.Response = (result != null) ? result.Response : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Data = (result != null && !string.IsNullOrEmpty(result.ServiceAuthKey)) ? result.ServiceAuthKey : null;
                }
                else
                {

                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
            }
            return serviceresponse;
        }

        /// <summary>ForgotPassword
        /// <CreatedFor>Forgot Password send email</CreatedFor>
        /// <CreatedBy>Bhushan </CreatedBy>
        /// <CreatedOn>Jan-15-2015</CreatedOn>
        /// </summary>
        /// <param name="EmailID"></param>
        /// <returns>Boolean Value</returns>
        public ServiceResponseModel<string> ForgotPassword(eTracLoginModel objETracLoginModel)
        {
            ServiceResponseModel<string> serviceresponse = new ServiceResponseModel<string>();
            LoginManager _ILogin = new LoginManager();
            bool status = false;
            //status
            string message = "";
            try
            {
                if (objETracLoginModel != null && objETracLoginModel.RecoveryEmail != null && !string.IsNullOrEmpty(objETracLoginModel.RecoveryEmail))
                {
                    //status = _ILogin.RecoveryEmailPassword(eTracLogin, out message, out recoveryPassword);
                    status = _ILogin.RecoveryEmailPassword(objETracLoginModel, out message);
                    if (status) //ViewBag.ForgotPWDModalflag = true;
                    {
                        message = CommonMessage.RecoveryPasswordSent(objETracLoginModel.RecoveryEmail);
                        serviceresponse.Message = message;
                        serviceresponse.Response = (status != null) ? Convert.ToInt32(status) : Convert.ToInt32(ServiceResponse.FailedResponse);
                    }
                    else
                    {
                        serviceresponse.Message = CommonMessage.InvalidUser();
                        serviceresponse.Response = Convert.ToInt32(ServiceResponse.InvalidSessionResponse, CultureInfo.CurrentCulture);
                        serviceresponse.Data = null;
                    }
                }
                else
                {

                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
            }
            return serviceresponse;
        }

        /// <summary>ForgotPassword
        /// <CreatedFor>Forgot Password send email</CreatedFor>
        /// <CreatedBy>Bhushan </CreatedBy>
        /// <CreatedOn>Jan-15-2015</CreatedOn>
        /// </summary>
        /// <param name="EmailID"></param>
        /// <returns>Boolean Value</returns>
        public ServiceResponseModel<string> ChangePassword(eTracLoginModel objETracLoginModel)
        {
            ServiceResponseModel<string> serviceresponse = new ServiceResponseModel<string>();
            LoginManager _ILogin = new LoginManager();
            bool status = false;
            //status
            string message = "";
            try
            {
                if (objETracLoginModel != null && objETracLoginModel.UserId != null && !string.IsNullOrEmpty(objETracLoginModel.OldPassword) && !string.IsNullOrEmpty(objETracLoginModel.NewPassword))
                {
                    status = _ILogin.ChangePassword(objETracLoginModel, out message);
                    if (status) //ViewBag.ForgotPWDModalflag = true;
                    {
                        serviceresponse.Message = message;
                        serviceresponse.Response = (status != null) ? Convert.ToInt32(status) : Convert.ToInt32(ServiceResponse.FailedResponse);
                    }
                    else
                    {
                        serviceresponse.Message = message;
                        serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    }
                }
                else
                {

                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
            }
            return serviceresponse;
        }

        /// <summary>Login Log Maintain
        /// <CreatedFor>For Employee Login Log to get idle time</CreatedFor>
        /// <CreatedBy>Bhushan</CreatedBy>
        /// <CreatedOn>June-25-2015</CreatedOn>
        /// </summary>
        /// <param name="Login"></param>
        /// <returns></returns>
        public ServiceResponseModel<eTracLoginModel> LoginLog(eTracLoginModel objLogIn)
        {
            ServiceResponseModel<eTracLoginModel> serviceresponse = new ServiceResponseModel<eTracLoginModel>();
            LoginManager _ILogin = new LoginManager();
            try
            {

                if (objLogIn != null && objLogIn.UserId != null && objLogIn.LocationID != null && objLogIn.UserRoleId != null)
                {

                    eTracLoginModel result = _ILogin.InsertLoginLog(objLogIn);

                    // This condition for invalid user
                    // Added By Bhushan Dod on Jan 12 2015
                    serviceresponse.Message = (result != null && !string.IsNullOrEmpty(result.ResponseMessage)) ? result.ResponseMessage : CommonMessage.DoesNotExistsRecordMessage();
                    serviceresponse.Response = (result != null) ? result.Response : 0;
                    serviceresponse.Data = result;
                }
                else
                {

                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = -1;
                serviceresponse.Data = null;
            }
            return serviceresponse;
        }
        #endregion For Login

        #region For dashboard
        /// <summary>Dashboard Service
        /// <CreatedFor>List of Assigned Task by Employee</CreatedFor>
        /// <CreatedBy>Bhushan </CreatedBy>
        /// <CreatedOn>Jan-12-2015</CreatedOn>
        /// </summary>
        /// <param name="Login"></param>
        /// <returns></returns> 
        public ServiceResponseModel<List<ServiceWorkAssignmentModel>> GetEmployeeTaskList(ServiceDurationModel objServiceDurationModel)
        {
            ServiceResponseModel<List<ServiceWorkAssignmentModel>> serviceresponse = new ServiceResponseModel<List<ServiceWorkAssignmentModel>>();
            LoginManager objLoginManager = new LoginManager();
            try
            {
                if (objServiceDurationModel != null && objServiceDurationModel.ServiceAuthKey != null && objServiceDurationModel.UserId > 0 && objServiceDurationModel.LocationId > 0)
                {
                    List<ServiceWorkAssignmentModel> result = objLoginManager.GetTaskListByEmployeeId(objServiceDurationModel.ServiceAuthKey, objServiceDurationModel.UserId, objServiceDurationModel.FromDate, objServiceDurationModel.ToDate, objServiceDurationModel.LocationId, objServiceDurationModel.TimeZoneName, objServiceDurationModel.TimeZoneOffset, objServiceDurationModel.IsTimeZoneinDaylight).ToList();

                    serviceresponse.Message = (result != null && result.Count > 0) ? CommonMessage.Successful() : CommonMessage.NoRecordMessage();
                    serviceresponse.Response = (result != null) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Data = result;
                }
                else
                {

                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
            }

            return serviceresponse;
        }

        ///// <summary>Dashboard Service
        ///// <CreatedFor>Change Status of Task</CreatedFor>
        ///// <CreatedBy>Bhushan Dod</CreatedBy>
        ///// <CreatedOn>Jan-16-2015</CreatedOn>
        ///// </summary>
        ///// <param name="Login"></param>
        ///// <returns></returns> 
        //public ServiceResponseModel<string> UpdateTaskStatus(ServiceWorkStatusModel ObjServiceWorkStatusModel)
        //{
        //    ServiceResponseModel<string> serviceresponse = new ServiceResponseModel<string>();
        //    WorkRequestManager objWorkRequestManager = new WorkRequestManager();
        //    try
        //    {
        //        ServiceResponseModel<string> result = objWorkRequestManager.UpdateTaskStatus(ObjServiceWorkStatusModel);

        //        serviceresponse.Message = (result != null && result.Count > 0) ? CommonMessage.Successfull() : CommonMessage.DoesNotExistsRecordMessage();
        //        serviceresponse.Response = (result != null && result.Count > 0) ? Convert.ToInt32(ServiceResponse.SuccessResponse) : Convert.ToInt32(ServiceResponse.FailedResponse);
        //        //serviceresponse.Data = result;                
        //        serviceresponse.Data = result;


        //    }
        //    catch (Exception ex)
        //    {
        //        serviceresponse.Message = ex.Message;
        //        serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse);
        //        serviceresponse.Data = null;
        //    }

        //    return serviceresponse;
        //}

        /// <summary>Dashboard Service
        /// <CreatedFor>List of Assigned Task by Manager</CreatedFor>
        /// <CreatedBy>Bhushan </CreatedBy>
        /// <CreatedOn>Jan-12-2015</CreatedOn>
        /// </summary>
        /// <param name="Login"></param>
        /// <returns></returns> 
        public ServiceResponseModel<List<UserModel>> GetEmployeeList(UserModel objUserModel)
        {
            ServiceResponseModel<List<UserModel>> serviceresponse = new ServiceResponseModel<List<UserModel>>();
            CommonMethodManager objCommonMethodManager = new CommonMethodManager();
            try
            {
                if (objUserModel != null && objUserModel.Location != null)
                {
                    List<UserModel> result = objCommonMethodManager.GetEmployeeListByLocation(objUserModel.Location).ToList();

                    serviceresponse.Message = (result != null && result.Count > 0) ? CommonMessage.Successful() : CommonMessage.DoesNotExistsRecordMessage();
                    serviceresponse.Response = (result != null && result.Count > 0) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    //serviceresponse.Data = result;                
                    serviceresponse.Data = result;
                }
                else
                {

                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
            }

            return serviceresponse;
        }

        /// <summary>Udate status of WorkRequest Assignment
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>UpdateTaskStatus</CreatedFor>
        /// <CreatedOn>Jan-19-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceWorkStatusModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> UpdateTaskStatus(ServiceWorkStatusModel objWorkStatusModel)
        {
            WorkRequestManager ObjWorkRequestManager = new WorkRequestManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                // var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == ObjWorkStatusModel.ServiceAuthKey && x.UserId == ObjWorkStatusModel.UserId);
                if (objWorkStatusModel.ServiceAuthKey != null && objWorkStatusModel.UserId > 0 && objWorkStatusModel.LocationID > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjWorkRequestManager.UpdateTaskStatus(objWorkStatusModel);

                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();

                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        /// <summary>Udate status of WorkRequest Assignment i.e accept work order or client request by employee
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>Accept or Assign work request</CreatedFor>
        /// <CreatedOn>March-04-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceWorkStatusModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> WorkOrderRequestAcceptAssigned(ServiceWorkStatusModel objWorkStatusModel)
        {
            WorkRequestManager ObjWorkRequestManager = new WorkRequestManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                // var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == ObjWorkStatusModel.ServiceAuthKey && x.UserId == ObjWorkStatusModel.UserId);
                if (objWorkStatusModel.ServiceAuthKey != null && objWorkStatusModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjWorkRequestManager.UpdateTaskStatus(objWorkStatusModel);

                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();

                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        /// <summary>Dashboard Service
        /// <CreatedFor>List of Requested Task by Client</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Jan-20-2015</CreatedOn>
        /// </summary>
        /// <param name="Login"></param>
        /// <returns></returns> 
        public ServiceResponseModel<List<ServiceWorkAssignmentModel>> GetClientRequestTaskList(ServiceDurationModel objServiceDurationModel)
        {
            ServiceResponseModel<List<ServiceWorkAssignmentModel>> serviceresponse = new ServiceResponseModel<List<ServiceWorkAssignmentModel>>();
            WorkRequestManager objWorkRequestManager = new WorkRequestManager();
            try
            {
                if (objServiceDurationModel != null && objServiceDurationModel.ServiceAuthKey != null && objServiceDurationModel.LocationId != null)
                {
                    List<ServiceWorkAssignmentModel> result = objWorkRequestManager.GetClientRequestedTaskList(objServiceDurationModel.ServiceAuthKey, objServiceDurationModel.UserId, objServiceDurationModel.FromDate, objServiceDurationModel.ToDate, objServiceDurationModel.LocationId, objServiceDurationModel.TimeZoneName, objServiceDurationModel.TimeZoneOffset, objServiceDurationModel.IsTimeZoneinDaylight).ToList();

                    serviceresponse.Message = (result != null && result.Count > 0) ? CommonMessage.Successful() : CommonMessage.NoRecordMessage();
                    serviceresponse.Response = (result != null) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Data = result;
                }
                else
                {

                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
            }

            return serviceresponse;
        }

        /// <summary>Get Dashboard Details 
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>DashboardCount</CreatedFor>
        /// <CreatedOn>April-21-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCElevatorModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<ServiceDashboardModel> DashboardCount(ServiceDashboardModel objServiceDashboardModel)
        {
            CommonMethodManager ObjCommonMethodManager = new CommonMethodManager();
            ServiceResponseModel<ServiceDashboardModel> objServiceResponseModel = new ServiceResponseModel<ServiceDashboardModel>();
            try
            {

                if (objServiceDashboardModel != null && objServiceDashboardModel.ServiceAuthKey != null && objServiceDashboardModel.UserId != null && objServiceDashboardModel.LocationId != null)
                {
                    objServiceResponseModel = ObjCommonMethodManager.GetCountforDashboard(objServiceDashboardModel);
                }
                else
                {
                    objServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    objServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                objServiceResponseModel.Message = ex.Message;
                objServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                objServiceResponseModel.Data = null;
            }

            return objServiceResponseModel;
        }

        /// <summary>Dashboard Service
        /// <CreatedFor>List of Continuous Task to Employee</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Aug-25-2015</CreatedOn>
        /// </summary>
        /// <param name="Login"></param>
        /// <returns></returns> 
        public ServiceResponseModel<List<ServiceWorkAssignmentModel>> GetEmployeeContinuousTaskList(ServiceDurationModel objServiceDurationModel)
        {
            ServiceResponseModel<List<ServiceWorkAssignmentModel>> serviceresponse = new ServiceResponseModel<List<ServiceWorkAssignmentModel>>();
            LoginManager objLoginManager = new LoginManager();
            try
            {
                if (objServiceDurationModel != null && objServiceDurationModel.ServiceAuthKey != null && objServiceDurationModel.UserId > 0 && objServiceDurationModel.LocationId > 0)
                {
                    List<ServiceWorkAssignmentModel> result = objLoginManager.GetContinuousTaskListByEmployeeId(objServiceDurationModel.ServiceAuthKey, objServiceDurationModel.UserId, objServiceDurationModel.LocationId).ToList();

                    serviceresponse.Message = (result != null && result.Count > 0) ? CommonMessage.Successful() : CommonMessage.NoRecordMessage();
                    serviceresponse.Response = (result != null && result.Count > 0) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                    //serviceresponse.Data = result;                
                    serviceresponse.Data = result;
                }
                else
                {

                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
            }

            return serviceresponse;
        }

        #endregion For dashboard

        #region For E-Scan

        /// <summary>Get QRCID Details 
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>GetQRCIDdetails</CreatedFor>
        /// <CreatedOn>Jan-27-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCElevatorModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<QRCModel> GetQrcIdDetails(ServiceQrcModel objServiceQrcModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<QRCModel> objServiceResponseModel = new ServiceResponseModel<QRCModel>();
            QRCModel ObjQRCModel = new QRCModel();
            try
            {

                if (objServiceQrcModel != null && objServiceQrcModel.ServiceAuthKey != null)
                {
                    objServiceResponseModel = ObjQRCSetupManager.GetQRCDetailsByID(objServiceQrcModel);
                }
                else
                {
                    objServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    objServiceResponseModel.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                objServiceResponseModel.Message = ex.Message;
                objServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                objServiceResponseModel.Data = null;
            }

            return objServiceResponseModel;
        }

        /// <summary>Save QRC Request
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveQRCrequest</CreatedFor>
        /// <CreatedOn>Jan-20-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCElevatorModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> QrcElevatorRequestDetails(ServiceQrcElevatorModel objServiceQrcElevatorModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (objServiceQrcElevatorModel != null && objServiceQrcElevatorModel.ServiceAuthKey != null && objServiceQrcElevatorModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcElevatorRequestDetails(objServiceQrcElevatorModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        /// <summary>Save QRC Request
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveQRCrequest</CreatedFor>
        /// <CreatedOn>Jan-23-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCBathRoomModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> QrcBathroomRequestDetails(ServiceQrcBathroomModel objServiceQrcBathroomModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (objServiceQrcBathroomModel != null && objServiceQrcBathroomModel.ServiceAuthKey != null && objServiceQrcBathroomModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcBathroomRequestDetails(objServiceQrcBathroomModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        /// <summary>Save QRC Request
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveQRCEquipmentrequest</CreatedFor>
        /// <CreatedOn>Jan-23-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCEquipmentModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> QrcEquipmentRequestDetails(ServiceQrcEquipmentModel objServiceQrcEquipmentModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (objServiceQrcEquipmentModel != null && objServiceQrcEquipmentModel.ServiceAuthKey != null && objServiceQrcEquipmentModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcEquipmentRequestDetails(objServiceQrcEquipmentModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        /// <summary>Save QRC Request
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveQRCCellPhonerequest</CreatedFor>
        /// <CreatedOn>Jan-23-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCCellphoneModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> QrcCellphoneRequestDetails(ServiceQrcCellphoneModel objServiceQrcCellphoneModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (objServiceQrcCellphoneModel != null && objServiceQrcCellphoneModel.ServiceAuthKey != null && objServiceQrcCellphoneModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcCellPhoneRequestDetails(objServiceQrcCellphoneModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        /// <summary>Save QRC Request
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveQRCEscalatorsrequest</CreatedFor>
        /// <CreatedOn>Jan-23-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCEscalatorsModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> QrcEscalatorsRequestDetails(ServiceQrcEscalatorsModel objServiceQrcEscalatorsModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (objServiceQrcEscalatorsModel != null && objServiceQrcEscalatorsModel.ServiceAuthKey != null && objServiceQrcEscalatorsModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcEscalatorsRequestDetails(objServiceQrcEscalatorsModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        /// <summary>Save QRC Moving Walkway
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveQRCMovingWalkwayrequest</CreatedFor>
        /// <CreatedOn>Jan-23-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCMovingWalkwayModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> QrcMovingWalkwayRequestDetails(ServiceQrcMovingWalkwayModel objServiceQrcMovingWalkwayModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (objServiceQrcMovingWalkwayModel != null && objServiceQrcMovingWalkwayModel.ServiceAuthKey != null && objServiceQrcMovingWalkwayModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcMovingWalkwayRequestDetails(objServiceQrcMovingWalkwayModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        /// <summary>Save QRC Parking Request
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>QRCParkingRequestDetails</CreatedFor>
        /// <CreatedOn>Jan-28-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCParkingModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> QrcParkingRequestDetails(ServiceQrcParkingModel objServiceQrcParkingModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (objServiceQrcParkingModel != null && objServiceQrcParkingModel.ServiceAuthKey != null && objServiceQrcParkingModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcParkingRequestDetails(objServiceQrcParkingModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        /// <summary>Save QRC Trash Can Request
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>QRCTrashCanRequestDetails</CreatedFor>
        /// <CreatedOn>Jan-28-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCParkingModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> QrcTrashCanRequestDetails(ServiceQrcTrashcanModel objServiceQrcTrashcanModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (objServiceQrcTrashcanModel != null && objServiceQrcTrashcanModel.ServiceAuthKey != null && objServiceQrcTrashcanModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcTrashCanRequestDetails(objServiceQrcTrashcanModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        /// <summary>Save Gate Arm Request
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>QRCGateArmRequestDetails</CreatedFor>
        /// <CreatedOn>Jan-28-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCGateArmModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> QrcGateArmRequestDetails(ServiceQrcGateArmModel objServiceQrcGateArmModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (objServiceQrcGateArmModel != null && objServiceQrcGateArmModel.ServiceAuthKey != null && objServiceQrcGateArmModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcGateArmRequestDetails(objServiceQrcGateArmModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        /// <summary>Save TicketSpitter  Request
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>QRCTicketSpitterRequestDetails</CreatedFor>
        /// <CreatedOn>Jan-28-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCTicketSpitterModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> QrcTicketSplitterRequestDetails(ServiceQrcTicketSplitterModel objServiceQrcTicketSplitterModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (objServiceQrcTicketSplitterModel != null && objServiceQrcTicketSplitterModel.ServiceAuthKey != null && objServiceQrcTicketSplitterModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcTicketSpitterRequestDetails(objServiceQrcTicketSplitterModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        /// <summary>Save BusStation  Request
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>QRCBusStationRequestDetails</CreatedFor>
        /// <CreatedOn>Jan-28-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCBusStationModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> QrcBusStationRequestDetails(ServiceQrcBusStationModel objServiceQrcBusStationModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (objServiceQrcBusStationModel != null && objServiceQrcBusStationModel.ServiceAuthKey != null && objServiceQrcBusStationModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcBusStationRequestDetails(objServiceQrcBusStationModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        /// <summary>Save Emergency Phone Systems  Request
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>QRCPhoneSystemRequestDetails</CreatedFor>
        /// <CreatedOn>Jan-28-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCPhoneSystemModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> QrcPhoneSystemRequestDetails(ServiceQrcPhoneSystemModel objServiceQrcPhoneSystemModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (objServiceQrcPhoneSystemModel != null && objServiceQrcPhoneSystemModel.ServiceAuthKey != null && objServiceQrcPhoneSystemModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcPhoneSystemsRequestDetails(objServiceQrcPhoneSystemModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        /// <summary>Save QRC Vehicle Request
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>QrcVehicleRequestDetails</CreatedFor>
        /// <CreatedOn>Feb-13-2015</CreatedOn>
        /// </summary>
        /// <param name="objServiceQrcVehicleModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> QrcVehicleRequestDetails(ServiceQrcVehicleModel objServiceQrcVehicleModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (objServiceQrcVehicleModel != null && objServiceQrcVehicleModel.ServiceAuthKey != null && objServiceQrcVehicleModel.UserId > 0)
                {
                    // Code for to get path of root directory and attach path of directory to store image
                    //string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                    //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + System.Configuration.ConfigurationManager.AppSettings.GetValues("QRCVehiclePath")[0];
                    //if (objServiceQrcVehicleModel.CheckingOut.IsDamage==true)
                    //{ 
                    //     objServiceQrcVehicleModel.CheckingOut.CroppedPicture = RootDirectory + objServiceQrcVehicleModel.CheckingOut.CroppedPicture;
                    //     objServiceQrcVehicleModel.CheckingOut.CapturedImage = RootDirectory + objServiceQrcVehicleModel.CheckingOut.CapturedImage;
                    //}
                    //if (objServiceQrcVehicleModel.VehicleCheck.IsDamage == true)
                    //{
                    //    objServiceQrcVehicleModel.VehicleCheck.CroppedPicture = RootDirectory + objServiceQrcVehicleModel.VehicleCheck.CroppedPicture;
                    //    objServiceQrcVehicleModel.VehicleCheck.CapturedImage = RootDirectory + objServiceQrcVehicleModel.VehicleCheck.CapturedImage;
                    //}

                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcVehicleRequestDetails(objServiceQrcVehicleModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        /// <summary>Send email and push to manager if checkout already checked by anyone
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>CheckoutEmail</CreatedFor>
        /// <CreatedOn>July-13-2015</CreatedOn>
        /// </summary>
        /// <param name="objServiceQrcModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> CheckoutEmail(ServiceQrcModel objServiceQrcModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (objServiceQrcModel != null && objServiceQrcModel.ServiceAuthKey != null && objServiceQrcModel.LocationId > 0)
                {
                    bool result = ObjQRCSetupManager.SendCheckoutDetailsToManager(objServiceQrcModel);
                    ObjServiceResponseModel.Message = (result == true) ? CommonMessage.Successful() : CommonMessage.InvalidEntry();
                    ObjServiceResponseModel.Response = (result == true) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);

                }

                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        /// <summary>
        /// Created By : Bhushan Dod
        /// Created Date : 07/17/2015
        /// Description : Push notification to the manager if Checked out equipment is not returned within 24 hours.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> EquipmentCheckInNotDone(ServiceWorkStatusModel obj)
        {
            GlobalAdminManager objGlobalAdminManager = new GlobalAdminManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (obj.LocationID > 0 && obj.UserId > 0)
                {
                    bool result = objGlobalAdminManager.EquipmentCheckOutStatus(Convert.ToInt64(obj.LocationID), obj.UserId, obj.UserRole);
                    ObjServiceResponseModel.Message = (result == true) ? CommonMessage.Successful() : CommonMessage.NoRecordMessage();
                    ObjServiceResponseModel.Response = (result == true) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);

                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.InnerException.ToString();
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        /// <summary>Save QRC Shuttle Request
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>QrcShuttleRequestDetails</CreatedFor>
        /// <CreatedOn>May-09-2017</CreatedOn>
        /// </summary>
        /// <param name="objServiceQrcShuttleBusModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> QrcShuttleRequestDetails(ServiceQrcShuttleBusModel objServiceQrcShuttleBusModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (objServiceQrcShuttleBusModel != null && objServiceQrcShuttleBusModel.ServiceAuthKey != null && objServiceQrcShuttleBusModel.UserId > 0)
                {
                    // Code for to get path of root directory and attach path of directory to store image
                    //string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                    //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + System.Configuration.ConfigurationManager.AppSettings.GetValues("QRCVehiclePath")[0];
                    //if (objServiceQrcVehicleModel.CheckingOut.IsDamage==true)
                    //{ 
                    //     objServiceQrcVehicleModel.CheckingOut.CroppedPicture = RootDirectory + objServiceQrcVehicleModel.CheckingOut.CroppedPicture;
                    //     objServiceQrcVehicleModel.CheckingOut.CapturedImage = RootDirectory + objServiceQrcVehicleModel.CheckingOut.CapturedImage;
                    //}
                    //if (objServiceQrcVehicleModel.VehicleCheck.IsDamage == true)
                    //{
                    //    objServiceQrcVehicleModel.VehicleCheck.CroppedPicture = RootDirectory + objServiceQrcVehicleModel.VehicleCheck.CroppedPicture;
                    //    objServiceQrcVehicleModel.VehicleCheck.CapturedImage = RootDirectory + objServiceQrcVehicleModel.VehicleCheck.CapturedImage;
                    //}

                    ServiceResponseModel<string> ObjRespnse = ObjQRCSetupManager.SaveQrcShuttleRequestDetails(objServiceQrcShuttleBusModel);
                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        #endregion For E-Scan

        #region For WorkRequestAssignment

        /// <summary>Save Work Request Assignment
        /// <CreatedFor>For Insert QRC TrashCan work order issue request</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Jan-30-2015</CreatedOn>
        /// </summary>
        /// <param name="objServiceWorkAssignmentModel"></param>
        /// <returns></returns> 
        public ServiceResponseModel<ServiceWorkAssignmentModel> SaveWorkRequestOrder(ServiceWorkAssignmentModel objServiceWorkAssignmentModel)
        {
            ServiceResponseModel<ServiceWorkAssignmentModel> serviceresponse = new ServiceResponseModel<ServiceWorkAssignmentModel>();
            WorkRequestManager objWorkRequestManager = new WorkRequestManager();
            try
            {
                if (objServiceWorkAssignmentModel != null && objServiceWorkAssignmentModel.ServiceAuthKey != null && objServiceWorkAssignmentModel.RequestBy > 0)
                {

                    //Added By Bhushan Dod On 03-02-2015 for client want to add image in work order request
                    string WorkOrderImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["WorkRequestAssignmentPath"].ToString());
                    string ImageUniqueName = string.Empty;
                    string ImageURL = string.Empty;
                    if (objServiceWorkAssignmentModel.WorkRequestImage != null && objServiceWorkAssignmentModel.WorkRequestImage.Trim() != "")
                    {
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmm") + "EMSWorkOrder" + "_" + objServiceWorkAssignmentModel.RequestBy;
                        ImageURL = ImageUniqueName + ".jpg";

                        // Code for to get path of root directory and attach path of directory to store image
                        //string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                        //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + System.Configuration.ConfigurationManager.AppSettings.GetValues("WorkRequestAssignmentPath")[0];

                        if (!Directory.Exists(WorkOrderImagePath))
                        {
                            Directory.CreateDirectory(WorkOrderImagePath);
                        }
                        ///* Code For to check the file is present then delete*/
                        //string[] Files = Directory.GetFiles(rootpath);
                        //foreach (string file in Files)
                        //{
                        //    if (file.ToUpper().Contains(ImageURL.ToUpper()))
                        //    {
                        //        System.IO.File.Delete(file);
                        //    }
                        //}
                        var ImageLocation = WorkOrderImagePath + ImageURL;

                        // Base64ToImage(ObjServiceWorkAssignmentModel.WorkRequestImage);
                        //Save the image to directory
                        //byte[] bytes = Convert.FromBase64String(ObjServiceWorkAssignmentModel.WorkRequestImage);
                        using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(objServiceWorkAssignmentModel.WorkRequestImage)))
                        {
                            using (Bitmap bm2 = new Bitmap(ms))
                            {
                                bm2.Save(ImageLocation);
                                objServiceWorkAssignmentModel.WorkRequestImage = ImageURL;

                            }
                        }
                    }

                    ServiceWorkAssignmentModel result = objWorkRequestManager.SaveWorkOrderRequest(objServiceWorkAssignmentModel);

                    serviceresponse.Message = (result != null && !string.IsNullOrEmpty(result.ResponseMessage)) ? result.ResponseMessage : CommonMessage.DoesNotExistsRecordMessage();
                    serviceresponse.Response = (result != null) ? result.Response : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Data = result;
                }
                else
                {
                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
            }

            return serviceresponse;
        }

        /// <summary>
        /// Created by vijay sahu on 18 may 2015
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //public ServiceResponseModel<WorkOrderEMS.Models.eMaintenance_M.eMaintenance_M> GetHTMLForEmaintenance(WorkOrderEMS.Models.eMaintenance_M.eMaintenance_M obj)
        //{
        //    ServiceResponseModel<WorkOrderEMS.Models.eMaintenance_M.eMaintenance_M> serviceresponse = new ServiceResponseModel<WorkOrderEMS.Models.eMaintenance_M.eMaintenance_M>();
        //    try
        //    {
        //        if (obj == null)
        //        {
        //            serviceresponse.Response = 0;
        //        }
        //        else
        //        {
        //            if (obj.WorkRequestAssignmentRequestId == 0)
        //            {
        //                serviceresponse.Message = "Please check the value for 'WorkRequestAssignmentRequestId' field";
        //                serviceresponse.Response = 2;
        //            }
        //            else
        //            {

        //                WorkOrderEMS.Models.eMaintenance_M.WorkRequestAssignment_M objData = new WorkOrderEMS.Models.eMaintenance_M.WorkRequestAssignment_M();
        //                GlobalAdminManager objM = new GlobalAdminManager();
        //                objData = objM.GetDataForRendringHTML(obj.WorkRequestAssignmentRequestId);
        //                var htmlData = TemplateDesigner.eMaintenanceTemplate(DateTime.Now.Day.ToString(), DateTime.Now.Year.ToString(), DateTime.Now.Date.ToString(), objData.DriverLicenseNo, objData.CustomerName, objData.Address, objData.CustomerContact, "");

        //                serviceresponse.Data = new Models.eMaintenance_M.eMaintenance_M();

        //                serviceresponse.Data.htmlContent = htmlData;
        //                serviceresponse.Response = 1;
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //    return serviceresponse;
        //}

        /// <summary>Save Work Request Assignment
        /// <CreatedFor>For Save Image Of Facility request</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>May-20-2015</CreatedOn>
        /// </summary>
        /// <param name="objServiceImageUpload"></param>
        /// <returns></returns> 
        public ServiceResponseModel<ServiceImageUpload> FacilitySignatureUpload(ServiceImageUpload objServiceImageUpload)
        {
            ServiceResponseModel<ServiceImageUpload> serviceresponse = new ServiceResponseModel<ServiceImageUpload>();
            ServiceImageUpload imgupload = new ServiceImageUpload();
            WorkRequestManager objWorkRequestManager = new WorkRequestManager();
            WorkRequestAssignment_M objData = new WorkRequestAssignment_M();
            GlobalAdminManager objM = new GlobalAdminManager();
            bool st;
            try
            {
                if (objServiceImageUpload != null
                    && objServiceImageUpload.Image != null
                    && objServiceImageUpload.UserId > 0
                    && objServiceImageUpload.Image.Trim() != ""
                    && objServiceImageUpload.ImageModuleName != null
                    && objServiceImageUpload.ImageEmp != null
                    && objServiceImageUpload.ImageEmp.Trim() != ""
                    && objServiceImageUpload.ImageModuleNameEmp != null)
                {
                    string WorkOrderImagePath = string.Empty;
                    string ImageUniqueName = string.Empty;
                    string ImageURL = string.Empty;
                    string ImageUniqueNameEmp = string.Empty;
                    string ImageURLEmp = string.Empty;

                    //Added By Bhushan Dod On 17-04-2015 for For Facilty Request Disclaimer Signature
                    #region For Facilty Request Disclaimer Signature
                    if (objServiceImageUpload.ImageModuleName == "FacilityRequestSign")
                    {
                        WorkOrderImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["FRSignature"].ToString());
                        ImageUniqueName = objServiceImageUpload.WorkAssignmentId + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "_" + objServiceImageUpload.UserId;
                    }
                    #endregion For Facilty Request Disclaimer Signature

                    //Added By Bhushan Dod On 25-07-2015 for For Facilty Request Employee or Manager Signature
                    #region For Facilty Request Employee or Manager Signature
                    if (objServiceImageUpload.ImageModuleNameEmp == "FacilitySignEmp")
                    {
                        WorkOrderImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["FRSignature"].ToString());
                        ImageUniqueNameEmp = objServiceImageUpload.UserId + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "_" + objServiceImageUpload.WorkAssignmentId;
                    }
                    #endregion For Facilty Request Employee or Manager Signature
                    ImageURL = ImageUniqueName + ".jpg";
                    ImageURLEmp = ImageUniqueNameEmp + ".jpg";

                    // Code for to get path of root directory and attach path of directory to store image
                    //string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                    //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + WorkOrderImagePath;
                    if (!Directory.Exists(WorkOrderImagePath))
                    {
                        Directory.CreateDirectory(WorkOrderImagePath);
                    }
                    var ImageLocation = WorkOrderImagePath + ImageURL;
                    var ImageLocationEmp = WorkOrderImagePath + ImageURLEmp;

                    //Save the image to directory
                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(objServiceImageUpload.Image)))
                    {
                        using (Bitmap bm2 = new Bitmap(ms))
                        {
                            bm2.Save(ImageLocation);
                            imgupload.Image = ImageURL;
                            imgupload.ImageUrl = ImageLocation;
                        }
                    }
                    //Save the image to directory
                    using (MemoryStream ms1 = new MemoryStream(Convert.FromBase64String(objServiceImageUpload.ImageEmp)))
                    {
                        using (Bitmap bm21 = new Bitmap(ms1))
                        {
                            bm21.Save(ImageLocationEmp);

                            imgupload.ImageEmp = ImageURLEmp;
                            imgupload.ImageUrl = ImageLocationEmp;
                        }
                    }

                    //objData.StartTime, objData.EndTime, objData.AssignedToName);

                    // htmlData = htmlData.Replace("\r\n", "").Replace("\0", "");
                    var EncryptWorkId = Cryptography.GetEncryptedData(objServiceImageUpload.WorkAssignmentId.ToString(), true);
                    EncryptWorkId = EncryptWorkId.Replace('/', '@');//Here @ char replace due to '/' encrypt id it break the URL to open file
                    string filename = HttpContext.Current.Server.MapPath("~/Content/eMaintenance/DisclaimerDownload/" + EncryptWorkId + ".pdf");
                    // HttpContext.Current.Server.MapPath("~") + "/Content/eMaintenance/DisclaimerDownload/" + EncryptWorkId + ".pdf";
                    string waterMarkDisclaimerFilename = "eTrac" + EncryptWorkId + ".pdf";
                    st = objWorkRequestManager.WorkFrSignature(objServiceImageUpload.WorkAssignmentId, imgupload.Image, imgupload.ImageEmp, waterMarkDisclaimerFilename, "","");
                    //Commented due to watermark. earlier we saved real file name but now we save watermark conversion file name. Because while adding watermark we need to delete disclaimer form and add dummy file with watermark.
                    //st = objWorkRequestManager.WorkFrSignature(objServiceImageUpload.WorkAssignmentId, imgupload.Image, imgupload.ImageEmp, EncryptWorkId + ".pdf", "");

                    if (st)
                    {
                        objData = objM.GetDataForRendringHTML(objServiceImageUpload.WorkAssignmentId);
                        //if (objData.StartTime != null && objData.EndTime != null)
                        //{
                        //    imgupload.StartTime = objData.StartTime.Value.ToMobileClientTimeZone(true);
                        //    imgupload.EndTime = objData.EndTime.Value.ToMobileClientTimeZone(true);

                        //    TimeSpan ts = objData.EndTime.Value - objData.StartTime.Value;

                        //    imgupload.TotalTime = ts.Days + "Days:" + ts.Hours + "Hours:" + ts.Minutes + "Minutes";
                        //}
                        var htmlData = TemplateDesigner.eMaintenanceTemplate(objData.LicensePlateNo, objData.CustomerName, objData.Address, objData.CustomerContact,
                                                                            imgupload.Image, imgupload.ImageEmp, objData.CurrentLocation, objData.VehicleMake,
                                                                            objData.VehicleYear, objData.DriverLicenseNo, objData.VehicleModel, objData.FacilityRequestName,
                                                                            objServiceImageUpload.TimeZoneName, objServiceImageUpload.TimeZoneOffset, objServiceImageUpload.IsTimeZoneinDaylight);
                        //----------------------------
                        Document doc = new Document(PageSize.A4, 30f, 30f, 40f, 30f);
                        iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(filename, FileMode.CreateNew));
                        doc.Open();
                        try
                        {
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

                            foreach (IElement element in HTMLWorker.ParseToList(
                            new StringReader(htmlData), null))
                            {
                                doc.Add(element);
                            }

                            PdfPTable table = new PdfPTable(2);

                            table.WidthPercentage = 96;
                            BaseFont bf = BaseFont.CreateFont(
                                        BaseFont.TIMES_ROMAN,
                                        BaseFont.CP1252,
                                        BaseFont.EMBEDDED);
                            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 11);

                            //PdfPCell cell = new PdfPCell(new Phrase("Start Time: " + imgupload.StartTime, font));
                            //cell.Colspan = 1;
                            //cell.Border = 0;
                            //cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            //table.AddCell(cell);

                            //cell = new PdfPCell(new Phrase("End Time: " + imgupload.EndTime, font));
                            //cell.Colspan = 1;
                            //cell.Border = 0;
                            //cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            //table.AddCell(cell);

                            PdfPCell cell = new PdfPCell(new Phrase("Manager or Employee Name: " + objData.AssignedFirstName + ' ' + objData.AssignedLastName, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Drivers Licence Number: " + objData.DriverLicenseNo, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Customer Name: " + objData.CustomerName, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Customer Address: " + objData.Address, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Customer Telephone: " + objData.CustomerContact, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            imageURL = HttpContext.Current.Server.MapPath("~/Content/Images/FRSignature/" + imgupload.Image);
                            jpg = iTextSharp.text.Image.GetInstance(imageURL);
                            jpg.ScaleToFit(140f, 120f);
                            jpg.SpacingBefore = 10f;
                            jpg.SpacingAfter = 1f;

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                            cell.AddElement(jpg);
                            table.AddCell(cell);

                            imageURL = HttpContext.Current.Server.MapPath("~/Content/Images/FRSignature/" + imgupload.ImageEmp);
                            jpg = iTextSharp.text.Image.GetInstance(imageURL);
                            jpg.ScaleToFit(140f, 120f);
                            jpg.SpacingBefore = 10f;
                            jpg.SpacingAfter = 1f;

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                            cell.AddElement(jpg);
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(objData.CustomerName, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(objData.AssignedFirstName + ' ' + objData.AssignedLastName, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Signature of Customer", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Signature of Manager or Employee", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            doc.Add(table);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            doc.Close();
                            // Watermark code            
                            string watermarkLoc = System.Web.HttpContext.Current.Server.MapPath("~/Images/eTrac380-light.png");
                            PdfReader pdfReader = new PdfReader(filename);
                            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(filename.Replace(EncryptWorkId + ".pdf", waterMarkDisclaimerFilename), FileMode.Create));

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
                    serviceresponse.Message = (imgupload.ImageUrl != "" && !string.IsNullOrEmpty(imgupload.ImageUrl) && st == true) ? CommonMessage.Successful() : CommonMessage.FailureMessage();
                    serviceresponse.Response = (imgupload.ImageUrl != "" && !string.IsNullOrEmpty(imgupload.ImageUrl) && st == true) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Data = imgupload;
                }
                else if (objServiceImageUpload != null &&
                    objServiceImageUpload.IsDecline == true &&
                    objServiceImageUpload.UserId > 0 &&
                    objServiceImageUpload.WorkAssignmentId > 0)
                {
                    bool status = objWorkRequestManager.WorkFrIsDecline(objServiceImageUpload.WorkAssignmentId, objServiceImageUpload.UserId, objServiceImageUpload.IsDecline);
                    serviceresponse.Message = (status == true) ? CommonMessage.Successful() : CommonMessage.FailureMessage();
                    serviceresponse.Response = (status == true) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                }
                else
                {
                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                Exception_B.exceptionHandel_Runtime(ex, "ServiceResponseModel<ServiceImageUpload> FacilitySignatureUpload(ServiceImageUpload objServiceImageUpload)", "objServiceImageUpload.UserId", objServiceImageUpload.UserId);
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
            }
            return serviceresponse;
        }

        public ServiceResponseModel<string> FeedbackSurvey(ServiceFedbackModel objServiceFedbackModel)
        {
            ServiceResponseModel<string> serviceresponse = new ServiceResponseModel<string>();
            WorkRequestManager objWorkRequestManager = new WorkRequestManager();
            try
            {
                if (objServiceFedbackModel != null && objServiceFedbackModel.Email != null && objServiceFedbackModel.UserId > 0)
                {
                    bool st = objWorkRequestManager.FeedbackEmailToEmployee(objServiceFedbackModel);
                    serviceresponse.Message = (st == true) ? CommonMessage.Successful() : CommonMessage.DoesNotExistsRecordMessage();
                    serviceresponse.Response = (st == true) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                }
                else
                {
                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
            }
            return serviceresponse;
        }

        public ServiceResponseModel<string> FacilityRequestTimer(ServiceWorkStatusModel objServiceWorkStatusModel)
        {
            ServiceResponseModel<string> serviceresponse = new ServiceResponseModel<string>();

            WorkRequestAssignmentModel objWork = new WorkRequestAssignmentModel();
            WorkRequestManager objWorkRequestManager = new WorkRequestManager();
            try
            {
                if (objServiceWorkStatusModel != null && objServiceWorkStatusModel.WorkRequestAssignmentID != null && objServiceWorkStatusModel.WorkRequestAssignmentID > 0)
                {
                    bool st = objWorkRequestManager.GetFacilityRequestByID(objServiceWorkStatusModel.WorkRequestAssignmentID, objServiceWorkStatusModel.LocationID, objServiceWorkStatusModel.UserId);
                    serviceresponse.Message = (st == true) ? CommonMessage.Successful() : CommonMessage.DoesNotExistsRecordMessage();
                    serviceresponse.Response = (st == true) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                }
                else
                {
                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
            }
            return serviceresponse;
        }

        /// <summary>Get the alert of CR on time
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>CRAlert</CreatedFor>
        /// <CreatedOn>June-09-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceWorkStatusModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> CRAlertToEmployee(ServiceWorkStatusModel obj)
        {
            WorkRequestManager objWorkRequestManager = new WorkRequestManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                // var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == ObjWorkStatusModel.ServiceAuthKey && x.UserId == ObjWorkStatusModel.UserId);
                if (obj.LocationID > 0 && obj.UserId > 0)
                {
                    bool result = objWorkRequestManager.CREmployeeAlert(obj.LocationID, obj.UserId, obj.TimeZoneName, obj.TimeZoneOffset,obj.IsTimeZoneinDaylight);

                    ObjServiceResponseModel.Message = (result == true) ? CommonMessage.Successful() : CommonMessage.NoRecordMessage();
                    ObjServiceResponseModel.Response = (result == true) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);

                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        /// <summary>Pause and Resume of work order
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>Work Order Pause/Resume</CreatedFor>
        /// <CreatedOn>Oct-24-2015</CreatedOn>
        /// </summary>
        /// <param name="ServiceWorkOrderAcceptanceModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> WorkOrderPauseResume(ServiceWorkOrderAcceptanceModel obj)
        {
            WorkRequestManager objWorkRequestManager = new WorkRequestManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (obj.LocationID > 0 && obj.UserId > 0)
                {
                    bool result = objWorkRequestManager.WorkOrderPauseResume(obj);

                    ObjServiceResponseModel.Message = (result == true) ? CommonMessage.Successful() : CommonMessage.NoRecordMessage();
                    ObjServiceResponseModel.Response = (result == true) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);

                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.InnerException.ToString();
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        /// <summary>
        /// Created By- Bhushan Dod
        /// Created Date - 04/02/2017
        /// Accept urgent work request after click on accept from push notification.
        /// </summary>
        /// <param name="objServiceDARModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<ServiceWorkAssignmentModel> AcceptUrgentWorkRequest(ServiceDARModel objServiceDARModel)
        {
            ServiceResponseModel<ServiceWorkAssignmentModel> serviceresponse = new ServiceResponseModel<ServiceWorkAssignmentModel>();
            WorkRequestManager objWorkRequestManager = new WorkRequestManager();
            try
            {
                if (objServiceDARModel != null &&
                    objServiceDARModel.ServiceAuthKey != null &&
                    objServiceDARModel.UserName != null &&
                    objServiceDARModel.UserId > 0
                    )
                {
                    ServiceWorkAssignmentModel result = objWorkRequestManager.UrgentWOAccpetedByEmployee(objServiceDARModel);

                    //serviceresponse.Message = (result != null && !string.IsNullOrEmpty(result.ResponseMessage)) ? result.ResponseMessage : CommonMessage.DoesNotExistsRecordMessage();
                    //serviceresponse.Response = (result != null) ? result.Response : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = result.ResponseMessage;
                    serviceresponse.Response = result.Response;
                    serviceresponse.Data = result;
                }
                else
                {
                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
            }

            return serviceresponse;
        }

        #endregion For WorkRequestAssignment

        #region For Client
        //This portion is skipped by client
        ///// <summary>Save New Client
        ///// <CreatedBy>Bhushan Dod</CreatedBY>
        ///// <CreatedFor>Add New Client</CreatedFor>
        ///// <CreatedOn>Feb-12-2015</CreatedOn>
        ///// </summary>
        ///// <param name="ObjServiceQRCElevatorModel"></param>
        ///// <returns></returns>
        //public ServiceResponseModel<string> SaveNewClient(UserModel objUserModel)
        //{
        //    ClientManager ObjClientManager = new ClientManager();
        //    ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
        //    try
        //    {

        //        if (objUserModel != null && objUserModel.ServiceAuthKey != null && objUserModel.UserId > 0)
        //        {
        //            ServiceResponseModel<string> ObjRespnse = ObjClientManager.AddNewClient(objUserModel);
        //            ObjServiceResponseModel.Response = ObjRespnse.Response;
        //            ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();
        //        }
        //        else
        //        {
        //            ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
        //            ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ObjServiceResponseModel.Message = ex.Message;
        //        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
        //        ObjServiceResponseModel.Data = null;
        //    }

        //    return ObjServiceResponseModel;
        //}

        /// <summary>Get all client details
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>ClientList</CreatedFor>
        /// <CreatedOn>Feb-12-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjUserModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<List<ServiceUserModel>> GetAllClientList(ServiceUserModel objUserModel)
        {
            ServiceResponseModel<List<ServiceUserModel>> serviceresponse = new ServiceResponseModel<List<ServiceUserModel>>();
            ClientManager objClientManager = new ClientManager();
            try
            {
                if (objUserModel != null && objUserModel.ServiceAuthKey != null && objUserModel.UserId != null)
                {
                    serviceresponse = objClientManager.GetAllClientList(objUserModel);

                    //serviceresponse.Message = (result != null || result.Count > 0) ? CommonMessage.Successful() : CommonMessage.DoesNotExistsRecordMessage();
                    //serviceresponse.Response = (result != null || result.Count > 0) ? Convert.ToInt32(ServiceResponse.FailedResponse) : Convert.ToInt32(ServiceResponse.FailedResponse);
                    //serviceresponse.Data = result;
                }
                else
                {
                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
            }

            return serviceresponse;
        }
        #endregion For Client

        #region For DAR

        /// <summary>Save DAR for QRC Type and Shift End for the day
        /// <CreatedFor>For Insert QRC Vehicle</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Feb-13-2015</CreatedOn>
        /// <ModifiedOn>June-02-2015</ModifiedOn>
        /// </summary>
        /// <param name="objServiceDARModel"></param>
        /// <returns></returns> 
        public ServiceResponseModel<ServiceDARModel> SaveDarDetails(ServiceDARModel objServiceDARModel)
        {
            ServiceResponseModel<ServiceDARModel> serviceresponse = new ServiceResponseModel<ServiceDARModel>();
            DARManager objDARManager = new DARManager();
            try
            {
                if (objServiceDARModel != null &&
                    objServiceDARModel.ServiceAuthKey != null &&
                    objServiceDARModel.UserName != null &&
                    objServiceDARModel.UserId > 0
                    )
                {
                    ServiceDARModel result = objDARManager.SaveDARDetails(objServiceDARModel);

                    serviceresponse.Message = (result != null && !string.IsNullOrEmpty(result.ResponseMessage)) ? result.ResponseMessage : CommonMessage.DoesNotExistsRecordMessage();
                    serviceresponse.Response = (result != null) ? result.Response : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Data = result;
                }
                else
                {
                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
            }

            return serviceresponse;
        }

        /// <summary>Save DAR for Jump Start
        /// <CreatedFor>For Insert Jump Start and GT Tracker</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>March-16-2015</CreatedOn>
        /// </summary>
        /// <param name="objServiceDARModel"></param>
        /// <returns></returns> 
        public ServiceResponseModel<ServiceDARModel> InsertDarDetailsTracking(ServiceDARModel objServiceDARModel)
        {
            ServiceResponseModel<ServiceDARModel> serviceresponse = new ServiceResponseModel<ServiceDARModel>();
            DARManager objDARManager = new DARManager();
            try
            {
                if (objServiceDARModel != null &&
                    objServiceDARModel.ServiceAuthKey != null &&
                    objServiceDARModel.UserName != null &&
                    objServiceDARModel.UserId > 0 &&
                    objServiceDARModel.StartTime != null
                    )
                {
                    ServiceDARModel result = objDARManager.SaveDARDetailsForTracking(objServiceDARModel);

                    serviceresponse.Message = (result != null && !string.IsNullOrEmpty(result.ResponseMessage)) ? result.ResponseMessage : CommonMessage.DoesNotExistsRecordMessage();
                    serviceresponse.Response = (result != null) ? result.Response : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Data = result;
                }
                else
                {
                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
            }
            return serviceresponse;
        }

        /// <summary>Get all details
        /// <CreatedFor>DAR Listing</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>March-05-2015</CreatedOn>
        /// </summary>
        /// <param name="objServiceDARModel"></param>
        /// <returns></returns> 
        public ServiceResponseModel<List<ServiceDARListModel>> GetListofAllDarDetails(ServiceDARListModel objServiceDARListModel)
        {
            ServiceResponseModel<List<ServiceDARListModel>> serviceresponse = new ServiceResponseModel<List<ServiceDARListModel>>();
            DARManager objDARManager = new DARManager();
            try
            {
                if (objServiceDARListModel != null && objServiceDARListModel.ServiceAuthKey != null)
                {

                    serviceresponse = objDARManager.GetAllDARDetails(objServiceDARListModel);
                }
                else
                {

                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
            }

            return serviceresponse;
        }

        /// <summary>Update the status of DAR jump start
        /// <CreatedFor>For Update Jump Start and GT Tracker</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>March-16-2015</CreatedOn>
        /// </summary>
        /// <param name="objServiceDARModel"></param>
        /// <returns></returns> 
        public ServiceResponseModel<string> UpdateDarTaskStatus(ServiceDARModel objServiceDARModel)
        {
            DARManager ObjDARManager = new DARManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                // var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == ObjWorkStatusModel.ServiceAuthKey && x.UserId == ObjWorkStatusModel.UserId);
                if (objServiceDARModel.ServiceAuthKey != null && objServiceDARModel.UserId > 0)
                {
                    ServiceResponseModel<string> ObjRespnse = ObjDARManager.UpdateDarTaskStatus(objServiceDARModel);

                    ObjServiceResponseModel.Response = ObjRespnse.Response;
                    ObjServiceResponseModel.Message = ObjRespnse.Message;//CommonMessage.MessageLogout();

                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        /// <summary>Save DAR Disclaimer form
        /// <CreatedFor>to save disclaimer form from mobile</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>May-26-2017</CreatedOn>
        /// </summary>
        /// <param name="objServiceDisclaimerModel"></param>
        /// <returns></returns> 
        public ServiceResponseModel<long> DarDisclaimerForm(ServiceDisclaimerModel objServiceDisclaimerModel)
        {
            ServiceResponseModel<long> serviceresponse = new ServiceResponseModel<long>();          
            ServiceDARModel objServiceDARModel = new ServiceDARModel();
            DARManager objDARManager = new DARManager();
           // WorkRequestAssignment_M objData = new WorkRequestAssignment_M();
            GlobalAdminManager objM = new GlobalAdminManager();     
            try
            {
                if (objServiceDisclaimerModel != null
                    && objServiceDisclaimerModel.ImageCust != null
                    && objServiceDisclaimerModel.UserId > 0
                    && objServiceDisclaimerModel.ImageCust.Trim() != ""
                    && objServiceDisclaimerModel.ImageModuleNameCust != null
                    && objServiceDisclaimerModel.ImageEmp != null
                    && objServiceDisclaimerModel.ImageEmp.Trim() != ""
                    && objServiceDisclaimerModel.ImageModuleNameEmp != null)
                {
                    string DARImagePath = string.Empty;
                    string ImageUniqueNameCust = string.Empty;
                    string ImageURL = string.Empty;
                    string ImageUniqueNameEmp = string.Empty;
                    string ImageURLEmp = string.Empty;

                    //Added By Bhushan Dod On 25-05-2017 for DAR Employee Disclaimer Signature
                    #region For DAR Employee Disclaimer Signature
                    if (objServiceDisclaimerModel.ImageModuleNameEmp == "DarDisclaimerEmpSign")
                    {
                        DARImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DARSignature"].ToString());
                        ImageUniqueNameEmp = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + objServiceDisclaimerModel.UserId;
                    }
                    #endregion For DAR Employee Disclaimer Signature

                    //Added By Bhushan Dod On 25-07-2015 for DAR Customer Disclaimer Signature
                    #region For DAR Customer Disclaimer Signature
                    if (objServiceDisclaimerModel.ImageModuleNameCust == "DarDisclaimerCustomerSign")
                    {
                        DARImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DARSignature"].ToString());
                        ImageUniqueNameCust  = objServiceDisclaimerModel.UserId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    }
                    #endregion For DAR Customer Disclaimer Signature
                    ImageURL = ImageUniqueNameCust + ".jpg";
                    ImageURLEmp = ImageUniqueNameEmp + ".jpg";

                    // Code for to get path of root directory and attach path of directory to store image
                    //string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                    //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + WorkOrderImagePath;
                    if (!Directory.Exists(DARImagePath))
                    {
                        Directory.CreateDirectory(DARImagePath);
                    }
                    var ImageLocationCust = DARImagePath + ImageURL;
                    var ImageLocationEmp = DARImagePath + ImageURLEmp;

                    //Save the image to directory
                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(objServiceDisclaimerModel.ImageCust)))
                    {
                        using (Bitmap bm2 = new Bitmap(ms))
                        {
                            bm2.Save(ImageLocationCust);
                            objServiceDisclaimerModel.ImageCust = ImageURL;
                            objServiceDisclaimerModel.ImageUrlCust = ImageLocationCust;
                        }
                    }
                    //Save the image to directory
                    using (MemoryStream ms1 = new MemoryStream(Convert.FromBase64String(objServiceDisclaimerModel.ImageEmp)))
                    {
                        using (Bitmap bm21 = new Bitmap(ms1))
                        {
                            bm21.Save(ImageLocationEmp);

                            objServiceDisclaimerModel.ImageEmp = ImageURLEmp;
                            objServiceDisclaimerModel.ImageUrlEmp = ImageLocationEmp;
                        }
                    }

                    string filename = HttpContext.Current.Server.MapPath("~/Content/DARDisclaimer/" + ImageUniqueNameCust + ".pdf");
                    objServiceDisclaimerModel.DisclaimerFormFile = "eTrac" + ImageUniqueNameCust + ".pdf";                   
                     var st = objDARManager.SaveDisclaimerDARDetails(objServiceDisclaimerModel);
                    //Commented due to watermark. earlier we saved real file name but now we save watermark conversion file name. Because while adding watermark we need to delete disclaimer form and add dummy file with watermark.
                    //st = objWorkRequestManager.WorkFrSignature(objServiceImageUpload.WorkAssignmentId, imgupload.Image, imgupload.ImageEmp, EncryptWorkId + ".pdf", "");

                    if (st.DARId > 0)
                    {                      
                        //if (objData.StartTime != null && objData.EndTime != null)
                        //{
                        //    imgupload.StartTime = objData.StartTime.Value.ToMobileClientTimeZone(true);
                        //    imgupload.EndTime = objData.EndTime.Value.ToMobileClientTimeZone(true);

                        //    TimeSpan ts = objData.EndTime.Value - objData.StartTime.Value;

                        //    imgupload.TotalTime = ts.Days + "Days:" + ts.Hours + "Hours:" + ts.Minutes + "Minutes";
                        //}
                        var htmlData = TemplateDesigner.eMaintenanceTemplate(objServiceDisclaimerModel.LicensePlateNo, objServiceDisclaimerModel.CustomerName, objServiceDisclaimerModel.Address, objServiceDisclaimerModel.CustomerContact,
                                                                            objServiceDisclaimerModel.ImageCust, objServiceDisclaimerModel.ImageEmp, objServiceDisclaimerModel.CurrentLocation, objServiceDisclaimerModel.VehicleMake,
                                                                            objServiceDisclaimerModel.VehicleYear, objServiceDisclaimerModel.DriverLicenseNo, objServiceDisclaimerModel.VehicleModel, objServiceDisclaimerModel.FacilityRequestName,
                                                                            objServiceDisclaimerModel.TimeZoneName, objServiceDisclaimerModel.TimeZoneOffset, objServiceDisclaimerModel.IsTimeZoneinDaylight);
                        //----------------------------
                        Document doc = new Document(PageSize.A4, 30f, 30f, 40f, 30f);
                        iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(filename, FileMode.CreateNew));
                        doc.Open();
                        try
                        {
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

                            foreach (IElement element in HTMLWorker.ParseToList(
                            new StringReader(htmlData), null))
                            {
                                doc.Add(element);
                            }

                            PdfPTable table = new PdfPTable(2);

                            table.WidthPercentage = 96;
                            BaseFont bf = BaseFont.CreateFont(
                                        BaseFont.TIMES_ROMAN,
                                        BaseFont.CP1252,
                                        BaseFont.EMBEDDED);
                            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 11);
                                
                            //PdfPCell cell = new PdfPCell(new Phrase("Start Time: " + imgupload.StartTime, font));
                            //cell.Colspan = 1;
                            //cell.Border = 0;
                            //cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            //table.AddCell(cell);

                            //cell = new PdfPCell(new Phrase("End Time: " + imgupload.EndTime, font));
                            //cell.Colspan = 1;
                            //cell.Border = 0;
                            //cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            //table.AddCell(cell);

                            PdfPCell cell = new PdfPCell(new Phrase("Manager or Employee Name: " + objServiceDisclaimerModel.UserName, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Drivers Licence Number: " + objServiceDisclaimerModel.DriverLicenseNo, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Customer Name: " + objServiceDisclaimerModel.CustomerName, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Customer Address: " + objServiceDisclaimerModel.Address, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Customer Telephone: " + objServiceDisclaimerModel.CustomerContact, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            imageURL = HttpContext.Current.Server.MapPath("~/Content/Images/DARSignature/" + objServiceDisclaimerModel.ImageCust);
                            jpg = iTextSharp.text.Image.GetInstance(imageURL);
                            jpg.ScaleToFit(140f, 120f);
                            jpg.SpacingBefore = 10f;
                            jpg.SpacingAfter = 1f;

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                            cell.AddElement(jpg);
                            table.AddCell(cell);

                            imageURL = HttpContext.Current.Server.MapPath("~/Content/Images/DARSignature/" + objServiceDisclaimerModel.ImageEmp);
                            jpg = iTextSharp.text.Image.GetInstance(imageURL);
                            jpg.ScaleToFit(140f, 120f);
                            jpg.SpacingBefore = 10f;
                            jpg.SpacingAfter = 1f;

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                            cell.AddElement(jpg);
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(objServiceDisclaimerModel.CustomerName, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(objServiceDisclaimerModel.UserName, font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(" ", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Signature of Customer", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase("Signature of Manager or Employee", font));
                            cell.Colspan = 1;
                            cell.Border = 0;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);

                            doc.Add(table);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            doc.Close();
                            // Watermark code            
                            string watermarkLoc = System.Web.HttpContext.Current.Server.MapPath("~/Images/eTrac380-light.png");
                            PdfReader pdfReader = new PdfReader(filename);
                            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(filename.Replace(ImageUniqueNameCust + ".pdf", objServiceDisclaimerModel.DisclaimerFormFile), FileMode.Create));

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
                    serviceresponse.Message = (objServiceDisclaimerModel.ImageUrlCust != "" && !string.IsNullOrEmpty(objServiceDisclaimerModel.ImageUrlCust) && st.DARId > 0) ? CommonMessage.Successful() : CommonMessage.FailureMessage();
                    serviceresponse.Response = (objServiceDisclaimerModel.ImageUrlCust != "" && !string.IsNullOrEmpty(objServiceDisclaimerModel.ImageUrlCust) && st.DARId > 0) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Data = objServiceDisclaimerModel.DARId;
                }             
                else
                {
                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                Exception_B.exceptionHandel_Runtime(ex, "ServiceResponseModel<ServiceImageUpload> FacilitySignatureUpload(ServiceImageUpload objServiceImageUpload)", "objServiceImageUpload.UserId", objServiceDisclaimerModel.UserId);
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = 0;
            }
            return serviceresponse;
        }

        /// <summary>Update the status of disclaimer end time
        /// <CreatedFor>For Update disclaimer end time status</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>May-26-2017</CreatedOn>
        /// </summary>
        /// <param name="objServiceDARModel"></param>
        /// <returns></returns> 
        public ServiceResponseModel<string> UpdateDisclaimerEndTimeStatus(ServiceDARModel objServiceDARModel)
        {
            DARManager ObjDARManager = new DARManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                // var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == ObjWorkStatusModel.ServiceAuthKey && x.UserId == ObjWorkStatusModel.UserId);
                if (objServiceDARModel.LocationId > 0 && objServiceDARModel.ServiceAuthKey != null && objServiceDARModel.UserId > 0 && objServiceDARModel.DARId > 0)
                {
                   var result = ObjDARManager.UpdateEndTimeDAR(objServiceDARModel);

                    ObjServiceResponseModel.Response = result.Response;
                    ObjServiceResponseModel.Message = result.ResponseMessage;//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }
            return ObjServiceResponseModel;
        }

        #endregion For DAR

        #region For ImageUpload

        /// <summary>Save Work Request Assignment
        /// <CreatedFor>For Save Image Of All Type</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Feb-20-2015</CreatedOn>
        /// </summary>
        /// <param name="objServiceImageUpload"></param>
        /// <returns></returns> 
        public ServiceResponseModel<ServiceImageUpload> ImageUpload(ServiceImageUpload objServiceImageUpload)
        {
            ServiceResponseModel<ServiceImageUpload> serviceresponse = new ServiceResponseModel<ServiceImageUpload>();
            ServiceImageUpload imgupload = new ServiceImageUpload();
            try
            {
                if (objServiceImageUpload != null
                    && objServiceImageUpload.Image != null
                    && objServiceImageUpload.UserId > 0
                    && objServiceImageUpload.Image.Trim() != ""
                    && objServiceImageUpload.ImageModuleName != null)
                {
                    string WorkOrderImagePath = string.Empty;
                    string ImageUniqueName = string.Empty;
                    string ImageURL = string.Empty;

                    //Added By Bhushan Dod On 17-06-2015 for save image
                    #region For QRCParkingFacility
                    if (objServiceImageUpload.ImageModuleName == "QRCParkingFacility")
                    {
                        WorkOrderImagePath = WorkOrderImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["QRCParkingFacilityPath"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmm") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId;
                    }
                    #endregion For QRCParkingFacility

                    //Added By Bhushan Dod On 20-02-2015 for save image
                    #region For QRCVehicleDamage
                    if (objServiceImageUpload.ImageModuleName == "QRCVehicleDamage")
                    {
                        WorkOrderImagePath = WorkOrderImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["QRCVehiclePath"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmmsstt") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId;
                    }
                    #endregion For QRCVehicleDamage

                    //Added By Bhushan Dod On 07-04-2015 for QRCVehicleEnforcement
                    #region For QRCVehicleEnforcement
                    if (objServiceImageUpload.ImageModuleName == "QRCVehicleEnforcement")
                    {
                        WorkOrderImagePath = WorkOrderImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["QRCVehicleEnforcementPath"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmm") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId;
                    }
                    #endregion For QRCVehicleEnforcement

                    //Added By Bhushan Dod On 17-04-2015 for DarImage
                    #region For DarImage
                    if (objServiceImageUpload.ImageModuleName == "DarImage")
                    {
                        WorkOrderImagePath = WorkOrderImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DarImagePath"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmm") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId;
                    }
                    #endregion For DarImage

                    //Added By Bhushan Dod On 17-04-2015 for For Facilty Request Disclaimer Signature
                    #region For Facilty Request Disclaimer Signature
                    if (objServiceImageUpload.ImageModuleName == "FacilityRequestSign")
                    {
                        WorkOrderImagePath = WorkOrderImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["FRSignature"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmm") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId;
                    }
                    #endregion For Facilty Request Disclaimer Signature

                    //Added By Bhushan Dod On 17-06-2015 for save image
                    #region For DAR Image
                    if (objServiceImageUpload.ImageModuleName == "DARImage")
                    {
                        WorkOrderImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DarImagePath"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmm") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId;
                    }
                    #endregion For DAR Image

                    //Added By Bhushan Dod On 17-06-2015 for save image
                    #region For Rules Violation
                    if (objServiceImageUpload.ImageModuleName == "RulesViolation")
                    {
                        WorkOrderImagePath = WorkOrderImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["RulesViolationPath"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmm") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId;
                    }
                    #endregion For Rules Violation

                    //Added By Bhushan Dod On 08-05-2017 for saving image of Shuttle bus.
                    #region For QRC Shuttle Bus
                    if (objServiceImageUpload.ImageModuleName == "QRCShuttleBusDamage")
                    {
                        WorkOrderImagePath = WorkOrderImagePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["QRCVehiclePath"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmmsstt") + objServiceImageUpload.ImageModuleName + "_" + objServiceImageUpload.UserId + "Shuttle";
                    }
                    #endregion QRC Shuttle Bus

                    ImageURL = ImageUniqueName + ".jpg";

                    // Code for to get path of root directory and attach path of directory to store image
                    //string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                    //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + WorkOrderImagePath;
                    if (!Directory.Exists(WorkOrderImagePath))
                    {
                        Directory.CreateDirectory(WorkOrderImagePath);
                    }
                    var ImageLocation = WorkOrderImagePath + ImageURL;

                    //Save the image to directory
                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(objServiceImageUpload.Image)))
                    {
                        using (Bitmap bm2 = new Bitmap(ms))
                        {
                            bm2.Save(ImageLocation);
                            imgupload.Image = ImageURL;
                            imgupload.ImageUrl = ImageLocation;
                        }
                    }

                    serviceresponse.Message = (imgupload.ImageUrl != "" && !string.IsNullOrEmpty(imgupload.ImageUrl)) ? CommonMessage.Successful() : CommonMessage.DoesNotExistsRecordMessage();
                    serviceresponse.Response = (imgupload.ImageUrl != "" && !string.IsNullOrEmpty(imgupload.ImageUrl)) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Data = imgupload;
                }
                else
                {
                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
            }
            return serviceresponse;
        }

        #endregion For ImageUpload

        #region For Push Notification
        /// <summary>Push Notification
        /// <CreatedFor>Send Notification when manager login through mobile</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>March-23-2015</CreatedOn>
        /// </summary>
        /// <param name="objServicePushModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> SendPushNotification(ServicePushModel objServicePushModel)
        {
            ServiceResponseModel<string> serviceresponse = new ServiceResponseModel<string>();
            PushNotificationManager objPushNotificationManager = new PushNotificationManager();
            try
            {
                if (objServicePushModel != null && objServicePushModel.ServiceAuthKey != null && objServicePushModel.DeviceId != null)
                {
                    serviceresponse = objPushNotificationManager.SendNotification(objServicePushModel);
                }
                else
                {
                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
            }
            return serviceresponse;
        }
        #endregion For Push Notification

        #region For RuleMaster

        public ServiceResponseModel<List<RuleMasterModelList>> GetListofRuleByLocation(RuleMasterModelList objRuleMasterModelList)
        {
            ServiceResponseModel<List<RuleMasterModelList>> serviceresponse = new ServiceResponseModel<List<RuleMasterModelList>>();
            RuleManager objRuleManager = new RuleManager();
            try
            {
                if (objRuleMasterModelList != null && objRuleMasterModelList.ServiceAuthKey != null && objRuleMasterModelList.LocationId != null)
                {

                    serviceresponse = objRuleManager.GetListOfAllRule(objRuleMasterModelList);
                }
                else
                {

                    serviceresponse.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    serviceresponse.Message = CommonMessage.WrongParameterMessage();
                }

            }
            catch (Exception ex)
            {
                serviceresponse.Message = ex.Message;
                serviceresponse.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                serviceresponse.Data = null;
            }

            return serviceresponse;
        }

        #endregion For RuleMaster

        #region for Tracking Employee Idle

        /// <summary>EmployeeIdleStatus
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>Employee Idle record</CreatedFor>
        /// <CreatedOn>July-19-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceWorkStatusModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> EmployeeIdleStatus(ServiceWorkStatusModel obj)
        {
            ManageManager ObjManageManager = new ManageManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                // var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == ObjWorkStatusModel.ServiceAuthKey && x.UserId == ObjWorkStatusModel.UserId);
                if (obj.LocationID > 0 && obj.UserId > 0)
                {
                    dynamic result = ObjManageManager.EmployeeIdleStatus(obj.LocationID, obj.UserId);

                    ObjServiceResponseModel.Message = (result != null && result.Response > 0) ? CommonMessage.Successful() : CommonMessage.EmployeeIdle();
                    ObjServiceResponseModel.Response = (result != null && result.Response > 0) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);

                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        /// <summary>Update idle time of employee
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>UpdateEmployeeIdleTime</CreatedFor>
        /// <CreatedOn>June-04-2015</CreatedOn>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> UpdateEmployeeIdleTimeLimit(ServiceWorkStatusModel obj)
        {
            ManageManager ObjManageManager = new ManageManager();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (obj.ManagerId > 0 && obj.UserId > 0 && obj.StartTime != null && obj.StartTime.Trim() != "" && obj.ServiceAuthKey != null)
                {
                    ObjServiceResponseModel = ObjManageManager.UpdateEmployeeTime(obj);
                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }

        #endregion for Tracking Employee Idle

        public ServiceResponseModel<ServiceQrcVehicleModel> TestingXmltoObj(ServiceQrcVehicleModel objServiceQrcTrashcanModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ServiceResponseModel<ServiceQrcVehicleModel> ObjServiceResponseModel = new ServiceResponseModel<ServiceQrcVehicleModel>();
            QRCModel ObjQRCModel = new QRCModel();
            try
            {

                if (objServiceQrcTrashcanModel != null && objServiceQrcTrashcanModel.ServiceAuthKey != null)
                {
                    ServiceQrcVehicleModel ObjResponse = ObjQRCSetupManager.XmlTestingForPhoneSystem(objServiceQrcTrashcanModel);
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.Successful();
                    ObjServiceResponseModel.Data = ObjResponse;
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception ex)
            {
                ObjServiceResponseModel.Message = ex.Message;
                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                ObjServiceResponseModel.Data = null;
            }

            return ObjServiceResponseModel;
        }
    }
}
