using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.ServiceModel
{
   public partial class VehicleScanModel
    {
        public string ServiceAuthKey { get; set; }
        public long VehicleID { get; set; }
        public string VehicleNumber { get; set; }
        public string QRCodeID { get; set; }
        public int QRCDefaultSize { get; set; }
        public long LocationID { get; set; }
        public string VehicleIdentificationNo { get; set; }
        public Nullable<int> FuelType { get; set; }
        //public string Make { get; set; }
        //public string Model { get; set; }
        //public string Year { get; set; }
        //public string AttachmentOfRegistration { get; set; }
        //public string AttachmentOfInsurance { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public string ChShDescription { get; set; }
        public string OldChShDescription { get; set; }
        public string VehicleImage { get; set; }
        //public string GVWR { get; set; }
        //public string StorageAddress { get; set; }
        //public string DummyField { get; set; }
        public Nullable<bool> CheckOutStatus { get; set; }
        public string UserName { get; set; }
        public Nullable<bool> IsDamage { get; set; }
        public Nullable<bool> DamageTireStatus { get; set; }
        public Nullable<bool> InteriorMileageDriverStatus { get; set; }
        public Nullable<bool> EngineExteriorStatus { get; set; }
        public Nullable<bool> EmergencyAccessoriesStatus { get; set; }
        public long VehicleScanLogId { get; set; }
        public long DarID { get; set; }
        //public eFleetDamageTireModel DamageTire { get; set; }
        //public eFleetInteriorMileageDriverModel InteriorMileageDriver { get; set; }
        //public eFleetEngineExteriorModel EngineExterior { get; set; }
        //public eFleetEmergencyAccessoriesModel EmergencyAccessories { get; set; }        
    }
}
