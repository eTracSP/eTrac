using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel.ApiModel;

namespace WorkOrderEMS.BusinessLogic.Interfaces
{
    public interface IPassengerTracking
    {
        Result SavePassengerTrackingRoute(eFleetPassengerTrackingModel objeFleetPassengerTrackingModel);

        List<eFleetPassengerTrackingRouteModel> GetAllPassengerTrackingRouteDetails(eFleetPassengerTrackingRouteServiceModel obj);
        ServiceResponseModel<string> InsertPassengerTrackingCount(eFleetPassengerTrackingCountModelForService objModel);
        eFleetPassengerTrackingModel GeteFleetPassengerTrackingDetailsById(long RouteId);
        Result DeleteeFleetPassengerTracking(long passenggerId, long loggedInUserId, string location);
    }
}
