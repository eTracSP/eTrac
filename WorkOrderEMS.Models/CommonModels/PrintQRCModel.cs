using System.Collections.Generic;

namespace WorkOrderEMS.Models
{
    public class PrintQRCModel
    {
        //QR Code        
        public long QRCId { get; set; }
        public string EncryptQRC { get; set; }
        public string EncryptLastQRC { get; set; }
        public string QRCName { get; set; }
        public string SpecialNotes { get; set; }
        public long QRCDefaultSize { get; set; }
        public string QRCTYPE { get; set; }
        public List<GlobalCodeModel> QRCSize { get; set; }
        public long LocationId { get; set; }
        public string Location { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public long QRCTYPEID { get; set; }

        //Vehicle
        public long VehicleID { get; set; }
        public string VehicleImage { get; set; }
        public string VehicleType { get; set; }
        public string MotorType { get; set; }
        public string SizeCaption { get; set; }
        //Driver
        public string DriverImage { get; set; }
        public string DriverName { get; set; }

        //Company
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyImage { get; set; }

        //Vendor
        public bool IsVendor { get; set; }
        public string VendorName { get; set; }
        public string VendorDetails { get; set; }
        public string QRCIDCode { get; set; }
    }
}
