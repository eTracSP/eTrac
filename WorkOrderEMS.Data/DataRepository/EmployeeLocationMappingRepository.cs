using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data
{
    public class EmployeeLocationMappingRepository : BaseRepository<EmployeeLocationMapping>
    {
        /// <summary>
        /// To Get Manager Locations
        /// </summary>
        /// <CreatedBy>Manoj jaswal</CreatedBy>
        /// <CreatedDate>2015/2/20</CreatedDate>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<UserLocations> GetEmployeeLocationList(long UserID)
        {
            workorderEMSEntities obj_workorderEMSEntities = new workorderEMSEntities();
            return obj_workorderEMSEntities.EmployeeLocationMappings.Join(obj_workorderEMSEntities.LocationMasters,
                (x => x.LocationId),
                (y => y.LocationId),
                ((x, y) => new { x, y })).Where(m => m.x.EmployeeUserId == UserID && m.x.IsDeleted == false)
                .Select(z => new UserLocations()
                {
                    LocationID = z.y.LocationId,
                    LocationName = z.y.LocationName,
                    LocationCode = z.y.Address2,
                }).ToList<UserLocations>();

        }
        ///// <summary>
        ///// To Get Employees According to User Locations
        ///// </summary>
        ///// <CreatedBy>Manoj jaswal</CreatedBy>
        ///// <CreatedDate>2015/2/20</CreatedDate>
        ///// <param name="UserID"></param>
        ///// <returns></returns>
        //public List<UserModel> GetEmployeeByLocation(long LocationId)
        //{
        //    workorderEMSEntities obj_workorderEMSEntities = new workorderEMSEntities();
        //    List<UserModel> objlist = obj_workorderEMSEntities.EmployeeLocationMappings.Join(obj_workorderEMSEntities.UserRegistrations,
        //       (x => x.EmployeeUserId),
        //       (y => y.UserId),
        //       ((x, y) => new { x, y })).Where(m => m.x.LocationId == LocationId && m.x.IsDeleted == false && m.y.IsLoginActive == true && m.x.IsDeleted == false)
        //       .Select(z => new UserModel()
        //       {
        //           UserId = z.y.UserId,
        //           FirstName = z.y.FirstName,
        //           LastName = z.y.LastName,

        //       }).ToList<UserModel>();
        //    return objlist;
        //}

        /// <summary>
        /// To Get Employees According to User Locations those have permission of eMaintenance
        /// Note :- If something went wrong in application upper code is the backup of previous method
        /// </summary>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedDate>2015/06/25</CreatedDate>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<UserModel> GetEmployeeByLocation(long LocationId)
        {
            workorderEMSEntities obj_workorderEMSEntities = new workorderEMSEntities();
            List<UserModel> objlist = obj_workorderEMSEntities.EmployeeLocationMappings
                                      .Join(obj_workorderEMSEntities.UserRegistrations, (x => x.EmployeeUserId), (y => y.UserId), ((x, y) => new { x, y }))
                                      .Join(obj_workorderEMSEntities.PermissionDetails, (z => z.x.EmployeeUserId), (t => t.UserId), (z, t) => new { z,t})
                                      .Where(m => m.z.x.LocationId == LocationId && m.z.x.IsDeleted == false && m.z.y.IsLoginActive == true && m.z.x.IsDeleted == false
                                               && m.t.LocationId == LocationId && (m.t.PermissionId == 190 || m.t.PermissionId == 4))//190 code for eMaintenance
               .Select(s => new UserModel()
               {
                   UserId = s.z.y.UserId,
                   FirstName = s.z.y.FirstName,
                   LastName = s.z.y.LastName,

               }).ToList<UserModel>();
            return objlist;
        }

        public List<SP_GetEmployeeByLocation_Result> GetEmployeeByLocDetailed(long Loc_ID)
        {
            workorderEMSEntities obj_workorderEMSEntities = new workorderEMSEntities();
            return obj_workorderEMSEntities.SP_GetEmployeeByLocation(Loc_ID).ToList();
        }

    }
}
