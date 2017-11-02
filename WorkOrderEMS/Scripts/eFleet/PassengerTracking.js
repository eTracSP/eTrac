$(document).ready(function () {

    $('.addrows').click(function () {
        debugger
        var divID = $('#routeDiv div.dymanicAdd').length;
        var delID = $(this).attr("id");
        $('#routeDiv').append('<div class="top-row dymanicAdd d' + divID + '" style=margin-left:20px;"><div class="field-wrap pickup" style="width :40%;"><label>Route Pick Up Point</label><input type="text" value="" /></div><div class="field-wrap droppoint" style="width : 40%"><label>Route Drop Point</label><input type="text" value="" /></div><div style="margin-top:-7%;margin-left:872px;" class="field-wrap"><div><a class="addrows minusSign" id=d' + divID + '><i class="fa fa-minus-circle fa-2x" aria-hidden="true"></i></a></div></div></div>');
        $('#routeDiv').append('<script>jQuery("a.minusSign").click(function (){ $("div.' + delID + '").remove() });</script>');
    });

    $("#StartDate, #EndDate").datepicker();
    //Attach change event to textbox
    $("#StartDate, #EndDate").change(function () {
        //Check if value is empty or not
        if ($(this).val() == "") {
            $(this).css("border", "1px solid #bc3838");
        }
        else {
            $(this).css("border", "1px solid #4475b5");
        }
    });

    $("#PickUpPoint, #DropPoint").change(function () {
        if ($(this).val() == "") {
            $(this).css("border", "1px solid #bc3838");
        }
        else {
            $(this).css("border", "1px solid #4475b5");
        }
    });

    $("#ServiceType").change(function () {
        if ($(this).val() == "") {
            $(this).css("border", "1px solid #bc3838");
        }
        else {
            $(this).css("border", "1px solid #4475b5");
        }
    });

    $("#increment").click(function () {

    });
});

var PickupFormat;
var DropFormat;
function createArrays() {
    debugger
    createPickUpArray();
    createDroppointArray();
}

function validateinputform() {
    var status = false;
    var txtServiceType = $("#ServiceType :checked").val();
    var txtStartDate = $("#StartDate").val();
    var txtEndDate = $("#EndDate").val();
    var txtPickUpPoint = $("#PickUpPoint").val();
    var txtDropPoint = $("#DropPoint").val();

    //Check if value is empty or not
    if (txtServiceType == "") {
        //if empty then assign the border
        $("#ServiceType").css("border", "1px solid #b94a48");
        status = false;
    }
    else {
        $("#ServiceType").css("border", "1px solid #4475b5");
        status = true;
    }

    if (txtStartDate == "") {
        //if empty then assign the border
        $("#StartDate").css("border", "1px solid #b94a48");
        status = false;
    }
    else {
        $("#StartDate").css("border", "1px solid #4475b5");
        status = true;
    }

    if (txtEndDate == "") {
        $("#EndDate").css("border", "1px solid #b94a48");
        status = false;
    }
    else {
        $("#EndDate").css("border", "1px solid #4475b5");
        status = true;
    }

    if (txtPickUpPoint == "") {
        $("#PickUpPoint").css("border", "1px solid #b94a48");
        status = false;
    }
    else {
        $("#PickUpPoint").css("border", "1px solid #4475b5");
        status = true;
    }

    if (txtDropPoint == "") {
        $("#DropPoint").css("border", "1px solid #b94a48");
        status = false;
    }
    else {
        $("#DropPoint").css("border", "1px solid #4475b5");
        status = true;
    }

    return status;
}

function createPickUpArray() {
    debugger
    PickupFormat = $('#PickUpPoint').val() + ',';
    $("#routeDiv div.dymanicAdd .pickup").each(function () {
        var myObjJson = {};
        debugger
        $this = $(this)
        var pickup = $this.find("input").val();
        PickupFormat = PickupFormat + pickup + ', ';
    });
    $('#PickupList').val(PickupFormat);
}

function createDroppointArray() {
    debugger
    DropFormat = $('#DropPoint').val() + ',';
    $("#routeDiv div.dymanicAdd .droppoint").each(function () {
        var myObjJson = {};
        debugger
        $this = $(this)
        var droppoint = $this.find("input").val();
        DropFormat = DropFormat + droppoint + ', ';
    });
    $('#DropList').val(DropFormat);
}
