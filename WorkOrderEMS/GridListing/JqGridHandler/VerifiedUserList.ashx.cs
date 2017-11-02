using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Helpers;

namespace WorkOrderEMS.GridListing.JqGridHandler
{
    /// <summary>
    /// Summary description for VerifiedUserList
    /// </summary>
    public class VerifiedUserList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int userid = 0;
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
            if (Convert.ToInt32(request["id"]) != 0)
            {
                long? id = Convert.ToInt32(request["id"]);
                //obj_StaffUserBusiness.Deleteuser(id);
            }
            ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
            var EmployeeList = _ManageManager.GetAllVerfiedEmployee(userid, "GetAllVerfiedEmployee", pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, paramTotalRecords);
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
        private string BuildJQGridResults(List<WorkOrderEMS.Models.UserModelList> EmployeeList, int numberOfRows, int pageIndex, int TotalRecords)
        {
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rows = new List<JQGridRow>();
            try
            {
                foreach (var Employee in EmployeeList)
                {
                    JQGridRow row = new JQGridRow();
                    //row.id = Project.ProjectID;
                    row.id = Employee.UserId;
                    row.cell = new string[6];
                    row.cell[0] = Employee.Name;
                    row.cell[1] = Employee.UserEmail;
                    row.cell[2] = Employee.EmployeeProfile;
                    row.cell[3] = Employee.HiringDate != null ? Convert.ToDateTime(Employee.HiringDate).ToShortDateString() : "";
                    row.cell[4] = Convert.ToString(Employee.UserType);
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