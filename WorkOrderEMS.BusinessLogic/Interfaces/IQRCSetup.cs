using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.BusinessLogic
{
    public interface IQRCSetup
    {
        QRCModel GetGlobalCodeForCategories();

        bool ProcessQRCSetup(QRCModel ObjQRCModel, out long QRCId, out Result result, out PrintQRCModel ObjPrintQRCModel);
        //List<QRCListModel> GetAllQRCList(long? QRCID, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch);
        List<QRCListModel> GetAllQRCList(long? qrcId, long? locationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, long? ddlQRCType, long userId, ObjectParameter TotalRecords);
        /// <summary>
        /// CreatedBy   :   Nagendra Upwanshi
        /// CreatedOn   :   Sep-24-2014
        /// CreatedFor  :   Get QRC by QRCId
        /// </summary>
        /// <param name="QRCID"></param>
        /// <returns></returns>
        QRCModel GetQrcById(long qrcId);

        /// <summary>DeleteQRC
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-11-2014</CreatedOn>
        /// <CreatedFor>Delete QRC</CreatedFor>
        /// </summary>
        /// <param name="QRCID"></param>
        /// <param name="LoggedInUser"></param>
        /// <returns></returns>
        Result DeleteQRC(long qrcId, long loggedInUser, DARModel objDAR, string locationName);

        /// <summary>
        ///<CreatedBy>Bhushan Dod</CreatedBy> 
        ///<CreatedDate>Jan 27 2015</CreatedDate>
        ///<Description>Get Details of QRCID</Description>
        /// </summary>
        /// <param name="ObjServiceQRCModel"></param>
        /// <returns></returns>
        ServiceResponseModel<QRCModel> GetQRCDetailsByID(ServiceQrcModel ObjServiceQRCModel);

        /// <summary>
        /// Created by vijay sahu  on 20 march 2015
        /// This method is using for check the QRCName already exists in Record(DB) or not.
        /// </summary>
        /// <param name="QRCName"></param>
        /// <returns></returns>
        byte checkQRCName(string QRCName, long QRCType, long LocId);

        /// <summary>GetQRCMasterById
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Aug-24-2015</CreatedOn>
        /// </summary>
        /// <param name="qrcId"></param>
        /// <returns></returns>
        string GetQRCDetailById(long qrcId);

        /// <summary>UpdateQRCheckIn
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Nov-18-2016</CreatedOn>
        /// <CreatedFor>Check In QRC</CreatedFor>
        /// </summary>
        /// <param name="QRCID"></param>
        /// <param name="LoggedInUser"></param>
        /// <returns></returns>
        Result UpdateQRCheckIn(long qrcId, long loggedInUser, DARModel objDAR, string locationName);


        /// <summary>DamageFixed
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Nov-18-2016</CreatedOn>
        /// <CreatedFor>Mark damage has been been fixed.</CreatedFor>
        /// </summary>
        /// <param name="QRCID"></param>
        /// <param name="LoggedInUser"></param>
        /// <returns></returns>
        Result DamageFixed(long qrcId, long loggedInUser, DARModel objDAR, string locationName);

        List<QRCListModel> GetAllQRCListPrint(long? qrcId, long? locationId, string sortColumnName, string sortOrderBy, string textSearch, long? ddlQRCType, long userId);
        long QRCCodeIDExist(long QRCID);
        ServiceResponseModel<string> SaveQrcShuttleRequestDetails(ServiceQrcShuttleBusModel obj);
    }
}
