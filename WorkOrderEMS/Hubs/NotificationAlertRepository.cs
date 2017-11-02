using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Models;

namespace WorkOrderEMS
{
    /// <summary>
    /// Created By : Bhushan Dod 
    /// Created Date : 02/11/2015
    /// Description : For work order ,infraction notification alert.
    /// this class created here due to SignalR is independent from EF.
    /// </summary>
    public class NotificationAlertRepository
    {
        readonly string _connString = ConfigurationManager.AppSettings["SQLConnection"].ToString();
        string date = DateTime.UtcNow.Date.AddDays(-14).ToString("yyyy-MM-dd");

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                Hubs.NotificationAlertHub.SendMessages();
            }
        }

        /// <summary>
        /// Created By : Bhushan Dod on 18/01/2015
        /// Description : This function used for signalR persistent connection alert on screen if new WO created or any chnage in table for dashboard progress,pending.
        /// </summary>
        /// <param name="IsGlobalAsax"></param>
        /// <returns></returns>
        public async Task<WorkOrderEMS.Hubs.NotificationModel.WorkRequestAssignment> WorkOrderDetailsForPushNotificationSignalR(bool IsGlobalAsax = false)
        {
            try
            {
                var returnData = new WorkOrderEMS.Hubs.NotificationModel.WorkRequestAssignment();
                GlobalAdminManager objGlobalAdminManager = new GlobalAdminManager();
                using (var connection = new SqlConnection(_connString))
                {
                    connection.Open();
                    //var query = @"SELECT [WorkRequestAssignmentID],[WorkOrderCode],[WorkOrderCodeID],[WorkRequestProjectType],[LocationID],[CreatedBy],[ModifiedDate] FROM [dbo].[WorkRequestAssignment] order by 1 desc";
                    using (var command = new SqlCommand("signalRPushNotifyForWorkOrder", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@SignalRDate", SqlDbType.Date).Value = date;
                        command.Notification = null;
                        var dependency = new SqlDependency(command);

                        dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                        if (connection.State == ConnectionState.Closed)
                            connection.Open();
                        var reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            returnData.LocationID = (long)reader["LocationID"];
                            returnData.WorkRequestAssignmentID = (long)reader["WorkRequestAssignmentID"];
                            returnData.WorkRequestProjectType = (long)reader["WorkRequestProjectType"];
                            returnData.WorkOrderCode = (string)reader["WorkOrderCode"];
                            returnData.WorkOrderCodeID = (long)reader["WorkOrderCodeID"];
                            returnData.CreatedBy = (long)reader["CreatedBy"];
                            returnData.SafetyHazard = (bool)reader["SafetyHazard"];

                            if (IsGlobalAsax == false)
                            {
                                eTracLoginModel objLoginSession = new eTracLoginModel();
                                objLoginSession = (eTracLoginModel)HttpContext.Current.Session["eTrac"];
                                dynamic LocWoList = objGlobalAdminManager.WorkOrderDetailsForPushNotificaitonSignal("", objLoginSession.UserId, objLoginSession.UserRoleId, returnData.LocationID);
                                if (LocWoList == true && returnData.CreatedBy == objLoginSession.UserId)
                                {
                                    returnData = null;
                                }
                                if (LocWoList == false)
                                {
                                    returnData = null;
                                }
                            }

                            break;
                        }
                    }
                }
                return returnData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Created By : Bhushan Dod on 18/01/2016
        /// Description : This function create sql dependency of workrequestassignment from globalasax file.
        /// To avoid asynchronous call and halt dashboard call need to create this function.
        /// </summary>
        /// <param name="IsGlobalAsax"></param>
        public void WorkOrderDetailsForPushNotificationSignalRGlobal(bool IsGlobalAsax = false)
        {
            try
            {
                var returnData = WorkOrderDetailsForPushNotificationSignalR(IsGlobalAsax);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        //Not In Used
        //public void LocationWiseWorkOrderDetailsDashboardChangeSignalR(long locID)
        //{
        //    try
        //    {                             
        //        using (var connection = new SqlConnection(_connString))
        //        {
        //            connection.Open();
        //            var query = @"SELECT [WorkRequestAssignmentID],[WorkOrderCode],[WorkOrderCodeID],[WorkRequestProjectType],[LocationID],[CreatedBy],[ModifiedDate] FROM [dbo].[WorkRequestAssignment] where [LocationID]=" + locID + " order by 1 desc";
        //            using (var command = new SqlCommand(query, connection))
        //            {
        //                command.Notification = null;

        //                var dependency = new SqlDependency(command);
        //                dependency.OnChange += new OnChangeEventHandler(dependency_OnChangeWOLocationWise);

        //                if (connection.State == ConnectionState.Closed)
        //                    connection.Open();
        //                var reader = command.ExecuteReader();
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);                
        //    }
        //}

        /// <summary>
        /// Created By : Bhushan Dod on 26/02/2016
        /// Description : To check is Server connected.This fn for sometimes server not connected then SingalR throws error.
        ///     </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public bool IsServerConnected(string conn)
        {
            bool result = false;
            using (var l_oConnection = new SqlConnection(conn))
            {
                try
                {
                    l_oConnection.Open();
                    result = true;
                    l_oConnection.Close();
                }
                catch (SqlException)
                {
                    result = false;
                }
            }
            return result;
        }

        ///// <summary>
        ///// Created By: Bhushan Dod
        ///// Created On: 13/04/2016
        /////  Utility Method -- Perform the no-parameter data query and return json
        ///// </summary>
        //public string GetDataFromQuery(string sqltext)
        //{
        //    WorkOrderEMS.Models.eTracLoginModel objLoginSession = new WorkOrderEMS.Models.eTracLoginModel();
        //    objLoginSession = (WorkOrderEMS.Models.eTracLoginModel)System.Web.HttpContext.Current.Session["eTrac"];
        //    // Perform SQL Query for chart 1 and export to JSON
        //    System.Data.DataTable dtResults = new System.Data.DataTable();
        //    string json = "";

        //    // A database query is present, get data from database
        //    try
        //    {
        //        string sql = sqltext;

        //        using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(_connString))
        //        {
        //            try
        //            {
        //                using (System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter())
        //                {
        //                    da.SelectCommand = new System.Data.SqlClient.SqlCommand(sql, conn);
        //                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

        //                    objLoginSession.UserId = 3;
        //                    // Set Input Parameter
        //                    System.Data.SqlClient.SqlParameter oParam = new System.Data.SqlClient.SqlParameter("@UserId", objLoginSession.UserId);
        //                    oParam.SqlDbType = SqlDbType.VarChar;
        //                    da.SelectCommand.Parameters.Add(oParam);
        //                    System.Data.SqlClient.SqlParameter oParam1 = new System.Data.SqlClient.SqlParameter("@LocationId", objLoginSession.LocationID);
        //                    oParam1.SqlDbType = SqlDbType.VarChar;
        //                    da.SelectCommand.Parameters.Add(oParam1);
        //                    System.Data.SqlClient.SqlParameter oParam2 = new System.Data.SqlClient.SqlParameter("@FromDate", "1753-01-01");
        //                    oParam2.SqlDbType = SqlDbType.VarChar;
        //                    da.SelectCommand.Parameters.Add(oParam2);
        //                    System.Data.SqlClient.SqlParameter oParam3 = new System.Data.SqlClient.SqlParameter("@ToDate", "9999-12-31");
        //                    oParam3.SqlDbType = SqlDbType.VarChar;
        //                    da.SelectCommand.Parameters.Add(oParam3);

        //                    DataSet ds = new DataSet();
        //                    da.Fill(ds);
        //                    json = JsonConvert.SerializeObject(ds);

        //                    //json = ConvertToJSON(ds);

        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine("Error: " + e.Message);
        //            }
        //        }

        //        //*****************************OLD CODE****************************************//

        //        ////WorkOrderEMS.Models.eTracLoginModel objLoginSession = new WorkOrderEMS.Models.eTracLoginModel();
        //        ////objLoginSession = (WorkOrderEMS.Models.eTracLoginModel)System.Web.HttpContext.Current.Session["eTrac"];
        //        //// Establish the databse connection
        //        //using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connString))
        //        //{
        //        //    using (System.Data.SqlClient.SqlCommand command = connection.CreateCommand())
        //        //    {
        //        //        command.CommandText = sqltext;
        //        //        command.CommandType = System.Data.CommandType.StoredProcedure;
        //        //        // Set Input Parameter
        //        //        System.Data.SqlClient.SqlParameter oParam = new System.Data.SqlClient.SqlParameter("@UserId", objLoginSession.UserId);
        //        //        oParam.SqlDbType = SqlDbType.VarChar; 
        //        //        command.Parameters.Add(oParam);
        //        //        System.Data.SqlClient.SqlParameter oParam1 = new System.Data.SqlClient.SqlParameter("@LocationId", objLoginSession.LocationID);
        //        //        oParam1.SqlDbType = SqlDbType.VarChar; 
        //        //        command.Parameters.Add(oParam1);
        //        //        System.Data.SqlClient.SqlParameter oParam2 = new System.Data.SqlClient.SqlParameter("@FromDate","1753-01-01");
        //        //        oParam2.SqlDbType = SqlDbType.VarChar; 
        //        //        command.Parameters.Add(oParam2);
        //        //        System.Data.SqlClient.SqlParameter oParam3 = new System.Data.SqlClient.SqlParameter("@ToDate", "9999-12-31");
        //        //        oParam3.SqlDbType = SqlDbType.VarChar; 
        //        //        command.Parameters.Add(oParam3);

        //        //        System.Data.SqlClient.SqlDataAdapter adapt = new System.Data.SqlClient.SqlDataAdapter(command);
        //        //        connection.Open();
        //        //        adapt.Fill(dtResults);
        //        //        connection.Close();
        //        //    }

        //        //    json = ConvertToJSON(dtResults);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;//json = ErrorStringToJSON(ex.ToString());
        //    }

        //    return json;
        //}
    }
}