using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WorkOrderEMS.Models
{
    [DataContract]
    public class ServiceResponseModel<T> 
    {
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public long Response { get; set; }

        [DataMember]
        public T Data { get; set; }      
    }


    [DataContract]
    public class ServiceResponseModel_Old
    {
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public long Response { get; set; }

        [DataMember]
        public Object Data { get; set; }

    }
    [DataContract]
    public class ServiceFedbackModel
    {
        [DataMember]
        public long UserId { get; set; }
        [DataMember]
        public long LocationId { get; set; }

        [DataMember]
        public long WorkAssignmentID { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string UserName { get; set; }
    }

}
