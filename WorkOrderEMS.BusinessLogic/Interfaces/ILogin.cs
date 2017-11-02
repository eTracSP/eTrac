using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;
using WorkOrderEMS.Models.SuperAdminModels;

namespace WorkOrderEMS.BusinessLogic
{
    public interface ILogin
    {
        /// <summary>SetLogin
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-20-2014</CreatedOn>
        /// <CreatedFor>Set Login</CreatedFor>
        /// </summary>
        /// <param name="UserTypeId"></param>
        /// <returns></returns>
        eTracLoginModel SetLogin(long UserTypeId);

        /// <summary>AuthenticateUser
        /// <CreatedBy>Nagendra Upwanshi</CreatedBY>
        /// <CreatedFor>Authenticate User Login</CreatedFor>
        /// <CreatedOn>Nov-27-2014</CreatedOn>
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        eTracLoginModel AuthenticateUser(eTracLoginModel loginViewModel);

        /// <summary>RecoveryEmailPassword
        /// <CreatedBY>Nagendra Upwanshi</CreatedBY>
        /// <CreatedFor>Recovery Email password</CreatedFor>
        /// <CreatedOn>Dec-02-2014</CreatedOn>
        /// </summary>
        /// <param name="eTracLogin"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool RecoveryEmailPassword(eTracLoginModel eTracLogin, out string message);
        //bool RecoveryEmailPassword(eTracLoginModel eTracLogin, out string message, out string RecoverPassword);

        /// <summary>ChangePassword
        /// <Createdby>Nagendra Upwanshi</Createdby>
        /// <CreatedDate>Aug-22-2014</CreatedDate>
        /// </summary>
        /// <param name="OldPassword"></param>
        /// <param name="NewPassword"></param>
        /// <returns></returns>
        bool ChangePassword(eTracLoginModel eTracLogin, out string message);


        /// <summary>
        /// Create by manoj jaswal on 20 feb 2015
        /// modified by vijay sahu on 29 april 2015
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        List<string> GetUserPremissionList(long userId,long Usertype,long locationId);
        /// <summary>
        /// To Get Assigned User Location
        /// </summary>
        /// <param name="UserType"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        List<UserLocations> GetUserAssignedLocations(long UserType, long UserID);

        List<UserLocations> GetManagerAssignedLocation(long UserID);
        List<UserLocations> GetEmployeeAssignedLocation(long UserID);
        List<UserLocations> GetAdminAssignedLocation(long UserID);



        Result LogoutWeb(long userId, long loginLogID);
        eTracLoginModel InsertLoginLog(eTracLoginModel objeTracLoginModel);
        /// <summary>To Update loginlog active status.
        /// <Createdby>Bhushan Dod</Createdby>
        /// <CreatedDate>April-25-2016</CreatedDate>
        /// </summary>
        /// <param name="eTracLogin"></param>
        /// <returns></returns>
        bool ChangeLoginLogActiveStatus(eTracLoginModel eTracLogin);

        WidgetList GetDashboardWidgetList(long userId, long locationId);
    }
}
