using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using WorkOrderEMS.Models;


namespace WorkOrderEMS.Data.Interfaces
{
    interface IQRCRepository
    {
        //List<QRCListModel> GetAllQRCList(long QRCID, string OperationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch);
        //List<QRCListModel> GetAllQRCList(long? QRCID, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch);
        //List<QRCListModel> GetAllQRCList(long? QRCID, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch);
        List<QRCListModel> GetAllQRCList(long? QRCID, long? locationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, long? ddlQRCType, long userId, ObjectParameter TotalRecords);



        long GetLastQRCID();
        List<QRCListModel> GetAllQRCListforPrint(long? QRCID, long? locationId, string sortColumnName, string sortOrderBy, string textSearch, long? ddlQRCType, long userId);

    }
}
