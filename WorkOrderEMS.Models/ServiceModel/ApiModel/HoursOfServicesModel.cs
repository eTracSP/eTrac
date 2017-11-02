using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.ServiceModel.ApiModel
{
    public class HoursOfServicesModel :  ServiceBaseModel
    {
        public long HoursID { get; set; }
        //public int IsSecondaryUserAbleToWork { get; set; }
        [DataType(DataType.Time)]
        public string StartTime { get; set; }

        [DataType(DataType.Date)]
        public string StartDate { get; set; }

        [DataType(DataType.Time)]
        public string EndTime { get; set; }

        [DataType(DataType.Date)]
        public string EndDate { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
    }
}
