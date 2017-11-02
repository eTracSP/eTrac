$(function () { 
    $("#tbl_WorkRequestList").jqGrid({
        url: '../GridListing/JqGridHandler/WorkRequestList.ashx?ProjectID=' + $_ProjectID + '&UserID=' + $_UserID,
        datatype: 'json',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['Task Name','Task Priority', 'Task Priority', 'RequestBy', 'Request By', 'WorkArea', 'AreaName',
                  'StartTime', 'CompletionTime', 'AssignedToUser', 'Assigned To', 'TaskStatus', 'ProjectId', 'Remarks', 'Task Status', 'WorkOrderID', 'AssetID', 'Asset No', 'Actions', ],
        //colNames: ['Task Name', 'TaskType', 'Task Type', 'Task Priority', 'Task Priority', 'RequestBy', 'Request By', 'WorkArea', 'AreaName',
        //           'StartTime', 'CompletionTime', 'AssignedToUser', 'Assigned To', 'TaskStatus', 'ProjectId', 'Remarks', 'Task Status', 'WorkOrderID', 'Actions', ],
                  colModel: [{ name: 'TaskName', width: 100, sortable: true },
                  //{ name: 'TaskType', width: 80, sortable: false, hidden: true },
                  //{ name: 'TaskTypeName', width: 80, sortable: false },
                  { name: 'TaskPriority', width: 80, sortable: false, hidden: true },
                  { name: 'TaskPriorityName', width: 80, sortable: false },
                  { name: 'RequestBy', width: 80, sortable: false, hidden: true },
                  { name: 'RequestByName', width: 100, sortable: false },
                  { name: 'WorkArea', width: 100, sortable: false, hidden: true, hidden: true },
                  { name: 'AreaName', width: 100, sortable: false, hidden: true },
                  { name: 'StartTime', width: 100, sortable: false, hidden: true },
                  { name: 'CompletionTime', width: 100, sortable: false, hidden: true },
                  { name: 'AssignedToUser', width: 100, sortable: false, hidden: true, hidden: true },
                  { name: 'AssignedToUserName', width: 100, sortable: false },
                  { name: 'TaskStatus', width: 100, sortable: false, hidden: true },
                  { name: 'ProjectId', width: 100, sortable: false, hidden: true },
                  { name: 'Remarks', width: 100, sortable: false, hidden: true },
                  { name: 'TaskStatusName', width: 100, sortable: false },
                  { name: 'WorkOrderID', width: 100, sortable: false, hidden: true },
                  { name: 'AssetID', width: 100, sortable: false, hidden: true },
                  { name: 'AssetNo', width: 100, sortable: false},
                  { name: 'act', index: 'act', width: 80, sortable: false }],
                  //{ name: 'act', index: 'act', width: 80, sortable: false, hidden: (($_UserID != undefined && (parseInt($_UserID) > 0)) ? true : false) }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divWorkRequestListPager',
        sortname: 'ItemName',
        viewrecords: true,
        sortorder: 'desc',
        caption: "List of Work Request",

        gridComplete: function () {
          
            var vact;
            var ids = jQuery("#tbl_WorkRequestList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                
                var cl = ids[i];
                vact = '';
                var rowData = jQuery("#tbl_WorkRequestList").getRowData(cl);
                var WorkOrderID = rowData['WorkOrderID'];
                var TaskStatus = rowData['TaskStatusName'];
                if ($_UserID == undefined || (parseInt($_UserID) == 0) || $_UserID == '') {
                 
                    if (WorkOrderID == 0) {
                  
                        be = '<div><a href="javascript:void(0)" data-toggle = "modal" data-target = "#myModallarge" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;">Assign<span class="ui-icon ui-icon-pencil"></span></a>'
                
                        de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;">delete<span class="ui-icon ui-icon-trash"></span></a>';
                        vact = be + de;
                    }
                    else {
                        vi = '<a href="javascript:void(0)" data-toggle = "modal" data-target = "#myModallarge"  class="viewRecord" title="view" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;">view<span class="ui-icon ui-icon-disk"></span></a></div>';
                        vact = vi;
                    }
                }
                else {
                    
                    if (TaskStatus != 'Pending') {
                        de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;">delete<span class="ui-icon ui-icon-trash"></span></a>';
                        vact = de;
                    }
                    else
                    {
                        //vi = '<a href="javascript:void(0)" data-toggle = "modal" data-target = "#myModallarge"  class="viewRecord" title="view" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;">view<span class="ui-icon ui-icon-disk"></span></a></div>';
                        $("#tbl_WorkRequestList").hideCol("act");
                    }
                }

                jQuery("#tbl_WorkRequestList").jqGrid('setRowData', ids[i], { act: vact });
            }
        },
        loadComplete: function () {
            if ($('#tbl_WorkRequestList').getGridParam('records') === 0) {
                $('#tbl_WorkRequestList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div class="header_search"><input id="txtSearch" class="inputSearch" placeholder="Search by Task Name" onkeydown="doSearch(arguments[0]||event)" type="text"></div>'
    });
    jQuery("#tbl_WorkRequestList").jqGrid('navGrid', '#divWorkRequestListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });

});

var timeoutHnd;
var flAuto = true;
function doSearch(ev) {

    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
}

function gridReload() {

    var txtSearch = jQuery("#txtSearch").val();
    jQuery("#tbl_WorkRequestList").jqGrid('setGridParam', { url: "../GridListing/JqGridHandler/WorkRequestList.ashx?txtSearch=" + txtSearch + '&ProjectID=' + $_ProjectID + '&UserID =' + $_UserID, page: 1 }).trigger("reloadGrid");
}
$(".EditRecord").live("click", function (event) {

    var id = $(this).attr("Id");
    var rowData = jQuery("#tbl_WorkRequestList").getRowData(id);
    var TaskName = rowData['TaskName'];
    var TaskPriority = rowData['TaskPriority'];
    var RequestBy = rowData['RequestBy'];
    var WorkArea = rowData['WorkArea'];
    var StartTime = rowData['StartTime'];
    var CompletionTime = rowData['CompletionTime'];
    var AssignedToUser = rowData['AssignedToUser'];
    var ProjectId = rowData['ProjectId'];
    var Remarks = rowData['Remarks'];
    var TaskStatusName = rowData['TaskStatusName'];
    var WorkOrderID = rowData['WorkOrderID'];
    var AssetID = rowData['AssetID'];
    // var TaskType = rowData["TaskType"];

    //window.location.href = '../Manager/WorkOrder/?id=' + id + '&TaskName=' + TaskName + '&TaskPriority=' + TaskPriority + '&RequestBy=' + RequestBy + '&WorkArea=' + WorkArea + '&StartTime=' + StartTime + '&CompletionTime=' + CompletionTime
    //    + '&AssignedToUser=' + AssignedToUser + '&ProjectId=' + ProjectId + '&Remarks=' + Remarks + '&WorkOrderID=' + WorkOrderID;
    //$("#largeeditpopup").load('../StaffUser/EditStaffUser/' + id);


    $('#largeeditpopup').load("../Manager/WorkOrder", {
        'id': id, 'TaskName': TaskName, 'TaskPriority': TaskPriority, 'RequestBy': RequestBy, 'WorkArea': WorkArea, 'StartTime': StartTime, 'CompletionTime': CompletionTime,
        'AssignedToUser': AssignedToUser, 'ProjectId': ProjectId, 'Remarks': Remarks, 'WorkOrderID': WorkOrderID, 'AssetID': AssetID// 'TaskType': TaskType

    }, function () {
        $("form").removeData("validator");
        $("form").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse("form");
        $('.modal-title').text("Assign Work Order");
        //$("#largeeditpopup form").validate();

    });

});
$(".viewRecord").live("click", function (event) {
    
    var id = $(this).attr("vid");
    var rowData = jQuery("#tbl_WorkRequestList").getRowData(id);
    var TaskName = rowData['TaskName'];
    var TaskTypeName = rowData['TaskTypeName'];
    var TaskPriorityName = rowData['TaskPriorityName'];
    var RequestByName = rowData['RequestByName'];
    var AreaName = rowData['AreaName'];
    var AssignedToUserName = rowData['AssignedToUserName'];
    var TaskStatusName = rowData['TaskStatusName'];
    var Remarks = rowData['Remarks'];
   
    $('#largeeditpopup').load("../Manager/WorkOrderDetails", {
        'TaskName': TaskName, 'TaskPriority': TaskPriorityName, 'RequestBy': RequestByName, 'WorkArea': AreaName, 'TaskType': TaskTypeName, 'AssignedUser': AssignedToUserName,
        'TaskStatus': TaskStatusName, 'remarks': Remarks
    }, function () {
        $('.modal-title').text("Work Order Details");
        //$("#largeeditpopup form").validate();

    });

});
$(".deleteRecord").live("click", function (event) {
    var id = $(this).attr("cid");
    showPopupRelativeMessage("Are you sure want to delete ?", $(this), "Delete Work Request", function () {
        $.ajax({
            type: "Post",
            data: "{'id':'" + id + "'}",
            //url: '../GridListing/JqGrid Handler/StaffUserList.ashx?id=' + id,
            url: '../Manager/DeleteWorkRequest/',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            error: function (xhr, status, error) {
                closeAjaxProgress();
            },
            beforeSend: function () {
                showAjaxProgress();
            },
            success: function (result) {
                //  AlertMessage(result);
                $("#message").html(result.Message);
                $("#message").addClass(result.AlertMessageClass);
                toastr.success(result.Message)
                closeAjaxProgress();
                jQuery("#tbl_WorkRequestList").jqGrid().trigger("reloadGrid");
                event.stopPropagation();
            },
            Complete: function (result) {
                closeAjaxProgress();
                event.stopPropagation();
            }
        });
    });
});