using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;

namespace WorkOrderEMS.GridListing.JqGridHandler
{
    /// <summary>
    /// Summary description for ProjectList
    /// </summary>
    public class ProjectList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int ProjectID = 0;
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
            var ProjectList = _GlobalAdminManager.GetAllProject(ProjectID, "GetAllProject", pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, paramTotalRecords);
            string output = BuildJQGridResults(ProjectList, Convert.ToInt32(numberOfRows), Convert.ToInt32(pageIndex), Convert.ToInt32(paramTotalRecords.Value));
            response.Write(output);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private string BuildJQGridResults(List<WorkOrderEMS.Models.ProjectMasterListModel> ProjectList, int numberOfRows, int pageIndex, int TotalRecords)
        {
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rows = new List<JQGridRow>();
            try
            {
                foreach (var Project in ProjectList)
                {
                    JQGridRow row = new JQGridRow();
                    //row.id = Project.ProjectID;
                    row.id = Cryptography.GetEncryptedData(Project.ProjectID.ToString(), true);
                    row.cell = new string[10];
                    row.cell[0] = Project.Location;
                    row.cell[1] = Project.LocationName;
                    row.cell[2] = Project.ProjectCategoryName;
                    row.cell[3] = Project.ProjectServiceName;
                    row.cell[4] = Project.Description;
                    row.cell[5] = Convert.ToString(Project.LocationID);
                    row.cell[6] = Convert.ToString(Project.ProjectCategory);
                    row.cell[7] = Convert.ToString(Project.ProjectServicesID);
                    row.cell[8] = Project.ProjectLogoName;
                    row.cell[9] = Convert.ToString(Project.QRCID);

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