using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;

namespace WorkOrderEMS.Data
{
    public class PermissionDetailsRepository : BaseRepository<PermissionDetail>, IPermissionDetails
    {
        workorderEMSEntities _workorderEMSEntities;

        public List<PermissionDetailsModel> GetAssignPermissions(int userId, long LocationId)
        {
            try
            {
                List<PermissionDetailsModel> objList = null;
                using (_workorderEMSEntities = new workorderEMSEntities())
                {
                    objList = _workorderEMSEntities.PermissionDetails.Where(t => t.UserId == userId && t.LocationId == LocationId && t.IsDeleted == false)
                        .Select(x => new PermissionDetailsModel
                        {
                            PermissionId = x.PermissionId

                        }).ToList();
                }
                return objList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// To Get Premissions of Users
        /// </summary>
        /// <CreatedBy>Manoj jaswal</CreatedBy>
        /// <CreatedDate>2015/2/20</CreatedDate>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<string> GetUserPremissionList(long UserID, long Usertype, long locationid)
        {
            try
            {
                workorderEMSEntities obj_workorderEMSEntities = new workorderEMSEntities();
                if (Usertype == 154645645645645645) //not for any user 
                {
                    GlobalCodesRepository obj_GlobalCodesRepository = new GlobalCodesRepository();
                    var data = obj_GlobalCodesRepository.GetAll(x => x.IsDeleted == false && x.Category == "PERMISSION").ToList();
                    List<string> objListStr = new List<string>();
                    foreach (var e in data)
                    {
                        objListStr.Add(e.CodeName);
                    }
                    //objListStr.Add(data.Select(x=>x.co);

                    return objListStr;
                }
                else
                {
                    //    var data = obj_workorderEMSEntities.PermissionDetails.Join(obj_workorderEMSEntities.GlobalCodes,
                    //        (x => x.PermissionId),
                    //        (y => y.GlobalCodeId),
                    //        ((x, y) => new { x, y })).Where(m => m.x.UserId == UserID && m.x.LocationId == locationid && m.x.IsDeleted == false)
                    //        .Select(z => new List<string>(){
                    //z.y.CodeName,
                    //}).ToList();
                    //    List<string> objListStr = new List<string>();
                    //    foreach (var e in data)
                    //    {
                    //        objListStr.Add(e[0]);
                    //    }

                    List<string> objListStr = new List<string>();

                    using (workorderEMSEntities Context = new workorderEMSEntities())
                    {
                        //       select sm.servicename from PermissionDetails as pd
                        //inner join serviceMaster as sm
                        //    on pd.permissionId = sm.serviceId
                        //    where pd.locationid = 6 and pd.userId = 35  

                        var data = (from pd in Context.PermissionDetails
                                    join sd in Context.ServiceMasters
                                    on pd.PermissionId equals sd.ServiceID
                                    where pd.LocationId == locationid && pd.UserId == UserID
                                    select sd.ServiceName.Trim()
                                         ).ToList();

                        foreach (var e in data)
                        {
                            objListStr.Add(e);
                        }
                    }


                    return objListStr;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// To Get assigned dashboard Settings of Users
        /// </summary>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedDate>2016-05-12</CreatedDate>
        /// <param name="UserID">UserID,locationid</param>
        /// <returns></returns>
        public WidgetList GetUserDashboardSettingList(long? UserId, long? LocationId)
        {
            try
            {
                WidgetList objListStr = new WidgetList();
                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    objListStr.CheckedList = Context.DashboardWidgetSettings.Where(x => (x.UserID == UserId)
                                               && (x.LocationId == LocationId)
                                               ).Select(x => new Widget()
                                               {
                                                   WidgetID = x.WidgetID,
                                                   WidgetName = x.GlobalCode.CodeName
                                               }).ToList<Widget>();
                    //We have set 
                    objListStr.AllWidgetList = Context.GlobalCodes.Where(x => (x.Category == DashboardWidgetGlobalName.DashboardSetting)
                                           && (x.IsActive == true)
                                           ).Select(x => new Widget()
                                           {
                                               WidgetID = x.GlobalCodeId,
                                               WidgetName = x.CodeName
                                           }).ToList<Widget>();
                }
                return objListStr;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
