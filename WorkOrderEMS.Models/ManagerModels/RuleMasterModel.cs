using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class RuleMasterModel
    {
        public long RuleID { get; set; }
        [Required]
        [DisplayName("Rule Name")]
        public string RuleName { get; set; }
        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }
        [Required]
        [DisplayName("Voilation Charges")]
        [Range(0, 9999999999.99)]
        public decimal VoilationCharges { get; set; }
        // [DisplayName("Is Active")]
        public bool IsActive { get; set; } // public bool? IsActive { get; set; } IF ANY ERROR OCCURE THEN YOU CAN REVERT 
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public long ProjectID { get; set; }
    }
    [DataContract]
    public class RuleMasterModelList
    {
        [DataMember]
        public long RuleID { get; set; }
        [DataMember]
        public string RuleName { get; set; }
        public string Description { get; set; }
        public decimal VoilationCharges { get; set; }
        public bool IsActive { get; set; }

        [DataMember]
        public string ServiceAuthKey { get; set; }
        [DataMember]
        public long LocationId { get; set; }
    }

}
