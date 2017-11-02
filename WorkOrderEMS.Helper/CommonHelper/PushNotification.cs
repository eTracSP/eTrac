using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace WorkOrderEMS.Helper
{
    public class PushNotification
    {
        public static string GCMAndroid(string alertmessage, string deviceId, object Data)
        {
            if (deviceId != null)
            {
                //if (deviceId.Length > 120)
                //{
                // your RegistrationID and DeviceId both are same which is received from GCM server.                                                              
                var applicationID = "AIzaSyDwUYDu7255IR3266k3AYjWErp4bCeeGl8";// applicationID means google Api key                                                                                                    
                var SENDER_ID = "1064381594331";   // SENDER_ID is nothing but your ProjectID (from API Console- google code)//                                         

                WebRequest tRequest;
                tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
                tRequest.Method = "POST";
                tRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

                //string postData = "data.Alert=" + alertmessage + "&data.action-loc-key=" + actionKey + " &registration_id=" + deviceId + "&badge=" + (1).ToString() + "";
                var Details = JsonConvert.SerializeObject(Data);

                string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message="
                    + alertmessage + "&data.time=" + System.DateTime.UtcNow.ToString() + "&data.details=" + Details + "&registration_id=" + deviceId + "";
                try
                {
                    Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    tRequest.ContentLength = byteArray.Length;
                    Stream dataStream = tRequest.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    WebResponse tResponse = tRequest.GetResponse();

                    dataStream = tResponse.GetResponseStream();
                    StreamReader tReader = new StreamReader(dataStream);
                    String sResponseFromServer = tReader.ReadToEnd();   //Get response from GCM server.

                    tReader.Close();
                    dataStream.Close();
                    tResponse.Close();
                    return "success";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //}
                //else
                //{
                //    return "failed";
                //}
            }
            else
            {
                return "failed";
            }
        }

    }
}
