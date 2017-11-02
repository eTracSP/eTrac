using System.Runtime.Serialization;

namespace WorkOrderEMS.Models
{
    [DataContract]
    public class ServiceImageUpload
    {
            [DataMember]
            public string Image { get; set; }
            [DataMember]
            public string ImageModuleName { get; set; }
            [DataMember]
            public string ImageUrl { get; set; }
            [DataMember]
            public string ImageEmp { get; set; }
            [DataMember]
            public string ImageModuleNameEmp { get; set; }
            [DataMember]
            public string ImageUrlEmp { get; set; }
            [DataMember]
            public bool IsDecline { get; set; }

            [DataMember]
            public long UserId { get; set; }
            [DataMember]
            public long WorkAssignmentId { get; set; }

            [DataMember]
            public string StartTime { get; set; }
            [DataMember]
            public string EndTime { get; set; }
            [DataMember]
            public string TotalTime { get; set; }
            [DataMember]
            public string TimeZoneName { get; set; }
            [DataMember]
            public long TimeZoneOffset { get; set; }
            [DataMember]
            public bool IsTimeZoneinDaylight { get; set; }
    }
}
