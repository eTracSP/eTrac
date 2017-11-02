using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
//using System.Globalization;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helper.SerializationHelper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class QRCSetupManager : IQRCSetup
    {
        //ICommonMethod _ICommonMethod;
        //CommonRepository : ICommonRepository
        UserRegistration ObjUser;
        CommonRepository ObjCommonRepository;
        //VendorRepository ObjVendorRepository;
        //VehicleRepository ObjVehicleRepository;
        QRCMasterRepository ObjQRCMasterRepository;
        //OtherQRCRepository ObjOtherQRCRepository;
        UserRepository ObjUserRepository;
        LoginLogRepository objLoginLogRepository;
        ManageManager ObjManageManager;
        AddressMasterRepository ObjAddressRepository;
        AddressManager ObjAddressManager;
        GlobalCodesRepository ObjGlobalCodesRepository;
      
        DARRepository objDARRepository;
        // readonly long VehicleQRCTYPE = 36;
        readonly long VEHICLETYPEGolfCartID = 54;
        CommonMethodManager _ICommonMethod = new CommonMethodManager();
        EmailLogRepository objEmailLogRepository = new EmailLogRepository();
        QRCScanLogRepository objQRCScanLogRepository;
        EmailLog objEmailog;

        string message = string.Empty;
        /// <summary>GetGlobalCodeForCategories
        /// CreatedBy   :   Nagendra Upwanshi
        /// CreatedOn   :   Sep-05-2014
        /// CreatedFor  :   Get Vehicle Global Code For Categories
        /// </summary>
        /// <returns></returns>
        public QRCModel GetGlobalCodeForCategories()
        {
            QRCModel ObjQRCModel = new QRCModel();
            ObjCommonRepository = new CommonRepository();
            string[] Categories = { "QRCSIZE", "QRCTYPE", "VEHICLETYPE", "MOTORTYPE", "FUNCTIONALSTATUS", "WORKINGSTATUS", "FUELLEVEL", "CLIENTTYPE", "PURCHASETYPE" };
            List<GlobalCodeModel> ObjGlobalCodeList = ObjCommonRepository.GetGlobalCodeFor(Categories);

            ObjQRCModel.VehicleTypeList = ObjGlobalCodeList.Where(g => g.Category == "VEHICLETYPE").ToList();
            ObjQRCModel.MotorTypeList = ObjGlobalCodeList.Where(g => g.Category == "MOTORTYPE").ToList();
            ObjQRCModel.QRCTypeList = ObjGlobalCodeList.Where(g => g.Category == "QRCTYPE").ToList();
            ObjQRCModel.QRCSize = ObjGlobalCodeList.Where(g => g.Category == "QRCSIZE").ToList();
            ObjQRCModel.PurchaseTypeList = ObjGlobalCodeList.Where(g => g.Category == "PURCHASETYPE").ToList();
            ObjQRCModel.VehicleModel = new VehicleModel();
            ObjQRCModel.VehicleModel.FuelLevelList = ObjGlobalCodeList.Where(g => g.Category == "FUELLEVEL").ToList();
            ObjQRCModel.VehicleModel.FunctionalStatus = ObjGlobalCodeList.Where(g => g.Category == "FUNCTIONALSTATUS").ToList();
            ObjQRCModel.VehicleModel.WorkingStatus = ObjGlobalCodeList.Where(g => g.Category == "WORKINGSTATUS").ToList();
            ObjQRCModel.VehicleModel.ClientType = ObjGlobalCodeList.Where(g => g.Category == "CLIENTTYPE").ToList();
            ObjQRCModel.MotorTypeVehicle = ObjQRCModel.VehicleTypeList.Where(g => g.Category == "VEHICLETYPE" && g.CodeName == "Motor Vehicle").First().GlobalCodeId;

            #region getPreviousQRCID
            ObjQRCMasterRepository = new QRCMasterRepository();
            long LastQRCId = ObjQRCMasterRepository.GetLastQRCID();
            //if (LastQRCId > 0) { ObjQRCModel.EncryptLastQRC = Cryptography.GetEncryptedData((LastQRCId + 1).ToString(), true); }
            ObjQRCModel.EncryptLastQRC = Convert.ToString((LastQRCId > 0) ? (LastQRCId + 1) : 1, CultureInfo.InvariantCulture);

            if (ObjQRCModel.EncryptLastQRC.Length > 0)
            {
                ObjQRCModel.EncryptLastQRC = Convert.ToString(ObjQRCModel.EncryptLastQRC).PadLeft(5, '0');
            }
            //ObjQRCModel.QRCId = LastQRCId;
            //     result = table.OrderByDescending(x => x.Status).First();
            #endregion getPreviousQRCID
            return ObjQRCModel;
        }

        #region comment code because duplicate found
        /*       
        /// <summary>GetVendorDetails
        /// CreatedBy   :   Nagendra Upwanshi
        /// CreatedOn   :   Sep-05-2014
        /// CreatedFor  :   Get Vendor Details
        /// </summary>
        /// <returns></returns>
        public List<VendorModel> GetVendorDetails()
        {
            try
            {
                List<VendorModel> ObjListVendorModel = null;
                ObjVendorRepository = new VendorRepository();
                ObjListVendorModel = ObjVendorRepository.GetVendorDetails(null, "", null, null, "", "");
                return ObjListVendorModel;
            }
            catch (Exception ex) { throw ex; }
        }
        
        */
        #endregion comment code because duplicate found

        /// <summary>ChkDuplicateQRC
        /// CreatedBy:  Nagendra Upwanshi
        /// CreatedFor: checking Duplicate QRC before new entry
        /// CreatedOn:  Sep-16-2014
        /// Modified By : Bhushan Dod on 06/29/2015
        /// Description : QRC Name exist only in paticular loc not in all loc
        /// </summary>
        /// <param name="QRCName"></param>
        /// <param name="QRCID"></param>
        /// <param name="ObjQRCMaster"></param>
        /// <returns></returns>
        public bool ChkDuplicateQRC(string QRCName, long QRCID, long LocId, out QRCMaster ObjQRCMaster)
        {
            try
            {
                ObjQRCMaster = null;
                //QRCID = 0;
                if (QRCID == 0)
                {
                    ObjQRCMasterRepository = new QRCMasterRepository();
                    var data = ObjQRCMasterRepository.GetAll(q => q.QRCName == QRCName.Trim() && q.QRCID != QRCID && q.LocationId == LocId && q.IsDeleted == false);
                    if (data.Count > 0)
                    {
                        ObjQRCMaster = data[0];
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            { throw; }
        }
        ///// <summary>ChkDuplicateOtherQRC
        ///// CreatedBy:  Nagendra Upwanshi
        ///// CreatedFor: checking Duplicate Other QRC before new entry
        ///// CreatedOn:  Sep-17-2014
        ///// </summary>
        ///// <param name="OtherQRCName"></param>
        ///// <param name="OtherQRCType"></param>
        ///// <param name="OtherQRCID"></param>
        ///// <param name="ObjOtherQRC"></param>
        ///// <returns></returns>
        //public bool ChkDuplicateOtherQRC(string OtherQRCName, long OtherQRCType, long OtherQRCID, out OtherQRCMaster ObjOtherQRC)
        //{
        //    try
        //    {
        //        ObjOtherQRC = null;
        //        ObjOtherQRCRepository = new OtherQRCRepository();
        //        var data = ObjOtherQRCRepository.GetAll(oq => oq.ItemName == OtherQRCName.Trim() && oq.QRCType == OtherQRCType && oq.QRCID != OtherQRCID && oq.IsDeleted == false);
        //        if (data.Count > 0)
        //        {
        //            ObjOtherQRC = data[0];
        //            return false;
        //        }
        //        return true;
        //    }
        //    catch (Exception)
        //    { throw; }
        //}

        /// <summary>ProcessQRCSetup
        /// CreatedBy:  Nagendra Upwanshi
        /// CreatedFor: Process QRC Setup for all QRC Type
        /// CreatedOn:  Sep-11-2014        
        /// </summary>
        /// <param name="ObjQRCModel"></param>
        /// <param name="QRCId"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool ProcessQRCSetup(QRCModel ObjQRCModel, out long QRCId, out Result result, out PrintQRCModel ObjPrintQRCModel)
        {
            bool statusresult = false; result = Result.Failed; long _moduleNameId = 91;
            try
            {
                long _qRCid = 0; DateTime _createdOn = DateTime.UtcNow; long _CreatedBy = ObjQRCModel.CreatedBy; //long _DriverUserType = 6;
                ObjManageManager = new ManageManager();
                QRCModel OutObjQRCModel; ObjPrintQRCModel = null;
                #region QRC Master Entry
                using (TransactionScope TranScope = new TransactionScope())
                {
                    //if (QRCMasterEntry(ObjQRCModel, _CreatedBy, _createdOn, _moduleNameId, out _qRCid, out ObjPrintQRCModel))
                    if (QRCMasterEntry(ObjQRCModel, _CreatedBy, _createdOn, _moduleNameId, out _qRCid, out OutObjQRCModel))
                    {
                        if (OutObjQRCModel.QRCId > 0)
                        {
                            TranScope.Complete(); statusresult = true;
                            if (ObjQRCModel.QRCId > 0 && ObjQRCModel.UpdateMode == true)
                            { result = Result.UpdatedSuccessfully; }
                            else { result = Result.Completed; }
                        }
                        /*blocked because same table and not in use*/
                        #region not in use
                        /*
                        if (ObjQRCModel != null && ObjQRCModel.QRCTYPE == Convert.ToInt64(QrcType.Vehicle))
                        {
                        }
                        else
                        {
                            #region OtherQRCMaster
                            OtherQRCMaster ObjOtherQRC; ObjOtherQRCRepository = new OtherQRCRepository();
                            if (ChkDuplicateOtherQRC(ObjQRCModel.QRCName, ObjQRCModel.QRCTYPE, ObjQRCModel.OtherQRCID, out ObjOtherQRC))
                            {
                                ObjOtherQRC = new OtherQRCMaster();
                                ObjOtherQRC.ItemName = ObjQRCModel.QRCName;
                                ObjOtherQRC.ItemDescription = ObjQRCModel.SpecialNotes;
                                ObjOtherQRC.QRCID = _qRCid;
                                ObjOtherQRC.CreatedBy = _CreatedBy;
                                ObjOtherQRC.CreatedOn = _createdOn;
                                ObjOtherQRC.QRCType = ObjQRCModel.QRCTYPE;
                                ObjOtherQRCRepository.Add(ObjOtherQRC);
                                if (ObjOtherQRC.OtherQRCID > 0)
                                { TranScope.Complete(); result = Result.Completed; statusresult = true; }
                            }
                            else
                            { TranScope.Dispose(); result = Result.DuplicateRecord; }
                            #endregion OtherQRCMaster
                            #region Other QRC Type
                            if (false)
                            {
                                if (ObjQRCModel != null && ObjQRCModel.QRCTYPE == Convert.ToInt64(QrcType.TrashCan))
                                {
                                    #region TrashCan
                                    //TranScope.Complete();
                                    //result = Result.Completed;
                                    #endregion TrashCan
                                }
                                else if (ObjQRCModel != null && ObjQRCModel.QRCTYPE == Convert.ToInt64(QrcType.TicketSpitter))
                                {
                                    #region TicketSpitter
                                    //TranScope.Complete();
                                    //result = Result.Completed;
                                    #endregion TicketSpitter End
                                }
                                else if (ObjQRCModel != null && ObjQRCModel.QRCTYPE == Convert.ToInt64(QrcType.MovingWalkway))
                                {
                                    #region MovingWalkway
                                    //TranScope.Complete();
                                    //result = Result.Completed;
                                    #endregion MovingWalkway End
                                }
                                else if (ObjQRCModel != null && ObjQRCModel.QRCTYPE == Convert.ToInt64(QrcType.GateArm))
                                {
                                    #region GateArm
                                    //TranScope.Complete();
                                    //result = Result.Completed;
                                    #endregion GateArm End
                                }
                                else if (ObjQRCModel != null && ObjQRCModel.QRCTYPE == Convert.ToInt64(QrcType.Escalators))
                                {
                                    #region Escalators
                                    //TranScope.Complete();
                                    //result = Result.Completed;
                                    #endregion Escalators End
                                }
                                else if (ObjQRCModel != null && ObjQRCModel.QRCTYPE == Convert.ToInt64(QrcType.Equipment))
                                {
                                    #region Equipment
                                    //TranScope.Complete();
                                    //result = Result.Completed;
                                    #endregion Equipment End
                                }
                                else if (ObjQRCModel != null && ObjQRCModel.QRCTYPE == Convert.ToInt64(QrcType.EmergencyPhoneSystems))
                                {
                                    #region EmergencyPhoneSystems
                                    //TranScope.Complete();
                                    //result = Result.Completed;
                                    #endregion EmergencyPhoneSystems End
                                }
                                else if (ObjQRCModel != null && ObjQRCModel.QRCTYPE == Convert.ToInt64(QrcType.Elevator))
                                {
                                    #region Elevator
                                    //TranScope.Complete();
                                    //result = Result.Completed;
                                    #endregion Elevator End
                                }
                                else if (ObjQRCModel != null && ObjQRCModel.QRCTYPE == Convert.ToInt64(QrcType.Devices))
                                {
                                    #region Devices
                                    //TranScope.Complete();
                                    //result = Result.Completed;
                                    #endregion Devices End
                                }
                                else if (ObjQRCModel != null && ObjQRCModel.QRCTYPE == Convert.ToInt64(QrcType.BusStation))
                                {
                                    #region BusStation
                                    //TranScope.Complete();
                                    //result = Result.Completed;
                                    #endregion BusStation End
                                }
                                else if (ObjQRCModel != null && ObjQRCModel.QRCTYPE == Convert.ToInt64(QrcType.Bathroom))
                                {
                                    #region Bathroom
                                    //TranScope.Complete();
                                    //result = Result.Completed;
                                    #endregion Bathroom End
                                }
                                else
                                {
                                    #region Other then available QRCType
                                    //TranScope.Complete();
                                    //result = Result.Completed;
                                    #endregion Other then available QRCType End
                                }
                            }
                            #endregion Other QRC Type
                        }
                        */
                        #endregion not in use
                        /*blocked because same table and not in use END*/
                    }
                    else
                    { result = Result.DuplicateRecord; }
                    #region add items for PrintQRCModel 2
                    ObjPrintQRCModel = GetPrintQRC(OutObjQRCModel);
                    #endregion add items for PrintQRCModel 2
                    QRCId = _qRCid;
                }
                #endregion QRC Master Entry End
                /// rinku end
                //AutoMapper.Mapper.CreateMap<QRCModel, PrintQRCModel>();
                //ObjPrintQRCModel = AutoMapper.Mapper.Map(OutObjQRCModel, ObjPrintQRCModel);
            }
            catch (TransactionAbortedException ex)
            {
                throw new Exception("TransactionAbortedException Message: " + ex.Message, ex.InnerException);
            }
            catch (ApplicationException ex)
            {
                throw new Exception("ApplicationException Message: " + ex.Message, ex.InnerException);
            }
            catch (Exception)
            { throw; }

            return statusresult;
        }
        /// <summary>SaveDriver
        /// 
        /// </summary>
        /// <param name="objUserModel"></param>
        /// <param name="QRCID"></param>
        /// <param name="ObjUser"></param>
        /// <returns></returns>
        public Result SaveDriver(UserModel objUserModel, out long QRCID, out UserRegistration objUser)
        {
            try
            {
                ObjUserRepository = new UserRepository();
                ObjAddressManager = new AddressManager();
                ObjUser = null;
                ObjGlobalCodesRepository = new GlobalCodesRepository();
                if (ObjManageManager.CheckDuplicateUser(objUserModel.UserEmail.Trim(), objUserModel.UserId, out QRCID, out objUser))
                {
                    ObjUser = new UserRegistration();
                    objUserModel.IsEmailVerify = true;
                    objUserModel.IsLoginActive = true;
                    if (objUserModel.Gender != null)
                    { objUserModel.Gender = objUserModel.Gender == 1 ? ObjGlobalCodesRepository.GetSingleOrDefault(g => g.CodeName == "Male").GlobalCodeId : ObjGlobalCodesRepository.GetSingleOrDefault(g => g.CodeName == "Female").GlobalCodeId; }
                    if (objUserModel.UserId == 0)
                    {
                        AutoMapper.Mapper.CreateMap<UserModel, UserRegistration>();
                        ObjUser = AutoMapper.Mapper.Map(objUserModel, ObjUser);
                        ObjUser.Password = Cryptography.GetEncryptedData(ObjUser.Password, true);
                        ObjUserRepository.Add(ObjUser);
                        if (ObjUser.UserId > 0)
                        {
                            objUserModel.Address.UserId = ObjUser.UserId;
                            ObjAddressManager.SaveAddress(objUserModel.Address);
                            return Result.Completed;
                        }
                        else
                            return Result.Failed;
                    }
                    else
                    {
                        ObjManageManager.UpdateUser(objUserModel, out QRCID, false, out ObjUser);
                        objUserModel.Address.UserId = objUserModel.UserId;
                        ObjAddressManager.SaveAddress(objUserModel.Address);
                        return Result.UpdatedSuccessfully;
                    }
                }
                else
                { return Result.DuplicateRecord; }
            }
            catch (Exception)
            { throw; }
        }
        ///
        private PrintQRCModel GetPrintQRC(QRCModel ObjQRCModel)
        {
            #region add items for PrintQRCModel
            PrintQRCModel _printQRCModel = null;
            AutoMapper.Mapper.CreateMap<QRCModel, PrintQRCModel>();
            _printQRCModel = AutoMapper.Mapper.Map(ObjQRCModel, _printQRCModel);
            _printQRCModel.IsVendor = (ObjQRCModel.ClientTypeID.HasValue && ObjQRCModel.ClientTypeID.Value > 0 && ObjQRCModel.ClientTypeID == Convert.ToInt64(ClientVendorType.IndividualClient, CultureInfo.InvariantCulture)) ? true : false;
            //_printQRCModel.VehicleImage = ObjVehicleMaster.VehicleImage;
            //_printQRCModel.VehicleImage = ObjQRCModel.VehicleImage;
            _printQRCModel.QRCName = ObjQRCModel.QRCName;
            _printQRCModel.SpecialNotes = ObjQRCModel.SpecialNotes;
            _printQRCModel.QRCTYPE = ObjQRCModel.QRCTYPECaption;
            _printQRCModel.VehicleType = ObjQRCModel.VehicleTypeCaption;
            _printQRCModel.MotorType = ObjQRCModel.MotorTypeCaption;
            _printQRCModel.SizeCaption = ObjQRCModel.SizeCaption;
            _printQRCModel.QRCIDCode = ObjQRCModel.QRCodeID;
            /*
            if (ObjQRCModel.ClientTypeID.HasValue && ObjQRCModel.ClientTypeID.Value > 0 && ObjQRCModel.ClientTypeID.Value == Convert.ToInt64(ClientVendorType.VendorClient))
            {
                VendorRepository ObjVendorRepository = new Data.VendorRepository();
                VendorRegistration vendor = ObjVendorRepository.GetSingleOrDefault(v => v.VendorID == ObjQRCModel.VendorID && v.IsDeleted == false);
                _printQRCModel.VendorName = vendor.ContactName;
                _printQRCModel.VendorDetails = vendor.OrganizationName + " " + vendor.Address1 + " " + vendor.ZipCode;
            }
            //driver
            if (string.IsNullOrEmpty(ObjQRCModel.DriverName))
            {
                ObjUserRepository = new UserRepository();
                UserRegistration ObjUser = ObjUserRepository.GetSingleOrDefault(u => u.QRCID == ObjQRCModel.QRCId && u.IsDeleted == false);
                if (ObjUser != null)
                {
                    _printQRCModel.DriverName = ObjUser.FirstName + " " + ObjUser.LastName;
                    _printQRCModel.DriverImage = ObjUser.ProfileImage;
                }
            }
            */
            // project
            //if (true)
            //{
            //        _printQRCModel.CompanyLogo = project.ProjectLogoURl;
            //        _printQRCModel.CompanyImage = project.ProjectLogoName;

            //}
            #endregion add items for PrintQRCModel
            return _printQRCModel;
        }

        ///// <summary>GetPrintVehicle
        ///// CreatedBy   :   Nagendra Upwanshi
        ///// CreatedOn   :   Oct-16-2014
        ///// CreatedFor  :   GetPrintVehicle
        ///// </summary>
        ///// <param name="ObjVehicleRegistration"></param>
        ///// <returns></returns>
        //private PrintQRCModel GetPrintVehicle_old(VehicleRegistration ObjVehicleRegistration)
        //{
        //    #region add items for PrintQRCModel
        //    PrintQRCModel _printQRCModel = null;
        //    /*
        //    AutoMapper.Mapper.CreateMap<QRCModel, PrintQRCModel>();
        //    _printQRCModel = AutoMapper.Mapper.Map(ObjQRCModel, _printQRCModel);
        //    _printQRCModel.IsVendor = (ObjQRCModel.ClientTypeID.HasValue && ObjQRCModel.ClientTypeID.Value > 0 && ObjQRCModel.ClientTypeID == Convert.ToInt64(ClientVendorType.IndividualClient)) ? true : false;
        //    //_printQRCModel.VehicleImage = ObjVehicleMaster.VehicleImage;
        //    //_printQRCModel.VehicleImage = ObjQRCModel.VehicleImage;
        //    if (ObjQRCModel.ClientTypeID.HasValue && ObjQRCModel.ClientTypeID.Value > 0 && ObjQRCModel.ClientTypeID.Value == Convert.ToInt64(ClientVendorType.VendorClient))
        //    {
        //        VendorRepository ObjVendorRepository = new Data.VendorRepository();
        //        VendorRegistration vendor = ObjVendorRepository.GetSingleOrDefault(v => v.VendorID == ObjQRCModel.VendorID && v.IsDeleted == false);
        //        _printQRCModel.VendorName = vendor.ContactName;
        //        _printQRCModel.VendorDetails = vendor.OrganizationName + " " + vendor.Address1 + " " + vendor.ZipCode;
        //    }
        //    //driver
        //    if (string.IsNullOrEmpty(ObjQRCModel.DriverName))
        //    {
        //        ObjUserRepository = new UserRepository();
        //        UserRegistration ObjUser = ObjUserRepository.GetSingleOrDefault(u => u.QRCID == ObjQRCModel.QRCId && u.IsDeleted == false);
        //        if (ObjUser != null)
        //        {
        //            _printQRCModel.DriverName = ObjUser.FirstName + " " + ObjUser.LastName;
        //            _printQRCModel.DriverImage = ObjUser.ProfileImage;
        //        }
        //    }
        //    // project
        //    //if (true)
        //    //{
        //    //        _printQRCModel.CompanyLogo = project.ProjectLogoURl;
        //    //        _printQRCModel.CompanyImage = project.ProjectLogoName;
        //    //}
        //    */
        //    #endregion add items for PrintQRCModel
        //    return _printQRCModel;
        //}
        ///// <summary>GetVehicleDetails
        ///// CreatedBy   :   Nagendra Upwanshi
        ///// CreatedOn   :   Sep-06-2014
        ///// CreatedFor  :   Get Vendor Details
        ///// </summary>
        ///// <returns></returns>
        //public List<VehicleModel> GetVehicleDetails()
        //{
        //    try
        //    {
        //        List<VehicleModel> ObjListVehicleModel = null;
        //        ObjVehicleRepository = new VehicleRepository();
        //        ObjListVehicleModel = ObjVehicleRepository.GetAll(v => v.IsDeleted == false).Select(s => new VehicleModel()
        //        {
        //            VehicleID = s.VehicleID,
        //        }).ToList();
        //        return ObjListVehicleModel;
        //    }
        //    catch (Exception) { throw; }
        //}
        public List<QRCListModel> GetAllQRCList(long? qrcId, long? locationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, long? ddlQRCType, long userId, ObjectParameter TotalRecords)
        {
            try
            {
                ObjQRCMasterRepository = new QRCMasterRepository();
                return ObjQRCMasterRepository.GetAllQRCList(qrcId, locationId, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, ddlQRCType, userId, TotalRecords);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// CreatedBy   :   Nagendra Upwanshi
        /// CreatedOn   :   Sep-24-2014
        /// CreatedFor  :   Get QRC by QRCId
        /// </summary>
        /// <param name="QRCID"></param>
        /// <returns></returns>
        public QRCModel GetQrcById(long qrcId)
        {
            try
            {

                ServiceQrcVehicleModel ObjReturn = new ServiceQrcVehicleModel();
                ServiceQrcTrashcanModel ObjReturnTrash = new ServiceQrcTrashcanModel();
                ObjQRCMasterRepository = new QRCMasterRepository();
                ObjUserRepository = new UserRepository();
                QRCMaster ObjQRCMaster = ObjQRCMasterRepository.GetSingleOrDefault(q => q.QRCID == qrcId && q.IsDeleted == false);
                if (ObjQRCMaster == null)
                { throw new Exception("Record not found."); }

                //if (ObjQRCMaster.QRCTYPE == Convert.ToInt64(QrcType.Vehicle, CultureInfo.InvariantCulture)) { }
                //else { }

                QRCModel ObjQRCModel = GetGlobalCodeForCategories();
                ObjQRCModel.QRCId = ObjQRCMaster.QRCID;
                ObjQRCModel.MotorType = ObjQRCMaster.MotorType;
                ObjQRCModel.VehicleType = ObjQRCMaster.VehicleType;
                //ObjQRCModel.OtherQRCID = ObjQRCMaster.OtherQRCID;
                ObjQRCModel.QRCDefaultSize = ObjQRCMaster.QRCDefaultSize;
                if (ObjQRCMaster.QRCDefaultSize != null)
                {
                    ObjQRCModel.QRCSizeGenerate = ObjQRCModel.QRCSize.FirstOrDefault(z => z.GlobalCodeId == ObjQRCMaster.QRCDefaultSize).Description;
                }
                ObjQRCModel.QRCName = ObjQRCMaster.QRCName;
                ObjQRCModel.QRCTYPE = ObjQRCMaster.QRCTYPE;
                ObjQRCModel.SpecialNotes = ObjQRCMaster.SpecialNotes;
                ObjQRCModel.VendorID = ObjQRCMaster.VendorID;
                ObjQRCModel.ClientTypeID = ObjQRCMaster.ClientTypeID;
                ObjQRCModel.QRCodeID = ObjQRCMaster.QRCodeID;
                ObjQRCModel.Administrator = ObjQRCMaster.CreatedBy;
                ObjQRCModel.CheckOutStatus = ObjQRCMaster.CheckOutStatus;
                ObjQRCModel.UserName = ObjQRCMaster.UserName;
                ObjQRCModel.CellPhoneCheckOutStatus = ObjQRCMaster.CheckOutStatus;

                //get extra and warrantydetails
                ObjQRCModel.SerialNo = ObjQRCMaster.SerialNo;
                ObjQRCModel.Make = ObjQRCMaster.Make;
                ObjQRCModel.Model = ObjQRCMaster.Model;
                ObjQRCModel.LocationPicture = ObjQRCMaster.LocationPicture;
                string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
                ObjQRCModel.AssetPicture = ObjQRCMaster.AssetPicture;
                ObjQRCModel.VendorName = ObjQRCMaster.VendorName;
                ObjQRCModel.PointOfContact = ObjQRCMaster.PointOfContact;
                ObjQRCModel.TelephoneNo = ObjQRCMaster.TelephoneNo;
                ObjQRCModel.EmialAdd = ObjQRCMaster.EmialAdd;
                //if (ObjQRCMaster.WarrantyEndDate.Value.ToClientTimeZoneinDateTime().ToString() == "")
                //{
                //    ObjQRCMaster.WarrantyEndDate = null;
                //}

                if (ObjQRCMaster.WarrantyEndDate != null)
                {
                    ObjQRCMaster.WarrantyEndDate = ObjQRCMaster.WarrantyEndDate.Value.ToClientTimeZoneinDateTime();
                    ObjQRCModel.WarrantyEndDate = ObjQRCMaster.WarrantyEndDate.Value.ToClientTimeZoneinDateTime();
                }
                //ObjQRCModel.WarrantyEndDate = ObjQRCMaster.WarrantyEndDate.Value.ToClientTimeZoneinDateTime();
                ObjQRCModel.WarrantyDoc = ObjQRCMaster.WarrantyDoc;
                ObjQRCModel.Website = ObjQRCMaster.Website;
                ObjQRCModel.PurchaseType = ObjQRCMaster.PurchaseType;
                ObjQRCModel.PurchaseTypeRemark = ObjQRCMaster.PurchaseTypeRemark;

                //if (ObjQRCMaster.InsuranceExpDate.Value.ToClientTimeZoneinDateTime().ToString() == "")
                //{
                //    ObjQRCMaster.InsuranceExpDate = null;
                //}
                if (ObjQRCMaster.InsuranceExpDate != null)
                {
                    ObjQRCMaster.InsuranceExpDate = ObjQRCMaster.InsuranceExpDate.Value.ToClientTimeZoneinDateTime();
                    ObjQRCModel.InsuranceExpDate = ObjQRCMaster.InsuranceExpDate.Value.ToClientTimeZoneinDateTime();
                }

                // ObjQRCModel.InsuranceExpDate = ObjQRCMaster.InsuranceExpDate.Value.ToClientTimeZoneinDateTime();
                // get vehicle details
               
                //ObjQRCModel.ClientTypeID = ObjVehicleMaster.ClientTypeID; 
                //Added By Bhushan Dod on 29/01/2015
                //Description :- For get the xml of qrc type and Prefix URL of Asset & Loc Img
                ObjQRCModel.QRCTypeDetails = ObjQRCMaster.QRCTypeDetails;
                ObjQRCModel.Allotedto = ObjQRCMaster.AllotedTo;
                ObjQRCModel.LocationId = ObjQRCMaster.LocationId;
                ObjQRCModel.LocationName = ObjQRCMaster.LocationMaster.LocationName;
                ObjQRCModel.CreatedOn = ObjQRCMaster.CreatedDate.ToClientTimeZone(true);
                ObjQRCModel.CreatedBy = ObjQRCMaster.CreatedBy;

                string AssetImgURLPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ProjectLogoPath"], CultureInfo.InvariantCulture);
                ObjQRCModel.AssetPicturePath = HostingPrefix + AssetImgURLPrefix.Replace("~", "") + (ObjQRCMaster.AssetPicture == null ? "no-profile-pic.jpg" : ObjQRCMaster.AssetPicture);
                ObjQRCModel.LocationPicturePath = HostingPrefix + AssetImgURLPrefix.Replace("~", "") + (ObjQRCMaster.LocationPicture == null ? "no-profile-pic.jpg" : ObjQRCMaster.LocationPicture);
                ObjQRCModel.WarrantyDocumentPath = AssetImgURLPrefix + ObjQRCMaster.WarrantyDoc;
                //Added By Bhushan Dod on 03/25/2015 for Mileage check in mobile app
                if (ObjQRCMaster.QRCTYPE == Convert.ToInt64(36) && ObjQRCMaster.QRCTypeDetails != null)
                {
                    ObjReturn = GenericDataContractSerializer<ServiceQrcVehicleModel>.DeserializeXml(ObjQRCMaster.QRCTypeDetails);
                    ObjQRCModel.ChkOutVmDescription = ObjReturn.CheckingOut.VmDescription;
                    ObjQRCModel.ChkInVmDescription = ObjReturn.CheckingIn.ChVmDescription;
                    ObjQRCModel.VfVmDescription = ObjReturn.VehicleFuel.VfVmDescription;
                    ObjQRCModel.WVmDescription = ObjReturn.VehicleCheck.WVmDescription;
                    ObjQRCModel.FuelLevel = ObjReturn.CheckingIn.FuelLevel;
                    ObjQRCModel.WeeklyFuelLevel = ObjReturn.VehicleCheck.FuelLevel;
                }
                //Added By Bhushan Dod on 04/29/2015 for Trash levels check in mobile app
                if (ObjQRCMaster.QRCTYPE == Convert.ToInt64(37) && ObjQRCMaster.QRCTypeDetails != null)
                {
                    ObjReturnTrash = GenericDataContractSerializer<ServiceQrcTrashcanModel>.DeserializeXml(ObjQRCMaster.QRCTypeDetails);
                    ObjQRCModel.RoutineTrashLevels = ObjReturnTrash.Routine.TrashLevels;
                    ObjQRCModel.RemovalTrashLevels = ObjReturnTrash.TrashRemoval.TrashLevels;
                }

                //get driver details

                //UserRegistration ObjUser = ObjQRCMaster.UserRegistrations.Where(u => u.QRCID == ObjQRCMaster.QRCID && u.IsDeleted == false).FirstOrDefault();
                ////ObjQRCModel.ClientTypeID = ObjVehicleMaster.ClientTypeID;                
                //if (ObjUser != null)
                //{
                //    AutoMapper.Mapper.CreateMap<UserRegistration, UserModel>();
                //    ObjQRCModel.UserModel = AutoMapper.Mapper.Map(ObjUser, ObjQRCModel.UserModel);

                //    ObjQRCModel.DriverImage = ObjUser.ProfileImage;
                //    ObjQRCModel.DriverName = ObjUser.FirstName + " " + ObjUser.LastName;

                //    //// addrsess
                //    //public AddressModel Address 
                //    //ObjQRCModel.UserModel.Address = new AddressModel();
                //    AddressModel ObjAddressModel = new AddressModel();
                //    AddressMaster address = new AddressMaster();
                //    ObjAddressRepository = new AddressMasterRepository();
                //    address = ObjAddressRepository.GetSingleOrDefault(a => a.UserId == ObjQRCModel.UserModel.UserId && a.IsDeleted == false);

                //    AutoMapper.Mapper.CreateMap<AddressMaster, AddressModel>();
                //    ObjAddressModel = AutoMapper.Mapper.Map(address, ObjAddressModel);
                //    ObjQRCModel.UserModel.Address = ObjAddressModel;
                //    //// addrsess end
                //}

                ObjUser = ObjUserRepository.GetSingleOrDefault(u => u.UserId == ObjQRCModel.CreatedBy && u.IsDeleted == false);
                if (ObjUser != null)
                {
                    ObjQRCModel.UserModel = new UserModel();
                    ObjQRCModel.UserModel.FirstName = ObjUser.FirstName;
                    ObjQRCModel.UserModel.LastName = ObjUser.LastName;
                }

                return ObjQRCModel;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>
        /// CreatedBy   :   Nagendra Upwanshi
        /// CreatedOn   :   Sep-24-2014
        /// CreatedFor  :   Get QRC by QRCId
        /// </summary>
        /// <param name="QRCID"></param>
        /// <returns></returns>
        public QRCModel GetQrcById(long qrcId, long locationid)
        {
            try
            {
                ServiceQrcVehicleModel ObjReturn = new ServiceQrcVehicleModel();
                ServiceQrcTrashcanModel ObjReturnTrash = new ServiceQrcTrashcanModel();
                ServiceQrcShuttleBusModel objReturnShuttle = new ServiceQrcShuttleBusModel();
                ObjQRCMasterRepository = new QRCMasterRepository();
                ObjUserRepository = new UserRepository();
                QRCMaster ObjQRCMaster = ObjQRCMasterRepository.GetSingleOrDefault(q => q.QRCID == qrcId && q.LocationId == locationid && q.IsDeleted == false);
                if (ObjQRCMaster == null)
                { throw new Exception("Record not found."); }

                //if (ObjQRCMaster.QRCTYPE == Convert.ToInt64(QrcType.Vehicle, CultureInfo.InvariantCulture)) { }
                //else { }

                QRCModel ObjQRCModel = GetGlobalCodeForCategories();
                ObjQRCModel.QRCId = ObjQRCMaster.QRCID;
                ObjQRCModel.MotorType = ObjQRCMaster.MotorType;
                ObjQRCModel.VehicleType = ObjQRCMaster.VehicleType;
                //ObjQRCModel.OtherQRCID = ObjQRCMaster.OtherQRCID;
                ObjQRCModel.QRCDefaultSize = ObjQRCMaster.QRCDefaultSize;
                if (ObjQRCMaster.QRCDefaultSize != null)
                {
                    ObjQRCModel.QRCSizeGenerate = ObjQRCModel.QRCSize.FirstOrDefault(z => z.GlobalCodeId == ObjQRCMaster.QRCDefaultSize).Description;
                }
                ObjQRCModel.QRCName = ObjQRCMaster.QRCName;
                ObjQRCModel.QRCTYPE = ObjQRCMaster.QRCTYPE;
                ObjQRCModel.SpecialNotes = ObjQRCMaster.SpecialNotes;
                ObjQRCModel.VendorID = ObjQRCMaster.VendorID;
                ObjQRCModel.ClientTypeID = ObjQRCMaster.ClientTypeID;
                ObjQRCModel.QRCodeID = ObjQRCMaster.QRCodeID;
                ObjQRCModel.Administrator = ObjQRCMaster.CreatedBy;
                ObjQRCModel.CheckOutStatus = ObjQRCMaster.CheckOutStatus;
                ObjQRCModel.UserName = ObjQRCMaster.UserName;
                ObjQRCModel.CellPhoneCheckOutStatus = ObjQRCMaster.CheckOutStatus;
                ObjQRCModel.IsDamage = ObjQRCMaster.IsDamage;//Added By Bhushan Dod on 21/November/2016 for to avoid checkIn/Out if QRC is damage.
                ObjQRCModel.IsDamageVerified = ObjQRCMaster.IsDamageVerified;//Added By Bhushan Dod on 21/November/2016 for to avoid checkIn/Out if QRC is damage.

                //get extra and warrantydetails
                ObjQRCModel.SerialNo = ObjQRCMaster.SerialNo;
                ObjQRCModel.Make = ObjQRCMaster.Make;
                ObjQRCModel.Model = ObjQRCMaster.Model;
                ObjQRCModel.LocationPicture = ObjQRCMaster.LocationPicture;
                string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
                ObjQRCModel.AssetPicture = ObjQRCMaster.AssetPicture;
                ObjQRCModel.VendorName = ObjQRCMaster.VendorName;
                ObjQRCModel.PointOfContact = ObjQRCMaster.PointOfContact;
                ObjQRCModel.TelephoneNo = ObjQRCMaster.TelephoneNo;
                ObjQRCModel.EmialAdd = ObjQRCMaster.EmialAdd;
                if (ObjQRCMaster.WarrantyEndDate != null)
                {
                    ObjQRCMaster.WarrantyEndDate = ObjQRCMaster.WarrantyEndDate.Value.ToClientTimeZoneinDateTime();
                    ObjQRCModel.WarrantyEndDate = ObjQRCMaster.WarrantyEndDate.Value.ToClientTimeZoneinDateTime();
                }

                ObjQRCModel.WarrantyDoc = ObjQRCMaster.WarrantyDoc;
                ObjQRCModel.Website = ObjQRCMaster.Website;
                ObjQRCModel.PurchaseType = ObjQRCMaster.PurchaseType;
                ObjQRCModel.PurchaseTypeRemark = ObjQRCMaster.PurchaseTypeRemark;
                if (ObjQRCMaster.InsuranceExpDate != null)
                {
                    ObjQRCMaster.InsuranceExpDate = ObjQRCMaster.InsuranceExpDate.Value.ToClientTimeZoneinDateTime();
                    ObjQRCModel.InsuranceExpDate = ObjQRCMaster.InsuranceExpDate.Value.ToClientTimeZoneinDateTime();
                }
                //if (ObjQRCMaster.InsuranceExpDate != null)
                //{
                //    ObjQRCMaster.InsuranceExpDate = ObjQRCMaster.InsuranceExpDate.Value.ToClientTimeZoneinDateTime();
                //}
                ////if (ObjQRCMaster.InsuranceExpDate.Value.ToClientTimeZoneinDateTime().ToString() == "")
                ////{
                ////    ObjQRCMaster.InsuranceExpDate = null;
                ////}

                //ObjQRCModel.InsuranceExpDate = ObjQRCMaster.InsuranceExpDate.Value.ToClientTimeZoneinDateTime();
                // get vehicle details
               //ObjQRCModel.ClientTypeID = ObjVehicleMaster.ClientTypeID; 
                //Added By Bhushan Dod on 29/01/2015
                //Description :- For get the xml of qrc type and Prefix URL of Asset & Loc Img
                ObjQRCModel.QRCTypeDetails = ObjQRCMaster.QRCTypeDetails;
                ObjQRCModel.Allotedto = ObjQRCMaster.AllotedTo;
                ObjQRCModel.LocationId = ObjQRCMaster.LocationId;
                ObjQRCModel.LocationName = ObjQRCMaster.LocationMaster.LocationName;
                ObjQRCModel.CreatedOn = ObjQRCMaster.CreatedDate.ToClientTimeZone(true);
                ObjQRCModel.CreatedBy = ObjQRCMaster.CreatedBy;

                string AssetImgURLPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ProjectLogoPath"], CultureInfo.InvariantCulture);
                ObjQRCModel.AssetPicturePath = HostingPrefix + AssetImgURLPrefix.Replace("~", "") + ObjQRCMaster.AssetPicture;
                ObjQRCModel.LocationPicturePath = HostingPrefix + AssetImgURLPrefix.Replace("~", "") + ObjQRCMaster.LocationPicture;
                ObjQRCModel.WarrantyDocumentPath = AssetImgURLPrefix + ObjQRCMaster.WarrantyDoc;

                //Added By Bhushan Dod on 03/25/2015 for Mileage check in mobile app
                if (ObjQRCMaster.QRCTYPE == Convert.ToInt64(36) && ObjQRCMaster.QRCTypeDetails != null)
                {
                    ObjReturn = GenericDataContractSerializer<ServiceQrcVehicleModel>.DeserializeXml(ObjQRCMaster.QRCTypeDetails);
                    ObjQRCModel.ChkOutVmDescription = ObjReturn.CheckingOut.VmDescription;
                    ObjQRCModel.ChkInVmDescription = ObjReturn.CheckingIn.ChVmDescription;
                    ObjQRCModel.VfVmDescription = ObjReturn.VehicleFuel.VfVmDescription;
                    ObjQRCModel.WVmDescription = ObjReturn.VehicleCheck.WVmDescription;
                    ObjQRCModel.FuelLevel = ObjReturn.CheckingIn.FuelLevel;
                    ObjQRCModel.WeeklyFuelLevel = ObjReturn.VehicleCheck.FuelLevel;
                }
                //Added By Bhushan Dod on 28/06/2017 for shuttle Mileage check in mobile app
                if (ObjQRCMaster.QRCTYPE == Convert.ToInt64(375) && ObjQRCMaster.QRCTypeDetails != null)
                {
                    objReturnShuttle = GenericDataContractSerializer<ServiceQrcShuttleBusModel>.DeserializeXml(ObjQRCMaster.QRCTypeDetails);
                    ObjQRCModel.ChkOutVmDescription = objReturnShuttle.Mileage.ChShDescription;
                    //ObjQRCModel.ChkInVmDescription = ObjReturn.CheckingIn.ChVmDescription;
                }
                //Added By Bhushan Dod on 04/29/2015 for Trash levels check in mobile app
                if (ObjQRCMaster.QRCTYPE == Convert.ToInt64(37) && ObjQRCMaster.QRCTypeDetails != null)
                {
                    ObjReturnTrash = GenericDataContractSerializer<ServiceQrcTrashcanModel>.DeserializeXml(ObjQRCMaster.QRCTypeDetails);
                    ObjQRCModel.RoutineTrashLevels = ObjReturnTrash.Routine.TrashLevels;
                    ObjQRCModel.RemovalTrashLevels = ObjReturnTrash.TrashRemoval.TrashLevels;
                }

                //get driver details

                //UserRegistration ObjUser = ObjQRCMaster.UserRegistrations.Where(u => u.QRCID == ObjQRCMaster.QRCID && u.IsDeleted == false).FirstOrDefault();
                ////ObjQRCModel.ClientTypeID = ObjVehicleMaster.ClientTypeID;                
                //if (ObjUser != null)
                //{
                //    AutoMapper.Mapper.CreateMap<UserRegistration, UserModel>();
                //    ObjQRCModel.UserModel = AutoMapper.Mapper.Map(ObjUser, ObjQRCModel.UserModel);

                //    ObjQRCModel.DriverImage = ObjUser.ProfileImage;
                //    ObjQRCModel.DriverName = ObjUser.FirstName + " " + ObjUser.LastName;

                //    //// addrsess
                //    //public AddressModel Address 
                //    //ObjQRCModel.UserModel.Address = new AddressModel();
                //    AddressModel ObjAddressModel = new AddressModel();
                //    AddressMaster address = new AddressMaster();
                //    ObjAddressRepository = new AddressMasterRepository();
                //    address = ObjAddressRepository.GetSingleOrDefault(a => a.UserId == ObjQRCModel.UserModel.UserId && a.IsDeleted == false);

                //    AutoMapper.Mapper.CreateMap<AddressMaster, AddressModel>();
                //    ObjAddressModel = AutoMapper.Mapper.Map(address, ObjAddressModel);
                //    ObjQRCModel.UserModel.Address = ObjAddressModel;
                //    //// addrsess end
                //}

                ObjUser = ObjUserRepository.GetSingleOrDefault(u => u.UserId == ObjQRCModel.CreatedBy && u.IsDeleted == false);
                if (ObjUser != null)
                {
                    ObjQRCModel.UserModel = new UserModel();
                    ObjQRCModel.UserModel.FirstName = ObjUser.FirstName;
                    ObjQRCModel.UserModel.LastName = ObjUser.LastName;
                }

                return ObjQRCModel;
            }
            catch (Exception)
            { throw; }
        }
  


        /// <summary>QRCMasterEntry
        /// CreatedBy:  Nagendra Upwanshi
        /// CreatedFor: new QRC Master Entry
        /// CreatedOn:  Sep-16-2014
        /// <ModifiedOn>Nov-10-2014</ModifiedOn>
        /// <ModifiedBy>Nagendra Upwanshi</ModifiedBy>        
        /// </summary>
        private bool QRCMasterEntry(QRCModel ObjQRCModel, long _CreatedBy, DateTime _createdOn, long _moduleNameId, out long _qRCid, out QRCModel OutObjQRCModel)
        {
            bool found = false;
            _qRCid = 0;
            try
            {
                ObjQRCMasterRepository = new QRCMasterRepository();
                QRCMaster ObjQRCMaster; //ObjPrintQRCModel = null;
                OutObjQRCModel = null;

                if (ObjQRCModel.QRCId > 0 && !ObjQRCModel.UpdateMode) { throw new Exception("Cannot process QR Code already exists"); }

                if (ChkDuplicateQRC(ObjQRCModel.QRCName, ObjQRCModel.QRCId, ObjQRCModel.Location, out ObjQRCMaster))//Location Id added by bhushan for QRC Name check only for particular loc
                {
                    if (!string.IsNullOrEmpty(ObjQRCModel.QRCodeID) && ObjQRCModel.QRCodeID.Contains(','))
                    {
                        string[] QrcArray = ObjQRCModel.QRCodeID.Split(',');

                        #region getPreviousQRCID                       
                        long LastQRCId = ObjQRCMasterRepository.GetLastQRCID();
                        //if (LastQRCId > 0) { ObjQRCModel.EncryptLastQRC = Cryptography.GetEncryptedData((LastQRCId + 1).ToString(), true); }
                        var QRCStringID = Convert.ToString((LastQRCId > 0) ? (LastQRCId + 1) : 1, CultureInfo.InvariantCulture);

                        if (QRCStringID.Length > 0)
                        {
                            QRCStringID = Convert.ToString(QRCStringID).PadLeft(5, '0');
                        }

                        #endregion getPreviousQRCID
                        ObjQRCModel.QRCodeID = QrcArray[0] + QrcArray[1] + QRCStringID;
                        //Commented by Bhushan on 04/25/2017 for as issue in QRC creation of same QRCCodeID.
                        //ObjQRCModel.QRCodeID = QrcArray[0] + QrcArray[1] + QrcArray[2];//(ObjQRCMasterRepository.GetLastQRCID() + 1);
                        ObjQRCModel.QRCImage = ObjQRCModel.QRCodeID + "_" + ObjQRCModel.LocationId + ".png";
                    }
                    ObjQRCMaster = new QRCMaster();
                    //Added by Bhushan Dod on 30/Nov/2016 for 
                    if (ObjQRCModel.VendorName != null && ObjQRCModel.VendorName != "")
                    {
                        ObjQRCModel.VendorName = ObjQRCModel.VendorName.ToTitleCase();
                    }
                    AutoMapper.Mapper.CreateMap<QRCModel, QRCMaster>();
                    ObjQRCMaster = AutoMapper.Mapper.Map(ObjQRCModel, ObjQRCMaster);

                    if (ObjQRCMaster.QRCID == 0)
                    {
                        ObjQRCMaster.CreatedBy = _CreatedBy;
                        ObjQRCMaster.CreatedDate = _createdOn;
                        ObjQRCMaster.ModuleNameId = _moduleNameId;
                        ObjQRCMasterRepository.Add(ObjQRCMaster);
                        found = true;

                    }
                    else
                    {
                        ObjQRCMaster = ObjQRCMasterRepository.GetSingleOrDefault(q => q.QRCID == ObjQRCModel.QRCId && q.IsDeleted == false);
                        if (ObjQRCMaster.QRCID > 0)
                        {
                            ObjQRCMaster.ModifiedBy = _CreatedBy;
                            ObjQRCMaster.ModifiedDate = _createdOn;
                            //ObjQRCMaster.ModuleNameId = _moduleNameId;

                            ObjQRCMaster.QRCName = ObjQRCModel.QRCName;
                            ObjQRCMaster.SpecialNotes = ObjQRCModel.SpecialNotes;

                            ObjQRCMaster.VehicleType = ObjQRCModel.VehicleType;
                            ObjQRCMaster.MotorType = ObjQRCModel.MotorType;


                            //ObjQRCMaster.QRCID = ObjQRCModel.QRCId;
                            ObjQRCMaster.QRCDefaultSize = ObjQRCModel.QRCDefaultSize;

                            //update warranty and other new added field
                            ObjQRCMaster.SerialNo = ObjQRCModel.SerialNo;
                            ObjQRCMaster.Make = ObjQRCModel.Make;
                            ObjQRCMaster.Model = ObjQRCModel.Model;

                            if (ObjQRCModel.LOCPicture != null)
                                ObjQRCMaster.LocationPicture = ObjQRCModel.LocationPicture;
                            if (ObjQRCModel.AssetPictureUrl != null)
                                ObjQRCMaster.AssetPicture = ObjQRCModel.AssetPicture;

                            ObjQRCMaster.VendorName = ObjQRCModel.VendorName;
                            ObjQRCMaster.PointOfContact = ObjQRCModel.PointOfContact;
                            ObjQRCMaster.TelephoneNo = ObjQRCModel.TelephoneNo;
                            ObjQRCMaster.EmialAdd = ObjQRCModel.EmialAdd;
                            if (ObjQRCModel.WarrantyEndDate != null)
                                ObjQRCMaster.WarrantyEndDate = ObjQRCModel.WarrantyEndDate.Value.ToUniversalTime();
                            ObjQRCMaster.Website = ObjQRCModel.Website;
                            ObjQRCMaster.PurchaseType = ObjQRCModel.PurchaseType;
                            if (ObjQRCModel.WarrantyDoc != null)
                            {
                                ObjQRCMaster.WarrantyDoc = ObjQRCModel.WarrantyDoc;
                            }
                            if (ObjQRCModel.InsuranceExpDate != null)
                            {
                                ObjQRCMaster.InsuranceExpDate = ObjQRCModel.InsuranceExpDate.Value.ToUniversalTime();
                            }

                            ObjQRCMaster.AllotedTo = ObjQRCModel.Allotedto;
                            ObjQRCMaster.PurchaseTypeRemark = ObjQRCModel.PurchaseTypeRemark;

                            ObjQRCMasterRepository.Update(ObjQRCMaster);
                            found = true;
                        }
                    }
                }

                AutoMapper.Mapper.CreateMap<QRCMaster, QRCModel>();
                OutObjQRCModel = AutoMapper.Mapper.Map(ObjQRCMaster, OutObjQRCModel);

                OutObjQRCModel.MotorTypeCaption = ObjQRCModel.MotorTypeCaption;
                OutObjQRCModel.VehicleTypeCaption = ObjQRCModel.VehicleTypeCaption;
                OutObjQRCModel.QRCTYPECaption = ObjQRCModel.QRCTYPECaption;
                OutObjQRCModel.SizeCaption = ObjQRCModel.SizeCaption;
                OutObjQRCModel.QRCodeID = ObjQRCModel.QRCodeID;
                OutObjQRCModel.QRCTYPEID = ObjQRCModel.QRCTYPE;

                _qRCid = OutObjQRCModel.QRCId;
            }
            catch (Exception ex)
            {

                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "private bool QRCMasterEntry(QRCModel ObjQRCModel, long _CreatedBy, DateTime _createdOn, long _moduleNameId, out long _qRCid, out QRCModel OutObjQRCModel)", "exception from c# while creting qrc type", ObjQRCModel);
                throw;

            }
            return found;
        }

        /// <summary>DeleteQRC
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-11-2014</CreatedOn>
        /// <CreatedFor>Delete QRC</CreatedFor>
        /// </summary>
        /// <param name="QRCID"></param>
        /// <param name="LoggedInUser"></param>
        /// <returns></returns>
        public Result DeleteQRC(long qrcId, long loggedInUser, DARModel objDAR, string locationName)
        {

            Result result;
            try
            {
                if (qrcId > 0)
                {
                    //if (ChkVendorMappedWithRegisteredVehicle(QRCID))
                    if (true)
                    {
                        ObjQRCMasterRepository = new QRCMasterRepository();
                        var data = ObjQRCMasterRepository.GetSingleOrDefault(q => q.QRCID == qrcId && q.IsDeleted == false);
                        if (data == null) { throw new Exception(CommonMessage.DoesNotExistsRecordMessage()); }
                        data.IsDeleted = true;
                        data.DeletedBy = loggedInUser;
                        data.DeletedDate = DateTime.UtcNow;
                        ObjQRCMasterRepository.Update(data);

                        objDAR.ActivityDetails = DarMessage.DeleteQRC(data.QRCName, locationName);

                        #region Save DAR
                        result = _ICommonMethod.SaveDAR(objDAR);
                        #endregion Save DAR

                        return Result.Delete;
                    }
                    else
                    { return Result.Failed; }
                }
                else { return Result.DoesNotExist; }


            }
            catch (Exception)
            { throw; }
        }

        #region For Service QRCType Details

        /// <summary>Get QRCID Details
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>GetQRCDeatilsByID</CreatedFor>
        /// <CreatedOn>Jan-27-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCElevatorModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<QRCModel> GetQRCDetailsByID(ServiceQrcModel ObjServiceQRCModel)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ObjQRCMasterRepository = new QRCMasterRepository();
            objQRCScanLogRepository = new QRCScanLogRepository();
            ObjUserRepository = new UserRepository();
            objDARRepository = new DARRepository();
            ServiceDARModel obj = new ServiceDARModel();
            ServiceResponseModel<QRCModel> ObjServiceResponseModel = new ServiceResponseModel<QRCModel>();
            try
            {
                if (ObjServiceQRCModel != null && ObjServiceQRCModel.QrcId > 0 && ObjServiceQRCModel.LocationId > 0)
                {

                    var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == ObjServiceQRCModel.ServiceAuthKey && x.IsDeleted == false);
                    if (authuser != null && authuser.UserId > 0)
                    {
                        var result = ObjQRCSetupManager.GetQrcById(Convert.ToInt64(ObjServiceQRCModel.QrcId), ObjServiceQRCModel.LocationId);

                        ObjServiceResponseModel.Message = (result != null && result.QRCId > 0) ? CommonMessage.Successful() : CommonMessage.DoesNotExistsRecordMessage();
                        ObjServiceResponseModel.Response = (result != null && result.QRCId > 0) ? Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Data = result;
                        if (result != null)
                        {
                            //For Scan log maintian
                            long QrcScanLogId = objQRCScanLogRepository.SaveQRCScanLog(ObjServiceResponseModel, authuser.UserId, Convert.ToInt64(result.LocationId));
                            ObjServiceResponseModel.Data.QrcScanLogId = QrcScanLogId;

                            //For DAR log maintian
                            obj.CreatedBy = authuser.UserId;
                            obj.ActivityDetails = DarMessage.QRCScanMessage((authuser.FirstName + " " + authuser.LastName), result.QRCName, result.QRCodeID);
                            obj.LocationId = Convert.ToInt64(result.LocationId);
                            obj.UserId = authuser.UserId;
                            obj.TaskType = (long)DARTASKTYPECATEGORY.QRCScan;
                            long DarId = objDARRepository.SaveDARDetails(obj);
                            ObjServiceResponseModel.Data.DarId = DarId;
                        }
                    }
                    else
                    {

                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.InvalidSessionResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    }
                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ObjServiceResponseModel;
        }

        /// <summary>Save QRC Elevator Request Details
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveQRCRequestDetails</CreatedFor>
        /// <CreatedOn>Jan-20-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCElevatorModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> SaveQrcElevatorRequestDetails(ServiceQrcElevatorModel obj)
        {
            ObjQRCMasterRepository = new QRCMasterRepository();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            List<EmailToManagerModel> objEmailReturn = new List<EmailToManagerModel>();
            try
            {
                if (obj != null)
                {
                    string ToXml;
                    ToXml = GenericDataContractSerializer<ServiceQrcElevatorModel>.SerializeObject(obj);

                    var result = ObjQRCMasterRepository.QRCSave(obj.ServiceAuthKey, obj.UserId, obj.QrcId, ToXml, obj.Action, true, false, null);

                    ObjServiceResponseModel.Response = (result != null) ? Convert.ToInt64(result.Data.Response, CultureInfo.InvariantCulture) : Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = (result != null) ? (result.Data.ResponseMessage).ToString() : CommonMessage.NoRecordMessage();//CommonMessage.MessageLogout();
                    if (ObjServiceResponseModel.Response == 1)
                    {
                        objEmailReturn = objEmailLogRepository.SendEmailToManagerForItemMissingQRC(obj.LocationId, obj.UserId);
                    }
                    if (obj.PmCheck.Inspection == false && ObjServiceResponseModel.Response == 1)
                    {
                        ObjCommonRepository = new CommonRepository();
                        foreach (var item in objEmailReturn)
                        {
                            EmailHelper objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = item.ManagerEmail;
                            objEmailHelper.ManagerName = item.ManagerName;
                            objEmailHelper.LocationName = item.LocationName;
                            objEmailHelper.UserName = obj.UserName;
                            objEmailHelper.MailType = "QRCELEVATORINSPECTION";
                            objEmailHelper.SentBy = item.RequestBy;
                            objEmailHelper.QrCodeId = obj.QrcodeId;
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
                                catch (Exception ex)
                                {
                                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public ServiceResponseModel<string> SaveQrcElevatorRequestDetails(ServiceQrcElevatorModel obj)", "LocationID", "QRCELEVATORINSPECTION");
                                }
                            }
                            message = PushNotificationMessages.ElevatorInspection(item.LocationName, obj.QrcodeId, obj.UserName);
                            PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
                        }
                    }
                    if (obj.PmCheck.Capacity == false && ObjServiceResponseModel.Response == 1)
                    {
                        ObjCommonRepository = new CommonRepository();
                        foreach (var item in objEmailReturn)
                        {
                            EmailHelper objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = item.ManagerEmail;
                            objEmailHelper.ManagerName = item.ManagerName;
                            objEmailHelper.LocationName = item.LocationName;
                            objEmailHelper.UserName = obj.UserName;
                            objEmailHelper.MailType = "QRCELEVATORCAPACITY";
                            objEmailHelper.SentBy = item.RequestBy;
                            objEmailHelper.QrCodeId = obj.QrcodeId;
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
                                catch (Exception ex)
                                {
                                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public ServiceResponseModel<string> SaveQrcElevatorRequestDetails(ServiceQrcElevatorModel obj)", "LocationID", "QRCELEVATORCAPACITY");
                                }
                            }
                            message = PushNotificationMessages.ElevatorCapacity(item.LocationName, obj.QrcodeId, obj.UserName);
                            PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
                        }
                    }

                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public ServiceResponseModel<string> SaveQrcElevatorRequestDetails(ServiceQrcElevatorModel obj)", "LocationID", obj.LocationId);
            }
            return ObjServiceResponseModel;
        }

        /// <summary>Save QRC BathRoom Request Details
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveQRCBathroomRequestDetails</CreatedFor>
        /// <CreatedOn>Jan-23-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCBathRoomModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> SaveQrcBathroomRequestDetails(ServiceQrcBathroomModel obj)
        {
            ObjQRCMasterRepository = new QRCMasterRepository();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (obj != null)
                {
                    string ToXml;
                    ToXml = GenericDataContractSerializer<ServiceQrcBathroomModel>.SerializeObject(obj);

                    var result = ObjQRCMasterRepository.QRCSave(obj.ServiceAuthKey, obj.UserId, obj.QrcId, ToXml, obj.Action, true, false, null);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt64(result.Data.Response, CultureInfo.InvariantCulture);
                        ObjServiceResponseModel.Message = (result.Data.ResponseMessage).ToString();//CommonMessage.MessageLogout();
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                    }
                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public ServiceResponseModel<string> SaveQrcBathroomRequestDetails(ServiceQrcBathroomModel obj)", "UserId", obj.UserId);
            }
            return ObjServiceResponseModel;
        }

        /// <summary>Save QRC Equipment Request Details
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveQRCEquipmentRequestDetails</CreatedFor>
        /// <CreatedOn>Jan-23-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCEquipmentModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> SaveQrcEquipmentRequestDetails(ServiceQrcEquipmentModel obj)
        {
            ObjQRCMasterRepository = new QRCMasterRepository();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (obj != null)
                {
                    string ToXml;
                    ToXml = GenericDataContractSerializer<ServiceQrcEquipmentModel>.SerializeObject(obj);

                    var result = ObjQRCMasterRepository.QRCSave(obj.ServiceAuthKey, obj.UserId, obj.QrcId, ToXml, obj.Action, obj.CheckOutStatus, false, obj.UserName);

                    ObjServiceResponseModel.Response = (result != null) ? Convert.ToInt64(result.Data.Response, CultureInfo.InvariantCulture) : Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = (result != null) ? (result.Data.ResponseMessage).ToString() : CommonMessage.NoRecordMessage();//CommonMessage.MessageLogout();
                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " ServiceResponseModel<string> SaveQrcEquipmentRequestDetails(ServiceQrcEquipmentModel obj)", "UserId", obj.UserId);
            }
            return ObjServiceResponseModel;
        }

        /// <summary>Save QRC Cellphone Request Details
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveQRCCellPhoneRequestDetails</CreatedFor>
        /// <CreatedOn>Jan-23-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCCellphoneModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> SaveQrcCellPhoneRequestDetails(ServiceQrcCellphoneModel obj)
        {
            ObjQRCMasterRepository = new QRCMasterRepository();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (obj != null)
                {
                    string ToXml;
                    ToXml = GenericDataContractSerializer<ServiceQrcCellphoneModel>.SerializeObject(obj);

                    var result = ObjQRCMasterRepository.QRCSave(obj.ServiceAuthKey, obj.UserId, obj.QrcId, ToXml, obj.Action, obj.Status, false, obj.UserName);

                    ObjServiceResponseModel.Response = (result != null) ? Convert.ToInt64(result.Data.Response, CultureInfo.InvariantCulture) : Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = (result != null) ? (result.Data.ResponseMessage).ToString() : CommonMessage.NoRecordMessage();//CommonMessage.MessageLogout();
                    if (obj.CheckingIn.ScreenCracked == true &&
                        obj.CheckingIn.ScreenCracked != null &&
                        ObjServiceResponseModel.Response == 1)
                    {
                        List<ssp_SendEmailForCellPhone_Result> objEmailReturn = new List<ssp_SendEmailForCellPhone_Result>();

                        objEmailReturn = objEmailLogRepository.SendEmailToManagerForCellphone(obj.QrcId, obj.UserId);

                        foreach (var item in objEmailReturn)
                        {
                            EmailHelper objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = item.ManagerEmail;
                            objEmailHelper.ManagerName = item.ManagerName;
                            objEmailHelper.ProblemDesc = "Screen is cracked";
                            objEmailHelper.LocationName = item.LocationName;
                            objEmailHelper.UserName = item.UserName;
                            objEmailHelper.MailType = "QRCCELLPHONE";
                            objEmailHelper.SentBy = item.RequestBy;
                            objEmailHelper.QrCodeId = item.QRCodeID;
                            objEmailHelper.QrcName = item.QRCName;
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
                                catch (Exception ex)
                                {
                                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, " ServiceResponseModel<string> SaveQrcEquipmentRequestDetails(ServiceQrcEquipmentModel obj)", "UserId", obj.UserId);
                                }
                            }
                            message = PushNotificationMessages.CellphoneScreenCracked(item.LocationName);
                            PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
                        }
                    }
                    if (obj.CheckingIn.Buttons == false &&
                       obj.CheckingIn.Buttons != null &&
                       ObjServiceResponseModel.Response == 1)
                    {
                        List<ssp_SendEmailForCellPhone_Result> objEmailReturn = new List<ssp_SendEmailForCellPhone_Result>();

                        objEmailReturn = objEmailLogRepository.SendEmailToManagerForCellphone(obj.QrcId, obj.UserId);
                        foreach (var item in objEmailReturn)
                        {
                            EmailHelper objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = item.ManagerEmail;
                            objEmailHelper.ManagerName = item.ManagerName;
                            objEmailHelper.ProblemDesc = "All Buttons are not present";
                            objEmailHelper.LocationName = item.LocationName;
                            objEmailHelper.UserName = item.UserName;
                            objEmailHelper.MailType = "QRCCELLPHONEBUTTONS";
                            objEmailHelper.SentBy = item.RequestBy;
                            objEmailHelper.QrCodeId = item.QRCodeID;
                            objEmailHelper.QrcName = item.QRCName;
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
                            message = PushNotificationMessages.CellphoneButtonsNotPresent(item.LocationName);
                            PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
                        }
                    }

                    if (obj.CheckingOut.FullyFunctional == false &&
                      obj.CheckingOut.FullyFunctional != null &&
                      ObjServiceResponseModel.Response == 1)
                    {
                        List<ssp_SendEmailForCellPhone_Result> objEmailReturn = new List<ssp_SendEmailForCellPhone_Result>();

                        objEmailReturn = objEmailLogRepository.SendEmailToManagerForCellphone(obj.QrcId, obj.UserId);
                        foreach (var item in objEmailReturn)
                        {
                            EmailHelper objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = item.ManagerEmail;
                            objEmailHelper.ManagerName = item.ManagerName;
                            objEmailHelper.ProblemDesc = "Cell phone is not functional properly";
                            objEmailHelper.LocationName = item.LocationName;
                            objEmailHelper.UserName = item.UserName;
                            objEmailHelper.MailType = "QRCCELLPHONEFUNCTIONAL";
                            objEmailHelper.SentBy = item.RequestBy;
                            objEmailHelper.QrCodeId = item.QRCodeID;
                            objEmailHelper.QrcName = item.QRCName;
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
                            message = PushNotificationMessages.CellphoneNotFunctional(item.LocationName);
                            PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
                        }
                    }

                    if (obj.CheckingOut.ScreenCracked == false &&
                        obj.CheckingOut.ScreenCracked != null &&
                        ObjServiceResponseModel.Response == 1)
                    {
                        List<ssp_SendEmailForCellPhone_Result> objEmailReturn = new List<ssp_SendEmailForCellPhone_Result>();

                        objEmailReturn = objEmailLogRepository.SendEmailToManagerForCellphone(obj.QrcId, obj.UserId);
                        foreach (var item in objEmailReturn)
                        {
                            EmailHelper objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = item.ManagerEmail;
                            objEmailHelper.ManagerName = item.ManagerName;
                            objEmailHelper.ProblemDesc = "Screen is cracked";
                            objEmailHelper.LocationName = item.LocationName;
                            objEmailHelper.UserName = item.UserName;
                            objEmailHelper.MailType = "QRCCELLPHONE";
                            objEmailHelper.SentBy = item.RequestBy;
                            objEmailHelper.QrCodeId = item.QRCodeID;
                            objEmailHelper.QrcName = item.QRCName;
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
                            message = PushNotificationMessages.CellphoneScreenCracked(item.LocationName);
                            PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
                        }
                    }
                    if (obj.CheckingOut.Buttons == true &&
                       obj.CheckingOut.Buttons != null &&
                       ObjServiceResponseModel.Response == 1)
                    {
                        List<ssp_SendEmailForCellPhone_Result> objEmailReturn = new List<ssp_SendEmailForCellPhone_Result>();

                        objEmailReturn = objEmailLogRepository.SendEmailToManagerForCellphone(obj.QrcId, obj.UserId);
                        foreach (var item in objEmailReturn)
                        {
                            EmailHelper objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = item.ManagerEmail;
                            objEmailHelper.ManagerName = item.ManagerName;
                            objEmailHelper.ProblemDesc = "All Buttons are present";
                            objEmailHelper.LocationName = item.LocationName;
                            objEmailHelper.UserName = item.UserName;
                            objEmailHelper.MailType = "QRCCELLPHONEBUTTONS";
                            objEmailHelper.SentBy = item.RequestBy;
                            objEmailHelper.QrCodeId = item.QRCodeID;
                            objEmailHelper.QrcName = item.QRCName;
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
                            message = PushNotificationMessages.CellphoneButtonsNotPresent(item.LocationName);
                            PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
                        }
                    }
                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ObjServiceResponseModel;
        }

        /// <summary>Save QRC Escalators Request Details
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveQRCEscalatorsRequestDetails</CreatedFor>
        /// <CreatedOn>Jan-23-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCEscalatorsModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> SaveQrcEscalatorsRequestDetails(ServiceQrcEscalatorsModel obj)
        {
            ObjQRCMasterRepository = new QRCMasterRepository();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (obj != null)
                {
                    string ToXml;
                    ToXml = GenericDataContractSerializer<ServiceQrcEscalatorsModel>.SerializeObject(obj);

                    var result = ObjQRCMasterRepository.QRCSave(obj.ServiceAuthKey, obj.UserId, obj.QrcId, ToXml, obj.Action, true, false, null);

                    ObjServiceResponseModel.Response = (result != null) ? Convert.ToInt64(result.Data.Response, CultureInfo.InvariantCulture) : Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = (result != null) ? (result.Data.ResponseMessage).ToString(CultureInfo.InvariantCulture) : CommonMessage.NoRecordMessage();//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ObjServiceResponseModel;
        }

        /// <summary>Save QRC MovingWalkway Request Details
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveQRCMovingWalkwayRequestDetails</CreatedFor>
        /// <CreatedOn>Jan-23-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCMovingWalkwayModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> SaveQrcMovingWalkwayRequestDetails(ServiceQrcMovingWalkwayModel obj)
        {
            ObjQRCMasterRepository = new QRCMasterRepository();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (obj != null)
                {
                    string ToXml;
                    ToXml = GenericDataContractSerializer<ServiceQrcMovingWalkwayModel>.SerializeObject(obj);

                    var result = ObjQRCMasterRepository.QRCSave(obj.ServiceAuthKey, obj.UserId, obj.QrcId, ToXml, obj.Action, true, false, null);

                    ObjServiceResponseModel.Response = (result != null) ? Convert.ToInt64(result.Data.Response, CultureInfo.InvariantCulture) : Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = (result != null) ? (result.Data.ResponseMessage).ToString(CultureInfo.InvariantCulture) : CommonMessage.NoRecordMessage();//CommonMessage.MessageLogout();
                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ObjServiceResponseModel;
        }

        /// <summary>Save QRC Parking Request Details
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveQRCParkingRequestDetails</CreatedFor>
        /// <CreatedOn>Jan-28-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCParkingModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> SaveQrcParkingRequestDetails(ServiceQrcParkingModel obj)
        {
            ObjQRCMasterRepository = new QRCMasterRepository();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {

                if (obj != null)
                {
                    string ToXml;
                    ToXml = GenericDataContractSerializer<ServiceQrcParkingModel>.SerializeObject(obj);

                    var result = ObjQRCMasterRepository.QRCSave(obj.ServiceAuthKey, obj.UserId, obj.QrcId, ToXml, obj.Action, true, false, null);

                    ObjServiceResponseModel.Response = (result != null) ? Convert.ToInt64(result.Data.Response, CultureInfo.InvariantCulture) : Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = (result != null) ? (result.Data.ResponseMessage).ToString(CultureInfo.InvariantCulture) : CommonMessage.NoRecordMessage();//CommonMessage.MessageLogout();
                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ObjServiceResponseModel;
        }

        /// <summary>Save QRC TrashCan Request Details
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveQRCTrashCanRequestDetails</CreatedFor>
        /// <CreatedOn>Jan-28-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCTrashCanModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> SaveQrcTrashCanRequestDetails(ServiceQrcTrashcanModel obj)
        {
            ObjQRCMasterRepository = new QRCMasterRepository();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (obj != null)
                {
                    string ToXml;
                    ToXml = GenericDataContractSerializer<ServiceQrcTrashcanModel>.SerializeObject(obj);

                    var result = ObjQRCMasterRepository.QRCSave(obj.ServiceAuthKey, obj.UserId, obj.QrcId, ToXml, obj.Action, true, false, null);

                    ObjServiceResponseModel.Response = (result != null) ? Convert.ToInt64(result.Data.Response, CultureInfo.InvariantCulture) : Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = (result != null) ? (result.Data.ResponseMessage).ToString(CultureInfo.InvariantCulture) : CommonMessage.NoRecordMessage();//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ObjServiceResponseModel;
        }

        /// <summary>Save QRC GateArm Request Details
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveQRCGateArmRequestDetails</CreatedFor>
        /// <CreatedOn>Jan-28-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCGateArmModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> SaveQrcGateArmRequestDetails(ServiceQrcGateArmModel obj)
        {
            ObjQRCMasterRepository = new QRCMasterRepository();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (obj.ServiceAuthKey != null)
                {
                    string ToXml;
                    ToXml = GenericDataContractSerializer<ServiceQrcGateArmModel>.SerializeObject(obj);

                    var result = ObjQRCMasterRepository.QRCSave(obj.ServiceAuthKey, obj.UserId, obj.QrcId, ToXml, obj.Action, true, false, null);

                    ObjServiceResponseModel.Response = (result != null) ? Convert.ToInt64(result.Data.Response, CultureInfo.InvariantCulture) : Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = (result != null) ? (result.Data.ResponseMessage).ToString(CultureInfo.InvariantCulture) : CommonMessage.NoRecordMessage();//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ObjServiceResponseModel;
        }

        /// <summary>Save QRC TicketSpitter  Request Details
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveQRCTicketSpitterRequestDetails</CreatedFor>
        /// <CreatedOn>Jan-28-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCTicketSpitterModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> SaveQrcTicketSpitterRequestDetails(ServiceQrcTicketSplitterModel obj)
        {
            ObjQRCMasterRepository = new QRCMasterRepository();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (obj.ServiceAuthKey != null)
                {
                    string ToXml;
                    ToXml = GenericDataContractSerializer<ServiceQrcTicketSplitterModel>.SerializeObject(obj);

                    var result = ObjQRCMasterRepository.QRCSave(obj.ServiceAuthKey, obj.UserId, obj.QrcId, ToXml, obj.Action, true, false, null);

                    ObjServiceResponseModel.Response = (result != null) ? Convert.ToInt64(result.Data.Response, CultureInfo.InvariantCulture) : Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = (result != null) ? (result.Data.ResponseMessage).ToString(CultureInfo.InvariantCulture) : CommonMessage.NoRecordMessage();//CommonMessage.MessageLogout();
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ObjServiceResponseModel;
        }

        /// <summary>Save QRC BusStation  Request Details
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveQRCBusStationRequestDetails</CreatedFor>
        /// <CreatedOn>Jan-28-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCTicketSpitterModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> SaveQrcBusStationRequestDetails(ServiceQrcBusStationModel obj)
        {
            ObjQRCMasterRepository = new QRCMasterRepository();
            ServiceResponseModel<string> objServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (obj.ServiceAuthKey != null)
                {
                    string ToXml;
                    ToXml = GenericDataContractSerializer<ServiceQrcBusStationModel>.SerializeObject(obj);

                    var result = ObjQRCMasterRepository.QRCSave(obj.ServiceAuthKey, obj.UserId, obj.QrcId, ToXml, obj.Action, true, false, null);

                    objServiceResponseModel.Response = (result != null) ? Convert.ToInt64(result.Data.Response, CultureInfo.InvariantCulture) : Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                    objServiceResponseModel.Message = (result != null) ? (result.Data.ResponseMessage).ToString(CultureInfo.InvariantCulture) : CommonMessage.NoRecordMessage();//CommonMessage.MessageLogout();
                }
                else
                {

                    objServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    objServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return objServiceResponseModel;
        }

        /// <summary>Save QRC Emergency Phone Systems  Request Details
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>SaveQRCPhoneSystemsRequestDetails</CreatedFor>
        /// <CreatedOn>Jan-28-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCPhoneSystemModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> SaveQrcPhoneSystemsRequestDetails(ServiceQrcPhoneSystemModel obj)
        {
            ObjQRCMasterRepository = new QRCMasterRepository();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            try
            {
                if (obj != null)
                {
                    string ToXml;
                    ToXml = GenericDataContractSerializer<ServiceQrcPhoneSystemModel>.SerializeObject(obj);

                    var result = ObjQRCMasterRepository.QRCSave(obj.ServiceAuthKey, obj.UserId, obj.QrcId, ToXml, obj.Action, true, false, null);

                    ObjServiceResponseModel.Response = (result != null) ? Convert.ToInt64(result.Data.Response, CultureInfo.CurrentCulture) : Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = (result != null) ? (result.Data.ResponseMessage).ToString() : CommonMessage.NoRecordMessage();//CommonMessage.MessageLogout();
                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ObjServiceResponseModel;
        }

        /// <summary>Save QRC Vehicle Request
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>QrcVehicleRequestDetails</CreatedFor>
        /// <CreatedOn>Feb-13-2015</CreatedOn>
        /// </summary>
        /// <param name="objServiceQrcVehicleModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> SaveQrcVehicleRequestDetails(ServiceQrcVehicleModel obj)
        {
            ObjQRCMasterRepository = new QRCMasterRepository();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            List<EmailToManagerModel> objEmailReturn = new List<EmailToManagerModel>();
            List<EmailLog> objListEmailog = new List<EmailLog>();
            try
            {
                if (obj.ServiceAuthKey != null)
                {
                    string ToXml;
                    ToXml = GenericDataContractSerializer<ServiceQrcVehicleModel>.SerializeObject(obj);

                    var result = ObjQRCMasterRepository.QRCSave(obj.ServiceAuthKey, obj.UserId, obj.QrcId, ToXml, obj.Action, obj.CheckingOut.VehicleMileage, obj.DamageStatus, obj.UserName);
                    ObjServiceResponseModel.Response = (result != null) ? Convert.ToInt64(result.Data.Response, CultureInfo.InvariantCulture) : Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = (result != null) ? (result.Data.ResponseMessage).ToString(CultureInfo.InvariantCulture) : CommonMessage.NoRecordMessage();//CommonMessage.MessageLogout();
                    if (ObjServiceResponseModel.Response == 1)
                    {
                        objEmailReturn = objEmailLogRepository.SendEmailToManagerForItemMissingQRC(obj.LocationId, obj.UserId);
                    }

                    if (objEmailReturn.Count > 0 && ObjServiceResponseModel.Response == 1)
                    {
                        foreach (var item in objEmailReturn)
                        {
                            if (obj.CheckingOut.IsItem == false &&
                             obj.CheckingOut.ItemDescription != null &&
                            string.IsNullOrEmpty(obj.CheckingOut.ItemDescription) == false)
                            {
                                EmailHelper objEmailHelper = new EmailHelper();
                                objEmailHelper.emailid = item.ManagerEmail;
                                objEmailHelper.ManagerName = item.ManagerName;
                                //Note:- Here problem desc is sent by Item Missing Description
                                objEmailHelper.ProblemDesc = obj.CheckingOut.ItemDescription;
                                objEmailHelper.LocationName = item.LocationName;
                                objEmailHelper.QrCodeId = obj.QrcodeId;
                                objEmailHelper.UserName = item.UserName;
                                objEmailHelper.MailType = "QRCVEHICLEITEMMISSING";
                                objEmailHelper.SentBy = item.RequestBy;
                                objEmailHelper.LocationID = item.LocationID;


                                bool IsSent = objEmailHelper.SendEmailWithTemplate();
                                //Push Notification
                                message = PushNotificationMessages.ItemMissing(item.LocationName);
                                PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
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

                                        objListEmailog.Add(objEmailog);
                                        //objEmailLogRepository.SaveEmailLog(objEmailog);
                                    }
                                    catch (Exception)
                                    {
                                        throw;
                                    }
                                }
                            } // CheckingOut.IsItem == false end here
                            if (obj.CheckingOut.IsDamage == true)
                            {
                                bool IsSent = false;
                                //ObjCommonRepository = new CommonRepository();
                                EmailHelper objEmailHelper = new EmailHelper();
                                objEmailHelper.emailid = item.ManagerEmail;
                                objEmailHelper.ManagerName = item.ManagerName;
                                //Note:- Here problem desc is sent by Item Missing Description
                                objEmailHelper.ProblemDesc = obj.CheckingOut.ItemDescription;
                                objEmailHelper.LocationName = item.LocationName;
                                objEmailHelper.UserName = item.UserName;
                                objEmailHelper.QrCodeId = obj.QrcodeId;
                                objEmailHelper.MailType = "QRCVEHICLEPARTDAMAGE";
                                objEmailHelper.SentBy = item.RequestBy;
                                objEmailHelper.LocationID = item.LocationID;
                                objEmailHelper.TimeAttempted = DateTime.UtcNow.ToClientTimeZone(true).ToString();

                                if (obj.AllPictures != null && obj.AllPictures.Trim() != "")
                                {
                                    obj.AllPictures = obj.AllPictures.Replace(" ", "");
                                    var list1 = obj.AllPictures.Split(',').ToList();
                                    string[] attachFiles = new string[list1.Count()];
                                    for (var i = 0; i < list1.Count(); i++)
                                    {
                                        //attachFiles[i] = HttpConfigurationManager.AppSettings["hostingPrefix"] + "Content/Images/QRCVehicle/" + list1[i];
                                        attachFiles[i] = HttpContext.Current.Server.MapPath("~/Content/Images/QRCVehicle/" + list1[i]);
                                    }
                                    IsSent = objEmailHelper.SendEmailWithTemplate(attachFiles);
                                }
                                else
                                {
                                    IsSent = objEmailHelper.SendEmailWithTemplate();
                                }
                                //Push Notification
                                message = PushNotificationMessages.VehiclePartDamage(item.LocationName);
                                PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
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
                                        objListEmailog.Add(objEmailog);
                                        //objEmailLogRepository.SaveEmailLog(objEmailog);
                                    }
                                    catch (Exception)
                                    {
                                        throw;
                                    }
                                }
                            } //obj.CheckingOut.IsDamage == true end here
                            if (obj.VehicleCheck.IsDamage == true)
                            {
                                bool IsSent = false;
                                EmailHelper objEmailHelper = new EmailHelper();
                                objEmailHelper.emailid = item.ManagerEmail;
                                objEmailHelper.ManagerName = item.ManagerName;
                                //Note:- Here problem desc is sent by Item Missing Description
                                objEmailHelper.ProblemDesc = obj.CheckingOut.ItemDescription;
                                objEmailHelper.LocationName = item.LocationName;
                                objEmailHelper.UserName = item.UserName;
                                objEmailHelper.MailType = "QRCVEHICLEPARTDAMAGEWEEKLY";
                                objEmailHelper.SentBy = item.RequestBy;
                                objEmailHelper.QrCodeId = obj.QrcodeId;
                                objEmailHelper.LocationID = item.LocationID;
                                objEmailHelper.TimeAttempted = DateTime.UtcNow.ToClientTimeZone(true).ToString();


                                if (obj.AllPictures != null && obj.AllPictures.Trim() != "")
                                {
                                    obj.AllPictures = obj.AllPictures.Replace(" ", "");
                                    var list1 = obj.AllPictures.Split(',').ToList();
                                    string[] attachFiles = new string[list1.Count()];
                                    for (var i = 0; i < list1.Count(); i++)
                                    {
                                        //attachFiles[i] = HttpConfigurationManager.AppSettings["hostingPrefix"] + "Content/Images/QRCVehicle/" + list1[i];
                                        attachFiles[i] = HttpContext.Current.Server.MapPath("~/Content/Images/QRCVehicle/" + list1[i]);

                                    }
                                    IsSent = objEmailHelper.SendEmailWithTemplate(attachFiles);
                                }
                                else
                                {
                                    IsSent = objEmailHelper.SendEmailWithTemplate();
                                }
                                //Push Notification
                                message = PushNotificationMessages.WeeklyVehicleCheck(item.LocationName);
                                PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
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
                                        objListEmailog.Add(objEmailog);
                                        //objEmailLogRepository.SaveEmailLog(objEmailog);
                                    }
                                    catch (Exception)
                                    {
                                        throw;
                                    }
                                }

                            } //obj.VehicleCheck.IsDamage == true end here

                            if (obj.CheckingOut.VehicleMileage == false &&
                                  obj.CheckingOut.VmDescription != null &&
                                 string.IsNullOrEmpty(obj.CheckingOut.VmDescription) == false &&
                                 string.IsNullOrEmpty(obj.CheckingOut.OldVmDescription) == false)
                            {
                                //  ObjCommonRepository = new CommonRepository();
                                EmailHelper objEmailHelper = new EmailHelper();
                                objEmailHelper.emailid = item.ManagerEmail;
                                objEmailHelper.ManagerName = item.ManagerName;
                                //Note:- Here Action for vehicle mileage
                                objEmailHelper.CurrentMileage = obj.CheckingOut.VmDescription;
                                objEmailHelper.LocationName = item.LocationName;
                                objEmailHelper.UserName = item.UserName;
                                objEmailHelper.MailType = "QRCVEHICLEMILEAGECO";
                                objEmailHelper.SentBy = item.RequestBy;
                                objEmailHelper.LocationID = item.LocationID;
                                objEmailHelper.QrCodeId = obj.QrcodeId;
                                objEmailHelper.PreviousMileage = obj.CheckingOut.OldVmDescription;
                                objEmailHelper.Action = "check out.";

                                bool IsSent = objEmailHelper.SendEmailWithTemplate();
                                //Push Notification
                                message = PushNotificationMessages.VehicleMileage(item.LocationName);
                                PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
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
                                        objListEmailog.Add(objEmailog);
                                        //objEmailLogRepository.SaveEmailLog(objEmailog);
                                    }
                                    catch (Exception)
                                    {
                                        throw;
                                    }
                                }
                            }//obj.CheckingOut.VehicleMileage == false end here
                            //Commented on 24/10/2015 by Bhushan Dod for client(Robert don't want to receive push)
                            //if (obj.CheckingIn.ChVehicleMileage == false &&
                            //    obj.CheckingIn.ChVmDescription != null &&
                            //    string.IsNullOrEmpty(obj.CheckingIn.ChVmDescription) == false &&
                            //    string.IsNullOrEmpty(obj.CheckingIn.OldChVmDescription) == false)
                            //{
                            //    //  ObjCommonRepository = new CommonRepository();
                            //    EmailHelper objEmailHelper = new EmailHelper();
                            //    objEmailHelper.emailid = item.ManagerEmail;
                            //    objEmailHelper.ManagerName = item.ManagerName;
                            //    //Note:- Here Action for vehicle mileage
                            //    objEmailHelper.CurrentMileage = obj.CheckingIn.ChVmDescription;
                            //    objEmailHelper.LocationName = item.LocationName;
                            //    objEmailHelper.UserName = item.UserName;
                            //    objEmailHelper.MailType = "QRCVEHICLEMILEAGECO";
                            //    objEmailHelper.SentBy = item.RequestBy;
                            //    objEmailHelper.LocationID = item.LocationID;
                            //    objEmailHelper.PreviousMileage = obj.CheckingIn.OldChVmDescription;
                            //    objEmailHelper.Action = "check in.";

                            //    bool IsSent = objEmailHelper.SendEmailWithTemplate();
                            //    //Push Notification
                            //    message = PushNotificationMessages.VehicleMileage(item.LocationName);
                            //    PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
                            //    if (IsSent == true)
                            //    {
                            //        objEmailog = new EmailLog();
                            //        try
                            //        {
                            //            objEmailog.CreatedBy = item.RequestBy;
                            //            objEmailog.CreatedDate = DateTime.Now;
                            //            objEmailog.DeletedBy = null;
                            //            objEmailog.DeletedOn = null;
                            //            objEmailog.LocationId = item.LocationID;
                            //            objEmailog.ModifiedBy = null;
                            //            objEmailog.ModifiedOn = null;
                            //            objEmailog.SentBy = item.RequestBy;
                            //            objEmailog.SentEmail = item.ManagerEmail;
                            //            objEmailog.Subject = objEmailHelper.Subject;
                            //            objEmailog.SentTo = item.ManagerUserId;
                            //            objListEmailog.Add(objEmailog);
                            //            //objEmailLogRepository.SaveEmailLog(objEmailog);
                            //        }
                            //        catch (Exception)
                            //        {
                            //            throw;
                            //        }
                            //    }
                            //}  //obj.CheckingIn.ChVehicleMileage == false end here

                            if (obj.VehicleFuel.VfVehicleMileage == false &&
                            obj.VehicleFuel.VfVmDescription != null &&
                           string.IsNullOrEmpty(obj.VehicleFuel.VfVmDescription) == false &&
                           string.IsNullOrEmpty(obj.VehicleFuel.OldVfVmDescription) == false)
                            {
                                EmailHelper objEmailHelper = new EmailHelper();
                                objEmailHelper.emailid = item.ManagerEmail;
                                objEmailHelper.ManagerName = item.ManagerName;
                                //Note:- Here Action for vehicle mileage
                                objEmailHelper.CurrentMileage = obj.CheckingIn.ChVmDescription;
                                objEmailHelper.LocationName = item.LocationName;
                                objEmailHelper.UserName = item.UserName;
                                objEmailHelper.MailType = "QRCVEHICLEMILEAGECO";
                                objEmailHelper.SentBy = item.RequestBy;
                                objEmailHelper.LocationID = item.LocationID;
                                objEmailHelper.QrCodeId = obj.QrcodeId;
                                objEmailHelper.PreviousMileage = obj.CheckingIn.OldChVmDescription;
                                objEmailHelper.Action = "vehicle fueling.";

                                bool IsSent = objEmailHelper.SendEmailWithTemplate();
                                //Push Notification
                                message = PushNotificationMessages.VehicleMileage(item.LocationName);
                                PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
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
                                        objListEmailog.Add(objEmailog);
                                        // objEmailLogRepository.SaveEmailLog(objEmailog);
                                    }
                                    catch (Exception)
                                    {
                                        throw;
                                    }
                                }
                            }//obj.VehicleFuel.VfVehicleMileage == false end here
                            //Commented on 24/10/2015 by Bhushan Dod for client(Robert don't want to receive push)
                            //if (obj.VehicleCheck.WVehicleMileage == false &&
                            // obj.VehicleCheck.WVmDescription != null &&
                            //string.IsNullOrEmpty(obj.VehicleCheck.WVmDescription) == false &&
                            //string.IsNullOrEmpty(obj.VehicleCheck.OldWVmDescription) == false)
                            //{
                            //    EmailHelper objEmailHelper = new EmailHelper();
                            //    objEmailHelper.emailid = item.ManagerEmail;
                            //    objEmailHelper.ManagerName = item.ManagerName;
                            //    //Note:- Here Action for vehicle mileage
                            //    objEmailHelper.CurrentMileage = obj.CheckingIn.ChVmDescription;
                            //    objEmailHelper.LocationName = item.LocationName;
                            //    objEmailHelper.UserName = item.UserName;
                            //    objEmailHelper.MailType = "QRCVEHICLEMILEAGECO";
                            //    objEmailHelper.SentBy = item.RequestBy;
                            //    objEmailHelper.LocationID = item.LocationID;
                            //    objEmailHelper.PreviousMileage = obj.CheckingIn.OldChVmDescription;
                            //    objEmailHelper.Action = "weekly vehicle check.";

                            //    bool IsSent = objEmailHelper.SendEmailWithTemplate();
                            //    //Push Notification
                            //    message = PushNotificationMessages.VehicleMileage(item.LocationName);
                            //    PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
                            //    if (IsSent == true)
                            //    {
                            //        objEmailog = new EmailLog();
                            //        try
                            //        {
                            //            objEmailog.CreatedBy = item.RequestBy;
                            //            objEmailog.CreatedDate = DateTime.Now;
                            //            objEmailog.DeletedBy = null;
                            //            objEmailog.DeletedOn = null;
                            //            objEmailog.LocationId = item.LocationID;
                            //            objEmailog.ModifiedBy = null;
                            //            objEmailog.ModifiedOn = null;
                            //            objEmailog.SentBy = item.RequestBy;
                            //            objEmailog.SentEmail = item.ManagerEmail;
                            //            objEmailog.Subject = objEmailHelper.Subject;
                            //            objEmailog.SentTo = item.ManagerUserId;
                            //            objListEmailog.Add(objEmailog);
                            //            //objEmailLogRepository.SaveEmailLog(objEmailog);
                            //        }
                            //        catch (Exception)
                            //        {
                            //            throw;
                            //        }
                            //    }
                            //} //obj.VehicleCheck.WVehicleMileage == false end here
                        }
                        Task<bool> x = null;
                        foreach (var i in objListEmailog)
                        {
                            x = objEmailLogRepository.SaveEmailLogAsync(i);
                        }
                    }
                    if (obj.VehicleCheck.BrakesWorkOrderId != null && obj.VehicleCheck.BrakesWorkOrderId > 0 && ObjServiceResponseModel.Response == 1)
                    {
                        List<ssp_SendEmailForBrakeNotFunctional_Result> objEmailReturn1 = new List<ssp_SendEmailForBrakeNotFunctional_Result>();
                        ObjCommonRepository = new CommonRepository();
                        objEmailReturn1 = ObjCommonRepository.SendEmailToManager(obj.VehicleCheck.BrakesWorkOrderId, obj.LocationId);
                        foreach (var item in objEmailReturn1)
                        {
                            EmailHelper objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = item.ManagerEmail;
                            objEmailHelper.ManagerName = item.ManagerName;
                            objEmailHelper.ProblemDesc = item.ProblemDesc;
                            objEmailHelper.LocationName = item.LocationName;
                            objEmailHelper.UserName = item.UserName;
                            objEmailHelper.MailType = "WORKORDERREQUEST";
                            objEmailHelper.QrCodeId = obj.QrcodeId;
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
                                    //objListEmailog.Add(objEmailog);
                                    objEmailLogRepository.SaveEmailLog(objEmailog);
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }
                        }
                    }
                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ObjServiceResponseModel;
        }


        /// <summary>Testing to read XML to Object
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>XMLTesting</CreatedFor>
        /// <CreatedOn>Jan-28-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCPhoneSystemModel"></param>
        /// <returns></returns>
        public ServiceQrcVehicleModel XmlTestingForPhoneSystem(ServiceQrcVehicleModel obj)
        {
            QRCSetupManager ObjQRCSetupManager = new QRCSetupManager();
            ObjQRCMasterRepository = new QRCMasterRepository();
            ServiceQrcVehicleModel ObjReturn = new ServiceQrcVehicleModel();
            try
            {
                var result = ObjQRCSetupManager.GetQrcById(Convert.ToInt64(obj.QrcId));

                ObjReturn = GenericDataContractSerializer<ServiceQrcVehicleModel>.DeserializeXml(result.QRCTypeDetails);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            return ObjReturn;
        }

        /// <summary>Save QRC Shuttle Request
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>QrcShuttleRequestDetails</CreatedFor>
        /// <CreatedOn>May-09-2017</CreatedOn>
        /// </summary>
        /// <param name="ServiceQrcShuttleBusModels"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> SaveQrcShuttleRequestDetails(ServiceQrcShuttleBusModel obj)
        {
            ObjQRCMasterRepository = new QRCMasterRepository();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            List<EmailToManagerModel> objEmailReturn = new List<EmailToManagerModel>();
            List<EmailLog> objListEmailog = new List<EmailLog>();
            try
            {
                if (obj.ServiceAuthKey != null)
                {
                    string ToXml; bool checkout = false;
                    ToXml = GenericDataContractSerializer<ServiceQrcShuttleBusModel>.SerializeObject(obj);
                    if (obj.Status == "PreTrip")
                        checkout = true;
                    var result = ObjQRCMasterRepository.QRCSave(obj.ServiceAuthKey, obj.UserId, obj.QrcId, ToXml, obj.Action, checkout, obj.DamageStatus, obj.UserName);
                    ObjServiceResponseModel.Response = (result != null) ? Convert.ToInt64(result.Data.Response, CultureInfo.InvariantCulture) : Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = (result != null) ? (result.Data.ResponseMessage).ToString(CultureInfo.InvariantCulture) : CommonMessage.NoRecordMessage();//CommonMessage.MessageLogout();
                    if (ObjServiceResponseModel.Response == 1)
                    {
                        objEmailReturn = objEmailLogRepository.SendEmailToManagerForItemMissingQRC(obj.LocationId, obj.UserId);
                    }

                    if (objEmailReturn.Count > 0 && ObjServiceResponseModel.Response == 1)
                    {
                        foreach (var item in objEmailReturn)
                        {

                            if (obj.Damage.IsDamage == true)
                            {
                                bool IsSent = false;
                                //ObjCommonRepository = new CommonRepository();
                                EmailHelper objEmailHelper = new EmailHelper();
                                objEmailHelper.emailid = item.ManagerEmail;
                                objEmailHelper.ManagerName = item.ManagerName;
                                //Note:- Here problem desc is sent by shuttle damage desc
                                objEmailHelper.ProblemDesc = obj.Damage.DamageDesc;
                                objEmailHelper.LocationName = item.LocationName;
                                objEmailHelper.UserName = item.UserName;
                                objEmailHelper.QrCodeId = obj.QrcodeId;
                                objEmailHelper.MailType = "QRCSHUTTLEPARTDAMAGE";
                                objEmailHelper.SentBy = item.RequestBy;
                                objEmailHelper.LocationID = item.LocationID;
                                objEmailHelper.TimeAttempted = DateTime.UtcNow.ToClientTimeZone(true).ToString();

                                if (obj.AllPictures != null && obj.AllPictures.Trim() != "")
                                {
                                    obj.AllPictures = obj.AllPictures.Replace(" ", "");
                                    var list1 = obj.AllPictures.Split(',').ToList();
                                    string[] attachFiles = new string[list1.Count()];
                                    for (var i = 0; i < list1.Count(); i++)
                                    {
                                        //attachFiles[i] = HttpConfigurationManager.AppSettings["hostingPrefix"] + "Content/Images/QRCVehicle/" + list1[i];
                                        attachFiles[i] = HttpContext.Current.Server.MapPath("~/Content/Images/QRCVehicle/" + list1[i]);
                                    }
                                    IsSent = objEmailHelper.SendEmailWithTemplate(attachFiles);
                                }
                                else
                                {
                                    IsSent = objEmailHelper.SendEmailWithTemplate();
                                }
                                //Push Notification
                                message = PushNotificationMessages.ShuttlePartDamage(item.LocationName);
                                PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
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
                                        objListEmailog.Add(objEmailog);
                                        //objEmailLogRepository.SaveEmailLog(objEmailog);
                                    }
                                    catch (Exception)
                                    {
                                        throw;
                                    }
                                }
                            } //obj.Tires.IsTireDamage == true end here
                            if (obj.Tires.IsTireDamage == true)
                            {
                                bool IsSent = false;
                                EmailHelper objEmailHelper = new EmailHelper();
                                objEmailHelper.emailid = item.ManagerEmail;
                                objEmailHelper.ManagerName = item.ManagerName;
                                //Note:- Here problem desc is sent by Item Missing Description
                                objEmailHelper.ProblemDesc = PushNotificationMessages.ShuttleTirePartDamage(item.LocationName);//obj.CheckingOut.ItemDescription;
                                objEmailHelper.LocationName = item.LocationName;
                                objEmailHelper.UserName = item.UserName;
                                objEmailHelper.MailType = "QRCSHUTTLETIREDAMAGE";
                                objEmailHelper.SentBy = item.RequestBy;
                                objEmailHelper.QrCodeId = obj.QrcodeId;
                                objEmailHelper.LocationID = item.LocationID;
                                objEmailHelper.TimeAttempted = DateTime.UtcNow.ToClientTimeZone(true).ToString();
                                if (obj.AllPictures != null && obj.AllPictures.Trim() != "")
                                {
                                    obj.AllPictures = obj.AllPictures.Replace(" ", "");
                                    var list1 = obj.AllPictures.Split(',').ToList();
                                    string[] attachFiles = new string[list1.Count()];
                                    for (var i = 0; i < list1.Count(); i++)
                                    {
                                        //attachFiles[i] = HttpConfigurationManager.AppSettings["hostingPrefix"] + "Content/Images/QRCVehicle/" + list1[i];
                                        attachFiles[i] = HttpContext.Current.Server.MapPath("~/Content/Images/QRCVehicle/" + list1[i]);

                                    }
                                    IsSent = objEmailHelper.SendEmailWithTemplate(attachFiles);
                                }
                                else
                                {
                                    IsSent = objEmailHelper.SendEmailWithTemplate();
                                }
                                //Push Notification
                                message = PushNotificationMessages.ShuttleTirePartDamage(item.LocationName);
                                PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
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
                                        objListEmailog.Add(objEmailog);
                                        //objEmailLogRepository.SaveEmailLog(objEmailog);
                                    }
                                    catch (Exception)
                                    {
                                        throw;
                                    }
                                }

                            } //obj.VehicleCheck.IsDamage == true end here

                            if (obj.Mileage.ChShuttleMileage == false &&
                                  obj.Mileage.ChShDescription != null &&
                                 string.IsNullOrEmpty(obj.Mileage.ChShDescription) == false &&
                                 string.IsNullOrEmpty(obj.Mileage.OldChShDescription) == false)
                            {
                                //  ObjCommonRepository = new CommonRepository();
                                EmailHelper objEmailHelper = new EmailHelper();
                                objEmailHelper.emailid = item.ManagerEmail;
                                objEmailHelper.ManagerName = item.ManagerName;
                                //Note:- Here Action for vehicle mileage
                                objEmailHelper.CurrentMileage = obj.Mileage.ChShDescription;
                                objEmailHelper.LocationName = item.LocationName;
                                objEmailHelper.UserName = item.UserName;
                                objEmailHelper.MailType = "QRCSHUTTLEMILEAGECO";
                                objEmailHelper.SentBy = item.RequestBy;
                                objEmailHelper.LocationID = item.LocationID;
                                objEmailHelper.QrCodeId = obj.QrcodeId;
                                objEmailHelper.PreviousMileage = obj.Mileage.OldChShDescription;
                                objEmailHelper.Action = obj.Status;

                                bool IsSent = objEmailHelper.SendEmailWithTemplate();
                                //Push Notification
                                message = PushNotificationMessages.ShuttleMileage(item.LocationName);
                                PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
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
                                        objListEmailog.Add(objEmailog);
                                        //objEmailLogRepository.SaveEmailLog(objEmailog);
                                    }
                                    catch (Exception)
                                    {
                                        throw;
                                    }
                                }
                            }
                            // if (obj.VehicleFuel.VfVehicleMileage == false &&
                            // obj.VehicleFuel.VfVmDescription != null &&
                            //string.IsNullOrEmpty(obj.VehicleFuel.VfVmDescription) == false &&
                            //string.IsNullOrEmpty(obj.VehicleFuel.OldVfVmDescription) == false)
                            // {
                            //     EmailHelper objEmailHelper = new EmailHelper();
                            //     objEmailHelper.emailid = item.ManagerEmail;
                            //     objEmailHelper.ManagerName = item.ManagerName;
                            //     //Note:- Here Action for vehicle mileage
                            //     objEmailHelper.CurrentMileage = obj.CheckingIn.ChVmDescription;
                            //     objEmailHelper.LocationName = item.LocationName;
                            //     objEmailHelper.UserName = item.UserName;
                            //     objEmailHelper.MailType = "QRCVEHICLEMILEAGECO";
                            //     objEmailHelper.SentBy = item.RequestBy;
                            //     objEmailHelper.LocationID = item.LocationID;
                            //     objEmailHelper.QrCodeId = obj.QrcodeId;
                            //     objEmailHelper.PreviousMileage = obj.CheckingIn.OldChVmDescription;
                            //     objEmailHelper.Action = "vehicle fueling.";

                            //     bool IsSent = objEmailHelper.SendEmailWithTemplate();
                            //     //Push Notification
                            //     message = PushNotificationMessages.VehicleMileage(item.LocationName);
                            //     PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
                            //     if (IsSent == true)
                            //     {
                            //         objEmailog = new EmailLog();
                            //         try
                            //         {
                            //             objEmailog.CreatedBy = item.RequestBy;
                            //             objEmailog.CreatedDate = DateTime.UtcNow;
                            //             objEmailog.DeletedBy = null;
                            //             objEmailog.DeletedOn = null;
                            //             objEmailog.LocationId = item.LocationID;
                            //             objEmailog.ModifiedBy = null;
                            //             objEmailog.ModifiedOn = null;
                            //             objEmailog.SentBy = item.RequestBy;
                            //             objEmailog.SentEmail = item.ManagerEmail;
                            //             objEmailog.Subject = objEmailHelper.Subject;
                            //             objEmailog.SentTo = item.ManagerUserId;
                            //             objListEmailog.Add(objEmailog);
                            //             // objEmailLogRepository.SaveEmailLog(objEmailog);
                            //         }
                            //         catch (Exception)
                            //         {
                            //             throw;
                            //         }
                            //     }
                            // }
                        }
                        Task<bool> x = null;
                        foreach (var i in objListEmailog)
                        {
                            x = objEmailLogRepository.SaveEmailLogAsync(i);
                        }
                    }
                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ObjServiceResponseModel;
        }

        #endregion For Service QRCType Details

        /// <summary>
        /// Created by vijay sahu  on 20 march 2015
        /// This method is using for check the QRCName already exists in Record(DB) or not.
        /// </summary>
        /// <param name="QRCName"></param>
        /// <returns></returns>
        public byte checkQRCName(string QRCName, long QRCType, long LocId)
        {
            byte status = 0;
            long qrcid = 0;
            using (workorderEMSEntities Context = new workorderEMSEntities())
            {
                //status = Context.QRCMasters(QRCName);
                qrcid = (from o in Context.QRCMasters
                         where o.QRCName == QRCName.Trim()
                         && o.QRCTYPE == QRCType
                         && o.IsDeleted != true
                         && o.LocationId == LocId
                         select (o.QRCID)).FirstOrDefault();

                status = qrcid > 0 ? status = 1 : status = 0;
            }

            return status;
        }

        /// <summary>Send email and push to manager if checkout already checked by anyone
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>CheckoutEmail</CreatedFor>
        /// <CreatedOn>July-13-2015</CreatedOn>
        /// </summary>
        /// <param name="objServiceQrcModel"></param>
        /// <returns></returns>
        public bool SendCheckoutDetailsToManager(ServiceQrcModel obj)
        {
            bool IsSent = false;
            ObjUserRepository = new UserRepository();
            objLoginLogRepository = new LoginLogRepository();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            List<EmailToManagerModel> objEmailReturn = new List<EmailToManagerModel>();
            try
            {
                var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == obj.ServiceAuthKey && x.IsDeleted == false);

                if (authuser != null && authuser.UserId > 0)
                {
                    //send email and push notification to manager 
                    objEmailReturn = objEmailLogRepository.SendEmailToManagerForItemMissingQRC(obj.LocationId, obj.UserId);//function return data according to location and username
                    if (objEmailReturn.Count() > 0)
                    {
                        foreach (var item in objEmailReturn)
                        {
                            EmailHelper objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = item.ManagerEmail;
                            objEmailHelper.ManagerName = item.ManagerName;
                            objEmailHelper.CheckoutUserName = obj.CheckoutUserName;
                            objEmailHelper.NewCheckoutUserName = obj.NewCheckoutUserName;
                            objEmailHelper.LocationName = item.LocationName;
                            objEmailHelper.UserName = item.UserName;
                            objEmailHelper.QrCodeId = obj.QrCodeId;
                            objEmailHelper.MailType = "CHECKOUT";
                            objEmailHelper.SentBy = item.RequestBy;
                            objEmailHelper.LocationID = item.LocationID;
                            objEmailHelper.TimeAttempted = DateTime.UtcNow.ToClientTimeZone(true).ToString();
                            IsSent = objEmailHelper.SendEmailWithTemplate();
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
                                message = PushNotificationMessages.CheckOut(item.LocationName, obj.QrCodeId, obj.CheckoutUserName);
                                PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
                            }
                        }
                    }
                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return IsSent;
        }

        /// <summary>GetQRCMasterById
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Aug-24-2015</CreatedOn>
        /// </summary>
        /// <param name="qrcId"></param>
        /// <returns></returns>
        public string GetQRCDetailById(long qrcId)
        {
            try
            {
                ObjQRCMasterRepository = new QRCMasterRepository();
                return ObjQRCMasterRepository.GetSingleOrDefault(g => g.QRCID == qrcId && g.IsDeleted == false).AssetPicture;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>UpdateQRCheckIn
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Nov-18-2016</CreatedOn>
        /// <CreatedFor>Check In QRC</CreatedFor>
        /// </summary>
        /// <param name="QRCID"></param>
        /// <param name="LoggedInUser"></param>
        /// <returns></returns>
        public Result UpdateQRCheckIn(long qrcId, long loggedInUser, DARModel objDAR, string locationName)
        {
            Result result;
            try
            {
                if (qrcId > 0)
                {
                    if (true)
                    {
                        ObjQRCMasterRepository = new QRCMasterRepository();
                        var data = ObjQRCMasterRepository.GetSingleOrDefault(q => q.QRCID == qrcId && q.IsDeleted == false);
                        if (data == null) { throw new Exception(CommonMessage.DoesNotExistsRecordMessage()); }
                        data.CheckOutStatus = true;
                        data.ModifiedBy = loggedInUser;
                        data.ModifiedDate = DateTime.UtcNow;
                        ObjQRCMasterRepository.Update(data);

                        objDAR.ActivityDetails = DarMessage.CheckIn(data.QRCName, locationName);

                        #region Save DAR
                        result = _ICommonMethod.SaveDAR(objDAR);
                        #endregion Save DAR

                        return Result.Completed;
                    }
                    else
                    { return Result.Failed; }
                }
                else { return Result.DoesNotExist; }


            }
            catch (Exception)
            { throw; }
        }

        /// <summary>DamageFixed
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Nov-18-2016</CreatedOn>
        /// <CreatedFor>Mark damage has been been fixed.</CreatedFor>
        /// </summary>
        /// <param name="QRCID"></param>
        /// <param name="LoggedInUser"></param>
        /// <returns></returns>
        public Result DamageFixed(long qrcId, long loggedInUser, DARModel objDAR, string locationName)
        {
            Result result;
            try
            {
                if (qrcId > 0)
                {
                    if (true)
                    {
                        ObjQRCMasterRepository = new QRCMasterRepository();
                        var data = ObjQRCMasterRepository.GetSingleOrDefault(q => q.QRCID == qrcId && q.IsDeleted == false);
                        if (data == null) { throw new Exception(CommonMessage.DoesNotExistsRecordMessage()); }
                        data.IsDamageVerified = true;
                        data.DamageVerifiedBy = loggedInUser;
                        data.ModifiedBy = loggedInUser;
                        data.ModifiedDate = DateTime.UtcNow;
                        ObjQRCMasterRepository.Update(data);

                        objDAR.ActivityDetails = DarMessage.CheckIn(data.QRCName, locationName);

                        #region Save DAR
                        result = _ICommonMethod.SaveDAR(objDAR);
                        #endregion Save DAR

                        return Result.Completed;
                    }
                    else
                    { return Result.Failed; }
                }
                else { return Result.DoesNotExist; }


            }
            catch (Exception)
            { throw; }
        }

        public List<QRCListModel> GetAllQRCListPrint(long? qrcId, long? locationId, string sortColumnName, string sortOrderBy, string textSearch, long? ddlQRCType, long userId)
        {
            try
            {
                ObjQRCMasterRepository = new QRCMasterRepository();
                return ObjQRCMasterRepository.GetAllQRCListforPrint(qrcId, locationId, sortColumnName, sortOrderBy, textSearch, ddlQRCType, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long QRCCodeIDExist(long QRCID)
        {
            long qrcid = 0;
            try
            {
                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    //status = Context.QRCMasters(QRCName);
                    qrcid = (from o in Context.QRCMasters
                             where o.QRCID == QRCID
                             && o.IsDeleted != true
                             select (o.QRCID)).FirstOrDefault();
                    qrcid = qrcid > 0 ? qrcid : 0;
                    return qrcid;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public long QRCCodeIDExist(long QRCID)", "QRCID", QRCID);
                return qrcid;
            }
        }
    }
}
