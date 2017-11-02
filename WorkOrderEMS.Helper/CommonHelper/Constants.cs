using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml;

namespace WorkOrderEMS.Helper
{
    public class Constants
    {
        /// <summary>
        /// To set months  
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Aug-22-2014</CreatedOn>
        /// </summary>
        public static class Month
        {
            public const string January = "January(1)";
            public const string February = "February(2)";
            public const string March = "March(3)";
            public const string April = "April(4)";
            public const string May = "May(5)";
            public const string June = "June(6)";
            public const string July = "July(7)";
            public const string August = "August(8)";
            public const string September = "September(9)";
            public const string October = "October(10)";
            public const string November = "November(11)";
            public const string December = "December(12)";
        }

        public static decimal DistanceCalculator(string originPlace, string destinationPlace)
        {
            //Declare variable to store XML result
            decimal miles = 0;
            string xmlResult = null;

            try
            {
                //Pass request to google api with orgin and destination details
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://maps.googleapis.com/maps/api/distancematrix/xml?origins=" + originPlace + "&destinations=" + destinationPlace + "&mode=Car&language=us-en&sensor=false");

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //Get response as stream from httpwebresponse
                StreamReader resStream = new StreamReader(response.GetResponseStream());

                //Create instance for xml document
                XmlDocument doc = new XmlDocument();

                xmlResult = resStream.ReadToEnd();

                //Load xmlResult variable value into xml documnet
                doc.LoadXml(xmlResult);

                string output = "";

                try
                {
                    //Get specified element value using select single node method and verify it return OK (success ) or failed
                    if (doc.DocumentElement.SelectSingleNode("/DistanceMatrixResponse/row/element/status").InnerText.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture) != "OK")
                    {
                    }

                    //Get DistanceMatrixResponse element and its values
                    XmlNodeList xnList = doc.SelectNodes("/DistanceMatrixResponse");
                    foreach (XmlNode xn in xnList)
                    {
                        if (xn["status"].InnerText.ToString() == "OK")
                        {
                            //Form a table and bind it orgin, destination place and return distance value, approximate duration
                            output = "<table align='center' width='600' cellpadding='0' cellspacing='0'>";
                            output += "<tr><td height='60' colspan='2' align='center'><b>Travel Details</b></td>";
                            output += "<tr><td height='40' width='30%' align='left'>Orgin Place</td><td align='left'>" + xn["origin_address"].InnerText.ToString() + "</td></tr>";
                            output += "<tr><td height='40' align='left'>Destination Place</td><td align='left'>" + xn["destination_address"].InnerText.ToString() + "</td></tr>";
                            output += "<tr><td height='40' align='left'>Travel Duration (apprx.)</td><td align='left'>" + doc.DocumentElement.SelectSingleNode("/DistanceMatrixResponse/row/element/duration/text").InnerText + "</td></tr>";
                            output += "<tr><td height='40' align='left'>Distance</td><td align='left'>" + doc.DocumentElement.SelectSingleNode("/DistanceMatrixResponse/row/element/distance/text").InnerText + "</td></tr>";
                            output += "</table>";

                            string kilmText = doc.DocumentElement.SelectSingleNode("/DistanceMatrixResponse/row/element/distance/text").InnerText;
                            kilmText = kilmText.Split(' ')[0];
                            double kilom = Convert.ToDouble(kilmText, CultureInfo.InvariantCulture);

                            miles = ConvertKilometersToMiles(kilom);
                        }
                    }
                }
                catch (Exception)
                {
                    // return 0;
                    throw;
                }

            }
            catch (Exception)
            {
                // return 0;
                throw;
            }
            return miles;



        }

        public static decimal ConvertKilometersToMiles(double kilometers)
        {
            decimal data = Convert.ToDecimal(kilometers * 0.621371192);
            return Math.Round(data, 2);
        }

        private static string loca_configCountryValue = System.Configuration.ConfigurationManager.AppSettings["eTracDefaultCountry"];
        private static string _configCountryValue = !string.IsNullOrEmpty(loca_configCountryValue) ? loca_configCountryValue : null;

        public static string ConfigCountryValue
        {
            get
            {
                return _configCountryValue;
            }
            set
            {
                _configCountryValue = value;
            }
        }
    }
}
