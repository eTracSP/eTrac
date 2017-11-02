using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.ServiceModel
{
    public class ServiceDisclaimerModel
    {
        [DataMember]
        public string ServiceAuthKey { get; set; }
        [DataMember]
        public long UserId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public long DARId { get; set; }
        [DataMember]
        public long LocationId { get; set; }
        [DataMember]
        public string ImageCust { get; set; }
        [DataMember]
        public string ImageModuleNameCust { get; set; }
        [DataMember]
        public string ImageUrlCust { get; set; }
        [DataMember]
        public string ImageEmp { get; set; }
        [DataMember]
        public string ImageModuleNameEmp { get; set; }
        [DataMember]
        public string ImageUrlEmp { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public string EndTime { get; set; }
        //[DataMember]
        //public string TotalTime { get; set; }
        [DataMember]
        public string TimeZoneName { get; set; }
        [DataMember]
        public long TimeZoneOffset { get; set; }
        [DataMember]
        public bool IsTimeZoneinDaylight { get; set; }
        [DataMember]
        public string LocationName { get; set; }
        [DataMember]
        public string FacilityRequestType { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string CustomerContact { get; set; }
        [DataMember]
        public string VehicleColor { get; set; }
        [DataMember]
        public string VehicleMake { get; set; }
        [DataMember]
        public string VehicleModel { get; set; }
        [DataMember]
        public Nullable<int> VehicleYear { get; set; }
        [DataMember]
        public string DriverLicenseNo { get; set; }
        [DataMember]
        public string CurrentLocation { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string ZipCode { get; set; }
        [DataMember]
        public string StateName { get; set; }
        [DataMember]
        public string LicensePlateNo { get; set; }
        [DataMember]
        public string ProjectDesc { get; set; }
        [DataMember]
        public string ActivityDetails { get; set; }
        public bool IsManual { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        [DataMember]
        public long TaskType { get; set; }
        [DataMember]
        public string ResponseMessage { get; set; }
        [DataMember]
        public int Response { get; set; }
        [DataMember]
        public string FacilityRequestName { get; set; }
        public string DisclaimerFormFile { get; set; }
        public string EmpSignatureImage { get; set; }
        public string CustomerSignatureImage { get; set; }
    }
}
