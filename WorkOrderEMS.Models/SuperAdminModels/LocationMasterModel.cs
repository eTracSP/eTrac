using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace WorkOrderEMS.Models
{
    public class LocationMasterModel
    {

        public long LocationId { get; set; }
        //public long Manager { get; set; }


        public UserModel ManagerModel { get; set; }
        public UserModel ClientModel { get; set; }

        public List<UserModel> EmployeeListModel { get; set; }

        public string ServicesID { get; set; }

        [Required]
        [DisplayName("Location Name")]
        public string LocationName { get; set; }
        public string LocationCode { get; set; }
        public string Description { get; set; }

        [Required]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Special characters are not  allowed.")]
        //[RegularExpression(@"^(?![\W_]+$)(?!\d+$)[a-zA-Z0-9 .&',_-]+$", ErrorMessage = "Special characters not allowed.")]
        [Display(Name = "Address")]
        public string Address1 { get; set; }

        [Required]
        [DisplayName("Location Code")]
        public string Address2 { get; set; }


        [Required]
        public string City { get; set; }
        [DisplayName("Location Type")]
        [Required]
        public long LocationType { get; set; }

        public long? LocationSubType { get; set; }

        [DisplayName("Location Sub Type Description")]
        public string LocationSubTypeDesc { get; set; }

        [Required]
        [DisplayName("State")]
        public Nullable<int> StateId { get; set; }


        [Required]
        [DisplayName("Country")]
        public Nullable<int> CountryId { get; set; }
        [Required]
        public string Mobile { get; set; }
        [DisplayName("Phone no")]
        public string PhoneNo { get; set; }
        [Required]
        [DisplayName("Zip Code")]
        public string ZipCode { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<long> QRCID { get; set; }
        public string EmployeeList { get; set; }

        public string Services { get; set; }
        public bool IsVerifiedByManager { get; set; }
        public bool IsVerifiedByClient { get; set; }
    }

    public class LocationListModel
    {
        public long LocationId { get; set; }
        public string Location { get; set; }
        public string LocationName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        [Display(Name = "Location Address")]
        public string Address1 { get; set; }
        [Display(Name = "Location Code")]
        public string Address2 { get; set; }
        public string City { get; set; }

        [Display(Name = "State")]
        public Nullable<int> StateId { get; set; }

        [Display(Name = "Country")]
        public Nullable<int> CountryId { get; set; }
        public string Mobile { get; set; }
        public string PhoneNo { get; set; }
        public string ZipCode { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public Nullable<long> QRCID { get; set; }
    }
    public class UserLocations
    {
        public Nullable<long> LocationID { get; set; }
        public string LocationName { get; set; }
        public string LocationCode { get; set; }
    }
}
