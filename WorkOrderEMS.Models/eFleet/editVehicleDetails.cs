using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Models.eFleet
{
    public class editVehicleDetails
    {
        public long VehicleID { get; set; }

        [Required(ErrorMessage = "Vehicle Number is required")]
        [RegularExpression("^[a-zA-Z0-9 .&',-]+$", ErrorMessage = "Special characters are not allowed.")]
        public string VehicleNumber { get; set; }

        public string QRCodeID { get; set; }
        public long LocationID { get; set; }

        [Required(ErrorMessage = "VIN is required")]
        [RegularExpression("^[a-zA-Z0-9 .&',-]+$", ErrorMessage = "Special characters are not allowed.")]
        public string VehicleIdentificationNo { get; set; }

        [Required(ErrorMessage = "Make is required")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Special characters and Number are not allowed.")]
        public string Make { get; set; }

        [Required(ErrorMessage = "Model is required")]
        [RegularExpression("^[a-zA-Z0-9 .&',-]+$", ErrorMessage = "Special characters are not allowed.")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Year is required")]
        public string Year { get; set; }

        [Required(ErrorMessage = "Attachment of Registration is required")]
        public string AttachmentOfRegistration { get; set; }
        public HttpPostedFileBase AttachmentOfRegistrationFile { get; set; }

        [Required(ErrorMessage = "Attachment of Insurance is required")]
        public string AttachmentOfInsurance { get; set; }
        public HttpPostedFileBase AttachmentOfInsuranceFile { get; set; }

        [Required(ErrorMessage = "Expiration date is required")]
        public Nullable<System.DateTime> ExpirationDate { get; set; }

        [Required(ErrorMessage = "Picture of Vehicle is required")]
        public string VehicleImage { get; set; }
        public HttpPostedFileBase VehicleImageFile { get; set; }

        [Required(ErrorMessage = "Gross vehicle Weight Rate is required")]
        public string GVWR { get; set; }

        [Required(ErrorMessage = "Storage Address is required")]
        [RegularExpression(@"^(?![\W_]+$)(?!\d+$)[a-zA-Z0-9 .&',_-]+$", ErrorMessage = "Special characters are not allowed.")]
        public string StorageAddress { get; set; }

        [Required(ErrorMessage = "Zip Code is required")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Special characters and Letters are not allowed.")]
        public string ZipCode { get; set; }

        public string DummyField { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Result Result { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
   
}
