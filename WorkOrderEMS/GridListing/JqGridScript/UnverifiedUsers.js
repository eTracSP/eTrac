var ListUserUrl = '../GlobalAdmin/GetListOfUnverifiedUsers';
var EditUserUrl = '../GlobalAdmin/EditUser';
var PopUpUrl = '../Manager/_AssignProfile';
var DeleteUrl = '../Common/DeleteUser';

var UserTypeOptions = "";
if ($Login_UserType == 1) { //Global Admin
    UserTypeOptions = '<option value="All Users">All Users</option><option value="IT Administrator">IT Administrator</option><option value="Administrator">Administrator</option><option value="Manager">Manager</option><option value="Employee">Employee</option><option value="Client">Client</option>';
}
if ($Login_UserType == 2) {//Manager
    UserTypeOptions = '<option value="All Users">All Users</option><option value="Employee">Employee</option><option value="Client">Client</option>';
}
if ($Login_UserType == 3) {//Employee
    UserTypeOptions = '';
}
if ($Login_UserType == 4) {//client
    UserTypeOptions = '';
}
if ($Login_UserType == 5) {//It Administrator
    UserTypeOptions = '<option value="All Users">All Users</option><option value="Administrator">Administrator</option><option value="Manager">Manager</option><option value="Employee">Employee</option><option value="Client">Client</option>';
}
if ($Login_UserType == 6) {//Administrator
    UserTypeOptions = '<option value="All Users">All Users</option><option value="Manager">Manager</option><option value="Employee">Employee</option><option value="Client">Client</option>';
}
$(function () {
    var _ListUserType = $('#hiddnListUserType').val();
    if (_ListUserType == undefined || _ListUserType.trim() == '') { _ListUserType = 'listuser'; }
    var UserType = $("#drp_UserTypeSelection").val();
    if (UserType == undefined) UserType = "All Users";
    var _adminflag = (_ListUserType == 'listadministrator' || _ListUserType == 'listitadministrator') ? false : true;
    $("#tbl_ManagerList").jqGrid({
        url: ListUserUrl,
        datatype: 'json',
        height: 400,
        mtype: 'GET',
        width: 700,
        //postData: { UserType: 5, UserId: null, NumberOfRows: 20, PageIndex: 1, SortOrderBy: 'asc', SortColumnName: 'UserEmail', SearchText: '' },
        //postData: {  NumberOfRows: 20, PageIndex: 1, SortOrderBy: 'asc', SortColumnName: 'UserEmail', SearchText: '' },
        postData: { UserType: UserType },
        autowidth: true,
        colNames: ['Name', 'Email', 'Location', 'User Type', 'DOB', 'Profile Image', 'Employee Profile', 'Actions'],
        colModel: [{ name: 'Name', width: 80, sortable: true },
                  { name: 'UserEmail', width: 100, sortable: false },
                  { name: 'Location', width: 100, sortable: false, hidden: _adminflag },
                  { name: 'UserType', width: 50, sortable: false, hidden: !_adminflag },
                  //{ name: 'UserType', width: 80, sortable: false, hidden: ($_controllerName != undefined && $_mycolflag == "False") ? false : false },
                  { name: 'DOH', width: 50, sortable: false },
                  { name: 'ProfileImage', width: 40, sortable: false, title: false, formatter: imageFormat },
                  { name: 'EmployeeProfile', width: 50, sortable: false, hidden: true },
                  { name: 'act', index: 'act', width: 50, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divManagerListPager',
        sortname: 'Name',
        viewrecords: true,
        emptyrecords: "No records to view",
        shrinkToFit: true,
        sortorder: 'asc',
        caption: "List of UnVerified User",
        loadError: function (data) { $('#message').html('no record found.'); },
        gridComplete: function () {
            $('#message').html('');
            //var text;
            // ;
            //alert("UserType under complete");
            var ids = jQuery("#tbl_ManagerList").jqGrid('getDataIDs');
            var colusrtyp = jQuery("#tbl_ManagerList").getCol('UserType');

            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                var editUrl = colusrtyp[i];
                var rowData = jQuery("#tbl_ManagerList").getRowData(cl);
                var Name = rowData['Name'];
                var EmployeeCategoryid = rowData['EmployeeCategoryid'];
                if (EmployeeCategoryid == 0) {
                    text = 'Assign Profile';
                }
                else {
                    //text = 'Change Profile';
                    text = 'Assign Location';
                }
                //be = '<div><a href="javascript:void(0)" data-toggle = "modal" data-target = "#myModalmedium" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;">' + text + '<span class="ui-icon ui-icon-pencil"></span></a>'
                //roshan be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;">edit<span class="ui-icon ui-icon-pencil"></span></a>'

                be = '<div><a href="javascript:void(0)" class="EditRecord " Id="' + cl + '" title="edit" roshandata=' + editUrl + ' style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a></div>';
                ee = '<div><a href="javascript:void(0)" onclick="DeleteUser(this);" class="deleteRecord " Id="' + cl + '" title="Delete" roshandata=' + DeleteUrl + ' style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a></div>';
                vi = '<a href="javascript:void(0)" class="viewRecord qrc doubleClassIcon" onclick="loadpreview(this)" title="view" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="glyphicon glyphicon-list-alt fa-2x texthover-solidblue"></span><span class="tooltips">Detail</span></a></div>';
                IsVerify = '<div><a href="javascript:void(0)" class="verify" onclick="IsVerifyUser(this)" title="verify" vid="' + cl + '" name="' + Name + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-solidblue"></span><span class="tooltips">Verify</span></a></div>';

                jQuery("#tbl_ManagerList").jqGrid('setRowData', ids[i], { act: IsVerify + be + ee + vi });

                var ab = new Array(19);

            }
            if ($("#tbl_ManagerList").getGridParam("records") <= 20) {
                $("#divManagerListPager").hide();
            }
            else {
                //jQuery("#tbl_ManagerList").jqGrid('navGrid', '#divManagerListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
                $("#divManagerListPager").show();
            }
            $("#toggle-two").parent('div').addClass("ViewAllButton");
            $("#toggle-two").parent('div').removeClass('toggle.btn');
            $("#toggle-two").siblings('div').children('label').attr('style', 'background-color:#84cfe6');
            if ($('#tbl_ManagerList').getGridParam('records') === 0) {
                $('#tbl_ManagerList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },

        caption: ($Login_UserType == 4) ? '<div class="header_search"><input id="txtSearch" class="inputSearch" placeholder="Search By Name" onkeydown="doSearch(arguments[0]||event)" type="text">&nbsp;<Select id="drp_UserTypeSelection" onchange="gridReload()" title="Select User Type">' + UserTypeOptions + '</select>&nbsp;</div>' : '<div class="header_search"><input id="txtSearch" class="inputSearch" placeholder="Search by Name" onkeydown="doSearch(arguments[0]||event)" type="text">&nbsp;<Select id="drp_UserTypeSelection" onchange="gridReload()" title="Select User Type">' + UserTypeOptions + '</select>&nbsp; <input id="toggle-two" class="ViewAllButton" type="checkbox"><script>$(function() {$("#toggle-two").bootstrapToggle({on: "View All Location Enabled",off: "View All Location Disabled"});})</script></div>' //<input type="button" value="View All Locations" class="ViewAllButton" onclick="ViewAllRecords();" title="Click to view user on All Locations."/>
    });

    $('#toggle-two').change(function () {
        ViewAllRecords();
    });

    if ($("#tbl_ManagerList").getGridParam("records") > 20) {
        jQuery("#tbl_ManagerList").jqGrid('navGrid', '#divManagerListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }

});

var timeoutHnd;
var flAuto = true;
UserType = $("#drp_UserTypeSelection").val();

function doSearch(ev) {
    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
}

function gridReload() {
    UserType = $("#drp_UserTypeSelection").val();
    var txtSearch = $("#txtSearch").val();
    var locaId = $('#toggle-two').prop('checked') == true ? 0 : $("#drp_MasterLocation :selected").val();

    jQuery("#tbl_ManagerList").jqGrid('setGridParam', { url: ListUserUrl + "?txtSearch=" + txtSearch + "&UserType=" + UserType + "&locationId=" + locaId, page: 1 }).trigger("reloadGrid");
}

$(".EditRecord").live("click", function (event) {
    var id = $(this).attr("Id");
    var edturl = $(this).attr("roshandata");
    var rowData = jQuery("#tbl_ManagerList").getRowData(id);
    var Name = rowData['Name'] != undefined ? rowData['Name'] : null;
    var UserEmail = rowData['UserEmail'] != undefined ? rowData['UserEmail'] : null;
    var EmployeeProfile = rowData['EmployeeProfile'] != undefined ? rowData['EmployeeProfile'] : null;
    var HiringDate = rowData['HiringDate'] != undefined ? rowData['HiringDate'] : null;
    var EmployeeCategoryid = rowData['EmployeeCategoryid'] != undefined ? rowData['EmployeeCategoryid'] : null;
    var id = id;
    var UserType = $("#drp_UserTypeSelection").val();
    if (UserType == undefined || UserType == "" || UserType == null || UserType == "All Users") {
        UserType = rowData['UserType'];
    }

    window.location.href = UserListUrl.CommonEdit + '?usr=' + id;
    //if (UserType == "IT Administrator") {

    //    window.location.href = UserListUrl.ITAdminEdit + '?usr=' + id;
    //}
    //else if (UserType == "Administrator") {

    //    window.location.href = UserListUrl.AdminEdit + '?usr=' + id;
    //}
    //else if (UserType == "Manager") {

    //    window.location.href = UserListUrl.ManagerEdit + '?usr=' + id;
    //}
    //else if (UserType == "Employee") {
    //    window.location.href = UserListUrl.EmployeeEdit + '?usr=' + id;
    //}
    //else if (UserType == "Client") {
    //    window.location.href = UserListUrl.ClientEdit + '?usr=' + id;
    //}
});
function imageFormat(cellvalue, options, rowObject) {
    if (cellvalue == "" || cellvalue == null)
    { return ""; }
    else {
        return '<img src="' + cellvalue + '" class="gridimage" cursor: pointer;" onclick="EnlargeImageView(this);"/>';
    }
}
function ViewAllRecords() {
    UserType = $("#drp_UserTypeSelection").val();
    var txtSearch = jQuery("#txtSearch").val();
    var locaId = $('#toggle-two').prop('checked') == true ? 0 : $("#drp_MasterLocation :selected").val();
    if (locaId == 0) {
        $("#drp_MasterLocation").hide();
    }
    else {
        $("#drp_MasterLocation").show();
    }

    jQuery("#tbl_ManagerList").jqGrid('setGridParam', { url: ListUserUrl + "?SearchText=" + txtSearch + "&UserType=" + UserType + "&locationId=" + locaId, page: 1 }).trigger("reloadGrid");
}
function DeleteUser(attr) {


    bootbox.dialog({
        message: "Are you sure you want to delete this user?",
        buttons: {
            success: {
                label: "Delete",
                className: "btn btn-primary",
                callback: function () {
                    $.ajax({
                        type: "POST",
                        url: '../Common/DeleteUser',
                        data: { UserID: $(attr).attr("Id") },
                        beforeSend: function () {
                            new fn_showMaskloader('Please wait...');
                        },
                        success: function (Data) {
                            toastr.success(Data.message);
                            gridReload();
                        },
                        error: function () {

                        },
                        complete: function () {
                            fn_hideMaskloader();
                        }
                    });

                }
            },
            danger: {
                label: "Cancel",
                className: "btn btn-primary",
                callback: function () {

                }
            }

        }
    });

}
function GetUserRoles(attr) {

    $("#myModalmedium").modal('show');
    $("#myModalmedium #myModalLabel").html("User Roles");

    $('#mediumeditpopup').load("../RolesAndPermissions/_PermissionsDisplay", { 'id': $(attr).attr("Id") });
}
function EnlargeImageView(value) {
    $("#myAssignedWorkOrder").modal('show');
    $("#myAssignedWorkOrder").find("#myModalLabel").html("");
    var path = $(value).attr("src");
    var data = "<img src='" + path + "' style='width:100%;height:100%;' />";
    $("#myAssignedWorkOrder").find("#AssignedWorkorderBody").html(data);

}
function loadpreview(result) {
    var empId = $(result).attr("vid");


    $.ajax({
        url: '../Common/getUserDetailsByUserId',
        data: { 'usr': empId.toString() },
        type: 'POST',
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (Data) {
            $("#DivForUserDetail").html(Data);
            $("#clickForUserDetails").trigger('click');
        },
        error: function () {

        },
        complete: function () {
            fn_hideMaskloader();
        }
    });


    //var gender = "";

    //if (EmployeeListModel[id].Gender == 10)
    //{ gender = "Female" } else if (EmployeeListModel[id].Gender == 9) { gender = "Male" }
    //$("#View_Gender").html(gender);
    ////if (UserID == null || UserID == undefined || UserID == "0" || UserID == 0) {
    ////    var imgPath = $_hostingPrefix + "Content/Images/ProfilePic/" + EmployeeListModel[id].ProfileImageFile;
    ////} else {
    //var imgPath = $_hostingPrefix + "Content/Images/ProfilePic/" + EmployeeListModel[id].ProfileImageFile;
    ////}
    //$("#View_myProfileImage").attr("src", imgPath);
    //var str = EmployeeListModel[id].DOB;
    //var res = str.replace("-", "/");
    //res = res.replace("-", "/");

    //$("#View_DOB").html(res);


    //$("#View_Address1").html(EmployeeListModel[id].Address.ZipCode);
    //$("#View_Address1").html(EmployeeListModel[id].Address.Mobile);
    //$("#View_Address_Phone").html(EmployeeListModel[id].Address.Phone);
    //$("#View_AddressMasterId").html(EmployeeListModel[id].Address.AddressMasterId);
    //$("#View_ModalConfirumationPreview").modal('show');
    //$("#View_AlternateEmail").attr("disabled", true);
    //$("#View_EmployeeID").attr("disabled", true);


}

function IsVerifyUser(attr) {
    var name = $(attr).attr("name");
    bootbox.dialog({
        message: "Are you sure you want to verify '"+name+"'?",
        buttons: {
            success: {
                label: "Verify",
                className: "btn btn-primary",
                callback: function () {
                    $.ajax({
                        type: "POST",
                        url: '../Common/IsVerifiedUserList',
                        data: { UserID: $(attr).attr("vid") },
                        beforeSend: function () {
                            new fn_showMaskloader('Please wait...');
                        },
                        success: function (Data) {
                           toastr.success(Data)
                            gridReload();
                        },
                        error: function (e) {
                        },
                        complete: function () {
                            fn_hideMaskloader();
                        }
                    });

                }
            },
            danger: {
                label: "Cancel",
                className: "btn btn-primary",
                callback: function () {

                }
            }

        }
    });

}

