using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.ServiceModel.ApiModel
{
    public class eFleetPassengerTrackingCountModelForService : ServiceBaseModel
    {
        public long PassengerCountID { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<long> LocationID { get; set; }
        public long ServiceType { get; set; }
        public long VehicleID { get; set; }
        public string VehicleNumber { get; set; }
        public long RouteID { get; set; }
        public Nullable<long> PassengerCount { get; set; }
    }
}
