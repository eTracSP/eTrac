var fuelurl = 'fueling/GetFuelingList';
var vehcileApprovalStatus = ''
//+ '<select id="vehcileStatusType" class="" onchange="doSearch(arguments[0]||event);">'
//+ '<option value="0">All Approved</option>'
//+ '<option value="244">Approved By Manager</option>'
////+ '<option value="0">Pending</option>'
//+ '</select>';
$(function () {
    $("#tbl_FuelingList").jqGrid({
        url: $_HostPrefix + fuelurl,
        datatype: 'json',
        type: 'GET',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['Vehicle Number', 'QRCodeID', 'Mileage', 'Current Fuel', 'Fuel Type', 'Receipt No', 'Fueling Date', 'Gallons', 'PricePerGallon', 'Total', 'Gas Station Name', 'Card No', 'Driver Name'],
        colModel: [{ name: 'VehicleNumber', width: 30, sortable: true },
        { name: 'QRCodeID', width: 40, sortable: true },
        { name: 'Mileage', width: 40, sortable: false },
        { name: 'CurrentFuel', width: 40, sortable: true },
        { name: 'FuelTypeName', width: 40, sortable: true },
        { name: 'ReceiptNo', width: 30, sortable: true },
        { name: 'FuelingDate', width: 30, sortable: false },
        { name: 'Gallons', width: 30, sortable: true },
        { name: 'PricePerGallon', width: 30, sortable: true },
        { name: 'Total', width: 30, sortable: true },
        { name: 'GasStatioName', width: 30, sortable: true },
        { name: 'CardNo', width: 30, sortable: true },
        { name: 'DriverName', width: 30, sortable: true }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divFuelingListPager',
        sortname: 'FuelingDate',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        //emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        caption: "List of Fueling",
        gridComplete: function () {
            var ids = jQuery("#tbl_FuelingList").jqGrid('getDataIDs');
            //for (var i = 0; i < ids.length; i++) {
            //    var cl = ids[i];
            //    be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
            //    de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a></div>';
            //    jQuery("#tbl_PassengerTrackingRouteList").jqGrid('setRowData', ids[i], { act: be + de }); //+ qrc 
            //}           
            if ($("#tbl_FuelingList").getGridParam("records") <= 20) {
                $("#divFuelingListPager").hide();
            }
            else {
                $("#divFuelingListPager").show();
            }
            if ($('#tbl_FuelingList').getGridParam('records') === 0) {
                $('#tbl_FuelingList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div class="header_search"><input id="SearchText" class="inputSearch" placeholder="Search by vehicle no" onkeydown="doSearch(arguments[0]||event)" type="text">&nbsp;' + vehcileApprovalStatus + '&nbsp;</div>'
    });
    if ($("#tbl_FuelingList").getGridParam("records") > 20) {
        jQuery("#tbl_FuelingList").jqGrid('navGrid', '#divFuelingListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
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
    //var statusType = jQuery("#serviceTypeStatus :selected").val();
    jQuery("#tbl_FuelingList").jqGrid('setGridParam', { url: $_HostPrefix + fuelurl + "?txtSearch=" + txtSearch.trim(), page: 1 }).trigger("reloadGrid");
}