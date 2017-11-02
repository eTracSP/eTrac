$(function () {

    var colname = ['Name', 'Email', 'DOB', 'ProjectID', 'Location', 'EmployeeID', 'Permission Levels', 'HiringDate', 'Actions'];

    var colModel = [{ name: 'Name', width: 100, sortable: true },
               { name: 'UserEmail', width: 80, sortable: false },
               { name: 'DOB', width: 80, sortable: false, hidden: true },
               { name: 'ProjectID', width: 80, sortable: false, hidden: true },
               { name: 'Location', width: 80, sortable: false },
               { name: 'EmployeeID', width: 80, sortable: false },
               //{ name: 'UserType', width: 80, sortable: false, hidden: ($_controllerName != undefined && $_controllerName == "Manager") ? true : false },
               { name: 'UserType', width: 80, sortable: false, hidden: false },
               { name: 'HiringDate', width: 80, sortable: false },
               { name: 'act', index: 'act', width: 50, sortable: false }];

    $("#tbl_ManagerList").jqGrid({
        url: '../GridListing/JqGridHandler/VerifiedMnagaerList.ashx',
        datatype: 'json',
        height: 400,
        width: 700,
        autowidth: true,
        //colNames: ['Name', 'Email', 'DOB', 'ProjectID', 'Location', 'EmployeeID', 'Permission Levels', 'HiringDate', 'Actions'],
        colNames: colname,
        colModel: colModel,
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divManagerListPager',
        sortname: 'Location',
        viewrecords: true,
        sortorder: 'desc',
        caption: "List of Manager",

        gridComplete: function () {
            var text;
            var ids = jQuery("#tbl_ManagerList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                var rowData = jQuery("#tbl_ManagerList").getRowData(cl);
                var ProjectID = rowData['ProjectID'];
                if (ProjectID == 0) {
                    text = 'Assign Location';
                }
                else {
                    text = 'Change Location';
                }

                be = '<div><a href="javascript:void(0)" data-toggle = "modal" data-target = "#myModalmedium" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;">' + text + '<span class="ui-icon ui-icon-pencil"></span></a>'

                jQuery("#tbl_ManagerList").jqGrid('setRowData', ids[i], { act: be });
            }
            //Added By Bhushan Dod on 30/03/2015 if record < 20 then pager become hide 
            if ($("#tbl_ManagerList").getGridParam("records") <= 20) {
                $("#divManagerListPager").hide();
            }
            else {
                $("#divManagerListPager").show();
            }
            if ($('#tbl_ManagerList').getGridParam('records') === 0) {
                $('#tbl_ManagerList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }

        },

        caption: '<div class="header_search"><input id="txtSearch" class="inputSearch" placeholder="Search by Name" onkeydown="doSearch(arguments[0]||event)" type="text"></div>'
    });
    if ($("#tbl_ManagerList").getGridParam("records") > 20) {
    jQuery("#tbl_ManagerList").jqGrid('navGrid', '#divManagerListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
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
    jQuery("#tbl_ManagerList").jqGrid('setGridParam', { url: "../GridListing/JqGridHandler/VerifiedMnagaerList.ashx?txtSearch=" + txtSearch, page: 1 }).trigger("reloadGrid");
}
$(".EditRecord").live("click", function (event) {

    var id = $(this).attr("Id");

    var rowData = jQuery("#tbl_ManagerList").getRowData(id);
    var Name = rowData['Name'];
    var UserEmail = rowData['UserEmail'];
    var ProjectID = rowData['ProjectID'];
    var Location = rowData['Location'];
    var HiringDate = rowData['HiringDate'];
    var id = id;

    //$("#mediumeditpopup").load("/GlobalAdmin/_AssignProject", {
    //    'Name': Name, 'UserEmail': UserEmail,
    //    'id': id
    //});

    //var url = '@Url.Action("_AssignProject", "GlobalAdmin")';
    $('#mediumeditpopup').load("../GlobalAdmin/_AssignProject", { 'Name': Name, 'UserEmail': UserEmail, 'id': id, 'ProjectID': ProjectID, 'HiringDate': HiringDate }
        , function () {
            $('#dHiringDate').datepicker({ dateFormat: 'dd/mm/yy' });
            //$("#mediumeditpopup form").validate();
            $("form").removeData("validator");
            $("form").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse("form");

        });

});