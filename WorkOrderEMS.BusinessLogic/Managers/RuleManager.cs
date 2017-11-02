using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public class RuleManager : IRuleManager
    {
        RuleRepository objRuleRepository;
        UserRepository objUserRepository;
        public ServiceResponseModel<List<RuleMasterModelList>> GetListOfAllRule(RuleMasterModelList objRuleMasterModelList)
        {
            var objRuleList = new ServiceResponseModel<List<RuleMasterModelList>>();
            try
            {
                 objRuleRepository = new RuleRepository();
                 objUserRepository = new UserRepository();            

                var authuser = objUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == objRuleMasterModelList.ServiceAuthKey && x.IsDeleted == false);
                if (authuser != null && authuser.UserId > 0)
                {
                    var result = objRuleRepository.GetAll().Where(r => r.ProjectID == objRuleMasterModelList.LocationId 
                                                                       || r.ProjectID == 0
                                                                       && r.IsActive == true 
                                                                       && r.IsDeleted == false)
                                                           .Select(r => new RuleMasterModelList()
                                                            {
                                                                 RuleID = r.RuleID,
                                                                 RuleName = r.RuleName            
                                                            }).ToList();
                    if (result != null && result.Count > 0)
                    {
                        objRuleList.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        objRuleList.Data = result;
                        objRuleList.Message = CommonMessage.Successful();
                    }
                    else
                    {
                        objRuleList.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                        objRuleList.Message = CommonMessage.NoRecordMessage();

                    }
                }
                else
                {
                    objRuleList.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    objRuleList.Message = CommonMessage.InvalidUser();
                }
                return objRuleList;
            }
            catch(Exception)
            {
                throw;
            }
          
        }

        /// <summary>GetGlobalCodeDetailById
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>June-03-2015</CreatedOn>
        /// </summary>
        /// <param name="RuleId"></param>
        /// <returns></returns>
        public string GetRuleNameById(long RuleId)
        {
            try
            {
                objRuleRepository = new RuleRepository();
                return objRuleRepository.GetSingleOrDefault(g => g.RuleID == RuleId && g.IsDeleted == false && g.IsActive == true).RuleName;
            }
            catch (Exception)
            { throw; }
        }
    }
}
