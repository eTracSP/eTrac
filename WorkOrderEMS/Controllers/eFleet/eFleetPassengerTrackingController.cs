using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.BusinessLogic.Interfaces.eFleet;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers.eFleet
{
    [RoutePrefix("passenger")]
    [Route("{action}")]
    public class eFleetPassengerTrackingController : Controller
    {
        private readonly IPassengerTracking _IPassengerTracking;

        public eFleetPassengerTrackingController(IPassengerTracking _IPassengerTracking)
        {
            this._IPassengerTracking = _IPassengerTracking;
        }
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        // GET: eFleetPassengerTracking
        [HttpGet]
        [Route("create")]
        public ActionResult CreateRoute()
        {
            return View();
        }

        [HttpPost]
        [Route("create")]
        public ActionResult CreateRoute(eFleetPassengerTrackingModel objeFleetPassengerTrackingModel)
        {
            try
            {
                eTracLoginModel objeTracLoginModel = (eTracLoginModel)(Session["eTrac"]);
                Result objStatus;
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
                if (objeFleetPassengerTrackingModel.RouteID == 0)
                {
                    objeFleetPassengerTrackingModel.CreatedBy = objeTracLoginModel.UserId;
                    objeFleetPassengerTrackingModel.CreatedDate = DateTime.UtcNow;                                    
                }
                else
                {
                    objeFleetPassengerTrackingModel.CreatedBy = objeTracLoginModel.UserId;
                    objeFleetPassengerTrackingModel.CreatedDate = DateTime.UtcNow;                   
                }
                objStatus = _IPassengerTracking.SavePassengerTrackingRoute(objeFleetPassengerTrackingModel);

                if (objStatus == Result.Completed)
                {
                    ModelState.Clear();
                    ViewBag.Message = CommonMessage.WorkOrderSaveSuccessMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                }
                else if (objStatus == Result.DuplicateRecord)
                {
                    ViewBag.Message = CommonMessage.DuplicateRecordMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Info; // store the message for successful in tempdata to display in view.
                }
                else if (objStatus == Result.UpdatedSuccessfully)
                {

                    ModelState.Clear();
                    ViewBag.Message = CommonMessage.WorkOrderUpdateMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;// store the message for successful in tempdata to display in view.
                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }
            return View();
        }
        [HttpGet]
        [Route("Edit")]
        public ActionResult EditPassengerTrackingRoute(string id)
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
                    long _PassengerTrackingRouteid = 0;
                    long.TryParse(id, out _PassengerTrackingRouteid);
                    var _eFleetPassengerTackingDetails = _IPassengerTracking.GeteFleetPassengerTrackingDetailsById(_PassengerTrackingRouteid);
                    return View("CreateRoute", _eFleetPassengerTackingDetails);
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
            return View("CreateRoute");
        }
        /// <summary>
        /// Created By Ashwajit Bansod
        /// Date : Oct/13/2017
        /// For Delete Passenger tracking Route
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteeFleetPassengerTrackingRoute(string id)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null; long loggedInUser = 0, passenggerId = 0;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
                loggedInUser = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;
                if (!string.IsNullOrEmpty(id))
                {
                    id = Cryptography.GetDecryptedData(id, true);
                }
                passenggerId = Convert.ToInt64(id);

                Result result = _IPassengerTracking.DeleteeFleetPassengerTracking(passenggerId, loggedInUser, ObjLoginModel.Location);
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