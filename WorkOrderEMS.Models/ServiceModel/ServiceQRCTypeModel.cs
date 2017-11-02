using System;
using System.Runtime.Serialization;

namespace WorkOrderEMS.Models
{
    #region For QRCID Details
    [DataContract]
    public class ServiceQrcModel
    {
        [DataMember]
        public string ServiceAuthKey { get; set; }
        [DataMember]
        public long QrcId { get; set; }
        [DataMember]
        public long LocationId { get; set; }
        [DataMember]
        public string QrcName { get; set; }
        [DataMember]
        public string SpecialNotes { get; set; }
        [DataMember]
        public long QRCDefaultSize { get; set; }
        [DataMember]
        public long QrcType { get; set; }
        [DataMember]
        public Nullable<long> ProjectId { get; set; }
        [DataMember]
        public long ModuleNameId { get; set; }
        [DataMember]
        public Nullable<long> ProjectCategoryId { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public Nullable<long> MotorType { get; set; }
        [DataMember]
        public Nullable<long> VendorId { get; set; }
        [DataMember]
        public Nullable<long> ClientTypeId { get; set; }
        [DataMember]
        public Nullable<long> VehicleType { get; set; }
        [DataMember]
        public string QrCodeId { get; set; }
        [DataMember]
        public string ModelNo { get; set; }
        [DataMember]
        public string SerialNo { get; set; }
        [DataMember]
        public string Make { get; set; }
        [DataMember]
        public string Model { get; set; }
        [DataMember]
        public string AssetPicture { get; set; }
        [DataMember]
        public string LocationPicture { get; set; }
        [DataMember]
        public string VendorName { get; set; }
        [DataMember]
        public string PointOfContact { get; set; }
        [DataMember]
        public string TelephoneNo { get; set; }
        [DataMember]
        public string EmialAdd { get; set; }
        [DataMember]
        public string WarrantyEndDate { get; set; }
        [DataMember]
        public string Website { get; set; }
        [DataMember]
        public Nullable<long> PurchaseType { get; set; }
        [DataMember]
        public string WarrantyDoc { get; set; }
        [DataMember]
        public string InsuranceExpDate { get; set; }
        [DataMember]
        public string PurchaseTypeRemark { get; set; }
        [DataMember]
        public string QrcTypeDetails { get; set; }
        [DataMember]
        public Nullable<long> QrcScanLogId { get; set; }

        [DataMember]
        public string CheckoutUserName { get; set; }
        [DataMember]
        public string NewCheckoutUserName { get; set; }
        [DataMember]
        public long UserId { get; set; }

    }
    #endregion For QRCID Details

    #region For QRC Elevator
    [DataContract]
    public class ServiceQrcElevatorModel
    {
        [DataMember]
        public string ServiceAuthKey { get; set; }

        [DataMember]
        public long UserId { get; set; }

        [DataMember]
        public long QrcId { get; set; }
        [DataMember]
        public RoutineCheck Routine { get; set; }

        [DataMember]
        public Cleaning Cleaning { get; set; }

        [DataMember]
        public PMCheck PmCheck { get; set; }
        [DataMember]
        public long LocationId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string QrcodeId { get; set; }


        [DataMember]
        public string Action { get; set; }
    }
    #endregion For QRC Elevator

    #region For QRC BathRoom
    [DataContract]
    public class ServiceQrcBathroomModel
    {
        [DataMember]
        public string ServiceAuthKey { get; set; }

        [DataMember]
        public long UserId { get; set; }

        [DataMember]
        public long QrcId { get; set; }

        [DataMember]
        public BathroomPmCheck PmCheck { get; set; }

        [DataMember]
        public BathroomCleaning Cleaning { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public int WorkOrderRequestId { get; set; }
    }
    #endregion For QRC BathRoom

    #region For QRC Equipment
    [DataContract]
    public class ServiceQrcEquipmentModel
    {
        [DataMember]
        public string ServiceAuthKey { get; set; }

        [DataMember]
        public long UserId { get; set; }

        [DataMember]
        public long QrcId { get; set; }

        [DataMember]
        public bool CheckingIn { get; set; }

        [DataMember]
        public string CheckingInDescription { get; set; }

        [DataMember]
        public bool CheckingOut { get; set; }

        [DataMember]
        public string CheckingOutDescription { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public bool CheckOutStatus { get; set; }

        [DataMember]
        public string UserName { get; set; }
    }
    #endregion For QRC Equipment

    #region For QRC Cellphone
    [DataContract]
    public class ServiceQrcCellphoneModel
    {
        [DataMember]
        public string ServiceAuthKey { get; set; }

        [DataMember]
        public long UserId { get; set; }

        [DataMember]
        public long QrcId { get; set; }

        [DataMember]
        public CellphoneCheckingIn CheckingIn { get; set; }

        [DataMember]
        public CellphoneCheckingOut CheckingOut { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public bool Status { get; set; }
        [DataMember]
        public string UserName { get; set; }

    }
    #endregion For QRC Cellphone

    #region For QRC Escalators
    [DataContract]
    public class ServiceQrcEscalatorsModel
    {
        [DataMember]
        public string ServiceAuthKey { get; set; }

        [DataMember]
        public long UserId { get; set; }

        [DataMember]
        public long QrcId { get; set; }
        [DataMember]
        public EscalatorsRoutineCheck Routine { get; set; }

        [DataMember]
        public EscalatorsCleaning Cleaning { get; set; }

        [DataMember]
        public EscalatorsPMCheck PmCheck { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public int WorkOrderRequestId { get; set; }
    }
    #endregion For QRC Escalators

    #region For QRC MovingWalkway
    [DataContract]
    public class ServiceQrcMovingWalkwayModel
    {
        [DataMember]
        public string ServiceAuthKey { get; set; }

        [DataMember]
        public long UserId { get; set; }

        [DataMember]
        public long QrcId { get; set; }
        [DataMember]
        public MovingWalkwayRoutineCheck Routine { get; set; }

        [DataMember]
        public MovingWalkwayCleaning Cleaning { get; set; }

        [DataMember]
        public MovingWalkwayPMCheck PmCheck { get; set; }

        [DataMember]
        public int WorkOrderRequestId { get; set; }

        [DataMember]
        public string Action { get; set; }
    }
    #endregion For QRC MovingWalkway

    #region For QRC ParkingFacility
    [DataContract]
    public class ServiceQrcParkingModel
    {
        [DataMember]
        public string ServiceAuthKey { get; set; }

        [DataMember]
        public long UserId { get; set; }

        [DataMember]
        public long QrcId { get; set; }

        [DataMember]
        public bool Routine { get; set; }

        [DataMember]
        public string RoutineDescription { get; set; }

        [DataMember]
        public ParkingCleaning Cleaning { get; set; }

        [DataMember]
        public bool Maintenance { get; set; }

        [DataMember]
        public bool CashPickup { get; set; }

        [DataMember]
        public string CashPickupDescription { get; set; }

        [DataMember]
        public bool Assistance { get; set; }

        [DataMember]
        public PreventativeMaintenance PreventativeMaintenance { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public CustomerAssistance CustomerAssistance { get; set; }
    }
    #endregion For QRC ParkingFacility

    #region For QRC TrashCan
    [DataContract]
    public class ServiceQrcTrashcanModel
    {
        [DataMember]
        public string ServiceAuthKey { get; set; }

        [DataMember]
        public long UserId { get; set; }

        [DataMember]
        public long QrcId { get; set; }

        [DataMember]
        public TrashcanRoutineCheck Routine { get; set; }

        [DataMember]
        public TrashcanTrashRemoval TrashRemoval { get; set; }

        [DataMember]
        public string Action { get; set; }
        [DataMember]
        public bool IsReported { get; set; }
        [DataMember]
        public string QRCName { get; set; }
    }
    #endregion For QRC TrashCan

    #region For QRC GateArm
    [DataContract]
    public class ServiceQrcGateArmModel
    {
        [DataMember]
        public string ServiceAuthKey { get; set; }

        [DataMember]
        public long UserId { get; set; }

        [DataMember]
        public long QrcId { get; set; }

        [DataMember]
        public GateArmCleaning Cleaning { get; set; }

        [DataMember]
        public GateArmPMCheck PmCheck { get; set; }
        [DataMember]
        public int WorkOrderRequestId { get; set; }

        [DataMember]
        public string Action { get; set; }
    }
    #endregion For QRC GateArm

    #region For QRC TicketSpitter
    [DataContract]
    public class ServiceQrcTicketSplitterModel
    {
        [DataMember]
        public string ServiceAuthKey { get; set; }

        [DataMember]
        public long UserId { get; set; }

        [DataMember]
        public long QrcId { get; set; }

        [DataMember]
        public TicketSplitterCleaning Cleaning { get; set; }

        [DataMember]
        public TicketSplitterPMCheck PmCheck { get; set; }
        [DataMember]
        public int WorkOrderRequestId { get; set; }

        [DataMember]
        public string Action { get; set; }
    }
    #endregion For QRC TicketSpitter

    #region For QRC BusStation
    [DataContract]
    public class ServiceQrcBusStationModel
    {
        [DataMember]
        public string ServiceAuthKey { get; set; }

        [DataMember]
        public long UserId { get; set; }

        [DataMember]
        public long QrcId { get; set; }

        [DataMember]
        public BusStationCleaning Cleaning { get; set; }

        [DataMember]
        public BusStationPmCheck PmCheck { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public int WorkOrderRequestId { get; set; }
    }
    #endregion For QRC BusStation

    #region For QRC Emergency Phone Systems
    [DataContract]
    public class ServiceQrcPhoneSystemModel
    {
        [DataMember]
        public string ServiceAuthKey { get; set; }

        [DataMember]
        public long UserId { get; set; }

        [DataMember]
        public long QrcId { get; set; }

        [DataMember]
        public PhoneSystemCleaning Cleaning { get; set; }

        [DataMember]
        public PhoneSystemPMCheck PmCheck { get; set; }

        [DataMember]
        public int WorkOrderRequestId { get; set; }

        [DataMember]
        public string Action { get; set; }
    }
    #endregion For QRC Emergency Phone Systems

    #region For QRC Vehicle
    [DataContract]
    public class ServiceQrcVehicleModel
    {
        [DataMember]
        public string ServiceAuthKey { get; set; }
        [DataMember]
        public long UserId { get; set; }
        [DataMember]
        public long QrcId { get; set; }
        [DataMember]
        public VehicleCheckingOut CheckingOut { get; set; }
        [DataMember]
        public VehicleCheckingIn CheckingIn { get; set; }
        [DataMember]
        public VehicleFuel VehicleFuel { get; set; }
        [DataMember]
        public VehicleCleaning Cleaning { get; set; }
        [DataMember]
        public WeeklyVehicleCheck VehicleCheck { get; set; }
        [DataMember]
        public string Action { get; set; }
        [DataMember]
        public long LocationId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string QrcodeId { get; set; }
        [DataMember]
        public string AllPictures { get; set; }
        [DataMember]
        public bool DamageStatus { get; set; }

    }
    #endregion For QRC Vehicle

    #region For QRC Shuttle Bus
    [DataContract]
    public class ServiceQrcShuttleBusModel
    {
        [DataMember]
        public string ServiceAuthKey { get; set; }
        [DataMember]
        public long UserId { get; set; }
        [DataMember]
        public long QrcId { get; set; }
        [DataMember]
        public long LocationId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string QrcodeId { get; set; }
        [DataMember]
        public string Action { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public bool DamageStatus { get; set; }
        [DataMember]
        public ShuttleDamage Damage { get; set; }
        [DataMember]
        public ShuttleMileage Mileage { get; set; }
        [DataMember]
        public Engine Engine { get; set; }
        [DataMember]
        public Interior Interior { get; set; }
        [DataMember]
        public Exterior Exterior { get; set; }
        [DataMember]
        public Tires Tires { get; set; }
        [DataMember]
        public DriversCabin DriversCabin { get; set; }
        [DataMember]
        public Accessories Accessories { get; set; }
        [DataMember]
        public Emergency Emergency { get; set; }

        [DataMember]
        public string AllPictures { get; set; }
        [DataMember]
        public int WorkOrderRequestId { get; set; }


    }
    #endregion For QRC Shuttle Bus

    public class ServiceQrcTrashcanModelTemp
    {
        public ServiceQrcTrashcanModel ServiceQrcTrashcanProp { get; set; }
        public long QrclogId { get; set; }
        public string QrcName { get; set; }
        public string QrCodeId { get; set; }
    }

    public class ServiceQrcVehicleModelTemp
    {
        public ServiceQrcVehicleModel ServiceQrcVehicleProp { get; set; }
        public long QrclogId { get; set; }
        public string QrcName { get; set; }
    }

}
