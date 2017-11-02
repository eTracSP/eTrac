using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data.Interfaces
{
    public interface IUserRepository
    {
        UserModel GetUserById(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter paramTotalRecords);
        List<UserModelList> GetAllVerfiedUser(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter paramTotalRecords);

        /// <summary>GetAllVerfiedUsers
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedFor>Get All IT Administrator List</CreatedFor>
        /// <CreatedOn>Nov-14-2014</CreatedOn>
        /// <param name="UserID"></param>
        /// <param name="OperationName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        List<UserModelList> GetAllVerfiedUsers(long? userId, long locationId, string useType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, out long totalRecords);

        /// <summary>ListLocationAdministrator
        /// <CreatedFor>ListLocationAdministrator</CreatedFor>
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Dec-10-2014</CreatedOn>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        List<UserModelList> ListLocationAdministrator(long locationId, string UserType = "");

        /// <summary>
        /// created by vijay sahu on 10 march 2015
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        List<AdminUserForDrop> UnAssignedAdministrationIdRepo(long LocationId, string UserType = "");
        List<Proc_GetAllWorkAssignedToEmployee_Result> GetEmployeeAssignedWorkRequest(long LocationId, long UserID, string OrderBy, string columnName);

        List<UserModelList> GetUnVerifiedUsers(long? userId, long locationId, string useType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, out long totalRecords);
        List<UserModelList> GetAllVerfiedUsersDAROnly(long? userId, long locationId, string useType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, out long totalRecords);
    }
}
