using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkOrderEMS.Models
{
    public class PermissionDetailsModel
    {

        public long PermissionDetailId { get; set; }
        public long PermissionId { get; set; }
        public long UserId { get; set; }
        [Required(ErrorMessage = "Please Select Location")]
        public long LocationId { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }

        public string PermissionName { get; set; }
        public string UserIds { get; set; }
        public long UserIdToSave { get; set; }
        public long UserType { get; set; }
        public string FullName { get; set; }
        public string LocationName { get; set; }

        public List<PermissionDetailsModel> GetPermission { get; set; }
        public List<PermissionDetailsModel> GetAssignedPermission { get; set; }
    }

    public class IsMapped
    {
        public bool IsMappedLocation { get; set; }
        public long userTypeRes { get; set; }
    }
}
