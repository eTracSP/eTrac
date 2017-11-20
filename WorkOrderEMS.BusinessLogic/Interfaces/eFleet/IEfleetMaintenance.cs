using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel.ApiModel;

namespace WorkOrderEMS.BusinessLogic.Interfaces.eFleet
{
    public interface IEfleetMaintenance
    {
        List<eFleetVehicleModel> GetVehicleNumber(long LocationID);
        List<GlobalCodeModel> GetAllMaintenanceType();
        List<PendingPM> GetAllPendingPMReminderDescription(long LoctionID);
        //eFleetMaintenanceModel SaveEfleetMaintenance(eFleetMaintenanceModel objeFleetMaintenanceModel);
        eFleetMaintenanceModel SaveEfleetMaintenance(eFleetMaintenanceModel objeFleetMaintenanceModel);
        eFleetMaintenanceModel GetAllMaintenanceList(eFleetMaintenanceModel objeFleetMaintenanceModelList);
        eFleetMaintenanceModel GeteFleetMaintenanceDetailsById(long maintenanceId);
        Result DeleteeFleetMaintenance(long maintenanceId, long loggedInUserId, string location);
        eDetailsMaintenance GetListeFleetMaintenanceDetails(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType);
        ServiceResponseModel<string> InsertMaintenance(eFleetMaintenanceModelForApiService objModel);
        List<PendingPM> GetPendingPM(string VehicleNumber, long LocationID);
    }
}
