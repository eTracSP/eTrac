

var forAdminOrForManager = 6; var ttcheck = 1; var LocationAddress = ""; var locationname = "";
var msg = "";
$(function () {

    $('#ModalConfirumationPreview').on('hidden.bs.modal', function () {

        $("#divConfirumationPreview").html('');

        $("#userRoleWaliDiv").css('display', 'none');
    })


    if (!$("#userRoleWaliDiv").is(':visible')) {
        ttcheck = 0;
    }

    $("#AddUserType").change(function () {
        $(this).css("border", "1px solid #ccc");
        $("#ModalConfirumationPreview").children('div').addClass("modal-large");
        $("#ModalConfirumationPreview").children('div').children('div').first().addClass("modelleftside");
        if (ttcheck == 0) {
            ttcheck = 1;
            $("name=['userRoleBind']").css('display', 'block');
            $("#ModalConfirumationPreview div:nth-child(2)").show();
        }
        $("#profileImage").attr("src", getImagePath($(this).val()));
        getAssignedPermissions($(this).val(), $('#hiddnSelectLocation').val());


    }); //AddUserType end block

    $("#tbl_LocationList").jqGrid({

        //url: '../GridListing/JqGridHandler/LocationList.ashx',
        url: '../GridListing/JqGridHandler/ListLocation.ashx',
        datatype: 'json',
        height: 400,
        width: 700,
        autowidth: true,

        //colNames: ['Location Name', 'Address', 'LocationAdministrator', 'LocationClient', 'City', 'State', 'Country', 'Contact No.', 'Description', 'Actions', ],
        colNames: ['Location', 'Address', 'Administrator', 'Manager', 'Employee', 'Actions', ],
        colModel: [{ name: 'LocationName', width: 80, sortable: true },
                  { name: 'Address', width: 100, sortable: false },
                  { name: 'LocationAdministrator', width: 45, sortable: false },
                  { name: 'LocationManager', width: 45, sortable: false },
                  //{ name: 'LocationClient', width: 80, sortable: false },
                  { name: 'LocationEmployee', width: 45, sortable: false },

                  //{ name: 'City', width: 80, sortable: false },
                  //{ name: 'State', width: 80, sortable: false },
                  //{ name: 'Country', width: 80, sortable: false },
                  //{ name: 'ContactNo', width: 80, sortable: false },
                  //{ name: 'Description', width: 100, sortable: false },
                  //{ name: 'QRCID', width: 80, sortable: false },
                  { name: 'act', index: 'act', width: 50, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divLocationListPager',
        sortname: 'LocationId',
        viewrecords: true,
        sortorder: 'desc',
        caption: "List of Locations",

        onCellSelect: function (rowid, iCol, cellcontent) {
            var rowData = $(this).jqGrid("getRowData", rowid);
            LocationAddress = rowData.Address;
            locationname = rowData.LocationName;
            $("#profileImage").attr("src", '../Content/Images/ProfilePic/no-profile-pic.jpg');
            $('#hiddnSelectLocation').val(rowid);
            $("#ModalConfirumationPreview").children('div').removeClass("modal-large");
            $("#ModalConfirumationPreview").children('div').children('div').first().removeClass("modelleftside");
            if (iCol == 2) { //You clicked on admin block


                forAdminOrForManager = 6;
                $('#divForUserTypeHeading').html("Select Administrator")
                $('#pop_admin_manager').html('Administrator');
                aa = $("#tbl_LocationList").jqGrid('getCell', rowid, 'LocationAdministrator');
                locationname = $("#tbl_LocationList").jqGrid('getCell', rowid, 'LocationName');
                $.ajax({
                    url: '../GlobalAdmin/GetListLocationAdministrator',
                    data: JSON.stringify({ LocationId: rowid, UserType: 6 }),
                    async: false,
                    type: 'POST',
                    contentType: "application/json",
                    success: function (data) {

                        $("#AddUserType").html('');
                        $("#AddUserType").append('<option value="-1"> Select</option>');


                        bindAdminusers(rowid, 6);
                        $.each(data, function (i, v) {


                            if (IsAlternateRow == true)
                            { RowBgColor = '#FFF'; IsAlternateRow = false; }  //ededed
                            else
                            { RowBgColor = '#D9EDF7'; IsAlternateRow = true; } //d0d0d0

                            var imagepath = (v.ProfileImage != "" && v.ProfileImage != 'System.Web.HttpPostedFileWrapper') ? v.ProfileImage : "no-profile-pic.jpg";

                            if (imagepath == null)
                            { imagepath = "no-profile-pic.jpg"; }

                            /*new */

                            var jobtitle = v.JobTitle == "" ? "" : v.JobTitle;
                            var codename = v.CodeName == "" ? "" : ' <div class="clearfix"> </div>  <label class="col-sm-4 control-label">Code Name</label>' + v.CodeName == null ? "" : v.CodeName;
                            var _adminname = v.Name;

                            $("#divConfirumationPreview").append('<div  class="col-lg-6 col-md-6 col-sm-6 col-xs-12" >' +
                                '<div class="assign-location-user">' +
                                 '<img src="../Content/Images/ProfilePic/' + imagepath + '" alt="Image not found" style="width: 75px; height: 75px;">' +
                                 '<p><strong>' + _adminname + '</strong></p>' +
                                '<p>' + v.UserEmail + '</p>' +
                                '<p>' + jobtitle + '</p>' +
                                '<button class="btn btn-danger" cursor:pointer; uid="' + v.UserId + '" uname="' + v.Name + '" title="Remove Permission" onclick="fnRemoveAdminPersmission(' + "'" + v.Name.toString() + "'" + ',' + v.UserId + ');">X</button> ' +
                            '</div></div>');
                        });


                    },
                    error: function (errordata) {

                        console.log('error');
                    }
                })
                $("#poplocationname").text('');
                $("#poplocationname").html('   ' + locationname).css("color", "black");
                if (aa != "") {

                }

                $("#ModalConfirumationPreview").modal('show');
            }
            else if (iCol == 3) { //associate Manager Cell
                forAdminOrForManager = 2;


                $('#pop_admin_manager').html('Manager');
                $('#divForUserTypeHeading').html("Select Manager")
                aa = cellcontent;

                if (aa != "" && aa != "&nbsp;") {
                    $.ajax({
                        url: '../GlobalAdmin/GetListLocationAdministrator',
                        data: JSON.stringify({ LocationId: rowid, UserType: 2 }),
                        async: false,
                        type: 'POST',
                        contentType: "application/json",
                        success: function (data) {


                            //Binding div with list of manager user.
                            $.each(data, function (i, v) {

                                var jobtitle = v.JobTitle == "" ? "" : v.JobTitle;
                                var imagepath = (v.ProfileImage != "" && v.ProfileImage != 'System.Web.HttpPostedFileWrapper') ? v.ProfileImage : "no-profile-pic.jpg";
                                $("#divConfirumationPreview").append('<div  class="col-lg-6 col-md-6 col-sm-6 col-xs-12" >' +
                                    '<div class="assign-location-user">' +
                                     '<img src="../Content/Images/ProfilePic/' + imagepath + '" alt="image not found" style="width: 75px; height: 75px;">' +
                                     '<p><strong>' + v.Name + '</strong></p>' +
                                    '<p>' + v.UserEmail + '</p>' +
                                    '<p>' + jobtitle + '</p>' +
                                    '<button class="btn btn-danger" cursor:pointer; uid="' + v.UserId + '" uname="' + v.Name + '" title="Remove Permission" onclick="fnRemoveManagerPersmission(' + "'" + v.Name.toString() + "'" + ',' + v.UserId + ');">X</button> ' +
                                '</div></div>');
                            });


                            $("#AddUserType").html('');
                            $("#AddUserType").append('<option value="-1"> Select</option>');

                            bindAdminusers(rowid, 2);
                        },
                        error: function (errordata) {

                            alert('error');
                        }
                    })


                    var IsAlternateRow = false;
                    var RowBgColor = '';
                    locationname = $("#tbl_LocationList").jqGrid('getCell', rowid, 'LocationName');
                    $("#poplocationname").text('');
                    $("#poplocationname").text(locationname);

                    $("#ModalConfirumationPreview").modal('show');
                }
            }
            else if (iCol == 4) { // You clicked on Employe columns
                forAdminOrForManager = 3;
                $('#pop_admin_manager').html('Employee');
                $('#divForUserTypeHeading').html("Select Employee");
                aa = cellcontent;
                if (aa != "" && aa != "&nbsp;") {
                    $.ajax({
                        url: '../GlobalAdmin/GetListLocationAdministrator',
                        data: JSON.stringify({ LocationId: rowid, UserType: 3 }), // 3 stand for employee type user 
                        async: false,
                        type: 'POST',
                        contentType: "application/json",
                        success: function (data) {


                            //Binding div with list of manager user.
                            $.each(data, function (i, v) {
                                if (IsAlternateRow == true) { RowBgColor = '#ededed'; IsAlternateRow = false; }
                                else { RowBgColor = '#d0d0d0'; IsAlternateRow = true; }
                                $('#poplocationid').val();

                                var jobtitle = v.JobTitle == "" ? "" : v.JobTitle;
                                var imagepath = (v.ProfileImage != "" && v.ProfileImage != 'System.Web.HttpPostedFileWrapper') ? v.ProfileImage : "no-profile-pic.jpg";

                                $("#divConfirumationPreview").append('<div  class="col-lg-6 col-md-6 col-sm-6 col-xs-12" >' +
                                    '<div class="assign-location-user">' +
                                     '<img src="../Content/Images/ProfilePic/' + imagepath + '" alt="image not found" style="width: 75px; height: 75px;">' +
                                     '<p><strong>' + v.Name + '</strong></p>' +
                                    '<p>' + v.UserEmail + '</p>' +
                                    '<p>' + jobtitle + '</p>' +
                                    '<button class="btn btn-danger" cursor:pointer; uid="' + v.UserId + '" uname="' + v.Name + '" title="Remove Permission" onclick="fnRemoveEmployeePersmission(' + "'" + v.Name.toString() + "'" + ',' + v.UserId + ');">X</button> ' +
                                '</div></div>');

                            });
                            $("#AddUserType").html('');
                            $("#AddUserType").append('<option value="-1"> Select</option>');

                            bindAdminusers(rowid, 3);
                        },
                        error: function (errordata) {
                            alert('error');
                        }
                    })




                    var IsAlternateRow = false;
                    var RowBgColor = '';
                    locationname = $("#tbl_LocationList").jqGrid('getCell', rowid, 'LocationName');
                    $("#poplocationname").text('');
                    $("#poplocationname").text(locationname);

                    $("#ModalConfirumationPreview").modal('show');
                }
            }
        },
        loadError: function (data) { $('#message').html('No records found.'); },
        gridComplete: function () {
            $('#message').html('');
            var ids = jQuery("#tbl_LocationList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
                de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
                vi = '<a href="javascript:void(0)" class="viewRecord qrc approveQRCIcon" onclick="loadpreview(this)" title="view" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="glyphicon glyphicon-list-alt fa-2x texthover-solidblue"></span><span class="tooltips">View</span></a></div>';
                jQuery("#tbl_LocationList").jqGrid('setRowData', ids[i], { act: be + de + vi });
            }
            if ($("#tbl_LocationList").getGridParam("records") <= 20) {
                $("#divLocationListPager").hide();
            }
            else {
                $("#divLocationListPager").show();
            }
            if ($('#tbl_LocationList').getGridParam('records') === 0) {
                $('#tbl_LocationList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div class="header_search"><input id="txtSearch" class="inputSearch"  placeholder="Search by Location" onkeydown="doSearch(arguments[0]||event)" width="220px;" type="text"></div>'
    });
    if ($("#tbl_LocationList").getGridParam("records") > 20) {
        jQuery("#tbl_LocationList").jqGrid('navGrid', '#divLocationListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }
});

var timeoutHnd;
var flAuto = true;
function doSearch(ev) {
    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
}


//added by vijay sahu on 6 may 2015

function getImagePath(UserIdIs) {
    var imgPath = "";
    $.ajax({
        type: 'GET',
        url: '../Common/getProfilePicture',
        data: { usr: UserIdIs },
        async: false,
        contentType: "application/json",
        success: function (dataresult) {
            imgPath = dataresult;
        },
        error: function (err, e, er) {
        }

    });
    return imgPath;
}
function getAssignedPermissions(UserIdIs, locationId) {

    $("#userRoleWaliDiv").css('display', 'block');
    $("#serviceToLocation").empty();

    $.ajax({
        type: 'GET',
        url: '../GlobalAdmin/GetAssignPermission',
        data: { UserIdIs: UserIdIs, locationId: locationId },
        async: false,
        contentType: "application/json",
        success: function (data) {

            sdata = "";
            for (var i = 0; i < data.data.length; i++) {
                sdata = sdata + '<div class="checkbox squaredTwo"><input type="checkbox" class="large" id="' + data.data[i].PermissionId + '"name="checkboxPermissions" />&nbsp;<span>' + data.data[i].PermissionName + '</span><label for="squaredTwo"></label></div>'

            }

            $("#serviceToLocation").append(sdata);

            var j = 0;
            $('#serviceToLocation input:checkbox').each(function () {
                for (j = 0 ; j < data.notAll.length; j++) {

                    var k = j;
                    console.log(this.id);
                    console.log(data.notAll[j].PermissionId);
                    if (parseInt(this.id) === data.notAll[j].PermissionId) {

                        $(this).attr("checked", "checked");

                    }
                }
            });

            $('input[type="checkbox"]').checkbox({
                buttonStyle: 'btn-link fa-1x',
                buttonStyleChecked: 'btn-link fa-1x',
                checkedClass: 'icon-check',
                uncheckedClass: 'icon-check-empty',
                constructorCallback: null,
                defaultState: true,
                defaultEnabled: true,
                checked: true,
                enabled: true

            });

            $('input[type="checkbox"].large').checkbox({
                buttonStyle: 'btn-link btn-large',
                checkedClass: 'icon-check',
                uncheckedClass: 'icon-check-empty'
            });

        },
        error: function (err, e, er) {

        }
    });
}
///Created by vijay sahu 
/// This function is used for binding admin user to dropDown.
function bindAdminusers(rowid, UserType) {

    $.ajax({

        url: '../GlobalAdmin/UnAssignedAdministrationId',
        data: JSON.stringify({ LocationId: rowid, UserType: UserType }),
        async: false,
        type: 'POST',
        contentType: "application/json",
        success: function (result) {

            $.each(result, function (i, v) {
                //alert('my aa gya idhr');
                $("#AddUserType").append('<option value="' + v.Value + '">' + v.Text + '</option>')
            });

        },
        error: function (er) { }

    });

}
function gridReload() {
    var txtSearch = jQuery("#txtSearch").val();
    jQuery("#tbl_LocationList").jqGrid('setGridParam', { url: "../GridListing/JqGridHandler/ListLocation.ashx?txtSearch=" + txtSearch, page: 1 }).trigger("reloadGrid");
}
//$(".EditRecord").die("click");
//$(".EditRecord").live("click", function (event) {
//$(".EditRecord").bind("click", function (event) {
$(".EditRecord").live("click", function (event) {
    var id = $(this).attr("Id");

    window.location.href = '../GlobalAdmin/EditLocationSetup/?loc=' + id;

    //$("#largeeditpopup").load('../StaffUser/EditStaffUser/' + id);
});
$(".deleteRecord").live("click", function (event) {
    
    var id = $(this).attr("cid");
    showPopupRelativeMessage("Do you want to delete this Location?", $(this), "Delete Location", function () {
        $.ajax({
            type: "Post",
            data: "{'id':'" + id + "'}",
            //url: '../GridListing/JqGrid Handler/StaffUserList.ashx?id=' + id,
            url: '../GlobalAdmin/DeleteLocation/',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            error: function (xhr, status, error) {
                closeAjaxProgress();
            },
            beforeSend: function () {
                new fn_showMaskloader('Please wait...');
            },
            success: function (result) {
          
                //$("#message").html(result.Message);
                //$("#message").addClass(result.AlertMessageClass);
                //$("#message").show();
                //setInterval(function () { $("#message").html(""); }, 10000);
                //closeAjaxProgress();
                //jQuery("#tbl_LocationList").jqGrid().trigger("reloadGrid");
                toastr.success(result.Message)
                //event.stopPropagation();
                gridReload();
            },
            error: function () {
                alert("Error:")
            },
            complete: function () {
           
                fn_hideMaskloader();
            }
        });
    });
});


function fnShareLocation() {
    var check = $(":checkbox:checked").length;

    var RolesIds = getUserRoles();
    if (parseInt(forAdminOrForManager) == 6) //6 stand for admin user 
    {

        locationname = $('#poplocationname').html().trim();
        var adminname = $('#AddUserType option:selected').text();
        var selectlocation = $('#hiddnSelectLocation').val();

        if (adminname.trim() == "Select") {
            $("#AddUserType").css("border-color", "red");
            bootbox.alert("Please select the administrator.")
            return false;
        }
        else {
            //$('.modal-content').addClass('modalfade2');
            $("#AddUserType").css("border", "0px solid white");
          
            if (check != '0') {
                if (RolesIds === "")
                { msg = "Are you sure you want to share <strong>" + locationname + "</strong> administrative rights with <strong>" + adminname + "</strong> ?"; }
                else
                {
                    msg = "Are you sure you want to share <strong>" + locationname + "</strong> administrative rights with <strong>" + adminname + "</strong>?";
                }

                bootbox.confirm(msg, function (result) {
                     //bootbox.confirm("Are you sure to share all rights for location: <strong>" + locationname + "</strong> with Administrator:" + adminname + " ?", function (result) {
                    if (result) {
                        new fn_showMaskloader('Please wait...');
                        $.ajax({
                            url: '../GlobalAdmin/MapAdminUserForLocation',
                            data: JSON.stringify({ LocationID: selectlocation, LocationAddress: LocationAddress, LocationName: locationname, AdminUserId: parseInt($('#AddUserType').val(), 10), RolesIds: RolesIds }),
                            type: 'POST',
                            contentType: "application/json",
                            success: function (adminmapped) {
                                if (adminmapped) {
                                    gridReload();
                                    $('.modal').modal('hide');
                                    toastr.success('Location and rights shared successfully.')
                                    hideUserRoleDiv();
                                }
                            },
                            complete: function () {
                                fn_hideMaskloader();
                            },
                            error: function (mapadminerror) {
                                console.log(mapadminerror);
                                gridReload();
                                $('.modal').modal('hide');
                                hideUserRoleDiv();
                            }
                        });
                    }
                    $('.modal-content').removeClass('modalfade2');
                });
            }
            else {
                bootbox.alert('Please select atleast one role.')
            }
        }
    }
    else if (parseInt(forAdminOrForManager) == 2) //2 stand for manager user 
    {
        locationname = $('#poplocationname').html().trim();
        var adminname = $('#AddUserType option:selected').text();
        var selectlocation = $('#hiddnSelectLocation').val();
        if (adminname.trim() == "Select") {
            $("#AddUserType").css("border-color", "red");
            bootbox.alert("Please select the manager.")
            return false;
        }
        else {
            //$('.modal-content').addClass('modalfade2');
            $("#AddUserType").css("border", "0px solid white");
            if (check != '0') {
                if (RolesIds === "")



                { msg = "Are you sure you want to share <strong>" + locationname + "</strong>  management rights with <strong>" + adminname + "</strong> ?"; }
                else
                {
                    msg = "Are you sure you want to share <strong>" + locationname + "</strong> management rights with <strong>" + adminname + "</strong> ?";
                }


                bootbox.confirm(msg, function (result) {
                    //bootbox.confirm("Are you sure to share all rights for location:<strong>" + locationname + "</strong> with Manager: <strong>" + adminname + "</strong> ?", function (result) {
                    if (result) {
                        new fn_showMaskloader('Please wait...');
                        $.ajax({
                            url: '../GlobalAdmin/MapManagerUserForLocation',
                            data: JSON.stringify({ LocationID: selectlocation, LocationAddress: LocationAddress, LocationName: locationname, ManagerUserId: parseInt($('#AddUserType option:selected').val(), 10), RolesIds: RolesIds }),
                            type: 'POST',
                            contentType: "application/json",
                            success: function (adminmapped) {




                                if (adminmapped) {
                                    gridReload();
                                    $('.modal').modal('hide');
                                    hideUserRoleDiv();
                                }
                            },
                            complete: function () {
                                fn_hideMaskloader();
                            },
                            error: function (mapadminerror) {
                                console.log("Error aa raha h kuch kar.");
                                gridReload();
                                $('.modal').modal('hide');
                                hideUserRoleDiv();
                            }
                        });
                    }
                    $('.modal-content').removeClass('modalfade2');
                });
            }
            else {
                bootbox.alert('Please select atleast one role.')
            }
        }

    }
    else if (parseInt(forAdminOrForManager) == 3) {


        locationname = $('#poplocationname').html().trim();
        var adminname = $('#AddUserType option:selected').text();
        var selectlocation = $('#hiddnSelectLocation').val();
        if (adminname.trim() == "Select") {
            $("#AddUserType").css("border-color", "red");
            bootbox.alert("Please select the Employee.")
            return false;
        }
        else {

            //$('.modal-content').addClass('modalfade2');
            $("#AddUserType").css("border", "0px solid white");
            if (check != '0') {
                //alert(RolesIds);

                if (RolesIds === "") {
                    msg = 'Are you sure you want to share <strong>' + locationname + '</strong> user rights with <strong>"' + adminname + '"</strong> ?';

                }
                else {
                    msg = 'Are you sure you want to share <strong>"' + locationname + '"</strong> user rights with <strong>"' + adminname + '"</strong> ?';
                }

                bootbox.confirm(msg, function (result) {




                    if (result) {
                        new fn_showMaskloader('Please wait...');
                        $.ajax({
                            url: '../GlobalAdmin/MapEmployeeUserForLocation',
                            data: JSON.stringify({ LocationID: selectlocation, LocationAddress: LocationAddress, LocationName: locationname, EmployeeUserId: parseInt($('#AddUserType option:selected').val(), 10), RolesIds: RolesIds }),
                            type: 'POST',
                            contentType: "application/json",
                            success: function (adminmapped) {
                                if (adminmapped) {
                                    gridReload();
                                    $('.modal').modal('hide');
                                    hideUserRoleDiv();
                                }
                            },
                            complete: function () {
                                fn_hideMaskloader();
                            },
                            error: function (mapadminerror) {
                                console.log(mapadminerror);
                                gridReload();
                                $('.modal').modal('hide');
                                hideUserRoleDiv();
                            }
                        });
                    }
                    $('.modal-content').removeClass('modalfade2');
                });
            }
            else {
                bootbox.alert('Please select atleast one role.')
            }
        }


    }
}


function fnRemoveAdminPersmission(adminname, UserId) {
    if (UserId != undefined && UserId != '' && parseInt(UserId, 10) > 0) {
        locationname = $('#poplocationname').html().trim();
        var selectlocation = $('#hiddnSelectLocation').val();

 
        //$('.modal-content').addClass('modalfade2');
        bootbox.confirm("Are you sure you want to remove  <strong>" + adminname + "</strong> administrative rights from  <strong>" + locationname + "</strong>?", function (result) {
            if (result) {
                new fn_showMaskloader('Please wait...');              //alert("call share location");
                $.ajax({
                    url: '../GlobalAdmin/MapAdminUserForLocation',
                    data: JSON.stringify({ LocationID: selectlocation, AdminUserId: UserId, LocationAddress: LocationAddress, LocationName: locationname, IsDelete: true }),
                    type: 'POST',
                    contentType: "application/json",
                    success: function (unshareadminun) {

                        if (parseInt(unshareadminun.record) == 20001) {



                            bootbox.alert("Last associated administrator <strong>" + adminname + "</strong> for location <strong>" + locationname + "</strong> can not be deleted.");

                        }
                        else {
                            if (unshareadminun) {
                                gridReload();
                                $('.modal').modal('hide');
                            }
                        }
                    }

                    ,
                    complete: function () {
                        fn_hideMaskloader();
                    },
                    //success: function (unshareadminun) {

                    //    if (parseInt(unshareadminun.record) == 20001) {

                    //        //bootbox.alert("You can't delete " + employeeName + ".");

                    //        bootbox.alert("Last associated Admin <strong>'" + employeeName + "'</strong> for location <strong>'" + locationname + "'</strong> can not be deleted.");

                    //    }
                    //    else {
                    //        if (unshareadminun) {
                    //            gridReload();
                    //            $('.modal').modal('hide');
                    //        }
                    //    }
                    //},
                    error: function (unshareadminunerror) {
                        console.log(unshareadminunerror);
                        gridReload();
                        $('.modal').modal('hide');
                    }
                });
            }
            $('.modal-content').removeClass('modalfade2');
        });

    }
}

function fnRemoveManagerPersmission(employeeName, UserId) {
    //alert('Why you deleting ' + employeeName + 'EmpId:- ' + UserId + ':(');
   
    if (UserId != undefined && UserId != '' && parseInt(UserId, 10) > 0) {
        locationname = $('#poplocationname').html().trim();
        var selectlocation = $('#hiddnSelectLocation').val();
        //$('.modal-content').addClass('modalfade2');
        bootbox.confirm("Are you sure you want to remove <strong>" + employeeName + "</strong> management rights from  <strong>" + locationname + "</strong>?", function (result) {
            if (result) {
                new fn_showMaskloader('Please wait...');
                $.ajax({
                    url: '../GlobalAdmin/MapManagerUserForLocation',
                    data: JSON.stringify({ LocationID: selectlocation, ManagerUserId: UserId, LocationAddress: LocationAddress, LocationName: locationname, IsDelete: true }),
                    type: 'POST',
                    contentType: "application/json",
                    success: function (unshareadminun) {

                        if (parseInt(unshareadminun.record) == 20001) {

                            //bootbox.alert("You can't delete " + employeeName + ".");

                            bootbox.alert("Last associated manager <strong>" + employeeName + "</strong> for location <strong>" + locationname + "</strong> can not be deleted.");

                        }
                        else {
                            if (unshareadminun) {
                                gridReload();
                                $('.modal').modal('hide');
                            }
                        }
                    }
                    ,
                    complete: function () {
                        fn_hideMaskloader();
                    },
                    error: function (unshareadminunerror) {
                        console.log(unshareadminunerror);
                        gridReload();
                        $('.modal').modal('hide');
                    }
                });
            }
            $('.modal-content').removeClass('modalfade2');
        });
    }
}

function fnRemoveEmployeePersmission(employeeName, EmmployeeId) {
    //alert('Why you deleting ' + employeeName + 'EmpId:- ' + EmmployeeId + ':(');
 

    if (EmmployeeId != undefined && EmmployeeId != '' && parseInt(EmmployeeId, 10) > 0) {
        locationname = $('#poplocationname').html().trim();
        var selectlocation = $('#hiddnSelectLocation').val();
        //$('.modal-content').addClass('modalfade2');
        bootbox.confirm("Are you sure you want to remove  <strong>" + employeeName + "</strong> user rights from  " + locationname + "?", function (result) {
            if (result) {
                new fn_showMaskloader('Please wait...');
                $.ajax({
                    url: '../GlobalAdmin/MapEmployeeUserForLocation',
                    data: JSON.stringify({ LocationID: selectlocation, EmployeeUserId: EmmployeeId, LocationAddress: LocationAddress, LocationName: locationname, IsDelete: true }),
                    type: 'POST',
                    contentType: "application/json",
                    success: function (unshareadminun) {
                        if (unshareadminun) {
                            gridReload();
                            $('.modal').modal('hide');
                        }
                    }
                    ,
                    complete: function () {
                        fn_hideMaskloader();
                    },
                    error: function (unshareadminunerror) {
                        console.log(unshareadminunerror);
                        gridReload();
                        $('.modal').modal('hide');
                    }
                });
            }
            $('.modal-content').removeClass('modalfade2');
        });

    }
}
function loadpreview(result) {
     
    var LocationID = $(result).attr("vid");
    $.ajax({
        url: '../GlobalAdmin/GetLocationDetailByLocationID',
        data: { 'LocationID': LocationID.toString() },
        type: 'POST',
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (Data) {
            if (Data.res.length > 0) {
                $("#View_LocationName").html(Data.res[0].LocationName)
                $("#View_Description").html(Data.res[0].Description);
                $("#View_City").html(Data.res[0].City);
                $("#View_Address1").html(Data.res[0].Address1);
                $("#View_Address2").html(Data.res[0].Address2);
                $("#View_LocationCountry").html(Data.res[0].LocationCountry);
                $("#View_LocationState").html(Data.res[0].LocationState);
                $("#View_PhoneNo").html(Data.res[0].PhoneNo);
                $("#View_ZipCode").html(Data.res[0].ZipCode);
                $("#View_ClientName").html(Data.res[0].ClientName);
                $("#View_ClientEmail").html(Data.res[0].ClientEmail);
                $("#View_ClientDOB").html(Data.res[0].ClientDOB);
                $("#View_ClientCountry").html(Data.res[0].ClientCountry);
                $("#View_ClientState").html(Data.res[0].ClientState);

                if (Data.res[0].isEmailVerified == false) {
                    $("#isEmailVerified").html("No");
                }
                else {
                    $("#isEmailVerified").html("Yes");
                }


                if (Data.res[0].ClientImage == "") {
                    $("#View_ClientImage").attr("src", "../Content/Images/ProjectLogo/no-profile-pic.jpg");
                } else {
                    $("#View_ClientImage").attr("src", Data.res[0].ClientImage)
                }
                var locationService = "";
                if (Data.res2.length > 0) {

                    for (i = 0; i < Data.res2.length; i++) {
                        locationService = locationService + "<div><span>" + parseInt(parseInt(i) + 1) + ").&nbsp;</span><span>" + Data.res2[i] + "<span/></div>";
                    }
                }
                $("#View_LocationServices").html(locationService);
                $("#ModalLocationViewPreview").modal('show');
            }
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
function PrintLocationDetail(DivId) {

    _isPrintDone = false;
    if (!_isPrintDone) {
        //var divToPrint = document.getElementById('DivQRCIndex');
        var LocationName = '';
        var Address = '';
        var City = '';
        var Description = '';
        var LocationCode = '';
        //var divToQRC = document.getElementById("container2");
        var popupWin = window.open('', '_blank', 'width=800,height=500');
        popupWin.document.open();
        popupWin.document.write('<link rel="styleshee" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">');
        //if ($("#View_LocationName").html() != null && $("#View_LocationName").html() != "" ) {
        //    LocationName = "<p></p><strong style='font-size: 16px;'>Location Name</strong> <br/>"
        //              + $("#View_LocationName").html();
        //}
        //if ($("#View_Address1").html() != null && $("#View_Address1").html() != "" ){
        //    Address = "<p></p><strong style='font-size: 16px;'>Address </strong> <br/>"
        //              + $("#View_Address1").html();
        //}
        //if ($("#View_City").html() != null && $("#View_City").html() != "") {
        //    City = "<p></p><strong style='font-size: 16px;'>City </strong> <br/>"
        //              + $("#View_City").html();
        //}
        //if ($("#View_Description").html() != null && $("#View_Description").html() != "") {
        //    Description = "<p></p><strong style='font-size: 16px;'>Description</strong> <br/>"
        //              + $("#View_Description").html();
        //}
        //if ($("#View_Address2").html() != null && $("#View_Address2").html() != "") {
        //    LocationCode = "<p></p><strong style='font-size: 16px;'>Location Code </strong><br/>"
        //              + $("#View_Address2").html();
        //}
        //popupWin.document.write("<html><body onload='window.print(); window.close();'><div style='width:800px;height:300px;'>" + divToPrint.innerHTML + "</div></body></html>");
        popupWin.document.write("<html><body onload='window.print();'><div class='panel panel-primary'><div class='panel-heading' style='margin-left: 405px;font-weight: 700;'>Location Details</div><div style='margin-left: 96px; margin-right: 100px; width: 520px;' ><table id='abc' style='width: 167%; border: 1px solid #0aa0cd; padding: 10px;'><tr><td valign='top' style='width: 210px;'><div class='row'><div><strong  style='font-size: 16px;'>Location Name</strong>"
            + $("#View_LocationName").html() + "</div>" + "<div style='margin-top: -16px;margin-left: 543px;'><strong style='font-size: 16px;'>Description </strong>"
            + $("#View_Description").html() + "</div></div>" + "<div class='row'><div style='margin-top: 28px;'><strong style='font-size: 16px;'>Address </strong>"
            + $("#View_Address1").html() + "</div>" + "<div style='margin-top: -19px;margin-left: 543px;'><strong style='font-size: 16px;'>Location Code </strong> "
            + $("#View_Address2").html() + "</div></div>" + "<div class='row'><div style='margin-top: 28px;'><strong style='font-size: 16px;'>City </strong> "
            + $("#View_City").html() + "</div>" + "<div style='margin-top: -19px;margin-left: 543px;'><strong style='font-size: 16px;'>Location Country </strong> "
            + $("#View_LocationCountry").html() + "</div></div>" + "<div class='row'><div style='margin-top: 28px;'><strong style='font-size: 16px;'>Location State </strong> "
            + $("#View_LocationState").html() + "</div>" + "<div style='margin-top: -19px;margin-left: 543px;'><strong style='font-size: 16px;'>Phone No </strong> <br/>"
            + $("#View_PhoneNo").html() + "</div></div>" + "<div class='row'><div style='margin-top: 28px;'><strong style='font-size: 16px;'>Zip Code </strong> "
            + $("#View_ZipCode").html() + "</div>" + "<div style='margin-top: -19px;margin-left: 543px;'><strong style='font-size: 16px;'>Client Name </strong> "
            + $("#View_ClientName").html() + "</div></div>" + "<div class='row'><div style='margin-top: 28px;'><strong style='font-size: 16px;'>Client Email </strong>  "
            + $("#View_ClientEmail").html() + "<div>" + "<div style='margin-top: -19px;margin-left: 543px;'><strong style='font-size: 16px;'>Is Client Email Verified </strong>  "
            + $("#isEmailVerified").html() + "</div></div>" + "<div class='row'><div style='margin-top: 28px;'><strong style='font-size: 16px;'>Client Country </strong> "
            + $("#View_ClientCountry").html() + "</div>" + "<div style='margin-top: -19px;margin-left: 543px;'><strong style='font-size: 16px;'>Client State </strong> "
            + $("#View_ClientState").html() + "</div></div>" + "<div class='row'><div style='margin-top: 28px;'><strong style='font-size: 16px;'>Client Image </strong> "
              + '<img id="View_ClientImage" src=' + $('#View_ClientImage').attr('src') + ' class="img-rounded" style="width:110px; height:110px;">'
            + "</div>" + "<div style='margin-top:-125px;margin-left: 543px;'><strong style='font-size: 16px;'>Location Services </strong>"
            + $("#View_LocationServices").html() + "</div></div>"
            //+ "<p></p><strong style='font-size: 16px;'>Zip Code</strong><br/>"
            //+ $("#View_ClientName").html() + Description + LocationCode
            + "<p><strong style='font-size: 16px;'></strong></p><p><br/></p></td></tr></td></tr></table></div></div></body></html>");

        if (popupWin.closed == false) {
            popupWin.document.close();
        }
        _isPrintDone = true;
    }
    //$('.noprint').show();
}

//function PrintLocationDetail(DivId) {
//    var mywindow = window.open('Location Detail', 'my div', 'height=1000,width=1000');
//    var PrintData = $("#abc").html();
//    mywindow.document.open('', '_blank', 'width=800,height=500');
//    mywindow.document.write('<html><head><title></title>');
    
//    mywindow.document.write('<link rel="stylesheet" href="../Content/bootstrap.min.css" rel="stylesheet" />');
//    //mywindow.document.write('<link rel="stylesheet" href="../Content/common/admin.css" rel="stylesheet" /> ');
//    mywindow.document.write('<link rel="stylesheet" href="http://www.test.com/style.css" type="text/css" />');
//    mywindow.document.write('<style type="text/css">Location Detail { color:red; } </style></head><body>');
//    mywindow.document.write(PrintData);
//    mywindow.document.write('</body></html>');
//    mywindow.document.onreadystatechange = function () {
//        if (this.readyState === 'complete') {
//            this.onreadystatechange = function () { };
//            mywindow.focus();
//            mywindow.print();
//            mywindow.close();
//        }
//    }
//    //mywindow.document.close(); // necessary for IE >= 10
//    //var mywindow = window.open('Location Detail', 'my div', 'height=1000,width=1000');
//    //var PrintData = $("#divLocationPreview").html();
//    //mywindow.document.open();

//    //mywindow.document.write('<html><head><title></title>');
//    //mywindow.document.write('<link rel="stylesheet" href="../Content/bootstrap.min.css" rel="stylesheet" />');
//    //mywindow.document.write('<link rel="stylesheet" href="../Content/common/admin.css" rel="stylesheet" /> ');
//    //mywindow.document.write('<link rel="stylesheet" href="http://www.test.com/style.css" type="text/css" />');
//    //mywindow.document.write('<style type="text/css">Location Detail { color:red; } </style></head><body>');
//    //mywindow.document.write(PrintData);
//    //mywindow.document.write('</body></html>');
//    //mywindow.document.onreadystatechange = function () {
//    //    if (this.readyState === 'complete') {
//    //        this.onreadystatechange = function () { };
//    //        mywindow.focus();
//    //        mywindow.print();
//    //        mywindow.close();
//    //    }
//    //}
//    //mywindow.document.close(); // necessary for IE >= 10
//}

function getUserRoles() {
     
    var Roleid = "";
    $('#serviceToLocation input:checkbox').each(function () {

        if (this.checked) {
            var ChkBoxId = this.id;
            if (Roleid == "") {
                Roleid += ChkBoxId
            }
            else {
                Roleid += "," + ChkBoxId
            }
        }

    });
    return Roleid;
}

function hideUserRoleDiv() {
    console.log('vijay');
    $("#serviceToLocation").empty();
    $("#ModalConfirumationPreview").children('div').animate({ left: '0px' }, 'fast');
    $("name=['userRoleBind']").css('display', 'block');
    $("#userRoleWaliDiv").hide();
    ttcheck = 0;



}