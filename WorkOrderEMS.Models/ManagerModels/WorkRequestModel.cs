using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkOrderEMS.Models
{
    public class WorkRequestModel
    {
        public long WorkRequestID { get; set; }
        public long RequestBy { get; set; }

        public long ProjectId { get; set; }
        [Required]
        [Display(Name = "Task Name")]
        public string TaskName { get; set; }
        [Required]
        [Display(Name = "Work Area")]
        public long WorkArea { get; set; }

        [Required]
        [Display(Name = "Task Type")]
        public long? TaskType { get; set; }

        [Required]
        [Display(Name = "Task Priority")]
        public long TaskPriority { get; set; }

        [Display(Name = "Asset")]
        public long AssetID { get; set; }

        public string Remarks { get; set; }
        public string StartTime { get; set; }
        public string CompletionTime { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public string status { get; set; }


    }
    public class WorkRequestModelList
    {
        public long WorkRequestID { get; set; }
        public long WorkOrderID { get; set; }
        public long RequestBy { get; set; }
        public string RequestByName { get; set; }
        public long ProjectId { get; set; }
        public string TaskName { get; set; }
        public long WorkArea { get; set; }
        public string AreaName { get; set; }
        public long? TaskType { get; set; }
        public string TaskTypeName { get; set; }
        public long TaskPriority { get; set; }
        public string TaskPriorityName { get; set; }
        public string Remarks { get; set; }
        public string StartTime { get; set; }
        public string CompletionTime { get; set; }
        public long AssignedToUser { get; set; }
        public string AssignedToUserName { get; set; }
        public long TaskStatus { get; set; }
        public string TaskStatusName { get; set; }
        public long AssetID { get; set; }
        public string AssetNo { get; set; }
    }

    public class CheckWorkRequestforDeleteUser
    {
        public List<ServiceWorkAssignmentModel> ContinuousList { get; set; }
        public List<ServiceWorkAssignmentModel> NormalList { get; set; }
    }
}
