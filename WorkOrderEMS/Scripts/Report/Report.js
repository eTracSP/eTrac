var userId = $("#ddlEmployee").val();
var fromDate = $("#txtFromdate").val();
var toDate = $("#txtTodate").val();
var LocationId = $("#drp_MasterLocation :selected").val();

$(document).ready(function () {


    //$("#txtFromdate").datepicker({ dateFormat: 'mm/dd/yy', endDate: new Date, startDate: '-1m' });
    //$("#txtTodate").datepicker({ dateFormat: 'mm/dd/yy', endDate: new Date });

    //var startDate = new Date();
    //var FromEndDate = new Date();
    //var ToEndDate = new Date();
    //ToEndDate.setDate(ToEndDate.getDate());

    //$('#txtFromdate').datepicker({
    //    weekStart: 1,
    //    startDate: FromEndDate.setDate(-30),
    //    endDate: FromEndDate,
    //    autoclose: true
    //})
    //    .on('changeDate', function (selected) {
    //        startDate = new Date(selected.date.valueOf());
    //        startDate.setDate(startDate.getDate(new Date(selected.date.valueOf())));
    //        $('#txtTodate').datepicker('setStartDate', startDate);
    //    });
    //$('#txtTodate')
    //    .datepicker({
    //        weekStart: 1,
    //        startDate: startDate,
    //        endDate: ToEndDate,
    //        autoclose: true
    //    })
    //    .on('changeDate', function (selected) {
    //        FromEndDate = new Date(selected.date.valueOf());
    //        FromEndDate.setDate(FromEndDate.getDate(new Date(selected.date.valueOf())));
    //        $('#txtFromdate').datepicker('setEndDate', FromEndDate);
    //    });
    //$(".calendar").click(function () {
    //    $(this).siblings("input[type='text']").datepicker('show');
    //})

    $('a[href="#sign_up"]').click(function () {
        $(window.parent.document).scrollTop(20);
    });
   
    if (LocationId == null || LocationId == 'undefined') {
        LocationId = 0;
    }
    $.ajax({
        url: '../DropDown/GetEmployeeByLocationforReport',
        data: JSON.stringify({ "LocationId": LocationId }),
        async: false,
        type: 'POST',
        contentType: "application/json",
        success: function (result) {
            $("#ddlEmployee").append('<option value="0" >All Employees</option>');
            $.each(result, function (i, v) {
                $("#ddlEmployee").append("<option value='" + v.Value + "'>" + v.Text + "</option>");
            });
        },
        error: function (er) {
        }

    });// ajax end block

});



function ReportData() {
    $.ajax({
        url: '../DropDown/GetEmployeeByLocationforReport',
        data: JSON.stringify({ "LocationId": LocationId }),
        async: false,
        type: 'POST',
        contentType: "application/json",
        success: function (result) {

            $("#ddlEmployee").append('<option value="0" >All Employees</option>');
            $.each(result, function (i, v) {
                $("#ddlEmployee").append("<option value='" + v.Value + "'>" + v.Text + "</option>");
            });
        },
        error: function (er) {
        }
    });
}
function trimChar(string, charToRemove) {
    while (string.charAt(0) == charToRemove) {
        string = string.substring(1);
    }

    while (string.charAt(string.length - 1) == charToRemove) {
        string = string.substring(0, string.length - 1);
    }

    return string;
}