using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WorkOrderEMS.App_Start;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Exception_B;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;

namespace WorkOrderEMS.Controllers.Login
{

    public class LoginController : Controller
    {
        //
        // GET: /Login/

        private readonly ILogin _ILogin;
        private readonly ICommonMethod _ICommonMethod;

        private UserManager<UserModel> UserManager;
        public LoginController(ILogin _ILogin, ICommonMethod ICommonMethod)
        {
            this._ILogin = _ILogin;
            this._ICommonMethod = ICommonMethod;
        }

        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"]);
        //////////[CustomActionFilter]
        //////////public ActionResult Index(string ReturnUrl)
        //////////{
        //////////    ViewBag.AlertMessageClass = new AflertMessageClass().Danger;
        //////////    ViewBag.Message = TempData["logout"];
        //////////    //ViewBag.Message = CommonMessage.SessionExpired();
        //////////    return View();

        //////////}


        //////////[AllowAnonymous]
        //////////[HttpPost]
        //////////public ActionResult Index(eTracLoginModel eTracLogin)
        //////////{
        //////////    try
        //////////    {
        //////////        string loginMessage = ""; if (ModelState.IsValid)
        //////////        {
        //////////            eTracLoginModel result = _ILogin.AuthenticateUser(eTracLogin);
        //////////            if (result.UserId > 0)
        //////////            {
        //////////                this.CreateAuthenticateFormsTicket(result);
        //////////                return RedirectToAction("Index", "GlobalAdmin");
        //////////            }//else { ModelState.AddModelError("", "User not found. Please check UserName or Password"); }
        //////////            else { loginMessage = "User not found. Please check UserName or Password"; }

        //////////        }//else { ModelState.AddModelError("", "Invalid UserName or Password"); }
        //////////        else { loginMessage = "Invalid UserName or Password"; }
        //////////        ViewBag.Message = loginMessage; ViewBag.AlertMessageClass = ObjAlertMessageClass.Info;
        //////////    }
        //////////    catch (Exception ex)
        //////////    { ViewBag.Error = ex.Message; ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; }//ModelState.AddModelError("", ex.Message);
        //////////    return View("Index", eTracLogin);
        //////////}

        ///////////// <summary>CreateAuthenticateFormsTicket
        ///////////// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        ///////////// <CreatedFor>Create Authenticate Forms Ticket</CreatedFor>
        ///////////// <CreatedOn>Dec-01-2014</CreatedOn>
        ///////////// </summary>
        ///////////// <param name="eTracLogin"></param>
        //////////[NonAction]
        //////////private void CreateAuthenticateFormsTicket(eTracLoginModel eTracLogin)
        //////////{
        //////////    try
        //////////    {
        //////////        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
        //////////                 1,
        //////////                 eTracLogin.UserId.ToString(),                                 //user Name
        //////////                 DateTime.Now,
        //////////                 DateTime.Now.AddMinutes(30),                          // expiry in 30 min
        //////////                 eTracLogin.RememberMe,
        //////////                 eTracLogin.UserRoleId.ToString());
        //////////        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
        //////////        Response.Cookies.Add(cookie);
        //////////        Session["eTrac"] = eTracLogin;
        //////////    }
        //////////    catch (Exception ex)
        //////////    { throw ex; }
        //////////}

        [CustomActionFilter]
        public ActionResult Index(string ReturnUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                //, string VendorID
                ViewBag.AlertMessageClass = new AlertMessageClass().Danger;
                ViewBag.Message = TempData["logout"];
                eTracLoginModel eTracLogin = new eTracLoginModel();
                if (Request.Cookies["eTrac_info"] != null)
                {

                    eTracLogin.UserName = Request.Cookies["eTrac_info"]["UserName"];
                    // Commented By Bhushan on 17/Oct/2016 for client don't want to remember pwd. As functionality username remember not pwd.
                    // eTracLogin.Password = Cryptography.GetDecryptedData(Request.Cookies["eTrac_info"]["pwd"], true);
                    eTracLogin.RememberMe = true;
                    //return View(eTracLogin);
                }
                return View(eTracLogin);
            }
            else
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    switch (objLoginSession.UserRoleId)
                    {
                        case ((Int64)(UserType.GlobalAdmin)):
                            return RedirectToAction("Index", "GlobalAdmin");
                            break;
                        case ((Int64)(UserType.ITAdministrator)):

                            return RedirectToAction("Index", "ITAdministrator");
                            break;
                        case ((Int64)(UserType.Administrator)):

                            return RedirectToAction("Index", "Administrator");
                            break;
                        case ((Int64)(UserType.Manager)):


                            return RedirectToAction("Dashboard", "Manager");
                            break;
                        case ((Int64)(UserType.Employee)):

                            return RedirectToAction("Index", "Employee");
                            break;
                        case ((Int64)(UserType.Client)):
                            return RedirectToAction("Index", "Client");
                            break;
                        default:

                            return RedirectToAction("Index", "Login");
                            break;


                    }

                }
                else
                {
                    return View(objLoginSession);
                }


            }


            //if (VendorID != null && VendorID.Trim().ToString().Length > 0)
            //{

            //    try
            //    {
            //        var abc = Cryptography.GetDecryptedData(VendorID, true);

            //        if (Convert.ToInt32(abc) > 0)
            //        {
            //            FormsAuthenticationTicket VendorAuthTicket = new FormsAuthenticationTicket(
            //                     "eTrack_VendorIdForEditAfterDeclinedByManager", true, 10);                        // expiry in 10 min

            //            string formsCookieStr = string.Empty;

            //            formsCookieStr = FormsAuthentication.Encrypt(VendorAuthTicket);

            //            HttpCookie FormsCookie = new HttpCookie("eTrack_VendorIdForEditAfterDeclinedByManager", FormsAuthentication.Encrypt(VendorAuthTicket));

            //            FormsCookie.Expires = DateTime.Now.AddMinutes(10);
            //            FormsCookie["VendorID"] = VendorID;

            //            Response.Cookies.Add(FormsCookie);
            //        }
            //    }
            //    catch
            //    {
            //    }




            //}

            //ViewBag.Message = CommonMessage.SessionExpired();


        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(eTracLoginModel eTracLogin)
        {
            try
            {
                //TimeZoneInfo nyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                //DateTime nyTime = GetLocalDateTime(DateTime.UtcNow, nyTimeZone);

                //if (nyTimeZone.IsDaylightSavingTime(nyTime))         
                string loginMessage = "";
                if (ModelState.IsValid)
                {
                    eTracLoginModel result = _ILogin.AuthenticateUser(eTracLogin);
                    //result.RememberMe = eTracLogin.RememberMe;
                    if (result.UserId > 0)
                    {
                        this.CreateAuthenticateFormsTicket(result);
                        Common_B obj_Common_B = new Common_B();
                        Session["eTrac_DashboardSetting"] = obj_Common_B.getUserDasboardSettings(result.UserId);
                        Session["eTrac_SelectedDasboardLocationID"] = result.LocationID;
                        Session["eTrac_UserRoles"] = this.Get_UserAssignedRoles();
                        Session["eTrac_DashboardWidget"] = this.GetUserDashboardWidget();
                        Session["eTrac_LocationServices"] = obj_Common_B.GetLocationServicesByLocationID(result.LocationID, result.UserRoleId);
                        switch (result.UserRoleId)
                        {
                            case ((Int64)(UserType.GlobalAdmin)):
                                Session["eTrac_UserLocations"] = _ILogin.GetUserAssignedLocations(result.UserRoleId, result.UserId);
                                Session["eTrac_UserRoles"] = Session["eTrac_LocationServices"]; // this line has been added by vijay bcz if usetype is GAdmin or ITAdmin then this type of users will be able too see all services which is assigned to this current location.
                                return RedirectToAction("Index", "GlobalAdmin");
                                break;
                            case ((Int64)(UserType.ITAdministrator)):
                                Session["eTrac_UserLocations"] = _ILogin.GetUserAssignedLocations(result.UserRoleId, result.UserId);
                                Session["eTrac_UserRoles"] = Session["eTrac_LocationServices"];
                                return RedirectToAction("Index", "ITAdministrator");
                                break;
                            case ((Int64)(UserType.Administrator)):
                                Session["eTrac_UserLocations"] = _ILogin.GetAdminAssignedLocation(result.UserId);
                                return RedirectToAction("Index", "Administrator");
                                break;
                            case ((Int64)(UserType.Manager)):
                                Session["eTrac_UserLocations"] = _ILogin.GetManagerAssignedLocation(result.UserId);

                                #region this code will execute only when manager declined vendor from vendor email verification page.
                                try
                                {
                                    if (Request.Cookies["eTrack_VendorIdForEditAfterDeclinedByManager"] != null)
                                    {
                                        string isVendorIDExists = Request.Cookies["eTrack_VendorIdForEditAfterDeclinedByManager"]["VendorID"];
                                        if (isVendorIDExists != null)
                                        {
                                            var abc = Cryptography.GetDecryptedData(isVendorIDExists, true);

                                            if (Convert.ToInt32(abc) > 0)
                                            {
                                                //string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], System.Globalization.CultureInfo.InvariantCulture);
                                                //var adfadsf = HostingPrefix + "/Manager/EditRegisterVendor/?vdr=" + isVendorIDExists;
                                                //Response.Redirect(adfadsf);
                                                return RedirectToAction("EditRegisterVendor", "Manager", new { vdr = isVendorIDExists });
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                }
                                #endregion // by vijay sahu on 2 july 2015

                                #region This Code Will Execute if Vehicle Declined by Manager and after login redirect to edit vehicle
                                try
                                {
                                    if (Request.Cookies["eTrac_VehicleIdForEditAfterDeclinedByManager"] != null)
                                    {
                                        string isVehicleIDExists = Request.Cookies["eTrac_VehicleIdForEditAfterDeclinedByManager"]["QRCID"];
                                        if (isVehicleIDExists != null)
                                        {
                                            var abc = Cryptography.GetDecryptedData(isVehicleIDExists, true);

                                            if (Convert.ToInt32(abc) > 0)
                                            {
                                                //string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], System.Globalization.CultureInfo.InvariantCulture);
                                                var redirectURL = HostingPrefix + "QRCSetup/VehicleRegistration/?qr=" + isVehicleIDExists;
                                                Response.Redirect(redirectURL);
                                                //return RedirectToAction("VehicleRegistration", "QRCSetup", new { qr = isVehicleIDExists });
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                }
                                #endregion // by Bhushan Dod on 22 September 2015

                                return RedirectToAction("Dashboard", "Manager");
                                break;
                            case ((Int64)(UserType.Employee)):
                                Session["eTrac_UserLocations"] = _ILogin.GetEmployeeAssignedLocation(result.UserId);
                                return RedirectToAction("Index", "Employee");
                                break;
                            case ((Int64)(UserType.Client)):
                                //Session["eTrac_UserLocations"] = _ILogin.GetEmployeeAssignedLocation(result.UserId);
                                return RedirectToAction("Index", "Client");
                                break;
                        }

                    }//else { ModelState.AddModelError("", "User not found. Please check UserName or Password"); }
                    else { loginMessage = "User not found. Please check Username or Password"; }

                }//else { ModelState.AddModelError("", "Invalid UserName or Password"); }
                else { loginMessage = "Invalid Username or Password"; }
                ViewBag.Message = loginMessage; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            catch (Exception ex)
            {
                Exception_B.exceptionHandel_Runtime(ex, "public ActionResult Index(eTracLoginModel eTracLogin)", "from loginController", eTracLogin);
                ViewBag.Error = ex.Message; ViewBag.Message = "Something went wrong. Please contact support team."; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }//ModelState.AddModelError("", ex.Message);
            return View("Index", eTracLogin);
        }

        /// <summary>CreateAuthenticateFormsTicket
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedFor>Create Authenticate Forms Ticket</CreatedFor>
        /// <CreatedOn>Dec-01-2014</CreatedOn>
        /// </summary>
        /// <param name="eTracLogin"></param>
        [NonAction]
        private void CreateAuthenticateFormsTicket(eTracLoginModel eTracLogin)
        {
            try
            {
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                             1,
                             eTracLogin.UserId.ToString(),                                 //user Name
                             DateTime.Now,
                             DateTime.Now.AddMinutes(30),                          // expiry in 30 min
                             eTracLogin.RememberMe,
                             eTracLogin.UserRoleId.ToString());

                if (eTracLogin.RememberMe)
                {
                    string formsCookieStr = string.Empty;

                    formsCookieStr = FormsAuthentication.Encrypt(authTicket);

                    HttpCookie FormsCookie = new HttpCookie("eTrac_info", formsCookieStr);
                    FormsCookie.Expires = DateTime.Now.AddDays(1);

                    FormsCookie["UserName"] = eTracLogin.UserName;
                    // Commented By Bhushan on 17/Oct/2016 for client don't want to remember pwd. As functionality username remember not pwd.
                    //FormsCookie["pwd"] = eTracLogin.Password;

                    HttpContext.Response.Cookies.Add(FormsCookie);

                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    HttpCookie myCookie = new HttpCookie("eTrac_info");
                    myCookie.Expires = DateTime.Now;
                    Response.Cookies.Add(myCookie);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                    Response.Cookies.Add(cookie);
                }
                Session["eTrac"] = eTracLogin;
            }
            catch (Exception ex)
            { throw ex; }
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index_(eTracLoginModel eTracLogin, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(eTracLogin.Email, eTracLogin.Password);
                if (user != null)
                {
                    await SignInAsync(user, eTracLogin.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form


            //if (ModelState.IsValid && WebSecurity.Login(eTracLogin.Email, eTracLogin.Password, persistCookie: eTracLogin.RememberMe))
            //{
            //    return RedirectToLocal(returnUrl);
            //}

            // If we got this far, something failed, redisplay form
            //ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(eTracLogin);
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private async Task SignInAsync(UserModel user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private IAuthenticationManager _authnManager;

        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                if (_authnManager == null)
                    _authnManager = HttpContext.GetOwinContext().Authentication;
                return _authnManager;
            }
            set { _authnManager = value; }
        }

        /// <summary>LoginAsGlobalAdmin
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-20-2014</CreatedOn>
        /// <CreatedFor>Login As Global Admin</CreatedFor>
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginAsGlobalAdmin()
        {
            try
            {
                this.CreateAuthenticateFormsTicket(_ILogin.SetLogin(Convert.ToInt64(UserType.GlobalAdmin)));
                return RedirectToAction("Index", "GlobalAdmin");
            }
            catch (Exception ex)
            { ViewBag.Error = ex.Message; ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }

        /// <summary>LoginAsITAdministrator
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-20-2014</CreatedOn>
        /// <CreatedFor>Login As IT Administrator</CreatedFor>
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginAsITAdministrator()
        {
            try
            {
                this.CreateAuthenticateFormsTicket(_ILogin.SetLogin(Convert.ToInt64(UserType.ITAdministrator)));
                return RedirectToAction("Index", "ITAdministrator");
            }
            catch (Exception ex)
            { ViewBag.Error = ex.Message; ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }

        /// <summary>LoginAsAdministrator
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-20-2014</CreatedOn>
        /// <CreatedFor>Login As Administrator</CreatedFor>
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginAsAdministrator()
        {
            try
            {
                this.CreateAuthenticateFormsTicket(_ILogin.SetLogin(Convert.ToInt64(UserType.Administrator)));
                return RedirectToAction("Index", "Administrator");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                return View("Error");
            }
        }

        /// <summary>LoginAsManager
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-20-2014</CreatedOn>
        /// <CreatedFor>Login As Manager</CreatedFor>
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginAsManager()
        {
            try
            {
                this.CreateAuthenticateFormsTicket(_ILogin.SetLogin(Convert.ToInt64(UserType.Manager)));
                return RedirectToAction("Index", "Manager");
            }
            catch (Exception ex)
            { ViewBag.Error = ex.Message; ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }


        /// <summary>LoginAsEmployee
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-20-2014</CreatedOn>
        /// <CreatedFor>Login As Employee</CreatedFor>
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginAsEmployee()
        {
            try
            {
                this.CreateAuthenticateFormsTicket(_ILogin.SetLogin(Convert.ToInt64(UserType.Employee)));
                return RedirectToAction("Index", "Employee");
            }
            catch (Exception ex)
            { ViewBag.Error = ex.Message; ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }


        /// <summary>LoginAsClient
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-20-2014</CreatedOn>
        /// <CreatedFor>Login As Client</CreatedFor>
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginAsClient()
        {
            try
            {
                this.CreateAuthenticateFormsTicket(_ILogin.SetLogin(Convert.ToInt64(UserType.Client)));
                return RedirectToAction("Index", "Client");
            }
            catch (Exception ex)
            { ViewBag.Error = ex.Message; ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }

        /// <summary>LogOut
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-20-2014</CreatedOn>
        /// <CreatedFor>LogOut User and remove Session</CreatedFor>
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut(long id)
        {
            try
            {
                if (Session["eTrac"] != null)
                {
                    eTracLoginModel obj_eTracLoginModel = new eTracLoginModel();
                    obj_eTracLoginModel = (eTracLoginModel)Session["eTrac"];
                    Result result = _ILogin.LogoutWeb(id, obj_eTracLoginModel.LogId);
                    if (result == Result.Completed)
                    {
                        Session.Abandon();
                        FormsAuthentication.SignOut();
                        FormsAuthentication.RedirectToLoginPage();
                        return RedirectToAction("Index", "Login");

                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                        return RedirectToAction("Index", "Login");
                        // store the failure message in tempdata to display in view.
                    }
                }
                else
                {
                    ViewBag.Message = CommonMessage.SessionExpired();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                    return RedirectToAction("Index", "Login");
                    // store the failure message in tempdata to display in view.
                }


            }
            catch (Exception ex)
            { ViewBag.Error = ex.Message; ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }

        }

        [HttpPost]
        public JsonResult RecoveryEmailPassword(eTracLoginModel eTracLogin)
        {
            bool status = false;
            //status
            string message = ""; //string recoveryPassword = "";

            try
            {
                if (eTracLogin.RecoveryEmail != null && !string.IsNullOrEmpty(eTracLogin.RecoveryEmail))
                {
                    //status = _ILogin.RecoveryEmailPassword(eTracLogin, out message, out recoveryPassword);
                    status = _ILogin.RecoveryEmailPassword(eTracLogin, out message);
                    if (status) //ViewBag.ForgotPWDModalflag = true;
                    {
                        message = CommonMessage.RecoveryPasswordSent(eTracLogin.RecoveryEmail);

                        #region EmailHelper

                        //  #region Email to Manager User
                        //  RecoverNmailUserPassword(eTracLogin);
                        //  #endregion Email to Manager User
                        #endregion EmailHelper
                    }
                }
            }
            catch (Exception ex)
            { message = ex.Message; status = false; }
            return Json(new { Message = message, Response = status }, JsonRequestBehavior.AllowGet);

        }

        #region EmailHelper RecoverNmailUserPassword


        /// <summary>RecoverNmailUserPassword
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedFor>Recover and mail User Password</CreatedFor>
        /// <CreatedOn>Dec-02-2014</CreatedOn>
        /// </summary>
        /// <param name="eTracLogin"></param>
        /// <returns></returns>
        //[NonAction]
        //private bool RecoverNmailUserPassword(eTracLoginModel eTracLogin)
        //{
        //    try
        //    {
        //        EmailHelper objEmailHelper = new EmailHelper();
        //        objEmailHelper.emailid = eTracLogin.RecoveryEmail;
        //        objEmailHelper.FirstName = eTracLogin.RecoveryEmail;

        //        objEmailHelper.Password = Cryptography.GetDecryptedData(eTracLogin.Password, true);
        //        objEmailHelper.MailType = "FORGOTPASSWORD";
        //        string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"]);

        //        objEmailHelper.RegistrationLink = HostingPrefix;
        //        objEmailHelper.SendEmailwithTemplate();
        //    }
        //    catch (Exception ex) { throw ex; } return true;
        //}

        #endregion EmailHelper SendEmailToUser

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml"
            };
            ViewBag.Error = filterContext.Exception.Message;
            ViewBag.Message = filterContext.Exception.Message;
            ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            filterContext.ExceptionHandled = true;

        }
        /// <summary>
        /// To get User Assignrd Premissions
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CretaedDate>2015/2/20</CretaedDate>
        /// <returns></returns>
        private List<string> Get_UserAssignedRoles()
        {
            long locationId = 0;
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                locationId = (long)Session["eTrac_SelectedDasboardLocationID"];

            }

            return _ILogin.GetUserPremissionList(ObjLoginModel.UserId, ObjLoginModel.UserRoleId, locationId);

        }
        [HttpPost]
        public bool SetUserSelectedLocation(long LocationId, string LocationName)
        {
            try
            {
                if (Session["eTrac"] != null)
                {
                    Common_B obj_Common_B = new Common_B();
                    eTracLoginModel obj_eTracLoginModel = new eTracLoginModel();
                    obj_eTracLoginModel = (eTracLoginModel)Session["eTrac"];

                    //Added By Bhushan Dod on 24-April-2016 for maintaining log of active user by location if selected from loc ddl
                    _ILogin.ChangeLoginLogActiveStatus(obj_eTracLoginModel);

                    obj_eTracLoginModel.LocationID = LocationId;
                    obj_eTracLoginModel.Location = LocationName;
                    //Added By Bhushan Dod on 24-April-2016 for maintaining log of active logid by location if selected from loc ddl
                    var objNewLogID = _ILogin.InsertLoginLog(obj_eTracLoginModel);
                    obj_eTracLoginModel.LogId = objNewLogID.LogId;

                    Session["eTrac"] = obj_eTracLoginModel;
                    Session["eTrac_DashboardSetting"] = obj_Common_B.getUserDasboardSettings(obj_eTracLoginModel.UserId);
                    Session["eTrac_SelectedDasboardLocationID"] = LocationId;
                    Session["eTrac_UserRoles"] = this.Get_UserAssignedRoles();

                    Session["eTrac_LocationServices"] = obj_Common_B.GetLocationServicesByLocationID(LocationId, obj_eTracLoginModel.UserRoleId);
                    Session["eTrac_DashboardWidget"] = null;
                    Session["eTrac_DashboardWidget"] = this.GetUserDashboardWidget();

                    if (obj_eTracLoginModel.UserRoleId == 1 || obj_eTracLoginModel.UserRoleId == 5)// 1 - GlobalAdmin ,2 IT Admin
                    {
                        Session["eTrac_UserRoles"] = Session["eTrac_LocationServices"];
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        [AllowAnonymous]
        public ActionResult SendEmail(string email, string message)
        {


            var send = _ICommonMethod.SendEmailJustforTesting(email, message);

            return View("Index");
        }

        public WidgetList GetUserDashboardWidget()
        {
            long locationId = 0;
            eTracLoginModel ObjLoginModel = null;

            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                locationId = (long)Session["eTrac_SelectedDasboardLocationID"];
                return _ILogin.GetDashboardWidgetList(ObjLoginModel.UserId, locationId);
            }
            else
            {
                return null;
            }



        }
    }
}