using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Models
{
    public class eFleetVehicleModelForService
    {
        public long VehicleID { get; set; }
        public string VehicleNumber { get; set; }
        public string QRCodeID { get; set; }
        public long LocationID { get; set; }
        public string VehicleIdentificationNo { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string AttachmentOfRegistration { get; set; }
        public string AttachmentOfInsurance { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public string VehicleImage { get; set; }
        public string GVWR { get; set; }
        public string StorageAddress { get; set; }
        public string DummyField { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Result Result { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
