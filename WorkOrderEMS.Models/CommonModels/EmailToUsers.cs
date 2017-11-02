using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
   public class EmailToUserModel
    {
        public long LocationId { get; set; }
        public string LocationName { get; set; }
        public string Description { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string City { get; set; }
        public string Mobile { get; set; }
        public string PhoneNo { get; set; }
        public string ZipCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserEmail { get; set; }
        public string AlternareEmail { get; set; }
        public string SubscriptionEmail { get; set; }
        public long UserType { get; set; }
        public string VehicleRegistrationNo { get; set; }
        public string VehicleType { get; set; }
        public string VehiclePermitType { get; set; }
        public string DeclineReason { get; set; }
       //Added By Kartik Bhave 14/08/2015
        public string FrVehicleTagNo { get; set; }
        public string FrVehicleModel { get; set; }
        public string DriverName { get; set; }
        public string LicenseNo { get; set; }
        public string CompanyName { get; set; }
        public string FrPermitDetailsType { get; set; }
        public Nullable<decimal> PermitTypePrice { get; set; }
        public int VehicleID { get; set; }
        public string VehicleIdentificationNumber { get; set; }
        public string LocAddress { get; set; }
        public string VehicleTagNo { get; set; }
        public string VehicleModel { get; set; }
        
    }

   public class EmailToManagerModel
   {
       public long ManagerUserId { get; set; }
       public string ManagerName { get; set; }
       public string ManagerEmail { get; set; }
       public string ProblemDesc { get; set; }
       public long LocationID { get; set; }
       public string LocationName { get; set; }
       public long RequestBy { get; set; }
       public string UserName { get; set; }
       public string DeviceId { get; set; }
       public Nullable<System.DateTime> IdleTimeLimit { get; set; }
       
       
   }
   public class EmailToVendorModel
   {
       public long VendorUserId { get; set; }
       public string VendorName { get; set; }
       public string VendorEmail { get; set; }
       public DateTime PermitDuration { get; set; }
       public long LocationID { get; set; }
       public string LocationName { get; set; }
       public long RequestBy { get; set; }
       public string UserName { get; set; }
       public string DeviceId { get; set; }
       public double RenewalCost { get; set; }
       public string QRCName { get; set; }
       public string QRCCodeId { get; set; }
       public string VehicleIdentification { get; set; }
   }


}
