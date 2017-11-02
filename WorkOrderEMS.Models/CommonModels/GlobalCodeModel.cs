using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class GlobalCodeModel
    {
        public long GlobalCodeId { get; set; }
        public string Category { get; set; }
        public string CodeName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public List<GlobalCodeModel> GlobalCodeList { get; set; }
    }

    public class GlobalCodeModelDDL
    {
        public long GlobalCodeId { get; set; }
        public string CodeName { get; set; }
    }

    public class listForEmployeeDevice
    {
        public long PermissionDetailId { get; set; }
        public string DeviceId { get; set; }

        public long ManagerUserId { get; set; }
        public string ManagerName { get; set; }
        public string ManagerEmail { get; set; }   
 
        public long LocationID { get; set; }
        public string LocationName { get; set; }

        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public long UserId { get; set; }
    }

}
