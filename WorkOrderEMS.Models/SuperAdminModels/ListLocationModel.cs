using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class ListLocationModel
    {
        public long? LocationId { get; set; }
        public string LocationName { get; set; }
        public string Description { get; set; }
        [Display(Name = "Location Address")]
        public string Address { get; set; }
        [Display(Name = "Location Code")]
        public string LocationCode { get; set; }
        public string City { get; set; }
        public Nullable<int> StateId { get; set; }
        public Nullable<int> CountryId { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Mobile { get; set; }
        public string PhoneNo { get; set; }
        public string ZipCode { get; set; }
        public Nullable<long> QRCID { get; set; }
        public string LocationAdministrator { get; set; }
        public string LocationManager { get; set; }
        public string LocationClient { get; set; }
        public string LocationEmployee { get; set; }
    }
}
