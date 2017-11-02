using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Web;

namespace WorkOrderEMS.Helper
{
    public class CommonHelper
    {
        /// <summary>
        /// This is used to Generate a r Chararter Code for Contact verifications
        /// </summary>
        /// <Createdby>Nagendra Upwanshi</Createdby>
        /// <CreateDate>Aug-22-2014</CreateDate>
        /// <returns></returns>
        public static string GenerateAnUniqueCodeForVerificationOfSixDigit()
        {
            try
            {

                //string randomValue = "";
                //Random ObjRandome = new Random();
                //randomValue = Convert.ToString(ObjRandome.Next(11111, 99999));
                //return randomValue;
                string CharaterString = "123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                Random ObjRandome = new Random();
                string Value = "";
                for (int i = 0; i < 6; i++)
                {
                    char ch = CharaterString[ObjRandome.Next(CharaterString.Length)];
                    Value += ch.ToString();
                }
                return Value;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// This is used to Generate a 4 Chararter Code for Contact verifications
        /// </summary>
        /// <Createdby>Nagendra Upwanshi</Createdby>
        /// <CreateDate>Aug-22-2014</CreateDate>
        /// <returns></returns>
        public static string GenerateAnUniqueCodeForVerificationOfFourDigit()
        {
            try
            {

                //string randomValue = "";
                //Random ObjRandome = new Random();
                //randomValue = Convert.ToString(ObjRandome.Next(11111, 99999));
                //return randomValue;
                string CharaterString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
                Random ObjRandome = new Random();
                string Value = "";
                for (int i = 0; i < 4; i++)
                {
                    char ch = CharaterString[ObjRandome.Next(CharaterString.Length)];
                    Value += ch.ToString();
                }
                return Value;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This is used to Generate a rendom leangth Chararter Code.
        /// </summary>
        /// <Createdby>Nagendra Upwanshi</Createdby>
        /// <CreateDate>aug-22-2014</CreateDate>
        /// <returns></returns>
        public static string GenerateRandomCode(int length)
        {
            try
            {

                string CharaterString = "123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                Random ObjRandome = new Random();
                string Value = "";
                for (int i = 0; i < length; i++)
                {
                    char ch = CharaterString[ObjRandome.Next(CharaterString.Length)];
                    Value += ch.ToString();
                }
                return Value;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This is used to Generate a rendom leangth Chararter Code.
        /// </summary>
        /// <Createdby>Nagendra Upwanshi</Createdby>
        /// <CreateDate>aug-22-2014</CreateDate>
        /// <returns></returns>
        public bool UploadImage(HttpPostedFileBase myFile, string path, string imageName)
        {
            //message = string.Empty;
            if (myFile != null && myFile.ContentLength != 0)
            {
                if (this.CreateFolderIfNeeded(path))
                {
                    try
                    {
                        myFile.SaveAs(Path.Combine(path, imageName));
                        return true;

                    }
                    catch (Exception)
                    {

                        return false;
                        //message = ex.Message;
                        throw;
                    }
                }

            }
            return false;
        }

        /// <summary>
        /// This is used to Generate a rendom leangth Chararter Code.
        /// </summary>
        /// <Createdby>Ashwajit bansod</Createdby>
        /// <CreateDate>Sept-29-2017</CreateDate>
        /// <returns></returns>
        public  static bool StaticUploadImage(HttpPostedFileBase myFile, string path, string imageName)
        {
            //message = string.Empty;
            if (myFile != null && myFile.ContentLength != 0)
            {
                if (StaticCreateFolderIfNeeded(path))
                {
                    try
                    {
                        myFile.SaveAs(Path.Combine(path, imageName));
                        return true;

                    }
                    catch (Exception)
                    {

                        return false;
                        //message = ex.Message;
                        throw;
                    }
                }

            }
            return false;
        }
        private bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {

                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    result = false;
                    throw;
                    /*TODO: You must process this exception.*/
                }
            }
            return result;
        }

        public static bool StaticCreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {

                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    result = false;
                    throw;
                    /*TODO: You must process this exception.*/
                }
            }
            return result;
        }

        /// <summary>
        /// <CreatedBy>Bhushan Dod</Created>
        /// <CreatedDate>02/11/2015</CreatedDate>
        /// <Description>Generate PDF of Grid Data</Description>
        /// </summary>
        /// <typeparam name="InventoryMasterModelList"></typeparam>
        /// <param name="customerList"></param>
        /// <param name="columns"></param>
        /// <param name="filePath"></param>
        public static void ExportPDF<InventoryMasterModelList>(IList<InventoryMasterModelList> customerList, string[] columns, string filePath, int? InventoryType)
        {
            string value;
            float[] widths;
            try
            {

                iTextSharp.text.Font headerFont = FontFactory.GetFont("Verdana", 11, BaseColor.WHITE);
                iTextSharp.text.Font rowfont = FontFactory.GetFont("Verdana", 10, BaseColor.BLACK);
                Document document = new Document(PageSize.A3);
                PdfWriter writer = PdfWriter.GetInstance(document,
                           new FileStream(filePath, FileMode.OpenOrCreate));
                document.Open();
                iTextSharp.text.Font pdfHeadFont = FontFactory.GetFont("TIMES_ROMAN", 18, BaseColor.BLUE);
                Paragraph paragraph = new Paragraph("Welcome to eTrac Services", pdfHeadFont);

                string imageURL = ConfigurationManager.AppSettings["hostingPrefix"] + "Images/logo2.png";

                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);

                //Resize image depend upon your need

                jpg.ScaleToFit(140f, 120f);

                //Give space before image

                jpg.SpacingBefore = 120f;

                //Give some space after the image

                jpg.SpacingAfter = 10f;

                jpg.Alignment = Element.ALIGN_CENTER;
                paragraph.Alignment = Element.ALIGN_CENTER;

                document.Add(paragraph);

                document.Add(jpg);


                PdfPTable table = new PdfPTable(columns.Length);
                table.TotalWidth = 700f;
                table.LockedWidth = true;
                if (InventoryType == 196)
                {
                    widths = new float[] { 32f, 60f, 60f, 65f, 40f, 35f, 40f };
                }
                else
                {
                    widths = new float[] { 32f, 40f, 60f, 65f, 35f, 35f, 100f, 50f, 50f };
                }
                table.SetWidths(widths);
                foreach (var column in columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column, headerFont));
                    cell.BackgroundColor = BaseColor.BLACK;
                    table.AddCell(cell);
                }

                foreach (var item in customerList)
                {
                    foreach (var column in columns)
                    {
                        if (item.GetType().GetProperty(column).GetValue(item) == null)
                            value = "Not Available";
                        else
                            value = item.GetType().GetProperty(column).GetValue(item).ToString();
                        PdfPCell cell5 = new PdfPCell(new Phrase(value, rowfont));
                        table.AddCell(cell5);

                    }
                }
                document.Add(table);
                document.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This is used to save W9 FORM when user register new vehicle.
        /// </summary>
        /// <Createdby>Vijay sahu</Createdby>
        /// <CreateDate>26 feb 2015</CreateDate>
        /// <returns></returns>
        public void UploadW9Form(HttpPostedFileBase myFile, string path, string imageName)
        {

            //message = string.Empty;
            if (myFile != null && myFile.ContentLength != 0)
            {
                if (this.CreateFolderIfNeeded(path))
                {
                    try
                    {

                        myFile.SaveAs(Path.Combine(path, imageName));


                    }
                    catch (Exception)
                    {
                        //message = ex.Message;
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// This fn is used to save QRC Image.
        /// </summary>
        /// <Createdby>Bhushan Dod</Createdby>
        /// <CreateDate>06 Dec 2016</CreateDate>
        /// <returns></returns>
        public bool UploadQRCImage(string base64, string path, string imageName)
        {
            bool status = false;
            if (base64 != null && base64.Trim() != "")
            {
                if (this.CreateFolderIfNeeded(path))
                {
                    try
                    {
                        string imgLoc = path + imageName;
                        using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64)))
                        {
                            using (Bitmap bm2 = new Bitmap(ms))
                            {
                                bm2.Save(imgLoc);
                                status = true;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        status = false;
                    }
                }
            }
            return status;
        }

    }
}
