using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data.Interfaces
{
    interface IInsurance
    {
        /// <summary>GetStateByInsuranceCompanyId
        /// CreatedBy   :   Nagendra Upwanshi
        /// CreatedOn   :   Oct-14-2014
        /// </summary>
        /// <param name="InsuranceCompanyId"></param>
        /// <returns></returns>
        List<InsurancePlanModel> GetStateByInsuranceCompanyId(long InsuranceCompanyId);
    }
}
