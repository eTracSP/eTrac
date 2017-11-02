var _LocationTypeUrban = 107;

function BindState_Old() {
    $("#States").empty();
    if ($("#Country").val() != "") {
        $("#States").prop("disabled", false);
        var options = {};
        //options.url = "/Dropdown/GetStateByCountryID";
        //options.url = '@Url.Action("GetStateByCountryID", "Dropdown")'
        //var GetStateByCountryIDUrl = $('#StateByCountryIDUrl').val();
        var GetStateByCountryIDUrl = $_GetStateByCountryIDUrl
        options.url = GetStateByCountryIDUrl;
        options.type = "POST";
        options.beforeSend = function () {
            new fn_showMaskloader('Please wait...');
        }
        options.complete = function () {
            fn_hideMaskloader();

        }
        options.data = JSON.stringify({ CountryID: $("#Country").val() });
        options.dataType = "json";
        options.contentType = "application/json";
        options.success = function (State) {

            var stateID = null;
            $("#States").append("<option value=''>--Select--</option>");
            for (var i = 0; i < State.Data.length; i++) {

                $("#States").append("<option value=" + State.Data[i].StateId + ">" + State.Data[i].StateName + "</option>");
                if (parseInt($_StateID) == State.Data[i].StateId) {
                    stateID = i;
                }
            }
            if (stateID != null) {

                var StateName = State.Data[stateID].StateName;
                $("#States option")
              .filter(function () { return $.trim($(this).text()) == StateName; })
              .attr('selected', true);

                $("#States").val($_StateID);
            }
        };
        options.error = function () { alert("Error retrieving State!"); };
        $.ajax(options);
    }
    else {
        $("#States").empty();
        $("#States").prop("disabled", true);
    }
}

function BindState(Isbilling) {

    //alert($('select:[id*=Company]'));

    if (Isbilling != undefined && Isbilling == true) {

        var ddlstate = $("#BillingStates");
        var ddlcountry = $("#BillingCountry");
    }
    else if (Isbilling != undefined && Isbilling == false) {
        var ddlstate = $("#CompanyStates");
        var ddlcountry = $("#CompanyCountry");
    }
    else if (Isbilling == 'user') {
        //alert('insidenull');
        var ddlstate = $("#EmployeeStates");
        var ddlcountry = $("#EmployeeCountry");
    }
    else //(Isbilling == undefined) 
    {
        var ddlstate = $("#States");
        var ddlcountry = $("#Country");
    }
    //alert($(this).prop('name'));
    //$.each($('select:[name*=' + $(ddlstate).prop('name') + ']'), function (index, valuee) {
    // ;
    //var ss = valuee;
    //alert(ss.name);
    //alert(ss.id);
    //});


    $(ddlstate).empty();

    if ($(ddlcountry).val() != "" && $(ddlcountry).val() != "-1") {
        $(ddlstate).prop("disabled", false);
        var options = {};
        //options.url = "/Dropdown/GetStateByCountryID";
        //options.url = '@Url.Action("GetStateByCountryID", "Dropdown")'
        //var GetStateByCountryIDUrl = $('#StateByCountryIDUrl').val();
        var GetStateByCountryIDUrl = $_GetStateByCountryIDUrl
        options.url = GetStateByCountryIDUrl;
        options.type = "POST";
        options.beforeSend = function () {
            new fn_showMaskloader('Please wait...');
        }
        options.complete = function () {
            fn_hideMaskloader();

        }
        options.asyn = false;
        options.data = JSON.stringify({ CountryID: $(ddlcountry).val() });
        options.dataType = "json";
        options.contentType = "application/json";
        options.async = false;
        options.success = function (State) {

            if (State != null && State != undefined) {
                var stateID = null;
                $(ddlstate).append("<option value=''>--Select--</option>");
                for (var i = 0; i < State.Data.length; i++) {

                    $(ddlstate).append("<option value=" + State.Data[i].StateId + ">" + State.Data[i].StateName + "</option>");
                    if (parseInt($_StateID) == State.Data[i].StateId) {
                        stateID = i;
                    }
                }
                if (stateID != null) {

                    var StateName = State.Data[stateID].StateName;
                    $("#States option")
                  .filter(function () { return $.trim($(this).text()) == StateName; })
                  .attr('selected', true);

                    $(ddlstate).val($_StateID);
                }
            }
        };
        options.error = function () { alert("Error retrieving State!"); };
        $.ajax(options);
    }
    else {
        $(ddlstate).empty();
        $(ddlstate).prop("disabled", true);
    }
}

function BindAsset() {
    // ;
    $("#ddlAssetID").empty();
    if ($("#ddlWorkArea").val() != "") {
        $("#ddlAssetID").prop("disabled", false);
        var options = {};
        //options.url = "/Dropdown/GetStateByCountryID";
        //options.url = '@Url.Action("GetStateByCountryID", "Dropdown")'
        //var GetStateByCountryIDUrl = $('#StateByCountryIDUrl').val();
        var GetAssetByWorkAreaUrl = $_GetAssetByWorkAreaUrl
        options.url = GetAssetByWorkAreaUrl;
        options.type = "POST";
        options.beforeSend = function () {
            new fn_showMaskloader('Please wait...');
        }
        options.complete = function () {
            fn_hideMaskloader();

        }
        options.data = JSON.stringify({ WorkArea: $("#ddlWorkArea").val() });
        options.dataType = "json";
        options.contentType = "application/json";
        options.success = function (Data) {

            var _AssetID = null;
            $("#ddlAssetID").append("<option value=''>--Select--</option>");
            for (var i = 0; i < Data.length; i++) {

                $("#ddlAssetID").append("<option value=" + Data[i].Value + ">" + Data[i].Text + "</option>");
                if (parseInt($_AssetID) == Data[i].Value) {
                    _AssetID = i;
                }
            }
            if (_AssetID != null) {

                var StateName = Data[_AssetID].Text;
                $("#ddlAssetID option")
              .filter(function () { return $.trim($(this).text()) == StateName; })
              .attr('selected', true);

                $("#ddlAssetID").val($_AssetID);
            }
        };
        options.error = function () { alert("Error retrieving Asset!"); };
        $.ajax(options);
    }
    else {
        $("#ddlAssetID").empty();
        $("#ddlAssetID").prop("disabled", true);
    }

}


//function BindInsurancePlan() {
//    $("#InsurancePlan").empty();
//    if ($("#InsuranceCompany").val() != "") {
//        $("#InsurancePlan").prop("disabled", false);
//        var options = {};
//        var GetInsurancePlanByInsuranceIdUrl = $_GetInsurancePlanByInsuranceIdUrl

//        options.url = GetInsurancePlanByInsuranceIdUrl;
//        options.type = "POST";
//        options.data = JSON.stringify({ InsuranceCompanyId: $("#InsuranceCompanyID option:selected").val() });
//        options.beforeSend = function () {
//            new fn_showMaskloader('Please wait...');
//        }
//        options.complete = function () {
//            fn_hideMaskloader();

//        }
//        options.dataType = "json";
//        options.contentType = "application/json";
//        options.success = function (State) {
//            var stateID = null;
//            $("#InsurancePlan").append("<option value=''>--Select--</option>");
//            for (var i = 0; i < State.InsurancePlanList.length; i++) {

//                $("#InsurancePlan").append("<option value=" + State.InsurancePlanList[i].InsurancePlanID + ">" + State.InsurancePlanList[i].InsurancePlan + "</option>");
//                if (parseInt($_InsurancePlanID) == State.InsurancePlanList[i].InsurancePlanID) {
//                    stateID = i;
//                }
//            }
//            if (stateID != null) {

//                var StateName = State.InsurancePlanList[stateID].InsurancePlan;
//                $("#InsurancePlan option")
//              .filter(function () { return $.trim($(this).text()) == StateName; })
//              .attr('selected', true);

//                $("#InsurancePlan").val($_InsurancePlanID);
//            }
//        };
//        options.error = function () { alert("Error retrieving State!"); };
//        $.ajax(options);
//    }
//    else {
//        $("#InsurancePlan").empty();
//        $("#InsurancePlan").prop("disabled", true);
//    }
//}

function 
BindLocationType() {
    $("#LocationSubType").empty();
    if ($("#LocationType").val() != "" && parseInt($("#LocationType").val()) > 0 && parseInt($("#LocationType").val()) == _LocationTypeUrban) {
        $("#LocationSubType").parent("div").parent("div").show();
        
        $("#LocationSubType").prop("disabled", false);
        var options = {};
        var GetLocationSubTypeByLocationTypeIdUrl = $_GetLocationSubTypeByLocationTypeIdUrl;
        options.url = GetLocationSubTypeByLocationTypeIdUrl;
        options.type = "POST";
        options.beforeSend= function () {
            new fn_showMaskloader('Please wait...');
        }
        options.complete= function () {
            fn_hideMaskloader();

        }
        options.data = JSON.stringify({ LocationType: $("#LocationType option:selected").val() });
        options.dataType = "json";
        options.contentType = "application/json";
        options.success = function (LocationType) {
            var LocationTypeID = null;
            $("#LocationSubType").append("<option value=''>--Select--</option>");
            for (var i = 0; i < LocationType.GlobalCodeList.length; i++) {

                $("#LocationSubType").append("<option value=" + LocationType.GlobalCodeList[i].GlobalCodeId + ">" + LocationType.GlobalCodeList[i].CodeName + "</option>");
                if (parseInt($_LocationTypeID) == LocationType.GlobalCodeList[i].GlobalCodeId) {
                    LocationTypeID = i;
                }
            }
            if (LocationTypeID != null) {
              //--Modified By Manoj Jaswal
              //  var StateName = State.GlobalCodeList[LocationTypeID].CodeName;
              //  $("#LocationSubType option")
              //.filter(function () { return $.trim($(this).text()) == StateName; })
              //.attr('selected', true);

                $("#LocationSubType").val($_LocationTypeID);
                $("#LocationSubTypeDesc").val(LocationSubTypeDesc);
            }
        };
        options.error = function () { alert("Error retrieving Location Sub Type!"); };
        $.ajax(options);
    }
    else {
        $("#LocationSubType").empty();
        $("#LocationSubType").prop("disabled", true);
        $("#LocationSubType").parent("div").parent("div").hide();
        $("#LocationSubTypeDesc").parent("div").parent("div").hide();
       
    }
}

function BindLocationByAdminID(methodUrl) {
    console.log("BindLocationByAdminID");
    $("#LocationId").empty();
    if ($("#ddlAdministrator").val() != "") {
        $("#LocationId").prop("disabled", false);

        var options = {};
        //options.url = "/Dropdown/GetStateByCountryID";
        //options.url = '@Url.Action("GetStateByCountryID", "Dropdown")'
        //var GetStateByCountryIDUrl = $('#StateByCountryIDUrl').val();
        var GetLocationByAdminIDUrl = methodUrl //$_GetLocationByAdminIDUrl
        options.url = GetLocationByAdminIDUrl;
        options.type = "POST";
        options.data = JSON.stringify({ userId: $("#ddlAdministrator").val() });
        options.dataType = "json";
        options.contentType = "application/json";
        options.success = function (Data) {

            //  var _AssetID = null;
            $("#LocationId").append("<option value=''>--Select--</option>");
            for (var i = 0; i < Data.length; i++) {

                $("#LocationId").append("<option value=" + Data[i].Value + ">" + Data[i].Text + "</option>");
                //if (parseInt($_AssetID) == Data[i].Value) {
                //    _AssetID = i;
                //}
            }
            //if (_AssetID != null) {

            //    var StateName = Data[_AssetID].Text;
            //    $("#ddlAssetID option")
            //  .filter(function () { return $.trim($(this).text()) == StateName; })
            //  .attr('selected', true);

            //    $("#ddlAssetID").val($_AssetID);
            //}
        };
        options.error = function () { alert("Error retrieving Asset!"); };
        $.ajax(options);
    }
    else {
        $("#LocationId").empty();
        $("#LocationId").prop("disabled", true);
    }

}

function BindManager() {
    $("#ddlManager").empty();
    if ($("#ddlAdministrator").val() != "") {
        // $("#ddlManager").prop("disabled", false);
        var options = {};
        //options.url = "/Dropdown/GetStateByCountryID";
        //options.url = '@Url.Action("GetStateByCountryID", "Dropdown")'
        //var GetStateByCountryIDUrl = $('#StateByCountryIDUrl').val();
        var BindManager = $_BindManager
        options.url = BindManager;
        options.type = "POST";
        options.data = JSON.stringify({ AdminID: $("#ddlAdministrator").val() });
        options.dataType = "json";
        options.contentType = "application/json";
        options.success = function (Data) {

            $("#ddlManager").empty();
            //var _AssetID = null;
            $("#ddlManager").append("<option value=''>--Select--</option>");
            for (var i = 0; i < Data.length; i++) {
                $("#ddlManager").append("<option value=" + Data[i].UserId + ">" + Data[i].Name + "</option>");
                //if (parseInt($_AssetID) == Data[i].Value) {
                //    _AssetID = i;
                //}
            }
            //if (_AssetID != null) {

            //    var StateName = Data[_AssetID].Text;
            //    $("#ddlAssetID option")
            //  .filter(function () { return $.trim($(this).text()) == StateName; })
            //  .attr('selected', true);

            //    $("#ddlAssetID").val($_AssetID);
            //}
        };
        options.error = function () { alert("Error retrieving Manager!"); };
        $.ajax(options);
    }
    else {
        $("#ddlManager").empty();
        $("#ddlManager").prop("disabled", true);
    }
}