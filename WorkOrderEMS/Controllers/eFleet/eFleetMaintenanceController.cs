using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic.Interfaces.eFleet;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers
{
    public class eFleetMaintenanceController : Controller
    {
        private readonly IEfleetMaintenance _IEfleetMaintenance;
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string IncidentImagePath = ConfigurationManager.AppSettings["IncidentImage"];
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        public eFleetMaintenanceController(IEfleetMaintenance _IEfleetMaintenance)
        {
            this._IEfleetMaintenance = _IEfleetMaintenance;
        }
        // GET: eFleetMaintenance
        public ActionResult Index()
        {
            var objeFleetMaintenanceModel = new eFleetMaintenanceModel();
            var objeFleetPMModel = new eFleetPMModel();
            try
            {
                //Added by Ashwajit Bansod dated Sept-20-2017 creating as session for eFleet Maintenance Reporting
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
                ViewBag.VehicleNumber = _IEfleetMaintenance.GetVehicleNumber(ObjLoginModel.LocationID);
                ViewBag.MaintenanceType = _IEfleetMaintenance.GetAllMaintenanceType();
                ViewBag.RemainderMetricDesc = _IEfleetMaintenance.GetAllPendingPMReminderDescription(ObjLoginModel.LocationID);
                return View("CreateeFleetMaintenance", objeFleetMaintenanceModel);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("CreateeFleetMaintenance");
        }
        /// <summary>
        /// Created By Ashwajit Bansod Dated : Sept-22-2017
        /// For Saving and editing the Maintenance Data 
        /// </summary>
        /// <param name="objeFleetMaintenanceModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(eFleetMaintenanceModel objeFleetMaintenanceModel)
        {
            var objeTracLoginModel = new eTracLoginModel();
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
                if (objeFleetMaintenanceModel.MaintenanceID == 0)
                {
                    objeFleetMaintenanceModel.CreatedBy = objeTracLoginModel.UserId;
                    objeFleetMaintenanceModel.CreatedDate = DateTime.UtcNow;
                    objeFleetMaintenanceModel.IsDeleted = false;
                    objeFleetMaintenanceModel.LocationID = objeTracLoginModel.LocationID;
                    //if (objeFleetMaintenanceModel. != null)
                    //{
                    //    string ImageName = objeTracLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + objeFleetDriverModel.DriverImageFile.FileName.ToString();
                    //    CommonHelper obj_CommonHelper = new CommonHelper();
                    //    obj_CommonHelper.UploadImage(objeFleetDriverModel.DriverImageFile, Server.MapPath(ConfigurationManager.AppSettings["DriverImage"]), ImageName);
                    //    objeFleetDriverModel.DriverImage = ImageName;
                    //}
                    objeFleetMaintenanceModel.LocationName = objeTracLoginModel.Location;
                    objeFleetMaintenanceModel.UserID = objeTracLoginModel.UserId;
                    var tt = _IEfleetMaintenance.SaveEfleetMaintenance(objeFleetMaintenanceModel);
                    if (objeFleetMaintenanceModel.Result == Result.Completed)
                    {
                        ModelState.Clear();
                        ViewBag.Message = CommonMessage.eFleetMaintenanceSaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    }
                }
                //for updating edited data
                else
                {
                    isUpdate = true;
                    objeFleetMaintenanceModel.ModifiedBy = objeTracLoginModel.UserId;
                    objeFleetMaintenanceModel.ModifiedDate = DateTime.UtcNow;
                    objeFleetMaintenanceModel.LocationID = objeTracLoginModel.LocationID;
                    //if (objeFleetDriverModel.DriverImageFile != null)
                    //{
                    //    string ImageName = objeTracLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + objeFleetDriverModel.DriverImageFile.FileName.ToString();
                    //    CommonHelper obj_CommonHelper = new CommonHelper();
                    //    obj_CommonHelper.UploadImage(objeFleetDriverModel.DriverImageFile, Server.MapPath(ConfigurationManager.AppSettings["DriverImage"]), ImageName);
                    //    objeFleetDriverModel.DriverImage = ImageName;
                    //}
                    objeFleetMaintenanceModel.LocationName = objeTracLoginModel.Location;
                    objeFleetMaintenanceModel.UserID = objeTracLoginModel.UserId;
                    var tt = _IEfleetMaintenance.SaveEfleetMaintenance(objeFleetMaintenanceModel);
                    if (objeFleetMaintenanceModel.Result == Result.UpdatedSuccessfully)
                    {
                        ModelState.Clear();
                        ViewBag.Message = CommonMessage.eFleetMaintenanceUpdateSuccessMessage();
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
                ViewBag.VehicleNumber = _IEfleetMaintenance.GetVehicleNumber(objeTracLoginModel.LocationID);
                ViewBag.MaintenanceType = _IEfleetMaintenance.GetAllMaintenanceType();
                ViewBag.RemainderMetricDesc = _IEfleetMaintenance.GetAllPendingPMReminderDescription(objeTracLoginModel.LocationID);
            }
            if (isUpdate = true && objeFleetMaintenanceModel.MaintenanceID > 0)
            {
                ModelState.Clear();
                return View("ListeFleetMaintenance", objeFleetMaintenanceModel);
            }
            ModelState.Clear();
            eFleetMaintenanceModel objeFleetMaintenance = new eFleetMaintenanceModel();
            return View("CreateeFleetMaintenance", objeFleetMaintenance);
        }
        /// <summary>
        /// Created By Ashwajit Bansod Dated : Sept-22-2017
        /// For creating a JQGrid List of Maintenance
        /// </summary>
        /// <param name="objeFleetMaintenanceModelList"></param>
        /// <returns></returns>
        public ActionResult ListeFleetMaintenance(eFleetMaintenanceModel objeFleetMaintenanceModelList)
        {
            try
            {
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
                var MaintenanceList = _IEfleetMaintenance.GetAllMaintenanceList(objeFleetMaintenanceModelList);

                return View("ListeFleetMaintenance", MaintenanceList);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View();
        }
        /// <summary>
        /// Created By Ashwajit Bansod Dated : Sept-22-2017
        /// for Displaying data in JQ Grid List form
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
        public JsonResult GetListMaintenance(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
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
            var objeFleetMaintenanceModel = new eFleetMaintenanceModel();
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rowss = new List<JQGridRow>();
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);
            try
            {
                var eFleetMaintenanceList = _IEfleetMaintenance.GetListeFleetMaintenanceDetails(UserId, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType);
                foreach (var efleetmaintenanceList in eFleetMaintenanceList.rows)
                {
                    if (efleetmaintenanceList.IsDeleted == false)
                    {
                        JQGridRow row = new JQGridRow();
                        row.id = Cryptography.GetEncryptedData(Convert.ToString(efleetmaintenanceList.MaintenanceID), true);
                        row.cell = new string[9];
                        row.cell[0] = efleetmaintenanceList.VehicleNumber;
                        row.cell[1] = efleetmaintenanceList.MaintenanceTypeList;
                        row.cell[2] = efleetmaintenanceList.MaintenanceDate.Value.ToString("dd/MM/yyyy");
                        row.cell[3] = efleetmaintenanceList.ReminderMetricDesc;
                        row.cell[4] = efleetmaintenanceList.DaysOutOfService.ToString();                       
                        row.cell[5] = efleetmaintenanceList.DriverName;                        
                        row.cell[6] = efleetmaintenanceList.TotalCost.ToString();
                        row.cell[7] = efleetmaintenanceList.Miles;
                        row.cell[8] = efleetmaintenanceList.Note;
                        // Vehicle Image will be use in Future if Image is Required for PM 
                        // row.cell[5] = (vehicleList.VehicleImage == "" || vehicleList.VehicleImage == null) ? "" : HostingPrefix + ProfileImagePath.Replace("~", "") + vehicleList.VehicleImage;
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
            //{ViewBag.Message = ex.Message;ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Created By Ashwajit Bansod Date : Sept-22-2017
        /// For editing the Maintenance Data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditMaintenance(string id)
        {
            eTracLoginModel ObjLoginModel = null;
            try
            {
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

                if (!string.IsNullOrEmpty(id))
                {
                    ViewBag.UpdateMode = true;
                    id = Cryptography.GetDecryptedData(id, true);
                    long _maintenanceid = 0;
                    long.TryParse(id, out _maintenanceid);
                    var _eFleetMaintenanceModel = _IEfleetMaintenance.GeteFleetMaintenanceDetailsById(_maintenanceid);
                    ViewBag.VehicleNumber = _IEfleetMaintenance.GetVehicleNumber(ObjLoginModel.LocationID);
                    ViewBag.MaintenanceType = _IEfleetMaintenance.GetAllMaintenanceType();
                    ViewBag.RemainderMetricDesc = _IEfleetMaintenance.GetAllPendingPMReminderDescription(ObjLoginModel.LocationID);
                    return View("CreateeFleetMaintenance", _eFleetMaintenanceModel);
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
            return View("CreateeFleetMaintenance");
        }
        /// <summary>
        /// Created By Ashwajit Bansod : Dated Sept-22-2017
        /// For deleting the maintenance record
        /// </summary>
        /// <param name="MaintenanceID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteMaintenance(string MaintenanceID)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null; long loggedInUser = 0, maintenanceId = 0;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
                loggedInUser = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;
                if (!string.IsNullOrEmpty(MaintenanceID))
                {
                    MaintenanceID = Cryptography.GetDecryptedData(MaintenanceID, true);
                }
                maintenanceId = Convert.ToInt64(MaintenanceID);

                Result result = _IEfleetMaintenance.DeleteeFleetMaintenance(maintenanceId, loggedInUser, ObjLoginModel.Location);
                if (result == Result.Delete)
                {
                    ViewBag.Message = CommonMessage.DeleteSuccessMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                }
                else if (result == Result.Failed)
                {
                    ViewBag.Message = "Can't Delete Maintenace Record";
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