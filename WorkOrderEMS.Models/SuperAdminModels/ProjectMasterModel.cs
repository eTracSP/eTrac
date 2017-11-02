using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WorkOrderEMS.Models
{
    public class ProjectMasterModel
    {
        public long ProjectID { get; set; }
        [Required]
        [DisplayName("Project Name")]
        public string Location { get; set; }
        [Required]
        [DisplayName("Location")]
        public long LocationID { get; set; }
        public string Description { get; set; }
        public string ProjectLogoName { get; set; }
        public bool IsLogoForQRCode { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        [Required]
        [DisplayName("Project Category")]
        public long ProjectCategory { get; set; }
        public string ProjectLogoURl { get; set; }
        public Nullable<long> QRCID { get; set; }
    }

    public class ProjectMasterListModel
    {
        public long ProjectID { get; set; }
        public string Location { get; set; }
        public long LocationID { get; set; }
        public string LocationName { get; set; }
        public string Description { get; set; }
        public long ProjectCategory { get; set; }
        public string ProjectCategoryName { get; set; }
        public string ProjectLogoName { get; set; }
        public bool IsLogoForQRCode { get; set; }
        public string ProjectServicesID { get; set; }
        public string ProjectServiceName { get; set; }
        public Nullable<long> QRCID { get; set; }
    }
}
