$(document).ready(function () {
    //Note : First data rendered through getAllDashboardData() which is in _NotificationAlert.cshtml.

    $('#daterangepicker0 .applyBtn').click(function () {
        RenderDashboardData(false);
    });

    $('.breadcrumb #RefreshDashboard').click(function () {
        RenderDashboardData(true);
        var date = new Date();
        var todaysDate = (date.toLocaleString("en-us", { month: "long" }) + ' ' + date.getDate() + ', ' + date.getFullYear());
        $('#reservation1').val(todaysDate + ' - ' + todaysDate);
    });

    function RenderDashboardData(IsRefresh) {
        var data;
        if (IsRefresh) {
            data = {};

        }
        else {
            var fromDate = $('#daterangepicker0 .daterangepicker_start_input .input-mini').val();
            var toDate = $('#daterangepicker0 .daterangepicker_end_input .input-mini').val();
            data = { "fromDate": fromDate, "toDate": toDate };
             
            clearInterval(refreshIntervalId);
            //refreshIntervalId = setInterval(function () {
            //    getAllDashboardData();
            //}, 60000);//60 Second
        }
        $.ajax({
            //   url: '@Url.Action("GetDashboardHeadCount", "GlobalAdmin")',
            url: $_HostPrefix + 'GlobalAdmin/GetDashboardHeadCount',
            data: data,
            type: 'GET',
            contentType: "application/json",
            beforeSend: function () {
                new fn_showMaskloader('Loading...');
            },
            success: function (elementJSON) {

                if (elementJSON != null || elementJSON != "") {
                    // Disabled as clock refreshed by client side timer above instead of server timer, but left for example
                    dataJSON = JSON.parse(elementJSON.dataJson);
                    //*******************Count panel*******************************//
                    $('#divCountPanel #QrcCount').text(dataJSON.CountPanel[0]['QrcCount']);
                    $('#divCountPanel #WorkRequestCount').text(dataJSON.CountPanel[0]['WorkRequestCount']);
                    $('#divCountPanel #UserCount').text(dataJSON.CountPanel[0]['UserCount']);
                    $('#divCountPanel #VendorsVehicleCount').text(dataJSON.CountPanel[0]['VendorsVehicleCount']);
                    $('#divPieUserGraph #pieEmpCount').data('percent', (dataJSON.CountPanel[0]['EmpCount']));

                    $('#divPieUserGraph #EmpCount').html(dataJSON.CountPanel[0]['EmpCount']);
                    $('#divPieUserGraph #AdminCount').html(dataJSON.CountPanel[0]['AdminCount']);
                    $('#divPieUserGraph #ManagerCount').html(dataJSON.CountPanel[0]['ManagerCount']);
                    $('#divPieUserGraph #ClientCount').html(dataJSON.CountPanel[0]['ClientCount']);
                    $('#diveCashAndInfraction #eCash').text('$' + dataJSON.CountPanel[0]['eCash']);
                    $('#diveCashAndInfraction #eCashLastAmt').text('$' + dataJSON.CountPanel[0]['eCashLastAmt']);
                    $('#diveCashAndInfraction #PaidBy').text(dataJSON.CountPanel[0]['PaidBy']);
                    $('#diveCashAndInfraction #AcceptedBy').text(dataJSON.CountPanel[0]['AcceptedBy']);
                    $('#diveCashAndInfraction #InfractionCount').text(dataJSON.CountPanel[0]['InfractionCount']);
                    $('#diveCashAndInfraction #Declined').text(dataJSON.CountPanel[0]['Declined']);
                    $('#diveCashAndInfraction #Approved').text(dataJSON.CountPanel[0]['Approved']);
                    $('#divScannedByPanel #PhoneSystems').text(dataJSON.CountPanel[0]['PhoneSystems']);
                    $('#divScannedByPanel #Devices').text(dataJSON.CountPanel[0]['Devices']);
                    $('#divScannedByPanel #TrashCan').text(dataJSON.CountPanel[0]['TrashCan']);
                    $('#divScannedByPanel #Elevator').text(dataJSON.CountPanel[0]['Elevator']);
                    $('#divScannedByPanel #Equipment').text(dataJSON.CountPanel[0]['Equipment']);
                    $('#divScannedByPanel #TicketSpitter').text(dataJSON.CountPanel[0]['TicketSpitter']);
                    $('#divScannedByPanel #Vehicle').text(dataJSON.CountPanel[0]['Vehicle']);
                    $('#divScannedByPanel #BusStation').text(dataJSON.CountPanel[0]['BusStation']);
                    $('#divScannedByPanel #MovingWalkway').text(dataJSON.CountPanel[0]['MovingWalkway']);
                    $('#divScannedByPanel #Escalators').text(dataJSON.CountPanel[0]['Escalators']);
                    $('#divScannedByPanel #Bathroom').text(dataJSON.CountPanel[0]['Bathroom']);
                    $('#divScannedByPanel #ParkingFacility').text(dataJSON.CountPanel[0]['ParkingFacility']);
                    $('#divScannedByPanel #ShuttleBus').text(dataJSON.CountPanel[0]['ShuttleBus']);
                    $('#divScannedByPanel #GateArm').text(dataJSON.CountPanel[0]['GateArm']);


                    //******************* Work Order Pie Chart ***********************************//

                    if ($("#divWOProgress #dtWOProgress").length > 0) {

                        $('#dtWOProgress').dataTable().fnDestroy();
                        $('#dtWOProgress').remove();
                        //*******************WO Progress data table*******************************//
                        var WOProgressBody = '';
                        WOProgressBody = "<table id='dtWOProgress' class='table table-bordered table-striped'><thead><tr><th style='display:none'>Code</th><th style='width:14%'>Code No. </th><th style='width:10%;'>Location<th style='width:10%;'>Project Type</th><th style='width:12%;'>Priority</th><th style='width:20%;'>Assign To</th><th style='width:14%;'>Created On</th><th style='width:17%;'>Progress</th></tr></thead><tbody id='dtWOProgressbody'>";
                        if (dataJSON.Progress.length > 0) {

                            for (i = 0; (i < dataJSON.Progress.length) ; i++) {
                                var ProgressTD = '';
                                var progress = dataJSON.Progress[i]['ProgressBar'];
                                if (progress != 296 && progress != 295) {
                                    var progbar = progress
                                } if (dataJSON.Progress[i]['PauseStatus'] == 330 || dataJSON.Progress[i]['PauseStatus'] == null) {
                                    switch (parseInt(progress)) {
                                        case (296):
                                            ProgressTD = '<td><div class="text-red">Over Limit</div></td>';
                                            break;
                                        case (295):
                                            ProgressTD = '<td><div class="text-green">No Limit</div></td>';
                                            break;
                                        case (progbar):
                                            {
                                                if (progress > 75 && progress < 90) {
                                                    ProgressTD = '<td class="IsProgress"><div data-perc="' + dataJSON.Progress[i]['ProgressBar'] + '" class="progressbar"><div class="bar color2" style="width: ' + dataJSON.Progress[i]['ProgressBar'] + '%; overflow: hidden;"><span></span></div><div class="label" style="left: 69px;"><span></span><div class="perc">' + dataJSON.Progress[i]['ProgressBar'] + '%</div></div></div><div class="timeTill">' + dataJSON.Progress[i]['TimeTill'] + '</div> <div class="starttime" style="display:none">' + dataJSON.Progress[i]['StartTime'] + '</div><div class="endtime" style="display:none">' + dataJSON.Progress[i]['EndTimeTimer'] + '</div></td>';
                                                }
                                                else if (progress > 90 && progress <= 99) {
                                                    ProgressTD = '<td class="IsProgress"><div data-perc="' + dataJSON.Progress[i]['ProgressBar'] + '" class="progressbar"><div class="bar color3" style="width: ' + dataJSON.Progress[i]['ProgressBar'] + '%; overflow: hidden;"><span></span></div><div class="label" style="left: 69px;"><span></span><div class="perc">' + dataJSON.Progress[i]['ProgressBar'] + '%</div></div></div><div class="timeTill">' + dataJSON.Progress[i]['TimeTill'] + '</div><div class="starttime" style="display:none">' + dataJSON.Progress[i]['StartTime'] + '</div><div class="endtime" style="display:none">' + dataJSON.Progress[i]['EndTimeTimer'] + '</div></td>';
                                                }
                                                else if (true) {
                                                    ProgressTD = '<td class="IsProgress"><div data-perc="' + dataJSON.Progress[i]['ProgressBar'] + '" class="progressbar"><div class="bar" style="width: ' + dataJSON.Progress[i]['ProgressBar'] + '%; overflow: hidden;"><span></span></div><div class="label" style="left: 69px;"><span></span><div class="perc">' + dataJSON.Progress[i]['ProgressBar'] + '%</div></div></div><div class="timeTill">' + dataJSON.Progress[i]['TimeTill'] + '</div><div class="starttime" style="display:none">' + dataJSON.Progress[i]['StartTime'] + '</div><div class="endtime" style="display:none">' + dataJSON.Progress[i]['EndTimeTimer'] + '</div></td>';
                                                }
                                            }
                                            break;
                                        default:
                                            ProgressTD = '<td><div class="text-yellow">Not-Assigned</div></td>';
                                            break
                                    }
                                }
                                else {
                                    ProgressTD = '<td><div class="text-green">Pause</div></td>';
                                }
                                //var now = getStatusTime();
                                WOProgressBody += '<tr class="odd" role="row"> <td style="display:none">' + dataJSON.Progress[i]['WorkRequestAssignmentID'] + '</td><td class="sorting_1">' + dataJSON.Progress[i]['CodeID'] + '</td> <td>' + dataJSON.Progress[i]['LocationName'] + '</td><td>' + dataJSON.Progress[i]['WorkRequestProjectTypeName'] + '</td><td><span class="' + dataJSON.Progress[i]['PriorityColor'] + '">' + dataJSON.Progress[i]['PriorityLevelName'] + '</span></td><td>' + dataJSON.Progress[i]['AssignToUserName'] + '</td><td>' + dataJSON.Progress[i]['CreatedDate'] + '</td>' + ProgressTD + '</tr>';
                            }
                            WOProgressBody += "</tbody></table>";
                        }
                        $('#divWOProgress').html(WOProgressBody);
                        $("#dtWOProgress").DataTable({
                            "order": [[0, "desc"]]
                        });

                        updateProgressTime();
                        //*******************WO Progress data table End*******************************//

                    }
                    if ($("#divWOPending #dtWOPending").length > 0) {
                        $('#dtWOPending').dataTable().fnDestroy();
                        $('#dtWOPending').remove();
                        //*******************WO Pending data table*******************************//
                        var WOPendingBody = '';
                        WOPendingBody = "<table id='dtWOPending' class='table table-bordered table-striped'><thead><tr><th style='display:none'>Code</th><th style='width:14%'>Code No. </th><th style='width:17%;'>Location</th><th style='width:18%;'>Project Type</th><th style='width:15%;'>Priority</th><th style='width:20%;'>Assign To</th><th style='width:16%;'>Created On</th></tr></thead><tbody id='dtWOPendingbody'>";
                        if (dataJSON.Pending.length > 0) {
                            for (i = 0; (i < dataJSON.Pending.length) ; i++) {
                                WOPendingBody += '<tr class="odd" role="row"> <td style="display:none">' + dataJSON.Pending[i]['WorkRequestAssignmentID'] + '</td><td class="sorting_1">' + dataJSON.Pending[i]['CodeID'] + '</td> <td>' + dataJSON.Pending[i]['LocationName'] + '</td><td>' + dataJSON.Pending[i]['WorkRequestProjectTypeName'] + '</td><td><span class="' + dataJSON.Pending[i]['PriorityColor'] + '">' + dataJSON.Pending[i]['PriorityLevelName'] + '</span></td><td>' + dataJSON.Pending[i]['AssignToUserName'] + '</td><td>' + dataJSON.Pending[i]['CreatedDate'] + '</td></tr>';
                            }
                            WOPendingBody += "</tbody></table>";
                        }
                        $('#divWOPending').html(WOPendingBody);
                        $("#dtWOPending").DataTable({
                            "order": [[0, "desc"]]
                        });
                        //*******************WO Pending data table End*******************************//
                    }
                    if ($("#divWOComplete #dtWOComplete").length > 0) {
                        $('#dtWOComplete').dataTable().fnDestroy();
                        $('#dtWOComplete').remove();
                        //*******************WO Completed data table*******************************//
                        var WOCompleteBody = '';
                        WOCompleteBody = "<table id='dtWOComplete' class='table table-bordered table-striped'><thead><tr><th style='display:none'>Code</th><th style='width:14%'>Code No. </th><th style='width:17%;'>Location</th><th style='width:15%;'>Priority</th><th style='width:20%;'>Completed By</th><th style='width:16%;'>Created On</th><th style='width:18%;'>Completed On</th></tr></thead><tbody id='dtWOCompletebody'>";
                        if (dataJSON.Completed.length > 0) {
                            for (i = 0; (i < dataJSON.Completed.length) ; i++) {
                                WOCompleteBody += '<tr class="odd" role="row"> <td style="display:none">' + dataJSON.Completed[i]['WorkRequestAssignmentID'] + '</td><td class="sorting_1">' + dataJSON.Completed[i]['CodeID'] + '</td> <td>' + dataJSON.Completed[i]['LocationName'] + '</td><td><span class="' + dataJSON.Completed[i]['PriorityColor'] + '">' + dataJSON.Completed[i]['PriorityLevelName'] + '</span></td><td>' + dataJSON.Completed[i]['AssignToUserName'] + '</td><td>' + dataJSON.Completed[i]['CreatedDate'] + '</td><td>' + dataJSON.Completed[i]['EndTimeFull'] + '</td></tr>';
                            }
                            WOCompleteBody += "</tbody></table>";
                        }
                        $('#divWOComplete').html(WOCompleteBody);
                        $("#dtWOComplete").DataTable({
                            "order": [[0, "desc"]]
                        });
                        //*******************WO Completed data table End*******************************//
                    }
                    var arrData = [];
                    if (dataJSON.WorkStatus.length > 0) {
                        for (i = 0; i < dataJSON.WorkStatus.length; i++) {
                            arrData.push({
                                title: dataJSON.WorkStatus[i].Title,
                                value: parseInt(dataJSON.WorkStatus[i].Value)
                            });
                        }
                    }
                    if (dataJSON.WorkProjectType.length > 0) {
                        for (i = 0; i < dataJSON.WorkProjectType.length; i++) {
                            arrData.push({
                                title: dataJSON.WorkProjectType[i].Title,
                                value: parseInt(dataJSON.WorkProjectType[i].Value)
                            });
                        }
                    }
                    var chart = AmCharts.makeChart("workOrderChart", {
                        "type": "pie",
                        "theme": "light",
                        "radius": "15%",
                        "innerRadius": "38%",
                        "gradientRatio": [-0.4, -0.4, -0.4, -0.4, -0.4, -0.4, 0, 0.1, 0.2, 0.1, 0, -0.2, -0.5],
                        "dataProvider": arrData,
                        "labelText": "[[title]]",
                        "balloonText": "[[value]]",
                        "valueField": "value",
                        "titleField": "title",
                        "balloon": {
                            "drop": true,
                            "adjustBorderColor": false,
                            "color": "#FFFFFF",
                            "fontSize": 16
                        },
                        "export": {
                            "enabled": true
                        }
                    });
                    //******************* Work Order Pie Chart End *******************************//

                    //*******************DAR Marquee*******************************//
                    var DARBody = '';
                    $('#divDarDetails #darDetail').append('');
                    if (dataJSON.DarDetail.length > 0) {
                        for (i = 0; (i < dataJSON.DarDetail.length) ; i++) {
                            DARBody += '<tr><td><div class="zoomin"><img src=' + $_HostPrefix + 'Content/Images/ProfilePic/' + dataJSON.DarDetail[i]['ProfileImage'] + '></div> <div class="dailyActRp"><strong>' + dataJSON.DarDetail[i]['TaskType'] + '</strong><br>' + dataJSON.DarDetail[i]['ActivityDetails'] + '</div></td></tr>';
                        }
                    }
                    $('#divDarDetails #darDetail').html(DARBody);
                    //*******************DAR Marquee End*******************************//

                    //*******************UnAssigned WO Section*******************************//
                    var UnAssignedWOBody = '';
                    $('#divUnAssignedWODetails #UnAssignedDetails').append('');
                    if (dataJSON.UnAssignedWO.length > 0) {
                        for (i = 0; (i < dataJSON.UnAssignedWO.length) ; i++) {
                            UnAssignedWOBody += '<tr id=' + dataJSON.UnAssignedWO[i]['ID'] + '><td> <strong>' + dataJSON.UnAssignedWO[i]['CodeID'] + '</strong><br>' + dataJSON.UnAssignedWO[i]['PriorityLevelName'] + '<br>' + dataJSON.UnAssignedWO[i]['ProblemDesc'] + '</td><td style="display:none;">' + dataJSON.UnAssignedWO[i]['WorkRequestProjectTypeName'] + '</td><td style="display:none;">' + dataJSON.UnAssignedWO[i]['WorkRequestTypeName'] + '</td><td style="display:none;">' + dataJSON.UnAssignedWO[i]['PriorityLevelName'] + '</td><td style="display:none;"><img style="height: 150px !important;width: 150px !important;" src=' + $_HostPrefix + 'Content/Images/WorkRequest/' + dataJSON.UnAssignedWO[i]['AssignedWorkOrderImage'] + '></td><td style="display:none;">' + dataJSON.UnAssignedWO[i]['WorkRequestStatusName'] + '</td><td style="display:none;">' + dataJSON.UnAssignedWO[i]['CreatedDate'] + '</td><td style="display:none;">' + dataJSON.UnAssignedWO[i]['ProblemDesc'] + '</td><td style="display:none;">' + dataJSON.UnAssignedWO[i]['CodeID'] + '</td><td style="display:none;">' + dataJSON.UnAssignedWO[i]['LocationName'] + '</td><td style="display:none;">' + dataJSON.UnAssignedWO[i]['LocationID'] + '</td><td style="display:none;">' + dataJSON.UnAssignedWO[i]['PriorityLevel'] + '</td><td class="vertibalM" align="center" style="width: 100px;"><a class="editBtn assign" id=' + dataJSON.UnAssignedWO[i]['ID'] + '><i class="fa fa-pencil"></i></a> <a class="editBtn view" id=' + dataJSON.UnAssignedWO[i]['ID'] + '><i class="fa fa-eye"></i></a></td></tr>';
                        }
                    }
                    $('#divUnAssignedWODetails #UnAssignedDetails').html(UnAssignedWOBody);
                    var dynamicScript = "<script type='text/javascript'>$('#divUnAssignedWODetails #UnAssignedDetails .view').click(function () { var row = $(this).closest('tr'); var id = $(this).attr('id'); var WorkRequestProjectTypeName = row.find('td').eq(1).html(); var WorkRequestTypeName = row.find('td').eq(2).html(); var PriorityLevelName = row.find('td').eq(3).html(); var AssignedWorkOrderImage = row.find('td').eq(4).html(); var WorkRequestStatusName = row.find('td').eq(5).html(); var Submittedon = row.find('td').eq(6).html(); var ProblemDesc = row.find('td').eq(7).html(); var CodeId = row.find('td').eq(8).html(); var LocationName = row.find('td').eq(9).html(); $('#lblProjectType').html(WorkRequestProjectTypeName); $('#lblWorkRequestType').html(WorkRequestTypeName); $('#lblAssignedWorkImage').html(AssignedWorkOrderImage); $('#lblPriorityLevel').html(PriorityLevelName); $('#lblWorkRequestStatus').html(WorkRequestStatusName); $('#lblSubmittedOn').html(Submittedon); if (ProblemDesc == null || ProblemDesc == '') { $('#labellProblemDescription').hide(); $('#lblProblemDescription').hide(); } else { $('#lblProblemDescription').html(ProblemDesc); $('#labellProblemDescription').show(); $('#lblProblemDescription').show(); } $('#lblCodeNo').html(CodeId); $('#lblLocationName').html(LocationName); $('#ModalDetailPreview').modal('show'); }); $('#divUnAssignedWODetails #UnAssignedDetails .assign').click(function () { var row = $(this).closest('tr'); var id = $(this).attr('id'); var ProblemDesc = row.find('td').eq(7).html(); var PriorityLevel = row.find('td').eq(11).html(); var ProjectDesc = row.find('td').eq(7).html(); var locationId = row.find('td').eq(10).html(); var WorkRequestType = row.find('td').eq(1).html(); if (PriorityLevel == null || PriorityLevel == '' || PriorityLevel == 'undefined') { PriorityLevel = 0; } $.ajax({ type: 'GET', data: { 'id': id, 'ProblemDesc': ProblemDesc, 'PriorityLevel': PriorityLevel, 'ProjectDesc': ProjectDesc, 'WorkRequestType': WorkRequestType, 'locationId': locationId }, url: '" + $_HostPrefix + "GlobalAdmin/_AssignWorkAssignmentRequest/', contentType: 'application/json; charset=utf-8', error: function (xhr, status, error) { }, success: function (result) { $('.modal-title').text('Assign Work Order'); $('#largeeditpopup').html(result); $('#myModallarge').modal('show'); } }); });<\/script>";
                    $('#divUnAssignedWODetails').append(dynamicScript);
                    //*******************UnAssigned WO Section End*******************************//

                    //******************* All Active User*******************************//
                    var AllActiveUser = '';
                    $('#divAllActiveUser #AllActiverUser').append('');
                    if (dataJSON.AllActiveUser.length > 0) {
                        for (i = 0; (i < dataJSON.AllActiveUser.length) ; i++) {
                            AllActiveUser += '<tr><td><img src=' + $_HostPrefix + 'Content/Images/ProfilePic/' + dataJSON.AllActiveUser[i]['ProfileImage'] + '><strong>' + dataJSON.AllActiveUser[i]['UserName'] + '</strong><br> <span class="text-green">' + dataJSON.AllActiveUser[i]['UserType'] + '</span></td></tr>';
                        }
                    }
                    $('#divAllActiveUser #AllActiverUser').html(AllActiveUser);
                    //*******************All Active User End*******************************//

                    //******************* WEB Active User*******************************//
                    var WebActiveUser = '';
                    $('#divWebActiveUser #WebActiverUser').append('');
                    if (dataJSON.WebActiveUser.length > 0) {
                        for (i = 0; (i < dataJSON.WebActiveUser.length) ; i++) {
                            WebActiveUser += '<tr><td><img src=' + $_HostPrefix + 'Content/Images/ProfilePic/' + dataJSON.WebActiveUser[i]['ProfileImage'] + '><strong>' + dataJSON.WebActiveUser[i]['UserName'] + '</strong><br> <span class="text-green">' + dataJSON.WebActiveUser[i]['UserType'] + '</span></td></tr>';
                        }
                    }
                    $('#divWebActiveUser #WebActiverUser').html(WebActiveUser);
                    //*******************WEB Active User End*******************************//

                    //******************* MOBILE Active User*******************************//
                    var MobActiveUser = '';
                    $('#divMobActiveUser #MobActiverUser').append('');
                    if (dataJSON.MobActiveUser.length > 0) {
                        for (i = 0; (i < dataJSON.MobActiveUser.length) ; i++) {
                            MobActiveUser += '<tr><td><img src=' + $_HostPrefix + 'Content/Images/ProfilePic/' + dataJSON.MobActiveUser[i]['ProfileImage'] + '><strong>' + dataJSON.MobActiveUser[i]['UserName'] + '</strong><br> <span class="text-green">' + dataJSON.MobActiveUser[i]['UserType'] + '</span></td></tr>';
                        }
                    }
                    $('#divMobActiveUser #MobActiverUser').html(MobActiveUser);
                    //*******************MOBILE Active User End*******************************//
                }

            },
            complete: function () {
                fn_hideMaskloader();

            },
            error: function (er) {
            }
        });//ajax end block
    }

    $("#divScannedByPanel ul li").click(function () {
        var fromDate = $('#daterangepicker0 .daterangepicker_start_input .input-mini').val();
        var toDate = $('#daterangepicker0 .daterangepicker_end_input .input-mini').val();
        var id = $(this).attr("id");
        if ($('#' + id + ' .number').html() > 0 || $('#' + id + ' .number').html() > '0') {
            try {
                $.ajax({
                    type: 'GET',
                    url: $_HostPrefix + '/GlobalAdmin/GetQRCScannedDetail',
                    data: { "qrcType": id, "fromDate": fromDate, "toDate": toDate },
                    async: true,
                    contentType: "application/json",
                    beforeSend: function () {
                        new fn_showMaskloader('Loading...');
                    },
                    complete: function () {
                        fn_hideMaskloader();
                    },
                }).success(function (dataItems) {
                    if (dataItems.dataJson != null || dataItems.dataJson != "") {
                        if (dataItems.dataJson.length > 0) {
                            if (!$("#ModalQRCScannedView  #divQRCScannedPreview").length > 0) {
                                $('#Scandetail').dataTable().fnDestroy();
                                $('#Scandetail').remove();

                                //*******************QRC Scanned Details data table*******************************//
                                var QRCScannedBody = '';
                                QRCScannedBody = "<table id='Scandetail' class='table table-bordered table-striped'><thead><tr><th>Serial No.</th><th>User Name</th><th>QR Code ID</th><th>QRC Name</th><th>Scan Date</th></tr></thead><tbody>";

                                for (i = 0; (i < dataItems.dataJson.length) ; i++) {
                                    QRCScannedBody += '<tr class="odd" role="row"><td>' + (i + 1) + '</td><td>' + dataItems.dataJson[i]['ScanUserName'] + '</td> <td>' + dataItems.dataJson[i]['QrCodeId'] + '</td><td>' + dataItems.dataJson[i]['QrcName'] + '</td><td>' + dataItems.dataJson[i]['StrCreatedDate'] + '</td></tr>';
                                }
                                QRCScannedBody += "</tbody></table>";
                                $("#Scandetail").DataTable({
                                    "order": [[3, "desc"]]
                                });
                                $('#ModalQRCScannedView  #divQRCScannedPreview').html(QRCScannedBody);
                                $("#ModalQRCScannedView").modal('show');
                                //*******************QRC Scanned Details data table End*******************************//
                            }
                            else {
                                //*******************QRC Scanned Details data table*******************************//
                                var QRCScannedBody = '';
                                QRCScannedBody = "<table id='Scandetail' class='table table-bordered table-striped'><thead><tr><th>Serial No.</th><th>User Name</th><th>QR Code ID</th><th>QRC Name</th><th>Scan Date</th></tr></thead><tbody>";

                                for (i = 0; (i < dataItems.dataJson.length) ; i++) {
                                    QRCScannedBody += '<tr class="odd" role="row"><td class="sorting_1">' + (i + 1) + '</td><td>' + dataItems.dataJson[i]['ScanUserName'] + '</td> <td>' + dataItems.dataJson[i]['QrCodeId'] + '</td><td>' + dataItems.dataJson[i]['QrcName'] + '</td><td>' + dataItems.dataJson[i]['StrCreatedDate'] + '</td></tr>';

                                }
                                QRCScannedBody += "</tbody></table>";
                                $("#Scandetail").DataTable({
                                    "order": [[3, "desc"]]
                                });
                                $('#ModalQRCScannedView  #divQRCScannedPreview').html(QRCScannedBody);
                                $("#ModalQRCScannedView").modal('show');
                                //*******************QRC Scanned Details data table End*******************************//
                            }

                        }
                        else {
                            $('#Scandetail').dataTable().fnDestroy();
                            $('#Scandetail').remove();
                            $("#ModalQRCScannedView  #divQRCScannedPreview").html("No records found");
                            $("#ModalQRCScannedView").modal('show');

                        }

                    }

                }).error(function (xhr) {
                });
            } catch (e) {
                console.log('catch', e);
            }
        }
        else {
            $("#ModalQRCScannedView  #divQRCScannedPreview").html("No records found");
            $("#ModalQRCScannedView").modal('show');
        }

    });

    $("#divUnAssignedWODetails #UnAssignedDetails .assign").click(function () {
        var row = $(this).closest("tr");
        var id = $(this).attr("id");
        var ProblemDesc = row.find("td").eq(7).html();
        var PriorityLevel = row.find("td").eq(11).html();
        var ProjectDesc = row.find("td").eq(7).html();
        var locationId = row.find("td").eq(10).html();
        var WorkRequestType = row.find("td").eq(1).html();
        if (PriorityLevel == null || PriorityLevel == "" || PriorityLevel == 'undefined') {
            PriorityLevel = 0;
        }
        $.ajax({
            type: "GET",
            data: { 'id': id, 'ProblemDesc': ProblemDesc, 'PriorityLevel': PriorityLevel, 'ProjectDesc': ProjectDesc, 'WorkRequestType': WorkRequestType, 'locationId': locationId },
            url: '../GlobalAdmin/_AssignWorkAssignmentRequest/',
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
                new fn_showMaskloader('Loading...');
            },
            error: function (xhr, status, error) {
            },
            success: function (result) {

                $('.modal-title').text("Assign Work Order");
                $("#largeeditpopup").html(result);
                $("#myModallarge").modal('show');
            },
            complete: function () {
                fn_hideMaskloader();
            },
        });
    });
})