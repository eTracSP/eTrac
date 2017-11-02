using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data
{
    public class RuleRepository:BaseRepository<RuleMaster>
    {
        workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();
        public List<RuleMasterModelList> GetAllRules(long? projectID, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter paramTotalRecords)
        {
            List<RuleMasterModelList> lstRules = new List<RuleMasterModelList>();
            try
            {
                lstRules = _workorderEMSEntities.SP_GetAllRule(projectID, operationName, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, paramTotalRecords).Select(r =>
                    new RuleMasterModelList()
                    {
                        RuleID = r.RuleID,
                        RuleName = r.RuleName,
                        Description = r.Description,
                        VoilationCharges = r.VoilationCharges,
                        IsActive = r.IsActive
                    }).ToList();
                return lstRules;
            }
            catch (Exception ex)
            {                
                throw ex;
            }

        }

        public RuleMasterModel GetRuleById (long ? locationId , long ? ruleId)
        {
              RuleMasterModel objRuleMasterModel = null;
            try
            {
                objRuleMasterModel = _workorderEMSEntities.SP_GetRuleBYId(locationId, ruleId).Select(r =>
                    new RuleMasterModel()
                    {
                        RuleID = r.RuleID,
                        RuleName = r.RuleName,
                        Description = r.Description,
                        VoilationCharges = r.VoilationCharges,
                        IsActive = r.IsActive
                    }).FirstOrDefault();

                return objRuleMasterModel;

            }
            catch (Exception )
            {
                
                throw;
            }
        }


    }
}
