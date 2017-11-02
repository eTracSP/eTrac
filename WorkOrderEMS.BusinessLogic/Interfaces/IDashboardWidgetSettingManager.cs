using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.BusinessLogic
{
   public interface IDashboardWidgetSettingManager
    {
        /// <summary>Update the Dashboard Widgets
        /// <CreatedBy>Bhushan Dod </CreatedBY>
        /// <CreatedOn>May 13 2016</CreatedOn>
        /// <CreatedFor> Insert widget settings and delete if already exist</CreatedFor>
        /// </summary>
        /// <returns></returns>
       bool UpdateDashboardWidgets(long UserId, long LocationId, string WidgetIds);

       /// <summary>Add the DashboardSettings
       /// <CreatedBy>Bhushan Dod</CreatedBY>
       /// <CreatedOn>May 13 2016</CreatedOn>
       /// <CreatedFor> Adding DashboardSettings gainst widget setting</CreatedFor>
       /// </summary>
       /// <returns></returns>
       bool AddWidgetSetting(long WidgetId, long userId, long locationId);
    }
}
