using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Models.CommonModels
{
    //Added by Bhushan Dod on 08/06/2015 for weekdays name
    public class WeekDaysModel
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public bool IsChecked { get; set; }
    }

    public class WorkRequestAssignmentModel
    {
        public long WorkRequestAssignmentID { get; set; }

        [Display(Name = "WorkRequest Type")]
        [Required(ErrorMessage = "Please select request type.")]
        public Nullable<long> WorkRequestType { get; set; }
        [Display(Name = "Asset")]
        [Required(ErrorMessage = "Please select asset.")]
        public Nullable<long> AssetID { get; set; }
        [Display(Name = "Location")]
        //[Required(ErrorMessage="Please Select Location")]
        public long LocationID { get; set; }

        [Required(ErrorMessage = "Please enter problem description.")]
        //[RegularExpression(@"^(?![\W_]+$)(?!\d+$)[a-zA-Z0-9 .&',_-]+$", ErrorMessage = "Special characters are not allowed.")]
        [Display(Name = "Problem Description")]
        public string ProblemDesc { get; set; }
        [Display(Name = "Priority Level")]
        [Required(ErrorMessage = "Please select priority level.")]
        public Nullable<long> PriorityLevel { get; set; }
        public string WorkRequestImage { get; set; }

        public Nullable<bool> SafetyHazard { get; set; }
        [Required(ErrorMessage = "Please enter project description.")]
        //[RegularExpression(@"^(?![\W_]+$)(?!\d+$)[a-zA-Z0-9 .&',_-]+$", ErrorMessage = "Special characters are not allowed.")]
        [Display(Name = "Project Description")]
        public string ProjectDesc { get; set; }
        public long WorkRequestStatus { get; set; }
        public long RequestBy { get; set; }
        [Display(Name = "Employee")]
        [Required(ErrorMessage = "Please select employee.")]
        public Nullable<long> AssignToUserId { get; set; }
        public Nullable<long> AssignByUserId { get; set; }
        public string Remarks { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        [Display(Name = "Project Type")]
        public Nullable<long> WorkRequestProjectType { get; set; }
        [Required, FileExtensions(Extensions = "jpg,jpeg,png",
             ErrorMessage = "Specify a Imgae file.")]
        public HttpPostedFileBase WorkRequestImg { get; set; }
        [Display(Name = "Work Order Image")]
        public HttpPostedFileBase WorkOrderImage { get; set; }
        public string AssignedWorkOrderImage { get; set; }
        public Result Result { get; set; }
        [Display(Name = "Assign Time Limit")]
        public Nullable<DateTime> AssignedTime { get; set; }
        public Nullable<System.DateTime> ConStartTime { get; set; }
        
        public Nullable<System.DateTime> StartTime { get; set; }
        [Required(ErrorMessage = "Start Time field is required.")]
        [Display(Name = "Start Time")]
        public string CrStartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string WorkStatusDesc { get; set; }
        public string AssignedTimeInterval { get; set; }
        public string WorkOrderCode { get; set; }
        public string WorkOrderCodeForPush { get; set; }
        public long WorkOrderCodeID { get; set; }
        public string WorkRequestTy { get; set; }
        public bool UpdateMode { get; set; }
        [Required(ErrorMessage = "Facility Request is required.")]
        public long FacilityRequestId { get; set; }
        public string DisclaimerForm { get; set; }
        public string SurveyForm { get; set; }

        // For Facility Request
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Customer Name is required.")]
        [StringLength(50, MinimumLength = 1)]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Display(Name = "Address")]
        [RegularExpression(@"^(?![\W_]+$)(?!\d+$)[a-zA-Z0-9 .&',_-]+$", ErrorMessage = "Special characters are not allowed.")]
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Vehilce Make is required.")]
        [Display(Name = "Vehicle Make")]
        public string VehicleMake { get; set; }

        [Display(Name = "Vehicle Model")]
        [Required(ErrorMessage = "Vehicle Model is required.")]
        public string VehicleModel { get; set; }

        [Display(Name = "Contact Number")]
        [Required(ErrorMessage = "Customer Contact is required.")]
        public string CustomerContact { get; set; }

        [Required(ErrorMessage = "Vehicle Year is required.")]
        [Display(Name = "Vehicle Year")]
        public Nullable<int> VehicleYear { get; set; }

        [Required(ErrorMessage = "Vehicle Color is required.")]
        [Display(Name = "Vehicle Color")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Please enter valid color.")]
        public string VehicleColor { get; set; }

        [Required]
        [Display(Name = "Current Location")]
        public string CurrentLocation { get; set; }

        [Required(ErrorMessage = "Driver License no is required.")]
        [Display(Name = "Driver License No")]
        public string DriverLicenseNo { get; set; }

        [Required]
        public string City { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "State")]
        public Nullable<int> StateId { get; set; }

        [Required(ErrorMessage = "The Zip Code field is required.")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        //For Continues
        [Required(ErrorMessage = "Start Date field is required.")]
        [Display(Name = "Start Date")]
        public Nullable<System.DateTime> StartDate { get; set; }

        [Required(ErrorMessage = "End Date field is required.")]
        [Display(Name = "End Date")]
        public Nullable<System.DateTime> EndDate { get; set; }

        [Required(ErrorMessage = "WeekDays field is required.")]
        [Display(Name = "WeekDays")]
        public string WeekDays { get; set; }

        //Added by Bhushan Dod on 08/06/2015 for weekdays name
        public List<WeekDaysModel> WeekDaysList { get; set; }

        public List<string> AssignedWeekDaysList { get; set; }
        public WorkRequestAssignmentModel()
        {
            WeekDaysList = new[]
            {
                new WeekDaysModel { Name = "Monday" },
                new WeekDaysModel { Name = "Tuesday" },
                new WeekDaysModel { Name = "Wednesday" },
                new WeekDaysModel { Name = "Thursday" },
                new WeekDaysModel { Name = "Friday" },
                new WeekDaysModel { Name = "Saturday" },
                new WeekDaysModel { Name = "Sunday" },
            }.ToList();
        }

        [Required(ErrorMessage = "Please select weekdays.")]
        public string WeekDayLst { get; set; }
        [Display(Name = "License Plate Number")]
        public string LicensePlateNo { get; set; }
        public Nullable<bool> FrDisclaimerStatus { get; set; }
        public string AssetPicture { get; set; }

    }

    public class WorkRequestAssignmentModelList
    {
        public long WorkRequestAssignmentID { get; set; }
        public string WorkRequestID { get; set; }
        public Nullable<long> WorkRequestType { get; set; }
        public string WorkRequestTypeName { get; set; }
        public Nullable<long> AssetID { get; set; }
        public long LocationID { get; set; }
        public string LocationName { get; set; }
        public string ProblemDesc { get; set; }
        public Nullable<long> PriorityLevel { get; set; }
        public string PriorityLevelName { get; set; }
        public string WorkRequestImage { get; set; }
        public Nullable<bool> SafetyHazard { get; set; }
        public string ProjectDesc { get; set; }
        public long WorkRequestStatus { get; set; }
        public string WorkRequestStatusName { get; set; }
        public long RequestBy { get; set; }
        public Nullable<long> AssignToUserId { get; set; }
        public string AssignToUserName { get; set; }
        public string ProfileImage { get; set; }
        public Nullable<long> AssignByUserId { get; set; }
        public string Remarks { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreationDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public long WorkRequestProjectType { get; set; }
        public string AssignedWorkOrderImage { get; set; }
        public string WorkRequestProjectTypeName { get; set; }
        public string QRCName { get; set; }
        public string AssetName { get; set; }
        public string WorkStatusDesc { get; set; }
        public string CodeID { get; set; }
        public string WorkOrderCode { get; set; }
        public long WorkOrderCodeID { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string AssignedTime { get; set; }
        public string CreatedByProfile { get; set; }
        public string CreatedByUserName { get; set; }
        public string FacilityRequestType { get; set; }
        public string DisclaimerForm { get; set; }
        public string SurveyForm { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string WeekDays { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string CustomerContact { get; set; }
        public Nullable<int> VehicleYear { get; set; }
        public string VehicleColor { get; set; }
        public string CurrentLocation { get; set; }
        public string DriverLicenseNo { get; set; }
        public Nullable<long> PauseStatus { get; set; }
        public string TotalTime { get; set; }
        public string ConStartTime { get; set; }

    }
}
