using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.ServiceModel.ApiModel
{

    public class eFleetMaintenanceModelForApiService :ServiceBaseModel
    {       
        public long VehicleID { get; set; }
        public string VehicleNumber { get; set; }    
        public long MaintenanceType { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public Nullable<long> PmID { get; set; }
        public string ReminderMetricDesc { get; set; }
        public string DriverName { get; set; }
        public Nullable<int> DaysOutOfService { get; set; }
        public Nullable<decimal> PartsCost { get; set; }
        public Nullable<decimal> LabourCost { get; set; }
        public Nullable<decimal> TotalCost { get; set; }
        public string Miles { get; set; }
        public string Note { get; set; }
    }
}
