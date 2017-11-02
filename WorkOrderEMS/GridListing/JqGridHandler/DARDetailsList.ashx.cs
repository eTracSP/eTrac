using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using WorkOrderEMS.BusinessLogic.Exception_B;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;


namespace WorkOrderEMS.GridListing.JqGridHandler
{
    /// <summary>
    /// Summary description for RuleMasterList
    /// </summary>
    public class DARDetailsList : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        private readonly string DarImagePath = ConfigurationManager.AppSettings["DARImage"];
     
        private readonly string ConstantImagePath = ConfigurationManager.AppSettings["ConstantImages"];
        private readonly string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            try
            {
                DARManager objDARDetailsList = new DARManager();
                eTracLoginModel ObjLoginModel = null;
                System.Collections.Specialized.NameValueCollection forms = context.Request.Form;


                long? locationId = null;
                long? userId = null;
                long loginUserId = 0;             
               
                if (Convert.ToInt64(request["empId"]) != 0)
                {
                    userId = Convert.ToInt64(request["empId"]);
                }
                else
                {
                    userId = null;
                }

                int? taskType = null;
                if (Convert.ToInt32(request["taskType"]) != 0)
                {
                    taskType = Convert.ToInt32(request["taskType"]);
                }
                else
                {
                    taskType = null;
                }

                //(Convert.ToInt64(request["empId"])!=0) ? Convert.ToInt64(request["empId"]) : null;
                string _search = request["_search"];
                string textSearch = request["txtSearch"] ?? "";
                int numberOfRows = Convert.ToInt32(request["rows"]);
                int pageIndex = Convert.ToInt32(request["page"]);
                string sortColumnName = request["sidx"];
                string sortOrderBy = request["sord"];

                //Getting client date time. 
                var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
                //flag status for if user filter record in time span so to date is till midnight. 
                bool isUTCDay = true;
                //Fetching record like 2017-06-11T00:00:00-04:00 to 2017-06-12T00:0000-04:00
                string fromDate = (request["fromDate"] == null || request["fromDate"] == " "|| request["fromDate"] == "") ? clientdt.Date.ToString(): request["fromDate"];
                string toDate = (request["toDate"] == null || request["toDate"] == " " || request["toDate"] == "") ? clientdt.AddDays(1).Date.ToString(): request["toDate"];

                //maintaining flag  if interval date come then need to fetch record till midnight of todate day
                if (request["toDate"] != null && request["toDate"] != "" && request["fromDate"] != "null")
                {
                    DateTime tt = Convert.ToDateTime(toDate);
                    if (tt.ToLongTimeString() == "12:00:00 AM")
                        isUTCDay = false;
                }
                    
                if (fromDate != null && toDate != null)
                {
                    DateTime frmd = Convert.ToDateTime(fromDate);
                    DateTime tod = Convert.ToDateTime(toDate);
                    ////if interval date come then need to fetch record till midnight of todate day
                    if ((frmd.Date != tod.Date) && (tod.ToLongTimeString() == "12:00:00 AM") && isUTCDay == false)
                    {
                        tod = tod.AddDays(1).Date;
                        toDate = tod.ToString();
                    }
                    if ((frmd.Date == tod.Date) && (tod.ToLongTimeString() == "12:00:00 AM"))
                    {
                        tod = tod.AddDays(1).Date;
                        toDate = tod.ToString();
                    }                    
                }
                if (context.Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(context.Session["eTrac"]);
                    locationId = ObjLoginModel.LocationID;
                    loginUserId = ObjLoginModel.UserId; //Added by Bhushan due to useid is null in Sp. It will not return created on due joinon the basis of userid.
                }
                //Converting datetime from userTZ to UTC
                fromDate = Convert.ToDateTime(fromDate).ConvertClientTZtoUTC().ToString(); 
                toDate = Convert.ToDateTime(toDate).ConvertClientTZtoUTC().ToString(); 

                ObjectParameter totalRecords = new ObjectParameter("TotalRecords", typeof(int));
                var darDetailsList = objDARDetailsList.GetDARDetails(loginUserId,locationId, userId, taskType, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, totalRecords, fromDate, toDate);
                if (darDetailsList.Count() > 0)
                {
                    string output = BuildJQGridResults(darDetailsList, numberOfRows, pageIndex, Convert.ToInt32(totalRecords.Value));
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
            catch (Exception ex)
            {
                Exception_B.exceptionHandel_Runtime(ex, "public void ProcessRequest(HttpContext context)-DARDetails.ashx", "context", context.ToString());
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
        private string BuildJQGridResults(List<DARModelList> darDetailsList, int numberOfRows, int pageIndex, int totalRecords)
        {
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rows = new List<JQGridRow>();
            try
            {
                foreach (var itemDAR in darDetailsList)
                {
                    JQGridRow row = new JQGridRow();
                    row.id = Cryptography.GetEncryptedData(itemDAR.DARId.ToString(), true);
                    row.cell = new string[7];
                    //row.cell[0] = itemDAR.LocationName;
                    row.cell[0] = itemDAR.EmployeeName;
                    row.cell[1] = itemDAR.ActivityDetails;
                    row.cell[2] = itemDAR.TaskType;
                    row.cell[3] = itemDAR.StartTime;
                    row.cell[4] = itemDAR.EndTime;                
                    row.cell[5] = itemDAR.CreatedOn;
                    row.cell[6] = itemDAR.DisclaimerFormFile;
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