//var QRCurl = '../QRCSetup/GetRegisteredVehicle';
//editurl='../QRCSetup/Edit/;
var QRCurl = '../Vehicle/GetRegisteredVehicle';
var editurl = '../QRCSetup/VehicleRegistration/';
var deleteURL = '../QRCSetup/DeleteQRC/';

//var editurl = '../Vehicle/VehicleRegistration/';
$(function () {
    //alert('GetRegisteredVehicle');
    $("#tbl_QRCList").jqGrid({
        //url: '../QRCSetup/GetQRCList',
        //url: '../QRCSetup/GetRegisteredVehicle',
        url: QRCurl,
        datatype: 'json',
        type: 'POST',
        height: 400,
        width: 700,
        autowidth: true,
        //colNames: ['Item Name', 'Item Type', 'Item Special Notes', 'Actions', ],        //colNames: ['QRCName', 'QRCTYPE', 'SpecialNotes',  'Actions', ],
        colNames: ['Vehicle Identification No', 'Vendor', 'License No', 'Insurance Plan', 'Driver Name', 'Actions', ],
        colModel: [{ name: 'VehicleIdentificationNo', width: 80, sortable: true },
                  { name: 'Vendor', width: 100, sortable: false },
                  { name: 'LicenseNo', width: 50, sortable: false },

                  { name: 'InsurancePlan', width: 80, sortable: false },
                  { name: 'DriverName', width: 80, sortable: false },
                  //{ name: 'VehicleId', width: 10, sortable: false },

                  { name: 'act', index: 'act', width: 80, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divQRCListPager',
        sortname: 'QRCName',
        viewrecords: true,
        sortorder: 'desc',
        caption: "List of QRC",

        gridComplete: function () {

            var ids = jQuery("#tbl_QRCList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;">edit<span class="ui-icon ui-icon-pencil"></span></a>'
                de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;">delete<span class="ui-icon ui-icon-trash"></span></a>';
                vi = '<a href="javascript:void(0)" class="viewRecord" title="view" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;">view<span class="ui-icon ui-icon-disk"></span></a></div>';
                jQuery("#tbl_QRCList").jqGrid('setRowData', ids[i], { act: be + de });
            }
        },
        caption: '<div class="header_search"><input id="SearchText" class="inputSearch" placeholder="Search by Item Name" onkeydown="doSearch(arguments[0]||event)" type="text"></div>'
    });
    jQuery("#tbl_QRCList").jqGrid('navGrid', '#divQRCListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });

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
    jQuery("#tbl_QRCList").jqGrid('setGridParam', { url: QRCurl + "?SearchText=" + txtSearch.trim(), page: 1 }).trigger("reloadGrid");
}
$(".EditRecord").live("click", function (event) {
    var id = $(this).attr("Id");
    debugger;
    window.location.href = editurl + '?qr=' + id;
    //$("#largeeditpopup").load('../StaffUser/EditStaffUser/' + id);
});
$(".deleteRecord").live("click", function (event) {

    var id = $(this).attr("cid");
    showPopupRelativeMessage("Are you sure want to delete Item?", $(this), "Delete Item", function () {
        $.ajax({
            type: "POST",
            data: "{'id':'" + id + "'}",
            //url: '../GridListing/JqGrid Handler/StaffUserList.ashx?id=' + id,
            url: deleteURL,
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
                jQuery("#tbl_QRCList").jqGrid().trigger("reloadGrid");
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
