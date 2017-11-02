using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;

namespace WorkOrderEMS.GridListing.JqGridHandler
{
    /// <summary>
    /// Summary description for RuleMasterList
    /// </summary>
    public class EmailList : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            long? emailLogId = 0;
            //  long? LocationId=0 ;
            EmailDetailManager objEmailDetailManager = new EmailDetailManager();
            eTracLoginModel ObjLoginModel = null;
            System.Collections.Specialized.NameValueCollection forms = context.Request.Form;
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;

            string _search = request["_search"];
            string textSearch = request["txtSearch"] ?? "";
            int? numberOfRows = Convert.ToInt32(request["rows"]);
            int? pageIndex = Convert.ToInt32(request["page"]);
            string sortColumnName = request["sidx"];
            string sortOrderBy = request["sord"];
            long? LocationId = Convert.ToInt32(request["locationId"]);

            if (context.Session["eTrac"] != null && request["locationId"] == null)
            {
                ObjLoginModel = (eTracLoginModel)(context.Session["eTrac"]);
                LocationId = ObjLoginModel.LocationID;
            }

            ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));

            var emailList = objEmailDetailManager.GetAllEmailList(emailLogId, LocationId, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, paramTotalRecords);
            if (emailList.Count() > 0)
            {
                string output = BuildJQGridResults(emailList, Convert.ToInt32(numberOfRows), Convert.ToInt32(pageIndex), Convert.ToInt32(paramTotalRecords.Value));
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
        private string BuildJQGridResults(List<WorkOrderEMS.Models.EmailLogModel> emailList, int numberOfRows, int pageIndex, int totalRecords)
        {
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rows = new List<JQGridRow>();
            try
            {
                foreach (var emailDetails in emailList)
                {
                    JQGridRow row = new JQGridRow();

                    row.id = Cryptography.GetEncryptedData(emailDetails.EmailLogId.ToString(),true);
                    row.cell = new string[4];
                    row.cell[0] = emailDetails.SentByUser;
                    row.cell[1] = emailDetails.SentTo;
                    row.cell[2] = emailDetails.Subject;
                    row.cell[3] = emailDetails.CreatedDate;
                    rows.Add(row);
                }
                result.rows = rows.ToArray();
                result.page = pageIndex;
                result.total = (int)Math.Ceiling((decimal)totalRecords / numberOfRows);
                result.records = totalRecords;
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