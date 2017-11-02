using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;


namespace WorkOrderEMS.BusinessLogic
{
    public interface IWorkRequestAssignment
    {
        Result DeleteWorkRequest(long workRequestId, long loggedInUserId, DARModel objDAR, string location);
        WorkRequestAssignmentModel GetWorkorderAssignmentByID(long WorkRequestAssignmentID);
        List<WorkRequestAssignmentModelList> GetAllWorkRequestCreatedByClient(long? WorkRequestAssignmentID, long? RequestedBy, string OperationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, long LocationID, long UserID, DateTime StartDate, DateTime EndDate,string filter, ObjectParameter paramTotalRecords);
        Result AcceptWorkOrderByEmployee(long WorOrderID, long iUserId);
        
        Result StartWorkOrderByEmployee(long WorOrderID, long iUserId, string StartTime);
        Result CompleteWorkOrderByEmployee(long WorOrderID, long iUserId, string EndTime);
        List<EmployeeWorkAssignmentCountModel> GetEmployeeTotalWorkStatus(long UserId, long LocationId);

        bool WorkFrSignature(long WorkAssignmentID, string ImageName, string ImageNameEmp, string DisclaimerName, string SurveyName, string SurveyEmailID);
        ServiceWorkAssignmentModel UrgentWOAccpetedByEmployee(ServiceDARModel obj);
        CheckWorkRequestforDeleteUser CheckingContinuousWorkRequestForUser(long userId);
    }
}
