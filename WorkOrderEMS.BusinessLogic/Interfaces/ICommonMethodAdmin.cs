using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public interface ICommonMethodAdmin
    {
        /// <summary>AssignLocationToAdminUser
        /// <CreatedOn>Nov-17-2014</CreatedOn>
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="AdminUserId"></param>
        void AssignLocationToAdminUser(long locationId, long adminUserId); 
       
        /// <summary>AssignLocationToManagerUser
        /// <CreatedOn>Sep-13-2016</CreatedOn>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="AdminUserId"></param>
        void AssignLocationToManagerUser(long locationId, long managerUserId);
        
        /// <summary>RemoveLocationForAdminUser
        /// <CreatedOn>Nov-17-2014</CreatedOn>
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="AdminUserId"></param>
        void RemoveLocationForAdminUser(long locationId, long adminUserId);
        
        ///
        /// Created By Gayatri To bind the drop 
        /// down Location on QRC setup page
        ///
        List<SelectListItem> GetLocationByAdminId(long adminId);

        List<UserModelList> GetManagerByAdminId(long adminId);

        
        /// <summary>AssignLocationToEmployeeUser
        /// <CreatedOn>Sep-14-2016</CreatedOn>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="AdminUserId"></param>
        void AssignLocationToEmployeeUser(long locationId, long empUserId);
    }
}
