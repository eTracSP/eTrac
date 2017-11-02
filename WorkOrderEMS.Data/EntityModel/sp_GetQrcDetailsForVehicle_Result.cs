//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorkOrderEMS.Data.EntityModel
{
    using System;
    
    public partial class sp_GetQrcDetailsForVehicle_Result
    {
        public long QRCID { get; set; }
        public long VehicleID { get; set; }
        public string DriverName { get; set; }
        public string DriverProfilePic { get; set; }
        public string VehicleImage { get; set; }
        public string VehicleTagNo { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string CompanyName { get; set; }
        public string PointOfContact { get; set; }
        public string BusinessNo { get; set; }
        public Nullable<System.DateTime> TimeSpan { get; set; }
        public Nullable<decimal> QRCScanLogId { get; set; }
        public string VehicleType { get; set; }
        public Nullable<System.DateTime> PermitDuration { get; set; }
        public string CityPermitNo { get; set; }
        public string LicenseNo { get; set; }
        public Nullable<System.DateTime> LicenseExpiryDate { get; set; }
        public string PermitType { get; set; }
    }
}