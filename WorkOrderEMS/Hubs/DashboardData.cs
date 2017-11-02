using Microsoft.AspNet.SignalR.Hubs;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.Configuration;
using System.Data.SqlClient;
using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WorkOrderEMS.Models;
using WorkOrderEMS.Hubs;
using System.Web;
using System;


namespace WorkOrderEMS
{
    public class DashboardData
    {

        //eTracLoginModel objeTracLoginModel = (eTracLoginModel)(System.Web.HttpContext.Current.Session["eTrac"]);

       // private const int CLOCKREFRESHTIMER = 15;
        private const int DATAREFRESHTIMER = 300;
        DateTime? fromDate = Convert.ToDateTime("1753-01-01");
        DateTime? toDate = Convert.ToDateTime("9999-12-31");
                   
        // Chart name array (must match names in Dashboard.js)
        private readonly string[] dashboardElements = new string[] {
			//"divClock_Panel", 
            "divCountPanel"

		};
        // Singleton instance
      //  private readonly static System.Lazy<DashboardData> _instance = new System.Lazy<DashboardData>(() => new DashboardData(GlobalHost.ConnectionManager.GetHubContext<DashboardHub>().Clients));

        // Get method for singleton instance variable
        //public static DashboardData Instance
        //{
        //    get
        //    {
        //        return _instance.Value;
        //    }
        //}

        // For Clients variable
        //private IHubConnectionContext<dynamic> Clients { get; set; }

        // Storage for all current dashboard data
        private readonly ConcurrentDictionary<string, DataElement> _dashboardData = new ConcurrentDictionary<string, DataElement>();

        // Refresh interval variables
        private readonly System.TimeSpan _updateInterval = System.TimeSpan.FromSeconds(DATAREFRESHTIMER);
        //private readonly TimeSpan _updateIntervalClock = TimeSpan.FromSeconds(CLOCKREFRESHTIMER);
        private readonly System.Threading.Timer _timer;

        //private readonly Timer _timerClock;

        // Refresh lock variables

        private readonly object _updateChartDataLock = new object();
        private volatile bool _updatingChartData = false;

        WorkOrderEMS.BusinessLogic.Managers.HubManager objHubManager = new WorkOrderEMS.BusinessLogic.Managers.HubManager();

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created On: 13/04/2016
        /// Get the dashboard data.Default class constructor
        /// </summary>
        public DashboardData()
        {
            // Clear the existing dashboard data container
            _dashboardData.Clear();

            // Add the chart data to the charts list and overall _dashboardData container
            var charts = new List<DataElement>();

            for (int i = 0; i < dashboardElements.Length; i++)
            {
                string json = "";
                DataElement tempChart;

                json = RefreshChartData(dashboardElements[i],0);
                tempChart = new DataElement(dashboardElements[i], json);
                charts.Add(tempChart);
            }

            charts.ForEach(DataElement => _dashboardData.TryAdd(DataElement.ElementName, DataElement));

            // Start the primary refresh timer
            _timer = new System.Threading.Timer(UpdateChartData, null, _updateInterval, _updateInterval);

        }

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created On: 13/04/2016
        /// Retrieve all data -- server method called by client Javascript to send data from server to client
        /// </summary>
        public IEnumerable<DataElement> GetAllData(long LocationId)
        {
          
            var ret = _dashboardData.Values;
            return ret;
        }
	
        /// <summary>
        /// Created By: Bhushan Dod
        /// Created On: 13/04/2016
        /// Update the chart data -- called by server timer
        /// </summary>
        public void UpdateChartData(object state)
        {
                    foreach (var data in _dashboardData.Values)
                    {
                        if (TryUpdateChartData(data))
                        {
                            return
                            BroadcastDashboardData(data);
                        }
                    }
                         
        }

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created On: 13/04/2016
        /// Attempt to update the chart data -- called by UpdateChartData
        /// </summary>
        public bool TryUpdateChartData(DataElement data)
        {
            string json = "";

            json = RefreshChartData(data.ElementName,0);

            data.ElementDataJSON = json;

            return true;
        }

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created On: 13/04/2016
        /// Broadcast the data to all connected clients -- called by UpdateChartData
        /// </summary> 	
        public void BroadcastDashboardData(DataElement data)
        {
            Clients.All.updateDashboardData(data);
        }

        /// <summary>
        /// Query the database for all chart data
        /// 
        /// </summary>
        /// 
        public string RefreshChartData(string elementName,long LocationID)
        {
            string json = "";

             WorkOrderEMS.Models.eTracLoginModel objLoginSession = new WorkOrderEMS.Models.eTracLoginModel();

             //if (System.Web.HttpContext.Current.Session["eTrac"] != null)
             //{
             //    string t = System.Web.HttpContext.Current.Session["eTrac"].ToString();
             //}
         
            if (elementName == "divCountPanel")
            {
                string sqltext = "sp_GetWebDashboardDetails";
              
                json = objHubManager.GetDataFromQuery(sqltext, 3, LocationID,fromDate,toDate);
            }
            return (json);
        }
    }

    public static class SignalRSession
    {
        private static Dictionary<string, long> _userConnections;
        public static Dictionary<string, long> UserConnections
        {
            get
            {
                if(_userConnections == null)
                {
                    _userConnections = new Dictionary<string, long>();
                }

                return _userConnections;
            }
        }
    }
}