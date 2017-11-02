
    var jqXHRData;
$(document).ready(function () {
    initAutoFileUpload();
});

function initAutoFileUpload() {

    'use strict';    
    $('#image').fileupload({
        autoUpload: true,
        url: $_imagePath,
        dataType: 'json',
        add: function (e, data) {
            var jqXHR = data.submit()
                .success(function (data, textStatus, jqXHR) {
                    //if (data != undefined) { alert(data); }
                    //if (textStatus != undefined) { alert(textStatus); }
                    //if (jqXHR != undefined) { alert(jqXHR); }
                    if (data.isUploaded) {

                    }
                    else {

                    }
                })
                .error(function (data, textStatus, errorThrown) {
                    if (typeof (data) != 'undefined' || typeof (textStatus) != 'undefined' || typeof (errorThrown) != 'undefined') {
                        alert(textStatus + errorThrown + data);
                    }
                });
        },
        fail: function (event, data) {
            if (data.files[0].error) {
                alert(data.files[0].error);
            }
        }
    });
}
