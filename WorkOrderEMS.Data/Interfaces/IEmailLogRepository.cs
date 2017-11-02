using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;
using System.Data.Entity.Core.Objects;

namespace WorkOrderEMS.Data.Interfaces
{
    public interface IEmailLogRepository
    {
        List<EmailLogModel> GetAllEmailList(long? emailId, long? locationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords);    
        Task<List<EmailToManagerModel>> SendEmailToManagerForeFleetInspection(long LocationId, long UserId);
    }
}
