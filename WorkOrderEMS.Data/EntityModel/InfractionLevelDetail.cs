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
    
    public partial class InfractionLevelDetail
    {
        public long InfractionLevelDetailsId { get; set; }
        public long InfractionId { get; set; }
        public long IsApporvedByManager { get; set; }
        public Nullable<long> LevelByManager { get; set; }
        public string CommentByManager { get; set; }
        public Nullable<System.DateTime> TimeByManager { get; set; }
        public long IsApporvedByClient { get; set; }
        public Nullable<long> LevelByClient { get; set; }
        public string CommentByClient { get; set; }
        public Nullable<System.DateTime> TimeByClient { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual Infraction Infraction { get; set; }
        public virtual GlobalCode GlobalCode { get; set; }
        public virtual GlobalCode GlobalCode1 { get; set; }
        public virtual GlobalCode GlobalCode2 { get; set; }
        public virtual GlobalCode GlobalCode3 { get; set; }
    }
}
