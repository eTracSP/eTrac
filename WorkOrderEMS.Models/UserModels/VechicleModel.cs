using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class VehicleModel
    {
        public long VehicleID { get; set; }
        public bool IsCheckingOut { get; set; }
        public bool IsCheckingIn { get; set; }

        [Display(Name = "Current Mileage")]
        public long CurrentMileage { get; set; }
        public bool AllItemsLocated { get; set; }
        public string ItemsMissing { get; set; }
        [Display(Name = "Is Damage")]
        public bool IsDamage { get; set; }

        [Display(Name = "Image for Damage")]
        public string DamageToReportImage { get; set; }
        [Display(Name = "Remarks for Damage")]
        public string DamageToReportRemarks { get; set; }
        [Display(Name = "Fuel Level")]
        public long FuelLevel { get; set; }
        //public List<GlobalCodeModel> FuelLevelList { get; set; }
        public long MileageAtVehicleFueling { get; set; }
        [Display(Name = "Registration No")]
        public string RegistrationNo { get; set; }
        public long Breaks { get; set; }

        public long Tires { get; set; }
        //public List<GlobalCodeModel> TiresList { get; set; }

        public long Window { get; set; }
        //public List<GlobalCodeModel> WindowList { get; set; }

        //public List<GlobalCodeModel> ConditionalStatus { get; set; }

        public long Damage { get; set; }
        public string DamageImageUrl { get; set; }

        [Display(Name = "Exterior Clean")]
        public bool ExteriorClean { get; set; }
        [Display(Name = "Exterior Clean Notes")]
        public string ExteriorCleanNotes { get; set; }
        [Display(Name = "Interior Clean")]
        public bool InteriorClean { get; set; }
        [Display(Name = "Interior Clean Notes")]
        public string InteriorCleanNotes { get; set; }
        [Display(Name = "Vehicle Cleaning")]
        public bool VehicleCleaning { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<long> QRCID { get; set; }


        public Nullable<long> VehicleTypeID { get; set; }
        public string VehicleType { get; set; }
        public string PermitDetailsType { get; set; }
        public string PermitType { get; set; }
        public string Profilepic { get; set; }

        public List<GlobalCodeModel> FuelLevelList { get; set; }
        public List<GlobalCodeModel> FunctionalStatus { get; set; }
        public List<GlobalCodeModel> WorkingStatus { get; set; }
        public List<GlobalCodeModel> ClientType { get; set; }

    }
}
