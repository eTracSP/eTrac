using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    [DataContract]
    public class ServiceUserModel
    {
        [DataMember]
        public long UserId { get; set; }
        public string Password { get; set; }
        [DataMember]
        public string UserEmail { get; set; }
        public string AlternateEmail { get; set; }
        public string SubscriptionEmail { get; set; }
        [DataMember]
        public long UserType { get; set; }
        public Nullable<long> ProjectID { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public Nullable<long> Gender { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string ProfileImage { get; set; }
        public bool IsLoginActive { get; set; }
        public bool IsEmailVerify { get; set; }
        public Nullable<long> TimeZoneId { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<long> VendorID { get; set; }
        public string BloodGroup { get; set; }
        public Nullable<System.DateTime> HiringDate { get; set; }
        public Nullable<long> EmployeeCategory { get; set; }
           public Nullable<long> QRCID { get; set; }
        [DataMember]
        public string EmployeeID { get; set; }
        public string JobTitle { get; set; }
        public string DeviceId { get; set; }
        public Nullable<long> DeviceType { get; set; }
        [DataMember]
        public string ServiceAuthKey { get; set; }
        [DataMember]
        public string ProfileImageFile { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public long Response { get; set; }
        [DataMember]
        public long LocationId { get; set; }
        [DataMember]
        public string LocationName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Address1 { get; set; }
        [DataMember]
        public string Address2 { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public Nullable<int> StateId { get; set; }
        [DataMember]
        public Nullable<int> CountryId { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public string PhoneNo { get; set; }
        [DataMember]
        public string ZipCode { get; set; }
    }
}
