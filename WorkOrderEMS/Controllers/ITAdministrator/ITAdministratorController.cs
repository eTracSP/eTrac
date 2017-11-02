using System;
using System.Configuration;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers.ITAdministrator
{
    [Authorize]
    public class ITAdministratorController : Controller
    {
        //
        // GET: /ITAdministrator/

        private readonly ICommonMethod _ICommonMethod;
        private readonly IGlobalAdmin _IGlobalAdmin;
        private readonly IClientManager _IClientManager;
        private readonly IWorkRequestAssignment _IWorkRequestAssignment;
        private readonly IUser _IUser;

        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        //string PWDGUIDMaxLength = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PWDGUIDMaxLength"]);
        //int pwdmaxlendth = 10;

        public ITAdministratorController(ICommonMethod _ICommonMethod, IClientManager _IClientManager, IGlobalAdmin _IGlobalAdmin, IUser _IUser)
        {
            this._ICommonMethod = _ICommonMethod; this._IClientManager = _IClientManager; this._IGlobalAdmin = _IGlobalAdmin;
            this._IUser = _IUser;
        } //int.TryParse(PWDGUIDMaxLength, out pwdmaxlendth); }

        public ActionResult Index(string usr)
        {
            //ViewBag.Country = _ICommonMethod.GetAllcountries();
            //var _UserModel = _ICommonMethod.LoadInvitedUser(usr);
            //ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("JOBTITLE");
            //ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
            //var tt = "/Views/Administrator/Administrator.cshtml";
            //return View(tt, _UserModel);
            //eTracLoginModel ObjLoginModel = null;
            //long LocationID = 0;
            //if (Session["eTrac"] != null)
            //{

            //    ObjLoginModel = (eTracLoginModel)Session["eTrac"];

            //    LocationID = Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]);
            //}
            //ObjectParameter obj_ObjectParameter = new ObjectParameter("TotalRecords", typeof(int));
            //ObjectParameter obj_ObjectParameter2 = new ObjectParameter("TotalRecords", typeof(int));
            //GlobalAdminManager obj_GlobalAdminManager = new GlobalAdminManager();
            //ViewBag.AllWorkOrder = obj_GlobalAdminManager.GetAllWorkRequestAssignment(0, 0, "GetAllWorkRequestAssignment", 1, 100000000, "WorkRequestAssignmentID", "asc", "", LocationID, ObjLoginModel.UserId, DateTime.Now, DateTime.Now, "", obj_ObjectParameter).Count();
            //ViewBag.AllAssignedWorkOrder = obj_GlobalAdminManager.GetAllWorkRequestAssignment(0, 0, "GetAllAssignedWorkRequest", 1, 100000000, "WorkRequestAssignmentID", "asc", "", LocationID, ObjLoginModel.UserId, DateTime.Now, DateTime.Now, "", obj_ObjectParameter2).Count();
            //UserManager obj_UserManager = new UserManager();
            //ViewBag.NoAssignedUsers = obj_UserManager.GetNotAssignedUsers(ObjLoginModel.UserId, 1, "Name", "asc", 100000000, "", "", obj_ObjectParameter2).Count();
            //ViewBag.AdministratorCount = _IGlobalAdmin.GetTotalManagerCount("Administrator", LocationID, ObjLoginModel.UserId);
            //ViewBag.ManagerCount = _IGlobalAdmin.GetTotalManagerCount("Manager", LocationID, ObjLoginModel.UserId);
            //ViewBag.EmployeeCount = _IGlobalAdmin.GetTotalManagerCount("Employee", LocationID, ObjLoginModel.UserId);
            //ViewBag.ClientCount = _IGlobalAdmin.GetTotalManagerCount("Client", LocationID, ObjLoginModel.UserId);
            return View();

        }

        /// <summary>Create
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-18-2014</CreatedOn>
        /// <CreatedFor>Load UI for Create New User</CreatedFor>
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        public ActionResult Create(string usr)
        {
            try
            {
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                var _UserModel = _ICommonMethod.LoadInvitedUser(usr);
                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
                return View("ITAdministrator", _UserModel);
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }

        /// <summary>Create
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-18-2014</CreatedOn>
        /// <CreatedFor>POST method for Create New User</CreatedFor>
        /// </summary>
        /// <param name="ObjUserModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(QRCModel ObjUserModel)
        {
            DARModel objDAR = null;

            try
            {
                eTracLoginModel ObjLoginModel = null;
                long LocId = 0;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
                if (Session["eTrac_SelectedDasboardLocationID"] != null)
                {
                    LocId = (long)Session["eTrac_SelectedDasboardLocationID"];
                }

                //if (ModelState.IsValid)
                //{

                if (ObjUserModel != null && ObjUserModel.UserModel != null) //&& ObjUserModel.UserModel.UserId == 0
                {
                    if (ObjUserModel.UserModel.UserId == 0)
                    {
                        #region password
                        //ObjUserModel.UserModel.Password = _ICommonMethod.CreateRandomPassword();
                        /*
                        ObjUserModel.UserModel.Password = Guid.NewGuid().ToString();
                        ObjUserModel.UserModel.Password = ObjUserModel.UserModel.Password.Length > pwdmaxlendth ? ObjUserModel.UserModel.Password.Substring(0, pwdmaxlendth) : ObjUserModel.UserModel.Password;
                        ObjUserModel.UserModel.Password = Cryptography.GetEncryptedData(ObjUserModel.UserModel.Password, true);
                        //ObjUserModel.UserModel.Password = (!string.IsNullOrEmpty(ObjUserModel.UserModel.Password)) ? Cryptography.GetEncryptedData(ObjUserModel.UserModel.Password, true) : ObjUserModel.UserModel.Password;
                        */
                        if (!String.IsNullOrEmpty(ObjUserModel.UserModel.Password))
                        {
                            ObjUserModel.UserModel.Password = Cryptography.GetEncryptedData(ObjUserModel.UserModel.Password, true);
                        }
                        #endregion password

                        ObjUserModel.UserModel.CreatedBy = ObjLoginModel.UserId;
                        ObjUserModel.UserModel.CreatedDate = DateTime.UtcNow;
                        ObjUserModel.UserModel.IsDeleted = false;


                        objDAR = new DARModel();
                        objDAR.LocationId = ObjLoginModel.LocationID;
                        objDAR.UserId = ObjLoginModel.UserId;
                        objDAR.CreatedBy = ObjLoginModel.UserId;
                        objDAR.CreatedOn = DateTime.UtcNow;

                        objDAR.TaskType = (long)TaskTypeCategory.UserCreation;
                        objDAR.ActivityDetails = DarMessage.NewITAdministratorCreatedDar(ObjLoginModel.Location);
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
                        objDAR.LocationId = ObjLoginModel.LocationID;
                        objDAR.UserId = ObjLoginModel.UserId;
                        objDAR.ModifiedBy = ObjLoginModel.UserId;
                        objDAR.ModifiedOn = DateTime.UtcNow;

                        objDAR.TaskType = (long)TaskTypeCategory.UserUpdate;
                        objDAR.ActivityDetails = DarMessage.ITAdministratorUpdatedDar(ObjLoginModel.Location);

                    }
                    if (ObjUserModel.UserModel.ProfileImage != null)
                    {
                        string ImageName = ObjLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + ObjUserModel.UserModel.ProfileImage.FileName.ToString();
                        CommonHelper obj_CommonHelper = new CommonHelper();
                        obj_CommonHelper.UploadImage(ObjUserModel.UserModel.ProfileImage, Server.MapPath(ConfigurationManager.AppSettings["ProfilePicPath"]), ImageName);
                        ObjUserModel.UserModel.ProfileImageFile = ImageName;
                    }
                    long QRCID = 0;
                    //Result result = _IClientManager.SaveClient(ObjUserModel.UserModel, out QRCID, true, objDAR); // commented by vijay sahu on 18 feb 2015
                    //Updated By Bhushan Dod on 30/05/2016 for location id parameter not send properly send 0 so need to update location id.
                    //Result result = _IClientManager.SaveClient(ObjUserModel.UserModel, out QRCID, true, objDAR, 0, ObjLoginModel.UserId, "");//added by vijay sahu on15 feb 2015
                    Result result = _IClientManager.SaveClient(ObjUserModel.UserModel, out QRCID, true, objDAR, ObjLoginModel.LocationID, ObjLoginModel.UserId, "");
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



                        var abc = _ICommonMethod.GetLocationDetailsById(LocId);

                        EmailHelper objEmailHelper = new EmailHelper();
                        objEmailHelper.emailid = ObjUserModel.UserModel.UserEmail;
                        objEmailHelper.UserName = ObjUserModel.UserModel.AlternateEmail;
                        objEmailHelper.UserType = ObjUserModel.UserModel.UserType;
                        objEmailHelper.FirstName = ObjUserModel.UserModel.FirstName;
                        objEmailHelper.LastName = ObjUserModel.UserModel.LastName;
                        objEmailHelper.Password = Cryptography.GetDecryptedData(ObjUserModel.UserModel.Password, true);
                        objEmailHelper.LocationName = abc.LocationName;
                        objEmailHelper.LocAddress = ObjUserModel.UserModel.Address.Address1; // here locAddress means user Address
                        objEmailHelper.MailType = "CreateNewUser";

                        string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], System.Globalization.CultureInfo.InvariantCulture);
                        HostingPrefix = HostingPrefix + "Manager/Employee?usr=" + Cryptography.GetEncryptedData(ObjUserModel.UserModel.UserId.ToString(), true);

                        objEmailHelper.RegistrationLink = HostingPrefix;

                        objEmailHelper.SendEmailWithTemplate();





                        #endregion Email to IT Admin User

                        #endregion EmailHelper

                        /* test mail code call*/
                        #region EmailHelper


                        ////if (QRCID > 0)
                        ////{
                        ////    EmailHelper objEmailHelper = new EmailHelper();
                        ////    objEmailHelper.emailid = ObjUserModel.UserModel.UserEmail;
                        ////    objEmailHelper.FirstName = ObjUserModel.UserModel.FirstName;
                        ////    objEmailHelper.LastName = ObjUserModel.UserModel.LastName;
                        ////    objEmailHelper.MailType = "REGISTRATIONMAIL";
                        ////    string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"]);
                        ////    /*

                        ////    string UserLink = "";                           //Enum.TryParse(userType, out _UserType);                           //long _userType = (long)_UserType;                        
                        ////    long _userType = ObjUserModel.UserModel.UserType;
                        ////    switch (_userType)
                        ////    {
                        ////        case (long)UserType.Manager:
                        ////            UserLink = "GlobalAdmin/Manager";
                        ////            break;
                        ////        case (long)UserType.Employee:
                        ////            UserLink = "Manager/Employee";
                        ////            break;
                        ////        case (long)UserType.Client:
                        ////            UserLink = "Client/Client";
                        ////            break;
                        ////        default:
                        ////            UserLink = "Error";
                        ////            break;
                        ////    }

                        ////    HostingPrefix = HostingPrefix + UserLink + "?usr=" + Cryptography.GetEncryptedData(userid.ToString(), true);
                        ////     */
                        ////    HostingPrefix = HostingPrefix + "?vrfy=" + Cryptography.GetEncryptedData(ObjUserModel.UserModel.Password.ToString(), true);
                        ////    objEmailHelper.RegistrationLink = HostingPrefix;

                        ////    #region comments
                        ////    // objEmailHelper.RegistrationLink = DomainName + "/?flag=Registration&id=" + System.Web.HttpUtility.UrlPathEncode(Cryptography.GetEncryptedData(UserId.ToString(), true));
                        ////    //objEmailHelper.RegistrationLink = DomainName + "/?flag=Registration&id=" + System.Web.HttpUtility.UrlPathEncode(Cryptography.GetEncryptedData(UserId.ToString(), true));
                        ////    // objEmailHelper.RegistrationCode = objRegistrationModel.EmailVerifcationCode;
                        ////    #endregion comments

                        ////    objEmailHelper.SendEmailwithTemplate();
                        ////    //return Result.EmailSendSuccessfully;

                        ////}

                        #endregion EmailHelper
                        /* test mail code call*/

                        ViewBag.Message = CommonMessage.SaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        ModelState.Clear();
                        //return View("ITAdministrator");//return RedirectToAction("Create ", "GlobalAdmin");
                        ObjUserModel = _ICommonMethod.LoadInvitedUser(string.Empty);
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
                        ModelState.Clear();
                        //return RedirectToAction("index", "GlobalAdmin");
                        ObjUserModel = _ICommonMethod.LoadInvitedUser(string.Empty);
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
                /*
                System.Data.Entity.Core.Objects.ObjectParameter paramTotalRecords = new System.Data.Entity.Core.Objects.ObjectParameter("TotalRecords", typeof(int));
                ObjUserModel.JobTitleList = _ICommonMethod.GetGlobalCodeData("JOBTITLE");
                ObjUserModel.LocationList = _IGlobalAdmin.GetAllLocationList(0, "GetAllLocation", 1, 10000, "LocationName", "desc", "", paramTotalRecords);
                paramTotalRecords = null;
                */
            }

            ViewBag.UpdateMode = false;
            //ObjUserModel.UserModel = _IClientManager.GetClientByID(ObjUserModel.UserModel.UserId, "GetUserByID", null, null, null, null, null);

            return View("ITAdministrator", ObjUserModel);

        }

        /// <summary>
        /// TO CHECK DUPLICATE EMPLOYEE ID
        /// </summary>
        /// <CreatedBy>Vijay Sahu</CreatedBy>
        /// <Created>26/03/2015</Created>
        /// <param name="empId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult isEmployeeIdExists(string empId)
        {


            return Json(new { status = _IGlobalAdmin.isEmployeeIdExists(empId) }, JsonRequestBehavior.AllowGet);
        }

    }


}