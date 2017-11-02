using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers.eFleet
{
    public class eFleetVehicleIncidentReportingController : Controller
    {
        private readonly IEfleetVehicleIncidentReport _IEfleetVehicleIncidentReport;
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string IncidentImagePath = ConfigurationManager.AppSettings["IncidentImage"];
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        public eFleetVehicleIncidentReportingController(IEfleetVehicleIncidentReport _IEfleetVehicleIncidentReport)
        {
            this._IEfleetVehicleIncidentReport = _IEfleetVehicleIncidentReport;
        }
        // GET: eFleetVehicleIncidentReporting
        public ActionResult Index()
        {
            eFleetVehicleIncidentModel objeFleetVehicleIncidentModel = new eFleetVehicleIncidentModel();
            try
            {
                //Added by Ashwajit Bansod dated Sep-13-2017 creating as session for eFleet Vehicle Incident Reporting
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
                ViewBag.VehicleNumber = _IEfleetVehicleIncidentReport.GetVehicleNumber();
                ViewBag.StateList = _IEfleetVehicleIncidentReport.GetStateID();
                return View("RegisterVehicleIncident", objeFleetVehicleIncidentModel);
            }          
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }          
            return View("RegisterVehicleIncident");
        }
        /// <summary>
        /// Created By Ashwajit Bansod Dated:Sep-13-2017
        /// for saving and updating of vehicle Incident report
        /// </summary>
        /// <param name="objeFleetVehicleIncidentModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(eFleetVehicleIncidentModel objeFleetVehicleIncidentModel)
        {
            var objeTracLoginModel = new eTracLoginModel();
            bool isUpdate = false;
            string fName = "";
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
                if (objeFleetVehicleIncidentModel.IncidentID == 0)
                {
                    objeFleetVehicleIncidentModel.CreatedBy = objeTracLoginModel.UserId;
                    objeFleetVehicleIncidentModel.CreatedDate = DateTime.UtcNow;
                    objeFleetVehicleIncidentModel.IsDeleted = false;
                    objeFleetVehicleIncidentModel.LocationID = objeTracLoginModel.LocationID;
                    for (int i = 0; i < Request.Files.Count; i++)                       
                    {                       
                        objeFleetVehicleIncidentModel.IncidentImageFile = Request.Files[i];
                        fName = objeFleetVehicleIncidentModel.IncidentImageFile.FileName;
                        if (objeFleetVehicleIncidentModel.IncidentImageFile != null && objeFleetVehicleIncidentModel.IncidentImageFile.ContentLength > 0)
                        {
                            var path = Path.Combine(Server.MapPath("~/Content/eFleetDocs/IncidentImage"));
                            string pathString = System.IO.Path.Combine(path.ToString());
                            var fileName1 = Path.GetFileName(objeFleetVehicleIncidentModel.IncidentImageFile.FileName);
                            bool isExists = System.IO.Directory.Exists(pathString);
                            if (!isExists) System.IO.Directory.CreateDirectory(pathString);
                            var uploadpath = string.Format("{0}\\{1}", pathString, objeFleetVehicleIncidentModel.IncidentImageFile.FileName);
                            objeFleetVehicleIncidentModel.IncidentImage = uploadpath;
                        }
                    }
                    objeFleetVehicleIncidentModel.LocationName = objeTracLoginModel.Location;
                    objeFleetVehicleIncidentModel.UserID = objeTracLoginModel.UserId;
                    var tt = _IEfleetVehicleIncidentReport.SaveEfleetVehicleIncident(objeFleetVehicleIncidentModel);
                    if (objeFleetVehicleIncidentModel.Result == Result.Completed)
                    {
                        ModelState.Clear();
                        ViewBag.Message = CommonMessage.eFleetVehicleIncidentSaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    }
                }
                //for updating data
                else
                {
                    isUpdate = true;
                    objeFleetVehicleIncidentModel.ModifiedBy = objeTracLoginModel.UserId;
                    objeFleetVehicleIncidentModel.ModifiedDate = DateTime.UtcNow;
                    objeFleetVehicleIncidentModel.LocationID = objeTracLoginModel.LocationID;
                    objeFleetVehicleIncidentModel.LocationName = objeTracLoginModel.Location;
                    var tt = _IEfleetVehicleIncidentReport.SaveEfleetVehicleIncident(objeFleetVehicleIncidentModel);
                    if (objeFleetVehicleIncidentModel.Result == Result.UpdatedSuccessfully)
                    {
                        ModelState.Clear();
                        ViewBag.Message = CommonMessage.eFleetVehicleIncidentUpdateSuccessMessage();
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
                ViewBag.VehicleNumber = _IEfleetVehicleIncidentReport.GetVehicleNumber();
                ViewBag.StateList = _IEfleetVehicleIncidentReport.GetStateID();
            }
            if (isUpdate = true && objeFleetVehicleIncidentModel.IncidentID > 0)
            {
                ModelState.Clear();
                return View("ListeFleetVehicleIncident", objeFleetVehicleIncidentModel);
            }
            ModelState.Clear();
            eFleetVehicleIncidentModel ObjeFleetVehicleIncidentModel = new eFleetVehicleIncidentModel();
            return View("RegisterVehicleIncident", ObjeFleetVehicleIncidentModel);
        }
        /// <summary>
        /// Created By Ashwajit Bansod dated: sept-16-2017
        /// for Displying Grid List  
        /// </summary>
        /// <param name="objeFleetVehicleList"></param>
        /// <returns></returns>
        public ActionResult ListeFleetVehicleIncident(eFleetVehicleIncidentModel objeFleetVehicleIncidentModel)
        {
            try
            {
                //Added by Ashwajit Bansod on 08/12/2017 for scenario as if view all Vehicle List is enabled.
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
                var vehicleIncidentList = _IEfleetVehicleIncidentReport.GetAllVehicleIncidentList(objeFleetVehicleIncidentModel);

                return View("ListeFleetVehicleIncident", vehicleIncidentList);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View();
        }
        /// <summary>
        /// Created  By Ashwajit Bansod Dated:Sept-16-2017
        /// For Getting All data from Database and Display in Grid List
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
        public JsonResult GetListIncidentVehicle(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
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
            eFleetVehicleIncidentModel objeFleetVehicleIncidentModel = new eFleetVehicleIncidentModel();
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rowss = new List<JQGridRow>();
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch;
            try
            {
                var eFleetVehicleIncidentList = _IEfleetVehicleIncidentReport.GetListVahicleListDetails(UserId, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType);
                foreach (var vehicleIncidentList in eFleetVehicleIncidentList.rows)
                {
                    if (vehicleIncidentList.IsDeleted == false)
                    {
                        JQGridRow row = new JQGridRow();
                        row.id = Cryptography.GetEncryptedData(Convert.ToString(vehicleIncidentList.IncidentID), true);
                        row.cell = new string[9];
                        row.cell[0] = vehicleIncidentList.QRCodeID;
                        row.cell[1] = vehicleIncidentList.VehicleNumber;
                        row.cell[2] = vehicleIncidentList.DriverName;
                        row.cell[3] = vehicleIncidentList.AccidentDate.Value.ToShortDateString();
                        row.cell[4] = vehicleIncidentList.NumberOfInjuries;
                        row.cell[5] = (vehicleIncidentList.Preventability== true) ? "Yes" : "No";
                        row.cell[6] = vehicleIncidentList.Description;
                        row.cell[7] = vehicleIncidentList.IncidentID.ToString();
                        row.cell[8] = (vehicleIncidentList.IncidentImage == "" || vehicleIncidentList.IncidentImage == null) ? "" : HostingPrefix + IncidentImagePath.Replace("~", "") + vehicleIncidentList.IncidentImage;
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
        /// Created By Ashwajit Bansod Date: Sept-16-2017
        /// For Editing the Incident Report by using IncidentID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditIncidentVehicle(string id)
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
                    long _userId = 0;
                    long.TryParse(id, out _userId);
                    var _UserModel = _IEfleetVehicleIncidentReport.GetIncidentDetailsById(_userId);
                    ViewBag.VehicleNumber = _IEfleetVehicleIncidentReport.GetVehicleNumber();
                    ViewBag.StateList = _IEfleetVehicleIncidentReport.GetStateID();
                    return View("RegisterVehicleIncident", _UserModel);
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
            return View("RegisterVehicleIncident");
        }
        /// <summary>
        /// Created by Ashwajit Bansod dated:Sept-18-2017
        /// for Deleting the Incident Vehicle report
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteeFleetIncident(string id)
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

                Result result = _IEfleetVehicleIncidentReport.DeleteeFleetIncidentVehicle(vehicleId, loggedInUser,ObjLoginModel.Location);
                if (result == Result.Delete)
                {
                    ViewBag.Message = CommonMessage.DeleteSuccessMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                }
                else if (result == Result.Failed)
                {
                    ViewBag.Message = "Can't Delete Incident Vehicle";
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