using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data
{
    public class InsuranceRepository : BaseRepository<InsuranceMaster>, IInsurance
    {
        workorderEMSEntities ObjWorkOrderEMSEntities = new workorderEMSEntities();

        /// <summary>GetStateByInsuranceCompanyId
        /// CreatedBy   :   Nagendra Upwanshi
        /// CreatedOn   :   Oct-14-2014
        /// </summary>
        /// <param name="InsuranceCompanyId"></param>
        /// <returns></returns>
        public List<InsurancePlanModel> GetStateByInsuranceCompanyId(long InsuranceCompanyId)
        {
            List<InsurancePlanModel> lstPlan = new List<InsurancePlanModel>();
            try
            {
                lstPlan = ObjWorkOrderEMSEntities.InsurancePlanMasters.Where(p => p.InsuranceId == InsuranceCompanyId && p.IsDeleted == false                    //&& p.IsActive == true
                    ).Select(pl => new InsurancePlanModel()
                {
                    Description = pl.Description,
                    InsurancePlan = pl.InsurancePlan,
                    InsuranceId = pl.InsuranceId,
                    InsurancePlanID = pl.InsurancePlanID,
                    IsActive = pl.IsActive,
                }).ToList();
                return lstPlan;
            }
            catch (Exception)
            {
                throw ;
            }
        }

    }
}
