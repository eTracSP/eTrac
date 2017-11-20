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
    
    public partial class eFleetVehicleIncident
    {
        public long IncidentID { get; set; }
        public long VehicleID { get; set; }
        public string QRCodeID { get; set; }
        public string VehicleNumber { get; set; }
        public string DriverName { get; set; }
        public long LocationID { get; set; }
        public Nullable<System.DateTime> AccidentDate { get; set; }
        public Nullable<long> StateId { get; set; }
        public string City { get; set; }
        public string NumberOfInjuries { get; set; }
        public Nullable<bool> Preventability { get; set; }
        public string Description { get; set; }
        public string IncidentImage { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    
        public virtual eFleetVehicle eFleetVehicle { get; set; }
        public virtual UserRegistration UserRegistration { get; set; }
        public virtual LocationMaster LocationMaster { get; set; }
    }
}
