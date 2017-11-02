using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace WorkOrderEMS.App_Start
{
    public class CustomActionFilter : ActionFilterAttribute, IActionFilter
    {

        void IActionFilter.OnActionExecuting(ActionExecutingContext filtercontext)
        {
            try
            {
                if (filtercontext != null)
                {
                    string controllerName = filtercontext.ActionDescriptor.ControllerDescriptor.ControllerName;
                    if (controllerName != "Login")
                    {
                        if (filtercontext.HttpContext.Session["eTrac"] == null)
                        {
                            filtercontext.Result = new RedirectToRouteResult(new
                            RouteValueDictionary(new { controller = "Login", action = "Index" }));
                        }
                    }
                    else
                    {

                        //WorkOrderEMS.Models.eTracLoginModel result = _ILogin.AuthenticateUser(eTracLogin);

                        //Session["eTrac_SelectedDasboardLocationID"] = result.LocationID;
                        //Session["eTrac_UserRoles"] = this.Get_UserAssignedRoles();
                        //Session["eTrac_LocationServices"] = obj_Common_B.GetLocationServicesByLocationID(result.LocationID, result.UserRoleId);
                        //switch (result.UserRoleId)
                        //{
                        //    case ((Int64)(UserType.GlobalAdmin)):
                        //        Session["eTrac_UserLocations"] = _ILogin.GetUserAssignedLocations(result.UserRoleId, result.UserId);
                        //        Session["eTrac_UserRoles"] = Session["eTrac_LocationServices"]; // this line has been added by vijay bcz if usetype is GAdmin or ITAdmin then this type of users will be able too see all services which is assigned to this current location.

                        //        break;
                        //    case ((Int64)(UserType.ITAdministrator)):
                        //        Session["eTrac_UserLocations"] = _ILogin.GetUserAssignedLocations(result.UserRoleId, result.UserId);
                        //        Session["eTrac_UserRoles"] = Session["eTrac_LocationServices"];

                        //        break;
                        //    case ((Int64)(UserType.Administrator)):
                        //        Session["eTrac_UserLocations"] = _ILogin.GetAdminAssignedLocation(result.UserId);

                        //        break;
                        //    case ((Int64)(UserType.Manager)):
                        //        Session["eTrac_UserLocations"] = _ILogin.GetManagerAssignedLocation(result.UserId);

                        //        break;
                        //    case ((Int64)(UserType.Employee)):
                        //        Session["eTrac_UserLocations"] = _ILogin.GetEmployeeAssignedLocation(result.UserId);

                        //        break;
                        //    case ((Int64)(UserType.Client)):
                        //        //Session["eTrac_UserLocations"] = _ILogin.GetEmployeeAssignedLocation(result.UserId);

                        //        break;
                        //}
                    }
                }
            }
            catch (ArgumentNullException)
            {
                throw;
            }

        }




        //override public void OnActionExecuted(ActionExecutedContext filterContext)
        //{

        //}


        //public void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    //if (filterContext.HttpContext.Session["testlogin"] == null)
        //    //    (filterContext.Controller as WorkOrderEMS.Controllers.GlobalAdmin.GlobalAdminController).RedirectToAction("index");
        //}
    }

    public class InfractionCustomFilter : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filtercontext)
        {
            try
            {
                if (filtercontext != null)
                {
                    string controllerName = filtercontext.ActionDescriptor.ControllerDescriptor.ControllerName;
                    if (controllerName != "Login")
                    {
                        if (filtercontext.HttpContext.Session["eTrac"] == null)
                        {
                            filtercontext.Result = new RedirectToRouteResult(new
                            RouteValueDictionary(new { controller = "Login", action = "Index" }));
                        }
                    }
                }
            }
            catch (ArgumentNullException)
            {
                throw;
            }

        }

    }
    public class TimeZoneCustomFilter : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filtercontext)
        {
            try
            {
                if (System.Web.HttpContext.Current.Request.Cookies != null)
                {
                    if (System.Web.HttpContext.Current.Request.Cookies["timezoneoffset"] != null)
                    {
                        System.Web.HttpContext.Current.Session["timezoneoffset"] = System.Web.HttpContext.Current.Request.Cookies["timezoneoffset"].Value;
                    }
                }
            }
            catch (ArgumentNullException)
            {
                throw;
            }

        }

    }
}