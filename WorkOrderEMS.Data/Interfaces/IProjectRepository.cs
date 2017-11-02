using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data.Interfaces
{
    interface IProjectRepository
    {
        List<ProjectMasterListModel> GetAllProject(int ProjectID, string OperationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter paramTotalRecords);
    }
}
