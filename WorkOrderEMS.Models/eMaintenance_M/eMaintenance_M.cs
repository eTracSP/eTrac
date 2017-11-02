using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WorkOrderEMS.Models.eMaintenance_M
{
    [DataContract]
    public class eMaintenance_M
    {
        [DataMember]
        public int UserId { get; set; }


        [DataMember]
        public string htmlContent { get; set; }

        [DataMember]
        public long WorkRequestAssignmentRequestId { get; set; }

    }


    public partial class WorkRequestAssignment_M
    {
        public long WorkRequestAssignmentID { get; set; }
        public long LocationID { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string CustomerContact { get; set; }
        public string DriverLicenseNo { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public Nullable<int> VehicleYear { get; set; }
        public string VehicleColor { get; set; }
        public string CurrentLocation { get; set; }
        public string LicensePlateNo { get; set; }
        public Nullable<long> FacilityRequestId { get; set; }
        public string FacilityRequestName { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string AssignedFirstName { get; set; }
        public string AssignedLastName { get; set; }
    }



    public class eMaintenanceSurvey_M
    {

        [Display(Name = "A. Did the attendant introduce themselves when initially meeting you?")]
        public string question1 { get; set; }
        public string ans1 { get; set; }
        [Display(Name = "B. Did the attendant help you accomplish your goal for requesting the service?")]
        public string question2 { get; set; }
        public string ans2 { get; set; }

        public string Ques2Comment { get; set; }
        public string Ques3Comment { get; set; }
        public string Ques5Comment { get; set; }
        public string Ques4Comment { get; set; }


        [Display(Name = "C. On a scale of 1 to 5, 5 being extremely fast and 1 extremely slow how would rate the time it took the attendant to get to you after you disconnected the call?")]
        public string question3 { get; set; }
        public string ans3 { get; set; }
        [Display(Name = "D. On a Scale of 1 to 5, 5 being extremely helpful and 1 not helpful, how would you rate the attendants level of helpfulness during the service?")]
        public string question4 { get; set; }
        public string ans4 { get; set; }
        [Display(Name = "E. On a scale of 1-5, 5 being extremely satisfied and 1 being extremely dissatisfied how would you rate the overall service and experience you received while utilizing our facility?")]
        public string question5 { get; set; }
        public string ans5 { get; set; }
        [Display(Name = "F. Would you like to be contacted by a member of management to discuss the service you received?")]
        public string question6 { get; set; }
        public string ans6 { get; set; }
        public long CreatedBy { get; set; }

        public string UserIds { get; set; }
        public string WorkAssignmentIds { get; set; }
        public string SurveyEmailID { get; set; }
        public string SurveyEmailIDs { get; set; }

        public long UserId { get; set; }
        public long WorkAssignmentId { get; set; }
        public string HtmlData { get; set; }
    }
    public class EMaintenanceSurvey
    {
        [ScaffoldColumn(false)]
        public long SurveyId { get; set; }
        public long GlobalCodeId { get; set; }
        public byte Answer { get; set; }
        public long SubmittedBy { get; set; }
        public long LocationId { get; set; }
        public string IfNoThenDescription { get; set; }

    }


}
