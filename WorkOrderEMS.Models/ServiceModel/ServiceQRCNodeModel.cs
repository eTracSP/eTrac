using System.Runtime.Serialization;

namespace WorkOrderEMS.Models
{

    #region Elevator

    [DataContract, KnownType(typeof(ServiceQrcElevatorModel))]
    public class RoutineCheck
    {
        [DataMember]
        public bool Clean { get; set; }

        [DataMember]
        public int CleanWorkOrderId { get; set; }
        [DataMember]
        public string RoutineDescription { get; set; }
    }

    [DataContract, KnownType(typeof(ServiceQrcElevatorModel))]
    public class Cleaning
    {
        [DataMember]
        public bool IsClean { get; set; }

        [DataMember]
        public string IsCleanDescription { get; set; }
        [DataMember]
        public string CleanDescription { get; set; }
    }

    [DataContract, KnownType(typeof(ServiceQrcElevatorModel))]
    public class PMCheck
    {
        [DataMember]
        public bool Inspection { get; set; }
        [DataMember]
        public int TeleSystem { get; set; }
        [DataMember]
        public int TeleSystemWorkOrderId { get; set; }
        [DataMember]
        public bool Capacity { get; set; }
        [DataMember]
        public int Lighting { get; set; }
        [DataMember]
        public int LightingWorkOrderId { get; set; }
        [DataMember]
        public int Door { get; set; }
        [DataMember]
        public int DoorWorkOrderId { get; set; }
        [DataMember]
        public int DoorSensor { get; set; }
        [DataMember]
        public int DoorSensorWorkOrderId { get; set; }
        [DataMember]
        public int Indicator { get; set; }
        [DataMember]
        public int IndicatorWorkOrderId { get; set; }
        [DataMember]
        public int CallButton { get; set; }
        [DataMember]
        public int CallButtonWorkOrderId { get; set; }
        [DataMember]
        public int CarRide { get; set; }
        [DataMember]
        public int CarRideWorkOrderId { get; set; }
    }
    #endregion Elevator

    #region BathRoom
    [DataContract, KnownType(typeof(ServiceQrcBathroomModel))]
    public class BathroomPmCheck
    {
        [DataMember]
        public bool Clean { get; set; }

        [DataMember]
        public int CleanWorkOrderId { get; set; }
    }

    [DataContract, KnownType(typeof(ServiceQrcBathroomModel))]
    public class BathroomCleaning
    {
        [DataMember]
        public bool IsClean { get; set; }

        [DataMember]
        public int CleanDarId { get; set; }
        [DataMember]
        public string CleanDescription { get; set; }
    }

    #endregion BathRoom

    #region CellPhone
    [DataContract, KnownType(typeof(ServiceQrcCellphoneModel))]
    public class CellphoneCheckingIn
    {
        [DataMember]
        public bool FullyFunctional { get; set; }

        [DataMember]
        public int FullyFunctionalWorkOrderId { get; set; }

        [DataMember]
        public bool ScreenCracked { get; set; }

        [DataMember]
        public int ScreenCrackedWorkOrderId { get; set; }

        [DataMember]
        public bool Buttons { get; set; }

        [DataMember]
        public int ButtonsWorkOrderId { get; set; }
    }

    [DataContract, KnownType(typeof(ServiceQrcCellphoneModel))]
    public class CellphoneCheckingOut
    {
        [DataMember]
        public bool FullyFunctional { get; set; }

        [DataMember]
        public int FullyFunctionalWorkOrderId { get; set; }

        [DataMember]
        public bool ScreenCracked { get; set; }

        [DataMember]
        public int ScreenCrackedWorkOrderId { get; set; }

        [DataMember]
        public bool Buttons { get; set; }

        [DataMember]
        public int ButtonsWorkOrderId { get; set; }
    }

    #endregion CellPhone

    #region Escalators

    [DataContract, KnownType(typeof(ServiceQrcEscalatorsModel))]
    public class EscalatorsRoutineCheck
    {
        [DataMember]
        public bool IsClean { get; set; }

        [DataMember]
        public int CleanWorkOrderId { get; set; }
        [DataMember]
        public string RoutineDescription { get; set; }
    }

    [DataContract, KnownType(typeof(ServiceQrcEscalatorsModel))]
    public class EscalatorsCleaning
    {
        [DataMember]
        public bool IsClean { get; set; }

        [DataMember]
        public int CleanDarId { get; set; }
        [DataMember]
        public string CleanDescription { get; set; }
    }
    [DataContract, KnownType(typeof(ServiceQrcEscalatorsModel))]
    public class EscalatorsPMCheck
    {
        [DataMember]
        public bool Operational { get; set; }
        [DataMember]
        public int OperationalWorkOrderId { get; set; }
        [DataMember]
        public int SpeedAcross { get; set; }
        [DataMember]
        public int Handrail { get; set; }
        [DataMember]
        public int HandrailWorkOrderId { get; set; }
        [DataMember]
        public bool IsBumpsErratic { get; set; }
        [DataMember]
        public int BumpsErraticWorkOrderId { get; set; }
    }
    #endregion Escalators

    #region MovingWalkway

    [DataContract, KnownType(typeof(ServiceQrcMovingWalkwayModel))]
    public class MovingWalkwayRoutineCheck
    {
        [DataMember]
        public bool IsClean { get; set; }

        [DataMember]
        public int CleanWorkOrderId { get; set; }
        [DataMember]
        public string RoutineDescription { get; set; }

    }

    [DataContract, KnownType(typeof(ServiceQrcMovingWalkwayModel))]
    public class MovingWalkwayCleaning
    {
        [DataMember]
        public bool IsClean { get; set; }

        [DataMember]
        public int CleanDarId { get; set; }
        [DataMember]
        public string CleanDescription { get; set; }
    }
    [DataContract, KnownType(typeof(ServiceQrcMovingWalkwayModel))]
    public class MovingWalkwayPMCheck
    {
        [DataMember]
        public bool Operational { get; set; }
        [DataMember]
        public int OperationalWorkOrderId { get; set; }
        [DataMember]
        public int SpeedAcross { get; set; }
        [DataMember]
        public int Handrail { get; set; }
        [DataMember]
        public int HandrailWorkOrderId { get; set; }
        [DataMember]
        public bool ErraticMovement { get; set; }
        [DataMember]
        public int ErraticMovWorkOrderId { get; set; }
    }
    #endregion MovingWalkway

    #region ParkingFacility

    [DataContract, KnownType(typeof(ServiceQrcMovingWalkwayModel))]
    public class ParkingRoutineCheck
    {
        [DataMember]
        public bool Clean { get; set; }

        [DataMember]
        public string CleanDescription { get; set; }

    }

    [DataContract, KnownType(typeof(ServiceQrcMovingWalkwayModel))]
    public class    ParkingCleaning
    {
        [DataMember]
        public bool IsClean { get; set; }
        [DataMember]
        public int FacilityValue { get; set; }
        [DataMember]
        public string FacilityValueDescription { get; set; }
        [DataMember]
        public string CleanDescription { get; set; }
    }
    [DataContract, KnownType(typeof(ServiceQrcMovingWalkwayModel))]
    public class PreventativeMaintenance
    {
        [DataMember]
        public bool LightCheck { get; set; }
        [DataMember]
        public string LightCheckDescription { get; set; }
        [DataMember]
        public bool LineStripSignage { get; set; }
        [DataMember]
        public int LineStripSignageWorkId { get; set; }
    }
    [DataContract, KnownType(typeof(ServiceQrcMovingWalkwayModel))]
    public class CustomerAssistance
    {
        [DataMember]
        public int AssistanceType { get; set; }
        [DataMember]
        public string AssistanceComment { get; set; }

    }
    #endregion ParkingFacility

    #region TrashCan

    [DataContract, KnownType(typeof(ServiceQrcTrashcanModel))]
    public class TrashcanRoutineCheck
    {
        [DataMember]
        public bool IsClean { get; set; }
        [DataMember]
        public bool IsCleanWorkOrder { get; set; }
        [DataMember]
        public int CleanWorkOrderId { get; set; }
        [DataMember]
        public int TrashLevels { get; set; }
        [DataMember]
        public bool IsTrashLevelsWorkOrder { get; set; }
        [DataMember]
        public int TrashLevelsWorkOrderId { get; set; }
        [DataMember]
        public int OldTrashLevels { get; set; }
        [DataMember]
        public string RoutineDescription { get; set; }

    }
    [DataContract, KnownType(typeof(ServiceQrcTrashcanModel))]
    public class TrashcanTrashRemoval
    {
        [DataMember]
        public int TrashLevels { get; set; }
        [DataMember]
        public bool IsTrashLevelsWorkOrder { get; set; }
        [DataMember]
        public int TrashLevelsWorkOrderId { get; set; }
        [DataMember]
        public int OldTrashLevels { get; set; }

    }
    #endregion TrashCan

    #region GateArm
    [DataContract, KnownType(typeof(ServiceQrcMovingWalkwayModel))]
    public class GateArmCleaning
    {
        [DataMember]
        public bool IsClean { get; set; }

        [DataMember]
        public int CleanDarId { get; set; }
        [DataMember]
        public string CleanDescription { get; set; }
    }
    [DataContract, KnownType(typeof(ServiceQrcMovingWalkwayModel))]
    public class GateArmPMCheck
    {
        [DataMember]
        public int ArmRaise { get; set; }
        [DataMember]
        public int ArmRaiseWorkOrderId { get; set; }
        [DataMember]
        public int Position { get; set; }
        [DataMember]
        public int PositionWorkOrderId { get; set; }
        [DataMember]
        public long RaiseSpeed { get; set; }
    }
    #endregion GateArm

    #region TicketSpitter
    [DataContract, KnownType(typeof(ServiceQrcMovingWalkwayModel))]
    public class TicketSplitterCleaning
    {
        [DataMember]
        public bool IsClean { get; set; }

        [DataMember]
        public int CleanDarId { get; set; }
        [DataMember]
        public string CleanDescription { get; set; }
    }
    [DataContract, KnownType(typeof(ServiceQrcMovingWalkwayModel))]
    public class TicketSplitterPMCheck
    {
        [DataMember]
        public int Dispenser { get; set; }
        [DataMember]
        public int DispenserWorkOrderId { get; set; }
        [DataMember]
        public int Display { get; set; }
        [DataMember]
        public int DisplayWorkOrderId { get; set; }
    }
    #endregion TicketSpitter

    #region BusStation
    [DataContract, KnownType(typeof(ServiceQrcMovingWalkwayModel))]
    public class BusStationCleaning
    {
        [DataMember]
        public bool IsClean { get; set; }

        [DataMember]
        public int CleanDarId { get; set; }
        [DataMember]
        public string CleanDescription { get; set; }
    }
    [DataContract, KnownType(typeof(ServiceQrcEscalatorsModel))]
    public class BusStationPmCheck
    {
        [DataMember]
        public bool AreaClean { get; set; }

        [DataMember]
        public int AreaCleanWorkOrderId { get; set; }
    }
    #endregion BusStation

    #region Emergency Phone Systems
    [DataContract, KnownType(typeof(ServiceQrcMovingWalkwayModel))]
    public class PhoneSystemCleaning
    {
        [DataMember]
        public bool IsClean { get; set; }

        [DataMember]
        public int CleanDarId { get; set; }
        [DataMember]
        public string CleanDescription { get; set; }
    }
    [DataContract, KnownType(typeof(ServiceQrcMovingWalkwayModel))]
    public class PhoneSystemPMCheck
    {
        [DataMember]
        public int EContact { get; set; }
        [DataMember]
        public int EContactWorkOrderId { get; set; }
        [DataMember]
        public long RingLength { get; set; }
    }
    #endregion Emergency Phone Systems

    #region Vehicle

    [DataContract, KnownType(typeof(ServiceQrcVehicleModel))]
    public class VehicleCheckingOut
    {
        [DataMember]
        public bool IsCheckOut { get; set; }
        [DataMember]
        public bool VehicleMileage { get; set; }
        [DataMember]
        public string VmDescription { get; set; }
        [DataMember]
        public string OldVmDescription { get; set; }
        [DataMember]
        public bool IsItem { get; set; }
        [DataMember]
        public string ItemDescription { get; set; }
        [DataMember]
        public bool IsDamage { get; set; }
        [DataMember]
        public string CroppedPicture { get; set; }
        [DataMember]
        public string CapturedImage { get; set; }
        //[DataMember]
        //public int DamageWorkOrderId { get; set; }

    }
    [DataContract, KnownType(typeof(ServiceQrcVehicleModel))]
    public class VehicleCheckingIn
    {
        [DataMember]
        public bool IsCheckIn { get; set; }
        [DataMember]
        public bool ChVehicleMileage { get; set; }
        [DataMember]
        public string ChVmDescription { get; set; }
        [DataMember]
        public string OldChVmDescription { get; set; }
        [DataMember]
        public int FuelLevel { get; set; }
    }
    [DataContract, KnownType(typeof(ServiceQrcVehicleModel))]
    public class VehicleFuel
    {
        [DataMember]
        public bool VfVehicleMileage { get; set; }
        [DataMember]
        public string VfVmDescription { get; set; }
        [DataMember]
        public string OldVfVmDescription { get; set; }
    }
    [DataContract, KnownType(typeof(ServiceQrcVehicleModel))]
    public class VehicleCleaning
    {
        [DataMember]
        public bool IsClean { get; set; }
        [DataMember]
        public int CleanDarId { get; set; }
        [DataMember]
        public string CleanDescription { get; set; }
    }
    [DataContract, KnownType(typeof(ServiceQrcVehicleModel))]
    public class WeeklyVehicleCheck
    {
        [DataMember]
        public bool IsDamage { get; set; }
        [DataMember]
        public string CroppedPicture { get; set; }
        [DataMember]
        public string CapturedImage { get; set; }
        [DataMember]
        public bool WVehicleMileage { get; set; }
        [DataMember]
        public string WVmDescription { get; set; }
        [DataMember]
        public string OldWVmDescription { get; set; }
        [DataMember]
        public bool IsInteriorClean { get; set; }
        [DataMember]
        public int InteriorWorkOrderId { get; set; }
        [DataMember]
        public bool IsExteriorClean { get; set; }
        [DataMember]
        public int ExteriorWorkOrderId { get; set; }
        [DataMember]
        public int Transmission { get; set; }
        [DataMember]
        public int TransWorkOrderId { get; set; }
        [DataMember]
        public int FuelLevel { get; set; }
        [DataMember]
        public int TireCondition { get; set; }
        [DataMember]
        public int TireConditionId { get; set; }
        [DataMember]
        public int Windows { get; set; }
        [DataMember]
        public int WindowsWorkOrderId { get; set; }
        [DataMember]
        public int Brakes { get; set; }
        [DataMember]
        public int BrakesWorkOrderId { get; set; }
    }
    #endregion Vehicle

    #region Shuttle

    [DataContract, KnownType(typeof(ServiceQrcShuttleBusModel))]
    public class ShuttleDamage
    {
        [DataMember]
        public bool IsDamage { get; set; }
        [DataMember]
        public string CroppedPicture { get; set; }
        [DataMember]
        public string CapturedImage { get; set; }
        [DataMember]
        public long DamageDarId { get; set; }
        [DataMember]
        public string DamageDesc { get; set; }
    }
    [DataContract, KnownType(typeof(ServiceQrcShuttleBusModel))]
    public class ShuttleMileage
    {
        [DataMember]
        public bool ChShuttleMileage { get; set; }
        [DataMember]
        public string ChShDescription { get; set; }
        [DataMember]
        public string OldChShDescription { get; set; }
    }
    [DataContract, KnownType(typeof(ServiceQrcShuttleBusModel))]
    public class Engine
    {
        [DataMember]
        public bool IsFluidLeak { get; set; }
        [DataMember]
        public long FluidWorkOrderId { get; set; }
        [DataMember]
        public long FluidDarId { get; set; }

        [DataMember]
        public bool IsLooseWire { get; set; }
        [DataMember]
        public long LooseWorkOrderId { get; set; }

        [DataMember]
        public long BeltCondition { get; set; }
        [DataMember]
        public long BeltWorkOrderId { get; set; }

        [DataMember]
        public long OilLevel { get; set; }
        [DataMember]
        public long OilLevelWorkOrderId { get; set; }

        [DataMember]
        public long IsRadiator { get; set; }
        [DataMember]
        public long RadiatorWorkOrderId { get; set; }

        [DataMember]
        public long IsEngineNoise { get; set; }
        [DataMember]
        public long EngNoiseWorkOrderId { get; set; }
    }

    [DataContract, KnownType(typeof(ServiceQrcShuttleBusModel))]
    public class Interior
    {
        [DataMember]
        public long Switches { get; set; }
        [DataMember]
        public long SwitchesWorkOrderId { get; set; }
        [DataMember]
        public long Horn { get; set; }
        [DataMember]
        public long HornWorkOrderId { get; set; }
        [DataMember]
        public long FansDefrosters { get; set; }
        [DataMember]
        public long FansDefrostersWorkOrderId { get; set; }
        [DataMember]
        public long BreakWarningLight { get; set; }
        [DataMember]
        public long BreakWarningLightWorkOrderId { get; set; }
        [DataMember]
        public long BreakWarningLightDarId { get; set; }
        [DataMember]
        public long Entrance { get; set; }
        [DataMember]
        public long EntranceWorkOrderId { get; set; }
        [DataMember]
        public bool IsClean { get; set; }
        [DataMember]
        public int CleanDarId { get; set; }
        [DataMember]
        public string CleanWorkOrderId { get; set; }
        [DataMember]
        public long SeatCoushin { get; set; }
        [DataMember]
        public long SeatCoushinWorkOrderId { get; set; }
        [DataMember]
        public long SeatBelts { get; set; }
        [DataMember]
        public long SeatBeltsWorkOrderId { get; set; }
    }
    [DataContract, KnownType(typeof(ServiceQrcShuttleBusModel))]
    public class Exterior
    {
        [DataMember]
        public long Headlights { get; set; }
        [DataMember]
        public long HeadlightsWorkOrderId { get; set; }
        [DataMember]
        public bool IsFlashers { get; set; }
        [DataMember]
        public long FlashersWorkOrderId { get; set; }
        [DataMember]
        public bool IsReflectors { get; set; }
        [DataMember]
        public long ReflectorsWorkOrderId { get; set; }
        [DataMember]
        public long Window { get; set; }
        [DataMember]
        public long WindowWorkOrderId { get; set; }
        [DataMember]
        public bool IsExhaust { get; set; }
        [DataMember]
        public long ExhaustWorkOrderId { get; set; }
        [DataMember]
        public bool IsTailPipe { get; set; }
        [DataMember]
        public long TailPipeWorkOrderId { get; set; }
        [DataMember]
        public bool IsWindShields { get; set; }
        [DataMember]
        public long WindShieldsWorkOrderId { get; set; }

        [DataMember]
        public long FuelLevel { get; set; }
        [DataMember]
        public long FuelLevelWorkOrderId { get; set; }
    }

    [DataContract, KnownType(typeof(ServiceQrcShuttleBusModel))]
    public class Tires
    {
        [DataMember]
        public long FrontTireCondition { get; set; }
        [DataMember]
        public long FrontWorkOrderId { get; set; }
        [DataMember]
        public long FrontRim { get; set; }
        [DataMember]
        public long FrontRimWorkOrderId { get; set; }
        [DataMember]
        public bool IsFrontWheelAssembly { get; set; }
        [DataMember]
        public long FrontWheelAssemblyWorkOrderId { get; set; }

        [DataMember]
        public long RearTireCondition { get; set; }
        [DataMember]
        public long RearWorkOrderId { get; set; }
        [DataMember]
        public long RearRim { get; set; }
        [DataMember]
        public long RearRimWorkOrderId { get; set; }
        [DataMember]
        public bool IsRearWheelAssembly { get; set; }
        [DataMember]
        public long RearWheelAssemblyWorkOrderId { get; set; }

        [DataMember]
        public bool IsTireDamage { get; set; }
        [DataMember]
        public string Flat { get; set; }
        [DataMember]
        public string LowAirPressue { get; set; }
        [DataMember]
        public string MarginalTread { get; set; }
        [DataMember]
        public string LooseLugNuts { get; set; }
        [DataMember]
        public string CracksCuts { get; set; }
        [DataMember]
        public string GreaseLeaks { get; set; }
        [DataMember]
        public string OtherDamage { get; set; }
        [DataMember]
        public long TireDamageWorkOrderId { get; set; }

    }

    [DataContract, KnownType(typeof(ServiceQrcShuttleBusModel))]
    public class DriversCabin
    {
        [DataMember]
        public long SeatBelts { get; set; }
        [DataMember]
        public long SeatBeltsWorkOrderId { get; set; }
        [DataMember]
        public long DirectionalLights { get; set; }
        [DataMember]
        public long DirectionalLightsWorkOrderId { get; set; }
        [DataMember]
        public long ServiceBreak { get; set; }
        [DataMember]
        public long ServiceBreakWorkOrderId { get; set; }
        [DataMember]
        public long Clutch { get; set; }
        [DataMember]
        public long ClutchWorkOrderId { get; set; }
        [DataMember]
        public long ClutchDarId { get; set; }
        [DataMember]
        public long Steering { get; set; }
        [DataMember]
        public long SteeringWorkOrderId { get; set; }
        [DataMember]
        public long SteeringDarId { get; set; }
    }

    [DataContract, KnownType(typeof(ServiceQrcShuttleBusModel))]
    public class Accessories
    {
        [DataMember]
        public long Wheelchair { get; set; }
        [DataMember]
        public long WheelchairWorkOrderId { get; set; }
        [DataMember]
        public long SpecialServiceDoor { get; set; }
        [DataMember]
        public long SpecialServiceDoorWorkOrderId { get; set; }
        [DataMember]
        public long PumpHandle { get; set; }
        [DataMember]
        public long PumpHandleWorkOrderId { get; set; }
        [DataMember]
        public long RadioCheck { get; set; }
        [DataMember]
        public long RadioCheckWorkOrderId { get; set; }
    }
    [DataContract, KnownType(typeof(ServiceQrcShuttleBusModel))]
    public class Emergency
    {
        [DataMember]
        public bool IsLiftOperation { get; set; }
        [DataMember]
        public long LiftOperationWorkOrderId { get; set; }
        [DataMember]
        public long Equipment { get; set; }
        [DataMember]
        public long EquipmentWorkOrderId { get; set; }
        [DataMember]
        public long Buzzers { get; set; }
        [DataMember]
        public long BuzzersWorkOrderId { get; set; }
        [DataMember]
        public long FirstAidRequired { get; set; }
        [DataMember]
        public long FirstAidWorkOrderId { get; set; }
        [DataMember]
        public bool IsDoorWarning { get; set; }
        [DataMember]
        public long DoorWarningWorkOrderId { get; set; }
        [DataMember]
        public bool IsPostedDecals { get; set; }
        [DataMember]
        public long PostedDecalsWorkOrderId { get; set; }
        [DataMember]
        public long ControlMechanism { get; set; }
        [DataMember]
        public long ControlMechanismWorkOrderId { get; set; }
        [DataMember]
        public bool IsFlares { get; set; }
        [DataMember]
        public long FlaresWorkOrderId { get; set; }
        [DataMember]
        public bool IsFireExtinguisher { get; set; }
        [DataMember]
        public long FireExtinguisherWorkOrderId { get; set; }
        [DataMember]
        public bool IsProtectivePadding { get; set; }
        [DataMember]
        public long ProtectivePaddingWorkOrderId { get; set; }
    }

    #endregion Shuttle
}
