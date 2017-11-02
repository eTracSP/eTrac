using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;

namespace WorkOrderEMS.Data
{
    public class ServiceMasterRepository : BaseRepository<ServiceMaster>, IServiceMasterRepository
    {
    }
}
