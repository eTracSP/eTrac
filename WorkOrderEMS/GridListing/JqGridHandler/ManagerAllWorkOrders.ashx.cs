using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;
namespace WorkOrderEMS.GridListing.JqGridHandler
{
    /// <summary>
    /// Summary description for WorkRequestAssignmentList
    /// </summary>
    public class ManagerAllWorkOrders : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            int? ProjectID = 0, UserID = 0;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
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
            string filter = request["filter"];
            DateTime StartDate = DateTime.UtcNow;
            DateTime EndDate = DateTime.UtcNow;
            if (request["UserID"] != "")
            {
                UserID = Convert.ToInt32(request["UserID"]);
                //obj_StaffUserBusiness.Deleteuser(id);
            }
            //if (request["StartDate"] != "" || request["StartDate"] != null)
            //{
            //    StartDate = Convert.ToDateTime(request["StartDate"]);
            //    //obj_StaffUserBusiness.Deleteuser(id);
            //}
            //if (request["EndDate"] !="" || request["EndDate"] != null)
            //{
            //    EndDate = Convert.ToDateTime(request["EndDate"]);
            //    //obj_StaffUserBusiness.Deleteuser(id);
            //}

            eTracLoginModel ObjLoginModel = null;
            long iUserID = 0;
            if (context.Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(context.Session["eTrac"]);
                iUserID = ObjLoginModel.UserId;
            }

            long LocationID = Convert.ToInt64(context.Session["eTrac_SelectedDasboardLocationID"]);
            ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
            var WorkRequestList = _GlobalAdminManager.GetAllWorkRequestAssignment(ProjectID, UserID, "GetAllWorkRequestAssignmentByManager", pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, LocationID, iUserID, StartDate, EndDate, filter, paramTotalRecords);
            if (WorkRequestList.Count() > 0)
            {
                string output = BuildJQGridResults(WorkRequestList, Convert.ToInt32(numberOfRows), Convert.ToInt32(pageIndex), Convert.ToInt32(paramTotalRecords.Value));
                response.Write(output);
            }
            else
            {
                JQGridResults result = new JQGridResults();
                List<JQGridRow> rows = new List<JQGridRow>();
                result.rows = rows.ToArray();
                result.page = 0;
                result.total = 0;
                result.records = 0;
                response.Write(new JavaScriptSerializer().Serialize(result));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private string BuildJQGridResults(List<WorkOrderEMS.Models.CommonModels.WorkRequestAssignmentModelList> WorkRequestList, int numberOfRows, int pageIndex, int TotalRecords)
        {
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rows = new List<JQGridRow>();
            try
            {
                foreach (var WorkRequest in WorkRequestList)
                {
                    JQGridRow row = new JQGridRow();
                    //row.id = Project.ProjectID;
                    row.id = Cryptography.GetEncryptedData(WorkRequest.WorkRequestAssignmentID.ToString(), true);
                    row.cell = new string[25];

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
                    row.cell[20] = WorkRequest.CodeID;
                    row.cell[21] = WorkRequest.CreationDate;
                    //row.cell[22] = WorkRequest.AssignedTime;
                    //row.cell[23] = WorkRequest.StartTime;
                    //row.cell[24] = WorkRequest.EndTime;
                    row.cell[22] = WorkRequest.AssignedTime != null ? Convert.ToDateTime(WorkRequest.AssignedTime).ToString("MM/dd/yyyy HH:mm:ss") : null;
                    row.cell[23] = WorkRequest.StartTime != null ? Convert.ToDateTime(WorkRequest.StartTime).ToString("MM/dd/yyyy HH:mm:ss") : null;
                    row.cell[24] = WorkRequest.EndTime != null ? Convert.ToDateTime(WorkRequest.EndTime).ToString("MM/dd/yyyy HH:mm:ss") : null;

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