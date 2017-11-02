using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;
namespace WorkOrderEMS.Data.Interfaces
{
    interface IPermissionDetails
    {
        List<PermissionDetailsModel> GetAssignPermissions(int userId,long locationId);

        WidgetList GetUserDashboardSettingList(long? UserId, long? LocationId);
    }
}
