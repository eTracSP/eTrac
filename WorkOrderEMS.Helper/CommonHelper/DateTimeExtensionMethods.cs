using System;
using System.Collections.Generic;
using System.Globalization;

namespace WorkOrderEMS.Helper
{
    public static class DateTimeExtensionMethods
    {
        public static DateTime GetLocalTime(this DateTime dateTime, string timeZoneName)
        {
            if (string.IsNullOrEmpty(timeZoneName))
            {
                timeZoneName = "India Standard Time";
            }
            var convertedtime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, timeZoneName);
            return convertedtime;
        }
        //gldgdfgkldfmgld,global;df
      
        /// <summary>
        /// THis method is for convert time according to client time zone.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="isESTFormat"></param>
        /// <returns></returns>
        public static string ToClientTimeZone(this DateTime dt, bool isESTFormat = false)
        {
            try
            {
                if (System.Web.HttpContext.Current.Request.Cookies["timezoneoffset"] != null || System.Web.HttpContext.Current.Request.Cookies["timezonename"] != null)
                {
                    //dt = DateTime.UtcNow; ; // may 1st
                    var timezonename = System.Web.HttpContext.Current.Request.Cookies["timezonename"].Value;
                    timezonename = timezonename.Replace("%2F", "/");
                    var timezoneLocal1 = TimeZoneConverter.TZConvert.IanaToWindows(timezonename);
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timezoneLocal1);

                    //dt = TimeZoneInfo.ConvertTimeFromUtc(dt, tzi);
                    bool isCurrentlyDaylightSavings = tzi.IsDaylightSavingTime(dt);
                    if (isCurrentlyDaylightSavings == true)
                        dt.AddHours(1);

                    var timeOffSet = System.Web.HttpContext.Current.Request.Cookies["timezoneoffset"].Value;
                    // var converted = (-1 * Convert.ToInt32(timeOffSet));
                    var offset = int.Parse(timeOffSet.ToString());
                    dt = dt.AddMinutes(-1 * offset);

                    if (isESTFormat)
                    {
                        return dt.ToString("MM'/'dd'/'yyyy hh:mm tt");
                    }
                    else
                    {
                        return dt.ToString();
                    }
                    //var timeUtc = DateTime.UtcNow;
                    //TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    //DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
                }
                // if there is no offset in session return the datetime in server timezone
                return dt.ToLocalTime().ToString();
            }
            catch (Exception)
            {
                return DateTime.UtcNow.ToLocalTime().ToString();
            }

        }
        public static DateTime ToClientTimeZoneinDateTime(this DateTime dt)
        {
            try {               
                if (System.Web.HttpContext.Current.Request.Cookies["timezoneoffset"] != null || System.Web.HttpContext.Current.Request.Cookies["timezonename"] != null)
                {                  
                    var timezonename = System.Web.HttpContext.Current.Request.Cookies["timezonename"].Value;
                    timezonename = timezonename.Replace("%2F", "/");
                    var timezoneLocal1 = TimeZoneConverter.TZConvert.IanaToWindows(timezonename);
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timezoneLocal1);
                    bool isCurrentlyDaylightSavings = tzi.IsDaylightSavingTime(dt);
                    if (isCurrentlyDaylightSavings == true)
                        dt.AddHours(1);

                    var timeOffSet = System.Web.HttpContext.Current.Request.Cookies["timezoneoffset"].Value;
                   
                    var offset = int.Parse(timeOffSet.ToString());
                    dt = dt.AddMinutes(-1 * offset);                  
                    return dt;
                }             
                return dt.ToLocalTime();
            }
            catch (Exception)
            {
                return DateTime.UtcNow;
            }
        }

        public static DateTime ToClientTimeZoneinDateTimeReports(this DateTime dt)
        {
            try
            {
                if (System.Web.HttpContext.Current.Request.Cookies["timezoneoffset"] != null || System.Web.HttpContext.Current.Request.Cookies["timezonename"] != null)
                {
                    var timezonename = System.Web.HttpContext.Current.Request.Cookies["timezonename"].Value;
                    timezonename = timezonename.Replace("%2F", "/");
                    var timezoneLocal1 = TimeZoneConverter.TZConvert.IanaToWindows(timezonename);
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timezoneLocal1);

                    bool isCurrentlyDaylightSavings = tzi.IsDaylightSavingTime(dt);
                    var timeOffSet = System.Web.HttpContext.Current.Request.Cookies["timezoneoffset"].Value;
                    var offset = int.Parse(timeOffSet.ToString());
                    //Modified on - 03/27/2017
                    //In DST time, no need of + - concept in minutes.In future if creates problem(may be when DST not) plz comment this method and uncomment below method.
                    TimeZoneInfo tziEST = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    DateTime myTimeEST = TimeZoneInfo.ConvertTime(DateTime.UtcNow, tziEST);
                    long dummyConversion;

                    bool isDSTinEST = tziEST.IsDaylightSavingTime(myTimeEST);
                    if (isDSTinEST)
                    {
                        timezoneLocal1 = timezoneLocal1.Replace(" ", "");
                        if (timezoneLocal1 == "EasternStandardTime")
                        {
                            dummyConversion = (1 * Convert.ToInt64(timeOffSet));
                            //dummyConversion = Convert.ToInt64(dummyConversion) - 60;
                            dt = dt.AddMinutes(1 * dummyConversion);
                        }
                        else
                        {
                            dummyConversion = (-1 * Convert.ToInt64(timeOffSet));
                            dt = dt.AddMinutes(1 * dummyConversion);
                            //  dt = dt.AddHours(1);// for conversion
                        }
                    }
                    else
                    {
                        dt = dt.AddMinutes(1 * offset);
                    }
                    return dt;

                    //if (dt.ToLongTimeString() != "12:00:00 AM" && dt.ToLongTimeString() != "11:59:59 PM")
                    //{
                    //    dt = TimeZoneInfo.ConvertTimeToUtc(dt, tzi);
                    //    return dt;
                    //}
                    //else
                    //{
                    //    //Newly done code start here
                    //    var isPreviousDay = TimeZoneInfo.ConvertTimeFromUtc(dt, tzi);
                    //    if (dt.Date != isPreviousDay.Date && dt.ToLongTimeString() == "12:00:00 AM")
                    //    {
                    //        isPreviousDay = new DateTime(isPreviousDay.Year, isPreviousDay.Month, isPreviousDay.Day, 0, 0, 0);
                    //        isPreviousDay = TimeZoneInfo.ConvertTimeToUtc(isPreviousDay, tzi);
                    //        //  return isPreviousDay;
                    //    }
                    //    if(dt.Date == isPreviousDay.Date)
                    //    //else
                    //    //{
                    //    //    dt = TimeZoneInfo.ConvertTimeToUtc(dt, tzi);
                    //    //    return dt;
                    //    //}
                    //    return isPreviousDay;
                    //}


                    //////Newly Done code end here

                }
                // if there is no offset in session return the datetime in server timezone
                return dt.ToLocalTime();
            }
            catch (Exception)
            {
                return DateTime.UtcNow;
            }
        }


        //public static DateTime ToClientTimeZoneinDateTimeReports(this DateTime dt)
        //{
        //    try
        //    {
        //        if (System.Web.HttpContext.Current.Request.Cookies["timezoneoffset"] != null || System.Web.HttpContext.Current.Request.Cookies["timezonename"] != null)
        //        {
        //            //var timezoneLocal1 = FindTimezoneName(timezoneLocal);
        //            var timezonename = System.Web.HttpContext.Current.Request.Cookies["timezonename"].Value;
        //            timezonename = timezonename.Replace("%2F", "/");
        //            var timezoneLocal1 = TimeZoneConverter.TZConvert.IanaToWindows(timezonename);                   
        //            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timezoneLocal1);
        //            bool isCurrentlyDaylightSavings = tzi.IsDaylightSavingTime(dt);
        //            if (isCurrentlyDaylightSavings == true)
        //                dt.AddHours(1);

        //            var timeOffSet = System.Web.HttpContext.Current.Request.Cookies["timezoneoffset"].Value;
        //            //  var converted = (-1 * Convert.ToInt32(timeOffSet));
        //            var offset = int.Parse(timeOffSet.ToString());
        //            dt = dt.AddMinutes(1 * offset);

        //            return dt;
        //        }
        //        // if there is no offset in session return the datetime in server timezone
        //        return dt.ToLocalTime();
        //    }
        //    catch (Exception)
        //    {
        //        return DateTime.UtcNow;
        //    }

        //}
        public static string ToMobileClientTimeZone(this DateTime dt,string TimeZoneName, long TimeZoneOffset, bool IsTimeZoneinDaylight, bool isESTFormat = false)
        {
            try
            {
                if (true)
                {
                    //dt = DateTime.UtcNow; ; // may 1st

                    TimeZoneName = TimeZoneName.Replace("%2F", "/");
                    var timezoneLocal1 = TimeZoneConverter.TZConvert.IanaToWindows(TimeZoneName);
                    //TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    //bool isCurrentlyDaylightSavings = tzi.IsDaylightSavingTime(dt);
                    if (IsTimeZoneinDaylight)
                        dt.AddHours(1);

                    //  var timeOffSet = System.Web.HttpContext.Current.Request.Cookies["timezoneoffset"].Value; //Commented by Bhushan Dod due yo hardcoded time by mobile.Need to change in future once mobile developer available.              
                    var offset = Convert.ToInt32(TimeZoneOffset);
                    //var offset = int.Parse(timeOffSet.ToString()); 240
                    dt = dt.AddMinutes(offset);

                    if (isESTFormat)
                    {
                        return dt.ToString("MM'/'dd'/'yyyy hh:mm tt");
                    }
                    else
                    {
                        return dt.ToString();
                    }

                    //var timeUtc = DateTime.UtcNow;
                    //TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    //DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
                }
                // if there is no offset in session return the datetime in server timezone
                // return dt.ToLocalTime().ToString();
            }
            catch (Exception)
            {
                return DateTime.UtcNow.ToLocalTime().ToString();
            }

        }

        public static DateTime ToMobileClientTimeZoneinDateTime(this DateTime dt, string TimeZoneName, long TimeZoneOffset, bool IsTimeZoneinDaylight)
        {         
                try
            {
                TimeZoneName = TimeZoneName.Replace("%2F", "/");
                var timezoneLocal1 = TimeZoneConverter.TZConvert.IanaToWindows(TimeZoneName);
                //TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                //bool isCurrentlyDaylightSavings = tzi.IsDaylightSavingTime(dt);
                if (IsTimeZoneinDaylight)
                    dt.AddHours(1);

                //  var timeOffSet = System.Web.HttpContext.Current.Request.Cookies["timezoneoffset"].Value; //Commented by Bhushan Dod due yo hardcoded time by mobile.Need to change in future once mobile developer available.              
                var offset = Convert.ToInt32(TimeZoneOffset);
                //var offset = int.Parse(timeOffSet.ToString()); 240
                dt = dt.AddMinutes(offset);

                return dt;
                //var timeUtc = DateTime.UtcNow;
                //TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                //DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

                // if there is no offset in session return the datetime in server timezone
                // return dt.ToLocalTime().ToString();
            }
            catch (Exception)
            {
                return DateTime.UtcNow.ToLocalTime();
            }
        }
        //public static DateTime ToMobileClientTimeZoneinDateTime(this DateTime dt)
        //{
        //    try
        //    {
        //        //dt = DateTime.UtcNow; ; // may 1st
        //        // var timezonename = System.Web.HttpContext.Current.Request.Cookies["timezonename"].Value;
        //        //  timezonename = timezonename.Replace("%2F", "/");
        //        //var timezoneLocal1 = FindTimezoneName("Eastern Standard Time");
        //        TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        //        bool isCurrentlyDaylightSavings = tzi.IsDaylightSavingTime(dt);
        //        if (isCurrentlyDaylightSavings == true)
        //            dt.AddHours(1);

        //        //  var timeOffSet = System.Web.HttpContext.Current.Request.Cookies["timezoneoffset"].Value; //Commented by Bhushan Dod due yo hardcoded time by mobile.Need to change in future once mobile developer available.              
        //        var offset = int.Parse("240");
        //        //var offset = int.Parse(timeOffSet.ToString()); 240
        //        dt = dt.AddMinutes(-1 * offset);

        //        return dt;
        //        //var timeUtc = DateTime.UtcNow;
        //        //TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        //        //DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

        //        // if there is no offset in session return the datetime in server timezone
        //        // return dt.ToLocalTime().ToString();
        //    }
        //    catch (Exception)
        //    {
        //        return DateTime.UtcNow.ToLocalTime();
        //    }

        //}

        //public static string ToMobileClientTimeZone(this DateTime dt, bool isESTFormat = false)
        //{
        //    try
        //    {
        //        if (true)
        //        {
        //            //dt = DateTime.UtcNow; ; // may 1st
        //            // var timezonename = System.Web.HttpContext.Current.Request.Cookies["timezonename"].Value;
        //            //  timezonename = timezonename.Replace("%2F", "/");
        //            //var timezoneLocal1 = FindTimezoneName("Eastern Standard Time");
        //            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        //            bool isCurrentlyDaylightSavings = tzi.IsDaylightSavingTime(dt);
        //            if (isCurrentlyDaylightSavings == true)
        //                dt.AddHours(1);

        //            //  var timeOffSet = System.Web.HttpContext.Current.Request.Cookies["timezoneoffset"].Value; //Commented by Bhushan Dod due yo hardcoded time by mobile.Need to change in future once mobile developer available.              
        //            var offset = int.Parse("240");
        //            //var offset = int.Parse(timeOffSet.ToString()); 240
        //            dt = dt.AddMinutes(-1 * offset);

        //            if (isESTFormat)
        //            {
        //                return dt.ToString("MM'/'dd'/'yyyy hh:mm tt");
        //            }
        //            else
        //            {
        //                return dt.ToString();
        //            }

        //            //var timeUtc = DateTime.UtcNow;
        //            //TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        //            //DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
        //        }
        //        // if there is no offset in session return the datetime in server timezone
        //        // return dt.ToLocalTime().ToString();
        //    }
        //    catch (Exception)
        //    {
        //        return DateTime.UtcNow.ToLocalTime().ToString();
        //    }

        //}
        /// <summary>
        /// Created By- Bhushan Dod on 25 June 2017
        /// Created for - To change client time zone according to UTC time.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ConvertClientTZtoUTC(this DateTime dt)
        {
            try
            {
                if (System.Web.HttpContext.Current.Request.Cookies["timezoneoffset"] != null || System.Web.HttpContext.Current.Request.Cookies["timezonename"] != null)
                {
                    var timezonename = System.Web.HttpContext.Current.Request.Cookies["timezonename"].Value;
                    timezonename = timezonename.Replace("%2F", "/");
                    var timezoneLocal1 = TimeZoneConverter.TZConvert.IanaToWindows(timezonename);
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timezoneLocal1);
                    return TimeZoneInfo.ConvertTimeToUtc(dt, tzi);                                      
                }                
                // if there is no offset in session return the datetime in server timezone
                return DateTime.UtcNow;
            }
            catch (Exception)
            {
                return DateTime.UtcNow;
            }
        }
        /// <summary>
        /// Created By- Bhushan Dod on 25 June 2017
        /// Created for - To change UTC time according to client time zone.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetClientDateTimeNow(this DateTime dt)
        {
            try
            {
                if (System.Web.HttpContext.Current.Request.Cookies["timezoneoffset"] != null || System.Web.HttpContext.Current.Request.Cookies["timezonename"] != null)
                {
                    var timezonename = System.Web.HttpContext.Current.Request.Cookies["timezonename"].Value;
                    timezonename = timezonename.Replace("%2F", "/");
                    var timezoneLocal1 = TimeZoneConverter.TZConvert.IanaToWindows(timezonename);
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timezoneLocal1);
                    return TimeZoneInfo.ConvertTimeFromUtc(dt, tzi);
                }                
                return DateTime.UtcNow;
            }
            catch (Exception)
            {
                return DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Created By- Bhushan Dod on 11 July 2017
        /// Created for - To change UTC time according to client time zone for mobile.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetClientDateTimeForMobileNow(this DateTime dt,string TimeZoneName)
        {
            try
            {                                  
                    var timezoneLocal1 = TimeZoneConverter.TZConvert.IanaToWindows(TimeZoneName);
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timezoneLocal1);
                    return TimeZoneInfo.ConvertTimeFromUtc(dt, tzi);                
            }
            catch (Exception)
            {
                return DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Formats the specified datetime.
        ///=============================================
        /// CREATED BY : Nagendra Upwanshi
        /// CREATED AT : Aug-22-2014
        ///=============================================
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="format">
        ///<para>d    2/27/2009</para>
        ///<para>D    Friday, February 27, 2009</para>
        ///<para>f    Friday, February 27, 2009 12:11 PM</para>
        ///<para>F    Friday, February 27, 2009 12:12:22 PM</para>
        ///<para>g    2/27/2009 12:12 PMv</para>
        ///<para>G    2/27/2009 12:12:22 PM</para>
        ///<para>m    February 27</para>
        ///<para>M    February 27</para>
        ///<para>o    2009-02-27T12:12:22.1020000-08:00</para>
        ///<para>O    2009-02-27T12:12:22.1020000-08:00</para>
        ///<para>s    2009-02-27T12:12:22</para>
        ///<para>t    12:12 PM</para>
        ///<para>T    12:12:22 PM</para>
        ///<para>u    2009-02-27 12:12:22Z</para>
        ///<para>U    Friday, February 27, 2009 8:12:22 PM</para>
        ///<para>y    February, 2009</para>
        ///<para>Y    February, 2009</para>
        /// </param>
        /// <returns></returns>
        /// 
        public static string Format(this DateTime newDateTime, string formatValue = "d")
        {
            return newDateTime.ToString(formatValue, CultureInfo.InvariantCulture);
        }
        public static string DateTimePickerFormat(this DateTime? dateTime, string format = "MM/dd/yyyy hh:mm tt", bool emptyOnNull = true)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToString(format, CultureInfo.InvariantCulture);
            }
            if (emptyOnNull)
            {
                return "";
            }
            return null;
        }
        public static string GetLocalTimeString(this DateTime dateTime, string timeZoneName, string format = "MM/dd/yyyy HH:mm:ss")
        {
            var convertedtime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, timeZoneName);
            return convertedtime.ToString(format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Adds (or subtracts) a specified number of weekdays to a DateTime.
        /// </summary>
        /// <param name="days">The number of weekdays to add to the DateTime. Use a negative number to subtract that many weekdays.</param>
        /// <returns>A DateTime that has been adjusted by the specified number of weekdays.</returns>
        public static DateTime AddWeekdays(this DateTime date, int days)
        {
            var sign = days < 0 ? -1 : 1;
            var unsignedDays = Math.Abs(days);
            var weekdaysAdded = 0;

            while (weekdaysAdded < unsignedDays)
            {
                date = date.AddDays(sign);

                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    weekdaysAdded++;
            }

            return date;
        }

        public static DateTime SetTime(this DateTime date, int hour)
        {
            return date.SetTime(hour, 0, 0, 0);
        }

        public static DateTime SetTime(this DateTime date, int hour, int minute)
        {
            return date.SetTime(hour, minute, 0, 0);
        }

        public static DateTime SetTime(this DateTime date, int hour, int minute, int second)
        {
            return date.SetTime(hour, minute, second, 0);
        }

        public static DateTime SetTime(this DateTime date, int hour, int minute, int second, int millisecond)
        {
            return new DateTime(date.Year, date.Month, date.Day, hour, minute, second, millisecond);
        }

        public static DateTime FirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime LastDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

        public static string ToString(this DateTime? date)
        {
            return date.ToString(null, DateTimeFormatInfo.CurrentInfo);
        }

        public static string ToString(this DateTime? date, string format)
        {
            return date.ToString(format, DateTimeFormatInfo.CurrentInfo);
        }

        public static string ToString(this DateTime? date, IFormatProvider provider)
        {
            return date.ToString(null, provider);
        }

        public static string ToString(this DateTime? date, string format, IFormatProvider provider)
        {
            if (date.HasValue)
                return date.Value.ToString(format, provider);
            else
                return string.Empty;
        }

        public static string ToRelativeDateString(this DateTime date)
        {
            return GetRelativeDateValue(date, DateTime.Now);
        }

        public static string ToRelativeDateStringUtc(this DateTime date)
        {
            return GetRelativeDateValue(date, DateTime.UtcNow);
        }

        private static string GetRelativeDateValue(DateTime date, DateTime comparedTo)
        {
            TimeSpan diff = comparedTo.Subtract(date);
            if (diff.TotalDays >= 365)
                return string.Concat("on ", date.ToString("MMMM d, yyyy", CultureInfo.InvariantCulture));
            if (diff.TotalDays >= 7)
                return string.Concat("on ", date.ToString("MMMM d", CultureInfo.InvariantCulture));
            else if (diff.TotalDays > 1)
                return string.Format(CultureInfo.InvariantCulture, "{0:N0} days ago", diff.TotalDays);
            else if (diff.TotalDays == 1)
                return "yesterday";
            else if (diff.TotalHours >= 2)
                return string.Format(CultureInfo.InvariantCulture, "{0:N0} hours ago", diff.TotalHours);
            else if (diff.TotalMinutes >= 60)
                return "more than an hour ago";
            else if (diff.TotalMinutes >= 5)
                return string.Format(CultureInfo.InvariantCulture, "{0:N0} minutes ago", diff.TotalMinutes);
            if (diff.TotalMinutes >= 1)
                return "a few minutes ago";
            else
                return "less than a minute ago";
        }

        //public static string FindTimezoneName(string timezoneLocal)
        //{
        //    var olsonWindowsTimes = new Dictionary<string, string>()
        //        {
        //            { "Africa/Bangui", "W. Central Africa Standard Time" },
        //            { "Africa/Cairo", "Egypt Standard Time" },
        //            { "Africa/Casablanca", "Morocco Standard Time" },
        //            { "Africa/Harare", "South Africa Standard Time" },
        //            { "Africa/Johannesburg", "South Africa Standard Time" },
        //            { "Africa/Lagos", "W. Central Africa Standard Time" },
        //            { "Africa/Monrovia", "Greenwich Standard Time" },
        //            { "Africa/Nairobi", "E. Africa Standard Time" },
        //            { "Africa/Windhoek", "Namibia Standard Time" },
        //            { "America/Anchorage", "Alaskan Standard Time" },
        //            { "America/Argentina/San_Juan", "Argentina Standard Time" },
        //            { "America/Asuncion", "Paraguay Standard Time" },
        //            { "America/Bahia", "Bahia Standard Time" },
        //            { "America/Bogota", "SA Pacific Standard Time" },
        //            { "America/Buenos_Aires", "Argentina Standard Time" },
        //            { "America/Caracas", "Venezuela Standard Time" },
        //            { "America/Cayenne", "SA Eastern Standard Time" },
        //            { "America/Chicago", "Central Standard Time" },
        //            { "America/Chihuahua", "Mountain Standard Time (Mexico)" },
        //            { "America/Cuiaba", "Central Brazilian Standard Time" },
        //            { "America/Denver", "Mountain Standard Time" },
        //            { "America/Fortaleza", "SA Eastern Standard Time" },
        //            { "America/Godthab", "Greenland Standard Time" },
        //            { "America/Guatemala", "Central America Standard Time" },
        //            { "America/Halifax", "Atlantic Standard Time" },
        //            { "America/Indianapolis", "US Eastern Standard Time" },
        //            { "America/Indiana/Indianapolis", "US Eastern Standard Time" },
        //            { "America/La_Paz", "SA Western Standard Time" },
        //            { "America/Los_Angeles", "Pacific Standard Time" },
        //            { "America/Mexico_City", "Mexico Standard Time" },
        //            { "America/Montevideo", "Montevideo Standard Time" },
        //            { "America/New_York", "Eastern Standard Time" },
        //            { "America/Noronha", "UTC-02" },
        //            { "America/Phoenix", "US Mountain Standard Time" },
        //            { "America/Regina", "Canada Central Standard Time" },
        //            { "America/Santa_Isabel", "Pacific Standard Time (Mexico)" },
        //            { "America/Santiago", "Pacific SA Standard Time" },
        //            { "America/Sao_Paulo", "E. South America Standard Time" },
        //            { "America/St_Johns", "Newfoundland Standard Time" },
        //            { "America/Tijuana", "Pacific Standard Time" },
        //            { "Antarctica/McMurdo", "New Zealand Standard Time" },
        //            { "Atlantic/South_Georgia", "UTC-02" },
        //            { "Asia/Almaty", "Central Asia Standard Time" },
        //            { "Asia/Amman", "Jordan Standard Time" },
        //            { "Asia/Baghdad", "Arabic Standard Time" },
        //            { "Asia/Baku", "Azerbaijan Standard Time" },
        //            { "Asia/Bangkok", "SE Asia Standard Time" },
        //            { "Asia/Beirut", "Middle East Standard Time" },
        //            { "Asia/Calcutta", "India Standard Time" },
        //            { "Asia/Colombo", "Sri Lanka Standard Time" },
        //            { "Asia/Damascus", "Syria Standard Time" },
        //            { "Asia/Dhaka", "Bangladesh Standard Time" },
        //            { "Asia/Dubai", "Arabian Standard Time" },
        //            { "Asia/Irkutsk", "North Asia East Standard Time" },
        //            { "Asia/Jerusalem", "Israel Standard Time" },
        //            { "Asia/Kabul", "Afghanistan Standard Time" },
        //            { "Asia/Kamchatka", "Kamchatka Standard Time" },
        //            { "Asia/Karachi", "Pakistan Standard Time" },
        //            { "Asia/Katmandu", "Nepal Standard Time" },
        //            { "Asia/Kolkata", "India Standard Time" },
        //            { "Asia/Krasnoyarsk", "North Asia Standard Time" },
        //            { "Asia/Kuala_Lumpur", "Singapore Standard Time" },
        //            { "Asia/Kuwait", "Arab Standard Time" },
        //            { "Asia/Magadan", "Magadan Standard Time" },
        //            { "Asia/Muscat", "Arabian Standard Time" },
        //            { "Asia/Novosibirsk", "N. Central Asia Standard Time" },
        //            { "Asia/Oral", "West Asia Standard Time" },
        //            { "Asia/Rangoon", "Myanmar Standard Time" },
        //            { "Asia/Riyadh", "Arab Standard Time" },
        //            { "Asia/Seoul", "Korea Standard Time" },
        //            { "Asia/Shanghai", "China Standard Time" },
        //            { "Asia/Singapore", "Singapore Standard Time" },
        //            { "Asia/Taipei", "Taipei Standard Time" },
        //            { "Asia/Tashkent", "West Asia Standard Time" },
        //            { "Asia/Tbilisi", "Georgian Standard Time" },
        //            { "Asia/Tehran", "Iran Standard Time" },
        //            { "Asia/Tokyo", "Tokyo Standard Time" },
        //            { "Asia/Ulaanbaatar", "Ulaanbaatar Standard Time" },
        //            { "Asia/Vladivostok", "Vladivostok Standard Time" },
        //            { "Asia/Yakutsk", "Yakutsk Standard Time" },
        //            { "Asia/Yekaterinburg", "Ekaterinburg Standard Time" },
        //            { "Asia/Yerevan", "Armenian Standard Time" },
        //            { "Atlantic/Azores", "Azores Standard Time" },
        //            { "Atlantic/Cape_Verde", "Cape Verde Standard Time" },
        //            { "Atlantic/Reykjavik", "Greenwich Standard Time" },
        //            { "Australia/Adelaide", "Cen. Australia Standard Time" },
        //            { "Australia/Brisbane", "E. Australia Standard Time" },
        //            { "Australia/Darwin", "AUS Central Standard Time" },
        //            { "Australia/Hobart", "Tasmania Standard Time" },
        //            { "Australia/Perth", "W. Australia Standard Time" },
        //            { "Australia/Sydney", "AUS Eastern Standard Time" },
        //            { "Etc/GMT", "UTC" },
        //            { "Etc/GMT+11", "UTC-11" },
        //            { "Etc/GMT+12", "Dateline Standard Time" },
        //            { "Etc/GMT+2", "UTC-02" },
        //            { "Etc/GMT-12", "UTC+12" },
        //            { "Europe/Amsterdam", "W. Europe Standard Time" },
        //            { "Europe/Athens", "GTB Standard Time" },
        //            { "Europe/Belgrade", "Central Europe Standard Time" },


        //            { "Europe/Warsaw", "Central European Standard Time" },
        //            { "Indian/Mauritius", "Mauritius Standard Time" },
        //            { "Pacific/Apia", "Samoa Standard Time" },
        //            { "Pacific/Auckland", "New Zealand Standard Time" },
        //            { "Pacific/Fiji", "Fiji Standard Time" },
        //            { "Pacific/Guadalcanal", "Central Pacific Standard Time" },
        //            { "Pacific/Guam", "West Pacific Standard Time" },
        //            { "Pacific/Honolulu", "Hawaiian Standard Time" },
        //            { "Pacific/Pago_Pago", "UTC-11" },
        //            { "Pacific/Port_Moresby", "West Pacific Standard Time" },
        //            { "Pacific/Tongatapu", "Tonga Standard Time" }
        //        };

        //    var windowsTimeZoneId = default(string);
        //    var windowsTimeZone = default(TimeZoneInfo);
        //    if (olsonWindowsTimes.TryGetValue(timezoneLocal, out windowsTimeZoneId))
        //    {
        //        try { windowsTimeZone = TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZoneId); }
        //        catch (TimeZoneNotFoundException) { }
        //        catch (InvalidTimeZoneException) { }
        //    }
        //    if (windowsTimeZone != null)
        //    {
        //        string a = windowsTimeZone.StandardName;
        //        return a;
        //    }
        //    return null;
        //}
    }
    public static class ObjectExtensionMethods
    {
        public static bool IsThisNull(this object value)
        {
            return value == null;
        }
    }
    /// <summary>
    /// Generic Extension methods
    /// </summary>
    public static partial class GenericExtensions
    {
        /// <summary>
        /// Returns the Default value if the current Object if it is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="DefaultValue">Default value for the Object if it is null. If it is a string it will check for null or empty or whitespace</param>
        /// <returns></returns>
        public static T Default<T>(this T value, T defaultValue)
        {
            if (value == null)
            {
                return defaultValue;
            }

            if (typeof(T) == typeof(String))
            {
                if (String.IsNullOrWhiteSpace((value as String)))
                {
                    return defaultValue;
                }
            }
            return value;
        }





    }
}
