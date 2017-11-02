using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models.ManagerModels;

namespace WorkOrderEMS.Data.DataRepository
{
    public class ManagerRepositroy
    {
        workorderEMSEntities objworkorderEMSEntities;
        public List<ManagerModel> GetTotalCountOfUsers(string UserType, long LocationID, long UserID)
        {
            objworkorderEMSEntities = new workorderEMSEntities();
            List<ManagerModel> obj_GlobalUserModel = objworkorderEMSEntities.Proc_GetManagerBasedTotalUser(UserType, LocationID, UserID).Select(x => new ManagerModel()
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
        /// Created By Vijay sahu on 13 june 2015
        /// Get All Active Managers who mapped with location based On LocationID
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public List<WorkOrderEMS.Models.UserModel> GetAllManagerUsingID(long locationId)
        {

            if (locationId > 0)
            {

                List<WorkOrderEMS.Models.UserModel> objManagerModel = null;
                workorderEMSEntities Context = new workorderEMSEntities();


                objManagerModel = Context.ManagerLocationMappings.Where(d => d.IsDeleted == false)
                    .Join(Context.UserRegistrations, x => x.ManagerUserId, y => y.UserId, (x, y) => new { x, y })
                    .Where(z => z.x.IsDeleted == false && z.y.IsEmailVerify == true && z.y.IsLoginActive == true && z.x.LocationId == locationId && z.y.UserType == 2) // 2 Manager
                    .Select(result => new WorkOrderEMS.Models.UserModel
                    {
                        UserEmail = result.y.UserEmail,
                        UserId = result.y.UserId
                    }).ToList();


                return objManagerModel;
            }
            else
            {
                return null;
            }
        }
    }
}
