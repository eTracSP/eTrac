$(function () {
    $("#tbl_LocationList").jqGrid({
        url: '../GridListing/JqGridHandler/LocationList.ashx',
        datatype: 'json',
        height: 400,
        width: 700,
        autowidth: true,
        //colNames: ['LocationName', 'Address', 'City', 'StateName', 'CountryName', 'Contact No.', 'Description', 'QRCID', 'Actions', ],
        colNames: ['LocationName', 'Address', 'City', 'StateName', 'CountryName', 'Contact No.', 'Description', 'Actions', ],
        colModel: [{ name: 'LocationName', width: 100, sortable: true },
                  { name: 'Address', width: 80, sortable: false },
                  { name: 'City', width: 80, sortable: false },
                  { name: 'StateName', width: 80, sortable: false },
                  { name: 'CountryName', width: 80, sortable: false },
                  { name: 'ContactNo', width: 80, sortable: false },
                  { name: 'Description', width: 100, sortable: false },
                  //{ name: 'QRCID', width: 80, sortable: false },
                  { name: 'act', index: 'act', width: 80, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divLocationListPager',
        sortname: 'LocationName',
        viewrecords: true,
        sortorder: 'desc',
        caption: "List of LocationName",
        emptyrecords: "No records to view",
        shrinkToFit: true,
        gridComplete: function () {

            var ids = jQuery("#tbl_LocationList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;">edit<span class="ui-icon ui-icon-pencil"></span><span class="tooltips">Edit</span></a>'
                de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;">delete<span class="ui-icon ui-icon-trash"></span><span class="tooltips">Delete</span></a>';
                vi = '<a href="javascript:void(0)" class="viewRecord" title="view" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;">view<span class="ui-icon ui-icon-disk"></span><span class="tooltips">Detail</span></a></div>';
                jQuery("#tbl_LocationList").jqGrid('setRowData', ids[i], { act: be + de });
            }
            if ($("#tbl_LocationList").getGridParam("records") <= 20) {
                $("#divLocationListPager").hide();
            }
            else {
                $("#divLocationListPager").show();
            }
            if ($('#tbl_LocationList').getGridParam('records') === 0) {
                $('#tbl_LocationList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div class="header_search"><input id="txtSearch" class="inputSearch" placeholder="Search by Location Name" onkeydown="doSearch(arguments[0]||event)" type="text"></div>'
    });
    if ($("#tbl_LocationList").getGridParam("records") > 20) {
    jQuery("#tbl_LocationList").jqGrid('navGrid', '#divLocationListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
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
    
    alert("SA");
    $("#tbl_LocationList").html("");
    var txtSearch = jQuery("#txtSearch").val();
    jQuery("#tbl_LocationList").jqGrid('setGridParam', { url: "../GridListing/JqGridHandler/LocationList.ashx?txtSearch=" + txtSearch, page: 1 }).trigger("reloadGrid");
}
//$(".EditRecord").live("click", function (event) {
//    var id = $(this).attr("Id");
//    window.location.href = '../GlobalAdmin/EditLocationSetup/?loc=' + id;
//    //$("#largeeditpopup").load('../StaffUser/EditStaffUser/' + id);
//});
$(".deleteRecord").live("click", function (event) {
     
    var id = $(this).attr("cid");
    showPopupRelativeMessage("Are you sure want to delete Location?", $(this), "Delete Location", function () {
        $.ajax({
            type: "Post",
            data: "{'id':'" + id + "'}",
            //url: '../GridListing/JqGrid Handler/StaffUserList.ashx?id=' + id,
            url: '../GlobalAdmin/DeleteLocation/',
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
                jQuery("#tbl_LocationList").jqGrid().trigger("reloadGrid");
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
