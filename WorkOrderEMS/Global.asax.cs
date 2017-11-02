using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WorkOrderEMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string connString = ConfigurationManager.AppSettings["SQLConnection"].ToString();
        NotificationAlertRepository objNotificationAlertRepository = new NotificationAlertRepository();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            WorkOrderEMS.Infrastructure.DependencyRegister.StructureMapper.Run();

            //if (objNotificationAlertRepository.IsServerConnected(connString) == true)
            //{
            //Start SqlDependency with application initializationAuthenticat
            System.Data.SqlClient.SqlDependency.Start(connString);
            objNotificationAlertRepository.WorkOrderDetailsForPushNotificationSignalRGlobal(true);
            //  }

        }
        
        protected void Application_End()
        {
            //Stop SQL dependency
            System.Data.SqlClient.SqlDependency.Stop(connString);
        }

        protected void Session_Start()
        {

        }
            
        protected void Session_End()
        {
            Session.Abandon();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var error = Server.GetLastError();
             WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(error, "Application_Error", error.Message, error.InnerException);
            //ViewBag.Message = error.Message;
            //ViewBag.Error = error.Message;
            string errorpagepath = Server.MapPath("/Login/Index");

            //Response.Redirect(errorpagepath);
            //Server.Transfer("~/Views/Shared/Error.cshtml");

            if (error.Message != "Not Found")
            {
                Response.Redirect(errorpagepath);
                //Server.Transfer(errorpagepath);
            }
        }

    }
}
