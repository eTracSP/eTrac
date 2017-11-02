using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    [DataContract]
    public class ServiceDARModel
    {
        [DataMember]
        public string ServiceAuthKey { get; set; }
        [DataMember]
        public long UserId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public long DARId { get; set; }
        [DataMember]
        public long LocationId { get; set; }
        [DataMember]
        public string ActivityDetails { get; set; }
        public bool IsManual { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        [DataMember]
        public long TaskType { get; set; }
        [DataMember]
        public string ResponseMessage { get; set; }
        [DataMember]
        public int Response { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public string EndTime { get; set; }
        [DataMember]
        public string LocationName { get; set; }
        [DataMember]
        public string StartTimeImage { get; set; }
        [DataMember]
        public string EndTimeImage { get; set; }     
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool FacilityRequest { get; set; }

        [DataMember]
        public long WorkAssignmentID { get; set; }
    }

    [DataContract]
    public class ServiceDARListModel
    {

        //[DataMember]
        //public long DARId { get; set; }

        //[DataMember]
        //public string ActivityDetails { get; set; }
        //public bool IsManual { get; set; }
        //public long CreatedBy { get; set; }
        //public Nullable<long> ModifiedBy { get; set; }
        //public Nullable<System.DateTime> ModifiedOn { get; set; }
        //public bool IsDeleted { get; set; }
        //public Nullable<long> DeletedBy { get; set; }
        //public Nullable<System.DateTime> DeletedOn { get; set; }
        [DataMember]
        public string ServiceAuthKey { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int LocationId { get; set; }
        [DataMember]
        public Nullable<int> TaskType { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }

        [DataMember]
        public long DARId { get; set; }
        [DataMember]
        public string Location_Name { get; set; }
        [DataMember]
        public string Employee_Name { get; set; }
        [DataMember]
        public string Activity_Details { get; set; }
        [DataMember]
        public string TaskTypeDetails { get; set; }
        [DataMember]
        public string CreatedOn { get; set; }
    }

    [DataContract]
    public class ServiceDarUpdateModel
    {
        [DataMember]
        public long UserId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string ServiceAuthKey { get; set; }
        [DataMember]
        public int LocationID { get; set; }
        [DataMember]
        public int TaskType { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public string EndTime { get; set; }
        [DataMember]
        public string LocationName { get; set; }
        [DataMember]
        public string WorkStatusDesc { get; set; }

    }

}
