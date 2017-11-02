//var edata = [
//        { UserEmail: "1", FirstName: "2010-05-24", LastName: "test", Mobile: "note" },

//];
// ;
//var mydata = JSON.stringify(EmployeeListModel)
//var ss = "^(?('')(''.+?(?<!\\)''@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))' + '(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
$(function () {



    jQuery("#tbl_EmployeeList").jqGrid({
        data: empdata,
        datatype: "json",
        height: 150,
        width: 1000,
        rowNum: 10,
        rowList: [10, 20, 30],
        colNames: ['User Email', 'First Name', 'Last Name', 'Employee ID', 'Mobile', 'Actions'],
        colModel: [

            { name: 'UserEmail', index: 'UserEmail', width: 100 },
            { name: 'FirstName', index: 'FirstName', width: 50 },
            { name: 'LastName', index: 'LastName', width: 50 },
            { name: 'EmployeeID', index: 'Employee ID', width: 50 },
            { name: 'Mobile', index: 'Mobile', width: 50 },
            { name: 'act', index: 'act', width: 50, sortable: false }
        ],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: "#divEmployeeListPager",
        viewrecords: true,
        caption: "Employee Data",
        loadComplete: function () {
            if ($('#tbl_EmployeeList').getGridParam('records') === 0) {
                $('#tbl_EmployeeList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        gridComplete: function () {
            var ids = jQuery("#tbl_EmployeeList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = '<div><a href="javascript:void(0)" class="EditRecord" recordId="' + i + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
                de = '<a href="javascript:void(0)" class="deleteRecord" recordId="' + i + '" title="delete"  style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
                vi = '<a href="javascript:void(0)" onclick="loadpreview(' + i + ')" class="viewRecord Assign approveQRCIcon" title="view" vid="' + i + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="glyphicon glyphicon-list-alt texthover-bluelight"></span><span class="tooltips">Detail</span></a>';
                jQuery("#tbl_EmployeeList").jqGrid('setRowData', ids[i], { act: be + de + vi });
            }
            //Added By Bhushan Dod on 30/03/2015 if record < 20 then pager become hide 
            if ($("#tbl_EmployeeList").getGridParam("records") <= 20) {
                $("#divEmployeeListPager").hide();
            }
            else {
                $("#divEmployeeListPager").show();
            }
            if ($('#tbl_EmployeeList').getGridParam('records') === 0) {
                $('#tbl_EmployeeList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }

        },

    });

    $(".EditRecord").live("click", function (event) {
        jQuery("[id='Password']").each(function () { $(this).parent("div").parent("div").hide(); })
        jQuery("[id='AlternateEmail']").each(function () { $(this).parent("div").parent("div").hide(); })
        jQuery("[id='EmployeeID']").each(function () { $(this).parent("div").parent("div").hide(); })
        clearallfieldsLocation();

        var id = $(this).attr("recordId");
        curEmp = id;
        var UserID = EmployeeListModel[id].UserId;
        $("#UserId").val(UserID)
        $("#EmployeeID").val(EmployeeListModel[id].EmployeeID)
        $("#UserEmail").val(EmployeeListModel[id].UserEmail);
        $("#JobTitle").val(EmployeeListModel[id].JobTitle);
        $("#FirstName").val(EmployeeListModel[id].FirstName);
        $("#LastName").val(EmployeeListModel[id].LastName);
        $("#AlternateEmail").val(EmployeeListModel[id].AlternateEmail);
        $("#Password").val(EmployeeListModel[id].Password);
        $("#Gender").val(EmployeeListModel[id].Gender);
        //if (UserID == null || UserID == undefined || UserID == "0" || UserID == 0) {
        //    var imgPath = $_hostingPrefix + "Content/Images/ProfilePic/" + EmployeeListModel[id].ProfileImageFile;
        //} else {
        var imgPath = $_hostingPrefix + "Content/Images/ProfilePic/" + EmployeeListModel[id].ProfileImageFile;
        //}
        $("#myProfileImage").attr("src", imgPath);
        var str = EmployeeListModel[id].DOB;
        var res = str.replace("/", "-");
        res = res.replace("/", "-");
        $("#DOB").val(res);
        $("#Address_Address1").val(EmployeeListModel[id].Address.Address1);
        $("#Address_Address2").val(EmployeeListModel[id].Address.Address2);
        $("#EmployeeStates").val(EmployeeListModel[id].Address.StateId);
        $("#Address_City").val(EmployeeListModel[id].Address.City);
        $("#Address_ZipCode").val(EmployeeListModel[id].Address.ZipCode);
        $("#Address_Mobile").val(EmployeeListModel[id].Address.Mobile);
        $("#Address_Phone").val(EmployeeListModel[id].Address.Phone);
        $("#AddressMasterId").val(EmployeeListModel[id].Address.AddressMasterId);
        $("#ModalConfirumationPreview").modal('show');
        $("#AlternateEmail").attr("disabled", true);
        $("#EmployeeID").attr("disabled", true);
        $('.field-validation-error')
                          .removeClass('field-validation-error')
                          .addClass('field-validation-valid');

        $('.input-validation-error')
            .removeClass('input-validation-error')
            .addClass('valid');
    });

    $("#btnAddNewEmployee").live("click", function () {
        jQuery("[id='Password']").each(function () { $(this).parent("div").parent("div").show(); })
        jQuery("[id='AlternateEmail']").each(function () { $(this).parent("div").parent("div").show(); })
        jQuery("[id='EmployeeID']").each(function () { $(this).parent("div").parent("div").show(); })
        //$("#ModalConfirumationPreview").find("input").val("");
        $("#ModalConfirumationPreview").find("input").each(function () {
            var typ = this.type;
            if (typ == "text" || typ == "password" || typ == "email") {
                $(this).val("");
            }

        })
        $("#EmployeeStates").val("");
        $("#AlternateEmail").removeAttr("disabled");
        $("#EmployeeID").removeAttr("disabled");
        $("#UserId").val("");
        $("#myProfileImage").attr("src", $_hostingPrefix + "Content/Images/ProjectLogo/no-profile-pic.jpg");
    })
    $(".deleteRecord").live("click", function (event) {

        var id = $(this).attr("recordId");
        showPopupRelativeMessage("Are you sure want to delete Employee?", $(this), "Delete Employee", function () {
            //    $.ajax({
            //        type: "POST",
            //        data: "{'id':'" + id + "'}",
            //        //url: '../GridListing/JqGrid Handler/StaffUserList.ashx?id=' + id,
            //        url: '../Manager/DeleteVendor/',
            //        dataType: "json",
            //        contentType: "application/json; charset=utf-8",
            //        error: function (xhr, status, error) {
            //            closeAjaxProgress();
            //        },
            //        beforeSend: function () {
            //            showAjaxProgress();
            //        },
            //        success: function (result) {
            //            // AlertMessage(result);
            //            $("#message").html(result.Message);
            //            $("#message").addClass(result.AlertMessageClass);
            //            closeAjaxProgress();

            //            jQuery("#tbl_EmployeeList").jqGrid().trigger("reloadGrid");
            //            event.stopPropagation();
            //            gridReload();
            //        },
            //        Complete: function (result) {
            //            closeAjaxProgress();
            //            event.stopPropagation();
            //        }
            //    });

            EmployeeListModel = jQuery.grep(EmployeeListModel, function (n, i) {
                return i != id
            });
            empdata = jQuery.grep(empdata, function (n, i) {
                return i != id
            });
            //$('#tbl_EmployeeList').jqGrid('delRowData', id);
            //jQuery("#tbl_EmployeeList").jqGrid().trigger("reloadGrid");

            jQuery('#tbl_EmployeeList').jqGrid('clearGridData');
            jQuery('#tbl_EmployeeList').jqGrid('setGridParam', { data: empdata });
            jQuery('#tbl_EmployeeList').trigger('reloadGrid');

        });

    });


    function clearallfieldsLocation() {



        //$.each($('#divConfirumationPreview input'), function (index, value) {

        //    if (value.type == 'text' || value.type == 'email' || value.name == 'DOB') {
        //        $('#' + value.id).val('');
        //    }
        //});
        $.each($('#divConfirumationPreview select'), function (index, valuee) {
            $('#' + valuee.id).val('-1');
        });
    }
});
function GetEmployeesOnLoactionBases() {
    var LocId = $("#LocationId").val()
    if (LocId == 0 || LocId == "" || LocId == undefined) { }
    else {
        $.ajax({
            type: "POST",
            url: $_ListUserUrl,
            data: { Loc_ID: $("#LocationId").val() },
            beforeSend: function () {

                new fn_showMaskloader('Please wait...');
            },
            success: function (Data) {
                //EmployeeListModel.push(Data);
                EmployeeListModel = [];
                empdata = [];
                var _EmployeeListModel = {};
                var _Emp = {};
                var obj = {};
                for (i = 0; i < Data.length; i++) {
                    var Address = {};
                    var obj = {};
                    Address.AddressMasterId = Data[i].AddressMasterId
                    Address.Address1 = Data[i].Address1;
                    Address.Address2 = Data[i].Address2;
                    Address.City = Data[i].City;
                    Address.CountryId = Data[i].CountryId;
                    Address.Mobile = Data[i].Mobile;
                    Address.Phone = Data[i].Phone;
                    Address.StateId = Data[i].StateId;
                    Address.ZipCode = Data[i].ZipCode;
                    obj.AlternateEmail = Data[i].AlternateEmail;
                    obj.EmployeeID = Data[i].EmployeeID;
                    obj.FirstName = Data[i].FirstName;
                    obj.Gender = Data[i].Gender;
                    obj.JobTitle = Data[i].JobTitle;
                    obj.UserId = Data[i].UserId;
                    obj.LastName = Data[i].LastName;
                    obj.UserEmail = Data[i].UserEmail;
                    if (Data[i].DOB != '' || Data[i].DOB != null) {
                        //var dd = "";
                        //dd = Data[i].DOB.split('/')[0] + '-' + Data[i].DOB.split('/')[1] + '-' + Data[i].DOB.split('/')[2];
                        obj.DOB = Data[i].DOB;

                        
                    }
                    obj.ProfileImageFile = Data[i].ProfileImage;
                    obj.Address = {}
                    obj.Address = Address;
                    EmployeeListModel.push(obj);
                    var obj1 = {};
                    obj1.FirstName = Data[i].FirstName;
                    obj1.LastName = Data[i].LastName;
                    obj1.Mobile = Data[i].Mobile;
                    obj1.UserEmail = Data[i].UserEmail;
                    obj1.EmployeeID = Data[i].EmployeeID;
                    empdata.push(obj1);
                }
                //empdata.push(obj);
                jQuery("#tbl_EmployeeList")
                             .jqGrid('setGridParam',
                                 {
                                     datatype: 'local',
                                     data: empdata
                                 })
                             .trigger("reloadGrid");
            },
            error: function (ex) {
                alert("Error:");
            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    }
}
function loadpreview(result) {
    var id = result;
    var UserID = EmployeeListModel[id].UserId;
    $("#View_UserId").html(UserID)
    $("#View_EmployeeID").html(EmployeeListModel[id].EmployeeID)
    $("#View_UserEmail").html(EmployeeListModel[id].UserEmail);
    $("#View_JobTitle").html(EmployeeListModel[id].JobTitle);
    $("#View_FirstName").html(EmployeeListModel[id].FirstName);
    $("#View_LastName").html(EmployeeListModel[id].LastName);
    $("#View_AlternateEmail").html(EmployeeListModel[id].AlternateEmail);
    $("#View_Password").html(EmployeeListModel[id].Password);
    var gender = "";

    if (EmployeeListModel[id].Gender == 10)
    { gender = "Female" } else if (EmployeeListModel[id].Gender == 9) { gender = "Male" }
    $("#View_Gender").html(gender);
    //if (UserID == null || UserID == undefined || UserID == "0" || UserID == 0) {
    //    var imgPath = $_hostingPrefix + "Content/Images/ProfilePic/" + EmployeeListModel[id].ProfileImageFile;
    //} else {
    var imgPath = "";
    if (EmployeeListModel[id].ProfileImageFile == "" || EmployeeListModel[id].ProfileImageFile == undefined) {
        imgPath = "/Content/Images/ProjectLogo/no-profile-pic.jpg";
    }
    else {
        imgPath = $_hostingPrefix + "Content/Images/ProfilePic/" + EmployeeListModel[id].ProfileImageFile;
    }
    //}
    $("#View_myProfileImage").attr("src", imgPath);
    var str = EmployeeListModel[id].DOB;
    var res = str.replace("/", "-");
    res = res.replace("/", "-");

    $("#View_DOB").html(res);

    $("#View_Address_Address1").html(EmployeeListModel[id].Address.Address1);
    $("#View_Address_Address2").html(EmployeeListModel[id].Address.Address2);
    var stateid = EmployeeListModel[id].Address.StateId;
    $("#View_EmployeeStates").html($("#EmployeeStates option[value='" + stateid + "']").html());
    var CountryId = EmployeeListModel[id].Address.CountryId;
    $("#View_EmployeeCountry").html($("#EmployeeCountry option[value='" + CountryId + "']").html());
    $("#View_Address_City").html(EmployeeListModel[id].Address.City);
    $("#View_Address_ZipCode").html(EmployeeListModel[id].Address.ZipCode);
    $("#View_Address_Mobile").html(EmployeeListModel[id].Address.Mobile);
    $("#View_Address_Phone").html(EmployeeListModel[id].Address.Phone);
    $("#View_AddressMasterId").html(EmployeeListModel[id].Address.AddressMasterId);
    $("#View_ModalConfirumationPreview").modal('show');
    $("#View_AlternateEmail").attr("disabled", true);
    $("#View_EmployeeID").attr("disabled", true);

    $("#ModalEmployeeViewPreview").modal('show');
    $("#EmployeeStates").val($("#States :selected").val());
}
