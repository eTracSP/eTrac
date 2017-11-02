using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.BusinessLogic
{
    public interface IEfleetVehicleIncidentReport
    {
        List<StateModel> GetStateID();
        eFleetVehicleIncidentModel SaveEfleetVehicleIncident(eFleetVehicleIncidentModel objeFleetVehicleIncidentModel);
        List<eFleetVehicleModel> GetVehicleNumber();
        eFleetVehicleIncidentModel GetAllVehicleIncidentList(eFleetVehicleIncidentModel objeFleetVehicleIncidentModel);
        eFleetIncidentDetails GetListVahicleListDetails(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType);
        eFleetVehicleIncidentModel GetIncidentDetailsById(long VehicleId);
        Result DeleteeFleetIncidentVehicle(long VehicleId, long loggedInUserId, string location);
        ServiceResponseModel<string> InsertVehicleIncident(eFleetIncidentModel objModel);
    }
}
