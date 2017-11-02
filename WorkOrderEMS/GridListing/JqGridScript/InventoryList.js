
var ProjectID = 0;
var InventoryList = ''
    + '<select id="InventoryListddl" class="" onchange="doSearch(arguments[0]||event);">'
    //+ testqRCTypeddl + '</select>';
+ '<option value="196">New Inventory</option>'
+ '<option value="197">Assigned Inventory</option> </select>';

var ItemOwn = ''
    + '<select id="ItemOwnddl" class=""  onchange="doSearch(arguments[0]||event);">'
    //+ testqRCTypeddl + '</select>';
/**/ + '<option value="0" >All</option>'
+ '<option value="194">Company Owned</option>'
+ '<option value="195">Client Owned</option> </select>';

$(function () {
    $("#tbl_InventoryList").jqGrid({
        url: '../GridListing/JqGridHandler/InventoryList.ashx?ProjectID=' + $_ProjectID,
        datatype: 'json',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['Item Name', 'Item Serial Number', 'ItemType', 'ItemTypeName', 'Description', 'Quantity', 'AssginedQuantity', 'ProjectID', 'AssignInventoryID', 'AssignedUserID', 'IssueDate', 'IssuedBy', 'AssignedTo', 'ReturnDate', 'ItemOwnership','Actions', ],
        colModel: [{ name: 'ItemName', width: 100, sortable: true },
                  { name: 'ItemCode', width: 100, sortable: false },
                  { name: 'ItemType', width: 80, sortable: false,hidden:true },
                  { name: 'ItemTypeName', width: 80, sortable: false },
                  { name: 'Description', width: 100, sortable: false,hidden:true },
                  { name: 'Quantity', width: 50, sortable: false, hidden: false },
                  { name: 'AssginedQuantity', width: 80, sortable: false, hidden: true },
                  { name: 'ProjectID', width: 100, sortable: false, hidden: true },
                  { name: 'AssignInventoryID', width: 100, sortable: false, hidden: true },
                  { name: 'AssignedUserID', width: 100, sortable: false, hidden: true },
                  { name: 'IssueDate', width: 100, sortable: false, hidden: true },
                  { name: 'IssuedBy', width: 100, sortable: false, hidden: true },
                  { name: 'AssignedToName', width: 100, sortable: false ,hidden: true },
                  { name: 'ReturnDate', width: 100, sortable: false, hidden: true },
                  { name: 'ItemOwnership', width: 100, sortable: false, hidden: true },
                  { name: 'act', index: 'act', width: 50, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divInventoryListPager',
        sortname: 'ItemName',
        viewrecords: true,
        sortorder: 'desc',
        caption: "List of Inventory",

        gridComplete: function () {         
            var ids = jQuery("#tbl_InventoryList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {               
                var cl = ids[i];
                var rowData = jQuery("#tbl_InventoryList").getRowData(cl);
                var ReturnDate = rowData['ReturnDate'];
                var AssignInventoryID = rowData['AssignInventoryID'];
                var Quantity = rowData['Quantity'];
              
                be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span></a>'
                de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span></a>'
                ai = '<div><a href="javascript:void(0)" data-toggle = "modal" data-target = "#myModallarge" class="Assign" Id="' + cl + '" title="assign" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-file-text fa-2x texthover-bluelight"></span></a>';
                jQuery("#tbl_InventoryList").jqGrid('setRowData', ids[i], { act: be + de + ai });
                //Added By Bhushan Dod on 30/03/2015 if record < 20 then pager become hide 
                //if ($("#tbl_InventoryList").getGridParam("records") <= 20) {
                //    $("#divInventoryListPager").hide();
                //}
                //else {
                //    $("#divInventoryListPager").show();
                //}

            }
            $("#removeMe").remove();
            $("#refresh_tbl_InventoryList").append('<input id="removeMe" type="button" value="Export PDF" onclick="exportPDF();" />&nbsp');
            $("#btnExcel").remove();
            $("#refresh_tbl_InventoryList").append('<input id="btnExcel" type="button" value="Export Excel" onclick="exportExcel();" />');
            if ($('#tbl_InventoryList').getGridParam('records') === 0) {
                $('#tbl_InventoryList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },

        caption: '<div class="header_search"><input id="txtSearch" class="inputSearch" placeholder="Search by Item Name" onkeydown="doSearch(arguments[0]||event)" type="text"><label id="ListType" value="ListType"/>' + InventoryList + ' '+ItemOwn + '</div>'
    });
    //if ($("#tbl_InventoryList").getGridParam("records") > 20) {
    jQuery("#tbl_InventoryList").jqGrid('navGrid', '#divInventoryListPager',  { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    //}
});

var timeoutHnd;
var flAuto = true;
function doSearch(ev) {
    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
}


function gridReload() {
    var ProjectID = 0;
    var txtSearch = jQuery("#txtSearch").val();
    var InventoryType = jQuery("#InventoryListddl").val();
    var ItemOwn = jQuery("#ItemOwnddl").val();
    var ProjectID = jQuery("#ProjectID").val();
    $("#InventoryTypeHidden").val(InventoryType);
    if (InventoryType == "197")
    {
        $("#tbl_InventoryList").hideCol("Quantity"); $("#tbl_InventoryList").showCol("AssginedQuantity");
        $("#tbl_InventoryList").hideCol("act"); $("#tbl_InventoryList").showCol("AssignedToName");
    }
    else
    {
        $("#tbl_InventoryList").hideCol("AssginedQuantity"); $("#tbl_InventoryList").showCol("Quantity");
        $("#tbl_InventoryList").hideCol("AssignedToName"); $("#tbl_InventoryList").showCol("act");
    }
    //jQuery("#tbl_InventoryList").jqGrid('setGridParam', { url: '../GridListing/JqGridHandler/InventoryList.ashx?txtSearch=' + txtSearch, page: 1 }).trigger("reloadGrid");
    jQuery("#tbl_InventoryList").jqGrid('setGridParam', { url: '../GridListing/JqGridHandler/InventoryList.ashx?txtSearch=' + txtSearch + '&ProjectID=' + ProjectID + '&InventoryType=' + InventoryType + '&ItemOwn=' + ItemOwn, page: 1 }).trigger("reloadGrid");
}
$(".EditRecord").live("click", function (event) {
    var id = $(this).attr("Id");
    //var rowData = jQuery("#tbl_InventoryList").getRowData(id);
    //var ItemName = rowData['ItemName'];
    //var ItemCode = rowData['ItemCode'];
    //var ItemType = rowData['ItemType'];
    //var ItemTypeName = rowData['ItemTypeName'];
    //var Description = rowData['Description'];
    //var Quantity = rowData['Quantity'];
    //var ProjectID = rowData['ProjectID'];
    //var ItemOwnership = rowData['ItemOwnership'];

    window.location.href = '../Manager/EditInventory/?id=' + id;
                           
    //$("#largeeditpopup").load('../StaffUser/EditStaffUser/' + id);
});

$(".Assign").live("click", function (event) {
   
    var id = $(this).attr("Id");
    var rowData = jQuery("#tbl_InventoryList").getRowData(id);
    //var ItemName = rowData['ItemName'];
    //var AssignInventoryID = rowData['AssignInventoryID'];
    //var AssignedUserID = rowData['AssignedUserID'];
    //var IssueDate = rowData['IssueDate'];
    //var IssuedBy = rowData['IssuedBy'];
    //var AssignedToName = rowData['AssignedToName'];
    var Quantity = rowData['Quantity'];
   // var Quantity = rowData['AssignedQuantity'];
    //window.location.href = '../Manager/AssignInventory/?id=' + id + '&AssignInventoryID=' + AssignInventoryID + '&AssignedUserID=' + AssignedUserID + '&IssueDate=' + IssueDate + '&IssuedBy=' + IssuedBy + '&AssignedToName=' + AssignedToName;

    $('#largeeditpopup').load("../Manager/_AssignInventory", { 'id': id, 'Quantity': Quantity }
       , function () {
           var d = new Date();
           var curr_year = d.getFullYear();
           $("#IssueDate").datepicker({});
           $("form").removeData("validator");
           $("form").removeData("unobtrusiveValidation");
           $.validator.unobtrusive.parse("form");
           $('.modal-title').text("Assign Inventory");
       });
});

$(".deleteRecord").live("click", function (event) {
    var id = $(this).attr("cid");
    showPopupRelativeMessage("Are you sure want to delete ?", $(this), "Delete Inventory", function () {
        $.ajax({
            type: "Post",
            data: "{'id':'" + id + "'}",
            //url: '../GridListing/JqGrid Handler/StaffUserList.ashx?id=' + id,
            url: '../Manager/DeleteInventory/',
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
                closeAjaxProgress();
                jQuery("#tbl_InventoryList").jqGrid().trigger("reloadGrid");
                event.stopPropagation();
            },
            Complete: function (result) {
                closeAjaxProgress();
                event.stopPropagation();
            }
        });
    });
});
