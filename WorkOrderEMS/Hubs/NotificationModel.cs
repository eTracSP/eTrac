using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkOrderEMS.Hubs
{
    public class NotificationModel
    {
        public class WorkRequestAssignment
        {
            public long LocationID { get; set; }
            public long WorkRequestAssignmentID { get; set; }
            public long WorkRequestProjectType { get; set; }
            public string WorkOrderCode { get; set; }
            public long WorkOrderCodeID { get; set; }
            public long CreatedBy { get; set; }
            public bool SafetyHazard { get; set; }
        }
    }
}