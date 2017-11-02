using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.ServiceModel
{
    public class ServiceBaseModel
    {
        public string ServiceAuthKey { get; set; }
        public long UserId { get; set; }
        public long LocationID { get; set; }
    }
    public class eFleetIncidentModel : ServiceBaseModel
    {
        public long VehicleID { get; set; }
        public string QRCodeID { get; set; }
        public string VehicleNumber { get; set; }        
        public DateTime AccidentDate { get; set; }
        public long StateId { get; set; }
        public string City { get; set; }
        public string NumberOfInjuries { get; set; }
        public bool Preventability { get; set; }
        public string Description { get; set; }
        public string IncidentImage { get; set; }
        //public string ActivityDetails { get; set; }
        //public long TaskType { get; set; }
        public string LocationName { get; set; }
    }
}
