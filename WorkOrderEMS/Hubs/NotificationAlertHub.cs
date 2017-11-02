using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Hubs
{
    public class NotificationAlertHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        [HubMethodName("sendMessages")]
        public static void SendMessages()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationAlertHub>();
            context.Clients.All.updateMessages();
        }

        [HubMethodName("getAllProgressWorkRequestAssignmentSignalR")]
        private string GetAllProgressWorkRequestAssignmentSignalR(long LocationId, long UserId, string SignalRequestType, string Filter)
        {
            try
            {
                WorkOrderEMS.BusinessLogic.Managers.GlobalAdminManager _GlobalAdminManager = new WorkOrderEMS.BusinessLogic.Managers.GlobalAdminManager();
                int? ProjectID = 0, UserID = 0;
                string textSearch = "";
                int? numberOfRows = 100000;
                int? pageIndex = 1;
                string sortColumnName = "";
                string sortOrderBy = "desc";
                string RequestType = SignalRequestType;
                string filter = Filter;
                DateTime StartDate = DateTime.UtcNow;
                DateTime EndDate = DateTime.UtcNow;
                long iUserID = UserId;
                long LocationID = LocationId;
                ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));

                var WorkRequestList = _GlobalAdminManager.GetAllWorkRequestAssignment(ProjectID, UserID, RequestType, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, LocationID, iUserID, StartDate, EndDate, filter, paramTotalRecords);
                if (WorkRequestList.Count() > 0)
                {
                    string result = "";
                    if (SignalRequestType.Trim() == "GetAllPendingWorkRequest" || SignalRequestType.Trim() == "GetAssignedWorktoEmployee")
                    {
                        result = BuildJQGridResults(WorkRequestList, Convert.ToInt32(numberOfRows), Convert.ToInt32(pageIndex), Convert.ToInt32(paramTotalRecords.Value));
                    }
                    else
                    {
                        result = BuildJQGridResultsPending(WorkRequestList, Convert.ToInt32(numberOfRows), Convert.ToInt32(pageIndex), Convert.ToInt32(paramTotalRecords.Value));
                    }

                    return result;
                }
                else
                {
                    WorkOrderEMS.Helpers.JQGridResults result = new WorkOrderEMS.Helpers.JQGridResults();
                    List<WorkOrderEMS.Helpers.JQGridRow> rows = new List<WorkOrderEMS.Helpers.JQGridRow>();
                    result.rows = rows.ToArray();
                    result.page = 0;
                    result.total = 0;
                    result.records = 0;
                    return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        [HubMethodName("getWorkOrderCreatedByClientSignalR")]
        public string GetWorkOrderCreatedByClientSignalR(long LocationId, long UserId, string SignalRequestType, string Filter)
        {
            try
            {//GetAllWorkRequestCreatedByClient
                WorkOrderEMS.BusinessLogic.Managers.WorkRequestManager _WorkRequestManager = new WorkOrderEMS.BusinessLogic.Managers.WorkRequestManager();

                string textSearch = "";
                string RequestType = SignalRequestType;
                string filter = Filter;
                long iUserID = UserId;
                long LocationID = LocationId;
                var rows = 100000;
                var page = 1;
                var sidx = "WorkRequestAssignmentID";
                var sord = "desc";
                ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));

                var WorkRequestList = _WorkRequestManager.GetAllWorkRequestCreatedByClient(0, iUserID, RequestType, page, rows, sidx, sord, textSearch, LocationID, iUserID, DateTime.Now, DateTime.Now, filter, paramTotalRecords);

                return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(WorkRequestList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private string BuildJQGridResults(List<WorkOrderEMS.Models.CommonModels.WorkRequestAssignmentModelList> WorkRequestList, int numberOfRows, int pageIndex, int TotalRecords)
        {
            WorkOrderEMS.Helpers.JQGridResults result = new WorkOrderEMS.Helpers.JQGridResults();
            List<WorkOrderEMS.Helpers.JQGridRow> rows = new List<WorkOrderEMS.Helpers.JQGridRow>();
            try
            {
                foreach (var WorkRequest in WorkRequestList)
                {
                    WorkOrderEMS.Helpers.JQGridRow row = new WorkOrderEMS.Helpers.JQGridRow();
                    //row.id = Project.ProjectID;
                    row.id = WorkOrderEMS.Helper.Cryptography.GetEncryptedData(WorkRequest.WorkRequestAssignmentID.ToString(), true);
                    row.cell = new string[26];

                    row.cell[0] = Convert.ToString(WorkRequest.WorkRequestType);
                    row.cell[1] = WorkRequest.WorkRequestTypeName;
                    row.cell[2] = Convert.ToString(WorkRequest.AssetID);
                    row.cell[3] = Convert.ToString(WorkRequest.LocationID);
                    row.cell[4] = WorkRequest.LocationName;
                    row.cell[5] = WorkRequest.ProblemDesc;
                    row.cell[6] = Convert.ToString(WorkRequest.PriorityLevel);
                    row.cell[7] = WorkRequest.PriorityLevelName;
                    row.cell[8] = WorkRequest.WorkRequestImage;
                    row.cell[9] = Convert.ToString(WorkRequest.SafetyHazard);
                    row.cell[10] = WorkRequest.ProjectDesc;
                    row.cell[11] = Convert.ToString(WorkRequest.WorkRequestStatus);
                    row.cell[12] = WorkRequest.WorkRequestStatusName;
                    row.cell[13] = Convert.ToString(WorkRequest.RequestBy);
                    row.cell[14] = Convert.ToString(WorkRequest.AssignToUserId);
                    row.cell[15] = WorkRequest.AssignToUserName;
                    row.cell[16] = Convert.ToString(WorkRequest.AssignByUserId);
                    row.cell[17] = WorkRequest.Remarks;
                    row.cell[18] = Convert.ToString(WorkRequest.WorkRequestProjectType);
                    row.cell[19] = WorkRequest.WorkRequestProjectTypeName;
                    row.cell[20] = WorkRequest.CreationDate;
                    row.cell[21] = WorkRequest.AssignedTime != null ? Convert.ToDateTime(WorkRequest.AssignedTime).ToString("MM/dd/yyyy HH:mm:ss") : null;
                    row.cell[22] = WorkRequest.StartTime != null ? Convert.ToDateTime(WorkRequest.StartTime).ToString("MM/dd/yyyy HH:mm:ss") : null;
                    row.cell[23] = WorkRequest.EndTime != null ? Convert.ToDateTime(WorkRequest.EndTime).ToString("MM/dd/yyyy HH:mm:ss") : null;
                    row.cell[24] = WorkRequest.CodeID;
                    row.cell[25] = WorkRequest.PauseStatus.ToString();
                    rows.Add(row);
                }
                result.rows = rows.ToArray();
                result.page = pageIndex;
                result.total = (int)Math.Ceiling((decimal)TotalRecords / numberOfRows);
                result.records = TotalRecords;
            }
            catch (DivideByZeroException ex)
            {
                string error = ex.Message;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result);
        }

        private string BuildJQGridResultsPending(List<WorkOrderEMS.Models.CommonModels.WorkRequestAssignmentModelList> WorkRequestList, int numberOfRows, int pageIndex, int TotalRecords)
        {
            WorkOrderEMS.Helpers.JQGridResults result = new WorkOrderEMS.Helpers.JQGridResults();
            List<WorkOrderEMS.Helpers.JQGridRow> rows = new List<WorkOrderEMS.Helpers.JQGridRow>();
            try
            {
                foreach (var WorkRequest in WorkRequestList)
                {
                    WorkOrderEMS.Helpers.JQGridRow row = new WorkOrderEMS.Helpers.JQGridRow();
                    //row.id = Project.ProjectID;
                    row.id = WorkOrderEMS.Helper.Cryptography.GetEncryptedData(WorkRequest.WorkRequestAssignmentID.ToString(), true);
                    row.cell = new string[23];

                    row.cell[0] = Convert.ToString(WorkRequest.WorkRequestType);
                    row.cell[1] = WorkRequest.WorkRequestTypeName;
                    row.cell[2] = Convert.ToString(WorkRequest.AssetID);
                    row.cell[3] = Convert.ToString(WorkRequest.LocationID);
                    row.cell[4] = WorkRequest.LocationName;
                    row.cell[5] = WorkRequest.ProblemDesc;
                    row.cell[6] = Convert.ToString(WorkRequest.PriorityLevel);
                    row.cell[7] = WorkRequest.PriorityLevelName;
                    row.cell[8] = WorkRequest.WorkRequestImage;
                    row.cell[9] = Convert.ToString(WorkRequest.SafetyHazard);
                    row.cell[10] = WorkRequest.ProjectDesc;
                    row.cell[11] = Convert.ToString(WorkRequest.WorkRequestStatus);
                    row.cell[12] = WorkRequest.WorkRequestStatusName;
                    row.cell[13] = Convert.ToString(WorkRequest.RequestBy);
                    row.cell[14] = Convert.ToString(WorkRequest.AssignToUserId);
                    row.cell[15] = WorkRequest.AssignToUserName;
                    row.cell[16] = Convert.ToString(WorkRequest.AssignByUserId);
                    row.cell[17] = WorkRequest.Remarks;
                    row.cell[18] = Convert.ToString(WorkRequest.WorkRequestProjectType);
                    row.cell[19] = WorkRequest.WorkRequestProjectTypeName;
                    row.cell[20] = WorkRequest.CreationDate;
                    row.cell[21] = WorkRequest.CodeID;
                    row.cell[22] = WorkRequest.PauseStatus.ToString();
                    rows.Add(row);
                }
                result.rows = rows.ToArray();
                result.page = pageIndex;
                result.total = (int)Math.Ceiling((decimal)TotalRecords / numberOfRows);
                result.records = TotalRecords;
            }
            catch (DivideByZeroException ex)
            {
                string error = ex.Message;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result);
        }

        [HubMethodName("getWorkOrderForDashboardSignalR")]
        public string GetWorkOrderForDashboardSignalR(long LocationId, long UserId, string fromDate, string toDate, long UserType)
        {
            try
            {
                //Getting client date time. 
                var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
                //flag status for if user filter record in time span so to date is till midnight. 
                bool isUTCDay = true;
                DateTime FromDate = (fromDate == null || string.IsNullOrWhiteSpace(fromDate)) ? clientdt.Date : Convert.ToDateTime(fromDate);
                DateTime ToDate = (toDate == null || string.IsNullOrWhiteSpace(toDate)) ? clientdt.AddDays(1).Date : Convert.ToDateTime(toDate);
                ////This condition for if fromdate Todate is same but todate time is upto now.
                //ToDate = (ToDate.Value.Date == DateTime.UtcNow.Date) ? DateTime.UtcNow : ToDate;
                //Newly added code
                if (FromDate != null && ToDate != null)
                {
                    ////if interval date come then need to fetch record till midnight of todate day
                    if ((FromDate.Date != ToDate.Date) && (ToDate.ToLongTimeString() == "12:00:00 AM") && isUTCDay == false)
                    {
                        ToDate = ToDate.AddDays(1).Date;
                    }
                    if ((FromDate.Date == ToDate.Date) && (ToDate.ToLongTimeString() == "12:00:00 AM"))
                    {
                        ToDate = ToDate.AddDays(1).Date;
                    }
                }
                FromDate = FromDate.ConvertClientTZtoUTC();
                ToDate = ToDate.ConvertClientTZtoUTC();
                //Newly added code end here
                WorkOrderEMS.BusinessLogic.Managers.GlobalAdminManager _GlobalAdminManager = new WorkOrderEMS.BusinessLogic.Managers.GlobalAdminManager();
                var dataJson = _GlobalAdminManager.GetDashboardHeadCount(LocationId, UserId, FromDate, ToDate, UserType);
                return dataJson;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}