//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorkOrderEMS.Data.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserRegistration
    {
        public UserRegistration()
        {
            this.AddressMasters = new HashSet<AddressMaster>();
            this.AdminEmployeeMappings = new HashSet<AdminEmployeeMapping>();
            this.eFleetDrivers = new HashSet<eFleetDriver>();
            this.eFleetFuelings = new HashSet<eFleetFueling>();
            this.eFleetMaintenances = new HashSet<eFleetMaintenance>();
            this.eFleetMeters = new HashSet<eFleetMeter>();
            this.eFleetPassengerTrackingCounts = new HashSet<eFleetPassengerTrackingCount>();
            this.eFleetPassengerTrackingRoutes = new HashSet<eFleetPassengerTrackingRoute>();
            this.eFleetPreventativeMaintenances = new HashSet<eFleetPreventativeMaintenance>();
            this.eFleetPreventativeMaintenances1 = new HashSet<eFleetPreventativeMaintenance>();
            this.eFleetVehicleIncidents = new HashSet<eFleetVehicleIncident>();
            this.eFleetVehicleMasterLogs = new HashSet<eFleetVehicleMasterLog>();
            this.HoursOfServices = new HashSet<HoursOfService>();
            this.LocationClientMappings = new HashSet<LocationClientMapping>();
            this.LoginLogs = new HashSet<LoginLog>();
            this.LoginLogs1 = new HashSet<LoginLog>();
            this.ManagerLocationMappings = new HashSet<ManagerLocationMapping>();
            this.QRCMasterLogs = new HashSet<QRCMasterLog>();
            this.QRCScanLogs = new HashSet<QRCScanLog>();
        }
    
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
        public string EmployeeID { get; set; }
        public string JobTitle { get; set; }
        public string DeviceId { get; set; }
        public Nullable<long> DeviceType { get; set; }
        public string ServiceAuthKey { get; set; }
        public Nullable<System.DateTime> IdleTimeLimit { get; set; }
        public Nullable<bool> IsOnline { get; set; }
        public string JobTitleOther { get; set; }
        public string TimeZoneName { get; set; }
        public string TimeZoneOffsetName { get; set; }
        public Nullable<System.DateTime> TokenExpiresOn { get; set; }
    
        public virtual ICollection<AddressMaster> AddressMasters { get; set; }
        public virtual ICollection<AdminEmployeeMapping> AdminEmployeeMappings { get; set; }
        public virtual ICollection<eFleetDriver> eFleetDrivers { get; set; }
        public virtual ICollection<eFleetFueling> eFleetFuelings { get; set; }
        public virtual ICollection<eFleetMaintenance> eFleetMaintenances { get; set; }
        public virtual ICollection<eFleetMeter> eFleetMeters { get; set; }
        public virtual ICollection<eFleetPassengerTrackingCount> eFleetPassengerTrackingCounts { get; set; }
        public virtual ICollection<eFleetPassengerTrackingRoute> eFleetPassengerTrackingRoutes { get; set; }
        public virtual ICollection<eFleetPreventativeMaintenance> eFleetPreventativeMaintenances { get; set; }
        public virtual ICollection<eFleetPreventativeMaintenance> eFleetPreventativeMaintenances1 { get; set; }
        public virtual ICollection<eFleetVehicleIncident> eFleetVehicleIncidents { get; set; }
        public virtual ICollection<eFleetVehicleMasterLog> eFleetVehicleMasterLogs { get; set; }
        public virtual GlobalCode GlobalCode { get; set; }
        public virtual GlobalCode GlobalCode1 { get; set; }
        public virtual GlobalCode GlobalCode2 { get; set; }
        public virtual GlobalCode GlobalCode3 { get; set; }
        public virtual ICollection<HoursOfService> HoursOfServices { get; set; }
        public virtual ICollection<LocationClientMapping> LocationClientMappings { get; set; }
        public virtual ICollection<LoginLog> LoginLogs { get; set; }
        public virtual ICollection<LoginLog> LoginLogs1 { get; set; }
        public virtual ICollection<ManagerLocationMapping> ManagerLocationMappings { get; set; }
        public virtual QRCMaster QRCMaster { get; set; }
        public virtual ICollection<QRCMasterLog> QRCMasterLogs { get; set; }
        public virtual ICollection<QRCScanLog> QRCScanLogs { get; set; }
    }
}
