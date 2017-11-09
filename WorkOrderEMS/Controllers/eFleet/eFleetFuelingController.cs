using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WorkOrderEMS.BusinessLogic.Exception_B;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.Controllers.eFleet
{
    [RoutePrefix("fueling")]
    [Route("{action}")]
    public class eFleetFuelingController : Controller
    {
        private readonly IeFleetFuelingManager _IeFleetFuelingManager;

        public eFleetFuelingController(IeFleetFuelingManager _IeFleetFuelingManager)
        {
            this._IeFleetFuelingManager = _IeFleetFuelingManager;
        }
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();

        [HttpGet]
        [Route("listfueling")]
        public ActionResult ListFueling()
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
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("ListFueling");
        }

        [HttpGet]
        public JsonResult GetFuelingList(string _search, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string statusType = null)
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
            var objeFleetFuelingModelForService = new eFleetFuelingModelForService();
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rowss = new List<JQGridRow>();
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "FuelingDate" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);		
            try
            {
                var eFleetFuelingList = _IeFleetFuelingManager.GetListeFleetFuelingForJQGridDetails(ObjLoginModel.LocationID, rows, TotalRecords, sidx, sord, txtSearch, Convert.ToInt64(statusType));
                foreach (var eFleetFuel in eFleetFuelingList.rows)
                {
                    JQGridRow row = new JQGridRow();
                    row.id = Cryptography.GetEncryptedData(Convert.ToString(eFleetFuel.FuelID), true);
                    row.cell = new string[13];
                    row.cell[0] = eFleetFuel.VehicleNumber;
                    row.cell[1] = eFleetFuel.QRCodeID;
                    row.cell[2] = eFleetFuel.Mileage;
                    row.cell[3] = eFleetFuel.CurrentFuel;
                    row.cell[4] = eFleetFuel.FuelTypeName;
                    row.cell[5] = eFleetFuel.ReceiptNo;
                    row.cell[6] = eFleetFuel.FuelingDate.ToString("MM'/'dd'/'yyyy hh:mm tt");
                    row.cell[7] = Convert.ToString(eFleetFuel.Gallons);
                    row.cell[8] = Convert.ToString(eFleetFuel.PricePerGallon);
                    row.cell[9] = Convert.ToString(eFleetFuel.Total);
                    row.cell[10] = eFleetFuel.GasStatioName;
                    row.cell[11] = eFleetFuel.CardNo;
                    row.cell[12] = eFleetFuel.DriverName;
                    rowss.Add(row);
                }
                result.rows = rowss.ToArray();
                result.page = Convert.ToInt32(page);
                result.total = (int)Math.Ceiling((decimal)Convert.ToInt32(TotalRecords.Value) / rows.Value);
                result.records = Convert.ToInt32(TotalRecords.Value);
            }
            catch (Exception ex)
            {
                Exception_B.exceptionHandel_Runtime(ex, "JsonResult GetFuelingList(string _search, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string statusType = null)", "eFleetFuelingController", null);

                List<JQGridRow> rowsss = new List<JQGridRow>();
                result.rows = rowsss.ToArray();
                result.page = 0;
                result.total = 0;
                result.records = 0;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}