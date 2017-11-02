using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    [DataContract]
    public class ServiceDashboardModel
    {
        [DataMember]
        public long UserId { get; set; }
        [DataMember]
        public long LocationId { get; set; }
        [DataMember]
        public string ServiceAuthKey { get; set; }
        [DataMember]
        public Nullable<long> WorkRequestCount { get; set; }
        [DataMember]
        public Nullable<long> DarCount { get; set; }
        [DataMember]
        public Nullable<long> RuleViolationCount { get; set; }
        [DataMember]
        public Nullable<long> FacilityRequestCount { get; set; }
        [DataMember]
        public Nullable<long> FacilityRequestCountLocation { get; set; }
        [DataMember]
        public Nullable<long> ContinuousRequestCount { get; set; }

    }
}
