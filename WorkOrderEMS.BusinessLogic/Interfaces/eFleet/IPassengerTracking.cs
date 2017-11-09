using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;
using WorkOrderEMS.Models.ServiceModel.ApiModel;

namespace WorkOrderEMS.BusinessLogic.Interfaces
{
    public interface IPassengerTracking
    {
        Result SavePassengerTrackingRoute(eFleetPassengerTrackingModel objeFleetPassengerTrackingModel);

        List<eFleetPassengerTrackingRouteModel> GetAllPassengerTrackingRouteDetails(eFleetPassengerTrackingRouteServiceModel obj);
        ServiceResponseModel<string> InsertPassengerTrackingCount(eFleetPassengerTrackingCountModelForService objModel);
        eFleetPassengerTrackingModel GeteFleetPassengerTrackingDetailsById(long RouteId);
        JQGridModel<eFleetPassengerTrackingModel> GetListeFleetPassengerRoutewithJQGridDetails(int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, long? statusType);
        Result DeleteeFleetPassengerTracking(long passengerId, long loggedInUserId);
    }
}
