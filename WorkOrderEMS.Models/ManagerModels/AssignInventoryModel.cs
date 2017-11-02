using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class AssignInventoryModel
    {
        public long AssignInventoryID { get; set; }
        
        [Required]
        [DisplayName("Inventory")]
        public long InventoryID { get; set; }        
        [Required]
        [DisplayName("Assigned To")]
        public long AssignedUserID { get; set; }
        [Required]
        [DisplayName("Issue Date")]
        public string IssueDate { get; set; }

        public long IssuedBy { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public long Quantity { get; set; }
        public Nullable<long> AssginedQuantity { get; set; }
        public Nullable<long> RemainingQuantity { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RemainingQuantity > Quantity)
            {
                yield return new ValidationResult("AssginedQuantity should less than Quantity", new[] { "Quantity" });
            }
        }

        //Added By Bhushan Dod on 03/17/2015 for if any changes in stock notify to manager and admin
        public string Location { get; set; }
        public string UserName { get; set; }
        public long UserId { get; set; }
        public long LocationId { get; set; }
    }
}
