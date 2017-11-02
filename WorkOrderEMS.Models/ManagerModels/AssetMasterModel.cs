using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class AssetMasterModel
    {
        public long AssetMasterID { get; set; }
        [DisplayName("Asset Name")]
        public string AssetName { get; set; }
        [DisplayName("Asset ID")]
        public string AssetID { get; set; }
        [DisplayName("Asset Class")]
        public Nullable<long> ClassID { get; set; }
        [DisplayName("Model Name")]
        public string ModelName { get; set; }
        [DisplayName("Serial No")]
        public string SerialNo { get; set; }
        [DisplayName("Work Area")]
        public Nullable<long> WorkAreaID { get; set; }
        [DisplayName("Asset Image")]
        public string AssetPhoto { get; set; }
        public Nullable<long> ProjectID { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
    }
}
