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
    
    public partial class LocationService
    {
        public long LocationServicesID { get; set; }
        public long LocationID { get; set; }
        public long ServiceId { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
    
        public virtual LocationMaster LocationMaster { get; set; }
        public virtual ServiceMaster ServiceMaster { get; set; }
    }
}