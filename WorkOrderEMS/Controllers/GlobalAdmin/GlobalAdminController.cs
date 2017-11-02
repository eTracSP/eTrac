using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WorkOrderEMS.App_Start;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;

namespace WorkOrderEMS.Controllers.GlobalAdmin
{
    [Authorize]
    [CustomActionFilter]
    public class GlobalAdminController : Controller
    {
        //
        // GET: /SuperAdmin/
        private readonly ICommonMethod _ICommonMethod;
        private readonly IGlobalAdmin _IGlobalAdmin;
        private readonly ILogin _ILogin;
        private readonly IManageManager _IManageManager;
        private readonly IUser _IUser;
        private readonly IWorkRequestAssignment _IWorkRequestAssignment;
        private readonly IEmployeeManager _IEmployeeManager;
        private readonly IClientManager _IClientManager;
        private readonly IQRCSetup _IQRCSetup;

        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();

        private string path = ConfigurationManager.AppSettings["ProjectLogoPath"];
        private string WorkRequestImagepath = ConfigurationManager.AppSettings["WorkRequestImage"];
        private string eTracDefaultCountryString = ConfigurationManager.AppSettings["eTracDefaultCountry"];
        private long eTracDefaultCountry = 0;
        private string eTracVerifyLocation = ConfigurationManager.AppSettings["eTracVerifyLocation"];
        private string ProfileImagePath = ConfigurationManager.AppSettings["ProfilePicPath"];
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        public GlobalAdminController(ICommonMethod _ICommonMethod, IGlobalAdmin _IGlobalAdmin, IManageManager _IManageManager, IUser _IUser, IWorkRequestAssignment _IWorkRequestAssignment, IEmployeeManager _IEmployeeManager, ILogin _ILogin, IQRCSetup _IQRCSetup)
        {
            this._ICommonMethod = _ICommonMethod;
            this._IGlobalAdmin = _IGlobalAdmin;
            this._IManageManager = _IManageManager;
            this._IUser = _IUser;
            this._ILogin = _ILogin;
            this._IWorkRequestAssignment = _IWorkRequestAssignment;
            this._IEmployeeManager = _IEmployeeManager;
            this._IQRCSetup = _IQRCSetup;
            if (!long.TryParse(eTracDefaultCountryString, out eTracDefaultCountry)) { eTracDefaultCountry = 0; }
        }
        public GlobalAdminController()
        {
        }

        public ActionResult Index()
        {
            //Session["eTrac_UserLocations"] = _IGlobalAdmin.GetSuperAdminUserLocation();
            //ViewBag.UserLocations = (List<MasterLocationModel>)Session["eTrac_UserLocations"];

            //eTracLoginModel ObjLoginModel = null;

            //long LocationID = 0;
            //if (Session["eTrac"] != null)
            //{
            //    //eTracLoginModel obj_eTracLoginModel = new eTracLoginModel();
            //    ObjLoginModel = (eTracLoginModel)Session["eTrac"];
            //    LocationID = Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]);
            //}
            //ObjectParameter obj_ObjectParameter = new ObjectParameter("TotalRecords", typeof(int));
            // ObjectParameter obj_ObjectParameter2 = new ObjectParameter("TotalRecords", typeof(int));
            // GlobalAdminManager obj_GlobalAdminManager = new GlobalAdminManager();
            //ViewBag.AllWorkOrder = obj_GlobalAdminManager.GetAllWorkRequestAssignment(0, 0, "GetAllWorkRequestAssignment", 1, 100000000, "WorkRequestAssignmentID", "asc", "", LocationID, ObjLoginModel.UserId, DateTime.Now, DateTime.Now, "", obj_ObjectParameter).Count();
            //ViewBag.AllAssignedWorkOrder = obj_GlobalAdminManager.GetAllWorkRequestAssignment(0, 0, "GetAllAssignedWorkRequest", 1, 100000000, "WorkRequestAssignmentID", "asc", "", LocationID, ObjLoginModel.UserId, DateTime.Now, DateTime.Now, "", obj_ObjectParameter).Count();
            //UserManager obj_UserManager = new UserManager();
            // ViewBag.NoAssignedUsers = obj_UserManager.GetNotAssignedUsers(ObjLoginModel.UserId, 1, "Name", "asc", 100000000, "", "", obj_ObjectParameter2).Count();
            //ViewBag.AdministratorCount = _IGlobalAdmin.GetTotalManagerCount("Administrator", LocationID, ObjLoginModel.UserId);
            //ViewBag.ManagerCount = _IGlobalAdmin.GetTotalManagerCount("Manager", LocationID, ObjLoginModel.UserId);
            //// ViewBag.EmployeeCount = _IGlobalAdmin.GetTotalManagerCount("Employee", LocationID, ObjLoginModel.UserId);
            // ViewBag.ClientCount = _IGlobalAdmin.GetTotalManagerCount("Client", LocationID, ObjLoginModel.UserId);
            //Tuple<decimal, decimal> objT = _IGlobalAdmin.EcashDataForDashBoard(ObjLoginModel.LocationID);

            //ViewBag.EcashData = objT.Item1;
            // ViewBag.EcashDataWeek = objT.Item2;

            return View();
        }

        //[CustomActionFilter]
        //public ActionResult Login()
        //{
        //    //For Development
        //    //Session["ProjectID"] = "5";
        //    //For Server                                  
        //    return View();

        //}
        //public ActionResult LogOut()
        //{
        //    Session.Abandon();
        //    return View("Login");
        //}

        #region Location
        [HttpGet]
        public ActionResult Location()
        {
            try
            {
                //CommonMethodManager _CommonMethodManager = new CommonMethodManager();
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                ViewBag.UpdateMode = false;
                return View();
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }

        /// <summary>SaveLocation
        /// CreatedBY:   Gayatri Pal
        /// CreatedOn:   Aug-25-2014
        /// CreatedFor:  To save location information
        /// ModifiedBY:  Nagendra Upwanshi
        /// ModifiedOn:  Aug-28-2014
        /// ModifiedFor: modified for QRC Saveing and return QRCId
        /// </summary>
        /// <param name="_LocationMasterModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Location(LocationMasterModel _LocationMasterModel)
        {

            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    }
                }
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                if (_LocationMasterModel.LocationId == 0)
                {
                    _LocationMasterModel.CreatedBy = ObjLoginModel.UserId;
                    _LocationMasterModel.CreatedDate = DateTime.UtcNow;
                    _LocationMasterModel.IsDeleted = false;
                    ViewBag.UpdateMode = false;
                }
                else
                {
                    _LocationMasterModel.ModifiedBy = ObjLoginModel.UserId;
                    _LocationMasterModel.ModifiedDate = DateTime.UtcNow;
                    _LocationMasterModel.IsDeleted = false;
                    ViewBag.UpdateMode = true;
                }

                //Result result = _IGlobalAdmin.SaveLocation(_LocationMasterModel, out locationId);
                //if (result == Result.Completed)

                QRCModel QRCDetail = new QRCModel();
                Result result = _IGlobalAdmin.SaveLocation(_LocationMasterModel, out QRCDetail);

                if (result == Result.Completed)
                {
                    ViewBag.Message = CommonMessage.SaveSuccessMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    ModelState.Clear();
                    return View();
                }
                else if (result == Result.DuplicateRecord)
                {
                    ViewBag.Message = CommonMessage.DuplicateRecordOnlyMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Info;// store the message for successful in tempdata to display in view.
                }
                else if (result == Result.UpdatedSuccessfully)
                {
                    ViewBag.Message = CommonMessage.UpdateSuccessMessage(); // store the message for successful in tempdata to display in view.
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                    // store the failure message in tempdata to display in view.
                }

                //ViewBag.EncryptQRC = Cryptography.GetEncryptedData(QRCDetail.QRCId.ToString(), true);
                ViewBag.QRCSize = _ICommonMethod.GetGlobalCodeData("QRCSIZE");
                ViewBag.EncryptQRC = QRCDetail.EncryptQRC;
                ViewBag.QRCName = QRCDetail.QRCName;
                ViewBag.SpecialNote = QRCDetail.SpecialNotes;
                //ViewBag.QRCSize = QRCDetail.DefaultSize;
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }

            // ViewBag.UpdateMode = false;
            return View("Location", _LocationMasterModel);
        }



        //created by gayatri 
        //on 26-Aug-2014
        [HttpGet]
        public ActionResult ListLocation()
        {
            try
            {
                //Added by Bhushan Dod on 27/06/2016 for scenario as if viewalllocation is enabled and user navigate any of module list but when click on any create from module so not able to create.
                //ViewAllLocation need to apply on dashboard only so when ever click on list we have to set Session["eTrac_SelectedDasboardLocationID"] is objeTracLoginModel.LocationID.
                eTracLoginModel ObjLoginModel = null;
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                        if (Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]) == 0)
                        {
                            (Session["eTrac_SelectedDasboardLocationID"]) = ObjLoginModel.LocationID;
                        }
                    }
                }

                ViewBag.AdministratorList = null;
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult EditLocation(string loc)
        {
            try
            {
                if (!string.IsNullOrEmpty(loc))
                {
                    loc = Cryptography.GetDecryptedData(loc, true);
                    LocationMasterModel obj = _IGlobalAdmin.GetLocationById(Convert.ToInt64(loc));
                    ViewBag.Country = _ICommonMethod.GetAllcountries();
                    ViewBag.UpdateMode = true;

                    return View("Location", obj);

                }
                else
                {
                    ViewBag.AlertMessageClass = new AlertMessageClass().Danger;
                    ViewBag.Message = Result.DoesNotExist;
                }
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
            return View("Location");
        }

        [HttpPost]
        public ActionResult DeleteLocation(string id)
        {
            DARModel objDAR;
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);

                    if (!string.IsNullOrEmpty(id))
                    {
                        id = Cryptography.GetDecryptedData(id, true);
                    }

                    objDAR = new DARModel();
                    objDAR.LocationId = ObjLoginModel.LocationID;
                    objDAR.UserId = ObjLoginModel.UserId;
                    objDAR.CreatedBy = ObjLoginModel.UserId;
                    objDAR.CreatedOn = DateTime.UtcNow;

                    Result result = _IGlobalAdmin.DeleteLocation(Convert.ToInt32(id), objDAR);
                    if (result == Result.Delete)
                    {
                        ViewBag.Message = CommonMessage.DeleteSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    }
                    else if (result == Result.Failed)
                    {
                        ViewBag.Message = "Can't Delete Location, Project Assign to the location";
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                    }
                }

                var res = this.SetUserLocationList();
            }
            catch (Exception ex)
            { throw ex; }
            return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
            //return Json(ViewBag.Message);
        }

        [HttpPost]
        public JsonResult isLocationNameExists(string locationName)
        {

            try
            {
                byte result = _IGlobalAdmin.isLocationNameExists(locationName);

                return Json(new { status = result }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult isEmailExists(string Email, string AlternateEmail)
        {
            try
            {
                byte result = _IGlobalAdmin.isEmailExists(Email, AlternateEmail);

                return Json(new { status = result }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult isLocationCodeExists(string LocationCode)
        {
            try
            {
                byte result = _IGlobalAdmin.isLocationCodeExists(LocationCode);

                return Json(new { status = result }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Project
        [HttpGet]
        public ActionResult Project()
        {
            try
            {
                ViewBag.Location = _ICommonMethod.GetAllLocation();
                ViewBag.Category = _ICommonMethod.GetGlobalCodeData(Convert.ToString(GlobalCodename.ProjectCategory));
                ViewBag.Services = _ICommonMethod.GetAllServices();
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }

            ViewBag.UpdateMode = false;
            return View();
        }

        //[HttpPost]
        //public ActionResult Project(ProjectMasterModel ObjProjectMasterModel, string ServicesID, string LogoName)
        //{
        //    QRCModel QRCDetail = new QRCModel();
        //    try
        //    {
        //        ViewBag.Location = _ICommonMethod.GetAllLocation();
        //        ViewBag.Category = _ICommonMethod.GetGlobalCodeData(Convert.ToString(GolbalCodeName.PROJECTCATEGORY));
        //        ViewBag.Services = _ICommonMethod.GetAllServices();
        //        if (!String.IsNullOrEmpty(ServicesID)) { ViewBag.ServicesID = ServicesID; }


        //        if (ObjProjectMasterModel.ProjectID == 0)
        //        {
        //            ObjProjectMasterModel.CreatedBy = 1;
        //            ObjProjectMasterModel.CreatedDate = DateTime.Now;
        //            ObjProjectMasterModel.IsDeleted = false;
        //            ViewBag.UpdateMode = false;
        //        }
        //        else
        //        {
        //            ObjProjectMasterModel.ModifiedBy = 1;
        //            ObjProjectMasterModel.ModifiedDate = DateTime.Now;
        //            ObjProjectMasterModel.IsDeleted = false;
        //            ViewBag.UpdateMode = true;
        //        }
        //        if (Session["ImageName"] != null)
        //        {
        //            ObjProjectMasterModel.ProjectLogoName = Convert.ToString(Session["ImageName"]);
        //            ObjProjectMasterModel.ProjectLogoURl = path + ObjProjectMasterModel.ProjectLogoName;
        //        }
        //        Result result = _IGlobalAdmin.SaveProject(ObjProjectMasterModel, ServicesID, out QRCDetail);
        //        Session["ImageName"] = null;


        //        if (QRCDetail != null)
        //        {
        //            //ViewBag.EncryptQRC = Cryptography.GetEncryptedData(QRCDetail.QRCId.ToString(), true);
        //            ViewBag.QRCSize = _ICommonMethod.GetGlobalCodeData("QRCSIZE");
        //            ViewBag.EncryptQRC = QRCDetail.EncryptQRC;
        //            ViewBag.QRCName = QRCDetail.QRCName;
        //            ViewBag.SpecialNote = QRCDetail.SpecialNotes;
        //            //ViewBag.QRCSize = QRCDetail.DefaultSize;
        //        }
        //        if (!string.IsNullOrEmpty(LogoName)) { ObjProjectMasterModel.ProjectLogoName = LogoName; }

        //        if (result == Result.Completed)
        //        {
        //            ViewBag.Message = CommonMessage.SaveSuccessMessage();
        //            ModelState.Clear();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
        //            return View();
        //        }
        //        else if (result == Result.DuplicateRecord)
        //        {
        //            ViewBag.Message = CommonMessage.DuplicateRecordOnlyMessage();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.info;// store the message for successful in tempdata to display in view.
        //        }
        //        else if (result == Result.UpdatedSuccessfully)
        //        {
        //            ViewBag.Message = CommonMessage.UpdateSuccessMessage();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;// store the message for successful in tempdata to display in view.
        //            ModelState.Clear();
        //            return View();
        //        }
        //        else
        //        {
        //            ViewBag.Message = CommonMessage.FailureMessage();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
        //        }
        //        //ViewBag.EncryptQRC = Cryptography.GetEncryptedData(QRCID.ToString(), true);


        //    }
        //    catch (Exception ex)
        //    { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }

        //    return View("Project", ObjProjectMasterModel);
        //}

        [HttpGet]
        public ActionResult ListProject()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult EditProject(string proj, string Location, string Description, string LocationID, string ProjectCategory, string ProjectServicesID, string ProjectLogoName, long? QRCID)
        {
            try
            {
                if (!string.IsNullOrEmpty(proj))
                {
                    proj = Cryptography.GetDecryptedData(proj, true);
                    ViewBag.UpdateMode = true;

                    ViewBag.Location = _ICommonMethod.GetAllLocation();
                    ViewBag.Category = _ICommonMethod.GetGlobalCodeData(Convert.ToString(GlobalCodename.ProjectCategory));
                    ViewBag.Services = _ICommonMethod.GetAllServices();
                    ProjectMasterModel _ProjectMasterModel = new ProjectMasterModel();
                    _ProjectMasterModel.ProjectID = Convert.ToInt32(proj);
                    _ProjectMasterModel.Location = Location;
                    _ProjectMasterModel.Description = Description;
                    _ProjectMasterModel.LocationID = Convert.ToInt32(LocationID);
                    _ProjectMasterModel.ProjectCategory = Convert.ToInt32(ProjectCategory);
                    _ProjectMasterModel.ProjectLogoName = ProjectLogoName;
                    ViewBag.ServicesID = ProjectServicesID;
                    return View("Project", _ProjectMasterModel);
                }
                else
                {
                    ViewBag.AlertMessageClass = new AlertMessageClass().Danger;
                    ViewBag.Message = Result.DoesNotExist;
                    return View("Project");
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        //[HttpPost]
        //public ActionResult DeleteProject(string proj)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(proj))
        //        {
        //            proj = Cryptography.GetDecryptedData(proj, true);
        //        }
        //        Result result = _IGlobalAdmin.DeleteProject(Convert.ToInt32(proj));
        //        if (result == Result.Delete)
        //        {
        //            ViewBag.Message = CommonMessage.DeleteSuccessMessage();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
        //        }
        //        else if (result == Result.Failed)
        //        {
        //            ViewBag.Message = "Can't Delete Project, Manager is Assign to the Project";
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
        //        }
        //        else
        //        {
        //            ViewBag.Message = CommonMessage.FailureMessage();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
        //    //return Json(ViewBag.Message);
        //}

        #endregion

        #region InviteManager
        [HttpGet]
        public ActionResult InviteManager()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }
        [HttpPost]
        public ActionResult InviteManager(UserModel objUserModel)
        {

            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    }
                }
                //if (ModelState.IsValid)
                //{
                objUserModel.CreatedBy = ObjLoginModel.UserId;
                objUserModel.CreatedDate = DateTime.UtcNow;
                objUserModel.IsDeleted = false;

                var _usertype = _ICommonMethod.GetGlobalCodeForName("USERTYPE", "Manager");
                if (_usertype > 0) { objUserModel.UserType = _usertype; }

                //Result result = _IGlobalAdmin.SendInvitation(objUserModel, Convert.ToString(UserType.Manager));
                Result result = _IGlobalAdmin.SendInvitation(objUserModel);
                if (result == Result.EmailSendSuccessfully)
                {
                    ViewBag.Message = CommonMessage.EmailSuccess();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    ModelState.Clear();
                }
                else if (result == Result.DuplicateRecordEmail)
                {
                    ViewBag.Message = CommonMessage.DuplicateRecordEmailIdMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Info;
                }
                //}
                //else
                //    return View();

            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }

            return View();
        }

        #endregion

        #region Manager This section is use when user click link in email

        //This Method is use to open the Manager Profile 
        //information when click on verify link in email
        public ActionResult Manager(string usr)
        {
            try
            { ViewBag.Country = _ICommonMethod.GetAllcountries(); return View("myManager", _ICommonMethod.LoadInvitedUser(usr)); }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }

        //This Method is use to save the Manager Profile 
        //information when click on verify link in email
        [HttpPost]
        public ActionResult Manager(QRCModel ObjUserModel)
        {
            ObjectParameter paramTotalrecord = new ObjectParameter("TotalRecords", typeof(int));

            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    }
                }

                if (ObjUserModel.UserModel.UserId > 0)
                {
                    ObjUserModel.UserModel.Password = (!string.IsNullOrEmpty(ObjUserModel.UserModel.Password)) ? Cryptography.GetEncryptedData(ObjUserModel.UserModel.Password, true) : ObjUserModel.UserModel.Password;
                    //if (ModelState.IsValid)
                    //{
                    if (ObjUserModel.UserModel.UserId == 0)
                    {
                        ObjUserModel.UserModel.CreatedBy = ObjLoginModel.UserId;
                        ObjUserModel.UserModel.CreatedDate = DateTime.UtcNow;
                        ObjUserModel.UserModel.IsDeleted = false;
                    }
                    else
                    {
                        ObjUserModel.UserModel.ModifiedBy = ObjLoginModel.UserId;
                        ObjUserModel.UserModel.ModifiedDate = DateTime.UtcNow;
                        ObjUserModel.UserModel.IsDeleted = false;
                    }
                    //if (Session["ImageName"] != null)
                    //{ ObjUserModel.UserModel.ProfileImage = Convert.ToString(Session["ImageName"]); }

                    long QRCID = 0;
                    Result result = _IGlobalAdmin.SaveManager(ObjUserModel.UserModel, out QRCID, true);
                    if (result == Result.Completed)
                    {
                        ViewBag.Message = CommonMessage.SaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
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
            ViewBag.Country = _ICommonMethod.GetAllcountries();
            ViewBag.UpdateMode = false;
            long Totalrecord = 0;
            ObjUserModel.UserModel = _IGlobalAdmin.GetManagerById(ObjUserModel.UserModel.UserId, "GetUserByID", null, null, null, null, null, out Totalrecord);
            return View("myManager", ObjUserModel);
        }

        #endregion Manager

        #region Invite Client
        //[HttpGet]
        //public ActionResult InviteClient()
        //{
        //    try
        //    {
        //        ViewBag.Project = _ICommonMethod.GetNotAssgnProject(Convert.ToString(UserType.Client), 0);
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //[HttpPost]
        //public ActionResult InviteClient(UserModel objUserModel)
        //{
        //    try
        //    {
        //        objUserModel.CreatedBy = 1;
        //        objUserModel.CreatedDate = DateTime.Now;
        //        objUserModel.IsDeleted = false;

        //        var _usertype = _ICommonMethod.GetGlobalCodeForName("USERTYPE", "Client");
        //        if (_usertype > 0) { objUserModel.UserType = _usertype; }

        //        //Result result = _IGlobalAdmin.SendInvitation(objUserModel, Convert.ToString(UserType.Client));
        //        Result result = _IGlobalAdmin.SendInvitation(objUserModel);
        //        if (result == Result.EmailSendSuccessfully)
        //        {
        //            ViewBag.Message = CommonMessage.EmailSuccess();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
        //            ModelState.Clear();
        //        }
        //        else if (result == Result.DuplicateRecordEmail)
        //        {
        //            ViewBag.Message = CommonMessage.DuplicateRecordEmailIdMessage();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.info;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return View();
        //}
        #endregion

        #region Assign Project
        public ActionResult ListVerifiedManager()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public ActionResult _AssignProject(string Name, string UserEmail, string id, string ProjectID, string HiringDate)
        //{
        //    try
        //    {
        //        UserModel _UserModel = new UserModel();
        //        _UserModel.FirstName = Name;
        //        _UserModel.UserEmail = UserEmail;
        //        _UserModel.UserId = Convert.ToInt32(id);
        //        _UserModel.ProjectID = Convert.ToInt32(ProjectID);
        //        if (HiringDate != string.Empty)
        //            _UserModel.HiringDate = Convert.ToDateTime(HiringDate);
        //        ViewBag.Project = _ICommonMethod.GetNotAssgnProject(Convert.ToString(UserType.Manager), Convert.ToInt32(ProjectID));
        //        return PartialView("_AssignProject", _UserModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //[HttpPost]
        //public JsonResult _AssignProjectManager(UserModel _UserModel)
        //{
        //    try
        //    {
        //        _UserModel.ModifiedBy = 1;
        //        _UserModel.ModifiedDate = DateTime.Now;
        //        Result result = _IGlobalAdmin.AssignProject(_UserModel);
        //        if (result == Result.Completed)
        //        {
        //            ViewBag.Message = CommonMessage.SaveSuccessMessage();
        //            ModelState.Clear();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
        //        }
        //        else if (result == Result.Failed)
        //        {
        //            ViewBag.Message = CommonMessage.FailureMessage(); // store the message for successful in tempdata to display in view.
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    //ViewBag.Project = _ICommonMethod.GetAllProject();
        //    //return PartialView("_AssignProject", _UserModel);
        //    return Json(new { Message = ViewBag.Message, AlertMessageClass = ObjAlertMessageClass.Success }, JsonRequestBehavior.AllowGet);
        //    //return Json(ViewBag.Message);
        //}

        #endregion

        [HttpPost]
        public virtual ActionResult UploadImage()
        {
            return this.UploadImage("ImageName");

            /*
            CommonHelper obj_CommonHelper = new CommonHelper();
            int c = Request.Files.Count;
            HttpPostedFileBase myFile = Request.Files["image"];
            Session["ImageName"] = DateTime.Now.Ticks + myFile.FileName;
            string msg = string.Empty;
            //if (Session["ImageName"] != null)
            //{
            //    obj_CommonAction.DeleteImage(Session["ImageName3"].ToString(), Server.MapPath(path));
            //    Session["ImageName"] = null;
            //    Session["ImageName"] = DateTime.Now.Ticks + myFile.FileName;
            //}
            //else
            //{
            //    Session["ImageName3"] = DateTime.Now.Ticks + myFile.FileName;
            //}
            path = Server.MapPath(path);
            obj_CommonHelper.uploadImage(myFile, path, Session["ImageName"].ToString(), out msg);
            //return Json("sucess" + path);
            return Json(msg);
            */
        }

        [HttpPost]
        public virtual ActionResult UploadImage(string SessionName)
        {
            CommonHelper obj_CommonHelper = new CommonHelper();
            //Commented by bhushan DOd for code review
            //int c = Request.Files.Count;
            HttpPostedFileBase myFile = Request.Files["image"];
            Session[SessionName] = DateTime.Now.Ticks + myFile.FileName;
            string msg = string.Empty;
            path = Server.MapPath(path);
            obj_CommonHelper.UploadImage(myFile, path, Session[SessionName].ToString());
            return Json(msg);
        }

        public ActionResult Administrator(string usr)
        {
            try
            { ViewBag.Country = _ICommonMethod.GetAllcountries(); return View("Administrator", _ICommonMethod.LoadInvitedUser(usr)); }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }

        /// <summary>LoadInvitedUser
        /// CreatedBy   :   Nagendra Upwanshi
        /// CreatedFor  :   Load User
        /// CreatedOn   :   Oct-31-2014
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        [NonAction]
        public QRCModel LoadInvitedUsernotinuse(string usr)
        {
            try
            {
                //string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                //string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();            

                ViewBag.Country = _ICommonMethod.GetAllcountries();
                long userid = 0;
                if (!string.IsNullOrEmpty(usr))
                {
                    usr = Cryptography.GetDecryptedData(usr, true);
                    long.TryParse(usr, out userid);
                }
                QRCModel _UserModel = new QRCModel();

                if (userid > 0)
                {
                    long Totalrecord = 0;
                    _UserModel.UserModel = _IGlobalAdmin.GetManagerById(userid, "GetUserByID", null, null, null, null, null, out Totalrecord);
                    _UserModel.UserModel.Password = "";
                }
                return _UserModel;
            }
            catch (Exception ex) { throw ex; }


        }

        #region ClientRegistrationLoadSelectedManagerDetails
        public ActionResult ClientRegistration()
        {
            try
            {
                //ViewBag.Caller = "clientregistration";
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                return PartialView("_UserRegistration");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("Index");
        }

        #endregion ClientRegistration

        #region UserList

        /// <summary>UserList
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-03-2014</CreatedOn>
        /// <CreatedFor>List all user with completed details</CreatedFor>
        /// </summary>
        /// <returns></returns>
        public ActionResult UserList()
        {
            try
            {
                ViewBag.ListUserType = "listuser";
                return View();
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion UserList

        #region ListAdministrator
        /// <summary>ListAdministrator
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-14-2014</CreatedOn>
        /// <CreatedFor>List all Administrator with completed details</CreatedFor>
        /// </summary>
        /// <returns></returns>
        public ActionResult ListAdministrator()
        {
            try
            { ViewBag.ListUserType = "listadministrator"; return View("UserList"); }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion ListAdministrator

        #region ListITAdministrator
        /// <summary>ListITAdministrator
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-14-2014</CreatedOn>
        /// <CreatedFor>List all IT Administrator with completed details</CreatedFor>
        /// </summary>
        /// <returns></returns>
        public ActionResult ListITAdministrator()
        {
            try
            { ViewBag.ListUserType = "listitadministrator"; return View("UserList"); }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion ListITAdministrator

        #region ListManager
        /// <summary>ListManager
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-14-2014</CreatedOn>
        /// <CreatedFor>List all Manager with completed details</CreatedFor>
        /// </summary>
        /// <returns></returns>
        public ActionResult ListManager()
        {
            try
            { ViewBag.ListUserType = "listmanager"; return View("UserList"); }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion ListManager

        #region ListEmployee
        /// <summary>ListEmployee
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-14-2014</CreatedOn>
        /// <CreatedFor>List all Employee with completed details</CreatedFor>
        /// </summary>
        /// <returns></returns>
        public ActionResult ListEmployee()
        {
            try
            { ViewBag.ListUserType = "listemployee"; return View("UserList"); }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion ListEmployee

        #region ListClient
        /// <summary>ListClient
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-14-2014</CreatedOn>
        /// <CreatedFor>List all Client with completed details</CreatedFor>
        /// </summary>
        /// <returns></returns>
        public ActionResult ListClient()
        {
            try
            {
                //Added by Bhushan Dod on 27/06/2016 for scenario as if viewalllocation is enabled and user navigate any of module list but when click on any create from module so not able to create.
                //ViewAllLocation need to apply on dashboard only so when ever click on list we have to set Session["eTrac_SelectedDasboardLocationID"] is objeTracLoginModel.LocationID.
                eTracLoginModel ObjLoginModel = null;
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                        if (Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]) == 0)
                        {
                            (Session["eTrac_SelectedDasboardLocationID"]) = ObjLoginModel.LocationID;
                        }
                    }
                }
                ViewBag.ListUserType = "listclient"; return View("UserList");
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion ListClient

        [HttpGet]
        public JsonResult GetListITAdministrator(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (locationId == null)
                {
                    locationId = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
            }

            JQGridResults result = new JQGridResults();
            List<JQGridRow> rowss = new List<JQGridRow>();
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);

            long TotalRows = 0;

            try
            {
                //ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
                long paramTotalRecords = 0;
                List<UserModelList> ITAdministratorList = _IGlobalAdmin.GetAllITAdministratorList(UserId, Convert.ToInt64(locationId), page, rows, sidx, sord, txtSearch, UserType, out paramTotalRecords);
                foreach (var ITAdmin in ITAdministratorList)
                {
                    JQGridRow row = new JQGridRow();
                    row.id = Cryptography.GetEncryptedData(ITAdmin.UserId.ToString(), true);
                    row.cell = new string[7];
                    row.cell[0] = ITAdmin.Name;
                    row.cell[1] = ITAdmin.UserEmail;
                    row.cell[2] = ITAdmin.Name;
                    row.cell[3] = ITAdmin.UserType;
                    row.cell[4] = ITAdmin.DOB.HasValue ? ITAdmin.DOB.Value.ToShortDateString() : "";
                    row.cell[5] = (ITAdmin.ProfileImage == "" || ITAdmin.ProfileImage == null) ? "" : HostingPrefix + ProfileImagePath.Replace("~/", "") + ITAdmin.ProfileImage;
                    row.cell[6] = ITAdmin.EmployeeProfile;
                    rowss.Add(row);
                }
                // result.rows = rowss.ToArray();
                //result.page = (page.HasValue) ? page.Value : 1;
                //result.total = Convert.ToInt32(rows / (rows.HasValue ? rows.Value : 20));
                //result.records = Convert.ToInt32(TotalRows);
                result.rows = rowss.ToArray();
                result.page = Convert.ToInt32(page);
                result.total = (int)Math.Ceiling((decimal)(Convert.ToInt32(paramTotalRecords) / rows));
                result.records = Convert.ToInt32(paramTotalRecords);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            //{ViewBag.Message = ex.Message;ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;}
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region LocationSetup

        /// <summary>EditLocationSetup
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-03-2014</CreatedOn>
        /// <CreatedFor>Create Location through Setup</CreatedFor>
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        public ActionResult EditLocationSetup(string loc)
        {
            try
            {


                if (TempData != null && TempData["exceptionRaisedWhileUpdating"] != null && TempData["exceptionRaisedWhileUpdating"].ToString().Length > 0)
                {
                    ViewBag.Message = TempData["exceptionRaisedWhileUpdating"].ToString();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                }

                eTracLoginModel objLoginSession = new eTracLoginModel();
                objLoginSession = (eTracLoginModel)Session["eTrac"];


                long locid = 0;
                if (!string.IsNullOrEmpty(loc))
                {
                    // loc = Cryptography.GetDecryptedData(loc, true);
                    locid = Convert.ToInt64(Cryptography.GetDecryptedData(loc, true));
                    //long.TryParse(loc, out locid);
                }
                else { locid = objLoginSession.LocationID; }
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                //ViewBag.StateList = _ICommonMethod.GetStateByCountryID(eTracDefaultCountry);
                ViewBag.LocationTypeList = _ICommonMethod.GetGlobalCodeDataList("LOCATIONTYPE");
                ViewBag.Services = _ICommonMethod.GetAllServices();

                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                long Totalrecords = 0;

                ViewBag.ManagerList = _IGlobalAdmin.GetAllITAdministratorList(objLoginSession.UserId, locid, 1, 100000, "Name", "asc", "", (UserType.Manager).ToString(), out Totalrecords);
                var Administrators = _IGlobalAdmin.GetAllITAdministratorList(objLoginSession.UserId, locid, 1, 100000, "Name", "asc", "", (UserType.Administrator).ToString(), out Totalrecords).ToList();
                if (Administrators.Count() > 0)
                {
                    ViewBag.AdministratorList = Administrators;
                }
                else
                {
                    ViewBag.AdministratorList = _IGlobalAdmin.GetApplicationGlobalAdmin().Select(x => new UserModelList()
                    {
                        Name = x.FirstName + " " + x.LastName,
                        UserId = x.UserID,
                        UserEmail = x.UserEmail,
                    });

                }

                //ViewBag.AdministratorList = _IGlobalAdmin.GetAllITAdministratorList(objLoginSession.UserId, objLoginSession.LocationID, 1, 1000, "Name", "asc", "", (UserType.Administrator).ToString(), out Totalrecords);

                //ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();

                //ViewBag.ManagerList = _UserManager
                ViewBag.UpdateMode = true;
                LocationMasterModel _LocationMasterModel = _IGlobalAdmin.GetLocationDetailsByLocationId(locid);
                ViewBag.ServicesID = _LocationMasterModel.Services;
                return View("LocationSetup", _LocationMasterModel);
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }

        }

        /// <summary>LocationSetup
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-03-2014</CreatedOn>
        /// <CreatedFor>Create Location through Setup</CreatedFor>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LocationSetup()
        {
            try
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                objLoginSession = (eTracLoginModel)Session["eTrac"];
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                //ViewBag.StateList = _ICommonMethod.GetStateByCountryID(eTracDefaultCountry);
                ViewBag.LocationTypeList = _ICommonMethod.GetGlobalCodeDataList("LOCATIONTYPE");
                ViewBag.Services = _ICommonMethod.GetAllServices();

                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                long Totalrecords = 0;
                ViewBag.ManagerList = _IGlobalAdmin.GetAllITAdministratorList(objLoginSession.UserId, 0, 1, 100000, "Name", "asc", "", (UserType.Manager).ToString(), out Totalrecords);
                var Administrators = _IGlobalAdmin.GetAllITAdministratorList(objLoginSession.UserId, 0, 1, 100000, "Name", "asc", "", (UserType.Administrator).ToString(), out Totalrecords).ToList();
                if (Administrators.Count() > 0)
                {
                    ViewBag.AdministratorList = Administrators;
                }
                else
                {
                    ViewBag.AdministratorList = _IGlobalAdmin.GetApplicationGlobalAdmin().Select(x => new UserModelList()
                    {
                        Name = x.FirstName + " " + x.LastName,
                        UserId = x.UserID,
                        UserEmail = x.UserEmail,
                    });

                }
                ViewBag.UpdateMode = false;
                // LocationMasterModel _LocationMasterModel = _IGlobalAdmin.GetLocationDetailsByLocationID(locid);
                return View("LocationSetup");
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }

        }

        public ActionResult GetManagerDetails(long UserID)
        {
            try
            {
                long TotalRecords = 0;
                ViewBag.myModelprefixName = "ManagerModel."; ViewBag.ActionSection = "manager";
                ViewBag.Country = _ICommonMethod.GetAllcountries();

                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                long Totalrecords = 0;
                ViewBag.ManagerList = _IGlobalAdmin.GetAllITAdministratorList(0, 0, 1, 1000, "UserEmail", "asc", "", (UserType.Manager).ToString(), out Totalrecords);
                ViewBag.AdministratorList = _IGlobalAdmin.GetAllITAdministratorList(0, 0, 1, 1000, "UserEmail", "asc", "", (UserType.Administrator).ToString(), out Totalrecords);
                LocationMasterModel _LocationMasterModel = new LocationMasterModel();
                _LocationMasterModel.ManagerModel = _IGlobalAdmin.GetManagerById(UserID, "GetUserByID", null, null, null, null, null, out TotalRecords);
                return PartialView("_myRegistration", _LocationMasterModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult LoadSelectedManagerDetails(long myManagerId)
        {
            LocationMasterModel _LocationMasterModel;
            try
            {
                long TotalRecords = 0;
                _LocationMasterModel = new LocationMasterModel();
                _LocationMasterModel.ManagerModel = _IGlobalAdmin.GetManagerById(myManagerId, "GetUserByID", null, null, null, null, null, out TotalRecords);
                if (!string.IsNullOrEmpty(_LocationMasterModel.ManagerModel.myProfileImage))
                {
                    _LocationMasterModel.ManagerModel.myProfileImage = HostingPrefix + ProfileImagePath.Replace("~", "") + _LocationMasterModel.ManagerModel.myProfileImage;
                }
                //else{
                //  _LocationMasterModel.ManagerModel.myProfileImage=
                //}
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(_LocationMasterModel, JsonRequestBehavior.AllowGet);
        }

        #endregion LocationSetup

        #region LocationSetup POST
        /// <summary>LocationSetup
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Nov-03-2015</CreatedOn>
        /// <CreatedFor>Create Location through Setup</CreatedFor>
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LocationSetup(LocationMasterModel ObjLocationMasterModel)
        {
            DARModel objDAR;
            bool isExceptionRaised = false;
            long verificationManagerId, verificationClientId, OutLocationId = 0;
            eTracLoginModel objLoginSession = new eTracLoginModel();
            objLoginSession = (eTracLoginModel)Session["eTrac"];
            try
            {
                //if (ModelState.IsValid)
                //{
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                // LocationCollection collection = serializer.Deserialize<LocationCollection>(ObjLocationMasterModel.EmployeeList);

                objDAR = new DARModel();
                string Country = _ICommonMethod.GetAllcountries().Where(c => c.CountryID == ObjLocationMasterModel.CountryId).Select(r => r.CountryName).SingleOrDefault();
                string StateName = _ICommonMethod.GetStateByCountryId(Convert.ToInt32(ObjLocationMasterModel.CountryId)).Where(r => r.StateId == ObjLocationMasterModel.StateId).Select(d => d.StateName).SingleOrDefault();
                string Address = ObjLocationMasterModel.Address1 + "," + Country + "," + StateName + "," + ObjLocationMasterModel.City + "," + ObjLocationMasterModel.ZipCode;
                Common.CommonController myCommonController = new Common.CommonController(_ICommonMethod, _IManageManager, _IGlobalAdmin, _IUser, _IWorkRequestAssignment, _IClientManager);

                //ObjLocationMasterModel.EmployeeListModel = !string.IsNullOrEmpty(ObjLocationMasterModel.EmployeeList) ? serializer.Deserialize<List<UserModel>>(ObjLocationMasterModel.EmployeeList) : null;

                ObjLocationMasterModel.EmployeeListModel = !string.IsNullOrEmpty(ObjLocationMasterModel.EmployeeList) ? serializer.Deserialize<List<UserModel>>(ObjLocationMasterModel.EmployeeList.Replace("/", "-")) : null;

                if (!string.IsNullOrEmpty(ObjLocationMasterModel.LocationName) && ObjLocationMasterModel != null && ObjLocationMasterModel.ManagerModel != null && ObjLocationMasterModel.ClientModel != null)
                {

                    if (ObjLocationMasterModel.LocationId == 0)
                    {

                        ObjLocationMasterModel.IsVerifiedByClient = true;
                        ObjLocationMasterModel.IsVerifiedByManager = true;
                        //#region password
                        //ObjLocationMasterModel.ManagerModel.Password = _ICommonMethod.CreateRandomPassword();
                        //ObjLocationMasterModel.ClientModel.Password = _ICommonMethod.CreateRandomPassword();
                        //#endregion password
                        if (ObjLocationMasterModel.ManagerModel.ProfileImage != null)
                        {
                            string ImageName = ObjLocationMasterModel.ManagerModel.UserId + "_" + DateTime.Now.Ticks.ToString() + ".png";
                            //string ImageName = ObjLocationMasterModel.ManagerModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + ObjLocationMasterModel.ManagerModel.ProfileImage.FileName.ToString();
                            CommonHelper obj_CommonHelper = new CommonHelper();
                            obj_CommonHelper.UploadImage(ObjLocationMasterModel.ManagerModel.ProfileImage, Server.MapPath(ConfigurationManager.AppSettings["ProfilePicPath"]), ImageName);
                            ObjLocationMasterModel.ManagerModel.ProfileImageFile = ImageName;
                        }
                        if (ObjLocationMasterModel.ClientModel.ProfileImage != null)
                        {
                            string ClImageName = ObjLocationMasterModel.ClientModel.UserId + "_" + DateTime.Now.Ticks.ToString() + ".png";
                            CommonHelper obj_CommonHelper = new CommonHelper();
                            obj_CommonHelper.UploadImage(ObjLocationMasterModel.ClientModel.ProfileImage, Server.MapPath(ConfigurationManager.AppSettings["ProfilePicPath"]), ClImageName);
                            ObjLocationMasterModel.ClientModel.ProfileImageFile = ClImageName;
                        }

                        ObjLocationMasterModel.CreatedBy = objLoginSession.UserId;
                        ObjLocationMasterModel.CreatedDate = DateTime.UtcNow;
                        ObjLocationMasterModel.IsDeleted = false;
                        ViewBag.UpdateMode = false;
                    }
                    else
                    {
                        //#endregion password
                        if (ObjLocationMasterModel.ManagerModel.ProfileImage != null)
                        {
                            string ImageName = ObjLocationMasterModel.ManagerModel.UserId + "_" + DateTime.Now.Ticks.ToString() + ".png";
                            CommonHelper obj_CommonHelper = new CommonHelper();
                            obj_CommonHelper.UploadImage(ObjLocationMasterModel.ManagerModel.ProfileImage, Server.MapPath(ConfigurationManager.AppSettings["ProfilePicPath"]), ImageName);
                            ObjLocationMasterModel.ManagerModel.ProfileImageFile = ImageName;
                        }
                        if (ObjLocationMasterModel.ClientModel.ProfileImage != null)
                        {
                            string ClImageName = ObjLocationMasterModel.ClientModel.UserId + "_" + DateTime.Now.Ticks.ToString() + ".png";
                            CommonHelper obj_CommonHelper = new CommonHelper();
                            obj_CommonHelper.UploadImage(ObjLocationMasterModel.ClientModel.ProfileImage, Server.MapPath(ConfigurationManager.AppSettings["ProfilePicPath"]), ClImageName);
                            ObjLocationMasterModel.ClientModel.ProfileImageFile = ClImageName;
                        }

                        ObjLocationMasterModel.ModifiedBy = objLoginSession.UserId;
                        ObjLocationMasterModel.ModifiedDate = DateTime.UtcNow;
                        ObjLocationMasterModel.IsDeleted = false;
                        ViewBag.UpdateMode = true;
                    }
                    bool OutSendMail = false;



                    ///Added by vijay sahu on 25 june 2015
                    #region Below code is use for sending mail to those employee who is newly added while updaing the location.

                    string[] cell = null;

                    if (ObjLocationMasterModel.EmployeeListModel != null)
                    {
                        foreach (var emp in ObjLocationMasterModel.EmployeeListModel.Where(r => r.UserId == 0))
                        {

                            if (emp.UserId == 0)
                            {
                                cell = new string[]
                        {
                            emp.UserEmail,
                            emp.Password,
                            emp.Address.Address1
                        };
                            }
                        }
                    }
                    #endregion

                    Result result = _IGlobalAdmin.ProcessLocationSetup(ObjLocationMasterModel, out verificationManagerId, out verificationClientId, out OutLocationId, out OutSendMail);
                    int eTracVerifyLocationFlag = 0;

                    objDAR.LocationId = objLoginSession.LocationID;
                    objDAR.UserId = objLoginSession.UserId;
                    objDAR.CreatedBy = objLoginSession.UserId;
                    objDAR.CreatedOn = DateTime.UtcNow;

                    //eTracVerifyLocation flag == null                                  //if (!string.IsNullOrEmpty(eTracVerifyLocation) && Convert.ToInt16(eTracVerifyLocation)>0)
                    if (string.IsNullOrEmpty(eTracVerifyLocation) || !int.TryParse(eTracVerifyLocation, out eTracVerifyLocationFlag) || eTracVerifyLocationFlag == 0)
                    {   //eTracVerifyLocation old Dec 29 2014                        
                        #region eTracVerifyLocation old Dec 29 2014

                        if (result == Result.Completed)
                        {

                            ViewBag.Message = "Location successfully created.";

                            objDAR.ActivityDetails = DarMessage.SaveSuccessLocationDar(ObjLocationMasterModel.LocationName);
                            objDAR.TaskType = (long)TaskTypeCategory.CreateLocation;

                            //path = Server.MapPath(path);
                            try
                            {
                                #region EmailHelper
                                // Common.CommonController myCommonController = new Common.CommonController(_ICommonMethod);

                                #region Email to Manager User

                                if (ObjLocationMasterModel.ManagerModel.IsExistingManager == false)
                                {
                                    ObjLocationMasterModel.ManagerModel.Password = Cryptography.GetEncryptedData(ObjLocationMasterModel.ManagerModel.Password, true);
                                    ObjLocationMasterModel.ManagerModel.UserType = Convert.ToInt32(UserType.Manager);
                                    myCommonController.SendEmailToUser(ObjLocationMasterModel.ManagerModel, ObjLocationMasterModel.LocationName, Address, ObjLocationMasterModel.Address2, objLoginSession.UserId);
                                }

                                #endregion Email to Manager User

                                #region Email to Client User

                                if (ObjLocationMasterModel.ClientModel.ExistClientID == 0)
                                {
                                    ObjLocationMasterModel.ClientModel.Password = Cryptography.GetEncryptedData(ObjLocationMasterModel.ClientModel.Password, true);
                                    ObjLocationMasterModel.ClientModel.UserType = Convert.ToInt32(UserType.Client);
                                    myCommonController.SendEmailToUser(ObjLocationMasterModel.ClientModel, ObjLocationMasterModel.LocationName, Address, ObjLocationMasterModel.Address2, objLoginSession.UserId);
                                }

                                #endregion Email to Client User


                                #region Send Email To employee
                                foreach (var employee in ObjLocationMasterModel.EmployeeListModel)
                                {
                                    employee.Password = Cryptography.GetEncryptedData(employee.Password, true);
                                    employee.UserType = Convert.ToInt32(UserType.Employee);
                                    employee.Location = ObjLocationMasterModel.LocationId;
                                    myCommonController.SendEmailToUser(employee, ObjLocationMasterModel.LocationName, Address, "", objLoginSession.UserId);
                                }
                                #endregion

                                #endregion EmailHelper


                                string msg = string.Empty;
                                CommonHelper ObjCommonHelper = new CommonHelper();

                                #region Save Manager Profile Image
                                if (ObjLocationMasterModel.ManagerModel.ProfileImage != null)
                                { ObjCommonHelper.UploadImage(ObjLocationMasterModel.ManagerModel.ProfileImage, path, ObjLocationMasterModel.ManagerModel.ProfileImage.FileName); }
                                #endregion Save Manager Profile Image

                                #region Save Client Profile Image
                                if (ObjLocationMasterModel.ManagerModel.ProfileImage != null)
                                { ObjCommonHelper.UploadImage(ObjLocationMasterModel.ClientModel.ProfileImage, path, ObjLocationMasterModel.ClientModel.ProfileImage.FileName); }
                                #endregion Save Client Profile Image
                            }
                            catch (Exception ex)
                            {
                                isExceptionRaised = true;
                                throw new ApplicationException(ViewBag.Message + " error: " + ex.Message);

                            }

                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                            ModelState.Clear();
                            ObjLocationMasterModel = new LocationMasterModel();
                            //return View("ITAdministrator");//return RedirectToAction("Create ", "GlobalAdmin");
                            //ObjUserModel = _ICommonMethod.LoadInvitedUser(string.Empty);
                        }
                        else if (result == Result.DuplicateRecord)
                        {
                            ViewBag.Message = CommonMessage.DuplicateLocationMessage(ObjLocationMasterModel.LocationName.Trim());
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Info; // store the message for successful in tempdata to display in view.
                        }
                        else if (result == Result.UpdatedSuccessfully)
                        {

                            objDAR.ActivityDetails = DarMessage.UpdateSuccessLocationDar(ObjLocationMasterModel.LocationName);
                            objDAR.TaskType = (long)TaskTypeCategory.UpdateLocation;

                            ViewBag.Message = CommonMessage.UpdateSuccessMessage();
                            ViewBag.Message = "Location detail successfully updated.";
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;// store the message for successful in tempdata to display in view.
                            ModelState.Clear();

                            //#region Email to Users
                            //foreach (var employee in ObjLocationMasterModel.EmployeeListModel)
                            //{
                            //    if (employee.UserId == 0)
                            //    {
                            //        myCommonController.SendEmailToUser(employee, ObjLocationMasterModel.LocationName, Address);
                            //    }
                            //}
                            //#endregion Email to Users

                            ObjLocationMasterModel = new LocationMasterModel();
                        }
                        else
                        {
                            ViewBag.Message = CommonMessage.FailureMessage();
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                        }

                        #endregion eTracVerifyLocation old Dec 29 2014
                    }
                    else
                    { //eTracVerifyLocation new Dec 29 2014
                        #region eTracVerifyLocation Dec 29 2014

                        if (result == Result.Completed)
                        {
                            objDAR.ActivityDetails = DarMessage.SaveSuccessLocationDar(ObjLocationMasterModel.LocationName);
                            objDAR.TaskType = (long)TaskTypeCategory.CreateLocation;

                            //ViewBag.Message = CommonMessage.SaveSuccessMessage();
                            ViewBag.Message = "Location has been created Successfully.";
                            path = Server.MapPath(path);
                            try
                            {
                                #region EmailHelper


                                #region Send Email to Admin
                                long TotalRecords = 0;

                                var objAdminDetails = _ICommonMethod.GetAdminByIdCode(ObjLocationMasterModel.ManagerModel.Administrator, "GetUserByID", null, null, null, null, null, out TotalRecords);

                                #endregion send email to admin
                                //Added by Bhushan Dod on 11-August-2016 for objLocation.locationid null due to this not able to send email to client, manager.
                                objAdminDetails.Location = ObjLocationMasterModel.LocationId;
                                myCommonController.SendEmailToUser(objAdminDetails, ObjLocationMasterModel.LocationName, Address, ObjLocationMasterModel.Address2, objLoginSession.UserId);

                                #region comment

                                #region Email to Client User

                                if (ObjLocationMasterModel.ClientModel.ExistClientID == 0)
                                {
                                    myCommonController.SendEmailToUser(ObjLocationMasterModel.ClientModel, ObjLocationMasterModel.LocationName, Address, ObjLocationMasterModel.Address2, objLoginSession.UserId);
                                }

                                #endregion Email to Client User
                                //#region Sed Email To employee
                                //foreach (var employee in ObjLocationMasterModel.EmployeeListModel)
                                //{
                                //    myCommonController.SendEmailToUser(employee, ObjLocationMasterModel.LocationName, Address);
                                //}
                                //#endregion

                                #endregion Email to Manager User
                                //Added by vijay sahu on 29 may 2015

                                if (ObjLocationMasterModel.ManagerModel.ExistManagerID == 0) // this block when you created new manager for new location.
                                {
                                    myCommonController.SendEmailToUser(ObjLocationMasterModel.ManagerModel, ObjLocationMasterModel.LocationName, Address, ObjLocationMasterModel.Address2, objLoginSession.UserId);
                                }
                                else // this block while you have selected manager from dropdown list.
                                {
                                    myCommonController.SendEmailToUser(ObjLocationMasterModel.ManagerModel, ObjLocationMasterModel.LocationName, Address, ObjLocationMasterModel.Address2, objLoginSession.UserId);
                                }

                                ////// //Verification mail commented by vijay sahu under the instructions of my manager (ankita).
                                if (OutSendMail == true)
                                {
                                    #region Email to Employee Users
                                    foreach (var employee in ObjLocationMasterModel.EmployeeListModel)
                                    {
                                        employee.Location = ObjLocationMasterModel.LocationId;
                                        myCommonController.SendEmailToUser(employee, ObjLocationMasterModel.LocationName, Address, ObjLocationMasterModel.Address2, objLoginSession.UserId);

                                    }
                                    #endregion Email to Employee Users
                                }


                                #endregion EmailHelper

                                string msg = string.Empty;
                                CommonHelper ObjCommonHelper = new CommonHelper();

                                #region Save Manager Profile Image
                                if (ObjLocationMasterModel.ManagerModel.ProfileImage != null)
                                { ObjCommonHelper.UploadImage(ObjLocationMasterModel.ManagerModel.ProfileImage, path, ObjLocationMasterModel.ManagerModel.ProfileImage.FileName); }
                                #endregion Save Manager Profile Image

                                #region Save Client Profile Image
                                if (ObjLocationMasterModel.ClientModel.ProfileImage != null)
                                { ObjCommonHelper.UploadImage(ObjLocationMasterModel.ClientModel.ProfileImage, path, ObjLocationMasterModel.ClientModel.ProfileImage.FileName); }
                                #endregion Save Client Profile Image
                            }
                            catch (Exception ex)
                            {
                                isExceptionRaised = true;
                                throw new ApplicationException(ViewBag.Message + " error: " + ex.Message);
                            }

                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                            ModelState.Clear();
                            ObjLocationMasterModel = new LocationMasterModel();
                            //return View("ITAdministrator");//return RedirectToAction("Create ", "GlobalAdmin");
                            //ObjUserModel = _ICommonMethod.LoadInvitedUser(string.Empty);
                        }
                        else if (result == Result.DuplicateRecord)
                        {
                            ViewBag.Message = CommonMessage.DuplicateLocationMessage(ObjLocationMasterModel.LocationName.Trim());
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Info; // store the message for successful in tempdata to display in view.
                        }
                        else if (result == Result.UpdatedSuccessfully)
                        {

                            objDAR.ActivityDetails = DarMessage.UpdateSuccessLocationDar(ObjLocationMasterModel.LocationName);
                            objDAR.TaskType = (long)TaskTypeCategory.UpdateLocation;

                            //ViewBag.Message = CommonMessage.UpdateSuccessMessage();
                            ViewBag.Message = "Location Detail updated successfully";
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;// store the message for successful in tempdata to display in view.
                            ModelState.Clear();

                            #region Email to Users

                            if (ObjLocationMasterModel.EmployeeListModel != null)
                            {
                                try
                                {
                                    for (int a = 0; a < cell.Length; a++)
                                    {
                                        for (int b = 0; b < ObjLocationMasterModel.EmployeeListModel.Count(); b++)
                                        {
                                            if (cell[a].ToString() == ObjLocationMasterModel.EmployeeListModel[b].UserEmail)
                                            {
                                                ObjLocationMasterModel.EmployeeListModel[b].Location = ObjLocationMasterModel.LocationId;
                                                string apass = Cryptography.GetEncryptedData(ObjLocationMasterModel.EmployeeListModel[b].Password, true).ToString();
                                                myCommonController.SendEmailToUser(ObjLocationMasterModel.EmployeeListModel[b], ObjLocationMasterModel.LocationName, Address, "", objLoginSession.UserId);

                                            }
                                        }
                                        //myCommonController.SendEmailToUser(a, ObjLocationMasterModel.LocationName, Address);
                                    }

                                    foreach (var employee in ObjLocationMasterModel.EmployeeListModel)
                                    {
                                        if (employee.UserId == 0)
                                        {
                                            employee.Password = Cryptography.GetEncryptedData(employee.Password, true);
                                            myCommonController.SendEmailToUser(employee, ObjLocationMasterModel.LocationName, Address, "", objLoginSession.UserId);
                                        }
                                    }
                                }
                                catch
                                {

                                }
                            }
                            #endregion Email to Users
                            ObjLocationMasterModel = new LocationMasterModel();
                        }
                        else
                        {
                            ViewBag.Message = CommonMessage.FailureMessage();
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                        }
                        #endregion eTracVerifyLocation Dec 29 2014
                    }
                    #region Save DAR
                    if (objDAR.ActivityDetails != null)
                    {
                        result = _ICommonMethod.SaveDAR(objDAR);
                    }
                    #endregion Save DAR
                }
                else { ViewBag.Message = CommonMessage.InvalidEntry(); }
            }
            catch (ApplicationException appex)
            {
                isExceptionRaised = true;
                ViewBag.Message = appex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            catch (Exception ex)
            {
                isExceptionRaised = true;
                if (ObjLocationMasterModel.ManagerModel.ProfileImage != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(ConfigurationManager.AppSettings["ProfilePicPath"]) + ObjLocationMasterModel.ManagerModel.ProfileImageFile))
                    {
                        System.IO.File.Delete(Server.MapPath(ConfigurationManager.AppSettings["ProfilePicPath"]) + ObjLocationMasterModel.ManagerModel.ProfileImageFile);
                    }


                }
                if (ObjLocationMasterModel.ClientModel.ProfileImage != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(ConfigurationManager.AppSettings["ProfilePicPath"]) + ObjLocationMasterModel.ClientModel.ProfileImageFile))
                    {
                        System.IO.File.Delete(Server.MapPath(ConfigurationManager.AppSettings["ProfilePicPath"]) + ObjLocationMasterModel.ClientModel.ProfileImageFile);
                    }
                }
                if (ObjLocationMasterModel.EmployeeListModel != null)
                {
                    foreach (var res in ObjLocationMasterModel.EmployeeListModel)
                    {
                        if (System.IO.File.Exists(Server.MapPath(ConfigurationManager.AppSettings["ProfilePicPath"]) + res.ProfileImageFile))
                        {
                            System.IO.File.Delete(Server.MapPath(ConfigurationManager.AppSettings["ProfilePicPath"]) + res.ProfileImageFile);
                        }
                    }
                }
                //ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; 
                //return View("Error"); 
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;

                TempData["exceptionRaisedWhileUpdating"] = ViewBag.Message;
            }
            finally
            {
                var res = this.SetUserLocationList();
                objLoginSession = (eTracLoginModel)Session["eTrac"];
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                //ViewBag.StateList = _ICommonMethod.GetStateByCountryID(eTracDefaultCountry);
                ViewBag.LocationTypeList = _ICommonMethod.GetGlobalCodeDataList("LOCATIONTYPE");
                ViewBag.Services = _ICommonMethod.GetAllServices();

                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                long Totalrecords = 0;
                ViewBag.ManagerList = _IGlobalAdmin.GetAllITAdministratorList(objLoginSession.UserId, 0, 1, 100000, "Name", "asc", "", (UserType.Manager).ToString(), out Totalrecords);
                var Administrators = _IGlobalAdmin.GetAllITAdministratorList(objLoginSession.UserId, 0, 1, 100000, "Name", "asc", "", (UserType.Administrator).ToString(), out Totalrecords).ToList();
                if (Administrators.Count() > 0)
                {
                    ViewBag.AdministratorList = Administrators;
                }
                else
                {
                    ViewBag.AdministratorList = _IGlobalAdmin.GetApplicationGlobalAdmin().Select(x => new UserModelList()
                    {
                        Name = x.FirstName + " " + x.LastName,
                        UserId = x.UserID,
                        UserEmail = x.UserEmail,
                    });
                }
                //Added By Bhushan Dod on 08/07/2016 for while location edit
                if (ViewBag.UpdateMode == true)
                {
                    Common_B obj_Common_B = new Common_B();
                    Session["eTrac_SelectedDasboardLocationID"] = objLoginSession.LocationID;
                    Session["eTrac_UserRoles"] = this.Get_UserAssignedRolesDashboard();

                    Session["eTrac_LocationServices"] = obj_Common_B.GetLocationServicesByLocationID(objLoginSession.LocationID, objLoginSession.UserRoleId);
                    Session["eTrac_DashboardWidget"] = null;
                    Session["eTrac_DashboardWidget"] = this.GetUserDashboardWidgetRolesViewAll();
                    if (objLoginSession.UserRoleId == 1 || objLoginSession.UserRoleId == 5)// 1 - GlobalAdmin ,2 IT Admin
                    {
                        Session["eTrac_UserRoles"] = Session["eTrac_LocationServices"];
                    }
                }
                ViewBag.UpdateMode = false;
                // LocationMasterModel _LocationMasterModel = _IGlobalAdmin.GetLocationDetailsByLocationID(locid);

            }
            ViewBag.UpdateMode = false;


            if (isExceptionRaised == true)
            {
                ViewBag.UpdateMode = true;
                var locIdS = "";
                try
                {
                    locIdS = Cryptography.GetEncryptedData(ObjLocationMasterModel.LocationId.ToString(), true);
                }
                catch
                {
                    locIdS = ObjLocationMasterModel.LocationId.ToString();
                }
                return RedirectToAction("EditLocationSetup", "GlobalAdmin", new { loc = Cryptography.GetEncryptedData(ObjLocationMasterModel.LocationId.ToString(), true) });

            }
            else
            {
                return View("LocationSetup", ObjLocationMasterModel);
            }
        }

        #endregion LocationSetup POST

        #region commmneted code send mail to user
        #region EmailHelper SendEmailToUser

        /// <summary>SendEmailToUser
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedFor>Send Email To User</CreatedFor>
        /// <CreatedOn>Nov-24-2014</CreatedOn>
        /// </summary>
        /// <param name="UserForEmail"></param>
        /// <returns></returns>

        [NonAction]
        private bool SendEmailToUser(UserModel UserForEmail)
        {
            try
            {
                EmailHelper objEmailHelper = new EmailHelper();
                objEmailHelper.emailid = UserForEmail.UserEmail;
                objEmailHelper.UserType = UserForEmail.UserType;
                objEmailHelper.FirstName = UserForEmail.FirstName;
                objEmailHelper.LastName = UserForEmail.LastName;
                objEmailHelper.Password = Cryptography.GetDecryptedData(UserForEmail.Password, true);
                objEmailHelper.MailType = "USERREGISTRATION";
                //  string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"]);

                #region comments

                string UserLink = "";                           //Enum.TryParse(userType, out _UserType);                           //long _userType = (long)_UserType;                        
                long _userType = UserForEmail.UserType;

                UserLink = "Manager/Employee";


                HostingPrefix = HostingPrefix + UserLink + "?usr=" + Cryptography.GetEncryptedData(UserForEmail.UserId.ToString(), true);
                #endregion comments
                //HostingPrefix = HostingPrefix + "?usr=" + Cryptography.GetEncryptedData(UserForEmail.UserId.ToString(), true);
                objEmailHelper.RegistrationLink = HostingPrefix;
                #region comments
                //objEmailHelper.RegistrationLink = HostingPrefix + "/?flag=Registration&id=" + System.Web.HttpUtility.UrlPathEncode(Cryptography.GetEncryptedData(HostingPrefix.ToString(), true));
                //objEmailHelper.RegistrationLink = HostingPrefix + "/?flag=Registration&id=" + System.Web.HttpUtility.UrlPathEncode(Cryptography.GetEncryptedData(UserForEmail.UserId.ToString(), true));
                //objEmailHelper.RegistrationCode = (UserForEmail.EmailVerifcationCode;
                #endregion comments
                objEmailHelper.SendEmailWithTemplate();
                return true;
            }
            catch (Exception ex)
            {
                return true;
                throw ex;

            }

        }

        #endregion EmailHelper SendEmailToUser
        #endregion commmneted code send mail to user

        public JsonResult GetListLocationAdministrator(string LocationId, string UserType)
        {
            List<UserModelList> ListData = new List<UserModelList>();
            try
            {

                ListData = _IGlobalAdmin.GetLocationListAdministratorClient(LocationId, UserType);


            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; }
            return Json(ListData);
        }

        public JsonResult UnAssignedAdministrationId(string LocationId, string UserType)
        {
            List<UserModelList> ListData = new List<UserModelList>();
            try
            {
                var res = this.SetUserLocationList();
                return Json(_IGlobalAdmin.UnAssignedAdministratorId(LocationId, UserType));
            }
            catch (Exception ex)
            { return null; }
        }

        /// <summary>
        /// Added by vijay sahu on 10 march 2015
        /// Modified by vijay sahu on 8 may 2015
        /// </summary>
        /// <param name="LocationID"></param>
        /// <param name="AdminUserId"></param>
        /// <param name="IsDelete"></param>
        /// <returns></returns>
        public JsonResult MapAdminUserForLocation(string LocationID, string LocationAddress, string LocationName, string AdminUserId, bool? IsDelete, string RolesIds)
        {

            Tuple<bool, int> tup = null;
            try
            {
                long _LocationID, _AdminUserId; long LoginUser = 0;
                if (!string.IsNullOrEmpty(LocationID) && !string.IsNullOrEmpty(AdminUserId))
                {
                    LocationID = Cryptography.GetDecryptedData(LocationID, true);
                    var LoginObj = (eTracLoginModel)Session["eTrac"];
                    LoginUser = LoginObj.UserId;
                    //AdminUserId = Cryptography.GetDecryptedData(AdminUserId, true);
                    if (long.TryParse(LocationID, out _LocationID) && long.TryParse(AdminUserId, out _AdminUserId))
                    {

                        PermissionDetailsModel objPermissionDetailsModel = new PermissionDetailsModel();
                        objPermissionDetailsModel.UserId = Convert.ToInt32(AdminUserId);
                        objPermissionDetailsModel.UserIds = RolesIds;
                        objPermissionDetailsModel.LocationId = Convert.ToInt32(LocationID);
                        var Result = _ICommonMethod.UpdateUserPermissions(objPermissionDetailsModel);

                        try
                        {
                            tup = _IGlobalAdmin.MapAdminForLocation(_LocationID, _AdminUserId, LoginUser, IsDelete);
                        }
                        catch
                        {
                        }

                        if (IsDelete == null || IsDelete == false)
                        {

                            #region Send Email to Admin
                            long TotalRecords = 0;
                            //var LocationDetails = _ICommonMethod.GetLocationDetailsById(_LocationID);
                            var ObjUserModel = _ICommonMethod.GetAdminByIdCode(_AdminUserId, "GetUserByID", null, null, null, null, null, out TotalRecords);
                            var address = "";
                            try
                            {
                                address = LocationAddress.Split(',')[0] + "<br/>" + LocationAddress.Split(',')[1] + ", " + LocationAddress.Split(',')[2];
                            }
                            catch
                            {

                                address = LocationAddress;
                            }

                            EmailHelper objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = ObjUserModel.UserEmail;
                            objEmailHelper.UserName = ObjUserModel.AlternateEmail;
                            objEmailHelper.UserType = ObjUserModel.UserType;
                            objEmailHelper.FirstName = ObjUserModel.FirstName;
                            objEmailHelper.LastName = ObjUserModel.LastName;
                            //objEmailHelper.Password = Cryptography.GetDecryptedData(ObjUserModel.Password, true);
                            objEmailHelper.LocationName = LocationName;
                            objEmailHelper.LocAddress = address; // here locAddress means user Address
                            objEmailHelper.MailType = "UserAddedToLocation";


                            objEmailHelper.SendEmailWithTemplate();

                            #endregion send email to admin
                        }
                    }
                }
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; }
            return Json(new { status = tup.Item1, record = tup.Item2 });
        }

        /// <summary>
        /// Added by vijay sahu on 10 march 2015
        /// Modified by vijay sahu on 8 may 2015
        /// </summary>
        /// <param name="LocationID"></param>
        /// <param name="AdminUserId"></param>
        /// <param name="IsDelete"></param>
        /// <returns></returns>
        public JsonResult MapEmployeeUserForLocation(string LocationID, string LocationAddress, string LocationName, string EmployeeUserId, bool? IsDelete, string RolesIds)
        {
            bool status = false;
            try
            {
                long _LocationID, _AdminUserId; long LoginUser = 0;
                if (!string.IsNullOrEmpty(LocationID) && !string.IsNullOrEmpty(EmployeeUserId))
                {
                    LocationID = Cryptography.GetDecryptedData(LocationID, true);
                    var LoginObj = (eTracLoginModel)Session["eTrac"];
                    LoginUser = LoginObj.UserId;
                    //AdminUserId = Cryptography.GetDecryptedData(AdminUserId, true);
                    //if (long.TryParse(LocationID, out _LocationID) && long.TryParse(EmployeeUserId, out _AdminUserId))
                    if (long.TryParse(LocationID, out _LocationID) && long.TryParse(EmployeeUserId, out _AdminUserId))
                    {

                        PermissionDetailsModel objPermissionDetailsModel = new PermissionDetailsModel();
                        objPermissionDetailsModel.UserId = Convert.ToInt32(EmployeeUserId);
                        objPermissionDetailsModel.UserIds = RolesIds;
                        objPermissionDetailsModel.LocationId = Convert.ToInt32(LocationID);
                        var Result = _ICommonMethod.UpdateUserPermissions(objPermissionDetailsModel);

                        try
                        {
                            status = _IGlobalAdmin.MapEmployeeForLocation(_LocationID, _AdminUserId, LoginUser, LocationName, IsDelete);
                        }
                        catch
                        {
                        }

                        if (IsDelete == null || IsDelete == false)
                        {

                            #region Send Email to Manager


                            long TotalRecords = 0;
                            //var LocationDetails = _ICommonMethod.GetLocationDetailsById(_LocationID);
                            var ObjUserModel = _ICommonMethod.GetAdminByIdCode(_AdminUserId, "GetUserByID", null, null, null, null, null, out TotalRecords);


                            var address = "";
                            try
                            {


                                address = LocationAddress.Split(',')[0] + "<br/>" + LocationAddress.Split(',')[1] + ", " + LocationAddress.Split(',')[2];
                            }
                            catch
                            {

                                address = LocationAddress;
                            }
                            EmailHelper objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = ObjUserModel.UserEmail;
                            objEmailHelper.UserName = ObjUserModel.AlternateEmail;
                            objEmailHelper.UserType = ObjUserModel.UserType;
                            objEmailHelper.FirstName = ObjUserModel.FirstName;
                            objEmailHelper.LastName = ObjUserModel.LastName;
                            //objEmailHelper.Password = Cryptography.GetDecryptedData(ObjUserModel.Password, true);
                            objEmailHelper.LocationName = LocationName;
                            objEmailHelper.LocAddress = address; // here locAddress means user Address
                            objEmailHelper.MailType = "UserAddedToLocation";



                            objEmailHelper.SendEmailWithTemplate();

                            #endregion send email to Manager

                        }

                    }
                }
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; }
            return Json(status);
        }

        /// <summary>
        /// Added by vijay sahu on 10 march 2015
        /// Modified by vijay sahu on 8 may 2015
        /// </summary>
        /// <param name="LocationID"></param>
        /// <param name="AdminUserId"></param>
        /// <param name="IsDelete"></param>
        /// <returns></returns>
        public JsonResult MapManagerUserForLocation(string LocationID, string LocationAddress, string LocationName, string ManagerUserId, bool? IsDelete, string RolesIds)
        {
            Tuple<bool, int> tup = null;
            try
            {
                long _LocationID, _AdminUserId; long LoginUser = 0;
                if (!string.IsNullOrEmpty(LocationID) && !string.IsNullOrEmpty(ManagerUserId))
                {
                    LocationID = Cryptography.GetDecryptedData(LocationID, true);
                    var LoginObj = (eTracLoginModel)Session["eTrac"];
                    LoginUser = LoginObj.UserId;

                    if (long.TryParse(LocationID, out _LocationID) && long.TryParse(ManagerUserId, out _AdminUserId))
                    {
                        PermissionDetailsModel objPermissionDetailsModel = new PermissionDetailsModel();
                        objPermissionDetailsModel.UserId = Convert.ToInt32(ManagerUserId);
                        objPermissionDetailsModel.UserIds = RolesIds;
                        objPermissionDetailsModel.LocationId = Convert.ToInt32(LocationID);
                        _ICommonMethod.UpdateUserPermissions(objPermissionDetailsModel);


                        try
                        {
                            tup = _IGlobalAdmin.MapManagerForLocation(_LocationID, _AdminUserId, LoginUser, IsDelete);
                        }
                        catch
                        {
                        }

                        if (IsDelete == null || IsDelete == false)
                        {
                            #region Send Email to Manager


                            long TotalRecords = 0;
                            //var LocationDetails = _ICommonMethod.GetLocationDetailsById(_LocationID);
                            var ObjUserModel = _ICommonMethod.GetManagerByIdCode(_AdminUserId, "GetUserByID", null, null, null, null, null, out TotalRecords);

                            var address = "";
                            try
                            {
                                address = LocationAddress.Split(',')[0] + "<br/>" + LocationAddress.Split(',')[1] + ", " + LocationAddress.Split(',')[2];
                            }
                            catch
                            {

                                address = LocationAddress;
                            }

                            EmailHelper objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = ObjUserModel.UserEmail;
                            objEmailHelper.UserName = ObjUserModel.AlternateEmail;
                            objEmailHelper.UserType = ObjUserModel.UserType;
                            objEmailHelper.FirstName = ObjUserModel.FirstName;
                            objEmailHelper.LastName = ObjUserModel.LastName;
                            //objEmailHelper.Password = Cryptography.GetDecryptedData(ObjUserModel.Password, true);
                            objEmailHelper.LocationName = LocationName;
                            objEmailHelper.LocAddress = address; // here LocAddress means user Address
                            objEmailHelper.MailType = "UserAddedToLocation";




                            objEmailHelper.SendEmailWithTemplate();

                            #endregion send email to Manager
                        }
                    }
                }
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; }
            return Json(new { status = tup.Item1, record = tup.Item2 });
        }

        #region WorkRequest

        [HttpGet]
        public ActionResult WorkRequestAssignment()
        {
            try
            {
                eTracLoginModel objeTracLoginModel = (eTracLoginModel)(Session["eTrac"]);
                // eTracLoginModel ObjLogin = (eTracLoginModel)Session["eTrac"];
                UserType _UserType = (WorkOrderEMS.Helper.UserType)objeTracLoginModel.UserRoleId;
                if (_UserType == UserType.Administrator)
                    ViewBag.Location = _ICommonMethod.GetLocationByAdminId(objeTracLoginModel.UserId);
                else if (_UserType == UserType.Manager)
                    ViewBag.Location = _ICommonMethod.GetLocationByManagerId(objeTracLoginModel.UserId);
                else
                    ViewBag.Location = _ICommonMethod.GetAllLocation();
                ViewBag.AssignToUser = _IGlobalAdmin.GetLocationEmployee(objeTracLoginModel.LocationID);
                ViewBag.Asset = _ICommonMethod.GetAssetList(objeTracLoginModel.LocationID);
                ViewBag.UpdateMode = false;
                ViewBag.PriorityLevel = _ICommonMethod.GetGlobalCodeData("WORKPRIORITY");
                ViewBag.WorkRequestType = _ICommonMethod.GetGlobalCodeData("WORKREQUESTTYPE");
                ViewBag.WorkRequestProjectTypeID = _ICommonMethod.GetGlobalCodeData("WORKREQUESTPROJECTTYPE");
                ViewBag.FacilityRequest = _ICommonMethod.GetGlobalCodeData("FACILITYREQUESTTYPE");
                ViewBag.StateId = _ICommonMethod.GetStateByCountryId(1);//Added By bhushan HardCoded value bcoz only one country id

                return View(new WorkRequestAssignmentModel());
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult WorkRequestAssignment(WorkRequestAssignmentModel objWorkRequestAssignmentModel)
        {
            WorkRequestAssignmentModel _objWorkRequestAssignmentModel = new WorkRequestAssignmentModel();
            CommonHelper ObjCommonHelper = new CommonHelper();
            eTracLoginModel objeTracLoginModel = (eTracLoginModel)(Session["eTrac"]);
            bool isUpdate = false;

            LocationMasterModel objLocCode = new LocationMasterModel();//objLocCode = _IGlobalAdmin.GetLocationById(Convert.ToInt64(loc));

            //string msg = string.Empty;
            DARModel objDAR;
            Result result;
            try
            {
                // This condition added 
                var empList = _IGlobalAdmin.GetLocationEmployee(objeTracLoginModel.LocationID);
                if (empList.Count > 0)
                {
                    objDAR = new DARModel();
                    objDAR.LocationId = objeTracLoginModel.LocationID;
                    objDAR.UserId = objeTracLoginModel.UserId;
                    objDAR.CreatedBy = objeTracLoginModel.UserId;
                    objDAR.CreatedOn = DateTime.UtcNow;

                    if (objWorkRequestAssignmentModel.WorkRequestAssignmentID == 0)
                    {
                        objLocCode = _IGlobalAdmin.GetLocationById(Convert.ToInt64(objeTracLoginModel.LocationID));
                        objWorkRequestAssignmentModel.CreatedBy = objeTracLoginModel.UserId;
                        objWorkRequestAssignmentModel.CreatedDate = DateTime.UtcNow;
                        objWorkRequestAssignmentModel.RequestBy = objeTracLoginModel.UserId;
                        objWorkRequestAssignmentModel.WorkRequestStatus = 14;
                        objWorkRequestAssignmentModel.WorkRequestImage = objWorkRequestAssignmentModel.WorkRequestImg != null ? objWorkRequestAssignmentModel.WorkRequestImg.FileName : string.Empty;
                        objWorkRequestAssignmentModel.IsDeleted = false;
                        objWorkRequestAssignmentModel.LocationID = objeTracLoginModel.LocationID;
                        objWorkRequestAssignmentModel.WorkOrderCode = objLocCode.Address2.Substring(0, 3).ToUpper() + objWorkRequestAssignmentModel.WorkOrderCode.ToUpper();

                        if (objWorkRequestAssignmentModel.WorkRequestProjectType == 129)
                        {
                            objWorkRequestAssignmentModel.PriorityLevel = 12;
                        }
                        else if (objWorkRequestAssignmentModel.WorkRequestProjectType == 256)
                        {
                            //  objWorkRequestAssignmentModel.CurrentLocation = objeTracLoginModel.LocationID; commented by Bhushan Dod on 06/25/2015 
                            objWorkRequestAssignmentModel.PriorityLevel = 11;
                        }
                        else if (objWorkRequestAssignmentModel.WorkRequestProjectType == 279)
                        {
                            objWorkRequestAssignmentModel.PriorityLevel = 12;
                        }

                        objDAR.ActivityDetails = DarMessage.CreateWorkRequest(objWorkRequestAssignmentModel.WorkOrderCode, objeTracLoginModel.Location);
                        objDAR.TaskType = (long)TaskTypeCategory.WorkOrderRequest;
                    }
                    else
                    {
                        isUpdate = true;
                        objWorkRequestAssignmentModel.ModifiedBy = objeTracLoginModel.UserId;
                        objWorkRequestAssignmentModel.ModifiedDate = DateTime.UtcNow;
                        objWorkRequestAssignmentModel.WorkRequestStatus = 14;
                        objWorkRequestAssignmentModel.WorkRequestImage = objWorkRequestAssignmentModel.WorkRequestImg != null ? objWorkRequestAssignmentModel.WorkRequestImg.FileName : string.Empty;
                        objWorkRequestAssignmentModel.LocationID = objeTracLoginModel.LocationID;

                        objDAR.ActivityDetails = DarMessage.UpdateWorkRequest(objWorkRequestAssignmentModel.WorkOrderCode, objeTracLoginModel.Location);
                        objDAR.TaskType = (long)TaskTypeCategory.WorkOrderUpdate;

                    }
                    if (objWorkRequestAssignmentModel.WorkOrderImage != null)
                    {
                        string ImageName = objeTracLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + objWorkRequestAssignmentModel.WorkOrderImage.FileName.ToString();
                        CommonHelper obj_CommonHelper = new CommonHelper();
                        obj_CommonHelper.UploadImage(objWorkRequestAssignmentModel.WorkOrderImage, Server.MapPath(ConfigurationManager.AppSettings["WorkRequestImage"]), ImageName);
                        objWorkRequestAssignmentModel.AssignedWorkOrderImage = ImageName;
                        objWorkRequestAssignmentModel.WorkRequestImage = ImageName;
                    }

                    _objWorkRequestAssignmentModel = _IGlobalAdmin.SaveWorkRequestAssignment(objWorkRequestAssignmentModel); //saving Data

                    ViewBag.WorkAssignmet = _objWorkRequestAssignmentModel.WorkOrderCode + _objWorkRequestAssignmentModel.WorkOrderCodeID;
                    ViewBag.ProjectType = _objWorkRequestAssignmentModel.WorkRequestProjectType;
                    ViewBag.ProrityLevel = _objWorkRequestAssignmentModel.PriorityLevel;
                    ViewBag.AssignedToEmployee = _objWorkRequestAssignmentModel.AssignToUserId;
                    ViewBag.RequestType = _objWorkRequestAssignmentModel.WorkRequestType;
                    ViewBag.AssetId = _objWorkRequestAssignmentModel.AssetID;
                    ViewBag.ProblemDesc = _objWorkRequestAssignmentModel.ProblemDesc;
                    ViewBag.ProjectDesc = _objWorkRequestAssignmentModel.ProjectDesc;
                    //Added by Bhushan Dod on 23/Sep/2016 for After generting WO then modal come with WO image. If no image uploaded modal don't need to display image.
                    ViewBag.WorkOrderImageModal = _objWorkRequestAssignmentModel.AssignedWorkOrderImage;
                    ViewBag.WorkOrderImage = _objWorkRequestAssignmentModel.AssignedWorkOrderImage == null ? HostingPrefix + "/Content/Images/ProjectLogo/no-profile-pic.jpg" : HostingPrefix + (ConfigurationManager.AppSettings["WorkRequestImage"]).Replace("~", "") + _objWorkRequestAssignmentModel.AssignedWorkOrderImage;

                    //Added By Bhushan Dod for Facility Request
                    ViewBag.CustomerName = _objWorkRequestAssignmentModel.CustomerName;
                    ViewBag.CustomerContact = _objWorkRequestAssignmentModel.CustomerContact;
                    ViewBag.VehicleColor = _objWorkRequestAssignmentModel.VehicleColor;
                    ViewBag.VehicleMake = _objWorkRequestAssignmentModel.VehicleMake;
                    ViewBag.VehicleModel = _objWorkRequestAssignmentModel.VehicleModel;
                    ViewBag.DriverLicenseNo = _objWorkRequestAssignmentModel.DriverLicenseNo;
                    ViewBag.VehicleYear = _objWorkRequestAssignmentModel.VehicleYear;
                    ViewBag.FacilityRequestType = _ICommonMethod.GetGlobalCodeDetailById(_objWorkRequestAssignmentModel.FacilityRequestId);
                    ViewBag.TimeAssigned = _objWorkRequestAssignmentModel.AssignedTimeInterval;
                    //THs field specifically added by bhushan for while Work Order or special assign to employee send push to emp.
                    objWorkRequestAssignmentModel.WorkOrderCodeForPush = _objWorkRequestAssignmentModel.WorkOrderCode + _objWorkRequestAssignmentModel.WorkOrderCodeID;

                    if (objWorkRequestAssignmentModel.WorkRequestImg != null)
                    {
                        WorkRequestImagepath = Server.MapPath(WorkRequestImagepath);
                        ObjCommonHelper.UploadImage(objWorkRequestAssignmentModel.WorkRequestImg, WorkRequestImagepath, objWorkRequestAssignmentModel.WorkRequestImg.FileName);
                    }
                    //Facility notification send to all employees.
                    if (_objWorkRequestAssignmentModel.Result == Result.Completed && objWorkRequestAssignmentModel.WorkRequestProjectType == 256 && objWorkRequestAssignmentModel.FacilityRequestId != 0)
                    {

                        EmailHelper objEmailHelper = new EmailHelper();
                        //This function will use for send notification to All Employee who has assigned to selected location and has the permission of eMaintenance.
                        //Added by vijay sahu
                        List<listForEmployeeDevice> res = _IGlobalAdmin.sendNotificaitonToAllEmployee(objeTracLoginModel.LocationID, objeTracLoginModel.UserId);
                        objEmailHelper.MailType = "EMAINTENANCE";
                        objEmailHelper.WorkRequestAssignmentID = objWorkRequestAssignmentModel.WorkRequestAssignmentID;
                        objEmailHelper.CustomerName = objWorkRequestAssignmentModel.CustomerName;
                        objEmailHelper.VehicleModel1 = objWorkRequestAssignmentModel.VehicleModel;
                        objEmailHelper.VehicleMake1 = objWorkRequestAssignmentModel.VehicleMake.ToString();
                        objEmailHelper.VehicleYear = objWorkRequestAssignmentModel.VehicleYear.ToString();
                        objEmailHelper.FrCurrentLocation = objWorkRequestAssignmentModel.CurrentLocation;
                        objEmailHelper.VehicleColor = objWorkRequestAssignmentModel.VehicleColor;
                        objEmailHelper.DriverLicenseNo = objWorkRequestAssignmentModel.DriverLicenseNo;
                        objEmailHelper.CustomerContact = objWorkRequestAssignmentModel.CustomerContact;
                        objEmailHelper.FacilityRequest = objWorkRequestAssignmentModel.FacilityRequestId;
                        objEmailHelper.AddressFacilityReq = objWorkRequestAssignmentModel.Address;
                        objEmailHelper.LicensePlateNo = objWorkRequestAssignmentModel.LicensePlateNo;
                        objEmailHelper.LocationName = objLocCode.LocationName;
                        objEmailHelper.WorkOrderCodeId = _objWorkRequestAssignmentModel.WorkOrderCode + _objWorkRequestAssignmentModel.WorkOrderCodeID;

                        foreach (var ab in res)
                        {
                            if (ab.DeviceId != null)
                            {
                                PushNotification.GCMAndroid("Facility request submitted at ", ab.DeviceId, objEmailHelper);
                            }

                        }
                    }
                    //Work Request with urgent priority notification send to all employees.
                    if (_objWorkRequestAssignmentModel.Result == Result.Completed && objWorkRequestAssignmentModel.WorkRequestProjectType == Convert.ToInt64(WorkRequestProjectType.WorkRequest) && objWorkRequestAssignmentModel.PriorityLevel == Convert.ToInt64(WorkRequestPriority.Level1Urgent))
                    {
                        EmailHelper objEmailHelper = new EmailHelper();
                        //This function will use for send notification to All Employee who has assigned to selected location and has the permission of eMaintenance.
                        List<listForEmployeeDevice> res = _IGlobalAdmin.sendNotificaitonToAllEmployee(objeTracLoginModel.LocationID, objeTracLoginModel.UserId);

                        objEmailHelper.MailType = "URGENTWORKORDERREQUESTTOEMPLOYEE";
                        objEmailHelper.WorkRequestAssignmentID = objWorkRequestAssignmentModel.WorkRequestAssignmentID;
                        objEmailHelper.WorkOrderCodeId = _objWorkRequestAssignmentModel.WorkOrderCode + _objWorkRequestAssignmentModel.WorkOrderCodeID;
                        objEmailHelper.AssignedTime = objWorkRequestAssignmentModel.AssignedTime.ToString("HH:mm");
                        objEmailHelper.ProjectDesc = objWorkRequestAssignmentModel.ProblemDesc;
                        objEmailHelper.LocationName = objLocCode.LocationName;
                        objEmailHelper.LocationCode = objLocCode.Address2;
                        objEmailHelper.SafetyHazard = objWorkRequestAssignmentModel.SafetyHazard;

                        foreach (var ab in res)
                        {
                            if (ab.DeviceId != null)
                            {
                                PushNotification.GCMAndroid("New urgent work request " + objEmailHelper.WorkOrderCodeId + " issued at " + objEmailHelper.LocationName, ab.DeviceId, objEmailHelper);
                            }
                        }
                    }
                    if (_objWorkRequestAssignmentModel.Result == Result.Completed && objWorkRequestAssignmentModel.PriorityLevel != Convert.ToInt64(WorkRequestPriority.Level1Urgent) && (objWorkRequestAssignmentModel.WorkRequestProjectType == 279
                                                                                        || objWorkRequestAssignmentModel.WorkRequestProjectType == 128
                                                                                        || objWorkRequestAssignmentModel.WorkRequestProjectType == 129))
                    {
                        //This function will use for send notification to All Employee who has assigned to selected location and has the permission of eMaintenance.
                        //Added by vijay sahu
                        if (objWorkRequestAssignmentModel.AssignToUserId != null && objWorkRequestAssignmentModel.AssignToUserId > 0
                                && objeTracLoginModel.LocationID != null && objeTracLoginModel.LocationID > 0)
                        {
                            listForEmployeeDevice res = _IGlobalAdmin.sendNotificationContinuousRequestToEmployee(objeTracLoginModel.LocationID, objWorkRequestAssignmentModel.AssignToUserId, objWorkRequestAssignmentModel);
                        }
                    }
                    if (_objWorkRequestAssignmentModel.Result == Result.UpdatedSuccessfully && objWorkRequestAssignmentModel.PriorityLevel != Convert.ToInt64(WorkRequestPriority.Level1Urgent) && (objWorkRequestAssignmentModel.WorkRequestProjectType == 279
                                                                                    || objWorkRequestAssignmentModel.WorkRequestProjectType == 128
                                                                                    || objWorkRequestAssignmentModel.WorkRequestProjectType == 129))
                    {
                        //This function will use for send notification to All Employee who has assigned to selected location and has the permission of eMaintenance.
                        //Added by vijay sahu
                        if (objWorkRequestAssignmentModel.AssignToUserId != null && objWorkRequestAssignmentModel.AssignToUserId > 0
                                && objeTracLoginModel.LocationID != null && objeTracLoginModel.LocationID > 0)
                        {
                            listForEmployeeDevice res = _IGlobalAdmin.sendNotificationContinuousRequestToEmployee(objeTracLoginModel.LocationID, objWorkRequestAssignmentModel.AssignToUserId, objWorkRequestAssignmentModel);
                        }
                    }
                 ////   Added By Bhushan Dod for Continuous Request
                    //ViewBag.CRStartDate = _objWorkRequestAssignmentModel.StartDate.ToString("MM'/'dd'/'yyyy");
                    //ViewBag.CREndDate = _objWorkRequestAssignmentModel.EndDate.ToString("MM'/'dd'/'yyyy");
               
                    if (_objWorkRequestAssignmentModel.StartDate != null)
                    {
                        //ViewBag.CRStartDate = _objWorkRequestAssignmentModel.StartDate.Value.ToClientTimeZoneinDateTime().ToString("MM'/'dd'/'yyyy");
                        ViewBag.CRStartDate = _objWorkRequestAssignmentModel.StartDate.Value.ToString("MM'/'dd'/'yyyy");
                    }
                    if (_objWorkRequestAssignmentModel.EndDate != null)
                    {
                        //ViewBag.CREndDate = _objWorkRequestAssignmentModel.EndDate.Value.ToClientTimeZoneinDateTime().ToString("MM'/'dd'/'yyyy");
                       ViewBag.CREndDate = _objWorkRequestAssignmentModel.EndDate.Value.ToString("MM'/'dd'/'yyyy");
                    }

                    ViewBag.CRStartTime = Convert.ToDateTime(_objWorkRequestAssignmentModel.CrStartTime).ToString("hh:mm tt");
                    //ViewBag.CRStartTime = _objWorkRequestAssignmentModel.StartTime.Value.ToClientTimeZone(true);

                    ViewBag.Weekdays = _objWorkRequestAssignmentModel.WeekDayLst;
                    ViewBag.CRTimeAssigned = _objWorkRequestAssignmentModel.AssignedTime.ToString("HH:mm");
                    if (_objWorkRequestAssignmentModel.Result == Result.Completed)
                    {
                        ModelState.Clear();
                        //FlashMessage.Confirmation(CommonMessage.WorkOrderSaveSuccessMessage());
                        ViewBag.Message = CommonMessage.WorkOrderSaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    }
                    else if (_objWorkRequestAssignmentModel.Result == Result.DuplicateRecord)
                    {
                        ViewBag.Message = CommonMessage.DuplicateRecordMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Info; // store the message for successful in tempdata to display in view.
                    }
                    else if (_objWorkRequestAssignmentModel.Result == Result.UpdatedSuccessfully)
                    {

                        ModelState.Clear();
                        ViewBag.Message = CommonMessage.WorkOrderUpdateMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;// store the message for successful in tempdata to display in view.
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                    }

                    #region Save DAR
                    result = _ICommonMethod.SaveDAR(objDAR);
                    #endregion Save DAR
                }
                else
                {
                    ModelState.Clear();
                    ViewBag.Message = CommonMessage.NoEmployeeAsscoiated();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                }



                //return Json(_objWorkRequestAssignmentModel);
                if (isUpdate == true) { return View("WorkAssignmentList"); }
                else
                    return View(new WorkRequestAssignmentModel());
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ViewBag.AssignToUser = _IGlobalAdmin.GetLocationEmployee(objeTracLoginModel.LocationID);
                ViewBag.Asset = _ICommonMethod.GetAssetList(objeTracLoginModel.LocationID);
                UserType _UserType = (WorkOrderEMS.Helper.UserType)objeTracLoginModel.UserRoleId;
                if (_UserType == UserType.Administrator)
                    ViewBag.Location = _ICommonMethod.GetLocationByAdminId(objeTracLoginModel.UserId);
                else if (_UserType == UserType.Manager)
                    ViewBag.Location = _ICommonMethod.GetLocationByManagerId(objeTracLoginModel.UserId);
                else
                    ViewBag.Location = _ICommonMethod.GetAllLocation();
                ViewBag.PriorityLevel = _ICommonMethod.GetGlobalCodeData("WORKPRIORITY");
                ViewBag.WorkRequestType = _ICommonMethod.GetGlobalCodeData("WORKREQUESTTYPE");
                ViewBag.WorkRequestProjectTypeID = _ICommonMethod.GetGlobalCodeData("WORKREQUESTPROJECTTYPE");
                ViewBag.FacilityRequest = _ICommonMethod.GetGlobalCodeData("FACILITYREQUESTTYPE");
                ViewBag.StateId = _ICommonMethod.GetStateByCountryId(1);//Added By bhushan HardCoded value bcoz only one country id
                ViewBag.FacilityRequestType = _ICommonMethod.GetGlobalCodeDetailById(_objWorkRequestAssignmentModel.FacilityRequestId);
                if (_objWorkRequestAssignmentModel.WorkRequestType != null)
                {
                    ViewBag.WorkRequestTypeName = _ICommonMethod.GetGlobalCodeDetailById(Convert.ToInt64(_objWorkRequestAssignmentModel.WorkRequestType));
                }
            }
        }

        public ActionResult WorkAssignmentList()
        {
            eTracLoginModel objeTracLoginModel = (eTracLoginModel)(Session["eTrac"]);
            try
            {
                UserType _usertype = (WorkOrderEMS.Helper.UserType)objeTracLoginModel.UserRoleId;
                if (_usertype == UserType.Manager || _usertype == UserType.Administrator)
                    ViewBag.UserID = objeTracLoginModel.UserId;
                //Added by Bhushan Dod on 27/06/2016 for scenario as if viewalllocation is enabled and user navigate any of module list but when click on any create from module so not able to create.
                //ViewAllLocation need to apply on dashboard only so when ever click on list we have to set Session["eTrac_SelectedDasboardLocationID"] is objeTracLoginModel.LocationID.
                if (Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]) == 0)
                {
                    (Session["eTrac_SelectedDasboardLocationID"]) = objeTracLoginModel.LocationID;
                }

                return View("WorkAssignmentList");

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Edit(string Id)
        {
            try
            {
                long workrequestid = 0;
                WorkRequestAssignmentModel objWorkRequestAssignmentModel = new WorkRequestAssignmentModel();

                if (!string.IsNullOrEmpty(Id))
                {
                    Id = Cryptography.GetDecryptedData(Id, true);
                    long.TryParse(Id, out workrequestid);
                }
                ViewBag.UpdateMode = true;
                eTracLoginModel objeTracLoginModel = (eTracLoginModel)(Session["eTrac"]);
                // eTracLoginModel ObjLogin = (eTracLoginModel)Session["eTrac"];
                ViewBag.AssignToUser = _IGlobalAdmin.GetLocationEmployee(objeTracLoginModel.LocationID);
                ViewBag.Asset = _ICommonMethod.GetAssetList(objeTracLoginModel.LocationID);
                UserType _UserType = (WorkOrderEMS.Helper.UserType)objeTracLoginModel.UserRoleId;
                if (_UserType == UserType.Administrator)
                    ViewBag.Location = _ICommonMethod.GetLocationByAdminId(objeTracLoginModel.UserId);
                else if (_UserType == UserType.Manager)
                    ViewBag.Location = _ICommonMethod.GetLocationByManagerId(objeTracLoginModel.UserId);
                else
                    ViewBag.Location = _ICommonMethod.GetAllLocation();
                ViewBag.PriorityLevel = _ICommonMethod.GetGlobalCodeData("WORKPRIORITY");
                ViewBag.WorkRequestType = _ICommonMethod.GetGlobalCodeData("WORKREQUESTTYPE");
                ViewBag.WorkRequestProjectTypeID = _ICommonMethod.GetGlobalCodeData("WORKREQUESTPROJECTTYPE");
                ViewBag.FacilityRequest = _ICommonMethod.GetGlobalCodeData("FACILITYREQUESTTYPE");
                //workorder data
                objWorkRequestAssignmentModel = _IWorkRequestAssignment.GetWorkorderAssignmentByID(workrequestid);

                if (objWorkRequestAssignmentModel.AssetID != null)
                {
                    objWorkRequestAssignmentModel.AssetPicture = _IQRCSetup.GetQRCDetailById(objWorkRequestAssignmentModel.AssetID.Value) == null ? HostingPrefix + "/Content/Images/ProjectLogo/no-profile-pic.jpg" : HostingPrefix + "/Content/Images/ProjectLogo/" + _IQRCSetup.GetQRCDetailById(objWorkRequestAssignmentModel.AssetID.Value);
                }

                objWorkRequestAssignmentModel.AssignedWorkOrderImage = objWorkRequestAssignmentModel.AssignedWorkOrderImage == null ? HostingPrefix + "/Content/Images/ProjectLogo/no-profile-pic.jpg" : HostingPrefix + (ConfigurationManager.AppSettings["WorkRequestImage"]).Replace("~", "") + objWorkRequestAssignmentModel.AssignedWorkOrderImage;
                objWorkRequestAssignmentModel.AssignedTimeInterval = objWorkRequestAssignmentModel.AssignedTime.ToString("HH:mm");
                ViewBag.WorkOrderImage = objWorkRequestAssignmentModel.AssignedWorkOrderImage == null ? HostingPrefix + "/Content/Images/ProjectLogo/no-profile-pic.jpg" : HostingPrefix + (ConfigurationManager.AppSettings["WorkRequestImage"]).Replace("~", "") + objWorkRequestAssignmentModel.AssignedWorkOrderImage;
                ViewBag.StateId = _ICommonMethod.GetStateByCountryId(1);//Added By bhushan HardCoded value bcoz only one country id
                List<string> obt = new List<string>();
                if (objWorkRequestAssignmentModel.WeekDayLst != null)
                {
                    obt = objWorkRequestAssignmentModel.WeekDayLst.Split(',').ToList();
                    objWorkRequestAssignmentModel.AssignedWeekDaysList = obt;
                }


                return View("WorkRequestAssignment", objWorkRequestAssignmentModel);
                // return View("WorkAssignmentList"); commented by bhushan dod on 18/06/2015 bcoz according to vijay testing list after update navigate to list

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult DeleteWorkRequest(string id)
        {
            DARModel objDAR;
            try
            {
                eTracLoginModel ObjLoginModel = null; long loggedInUser = 0, workRequestId = 0;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
                loggedInUser = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;
                if (!string.IsNullOrEmpty(id))
                {
                    id = Cryptography.GetDecryptedData(id, true);

                    workRequestId = Convert.ToInt64(id);

                    objDAR = new DARModel();
                    objDAR.LocationId = ObjLoginModel.LocationID;
                    objDAR.UserId = ObjLoginModel.UserId;
                    objDAR.CreatedBy = ObjLoginModel.UserId;
                    objDAR.CreatedOn = DateTime.UtcNow;

                    Result result = _IWorkRequestAssignment.DeleteWorkRequest(workRequestId, loggedInUser, objDAR, ObjLoginModel.Location);
                    if (result == Result.Delete)
                    {


                        ViewBag.Message = CommonMessage.DeleteSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    }
                    else if (result == Result.Failed)
                    {
                        ViewBag.Message = "Can't Delete Vehicle";
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                    }

                    #region Save DAR
                    result = _ICommonMethod.SaveDAR(objDAR);
                    #endregion Save DAR
                }
            }
            catch (Exception ex)
            { throw ex; }
            return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
        }

#pragma warning disable CS3008 // Identifier is not CLS-compliant
        public ActionResult _AssignWorkAssignmentRequest(string id, string ProblemDesc, long PriorityLevel, string ProjectDesc, string WorkRequestType, long locationId, string AssignedTime, int? AssignToUserId)
#pragma warning restore CS3008 // Identifier is not CLS-compliant
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                WorkRequestAssignmentModel objWorkRequestAssignmentModel = new WorkRequestAssignmentModel();
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
                long workrequestid = 0;
                if (!string.IsNullOrEmpty(id))
                {
                    id = Cryptography.GetDecryptedData(id, true);
                    long.TryParse(id, out workrequestid);
                }
                objWorkRequestAssignmentModel.WorkRequestAssignmentID = workrequestid;
                objWorkRequestAssignmentModel.ProblemDesc = ProblemDesc;
                objWorkRequestAssignmentModel.PriorityLevel = PriorityLevel;
                objWorkRequestAssignmentModel.ProjectDesc = ProjectDesc;
                objWorkRequestAssignmentModel.WorkRequestTy = WorkRequestType;
                objWorkRequestAssignmentModel.AssignedTimeInterval = AssignedTime;
                objWorkRequestAssignmentModel.AssignToUserId = AssignToUserId;
                ViewBag.AssignedToUser = _IGlobalAdmin.GetLocationEmployee(locationId);
                //ViewBag.AssignedToUser = _ICommonMethod.GetEmployeeProject(locationId);
                ViewBag.PriorityLevel = _ICommonMethod.GetGlobalCodeData("WORKPRIORITY");
                //objWorkRequestAssignmentModel.WorkRequestProjectType = WorkRequestProjectType;
                return PartialView("_AssignWorkAssignmentRequest", objWorkRequestAssignmentModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult _AssignWorkAssignmentRequest(WorkRequestAssignmentModel objWorkRequestAssignmentModel)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
            try
            {
                objWorkRequestAssignmentModel.ModifiedBy = ObjLoginModel.UserId;
                objWorkRequestAssignmentModel.ModifiedDate = Convert.ToDateTime(DateTime.UtcNow);
                objWorkRequestAssignmentModel.IsDeleted = false;
                objWorkRequestAssignmentModel.AssignByUserId = ObjLoginModel.UserId;

                Result result = _IGlobalAdmin.AssignedToWorkRequestAssignment(objWorkRequestAssignmentModel);
                if (result == Result.Completed && (objWorkRequestAssignmentModel.WorkRequestProjectType == 128
                                                   || objWorkRequestAssignmentModel.WorkRequestProjectType == 129))
                {
                    //This function will use for send notification of assigned Employee who has assigned to selected location and has the permission of eMaintenance.
                    //Added by Bhushan Dod
                    listForEmployeeDevice res = _IGlobalAdmin.sendNotificationContinuousRequestToEmployee(ObjLoginModel.LocationID, objWorkRequestAssignmentModel.AssignToUserId, objWorkRequestAssignmentModel);
                }
                if (result == Result.Completed)
                {
                    ViewBag.Message = CommonMessage.SaveSuccessMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
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
                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                }
                return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Edit Users

        [HttpGet]
        public ActionResult EditUser(string userId)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    //userId = Cryptography.GetDecryptedData(userId, true);

                    long _userId;
                    long.TryParse(userId, out _userId);

                    var _UserModel = _IUser.GetUserDetailsById(_userId);
                    ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                    //ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();

                    #region ssd


                    //       bool validEmail = System.Text.RegularExpressions.Regex.IsMatch(_UserModel.Email,

                    //"^(?('')(''.+?(?<!\\)''@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))' +

                    //'(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$"

                    //System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    #endregion ssd

                    return View("Administrator", _UserModel);
                }
                else
                {
                    ViewBag.AlertMessageClass = new AlertMessageClass().Danger;
                    ViewBag.Message = Result.DoesNotExist;
                }


            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
            return View("Administrator");
        }


        #endregion Edit Users
        [HttpPost]
        public bool SetUserDashboardLocation()
        {
            try
            {

                Session["eTrac_SelectedDasboardLocationID"] = 0;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpPost]
        public bool SetViewAllUserDashboardLocation(long LocId)
        {
            try
            {
                if (LocId != 0)
                    Session["eTrac_SelectedDasboardLocationID"] = LocId;
                else
                    Session["eTrac_SelectedDasboardLocationID"] = 0;

                Session["eTrac_DashboardWidget"] = null;
                Session["eTrac_DashboardWidget"] = this.GetUserDashboardWidgetRolesViewAll();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpPost]
        public JsonResult GetUserDetailByUserID(string UserID)
        {
            try
            {
                if (!string.IsNullOrEmpty(UserID))
                {
                    return Json(_IUser.GetUserDetailsById(Convert.ToInt64(UserID)));
                }
                else
                {
                    return Json("Please Enter UserId");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// TO GET MANAGER BY LOCATION
        /// </summary>
        /// <CreatedBY>Manoj Jaswal</CreatedBY>
        /// <CreatedDate>2015-03-16</CreatedDate>
        /// <param name="loc"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUsersByLocation(string loc)
        {
            try
            {
                long Totalrecords = 0;
                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    long locid = 0;
                    if (!string.IsNullOrEmpty(loc))
                    {
                        //locid = Convert.ToInt64(Cryptography.GetDecryptedData(loc, true));
                        locid = Convert.ToInt64(loc);
                        return Json(_IGlobalAdmin.GetAllITAdministratorList(objLoginSession.UserId, locid, 1, 100000, "Name", "asc", "", (UserType.Manager).ToString(), out Totalrecords));
                    }
                    else
                    {
                        return Json("Please Enter LocationId");
                    }
                }
                else
                {
                    return Json("Session Expired !");
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult GetEmployeeByLocDetailed(long Loc_ID)
        {

            try
            {
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    var ab = _IEmployeeManager.GetEmployeeByLocDetailed(Loc_ID);

                    foreach (var a in ab)
                    {
                        a.Password = "1234";
                        a.DOB = Convert.ToDateTime(a.DOB).ToString("MM-dd-yy");

                    }
                    return Json(ab);
                }
                else { return Json("Session Expired !"); }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult NotAssignedUsers(int? i)
        {
            if (i == 1)
            {
                ViewBag.Message = CommonMessage.UserSaveSuccessMessage();
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
            }
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                return View("NotAssignedUsers");
            }
            else
            {
                return Json("Session Expired !");
            }
        }

        /// <summary>
        /// TO GET LOCATION DETAIL BY LOCATION ID
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreateDate>3 April 2015</CreateDate>
        /// <param name="LocationID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetLocationDetailByLocationID(string LocationID)
        {
            try
            {
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    long Loc_ID = Convert.ToInt64(Cryptography.GetDecryptedData(LocationID, true));
                    Common_B obj_Common_B = new Common_B();
                    List<LocationDetailModel> obj_LocationDetailModel = _IGlobalAdmin.LocationDetailByLocationID(Loc_ID);
                    for (int i = 0; i < obj_LocationDetailModel.Count; i++)
                    {
                        obj_LocationDetailModel[i].ClientImage = ((obj_LocationDetailModel[i].ClientImage == "" || obj_LocationDetailModel[i].ClientImage == null) ? "" : HostingPrefix + ProfileImagePath.Replace("~", "") + obj_LocationDetailModel[i].ClientImage);
                    }

                    return Json(new { res = obj_LocationDetailModel, res2 = obj_Common_B.GetLocationServicesByLocationID(Loc_ID, 0) });
                }
                else
                {
                    return Json("Session Expired !");
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Created By : Bhushan Dod on 18/01/2015
        /// Description : This function used for signalR persistent connection alert on screen if new WO created or any chnage in table for dashboard progress,pending.
        /// </summary>
        /// <param name="IsGlobalAsax"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult WorkOrderDetailsForPushNotificaiton(string id)
        {
            try
            {
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    eTracLoginModel objLoginSession = new eTracLoginModel();
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    var re = _IGlobalAdmin.WorkOrderDetailsForPushNotificaiton(id, objLoginSession.UserId, objLoginSession.UserRoleId);
                    return Json(new { re }, JsonRequestBehavior.AllowGet);
                }
                else { return Json(null, JsonRequestBehavior.AllowGet); }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// To set User Location List in tHe Session
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>16 April 2015</CreatedDate>
        /// <param name="UserRoleId"></param>
        /// <returns></returns>
        public bool SetUserLocationList()
        {
            try
            {
                if (Session["eTrac"] != null)
                {
                    eTracLoginModel obj_eTracLoginModel = new eTracLoginModel();
                    obj_eTracLoginModel = (eTracLoginModel)Session["eTrac"];
                    switch (obj_eTracLoginModel.UserRoleId)
                    {
                        case ((Int64)(UserType.GlobalAdmin)):
                            Session["eTrac_UserLocations"] = _ILogin.GetUserAssignedLocations(obj_eTracLoginModel.UserRoleId, obj_eTracLoginModel.UserId);
                            break;
                        case ((Int64)(UserType.ITAdministrator)):
                            Session["eTrac_UserLocations"] = _ILogin.GetUserAssignedLocations(obj_eTracLoginModel.UserRoleId, obj_eTracLoginModel.UserId);
                            break;
                        case ((Int64)(UserType.Administrator)):
                            Session["eTrac_UserLocations"] = _ILogin.GetAdminAssignedLocation(obj_eTracLoginModel.UserId);
                            break;
                        case ((Int64)(UserType.Manager)):
                            Session["eTrac_UserLocations"] = _ILogin.GetManagerAssignedLocation(obj_eTracLoginModel.UserId);
                            break;
                        case ((Int64)(UserType.Employee)):
                            Session["eTrac_UserLocations"] = _ILogin.GetEmployeeAssignedLocation(obj_eTracLoginModel.UserId);
                            break;
                        case ((Int64)(UserType.Client)):
                            break;
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

                throw;
            }
        }

        public ActionResult WorkOrderReport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetAssignPermission(string UserIdIs, string locationId)
        {
            long Locationidlon = 0;

            try
            {
                if (Session != null)
                {

                    locationId = Cryptography.GetDecryptedData(locationId, true);
                    Locationidlon = Convert.ToInt64(locationId);
                    var assignedRole = _ICommonMethod.GetAssignPermission(Convert.ToInt32(UserIdIs), Convert.ToInt32(Locationidlon));


                    //var All = _ICommonMethod.GetAllPermissions(Locationidlon);
                    var All = _ICommonMethod.GetPermissionsWithFilterByUserTypeLocationId(Locationidlon, Convert.ToInt32(UserIdIs));




                    return Json(new { status = All != null ? 1 : 0, data = All, notAll = assignedRole }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Created By : Bhushan Dod
        /// Created Date : 07/17/2015
        /// Description : Push notification to the manager if Checked out equipment is not returned within 24 hours.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult QRCDetailsForCheckOutStatus(string id)
        {
            try
            {
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    eTracLoginModel objLoginSession = new eTracLoginModel();
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    bool re = _IGlobalAdmin.EquipmentCheckOutStatus(Convert.ToInt64(id), objLoginSession.UserId, objLoginSession.UserRoleId);
                    return Json(new { re }, JsonRequestBehavior.AllowGet);
                }
                else { return Json(null, JsonRequestBehavior.AllowGet); }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Created By : Bhushan Dod on 18/01/2015
        /// Description : This function used for signalR persistent connection alert on screen if new WO created or any chnage in table for dashboard progress,pending.
        /// </summary>
        /// <param name="IsGlobalAsax"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> WorkOrderDetailsForPushNotificaitonSignalR(string id)
        {
            try
            {
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    NotificationAlertRepository objNotificationAlertRepository = new NotificationAlertRepository();
                    var re = await objNotificationAlertRepository.WorkOrderDetailsForPushNotificationSignalR();
                    return Json(new { re }, JsonRequestBehavior.AllowGet);
                }
                else { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created On: 26/04/2016
        /// Retrieve all data for dashboard
        /// </summary>
        [HttpGet]
        public JsonResult GetDashboardHeadCount(string fromDate, string toDate)
        {
            try
            {
                long LocationID = 0;
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    //Getting client date time. 
                    var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
                    //flag status for if user filter record in time span so to date is till midnight. 
                    bool isUTCDay = true;

                    eTracLoginModel objLoginSession = new eTracLoginModel();
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    DateTime FromDate = (fromDate == null || string.IsNullOrWhiteSpace(fromDate)) ? clientdt.Date : Convert.ToDateTime(fromDate);
                    DateTime ToDate = (toDate == null || string.IsNullOrWhiteSpace(toDate)) ? clientdt.AddDays(1).Date : Convert.ToDateTime(toDate);
                    ////This condition for if fromdate Todate is same but todate time is upto now.
                    //ToDate = (ToDate.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : ToDate;
                    if (Session["eTrac_SelectedDasboardLocationID"] != null)
                    {
                        if (Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]) != 0)
                        {
                            LocationID = objLoginSession.LocationID;
                        }
                    }

                    //if(ToDate != null)
                    //{
                    //    if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
                    //        ToDate = (ToDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : ToDate;
                    //}

                    //if(FromDate != null && ToDate != null)
                    //{
                    //     if ((FromDate.Value.Date == ToDate.Value.Date) && (ToDate.Value.ToLongTimeString() == "12:00:00 AM") || (ToDate.Value.ToLongTimeString() == "12:00:00 AM"))
                    //     {
                    //         ToDate = ToDate.Value.AddDays(1).AddSeconds(-1);
                    //     }
                    //}
                    //Newly added code
                    if (FromDate != null && ToDate != null)
                    {
                        ////if interval date come then need to fetch record till midnight of todate day
                        if ((FromDate.Date != ToDate.Date) && (ToDate.ToLongTimeString() == "12:00:00 AM") && isUTCDay == false)
                        {
                            ToDate = ToDate.AddDays(1).Date;
                        }
                        if ((FromDate.Date == ToDate.Date) && (ToDate.ToLongTimeString() == "12:00:00 AM"))
                        {
                            ToDate = ToDate.AddDays(1).Date;
                        }
                    }
                                                                                        
                    //FromDate = Convert.ToDateTime(FromDate).ToString();
                    //toDate = Convert.ToDateTime(toDate).ToString();
                    FromDate = FromDate.ConvertClientTZtoUTC();
                    ToDate = ToDate.ConvertClientTZtoUTC();
                    //Newly added code end here

                    var dataJson = _IGlobalAdmin.GetDashboardHeadCount(LocationID, objLoginSession.UserId, FromDate, ToDate, objLoginSession.UserRoleId);
                    return Json(new { dataJson }, JsonRequestBehavior.AllowGet);
                }
                else { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            {
                return Json(new { ex.InnerException }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created On: 26/04/2016
        /// Retrieve all data for dashboard
        /// </summary>
        [HttpGet]
        public JsonResult GetWorkOrderDashboardDetail(string fromDate, string toDate)
        {
            try
            {
                long LocationID = 0;
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    eTracLoginModel objLoginSession = new eTracLoginModel();
                    objLoginSession = (eTracLoginModel)Session["eTrac"];

                    DateTime? FromDate = (fromDate == null || string.IsNullOrWhiteSpace(fromDate)) ? DateTime.UtcNow.Date : Convert.ToDateTime(fromDate);
                    DateTime? ToDate = (toDate == null || string.IsNullOrWhiteSpace(fromDate)) ? DateTime.UtcNow : Convert.ToDateTime(toDate);
                    //This condition for if fromdate Todate is same but todate time is upto now.
                    ToDate = (ToDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : ToDate;
                    if (Session["eTrac_SelectedDasboardLocationID"] != null)
                    {
                        if (Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]) != 0)
                        {
                            LocationID = objLoginSession.LocationID;
                        }
                    }

                    if (ToDate != null)
                    {
                        if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
                            ToDate = (ToDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : ToDate;
                    }

                    if (FromDate != null && ToDate != null)
                    {
                        if ((FromDate.Value.Date == ToDate.Value.Date) && (ToDate.Value.ToLongTimeString() == "12:00:00 AM") || (ToDate.Value.ToLongTimeString() == "12:00:00 AM"))
                        {
                            ToDate = ToDate.Value.AddDays(1).AddSeconds(-1);
                        }
                    }
                    FromDate = FromDate.Value.ToClientTimeZoneinDateTimeReports();
                    ToDate = ToDate.Value.ToClientTimeZoneinDateTimeReports();

                    var dataJson = _IGlobalAdmin.GetWorkOrderforDashboard(LocationID, objLoginSession.UserId, FromDate, ToDate);
                    return Json(new { dataJson }, JsonRequestBehavior.AllowGet);
                }
                else { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            {
                return Json(new { ex.InnerException }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created On: 11/05/2016
        /// Retrieve all QRC Scanned data for dashboard
        /// </summary>
        [HttpGet]
        public JsonResult GetQRCScannedDetail(string qrcType, string fromDate, string toDate)
        {
            try
            {
                long LocationID = 0;
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    eTracLoginModel objLoginSession = new eTracLoginModel();
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    DateTime? FromDate = (fromDate == null || string.IsNullOrWhiteSpace(fromDate)) ? DateTime.UtcNow.Date : Convert.ToDateTime(fromDate);
                    DateTime? ToDate = (toDate == null || string.IsNullOrWhiteSpace(fromDate)) ? DateTime.UtcNow : Convert.ToDateTime(toDate);
                    //This condition for if fromdate Todate is same but todate time is upto now.
                    ToDate = (ToDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : ToDate;
                    if (Session["eTrac_SelectedDasboardLocationID"] != null)
                    {
                        if (Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]) != 0)
                        {
                            LocationID = objLoginSession.LocationID;
                        }
                    }

                    if (ToDate != null)
                    {
                        if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
                            ToDate = (ToDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : ToDate;
                    }

                    if (FromDate != null && ToDate != null)
                    {
                        if ((FromDate.Value.Date == ToDate.Value.Date) && (ToDate.Value.ToLongTimeString() == "12:00:00 AM") || (ToDate.Value.ToLongTimeString() == "12:00:00 AM"))
                        {
                            ToDate = ToDate.Value.AddDays(1).AddSeconds(-1);
                        }
                    }

                    FromDate = FromDate.Value.ToClientTimeZoneinDateTimeReports();
                    ToDate = ToDate.Value.ToClientTimeZoneinDateTimeReports();

                    var dataJson = _IGlobalAdmin.QrcScannedDetails(LocationID, Convert.ToInt64(qrcType), objLoginSession.UserRoleId, objLoginSession.UserId, FromDate, ToDate);
                    return Json(new { dataJson }, JsonRequestBehavior.AllowGet);
                }
                else { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            {
                return Json(new { ex.InnerException }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Created By : Bhushan DOd
        /// Created Date :02/06/2016
        /// THis method for while view all location diabled an again enable then according to location wise setting need to show.
        /// </summary>
        /// <returns></returns>
        public WidgetList GetUserDashboardWidgetRolesViewAll()
        {
            long locationId = 0;
            eTracLoginModel ObjLoginModel = null;

            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                locationId = Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]);
                return _ILogin.GetDashboardWidgetList(ObjLoginModel.UserId, locationId);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// To get User Assignrd Premissions(Same as Get_UserAssignedRoles in login controller)
        /// </summary>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CretaedDate>2016/07/08</CretaedDate>
        /// <returns></returns>
        private List<string> Get_UserAssignedRolesDashboard()
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

        /// <summary>
        /// To navigate to Unverified List
        /// </summary>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CretaedDate>2016/10/19</CretaedDate>
        /// <returns></returns>
        public ActionResult UnVerifiedUsers(int? i)//i for if redirection from create user need to show msg created successfully.
        {
            if (i == 1)
            {
                ViewBag.Message = CommonMessage.UserSaveSuccessMessage();
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
            }

            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                return View("UnVerifiedUsers");
            }
            else
            {
                return Json("Session Expired !");
            }
        }

        [HttpGet]
        public JsonResult GetListOfUnverifiedUsers(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (locationId == null)
                {
                    locationId = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
            }

            JQGridResults result = new JQGridResults();
            List<JQGridRow> rowss = new List<JQGridRow>();
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);

            long TotalRows = 0;

            try
            {
                //ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
                long paramTotalRecords = 0;
                List<UserModelList> UnVerifiedUserList = _IGlobalAdmin.GetAllUnVerifiedUserList(UserId, Convert.ToInt64(locationId), page, rows, sidx, sord, txtSearch, UserType, out paramTotalRecords);
                foreach (var UnVerifiedUser in UnVerifiedUserList)
                {
                    JQGridRow row = new JQGridRow();
                    row.id = Cryptography.GetEncryptedData(UnVerifiedUser.UserId.ToString(), true);
                    row.cell = new string[7];
                    row.cell[0] = UnVerifiedUser.Name;
                    row.cell[1] = UnVerifiedUser.UserEmail;
                    row.cell[2] = UnVerifiedUser.Name;
                    row.cell[3] = UnVerifiedUser.UserType;
                    row.cell[4] = UnVerifiedUser.DOB.HasValue ? UnVerifiedUser.DOB.Value.ToShortDateString() : "";
                    row.cell[5] = (UnVerifiedUser.ProfileImage == "" || UnVerifiedUser.ProfileImage == null) ? "" : HostingPrefix + ProfileImagePath.Replace("~/", "") + UnVerifiedUser.ProfileImage;
                    row.cell[6] = UnVerifiedUser.EmployeeProfile;
                    rowss.Add(row);
                }

                result.rows = rowss.ToArray();
                result.page = Convert.ToInt32(page);
                result.total = (int)Math.Ceiling((decimal)(Convert.ToInt32(paramTotalRecords) / rows));
                result.records = Convert.ToInt32(paramTotalRecords);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
