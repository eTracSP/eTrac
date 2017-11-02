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
    public class VehicleRepository : BaseRepository<VehicleMaster>, IVehicleRepository
    {

        workorderEMSEntities objworkorderEMSEntities = new workorderEMSEntities();
        /// <summary>GetVehicleDetails
        /// CreatedBy   :   Nagendra Upwanshi
        /// CreatedOn   :   Sep-06-2014
        /// CreatedFor  :   Get Vendor Details
        /// </summary>
        /// <returns></returns>
        List<VehicleModel> GetVehicleDetails()
        {
            return new List<VehicleModel>();

        }


       
    }
}
