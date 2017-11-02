using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;

namespace WorkOrderEMS.Data
{
    public class DashboardWidgetSettingRepository : BaseRepository<DashboardWidgetSetting>
    {
        ///// <summary>
        ///// To Get Dashboard Settings of Users
        ///// </summary>
        ///// <CreatedBy>Bhushan Dod</CreatedBy>
        ///// <CreatedDate>2016-05-12</CreatedDate>
        ///// <param name="UserID">UserID,locationid</param>
        ///// <returns></returns>
        //public List<string> GetUserDashboardSettingList(long? UserId, long? LocationId)
        //{
        //    try
        //    {
        //        List<string> objListStr = new List<string>();
        //        using (workorderEMSEntities Context = new workorderEMSEntities())
        //        {
        //            var data = Context.DashboardWidgetSettings.Where(x => (x.UserID == UserId)
        //                                       && ((LocationId == 0 ? null : LocationId) == null || x.LocationId == LocationId)
        //                                       ).Select(x =>  x.GlobalCode.CodeName).ToList();                                                
        //            foreach (var e in data)
        //            {
        //                objListStr.Add(e);
        //            }
        //        }
        //        return objListStr;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
    }
}
