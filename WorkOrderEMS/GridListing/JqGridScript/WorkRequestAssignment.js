
var allLocation = '';
if ($_userType == "1" || $_userType == "5" || $_userType == "6") {
    allLocation = '<div class="onoffswitch2"><input type="checkbox" name="onoffswitch2" class="onoffswitch2-checkbox" id="ViewAllLocation"><label for="ViewAllLocation" class="onoffswitch2-label"><span class="onoffswitch2-inner"></span><span class="onoffswitch2-switch"></span></label></div>'
    //allLocation = '<input type="button" value="View All Locations" class="ViewAllButton" onclick="ViewAllRecords();" title="Click to view user on All Locations."/>'
}
var workrequestStatus = ''
    + '&nbsp&nbsp<select id="workRequestStatusTypedll" class="" onchange="doSearch(arguments[0]||event);">'
    + '<option value="">Select All</option>'
    + '<option value="14">Pending</option>'
    + '<option value="15">In Progress</option>'
    + '<option value="16">Complete</option>'
    + '<option value="341">Decline</option> </select>';

var downloadDisclaimerDoc = '../eMaintenanceDisclaimer/DisclaimerDownload/';
var downloadSurveyDoc = '../eMaintenanceDisclaimer/SurveyDownload/';

$(function () {
    var urlS = '../GridListing/JqGridHandler/WorkRequestAssignmentList.ashx?UserID=' + $_UserID;

    if (window.location.href.split('?').length >= 2) {
        if (window.location.href.split('?')[1].split('=')[0] == "filter") {
            urlS = '../GridListing/JqGridHandler/WorkRequestAssignmentList.ashx?UserID=' + $_UserID + '&filter=' + window.location.href.split('?')[1].split('=')[1];
        }
    }

    $("#tbl_WorkRequestAssignmentList").jqGrid({
        url: urlS,//'../GridListing/JqGridHandler/WorkRequestAssignmentList.ashx?UserID=' + $_UserID,
        datatype: 'json',
        height: 400,
        width: 700,
        mtype: 'POST',
        autowidth: true,
        //colNames: ['LocationName', 'Address', 'City', 'StateName', 'CountryName', 'Contact No.', 'Description', 'QRCID', 'Actions', ],
        colNames: ['Code No.', 'WorkRequestType', 'WorkRequest Type', 'Asset', 'LocationID', 'LocationName', 'Problem Desc', 'PriorityLevel', 'Priority Level', 'WorkRequestImage', 'SafetyHazard', 'Project Desc', 'WorkRequestStatus', 'Status', 'RequestBy', 'AssignToUserId', 'Assign To User', 'AssignByUserId', 'Remarks', 'WorkRequestProjectType', 'Project Type', 'Facility Request Type', 'Profile Image', 'Work Order Image', 'Submitted On', 'Submitted By', 'Submitted By User', 'DisclaimerForm', 'SurveyForm', 'StartDate', 'EndDate', 'WeekDays', 'StartTime', 'AssignedTime', 'CustomerName', 'VehicleMake', 'VehicleModel', 'CustomerContact', 'VehicleYear', 'VehicleColor', 'DriverLicenseNo', 'TotalTime','ConStartTime', 'Actions'],
        colModel: [{ name: 'CodeID', width: 80, sortable: true, hidden: false },
                  { name: 'WorkRequestType', width: 100, sortable: true, hidden: true },
                  { name: 'WorkRequestTypeName', width: 80, sortable: false, hidden: true },
                  { name: 'AssetID', width: 80, sortable: false, hidden: true },
                  { name: 'LocationID', width: 80, sortable: true, hidden: true },
                  { name: 'LocationName', width: 100, sortable: false, hidden: true },
                  { name: 'ProblemDesc', width: 80, sortable: false, hidden: true },
                  { name: 'PriorityLevel', width: 95, sortable: true, hidden: true },
                  { name: 'PriorityLevelName', width: 100, sortable: true, hidden: false },
                  { name: 'WorkRequestImage', width: 100, sortable: false, hidden: true },
                  { name: 'SafetyHazard', width: 100, sortable: false, hidden: true },
                  { name: 'ProjectDesc', width: 100, sortable: false, hidden: true },
                  { name: 'WorkRequestStatus', width: 25, sortable: true, hidden: true },
                  { name: 'WorkRequestStatusName', width: 60, sortable: false, hidden: false },
                  { name: 'RequestBy', width: 100, sortable: false, hidden: true },
                  { name: 'AssignToUserId', width: 100, sortable: false, hidden: true },
                  { name: 'AssignToUserName', width: 90, sortable: false, hidden: false },
                  { name: 'AssignByUserId', width: 100, sortable: false, hidden: true },
                  { name: 'Remarks', width: 100, sortable: false, hidden: true },
                  { name: 'WorkRequestProjectType', width: 100, sortable: false, hidden: true },
                  { name: 'WorkRequestProjectTypeName', width: 80, sortable: false },
                  { name: 'FacilityRequestType', width: 90, sortable: false },
                  { name: 'ProfileImage', width: 45, sortable: false, formatter: imageFormat },
                  { name: 'AssignedWorkImage', width: 45, sortable: false, formatter: imageFormat },
                  { name: 'CreatedDate', width: 118, sortable: false, hidden: false },
                  { name: 'CreatedByProfile', width: 45, sortable: false, formatter: imageFormat },
                  { name: 'CreatedByUserName', width: 90, sortable: false },
                  { name: 'DisclaimerForm', width: 45, sortable: true, hidden: true },
                  { name: 'SurveyForm', width: 45, sortable: true, hidden: true },
                  { name: 'StartDate', width: 45, sortable: true, hidden: true },
                  { name: 'EndDate', width: 45, sortable: true, hidden: true },
                  { name: 'WeekDays', width: 45, sortable: true, hidden: true },
                  { name: 'StartTime', width: 45, sortable: true, hidden: true },
                  { name: 'AssignedTime', width: 45, sortable: true, hidden: true },
                  { name: 'CustomerName', width: 45, sortable: true, hidden: true },
                  { name: 'VehicleMake', width: 45, sortable: true, hidden: true },
                  { name: 'VehicleModel', width: 45, sortable: true, hidden: true },
                  { name: 'CustomerContact', width: 45, sortable: true, hidden: true },
                  { name: 'VehicleYear', width: 45, sortable: true, hidden: true },
                  { name: 'VehicleColor', width: 45, sortable: true, hidden: true },
                  { name: 'DriverLicenseNo', width: 45, sortable: true, hidden: true },
                  { name: 'TotalTime', width: 45, sortable: true, hidden: true },
                  { name: 'ConStartTime', width: 45, sortable: true, hidden: true },
                  { name: 'act', index: 'act', width: 100, sortable: false, hidden: (($_isEdit != undefined && (parseInt($_isEdit) == 1)) ? false : true) }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divWorkRequestAssignmentListPager',
        sortname: 'CreatedDate',
        viewrecords: true,
        sortorder: 'desc',
        caption: "List of WorkRequestAssignment",

        gridComplete: function () {
            var ids = jQuery("#tbl_WorkRequestAssignmentList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                var rowData = jQuery("#tbl_WorkRequestAssignmentList").getRowData(cl);

                var AssignToUserId = rowData['AssignToUserId'];
                var DisclaimerForm = rowData['DisclaimerForm'];
                var SurveyForm = rowData['SurveyForm'];
                var ProjectType = rowData['WorkRequestProjectType'];
                var WorkRequestStatus = rowData['WorkRequestStatus'];
                be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 3px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
                de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 3px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
                vi = '<a href="javascript:void(0)" class="viewRecord" title="view" vid="' + cl + '" style=" float: left;margin-right: 3px;cursor:pointer;">view<span class=" icon-cog fa-2x texthover-bluelight"></span><span class="tooltips">View</span></a></div>';
                ai = '<div><a href="javascript:void(0)"  class="Assign" Id="' + cl + '" title="assign" style=" float: left;margin-right: 3px;cursor:pointer;"><span class="fa fa-file-text fa-2x texthover-yellowlight"></span><span class="tooltips">Assign</span></a>';
                qrc = '<a href="javascript:void(0)" class="qrc " title="Detail" data-value="' + cl + '" id="workorderDetail" style=" float: left;margin-right: 3px;cursor:pointer;"><span class="glyphicon glyphicon-list-alt texthover-bluelight"></span><span class="tooltips">Detail</span></a></div>';
                //jQuery("#tbl_WorkRequestAssignmentList").jqGrid('setRowData', ids[i], { act: be + de });
                var df = "";
                var sf = "";
                if (DisclaimerForm == null || DisclaimerForm == "" || DisclaimerForm == '' || DisclaimerForm == undefined) {
                }
                else {

                    df = '<a href="' + downloadDisclaimerDoc + '?Id=' + cl + '" class="download-cloud" title="Disclaimer" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-cloud-download fa-2x"></span><span class="tooltips">Disclaimer</span></a></div>';
                }
                if (SurveyForm == null || SurveyForm == "" || SurveyForm == '' || SurveyForm == undefined) {
                }
                else {

                    sf = '<a href="' + downloadSurveyDoc + '?Id=' + cl + '" class="download" title="Survey" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-download fa-2x"></span><span class="tooltips">Survey</span></a></div>';
                }

                if (WorkRequestStatus == 14 && ProjectType != 256) {
                    jQuery("#tbl_WorkRequestAssignmentList").jqGrid('setRowData', ids[i], { act: be + de + qrc + ai + df + sf });
                }
                else {
                    jQuery("#tbl_WorkRequestAssignmentList").jqGrid('setRowData', ids[i], { act: be + de + qrc + df + sf });
                }
            }
            if ($("#tbl_WorkRequestAssignmentList").getGridParam("records") <= 20) {
                $("#divWorkRequestAssignmentListPager").hide();
            }
            else {
                $("#divWorkRequestAssignmentListPager").show();
            }

            if ($('#tbl_WorkRequestAssignmentList').getGridParam('records') === 0) {
                $('#tbl_WorkRequestAssignmentList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }


            //var ProfImage = jQuery("#tbl_WorkRequestAssignmentList_ProfileImage").jqGrid('getDataIDs');

            //for (var i = 0; i < ProfImage.length; i++) {
            //    var cl = ProfImage[i];
            //    img = '<div><img src="' + cl + '" /></div>';
            //    jQuery("#tbl_WorkRequestAssignmentList_ProfileImage").jqGrid('setRowData',img);
            //}
        },
        caption: '<div class="header_search"><input id="txtSearch" class="inputSearch" placeholder="Search by Assign To User" onkeydown="doSearch(arguments[0]||event)" type="text">' + workrequestStatus + '' + allLocation + '</div>'
    });
    $('#ViewAllLocation').change(function () {
        ViewAllRecords();
    });

    if ($("#tbl_WorkRequestAssignmentList").getGridParam("records") > 20) {
        jQuery("#tbl_WorkRequestAssignmentList").jqGrid('navGrid', '#divWorkRequestAssignmentListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }

});

download = function (event) {
    var data = $("#container2 canvas")[0].toDataURL('image/png');
    $("#download").attr("href", data);
};


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
    var txtSearch = jQuery("#txtSearch").val();
    var workStauts = jQuery("#workRequestStatusTypedll option:selected").text();

    var locaId = $('#ViewAllLocation').prop('checked') == true ? 0 : $("#drp_MasterLocation :selected").val();

    if (locaId == 0) {
        $("#drp_MasterLocation").hide();
    }
    else {
        $("#drp_MasterLocation").show();
    }
    jQuery("#tbl_WorkRequestAssignmentList").jqGrid('setGridParam', { url: '../GridListing/JqGridHandler/WorkRequestAssignmentList.ashx?UserID=' + $_UserID + '&txtSearch=' + txtSearch + "&locationId=" + locaId + '&filter=' + workStauts, page: 1 }).trigger("reloadGrid");
    //jQuery("#tbl_WorkRequestAssignmentList").jqGrid('setGridParam', { url: '../GridListing/JqGridHandler/WorkRequestAssignmentList.ashx?UserID=' + $_UserID + '&txtSearch=' + txtSearch + '&filter=' + workStauts, page: 1 }).trigger("reloadGrid");
}

//$("#rightCornerClass").click(function () {
//    console.log('testing for right corner close');
//    gridReload();
//});


//#region ACTION BUTTON
$(".EditRecord").live("click", function (event) {
    var id = $(this).attr("Id");

    var rowData = jQuery("#tbl_WorkRequestAssignmentList").getRowData(id);
    var WorkRequestType = rowData['WorkRequestType'];
    var WorkRequestTypeName = rowData['WorkRequestTypeName'];
    var AssetID = rowData['AssetID'];
    var LocationID = rowData['LocationID'];
    var LocationName = rowData['LocationName'];
    var ProblemDesc = rowData['ProblemDesc'];
    var PriorityLevel = rowData['PriorityLevel'];
    var PriorityLevelName = rowData['PriorityLevelName'];
    var WorkRequestImage = rowData['WorkRequestImage'];
    var SafetyHazard = rowData['SafetyHazard'];
    var ProjectDesc = rowData['ProjectDesc'];
    var WorkRequestStatus = rowData['WorkRequestStatus'];

    var WorkRequestStatusName = rowData['WorkRequestStatusName'];
    var RequestBy = rowData['RequestBy'];
    var AssignToUserId = rowData['AssignToUserId'];
    var AssignToUserName = rowData['AssignToUserName'];
    var AssignByUserId = rowData['AssignByUserId'];
    var Remarks = rowData['Remarks'];
    var WorkRequestProjectType = rowData['WorkRequestProjectType'];
    var WorkRequestProjectTypeName = rowData['WorkRequestProjectTypeName'];
    var id = $(this).attr("Id");
    window.location.href = '../GlobalAdmin/Edit?id=' + id
    //window.location.href = '../GlobalAdmin/Edit?id=' + id + '&WorkRequestType=' + WorkRequestType + '&AssetID=' + AssetID + '&LocationID=' + LocationID
    //+ '&ProblemDesc=' + ProblemDesc + '&PriorityLevel=' + PriorityLevel + '&WorkRequestImage=' + WorkRequestImage + '&SafetyHazard=' + SafetyHazard
    //+ '&ProjectDesc=' + ProjectDesc + '&WorkRequestStatus=' + WorkRequestStatus + '&RequestBy=' + RequestBy + '&AssignToUserId=' + AssignToUserId + '&WorkRequestProjectType=' + WorkRequestProjectType;

});

$(".Assign").live('click', function (event) {
    
    var id = $(this).attr("id");
    var rowData = jQuery("#tbl_WorkRequestAssignmentList").getRowData(id);
    var ProblemDesc = rowData['ProblemDesc'];
    var PriorityLevel = rowData['PriorityLevel'];
    var ProjectDesc = rowData['ProjectDesc'];
    var locationId = rowData['LocationID'];
    var WorkRequestType = rowData['WorkRequestProjectTypeName'];
    var AssignedTime = rowData['AssignedTime'];
    var AssignToUserId = rowData['AssignToUserId'];
    if (PriorityLevel == null || PriorityLevel == "" || PriorityLevel == 'undefined') {
        PriorityLevel = 0;
    }
    $.ajax({
        type: "GET",
        data: { 'id': id, 'ProblemDesc': ProblemDesc, 'PriorityLevel': PriorityLevel, 'ProjectDesc': ProjectDesc, 'WorkRequestType': WorkRequestType, 'locationId': locationId, 'AssignedTime': AssignedTime, 'AssignToUserId': AssignToUserId },
        url: '../GlobalAdmin/_AssignWorkAssignmentRequest/',
        contentType: "application/json; charset=utf-8",
        error: function (xhr, status, error) {
        },
        success: function (result) {
            $('.modal-title').text("Assign Work Order");
            $("#largeeditpopup").html(result);
            $("#myModallarge").modal('show');
        }
    });
});
$(".deleteRecord").live("click", function (event) {
     //debugger
    var id = $(this).attr("cid");

    showPopupRelativeMessage("Are you sure want to delete work order?", $(this), "Delete Work Order", function () {
        $.ajax({
            type: "POST",
            data: "{'id':'" + id + "'}",
            //url: '../GridListing/JqGrid Handler/StaffUserList.ashx?id=' + id,
            url: '../GlobalAdmin/DeleteWorkRequest/',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            error: function (xhr, status, error) {
                closeAjaxProgress();
            },
            beforeSend: function () {
                showAjaxProgress();
            },
            success: function (result) {
                // AlertMessage(result);
                $("#message").html(result.Message);
                $("#message").addClass(result.AlertMessageClass);
                closeAjaxProgress();
                jQuery("#tbl_WorkRequestAssignmentList").jqGrid().trigger("reloadGrid");
                toastr.success(result.Message)
                event.stopPropagation();
                gridReload();
            },
            Complete: function (result) {
                closeAjaxProgress();
                event.stopPropagation();
            }
        });
    });
});

$("#workorderDetail").live("click", function (event) {

    var id = $(this).attr("data-value");

    var rowData = jQuery("#tbl_WorkRequestAssignmentList").getRowData(id);
    var WorkRequestType = rowData['WorkRequestType'];
    var WorkRequestTypeName = rowData['WorkRequestTypeName'];
    var AssetID = rowData['AssetID'];
    var LocationID = rowData['LocationID'];
    var LocationName = rowData['LocationName'];
    var ProblemDesc = rowData['ProblemDesc'];
    var PriorityLevel = rowData['PriorityLevel'];
    var PriorityLevelName = rowData['PriorityLevelName'];
    var WorkRequestImage = rowData['WorkRequestImage'];
    var SafetyHazard = rowData['SafetyHazard'];
    var ProjectDesc = rowData['ProjectDesc'];
    var WorkRequestStatus = rowData['WorkRequestStatus'];

    var WorkRequestStatusName = rowData['WorkRequestStatusName'];
    var RequestBy = rowData['RequestBy'];
    var AssignToUserId = rowData['AssignToUserId'];
    var AssignToUserName = rowData['AssignToUserName'];
    var AssignByUserId = rowData['AssignByUserId'];
    var Remarks = rowData['Remarks'];
    var WorkRequestProjectType = rowData['WorkRequestProjectType'];
    var WorkRequestProjectTypeName = rowData['WorkRequestProjectTypeName'];
    var Submittedon = rowData['CreatedDate'];
    var AssignedWorkOrderImage = rowData['AssignedWorkImage'];
    //alert($('.gridimage img').prop('src'));
    var ProfileImage = rowData['ProfileImage'];
    var CodeId = rowData['CodeID'];
    var StartDate = rowData['StartDate'];
    var EndDate = rowData['EndDate'];
    var WeekDays = rowData['WeekDays'];
    var StartTime = rowData['ConStartTime'];
    var AssignedTime = rowData['AssignedTime'];
    var CustomerName = rowData['CustomerName'];
    var VehicleMake = rowData['VehicleMake'];
    var VehicleModel = rowData['VehicleModel'];
    var CustomerContact = rowData['CustomerContact'];
    var VehicleYear = rowData['VehicleYear'];
    var VehicleColor = rowData['VehicleColor'];
    var DriverLicenseNo = rowData['DriverLicenseNo'];
    var TotalTimeTaken = rowData['TotalTime'];
    $("#lblCodeNo").html(CodeId);
    $("#lblProjectType").html(WorkRequestProjectTypeName);
    if (WorkRequestType == null || WorkRequestType == "") {
        $("#labelWorkRequestType").hide();
        $("#lblWorkRequestType").hide();
    }
    else {
        $("#lblWorkRequestType").html(WorkRequestTypeName);
        $("#labelWorkRequestType").show();
        $("#lblWorkRequestType").show();
    }

    $("#lblPriorityLevel").html(PriorityLevelName);
    if (WorkRequestProjectType != 279) {
        $("#lblWorkRequestStatus").html(WorkRequestStatusName);
        $("#labellWorkRequestStatus").show();
        $("#lblWorkRequestStatus").show();
    }
    else {
        $("#labellWorkRequestStatus").hide();
        $("#lblWorkRequestStatus").hide();
    }

    $("#lblSubmittedOn").html(Submittedon);

    if (ProblemDesc == null || ProblemDesc == "") {
        $("#labellProblemDescription").hide();
        $("#lblProblemDescription").hide();
    }
    else {
        $("#lblProblemDescription").html(ProblemDesc);
        $("#labellProblemDescription").show();
        $("#lblProblemDescription").show();
    }
    if (ProjectDesc == null || ProjectDesc == "") {
        $("#labellProjectDescription").hide();
        $("#lblProjectDescription").hide();
    }
    else {
        $("#lblProjectDescription").html(ProjectDesc);
        $("#labellProjectDescription").show();
        $("#lblProjectDescription").show();
    }
    if (AssignedTime != null && AssignedTime != "") {
        $("#lblTimeAssigned").html(AssignedTime);
        $("#labellTimeAssigned").show();
        $("#lblTimeAssigned").show();
    }
    else {
        $("#labellTimeAssigned").hide();
        $("#lblTimeAssigned").hide();
    }
    if (WorkRequestProjectType == 279) {
        $("#lblStartDate").html(StartDate);
        $("#lblEndDate").html(EndDate);
        $("#lblWeekdays").html(WeekDays);
        $("#lblStartTime").html(StartTime);
        $("#labellStartDate").show();
        $("#lblStartDate").show();
        $("#labellEndDate").show();
        $("#lblEndDate").show();
        $("#labellWeekdays").show();
        $("#lblWeekdays").show();
        $("#labellStartTime").show();
        $("#lblStartTime").show();

    }
    else {
        // $("#labellWorkRequestStatus").hide();
        //$("#lblWorkRequestStatus").hide();
        $("#labellStartDate").hide();
        $("#lblStartDate").hide();
        $("#labellEndDate").hide();
        $("#lblEndDate").hide();
        $("#labellWeekdays").hide();
        $("#lblWeekdays").hide();
        $("#labellStartTime").hide();
        $("#lblStartTime").hide();
        //$("#labellTimeAssigned").hide();
        //$("#lblTimeAssigned").hide();
    }

    if (WorkRequestProjectType == 256) {
        $("#lblCustomerName").html(CustomerName);
        $("#lblVehicleMake").html(VehicleMake);
        $("#lblCustomerContact").html(CustomerContact);
        $("#lblVehicleYear").html(VehicleYear);
        $("#lblVehicleModel").html(VehicleModel);
        $("#lblVehicleColor").html(VehicleColor);
        $("#lblDriverLicenseNo").html(DriverLicenseNo);
        $("#labellCustomerName").show();
        $("#lblCustomerName").show();
        $("#labellVehicleMake").show();
        $("#lblVehicleMake").show();
        $("#labellCustomerContact").show();
        $("#lblCustomerContact").show();
        $("#labellVehicleYear").show();
        $("#lblVehicleYear").show();
        $("#labellVehicleColor").show();
        $("#lblVehicleColor").show();
        $("#labellDriverLicenseNo").show();
        $("#lblDriverLicenseNo").show();
        $("#lblVehicleModel").show();
        $("#labellVehicleModel").show();
        $("#labellAssignedWorkImage").hide();
        $("#lblAssignedWorkImage").hide();

    }
    else {
        $("#labellCustomerName").hide();
        $("#lblCustomerName").hide();
        $("#labellVehicleMake").hide();
        $("#lblVehicleMake").hide();
        $("#labellCustomerContact").hide();
        $("#lblCustomerContact").hide();
        $("#labellVehicleYear").hide();
        $("#lblVehicleYear").hide();
        $("#labellVehicleColor").hide();
        $("#lblVehicleColor").hide();
        $("#labellDriverLicenseNo").hide();
        $("#lblDriverLicenseNo").hide();
        $("#lblVehicleModel").hide();
        $("#labellVehicleModel").hide();
        $("#labellAssignedWorkImage").show();
        $("#lblAssignedWorkImage").html(AssignedWorkOrderImage);
        $("#lblAssignedWorkImage").show();
        $("#divWoimg").show();
    }

    if (AssignedWorkOrderImage == '' || AssignedWorkOrderImage == null || AssignedWorkOrderImage == "") {
        $("#lblAssignedWorkImage").hide();
        $("#labellAssignedWorkImage").hide();
    }
    else {
        var img = $('#lblAssignedWorkImage img').attr('src');
        if (img != null && img != undefined && img != '') {
            if (img.split("/").pop() == 'no-asset-pic.png') {
                $("#divWoimg").hide();
            }
        }

    }

    if (AssignToUserId != null && AssignToUserId != "" && AssignToUserId != '') {
        $('#divEmpAssigned').show();
        $("#lblProfile").html(ProfileImage);
        $("#lblAssignToUser").html(AssignToUserName);
    }
    else {
        $('#divEmpAssigned').hide();
        //$("#lblProfile").html(ProfileImage);
        //$("#lblAssignToUser").html(AssignToUserName);
    }
    // $("#lblAssignedWorkImage").html(AssignedWorkOrderImage);
 
    if (WorkRequestProjectType != 279 && TotalTimeTaken != null && TotalTimeTaken != "") {
        $("#lblTotalTimeTaken").html(TotalTimeTaken);
        $("#labelTotalTimeTaken").show();
        $("#lblTotalTimeTaken").show();
    }
    else {
        $("#labelTotalTimeTaken").hide();
        $("#lblTotalTimeTaken").hide();
    }

    $("#lblLocationName").html(LocationName);
    $('.modal-title').text("Work Order Detail");
    $("#ModalDetailPreview").modal('show');

});

//#endregion
function EnlargeImageView(value) {
    $("#myAssignedWorkOrder").modal('show');
    $("#myAssignedWorkOrder").find("#myModalLabel").html("");
    var path = $(value).attr("src");
    var data = "<img src='" + path + "' style='width:100%;height:100%;' />";
    $("#myAssignedWorkOrder").find("#AssignedWorkorderBody").html(data);

}

function ViewAllRecords() {
    var txtSearch = jQuery("#txtSearch").val();
    var workStauts = jQuery("#workRequestStatusTypedll option:selected").text();
    var locaId = $('#ViewAllLocation').prop('checked') == true ? 0 : $("#drp_MasterLocation :selected").val();

    if (locaId == 0) {
        $("#drp_MasterLocation").hide();
    }
    else {
        $("#drp_MasterLocation").show();
    }

    jQuery("#tbl_WorkRequestAssignmentList").jqGrid('setGridParam', { url: '../GridListing/JqGridHandler/WorkRequestAssignmentList.ashx?UserID=' + $_UserID + '&txtSearch=' + txtSearch + "&locationId=" + locaId + '&filter=' + workStauts, page: 1 }).trigger("reloadGrid");
}

function closeRightCornerPopUP(e) {

    $(e).parent("div").hide();
    console.log("grid loading");

    gridReload();
}

function PrintDivForWorkDetails(DivId) {

    _isPrintDone = false;
    if (!_isPrintDone) {
        //var divToPrint = document.getElementById('DivQRCIndex');
        var workrequesttype = '';
        var divToAssignedWorkImg = document.getElementById("lblAssignedWorkImage");
        var divToProfile = document.getElementById("lblProfile");
        var popupWin = window.open('', '_blank', 'width=800,height=600');
        popupWin.document.open();
        //popupWin.document.write("<html><body onload='window.print(); window.close();'><div style='width:800px;height:300px;'>" + divToPrint.innerHTML + "</div></body></html>");
        if ($("#lblWorkRequestType").html() != null && $("#lblWorkRequestType").html() != "") {
            workrequesttype = "<p></p><strong style='font-size: 16px;'>Work Request Type </strong> <br/>"
                      + $("#lblWorkRequestType").html();
        }

        popupWin.document.write("<style>img {height: 110px;width: 115px;}</style><html><body onload='window.print();'><div style='margin-left: 96px; margin-right: 100px; width: 420px;' class='row '><table id='DivWorkDetailsIndex' style='width: 400px; border: 1px solid #0aa0cd; padding: 10px;'><tr><td valign='top' style='width: 210px;'><p></p><strong style='font-size: 16px;'> Code No</strong> <br/>"
            + $("#lblCodeNo").html() + "<p></p><strong style='font-size: 16px;'>Project Type </strong> <br/>"
            + $("#lblProjectType").html() + workrequesttype + "<p></p><strong style='font-size: 16px;'>Priority Level </strong> <br/>"
            + $("#lblPriorityLevel").html() + "<p></p><strong style='font-size: 16px;'>Work Request Status </strong> <br/>"
            + $("#lblWorkRequestStatus").html() + "<p></p><strong style='font-size: 16px;'>Submitted On </strong> <br/>"
            + $("#lblSubmittedOn").html()
            + "</td><td td valign='top';>"
            + "<p></p><strong style='font-size: 16px;'>Location Name </strong><br/>"
            + $("#lblLocationName").html()
            + "<p></p><strong style='font-size: 16px;'>Employee Assigned</strong><br/> " + divToProfile.innerHTML + "<p></p><strong style='font-size: 16px;'>Work Order Image</strong><br/>" + divToAssignedWorkImg.innerHTML + "</td></tr></tbody></table></td></tr></table></div></body></html>");

        if (popupWin.closed == false) {
            popupWin.document.close();
        }
        _isPrintDone = true;
    }
    //$('.noprint').show();
}
