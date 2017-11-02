using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;

namespace WorkOrderEMS.Data.DataRepository
{
    public class PremissionDetailsRepository : BaseRepository<PermissionDetail>
    {

        /// <summary>
        /// To Get Premissions of Users
        /// </summary>
        /// <CreatedBy>Manoj jaswal</CreatedBy>
        /// <CreatedDate>2015/2/20</CreatedDate>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public  IQueryable<List<string>> GetUserPremissionList(long UserID)
        {
            workorderEMSEntities obj_workorderEMSEntities = new workorderEMSEntities();
            return obj_workorderEMSEntities.PermissionDetails.Join(obj_workorderEMSEntities.GlobalCodes,
                (x => x.PermissionId),
                (y => y.GlobalCodeId),
                ((x, y) => new { x, y })).Where(m => m.x.UserId == UserID && m.x.IsDeleted == false)
                .Select(z => new List<string>(){
                z.y.CodeName,
                }) ;

        }
    }
}
