using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;



namespace WorkOrderEMS.Controllers.Roles_And_Permissions
{


    public class RolesAndPermissionsController : Controller
    {
        private readonly IGlobalAdmin _IGlobalAdmin;
        private readonly ICommonMethod _ICommonMethod;
        private readonly IDashboardWidgetSettingManager _IDashboardWidgetSettingManager;
        private readonly ILogin _ILogin;

        public RolesAndPermissionsController(IGlobalAdmin _IGlobalAdmin, ICommonMethod _ICommonMethod, IDashboardWidgetSettingManager _IDashboardWidgetSettingManager, ILogin _ILogin)
        {
            this._IGlobalAdmin = _IGlobalAdmin;
            this._ICommonMethod = _ICommonMethod;
            this._IDashboardWidgetSettingManager = _IDashboardWidgetSettingManager;
            this._ILogin = _ILogin;
        }
        //
        // GET: /RolesAndPermissions/
        public ActionResult ListUsers()
        {
            ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
            return View();
        }


        [HttpPost]
        public JsonResult GetListUserForPermission(long? UserId, long? locationId, int? NumberOfRows = 20, int? PageIndex = 1, int? TotalRecords = 10, string SortOrderBy = null, string SearchText = null, string SortColumnName = null, string UserType = null)
        {
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rows = new List<JQGridRow>();
            SortOrderBy = string.IsNullOrEmpty(SortOrderBy) ? "asc" : SortOrderBy;
            SortColumnName = string.IsNullOrEmpty(SortColumnName) ? "UserEmail" : SortColumnName;
            SearchText = string.IsNullOrEmpty(SearchText) ? "" : SearchText; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);

            long TotalRows = 0;

            try
            {
                List<UserModelList> ITAdministratorList = _IGlobalAdmin.GetAllITAdministratorList(UserId, Convert.ToInt64(locationId), PageIndex, NumberOfRows, SortColumnName, SortOrderBy, SearchText, UserType, out TotalRows);
                foreach (var ITAdmin in ITAdministratorList)
                {
                    JQGridRow row = new JQGridRow();
                    row.id = Cryptography.GetEncryptedData(ITAdmin.UserId.ToString(), true);
                    row.cell = new string[7];
                    row.cell[0] = ITAdmin.Name;
                    row.cell[1] = ITAdmin.UserEmail;
                    row.cell[2] = ITAdmin.Name;
                    row.cell[3] = ITAdmin.UserType;
                    row.cell[4] = ITAdmin.DOB.HasValue ? ITAdmin.DOB.Value.ToShortDateString() : "";
                    row.cell[5] = ITAdmin.ProfileImage;
                    row.cell[6] = ITAdmin.EmployeeProfile;
                    rows.Add(row);
                }
                result.rows = rows.ToArray();
                result.page = (PageIndex.HasValue) ? PageIndex.Value : 1;
                result.total = Convert.ToInt32(TotalRows / (NumberOfRows.HasValue ? NumberOfRows.Value : 20));
                result.records = Convert.ToInt32(TotalRows);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            //{ViewBag.Message = ex.Message;ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;}
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _PermissionsDisplay(string id)
        {
            long userId;
            eTracLoginModel ObjLoginModel = null;
            AssignInventoryModel objAssignInventoryModel = new AssignInventoryModel();
            PermissionDetailsModel objPermissionDetails = new PermissionDetailsModel();
            try
            {

                if (!string.IsNullOrEmpty(id))
                {
                    id = Cryptography.GetDecryptedData(id, true);
                }
                userId = Convert.ToInt64(id);


                if (Session["eTrac"] != null)
                {
                    if (Session != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                        long locationId = (long)Session["eTrac_SelectedDasboardLocationID"];
                        objPermissionDetails.GetAssignedPermission = _ICommonMethod.GetAssignPermission(Convert.ToInt32(userId), locationId);
                        //objPermissionDetails.GetPermission = _ICommonMethod.GetAllPermissions(locationId);

                        objPermissionDetails.GetPermission = _ICommonMethod.GetPermissionsWithFilterByUserTypeLocationId(locationId, Convert.ToInt32(userId));
                        objPermissionDetails.UserIdToSave = userId;
                    }
                }

                return PartialView("_Permissions", objPermissionDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// To Set Premission
        /// </summary>
        /// <ModifiedBy>Manoj Jaswal</ModifiedBy>
        /// <ModeifiedDate>2015/03/09</ModeifiedDate>
        /// <param name="objPermissionDetails"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SavePermissions(PermissionDetailsModel objPermissionDetails)
        {
            eTracLoginModel ObjLoginModel = null;
            try
            {
                AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
                if (Session["eTrac"] != null)
                {

                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    IsMapped IsMapped = _ICommonMethod.isUserMappedWithLocation(objPermissionDetails.UserIdToSave, (long)Session["eTrac_SelectedDasboardLocationID"]);
                    if (IsMapped.IsMappedLocation)
                    {
                        objPermissionDetails.UserId = objPermissionDetails.UserIdToSave;
                        objPermissionDetails.CreatedBy = ObjLoginModel.UserId;
                        objPermissionDetails.LocationId = ObjLoginModel.LocationID;
                        var Result = _ICommonMethod.UpdateUserPermissions(objPermissionDetails);
                        if (Result)
                        {
                            if (IsMapped.userTypeRes == 2 || IsMapped.userTypeRes == 6)
                            {
                                //Added By Bhushan Dod on 11/07/2016 for when roles assign to user then according to roles,Widget save by deafult.
                                _IGlobalAdmin.SaveByDefaultWidgetSetting(ObjLoginModel.LocationID, objPermissionDetails.UserIds, objPermissionDetails.UserIdToSave);
                            }

                            TempData["Message"] = CommonMessage.UpdateSuccessMessage();
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                            return Json(CommonMessage.UpdateSuccessMessage());
                        }
                        else
                        {
                            TempData["Message"] = CommonMessage.FailureMessage();
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                            return Json(CommonMessage.FailureMessage());
                        }
                    }
                    else
                    {
                        return Json("NotRegistered");
                    }
                }
                else { return Json("Session Expired !"); }


            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult SaveDashboardWidgetSetting(string WidgetIds)
        {
            eTracLoginModel ObjLoginModel = null;
            AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
            try
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    long loc = Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]);

                    var Result = _IDashboardWidgetSettingManager.UpdateDashboardWidgets(ObjLoginModel.UserId, loc, WidgetIds);
                    if (Result)
                    {
                        TempData["Message"] = CommonMessage.UpdateSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        Session["eTrac_DashboardWidget"] = null;
                        Session["eTrac_DashboardWidget"] = this.GetUserDashboardWidgetRoles();
                        return Json(Result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        TempData["Message"] = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                        return Json(Result, JsonRequestBehavior.AllowGet);
                    }

                }
                else { return Json("Session Expired !", JsonRequestBehavior.AllowGet); }


            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public WidgetList GetUserDashboardWidgetRoles()
        {
            long locationId = 0;
            eTracLoginModel ObjLoginModel = null;

            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                locationId = Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]);
                return _ILogin.GetDashboardWidgetList(ObjLoginModel.UserId, locationId);
            }
            else
            {
                return null;
            }



        }

        public ActionResult _AssignLocationAndRoles(string id, string name)
        {
            try
            {
                long userId;

                PermissionDetailsModel objPermissionDetails = new PermissionDetailsModel();
                if (!string.IsNullOrEmpty(id))
                {
                    id = Cryptography.GetDecryptedData(id, true);
                }
                userId = Convert.ToInt64(id);
                objPermissionDetails.UserId = userId;
                objPermissionDetails.FullName = name;
                objPermissionDetails.UserType = _ICommonMethod.GetUserByID(userId).UserType;
                ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
                return PartialView("_AssignLocationAndRoles", objPermissionDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ActionName("AssignLocationBasedRole")]
        public ActionResult GetLocationServicePermissionListForAssignRolesandLocation(long LocationID, long UserType)
        {
            try
            {
                PermissionDetailsModel objPermissionDetails = new PermissionDetailsModel();
                if (LocationID > 0 && UserType > 0)
                {
                    objPermissionDetails.GetPermission = _ICommonMethod.GetPermissionsWithUserType(LocationID, UserType);
                    return PartialView("_GetCheckboxDesignRole", objPermissionDetails);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }


        }

        /// <summary>
        /// To Assign location and roles of unassigned user.
        /// </summary>
        /// <ModifiedBy>Bhushan Dod</ModifiedBy>
        /// <ModifiedDate>2016/11/25</ModifiedDate>
        /// <param name="objPermissionDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("SaveLocationAndRoles")]
        public ActionResult SaveAssignLocationandRolesPermissions(PermissionDetailsModel objPermissionDetails)
        {
            DARModel objDAR = null;
            eTracLoginModel ObjLoginModel = null;
            try
            {

                AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
                if (objPermissionDetails != null && objPermissionDetails.LocationId > 0 && objPermissionDetails.UserIds != null)
                {
                    if (Session["eTrac"] != null)
                    {

                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);

                        if (ObjLoginModel != null)
                        {
                            objPermissionDetails.CreatedBy = ObjLoginModel.UserId;
                            objPermissionDetails.CreatedOn = DateTime.UtcNow;

                            objDAR = new DARModel();
                            objDAR.LocationId = objPermissionDetails.LocationId;
                            objDAR.UserId = ObjLoginModel.UserId;
                            objDAR.CreatedBy = ObjLoginModel.UserId;
                            objDAR.CreatedOn = DateTime.UtcNow;

                            objDAR.TaskType = (long)TaskTypeCategory.AssignLocationPermission;
                            objPermissionDetails.FullName = objPermissionDetails.FullName.ToTitleCase();
                            switch (objPermissionDetails.UserType)
                            {
                                case 2:
                                    {
                                        objDAR.ActivityDetails = DarMessage.LocationAssignedManager(objPermissionDetails.FullName, objPermissionDetails.LocationName);
                                        break;
                                    }
                                case 3:
                                    {
                                        objDAR.ActivityDetails = DarMessage.LocationAssignedEmployee(objPermissionDetails.FullName, objPermissionDetails.LocationName);
                                        break;
                                    }
                                case 4:
                                    {
                                        objDAR.ActivityDetails = DarMessage.LocationAssigned(objPermissionDetails.FullName, objPermissionDetails.LocationName);
                                        break;
                                    }
                                case 5:
                                    {
                                        objDAR.ActivityDetails = DarMessage.LocationAssigned(objPermissionDetails.FullName, objPermissionDetails.LocationName);
                                        break;
                                    }
                                case 6:
                                    {
                                        objDAR.ActivityDetails = DarMessage.LocationAssignedAdmin(objPermissionDetails.FullName, objPermissionDetails.LocationName);
                                        break;
                                    }
                                default:
                                    {
                                        objDAR.ActivityDetails = "Something went wrong";
                                        break;
                                    }
                            }

                            var result = _ICommonMethod.AssignLocationRoles(objPermissionDetails, objDAR, ObjLoginModel.UserId);
                            if (result == Result.Completed)
                            {
                                ViewBag.Message = CommonMessage.SaveSuccessMessage();
                                ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                                ModelState.Clear();

                            }
                            else
                            {
                                ViewBag.Message = CommonMessage.FailureMessage();
                                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                            }
                            return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("NotRegistered");
                        }
                    }
                    else { return Json("Session Expired !"); }
                }
                else
                {
                    ViewBag.Message = CommonMessage.FillAllRequired();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                }
                return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);



            }
            catch (Exception)
            {
                ViewBag.Message = CommonMessage.FailureMessage();
                ViewBag.AlertMessageClass = "text-danger";// store the failure message in tempdata to display in view.

                return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
