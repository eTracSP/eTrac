using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.SuperAdminModels;

namespace WorkOrderEMS.Data.DataRepository
{
    public class GlobalAdminRepository
    {
        workorderEMSEntities objworkorderEMSEntities;

        public List<GlobalUserModel> GetTotalCountOfUsers(string UserType, long LocationID,long UserID)
        {
            objworkorderEMSEntities = new workorderEMSEntities();
            List<GlobalUserModel> obj_GlobalUserModel = objworkorderEMSEntities.Proc_GetTotalUser(UserType, LocationID,UserID).Select(x => new GlobalUserModel()
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                UserEmail = x.UserEmail,
                UserID = x.UserID,
                UserName = x.UserName

            }).ToList();
            return obj_GlobalUserModel;
        }
        /// <summary>
        /// GET GLOBAL ADMIN IN THE APPLICATION
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreateDate>2015-16-03</CreateDate>
        /// <returns></returns>
        public List<GlobalUserModel> GetApplicationGlobalAdmin()
        {
            try
            {
                objworkorderEMSEntities = new workorderEMSEntities();
                return objworkorderEMSEntities.UserRegistrations.Join(objworkorderEMSEntities.GlobalCodes, u => u.UserType, x => x.GlobalCodeId,
                  (u, x) => new { u, x })
                  .Where(z => z.x.CodeName == "Global Admin" && z.u.IsDeleted == false && z.u.IsLoginActive == true)
                  .Select(z => new GlobalUserModel()
                  {

                      UserID = z.u.UserId,
                      UserName = z.u.AlternateEmail,
                      FirstName = z.u.FirstName,
                      LastName = z.u.LastName,


                  }).ToList();

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
