using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;


namespace WorkOrderEMS.Models
{
   public class EmailLogModel
    {
        public long EmailLogId { get; set; }
        public Nullable<long> SentBy { get; set; }
        public string SentByUser { get; set; }
        public string SentTo { get; set; }
        public string SentEmail { get; set; }
        public string Subject { get; set; }
        public Nullable<long> LocationId { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public String ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public bool isForgot { get; set; }
    }
}
