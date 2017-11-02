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
    public class LocationClientMappingRepository : BaseRepository<LocationClientMapping>, ILocationClientMappingRepository
    {
        /// <summary>
        /// To Get Manager Locations
        /// </summary>
        /// <CreatedBy>Manoj jaswal</CreatedBy>
        /// <CreatedDate>2015/4/06</CreatedDate>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<UserLocations> GetClientLocationList(long UserID)
        {
            workorderEMSEntities obj_workorderEMSEntities = new workorderEMSEntities();
            return obj_workorderEMSEntities.LocationClientMappings.Join(obj_workorderEMSEntities.LocationMasters,
                (x => x.LocationId),
                (y => y.LocationId),
                ((x, y) => new { x, y })).Where(m => m.x.ClientUserId== UserID && m.x.IsDeleted == false)
                .Select(z => new UserLocations()
                {
                    LocationID = z.y.LocationId,
                    LocationName = z.y.LocationName,

                }).ToList<UserLocations>();

        }
    }
}
