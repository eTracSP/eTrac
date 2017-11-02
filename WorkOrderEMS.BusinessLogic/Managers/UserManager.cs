using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Text;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.UserModels;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class UserManager : IUser
    {
        UserRepository ObjUserRepository;
        IUserRepository _iUserRepository;
        ICommonMethod _ICommonMethod;
        CommonMethodManager objCommonMethodManager;

        string ProfilePicPath = ConfigurationManager.AppSettings["ProfilePicPath"];
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        public UserModel GetUserDetailsById(long userId)
        {
            try
            {
                ObjUserRepository = new UserRepository();
                UserModel editUserDetails = new UserModel();
                //editUserDetails.Address OBJAdress = new AddressModel();
                AddressMaster ObjAddressMaster = new AddressMaster();
                UserRegistration userDetails = ObjUserRepository.GetSingleOrDefault(u => u.UserId == userId);

                if (userDetails != null)
                {

                    ObjAddressMaster = userDetails.AddressMasters.FirstOrDefault();
                    editUserDetails.EmployeeID = userDetails.EmployeeID;
                    editUserDetails.UserId = userDetails.UserId;
                    editUserDetails.UserName = userDetails.AlternateEmail;
                    editUserDetails.FirstName = userDetails.FirstName;
                    editUserDetails.LastName = userDetails.LastName;
                    editUserDetails.myProfileImage = userDetails.ProfileImage == null ? "" : HostingPrefix + ProfilePicPath.Replace("~", "") + userDetails.ProfileImage;
                    editUserDetails.Email = userDetails.UserEmail;
                    editUserDetails.UserEmail = userDetails.UserEmail;
                    editUserDetails.SubscriptionEmail = userDetails.SubscriptionEmail;
                    editUserDetails.UserType = userDetails.UserType;
                    editUserDetails.IsLoginActive = userDetails.IsLoginActive;
                    editUserDetails.IsEmailVerify = userDetails.IsEmailVerify;
                    editUserDetails.Password = userDetails.Password;
                    editUserDetails.AlternateEmail = userDetails.AlternateEmail;
                    editUserDetails.Gender = userDetails.Gender;
                    editUserDetails.DOB = Convert.ToDateTime(userDetails.DOB).ToString("MM/dd/yy");
                    //editUserDetails.Address.Address1 = ObjAddressMaster.Address1;
                    //editUserDetails.Address.Address2 = ObjAddressMaster.Address2;
                    //Commented by Bhushan Dod due to Error on below line
                    editUserDetails.Address = new AddressModel() { Address1 = ObjAddressMaster.Address1, Address2 = ObjAddressMaster.Address2 };

                    //editUserDetails.Address = userDetails.AddressMasters

                    //AutoMapper.Mapper.CreateMap<UserRegistration, UserModel>();
                    //  editUserDetails = AutoMapper.Mapper.Map(userDetails, editUserDetails);
                    //editUserDetails.myProfileImage = editUserDetails.ProfileImage == null ? "" : ProfilePicPath.Replace("~", "") + editUserDetails.ProfileImage;
                    //editUserDetails.Address = new AddressModel() { Address1 = ObjAddressMaster.Address1, Address2 = ObjAddressMaster.Address2 };
                }
                return editUserDetails;

            }
            catch (Exception)
            {
                throw;
            }

        }
        /// <summary>
        /// To Delete the User
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>2015-03-05</CreatedDate>
        /// <param name="UserId"></param>
        /// <param name="DeletedBy"></param>
        public Result DeleteUser(long UserId, long DeletedBy, DARModel objDAR)
        {
            long UserType = 0;
            Result result;
            long count = 0;
            try
            {
                ObjUserRepository = new UserRepository();
                objCommonMethodManager = new CommonMethodManager();
                UserRegistration obj_UserRegistration = ObjUserRepository.GetAll(x => x.UserId == UserId).FirstOrDefault();
                if (obj_UserRegistration != null)
                {

                    UserType = obj_UserRegistration.UserType;

                    using (workorderEMSEntities context = new workorderEMSEntities())
                    {

                        if (UserType == 6)
                        {
                            count = (from o in context.AdminLocationMappings
                                     join ur in context.UserRegistrations
                                     on o.AdminUserId equals ur.UserId

                                     where
                                     ur.IsLoginActive == true
                                     && ur.IsEmailVerify == true
                                     && ur.IsDeleted == false
                                      && ur.UserId == UserId
                                     && o.IsDeleted == false
                                     select o.AdminUserId
                                            ).Count();
                        }
                        else if (UserType == 2)
                        {
                            count = (from o in context.ManagerLocationMappings
                                     join ur in context.UserRegistrations
                                     on o.ManagerUserId equals ur.UserId

                                     where
                                     ur.IsLoginActive == true
                                     && ur.IsEmailVerify == true
                                     && ur.IsDeleted == false
                                     && o.IsDeleted == false
                                     && ur.UserId == UserId

                                     select o.ManagerUserId
                                          ).Count();
                        }
                    }

                    if (count == 0)
                    {
                        obj_UserRegistration.IsDeleted = true;
                        obj_UserRegistration.IsLoginActive = false;
                        obj_UserRegistration.DeletedDate = DateTime.UtcNow;
                        obj_UserRegistration.DeletedBy = DeletedBy;
                        ObjUserRepository.SaveChanges();

                        objDAR.TaskType = (long)TaskTypeCategory.UserDelete;
                        objDAR.ActivityDetails = DarMessage.UserDeleteDar(obj_UserRegistration.FirstName + ' ' + obj_UserRegistration.LastName);

                        #region Save DAR
                        result = objCommonMethodManager.SaveDAR(objDAR);
                        #endregion Save DAR
                        return Result.Delete;
                    }
                    else
                    {
                        return Result.ExistRecord;
                    }
                }
                else
                {
                    return Result.DoesNotExist;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// To Delete the User permanently
        /// </summary>
        /// <CreatedBy>Vijay Sahu</CreatedBy>
        /// <CreatedDate>2015-06-22</CreatedDate>
        public Tuple<int, string> DeleteUserFromUserList(long UserId, long DeletedBy, DARModel objDAR)
        {

            StringBuilder sb = new StringBuilder();
            int count = 0;
            try
            {
                ObjUserRepository = new UserRepository();
                CommonMethodManager objCommonMethodManager = new CommonMethodManager();
                UserRegistration obj_UserRegistration = ObjUserRepository.GetAll(x => x.UserId == UserId).FirstOrDefault();
                if (obj_UserRegistration != null)
                {
                    //UserType = obj_UserRegistration.UserType;
                    workorderEMSEntities Context = new workorderEMSEntities();

                    List<ssp_DeleteUser_Result> resu = Context.ssp_DeleteUser(UserId, DeletedBy).ToList();



                    count = Convert.ToInt32(resu.Select(x => x.Result).FirstOrDefault());
                    if (count == 0) //--SELECT 0,0,0,'There is one or more than one location where he is only one manager'
                    {


                        sb.Append("You can't delete this user because is the only manager for location(s) ");
                        foreach (var locName in resu)
                        {
                            sb.Append("<strong>" + locName.LocationName + "<strong>, ");
                        }

                        try
                        {

                            if (sb.Length > 2 && sb.ToString().Contains(","))
                            {
                                sb.Remove(sb.Length - 2, 2).Append(".");
                            }
                        }
                        catch
                        {

                        }

                    }
                    else if (count == 1)    //--SELECT 1,0,0,'There is no location where he is only one manager'
                    {
                        obj_UserRegistration.IsDeleted = true;
                        obj_UserRegistration.IsLoginActive = false;
                        obj_UserRegistration.DeletedDate = DateTime.UtcNow;
                        obj_UserRegistration.DeletedBy = DeletedBy;
                        ObjUserRepository.SaveChanges();

                        objDAR.TaskType = (long)TaskTypeCategory.UserDelete;
                        objDAR.ActivityDetails = DarMessage.UserDeleteDar(obj_UserRegistration.FirstName + ' ' + obj_UserRegistration.LastName);

                        #region Save DAR
                        objCommonMethodManager.SaveDAR(objDAR);
                        #endregion Save DAR

                        sb.Clear();
                        sb.Append(resu.Select(x => x.LocationName).FirstOrDefault() + ".");
                    }
                    else if (count == 2) //--SELECT 2,0,0,'You are not authorised to delete this user'
                    {
                        sb.Clear();
                        sb.Append(resu.Select(x => x.LocationName).FirstOrDefault() + ".");


                    }

                }
                else
                {
                    sb.Clear();
                    sb.Append("Please provide proper details for execute delete operation.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Tuple<int, string> returnResult = new Tuple<int, string>(count, sb.ToString());
            return returnResult;
        }

        /// <summary>
        /// TO GET EMPLOYEE WORK ASSIGNMENT
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>2015-03-12</CreatedDate>
        /// <param name="LocationId"></param>
        /// <param name="UserID"></param>
        /// <param name="OrderBy"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public List<AllWorkAssignedToEmployeeModel> GetEmployeeAssignedWorkRequest(long LocationId, long UserID, string OrderBy, string columnName)
        {
            var returnData = new List<AllWorkAssignedToEmployeeModel>();
            var getData = _iUserRepository.GetEmployeeAssignedWorkRequest(LocationId, UserID, OrderBy, columnName).ToList();

            foreach (var item in getData)
            {
                var x = new AllWorkAssignedToEmployeeModel();
                x.RN = item.RN;
                x.WorkStatus = item.WorkStatus;
                x.Priority = item.Priority;
                x.WorkRequestAssignmentID = item.WorkRequestAssignmentID;
                x.WorkRequestType = item.WorkRequestType;
                x.AssetID = item.AssetID;
                x.LocationID = item.LocationID;
                x.ProblemDesc = item.ProblemDesc;
                x.PriorityLevel = item.PriorityLevel;
                x.WorkRequestImage = item.WorkRequestImage;
                x.SafetyHazard = item.SafetyHazard;
                x.ProjectDesc = item.ProjectDesc;
                x.WorkRequestStatus = Convert.ToInt64(item.WorkRequestStatus);
                x.RequestBy = item.RequestBy;
                x.AssignToUserId = item.AssignToUserId;
                x.AssignByUserId = item.AssignByUserId;
                x.Remarks = item.Remarks;
                x.CreatedBy = item.CreatedBy;
                x.CreatedDate = item.CreatedDate.ToClientTimeZoneinDateTime();
                x.ModifiedBy = item.ModifiedBy;
                if (item.ModifiedDate != null)
                    x.ModifiedDate = item.ModifiedDate.Value.ToClientTimeZoneinDateTime();
                x.IsDeleted = item.IsDeleted;
                x.DeletedBy = item.DeletedBy;
                if (item.DeletedDate != null)
                    x.DeletedDate = item.DeletedDate.Value.ToClientTimeZoneinDateTime();
                x.WorkRequestProjectType = item.WorkRequestProjectType;
                x.AssignedWorkOrderImage = item.AssignedWorkOrderImage;
                if (item.StartTime != null)
                    x.StartTime = item.StartTime.Value.ToClientTimeZoneinDateTime();
                if (item.EndTime != null)
                    x.EndTime = item.EndTime.Value.ToClientTimeZoneinDateTime();
                x.AssignedTime = item.AssignedTime;
                x.WorkStatusDesc = item.WorkStatusDesc;

                returnData.Add(x);
            }

            return returnData;
            //return _iUserRepository.GetEmployeeAssignedWorkRequest(LocationId, UserID, OrderBy, columnName).Select(x => new AllWorkAssignedToEmployeeModel()
            //{
            //    RN = x.RN,
            //    WorkStatus = x.WorkStatus,
            //    Priority = x.Priority,
            //    WorkRequestAssignmentID = x.WorkRequestAssignmentID,
            //    WorkRequestType = x.WorkRequestType,
            //    AssetID = x.AssetID,
            //    LocationID = x.LocationID,
            //    ProblemDesc = x.ProblemDesc,
            //    PriorityLevel = x.PriorityLevel,
            //    WorkRequestImage = x.WorkRequestImage,
            //    SafetyHazard = x.SafetyHazard,
            //    ProjectDesc = x.ProjectDesc,
            //    WorkRequestStatus = Convert.ToInt64(x.WorkRequestStatus),
            //    RequestBy = x.RequestBy,
            //    AssignToUserId = x.AssignToUserId,
            //    AssignByUserId = x.AssignByUserId,
            //    Remarks = x.Remarks,
            //    CreatedBy = x.CreatedBy,
            //    CreatedDate = x.CreatedDate.ToClientTimeZoneinDateTime(),
            //    ModifiedBy = x.ModifiedBy,
            //    ModifiedDate = x.ModifiedDate.Value.ToClientTimeZoneinDateTime(),
            //    IsDeleted = x.IsDeleted,
            //    DeletedBy = x.DeletedBy,
            //    DeletedDate = x.DeletedDate.Value.ToClientTimeZoneinDateTime(),
            //    WorkRequestProjectType = x.WorkRequestProjectType,
            //    AssignedWorkOrderImage = x.AssignedWorkOrderImage,
            //    StartTime = x.StartTime.Value.ToClientTimeZoneinDateTime(),
            //    EndTime = x.EndTime.Value.ToClientTimeZoneinDateTime(),
            //    AssignedTime = x.AssignedTime,
            //    WorkStatusDesc = x.WorkStatusDesc,

            //}).ToList();
        }

        /// <summary>
        /// TO GET NOT ASSIGNED USERS
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>28-03-2015</CreatedDate>
        /// <param name="requestedBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="textSearch"></param>
        /// <param name="userType"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public List<NotAssignedUserModel> GetNotAssignedUsers(long? requestedBy, int? pageIndex, string sortColumnName, string sortOrderBy, int? numberOfRows, string textSearch, string userType, ObjectParameter totalRecords)
        {
            ObjUserRepository = new UserRepository();
            return ObjUserRepository.GetNotAssignedUsers(requestedBy, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, userType, totalRecords);

        }


        /// <summary>
        /// Added by vijay sahu on 12 june 2015
        /// </summary>
        /// <param name="UserId"></param>
        public string getProfilePicture(long UserId)
        {

            string profilePath = "";
            try
            {


                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    profilePath = (from o in Context.UserRegistrations
                                   where o.UserId == UserId && o.IsDeleted == false
                                   select o.ProfileImage).FirstOrDefault();

                }


                if (string.IsNullOrWhiteSpace(profilePath))
                {
                    profilePath = "no-profile-pic.jpg";
                }
                profilePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture) + ConfigurationManager.AppSettings["ProfilePicPath"].Replace("~", "") + profilePath;
            }
            catch (Exception)
            {
                throw;
            }

            return profilePath;


        }

        /// <summary>
        /// To Verify the User
        /// </summary>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedDate>2016-10-08</CreatedDate>
        /// <param name="UserId"></param>
        /// <param name="DeletedBy"></param>
        public Result UpdateVerifyUser(long UserId, long ModifiedBy, DARModel objDAR)
        {
            Result result;
            try
            {
                ObjUserRepository = new UserRepository();
                objCommonMethodManager = new CommonMethodManager();
                UserRegistration obj_UserRegistration = ObjUserRepository.GetAll(x => x.UserId == UserId).FirstOrDefault();
                if (obj_UserRegistration != null)
                {
                    obj_UserRegistration.IsLoginActive = true;
                    obj_UserRegistration.IsEmailVerify = true;
                    obj_UserRegistration.ModifiedDate = DateTime.UtcNow;
                    obj_UserRegistration.ModifiedBy = ModifiedBy;
                    ObjUserRepository.SaveChanges();

                    objDAR.TaskType = (long)TaskTypeCategory.UserUpdate;
                    objDAR.ActivityDetails = DarMessage.UserVerifiedDar(obj_UserRegistration.FirstName + ' ' + obj_UserRegistration.LastName);

                    #region Save DAR
                    result = objCommonMethodManager.SaveDAR(objDAR);
                    #endregion Save DAR
                    return Result.UpdatedSuccessfully;

                }
                else
                {
                    return Result.DoesNotExist;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
