using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.ServiceModel
{
    public class UpdateEfleetInspectionTypeXML
    {
        public Nullable<int> Response { get; set; }
        public string ResponseMessage { get; set; }
        public long VehicleID { get; set; }
        public string VehicleNumber { get; set; }
        public string QRCodeID { get; set; }
        public long LocationID { get; set; }
        public Nullable<bool> CheckOutStatus { get; set; }
        public Nullable<bool> IsDamage { get; set; }
        public Nullable<bool> DamageTireStatus { get; set; }
        public Nullable<bool> InteriorMileageDriverStatus { get; set; }
        public Nullable<bool> EngineExteriorStatus { get; set; }
        public Nullable<bool> EmergencyAccessoriesStatus { get; set; }
        public string DamageTireDetails { get; set; }
        public string InteriorMileageDriverDetails { get; set; }
        public string EngineExteriorDetails { get; set; }
        public string EmergencyAccessoriesDetails { get; set; }
        public string LocationName { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string InspectionStatusFile { get; set; }
    }
}
