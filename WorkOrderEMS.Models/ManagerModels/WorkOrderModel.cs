using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class WorkOrderModel
    {
        public long WorkOrderID { get; set; }
        public long ProjectId { get; set; }
        [Display(Name = "Requested By")]
        public string RequestByName { get; set; }
        [Required]
        [Display(Name = "Task Name")]
        public string TaskName { get; set; }
        [Required]
        [Display(Name = "Work Area")]
        public long WorkArea { get; set; }
        [Display(Name = "Work Area")]
        public string WorkAreaName { get; set; }
        [Required]
        [Display(Name = "Task Type")]
        public long? TaskType { get; set; }
        [Display(Name = "Task Type")]
        public string TaskTypeName { get; set; }
        [Required]
        [Display(Name = "Task Priority")]
        public long TaskPriority { get; set; }
        [Display(Name = "Task Priority")]
        public string TaskPriorityName { get; set; }
        [Required]
        [Display(Name = "Assigned To")]
        public long AssignedToUser { get; set; }

        [Display(Name = "Assigned To")]
        public string AssignedToName { get; set; }
        public string Remarks { get; set; }
        [Required]
        [Display(Name = "Start Time")]
        public string StartTime { get; set; }
        [Required]
        [Display(Name = "Completion Time")]
        public string CompletionTime { get; set; }
        public long TaskStatus { get; set; }
        [Display(Name = "Status")]
        public string TaskStatusName { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public long WorkRequestID { get; set; }
        public long AssetID { get; set; }
    
    }
}
