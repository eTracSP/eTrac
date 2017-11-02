using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data.Interfaces
{
    interface IVendorMasterRepository
    {
        /// <summary>GetAllVendorList
        /// CreatedBy   :   Nagendra Upwanshi
        /// CreatedOn   :   Sep-22-2014
        /// CreatedFor  :   Get All Vendor List
        /// </summary>
        /// <param name="VendorID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        List<VendorListModel> GetAllVendorList(long? VendorID, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch);

        #region vendor registration new

        /// <summary>GetAllRegisterVendorList
        /// CreatedBy   :   Nagendra Upwanshi
        /// CreatedOn   :   Sep-29-2014
        /// CreatedFor  :   Get All Vendor List
        /// </summary>
        /// <param name="VendorID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        List<VendorListModel> GetAllRegisterVendorList(long? VendorID, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, int? status, long? userId, long? locationId);
        List<PermitTypeDDModel> GetPermitType(int VendorID, string Fields, int PermitTypeID = 0, int VehicleTypeID = 0);
        #endregion vendor registration new
    }
}
