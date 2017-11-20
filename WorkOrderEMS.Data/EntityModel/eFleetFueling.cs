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
    using System.Collections.Generic;
    
    public partial class eFleetFueling
    {
        public long FuelID { get; set; }
        public long VehicleID { get; set; }
        public string VehicleNumber { get; set; }
        public string QRCodeID { get; set; }
        public long LocationID { get; set; }
        public string Mileage { get; set; }
        public string CurrentFuel { get; set; }
        public long FuelType { get; set; }
        public string ReceiptNo { get; set; }
        public System.DateTime FuelingDate { get; set; }
        public Nullable<int> TankSize { get; set; }
        public decimal Gallons { get; set; }
        public decimal PricePerGallon { get; set; }
        public decimal Total { get; set; }
        public string GasStatioName { get; set; }
        public string CardNo { get; set; }
        public string DriverName { get; set; }
        public long DARId { get; set; }
        public string OtherComment { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    
        public virtual DARDetail DARDetail { get; set; }
        public virtual GlobalCode GlobalCode { get; set; }
        public virtual eFleetVehicle eFleetVehicle { get; set; }
        public virtual UserRegistration UserRegistration { get; set; }
        public virtual LocationMaster LocationMaster { get; set; }
    }
}
