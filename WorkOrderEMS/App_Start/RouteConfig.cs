using System.Web.Mvc;
using System.Web.Routing;

namespace WorkOrderEMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
               name: "Login",
               url: "",
                //defaults: new { controller = "GlobalAdmin", action = "login", id = UrlParameter.Optional }
               defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "{controller}", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
