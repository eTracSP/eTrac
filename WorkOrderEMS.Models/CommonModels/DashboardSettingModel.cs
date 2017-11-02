using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.CommonModels
{
    public class DashboardSettingModel
    {
        public long DisplayID { get; set; }
        public long UserID { get; set; }
        public string DisplaySettings { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    
    }
    public class TimeZoneInfoModel
    {
        public string TimeZoneName { get; set; }
        public string GMT { get; set; }
        public Nullable<System.DateTime> IndianStandardTime { get; set; }
        public System.DateTime SQLServerTime { get; set; }
       public TimeZone zone { get; set; }
        public TimeSpan offset { get; set; }
        public DaylightTime time { get; set; }
        public string standard { get; set; }
        public string daylight { get; set; }
    }
    public class Widget
    {   
        public long WidgetID { get; set; }
        public string WidgetName { get; set; }

    }
    public class WidgetList
    {
        public List<Widget> CheckedList { get; set; }
        public List<Widget> AllWidgetList { get; set; }
    }
    public class DashboardWidgetSettingModel
    {
        public long WidgetID { get; set; }
        public long UserID { get; set; }
        public long LocationId { get; set; }
    }
}
