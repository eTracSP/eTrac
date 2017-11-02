using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Models
{
    public class eFleetMaintenanceModel
    {
        public long MaintenanceID { get; set; }
        public long VehicleID { get; set; }
        public long UserID { get; set; }
        public string LocationName { get; set; }

        [Required(ErrorMessage = "Vehicle Number is required")]
        public string VehicleNumber { get; set; }

        public long LocationID { get; set; }

        [Required(ErrorMessage = "Maintenance Type is required")]
        //[RegularExpression("^[a-zA-Z0-9 .&',-]+$", ErrorMessage = "Special characters are not allowed.")]
        public long MaintenanceType { get; set; }

        public string MaintenanceTypeList { get; set; }
        [Required(ErrorMessage = "Maintenance Date is required")]
        public Nullable<System.DateTime> MaintenanceDate { get; set; }

        public Nullable<long> PmID { get; set; }

        [Required(ErrorMessage = "Issue Details Is required is required")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Special character not allowed")]
        public string ReminderMetricDesc { get; set; }

        [Required(ErrorMessage = "Please Select Scheduled Preventative Maintenance is required")]
        public string ScheduledPM { get; set; }

        [Required(ErrorMessage = "Driver Name is required")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Special character should not be entered")]
        public string DriverName { get; set; }

        [Required(ErrorMessage = "Days Out of Services is required")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        public Nullable<int> DaysOutOfService { get; set; }

        [Required(ErrorMessage = "Part Cost is required")]
        [RegularExpression("([0-9/.]+$)", ErrorMessage = "Please enter valid Number")]
        public Nullable<decimal> PartsCost { get; set; }

        [Required(ErrorMessage = "Labour Cost is required")]
        [RegularExpression("([0-9/.]+$)", ErrorMessage = "Please enter valid Number")]
        public Nullable<decimal> LabourCost { get; set; }

        public Nullable<decimal> TotalCost { get; set; }

        [Required(ErrorMessage = "End of Odo.(Miles) is required")]
        [RegularExpression("([0-9/.]+$)", ErrorMessage = "Please enter valid Number")]
        public string Miles { get; set; }

        [Required(ErrorMessage = "Notes is required")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Special character not allowed")]
        public string Note { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Result Result { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }

    }
    public class eDetailsMaintenance
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<eFleetMaintenanceModel> rows { get; set; }

    }
}
