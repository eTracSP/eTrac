var Vehicleurl = 'eFleetVehicle/GetListVehicle';
var editurl = 'eFleetVehicle/EditVehicle/';
var deleteURL = 'eFleetVehicle/DeleteVehicle/';
var generateQRC = "eFleetVehicle/_GenerateQRCForVehicle/";
var InspectionDoc = '../eFleetVehicle/InspectionDownload/';
var vehcileApprovalStatus = ''
    //+ '<select id="vehcileStatusType" class="" onchange="doSearch(arguments[0]||event);">'
    //+ '<option value="0">All Approved</option>'
    //+ '<option value="244">Approved By Manager</option>'
    ////+ '<option value="0">Pending</option>'
    //+ '</select>';
$(function () {
    $("#tbl_VehicleList").jqGrid({
        url: $_HostPrefix + Vehicleurl + '?LocationID=' + $_locationId,
        datatype: 'json',
        type: 'GET',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['QRCCodeID', 'Vehicle Identification No', 'Vehicle No', 'Make', 'Model', 'Fuel Type', 'Vehicle Image', 'GVWR', 'Year', 'StorageAddress','DummyField', 'Actions'],
        colModel: [{ name: 'QRCCodeID', width: 30, sortable: true },
        { name: 'VehicleIdentificationNo', width: 30, sortable: true },
        { name: 'VehicleNumber', width: 40, sortable: false },
        { name: 'Make', width: 40, sortable: true },
        { name: 'Model', width: 40, sortable: true },
        { name: 'ListFuelType', width: 40, sortable: true },
        { name: 'VehicleImage', width: 15, sortable: false, formatter: imageFormat },
        //{ name: 'LocationName', width: 100, sortable: false, hidden: true },
        { name: 'GVWR', width: 5, sortable: false, hidden: true },
        { name: 'Year', width: 5, sortable: false, hidden: true },
        { name: 'StorageAddress', width: 5, sortable: false, hidden: true },
        { name: 'DummyField', width: 5, sortable: false, hidden: true },
        { name: 'act', index: 'act', width: 30, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divQRCListPager',
        sortname: 'VehicleNumber',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        //emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        caption: "List of Vehicle",
        gridComplete: function () {
            var ids = jQuery("#tbl_VehicleList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                var rowData = jQuery("#tbl_VehicleList").getRowData(cl);

                var InspectionStatusFile = rowData['DummyField'];
                be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
                de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
                vi = '<a href="javascript:void(0)" class="viewRecord" title="view" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;">view<span class="ui-icon ui-icon-disk"></span><span class="tooltips">Delete</span></a>';
                qrc = '<a href="javascript:void(0)" class="qrc-code" title="QRC Generate" data-value="' + cl + '" id="QRCGenerate" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-qrcode fa-2x texthover-bluelight"></span><span class="tooltips">Detail</span></a></div>';
                var insfile = "";
                if (InspectionStatusFile == null || InspectionStatusFile == "" || InspectionStatusFile == '' || InspectionStatusFile == undefined) {
                }
                else {
                    insfile = '<a href="' + InspectionDoc + '?Id=' + cl + '" class="download-cloud" title="Inspection file" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-cloud-download fa-2x"></span><span class="tooltips">Disclaimer</span></a></div>';
                }

                jQuery("#tbl_VehicleList").jqGrid('setRowData', ids[i], { act: be + de + qrc + insfile }); //+ qrc 
            }
            if ($("#tbl_VehicleList").getGridParam("records") <= 20) {
                $("#divQRCListPager").hide();
            }
            else {
                $("#divQRCListPager").show();
            }
            if ($('#tbl_VehicleList').getGridParam('records') === 0) {
                $('#tbl_VehicleList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div class="header_search"><input id="SearchText" class="inputSearch" placeholder="Search by Vehicle No" onkeydown="doSearch(arguments[0]||event)" type="text">&nbsp;' + vehcileApprovalStatus + '&nbsp;</div>'

    });
    if ($("#tbl_VehicleList").getGridParam("records") > 20) {
        jQuery("#tbl_VehicleList").jqGrid('navGrid', '#divQRCListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
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
    var statusType = jQuery("#vehcileStatusType :selected").val();
    jQuery("#tbl_VehicleList").jqGrid('setGridParam', { url: $_HostPrefix + Vehicleurl + "?SearchText=" + txtSearch.trim() + "&statusType=" + statusType + "&LocationID=" + $_locationId, page: 1 }).trigger("reloadGrid");
}

$(".EditRecord").live("click", function (event) {
    
    var id = $(this).attr("Id");
    window.location.href = $_HostPrefix + editurl + '?id=' + id;
});

$("#QRCGenerate").live("click", function (event) {
    
    var id = $(this).attr("data-value");
    var rowData = jQuery("#tbl_VehicleList").getRowData(id);
    var VehicleNumber = rowData['VehicleNumber'];
    var QRCCodeID = rowData['QRCCodeID'];
    var Make = rowData['Make'];
    var Model = rowData['Model'];
    var Year = rowData['Year'];
    var FuelType = rowData['ListFuelType'];
    var GVWR = rowData['GVWR'];
    var StorageAddress = rowData['StorageAddress'];
    var VehicleImage = rowData['VehicleImage'];
    $("#lblVehicleNumber").html(VehicleNumber);
    $("#lblQRCCodeID").html(QRCCodeID);
    $("#lblMake").html(Make);
    $("#lblModel").html(Model);
    $("#lblYear").html(Year);
    $("#lblVehicleImage").html(VehicleImage);
    // $("#labellWorkRequestStatus").show();
    //$("#lblWorkRequestStatus").show();
    $("#lblFuelType").html(FuelType);
    $("#lblGVWR").html(GVWR);
    $("#lblStorageAddress").html(StorageAddress);
    //$("#lblVehicleImage").html(VehicleImage);
    if (VehicleImage == '' || VehicleImage == null || VehicleImage == "") {
        $("#labelVehicleImage").hide();
        $("#lblVehicleImage").hide();
    }
    generatelistvehicleqrcode(QRCCodeID);   
    
    $('.modal-title').text("eFleet Vehicle Details");
    $("#myModalFOReFleetVehicleQRC").modal('show');

});

//$("#QRCGenerate").live("click", function (event) {
//    var id = $(this).attr("data-value");
//    var rowData = jQuery("#tbl_VehicleList").getRowData(id);
//    var LocationName = $("#drp_MasterLocation option[value='" + $_locationId + "']").text();
//    $.ajax({
//        type: "POST",
//        data: "{'id':'" + id + "'}",
//        url: generateQRC, //$_HostPrefix + generateQRC,
//        dataType: "json",
//        async: false,
//        contentType: "application/json; charset=utf-8",
//        error: function (xhr, status, error) {
//            closeAjaxProgress();
//        },
//        beforeSend: function () {
//            //showAjaxProgress();
//        },
//        success: function (result) {
//            loadpreview(result);
//            $("#lblDriverName").html(result.data.DriverName);
//            $("#lblQRCId").val(result.data.QRCID);
//            $("#").html(result.data.DriverProfilePic);
//            $("#lblLicenseNo").html(result.data.LicenseNo);
//            //$("#lblW9Form").html(result.data.W9Form);
//            //$("#lblVehicleImage").html(result.data.VehicleImage);
//            //$("#lblDriverProfilePic").html(result.data.DriverProfilePic);
//            $("#lblQRC").html(result.data.QRCName);
//            $("#lblSpecialNotes").html(result.data.SpecialNotes);
//            $("#lblQRCTYPE").html(result.data.QRCTYPE);
//            $("#").html(result.data.VendorID);
//            $("#lblCompanyName").html(result.data.CompanyName);
//            $("#").html(result.data.ContactName);
//            $("#lblBusinessNo").html(result.data.BusinessNo);
//            $("#pDriverName").html("<b>Company Name: </b>" + result.data.CompanyName);
//            $("#pCompanyName").html("<b>Driver Name: </b>" + result.data.DriverName);
//            $("#VendorName").html(result.data.VendorName);
//            $("#PointOfContact").html(result.data.PointOfContact);
//            $("#TelephoneNo").html(result.data.TelephoneNo);
//            $("#EmialAdd").html(result.data.EmialAdd);
//            $("#InsuranceExpDate").html(result.data.InsuranceExpDate);
//            $("#Website").html(result.data.Website);
//            $("#WarrantyEndDate").html(result.data.WarrantyEndDate);
//            $("#PurchaseType").html(result.data.PurchaseType);
//            $("#lblVehicleIdentificationNo").html(result.data.VehicleIdentificationNo);
//            $("#lblPermitType").html(result.data.PermitDetailType);
//            $("#lblVehicleModel").html(result.data.VehicleModel);
//            $("#lblVehicleTagNo").html(result.data.VehicleTagNo);
//            $("#lblLocationName").html(LocationName);
//            //var canvas = document.getElementById('imgDriverProfilePic');
//            //var context = canvas.getContext('2d');
//            //var imageObj = new Image();
//            //imageObj.onload = function () {
//            //    context.drawImage(imageObj, 5, 5);
//            //    //context.drawImage(imageObj, 5, 5, 100, 155, 5, 5, 100, 155);
//            //};
//            //imageObj.src = '/Content/Images/ProjectLogo/' + result.data.DriverProfilePic;//Driver_VK_10-24-2015mozlia.png'; // this code will draw image driver profile pic after generating QRC Code.
//            var imgVehicleImg = result.data.VehicleImage == null ? "defaultImage.png" : result.data.VehicleImage;
//            console.log('Content/eFleetDocs/VehicleImage/' + imgVehicleImg);
//            $("#imgVehicleImagePic34").attr('src', '' + 'Content/Images/ProjectLogo/' + imgVehicleImg);
//            var img23 = result.data.DriverProfilePic == null ? "no-profile-pic.jpg" : result.data.DriverProfilePic;
//            $("#imgDriverProfilePic").attr('src', 'Content/Images/ProjectLogo/' + img23);
//            console.log('Content/Images/DriverProfilePic/' + result.data.DriverProfilePic);
//            //// This code will bind driver profiel pic in canvas of generate licence div(page).
//            //var canvas2 = document.getElementById('imgDriverProfilePic2');
//            //var context2 = canvas2.getContext('2d');
//            //imageObj2 = new Image();
//            //imageObj2.onload = function () {
//            //    context2.drawImage(imageObj2, 5, 5);
//            //};
//            //imageObj2.src = '/Content/Images/ProjectLogo/' + result.data.DriverProfilePic;//Driver_VK_10-24-2015mozlia.png'; // this code will draw image driver profile pic after generating QRC Code.
//            ////-------------------------------------------------------------------------------------
//            $("#imgDriverProfilePic2,#imgClinetLogo,#qrcprint").css({ 'width': '80', 'height': '80' });
//            $("#imgDriverProfilePic").css({ 'width': '120', 'height': '120' });
//            $("#imgDriverProfilePic2").attr('src', 'Content/Images/ProjectLogo/' + img23);
//            //// This code will bind CompanyLogo pic in canvas of generate licence div(page).
//            //var canvas3 = document.getElementById('imgClinetLogo');
//            //var context3 = canvas3.getContext('2d');
//            //imageObj3 = new Image();
//            //imageObj3.onload = function () {
//            //    context3.drawImage(imageObj3, 5, 5);
//            //};
//            //imageObj3.src = '/Content/Images/ProjectLogo/' + result.data.CompanyLogo;//Driver_VK_10-24-2015mozlia.png'; // this code will draw image driver profile pic after generating QRC Code.
//            img23 = "";
//            img23 = result.data.CompanyLogo != null && result.data.CompanyLogo != '' ? result.data.CompanyLogo : "no_img_found.png";
//            $("#imgClinetLogo").attr('src', 'Content/Images/ProjectLogo/' + img23);
//            $('#myModalFORQRC').modal('show');
//            //$('#myModalLicence').modal('show');
//        },
//        Complete: function (result) {
//            closeAjaxProgress();
//            console.log('Ajax Div');
//            $("#ajaxProgress").css("display", "none");
//            event.stopPropagation();
//        }
//    }); // ajax call end
//    //});// pop up alert show
//});
$(".deleteRecord").live("click", function (event) {
    var id = $(this).attr("cid");
    bootbox.dialog({
        message: "are you sure you want to delete this Vehicle?",
        buttons: {
            success: {
                label: "delete",
                classname: "btn btn-primary",
                callback: function () {
                    $.ajax({
                        type: "post",
                        url: '../eFleetVehicle/DeleteVehicle/' + '?VehicleID=' + id,
                        //$(event).attr("id")
                        beforesend: function () {
                            new fn_showmaskloader('please wait...');
                        },
                        success: function (data) {
                            toastr.success(data.message);
                            $('#tbl_eFleetPMList').trigger('reloadgrid');
                            gridReload();
                        },
                        error: function () {
                            alert("error:")
                        }
                        //complete: function () {
                        //    fn_hidemaskloader();
                        //}
                    });
                }
            },
            danger: {
                label: "cancel",
                classname: "btn btn-primary",
                callback: function () {

                }
            }
        }
    })   
});
function loadpreview(result) {
    var isanyfieldempty = false;
    var QRCType;
    var errorMessage;
    $_QRCIDNumber = result.data.QRCID.toString();
    generateqrcodeByVJ();
    if ($("#QRCType").val() != "36") {
        $(".VehicleTypeDisplay").css('display', 'none');
    }
    else {
        $(".VehicleTypeDisplay").css('display', '');
    }
    $('#myModalFORQRC').modal('show');
    $("#ModalConfirumationPreview").modal('show');
    return !isanyfieldempty;
}
function generateqrcodeByVJ() {
    console.log("generateqrcode");
    var size = 155;
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
            var EncryptQRC = mykey;
            // ;
            // ;
            //alert('test 2');
            //lblQRCId
            EncryptQRC = $_QRCIDNumber;
            if (EncryptQRC == undefined || EncryptQRC == '' || EncryptQRC == 'rinku') {
                EncryptQRC = $('#lblQRCId').text();
            }
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
            $('#container').empty().qrcode(options);
            $('#container2').empty().qrcode(options);
            $('#container3').empty().qrcode(options);
            //$("#container").attr('class', 'show');
            $("#divToPrint").attr('class', 'show');
        },
        update = function () {
            updateGui();
            updateQrCode('saadad', 'container');
            updateQrCode('saadad', 'container2');
            updateQrCode('saadad', 'container3');
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
            var data = $("#container canvas")[0].toDataURL('image/png');
            var data = $("#container2 canvas")[0].toDataURL('image/png');
            var data = $("#container3 canvas")[0].toDataURL('image/png');
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
        var EncryptQRC = $_QRCIDNumber;
        var EncryptLastQRC = EncryptQRC;
        //if (_hddnUpdateMode != 'True' && EncryptLastQRC != undefined && EncryptLastQRC != '') {
        if (EncryptQRC != undefined) {
            //alert('EncryptLastQRC');
            updateGui();
            updateQrCode(EncryptLastQRC, 'container');
            updateQrCode(EncryptQRC, 'container2');
            updateQrCode(EncryptQRC, 'container3');
        }
        if (EncryptQRC != undefined && EncryptQRC != '') {
            //alert('new EncryptQRC');
            updateGui();
            updateQrCode();
        }
    });
}
//#region Image
function imageFormat(cellvalue, options, rowObject) {
    if (cellvalue == "")
    { return ""; }
    else {
        return '<img src="' + cellvalue + '" class="gridimage" onclick="EnlargeImageView(this);"/>';
    }
}
//#endregion
function PrintDivIndexForLicence(DivId) {
    _isPrintDone = false;
    if (!_isPrintDone) {
        //var divToPrint = document.getElementById('DivQRCIndex');
        var divToPrint = document.getElementById(DivId);
        var divToQRC = document.getElementById("container2");
        $("#container3 img").css("width", "80");
        //alert(divToPrint);
        //$('.modal-body').html(divToPrint.innerHTML);
        var popupWin = window.open('', '_blank', 'width=450,height=300');
        popupWin.document.open();
        //popupWin.document.write("<html><body onload='window.print(); window.close();'><div style='width:800px;height:300px;'>" + divToPrint.innerHTML + "</div></body></html>");
        //popupWin.document.write("<html><body onload='window.print();'><div style='border:1px solid blue;width:600px;height:300px;'>" + divToPrint.innerHTML + "<div style='margin-left: 96px; margin-right: 100px;' class='row'><br><br><br><br><br><br><table id='tblToPrint' border='0'><tr><td><table><tbody><tr><td colaspan='2'>" + divToQRC.innerHTML + "</td><td valign='top'><h3>Description:- </h3><hr /> <p>ELite even interfaces with…</p></td></tr></tbody></table></td></tr></table></div></body></html>");
        popupWin.document.write("<html><head></head><body style='background-color:#fff;font-family: Helvetica Neue,Helvetica,Arial,sans-serif;' onload='window.print();'><div style='border:1px solid blue;width:372px;height:190px;margin-left:20px;padding:10px 22px;font-size:12px;'>" + divToPrint.innerHTML.replace('margin-left: 120px;', '') + "</div></body></html>");
        //alert($(DivId).html());
        if (popupWin.closed == false) {
            popupWin.document.close();
        }
        _isPrintDone = true;
    }
    //$('.noprint').show();
}
function PrintDivForVehicleDetails(DivId) {
    
    _isPrintDone = false;
    if (!_isPrintDone) {
        //var divToPrint = document.getElementById('DivQRCIndex');
        var Fueltype = '';
        var divToVehicleImg = document.getElementById("lblVehicleImage");
        //var divToProfile = document.getElementById("lblProfile");
        var popupWin = window.open('', '_blank', 'width=800,height=600');
        popupWin.document.open();
        //popupWin.document.write("<html><body onload='window.print(); window.close();'><div style='width:800px;height:300px;'>" + divToPrint.innerHTML + "</div></body></html>");
        if ($("#lblFuelType").html() != null && $("#lblFuelType").html() != "") {
            Fueltype = "<p></p><strong style='font-size: 16px;'>Fuel Type </strong> <br/>"
                + $("#lblFuelType").html();
        }

        popupWin.document.write("<style>img {height: 110px;width: 115px;}</style><html><body onload='window.print();'><div style='margin-left: 96px; margin-right: 100px; width: 420px;' class='row '><table id='DivVehicleDetailsIndex' style='width: 400px; border: 1px solid #0aa0cd; padding: 10px;'><tr><td valign='top' style='width: 210px;'><p></p><strong style='font-size: 16px;'> QRC Code No</strong> <br/>"
            + $("#lblQRCCodeID").html() + "<p></p><strong style='font-size: 16px;'>Vehicle Number </strong> <br/>"
            + $("#lblVehicleNumber").html() + "<p></p><strong style='font-size: 16px;'>Make </strong> <br/>"
            + $("#lblMake").html() + "<p></p><strong style='font-size: 16px;'>Model</strong> <br/>"
            + $("#lblModel").html() + "<p></p><strong style='font-size: 16px;'>GVWR</strong> <br/>"
            + $("#lblGVWR").html()
            + "</td><td td valign='top';>"
            + "<p></p><strong style='font-size: 16px;'>Storage Address </strong><br/>"
            + $("#lblStorageAddress").html()
            + "<p></p><strong style='font-size: 16px;'>Vehicle Image</strong><br/>" + divToVehicleImg.innerHTML + "</td></tr></tbody></table></td></tr></table></div></body></html>");

        if (popupWin.closed == false) {
            popupWin.document.close();
        }
        _isPrintDone = true;
    }
    //$('.noprint').show();
}


