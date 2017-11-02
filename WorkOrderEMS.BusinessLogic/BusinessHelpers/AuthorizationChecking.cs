using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using WorkOrderEMS.Data.EntityModel;

namespace WorkOrderEMS.BusinessLogic.BusinessHelpers
{
    public static class AuthorizationChecking
    {
        public static bool Authorize(HttpActionContext actionContext)
        {
            bool validFlag = false;
            //  workorderEMSEntities db = new workorderEMSEntities();
            try
            {
                if (actionContext.Request.Headers.Contains("ServiceAuthKey") && actionContext.Request.Headers.GetValues("ServiceAuthKey") != null)
                {
                    // get value from header
                    string encodedString = Convert.ToString(actionContext.Request.Headers.GetValues("ServiceAuthKey").FirstOrDefault());

                    if (!string.IsNullOrEmpty(encodedString))
                    {
                        // By passing this parameter 
                        var myDateTime = DateTime.UtcNow;
                        using (var dbCtx = new workorderEMSEntities())
                        {
                            var registerModel = dbCtx.UserRegistrations.Where(x => x.ServiceAuthKey == encodedString
                                                                               && x.IsDeleted == false
                                                                               && myDateTime < x.TokenExpiresOn).FirstOrDefault();

                            if (registerModel != null)
                            {
                                registerModel.TokenExpiresOn = DateTime.UtcNow.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["TokenExpiry"]));
                                //3. Mark entity as modified
                                dbCtx.Entry(registerModel).State = System.Data.Entity.EntityState.Modified;
                                //4. call SaveChanges
                                dbCtx.SaveChanges();
                                validFlag = true;
                            }
                            else
                            {
                                validFlag = false;
                            }
                        }
                    }
                    return validFlag;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
