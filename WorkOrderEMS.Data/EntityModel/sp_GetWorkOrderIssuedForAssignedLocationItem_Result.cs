//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorkOrderEMS.Data.EntityModel
{
    using System;
    
    public partial class sp_GetWorkOrderIssuedForAssignedLocationItem_Result
    {
        public long WorkRequestAssignmentID { get; set; }
        public Nullable<long> assetid { get; set; }
        public string QRCName { get; set; }
        public string QRCTYPE { get; set; }
        public string WorkRequestType { get; set; }
        public string ProblemDesc { get; set; }
        public string PriorityLevel { get; set; }
        public string ProjectDesc { get; set; }
        public string WorkRequestStatus { get; set; }
        public string RequestBy { get; set; }
        public string AssignTo { get; set; }
        public string AssignBy { get; set; }
        public string CreatedDate { get; set; }
        public string CodeID { get; set; }
        public long createdBy { get; set; }
    }
}
