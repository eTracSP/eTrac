using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data
{
    public interface IReportRepository
    {
        List<CleaningModel> NoCleaningDone();
        //List<TrashData> GetTrashData();
    }
}
