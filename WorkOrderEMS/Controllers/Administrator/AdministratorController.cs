using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers.Administrator
{
    [Authorize]
    public class AdministratorController : Controller
    {
        //
        // GET: /Administrator/
        private readonly ICommonMethod _ICommonMethod;
        private readonly IGlobalAdmin _IGlobalAdmin;
        private readonly IClientManager _IClientManager;
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], System.Globalization.CultureInfo.InvariantCulture);
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();

        private string path = ConfigurationManager.AppSettings["ProjectLogoPath"];
        //private string PWDGUIDMaxLength = ConfigurationManager.AppSettings["PWDGUIDMaxLength"];
        //int pwdmaxlendth = 10;

        public AdministratorController(ICommonMethod _ICommonMethod, IGlobalAdmin _IGlobalAdmin, IClientManager _IClientManager)
        { this._ICommonMethod = _ICommonMethod; this._IGlobalAdmin = _IGlobalAdmin; this._IClientManager = _IClientManager; }
        //int.TryParse(PWDGUIDMaxLength, out pwdmaxlendth); }

        public ActionResult Index1()
        {
            return View();
        }
        /// <summary>After click on side menu this page will be called for creating administrator. 
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
                return View("Administrator", _UserModel);
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }


        /// <summary>After login this page will be called. 
        /// Modified by vijay sahu on 20 feb 2015
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-18-2014</CreatedOn>
        /// <CreatedFor>Load UI for Create New User</CreatedFor>
        /// </summary>
        /// <ModifiedBy>Manoj Jaswal</ModifiedBy>
        /// <ModifiedDate>2015-03-11</ModifiedDate>
        /// <param name="usr"></param>
        /// <returns></returns>
        public ActionResult Index(string usr)
        {
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
        /// <CreatedFor>POST method for Create New User</CreatedFor>
        /// </summary>
        /// <param name="ObjUserModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(QRCModel ObjUserModel)
        {
            DARModel objDAR = null; long LocId = 0;
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
                if (Session["eTrac_SelectedDasboardLocationID"] != null)
                {
                    LocId = (long)Session["eTrac_SelectedDasboardLocationID"];
                }
                //if (ModelState.IsValid)
                //{
                CommonHelper ObjCommonHelper = new CommonHelper();
                HttpPostedFileBase ProfileImage = null;
                string msg = string.Empty;
                if (ObjUserModel != null && ObjUserModel.UserModel != null) //&& ObjUserModel.UserModel.UserId == 0
                {
                    objDAR = new DARModel();
                    objDAR.LocationId = ObjLoginModel.LocationID;
                    objDAR.UserId = ObjLoginModel.UserId;
                    objDAR.CreatedBy = ObjLoginModel.UserId;
                    objDAR.CreatedOn = DateTime.UtcNow;
                    objDAR.TaskType = (long)TaskTypeCategory.UserCreation;

                    if (ObjUserModel.UserModel.ProfileImage != null)
                        ProfileImage = ObjUserModel.UserModel.ProfileImage;
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

                        objDAR.TaskType = (long)TaskTypeCategory.UserCreation;
                        objDAR.ActivityDetails = DarMessage.NewAdministratorCreatedDar(ObjLoginModel.Location);

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
                        objDAR.TaskType = (long)TaskTypeCategory.UserUpdate;
                        objDAR.ActivityDetails = DarMessage.AdministratorUpdatedDar(ObjLoginModel.Location);
                    }
                    if (ObjUserModel.UserModel.ProfileImage != null)
                    {
                        string ImageName = ObjLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + ObjUserModel.UserModel.ProfileImage.FileName.ToString();
                        CommonHelper obj_CommonHelper = new CommonHelper();
                        obj_CommonHelper.UploadImage(ObjUserModel.UserModel.ProfileImage, Server.MapPath(ConfigurationManager.AppSettings["ProfilePicPath"]), ImageName);
                        ObjUserModel.UserModel.ProfileImageFile = ImageName;
                    }
                    long QRCID = 0;
                    //Result result = _IClientManager.SaveClient(ObjUserModel.UserModel, out QRCID, true, objDAR);

                    Result result = Result.LoginFailed; // if LocId is null then we will consider that the session is expired. 
                    if (LocId > 0)
                    {
                        result = _IClientManager.SaveClient(ObjUserModel.UserModel, out QRCID, true, objDAR, LocId, ObjLoginModel.UserId, "");
                    }
                    if (result == Result.Completed)
                    {
                        ViewBag.Message = CommonMessage.SaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
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
                        HostingPrefix = HostingPrefix + "Manager/Employee?usr=" + Cryptography.GetEncryptedData(ObjUserModel.UserModel.UserId.ToString(), true);
                        //HostingPrefix = HostingPrefix + "?usr=" + Cryptography.GetEncryptedData(ObjUserModel.UserModel.UserId.ToString(), true);
                        objEmailHelper.RegistrationLink = HostingPrefix;

                        objEmailHelper.SendEmailWithTemplate();
                        ObjUserModel = _ICommonMethod.LoadInvitedUser(string.Empty);
                        //CommonMethodAdmin objCMA = new CommonMethodAdmin();
                        //                        objCMA.AssignLocationToAdminUser(LocId,obj
                        ModelState.Clear();
                    }
                    else if (result == Result.DuplicateRecord)
                    {
                        ViewBag.Message = CommonMessage.DuplicateRecordEmailIdMessage();
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

            return View("Administrator", ObjUserModel);
        }
    }
}