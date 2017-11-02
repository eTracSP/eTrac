using System;
using System.Globalization;
using System.Transactions;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.EntityModel;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class DashboardWidgetSettingManager : IDashboardWidgetSettingManager
    {

        DashboardWidgetSettingRepository objDashboardWidgetSettingRepository;

        /// <summary>Update the Dashboard Widgets
        /// <CreatedBy>Bhushan Dod </CreatedBY>
        /// <CreatedOn>May 13 2016</CreatedOn>
        /// <CreatedFor> Insert widget settings and delete if already exist</CreatedFor>
        /// </summary>
        /// <returns></returns>
        public bool UpdateDashboardWidgets(long UserId, long LocationId, string WidgetIds)
        {
            bool IsInserted = false;
            try
            {
                objDashboardWidgetSettingRepository = new DashboardWidgetSettingRepository();

                ////if (DeleteUserPermission(objPermissionDetailsModel.UserId, objPermissionDetailsModel.LocationId) == true)
                if (UserId != 0)
                {
                    objDashboardWidgetSettingRepository.DeleteAll(x => x.UserID == UserId && x.LocationId == LocationId);
                    //Commented due to above code line doing the same.
                    //var GetOldWidgetSetting = objDashboardWidgetSettingRepository.GetAll(x => x.UserID == UserId && x.LocationId == LocationId).ToList();
                    //if (GetOldWidgetSetting.Count > 0)
                    //{
                    //    foreach (var i in GetOldWidgetSetting)
                    //    {
                    //        objDashboardWidgetSettingRepository.Delete(i);
                    //        // objDashboardWidgetSettingRepository.SaveChanges();
                    //    }
                    //}
                    if (WidgetIds != "" && WidgetIds != null && WidgetIds.Trim() != "")
                    {
                        var userWidgetId = WidgetIds.Split(',');
                        foreach (var widget in userWidgetId)
                        {
                            if (widget != null && !string.IsNullOrEmpty(widget) && Convert.ToInt64(widget, CultureInfo.InvariantCulture) > 0)
                            {
                                long WidgetId = Convert.ToInt64(widget, CultureInfo.InvariantCulture);
                                IsInserted = AddWidgetSetting(WidgetId, UserId, LocationId);
                            }
                        }
                    }
                    else
                    {
                        IsInserted = true;
                    }


                }
                return IsInserted;
            }
            catch (Exception ex)
            { IsInserted = false; return IsInserted; }
        }


        /// <summary>Add the DashboardSettings
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedOn>May 13 2016</CreatedOn>
        /// <CreatedFor> Adding DashboardSettings gainst widget setting</CreatedFor>
        /// </summary>
        /// <returns></returns>
        public bool AddWidgetSetting(long WidgetId, long userId, long locationId)
        {
            DashboardWidgetSetting objDashboardWidgetSetting = new DashboardWidgetSetting();
            objDashboardWidgetSettingRepository = new DashboardWidgetSettingRepository();
            try
            {
                using (TransactionScope TranScope = new TransactionScope())
                {
                    objDashboardWidgetSetting.WidgetID = WidgetId;
                    objDashboardWidgetSetting.UserID = userId;
                    objDashboardWidgetSetting.LocationId = locationId;
                    objDashboardWidgetSetting.IsActive = true;
                    objDashboardWidgetSetting.CreatedBy = userId;
                    objDashboardWidgetSetting.CreatedDate = DateTime.UtcNow;
                    objDashboardWidgetSettingRepository.Add(objDashboardWidgetSetting);
                    TranScope.Complete();
                }
                if (objDashboardWidgetSetting.DisplayID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "   public bool AddWidgetSetting(long WidgetId, long userId, long locationId)", "Exception in  DashboardWidgetSettingManager.cs", WidgetId);
                return false;
            }
        }
    }
}
