using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Report
{
    public partial class WorkOrderIssuedFixedTime : System.Web.UI.Page
    {
        ReportManager objReportManager = new ReportManager();
        eTracLoginModel ObjLoginModel = null;
        string FromDate = string.Empty;
        string ToDate = string.Empty;
        string sSearchBy = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                try
                {
                    if (System.Web.HttpContext.Current.Session["eTrac"] != null)
                    {
                        ReportDataSource rds = null;
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                        if (txtFromdate.Text != null || txtTodate.Text != null)
                        {
                            if (txtFromdate.Text.Trim() != "")
                                FromDate = txtFromdate.Text;
                            else
                                FromDate = DateTime.Today.AddMonths(-3).ToString("MM/dd/yyyy");

                            if (txtTodate.Text.Trim() != "")
                                ToDate = txtTodate.Text;
                            else
                                ToDate = DateTime.Today.ToString("MM/dd/yyyy");
                        }
                        else
                        {
                            FromDate = DateTime.Today.AddMonths(-3).ToString("MM/dd/yyyy");
                            ToDate = DateTime.Today.ToString("MM/dd/yyyy");
                        }

                        if (txtSearchBy.Text.Trim() != null)
                        {
                            sSearchBy = txtSearchBy.Text;
                        }

                        int projectType = Convert.ToInt32(ddlProjectType.SelectedValue.Trim() == "0" ? null : ddlProjectType.SelectedValue);
                        if (ObjLoginModel.UserRoleId == 1 || ObjLoginModel.UserRoleId == 5)
                        {
                            btnSearchForAll.Visible = true;
                        }
                        else
                        {
                            btnSearchForAll.Visible = false;
                        }

                        if (projectType == 0)
                        {
                            rds = new ReportDataSource("DataSet_WorkOrderIssuedFixedTime", objReportManager.GetWorkOrderIssuedListFixedTime(ObjLoginModel.LocationID,
                                                                                                                                    FromDate,
                                                                                                                                    ToDate,
                                                                                                                                     null,
                                                                                                                                     sSearchBy));
                        }
                        else
                        {
                            rds = new ReportDataSource("DataSet_WorkOrderIssuedFixedTime", objReportManager.GetWorkOrderIssuedListFixedTime(ObjLoginModel.LocationID,
                                                                                                                                    FromDate,
                                                                                                                                    ToDate,
                                                                                                                                     projectType,
                                                                                                                                     sSearchBy));
                        }
                        List<ReportParameter> objReportParameter = new List<ReportParameter>();
                        objReportParameter.Add(new ReportParameter("FromDate", FromDate, true));
                        objReportParameter.Add(new ReportParameter("ToDate", ToDate, true));
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportRDLC/WorkOrderIssuedFixedTime.rdlc");
                        ReportViewer1.LocalReport.SetParameters(objReportParameter);
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(rds);
                        ReportViewer1.LocalReport.Refresh();
                    }
                }
                catch (Exception)
                {
                    throw;
                }

            }

        }

        protected void btnSearchByLoc_Click(object sender, EventArgs e)
        {

            int projectType = Convert.ToInt32(ddlProjectType.SelectedValue.Trim() == "0" ? null : ddlProjectType.SelectedValue);
            ReportDataSource rds = null;
            try
            {
                if (System.Web.HttpContext.Current.Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    if (txtFromdate.Text.Trim() != null || txtFromdate.Text.Trim() != "" || txtTodate.Text.Trim() != null || txtTodate.Text.Trim() != "")
                    {
                        if (txtFromdate.Text.Trim() != null || txtFromdate.Text.Trim() != "")
                            FromDate = txtFromdate.Text;
                        else
                            FromDate = DateTime.Today.AddMonths(-3).ToString("MM/dd/yyyy");

                        if (txtTodate.Text.Trim() != null || txtTodate.Text.Trim() != "")
                            ToDate = txtTodate.Text;
                        else
                            ToDate = DateTime.Today.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        FromDate = DateTime.Today.AddMonths(-3).ToString("MM/dd/yyyy");
                        ToDate = DateTime.Today.ToString("MM/dd/yyyy");
                    }

                    if (txtSearchBy.Text.Trim() != null)
                    {
                        sSearchBy = txtSearchBy.Text;
                    }

                    if (projectType == 0)
                    {
                        rds = new ReportDataSource("DataSet_WorkOrderIssuedFixedTime", objReportManager.GetWorkOrderIssuedListFixedTime(ObjLoginModel.LocationID,
                                                                                                                                FromDate,
                                                                                                                                ToDate,
                                                                                                                                 null,
                                                                                                                                 sSearchBy));
                    }
                    else
                    {
                        rds = new ReportDataSource("DataSet_WorkOrderIssuedFixedTime", objReportManager.GetWorkOrderIssuedListFixedTime(ObjLoginModel.LocationID,
                                                                                                                                FromDate,
                                                                                                                                ToDate,
                                                                                                                                 projectType,
                                                                                                                                 sSearchBy));
                    }
                    List<ReportParameter> objReportParameter = new List<ReportParameter>();
                    objReportParameter.Add(new ReportParameter("FromDate", FromDate, true));
                    objReportParameter.Add(new ReportParameter("ToDate", ToDate, true));
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportRDLC/WorkOrderIssuedFixedTime.rdlc"); //string.Format(@"Reports\{0}.rdl", rptName);
                    ReportViewer1.LocalReport.SetParameters(objReportParameter);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(rds);
                    ReportViewer1.LocalReport.Refresh();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        protected void btnSearchForAll_Click(object sender, EventArgs e)
        {

            int projectType = Convert.ToInt32(ddlProjectType.SelectedValue.Trim() == "0" ? null : ddlProjectType.SelectedValue);
            ReportDataSource rds = null;
            try
            {
                if (System.Web.HttpContext.Current.Session["eTrac"] != null)
                {

                    if (txtFromdate.Text.Trim() != null || txtFromdate.Text.Trim() != "" || txtTodate.Text.Trim() != null || txtTodate.Text.Trim() != "")
                    {
                        if (txtFromdate.Text.Trim() != null || txtFromdate.Text.Trim() != "")
                            FromDate = txtFromdate.Text;
                        else
                            FromDate = DateTime.Today.AddMonths(-3).ToString("MM/dd/yyyy");

                        if (txtTodate.Text.Trim() != null || txtTodate.Text.Trim() != "")
                            ToDate = txtTodate.Text;
                        else
                            ToDate = DateTime.Today.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        FromDate = DateTime.Today.AddMonths(-3).ToString("MM/dd/yyyy");
                        ToDate = DateTime.Today.ToString("MM/dd/yyyy");
                    }

                    if (txtSearchBy.Text.Trim() != null)
                    {
                        sSearchBy = txtSearchBy.Text;
                    }

                    if (projectType == 0)
                    {
                        rds = new ReportDataSource("DataSet_WorkOrderIssuedFixedTime", objReportManager.GetWorkOrderIssuedListFixedTime(null,
                                                                                                                                FromDate,
                                                                                                                                ToDate,
                                                                                                                                 null,
                                                                                                                                 sSearchBy));
                    }
                    else
                    {
                        rds = new ReportDataSource("DataSet_WorkOrderIssuedFixedTime", objReportManager.GetWorkOrderIssuedListFixedTime(null,
                                                                                                                                FromDate,
                                                                                                                                ToDate,
                                                                                                                                 projectType,
                                                                                                                                 sSearchBy));
                    }
                    List<ReportParameter> objReportParameter = new List<ReportParameter>();
                    objReportParameter.Add(new ReportParameter("FromDate", FromDate, true));
                    objReportParameter.Add(new ReportParameter("ToDate", ToDate, true));
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportRDLC/WorkOrderIssuedFixedTime.rdlc"); //string.Format(@"Reports\{0}.rdl", rptName);
                    ReportViewer1.LocalReport.SetParameters(objReportParameter);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(rds);
                    ReportViewer1.LocalReport.Refresh();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}