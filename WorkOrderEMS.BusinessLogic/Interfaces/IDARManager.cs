using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.BusinessLogic
{
    public interface IDARManager
    {
        //List<DARModelList> GetDARDetails(long? locationId, long? userId, DateTime? fromDate, DateTime? toDate, int? taskType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords);
        List<DARModelList> GetDARDetails(long? LoginUserId, long? locationId, long? userId, int? taskType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords, string fromDate = null, string toDate = null);

        ServiceDARModel SaveDARDetails(ServiceDARModel obj);

        ServiceDARModel UserLocationMappingDelete(ServiceDARModel obj, string locationname, string userType);

        DARModel GetDARById(long? darId);

        Result UpdateDAR(DARModel objDARMOdel);
        ServiceDisclaimerModel SaveDisclaimerDARDetails(ServiceDisclaimerModel obj);
        ServiceDARModel UpdateEndTimeDAR(ServiceDARModel obj);
        DARModelList GetDARDetailsById(long? darId);

        long SaveeFleetDAR(ServiceDARModel ObjServiceDARModel);

    }
}
