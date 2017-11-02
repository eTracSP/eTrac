using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;

namespace WorkOrderEMS.Data.DataRepository
{
    public class eFleetVehicleScanLogRepository : BaseRepository<eFleetVehicleScanLog>
    {
        public long SaveeFleetVehicleScanLog(long vehicleId, long vehicleType, long userId, long locationId)
        {
            var ObjeFleetVehicleScanLog = new eFleetVehicleScanLog();
            try
            {
                ObjeFleetVehicleScanLog.VehicleID = vehicleId;
                ObjeFleetVehicleScanLog.LocationId = locationId;
                ObjeFleetVehicleScanLog.InspectionType = vehicleType;
                ObjeFleetVehicleScanLog.CreatedBy = userId;
                ObjeFleetVehicleScanLog.CreatedOn = DateTime.UtcNow;
                ObjeFleetVehicleScanLog.DeletedBy = null;
                ObjeFleetVehicleScanLog.DeletedOn = null;
                ObjeFleetVehicleScanLog.IsDeleted = false;
                ObjeFleetVehicleScanLog.ModifiedBy = null;
                ObjeFleetVehicleScanLog.ModifiedOn = null;
                ObjeFleetVehicleScanLog.ScanUserId = userId;

                Add(ObjeFleetVehicleScanLog);
                long VehicleScanLogId = ObjeFleetVehicleScanLog.eFleetVehicleScanLogId;

                return VehicleScanLogId;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
