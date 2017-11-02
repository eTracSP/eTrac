using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace WorkOrderEMS.Helper
{
    public static class StringExtensionMethods
    {


        /// <summary>
        /// Matches the specified value with the specified RegExp.
        /// =============================================
        /// CREATED BY : Nagendra Upwanshi
        /// CREATED AT : Aug-22-2014
        /// =============================================
        /// USAGE :(myData.Match("[0-9]"))
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="pattern">The pattern.</param>
        /// <returns></returns>
        public static bool Match(this string value, string pattern)
        {
            Regex regex = new Regex(pattern);
            return regex.IsMatch(value);
        }


        /// <summary>
        /// Trims whitespaces including non printing 
        /// whitespaces like carriage returns, line feeds,
        /// and form feeds
        /// </summary>
        /// <param name="str">The string to trim</param>
        /// <returns></returns>
        public static String TrimWhiteSpace(this String newValue)
        {
            if (newValue == null)
            {
                return null;
            }
            Char[] whiteSpace = { '\r', '\n', '\f', '\t', '\v' };
            return newValue.Trim(whiteSpace).Trim();
        }

        public static String UNJavaScriptEscape(this String newValue)
        {
            return HttpUtility.UrlDecode(newValue);
        }
        public static String FixLineBreakForWeb(this String newValue)
        {
            return newValue.Replace(Environment.NewLine, "<br/>");
        }
        public static String FixTabsForWeb(this String newValue)
        {
            return newValue.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
        }

        public static String FixSpaceForWeb(this String newValue)
        {
            return newValue.Replace(" ", "&nbsp;");
        }

        /// <summary>
        /// Empties the on null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///=============================================
        /// CREATED BY : Nagendra Upwanshi
        /// CREATED AT : Aug-22-2014
        ///=============================================
        /// </returns>
        public static string EmptyOnNull(this string value)
        {

            if (string.IsNullOrEmpty(value))
                return "";
            else
                return value;
        }

        /// <summary>
        /// Replaces the by a string on null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="toReplaceValue">replacement value.</param>
        /// <returns>
        ///=============================================
        /// CREATED BY : Nagendra Upwanshi
        /// CREATED AT : Aug-22-2014        
        ///=============================================
        /// </returns>
        public static string ReplaceByOnNull(this string value, string toReplaceValue = "")
        {
            if (string.IsNullOrEmpty(value))
                return toReplaceValue;
            else
                return value;
        }

        /// <summary>
        //////=============================================
        /// Created By : Bhushan Dod
        /// Created On : November-11-2016        
        ///=============================================
        /// Use the current thread's culture info for conversion
        /// </summary>
        public static string ToTitleCase(this string str)
        {
            var cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        /// <summary>
        /// Overload which uses the culture info with the specified name
        /// </summary>
        public static string ToTitleCase(this string str, string cultureInfoName)
        {
            var cultureInfo = new CultureInfo(cultureInfoName);
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        public static string AppendTimeStamp(this string fileName)
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(fileName),
                DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Path.GetExtension(fileName)
                );
        }

    }

}
