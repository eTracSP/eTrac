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
    
    public partial class ssp_GetQRCDetails_Result
    {
        public Nullable<long> RowNum { get; set; }
        public long QRCID { get; set; }
        public string QRCName { get; set; }
        public string SpecialNotes { get; set; }
        public long QRCDefaultSize { get; set; }
        public long QRCTYPE { get; set; }
        public string ModuleName { get; set; }
        public Nullable<long> ProjectCategoryId { get; set; }
        public string ProjectCategory { get; set; }
        public string WarrantyDoc { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string QRCodeID { get; set; }
        public Nullable<bool> IsDamage { get; set; }
        public Nullable<bool> IsDamageVerified { get; set; }
        public Nullable<bool> CheckOutStatus { get; set; }
        public string Created_By { get; set; }
        public string LocationName { get; set; }
        public string QRCSize { get; set; }
    }
}