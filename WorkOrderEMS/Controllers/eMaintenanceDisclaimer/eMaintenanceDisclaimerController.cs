using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models.CommonModels;

namespace WorkOrderEMS.Controllers.eMaintenanceDisclaimer
{
    public class eMaintenanceDisclaimerController : Controller
    {
        //
        // GET: /eMaintenance/
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        private readonly IGlobalAdmin _IGlobalAdmin;
        private readonly IWorkRequestAssignment _IWorkRequestAssignment;
        private string DisclaimerFormPath = ConfigurationManager.AppSettings["DisclaimerFormPath"];
        private string SurveyFormPath = ConfigurationManager.AppSettings["SurveyFormPath"];
        private string DARDisclaimerFormPath = ConfigurationManager.AppSettings["DARDisclaimerFormPath"];
        public eMaintenanceDisclaimerController(IGlobalAdmin _GlobalAdmin, IWorkRequestAssignment _IWorkRequestAssignment)
        {
            this._IGlobalAdmin = _GlobalAdmin;
            this._IWorkRequestAssignment = _IWorkRequestAssignment;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index(string id, string LocationID)
        {
            return View();
        }

        public ActionResult SurveyFeedback(string user, string work, string email)
        {
            ViewBag.userId = user;
            ViewBag.workId = work;
            ViewBag.SurveyEmailID = email;
            return View("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SubmitSurveyFormNew(WorkOrderEMS.Models.eMaintenance_M.eMaintenanceSurvey_M obj)
        {
            WorkRequestManager objWorkRequestManager = new WorkRequestManager();
            bool st;
            try
            {
                if (obj.WorkAssignmentIds != null && obj.UserIds != null && obj.SurveyEmailIDs != null)
                {
                    obj.WorkAssignmentId = Convert.ToInt64(WebUtility.HtmlEncode(Cryptography.GetDecryptedData(obj.WorkAssignmentIds, true)));
                    obj.UserId = Convert.ToInt64(Cryptography.GetDecryptedData(obj.UserIds, true));
                    obj.SurveyEmailID = Cryptography.GetDecryptedData(obj.SurveyEmailIDs, true);
                }
                bool isExist = objWorkRequestManager.IsSurveySubmit(obj.WorkAssignmentId);
                if (isExist == false)
                {
                    var s = _IGlobalAdmin.SubmitSurveyForm(obj);
                    string dataHtml = obj.HtmlData;
                    dataHtml = dataHtml.Replace("\r\n", "").Replace("\n", " ").Replace("\r ", " ");
                    string savedFileName = SurveyHTMLToPdf(dataHtml, obj.WorkAssignmentIds, obj.WorkAssignmentId);
                    st = objWorkRequestManager.WorkFrSignature(obj.WorkAssignmentId, "", "", "", savedFileName, obj.SurveyEmailID);

                    return Json(new { isExist }, JsonRequestBehavior.AllowGet);
                }
                else { return Json(isExist, JsonRequestBehavior.AllowGet); }
                // return null;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public ActionResult DisclaimerDownload(string Id)
        {
            WorkRequestAssignmentModel ObjWorkRequestAssignmentModel;
            try
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    Id = Cryptography.GetDecryptedData(Id, true);
                    ObjWorkRequestAssignmentModel = _IWorkRequestAssignment.GetWorkorderAssignmentByID(Convert.ToInt64(Id));
                    if (!string.IsNullOrEmpty(ObjWorkRequestAssignmentModel.DisclaimerForm))
                    {
                        string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                        string IsFileExist = RootDirectory + DisclaimerFormPath;
                        RootDirectory = RootDirectory + DisclaimerFormPath + ObjWorkRequestAssignmentModel.DisclaimerForm;
                        //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + DisclaimerFormPath + ObjWorkRequestAssignmentModel.DisclaimerForm;
                        if (Directory.GetFiles(IsFileExist, ObjWorkRequestAssignmentModel.DisclaimerForm).Length > 0)
                        {
                            byte[] fileBytes = System.IO.File.ReadAllBytes(RootDirectory);
                            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, ObjWorkRequestAssignmentModel.DisclaimerForm);
                        }
                        else
                        {
                            RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + DisclaimerFormPath + "FileNotFound.png";
                            byte[] fileBytes = System.IO.File.ReadAllBytes(RootDirectory);
                            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "FileNotFound.png");
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else { return Json("Id is Empty!"); }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                return Json(ex.Message);
            }
        }

        public ActionResult SurveyDownload(string Id)
        {
            WorkRequestAssignmentModel ObjWorkRequestAssignmentModel;
            try
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    Id = Cryptography.GetDecryptedData(Id, true);
                    ObjWorkRequestAssignmentModel = _IWorkRequestAssignment.GetWorkorderAssignmentByID(Convert.ToInt64(Id));
                    if (!string.IsNullOrEmpty(ObjWorkRequestAssignmentModel.SurveyForm))
                    {
                        string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                        string IsFileExist = RootDirectory + SurveyFormPath;
                        RootDirectory = RootDirectory + SurveyFormPath + ObjWorkRequestAssignmentModel.SurveyForm;
                        //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + DisclaimerFormPath + ObjWorkRequestAssignmentModel.DisclaimerForm;
                        if (Directory.GetFiles(IsFileExist, ObjWorkRequestAssignmentModel.SurveyForm).Length > 0)
                        {
                            byte[] fileBytes = System.IO.File.ReadAllBytes(RootDirectory);
                            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, ObjWorkRequestAssignmentModel.SurveyForm);
                        }
                        else
                        {
                            RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + DisclaimerFormPath + "FileNotFound.png";
                            byte[] fileBytes = System.IO.File.ReadAllBytes(RootDirectory);
                            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "FileNotFound.png");
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else { return Json("Id is Empty!"); }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                return Json(ex.Message);
            }
        }

        public string HTMLToPdf(string HTML, string FileName)
        {
            FileName = FileName.Replace(' ', '+').Replace('/', '@');//Here @ char replace due to '/' encrypt id it break the URL to open file in HTMLToPDF

            string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"]);
            var fileName = System.Web.HttpContext.Current.Server.MapPath("~/Content/eMaintenance/SurveyDownload/") + FileName + ".pdf";

            //Render PlaceHolder to temporary stream
            System.IO.StringWriter stringWrite = new StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

            StringReader reader = new StringReader(HTML);

            //Create PDF document
            Document doc = new Document(PageSize.A4);
            HTMLWorker parser = new HTMLWorker(doc);
            PdfWriter.GetInstance(doc, new FileStream(fileName,

            FileMode.Create));
            doc.Open();

            /********************************************************************************/
            var interfaceProps = new Dictionary<string, Object>();
            var ih = new ImageHander() { BaseUri = Request.Url.ToString() };

            interfaceProps.Add(HTMLWorker.IMG_PROVIDER, ih);

            foreach (IElement element in HTMLWorker.ParseToList(
            new StringReader(HTML), null))
            {
                doc.Add(element);
            }
            doc.Close();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + FileName);     // to open file prompt Box open or Save file        
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.WriteFile(fileName);
            Response.End();

            return FileName;
        }

        public string SurveyHTMLToPdf(string HTML, string FileName, long WorkAssignmentId)
        {
            GlobalAdminManager objM = new GlobalAdminManager();
            FileName = FileName.Replace(' ', '+').Replace('/', '@');//Here @ char replace due to '/' encrypt id it break the URL to open file in HTMLToPDF
            string waterMarkSurveyFilename = "eTrac" + FileName + ".pdf";
            string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"]);
            var fileName = System.Web.HttpContext.Current.Server.MapPath("~/Content/eMaintenance/SurveyDownload/") + FileName + ".pdf";
            var objData = objM.GetDataForRendringHTML(WorkAssignmentId);

            //----------------------------

            Document doc = new Document(PageSize.A4, 30f, 30f, 40f, 30f);
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.CreateNew));
            doc.Open();
            try
            {
                //var content = writer.DirectContent;
                //var pageBorderRect = new iTextSharp.text.Rectangle(doc.PageSize);
                //pageBorderRect.Left += doc.LeftMargin;
                //pageBorderRect.Right -= doc.RightMargin;
                //pageBorderRect.Top -= doc.TopMargin;
                //pageBorderRect.Bottom += doc.BottomMargin;
                //content.SetColorStroke(BaseColor.BLACK);
                //content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
                //content.Stroke();

                //Logo
                string imageURL = System.Web.HttpContext.Current.Server.MapPath("~/Images/logo-etrac.png");
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                jpg.Alignment = 3;
                jpg.SpacingBefore = 30f;
                jpg.ScaleToFit(80f, 60f);
                jpg.SpacingBefore = 10f;
                jpg.SpacingAfter = 1f;
                doc.Add(jpg);

                Paragraph objFormName = new Paragraph("Customer Assistance Survey Form");
                objFormName.Alignment = 3;
                objFormName.SpacingBefore = 3f;
                objFormName.SpacingAfter = 3f;
                objFormName.IndentationLeft = 3f;
                doc.Add(objFormName);

                StyleSheet css = new StyleSheet();
                css.LoadTagStyle("body", "face", "Times-Roman");
                css.LoadTagStyle("body", "font", "Times");
                css.LoadTagStyle("body", "size", "11pt");
                css.LoadTagStyle("body", "color", "#000");

                foreach (IElement element in HTMLWorker.ParseToList(
                new StringReader(HTML), css))
                {
                    doc.Add(element);
                }
                var font1 = FontFactory.GetFont("Times-Roman", 11, Font.NORMAL, BaseColor.BLACK);

                PdfPTable table = new PdfPTable(2);

                PdfPCell cell = new PdfPCell(new Phrase("Customer Name: " + StringExtensionMethods.ToTitleCase(objData.CustomerName), font1));
                cell.Colspan = 1;
                cell.Border = 0;
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Employee Name: " + StringExtensionMethods.ToTitleCase(objData.AssignedFirstName) + ' ' + StringExtensionMethods.ToTitleCase(objData.AssignedLastName), font1));
                cell.Colspan = 1;
                cell.Border = 0;
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);

                doc.Add(table);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                doc.Close();
                // Watermark code            
                string watermarkLoc = System.Web.HttpContext.Current.Server.MapPath("~/Images/eTrac380-light.png");
                PdfReader pdfReader = new PdfReader(fileName);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(fileName.Replace(FileName + ".pdf", waterMarkSurveyFilename), FileMode.Create));

                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(watermarkLoc);
                var pageSize = pdfReader.GetPageSizeWithRotation(1);

                var x = pageSize.Width / 2 - img.ScaledWidth / 2;
                var y = pageSize.Height / 2 - img.ScaledHeight / 2;
                img.SetAbsolutePosition(x, y);
                //img.ScaleToFit(100f,120f);
                PdfContentByte waterMark;
                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    waterMark = pdfStamper.GetUnderContent(page);
                    waterMark.AddImage(img);
                }
                pdfStamper.FormFlattening = true;
                pdfStamper.Close();
                pdfReader.Close();
                //delete old file. No more need of that file.
                System.IO.File.Delete(fileName);
            }
            ////Render PlaceHolder to temporary stream
            //System.IO.StringWriter stringWrite = new StringWriter();
            //System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

            //StringReader reader = new StringReader(HTML);

            ////Create PDF document
            //Document doc = new Document(PageSize.A4);
            //HTMLWorker parser = new HTMLWorker(doc);
            //PdfWriter.GetInstance(doc, new FileStream(fileName,

            //FileMode.Create));
            //doc.Open();

            ///********************************************************************************/
            //var interfaceProps = new Dictionary<string, Object>();
            //var ih = new ImageHander() { BaseUri = Request.Url.ToString() };

            //interfaceProps.Add(HTMLWorker.IMG_PROVIDER, ih);

            //foreach (IElement element in HTMLWorker.ParseToList(
            //new StringReader(HTML), null))
            //{
            //    doc.Add(element);
            //}
            //doc.Close();
            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=" + FileName);     // to open file prompt Box open or Save file        
            //Response.Charset = "";
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.WriteFile(fileName);
            //Response.End();

            //return FileName; commented bcoz for wtermark we need tosave same file name in db
            return waterMarkSurveyFilename;
        }

        public ActionResult DARDisclaimerDownload(string Id)
        {
            DARManager objDARManager = new DARManager();
            try
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    Id = Cryptography.GetDecryptedData(Id, true);
                    var darDetail  = objDARManager.GetDARDetailsById(Convert.ToInt64(Id));
                    if (!string.IsNullOrEmpty(darDetail.DisclaimerFormFile))
                    {
                        string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                        string IsFileExist = RootDirectory + DARDisclaimerFormPath;
                        RootDirectory = RootDirectory + DARDisclaimerFormPath + darDetail.DisclaimerFormFile;
                        if (Directory.GetFiles(IsFileExist, darDetail.DisclaimerFormFile).Length > 0)
                        {
                            byte[] fileBytes = System.IO.File.ReadAllBytes(RootDirectory);
                            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, darDetail.DisclaimerFormFile);
                        }
                        else
                        {
                            RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + DisclaimerFormPath + "FileNotFound.png";
                            byte[] fileBytes = System.IO.File.ReadAllBytes(RootDirectory);
                            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "FileNotFound.png");
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else { return Json("Id is Empty!"); }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                return Json(ex.Message);
            }
        }

        public class ImageHander : IImageProvider
        {
            public string BaseUri;
            public iTextSharp.text.Image GetImage(string src,
            IDictionary<string, string> h,
            ChainedProperties cprops,
            IDocListener doc)
            {
                string imgPath = string.Empty;

                if (src.ToLower().Contains("http://") == false)
                {
                    imgPath = System.Web.HttpContext.Current.Request.Url.Scheme + "://" +

                    System.Web.HttpContext.Current.Request.Url.Authority + src;
                }
                else
                {
                    imgPath = src;
                }

                return iTextSharp.text.Image.GetInstance(imgPath);
            }
        }

        private void ShowPdf(string filename, string filePath)
        {
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.BufferOutput = true;
            Response.WriteFile(filePath);
            Response.End();
        }



    }
}