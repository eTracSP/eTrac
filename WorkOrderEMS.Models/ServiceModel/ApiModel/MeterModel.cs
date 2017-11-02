using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.ServiceModel
{
    public class MeterModel
    {
        public string ServiceAuthKey { get; set; }
        public long UserId { get; set; }
    }
    public class MeterList
    {
        public long MeterID { get; set; }
        public string MeterValue { get; set; }
    }

}
