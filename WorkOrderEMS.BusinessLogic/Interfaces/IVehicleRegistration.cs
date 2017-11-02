using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.BusinessLogic
{
    public interface IVehicleRegistration
    {
        VehicleRegistrationModel GetVehicleDetailsById(long vehicleId);
        bool ManagerVehicleApproval(long vehicleId, long status, string declineReason);
        bool ClientVehicleApproval(long vehicleId, long status, string declineReason);
        Result DeleteVehicle(long vehicleId, long loggedInUserId);
        Tuple<GenerateQRCForVehicle_M, Result> GenerateQRCForVehicle(long vehicleId, long loggedInUserId);
    }
}
