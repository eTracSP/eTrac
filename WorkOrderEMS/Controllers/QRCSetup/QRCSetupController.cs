using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers.QRCSetup
{

    [Authorize]
    public class QRCSetupController : Controller
    {

        // private string path = "~/Content/Images/LocationLogo/";

        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        private readonly IQRCSetup _IQRCSetup;
        private readonly IManageManager _IManager;
        private readonly ICommonMethod _ICommonMethod;
        private readonly IGlobalAdmin _IGlobalAdmin;
        private readonly IUser _IUser;
        private readonly IWorkRequestAssignment _IWorkRequestAssignment;
        private readonly IClientManager _IClientManager;

        PrintQRCModel ObjPrintQRCModel;
        private string path = ConfigurationManager.AppSettings["ProjectLogoPath"];
        private string globalPath = System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"];

        private string W9FormPath = ConfigurationManager.AppSettings["W9FormPath"];

        #region old working code

        public QRCSetupController(IQRCSetup _IQRCSetup, ICommonMethod _ICommonMethod, IManageManager _IManager, IGlobalAdmin _IGlobalAdmin, IUser _IUser)
        {
            this._IQRCSetup = _IQRCSetup;
            this._ICommonMethod = _ICommonMethod;
            this._IManager = _IManager;
            this._IGlobalAdmin = _IGlobalAdmin;
            this._IUser = _IUser;
        }

        /// <ModifiedBY>Bhushan</ModifiedBY>
        /// <ModifiedOn>Oct-12-2015</ModifiedOn>
        /// <ModifiedFor>List code</ModifiedFor>
        /// </summary>For Code Review Process
        /// <param name="qr"></param>
        /// <returns></returns>
        [NonAction]
        private QRCModel QRCInIt()
        {
            QRCModel ObjQRCModel = new QRCModel();
            try
            {
                ObjQRCModel = _IQRCSetup.GetGlobalCodeForCategories();
                ObjQRCModel.UserModel = new UserModel();
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                //ObjQRCModel.VendorID = _IManager.GetAllVendorList(null, null, null, "", null, "");
                ObjQRCModel.UpdateMode = false;
                //ObjQRCModel.QRCTYPE = ObjQRCModel.QRCTypeList.SingleOrDefault(q => q.CodeName == "Vehicle" && q.IsDeleted == false).GlobalCodeId;
            }
            catch (Exception ex) { throw ex; }
            return ObjQRCModel;
        }


        #endregion old working code

        #region new section of code

        #endregion new section of code

        public ActionResult Index()
        {
            long Totalrecords = 0;
            LocationMasterModel objLocationMasterModel = null;
            try
            {
                var data = QRCInIt();
                //ViewBag.EncryptLastQRC = data.EncryptLastQRC;
                eTracLoginModel ObjLogin = (eTracLoginModel)Session["eTrac"];
                objLocationMasterModel = _ICommonMethod.GetLocationDetailsById(ObjLogin.LocationID);
                ViewBag.EncryptLastQRC = data.EncryptLastQRC + "," + (string.IsNullOrEmpty(objLocationMasterModel.Address2) ? ObjLogin.Location.ToString().Substring(0, 3).ToUpper() : objLocationMasterModel.Address2.ToString().Substring(0, 3).ToUpper());
                ViewBag.ItemAbberivationList = Convert.ToString(ConfigurationManager.AppSettings["ItemAbberivationList"]);
                ViewBag.PurchType = _ICommonMethod.GetGlobalCodeDataList("PURCHASETYPE");
                ViewBag.RefreshMode = false;
                if (ObjLogin != null && (ObjLogin.UserRoleId == Convert.ToInt64(UserType.GlobalAdmin) || ObjLogin.UserRoleId == Convert.ToInt64(UserType.ITAdministrator)))
                {
                    //ViewBag.ManagerList = _IGlobalAdmin.GetAllITAdministratorList(0, 1, 1000, "UserEmail", "asc", "", Convert.ToInt64(UserType.Manager), out Totalrecords);
                    ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
                    ViewBag.AdministratorList = _IGlobalAdmin.GetAllITAdministratorList(0, 0, 1, 1000, "UserEmail", "asc", "", (UserType.Administrator).ToString(), out Totalrecords);
                }
                else if (ObjLogin != null && (ObjLogin.UserRoleId == Convert.ToInt64(UserType.Administrator)))
                {
                    ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
                    // ViewBag.AdministratorList = _IGlobalAdmin.GetAllITAdministratorList(0, 1, 1000, "UserEmail", "asc", "", Convert.ToInt64(UserType.Manager), out Totalrecords);

                    ViewBag.AdministratorList = _ICommonMethod.GetManagersBYLocationId(ObjLogin.LocationID);

                }
                //if (!string.IsNullOrEmpty(successCode) && successCode == "Success") 
                //{
                //    ModelState.AddModelError("", "Record Saved Sucessfully.");
                //}
                return View(data);
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(QRCModel ObjQRCModel)
        {
            DARModel objDAR;
            long Totalrecords = 0;
            eTracLoginModel ObjLoginModel = new eTracLoginModel();
            LocationMasterModel objLocationMasterModel = null;
            try
            {
                //if (ModelState.IsValid)
                if (true)
                {
                    #region login model
                    ////
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);

                        // ObjQRCModel.UserModel.ProjectID = (ObjLoginModel != null && ObjLoginModel.LocationID > 0) ? (ObjLoginModel.LocationID) : 0;
                    }
                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }
                    ///
                    #endregion login model

                    long _qRCId = 0;
                    Result _fnResult, result;
                    PrintQRCModel ObjPrintQRCModel;
                    CommonHelper ObjCommonHelper = new CommonHelper();
                    string QRCImageName = string.Empty;
                    //string msg;
                    if (Session["ImageName"] != null)
                    { ObjQRCModel.UserModel.myProfileImage = Convert.ToString(Session["ImageName"]); }

                    if (ObjQRCModel.WarrantyDocument != null)
                        ObjQRCModel.WarrantyDoc = DateTime.Now.Ticks + "_" + ObjQRCModel.WarrantyDocument.FileName.Replace(" ", "");
                    if (ObjQRCModel.LOCPicture != null)
                        ObjQRCModel.LocationPicture = DateTime.Now.Ticks + "_" + ObjQRCModel.LOCPicture.FileName.Replace(" ", "");
                    if (ObjQRCModel.AssetPictureUrl != null)
                        ObjQRCModel.AssetPicture = DateTime.Now.Ticks + "_" + ObjQRCModel.AssetPictureUrl.FileName.Replace(" ", "");

                    ObjQRCModel.CreatedBy = ObjLoginModel.UserId;
                    ObjQRCModel.LocationId = ObjLoginModel.LocationID;

                    objDAR = new DARModel();
                    objDAR.LocationId = ObjLoginModel.LocationID;
                    objDAR.UserId = ObjLoginModel.UserId;
                    objDAR.CreatedBy = ObjLoginModel.UserId;
                    objDAR.CreatedOn = DateTime.UtcNow;

                    //Added by Bhushan on 26/Oct/2016 for client has changed request for If QRC is vehicle then directly set vehicle type is Motor Vehicle and for Shuttle Bus  set vehicle type is Shuttle Bus
                    if (ObjQRCModel.QRCTYPE == Convert.ToInt64(QrcType.Vehicle))
                    {
                        ObjQRCModel.VehicleType = Convert.ToInt64(VEHICLETYPE.MotorVehicle);
                    }
                    if (ObjQRCModel.QRCTYPE == Convert.ToInt64(QrcType.ShuttleBus))
                    {
                        ObjQRCModel.VehicleType = Convert.ToInt64(VEHICLETYPE.ShuttleBus);
                    }

                    if (_IQRCSetup.ProcessQRCSetup(ObjQRCModel, out _qRCId, out _fnResult, out ObjPrintQRCModel))
                    {

                        path = Server.MapPath(path);

                        if (ObjQRCModel.WarrantyDocument != null)
                        {
                            ObjCommonHelper.UploadImage(ObjQRCModel.WarrantyDocument, path, ObjQRCModel.WarrantyDoc);
                        }
                        if (ObjQRCModel.LOCPicture != null)
                        {
                            ObjCommonHelper.UploadImage(ObjQRCModel.LOCPicture, path, ObjQRCModel.LocationPicture);
                        }
                        if (ObjQRCModel.AssetPictureUrl != null)
                        { ObjCommonHelper.UploadImage(ObjQRCModel.AssetPictureUrl, path, ObjQRCModel.AssetPicture); }

                        if (_fnResult == Result.Completed)
                        {
                            // Code for to get path of root directory and attach path of directory to store image

                            string RootDirectory = ConfigurationManager.AppSettings["QRCImage"];
                            RootDirectory = Server.MapPath(RootDirectory);
                            if (ObjQRCModel.QRCImageBase64 != null)
                            {
                                ObjQRCModel.QRCImageBase64 = ObjQRCModel.QRCImageBase64.Split(',')[1];
                                ObjCommonHelper.UploadQRCImage(ObjQRCModel.QRCImageBase64, RootDirectory, ObjQRCModel.QRCImage);
                            }

                            objDAR.ActivityDetails = DarMessage.SaveQRC(ObjPrintQRCModel.QRCName, ObjLoginModel.Location);
                            objDAR.TaskType = (long)TaskTypeCategory.QRCCreation;

                            ViewBag.Message = CommonMessage.SaveSuccessMessage();
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                            ModelState.Clear();
                            ObjQRCModel = QRCInIt();

                        }
                        else if (_fnResult == Result.UpdatedSuccessfully)
                        {
                            objDAR.ActivityDetails = DarMessage.UpdateQRC(ObjPrintQRCModel.QRCName, ObjLoginModel.Location);
                            objDAR.TaskType = (long)TaskTypeCategory.QRCUpdate;

                            ViewBag.Message = CommonMessage.UpdateSuccessMessage();
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                            ModelState.Clear();
                            ObjQRCModel = QRCInIt();
                        }
                        else if (_fnResult == Result.Failed)
                        {
                            ViewBag.Message = CommonMessage.FailureMessage();
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                            // store the failure message in tempdata to display in view.
                        }

                        #region Save DAR
                        result = _ICommonMethod.SaveDAR(objDAR);
                        #endregion Save DAR
                    }
                    else
                    {

                        ViewBag.Message = CommonMessage.DuplicateRecordMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Info;
                        // store the failure message in tempdata to display in view.
                    }
                    //ViewBag.EncryptQRC = Cryptography.GetEncryptedData(QRCDetail.QRCId.ToString(), true);

                    //ViewBag.EncryptQRC = Cryptography.GetEncryptedData(_qRCId.ToString(), true);

                    ViewBag.QRCName = ObjQRCModel.QRCName;
                    ViewBag.SpecialNote = ObjQRCModel.SpecialNotes;

                    //ViewBag.QRCSize = QRCDetail.DefaultSize;
                    /*
                                        ObjPrintQRCModel = new PrintQRCModel();


                                        /// pull details from ObjQRCModel and plug into ObjPrintQRCModel
                                        ObjPrintQRCModel.QRCId = ObjQRCModel.QRCId;

                                        ObjPrintQRCModel.QRCName = ObjQRCModel.QRCName;
                                        ObjPrintQRCModel.SpecialNotes = ObjQRCModel.SpecialNotes;
                                        ObjPrintQRCModel.QRCTYPE = ObjQRCModel.QRCTYPECaption;
                                        ObjPrintQRCModel.VehicleType = ObjQRCModel.VehicleTypeCaption;
                                        ObjPrintQRCModel.MotorType = ObjQRCModel.MotorTypeCaption;
                                        */


                    /// pull details from ObjQRCModel and plug into ObjPrintQRCModel End

                    ObjPrintQRCModel.QRCSize = _ICommonMethod.GetGlobalCodeData("QRCSIZE");
                    ObjPrintQRCModel.EncryptQRC = Cryptography.GetEncryptedData(ObjPrintQRCModel.QRCId.ToString(), true);

                    ObjPrintQRCModel.CompanyLogo = (ObjLoginModel != null && !string.IsNullOrEmpty(ObjLoginModel.LocationLogo)) ? (ObjLoginModel.LocationLogo) : "/";
                    ObjPrintQRCModel.CompanyImage = (ObjLoginModel != null && !string.IsNullOrEmpty(ObjLoginModel.LocationImage)) ? (ObjLoginModel.LocationImage) : "/";
                    ObjPrintQRCModel.CompanyName = (ObjLoginModel != null && !string.IsNullOrEmpty(ObjLoginModel.Location)) ? (ObjLoginModel.Location) : "/";

                    objLocationMasterModel = _IGlobalAdmin.GetLocationById(ObjPrintQRCModel.LocationId);
                    ObjPrintQRCModel.Location = objLocationMasterModel.LocationName;

                    //ViewBag.EncryptQRC = ObjPrintQRCModel.EncryptQRC;
                    ViewBag.EncryptQRC = ObjPrintQRCModel.QRCIDCode;
                    ViewBag.LastEncryptQRC = ObjPrintQRCModel.QRCIDCode;// + ',' + ObjPrintQRCModel.QRCTYPEID;//This field added by Bhushan Dod  //After comma we don't need type of QRC.We need to comment .
                    ViewBag.QRCSize = ObjPrintQRCModel.QRCSize;

                    //PrintQRCModel ObjPrintQRCModel;                    
                    ViewBag.QRCModel = ObjPrintQRCModel;
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    string _message = "";
                    foreach (var items in errors)
                    { _message = _message + items.ErrorMessage; }
                    ViewBag.Message = _message;
                    ObjQRCModel = _IQRCSetup.GetGlobalCodeForCategories();
                }
            }
            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "Exception For File Upload", path.ToString(), null);
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                ObjQRCModel = _IQRCSetup.GetGlobalCodeForCategories();
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                ViewBag.PurchType = _ICommonMethod.GetGlobalCodeDataList("PURCHASETYPE");
                return View(ObjQRCModel);
            }
            ObjQRCModel = _IQRCSetup.GetGlobalCodeForCategories();
            ViewBag.Country = _ICommonMethod.GetAllcountries();
            // ViewBag.PurchType = _ICommonMethod.GetGlobalCodeDataList("PURCHASETYPE");
            ViewBag.PurchType = _ICommonMethod.GetGlobalCodeDataList("PURCHASETYPE");
            ViewBag.RefreshMode = true;
            //ViewBag.EncryptLastQRC = data.EncryptLastQRC;
            //ViewBag.EncryptLastQRC = ObjQRCModel.EncryptLastQRC + "," + (string.IsNullOrEmpty(objLocationMasterModel.Address2) ? ObjLoginModel.Location.ToString().Substring(0, 3).ToUpper() : objLocationMasterModel.Address2.ToString().Substring(0, 3).ToUpper());
            //ViewBag.ItemAbberivationList = Convert.ToString(ConfigurationManager.AppSettings["ItemAbberivationList"]);

            //if (ObjLoginModel != null && (ObjLoginModel.UserRoleId == Convert.ToInt64(UserType.GlobalAdmin) || ObjLoginModel.UserRoleId == Convert.ToInt64(UserType.ITAdministrator)))
            //{
            //    //ViewBag.ManagerList = _IGlobalAdmin.GetAllITAdministratorList(0, 1, 1000, "UserEmail", "asc", "", Convert.ToInt64(UserType.Manager), out Totalrecords);
            //    ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
            //    ViewBag.AdministratorList = _IGlobalAdmin.GetAllITAdministratorList(0, 0, 1, 1000, "UserEmail", "asc", "", Convert.ToInt64(UserType.Administrator), out Totalrecords);
            //}
            //else if (ObjLoginModel != null && (ObjLoginModel.UserRoleId == Convert.ToInt64(UserType.Administrator)))
            //{
            //    ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
            //    //ViewBag.AdministratorList = _IGlobalAdmin.GetAllITAdministratorList(0, 1, 1000, "UserEmail", "asc", "", Convert.ToInt64(UserType.Manager), out Totalrecords);
            //    ViewBag.AdministratorList = _ICommonMethod.GetManagersBYLocationId(ObjLoginModel.LocationID);
            //}

            return View(ObjQRCModel);
            // return RedirectToAction("Index","QRCSetup");
        }

        [HttpGet]
        public ActionResult Edit(string qr)
        {
            long Totalrecords = 0;
            eTracLoginModel ObjLoginModel = null;
            try
            {
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

                if (!string.IsNullOrEmpty(qr))
                {
                    ViewBag.UpdateMode = true;
                    qr = Cryptography.GetDecryptedData(qr, true);
                    ViewBag.Country = _ICommonMethod.GetAllcountries();
                    ViewBag.PurchType = _ICommonMethod.GetGlobalCodeDataList("PURCHASETYPE");

                    if (ObjLoginModel != null && (ObjLoginModel.UserRoleId == Convert.ToInt64(UserType.GlobalAdmin) || ObjLoginModel.UserRoleId == Convert.ToInt64(UserType.ITAdministrator)))
                    {
                        //ViewBag.ManagerList = _IGlobalAdmin.GetAllITAdministratorList(0, 1, 1000, "UserEmail", "asc", "", Convert.ToInt64(UserType.Manager), out Totalrecords);
                        ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
                        ViewBag.AdministratorList = _IGlobalAdmin.GetAllITAdministratorList(0, 0, 1, 1000, "UserEmail", "asc", "", (UserType.Administrator.ToString()), out Totalrecords);
                    }
                    else if (ObjLoginModel != null && (ObjLoginModel.UserRoleId == Convert.ToInt64(UserType.Administrator)))
                    {
                        ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
                        //ViewBag.AdministratorList = _IGlobalAdmin.GetAllITAdministratorList(0, 1, 1000, "UserEmail", "asc", "", Convert.ToInt64(UserType.Manager), out Totalrecords);
                        ViewBag.AdministratorList = _ICommonMethod.GetManagersBYLocationId(ObjLoginModel.LocationID);
                    }

                    //QRCModel ObjQRCModel = _IQRCSetup.GetGlobalCodeForCategories();
                    QRCModel ObjQRCModel = _IQRCSetup.GetQrcById(Convert.ToInt64(qr));
                    ObjQRCModel.EncryptLastQRC = Cryptography.GetEncryptedData(ObjQRCModel.QRCId.ToString(), true);


                    ObjQRCModel.UpdateMode = true;

                    if (ObjQRCModel != null && ObjQRCModel.Allotedto != null)
                    {
                        ViewBag.LocationByAdmin = _ICommonMethod.GetLocationByAdminId(ObjQRCModel.Allotedto);
                    }

                    return View("Index", ObjQRCModel);
                }
                else
                {
                    ViewBag.AlertMessageClass = new AlertMessageClass().Danger;
                    ViewBag.Message = Result.DoesNotExist;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("Index");
        }

        public ActionResult ListQRC()
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
                ViewBag.QRCType = _ICommonMethod.GetGlobalCodeDataList("QRCTYPE");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("ListQRC");
        }

        [HttpPost]
        public JsonResult GetQRCList(bool _search, long? nd, int page, int rows, string sidx = null, string sord = null, long? locationId = 0, string SearchText = "", long SearchQRCType = 0)
        {
            JQGridResults result = new JQGridResults();
            List<JQGridRow> jqRows = new List<JQGridRow>();
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                ObjectParameter TotalRecords = new ObjectParameter("TotalRecords", typeof(int));
                var QRCList = _IQRCSetup.GetAllQRCList(null, locationId, page, rows, sidx, sord, SearchText, SearchQRCType, ObjLoginModel.UserId, TotalRecords);

                try
                {
                    foreach (var QRCode in QRCList)
                    {
                        JQGridRow row = new JQGridRow();
                        //row.id = Location.LocationId;
                        row.id = QRCode.EncryptQRC;
                        row.cell = new string[11];
                        row.cell[0] = QRCode.QRCodeID;
                        row.cell[1] = QRCode.QRCName;
                        row.cell[2] = QRCode.QRCTYPE;
                        row.cell[3] = QRCode.SpecialNotes;
                        row.cell[4] = QRCode.WarrentyDoc;
                        row.cell[5] = Convert.ToString(QRCode.QRCTYPEId);
                        row.cell[6] = Convert.ToString(QRCode.CheckOutStatus);
                        row.cell[7] = Convert.ToString(QRCode.IsDamage);
                        row.cell[8] = (QRCode.IsDamageVerified == null) ? "YesNull" : Convert.ToString(QRCode.IsDamageVerified);//This is just to check condition on JS
                        row.cell[9] = QRCode.LocationName;
                        row.cell[10] = QRCode.QRCSize;

                        jqRows.Add(row);
                    }
                    result.rows = jqRows.ToArray();
                    result.page = Convert.ToInt32(page);
                    result.total = (int)Math.Ceiling((decimal)Convert.ToInt32(TotalRecords.Value) / rows);
                    result.records = Convert.ToInt32(TotalRecords.Value);
                }

                catch (Exception ex)
                { string error = ex.Message; }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult checkQRCName(string QRCName, long QRCType, long LocId)
        {
            byte status = 0;

            status = _IQRCSetup.checkQRCName(QRCName, QRCType, LocId);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// Created By: Bhushan Dod
        /// Created Date: 04/25/2017
        /// Description: This method check is QRC exist or not bcoz while creating we shown QRCCodeID to user and after creating it would be save other user save with same codeid it will create problem to scan via mobile..
        /// </summary>
        /// <param name="QRCName"></param>
        /// <param name="QRCType"></param>
        /// <param name="LocId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult checkQRCCodeIDExist(string QRcodeID)
        {
            string[] QrcArray = QRcodeID.Split(',');
            long qrccode = Convert.ToInt64(QrcArray[2]);
            var QRCodeID = _IQRCSetup.QRCCodeIDExist(qrccode);
            return Json(new { QRCodeID }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult QRCGenerate(string qr)
        {
            long Totalrecords = 0;
            int status = 0;
            QRCModel ObjQRCModel = new QRCModel();
            eTracLoginModel ObjLoginModel = null;
            try
            {
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

                if (!string.IsNullOrEmpty(qr))
                {
                    ViewBag.UpdateMode = true;
                    qr = Cryptography.GetDecryptedData(qr, true);

                    ObjQRCModel = _IQRCSetup.GetQrcById(Convert.ToInt64(qr));
                    ObjQRCModel.WExpDate = Convert.ToDateTime(ObjQRCModel.WarrantyEndDate).ToString("MM/dd/yy");
                    ObjQRCModel.IExpDate = Convert.ToDateTime(ObjQRCModel.InsuranceExpDate).ToString("MM/dd/yy");

                    return Json(new { status = status, data = ObjQRCModel }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    //ViewBag.AlertMessageClass = new AlertMessageClass().Danger;
                    //ViewBag.Message = Result.DoesNotExist;
                }
            }
            catch (Exception ex)
            {
            }
            return Json(new { status = status, data = ObjQRCModel }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>Delete
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-11-2014</CreatedOn>
        /// <CreatedFor>Delete QRC</CreatedFor>
        /// </summary>
        /// <param name="qr"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(string qr)
        {
            DARModel objDAR;

            try
            {
                eTracLoginModel ObjLoginModel = null; long LoggedInUser = 0, QRCID = 0;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
                LoggedInUser = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;
                if (!string.IsNullOrEmpty(qr))
                { qr = Cryptography.GetDecryptedData(qr, true); }
                QRCID = Convert.ToInt64(qr);

                objDAR = new DARModel();
                objDAR.LocationId = ObjLoginModel.LocationID;
                objDAR.UserId = ObjLoginModel.UserId;
                objDAR.CreatedBy = ObjLoginModel.UserId;
                objDAR.CreatedOn = DateTime.UtcNow;
                objDAR.TaskType = (long)TaskTypeCategory.QRCDelete;

                Result result = _IQRCSetup.DeleteQRC(QRCID, LoggedInUser, objDAR, ObjLoginModel.Location);

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
            catch (Exception ex)
            { throw ex; }
            return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssetImage(long QrcID)
        {
            try
            {
                string sAssetImage = _ICommonMethod.GetAssetImageByQrcId(QrcID);
                return Json(new { AssetImage = sAssetImage });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Download(string Id)
        {
            QRCModel ObjQRCModel;
            try
            {
                if (!string.IsNullOrEmpty(Id))
                {

                    Id = Cryptography.GetDecryptedData(Id, true);
                    ObjQRCModel = _IQRCSetup.GetQrcById(Convert.ToInt64(Id));
                    if (!string.IsNullOrEmpty(ObjQRCModel.WarrantyDoc))
                    {
                        byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(ObjQRCModel.WarrantyDocumentPath));
                        string fileName = ObjQRCModel.WarrantyDoc;
                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                    }
                    else
                    {
                        return null;
                    }
                }
                else { return Json("Id is Empty!"); }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                return Json(ex.Message);
            }
        }


        public ActionResult EscannerReport()
        {
            return View();
        }


        /// <summary>QRCheckIn
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Nov-18-2016</CreatedOn>
        /// <CreatedFor>Check In QRC</CreatedFor>
        /// </summary>
        /// <param name="QRCID"></param>
        /// <param name="LoggedInUser"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("CheckIn")]
        public JsonResult QRCheckIn(string qr)
        {
            DARModel objDAR;

            try
            {
                eTracLoginModel ObjLoginModel = null; long LoggedInUser = 0, QRCID = 0;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
                LoggedInUser = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;
                if (!string.IsNullOrEmpty(qr))
                { qr = Cryptography.GetDecryptedData(qr, true); }
                QRCID = Convert.ToInt64(qr);

                objDAR = new DARModel();
                objDAR.LocationId = ObjLoginModel.LocationID;
                objDAR.UserId = ObjLoginModel.UserId;
                objDAR.CreatedBy = ObjLoginModel.UserId;
                objDAR.CreatedOn = DateTime.UtcNow;
                objDAR.TaskType = (long)TaskTypeCategory.QRCDelete;

                Result result = _IQRCSetup.UpdateQRCheckIn(QRCID, LoggedInUser, objDAR, ObjLoginModel.Location);

                if (result == Result.Completed)
                {
                    ViewBag.Message = CommonMessage.UpdateSuccessMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                }
                else if (result == Result.Failed)
                {
                    ViewBag.Message = "Something went wrong.";
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                }
            }
            catch (Exception ex)
            { throw ex; }
            return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>QRCheckIn
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Nov-18-2016</CreatedOn>
        /// <CreatedFor>Check In QRC</CreatedFor>
        /// </summary>
        /// <param name="QRCID"></param>
        /// <param name="LoggedInUser"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("IsDamageFix")]
        public JsonResult DamageFixedWeb(string qr)
        {
            DARModel objDAR;

            try
            {
                eTracLoginModel ObjLoginModel = null; long LoggedInUser = 0, QRCID = 0;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
                LoggedInUser = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;
                if (!string.IsNullOrEmpty(qr))
                { qr = Cryptography.GetDecryptedData(qr, true); }
                QRCID = Convert.ToInt64(qr);

                objDAR = new DARModel();
                objDAR.LocationId = ObjLoginModel.LocationID;
                objDAR.UserId = ObjLoginModel.UserId;
                objDAR.CreatedBy = ObjLoginModel.UserId;
                objDAR.CreatedOn = DateTime.UtcNow;
                objDAR.TaskType = (long)TaskTypeCategory.QRCDelete;

                Result result = _IQRCSetup.DamageFixed(QRCID, LoggedInUser, objDAR, ObjLoginModel.Location);

                if (result == Result.Completed)
                {
                    ViewBag.Message = CommonMessage.UpdateSuccessMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                }
                else if (result == Result.Failed)
                {
                    ViewBag.Message = "Something went wrong.";
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                }
            }
            catch (Exception ex)
            { throw ex; }
            return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created On: Dec 12 2016
        /// Description : Get server side details of print all record for QRC code.
        /// </summary>
        /// <param name="_search"></param>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <param name="locationId"></param>
        /// <param name="SearchText"></param>
        /// <param name="SearchQRCType"></param>
        /// <returns></returns>
        public JsonResult GetQRCListforPrint(string sidx = null, string sord = null, long? locationId = 0, string SearchText = "", long SearchQRCType = 0)
        {
            JQGridResults result = new JQGridResults();
            List<JQGridRow> jqRows = new List<JQGridRow>();
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                ObjectParameter TotalRecords = new ObjectParameter("TotalRecords", typeof(int));
                var QRCList = _IQRCSetup.GetAllQRCListPrint(null, locationId, sidx, sord, SearchText, SearchQRCType, ObjLoginModel.UserId);
                try
                {
                    foreach (var QRCode in QRCList)
                    {
                        JQGridRow row = new JQGridRow();
                        row.id = QRCode.EncryptQRC;
                        row.cell = new string[6];
                        row.cell[0] = QRCode.QRCodeID;
                        row.cell[1] = QRCode.QRCName;
                        row.cell[2] = QRCode.QRCTYPE;
                        row.cell[3] = Convert.ToString(QRCode.QRCTYPEId);
                        row.cell[4] = QRCode.LocationName;
                        row.cell[5] = QRCode.QRCSize;

                        jqRows.Add(row);
                    }
                    result.rows = jqRows.ToArray();
                }

                catch (Exception ex)
                { string error = ex.Message; }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}