using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers.Common
{
    public class DropDownController : Controller
    {
        //
        // GET: /DropDown/
        private readonly ICommonMethod _ICommonMethod;
        private readonly ICommonMethodAdmin _ICommonMethodAdmin;
        private readonly IGlobalAdmin _IGlobalAdmin;
        private readonly IReportManager _IReportManager;
        public DropDownController(ICommonMethod _ICommonMethod, ICommonMethodAdmin _ICommonMethodAdmin, IGlobalAdmin _IGlobalAdmin, IReportManager _IReportManager)
        {
            this._ICommonMethod = _ICommonMethod;
            this._ICommonMethodAdmin = _ICommonMethodAdmin;
            this._IGlobalAdmin = _IGlobalAdmin;
            this._IReportManager = _IReportManager;
        }
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetStateByCountryID(string CountryID)
        {
            int _countryId = 0;
            if (!string.IsNullOrEmpty(CountryID))
            {
                int.TryParse(CountryID, out _countryId);
                StateModel State = new StateModel();
                CommonMethodManager _CommonMethodManager = new CommonMethodManager();

                State.Data = _ICommonMethod.GetStateByCountryId(_countryId);
                return Json(State, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }

        }
        //public JsonResult GetAllAssetByWorkArea(string WorkArea)
        //{
        //    eTracLoginModel ObjLoginModel = null;
        //    if (Session["eTrac"] != null)
        //    { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
        //    int _WorkAreaID = 0;
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(WorkArea))
        //        {
        //            int.TryParse(WorkArea, out _WorkAreaID);
        //            StateModel State = new StateModel();
        //            CommonMethodManager _CommonMethodManager = new CommonMethodManager();

        //            List<SelectListItem> lstAsset = _ICommonMethod.GetAllAssetByWorkArea(ObjLoginModel.LocationID, _WorkAreaID);
        //            return Json(lstAsset, JsonRequestBehavior.AllowGet);
        //        }
        //        else { return Json(null, JsonRequestBehavior.AllowGet); }
        //    }
        //    catch (Exception)
        //    {
        //        throw ;
        //    }

        //}


        /// <summary>GetLocationSubType
        /// 
        /// </summary>
        /// <param name="LocationType"></param>
        /// <returns></returns>
        public JsonResult GetLocationSubType(string LocationType)
        {
            int _locationType = 0;
            if (!string.IsNullOrEmpty(LocationType))
            {
                int.TryParse(LocationType, out _locationType);
                GlobalCodeModel GlobalCodeList = new GlobalCodeModel();
                GlobalCodeList.GlobalCodeList = (_locationType > 0 && _locationType == 107) ? _ICommonMethod.GetGlobalCodeData("LOCATIONSUBTYPE") : null;
                return Json(GlobalCodeList, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }


        /// <summary>Created by gayatri
        /// To bind the location drop down on the qrc page
        /// </summary>
        /// <param name="LocationType"></param>
        /// <returns></returns>
        public JsonResult GetLocationByAdminID(string userId)
        {
            long lAdminID = 0;
            if (!string.IsNullOrEmpty(userId))
            {
                long.TryParse(userId, out lAdminID);
                List<SelectListItem> lstLocation = _ICommonMethodAdmin.GetLocationByAdminId(lAdminID);
                return Json(lstLocation, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>Created by Roshan
        /// To bind the location drop down on the qrc page
        /// </summary>
        /// <param name="LocationType"></param>
        /// <returns></returns>
        public JsonResult GetLocationByManagerID(string userId)
        {
            long lManagerId = 0;
            if (!string.IsNullOrEmpty(userId))
            {
                long.TryParse(userId, out lManagerId);
                List<SelectListItem> lstLocation = _ICommonMethod.GetLocationByManagerIdForDdl(lManagerId);

                return Json(lstLocation, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }

        public JsonResult GetManagerByAdminID(string AdminID)
        {
            long lAdminID = 0;
            List<UserModelList> lstmanager = new List<UserModelList>();
            if (!string.IsNullOrEmpty(AdminID))
            {
                long.TryParse(AdminID, out lAdminID);
                lstmanager = _ICommonMethodAdmin.GetManagerByAdminId(lAdminID);
                return Json(lstmanager, JsonRequestBehavior.AllowGet);
            }
            else { return Json(null, JsonRequestBehavior.AllowGet); }
        }
        /// <summary>Created by Bhushan Dod on 01/04/2015
        /// To bind the Employee drop down on the DAR page
        /// </summary>
        /// <param name="project id"></param>
        /// <returns></returns>
        public JsonResult GetEmployeeByLocation(string LocationId)
        {
            long Totalrecords = 0;
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
                long projectId = 0;
                if (!string.IsNullOrEmpty(LocationId))
                {
                    long.TryParse(LocationId, out projectId);
                    List<SelectListItem> lstEmployee = _IGlobalAdmin.GetAllUserListforDAR(objLoginSession.UserId, projectId, 1, 100000, "Name", "asc", "", "All Users", out Totalrecords).Select(u => new SelectListItem()
                    {
                        Text = u.Name,
                        Value = Convert.ToString(u.UserId, CultureInfo.InvariantCulture)
                    }).ToList();
                    return Json(lstEmployee, JsonRequestBehavior.AllowGet);
                }
                else { return Json(null, JsonRequestBehavior.AllowGet); }

            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>Created by Bhushan Dod on 01/04/2015
        /// To bind the Employee drop down on the DAR page
        /// </summary>
        /// <param name="project id"></param>
        /// <returns></returns>
        public JsonResult GetEmployeeByLocationforReport(string LocationId)
        {
            long Totalrecords = 0;
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
                long projectId = 0;
                if (!string.IsNullOrEmpty(LocationId))
                {
                    long.TryParse(LocationId, out projectId);
                    //Commented by Bhushan Dod on 05/30/2017 for location is wrong. we need location id from session.
                    //List<SelectListItem> lstEmployee = _IGlobalAdmin.GetAllITAdministratorListForReport(objLoginSession.UserId, projectId, 1, 100000, "Name", "asc", "", "All Users", out Totalrecords).Select(u => new SelectListItem()
                    //{
                    //    Text = u.Name,
                    //    Value = Convert.ToString(u.UserId, CultureInfo.InvariantCulture)
                    //}).ToList();
                    List<SelectListItem> lstEmployee = _IGlobalAdmin.GetAllITAdministratorListForReport(objLoginSession.UserId, objLoginSession.LocationID, 1, 100000, "Name", "asc", "", "All Users", out Totalrecords).Select(u => new SelectListItem()
                    {
                        Text = u.Name,
                        Value = Convert.ToString(u.UserId, CultureInfo.InvariantCulture)
                    }).ToList();
                    return Json(lstEmployee, JsonRequestBehavior.AllowGet);
                }
                else { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>Created by Bhushan Dod on 01/04/2015
        /// To bind the Item list drop down
        /// </summary>
        /// <param name="project id"></param>
        /// <returns></returns>
        public JsonResult GetItemListByLocationforReport(string LocationId)
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
                long projectId = 0;
                if (!string.IsNullOrEmpty(LocationId))
                {
                    long.TryParse(LocationId, out projectId);
                    List<SelectListItem> lstEmployee = _IReportManager.GetAssetListForReportingDDL(projectId).Select(u => new SelectListItem()
                    {
                        Text = u.Text,
                        Value = Convert.ToString(u.Value, CultureInfo.InvariantCulture)
                    }).ToList();
                    return Json(lstEmployee, JsonRequestBehavior.AllowGet);
                }
                else { return Json(null, JsonRequestBehavior.AllowGet); }

            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }
    }
}