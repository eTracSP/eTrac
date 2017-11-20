var delID;
var PickupFormat;
var DropFormat;
$(document).ready(function () {

    var startDate = new Date();
    var FromEndDate = new Date();
    var ToEndDate = new Date();
    ToEndDate.setDate(ToEndDate.getDate());

    $('#StartDate').datepicker({
        format: "mm/dd/yyyy",
        startDate: new Date()
    }).on('changeDate', function (selected) {
        startDate = new Date(selected.date.valueOf());
        startDate.setDate(startDate.getDate(new Date(selected.date.valueOf())));
        $('#EndDate').datepicker('setStartDate', startDate);
    });

    $('#EndDate').datepicker({
        format: "mm/dd/yyyy",
        startDate: startDate
    }).on('changeDate', function (selected) {
        FromEndDate = new Date(selected.date.valueOf());
        FromEndDate.setDate(FromEndDate.getDate(new Date(selected.date.valueOf())));
        $('#StartDate').datepicker('setEndDate', FromEndDate);
    });
    $('.addrows').click(function () {
        var divID = $('#routeDiv div.dymanicAdd').length;
        $('#routeDiv').append('<div class="top-row dymanicAdd d' + divID + '" style=margin-left:20px;"><div class="field-wrap pickup" style="width :40%;"><label>Route Pick Up Point</label><input type="text" value="" /></div><div class="field-wrap droppoint" style="width : 40%"><label>Route Drop Point</label><input type="text" value="" /></div><div style="margin-top:-7%;margin-left:872px;" class="field-wrap"><div style="margin-left:27px;"><a class="addrows minusSign" id=d' + divID + '><i class="fa fa-minus-circle fa-2x" aria-hidden="true"></i></a></div></div></div>');
        $('#routeDiv').append('<script>jQuery("a.minusSign#d' + divID + '").click(function (){$("div.d' + divID + '").remove();  });</script>');
    });


    //$("#StartDate, #EndDate").datepicker();
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
});

function createArrays() {
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
$(function () {
    $("input[id='RouteName']").blur();
    $("input[id='PickUpPoint']").blur();
    $("input[id='DropPoint']").blur();
    $("#EndDate").blur();
    $("#StartDate").blur();
});

function createPickUpArray() {
    PickupFormat = $('#PickUpPoint').val() + ',';
    $("#routeDiv div.dymanicAdd .pickup").each(function () {
        var myObjJson = {};
        $this = $(this)
        var pickup = $this.find("input").val();
        PickupFormat = PickupFormat + pickup + ', ';
    });
    $('#PickupList').val(PickupFormat);
}

function createDroppointArray() {
    DropFormat = $('#DropPoint').val() + ',';
    $("#routeDiv div.dymanicAdd .droppoint").each(function () {
        var myObjJson = {};
        $this = $(this)
        var droppoint = $this.find("input").val();
        DropFormat = DropFormat + droppoint + ', ';
    });
    $('#DropList').val(DropFormat);
}
