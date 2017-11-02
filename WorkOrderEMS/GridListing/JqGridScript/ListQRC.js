/*ListQRC.js*/
/*Created By    :   Nagendra Upwanshi*/
/*Created On    :   Nov-10-2014*/
/*Modified by   : Vijay Sahu 18 march 2015*/
/*Modified by   : Bhushan Dod 27 July 2015 for Listing according to location and QRCCodeId*/


//var QRCurl = '../QRCSetup/GetRegisteredVehicle';
var editurl = '../QRCSetup/Edit/';
var QRCurl = '../QRCSetup/GetQRCList';
var deleteurl = '../QRCSetup/Delete/';
var downloadWarrentyDoc = '../QRCSetup/Download/';
var checkInurl = '../QRCSetup/CheckIn/';
var damagefixedurl = '../QRCSetup/IsDamageFix/';

//#region QRC Type
var qRCTypeddl = ''
    + '<select id="SearchQRCType" class="" onchange="doSearch(arguments[0]||event);">'
    //+ testqRCTypeddl + '</select>';
/**/ + '<option value="">Select All</option>'
+ '<option value="36">Vehicle</option>'
+ '<option value="37">Trash Can</option>'
+ '<option value="38">Elevator</option>'
+ '<option value="39">Gate Arm</option>'
+ '<option value="40">Ticket Spitter</option>'
+ '<option value="41">Bus Station</option>'
+ '<option value="42">Emergency Phone Systems</option>'
+ '<option value="43">Moving Walkway</option>'
+ '<option value="44">Escalators</option>'
+ '<option value="45">Bathroom</option>'
+ '<option value="46">Equipment</option>'
+ '<option value="47">Devices</option>'
+ '<option value="101">Parking Facility</option>'
+ '<option value="375">Shuttle Bus</option></select>'; //this will need to uncomment once app is live

//#endregion QRC Type

var allLocation = ''
if ($_userTypeId == "1" || $_userTypeId == "5" || $_userTypeId == "6") {
    allLocation = '<div class="onoffswitch2"><input type="checkbox" name="onoffswitch2" class="onoffswitch2-checkbox" id="ViewAllLocation"><label for="ViewAllLocation" class="onoffswitch2-label"><span class="onoffswitch2-inner"></span><span class="onoffswitch2-switch"></span></label></div>'
    //allLocation = '<input type="button" id="allLocationright" value="View All Locations" class="ViewAllButton" onclick="ViewAllRecords();" title="Click to view user on All Locations."/>'
}

var ExportAllQRC = ''
    + '&nbsp&nbsp<select id="printQRC" class="" onchange="exportAllQRC(arguments[0]||event);">'
    + '<option value="0">Print QRC</option>'
    + '<option value="All">All QRC</option>'
    + '<option value="Grid">Grid Only</option></select>';
/**/
//var editurl = '../QRCSetup/VehicleRegistration/';
$(function () {
    //alert('GetRegisteredVehicle');
    $("#tbl_QRCList").jqGrid({
        //url: '../QRCSetup/GetQRCList',
        //url: '../QRCSetup/GetRegisteredVehicle',
        url: QRCurl + '?locationId=' + $_locationId,
        datatype: 'json',
        mtype: 'POST',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['QRCodeID', 'QRC Item Name', 'QRC Type', 'Special Notes', 'WarrentyDoc', 'QRCTYPEId', 'CheckOutStatus', 'IsDamage', 'IsDamageVerified', 'LocationName', 'QRCSize', 'Actions', ],        //colNames: ['QRCName', 'QRCTYPE', 'SpecialNotes',  'Actions', ],
        //colNames: ['Vehicle Identification No', 'Vendor', 'License No', 'Insurance Plan', 'Driver Name', 'Actions', ],
        colModel: [{ name: 'QRCodeID', width: 35, sortable: true },
                  { name: 'QRCItemName', width: 70, sortable: true },
                  { name: 'QRCType', width: 35, sortable: false },
                  { name: 'SpecialNotes', width: 100, sortable: false },
                  { name: 'WarrentyDoc', width: 20, sortable: false, hidden: true },
                  { name: 'QRCTYPEId', width: 20, sortable: false, hidden: true },
                  { name: 'CheckOutStatus', width: 20, sortable: false, hidden: true },
                  { name: 'IsDamage', width: 20, sortable: false, hidden: true },
                  { name: 'IsDamageVerified', width: 20, sortable: false, hidden: true },
                  { name: 'LocationName', width: 20, sortable: false, hidden: true },
                  { name: 'QRCSize', width: 20, sortable: false, hidden: true },
                  { name: 'act', index: 'act', width: 50, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        pager: '#divQRCListPager',
        sortname: 'CreatedDate',
        
        viewrecords: true,
        sortorder: 'desc',
        caption: "List of QRC",

        gridComplete: function () {

            var ids = jQuery("#tbl_QRCList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>';
                de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
                qrc = '<a href="javascript:void(0)" class="qrc" title="QRC Generate" data-value="' + cl + '" id="QRCGenerate" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-qrcode fa-2x texthover-bluelight"></span><span class="tooltips">Detail</span></a></div>';
                checkIn = '<div><a href="javascript:void(0)" class="ChkIn" Id="' + cl + '" title="checkin" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check-circle-o fa-2x"></span><span class="tooltips">CheckIn</span></a>';
                damage = '<div><a href="javascript:void(0)" class="damage" Id="' + cl + '" title="damage" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa"><img src="../Content/Images/car_damage.png" /></span><span class="tooltips">Verify</span></a>';
                var rowData = jQuery("#tbl_QRCList").getRowData(cl);
                var WarrentyDoc = rowData['WarrentyDoc'];
                var CheckOut = rowData['CheckOutStatus'];
                var IsDamage = rowData['IsDamage'];
                var IsDamageVerified = rowData['IsDamageVerified'];
                var QRCTYPEId = rowData['QRCTYPEId'];

                var vi = "";
                if (WarrentyDoc == null || WarrentyDoc == "" || WarrentyDoc == '' || WarrentyDoc == undefined) {
                }
                else {
                    vi = '<a href="' + downloadWarrentyDoc + '?Id=' + cl + '" class="download" title="view" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-download fa-2x"></span></a></div>';
                }
                if ($_userTypeId == "3" || $_userTypeId == 3) {
                    //jQuery("#tbl_QRCList").jqGrid('setRowData', ids[i], { act: be + qrc + vi });////Emp doesn't have wright to delete QRC.
                    jQuery("#tbl_QRCList").jqGrid('setRowData', ids[i], { act: qrc + vi });//Emp doesn't have wright to delete QRC.
                }
                else {
                    if ((CheckOut == 'False' || CheckOut == '0') && ($_userTypeId != "4") && ((IsDamage == 'True' || IsDamage == 1) && (IsDamageVerified == null || IsDamageVerified == 'YesNull' || IsDamageVerified == undefined)) && (QRCTYPEId == "36" || QRCTYPEId == 36)) {
                        jQuery("#tbl_QRCList").jqGrid('setRowData', ids[i], { act: be + de + qrc + vi + checkIn + damage });
                    }
                    else if ((CheckOut == 'False' || CheckOut == '0') && ($_userTypeId != "4") && (QRCTYPEId == "36" || QRCTYPEId == 36)) {
                        jQuery("#tbl_QRCList").jqGrid('setRowData', ids[i], { act: be + de + qrc + vi + checkIn });
                    }
                    else if (($_userTypeId != "4") && (IsDamage == 'True' || IsDamage == 1) && (IsDamageVerified == null || IsDamageVerified == 'YesNull' || IsDamageVerified == undefined) && (QRCTYPEId == "36" || QRCTYPEId == 36)) {
                        jQuery("#tbl_QRCList").jqGrid('setRowData', ids[i], { act: be + de + qrc + vi + damage });
                    }
                    else {
                        jQuery("#tbl_QRCList").jqGrid('setRowData', ids[i], { act: be + de + qrc + vi });
                    }
                }
            }

            if ($("#tbl_QRCList").getGridParam("records") <= 20) {
                $("#divQRCListPager").hide();
            }
            else {
                jQuery("#tbl_QRCList").jqGrid('navGrid', '#divQRCListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
                $("#divQRCListPager").show();
            }
            if ($('#tbl_QRCList').getGridParam('records') === 0) {
                $('#tbl_QRCList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },

        caption: '<div class="header_search"> <input id="SearchText" class="inputSearch" placeholder="Search by QRC Name" onkeydown="doSearch(arguments[0]||event)" type="text">&nbsp;' + qRCTypeddl + '&nbsp;' + ExportAllQRC + '&nbsp;' + allLocation + '</div>'
    });

    $('#ViewAllLocation').change(function () {
         
        ViewAllRecords();
    });

    if ($("#tbl_QRCList").getGridParam("records") > 20) {
        jQuery("#tbl_QRCList").jqGrid('navGrid', '#divQRCListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }


});

var timeoutHnd;
var flAuto = true;
function doSearch(ev) {
    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
}

function ViewAllRecords() {
     
    var txtSearch = jQuery("#SearchText").val();
    var selectSearchQRCType = jQuery("#SearchQRCType").val();
    var locaId = $('#ViewAllLocation').prop('checked') == true ? 0 : $("#drp_MasterLocation :selected").val();

    if (locaId == 0) {
        $("#drp_MasterLocation").hide();
    }
    else {
        $("#drp_MasterLocation").show();
    }

    jQuery("#tbl_QRCList").jqGrid('setGridParam', { url: QRCurl + "?locationId=" + locaId + "&SearchText=" + txtSearch.trim() + "&SearchQRCType=" + selectSearchQRCType, page: 1 }).trigger("reloadGrid");
}

function gridReload() {
    var txtSearch = jQuery("#SearchText").val();
    var selectSearchQRCType = jQuery("#SearchQRCType").val();
    var locaId = $('#ViewAllLocation').prop('checked') == true ? 0 : $("#drp_MasterLocation :selected").val();

    if (locaId == 0) {
        $("#drp_MasterLocation").hide();
    }
    else {
        $("#drp_MasterLocation").show();
    }
    selectSearchQRCType = (selectSearchQRCType != undefined && selectSearchQRCType != '' && parseInt(selectSearchQRCType, 10) > 0) ? parseInt(selectSearchQRCType, 10) : null;
    jQuery("#tbl_QRCList").jqGrid('setGridParam', { url: QRCurl + "?&locationId=" + locaId + "&SearchText=" + txtSearch.trim() + "&SearchQRCType=" + selectSearchQRCType, page: 1 }).trigger("reloadGrid");
}

function exportAllQRC(ev) {
    
    var select = $('#printQRC :selected').val();
    if ($.trim(select) != '' && $.trim(select) != '0') {
        if ($.trim(select) == 'Grid') {
            var getAllGridData = $("#tbl_QRCList").jqGrid('getRowData');
            if (getAllGridData.length > 0) {
                var divPrint = '<html><style type="text/css">.pagebreak{  padding: 0px;} table {  page-break-after:auto }tr{ page-break-inside:avoid; page-break-after:auto }thead { display:table-header-group }tfoot{ display:table-footer-group }table {display:inline-block;}</style><body onload="window.print();"><div style="width: 90%;" class="row">';
                for (var i = 0; i < getAllGridData.length ; i++) {
                    generateqrcodeByVJ(getAllGridData[i].QRCodeID, getAllGridData[i].QRCSize);
                    divPrint = divPrint + "<table class='pagebreak' ><tr><td valign='top' style=''><div style='padding: 3px; margin: 0px auto; border: 0px solid rgb(10, 160, 205); float:left;'><strong style='font-size: 16px;'></strong> <br/>"
                  + getAllGridData[i].QRCodeID + "<p><strong style='font-size: 16px;'></strong></p><p>" + container2.innerHTML + "</p></div></td></tr></tbody></table>";
                }
                divPrint = divPrint + "</div></body></html>";

                var popupWin = window.open('', '_blank', 'width=800,height=600');
                popupWin.document.open();
                popupWin.document.write(divPrint);
                if (popupWin.closed == false) {
                    popupWin.document.close();
                }
                $("#printQRC option:eq(0)").attr('selected', 'selected');
            }
        }
        else {
            fn_showMaskloader('Please wait...Loading');
            var sortColumnName = $("#tbl_QRCList").jqGrid('getGridParam', 'sortname');
            var sortOrder = $("#tbl_QRCList").jqGrid('getGridParam', 'sortorder');
            var txtSearch = jQuery("#SearchText").val();
            var selectSearchQRCType = jQuery("#SearchQRCType").val();
            var locaId = $('#ViewAllLocation').prop('checked') == true ? 0 : $("#drp_MasterLocation :selected").val();

            //do export option here
            $.ajax({
                type: "GET",
                url: $_HostPrefix + 'QRCSetup/GetQRCListforPrint',
                async: false,
                data: { "sidx": sortColumnName, "sord": sortOrder, "locationId": locaId, "SearchText": txtSearch, "SearchQRCType": selectSearchQRCType },
                //beforeSend: function () {
                //    new fn_showMaskloader('Please wait...Loading');
                //},
                complete: function () {
                    fn_hideMaskloader();
                },
                success: function (html) {
                    //fn_hideMaskloader();
                    if (html != null && html.rows.length > 0) {

       
                        var divPrint = '<html><style type="text/css">.pagebreak{  padding: 10px;} table { page-break-inside:auto}tr{ page-break-inside:avoid; page-break-after:auto }thead { display:table-header-group }tfoot{ display:table-footer-group }table {display: inline-block;}</style><body onload="window.print();"><div style="width: 90%;" class="row">';
                        for (var i = 0; i < html.rows.length ; i++) {
                            generateqrcodeByVJ(html.rows[i].cell[0], html.rows[i].cell[5]);
                            divPrint = divPrint + "<table class='pagebreak'><tr><td valign='top' style=''><div style='padding: 3px; margin: 0px auto; border: 0px solid rgb(10, 160, 205); float:left;'><strong style='font-size: 16px;'></strong> <br/>"
                          + html.rows[i].cell[0] + "<p><strong style='font-size: 16px;'></strong></p><p>" + container2.innerHTML + "</p></div></td></tr></tbody></table>";
                        }
                        divPrint = divPrint + "</div></body></html>";
                        //fn_hideMaskloader();
                        //$('body').find('.eliteMask').remove();

                        var popupWin = window.open('', '_blank', 'width=800,height=600');
                        popupWin.document.open();
                        popupWin.document.write(divPrint);

                        if (popupWin.closed == false) {
                            fn_hideMaskloader();
                            popupWin.document.close();
                        }
                        else {
                            fn_hideMaskloader();
                        }
                    }
                },
                error: function (err) {
                    fn_hideMaskloader();
                }
            });//ajax end block for fetching list of grc.
            fn_hideMaskloader();
            $("#printQRC option:eq(0)").attr('selected', 'selected');
        }
    }
}
$(".EditRecord").live("click", function (event) {
    var id = $(this).attr("Id");
    window.location.href = editurl + '?qr=' + id;
    //$("#largeeditpopup").load('../StaffUser/EditStaffUser/' + id); 
});

$("#QRCGenerate").live("click", function (event) {
    var id = $(this).attr("data-value");

    //showPopupRelativeMessage("Are you sure want to Generate QRC?", $(this), "QRC Generate For Vehicle!", function () {

    $.ajax({
        type: "GET",
        data: { qr: id },
        url: $_HostPrefix + "QRCSetup/QRCGenerate",
        dataType: "json",
        async: false,
        contentType: "application/json; charset=utf-8",
        error: function (xhr, status, error) {

            closeAjaxProgress();
        },
        success: function (result) {

            $("#qrcNameTxt").html(result.data.QRCName);
            $("#txtSerialNo").html(result.data.SerialNo);
            $("#txtMake").html(result.data.Make);
            // $("#txtSpecialNotes").html(result.data.SpecialNotes);
            if (result.data.Model == '' || result.data.Model == null) {
                $("#Model").hide();
                $("#txtModel").hide();
            }
            else {
                $("#txtModel").html(result.data.Model);
            }
            $("#divLocation").html(result.data.LocationName)

            for (var i = 0; i < result.data.QRCTypeList.length ; i++) {
                if (result.data.QRCTYPE == result.data.QRCTypeList[i]["GlobalCodeId"]) {
                    $("#txtQrcType").html(result.data.QRCTypeList[i]["CodeName"]);
                }
            }
            if (result.data.VehicleType == '' || result.data.VehicleType == null) {
                $("#VehicleType").hide(); $("#lblVehicleType").hide();
            }
            else {
                for (var i = 0; i < result.data.QRCTypeList.length ; i++) {
                    if (result.data.VehicleType == result.data.QRCTypeList[i]["GlobalCodeId"]) {
                        $("#qrcNameTxt").html(result.data.VehicleTypeList[i]["CodeName"]);
                    }

                }

                for (var i = 0; i < result.data.VehicleTypeList.length ; i++) {
                    if (result.data.VehicleType == result.data.VehicleTypeList[i]["GlobalCodeId"]) {
                        $("#lblVehicleType").html(result.data.VehicleTypeList[i]["CodeName"]);
                    }
                }
                $("#VehicleType").show(); $("#lblVehicleType").show();
            }

            if (result.data.MotorType == '' || result.data.MotorType == null) {
                $("#MotorType").hide(); $("#lblMotorType").hide();
            } else {
                for (var i = 0; i < result.data.MotorTypeList.length ; i++) {
                    if (result.data.MotorType == result.data.MotorTypeList[i]["GlobalCodeId"]) {
                        $("#lblMotorType").html(result.data.MotorTypeList[i]["CodeName"]);
                    }
                }
                $("#MotorType").show(); $("#lblMotorType").show();
            }


            $("#lblLocationName").html(result.data.Location);

            $("#lblQRCId").html(result.data.QRCodeID);

            $("#lblDriverName").val(result.data.DriverName);
            //$("#lblQRCId").val(result.data.QRCID);

            //$("#").html(result.data.DriverProfilePic);
            $("#lblLicenseNo").html(result.data.LicenseNo);
            //$("#lblW9Form").html(result.data.W9Form);
            $("#lblVehicleImage").html(result.data.VehicleImage);
            //$("#lblDriverProfilePic").html(result.data.DriverProfilePic);
            $("#lblQRC").html(result.data.QRCName);
            if (result.data.SpecialNotes == '' || result.data.SpecialNotes == null) {
                $("#labelSpecialNotes").hide(); $("#txtSpecialNotes").hide();
            }
            else {
                $("#txtSpecialNotes").html(result.data.SpecialNotes);
                $("#labelSpecialNotes").show(); $("#txtSpecialNotes").show();
            }

            if (result.data.Make == '' || result.data.Make == null) {
                $("#lblMake").hide(); $("#divMake").hide();
            }
            else {
                $("#divMake").html(result.data.Make);
                $("#lblMake").show(); $("#divMake").show();
            }
            $("#lblSpecialNotes").html(result.data.SpecialNotes);
            //$("#lblQRCTYPE").html(result.data.QRCTYPE);
            $("#").html(result.data.VendorID);
            $("#lblCompanyName").html(result.data.CompanyName);
            $("#").html(result.data.ContactName);
            $("#lblBusinessNo").html(result.data.BusinessNo);
            $("#pDriverName").html("<b>Company Name:- </b>" + result.data.CompanyName);
            $("#pCompanyName").html("<b>Driver Name:- </b>" + result.data.DriverName);

            if (result.data.VendorName == '' || result.data.VendorName == null) {
                $("#VendorName").hide(); $("#lblVendorName").hide();
            }
            else {
                $("#VendorName").html(result.data.VendorName);
                $("#VendorName").show(); $("#lblVendorName").show();
            }

            if (result.data.PointOfContact == '' || result.data.PointOfContact == null) {
                $("#PointOfContact").hide(); $("#lblPointOfContact").hide();
            }
            else {
                $("#PointOfContact").html(result.data.PointOfContact);
                $("#PointOfContact").show(); $("#lblPointOfContact").show();
            }
            if (result.data.TelephoneNo == '' || result.data.TelephoneNo == null) {
                $("#TelephoneNo").hide(); $("#lblTelephoneNo").hide();
            }
            else {
                $("#TelephoneNo").html(result.data.TelephoneNo);
                $("#TelephoneNo").show(); $("#lblTelephoneNo").show();
            }
            if (result.data.EmialAdd == '' || result.data.EmialAdd == null) {
                $("#EmialAdd").hide(); $("#lblEmialAdd").hide();
            }
            else {
                $("#EmialAdd").html(result.data.EmialAdd);
                $("#EmialAdd").show(); $("#lblEmialAdd").show();
            }

            if (result.data.IExpDate == '01-01-01' || result.data.IExpDate == null || result.data.IExpDate == '01/01/01') {
                $("#InsuranceExpDate").hide(); $("#lblInsuranceExpDate").hide();
            }
            else {
                $("#InsuranceExpDate").html(result.data.IExpDate);
                $("#InsuranceExpDate").show(); $("#lblInsuranceExpDate").show();
            }
            if (result.data.WExpDate == '01-01-01' || result.data.WExpDate == null || result.data.WExpDate == '01/01/01') {
                $("#WarrantyEndDate").hide(); $("#lblWarrantyEndDate").hide();
            }
            else {
                $("#WarrantyEndDate").html(result.data.WExpDate);
                $("#WarrantyEndDate").show(); $("#lblWarrantyEndDate").show();
            }
            if (result.data.Website == '' || result.data.Website == null) {
                $("#Website").hide(); $("#lblWebsite").hide();
            }
            else {
                $("#Website").html(result.data.Website);
                $("#Website").show(); $("#lblWebsite").show();
            }
            if (result.data.PurchaseType == '' || result.data.PurchaseType == null) {
                $("#ddlPurchaseType").hide(); $("#PurchaseType").hide();
            }
            else {
                for (var i = 0; i < result.data.PurchaseTypeList.length ; i++) {
                    if (result.data.PurchaseType == result.data.PurchaseTypeList[i]["GlobalCodeId"]) {
                        $("#ddlPurchaseType").html(result.data.PurchaseTypeList[i]["CodeName"]);
                    }
                }
                $("#ddlPurchaseType").show(); $("#PurchaseType").show();
            }


            if (result.data.PurchaseTypeRemark == '' || result.data.PurchaseTypeRemark == null) {
                $("#PurchaseTypeRemark").hide(); $("#lblPurchaseTypeRemark").hide();
            }
            else {
                $("#PurchaseTypeRemark").html(result.data.PurchaseTypeRemark);
                $("#PurchaseTypeRemark").show(); $("#lblPurchaseTypeRemark").show();
            }

            $("#divCreatedBy").html(result.data.UserModel.FirstName + ' ' + result.data.UserModel.LastName);

            $("#divCreatedOn").html(result.data.CreatedOn);

            $('#myModalFORQR').modal('show');

            generateqrcodeByVJ(result.data.QRCodeID, result.data.QRCSizeGenerate);

            $("#myModalFORQR :text").attr("readOnly", "true");
        },
        Complete: function (result) {
            closeAjaxProgress();
            console.log('Ajax Div');
            $("#ajaxProgress").css("display", "none");
            event.stopPropagation();
        }
    }); // ajax call end
    //});// pop up alert show


});

function generateqrcodeByVJ(id, sizeGenerate) {
    var $_QRCIDNumber = id;
    var size = (sizeGenerate != undefined && sizeGenerate != '' && $.trim(sizeGenerate) != '') ? $.trim(sizeGenerate) : '155';
    size = (size != undefined && size != '' && $.trim(size) != '') ? $.trim(size) : '155';
    var qrcsize = size.toLowerCase().split('x');

    size = qrcsize[0];
    size = size.trim();
    'use strict';

    var isOpera = Object.prototype.toString.call(window.opera) === '[object Opera]',

        guiValuePairs = [
            ["size", "px"],
            ["minversion", ""],
            ["quiet", " modules"],
            ["radius", "%"],
            ["msize", "%"],
            ["mposx", "%"],
            ["mposy", "%"]
        ],

        updateGui = function () {

            $.each(guiValuePairs, function (idx, pair) {

                var $label = $('label[for="' + pair[0] + '"]');

                $label.text($label.text().replace(/:.*/, ': ' + $('#' + pair[0]).val() + pair[1]));
            });
        },

        updateQrCode = function (mykey, mycontainer) {
            //var EncryptQRC = $('#EncryptQRC').val();
            var EncryptQRC = mykey;

            // ;
            //alert('test 2');
            //lblQRCId

            EncryptQRC = id.toString();//$_QRCIDNumber;


            var options = {
                //render: $("#render").val(),
                render: "image",//render: "image",

                //ecLevel: $("#eclevel").val(),
                ecLevel: "Q",// L=Low, M=Medium,

                //minVersion: parseInt($("#minversion").val(), 10),
                minVersion: parseInt(5, 10),

                fill: '#333333',
                //fill: $("#fill").val(),

                //background: $("#background").val(),
                background: '#ffffff',
                // fill: $("#img-buffer")[0],

                //text: $("#text").val(),
                //text: 'my name is Developer, i am a SSE having around 5 years of experience' + new Date() + '',

                text: EncryptQRC,


                //size: parseInt($("#size").val(), 10),



                size: parseInt(size, 10),

                //radius: parseInt($("#radius").val(), 10) * 0.01,
                radius: parseInt(50, 10) * 0.01,

                //quiet: parseInt($("#quiet").val(), 10),
                quiet: parseInt(1, 10),

                //mode: parseInt($("#mode").val(), 10),
                mode: parseInt(0, 10),

                //mSize: parseInt($("#msize").val(), 10) * 0.01,
                mSize: parseInt(11, 10) * 0.01,
                //mPosX: parseInt($("#mposx").val(), 10) * 0.01,
                mPosX: parseInt(50, 10) * 0.01,
                //mPosY: parseInt($("#mposy").val(), 10) * 0.01,
                mPosY: parseInt(50, 10) * 0.01,

                //label: $("#label").val(),
                label: 'Smartian says',
                //fontname: $("#font").val(),
                fontname: 'Ubuntu',
                //fontcolor: $("#fontcolor").val(),
                fontcolor: '#ff9818',

                //image: $("#img-buffer")[0]
                image: 'http://localhost:57572/Images/upload.jpg'
            };
            //$('"#'+mycontainer+'"').empty().qrcode(options);

            $('#container2').empty().qrcode(options);
            //$("#container").attr('class', 'show');


        },

        update = function () {

            updateGui();
            //updateQrCode();
            updateQrCode('saadad', 'container2');
        },

        onImageInput = function () {

            var input = $("#image")[0];

            if (input.files && input.files[0]) {

                var reader = new FileReader();

                reader.onload = function (event) {
                    $("#img-buffer").attr("src", event.target.result);
                    $("#mode").val("4");
                    setTimeout(update, 250);
                };
                reader.readAsDataURL(input.files[0]);
            }
        },

        download = function (event) {

            var data = $("#container2 canvas")[0].toDataURL('image/png');
            $("#download").attr("href", data);
        };


    $(function () {

        //if (isOpera) {
        //    $('html').addClass('opera');
        //    $('#radius').prop('disabled', true);
        //}

        //$("#download").on("click", download);
        //$("#image").on('change', onImageInput);
        //$("input, textarea, select").on("input change", update);        
        var EncryptQRC = id;
        var EncryptLastQRC = id;


        //if (_hddnUpdateMode != 'True' && EncryptLastQRC != undefined && EncryptLastQRC != '') {
        if (EncryptLastQRC != undefined && EncryptLastQRC != '') {
            //alert('EncryptLastQRC');
            // ;
            updateGui();
            updateQrCode(EncryptLastQRC, 'container2');
        }

        if (EncryptQRC != undefined && EncryptQRC != '') {
            //alert('new EncryptQRC');
            updateGui();
            updateQrCode();
        }

    });
}

function PrintDivIndexForLicence(DivId) {
   
    _isPrintDone = false;
    if (!_isPrintDone) {
        //var divToPrint = document.getElementById('DivQRCIndex');
        var vehicletype = '';
        var motortype = '';
        var specialnotes = '';
        var make = '';
        var model = '';
        var phone = '';
        var divToQRC = document.getElementById("container2");
        var popupWin = window.open('', '_blank', 'width=800,height=500');
        popupWin.document.open();
        if ($("#lblVehicleType").html() != null && $("#lblVehicleType").html() != "" && $("#txtQrcType").html() == 'Vehicle') {
            vehicletype = "<p></p><strong style='font-size: 16px;'>Vehicle Type </strong> <br/>"
                      + $("#lblVehicleType").html();
        }
        if ($("#lblMotorType").html() != null && $("#lblMotorType").html() != "" && $("#txtQrcType").html() == 'Vehicle') {
            motortype = "<p></p><strong style='font-size: 16px;'>Motor Type </strong> <br/>"
                      + $("#lblMotorType").html();
        }
        if ($("#txtSpecialNotes").html() != null && $("#txtSpecialNotes").html() != "") {
            specialnotes = "<p></p><strong style='font-size: 16px;'>Special Notes </strong> <br/>"
                      + $("#txtSpecialNotes").html();
        }
        if ($("#divMake").html() != null && $("#divMake").html() != "") {
            make = "<p></p><strong style='font-size: 16px;'>Make</strong> <br/>"
                      + $("#divMake").html();
        }
        if ($("#txtModel").html() != null && $("#txtModel").html() != "") {
            model = "<p></p><strong style='font-size: 16px;'>Model </strong><br/>"
                      + $("#txtModel").html();
        }
        if ($("#TelephoneNo").html() != null && $("#TelephoneNo").html() != "") {
            phone = "<p></p><strong style='font-size: 16px;'>Phone </strong><br/>"
                      + $("#TelephoneNo").html();
        }
        //popupWin.document.write("<html><body onload='window.print(); window.close();'><div style='width:800px;height:300px;'>" + divToPrint.innerHTML + "</div></body></html>");
        popupWin.document.write("<html><body onload='window.print();'><div style='margin-left: 96px; margin-right: 100px; width: 520px;' class='row '><table id='tblToPrint' style='width: 470px; border: 1px solid #0aa0cd; padding: 10px;'><tr><td valign='top' style='width: 210px;'><p></p><strong style='font-size: 16px;'> QRC ID</strong> <br/>"
            + $("#lblQRCId").html() + "<p></p><strong style='font-size: 16px;'>QRC Name </strong> <br/>"
            + $("#qrcNameTxt").html() + phone + vehicletype + motortype + specialnotes
            + "</td><td td valign='top';>"
            + "<p></p><strong style='font-size: 16px;'>Location Name </strong><br/>"
            + $("#divLocation").html() + make + model
            + "<p><strong style='font-size: 16px;'>QRC Code</strong></p><p>" + divToQRC.innerHTML + "</p></td></tr></tbody></table></td></tr></table></div></body></html>");

        if (popupWin.closed == false) {
            popupWin.document.close();
        }
        _isPrintDone = true;
    }
    //$('.noprint').show();
}

$(".deleteRecord").live("click", function (event) {

    var id = $(this).attr("cid");
    showPopupRelativeMessage("Are you sure want to delete QRCode?", $(this), "Delete QRC", function () {
        $.ajax({
            type: "POST",
            data: "{'qr':'" + id + "'}",
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
                toastr.success(result.Message)
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

$(".ChkIn").live("click", function (event) {
    var id = $(this).attr("Id");
    showPopupRelativeMessage("Are you sure want to CheckIn QRCode?", $(this), "QRCode CheckIn", function () {
        $.ajax({
            type: "POST",
            data: "{'qr':'" + id + "'}",
            url: checkInurl,
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

$(".damage").live("click", function (event) {
    var id = $(this).attr("Id");
    showPopupRelativeMessage("Confirmation! QRCode/vehicle damage has been fixed?", $(this), "QRCode Damage", function () {
        $.ajax({
            type: "POST",
            data: "{'qr':'" + id + "'}",
            url: damagefixedurl,
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