using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
//using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers.eFleet
{
    public class eFleetDriverController : Controller
    {
        private readonly BusinessLogic.IDriverEfleet _IEfleetDriver;
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string ProfileImagePath = ConfigurationManager.AppSettings["ProfilePicPath"];
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        // GET: eFleetDriver
        public eFleetDriverController(BusinessLogic.IDriverEfleet _IEfleetDriver)
        {
            this._IEfleetDriver = _IEfleetDriver;
        }
        public ActionResult Index()
        {
            try
            {
                //Added by Ashwajit Bansod on 18/08/2017 for scenario as if view Driver Registration
                // set Session["eTrac_SelectedDasboardLocationID"] is objeTracLoginModel.LocationID.
                eTracLoginModel ObjLoginModel = null;
                var objeFleetDriverModel = new eFleetDriverModel();
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
                ViewBag.EmployeeList = _IEfleetDriver.GetAllEmployees();
                ViewBag.Country = _IEfleetDriver.GetAllcountries();
                ViewBag.StateList = _IEfleetDriver.GetStateByCountryID();
                return View("RegisterDriver", objeFleetDriverModel);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View();
        }

        /// <summary>
        /// Created By Ashwajit Bansod 08/21/2017
        /// Saving Driver data To database and also updated edited data
        /// </summary>
        /// <param name="objeFleetDriverModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(eFleetDriverModel objeFleetDriverModel)
        {
            var objeTracLoginModel = new eTracLoginModel();
            //var objeTracLoginModel = (eTracLoginModel)(Session["eTrac"]);           
            bool isUpdate = false;
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    objeTracLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    if (Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]) == 0)
                    {
                        (Session["eTrac_SelectedDasboardLocationID"]) = objeTracLoginModel.LocationID;
                    }
                }
            }
            try
            {
                if (objeFleetDriverModel.DriverID == 0)
                {
                    objeFleetDriverModel.CreatedBy = objeTracLoginModel.UserId;
                    objeFleetDriverModel.CreatedDate = DateTime.UtcNow;
                    objeFleetDriverModel.IsDeleted = false;
                    objeFleetDriverModel.LocationID = objeTracLoginModel.LocationID;
                    if (objeFleetDriverModel.DriverImage != null)
                    {
                        objeFleetDriverModel.DriverImage= HostingPrefix + ProfileImagePath.Replace("~", "") + objeFleetDriverModel.DriverImage;
                    }
                    //if (objeFleetDriverModel.DriverImage != null)
                    //{
                    //    string ImageName = objeTracLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + objeFleetDriverModel.DriverImageFile.FileName;
                    //    CommonHelper obj_CommonHelper = new CommonHelper();
                    //    obj_CommonHelper.UploadImage(objeFleetDriverModel.DriverImageFile, Server.MapPath(ConfigurationManager.AppSettings["DriverImage"]), ImageName);
                    //    objeFleetDriverModel.DriverImage = ImageName;
                    //}
                    objeFleetDriverModel.LocationName = objeTracLoginModel.Location;
                    objeFleetDriverModel.UserID = objeTracLoginModel.UserId;
                    var tt = _IEfleetDriver.SaveEfleetDriver(objeFleetDriverModel);
                    if (objeFleetDriverModel.Result == Result.Completed)
                    {
                        ModelState.Clear();
                        ViewBag.Message = CommonMessage.eFleetDriverSaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    }
                }
                //for updating edited data
                else
                {
                    isUpdate = true;
                    objeFleetDriverModel.ModifiedBy = objeTracLoginModel.UserId;
                    objeFleetDriverModel.ModifiedDate = DateTime.UtcNow;
                    objeFleetDriverModel.LocationID = objeTracLoginModel.LocationID;
                    if (objeFleetDriverModel.DriverImageFile != null)
                    {
                        string ImageName = objeTracLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + objeFleetDriverModel.DriverImageFile.FileName.ToString();
                        CommonHelper obj_CommonHelper = new CommonHelper();
                        obj_CommonHelper.UploadImage(objeFleetDriverModel.DriverImageFile, Server.MapPath(ConfigurationManager.AppSettings["DriverImage"]), ImageName);
                        objeFleetDriverModel.DriverImage = ImageName;
                    }
                    objeFleetDriverModel.LocationName = objeTracLoginModel.Location;
                    objeFleetDriverModel.UserID = objeTracLoginModel.LocationID;
                    var tt = _IEfleetDriver.SaveEfleetDriver(objeFleetDriverModel);
                    if (objeFleetDriverModel.Result == Result.UpdatedSuccessfully)
                    {
                        ModelState.Clear();
                        ViewBag.Message = CommonMessage.eFleetDriverUpdateSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            finally
            {
                ViewBag.EmployeeList = _IEfleetDriver.GetAllEmployees();
                ViewBag.Country = _IEfleetDriver.GetAllcountries();
                ViewBag.StateList = _IEfleetDriver.GetStateByCountryID();
            }
            if (isUpdate = true && objeFleetDriverModel.DriverID > 0)
            {
                ModelState.Clear();
                return View("DriverList", objeFleetDriverModel);
            }
            ModelState.Clear();
            var objeFleetDriver = new eFleetDriverModel();
            return View("RegisterDriver", objeFleetDriver);
        }

        /// <summary>
        /// Created By Ashwajit Bansod 08/21/2017
        /// For creating JQGrid List
        /// </summary>
        /// <param name="objeFleetDriverList"></param>
        /// <returns></returns>
        public ActionResult ListDriver(eFleetDriverModel objeFleetDriverList)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                var returnModel = new eFleetDriverModel();
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
                return View("DriverList", returnModel);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View();
        }
        /// <summary>
        /// Created By Ashwajit Bansod
        /// Getting all data from database and Display in JQ Grid List
        /// </summary>
        /// <param name="_search"></param>
        /// <param name="UserId"></param>
        /// <param name="locationId"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="TotalRecords"></param>
        /// <param name="sord"></param>
        /// <param name="txtSearch"></param>
        /// <param name="sidx"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetListDriver(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
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
            eFleetDriverModel objeFleetDriverModel = new eFleetDriverModel();
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rowss = new List<JQGridRow>();
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
            try
            {               
                var eFleetDriverList = _IEfleetDriver.GetListDriverDetails(UserId, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType);
                foreach (var driverList in eFleetDriverList.rows)
                {
                    if (driverList.IsDeleted == false)
                    {
                        JQGridRow row = new JQGridRow();
                        row.id = Cryptography.GetEncryptedData(Convert.ToString(driverList.DriverID), true);
                        row.cell = new string[6];
                        //row.cell[0] = driverList.QRCCodeID;
                        row.cell[0] = driverList.EmployeeNameList;
                        row.cell[1] = driverList.DriverLicenseNo;
                        row.cell[2] = driverList.CDLType;
                        row.cell[3] = (driverList.CDLExpiration == null) ? "" : driverList.CDLExpiration.Value.ToShortDateString();
                        row.cell[4] = driverList.MVRExpiration.Value.ToShortDateString();
                        row.cell[5] = driverList.DriverImage;// == "" || driverList.DriverImage == null) ? "" : HostingPrefix + ProfileImagePath.Replace("~", "") + driverList.DriverImage;
                        rowss.Add(row);
                    }
                }
                result.rows = rowss.ToArray();
                result.page = Convert.ToInt32(page);
                result.total = (int)Math.Ceiling((decimal)Convert.ToInt32(TotalRecords.Value) / rows.Value);
                result.records = Convert.ToInt32(TotalRecords.Value);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Created By Ashwajit Bansod
        /// for edited Data and fetch driver  Data 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditDriver(string id)
        {
            eTracLoginModel ObjLoginModel = null;
            eFleetDriverModel objeFleetDriverModel = new eFleetDriverModel();
            try
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                if (!string.IsNullOrEmpty(id))
                {
                    ViewBag.UpdateMode = true;
                    id = Cryptography.GetDecryptedData(id, true);
                    long _driverId = 0;
                    long.TryParse(id, out _driverId);

                    var _UserModel = _IEfleetDriver.GetDriverDetailsById(_driverId);
                    ViewBag.EmployeeList = _IEfleetDriver.GetAllEmployees();
                    ViewBag.Country = _IEfleetDriver.GetAllcountries();
                    ViewBag.StateList = _IEfleetDriver.GetStateByCountryID();
                    return View("RegisterDriver", _UserModel);
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
            return View("RegisterDriver");
        }

        /// <summary>
        /// Created By Ashwajit Bansod 08/22/2017
        /// Deletion of driver
        /// </summary>
        /// <param name="DriverID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteDriver(string DriverID)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null; long loggedInUser = 0, driverId = 0;
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                loggedInUser = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;
                if (!string.IsNullOrEmpty(DriverID))
                {
                    DriverID = Cryptography.GetDecryptedData(DriverID, true);
                }
                driverId = Convert.ToInt64(DriverID);
                Result result = _IEfleetDriver.DeleteeFleetDriver(driverId, loggedInUser, ObjLoginModel.Location);
                if (result == Result.Delete)
                {
                    ViewBag.Message = CommonMessage.DeleteSuccessMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                }
                else if (result == Result.Failed)
                {
                    ViewBag.Message = "Can't Delete ";
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

    }
}