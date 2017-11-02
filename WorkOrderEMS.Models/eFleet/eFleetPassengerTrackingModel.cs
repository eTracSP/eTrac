using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class eFleetPassengerTrackingModel
    {
        public long RouteID { get; set; }
        public bool ServiceType { get; set; }
        [Required(ErrorMessage = "Start date is required")]
        public System.DateTime StartDate { get; set; }
        [Required(ErrorMessage = "End date is required")]
        public System.DateTime EndDate { get; set; }
        public Nullable<long> LocationID { get; set; }
        [Required(ErrorMessage = "PickUp point is required")]
        public string PickUpPoint { get; set; }
        public string PickupList { get; set; }
        public string DropList { get; set; }
        [Required(ErrorMessage = "Drop point is required")]
        public string DropPoint { get; set; }
        public long CreatedBy { get; set; }
        public string RouteName { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
