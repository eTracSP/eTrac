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
    /// Summary description for RuleMasterList
    /// </summary>
    public class RuleMasterList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            long? ProjectID = 0;
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
            if (Convert.ToInt32(request["ProjectID"]) != 0)
            {
                ProjectID = Convert.ToInt32(request["ProjectID"]);
                //obj_StaffUserBusiness.Deleteuser(id);
            }
            ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
            var RuleList = _ManageManager.GetAllRules(ProjectID, "GetAllRule", pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, paramTotalRecords);
            if (RuleList.Count() > 0)
            {
                string output = BuildJQGridResults(RuleList, Convert.ToInt32(numberOfRows), Convert.ToInt32(pageIndex), Convert.ToInt32(paramTotalRecords.Value));
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
        private string BuildJQGridResults(List<WorkOrderEMS.Models.RuleMasterModelList> RuleList, int numberOfRows, int pageIndex, int TotalRecords)
        {
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rows = new List<JQGridRow>();
            try
            {
                foreach (var Rule in RuleList)
                {
                    JQGridRow row = new JQGridRow();
                    //row.id = Project.ProjectID;
                    row.id = Cryptography.GetEncryptedData(Rule.RuleID.ToString(),true);
                    row.cell = new string[6];
                    row.cell[0] = Rule.RuleName;
                    row.cell[1] = Convert.ToString(Rule.VoilationCharges);
                    row.cell[2] = Rule.Description;
                    row.cell[3] = Rule.IsActive == true ? "Active" : "Inactive";

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