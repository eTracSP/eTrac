using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models.UserModels;

namespace WorkOrderEMS.BusinessLogic
{
    public interface IEmployeeManager
    {
        List<EmployeeModel> GetEmployeeByLocDetailed(long Loc_ID);
    }
}
