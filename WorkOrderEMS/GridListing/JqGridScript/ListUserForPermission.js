var ListUserUrl = '../RolesAndPermissions/GetListUserForPermission';
var EditUserUrl = '../GlobalAdmin/EditUser';
var PopUpUrl = '../Manager/_AssignProfile';

var userTypeddl = ''
    + '<select id="SearchUserType" class="" style="margin-left:120px;" onchange="doSearch(arguments[0]||event);">'
    //+ testqRCTypeddl + '</select>';
/**/ + '<option value="" >Select</option>'
+ '<option value="1">Global Admin</option>'
+ '<option value="5">IT Administrator</option>'
+ '<option value="6">Administrator</option>'
+ '<option value="2">Manager</option>'
+ '<option value="4">Client</option>'
+ '<option value="3">Employee</option> </select>';



$(function () {
    $("#tbl_UserListForPermission").jqGrid({
        url: ListUserUrl,
        datatype: 'json',
        height: 400,
        mtype: 'POST',
        width: 700,
        autowidth: true,
        colNames: ['Name', 'Email', 'Location', 'User Type', 'DOB', 'Employee ID', 'Employee Profile', 'Actions'],
        colModel: [{ name: 'Name', width: 80, sortable: true },
                  { name: 'UserEmail', width: 100, sortable: false },
                  { name: 'Location', width: 100, sortable: false,hidden :true},
                  { name: 'UserType', width: 50, sortable: false},
                  { name: 'DOB', width: 50, sortable: false },
                  { name: 'EmployeeID', width: 80, sortable: false },
                  { name: 'EmployeeProfile', width: 50, sortable: false, hidden: true },
                  { name: 'act', index: 'act', width: 60, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divUserListForPermissionPager',
        sortname: 'Name',
        viewrecords: true,
        sortorder: 'desc',
        caption: "List of Users",
        loadError: function (data) { $('#message').html('no record found.'); },
        gridComplete: function () {
            $('#message').html('');
            //var text;
            // ;
            //alert("UserType under complete");
            var ids = jQuery("#tbl_UserListForPermission").jqGrid('getDataIDs');
            var colusrtyp = jQuery("#tbl_UserListForPermission").getCol('UserType');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                var editUrl = colusrtyp[i];
                var rowData = jQuery("#tbl_UserListForPermission").getRowData(cl);
                
                //be = '<div><a href="javascript:void(0)" data-toggle = "modal" data-target = "#myModalmedium" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;">' + text + '<span class="ui-icon ui-icon-pencil"></span></a>'
                //roshan be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;">edit<span class="ui-icon ui-icon-pencil"></span></a>'
               
                be = '<div><a href="javascript:void(0)"  data-toggle = "modal" data-target = "#myModalmedium"  class="Assign notepad2" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="ui-icon ui-icon-notepad"></span></a>'
              
                    jQuery("#tbl_UserListForPermission").jqGrid('setRowData', ids[i], { act: be });
            }
            //Added By Bhushan Dod on 30/03/2015 if record < 20 then pager become hide 
            if ($("#tbl_UserListForPermission").getGridParam("records") <= 20) {
                $("#divUserListForPermissionPager").hide();
            }
            else {
                $("#divUserListForPermissionPager").show();
            }
            if ($('#tbl_UserListForPermission').getGridParam('records') === 0) {
                $('#tbl_UserListForPermission tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },

        caption: '<div class="header_search"><input id="txtSearch" class="inputSearch" placeholder="Search by Name" onkeydown="doSearch(arguments[0]||event)" type="text">' + userTypeddl + '</div>'
    });
    if ($("#tbl_UserListForPermission").getGridParam("records") > 20) {
        jQuery("#tbl_UserListForPermission").jqGrid('navGrid', '#divUserListForPermissionPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }
});

var timeoutHnd;
var flAuto = true;
function doSearch(ev) {
    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
}

function gridReload() {
    var txtSearch = jQuery("#txtSearch").val();
    var selectedUserType = jQuery("#SearchUserType").val();
    selectedUserType = (selectedUserType != undefined && selectedUserType != '' && parseInt(selectedUserType, 10) > 0) ? parseInt(selectedUserType, 10) : null;
    var selectedLocation = jQuery("#Location").val();
    selectedLocation = (selectedLocation != undefined && selectedLocation != '' && parseInt(selectedLocation, 10) > 0) ? parseInt(selectedLocation, 10) : null;
    jQuery("#tbl_UserListForPermission").jqGrid('setGridParam', { url: ListUserUrl + "?SearchText=" + txtSearch + "&locationId=" + selectedLocation + "&UserType=" + selectedUserType, page: 1 }).trigger("reloadGrid");
}


//$(".Assign").live("click", function (event) {

//    var id = $(this).attr("Id");
//    //var rowData = jQuery("#tbl_InventoryList").getRowData(id);
//    //var ItemName = rowData['ItemName'];
//    //var AssignInventoryID = rowData['AssignInventoryID'];
//    //var AssignedUserID = rowData['AssignedUserID'];
//    //var IssueDate = rowData['IssueDate'];
//    //var IssuedBy = rowData['IssuedBy'];
//    //var AssignedToName = rowData['AssignedToName'];
//    ////window.location.href = '../Manager/AssignInventory/?id=' + id + '&AssignInventoryID=' + AssignInventoryID + '&AssignedUserID=' + AssignedUserID + '&IssueDate=' + IssueDate + '&IssuedBy=' + IssuedBy + '&AssignedToName=' + AssignedToName;
//    $('#largeeditpopup').load("../Manager/_AssignInventory", { 'id': id }
//       , function () {
//           $('.modal-title').text("Assign Inventory");
//       });
//});




//$(".EditRecord").live("click", function (event) {
//    //alert('roshan caled me');
//    var id = $(this).attr("Id");
//    var edturl = $(this).attr("roshandata");
//    var rowData = jQuery("#tbl_UserListForPermission").getRowData(id);
//    var Name = rowData['Name'] != undefined ? rowData['Name'] : null;
//    var UserEmail = rowData['UserEmail'] != undefined ? rowData['UserEmail'] : null;
//    var EmployeeProfile = rowData['EmployeeProfile'] != undefined ? rowData['EmployeeProfile'] : null;
//    var HiringDate = rowData['HiringDate'] != undefined ? rowData['HiringDate'] : null;
//    var EmployeeCategoryid = rowData['EmployeeCategoryid'] != undefined ? rowData['EmployeeCategoryid'] : null;
//    var id = id;
//     ;
//    EditUserUrl = '../GlobalAdmin/EditUser';
//    EditUserUrl = "../" + edturl + "/Index";
//    EditUserUrl = EditUserUrl;
//    alert(EditUserUrl);
//    window.location.href = EditUserUrl + '?usr=' + id;
//    //window.location.href = EditUserUrl + '?userId=' + id;


//    //$('#mediumeditpopup').load(PopUpUrl, { 'id': id, 'Name': Name, 'UserEmail': UserEmail, 'EmployeeCategoryid': EmployeeCategoryid, 'HiringDate': HiringDate }
//    //    , function () {
//    //        $('#dHiringDate').datepicker({ dateFormat: 'dd/mm/yy' });
//    //        $("form").removeData("validator");
//    //        $("form").removeData("unobtrusiveValidation");
//    //        $.validator.unobtrusive.parse("form");
//    //        $('.modal-title').text("Assign Profile");
//    //    });

//});


$(".Assign").live("click", function (event) {
   
//    // ;
    var id = $(this).attr("Id");
//    //var rowData = jQuery("#tbl_InventoryList").getRowData(id);
//    //var ItemName = rowData['ItemName'];
//    //var AssignInventoryID = rowData['AssignInventoryID'];
//    //var AssignedUserID = rowData['AssignedUserID'];
//    //var IssueDate = rowData['IssueDate'];
//    //var IssuedBy = rowData['IssuedBy'];
//    //var AssignedToName = rowData['AssignedToName'];
//    //// var Quantity = rowData['AssignedQuantity'];
//    ////window.location.href = '../Manager/AssignInventory/?id=' + id + '&AssignInventoryID=' + AssignInventoryID + '&AssignedUserID=' + AssignedUserID + '&IssueDate=' + IssueDate + '&IssuedBy=' + IssuedBy + '&AssignedToName=' + AssignedToName;
    //    alert('call');
    $('#mediumeditpopup').load("../RolesAndPermissions/_PermissionsDisplay", { 'id': id }
       , function () {
//           alert('success');
//            ;
//           //var d = new Date();
//           //var curr_year = d.getFullYear();
//           //$("#IssueDate").datepicker({});
//           //$("form").removeData("validator");
//           //$("form").removeData("unobtrusiveValidation");
//           //$.validator.unobtrusive.parse("form");
             $('.modal-title').text("Set Permissions");
           });
    
});