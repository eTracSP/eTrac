using System;
using System.Linq;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data
{
    public class LoginLogRepository : BaseRepository<LoginLog>, ILoginLogRepository
    {

        workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();

        /// <summary>
        /// Created By :Bhushan Dod
        /// Created Date: 26-05-2015
        /// Description : For Save log of login to trac the employee idle state
        /// </summary>
        /// <param name="objLoginLogModel"></param>
        /// <returns>LogId</returns>
        public long SaveLoginLog(LoginLogModel objLoginLogModel)
        {
            LoginLog Obj = new LoginLog();
            try
            {

                Obj.UserID = objLoginLogModel.UserID;
                Obj.LocationId = objLoginLogModel.LocationId;
                Obj.UserType = objLoginLogModel.UserType;
                Obj.CreatedBy = objLoginLogModel.UserID;
                Obj.CreatedOn = DateTime.UtcNow;
                Obj.DeletedBy = null;
                Obj.DeletedOn = null;
                Obj.IsDeleted = false;
                Obj.ModifiedBy = null;
                Obj.ModifiedOn = null;
                Add(Obj);
                long LogId = Obj.LogId;

                return LogId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public sp_GetIdleStatusOfEmployee_Result IdleStatusOfEmployee(long UserId, long LocationId)
        {
            sp_GetIdleStatusOfEmployee_Result obj = new sp_GetIdleStatusOfEmployee_Result();
            try
            {
                obj = _workorderEMSEntities.sp_GetIdleStatusOfEmployee(LocationId, UserId).Select(t => new sp_GetIdleStatusOfEmployee_Result()
                {
                    Response = t.Response,
                    ResponseLocation = t.ResponseLocation,
                    ResponseMessage = t.ResponseMessage
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
            return obj;
        }
    }
}
