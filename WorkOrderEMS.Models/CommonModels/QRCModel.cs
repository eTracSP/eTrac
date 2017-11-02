using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Runtime.Serialization;
using System.Web;


namespace WorkOrderEMS.Models
{
    [DataContract]
    public class QRCModel
    {
        [DataMember]
        public long QRCId { get; set; }

        [Required(ErrorMessage = "test")]
        public long Administrator { get; set; }

        [Required(ErrorMessage = "test")]
        public long Location { get; set; }

        public long MotorTypeVehicle { get; set; }
        public string EncryptQRC { get; set; }
        public string EncryptLastQRC { get; set; }

        public List<GlobalCodeModel> JobTitleList { get; set; }
        [DataMember]
        [Required(ErrorMessage = "QRC Name is required")]
        [Display(Name = "QRC Name")]
        public string QRCName { get; set; }

        [RegularExpression(@"^(?![\W_]+$)(?!\d+$)[a-zA-Z0-9 .&',_-]+$", ErrorMessage = "Special characters are not allowed.")]
        [DataMember]
        [Display(Name = "Special Notes")]
        public string SpecialNotes { get; set; }
        [DataMember]
        [Display(Name = "Vehicle Image")]
        public string VehicleImage { get; set; }
        [DataMember]
        [Display(Name = "QRC Default Size")]
        public long QRCDefaultSize { get; set; }
        [DataMember]
        public string SizeCaption { get; set; }
        [DataMember]
        [Display(Name = "Vehicle Type")]
        public long? VehicleType { get; set; }
        [DataMember]
        public string VehicleTypeCaption { get; set; }
        [DataMember]
        [Display(Name = "Motor Vehicle Type")]
        public long? MotorType { get; set; }
        [DataMember]
        public string MotorTypeCaption { get; set; }

        [DataMember]
        [Display(Name = "QRC Type")]
        public long QRCTYPE { get; set; }
        [DataMember]
        public string QRCTYPECaption { get; set; }
        [DataMember]
        public Nullable<long> VendorID { get; set; }
        [DataMember]
        public long? ClientTypeID { get; set; }
        //public bool IsVendor { get; set; }
        public long OtherQRCID { get; set; }
        [DataMember]
        public Nullable<bool> CheckOutStatus { get; set; }
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public Nullable<bool> CellPhoneCheckOutStatus { get; set; }

        //Driver
        public string DriverImage { get; set; }
        public string DriverName { get; set; }

        public VehicleModel VehicleModel { get; set; }
        public UserModel UserModel { get; set; }

        public bool UpdateMode { get; set; }

        public List<LocationListModel> LocationList { get; set; }

        public List<GlobalCodeModel> QRCTypeList { get; set; }
        public List<GlobalCodeModel> QRCSize { get; set; }
        public List<GlobalCodeModel> PurchaseTypeList { get; set; }


        public List<GlobalCodeModel> VehicleTypeList { get; set; }
        public List<GlobalCodeModel> MotorTypeList { get; set; }
        [DataMember]
        public string QRCodeID { get; set; }
        [DataMember]
        [Display(Name = "Serial No")]
        public string SerialNo { get; set; }
        [DataMember]
        public string Make { get; set; }
        [DataMember]
        public string Model { get; set; }
        [DataMember]
        [Display(Name = "Asset Picture")]
        public string AssetPicture { get; set; }
        public HttpPostedFileBase AssetPictureUrl { get; set; }
        [DataMember]
        [Display(Name = "Location Picture")]
        public string LocationPicture { get; set; }
        public HttpPostedFileBase LOCPicture { get; set; }
        [DataMember]
        [Display(Name = "Vendor Name")]
        public string VendorName { get; set; }
        [DataMember]
        [Display(Name = "Point Of Contact")]
        public string PointOfContact { get; set; }
        [DataMember]
        [Display(Name = "Telephone No")]
        public string TelephoneNo { get; set; }

        [DataMember]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmialAdd { get; set; }
        [Display(Name = "Warranty End Date")]
        public Nullable<System.DateTime> WarrantyEndDate { get; set; }

        [RegularExpression(@"(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ;,./?%&=]*)?", ErrorMessage = "Invalid URL")]
        public string Website { get; set; }

        [Display(Name = "Purchase Type")]
        public Nullable<long> PurchaseType { get; set; }
        public HttpPostedFileBase WarrantyDocument { get; set; }

        [Display(Name = "WarrantyDoc")]
        public string WarrantyDoc { get; set; }

        [Display(Name = "Expiration")]
        public Nullable<System.DateTime> InsuranceExpDate { get; set; }
        [Display(Name = "Remark")]
        public string PurchaseTypeRemark { get; set; }

        /*Added by Bhushan Dod On 05-02-2015*/
        [DataMember]
        public string QRCTypeDetails { get; set; }
        [DataMember]
        public string AssetPicturePath { get; set; }
        [DataMember]
        public string LocationPicturePath { get; set; }
        [DataMember]
        public string WarrantyDocumentPath { get; set; }
        [DataMember]
        public Nullable<long> Allotedto { get; set; }
        [DataMember]
        public string LocationName { get; set; }

        [DataMember]
        public Nullable<long> LocationId { get; set; }

        public long CreatedBy { get; set; }

        public string CreatedOn { get; set; }
        public System.DateTime CreatedDate { get; set; }

        [DataMember]
        public string WExpDate { get; set; }
        [DataMember]
        public string IExpDate { get; set; }
        //Added By Bhushan Dod on 03/25/2015 for Mileage check in mobile app
        [DataMember]
        public string ChkOutVmDescription { get; set; }
        [DataMember]
        public string ChkInVmDescription { get; set; }
        [DataMember]
        public string VfVmDescription { get; set; }
        [DataMember]
        public string WVmDescription { get; set; }
        [DataMember]
        public int FuelLevel { get; set; }
        [DataMember]
        public int WeeklyFuelLevel { get; set; }
        //Added By Bhushan Dod on 21/04/2015 for save dar while scan and scan log id
        [DataMember]
        public Nullable<long> QrcScanLogId { get; set; }
        [DataMember]
        public Nullable<long> DarId { get; set; }

        //Added By Bhushan Dod on 04/29/2015 for Trash Level check in mobile app
        [DataMember]
        public int RoutineTrashLevels { get; set; }
        [DataMember]
        public int RemovalTrashLevels { get; set; }

        public string ServicesID { get; set; }
        [Required(ErrorMessage = "The Selected UserType field is required.")]
        public long SelectedUserType { get; set; }
        public long QRCTYPEID { get; set; }

        //Added By Bhushan Dod on 21/November/2016 for to avoid checkIn/Out if QRC is damage.
        [DataMember]
        public Nullable<bool> IsDamage { get; set; }
        [DataMember]
        public Nullable<bool> IsDamageVerified { get; set; }
        public string QRCSizeGenerate { get; set; }
        public string QRCImage { get; set; }
        public string QRCImageBase64 { get; set; }
    }



    //public class ParentModel
    //{
    //    public QRCModel myQRCModel { get; set; }
    //    public LocationMasterModel myLocationMaster { get; set; }

    //}


    public class VehiclesModel
    {
        public VehicleModel VehicleModel { get; set; }
    }
    public class QRCListModel
    {
        public string EncryptQRC { get; set; }

        [Display(Name = "QRC Name")]
        public string QRCName { get; set; }

        [Display(Name = "Special Notes")]
        public string SpecialNotes { get; set; }

        [Display(Name = "QRC Type")]
        public string QRCTYPE { get; set; }

        public long QRCTYPEId { get; set; }

        public string ItemName { get; set; }

        public string VehicleNumber { get; set; }

        public string WarrentyDoc { get; set; }
        public string QRCodeID { get; set; }
        public Nullable<bool> IsDamage { get; set; }
        public Nullable<bool> IsDamageVerified { get; set; }
        public Nullable<bool> CheckOutStatus { get; set; }
        public string LocationName { get; set; }
        public string QRCSize { get; set; }
    }

}


