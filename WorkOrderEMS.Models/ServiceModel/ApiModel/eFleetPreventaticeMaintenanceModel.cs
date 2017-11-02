using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.ServiceModel
{
    public class eFleetPreventaticeMaintenanceModel
    {
        public string ServiceAuthKey { get; set; }
        public long UserId { get; set; }
        public long VehicleID { get; set; }
        public string VehicleNumber { get; set; }
        public string QrCodeId { get; set; }
        public long LocationID { get; set; }
        public Nullable<long> Category { get; set; }
        public Nullable<long> Meter { get; set; }
        public Nullable<long> ReminderMetric { get; set; }
        public string ReminderMetricDesc { get; set; }    
        public string OtherComment { get; set; }       
        public Nullable<System.DateTime> ServiceDueDate { get; set; }
        public string  LocationName { get; set; }
    }
}
