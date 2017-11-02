using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;


namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class LoginManager : ILogin
    {
        UserRepository ObjUserRepository;
        //workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();
        EmailLog objEmailog;
        EmailLogRepository objEmailLogRepository = new EmailLogRepository();
        LocationServicesRepository objLocationServicesRepository;
        PermissionDetailsRepository objPermissionDetailsRepository = null;
        LoginLogRepository objLoginLogRepository;
        // ILocationRepository iLocationRepo = null;
        /// <summary>SetLogin
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-20-2014</CreatedOn>
        /// <CreatedFor>Set Login</CreatedFor>
        /// </summary>
        /// <param name="UserTypeId"></param>
        /// <returns></returns>
        public eTracLoginModel SetLogin(long userTypeId)
        {
            try
            {
                eTracLoginModel ObjLogin = new eTracLoginModel();

                switch ((UserType)userTypeId)
                {
                    case UserType.GlobalAdmin:
                        ObjLogin = GlobalAdminLogin();
                        break;
                    case UserType.ITAdministrator:
                        ObjLogin = ITAdministratorLogin();
                        break;
                    case UserType.Administrator:
                        ObjLogin = AdministratorLogin();
                        break;
                    case UserType.Manager:
                        ObjLogin = ManagerLogin();
                        break;
                    case UserType.Employee:
                        ObjLogin = EmployeeLogin();
                        break;
                    case UserType.Client:
                        ObjLogin = ClientLogin();
                        break;
                    default:
                        ObjLogin = GlobalAdminLogin();
                        break;
                }


                return ObjLogin;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>GlobalAdminLogin
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-20-2014</CreatedOn>
        /// <CreatedFor>Global Admin Login</CreatedFor>
        /// </summary>
        /// <returns></returns>
        private eTracLoginModel GlobalAdminLogin()
        {
            try
            {
                eTracLoginModel ObjLogin = GetLoginDetails(Convert.ToInt64(UserType.GlobalAdmin, CultureInfo.InvariantCulture));
                return ObjLogin;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>ITAdministratorLogin
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-20-2014</CreatedOn>
        /// <CreatedFor>IT Administrator Login</CreatedFor>
        /// </summary>
        /// <returns></returns>
        private eTracLoginModel ITAdministratorLogin()
        {
            try
            {
                eTracLoginModel ObjLogin = GetLoginDetails(Convert.ToInt64(UserType.ITAdministrator, CultureInfo.InvariantCulture));
                return ObjLogin;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>AdministratorLogin
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-20-2014</CreatedOn>
        /// <CreatedFor>Administrator Login</CreatedFor>
        /// </summary>
        /// <returns></returns>
        private eTracLoginModel AdministratorLogin()
        {
            try
            {
                eTracLoginModel ObjLogin = GetLoginDetails(Convert.ToInt64(UserType.Administrator, CultureInfo.InvariantCulture));
                return ObjLogin;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>ManagerLogin
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-20-2014</CreatedOn>
        /// <CreatedFor>Manager Login</CreatedFor>
        /// </summary>
        /// <returns></returns>
        private eTracLoginModel ManagerLogin()
        {
            try
            {
                eTracLoginModel ObjLogin = GetLoginDetails(Convert.ToInt64(UserType.Manager, CultureInfo.InvariantCulture));
                //eTracLoginModel ObjLogin = GetManagerLoginDetails(Convert.ToInt64(UserType.Manager));
                return ObjLogin;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>EmployeeLogin
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-20-2014</CreatedOn>
        /// <CreatedFor>Employee Login</CreatedFor>
        /// </summary>
        /// <returns></returns>
        private eTracLoginModel EmployeeLogin()
        {
            try
            {
                eTracLoginModel ObjLogin = GetLoginDetails(Convert.ToInt64(UserType.Employee, CultureInfo.InvariantCulture));
                return ObjLogin;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>ClientLogin
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-20-2014</CreatedOn>
        /// <CreatedFor>Client Login</CreatedFor>
        /// </summary>
        /// <returns></returns>
        private eTracLoginModel ClientLogin()
        {
            try
            {
                eTracLoginModel ObjLogin = GetLoginDetails(Convert.ToInt64(UserType.Client, CultureInfo.InvariantCulture));
                return ObjLogin;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>GetLoginDetails
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-20-2014</CreatedOn>
        /// <CreatedFor>Get Login Details for the User Type</CreatedFor>
        /// </summary>
        /// <param name="myUserType"></param>
        /// <returns></returns>
        private eTracLoginModel GetLoginDetails(long myUserType)
        {
            try
            {
                if (myUserType > 0)
                {
                    ObjUserRepository = new UserRepository();
                    eTracLoginModel ObjLogin = new eTracLoginModel();
                    UserRegistration loginUser = ObjUserRepository.GetSingleOrDefault(u => u.UserType == myUserType && u.IsDeleted == false && u.IsEmailVerify == true && u.IsLoginActive == true);

                    //throw new Exception(CommonMessage.PermissionUserNotExists());

                    if (loginUser != null)
                    {
                        //ObjLogin.ClientUserID = loginUser.UserId;                
                        ObjLogin.FName = loginUser.FirstName;
                        ObjLogin.LName = loginUser.LastName;
                        ObjLogin.Email = loginUser.UserEmail;
                        ObjLogin.UserId = loginUser.UserId;
                        ObjLogin.UserRoleId = myUserType == 1 ? myUserType : loginUser.UserType;
                        //ObjLogin.UserProfile = (loginUser.GlobalCode1 != null && !string.IsNullOrEmpty(loginUser.GlobalCode1.CodeName) ? loginUser.GlobalCode1.CodeName : "");
                        if (loginUser.GlobalCode1 == null) { throw new Exception(CommonMessage.PermissionUserNotExists()); }
                        ObjLogin.UserProfile = (myUserType == 1 ? "Global Admin" : loginUser.GlobalCode1.CodeName);
                        ObjLogin.LocationID = ObjLogin.LocationID;// Temp Location 4

                        //ObjLogin.EmployeeUserID = loginUser.EmployeeID;
                        //ObjLogin.LocationID = loginUser.LocationID.HasValue ? loginUser.LocationID.Value : 0;
                        return ObjLogin;
                    }
                    else { throw new Exception(CommonMessage.InvalidUser()); }
                }
                else { throw new Exception(CommonMessage.UserTypeRequire()); }
            }
            catch (Exception)
            { throw; }

        }

        ///// <summary>Get Logindetails for manager
        ///// <CreatedBy>Gayatri Pal  </CreatedBY>
        ///// <CreatedFor>Authenticate User Login</CreatedFor>
        ///// <CreatedOn>05-Dec-2014</CreatedOn>
        ///// </summary>
        ///// <param name="loginViewModel"></param>
        ///// <returns></returns>
        //private eTracLoginModel GetManagerLoginDetails(long myUserType)
        //{
        //    try
        //    {
        //        if (myUserType > 0)
        //        {
        //            ObjUserRepository = new UserRepository();
        //            eTracLoginModel ObjLogin = new eTracLoginModel();
        //            var loginUser = ObjUserRepository.GetSingleOrDefault(u => u.UserType == myUserType && u.IsDeleted == false && u.IsEmailVerify == true && u.IsLoginActive == true);

        //            if (loginUser != null)
        //            {
        //                //ObjLogin.ClientUserID = loginUser.UserId;                
        //                ObjLogin.FName = loginUser.FirstName;
        //                ObjLogin.LName = loginUser.LastName;
        //                ObjLogin.Email = loginUser.UserEmail;
        //                ObjLogin.UserId = loginUser.UserId;
        //                ObjLogin.UserRoleId = myUserType == 1 ? myUserType : loginUser.UserType;
        //                //ObjLogin.UserProfile = (loginUser.GlobalCode1 != null && !string.IsNullOrEmpty(loginUser.GlobalCode1.CodeName) ? loginUser.GlobalCode1.CodeName : "");
        //                ObjLogin.UserProfile = (myUserType == 1 ? "Global Admin" : loginUser.GlobalCode1.CodeName);
        //                if (loginUser.ManagerLocationMappings.Count() > 0)
        //                    ObjLogin.LocationCode = loginUser.ManagerLocationMappings.FirstOrDefault().LocationMaster.Address2;

        //                //ObjLogin.EmployeeUserID = loginUser.EmployeeID;
        //                //ObjLogin.LocationID = loginUser.LocationID.HasValue ? loginUser.LocationID.Value : 0;
        //                return ObjLogin;
        //            }
        //            else { throw new Exception("Invalid User."); }
        //        }
        //        else { throw new Exception("User Type required"); }
        //    }
        //    catch (Exception)
        //    { throw; }

        //}

        ///////////// <summary>AuthenticateUser
        ///////////// <CreatedBy>Nagendra Upwanshi</CreatedBY>
        ///////////// <CreatedFor>Authenticate User Login</CreatedFor>
        ///////////// <CreatedOn>Nov-27-2014</CreatedOn>
        ///////////// </summary>
        ///////////// <param name="loginViewModel"></param>
        ///////////// <returns></returns>
        //////////public eTracLoginModel AuthenticateUser(eTracLoginModel loginViewModel)
        //////////{
        //////////    ObjUserRepository = new UserRepository();
        //////////    try
        //////////    {
        //////////        string mypassword = Cryptography.GetEncryptedData(loginViewModel.Password, true);
        //////////        //var authuser = ObjUserRepository.GetSingleOrDefault(x => x.UserEmail == loginViewModel.Email && x.Password == mypassword && x.IsDeleted == false && x.IsEmailVerify == true && x.IsLoginActive == true);
        //////////        var authuser = ObjUserRepository.GetSingleOrDefault(x => x.UserEmail == loginViewModel.Email && x.Password == mypassword && x.IsDeleted == false && x.IsEmailVerify == true && x.IsLoginActive == true);

        //////////        eTracLoginModel locationDetails = ObjUserRepository.GetLocationDetailsByUserID(authuser.UserId);

        //////////        if (authuser != null && authuser.UserId > 0)
        //////////        {
        //////////            //Added by Bhushan  on Jan-12-2015 for Validate Login through Serivce call
        //////////            #region Validate Login through Serivce call

        //////////            if ((loginViewModel.DeviceType != null && loginViewModel.DeviceType > 0) || !string.IsNullOrEmpty(loginViewModel.DeviceId))
        //////////            {
        //////////                //Create ServiceAuthKey
        //////////                #region Create ServiceAuthKey

        //////////                loginViewModel.ServiceAuthKey = Guid.NewGuid().ToString();

        //////////                #endregion Create ServiceAuthKey
        //////////                //Update User table with Device details along with ServiceAuthKey                         
        //////////                authuser.DeviceId = loginViewModel.DeviceId;
        //////////                authuser.DeviceType = loginViewModel.DeviceType;
        //////////                authuser.ServiceAuthKey = loginViewModel.ServiceAuthKey;

        //////////                ObjUserRepository.Update(authuser);
        //////////            }

        //////////            #endregion Validate Login through Serivce call
        //////////            loginViewModel.Location = string.IsNullOrEmpty(locationDetails.Location) ? "Not Avaialable" : locationDetails.Location;
        //////////            loginViewModel.LocationID = (locationDetails.LocationID).IsThisNull() ? 0 : locationDetails.LocationID;
        //////////            loginViewModel.UserId = authuser.UserId;
        //////////            loginViewModel.UserRoleId = authuser.UserType;
        //////////            loginViewModel.FName = authuser.FirstName;
        //////////            loginViewModel.LName = authuser.LastName;
        //////////            loginViewModel.Email = authuser.UserEmail;
        //////////            //loginViewModel.LocationID = authuser.;
        //////////            loginViewModel.UserProfile = (authuser.GlobalCode1 != null && !string.IsNullOrEmpty(authuser.GlobalCode1.CodeName) ? authuser.GlobalCode1.CodeName : "");

        //////////            loginViewModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.InvariantCulture);
        //////////            loginViewModel.ServiceAuthKey = authuser.ServiceAuthKey;
        //////////            loginViewModel.ResponseMessage = CommonMessage.LogOnSuccessMessage();
        //////////        }
        //////////        else
        //////////        {
        //////////            loginViewModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.InvariantCulture);
        //////////            loginViewModel.ServiceAuthKey = null;
        //////////            loginViewModel.ResponseMessage = CommonMessage.InvalidUser();
        //////////        }
        //////////    }
        //////////    catch (Exception)
        //////////    { throw; }
        //////////    loginViewModel.Password = string.Empty;
        //////////    return loginViewModel;
        //////////}


        /////////// <summary>RecoveryEmailPassword
        /////////// <CreatedBY>Nagendra Upwanshi</CreatedBY>
        /////////// <CreatedFor>Recovery Email password</CreatedFor>
        /////////// <CreatedOn>Dec-02-2014</CreatedOn>
        /////////// </summary>
        /////////// <param name="eTracLogin"></param>
        /////////// <param name="message"></param>
        /////////// <returns></returns>
        ////////public bool RecoveryEmailPassword(eTracLoginModel eTracLogin, out string message)//, out string RecoverPassword)
        ////////{
        ////////    //RecoverPassword = "";
        ////////    bool isfound = false;
        ////////    message = CommonMessage.RecoveryEmailNotFound(eTracLogin.RecoveryEmail);
        ////////    try
        ////////    {
        ////////        ObjUserRepository = new UserRepository();
        ////////        var user = ObjUserRepository.GetSingleOrDefault(u => u.UserEmail == eTracLogin.RecoveryEmail && u.IsDeleted == false && u.IsEmailVerify == true && u.IsLoginActive == true);

        ////////        //Added By Bhushan Dod on 15-01-2015 for Password by EmailID
        ////////        if (user != null && user.UserId > 0)
        ////////        {
        ////////            eTracLogin.Password = user.Password;
        ////////            isfound = true;
        ////////            message = CommonMessage.RecoveryPasswordSent(eTracLogin.RecoveryEmail);

        ////////            #region Email to Manager User

        ////////            RecoverNmailUserPassword(eTracLogin);

        ////////            #endregion Email to Manager User
        ////////        }
        ////////    }
        ////////    catch (Exception) { throw; } return isfound;
        ////////}


        /// <summary>AuthenticateUser
        /// <CreatedBy>Nagendra Upwanshi</CreatedBY>
        /// <CreatedFor>Authenticate User Login</CreatedFor>
        /// <CreatedOn>Nov-27-2014</CreatedOn>
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        /// <UpdatedBy>Manoj Jaswal</UpdatedBy>
        /// <UpdatedDate>2015/2/19</UpdatedDate>
        /// <UpdatedFor>To remove Login Error and For Login the Application</UpdatedFor>
        /// <UpdatedBy>Bhushan Dod</UpdatedBy>
        /// <UpdatedDate>2015/9/16</UpdatedDate>
        /// <UpdatedFor>To add multiple location string</UpdatedFor>
        public eTracLoginModel AuthenticateUser(eTracLoginModel loginViewModel)
        {
            ObjUserRepository = new UserRepository();
            //objLocationServicesRepository = new LocationServicesRepository();
            objPermissionDetailsRepository = new PermissionDetailsRepository();
            try
            {
                string mypassword = Cryptography.GetEncryptedData(loginViewModel.Password, true);
                var authuser = ObjUserRepository.GetAll(x => x.AlternateEmail == loginViewModel.UserName && x.Password == mypassword && x.IsDeleted == false && x.IsLoginActive == true && x.IsEmailVerify == true).FirstOrDefault();

                if (authuser != null && authuser.UserId > 0)
                {
                    //Added by Bhushan  on Jan-12-2015 for Validate Login through Serivce call
                    #region Validate Login through Serivce call
                    if ((loginViewModel.DeviceType > 0) || !string.IsNullOrEmpty(loginViewModel.DeviceId))
                    {
                        //Create ServiceAuthKey
                        #region Create ServiceAuthKey
                        loginViewModel.ServiceAuthKey = Guid.NewGuid().ToString();
                        #endregion Create ServiceAuthKey
                        //Update User table with Device details along with ServiceAuthKey                        
                        authuser.DeviceId = loginViewModel.DeviceId;
                        authuser.DeviceType = loginViewModel.DeviceType;
                        authuser.ServiceAuthKey = loginViewModel.ServiceAuthKey;
                        authuser.IsOnline = true;
                        authuser.ModifiedBy = authuser.UserId;
                        authuser.ModifiedDate = DateTime.UtcNow;
                        authuser.TokenExpiresOn = DateTime.UtcNow.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["TokenExpiry"]));
                        var timezonename = loginViewModel.TimeZoneName;
                        timezonename = timezonename.Replace("%2F", "/");
                        var timezoneLocal = TimeZoneConverter.TZConvert.IanaToWindows(timezonename);
                        
                        TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                        DateTime myTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, tzi);

                        bool isCurrentlyDaylightSavings = tzi.IsDaylightSavingTime(myTime);
                        long converted;

                        //This code written due to there is difference while daylight timing in backend sp([dbo].[fun_GetClientDateTime] ). Due to this issue it will shows ahead one hour.
                        // dummyConversion = (-1 * Convert.ToInt64(loginViewModel.TimeZoneOffset));
                        authuser.TimeZoneId = Convert.ToInt64(loginViewModel.TimeZoneOffset);
                        if (isCurrentlyDaylightSavings)
                        {
                            converted = Convert.ToInt32(loginViewModel.TimeZoneOffset) - 60;
                            authuser.TimeZoneId = converted;
                            //if (timezoneLocal != "Eastern Standard Time")
                            //{
                            //    converted = Convert.ToInt32(loginViewModel.TimeZoneOffset) - 60;
                            //    authuser.TimeZoneId = converted;
                            //}
                            //else
                            //{
                            //    converted = Convert.ToInt32(loginViewModel.TimeZoneOffset) - 60;
                            //    authuser.TimeZoneId = converted;
                            //}
                        }
                        authuser.TimeZoneName = timezoneLocal;
                        authuser.TimeZoneOffsetName = timezonename;
                        ObjUserRepository.Update(authuser);
                    }
                    else
                    {
                        if (System.Web.HttpContext.Current.Request.Cookies["timezoneoffset"] != null)
                        {
                            //dt = DateTime.UtcNow; ; // may 1st
                            var timezonename = System.Web.HttpContext.Current.Request.Cookies["timezonename"].Value;
                            timezonename = timezonename.Replace("%2F", "/");
                            var timezoneLocal = TimeZoneConverter.TZConvert.IanaToWindows(timezonename);
                            
                            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                            DateTime myTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, tzi);

                            bool isCurrentlyDaylightSavings = tzi.IsDaylightSavingTime(myTime);
                            long converted;
                            long dummyConversion;
                            var timeOffSet = System.Web.HttpContext.Current.Request.Cookies["timezoneoffset"].Value;
                            //This code written due to there is difference while daylight timing in backend sp([dbo].[fun_GetClientDateTime] ). Due to this issue it will shows ahead one hour.
                            dummyConversion = (-1 * Convert.ToInt64(timeOffSet));
                            authuser.TimeZoneId = dummyConversion;
                            if (isCurrentlyDaylightSavings)
                            {
                                converted = Convert.ToInt32(dummyConversion) - 60;
                                authuser.TimeZoneId = converted;
                                //if (timezoneLocal != "Eastern Standard Time")
                                //{
                                //    converted = Convert.ToInt32(dummyConversion) - 60;
                                //    authuser.TimeZoneId = converted;                                    
                                //}
                                //else
                                //{
                                //    converted = Convert.ToInt32(dummyConversion) - 60;
                                //    authuser.TimeZoneId = converted;
                                //}
                            }
                            authuser.TimeZoneName = timezoneLocal;
                            authuser.TimeZoneOffsetName = timezonename;
                            ///Commented by Bhushan Dod on 03/27/2017 for we are doing jugad code for day light saving time.                                                                                 
                        }
                        authuser.IsLoginActive = true;
                        authuser.IsOnline = true;
                        authuser.ModifiedBy = authuser.UserId;
                        authuser.ModifiedDate = DateTime.UtcNow;
                
                        ObjUserRepository.Update(authuser);
                    }

                    #endregion Validate Login through Serivce call
                    //loginViewModel.Location = string.IsNullOrEmpty(locationDetails.Location) ? "Not Avaialable" : locationDetails.Location;
                    LocationMasterModel obj_LocationMasterModel = new LocationMasterModel();
                    switch (authuser.UserType)
                    {
                        case (Int64)(UserType.GlobalAdmin):
                            CommonMethodManager objCommonMethodManager = new CommonMethodManager();
                            obj_LocationMasterModel = GetSuperAdminUserLocation();
                            var locationListAdmin = GetGlobalandITAdminLocationList(authuser.UserId);
                            if (obj_LocationMasterModel != null)
                            {
                                loginViewModel.LocationID = obj_LocationMasterModel.LocationId;
                                loginViewModel.Location = obj_LocationMasterModel.LocationName;
                                loginViewModel.LocationCode = obj_LocationMasterModel.LocationCode;
                            }
                            if (locationListAdmin != null && locationListAdmin.Count > 0)
                            {
                                StringBuilder sb = new StringBuilder();

                                foreach (var t in locationListAdmin)
                                {
                                    var locationService = objPermissionDetailsRepository.GetUserPremissionList(authuser.UserId, authuser.UserType, t.LocationId);
                                    //var locationService1 = objCommonMethodManager.GetPermissionsWithFilterByUserTypeLocationId(locationId, Convert.ToInt32(userId));
                                    if (locationService.Count > 0)
                                    {
                                        sb.Append("[");
                                        foreach (var serviceItem in locationService)
                                        {
                                            sb.Append(serviceItem + ",");
                                        }
                                        sb.Remove(sb.Length - 1, 1);
                                        sb.Append("]@");
                                    }
                                    else
                                    {
                                        sb.Append("[");
                                        sb.Append("]@");
                                    }

                                    loginViewModel.LocationIds += t.LocationId + ",";
                                    loginViewModel.LocationNames += t.LocationName + ",";
                                    loginViewModel.LocationCodes += t.LocationCode + ",";
                                }
                                loginViewModel.LocationServices = sb.ToString();
                                loginViewModel.LocationServices = loginViewModel.LocationServices.Remove(loginViewModel.LocationServices.Length - 1, 1);
                                loginViewModel.LocationIds = loginViewModel.LocationIds.Remove(loginViewModel.LocationIds.Length - 1, 1);
                                loginViewModel.LocationNames = loginViewModel.LocationNames.Remove(loginViewModel.LocationNames.Length - 1, 1);
                                loginViewModel.LocationCodes = loginViewModel.LocationCodes.Remove(loginViewModel.LocationCodes.Length - 1, 1);
                            }
                            break;
                        case (Int64)(UserType.ITAdministrator):
                            obj_LocationMasterModel = GetSuperAdminUserLocation();
                            if (obj_LocationMasterModel != null)
                            {
                                loginViewModel.LocationID = obj_LocationMasterModel.LocationId;
                                loginViewModel.Location = obj_LocationMasterModel.LocationName;
                                loginViewModel.LocationCode = obj_LocationMasterModel.LocationCode;
                            }
                            break;
                        case (Int64)(UserType.Administrator):
                            obj_LocationMasterModel = GetAdminUserLocation_First(authuser.UserId);
                            if (obj_LocationMasterModel != null)
                            {
                                loginViewModel.LocationID = obj_LocationMasterModel.LocationId;
                                loginViewModel.Location = obj_LocationMasterModel.LocationName;
                                loginViewModel.LocationCode = obj_LocationMasterModel.LocationCode;
                            }
                            else
                            {
                                throw new Exception("You are not associated with any location, Please contact to your superior.");
                            }
                            break;
                        case (Int64)(UserType.Manager):
                            obj_LocationMasterModel = GetManageAdminUserLocation_First(authuser.UserId);
                            var locationList = GetManageAdminUserLocationList(authuser.UserId);
                            if (obj_LocationMasterModel != null)
                            {
                                loginViewModel.LocationID = obj_LocationMasterModel.LocationId;
                                loginViewModel.Location = obj_LocationMasterModel.LocationName;
                                loginViewModel.LocationCode = obj_LocationMasterModel.LocationCode;
                            }
                            else
                            {
                                throw new Exception("You are not associated with any location, Please contact to your superior.");
                            }
                            if (locationList != null && locationList.Count > 0)
                            {
                                StringBuilder sb = new StringBuilder();
                                foreach (var t in locationList)
                                {
                                    var locationService = objPermissionDetailsRepository.GetUserPremissionList(authuser.UserId, authuser.UserType, t.LocationId);
                                    if (locationService.Count > 0)
                                    {
                                        sb.Append("[");
                                        foreach (var serviceItem in locationService)
                                        {
                                            sb.Append(serviceItem + ",");
                                        }
                                        sb.Remove(sb.Length - 1, 1);
                                        sb.Append("]@");
                                    }
                                    else
                                    {
                                        sb.Append("[");
                                        sb.Append("]@");
                                    }

                                    loginViewModel.LocationIds += t.LocationId + ",";
                                    loginViewModel.LocationNames += t.LocationName + ",";
                                    loginViewModel.LocationCodes += t.LocationCode + ",";
                                }
                                loginViewModel.LocationServices = sb.ToString();
                                if (loginViewModel.LocationServices.Length != 0)
                                {
                                    loginViewModel.LocationServices = loginViewModel.LocationServices.Remove(loginViewModel.LocationServices.Length - 1, 1);
                                }
                                loginViewModel.LocationIds = loginViewModel.LocationIds.Remove(loginViewModel.LocationIds.Length - 1, 1);
                                loginViewModel.LocationNames = loginViewModel.LocationNames.Remove(loginViewModel.LocationNames.Length - 1, 1);
                                loginViewModel.LocationCodes = loginViewModel.LocationCodes.Remove(loginViewModel.LocationCodes.Length - 1, 1);
                            }
                            break;
                        case (Int64)(UserType.Employee):
                            obj_LocationMasterModel = GetEmployeeUserLocation_First(authuser.UserId);
                            var locationListM = GetEmployeeUserLocationList(authuser.UserId);
                            if (obj_LocationMasterModel != null)
                            {
                                loginViewModel.LocationID = obj_LocationMasterModel.LocationId;
                                loginViewModel.Location = obj_LocationMasterModel.LocationName;
                                loginViewModel.LocationCode = obj_LocationMasterModel.LocationCode;
                            }
                            else
                            {
                                if (locationListM.Count == 0)
                                {
                                    throw new Exception("You are not associated with any location, Please contact to your superior.");
                                }
                            }
                            if (locationListM != null && locationListM.Count > 0)
                            {
                                StringBuilder sb = new StringBuilder();
                                foreach (var t in locationListM)
                                {
                                    var locationService = objPermissionDetailsRepository.GetUserPremissionList(authuser.UserId, authuser.UserType, t.LocationId);
                                    if (locationService.Count > 0)
                                    {
                                        sb.Append("[");
                                        foreach (var serviceItem in locationService)
                                        {
                                            sb.Append(serviceItem + ",");
                                        }
                                        sb.Remove(sb.Length - 1, 1);
                                        sb.Append("]@");
                                    }
                                    else
                                    {
                                        sb.Append("[");
                                        sb.Append("]@");
                                    }

                                    loginViewModel.LocationIds += t.LocationId + ",";
                                    loginViewModel.LocationNames += t.LocationName + ",";
                                    loginViewModel.LocationCodes += t.LocationCode + ",";
                                }
                                loginViewModel.LocationServices = sb.ToString();


                                if (loginViewModel.LocationServices.Length != 0)
                                {
                                    loginViewModel.LocationServices = loginViewModel.LocationServices.Remove(loginViewModel.LocationServices.Length - 1, 1);
                                }
                                loginViewModel.LocationIds = loginViewModel.LocationIds.Remove(loginViewModel.LocationIds.Length - 1, 1);
                                loginViewModel.LocationNames = loginViewModel.LocationNames.Remove(loginViewModel.LocationNames.Length - 1, 1);
                                loginViewModel.LocationCodes = loginViewModel.LocationCodes.Remove(loginViewModel.LocationCodes.Length - 1, 1);
                            }
                            break;
                        case (Int64)(UserType.Client):
                            obj_LocationMasterModel = GetClientUserLocation_First(authuser.UserId);
                            if (obj_LocationMasterModel != null)
                            {
                                loginViewModel.LocationID = obj_LocationMasterModel.LocationId;
                                loginViewModel.Location = obj_LocationMasterModel.LocationName;
                                loginViewModel.LocationCode = obj_LocationMasterModel.LocationCode;
                            }
                            else
                            {
                                throw new Exception("You are not authorised to login, Please contact to your superior.");
                            }
                            break;
                        default:
                            eTracLoginModel locationDetails = ObjUserRepository.GetLocationDetailsByUserID(authuser.UserId);
                            loginViewModel.LocationID = locationDetails == null ? 0 : locationDetails.LocationID;
                            loginViewModel.Location = locationDetails == null ? "Not Avaialable" : locationDetails.Location;
                            loginViewModel.LocationCode = locationDetails == null ? "Not Avaialable" : locationDetails.LocationCode;
                            break;
                    }
                    loginViewModel.LocationID = loginViewModel.LocationID == null ? 0 : loginViewModel.LocationID;
                    loginViewModel.Location = loginViewModel.Location == null ? "Not Avaialable" : loginViewModel.Location;
                    loginViewModel.LocationCode = loginViewModel.LocationCode == null ? "Not Avaialable" : loginViewModel.LocationCode;
                    loginViewModel.UserId = authuser.UserId;
                    loginViewModel.UserRoleId = authuser.UserType;
                    loginViewModel.FName = authuser.FirstName;
                    loginViewModel.LName = authuser.LastName;
                    loginViewModel.Email = authuser.UserEmail;
                    loginViewModel.Password = authuser.Password;
                    loginViewModel.ProfileImage = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/ProfilePic/" + authuser.ProfileImage;
                    //loginViewModel.LocationID = authuser.;
                    loginViewModel.UserName = authuser.AlternateEmail;
                    loginViewModel.UserProfile = (authuser.GlobalCode1 != null && !string.IsNullOrEmpty(authuser.GlobalCode1.CodeName) ? authuser.GlobalCode1.CodeName : "");
                    loginViewModel.IdleTime = authuser.IdleTimeLimit.ToString("HH:mm:ss");

                    if (loginViewModel.LocationID != 0)
                    {
                        loginViewModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.InvariantCulture);
                        loginViewModel.ServiceAuthKey = authuser.ServiceAuthKey;
                        loginViewModel.ResponseMessage = CommonMessage.LogOnSuccessMessage();
                    }

                    //Added By Bhushan Dod on 06/05/2016 for if user logged in but forget to logout or shift end so next time only one login active by web or mobile.
                    if (authuser.UserId > 0)
                    {
                        objLoginLogRepository = new LoginLogRepository();
                        var listActive = objLoginLogRepository.GetAll(obl => obl.UserID == authuser.UserId && obl.IsActive == true && DbFunctions.TruncateTime(obl.CreatedOn) == DbFunctions.TruncateTime(DateTime.UtcNow));
                        if (listActive.Count() > 0)
                        {
                            foreach (var active in listActive)
                            {
                                active.IsActive = false;
                            }
                            objLoginLogRepository.SaveChanges();
                        }
                    }
                    //Commented by Bhushan Dod for maintaining log of each and every login user.
                    if (((loginViewModel.DeviceId == null || loginViewModel.DeviceId.Trim().Length <= 0) && authuser.UserType == 3) || (loginViewModel.DeviceId == null || loginViewModel.DeviceId.Trim().Length <= 0)) //it will let us know that user is loged in from web or from mobile.. 
                    {
                        //For login log maintian

                        objLoginLogRepository = new LoginLogRepository();
                        LoginLog Obj = new LoginLog();

                        Obj.UserID = authuser.UserId;
                        Obj.LocationId = loginViewModel.LocationID;
                        Obj.UserType = authuser.UserType;
                        Obj.CreatedBy = authuser.UserId;
                        Obj.CreatedOn = DateTime.UtcNow;
                        Obj.DeletedBy = null;
                        Obj.DeletedOn = null;
                        Obj.IsDeleted = false;
                        Obj.ModifiedBy = null;
                        Obj.ModifiedOn = null;
                        Obj.IsActive = true;
                        objLoginLogRepository.Add(Obj);
                        loginViewModel.LogId = Obj.LogId;
                        loginViewModel.IdleTime = authuser.IdleTimeLimit.ToString("HH:mm:ss");
                        loginViewModel.LoginTime = Obj.CreatedOn.ToString();
                    }
                }
                else
                {
                    loginViewModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.InvariantCulture);
                    loginViewModel.ServiceAuthKey = null;
                    loginViewModel.ResponseMessage = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public eTracLoginModel AuthenticateUser(eTracLoginModel loginViewModel)", "from login controller", loginViewModel);
                throw;
            }
            //loginViewModel.Password = string.Empty;
            return loginViewModel;
        }

        public bool RecoveryEmailPassword(eTracLoginModel eTracLogin, out string message)//, out string RecoverPassword)
        {
            //RecoverPassword = "";
            bool isfound = false;
            message = CommonMessage.RecoveryEmailNotFound(eTracLogin.RecoveryEmail);
            try
            {
                ObjUserRepository = new UserRepository();
                var user = ObjUserRepository.GetSingleOrDefault(u => u.UserEmail == eTracLogin.RecoveryEmail && u.IsDeleted == false && u.IsEmailVerify == true && u.IsLoginActive == true);

                //Added By Bhushan Dod on 15-01-2015 for Password by EmailID
                if (user != null && user.UserId > 0)
                {
                    eTracLogin.Password = user.Password;
                    eTracLogin.FName = user.FirstName;
                    eTracLogin.LName = user.LastName;
                    eTracLogin.UserName = user.AlternateEmail;
                    isfound = true;
                    message = CommonMessage.RecoveryPasswordSent(eTracLogin.RecoveryEmail);

                    #region Email to Manager User

                    RecoverNmailUserPassword(eTracLogin);

                    #endregion Email to Manager User
                }
                else
                {
                    message = CommonMessage.RecoveryEmailNotFound(eTracLogin.RecoveryEmail);
                }
            }
            catch (Exception) { throw; } return isfound;
        }

        /// <summary>RecoverNmailUserPassword
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedFor>Recover and mail User Password</CreatedFor>
        /// <CreatedOn>Dec-02-2014</CreatedOn>
        /// </summary>
        /// <param name="eTracLogin"></param>
        /// <returns></returns>
        private bool RecoverNmailUserPassword(eTracLoginModel eTracLogin)
        {
            try
            {

                EmailHelper objEmailHelper = new EmailHelper();
                objEmailHelper.emailid = eTracLogin.RecoveryEmail;
                objEmailHelper.FirstName = eTracLogin.RecoveryEmail;
                objEmailHelper.UserName = eTracLogin.UserName;
                objEmailHelper.Password = Cryptography.GetDecryptedData(eTracLogin.Password, true);
                objEmailHelper.MailType = "FORGOTPASSWORD";
                string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);

                objEmailHelper.RegistrationLink = HostingPrefix;
                bool IsSent = objEmailHelper.SendEmailWithTemplate();
                if (IsSent == true)
                {
                    objEmailog = new EmailLog();
                    try
                    {

                        objEmailog.CreatedBy = eTracLogin.UserId;
                        objEmailog.CreatedDate = DateTime.UtcNow;
                        objEmailog.DeletedBy = null;
                        objEmailog.DeletedOn = null;
                        objEmailog.LocationId = null;
                        objEmailog.ModifiedBy = null;
                        objEmailog.ModifiedOn = null;
                        objEmailog.SentBy = null;
                        objEmailog.SentEmail = objEmailHelper.emailid;
                        objEmailog.Subject = objEmailHelper.Subject;
                        objEmailog.isForgot = true;
                        objEmailog.SentTo = null;

                        objEmailLogRepository.SaveEmailLog(objEmailog);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception) { throw; } return true;
        }

        /// <summary>ChangePassword
        /// <Createdby>Bhushan Dod</Createdby>
        /// <CreatedDate>Jan-15-2015</CreatedDate>
        /// </summary>
        /// <param name="OldPassword"></param>
        /// <param name="NewPassword"></param>
        /// <returns></returns>
        public bool ChangePassword(eTracLoginModel eTracLogin, out string message)
        {
            bool isfound = false;
            try
            {
                ObjUserRepository = new UserRepository();
                // var user = ObjUserRepository.GetSingleOrDefault(u => u.ServiceAuthKey == eTracLogin.ServiceAuthKey && u.IsDeleted == false && u.IsEmailVerify == true && u.IsLoginActive == true);
                string mypassword = Cryptography.GetEncryptedData(eTracLogin.OldPassword, true);
                var UserDetail = ObjUserRepository.GetSingleOrDefault(u => u.UserId == eTracLogin.UserId && u.Password == mypassword && u.ServiceAuthKey == eTracLogin.ServiceAuthKey);
                if (UserDetail != null && UserDetail.UserId > 0)
                {
                    UserDetail.Password = Cryptography.GetEncryptedData(eTracLogin.NewPassword, true); //eTracLogin.NewPassword;
                    ObjUserRepository.Update(UserDetail);
                    isfound = true;
                    message = CommonMessage.SuccessfullyUpdated();
                }
                else
                {
                    isfound = false;
                    message = CommonMessage.OldPasswordNotMatched();

                }

            }
            catch (Exception)
            {
                throw;
            }
            return isfound;
        }


        /// <summary>Logout User
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor> User Logout</CreatedFor>
        /// <CreatedOn>Jan-12-2015</CreatedOn>
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        public eTracLoginModel Logout(eTracLoginModel logoutViewModel)
        {
            ObjUserRepository = new UserRepository();
            objLoginLogRepository = new LoginLogRepository();
            try
            {
                var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == logoutViewModel.ServiceAuthKey && x.IsDeleted == false && x.IsEmailVerify == true && x.IsLoginActive == true);

                if (authuser != null && authuser.UserId > 0)
                {
                    //Added by Bhushan on Jan-12-2015 for Validate Login through Serivce call
                    #region Validate Logout through Serivce call
                    //Update User table with Device details along with ServiceAuthKey                         

                    authuser.DeviceId = null;
                    //authuser.DeviceType = null;
                    authuser.ServiceAuthKey = null;
                    authuser.IsOnline = false;
                    authuser.ModifiedBy = authuser.UserId;
                    authuser.ModifiedDate = DateTime.UtcNow;
                    ObjUserRepository.Update(authuser);

                    var loginlog = objLoginLogRepository.GetSingleOrDefault(x => x.UserID == authuser.UserId && x.LogId == logoutViewModel.LogId && x.IsDeleted == false && x.IsActive == true);

                    if (loginlog.LogId != 0)
                    {
                        loginlog.IsActive = false;
                        objLoginLogRepository.Update(loginlog);
                    }
                    #endregion Validate Logout through Serivce call

                    logoutViewModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.InvariantCulture);
                    logoutViewModel.ServiceAuthKey = null;
                    logoutViewModel.ResponseMessage = CommonMessage.MessageLogOff();
                }
                else
                {
                    logoutViewModel.Response = Convert.ToInt32(ServiceResponse.InvalidSessionResponse, CultureInfo.InvariantCulture);
                    logoutViewModel.ServiceAuthKey = null;
                    logoutViewModel.ResponseMessage = CommonMessage.InvalidUser();
                }
            }
            catch (Exception)
            { throw; }

            return logoutViewModel;
        }

        public Result LogoutWeb(long userId, long loginLogID)
        {
            ObjUserRepository = new UserRepository();
            objLoginLogRepository = new LoginLogRepository();

            try
            {

                var authuser = ObjUserRepository.GetSingleOrDefault(x => x.UserId == userId && x.IsDeleted == false && x.IsEmailVerify == true && x.IsLoginActive == true);

                if (authuser != null && authuser.UserId > 0)
                {
                    //Added by Bhushan on Jan-12-2015 for Validate Login through Serivce call
                    #region Validate Logout through Serivce call
                    //Update User table with Device details along with ServiceAuthKey                         

                    authuser.DeviceId = null;
                    //authuser.DeviceType = null;
                    authuser.ServiceAuthKey = null;
                    authuser.IsOnline = false;
                    authuser.ModifiedBy = authuser.UserId;
                    authuser.ModifiedDate = DateTime.UtcNow;
                    ObjUserRepository.Update(authuser);

                    var loginlog = objLoginLogRepository.GetSingleOrDefault(x => x.UserID == userId && x.LogId == loginLogID && x.IsDeleted == false && x.IsActive == true);

                    if (loginlog.LogId != 0)
                    {
                        loginlog.IsActive = false;
                        objLoginLogRepository.Update(loginlog);
                    }
                    return Result.Completed;

                    #endregion Validate Logout through Serivce call

                }
                else
                {
                    return Result.Failed;
                }
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>Get task list by employee id
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedOn>Jan-12-2015</CreatedOn>
        /// <CreatedFor> GetTaskListByEmpID</CreatedFor>
        /// </summary>
        /// <param name="ServiceAuthKey"></param>
        /// <returns></returns>
        public List<ServiceWorkAssignmentModel> GetTaskListByEmployeeId(string serviceAuthKey, long userId, string fromDate, string toDate, long locationId, string TimeZoneName, long TimeZoneOffset, bool IsTimeZoneinDaylight)
        {
            WorkRequestAssignmentRepository _WorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
            try
            {
                List<ServiceWorkAssignmentModel> tasklist = _WorkRequestAssignmentRepository.GetTaskListByEmpID(serviceAuthKey, userId,
                                                             fromDate, toDate, locationId, TimeZoneName,  TimeZoneOffset, IsTimeZoneinDaylight).Select(t => new ServiceWorkAssignmentModel()
                {
                    WorkRequestAssignmentID = t.WorkRequestAssignmentID,
                    AssetID = t.AssetID,
                    QRCName = t.QRCName,
                    AssetName = t.AssetName,
                    WorkRequestType = t.WorkRequestType,
                    WorkRequestTypeName = t.WorkRequestProjectTypeName,
                    ProblemDescription = t.ProblemDesc,
                    ProjectDescription = t.ProjectDesc,
                    WorkRequestStatus = t.WorkRequestStatus,
                    WorkRequestStatusName = t.WorkRequestStatusName,
                    WorkRequestProjectType = t.WorkRequestProjectType,
                    WorkRequestProjectTypeName = t.WorkRequestTypeCodeName,
                    PriorityLevel = t.PriorityLevel,
                    SafetyHazard = t.SafetyHazard,
                    LocationID = t.LocationID,
                    LocationName = t.LocationName,
                    AssignByUserId = t.AssignByUserId,
                    AssignByUserName = t.AssignByUserName,
                    RequestBy = t.RequestBy,
                    RequestByName = t.RequestByName,
                    UserType = t.UserType,
                    CreatedDate = t.CreatedDate.ToString(),
                    WorkRequestImage = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/WorkRequest/" + t.WorkRequestImage,
                    WorkrequestCode = t.WorkOrderCode + t.WorkOrderCodeID.ToString(),
                    FrCurrentLocation = t.CurrentLocation,
                    CustomerContact = t.CustomerContact,
                    CustomerName = t.CustomerName,
                    DriverLicenseNo = t.DriverLicenseNo,
                    VehicleColor = t.VehicleColor,
                    VehicleMake1 = t.VehicleMake,
                    VehicleModel1 = t.VehicleModel,
                    VehicleYear = t.VehicleYear.ToString(),
                    AddressFacilityReq = t.Address,
                    LicensePlateNo = t.LicensePlateNo,
                    FacilityRequest = t.FacilityRequest.ToString()


                }).ToList();
                // List<ServiceWorkAssignmentModel> tasklist = new List<ServiceWorkAssignmentModel>();
                return tasklist;
            }
            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<ServiceWorkAssignmentModel> GetTaskListByEmployeeId(string serviceAuthKey, long userId, string fromDate, string toDate, long locationId)", "from c# while fetch work list loginmanager.cs", locationId);
                throw;
            }
        }


        /// <summary>Get continuous assignment task list by employee id
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedOn>Aug-26-2015</CreatedOn>
        /// <CreatedFor> GetContinuousTaskListByEmpID</CreatedFor>
        /// </summary>
        /// <param name="ServiceAuthKey"></param>
        /// <returns></returns>
        public List<ServiceWorkAssignmentModel> GetContinuousTaskListByEmployeeId(string serviceAuthKey, long userId, long locationId)
        {
            WorkRequestAssignmentRepository _WorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
            try
            {
                List<ServiceWorkAssignmentModel> tasklist = _WorkRequestAssignmentRepository.GetContinuousTaskListByEmpID(serviceAuthKey, userId,
                                                          locationId).Select(t => new ServiceWorkAssignmentModel()
                                                             {
                                                                 WorkRequestAssignmentID = t.WorkRequestAssignmentID,
                                                                 ProjectDescription = t.ProjectDesc,
                                                                 PriorityLevel = t.PriorityLevel,
                                                                 LocationID = t.LocationID,
                                                                 LocationName = t.LocationName,
                                                                 AssignByUserId = t.AssignByUserId,
                                                                 AssignByUserName = t.AssignByUserName,
                                                                 UserType = t.UserType,
                                                                 CreatedDate = t.CreatedDate.ToString("MM/dd/yyyy hh:mm tt"),
                                                                 WorkRequestImage = t.AssignedWorkOrderImage == null ? null : ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/WorkRequest/" + t.AssignedWorkOrderImage,
                                                                 WorkrequestCode = t.WorkOrderCode + t.WorkOrderCodeID.ToString(),
                                                                 StartDate = t.StartDate.ToString("MM/dd/yyyy"),
                                                                 EndDate = t.EndDate.ToString("MM/dd/yyyy"),
                                                                 StartTime = t.StartTime.ToString("hh:mm tt"),
                                                                 AssignedTime = t.AssignedTime.ToString("HH:MM"),
                                                                 WeekDaysName = t.WeekDaysName,
                                                                 WorkRequestProjectType = t.WorkRequestProjectType,
                                                                 WorkRequestProjectTypeName = t.WorkRequestProjectTypeName,
                                                                 WorkRequestType = t.WorkRequestType,
                                                                 WorkRequestTypeName = t.WorkRequestProjectTypeName,
                                                                 WorkRequestStatus = t.WorkRequestStatus,
                                                                 WorkRequestStatusName = t.WorkRequestStatusName
                                                             }).ToList();
                // List<ServiceWorkAssignmentModel> tasklist = new List<ServiceWorkAssignmentModel>();
                return tasklist;
            }
            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<ServiceWorkAssignmentModel> GetTaskListByEmployeeId(string serviceAuthKey, long userId, string fromDate, string toDate, long locationId)", "from c# while fetch work list loginmanager.cs", locationId);
                throw;
            }
        }


        /// <summary>
        /// Created by Manoj Jaswal on 20 feb 2015
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<string> GetUserPremissionList(long userId, long Usertype, long locationId)
        {
            PermissionDetailsRepository obj_PremissionDetailsRepository = new PermissionDetailsRepository();
            //var result = obj_PremissionDetailsRepository.GetAll(x => x.UserId == UserID && x.IsDeleted == false)

            return obj_PremissionDetailsRepository.GetUserPremissionList(userId, Usertype, locationId);
        }

        /// <summary>
        /// To Get the All Locations For SuperAdmin
        /// </summary>
        /// <returns></returns>
        private LocationMasterModel GetSuperAdminUserLocation()
        {
            LocationMasterRepository obj_LocationMasterRepository = new LocationMasterRepository();
            return obj_LocationMasterRepository.GetAll(x => x.IsDeleted == false).Select(x => new LocationMasterModel()
            {
                LocationId = x.LocationId,
                LocationName = x.LocationName,
                LocationCode = x.Address2,
            }).FirstOrDefault<LocationMasterModel>();
        }

        public List<UserLocations> GetUserAssignedLocations(long UserType, long UserID)
        {
            //LocationMasterRepository obj_LocationMasterRepository = new LocationMasterRepository();
            //return obj_LocationMasterRepository.GetAll(x => x.IsDeleted == false).Select(x => new LocationMasterModel()
            //{
            //    LocationId = x.LocationId,
            //    LocationName = x.LocationName,
            //}).FirstOrDefault<LocationMasterModel>();s
            LocationRepository obj_LocationRepository = new LocationRepository();
            List<UserLocations> obj_UserLocations = obj_LocationRepository.GetUserLocations(UserType, UserID);
            return obj_UserLocations;

        }
        private LocationMasterModel GetManageAdminUserLocation_First(long UserID)
        {
            ManagerLocationMappingRepository obj_ManagerLocationMappingRepository = new ManagerLocationMappingRepository();
            return obj_ManagerLocationMappingRepository.GetManagerLocationList(UserID).Select(x => new LocationMasterModel()
            {
                LocationId = Convert.ToInt64(x.LocationID),
                LocationName = x.LocationName,
                LocationCode = x.LocationCode,
            }).FirstOrDefault<LocationMasterModel>();
        }
        public List<UserLocations> GetManagerAssignedLocation(long UserID)
        {
            ManagerLocationMappingRepository obj_ManagerLocationMappingRepository = new ManagerLocationMappingRepository();
            return obj_ManagerLocationMappingRepository.GetManagerLocationList(UserID);
        }

        private LocationMasterModel GetEmployeeUserLocation_First(long UserID)
        {
            EmployeeLocationMappingRepository obj_ManagerLocationMappingRepository = new EmployeeLocationMappingRepository();
            return obj_ManagerLocationMappingRepository.GetEmployeeLocationList(UserID).Select(x => new LocationMasterModel()
            {
                LocationId = Convert.ToInt64(x.LocationID),
                LocationName = x.LocationName,
                LocationCode = x.LocationCode,
            }).FirstOrDefault<LocationMasterModel>();
        }

        /// <summary>
        /// Created By :- Bhushan Dod
        /// Created Date :- 27/10/2015
        /// Description :- Get all Location List for global admin and it admin
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<LocationMasterModel> GetGlobalandITAdminLocationList(long UserID)
        {
            ManagerLocationMappingRepository obj_ManagerLocationMappingRepository = new ManagerLocationMappingRepository();
            return obj_ManagerLocationMappingRepository.GetAllLocationList(UserID).Select(x => new LocationMasterModel()
            {
                LocationId = Convert.ToInt64(x.LocationID),
                LocationName = x.LocationName,
                LocationCode = x.LocationCode,
            }).ToList<LocationMasterModel>();
        }

        /// <summary>
        /// Created By :- Bhushan Dod
        /// Created Date :- 06/16/2015
        /// Description :- Get Employee Location List
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        private List<LocationMasterModel> GetEmployeeUserLocationList(long UserID)
        {
            EmployeeLocationMappingRepository obj_ManagerLocationMappingRepository = new EmployeeLocationMappingRepository();
            return obj_ManagerLocationMappingRepository.GetEmployeeLocationList(UserID).Select(x => new LocationMasterModel()
            {
                LocationId = Convert.ToInt64(x.LocationID),
                LocationName = x.LocationName,
                LocationCode = x.LocationCode,
            }).ToList<LocationMasterModel>();
        }

        /// <summary>
        /// Created By :- Bhushan Dod
        /// Created Date :- 22/06/2015
        /// Description :- Get Admin Location List
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<LocationMasterModel> GetAdminUserLocationList(long UserID)
        {
            ManagerLocationMappingRepository obj_ManagerLocationMappingRepository = new ManagerLocationMappingRepository();
            return obj_ManagerLocationMappingRepository.GetAdminLocationList(UserID).Select(x => new LocationMasterModel()
            {
                LocationId = Convert.ToInt64(x.LocationID),
                LocationName = x.LocationName,
                LocationCode = x.LocationCode,
            }).ToList<LocationMasterModel>();
        }

        /// <summary>
        /// Created By :- Bhushan Dod
        /// Created Date :- 06/16/2015
        /// Description :- Get Manager Location List
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<LocationMasterModel> GetManageAdminUserLocationList(long UserID)
        {
            ManagerLocationMappingRepository obj_ManagerLocationMappingRepository = new ManagerLocationMappingRepository();
            return obj_ManagerLocationMappingRepository.GetManagerLocationList(UserID).Select(x => new LocationMasterModel()
            {
                LocationId = Convert.ToInt64(x.LocationID),
                LocationName = x.LocationName,
                LocationCode = x.LocationCode,
            }).ToList<LocationMasterModel>();
        }
        public List<LocationMasterModel> GetClientUserLocationList(long UserID)
        {
            LocationClientMappingRepository obj_LocationClientMappingRepository = new LocationClientMappingRepository();
            return obj_LocationClientMappingRepository.GetClientLocationList(UserID).Select(x => new LocationMasterModel()
            {
                LocationId = Convert.ToInt64(x.LocationID),
                LocationName = x.LocationName,
                LocationCode = x.LocationCode,
            }).ToList<LocationMasterModel>();
        }
        /// <summary>
        /// To Get Client Location ID
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        private LocationMasterModel GetClientUserLocation_First(long UserID)
        {
            LocationClientMappingRepository obj_LocationClientMappingRepository = new LocationClientMappingRepository();
            return obj_LocationClientMappingRepository.GetClientLocationList(UserID).Select(x => new LocationMasterModel()
            {
                LocationId = Convert.ToInt64(x.LocationID),
                LocationName = x.LocationName,
                LocationCode = x.LocationCode,
            }).FirstOrDefault<LocationMasterModel>();
        }
        public List<UserLocations> GetEmployeeAssignedLocation(long UserID)
        {
            EmployeeLocationMappingRepository obj_ManagerLocationMappingRepository = new EmployeeLocationMappingRepository();
            return obj_ManagerLocationMappingRepository.GetEmployeeLocationList(UserID);
        }

        private LocationMasterModel GetAdminUserLocation_First(long UserID)
        {
            AdminLocationMappingRepository obj_AdminLocationMappingRepository = new AdminLocationMappingRepository();
            return obj_AdminLocationMappingRepository.GetAdminLocationList(UserID).Select(x => new LocationMasterModel()
            {
                LocationId = Convert.ToInt64(x.LocationID),
                LocationName = x.LocationName,
                LocationCode = x.LocationCode,

            }).FirstOrDefault<LocationMasterModel>();
        }
        public List<UserLocations> GetAdminAssignedLocation(long UserID)
        {
            AdminLocationMappingRepository obj_AdminLocationMappingRepository = new AdminLocationMappingRepository();
            return obj_AdminLocationMappingRepository.GetAdminLocationList(UserID);
        }

        public eTracLoginModel InsertLoginLog(eTracLoginModel objeTracLoginModel)
        {
            //For login log maintian
            try
            {
                objLoginLogRepository = new LoginLogRepository();
                LoginLog Obj = new LoginLog();

                Obj.UserID = objeTracLoginModel.UserId;
                Obj.LocationId = objeTracLoginModel.LocationID;
                Obj.UserType = objeTracLoginModel.UserRoleId;
                Obj.CreatedBy = objeTracLoginModel.UserId;
                Obj.CreatedOn = DateTime.UtcNow;
                Obj.DeletedBy = null;
                Obj.DeletedOn = null;
                Obj.IsDeleted = false;
                Obj.ModifiedBy = null;
                Obj.ModifiedOn = null;
                Obj.IsActive = true;
                objLoginLogRepository.Add(Obj);
                objeTracLoginModel.LogId = Obj.LogId;
                //objeTracLoginModel.IdleTime = authuser.IdleTimeLimit.ToString("HH:mm:ss");
                objeTracLoginModel.LoginTime = Obj.CreatedOn.ToString();
                if (Obj.LogId != 0 && Obj.LogId > 0)
                {
                    objeTracLoginModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.InvariantCulture);
                    objeTracLoginModel.ResponseMessage = CommonMessage.LogOnSuccessMessage();
                }
            }
            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "eTracLoginModel InsertLoginLog(eTracLoginModel objeTracLoginModel)", "while insert login log loginmanager.cs", objeTracLoginModel);
                throw;
            }
            return objeTracLoginModel;
        }

        /// <summary>To Update loginlog active status.
        /// <Createdby>Bhushan Dod</Createdby>
        /// <CreatedDate>April-25-2016</CreatedDate>
        /// </summary>
        /// <param name="eTracLogin"></param>
        /// <returns></returns>
        public bool ChangeLoginLogActiveStatus(eTracLoginModel eTracLogin)
        {
            bool isfound = false;
            try
            {

                //Added By Bhushan Dod on 06/05/2016 for if user logged in but forget to logout or shift end so next time only one login active by web or mobile.
                if (eTracLogin.UserId > 0)
                {
                    objLoginLogRepository = new LoginLogRepository();
                    var listActive = objLoginLogRepository.GetAll(obl => obl.UserID == eTracLogin.UserId && obl.IsActive == true && DbFunctions.TruncateTime(obl.CreatedOn) == DbFunctions.TruncateTime(DateTime.UtcNow));
                    if (listActive.Count() > 0)
                    {
                        foreach (var active in listActive)
                        {
                            active.IsActive = false;
                        }
                        objLoginLogRepository.SaveChanges();
                    }
                }

                objLoginLogRepository = new LoginLogRepository();
                var loginlog = objLoginLogRepository.GetSingleOrDefault(x => x.UserID == eTracLogin.UserId && x.LogId == eTracLogin.LogId && x.IsDeleted == false && x.IsActive == true);
                if (loginlog.LogId != 0)
                {
                    loginlog.IsActive = false;
                    objLoginLogRepository.Update(loginlog);
                    isfound = true;
                }
                else
                {
                    isfound = false;
                }
            }
            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "bool ChangeLoginLogActiveStatus(eTracLoginModel eTracLogin)", "Update loginlog active status.", eTracLogin);
                isfound = false;
            }
            return isfound;
        }

        /// <summary>
        /// Created by Bhushan Dod on 12 May 2016
        /// For Dashboard widget setting 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public WidgetList GetDashboardWidgetList(long userId, long locationId)
        {
            try
            {
                PermissionDetailsRepository obj_PremissionDetailsRepository = new PermissionDetailsRepository();
                return obj_PremissionDetailsRepository.GetUserDashboardSettingList(userId, locationId);
            }
            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "WidgetList GetDashboardWidgetList(long userId, long locationId)", "GetDashboardWidgetList", userId);
                throw;
            }

        }
    }
}
