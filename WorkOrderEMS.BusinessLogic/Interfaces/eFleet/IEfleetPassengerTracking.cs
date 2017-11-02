using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models.ServiceModel;
using WorkOrderEMS.Models.ServiceModel.ApiModel;

namespace WorkOrderEMS.BusinessLogic.Interfaces.eFleet
{
    public interface IEfleetPassengerTracking
    {
        List<eFleetPassengerTrackingRouteModel> GetAllPassengerTrackingRouteDetails(ServiceBaseModel obj);
    }
}
