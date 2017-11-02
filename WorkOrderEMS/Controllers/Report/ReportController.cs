using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.UserModels;

namespace WorkOrderEMS.Controllers.Report
{
    [Authorize]
    public class ReportController : Controller
    {
        //
        // GET: /Report/
        private readonly ICommonMethod _ICommonMethod;
        private readonly IReportManager _IReportManager;

        public ReportController(ICommonMethod _ICommonMethod, IReportManager _IReportManager)
        {
            this._ICommonMethod = _ICommonMethod;
            this._IReportManager = _IReportManager;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Reports(string ReportName)
        {
            try
            {
                ReportModel _ReportModel = new ReportModel();
                _ReportModel.ReportName = ReportName;
                return View("Reports", _ReportModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #region eScanner

        public ActionResult TrashLevelCheck()
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];

            }
            var objWorkOrderIssueedModelList = new List<WorkOrderIssueedModel>();
            //objWorkOrderIssueedModelList = _IReportManager.GetWorkOrderListForLocation(objLoginSession.LocationID, "02/08/14", "04/10/2015", null, "");
            return PartialView("_TrashLevelCheck");

        }

        #region Report QRC Scan
        [ActionName("QRCScan")]
        public ActionResult GetQRCScanLog()
        {
            return PartialView("_GetQRCScanLog");
        }
        /// <summary>Created by Bhushan Dod on 15/04/2015
        /// Get details of QRC Scan for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public ActionResult GetAllQRCScan(string locationId, string userId, DateTime? fromDate, DateTime? toDate)
        {
            if (locationId == null || locationId == "0")
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    locationId = objLoginSession.LocationID.ToString();
                }
            }
            long projectId = 0;
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            if (!string.IsNullOrEmpty(locationId))
            {
                long.TryParse(locationId, out projectId);
                long.TryParse(userId, out _userId);
                //if (toDate != null)
                //{
                //    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                //        toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                //}

                ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
                //if (fromDate != null && toDate != null)
                //{
                //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
                //    {
                //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
                //    }
                //}
                lstdata = _IReportManager.GetQrcScanList(projectId, _userId, fromDate, toDate, null).ToList();
                return Json(lstdata, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>Created by Bhushan Dod on 03/04/2015
        /// Get details of Routine Check for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public JsonResult GetAllQRCScanDataByQrcType(string locationId, string userId, DateTime? fromDate, DateTime? toDate, string name)
        {
            if (locationId == null || locationId == "0")
            {

                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    locationId = objLoginSession.LocationID.ToString();
                }
            }
            long projectId = 0;
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            if (!string.IsNullOrEmpty(locationId))
            {
                long.TryParse(locationId, out projectId);
                long.TryParse(userId, out _userId);
                //if (toDate != null)
                //{
                //    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                //        toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                //}

                ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
                //if (fromDate != null && toDate != null)
                //{
                //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
                //    {
                //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
                //    }
                //}
                lstdata = _IReportManager.GetQrcScanList(projectId, _userId, fromDate, toDate, name).ToList(); ;
                return Json(lstdata, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>Created by Bhushan Dod on 17/04/2015
        /// Get details of Employee scan qrc log
        /// </summary>
        /// <param name="qrcName,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public JsonResult GetEmployeeQRCScandetails(string qrcName, string userName, DateTime? fromDate, DateTime? toDate, [Bind(Prefix = "eT")] string exportingType = null,
            [Bind(Prefix = "sC")] string sortCol = null, [Bind(Prefix = "sD")] string sortDir = null, [Bind(Prefix = "h1")] string highChart1 = null,
            [Bind(Prefix = "h2")] string highChart2 = null, [Bind(Prefix = "iT")] bool isTable = false)
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            bool isTo12noon = false;
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
          //  long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                {
                    isTo12noon = true;
                   // toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                }
            }

            ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
            //if (fromDate != null && toDate != null)
            //{
            //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
            //    {
            //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
            //    }
            //}
            lstdata = _IReportManager.QrcTypeScanListByEmployee(objLoginSession.LocationID, qrcName, userName, fromDate, toDate).ToList();
            //Below code is for if click on FullPage PDF Export
            if (exportingType == "PDF")
            {
                DateTime FD = Convert.ToDateTime(fromDate);
                string fDate = FD.ToString("MMMM dd, yyyy hh:mm tt");
                DateTime TD = Convert.ToDateTime(toDate);
                if (isTo12noon)
                {
                    TimeSpan ts = new TimeSpan(23, 59, 59);
                    TD = TD.Date + ts;
                }
                string tDate = TD.ToString("MMMM dd, yyyy hh:mm tt");
                string fileName = StringExtensionMethods.AppendTimeStamp(ReportName.QRCScan + ".pdf");
                string RootPath = Server.MapPath("~/");
                string RootDirectory = Server.MapPath("~/ReportPDF/FullPageReport/eScanner/");
                if (!Directory.Exists(RootDirectory))
                {
                    Directory.CreateDirectory(RootDirectory);
                }
                string filePath = RootDirectory + fileName;

                if (isTable == true)
                {
                    switch (sortCol)
                    {
                        case "User Name":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.ScanUserName).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.ScanUserName).ToList();
                            break;
                        case "QR Code ID":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.QrCodeId).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.QrCodeId).ToList();
                            break;
                        case "QRC Name":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.QrcName).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.QrcName).ToList();
                            break;
                        case "Scan Date":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.CreatedDate).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.CreatedDate).ToList();
                            break;

                        default:
                            lstdata = lstdata.OrderByDescending(o => o.CreatedDate).ToList();
                            break;
                    }
                    ExportPDF<ReportChart>(lstdata as IList<ReportChart>, new string[] { "ScanUserName", "QrCodeId", "QrcName", "QrcTypeName", "LocationName", "StrCreatedDate" },
                        new string[] { "User Name", "QR Code ID", "QRC Name", "QRC Type", "Location", "Scan Date" },
                        filePath, PDFReportNameHeading.QRCScan, fDate, tDate, highChart1, highChart2, isTable, "QRC  for" + qrcName + " scanned  by " + userName);
                }
                else
                {
                    ExportPDF<ReportChart>(lstdata as IList<ReportChart>, new string[] { "ScanUserName", "QrCodeId", "QrcName", "QrcTypeName", "LocationName", "StrCreatedDate" },
                      new string[] { "User Name", "QR Code ID", "QRC Name", "QRC Type", "Location", "Scan Date" },
                      filePath, PDFReportNameHeading.QRCScan, fDate, tDate, highChart1, highChart2, isTable);
                }
                promptBoxForPDFFileSave(filePath, fileName);
                return null;

            }
            else
                return Json(lstdata, JsonRequestBehavior.AllowGet);
        }

        #endregion Report QRC Scan

        #region Report Routine Check
        [ActionName("RoutineCheck")]
        public ActionResult GetAllRoutineCheck1()
        {
            return PartialView("_RoutineCheck");
        }

        /// <summary>Created by Bhushan Dod on 03/04/2015
        /// Get details of Routine Check for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public ActionResult GetAllRoutineCheck(string locationId, string userId, DateTime? fromDate, DateTime? toDate)
        {
            if (locationId == null || locationId == "0")
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    locationId = objLoginSession.LocationID.ToString();
                }
            }
            long projectId = 0;
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            if (!string.IsNullOrEmpty(locationId))
            {
                long.TryParse(locationId, out projectId);
                long.TryParse(userId, out _userId);
                //if (toDate != null)
                //{
                //    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                //        toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                //}

                ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
                //if (fromDate != null && toDate != null)
                //{
                //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
                //    {
                //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
                //    }
                //}
                lstdata = _IReportManager.GetRoutineCheckList(projectId, _userId, fromDate, toDate, null).ToList();
                return Json(lstdata, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>Created by Bhushan Dod on 03/04/2015
        /// Get details of Routine Check for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public JsonResult GetAllRoutineCheckDataByQrcType(string locationId, string userId, DateTime? fromDate, DateTime? toDate, string name)
        {
            if (locationId == null || locationId == "0")
            {

                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    locationId = objLoginSession.LocationID.ToString();
                }
            }
            long projectId = 0;
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            if (!string.IsNullOrEmpty(locationId))
            {
                long.TryParse(locationId, out projectId);
                long.TryParse(userId, out _userId);
                //if (toDate != null)
                //{
                //    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                //        toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                //}
                lstdata = _IReportManager.GetRoutineCheckList(projectId, _userId, fromDate, toDate, name).ToList(); ;
                return Json(lstdata, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>
        /// Created by Ankit Choudhary on 01/02/2017
        /// Routine scandetails for table 
        /// </summary>
        /// <param name="qrcName"></param>
        /// <param name="userName"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="exportingType"></param>
        /// <param name="sortCol"></param>
        /// <param name="sortDir"></param>
        /// <param name="highChart1"></param>
        /// <param name="highChart2"></param>
        /// <param name="isTable"></param>
        /// <returns></returns>
        public JsonResult GetEmployeeRoutineScandetails(string qrcName, string userName, DateTime? fromDate, DateTime? toDate, [Bind(Prefix = "eT")] string exportingType = null,
           [Bind(Prefix = "sC")] string sortCol = null, [Bind(Prefix = "sD")] string sortDir = null, [Bind(Prefix = "h1")] string highChart1 = null,
           [Bind(Prefix = "h2")] string highChart2 = null, [Bind(Prefix = "iT")] bool isTable = false)
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();

            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            bool isTo12noon = false;
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isTo12noon = true;
                //toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
            }

            ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
            //if (fromDate != null && toDate != null)
            //{
            //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
            //    {
            //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
            //    }
            //}
            lstdata = _IReportManager.RoutineScanListByEmployee(objLoginSession.LocationID, qrcName, userName, fromDate, toDate).ToList();
            //Below code is for if click on FullPage PDF Export
            if (exportingType == "PDF")
            {
                DateTime FD = Convert.ToDateTime(fromDate);
                string fDate = FD.ToString("MMMM dd, yyyy hh:mm tt");
                DateTime TD = Convert.ToDateTime(toDate);
                if (isTo12noon)
                {
                    TimeSpan ts = new TimeSpan(23, 59, 59);
                    TD = TD.Date + ts;
                }
                //string tDate = TD.ToString("MMMM dd, yyyy hh:mm");
                string tDate = TD.ToString("MMMM dd, yyyy hh:mm tt");
                string fileName = StringExtensionMethods.AppendTimeStamp(ReportName.RoutineCheck + ".pdf");
                string RootPath = Server.MapPath("~/");
                string RootDirectory = Server.MapPath("~/ReportPDF/FullPageReport/eScanner/");
                if (!Directory.Exists(RootDirectory))
                {
                    Directory.CreateDirectory(RootDirectory);
                }
                string filePath = RootDirectory + fileName;

                if (isTable == true)
                {
                    switch (sortCol)
                    {
                        case "User Name":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.ScanUserName).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.ScanUserName).ToList();
                            break;
                        case "QR Code ID":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.QrCodeId).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.QrCodeId).ToList();
                            break;
                        case "QRC Name":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.QrcName).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.QrcName).ToList();
                            break;
                        case "Scan Date":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.CreatedDate).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.CreatedDate).ToList();
                            break;
                        case "Description":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.RoutineDescription).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.RoutineDescription).ToList();
                            break;

                        default:
                            lstdata = lstdata.OrderByDescending(o => o.CreatedDate).ToList();
                            break;
                    }
                    ExportPDF<ReportChart>(lstdata as IList<ReportChart>, new string[] { "ScanUserName", "QrCodeId", "QrcName", "KeyName", "RoutineDescription", "StrCreatedDate" },
                        new string[] { "User Name", "QR Code ID", "QRC Name", "QRC Type", "Description", "Scan Date" },
                        filePath, PDFReportNameHeading.RoutineCheck, fDate, tDate, highChart1, highChart2, isTable, "Routine check for " + qrcName + " checked by " + userName);
                }
                else
                {
                    ExportPDF<ReportChart>(lstdata as IList<ReportChart>, new string[] { "ScanUserName", "QrCodeId", "QrcName", "KeyName", "RoutineDescription", "StrCreatedDate" },
                      new string[] { "User Name", "QR Code ID", "QRC Name", "QRC Type", "Description", "Scan Date" },
                      filePath, PDFReportNameHeading.RoutineCheck, fDate, tDate, highChart1, highChart2, isTable);
                }
                promptBoxForPDFFileSave(filePath, fileName);
                return null;

            }
            else
                return Json(lstdata, JsonRequestBehavior.AllowGet);
        }
        #endregion Report Routine Check

        #region Report Cleaning
        [ActionName("Cleaning")]
        public ActionResult Cleaning()
        {
            return PartialView("_Cleaning");
        }

        /// <summary>Created by Bhushan Dod on 22/04/2015
        /// Get details of Cleaning done for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public ActionResult GetAllCleaningDone(string locationId, string userId, DateTime? fromDate, DateTime? toDate)
        {
            if (locationId == null || locationId == "0")
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    locationId = objLoginSession.LocationID.ToString();
                }
            }
            long projectId = 0;
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            if (!string.IsNullOrEmpty(locationId))
            {
                long.TryParse(locationId, out projectId);
                long.TryParse(userId, out _userId);
                //if (toDate != null)
                //{
                //    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                //        toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                //}

                ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
                //if (fromDate != null && toDate != null)
                //{
                //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
                //    {
                //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
                //    }
                //}
                lstdata = _IReportManager.GetCleaningDoneList(projectId, _userId, fromDate, toDate, null).ToList();
                return Json(lstdata, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>Created by Bhushan Dod on 22/04/2015
        /// Get details of Cleaning done for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public JsonResult GetAllCleaningDoneDataByQrcType(string locationId, string userId, DateTime? fromDate, DateTime? toDate, string name)
        {
            if (locationId == null || locationId == "0")
            {

                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    locationId = objLoginSession.LocationID.ToString();
                }
            }
            long projectId = 0;
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            if (!string.IsNullOrEmpty(locationId))
            {
                long.TryParse(locationId, out projectId);
                long.TryParse(userId, out _userId);
                //if (toDate != null)
                //{
                //    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                //        toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                //}

                ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
                //if (fromDate != null && toDate != null)
                //{
                //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
                //    {
                //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
                //    }
                //}
                lstdata = _IReportManager.GetCleaningDoneList(projectId, _userId, fromDate, toDate, name).ToList(); ;
                return Json(lstdata, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>Created by Bhushan Dod on 23/04/2015
        /// Get details of Cleanin done qrc log
        /// </summary>
        /// <param name="qrcName,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public JsonResult GetEmployeeCleaningDonedetails(string qrcName, string userName, DateTime? fromDate, DateTime? toDate, [Bind(Prefix = "eT")] string exportingType = null, [Bind(Prefix = "sC")] string sortCol = null, [Bind(Prefix = "sD")] string sortDir = null, [Bind(Prefix = "h1")] string highChart1 = null, [Bind(Prefix = "h2")] string highChart2 = null, [Bind(Prefix = "iT")] bool isTable = false)
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            bool isTo12noon = false;
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isTo12noon = true;
               // toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
            }

            ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
            //if (fromDate != null && toDate != null)
            //{
            //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
            //    {
            //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
            //    }
            //}
            lstdata = _IReportManager.RoutineDoneListByEmployee(objLoginSession.LocationID, qrcName, userName, fromDate, toDate).ToList();
            if (exportingType == "PDF")
            {
                DateTime FD = Convert.ToDateTime(fromDate);
                string fDate = FD.ToString("MMMM dd, yyyy hh:mm tt");
                DateTime TD = Convert.ToDateTime(toDate);
                if (isTo12noon)
                {
                    TimeSpan ts = new TimeSpan(23, 59, 59);
                    TD = TD.Date + ts;
                }
                string tDate = TD.ToString("MMMM dd, yyyy hh:mm tt");
                string fileName = StringExtensionMethods.AppendTimeStamp(ReportName.CleaningReport + ".pdf");
                string RootPath = Server.MapPath("~/");
                string RootDirectory = Server.MapPath("~/ReportPDF/FullPageReport/eScanner/");
                if (!Directory.Exists(RootDirectory))
                {
                    Directory.CreateDirectory(RootDirectory);
                }
                string filePath = RootDirectory + fileName;

                if (isTable == true)
                {
                    switch (sortCol)
                    {
                        case "User Name":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.ScanUserName).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.ScanUserName).ToList();
                            break;
                        case "QR Code ID":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.QrCodeId).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.QrCodeId).ToList();
                            break;
                        case "QRC Name":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.QrcName).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.QrcName).ToList();
                            break;
                        case "Description":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.CleaningDescription).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.CleaningDescription).ToList();
                            break;
                        case "Scan Date":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.StrCreatedDate).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.StrCreatedDate).ToList();
                            break;

                        default:
                            lstdata = lstdata.OrderByDescending(o => o.StrCreatedDate).ToList();
                            break;
                    }
                    ExportPDF<ReportChart>(lstdata as IList<ReportChart>, new string[] { "ScanUserName", "QrCodeId", "QrcName", "KeyName", "CleaningDescription", "StrCreatedDate" },
                        new string[] { "User Name", "QR Code ID", "QRC Name", "QRC Type", "Description", "Scan Date" },
                        filePath, PDFReportNameHeading.CleaningReport, fDate, tDate, highChart1, highChart2, isTable, "" + " Cleaning done for " + qrcName + " by " + userName);
                }
                else
                {
                    ExportPDF<ReportChart>(lstdata as IList<ReportChart>, new string[] { "ScanUserName", "QrCodeId", "QrcName", "KeyName", "CleaningDescription", "StrCreatedDate" },
                        new string[] { "User Name", "QR Code ID", "QRC Name", "QRC Type", "Description", "Scan Date" },
                      filePath, PDFReportNameHeading.CleaningReport, fDate, tDate, highChart1, highChart2, isTable);
                }
                promptBoxForPDFFileSave(filePath, fileName);
                return null;

            }
            else
                return Json(lstdata, JsonRequestBehavior.AllowGet);

        }

        #endregion Report Cleaning

        #region Report Trash Levels
        [ActionName("TrashLevels")]
        public ActionResult TrashLevels()
        {
            return PartialView("_TrashLevels");
        }

        /// <summary>Created by Bhushan Dod on 24/04/2015
        /// Get details of trash levels for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public ActionResult GetAllTrashLevels(string locationId, string userId, DateTime? fromDate, DateTime? toDate)
        {
            if (locationId == null || locationId == "0")
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    locationId = objLoginSession.LocationID.ToString();
                }
            }
            long projectId = 0;
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            if (!string.IsNullOrEmpty(locationId))
            {
                long.TryParse(locationId, out projectId);
                long.TryParse(userId, out _userId);
                //if (toDate != null)
                //{
                //    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                //        toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                //}

                ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
                //if (fromDate != null && toDate != null)
                //{
                //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
                //    {
                //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
                //    }
                //}
                lstdata = _IReportManager.GetTrashLevelList(projectId, _userId, fromDate, toDate, null).ToList();
                return Json(lstdata, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>Created by Bhushan Dod on 24/04/2015
        /// Get details of trash level reported by users for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public JsonResult GetAllReportedTrashByLevels(string locationId, string userId, DateTime? fromDate, DateTime? toDate, string name)
        {
            if (locationId == null || locationId == "0")
            {

                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    locationId = objLoginSession.LocationID.ToString();
                }
            }
            long projectId = 0;
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            if (!string.IsNullOrEmpty(locationId))
            {
                long.TryParse(locationId, out projectId);
                long.TryParse(userId, out _userId);
                //if (toDate != null)
                //{
                //    if (toDate.Value.ToLongTimeString() == "12:00:00 AM") ;
                //       // toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                //}

                ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
                //if (fromDate != null && toDate != null)
                //{
                //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
                //    {
                //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
                //    }
                //}
                lstdata = _IReportManager.GetTrashLevelList(projectId, _userId, fromDate, toDate, name).ToList(); ;
                return Json(lstdata, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        #endregion Report Trash Levels

        #region Report Trash Picked Up

        public ActionResult TrashPickedUp()
        {
            return PartialView("_TrashPickedUp");
        }

        /// <summary>Created by Bhushan Dod on 27/04/2015
        /// Get details of trash picked up for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public ActionResult GetAllTrashPickedUp(string locationId, string userId, DateTime? fromDate, DateTime? toDate)
        {
            if (locationId == null || locationId == "0")
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    locationId = objLoginSession.LocationID.ToString();
                }
            }
            long projectId = 0;
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            if (!string.IsNullOrEmpty(locationId))
            {
                long.TryParse(locationId, out projectId);
                long.TryParse(userId, out _userId);
                //if (toDate != null)
                //{
                //    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                //        toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                //}

                ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
                //if (fromDate != null && toDate != null)
                //{
                //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
                //    {
                //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
                //    }
                //}
                lstdata = _IReportManager.GetTrashPickedUpList(projectId, _userId, fromDate, toDate, null).ToList();
                return Json(lstdata, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>Created by Bhushan Dod on 24/04/2015
        /// Get details of trash level reported by users for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public JsonResult GetAllReportedTrashPickedUpByLevels(string locationId, string userId, DateTime? fromDate, DateTime? toDate, string name)
        {
            if (locationId == null || locationId == "0")
            {

                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    locationId = objLoginSession.LocationID.ToString();
                }
            }
            long projectId = 0;
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            if (!string.IsNullOrEmpty(locationId))
            {
                long.TryParse(locationId, out projectId);
                long.TryParse(userId, out _userId);
                if (toDate != null)
                {
                    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                        toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                }

                //Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
                if (fromDate != null && toDate != null)
                {
                    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
                    {
                        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
                    }
                }
                lstdata = _IReportManager.GetTrashPickedUpList(projectId, _userId, fromDate, toDate, name).ToList(); ;
                return Json(lstdata, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>Created by Bhushan Dod on 23/04/2015
        /// Get details of Get Employee Reported Trash PickedUp details
        /// </summary>
        /// <param name="qrcName,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public JsonResult GetEmployeeReportedTrashLeveldetails(string trashname, string userName, string qrctrashlevel, DateTime? fromDate, DateTime? toDate, [Bind(Prefix = "eT")] string exportingType = null,
            [Bind(Prefix = "sC")] string sortCol = null, [Bind(Prefix = "sD")] string sortDir = null, [Bind(Prefix = "h1")] string highChart1 = null, [Bind(Prefix = "h2")] string highChart2 = null, [Bind(Prefix = "iT")] bool isTable = false)
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            bool isTo12noon = false;
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isTo12noon = true;
               // toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
            }
            ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
            //if (fromDate != null && toDate != null)
            //{
            //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
            //    {
            //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
            //    }
            //}
            lstdata = _IReportManager.ReportedTrashLevelListByEmployee(objLoginSession.LocationID, trashname, userName, qrctrashlevel, fromDate, toDate, "RoutineTrashLevel").ToList();
            //Below code is for if click on FullPage PDF Export
            if (exportingType == "PDF")
            {
                DateTime FD = Convert.ToDateTime(fromDate);
                string fDate = FD.ToString("MMMM dd, yyyy hh:mm tt");
                DateTime TD = Convert.ToDateTime(toDate);
                if (isTo12noon)
                {
                    TimeSpan ts = new TimeSpan(23, 59, 59);
                    TD = TD.Date + ts;
                }
                string tDate = TD.ToString("MMMM dd, yyyy hh:mm tt");
                string fileName = StringExtensionMethods.AppendTimeStamp(ReportName.TrashLevel + ".pdf");
                string RootPath = Server.MapPath("~/");
                string RootDirectory = Server.MapPath("~/ReportPDF/FullPageReport/eScanner/");
                if (!Directory.Exists(RootDirectory))
                {
                    Directory.CreateDirectory(RootDirectory);
                }
                string filePath = RootDirectory + fileName;

                if (isTable == true)
                {
                    switch (sortCol)
                    {
                        case "User Name":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.ScanUserName).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.ScanUserName).ToList();
                            break;
                        case "QR Code ID":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.QrCodeId).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.QrCodeId).ToList();
                            break;
                        case "Trash Level":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.QrcName).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.QrcName).ToList();
                            break;
                        case "Reported Date":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.StrCreatedDate).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.StrCreatedDate).ToList();
                            break;
                        case "Trash Name":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.QrcTypeName).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.QrcTypeName).ToList();
                            break;

                        default:
                            lstdata = lstdata.OrderByDescending(o => o.StrCreatedDate).ToList();
                            break;
                    }
                    ExportPDF<ReportChart>(lstdata as IList<ReportChart>, new string[] { "QrCodeId", "ScanUserName", "QrcTypeName", "QrcName", "StrCreatedDate" },
                        new string[] { "QR Code ID", "User Name", "Trash Name", "Trash Level", "Reported Date" },
                        filePath, PDFReportNameHeading.TrashLevel, fDate, tDate, highChart1, highChart2, isTable, "Reported Trash levels for " + trashname + " by " + userName);
                }
                else
                {
                    ExportPDF<ReportChart>(lstdata as IList<ReportChart>, new string[] { "QrCodeId", "ScanUserName", "QrcTypeName", "QrcName", "StrCreatedDate" },
                        new string[] { "QR Code ID", "User Name", "Trash Name", "Trash Level", "Reported Date" },
                      filePath, PDFReportNameHeading.TrashLevel, fDate, tDate, highChart1, highChart2, isTable);
                }
                promptBoxForPDFFileSave(filePath, fileName);
                return null;

            }
            else
                return Json(lstdata, JsonRequestBehavior.AllowGet);
        }


        #endregion Report Trash Picked Up

        #region Report Trash PickedUp Details
        public ActionResult TrashPickedUpDetails()
        {
            return PartialView("_TrashPickedUpDetails");
        }

        /// <summary>Created by Bhushan Dod on 24/04/2015
        /// Get details of trash level reported by users for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public JsonResult GetAllReportedTrashPickedUpByLevelsDetails(string locationId, string userId, DateTime? fromDate, DateTime? toDate, string name, [Bind(Prefix = "eT")] string exportingType = null,
            [Bind(Prefix = "sC")] string sortCol = null, [Bind(Prefix = "sD")] string sortDir = null, [Bind(Prefix = "h1")] string highChart1 = null, [Bind(Prefix = "h2")] string highChart2 = null, [Bind(Prefix = "iT")] bool isTable = false)
        {
            if (locationId == null || locationId == "0")
            {

                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    locationId = objLoginSession.LocationID.ToString();
                }
            }
            long projectId = 0;
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            if (!string.IsNullOrEmpty(locationId))
            {
                long.TryParse(locationId, out projectId);
                long.TryParse(userId, out _userId);
                bool isTo12noon = false;
                if (toDate != null)
                {
                    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                        isTo12noon = true;
                   // toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                }
                //Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
                //if (fromDate != null && toDate != null)
                //{
                //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
                //    {
                //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
                //    }
                //}
                lstdata = _IReportManager.GetTrashPickedUpListDetails(projectId, _userId, fromDate, toDate, name).ToList();
                //Below code is for if click on FullPage PDF Export
                if (exportingType == "PDF")
                {
                    DateTime FD = Convert.ToDateTime(fromDate);
                    string fDate = FD.ToString("MMMM dd, yyyy hh:mm tt");
                    DateTime TD = Convert.ToDateTime(toDate);
                    if (isTo12noon)
                    {
                        TimeSpan ts = new TimeSpan(23, 59, 59);
                        TD = TD.Date + ts;
                    }
                    string tDate = TD.ToString("MMMM dd, yyyy hh:mm tt");
                    string fileName = StringExtensionMethods.AppendTimeStamp(ReportName.TrashPickedUp + ".pdf");
                    string RootPath = Server.MapPath("~/");
                    string RootDirectory = Server.MapPath("~/ReportPDF/FullPageReport/eScanner/");
                    if (!Directory.Exists(RootDirectory))
                    {
                        Directory.CreateDirectory(RootDirectory);
                    }
                    string filePath = RootDirectory + fileName;

                    if (isTable == true)
                    {
                        switch (sortCol)
                        {
                            case "User Name":
                                if (sortDir == "asc")
                                    lstdata = lstdata.OrderBy(o => o.ScanUserName).ToList();
                                else
                                    lstdata = lstdata.OrderByDescending(o => o.ScanUserName).ToList();
                                break;
                            case "QR Code ID":
                                if (sortDir == "asc")
                                    lstdata = lstdata.OrderBy(o => o.QrCodeId).ToList();
                                else
                                    lstdata = lstdata.OrderByDescending(o => o.QrCodeId).ToList();
                                break;
                            case "Trash Level":
                                if (sortDir == "asc")
                                    lstdata = lstdata.OrderBy(o => o.QrcName).ToList();
                                else
                                    lstdata = lstdata.OrderByDescending(o => o.QrcName).ToList();
                                break;
                            case "Picked Up Date":
                                if (sortDir == "asc")
                                    lstdata = lstdata.OrderBy(o => o.StrCreatedDate).ToList();
                                else
                                    lstdata = lstdata.OrderByDescending(o => o.StrCreatedDate).ToList();
                                break;
                            case "Trash Name":
                                if (sortDir == "asc")
                                    lstdata = lstdata.OrderBy(o => o.QrcTypeName).ToList();
                                else
                                    lstdata = lstdata.OrderByDescending(o => o.QrcTypeName).ToList();
                                break;

                            default:
                                lstdata = lstdata.OrderByDescending(o => o.StrCreatedDate).ToList();
                                break;
                        }
                        ExportPDF<ReportChart>(lstdata as IList<ReportChart>, new string[] { "QrCodeId", "ScanUserName", "QrcTypeName", "QrcName", "StrCreatedDate" },
                            new string[] { "QR Code ID", "User Name", "Trash Name", "Trash Level", "Picked Up Date" },
                            filePath, PDFReportNameHeading.TrashPickedUp, fDate, tDate, highChart1, highChart2, isTable, "Picked up trash can for " + name);
                    }
                    else
                    {
                        ExportPDF<ReportChart>(lstdata as IList<ReportChart>, new string[] { "QrCodeId", "ScanUserName", "QrcTypeName", "QrcName", "StrCreatedDate" },
                            new string[] { "QR Code ID", "User Name", "Trash Name", "Trash Level", "Picked Up Date" },
                          filePath, PDFReportNameHeading.TrashPickedUp, fDate, tDate, highChart1, highChart2, isTable);
                    }
                    promptBoxForPDFFileSave(filePath, fileName);
                    return null;

                }
                else
                    return Json(lstdata, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>Created by Bhushan Dod on 23/04/2015
        /// Get details of Get Employee Reported Trash PickedUp details
        /// </summary>
        /// <param name="qrcName,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public JsonResult GetEmployeeReportedTrashPickedUpdetails(string trashname, string userName, string qrctrashlevel, DateTime? fromDate, DateTime? toDate, [Bind(Prefix = "eT")] string exportingType = null,
            [Bind(Prefix = "sC")] string sortCol = null, [Bind(Prefix = "sD")] string sortDir = null, [Bind(Prefix = "h1")] string highChart1 = null, [Bind(Prefix = "h2")] string highChart2 = null, [Bind(Prefix = "iT")] bool isTable = false)
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            bool isTo12noon = false;
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isTo12noon = true;
                toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
            }

            //Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
            if (fromDate != null && toDate != null)
            {
                if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
                {
                    toDate = toDate.Value.AddDays(1).AddSeconds(-1);
                }
            }
            lstdata = _IReportManager.ReportedTrashLevelListByEmployee(objLoginSession.LocationID, trashname, userName, qrctrashlevel, fromDate, toDate, null).ToList();
            //Below code is for if click on FullPage PDF Export
            if (exportingType == "PDF")
            {
                DateTime FD = Convert.ToDateTime(fromDate);
                string fDate = FD.ToString("MMMM dd, yyyy hh:mm");
                DateTime TD = Convert.ToDateTime(toDate).ToClientTimeZoneinDateTime();
                if (isTo12noon)
                {
                    TimeSpan ts = new TimeSpan(11, 59, 59);
                    TD = TD.Date + ts;
                }
                string tDate = TD.ToString("MMMM dd, yyyy hh:mm");
                string fileName = StringExtensionMethods.AppendTimeStamp(ReportName.TrashPickedUp + ".pdf");
                string RootPath = Server.MapPath("~/");
                string RootDirectory = Server.MapPath("~/ReportPDF/FullPageReport/eScanner/");
                if (!Directory.Exists(RootDirectory))
                {
                    Directory.CreateDirectory(RootDirectory);
                }
                string filePath = RootDirectory + fileName;

                if (isTable == true)
                {
                    switch (sortCol)
                    {
                        case "User Name":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.ScanUserName).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.ScanUserName).ToList();
                            break;
                        case "QR Code ID":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.QrCodeId).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.QrCodeId).ToList();
                            break;
                        case "Trash Level":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.QrcName).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.QrcName).ToList();
                            break;
                        case "Reported Date":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.StrCreatedDate).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.StrCreatedDate).ToList();
                            break;
                        case "Trash Name":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.QrcTypeName).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.QrcTypeName).ToList();
                            break;

                        default:
                            lstdata = lstdata.OrderByDescending(o => o.StrCreatedDate).ToList();
                            break;
                    }
                    ExportPDF<ReportChart>(lstdata as IList<ReportChart>, new string[] { "QrCodeId", "ScanUserName", "QrcTypeName", "QrcName", "StrCreatedDate" },
                        new string[] { "QR Code ID", "User Name", "Trash Name", "Trash Level", "Reported Date" },
                        filePath, PDFReportNameHeading.TrashPickedUp, fDate, tDate, highChart1, highChart2, isTable, "Reported Trash levels for " + trashname + " by " + userName);
                }
                else
                {
                    ExportPDF<ReportChart>(lstdata as IList<ReportChart>, new string[] { "QrCodeId", "ScanUserName", "QrcTypeName", "QrcName", "StrCreatedDate" },
                        new string[] { "QR Code ID", "User Name", "Trash Name", "Trash Level", "Reported Date" },
                      filePath, PDFReportNameHeading.TrashPickedUp, fDate, tDate, highChart1, highChart2, isTable);
                }
                promptBoxForPDFFileSave(filePath, fileName);
                return null;

            }
            else
                return Json(lstdata, JsonRequestBehavior.AllowGet);
        }


        #endregion Report Trash PickedUp Details

        #region Report QRC Expiration

        public ActionResult QrcExpiration()
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            var objQRCModelList = new List<QRCModel>();
            objQRCModelList = _IReportManager.GetExpirationDateList(objLoginSession.LocationID, 128);//Hardcoded Value for first time by default Expiry Type is Warranty Exp Date
            return PartialView("_QrcExpiration", objQRCModelList);
        }
        [HttpPost]
        public JsonResult QrcExpirationList(int expiryType)
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];

            }
            var objQRCModelList = new List<QRCModel>();
            objQRCModelList = _IReportManager.GetExpirationDateList(objLoginSession.LocationID, expiryType);//Hardcoded Value for first time bt default Expiry Type Warranty Exp Date
            return Json(objQRCModelList, JsonRequestBehavior.AllowGet);

        }


        #endregion Report QRC Expiration

        #region Report QRC Owned By

        public ActionResult QrcOwnedBy()
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            var objQRCModelList = new List<QRCModel>();
            objQRCModelList = _IReportManager.PurchaseTypeList(objLoginSession.LocationID, 130);//Hardcoded Value for first time by default Expiry Type is Warranty Exp Date
            return PartialView("_QrcOwnedBy", objQRCModelList);
        }

        public ActionResult QrcOwnedList(int purchaseType, string exportingType = null, string sortCol = null, string sortDir = null, [Bind(Prefix = "iT")] bool isTable = false, [Bind(Prefix = "iD")] bool isDetail = false)
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];

            }
            var objQRCModelList = new List<QRCModel>();
            objQRCModelList = _IReportManager.PurchaseTypeList(objLoginSession.LocationID, purchaseType);//Hardcoded Value for first time bt default Expiry Type Warranty Exp Date
            if (exportingType == "PDF")
            {
                List<QRCModel> qrcModel = new List<QRCModel>();
                qrcModel = (from rw in objQRCModelList.AsEnumerable()
                            select new QRCModel()
                            {
                                QRCodeID = rw.QRCodeID,
                                QRCName = rw.QRCName,
                                SpecialNotes = rw.SpecialNotes,
                                QRCTYPECaption = rw.QRCTYPECaption,
                                AssetPicture = rw.AssetPicture,
                                VendorName = rw.VendorName,
                                UserName = rw.UserName,
                                CreatedDate = rw.CreatedDate
                            }).ToList();
                switch (sortCol)
                {
                    case "QR Code ID":
                        if (sortDir == "asc")
                            qrcModel = qrcModel.OrderBy(o => o.QRCodeID).ToList();
                        else
                            qrcModel = qrcModel.OrderByDescending(o => o.QRCodeID).ToList();
                        break;
                    case "QRC Name":
                        if (sortDir == "asc")
                            qrcModel = qrcModel.OrderBy(o => o.QRCName).ToList();
                        else
                            qrcModel = qrcModel.OrderByDescending(o => o.QRCName).ToList();
                        break;
                    case "Special Notes":
                        if (sortDir == "asc")
                            qrcModel = qrcModel.OrderBy(o => o.SpecialNotes).ToList();
                        else
                            qrcModel = qrcModel.OrderByDescending(o => o.SpecialNotes).ToList();
                        break;
                    case "QRC Type":
                        if (sortDir == "asc")
                            qrcModel = qrcModel.OrderBy(o => o.QRCTYPECaption).ToList();
                        else
                            qrcModel = qrcModel.OrderByDescending(o => o.QRCTYPECaption).ToList();
                        break;
                    case "Vendor Name":
                        if (sortDir == "asc")
                            qrcModel = qrcModel.OrderBy(o => o.VendorName).ToList();
                        else
                            qrcModel = qrcModel.OrderByDescending(o => o.VendorName).ToList();
                        break;
                    case "Created By":
                        if (sortDir == "asc")
                            qrcModel = qrcModel.OrderBy(o => o.UserName).ToList();
                        else
                            qrcModel = qrcModel.OrderByDescending(o => o.UserName).ToList();
                        break;
                    case "Created Date":
                        if (sortDir == "asc")
                            qrcModel = qrcModel.OrderBy(o => o.CreatedDate).ToList();
                        else
                            qrcModel = qrcModel.OrderByDescending(o => o.CreatedDate).ToList();
                        break;
                }
                string fileName = StringExtensionMethods.AppendTimeStamp(ReportName.QRCOwnedBy + ".pdf");
                string RootPath = Server.MapPath("~/");
                string RootDirectory = Server.MapPath("~/ReportPDF/FullPageReport/eScanner/");
                if (!Directory.Exists(RootDirectory))
                {
                    Directory.CreateDirectory(RootDirectory);
                }
                string filePath = RootDirectory + fileName;
                if (isDetail)
                {
                    ExportPDFDetail<QRCModel>(qrcModel, new string[] { "QRCodeID", "QRCName", "SpecialNotes", "QRCTYPECaption", "AssetPicture", "VendorName",
                    "UserName","CreatedDate"}, new string[] { "QR Code ID", "QRC Name", "Special Notes", "QRC Type", "Asset Picture", "Vendor Name",
                    "Created By","Created Date"}, filePath, PDFReportNameHeading.QRCOwnedBy, null, null, null, null, isTable, null, isDetail);
                }
                else
                {
                    ExportPDF<QRCModel>(qrcModel, new string[] { "QRCodeID", "QRCName", "SpecialNotes", "QRCTYPECaption", "AssetPicture", "VendorName",
                    "UserName","CreatedDate"}, new string[] { "QR Code ID", "QRC Name", "Special Notes", "QRC Type", "Asset Picture", "Vendor Name",
                    "Created By","Created Date"}, filePath, PDFReportNameHeading.QRCOwnedBy, null, null, null, null, isTable);
                }


                promptBoxForPDFFileSave(filePath, fileName);
                return null;
            }
            else
                return Json(objQRCModelList, JsonRequestBehavior.AllowGet);

        }


        #endregion Report QRC Owned By

        #region Report Damage

        public ActionResult Damage()
        {
            return PartialView("_Damage");
        }

        /// <summary>Created by Bhushan Dod on 06/05/2015
        /// Get details of Damage in veghicle for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public ActionResult GetAllDamageDetails(string locationId, string userId, DateTime? fromDate, DateTime? toDate, string name, [Bind(Prefix = "eT")] string exportingType = null,
            [Bind(Prefix = "sC")] string sortCol = null, [Bind(Prefix = "sD")] string sortDir = null, [Bind(Prefix = "h1")] string highChart1 = null, [Bind(Prefix = "h2")] string highChart2 = null, [Bind(Prefix = "iT")] bool isTable = false, [Bind(Prefix = "iD")] bool isDetail = false)
        {
            if (locationId == null || locationId == "0")
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    locationId = objLoginSession.LocationID.ToString();
                }
            }
            long projectId = 0;
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            if (!string.IsNullOrEmpty(locationId))
            {
                long.TryParse(locationId, out projectId);
                long.TryParse(userId, out _userId);
                bool isTo12noon = false;
                if (toDate != null)
                {
                    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                        isTo12noon = true;
                    //toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                }
                ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
                //if (fromDate != null && toDate != null)
                //{
                //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
                //    {
                //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
                //    }
                //}

                lstdata = _IReportManager.GetDamageVehicleList(projectId, _userId, fromDate, toDate, name).ToList();
                //shubham 07112016 code for PDF Export
                if (exportingType == "PDF")
                {
                    DateTime FD = Convert.ToDateTime(fromDate);
                    string fDate = FD.ToString("MMMM dd, yyyy hh:mm tt");
                    DateTime TD = Convert.ToDateTime(toDate);
                    if (isTo12noon)
                    {
                        TimeSpan ts = new TimeSpan(23, 59, 59);
                        TD = TD.Date + ts;
                    }

                    string tDate = TD.ToString("MMMM dd, yyyy hh:mm tt");
                    List<ReportChartForPDFExport> reportingChart = new List<ReportChartForPDFExport>();
                    reportingChart = (from rw in lstdata.AsEnumerable()
                                      select new ReportChartForPDFExport()
                                      {
                                          QRCCode = rw.KeyName,
                                          QRCName = rw.QrcName,
                                          QRCType = rw.QrcTypeName,
                                          Capture1 = rw.CapturedPicture1,
                                          Capture2 = rw.CapturedPicture2,
                                          Capture3 = rw.CapturedPicture3,
                                          Capture4 = rw.CapturedPicture4,
                                          Damage1 = rw.CroppedPicture1,
                                          Damage2 = rw.CroppedPicture2,
                                          Damage3 = rw.CroppedPicture3,
                                          Damage4 = rw.CroppedPicture4,
                                          ReportBy = rw.ScanUserName,
                                          ReportDate = rw.StrCreatedDate,
                                          CompletedDate = rw.CreatedDate// This field just added for sorting..Facing issue while pdf export. If sort is created date it sort by string and we need to sort by date.
                                      }).ToList();
                    switch (sortCol)
                    {
                        case "QR Code ID":
                            if (sortDir == "asc")
                                reportingChart = reportingChart.OrderBy(o => o.QRCCode).ToList();
                            else
                                reportingChart = reportingChart.OrderByDescending(o => o.QRCCode).ToList();
                            break;
                        case "QRC Name":
                            if (sortDir == "asc")
                                reportingChart = reportingChart.OrderBy(o => o.QRCName).ToList();
                            else
                                reportingChart = reportingChart.OrderByDescending(o => o.QRCName).ToList();
                            break;
                        case "QRC Type":
                            if (sortDir == "asc")
                                reportingChart = reportingChart.OrderBy(o => o.QRCType).ToList();
                            else
                                reportingChart = reportingChart.OrderByDescending(o => o.QRCType).ToList();
                            break;
                        case "Report By":
                            if (sortDir == "asc")
                                reportingChart = reportingChart.OrderBy(o => o.ReportBy).ToList();
                            else
                                reportingChart = reportingChart.OrderByDescending(o => o.ReportBy).ToList();
                            break;
                        case "Report Date":
                            if (sortDir == "asc")
                                reportingChart = reportingChart.OrderBy(o => o.ReportDate).ToList();
                            else
                                reportingChart = reportingChart.OrderByDescending(o => o.ReportDate).ToList();
                            break;
                    }
                    string fileName = StringExtensionMethods.AppendTimeStamp(ReportName.Damage + ".pdf");
                    string RootPath = Server.MapPath("~/");
                    string RootDirectory = Server.MapPath("~/ReportPDF/FullPageReport/eScanner/");
                    if (!Directory.Exists(RootDirectory))
                    {
                        Directory.CreateDirectory(RootDirectory);
                    }
                    string filePath = RootDirectory + fileName;
                    if (isDetail)//This condition for table print. It doesn't contains border and timing
                    {
                        ExportPDFDetail<ReportChartForPDFExport>(reportingChart, new string[] { "QRCCode", "QRCName", "Capture1", "Capture2", "Capture3", "Capture4",
                    "Damage1","Damage2","Damage3","Damage4","ReportBy","ReportDate"}, new string[] { "QR Code ID", "QRCName", "Capture1", "Capture2", "Capture3", "Capture4",
                    "Damage1","Damage2","Damage3","Damage4","Report By","Report Date"},
                    filePath, PDFReportNameHeading.Damage, fDate, tDate, highChart1, highChart2, isTable, "Damage details for " + name, isDetail);
                    }
                    else
                    {
                        ExportPDF<ReportChartForPDFExport>(reportingChart, new string[] { "QRCCode", "QRCName", "Capture1", "Capture2", "Capture3", "Capture4",
                    "Damage1","Damage2","Damage3","Damage4","ReportBy","ReportDate"}, new string[] { "QR Code ID", "QRCName", "Capture1", "Capture2", "Capture3", "Capture4",
                    "Damage1","Damage2","Damage3","Damage4","Report By","Report Date"},
                    filePath, PDFReportNameHeading.Damage, fDate, tDate, highChart1, highChart2, isTable, "Damage details for " + name);
                    }


                    promptBoxForPDFFileSave(filePath, fileName);
                    return null;

                }
                else
                    return Json(lstdata, JsonRequestBehavior.AllowGet);
                //end 07112016
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>Created by Bhushan Dod on 06/05/2015
        /// Get details of Damage in veghicle for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public JsonResult GetEmployeeDamageReportedDetails(string trashname, string userName, string qrctrashlevel, DateTime? fromDate, DateTime? toDate)
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            if (toDate != null)
                toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
            lstdata = _IReportManager.ReportedTrashLevelListByEmployee(objLoginSession.LocationID, trashname, userName, qrctrashlevel, fromDate, toDate, null).ToList();

            return Json(lstdata, JsonRequestBehavior.AllowGet);
        }

        #endregion Report Damage

        #region Report QRC Amounts Created
        public ActionResult QrcDetails()
        {
            return PartialView("_QrcDetails");
        }

        /// <summary>Created by Bhushan Dod on 11/05/2015
        /// Get details of all QRC 
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public ActionResult GetAllQrcDetails(string locationId, string userId, string name, [Bind(Prefix = "eT")] string exportingType = null,
            [Bind(Prefix = "sC")] string sortCol = null, [Bind(Prefix = "sD")] string sortDir = null, [Bind(Prefix = "h1")] string highChart1 = null, [Bind(Prefix = "h2")] string highChart2 = null, [Bind(Prefix = "iT")] bool isTable = false, [Bind(Prefix = "iD")] bool isDetail = false)
        {
            if (locationId == null || locationId == "0")
            {

                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    locationId = objLoginSession.LocationID.ToString();
                }
            }
            long projectId = 0;
            long _userId = 0;

            if (!string.IsNullOrEmpty(locationId))
            {
                long.TryParse(locationId, out projectId);
                long.TryParse(userId, out _userId);
                var lstdata = _IReportManager.GetDetailsOfAllQrc(projectId, _userId, name).ToList();
                if (exportingType == "PDF")
                {
                    //DateTime FD = Convert.ToDateTime(fromDate);
                    //string fDate = FD.ToString("MMMM dd, yyyy hh:mm");
                    //DateTime TD = Convert.ToDateTime(toDate);
                    //string tDate = TD.ToString("MMMM dd, yyyy hh:mm");
                    switch (sortCol)
                    {
                        case "QR Code ID":
                            {
                                if (sortDir == "asc")
                                    lstdata = lstdata.OrderBy(o => o.KeyName).ToList();
                                else
                                    lstdata = lstdata.OrderByDescending(o => o.KeyName).ToList();
                                break;
                            }
                        case "QRC Name":
                            {
                                if (sortDir == "asc")
                                    lstdata = lstdata.OrderBy(o => o.QrcName).ToList();
                                else
                                    lstdata = lstdata.OrderByDescending(o => o.QrcName).ToList();
                                break;
                            }
                        case "Special Notes":
                            {
                                if (sortDir == "asc")
                                    lstdata = lstdata.OrderBy(o => o.QrcTypeName).ToList();
                                else
                                    lstdata = lstdata.OrderByDescending(o => o.QrcTypeName).ToList();
                                break;
                            }
                        case "Asset Picture":
                            {
                                if (sortDir == "asc")
                                    lstdata = lstdata.OrderBy(o => o.CroppedPicture).ToList();
                                else
                                    lstdata = lstdata.OrderByDescending(o => o.CroppedPicture).ToList();
                                break;
                            }
                        case "Created By":
                            {
                                if (sortDir == "asc")
                                    lstdata = lstdata.OrderBy(o => o.ScanUserName).ToList();
                                else
                                    lstdata = lstdata.OrderByDescending(o => o.ScanUserName).ToList();
                                break;
                            }
                        case "Created Date":
                            {
                                if (sortDir == "asc")
                                    lstdata = lstdata.OrderBy(o => o.CreatedDate).ToList();
                                else
                                    lstdata = lstdata.OrderByDescending(o => o.CreatedDate).ToList();
                                break;
                            }
                        default:
                            {
                                lstdata = lstdata.OrderByDescending(o => o.CreatedDate).ToList();
                                break;
                            }
                    }
                    string fileName = StringExtensionMethods.AppendTimeStamp(ReportName.QRCAmount + ".pdf");
                    string RootPath = Server.MapPath("~/");
                    string RootDirectory = Server.MapPath("~/ReportPDF/FullPageReport/eScanner/");
                    if (!Directory.Exists(RootDirectory))
                    {
                        Directory.CreateDirectory(RootDirectory);
                    }
                    string filePath = RootDirectory + fileName;
                    if (isDetail)
                    {
                        ExportPDFDetail<ReportChart>(lstdata, new string[] { "KeyName", "QrcName", "QrcTypeName", "CroppedPicture", "ScanUserName", "StrCreatedDate"
                    }, new string[] { "QR Code ID", "QRC Name", "Special Notes", "Asset Picture", "Created By", "Created Date"
                    }, filePath, PDFReportNameHeading.QRCAmount, null, null, highChart1, highChart2, isTable, "QRC details for " + name, isDetail);
                    }
                    else
                    {
                        ExportPDF<ReportChart>(lstdata, new string[] { "KeyName", "QrcName", "QrcTypeName", "CroppedPicture", "ScanUserName", "StrCreatedDate"
                    }, new string[] { "QR Code ID", "QRC Name", "Special Notes", "Asset Picture", "Created By", "Created Date"
                    }, filePath, PDFReportNameHeading.QRCAmount, null, null, highChart1, highChart2, isTable, "QRC details for " + name);
                    }


                    promptBoxForPDFFileSave(filePath, fileName);
                    return null;

                }
                else
                    return Json(lstdata, JsonRequestBehavior.AllowGet);
                //end 07112016
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>Created by Bhushan Dod on 23/04/2015
        /// Get details of Get Employee Reported Trash PickedUp details
        /// </summary>
        /// <param name="qrcName,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        //public JsonResult GetEmployeeReportedTrashPickedUpdetails(string trashname, string userName, string qrctrashlevel, DateTime? fromDate, DateTime? toDate)
        //{
        //    eTracLoginModel objLoginSession = new eTracLoginModel();
        //    if ((eTracLoginModel)Session["eTrac"] != null)
        //    {
        //        objLoginSession = (eTracLoginModel)Session["eTrac"];
        //    }
        //    long _userId = 0;
        //    List<ReportChart> lstdata = new List<ReportChart>();
        //    // long.TryParse(userId, out _userId);
        //    lstdata = _IReportManager.ReportedTrashLevelListByEmployee(objLoginSession.LocationID, trashname, userName, qrctrashlevel, fromDate, toDate, null).ToList();

        //    return Json(lstdata, JsonRequestBehavior.AllowGet);
        //}


        #endregion Report QRC Amounts Created

        #endregion eScanner

        #region Work Order

        #region Report Work Order Issued
        [ActionName("IssuedWorkOrder")]
        public ActionResult ReportWorkOrderIssued()
        {
            return PartialView("_ReportWorkOrderIssued");
        }

        /// <summary>Created by Bhushan Dod on 28/07/2015
        /// Get details of Work Order Issued for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public ActionResult WorkOrderIssued(string locationId, string userId, DateTime? fromDate, DateTime? toDate)
        {
            if (locationId == null || locationId == "0")
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    locationId = objLoginSession.LocationID.ToString();
                }
            }
            long projectId = 0;
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            if (!string.IsNullOrEmpty(locationId))
            {
                long.TryParse(locationId, out projectId);
                long.TryParse(userId, out _userId);
                //if (toDate != null)
                //    toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                //if (toDate != null)
                //{
                //    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                //        toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                //}
                //if (fromDate != null && toDate != null)
                //{
                //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
                //    {
                //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
                //    }
                //}
                lstdata = _IReportManager.GetIssuedWorkOrder(projectId,_userId, fromDate, toDate, null).ToList();
                return Json(lstdata, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>Created by Bhushan Dod on 16/04/2015
        /// Get details of work order issued
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public JsonResult GetAllWorkOrderIssued(string projectType, long userId, DateTime? fromDate, DateTime? toDate, [Bind(Prefix = "eT")] string exportingType = null,
             [Bind(Prefix = "sC")] string sortCol = null, [Bind(Prefix = "sD")] string sortDir = null, [Bind(Prefix = "h1")] string highChart1 = null,
             [Bind(Prefix = "h2")] string highChart2 = null, [Bind(Prefix = "iT")] bool isTable = false)
        {

            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = fromDate ?? clientdt.Date;
            DateTime _toDate = toDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isUTCDay = false;
            }
            if (_fromDate != null && _toDate != null)
            {
                ////if interval date come then need to fetch record till midnight of todate day
                if ((_fromDate.Date != _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM") && isUTCDay == false)
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
                if ((_fromDate.Date == _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM"))
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
            }
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];

                var objWorkOrderIssueedModelList = new List<WorkOrderIssueedModel>();
                //if (toDate != null)
                //    toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                //DateTime _fromDate = fromDate ?? DateTime.UtcNow.Date;
                //DateTime _toDate = toDate ?? DateTime.UtcNow;
                bool isTo12noon = false;
                if (toDate != null)
                {
                    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    {
                        isTo12noon = true;
                       // toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                    }
                }
                ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
                //if (fromDate != null && toDate != null)
                //{
                //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
                //    {
                //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
                //    }
                //}

                //DateTime _fromDate = fromDate ?? DateTime.UtcNow.Date;
                //DateTime _toDate = toDate ?? DateTime.UtcNow;
                //Converted to UTC because datetime in utc in db.
                _fromDate = _fromDate.ConvertClientTZtoUTC();
                _toDate = _toDate.ConvertClientTZtoUTC();
                objWorkOrderIssueedModelList = _IReportManager.GetWorkOrderListForLocation(objLoginSession.LocationID, userId, objLoginSession.UserId, _fromDate.ToString(), _toDate.ToString(), projectType, "");

                //Below code is for if click on FullPage PDF Export
                if (exportingType == "PDF")
                {
                    DateTime FD = Convert.ToDateTime(fromDate);
                    string fDate = FD.ToString("MMMM dd, yyyy hh:mm tt");

                    DateTime TD = Convert.ToDateTime(toDate);
                    if (isTo12noon)
                    {
                        TimeSpan ts = new TimeSpan(23, 59, 59);
                        TD = TD.Date + ts;
                    }
                    string tDate = TD.ToString("MMMM dd, yyyy hh:mm tt");


                    string fileName = StringExtensionMethods.AppendTimeStamp(ReportName.WorkOrderIssued + ".pdf");
                    string RootPath = Server.MapPath("~/");
                    string RootDirectory = Server.MapPath("~/ReportPDF/FullPageReport/eMaintenance/");
                    if (!Directory.Exists(RootDirectory))
                    {
                        Directory.CreateDirectory(RootDirectory);
                    }
                    string filePath = RootDirectory + fileName;

                    if (isTable == true)
                    {
                        switch (sortCol)
                        {
                            case "CodeNo":
                                if (sortDir == "asc")
                                    objWorkOrderIssueedModelList = objWorkOrderIssueedModelList.OrderBy(o => o.CodeID).ToList();
                                else
                                    objWorkOrderIssueedModelList = objWorkOrderIssueedModelList.OrderByDescending(o => o.CodeID).ToList();
                                break;
                            case "PriorityLevel":
                                if (sortDir == "asc")
                                    objWorkOrderIssueedModelList = objWorkOrderIssueedModelList.OrderBy(o => o.PriorityLevel).ToList();
                                else
                                    objWorkOrderIssueedModelList = objWorkOrderIssueedModelList.OrderByDescending(o => o.PriorityLevel).ToList();
                                break;
                            case "ProjectDesc":
                                if (sortDir == "asc")
                                    objWorkOrderIssueedModelList = objWorkOrderIssueedModelList.OrderBy(o => o.ProjectDesc).ToList();
                                else
                                    objWorkOrderIssueedModelList = objWorkOrderIssueedModelList.OrderByDescending(o => o.ProjectDesc).ToList();
                                break;
                            case "ProblemDesc":
                                if (sortDir == "asc")
                                    objWorkOrderIssueedModelList = objWorkOrderIssueedModelList.OrderBy(o => o.ProblemDesc).ToList();
                                else
                                    objWorkOrderIssueedModelList = objWorkOrderIssueedModelList.OrderByDescending(o => o.ProblemDesc).ToList();
                                break;
                            case "RequestStatus":
                                if (sortDir == "asc")
                                    objWorkOrderIssueedModelList = objWorkOrderIssueedModelList.OrderBy(o => o.WorkRequestStatus).ToList();
                                else
                                    objWorkOrderIssueedModelList = objWorkOrderIssueedModelList.OrderByDescending(o => o.WorkRequestStatus).ToList();
                                break;
                            case "RequestBy":
                                if (sortDir == "asc")
                                    objWorkOrderIssueedModelList = objWorkOrderIssueedModelList.OrderBy(o => o.RequestBy).ToList();
                                else
                                    objWorkOrderIssueedModelList = objWorkOrderIssueedModelList.OrderByDescending(o => o.RequestBy).ToList();
                                break;
                        }
                        if (projectType.Trim() != "Work Request")
                        {
                            ExportPDF<WorkOrderIssueedModel>(objWorkOrderIssueedModelList as IList<WorkOrderIssueedModel>, new string[] { "CodeID", "PriorityLevel", "ProjectDesc", "WorkRequestStatus", "RequestBy", "AssignTo",
                    "AssignBy","CreatedDate"}, new string[] { "Code No", "Priority Level", "Project Desc", "Request Status", "Request By", "Assign To",
                    "Assign By","Issued Date"}, filePath, "Work Order Issued Report  ", fDate, tDate, highChart1, highChart2, isTable, projectType + " Details");
                        }
                        else
                        {
                            ExportPDF<WorkOrderIssueedModel>(objWorkOrderIssueedModelList as IList<WorkOrderIssueedModel>, new string[] { "CodeID", "WorkRequestType","PriorityLevel", "ProblemDesc", "WorkRequestStatus", "RequestBy", "AssignTo",
                    "AssignBy","CreatedDate"}, new string[] { "Code No","Request Type", "Priority Level", "Problem Desc", "Request Status", "Request By", "Assign To",
                    "Assign By","Issued Date"}, filePath, "Work Order Issued Report  ", fDate, tDate, highChart1, highChart2, isTable, projectType + " Details");

                        }

                    }
                    else
                    {
                        ExportPDF<WorkOrderIssueedModel>(objWorkOrderIssueedModelList as IList<WorkOrderIssueedModel>, new string[] { "CodeID", "PriorityLevel", "ProjectDesc", "WorkRequestStatus", "RequestBy", "AssignTo",
                    "AssignBy","CreatedDate"}, new string[] { "Code No", "Priority Level", "Project Desc", "Request Status", "Request By", "Assign To",
                    "Assign By","Issued Date"}, filePath, "Work Order Issued Report  ", fDate, tDate, highChart1, highChart2, isTable);
                    }
                    promptBoxForPDFFileSave(filePath, fileName);
                    return null;

                }
                else
                    return Json(objWorkOrderIssueedModelList, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }


        }

        #endregion Report Work Order Issued

        #region Report FixedTime
        [ActionName("WorkOrderFixedTime")]
        public ActionResult WorkOrderFixedTime()
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }

            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate =  clientdt.Date;
            DateTime _toDate =  clientdt.AddDays(1).Date;
            _fromDate = _fromDate.ConvertClientTZtoUTC();
            _toDate = _toDate.ConvertClientTZtoUTC();
            var objWorkOrderIssueedModelList = new List<WorkOrderIssueedModel>();
            objWorkOrderIssueedModelList = _IReportManager.GetWorkOrderIssuedListFixedTime(objLoginSession.UserId ,objLoginSession.LocationID, _fromDate.ToString(), _toDate.ToString(), null, "");
            return PartialView("_WorkOrderFixedTime", objWorkOrderIssueedModelList);

        }
        /// <summary>Created by Bhushan Dod on 16/04/2015
        /// Get details of work order issued
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public JsonResult GetAllWorkOrderFixed(string projectType, DateTime? fromDate, DateTime? toDate)
        {

            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            var objWorkOrderIssueedModelList = new List<WorkOrderIssueedModel>();
            //if (toDate != null)
            //{
            //    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
            //        toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
            //}
            ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
            //if (fromDate != null && toDate != null)
            //{
            //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
            //    {
            //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
            //    }
            //}
            ////Converted to UTC because datetime in utc in db.
            //if (fromDate != null)
            //    fromDate = fromDate.Value.ToClientTimeZoneinDateTimeReports();
            //if (toDate != null)
            //    toDate = toDate.Value.ToClientTimeZoneinDateTimeReports();
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = fromDate ?? clientdt.Date;
            DateTime _toDate = toDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isUTCDay = false;
            }
            if (_fromDate != null && _toDate != null)
            {
                ////if interval date come then need to fetch record till midnight of todate day
                if ((_fromDate.Date != _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM") && isUTCDay == false)
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
                if ((_fromDate.Date == _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM"))
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
            }
            _fromDate = _fromDate.ConvertClientTZtoUTC();
            _toDate = _toDate.ConvertClientTZtoUTC();
            objWorkOrderIssueedModelList = _IReportManager.GetWorkOrderIssuedListFixedTime(objLoginSession.UserId,objLoginSession.LocationID, _fromDate.ToString(), _toDate.ToString(), Convert.ToInt32(projectType), "");

            return Json(objWorkOrderIssueedModelList, JsonRequestBehavior.AllowGet);
        }

        #endregion Report FixedTime

        #region Report Work Order Issued for assigned location item
        [ActionName("AssetsWorkOrder")]
        public ActionResult ReportWorkOrderIssuedAssignedLocItem()
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];

            }
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = clientdt.Date;
            DateTime _toDate = clientdt.AddDays(1).Date;
            _fromDate = _fromDate.ConvertClientTZtoUTC();
            _toDate = _toDate.ConvertClientTZtoUTC();
            var objWorkOrderIssueedModelList = new List<WorkOrderIssueedModel>();
            objWorkOrderIssueedModelList = _IReportManager.GetWorkOrderAssignedListForLocationItem(objLoginSession.UserId, objLoginSession.LocationID, _fromDate.ToString(), _toDate.ToString(), null, null, null, null, null, null, "");
            return PartialView("_ReportWorkOrderIssuedAssignedItem", objWorkOrderIssueedModelList);

        }

        /// <summary>Created by Bhushan Dod on 18/05/2015
        /// Get details of work order issued
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public JsonResult GetAllWorkOrderIssuedAssignedLocItem(string qrcId, DateTime? fromDate, DateTime? toDate, long? ReqType, int? WorkRequestProjectType, long? safetyHuzzard, long? priorityLevel, long? userId)
        {

            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            var objWorkOrderIssueedModelList = new List<WorkOrderIssueedModel>();
            //if (toDate != null)
            //{
            //    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
            //        toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
            //}
            ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
            //if (fromDate != null && toDate != null)
            //{
            //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
            //    {
            //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
            //    }
            //}
            ////Converted to UTC because datetime in utc in db.
            //if (fromDate != null)
            //    fromDate = fromDate.Value.ToClientTimeZoneinDateTimeReports();
            //if (toDate != null)
            //    toDate = toDate.Value.ToClientTimeZoneinDateTimeReports();
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = fromDate ?? clientdt.Date;
            DateTime _toDate = toDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isUTCDay = false;
            }
            if (_fromDate != null && _toDate != null)
            {
                ////if interval date come then need to fetch record till midnight of todate day
                if ((_fromDate.Date != _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM") && isUTCDay == false)
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
                if ((_fromDate.Date == _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM"))
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
            }
            _fromDate = _fromDate.ConvertClientTZtoUTC();
            _toDate = _toDate.ConvertClientTZtoUTC();
            objWorkOrderIssueedModelList = _IReportManager.GetWorkOrderAssignedListForLocationItem(objLoginSession.UserId, objLoginSession.LocationID, fromDate.ToString(), toDate.ToString(), Convert.ToInt32(qrcId), ReqType, WorkRequestProjectType, safetyHuzzard, priorityLevel, userId, "");

            return Json(objWorkOrderIssueedModelList, JsonRequestBehavior.AllowGet);

        }

        #endregion  Report Work Order Issued for assigned location item

        #region Report Work Order In Progress
        [ActionName("WorkOrderInProgress")]
        public ActionResult ReportWorkOrderInProgress()
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];

            }
            var objWorkOrderIssueedModelList = new List<WorkOrderIssueedModel>();
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = clientdt.Date;
            DateTime _toDate = clientdt.AddDays(1).Date;
            _fromDate = _fromDate.ConvertClientTZtoUTC();
            _toDate = _toDate.ConvertClientTZtoUTC();
            objWorkOrderIssueedModelList = _IReportManager.GetWorkOrderInProgressListForLocation(objLoginSession.UserId, objLoginSession.LocationID, _fromDate.ToString(), _toDate.ToString(), null, null, "");
            return PartialView("_ReportWorkOrderInProgress", objWorkOrderIssueedModelList);

        }
        /// <summary>Created by Bhushan Dod on 16/04/2015
        /// Get details of work order issued
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public JsonResult GetAllWorkOrderInProgress(string projectType, DateTime? fromDate, DateTime? toDate, long? userId)
        {

            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            var objWorkOrderIssueedModelList = new List<WorkOrderIssueedModel>();
            //if (toDate != null)
            //{
            //    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
            //        toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
            //}
            ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
            //if (fromDate != null && toDate != null)
            //{
            //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
            //    {
            //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
            //    }
            //}
            //DateTime _fromDate = fromDate ?? DateTime.UtcNow.Date;
            //DateTime _toDate = toDate ?? DateTime.UtcNow;

            ////Converted to UTC because datetime in utc in db.
            //if (_fromDate != null)
            //    _fromDate = _fromDate.ToClientTimeZoneinDateTimeReports();
            //if (_toDate != null)
            //    _toDate = _toDate.ToClientTimeZoneinDateTimeReports();

            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = fromDate ?? clientdt.Date;
            DateTime _toDate = toDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isUTCDay = false;
            }
            if (_fromDate != null && _toDate != null)
            {
                ////if interval date come then need to fetch record till midnight of todate day
                if ((_fromDate.Date != _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM") && isUTCDay == false)
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
                if ((_fromDate.Date == _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM"))
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
            }
            _fromDate = _fromDate.ConvertClientTZtoUTC();
            _toDate = _toDate.ConvertClientTZtoUTC();
            objWorkOrderIssueedModelList = _IReportManager.GetWorkOrderInProgressListForLocation(objLoginSession.UserId,objLoginSession.LocationID, _fromDate.ToString(), _toDate.ToString(), Convert.ToInt32(projectType), userId, "");

            return Json(objWorkOrderIssueedModelList, JsonRequestBehavior.AllowGet);
        }

        #endregion Report Work Order In Progress

        #region Report Work Order Missed Time
        [ActionName("WorkOrderMissedTime")]
        public ActionResult ReportWorkOrderMissedTime()
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];

            }
            var objWorkOrderIssueedModelList = new List<WorkOrderIssueedModel>();
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = clientdt.Date;
            DateTime _toDate = clientdt.AddDays(1).Date;
            _fromDate = _fromDate.ConvertClientTZtoUTC();
            _toDate = _toDate.ConvertClientTZtoUTC();
            objWorkOrderIssueedModelList = _IReportManager.GetWorkOrderMissedTime(objLoginSession.UserId,objLoginSession.LocationID, _fromDate.ToString(), _toDate.ToString(), null, null, null, "");
            return PartialView("_ReportWorkOrderrMissedTime", objWorkOrderIssueedModelList);

        }
        /// <summary>Created by Bhushan Dod on 16/04/2015
        /// Get details of work order issued
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public JsonResult GetAllWorkOrderMissedTime(string projectType, DateTime? fromDate, DateTime? toDate, long? priorityLevel, long? userId)
        {

            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            var objWorkOrderIssueedModelList = new List<WorkOrderIssueedModel>();
            //if (toDate != null)
            //{
            //    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
            //        toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
            //}
            ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
            //if (fromDate != null && toDate != null)
            //{
            //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
            //    {
            //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
            //    }
            //}
            ////Converted to UTC because datetime in utc in db.
            //if (fromDate != null)
            //    fromDate = fromDate.Value.ToClientTimeZoneinDateTimeReports();
            //if (toDate != null)
            //    toDate = toDate.Value.ToClientTimeZoneinDateTimeReports();

            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = fromDate ?? clientdt.Date;
            DateTime _toDate = toDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isUTCDay = false;
            }
            if (_fromDate != null && _toDate != null)
            {
                ////if interval date come then need to fetch record till midnight of todate day
                if ((_fromDate.Date != _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM") && isUTCDay == false)
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
                if ((_fromDate.Date == _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM"))
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
            }
            _fromDate = _fromDate.ConvertClientTZtoUTC();
            _toDate = _toDate.ConvertClientTZtoUTC();
            objWorkOrderIssueedModelList = _IReportManager.GetWorkOrderMissedTime(objLoginSession.UserId,objLoginSession.LocationID, _fromDate.ToString(), _toDate.ToString(), Convert.ToInt32(projectType), priorityLevel, userId, "");

            return Json(objWorkOrderIssueedModelList, JsonRequestBehavior.AllowGet);

        }

        #endregion Report Work Order Missed Time

        #region Report Work Order Completed
        [ActionName("CompletedWorkOrder")]
        public ActionResult GetAllCompletedWorkOrder()
        {
            return PartialView("_GetCompletedWorkOrder");
        }
        /// <summary>Created by Bhushan Dod on 28/07/2015
        /// Get details of Work Order Type for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>

        public ActionResult WorkOrderCompleted(string locationId, string userId, DateTime? fromDate, DateTime? toDate)
        {
            if (locationId == null || locationId == "0")
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    locationId = objLoginSession.LocationID.ToString();
                }
            }
            long projectId = 0;
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            if (!string.IsNullOrEmpty(locationId))
            {
                long.TryParse(locationId, out projectId);
                long.TryParse(userId, out _userId);
                //if (toDate != null)
                //{
                //    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                //        toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                //}

                ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
                //if (fromDate != null && toDate != null)
                //{
                //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
                //    {
                //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
                //    }
                //}
                lstdata = _IReportManager.GetCompletedWorkOrder(projectId, _userId, fromDate, toDate, null).ToList();
                return Json(lstdata, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>Created by Bhushan Dod on 28/07/2015
        /// Get details of type work order  for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public JsonResult GetAllWorkOrderByType(string locationId, string userId, DateTime? fromDate, DateTime? toDate, string name)
        {
            if (locationId == null || locationId == "0")
            {

                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    locationId = objLoginSession.LocationID.ToString();
                }
            }
            long projectId = 0;
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            if (!string.IsNullOrEmpty(locationId))
            {
                long.TryParse(locationId, out projectId);
                long.TryParse(userId, out _userId);
                //if (toDate != null)
                //{
                //    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                //        toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                //}
                lstdata = _IReportManager.GetCompletedWorkOrder(projectId, _userId, fromDate, toDate, name).ToList(); ;
                return Json(lstdata, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>Created by Bhushan Dod on 28/07/2015
        /// Get details of work order by Employee wise        /// </summary>
        /// <param name="qrcName,UserId,FromDate,ToDate"></param>
        /// <returns></returns>  /// This function directly copied from QRC Scan log for better understanding refer QRC Scan log
        public JsonResult GetEmployeeWorkOrderdetails(string qrcName, string userName, DateTime? fromDate, DateTime? toDate, [Bind(Prefix = "eT")] string exportingType = null,
            [Bind(Prefix = "sC")] string sortCol = null, [Bind(Prefix = "sD")] string sortDir = null, [Bind(Prefix = "h1")] string highChart1 = null,
            [Bind(Prefix = "h2")] string highChart2 = null, [Bind(Prefix = "iT")] bool isTable = false)
        {

            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            bool isTo12noon = false;
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isTo12noon = true;
               // toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
            }
            ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
            //if (fromDate != null && toDate != null)
            //{
            //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
            //    {
            //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
            //    }
            //}
            lstdata = _IReportManager.WorkOrderListByEmployee(objLoginSession.LocationID, qrcName, userName, fromDate, toDate).ToList();

            if (exportingType == "PDF")
            {
                DateTime FD = Convert.ToDateTime(fromDate);
                string fDate = FD.ToString("MMMM dd, yyyy hh:mm tt");

                DateTime TD = Convert.ToDateTime(toDate);
                if (isTo12noon)
                {
                    TimeSpan ts = new TimeSpan(23, 59, 59);
                    TD = TD.Date + ts;
                }
                string tDate = TD.ToString("MMMM dd, yyyy hh:mm tt");
                string fileName = StringExtensionMethods.AppendTimeStamp(ReportName.CompletedWorkOrder + ".pdf");
                string RootPath = Server.MapPath("~/");
                string RootDirectory = Server.MapPath("~/ReportPDF/FullPageReport/eMaintenance/");
                if (!Directory.Exists(RootDirectory))
                {
                    Directory.CreateDirectory(RootDirectory);
                }
                string filePath = RootDirectory + fileName;

                if (isTable == true)
                {
                    switch (sortCol)
                    {
                        case "Code No":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.WorkOrderCode).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.WorkOrderCode).ToList();
                            break;
                        case "Project Description":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.ProjectDescription).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.ProjectDescription).ToList();
                            break;
                        case "User Name":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.ScanUserName).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.ScanUserName).ToList();
                            break;
                        case "Work Order Type":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.QrcName).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.QrcName).ToList();
                            break;
                        case "Completed On":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.CreatedDate).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.CreatedDate).ToList();
                            break;
                        case "Problem Description":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.ProblemDescription).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.ProblemDescription).ToList();
                            break;
                        default:
                            lstdata = lstdata.OrderByDescending(o => o.StrCreatedDate).ToList();
                            break;
                    }
                    if (qrcName.Trim() != "Work Request")
                    {
                        ExportPDF<ReportChart>(lstdata, new string[] { "WorkOrderCode", "ProjectDescription", "ScanUserName", "QrcName", "StrCreatedDate" },
                            new string[] { "Code No", "Project Description", "User Name", "Work Order Type", "Completed On" },
                            filePath, PDFReportNameHeading.AssignedWorkOrder, fDate, tDate, highChart1, highChart2, isTable, (qrcName + " completed by  " + userName));
                    }
                    else
                    {
                        ExportPDF<ReportChart>(lstdata, new string[] { "WorkOrderCode", "ProblemDescription", "ScanUserName", "QrcName", "StrCreatedDate" },
                            new string[] { "Code No", "Problem Description", "User Name", "Work Order Type", "Completed On" },
                            filePath, PDFReportNameHeading.AssignedWorkOrder, fDate, tDate, highChart1, highChart2, isTable, (qrcName + " completed by " + userName));
                    }
                }
                else
                {
                    ExportPDF<ReportChart>(lstdata, new string[] { "WorkOrderCode", "ProjectDescription", "ScanUserName", "QrcName", "StrCreatedDate" },
                        new string[] { "Code No", "Project Description", "User Name", "Work Order Type", "Completed On" },
                    filePath, PDFReportNameHeading.AssignedWorkOrder, fDate, tDate, highChart1, highChart2, isTable);
                }

                promptBoxForPDFFileSave(filePath, fileName);
                return null;

            }
            else
                return Json(lstdata, JsonRequestBehavior.AllowGet);
        }

        #endregion Report Work Order Completed

        #region Report Work Order Assigned
        [ActionName("AssignedWorkOrder")]
        public ActionResult GetAllAssignedWorkOrder()
        {
            return PartialView("_GetAssignedWorkOrder");
        }
        /// <summary>Created by Bhushan Dod on 28/07/2015
        /// Get details of Work Order Type for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public ActionResult WorkOrderAssigned(string locationId, string userId, DateTime? fromDate, DateTime? toDate)
        {
            if (locationId == null || locationId == "0")
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    locationId = objLoginSession.LocationID.ToString();
                }
            }
            long projectId = 0;
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            if (!string.IsNullOrEmpty(locationId))
            {
                long.TryParse(locationId, out projectId);
                long.TryParse(userId, out _userId);

                //if (toDate != null)
                //{
                //    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")

                //        toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                //}

                ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
                //if (fromDate != null && toDate != null)
                //{
                //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
                //    {
                //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
                //    }
                //}

                //DateTime _fromDate = fromDate ?? DateTime.UtcNow.Date;
                //DateTime _toDate = toDate ?? DateTime.UtcNow;
                lstdata = _IReportManager.GetAssignedWorkOrder(projectId, _userId, fromDate, toDate, null).ToList();
                return Json(lstdata, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>Created by Bhushan Dod on 28/07/2015
        /// Get details of type work order  for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public JsonResult GetAllAssignedWorkOrderByType(string locationId, string userId, DateTime? fromDate, DateTime? toDate, string name)
        {
            if (locationId == null || locationId == "0")
            {

                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    locationId = objLoginSession.LocationID.ToString();
                }
            }
            long projectId = 0;
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            if (!string.IsNullOrEmpty(locationId))
            {
                long.TryParse(locationId, out projectId);
                long.TryParse(userId, out _userId);
                //if (toDate != null)
                //{
                //    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                //        toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                //}

                lstdata = _IReportManager.GetAssignedWorkOrder(projectId, _userId, fromDate, toDate, name).ToList(); ;
                return Json(lstdata, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>Created by Bhushan Dod on 28/07/2015
        /// Get details of work order by Employee wise        /// </summary>
        /// <param name="qrcName,UserId,FromDate,ToDate"></param>
        /// <returns></returns>  /// This function directly copied from QRC Scan log for better understanding refer QRC Scan log
        public JsonResult GetAssignedEmployeeWorkOrderdetails(string qrcName, string userName, DateTime? fromDate, DateTime? toDate, [Bind(Prefix = "eT")] string exportingType = null,
            [Bind(Prefix = "sC")] string sortCol = null, [Bind(Prefix = "sD")] string sortDir = null, [Bind(Prefix = "h1")] string highChart1 = null,
            [Bind(Prefix = "h2")] string highChart2 = null, [Bind(Prefix = "iT")] bool isTable = false)
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            bool isTo12noon = false;
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isTo12noon = true;
                    //toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
            }

            ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
            //if (fromDate != null && toDate != null)
            //{
            //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
            //    {
            //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
            //    }
            //}
            lstdata = _IReportManager.AssignedWorkOrderListByEmployee(objLoginSession.LocationID, qrcName, userName, fromDate, toDate).ToList();
            //shubham 07112016 code for PDF Export
            if (exportingType == "PDF")
            {

                DateTime FD = Convert.ToDateTime(fromDate);
                string fDate = FD.ToString("MMMM dd, yyyy hh:mm tt");

                DateTime TD = Convert.ToDateTime(toDate);
                if (isTo12noon)
                {
                    TimeSpan ts = new TimeSpan(23, 59, 59);
                    TD = TD.Date + ts;
                }
                string tDate = TD.ToString("MMMM dd, yyyy hh:mm tt");
                string fileName = StringExtensionMethods.AppendTimeStamp(ReportName.AssignedWorkOrder + ".pdf");
                string RootPath = Server.MapPath("~/");
                string RootDirectory = Server.MapPath("~/ReportPDF/FullPageReport/eMaintenance/");
                if (!Directory.Exists(RootDirectory))
                {
                    Directory.CreateDirectory(RootDirectory);
                }
                string filePath = RootDirectory + fileName;

                if (isTable == true)
                {
                    switch (sortCol)
                    {
                        case "Code No":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.WorkOrderCode).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.WorkOrderCode).ToList();
                            break;
                        case "Project Description":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.ProjectDescription).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.ProjectDescription).ToList();
                            break;
                        case "User Name":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.ScanUserName).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.ScanUserName).ToList();
                            break;
                        case "Work Order Type":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.QrcName).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.QrcName).ToList();
                            break;
                        case "Completed On":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.CreatedDate).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.CreatedDate).ToList();
                            break;
                        case "Problem Description":
                            if (sortDir == "asc")
                                lstdata = lstdata.OrderBy(o => o.ProblemDescription).ToList();
                            else
                                lstdata = lstdata.OrderByDescending(o => o.ProblemDescription).ToList();
                            break;
                        default:
                            lstdata = lstdata.OrderByDescending(o => o.StrCreatedDate).ToList();
                            break;
                    }
                    if (qrcName.Trim() != "Work Request")
                    {
                        ExportPDF<ReportChart>(lstdata, new string[] { "WorkOrderCode", "ProjectDescription", "ScanUserName", "QrcName", "StrCreatedDate" },
                            new string[] { "Code No", "Project Description", "User Name", "Work Order Type", "Completed On" },
                            filePath, PDFReportNameHeading.AssignedWorkOrder, fDate, tDate, highChart1, highChart2, isTable, ("Assigned " + qrcName + " details for " + userName));
                    }
                    else
                    {
                        ExportPDF<ReportChart>(lstdata, new string[] { "WorkOrderCode", "ProblemDescription", "ScanUserName", "QrcName", "StrCreatedDate" },
                            new string[] { "Code No", "Problem Description", "User Name", "Work Order Type", "Completed On" },
                            filePath, PDFReportNameHeading.AssignedWorkOrder, fDate, tDate, highChart1, highChart2, isTable, ("Assigned " + qrcName + " details for " + userName));
                    }
                }
                else
                {
                    ExportPDF<ReportChart>(lstdata, new string[] { "WorkOrderCode", "ProjectDescription", "ScanUserName", "QrcName", "StrCreatedDate" },
                        new string[] { "Code No", "Project Description", "User Name", "Work Order Type", "Completed On" },
                    filePath, PDFReportNameHeading.AssignedWorkOrder, fDate, tDate, highChart1, highChart2, isTable);
                }

                promptBoxForPDFFileSave(filePath, fileName);
                return null;

            }
            else
                return Json(lstdata, JsonRequestBehavior.AllowGet);
        }

        #endregion Report Work Order Assigned

        #endregion Work Order

        #region DAR

        #region CustomerAssistance

        public ActionResult CustomerAssistance()
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];

            }
            var objDARModelList = new List<DARModel>();
            objDARModelList = _IReportManager.GetDetailsOfDARCustomerAssistance(objLoginSession.LocationID, null, null, null);
            return PartialView("_CustomerAssistance", objDARModelList);

        }
        /// <summary>Created by Bhushan Dod on 16/04/2015
        /// Get details of work order issued
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public ActionResult GetAllCustomerAssistance(long? userId, DateTime? fromDate, DateTime? toDate, string exportingType = null, string sortCol = null, string sortDir = null, bool iD = false)
        {

            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            var objDARModelList = new List<DARModel>();

            bool isTo12noon = false;
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isTo12noon = true;
               // toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
            }
            ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
            //if (fromDate != null && toDate != null)
            //{
            //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
            //    {
            //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
            //    }
            //}

            objDARModelList = _IReportManager.GetDetailsOfDARCustomerAssistance(objLoginSession.LocationID, userId, fromDate, toDate);
            //shubham 07112016 Code for PDF Export
            if (exportingType == "PDF")
            {
                DateTime FD = Convert.ToDateTime(fromDate);
                string fDate = FD.ToString("MMMM dd, yyyy hh:mm tt");

                DateTime TD = Convert.ToDateTime(toDate).ToClientTimeZoneinDateTime();
                if (isTo12noon)
                {
                    TimeSpan ts = new TimeSpan(23, 59, 59);
                    TD = TD.Date + ts;
                }


                string tDate = TD.ToString("MMMM dd, yyyy hh:mm tt");
                List<DARModelForPDFExport> darModel = new List<DARModelForPDFExport>();
                darModel = (from rw in objDARModelList.AsEnumerable()
                            select new DARModelForPDFExport()
                            {
                                CreatedBy = rw.UserName,
                                ActivityDetails = rw.ActivityDetails,
                                StartTime = rw.StartTime,
                                EndTime = rw.EndTime,
                                //StartTimeImage = rw.StartTimeImage,
                                //EndTimeImage = rw.EndTimeImage,
                                SubmittedDate = rw.CreatedDate
                            }).ToList();
                switch (sortCol)
                {
                    case "Created By":
                        if (sortDir == "asc")
                            darModel = darModel.OrderBy(o => o.CreatedBy).ToList();
                        else
                            darModel = darModel.OrderByDescending(o => o.CreatedBy).ToList();
                        break;
                    case "Activity Details":
                        if (sortDir == "asc")
                            darModel = darModel.OrderBy(o => o.ActivityDetails).ToList();
                        else
                            darModel = darModel.OrderByDescending(o => o.ActivityDetails).ToList();
                        break;
                    case "Start Time":
                        if (sortDir == "asc")
                            darModel = darModel.OrderBy(o => o.StartTime).ToList();
                        else
                            darModel = darModel.OrderByDescending(o => o.StartTime).ToList();
                        break;
                    case "End Time":
                        if (sortDir == "asc")
                            darModel = darModel.OrderBy(o => o.EndTime).ToList();
                        else
                            darModel = darModel.OrderByDescending(o => o.EndTime).ToList();
                        break;
                    case "Submitted Date":
                        if (sortDir == "asc")
                            darModel = darModel.OrderBy(o => o.SubmittedDate).ToList();
                        else
                            darModel = darModel.OrderByDescending(o => o.SubmittedDate).ToList();
                        break;
                    default:
                        if (sortDir == "asc")
                            darModel = darModel.OrderBy(o => o.SubmittedDate).ToList();
                        else
                            darModel = darModel.OrderByDescending(o => o.SubmittedDate).ToList();
                        break;
                }
                string fileName = StringExtensionMethods.AppendTimeStamp(ReportName.CustomerAssistance + ".pdf");
                string RootPath = Server.MapPath("~/");
                string RootDirectory = Server.MapPath("~/ReportPDF/FullPageReport/DailyActivityReport/");
                if (!Directory.Exists(RootDirectory))
                {
                    Directory.CreateDirectory(RootDirectory);
                }
                string filePath = RootDirectory + fileName;
                if (iD)
                {
                    ExportPDFDetail<DARModelForPDFExport>(darModel, new string[] { "CreatedBy", "ActivityDetails", "StartTime", "EndTime", "StartTimeImage", "EndTimeImage",
                    "SubmittedDate"}, new string[] { "Created By", "Activity Details", "Start Time", "End Time", "Start Time Image", "End Time Image",
                    "Submitted Date"}, filePath, PDFReportNameHeading.CustomerAssistance, fDate, tDate, null, null, true, null, iD);
                }
                else
                {
                    ExportPDF<DARModelForPDFExport>(darModel, new string[] { "CreatedBy", "ActivityDetails", "StartTime", "EndTime", "StartTimeImage", "EndTimeImage",
                    "SubmittedDate"}, new string[] { "Created By", "Activity Details", "Start Time", "End Time", "Start Time Image", "End Time Image",
                    "Submitted Date"}, filePath, PDFReportNameHeading.CustomerAssistance, fDate, tDate, null, null, true);
                }


                promptBoxForPDFFileSave(filePath, fileName);
                return null;

            }
            else
                return Json(objDARModelList, JsonRequestBehavior.AllowGet);

        }

        #endregion CustomerAssistance

        #region  Jump Starts

        public ActionResult JumpStarts()
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];

            }
            var objDARModelList = new List<DARModel>();
            objDARModelList = _IReportManager.GetDetailsOfDARJumpStarts(objLoginSession.LocationID, null, null, null);
            return PartialView("_JumpStarts", objDARModelList);

        }
        /// <summary>Created by Bhushan Dod on 22/06/2015
        /// Get details of Jump Starts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public JsonResult GetAllJumpStarts(long? userId, DateTime? fromDate, DateTime? toDate, string exportingType = null, string sortCol = null, string sortDir = null, bool iD = false)
        {

            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            var objDARModelList = new List<DARModel>();
            bool isTo12noon = false;
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isTo12noon = true;
                //toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
            }
            ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
            //if (fromDate != null && toDate != null)
            //{
            //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
            //    {
            //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
            //    }
            //}

            objDARModelList = _IReportManager.GetDetailsOfDARJumpStarts(objLoginSession.LocationID, userId, fromDate, toDate);
            //shubham 07112016 Code for PDF Export
            if (exportingType == "PDF")
            {
                DateTime FD = Convert.ToDateTime(fromDate);
                string fDate = FD.ToString("MMMM dd, yyyy hh:mm tt");

                DateTime TD = Convert.ToDateTime(toDate);
                if (isTo12noon)
                {
                    TimeSpan ts = new TimeSpan(23, 59, 59);
                    TD = TD.Date + ts;
                }

                string tDate = TD.ToString("MMMM dd, yyyy hh:mm tt");
                List<DARModelForPDFExport> darModel = new List<DARModelForPDFExport>();
                darModel = (from rw in objDARModelList.AsEnumerable()
                            select new DARModelForPDFExport()
                            {
                                CreatedBy = rw.UserName,
                                ActivityDetails = rw.ActivityDetails,
                                StartTime = rw.StartTime,
                                EndTime = rw.EndTime,
                                StartTimeImage = rw.StartTimeImage,
                                EndTimeImage = rw.EndTimeImage,
                                SubmittedDate = rw.CreatedDate
                            }).ToList();
                darModel = GetOrderData(darModel, sortCol, sortDir);
                string fileName = StringExtensionMethods.AppendTimeStamp(ReportName.JumpStarts + ".pdf");
                string RootPath = Server.MapPath("~/");
                string RootDirectory = Server.MapPath("~/Content/eMaintenance/JumpStartsReport/");
                if (!Directory.Exists(RootDirectory))
                {
                    Directory.CreateDirectory(RootDirectory);
                }
                string filePath = RootDirectory + fileName;
                if (iD)
                {
                    ExportPDFDetail<DARModelForPDFExport>(darModel, new string[] { "CreatedBy", "ActivityDetails", "StartTime", "EndTime", "StartTimeImage", "EndTimeImage",
                    "SubmittedDate"}, new string[] { "Created By", "Activity Details", "Start Time", "End Time", "Start Time Image", "End Time Image",
                    "Submitted Date"}, filePath, PDFReportNameHeading.JumpStarts, fDate, tDate, null, null, true, null, iD);
                }
                else
                {
                    ExportPDF<DARModelForPDFExport>(darModel, new string[] { "CreatedBy", "ActivityDetails", "StartTime", "EndTime", "StartTimeImage", "EndTimeImage",
                    "SubmittedDate"}, new string[] { "Created By", "Activity Details", "Start Time", "End Time", "Start Time Image", "End Time Image",
                    "Submitted Date"}, filePath, PDFReportNameHeading.JumpStarts, fDate, tDate, null, null, true);
                }


                promptBoxForPDFFileSave(filePath, fileName);
                return null;

            }
            else
                return Json(objDARModelList, JsonRequestBehavior.AllowGet);

        }

        #endregion Jump Starts

        #region  Tire Inflation

        public ActionResult TireInflation()
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];

            }
            var objDARModelList = new List<DARModel>();
            objDARModelList = _IReportManager.GetDetailsOfDARTireInflation(objLoginSession.LocationID, null, null, null);
            return PartialView("_TireInflation", objDARModelList);

        }
        /// <summary>Created by Bhushan Dod on 22/06/2015
        /// Get details of Tire Inflation
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public JsonResult GetAllTireInflation(long? userId, DateTime? fromDate, DateTime? toDate, string exportingType = null, string sortCol = null, string sortDir = null, bool iD = true)
        {

            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            var objDARModelList = new List<DARModel>();

            bool isTo12noon = false;
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isTo12noon = true;
              //  toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
            }

            //Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
            //if (fromDate != null && toDate != null)
            //{
            //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
            //    {
            //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
            //    }
            //}

            objDARModelList = _IReportManager.GetDetailsOfDARTireInflation(objLoginSession.LocationID, userId, fromDate, toDate);
            //shubham 07112016 Code For PDF Export
            if (exportingType == "PDF")
            {
                DateTime FD = Convert.ToDateTime(fromDate);
                string fDate = FD.ToString("MMMM dd, yyyy hh:mm tt");

                DateTime TD = Convert.ToDateTime(toDate);
                if (isTo12noon)
                {
                    TimeSpan ts = new TimeSpan(23, 59, 59);
                    TD = TD.Date + ts;
                }

                string tDate = TD.ToString("MMMM dd, yyyy hh:mm tt");
                List<DARModelForPDFExport> darModel = new List<DARModelForPDFExport>();
                darModel = (from rw in objDARModelList.AsEnumerable()
                            select new DARModelForPDFExport()
                            {
                                CreatedBy = rw.UserName,
                                ActivityDetails = rw.ActivityDetails,
                                StartTime = rw.StartTime,
                                EndTime = rw.EndTime,
                                StartTimeImage = rw.StartTimeImage,
                                EndTimeImage = rw.EndTimeImage,
                                SubmittedDate = rw.CreatedDate
                            }).ToList();
                darModel = GetOrderData(darModel, sortCol, sortDir);
                string fileName = StringExtensionMethods.AppendTimeStamp(ReportName.TireInflation + ".pdf");
                string RootPath = Server.MapPath("~/");
                string RootDirectory = Server.MapPath("~/Content/eMaintenance/TireInflationReport/");
                if (!Directory.Exists(RootDirectory))
                {
                    Directory.CreateDirectory(RootDirectory);
                }
                string filePath = RootDirectory + fileName;
                if (iD)
                {
                    ExportPDFDetail<DARModelForPDFExport>(darModel, new string[] { "CreatedBy", "ActivityDetails", "StartTime", "EndTime", "StartTimeImage", "EndTimeImage",
                    "SubmittedDate"}, new string[] { "Created By", "Activity Details", "Start Time", "End Time", "Start Time Image", "End Time Image",
                    "Submitted Date"}, filePath, PDFReportNameHeading.TireInflation, fDate, tDate, null, null, true, null, iD);
                }
                else
                {
                    ExportPDF<DARModelForPDFExport>(darModel, new string[] { "CreatedBy", "ActivityDetails", "StartTime", "EndTime", "StartTimeImage", "EndTimeImage",
                    "SubmittedDate"}, new string[] { "Created By", "Activity Details", "Start Time", "End Time", "Start Time Image", "End Time Image",
                    "Submitted Date"}, filePath, PDFReportNameHeading.TireInflation, fDate, tDate, null, null, true);
                }


                promptBoxForPDFFileSave(filePath, fileName);
                return null;

            }
            else
                return Json(objDARModelList, JsonRequestBehavior.AllowGet);

        }

        #endregion Tire Inflation

        #region  Space Count

        public ActionResult SpaceCount()
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];

            }
            var objDARModelList = new List<DARModel>();
            objDARModelList = _IReportManager.GetDetailsOfDARSpaceCount(objLoginSession.LocationID, null, null, null);
            return PartialView("_SpaceCount", objDARModelList);

        }
        /// <summary>Updated by Shubham Bhojane on 22/06/2015
        /// Get details of Space Count
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate,exportingType,sortCol,sortDir"></param>
        /// <returns></returns>
        public JsonResult GetAllSpaceCount(long? userId, DateTime? fromDate, DateTime? toDate, string exportingType = null, string sortCol = null, string sortDir = null, bool iD = false)
        {

            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            var objDARModelList = new List<DARModel>();
            bool isTo12noon = false;
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isTo12noon = true;
                //toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
            }
            //Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
            //if (fromDate != null && toDate != null)
            //{
            //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
            //    {
            //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
            //    }
            //}

            objDARModelList = _IReportManager.GetDetailsOfDARSpaceCount(objLoginSession.LocationID, userId, fromDate, toDate);
            //shubham 07112016 Code for PDF Export  // dummy code from start time image and end time image removed
            if (exportingType == "PDF")
            {
                DateTime FD = Convert.ToDateTime(fromDate);
                string fDate = FD.ToString("MMMM dd, yyyy hh:mm tt");

                DateTime TD = Convert.ToDateTime(toDate);
                if (isTo12noon)
                {
                    TimeSpan ts = new TimeSpan(23, 59, 59);
                    TD = TD.Date + ts;
                }

                string tDate = TD.ToString("MMMM dd, yyyy hh:mm tt");
                List<DARModelForPDFExport> darModel = new List<DARModelForPDFExport>();
                darModel = (from rw in objDARModelList.AsEnumerable()
                            select new DARModelForPDFExport()
                            {
                                CreatedBy = rw.UserName,
                                ActivityDetails = rw.ActivityDetails,
                                StartTime = rw.StartTime,
                                EndTime = rw.EndTime,
                                StartTimeImage = rw.StartTimeImage,
                                EndTimeImage = rw.EndTimeImage,
                                SubmittedDate = rw.CreatedDate,
                                Description = rw.Description
                                
                            }).ToList();
                darModel = GetOrderData(darModel, sortCol, sortDir);
                string fileName = StringExtensionMethods.AppendTimeStamp(ReportName.SpaceCount + ".pdf");
                string RootPath = Server.MapPath("~/");
                string RootDirectory = Server.MapPath("~/Content/eMaintenance/SpaceCountReport/");
                if (!Directory.Exists(RootDirectory))
                {
                    Directory.CreateDirectory(RootDirectory);
                }
                string filePath = RootDirectory + fileName;
                if (iD)
                {
                    ExportPDFDetail<DARModelForPDFExport>(darModel, new string[] { "CreatedBy", "ActivityDetails", "StartTime", "EndTime", "StartTimeImage", "EndTimeImage",
                    "SubmittedDate"}, new string[] { "Created By", "Activity Details", "Start Time", "End Time", "Start Time Image", "End Time Image",
                    "Submitted Date"}, filePath, PDFReportNameHeading.SpaceCount, fDate, tDate, null, null, true, null, iD);
                }
                else
                {
                    ExportPDF<DARModelForPDFExport>(darModel, new string[] { "CreatedBy", "ActivityDetails", "StartTime", "EndTime", "StartTimeImage", "EndTimeImage",
                    "SubmittedDate"}, new string[] { "Created By", "Activity Details", "Start Time", "End Time", "Start Time Image", "End Time Image",
                    "Submitted Date"}, filePath, PDFReportNameHeading.SpaceCount, fDate, tDate, null, null, true);
                }


                promptBoxForPDFFileSave(filePath, fileName);
                return null;

            }
            else
                return Json(objDARModelList, JsonRequestBehavior.AllowGet);

        }

        #endregion Space Count

        #region Report DAR Codes
        [ActionName("DARCodes")]
        public ActionResult GetAllDARCodes()
        {
            return PartialView("_DARCodes");
        }

        /// <summary>Created by Bhushan Dod on 06/26/2015
        /// Get details of DAR Codes for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public ActionResult GetAllDARCodesType(string locationId, string userId, DateTime? fromDate, DateTime? toDate)
        {
            if (locationId == null || locationId == "0")
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    locationId = objLoginSession.LocationID.ToString();
                }
            }
            long projectId = 0;
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            if (!string.IsNullOrEmpty(locationId))
            {
                long.TryParse(locationId, out projectId);
                long.TryParse(userId, out _userId);
                //if (toDate != null)
                //{
                //    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                //        toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                //}

                ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
                //if (fromDate != null && toDate != null)
                //{
                //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
                //    {
                //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
                //    }
                //}

                lstdata = _IReportManager.GetDARCodesList(projectId, _userId, fromDate, toDate, null).ToList();
                return Json(lstdata, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>Created by Bhushan Dod on 03/04/2015
        /// Get details of DAR submitted  DAR code for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public JsonResult GetAllDarByCodes(string locationId, string userId, DateTime? fromDate, DateTime? toDate, string name)
        {
            if (locationId == null || locationId == "0")
            {

                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                    locationId = objLoginSession.LocationID.ToString();
                }
            }
            long projectId = 0;
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            if (!string.IsNullOrEmpty(locationId))
            {
                long.TryParse(locationId, out projectId);
                long.TryParse(userId, out _userId);
                //if (toDate != null)
                //{
                //    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                //        toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
                //}

                ////Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
                //if (fromDate != null && toDate != null)
                //{
                //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
                //    {
                //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
                //    }
                //}

                lstdata = _IReportManager.GetDARCodesList(projectId, _userId, fromDate, toDate, name).ToList(); ;
                return Json(lstdata, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>Updated by Shubham Bhojane on 10/11/2015
        /// Get details of Employee scan qrc log
        /// </summary>
        /// <param name="qrcName,UserId,FromDate,ToDate,exportingType,sortCol,sortDir"></param>
        /// <returns></returns>
        public JsonResult GetEmployeeDarSubmittedDetails(string qrcName, string userName, DateTime? fromDate, DateTime? toDate, [Bind(Prefix = "eT")] string exportingType = null,
            [Bind(Prefix = "sC")] string sortCol = null, [Bind(Prefix = "sD")] string sortDir = null, [Bind(Prefix = "h1")] string highChart1 = null,
            [Bind(Prefix = "h2")] string highChart2 = null, [Bind(Prefix = "iT")] bool isTable = false, [Bind(Prefix = "iD")] bool isDetail = false)
        {
            //ShowPresentation();

            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            long _userId = 0;
            List<ReportChart> lstdata = new List<ReportChart>();
            bool isTo12noon = false;
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isTo12noon = true;
               // toDate = (toDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : toDate;
            }

            //Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
            //if (fromDate != null && toDate != null)
            //{
            //    if ((fromDate.Value.Date == toDate.Value.Date) && (toDate.Value.ToLongTimeString() == "12:00:00 AM") || (toDate.Value.ToLongTimeString() == "12:00:00 AM"))
            //    {
            //        toDate = toDate.Value.AddDays(1).AddSeconds(-1);
            //    }
            //}

            lstdata = _IReportManager.DARSubmittedListByEmployee(objLoginSession.LocationID, qrcName, userName, fromDate, toDate).ToList();
            //shubham 07112016 code for PDF Export
            if (exportingType == "PDF")
            {
                DateTime FD = Convert.ToDateTime(fromDate);
                string fDate = FD.ToString("MMMM dd, yyyy hh:mm tt");

                DateTime TD = Convert.ToDateTime(toDate);
                if (isTo12noon)
                {
                    TimeSpan ts = new TimeSpan(23, 59, 59);
                    TD = TD.Date + ts;
                }

                string tDate = TD.ToString("MMMM dd, yyyy hh:mm tt");

                string fileName = StringExtensionMethods.AppendTimeStamp(ReportName.DailyActivityCode + ".pdf");
                string RootPath = Server.MapPath("~/");
                string RootDirectory = Server.MapPath("~/ReportPDF/FullPageReport/DailyActivityReport/");
                if (!Directory.Exists(RootDirectory))
                {
                    Directory.CreateDirectory(RootDirectory);
                }
                string filePath = RootDirectory + fileName;

                if (isTable == true)
                {
                    switch (sortCol)
                    {
                        case "User Name":
                            {
                                if (sortDir == "asc")
                                    lstdata = lstdata.OrderBy(o => o.ScanUserName).ToList();
                                else
                                    lstdata = lstdata.OrderByDescending(o => o.ScanUserName).ToList();
                                break;
                            }
                        case "DAR Code":
                            {
                                if (sortDir == "asc")
                                    lstdata = lstdata.OrderBy(o => o.QrcName).ToList();
                                else
                                    lstdata = lstdata.OrderByDescending(o => o.QrcName).ToList();
                                break;
                            }
                        case "Description":
                            {
                                if (sortDir == "asc")
                                    lstdata = lstdata.OrderBy(o => o.Description).ToList();
                                else
                                    lstdata = lstdata.OrderByDescending(o => o.Description).ToList();
                                break;
                            }
                        case "Start Time":
                            {
                                if (sortDir == "asc")
                                    lstdata = lstdata.OrderBy(o => o.StrStartTime).ToList();
                                else
                                    lstdata = lstdata.OrderByDescending(o => o.StrStartTime).ToList();
                                break;
                            }
                        case "End Time":
                            {
                                if (sortDir == "asc")
                                    lstdata = lstdata.OrderBy(o => o.StrEndTime).ToList();
                                else
                                    lstdata = lstdata.OrderByDescending(o => o.StrEndTime).ToList();
                                break;
                            }
                        case "Scan Date":
                            {
                                if (sortDir == "asc")
                                    lstdata = lstdata.OrderBy(o => o.StrCreatedDate).ToList();
                                else
                                    lstdata = lstdata.OrderByDescending(o => o.StrCreatedDate).ToList();
                                break;
                            }
                        default:
                            {
                                lstdata = lstdata.OrderByDescending(o => o.StrCreatedDate).ToList();
                                break;
                            }
                    }
                    if (isDetail)
                    {
                        ExportPDFDetail<ReportChart>(lstdata, new string[] { "ScanUserName", "QrcName", "Description", "StrStartTime", "StrEndTime", "StartTimeImage",
                    "StrCreatedDate"}, new string[] { "User Name", "DAR Code", "Description", "Start Time", "End Time", "Start Time Image",
                    "Scan Date"}, filePath, "Daily Activity Code Report", fDate, tDate, highChart1, highChart2, isTable, (qrcName + " by " + userName), isDetail);
                    }
                    else
                    {
                        ExportPDF<ReportChart>(lstdata, new string[] { "ScanUserName", "QrcName", "Description", "StrStartTime", "StrEndTime", "StartTimeImage",
                    "StrCreatedDate"}, new string[] { "User Name", "DAR Code", "Description", "Start Time", "End Time", "Start Time Image",
                    "Scan Date"}, filePath, "Daily Activity Code Report", fDate, tDate, highChart1, highChart2, isTable, (qrcName + " by " + userName));
                    }

                }
                else
                {
                    if (isDetail)
                    {
                        ExportPDFDetail<ReportChart>(lstdata, new string[] { "ScanUserName", "QrcName", "Description", "StrStartTime", "StrEndTime", "StartTimeImage",
                    "StrCreatedDate"}, new string[] { "User Name", "DAR Code", "Description", "Start Time", "End Time", "Start Time Image",
                    "Scan Date"}, filePath, "Daily Activity Code Report", fDate, tDate, highChart1, highChart2, isTable, null, isDetail);
                    }
                    else
                    {
                        ExportPDF<ReportChart>(lstdata, new string[] { "ScanUserName", "QrcName", "Description", "StrStartTime", "StrEndTime", "StartTimeImage",
                    "StrCreatedDate"}, new string[] { "User Name", "DAR Code", "Description", "Start Time", "End Time", "Start Time Image",
                    "Scan Date"}, filePath, "Daily Activity Code Report", fDate, tDate, highChart1, highChart2, isTable);
                    }
                }
                promptBoxForPDFFileSave(filePath, fileName);
                return null;

            }
            else
                return Json(lstdata, JsonRequestBehavior.AllowGet);
        }

        private void ShowPresentation()
        {

            string pictureFileName = "D:\\logo-etrac.png";

            Application pptApplication = new Application();

            Microsoft.Office.Interop.PowerPoint.Slides slides;
            Microsoft.Office.Interop.PowerPoint._Slide slide;
            Microsoft.Office.Interop.PowerPoint.TextRange objText;

            // Create the Presentation File
            Presentation pptPresentation = pptApplication.Presentations.Add(MsoTriState.msoTrue);

            Microsoft.Office.Interop.PowerPoint.CustomLayout customLayout = pptPresentation.SlideMaster.CustomLayouts[Microsoft.Office.Interop.PowerPoint.PpSlideLayout.ppLayoutText];

            // Create new Slide
            slides = pptPresentation.Slides;
            slide = slides.AddSlide(1, customLayout);

            // Add title
            objText = slide.Shapes[1].TextFrame.TextRange;
            objText.Text = "FPPT.com";
            objText.Font.Name = "Arial";
            objText.Font.Size = 32;

            //
            slide = slides.AddSlide(2, customLayout);

            objText = slide.Shapes[2].TextFrame.TextRange;
            objText.Text = "Content goes here\nYou can add text\nItem 3";

            Microsoft.Office.Interop.PowerPoint.Shape shape = slide.Shapes[2];
            slide.Shapes.AddPicture(pictureFileName, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, shape.Left, shape.Top, shape.Width, shape.Height);

            //slide.NotesPage.Shapes[2].TextFrame.TextRange.Text = "This demo is created by FPPT using C# - Download free templates from http://FPPT.com";

            //
            slide = slides.AddSlide(3, customLayout);
            Microsoft.Office.Interop.PowerPoint.Slides objSlides;
            Microsoft.Office.Interop.PowerPoint._Slide objSlide;
            Microsoft.Office.Interop.PowerPoint._Presentation objPres;



            objSlides = slides; objSlide = slide;
            int rows = 25, colm = 10; int currtSlideRow = 0;
            //objTable = objSlide.Shapes.AddTable(rows, colm, 36, 138, 648, 294).Table;
            Microsoft.Office.Interop.PowerPoint.Table objTable = null;

            if (rows > 5)
            {
                objTable = objSlide.Shapes.AddTable(5, colm, 36, 138, 648, 294).Table;
                currtSlideRow = 5; rows = rows - 5;
            }
            else
                objTable = objSlide.Shapes.AddTable(rows, colm, 36, 138, 648, 294).Table;

            for (int i = 1; i <= colm; i++)
            {
                objTable.Cell(1, i).Shape.TextFrame.TextRange.Text = "column" + i;
            }


            for (int i = 1; i <= currtSlideRow; i++)
            {

                for (int j = 1; j <= colm; j++)
                {
                    objTable.Cell(i, j).Shape.TextFrame.TextRange.Text = "data" + i + j;
                }
                if (i % 5 == 0)
                {
                    object myRow = null;
                    if (rows > 5)
                    {
                        rows = rows - 5;
                        currtSlideRow = 5;
                    }
                    else
                    {
                        currtSlideRow = rows;
                    }
                    objTable = objSlide.Shapes.AddTable(currtSlideRow, colm, 36, 138, 648, 294).Table;

                    if (rows > 5)
                    {
                        for (int myCount = 0; myCount < rows; myCount++)
                        {
                            objTable = objSlide.Shapes.AddTable(currtSlideRow, colm, 36, 138, 648, 294).Table;
                        }

                    }

                }
            }


            //objTable.Cell(1, 1).Shape.TextFrame.TextRange.Text = "Stundennnnnnnnnnnn";
            //objTable.Cell(1, 2).Shape.TextFrame.TextRange.Text = "Fach";
            //objTable.Cell(1, 3).Shape.TextFrame.TextRange.Text = "Note";



            //


            pptPresentation.SaveAs(@"D:\fppt12228.pptx", Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType.ppSaveAsDefault, MsoTriState.msoTrue);
            //pptPresentation.Close();
            //pptApplication.Quit();



        }

        /// <summary>
        /// Created by Shubham Bhojane 10/11/2016
        /// return DARModelForPDFExport object orederby
        /// </summary>
        /// <param name="reportChart"></param>
        /// <param name="sortCol"></param>
        /// <param name="sortDir"></param>
        /// <returns></returns>
        public List<DARModelForPDFExport> GetOrderData(List<DARModelForPDFExport> reportChart, string sortCol, string sortDir)
        {
            switch (sortCol)
            {
                case "Created By":
                    if (sortDir == "asc")
                        reportChart = reportChart.OrderBy(o => o.CreatedBy).ToList();
                    else
                        reportChart = reportChart.OrderByDescending(o => o.CreatedBy).ToList();
                    break;
                case "Activity Details":
                    if (sortDir == "asc")
                        reportChart = reportChart.OrderBy(o => o.ActivityDetails).ToList();
                    else
                        reportChart = reportChart.OrderByDescending(o => o.ActivityDetails).ToList();
                    break;
                case "Start Time":
                    if (sortDir == "asc")
                        reportChart = reportChart.OrderBy(o => o.StartTime).ToList();
                    else
                        reportChart = reportChart.OrderByDescending(o => o.StartTime).ToList();
                    break;
                case "End Time":
                    if (sortDir == "asc")
                        reportChart = reportChart.OrderBy(o => o.EndTime).ToList();
                    else
                        reportChart = reportChart.OrderByDescending(o => o.EndTime).ToList();
                    break;
                case "Submitted Date":
                    if (sortDir == "asc")
                        reportChart = reportChart.OrderBy(o => o.SubmittedDate).ToList();
                    else
                        reportChart = reportChart.OrderByDescending(o => o.SubmittedDate).ToList();
                    break;
            }
            return reportChart;

        }
        #endregion Report DAR Codes

        #endregion DAR
        #region ExportPDF

        //shubham 07112016
        /// <summary>
        /// Created by Shubham Bhojane 10/11/2016
        /// Export records in PDF
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customerList"></param>
        /// <param name="columns"></param>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        public static void ExportPDF<T>(IList<T> customerList, string[] columns, string[] columnNamesOnPDF, string filePath, string fileName, string formDate = null, string toDate = null
            , string highChart1 = null, string highChart2 = null, bool isTable = false, string tableIdentifier = null)
        {
            string value;
            float[] widths;
            try
            {
                iTextSharp.text.Font headerFont = FontFactory.GetFont("Verdana", 11, BaseColor.WHITE);
                iTextSharp.text.Font rowfont = FontFactory.GetFont("Verdana", 10, BaseColor.BLACK);
                Document document = new Document(PageSize.A3);
                PdfWriter writer = PdfWriter.GetInstance(document,
                           new FileStream(filePath, FileMode.OpenOrCreate));
                //    writer.PageEvent = new PdfWriterEvents("eTrac");// water mark

                document.Open();

                var content = writer.DirectContent;
                var pageBorderRect = new Rectangle(document.PageSize);

                pageBorderRect.Left = 10.0f;
                pageBorderRect.Right = 830.0f;
                pageBorderRect.Top = 1180.0f;
                pageBorderRect.Bottom = 10.0f;
                content.SetColorStroke(BaseColor.BLACK);
                content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
                content.Stroke();

                //Image watermark code
                //string imageURLWaterMark = System.Web.HttpContext.Current.Server.MapPath("~/Images/logo.png");
                //Image watermark = Image.GetInstance(imageURLWaterMark);
                //watermark.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                //watermark.Bottom = 0f;
                //watermark.ScalePercent(60f);
                //PdfWriterEvents writerEvent = new PdfWriterEvents(watermark);
                //writer.PageEvent = writerEvent;

                PdfContentByte cb = writer.DirectContent;
                cb.SetLineWidth(2.0f);   // Make a bit thicker than 1.0 default
                cb.SetGrayStroke(0.55f); // 0 = black, 1 = white
                cb.MoveTo(15, document.Top - 78f);
                cb.LineTo(825, document.Top - 78f);
                cb.Stroke();

                iTextSharp.text.Font pdfHeadFont = FontFactory.GetFont("TIMES_ROMAN", 18, BaseColor.BLACK);

                string imageURL = System.Web.HttpContext.Current.Server.MapPath("~/Images/logo.png");
                Image tif = Image.GetInstance(imageURL);
                tif.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                tif.Bottom = 0f;
                tif.ScalePercent(60f);
                document.Add(tif);

                Paragraph paragraph = new Paragraph(fileName, pdfHeadFont);
                paragraph.Alignment = PdfPCell.ALIGN_CENTER;
                paragraph.SpacingAfter = 20;
                document.Add(paragraph);

                iTextSharp.text.Font pdfDateTimeFont = FontFactory.GetFont("TIMES_ROMAN", 10, BaseColor.BLACK);
                if (formDate != null && toDate != null)
                {
                    Paragraph timeParagraph = new Paragraph((formDate + " - " + toDate), pdfDateTimeFont);
                    timeParagraph.Alignment = PdfPCell.ALIGN_LEFT;
                    timeParagraph.SpacingAfter = 20;

                    document.Add(timeParagraph);
                }


                //Add highcharts in PDF export 11/11/2016
                if (highChart1 != null)
                {
                    string hCURL = System.Web.HttpContext.Current.Server.MapPath(highChart1);
                    Image img = Image.GetInstance(hCURL);
                    img.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                    img.Bottom = 0f;
                    img.ScalePercent(80f);
                    document.Add(img);
                }

                //iTextSharp.text.Font pdfHeadFont2 = FontFactory.GetFont("TIMES_ROMAN", 18, BaseColor.WHITE);

                //Paragraph paragraph2 = new Paragraph("Hello", pdfHeadFont2);
                //paragraph2.Alignment = PdfPCell.ALIGN_CENTER;
                //paragraph2.SpacingAfter = 20;
                //document.Add(paragraph2);

                if (highChart2 != null)
                {
                    string hCURL = System.Web.HttpContext.Current.Server.MapPath(highChart2);
                    Image img = Image.GetInstance(hCURL);
                    img.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                    img.Bottom = 0f;
                    img.ScalePercent(80f);
                    img.SpacingAfter = 30;
                    img.SpacingBefore = 20;
                    document.Add(img);
                }

                //End 11/11/2016
                if (isTable == true)
                {
                    if (highChart1 != null)
                    {
                        iTextSharp.text.Font pdfHeadFont3 = FontFactory.GetFont("TIMES_ROMAN", 14, BaseColor.BLACK);
                        Paragraph paragraph3 = new Paragraph(tableIdentifier, pdfHeadFont3);
                        paragraph3.Alignment = PdfPCell.ALIGN_LEFT;
                        paragraph3.SpacingBefore = 40;
                        paragraph3.SpacingAfter = 20;
                        document.Add(paragraph3);
                    }

                    PdfPTable table = new PdfPTable(columns.Length);
                    table.TotalWidth = 800f;
                    table.LockedWidth = true;
                    switch (fileName)
                    {
                        case "Damage Report":
                            widths = new float[] { 60f, 60f, 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f, 50f, 50f };
                            table.SetWidths(widths);
                            break;
                        case "QRC OwnedBy Report":
                            widths = new float[] { 60f, 60f, 80f, 60f, 70f, 70f, 70f, 70f };
                            table.SetWidths(widths);
                            break;
                        case "QRC Amounts Report":
                            widths = new float[] { 60f, 60f, 60f, 80f, 70f, 70f };
                            table.SetWidths(widths);
                            break;
                        case "Customer Assistance Report":
                        case "Tire Inflation Report":
                        case "Jump Starts Report":
                        case "Space Count Report":
                        case "Daily Activity Code Report":
                            widths = new float[] { 60f, 60f, 60f, 60f, 70f, 70f, 70f };
                            table.SetWidths(widths);
                            break;
                    }

                    //widths = new float[] { 60f, 60f, 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f, 50f, 50f };
                    //table.SetWidths(widths);               

                    foreach (var column in columnNamesOnPDF)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column, headerFont));
                        cell.BackgroundColor = new BaseColor(45, 65, 84);
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);
                    }

                    foreach (var item in customerList)
                    {
                        foreach (var column in columns)
                        {
                            PdfPCell cell5 = null;
                            if (item.GetType().GetProperty(column).GetValue(item) == null || item.GetType().GetProperty(column).GetValue(item) == "" || item.GetType().GetProperty(column).GetValue(item) == "null")
                            {
                                value = "Not Available";
                                cell5 = new PdfPCell(new Phrase(value, rowfont));
                                cell5.HorizontalAlignment = Element.ALIGN_CENTER;
                                table.AddCell(cell5);
                            }
                            else
                            {
                                value = item.GetType().GetProperty(column).GetValue(item).ToString();
                                if (column == "Damage1" || column == "Damage2" || column == "Damage3" || column == "Damage4"
                                    || column == "Capture1" || column == "Capture2" || column == "Capture3" || column == "Capture4"
                                    || column == "AssetPicture" || column == "StartTimeImage" || column == "EndTimeImage" || column == "CroppedPicture" || column == "CapturedPicture")
                                {
                                    cell5 = ImageCell(value, 12f, 12f, PdfPCell.ALIGN_CENTER);
                                }
                                else
                                {
                                    cell5 = new PdfPCell(new Phrase(value, rowfont));
                                }
                                cell5.HorizontalAlignment = Element.ALIGN_CENTER;
                                table.AddCell(cell5);
                            }
                        }
                    }
                    document.Add(table);
                }
                document.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void ExportPDFDetail<T>(IList<T> customerList, string[] columns, string[] columnNamesOnPDF, string filePath, string fileName, string formDate = null, string toDate = null
    , string highChart1 = null, string highChart2 = null, bool isTable = false, string tableIdentifier = null, bool isDetailOnly = false)
        {
            string value;
            float[] widths;
            try
            {
                iTextSharp.text.Font headerFont = FontFactory.GetFont("Verdana", 11, BaseColor.WHITE);
                iTextSharp.text.Font rowfont = FontFactory.GetFont("Verdana", 10, BaseColor.BLACK);
                Document document = new Document(PageSize.A3);
                PdfWriter writer = PdfWriter.GetInstance(document,
                           new FileStream(filePath, FileMode.OpenOrCreate));
                //writer.PageEvent = new PdfWriterEvents("eTrac");

                document.Open();

                //Commented page border due Ankita don't want border in detail report
                //var content = writer.DirectContent;
                //var pageBorderRect = new Rectangle(document.PageSize);

                //pageBorderRect.Left = 10.0f;
                //pageBorderRect.Right = 830.0f;
                //pageBorderRect.Top = 1180.0f;
                //pageBorderRect.Bottom = 10.0f;
                //content.SetColorStroke(BaseColor.BLACK);
                //content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
                //content.Stroke();

                //string imageURLWaterMark = System.Web.HttpContext.Current.Server.MapPath("~/Images/logo.png");
                //Image watermark = Image.GetInstance(imageURLWaterMark);
                //watermark.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                //watermark.Bottom = 0f;
                //watermark.ScalePercent(60f);
                //PdfWriterEvents writerEvent = new PdfWriterEvents(watermark);
                //writer.PageEvent = writerEvent;

                //Commented page border due Ankita don't want border in detail report
                //PdfContentByte cb = writer.DirectContent;
                //cb.SetLineWidth(2.0f);   // Make a bit thicker than 1.0 default
                //cb.SetGrayStroke(0.55f); // 0 = black, 1 = white
                //cb.MoveTo(15, document.Top - 78f);
                //cb.LineTo(825, document.Top - 78f);
                //cb.Stroke();

                iTextSharp.text.Font pdfHeadFont = FontFactory.GetFont("TIMES_ROMAN", 18, BaseColor.BLACK);

                string imageURL = System.Web.HttpContext.Current.Server.MapPath("~/Images/logo.png");
                Image tif = Image.GetInstance(imageURL);
                tif.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                tif.Bottom = 0f;
                tif.ScalePercent(60f);
                document.Add(tif);

                Paragraph paragraph = new Paragraph(fileName, pdfHeadFont);
                paragraph.Alignment = PdfPCell.ALIGN_CENTER;
                paragraph.SpacingAfter = 20;
                document.Add(paragraph);

                //iTextSharp.text.Font pdfDateTimeFont = FontFactory.GetFont("TIMES_ROMAN", 10, BaseColor.BLACK);
                //if (formDate != null && toDate != null)
                //{
                //    Paragraph timeParagraph = new Paragraph((formDate + " - " + toDate), pdfDateTimeFont);
                //    timeParagraph.Alignment = PdfPCell.ALIGN_LEFT;
                //    timeParagraph.SpacingAfter = 20;

                //    document.Add(timeParagraph);
                //}


                //iTextSharp.text.Font pdfHeadFont2 = FontFactory.GetFont("TIMES_ROMAN", 18, BaseColor.WHITE);

                //Paragraph paragraph2 = new Paragraph("Hello", pdfHeadFont2);
                //paragraph2.Alignment = PdfPCell.ALIGN_CENTER;
                //paragraph2.SpacingAfter = 20;
                //document.Add(paragraph2);

                //End 11/11/2016
                if (isTable == true)
                {
                    if (highChart1 != null)
                    {
                        iTextSharp.text.Font pdfHeadFont3 = FontFactory.GetFont("TIMES_ROMAN", 14, BaseColor.BLACK);
                        Paragraph paragraph3 = new Paragraph(tableIdentifier, pdfHeadFont3);
                        paragraph3.Alignment = PdfPCell.ALIGN_LEFT;
                        paragraph3.SpacingBefore = 40;
                        paragraph3.SpacingAfter = 20;
                        document.Add(paragraph3);
                    }

                    PdfPTable table = new PdfPTable(columns.Length);
                    table.TotalWidth = 800f;
                    table.LockedWidth = true;
                    switch (fileName)
                    {
                        case "Damage Report":
                            widths = new float[] { 60f, 60f, 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f, 50f, 50f };
                            table.SetWidths(widths);
                            break;
                        case "QRC OwnedBy Report":
                            widths = new float[] { 60f, 60f, 80f, 60f, 70f, 70f, 70f, 70f };
                            table.SetWidths(widths);
                            break;
                        case "QRC Amounts Report":
                            widths = new float[] { 60f, 60f, 60f, 80f, 70f, 70f };
                            table.SetWidths(widths);
                            break;
                        case "Customer Assistance Report":
                        case "Tire Inflation Report":
                        case "Jump Starts Report":
                        case "Space Count Report":
                        case "Daily Activity Code Report":
                            widths = new float[] { 60f, 60f, 60f, 60f, 70f, 70f, 70f };
                            table.SetWidths(widths);
                            break;
                    }

                    //widths = new float[] { 60f, 60f, 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f, 50f, 50f };
                    //table.SetWidths(widths);               

                    foreach (var column in columnNamesOnPDF)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column, headerFont));
                        cell.BackgroundColor = new BaseColor(45, 65, 84);
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);
                    }

                    foreach (var item in customerList)
                    {
                        foreach (var column in columns)
                        {
                            PdfPCell cell5 = null;
                            if (item.GetType().GetProperty(column).GetValue(item) == null || item.GetType().GetProperty(column).GetValue(item) == "" || item.GetType().GetProperty(column).GetValue(item) == "null")
                            {
                                value = "Not Available";
                                cell5 = new PdfPCell(new Phrase(value, rowfont));
                                cell5.HorizontalAlignment = Element.ALIGN_CENTER;
                                table.AddCell(cell5);
                            }
                            else
                            {
                                value = item.GetType().GetProperty(column).GetValue(item).ToString();
                                if (column == "Damage1" || column == "Damage2" || column == "Damage3" || column == "Damage4"
                                    || column == "Capture1" || column == "Capture2" || column == "Capture3" || column == "Capture4"
                                    || column == "AssetPicture" || column == "StartTimeImage" || column == "EndTimeImage" || column == "CroppedPicture" || column == "CapturedPicture")
                                {
                                    cell5 = ImageCell(value, 12f, 12f, PdfPCell.ALIGN_CENTER);
                                }
                                else
                                {
                                    cell5 = new PdfPCell(new Phrase(value, rowfont));
                                }
                                cell5.HorizontalAlignment = Element.ALIGN_CENTER;
                                table.AddCell(cell5);
                            }
                        }
                    }
                    document.Add(table);
                }
                document.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Created by Shubham Bhojane 11/11/2016
        /// Save highcharts to display in PDF export.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Upload(string file)
        {
            if (file != null)
            {
                System.Drawing.Image image;
                var imageName = Guid.NewGuid() + ".png";
                string RootPath = Server.MapPath("~/");
                string RootDirectory = Server.MapPath("~/Images/HighChartsImageForExport/");
                if (!Directory.Exists(RootDirectory))
                {
                    Directory.CreateDirectory(RootDirectory);
                }
                var imagePath = RootDirectory + imageName;
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(file)))
                {
                    image = System.Drawing.Image.FromStream(ms);
                    var path = imagePath;
                    image.Save(path);
                }
                imagePath = "~/Images/HighChartsImageForExport/" + imageName;
                return Json(imagePath, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        ///// <summary>
        /// Created by Shubham Bhojane 10/11/2016
        /// To open file prompt Box open or Save file   
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        //public void promptBoxForPDFFileSave(string filePath, string fileName)
        //{
        //    string newFileName = "eTrac" + fileName;
        //    string watermarkLoc = System.Web.HttpContext.Current.Server.MapPath("~/Images/eTrac450-light.png");
        //    Document doc = new Document();
        //    PdfReader pdfReader = new PdfReader(filePath);
        //    PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(filePath.Replace(fileName, newFileName), FileMode.Create));

        //    Image img = Image.GetInstance(watermarkLoc);
        //    var pageSize = pdfReader.GetPageSizeWithRotation(1);

        //    var x = pageSize.Width / 2 - img.ScaledWidth / 2;
        //    var y = pageSize.Height / 2 - img.ScaledHeight / 2;
        //    img.SetAbsolutePosition(x, y);
        //    //img.ScaleToFit(100f,120f);
        //    PdfContentByte waterMark;
        //    for (int page = 1; page <= pdfReader.NumberOfPages; page++)
        //    {
        //        waterMark = pdfStamper.GetUnderContent(page);
        //        waterMark.AddImage(img);
        //    }
        //    pdfStamper.FormFlattening = true;
        //    pdfStamper.Close();
        //    pdfReader.Close();
        //    //delete old file. No more need of that file.
        //    System.IO.File.Delete(filePath);
        //    //For new file directory.
        //    //string RootDirectory = Server.MapPath("~/ReportPDF/FullPageReport/eMaintenance/");
        //    string RootDirectory = filePath.Replace(fileName,"");
        //    string pdfPath = RootDirectory + newFileName;
        //    WebClient client = new WebClient();
        //    Byte[] buffer = client.DownloadData(pdfPath);
        //    Response.Clear();
        //    Response.Buffer = true;
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-disposition", "attachment;filename="+ newFileName);
        //    Response.Charset = "";
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.WriteFile(pdfPath);
        //    Response.End();
        //}

        /// <summary>
        /// Created by Shubham Bhojane 10/11/2016
        /// To open file prompt Box open or Save file   
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        public void promptBoxForPDFFileSave(string filePath, string fileName)
        {
            string pdfPath = filePath;
            WebClient client = new WebClient();
            Byte[] buffer = client.DownloadData(pdfPath);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.WriteFile(pdfPath);
            Response.End();
        }
        /// <summary>
        /// Created By Shubham Bhojane 10/11/2016
        /// Read image and set height and width of image.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="scaleWidth"></param>
        /// <param name="scaleHeight"></param>
        /// <param name="align"></param>
        /// <returns></returns>
        private static PdfPCell ImageCell(string path, float scaleWidth, float scaleHeight, int align)
        {
            iTextSharp.text.Image image = null;
            try
            {
                image = iTextSharp.text.Image.GetInstance(path);
            }
            catch (Exception e)
            {
                string imageURL = System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/ProjectLogo/defaultImage.png");
                image = iTextSharp.text.Image.GetInstance(imageURL);
            }
            var height = image.Height; var width = image.Width;
            image.ScaleToFit(60f, 60f);
            PdfPCell cell = new PdfPCell(image);
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 1.0f;
            cell.PaddingTop = 0.5f;
            return cell;
        }

        private void setWatermark()
        {

            string filelocation = "~/Content/Images/ProjectLogo/defaultImage.png";
            string watermarkLoc = "~/Content/Images/ProjectLogo/defaultImage.png";
            Document doc = new Document();
            PdfReader pdfReader = new PdfReader(filelocation);
            PdfStamper stamp = new PdfStamper(pdfReader, new FileStream(filelocation.Replace(".pdf", "abcd.pdf"), FileMode.Create));

            Image img = Image.GetInstance(watermarkLoc);
            img.SetAbsolutePosition(250, 300);
            PdfContentByte waterMark;
            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            {
                waterMark = stamp.GetUnderContent(page);
                waterMark.AddImage(img);
            }
            stamp.FormFlattening = true;
            stamp.Close();
            System.IO.File.Delete(filelocation);
            System.IO.File.Move(filelocation.Replace(".pdf", "abcd.pdf"), filelocation);
        }


        //end 07112016
        #endregion ExportPDF
    }
    class myClass
    {

        public string Name { get; set; }
        public myClass()
        {
        }
    }

    //public class PdfWriterEvents : IPdfPageEvent
    //{
    //    string watermarkText = string.Empty;

    //    public PdfWriterEvents(string watermark)
    //    {
    //        watermarkText = watermark;
    //    }

    //    public void OnOpenDocument(PdfWriter writer, Document document) { }
    //    public void OnCloseDocument(PdfWriter writer, Document document) { }
    //    public void OnStartPage(PdfWriter writer, Document document)
    //    {
    //        float fontSize = 80;
    //        float xPosition = iTextSharp.text.PageSize.A3.Width / 2;
    //        float yPosition = (iTextSharp.text.PageSize.A3.Height - 140f) / 2;
    //        float angle = 45;
    //        try
    //        {
    //            PdfContentByte under = writer.DirectContentUnder;
    //            BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
    //            under.BeginText();
    //            under.SetColorFill(BaseColor.LIGHT_GRAY);
    //            under.SetFontAndSize(baseFont, fontSize);
    //            under.ShowTextAligned(PdfContentByte.ALIGN_CENTER, watermarkText, xPosition, yPosition, angle);
    //            under.EndText();
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.Error.WriteLine(ex.Message);
    //        }

    //    }
    //    public void OnEndPage(PdfWriter writer, Document document)
    //    {
    //    }
    //    public void OnParagraph(PdfWriter writer, Document document, float paragraphPosition) { }
    //    public void OnParagraphEnd(PdfWriter writer, Document document, float paragraphPosition) { }
    //    public void OnChapter(PdfWriter writer, Document document, float paragraphPosition, Paragraph title) { }
    //    public void OnChapterEnd(PdfWriter writer, Document document, float paragraphPosition) { }
    //    public void OnSection(PdfWriter writer, Document document, float paragraphPosition, int depth, Paragraph title) { }
    //    public void OnSectionEnd(PdfWriter writer, Document document, float paragraphPosition) { }
    //    public void OnGenericTag(PdfWriter writer, Document document, Rectangle rect, String text) { }

    //}


}