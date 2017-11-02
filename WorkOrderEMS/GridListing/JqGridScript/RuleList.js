$(function () {
    $("#tbl_RuleList").jqGrid({
        url: '../GridListing/JqGridHandler/RuleMasterList.ashx?ProjectID=' + $_ProjectID,
        datatype: 'json',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['Rule Name', 'Voilation Charges', 'Description', 'IsActive', 'Actions', ],
        colModel: [{ name: 'RuleName', width: 100, sortable: true },
                  { name: 'VoilationCharges', width: 50, sortable: false },
                  { name: 'Description', width: 130, sortable: false},
                  { name: 'IsActive', width: 30, sortable: false },                 
                  { name: 'act', index: 'act', width: 40, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divRuleListPager',
        sortname: 'CreatedDate',
        viewrecords: true,
        sortorder: 'desc',
        caption: "List of Rule",

        gridComplete: function () {

            var ids = jQuery("#tbl_RuleList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {             
               
                var cl = ids[i];
                be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span></a>'
                de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span></a>'
               
                jQuery("#tbl_RuleList").jqGrid('setRowData', ids[i], { act: be + de });
            }
            //Added By Bhushan Dod on 30/03/2015 if record < 20 then pager become hide 
            if ($("#tbl_RuleList").getGridParam("records") <= 20) {
                $("#divRuleListPager").hide();
            }
            else {
                $("#divRuleListPager").show();
            }
            if ($('#tbl_RuleList').getGridParam('records') === 0) {
                $('#tbl_RuleList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },

        caption: '<div class="header_search"><input id="txtSearch" class="inputSearch" placeholder="Search by Rule Name" onkeydown="doSearch(arguments[0]||event)" type="text"></div>'
    });
    if ($("#tbl_RuleList").getGridParam("records") > 20) {
        jQuery("#tbl_RuleList").jqGrid('navGrid', '#divRuleListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
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
    jQuery("#tbl_RuleList").jqGrid('setGridParam', { url: "../GridListing/JqGridHandler/RuleMasterList.ashx?txtSearch=" + txtSearch.trim() +"&ProjectID=" + $_ProjectID, page: 1 }).trigger("reloadGrid");
}
$(".EditRecord").live("click", function (event) {

    var id = $(this).attr("Id");
    var rowData = jQuery("#tbl_RuleList").getRowData(id);
    var RuleName = rowData['RuleName'];
    var VoilationCharges = rowData['VoilationCharges'];
    var Description = rowData['Description'];
    var IsActive = rowData['IsActive'];


    window.location.href = '../Manager/EditRule/?id=' + id;

    //$("#largeeditpopup").load('../StaffUser/EditStaffUser/' + id);
});

$(".deleteRecord").live("click", function (event) {
    var id = $(this).attr("cid");
    showPopupRelativeMessage("Are you sure want to delete ?", $(this), "Delete Rule", function () {
        $.ajax({
            type: "Post",
            data: "{'id':'" + id + "'}",
            //url: '../GridListing/JqGrid Handler/StaffUserList.ashx?id=' + id,
            url: '../Manager/DeleteRule/',
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
                jQuery("#tbl_RuleList").jqGrid().trigger("reloadGrid");
                event.stopPropagation();
            },
            Complete: function (result) {
                closeAjaxProgress();
                event.stopPropagation();
            }
        });
    });
});