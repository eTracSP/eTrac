using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.GridListing.JqGridHandler
{
    /// <summary>
    /// Summary description for InventoryList
    /// </summary>
    public class InventoryList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int ProjectID = 0;
            bool pdf = false;
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
                pdf = Convert.ToBoolean(request["pdf"]);
                //obj_StaffUserBusiness.Deleteuser(id);
            }
            int InventoryType = Convert.ToInt32(request["InventoryType"]) == 0 ? 196 : Convert.ToInt32(request["InventoryType"]);
            int ItemOwn = Convert.ToInt32(request["ItemOwn"]);
            ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
            var InventoryList = _ManageManager.GetAllInventory(ProjectID, "GetAllInventory", pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, InventoryType, ItemOwn, paramTotalRecords);
            if (InventoryList.Count() > 0)
            {
                string output = BuildJQGridResults(InventoryList, Convert.ToInt32(numberOfRows), Convert.ToInt32(pageIndex), Convert.ToInt32(paramTotalRecords.Value));

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
        private string BuildJQGridResults(List<WorkOrderEMS.Models.InventoryMasterModelList> InventoryList, int numberOfRows, int pageIndex, int TotalRecords)
        {
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rows = new List<JQGridRow>();
            try
            {
                foreach (var Inventory in InventoryList)
                {
                    JQGridRow row = new JQGridRow();
                    //row.id = Location.LocationId;
                    //row.id = Cryptography.GetEncryptedData(Inventory.InventoryID.ToString(), true);
                    row.id = Cryptography.GetEncryptedData(Inventory.InventoryID.ToString(),true);
                    row.cell = new string[15];
                    row.cell[0] = Inventory.ItemName;
                    row.cell[1] = Inventory.ItemCode;
                    row.cell[2] = Convert.ToString(Inventory.ItemType);
                    row.cell[3] = Inventory.ItemTypeName;
                    row.cell[4] = Inventory.Description;
                    row.cell[5] = Convert.ToString(Inventory.Quantity);
                    row.cell[6] = Convert.ToString(Inventory.AssginedQuantity);
                    row.cell[7] = Convert.ToString(Inventory.LocationId);
                    row.cell[8] = Convert.ToString(Inventory.AssignInventoryID);
                    row.cell[9] = Convert.ToString(Inventory.AssignedUserID);
                    row.cell[10] = Convert.ToString(Inventory.IssueDate);
                    row.cell[11] = Convert.ToString(Inventory.IssuedBy);
                    row.cell[12] = Convert.ToString(Inventory.AssignedToName);
                    row.cell[13] = Convert.ToString(Inventory.ReturnDate);
                    row.cell[14] = Convert.ToString(Inventory.ItemOwnership);

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