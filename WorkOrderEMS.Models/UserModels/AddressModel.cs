using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WorkOrderEMS.Models
{
    public class AddressModel
    {
        public long AddressMasterId { get; set; }
        public Nullable<long> UserId { get; set; }

        [Required]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Special characters are not  allowed.")]
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }
        [Required]
        public string City { get; set; }

        [Required]
        [DisplayName("State")]
        public Nullable<int> StateId { get; set; }

        [Required]
        [DisplayName("Country")]
        public Nullable<int> CountryId { get; set; }

        public long? VendorID { get; set; }
        [Required]
        [Display(Name = "Mobile No.")]
        public string Mobile { get; set; }


        [Display(Name = "Phone No.")]
        public string Phone { get; set; }

        [Display(Name = "Fax No.")]
        public string Fax { get; set; }


        [Required, MaxLength(5)]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        public bool IsCurrentAddress { get; set; }
        public bool IsPermanent { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    }
}
