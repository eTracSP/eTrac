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
   public class eFleetVehicleIncidentModel
    {
        public long IncidentID { get; set; }
        public long UserID { get; set; }
        [Required(ErrorMessage = "Vehicle Number is required")]
        [RegularExpression("^[a-zA-Z0-9 .&',-]+$", ErrorMessage = "Special characters are not allowed.")]
        public string VehicleNumber { get; set; }
        public string LocationName { get; set; }

        [Required(ErrorMessage = "Driver Name is required")]
        [RegularExpression("^[a-zA-Z .&',-]+$", ErrorMessage = "Special characters are not allowed.")]
        public string DriverName { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public Nullable<System.DateTime> AccidentDate { get; set; }

        [Required(ErrorMessage = "State is required")]
        public long StateId { get; set; }

        [Required(ErrorMessage = "City is required")]
        [RegularExpression("^[a-zA-Z0-9 .&',-]+$", ErrorMessage = "Special characters are not allowed.")]
        public string City { get; set; }

        public string QRCodeID { get; set; }
        public long VehicleID { get; set; }
        public int CountryId { get; set;}
        public long LocationID { get; set; }

        [Required(ErrorMessage = "Number of Injuries is required")]
        [RegularExpression("^[0-9 ]+$", ErrorMessage = "Special characters and Letters are not allowed.")]
        public string NumberOfInjuries { get; set; }

        [Required(ErrorMessage = "Preventability is required")]
        public bool Preventability { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public string IncidentImage { get; set; }
        public HttpPostedFileBase IncidentImageFile { get; set; }

        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Result Result { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }

    }
    public class eFleetIncidentDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<eFleetVehicleIncidentModel> rows { get; set; }

    }
}
