using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Interfaces
{
   public interface IPushNotification
    {

       ServiceResponseModel<string> SendNotification(ServicePushModel obj);
    }
}
