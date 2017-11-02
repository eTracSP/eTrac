using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.Data
{
    public class QRCMasterRepository : BaseRepository<QRCMaster>, IQRCRepository
    {
        workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();

        public List<QRCListModel> GetAllQRCList(long? QRCID, long? locationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, long? ddlQRCType, long userId, ObjectParameter TotalRecords)
        {///*
            try
            {
                List<QRCListModel> QRCList = _workorderEMSEntities.ssp_GetQRCDetails(QRCID, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, ddlQRCType, locationId, userId, TotalRecords).Select(q =>
                    new QRCListModel()
                    {
                        EncryptQRC = Cryptography.GetEncryptedData(q.QRCID.ToString(), true),
                        QRCName = q.QRCName,
                        QRCTYPE = q.ModuleName,
                        SpecialNotes = q.SpecialNotes,
                        WarrentyDoc = q.WarrantyDoc,
                        QRCodeID = q.QRCodeID,
                        CheckOutStatus = q.CheckOutStatus,//Added by Bhushan on 17/Nov/2016 for if vehicle checkout, manager able to checkin/out.
                        IsDamage = q.IsDamage,//Added by Bhushan on 17/Nov/2016 for if vehicle is damage,manager able to verify.
                        IsDamageVerified = q.IsDamageVerified,
                        QRCTYPEId = q.QRCTYPE,
                        LocationName = q.LocationName,
                        QRCSize = q.QRCSize
                    }).ToList();
                return QRCList;
            }
            catch (Exception)
            { throw; }
            //*/
            //return new List<QRCListModel>();
        }

        /// <summary>GetLastQRCID
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CraetedOn>Nov-10-2014</CraetedOn>
        /// <CreatedFor>GetLastQRCID</CreatedFor>
        /// </summary>
        /// <returns></returns>
        public long GetLastQRCID()
        {
            try
            {
                var resultList = _workorderEMSEntities.QRCMasters.OrderByDescending(x => x.QRCID);
                return (resultList != null && resultList.Count() > 0) ? resultList.First().QRCID : 0;
            }
            catch (Exception)
            { throw; }
        }

        #region For QRC
        public ServiceResponseModel<ssp_UpdateEscannerQRCTypeXML_Result> QRCSave(string ServiceAuthKey, long? UserId, long? QRCID, string ToXml, string Action, bool CheckOutStatus, bool IsDamage, string UserName = null)
        {
            ServiceResponseModel<ssp_UpdateEscannerQRCTypeXML_Result> ObjServiceResponseModel = new ServiceResponseModel<ssp_UpdateEscannerQRCTypeXML_Result>();
            try
            {
                ObjServiceResponseModel.Data = _workorderEMSEntities.ssp_UpdateEscannerQRCTypeXML(ServiceAuthKey, UserId, QRCID, ToXml, Action, CheckOutStatus, IsDamage, UserName).
                                                                                            Select(t => new ssp_UpdateEscannerQRCTypeXML_Result()
                                                                                            {
                                                                                                Response = t.Response,
                                                                                                ResponseMessage = t.ResponseMessage
                                                                                            }).FirstOrDefault();
            }
            catch (Exception) { throw; }
            return ObjServiceResponseModel;
        }
        #endregion For Qrc

        public List<QRCListModel> GetAllQRCListforPrint(long? QRCID, long? locationId, string sortColumnName, string sortOrderBy, string textSearch, long? ddlQRCType, long userId)
        {///*
            try
            {
                List<QRCListModel> QRCList = _workorderEMSEntities.ssp_GetQRCDetailsforPrint(QRCID, sortColumnName, sortOrderBy, textSearch, ddlQRCType, locationId, userId).Select(q =>
                    new QRCListModel()
                    {
                        EncryptQRC = Cryptography.GetEncryptedData(q.QRCID.ToString(), true),
                        QRCName = q.QRCName,
                        QRCTYPE = q.ModuleName,
                        QRCodeID = q.QRCodeID,
                        QRCTYPEId = q.QRCTYPE,
                        LocationName = q.LocationName,
                        QRCSize = q.QRCSize
                    }).ToList();
                return QRCList;
            }
            catch (Exception)
            { throw; }
            //*/
            //return new List<QRCListModel>();
        }

        #region For eFleet
        public ServiceResponseModel<UpdateEfleetInspectionTypeXML> eFLeetSave(string ServiceAuthKey, long? UserId, long? QRCID, string ToXml, string Action, bool IsDamage, string UserName = null)
        {
            ServiceResponseModel<UpdateEfleetInspectionTypeXML> ObjServiceResponseModel = new ServiceResponseModel<UpdateEfleetInspectionTypeXML>();
            try
            {
                
                ObjServiceResponseModel.Data = _workorderEMSEntities.ssp_UpdateEfleetInspectionTypeXML(ServiceAuthKey, UserId, QRCID, ToXml, Action, IsDamage, UserName).
                                                    Select(t => new UpdateEfleetInspectionTypeXML()
                                                    {
                                                        Response = t.Response,
                                                        ResponseMessage = t.ResponseMessage,
                                                        CheckOutStatus = t.CheckOutStatus,
                                                        DamageTireStatus = t.DamageTireStatus,
                                                        EmergencyAccessoriesStatus = t.EmergencyAccessoriesStatus,
                                                        EngineExteriorStatus = t.EngineExteriorStatus,
                                                        InteriorMileageDriverStatus = t.InteriorMileageDriverStatus,
                                                        DamageTireDetails = t.DamageTireDetails,
                                                        EmergencyAccessoriesDetails = t.EmergencyAccessoriesDetails,
                                                        InteriorMileageDriverDetails = t.InteriorMileageDriverDetails,
                                                        EngineExteriorDetails = t.EngineExteriorDetails,
                                                        IsDamage = t.IsDamage,
                                                        LocationID = t.LocationID,
                                                        QRCodeID = t.QRCodeID,
                                                        VehicleID = t.VehicleID,
                                                        VehicleNumber = t.VehicleNumber,
                                                        LocationName = t.LocationName,
                                                        Make = t.Make,
                                                        Model = t.Model
                                                        
                                                    }).FirstOrDefault();
            }
            catch (Exception) { throw; }
            return ObjServiceResponseModel;
        }
        #endregion For eFleet
    }
}
