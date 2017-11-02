using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.ServiceModel.ApiModel
{
    #region Inspection

    
    public class ShuttleDamage
    {        
        public bool IsDamage { get; set; }
        
        public string CroppedPicture { get; set; }
        
        public string CapturedImage { get; set; }
        
        public long DamageDarId { get; set; }
        
        public string DamageDesc { get; set; }
    }
    
    public class ShuttleMileage
    {        
        public bool ChShuttleMileage { get; set; }
        
        public string ChShDescription { get; set; }
        
        public string OldChShDescription { get; set; }
    }
    
    public class Engine
    {        
        public bool IsFluidLeak { get; set; }
        
        public long FluidWorkOrderId { get; set; }
        
        public long FluidDarId { get; set; }
        
        public bool IsLooseWire { get; set; }
        
        public long LooseWorkOrderId { get; set; }
        
        public long BeltCondition { get; set; }
        
        public long BeltWorkOrderId { get; set; }
        
        public long OilLevel { get; set; }
        
        public long OilLevelWorkOrderId { get; set; }
        
        public long IsRadiator { get; set; }
        
        public long RadiatorWorkOrderId { get; set; }
        
        public long IsEngineNoise { get; set; }
        
        public long EngNoiseWorkOrderId { get; set; }
    }

    
    public class Interior
    {        
        public long Switches { get; set; }
        
        public long SwitchesWorkOrderId { get; set; }
        
        public long Horn { get; set; }
        
        public long HornWorkOrderId { get; set; }
        
        public long FansDefrosters { get; set; }
        
        public long FansDefrostersWorkOrderId { get; set; }
        
        public long BreakWarningLight { get; set; }
        
        public long BreakWarningLightWorkOrderId { get; set; }
        
        public long BreakWarningLightDarId { get; set; }
        
        public long Entrance { get; set; }
        
        public long EntranceWorkOrderId { get; set; }
        
        public bool IsClean { get; set; }
        
        public int CleanDarId { get; set; }
        
        public string CleanWorkOrderId { get; set; }
        
        public long SeatCoushin { get; set; }
        
        public long SeatCoushinWorkOrderId { get; set; }
        
        public long SeatBelts { get; set; }
        
        public long SeatBeltsWorkOrderId { get; set; }
    }
    
    public class Exterior
    {        
        public long Headlights { get; set; }
        
        public long HeadlightsWorkOrderId { get; set; }
        
        public bool IsFlashers { get; set; }
        
        public long FlashersWorkOrderId { get; set; }
        
        public bool IsReflectors { get; set; }
        
        public long ReflectorsWorkOrderId { get; set; }
        
        public long Window { get; set; }
        
        public long WindowWorkOrderId { get; set; }
        
        public bool IsExhaust { get; set; }
        
        public long ExhaustWorkOrderId { get; set; }
        
        public bool IsTailPipe { get; set; }
        
        public long TailPipeWorkOrderId { get; set; }
        
        public bool IsWindShields { get; set; }
        
        public long WindShieldsWorkOrderId { get; set; }
        
        public long FuelLevel { get; set; }
        
        public long FuelLevelWorkOrderId { get; set; }
    }

    
    public class Tires
    {        
        public long FrontTireCondition { get; set; }
        
        public long FrontWorkOrderId { get; set; }
        
        public long FrontRim { get; set; }
        
        public long FrontRimWorkOrderId { get; set; }
        
        public bool IsFrontWheelAssembly { get; set; }
        
        public long FrontWheelAssemblyWorkOrderId { get; set; }
        
        public long RearTireCondition { get; set; }
        
        public long RearWorkOrderId { get; set; }
        
        public long RearRim { get; set; }
        
        public long RearRimWorkOrderId { get; set; }
        
        public bool IsRearWheelAssembly { get; set; }
        
        public long RearWheelAssemblyWorkOrderId { get; set; }
        
        public bool IsTireDamage { get; set; }
        
        public string Flat { get; set; }
        
        public string LowAirPressue { get; set; }
        
        public string MarginalTread { get; set; }
        
        public string LooseLugNuts { get; set; }
        
        public string CracksCuts { get; set; }
        
        public string GreaseLeaks { get; set; }
        
        public string OtherDamage { get; set; }
        
        public long TireDamageWorkOrderId { get; set; }
    }

    
    public class DriversCabin
    {        
        public long SeatBelts { get; set; }
        
        public long SeatBeltsWorkOrderId { get; set; }
        
        public long DirectionalLights { get; set; }
        
        public long DirectionalLightsWorkOrderId { get; set; }
        
        public long ServiceBreak { get; set; }
        
        public long ServiceBreakWorkOrderId { get; set; }
        
        public long Clutch { get; set; }
        
        public long ClutchWorkOrderId { get; set; }
        
        public long ClutchDarId { get; set; }
        
        public long Steering { get; set; }
        
        public long SteeringWorkOrderId { get; set; }
        
        public long SteeringDarId { get; set; }
    }

    
    public class Accessories
    {        
        public long Wheelchair { get; set; }
        
        public long WheelchairWorkOrderId { get; set; }
        
        public long SpecialServiceDoor { get; set; }
        
        public long SpecialServiceDoorWorkOrderId { get; set; }
        
        public long PumpHandle { get; set; }
        
        public long PumpHandleWorkOrderId { get; set; }
        
        public long RadioCheck { get; set; }
        
        public long RadioCheckWorkOrderId { get; set; }
    }
    
    public class Emergency
    {        
        public bool IsLiftOperation { get; set; }
        
        public long LiftOperationWorkOrderId { get; set; }
        
        public long Equipment { get; set; }
        
        public long EquipmentWorkOrderId { get; set; }
        
        public long Buzzers { get; set; }
        
        public long BuzzersWorkOrderId { get; set; }
        
        public long FirstAidRequired { get; set; }
        
        public long FirstAidWorkOrderId { get; set; }
        
        public bool IsDoorWarning { get; set; }
        
        public long DoorWarningWorkOrderId { get; set; }
        
        public bool IsPostedDecals { get; set; }
        
        public long PostedDecalsWorkOrderId { get; set; }
        
        public long ControlMechanism { get; set; }
        
        public long ControlMechanismWorkOrderId { get; set; }
        
        public bool IsFlares { get; set; }
        
        public long FlaresWorkOrderId { get; set; }
        
        public bool IsFireExtinguisher { get; set; }
        
        public long FireExtinguisherWorkOrderId { get; set; }
        
        public bool IsProtectivePadding { get; set; }
        
        public long ProtectivePaddingWorkOrderId { get; set; }
    }

    #endregion Inspection
}
