using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class InventoryMasterModel
    {
        public long InventoryID { get; set; }
        [Required]
        public string ItemName { get; set; }
        [Required]
        [DisplayName("Item Serial Number")]
        public string ItemCode { get; set; }
        [Required]
        [DisplayName("Item Type")]
        public long ItemType { get; set; }

        [DisplayName("Item Type")]
        public long itType { get; set; }


        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }
        public long Quantity { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public long LocationId { get; set; }
        [Required]
        [DisplayName("Item Ownership")]
        public Nullable<int> ItemOwnership { get; set; }

        //Added By Bhushan Dod on 03/17/2015 for if any changes in stock notify to manager and admin
        public string Location { get; set; }
        public string UserName { get; set; }
        public long UserId { get; set; }
    }
    public class InventoryMasterModelList
    {
        public long InventoryID { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public long ItemType { get; set; }
        public string ItemTypeName { get; set; }
        public string Description { get; set; }
        public long Quantity { get; set; }
        public Nullable<int> ItemOwnership { get; set; }

        public long AssginedQuantity { get; set; }
        public long LocationId { get; set; }
        public long AssignInventoryID { get; set; }
        public long AssignedUserID { get; set; }
        public DateTime IssueDate { get; set; }
        public long IssuedBy { get; set; }
        public string AssignedToName { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string CodeName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedDate { get; set; }

    }
    public class InventoryType
    {
        public string Inventory { get; set; }
        public string AssignedInventory { get; set; }
    }
}
