using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    [DataContract]
    public class ServiceDurationModel
    {
        [DataMember]
        public string ServiceAuthKey { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public long UserId { get; set; }
        [DataMember]
        public long LocationId { get; set; }

        [DataMember]
        public string TimeZoneName { get; set; }
        [DataMember]
        public long TimeZoneOffset { get; set; }
        [DataMember]
        public bool IsTimeZoneinDaylight { get; set; }
    }
}
