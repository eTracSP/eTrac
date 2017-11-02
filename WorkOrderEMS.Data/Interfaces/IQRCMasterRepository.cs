using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data.Interfaces
{
    interface IQRCMasterRepository
    {
        //List<QRCListModel> GetAllQRCList(long QRCID, string OperationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch);
        List<QRCListModel> GetAllQRCList(long? QRCID, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch);
    }
}
