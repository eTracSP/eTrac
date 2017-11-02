using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class ReportModel
    {
        public string ReportName { get; set; }
    }
    public class CleaningModel
    {
        public string ItemName { get; set; }
        public int NoCleaning { get; set; }
        public long QRCID { get; set; }
        public string QRCType { get; set; }
    }
    public class TrashLevelChartModel
    {
        public string TrashTitle { get; set; }
        public string TrashLevelTitle { get; set; }
        public List<TrashData> lsttrashData { get; set; }
        public TrashData trashData { get; set; }
       
    }
    public class WorkOrderIssueedModel
    {
        public string TaskName { get; set; }
        public string TaskPriority { get; set; }
        public string AssignedToUser { get; set; }
        public string RequestedBy { get; set; }
        public string IssuedBy { get; set; }
        public string IssuedDate { get;set;}

        public string QRCName { get; set; }
        public string QRCTYPE1 { get; set; }
        public string WorkRequestType { get; set; }
        public string ProblemDesc { get; set; }
        public string PriorityLevel { get; set; }
        public string ProjectDesc { get; set; }
        public string WorkRequestStatus { get; set; }
        public string RequestBy { get; set; }
        public string AssignTo { get; set; }
        public string AssignBy { get; set; }
        public string CreatedDate { get; set; }
        public Nullable<DateTime> CreatedDateTM { get; set; }
        public Nullable<DateTime> AssignedTimeDateTime { get; set; }
        public string AssignedTime { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string FixedTime { get; set; }
        public string MissedTime { get; set; }
        public string CodeID { get; set; }
    }
    public class TrashData
    {
        public string TrashName { get; set; }
        public string TrashLevel { get; set; }
    }

    //For Test
    public class ProductModel
    {
        public string YearTitle { get; set; }
        public string SaleTitle { get; set; }
        public string PurchaseTitle { get; set; }
        public Product ProductData { get; set; }
    }
    //For Test
    public class Product
    {
        public string Year { get; set; }
        public string Purchase { get; set; }
        public string Sale { get; set; }
    }
 


 

}
