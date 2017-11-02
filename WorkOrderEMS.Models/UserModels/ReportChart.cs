using System;
using System.Collections.Generic;

namespace WorkOrderEMS.Models.UserModels
{
    public class ReportChart
    {
        public long QrcType { get; set; }
        public string QrcTypeName { get; set; }
        public int QrcTypeCount { get; set; }
        public string QrcName { get; set; }
        public int QrcNameCount { get; set; }
        public string QrCodeId { get; set; }
        public long ScanId { get; set; }
        public long ScanUserId { get; set; }
        public int ScanUserCount { get; set; }
        public string ScanUserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string StrCreatedDate { get; set; }

        public string KeyName { get; set; }
        public List<TrashData1> ValueUser { get; set; }

        public string CroppedPicture { get; set; }
        public string CapturedPicture { get; set; }
        public string CroppedPicture1 { get; set; }
        public string CapturedPicture1 { get; set; }
        public string CroppedPicture2 { get; set; }
        public string CapturedPicture2 { get; set; }
        public string CroppedPicture3 { get; set; }
        public string CapturedPicture3 { get; set; }
        public string CroppedPicture4 { get; set; }
        public string CapturedPicture4 { get; set; }

        public string WorkOrderCode { get; set; }
        public string ProblemDescription { get; set; }
        public string ProjectDescription { get; set; }

        public string Description { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string StartTimeImage { get; set; }
        public string StrStartTime { get; set; }
        public string StrEndTime { get; set; }
        public DateTime CompletedDate { get; set; }
        public string StrCompletedDate { get; set; }
        public string LocationName { get; set; }
        public string CleaningDescription { get; set; }
        public string RoutineDescription { get; set; }

    }
    public class TrashData1
    {
        public long ReportCount { get; set; }
        public string ReportUserName { get; set; }
    }

    public class DamageReport
    {
        public ReportChart WeeklyCheckOut { get; set; }
        public ReportChart DailyCheckOut { get; set; }
    }

    //shubham 07112016
    public class ReportChartForPDFExport
    {
        public long QrcType { get; set; }
        public string QRCType { get; set; }
        public int QrcTypeCount { get; set; }
        public string QRCName { get; set; }

        public string QRCCode { get; set; }
        public long ScanUserId { get; set; }
        public int ScanUserCount { get; set; }
        public string ReportBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        public string ReportDate { get; set; }

        public string KeyName { get; set; }
        public List<TrashData1> ValueUser { get; set; }

        public string CroppedPicture { get; set; }
        public string CapturedPicture { get; set; }
        public string Damage1 { get; set; }
        public string Capture1 { get; set; }
        public string Damage2 { get; set; }
        public string Capture2 { get; set; }
        public string Damage3 { get; set; }
        public string Capture3 { get; set; }
        public string Damage4 { get; set; }
        public string Capture4 { get; set; }

        public string WorkOrderCode { get; set; }
        public string ProblemDescription { get; set; }
        public string ProjectDescription { get; set; }

        public string Description { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string StartTimeImage { get; set; }
        public string StrStartTime { get; set; }
        public string StrEndTime { get; set; }
        public DateTime CompletedDate { get; set; }
        public string StrCompletedDate { get; set; }
        public string LocationName { get; set; }

        public string QRCCodeID { get; set; }
        public string SpecialNotes { get; set; }
        public string AssetPicture { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }

        public string UserName { get; set; }
        public string DARCode { get; set; }
        public string ScanDate { get; set; }

    }
    public class SortInfo
    {
        public string sortCol { get; set; }
        public string sortDir { get; set; }
    }
}
