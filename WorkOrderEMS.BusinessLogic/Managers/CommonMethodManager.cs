using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class CommonMethodManager : ICommonMethod
    {
        private string PWDGUIDMaxLength = ConfigurationManager.AppSettings["PWDGUIDMaxLength"];
        int pwdmaxlendth = 10;
        private string ProfileImagePath = ConfigurationManager.AppSettings["ProfilePicPath"];
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        CommonRepository objCommonRepository = new CommonRepository();

        GlobalAdminManager ObjGlobalAdminManager = new GlobalAdminManager();
        ServiceMasterRepository objServiceMasterRepository = new ServiceMasterRepository();
        QRCMasterRepository ObjQRCMasterRepository;
        //ProjectRepository objProjectRepository = new ProjectRepository();
        WorkAreaMasterRepository objWorkAreaMasterRepository = new WorkAreaMasterRepository();
        InventoryMasterRepository objInventoryMasterRepository = new InventoryMasterRepository();
        UserRepository ObjUserRepository;
        GlobalCodesRepository ObjGlobalCodesRepository;
        //AssetMasterRepository objAssetMasterRepository = new AssetMasterRepository();
        InsuranceRepository objInsuranceRepository;
        LocationClientMappingRepository ObjLocationClientMappingRepository;
        //LocationRepository objLocationRepository = new LocationRepository();
        AdminLocationMappingRepository objAdminLocationMappingRepository = new AdminLocationMappingRepository();
        ManagerLocationMappingRepository objManagerLocationMappingRepository = new ManagerLocationMappingRepository();
        LocationClientMappingRepository objLocationClientMappingRepository = new LocationClientMappingRepository();
        EmployeeLocationMappingRepository objEmployeeLocationMapping = new EmployeeLocationMappingRepository();
        DARDetail ObjDARDetail;
        DARRepository objDARRepository = new DARRepository();
        PermissionDetailsRepository objPermissionDetailsRepository;
        DashboardWidgetSettingRepository objDashboardWidgetSettingRepository;
        EmailLog objemail;
        EmailLogRepository objEmailLogRepository = new EmailLogRepository();


        /// <summary>UploadImage
        /// <Createdby>Nagendra Upwanshi</Createdby>
        /// <CreatedDate>Aug-22-2014</CreatedDate>
        /// </summary>        
        /// <param name="myFile"></param>
        /// <param name="path"></param>
        /// <param name="ImageName"></param>
        public void UploadImage(HttpPostedFileBase myFile, string path, string imageName)
        {

        }

        /// <summary>IsEmailExist
        /// <Createdby>Nagendra Upwanshi</Createdby>
        /// <CreatedDate>Aug-22-2014</CreatedDate>
        /// <modifedby>Nagendra Upwanshi</modifedby>
        /// <modifiedOn>Oct-08-2014</modifiedOn>
        /// </summary>
        /// <param name="EmailId"></param>
        /// <returns></returns>
        public bool IsEmailExist(string emailId)
        {
            try
            {
                if (Helper.EmailHelper.IsValidEmail(emailId))
                {
                    ObjUserRepository = new UserRepository();
                    long userid = ObjUserRepository.GetSingleOrDefault(u => u.UserEmail == emailId && u.IsDeleted == false).UserId;
                    if (userid > 0) { return true; }
                }
                else { throw new Exception(Helper.CommonMessage.EmailInvalid()); }
            }
            catch (Exception)
            { throw; }
            return false;
        }

        /// <summary>IsEmployeeIdExist
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Oct-08-2014</CreatedOn>        
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public bool IsEmployeeIdExist(string employeeId)
        {
            try
            {
                ObjUserRepository = new UserRepository();
                long userid = ObjUserRepository.GetSingleOrDefault(u => u.EmployeeID == employeeId && u.IsDeleted == false).UserId;
                if (userid > 0) { return true; }
            }
            catch (Exception)
            { throw; }
            return false;
        }

        /// <summary>IsUserExists
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Oct-08-2014</CreatedOn>        
        /// </summary>
        /// <param name="FirstName"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        public bool IsUserExists(string firstName, long userType)
        {
            try
            {
                ObjUserRepository = new UserRepository();
                long userid = ObjUserRepository.GetSingleOrDefault(u => u.FirstName == firstName && u.UserType == userType && u.IsDeleted == false).UserId;
                if (userid > 0) { return true; }
            }
            catch (Exception)
            { throw; }
            return false;
        }


        /// <summary>get all country
        /// <Createdby>Gayatri Pal</Createdby>
        /// <CreatedDate>Aug-25-2014</CreatedDate>
        /// </summary>
        /// <param name="EmailId"></param>
        /// <returns></returns>
        public List<CountryModel> GetAllcountries()
        {
            return objCommonRepository.GetAllcountries();
        }

        /// <summary>get all state By Countryid
        /// <Createdby>Gayatri Pal</Createdby>
        /// <CreatedDate>11-Sep-2014</CreatedDate>
        /// </summary>
        /// <param name="EmailId"></param>
        /// <returns></returns>

        /// <summary>get all state By Countryid
        /// <Createdby>Gayatri Pal</Createdby>
        /// <CreatedDate>Aug-25-2014</CreatedDate>
        /// </summary>
        /// <param name="EmailId"></param>
        /// <returns></returns>
        public List<StateModel> GetStateByCountryId(long countryId)
        {
            return objCommonRepository.GetStateByCountryID(countryId);
        }

        /// <summary>call the method at data layer
        /// to get data from the globalcode table
        /// <Createdby>Gayatri Pal</Createdby>
        /// <CreatedDate>Aug-25-2014</CreatedDate>
        /// </summary>
        /// <param name="EmailId"></param>
        /// <returns></returns>
        public List<GlobalCodeModel> GetGlobalCodeData(string category)
        {
            return objCommonRepository.GetGlobalCodeData(category);
        }

        /// <summary>GetGlobalCodeDetailById
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>June-03-2015</CreatedOn>
        /// </summary>
        /// <param name="globalCodeId"></param>
        /// <returns></returns>
        public string GetGlobalCodeDetailById(long globalCodeId)
        {
            try
            {
                ObjGlobalCodesRepository = new GlobalCodesRepository();
                return ObjGlobalCodesRepository.GetSingleOrDefault(g => g.GlobalCodeId == globalCodeId && g.IsDeleted == false && g.IsActive == true).CodeName;
            }
            catch (Exception)
            { throw; }
        }
        /// <summary>GetGlobalCodeForName
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Oct-08-2014</CreatedOn>
        /// </summary>
        /// <param name="Category"></param>
        /// <param name="CodeName"></param>
        /// <returns></returns>
        public long GetGlobalCodeForName(string category, string codeName)
        {
            try
            {
                ObjGlobalCodesRepository = new GlobalCodesRepository();
                return ObjGlobalCodesRepository.GetSingleOrDefault(g => g.Category == category && g.CodeName == codeName && g.IsDeleted == false && g.IsActive == true).GlobalCodeId;
            }
            catch (Exception)
            { throw; }
        }

        public List<LocationMasterModel> GetAllLocation()
        {
            return ObjGlobalAdminManager.GetAllLocation();
        }
        /// <summary>Get All Project Services       
        /// <Createdby>Gayatri Pal</Createdby>
        /// <CreatedDate>Aug-28-2014</CreatedDate>
        /// </summary>
        /// <param name="EmailId"></param>
        /// <returns></returns>
        public List<ServiceMasterModel> GetAllServices()
        {
            List<ServiceMasterModel> lstServices = new List<ServiceMasterModel>();
            try
            {
                lstServices = objServiceMasterRepository.GetAll(s => s.IsDeleted == false).Select(l => new ServiceMasterModel()
                {
                    ServiceID = l.ServiceID,
                    ServiceName = l.ServiceName,
                    Description = l.Description
                    //Option Grop
                }).ToList();
                return lstServices;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>GenerateQRCode
        /// CreatedBy:  Nagendra Upwanshi
        /// CreatedOn:  Aug-28-2014
        /// CreatedFor: make QR Code entry return new QRCodeId
        /// </summary>
        /// <param name="QRCName"></param>
        /// <param name="MODULEId"></param>
        /// <param name="PROJECTCATEGORYId"></param>
        /// <param name="ProjectId"></param>
        /// <param name="QRCDefaultSizeID"></param>
        /// <param name="QRCTYPEID"></param>
        /// <param name="SpecialNotes"></param>
        /// <param name="CreatedBy"></param>
        /// <param name="QRCID"></param>
        /// <returns></returns>
        public bool GenerateQRCode(string qrcName, long moduleId, long? projectCategoryId, long? projectId, long qrcDefaultSizeId, long qrcTypeId, string specialNotes, long? createdBy, out long qrcId)
        {
            bool resultstatus = false;
            try
            {
                ObjQRCMasterRepository = new QRCMasterRepository();
                QRCMaster ObjQRCMaster = new QRCMaster
                {
                    QRCName = qrcName,
                    CreatedBy = (createdBy.HasValue) ? createdBy.Value : 0,
                    CreatedDate = DateTime.UtcNow,
                    ModuleNameId = moduleId,
                    //ProjectCategoryId = (PROJECTCATEGORYId.HasValue) ? (PROJECTCATEGORYId.Value) : 0,
                    ProjectCategoryId = projectCategoryId,
                    ProjectId = projectId,
                    QRCDefaultSize = qrcDefaultSizeId,
                    QRCTYPE = qrcTypeId,
                    SpecialNotes = specialNotes,
                };
                ObjQRCMasterRepository.Add(ObjQRCMaster);
                qrcId = ObjQRCMaster.QRCID;
                resultstatus = true;
            }
            catch (Exception) { throw; }
            return resultstatus;
        }


        ////created by Gayatri
        ////Created on 07-Oct-2014
        ////To get the Asset
        //public string GetAssetImage(long ProjectId, long AssetID)
        //{
        //    string AssetImage = string.Empty;
        //    try
        //    {
        //        AssetImage = objAssetMasterRepository.GetSingleOrDefault(a => a.ProjectID == ProjectId && a.AssetMasterID == AssetID).AssetPhoto;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return AssetImage;
        //}

        #region DropDown Methods
        public List<SelectListItem> GetAllCountryList()
        {
            List<SelectListItem> listCountry = objCommonRepository.GetAllcountries()
                                          .Select(i => new SelectListItem()
                                          {
                                              Text = i.CountryName,
                                              Value = i.CountryID.ToString(CultureInfo.InvariantCulture)
                                          }).ToList();
            return listCountry;

        }
        public List<SelectListItem> GetGlobalCodeDataList(string category)
        {
            try
            {
                List<SelectListItem> listGlobalCodedata = objCommonRepository.GetGlobalCodeData(category)
               .Select(g => new SelectListItem()
               {
                   Text = g.CodeName,
                   Value = g.GlobalCodeId.ToString(CultureInfo.InvariantCulture)
               }).ToList();

                return listGlobalCodedata;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public List<SelectListItem> GetWorkArea()
        {
            try
            {
                List<SelectListItem> lstWorkArea = objWorkAreaMasterRepository.GetAll(w => w.IsDeleted == false).Select(t =>
                    new SelectListItem()
                    {
                        Text = t.AreaName,
                        Value = Convert.ToString(t.WorkAreaMasterID, CultureInfo.InvariantCulture)
                    }).ToList();

                return lstWorkArea;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<SelectListItem> GetEmployeeProject(long projectId)
        {
            try
            {
                List<SelectListItem> lstemp = objCommonRepository.GetEmployeeProject(projectId).Select(u => new SelectListItem()
                {
                    Text = u.FirstName,
                    Value = Convert.ToString(u.UserId, CultureInfo.InvariantCulture)
                }).ToList();

                return lstemp;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public List<SelectListItem> GetNotAssgnProject(string UserType, long ProjectId)
        //{

        //    try
        //    {
        //        List<SelectListItem> lstproject = objCommonRepository.GetNotAssgnProject(UserType, ProjectId).Select(p => new SelectListItem()
        //        {
        //            Value = Convert.ToString(p.ProjectID),
        //            Text = p.Location
        //        }).ToList();
        //        return lstproject;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public List<SelectListItem> GetAllInventoryByProjectId(long projectId)
        {
            try
            {
                List<SelectListItem> lstInventory = objInventoryMasterRepository.GetAll(i => i.IsDeleted == false && i.LocationId == projectId).Select(t => new SelectListItem()
                {
                    Value = Convert.ToString(t.InventoryID, CultureInfo.InvariantCulture),
                    Text = Convert.ToString(t.ItemName, CultureInfo.InvariantCulture) + " (" + Convert.ToString(t.ItemCode, CultureInfo.InvariantCulture) + ")"
                }).ToList();

                return lstInventory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        ////Created by Gayatri
        ////created on 07-oct-2014
        ////To get All Asset by Work area
        ////This Method is use to bind the drop on work request page
        //public List<SelectListItem> GetAllAssetByWorkArea(long ProjectId, long WorkAreaId)
        //{
        //    try
        //    {
        //        List<SelectListItem> lstAsset = objAssetMasterRepository.GetAll(a => a.WorkAreaID == WorkAreaId && a.ProjectID == ProjectId).Select(
        //                                                t => new SelectListItem()
        //                                                {
        //                                                    Text = t.AssetID,
        //                                                    Value = Convert.ToString(t.AssetMasterID)
        //                                                }).ToList();
        //        return lstAsset;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}



        #endregion

        /// <summary>GetPlanByInsuranceCompanyId
        /// CreatedBy   :   Nagendra Upwanshi
        /// CreatedOn   :   Oct-14-2014
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public List<InsurancePlanModel> GetPlanByInsuranceCompanyId(long insuranceCompanyId)
        {
            objInsuranceRepository = new InsuranceRepository();
            return objInsuranceRepository.GetStateByInsuranceCompanyId(insuranceCompanyId);
        }

        /// <summary>
        /// This function is used to verify the email.
        /// added by vijay sahu on 3 june 2015
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        public bool ActivateNewUser(string usr)
        {
            try
            {
                long userid = 0;
                if (!string.IsNullOrEmpty(usr))
                {
                    usr = Cryptography.GetDecryptedData(usr, true);
                    long.TryParse(usr, out userid);
                }


                if (userid > 0)
                {

                    try
                    {
                        using (workorderEMSEntities Context = new workorderEMSEntities())
                        {

                            var result = (from o in Context.UserRegistrations
                                          where o.UserId == userid && o.IsDeleted == false
                                          select o).FirstOrDefault();

                            if (result != null)
                            {
                                result.IsEmailVerify = true;
                                result.IsLoginActive = true;

                                Context.SaveChanges();
                            }
                        }

                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception) { throw; }
        }

        public QRCModel LoadInvitedUser(string usr)
        {
            try
            {
                long userid = 0;
                if (!string.IsNullOrEmpty(usr))
                {
                    usr = Cryptography.GetDecryptedData(usr, true);
                    long.TryParse(usr, out userid);
                }
                QRCModel _UserModel = new QRCModel();

                if (userid > 0)
                {
                    long Totalrecord = 0;
                    _UserModel.UserModel = GetManagerByIdCode(userid, "GetUserByID", null, null, null, null, null, out Totalrecord);
                    _UserModel.UserModel.Password = "";
                    _UserModel.UserModel.myProfileImage =
                    (_UserModel.UserModel.myProfileImage == "" || _UserModel.UserModel.myProfileImage == null) ? "no-profile-pic.jpg" : HostingPrefix + ProfileImagePath.Replace("~", "") + _UserModel.UserModel.myProfileImage;
                }
                //ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));

                //_UserModel.JobTitleList = GetGlobalCodeData("JOBTITLE");
                //_UserModel.LocationList = ObjGlobalAdminManager.GetAllLocationList(0, "GetAllLocation", 1, 10000, "LocationName", "desc", "", paramTotalRecords);


                return _UserModel;
            }
            catch (Exception) { throw; }
        }

        public UserModel GetManagerByIdCode(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, out long totalRecords)
        {
            totalRecords = 0;
            ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
            ObjUserRepository = new UserRepository();
            try
            {
                var result = ObjUserRepository.GetUserById(userId, operationName, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, paramTotalRecords);
                result.Password = "";
                if (paramTotalRecords != null && paramTotalRecords.Value != null && paramTotalRecords.Value != DBNull.Value)
                { long.TryParse(paramTotalRecords.Value.ToString(), out totalRecords); }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Added by vijay sahu 
        /// You can use this function for fetching details of any user type. 
        /// </summary>

        /// <returns></returns>
        public UserModel GetAdminByIdCode(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, out long totalRecords)
        {
            totalRecords = 0;
            ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
            ObjUserRepository = new UserRepository();
            try
            {
                var result = ObjUserRepository.GetUserById(userId, operationName, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, paramTotalRecords);
                if (paramTotalRecords != null && paramTotalRecords.Value != null && paramTotalRecords.Value != DBNull.Value)
                { long.TryParse(paramTotalRecords.Value.ToString(), out totalRecords); }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>CreateRandomPassword()
        /// <CreatedBY>Nagendra Upwanshi</CreatedBY>
        /// <CreatedFor>Create Random Password</CreatedFor>
        /// <CreteadOn>Nov-18-2014</CreteadOn>
        /// </summary>
        /// <returns></returns>
        public string CreateRandomPassword()
        {
            string mypassword = "";
            try
            {
                int.TryParse(PWDGUIDMaxLength, out pwdmaxlendth);
                mypassword = Guid.NewGuid().ToString();
                mypassword = mypassword.Length > pwdmaxlendth ? mypassword.Substring(0, pwdmaxlendth) : mypassword;
                mypassword = Cryptography.GetEncryptedData(mypassword, true);
                //ObjUserModel.UserModel.Password = (!string.IsNullOrEmpty(ObjUserModel.UserModel.Password)) ? Cryptography.GetEncryptedData(ObjUserModel.UserModel.Password, true) : ObjUserModel.UserModel.Password;
            }
            catch (Exception)
            { throw; }
            return mypassword;
        }

        /// <summary>GetUserTypeList
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <Craeted For>Get User Type List</Craeted>
        /// <ModifiedOn>Dec-08-2014</ModifiedOn>
        /// </summary>
        /// <param name="Category"></param>
        /// <param name="CalledByUserType"></param>
        /// <returns></returns>
        public List<GlobalCodeModel> GetUserTypeList(string category, long calledByUserType)
        {
            try
            {
                //return objCommonRepository.GetGlobalCodeData(Category).Where(o=>o.GlobalCodeId==Convert.ToInt64(UserType.Manager)
                //        || o.GlobalCodeId==Convert.ToInt64(UserType.Client)
                //        || o.GlobalCodeId == Convert.ToInt64(UserType.Employee)).ToList();

                if (calledByUserType == Convert.ToInt64(UserType.ITAdministrator, CultureInfo.InvariantCulture) || calledByUserType == Convert.ToInt64(UserType.GlobalAdmin, CultureInfo.InvariantCulture)
                    || calledByUserType == Convert.ToInt64(UserType.Administrator, CultureInfo.InvariantCulture))
                {
                    return objCommonRepository.GetGlobalCodeData(category).Where(o => o.GlobalCodeId == Convert.ToInt64(UserType.Manager, CultureInfo.InvariantCulture)
                            || o.GlobalCodeId == Convert.ToInt64(UserType.Employee, CultureInfo.InvariantCulture)).ToList();
                }
                else if (calledByUserType == Convert.ToInt64(UserType.Manager, CultureInfo.InvariantCulture))
                {
                    return objCommonRepository.GetGlobalCodeData(category).Where(o => o.GlobalCodeId == Convert.ToInt64(UserType.Employee, CultureInfo.InvariantCulture)).ToList();
                }
                else return null;
            }
            catch (Exception)
            { throw; }
        }


        /// <summary>Get List Of Asset
        /// <CreatedBy>Gayatri Pal</CreatedBy>
        /// <Craeted For>Get Asset List from  QRCMaster </Craeted>
        /// <ModifiedOn>Dec-23-2014</ModifiedOn>
        /// </summary>
        /// <param name="Category"></param>
        /// <param name="CalledByUserType"></param>
        /// <returns></returns>
        public List<SelectListItem> GetAssetList(long LocationID)
        {
            QRCMasterRepository objQRCMasterRepository = new QRCMasterRepository();
            try
            {
                List<SelectListItem> lstAsset = objQRCMasterRepository.GetAll(r => r.IsDeleted == false && r.LocationId == LocationID && r.ClientTypeID == null).Select(e => new SelectListItem()
                {
                    Value = Convert.ToString(e.QRCID, CultureInfo.InvariantCulture),
                    Text = e.QRCName
                }).ToList();

                return lstAsset;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>GetUserDetailsByLocationId
        /// <CreatedBy>Roshan Rahood</CreatedBy>
        /// <Craeted For>Get User Details by location Id</Craeted>
        /// <ModifiedOn>Dec-23-2014</ModifiedOn>
        /// </summary>
        /// <param name="locationId">Location Id</param>
        /// <returns></returns>
        public UserDetailsForVerificationModel GetUserDetailsByLocationId(long locationId)
        {
            try
            {
                ObjLocationClientMappingRepository = new LocationClientMappingRepository();
                UserDetailsForVerificationModel ObjUserDetailsModel = new UserDetailsForVerificationModel();
                ObjUserRepository = new UserRepository();

                var ss = ObjLocationClientMappingRepository.GetSingleOrDefault(map => map.LocationId == locationId && map.IsDeleted == false);
                UserRegistration _clientUser = ObjUserRepository.GetSingleOrDefault(u => u.UserId == ss.ClientUserId && u.IsDeleted == false);

                ObjUserDetailsModel.LocationName = ss.LocationMaster.LocationName;
                ObjUserDetailsModel.LocationAddress = ss.LocationMaster.Address1 + "" + ss.LocationMaster.Address2;
                ObjUserDetailsModel.ClientFName = _clientUser.FirstName;
                ObjUserDetailsModel.ClientLName = _clientUser.LastName;
                ObjUserDetailsModel.ClientId = _clientUser.UserId;
                ObjUserDetailsModel.mappintId = ss.LocationClientMappingID;
                ObjUserDetailsModel.EmailAddress = _clientUser.UserEmail;
                ObjUserDetailsModel.UserType = _clientUser.UserType;
                return ObjUserDetailsModel;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>Email To user
        /// <CreatedBy>Roshan Rahood</CreatedBy>
        /// <Craeted For>Get Email To user</Craeted>
        /// <ModifiedOn>Dec-24-2014</ModifiedOn>
        /// </summary>
        /// <param name="locationId">Location Id</param>
        /// <returns></returns>
        public List<EmailToUserModel> GetUsersToEmail(long LocationID, long? employeeId)
        {
            try
            {
                objCommonRepository = new CommonRepository();
                return objCommonRepository.GetUsersToEmail(LocationID, employeeId);
            }
            catch (Exception)
            { throw; }
        }



        public List<LocationListModel> GetLocationByAdminId(long? adminId)
        {
            List<LocationListModel> lstlocation = new List<LocationListModel>();

            try
            {
                var listlocation = objAdminLocationMappingRepository.GetAll(r => r.IsDeleted == false && r.AdminUserId == adminId).Select(l => l.LocationMaster);
                lstlocation = listlocation.Select(r => new LocationListModel()
                {
                    LocationId = r.LocationId,
                    LocationName = r.LocationName
                }).ToList();
                return lstlocation;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<LocationListModel> GetLocationByManagerId(long managerId)
        {
            List<LocationListModel> lstlocation = new List<LocationListModel>();
            try
            {
                var listlocation = objManagerLocationMappingRepository.GetAll(r => r.IsDeleted == false && r.ManagerUserId == managerId).Select(l => l.LocationMaster);
                lstlocation = listlocation.Select(r => new LocationListModel()
                {
                    LocationId = r.LocationId,
                    LocationName = r.LocationName
                }).ToList();
                return lstlocation;

            }
            catch (Exception)
            {
                throw;
            }

        }
        public List<LocationListModel> GetLocationByClientId(long clientId)
        {
            List<LocationListModel> lstlocation = new List<LocationListModel>();
            try
            {
                var listlocation = objLocationClientMappingRepository.GetAll(r => r.IsDeleted == false && r.ClientUserId == clientId).Select(l => l.LocationMaster);
                lstlocation = listlocation.Select(r => new LocationListModel()
                {
                    LocationId = r.LocationId,
                    LocationName = r.LocationName
                }).ToList();
                return lstlocation;

            }
            catch (Exception)
            {
                throw;
            }

        }
        public List<LocationListModel> GetLocationByEmpId(long empId)
        {
            List<LocationListModel> lstlocation = new List<LocationListModel>();
            try
            {
                var listlocation = objEmployeeLocationMapping.GetAll(r => r.IsDeleted == false && r.EmployeeUserId == empId).Select(l => l.LocationMaster);
                lstlocation = listlocation.Select(r => new LocationListModel()
                {
                    LocationId = r.LocationId,
                    LocationName = r.LocationName
                }).ToList();
                return lstlocation;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<UserModel> GetManagersBYLocationId(long locationId)
        {
            try
            {
                List<UserModel> lstManager = objCommonRepository.GetManagersBYLocationId(locationId);
                return lstManager;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UserModel> GetAdminBYLocationId(long locationId)
        {
            try
            {
                List<UserModel> lstManager = objCommonRepository.GetAdminBYLocationId(locationId);
                return lstManager;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UserModel> GetClientsBYLocationId(long locationId)
        {
            try
            {
                List<UserModel> lstClients = objCommonRepository.GetClientsBYLocationId(locationId);
                return lstClients;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public LocationMasterModel GetLocationDetailsById(long locationId)
        {
            try
            {
                LocationMasterModel locationDetails = objCommonRepository.GetLocationDetailsById(locationId);
                return locationDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Result SaveDAR(DARModel objDarModel)
        {
            ObjDARDetail = new DARDetail();
            try
            {
                AutoMapper.Mapper.CreateMap<DARModel, DARDetail>();
                ObjDARDetail = AutoMapper.Mapper.Map(objDarModel, ObjDARDetail);
                ObjDARDetail.CreatedOn = DateTime.UtcNow;
                if (ObjDARDetail.UserId != null && ObjDARDetail.LocationId != null)
                {
                    objDARRepository.Add(ObjDARDetail);
                    return Result.Completed;
                }
                else
                {
                    return Result.Failed;
                }

            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "SaveDAR(DARModel objDarModel)", "Exception Raised while inserting into DARDetails table From CommonMethodManager.cs", ObjDARDetail);
                throw;
            }
        }

        public string GetAssetImageByQrcId(long qrcId)
        {
            ObjQRCMasterRepository = new QRCMasterRepository();
            string AssetImage = string.Empty;
            try
            {
                AssetImage = ObjQRCMasterRepository.GetSingleOrDefault(r => r.QRCID == qrcId).AssetPicture;
                return AssetImage;
            }
            catch (Exception)
            {
                throw;
            }
        }

        ///
        /// Created By Roshan To bind the drop 
        /// down Location on QRC setup page
        ///
        public List<SelectListItem> GetLocationByManagerIdForDdl(long managerId)
        {
            try
            {
                List<SelectListItem> lstlocation = GetLocationByManagerId(managerId).Select(t => new SelectListItem()
                {
                    Text = t.LocationName,
                    Value = Convert.ToString(t.LocationId, CultureInfo.InvariantCulture)
                }).ToList();
                return lstlocation;
            }
            catch (Exception)
            {
                throw;
            }
        }


        ///
        /// Created By Roshan To bind the drop 
        /// Modified by vijay sahu on  15 may 2015 
        /// comment by vijay :- you can get all permissions which is assigned to selected location 
        /// down Location on QRC setup page
        ///
        public List<PermissionDetailsModel> GetAllPermissions(long locationId)
        {

            if (locationId == 0)
            {
                try
                {
                    List<GlobalCodeModel> lstPermission = objCommonRepository.GetGlobalCodeData("PERMISSION");
                    List<PermissionDetailsModel> lstPermissionDetails = new List<PermissionDetailsModel>();
                    PermissionDetailsModel objPermissionDetailsModel;
                    foreach (var item in lstPermission)
                    {
                        objPermissionDetailsModel = new PermissionDetailsModel();
                        objPermissionDetailsModel.PermissionId = item.GlobalCodeId;
                        objPermissionDetailsModel.PermissionName = item.CodeName;
                        lstPermissionDetails.Add(objPermissionDetailsModel);
                    }

                    return lstPermissionDetails;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {// for getting only those service which is assigned to selected locaiton.
                try
                {
                    //List<GlobalCodeModel> lstPermission = objCommonRepository.GetGlobalCodeData("PERMISSION");
                    List<PermissionDetailsModel> lstPermissionDetails = new List<PermissionDetailsModel>();
                    //PermissionDetailsModel objPermissionDetailsModel;
                    //foreach (var item in lstPermission)
                    //{
                    //    objPermissionDetailsModel = new PermissionDetailsModel();
                    //    objPermissionDetailsModel.PermissionId = item.GlobalCodeId;
                    //    objPermissionDetailsModel.PermissionName = item.CodeName;
                    //    lstPermissionDetails.Add(objPermissionDetailsModel);
                    //}



                    // here we have two 
                    //added by vijay sahu on 25 april 2015
                    using (workorderEMSEntities Context = new workorderEMSEntities())
                    {
                        lstPermissionDetails = (from o in Context.GlobalCodes
                                                join sm in Context.ServiceMasters
                                                 on o.Description.Trim() equals sm.ServiceName.Trim()
                                                join ls in Context.LocationServices
                                                on sm.ServiceID equals ls.ServiceId
                                                where ls.LocationID == locationId
                                                select new PermissionDetailsModel()
                                                {
                                                    PermissionId = sm.ServiceID,
                                                    PermissionName = sm.ServiceName,
                                                }).ToList();



                        //lstPermissionDetails = (from o in Context.LocationServices
                        //                        join sm in Context.ServiceMasters
                        //                        on o.ServiceId equals sm.ServiceID
                        //                        where o.LocationID == locationId
                        //                        select new PermissionDetailsModel()
                        //                        {
                        //                            PermissionDetailId = o.ServiceId,
                        //                            PermissionName = sm.ServiceName,
                        //                        }).ToList();
                    }

                    return lstPermissionDetails;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        /// <summary>
        ///Created by vijay sahu on  15 may 2015 
        /// comment by vijay :- you can get all permissions which is assigned to selected location and also based on userType
        /// down Location on QRC setup page
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public List<PermissionDetailsModel> GetPermissionsWithFilterByUserTypeLocationId(long locationId, long UserId)
        {
            List<PermissionDetailsModel> lstPermissionDetails = new List<PermissionDetailsModel>();
            if (locationId == 0)
            {
                try
                {
                    List<GlobalCodeModel> lstPermission = objCommonRepository.GetGlobalCodeData("PERMISSION");

                    PermissionDetailsModel objPermissionDetailsModel;
                    foreach (var item in lstPermission)
                    {
                        objPermissionDetailsModel = new PermissionDetailsModel();
                        objPermissionDetailsModel.PermissionId = item.GlobalCodeId;
                        objPermissionDetailsModel.PermissionName = item.CodeName;
                        lstPermissionDetails.Add(objPermissionDetailsModel);
                    }

                    return lstPermissionDetails;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {// for getting only those service which is assigned to selected locaiton.
                try
                {


                    // here we have two 
                    //added by vijay sahu on 25 april 2015
                    using (workorderEMSEntities Context = new workorderEMSEntities())
                    {
                        lstPermissionDetails = (from o in Context.GlobalCodes
                                                join sm in Context.ServiceMasters
                                                 on o.Description.Trim() equals sm.ServiceName.Trim()
                                                join ls in Context.LocationServices
                                                on sm.ServiceID equals ls.ServiceId
                                                where ls.LocationID == locationId
                                                && sm.IsDeleted == false
                                                select new PermissionDetailsModel()
                                                {
                                                    PermissionId = sm.ServiceID,
                                                    PermissionName = sm.ServiceName,
                                                }).ToList();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            long UserTy = 0;
            using (workorderEMSEntities context = new workorderEMSEntities())
            {
                UserTy = (from o in context.UserRegistrations
                          where o.UserId == UserId && o.IsDeleted == false
                          select o.UserType).FirstOrDefault();

                if (UserTy == 4 || UserTy == 3)
                {
                    lstPermissionDetails.RemoveAll(a => a.PermissionName == "Location Setup");
                    lstPermissionDetails.RemoveAll(a => a.PermissionName == "Manage Users");

                }
            }



            return lstPermissionDetails;
        }

        /// <summary>Get employee list
        /// <CreatedBy>Bhushan Dod </CreatedBY>
        /// <CreatedOn>Jan-14-2015</CreatedOn>
        /// <CreatedFor> GetEmployeeListByLocationID</CreatedFor>
        /// </summary>
        /// <param name="ServiceAuthKey"></param>
        /// <returns></returns>
        public List<UserModel> GetEmployeeListByLocation(long location)
        {

            try
            {
                List<UserModel> emplist = objCommonRepository.GetEmployeeListByLocation(location).Select(t => new UserModel()
                {

                }).ToList();
                return emplist;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>Update the permissions
        /// <CreatedBy>Roshan Rahood </CreatedBY>
        /// <CreatedOn>Feb 17 2015</CreatedOn>
        /// <CreatedFor> UpdateUserRoles</CreatedFor>
        /// </summary>
        /// <returns></returns>
        public bool UpdateUserPermissions(PermissionDetailsModel objPermissionDetailsModel)
        {
            try
            {
                objPermissionDetailsRepository = new PermissionDetailsRepository();

                ////if (DeleteUserPermission(objPermissionDetailsModel.UserId, objPermissionDetailsModel.LocationId) == true)
                if (objPermissionDetailsModel.UserId != 0)
                {
                    if (objPermissionDetailsModel.UserIds != null)
                    {
                        var GetOldPremission = objPermissionDetailsRepository.GetAll(x => x.UserId == objPermissionDetailsModel.UserId && x.LocationId == objPermissionDetailsModel.LocationId).ToList();
                        foreach (var i in GetOldPremission)
                        {
                            objPermissionDetailsRepository.Delete(i);
                            objPermissionDetailsRepository.SaveChanges();
                        }
                        var userPermissionArray = objPermissionDetailsModel.UserIds.Split(',');
                        foreach (var permission in userPermissionArray)
                        {
                            long permisssionId = Convert.ToInt64(permission, CultureInfo.InvariantCulture);
                            //   objPermissionDetail = objPermissionDetailsRepository.GetAll(t => t.UserId == objPermissionDetailsModel.UserId && t.LocationId == objPermissionDetailsModel.LocationId).;
                            //   if (objPermissionDetail.PermissionDetailId > 0)
                            //   {
                            //       if (objPermissionDetail.IsDeleted == true && objPermissionDetail.PermissionId == permisssionId)
                            //       {
                            //           objPermissionDetail.IsDeleted = false;
                            //           objPermissionDetailsRepository.Update(objPermissionDetail);
                            //       }
                            //       else
                            //       {
                            //           objPermissionDetail.IsDeleted = true;
                            //           objPermissionDetailsRepository.Update(objPermissionDetail);
                            //       }
                            //   }
                            //   else
                            //   {
                            AddUserPermissions(permisssionId, objPermissionDetailsModel.UserId, objPermissionDetailsModel.LocationId, objPermissionDetailsModel.CreatedBy);
                            //}
                        }
                    }
                    else if (objPermissionDetailsModel.UserIds == null && objPermissionDetailsModel.UserId > 0 && objPermissionDetailsModel.LocationId > 0)
                    {
                        var GetOldPremission = objPermissionDetailsRepository.GetAll(x => x.UserId == objPermissionDetailsModel.UserId && x.LocationId == objPermissionDetailsModel.LocationId).ToList();
                        foreach (var i in GetOldPremission)
                        {
                            objPermissionDetailsRepository.Delete(i);
                            objPermissionDetailsRepository.SaveChanges();
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            { return false; }
        }


        /// <summary>Add the permissions
        /// <CreatedBy>Roshan Rahood </CreatedBY>
        /// <CreatedOn>Feb 17 2015</CreatedOn>
        /// <CreatedFor> UpdateUserRoles</CreatedFor>
        /// </summary>
        /// <returns></returns>
        public bool AddUserPermissions(long permissionId, long userId, long locationId, long createdBy)
        {

            PermissionDetail objPermissionDetails = new PermissionDetail();
            objPermissionDetailsRepository = new PermissionDetailsRepository();

            try
            {
                objPermissionDetails.PermissionId = permissionId;
                objPermissionDetails.UserId = userId;
                objPermissionDetails.LocationId = locationId;
                objPermissionDetails.CreatedBy = createdBy;
                objPermissionDetails.CreatedOn = DateTime.UtcNow;
                objPermissionDetailsRepository.Add(objPermissionDetails);
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        public List<PermissionDetailsModel> GetAssignPermission(long userId, long Locationid)
        {
            objPermissionDetailsRepository = new PermissionDetailsRepository();
            try
            {
                return objPermissionDetailsRepository.GetAssignPermissions(Convert.ToInt32(userId), Convert.ToInt32(Locationid));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objPermissionDetailsRepository = null;
            }
        }

        public bool DeleteUserPermission(long userId, long locationId)
        {
            List<PermissionDetail> objPermissionDetail = new List<PermissionDetail>();
            objPermissionDetailsRepository = new PermissionDetailsRepository();
            try
            {
                objPermissionDetail = objPermissionDetailsRepository.GetAll(t => t.UserId == userId && t.LocationId == locationId).ToList();
                if (objPermissionDetail.Count > 0)
                {
                    foreach (var item in objPermissionDetail)
                    {
                        item.IsDeleted = true;
                    }
                    objPermissionDetailsRepository.ListUpdate(objPermissionDetail);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>Save Email Log
        /// <CreatedBy>Bhushan Dod </CreatedBY>
        /// <CreatedOn>Feb-17-2015</CreatedOn>
        /// <CreatedFor> Save entry in EmailLog for LocationVerification</CreatedFor>
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="SentTo"></param>
        /// <param name="emailid"></param>
        /// <param name="Subject"></param>
        /// <param name="LocationId"></param>
        /// <returns>bool</returns>
        public bool EmailLog(long userId, long? sentTo, string emailId, string subject, long locationId)
        {
            bool log = false;
            objemail = new Data.EntityModel.EmailLog();
            objemail.SentBy = userId;
            objemail.SentTo = sentTo;
            objemail.SentEmail = emailId;
            objemail.Subject = subject;
            objemail.LocationId = locationId;
            objemail.CreatedBy = userId;
            objemail.CreatedDate = DateTime.UtcNow;
            objemail.DeletedBy = null;
            objemail.DeletedOn = null;
            objemail.ModifiedBy = null;
            objemail.ModifiedOn = null;

            log = objEmailLogRepository.SaveEmailLog(objemail);
            return log;
        }
        /// <summary>Get Dashboard Details 
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>DashboardCount</CreatedFor>
        /// <CreatedOn>April-21-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCElevatorModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<ServiceDashboardModel> GetCountforDashboard(ServiceDashboardModel obj)
        {
            ServiceResponseModel<ServiceDashboardModel> ObjServiceResponseModel = new ServiceResponseModel<ServiceDashboardModel>();
            ServiceDashboardModel result = new ServiceDashboardModel();
            ObjUserRepository = new UserRepository();
            try
            {
                var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == obj.ServiceAuthKey && x.IsDeleted == false);
                if (authuser != null && authuser.UserId > 0)
                {
                    result = objCommonRepository.DashboardCountForMobile(obj.UserId, obj.LocationId);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Data = result;
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Data = result;
                        ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                    }
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.InvalidSessionResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "ServiceResponseModel<ServiceDashboardModel> GetCountforDashboard(ServiceDashboardModel obj)", "While get count for dashboard", obj);
                throw ex;
            }
            return ObjServiceResponseModel;
        }


        /// <summary>
        /// Get user Type based on user Id and then find out in which location he has assigned. 
        /// </summary>5
        public IsMapped isUserMappedWithLocation(long UserID, long LocationID)
        {
            ObjUserRepository = new UserRepository();
            IsMapped objIsMapped = new IsMapped();
            objIsMapped.userTypeRes = ObjUserRepository.GetAll().Where(x => x.UserId == UserID).Select(x => x.UserType).FirstOrDefault();

            long result = 0;
            objIsMapped.IsMappedLocation = false;
            try
            {
                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    switch (objIsMapped.userTypeRes)
                    {
                        case 1:

                            result = (from o in Context.AdminLocationMappings
                                      where o.AdminUserId == UserID
                                      && o.LocationId == LocationID
                                      select o.AdminUserId).FirstOrDefault();
                            break;

                        case 6:
                            result = (from o in Context.AdminLocationMappings
                                      where o.AdminUserId == UserID
                                      && o.LocationId == LocationID
                                      select o.AdminUserId).FirstOrDefault();

                            break;
                        case 2:

                            result = (from o in Context.ManagerLocationMappings
                                      where o.ManagerUserId == UserID
                                      && o.LocationId == LocationID
                                      select o.ManagerUserId).FirstOrDefault();
                            break;
                        case 3:

                            result = (from o in Context.EmployeeLocationMappings
                                      where o.EmployeeUserId == UserID
                                      && o.LocationId == LocationID
                                      select o.EmployeeUserId).FirstOrDefault();
                            break;

                        case 4:
                            result = (from o in Context.LocationClientMappings
                                      where o.ClientUserId == UserID
                                      && o.LocationId == LocationID
                                      select o.ClientUserId).FirstOrDefault();
                            break;
                    }
                }
                if (result > 0)
                {

                    objIsMapped.IsMappedLocation = true;
                }

            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public byte isUserMappedWithLocation(long UserID, long LocationID)", "", ("UserID:" + UserID + "LocationID:" + LocationID));
            }
            return objIsMapped;
        }

        /// <summary>
        /// Just for testing purpose plz don't use this function anywhere.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool SendEmailJustforTesting(string email, string message = "")
        {
            try
            {
                if (message == null)
                {
                    message = "";
                }

                if (message.Length > 0)
                { }
                else
                { message = "Default message testing by vijay"; }
                List<System.Net.Mail.Attachment> tt = new List<System.Net.Mail.Attachment>();
                EmailHelper.SendEmail(email, message, "Hello Testing vijay sahu", tt);
                return true;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SendEmailJustforTesting(string email)", "Exception while sending notification for testing purpose.", email);
                return false;
            }
        }

        public EmailToUserModel GetUserByID(long UserID)
        {
            EmailToUserModel obj = new EmailToUserModel();
            try
            {
                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    obj = (from o in Context.UserRegistrations
                           where o.UserId == UserID
                           && o.IsDeleted != true
                           select new EmailToUserModel
                           {
                               UserEmail = o.UserEmail,
                               AlternareEmail = o.AlternateEmail,
                               UserType = o.UserType,
                               FirstName = o.FirstName,
                               LastName = o.LastName

                           }).FirstOrDefault();


                    //return new EmailToUserModel ({});
                }
            }
            catch (Exception)
            {

                throw;
            }

            return obj;
        }

        /// <summary>
        /// Get TimeZone Info of SQL + c#
        /// </summary>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedDate>20/10/2015</CreatedDate>
        /// <returns></returns>
        public TimeZoneInfoModel GetTimeZoneInfo(string server)
        {
            try
            {
                TimeZoneInfoModel objTimeZoneInfoModel = new TimeZoneInfoModel();
                if (Convert.ToInt32(server) == (int)WorkOrderEMS.Helper.TimeZoneDetails.SQL)
                {
                    workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();
                    objTimeZoneInfoModel = _workorderEMSEntities.ssp_TimeZoneInfo()
                        .Select(tz => new TimeZoneInfoModel
                        {
                            GMT = tz.GMT,
                            IndianStandardTime = tz.IndianStandardTime,
                            SQLServerTime = tz.SQLServerTime,
                            TimeZoneName = tz.TimeZoneName,
                        }).SingleOrDefault();
                }
                else if (Convert.ToInt32(server) == (int)WorkOrderEMS.Helper.TimeZoneDetails.Both)
                {
                    workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();
                    objTimeZoneInfoModel = _workorderEMSEntities.ssp_TimeZoneInfo()
                        .Select(tz => new TimeZoneInfoModel
                        {
                            GMT = tz.GMT,
                            IndianStandardTime = tz.IndianStandardTime,
                            SQLServerTime = tz.SQLServerTime,
                            TimeZoneName = tz.TimeZoneName,
                        }).SingleOrDefault();
                    System.TimeZone zone = System.TimeZone.CurrentTimeZone;
                    TimeSpan offset = zone.GetUtcOffset(DateTime.Now);    // Get offset.
                    DaylightTime time = zone.GetDaylightChanges(DateTime.Today.Year);
                    objTimeZoneInfoModel.zone = zone;
                    objTimeZoneInfoModel.offset = offset;
                    objTimeZoneInfoModel.time = time;
                    objTimeZoneInfoModel.standard = zone.StandardName;
                    objTimeZoneInfoModel.daylight = zone.DaylightName;

                }
                else
                {
                    System.TimeZone zone = System.TimeZone.CurrentTimeZone;
                    TimeSpan offset = zone.GetUtcOffset(DateTime.Now);    // Get offset.
                    DaylightTime time = zone.GetDaylightChanges(DateTime.Today.Year);
                    objTimeZoneInfoModel.zone = zone;
                    objTimeZoneInfoModel.offset = offset;
                    objTimeZoneInfoModel.time = time;
                    objTimeZoneInfoModel.standard = zone.StandardName;
                    objTimeZoneInfoModel.daylight = zone.DaylightName;
                }
                return objTimeZoneInfoModel;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public TimeZoneInfoModel GetTimeZoneInfo(string server)", "Exception while TimeZoneInfo", 6);
                throw;
            }
        }

        /// <summary>GetUserTypeListForUserRegistration
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <Craeted For>Get User Type List</Craeted>
        /// <ModifiedOn>Thu-08-2016</ModifiedOn>
        /// </summary>
        /// <param name="Category"></param>
        /// <param name="CalledByUserType"></param>
        /// <returns></returns>
        public List<GlobalCodeModel> GetUserTypeListForUserRegistration(string category, long calledByUserType)
        {
            try
            {

                if (calledByUserType == Convert.ToInt64(UserType.GlobalAdmin, CultureInfo.InvariantCulture))
                {
                    return objCommonRepository.GetGlobalCodeData(category).Where(o => o.GlobalCodeId == Convert.ToInt64(UserType.ITAdministrator, CultureInfo.InvariantCulture)
                                                                                    || o.GlobalCodeId == Convert.ToInt64(UserType.Administrator, CultureInfo.InvariantCulture)
                                                                                    || o.GlobalCodeId == Convert.ToInt64(UserType.Manager, CultureInfo.InvariantCulture)
                                                                                    || o.GlobalCodeId == Convert.ToInt64(UserType.Employee, CultureInfo.InvariantCulture)).ToList();
                }
                else if (calledByUserType == Convert.ToInt64(UserType.ITAdministrator, CultureInfo.InvariantCulture))
                {
                    return objCommonRepository.GetGlobalCodeData(category).Where(o => o.GlobalCodeId == Convert.ToInt64(UserType.ITAdministrator, CultureInfo.InvariantCulture)
                                                                                    || o.GlobalCodeId == Convert.ToInt64(UserType.Administrator, CultureInfo.InvariantCulture)
                                                                                    || o.GlobalCodeId == Convert.ToInt64(UserType.Manager, CultureInfo.InvariantCulture)
                                                                                    || o.GlobalCodeId == Convert.ToInt64(UserType.Employee, CultureInfo.InvariantCulture)).ToList();
                }
                else if (calledByUserType == Convert.ToInt64(UserType.Administrator, CultureInfo.InvariantCulture))
                {
                    return objCommonRepository.GetGlobalCodeData(category).Where(o => o.GlobalCodeId == Convert.ToInt64(UserType.Administrator, CultureInfo.InvariantCulture)
                                                                                    || o.GlobalCodeId == Convert.ToInt64(UserType.Manager, CultureInfo.InvariantCulture)
                                                                                    || o.GlobalCodeId == Convert.ToInt64(UserType.Employee, CultureInfo.InvariantCulture)).ToList();
                }
                else if (calledByUserType == Convert.ToInt64(UserType.Manager, CultureInfo.InvariantCulture))
                {
                    return objCommonRepository.GetGlobalCodeData(category).Where(o => o.GlobalCodeId == Convert.ToInt64(UserType.Manager, CultureInfo.InvariantCulture)
                                                                                    || o.GlobalCodeId == Convert.ToInt64(UserType.Employee, CultureInfo.InvariantCulture)).ToList();
                }
                else return null;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>Add the Permission
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Sep 13 2016</CreatedOn>
        /// <CreatedFor> Adding AddPermissionDetail </CreatedFor>
        /// </summary>
        /// <returns></returns>
        public static bool AddPermissionDetail(long ServiceId, long userId, long locationId)
        {
            PermissionDetail objPermissionDetail = new PermissionDetail();
            PermissionDetailsRepository objPermissionDetailsRepository = new PermissionDetailsRepository();
            try
            {
                using (TransactionScope TranScope = new TransactionScope())
                {
                    objPermissionDetail.PermissionId = ServiceId;
                    objPermissionDetail.UserId = userId;
                    objPermissionDetail.LocationId = locationId;
                    objPermissionDetail.CreatedBy = userId;
                    objPermissionDetail.CreatedOn = DateTime.UtcNow;
                    objPermissionDetailsRepository.Add(objPermissionDetail);
                    TranScope.Complete();
                }
                if (objPermissionDetail.PermissionDetailId > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool AddPermissionDetail(long ServiceId, long userId, long locationId)", "Exception in  CommonMethodManager.cs", ServiceId);
                return false;
            }
        }

        /// <summary>
        /// Created By - Bhushan Dod
        /// Created On - 04/September/2016
        /// Description - This function for getting location service list for user registration
        /// </summary>
        /// <param name="locationId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<PermissionDetailsModel> GetPermissionsWithUserType(long locationId, long userType)
        {
            List<PermissionDetailsModel> lstPermissionDetails = new List<PermissionDetailsModel>();
            if (locationId == 0)
            {
                try
                {
                    List<GlobalCodeModel> lstPermission = objCommonRepository.GetGlobalCodeData("PERMISSION");
                    PermissionDetailsModel objPermissionDetailsModel;
                    foreach (var item in lstPermission)
                    {
                        objPermissionDetailsModel = new PermissionDetailsModel();
                        objPermissionDetailsModel.PermissionId = item.GlobalCodeId;
                        objPermissionDetailsModel.PermissionName = item.CodeName;
                        lstPermissionDetails.Add(objPermissionDetailsModel);
                    }
                    return lstPermissionDetails;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {// for getting only those service which is assigned to selected locaiton.
                try
                {
                    using (workorderEMSEntities Context = new workorderEMSEntities())
                    {
                        lstPermissionDetails = (from o in Context.GlobalCodes
                                                join sm in Context.ServiceMasters
                                                 on o.Description.Trim() equals sm.ServiceName.Trim()
                                                join ls in Context.LocationServices
                                                on sm.ServiceID equals ls.ServiceId
                                                where ls.LocationID == locationId
                                                && sm.IsDeleted == false
                                                select new PermissionDetailsModel()
                                                {
                                                    PermissionId = sm.ServiceID,
                                                    PermissionName = sm.ServiceName,
                                                }).ToList();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            if (userType == 4 || userType == 3)
            {
                lstPermissionDetails.RemoveAll(a => a.PermissionName == "Location Setup");
                lstPermissionDetails.RemoveAll(a => a.PermissionName == "Manage Users");
            }
            return lstPermissionDetails;
        }

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created Date: 05/10/2016
        /// This method fetch all the data of user for UnVerified edit user.
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        public QRCModel LoadUnVerifiedInvitedUser(string usr)
        {
            try
            {
                long userid = 0;
                if (!string.IsNullOrEmpty(usr))
                {
                    usr = Cryptography.GetDecryptedData(usr, true);
                    long.TryParse(usr, out userid);
                }
                QRCModel _UserModel = new QRCModel();

                if (userid > 0)
                {
                    long Totalrecord = 0;
                    _UserModel.UserModel = GetUserIdCode(userid, "GetUserByID", null, null, null, null, null, out Totalrecord);
                    _UserModel.UserModel.Password = "";
                    _UserModel.UserModel.myProfileImage =
                    (_UserModel.UserModel.myProfileImage == "" || _UserModel.UserModel.myProfileImage == null) ? "no-profile-pic.jpg" : HostingPrefix + ProfileImagePath.Replace("~", "") + _UserModel.UserModel.myProfileImage;
                }

                return _UserModel;
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created Date: 05/10/2016
        /// This method fetch all the data of user for UnVerified edit user.
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        public UserModel GetUserIdCode(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, out long totalRecords)
        {
            totalRecords = 0;
            ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
            ObjUserRepository = new UserRepository();
            try
            {
                var result = ObjUserRepository.GetUserByIdForUnverifiedUser(userId, operationName, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, paramTotalRecords);
                result.Password = "";
                if (paramTotalRecords != null && paramTotalRecords.Value != null && paramTotalRecords.Value != DBNull.Value)
                { long.TryParse(paramTotalRecords.Value.ToString(), out totalRecords); }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Result AssignLocationRoles(PermissionDetailsModel objPermissionDetailsModel, DARModel objDarModel, long createdBy)
        {

            ObjDARDetail = new DARDetail();
            CommonMethodAdmin objCommonMethod = new CommonMethodAdmin();
            CommonMethodManager objCommonManager = new CommonMethodManager();
            try
            {
                if (objPermissionDetailsModel.UserType == Convert.ToInt64(UserType.GlobalAdmin, CultureInfo.InvariantCulture)
                                    || objPermissionDetailsModel.UserType == Convert.ToInt64(UserType.ITAdministrator, CultureInfo.InvariantCulture)
                                    || objPermissionDetailsModel.UserType == Convert.ToInt64(UserType.Administrator, CultureInfo.InvariantCulture)
                    //Commented by Bhushan on 30/05/2016 for no need to check manager bcoz objBB.AssignRoleAndPermission code is doing same 
                    // || objUserModel.UserType == Convert.ToInt64(UserType.Manager, CultureInfo.InvariantCulture)
                                    )
                {
                    objCommonMethod.AssignLocationToAdminUser(objPermissionDetailsModel.LocationId, objPermissionDetailsModel.UserId);
                }
                else if (objPermissionDetailsModel.UserType == Convert.ToInt64(UserType.Manager, CultureInfo.InvariantCulture))
                {
                    objCommonMethod.AssignLocationToManagerUser(objPermissionDetailsModel.LocationId, objPermissionDetailsModel.UserId);
                }
                else if (objPermissionDetailsModel.UserType == Convert.ToInt64(UserType.Employee, CultureInfo.InvariantCulture))
                {
                    objCommonMethod.AssignLocationToEmployeeUser(objPermissionDetailsModel.LocationId, objPermissionDetailsModel.UserId);
                }
                Result objResult = objCommonManager.SaveDAR(objDarModel);
                ////this block is used for assigning roles to the users.. 
                {
                    WorkOrderEMS.BusinessLogic.Managers.Common_B objBB = new WorkOrderEMS.BusinessLogic.Managers.Common_B();
                    byte a = objBB.AssignRoleAndPermission(objPermissionDetailsModel.UserId, objPermissionDetailsModel.UserType, objPermissionDetailsModel.LocationId, createdBy, "");
                }
                if (objPermissionDetailsModel.UserIds != "" && objPermissionDetailsModel.UserIds != null && objPermissionDetailsModel.UserIds.Trim() != "")
                {
                    GlobalAdminManager ObjGlobalAdminManager = new GlobalAdminManager();

                    var userServicesID = objPermissionDetailsModel.UserIds.Split(',');
                    if (userServicesID != null && userServicesID.Length > 0)
                    {
                        foreach (var service in userServicesID)
                        {
                            if (service != null && !string.IsNullOrEmpty(service) && Convert.ToInt64(service, CultureInfo.InvariantCulture) > 0)
                            {
                                long WidgetId = Convert.ToInt64(service, CultureInfo.InvariantCulture);
                                var IsInserted = CommonMethodManager.AddPermissionDetail(WidgetId, objPermissionDetailsModel.UserId, objPermissionDetailsModel.LocationId);
                            }
                        }
                        //Added By Bhushan Dod on 07/06/2016 for bydefault setting when location created according to loc services.
                        ObjGlobalAdminManager.SaveByDefaultWidgetSetting(objPermissionDetailsModel.LocationId, objPermissionDetailsModel.UserIds, objPermissionDetailsModel.UserId);
                    }
                }
                if (objResult == Result.Completed)
                {
                    return Result.Completed;
                }
                else
                {
                    return Result.Failed;
                }

            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "Result AssignLocationRoles(PermissionDetailsModel objPermissionDetailsModel, DARModel objDarModel)", "Exception Raised while AssignLocationRoles From CommonMethodManager.cs", ObjDARDetail);
                return Result.Failed;
            }
        }
    }
}
