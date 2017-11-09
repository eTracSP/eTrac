using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;
using WorkOrderEMS.Models.ManagerModels;

namespace WorkOrderEMS.BusinessLogic
{
    public interface IManageManager
    {
        UserModel GetEmployeeById(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords);
        Result SaveEmployee(UserModel objUserModel, out long QRCID, bool isEmployeeRegistration);
        Result AssignProfile(UserModel objUserModel);
        //Result SaveWorkRequest(WorkRequestModel objWorkRequestModel);
        //Result SaveWorkOrder(WorkOrderModel objWorkOrderModel);

        /// <summary>
        /// CreatedOn   :   Oct-06-2014
        /// CreatedBy   :   Nagendra Upwanshi
        /// </summary>
        /// <param name="objUserModel"></param>
        /// <param name="QRCID"></param>
        /// <param name="IsEmployeeRegistration"></param>
        //void UpdateUser(UserModel objUserModel, out long QRCID, bool IsEmployeeRegistration);
        void UpdateUser(UserModel objUserModel, out long qrcId, bool isEmployeeRegistration, out UserRegistration user);

        bool CheckDuplicateUser(string userEmail, long userId, out long qrcId, out UserRegistration objUserRegistration);

        long GetTotalManagerCount(string LoginUserType, long LocationID, long iUserID);
        List<ManagerModel> GetUserByManager(string LoginUserType, long LocationID, long iUserID);

        dynamic EmployeeIdleStatus(long locationId, long userId);

        Task<dynamic> IdleEmployeeAlert(long UserId);

        /// <summary>
        /// Created By :Bhushan Dod
        /// Created Date : 01/06/2015
        /// Description : For change the idle time limit by Manager
        /// </summary>
        /// <param name="id"></param>
        /// <param name="time"></param>
        /// <param name="managerId"></param>
        /// <returns></returns>
        bool UpdateEmployeeIdleTime(string id, string time, long managerId);

        dynamic GetUpdatedEmployeeTimeLimit(long id);

        dynamic FacRequestNotAcceptPushNotificaiton(long UserId);

        /// <summary>
        /// Created By Vijay sahu on 13 june 2015
        /// Get All Active Managers who mapped with location based On LocationID
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        List<UserModel> GetAllManagerUsingID(long locationID);

        /// <summary>
        /// Created By : Bhushan Dod
        /// Created Date : 07/22/2015
        /// Description : Send Email to manager if Qrc Expiration is today(DateTime.Now)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool EmailToMangerForQrcExpiration(long LocId, long userId, long userType);
    }
}
