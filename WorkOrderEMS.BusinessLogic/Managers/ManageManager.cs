using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;
using WorkOrderEMS.Models.ManagerModels;


namespace WorkOrderEMS.BusinessLogic
{
    public class ManageManager : IManageManager
    {

        long _createdBy = 1; DateTime _createdOn = DateTime.UtcNow;
        string _defaultCompanyLogo = "";
        string _defaultVendorImage = "";
        long _defaultCompanySize = 100;
        //_joinFrom = DateTime.Now;

        #region Declaration Section
        UserRepository ObjUserRepository;
        //VehicleRepository ObjVehicleRepository;
        UserRegistration ObjManagerUser;
        AddressManager objAddressManager = new AddressManager();
        GlobalCodesRepository objGlobalCodesRepository = new GlobalCodesRepository();
        InventoryMasterRepository objInventoryMasterRepository;
        AssignInventoryRepository objAssignInventoryRepository = new AssignInventoryRepository();
        //WorkRequestRepository objWorkRequestRepository;
        //WorkOrderRepository objWorkOrderRepository;
        //VendorRepository ObjVendorRepository;        
        //InventoryMaster ObjInventoryMaster;

        RuleRepository objRuleRepository;
        LocationRuleMappingRepository objLocationRuleMappingRepository;
        //AssetMasterRepository objAssetMasterRepository;
        AddressMasterRepository ObjAddressMasterRepo;

        CommonMethodManager _ICommonMethod = new CommonMethodManager();
        EmailLog objEmailog = null;
        EmailLogRepository objEmailLogRepository = new EmailLogRepository();
        UserManager objUserManager = null;
        LoginLogRepository objLoginLogRepository;
        QRCMasterRepository objQRCMasterRepository;

        //Added By Kartik
        string message = string.Empty;
        #endregion Declaration Section

        #region Employee

        /// <summary>GetEmployeeByID
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="OperationName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <param name="paramTotalRecords"></param>
        /// <returns></returns>
        public UserModel GetEmployeeById(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords)
        {
            ObjUserRepository = new UserRepository();
            try
            {
                return ObjUserRepository.GetUserById(userId, operationName, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, totalRecords);

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>SaveEmployee
        /// 
        /// </summary>
        /// <param name="objUserModel"></param>
        /// <param name="QRCID"></param>
        /// <param name="IsEmployeeRegistration"></param>
        /// <returns></returns>
        public Result SaveEmployee(UserModel objUserModel, out long QRCID, bool isEmployeeRegistration)
        {
            try
            {

                ObjUserRepository = new UserRepository();
                ObjManagerUser = new UserRegistration();

                if (CheckDuplicateUser(objUserModel.UserEmail.Trim(), objUserModel.UserId, out QRCID, out ObjManagerUser))
                {
                    objUserModel.IsEmailVerify = true;
                    objUserModel.IsLoginActive = true;
                    if (objUserModel.Gender != null)
                    {
                        objUserModel.Gender = objUserModel.Gender == 1 ? objGlobalCodesRepository.GetSingleOrDefault(g => g.CodeName == "Male").GlobalCodeId : objGlobalCodesRepository.GetSingleOrDefault(g => g.CodeName == "Female").GlobalCodeId;
                    }
                    if (objUserModel.UserId == 0)
                    {

                        AutoMapper.Mapper.CreateMap<UserModel, UserRegistration>();
                        ObjManagerUser = AutoMapper.Mapper.Map(objUserModel, ObjManagerUser);
                        ObjUserRepository.Add(ObjManagerUser);
                        //ObjUserRepository.SaveChanges();
                        //if (ObjManagerUser != null)
                        //{
                        //    //ObjManagerUser.Password = (string.IsNullOrEmpty(ObjManagerUser.Password)) ? Cryptography.GetDecryptedData(0.ToString(), true) : ObjManagerUser.Password;
                        //    if (_ICommonMethod.GenerateQRCode(QRCName, ManageManagerMODULE, null, null, QRCDefaultSizeID, QRCTYPEID, SpecialNotes, objUserModel.CreatedBy, out QRCID))
                        //    { ObjUserRepository.Add(ObjManagerUser); }
                        //}
                        if (ObjManagerUser.UserId > 0)
                        {
                            objUserModel.Address.UserId = ObjManagerUser.UserId;
                            objAddressManager.SaveAddress(objUserModel.Address);
                            return Result.Completed;
                        }
                        else
                            return Result.Failed;
                    }
                    else
                    {
                        UpdateUser(objUserModel, out QRCID, isEmployeeRegistration, out ObjManagerUser);
                        objUserModel.Address.UserId = objUserModel.UserId;
                        objAddressManager.SaveAddress(objUserModel.Address);
                        return Result.UpdatedSuccessfully;
                    }
                }
                else
                { return Result.DuplicateRecord; }
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>ChkDuplicateUser
        /// 
        /// </summary>
        /// <param name="UserEmail"></param>
        /// <param name="UserID"></param>
        /// <param name="QRCID"></param>
        /// <param name="ObjUserRegistration"></param>
        /// <returns></returns>
        public bool CheckDuplicateUser(string userEmail, long userId, out long qrcId, out UserRegistration objUserRegistration)
        {
            try
            {
                qrcId = 0; objUserRegistration = null;
                ObjUserRepository = new UserRepository();
                var data = ObjUserRepository.GetAll(u => u.UserEmail == userEmail.Trim() && u.UserId != userId && u.IsDeleted == false);
                if (data.Count > 0)
                {
                    qrcId = (data[0].QRCID.HasValue) ? (data[0].QRCID.Value) : 0;
                    objUserRegistration = data[0];
                    return false;
                }
                return true;
            }
            catch (Exception)
            { throw; }
        }


        ///// <summary>ChkDuplicateVehicle
        ///// 
        ///// </summary>
        ///// <param name="RegistrationNo"></param>
        ///// <param name="VehicleID"></param>
        ///// <param name="ObjVehicleMaster"></param>
        ///// <returns></returns>
        //public bool ChkDuplicateVehicle(string RegistrationNo, long VehicleID, out VehicleMaster ObjVehicleMaster)
        //{
        //    try
        //    {
        //        ObjVehicleMaster = null;
        //        ObjVehicleRepository = new VehicleRepository();
        //        var data = ObjVehicleRepository.GetAll(v => v.RegistrationNo == RegistrationNo.Trim() && v.VehicleID != VehicleID);
        //        if (data.Count > 0)
        //        {
        //            ObjVehicleMaster = data[0];
        //            return false;
        //        }
        //        return true;
        //    }
        //    catch (Exception)
        //    { throw; }
        //}

        /// <summary>UpdateUser
        /// 
        /// </summary>
        /// <param name="objUserModel"></param>
        /// <param name="QRCID"></param>
        /// <param name="IsEmployeeRegistration"></param>
        public void UpdateUser(UserModel objUserModel, out long qrcId, bool isEmployeeRegistration, out UserRegistration user)
        {
            ObjUserRepository = new UserRepository();
            user = null;
            user = ObjUserRepository.GetSingleOrDefault(u => u.UserId == objUserModel.UserId && u.IsDeleted == false);
            qrcId = (user.QRCID.HasValue) ? user.QRCID.Value : 0;
            if (user != null)
            {
                user.FirstName = objUserModel.FirstName;
                user.LastName = objUserModel.LastName;
                user.DOB = Convert.ToDateTime(objUserModel.DOB);
                user.BloodGroup = objUserModel.BloodGroup;
                user.AlternateEmail = objUserModel.AlternateEmail;
                user.Gender = objUserModel.Gender;
                if (isEmployeeRegistration)
                { user.Password = objUserModel.Password; }
                user.IsEmailVerify = objUserModel.IsEmailVerify;
                user.IsLoginActive = objUserModel.IsLoginActive;
                user.Gender = objUserModel.Gender;
                if (!string.IsNullOrEmpty(objUserModel.ProfileImage.FileName)) { user.ProfileImage = objUserModel.ProfileImage.FileName; }
                ObjUserRepository.Update(user);
            }
        }


        /// <summary>GetAllVerfiedEmployee
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="OperationName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <param name="paramTotalRecords"></param>
        /// <returns></returns>
        public List<UserModelList> GetAllVerfiedEmployee(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords)
        {
            ObjUserRepository = new UserRepository();
            try
            {
                return ObjUserRepository.GetAllVerfiedUser(userId, operationName, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, totalRecords);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>AssignProfile
        /// 
        /// </summary>
        /// <param name="objUserModel"></param>
        /// <returns></returns>
        public Result AssignProfile(UserModel objUserModel)
        {
            ObjUserRepository = new UserRepository();
            try
            {
                var data = ObjUserRepository.GetSingleOrDefault(u => u.UserId == objUserModel.UserId);
                if (data != null)
                {
                    data.EmployeeCategory = objUserModel.EmployeeCategory;
                    data.HiringDate = objUserModel.HiringDate;
                    data.ModifiedBy = objUserModel.ModifiedBy;
                    data.ModifiedDate = objUserModel.ModifiedDate; //.Value.ToClientTimeZoneinDateTime();
                    ObjUserRepository.SaveChanges();
                    return Result.Completed;
                }
                else
                    return Result.Failed;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Employee

        #region Inventory

        #region Manage Inventory

        /// <summary>SaveInventory
        /// 
        /// </summary>
        /// <param name="inventoryMasterModel"></param>
        /// <returns></returns>
        public Result SaveInventory(InventoryMasterModel inventoryMasterModel, DARModel ObjDARModel)
        {
            InventoryMaster _InventoryMaster = new InventoryMaster();
            objInventoryMasterRepository = new InventoryMasterRepository();
            Result objResult;
            try
            {
                if (CheckDuplicateInventory(inventoryMasterModel.ItemName, inventoryMasterModel.ItemCode, inventoryMasterModel.InventoryID))
                {
                    var ManagerList = _ICommonMethod.GetManagersBYLocationId(inventoryMasterModel.LocationId);
                    var AdminList = _ICommonMethod.GetAdminBYLocationId(inventoryMasterModel.LocationId);
                    if (inventoryMasterModel.InventoryID == 0)
                    {
                        AutoMapper.Mapper.CreateMap<InventoryMasterModel, InventoryMaster>();
                        _InventoryMaster = AutoMapper.Mapper.Map(inventoryMasterModel, _InventoryMaster);
                        objInventoryMasterRepository.Add(_InventoryMaster);
                        objInventoryMasterRepository.SaveChanges();
                        objResult = _ICommonMethod.SaveDAR(ObjDARModel);
                        if (_InventoryMaster.InventoryID > 0)
                        {
                            //Added By Bhushan Dod on 03/17/2015 for if any changes in stock notify to manager and admin                            
                            if (ManagerList.Count > 0)
                            {
                                foreach (var item in ManagerList)
                                {
                                    EmailHelper objEmailHelper = new EmailHelper();
                                    objEmailHelper.emailid = item.UserEmail;
                                    objEmailHelper.ManagerName = item.FirstName;
                                    objEmailHelper.ItemName = inventoryMasterModel.ItemName;
                                    objEmailHelper.ItemCode = inventoryMasterModel.ItemCode;
                                    objEmailHelper.ItemDescription = inventoryMasterModel.Description;
                                    objEmailHelper.ItemQuantity = inventoryMasterModel.Quantity;
                                    objEmailHelper.LocationName = inventoryMasterModel.Location;
                                    objEmailHelper.UserName = inventoryMasterModel.UserName;
                                    objEmailHelper.MailType = "INVENTORYSTOCKCREATE";
                                    objEmailHelper.SentBy = inventoryMasterModel.UserId;
                                    objEmailHelper.LocationID = inventoryMasterModel.LocationId;

                                    bool IsSent = objEmailHelper.SendEmailWithTemplate();
                                    if (IsSent == true)
                                    {
                                        objEmailog = new EmailLog();
                                        try
                                        {
                                            objEmailog.CreatedBy = inventoryMasterModel.UserId;
                                            objEmailog.CreatedDate = DateTime.UtcNow;
                                            objEmailog.DeletedBy = null;
                                            objEmailog.DeletedOn = null;
                                            objEmailog.LocationId = inventoryMasterModel.LocationId;
                                            objEmailog.ModifiedBy = null;
                                            objEmailog.ModifiedOn = null;
                                            objEmailog.SentBy = inventoryMasterModel.UserId;//Session UserId
                                            objEmailog.SentEmail = item.UserEmail;
                                            objEmailog.Subject = objEmailHelper.Subject;
                                            objEmailog.SentTo = item.UserId;//ManagerId

                                            objEmailLogRepository.SaveEmailLog(objEmailog);
                                        }
                                        catch (Exception)
                                        {
                                            throw;
                                        }
                                    }
                                }
                            }
                            if (AdminList.Count > 0)
                            {
                                foreach (var item in AdminList)
                                {
                                    EmailHelper objEmailHelper = new EmailHelper();
                                    objEmailHelper.emailid = item.UserEmail;
                                    objEmailHelper.ManagerName = item.FirstName;
                                    objEmailHelper.ItemName = inventoryMasterModel.ItemName;
                                    objEmailHelper.ItemCode = inventoryMasterModel.ItemCode;
                                    objEmailHelper.ItemDescription = inventoryMasterModel.Description;
                                    objEmailHelper.ItemQuantity = inventoryMasterModel.Quantity;
                                    objEmailHelper.LocationName = inventoryMasterModel.Location;
                                    objEmailHelper.UserName = inventoryMasterModel.UserName;
                                    objEmailHelper.MailType = "INVENTORYSTOCKCREATE";
                                    objEmailHelper.SentBy = inventoryMasterModel.UserId;
                                    objEmailHelper.LocationID = inventoryMasterModel.LocationId;

                                    bool IsSent = objEmailHelper.SendEmailWithTemplate();
                                    if (IsSent == true)
                                    {
                                        objEmailog = new EmailLog();
                                        try
                                        {
                                            objEmailog.CreatedBy = inventoryMasterModel.UserId;
                                            objEmailog.CreatedDate = DateTime.UtcNow;
                                            objEmailog.DeletedBy = null;
                                            objEmailog.DeletedOn = null;
                                            objEmailog.LocationId = inventoryMasterModel.LocationId;
                                            objEmailog.ModifiedBy = null;
                                            objEmailog.ModifiedOn = null;
                                            objEmailog.SentBy = inventoryMasterModel.UserId;//Session UserId
                                            objEmailog.SentEmail = item.UserEmail;
                                            objEmailog.Subject = objEmailHelper.Subject;
                                            objEmailog.SentTo = item.UserId;//ManagerId

                                            objEmailLogRepository.SaveEmailLog(objEmailog);
                                        }
                                        catch (Exception)
                                        {
                                            throw;
                                        }
                                    }
                                }
                            }
                            return Result.Completed;
                        }
                        else
                            return Result.Failed;
                    }
                    else
                    {
                        var data = objInventoryMasterRepository.GetSingleOrDefault(i => i.InventoryID == inventoryMasterModel.InventoryID);
                        if (data != null)
                        {
                            data.ItemName = inventoryMasterModel.ItemName;
                            data.ItemCode = inventoryMasterModel.ItemCode;
                            data.ItemType = inventoryMasterModel.ItemType;
                            data.Description = inventoryMasterModel.Description;
                            data.Quantity = inventoryMasterModel.Quantity;
                            objInventoryMasterRepository.SaveChanges();
                            objResult = _ICommonMethod.SaveDAR(ObjDARModel);
                            if (ManagerList.Count > 0)
                            {
                                foreach (var item in ManagerList)
                                {
                                    EmailHelper objEmailHelper = new EmailHelper();
                                    objEmailHelper.emailid = item.UserEmail;
                                    objEmailHelper.ManagerName = item.FirstName;
                                    objEmailHelper.ItemName = inventoryMasterModel.ItemName;
                                    objEmailHelper.ItemCode = inventoryMasterModel.ItemCode;
                                    objEmailHelper.ItemDescription = inventoryMasterModel.Description;
                                    objEmailHelper.ItemQuantity = inventoryMasterModel.Quantity;
                                    objEmailHelper.LocationName = inventoryMasterModel.Location;
                                    objEmailHelper.UserName = inventoryMasterModel.UserName;
                                    objEmailHelper.MailType = "INVENTORYSTOCKUPDATE";
                                    objEmailHelper.SentBy = inventoryMasterModel.UserId;
                                    objEmailHelper.LocationID = inventoryMasterModel.LocationId;

                                    bool IsSent = objEmailHelper.SendEmailWithTemplate();
                                    if (IsSent == true)
                                    {
                                        objEmailog = new EmailLog();
                                        try
                                        {
                                            objEmailog.CreatedBy = inventoryMasterModel.UserId;
                                            objEmailog.CreatedDate = DateTime.UtcNow;
                                            objEmailog.DeletedBy = null;
                                            objEmailog.DeletedOn = null;
                                            objEmailog.LocationId = inventoryMasterModel.LocationId;
                                            objEmailog.ModifiedBy = null;
                                            objEmailog.ModifiedOn = null;
                                            objEmailog.SentBy = inventoryMasterModel.UserId;//Session UserId
                                            objEmailog.SentEmail = item.UserEmail;
                                            objEmailog.Subject = objEmailHelper.Subject;
                                            objEmailog.SentTo = item.UserId;//ManagerId

                                            objEmailLogRepository.SaveEmailLog(objEmailog);
                                        }
                                        catch (Exception)
                                        {
                                            throw;
                                        }
                                    }
                                }
                            }
                            if (AdminList.Count > 0)
                            {
                                foreach (var item in AdminList)
                                {
                                    EmailHelper objEmailHelper = new EmailHelper();
                                    objEmailHelper.emailid = item.UserEmail;
                                    objEmailHelper.ManagerName = item.FirstName;
                                    objEmailHelper.ItemName = inventoryMasterModel.ItemName;
                                    objEmailHelper.ItemCode = inventoryMasterModel.ItemCode;
                                    objEmailHelper.ItemDescription = inventoryMasterModel.Description;
                                    objEmailHelper.ItemQuantity = inventoryMasterModel.Quantity;
                                    objEmailHelper.LocationName = inventoryMasterModel.Location;
                                    objEmailHelper.UserName = inventoryMasterModel.UserName;
                                    objEmailHelper.MailType = "INVENTORYSTOCKUPDATE";
                                    objEmailHelper.SentBy = inventoryMasterModel.UserId;
                                    objEmailHelper.LocationID = inventoryMasterModel.LocationId;

                                    bool IsSent = objEmailHelper.SendEmailWithTemplate();
                                    if (IsSent == true)
                                    {
                                        objEmailog = new EmailLog();
                                        try
                                        {
                                            objEmailog.CreatedBy = inventoryMasterModel.UserId;
                                            objEmailog.CreatedDate = DateTime.UtcNow;
                                            objEmailog.DeletedBy = null;
                                            objEmailog.DeletedOn = null;
                                            objEmailog.LocationId = inventoryMasterModel.LocationId;
                                            objEmailog.ModifiedBy = null;
                                            objEmailog.ModifiedOn = null;
                                            objEmailog.SentBy = inventoryMasterModel.UserId;//Session UserId
                                            objEmailog.SentEmail = item.UserEmail;
                                            objEmailog.Subject = objEmailHelper.Subject;
                                            objEmailog.SentTo = item.UserId;//ManagerId

                                            objEmailLogRepository.SaveEmailLog(objEmailog);
                                        }
                                        catch (Exception)
                                        {
                                            throw;
                                        }
                                    }
                                }
                            }
                            return Result.UpdatedSuccessfully;
                        }
                    }
                }
                return Result.DuplicateRecord;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>DeleteInventory
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result DeleteInventory(long id)
        {
            try
            {
                objInventoryMasterRepository = new InventoryMasterRepository();
                if (CheckIsInventoryAssigned(id))
                {
                    objInventoryMasterRepository = new InventoryMasterRepository();
                    if (CheckIsInventoryAssigned(id))
                    {
                        var data = objInventoryMasterRepository.GetSingleOrDefault(i => i.InventoryID == id);
                        if (data != null)
                        {
                            data.DeletedBy = 1;
                            data.DeletedOn = DateTime.UtcNow;
                            data.IsDeleted = true;
                            objInventoryMasterRepository.SaveChanges();
                            return Result.Delete;
                        }
                    }
                    else
                    { return Result.DuplicateRecord; }
                }
                return Result.Failed;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>ChkIsInventoryAssigned
        /// 
        /// </summary>
        /// <param name="Inventoryid"></param>
        /// <returns></returns>
        public bool CheckIsInventoryAssigned(long Inventoryid)
        {
            int count = objAssignInventoryRepository.GetAll(i => i.InventoryID == Inventoryid).Count();
            if (count > 0)
                return false;
            return true;
        }

        /// <summary>ChkDuplicateInventory
        /// 
        /// </summary>
        /// <param name="ItemName"></param>
        /// <param name="ItemCode"></param>
        /// <param name="InventoryID"></param>
        /// <returns></returns>
        public bool CheckDuplicateInventory(string ItemName, string ItemCode, long InventoryID)
        {
            objInventoryMasterRepository = new InventoryMasterRepository();
            int count = objInventoryMasterRepository.GetAll(i => i.ItemName == ItemName && i.ItemCode == ItemCode && i.InventoryID != InventoryID).Count();
            if (count > 0)
                return false;
            return true;
        }

        /// <summary>GetAllInventory
        /// 
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="OperationName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <param name="paramTotalRecords"></param>
        /// <returns></returns>
        public List<InventoryMasterModelList> GetAllInventory(int? projectId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, int? InventoryType, int? ItemOwn, ObjectParameter paramTotalRecords)
        {
            objInventoryMasterRepository = new InventoryMasterRepository();
            try
            {
                return objInventoryMasterRepository.GetAllInventory(projectId, operationName, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, InventoryType, ItemOwn, paramTotalRecords);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedDate>02/09/2015</CreatedDate>
        /// <Description>Get All Created Inventory list by LocationID</Description>
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<InventoryMasterModelList> GetAllInventoryExportToPDF(int? projectId, int? InventoryType, int? OwnershipType)
        {
            objInventoryMasterRepository = new InventoryMasterRepository();
            List<InventoryMasterModelList> listInventory = new List<InventoryMasterModelList>();

            try
            {
                //  var In = objInventoryMasterRepository.GetAllInventory();
                var InventoryList = objInventoryMasterRepository.GetAllInventoryForPDF(projectId, InventoryType, OwnershipType);

                foreach (var list in InventoryList)
                {
                    InventoryMasterModelList ObjInventoryMasterModelList = new InventoryMasterModelList();
                    ObjInventoryMasterModelList.AssignedToName = list.AssignedToName;
                    ObjInventoryMasterModelList.AssignedUserID = list.AssignedUserID;
                    ObjInventoryMasterModelList.AssignInventoryID = list.AssignInventoryID;
                    ObjInventoryMasterModelList.Description = list.Description;
                    ObjInventoryMasterModelList.InventoryID = list.InventoryID;
                    ObjInventoryMasterModelList.IssueDate = list.IssueDate.ToClientTimeZoneinDateTime();
                    ObjInventoryMasterModelList.IssuedBy = list.IssuedBy;
                    ObjInventoryMasterModelList.ItemCode = list.ItemCode;
                    ObjInventoryMasterModelList.ItemName = list.ItemName;
                    ObjInventoryMasterModelList.ItemType = list.ItemType;
                    ObjInventoryMasterModelList.LocationId = list.LocationId;
                    ObjInventoryMasterModelList.Quantity = list.Quantity;
                    if (list.ReturnDate != null)
                        ObjInventoryMasterModelList.ReturnDate = list.ReturnDate.Value.ToClientTimeZoneinDateTime();
                    ObjInventoryMasterModelList.ItemTypeName = list.CodeName;
                    // ObjInventoryMasterModelList.CreatedDate = list.CreatedOn.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    ObjInventoryMasterModelList.CreatedDate = list.CreatedOn.ToClientTimeZone(true);

                    listInventory.Add(ObjInventoryMasterModelList);
                }
                return listInventory;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// CreatedBy :Bhushan Dod
        /// CreatedDate :28-02-2015
        /// Description :Overload function of GetAllInventory
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="operationName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <param name="InventoryType"></param>
        /// <param name="ItemOwn"></param>
        /// <param name="paramTotalRecords"></param>
        /// <returns></returns>
        public List<InventoryMasterModelList> GetAllInventoryPDF(int projectId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, int InventoryType, int ItemOwn, ObjectParameter paramTotalRecords)
        {
            objInventoryMasterRepository = new InventoryMasterRepository();
            try
            {
                return objInventoryMasterRepository.GetAllInventory(projectId, operationName, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, InventoryType, ItemOwn, paramTotalRecords);
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion Manage Inventory

        #region Assigned Inventory
        public Result AssignedInventory(AssignInventoryModel assignInventoryModel)
        {

            objInventoryMasterRepository = new InventoryMasterRepository();
            //AutoMapper.Mapper.CreateMap<UserModel, UserRegistration>();
            //ObjManagerUser = AutoMapper.Mapper.Map(objUserModel, ObjManagerUser);
            //ObjUserRepository.Add(ObjManagerUser);
            //ObjUserRepository.SaveChanges();
            AssignInventory _AssignInventory = new AssignInventory();
            try
            { // Commented By Bhushan Dod on 18/02/2015 for separation of 
                //if (ChkInventoryAsgned(_AssignInventoryModel.InventoryID))
                //{
                //    // Commented By Bhushan Dod on 09/02/2015 for manually assigned property.
                //AutoMapper.Mapper.CreateMap<AssignInventoryModel, AssignInventory>();
                //_AssignInventory = AutoMapper.Mapper.Map(_AssignInventoryModel, _AssignInventory);
                //objAssignInventoryRepository.Add(_AssignInventory);
                //if (_AssignInventory.AssignInventoryID > 0)
                //    return Result.Completed;
                //else
                //    return Result.Failed;
                // Added By Bhushan Dod on 18/03/2015 for send email if assign any quantity to user.
                objUserManager = new UserManager();
                var AssignedDetails = objUserManager.GetUserDetailsById(assignInventoryModel.AssignedUserID);
                var ManagerList = _ICommonMethod.GetManagersBYLocationId(assignInventoryModel.LocationId);
                var AdminList = _ICommonMethod.GetAdminBYLocationId(assignInventoryModel.LocationId);

                _AssignInventory.AssignedUserID = assignInventoryModel.AssignedUserID;
                _AssignInventory.AssignInventoryID = assignInventoryModel.AssignInventoryID;
                _AssignInventory.CreatedBy = assignInventoryModel.CreatedBy;
                _AssignInventory.CreatedOn = Convert.ToDateTime(DateTime.UtcNow, CultureInfo.InvariantCulture).ToUniversalTime();//_AssignInventoryModel.CreatedOn;
                _AssignInventory.DeletedBy = assignInventoryModel.DeletedBy;
                _AssignInventory.DeletedOn = assignInventoryModel.DeletedOn; //.Value.ToClientTimeZoneinDateTime();
                _AssignInventory.InventoryID = assignInventoryModel.InventoryID;
                _AssignInventory.IsDeleted = assignInventoryModel.IsDeleted;
                _AssignInventory.IssuedBy = assignInventoryModel.IssuedBy;
                _AssignInventory.IssueDate = Convert.ToDateTime(assignInventoryModel.IssueDate, CultureInfo.InvariantCulture).ToUniversalTime();
                _AssignInventory.ModifiedBy = assignInventoryModel.ModifiedBy;
                _AssignInventory.ModifiedOn = assignInventoryModel.ModifiedOn;//.Value.ToClientTimeZoneinDateTime();
                //Added By Bhushan Dod on 11/02/2015 for insert quantity into AssignedInventory 
                _AssignInventory.AssginedQuantity = assignInventoryModel.Quantity;
                objAssignInventoryRepository.Add(_AssignInventory);

                if (_AssignInventory.AssignInventoryID > 0)
                {
                    //Added By Bhushan Dod on 09/02/2015 for Assigned inventory get subtracted from current. 
                    var InventoryId = objInventoryMasterRepository.GetSingleOrDefault(t => t.InventoryID == assignInventoryModel.InventoryID);
                    InventoryId.Quantity = (InventoryId.Quantity - assignInventoryModel.Quantity);
                    objInventoryMasterRepository.SaveChanges();
                    // Added By Bhushan Dod on 18/03/2015 for send email if assign any quantity to user.
                    if (ManagerList.Count > 0)
                    {
                        foreach (var item in ManagerList)
                        {
                            EmailHelper objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = item.UserEmail;
                            objEmailHelper.ManagerName = item.FirstName;
                            //objEmailHelper.ItemName = assignInventoryModel.ItemName;
                            objEmailHelper.ItemCode = assignInventoryModel.InventoryID.ToString();
                            objEmailHelper.AssignedTo = (AssignedDetails.FirstName + " " + AssignedDetails.LastName).ToString();
                            objEmailHelper.ItemQuantity = assignInventoryModel.Quantity;
                            objEmailHelper.LocationName = assignInventoryModel.Location;
                            objEmailHelper.UserName = assignInventoryModel.UserName;
                            objEmailHelper.MailType = "INVENTORYSTOCKASSIGNED";
                            objEmailHelper.SentBy = assignInventoryModel.UserId;
                            objEmailHelper.LocationID = assignInventoryModel.LocationId;

                            bool IsSent = objEmailHelper.SendEmailWithTemplate();
                            if (IsSent == true)
                            {
                                objEmailog = new EmailLog();
                                try
                                {
                                    objEmailog.CreatedBy = assignInventoryModel.UserId;
                                    objEmailog.CreatedDate = DateTime.UtcNow;
                                    objEmailog.DeletedBy = null;
                                    objEmailog.DeletedOn = null;
                                    objEmailog.LocationId = assignInventoryModel.LocationId;
                                    objEmailog.ModifiedBy = null;
                                    objEmailog.ModifiedOn = null;
                                    objEmailog.SentBy = assignInventoryModel.UserId;//Session UserId
                                    objEmailog.SentEmail = item.UserEmail;
                                    objEmailog.Subject = objEmailHelper.Subject;
                                    objEmailog.SentTo = item.UserId;//ManagerId

                                    objEmailLogRepository.SaveEmailLog(objEmailog);
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }
                        }
                    }
                    if (AdminList.Count > 0)
                    {
                        foreach (var item in AdminList)
                        {
                            EmailHelper objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = item.UserEmail;
                            objEmailHelper.ManagerName = item.FirstName;
                            //objEmailHelper.ItemName = assignInventoryModel.ItemName;
                            objEmailHelper.ItemCode = assignInventoryModel.InventoryID.ToString();
                            objEmailHelper.AssignedTo = (AssignedDetails.FirstName + " " + AssignedDetails.LastName).ToString();
                            objEmailHelper.ItemQuantity = assignInventoryModel.Quantity;
                            objEmailHelper.LocationName = assignInventoryModel.Location;
                            objEmailHelper.UserName = assignInventoryModel.UserName;
                            objEmailHelper.MailType = "INVENTORYSTOCKASSIGNED";
                            objEmailHelper.SentBy = assignInventoryModel.UserId;
                            objEmailHelper.LocationID = assignInventoryModel.LocationId;

                            bool IsSent = objEmailHelper.SendEmailWithTemplate();
                            if (IsSent == true)
                            {
                                objEmailog = new EmailLog();
                                try
                                {
                                    objEmailog.CreatedBy = assignInventoryModel.UserId;
                                    objEmailog.CreatedDate = DateTime.UtcNow;
                                    objEmailog.DeletedBy = null;
                                    objEmailog.DeletedOn = null;
                                    objEmailog.LocationId = assignInventoryModel.LocationId;
                                    objEmailog.ModifiedBy = null;
                                    objEmailog.ModifiedOn = null;
                                    objEmailog.SentBy = assignInventoryModel.UserId;//Session UserId
                                    objEmailog.SentEmail = item.UserEmail;
                                    objEmailog.Subject = objEmailHelper.Subject;
                                    objEmailog.SentTo = item.UserId;//ManagerId

                                    objEmailLogRepository.SaveEmailLog(objEmailog);
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }
                        }
                    }
                    return Result.Completed;
                }
                else
                    return Result.Failed;
                //}
                return Result.DuplicateRecord;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ChkInventoryAsgned(long inventoryId)
        {
            int count = objAssignInventoryRepository.GetAll(i => i.InventoryID == inventoryId && i.ReturnDate == null).Count();
            if (count > 0)
                return false;
            return true;

        }

        #endregion Assigned Inventory

        #endregion Inventory

        #region WorkRequest

        ///// <summary>SaveWorkRequest
        ///// 
        ///// </summary>
        ///// <param name="objWorkRequestModel"></param>
        ///// <returns></returns>
        //public Result SaveWorkRequest(WorkRequestModel objWorkRequestModel)
        //{
        //    try
        //    {
        //        if (ChkDuplicateWorkRequest(objWorkRequestModel))
        //        {
        //            WorkRequest _WorkRequest = new WorkRequest();
        //            objWorkRequestRepository = new WorkRequestRepository();
        //            AutoMapper.Mapper.CreateMap<WorkRequestModel, WorkRequest>();
        //            _WorkRequest = AutoMapper.Mapper.Map(objWorkRequestModel, _WorkRequest);
        //            objWorkRequestRepository.Add(_WorkRequest);
        //            objWorkRequestRepository.SaveChanges();
        //            if (_WorkRequest.WorkRequestID > 0)
        //                return Result.Completed;
        //            else
        //                return Result.Failed;
        //        }
        //        else
        //        {
        //            return Result.DuplicateRecord;
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>GetAllWorkRequest
        ///// 
        ///// </summary>
        ///// <param name="ProjectId"></param>
        ///// <param name="UserID"></param>
        ///// <param name="OperationName"></param>
        ///// <param name="pageIndex"></param>
        ///// <param name="numberOfRows"></param>
        ///// <param name="sortColumnName"></param>
        ///// <param name="sortOrderBy"></param>
        ///// <param name="textSearch"></param>
        ///// <param name="paramTotalRecords"></param>
        ///// <returns></returns>
        //public List<WorkRequestModelList> GetAllWorkRequest(long? projectId, long? userID, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter paramTotalRecords)
        //{
        //    objWorkRequestRepository = new WorkRequestRepository();
        //    try
        //    {
        //        return objWorkRequestRepository.GetAllWorkRequest(projectId, userID, operationName, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, paramTotalRecords);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        ///// <summary>SaveWorkOrder
        ///// 
        ///// </summary>
        ///// <param name="objWorkOrderModel"></param>
        ///// <returns></returns>
        //public Result SaveWorkOrder(WorkOrderModel objWorkOrderModel)
        //{
        //    try
        //    {
        //        objWorkOrderRepository = new WorkOrderRepository();
        //        objWorkRequestRepository = new WorkRequestRepository();
        //        WorkOrder _WorkOrder = new WorkOrder();
        //        string TaskStatus = Convert.ToString(WorkOrderStatus.Pending);
        //        if (objWorkOrderModel.WorkOrderID == 0)
        //        {
        //            objWorkOrderModel.TaskStatus = objGlobalCodesRepository.GetSingleOrDefault(g => g.CodeName == TaskStatus).GlobalCodeId;
        //            AutoMapper.Mapper.CreateMap<WorkOrderModel, WorkOrder>();
        //            _WorkOrder = AutoMapper.Mapper.Map(objWorkOrderModel, _WorkOrder);
        //            objWorkOrderRepository.Add(_WorkOrder);
        //            objWorkOrderRepository.SaveChanges();

        //            var data = objWorkRequestRepository.GetSingleOrDefault(s => s.WorkRequestID == objWorkOrderModel.WorkRequestID);
        //            if (data != null)
        //            {
        //                data.status = Convert.ToString(WorkRequestStatus.Assigned);
        //                objWorkRequestRepository.SaveChanges();
        //            }
        //            return Result.Completed;
        //        }
        //        return Result.Failed;

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>ChkDuplicateWorkRequest
        ///// 
        ///// </summary>
        ///// <param name="objWorkRequestModel"></param>
        ///// <returns></returns>
        //public bool ChkDuplicateWorkRequest(WorkRequestModel objWorkRequestModel)
        //{
        //    objWorkRequestRepository = new WorkRequestRepository();
        //    int count = objWorkRequestRepository.GetAll(w => w.AssetID == objWorkRequestModel.AssetID && EntityFunctions.TruncateTime(w.CreatedDate) == EntityFunctions.TruncateTime(DateTime.Now) && w.ProjectId == objWorkRequestModel.ProjectId).Count();
        //    if (count > 0)
        //        return false;
        //    return true;
        //}

        ///// <summary>DeleteWorkRequest
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public Result DeleteWorkRequest(long id)
        //{
        //    objWorkRequestRepository = new WorkRequestRepository();
        //    try
        //    {
        //        var data = objWorkRequestRepository.GetSingleOrDefault(w => w.WorkRequestID == id);
        //        if (data != null)
        //        {
        //            data.IsDeleted = true;
        //            data.DeletedDate = DateTime.Now;
        //            objWorkRequestRepository.SaveChanges();
        //            return Result.Delete;
        //        }
        //        else
        //        {
        //            return Result.Failed;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        #endregion WorkRequest

        #region Rule
        public Result SaveRule(RuleMasterModel ruleMasterModel, DARModel objDAR)
        {
            objRuleRepository = new RuleRepository();
            objLocationRuleMappingRepository = new LocationRuleMappingRepository();
            RuleMaster _RuleMaster = new RuleMaster();
            LocationRuleMapping _LocationRuleMapping = new LocationRuleMapping();
            LocationRuleMappingModel objLocationRuleMappingModel = new LocationRuleMappingModel();
            Result objReuslt;
            try
            {

                if (ruleMasterModel.RuleID == 0)
                {
                    if (CheckDuplicateRule(ruleMasterModel.RuleName, ruleMasterModel.RuleID))
                    {
                        //For Maintaining voilation charges which is store in mapping but 0 in RuleList with location id.
                        var tempVoilation = ruleMasterModel.VoilationCharges;
                        ruleMasterModel.VoilationCharges = 0;

                        AutoMapper.Mapper.CreateMap<RuleMasterModel, RuleMaster>();
                        _RuleMaster = AutoMapper.Mapper.Map(ruleMasterModel, _RuleMaster);
                        objRuleRepository.Add(_RuleMaster);

                        //Added By Bhushan Dod on 14-03-2015 for mapping rule voilation
                        objLocationRuleMappingModel.LocationID = _RuleMaster.ProjectID;
                        objLocationRuleMappingModel.RuleID = _RuleMaster.RuleID;
                        objLocationRuleMappingModel.VoilationCharges = tempVoilation;
                        objLocationRuleMappingModel.CreatedBy = _RuleMaster.CreatedBy;
                        objLocationRuleMappingModel.CreatedDate = _RuleMaster.CreatedDate;//.ToClientTimeZoneinDateTime();
                        objLocationRuleMappingModel.IsActive = _RuleMaster.IsActive;
                        objLocationRuleMappingModel.IsDeleted = _RuleMaster.IsDeleted;

                        AutoMapper.Mapper.CreateMap<LocationRuleMappingModel, LocationRuleMapping>();
                        _LocationRuleMapping = AutoMapper.Mapper.Map(objLocationRuleMappingModel, _LocationRuleMapping);
                        objLocationRuleMappingRepository.Add(_LocationRuleMapping);

                        objReuslt = _ICommonMethod.SaveDAR(objDAR);

                        if (_RuleMaster.RuleID > 0)
                            return Result.Completed;
                        else
                            return Result.Failed;
                    }
                }
                else
                {
                    var data = objRuleRepository.GetSingleOrDefault(i => i.RuleID == ruleMasterModel.RuleID && (i.ProjectID == ruleMasterModel.ProjectID || i.ProjectID == 0));
                    var dataLocationRuleMapping = objLocationRuleMappingRepository.GetSingleOrDefault(i => i.RuleID == ruleMasterModel.RuleID && i.LocationID == ruleMasterModel.ProjectID);

                    if (dataLocationRuleMapping != null)
                    {
                        data.RuleName = ruleMasterModel.RuleName;
                        data.Description = ruleMasterModel.Description;
                        objRuleRepository.SaveChanges();
                        if (dataLocationRuleMapping.RuleID == ruleMasterModel.RuleID && dataLocationRuleMapping.LocationID == ruleMasterModel.ProjectID)
                        {
                            //Added By Bhushan Dod on 14-03-2015 for update mapping rule voilation
                            dataLocationRuleMapping.VoilationCharges = ruleMasterModel.VoilationCharges;
                            dataLocationRuleMapping.IsActive = ruleMasterModel.IsActive;
                            dataLocationRuleMapping.ModifiedBy = ruleMasterModel.ModifiedBy;
                            dataLocationRuleMapping.ModifiedDate = ruleMasterModel.ModifiedDate; //.Value.ToClientTimeZoneinDateTime();
                            dataLocationRuleMapping.IsDeleted = ruleMasterModel.IsDeleted;
                            objLocationRuleMappingRepository.SaveChanges();
                        }
                        else
                        {
                            objLocationRuleMappingModel.CreatedDate = DateTime.UtcNow;
                            objLocationRuleMappingModel.CreatedBy = ruleMasterModel.CreatedBy;
                            objLocationRuleMappingModel.IsActive = ruleMasterModel.IsActive;
                            objLocationRuleMappingModel.LocationID = ruleMasterModel.ProjectID;
                            objLocationRuleMappingModel.RuleID = ruleMasterModel.RuleID;
                            objLocationRuleMappingModel.VoilationCharges = ruleMasterModel.VoilationCharges;

                            AutoMapper.Mapper.CreateMap<LocationRuleMappingModel, LocationRuleMapping>();
                            _LocationRuleMapping = AutoMapper.Mapper.Map(objLocationRuleMappingModel, _LocationRuleMapping);
                            objLocationRuleMappingRepository.Add(_LocationRuleMapping);
                        }

                        objReuslt = _ICommonMethod.SaveDAR(objDAR);
                        return Result.UpdatedSuccessfully;
                    }


                }
                return Result.DuplicateRecord;
            }
            catch (Exception)
            {
                throw;
            }
            return Result.Completed;
        }
        public bool CheckDuplicateRule(string ruleName, long ruleId)
        {
            objRuleRepository = new RuleRepository();
            int Count = objRuleRepository.GetAll(r => r.RuleName == ruleName && r.RuleID != ruleId).Count();
            if (Count > 0)
                return false;
            return true;
        }

        public List<RuleMasterModelList> GetAllRules(long? ProjectID, string OperationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter paramTotalRecords)
        {
            objRuleRepository = new RuleRepository();
            try
            {
                return objRuleRepository.GetAllRules(ProjectID, OperationName, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, paramTotalRecords);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete the Rule.
        /// CreatedBy   :   Roshan Rahood
        /// CreatedOn   :   Feb 10 2015
        /// </summary>
        /// <param name="ruleId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public Result DeleteRule(long ruleId, long loggedInUserId, string location, DARModel objDAR)
        {
            Result result;
            try
            {
                if (ruleId > 0)
                {
                    //if (ChkVendorMappedWithRegisteredVehicle(VendorID))
                    if (true)
                    {
                        objRuleRepository = new RuleRepository();
                        var data = objRuleRepository.GetSingleOrDefault(v => v.RuleID == ruleId && v.IsDeleted == false);
                        data.IsDeleted = true;
                        data.DeletedBy = loggedInUserId;
                        data.DeletedDate = DateTime.UtcNow;
                        objRuleRepository.Update(data);

                        objDAR.ActivityDetails = DarMessage.DeleteRuleDar(data.RuleName, location);
                        #region Save DAR
                        result = _ICommonMethod.SaveDAR(objDAR);
                        #endregion Save DAR

                        return Result.Delete;
                    }
                    else
                    { return Result.Failed; }
                }
                else { return Result.DoesNotExist; }
                return Result.Delete;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Edit the Rule.
        /// CreatedBy   :   Roshan Rahood
        /// CreatedOn   :   Apr 10 2015
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public RuleMasterModel EditRule(long ruleId, long locationId)
        {
            RuleMasterModel objRuleMasterModel;
            workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();
            try
            {
                objRuleMasterModel = new RuleMasterModel();
                objRuleRepository = new RuleRepository();
                if (ruleId > 0)
                {

                    ////////objRuleMasterModel = _workorderEMSEntities.RuleMasters.Join(_workorderEMSEntities.LocationRuleMappings, rm => rm.RuleID, lrm => lrm.RuleID,
                    ////////    (rm, lrm) => new { rm, lrm })
                    ////////    .Where(x => x.lrm.LocationID == locationId && x.rm.RuleID == ruleId)
                    ////////    .Select(z => new RuleMasterModel()
                    ////////    {
                    ////////        RuleID = z.rm.RuleID,
                    ////////        RuleName = z.rm.RuleName,
                    ////////        Description = z.rm.Description,
                    ////////        VoilationCharges = z.lrm.VoilationCharges,
                    ////////        IsActive = z.rm.IsActive,
                    ////////    }).SingleOrDefault();

                    //objRuleMasterModel = _workorderEMSEntities.RuleMasters.Join(_workorderEMSEntities.LocationRuleMappings, rm => rm.RuleID, lrm => lrm.RuleID,
                    //    (rm, lrm) => new { rm, lrm })
                    //    .Where(x => x.lrm.LocationID == locationId && x.rm.RuleID == ruleId)
                    //    .Select(z => new RuleMasterModel()
                    //    {
                    //        RuleID = z.rm.RuleID,
                    //        RuleName = z.rm.RuleName,
                    //        Description = z.rm.Description,
                    //        VoilationCharges = z.lrm.VoilationCharges,
                    //        IsActive = z.rm.IsActive,
                    //    }).SingleOrDefault();


                    //using (workorderEMSEntities Context = new workorderEMSEntities())
                    //{
                    //    //objRuleMasterModel = (from o in Context.RuleMasters
                    //    //                      join l in Context.LocationRuleMappings
                    //    //                      on o.RuleID equals l.RuleID
                    //    //                      into tempResult
                    //    //                      from subset in tempResult.DefaultIfEmpty()
                    //    //                      select (new RuleMasterModel(){
                    //    //                          o.RuleID,
                    //    //                          o.RuleName,
                    //    //                          o.Description,
                    //    //                          subset.VoilationCharges,
                    //    //                          o.IsActive
                    //    //                      })).FirstOrDefault();

                    //    objRuleMasterModel = (from o in Context.RuleMasters
                    //                          join l in Context.LocationRuleMappings
                    //                          on o.RuleID equals l.RuleID 
                    //                          into tempResult
                    //                          from subset in tempResult.DefaultIfEmpty()
                    //                          where o.RuleID == ruleId && (subset.LocationID == locationId )
                    //                          select new
                    //                           RuleMasterModel()
                    //                           {
                    //                               RuleID = o.RuleID,
                    //                               RuleName = o.RuleName,
                    //                               Description = o.Description,
                    //                               VoilationCharges = subset.VoilationCharges,
                    //                               IsActive = o.IsActive
                    //                           }).FirstOrDefault();





                    //}





                    //RuleMaster objRuleMaster = objRuleRepository. 
                    //    GetSingleOrDefault(q => q.RuleID == ruleId && q.IsDeleted == false);

                    //if (objRuleMaster != null)
                    //{
                    //    objRuleMasterModel.RuleID = Convert.ToInt32(ruleId);
                    //    objRuleMasterModel.RuleName = objRuleMaster.RuleName;
                    //    objRuleMasterModel.Description = objRuleMaster.Description;
                    //    objRuleMasterModel.VoilationCharges = Convert.ToDecimal(objRuleMaster.VoilationCharges);
                    //    objRuleMasterModel.IsActive = objRuleMaster.IsActive;
                    //}

                    objRuleMasterModel = objRuleRepository.GetRuleById(locationId, ruleId);

                }
                return objRuleMasterModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Manage Asset
        //public Result SaveAsset(AssetMasterModel _AssetMasterModel)
        //{
        //    objAssetMasterRepository = new AssetMasterRepository();
        //    AssetMaster _AssetMaster = new AssetMaster();
        //    try
        //    {
        //        if (chkDuplicateAsset(_AssetMasterModel.AssetName, _AssetMasterModel.AssetID, _AssetMasterModel.ProjectID, _AssetMasterModel.AssetMasterID))
        //        {
        //            if (_AssetMasterModel.AssetMasterID == 0)
        //            {
        //                AutoMapper.Mapper.CreateMap<AssetMasterModel, AssetMaster>();
        //                _AssetMaster = AutoMapper.Mapper.Map(_AssetMasterModel, _AssetMaster);
        //                objAssetMasterRepository.Add(_AssetMaster);
        //                objAssetMasterRepository.SaveChanges();
        //                if (_AssetMaster.AssetMasterID > 0)
        //                    return Result.Completed;
        //                else
        //                    return Result.Failed;
        //            }
        //            //else
        //            //{
        //            //    var data = objInventoryMasterRepository.GetSingleOrDefault(i => i.InventoryID == _InventoryMasterModel.InventoryID);
        //            //    if (data != null)
        //            //    {
        //            //        data.ItemName = _InventoryMasterModel.ItemName;
        //            //        data.ItemCode = _InventoryMasterModel.ItemCode;
        //            //        data.ItemType = _InventoryMasterModel.ItemType;
        //            //        data.Description = _InventoryMasterModel.Description;
        //            //        data.Quantity = _InventoryMasterModel.Quantity;
        //            //        objInventoryMasterRepository.SaveChanges();
        //            //        return Result.UpdatedSuccessfully;
        //            //    }
        //            //}
        //        }
        //        else
        //            return Result.DuplicateRecord;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return Result.Completed;
        //}
        //public bool chkDuplicateAsset(string AssetName, string AssetID, long? ProjectId, long AssetMasterID)
        //{
        //    objAssetMasterRepository = new AssetMasterRepository();

        //    int count = objAssetMasterRepository.GetAll(a => a.AssetName == AssetName && a.AssetID == AssetID && a.ProjectID != ProjectId && a.AssetMasterID != AssetMasterID).Count();
        //    if (count > 0)
        //        return false;
        //    return true;
        //}

        #endregion

        #region Insurance Master

        /// <summary>GetAllInsuranceCompany
        /// CreatedFor      :   Get All Insurance Company
        /// CreatedBy       :   Nagendra Upwanshi
        /// CreatedOn       :   Oct-14-2014
        /// </summary>
        /// <returns></returns>
        public InsuranceModel GetAllInsuranceCompany()
        {
            try
            {
                InsuranceModel ObjInsuranceModel = new InsuranceModel();

                InsuranceRepository ObjInsuranceRepository = new InsuranceRepository();
                ObjInsuranceModel.InsuranceList = ObjInsuranceRepository.GetAll(i => i.IsDeleted == false).Select(im => new InsuranceModel()
                {
                    Description = im.Description,
                    InsuranceCompany = im.InsuranceCompany,
                    InsuranceId = im.InsuranceId,
                }).ToList();
                return ObjInsuranceModel;
            }
            catch (Exception)
            { throw; }
        }

        #endregion Insurance Master

        #region new code for vehicle

        #endregion
        /// <summary>
        /// TO GET THE COUNTS FOR THE MANAGER
        /// </summary>
        /// <CreatedBy>Mano Jaswal</CreatedBy>
        /// <CretaedDate>2015-2-25</CretaedDate>
        /// <param name="LoginUserType"></param>
        /// <param name="LocationID"></param>
        /// <returns></returns>
        public long GetTotalManagerCount(string LoginUserType, long LocationID, long iUserID)
        {
            ManagerRepositroy obj_ManagerRepositroy = new ManagerRepositroy();
            return obj_ManagerRepositroy.GetTotalCountOfUsers(LoginUserType, LocationID, iUserID).Count();

        }
        /// <summary>
        /// TO GET THE COUNTS FOR THE MANAGER
        /// </summary>
        /// <CreatedBy>Mano Jaswal</CreatedBy>
        /// <CretaedDate>2015-3-12</CretaedDate>
        /// <param name="LoginUserType"></param>
        /// <param name="LocationID"></param>
        /// <returns></returns>
        public List<ManagerModel> GetUserByManager(string LoginUserType, long LocationID, long iUserID)
        {
            ManagerRepositroy obj_ManagerRepositroy = new ManagerRepositroy();
            return obj_ManagerRepositroy.GetTotalCountOfUsers(LoginUserType, LocationID, iUserID).Select(x => new ManagerModel()
            {
                UserEmail = x.UserEmail,
                FirstName = x.FirstName,
                LastName = x.LastName,
                UserID = x.UserID,
            }).ToList();

        }

        /// <summary>
        /// To edit the inventory details.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InventoryMasterModel EditInventory(long id)
        {
            try
            {
                objInventoryMasterRepository = new InventoryMasterRepository();
                InventoryMaster _InventoryMaster = objInventoryMasterRepository.GetSingleOrDefault(q => q.InventoryID == id && q.IsDeleted == false);
                InventoryMasterModel objInventoryMasterModel = new InventoryMasterModel();
                if (_InventoryMaster != null)
                {
                    objInventoryMasterModel.InventoryID = Convert.ToInt32(id);
                    objInventoryMasterModel.ItemName = _InventoryMaster.ItemName;
                    objInventoryMasterModel.ItemCode = _InventoryMaster.ItemCode;
                    objInventoryMasterModel.ItemType = Convert.ToInt32(_InventoryMaster.ItemType);
                    objInventoryMasterModel.Description = _InventoryMaster.Description;
                    objInventoryMasterModel.Quantity = Convert.ToInt64(_InventoryMaster.Quantity);
                    objInventoryMasterModel.ItemOwnership = Convert.ToInt32(_InventoryMaster.ItemOwnership);

                    objInventoryMasterModel.itType = Convert.ToInt32(_InventoryMaster.ItemType);
                }
                return objInventoryMasterModel;
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Created by :Bhushan Dod 
        /// Created Date :05/26/2015
        /// Alert on screen on admin and manager if any employee idle for 30 min
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic EmployeeIdleStatus(long locationId, long userId)
        {
            dynamic result = null;

            try
            {
                objLoginLogRepository = new LoginLogRepository();
                List<EmailToManagerModel> objEmailReturn = new List<EmailToManagerModel>();

                result = objLoginLogRepository.IdleStatusOfEmployee(userId, locationId);
                if (result.Response != 0)
                {   //send email and push notification to manager 
                    objEmailReturn = objEmailLogRepository.SendEmailToManagerForItemMissing(locationId, userId);//function return data according to location and username
                    if (objEmailReturn.Count() > 0)
                    {
                        foreach (var item in objEmailReturn)
                        {
                            var time = item.IdleTimeLimit;
                            EmailHelper objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = item.ManagerEmail;
                            objEmailHelper.ManagerName = item.ManagerName;

                            //if (item.IdleTimeLimit.Value.ToString("tt") == "AM" && item.IdleTimeLimit.Value.ToString("hh") == "12")
                            //{
                            //    objEmailHelper.ProblemDesc =  "00:"+item.IdleTimeLimit.Value.ToString("mm")+ " (hr:mm)";
                            //}
                            //else{
                            //    objEmailHelper.ProblemDesc = item.IdleTimeLimit.Value.ToString("hh:mm") + " (hr:mm)";
                            //}
                            //"30 Min";// this is hardcoded time for all employee
                            objEmailHelper.ProblemDesc = item.IdleTimeLimit.Value.ToString("HH:mm") + " (hr:mm)";
                            objEmailHelper.LocationName = item.LocationName;
                            objEmailHelper.UserName = item.UserName;
                            objEmailHelper.MailType = "EMPLOYEEIDLE";
                            objEmailHelper.SentBy = item.RequestBy;
                            objEmailHelper.LocationID = item.LocationID;

                            bool IsSent = objEmailHelper.SendEmailWithTemplate();
                            if (IsSent == true)
                            {
                                objEmailog = new EmailLog();
                                try
                                {
                                    objEmailog.CreatedBy = item.RequestBy;
                                    objEmailog.CreatedDate = DateTime.UtcNow;
                                    objEmailog.DeletedBy = null;
                                    objEmailog.DeletedOn = null;
                                    objEmailog.LocationId = item.LocationID;
                                    objEmailog.ModifiedBy = null;
                                    objEmailog.ModifiedOn = null;
                                    objEmailog.SentBy = item.RequestBy;
                                    objEmailog.SentEmail = item.ManagerEmail;
                                    objEmailog.Subject = objEmailHelper.Subject;
                                    objEmailog.SentTo = item.ManagerUserId;

                                    objEmailLogRepository.SaveEmailLog(objEmailog);
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }
                            if (item.DeviceId != null && item.DeviceId.Trim() != "")
                            {
                                message = PushNotificationMessages.EmployeeIdle(item.LocationName, item.UserName);
                                PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public dynamic EmployeeIdleStatus(string id)", "Exception while fetching employee idle status", locationId);
            }
            return result;
        }

        /// <summary>
        /// Created by :Bhushan Dod 
        /// Created Date :04/29/2015
        /// Alert on screen on client and manager if any infraction submitted 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<dynamic> IdleEmployeeAlert(long UserId)
        {
            dynamic abc = null;
            LoginManager objLoginManager = new LoginManager();
            try
            {
                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    List<long> objL = new List<long>();
                    var list = objLoginManager.GetManageAdminUserLocationList(UserId);
                    foreach (var t in list)
                    {
                        objL.Add(t.LocationId);
                    }

                    abc = await (from o in Context.TrackEmployeeStatus
                                .Join(Context.UserRegistrations, q => q.UserID, u => u.UserId, (q, u) => new { q, u })
                                .Where(x => objL.Contains(x.q.LocationId) && x.u.IsDeleted == false)
                                .OrderByDescending(s => s.q.TrackId)
                                 select new
                                 {
                                     o.q.LocationId,
                                     o.q.TrackId,
                                     o.u.FirstName,
                                     o.u.LastName,
                                     o.u.EmployeeID,
                                     o.q.UserID
                                 }).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public dynamic IdleEmployeeAlert(long UserId)", "Exception while fetching latest record for TRACKEMPLOYEESTATUS ", UserId);
            }
            return abc;
        }

        /// <summary>
        /// Created By :Bhushan Dod
        /// Created Date : 01/06/2015
        /// Description : For change the idle time limit by Manager
        /// </summary>
        /// <param name="id"></param>
        /// <param name="time"></param>
        /// <param name="managerId"></param>
        /// <returns></returns>
        public bool UpdateEmployeeIdleTime(string id, string time, long managerId)
        {
            bool result = false;
            try
            {
                long _userId = 0;
                if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(time))
                {
                    long.TryParse(id, out _userId);
                    DateTime? IdleTime;
                    IdleTime = Convert.ToDateTime(time);
                    ObjUserRepository = new UserRepository();
                    var data = ObjUserRepository.GetSingleOrDefault(v => v.UserId == _userId && v.IsDeleted == false);
                    if (data != null)
                    {
                        data.ModifiedBy = managerId;
                        data.ModifiedDate = DateTime.UtcNow;
                        data.IdleTimeLimit = IdleTime; //.Value.ToClientTimeZoneinDateTime();
                        ObjUserRepository.Update(data);

                        result = true;
                    }
                }
                else { result = false; }
            }
            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "bool UpdateEmployeeIdleTime(string id, string time,long managerId)", "While Update Emp idle limit", managerId);
                throw;
            }
            return result;
        }

        public ServiceResponseModel<string> UpdateEmployeeTime(ServiceWorkStatusModel obj)
        {
            ObjUserRepository = new UserRepository();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == obj.ServiceAuthKey && x.IsDeleted == false);

                if (authuser != null && authuser.UserId > 0)
                {
                    DateTime? IdleTime;
                    IdleTime = Convert.ToDateTime(obj.StartTime);
                    var data = ObjUserRepository.GetSingleOrDefault(v => v.UserId == obj.UserId && v.IsDeleted == false);
                    if (data != null)
                    {
                        data.ModifiedBy = obj.ManagerId;
                        data.ModifiedDate = DateTime.UtcNow;
                        data.IdleTimeLimit = IdleTime; //.Value.ToClientTimeZoneinDateTime();
                        ObjUserRepository.Update(data);

                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Data = obj.StartTime;
                        ObjServiceResponseModel.Message = CommonMessage.SaveSuccessMessage();
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                    }
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.InvalidSessionResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }

            }
            catch (Exception)
            { throw; }

            return ObjServiceResponseModel;
        }
        /// <summary>
        /// Created by :Bhushan Dod 
        /// Created Date :06/01/2015
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic GetUpdatedEmployeeTimeLimit(long id)
        {
            dynamic abc = null;

            try
            {
                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    var result = (from o in Context.UserRegistrations.Where(u => u.UserId == id && u.IsDeleted == false
                                      )
                                  select new
                                  {
                                      o.IdleTimeLimit,
                                      o.UserId
                                  }).FirstOrDefault();
                    abc = result.IdleTimeLimit.ToString("HH:mm:ss");
                }

            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public dynamic IdleEmployeeAlert(string id)", "Exception while fetching latest record for TRACKEMPLOYEESTATUS ", id);
            }
            return abc;
        }

        /// <summary>
        /// Created by :Bhushan Dod 
        /// Created Date :06/04/2015
        /// Alert screen on  manager if any Facility request not accepted with in 30 sec
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic FacRequestNotAcceptPushNotificaiton(long UserId)
        {
            dynamic abc = null;
            LoginManager objLoginManager = new LoginManager();
            try
            {
                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    List<long> objL = new List<long>();
                    var list = objLoginManager.GetManageAdminUserLocationList(UserId);
                    foreach (var t in list)
                    {
                        objL.Add(t.LocationId);
                    }
                    abc = (from o in Context.WorkRequestAssignments.Where(q => q.WorkRequestProjectType == 256 && q.AssignToUserId == null && objL.Contains(q.LocationID))
                           orderby o.WorkRequestAssignmentID descending
                           select new
                           {
                               o.LocationID,
                               o.WorkRequestAssignmentID,
                               o.WorkOrderCode,
                               o.WorkOrderCodeID
                               // o.AssignToUserId
                           }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "  public dynamic FacRequestNotAcceptPushNotificaiton(string id)", "Exception while fetching latest record for FacilityRequest not accepted by any user ", UserId);
            }
            return abc;
        }

        /// <summary>
        /// Created By Vijay sahu on 13 june 2015
        /// Get All Active Managers who mapped with location based On LocationID
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public List<UserModel> GetAllManagerUsingID(long locationID)
        {
            try
            {

                if (locationID > 0)
                {
                    ManagerRepositroy objRe = new ManagerRepositroy();
                    return objRe.GetAllManagerUsingID(locationID);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {

                return null;
            }
        }

        /// <summary>
        /// Created By : Bhushan Dod
        /// Created Date : 07/22/2015
        /// Description : Send Email to manager if Qrc Expiration is today(DateTime.Now)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool EmailToMangerForQrcExpiration(long LocId, long userId, long userType)
        {
            LoginManager objLoginManager = new LoginManager();
            List<EmailToManagerModel> objEmailReturn = new List<EmailToManagerModel>();
            objEmailLogRepository = new EmailLogRepository();

            bool result = false;
            try
            {
                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    List<long> objL = new List<long>();

                    if (userType == 2)//Manager
                    {
                        var list = objLoginManager.GetManageAdminUserLocationList(userId);
                        foreach (var t in list)
                        { objL.Add(t.LocationId); }
                    }
                    if (userType == 5 || userType == 6 || userType == 1)//IT Administrator//Administrator
                    {
                        var list = objLoginManager.GetAdminUserLocationList(userId);
                        foreach (var t in list)
                        { objL.Add(t.LocationId); }
                    }

                    EmailHelper objEmailHelper = new EmailHelper();
                    DateTime time = DateTime.UtcNow;

                    var QrcDetails = Context.QRCMasters.Where(x => x.InsuranceExpDate != null && objL.Contains(x.LocationId.Value)
                                                                && (x.ExpirationStatus == null || x.ExpirationStatus == false)).ToList();

                    var QrcDetailsMinified = QrcDetails.Where(y => y.InsuranceExpDate.Value.Date == time.Date);

                    if (QrcDetailsMinified.Count() > 0)
                    {
                        objEmailReturn = objEmailLogRepository.SendEmailToManagerForItemMissingQRC(LocId, userId);
                        if (objEmailReturn.Count() > 0)
                        {
                            foreach (var d in QrcDetailsMinified)
                            {
                                foreach (var item in objEmailReturn)
                                {
                                    objEmailHelper.MailType = "QRCEXPIRATIONMAIL";
                                    objEmailHelper.QrCodeId = d.QRCodeID;
                                    objEmailHelper.ManagerName = item.ManagerName;
                                    objEmailHelper.UserName = item.UserName;
                                    objEmailHelper.emailid = item.ManagerEmail;
                                    objEmailHelper.LocationName = item.LocationName;
                                    objEmailHelper.SentBy = item.RequestBy;
                                    objEmailHelper.LocationID = item.LocationID;
                                    objEmailHelper.ItemName = d.QRCName;
                                    objEmailHelper.QrcType = _ICommonMethod.GetGlobalCodeDetailById(d.QRCTYPE);

                                    bool IsSent = objEmailHelper.SendEmailWithTemplate();
                                    if (IsSent == true)
                                    {
                                        objEmailog = new EmailLog();
                                        try
                                        {
                                            objEmailog.CreatedBy = item.RequestBy;
                                            objEmailog.CreatedDate = DateTime.UtcNow;
                                            objEmailog.DeletedBy = null;
                                            objEmailog.DeletedOn = null;
                                            objEmailog.LocationId = item.LocationID;
                                            objEmailog.ModifiedBy = null;
                                            objEmailog.ModifiedOn = null;
                                            objEmailog.SentBy = item.RequestBy;
                                            objEmailog.SentEmail = item.ManagerEmail;
                                            objEmailog.Subject = objEmailHelper.Subject;
                                            objEmailog.SentTo = item.ManagerUserId;

                                            objEmailLogRepository.SaveEmailLog(objEmailog);
                                        }
                                        catch (Exception)
                                        {
                                            throw;
                                        }
                                    }
                                    if (item.DeviceId != null)
                                    {
                                        PushNotification.GCMAndroid("Today is the last day of QRC " + d.QRCodeID + ".It will get expire by tommorrow at location " + item.LocationName, item.DeviceId, objEmailHelper);
                                    }
                                    result = true;
                                }
                                if (result == true)
                                {
                                    objQRCMasterRepository = new QRCMasterRepository();
                                    var qrcDetails = objQRCMasterRepository.GetSingleOrDefault(t => t.QRCID == d.QRCID && t.IsDeleted == false);
                                    if (qrcDetails != null)
                                    {
                                        qrcDetails.ExpirationStatus = true;
                                        objQRCMasterRepository.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "bool EmailToMangerForQrcExpiration(long LocId, long userId, long userType)", "Exception while EmailToMangerForQrcExpiration ", LocId);
            }
            return result;

        }
    }
}

