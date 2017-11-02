using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Helpers;

namespace WorkOrderEMS.GridListing.JqGridHandler
{
    /// <summary>
    /// Summary description for VerifiedMnagaerList
    /// </summary>
    public class VerifiedMnagaerList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int userid = 0;
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
            if (Convert.ToInt32(request["id"]) != 0)
            {
                long? id = Convert.ToInt32(request["id"]);
                //obj_StaffUserBusiness.Deleteuser(id);
            }
            ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
            var ManagaerList = _GlobalAdminManager.GetAllVerifiedManager(userid, "GetAllVerfiedManager", pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, paramTotalRecords);
            if (ManagaerList.Count() > 0)
            {
                string output = BuildJQGridResults(ManagaerList, Convert.ToInt32(numberOfRows), Convert.ToInt32(pageIndex), Convert.ToInt32(paramTotalRecords.Value));
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
        private string BuildJQGridResults(List<WorkOrderEMS.Models.UserModelList> ManagaerList, int numberOfRows, int pageIndex, int TotalRecords)
        {
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rows = new List<JQGridRow>();
            try
            {
                foreach (var Manager in ManagaerList)
                {
                    JQGridRow row = new JQGridRow();
                    //row.id = Project.ProjectID;
                    row.id = Manager.UserId;
                    row.cell = new string[8];
                    row.cell[0] = Manager.Name;
                    row.cell[1] = Manager.UserEmail;
                    row.cell[2] = Convert.ToString(Manager.DOB);
                    row.cell[3] = Convert.ToString(Manager.UserId);
                    row.cell[4] = Manager.Name;
                    row.cell[5] = Manager.ProfileImage;
                    row.cell[6] = Manager.UserType;
                    row.cell[7] = Manager.HiringDate != null ? Convert.ToDateTime(Manager.HiringDate).ToShortDateString() : "";
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