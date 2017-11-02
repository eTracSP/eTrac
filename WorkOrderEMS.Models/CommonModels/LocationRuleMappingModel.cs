using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
   public class LocationRuleMappingModel
    {
        public long RuleMappingId { get; set; }
        public long RuleID { get; set; }
        public decimal VoilationCharges { get; set; }
        public Nullable<long> LocationID { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }      
    }
}
