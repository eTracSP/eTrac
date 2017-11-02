using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WorkOrderEMS.Models
{
    [DataContract]
    public class ServiceWorkAssignmentModel
    {
        [DataMember]
        public string ServiceAuthKey { get; set; }
        [DataMember]
        public long WorkRequestAssignmentID { get; set; }
        [DataMember]
        public Nullable<long> AssetID { get; set; }
        [DataMember]
        public string AssetName { get; set; }
        [DataMember]
        public string QRCName { get; set; }
        [DataMember]
        public Nullable<long> WorkRequestType { get; set; }
        [DataMember]
        public string WorkRequestTypeName { get; set; }
        [DataMember]
        public string ProblemDescription { get; set; }
        [DataMember]
        public string ProjectDescription { get; set; }
        [DataMember]
        public Nullable<long> WorkRequestStatus { get; set; }
        [DataMember]
        public string WorkRequestStatusName { get; set; }
        [DataMember]
        public long WorkRequestProjectType { get; set; }
        [DataMember]
        public string WorkRequestProjectTypeName { get; set; }
        [DataMember]
        public Nullable<long> PriorityLevel { get; set; }
        [DataMember]
        public Nullable<bool> SafetyHazard { get; set; }
        [DataMember]
        public long LocationID { get; set; }
        [DataMember]
        public string LocationName { get; set; }
        [DataMember]
        public long RequestBy { get; set; }
        [DataMember]
        public string RequestByName { get; set; }
        [DataMember]
        public Nullable<long> AssignByUserId { get; set; }
        [DataMember]
        public Nullable<long> AssignToUserId { get; set; }
        [DataMember]
        public string AssignByUserName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string UserType { get; set; }
        [DataMember]
        public string Remarks { get; set; }
        [DataMember]
        public string WorkRequestImage { get; set; }

        [DataMember]
        public string WorkrequestCode { get; set; }
        [DataMember]
        public string WorkOrderCode { get; set; }
        [DataMember]
        public long WorkOrderCodeID { get; set; }
          [DataMember]
        public string CustomerName { get; set; }
          [DataMember]
          public string FrCurrentLocation { get; set; }
          [DataMember]
        public string DriverLicenseNo { get; set; }
          [DataMember]
        public string CustomerContact { get; set; }
          [DataMember]
        public string VehicleYear { get; set; }
          [DataMember]
        public string VehicleColor { get; set; }
          [DataMember]
        public string VehicleMake1 { get; set; }
          [DataMember]
        public string VehicleModel1 { get; set; }
          [DataMember]
        public string FacilityRequest { get; set; }
          [DataMember]
        public string AddressFacilityReq { get; set; }
          [DataMember]
          public string LicensePlateNo { get; set; }
        [DataMember]
        public string ResponseMessage { get; set; }
        [DataMember]
        public int Response { get; set; }
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public string AssignedTime { get; set; }
        [DataMember]
        public string WeekDaysName { get; set; }
        [DataMember]
        public Nullable<long> PauseStatus { get; set; }
    }

    [DataContract]
    public class ServiceWorkStatusModel
    {
        [DataMember]
        public long UserId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string ServiceAuthKey { get; set; }
        [DataMember]
        public long WorkRequestAssignmentID { get; set; }
        [DataMember]
        public int WorkRequestStatus { get; set; }
        [DataMember]
        public int LocationID { get; set; }
        [DataMember]
        public long UserRole { get; set; }
        [DataMember]
        public string ClientUserName { get; set; }
        [DataMember]
        public int WorkRequestType { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public string EndTime { get; set; }
        [DataMember]
        public string LocationName { get; set; }
        [DataMember]
        public string WorkStatusDesc { get; set; }
        [DataMember]
        public long ManagerId { get; set; }
        [DataMember]
        public string AcitivityDetails { get; set; }
        [DataMember]
        public string TimeZoneName { get; set; }
        [DataMember]
        public long TimeZoneOffset { get; set; }
        [DataMember]
        public bool IsTimeZoneinDaylight { get; set; }
    }

    [DataContract]
    public class ServiceWorkOrderAcceptanceModel
    {
        [DataMember]
        public long UserId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string ServiceAuthKey { get; set; }
        [DataMember]
        public long WorkRequestAssignmentID { get; set; }
        [DataMember]
        public int WorkRequestStatus { get; set; }
        [DataMember]
        public int LocationID { get; set; }
        //[DataMember]
        //public string LocationName { get; set; }
        [DataMember]
        public string AssignByUserName { get; set; }
        [DataMember]
        public string LocationName { get; set; }
        [DataMember]
        public string WorkStatusDesc { get; set; }
        [DataMember]
        public long Status { get; set; }

    }
}
