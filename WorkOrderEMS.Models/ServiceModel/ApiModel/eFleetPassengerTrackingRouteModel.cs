using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.ServiceModel.ApiModel
{
    public class eFleetPassengerTrackingRouteServiceModel : ServiceBaseModel
    {
        public long ServiceType { get; set; }
    }
    public  class eFleetPassengerTrackingRouteModel
    {
        public long RouteID { get; set; }
        public string RouteName { get; set; }
        public long ServiceType { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
    }
}
