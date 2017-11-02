using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class TemplateModel : BaseTimeZoneParameter
    {
        public eFleetDamageTireModel DamageTire { get; set; }
        public eFleetInteriorMileageDriverModel InteriorMileageDriver { get; set; }
        public eFleetEngineExteriorModel EngineExterior { get; set; }
        public eFleetEmergencyAccessoriesModel EmergencyAccessories { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string QRCodeID { get; set; }
        public string VehicleNumber { get; set; }

    }
}
