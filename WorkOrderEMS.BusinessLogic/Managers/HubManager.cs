using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Script.Serialization;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class HubManager
    {
        // Database connection strings
        private string connString = System.Configuration.ConfigurationManager.AppSettings["SQLConnection"].ToString();

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created On: 13/04/2016
        ///  Utility Method -- Perform the no-parameter data query and return json
        /// </summary>
        public string GetDataFromQuery(string sqltext, long UserID, long LocationID, DateTime? fromDate, DateTime? toDate, long userType)
        {
            string json = "";
            // A database query is present, get data from database
            try
            {
                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connString))
                {
                    try
                    {
                        using (System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            da.SelectCommand = new System.Data.SqlClient.SqlCommand(sqltext, conn);
                            da.SelectCommand.CommandType = CommandType.StoredProcedure;
                            da.SelectCommand.CommandTimeout = 120;
                            // Set Input Parameter
                            System.Data.SqlClient.SqlParameter oParam = new System.Data.SqlClient.SqlParameter("@UserId", UserID);
                            oParam.SqlDbType = SqlDbType.VarChar;
                            da.SelectCommand.Parameters.Add(oParam);
                            System.Data.SqlClient.SqlParameter oParam1 = new System.Data.SqlClient.SqlParameter("@LocationId", LocationID);
                            oParam1.SqlDbType = SqlDbType.VarChar;
                            da.SelectCommand.Parameters.Add(oParam1);
                            System.Data.SqlClient.SqlParameter oParam2 = new System.Data.SqlClient.SqlParameter("@FromDate", fromDate);
                            oParam2.SqlDbType = SqlDbType.VarChar;
                            da.SelectCommand.Parameters.Add(oParam2);
                            System.Data.SqlClient.SqlParameter oParam3 = new System.Data.SqlClient.SqlParameter("@ToDate", toDate);
                            oParam3.SqlDbType = SqlDbType.VarChar;
                            da.SelectCommand.Parameters.Add(oParam3);
                            System.Data.SqlClient.SqlParameter oParam4 = new System.Data.SqlClient.SqlParameter("@WebDateTime", DateTime.UtcNow);
                            oParam3.SqlDbType = SqlDbType.VarChar;
                            da.SelectCommand.Parameters.Add(oParam4);

                            if (userType == (long)UserType.Administrator || userType == (long)UserType.Manager)
                            {
                                DataSet ds = new DataSet();
                                da.Fill(ds);
                                //For Adding encrypted id for assign on dashboard
                                var table4 = DataTableConversion(ds.Tables[4]);
                                ds.Tables.Remove(ds.Tables[4]);
                                ds.Tables.Add(table4);

                                //For Adding encrypted id for assign on dashboard(Client Created)
                                var table5 = DataTableConversion(ds.Tables[4]);
                                ds.Tables.Remove(ds.Tables[4]);
                                ds.Tables.Add(table5);

                                ds.Tables[0].TableName = "CountPanel";
                                ds.Tables[1].TableName = "WorkStatus";
                                ds.Tables[2].TableName = "WorkProjectType";
                                ds.Tables[3].TableName = "DarDetail";
                                ds.Tables[4].TableName = "MobActiveUser";
                                ds.Tables[5].TableName = "WebActiveUser";
                                ds.Tables[6].TableName = "AllActiveUser";
                                ds.Tables[7].TableName = "Progress";
                                ds.Tables[8].TableName = "Pending";
                                ds.Tables[9].TableName = "Completed";
                                ds.Tables[10].TableName = "UnAssignedWO";
                                ds.Tables[11].TableName = "UnAssignedWOClientCreated";

                                json = JsonConvert.SerializeObject(ds);
                            }
                            else
                            {
                                DataSet ds = new DataSet();
                                da.Fill(ds);
                                //For Adding encrypted id for assign on dashboard
                                var table4 = DataTableConversion(ds.Tables[4]);
                                ds.Tables.Remove(ds.Tables[4]);
                                ds.Tables.Add(table4);
                                ds.Tables[0].TableName = "CountPanel";
                                ds.Tables[1].TableName = "WorkStatus";
                                ds.Tables[2].TableName = "WorkProjectType";
                                ds.Tables[3].TableName = "DarDetail";
                                ds.Tables[4].TableName = "MobActiveUser";
                                ds.Tables[5].TableName = "WebActiveUser";
                                ds.Tables[6].TableName = "AllActiveUser";
                                ds.Tables[7].TableName = "Progress";
                                ds.Tables[8].TableName = "Pending";
                                ds.Tables[9].TableName = "Completed";
                                ds.Tables[10].TableName = "UnAssignedWO";

                                json = JsonConvert.SerializeObject(ds);
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        Exception_B.Exception_B.exceptionHandel_Runtime(e, "string GetDataFromQuery(string sqltext, long UserID, long LocationID, DateTime? fromDate, DateTime? toDate)", "Exception while fetching record for dashboard ", sqltext);
                        Console.WriteLine("Error: " + e.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                json = ErrorStringToJSON(ex.ToString());
            }

            return json;
        }

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created On: 27/04/2016
        /// This method have extra one input parameter for progress bar calculation.
        /// </summary>
        public string GetDataFromQueryForWO(string sqltext, long UserID, long LocationID, DateTime? fromDate, DateTime? toDate)
        {
            string json = "";
            // A database query is present, get data from database
            try
            {
                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connString))
                {
                    try
                    {
                        using (System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            da.SelectCommand = new System.Data.SqlClient.SqlCommand(sqltext, conn);
                            da.SelectCommand.CommandType = CommandType.StoredProcedure;

                            // Set Input Parameter
                            System.Data.SqlClient.SqlParameter oParam = new System.Data.SqlClient.SqlParameter("@UserId", UserID);
                            oParam.SqlDbType = SqlDbType.VarChar;
                            da.SelectCommand.Parameters.Add(oParam);
                            System.Data.SqlClient.SqlParameter oParam1 = new System.Data.SqlClient.SqlParameter("@LocationId", LocationID);
                            oParam1.SqlDbType = SqlDbType.VarChar;
                            da.SelectCommand.Parameters.Add(oParam1);
                            System.Data.SqlClient.SqlParameter oParam2 = new System.Data.SqlClient.SqlParameter("@FromDate", fromDate);
                            oParam2.SqlDbType = SqlDbType.VarChar;
                            da.SelectCommand.Parameters.Add(oParam2);
                            System.Data.SqlClient.SqlParameter oParam3 = new System.Data.SqlClient.SqlParameter("@ToDate", toDate);
                            oParam3.SqlDbType = SqlDbType.VarChar;
                            da.SelectCommand.Parameters.Add(oParam3);
                            System.Data.SqlClient.SqlParameter oParam4 = new System.Data.SqlClient.SqlParameter("@WebDateTime", DateTime.UtcNow);
                            oParam3.SqlDbType = SqlDbType.VarChar;
                            da.SelectCommand.Parameters.Add(oParam4);

                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            //For Adding encrypted id for assign on dashboard
                            var table3 = DataTableConversion(ds.Tables[3]);
                            ds.Tables.Remove(ds.Tables[3]);
                            ds.Tables.Add(table3);
                            ds.Tables[0].TableName = "Progress";
                            ds.Tables[1].TableName = "Pending";
                            ds.Tables[2].TableName = "Completed";
                            ds.Tables[3].TableName = "UnAssignedWO";
                            json = JsonConvert.SerializeObject(ds);
                        }
                    }
                    catch (Exception e)
                    {
                        Exception_B.Exception_B.exceptionHandel_Runtime(e, "string GetDataFromQueryForWO(string sqltext, long UserID, long LocationID, DateTime? fromDate, DateTime? toDate)", "Exception while fetching record for dashboard ", sqltext);
                        Console.WriteLine("Error: " + e.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                json = ErrorStringToJSON(ex.ToString());
            }

            return json;
        }

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created On: 13/04/2016
        /// For Adding encrypted id for assign on dashboard
        /// </summary>
        public DataTable DataTableConversion(DataTable table)
        {
            try
            {
                table.Columns.Add("ID", typeof(string));
                foreach (DataRow row in table.Rows)
                {
                    row["ID"] = Cryptography.GetEncryptedData(row["WorkRequestAssignmentID"].ToString(), true);
                }
                return table;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public DataTable DataTableConversion(DataTable table)", "Adding encrypted id for assign on dashboard ", table);
                return null;
            }
        }
        ///// <summary>
        ///// Created By: Bhushan Dod
        ///// Created On: 13/04/2016
        ///// Utility method -- Convert a DataTable to JSON
        ///// </summary>
        public static string DataTableToJSON(DataTable table)
        {
            try
            {
                var list = new List<Dictionary<string, object>>();

                foreach (DataRow row in table.Rows)
                {
                    var dict = new Dictionary<string, object>();

                    foreach (DataColumn col in table.Columns)
                    {
                        dict[col.ColumnName] = row[col];
                    }
                    list.Add(dict);
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(list);

            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "  public static string DataTableToJSON(DataTable table)", "Exception while converting table to JSON ", table);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(ErrorStringToJSON(ex.ToString()));
            }
        }

        //public string ConvertToJSON(DataSet ds)
        //{
        //    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        //    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        //    Dictionary<string, object> row = null;
        //    for (var i = 0; i < ds.Tables.Count; i++)
        //    {
        //        foreach (DataRow dr in ds.Tables[i].Rows)
        //        {
        //            row = new Dictionary<string, object>();
        //            foreach (DataColumn col in ds.Tables[i].Columns)
        //            {
        //                row.Add(col.ColumnName.Trim(), dr[col]);
        //            }
        //            rows.Add(row);
        //        }

        //        string JSONString = string.Empty;
        //        JSONString = JsonConvert.SerializeObject(ds);   

        //    }


        //    return serializer.Serialize(rows);
        //} 

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created On: 13/04/2016
        /// Convert an input string into a JSON string
        /// </summary>
        public static string ErrorStringToJSON(string inVal)
        {
            DataTable dtResults = new DataTable();
            string json = "";

            DataTable table = new DataTable();
            DataRow newRow = table.NewRow();

            DataColumn ErrorMsgCol = new DataColumn("ErrorMsg");
            table.Columns.Add(ErrorMsgCol);

            newRow["ErrorMsg"] = inVal;
            table.Rows.Add(newRow);

            json = DataTableToJSON(table);

            return (json);
        }
    }
}
