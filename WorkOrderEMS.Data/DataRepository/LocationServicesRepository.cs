using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;

namespace WorkOrderEMS.Data
{
    public class LocationServicesRepository : BaseRepository<LocationService>, ILocationServicesRepository
    {
        /// <summary>
        /// TO GET LOCATIONSERVICES BY LOCATIONID
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>2/4/2015</CreatedDate>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        public List<string> GetLocationServicesByLocationID(long LocationId, long UserType)
        {

            try
            {


                workorderEMSEntities obj_workorderEMSEntities = new workorderEMSEntities();
                if (UserType == 198767354324356456)
                {
                    var data = obj_workorderEMSEntities.ServiceMasters
                            .Where(y => y.IsDeleted == false)
                            .OrderBy(y => y.ServiceName)
                            .Select(z => new List<string>(){
                        z.ServiceName,
                });
                    List<string> objListStr = new List<string>();
                    foreach (var e in data)
                    {
                        objListStr.Add(e[0]);
                    }
                    return objListStr;
                }
                else
                {
                    var data = obj_workorderEMSEntities.ServiceMasters.Join(obj_workorderEMSEntities.LocationServices,
                        (x => x.ServiceID),
                        (y => y.ServiceId),
                        ((x, y) => new { x, y }))
                        .Where(m => m.y.LocationID == LocationId && m.y.IsDeleted == false)
                        .OrderBy(g => g.x.ServiceName)
                        .Select(z => new List<string>(){
                z.x.ServiceName,
                });
                    List<string> objListStr = new List<string>();
                    foreach (var e in data)
                    {
                        objListStr.Add(e[0]);
                    }


                    if (UserType == 1 || UserType == 5)
                    {
                        objListStr.Add("Location Setup");
                        objListStr.Add("Manage Users");
                    }
                    return objListStr;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
