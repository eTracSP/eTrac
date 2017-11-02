using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class InsuranceModel
    {
        public long InsuranceId { get; set; }
        public string InsuranceCompany { get; set; }
        public string Description { get; set; }
        public List<InsuranceModel> InsuranceList { get; set; }
    }

    public  class InsurancePlanModel
    {
        public long InsurancePlanID { get; set; }
        public long InsuranceId { get; set; }
        public string InsurancePlan { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<InsurancePlanModel> InsurancePlanList { get; set; }
    }
}
