using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data.Interfaces
{
    interface IVehicleRegistration
    {
        VehicleRegistrationModel GetVehicleDetailsById(long vehicleId);
       
    }
}
