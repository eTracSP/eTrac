using System;
using System.IO;
using System.Web;

namespace WorkOrderEMS.BusinessLogic.Exception_B
{
    public static class Exception_B
    {
        /// <summary>
        /// Created by vijay sahu on feb 2015
        /// This method is used for mentaining all exception into database. 
        /// Modified By :  Bhushan Dod on 05/04/2016 for stack trace event and also to catch inner exception.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="methodName"></param>
        /// <param name="type"></param>
        /// <param name="param"></param>
        public static void exceptionHandel_Runtime(Exception ex, string methodName, string type, object param)
        {
            try
            {

                System.Web.Script.Serialization.JavaScriptSerializer objSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                string exx = "";
                if (ex.InnerException == null)
                {
                    exx = "null";
                }
                else
                {
                    exx = ex.InnerException.Message.ToString();
                }

                using (WorkOrderEMS.Data.EntityModel.workorderEMSEntities objContext = new WorkOrderEMS.Data.EntityModel.workorderEMSEntities())
                {
                    objContext.ExceptionLogs.Add(new WorkOrderEMS.Data.EntityModel.ExceptionLog()
                    {
                        methodName = methodName.ToString(),
                        InnerExceptionMessage = exx,
                        Message = ex.Message.ToString(),
                        GetBaseException = ex.GetBaseException().ToString(),
                        GetHashCode = ex.HResult.GetHashCode().ToString(),
                        Type = type,
                        createdDate = DateTime.UtcNow,
                        Parameters = objSerializer.Serialize(param)

                    });

                    objContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                //ShiftsterService.EmailUtility.SendMail("vijaysa@smartdatainc.net", "exceptionRaisedInShiftsterServiceV4", ex.Message);
            }
        }


        public static void TraceService(string content)
        {
            //set up a filestream
            //FileStream fs = new FileStream(@"d:\Testing_Path.txt", FileMode.OpenOrCreate, FileAccess.Write);
            System.IO.FileStream fs = new System.IO.FileStream(HttpContext.Current.Server.MapPath("~") + "/WorkOrderEms.txt", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);

            fs.Flush();
            //set up a streamwriter for adding text
            StreamWriter sw = new StreamWriter(fs);

            //find the end of the underlying filestream
            sw.BaseStream.Seek(0, SeekOrigin.End);
            //add the text
            sw.WriteLine(content);
            //add the text to the underlying filestream

            sw.Flush();
            //close the writer
            sw.Close();
        }

    }
}
