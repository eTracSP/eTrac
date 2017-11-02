using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace WorkOrderEMS.Helper.SerializationHelper
{
    /// <summary>
    /// DESCRIPTION : THIS STATIC CLASS PROVIDE THE CUSTOM METHODS TO HANDLE SEARIALIZATION AND DE-SEARIALIZATION
    ///=============================================
    /// CREATED BY : Nagendra Upwanshi
    /// CREATED AT : Aug-22-2014
    ///=============================================
    /// </summary>
    public static class SerializationHelper
    {
        #region SEARILIZATION
        /// <summary>
        /// Converts the object to XML.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="rootnode">The root node.</param>
        /// <returns>
        public static XmlDocument SerializeObjectToXml(object source, string rootnode = "data")
        {
            var jsonData = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeXmlNode(jsonData, rootnode);
        }
        /// <summary>
        /// Json  to XML.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        /// <param name="rootElementName">Name of the root element.</param>
        /// <returns>
        public static string SerializeJsonToXml(string jsonString, string rootElementName = "data")
        {
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                return JsonConvert.DeserializeXmlNode(jsonString, rootElementName, true)
                    .InnerXml
                    .ToString();
            }
            return "<data></data>";
        }
        /// <summary>
        /// Object to json.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        public static string SerializeObjectToJson(dynamic objValue)
        {
            return JsonConvert.SerializeObject(objValue);
        }
        /// <summary>
        /// Serializes the xml to json.
        /// </summary>
        /// <param name="xmlString">The XML string.</param>
        /// <returns>
        public static string SerializeXmlToJson(string xmlString = "<data></data>")
        {
            var doc1 = new XDocument();
            doc1 = XDocument.Parse(xmlString);
            return JsonConvert.SerializeXNode(doc1);
        }
        /// <summary>
        /// Serializes the XML string array.
        /// </summary>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="xmlString">The XML string.</param>
        /// <returns>
        public static string[] SerializeXmlStringArray(string elementName = "", string xmlString = "<data></data>")
        {
            XDocument doc = XDocument.Parse(xmlString);
            return doc.Root.Elements(elementName)
                               .Select(element => element.Value)
                               .ToArray();
        }
        #endregion

        #region DE SERIALIZATION

        /// <summary>
        /// Deserializes from XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">The XML.</param>
        /// <returns>
        /// =============================================
        /// CREATED BY : Nagendra Upwanshi
        /// CREATED AT : Aug-22-2014
        /// =============================================
        /// </returns>
        public static T DeserializeFromXml<T>(string xml)
        {
            T result;
            var ser = new XmlSerializer(typeof(T));
            using (TextReader tr = new StringReader(xml))
            {
                result = (T)ser.Deserialize(tr);
            }
            return result;
        }
        /// <summary>
        /// De serialization  the json to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsondata">The jsondata.</param>
        /// <returns>
        ///=============================================
        /// CREATED BY : Nagendra Upwanshi
        /// CREATED AT : Aug-22-2014
        ///=============================================
        /// </returns>
        public static T DeSerializeJsonToObject<T>(string jsondata)
        {
            return JsonConvert.DeserializeObject<T>(jsondata);
        }

        #endregion

        #region OTHERS
        /// <summary>
        /// Gets the type of the deserialized anonymous.
        /// </summary>
        /// <param name="jsonstring">The json string.</param>
        /// <param name="obj">The obj.</param>
        /// <returns>
        ///=============================================
        /// CREATED BY : Nagendra Upwanshi
        /// CREATED AT : Aug-22-2014
        ///=============================================
        /// </returns>
        public static dynamic GetDeserializedAnonymousType(string jsonstring, dynamic obj)
        {
            JToken root = JObject.Parse(jsonstring);
            return JsonConvert.DeserializeAnonymousType(root.ToString(), obj);
        }
        /// <summary>
        /// Converts the text to HTML.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="IshtmlTagsAllowed">if set to <c>true</c> [is html tags allowed].</param>
        /// <returns>
        ///=============================================
        /// CREATED BY : Nagendra Upwanshi
        /// CREATED AT : Aug-22-2014
        ///=============================================
        /// </returns>
        public static string ConvertTextToHtml(string text, bool ishtmlTagsAllowed)
        {
            //Create a StringBuilder object from the string input
            //parameter
            StringBuilder sb = new StringBuilder(text);
            //Replace all double white spaces with a single white space
            //and &nbsp;
            sb.Replace("  ", " &nbsp;");
            //Check if HTML tags are not allowed
            if (!ishtmlTagsAllowed)
            {
                //Convert the brackets into HTML equivalents
                sb.Replace("<", "&lt;");
                sb.Replace(">", "&gt;");
                //Convert the double quote
                sb.Replace("\"", "&quot;");
            }
            //Create a StringReader from the processed string of
            //the StringBuilder object
            StringReader sr = new StringReader(sb.ToString());
            StringWriter sw = new StringWriter();
            //Loop while next character exists
            while (sr.Peek() > -1)
            {
                //Read a line from the string and store it to a temp
                //variable
                string temp = sr.ReadLine();
                //write the string with the HTML break tag
                //Note here write method writes to a Internal StringBuilder
                //object created automatically
                sw.Write(temp + "<br>");
            }
            //Return the final processed text
            return sw.GetStringBuilder().ToString();
        }
        #endregion
    }

    public static class SerializationHelperExtension
    {
        #region SEARILIZATION

        /// <summary>
        /// Json  to XML.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        /// <param name="rootElementName">Name of the root element.</param>
        /// <returns>
        public static string ToXml(this string jsonString, string rootElementName = "data")
        {
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                return JsonConvert.DeserializeXmlNode(jsonString, rootElementName, true)
                    .InnerXml
                    .ToString();
            }
            return "<data></data>";
        }

        /// <summary>
        /// Object to json.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// </returns>
        public static string ToObjJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static string ToObjXml(this object obj)
        {
            return JsonConvert.SerializeObject(obj).ToXml();
        }


        /// <summary>
        /// Serializes the xml to json.
        /// </summary>
        /// <param name="xmlString">The XML string.</param>
        /// <returns>
        /// </returns>
        public static string ToJson(this string xmlValue)
        {
            if (string.IsNullOrEmpty(xmlValue))
            {
                xmlValue = "<data></data>";
            }
            var doc1 = new XDocument();
            doc1 = XDocument.Parse(xmlValue);
            return JsonConvert.SerializeXNode(doc1);
        }

        /// <summary>
        /// Gets the json value.
        /// </summary>
        /// <param name="jsonstring">The jsonstring.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string GetJsonValue(this string jsonString, string name)
        {
            JObject o = JObject.Parse(jsonString);
            return o.SelectToken(name).ToString();
        }

        /// <summary>
        /// Serializes the XML string array.
        /// </summary>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="xmlString">The XML string.</param>
        /// <returns>
        /// </returns>
        public static string[] SerializeXmlStringArray(string elementName = "", string xmlValue= "<data></data>")
        {
            XDocument doc = XDocument.Parse(xmlValue);
            return doc.Root.Elements(elementName)
                               .Select(element => element.Value)
                               .ToArray();
        }
        #endregion

        #region DE SERIALIZATION

        /// <summary>
        /// Desterilizes from XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">The XML.</param>
        /// <returns>
        public static T DeserializeFromXml<T>(this string xml)
        {
            T result;
            var ser = new XmlSerializer(typeof(T));
            using (TextReader tr = new StringReader(xml))
            {
                result = (T)ser.Deserialize(tr);
            }
            return result;
        }

        /// <summary>
        /// De serialization  the json to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsondata">The jsondata.</param>
        /// <returns>
        public static T DeserializeJsonToObject<T>(this string jsonData)
        {
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
        public static T DeserializeXmlToObject<T>(this string jsonData)
        {
            return JsonConvert.DeserializeObject<T>(jsonData.ToJson());
        }

        #endregion

        #region OTHERS
        /// <summary>
        /// Gets the type of the deserialized anonymous.
        /// </summary>
        /// <param name="jsonstring">The json string.</param>
        /// <param name="obj">The obj.</param>
        /// <returns>
        public static dynamic GetDeserializedAnonymousType(this string jsonValue, dynamic value)
        {
            JToken root = JObject.Parse(jsonValue);
            return JsonConvert.DeserializeAnonymousType(root.ToString(), value);
        }

        /// <summary>
        /// Converts the text to HTML.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="IshtmlTagsAllowed">if set to <c>true</c> [is html tags allowed].</param>
        /// <returns>
        public static string ConvertTextToHtml(this string text, bool ishtmlTagsAllowed)
        {
            //Create a StringBuilder object from the string input
            //parameter
            StringBuilder sb = new StringBuilder(text);
            //Replace all double white spaces with a single white space
            //and &nbsp;
            sb.Replace("  ", " &nbsp;");
            //Check if HTML tags are not allowed
            if (!ishtmlTagsAllowed)
            {
                //Convert the brackets into HTML equivalents
                sb.Replace("<", "&lt;");
                sb.Replace(">", "&gt;");
                //Convert the double quote
                sb.Replace("\"", "&quot;");
            }
            //Create a StringReader from the processed string of
            //the StringBuilder object
            StringReader sr = new StringReader(sb.ToString());
            StringWriter sw = new StringWriter();
            //Loop while next character exists
            while (sr.Peek() > -1)
            {
                //Read a line from the string and store it to a temp
                //variable
                string temp = sr.ReadLine();
                //write the string with the HTML break tag
                //Note here write method writes to a Internal StringBuilder
                //object created automatically
                sw.Write(temp + "<br>");
            }
            //Return the final processed text
            return sw.GetStringBuilder().ToString();
        }
        #endregion
    }
    /// <summary>
    /// <Created By>Bhushan Dod</Created>
    /// <CreatedDate>06-02-2015</CreatedDate>
    /// <Description>For Converersion Of JSON-To-XML and XML-To-JSON</Description>
    /// </summary>
    /// <typeparam name="T">Model</typeparam>
    public static class GenericDataContractSerializer<T>
    {
        /// <summary>
        /// <Created By>Bhushan Dod</Created>
        /// <CreatedDate>06-02-2015</CreatedDate>
        /// <Description>For Converersion Of JSON-To-XML </Description>
        /// </summary>
        /// <typeparam name="T">Json Data</typeparam>
        public static string SerializeObject(T obj)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                var stringBuilder = new StringBuilder();
                var stringWriter = new StringWriter(stringBuilder);
                xmlSerializer.Serialize(stringWriter, obj);

                return stringBuilder.ToString();
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to serialize data contract object to xml string:", exception);
            }
        }
        /// <summary>
        /// <Created By>Bhushan Dod</Created>
        /// <CreatedDate>06-02-2015</CreatedDate>
        /// <Description>Deserialize XML </Description>
        /// </summary>
        /// <typeparam name="T">XML Data</typeparam>
        public static T DeserializeXml(string xml)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                return (T)xmlSerializer.Deserialize(new StringReader(xml));
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to deserialize xml string to data contract object:", exception);
            }
        }
    }


}
