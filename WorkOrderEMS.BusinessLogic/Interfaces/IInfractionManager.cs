using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Interfaces
{
    public interface IInfractionManager
    {
        List<InfractionList_M> InfractionListForGrid(long LocationId);

        InfractionList_M getInfractionDetails(long InfractionID);

        Result SaveInfractionLevelDetails(InfractionLevelDetails objInfractionLevelDetails);

        bool SendEmailToClient(long locationId,long infractionLevelDetailId);

        Result DeleteInfraction(long infractionId, long loggedInUser, DARModel objDAR, string locationName);

        List<InfractionList_M> GetAllInfractionDetails(long? infractionId, long? locationId, string operationType, int? statusType,int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords);
    }
}
