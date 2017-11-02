using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.ServiceModel
{
   public  class VehicleDetailsModel
    {

        public long VehicleID { get; set; }
        public string VehicleNumber { get; set; }
        public string QRCodeID { get; set; }
        public string VehicleIdentificationNo { get; set; }
    }
}
