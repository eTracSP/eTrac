using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WorkOrderEMS.BusinessLogic;

namespace WorkOrderEMS.Report
{
    public partial class Cleaning : System.Web.UI.Page
    {
        ReportManager objReportManager = new ReportManager();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportDataSource rds = new ReportDataSource("ItemCleaning", objReportManager.NoCleaningDone());
                List<ReportParameter> objReportParameter = new List<ReportParameter>();
                ReportViewer2.LocalReport.ReportPath = Server.MapPath("~/ReportRDLC/CleaningReport.rdlc"); //string.Format(@"Reports\{0}.rdl", rptName);
                ReportViewer2.LocalReport.SetParameters(objReportParameter);
                ReportViewer2.LocalReport.DataSources.Clear();
                ReportViewer2.LocalReport.DataSources.Add(rds);
                ReportViewer2.LocalReport.Refresh();
            }
        }
    }
}