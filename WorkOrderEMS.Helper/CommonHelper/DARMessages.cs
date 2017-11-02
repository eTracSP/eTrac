using System.Globalization;

namespace WorkOrderEMS.Helper
{
    /// <summary>
    /// Created by: Roshan Rahood
    /// Created on: 09-Jan-2015
    /// Purpose: This class will be used to create the common message for the DAR.
    /// </summary>
    public static class DarMessage
    {
        public static string SaveSuccessLocationDar(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "New Location : {0} has been created sucessfully.", locationName);
        }

        public static string UpdateSuccessLocationDar(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Location : {0} has been updated.", locationName);
        }

        public static string DeleteSuccessLocationDar(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Location : {0} has been deleted.", locationName);
        }

        public static string AdministratorDeleteDar(string locationName)
        {
            return "Administrator for the location :" + locationName + " has been deleted.";
        }

        public static string NewManagerCreatedDar(string locationName)
        {
            return "New manager for the location :" + locationName + " has been created.";
        }

        public static string ClientUpdatedDar(string locationName)
        {
            return "Client for the location :" + locationName + " has been updated.";
        }

        public static string NewClientCreatedDar(string locationName)
        {
            return "New Client for the location :" + locationName + " has been created.";
        }

        public static string ManagerUpdatedDar(string locationName)
        {
            return "Manager for the location :" + locationName + " has been updated.";
        }

        public static string NewEmployeeCreatedDar(string locationName)
        {
            return "New employee for the location :" + locationName + " has been Created.";
        }

        public static string EmployeeUpdatedDar(string locationName)
        {
            if (string.IsNullOrEmpty(locationName))
            { return "Employee without location has been updated."; }
            else
            {
                return "Employee for the location :" + locationName + " has been updated.";
            }
        }

        public static string NewAdministratorCreatedDar(string locationName)
        {
            return "Administrator for the location :" + locationName + " has been Created.";
        }

        public static string AdministratorUpdatedDar(string locationName)
        {
            return "Administrator for the location :" + locationName + " has been updated.";
        }

        public static string NewITAdministratorCreatedDar(string locationName)
        {
            return "IT Administrator for the location :" + locationName + " has been Created.";
        }

        public static string ITAdministratorUpdatedDar(string locationName)
        {
            return "IT Administrator for the location :" + locationName + " has been updated.";
        }

        public static string ITAdministratorDeletedDar(string locationName)
        {
            return "IT Administrator for the location :" + locationName + " has been deleted.";
        }

        public static string UserDeleteDar(string userName)
        {
            return string.Format(CultureInfo.InvariantCulture, "User : {0}  has been deleted.", userName);
        }

        public static string UserVerifiedDar(string userName)
        {
            return string.Format(CultureInfo.InvariantCulture, "User : {0} has been manually verified.", userName);
        }

        public static string SaveNewInventoryDar(string locationName)
        {
            return "New inventory for the location :" + locationName + " has been added.";
        }

        public static string UpdateInventoryDar(string locationName)
        {
            return "Inventory for the location :" + locationName + " has been updated.";
        }
        public static string SaveNewRuleDar(string ruleName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "New rule : {0} for the location : {1} has been created.", ruleName, locationName);
        }
        public static string UpdateRuleDar(string ruleName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Rule : {0} for the location : {1} has been updated.", ruleName, locationName);
        }
        public static string DeleteRuleDar(string ruleName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Rule : {0} for the location : {1} has been deleted.", ruleName, locationName);
        }
        public static string UpdateVehicle(string QRCCodeID,string locationName)
        {          
            return "Vehicle: " + QRCCodeID + " for the location :" + locationName + " has been updated.";
        }       
        public static string DarUpdateTaskStatus(string userName, string clientUserName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Employee :{0} has accepted the work order requested by client:{1}.", userName, clientUserName);
        }
        public static string DarUpdateStartTimeTaskStatus(string userName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "DAR has been updated for Work Order Assignment - Start  at location {1}, for {0}.", userName, locationName);
        }
        public static string DarUpdateEndTimeTaskStatus(string userName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "DAR has been updated for Work Order Assignment - End  at location {1}, for {0}.", userName, locationName);
        }
        public static string QrcVehicleCleaning(string userName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Employee :{0} has clean the vehicle", userName);
        }
        public static string UserSuccessfullyCreated(string UserName, string CreatedBy, string UserType)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1} is created by {2}.", UserType, UserName, CreatedBy);
        }
        public static string DeleteLocationMapping(string username, string mappingWith, string userType, string operationBy)
        {
            return string.Format(CultureInfo.InvariantCulture, "{1} {0} mapping with {2} location has been deleted by {3} Admin User.", userType, username, mappingWith, operationBy);
        }
        public static string DarJumpStartEndTimeStatus(string userName, string locationName, string desc)
        {
            return string.Format(CultureInfo.InvariantCulture, "DAR has been updated at location {1} for {2}.", userName, locationName, desc);
        }
        public static string FacilityRequestAccept(string userName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "DAR has been updated for Facility Request accept at location {1}, for {0}", userName, locationName);
        }
        public static string UrgentWOAcceptbyEmp(string userName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "DAR has been updated for Urgent Work Request accept at location {1}, for {0}", userName, locationName);
        }
        public static string DarCustomerCall(string userName, string locationName, string desc)
        {
            return string.Format(CultureInfo.InvariantCulture, "Employee :{0} has called to {1} at location :{2}", userName, desc, locationName);
        }
        public static string SaveQRC(string qrcName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "New QRC : {0} for the location :{1} has been created.", qrcName, locationName);
        }
        public static string UpdateQRC(string qrcName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "QRC : {0} for the location :{1} has been updated.", qrcName, locationName);
        }
        public static string DeleteQRC(string qrcName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "QRC : {0} for the location :{1} has been deleted.", qrcName, locationName);
        }
        public static string CreateWorkRequest(string workOrderCode, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Work Order : {0} for the location :{1} has been created.", workOrderCode, locationName);
        }
        public static string UpdateWorkRequest(string workOrderCode, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Work Order : {0} for the location :{1} has been updated.", workOrderCode, locationName);
        }
        public static string DeleteWorkRequest(string workOrderCode, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Work Order : {0} for the location :{1} has been deleted.", workOrderCode, locationName);
        }
        public static string QRCScanMessage(string userName, string qrcName, string qrcCode)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} has scanned the QR code {1}({2}) ", userName, qrcName, qrcCode);
        }
        public static string VehicleScanMessage(string userName, string vehicleName, string vehicleCode)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} has scanned the Vehicle QR code {1}({2}) ", userName, vehicleName, vehicleCode);
        }
        public static string ShiftEnd(string userName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Employee :{0} has end the shift for the day", userName);
        }
        public static string CRTaskStart(string userName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "DAR has been updated for Continuous Request - Start  at location {1}, for {0}.", userName, locationName);
        }
        public static string CRTaskEnd(string userName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "DAR has been updated for Continuous Request - End  at location {1}, for {0}.", userName, locationName);
        }
        public static string CheckIn(string qrcName, string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "QRC : {0} for the location :{1} has been check in through web.", qrcName, locationName);
        }
        public static string LocationAssignedAdmin(string userName, string location)
        {
            return string.Format(CultureInfo.InvariantCulture, "Administrator: {0} for the location:{1} has been assigned.", userName, location);
        }
        public static string LocationAssignedEmployee(string userName, string location)
        {
            return string.Format(CultureInfo.InvariantCulture, "Employee: {0} for the location:{1} has been assigned.", userName, location);
        }
        public static string LocationAssignedManager(string userName, string location)
        {
            return string.Format(CultureInfo.InvariantCulture, "Manager: {0} for the location:{1} has been assigned.", userName, location);

        }
        public static string LocationAssigned(string userName, string location)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} for the location:{1} has been assigned.", userName, location);

        }
        public static string NeweFleetPMCreated(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "New eFleet Preventative Maintenance for the location :{0} has been created.", locationName);
        }
        public static string NeweFleetPMUpdated(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Preventative Maintenance for the location :{0} has been Updated.", locationName);
        }
        public static string DeleteFleetPM(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Prevantative Maintenance for the location :{0} has been deleted.", locationName);
        }
        public static string RegisterNeweFleetMaintenance(string locationName)
        {
            return "New Vehicle Maintenance for the location :" + locationName + " has been registered.";
        }
        public static string UpdateeFleetMaintenance(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Maintenance for the location :{0} has been Updated.", locationName);
        }
        public static string DeleteFleetMaintenance(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Maintenance for the location :{0} has been deleted.", locationName);
        }
        public static string RegisterNeweFleetDriver(string locationName)
        {
            return "New eFleet driver for the location :" + locationName + " has been registered.";
        }
        public static string UpdateeFleetDriver(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Driver for the location :{0} has been Updated.", locationName);
        }
        public static string DeleteFleetDriver(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Driver for the location :{0} has been deleted.", locationName);
        }
        public static string RegisterNeweFleetIncidentVehicle(string locationName)
        {
            return "New eFleet Vehicle Incident for the location :" + locationName + " has been registered.";
        }
        public static string UpdateeFleetVehicleIncident(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Vehicle Incident for the location :{0} has been Updated.", locationName);
        }
        public static string DeleteFleetVehicleIncident(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Vehicle Incident for the location :{0} has been deleted.", locationName);
        }
        public static string RegisterNeweFleetVehicle(string locationName)
        {
            return "New eFleet Vehicle for the location :" + locationName + " has been registered.";
        }
        public static string UpdateeFleetVehicle(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Vehicle for the location :{0} has been Updated.", locationName);
        }
        public static string DeleteFleetVehicle(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Vehicle for the location :{0} has been deleted.", locationName);
        }
        public static string RegisterNeweFleetFueling(string locationName)
        {
            return "New eFleet Fueling for the location :" + locationName + " has been registered.";
        }
    }
}
