using System;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers
{
    [Authorize]
    public class DARController : Controller
    {
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();

        private readonly IDARManager _IDARManager;

        public DARController(IDARManager _IDARManager)
        {
            this._IDARManager = _IDARManager;
        }
        public ActionResult ListDAR()
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
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View();
        }

        public ActionResult DARReport()
        {
            return View();
        }


        ////public JsonResult GetListDAR(long UserId, DateTime fromDate, DateTime toDate, long taskType, int? NumberOfRows = 20, int? PageIndex = 1, int? TotalRecords = 10, string SortOrderBy = null, string SearchText = null, string SortColumnName = null, long? UserType = null)
        //[HttpGet]
        //public JsonResult GetListDAR(long UserId, DateTime? fromDate, DateTime? toDate, long? taskType, int? NumberOfRows = 20, int? PageIndex = 1, int? TotalRecords = 10, string SortOrderBy = null, string SearchText = null, string SortColumnName = null, long? UserType = null)
        //{
        //    JQGridResults result = new JQGridResults();
        //    //List<JQGridRow> rows = new List<JQGridRow>();
        //    //SortOrderBy = string.IsNullOrEmpty(SortOrderBy) ? "asc" : SortOrderBy;
        //    //SortColumnName = string.IsNullOrEmpty(SortColumnName) ? "UserEmail" : SortColumnName;
        //    //SearchText = string.IsNullOrEmpty(SearchText) ? "" : SearchText; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);

        //    //long TotalRows = 0;
        //    //if (!fromDate.HasValue) { fromDate = DateTime.Now; }
        //    //if (!toDate.HasValue) { toDate = DateTime.Now; }
        //    //if (!taskType.HasValue) { taskType = 1; }

        //    //try
        //    //{
        //    //    List<DARModel> listDAR = _IDARManager.GetDARDetails(UserId, fromDate.Value, toDate.Value, taskType.Value, UserType, PageIndex, NumberOfRows, SortColumnName, SortOrderBy, SearchText, out TotalRows);

        //    //    foreach (var itemDAR in listDAR)
        //    //    {
        //    //        JQGridRow row = new JQGridRow();
        //    //        row.id = itemDAR.DARId;
        //    //        row.cell = new string[1];
        //    //        row.cell[0] = itemDAR.ActivityDetails;
        //    //        //row.cell[1] = Convert.ToString(itemDAR.CreatedOn);
        //    //        rows.Add(row);
        //    //    }
        //    //    result.rows = rows.ToArray();
        //    //    result.page = (PageIndex.HasValue) ? PageIndex.Value : 1;
        //    //    result.total = Convert.ToInt32(TotalRows / (NumberOfRows.HasValue ? NumberOfRows.Value : 20));
        //    //    result.records = Convert.ToInt32(TotalRows);
        //    //}
        //    //catch (Exception ex)
        //    //{ return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        //    ////{ViewBag.Message = ex.Message;ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;}
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult EditDARDetail(string id)
        {
            long darId;
            eTracLoginModel ObjLoginModel = null;
            DARModel objDARModel = new DARModel();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    id = Cryptography.GetDecryptedData(id, true);
                }
                darId = Convert.ToInt64(id);

                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);

                    objDARModel = _IDARManager.GetDARById(darId);
                }

                return PartialView("_EditDARDetails", objDARModel);
            }
            catch (Exception ex)
            {
                { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
            }
        }

        [HttpPost]
        public ActionResult UpdateDARDetails(DARModel objDARModel)
        {
            eTracLoginModel ObjLoginModel = null;
            try
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);

                    if (objDARModel.DARId > 0)
                    {
                        objDARModel.ModifiedBy = ObjLoginModel.UserId;
                        objDARModel.ModifiedOn = DateTime.UtcNow;
                        _IDARManager.UpdateDAR(objDARModel);
                    }
                }
                return RedirectToAction("ListDAR");

            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }
    }
}