$(function () {

    $("#tbl_HistoryWorkRequestAssignmentList").jqGrid({
        url: '../GridListing/JqGridHandler/WorkOrderAssignmentByManager.ashx?UserID=' + $_UserID,
        datatype: 'json',
        height: 400,
        width: 700,
        autowidth: true,
        //colNames: ['LocationName', 'Address', 'City', 'StateName', 'CountryName', 'Contact No.', 'Description', 'QRCID', 'Actions', ],
        colNames: ['WorkRequestType', 'WorkRequest Type', 'Asset', 'LocationID', 'LocationName', 'Problem Desc', 'PriorityLevel', 'Priority Level', 'WorkRequestImage', 'SafetyHazard', 'Project Desc', 'WorkRequestStatus', 'WorkRequest Status', 'RequestBy', 'AssignToUserId', 'Assign To User', 'AssignByUserId', 'Remarks', 'WorkRequestProjectType', 'Project Type', 'Actions'],
        colModel: [{ name: 'WorkRequestType', width: 100, sortable: true, hidden: true },
                  { name: 'WorkRequestTypeName', width: 80, sortable: false, hidden: true },
                  { name: 'AssetID', width: 80, sortable: false, hidden: true },
                  { name: 'LocationID', width: 80, sortable: false, hidden: true },
                  { name: 'LocationName', width: 80, sortable: false, hidden: false },
                  { name: 'ProblemDesc', width: 80, sortable: false, hidden: true },
                  { name: 'PriorityLevel', width: 100, sortable: false, hidden: true },
                  { name: 'PriorityLevelName', width: 100, sortable: false, hidden: false },
                  { name: 'WorkRequestImage', width: 100, sortable: false, hidden: true },
                  { name: 'SafetyHazard', width: 100, sortable: false, hidden: true },
                  { name: 'ProjectDesc', width: 100, sortable: false, hidden: true },
                  { name: 'WorkRequestStatus', width: 100, sortable: false, hidden: true },
                  { name: 'WorkRequestStatusName', width: 100, sortable: false, hidden: false },
                  { name: 'RequestBy', width: 100, sortable: false, hidden: true },
                  { name: 'AssignToUserId', width: 100, sortable: false, hidden: true },
                  { name: 'AssignToUserName', width: 100, sortable: false, hidden: false },
                  { name: 'AssignByUserId', width: 100, sortable: false, hidden: true },
                  { name: 'Remarks', width: 100, sortable: false, hidden: true },
                  { name: 'WorkRequestProjectType', width: 100, sortable: false, hidden: true },
                  { name: 'WorkRequestProjectTypeName', width: 100, sortable: false },
                  //{ name: 'QRCID', width: 80, sortable: false },
                  { name: 'act', index: 'act', width: 80, sortable: false, hidden: (($_isEdit != undefined && (parseInt($_isEdit) == 1)) ? false : true) }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divHistoryWorkRequestAssignmentListPager',
        sortname: 'LocationName',
        viewrecords: true,
        sortorder: 'desc',
        caption: "List of WorkRequestAssignment",

        gridComplete: function () {

            var ids = jQuery("#tbl_HistoryWorkRequestAssignmentList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="ui-icon ui-icon-pencil"></span></a>'
                de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="ui-icon ui-icon-trash"></span></a>';
                vi = '<a href="javascript:void(0)" class="viewRecord" title="view" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;">view<span class="ui-icon ui-icon-disk"></span></a></div>';
                jQuery("#tbl_HistoryWorkRequestAssignmentList").jqGrid('setRowData', ids[i], { act: be + de });
            }
            //Added By Bhushan Dod on 30/03/2015 if record < 20 then pager become hide 
            if ($("#tbl_HistoryWorkRequestAssignmentList").getGridParam("records") <= 20) {
                $("#divHistoryWorkRequestAssignmentListPager").hide();
            }
            else {
                $("#divHistoryWorkRequestAssignmentListPager").show();
            }
            if ($('#tbl_HistoryWorkRequestAssignmentList').getGridParam('records') === 0) {
                $('#tbl_HistoryWorkRequestAssignmentList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div class="header_search"><input id="WorkOrdertxtSearch" class="inputSearch" placeholder="Search by Location Name" onkeydown="doWeekHistorySearch(arguments[0]||event)" type="text"></div>'
    });
    if ($("#tbl_HistoryWorkRequestAssignmentList").getGridParam("records") > 20) {
    jQuery("#tbl_HistoryWorkRequestAssignmentList").jqGrid('navGrid', '#divHistoryWorkRequestAssignmentListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }
});

var timeoutHnd;
var flAuto = true;
function doWeekHistorySearch(ev) {

    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
}

function gridReload() {

    var txtSearch = jQuery("#WorkOrdertxtSearch").val();
    jQuery("#tbl_HistoryWorkRequestAssignmentList").jqGrid('setGridParam', { url: '../GridListing/JqGridHandler/WorkOrderAssignmentByManager.ashx?UserID=' + $_UserID + "&txtSearch=" + txtSearch, page: 1 }).trigger("reloadGrid");
}

$(".EditRecord").live("click", function (event) {
    var id = $(this).attr("Id");

    var rowData = jQuery("#tbl_HistoryWorkRequestAssignmentList").getRowData(id);
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
    window.location.href = '../GlobalAdmin/Edit?id=' + id + '&WorkRequestType=' + WorkRequestType + '&AssetID=' + AssetID + '&LocationID=' + LocationID
    + '&ProblemDesc=' + ProblemDesc + '&PriorityLevel=' + PriorityLevel + '&WorkRequestImage=' + WorkRequestImage + '&SafetyHazard=' + SafetyHazard
    + '&ProjectDesc=' + ProjectDesc + '&WorkRequestStatus=' + WorkRequestStatus + '&RequestBy=' + RequestBy + '&AssignToUserId=' + AssignToUserId + '&WorkRequestProjectType=' + WorkRequestProjectType;

});


$(".deleteRecord").live("click", function (event) {

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
                jQuery("#tbl_HistoryWorkRequestAssignmentList").jqGrid().trigger("reloadGrid");
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
