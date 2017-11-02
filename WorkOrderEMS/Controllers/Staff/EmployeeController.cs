using System;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;

namespace WorkOrderEMS.Controllers.Staff
{
    [Authorize]
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/  
        private readonly ICommonMethod _ICommonMethod;
        private readonly IEmployeeManager _IEmployeeManager;
        private readonly IManageManager _IManageManager;
        private readonly IGlobalAdmin _IGlobalAdmin;
        private readonly IClientManager _IClientManager;
        private readonly IUser _IUser;
        private readonly IWorkRequestAssignment _IWorkRequestAssignment;

        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        private string path = ConfigurationManager.AppSettings["ProjectLogoPath"];
        private string ProfileImagePath = ConfigurationManager.AppSettings["ProfilePicPath"];
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        public EmployeeController(ICommonMethod _ICommonMethod, IEmployeeManager _IEmployeeManager, IManageManager _IManageManager, IGlobalAdmin _IGlobalAdmin, IClientManager _IClientManager, IWorkRequestAssignment _IWorkRequestAssignment)
        {
            this._ICommonMethod = _ICommonMethod;
            this._IEmployeeManager = _IEmployeeManager;
            this._IManageManager = _IManageManager;
            this._IGlobalAdmin = _IGlobalAdmin;
            this._IClientManager = _IClientManager;
            this._IWorkRequestAssignment = _IWorkRequestAssignment;
        }

        //public ActionResult Index()
        //{
        //    return View();
        //}
        #region MyProfile
        [HttpGet]
        public ActionResult EmployeeProfile()
        {
            try
            {
                long userid = 0; ;
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
                    ObjectParameter paramTotalrecord = new ObjectParameter("TotalRecords", typeof(int));
                    _UserModel.UserModel = _IManageManager.GetEmployeeById(userid, "GetUserByID", null, null, null, null, null, paramTotalrecord);
                    _UserModel.UpdateMode = true;
                }
                return View("EmployeeProfile", _UserModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult EmployeeProfile(QRCModel ObjUserModel)
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
                    //{
                    //    ObjUserModel.UserModel.ProfileImage = Convert.ToString(Session["ImageName"]);
                    //}
                    long QRCID = 0;
                    Result result = _IManageManager.SaveEmployee(ObjUserModel.UserModel, out QRCID, false);
                    if (result == Result.Completed)
                    {
                        ViewBag.Message = CommonMessage.SaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        ModelState.Clear();
                    }
                    else if (result == Result.DuplicateRecord)
                    {
                        ViewBag.Message = CommonMessage.DuplicateRecordEmailIdMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Info;// store the message for successful in tempdata to display in view.
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
            ObjUserModel.UserModel = _IManageManager.GetEmployeeById(ObjUserModel.UserModel.UserId, "GetUserByID", null, null, null, null, null, paramTotalrecord);
            return View("EmployeeProfile", ObjUserModel);
        }
        #endregion

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
        //        throw ex;
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
        //            _WorkRequestModel.ProjectId = (ObjLoginModel != null && ObjLoginModel.LocationID> 0) ? (ObjLoginModel.LocationID) : 0;
        //            _WorkRequestModel.RequestBy = ObjLoginModel.UserId;
        //            _WorkRequestModel.CreatedBy = ObjLoginModel.UserId;
        //            _WorkRequestModel.status = Convert.ToString(WorkRequestStatus.Pending);
        //            _WorkRequestModel.CreatedDate = DateTime.Now;
        //            _WorkRequestModel.IsDeleted = false;
        //        }
        //        Result result = _IManageManager.SaveWorkRequest(_WorkRequestModel);
        //        if (result == Result.Completed)
        //        {
        //            ViewBag.Message = CommonMessage.SaveSuccessMessage();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;

        //        }
        //        else if (result == Result.DuplicateRecord)
        //        {
        //            ViewBag.Message = CommonMessage.DuplicateRecordEmailIdMessage();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.Info; // store the message for successful in tempdata to display in view.
        //        }
        //        else if (result == Result.UpdatedSuccessfully)
        //        {
        //            ViewBag.Message = CommonMessage.UpdateSuccessMessage();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;// store the message for successful in tempdata to display in view.
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
        //    catch (Exception)
        //    {
        //        throw;
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
        //        ViewBag.UserId = ObjLoginModel.UserId;
        //        return View();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        #endregion

        [HttpPost]
        public virtual ActionResult UploadImage()
        {
            CommonHelper obj_CommonHelper = new CommonHelper();
            int c = Request.Files.Count;
            HttpPostedFileBase myFile = Request.Files["image"];
            Session["ImageName"] = DateTime.UtcNow.Ticks + myFile.FileName;
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
            //obj_CommonHelper.uploadImage(myFile, path, Session["ImageName"].ToString());
            return Json("message");
        }

        /// <summary>Create
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

                long LocationID = 0, UserID = 0;
                eTracLoginModel obj_eTracLoginModel = new eTracLoginModel();
                if (Session["eTrac"] != null)
                {

                    obj_eTracLoginModel = (eTracLoginModel)Session["eTrac"];
                    UserID = obj_eTracLoginModel.UserId;
                    LocationID = Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]);
                }
                ObjectParameter obj_ObjectParameter = new ObjectParameter("TotalRecords", typeof(int));
                ObjectParameter obj_ObjectParameter2 = new ObjectParameter("TotalRecords", typeof(int));
                GlobalAdminManager obj_GlobalAdminManager = new GlobalAdminManager();
              //  ViewBag.AllWorkOrder = obj_GlobalAdminManager.GetAllWorkRequestAssignment(0, 0, "GetAllWorkRequestAssignment", 1, 100000000, "WorkRequestAssignmentID", "asc", "", LocationID, obj_eTracLoginModel.UserId, DateTime.Now, DateTime.Now, "", obj_ObjectParameter).Count();
                ViewBag.AllAssignedWorkOrder = obj_GlobalAdminManager.GetAllWorkRequestAssignment(0, 0, "GetAllAssignedWorkRequest", 1, 100000000, "WorkRequestAssignmentID", "asc", "", LocationID, obj_eTracLoginModel.UserId, DateTime.Now, DateTime.Now, "", obj_ObjectParameter2).Count();

                return View();
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }

        /// <summary>Create
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-18-2014</CreatedOn>
        /// <CreatedFor>Load UI for Create New User</CreatedFor>
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        public ActionResult CreateEmployee(string usr)
        {
            try
            {
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                var _UserModel = _ICommonMethod.LoadInvitedUser(usr);
                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
                return View("Employee", _UserModel);
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
        public ActionResult Index(QRCModel ObjUserModel)
        {
            DARModel objDAR = null; long LocId = 0;

            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }


            if (Session["eTrac_SelectedDasboardLocationID"] != null)
            {
                LocId = (long)Session["eTrac_SelectedDasboardLocationID"];
            }
            try
            {

                //if (ModelState.IsValid)
                //{

                if (ObjUserModel != null && ObjUserModel.UserModel != null) //&& ObjUserModel.UserModel.UserId == 0
                {
                    objDAR = new DARModel();
                    objDAR.LocationId = ObjLoginModel.LocationID;
                    objDAR.UserId = ObjLoginModel.UserId;
                    objDAR.CreatedBy = ObjLoginModel.UserId;
                    objDAR.CreatedOn = DateTime.UtcNow;


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

                        objDAR.TaskType = (long)TaskTypeCategory.UserCreation;
                        objDAR.ActivityDetails = DarMessage.NewEmployeeCreatedDar(ObjLoginModel.Location);
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
                        objDAR.ActivityDetails = DarMessage.EmployeeUpdatedDar(ObjLoginModel.Location);
                    }

                    if (ObjUserModel.UserModel.ProfileImage != null)
                    {
                        string ImageName = ObjLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + ObjUserModel.UserModel.ProfileImage.FileName.ToString();
                        CommonHelper obj_CommonHelper = new CommonHelper();
                        obj_CommonHelper.UploadImage(ObjUserModel.UserModel.ProfileImage, Server.MapPath(ConfigurationManager.AppSettings["ProfilePicPath"]), ImageName);
                        ObjUserModel.UserModel.ProfileImageFile = ImageName;
                    }
                    long QRCID = 0;

                    Result result = _IClientManager.SaveClient(ObjUserModel.UserModel, out QRCID, true, objDAR, LocId, ObjLoginModel.UserId, "");

                    if (result == Result.Completed)
                    {
                        var abc = _ICommonMethod.GetLocationDetailsById(ObjLoginModel.LocationID);

                        EmailHelper objEmailHelper = new EmailHelper();
                        objEmailHelper.emailid = ObjUserModel.UserModel.UserEmail;
                        objEmailHelper.LocationName = abc.LocationName;
                        objEmailHelper.LocAddress = ObjUserModel.UserModel.Address.Address1; // here locAddress means user Address
                        objEmailHelper.UserName = ObjUserModel.UserModel.AlternateEmail;
                        objEmailHelper.UserType = ObjUserModel.UserModel.UserType;
                        objEmailHelper.FirstName = ObjUserModel.UserModel.FirstName;
                        objEmailHelper.LastName = ObjUserModel.UserModel.LastName;
                        objEmailHelper.Password = Cryptography.GetDecryptedData(ObjUserModel.UserModel.Password, true);
                        objEmailHelper.MailType = "CreateNewUser";

                        HostingPrefix = HostingPrefix + "Manager/Employee?usr=" + Cryptography.GetEncryptedData(ObjUserModel.UserModel.UserId.ToString(), true);
                        objEmailHelper.RegistrationLink = HostingPrefix;
                        objEmailHelper.SendEmailWithTemplate();

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

                //System.Data.Entity.Core.Objects.ObjectParameter paramTotalRecords = new System.Data.Entity.Core.Objects.ObjectParameter("TotalRecords", typeof(int));
                //ObjUserModel.JobTitleList = _ICommonMethod.GetGlobalCodeData("JOBTITLE");
                //ObjUserModel.LocationList = _IGlobalAdmin.GetAllLocationList(0, "GetAllLocation", 1, 10000, "LocationName", "desc", "", paramTotalRecords);
                //paramTotalRecords = null;
            }

            ViewBag.UpdateMode = false;
            //ObjUserModel.UserModel = _IClientManager.GetClientByID(ObjUserModel.UserModel.UserId, "GetUserByID", null, null, null, null, null);


            //1	USERTYPE	Global Admin
            //2	USERTYPE	Manager
            //3	USERTYPE	Employee
            //4	USERTYPE	Client
            //5	USERTYPE	IT Administrator
            //6	USERTYPE	Administrator
            //137	USERTYPE	Vendor User
            //138	USERTYPE	Guest User
            //switch (ObjLoginModel.UserRoleId)
            //{
            //    case 1:
            //        {
            //            //return View("", ObjUserModel); 
            //            return RedirectToAction("Index", "GlobalAdmin");
            //        } break;
            //    case 2:
            //        { } break;
            //    case 3:
            //        { } break;
            //    case 4:
            //        { } break;
            //    case 5:
            //        { } break;
            //    case 6:
            //        { } break;
            //}

            return View("Employee", ObjUserModel);
        }

        #region WorkRequest New
        [HttpGet]
        public ActionResult WorkRequestAssignment()
        {
            try
            {
                eTracLoginModel objeTracLoginModel = (eTracLoginModel)(Session["eTrac"]);
                ViewBag.AssignToUser = _IGlobalAdmin.GetLocationEmployee(objeTracLoginModel.LocationID);
                ViewBag.Asset = _ICommonMethod.GetAssetList(objeTracLoginModel.LocationID);
                ViewBag.Location = _ICommonMethod.GetLocationByEmpId(objeTracLoginModel.UserId);
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
            try
            {

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
        /// TO GET EMPLOYEE ASSIGNED WORK
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <cretaedFate>2015-03-12</cretaedFate>
        /// <param name="LocationId"></param>
        /// <param name="UserID"></param>
        /// <param name="OrderBy"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetEmployeeAssignedWorkRequest(long LocationId, long UserID, string OrderBy, string columnName)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);

                    return Json(_IUser.GetEmployeeAssignedWorkRequest(LocationId, UserID, OrderBy, columnName));
                }
                else { return Json("Session Expired !"); }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// TO ACCEPT THE WORKORDER ASSIGNMENT BY EMPLOYEE ITSELF
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>4-9-2015</CreatedDate>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AcceptWorkOrderCreatedByClient(string Id)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                long WorOrderID = Convert.ToInt64(Cryptography.GetDecryptedData(Id, true));
                var Res = _IWorkRequestAssignment.AcceptWorkOrderByEmployee(WorOrderID, ObjLoginModel.UserId);
                if (Res == Result.Completed)
                {
                    return Json("Work order Assigned.");
                }
                else if (Res == Result.DuplicateRecord)
                {
                    return Json("Work order already assigned.");
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
        /// <summary>
        /// TO SET STATUS INPROGRESS FROM PENDING 
        /// </summary>
        /// <param name="WorOrderID"></param>
        /// <param name="StartTime"></param>
        /// <returns></returns>
        /// <ModifiedBy> Bhushan </ModifiedBy>
        /// <ModifiedOn> 06/10/2015 </ModifiedOn>
        /// <ModifiedFor> Update starttime bcoz in service it Datetime.Now and previously  from jqueryDate </ModifiedFor>
        [HttpPost]
        public JsonResult StartWorkOrderByEmployee(string Id, string StartTime)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    long WorOrderID = Convert.ToInt64(Cryptography.GetDecryptedData(Id, true));
                    var Res = _IWorkRequestAssignment.StartWorkOrderByEmployee(WorOrderID, ObjLoginModel.UserId, StartTime);
                    if (Res == Result.UpdatedSuccessfully)
                    {
                        return Json("Work order Start.");
                    }
                    else if (Res == Result.DoesNotExist)
                    {
                        return Json("Work order not Exist.");
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
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// TO SET STATUS COMPLETE FROM IN PROGRESS 
        /// </summary>
        /// <param name="WorOrderID"></param>
        /// <param name="iUserId"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        /// <ModifiedBy> Bhushan </ModifiedBy>
        /// <ModifiedOn> 06/10/2015 </ModifiedOn>
        /// <ModifiedFor> Update endtime bcoz in service it Datetime.Now and previously  from jqueryDate </ModifiedFor>
        [HttpPost]
        public JsonResult CompleteWorkOrderByEmployee(string Id, string EndTime)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    long WorOrderID = Convert.ToInt64(Cryptography.GetDecryptedData(Id, true));
                    var Res = _IWorkRequestAssignment.CompleteWorkOrderByEmployee(WorOrderID, ObjLoginModel.UserId, EndTime);
                    if (Res == Result.UpdatedSuccessfully)
                    {
                        return Json("Work order Completed.");
                    }
                    else if (Res == Result.DoesNotExist)
                    {
                        return Json("Work order Not Exist.");
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
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// TO GET EMPLOYEE DASHBOARD COUNT
        /// <CreatedDate>14/April/2015</CreatedDate>
        /// <createBy>Manoj Jaswal</createBy>
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetEmployeeTotalWorkStatus()
        {
            try
            {
                eTracLoginModel obj_eTracLoginModel = new eTracLoginModel();
                long UserId; long LocationId;
                if (Session["eTrac"] != null)
                {

                    obj_eTracLoginModel = (eTracLoginModel)Session["eTrac"];
                    UserId = obj_eTracLoginModel.UserId;
                    LocationId = Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]);
                    return Json(_IWorkRequestAssignment.GetEmployeeTotalWorkStatus(UserId, LocationId));
                }
                else { return Json("Session expired!"); }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
