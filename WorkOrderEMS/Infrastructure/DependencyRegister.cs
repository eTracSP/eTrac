using StructureMap;
using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.BusinessLogic.Interfaces.eFleet;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.BusinessLogic.Managers.eFleet;

namespace WorkOrderEMS.Infrastructure
{
    public class DependencyRegister : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestcontext, Type controllerType)
        {
            if ((controllerType == null) || (requestcontext == null))
                return null;
            return (Controller)ObjectFactory.GetInstance(controllerType);
        }

        public static class StructureMapper
        {
            public static void Run()
            {
                ControllerBuilder.Current
                    .SetControllerFactory(new DependencyRegister());

                ObjectFactory.Initialize(action =>
                {
                    action.AddRegistry(new RepositoryRegistry());
                });
            }
        }
   
        /// <summary>        
        /// To Map Interface with Repository
        /// </summary>
        /// <CreatedBy>Gayatri Pal</CreatedBy>
        /// <CreatedDate>30thJun2014</CreatedDate>
        public class RepositoryRegistry : Registry
        {
            public RepositoryRegistry()
            {

                For<ICommonMethod>().Use<CommonMethodManager>();
                For<IGlobalAdmin>().Use<GlobalAdminManager>();
                For<IQRCSetup>().Use<QRCSetupManager>();
                For<IManageManager>().Use<ManageManager>();
                For<IEmployeeManager>().Use<EmployeeManager>();
                For<IClientManager>().Use<ClientManager>();
                For<IReportManager>().Use<ReportManager>();
                For<ICommonMethodAdmin>().Use<CommonMethodAdmin>();
                For<ILogin>().Use<LoginManager>();
                For<IDARManager>().Use<DARManager>();
                For<IUser>().Use<UserManager>();        
                For<IWorkRequestAssignment>().Use<WorkRequestManager>();
                For<IEmailDetail>().Use<EmailDetailManager>();
                For<IDashboardWidgetSettingManager>().Use<DashboardWidgetSettingManager>();
                For<IEfleetVehicle>().Use<VehicleManager>();
                For<IDriverEfleet>().Use<DriverManager>();
                For<IEfleetPM>().Use<PreventativeMaintenaceManager>();
                For<IEfleetVehicleIncidentReport>().Use<VehicleIncidentManager>();
                For<IEfleetMaintenance>().Use<MaintenanceManager>();
                For<IPassengerTracking>().Use<PassengerTrackingManager>();
                For<IHoursOfServices>().Use<HoursOfServicesManager>();
            }
        }
    }
}