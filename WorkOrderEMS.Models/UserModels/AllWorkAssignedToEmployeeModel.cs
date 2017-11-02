using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.UserModels
{
    public class AllWorkAssignedToEmployeeModel
    {
        public Nullable<long> RN { get; set; }
        public string WorkStatus { get; set; }
        public string Priority { get; set; }
        public long WorkRequestAssignmentID { get; set; }
        public Nullable<long> WorkRequestType { get; set; }
        public Nullable<long> AssetID { get; set; }
        public long LocationID { get; set; }
        public string ProblemDesc { get; set; }
        public Nullable<long> PriorityLevel { get; set; }
        public string WorkRequestImage { get; set; }
        public Nullable<bool> SafetyHazard { get; set; }
        public string ProjectDesc { get; set; }
        public long WorkRequestStatus { get; set; }
        public long RequestBy { get; set; }
        public Nullable<long> AssignToUserId { get; set; }
        public Nullable<long> AssignByUserId { get; set; }
        public string Remarks { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public long WorkRequestProjectType { get; set; }
        public string AssignedWorkOrderImage { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<System.DateTime> AssignedTime { get; set; }
        public string WorkStatusDesc { get; set; }
    }
}
