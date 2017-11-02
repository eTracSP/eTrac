using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.UserModels;


namespace WorkOrderEMS.Data
{
    public class UserRepository : BaseRepository<UserRegistration>, IUserRepository
    {
        workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();


        public UserModel GetUserById(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter paramTotalRecords)
        {
            UserModel objUserModel = new UserModel();
            var data = _workorderEMSEntities.SP_GetUser(userId, operationName, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, paramTotalRecords);
            foreach (var item in data)
            {
                objUserModel.UserId = item.UserId;
                objUserModel.UserType = item.UserType;
                objUserModel.FirstName = item.FirstName;
                objUserModel.LastName = item.LastName;
                objUserModel.UserEmail = item.UserEmail;
                objUserModel.AlternateEmail = item.AlternateEmail;
                objUserModel.DOB = item.DOB.ToString("MM/dd/yy");
                objUserModel.BloodGroup = item.BloodGroup;
                //objUserModel.Gender = item.GenderName != "" ? (item.GenderName == "Male" ? 1 : 2) : 0; // previously written.
                objUserModel.Gender = item.GenderName != "" ? (item.GenderName == "Male" ? 9 : 10) : 9; // added by vijay sahu .
                objUserModel.myProfileImage = item.ProfileImage;
                objUserModel.Password = item.Password;
                objUserModel.Address = new AddressModel();
                objUserModel.Address.AddressMasterId = Convert.ToInt32(item.AddressMasterId);
                objUserModel.Address.Address1 = item.Address1;
                objUserModel.Address.Address2 = item.Address2;
                objUserModel.Address.CountryId = item.CountryId;
                objUserModel.Address.StateId = item.StateId;
                objUserModel.Address.City = item.City;
                objUserModel.Address.ZipCode = item.ZipCode;
                objUserModel.Address.Mobile = item.Mobile;
                objUserModel.Address.Phone = item.Phone;
                objUserModel.EmployeeID = item.EmployeeID;
                objUserModel.JobTitle = item.JobTitle;
                objUserModel.JobTitleOther = item.JobTitleOther;
            }
            return objUserModel;
        }

        public List<UserModelList> GetAllVerfiedUser(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter paramTotalRecords)
        {
            List<UserModelList> lstVerifiedMnagaer = new List<UserModelList>();
            try
            {
                int ss = _workorderEMSEntities.SP_GetAllVerifiedUser(userId, operationName, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, paramTotalRecords);
                //lstVerifiedMnagaer = _workorderEMSEntities.SP_GetAllVerifiedUser(userId, operationName, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, paramTotalRecords).Select(t =>
                //    new UserModelList()
                //    {
                //        UserId = t.UserId,
                //        ProjectID = t.ProjectID,
                //        ProjectName = t.ProjectName,
                //        UserEmail = t.UserEmail,
                //        DOB = t.DOB,
                //        Name = t.Name,
                //        HiringDate = t.HiringDate,
                //        EmployeeCategoryid = t.EmployeeCategory,
                //        EmployeeProfile = t.EmployeeProfile,
                //        EmployeeID = t.EmployeeID,
                //        UserType = t.UserType
                //    }).ToList();
                return lstVerifiedMnagaer;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>GetAllVerfiedUsers
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedFor>Get All IT Administrator List</CreatedFor>
        /// <CreatedOn>Nov-14-2014</CreatedOn>
        /// <param name="UserID"></param>
        /// <param name="OperationName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<UserModelList> GetAllVerfiedUsers(long? userId, long locationId, string useType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, out long totalRecords)
        {
            //totalRecords = 0;
            ObjectParameter totalRecord = new ObjectParameter("TotalRecords", typeof(int));

            List<UserModelList> lstVerifiedMnagaer = new List<UserModelList>();
            try
            {
                lstVerifiedMnagaer = _workorderEMSEntities.SP_GetAllActiveUser(userId, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, locationId, useType, totalRecord).Select(t =>
                    new UserModelList()
                    {
                        UserId = t.UserId,
                        //ProjectID = t.ProjectID,
                        //ProjectName = t.ProjectName,
                        UserEmail = t.UserEmail,
                        DOB = t.DOB,
                        Name = t.Name,
                        HiringDate = t.HiringDate,
                        EmployeeProfile = t.EmployeeProfile,
                        UserType = t.UserType,
                        CodeName = t.CodeName,
                        ProfileImage = t.ProfileImage,
                        QRCID = t.QRCID,
                    }).ToList();
                totalRecords = Convert.ToInt32(totalRecord.Value);
                return lstVerifiedMnagaer;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>GetAllVerfiedUsers
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedFor>Get All IT Administrator List</CreatedFor>
        /// <CreatedOn>Nov-14-2014</CreatedOn>
        /// <param name="UserID"></param>
        /// <param name="OperationName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<UserModelList> GetAllVerfiedUsersForReport(long? userId, long locationId, string useType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, out long totalRecords)
        {
            //totalRecords = 0;
            ObjectParameter totalRecord = new ObjectParameter("TotalRecords", typeof(int));

            List<UserModelList> lstVerifiedMnagaer = new List<UserModelList>();
            try
            {
                lstVerifiedMnagaer = _workorderEMSEntities.SP_GetAllActiveUserForReport(userId, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, locationId, useType, totalRecord).Select(t =>
                    new UserModelList()
                    {
                        UserId = t.UserId,
                        //ProjectID = t.ProjectID,
                        //ProjectName = t.ProjectName,
                        UserEmail = t.UserEmail,
                        DOB = t.DOB,
                        Name = t.Name + " (" + t.EmployeeID + ")",
                        HiringDate = t.HiringDate,
                        EmployeeProfile = t.EmployeeProfile,
                        UserType = t.UserType,
                        CodeName = t.CodeName,
                        ProfileImage = t.ProfileImage,
                        QRCID = t.QRCID,

                    }).ToList();
                totalRecords = Convert.ToInt32(totalRecord.Value);
                return lstVerifiedMnagaer;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>GetAllVerfiedUsersinDAR
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedFor>Get all user list for DAR</CreatedFor>
        /// <CreatedOn>June-26-2017</CreatedOn>
        /// <param name="UserID"></param>
        /// <param name="OperationName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<UserModelList> GetAllVerfiedUsersDAROnly(long? userId, long locationId, string useType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, out long totalRecords)
        {
            //totalRecords = 0;
            ObjectParameter totalRecord = new ObjectParameter("TotalRecords", typeof(int));

            List<UserModelList> lstVerifiedMnagaer = new List<UserModelList>();
            try
            {
                lstVerifiedMnagaer = _workorderEMSEntities.SP_GetAllActiveUserForDAR(userId, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, locationId, useType, totalRecord).Select(t =>
                    new UserModelList()
                    {
                        UserId = t.UserId,
                        //ProjectID = t.ProjectID,
                        //ProjectName = t.ProjectName,
                        UserEmail = t.UserEmail,
                        DOB = t.DOB,
                        Name = t.Name,
                        HiringDate = t.HiringDate,
                        EmployeeProfile = t.EmployeeProfile,
                        UserType = t.UserType,
                        CodeName = t.CodeName,
                        ProfileImage = t.ProfileImage,
                        QRCID = t.QRCID,
                    }).ToList();
                totalRecords = Convert.ToInt32(totalRecord.Value);
                return lstVerifiedMnagaer;
            }
            catch (Exception)
            { throw; }
        }


        //public List<UserModelList> GetLocationListAdministrator(string LocationId, UserType UserType = UserType.Administrator)
        //{
        //    List<UserModelList> lstVerifiedMnagaer = new List<UserModelList>();
        //    try
        //    {
        //        lstVerifiedMnagaer = _workorderEMSEntities.(UserID, UseType, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, paramTotalRecords).Select(t =>
        //        return lstVerifiedMnagaer;
        //    }
        //    catch (Exception ex)
        //    {throw ex;}
        //        }
        //}

        /// <summary>ListLocationAdministrator
        /// <CreatedFor>ListLocationAdministrator</CreatedFor>
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Dec-10-2014</CreatedOn>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        public List<UserModelList> ListLocationAdministrator(long locationId, string UserType = "")
        {
            try
            {
                _workorderEMSEntities = new workorderEMSEntities();
                return _workorderEMSEntities.fnListLocationAdministrator(locationId, Convert.ToInt32(UserType)).Select(t =>
                                    new UserModelList()
                                    {
                                        UserId = t.UserId,
                                        UserEmail = t.UserEmail,
                                        Name = t.Name,
                                        DOB = t.DOB,
                                        JobTitle = t.JobTitle,
                                        ProfileImage = t.ProfileImage,
                                        //UserType = t.UserType
                                    }).ToList();
            }
            catch (Exception)
            { throw; }

        }

        /// <summary>
        /// Created by vijay sahu on 10 march 2015 where We fatching location based on his userTYpe means 
        /// all manager of given locationid, or we can say all admin user of given location.
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        public List<AdminUserForDrop> UnAssignedAdministrationIdRepo(long LocationId, string UserType)
        {

            List<AdminUserForDrop> list = new List<AdminUserForDrop>();
            try
            {
                using (workorderEMSEntities objContext = new workorderEMSEntities())
                {
                    long userTy = Convert.ToInt32(UserType);



                    if (userTy == 6) // 6 means admin user
                    {

                        //list = (from UR in objContext.UserRegistrations
                        //        join ADL in objContext.AdminLocationMappings
                        //            on UR.UserId equals ADL.AdminUserId
                        //        where UR.UserType == userTy
                        //        && ADL.LocationId != LocationId
                        //        && UR.IsLoginActive == true
                        //        && UR.IsEmailVerify == true
                        //        && UR.IsDeleted == false
                        //        && ADL.IsDeleted == false
                        //        select new AdminUserForDrop()
                        //        {
                        //            UserId = UR.UserId,
                        //            Name = UR.FirstName + " " + UR.LastName,
                        //            UserEmail = UR.UserEmail
                        //        }).ToList();


                        list = (from UR in objContext.UserRegistrations

                                where UR.UserType == userTy
                                && UR.UserId != ((from ad in objContext.AdminLocationMappings where ad.LocationId == LocationId && ad.IsDeleted == false select ad.AdminUserId).FirstOrDefault())
                                    //&& ADL.LocationId != LocationId
                                && UR.IsLoginActive == true
                                && UR.IsEmailVerify == true
                                && UR.IsDeleted == false
                                //&& ADL.IsDeleted == false
                                select new AdminUserForDrop()
                                {
                                    UserId = UR.UserId,
                                    Name = UR.FirstName + " " + UR.LastName,
                                    UserEmail = UR.UserEmail
                                }).ToList();


                    }
                    else if (userTy == 2)// means manager user.
                    {
                        //list = (from UR in objContext.UserRegistrations
                        //        join ADL in objContext.ManagerLocationMappings
                        //            on UR.UserId equals ADL.ManagerUserId
                        //        where UR.UserType == userTy
                        //        && ADL.LocationId != LocationId
                        //        && UR.IsLoginActive == true
                        //        && UR.IsEmailVerify == true
                        //        && UR.IsDeleted == false
                        //        && ADL.IsDeleted == false
                        //        select new AdminUserForDrop()
                        //        {
                        //            UserId = UR.UserId,
                        //            Name = UR.FirstName + " " + UR.LastName,
                        //            UserEmail = UR.UserEmail
                        //        }).Distinct().ToList();


                        list = (from UR in objContext.UserRegistrations

                                where UR.UserType == userTy
                                && UR.UserId != ((from m in objContext.ManagerLocationMappings where m.LocationId == LocationId && m.IsDeleted == false select m.ManagerUserId).FirstOrDefault())
                                    //&& ADL.LocationId != LocationId
                                && UR.IsLoginActive == true
                                    && UR.IsEmailVerify == true
                                && UR.IsDeleted == false

                                select new AdminUserForDrop()
                                {
                                    UserId = UR.UserId,
                                    Name = UR.FirstName + " " + UR.LastName,
                                    UserEmail = UR.UserEmail
                                }).ToList();
                    }
                    else if (userTy == 3)// means Employee user.
                    {

                        list = (from UR in objContext.UserRegistrations

                                where UR.UserType == userTy
                                && UR.UserId != ((from m in objContext.EmployeeLocationMappings where m.LocationId == LocationId && m.IsDeleted == false select m.EmployeeUserId).FirstOrDefault())
                                    //&& ADL.LocationId != LocationId
                                && UR.IsLoginActive == true
                                && UR.IsEmailVerify == true
                                && UR.IsDeleted == false
                                //&& ADL.IsDeleted == false
                                select new AdminUserForDrop()
                                {
                                    UserId = UR.UserId,
                                    Name = UR.FirstName + " " + UR.LastName,
                                    UserEmail = UR.UserEmail
                                }).ToList();
                    }

                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return list;
        }

        public List<UserModelList> GetManagerByAdminId(long AdminId)
        {
            List<UserModelList> lstManager = new List<UserModelList>();
            try
            {
                lstManager = (from t in _workorderEMSEntities.UserRegistrations
                              join ae in _workorderEMSEntities.AdminEmployeeMappings on t.UserId equals ae.EmployeeUserId
                              where ae.AdminUserId == AdminId
                              select t).ToList().Select(c => new UserModelList()
                              {
                                  UserId = c.UserId,
                                  Name = c.FirstName + " " + c.LastName
                              }).ToList();
                return lstManager;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public eTracLoginModel GetLocationDetailsByUserID(long userId)
        {

            try
            {
                eTracLoginModel locDetails = (from ur in _workorderEMSEntities.UserRegistrations
                                              join elm in _workorderEMSEntities.EmployeeLocationMappings on ur.UserId equals elm.EmployeeUserId
                                              join lm in _workorderEMSEntities.LocationMasters on elm.LocationId equals lm.LocationId
                                              where ur.UserId == userId && ur.IsDeleted == false
                                              select new eTracLoginModel { LocationID = lm.LocationId, Location = lm.LocationName }).FirstOrDefault();

                return locDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public UserRegistration AuthenticateLogin(eTracLoginModel eTracLogin)
        //{
        //    _workorderEMSEntities = new workorderEMSEntities();
        //    GetSingleOrDefault(x => x.UserEmail == loginViewModel.Email && x.Password == mypassword && x.IsDeleted == false && x.IsEmailVerify == true && x.IsLoginActive == true);
        //    //UserModel User = (from mlm in _workorderEMSEntities.userLocationMappings
        //    //                  join ur in objworkorderEMSEntities.UserRegistrations on mlm.ManagerUserId equals ur.UserId
        //    //                  where mlm.LocationId == locationId
        //    //                  select ur).Select(u => new UserModel()
        //    //                               {
        //    //                                   UserId = u.UserId,
        //    //                                   FirstName = !string.IsNullOrEmpty(u.LastName) ? u.FirstName + ' ' + u.LastName : u.FirstName,
        //    //                                   UserEmail = u.UserEmail,
        //    //                                   UserType = u.UserType
        //    //                               }).ToList();
        //}
        /// <summary>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedDate>12-Feb-2015</CreatedDate>
        /// <Description>From android to save new client registration </Description>
        /// </summary>
        /// <param name="ObjUserModel"></param>
        /// <returns></returns>
        public bool SaveNewClientRegistration(UserModel ObjUserModel)
        {
            bool flag = false;
            UserRegistration ObjUserRegistration = new UserRegistration();
            try
            {
                ObjUserRegistration.UserEmail = ObjUserModel.UserEmail;
                ObjUserRegistration.AlternateEmail = ObjUserModel.AlternateEmail;
                ObjUserRegistration.SubscriptionEmail = ObjUserModel.UserEmail;
                ObjUserRegistration.CreatedBy = ObjUserModel.UserId;
                ObjUserRegistration.CreatedDate = DateTime.UtcNow;
                ObjUserRegistration.DeletedBy = null;
                ObjUserRegistration.DeletedDate = null;
                ObjUserRegistration.IsDeleted = false;
                ObjUserRegistration.UserType = ObjUserModel.UserType;
                ObjUserRegistration.ModifiedBy = null;
                ObjUserRegistration.ModifiedDate = null;
                //ObjUserRegistration.LocationClientMappings = ObjUserModel.Location;
                ObjUserRegistration.FirstName = ObjUserModel.FirstName;
                ObjUserRegistration.LastName = ObjUserModel.LastName;
                ObjUserRegistration.Gender = ObjUserModel.Gender;
                ObjUserRegistration.DOB = Convert.ToDateTime(ObjUserModel.DOB);

                Add(ObjUserRegistration);
                flag = true;
                return flag;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>GetAllClientsDetail
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>12-Feb-2015</CreatedOn>
        /// <CreatedFor>To get the Client details for android</CreatedFor>
        /// </summary>
        /// <returns>lstClients</returns>
        public List<ServiceUserModel> GetAllClientsDetail()
        {
            try
            {
                List<ServiceUserModel> lstClients = (from lcm in _workorderEMSEntities.LocationClientMappings
                                                     join ur in _workorderEMSEntities.UserRegistrations on lcm.ClientUserId equals ur.UserId
                                                     join lm in _workorderEMSEntities.LocationMasters on lcm.LocationId equals lm.LocationId
                                                     //where lcm.LocationId == locationId
                                                     where ur.IsDeleted == false
                                                     select new ServiceUserModel()
                                                     {
                                                         UserId = ur.UserId,
                                                         FirstName = ur.FirstName,
                                                         LastName = ur.LastName,
                                                         UserEmail = ur.UserEmail,
                                                         UserType = ur.UserType,
                                                         Gender = ur.Gender,
                                                         ProfileImageFile = ur.ProfileImage,
                                                         EmployeeID = ur.EmployeeID,
                                                         JobTitle = ur.JobTitle,
                                                         LocationId = lm.LocationId,
                                                         LocationName = lm.LocationName,
                                                         Address1 = lm.Address1,
                                                         Address2 = lm.Address2,
                                                         City = lm.City,
                                                         CountryId = lm.CountryId,
                                                         PhoneNo = lm.PhoneNo,
                                                         Mobile = lm.Mobile,
                                                         ZipCode = lm.ZipCode
                                                     }).ToList();
                return lstClients;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// TO GET EMPLOYEE ASSIGNED WORKREQUEST
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>2015-03-12</CreatedDate>
        /// <param name="LocationId"></param>
        /// <param name="UserID"></param>
        /// <param name="OrderBy"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public List<Proc_GetAllWorkAssignedToEmployee_Result> GetEmployeeAssignedWorkRequest(long LocationId, long UserID, string OrderBy, string columnName)
        {
            return _workorderEMSEntities.Proc_GetAllWorkAssignedToEmployee(LocationId, UserID, OrderBy, columnName).ToList();
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
            try
            {
                return _workorderEMSEntities.SP_GetAllNotAssignedUsers(requestedBy, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, userType, totalRecords).Select(x => new NotAssignedUserModel()
                {
                    RN = x.RN,
                    CodeName = x.CodeName,
                    GlobalCodeId = x.GlobalCodeId,
                    UserId = x.UserId,
                    UserEmail = x.UserEmail,
                    Name = x.Name,
                    Gender = x.Gender,
                    DOB = x.DOB,
                    ProfileImage = x.ProfileImage,
                    IsLoginActive = x.IsLoginActive,

                }).ToList();

            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created Date:05-Oct-2016
        /// This method call the sp SP_GetAllUnVerifiedUsers to get unverified user list.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="locationId"></param>
        /// <param name="useType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public List<UserModelList> GetUnVerifiedUsers(long? userId, long locationId, string useType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, out long totalRecords)
        {
            //totalRecords = 0;
            ObjectParameter totalRecord = new ObjectParameter("TotalRecords", typeof(int));

            List<UserModelList> lstVerifiedMnagaer = new List<UserModelList>();
            try
            {
                lstVerifiedMnagaer = _workorderEMSEntities.SP_GetAllUnVerifiedUsers(userId, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, locationId, useType, totalRecord).Select(t =>
                    new UserModelList()
                    {
                        UserId = t.UserId,
                        //ProjectID = t.ProjectID,
                        //ProjectName = t.ProjectName,
                        UserEmail = t.UserEmail,
                        DOB = t.DOB,
                        Name = t.Name,
                        HiringDate = t.HiringDate,
                        EmployeeProfile = t.EmployeeProfile,
                        UserType = t.UserType,
                        CodeName = t.CodeName,
                        ProfileImage = t.ProfileImage,
                        QRCID = t.QRCID,

                    }).ToList();
                totalRecords = Convert.ToInt32(totalRecord.Value);
                return lstVerifiedMnagaer;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created Date:05-Oct-2016
        /// This method fetch user to edit unverified user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="operationName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <param name="paramTotalRecords"></param>
        /// <returns></returns>
        public UserModel GetUserByIdForUnverifiedUser(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter paramTotalRecords)
        {
            UserModel objUserModel = new UserModel();
            var data = _workorderEMSEntities.SP_GetUnverifiedUser(userId, operationName, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, paramTotalRecords);
            foreach (var item in data)
            {
                objUserModel.UserId = item.UserId;
                objUserModel.UserType = item.UserType;
                objUserModel.FirstName = item.FirstName;
                objUserModel.LastName = item.LastName;
                objUserModel.UserEmail = item.UserEmail;
                objUserModel.AlternateEmail = item.AlternateEmail;
                objUserModel.DOB = item.DOB.ToString("MM/dd/yy");
                objUserModel.BloodGroup = item.BloodGroup;
                objUserModel.Gender = item.GenderName != "" ? (item.GenderName == "Male" ? 9 : 10) : 9; // added by vijay sahu .
                objUserModel.myProfileImage = item.ProfileImage;
                objUserModel.Password = item.Password;
                objUserModel.Address = new AddressModel();
                objUserModel.Address.AddressMasterId = Convert.ToInt32(item.AddressMasterId);
                objUserModel.Address.Address1 = item.Address1;
                objUserModel.Address.Address2 = item.Address2;
                objUserModel.Address.CountryId = item.CountryId;
                objUserModel.Address.StateId = item.StateId;
                objUserModel.Address.City = item.City;
                objUserModel.Address.ZipCode = item.ZipCode;
                objUserModel.Address.Mobile = item.Mobile;
                objUserModel.Address.Phone = item.Phone;
                objUserModel.EmployeeID = item.EmployeeID;
                objUserModel.JobTitle = item.JobTitle;
                objUserModel.JobTitleOther = item.JobTitleOther;
            }
            return objUserModel;
        }
    }
}
