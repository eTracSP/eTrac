using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.ServiceModel
{
    public class ChangeInspectionStatusModel
    {
        public string ServiceAuthKey { get; set; }

        public long UserId { get; set; }

        public long VehicleID { get; set; }

        public long LocationId { get; set; }

        public string UserName { get; set; }

        public string QrcodeId { get; set; }

        public bool CheckOutStatus { get; set; }

        public string InspectionStatusFile { get; set; }
    }
}
