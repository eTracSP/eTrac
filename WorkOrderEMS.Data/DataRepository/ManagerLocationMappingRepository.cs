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
    public class ManagerLocationMappingRepository : BaseRepository<ManagerLocationMapping>, IManagerLocationMappingRepository
    {
        /// <summary>
        /// To Get Manager Locations
        /// </summary>
        /// <CreatedBy>Manoj jaswal</CreatedBy>
        /// <CreatedDate>2015/2/20</CreatedDate>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<UserLocations> GetManagerLocationList(long UserID)
        {
            workorderEMSEntities obj_workorderEMSEntities = new workorderEMSEntities();
            return obj_workorderEMSEntities.ManagerLocationMappings.Join(obj_workorderEMSEntities.LocationMasters,
                (x => x.LocationId),
                (y => y.LocationId),
                ((x, y) => new { x, y })).Where(m => m.x.ManagerUserId == UserID && m.x.IsDeleted == false)
                .Select(z => new UserLocations()
                {
                    LocationID=z.y.LocationId,
                    LocationName=z.y.LocationName,
                    LocationCode=z.y.Address2,
                }).ToList<UserLocations>();
        }

        /// <summary>
        /// To Get Admin Locations List
        /// </summary>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedDate>2015/06/22</CreatedDate>
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
                    LocationCode = z.y.Address2,
                }).ToList<UserLocations>();

        }

        /// <summary>
        /// To Get Locations List for Global Admin and IT Admin
        /// </summary>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedDate>2015/10/27</CreatedDate>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<UserLocations> GetAllLocationList(long UserID)
        {
            workorderEMSEntities obj_workorderEMSEntities = new workorderEMSEntities();
            return obj_workorderEMSEntities.LocationMasters.Where(m => m.IsDeleted == false)
                .Select(z => new UserLocations()
                {
                    LocationID = z.LocationId,
                    LocationName = z.LocationName,
                    LocationCode = z.Address2,
                }).ToList<UserLocations>();

        }
    }
}
