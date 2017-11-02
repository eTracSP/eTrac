using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    [DataContract]
    public class ServicePushModel
    {
        [DataMember]
        public string ServiceAuthKey { get; set; }
        [DataMember]
        public string DeviceId { get; set; }
        [DataMember]
        public Nullable<long> DeviceType { get; set; }
        [DataMember]
        public long UserId { get; set; }
    }
}
