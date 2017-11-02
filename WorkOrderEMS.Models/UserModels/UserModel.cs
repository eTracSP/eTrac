using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web;


namespace WorkOrderEMS.Models
{

    public class UserModel : IdentityUser
    {

        public long UserId { get; set; }
        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }
        public bool UpdateMode { get; set; }

        [Required]
        [DisplayName("Confirm Password")]
        [CompareAttribute("Password", ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DisplayName("Email ID")]
        [EmailAddress]
        // [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "Please enter correct email address")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Username requiured.")]
        [DisplayName("User Name")]
        public string AlternateEmail { get; set; }

        public string SubscriptionEmail { get; set; }
        [Display(Name = "Permission Level")]
        [DataMember]
        public long UserType { get; set; }
        [Required]
        [DisplayName("Location Name")]
        public Nullable<long> ProjectID { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Company Name")]
        public string CompanyName { get; set; }

        public string Logo { get; set; }
        public string LogoFile { get; set; }

        public Nullable<long> Gender { get; set; }

        [DisplayName("Date of Hire")]
        public string DOB { get; set; }

        [DisplayName("Profile Image")]
        public string myProfileImage { get; set; }

        [Required, FileExtensions(Extensions = "jpg,jpeg,png",
             ErrorMessage = "Specify a Imgae file.")]
        public HttpPostedFileBase ProfileImage { get; set; }


        public string ProfileImageFile { get; set; }

        [DisplayName("Login Active")]
        public bool IsLoginActive { get; set; }

        [DisplayName("Email Verify")]
        public bool IsEmailVerify { get; set; }

        [DisplayName("Select Time Zone")]
        public Nullable<long> TimeZoneId { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        [DisplayName("Select Vendor")]
        public Nullable<long> VendorID { get; set; }
        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }
        [Required]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Hiring Date")]
        public Nullable<System.DateTime> HiringDate { get; set; }
        [Required]
        [DisplayName("Job Title")]
        public string JobTitle { get; set; }

        [Required]
        [DisplayName("Other Job Title")]
        public string JobTitleOther { get; set; }


        [DisplayName("Employee Category")]
        public Nullable<long> EmployeeCategory { get; set; }
        [DisplayName("QRC ID")]
        public Nullable<long> QRCID { get; set; }
        public AddressModel Address { get; set; }

        [DisplayName("Employee ID")]
        public string EmployeeID { get; set; }


        [Required]
        [DisplayName("Select Location")]
        public long Location { get; set; }


        [Required]
        [DisplayName("Select Administrator")]
        public long Administrator { get; set; }

        [DisplayName("Select Manager")]
        public bool IsExistingManager { get; set; }
        [DisplayName("Select Client")]
        public long ExistClientID { get; set; }
        [DisplayName("Select Manager")]
        public long ExistManagerID { get; set; }
        public Nullable<System.DateTime> IdleTimeLimit { get; set; }
        //public long Manager { get; set; }

        public string ServiceAuthKey { get; set; }
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public long Response { get; set; }
        public string SelectedServicesID { get; set; }
        public string SelectedLocationName { get; set; }
        public Nullable<long> SelectedLocationId { get; set; }
        public long SelectedUserType { get; set; }

    }
    public class UserModelList
    {
        public Nullable<long> UserId { get; set; }
        public string UserEmail { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<System.DateTime> HiringDate { get; set; }
        public string CodeName { get; set; }
        public string EmployeeProfile { get; set; }
        public string UserType { get; set; }
        public string ProfileImage { get; set; }
        public Nullable<long> QRCID { get; set; }
        public long RoleId { get; set; }
        public long Location { get; set; }

    }


    public class AdminUserForDrop
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string UserEmail { get; set; }
    }

}
