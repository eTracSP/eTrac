var ListUserUrl = '../GlobalAdmin/GetListITAdministrator';
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
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        //emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        caption: "List of IT Administrator",
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
                de = '<div><a href="javascript:void(0)"  onclick="GetUserRoles(this);"  class="Assign notepad2 " Id="' + cl + '" title="Roles & Premissions" style=" float: left;margin-right: 10px;cursor:pointer;"><span class=" icon-cog fa-2x texthover-bluelight"></span><span class="tooltips">Assign</span></a></div>';
                if (colusrtyp[i] == 'IT Administrator') {
                    jQuery("#tbl_ManagerList").jqGrid('setRowData', ids[i], { act: be + ee });
                }
                else { jQuery("#tbl_ManagerList").jqGrid('setRowData', ids[i], { act: be + ee + de }); }
                var ab = new Array(19);
            }
            if ($("#tbl_ManagerList").getGridParam("records") <= 10) {
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
    })

    $('#toggle-two').change(function () {       
        ViewAllRecords();       
    });

    if ($("#tbl_ManagerList").getGridParam("records") > 10) {
        jQuery("#tbl_ManagerList").jqGrid('navGrid', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
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
    if (UserType == "IT Administrator") {

        window.location.href = UserListUrl.ITAdminEdit + '?usr=' + id;
    }
    else if (UserType == "Administrator") {

        window.location.href = UserListUrl.AdminEdit + '?usr=' + id;
    }
    else if (UserType == "Manager") {

        window.location.href = UserListUrl.ManagerEdit + '?usr=' + id;
    }
    else if (UserType == "Employee") {
        window.location.href = UserListUrl.EmployeeEdit + '?usr=' + id;
    }
    else if (UserType == "Client") {
        window.location.href = UserListUrl.ClientEdit + '?usr=' + id;
    }
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
    // jQuery("#tbl_QRCList").jqGrid('setGridParam', { url: QRCurl + "?locationId=" + locaId + "&SearchText=" + txtSearch.trim() + "&SearchQRCType=" + selectSearchQRCType, page: 1 }).trigger("reloadGrid");
    jQuery("#tbl_ManagerList").jqGrid('setGridParam', { url: ListUserUrl + "?SearchText=" + txtSearch + "&UserType=" + UserType + "&locationId=" + locaId, page: 1 }).trigger("reloadGrid");
}
function DeleteUser(attr) {   
    //Modified by Ashwajit Bansod for checking is delete user assigned wo or not.
    if (attr != null && attr != '') {
        var id = $(attr).attr("Id");
        var rowData = jQuery("#tbl_ManagerList").getRowData(id);
        if (rowData.UserType == 'Employee') {
            $.ajax({
                type: "GET",
                url: '../Common/CheckingContinuousAssign',
                contentType: "application/json; charset=utf-8",
                dataType: "html",
                data: {UserID: $(attr).attr("Id")},
                beforeSend: function () {
                    new fn_showMaskloader('Please wait...');
                },
                success: function (Data) {
                    //if user contain wo assigned
                    if (Data.trim() != null && Data.trim() != '') {
                        $("#ContinuousWorkData").html(Data);
                        $("#ContinuousModal").modal('show');
                    }
                    //go for delete.
                    else {
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
                                                $('#tbl_ManagerList').trigger('reloadGrid');
                                                gridReload();
                                            },
                                            error: function () {
                                                alert("Error:")
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
                        })
                    }
                },
                error: function () {
                    alert("Error:")
                },
                complete: function () {
                    fn_hideMaskloader();
                }
            })
        }
        else {
            bootbox.dialog({
                message: "are you sure you want to delete this user?",
                buttons: {
                    success: {
                        label: "delete",
                        classname: "btn btn-primary",
                        callback: function () {
                            $.ajax({
                                type: "post",
                                url: '../common/deleteuser',
                                data: { userid: $(attr).attr("id") },
                                beforesend: function () {
                                    new fn_showmaskloader('please wait...');
                                },
                                success: function (data) {
                                    toastr.success(data.message);
                                    $('#tbl_managerlist').trigger('reloadgrid');
                                    gridreload();
                                },
                                error: function () {
                                    alert("error:")
                                },
                                complete: function () {
                                    fn_hidemaskloader();
                                }
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
        }
    }
}
function GetUserRoles(attr) {
    $("#myModalmedium").modal('show');
    $("#myModalmedium #myModalLabel").html("User Roles");
    $("#myModalmedium").addClass('UserRolesEditCSS');

    $('#mediumeditpopup').load("../RolesAndPermissions/_PermissionsDisplay", { 'id': $(attr).attr("Id") });
}
function EnlargeImageView(value) {
    $("#myAssignedWorkOrder").modal('show');
    $("#myAssignedWorkOrder").find("#myModalLabel").html("");
    var path = $(value).attr("src");
    var data = "<img src='" + path + "' style='width:100%;height:100%;' />";
    $("#myAssignedWorkOrder").find("#AssignedWorkorderBody").html(data);

}

