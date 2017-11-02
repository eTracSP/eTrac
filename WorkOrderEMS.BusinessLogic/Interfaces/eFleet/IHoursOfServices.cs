using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel.ApiModel;

namespace WorkOrderEMS.BusinessLogic.Interfaces.eFleet
{
    public interface IHoursOfServices
    {
        ServiceResponseModel<string> InsertHoursOfServices(HoursOfServicesModel objModel);
    }
}
