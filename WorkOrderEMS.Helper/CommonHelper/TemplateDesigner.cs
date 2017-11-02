using System;
using System.Web;

namespace WorkOrderEMS.Helper
{
    public class TemplateDesigner
    {
        //public static string eMaintenanceTemplate(string licenseplate, string customername, string customeraddress, string customertelephone, string signaturecustomer, string empsignature, string locname, string vmake, int? vyear, string driverlicenseno, string vmodel, string frname, DateTime? StartTime, DateTime? EndTime, string AssignedToName)
        public static string eMaintenanceTemplate(string licenseplate, string customername, string customeraddress, string customertelephone, string signaturecustomer, string empsignature, string locname, string vmake, int? vyear, string driverlicenseno, string vmodel, string frname, string TimeZoneName, long TimeZoneOffset, bool IsTimeZoneinDaylight)
        {
            try
            {
                DateTime DateTimeNow = Convert.ToDateTime(DateTime.UtcNow.ToMobileClientTimeZone(TimeZoneName, TimeZoneOffset, IsTimeZoneinDaylight, false));
                string HTMLTemplatePath = HttpContext.Current.Server.MapPath("~") + "/Content/eMaintenanceForm/WorkRequestHTMLForm.html";
                string HTMLBody = "";
                HTMLBody = System.IO.File.ReadAllText(HTMLTemplatePath);
                HTMLBody = HTMLBody.Replace("[LOCNAME]", locname);
                HTMLBody = HTMLBody.Replace("[DATETIME]", DateTimeNow.ToString("MM'/'dd'/'yyyy hh:mm tt"));
                HTMLBody = HTMLBody.Replace("[LicensePlateNumber]", licenseplate);
                HTMLBody = HTMLBody.Replace("[VEHICLEYEAR]", vyear.ToString());
                HTMLBody = HTMLBody.Replace("[VEHICLEMAKE]", vmake);
                HTMLBody = HTMLBody.Replace("[VEHICLEMODEL]", vmodel);
                HTMLBody = HTMLBody.Replace("[FACILITYREQUESTNAME]", frname);
                HTMLBody = HTMLBody.Replace("[WORKDATE]", DateTimeNow.ToString("MM'/'dd'/'yyyy hh:mm tt"));
                HTMLBody = HTMLBody.Replace("[WORKYEAR]", DateTimeNow.Year.ToString());
                string body = System.Web.HttpUtility.HtmlDecode(HTMLBody);
                return body;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string eFleetTemplate(dynamic obj)
        {
            try
            {
                string TimeZoneName = obj.TimeZoneName;
                long TimeZoneOffset = obj.TimeZoneOffset;
                bool IsTimeZoneinDaylight = obj.IsTimeZoneinDaylight;
                DateTime DateTimeNow = Convert.ToDateTime(DateTime.UtcNow.ToMobileClientTimeZone(TimeZoneName, TimeZoneOffset, IsTimeZoneinDaylight, false));
                string HTMLTemplatePath = HttpContext.Current.Server.MapPath("~") + "/Content/eFleetDocs/Inspection/Form/EfleetInspection.html";
                string HTMLBody = "";
                HTMLBody = System.IO.File.ReadAllText(HTMLTemplatePath);
                HTMLBody = HTMLBody.Replace("[UserName]", obj.UserName); 
                HTMLBody = HTMLBody.Replace("[Status]", obj.Status);
                HTMLBody = HTMLBody.Replace("[QRCodeID]", obj.QRCodeID);
                HTMLBody = HTMLBody.Replace("[VehicleNumber]", obj.VehicleNumber);
                HTMLBody = HTMLBody.Replace("[DATETIME]", DateTimeNow.ToString("MM'/'dd'/'yyyy hh:mm tt"));
                HTMLBody = HTMLBody.Replace("[IsDamage]", (obj.DamageTire.Damage.IsDamage == true) ? "Yes" : "No");
                HTMLBody = HTMLBody.Replace("[ChShuttleMileage]", (obj.InteriorMileageDriver.Mileage.ChShuttleMileage == true) ? "Yes" : "No");
                HTMLBody = HTMLBody.Replace("[OldChShDescription]", Convert.ToString(obj.InteriorMileageDriver.Mileage.OldChShDescription));
                HTMLBody = HTMLBody.Replace("[ChShDescription]", Convert.ToString(obj.InteriorMileageDriver.Mileage.ChShDescription));
                if (obj.DamageTire.Tires.FrontTireCondition != 0)
                    HTMLBody = HTMLBody.Replace("[FrontTireCondition]", (obj.DamageTire.Tires.FrontTireCondition == (long)eFleetEnum.Good) ? "Good" : (obj.DamageTire.Tires.FrontTireCondition == (long)eFleetEnum.Fair) ? "Fair" : "Poor");
                else
                    HTMLBody = HTMLBody.Replace("[FrontTireCondition]", "Not Checked");

                if (obj.DamageTire.Tires.FrontRim != 0)
                    HTMLBody = HTMLBody.Replace("[FrontRim]", (obj.DamageTire.Tires.FrontRim == (long)eFleetEnum.Good) ? "Good" : (obj.DamageTire.Tires.FrontRim == (long)eFleetEnum.Fair) ? "Fair" : "Poor");
                else
                    HTMLBody = HTMLBody.Replace("[FrontRim]", "Not Checked");

                HTMLBody = HTMLBody.Replace("[IsFrontWheelAssembly]", (obj.DamageTire.Tires.IsFrontWheelAssembly == true) ? "Yes" : "No");
                
                if (obj.DamageTire.Tires.RearTireCondition != 0)
                    HTMLBody = HTMLBody.Replace("[RearTireCondition]", (obj.DamageTire.Tires.RearTireCondition == (long)eFleetEnum.Good) ? "Good" : (obj.DamageTire.Tires.RearTireCondition == (long)eFleetEnum.Fair) ? "Fair" : "Poor");
                else
                    HTMLBody = HTMLBody.Replace("[RearTireCondition]", "Not Checked");

                if (obj.DamageTire.Tires.RearRim != 0)
                    HTMLBody = HTMLBody.Replace("[RearRim]", (obj.DamageTire.Tires.RearRim == (long)eFleetEnum.Good) ? "Good" : (obj.DamageTire.Tires.RearRim == (long)eFleetEnum.Fair) ? "Fair" : "Poor");
                else
                    HTMLBody = HTMLBody.Replace("[RearRim]", "Not Checked");

                HTMLBody = HTMLBody.Replace("[IsRearWheelAssembly]", (obj.DamageTire.Tires.IsRearWheelAssembly == true) ? "Yes" : "No");
                HTMLBody = HTMLBody.Replace("[Flat]", Convert.ToString(obj.DamageTire.Tires.Flat));
                HTMLBody = HTMLBody.Replace("[LowAirPressue]", Convert.ToString(obj.DamageTire.Tires.LowAirPressue));
                HTMLBody = HTMLBody.Replace("[MarginalTread]", Convert.ToString(obj.DamageTire.Tires.MarginalTread));
                HTMLBody = HTMLBody.Replace("[LooseLugNuts]", Convert.ToString(obj.DamageTire.Tires.LooseLugNuts));
                HTMLBody = HTMLBody.Replace("[CracksCuts]", Convert.ToString(obj.DamageTire.Tires.CracksCuts));
                HTMLBody = HTMLBody.Replace("[GreaseLeaks]", Convert.ToString(obj.DamageTire.Tires.GreaseLeaks));
                HTMLBody = HTMLBody.Replace("[OtherDamage]", Convert.ToString(obj.DamageTire.Tires.OtherDamage));
                HTMLBody = HTMLBody.Replace("[Switches]", (obj.InteriorMileageDriver.Interior.Switches != 0 && obj.InteriorMileageDriver.Interior.Switches == (long)eFleetEnum.Functional) ? "Functional" : "Not Functional");
                HTMLBody = HTMLBody.Replace("[Horn]", (obj.InteriorMileageDriver.Interior.Horn != 0 && obj.InteriorMileageDriver.Interior.Horn == (long)eFleetEnum.Functional) ? "Functional" : "Not Functional");
                HTMLBody = HTMLBody.Replace("[FansDefrosters]", (obj.InteriorMileageDriver.Interior.FansDefrosters != 0 && obj.InteriorMileageDriver.Interior.FansDefrosters == (long)eFleetEnum.Functional) ? "Functional" : "Not Functional");
                HTMLBody = HTMLBody.Replace("[BreakWarningLight]", (obj.InteriorMileageDriver.Interior.BreakWarningLight != 0 && obj.InteriorMileageDriver.Interior.BreakWarningLight == (long)eFleetEnum.Functional) ? "Functional" : "Not Functional");
                
                if (obj.InteriorMileageDriver.Interior.Entrance != 0)
                    HTMLBody = HTMLBody.Replace("[Entrance]", (obj.InteriorMileageDriver.Interior.Entrance == (long)eFleetEnum.Good) ? "Good" : (obj.InteriorMileageDriver.Interior.Entrance == (long)eFleetEnum.Fair) ? "Fair" : "Poor");
                else
                    HTMLBody = HTMLBody.Replace("[Entrance]", "Not Checked");

                HTMLBody = HTMLBody.Replace("[IsClean]", (obj.InteriorMileageDriver.Interior.IsClean == true) ? "Yes" : "No");
                
                if (obj.InteriorMileageDriver.Interior.SeatCoushin != 0)
                    HTMLBody = HTMLBody.Replace("[SeatCoushin]", (obj.InteriorMileageDriver.Interior.SeatCoushin == (long)eFleetEnum.Good) ? "Good" : (obj.InteriorMileageDriver.Interior.SeatCoushin == (long)eFleetEnum.Fair) ? "Fair" : "Poor");
                else
                    HTMLBody = HTMLBody.Replace("[SeatCoushin]", "Not Checked");
                
                if (obj.InteriorMileageDriver.Interior.SeatBelts != 0)
                    HTMLBody = HTMLBody.Replace("[SeatBelts]", (obj.InteriorMileageDriver.Interior.SeatBelts == (long)eFleetEnum.Good) ? "Good" : (obj.InteriorMileageDriver.Interior.SeatBelts == (long)eFleetEnum.Fair) ? "Fair" : "Poor");
                else
                    HTMLBody = HTMLBody.Replace("[SeatBelts]", "Not Checked");

                HTMLBody = HTMLBody.Replace("[IsFluidLeak]", (obj.EngineExterior.Engine.IsFluidLeak == true) ? "Yes" : "No");
                HTMLBody = HTMLBody.Replace("[IsLooseWire]", (obj.EngineExterior.Engine.IsLooseWire == true) ? "Yes" : "No");

                if (obj.EngineExterior.Engine.BeltCondition != 0)
                    HTMLBody = HTMLBody.Replace("[BeltCondition]", (obj.EngineExterior.Engine.BeltCondition == (long)eFleetEnum.Good) ? "Good" : (obj.EngineExterior.Engine.BeltCondition == (long)eFleetEnum.Fair) ? "Fair" : "Poor");
                else
                    HTMLBody = HTMLBody.Replace("[BeltCondition]", "Not Checked");

                if (obj.EngineExterior.Engine.OilLevel != 0)
                    HTMLBody = HTMLBody.Replace("[OilLevel]", (obj.EngineExterior.Engine.OilLevel == (long)eFleetEnum.Full) ? "Full" : (obj.EngineExterior.Engine.OilLevel == (long)eFleetEnum.ThreeByFour) ? "3/4" : (obj.EngineExterior.Engine.OilLevel == (long)eFleetEnum.OneByTwo) ? "1/2" : "1/4");
                else
                    HTMLBody = HTMLBody.Replace("[OilLevel]", "Not Checked");
                HTMLBody = HTMLBody.Replace("[IsRadiator]", (obj.EngineExterior.Engine.IsRadiator == 1) ? "Yes" : "No");
                HTMLBody = HTMLBody.Replace("[IsEngineNoise]", (obj.EngineExterior.Engine.IsEngineNoise == 1) ? "Yes" : "No");
                HTMLBody = HTMLBody.Replace("[Headlights]", (obj.EngineExterior.Exterior.Headlights != 0 && obj.EngineExterior.Exterior.Headlights == (long)eFleetEnum.Good) ? "Good" : "Poor");
                HTMLBody = HTMLBody.Replace("[IsFlashers]", (obj.EngineExterior.Exterior.IsFlashers == true) ? "Yes" : "No");
                HTMLBody = HTMLBody.Replace("[IsReflectors]", (obj.EngineExterior.Exterior.IsReflectors == true) ? "Yes" : "No");
                HTMLBody = HTMLBody.Replace("[Window]", (obj.EngineExterior.Exterior.Window == (long)eFleetEnum.Functional) ? "Functional" : "Not Functional");
                HTMLBody = HTMLBody.Replace("[IsExhaust]", (obj.EngineExterior.Exterior.IsExhaust == true) ? "Yes" : "No");
                HTMLBody = HTMLBody.Replace("[IsTailPipe]", (obj.EngineExterior.Exterior.IsTailPipe == true) ? "Yes" : "No");
                HTMLBody = HTMLBody.Replace("[IsWindShields]", (obj.EngineExterior.Exterior.IsWindShields == true) ? "Yes" : "No");
                HTMLBody = HTMLBody.Replace("[IsLiftOperation]", (obj.EmergencyAccessories.Emergency.IsLiftOperation == true) ? "Yes" : "No");
                HTMLBody = HTMLBody.Replace("[Equipment]", (obj.EmergencyAccessories.Emergency.Equipment == (long)eFleetEnum.Functional) ? "Functional" : "Not Functional");
                HTMLBody = HTMLBody.Replace("[Buzzers]", (obj.EmergencyAccessories.Emergency.Buzzers == (long)eFleetEnum.Functional) ? "Functional" : "Not Functional");
                HTMLBody = HTMLBody.Replace("[FirstAidRequired]", (obj.EmergencyAccessories.Emergency.FirstAidRequired == (long)eFleetEnum.Required) ? "Required" : "Not Required");
                HTMLBody = HTMLBody.Replace("[IsDoorWarning]", (obj.EmergencyAccessories.Emergency.IsDoorWarning == true) ? "Yes" : "No");
                HTMLBody = HTMLBody.Replace("[IsPostedDecals]", (obj.EmergencyAccessories.Emergency.IsPostedDecals == true) ? "Yes" : "No");
                HTMLBody = HTMLBody.Replace("[ControlMechanism]", (obj.EmergencyAccessories.Emergency.ControlMechanism == (long)eFleetEnum.Functional) ? "Functional" : "Not Functional");
                HTMLBody = HTMLBody.Replace("[IsFlares]", (obj.EmergencyAccessories.Emergency.IsFlares == true) ? "Yes" : "No");
                HTMLBody = HTMLBody.Replace("[IsFireExtinguisher]", (obj.EmergencyAccessories.Emergency.IsFireExtinguisher == true) ? "Yes" : "No");
                HTMLBody = HTMLBody.Replace("[IsProtectivePadding]", (obj.EmergencyAccessories.Emergency.IsProtectivePadding == true) ? "Yes" : "No");
                if (obj.InteriorMileageDriver.DriversCabin.SeatBelts != 0)
                    HTMLBody = HTMLBody.Replace("[SeatBeltsDC]", (obj.InteriorMileageDriver.DriversCabin.SeatBelts == (long)eFleetEnum.Good) ? "Good" : (obj.InteriorMileageDriver.DriversCabin.SeatBelts == (long)eFleetEnum.Fair) ? "Fair" : "Poor");
                else
                    HTMLBody = HTMLBody.Replace("[SeatBeltsDC]", "Not Checked");
              
                HTMLBody = HTMLBody.Replace("[DirectionalLights]", (obj.InteriorMileageDriver.DriversCabin.DirectionalLights == (long)eFleetEnum.Functional) ? "Functional" : "Not Functional");
                HTMLBody = HTMLBody.Replace("[ServiceBreak]", (obj.InteriorMileageDriver.DriversCabin.ServiceBreak == (long)eFleetEnum.Functional) ? "Functional" : "Not Functional");
                HTMLBody = HTMLBody.Replace("[Clutch]", (obj.InteriorMileageDriver.DriversCabin.Clutch == (long)eFleetEnum.Functional) ? "Functional" : "Not Functional");
                HTMLBody = HTMLBody.Replace("[Steering]", (obj.InteriorMileageDriver.DriversCabin.Steering == (long)eFleetEnum.Functional) ? "Functional" : "Not Functional");
                HTMLBody = HTMLBody.Replace("[Wheelchair]", (obj.EmergencyAccessories.Accessories.Wheelchair == (long)eFleetEnum.Functional) ? "Functional" : "Not Functional");
                HTMLBody = HTMLBody.Replace("[SpecialServiceDoor]", (obj.EmergencyAccessories.Accessories.SpecialServiceDoor == (long)eFleetEnum.Functional) ? "Functional" : "Not Functional");
                HTMLBody = HTMLBody.Replace("[PumpHandle]", (obj.EmergencyAccessories.Accessories.PumpHandle == (long)eFleetEnum.Functional) ? "Functional" : "Not Functional");
                HTMLBody = HTMLBody.Replace("[RadioCheck]", (obj.EmergencyAccessories.Accessories.RadioCheck == (long)eFleetEnum.Functional) ? "Functional" : "Not Functional");

                //HTMLBody = HTMLBody.Replace("[]", Convert.ToString(obj.));
                string body = System.Web.HttpUtility.HtmlDecode(HTMLBody);
                return body;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }
}
