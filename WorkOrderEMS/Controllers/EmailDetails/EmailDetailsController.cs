using System;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;



namespace WorkOrderEMS.Controllers.EmailDetails
{
    [Authorize]
    public class EmailDetailsController : Controller
    {
        private readonly IEmailDetail _IEmailDetail;

        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();

        public EmailDetailsController(IEmailDetail _IEmailDetail)
        {
            this._IEmailDetail = _IEmailDetail;
        }

        //
        // GET: /EmailDetails/
        public ActionResult ListEmail()
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


        //[HttpGet]
        //public ActionResult GetEmailList(long? emailId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords)
        //{

        //    var emailList = _IEmailDetail.GetAllEmailList(emailId,pageIndex, numberOfRows,sortColumnName ,sortOrderBy, textSearch,totalRecords);

        //    JQGridResults result = new JQGridResults();
        //    List<JQGridRow> rows = new List<JQGridRow>();
        //    try
        //    {
        //        foreach (var emailDetails in emailList)
        //        {
        //            JQGridRow row = new JQGridRow();

        //            row.id = emailDetails.EmailLogId;
        //            row.cell = new string[2];
        //            row.cell[0] = emailDetails.SentEmail;
        //            row.cell[1] = emailDetails.Subject;
        //            rows.Add(row);
        //        }
        //        result.rows = rows.ToArray();
        //        result.page = pageIndex.Value;
        //        result.total = (int)Math.Ceiling((decimal)totalRecords.Value / numberOfRows.Value);
        //        result.records = Convert.ToInt16(totalRecords.Value);
        //    }
        //    catch (Exception ex)
        //    { string error = ex.Message; }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult DeleteEmail(string logId)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null; long LoggedInUser = 0, emailLogId = 0;
                string Id;
                DARModel objDAR;
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    LoggedInUser = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;


                    if (!string.IsNullOrEmpty(logId))
                    {
                        Id = Cryptography.GetDecryptedData(logId, true);
                        emailLogId = Convert.ToInt32(Id);

                        objDAR = new DARModel();
                        objDAR.LocationId = ObjLoginModel.LocationID;
                        objDAR.UserId = ObjLoginModel.UserId;
                        objDAR.CreatedBy = ObjLoginModel.UserId;
                        objDAR.CreatedOn = DateTime.UtcNow;
                        objDAR.TaskType = (long)TaskTypeCategory.DeleteEmail;

                        Result result = _IEmailDetail.DeleteEmail(emailLogId, LoggedInUser);
                        if (result == Result.Delete)
                        {
                            ViewBag.Message = CommonMessage.DeleteSuccessMessage();
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        }
                        else if (result == Result.Failed)
                        {
                            ViewBag.Message = "Can't Delete Email";
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                        }
                        else
                        {
                            ViewBag.Message = CommonMessage.FailureMessage();
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                        }
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
        }
    }
}