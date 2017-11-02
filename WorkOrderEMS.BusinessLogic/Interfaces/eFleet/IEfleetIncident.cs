using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.BusinessLogic
{
    interface IEfleetIncident
    {
        ServiceResponseModel<string> InsertVehicleIncident(eFleetIncidentModel objModel);
    }
}
