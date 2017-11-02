var $userType;
var $locationId;
var $hiddenAction = true;
var ListUserUrl = '../GridListing/JqGridHandler/DARDetailsList.ashx';

function _bind_DatePickers() {

    $("#fromDateDAR").datepicker({ dateFormat: 'mm/dd/yy', endDate: new Date, startDate: '-3m' });
    $("#toDateDAR").datepicker({ dateFormat: 'mm/dd/yy', endDate: new Date, startDate: '0d' });

    var startDate = new Date();
    var FromEndDate = new Date();
    var ToEndDate = new Date();
    ToEndDate.setDate(ToEndDate.getDate());

    $('#fromDateDAR').datepicker({
        weekStart: 1,
        startDate: FromEndDate.setDate(-30),
        endDate: FromEndDate,
        autoclose: true
    })
        .on('changeDate', function (selected) {
            startDate = new Date(selected.date.valueOf());
            startDate.setDate(startDate.getDate(new Date(selected.date.valueOf())));
            $('#toDateDAR').datepicker('setStartDate', startDate);
            $('#fromDateDAR').datepicker('hide');
        });

    $('#toDateDAR')
        .datepicker({
            weekStart: 1,
            startDate: startDate,
            endDate: ToEndDate,
            autoclose: true
        })
        .on('changeDate', function (selected) {
            FromEndDate = new Date(selected.date.valueOf());
            FromEndDate.setDate(FromEndDate.getDate(new Date(selected.date.valueOf())));
            $('#fromDateDAR').datepicker('setEndDate', FromEndDate);
            $('#toDateDAR').datepicker('hide');
        });
};

var EmployeeDDL = '<select id="Employeeddl">'
    + '<option value="0" >All Employee</option>';

$(window).load(function () {
    if ($userType == "1" || $userType == "5") { //Global Admin +IT Administrator
        $hiddenAction = false;
    }
    else {
        $hiddenAction = true;
    }
    var LocationId = $("#drp_MasterLocation").val();

    $.ajax({
        //url: '../GlobalAdmin/GetListITAdministrator',
        //data: { UserType: "All Users" },//JSON.stringify({ UserType :"All Users" }),
        url: '../DropDown/GetEmployeeByLocation',
        data: JSON.stringify({ LocationId: LocationId }),
        async: false,
        type: 'POST',
        contentType: "application/json",
        success: function (result) {
            $.each(result, function (i, v) {
                EmployeeDDL = EmployeeDDL + '<option value="' + v.Value + '">' + v.Text + '</option>';
            });
            EmployeeDDL = EmployeeDDL + '</select>';
        },
        error: function (er) {
        }
    });

    var TaskTypeDll = '<select id="TaskTypeDll">'
        + '<option value="0" >All Work Request Type</option>';

    $(function () {
        $.ajax({
            url: '../Common/GetTaskType',
            data: JSON.stringify({ taskType: "TASKTYPECATEGORY" }),
            async: false,
            type: 'POST',
            contentType: "application/json",
            success: function (result) {
                $.each(result, function (i, v) {
                    TaskTypeDll = TaskTypeDll + '<option value="' + v.Value + '">' + v.Text + '</option>';
                });
                TaskTypeDll = TaskTypeDll + '</select>';
            },
            error: function (er) {
            }
        });

        $("#tbl_DARList").jqGrid({

            url: ListUserUrl,
            datatype: 'json',
            height: 400,
            mtype: 'POST',
            width: 700,
            autowidth: true,
            //colNames: ['Employee Name', 'Activity Details', 'Work Requst Type', 'Start Time', 'End Time', 'Start Time Image', 'End Time Image', 'Submitted On', 'Actions'],
            colNames: ['Employee Name', 'Activity Details', 'Work Requst Type', 'Start Time', 'End Time', 'Submitted On', 'Actions'],
            colModel: [
                { name: 'Employee Name', width: 65, sortable: false },
                { name: 'Activity Details', width: 180, sortable: false },
                { name: 'Work Requst Type', width: 80, sortable: false },
                { name: 'Start Time', width: 40, sortable: false },
                { name: 'End Time', width: 40, sortable: false },
                //{ name: 'Start Time Image', width: 30, sortable: false, formatter: imageFormat },
                //{ name: 'End Time Image', width: 25, sortable: false, formatter: imageFormat },
                { name: 'Submitted On', width: 65, sortable: false },
                { name: 'act', index: 'act', width: 35, sortable: false, hidden: $hiddenAction }],
            rownum: 10,
            rowList: [10, 20, 30],
            scrollOffset: 0,
            pager: '#divDARListPager',
            sortname: 'CreatedOn',
            viewrecords: true,
            sortorder: 'desc',
            caption: "List of DAR",
            loadError: function (data) { $('#message').html(JSON.stringify(data) + 'No records found.'); },
            gridComplete: function () {
                //$('.clockpicker').each(function () {
                //    $(this).clockpicker();
                //});
                _bind_DatePickers();
                $('#message').html('');
                var ids = jQuery("#tbl_DARList").jqGrid('getDataIDs');
                for (var i = 0; i < ids.length; i++) {
                    var cl = ids[i];
                    var rowData = jQuery("#tbl_DARList").getRowData(cl);

                    be = '<div><a href="javascript:void(0)" data-toggle = "modal" data-target = "#myModalmedium" class="EditRecord"  Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
                    jQuery("#tbl_DARList").jqGrid('setRowData', ids[i], { act: be });
                }
                $("#fromDate").datepicker({ dateFormat: 'mm/dd/y', minDate: '-30', maxDate: new Date });
                $("#toDate").datepicker({ dateFormat: 'mm/dd/y', minDate: new Date, maxDate: new Date });
                //Added By Bhushan Dod on 30/03/2015 if record < 20 then pager become hide 
                if ($("#tbl_DARList").getGridParam("records") <= 20) {
                    $("#divDARListPager").hide();
                }
                else {
                    $("#divDARListPager").show();
                }
                if ($('#tbl_DARList').getGridParam('records') === 0) {
                    $('#tbl_DARList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
                }

                if (!($('#txtFromTime').length > 0)) {
                    var label = $("<input type=\"text\" placeholder=\"From Time\" id=\"txtFromTime\" style=\"width:95px;height: 30px; margin - right: 5px; cursor: pointer;\" data-role=\"datebox\" data-options=\'{\"mode\":\"durationflipbox\",\"useFocus\":true,\"overrideDurationOrder\":[\"h\",\"i\"],\"overrideDurationFormat\": \"%Dl:%DM\" }\' readonly=\"readonly\" />");
                    $("#dynamicFromTime").append(label).trigger("create"); // for dymanic creation
                }
                if (!($('#txtToTime').length > 0)) {
                    var label = $("<input type=\"text\" placeholder=\"To Time\" id=\"txtToTime\" style=\"width:90px;height: 30px; margin - right: 5px; cursor: pointer;\" data-role=\"datebox\" data-options=\'{\"mode\":\"durationflipbox\",\"useFocus\":true,\"overrideDurationOrder\":[\"h\",\"i\"],\"overrideDurationFormat\": \"%Dl:%DM\" }\' readonly=\"readonly\" />");
                    $("#dynamicToTime").append(label).trigger("create"); // for dymanic creation
                }
            },
            caption: '<div class="header_search"><input id="fromDateDAR" class="" type="text" value="" style="width:110px;  margin-right: 5px; cursor: pointer;" name="startDate_Check" placeholder="From Date" readonly="readonly"><div id="dynamicFromTime" class="dynamic-input"></div><input id="toDateDAR" class="" type="text" value="" style="width:110px; margin-right: 5px; cursor: pointer;" name="endDate_Check" placeholder="To Date" readonly="readonly" ><div id="dynamicToTime"></div>' + TaskTypeDll + '' + EmployeeDDL + '&nbsp;<input type="button" value="Show DAR" class="ViewAllButton" onclick="ViewDARDetails();" title="Click to view user on All Locations."/></div>'
            //caption: '<div class="header_search"><input id="fromDateDAR" class="" type="text" value="" style="width:110px;  margin-right: 5px; cursor: pointer;" name="startDate_Check" placeholder="From Date" readonly="readonly"><input type="text" class="clockpicker" id="txtFromTime" placeholder="From Time" style="width:95px;height:30px; margin-right: 5px; cursor: pointer;" data-placement="bottom" data-align="top" data-autoclose="true" readonly="readonly" /><input id="toDateDAR" class="" type="text" value="" style="width:110px; margin-right: 5px; cursor: pointer;" name="endDate_Check" placeholder="To Date" readonly="readonly" ><input type="text" class="clockpicker" id="txtToTime" placeholder="To Time" style="width:90px;height:30px; margin-right: 5px; cursor: pointer;" data-placement="bottom" data-align="top" data-autoclose="true" readonly="readonly" />' + TaskTypeDll + '' + EmployeeDDL + '&nbsp;<input type="button" value="Show DAR" class="ViewAllButton" onclick="ViewDARDetails();" title="Click to view user on All Locations."/></div>'

        });
        if ($("#tbl_DARList").getGridParam("records") > 20) {
            jQuery("#tbl_DARList").jqGrid('navGrid', '#divDARListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
        }
    });
});

//#region Image
function imageFormat(cellvalue, options, rowObject) {

    if (cellvalue == "")
    { return ""; }
    else {
        return '<img src="' + cellvalue + '" class="gridimage" onclick="EnlargeImageView(this);"/>';
    }
}
//#endregion
var timeoutHnd;
var flAuto = true;
function doSearch(ev) {
    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
}


function gridReload() {
    var fromDate = $("#fromDateDAR").val() + " " + $("#txtFromTime").val();
    var toDate = jQuery("#toDateDAR").val() + " " + $("#txtToTime").val();
    var empId = $("#Employeeddl").val();
    var taskType = $("#TaskTypeDll").val();
    jQuery("#tbl_DARList").jqGrid('setGridParam', { url: ListUserUrl + "?fromDate=" + fromDate + "&toDate=" + toDate + "&taskType=" + taskType + "&empId=" + empId, page: 1 }).trigger("reloadGrid");
}

//#endregion
function EnlargeImageView(value) {
    $("#myAssignedWorkOrder").modal('show');
    $("#myAssignedWorkOrder").find("#myModalLabel").html("");
    var path = $(value).attr("src");
    var data = "<img src='" + path + "' style='width:100%;height:100%;' />";
    $("#myAssignedWorkOrder").find("#AssignedWorkorderBody").html(data);
}
function ViewDARDetails() {
    var fromDate = $("#fromDateDAR").val() + " " + $("#txtFromTime").val();;
    var toDate = jQuery("#toDateDAR").val() + " " + $("#txtToTime").val();;
    var empId = $("#Employeeddl").val();
    var taskType = $("#TaskTypeDll").val();
    jQuery("#tbl_DARList").jqGrid('setGridParam', { url: ListUserUrl + "?fromDate=" + fromDate + "&toDate=" + toDate + "&taskType=" + taskType + "&empId=" + empId, page: 1 }).trigger("reloadGrid");
}

$(".EditRecord").live("click", function (event) {
    var id = $(this).attr("Id");
    $('#mediumeditpopup').load("../DAR/EditDARDetail", { 'id': id }
        , function (e) {
            $('.modal-title').text("Edit DAR Details");
            //window.location.href="../"

        });
});

function success(e) {
    $("#myModalmedium").modal("hide");
    ViewDARDetails();
}