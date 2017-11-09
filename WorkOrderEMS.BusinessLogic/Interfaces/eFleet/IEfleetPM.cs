using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;
using static WorkOrderEMS.Models.PreventativeMaintenance;
namespace WorkOrderEMS.BusinessLogic
{
   public interface IEfleetPM
    {
       // eFleetPMModel SaveEfleetPreventativeMaintenance(eFleetPMModel objeFleetPMModel);
        List<eFleetVehicleModel> GetAllVehicleNumber();

        ServiceResponseModel<string> InsertPreventativeMaintenance(eFleetPreventaticeMaintenanceModel obj);

        eFleetPMModel SaveEfleetPreventativeMaintenance(eFleetPMModel objeFleetPMModel);

        List<GlobalCodeModel> GetAllMeterList();
        List<eFleetMeterModel> GetAllMilesValue();
        //List<SelectListItem> GetAllCategory();
        eFleetPMModel GetAlleFleetPMList(eFleetPMModel objeFleetPMModelList);
        eDetailsPM GetListeFleetPMDetails(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType);
        eFleetPMModel GeteFleetPMDetailsById(long pmId);
        Result DeleteeFleetPM(long VehicleId, long loggedInUserId, string location);
        List<GlobalCodeModelDDL> GetAllCategory();
        List<PendingPM> GetAllPendingPMReminderDescription(long LocationID, long VehicleID);
    }
}
