using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data
{
    public class WorkRequestRepository:BaseRepository<WorkRequestAssignment>
    {
        workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();

        //public List<WorkRequestModelList> GetAllWorkRequest(long? projectId,long? userID, string OperationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter paramTotalRecords)
        //{
        //    try
        //    {
        //        List<WorkRequestModelList> lstWorkRequest = _workorderEMSEntities.SP_GetAllWorkRequestAssignment(projectId,userID, OperationName, pageIndex,  sortColumnName, sortOrderBy,numberOfRows, textSearch, paramTotalRecords).Select(
        //            w => new WorkRequestModelList()
        //            {
        //                WorkRequestID =  w.WorkRequestAssignmentID,
        //                RequestBy = w.RequestBy,
        //                RequestByName = w.RequestBy,
        //                ProjectId = w.ProjectId,
        //                TaskName = w.TaskName,
        //                WorkArea = w.WorkArea,
        //                AreaName = w.AreaName,
        //                //TaskType = w.TaskType,
        //               // TaskTypeName = w.TaskTypeName,
        //                TaskPriority = w.TaskPriority,
        //                TaskPriorityName = w.TaskPriorityName,
        //                Remarks = w.Remarks,
        //                StartTime = w.StartTime,
        //                CompletionTime = w.CompletionTime,
        //                AssignedToUser = Convert.ToInt32(w.AssignedToUser),
        //                AssignedToUserName = w.AssignedToUserName,
        //                TaskStatus = Convert.ToInt32(w.TaskStatus),
        //                TaskStatusName = w.TaskStatusName,
        //                WorkOrderID = Convert.ToInt32(w.WorkOrderID),
        //                AssetID  = Convert.ToInt32(w.AssetID),
        //                AssetNo = w.AssetNo
        //            }).ToList();
        //        return lstWorkRequest;
        //    }
        //    catch (Exception )
        //    {                
        //        throw ;
        //    }
        //}
    }
}
