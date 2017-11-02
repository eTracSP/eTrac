using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class eFleetMeterModel
    {
        public long ID { get; set; }
        public Nullable<long> Meter { get; set; }
        public string MeterValue { get; set; }
    }
}
