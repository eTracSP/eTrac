using System.Globalization;

namespace WorkOrderEMS.Helper
{
    /// <summary>
    /// Created by: Gayarti
    /// Created on: 25-Aug-2014
    /// Purpose: This class will be used to create the common message.
    /// </summary>
    public static class CommonMessage
    {
        public const string UserSession = "UserSession";

        public static string SaveSuccessMessage()
        {
            return "Record has been saved successfully.";
        }
        public static string UserSaveSuccessMessage()
        {
            return "User has been saved successfully.";
        }
        public static string WorkOrderSaveSuccessMessage()
        {
            return "Work Order has been saved successfully.";
        }
        public static string NoEmployeeAsscoiated()
        {
            return "No employee associated with location. Please add employee to proceed";
        }
        public static string ShareSuccessMessage()
        {
            //return "User Roles updated successfully.";
            return "Record has been shared successfully";
        }
        public static string UpdateSuccessMessage()
        {
            //return "Record has been Updated successfully.";
            return "Updated successfully.";

        }
        public static string AssignedSuccessMessage()
        {
            //return "Record has been Updated successfully.";
            return "Assigned successfully.";

        }
        public static string RecordUpdateMessage()
        {
            return "Record has been Updated successfully.";

        }
        public static string WorkOrderUpdateMessage()
        {
            return "Work Order has been Updated successfully.";

        }
        public static string SuccessfullyAssigned()
        {
            return "Record has been successfully assigned";
        }
        public static string DeleteSuccessMessage()
        {
            return "Record has been deleted successfully.";
        }
        public static string DeleteNotAllowedMessage()
        {
            return "Field cannot be deleted as it is reffered in another table.";
        }
        public static string LogOnDeleteNotAllowed()
        {
            return "Record cannot be deleted. User has logged-in with the same Name.";
        }
        public static string FailureMessage()
        {
            return "An error occurred while saving/updating the record.";
        }
        public static string DuplicateRecordOnlyMessage()
        {
            return "Record already exist.";
        }
        public static string DuplicateRecordMessage()
        {
            return "Record already exist. Please specify some other name.";
        }
        public static string DuplicateRecordEmailIdMessage()
        {
            //return "Email-Id already exist. Please specify some other Email-Id.";
            return "Email Address provided already exist, please an updated email address.";
        }
        public static string DuplicateEmailIdInSheet()
        {
            return "Please remove duplicate Email-Id from the sheet.";
        }
        public static string DoesNotExistsRecordMessage()
        {
            return "Record was either moved or deleted.";

        }
        public static string NoRecordMessage()
        {
            return "No Record Found";

        }
        public static string InvalidEntry()
        {
            return "Cannot process, invalid record entry.";
        }
        public static string EmailNotification()
        {
            return "Details sent,check your email address.";

        }
        public static string RegistrationNotification()
        {
            return "In order to activate your account please check our activation mail sent to your specified email address. ";

        }
        public static string RecordCannotDelete()
        {
            return "Field cannot be deleted as it is referred with another table.";

        }
        public static string FillAllRequired()
        {
            return "Please fill all required fields.";
        }
        public static string ChooseFile()
        {
            return "Please choose a respective file for import.";
        }

        public static string ConsignmentLimit()
        {
            return "Your consignment limit is over. Please upgrade your plan.";
        }

        public static string ContactLimit()
        {
            return "Your contact limit is over. Please upgrade your plan.";
        }
        public static string ImportContact()
        {
            return "Contact successfully imported.";
        }
        public static string EmailSuccess()
        {
            return "Invitation Email Sent Successfully.";
        }
        public static string EmailInvalid()
        {
            return "Email format is not valid.";
        }
        public static string OldPasswordNotMatched()
        {
            return "Old Password is incorrect";
        }

        public static string EmailExists(string email)
        {
            return "Email id: " + email + " already exists in system.";
        }

        public static string EmployeeIdExists(string employeeId)
        {
            return "Employee ID: " + employeeId + " already exists in system.";
        }

        public static string UserDuplicateEmail(string managerEmail, string userCaption)
        { return userCaption + " Email id: " + managerEmail + " already exist in system."; }

        public static string DuplicateLocationMessage(string locationName)
        { return "Location: " + locationName + " already exist in system. Please specify some other Location Name."; }



        public static string RecoveryPasswordSent(string recoveryEmail)
        {
            string RecoveryMessage = "Invalid Recovery Email";

            if (recoveryEmail != null && !string.IsNullOrEmpty(recoveryEmail))
            { RecoveryMessage = string.Format(CultureInfo.InvariantCulture, "An email with a your password has been sent to the email address: {0}, check your Inbox/spam.", recoveryEmail); }
            return RecoveryMessage;
        }
        public static string RecoveryEmailNotFound(string recoveryEmail)
        {
            string RecoveryMessage = "invalid Recovery Email";

            if (recoveryEmail != null && !string.IsNullOrEmpty(recoveryEmail))
            { RecoveryMessage = string.Format(CultureInfo.InvariantCulture, "Email address: {0} does not exists in database.", recoveryEmail); }
            return RecoveryMessage;
        }

        public static string PermissionUserNotExists()
        { return "User with this permission is not exists in database. please contact with Global Admin."; }

        public static string InvalidUser()
        { return "Cannot process, invalid User."; }

        public static string UserTypeRequire()
        { return "Cannot process, User Type required."; }

        /// <summary>UserAlreadyApproved
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Dec-24-2014</CreatedOn>
        /// </summary>
        /// <param name="myUserType"></param>
        /// <param name="UserName"></param>
        /// <param name="LocationName"></param>
        /// <returns></returns>
        public static string UserAlreadyApproved(UserType myUserType, string userName, string locationName)
        {
            string UserTypeCaption = (myUserType == UserType.Manager) ? "Manager" : "Client";
            return string.Format(CultureInfo.InvariantCulture, "{0}:{1} already verified for the location:{2}.", UserTypeCaption, userName, locationName);
        }

        /// <summary>UserApproved
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Dec-24-2014</CreatedOn>
        /// <CreatedFor>User Approved</CreatedFor>
        /// </summary>
        /// <param name="myUserType"></param>
        /// <param name="UserName"></param>
        /// <param name="LocationName"></param>
        /// <returns></returns>
        public static string UserApproved(UserType myUserType, string userName, string locationName)
        {
            string UserTypeCaption = (myUserType == UserType.Manager) ? "Manager" : "Client";
            return string.Format(CultureInfo.InvariantCulture, "{0}:{1} verified for the location:{2}, please check your Inbox.", UserTypeCaption, userName, locationName);
        }

        /// <summary>
        /// <CreatedBy></CreatedBy>
        /// </summary>
        /// <param name="LocationName"></param>
        /// <returns></returns>
        public static string MessageLocationCreatedSuccessfully(string locationName)
        { return string.Format(CultureInfo.InvariantCulture, "Location: {0} created successfully, please check your Inbox.", locationName); }


        //public static void CleanMessage()
        //{
        //    try
        //    {
        //        ((CommonHelper.UserSession)(HttpContext.Current.Session[UserSession])).ShowMessage = string.Empty;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static void SetHeaderString(string header)
        //{
        //    try
        //    {
        //        ((CommonHelper.UserSession)(HttpContext.Current.Session[UserSession])).Header = header;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static void SetMessageString(string setmessage)
        //{
        //    try
        //    {
        //        ((CommonHelper.UserSession)(HttpContext.Current.Session[UserSession])).ShowMessage = setmessage;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public static void ClearHeader()
        //{
        //    try
        //    {
        //        ((CommonHelper.UserSession)(HttpContext.Current.Session[UserSession])).Header = string.Empty;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static string MessageSuccess()
        {
            return "Your Message has been sent.";
        }

        public static string MessageFailure()
        {
            return "The Message was not sent.";
        }
        public static string DoNotDeleteFreePlan()
        {
            return "For Registration atleast free plan should exist.";
        }
        public static string SendFormSuccess()
        {
            return "Consignment shared successfully";
        }
        public static string SendFormFailure()
        {
            return "Unable to share consignment";
        }

        public static string StatusFailMessage()
        {
            return "You have declined the consignment";
        }

        public static string DeclineFailMessage()
        {
            return "You have accepted the consignment";

        }
        public static string LogOnSuccessMessage()
        {
            return "Successfully Login";
        }

        public static string LogOnFailMessage()
        {
            return " Please check UserName or Password to Login";
        }

        public static string EmailFailMessage()
        {
            return "Please check EmailId again.";
        }

        public static string SessionExpired()
        {
            return "Session expired, please login again.";
        }
        public static string MessageLogOff()
        {
            return "You Have Successfully Logged Out";
        }
        public static string Successful()
        {
            return "Success";
        }
        public static string SuccessfullyUpdated()
        {
            return "Successfully Updated";
        }
        public static string WrongParameterMessage()
        {
            return "The Parameter Is Incorrect ";
        }



        /// <summary>
        /// added by vijay sahu on 26 march 2015
        /// </summary>
        /// <returns></returns>
        public static string DuplicateRecordEmailIdUserNameEmpIdMessage()
        {
            return "Email-Id,UserName or EmployeeId already exist. Please specify some other Email-Id,UserName or Employee ID.";
        }

        /// <summary>
        /// added by Roshan Rahood for the duplicate inventory.
        /// </summary>
        /// <returns></returns>
        public static string DuplicateRecordInventory()
        {
            return "Inventory with the same name already exists.";
        }

        public static string AlreadyAcceptedFacilityRequest()
        {
            return "This facility request has recently accepted by another employee";
        }

        public static string AlreadyAcceptedUrgentWorkRequest()
        {
            return "This urgent work request has recently accepted by another employee";
        }
        public static string EmployeeIdle()
        {
            return "Employee not idle";
        }
        /// <summary>
        /// Added By Ashwajit Bansod for Saving Updating and deleting the eFleet Data
        /// Date : Oct-03-2017
        /// </summary>
        /// <returns></returns>
        public static string eFleetDriverSaveSuccessMessage()
        {
            return "eFleet Driver has been saved successfully.";
        }
        public static string eFleetDriverUpdateSuccessMessage()
        {
            return "eFleet Driver has been Updated successfully.";
        }
        public static string eFleetMaintenanceSaveSuccessMessage()
        {
            return "eFleet Maintenance has been saved successfully.";
        }
        public static string eFleetMaintenanceUpdateSuccessMessage()
        {
            return "eFleet Maintenance has been Updated successfully.";
        }
        public static string eFleetPrevantativeMaintenanceSaveSuccessMessage()
        {
            return "eFleet Prevantative Maintenance has been saved successfully.";
        }
        public static string eFleetPrevantativeMaintenanceUpdateSuccessMessage()
        {
            return "eFleet Prevantative Maintenance has been Updated successfully.";
        }
        public static string eFleetVehicleSaveSuccessMessage()
        {
            return "eFleet Vehicle has been saved successfully.";
        }
        public static string eFleetVehicleUpdateSuccessMessage()
        {
            return "eFleet Vehicle has been Updated successfully.";
        }
        public static string eFleetVehicleIncidentSaveSuccessMessage()
        {
            return "eFleet Vehicle Incident has been saved successfully.";
        }
        public static string eFleetVehicleIncidentUpdateSuccessMessage()
        {
            return "eFleet Vehicle Incident has been Updated successfully.";
        }
    }
}
