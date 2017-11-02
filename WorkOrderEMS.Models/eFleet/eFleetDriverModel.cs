using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Models
{
    public class eFleetDriverModel
    {
        public long DriverID { get; set; }

        // public string QRCodeID { get; set; }
        [Required(ErrorMessage = "Location is required")]
        public long LocationID { get; set; }
        public string LocationName { get; set; }
        public long UserID { get; set; }

        [Required(ErrorMessage = "Driver Name is required")]
        public long EmployeeName { get; set; }
        public string EmployeeNameList { get; set; }

        // [Required(ErrorMessage = "Picture of Ve is required")]
        public string DriverImage { get; set; }
        public HttpPostedFileBase DriverImageFile { get; set; }

        [Required(ErrorMessage = "Driver License Number is required")]
        [RegularExpression("^[a-zA-Z0-9 ',-]+$", ErrorMessage = "Special characters are not allowed.")]
        public string DriverLicenseNo { get; set; }

        public int StateId { get; set; }

        [Required(ErrorMessage = "CDLType is required")]
        public string CDLType { get; set; }
        //public string CDLType { get; set; }

        [Required(ErrorMessage = "CDL Expiration is required")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> CDLExpiration { get; set; }

        [Required(ErrorMessage = "Medicle Card Expiration is required")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> MedicleCardExpiration { get; set; }

        [Required(ErrorMessage = "MVR Expiration is required")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> MVRExpiration { get; set; }
        public string DummyField { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Result Result { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }       
        public class eDriverDetails
        {
            public int total { get; set; }
            public int pageindex { get; set; }
            public int records { get; set; }
            public List<eFleetDriverModel> rows { get; set; }
        }

    }

}


