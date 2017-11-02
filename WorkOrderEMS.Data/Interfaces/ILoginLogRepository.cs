using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data.Interfaces
{
    interface ILoginLogRepository
    {
        /// <summary>
        /// Created By :Bhushan Dod
        /// Created Date: 26-05-2015
        /// Description : For Save log of login to trac the employee idle state
        /// </summary>
        /// <param name="objLoginLogModel"></param>
        /// <returns>LogId</returns>
        long SaveLoginLog(LoginLogModel objLoginLogModel);
        sp_GetIdleStatusOfEmployee_Result IdleStatusOfEmployee(long UserId, long LocationId);
    }
}
