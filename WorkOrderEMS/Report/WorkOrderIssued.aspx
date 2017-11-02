<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderIssued.aspx.cs" Inherits="WorkOrderEMS.Report.WorkOrderIssued" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <!--DatePicker Javascript-->
    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/jquery.ui.datepicker.js"></script>
    <script src="../Scripts/jquery.ui.core.js"></script>
    <link href="../Content/themes/base/jquery.ui.all.css" rel="stylesheet" />
    <link href="../Content/themes/base/jquery.ui.theme.css" rel="stylesheet" />
    <link href="../Content/themes/base/jquery.ui.core.css" rel="stylesheet" />
    <link href="../Content/themes/base/jquery.ui.datepicker.css" rel="stylesheet" />
    <link href="../Content/JqGridCSS/ui.jqgrid.css" rel="stylesheet" />

    <link href="../Scripts/Report/StyleReport.css" rel="stylesheet" />
    <script src="../Scripts/Report/Report.js"></script>

    <style type="text/css">
        .auto-style1 {
            width: 213px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
     <div class="panel panel-defaultReport">
        <div class="panel-headingReport">
            Work Order Issued
        </div>
     
        <div class="pagi">

            <table id="appointment">
                <tr>
                    <td class="auto-style1">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label runat="server" ID="lblFromdate" Text="From" class="lab07" Style="font-weight: 700"></asp:Label>
                    </td>
                    <td>
                        <span class="inpt_rit">
                            <asp:TextBox runat="server" ID="txtFromdate" onpaste="return false" class="form-control" placeholder="mm/dd/yyyy" Width="190px"></asp:TextBox>
                        </span>
                    </td>

                    <td>
                        <asp:Label runat="server" ID="lblTodate" Text="To" class="lab07" Style="font-weight: 700"></asp:Label>
                    </td>
                    <td>
                        <span class="inpt_rit">
                            <asp:TextBox runat="server" ID="txtTodate" class="form-control" onpaste="return false" placeholder="mm/dd/yyyy" Width="190px"></asp:TextBox>
                        </span>
                    </td>

                </tr>
                <tr></tr>
                <tr>
                    <td class="auto-style1">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label runat="server" ID="lblProjectType" Text="Project Type" class="lab07" Style="font-weight: 700"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlProjectType" class="form-control">
                            <asp:ListItem Value="0">All</asp:ListItem>
                            <asp:ListItem Value="128">Work Order Request</asp:ListItem>
                            <asp:ListItem Value="129">Special Project</asp:ListItem>
                        </asp:DropDownList>
                    </td>

                    <td>
                        <asp:Label runat="server" ID="lblSearchBy" Text="Search By" class="lab07" Style="font-weight: 700"></asp:Label>
                    </td>
                    <td>
                        <span class="inpt_rit">
                            <asp:TextBox runat="server" ID="txtSearchBy" class="form-control" placeholder="Search By" Width="190px"></asp:TextBox>
                        </span>
                    </td>
                </tr>

                <tr>
                    <td></td>
                    <td class="auto-style1" colspan="2">&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnSearchByLoc" runat="server" Text="Search By Location" class="btn btn-default bluebutton" OnClick="btnSearchByLoc_Click" Width="155px" />
                    </td>
                    <td>&nbsp;&nbsp;&nbsp
                        <asp:Button ID="btnSearchForAll" runat="server" Text="Search for All Location" class="btn btn-default bluebutton" OnClick="btnSearchForAll_Click" Width="155px" />

                    </td>
                    <td>
                        <%--<input type="reset" value="ResetDate" id="resetdate" onclick="ResetDates()" class="big_ogng_tab" />--%>
                    </td>
                </tr>
            </table>
          
             </div>
           <br />
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="666px" Style="margin-left: 0%;"
                Font-Names="Verdana"
                Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt">
            </rsweb:ReportViewer>

            <table>
                <tr>
                    <td><span style="display: none; margin-left: 258px" class="errormsg mgnrht55" id="errordate">(Startdate can't greater than End Date)</span></td>
                    <td>
                        <span style="display: none; margin-left: 258px" class="errormsg mgnrht55" id="error">(For Search by date please select Start and End Date)</span>
                    </td>
                </tr>
            </table>
       
        <br />
        <br />
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
    </div>
    </form>
</body>
</html>
