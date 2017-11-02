using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Globalization;
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

    public class WorkRequestAssignmentList : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        private readonly string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private readonly string WorkRequestImagepath = ConfigurationManager.AppSettings["WorkRequestImage"];
        private readonly string ProfilePicPath = ConfigurationManager.AppSettings["ProfilePicPath"];
        private readonly string ConstantImages = ConfigurationManager.AppSettings["ConstantImages"];
        private readonly string NoImage = ConfigurationManager.AppSettings["DefaultImage"];
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
            string Location = request["LocationId"];
            string filter = (request["filter"] == null || request["filter"] == "Select All") ? "" : request["filter"];
            if (request["UserID"] != "")
            {
                UserID = Convert.ToInt32(request["UserID"]);
                //obj_StaffUserBusiness.Deleteuser(id);
            }
            DateTime StartDate = DateTime.UtcNow;
            DateTime EndDate = DateTime.UtcNow;
            eTracLoginModel ObjLoginModel = null;
            long iUserID = 0;
            long LocationID = 0;

            if (context.Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(context.Session["eTrac"]);

                if (!string.IsNullOrEmpty(Location))
                {
                    LocationID = Convert.ToInt64(Location);
                    //LocationID = 0;
                    iUserID = ObjLoginModel.UserId;
                }
                else
                {
                    iUserID = ObjLoginModel.UserId;
                    LocationID = ObjLoginModel.LocationID;
                }
            }
            else
            {
                return;
            }
            //long LocationID = Convert.ToInt64(context.Session["eTrac_SelectedDasboardLocationID"]);
            ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));

            var WorkRequestList = _GlobalAdminManager.GetAllWorkRequestAssignment(ProjectID, UserID, "GetAllWorkRequestAssignment", pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, LocationID, iUserID, StartDate, EndDate, (filter == "All" ? "" : filter), paramTotalRecords);
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
                    row.cell = new string[43];
                    row.cell[0] = WorkRequest.CodeID;
                    row.cell[1] = Convert.ToString(WorkRequest.WorkRequestType);
                    row.cell[2] = WorkRequest.WorkRequestTypeName;
                    row.cell[3] = Convert.ToString(WorkRequest.AssetID);
                    row.cell[4] = Convert.ToString(WorkRequest.LocationID);
                    row.cell[5] = WorkRequest.LocationName;
                    row.cell[6] = WorkRequest.ProblemDesc;
                    row.cell[7] = Convert.ToString(WorkRequest.PriorityLevel);
                    row.cell[8] = WorkRequest.PriorityLevelName;
                    row.cell[9] = WorkRequest.WorkRequestImage;
                    row.cell[10] = Convert.ToString(WorkRequest.SafetyHazard);
                    row.cell[11] = WorkRequest.ProjectDesc;
                    row.cell[12] = Convert.ToString(WorkRequest.WorkRequestStatus);
                    row.cell[13] = WorkRequest.WorkRequestStatusName;
                    row.cell[14] = Convert.ToString(WorkRequest.RequestBy);
                    row.cell[15] = Convert.ToString(WorkRequest.AssignToUserId);
                    row.cell[16] = WorkRequest.AssignToUserName;
                    row.cell[17] = Convert.ToString(WorkRequest.AssignByUserId);
                    row.cell[18] = WorkRequest.Remarks;
                    row.cell[19] = Convert.ToString(WorkRequest.WorkRequestProjectType);
                    row.cell[20] = WorkRequest.WorkRequestProjectTypeName;
                    //row.cell[21] = WorkRequest.FacilityRequestType == null ? "N/A" : WorkRequest.FacilityRequestType;
                    row.cell[21] = (WorkRequest.FacilityRequestType == null || WorkRequest.FacilityRequestType.TrimWhiteSpace() == "" || WorkRequest.FacilityRequestType.Trim() == "") ? "N/A" : WorkRequest.FacilityRequestType;
                    row.cell[22] = WorkRequest.ProfileImage == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + WorkRequest.ProfileImage;
                    row.cell[23] = WorkRequest.AssignedWorkOrderImage == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-asset-pic.png" : HostingPrefix + WorkRequestImagepath.Replace("~", "") + WorkRequest.AssignedWorkOrderImage;
                    row.cell[24] = WorkRequest.CreationDate; //CreatedDate.ToString("MM/dd/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture);
                    row.cell[25] = WorkRequest.CreatedByProfile == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + WorkRequest.CreatedByProfile;
                    row.cell[26] = WorkRequest.CreatedByUserName;
                    row.cell[27] = WorkRequest.DisclaimerForm;
                    row.cell[28] = WorkRequest.SurveyForm;
                    row.cell[29] = WorkRequest.StartDate;
                    row.cell[30] = WorkRequest.EndDate;
                    row.cell[31] = WorkRequest.WeekDays;
                    row.cell[32] = WorkRequest.StartTime;
                    row.cell[33] = WorkRequest.AssignedTime;
                    row.cell[34] = WorkRequest.CustomerName;
                    row.cell[35] = WorkRequest.VehicleMake;
                    row.cell[36] = WorkRequest.VehicleModel;
                    row.cell[37] = WorkRequest.CustomerContact;
                    row.cell[38] = WorkRequest.VehicleYear.ToString();
                    row.cell[39] = WorkRequest.VehicleColor;
                    row.cell[40] = WorkRequest.DriverLicenseNo;
                    row.cell[41] = WorkRequest.TotalTime;
                    row.cell[42] = WorkRequest.ConStartTime;
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