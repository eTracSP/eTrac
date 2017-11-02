var ListUserUrl = '../GridListing/JqGridHandler/NotAssignedUsers.ashx';
var EditUserUrl = '../GridListing/JqGridHandler/NotAssignedUsers.ashx';
var PopUpUrl = '../GridListing/JqGridHandler/NotAssignedUsers.ashx';
var DeleteUrl = '../Common/DeleteUser';

var UserTypeOptions = "";
if ($Login_UserType == 1) { //Global Admin
    UserTypeOptions = '<option value="">All Users</option><option value="Administrator">Administrator</option><option value="Manager">Manager</option><option value="Employee">Employee</option>';
}
if ($Login_UserType == 2) {//Manager
    UserTypeOptions = '<option value="Employee">Employee</option>';
}
if ($Login_UserType == 3) {//Employee
    UserTypeOptions = '';
}
if ($Login_UserType == 4) {//client
    UserTypeOptions = '';
}
if ($Login_UserType == 5) {//It Administrator
    UserTypeOptions = '<option value="">All Users</option><option value="Administrator">Administrator</option><option value="Manager">Manager</option><option value="Employee">Employee</option>';
}
if ($Login_UserType == 6) {//Administrator
    UserTypeOptions = '<option value="">All Users</option><option value="Manager">Manager</option><option value="Employee">Employee</option>';
}
$(function () {
    var _ListUserType = $('#hiddnListUserType').val();
    if (_ListUserType == undefined || _ListUserType.trim() == '') { _ListUserType = 'listuser'; }
    var UserType = $("#drp_UserTypeSelection").val();
    if (UserType == undefined) UserType = "";
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
        colNames: ['Names', 'Email', 'User Type', 'DOB', 'Actions'],
        colModel: [{ name: 'Name', width: 80, sortable: true },
                  { name: 'UserEmail', width: 100, sortable: false },
                  { name: 'CodeName', width: 50, sortable: false, hidden: !_adminflag },
                  //{ name: 'UserType', width: 80, sortable: false, hidden: ($_controllerName != undefined && $_mycolflag == "False") ? false : false },
                  { name: 'DOH', width: 50, sortable: false },
                  //{ name: 'ProfileImage', width: 40, sortable: false, title: false, formatter: imageFormat },
                  { name: 'act', index: 'act', width: 50, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divManagerListPager',
        sortname: 'Name',
        viewrecords: true,
        gridview: true,
        loadonce: true,
        multiSort: true,
        rownumbers: true,
        sortorder: 'asc',
        caption: "List of IT Administrator",
        loadError: function (data) { $('#message').html('no record found.'); },
        gridComplete: function () {
            $('#message').html('');
            //var text;
            //debugger;
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

                be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" roshandata=' + editUrl + ' style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a></div>';
                ee = '<div><a href="javascript:void(0)" onclick="DeleteUser(this);" class="deleteRecord " Id="' + cl + '" title="Delete" roshandata=' + DeleteUrl + ' style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a></div>';
                vi = '<a href="javascript:void(0)" class="viewRecord qrc doubleClassIcon" onclick="loadpreview(this)" title="view" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="glyphicon glyphicon-list-alt fa-2x texthover-solidblue"></span><span class="tooltips">Detail</span></a></div>';
                ai = '<div><a href="javascript:void(0)"  onclick="AssignLocationAndRoles(this);"  class="Assign notepad2 " Id="' + cl + '" title="Assign" style=" float: left;margin-right: 10px;cursor:pointer;"><span class=" icon-cog fa-2x texthover-bluelight"></span><span class="tooltips">Assign</span></a></div>';
               
                jQuery("#tbl_ManagerList").jqGrid('setRowData', ids[i], { act: be + ee + vi +ai });
            }
            if ($('#tbl_ManagerList').getGridParam('records') === 0) {
                $('#tbl_ManagerList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div class="header_search"><input id="txtSearch" class="inputSearch" placeholder="Search by Name" onkeydown="doSearch(arguments[0]||event)" type="text">&nbsp;<Select id="drp_UserTypeSelection" onchange="gridReload()" title="Select User Type">' + UserTypeOptions + '</select>&nbsp;</div>'
    });
    jQuery("#tbl_ManagerList").jqGrid('navGrid', '#divManagerListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });

});

var timeoutHnd;
var flAuto = true;
function doSearch(ev) {
    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
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
function gridReload() {

    UserType = $("#drp_UserTypeSelection").val();
    var txtSearch = jQuery("#txtSearch").val();

    //jQuery("#tbl_ManagerList").jqGrid('setGridParam', { url: "../GridListing/JqGridHandler/NotAssignedUsers.ashx?txtSearch=" + txtSearch + "&UserType=" + UserType.toString() + "&page: 1" }).trigger("reloadGrid");
    jQuery("#tbl_ManagerList").jqGrid('setGridParam', { url: "../GridListing/JqGridHandler/NotAssignedUsers.ashx?txtSearch=" + txtSearch + "&UserType=" + UserType.toString(), page: 1 }).trigger("reloadGrid");
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


    var UserType = rowData["CodeName"];
   
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
    //window.location.href = EditUserUrl + '?userId=' + id;


    //$('#mediumeditpopup').load(PopUpUrl, { 'id': id, 'Name': Name, 'UserEmail': UserEmail, 'EmployeeCategoryid': EmployeeCategoryid, 'HiringDate': HiringDate }
    //    , function () {
    //        $('#dHiringDate').datepicker({ dateFormat: 'dd/mm/yy' });
    //        $("form").removeData("validator");
    //        $("form").removeData("unobtrusiveValidation");
    //        $.validator.unobtrusive.parse("form");
    //        $('.modal-title').text("Assign Profile");
    //        $("#myModalmedium").modal("show");
    //    });

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
    jQuery("#tbl_ManagerList").jqGrid('setGridParam', { url: ListUserUrl + "?SearchText=" + txtSearch + "&UserType=" + UserType + "&locationId=" + 0, page: 1 }).trigger("reloadGrid");
}
function DeleteUser(attr) {
   //debugger
    bootbox.confirm("Are you sure you want to delete this user?", function (result) {
        if (result == true) {
            $.ajax({
                type: "POST",
                url: '../Common/DeleteUser',
                data: { UserID: $(attr).attr("Id") },
                datatype:"json",
                beforeSend: function () {
                    new fn_showMaskloader('please wait...');
                    
                },
                success: function (Data) {
                    toastr.success(Data.message);
                    jQuery("#tbl_ManagerList").jqGrid().trigger("reloadGrid");
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

    });

}
function AssignLocationAndRoles(event) {
  
    $('#mediumeditpopup').html('');
    var id = $(event).attr("Id");
    var rowData = jQuery("#tbl_ManagerList").getRowData(id);
    var Name = rowData['Name'] != undefined ? rowData['Name'] : null;
    $("#myModalmedium").modal('show');
    $("#myModalmedium #myModalLabel").html("Assign Location & Roles");
    $('#mediumeditpopup').load("../RolesAndPermissions/_AssignLocationAndRoles", { 'id': id, 'name': Name });
}
function EnlargeImageView(value) {
    $("#myAssignedWorkOrder").modal('show');
    $("#myAssignedWorkOrder").find("#myModalLabel").html("");
    var path = $(value).attr("src");
    var data = "<img src='" + path + "' style='width:100%;height:100%;' />";
    $("#myAssignedWorkOrder").find("#AssignedWorkorderBody").html(data);

}
