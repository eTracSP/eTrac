var $_QRCIDNumber = "smartData";
var errormessage = "QR Code size is required.";
function requiredqrczise() {
    $('#diverrormessage').text(errormessage);
}

(function ($) {
    generateqrcode();
}(jQuery));


function generateqrcode() {
    
    var size = '155';
    //alert('i am inside'); 
    if ($('#QRCSize') != undefined && $('#QRCSize option:selected').val() != '' && parseInt($('#QRCSize option:selected').val(), 10) > 0)
    { size = $('#QRCSize option:selected').text(); }
    else if ($('#DefaultSize') != undefined && $('#DefaultSize option:selected').val() != '' && parseInt($('#DefaultSize option:selected').val(), 10) > 0)
    { size = $('#DefaultSize option:selected').text(); }
    else if ($('#lblSizeCaption') != undefined && $('#lblSizeCaption').text() != '')
    { size = $('#lblSizeCaption').text(); }
    else { requiredqrczise(); $('[type="submit"]').prop('disabled', true); return false; }

    size = (size != undefined && size != '' && size.trim() != '') ? size.trim() : '155';
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

            //alert('test 2');
            //lblQRCId

            EncryptQRC = $_QRCIDNumber;
            if (EncryptQRC == 'smartData') {
                var t = $('#LastQRCID').val();
                EncryptQRC = t;
            }
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
            //$("#container").attr('class', 'show');
            $("#divToPrint").attr('class', 'show');

        },

        update = function () {

            updateGui();
            //updateQrCode();
            updateQrCode('saadad', 'container');
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
            var data = $("#container canvas")[0].toDataURL('image/png');
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
        var EncryptQRC = $('#EncryptQRC').val();
        var EncryptLastQRC = $('#EncryptLastQRC').val();


        //if (_hddnUpdateMode != 'True' && EncryptLastQRC != undefined && EncryptLastQRC != '') {
        if (EncryptLastQRC != undefined && EncryptLastQRC != '') {
            //alert('EncryptLastQRC');
            // ;
            updateGui();
            updateQrCode(EncryptLastQRC, 'container');
            updateQrCode(EncryptLastQRC, 'container2');
        }

        if (EncryptQRC != undefined && EncryptQRC != '') {
            //alert('new EncryptQRC');
            updateGui();
            updateQrCode();
        }

    });
}
