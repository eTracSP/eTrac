using System;
using System.Collections.Generic;
using System.Linq;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data
{

    public class QRCScanLogRepository : BaseRepository<QRCScanLog>
    {
        workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();
        public long SaveQRCScanLog(ServiceResponseModel<QRCModel> ObjServiceQrcModel, long UserId, long LocationId)
        {
            QRCScanLog ObjQRCScanLog = new QRCScanLog();
            try
            {

                ObjQRCScanLog.QRCID = ObjServiceQrcModel.Data.QRCId;
                ObjQRCScanLog.LocationId = LocationId;
                ObjQRCScanLog.QrcType = ObjServiceQrcModel.Data.QRCTYPE;
                ObjQRCScanLog.CreatedBy = UserId;
                ObjQRCScanLog.CreatedOn = DateTime.UtcNow;
                ObjQRCScanLog.DeletedBy = null;
                ObjQRCScanLog.DeletedOn = null;
                ObjQRCScanLog.IsDeleted = false;
                ObjQRCScanLog.ModifiedBy = null;
                ObjQRCScanLog.ModifiedOn = null;
                ObjQRCScanLog.ScanUserId = UserId;

                Add(ObjQRCScanLog);
                long QRCScanLogId = ObjQRCScanLog.QRCScanLogId;

                return QRCScanLogId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<sp_GetAllRoutinecheck_Result> GetRoutineCheck(long? LocationId, long? UserId, string FromDate = null, string ToDate = null)
        {
            List<sp_GetAllRoutinecheck_Result> lst = new List<sp_GetAllRoutinecheck_Result>();
            try
            {
                lst = _workorderEMSEntities.sp_GetAllRoutinecheck(UserId, LocationId, FromDate, ToDate).ToList();

                return lst;
            }
            catch (Exception)
            {
                throw;
            }
        }        
    }
}
