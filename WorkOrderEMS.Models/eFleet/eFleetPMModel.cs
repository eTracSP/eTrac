using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Models
{
    public class eFleetPMModel
    {
        public long PmID { get; set; }
        public long ID { get; set; }
        public long VehicleID { get; set; }
        public long UserId { get; set; }
        public string LocationName { get; set; }

        [Required(ErrorMessage = "Vehicle Number is required")]
        public string VehicleNumber { get; set; }

        [Required(ErrorMessage = "Meter is required")]
        public string ListMeter { get; set; }

        public long? Meter { get; set; }

        [Required(ErrorMessage = "Miles Value is required")]
        public string MilesValue { get; set; }

        [Required(ErrorMessage = "Hours Value is required")]
        public string ListHoursValue { get; set; }

        [Required(ErrorMessage = "Hours Value is required")]
        [RegularExpression(@"^[0-9 .&']+$", ErrorMessage = "Special characters and Letters are not allowed.")]
        public long? HoursValue { get; set; }
        

        [Required(ErrorMessage = "Category is required")]
        public long? Category { get; set; }

        public string PMCategoryList{ get; set; }


        public string ListReminderMetric { get; set; }

        [Required(ErrorMessage = "Reminder Metric is required")]
        public long? ReminderMetric { get; set; }

        [Required(ErrorMessage = "Sub Category is required")]
        public string SubCategory { get; set; }

        [Required(ErrorMessage = "Reminder Metric Description is required")]
        public string ReminderMetricDesc { get; set; }
        public long LocationID { get; set; }
        public string QRCodeID { get; set; }
        public Result Result { get; set; }
        public string MeterValue { get; set; }

        public string OtherComment { get; set; }
        [Required(ErrorMessage = "Comment for other miles value is required")]
        [RegularExpression(@"^[0-9 .&']+$", ErrorMessage = "Special characters and Letters are not allowed.")]
        public Nullable<int> OtherMilesComment { get; set; }

        [Required(ErrorMessage = "Service Due Date is required")]
        public Nullable<System.DateTime> ServiceDueDate { get; set; }
        public string QRCID { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }

    }
    public class eDetailsPM
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<eFleetPMModel> rows { get; set; }

    }

    public class PendingPM
    {
        public long PmID { get; set; }
        public string ReminderMetricDesc { get; set; }
        public long VehicleID
        { get; set;}
        public Nullable<System.DateTime> ServiceDueDate { get; set; }
    }
}
