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
    public class AdminLocationMappingRepository : BaseRepository<AdminLocationMapping>, IAdminLocationMappingRepository
    {
        /// <summary>
        /// To Get Manager Locations
        /// </summary>
        /// <CreatedBy>Manoj jaswal</CreatedBy>
        /// <CreatedDate>2015/2/20</CreatedDate>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<UserLocations> GetAdminLocationList(long UserID)
        {
            workorderEMSEntities obj_workorderEMSEntities = new workorderEMSEntities();
            return obj_workorderEMSEntities.AdminLocationMappings.Join(obj_workorderEMSEntities.LocationMasters,
                (x => x.LocationId),
                (y => y.LocationId),
                ((x, y) => new { x, y })).Where(m => m.x.AdminUserId == UserID && m.x.IsDeleted == false)
                .Select(z => new UserLocations()
                {
                    LocationID = z.y.LocationId,
                    LocationName = z.y.LocationName,
                    LocationCode=z.y.Address2,

                }).ToList<UserLocations>();

        }
    }
}
