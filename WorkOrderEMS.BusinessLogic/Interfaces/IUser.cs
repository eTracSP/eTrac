using System;
using System.Collections.Generic;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.UserModels;

namespace WorkOrderEMS.BusinessLogic
{
    public interface IUser
    {
        UserModel GetUserDetailsById(long userId);
        Result DeleteUser(long UserId, long DeletedBy, DARModel objDAR);
        List<AllWorkAssignedToEmployeeModel> GetEmployeeAssignedWorkRequest(long LocationId, long UserID, string OrderBy, string columnName);

        /// <summary>
        /// Added by vijay sahu on 12 june 2015
        /// get Image Picture of user based on his userid
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        string getProfilePicture(long UserId);

        /// <summary>
        /// To Delete the User
        /// </summary>
        /// <CreatedBy>Vijay Sahu</CreatedBy>
        /// <CreatedDate>2015-06-22</CreatedDate>
        Tuple<int, string> DeleteUserFromUserList(long UserId, long DeletedBy, DARModel objDAR);

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created Date:08/Oct/2016
        /// This method verify the user in repository.
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="DeletedBy"></param>
        /// <param name="objDAR"></param>
        /// <returns></returns>
        Result UpdateVerifyUser(long UserId, long ModifiedBy, DARModel objDAR);

    }
}
