using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.SuperAdminModels
{
    public class MasterLocationModel
    {
        public long LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationCode { get; set; }
        public string Description { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public Nullable<int> StateId { get; set; }
        public Nullable<int> CountryId { get; set; }
        public string Mobile { get; set; }
        public string PhoneNo { get; set; }
        public string ZipCode { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<long> QRCID { get; set; }
        public long LocationType { get; set; }
        public Nullable<long> LocationSubType { get; set; }
        public bool IsVerifiedByManager { get; set; }
        public bool IsVerifiedByClient { get; set; }
        public string LocationSubTypeDesc { get; set; }

        
    }
}
