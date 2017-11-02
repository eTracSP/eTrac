$(function () {
    $("#tbl_ProjectList").jqGrid({
        url: '../GridListing/JqGridHandler/ProjectList.ashx',
        datatype: 'json',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['Project Name', 'Location Name', 'Project Category', 'Project Service', 'Description', 'LocationID', 'ProjectCategoryId', 'ProjectServicesID', 'ProjectLogoName', 'QRCID', 'Actions', ],
        colModel: [{ name: 'Location', width: 100, sortable: true },
                  { name: 'LocationName', width: 80, sortable: false },
                  { name: 'ProjectCategoryName', width: 80, sortable: false },
                  { name: 'ProjectServiceName', width: 80, sortable: false },
                  { name: 'Description', width: 80, sortable: false },
                  { name: 'LocationID', width: 80, hidden: true },
                  { name: 'ProjectCategory', width: 80, sortable: false, hidden: true },
                  { name: 'ProjectServicesID', width: 80, sortable: false, hidden: true },
                  { name: 'ProjectLogoName', width: 80, sortable: false, hidden: true },
                  { name: 'QRCID', width: 80, sortable: false, hidden: true },
                  { name: 'act', index: 'act', width: 80, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divProjectListPager',
        sortname: 'Location',
        viewrecords: true,
        sortorder: 'desc',
        caption: "List of Project",

        gridComplete: function () {

            var ids = jQuery("#tbl_ProjectList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;">edit<span class="ui-icon ui-icon-pencil"></span></a>'
                de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;">delete<span class="ui-icon ui-icon-trash"></span></a>';
                vi = '<a href="javascript:void(0)" class="viewRecord" title="view" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;">view<span class="ui-icon ui-icon-disk"></span></a></div>';
                jQuery("#tbl_ProjectList").jqGrid('setRowData', ids[i], { act: be + de });
            }
            //Added By Bhushan Dod on 30/03/2015 if record < 20 then pager become hide 
            if ($("#tbl_ProjectList").getGridParam("records") <= 20) {
                $("#divProjectListPager").hide();
            }
            else {
                $("#divProjectListPager").show();
            }
            if ($('#tbl_ProjectList').getGridParam('records') === 0) {
                $('#tbl_ProjectList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div class="header_search"><input id="txtSearch" class="inputSearch" placeholder="Search by Project Name" onkeydown="doSearch(arguments[0]||event)" type="text"></div>'
    });
    if ($("#tbl_ProjectList").getGridParam("records") > 20) {
    jQuery("#tbl_ProjectList").jqGrid('navGrid', '#divProjectListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
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
    jQuery("#tbl_ProjectList").jqGrid('setGridParam', { url: "../GridListing/JqGridHandler/ProjectList.ashx?txtSearch=" + txtSearch, page: 1 }).trigger("reloadGrid");
}
$(".EditRecord").live("click", function (event) {
    var id = $(this).attr("Id");
    //data: JSON.stringify({ 'proj': id }),
    var rowData = jQuery("#tbl_ProjectList").getRowData(id);
    var Location = rowData['Location'];
    var Description = rowData['Description'];
    var LocationID = rowData['LocationID'];
    var ProjectCategory = rowData['ProjectCategory'];
    var ProjectServicesID = rowData['ProjectServicesID'];
    var ProjectLogoName = rowData['ProjectLogoName'];
    var QRCID = rowData['QRCID'];
    //    alert("stop");
    

    window.location.href = '../GlobalAdmin/EditProject/?proj=' + id + '&Location=' + Location + '&Description=' + Description + '&LocationID=' + LocationID + '&ProjectCategory=' + ProjectCategory + '&ProjectServicesID=' + ProjectServicesID + '&ProjectLogoName=' + ProjectLogoName + '&QRCID=' + QRCID;
    //$("#largeeditpopup").load('../StaffUser/EditStaffUser/' + id);


});
// window.location.href = '../GlobalAdmin/EditProject/' + id;
//$("#largeeditpopup").load('../StaffUser/EditStaffUser/' + id);


$(".deleteRecord").live("click", function (event) {
    var id = $(this).attr("cid");
    showPopupRelativeMessage("Are you sure want to delete ?", $(this), "Delete Project", function () {
        $.ajax({
            type: "Post",
            data: "{'proj':'" + id + "'}",
            //url: '../GridListing/JqGrid Handler/StaffUserList.ashx?id=' + id,
            url: '../GlobalAdmin/DeleteProject/' ,
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
                jQuery("#tbl_ProjectList").jqGrid().trigger("reloadGrid");
                event.stopPropagation();
            },
            Complete: function (result) {
                closeAjaxProgress();
                event.stopPropagation();
            }
        });
    });
});