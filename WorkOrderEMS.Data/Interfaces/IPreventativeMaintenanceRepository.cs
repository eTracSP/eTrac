using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data.Interfaces
{
    public interface IPreventativeMaintenanceRepository
    {
        List<eFleetVehicleModel> GetVehicleNumber();
        List<eFleetMeterModel> GetAllMeterValue();
        List<GlobalCodeModel> GetAllMeterList();
       
        //List<SelectListItem> GetAllCategory();
        //eFleetPMModel GetAlleFleetPMList(eFleetPMModel objeFleetPMModelList);
        //eDetailsPM GetListeFleetPMDetails(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType);
        //eFleetPMModel GeteFleetPMDetailsById(long pmId);
        //Result DeleteeFleetPM(long VehicleId, long loggedInUserId);
        //List<GlobalCodeModelDDL> GetAllCategory();
    }
}
