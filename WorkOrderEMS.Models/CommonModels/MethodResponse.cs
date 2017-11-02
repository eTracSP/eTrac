using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Models
{
    public class MethodResponse
    {
        public Result Result { get; set; }
        public string Message { get; set; }
        public string vendorID { get; set; }
    }
}
