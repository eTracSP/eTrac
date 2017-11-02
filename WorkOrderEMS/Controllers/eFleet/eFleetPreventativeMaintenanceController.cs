using System;
using System.Collections.Generic;
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
    public class eFleetPreventativeMaintenanceController : Controller
    {
        // GET: preventiveMaintenance
        private readonly IEfleetPM _IEfleetPM;
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        public eFleetPreventativeMaintenanceController(IEfleetPM _IEfleetPM)
        {
            this._IEfleetPM = _IEfleetPM;
        }
        public ActionResult Index()
        {
            try
            {
                //Added by Ashwajit Bansod on 28/08/2017 for scenario as if view Driver Registration.
                eTracLoginModel ObjLoginModel = null;
                var objeFleetPMModel = new eFleetPMModel();
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
                ViewBag.VehicleNumber = _IEfleetPM.GetAllVehicleNumber();
                ViewBag.MeterList = _IEfleetPM.GetAllMeterList();
                ViewBag.MilesValue = _IEfleetPM.GetAllMilesValue();
                ViewBag.Category = _IEfleetPM.GetAllCategory();
                ModelState.Clear();
                return View("CreateeFleetPM", objeFleetPMModel);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View();
        }
        /// <summary>
        /// Created By Ashwajit Bansod 08/29/2017
        /// Saving Preventative Maintenance Data To database and also updated edited data
        /// </summary>
        /// <param name="objeFleetDriverModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(eFleetPMModel objeFleetPMModel)
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
                if (objeFleetPMModel.PmID == 0)
                {
                    objeFleetPMModel.CreatedBy = objeTracLoginModel.UserId;
                    objeFleetPMModel.CreatedDate = DateTime.UtcNow;
                    objeFleetPMModel.IsDeleted = false;
                    objeFleetPMModel.QRCID = objeTracLoginModel.LocationCode;
                    objeFleetPMModel.LocationID = objeTracLoginModel.LocationID;
                    //if (objeFleetPMModel.DriverImageFile != null)   // will use in Future of Vehicle Image is Required..
                    //{
                    //    string ImageName = objeTracLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + objeFleetDriverModel.DriverImageFile.FileName.ToString();
                    //    CommonHelper obj_CommonHelper = new CommonHelper();
                    //    obj_CommonHelper.UploadImage(objeFleetDriverModel.DriverImageFile, Server.MapPath(ConfigurationManager.AppSettings["DriverImage"]), ImageName);
                    //    objeFleetDriverModel.DriverImage = ImageName;
                    //}
                    objeFleetPMModel.LocationName = objeTracLoginModel.Location;
                    objeFleetPMModel.UserId = objeTracLoginModel.UserId;
                    var tt = _IEfleetPM.SaveEfleetPreventativeMaintenance(objeFleetPMModel);
                    if (objeFleetPMModel.Result == Result.Completed)
                    {
                        ModelState.Clear();
                        ViewBag.Message = CommonMessage.eFleetPrevantativeMaintenanceSaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    }
                }
                //for updating edited data
                else
                {
                    isUpdate = true;
                    objeFleetPMModel.ModifiedBy = objeTracLoginModel.UserId;
                    objeFleetPMModel.ModifiedDate = DateTime.UtcNow;
                    objeFleetPMModel.LocationID = objeTracLoginModel.LocationID;
                    objeFleetPMModel.ID = objeFleetPMModel.PmID;
                    objeFleetPMModel.LocationName = objeTracLoginModel.Location;
                    var tt = _IEfleetPM.SaveEfleetPreventativeMaintenance(objeFleetPMModel);
                    if (objeFleetPMModel.Result == Result.UpdatedSuccessfully)
                    {
                        ModelState.Clear();
                        ViewBag.Message = CommonMessage.eFleetPrevantativeMaintenanceUpdateSuccessMessage();
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
                ViewBag.VehicleNumber = _IEfleetPM.GetAllVehicleNumber();
                ViewBag.MeterList = _IEfleetPM.GetAllMeterList();
                ViewBag.MilesValue = _IEfleetPM.GetAllMilesValue();
                ViewBag.Category = _IEfleetPM.GetAllCategory();
            }
            if (isUpdate = true && objeFleetPMModel.PmID > 0)
            {
                ModelState.Clear();
                return View("eFleetPMList", objeFleetPMModel);
            }
            // if (ModelState.IsValid)
            ModelState.Clear();
            eFleetPMModel PMModel =  new eFleetPMModel();
            return View("CreateeFleetPM", PMModel);
        }
        /// <summary>
        /// Created By Ashwajit Bansod 08/31/2017
        /// For creating JQGrid List
        /// </summary>
        /// <param name="objeFleetDriverList"></param>
        /// <returns></returns>
        public ActionResult ListeFleetPM(eFleetPMModel objeFleetPMModelList)
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
                var eFleetPMList = _IEfleetPM.GetAlleFleetPMList(objeFleetPMModelList);

                return View("eFleetPMList", eFleetPMList);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View();
        }
        /// <summary>
        /// Created By Ashwajit Bansod dated 09/01/2017
        /// For creating List and fetching data from database
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
        public JsonResult GetListeFleetPM(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
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
            eFleetVehicleModel objeFleetVehicleModel = new eFleetVehicleModel();
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rowss = new List<JQGridRow>();
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch;
            try
            {
                var eFleetPMList = _IEfleetPM.GetListeFleetPMDetails(UserId, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType);
                foreach (var efleetpmList in eFleetPMList.rows)
                {
                    if (efleetpmList.IsDeleted == false)
                    {
                        JQGridRow row = new JQGridRow();
                        row.id = Cryptography.GetEncryptedData(Convert.ToString(efleetpmList.PmID), true);
                        row.cell = new string[9];
                        row.cell[0] = efleetpmList.QRCodeID;
                        row.cell[1] = efleetpmList.VehicleNumber;
                        row.cell[2] = efleetpmList.ListMeter;
                        row.cell[3] = (efleetpmList.ListMeter == "Hours" || efleetpmList.ListMeter == "Annual") ? (efleetpmList.ListMeter == "Hours") ? efleetpmList.HoursValue.ToString() : "N/A" : efleetpmList.ListReminderMetric;
                        row.cell[4] = (efleetpmList.PMCategoryList != null) ? efleetpmList.PMCategoryList : null;
                        row.cell[5] = efleetpmList.ServiceDueDate.Value.ToString("dd/MM/yyyy");
                        row.cell[6] = efleetpmList.ReminderMetricDesc;
                        row.cell[7] = (efleetpmList.ListReminderMetric == "Other") ? efleetpmList.OtherMilesComment.ToString() : "N/A";
                        row.cell[8] = efleetpmList.PmID.ToString();
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
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Edit Vehicle
        /// Created By Ashwajit Bansod
        /// Date : 08/12/2017
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditeFleetPM(string id)
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
                    long _vehicleid = 0;
                    long.TryParse(id, out _vehicleid);
                    var _eFleetPMModel = _IEfleetPM.GeteFleetPMDetailsById(_vehicleid);
                    ViewBag.VehicleNumber = _IEfleetPM.GetAllVehicleNumber();
                    ViewBag.MeterList = _IEfleetPM.GetAllMeterList();
                    ViewBag.MilesValue = _IEfleetPM.GetAllMilesValue();
                    ViewBag.Category = _IEfleetPM.GetAllCategory();
                    return View("CreateeFleetPM", _eFleetPMModel);
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
            return View("CreateeFleetPM");
        }
        /// <summary>
        /// Created By Ashwajit Bansod for deleting a vehicle from List
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteeFleetPM(string id)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null; long loggedInUser = 0, vehicleId = 0;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
                loggedInUser = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;
                if (!string.IsNullOrEmpty(id))
                {
                    id = Cryptography.GetDecryptedData(id, true);
                }
                vehicleId = Convert.ToInt64(id);

                Result result = _IEfleetPM.DeleteeFleetPM(vehicleId, loggedInUser, ObjLoginModel.Location);
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
            }
            catch (Exception ex)
            { throw ex; }
            return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
        }
    }
}