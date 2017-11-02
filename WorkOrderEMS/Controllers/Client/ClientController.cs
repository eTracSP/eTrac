using System;
using System.Configuration;
using System.Web.Mvc;
using WorkOrderEMS.App_Start;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;

namespace WorkOrderEMS.Controllers.Client
{
    [CustomActionFilter]
    [Authorize]
    public class ClientController : Controller
    {
        //
        // GET: /Client/
        private readonly ICommonMethod _ICommonMethod;
        private readonly IManageManager _IManageManager;
        private readonly IClientManager _IClientManager;
        private readonly IGlobalAdmin _IGlobalAdmin;
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        private string path = ConfigurationManager.AppSettings["ProjectLogoPath"];
        private string WorkRequestImagepath = ConfigurationManager.AppSettings["WorkRequestImage"];
        public ClientController(ICommonMethod _ICommonMethod, IManageManager _IManageManager, IClientManager _IClientManager, IGlobalAdmin _IGlobalAdmin)
        {
            this._ICommonMethod = _ICommonMethod;
            this._IManageManager = _IManageManager;
            this._IClientManager = _IClientManager;
            this._IGlobalAdmin = _IGlobalAdmin;
        }

        #region client This section is use when user click link in email


        //This Method is use to open the Client  Profile 
        //information when click on verify link in email
        [NonAction]
        public ActionResult Client(string usr)
        {
            try
            {
                //long userid = 53;
                long userid = 0;

                if (!string.IsNullOrEmpty(usr))
                {
                    usr = Cryptography.GetDecryptedData(usr, true);
                    long.TryParse(usr, out userid);
                }
                QRCModel _UserModel = new QRCModel();
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                if (userid > 0)
                {
                    _UserModel.UserModel = _IClientManager.GetClientById(userid, "GetUserByID", null, null, null, null, null);
                    //_UserModel.Password = Cryptography.GetDecryptedData(_UserModel.Password, true);
                    _UserModel.UserModel.Password = "";
                }
                return View("Client", _UserModel);
            }


            catch (Exception ex) { ViewBag.Error = ex.Message; return View("Error"); }
            //ViewBag.UpdateMode = false;
            //return View();
        }

        //This Method is use to open the Client  Profile 

        [HttpGet]
        public ActionResult ClientProfile()
        {
            try
            {
                // long userid = 53; ;
                long userid = 0;
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

                //ViewBag.ProjectID = (ObjLoginModel != null && ObjLoginModel.LocationID > 0) ? (ObjLoginModel.LocationID) : 0;
                //userid = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;
                userid = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;



                QRCModel _UserModel = new QRCModel();
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                if (userid > 0)
                {
                    _UserModel.UserModel = _IClientManager.GetClientById(userid, "GetUserByID", null, null, null, null, null);
                    _UserModel.UpdateMode = true;
                }
                return View("Client", _UserModel);
            }
            catch (Exception ex)
            {
                { ViewBag.Error = ex.Message; return View("Error"); }
            }
        }

        [HttpPost]
        public ActionResult Client(QRCModel ObjUserModel, string ActionName)
        {
            //Commented By Bhushan DOD for Code review which is unused variable
            //ObjectParameter paramTotalrecord = new ObjectParameter("TotalRecords", typeof(int));
            var action = this.ControllerContext.RouteData.Values["action"].ToString();
            DARModel objDAR = null;

            try
            {

                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

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

                        objDAR = new DARModel();
                        objDAR.LocationId = ObjLoginModel.LocationID;
                        objDAR.UserId = ObjLoginModel.UserId;
                        objDAR.CreatedBy = ObjLoginModel.UserId;
                        objDAR.CreatedOn = DateTime.UtcNow;
                        objDAR.ActivityDetails = DarMessage.NewManagerCreatedDar(ObjLoginModel.Location);
                    }
                    else
                    {
                        ObjUserModel.UserModel.ModifiedBy = ObjLoginModel.UserId;
                        ObjUserModel.UserModel.ModifiedDate = DateTime.UtcNow;
                        ObjUserModel.UserModel.IsDeleted = false;


                        objDAR = new DARModel();
                        objDAR.LocationId = ObjLoginModel.LocationID;
                        objDAR.UserId = ObjLoginModel.UserId;
                        objDAR.ModifiedBy = ObjLoginModel.UserId;
                        objDAR.ModifiedOn = DateTime.UtcNow;
                        objDAR.ActivityDetails = DarMessage.ManagerUpdatedDar(ObjLoginModel.Location);
                    }
                    //if (Session["ImageName"] != null)
                    //{
                    //    ObjUserModel.UserModel.ProfileImage = Convert.ToString(Session["ImageName"]);
                    //}

                    //Result result = _IGlobalAdmin.SaveLocation(ObjUserModel, out locationId);
                    //if (result == Result.Completed)

                    long QRCID = 0;
                    //Result result = _IClientManager.SaveClient(ObjUserModel.UserModel, out QRCID, true, objDAR);
                    Result result = _IClientManager.SaveClient(ObjUserModel.UserModel, out QRCID, true, objDAR, ObjLoginModel.LocationID, ObjLoginModel.UserId, "");
                    if (result == Result.Completed)
                    {
                        ViewBag.Message = CommonMessage.SaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        ModelState.Clear();
                        return RedirectToAction("Login", "GlobalAdmin");
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
                        return RedirectToAction("Login", "GlobalAdmin");

                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                    }

                    //   ViewBag.Location = Cryptography.GetEncryptedData(QRCID.ToString(), true);
                    //}
                    //else
                    //{
                    //    ViewBag.Message = CommonMessage.FillAllRequired(); // Please fill all required fields.
                    //}
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
            ObjUserModel.UserModel = _IClientManager.GetClientById(ObjUserModel.UserModel.UserId, "GetUserByID", null, null, null, null, null);
            ViewBag.ActionName = ActionName;
            return View("Client", ObjUserModel);

        }

        #endregion Manager

        #region Work Request OLD

        //[HttpGet]
        //public ActionResult WorkRequest()
        //{
        //    try
        //    {
        //        WorkRequestModel WorkRequestModel = new WorkRequestModel();
        //        ViewBag.TaskType = _ICommonMethod.GetGlobalCodeDataList(Convert.ToString(GolbalCodeName.TASKTYPE));
        //        ViewBag.TaskPriority = _ICommonMethod.GetGlobalCodeDataList(Convert.ToString(GolbalCodeName.WORKPRIORITY));
        //        ViewBag.WorkArea = _ICommonMethod.GetWorkArea();

        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        { ViewBag.Error = ex.Message; return View("Error"); }
        //    }
        //}

        //[HttpPost]
        //public ActionResult WorkRequest(WorkRequestModel _WorkRequestModel)
        //{
        //    try
        //    {
        //        eTracLoginModel ObjLoginModel = null;
        //        if (Session["eTrac"] != null)
        //        { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

        //        if (_WorkRequestModel.WorkRequestID == 0)
        //        {
        //            _WorkRequestModel.ProjectId = (ObjLoginModel != null && ObjLoginModel.LocationID > 0) ? (ObjLoginModel.LocationID) : 0;
        //            _WorkRequestModel.RequestBy = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;
        //            _WorkRequestModel.CreatedBy = _WorkRequestModel.RequestBy;

        //            //_WorkRequestModel.ProjectId = Session["ProjectID"] != null ? Convert.ToInt32(Session["ProjectID"]) : 0;
        //            //_WorkRequestModel.RequestBy = Session["UserID"] != null ? Convert.ToInt32(Session["UserID"]) : 0; ;
        //            //_WorkRequestModel.CreatedBy = Session["UserID"] != null ? Convert.ToInt32(Session["UserID"]) : 0; ;
        //            _WorkRequestModel.status = Convert.ToString(WorkRequestStatus.Pending);
        //            _WorkRequestModel.CreatedDate = DateTime.Now;
        //            _WorkRequestModel.IsDeleted = false;
        //        }
        //        Result result = _IManageManager.SaveWorkRequest(_WorkRequestModel);
        //        if (result == Result.Completed)
        //        {
        //            ViewBag.Message = CommonMessage.SaveSuccessMessage();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.success;

        //        }
        //        else if (result == Result.DuplicateRecord)
        //        {
        //            ViewBag.Message = CommonMessage.DuplicateRecordEmailIdMessage();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.Info; // store the message for successful in tempdata to display in view.
        //        }
        //        else if (result == Result.UpdatedSuccessfully)
        //        {
        //            ViewBag.Message = CommonMessage.UpdateSuccessMessage();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.success;// store the message for successful in tempdata to display in view.
        //        }
        //        else
        //        {
        //            ViewBag.Message = CommonMessage.FailureMessage();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
        //        }
        //        ViewBag.TaskType = _ICommonMethod.GetGlobalCodeDataList(Convert.ToString(GolbalCodeName.TASKTYPE));
        //        ViewBag.TaskPriority = _ICommonMethod.GetGlobalCodeDataList(Convert.ToString(GolbalCodeName.WORKPRIORITY));
        //        ViewBag.WorkArea = _ICommonMethod.GetWorkArea();
        //        return View("WorkRequest", _WorkRequestModel);

        //    }
        //    catch (Exception ex)
        //    {
        //        { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        //    }

        //}

        //public ActionResult WorkRequestList()
        //{
        //    try
        //    {
        //        eTracLoginModel ObjLoginModel = null;
        //        if (Session["eTrac"] != null)
        //        { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

        //        ViewBag.ProjectID = (ObjLoginModel != null && ObjLoginModel.LocationID > 0) ? (ObjLoginModel.LocationID) : 0;
        //        ViewBag.UserId = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        //    }
        //}
        #endregion


        /// <summary>Create
        /// Modified by vijay sahu on 20 feb 2015
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-18-2014</CreatedOn>
        /// <CreatedFor>Load UI for Create New User</CreatedFor>
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        public ActionResult Index(string usr)
        {
            try
            {
                //ViewBag.Country = _ICommonMethod.GetAllcountries();
                //var _UserModel = _ICommonMethod.LoadInvitedUser(usr);
                //ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("JOBTITLE");
                //ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
                //return View("myClient", _UserModel);
                return View();
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }

        /// <summary>Create
        /// Modified by vijay sahu on 20 feb 2015  
        /// <CreatedFor>Load UI for Create New User</CreatedFor>
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        public ActionResult Create(string usr)
        {
            try
            {
                //string UserId = Cryptography.GetDecryptedData(usr, true);
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                var _UserModel = _ICommonMethod.LoadInvitedUser(usr);
                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
                return View("myClient", _UserModel);
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }


        /// <summary>ClientRegistration
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-18-2014</CreatedOn>
        /// <CreatedFor>Load Partial UI to Create New User</CreatedFor>
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        //[HttpGet]
        public ActionResult ClientRegistration(UserModel ObjUserModel)
        {
            try
            {
                //UserModel ObjUserModel = new UserModel();
                ViewBag.myModelprefixName = "ClientModel.";
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                //var _UserModel = _ICommonMethod.LoadInvitedUser(usr);
                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
                return PartialView("_myRegistration", ObjUserModel);
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }


        //[HttpGet]
        //public ActionResult ClientRegistration()
        //{
        //    UserModel ObjUserModel = new UserModel();
        //    return RedirectToAction("ClientRegistration", ObjUserModel);
        //}




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
            DARModel objDAR = null;
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

                //if (ModelState.IsValid)
                //{

                if (ObjUserModel != null && ObjUserModel.UserModel != null) //&& ObjUserModel.UserModel.UserId == 0
                {
                    if (ObjUserModel.UserModel.UserId == 0)
                    {
                        #region password

                        ObjUserModel.UserModel.Password = _ICommonMethod.CreateRandomPassword();

                        /*
                        ObjUserModel.UserModel.Password = Guid.NewGuid().ToString();
                        ObjUserModel.UserModel.Password = ObjUserModel.UserModel.Password.Length > pwdmaxlendth ? ObjUserModel.UserModel.Password.Substring(0, pwdmaxlendth) : ObjUserModel.UserModel.Password;
                        ObjUserModel.UserModel.Password = Cryptography.GetEncryptedData(ObjUserModel.UserModel.Password, true);
                        //ObjUserModel.UserModel.Password = (!string.IsNullOrEmpty(ObjUserModel.UserModel.Password)) ? Cryptography.GetEncryptedData(ObjUserModel.UserModel.Password, true) : ObjUserModel.UserModel.Password;
                        */
                        #endregion password

                        ObjUserModel.UserModel.CreatedBy = ObjLoginModel.UserId;
                        ObjUserModel.UserModel.CreatedDate = DateTime.UtcNow;
                        ObjUserModel.UserModel.IsDeleted = false;

                        objDAR = new DARModel();
                        objDAR.LocationId = ObjLoginModel.LocationID;
                        objDAR.UserId = ObjLoginModel.UserId;
                        objDAR.CreatedBy = ObjLoginModel.UserId;
                        objDAR.CreatedOn = DateTime.UtcNow;
                        objDAR.ActivityDetails = DarMessage.NewManagerCreatedDar(ObjLoginModel.Location);

                    }
                    else
                    {
                        ObjUserModel.UserModel.ModifiedBy = ObjLoginModel.UserId;
                        ObjUserModel.UserModel.ModifiedDate = DateTime.UtcNow;
                        ObjUserModel.UserModel.IsDeleted = false;

                        objDAR = new DARModel();
                        objDAR.LocationId = ObjLoginModel.LocationID;
                        objDAR.UserId = ObjLoginModel.UserId;
                        objDAR.ModifiedBy = ObjLoginModel.UserId;
                        objDAR.ModifiedOn = DateTime.UtcNow;
                        objDAR.ActivityDetails = DarMessage.ManagerUpdatedDar(ObjLoginModel.Location);
                    }

                    long QRCID = 0;
                    //Result result = _IClientManager.SaveClient(ObjUserModel.UserModel, out QRCID, true, objDAR);
                    Result result = _IClientManager.SaveClient(ObjUserModel.UserModel, out QRCID, true, objDAR, ObjLoginModel.LocationID, ObjLoginModel.UserId, "");

                    if (result == Result.Completed)
                    {
                        ViewBag.Message = CommonMessage.SaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        ModelState.Clear();
                        //return View("ITAdministrator");//return RedirectToAction("Create ", "GlobalAdmin");
                        ObjUserModel = _ICommonMethod.LoadInvitedUser(string.Empty);
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

                //System.Data.Entity.Core.Objects.ObjectParameter paramTotalRecords = new System.Data.Entity.Core.Objects.ObjectParameter("TotalRecords", typeof(int));
                //ObjUserModel.JobTitleList = _ICommonMethod.GetGlobalCodeData("JOBTITLE");
                //ObjUserModel.LocationList = _IGlobalAdmin.GetAllLocationList(0, "GetAllLocation", 1, 10000, "LocationName", "desc", "", paramTotalRecords);
                //paramTotalRecords = null;
            }

            ViewBag.UpdateMode = false;
            //ObjUserModel.UserModel = _IClientManager.GetClientByID(ObjUserModel.UserModel.UserId, "GetUserByID", null, null, null, null, null);
            return View("myClient", ObjUserModel);
        }


        #region WorkRequest New
        [HttpGet]
        public ActionResult WorkRequestAssignment()
        {
            try
            {
                eTracLoginModel objeTracLoginModel = (eTracLoginModel)(Session["eTrac"]);
                //Added By Bhushan Dod on 03/27/2015 for Harcoded change value
                ViewBag.AssignToUser = _IGlobalAdmin.GetLocationEmployee(objeTracLoginModel.LocationID);//Here previously hard coded value 
                ViewBag.Asset = _ICommonMethod.GetAssetList(objeTracLoginModel.LocationID);
                ViewBag.Location = _ICommonMethod.GetLocationByClientId(objeTracLoginModel.UserId);
                ViewBag.PriorityLevel = _ICommonMethod.GetGlobalCodeData("WORKPRIORITY");
                ViewBag.WorkRequestType = _ICommonMethod.GetGlobalCodeData("WORKREQUESTTYPE");
                ViewBag.WorkRequestProjectTypeID = _ICommonMethod.GetGlobalCodeData("WORKREQUESTPROJECTTYPE");
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult WorkRequestAssignment(WorkRequestAssignmentModel objWorkRequestAssignmentModel)
        {
            eTracLoginModel objeTracLoginModel = (eTracLoginModel)(Session["eTrac"]);
            WorkRequestAssignmentModel _objWorkRequestAssignmentModel = new WorkRequestAssignmentModel();
            string msg = string.Empty;
            try
            {

                CommonHelper ObjCommonHelper = new CommonHelper();
                if (objWorkRequestAssignmentModel.WorkRequestAssignmentID == 0)
                {
                    objWorkRequestAssignmentModel.CreatedBy = objeTracLoginModel.UserId;
                    objWorkRequestAssignmentModel.CreatedDate = DateTime.UtcNow;
                    objWorkRequestAssignmentModel.RequestBy = objeTracLoginModel.UserId;
                    objWorkRequestAssignmentModel.WorkRequestStatus = 14;
                }
                else
                {
                    objWorkRequestAssignmentModel.ModifiedBy = objeTracLoginModel.UserId;
                    objWorkRequestAssignmentModel.ModifiedDate = DateTime.UtcNow;
                }

                _objWorkRequestAssignmentModel = _IGlobalAdmin.SaveWorkRequestAssignment(objWorkRequestAssignmentModel);
                if (objWorkRequestAssignmentModel.WorkRequestImg != null)
                {
                    WorkRequestImagepath = Server.MapPath(WorkRequestImagepath);
                    ObjCommonHelper.UploadImage(objWorkRequestAssignmentModel.WorkRequestImg, WorkRequestImagepath, objWorkRequestAssignmentModel.WorkRequestImg.FileName);
                }
                if (_objWorkRequestAssignmentModel.Result == Result.Completed)
                {
                    ViewBag.Message = CommonMessage.SaveSuccessMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                }
                else if (_objWorkRequestAssignmentModel.Result == Result.DuplicateRecord)
                {
                    ViewBag.Message = CommonMessage.DuplicateRecordMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Info; // store the message for successful in tempdata to display in view.
                }
                else if (_objWorkRequestAssignmentModel.Result == Result.UpdatedSuccessfully)
                {
                    ViewBag.Message = CommonMessage.UpdateSuccessMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;// store the message for successful in tempdata to display in view.

                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                }
                return View("WorkRequestAssignment", _objWorkRequestAssignmentModel);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ViewBag.AssignToUser = _IGlobalAdmin.GetLocationEmployee(objeTracLoginModel.LocationID);
                ViewBag.Asset = _ICommonMethod.GetAssetList(objeTracLoginModel.LocationID);
                ViewBag.Location = _ICommonMethod.GetAllLocation();
                ViewBag.PriorityLevel = _ICommonMethod.GetGlobalCodeData("WORKPRIORITY");
                ViewBag.WorkRequestType = _ICommonMethod.GetGlobalCodeData("WORKREQUESTTYPE");
                ViewBag.WorkRequestProjectTypeID = _ICommonMethod.GetGlobalCodeData("WORKREQUESTPROJECTTYPE");
            }
        }
        public ActionResult WorkAssignmentList()
        {
            eTracLoginModel objeTracLoginModel = (eTracLoginModel)(Session["eTrac"]);
            try
            {
                ViewBag.UserID = objeTracLoginModel.UserId;
                return View("WorkAssignmentList");

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Created By : Bhushan Dod on 26/02/2015
        /// Description : To Create work request created client.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateClient(UserModel UserModel)
        {
            //Commented by Bhushan Dod for Code review bcoz unused variable.
            //ObjectParameter paramTotalrecord = new ObjectParameter("TotalRecords", typeof(int));
            var action = this.ControllerContext.RouteData.Values["action"].ToString();
            DARModel objDAR = null;
            try
            {

                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

                if (UserModel != null)
                {
                    UserModel.Password = (!string.IsNullOrEmpty(UserModel.Password)) ? Cryptography.GetEncryptedData(UserModel.Password, true) : UserModel.Password;
                    //if (ModelState.IsValid)
                    //{
                    if (UserModel.UserId == 0)
                    {
                        if (UserModel.ProfileImage != null)
                        {
                            string ClImageName = UserModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + UserModel.ProfileImage.FileName.ToString();
                            CommonHelper obj_CommonHelper = new CommonHelper();
                            obj_CommonHelper.UploadImage(UserModel.ProfileImage, Server.MapPath(ConfigurationManager.AppSettings["ProfilePicPath"]), ClImageName);
                            UserModel.ProfileImageFile = ClImageName;
                        }
                        UserModel.CreatedBy = ObjLoginModel.UserId;
                        UserModel.CreatedDate = DateTime.UtcNow;
                        UserModel.IsDeleted = false;

                        objDAR = new DARModel();
                        objDAR.LocationId = UserModel.Location;
                        objDAR.UserId = ObjLoginModel.UserId;
                        objDAR.CreatedBy = ObjLoginModel.UserId;
                        objDAR.CreatedOn = DateTime.UtcNow;
                        objDAR.ActivityDetails = DarMessage.NewClientCreatedDar(ObjLoginModel.Location);
                        objDAR.TaskType = Convert.ToInt64(TaskTypeCategory.UserCreation);
                    }
                    else
                    {
                        if (UserModel.ProfileImage != null)
                        {
                            string ClImageName = UserModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + UserModel.ProfileImage.FileName.ToString();
                            CommonHelper obj_CommonHelper = new CommonHelper();
                            obj_CommonHelper.UploadImage(UserModel.ProfileImage, Server.MapPath(ConfigurationManager.AppSettings["ProfilePicPath"]), ClImageName);
                            UserModel.ProfileImageFile = ClImageName;
                        }
                        UserModel.ModifiedBy = ObjLoginModel.UserId;
                        UserModel.ModifiedDate = DateTime.UtcNow;
                        UserModel.IsDeleted = false;
                        if (!String.IsNullOrEmpty(UserModel.Password))
                        {
                            UserModel.Password = Cryptography.GetEncryptedData(UserModel.Password, true);
                        }

                        objDAR = new DARModel();
                        objDAR.LocationId = ObjLoginModel.LocationID;
                        objDAR.UserId = ObjLoginModel.UserId;
                        objDAR.ModifiedBy = ObjLoginModel.UserId;
                        objDAR.ModifiedOn = DateTime.UtcNow;
                        objDAR.ActivityDetails = DarMessage.ClientUpdatedDar(ObjLoginModel.Location);
                        objDAR.TaskType = Convert.ToInt64(TaskTypeCategory.UserUpdate);
                    }
                    //if (Session["ImageName"] != null)
                    //{
                    //    ObjUserModel.UserModel.ProfileImage = Convert.ToString(Session["ImageName"]);
                    //}

                    //Result result = _IGlobalAdmin.SaveLocation(ObjUserModel, out locationId);
                    //if (result == Result.Completed)

                    long QRCID = 0;
                    //Result result = _IClientManager.SaveClient(ObjLocationMasterModel.ClientModel, out QRCID, true, objDAR);
                    Result result = _IClientManager.SaveClient(UserModel, out QRCID, true, objDAR, 0, ObjLoginModel.UserId, "");

                    if (result == Result.Completed)
                    {
                        ViewBag.Message = CommonMessage.SaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        ModelState.Clear();
                        return View("myClient");
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
                        return View("myClient");
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
            UserModel = _IClientManager.GetClientById(UserModel.UserId, "GetUserByID", null, null, null, null, null);
            //ViewBag.ActionName = ActionName;
            return View("myClient");


        }

        /// <summary>
        /// Created By : Bhushan Dod on 26/02/2015
        /// Description : To Cancel work request created by client.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CancelWorkRequestCreatedByClient(string Id)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                long WorOrderID = Convert.ToInt64(Cryptography.GetDecryptedData(Id, true));
                var Res = _IClientManager.CancelWorkOrderByEmployee(WorOrderID, ObjLoginModel.UserId);
                if (Res == Result.Delete)
                {
                    return Json("Work order Canceled .");
                }
                else if (Res == Result.DoesNotExist)
                {
                    return Json("Work order not found");
                }
                else
                {
                    return Json(Res);
                }

            }
            else
            {
                return Json("Session Expired.");
            }


        }
    }
}