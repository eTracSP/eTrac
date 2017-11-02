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
    
    public partial class eFleetPassengerTrackingCount
    {
        public long PassengerCountID { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<long> LocationID { get; set; }
        public long ServiceType { get; set; }
        public long VehicleID { get; set; }
        public string VehicleNumber { get; set; }
        public long RouteID { get; set; }
        public Nullable<long> PassengerCount { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual eFleetPassengerTrackingRoute eFleetPassengerTrackingRoute { get; set; }
        public virtual eFleetVehicle eFleetVehicle { get; set; }
        public virtual GlobalCode GlobalCode { get; set; }
        public virtual UserRegistration UserRegistration { get; set; }
    }
}
