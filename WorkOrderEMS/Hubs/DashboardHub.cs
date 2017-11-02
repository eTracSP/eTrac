using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace WorkOrderEMS.Hubs
{
    [HubName("dashboardHub")]
    public class DashboardHub : Hub
    {

        // DashboardData class
        private readonly WorkOrderEMS.DashboardData _dashboardData;

       // public DashboardHub() : this(WorkOrderEMS.DashboardData.Instance) { }

        /// <summary>
        /// Primary class constructor
        /// </summary>
        public DashboardHub(DashboardData data)
        {          
            _dashboardData = data;
        }

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created On: 13/04/2016
        /// Retrieve all data -- server method called by client Javascript to send data from server to client
        /// </summary>
        [HubMethodName("getAllData")]
        public string GetAllData(long LocationId, long UserId)
        {
            DashboardData objDashboardData = new DashboardData();
            WorkOrderEMS.Models.eTracLoginModel obj = new WorkOrderEMS.Models.eTracLoginModel();

            // SignalRSession.UserConnections.Add(this.Context.ConnectionId, LocationId);
            string json = objDashboardData.RefreshChartData(LocationId, UserId);
            return json;
        }
    }
}