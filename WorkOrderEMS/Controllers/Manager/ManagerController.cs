using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Exception_B;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers.Manager
{
    [Authorize]
    public class ManagerController : Controller
    {
        //
        //// GET: /Manager/
        private readonly ICommonMethod _ICommonMethod;
        private readonly IManageManager _IManageManager;
        private readonly IGlobalAdmin _IGlobalAdmin;
        private readonly IClientManager _IClientManager;
        private readonly IUser _IUser;
        private readonly IWorkRequestAssignment _IWorkRequestAssignment;
        private List<InventoryMasterModelList> InventoryList;
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], System.Globalization.CultureInfo.InvariantCulture);
        ManageManager _ManageManager = new ManageManager();

        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();

        private string path = ConfigurationManager.AppSettings["ProjectLogoPath"];
        private string W9FormPathVendor = ConfigurationManager.AppSettings["W9FormPathVendor"];
        //private string PWDGUIDMaxLength = ConfigurationManager.AppSettings["PWDGUIDMaxLength"];
        //int pwdmaxlendth = 10;

        public ManagerController(ICommonMethod _ICommonMethod, IManageManager _IManageManager, IGlobalAdmin _IGlobalAdmin, IClientManager _IClientManager)
        {
            this._ICommonMethod = _ICommonMethod;
            this._IManageManager = _IManageManager;
            this._IGlobalAdmin = _IGlobalAdmin;
            this._IClientManager = _IClientManager;
        }

        //public ActionResult Index()
        //{
        //    return View();
        //}

        #region Employee Registration
        /// <summary>
        /// CreatedBy   :  Gayatri Pal
        /// CreatedOn   :   Sep-06-2014
        /// CreatedFor  :   This action method is use for employee registration link"
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Employee(string usr)
        {
            try
            {


                _ICommonMethod.ActivateNewUser(usr);

                return RedirectToAction("Index", "Login");
                //return View("Employee", _ICommonMethod.LoadInvitedUser(usr));
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }

        }

        /// <summary>
        /// CreatedBy   :   Gayatri Pal
        /// CreatedOn   :   Sep-06-2014
        /// CreatedFor  :   This action method is use for employee profile registration through link "
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult Employee(QRCModel ObjUserModel)
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

                    //Result result = _IGlobalAdmin.SaveLocation(ObjUserModel, out locationId);
                    //if (result == Result.Completed)

                    long QRCID = 0;
                    Result result = _IManageManager.SaveEmployee(ObjUserModel.UserModel, out QRCID, true);
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
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Info;// store the message for successful in tempdata to display in view.
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

                    //  ViewBag.Location = Cryptography.GetEncryptedData(QRCID.ToString(), true);
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
            ObjUserModel.UserModel = _IManageManager.GetEmployeeById(ObjUserModel.UserModel.UserId, "GetUserByID", null, null, null, null, null, paramTotalrecord);
            return View("Employee", ObjUserModel);
        }

        /// <summary>
        /// CreatedBy   :   Gayatri Pal
        /// CreatedOn   :   Sep-06-2014
        /// CreatedFor  :   This action method is use to invite employee "
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InviteEmployee()
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

        [HttpPost]
        public ActionResult InviteEmployee(UserModel objUserModel)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
                //if (ModelState.IsValid)
                //{
                GlobalAdminManager _SuperAdminManager = new GlobalAdminManager(this._ICommonMethod);
                objUserModel.CreatedBy = ObjLoginModel.UserId;
                objUserModel.CreatedDate = DateTime.UtcNow;
                objUserModel.IsDeleted = false;

                var _usertype = _ICommonMethod.GetGlobalCodeForName("USERTYPE", "Employee");
                if (_usertype > 0) { objUserModel.UserType = _usertype; }

                //For server
                //objUserModel.ProjectID = 4;// Convert.ToInt32(Session["ProjectID"]);//This will get from session

                objUserModel.ProjectID = (ObjLoginModel != null && ObjLoginModel.LocationID > 0) ? (ObjLoginModel.LocationID) : 0;
                Result result = _SuperAdminManager.SendInvitation(objUserModel);
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
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View();
        }

        public ActionResult ListVerifiedUser()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult _AssignProfile(string id, string Name, string UserEmail, string EmployeeCategoryid, string HiringDate)
        {
            try
            {
                UserModel _UserModel = new UserModel();
                ViewBag.EmployeeProfile = _ICommonMethod.GetGlobalCodeData(Convert.ToString(GlobalCodename.EmployeeProfile));

                _UserModel.FirstName = Name;
                _UserModel.UserEmail = UserEmail;
                if (!string.IsNullOrEmpty(id))
                {
                    _UserModel.UserId = Convert.ToInt32(Cryptography.GetDecryptedData(id, true));
                }
                else
                {
                    return PartialView("_AssignProfile", _UserModel);
                }


                long _EmployeeCategoryid = 0; if (Int64.TryParse(EmployeeCategoryid, out  _EmployeeCategoryid)) { _UserModel.EmployeeCategory = _EmployeeCategoryid; }
                if (string.IsNullOrEmpty(HiringDate)) { _UserModel.HiringDate = Convert.ToDateTime(HiringDate); }
                return PartialView("_AssignProfile", _UserModel);
            }
            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "_AssignProfile(string id, string Name, string UserEmail, string EmployeeCategoryid, string HiringDate)", "(ManagerController.cs)Excetion While edit any user from All User LIst menu", "id:-" + id);
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult _AssignProfilEmployee(UserModel _UserModel)
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

                _UserModel.ModifiedBy = ObjLoginModel.UserId;
                _UserModel.ModifiedDate = DateTime.UtcNow;
                Result result = _IManageManager.AssignProfile(_UserModel);
                if (result == Result.Completed)
                {
                    ViewBag.Message = CommonMessage.SaveSuccessMessage();
                    ModelState.Clear();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                }
                else if (result == Result.Failed)
                {
                    ViewBag.Message = CommonMessage.FailureMessage(); // store the message for successful in tempdata to display in view.
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //ViewBag.Project = _ICommonMethod.GetAllProject();
            //return PartialView("_AssignProject", _UserModel);
            return Json(new { Message = ViewBag.Message, AlertMessageClass = ObjAlertMessageClass.Success }, JsonRequestBehavior.AllowGet);
            //return Json(ViewBag.Message);
        }

        #endregion

        #region MyProfile
        /// <summary>
        /// CreatedBy   :   Gayatri Pal
        /// CreatedOn   :   Sep-06-2014
        /// CreatedFor  :   To view/Edit Profile "
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManagerProfile()
        {
            try
            {
                long userid = 0; ;
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

                userid = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;

                QRCModel _UserModel = new QRCModel();
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                if (userid > 0)
                {
                    long Totalrecord = 0;
                    _UserModel.UserModel = _IGlobalAdmin.GetManagerById(userid, "GetUserByID", null, null, null, null, null, out Totalrecord);
                    _UserModel.UpdateMode = true;
                }
                return View("ManagerProfile", _UserModel);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("ManagerProfile");
        }

        //This Method is use to edit his own profile
        [HttpPost]
        public ActionResult ManagerProfile(QRCModel ObjUserModel)
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
                    Result result = _IGlobalAdmin.SaveManager(ObjUserModel.UserModel, out QRCID, false);
                    if (result == Result.Completed)
                    {
                        ViewBag.Message = CommonMessage.SaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        ModelState.Clear();
                        return View("Index");
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
                        return View("Index");
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
            long Totalrecord = 0;
            ObjUserModel.UserModel = _IGlobalAdmin.GetManagerById(ObjUserModel.UserModel.UserId, "GetUserByID", null, null, null, null, null, out Totalrecord);
            return View("ManagerProfile", ObjUserModel);
        }
        #endregion

        #region Inventory
        [HttpGet]
        public ActionResult Inventory()
        {
            try
            {
                ViewBag.ItemTypeDrop = _ICommonMethod.GetGlobalCodeDataList(Convert.ToString(GlobalCodename.InventoryItem));
                ViewBag.UpdateMode = false;
                ViewBag.PurchType = _ICommonMethod.GetGlobalCodeDataList("ITEMOWNERSHIP");

                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult Inventory(InventoryMasterModel objInventoryMasterModel)
        {
            DARModel objDAR = null;
            try
            {
                eTracLoginModel ObjLoginModel = new eTracLoginModel();
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

                objDAR = new DARModel();
                objDAR.LocationId = ObjLoginModel.LocationID;
                objDAR.UserId = ObjLoginModel.UserId;
                objDAR.CreatedBy = ObjLoginModel.UserId;
                objDAR.CreatedOn = DateTime.UtcNow;


                if (objInventoryMasterModel.InventoryID == 0)
                {
                    objInventoryMasterModel.CreatedBy = ObjLoginModel.UserId;
                    objInventoryMasterModel.CreatedOn = DateTime.UtcNow;
                    objInventoryMasterModel.LocationId = ObjLoginModel.LocationID;
                    objInventoryMasterModel.Location = ObjLoginModel.Location;
                    objInventoryMasterModel.UserId = ObjLoginModel.UserId;
                    objInventoryMasterModel.UserName = ObjLoginModel.UserName;
                    ViewBag.UpdateMode = false;

                    objDAR.TaskType = (long)TaskTypeCategory.CreateInventory;
                    objDAR.ActivityDetails = DarMessage.SaveNewInventoryDar(ObjLoginModel.Location);
                }
                else
                {
                    objInventoryMasterModel.ModifiedBy = ObjLoginModel.UserId;
                    objInventoryMasterModel.ModifiedOn = DateTime.UtcNow;
                    objInventoryMasterModel.Location = ObjLoginModel.Location;
                    objInventoryMasterModel.UserId = ObjLoginModel.UserId;
                    objInventoryMasterModel.LocationId = ObjLoginModel.LocationID;
                    objInventoryMasterModel.UserName = ObjLoginModel.UserName;
                    ViewBag.UpdateMode = true;

                    objDAR.TaskType = (long)TaskTypeCategory.UpdateInventory;
                    objDAR.ActivityDetails = DarMessage.UpdateInventoryDar(ObjLoginModel.Location);

                }
                Result result = _IManageManager.SaveInventory(objInventoryMasterModel, objDAR);
                if (result == Result.Completed)
                {
                    ModelState.Clear();
                    ViewBag.Message = CommonMessage.SaveSuccessMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                }
                else if (result == Result.DuplicateRecord)
                {
                    ViewBag.Message = CommonMessage.DuplicateRecordInventory();
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
                ViewBag.ItemTypeDrop = _ICommonMethod.GetGlobalCodeDataList(Convert.ToString(GlobalCodename.InventoryItem));
                ViewBag.PurchType = _ICommonMethod.GetGlobalCodeDataList("ITEMOWNERSHIP");
                ModelState.Clear();
                return View("Inventory", new InventoryMasterModel());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult EditInventory(string id)
        {
            try
            {
                ViewBag.ItemTypeDrop = _ICommonMethod.GetGlobalCodeDataList("INVENTORYITEM");
                ViewBag.PurchType = _ICommonMethod.GetGlobalCodeDataList("ITEMOWNERSHIP");


                if (!string.IsNullOrEmpty(id))
                {
                    id = Cryptography.GetDecryptedData(id, true);
                    ViewBag.UpdateMode = true;
                    InventoryMasterModel _InventoryMasterModel = _IManageManager.EditInventory(Convert.ToInt64(id));
                    return View("Inventory", _InventoryMasterModel);
                }
                else
                {
                    ViewBag.AlertMessageClass = new AlertMessageClass().Danger;
                    ViewBag.Message = Result.DoesNotExist;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View("Inventory");
        }
        public ActionResult DeleteInventory(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    id = Cryptography.GetDecryptedData(id, true);

                    Result result = _IManageManager.DeleteInventory(Convert.ToInt32(id));
                    if (result == Result.Delete)
                    {
                        ViewBag.Message = CommonMessage.DeleteSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    }
                    else if (result == Result.DuplicateRecord)
                    {
                        ViewBag.Message = "Inventory is Assigned to employee Can't delete";
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListInventory(bool? pdf, int? ProjectID, int? pageIndex, int? numberOfRows, int? InventoryType, int? ItemOwn, string textSearch = null)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

                ViewBag.ProjectID = ObjLoginModel.LocationID;
                if (TempData["AssignedMessage"] != null)
                {
                    ViewBag.Message = CommonMessage.SuccessfullyAssigned();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                }
                //Added By Bhushan Dod On 02/11/2015
                //Description :- To generate pdf of grid data 
                if (pdf == null || ProjectID == null)
                {
                    return View();
                }
                else
                {
                    string sortColumnName = "ItemName";
                    //Commented by Bhushan Dod for Code Review
                    // string operationName = "GetAllInventory";
                    string sortOrderBy = "desc";
                    InventoryList = _ManageManager.GetAllInventory(ProjectID, "GetAllInventory", pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, InventoryType, ItemOwn, paramTotalRecords);
                    //  InventoryList = _ManageManager.GetAllInventoryExportToPDF(ProjectID,InventoryType,ItemOwn);

                    string fileName = ObjLoginModel.Location.Replace(" ", "-") + "_" + DateTime.Now.ToString("yyyy-MM-dd") + ".pdf";

                    string RootPath = Server.MapPath("~/");
                    string RootDirectory = Server.MapPath("~/ReportPDF/");//System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                    // string RootDirectory = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "ReportPDF/";
                    // RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + System.Configuration.ConfigurationManager.AppSettings.GetValues("JQGridPDF")[0];
                    if (!Directory.Exists(RootDirectory))
                    {
                        Directory.CreateDirectory(RootDirectory);
                    }
                    string filePath = RootDirectory + fileName;
                    if (InventoryType == 196)
                    {
                        CommonHelper.ExportPDF(InventoryList, new string[] { "InventoryID", "ItemCode", "ItemName", "Description", "ItemTypeName", "Quantity", "LocationId" }, filePath, InventoryType);
                    }
                    else
                    {
                        CommonHelper.ExportPDF(InventoryList, new string[] { "AssignInventoryID", "LocationId", "ItemName", "ItemCode", "ItemTypeName", "AssginedQuantity", "AssignedToName", "IssueDate", "ReturnDate" }, filePath, InventoryType);
                    }
                    string pdfPath = filePath;
                    WebClient client = new WebClient();
                    Byte[] buffer = client.DownloadData(pdfPath);
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);     // to open file prompt Box open or Save file        
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.WriteFile(pdfPath);
                    Response.End();
                    return null;
                    // return File(filePath, "application/pdf", fileName);
                }
                // return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ExportToExcel(int? ProjectID, int? pageIndex, int? numberOfRows, int? InventoryType, int? ItemOwn, string textSearch = null)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

                string sortColumnName = "ItemName";
                string operationName = "GetAllInventory";
                string sortOrderBy = "desc";
                string fileName = ObjLoginModel.Location.Replace(" ", "-") + "_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls";
                string imageURL = ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png";
                InventoryList = _ManageManager.GetAllInventory(ProjectID, "GetAllInventory", pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, InventoryType, ItemOwn, paramTotalRecords);
                var grid = new GridView();
                if (InventoryType == 196)
                {

                    grid.DataSource = from p in InventoryList
                                      select new
                                      {
                                          InventoryID = p.InventoryID,
                                          ItemCode = p.ItemCode,
                                          ItemName = p.ItemName,
                                          Description = p.Description,
                                          ItemTypeName = p.ItemTypeName,
                                          Quantity = p.Quantity,
                                          LocationId = p.LocationId
                                      };
                }
                else
                {
                    grid.DataSource = from p in InventoryList
                                      select new
                                      {
                                          AssignInventoryID = p.AssignInventoryID,
                                          LocationId = p.LocationId,
                                          ItemName = p.ItemName,
                                          ItemCode = p.ItemCode,
                                          ItemTypeName = p.ItemTypeName,
                                          AssginedQuantity = p.AssginedQuantity,
                                          AssignedToName = p.AssignedToName,
                                          IssueDate = p.IssueDate,
                                          ReturnDate = p.ReturnDate
                                      };
                }
                grid.DataBind();
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                Response.ContentType = "application/excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grid.RenderControl(htw);
                //string headerTable = @"<Table><tr><td><img src="+imageURL+"></img></td></tr></Table>";
                //Response.Write(headerTable);
                Response.Write(sw.ToString());
                Response.End();

                return View("ListInventory");
                //return PartialView("ListInventory");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public ActionResult ListAssignedInventory(bool? pdf, int? ProjectID)
        //{
        //    //Bool pdf;
        //    try
        //    {
        //        eTracLoginModel ObjLoginModel = null;
        //        if (Session["eTrac"] != null)
        //        { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

        //        ViewBag.ProjectID = ObjLoginModel.LocationID;
        //        if (TempData["AssignedMessage"] != null)
        //        {
        //            ViewBag.Message = CommonMessage.SuccessfullyAssigned();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
        //        }
        //        //Added By Bhushan Dod On 02/11/2015
        //        //Description :- To generate pdf of grid data 
        //        if (pdf == null || ProjectID == null)
        //        {
        //            return View();
        //        }
        //        else
        //        {
        //            //InventoryList = _ManageManager.GetAllInventoryExportToPDF(ProjectID);
        //            string fileName = ObjLoginModel.Location + "_" + ObjLoginModel.FName + " " + ObjLoginModel.LName + "_" + DateTime.Now.ToString("yyyy/MM/dd") + ".pdf";
        //            string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
        //            RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + System.Configuration.ConfigurationManager.AppSettings.GetValues("JQGridPDF")[0];
        //            if (!Directory.Exists(RootDirectory))
        //            {
        //                Directory.CreateDirectory(RootDirectory);
        //            }
        //            string filePath = RootDirectory + fileName;
        //            // string filePath = Server.MapPath("Content") + "Sample.pdf";
        //            CommonHelper.ExportPDF(InventoryList, new string[] { "InventoryID", 
        //                                                                 "ItemName", "ItemCode", "ItemTypeName",
        //                                                                 "Description", "Quantity", "CreatedDate"}, filePath);

        //            return File(filePath, "application/pdf", fileName);
        //        }
        //        // return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        [HttpGet]
        public ActionResult AssignInventory()
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

                ViewBag.Inventory = _ICommonMethod.GetAllInventoryByProjectId(ObjLoginModel.LocationID);
                ViewBag.AssignedUser = _ICommonMethod.GetEmployeeProject(ObjLoginModel.LocationID);
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult _AssignInventory(string id, int Quantity)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                AssignInventoryModel objAssignInventoryModel = new AssignInventoryModel();
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

                if (!string.IsNullOrEmpty(id))
                {
                    id = Cryptography.GetDecryptedData(id, true);
                }
                objAssignInventoryModel.IssueDate = DateTime.UtcNow.ToString("MM/dd/yyyy");
                objAssignInventoryModel.InventoryID = Convert.ToInt32(id);
                objAssignInventoryModel.RemainingQuantity = Convert.ToInt32(Quantity);
                ViewBag.Inventory = _ICommonMethod.GetAllInventoryByProjectId(ObjLoginModel.LocationID);
                ViewBag.AssignedUser = _ICommonMethod.GetEmployeeProject(ObjLoginModel.LocationID);
                objAssignInventoryModel.Quantity = 0;
                return PartialView("_AssignInventory", objAssignInventoryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult SaveAssignInventory(AssignInventoryModel AssignInventoryModel)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
            try
            {
                AssignInventoryModel.CreatedBy = ObjLoginModel.UserId;
                AssignInventoryModel.CreatedOn = Convert.ToDateTime(DateTime.UtcNow);
                AssignInventoryModel.IsDeleted = false;
                AssignInventoryModel.IssuedBy = ObjLoginModel.UserId;
                AssignInventoryModel.Location = ObjLoginModel.Location;
                AssignInventoryModel.UserId = ObjLoginModel.UserId;
                AssignInventoryModel.LocationId = ObjLoginModel.LocationID;
                AssignInventoryModel.UserName = ObjLoginModel.UserName;

                Result result = _IManageManager.AssignedInventory(AssignInventoryModel);
                if (result == Result.Completed)
                {
                    ViewBag.Message = CommonMessage.SaveSuccessMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    //Added By Bhushan Dod on 08/02/2015 For Label seen to view.
                    TempData["AssignedMessage"] = "Success";
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

                // ViewBag.ItemType = _ICommonMethod.GetGlobalCodeDataList(Convert.ToString(GolbalCodeName.INVENTORYITEM));
                //return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
                //return View("ListInventory", AssignInventoryModel);
                //return RedirectToAction("ListInventory", "Manager");
                return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Work Request
        [HttpGet]
        public ActionResult WorkRequest()
        {
            try
            {
                WorkRequestModel WorkRequestModel = new WorkRequestModel();
                ViewBag.TaskType = _ICommonMethod.GetGlobalCodeDataList(Convert.ToString(GlobalCodename.TaskType));
                ViewBag.TaskPriority = _ICommonMethod.GetGlobalCodeDataList(Convert.ToString(GlobalCodename.WorkPriority));
                ViewBag.WorkArea = _ICommonMethod.GetWorkArea();

                return View("WorkRequest", WorkRequestModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;

        //        }
        //        else if (result == Result.DuplicateRecord)
        //        {
        //            ViewBag.Message = CommonMessage.DuplicateRecordMessage();
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
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        [HttpGet]
        public ActionResult ListWorkRequest()
        {
            try
            {

                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

                ViewBag.ProjectID = (ObjLoginModel != null && ObjLoginModel.LocationID > 0) ? (ObjLoginModel.LocationID) : 0;
                // ViewBag.UserId = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // [HttpGet]
        public ActionResult WorkOrder(string id, string TaskName, string TaskPriority, string RequestBy, string WorkArea, string StartTime, string CompletionTime,
                                        string AssignedToUser, string ProjectId, string Remarks, string WorkOrderID, string AssetID)
        {
            try
            {
                WorkOrderModel _WorkOrderModel = new WorkOrderModel();

                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

                long ProjectID = (ObjLoginModel != null && ObjLoginModel.LocationID > 0) ? (ObjLoginModel.LocationID) : 0;
                ViewBag.TaskType = _ICommonMethod.GetGlobalCodeDataList(Convert.ToString(GlobalCodename.TaskType));
                ViewBag.TaskPriority = _ICommonMethod.GetGlobalCodeDataList(Convert.ToString(GlobalCodename.WorkPriority));
                ViewBag.WorkArea = _ICommonMethod.GetWorkArea();
                ViewBag.AssignedToUser = _ICommonMethod.GetEmployeeProject(ProjectID);

                if (!string.IsNullOrEmpty(id))
                {
                    id = Cryptography.GetDecryptedData(id, true);
                    _WorkOrderModel.WorkRequestID = Convert.ToInt32(id);
                }

                _WorkOrderModel.TaskName = TaskName;
                _WorkOrderModel.TaskPriority = Convert.ToInt32(TaskPriority);
                _WorkOrderModel.WorkArea = Convert.ToInt32(WorkArea);
                _WorkOrderModel.StartTime = StartTime;
                _WorkOrderModel.CompletionTime = CompletionTime;
                _WorkOrderModel.AssignedToUser = Convert.ToInt32(AssignedToUser);
                _WorkOrderModel.ProjectId = Convert.ToInt32(ProjectId);
                _WorkOrderModel.Remarks = Remarks;
                _WorkOrderModel.WorkOrderID = Convert.ToInt32(WorkOrderID);
                _WorkOrderModel.AssetID = Convert.ToInt32(AssetID);
                //_WorkOrderModel.TaskType = Convert.ToInt32(TaskType);
                return PartialView("_WorkOrder", _WorkOrderModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //[HttpPost]
        //public ActionResult SaveWorkOrder(WorkOrderModel objWorkOrderModel)
        //{
        //    try
        //    {
        //        eTracLoginModel ObjLoginModel = null;
        //        if (Session["eTrac"] != null)
        //        { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

        //        long ProjectID = (ObjLoginModel != null && ObjLoginModel.LocationID > 0) ? (ObjLoginModel.LocationID) : 0;

        //        if (objWorkOrderModel.WorkOrderID == 0)
        //        {
        //            objWorkOrderModel.IsDeleted = false;
        //            objWorkOrderModel.CreatedBy = 1;
        //            objWorkOrderModel.CreatedDate = DateTime.Now;
        //            objWorkOrderModel.WorkRequestID = objWorkOrderModel.WorkRequestID;
        //            Result result = _IManageManager.SaveWorkOrder(objWorkOrderModel);
        //            if (result == Result.Completed)
        //            {
        //                ViewBag.Message = CommonMessage.SaveSuccessMessage();
        //                ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;

        //            }
        //            else if (result == Result.DuplicateRecord)
        //            {
        //                ViewBag.Message = CommonMessage.DuplicateRecordEmailIdMessage();
        //                ViewBag.AlertMessageClass = ObjAlertMessageClass.Info; // store the message for successful in tempdata to display in view.
        //            }
        //            else if (result == Result.UpdatedSuccessfully)
        //            {
        //                ViewBag.Message = CommonMessage.UpdateSuccessMessage();
        //                ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;// store the message for successful in tempdata to display in view.
        //            }
        //            else
        //            {
        //                ViewBag.Message = CommonMessage.FailureMessage();
        //                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
        //            }

        //        }

        //        ViewBag.TaskType = _ICommonMethod.GetGlobalCodeDataList(Convert.ToString(GolbalCodeName.TASKTYPE));
        //        ViewBag.TaskPriority = _ICommonMethod.GetGlobalCodeDataList(Convert.ToString(GolbalCodeName.WORKPRIORITY));
        //        ViewBag.WorkArea = _ICommonMethod.GetWorkArea();
        //        ViewBag.AssignedToUser = _ICommonMethod.GetEmployeeProject(ProjectID);
        //        return Json(new { Message = ViewBag.Message, AlertMessageClass = ObjAlertMessageClass.Success }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

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
                return PartialView("_WorkOrderDetails", _WorkRequestModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public ActionResult DeleteWorkRequest(string id)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(id))
        //        {
        //            id = Cryptography.GetDecryptedData(id, true);
        //        }
        //        Result result = _IManageManager.DeleteWorkRequest(Convert.ToInt32(id));
        //        if (result == Result.Delete)
        //        {
        //            ViewBag.Message = CommonMessage.DeleteSuccessMessage();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
        //        }
        //        else if (result == Result.Failed)
        //        {
        //            ViewBag.Message = CommonMessage.FailureMessage();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
        //        }
        //        return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        /// <summary>
        /// CreatedBy   :   Nagendra Upwanshi
        /// CreatedOn   :   Oct-31-2014
        /// CreatedFor  :   Create Client
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ActionResult Client(string usr)
        {
            try
            { ViewBag.Country = _ICommonMethod.GetAllcountries(); return View("Client", _ICommonMethod.LoadInvitedUser(usr)); }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }

        #region Invite Client
        [HttpGet]
        public ActionResult InviteClient()
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
        [HttpPost]
        public ActionResult InviteClient(UserModel objUserModel)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

                objUserModel.CreatedBy = ObjLoginModel.UserId;
                objUserModel.CreatedDate = DateTime.UtcNow;
                objUserModel.IsDeleted = false;

                var _usertype = _ICommonMethod.GetGlobalCodeForName("USERTYPE", "Client");
                if (_usertype > 0) { objUserModel.UserType = _usertype; }

                objUserModel.ProjectID = ObjLoginModel.LocationID;
                //Result result = _IGlobalAdmin.SendInvitation(objUserModel, Convert.ToString(UserType.Client));
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
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View();
        }
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

        #region Vendor
        #region comments
        /*
        Hello Amit,
            Meeting for Project execution flow done with client 'Dane Grey' On Sep-15-2014 and Sep-17-2014.
            Client suggest us to change login screen design, and asked us to wait for 2 days for the flow of "Work Order", he would share with us.

         * Modification in Project architecture are waiting for client approval on the MOM,because these modification should be based on disscussion done with Client.
            Design Chages in Login Screen done and also need approval from client.
         
         
         We did not find any response form Client for the MOM
         A reminder already sent to client to please send us feedback or approval for MOM task disscussed in above Meeting and design cahnges of Login Screen.
         Also we informed to Amit form BDG team to update client for the same.
         
         Please suggest us, Because delay in client repsonse hamperring our development.
         We require client approval to move further in our development.

        */

        #endregion comments

        #endregion vendor

        #region Rule
        [HttpGet]
        public ActionResult ManageRule()
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

        [HttpPost]
        public ActionResult ManageRule(RuleMasterModel _RuleMasterModel)
        {
            DARModel objDAR = null;
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

                objDAR = new DARModel();
                objDAR.LocationId = ObjLoginModel.LocationID;
                objDAR.UserId = ObjLoginModel.UserId;
                objDAR.CreatedBy = ObjLoginModel.UserId;
                objDAR.CreatedOn = DateTime.UtcNow;

                if (_RuleMasterModel.RuleID == 0)
                {
                    _RuleMasterModel.CreatedBy = ObjLoginModel.UserId;
                    _RuleMasterModel.CreatedDate = DateTime.UtcNow;
                    _RuleMasterModel.IsDeleted = false;
                    _RuleMasterModel.ProjectID = ObjLoginModel.LocationID;
                    ViewBag.UpdateMode = false;
                    objDAR.TaskType = (long)TaskTypeCategory.CreateRule;
                    objDAR.ActivityDetails = DarMessage.SaveNewRuleDar(_RuleMasterModel.RuleName, ObjLoginModel.Location);
                }
                else
                {
                    _RuleMasterModel.ModifiedBy = ObjLoginModel.UserId;
                    _RuleMasterModel.ModifiedDate = DateTime.UtcNow;
                    _RuleMasterModel.IsDeleted = false;
                    _RuleMasterModel.ProjectID = ObjLoginModel.LocationID;
                    ViewBag.UpdateMode = true;
                    objDAR.TaskType = (long)TaskTypeCategory.UpdateRule;
                    objDAR.ActivityDetails = DarMessage.UpdateRuleDar(_RuleMasterModel.RuleName, ObjLoginModel.Location);
                }
                Result result = _IManageManager.SaveRule(_RuleMasterModel, objDAR);
                if (result == Result.Completed)
                {
                    ViewBag.Message = CommonMessage.SaveSuccessMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    ModelState.Clear();
                    _RuleMasterModel = null;
                }
                else if (result == Result.DuplicateRecord)
                {
                    ViewBag.Message = CommonMessage.DuplicateRecordMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Info; // store the message for successful in tempdata to display in view.
                }
                else if (result == Result.UpdatedSuccessfully)
                {
                    ViewBag.Message = CommonMessage.UpdateSuccessMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;// store the message for successful in tempdata to display in view.
                    ModelState.Clear();
                    _RuleMasterModel = null;
                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                }
                ViewBag.ItemType = _ICommonMethod.GetGlobalCodeDataList(Convert.ToString(GlobalCodename.InventoryItem));
                return View("ManageRule", _RuleMasterModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult ListRule()
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

                ViewBag.ProjectID = ObjLoginModel.LocationID;
                return View();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult EditRule(string id)
        {
            long ruleId = 0;
            eTracLoginModel ObjLoginModel = null;
            RuleMasterModel objRuleMasterModel = null;

            try
            {
                objRuleMasterModel = new RuleMasterModel();

                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);

                    if (!string.IsNullOrEmpty(id))
                    {
                        ViewBag.UpdateMode = true;
                        id = Cryptography.GetDecryptedData(id, true);
                        ruleId = Convert.ToInt64(id);

                        objRuleMasterModel = _IManageManager.EditRule(ruleId, ObjLoginModel.LocationID);
                        ViewBag.UpdateMode = true;
                    }
                }
                return View("ManageRule", objRuleMasterModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult DeleteRule(string id)
        {
            DARModel objDAR;
            try
            {
                eTracLoginModel ObjLoginModel = null; long LoggedInUser = 0, ruleId = 0;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
                LoggedInUser = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;
                if (!string.IsNullOrEmpty(id))
                {
                    id = Cryptography.GetDecryptedData(id, true);
                    ruleId = Convert.ToInt64(id);

                    objDAR = new DARModel();
                    objDAR.LocationId = ObjLoginModel.LocationID;
                    objDAR.UserId = ObjLoginModel.UserId;
                    objDAR.CreatedBy = ObjLoginModel.UserId;
                    objDAR.CreatedOn = DateTime.UtcNow;
                    objDAR.TaskType = (long)TaskTypeCategory.DeleteRule;

                    Result result = _IManageManager.DeleteRule(ruleId, LoggedInUser, ObjLoginModel.Location, objDAR);
                    if (result == Result.Delete)
                    {
                        ViewBag.Message = CommonMessage.DeleteSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    }
                    else if (result == Result.Failed)
                    {
                        ViewBag.Message = "Can't Delete Vendor";
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Manage Asset
        //[HttpGet]
        //public ActionResult ManageAsset()
        //{
        //    try
        //    {
        //        ViewBag.AssetClass = _ICommonMethod.GetGlobalCodeData(Convert.ToString(GolbalCodeName.ASSETCLASS));
        //        ViewBag.WorkArea = _ICommonMethod.GetWorkArea();
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //[HttpPost]
        //public ActionResult ManageAsset(AssetMasterModel objAssetMasterModel)
        //{
        //    try
        //    {
        //        eTracLoginModel ObjLoginModel = null;
        //        if (Session["eTrac"] != null)
        //        { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

        //        if (objAssetMasterModel.AssetMasterID == 0)
        //        {
        //            objAssetMasterModel.CreatedBy = ObjLoginModel.UserId;
        //            objAssetMasterModel.CreatedOn = DateTime.Now;
        //            objAssetMasterModel.IsDeleted = false;
        //            objAssetMasterModel.ProjectID = ObjLoginModel.LocationID;
        //        }
        //        if (Session["ImageName"] != null)
        //        {
        //            objAssetMasterModel.AssetPhoto = Convert.ToString(Session["ImageName"]);
        //        }
        //        Result result = _IManageManager.SaveAsset(objAssetMasterModel);
        //        if (result == Result.Completed)
        //        {
        //            ViewBag.Message = CommonMessage.SaveSuccessMessage();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
        //            ModelState.Clear();
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
        //        ViewBag.AssetClass = _ICommonMethod.GetGlobalCodeData(Convert.ToString(GolbalCodeName.ASSETCLASS));
        //        ViewBag.WorkArea = _ICommonMethod.GetWorkArea();
        //        return View("ManageAsset", objAssetMasterModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        #endregion

        #region UserList
        /// <summary>UserList
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-05-2014</CreatedOn>
        /// <CreatedFor>List all user with completed details</CreatedFor>
        /// </summary>
        /// <returns></returns>
        public ActionResult UserList()
        {
            try
            { return View("UserList"); }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion UserList

        /// <summary>Manager Dashboard
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedOn>Feb-26-2015</CreatedOn>
        /// <CreatedFor>Dashboard for Manager</CreatedFor>
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        public ActionResult Dashboard()
        {
            try
            {
                //long LocationID = 0, UserID = 0;
                //eTracLoginModel obj_eTracLoginModel = new eTracLoginModel();
                //if (Session["eTrac"] != null)
                //{

                //    obj_eTracLoginModel = (eTracLoginModel)Session["eTrac"];
                //    UserID = obj_eTracLoginModel.UserId;
                //    LocationID = Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]);
                //}
                //ObjectParameter obj_ObjectParameter = new ObjectParameter("TotalRecords", typeof(int));
                //ObjectParameter obj_ObjectParameter2 = new ObjectParameter("TotalRecords", typeof(int));
                //GlobalAdminManager obj_GlobalAdminManager = new GlobalAdminManager();
                //ViewBag.AllWorkOrder = obj_GlobalAdminManager.GetAllWorkRequestAssignment(0, 0, "GetAllWorkRequestAssignment", 1, 100000000, "WorkRequestAssignmentID", "asc", "", LocationID, obj_eTracLoginModel.UserId, DateTime.Now, DateTime.Now, "", obj_ObjectParameter).Count();
                //ViewBag.AllAssignedWorkOrder = obj_GlobalAdminManager.GetAllWorkRequestAssignment(0, 0, "GetAllAssignedWorkRequest", 1, 100000000, "WorkRequestAssignmentID", "asc", "", LocationID, obj_eTracLoginModel.UserId, DateTime.Now, DateTime.Now, "", obj_ObjectParameter2).Count();
                //ViewBag.EmployeeCount = _IManageManager.GetTotalManagerCount("Employee", LocationID, UserID);
                //ViewBag.ClientCount = _IManageManager.GetTotalManagerCount("Client", LocationID, UserID);
                //Tuple<decimal, decimal> objT = _IGlobalAdmin.EcashDataForDashBoard(LocationID);

                //ViewBag.EcashData = objT.Item1;
                //ViewBag.EcashDataWeek = objT.Item2;
                return View();
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
            // Commented by vijay sahu
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
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                var _UserModel = _ICommonMethod.LoadInvitedUser(usr);
                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
                /*                
                System.Data.Entity.Core.Objects.ObjectParameter paramTotalRecords = new System.Data.Entity.Core.Objects.ObjectParameter("TotalRecords", typeof(int));
                _UserModel.JobTitleList = _ICommonMethod.GetGlobalCodeData("JOBTITLE");
                _UserModel.LocationList = _IGlobalAdmin.GetAllLocationList(0, "GetAllLocation", 1, 10000, "LocationName", "desc", "", paramTotalRecords);
                paramTotalRecords = null;
                */
                return View("Manager", _UserModel);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error");
            }
            // Commented by vijay sahu
        }

        /// <summary>Create
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-18-2014</CreatedOn>
        /// Modified by vijay sahu on 30 may 2015 add email send function
        /// <CreatedFor>POST method for Create New User</CreatedFor>
        /// </summary>
        /// <param name="ObjUserModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(QRCModel ObjUserModel)
        {
            DARModel objDAR = null;
            long LocId = 0;
            try
            {
                eTracLoginModel ObjLoginModel = null;

                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
                if (Session["eTrac_SelectedDasboardLocationID"] != null)
                {
                    LocId = (long)Session["eTrac_SelectedDasboardLocationID"];
                }
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
                        ObjUserModel.UserModel.Password = Cryptography.GetEncryptedData(ObjUserModel.UserModel.Password, true);
                        //ObjUserModel.UserModel.Password = _ICommonMethod.CreateRandomPassword();

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

                        objDAR.TaskType = (long)TaskTypeCategory.UserCreation;
                        objDAR.ActivityDetails = DarMessage.NewManagerCreatedDar(ObjLoginModel.Location);

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
                        objDAR.ActivityDetails = DarMessage.ManagerUpdatedDar(ObjLoginModel.Location);
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
                        objEmailHelper.UserName = ObjUserModel.UserModel.AlternateEmail;
                        objEmailHelper.UserType = ObjUserModel.UserModel.UserType;
                        objEmailHelper.FirstName = ObjUserModel.UserModel.FirstName;
                        objEmailHelper.LastName = ObjUserModel.UserModel.LastName;
                        objEmailHelper.Password = Cryptography.GetDecryptedData(ObjUserModel.UserModel.Password, true);
                        objEmailHelper.LocationName = abc.LocationName;
                        objEmailHelper.LocAddress = ObjUserModel.UserModel.Address.Address1; // here locAddress means user Address
                        objEmailHelper.MailType = "CreateNewUser";

                        HostingPrefix = HostingPrefix + "Manager/Employee?usr=" + Cryptography.GetEncryptedData(ObjUserModel.UserModel.UserId.ToString(), true);

                        objEmailHelper.RegistrationLink = HostingPrefix;

                        objEmailHelper.SendEmailWithTemplate();

                        ViewBag.Message = CommonMessage.SaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        ModelState.Clear();
                        ObjUserModel = _ICommonMethod.LoadInvitedUser(string.Empty);
                        //return View("ITAdministrator");//return RedirectToAction("Create ", "GlobalAdmin");
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
                        ObjUserModel = _ICommonMethod.LoadInvitedUser(string.Empty);
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
                /*
                System.Data.Entity.Core.Objects.ObjectParameter paramTotalRecords = new System.Data.Entity.Core.Objects.ObjectParameter("TotalRecords", typeof(int));
                ObjUserModel.JobTitleList = _ICommonMethod.GetGlobalCodeData("JOBTITLE");
                ObjUserModel.LocationList = _IGlobalAdmin.GetAllLocationList(0, "GetAllLocation", 1, 10000, "LocationName", "desc", "", paramTotalRecords);
                paramTotalRecords = null;
                */
            }
            ViewBag.UpdateMode = false;
            //ObjUserModel.UserModel = _IClientManager.GetClientByID(ObjUserModel.UserModel.UserId, "GetUserByID", null, null, null, null, null);
            return View("Manager", ObjUserModel);
        }

        /// <summary>
        /// To Get Employee List under Manager
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>2105-03-12</CreatedDate>
        /// <param name="LoginUserType"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetUserByManager(string LoginUserType)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                long LocationID = 0;
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    LocationID = Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]);
                    return Json(_IManageManager.GetUserByManager(LoginUserType, LocationID, ObjLoginModel.UserId));
                }
                else { return Json("Session Expired"); }
            }
            catch (Exception ex)
            { return Json(ex.Message); }

        }
        private byte[] GetFile(string s)
        {
            byte[] data;
            using (System.IO.FileStream fs = System.IO.File.OpenRead(s))
            {
                data = new byte[fs.Length];
                int br = fs.Read(data, 0, data.Length);
                if (br != fs.Length)
                    throw new System.IO.IOException(s);
            }
            return data;
        }

        /// <summary>
        /// Created By : Bhushan Dod
        /// Created Date: 26/05/2015
        /// Description : Fetched record and check Is employee idle
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> EmployeeIdleStatusAlertManager(string id)
        {


            eTracLoginModel ObjLoginModel = null;

            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                var re = await _IManageManager.IdleEmployeeAlert(ObjLoginModel.UserId);

                return Json(new { re }, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>
        /// Created By : Bhushan Dod
        /// Created Date: 26/05/2015
        /// Description : Insert record if employee is Idle
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult EmployeeIdleStatus(long id)
        {
            eTracLoginModel ObjLoginModel = null;

            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                var re = _IManageManager.EmployeeIdleStatus(id, ObjLoginModel.UserId);

                return Json(new { re }, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>
        /// Created By :Bhushan Dod
        /// Created Date : 01/06/2015
        /// Description : For change the idle time limit by Manager
        /// </summary>
        /// <param name="id"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ExtendEmployeeTimeLimit(string id, string time)
        {
            try
            {
                eTracLoginModel ObjLoginModel = new eTracLoginModel();

                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    var re = _IManageManager.UpdateEmployeeIdleTime(id, time, ObjLoginModel.UserId);
                    return Json(new { re }, JsonRequestBehavior.AllowGet);
                }

                else { return Json(null, JsonRequestBehavior.AllowGet); }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult GetRefreshedEmployeeIdleLimit(long id)
        {
            eTracLoginModel ObjLoginModel = null;

            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                var re = _IManageManager.GetUpdatedEmployeeTimeLimit(ObjLoginModel.UserId);

                return Json(new { re }, JsonRequestBehavior.AllowGet);
            }

            else { return Json(null, JsonRequestBehavior.AllowGet); }

        }

        [HttpGet]
        public JsonResult FacilityRequest30SecPushNotificaiton(string id)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                var re = _IManageManager.FacRequestNotAcceptPushNotificaiton(ObjLoginModel.UserId);

                return Json(new { re }, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }
        /// <summary>
        /// Created By : Bhushan Dod
        /// Created Date : 07/22/2015
        /// Description : Send Email to manager if Qrc Expiration is today(DateTime.Now)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SendQrcExpirationMail(long id)
        {
            bool status = false;
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                objLoginSession = (eTracLoginModel)Session["eTrac"];
                status = _IManageManager.EmailToMangerForQrcExpiration(Convert.ToInt64(id), objLoginSession.UserId, objLoginSession.UserRoleId);
                return Json(new { status }, JsonRequestBehavior.AllowGet);
            }
            else { return Json(status, JsonRequestBehavior.AllowGet); }
        }
    }
}