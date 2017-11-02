using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;

namespace WorkOrderEMS.GridListing.JqGridHandler
{
    /// <summary>
    /// Summary description for WorkRequestList
    /// </summary>
    public class WorkRequestList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int? ProjectID = 0, UserID =0;
            ManageManager _ManageManager = new ManageManager();
            System.Collections.Specialized.NameValueCollection forms = context.Request.Form;
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;

            string strOperation = forms.Get("oper");
            string _search = request["_search"];
            string textSearch = request["txtSearch"] ?? "";
            int? numberOfRows = Convert.ToInt32(request["rows"]);
            int? pageIndex = Convert.ToInt32(request["page"]);
            string sortColumnName = request["sidx"];
            string sortOrderBy = request["sord"];
            if (request["ProjectID"] != "")
            {
                ProjectID = Convert.ToInt32(request["ProjectID"]);
                //obj_StaffUserBusiness.Deleteuser(id);
            }
            if (request["UserID"] != "")
            {
                UserID = Convert.ToInt32(request["UserID"]);
                //obj_StaffUserBusiness.Deleteuser(id);
            }
            ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
            var WorkRequestList = _ManageManager.GetAllWorkRequest(ProjectID,UserID, "GetAllWorkRequest", pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, paramTotalRecords);
            string output = BuildJQGridResults(WorkRequestList, Convert.ToInt32(numberOfRows), Convert.ToInt32(pageIndex), Convert.ToInt32(paramTotalRecords.Value));
            response.Write(output);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private string BuildJQGridResults(List<WorkOrderEMS.Models.WorkRequestModelList> WorkRequestList, int numberOfRows, int pageIndex, int TotalRecords)
        {
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rows = new List<JQGridRow>();
            try
            {
                foreach (var WorkRequest in WorkRequestList)
                {
                    JQGridRow row = new JQGridRow();
                    //row.id = Project.ProjectID;
                    row.id = Cryptography.GetEncryptedData(WorkRequest.WorkRequestID.ToString(), true);
                    row.cell = new string[18];
                    row.cell[0] = WorkRequest.TaskName;
                   // row.cell[1] = Convert.ToString(WorkRequest.TaskType);
                   // row.cell[2] = WorkRequest.TaskTypeName;
                    row.cell[1] = Convert.ToString(WorkRequest.TaskPriority);
                    row.cell[2] = WorkRequest.TaskPriorityName;
                    row.cell[3] = Convert.ToString(WorkRequest.RequestBy);
                    row.cell[4] = WorkRequest.RequestByName;
                    row.cell[5] = Convert.ToString(WorkRequest.WorkArea);
                    row.cell[6] = WorkRequest.AreaName;
                    row.cell[7] = WorkRequest.StartTime;
                    row.cell[8] = WorkRequest.CompletionTime;
                    row.cell[9] = Convert.ToString(WorkRequest.AssignedToUser);
                    row.cell[10] = WorkRequest.AssignedToUserName;
                    row.cell[11] = Convert.ToString(WorkRequest.TaskStatus);
                    row.cell[12] = Convert.ToString(WorkRequest.ProjectId);               
                    row.cell[13] = WorkRequest.Remarks;
                    row.cell[14] = WorkRequest.TaskStatusName;
                    row.cell[15] = Convert.ToString(WorkRequest.WorkOrderID);
                    row.cell[16] = Convert.ToString(WorkRequest.AssetID);
                    row.cell[17] = WorkRequest.AssetNo;
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
            return new JavaScriptSerializer().Serialize(result);
        }

    }
}