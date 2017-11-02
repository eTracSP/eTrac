using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.Helper;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Models;
using System.Configuration;
using WorkOrderEMS.Helpers;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.IO;

namespace WorkOrderEMS.Controllers.eFleet
{
    public class eFleetVehicleController : Controller
    {
        private readonly IEfleetVehicle _IEfleetVehicle;
        private string InspectionDocPath = ConfigurationManager.AppSettings["InspectionDoc"];
        public string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string VehicleImagePath = Convert.ToString(ConfigurationManager.AppSettings["VehicleImage"], CultureInfo.InvariantCulture);
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        public eFleetVehicleController(IEfleetVehicle _IEfleetVehicle)
        {
            this._IEfleetVehicle = _IEfleetVehicle;
        }
        // GET: eFleetVehicle
        public ActionResult Index()
        {
            try
            {
                //Added by Ashwajit Bansod dated 08/23/2017 creating as session for eFleet Vehicle Registration
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
                eFleetVehicleModel objeFleetVehicleModel = new eFleetVehicleModel();
                // ViewBag.FuelType = _IEfleetVehicle.GetAllFuelType();

                return View("RegisterVehicle", objeFleetVehicleModel);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            finally
            {
                ViewBag.FuelType = _IEfleetVehicle.GetAllFuelType();
            }
            return View();
        }
        /// <summary>
        /// Created By Ashwajit Bansod
        /// date: 08/18/2017
        /// To save Vehicle data into database and also updating edited data into database  
        /// </summary>
        /// <param name="objeFleetVehicleModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(eFleetVehicleModel objeFleetVehicleModel)
        {
            var objeTracLoginModel = new eTracLoginModel();
            //eTracLoginModel objeTracLoginModel = (eTracLoginModel)(Session["eTrac"]);
            DARModel objDAR;
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

                if (objeFleetVehicleModel.VehicleID == 0)
                {
                    objeFleetVehicleModel.CreatedBy = objeTracLoginModel.UserId;
                    objeFleetVehicleModel.CreatedDate = DateTime.UtcNow;
                    objeFleetVehicleModel.IsDeleted = false;
                    objeFleetVehicleModel.QRCodeID = objeTracLoginModel.LocationCode;
                    objeFleetVehicleModel.LocationID = objeTracLoginModel.LocationID;
                    if (objeFleetVehicleModel.AttachmentOfInsuranceFile != null)
                    {
                        string ImageName = objeTracLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString(objeFleetVehicleModel.AttachmentOfInsuranceFile.FileName);
                        CommonHelper.StaticUploadImage(objeFleetVehicleModel.AttachmentOfInsuranceFile, Server.MapPath(ConfigurationManager.AppSettings["AttachmentOfInsurance"]), ImageName);
                        objeFleetVehicleModel.AttachmentOfInsurance = ImageName;
                    }
                    if (objeFleetVehicleModel.AttachmentOfRegistrationFile != null)
                    {
                        string ImageNameRegister = objeTracLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString(objeFleetVehicleModel.AttachmentOfRegistrationFile.FileName);
                        CommonHelper.StaticUploadImage(objeFleetVehicleModel.AttachmentOfRegistrationFile, Server.MapPath(ConfigurationManager.AppSettings["AttachmentOfRegistration"]), ImageNameRegister);
                        objeFleetVehicleModel.AttachmentOfRegistration = ImageNameRegister;
                    }
                    if (objeFleetVehicleModel.VehicleImageFile != null)
                    {
                        string ImageNameVehicle = objeTracLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString(objeFleetVehicleModel.VehicleImageFile.FileName);
                        CommonHelper.StaticUploadImage(objeFleetVehicleModel.VehicleImageFile, Server.MapPath(ConfigurationManager.AppSettings["VehicleImage"]), ImageNameVehicle);
                        objeFleetVehicleModel.VehicleImage = ImageNameVehicle;
                    }

                    objeFleetVehicleModel.LocationName = objeTracLoginModel.Location;
                    var returnModel = _IEfleetVehicle.SaveEfleetVehicle(objeFleetVehicleModel);

                    //Below code for details of vehicle after save Modal show brief view of vehicle.                  
                    // ViewBag.QRCodeID = returnModel.QRCodeID;
                    returnModel.EncryptLastQRC = returnModel.QRCodeID;
                    ViewBag.LastEncryptQRC = returnModel.EncryptLastQRC;
                    ViewBag.Make = returnModel.Make;
                    ViewBag.Model = returnModel.Model;
                    ViewBag.VehicleIdentificationNo = returnModel.VehicleIdentificationNo;
                    ViewBag.VehicleNumber = returnModel.VehicleNumber;
                    ViewBag.Year = returnModel.Year;
                    ViewBag.FuelTypeForModel = (returnModel.FuelType == Convert.ToInt32(eFleetEnum.Gasoline)) ? Convert.ToString(eFleetEnum.Gasoline) : Convert.ToString(eFleetEnum.DieselFuel);
                    ViewBag.VehicleImage = (objeFleetVehicleModel.VehicleImage == null) ? HostingPrefix + "/Content/Images/ProjectLogo/no-profile-pic.jpg" : HostingPrefix + (ConfigurationManager.AppSettings["VehicleImage"]).Replace("~", "") + objeFleetVehicleModel.VehicleImage;
                    ViewBag.LocationName = objeTracLoginModel.Location;
                    if (objeFleetVehicleModel.Result == Result.Completed)
                    {
                        ModelState.Clear();
                        //FlashMessage.Confirmation(CommonMessage.WorkOrderSaveSuccessMessage());
                        ViewBag.Message = CommonMessage.eFleetVehicleSaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    }
                    //objDAR.ActivityDetails = DarMessage.RegisterNewVehicle(objeTracLoginModel.LocationNames);
                    //objDAR.TaskType = (long)TaskTypeCategory.VehicleRegistration;
                }
                //for updating data
                else
                {
                    isUpdate = true;
                    objeFleetVehicleModel.ModifiedBy = objeTracLoginModel.UserId;
                    objeFleetVehicleModel.ModifiedDate = DateTime.UtcNow;
                    objeFleetVehicleModel.LocationID = objeTracLoginModel.LocationID;
                    if (objeFleetVehicleModel.AttachmentOfInsuranceFile != null)
                    {
                        string ImageName = objeTracLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString(objeFleetVehicleModel.AttachmentOfInsuranceFile.FileName);
                        CommonHelper.StaticUploadImage(objeFleetVehicleModel.AttachmentOfInsuranceFile, Server.MapPath(ConfigurationManager.AppSettings["AttachmentOfInsurance"]), ImageName);
                        objeFleetVehicleModel.AttachmentOfInsurance = ImageName;
                    }
                    if (objeFleetVehicleModel.AttachmentOfRegistrationFile != null)
                    {
                        string ImageNameRegister = objeTracLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString(objeFleetVehicleModel.AttachmentOfRegistrationFile.FileName);
                        CommonHelper.StaticUploadImage(objeFleetVehicleModel.AttachmentOfRegistrationFile, Server.MapPath(ConfigurationManager.AppSettings["AttachmentOfRegistration"]), ImageNameRegister);
                        objeFleetVehicleModel.AttachmentOfRegistration = ImageNameRegister;
                    }
                    if (objeFleetVehicleModel.VehicleImageFile != null)
                    {
                        string ImageNameVehicle = objeTracLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString(objeFleetVehicleModel.VehicleImageFile.FileName);
                        CommonHelper.StaticUploadImage(objeFleetVehicleModel.VehicleImageFile, Server.MapPath(ConfigurationManager.AppSettings["VehicleImage"]), ImageNameVehicle);
                        objeFleetVehicleModel.VehicleImage = ImageNameVehicle;
                    }
                    objeFleetVehicleModel.LocationName = objeTracLoginModel.Location;
                    var returnModel = _IEfleetVehicle.SaveEfleetVehicle(objeFleetVehicleModel);
                    //Below code for details of vehicle after save Modal show brief view of vehicle.                  
                    ViewBag.LastEncryptQRC = returnModel.EncryptLastQRC;
                    ViewBag.Make = returnModel.Make;
                    ViewBag.Model = returnModel.Model;
                    ViewBag.VehicleIdentificationNo = returnModel.VehicleIdentificationNo;
                    ViewBag.VehicleNumber = returnModel.VehicleNumber;
                    ViewBag.Year = returnModel.Year;
                    ViewBag.FuelTypeForModel = returnModel.FuelType;
                    ViewBag.VIN = returnModel.VehicleIdentificationNo;
                    ViewBag.VehicleImage = (objeFleetVehicleModel.VehicleImage == null) ? HostingPrefix + "/Content/Images/ProjectLogo/no-profile-pic.jpg" : HostingPrefix + (ConfigurationManager.AppSettings["VehicleImage"]).Replace("~", "") + objeFleetVehicleModel.VehicleImage;
                    ViewBag.LocationName = objeTracLoginModel.Location;
                    returnModel.EncryptLastQRC = returnModel.QRCodeID;
                    ViewBag.LastEncryptQRC = returnModel.EncryptLastQRC;
                    if (objeFleetVehicleModel.Result == Result.UpdatedSuccessfully)
                    {
                        ModelState.Clear();
                        //FlashMessage.Confirmation(CommonMessage.WorkOrderSaveSuccessMessage());
                        ViewBag.Message = CommonMessage.eFleetVehicleUpdateSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    }
                    //objDAR.ActivityDetails = DarMessage.UpdateVehicle(objeFleetVehicleModel.QRCodeID, objeTracLoginModel.LocationNames);
                    //objDAR.TaskType = (long)TaskTypeCategory.UpdateVehicleRegistration;

                }

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            finally
            {
                ViewBag.FuelType = _IEfleetVehicle.GetAllFuelType();
            }
            if (isUpdate = true && objeFleetVehicleModel.VehicleID > 0)
            {
                ModelState.Clear();
                return View("VehicleList", objeFleetVehicleModel);
            }
            ModelState.Clear();
            eFleetVehicleModel objeFleetVehicle = new eFleetVehicleModel();
            return View("RegisterVehicle", objeFleetVehicle);
        }

        /// <summary>
        /// created By Ashwait Bansod which Showing the List in JQGrid Form
        /// </summary>
        /// <param name="objeFleetVehicleList"></param>
        /// <returns></returns>
        /// 
        public ActionResult ListVehicle(eFleetVehicleModel objeFleetVehicleList)
        {
            try
            {
                //Added by Ashwajit Bansod on 08/12/2017 for scenario as if view all Vehicle List is enabled.
                // click on list we have to set Session["eTrac_SelectedDasboardLocationID"] is objeTracLoginModel.LocationID.
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
                var vehicleList = _IEfleetVehicle.GetAllVehicleList(objeFleetVehicleList);
                return View("VehicleList", vehicleList);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View();
        }

        /// <summary>
        /// Added By Ashwajit Bansod for Fetching the data from Database and and add into List
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
        public JsonResult GetListVehicle(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
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
                var eFleetVehicleList = _IEfleetVehicle.GetListVehicleDetails(UserId, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType);
                foreach (var vehicleList in eFleetVehicleList.rows)
                {
                    if (vehicleList.IsDeleted == false)
                    {
                        JQGridRow row = new JQGridRow();
                        row.id = Cryptography.GetEncryptedData(Convert.ToString(vehicleList.VehicleID), true);
                        row.cell = new string[11];
                        row.cell[0] = vehicleList.QRCodeID;
                        row.cell[1] = vehicleList.VehicleIdentificationNo;
                        row.cell[2] = vehicleList.VehicleNumber;
                        row.cell[3] = vehicleList.Make;
                        row.cell[4] = vehicleList.Model;
                        row.cell[5] = vehicleList.ListFuelType;
                        row.cell[6] = (vehicleList.VehicleImage == "" || vehicleList.VehicleImage == null) ? "" : HostingPrefix + VehicleImagePath.Replace("~", "") + vehicleList.VehicleImage;
                        row.cell[7] = vehicleList.GVWR;
                        row.cell[8] = vehicleList.Year;
                        row.cell[9] = vehicleList.StorageAddress;
                        row.cell[10] = vehicleList.DummyField;
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
        public ActionResult EditVehicle(string id)
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
                    long _vehicleId = 0;
                    long.TryParse(id, out _vehicleId);
                    var _VehicleModel = _IEfleetVehicle.GetVehicleDetailsById(_vehicleId);
                    ViewBag.FuelType = _IEfleetVehicle.GetAllFuelType();
                    return View("RegisterVehicle", _VehicleModel);
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
            return View("RegisterVehicle");
        }
        /// <summary>
        /// Created By Ashwajit Bansod for deleting a vehicle from List
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteVehicle(string VehicleID)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null; long loggedInUser = 0, vehicleId = 0;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
                loggedInUser = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;
                if (!string.IsNullOrEmpty(VehicleID))
                {
                    VehicleID = Cryptography.GetDecryptedData(VehicleID, true);
                }
                vehicleId = Convert.ToInt64(VehicleID);

                Result result = _IEfleetVehicle.DeleteeFleetVehicle(vehicleId, loggedInUser, ObjLoginModel.Location);
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
            {
                throw ex;
            }
            return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult doesVehicleNumberExist(string VehicleNumber)
        {
            var existVehicle = _IEfleetVehicle.IsVehicleExist(VehicleNumber);
            return Json(existVehicle, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InspectionDownload(string Id)
        {
            try
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    Id = Cryptography.GetDecryptedData(Id, true);
                    var vehicleData = _IEfleetVehicle.GetVehicleDetailsById(Convert.ToInt64(Id));
                    if (!string.IsNullOrEmpty(vehicleData.DummyField))
                    {
                        string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                        string IsFileExist = RootDirectory + InspectionDocPath;
                        RootDirectory = RootDirectory + InspectionDocPath + vehicleData.DummyField;
                        //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + DisclaimerFormPath + ObjWorkRequestAssignmentModel.DisclaimerForm;
                        if (Directory.GetFiles(IsFileExist, vehicleData.DummyField).Length > 0)
                        {
                            byte[] fileBytes = System.IO.File.ReadAllBytes(RootDirectory);
                            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, vehicleData.DummyField);
                        }
                        else
                        {
                            RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + InspectionDocPath + "FileNotFound.png";
                            byte[] fileBytes = System.IO.File.ReadAllBytes(RootDirectory);
                            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "FileNotFound.png");
                        }
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
    }
}