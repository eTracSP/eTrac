using System;
using System.ComponentModel.DataAnnotations;

namespace WorkOrderEMS.Models
{
    public class DARModel
    {
        public long DARId { get; set; }
        public long UserId { get; set; }
        public long LocationId { get; set; }
        [Required(ErrorMessage = "Please enter activity details.")]
        public string ActivityDetails { get; set; }
        public bool IsManual { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public long TaskType { get; set; }
        public string UserName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string StartTimeImage { get; set; }
        public string EndTimeImage { get; set; }
        public string CreatedDate { get; set; }
        public string Description { get; set; }
    }

    public class DARModelList
    {
        public long DARId { get; set; }
        public string LocationName { get; set; }
        public string EmployeeName { get; set; }
        public string ActivityDetails { get; set; }
        public string Description { get; set; }
        public string CreatedOn { get; set; }
        public string TaskType { get; set; }
        public string StartTimeImage { get; set; }
        public string EndTimeImage { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public long TaskTypeInt { get; set; }
        public string DisclaimerFormFile { get; set; }
    }

    public class DARModelForPDFExport
    {
        public string CreatedBy { get; set; }
        public string ActivityDetails { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string StartTimeImage { get; set; }
        public string EndTimeImage { get; set; }
        public string SubmittedDate { get; set; }
        public string Description { get; set; }
    }
}
