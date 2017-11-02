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
    /// Summary description for LocationList
    /// </summary>
    public class LocationList : IHttpHandler
    {

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
                var LocationList = _GlobalAdminManager.GetAllLocationList(LocationID, "GetAllLocation", pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, paramTotalRecords);
                if (LocationList.Count() > 0)
                {
                    string output = BuildJQGridResults(LocationList, Convert.ToInt32(numberOfRows), Convert.ToInt32(pageIndex), Convert.ToInt32(paramTotalRecords.Value));
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

        private string BuildJQGridResults(List<WorkOrderEMS.Models.LocationListModel> LocationList, int numberOfRows, int pageIndex, int TotalRecords)
        {
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rows = new List<JQGridRow>();
            try
            {
                foreach (var Location in LocationList)
                {
                    JQGridRow row = new JQGridRow();
                    //row.id = Location.LocationId;
                    row.id = Cryptography.GetEncryptedData(Location.LocationId.ToString(), true);
                    row.cell = new string[8];
                    row.cell[0] = Location.LocationName;
                    row.cell[1] = Location.Address1 + ' ' + Location.Address2 + Location.ZipCode;
                    row.cell[2] = Location.City;
                    row.cell[3] = Location.StateName;
                    row.cell[4] = Location.CountryName;
                    row.cell[5] = Location.PhoneNo + " / " + Location.Mobile;
                    row.cell[6] = Location.Description;
                    row.cell[7] = Convert.ToString(Location.QRCID);
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