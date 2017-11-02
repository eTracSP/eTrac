using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data.Interfaces
{
    public interface ILocationRepository
    {
        List<LocationListModel> GetAllLocationList(int LocationID, string OperationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter paramTotalRecords);

        /// <summary>GetListAllLocation
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Dec-04-2014</CreatedOn>
        /// <CreatedFor>Get All Location List</CreatedFor>
        /// </summary>
        /// <param name="LocationID"></param>        
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <param name="paramTotalRecords"></param>
        /// <returns></returns>
        List<ListLocationModel> GetListAllLocation(int? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter paramTotalRecords);


        List<UserLocations> GetUserLocations(long UserType, long UserID);
    }
}
