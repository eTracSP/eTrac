using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;


namespace WorkOrderEMS.Controllers.Common
{
    public class CommonController : Controller
    {
        //
        // GET: /Common/
        public ActionResult Index()
        {
            return View();
        }
        private readonly IManageManager _IManageManager;
        private readonly ICommonMethod _ICommonMethod;
        private readonly IGlobalAdmin _IGlobalAdmin;
        private readonly IUser _IUser;
        private readonly IWorkRequestAssignment _IWorkRequestAssignment;
        private readonly IClientManager _IClientManager;
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        CommonMethodManager objCommonMethodManager = new CommonMethodManager();

        eTracLoginModel ObjSession;
        public CommonController(ICommonMethod _ICommonMethod, IManageManager _IManageManager, IGlobalAdmin _IGlobalAdmin, IUser _IUser, IWorkRequestAssignment _IWorkRequestAssignment, IClientManager _IClientManager)
        {
            this._ICommonMethod = _ICommonMethod;
            this._IGlobalAdmin = _IGlobalAdmin;
            this._IUser = _IUser;
            this._IWorkRequestAssignment = _IWorkRequestAssignment;
            this._IClientManager = _IClientManager;
            this._IManageManager = _IManageManager;
        }

        public ActionResult WorkOrderDetails(string TaskName, string TaskPriority, string RequestBy, string WorkArea, string TaskType, string AssignedUser, string TaskStatus, string remarks)
        {
            WorkOrderModel _WorkRequestModel = new WorkOrderModel();
            try
            {
                _WorkRequestModel.TaskName = TaskName;
                _WorkRequestModel.TaskPriorityName = TaskPriority;
                _WorkRequestModel.RequestByName = RequestBy;
                _WorkRequestModel.WorkAreaName = WorkArea;
                _WorkRequestModel.TaskTypeName = TaskType;
                _WorkRequestModel.AssignedToName = AssignedUser;
                _WorkRequestModel.TaskStatusName = TaskStatus;
                return PartialView("~/Views/Manager/_WorkOrderDetails.cshtml", _WorkRequestModel);
            }
            catch (Exception ex)
            {
                { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
            }
        }
        //public JsonResult GetAssetImage(string AssetID)
        //{

        //    try
        //    {
        //        eTracLoginModel ObjLoginModel = null;
        //        long _AssetID = 0;
        //        if (Session["eTrac"] != null)
        //        { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
        //        if (!string.IsNullOrEmpty(AssetID))
        //        {
        //            long.TryParse(AssetID, out _AssetID);
        //        }
        //        string Img = _ICommonMethod.GetAssetImage(ObjLoginModel.LocationID, Convert.ToUInt32(_AssetID));
        //        return Json(Img, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        #region EmailHelper SendEmailToUser

        /// <summary>SendEmailToUser
        /// <CreatedFor>Send Email To User</CreatedFor>
        /// <CreatedOn>Nov-24-2014</CreatedOn>
        /// </summary>
        /// <param name="UserForEmail"></param>
        ///Added By Bhushan Dod on 17-02-2015 for save userId into email log
        /// <returns></returns>
        [NonAction]
        public bool SendEmailToUser(UserModel UserForEmail, string LocationName, string LocAdd, string LocationCode = "", long sendBy = 0)
        {


            dynamic values = null;
            try
            {
                string passwo = "";
                //added by vijay sahu on 26  march 2015
                try
                {
                    passwo = Cryptography.GetDecryptedData(UserForEmail.Password, true);
                }
                catch (Exception)
                {
                    passwo = UserForEmail.Password;
                }

                EmailHelper objEmailHelper = new EmailHelper();
                objEmailHelper.emailid = UserForEmail.UserEmail;

                objEmailHelper.UserName = UserForEmail.AlternateEmail;
                objEmailHelper.UserType = UserForEmail.UserType;
                objEmailHelper.FirstName = UserForEmail.FirstName;
                objEmailHelper.LastName = UserForEmail.LastName;
                objEmailHelper.Password = passwo;//Cryptography.GetDecryptedData(UserForEmail.Password, true);
                objEmailHelper.LocationName = LocationName;
                objEmailHelper.LocationCode = LocationCode;
                objEmailHelper.LocAddress = LocAdd;
                objEmailHelper.MailType = "USERREGISTRATION";
                string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"]);


                HostingPrefix = HostingPrefix + "Manager/Employee?usr=" + Cryptography.GetEncryptedData(UserForEmail.UserId.ToString(), true);
                objEmailHelper.RegistrationLink = HostingPrefix;


                values = objEmailHelper;
                bool IsSent = objEmailHelper.SendEmailWithTemplate();
                //return Result.EmailSendSuccessfully;
                //Added By Bhushan Dod on 17-02-2015 for maintain email log
                if (IsSent == true)
                {
                    try
                    {
                        //objCommonMethodManager = new CommonMethodManager();
                        if (sendBy != 0)
                        {
                            IsSent = objCommonMethodManager.EmailLog(sendBy, UserForEmail.UserId, objEmailHelper.emailid, objEmailHelper.Subject, UserForEmail.Location);
                        }
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SendEmailToUser(UserModel UserForEmail,", "while sending email to registered user", values);
                return true;
            }
            return true;
        }

        #endregion EmailHelper SendEmailToUser


        #region EmailHelper RecoverNmailUserPassword


        /// <summary>RecoverNmailUserPassword
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedFor>Recover and mail User Password</CreatedFor>
        /// <CreatedOn>Dec-02-2014</CreatedOn>
        /// </summary>
        /// <param name="eTracLogin"></param>
        /// <returns></returns>
        [NonAction]
        private bool RecoverNmailUserPassword(eTracLoginModel eTracLogin)
        {
            try
            {
                EmailHelper objEmailHelper = new EmailHelper();
                objEmailHelper.emailid = eTracLogin.RecoveryEmail;
                objEmailHelper.FirstName = eTracLogin.RecoveryEmail;
                objEmailHelper.Password = Cryptography.GetDecryptedData(eTracLogin.Password, true);
                objEmailHelper.MailType = "FORGOTPASSWORD";
                string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"]);
                objEmailHelper.RegistrationLink = HostingPrefix;
                objEmailHelper.SendEmailWithTemplate();
            }
            catch (Exception) { throw; } return true;
        }

        #endregion EmailHelper SendEmailToUser

        #region EmailHelper Location Verfication
        public bool SendEmailForLocationVerification(UserModel UserForEmail, string LocationName, string LocAdd, long VerificationId, string MethodForVerfication, long LocationId)
        {

            try
            {

                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    { ObjSession = (eTracLoginModel)(Session["eTrac"]); }
                    string _verificationId = "";
                    string _locationId = "";
                    string _referralAdderss = "";
                    string _verificatrionUrl = "";
                    try
                    {
                        EmailHelper objEmailHelper = new EmailHelper();
                        objEmailHelper.emailid = UserForEmail.UserEmail;
                        objEmailHelper.UserType = UserForEmail.UserType;
                        objEmailHelper.FirstName = UserForEmail.FirstName;
                        objEmailHelper.LastName = UserForEmail.LastName;
                        objEmailHelper.LocationName = LocationName;
                        objEmailHelper.LocAddress = LocAdd;

                        objEmailHelper.MailType = "LOCATIONVERIFICATION";
                        string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"]);


                        _verificationId = Cryptography.GetEncryptedData(VerificationId.ToString(), true);
                        _verificationId = (_verificationId != null && !string.IsNullOrEmpty(_verificationId)) ? _verificationId.Trim() : null;
                        _locationId = Cryptography.GetEncryptedData(LocationId.ToString(), true);
                        _referralAdderss = string.Format("{0}" + MethodForVerfication, HostingPrefix.Trim());
                        _verificatrionUrl = string.Format("{0}?ver={1}&loc={2}", _referralAdderss, _verificationId, _locationId);

                        // HostingPrefix = HostingPrefix + "?usr=" + Cryptography.GetEncryptedData(UserForEmail.UserId.ToString(), true);
                        objEmailHelper.VerificationLink = _verificatrionUrl;

                        bool IsSent = objEmailHelper.SendEmailWithTemplate();
                        //Added By Bhushan Dod on 17-02-2015 for maintain email log
                        if (IsSent == true)
                        {
                            try
                            {
                                IsSent = objCommonMethodManager.EmailLog(ObjSession.UserId, UserForEmail.UserId, objEmailHelper.emailid, objEmailHelper.Subject, LocationId);

                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                        //return Result.EmailSendSuccessfully;
                    }
                    catch (Exception)
                    { throw; }
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion EmailHelper Location Verfication


        #region Verify Location
        /// <summary>Verification By Manager
        /// <CreatedBy>Roshan Rahood</CreatedBy>
        /// <CreatedOn>Dec-23-2014</CreatedOn>
        /// <CreatedFor>Verify the location by manager</CreatedFor>
        /// </summary>
        /// <returns></returns>
        // public JsonResult VerifyLocationByManager(string ver, string loc)
        public JsonResult VerifyLocationByManager(string ver, string loc)
        {
            string _message = string.Empty;
            try
            {
                /* parse this data decode and decrypt*/

                long _verificationManagerId = 0;
                long _locationId = 0;

                if (!string.IsNullOrEmpty(ver) && !string.IsNullOrEmpty(loc))
                {

                    /*Decrypt data*/
                    ver = Cryptography.GetDecryptedData(ver, true);
                    loc = Cryptography.GetDecryptedData(loc, true);
                    /*Decrypt data end*/

                    long.TryParse(ver, out _verificationManagerId);
                    long.TryParse(loc, out _locationId);
                }


                /* parse this data decode and decrypt end*/
                if (_locationId > 0 && _verificationManagerId > 0)
                {
                    if (_IGlobalAdmin.ManagerLocationApproval(_verificationManagerId, _locationId))
                    {
                        UserDetailsForVerificationModel objUserModel = new UserDetailsForVerificationModel();
                        objUserModel = _ICommonMethod.GetUserDetailsByLocationId(_locationId);

                        UserModel objModel = new UserModel();

                        objModel.FirstName = objUserModel.ClientFName;
                        objModel.LastName = objUserModel.ClientLName;
                        objModel.UserEmail = objUserModel.EmailAddress;
                        objModel.UserType = objUserModel.UserType;

                        #region Email To Manager for Location Verfication
                        SendEmailForLocationVerification(objModel, objUserModel.LocationName, objUserModel.LocationAddress, objUserModel.ClientId, "common/VerifyLocationByClient", _locationId);
                        #endregion Email To Manager for Location Verfication

                        _message = "Location approval is done,Verification mail has been sent to client waiting for client approval";
                    }
                    else
                    { _message = "Location is already approved"; }
                }
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; }
            return Json(_message, JsonRequestBehavior.AllowGet);
        }

        /// <summary>Verification By Client
        /// <CreatedBy>Roshan Rahood</CreatedBy>
        /// <CreatedOn>Dec-23-2014</CreatedOn>
        /// <CreatedFor>Verify the location by Client</CreatedFor>
        /// </summary>
        /// <returns></returns>
        public JsonResult VerifyLocationByClient(string ver, string loc)
        {
            string _message = string.Empty;
            try
            {
                /* parse this data decode and decrypt*/
                #region parse this data decode and decrypt

                long _verificationClientId = 0;
                long _locationId = 0;

                if (!string.IsNullOrEmpty(ver) && !string.IsNullOrEmpty(loc))
                {
                    /*Decrypt data*/
                    ver = Cryptography.GetDecryptedData(ver, true);
                    loc = Cryptography.GetDecryptedData(loc, true);
                    /*Decrypt data end*/

                    long.TryParse(ver, out _verificationClientId);
                    long.TryParse(loc, out _locationId);
                }
                #endregion parse this data decode and decrypt
                /* parse this data decode and decrypt end*/

                if (_locationId > 0 && _verificationClientId > 0)
                {
                    if (_IGlobalAdmin.ClientLocationApproval(_verificationClientId, _locationId))
                    {
                        List<EmailToUserModel> listToEmail = _ICommonMethod.GetUsersToEmail(_locationId, null);


                        foreach (var userToEmail in listToEmail)
                        {
                            UserModel objModel = new UserModel();

                            objModel.FirstName = userToEmail.FirstName;
                            objModel.LastName = userToEmail.LastName;
                            objModel.UserEmail = userToEmail.UserEmail;
                            objModel.Password = _ICommonMethod.CreateRandomPassword();
                            objModel.UserType = userToEmail.UserType;

                            #region Email to Users
                            SendEmailToUser(objModel, userToEmail.LocationName, userToEmail.Address1 + " " + userToEmail.Address2);
                            #endregion Email to Users

                        }

                        //_message = CommonMessage.UserApproved(UserType.Client, "ClientUserName", "templocation");
                        _message = "Location approverd,please check your Inbox.";
                    }
                    else
                    {
                        //_message = CommonMessage.UserAlreadyApproved(UserType.Client, "ClientUserName", "templocation");
                        _message = "Location already approverd.";
                    }
                }
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; }
            return Json(_message, JsonRequestBehavior.AllowGet);
        }

        #endregion Verify Location
        [HttpPost]
        public JsonResult SaveUserDashboardSettings(string SelectedSettings)
        {
            if (Session["eTrac"] != null)
            {
                eTracLoginModel obj_eTracLoginModel = new eTracLoginModel();
                obj_eTracLoginModel = (eTracLoginModel)Session["eTrac"];
                Common_B obj_Common_B = new Common_B();
                string result = obj_Common_B.Save_UpdateDashboardSettings(obj_eTracLoginModel.UserId, SelectedSettings);
                Session["eTrac_DashboardSetting"] = obj_Common_B.getUserDasboardSettings(obj_eTracLoginModel.UserId);
                return Json(result);
            }
            else
            {
                return Json("");
            }
        }
        /// <summary>
        /// TO DELETE USER FROM THE APPLICATION
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>2015-02-03</CreatedDate>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteUser(string UserID)
        {
            DARModel objDAR;
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                {
                    long userid = 0;
                    if (!string.IsNullOrEmpty(UserID))
                    {
                        UserID = Cryptography.GetDecryptedData(UserID, true);
                        long.TryParse(UserID, out userid);
                    }
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);

                    objDAR = new DARModel();
                    objDAR.LocationId = ObjLoginModel.LocationID;
                    objDAR.UserId = ObjLoginModel.UserId;
                    objDAR.CreatedBy = ObjLoginModel.UserId;
                    objDAR.CreatedOn = DateTime.UtcNow;


                    Tuple<int, string> rec_Result = _IUser.DeleteUserFromUserList(userid, ObjLoginModel.UserId, objDAR);
                    //if (rec_Result.Item1 == 1)
                    //{
                    //    return Json("User has been successfully deleted.");
                    //}
                    //else if (rec_Result.Item1 == 0)
                    //{
                    //    return Json("User does not exist in the application");
                    //}
                    //else if (rec_Result.Item1 == 1)
                    //{
                    //    return Json("You can't delete this user because this user has already been mapped in one or more than one location.");
                    //}
                    //else {
                    return Json(new { result = rec_Result.Item1, message = rec_Result.Item2 });
                    //}

                }
                else
                {
                    return Json(new { result = 0, message = "Session has expired" });
                }
            }
            catch (Exception ex) { return Json(new { result = 0, message = ex.Message.ToString() }); }
        }

        [HttpPost]
        public JsonResult DeleteUserFromUserList(string UserID)
        {
            DARModel objDAR;
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                {
                    long userid = 0;
                    if (!string.IsNullOrEmpty(UserID))
                    {
                        UserID = Cryptography.GetDecryptedData(UserID, true);
                        long.TryParse(UserID, out userid);
                    }
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);

                    objDAR = new DARModel();
                    objDAR.LocationId = ObjLoginModel.LocationID;
                    objDAR.UserId = ObjLoginModel.UserId;
                    objDAR.CreatedBy = ObjLoginModel.UserId;
                    objDAR.CreatedOn = DateTime.UtcNow;


                    var rec_Result = _IUser.DeleteUser(userid, ObjLoginModel.UserId, objDAR);
                    if (rec_Result == Result.Delete)
                    {
                        return Json("User has been successfully deleted.");
                    }
                    else if (rec_Result == Result.DoesNotExist)
                    {
                        return Json("User does not exist in the application");
                    }
                    else if (rec_Result == Result.ExistRecord)
                    {
                        return Json("You can't delete this user because this user has already been mapped in one or more than one location.");
                    }
                    else { return Json(rec_Result); }

                }
                else
                {
                    return Json("Session has expired");
                }
            }
            catch (Exception ex) { return Json(ex.Message.ToString()); }
        }

        /// <summary>
        /// To Upload Image fro ajax Calls
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>24 March 2015</CreatedDate>
        /// <param name="File"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadedWebImage(HttpPostedFileBase File)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (File != null)
                {
                    string ImageName = ObjLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + File.FileName.ToString();
                    CommonHelper obj_CommonHelper = new CommonHelper();
                    var res = obj_CommonHelper.UploadImage(File, Server.MapPath(ConfigurationManager.AppSettings["ProfilePicPath"]), ImageName);
                    if (res)
                    {
                        return Json(ImageName);
                    }
                    else { return Json(""); }
                }
                return Json("");
            }
            else
            {
                return Json("");
            }
        }

        public JsonResult GetTaskType(string taskType)
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
                if (!string.IsNullOrEmpty(taskType))
                {
                    List<SelectListItem> lstEmployee = _ICommonMethod.GetGlobalCodeData(taskType).Select(u => new SelectListItem()
                    {
                        Text = u.CodeName,
                        Value = Convert.ToString(u.GlobalCodeId, CultureInfo.InvariantCulture)
                    }).ToList();
                    return Json(lstEmployee, JsonRequestBehavior.AllowGet);
                }
                else { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// TO GET WORKORDER CREATED BY CLIENT AND NOT ASSIGNED
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>4/7/2015</CreatedDate>
        /// </summary>
        /// <param name="PageNo"></param>
        /// <param name="NoOfRecords"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetWorkOrderCreatedByClient(int PageNo, int NoOfRecords, string txtSearch, string operation, string sord, string _search, string sidx, string filter)
        {

            eTracLoginModel ObjLoginModel = null;

            long LocationID = 0;
            if (Session["eTrac"] != null)
            {
                //eTracLoginModel obj_eTracLoginModel = new eTracLoginModel();
                ObjLoginModel = (eTracLoginModel)Session["eTrac"];

                LocationID = Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]);

                ObjectParameter obj_ObjectParameter = new ObjectParameter("TotalRecords", typeof(int));
                var Result = _IWorkRequestAssignment.GetAllWorkRequestCreatedByClient(0, ObjLoginModel.UserId, operation, PageNo, NoOfRecords, sidx, sord, txtSearch, LocationID, ObjLoginModel.UserId, DateTime.Now, DateTime.Now, filter, obj_ObjectParameter);
                return Json(Result);
            }
            else
            {
                return Json("Session Expired!");
            }
        }

        #region Commom Email

        #endregion
        //public ActionResult ApproveVendorResult()
        //{
        //    string mess = ViewBag.Message;
        //    return View();
        //}


        [HttpPost]
        [AllowAnonymous]
        public ActionResult isVendorEmailExists(string vendorEmail)
        {
            byte status = 0;
            try
            {

                Common_B obj = new Common_B();

                byte result = obj.isVendorEmailExists(vendorEmail);

                status = result;

            }
            catch (Exception)
            {
                status = 0;
            }
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult getUserDetailsByUserId(string usr)
        {
            long userid = 0;
            if (!string.IsNullOrEmpty(usr))
            {
                usr = Cryptography.GetDecryptedData(usr, true);
                long.TryParse(usr, out userid);
            }
            UserModel ObjUserModel = _IUser.GetUserDetailsById(userid);

            return PartialView("PartialForUserDetails", ObjUserModel);
            //return Json(new { data = ObjUserModel });
        }

        [HttpGet]
        public string getProfilePicture(string usr)
        {
            long userid = 0;
            if (!string.IsNullOrEmpty(usr))
            {

                long.TryParse(usr, out userid);
            }
            return _IUser.getProfilePicture(userid);
        }

        [HttpPost]
        public JsonResult TimeZoneInfo()
        {
            try
            {

                var Result = _ICommonMethod.GetTimeZoneInfo("373");
                return Json(Result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>This method is same as Create method in ITController but due to client requirement change as any type of user can create from one page with services assign from the same.
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Sep-08-2016</CreatedOn>
        /// <CreatedFor>POST method for Create New User</CreatedFor>
        /// </summary>
        /// <param name="ObjUserModel"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateUser(string usr)
        {
            try
            {
                if (Session["eTrac"] != null)
                {
                    eTracLoginModel obj_eTracLoginModel = new eTracLoginModel();
                    obj_eTracLoginModel = (eTracLoginModel)Session["eTrac"];
                    ViewBag.UserType = _ICommonMethod.GetUserTypeListForUserRegistration("USERTYPE", obj_eTracLoginModel.UserRoleId);
                }
                ViewBag.Country = _ICommonMethod.GetAllcountries();

                var _UserModel = _ICommonMethod.LoadInvitedUser(usr);
                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
                return View("User", _UserModel);
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }

        /// <summary>This method is same as Create method in ITController but due to client requirement change as any type of user can create from one page with services assign from the same.
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Sep-08-2016</CreatedOn>
        /// <CreatedFor>POST method for Create New User</CreatedFor>
        /// </summary>
        /// <param name="ObjUserModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateUser(QRCModel ObjUserModel)
        {
            DARModel objDAR = null;
            eTracLoginModel ObjLoginModel = null;
            try
            {
                long LocId = 0;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
                if (Session["eTrac_SelectedDasboardLocationID"] != null)
                {
                    LocId = (long)Session["eTrac_SelectedDasboardLocationID"];
                }

                //Added by Bhushan Dod on 13/09/2016 for mapping selected Location, User Type, Services to permission detail
                ObjUserModel.UserModel.SelectedServicesID = (ObjUserModel.ServicesID == null) ? "0" : ObjUserModel.ServicesID;
                ObjUserModel.UserModel.SelectedLocationId = (ObjUserModel.LocationId == null) ? null : ObjUserModel.LocationId;
                ObjUserModel.UserModel.SelectedUserType = (ObjUserModel.SelectedUserType == 0) ? ObjLoginModel.UserRoleId : ObjUserModel.SelectedUserType;
                ObjUserModel.UserModel.SelectedLocationName = ObjUserModel.LocationName;

                //if (ModelState.IsValid)
                //{

                if (ObjUserModel != null && ObjUserModel.UserModel != null) //&& ObjUserModel.UserModel.UserId == 0
                {
                    Result result = Result.FreePlan;
                    if (ObjUserModel.LocationId == null)
                    {
                        #region password
                        if (!String.IsNullOrEmpty(ObjUserModel.UserModel.Password))
                        {
                            ObjUserModel.UserModel.Password = Cryptography.GetEncryptedData(ObjUserModel.UserModel.Password, true);
                        }
                        #endregion password

                        ObjUserModel.UserModel.CreatedBy = ObjLoginModel.UserId;
                        ObjUserModel.UserModel.CreatedDate = DateTime.UtcNow;
                        ObjUserModel.UserModel.IsDeleted = false;
                        ObjUserModel.UserModel.IdleTimeLimit = DateTime.UtcNow.SetTime(0, 30, 0, 0);//Added By Bhushan on 07/06/2015 for by deafult IDLE Time set
                        if (ObjUserModel.UserModel.ProfileImage != null)
                        {
                            string ImageName = ObjLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString()+".png";
                            CommonHelper obj_CommonHelper = new CommonHelper();
                            obj_CommonHelper.UploadImage(ObjUserModel.UserModel.ProfileImage, Server.MapPath(ConfigurationManager.AppSettings["ProfilePicPath"]), ImageName);
                            ObjUserModel.UserModel.ProfileImageFile = ImageName;
                        }
                        long QRCID = 0;
                        //Result result = _IClientManager.SaveClient(ObjUserModel.UserModel, out QRCID, true, objDAR); // commented by vijay sahu on 18 feb 2015
                        //Updated By Bhushan Dod on 30/05/2016 for location id parameter not send properly send 0 so need to update location id.

                        result = _IClientManager.SaveNewUserRegistration(ObjUserModel.UserModel, out QRCID, true, ObjLoginModel.UserId, "");
                    }
                    else
                    {
                        if (ObjUserModel.UserModel.UserId == 0)
                        {
                            #region password

                            if (!String.IsNullOrEmpty(ObjUserModel.UserModel.Password))
                            {
                                ObjUserModel.UserModel.Password = Cryptography.GetEncryptedData(ObjUserModel.UserModel.Password, true);
                            }
                            #endregion password

                            ObjUserModel.UserModel.CreatedBy = ObjLoginModel.UserId;
                            ObjUserModel.UserModel.CreatedDate = DateTime.UtcNow;
                            ObjUserModel.UserModel.IsDeleted = false;
                            ObjUserModel.UserModel.IdleTimeLimit = DateTime.UtcNow.SetTime(0, 30, 0, 0);//Added By Bhushan on 07/06/2015 for by deafult IDLE Time set

                            objDAR = new DARModel();
                            objDAR.LocationId = ObjUserModel.LocationId.Value;
                            objDAR.UserId = ObjLoginModel.UserId;
                            objDAR.CreatedBy = ObjLoginModel.UserId;
                            objDAR.CreatedOn = DateTime.UtcNow;

                            objDAR.TaskType = (long)TaskTypeCategory.UserCreation;
                            switch (ObjUserModel.UserModel.SelectedUserType)
                            {
                                case 2:
                                    {
                                        objDAR.ActivityDetails = DarMessage.NewManagerCreatedDar(ObjUserModel.LocationName);
                                        break;
                                    }
                                case 3:
                                    {
                                        objDAR.ActivityDetails = DarMessage.NewEmployeeCreatedDar(ObjUserModel.LocationName);
                                        break;
                                    }
                                case 4:
                                    {
                                        objDAR.ActivityDetails = DarMessage.ClientUpdatedDar(ObjUserModel.LocationName);
                                        break;
                                    }
                                case 5:
                                    {
                                        objDAR.ActivityDetails = DarMessage.NewITAdministratorCreatedDar(ObjUserModel.LocationName);
                                        break;
                                    }
                                case 6:
                                    {
                                        objDAR.ActivityDetails = DarMessage.NewAdministratorCreatedDar(ObjUserModel.LocationName);
                                        break;
                                    }
                                default:
                                    {
                                        objDAR.ActivityDetails = "Something went wrong";
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            ObjUserModel.UserModel.ModifiedBy = ObjLoginModel.UserId;
                            ObjUserModel.UserModel.ModifiedDate = DateTime.UtcNow;
                            ObjUserModel.UserModel.IsDeleted = false;
                            if (!String.IsNullOrEmpty(ObjUserModel.UserModel.Password))
                            {
                                ObjUserModel.UserModel.Password = Cryptography.GetEncryptedData(ObjUserModel.UserModel.Password, true);
                            }
                            objDAR = new DARModel();
                            objDAR.LocationId = ObjUserModel.LocationId.Value;
                            objDAR.UserId = ObjLoginModel.UserId;
                            objDAR.CreatedBy = ObjLoginModel.UserId;
                            objDAR.CreatedOn = DateTime.UtcNow;

                            objDAR.TaskType = (long)TaskTypeCategory.UserUpdate;
                            switch (ObjUserModel.UserModel.SelectedUserType)
                            {
                                case 2:
                                    {
                                        objDAR.ActivityDetails = DarMessage.ManagerUpdatedDar(ObjUserModel.LocationName);
                                        break;
                                    }
                                case 3:
                                    {
                                        objDAR.ActivityDetails = DarMessage.EmployeeUpdatedDar(ObjUserModel.LocationName);
                                        break;
                                    }
                                case 4:
                                    {
                                        objDAR.ActivityDetails = DarMessage.ClientUpdatedDar(ObjUserModel.LocationName);
                                        break;
                                    }
                                case 5:
                                    {
                                        objDAR.ActivityDetails = DarMessage.ITAdministratorUpdatedDar(ObjUserModel.LocationName);
                                        break;
                                    }
                                case 6:
                                    {
                                        objDAR.ActivityDetails = DarMessage.AdministratorUpdatedDar(ObjUserModel.LocationName);
                                        break;
                                    }
                                default:
                                    {
                                        objDAR.ActivityDetails = "Something went wrong";
                                        break;
                                    }
                            }
                        }
                        if (ObjUserModel.UserModel.ProfileImage != null)
                        {
                            string ImageName = ObjLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + ".png";
                            CommonHelper obj_CommonHelper = new CommonHelper();
                            obj_CommonHelper.UploadImage(ObjUserModel.UserModel.ProfileImage, Server.MapPath(ConfigurationManager.AppSettings["ProfilePicPath"]), ImageName);
                            ObjUserModel.UserModel.ProfileImageFile = ImageName;
                        }
                        long QRCID = 0;
                        //Result result = _IClientManager.SaveClient(ObjUserModel.UserModel, out QRCID, true, objDAR); // commented by vijay sahu on 18 feb 2015
                        //Updated By Bhushan Dod on 30/05/2016 for location id parameter not send properly send 0 so need to update location id.
                        //Result result = _IClientManager.SaveClient(ObjUserModel.UserModel, out QRCID, true, objDAR, 0, ObjLoginModel.UserId, "");//added by vijay sahu on15 feb 2015
                        result = _IClientManager.SaveClientNewUserRegistrationforAll(ObjUserModel.UserModel, out QRCID, true, objDAR, ObjUserModel.LocationId.Value, ObjLoginModel.UserId, "");

                    }

                    if (result == Result.Completed)
                    {
                        //var LocationDetails = _ICommonMethod.GetLocationDetailsById(LocId);
                        #region EmailHelper
                        //Common.CommonController myCommonController = new Common.CommonController(_ICommonMethod, _IGlobalAdmin, _IVehicleRegistration, _IUser, _IWorkRequestAssignment, _IClientManager);
                        #region Email to IT Admin User

                        //if (ObjUserModel.UserModel.UserId != 0)
                        //{
                        //    ObjUserModel.UserModel.Location = LocId;
                        //    myCommonController.SendEmailToUser(ObjUserModel.UserModel, LocationDetails.LocationName, LocationDetails.Address1, LocationDetails.Address2, ObjLoginModel.UserId);
                        //} 

                        //Commented by Bhushan Dod. No use of this bcoz location name already in 
                        //var abc = _ICommonMethod.GetLocationDetailsById(ObjUserModel.LocationId.Value);

                        EmailHelper objEmailHelper = new EmailHelper();
                        objEmailHelper.emailid = ObjUserModel.UserModel.UserEmail;
                        objEmailHelper.UserName = ObjUserModel.UserModel.AlternateEmail;
                        objEmailHelper.UserType = ObjUserModel.UserModel.SelectedUserType;
                        objEmailHelper.FirstName = ObjUserModel.UserModel.FirstName;
                        objEmailHelper.LastName = ObjUserModel.UserModel.LastName;
                        objEmailHelper.Password = Cryptography.GetDecryptedData(ObjUserModel.UserModel.Password, true);

                        objEmailHelper.LocationName = (ObjUserModel.LocationId == null) ? "Not Assigned" : ObjUserModel.LocationName;
                        objEmailHelper.LocAddress = ObjUserModel.UserModel.Address.Address1; // here locAddress means user Address
                        objEmailHelper.MailType = "CreateNewUser";

                        string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], System.Globalization.CultureInfo.InvariantCulture);
                        HostingPrefix = HostingPrefix + "Manager/Employee?usr=" + Cryptography.GetEncryptedData(ObjUserModel.UserModel.UserId.ToString(), true);

                        objEmailHelper.RegistrationLink = HostingPrefix;

                        objEmailHelper.SendEmailWithTemplate();

                        #endregion Email to IT Admin User

                        #endregion EmailHelper
                        /* test mail code call*/

                        ViewBag.Message = CommonMessage.UserSaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;

                        //Commented by Bhushan Dod on 19-Oct-2016 for unused code no need to load also comment for after saved redirect according to condition.
                        // ModelState.Clear();
                        //ObjUserModel = _ICommonMethod.LoadInvitedUser(string.Empty);
                    }
                    else if (result == Result.DuplicateRecord)
                    {
                        ViewBag.Message = CommonMessage.DuplicateRecordEmailIdUserNameEmpIdMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Info; // store the message for successful in tempdata to display in view.
                    }
                    else if (result == Result.UpdatedSuccessfully)
                    {
                        ViewBag.Message = CommonMessage.UpdateSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;// store the message for successful in tempdata to display in view.
                        //Commented by Bhushan Dod on 19-Oct-2016 for unused code no need to load also comment for after saved redirect according to condition.
                        // ModelState.Clear();                        
                        // ObjUserModel = _ICommonMethod.LoadInvitedUser(string.Empty);
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                    }
                }
                else { ViewBag.Message = CommonMessage.InvalidEntry(); }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            finally
            {
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
                ViewBag.UserType = _ICommonMethod.GetUserTypeListForUserRegistration("USERTYPE", ObjLoginModel.UserRoleId);
                /*
                System.Data.Entity.Core.Objects.ObjectParameter paramTotalRecords = new System.Data.Entity.Core.Objects.ObjectParameter("TotalRecords", typeof(int));
                ObjUserModel.JobTitleList = _ICommonMethod.GetGlobalCodeData("JOBTITLE");
                ObjUserModel.LocationList = _IGlobalAdmin.GetAllLocationList(0, "GetAllLocation", 1, 10000, "LocationName", "desc", "", paramTotalRecords);
                paramTotalRecords = null;
                */
            }

            ViewBag.UpdateMode = false;

            if (ObjUserModel != null && ObjUserModel.UserModel != null
                && ObjUserModel.UserModel.SelectedLocationId == null
                && ObjUserModel.UserModel.SelectedUserType == Convert.ToInt64(UserType.ITAdministrator))
            {
                ModelState.Clear();
                ObjUserModel = _ICommonMethod.LoadInvitedUser(string.Empty);

                return RedirectToAction("UnVerifiedUsers", "GlobalAdmin");
            }
            else if (ObjUserModel != null && ObjUserModel.UserModel != null
               && ObjUserModel.UserModel.SelectedLocationId == null
               && ObjUserModel.UserModel.SelectedUserType != Convert.ToInt64(UserType.ITAdministrator))
            {
                ModelState.Clear();
                ObjUserModel = _ICommonMethod.LoadInvitedUser(string.Empty);
                return RedirectToAction("NotAssignedUsers", "GlobalAdmin", new { i = 1 });
            }
            else if (ObjUserModel != null && ObjUserModel.UserModel != null
           && ObjUserModel.UserModel.SelectedLocationId != null
           && ObjUserModel.UserModel.SelectedUserType != Convert.ToInt64(UserType.ITAdministrator))
            {
                ModelState.Clear();
                ObjUserModel = _ICommonMethod.LoadInvitedUser(string.Empty);
                return RedirectToAction("UnVerifiedUsers", "GlobalAdmin", new { i = 1 });
            }
            else
            {
                ModelState.Clear();
                ObjUserModel = _ICommonMethod.LoadInvitedUser(string.Empty);
                return View("User", ObjUserModel);
            }

            return View("User", ObjUserModel);
        }

        /// <summary>
        /// Created By : Bhushan Dod
        /// Created On: 03-October-2016
        /// This function returns the servcies associated with location. 
        /// </summary>
        /// <param name="LocationID"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        public JsonResult GetLocationServicePermissionList(long LocationID, long UserType)
        {
            if (LocationID > 0 && UserType > 0)
            {
                var lstServices = _ICommonMethod.GetPermissionsWithUserType(LocationID, UserType);
                if (lstServices != null)
                {
                    return Json(lstServices, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult EditUnVerifiedUser(string usr)
        {
            try
            {
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                var _UserModel = _ICommonMethod.LoadUnVerifiedInvitedUser(usr);
                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
                if (_UserModel.UserModel.UserType == 1)//Global Admin
                {
                    return View("~/Views/GlobalAdmin/Index.cshtml", _UserModel);

                }
                else if (_UserModel.UserModel.UserType == Convert.ToInt64(UserType.Manager))//Manager
                {
                    return View("~/Views/Manager/Manager.cshtml", _UserModel);

                }
                else if (_UserModel.UserModel.UserType == Convert.ToInt64(UserType.Employee))//Employee
                {
                    return View("~/Views/Employee/Employee.cshtml", _UserModel);

                }
                else if (_UserModel.UserModel.UserType == Convert.ToInt64(UserType.Client))//Client
                {
                    return View("~/Views/Client/myClient.cshtml", _UserModel);

                }
                else if (_UserModel.UserModel.UserType == Convert.ToInt64(UserType.ITAdministrator))//IT Admin
                {
                    return View("~/Views/ITAdministrator/ITAdministrator.cshtml", _UserModel);

                }
                else if (_UserModel.UserModel.UserType == Convert.ToInt64(UserType.Administrator))//Administrator
                {
                    return View("~/Views/Administrator/Administrator.cshtml", _UserModel);

                }
                else
                {
                    return View("Error");
                }

            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }

        //[HttpPut]
        //public JsonResult IsVerifiedUser(string UserID)
        //{
        //    DARModel objDAR;
        //    try
        //    {
        //        eTracLoginModel ObjLoginModel = null;
        //        if (Session["eTrac"] != null)
        //        {
        //            long userid = 0;
        //            if (!string.IsNullOrEmpty(UserID))
        //            {
        //                UserID = Cryptography.GetDecryptedData(UserID, true);
        //                long.TryParse(UserID, out userid);
        //            }
        //            ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);

        //            objDAR = new DARModel();
        //            objDAR.LocationId = ObjLoginModel.LocationID;
        //            objDAR.UserId = ObjLoginModel.UserId;
        //            objDAR.CreatedBy = ObjLoginModel.UserId;
        //            objDAR.CreatedOn = DateTime.Now;

        //            Tuple<int, string> rec_Result = IsVerifiedUserList(userid, ObjLoginModel.UserId, objDAR);
        //            return Json(new { result = rec_Result.Item1, message = rec_Result.Item2 });

        //        }
        //        else
        //        {
        //            return Json(new { result = 0, message = "Session has expired" });
        //        }
        //    }
        //    catch (Exception ex) { return Json(new { result = 0, message = ex.Message.ToString() }); }
        //}

        [HttpPost]
        public JsonResult IsVerifiedUserList(string UserID)
        {
            DARModel objDAR;
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                {
                    long userid = 0;
                    if (!string.IsNullOrEmpty(UserID))
                    {
                        UserID = Cryptography.GetDecryptedData(UserID, true);
                        long.TryParse(UserID, out userid);
                    }
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);

                    objDAR = new DARModel();
                    objDAR.LocationId = ObjLoginModel.LocationID;
                    objDAR.UserId = ObjLoginModel.UserId;
                    objDAR.CreatedBy = ObjLoginModel.UserId;
                    objDAR.CreatedOn = DateTime.UtcNow;


                    var rec_Result = _IUser.UpdateVerifyUser(userid, ObjLoginModel.UserId, objDAR);
                    if (rec_Result == Result.UpdatedSuccessfully)
                    {
                        return Json("User has been successfully verified.");
                    }
                    else if (rec_Result == Result.DoesNotExist)
                    {
                        return Json("User does not exist in the application");
                    }
                    else if (rec_Result == Result.ExistRecord)
                    {
                        return Json("You can't verified this user because this user has already verified.");
                    }
                    else { return Json(rec_Result); }

                }
                else
                {
                    return Json("Session has expired");
                }
            }
            catch (Exception ex) { return Json(ex.Message.ToString()); }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created On : 08/04/2017
        /// Method checking details of any work order assigned to user before delete.
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CheckingContinuousAssign(string UserID)
        {
            try
            {       
                long userid = 0;
                if (!string.IsNullOrEmpty(UserID))
                {
                    UserID = Cryptography.GetDecryptedData(UserID, true);
                    long.TryParse(UserID, out userid);
                }
                var result = _IWorkRequestAssignment.CheckingContinuousWorkRequestForUser(userid);
                return PartialView("_ContinuesWorkAssignUser", result);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error");
            }

        }


    }
}