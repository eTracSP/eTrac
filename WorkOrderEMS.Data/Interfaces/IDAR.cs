using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.Data
{
    interface IDAR
    {
        //<<<<<<< .mine
        //        //List<DARModel> GetDARDetails(long UserId, DateTime fromDate, DateTime toDate, long taskType, long? UseType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, out long totalRecords);
        //        List<DARModelList> GetDARDetails(long? locationId, long? userId, int? taskType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords, string fromDate = null, string toDate = null);
        //=======
        //List<DARModelList> GetDARDetails(long? locationId, long? userId, DateTime? fromDate, DateTime? toDate, int? taskType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords);
        List<DARModelList> GetDARDetails(long? LoginUserId, long? locationId, long? userId, int? taskType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords, string fromDate = null, string toDate = null);
       long SaveDisclaimerDAR(ServiceDisclaimerModel ObjServiceDARModel);
        long SaveDARDetails(ServiceDARModel ObjServiceDARModel);
    }
}
