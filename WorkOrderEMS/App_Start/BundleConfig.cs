using System.Web.Optimization;

namespace WorkOrderEMS
{

    public class BundleConfig
    {
        
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
           // string eTracScriptVersion = System.Configuration.ConfigurationManager.AppSettings["eTracScriptVersion"];

            if (System.Diagnostics.Debugger.IsAttached)
            {
                BundleTable.EnableOptimizations = false;
            }
            else
            {
                BundleTable.EnableOptimizations = false;
            }
            //BundleTable.EnableOptimizations = true;
            bundles.Add(new ScriptBundle("~/Scripts/jQueryFile").Include(
            "~/Scripts/jquery-3.2.1.min.js"
           ));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/css2").Include(
                      "~/Content/common/admin.css",
                "~/Content/font-awesome/css/font-awesome.css",
                "~/Content/common/plugins/morris/morris-0.4.3.min.css",
                "~/Content/themes/base/jquery.ui.theme.css",
               "~/Content/themes/base/jquery.ui.core.css",
                "~/Content/themes/base/jquery.ui.all.css",
                "~/Content/JqGridCSS/ui.jqgrid.css",
                "~/Content/JqGridCSS/DilogPopup.css",
                "~/Content/themes/base/jquery.ui.datepicker.css",
               "~/Scripts/BootStrap-Multiselect/CSS/bootstrap-multiselect.css",
               "~/Content/SuperAdmin/sb-admin.css",
               "~/Content/common/plugins/simple-sidebar.css",
                "~/Content/toastr.css"
                ));

            bundles.Add(new ScriptBundle("~/Content/script2").Include(
                        "~/Scripts/jquery-1.10.2.min.js"
                        , "~/Scripts/1.9.1-js/jquery-ui-1.9.2.min.js"
                        , "~/Scripts/common/sb-admin.js"
                        , "~/Scripts/common/plugins/morris/raphael-2.1.0.min.js"
                        , "~/Scripts/common/plugins/metisMenu/jquery.metisMenu.js"
                        , "~/Scripts/jquery.ui.core.js"
                        , "~/Scripts/JqGrid/jquery.js"
                        , "~/Scripts/JqGrid/grid.common.js"
                        , "~/Scripts/JqGrid/jquery.jqGrid.min.js"
                        , "~/Scripts/JqGrid/i18n/grid.locale-en.js"
                        , "~/Scripts/FileUpload/jquery.fileupload.js"
                        , "~/Scripts/FileUpload/jquery.fileupload-ui.js"
                        , "~/Scripts/FileUpload/jquery.iframe-transport.js"
                        , "~/Scripts/jquery.validate.js"
                        , "~/Scripts/jquery.validate.unobtrusive.js"
                        , "~/Scripts/jquery.unobtrusive-ajax.min.js"
                        , "~/Scripts/jquery.maskedinput-1.3.min.js"
                        , "~/Content/common/plugins/Loader/maskLoader.js"
                        , "~/Scripts/JqGrid/DilogPopUP.js"
                        , "~/Scripts/jquery.ui.datepicker.js"
                        , "~/Scripts/toastr.js"

                ));
            bundles.Add(new ScriptBundle("~/Content/JQ").Include(
                        "~/Scripts/jquery-1.10.2.min.js",
                        "~/Scripts/JqGrid/jquery.js",
                        "~/Scripts/JqGrid/grid.common.js",
                        "~/Scripts/JqGrid/jquery.jqGrid.min.js",
                        "~/Scripts/JqGrid/i18n/grid.locale-en.js",
                        "~/Scripts/JqGrid/DilogPopUP.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/Scripts/jquery.ui.datepicker.js",
                        "~/Scripts/jquery.ui.core.js",
                        "~/Scripts/1.9.1-js/jquery-ui-1.9.2.min.js",
                        "~/Scripts/FileUpload/jquery.fileupload.js",
                        "~/Scripts/FileUpload/jquery.fileupload-ui.js",
                        "~/Scripts/FileUpload/jquery.iframe-transport.js",
                        "~/Scripts/jquery.maskedinput-1.3.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/common/plugins/morris/raphael-2.1.0.min.js",
                        "~/Scripts/common/sb-admin.js",
                        "~/Scripts/jquery.ui.datepicker.js",
                        "~/Scripts/jquery.ui.core.js",
                        "~/Content/common/plugins/Loader/maskLoader.js",
                        "~/Scripts/BootStrap-Multiselect/JS/bootstrap-multiselect.js",
                        "~/Scripts/BootStrap-Multiselect/JS/bootstrap-tooltip.js",
                        "~/Scripts/BootStrap-Multiselect/JS/bootstrap-confirmation.js",
                        "~/Scripts/BootStrap-Multiselect/JS/bootbox.js",
                        "~/Scripts/common/plugins/metisMenu/jquery.metisMenu.js",
                        "~/Scripts/common/plugins/morris/raphael-2.1.0.min.js"
              ));
            //Commented by Bhushan Dod on 22/09/2016 due to not in used in application.
            // bundles.Add(new StyleBundle("~/Scripts/MobiTimePickerCss").Include(
            // "~/Scripts/MobiTimePicker/css/mobiscroll.animation.css",
            // "~/Scripts/MobiTimePicker/css/mobiscroll.icons.css",
            // "~/Scripts/MobiTimePicker/css/mobiscroll.frame.css",
            // "~/Scripts/MobiTimePicker/css/mobiscroll.frame.android.css",
            // "~/Scripts/MobiTimePicker/css/mobiscroll.frame.android-holo.css",
            // "~/Scripts/MobiTimePicker/css/mobiscroll.frame.ios-classic.css",
            // "~/Scripts/MobiTimePicker/css/mobiscroll.frame.ios.css",
            // "~/Scripts/MobiTimePicker/css/mobiscroll.frame.jqm.css",
            // "~/Scripts/MobiTimePicker/css/mobiscroll.frame.sense-ui.css",
            // "~/Scripts/MobiTimePicker/css/mobiscroll.frame.wp.css",
            // "~/Scripts/MobiTimePicker/css/mobiscroll.scroller.css",
            // "~/Scripts/MobiTimePicker/css/mobiscroll.scroller.android.css",
            // "~/Scripts/MobiTimePicker/css/mobiscroll.scroller.android-holo.css",
            // "~/Scripts/MobiTimePicker/css/mobiscroll.scroller.ios-classic.css", 
            // "~/Scripts/MobiTimePicker/css/mobiscroll.scroller.ios.css",
            // "~/Scripts/MobiTimePicker/css/mobiscroll.scroller.jqm.css",
            // "~/Scripts/MobiTimePicker/css/mobiscroll.scroller.sense-ui.css",
            // "~/Scripts/MobiTimePicker/css/mobiscroll.scroller.wp.css",
            // "~/Scripts/MobiTimePicker/css/mobiscroll.image.css",
            // "~/Scripts/MobiTimePicker/css/mobiscroll.android-holo-light.css",
            // "~/Scripts/MobiTimePicker/css/mobiscroll.wp-light.css",
            // "~/Scripts/MobiTimePicker/css/mobiscroll.mobiscroll-dark.css",
            // "~/Scripts/MobiTimePicker/panel panel-default first-mar10"       
            // ));
            // bundles.Add(new ScriptBundle("~/Scripts/MobiTimePickerJS").Include(
            //"~/Scripts/MobiTimePicker/mobiscroll.core.js",
            //"~/Scripts/MobiTimePicker/mobiscroll.frame.js" ,
            //"~/Scripts/MobiTimePicker/mobiscroll.scroller.js",
            //"~/Scripts/MobiTimePicker/mobiscroll.util.datetime.js",
            //"~/Scripts/MobiTimePicker/mobiscroll.datetimebaseEdited.js",
            //"~/Scripts/MobiTimePicker/mobiscroll.datetime.js",
            //"~/Scripts/MobiTimePicker/mobiscroll.select.js",
            //"~/Scripts/MobiTimePicker/mobiscroll.listbase.js",
            //"~/Scripts/MobiTimePicker/mobiscroll.image.js",
            //"~/Scripts/MobiTimePicker/mobiscroll.treelist.js",
            //"~/Scripts/MobiTimePicker/mobiscroll.frame.android.js",
            //"~/Scripts/MobiTimePicker/mobiscroll.frame.android-holo.js",
            //"~/Scripts/MobiTimePicker/mobiscroll.frame.ios-classic.js",
            //"~/Scripts/MobiTimePicker/mobiscroll.frame.ios.js",
            //"~/Scripts/MobiTimePicker/mobiscroll.frame.jqm.js",
            //"~/Scripts/MobiTimePicker/mobiscroll.frame.sense-ui.js",
            //"~/Scripts/MobiTimePicker/mobiscroll.frame.wp.js",
            //"~/Scripts/MobiTimePicker/mobiscroll.android-holo-light.js",
            //"~/Scripts/MobiTimePicker/mobiscroll.wp-light.js",
            //"~/Scripts/MobiTimePicker/mobiscroll.mobiscroll-dark.js",
            //"~/Scripts/MobiTimePicker/i18n/mobiscroll.i18n.en-UK.js"
            //));

            bundles.Add(new StyleBundle("~/Scripts/ReportCss").Include(
            "~/Content/themes/base/Jquery-UIDataTableReport.css",
            "~/Content/dataTables.jqueryuiReport.css",
            "~/Content/Report-NavBar/daterangepicker-bs3.css",
            "~/Content/Report-NavBar/bootstrap-clockpicker.css",
            "~/Scripts/Report/StyleReport.css"
           ));
            bundles.Add(new ScriptBundle("~/Scripts/ReportJS").Include(
            "~/Content/Report-NavBar/moment.js",
            "~/Content/Report-NavBar/daterangepicker.js",
            "~/Content/Report-NavBar/bootstrap-clockpicker.js",
                //"~/Scripts/jquery.dataTables.js",
              "~/Scripts/jquery.dataTables.min.js",
            "~/Scripts/Report/highcharts.js",
            "~/Scripts/Report/exporting.js",
            "~/Scripts/Report/Report.js"
                //"~/Scripts/TableExport/tableExport.js",
                //"~/Scripts/TableExport/jquery.base64.js",
                //"~/Scripts/TableExport/html2canvas.js",
                //"~/Scripts/TableExport/jspdf/libs/sprintf.js",
                //"~/Scripts/TableExport/jspdf/jspdf.js",
                //"~/Scripts/TableExport/jspdf/libs/base64.js"
           ));
            bundles.Add(new StyleBundle("~/Scripts/ReportCssDT").Include(
                "~/Content/themes/base/Jquery-UIDataTable.css",
                "~/Content/dataTables.jqueryui.css",
                "~/Content/datepicker.css",
                "~/Scripts/Report/StyleReport.css"
        ));
            bundles.Add(new ScriptBundle("~/Scripts/ReportJsDT").Include(
                "~/Scripts/jquery.dataTables.js",
                "~/Scripts/fnAddDataAndDisplay.js",
                "~/Scripts/bootstrap-datepicker.js",
                "~/Scripts/Report/highcharts.js",
                "~/Scripts/Report/exporting.js",
                "~/Scripts/Report/Report.js",
                "~/Scripts/TableExport/tableExport.js",
                "~/Scripts/TableExport/jquery.base64.js",
                "~/Scripts/TableExport/html2canvas.js",
                "~/Scripts/TableExport/jspdf/libs/sprintf.js",
                "~/Scripts/TableExport/jspdf/jspdf.js",
                "~/Scripts/TableExport/jspdf/libs/base64.js",
                "~/Scripts/Report/highcharts-3d.js"
           ));

            bundles.Add(new StyleBundle("~/Scripts/ClockPickerCss").Include(
                                    "~/Content/Report-NavBar/daterangepicker-bs3.css",
                                    "~/Content/Report-NavBar/bootstrap-clockpicker.css"
                        ));
            bundles.Add(new ScriptBundle("~/Scripts/ClockPickerJs").Include(
                                   "~/Content/Report-NavBar/moment.js",
                                   "~/Content/Report-NavBar/daterangepicker.js",
                                   "~/Content/Report-NavBar/bootstrap-clockpicker.js"
                       ));

            bundles.Add(new StyleBundle("~/Scripts/ReportCssDTeMaintenance").Include(
                                   "~/Content/themes/base/Jquery-UIDataTable.css",
                                   "~/Content/dataTables.jqueryui.css",
                                   "~/Content/Report-NavBar/daterangepicker-bs3.css",
                                   "~/Content/Report-NavBar/bootstrap-clockpicker.css",
                                   "~/Scripts/Report/StyleReport.css"
                       ));
            bundles.Add(new ScriptBundle("~/Scripts/ReportJsDTeMaintenance").Include(
                                   "~/Content/Report-NavBar/moment.js",
                                   "~/Content/Report-NavBar/daterangepicker.js",
                                   "~/Content/Report-NavBar/bootstrap-clockpicker.js",
                                   "~/Scripts/jquery.dataTables.js",
                                   "~/Scripts/Report/highcharts.js",
                                   "~/Scripts/Report/exporting.js",
                                   "~/Scripts/Report/Report.js",
                                   "~/Scripts/TableExport/tableExportEdit.js",
                                   "~/Scripts/TableExport/jquery.base64.js",
                                   "~/Scripts/TableExport/html2canvas.js",
                                   "~/Scripts/TableExport/jspdf/libs/sprintf.js",
                                   "~/Scripts/TableExport/jspdf/jspdf.js",
                                   "~/Scripts/TableExport/jspdf/libs/base64.js",
                                  "~/Scripts/Report/highcharts-3d.js"
                       ));
            bundles.Add(new ScriptBundle("~/Scripts/ReportJsDTExporting").Include(
                                   "~/Content/Dashboard/ExportDataTable/dataTables.buttons.js",
                                   "~/Content/Dashboard/ExportDataTable/jszip.js",
                                   "~/Content/Dashboard/ExportDataTable/pdfmake.js",
                                   "~/Content/Dashboard/ExportDataTable/vfs_fonts.js",
                                   "~/Content/Dashboard/ExportDataTable/buttons.html5.js"
                       ));

            //Ashwajit Bansod .... bundles added for all files which are same.....
            //bundles for jquery ui//
            bundles.Add(new StyleBundle("~/Content/jquery-ui").Include(
               "~/Content/themes/base/jquery.ui.theme.css",
               "~/Content/themes/base/jquery.ui.core.css",
               "~/Content/themes/base/jquery.ui.all.css"
       ));

            bundles.Add(new StyleBundle("~/Content/JQGrid-Datepicker").Include(
               
               "~/Content/JqGridCSS/ui.jqgrid.css",
               "~/Content/JqGridCSS/DilogPopup.css",
               "~/Content/datepicker.css"
       ));

            bundles.Add(new StyleBundle("~/Content/Wizard-Sb-Admin").Include(
                   "~/Content/formwizard.css",
                   "~/Content/SuperAdmin/sb-admin.css"
       ));

            // < !--AdminLTE Skins.Choose a skin from the css/ skins
            bundles.Add(new StyleBundle("~/Content/AdminLTE-Skins").Include(
              "~/Content/Dashboard/dist/css/skins/_all-skins.min.css",
              "~/Content/Dashboard/dist/css/component.css",
              "~/Content/Dashboard/dist/css/flipcss.css",
               "~/Content/Dashboard/bootstrap/css/style.css"
      ));

            //bundles for font awesome//
            bundles.Add(new StyleBundle("~/Content/Font-Awesome").Include(
              "~/Content/font-awesome/css/font-awesome.css",
              "~/Content/Dashboard/bootstrap/css/font-awesome.min.css"
      ));

            bundles.Add(new StyleBundle("~/Content/DateTimepicker").Include(

              "~/Content/Dashboard/plugins/daterangepicker/daterangepicker-bs3.css",
              "~/Content/Dashboard/plugins/colorpicker/bootstrap-colorpicker.min.css",
              "~/Content/Dashboard/plugins/timepicker/bootstrap-timepicker.min.css"
      ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            //Scripts for JqGrid
            bundles.Add(new ScriptBundle("~/Scripts/JqGrid").Include(
                                   "~/Scripts/JqGrid/jquery.js",
                                   "~/Scripts/JqGrid/grid.common.js",
                                   "~/Scripts/JqGrid/jquery.jqGrid.min.js",
                                   "~/Scripts/JqGrid/i18n/grid.locale-en.js",
                                   "~/Scripts/JqGrid/DilogPopUP.js"
                       ));

            //Jquery File Upload
            bundles.Add(new ScriptBundle("~/Scripts/JqueryFileUpload").Include(
                                  "~/Scripts/FileUpload/jquery.fileupload.js",
                                  "~/Scripts/FileUpload/jquery.fileupload-ui.js",
                                  "~/Scripts/FileUpload/jquery.iframe-transport.js"
                      ));

            //moment files
            bundles.Add(new ScriptBundle("~/Scripts/Moment").Include(
                                  "~/Scripts/moment.js",
                                  "~/Scripts/moment-timezone-with-data.js"
                      ));

            //DurationFileBox Script
            bundles.Add(new ScriptBundle("~/Scripts/DurationFlipbox").Include(
                                  "~/Scripts/DurationFlipbox/jquery.mousewheel.min.js",
                                  "~/Scripts/DurationFlipbox/jtsage-datebox-4.1.1.bootstrap.min.js"
                      ));

            //Bootstrap-Multiselect
            bundles.Add(new ScriptBundle("~/Scripts/Bootstrap-Multiselect").Include(
                                 "~/Scripts/BootStrap-Multiselect/JS/bootstrap-multiselect.js",
                                 "~/Scripts/BootStrap-Multiselect/JS/bootstrap-confirmation.js",
                                 "~/Scripts/BootStrap-Multiselect/JS/bootbox.js"

                     ));
            //Input-Mask
            bundles.Add(new ScriptBundle("~/Content/input-mask").Include(
                                 "~/Content/Dashboard/plugins/input-mask/jquery.inputmask.js",
                                 "~/Content/Dashboard/plugins/input-mask/jquery.inputmask.date.extensions.js",
                                 "~/Content/Dashboard/plugins/input-mask/jquery.inputmask.extensions.js"
                     ));
            //<!-- jvectormap -->
            bundles.Add(new ScriptBundle("~/Content/jvectormap").Include(
                                 "~/Content/Dashboard/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                                 "~/Content/Dashboard/plugins/jvectormap/jquery-jvectormap-world-mill-en.js"
                     ));

            //< !--USER Mini - chart - list-- >
            bundles.Add(new ScriptBundle("~/Content/MiniChartList").Include(
                                "~/Content/Dashboard/dist/js/jRespond.js",
                                "~/Content/Dashboard/dist/js/jquery_002.js",
                                "~/Content/Dashboard/dist/js/smart-resize.js",
                                "~/Content/Dashboard/dist/js/layout.js"
                    ));

            ////< !--MomentJavascript-- >
            //bundles.Add(new ScriptBundle("~/Scripts/MomentJavascript").Include(
            //                    "~/Scripts/moment.min.js",
            //                    "~/Scripts/moment-timezone-with-data.js",
            //                    "~/Scripts/jstz.min.js",
            //                    "~/Scripts/jquery.cookie.min.js"
            //        ));

            bundles.Add(new ScriptBundle("~/Content/KnobChart-BootstrapDatepicker-wysihtml5").Include(
                                "~/Content/Dashboard/plugins/knob/jquery.knob.js",
                                "~/Content/Dashboard/plugins/datepicker/bootstrap-datepicker.js",
                                "~/Content/Dashboard/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js"
                    ));

            bundles.Add(new ScriptBundle("~/Content/Time-date-oll-icheck-demopurpose").Include(
                                "~/Content/Dashboard/plugins/daterangepicker/daterangepicker.js",
                                "~/Content/Dashboard/plugins/colorpicker/bootstrap-colorpicker.min.js",
                                "~/Content/Dashboard/plugins/timepicker/bootstrap-timepicker.min.js",
                                "~/Content/Dashboard/plugins/slimScroll/jquery.slimscroll.min.js",
                                "~/Content/Dashboard/plugins/iCheck/icheck.min.js",
                                "~/Content/Dashboard/plugins/fastclick/fastclick.min.js",
                                "~/Content/Dashboard/dist/js/app.min.js",
                                "~/Content/Dashboard/dist/js/demo.js",
                                "~/Content/Dashboard/dist/js/flip.js",
                                "~/Content/Dashboard/bootstrap/js/jquery-ui.min.js"


                    ));

            bundles.Add(new ScriptBundle("~/Content/DataTable-Select2").Include(
                                "~/Content/Dashboard/plugins/datatables/jquery.dataTables.min.js",
                                "~/Content/Dashboard/plugins/datatables/dataTables.bootstrap.min.js",
                                "~/Content/Dashboard/plugins/select2/select2.full.min.js"

));       
        }
    }
    public class SiteKeys
    {
        public static string StyleVersion
        {
            get
            {
                return "<link href=\"{0}?v=" + System.Configuration.ConfigurationManager.AppSettings["eTracScriptVersion"] + "\" rel=\"stylesheet\"/>";
            }
        }
        public static string ScriptVersion
        {
            get
            {
                return "<script src=\"{0}?v=" + System.Configuration.ConfigurationManager.AppSettings["eTracScriptVersion"] + "\"></script>";
            }
        }
    }
}
