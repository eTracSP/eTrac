using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.UserModels
{
    public class EmployeeModel
    {
        public long UserId { get; set; }
        public string Password { get; set; }
        public string UserEmail { get; set; }
        public string AlternateEmail { get; set; }
        public string SubscriptionEmail { get; set; }
        public long UserType { get; set; }
        public Nullable<long> ProjectID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<long> Gender { get; set; }
        public string DOB { get; set; }
        public string ProfileImage { get; set; }
        public bool IsLoginActive { get; set; }
        public bool IsEmailVerify { get; set; }
        public Nullable<long> TimeZoneId { get; set; }
        public Nullable<long> VendorID { get; set; }
        public string BloodGroup { get; set; }
        public Nullable<System.DateTime> HiringDate { get; set; }
        public Nullable<long> EmployeeCategory { get; set; }
        public Nullable<long> QRCID { get; set; }
        public string EmployeeID { get; set; }
        public string JobTitle { get; set; }
        public string DeviceId { get; set; }
        public Nullable<long> DeviceType { get; set; }
        public string ServiceAuthKey { get; set; }
        public Nullable<long> EmployeeLocationMappingId { get; set; }
        public Nullable<long> EmployeeUserId { get; set; }
        public Nullable<long> LocationId { get; set; }
        public long AddressMasterId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public Nullable<int> StateId { get; set; }
        public Nullable<int> CountryId { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string ZipCode { get; set; }
        public bool IsCurrentAddress { get; set; }
        public bool IsPermanent { get; set; }
    }
}
