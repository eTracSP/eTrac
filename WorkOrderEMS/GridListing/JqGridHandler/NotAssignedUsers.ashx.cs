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
    /// Summary description for VerifiedUserList
    /// </summary>
    public class NotAssignedUsers : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        private readonly string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private readonly string WorkRequestImagepath = ConfigurationManager.AppSettings["WorkRequestImage"];
        private readonly string ProfilePicPath = ConfigurationManager.AppSettings["ProfilePicPath"];
        private readonly string ConstantImages = ConfigurationManager.AppSettings["ConstantImages"];
        private readonly string NoImage = ConfigurationManager.AppSettings["DefaultImage"];
        public void ProcessRequest(HttpContext context)
        {
            UserManager _UserManager = new UserManager();
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
            string UserType = request["UserType"];
            long iUserID = 0;
            eTracLoginModel ObjLoginModel = null;
            if (context.Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(context.Session["eTrac"]);
                iUserID = ObjLoginModel.UserId;
            }
            long UserID = 0;
            if (request["UserID"] != "")
            {
                UserID = Convert.ToInt32(request["UserID"]);
                //obj_StaffUserBusiness.Deleteuser(id);
            }
            ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
            var EmployeeList = _UserManager.GetNotAssignedUsers(iUserID, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, UserType.Replace(",", ""), paramTotalRecords);
            if (EmployeeList.Count() > 0)
            {
                string output = BuildJQGridResults(EmployeeList, Convert.ToInt32(numberOfRows), Convert.ToInt32(pageIndex), Convert.ToInt32(paramTotalRecords.Value));
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
        private string BuildJQGridResults(List<WorkOrderEMS.Models.UserModels.NotAssignedUserModel> EmployeeList, int numberOfRows, int pageIndex, int TotalRecords)
        {
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rows = new List<JQGridRow>();
            try
            {
                foreach (var Employee in EmployeeList)
                {
                    JQGridRow row = new JQGridRow();
                    //row.id = Project.ProjectID;
                    row.id = row.id = Cryptography.GetEncryptedData(Employee.UserId.ToString(), true);
                    row.cell = new string[6];
                    row.cell[0] = Employee.Name;
                    row.cell[1] = Employee.UserEmail;
                    row.cell[2] = Employee.CodeName;
                    row.cell[3] = Employee.DOB != null ? Convert.ToDateTime(Employee.DOB).ToShortDateString() : "";
                    row.cell[4] = Employee.ProfileImage == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + Employee.ProfileImage;
                    row.cell[5] = Employee.IsLoginActive == true ? "Active" : "Un-Active";
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