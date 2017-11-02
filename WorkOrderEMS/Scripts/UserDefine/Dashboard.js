var Dashboard = {
    //***************Global Admin And IT-Admin**************//
    //Actually this is Progress Assignment for progress bar
    "PendingAssignmnet": function (data) {
        var oper = ""
        var _search = ""
        var txtSearch = ""
        var rows = 100000;
        var page = 1
        var sidx = ""
        var sord = "desc"
        var UserID = "";
        var RequestType = "";

        $.ajax({
            type: "POST",
            url: $_HostPrefix + '/GridListing/JqGridHandler/PendingWorkRequests.ashx?sord=' + sord + '&page=' + page + '&rows=' + rows + '&txtSearch=' + txtSearch + '&_search=' + _search + '&rqtype=GetAllPendingWorkRequest',
            data: '',
            //beforeSend: function () {
            //    new fn_showMaskloader('Please wait...');
            //},
            async: true,
            success: function (Data) {

                var ParseData = $.parseJSON(Data);
                var PendingBarHtmlString = '';
                var PendingBoxString = '';
                PendingBarHtmlString = '';
                PendingBoxString = '';
                $("#TestPending").append('');
                $("#PendingAssignmentDashboardBox").append('');
             
                if (ParseData.rows.length > 0) {
                    for (i = 0; (i < ParseData.rows.length) ; i++) {

                        if (i == 4) { PendingBarHtmlString += '<span id="More_PendingAssignGrid" class="hide">'; }

                        PendingBarHtmlString += '<div class="col-lg-3 col-md-6">';
                        PendingBarHtmlString += '<div class="graybox">';
                        PendingBarHtmlString += '<ul>';
                        PendingBarHtmlString += '<li><span class="left">CodeNo</span><span class="right">' + ParseData.rows[i].cell[24] + '</span></li>';
                        PendingBarHtmlString += '<li><span class="left">Location</span><span class="right">' + ParseData.rows[i].cell[4] + '</span></li>';
                        PendingBarHtmlString += '<li><span class="left">Priority</span><span class="right">' + ParseData.rows[i].cell[7] + '</span></li>';
                        //PendingBarHtmlString += '<li><span class="left">Status</span><span class="right">' + ParseData.rows[i].cell[12] + '</span></li>';
                        PendingBarHtmlString += '<li><span class="left">Assign To</span><span class="right">' + ParseData.rows[i].cell[15] + '</span></li>';
                        PendingBarHtmlString += '<li><span class="left">Project Type</span><span class="right">' + ParseData.rows[i].cell[19] + '</span></li>';
                        PendingBarHtmlString += '<li><span class="left">Created On</span><span class="right">' + ParseData.rows[i].cell[20] + '</span></li>';
                       
                        var AssignTime = ParseData.rows[i].cell[21];
                        var StartTime = ParseData.rows[i].cell[22];
                        var ProjectType = ParseData.rows[i].cell[19];
                        var Progress = "";
                        if (AssignTime == "") {
                            PendingBarHtmlString += '<li><span class="jTimer2" style="display: none">' + ParseData.rows[i].cell[23] + '</span>';
                        } else {

                            var ass_tim = new Date(AssignTime);
                            var stat_tim = new Date(StartTime);
                            var seconds = ass_tim.getSeconds();
                            var minutes = ass_tim.getMinutes();
                            var hours = ass_tim.getHours()
                            var EndTime = new Date(AssignTime);
                            EndTime.setSeconds((seconds + (minutes * 60) + (hours * 60 * 60)));
                            //PendingBarHtmlString += '<li><span class="jTimer2" style="display: none">' + stat_tim.toLocaleDateString() + " " + EndTime.toTimeString().split(" ")[0] + '</span>';
                            PendingBarHtmlString += '<li><span class="left">Progress</span><span class="jTimer2 left" style="display: none">' + ParseData.rows[i].cell[23] + '</span>';
                            Progress += '<div class="progressbar" data-perc="100"><div class="bar"><span></span></div><div class="label"><span></span></div></div>';
                        }

                        //PendingBarHtmlString += '<li><span class="jTimer2" style="display: none">' + EndTime + '</span>';
                        PendingBarHtmlString += '<span class="jTimer1" style="display: none">' + StartTime + '</span>';
                        PendingBarHtmlString += '<span class="jTimer "></span>';
                        PendingBarHtmlString += Progress;
                        PendingBarHtmlString += '</li></ul>';
                        PendingBarHtmlString += '<li><span class="pauseStatus" style="display: none">' + ParseData.rows[i].cell[25] + '</span></li>';
                        //PendingBarHtmlString += '</ul>';
                        PendingBarHtmlString += '</div>';
                        PendingBarHtmlString += '</div>';
                    
                        PendingBoxString += '<li><i class="fa fa-angle-double-right"></i><span class="left">' + ParseData.rows[i].cell[4] + '</span> <span class="right">' + ParseData.rows[i].cell[1] + '</span></li>';

                        if (i == parseInt(parseInt(ParseData.rows.length) - 1)) {
                            PendingBarHtmlString += '</span>';
                            // PendingBoxString += '</span>';
                        }
                    }
                    $("#Pending_Section_Of_WorkOrder").css("display", "block");
                }
                else { $("#Pending_Section_Of_WorkOrder").css("display", "none"); }
                $("#Pending_Section_Of_WorkOrder").html(PendingBarHtmlString);
                $("#TestPending").html(PendingBarHtmlString);
                $("#PendingAssignmentDashboardBox").html(PendingBoxString);


            },
            error: function () {

            },
            complete: function () {
                setInterval(function () {
                    myTimer2();
                }, 8000);
                fn_hideMaskloader();
            }
        });
    },
    "LastWeekWorkAssignments": function (data) {
        var oper = ""
        var _search = ""
        var txtSearch = ""
        var rows = 100000;
        var page = 1
        var sidx = ""
        var sord = "desc"
        var UserID = "";
        $.ajax({
            type: "POST",
            url: $_HostPrefix + '/GridListing/JqGridHandler/WorkOrderWeekHistory.ashx?sord=' + sord + '&page=' + page + '&rows=' + rows + '&txtSearch=' + txtSearch + '&_search=' + _search,
            data: '',
            //beforeSend: function () {
            //    new fn_showMaskloader('Please wait...');
            //},
            async: true,
            success: function (Data) {
                var ParseData = $.parseJSON(Data);
                var HtmlString = '';
                if (ParseData.rows.length > 0) {
                    for (i = 0; (i < ParseData.rows.length) ; i++) {
                        if (i == 4) { HtmlString += '<span id="Box_LastWeekAssignment"class="hide">'; }

                        HtmlString += '<div class="col-lg-3 col-md-6">';
                        HtmlString += '<div class="graybox">';
                        HtmlString += '<ul>';
                        HtmlString += '<li><span class="left">CodeNo</span><span class="right">' + ParseData.rows[i].cell[21] + '</span></li>';
                        HtmlString += '<li><span class="left">Location</span><span class="right">' + ParseData.rows[i].cell[4] + '</span></li>';
                        HtmlString += '<li><span class="left">Priority</span><span class="right">' + ParseData.rows[i].cell[7] + '</span></li>';
                        //HtmlString += '<li><span class="left">Status</span><span class="right">' + ParseData.rows[i].cell[12] + '</span></li>';
                        if (ParseData.rows[i].cell[15] != null && $.trim(ParseData.rows[i].cell[15]) != "") {
                            HtmlString += '<li><span class="left">Assign To</span><span class="right">' + ParseData.rows[i].cell[15] + '</span></li>';
                        }
                        else {
                            HtmlString += '<li><span class="left">Assign To</span><span class="right">' + "Not Assigned" + '</span></li>';
                        }                    
                        HtmlString += '<li><span class="left">Project Type</span><span class="right">' + ParseData.rows[i].cell[19] + '</span></li>';
                        HtmlString += '<li><span class="left">Created On</span><span class="right">' + ParseData.rows[i].cell[20] + '</span></li>';
                        HtmlString += '</ul>';
                        HtmlString += '</div>';
                        HtmlString += '</div>';
                        if (i == parseInt(parseInt(ParseData.rows.length) - 1)) {

                            HtmlString += '</span>';
                        }
                    }
                    $("#lastweekHistoryAssignments").css("display", "block")
                }
                else { $("#lastweekHistoryAssignments").css("display", "none") }
                $("#lastweekHistoryAssignments").html(HtmlString);

            },
            error: function () {

            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    },
    "AssignedWorkRequests": function (data) {
        var oper = ""
        var _search = ""
        var txtSearch = ""
        var rows = 100000;
        var page = 1
        var sidx = ""
        var sord = "desc"
        var UserID = "";
        var RequestType = "";

        $.ajax({
            type: "POST",
            url: $_HostPrefix + '/GridListing/JqGridHandler/PendingWorkRequests.ashx?sord=' + sord + '&page=' + page + '&rows=' + rows + '&txtSearch=' + txtSearch + '&_search=' + _search + '&rqtype=GetAllAssignedWorkRequest',
            data: '',
            //beforeSend: function () {
            //    new fn_showMaskloader('Please wait...');
            //},
            async: true,
            success: function (Data) {
                var ParseData = $.parseJSON(Data);

                var AssignedBoxString = '';
                if (ParseData) {
                    for (i = 0; (i < ParseData.rows.length) ; i++) {

                        if (i == 4) { AssignedBoxString += '<span id="Box_AssignedAssignment"class="hide">'; }


                        AssignedBoxString += '<li><i class="fa fa-angle-double-right"></i><span class="left">' + ParseData.rows[i].cell[4] + '</span> <span class="">' + ParseData.rows[i].cell[1] + '</span><span class="left">' + ParseData.rows[i].cell[15] + '</span></li>';
                        if (i == parseInt(parseInt(ParseData.rows.length) - 1)) {

                            AssignedBoxString += '</span>';
                        }
                    }

                }
                $("#AssignedAssignmentsRequest").html(AssignedBoxString);
                //AssignedBoxString = '';

                //for (i = 0; (i < ParseData.rows.length) ; i++) {
                //    AssignedBoxString += '<li><i class="fa fa-angle-double-right"></i><span class="left">' + ParseData.rows[i].cell[4] + '</span> <span class="">' + ParseData.rows[i].cell[1] + '</span><span class="left">' + ParseData.rows[i].cell[15] + '</span></li>';
                //}
                ////$("#TestPending").html(PendingBarHtmlString);
                //$("#AssignedAssignmentsRequest").html(AssignedBoxString);

            },
            error: function () {

            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    },
    "AllWorkAssignment": function (data) {
        var oper = ""
        var _search = ""
        var txtSearch = ""
        var rows = 100000;
        var page = 1
        var sidx = ""
        var sord = "desc"
        var UserID = "";
        var RequestType = "";

        $.ajax({
            type: "POST",
            url: $_HostPrefix + '/GridListing/JqGridHandler/WorkRequestAssignmentList.ashx?sord=' + sord + '&page=' + page + '&rows=' + rows + '&txtSearch=' + txtSearch + '&_search=' + _search,
            data: '',
            //beforeSend: function () {
            //    new fn_showMaskloader('Please wait...');
            //},
            async: true,
            success: function (Data) {
                var ParseData = $.parseJSON(Data);
                var PendingBarHtmlString = '';
                var PendingBoxString = '';//'<li><i class="fa "></i><span class="left">Location Name</span><span class="right">Type</span></li>';
                for (i = 0; (i < ParseData.rows.length) ; i++) {

                    if (i == 4) { PendingBoxString += '<span id="Box_allWorkAssignment"class="hide">'; }


                    PendingBoxString += '<li><i class="fa fa-angle-double-right"></i><span class="left">' + ParseData.rows[i].cell[0] + '</span> <span class="right">' + ParseData.rows[i].cell[6] + '</span></li>';
                    if (i == parseInt(parseInt(ParseData.rows.length) - 1)) {
                        PendingBarHtmlString += '</span>';
                        PendingBoxString += '</span>';
                    }

                }

                $("#Box_DisplayAllWorkAssignment").html(PendingBoxString);


            },
            error: function () {

            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    },
    "GetAllWorkRequestCreatedByClient": function (data) {
        var oper = "";
        var _search = "";
        var txtSearch = "";
        var rows = 100000;
        var page = 1;
        var sidx = "LocationName";
        var sord = "desc";
        var UserID = "";
        var RequestType = "";
        var filter = "";
        var operation = "NotAssignedBasedOnLocationCreated";
        $.ajax({
            type: "POST",
            url: $_HostPrefix + '/Common/GetWorkOrderCreatedByClient?sord=' + sord + '&PageNo=' + page + '&NoOfRecords=' + rows + '&txtSearch=' + txtSearch + '&_search=' + _search + '&operation=' + operation + '&sidx=' + sidx + '&filter=' + filter,
            data: '',
            //beforeSend: function () {
            //    new fn_showMaskloader('Please wait...');
            //},
            async: true,
            success: function (Data) {

                if (Data.length > 0) {
                    var HtmlString = "";
                    for (i = 0; (i < Data.length) ; i++) {
                        if (i == 4) { HtmlString += '<span id="Box_LastWeekAssignment" class="hide">'; }

                        HtmlString += '<div class="col-lg-3 col-md-6">';
                        HtmlString += '<div class="graybox">';
                        HtmlString += '<ul>';
                        HtmlString += '<li><span class="left">CodeNo</span><span class="right">' + Data[i].CodeID + '</span></li>';
                        HtmlString += '<li><span class="left">Location</span><span class="right">' + Data[i].LocationName + '</span></li>';
                        HtmlString += '<li><span class="left">Priority</span><span class="right">' + Data[i].PriorityLevelName + '</span></li>';
                        HtmlString += '<li><span class="left">Status</span><span class="right">' + Data[i].WorkRequestStatusName + '</span></li>';
                        HtmlString += '<li><span class="left">Description</span><span class="right">' + Data[i].ProblemDesc + '</span></li>';
                        HtmlString += '<li><span class="left">Project Type</span><span class="right">' + Data[i].WorkRequestProjectType + '</span></li>';
                        HtmlString += '<li><span class="left">Created Date</span><span class="right">' + Data[i].CreationDate + '</span></li>';
                        HtmlString += '</ul>';
                        HtmlString += '</div>';
                        HtmlString += '</div>';
                        if (i == parseInt(parseInt(Data.length) - 1)) {

                            HtmlString += '</span>';
                        }
                    }
                    $("#WorkorderCreatedByClient").html(HtmlString);
                    $("#WorkorderCreatedByClient").css("display", "block")
                }
                else {
                    $("#WorkorderCreatedByClient").hide();
                }

            },
            error: function () {

            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    },
    "GetAllWorkRequestCreatedByClientToEmployee": function (data) {
        var oper = "";
        var _search = "";
        var txtSearch = "";
        var rows = 100000;
        var page = 1;
        var sidx = "LocationName";
        var sord = "desc";
        var UserID = "";
        var RequestType = "";
        var filter = "";
        var operation = "NotAssignedBasedOnLocationCreated";
        $.ajax({
            type: "POST",
            url: $_HostPrefix + '/Common/GetWorkOrderCreatedByClient?sord=' + sord + '&PageNo=' + page + '&NoOfRecords=' + rows + '&txtSearch=' + txtSearch + '&_search=' + _search + '&operation=' + operation + '&sidx=' + sidx + '&filter=' + filter,
            data: '',
            //beforeSend: function () {
            //    new fn_showMaskloader('Please wait...');
            //},
            async: true,
            success: function (Data) {

                if (Data.length > 0) {
                    var HtmlString = "";
                    for (i = 0; (i < Data.length) ; i++) {
                        if (i == 4) { HtmlString += '<span id="Box_LastWeekAssignment" class="hide">'; }

                        HtmlString += '<div class="col-lg-3 col-md-6">';
                        HtmlString += '<div class="graybox">';
                        HtmlString += '<ul>';
                        HtmlString += '<li><span class="left">CodeNo</span><span class="right">' + Data[i].CodeID + '</span></li>';
                        HtmlString += '<li><span class="left">Location</span><span class="right">' + Data[i].LocationName + '</span></li>';
                        HtmlString += '<li><span class="left">Priority</span><span class="right">' + Data[i].PriorityLevelName + '</span></li>';
                        HtmlString += '<li><span class="left">Status</span><span class="right">' + Data[i].WorkRequestStatusName + '</span></li>';
                        HtmlString += '<li><span class="left">Description</span><span class="right">' + Data[i].ProblemDesc + '</span></li>';
                        HtmlString += '<li><span class="left">Project Type</span><span class="right">' + Data[i].WorkRequestProjectType + '</span></li>';
                        //HtmlString += '<li><span class="left">Created Date</span><span class="right">' + Data[i].cell[20] + '</span></li>';
                        HtmlString += '</ul>';
                        HtmlString += '</div>';
                        HtmlString += '</div>';
                        if (i == parseInt(parseInt(Data.length) - 1)) {

                            HtmlString += '</span>';
                        }
                    }
                    $("#WorkorderCreatedByClient").html(HtmlString);
                    $("#WorkorderCreatedByClient").css("display", "block")
                }
                else {
                    $("#WorkorderCreatedByClient").hide();
                }

            },
            error: function () {

            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    },
    //************************END**************************//_
    //*****************For Manager Section*****************//
    //Pending
    "AllAssignedWorkByManager": function (data) {
        var oper = ""
        var _search = ""
        var txtSearch = ""
        var rows = 100000;
        var page = 1
        var sidx = ""
        var sord = "desc"
        var UserID = "";
        var RequestType = "";

        $.ajax({
            type: "POST",
            url: $_HostPrefix + '/GridListing/JqGridHandler/WorkOrderAssignmentByManager.ashx?sord=' + sord + '&page=' + page + '&rows=' + rows + '&txtSearch=' + txtSearch + '&_search=' + _search,
            data: '',
            //beforeSend: function () {
            //    new fn_showMaskloader('Please wait...');
            //},
            async: true,
            success: function (Data) {  
                var ParseData = $.parseJSON(Data);
                var AssignedBoxString = '', HtmlString = '';
                if (ParseData) {
                    for (i = 0; (i < ParseData.rows.length) ; i++) {

                        if (i == 4) { AssignedBoxString += '<span id="Box_AssignedAssignment"class="hide">'; HtmlString += '<span id="Box_LastWeekAssignment"class="hide">'; }

                        HtmlString += '<div class="col-lg-3 col-md-6">';
                        HtmlString += '<div class="graybox">';
                        HtmlString += '<ul>';
                        HtmlString += '<li><span class="left">CodeNo</span><span class="right">' + ParseData.rows[i].cell[20] + '</span></li>';
                        HtmlString += '<li><span class="left">Location</span><span class="right">' + ParseData.rows[i].cell[4] + '</span></li>';
                        HtmlString += '<li><span class="left">Priority</span><span class="right">' + ParseData.rows[i].cell[7] + '</span></li>';
                        //HtmlString += '<li><span class="left">Status</span><span class="right">' + ParseData.rows[i].cell[12] + '</span></li>';
                        if (ParseData.rows[i].cell[15] != null && $.trim(ParseData.rows[i].cell[15]) != "") {
                            HtmlString += '<li><span class="left">Assign To</span><span class="right">' + ParseData.rows[i].cell[15] + '</span></li>';
                        }
                        else {
                            HtmlString += '<li><span class="left">Assign To</span><span class="right">' + "Not Assigned" + '</span></li>';
                        }
                        //HtmlString += '<li><span class="left">Assign To</span><span class="right">' + ParseData.rows[i].cell[15] + '</span></li>';
                        HtmlString += '<li><span class="left">Project Type</span><span class="right">' + ParseData.rows[i].cell[19] + '</span></li>';
                        HtmlString += '<li><span class="left">Created On</span><span class="right">' + ParseData.rows[i].cell[21] + '</span></li>';
                        HtmlString += '</ul>';
                        HtmlString += '</div>';
                        HtmlString += '</div>';

                        AssignedBoxString += '<li><i class="fa fa-angle-double-right"></i><span class="left">' + ParseData.rows[i].cell[4] + '</span> <span class="">' + ParseData.rows[i].cell[1] + '</span><span class="left">' + ParseData.rows[i].cell[15] + '</span></li>';
                        if (i == parseInt(parseInt(ParseData.rows.length) - 1)) {

                            AssignedBoxString += '</span>';
                            HtmlString += '</span>';
                        }
                    }

                }
                $("#AssignedAssignmentsRequest").html(AssignedBoxString);
                $("#lastweekHistoryAssignments").html(HtmlString);

            },
            error: function () {

            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    },
    //In Progress 
    "AllWorkOrderAssignmnetByManager": function (data) {

        var oper = ""
        var _search = ""
        var txtSearch = ""
        var rows = 100000;
        var page = 1
        var sidx = ""
        var sord = "desc"
        var UserID = "";
        var RequestType = "";

        $.ajax({
            type: "POST",
            url: $_HostPrefix + '/GridListing/JqGridHandler/ManagerAllWorkOrders.ashx?sord=' + sord + '&page=' + page + '&rows=' + rows + '&txtSearch=' + txtSearch + '&_search=' + _search + '&rqtype=GetAllPendingWorkRequest',
            data: '',
            //beforeSend: function () {
            //    new fn_showMaskloader('Please wait...');
            //},
            async: true,
            success: function (Data) {
                var ParseData = $.parseJSON(Data);
    
                var AllWorkOrder = '';
                var AllWorkOrderBoxString = '';
                $("#TestPending").html('');
                $("#PendingAssignmentDashboardBox").html('');
              
                for (i = 0; (i < ParseData.rows.length) ; i++) {

                    if (i == 4) { AllWorkOrder += '<span id="More_PendingAssignGrid" class="hide">'; AllWorkOrderBoxString += '<span id="Box_PendingAssignment" class="hide">'; }

                    AllWorkOrder += '<div class="col-lg-3 col-md-6">';
                    AllWorkOrder += '<div class="graybox">';
                    AllWorkOrder += '<ul>';
                    AllWorkOrder += '<li><span class="left">CodeNo</span><span class="right">' + ParseData.rows[i].cell[20] + '</span></li>';
                    AllWorkOrder += '<li><span class="left">Location</span><span class="right">' + ParseData.rows[i].cell[4] + '</span></li>';
                    AllWorkOrder += '<li><span class="left">Priority</span><span class="right">' + ParseData.rows[i].cell[7] + '</span></li>';
                    //AllWorkOrder += '<li><span class="left">Status</span><span class="right">' + ParseData.rows[i].cell[12] + '</span></li>';
                    //if (ParseData.rows[i].cell[10] != null && ParseData.rows[i].cell[10] != "") {
                    //    AllWorkOrder += '<li><span class="left">Description</span><span class="right">' + ParseData.rows[i].cell[10] + '</span></li>';
                    //}
                    //PendingBarHtmlString += '<li><span class="left">Assign To User</span><span class="right">' + ParseData.rows[i].cell[12] + '</span></li>';
                    AllWorkOrder += '<li><span class="left">Project Type</span><span class="right">' + ParseData.rows[i].cell[19] + '</span></li>';
                    AllWorkOrder += '<li><span class="left">Assign To</span><span class="right">' + ParseData.rows[i].cell[15] + '</span></li>';
                    AllWorkOrder += '<li><span class="left">Created On</span><span class="right">' + ParseData.rows[i].cell[21] + '</span></li>';
        
                    var AssignTime = ParseData.rows[i].cell[22];
                    var StartTime = ParseData.rows[i].cell[23];
                    var Progress = "";
                    if (AssignTime == "") {
                        AllWorkOrder += '<li><span class="jTimer2" style="display: none">' + ParseData.rows[i].cell[24] + '</span>';
                    } else {
                        var ass_tim = new Date(AssignTime);
                        var stat_tim = new Date(StartTime);
                        var seconds = ass_tim.getSeconds();
                        var minutes = ass_tim.getMinutes();
                        var hours = ass_tim.getHours()
                        var EndTime = new Date(AssignTime);
                        EndTime.setSeconds((seconds + (minutes * 60) + (hours * 60 * 60)));
                        //PendingBarHtmlString += '<li><span class="jTimer2" style="display: none">' + stat_tim.toLocaleDateString() + " " + EndTime.toTimeString().split(" ")[0] + '</span>';
                        AllWorkOrder += '<li><span class="left">Progress</span><span class="jTimer2 left" style="display: none">' + ParseData.rows[i].cell[24] + '</span>';
                        Progress += '<div class="progressbar" data-perc="100"><div class="bar"><span></span></div><div class="label"><span></span></div></div>';
                    }

                    //PendingBarHtmlString += '<li><span class="jTimer2" style="display: none">' + EndTime + '</span>';
                    AllWorkOrder += '<span class="jTimer1" style="display: none">' + StartTime + '</span>';
                    AllWorkOrder += '<span class="jTimer "></span>';
                    AllWorkOrder += Progress;
                    AllWorkOrder += '</li></ul>';

                    //AllWorkOrder += '</ul>';
                    AllWorkOrder += '</div>';
                    AllWorkOrder += '</div>';

                    AllWorkOrderBoxString += '<li><i class="fa fa-angle-double-right"></i><span class="left">' + ParseData.rows[i].cell[4] + '</span> <span class="left">' + ParseData.rows[i].cell[1] + '</span><span class="">' + ParseData.rows[i].cell[20] + '</span></li>';
                    if (i == parseInt(parseInt(ParseData.rows.length) - 1)) {
                        AllWorkOrder += '</span>';
                        AllWorkOrderBoxString += '</span>';
                    }

                }
                $("#Pending_Section_Of_WorkOrder").html(AllWorkOrder);
                $("#TestPending").html(AllWorkOrder);
                $("#PendingAssignmentDashboardBox").html(AllWorkOrderBoxString);
            },
            error: function () {

            },
            complete: function () {
                setInterval(function () {
                    myTimer2();
                }, 8000);
                fn_hideMaskloader();
            }
        });
    },
    "GetEmployeeList": function () {
        $.ajax({
            type: "POST",
            url: DashboardUrl.AllEmployeesList,
            data: { "LoginUserType": "Employee" },
            //beforeSend: function () {
            //    new fn_showMaskloader('Please wait...');
            //},
            async: true,
            success: function (Data) {
                var res = jQuery.type(Data);
                if (res == "string") {
                    alert(Data);
                }
                else {
                    var EmployeeBoxString = '';
                    for (i = 0; i < Data.length; i++) {
                        if (i == 4) { EmployeeBoxString += '<span id="Box_allWorkAssignment"class="hide">'; }


                        EmployeeBoxString += '<li><i class="fa fa-angle-double-right"></i><span class="left">' + Data[i].FirstName + " " + Data[i].LastName + '</span> <span class="right">' + Data[i].UserEmail + '</span></li>';
                        if (i == parseInt(parseInt(Data.length) - 1)) {
                            EmployeeBoxString += '</span>';
                        }
                    }
                    $("#Box_DisplayAllWorkAssignment").html(EmployeeBoxString);

                }
            },
            error: function () {

            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    },
    //*****************For Employee Section*****************//
    "AllPendingAssignedWorkToEmployee": function (data) {
        var oper = ""
        var _search = ""
        var txtSearch = ""
        var rows = 100000;
        var page = 1
        var sidx = ""
        var sord = "desc"
        var UserID = "";
        var RequestType = "";
        var filter = "Pending";
        var operation = "GetAssignedWorktoEmployee";
        $.ajax({
            type: "POST",
            url: $_HostPrefix + '/GridListing/JqGridHandler/PendingWorkRequests.ashx?sord=' + sord + '&page=' + page + '&rows=' + rows + '&txtSearch=' + txtSearch + '&_search=' + _search + '&rqtype=' + operation + '&filter=' + filter,
            data: '',
            //beforeSend: function () {
            //    new fn_showMaskloader('Please wait...');
            //},
            async: true,
            success: function (Data) {
                var ParseData = $.parseJSON(Data);
                var PendingBarHtmlString = "";
                if (ParseData.rows.length > 0) {
                    for (i = 0; (i < ParseData.rows.length) ; i++) {

                        if (i == 4) { PendingBarHtmlString += '<span id="More_PendingAssignGrid" class="hide">'; }

                        PendingBarHtmlString += '<div class="col-lg-3 col-md-6">';
                        PendingBarHtmlString += '<div class="graybox">';
                        PendingBarHtmlString += '<ul>';
                        PendingBarHtmlString += '<li><span class="left">CodeNo</span><span class="right">' + ParseData.rows[i].cell[24] + '</span></li>';
                        PendingBarHtmlString += '<li><span class="left">Location</span><span class="right">' + ParseData.rows[i].cell[4] + '</span></li>';
                        PendingBarHtmlString += '<li><span class="left">Priority</span><span class="right">' + ParseData.rows[i].cell[7] + '</span></li>';
                        PendingBarHtmlString += '<li><span class="left">Status</span><span class="right">' + ParseData.rows[i].cell[12] + '</span></li>';
                        //PendingBarHtmlString += '<li><span class="left">Assign To User</span><span class="right">' + ParseData.rows[i].cell[12] + '</span></li>';
                        PendingBarHtmlString += '<li><span class="left">Project Type</span><span class="right">' + ParseData.rows[i].cell[19] + '</span></li>';
                        PendingBarHtmlString += '<li><span class="left">Created Date</span><span class="right">' + ParseData.rows[i].cell[20] + '</span></li>';
                        PendingBarHtmlString += '</ul>';
                        PendingBarHtmlString += '<div><span class=""><input type="button" value="Start Work" title="Click here to Start the work." class="btn btn-default bluebutton btn_full" onclick="Dashboard.StartWorkorder(this)" id="' + ParseData.rows[i].id + '"/></span></div>';
                        PendingBarHtmlString += '</div>';
                        PendingBarHtmlString += '</div>';

                        if (i == parseInt(parseInt(ParseData.rows.length) - 1)) {
                            PendingBarHtmlString += '</span>';

                        }

                    }
                    $("#lastweekHistoryAssignments").css("display", "block");
                }
                else { $("#lastweekHistoryAssignments").css("display", "none"); }
                $("#lastweekHistoryAssignments").html(PendingBarHtmlString);
            },
            error: function () {

            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    },
    "GetAllWorkRequestCreatedByClientToEmployee": function (data) {
        var oper = "";
        var _search = "";
        var txtSearch = "";
        var rows = 100000;
        var page = 1;
        var sidx = "LocationName";
        var sord = "desc";
        var UserID = "";
        var RequestType = "";
        var operation = "NotAssignedBasedOnLocationCreated";
        $.ajax({
            type: "POST",
            url: $_HostPrefix + '/Common/GetWorkOrderCreatedByClient?sord=' + sord + '&PageNo=' + page + '&NoOfRecords=' + rows + '&txtSearch=' + txtSearch + '&_search=' + _search + '&operation=' + operation + '&sidx=' + sidx,
            data: '',
            //beforeSend: function () {
            //    new fn_showMaskloader('Please wait...');
            //},
            async: true,
            success: function (Data) {
                if (Data.length > 0) {
                    var HtmlString = "";
                    var AssignmentBoxString = "";
                    for (i = 0; (i < Data.length) ; i++) {
                        if (i == 4) { HtmlString += '<span id="Box_LastWeekAssignment" class="hide">'; AssignmentBoxString += '<span id="Box_allWorkAssignment"class="hide">'; }

                        HtmlString += '<div class="col-lg-3 col-md-6">';
                        HtmlString += '<div class="graybox">';
                        HtmlString += '<ul>';
                        HtmlString += '<li><span class="left">CodeNo</span><span class="right">' + Data[i].CodeID + '</span></li>';
                        HtmlString += '<li><span class="left">Location</span><span class="right">' + Data[i].LocationName + '</span></li>';
                        HtmlString += '<li><span class="left">Priority</span><span class="right">' + Data[i].PriorityLevelName + '</span></li>';
                        HtmlString += '<li><span class="left">Status</span><span class="right">' + Data[i].WorkRequestStatusName + '</span></li>';
                        HtmlString += '<li><span class="left">Description</span><span class="right">' + Data[i].ProblemDesc + '</span></li>';
                        HtmlString += '<li><span class="left">Project Type</span><span class="right">' + Data[i].WorkRequestProjectTypeName + '</span></li>';
                        HtmlString += '<li><span class="left">Created on</span><span class="right">' + Data[i].CreationDate + '</span></li>';
                        HtmlString += '</ul>';
                        HtmlString += '<div><span class=""><input type="button" value="Accept Order" title="Click here to accept the order request." class="btn btn-default bluebutton btn_full" onclick="Dashboard.AcceptWorkorder(this)" id="' + Data[i].WorkRequestID + '"/></span></div>';
                        HtmlString += '</div>';
                        HtmlString += '</div>';
                        AssignmentBoxString += '<li><i class="fa fa-angle-double-right"></i><span class="">' + Data[i].ProblemDesc + " " + Data[i].WorkRequestProjectTypeName + '</span> <span class=""> on ' + Data[i].CreationDate + '</span></li>';
                        if (i == parseInt(parseInt(Data.length) - 1)) {
                            AssignmentBoxString += '</span>';
                            HtmlString += '</span>';
                        }
                    }
                    $("#Box_DisplayAllWorkAssignment").html(AssignmentBoxString);
                    $("#WorkorderCreatedByClient").html(HtmlString);
                    $("#WorkorderCreatedByClient").css("display", "block")
                }
                else {
                    $("#WorkorderCreatedByClient").hide();
                }

            },
            error: function () {

            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    },
    "AcceptWorkorder": function (data) {
        bootbox.confirm("Are you sure ! you want to accept the request?", function (result) {
            if (result == true) {
                if (data !== "") {
                    var res = data.id;

                    $.ajax({
                        type: "POST",
                        url: $_HostPrefix + '/Employee/AcceptWorkOrderCreatedByClient',
                        data: { "Id": res },
                        beforeSend: function () {
                            new fn_showMaskloader('Please wait...');
                        },
                        success: function (Data) {
                            bootbox.alert(Data);
                            Dashboard.GetAllWorkRequestCreatedByClientToEmployee();
                            Dashboard.AllPendingAssignedWorkToEmployee();

                        },
                        error: function () {

                        },
                        complete: function () {
                            fn_hideMaskloader();
                        }
                    });
                }
            }
        });
    },
    "StartWorkorder": function (data) {
        bootbox.confirm("Are you ready to start Work ?", function (result) {
            if (result == true) {
                if (data !== "") {
                    var WorOrderID = data.id;
                    //Commented by Bhushan Dod on 04/10/2015 for no need to send time from here  
                    var stdate = new Date();
                    var StartTime = parseInt(parseInt(stdate.getMonth()) + 1) + "/" + stdate.getDate() + "/" + stdate.getFullYear() + " " + stdate.getHours() + ":" + stdate.getMinutes() + ":" + stdate.getSeconds();
                 
                    $.ajax({
                        type: "POST",
                        url: $_HostPrefix + '/Employee/StartWorkOrderByEmployee',
                        data: { "Id": WorOrderID, "StartTime": StartTime },
                        beforeSend: function () {
                            new fn_showMaskloader('Please wait...');
                        },
                        success: function (Data) {
                            bootbox.alert(Data);
                            Dashboard.AllInprogressAssignedWorkToEmployee();
                            Dashboard.CompletedAssignment();

                        },
                        error: function () {

                        },
                        complete: function () {
                            fn_hideMaskloader();
                        }
                    });
                }
            }
        });
    },
    "CompleteWorkOrder": function (data) {
        bootbox.confirm("Are you sure! You want to change status to complete?", function (result) {
            if (result == true) {
                if (data !== "") {
                    var WorOrderID = data.id;
                    //Commented by Bhushan Dod on 04/10/2015 for no need to send time from here  
                    var stdate = new Date();
                    var EndTime = stdate.getMonth() + "/" + stdate.getDate() + "/" + stdate.getFullYear() + " " + stdate.getHours() + ":" + stdate.getMinutes() + ":" + stdate.getSeconds();
                    $.ajax({
                        type: "POST",
                        url: $_HostPrefix + '/Employee/CompleteWorkOrderByEmployee',
                        data: { "Id": WorOrderID, "EndTime": EndTime },
                        beforeSend: function () {
                            new fn_showMaskloader('Please wait...');
                        },
                        success: function (Data) {
                            bootbox.alert(Data);
                            Dashboard.AllInprogressAssignedWorkToEmployee();
                            Dashboard.WorkOrderTotalCounts();
                        },
                        error: function () {

                        },
                        complete: function () {
                            fn_hideMaskloader();
                        }
                    });
                }
            }
        });
    },
    "AllInprogressAssignedWorkToEmployee": function (data) {
        var oper = ""
        var _search = ""
        var txtSearch = ""
        var rows = 100000;
        var page = 1
        var sidx = ""
        var sord = "desc"
        var UserID = "";
        var RequestType = "";
        var filter = "In Progress";
        var operation = "GetAssignedWorktoEmployee";
        $.ajax({
            type: "POST",
            url: $_HostPrefix + '/GridListing/JqGridHandler/PendingWorkRequests.ashx?sord=' + sord + '&page=' + page + '&rows=' + rows + '&txtSearch=' + txtSearch + '&_search=' + _search + '&rqtype=' + operation + '&filter=' + filter,
            data: '',
            //beforeSend: function () {
            //    new fn_showMaskloader('Please wait...');
            //},
            async: true,
            success: function (Data) {
                var ParseData = $.parseJSON(Data);

                var PendingBarHtmlString = "";
                if (ParseData.rows.length > 0) {
                    for (i = 0; (i < ParseData.rows.length) ; i++) {

                        if (i == 4) { PendingBarHtmlString += '<span id="More_PendingAssignGrid" class="hide">'; }

                        PendingBarHtmlString += '<div class="col-lg-3 col-md-6">';
                        PendingBarHtmlString += '<div class="graybox">';
                        PendingBarHtmlString += '<ul>';
                        PendingBarHtmlString += '<li><span class="left">CodeNo</span><span class="right">' + ParseData.rows[i].cell[24] + '</span></li>';
                        PendingBarHtmlString += '<li><span class="left">Location</span><span class="right">' + ParseData.rows[i].cell[4] + '</span></li>';
                        PendingBarHtmlString += '<li><span class="left">Priority </span><span class="right">' + ParseData.rows[i].cell[7] + '</span></li>';
                        PendingBarHtmlString += '<li><span class="left">Status</span><span class="right">' + ParseData.rows[i].cell[12] + '</span></li>';
                        //PendingBarHtmlString += '<li><span class="left">Assign To User</span><span class="right">' + ParseData.rows[i].cell[12] + '</span></li>';
                        PendingBarHtmlString += '<li><span class="left">Project Type</span><span class="right">' + ParseData.rows[i].cell[19] + '</span></li>';
                        PendingBarHtmlString += '<li><span class="left">Created Date</span><span class="right">' + ParseData.rows[i].cell[20] + '</span></li>';
                        var AssignTime = ParseData.rows[i].cell[21];
                        var StartTime = ParseData.rows[i].cell[22];
                        var Progress = "";
                        if (AssignTime == "") {
                            PendingBarHtmlString += '<li><span class="jTimer2" style="display: none">' + ParseData.rows[i].cell[23] + '</span>';
                        } else {
                            var ass_tim = new Date(AssignTime);
                            var stat_tim = new Date(StartTime);
                            var seconds = ass_tim.getSeconds();
                            var minutes = ass_tim.getMinutes();
                            var hours = ass_tim.getHours()
                            var EndTime = new Date(AssignTime);
                            EndTime.setSeconds((seconds + (minutes * 60) + (hours * 60 * 60)));
                            //PendingBarHtmlString += '<li><span class="jTimer2" style="display: none">' + stat_tim.toLocaleDateString() + " " + EndTime.toTimeString().split(" ")[0] + '</span>';
                            PendingBarHtmlString += '<li><span class="left">Progress</span><span class="jTimer2 left" style="display: none">' + ParseData.rows[i].cell[23] + '</span>';
                            Progress += '<div class="progressbar" data-perc="100"><div class="bar"><span></span></div><div class="label"><span></span></div></div>';
                        }

                        //PendingBarHtmlString += '<li><span class="jTimer2" style="display: none">' + EndTime + '</span>';
                        PendingBarHtmlString += '<span class="jTimer1" style="display: none">' + StartTime + '</span>';
                        PendingBarHtmlString += '<span class="jTimer "></span>';
                        PendingBarHtmlString += Progress;
                        PendingBarHtmlString += '</li></ul>';
                        PendingBarHtmlString += '<div><span class=""><input type="button" value="Complete Order" title="Click here to Complete the order status." class="btn btn-default bluebutton btn_full" onclick="Dashboard.CompleteWorkOrder(this)" id="' + ParseData.rows[i].id + '"/></span></div>';
                        PendingBarHtmlString += '</div>';
                        PendingBarHtmlString += '</div>';

                        if (i == parseInt(parseInt(ParseData.rows.length) - 1)) {
                            PendingBarHtmlString += '</span>';

                        }

                    }
                    $("#Pending_Section_Of_WorkOrder").css("display", "block");
                }
                else { $("#Pending_Section_Of_WorkOrder").css("display", "none"); }
                $("#Pending_Section_Of_WorkOrder").html(PendingBarHtmlString);
            },
            error: function () {

            },
            complete: function () {
                setInterval(function () {
                    myTimer2();
                }, 8000);

                fn_hideMaskloader();
            }
        });
    },
    "WorkOrderTotalCounts": function (data) {
        $.ajax({
            type: "POST",
            url: $_HostPrefix + '/Employee/GetEmployeeTotalWorkStatus',
            data: '',
            //beforeSend: function () {
            //    new fn_showMaskloader('Please wait...');
            //},
            async: true,
            success: function (Data) {
                var Res = [];
                var statusDetail = "";
                if (Data.length > 0) {
                    for (i = 0; i < Data.length; i++) {
                        var inne = {};
                        inne.label = Data[i].CodeName;
                        inne.value = Data[i].Column1;
                        Res.push(inne);
                        statusDetail = statusDetail + "<li><i class=''></i>" + Data[i].CodeName + "  (" + Data[i].Column1 + ")</li>";

                    }
                }
                $("#morris-donut-chart").html("");
                $("#TotalStatus").html(statusDetail);
                Morris.Donut({
                    element: 'morris-donut-chart',
                    data: Res
                });
                //Morris.Donut({
                //    element: 'morris-donut-chart',
                //    data: [{
                //        label: "Download Sales",
                //        value: 12
                //    }, {
                //        label: "In-Store Sales",
                //        value: 30
                //    }, {
                //        label: "Mail-Order Sales",
                //        value: 20
                //    }],
                //    resize: true
                //});

            },
            error: function () {

            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    },
    "CompletedAssignment": function (data) {
        var oper = ""
        var _search = ""
        var txtSearch = ""
        var rows = 100000;
        var page = 1
        var sidx = ""
        var sord = "desc"
        var UserID = "";
        var RequestType = "";
        var filter = "Complete";
        var operation = "GetAssignedWorktoEmployee";
        $.ajax({
            type: "POST",
            url: $_HostPrefix + '/GridListing/JqGridHandler/PendingWorkRequests.ashx?sord=' + sord + '&page=' + page + '&rows=' + rows + '&txtSearch=' + txtSearch + '&_search=' + _search + '&rqtype=' + operation + '&filter=' + filter,
            data: '',
            //beforeSend: function () {
            //    new fn_showMaskloader('Please wait...');
            //},
            async: true,
            success: function (Data) {
                var ParseData = $.parseJSON(Data);
                var PendingBarHtmlString = "";

                $("#CompletedWorkAssignment").html(ParseData.rows.length);
            },
            error: function () {

            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    },
    //************************END**************************//
    //******************For Client Section*****************//
    "Client_CreatedWorkOrder": function (data) {
        var oper = "";
        var _search = "";
        var txtSearch = "";
        var rows = 100000;
        var page = 1;
        var sidx = "WorkRequestAssignmentID";
        var sord = "desc";
        var UserID = "";
        var filter = "";
        var RequestType = "";
        var operation = "UnAssignWorkrequestCreatedByClient";
        $.ajax({
            type: "POST",
            url: $_HostPrefix + '/Common/GetWorkOrderCreatedByClient?sord=' + sord + '&PageNo=' + page + '&NoOfRecords=' + rows + '&txtSearch=' + txtSearch + '&_search=' + _search + '&operation=' + operation + '&sidx=' + sidx + '&filter=' + filter,
            data: '',
            beforeSend: function () {
                new fn_showMaskloader('Please wait...');
            },
            success: function (Data) {
                if (Data.length > 0) {

                    var HtmlString = "";
                    var AssignmentBoxString = "";
                    for (i = 0; (i < Data.length) ; i++) {
                        if (i == 4) { HtmlString += '<span id="Box_LastWeekAssignment" class="hide">'; AssignmentBoxString += '<span id="Box_allWorkAssignment"class="hide">'; }

                        HtmlString += '<div class="col-lg-3 col-md-6">';
                        HtmlString += '<div class="graybox">';
                        HtmlString += '<ul>';
                        HtmlString += '<li><span class="left">CodeNo</span><span class="right">' + Data[i].CodeID + '</span></li>';
                        HtmlString += '<li><span class="left">Location</span><span class="right">' + Data[i].LocationName + '</span></li>';
                        HtmlString += '<li><span class="left">Priority</span><span class="right">' + Data[i].PriorityLevelName + '</span></li>';
                        HtmlString += '<li><span class="left">Status</span><span class="right">' + Data[i].WorkRequestStatusName + '</span></li>';
                        HtmlString += '<li><span class="left">Description</span><span class="right">' + Data[i].ProblemDesc + '</span></li>';
                        HtmlString += '<li><span class="left">Project Type</span><span class="right">' + Data[i].WorkRequestProjectTypeName + '</span></li>';
                        HtmlString += '<li><span class="left">Created On</span><span class="right">' + Data[i].CreationDate + '</span></li>';
                        HtmlString += '</ul>';
                        HtmlString += '<div><span class=""><input type="button" value="Cancel" title="Click here to accept the order request." class="btn btn-default bluebutton btn_full" onclick="Dashboard.Client_CancelWorkorder(this)" id="' + Data[i].WorkRequestID + '"/></span></div>';
                        HtmlString += '</div>';
                        HtmlString += '</div>';
                        AssignmentBoxString += '<li><i class="fa fa-angle-double-right"></i><span class="">' + Data[i].ProblemDesc + " " + Data[i].WorkRequestProjectTypeName + '</span> <span class=""> on ' + Data[i].CreationDate + '</span></li>';
                        if (i == parseInt(parseInt(Data.length) - 1)) {
                            AssignmentBoxString += '</span>';
                            HtmlString += '</span>';
                        }
                    }
                    $("#Box_DisplayAllWorkAssignment").html(AssignmentBoxString);
                    $("#WorkorderCreatedByClient").html(HtmlString);
                    $("#WorkorderCreatedByClient").css("display", "block")
                }
                else {
                    $("#WorkorderCreatedByClient").hide();
                }

            },
            error: function () {

            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    },
    "Client_CancelWorkorder": function (data) {
        bootbox.confirm("Are you sure ! you want to cancel the request?", function (result) {
            if (result == true) {
                if (data !== "") {
                    var res = data.id;

                    $.ajax({
                        type: "POST",
                        url: $_HostPrefix + '/Client/CancelWorkRequestCreatedByClient',
                        data: { "Id": res },
                        beforeSend: function () {
                            new fn_showMaskloader('Please wait...');
                        },
                        success: function (Data) {
                            bootbox.alert(Data);
                            Dashboard.GetAllWorkRequestCreatedByClientToEmployee();

                        },
                        error: function () {

                        },
                        complete: function () {
                            fn_hideMaskloader();
                        }
                    });
                }
            }
        });
    },
    "Client_PendingWorkOrder": function (data, divid) {
        var oper = "";
        var _search = "";
        var txtSearch = "";
        var rows = 100000;
        var page = 1;
        var sidx = "WorkRequestAssignmentID";
        var sord = "desc";
        var UserID = "";
        var RequestType = "";
        var filter = data;
        var operation = "PendingAssignWorkrequestCreatedByClient";
        $.ajax({
            type: "POST",
            url: $_HostPrefix + '/Common/GetWorkOrderCreatedByClient?sord=' + sord + '&PageNo=' + page + '&NoOfRecords=' + rows + '&txtSearch=' + txtSearch + '&_search=' + _search + '&operation=' + operation + '&sidx=' + sidx + '&filter=' + filter,
            data: '',
            //beforeSend: function () {
            //    new fn_showMaskloader('Please wait...');
            //},
            async: true,
            success: function (Data) {
                if (Data.length > 0) {
                    var HtmlString = "";
                    var AssignmentBoxString = "";
                    for (i = 0; (i < Data.length) ; i++) {
                        if (i == 4) { HtmlString += '<span id="Box_LastWeekAssignment" class="hide">'; AssignmentBoxString += '<span id="Box_allWorkAssignment"class="hide">'; }

                        HtmlString += '<div class="col-lg-3 col-md-6">';
                        HtmlString += '<div class="graybox graybox-height-clientpending">';
                        HtmlString += '<ul>';
                        HtmlString += '<li><span class="left">CodeNo</span><span class="right">' + Data[i].CodeID + '</span></li>';
                        HtmlString += '<li><span class="left">Location</span><span class="right">' + Data[i].LocationName + '</span></li>';
                        HtmlString += '<li><span class="left">Priority</span><span class="right">' + Data[i].PriorityLevelName + '</span></li>';
                        HtmlString += '<li><span class="left">Status</span><span class="right">' + Data[i].WorkRequestStatusName + '</span></li>';
                        HtmlString += '<li><span class="left">Description</span><span class="right">' + Data[i].ProblemDesc + '</span></li>';
                        HtmlString += '<li><span class="left">Project Type</span><span class="right">' + Data[i].WorkRequestProjectTypeName + '</span></li>';
                        HtmlString += '<li><span class="left">Assigned To</span><span class="right">' + Data[i].AssignToUserName + '</span></li>';
                        HtmlString += '<li><span class="left">Created on</span><span class="right">' + Data[i].CreationDate + '</span></li>';
                        var AssignTime = Data[i].AssignedTime;
                        var StartTime = Data[i].StartTime;
                        var Progress = "";
                        HtmlString += '</ul>';
                        HtmlString += '</div>';
                        HtmlString += '</div>';
                        AssignmentBoxString += '<li><i class="fa fa-angle-double-right"></i><span class="">' + Data[i].ProblemDesc + " " + Data[i].WorkRequestProjectTypeName + '</span> <span class=""> on ' + Data[i].CreationDate + '</span></li>';
                        if (i == parseInt(parseInt(Data.length) - 1)) {
                            AssignmentBoxString += '</span>';
                            HtmlString += '</span>';
                        }
                    }
                    $("#Box_DisplayAllWorkAssignment").html(AssignmentBoxString);
                    $("#" + divid).html(HtmlString);
                    $("#" + divid).css("display", "block")
                }
                else {
                    $("#" + divid).hide();
                }

            },
            error: function () {

            },
            complete: function () {

                fn_hideMaskloader();
            }
        });
    },
    "Client_InProgressWorkOrder": function (data, divid) {
        var oper = "";
        var _search = "";
        var txtSearch = "";
        var rows = 100000;
        var page = 1;
        var sidx = "WorkRequestAssignmentID";
        var sord = "desc";
        var UserID = "";
        var RequestType = "";
        var filter = data;
        var operation = "PendingAssignWorkrequestCreatedByClient"; 
        $.ajax({
            type: "POST",
            url: $_HostPrefix + '/Common/GetWorkOrderCreatedByClient?sord=' + sord + '&PageNo=' + page + '&NoOfRecords=' + rows + '&txtSearch=' + txtSearch + '&_search=' + _search + '&operation=' + operation + '&sidx=' + sidx + '&filter=' + filter,
            data: '',
            //beforeSend: function () {
            //    new fn_showMaskloader('Please wait...');
            //},
            async: true,
            success: function (Data) {
                if (Data.length > 0) {
                    var HtmlString = "";
                    var AssignmentBoxString = "";
                    for (i = 0; (i < Data.length) ; i++) {
                        if (i == 4) { HtmlString += '<span id="Box_LastWeekAssignment" class="hide">'; AssignmentBoxString += '<span id="Box_allWorkAssignment"class="hide">'; }

                        HtmlString += '<div class="col-lg-3 col-md-6">';
                        HtmlString += '<div class="graybox graybox-height">';
                        HtmlString += '<ul>';
                        HtmlString += '<li><span class="left">CodeNo</span><span class="right">' + Data[i].CodeID + '</span></li>';
                        HtmlString += '<li><span class="left">Location</span><span class="right">' + Data[i].LocationName + '</span></li>';
                        HtmlString += '<li><span class="left">Priority</span><span class="right">' + Data[i].PriorityLevelName + '</span></li>';
                        HtmlString += '<li><span class="left">Status</span><span class="right">' + Data[i].WorkRequestStatusName + '</span></li>';
                        HtmlString += '<li><span class="left">Description</span><span class="right">' + Data[i].ProblemDesc + '</span></li>';
                        HtmlString += '<li><span class="left">Project Type</span><span class="right">' + Data[i].WorkRequestProjectTypeName + '</span></li>';
                        HtmlString += '<li><span class="left">Assigned To</span><span class="right">' + Data[i].AssignToUserName + '</span></li>';
                        HtmlString += '<li><span class="left">Created on</span><span class="right">' + Data[i].CreationDate + '</span></li>';

                        var AssignTime = Data[i].AssignedTime;
                        var StartTime = Data[i].StartTime;
                        var Progress = "";
                        if (AssignTime == "" || AssignTime == null) {
                            HtmlString += '<li><span class="left">Progress</span><span class="jTimer2" style="display: none">' + Data[i].EndTime + '</span>';
                        } else {
                            var ass_tim = new Date(AssignTime);
                            var stat_tim = new Date(StartTime);
                            var seconds = ass_tim.getSeconds();
                            var minutes = ass_tim.getMinutes();
                            var hours = ass_tim.getHours()
                            var EndTime = new Date(AssignTime);
                            EndTime.setSeconds((seconds + (minutes * 60) + (hours * 60 * 60)));
                            //PendingBarHtmlString += '<li><span class="jTimer2" style="display: none">' + stat_tim.toLocaleDateString() + " " + EndTime.toTimeString().split(" ")[0] + '</span>';
                            HtmlString += '<li><span class="left">Progress</span><span class="jTimer2 left" style="display: none">' + Data[i].EndTime + '</span>';
                            Progress += '<div class="progressbar" data-perc="100"><div class="bar"><span></span></div><div class="label"><span></span></div></div>';
                        }
                        //PendingBarHtmlString += '<li><span class="jTimer2" style="display: none">' + EndTime + '</span>';
                        HtmlString += '<span class="jTimer1" style="display: none">' + StartTime + '</span>';
                        HtmlString += '<span class="jTimer "></span>';
                        HtmlString += Progress;
                        HtmlString += '</li></ul>';

                        HtmlString += '</div>';
                        HtmlString += '</div>';
                        AssignmentBoxString += '<li><i class="fa fa-angle-double-right"></i><span class="">' + Data[i].ProblemDesc + " " + Data[i].WorkRequestProjectTypeName + '</span> <span class=""> on ' + Data[i].CreationDate + '</span></li>';
                        if (i == parseInt(parseInt(Data.length) - 1)) {
                            AssignmentBoxString += '</span>';
                            HtmlString += '</span>';
                        }
                    }
                    $("#Box_DisplayAllWorkAssignment").html(AssignmentBoxString);
                    $("#" + divid).html(HtmlString);
                    $("#" + divid).css("display", "block")
                }
                else {
                    $("#" + divid).hide();
                }

            },
            error: function () {

            },
            complete: function () {
                setInterval(function () {
                    myTimer2();
                }, 8000);
                fn_hideMaskloader();
            }
        });
    },
    "Client_GetAllVendor": function (data, divid) {
        var oper = "";
        var _search = "";
        var txtSearch = "";
        var rows = 100000;
        var page = 1;
        var sortColumnName = "CreatedDate";
        var sord = "desc";
        var UserID = "";
        var RequestType = "";
        var filter = data;
        var operation = "PendingAssignWorkrequestCreatedByClient";
        $.ajax({
            type: "POST",
            url: $_HostPrefix + '/Manager/GetAllVendorList?sortOrderBy=' + sord + '&pageIndex=' + page + '&TotalRows=' + rows + '&textSearch=' + txtSearch + '&sortColumnName=' + sortColumnName + '&Statusfilter=' + filter,
            data: '',
            //beforeSend: function () {
            //    new fn_showMaskloader('Please wait...');
            //},
            async: true,
            success: function (Data) {
                if (Data.length > 0) {
                    var HtmlString = "";
                    //HtmlString += '<li class="listStyle"><i class="fa fa-angle-double-space" style="padding-right:10px;"></i><span class="left">CompanyName</span> <span class="right">BusinessNo</span></li>';
                    var AssignmentBoxString = "";
        
                    for (i = 0; (i < Data.length) ; i++) {
                        //if (i == 4) { HtmlString += '<span id="Box_LastWeekAssignment" class="hide">'; AssignmentBoxString += '<span id="Box_allWorkAssignment"class="hide">'; }

                        //HtmlString += '<div class="col-lg-3 col-md-6">';
                        //HtmlString += '<div class="graybox">';
                        //HtmlString += '<ul>';
                        //HtmlString += '<li><span class="left">CompanyName</span><span class="right">' + Data[i].CompanyName + '</span></li>';
                        //HtmlString += '<li><span class="left">Priority Level</span><span class="right">' + Data[i].PriorityLevelName + '</span></li>';
                        //HtmlString += '<li><span class="left">WorkRequest Status</span><span class="right">' + Data[i].WorkRequestStatusName + '</span></li>';
                        //HtmlString += '<li><span class="left">Description</span><span class="right">' + Data[i].ProblemDesc + '</span></li>';
                        //HtmlString += '<li><span class="left">Project Type</span><span class="right">' + Data[i].WorkRequestProjectTypeName + '</span></li>';
                        //HtmlString += '<li><span class="left">Assigned To</span><span class="right">' + Data[i].AssignToUserName + '</span></li>';
                        //HtmlString += '<li><span class="left">Created on</span><span class="right">' + Data[i].CreationDate + '</span></li>'; 
                        //HtmlString += '</ul>';
                        //HtmlString += '</div>';
                        //HtmlString += '</div>';
                        //AssignmentBoxString += '<li><i class="fa fa-angle-double-right"></i><span class="">' + Data[i].ProblemDesc + " " + Data[i].WorkRequestProjectTypeName + '</span> <span class=""> on ' + Data[i].CreationDate + '</span></li>';
                        //if (i == parseInt(parseInt(Data.length) - 1)) {
                        //    AssignmentBoxString += '</span>';
                        //    HtmlString += '</span>';
                        //}
                        //HtmlString += '<li class="listStyle"><i class="fa fa-angle-double-right" style="padding-right:10px;"></i><span class="left">' + Data[i].CompanyName + '</span> <span class="right">' + Data[i].BusinessNo + '</span></li>';
                        HtmlString += '<tr><td>' + parseInt(parseInt(i) + 1) + '</td><td>' + Data[i].CompanyName + '</td><td>' + Data[i].BusinessNo + '</td><td>' + Data[i].UserEmail + '</td><?tr>';
                    }
                    $("#PendingVerificationtable").html(HtmlString);
                    $("#" + divid).html(HtmlString);
                    $("#" + divid).css("display", "block")
                }
                else {
                    $("#" + divid).hide();
                }

            },
            error: function () {

            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    },
    "Client_GetVendorTotalCounts": function (data) {
        $.ajax({
            type: "POST",
            url: $_HostPrefix + '/Client/GetVendorTotalCount',
            data: '',
            //beforeSend: function () {
            //    new fn_showMaskloader('Please wait...');
            //},
            async: true,
            success: function (Data) {
                var Res = [];
                var statusDetail = "";
                if (Data.length > 0) {
                    for (i = 0; i < Data.length; i++) {
                        var inne = {};
                        inne.label = Data[i].CodeName;
                        inne.value = Data[i].Column1;
                        Res.push(inne);
                        statusDetail = statusDetail + "<li><i class=''></i>" + Data[i].CodeName + "  (" + Data[i].Column1 + ")</li>";

                    }
                }
                $("#morris-donut-chart").html("");
                $("#TotalStatus").html(statusDetail);
                Morris.Donut({
                    element: 'morris-donut-chart',
                    data: Res
                });
                //Morris.Donut({
                //    element: 'morris-donut-chart',
                //    data: [{
                //        label: "Download Sales",
                //        value: 12
                //    }, {
                //        label: "In-Store Sales",
                //        value: 30
                //    }, {
                //        label: "Mail-Order Sales",
                //        value: 20
                //    }],
                //    resize: true
                //});

            },
            error: function () {

            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    },
    //************************END**************************//
    "ShowProgressBar": function () {
        $('.progressbar').each(function () {
            var t = $(this),
                dataperc = t.attr('data-perc'),
                barperc = t.css("width");
            barperc = parseInt(parseInt(barperc) + 40);
            // barperc = Math.round(dataperc * 5.56);
            //var daat = parseInt(parseInt(barperc * parseInt(dataperc) )/ 100);
            var daat = dataperc;
            t.find('.bar').animate({ width: daat + "%" });
            // t.find('.bar').animate({ width: barperc }, dataperc * 10);
            t.find('.label .perc').remove();
            t.find('.label').append('<div class="perc"></div>');

            function perc() {
                var length = t.find('.bar').css('width'),
                    perc = Math.round(parseInt(length) / barperc * 100),
                    labelpos = (parseInt(length) - 2);
                t.find('.label').css('left', labelpos);
                t.find('.perc').text(perc + '%');
            }
            perc();
            setInterval(perc, 0);
        });
    },
}

$(document).ready(function () {
    $("#ViewAll_pendingTbl").click(function () {
        $("#More_PendingAssignGrid").toggleClass("hide");
        //var dsl = $("#More_PendingAssignGrid").parents("div")[0];
        //dsl.removeAttribute("style");
    });
    $("#ViewAll_LastWeekHistoryTbl").click(function () {
        //$("#Box_LastWeekAssignment").toggleClass("hide");
        $("#Box_AssignedAssignment").toggleClass("hide");
        //var dsl = $("#More_PendingAssignGrid").parents("div")[0];
        //dsl.removeAttribute("style");
    });

    $("#ViewAll_AllWorkTbl").click(function () {
        $("#Box_allWorkAssignment").toggleClass("hide");
    });
    $("#ViewAll_AssignedAssignTbl").click(function () {
        $("#Box_AssignedAssignment").toggleClass("hide");
    });
    $("#ViewAll_PendingAssignTbl").click(function () {
        $("#Box_PendingAssignment").toggleClass("hide");
    });

})

//Description :- Calculation and NaN issu with comments
    function myTimer2() {
        var TimerS = 1000;
        var DateCurrentTime = new Date();
        TimerS = parseInt(TimerS) + 0;

        $('.jTimer1').each(function () {
            // Dashboard.ShowProgressBar();
  
            var woDetail = $(this);
            var startTime = $(this).html();//Start Time
            var endTime = $(this).prev().html();//End Time           
            if (endTime == undefined || endTime == "" || endTime == null || endTime == '')
            {
                $(woDetail).next().css("color", "Green");
                $(woDetail).next().html('No Limit');
                $(woDetail).parents(".graybox").find(".progressbar").hide();
            }
            else
            {
                var spmmm = $(this).next();//Dummy Field
                var startdate = new Date(startTime).getTime();//Start Time convert to second
                var current_time = moment().tz("America/Mexico_City").format('MM/DD/YYYY HH:mm:ss');//This is for according to 108 timezone date
                //var current_time = moment().tz("US/Eastern").format('MM/DD/YYYY HH:mm:ss');//This is for according to client's timezone date
                var current_date = new Date(current_time).getTime();//Current Time convert to second
                var target_date = new Date(endTime).getTime();//End Time convert to second
                if (isNaN(target_date)) {
                    var target_date = 0;
                }
                current_date = current_date + TimerS;
                var seconds_Passed = (current_date -     startdate) / 1000;   //How much time passed from start time with current time
                var seconds_left = (target_date - current_date) / 1000; //How much time away from end time with current time
                // var ProgPercent = (seconds_Passed + seconds_left )/ (100*10);
                var ProgPercent = seconds_Passed / (seconds_Passed + seconds_left) * 100; 
                // var seconds_left  = current_date + TimerS;
                //Passed Date
                Pdays = parseInt(seconds_Passed / 86400);
                seconds_Passed = seconds_Passed % 86400;

                Phours = parseInt(seconds_Passed / 3600);
                seconds_Passed = seconds_Passed % 3600;

                Pminutes = parseInt(seconds_Passed / 60);
                Pseconds = parseInt(seconds_Passed % 60);

                // do some time calculations
                days = parseInt(seconds_left / 86400);
                seconds_left = seconds_left % 86400;

                hours = parseInt(seconds_left / 3600);
                seconds_left = seconds_left % 3600;

                minutes = parseInt(seconds_left / 60);
                seconds = parseInt(seconds_left % 60);

                var pauseStatus = $(woDetail).parents(".graybox").find(".pauseStatus").html();
                if (current_date > target_date)
                {
                    if (target_date == 0)
                    {
                        $(woDetail).next().css('color', 'green');
                        $(woDetail).next().html('No Limit');
                        $(woDetail).parent('td').next('.tdTimer').find(".tdbtnclass").show();
                        $(woDetail).parents(".tdTimer").find(".tdbtnclass").css("display", "none");
                        $(woDetail).parents(".graybox").find(".progressbar").hide();
                    }
                    else
                    {
                           // Dashboard.PendingAssignmnet();
                            $(woDetail).next().css('color', 'red');
                            $(woDetail).next().html('Over Limit');
                            $(woDetail).parent('td').next('.tdTimer').find(".tdbtnclass").show();
                            $(woDetail).parents(".tdTimer").find(".tdbtnclass").css("display", "none");
                            $(woDetail).parents(".graybox").find(".progressbar").hide();                                            
                    }
                }
                else if (hours == 0 && minutes == 0 && seconds == 0 && days == 0)
                {
                    $(woDetail).next().css('color', 'red');
                    // $(woDetail).next().html(days + "D " + hours + "hr " + minutes + "m " + seconds + "s");
                    $(woDetail).next().attr("data", days + "D " + hours + "hr " + minutes + "m " + seconds + "s");
                    //$(woDetail).next().html(Pdays + "D " + Phours + "hr " + Pminutes + "m " + Pseconds + "s");
                    $(woDetail).parent('td').next('.tdTimer').find(".tdbtnclass").show();
                    $(woDetail).parents(".tdTimer").find(".tdbtnclass").css("display", "none");
                    $(woDetail).parents(".graybox").find(".progressbar").show();
                }              
                else
                {
                    $(woDetail).next().html(Pdays + "D " + Phours + "hr " + Pminutes + "m " + Pseconds + "s");
                    $(woDetail).parents(".graybox").find(".progressbar").show();
                    //$(woDetail).next().html(days + "D " + hours + "hr " + minutes + "m " + seconds + "s");
                }
                
                if (ProgPercent > 100)
                {
                    var ExtraTime = parseInt(ProgPercent) - 100;
                    ProgPercent = 100;
                    $(woDetail).siblings("div").find('.bar').removeClass("[class^='color']");
                    $(woDetail).siblings("div").find('.bar').addClass("color3"); //Color3 is Indicating Red
                }
                if (ProgPercent > 80 && ProgPercent < 99)
                {
                    $(woDetail).siblings("div").find('.bar').removeClass("[class^='color']");
                    $(woDetail).siblings("div").find('.bar').addClass("color2"); //color2 is indicating Yellow
                }
                //Added By Bhushan Dod on 29/10/2015 for Progress shown in minus
                if (ProgPercent < 0 && endTime != null)
                {
                    $(woDetail).next().css('color', 'red');
                    $(woDetail).next().html('Over Limit');
                    $(woDetail).parent('td').next('.tdTimer').find(".tdbtnclass").show();
                    $(woDetail).parents(".tdTimer").find(".tdbtnclass").css("display", "none");
                    $(woDetail).parents(".graybox").find(".progressbar").hide();
                }
                if (pauseStatus == "329" || parseInt(pauseStatus) == parseInt(329))
                {
                    $(woDetail).next().css('color', 'green');
                    $(woDetail).next().html('Pause');
                    $(woDetail).parent('td').next('.tdTimer').find(".tdbtnclass").show();
                    $(woDetail).parents(".tdTimer").find(".tdbtnclass").css("display", "none");
                    $(woDetail).parents(".graybox").find(".progressbar").hide();
                }
                $(woDetail).siblings("div").attr('data-perc', ProgPercent);
                var t = $(woDetail).siblings("div"),//.find(".progressbar"),

                  dataperc = t.attr('data-perc'),
                  barperc = t.css("width");
                barperc = parseInt(parseInt(barperc) + 40);
                // barperc = Math.round(dataperc * 5.56);
                //var daat = parseInt(parseInt(barperc * parseInt(dataperc) )/ 100);
                var daat = dataperc;
                t.find('.bar').animate({ width: daat + "%" });
                // t.find('.bar').animate({ width: barperc }, dataperc * 10 );
                t.find('.label .perc').remove();
                t.find('.label').append('<div class="perc"></div>');

                function perc() {
                    var length = t.find('.bar').css('width'),
                        //perc = Math.round(parseInt(length) / barperc * 100),
                        perc = parseInt(dataperc);

                    labelpos = (parseInt(length) - 2);
                    t.find('.label').css('left', parseInt(labelpos - 20));
                    t.find('.perc').text(perc + '%');
                }
                perc();
                setInterval(perc, 0);
            }
        });
    }
    //*********************END**********************//
