$(function () {
    $("#tbl_AssignedInventoryList").jqGrid({
        url: '../GridListing/JqGridHandler/AssignedInventoryList.ashx?ProjectID=' + $_ProjectID,
        datatype: 'json',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['AssignInventoryID', 'InventoryID', 'AssignedUserID', 'AssignedUser', 'AssignedQuantity', 'IssueDate', 'IssuedBy', 'IssuedUser', 'ReturnDate', 'Actions', ],
        colModel: [{ name: 'AssignInventoryID', width: 80, sortable: true },
                  { name: 'InventoryID', width: 80, sortable: true },
                  { name: 'AssignedUserID', width: 80, sortable: false, hidden: true },
                  { name: 'AssignedUser', width: 100, sortable: false },
                  { name: 'AssignedQuantity', width: 80, sortable: false},
                  { name: 'IssueDate', width: 100, sortable: false },
                  { name: 'IssuedBy', width: 100, sortable: false, hidden: true },
                  { name: 'IssuedUser', width: 100, sortable: false },
                  { name: 'ReturnDate', width: 100, sortable: false },
                  { name: 'act', index: 'act', width: 80, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divAssignedInventoryListPager',
        sortname: 'InventoryID',
        viewrecords: true,
        sortorder: 'desc',
        caption: "List of Assigned Inventory",

        gridComplete: function () {
            var ids = jQuery("#tbl_AssignedInventoryList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                ai = '';
                var cl = ids[i];
                var rowData = jQuery("#tbl_AssignedInventoryList").getRowData(cl);
                var ReturnDate = rowData['ReturnDate'];
                var AssignInventoryID = rowData['AssignInventoryID'];

                be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="ui-icon ui-icon-pencil"></span></a>'
                de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="ui-icon ui-icon-trash"></span></a>'
                if (AssignInventoryID == 0) {
                    ai = '<div><a href="javascript:void(0)" data-toggle = "modal" data-target = "#myModallarge" class="Assign" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;">Assign<span class="ui-icon ui-icon-pencil"></span></a>';
                }
                jQuery("#tbl_AssignedInventoryList").jqGrid('setRowData', ids[i], { act: be + de + ai });
            }
            //$("#refresh_tbl_InventoryList").remove("#removeMe");
            $("#removeMe").remove();
            $("#refreshtbl_AssignedInventoryList").append('<input id="removeMe" type="button" value="Export PDF" onclick="exportPDF();" />');
            //Added By Bhushan Dod on 30/03/2015 if record < 20 then pager become hide 
            if ($("#tbl_AssignedInventoryList").getGridParam("records") <= 20) {
                $("#divAssignedInventoryListPager").hide();
            }
            else {
                $("#divAssignedInventoryListPager").show();
            }
            if ($('#tbl_AssignedInventoryList').getGridParam('records') === 0) {
                $('#tbl_AssignedInventoryList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div class="header_search"><input id="txtSearch" class="inputSearch" placeholder="Search by Item Name" onkeydown="doSearch(arguments[0]||event)" type="text"></div>'
    });
    if ($("#tbl_AssignedInventoryList").getGridParam("records") > 20) {
    jQuery("#tbl_AssignedInventoryList").jqGrid('navGrid', '#divAssignedInventoryListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }

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
    jQuery("#tbl_AssignedInventoryList").jqGrid('setGridParam', { url: "../GridListing/JqGridHandler/AssignedInventoryList.ashx?txtSearch=" + txtSearch, page: 1 }).trigger("reloadGrid");
}