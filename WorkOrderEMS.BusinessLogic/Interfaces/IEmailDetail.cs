using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;
using WorkOrderEMS.Helper;


namespace WorkOrderEMS.BusinessLogic
{
    public interface IEmailDetail 
    {
        List<EmailLogModel> GetAllEmailList(long? emailId, long? locationId ,int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords);

        Result DeleteEmail(long emailLogId, long loggedInUserId);
    }
}
