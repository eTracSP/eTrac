$(function () {
    $("#tbl_EmployeeList").jqGrid({
        url: '../GridListing/JqGridHandler/VerifiedUserList.ashx',
        datatype: 'json',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['Name', 'Email',  'Employee Profile', 'HiringDate','EmployeeCategoryid', 'Actions'],
        colModel: [{ name: 'Name', width: 100, sortable: true },
                  { name: 'UserEmail', width: 80, sortable: false },
                  { name: 'EmployeeProfile', width: 80, sortable: false },
                  { name: 'HiringDate', width: 80, sortable: false },
                   { name: 'EmployeeCategoryid', width: 80, sortable: false,hidden:true },
                  { name: 'act', index: 'act', width: 50, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divEmployeeListPager',
        sortname: 'Name',
        viewrecords: true,
        sortorder: 'desc',
        caption: "List of Employee",

        gridComplete: function () {
            var text;
            var ids = jQuery("#tbl_EmployeeList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                var rowData = jQuery("#tbl_EmployeeList").getRowData(cl);
                var EmployeeCategoryid = rowData['EmployeeCategoryid'];
                if (EmployeeCategoryid == 0) {
                    text = 'Assign Profile';
                }
                else {
                    text = 'Change Profile';
                }

                be = '<div><a href="javascript:void(0)" data-toggle = "modal" data-target = "#myModalmedium" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;">' + text + '<span class="ui-icon ui-icon-pencil"></span></a>'

                jQuery("#tbl_EmployeeList").jqGrid('setRowData', ids[i], { act: be });
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
        caption: '<div class="header_search"><input id="txtSearch" class="inputSearch" placeholder="Search by Name" onkeydown="doSearch(arguments[0]||event)" type="text"></div>'
    });
    if ($("#tbl_EmployeeList").getGridParam("records") > 20) {
    jQuery("#tbl_EmployeeList").jqGrid('navGrid', '#divEmployeeListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
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
    jQuery("#tbl_EmployeeList").jqGrid('setGridParam', { url: "../GridListing/JqGridHandler/VerifiedUserList.ashx?txtSearch=" + txtSearch, page: 1 }).trigger("reloadGrid");
}
$(".EditRecord").live("click", function (event) {
   
    var id = $(this).attr("Id");

    var rowData = jQuery("#tbl_EmployeeList").getRowData(id);
    var Name = rowData['Name'];
    var UserEmail = rowData['UserEmail'];
    var EmployeeProfile = rowData['EmployeeProfile'];
    var HiringDate = rowData['HiringDate'];
    var EmployeeCategoryid = rowData['EmployeeCategoryid'];
    var id = id;

    //$("#mediumeditpopup").load("/GlobalAdmin/_AssignProject", {
    //    'Name': Name, 'UserEmail': UserEmail,
    //    'id': id
    //});

   // var url = '@Url.Action("_AssignProfile", "GlobalAdmin")';
    $('#mediumeditpopup').load("../Manager/_AssignProfile", { 'id': id, 'Name': Name, 'UserEmail': UserEmail, 'EmployeeCategoryid': EmployeeCategoryid, 'HiringDate': HiringDate }
        , function () {
        $('#dHiringDate').datepicker({ dateFormat: 'dd/mm/yy' });           
        $("form").removeData("validator");
        $("form").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse("form");
        $('.modal-title').text("Assign Profile");
    });

});