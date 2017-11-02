<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoutineCheck.aspx.cs" Inherits="WorkOrderEMS.Report.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <!--DatePicker Javascript-->
        <script src="../Scripts/Jquery2.1.js"></script>

<%--    <script src="../Scripts/jquery-1.10.2.min.js"></script>--%>
    <script src="../Scripts/bootstrap.min.js"></script>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <%--  <link href="../Content/datepicker.css" rel="stylesheet" />--%>
<%--    <link href="../Content/bootstrap.css" rel="stylesheet" />--%>
    <%--  <script src="../Scripts/bootstrap-datepicker.js"></script>--%>
    <%--<script src="../Scripts/bootstrap.js"></script>--%>
     <link href="../Scripts/Bootstrap-DateTimePicker/css/bootstrap-datetimepicker.css" rel="stylesheet" />
    <%--    <script src="../Scripts/jquery.ui.datepicker.js"></script>--%>


    <!--Graph Javascript-->
    <script src="http://code.highcharts.com/highcharts.js"></script>
    <script src="http://code.highcharts.com/highcharts-3d.js"></script>
    <script src="http://code.highcharts.com/modules/exporting.js"></script>
    <link href="~/Content/font-awesome/css/font-awesome.css" rel="stylesheet" />
        <link href="../Scripts/Report/StyleReport.css" rel="stylesheet" />
        <script src="../Scripts/Bootstrap-DateTimePicker/js/moment-with-locales.js"></script>
    <script src="../Scripts/Bootstrap-DateTimePicker/js/bootstrap-datetimepicker.js"></script>

    <script src="../Scripts/Report/Report.js"></script>

    <style type="text/css">
        .auto-style1 {
            width: 213px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="idLoc"></asp:HiddenField>        
        <div class="panel panel-defaultReport">
            <div class="panel-headingReport">
                Routine Check
                 <table style="float: right;">
                     <tr>
                         <td>                             
                             <div class='input-group date' id="fromdatetimepicker1" style="top: -5px;">
                                 <input type='text' class="form-control form-control2" id="txtFromdate" placeholder="From Date" style="width:170px;height:30px"/>                                
                                 <span class="input-group-addon">
                                     <span class="glyphicon glyphicon-calendar form-control2"></span>
                                 </span>
                             </div>

                         </td>
                         <td>
                             <div class='input-group date' id="todatetimepicker1" style="top: -5px;">
                                   <input type='text' class="form-control form-control2" id="txtTodate" placeholder="To Date" style="width:170px;height:30px"/>                                
                                 <%--<asp:TextBox runat="server" ID="txtTodate" class="form-control form-control2" onpaste="return false" placeholder="To Date" Width="110px" Height="30px" ReadOnly="true"></asp:TextBox>--%>
                                 <span class="input-group-addon">
                                     <span class="glyphicon glyphicon-calendar form-control2"></span>
                                 </span>
                             </div>

                         </td>
                         <td>
                             <div id="ddlemployeedatetimepicker1" class="input-group date" style="top: -5px;">
                                 <asp:DropDownList runat="server" ID="ddlEmployee" class="form-control" Style="border-radius: 5px; height: 30px">
                                 </asp:DropDownList>
                             </div>

                         </td>
                         <td>&nbsp;
                            <asp:Button ID="btnSearchByLoc" runat="server" Text="Go" class="btn btn-default bluebutton btngo" Style="top: -5px; font-weight: bold" />
                         </td>

                     </tr>
                 </table>
            </div>
            <div class="pagi">

                <div id="containerCharts" style="height: 400px"></div>
                <div id="tableCharts" style="height: 400px"></div>
                <span class="top-right">
                    <a class="fa fa-arrow-circle-up fa-3x" href="#sign_up"></a>
                </span>
            </div>

            
        </div>
    </form>   
</body>

<script>
    $(document).ready(function () {

        $('#txtFromdate').datetimepicker({
            locale: 'ru'
        });
        $("#todatetimepicker1").datetimepicker({
            locale: 'ru'
        });
        //$("#txtFromdate").on("dp.change", function (e) {
        //    $('#txtTodate').data("DateTimePicker").minDate(e.date);
        //});
        $("#txtTodate").on("dp.change", function (e) {
            $('#txtFromdate').data("DateTimePicker").maxDate(e.date);
        });

        $('#btnSearchByLoc').click(function () {
            var userId = $("#ddlEmployee").val();
            var fromDate = new Date($("#txtFromdate").val());
            var toDate = new Date($("#txtTodate").val());
            var LocationId = $("#drp_MasterLocation").val();
            debugger;
            $.ajax({
                url: '../Report/GetAllRoutineCheck',
                data: JSON.stringify({ "locationId": LocationId, "userId": userId, "fromDate": fromDate, "toDate": toDate }),
                async: false,
                type: 'POST',
                contentType: "application/json",
                success: function (target) {
                    var arrData = [];
                    if (target.length > 0) {
                        for (i = 0; i < target.length; i++) {
                            arrData.push({
                                name: target[i].QrcTypeName,
                                y: parseInt(target[i].QrcTypeCount)
                            });
                        }


                        $('#containerCharts').highcharts({
                            chart: {
                                type: 'pie',
                                options3d: {
                                    enabled: true,
                                    alpha: 45,
                                    beta: 0
                                }
                            },
                            credits: {
                                enabled: false
                            },
                            title: {
                                text: 'Routine Check Comparison'
                            },
                            tooltip: {
                                pointFormat: '{series.name}: <b>{point.y}</b>'
                            },
                            plotOptions: {
                                pie: {
                                    allowPointSelect: true,
                                    cursor: 'pointer',
                                    depth: 35,
                                    dataLabels: {
                                        enabled: true,
                                        format: '{point.name}'
                                    },
                                    point: {
                                        events: {
                                            click: function () { printdata(LocationId, userId, fromDate, toDate, this.name, this.color); }
                                        }
                                    }
                                }
                            },
                            series: [{
                                type: 'pie',
                                name: 'No of Routine Check',
                                data: arrData
                            }]
                        });
                    }
                    else {
                        $("#containerCharts").html("No records found")
                    }
                    $("#tableCharts").hide();
                },
                error: function (er) {
                }
            });//ajax end block
        });

    })
</script>

</html>
