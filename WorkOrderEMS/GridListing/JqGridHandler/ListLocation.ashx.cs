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
    /// Summary description for ListLocation
    /// </summary>
    public class ListLocation : IHttpHandler
    {

        [System.Web.Mvc.Authorize]
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int LocationID = 0;
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
                var ListLocation = _GlobalAdminManager.ListAllLocation(LocationID, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, paramTotalRecords);
                if (ListLocation.Count() > 0)
                {
                    string output = BuildJQGridResults(ListLocation, Convert.ToInt32(numberOfRows), Convert.ToInt32(pageIndex), Convert.ToInt32(paramTotalRecords.Value));
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
            catch (Exception ex) { throw ex; }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private string BuildJQGridResults(List<WorkOrderEMS.Models.ListLocationModel> ListLocation, int numberOfRows, int pageIndex, int TotalRecords)
        {
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rows = new List<JQGridRow>();
            try
            {
                foreach (var locList in ListLocation)
                {
                    JQGridRow row = new JQGridRow();
                    row.id = Cryptography.GetEncryptedData(locList.LocationId.ToString(), true);
                    row.cell = new string[11];
                    row.cell[0] = locList.LocationName;
                    row.cell[1] = locList.Address + "," + locList.City + ", " + locList.State + ", " + locList.ZipCode + "," + locList.Country;
                    row.cell[2] = locList.LocationAdministrator;
                    row.cell[3] = locList.LocationManager;
                    row.cell[4] = locList.LocationEmployee;
                    //row.cell[4] = locList.LocationClient;
                    row.cell[5] = locList.City;
                    row.cell[6] = locList.State;
                    row.cell[7] = locList.Country;
                    row.cell[8] = locList.PhoneNo + " / " + locList.Mobile;
                    row.cell[9] = locList.Description;
                    row.cell[10] = Convert.ToString(locList.QRCID);
                    rows.Add(row);
                }
                result.rows = rows.ToArray(); result.page = pageIndex;
                result.total = (int)Math.Ceiling((decimal)TotalRecords / numberOfRows);
                result.records = TotalRecords;
            }
            catch (DivideByZeroException ex) { string error = ex.Message; }
            catch (Exception ex) { string error = ex.Message; }
            return new JavaScriptSerializer().Serialize(result);
        }
    }
}