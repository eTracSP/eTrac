/*ListQRC.js*/
/*Created By    : Roshan Rahood*/
/*Created On    : Feb-24-2015*/

var EmailListurl = '../GridListing/JqGridHandler/EmailList.ashx';
var deleteurl = '../EmailDetails/DeleteEmail/';

$(function () {

    $("#tbl_EmailList").jqGrid({
 
        url: '../GridListing/JqGridHandler/EmailList.ashx',
        datatype: 'json',
        type: 'POST',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['Email Sent By','Email Sent To', 'Subject', 'Sent On','Actions'],       
        colModel: [{ name:'Email Sent By', width: 60, sortable: true },
                  { name: 'Email Sent To', width: 60, sortable: true },
                  { name: 'Subject', width: 220, sortable: false },
                  { name: 'Sent On', width: 65, sortable: false },
                  { name: 'act', index: 'act', width: 30, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divEmailListPager',
        sortname: 'SentEmail',
        viewrecords: true,
        sortorder: 'desc',
        caption: "List of Email",

        gridComplete: function () {

            var ids = jQuery("#tbl_EmailList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span></span><span class="tooltips">Delete</span></a>';
                jQuery("#tbl_EmailList").jqGrid('setRowData', ids[i], { act: de });
            }
            //Added By Bhushan Dod on 30/03/2015 if record < 20 then pager become hide 
            if ($("#tbl_EmailList").getGridParam("records") <= 20) {
                $("#divEmailListPager").hide();
            }
            else {
                $("#divEmailListPager").show();
            }
            if ($('#tbl_EmailList').getGridParam('records') === 0) {
                $('#tbl_EmailList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },

        caption: '<div class="header_search"><input id="SearchText" class="inputSearch" placeholder="Search Sent By or Sent To" onkeydown="doSearch(arguments[0]||event)" type="text">&nbsp;<div class="onoffswitch2 ViewAllButton ViewAllButton_email"><input type="checkbox" name="onoffswitch2" class="onoffswitch2-checkbox " id="ViewAllLocation"><label for="ViewAllLocation" class="onoffswitch2-label"><span class="onoffswitch2-inner"></span><span class="onoffswitch2-switch"></span></label></div></div>'
    });
    $('#ViewAllLocation').change(function () {
        ViewAllRecords();
    });

    if ($("#tbl_EmailList").getGridParam("records") > 20) {
    jQuery("#tbl_EmailList").jqGrid('navGrid', '#divEmailListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
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
    var txtSearch = jQuery("#SearchText").val();
    var locaId = $('#ViewAllLocation').prop('checked') == true ? 0 : $("#drp_MasterLocation :selected").val();

    if (locaId == 0) {
        $("#drp_MasterLocation").hide();
    }
    else {
        $("#drp_MasterLocation").show();
    }
    jQuery("#tbl_EmailList").jqGrid('setGridParam', { url: EmailListurl + "?txtSearch=" + txtSearch + "&locationId=" + locaId, page: 1 }).trigger("reloadGrid");
    //jQuery("#tbl_EmailList").jqGrid('setGridParam', { url: EmailListurl + "?txtSearch=" + txtSearch.trim(), page: 1 }).trigger("reloadGrid");
}

function ViewAllRecords() {
    var txtSearch = jQuery("#SearchText").val();
    var locaId = $('#ViewAllLocation').prop('checked') == true ? 0 : $("#drp_MasterLocation :selected").val();

    if (locaId == 0) {
        $("#drp_MasterLocation").hide();
    }
    else {
        $("#drp_MasterLocation").show();
    }

    jQuery("#tbl_EmailList").jqGrid('setGridParam', { url: EmailListurl + "?txtSearch=" + txtSearch + "&locationId=" + locaId, page: 1 }).trigger("reloadGrid");
}

$(".EditRecord").live("click", function (event) {
    var id = $(this).attr("Id");
    window.location.href = editurl + '?qr=' + id;
    //$("#largeeditpopup").load('../StaffUser/EditStaffUser/' + id); 
});
$(".deleteRecord").live("click", function (event) {

    var id = $(this).attr("cid");
    showPopupRelativeMessage("Are you sure want to delete Email?", $(this), "Delete Email", function () {
        $.ajax({
            type: "POST",
            data: "{'logId':'" + id + "'}",
            url: deleteurl,
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
                jQuery("#tbl_EmailList").jqGrid().trigger("reloadGrid");
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
