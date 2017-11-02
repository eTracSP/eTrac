using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.Mvc;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;
using WorkOrderEMS.Models.SuperAdminModels;
using WorkOrderEMS.Models.UserModels;


namespace WorkOrderEMS.BusinessLogic.Managers
{
    [Authorize(Roles = CustomUserRoles.ITAdministrator + "," + CustomUserRoles.Administrator)]
    public class GlobalAdminManager : IGlobalAdmin
    {
        #region Location

        LocationRepository objLocationRepository = new LocationRepository();
        //ProjectServicesRepository ObjProjectServicesRepository;
        UserRepository ObjUserRepository;
        AdminLocationMappingRepository ObjAdminLocationMappingRepository;
        LocationClientMappingRepository ObjLocationClientMappingRepository;
        LocationRepository ObjLocationRepository;
        ManagerLocationMappingRepository ObjManagerLocationMappingRepository;
        QRCMasterRepository ObjQRCMasterRepository;
        LocationMaster objLocationMaster = new LocationMaster();
        UserRegistration ObjManagerUser;
        UserRegistration ObjClientUser;
        LocationMaster ObjLocationMaster;
        UserRegistration ObjEmployeeUser;
        AddressManager objAddressManager = new AddressManager();
        EmployeeLocationMappingRepository objEmployeeLocationMappingRepository;
        EmployeeLocationMapping objEmployeeMapping;
        CommonMethodManager commonMethodManager;
        WorkRequestAssignmentRepository objWorkRequestAssignmentRepository;
        EmailLogRepository objEmailLogRepository = null;

        private string ProfileImagePath = ConfigurationManager.AppSettings["ProfilePicPath"];
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private readonly string ConstantImages = ConfigurationManager.AppSettings["ConstantImages"] + "Images/constant/no-profile-pic.jpg";
        private readonly string ConstantImagesForClient = ConfigurationManager.AppSettings["ConstantImages"] + "no-profile-pic.jpg";

        //workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();

        //Created by Gayatri Pal
        //on 25-Aug-2014\
        //To save location information
        //public Result SaveLocation(LocationMasterModel objLocationMasterModel, out long locationId)

        private readonly ICommonMethod _ICommonMethod;
        private readonly IDARManager _IDARManager;
        public GlobalAdminManager()
        { }

        public GlobalAdminManager(ICommonMethod _ICommonMethod)
        {
            this._ICommonMethod = _ICommonMethod;
        }

        public GlobalAdminManager(ICommonMethod _ICommonMethod, IDARManager _IDARManager)
        {
            this._ICommonMethod = _ICommonMethod;
            this._IDARManager = _IDARManager;
        }

        private readonly long ManageLocationMODULE = 89;
        //private readonly long ManageProjectMODULE = 90;
        //private readonly long ManageManagerMODULE = 93;
        //private readonly long WorkProcessPROJECTCATEGORY = 8;
        private readonly long QRCDefaultSizeID = 48;    //QRCSIZE	94 X 94
        private readonly long QRCTYPEID = 42;	        //QRCTYPE	Emergency Phone Systems
        private string SpecialNotes = "Location added successfully";
        private string QRCName = "Location";
        private string eTracVerifyLocation = System.Configuration.ConfigurationManager.AppSettings["eTracVerifyLocation"];

        /// <summary>SaveLocation
        /// CreatedBY:   Gayatri Pal
        /// CreatedOn:   Aug-25-2014
        /// CreatedFor:  To save location information
        /// ModifiedBY:  Nagendra Upwanshi
        /// ModifiedOn:  Aug-28-2014
        /// ModifiedFor: modified for QRC Saveing and return QRCId
        /// </summary>
        /// <param name="objLocationMasterModel"></param>
        /// <param name="QRCID"></param>
        /// <returns></returns>
        public Result SaveLocation(LocationMasterModel objLocationMasterModel, out QRCModel qrcDetail)
        {
            try
            {
                qrcDetail = new QRCModel();
                QRCName = "Location"; SpecialNotes = "Location added successfully";
                if (CheckDuplicateLocation(objLocationMasterModel.LocationName.Trim(), objLocationMasterModel.LocationId, out qrcDetail))
                {
                    if (objLocationMasterModel.LocationId == 0)
                    {
                        AutoMapper.Mapper.CreateMap<LocationMasterModel, LocationMaster>();
                        objLocationMaster = AutoMapper.Mapper.Map(objLocationMasterModel, objLocationMaster);
                        long QRCID = 0;
                        if (_ICommonMethod.GenerateQRCode(QRCName, ManageLocationMODULE, null, null, QRCDefaultSizeID, QRCTYPEID, SpecialNotes, objLocationMasterModel.CreatedBy, out QRCID))
                        {
                            objLocationMaster.QRCID = QRCID;
                            objLocationRepository.Add(objLocationMaster);

                            if (QRCID > 0)
                            {
                                qrcDetail.QRCId = QRCID;
                                qrcDetail.QRCName = QRCName;
                                qrcDetail.SpecialNotes = SpecialNotes;
                                qrcDetail.EncryptQRC = Cryptography.GetEncryptedData(QRCID.ToString(CultureInfo.InvariantCulture), true);
                                qrcDetail.QRCDefaultSize = QRCDefaultSizeID;
                            }
                        }

                        if (objLocationMaster.LocationId > 0)
                            return Result.Completed;
                        else
                            return Result.Failed;
                    }
                    else
                    {
                        UpdateLocation(objLocationMasterModel, out qrcDetail);
                        return Result.UpdatedSuccessfully;
                    }
                }
                else
                { return Result.DuplicateRecord; }
            }
            catch (Exception)
            { throw; }
        }

        public bool CheckDuplicateLocation(string locationName, long locationId, out QRCModel qrcDetail)
        {
            try
            {
                qrcDetail = new QRCModel();
                var data = objLocationRepository.GetAll(l => l.LocationName == locationName.Trim() && l.LocationId != locationId);
                if (data != null && data.Count() > 0)
                {
                    qrcDetail = GetQRCDetails((data[0].QRCID.HasValue) ? (data[0].QRCID.Value) : 0);
                    return false;
                }
                return true;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>GetQRCDetails
        /// CreatedBy   :   Nagendra Upwanshi
        /// CreatedOn   :   Sep-02-2014
        /// CreatedFor  :   Get existing QRC Details by QRCID
        /// </summary>
        /// <param name="QRCID"></param>
        /// <returns></returns>
        private QRCModel GetQRCDetails(long QRCID)
        {
            QRCModel QRCDetail = new QRCModel();
            if (QRCID > 0)
            {
                ObjQRCMasterRepository = new QRCMasterRepository();
                var QRCdata = ObjQRCMasterRepository.GetSingleOrDefault(q => q.QRCID == QRCID && q.IsDeleted == false);
                if (QRCdata != null)
                {
                    if (QRCID != null)
                    {
                        QRCDetail.QRCId = QRCdata.QRCID;
                        QRCDetail.QRCName = QRCdata.QRCName;
                        QRCDetail.SpecialNotes = QRCdata.SpecialNotes;
                        QRCDetail.EncryptQRC = Cryptography.GetEncryptedData(QRCdata.QRCID.ToString(CultureInfo.InvariantCulture), true);
                        QRCDetail.QRCDefaultSize = QRCdata.QRCDefaultSize;
                    }
                }
            }
            return QRCDetail;
        }

        /// <summary>GetAllLocationNew
        /// <CreatedOn>NOv-18-2014</CreatedOn>
        /// <CreatedFor>Get All New Location for SelectListItem</CreatedFor>
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// </summary>
        /// <returns></returns>
        public List<LocationListModel> GetAllLocationNew()
        {
            try
            {

                ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
                return GetAllLocationList(0, "GetAllLocation", 1, 10000, "LocationName", "desc", "", paramTotalRecords).ToList();
                paramTotalRecords = null;
                /*Select(g => new SelectListItem()
            {
                Text = g.LocationName,
                Value = g.LocationId.ToString()
            }).ToList();
             */

            }
            catch (Exception)
            { throw; }

        }

        public List<LocationListModel> GetAllLocationList(int locationId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords)
        {
            try
            {
                return objLocationRepository.GetAllLocationList(locationId, operationName, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, totalRecords);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>ListAllLocation
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Dec-04-2014</CreatedOn>
        /// <CreatedFor>Get All Location List</CreatedFor>
        /// </summary>
        /// <param name="LocationID"></param>        
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <param name="paramTotalRecords"></param>
        /// <returns></returns>
        public List<ListLocationModel> ListAllLocation(int? locationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords)
        {
            try
            {
                if (locationId == 0) { locationId = null; }
                return objLocationRepository.GetListAllLocation(locationId, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, totalRecords);
            }
            catch (Exception)
            { throw; }
        }

        public LocationMasterModel GetLocationById(long locationId)
        {
            LocationMasterModel _LocationMasterModel = new LocationMasterModel();
            try
            {
                var data = objLocationRepository.GetSingleOrDefault(l => l.LocationId == locationId && l.IsDeleted == false);
                if (data != null)
                {
                    _LocationMasterModel.LocationId = locationId;
                    _LocationMasterModel.LocationName = data.LocationName;
                    _LocationMasterModel.Description = data.Description;
                    _LocationMasterModel.Address1 = data.Address1;
                    _LocationMasterModel.Address2 = data.Address2;
                    _LocationMasterModel.City = data.City;
                    _LocationMasterModel.StateId = data.StateId;
                    _LocationMasterModel.CountryId = data.CountryId;
                    _LocationMasterModel.Mobile = data.Mobile;
                    _LocationMasterModel.PhoneNo = data.PhoneNo;
                    _LocationMasterModel.ZipCode = data.ZipCode;
                }
                return _LocationMasterModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Created by vijay sahu on march 2015
        /// </summary>
        /// <param name="locationId"></param>
        /// <param name="objDAR"></param>
        /// <returns></returns>
        public Result DeleteLocation(long locationId, DARModel objDAR)
        {
            Result result;
            try
            {
                if (locationId != null || locationId != 0 || Convert.ToString(locationId) != string.Empty)
                {

                    if (true)
                    {

                        System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();

                        LocationMasterModel objLocationMasterModel = GetLocationById(Convert.ToInt64(locationId));
                        objDAR.ActivityDetails = DarMessage.DeleteSuccessLocationDar(objLocationMasterModel.LocationName);
                        objDAR.TaskType = (long)TaskTypeCategory.DeleteLocation;

                        loc obj = new loc();
                        obj.LocationId = Convert.ToString(locationId);
                        var aa = ser.Serialize(obj);
                        sp_DeleteLocation_Result objRes = new sp_DeleteLocation_Result();
                        using (workorderEMSEntities Context = new workorderEMSEntities())
                        {
                            objRes = Context.sp_DeleteLocation(aa).FirstOrDefault();
                        }


                        if (objRes.Result == "success")
                        {

                            #region Save DAR
                            result = _ICommonMethod.SaveDAR(objDAR);
                            #endregion Save DAR

                            return Result.Delete;
                        }
                        else
                        { return Result.Failed; }

                    }
                    else
                    {
                        return Result.Failed;
                    }



                    //previous code commented by vijay sahu on 9 march 2015
                    //if (CheckProjectExistsForLocation(locationId))
                    //{
                    //    var data = objLocationRepository.GetSingleOrDefault(l => l.LocationId == locationId && l.IsDeleted == false);
                    //    data.IsDeleted = true;
                    //    data.DeletedBy = 1;
                    //    data.DeletedDate = DateTime.Now;
                    //    objLocationRepository.Update(data);
                    //    return Result.Delete;
                    //}
                    //else
                    //{
                    //    return Result.Failed;
                    //}
                }
                return Result.Delete;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// created by vijay sahu on 24 march 2015
        /// </summary>
        /// <param name="locationName"></param>
        /// <returns></returns>
        public byte isLocationNameExists(string locationName)
        {
            byte result = 0;
            try
            {
                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    var re = (from o in Context.LocationMasters
                              where o.LocationName == locationName.Trim()
                                     && o.IsDeleted == false
                              select o.LocationId).FirstOrDefault();

                    if (re > 0)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result = 0;

            }
            return result;
        }

        public bool CheckProjectExistsForLocation(long locationId)
        {
            //int count = _workorderEMSEntities.ProjectMasters.Select(p => p.LocationID == LocationID).Count();
            int count = objLocationRepository.GetAll(l => l.LocationId == locationId).Count();
            if (count > 0)
                return false;
            return true;
        }

        public List<LocationMasterModel> GetAllLocation()
        {
            List<LocationMasterModel> lstLocation = new List<LocationMasterModel>();
            try
            {
                lstLocation = objLocationRepository.GetAll(l => l.IsDeleted == false).Select(s => new LocationMasterModel()
                {
                    LocationId = s.LocationId,
                    LocationName = s.LocationName
                }).ToList();
                return lstLocation;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Created by gayatri
        //to get location details by locationid
        public LocationMasterModel GetLocationDetailsByLocationId(long locationId)
        {
            LocationRepository _LocationRepository = new LocationRepository();
            try
            {
                //return _LocationRepository.GetLocationDetailsByLocationID(LocationID);
                LocationMasterModel ObjLocationMasterModel = new LocationMasterModel();
                ObjLocationMasterModel.ManagerModel = new UserModel();
                ObjLocationMasterModel.ClientModel = new UserModel();
                ObjLocationMasterModel.ManagerModel.Address = new AddressModel();
                ObjLocationMasterModel.ClientModel.Address = new AddressModel();
                var ss = _LocationRepository.GetSingleOrDefault(l => l.LocationId == locationId && l.IsDeleted == false);
                ObjLocationMasterModel.LocationId = ss.LocationId;
                ObjLocationMasterModel.LocationName = ss.LocationName;
                ObjLocationMasterModel.Description = ss.Description;
                ObjLocationMasterModel.Address1 = ss.Address1;
                ObjLocationMasterModel.Address2 = ss.Address2;
                ObjLocationMasterModel.CountryId = ss.CountryId;
                ObjLocationMasterModel.StateId = ss.StateId;
                ObjLocationMasterModel.ZipCode = ss.ZipCode;
                ObjLocationMasterModel.City = ss.City;
                ObjLocationMasterModel.PhoneNo = ss.PhoneNo;
                ObjLocationMasterModel.LocationType = ss.LocationType;
                ObjLocationMasterModel.LocationSubType = ss.LocationSubType;
                if (ss.LocationServices.FirstOrDefault() != null)
                {
                    ObjLocationMasterModel.Services = string.Join(",", ss.LocationServices.Select(r => r.ServiceId).ToArray());

                    // ObjLocationMasterModel.Services = services.toar
                }
                if (ss.ManagerLocationMappings.FirstOrDefault() != null)
                {
                    ObjLocationMasterModel.ManagerModel.UserId = ss.ManagerLocationMappings.FirstOrDefault().UserRegistration.UserId;
                    ObjLocationMasterModel.ManagerModel.EmployeeID = ss.ManagerLocationMappings.FirstOrDefault().UserRegistration.EmployeeID;
                    ObjLocationMasterModel.ManagerModel.UserEmail = ss.ManagerLocationMappings.FirstOrDefault().UserRegistration.UserEmail;
                    ObjLocationMasterModel.ManagerModel.AlternateEmail = ss.ManagerLocationMappings.FirstOrDefault().UserRegistration.AlternateEmail;
                    ObjLocationMasterModel.ManagerModel.FirstName = ss.ManagerLocationMappings.FirstOrDefault().UserRegistration.FirstName;
                    ObjLocationMasterModel.ManagerModel.LastName = ss.ManagerLocationMappings.FirstOrDefault().UserRegistration.LastName;
                    ObjLocationMasterModel.ManagerModel.Gender = ss.ManagerLocationMappings.FirstOrDefault().UserRegistration.Gender;
                    ObjLocationMasterModel.ManagerModel.DOB = ss.ManagerLocationMappings.FirstOrDefault().UserRegistration.DOB.ToString("MM/dd/yy");
                    ObjLocationMasterModel.ManagerModel.JobTitle = ss.ManagerLocationMappings.FirstOrDefault().UserRegistration.JobTitle;
                    var ManagerAddress = ss.ManagerLocationMappings.FirstOrDefault().UserRegistration.AddressMasters.FirstOrDefault();
                    if (ManagerAddress != null)
                    {
                        ObjLocationMasterModel.ManagerModel.Address.AddressMasterId = ss.ManagerLocationMappings.FirstOrDefault().UserRegistration.AddressMasters.FirstOrDefault().AddressMasterId;
                        ObjLocationMasterModel.ManagerModel.Address.Address1 = ss.ManagerLocationMappings.FirstOrDefault().UserRegistration.AddressMasters.FirstOrDefault().Address1;
                        ObjLocationMasterModel.ManagerModel.Address.Address2 = ss.ManagerLocationMappings.FirstOrDefault().UserRegistration.AddressMasters.FirstOrDefault().Address2;
                        ObjLocationMasterModel.ManagerModel.Address.CountryId = ss.ManagerLocationMappings.FirstOrDefault().UserRegistration.AddressMasters.FirstOrDefault().CountryId;
                        ObjLocationMasterModel.ManagerModel.Address.StateId = ss.ManagerLocationMappings.FirstOrDefault().UserRegistration.AddressMasters.FirstOrDefault().StateId;
                        ObjLocationMasterModel.ManagerModel.Address.City = ss.ManagerLocationMappings.FirstOrDefault().UserRegistration.AddressMasters.FirstOrDefault().City;
                        ObjLocationMasterModel.ManagerModel.Address.ZipCode = ss.ManagerLocationMappings.FirstOrDefault().UserRegistration.AddressMasters.FirstOrDefault().ZipCode;
                        ObjLocationMasterModel.ManagerModel.Address.Mobile = ss.ManagerLocationMappings.FirstOrDefault().UserRegistration.AddressMasters.FirstOrDefault().Mobile;
                        ObjLocationMasterModel.ManagerModel.Address.Phone = ss.ManagerLocationMappings.FirstOrDefault().UserRegistration.AddressMasters.FirstOrDefault().Phone;
                    }
                }
                if (ss.LocationClientMappings.FirstOrDefault() != null)
                {
                    ObjLocationMasterModel.ClientModel.UserEmail = ss.LocationClientMappings.FirstOrDefault().UserRegistration.UserEmail;
                    ObjLocationMasterModel.ClientModel.UserId = ss.LocationClientMappings.FirstOrDefault().UserRegistration.UserId;
                    ObjLocationMasterModel.ClientModel.EmployeeID = ss.LocationClientMappings.FirstOrDefault().UserRegistration.EmployeeID;
                    ObjLocationMasterModel.ClientModel.AlternateEmail = ss.LocationClientMappings.FirstOrDefault().UserRegistration.AlternateEmail;
                    ObjLocationMasterModel.ClientModel.FirstName = ss.LocationClientMappings.FirstOrDefault().UserRegistration.FirstName;
                    ObjLocationMasterModel.ClientModel.LastName = ss.LocationClientMappings.FirstOrDefault().UserRegistration.LastName;
                    ObjLocationMasterModel.ClientModel.Gender = ss.LocationClientMappings.FirstOrDefault().UserRegistration.Gender;
                    ObjLocationMasterModel.ClientModel.DOB = ss.LocationClientMappings.FirstOrDefault().UserRegistration.DOB.ToString("MM/dd/yy");
                    ObjLocationMasterModel.ClientModel.JobTitle = ss.LocationClientMappings.FirstOrDefault().UserRegistration.JobTitle;
                    ObjLocationMasterModel.ClientModel.Gender = ss.LocationClientMappings.FirstOrDefault().UserRegistration.Gender;
                    var ClientAddress = ss.LocationClientMappings.FirstOrDefault().UserRegistration.AddressMasters.FirstOrDefault();
                    if (ClientAddress != null)
                    {
                        ObjLocationMasterModel.ClientModel.Address.AddressMasterId = ss.LocationClientMappings.FirstOrDefault().UserRegistration.AddressMasters.FirstOrDefault().AddressMasterId;
                        ObjLocationMasterModel.ClientModel.Address.Address1 = ss.LocationClientMappings.FirstOrDefault().UserRegistration.AddressMasters.FirstOrDefault().Address1;
                        ObjLocationMasterModel.ClientModel.Address.Address2 = ss.LocationClientMappings.FirstOrDefault().UserRegistration.AddressMasters.FirstOrDefault().Address2;
                        ObjLocationMasterModel.ClientModel.Address.CountryId = ss.LocationClientMappings.FirstOrDefault().UserRegistration.AddressMasters.FirstOrDefault().CountryId;
                        ObjLocationMasterModel.ClientModel.Address.StateId = ss.LocationClientMappings.FirstOrDefault().UserRegistration.AddressMasters.FirstOrDefault().StateId;
                        ObjLocationMasterModel.ClientModel.Address.City = ss.LocationClientMappings.FirstOrDefault().UserRegistration.AddressMasters.FirstOrDefault().City;
                        ObjLocationMasterModel.ClientModel.Address.ZipCode = ss.LocationClientMappings.FirstOrDefault().UserRegistration.AddressMasters.FirstOrDefault().ZipCode;
                        ObjLocationMasterModel.ClientModel.Address.Mobile = ss.LocationClientMappings.FirstOrDefault().UserRegistration.AddressMasters.FirstOrDefault().Mobile;
                        ObjLocationMasterModel.ClientModel.Address.Phone = ss.LocationClientMappings.FirstOrDefault().UserRegistration.AddressMasters.FirstOrDefault().Phone;
                    }
                    string ClientImage = ss.LocationClientMappings.FirstOrDefault().UserRegistration.ProfileImage;
                    ObjLocationMasterModel.ClientModel.myProfileImage = (ClientImage == null || ClientImage.Trim() == "") ? HostingPrefix + ConstantImagesForClient.Replace("~", "") : HostingPrefix + ProfileImagePath.Replace("~", "") + ClientImage;
                }
                return ObjLocationMasterModel;
            }
            catch (Exception)
            { throw; }
        }

        public LocationMasterModel GetManagerByIdCode(long userId)
        {
            long Totalrecord = 0;
            LocationMasterModel _LocationMasterModel = new LocationMasterModel();
            _LocationMasterModel.ManagerModel = new UserModel();
            _LocationMasterModel.ManagerModel = _ICommonMethod.GetManagerByIdCode(userId, "GetUserByID", null, null, null, null, null, out Totalrecord);
            return _LocationMasterModel;
        }
        #endregion

        #region Project

        //ProjectMaster _ProjectMaster = new ProjectMaster();
        //ProjectRepository objProjectRepository = new ProjectRepository();
        //ProjectServicesRepository objProjectServicesRepository;
        LocationServicesRepository ObjLocationServicesRepository;

        /// <summary>SaveProject
        /// CreatedBY:   Gayatri Pal
        /// CreatedOn:   Aug-28-2014
        /// CreatedFor:  To Save Project
        /// ModifiedBY:  Nagendra Upwanshi
        /// ModifiedOn:  Aug-29-2014
        /// ModifiedFor: colding modify
        /// </summary>
        /// <param name="objProjectMasterModel"></param>
        /// <param name="ProjectID"></param>
        /// <param name="ServicesID"></param>
        /// <returns></returns>
        //public Result SaveProject(ProjectMasterModel objProjectMasterModel, string ServicesID, out QRCModel QRCDetail)
        //{
        //    bool IsEdit = false;
        //    try
        //    {

        //        long QRCID = 0; QRCName = "Project"; SpecialNotes = "Project added successfully";
        //        if (chkDuplicateProject(objProjectMasterModel.Location, objProjectMasterModel.ProjectID, out QRCDetail))
        //        {
        //            QRCDetail = new QRCModel();
        //            if (objProjectMasterModel.ProjectID == 0)
        //            {
        //                AutoMapper.Mapper.CreateMap<ProjectMasterModel, ProjectMaster>();
        //                _ProjectMaster = AutoMapper.Mapper.Map(objProjectMasterModel, _ProjectMaster);

        //                if (_ICommonMethod.GenerateQRCode(QRCName, ManageProjectMODULE, null, null, QRCDefaultSizeID, QRCTYPEID, SpecialNotes, objProjectMasterModel.CreatedBy, out QRCID))
        //                {
        //                    _ProjectMaster.QRCID = QRCID;
        //                    objProjectRepository.Add(_ProjectMaster);

        //                    if (QRCID > 0)
        //                    {
        //                        // QRCDetail = new QRCModel();
        //                        QRCDetail.QRCId = QRCID;
        //                        QRCDetail.QRCName = QRCName;
        //                        QRCDetail.SpecialNotes = SpecialNotes;
        //                        QRCDetail.EncryptQRC = Cryptography.GetEncryptedData(QRCID.ToString(), true);
        //                        QRCDetail.QRCDefaultSize = QRCDefaultSizeID;
        //                    }
        //                }

        //                if (_ProjectMaster.ProjectID > 0)
        //                {
        //                    if (SaveProjectServices(_ProjectMaster.ProjectID, ServicesID, IsEdit))
        //                    { return Result.Completed; }
        //                    else
        //                    { return Result.Failed; }
        //                }
        //                else
        //                    return Result.Failed;
        //            }
        //            else
        //            {
        //                IsEdit = true;
        //                UpdateProject(objProjectMasterModel);
        //                SaveProjectServices(objProjectMasterModel.ProjectID, ServicesID, IsEdit);
        //                QRCDetail = GetQRCDetails((objProjectMasterModel.QRCID.HasValue) ? (objProjectMasterModel.QRCID.Value) : 0);
        //                return Result.UpdatedSuccessfully;
        //            }

        //        }
        //        else
        //            return Result.DuplicateRecord;
        //    }
        //    catch (Exception)
        //    { throw ; }
        //}

        //public void UpdateProject(ProjectMasterModel _ProjectMasterModel)
        //{
        //    var data = objProjectRepository.GetSingleOrDefault(l => l.ProjectID == _ProjectMasterModel.ProjectID && l.IsDeleted == false);
        //    if (data != null)
        //    {
        //        data.ProjectName = _ProjectMasterModel.Location;
        //        data.LocationID = _ProjectMasterModel.LocationID;
        //        data.Description = _ProjectMasterModel.Description;
        //        data.ProjectCategory = _ProjectMasterModel.ProjectCategory;
        //        if (!string.IsNullOrEmpty(_ProjectMasterModel.ProjectLogoName))
        //        {
        //            data.ProjectLogoName = _ProjectMasterModel.ProjectLogoName;
        //            data.ProjectLogoURl = _ProjectMasterModel.ProjectLogoURl;
        //        }
        //        objLocationRepository.SaveChanges();
        //    }
        //}

        //public bool SaveProjectServices(long ProjectID, string ServicesID, bool IsEdit)
        //{
        //    ProjectService _ProjectService = null;

        //    bool statusflag = false;
        //    try
        //    {
        //        string[] aServicesID = ServicesID.Split(',');
        //        if (IsEdit)
        //        {
        //            objProjectServicesRepository = new ProjectServicesRepository();
        //            objProjectServicesRepository.DeleteAll(p => p.ProjectID == ProjectID);
        //            //objProjectServicesRepository.SaveChanges();
        //        }
        //        for (int i = 0; i < aServicesID.Length; i++)
        //        {
        //            objProjectServicesRepository = new ProjectServicesRepository();
        //            if (aServicesID[i] != null && !string.IsNullOrEmpty(aServicesID[i]) && Convert.ToInt64(aServicesID[i]) > 0)
        //            {
        //                _ProjectService = new ProjectService();
        //                _ProjectService.ProjectID = ProjectID;
        //                _ProjectService.ServiceId = Convert.ToInt64(aServicesID[i]);
        //                _ProjectService.CreatedBy = 1;//Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
        //                _ProjectService.CreatedOn = DateTime.Now;
        //                _ProjectService.IsDeleted = false;
        //                objProjectServicesRepository.Add(_ProjectService);
        //                //_workorderEMSEntities.ProjectServices.Add(_ProjectService);
        //                //_workorderEMSEntities.SaveChanges();
        //                statusflag = true;
        //            }
        //        }
        //    }
        //    catch (Exception) { throw; }
        //    return statusflag;
        //}


        //public bool chkDuplicateProject(string Location, long ProjectID, out QRCModel QRCDetail)
        //{
        //    QRCDetail = null;
        //    var data = objProjectRepository.GetSingle(p => p.ProjectName == Location && p.ProjectID != ProjectID);
        //    if (data != null)
        //    {
        //        QRCDetail = GetQRCDetails((data.QRCID.HasValue) ? (data.QRCID.Value) : 0);
        //        return false;
        //    }
        //    return true;
        //}

        //public List<ProjectMasterListModel> GetAllProject(int ProjectID, string OperationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter paramTotalRecords)
        //{
        //    return objProjectRepository.GetAllProject(ProjectID, OperationName, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, paramTotalRecords);
        //}

        //public Result DeleteProject(long ProjectID)
        //{
        //    try
        //    {
        //        if (ProjectID > 0)
        //        {
        //            if (chkManagerAssignToProject(ProjectID))
        //            {
        //                var data = objProjectRepository.GetSingleOrDefault(p => p.ProjectID == ProjectID && p.IsDeleted == false);
        //                data.IsDeleted = true;
        //                data.DeletedBy = 1;
        //                data.DeletedDate = DateTime.Now;
        //                objProjectRepository.Update(data);
        //                return Result.Delete;
        //            }
        //            return Result.DuplicateRecord;
        //        }
        //        return Result.Delete;
        //    }
        //    catch (Exception)
        //    { throw; }
        //}

        //public bool chkManagerAssignToProject(long projectid)
        //{
        //    ObjUserRepository = new UserRepository();
        //    int count = ObjUserRepository.GetAll(u => u.ProjectID == projectid).Count();
        //    if (count > 0)
        //        return false;
        //    return true;

        //}

        #endregion

        #region Manager

        public UserModel GetManagerById(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, out long totalRecords)
        {
            try
            { return _ICommonMethod.GetManagerByIdCode(userId, operationName, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, out totalRecords); }
            catch (Exception)
            { throw; }
        }

        public Result SaveManager(UserModel objUserModel, out long qrcID, bool isManagerRegistration)
        {
            try
            {
                qrcID = 0; QRCName = "Manager"; SpecialNotes = "Manager added successfully";
                ObjUserRepository = new UserRepository();
                ObjManagerUser = new UserRegistration();

                if (CheckDuplicateUser(objUserModel.UserEmail.Trim(), objUserModel.UserId, out qrcID))
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
                        ObjUserRepository.SaveChanges();
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
                        UpdateUser(objUserModel, out qrcID, isManagerRegistration);
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

        public bool CheckDuplicateUser(string userEmail, long userId, out long qrcId)
        {
            try
            {
                qrcId = 0;
                var data = ObjUserRepository.GetAll(u => u.UserEmail == userEmail.Trim() && u.IsDeleted == false && u.UserId != userId);

                if (data.Count > 0)
                {
                    qrcId = (data[0].QRCID.HasValue) ? (data[0].QRCID.Value) : 0;
                    return false;
                }
                return true;
            }
            catch (Exception)
            { throw; }
        }

        public void UpdateUser(UserModel objUserModel, out long qrcId, bool isManagerRegistration)
        {
            ObjUserRepository = new UserRepository();
            var data = ObjUserRepository.GetSingleOrDefault(u => u.UserId == objUserModel.UserId && u.IsDeleted == false);
            qrcId = (data.QRCID.HasValue) ? data.QRCID.Value : 0;
            if (data != null)
            {
                data.FirstName = objUserModel.FirstName;
                data.LastName = objUserModel.LastName;
                data.DOB = Convert.ToDateTime(objUserModel.DOB);
                data.BloodGroup = objUserModel.BloodGroup;
                data.AlternateEmail = objUserModel.AlternateEmail;
                data.Gender = objUserModel.Gender;
                if (isManagerRegistration)
                    data.Password = objUserModel.Password;
                data.IsEmailVerify = objUserModel.IsEmailVerify;
                data.IsLoginActive = objUserModel.IsLoginActive;
                data.Gender = objUserModel.Gender;
                if (!string.IsNullOrEmpty(objUserModel.ProfileImage.FileName)) { data.ProfileImage = objUserModel.ProfileImage.FileName; }
                ObjUserRepository.SaveChanges();
            }
        }

        #endregion Manager

        #region User Registration
        GlobalCodesRepository objGlobalCodesRepository = new GlobalCodesRepository();

        //public Result SendInvitation(UserModel objUserModel, string userType)
        public Result SendInvitation(UserModel objUserModel)
        {
            ObjUserRepository = new UserRepository();
            ObjManagerUser = new UserRegistration();            //UserType _UserType;
            try
            {
                if (_ICommonMethod.IsEmailExist(objUserModel.UserEmail)) { throw new Exception(Helper.CommonMessage.EmailExists(objUserModel.UserEmail)); }

                if (objUserModel.UserType == Convert.ToInt64(UserType.Employee, CultureInfo.InvariantCulture))
                { if (_ICommonMethod.IsEmployeeIdExist(objUserModel.EmployeeID)) { throw new Exception(Helper.CommonMessage.EmployeeIdExists(objUserModel.EmployeeID)); } }

                if (_ICommonMethod.IsUserExists(objUserModel.FirstName, objUserModel.UserType)) { throw new Exception(Helper.CommonMessage.DuplicateRecordMessage()); }

                if (CheckIsUserRegistered(objUserModel.UserEmail, objUserModel.EmployeeID))
                {
                    objUserModel.IsEmailVerify = false;
                    objUserModel.IsLoginActive = false;
                    objUserModel.Password = Cryptography.GetEncryptedData(objUserModel.FirstName + "@123", true);
                    objUserModel.SubscriptionEmail = objUserModel.UserEmail;
                    //objUserModel.UserType = objGlobalCodesRepository.GetSingleOrDefault(u => u.CodeName == userType).GlobalCodeId;                    //objUserModel.UserType = _userType;
                    objUserModel.myProfileImage = "no-profile-pic.jpg";
                    AutoMapper.Mapper.CreateMap<UserModel, UserRegistration>();
                    ObjManagerUser = AutoMapper.Mapper.Map(objUserModel, ObjManagerUser);
                    ObjUserRepository.Add(ObjManagerUser);
                    long userid = ObjManagerUser.UserId;

                    #region EmailHelper
                    if (userid > 0)
                    {
                        EmailHelper objEmailHelper = new EmailHelper();
                        objEmailHelper.emailid = objUserModel.UserEmail;
                        objEmailHelper.FirstName = objUserModel.FirstName;
                        objEmailHelper.LastName = objUserModel.LastName;
                        objEmailHelper.MailType = "REGISTRATIONMAIL";
                        string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
                        string UserLink = "";                           //Enum.TryParse(userType, out _UserType);                           //long _userType = (long)_UserType;                        
                        long _userType = objUserModel.UserType;
                        switch (_userType)
                        {
                            case (long)UserType.Manager:
                                UserLink = "GlobalAdmin/Manager";
                                break;
                            case (long)UserType.Employee:
                                UserLink = "Manager/Employee";
                                break;
                            case (long)UserType.Client:
                                UserLink = "Client/Client";
                                break;
                            default:
                                UserLink = "Error";
                                break;
                        }

                        HostingPrefix = HostingPrefix + UserLink + "?usr=" + Cryptography.GetEncryptedData(userid.ToString(CultureInfo.InvariantCulture), true);
                        objEmailHelper.RegistrationLink = HostingPrefix;

                        #region comments
                        // objEmailHelper.RegistrationLink = DomainName + "/?flag=Registration&id=" + System.Web.HttpUtility.UrlPathEncode(Cryptography.GetEncryptedData(UserId.ToString(), true));
                        //objEmailHelper.RegistrationLink = DomainName + "/?flag=Registration&id=" + System.Web.HttpUtility.UrlPathEncode(Cryptography.GetEncryptedData(UserId.ToString(), true));
                        // objEmailHelper.RegistrationCode = objRegistrationModel.EmailVerifcationCode;
                        #endregion comments

                        objEmailHelper.SendEmailWithTemplate();
                        return Result.EmailSendSuccessfully;
                    }

                    #endregion EmailHelper
                }
                else
                {
                    return Result.DuplicateRecordEmail;
                }
                return Result.Completed;
            }
            catch (Exception)
            { throw; }
        }

        public bool CheckIsUserRegistered(string userEmail, string employeeId)
        {
            ObjUserRepository = new UserRepository();
            int count = ObjUserRepository.GetAll(u => u.UserEmail == userEmail && u.EmployeeID == employeeId).Count();
            if (count > 0)
                return false;
            return true;
        }

        #endregion

        #region Assign Project
        /// <summary>Get List of Verfied Mnagaer
        /// <Createdby>Gayatri Pal</Createdby>
        /// <CreatedDate>05-Sep-2014</CreatedDate>
        /// </summary>
        public List<UserModelList> GetAllVerifiedManager(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords)
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

        public Result AssignProject(UserModel objUserModel)
        {
            ObjUserRepository = new UserRepository();
            try
            {
                var data = ObjUserRepository.GetSingleOrDefault(u => u.UserId == objUserModel.UserId);
                if (data != null)
                {
                    data.ProjectID = objUserModel.ProjectID;
                    data.HiringDate = objUserModel.HiringDate;
                    data.ModifiedBy = objUserModel.ModifiedBy;
                    data.ModifiedDate = objUserModel.ModifiedDate;
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
        #endregion

        /// <summary>GetAllITAdministratorList
        /// <Modified By>Bhushan Dod & Vijay Sahu</Modified> 
        /// <CreatedFor>Get All IT Administrator List</CreatedFor>
        /// <CreatedOn>Nov-14-2014</CreatedOn>
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="PageIndex"></param>
        /// <param name="NumberOfRows"></param>
        /// <param name="SortColumnName"></param>
        /// <param name="SortOrderBy"></param>
        /// <param name="SearchText"></param>
        /// <returns></returns>
        public List<UserModelList> GetAllITAdministratorList(long? userId, long locationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string searchText, string myUserType, out long totalRecords)
        {
            ObjUserRepository = new UserRepository();
            try
            {

                return ObjUserRepository.GetAllVerfiedUsers(userId, locationId, myUserType, pageIndex, numberOfRows, sortColumnName, sortOrderBy, searchText, out totalRecords);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<UserModelList> GetAllITAdministratorListForReport(long? userId, long locationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string searchText, string myUserType, out long totalRecords)
        {
            ObjUserRepository = new UserRepository();
            try
            {

                return ObjUserRepository.GetAllVerfiedUsersForReport(userId, locationId, myUserType, pageIndex, numberOfRows, sortColumnName, sortOrderBy, searchText, out totalRecords);
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>Get All user list associated to locaton for DAR
        /// <Modified By>Bhushan Dod & Vijay Sahu</Modified> 
        /// <CreatedFor>Get All IT Administrator List</CreatedFor>
        /// <CreatedOn>Nov-14-2014</CreatedOn>
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="PageIndex"></param>
        /// <param name="NumberOfRows"></param>
        /// <param name="SortColumnName"></param>
        /// <param name="SortOrderBy"></param>
        /// <param name="SearchText"></param>
        /// <returns></returns>
        public List<UserModelList> GetAllUserListforDAR(long? userId, long locationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string searchText, string myUserType, out long totalRecords)
        {
            ObjUserRepository = new UserRepository();
            try
            {

                return ObjUserRepository.GetAllVerfiedUsersDAROnly(userId, locationId, myUserType, pageIndex, numberOfRows, sortColumnName, sortOrderBy, searchText, out totalRecords);
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary> GetLocationListAdministratorClient
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedFor>Get Location List Administrator or Client</CreatedFor>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="CalledForUserType"></param>
        /// <returns></returns>
        public List<UserModelList> GetLocationListAdministratorClient(string locationId, string UserType = "")
        {
            List<UserModelList> ResultUserList = new List<UserModelList>();
            try
            {
                if (!string.IsNullOrEmpty(locationId))
                {

                    locationId = Cryptography.GetDecryptedData(locationId, true); long _locationId = 0;
                    if (long.TryParse(locationId, out _locationId))
                    {
                        ObjUserRepository = new UserRepository();
                        ResultUserList = ObjUserRepository.ListLocationAdministrator(_locationId, UserType);



                        /*//kamal
                        ResultUserList = ObjUserRepository.GetAll(u => u.AdminLocationMappings.FirstOrDefault().AdminUserId != u.UserId && u.UserType == (long)UserType.Administrator && u.IsDeleted == false && u.IsEmailVerify == true && u.IsLoginActive == true)
                                .Select(t =>
                                new UserModelList()
                                {
                                    UserId = t.UserId,
                                    ProjectID = t.ProjectID,
                                    UserEmail = t.UserEmail,
                                    DOB = t.DOB,
                                    Name = t.FirstName + (!string.IsNullOrEmpty(t.LastName) ? t.LastName : string.Empty),
                                    HiringDate = t.HiringDate,
                                    EmployeeCategoryid = t.EmployeeCategory,
                                    EmployeeID = t.EmployeeID,
                                    LocationId = t.AdminLocationMappings.FirstOrDefault().LocationId
                                }).Where(s => s.LocationId == _LocationId).ToList();
                        */

                    }
                }
                return ResultUserList;
            }
            catch (Exception)
            { throw; }


        }

        /// <summary> GetLocationListAdministratorClient
        /// <CreatedBy>Vijay sahu</CreatedBy>
        /// <CreatedFor>Get un assigned Administrator User</CreatedFor>
        /// </summary>
        /// <param name="LocationId"></param> 
        /// <returns></returns>
        public List<SelectListItem> UnAssignedAdministratorId(string locationId, string UserType)
        {
            List<AdminUserForDrop> ResultUserList = new List<AdminUserForDrop>();
            List<SelectListItem> lst = new List<SelectListItem>();
            try
            {
                if (!string.IsNullOrEmpty(locationId))
                {
                    locationId = Cryptography.GetDecryptedData(locationId, true); long _locationId = 0;
                    if (long.TryParse(locationId, out _locationId))
                    {
                        ObjUserRepository = new UserRepository();
                        ResultUserList = ObjUserRepository.UnAssignedAdministrationIdRepo(_locationId, UserType);
                        //lst = ResultUserList.Select(x => new SelectListItem() { Text = x.Name, Value = x.UserId }).ToList();
                        SelectListItem item = null;
                        foreach (var a in ResultUserList)
                        {
                            item = new SelectListItem();
                            item.Value = a.UserId.ToString();
                            item.Text = a.Name;
                            lst.Add(item);
                        }
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {

                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public List<SelectListItem> UnAssignedAdministratorId(string locationId, string UserType)", "Exception while getting user List", locationId);
                throw ex;
            }


        }

        ///// <summary> GetLocationListAdministratorClient
        ///// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        ///// <CreatedFor>Get Location List Administrator or Client</CreatedFor>
        ///// </summary>
        ///// <param name="LocationId"></param>
        ///// <param name="CalledForUserType"></param>
        ///// <returns></returns>
        //public List<UserModelList> GetLocationListAdministratorClient_Wrong(string locationId, UserType calledForUserType = UserType.Administrator)
        //{
        //    List<UserModelList> ResultUserList = new List<UserModelList>();
        //    try
        //    {
        //        long _LocationId = 0;
        //        if (long.TryParse(locationId, out _LocationId))
        //        {

        //            ObjAdminLocationMappingRepository = new AdminLocationMappingRepository();
        //            ObjLocationClientMappingRepository = new LocationClientMappingRepository();

        //            if (calledForUserType == UserType.Client)
        //            {
        //                ResultUserList = ObjLocationClientMappingRepository.GetAll(map => map.LocationId == _LocationId && map.IsDeleted == false)
        //                    .Select(t =>
        //                    new UserModelList()
        //                    {
        //                        UserId = t.UserRegistration.UserId,
        //                        //ProjectID = t.UserRegistration.ProjectID,
        //                        UserEmail = t.UserRegistration.UserEmail,
        //                        DOB = t.UserRegistration.DOB,
        //                        Name = t.UserRegistration.FirstName + (!string.IsNullOrEmpty(t.UserRegistration.LastName) ? t.UserRegistration.LastName : string.Empty),
        //                        HiringDate = t.UserRegistration.HiringDate,
        //                        EmployeeCategoryid = t.UserRegistration.EmployeeCategory,
        //                        EmployeeID = t.UserRegistration.EmployeeID
        //                    }).ToList();
        //            }
        //            else
        //            {
        //                ResultUserList = ObjAdminLocationMappingRepository.GetAll(map => map.LocationId == _LocationId && map.IsDeleted == false).Select(t =>
        //                new UserModelList()
        //                {
        //                    UserId = t.UserRegistration.UserId,
        //                    //ProjectID = t.UserRegistration.ProjectID,
        //                    UserEmail = t.UserRegistration.UserEmail,
        //                    DOB = t.UserRegistration.DOB,
        //                    Name = t.UserRegistration.FirstName + (!string.IsNullOrEmpty(t.UserRegistration.LastName) ? t.UserRegistration.LastName : string.Empty),
        //                    HiringDate = t.UserRegistration.HiringDate,
        //                    EmployeeCategoryid = t.UserRegistration.EmployeeCategory,
        //                    EmployeeID = t.UserRegistration.EmployeeID
        //                }).ToList();
        //            }
        //        }
        //        return ResultUserList;
        //    }
        //    catch (Exception)
        //    { throw; }


        //}

        /// <summary>ProcessLocationSetup
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-19-2014</CreatedOn>
        /// <CreatedFor>Process Location Setup</CreatedFor>
        /// <UpdatedBy>Bhushan Dod</UpdatedBy>
        /// <UpdatedOn>06/07/2016</UpdatedOn>
        /// <UpdatedFor>To add widget bydefault to show on dashboard</UpdatedFor> 
        /// </summary>
        /// <param name="ObjLocationMasterModel"></param>
        /// <returns></returns
        public Result ProcessLocationSetup(LocationMasterModel objLocation, out long verificationManagerId, out long verificationClientId, out long outLocationId, out bool outSendMail)
        {

            try
            {
                ObjUserRepository = new UserRepository();
                ObjLocationRepository = new LocationRepository();
                ObjManagerUser = new UserRegistration();
                ObjClientUser = new UserRegistration();
                ObjLocationMaster = new LocationMaster();
                objGlobalCodesRepository = new GlobalCodesRepository();
                QRCModel ObjQrc = new QRCModel();
                DashboardWidgetSettingManager objDashboardWidgetSettingManager = new DashboardWidgetSettingManager();
                outSendMail = false;


                verificationManagerId = 0;
                outLocationId = 0;
                verificationClientId = 0;


                if (CheckDuplicateLocation(objLocation.LocationName.Trim(), objLocation.LocationId, out ObjQrc))
                {
                    long managerQRCID = 0, clientQRCID = 0;
                    if (objLocation.ManagerModel.IsExistingManager == false)
                    {
                        if (!CheckDuplicateUser(objLocation.ManagerModel.UserEmail.Trim(), objLocation.ManagerModel.UserId, out managerQRCID))
                        {
                            throw new Exception(CommonMessage.UserDuplicateEmail(objLocation.ManagerModel.UserEmail.Trim(), "Manager"));
                        }
                    }
                    if (objLocation.ClientModel.ExistClientID == 0)
                    {
                        if (!CheckDuplicateUser(objLocation.ClientModel.UserEmail.Trim(), objLocation.ClientModel.UserId, out clientQRCID))
                        {
                            throw new Exception(CommonMessage.UserDuplicateEmail(objLocation.ClientModel.UserEmail.Trim(), "Client"));
                        }
                    }

                    //objUserModel.IsEmailVerify = true;
                    //objUserModel.IsLoginActive = true;
                    //objUserModel.SubscriptionEmail = objUserModel.UserEmail;
                    //if (objUserModel.Gender != null) { objUserModel.Gender = objUserModel.Gender == 1 ? objGlobalCodesRepository.GetSingleOrDefault(g => g.CodeName == "Male").GlobalCodeId : objGlobalCodesRepository.GetSingleOrDefault(g => g.CodeName == "Female").GlobalCodeId; }

                    using (TransactionScope TransScope = new TransactionScope())
                    {
                        if (objLocation.LocationId == 0)
                        {
                            AutoMapper.Mapper.CreateMap<LocationMasterModel, LocationMaster>();
                            ObjLocationMaster = AutoMapper.Mapper.Map(objLocation, ObjLocationMaster);
                            //int eTracVerifyLocationFlag = 0;
                            //ObjLocationMaster.IsVerifiedByClient = (string.IsNullOrEmpty(eTracVerifyLocation) || !int.TryParse(eTracVerifyLocation, out eTracVerifyLocationFlag) || eTracVerifyLocationFlag == 0) ? true : false;
                            //ObjLocationMaster.IsVerifiedByManager = (string.IsNullOrEmpty(eTracVerifyLocation) || !int.TryParse(eTracVerifyLocation, out eTracVerifyLocationFlag) || eTracVerifyLocationFlag == 0) ? true : false;


                            ObjLocationRepository.Add(ObjLocationMaster);

                            outLocationId = ObjLocationMaster.LocationId;

                            //Added by Bhushan Dod on 11-August-2016 for objLocation.locationid null due to this not able to send email to client, manager.
                            objLocation.LocationId = ObjLocationMaster.LocationId;
                            objLocation.ManagerModel.Location = ObjLocationMaster.LocationId;
                            objLocation.ClientModel.Location = ObjLocationMaster.LocationId;

                            if (ObjLocationMaster.LocationId > 0)
                            {
                                #region Save Location Services & Widget Setting
                                if (!string.IsNullOrEmpty(objLocation.ServicesID))
                                {
                                    SaveLocationServices(ObjLocationMaster.LocationId, objLocation.ServicesID, false);

                                    //Added By Bhushan Dod on 07/06/2016 for bydefault setting when location created according to loc services.
                                    SaveByDefaultWidgetSetting(ObjLocationMaster.LocationId, objLocation.ServicesID, ObjLocationMaster.CreatedBy);
                                }
                                #endregion Save Location Services & Widget Setting

                                #region Get All Premission List
                                PermissionDetailsModel objPermissionDetails = new PermissionDetailsModel();
                                //var premission = _ICommonMethod.GetAllPermissions(0);//Get All premission List// 0 means all 
                                //string premissionIDs = "";
                                //foreach (var pre in premission)
                                //{
                                //    premissionIDs = premissionIDs + pre.PermissionId + ",";
                                //}
                                objPermissionDetails.CreatedBy = Convert.ToInt64(ObjLocationMaster.CreatedBy);
                                objPermissionDetails.LocationId = ObjLocationMaster.LocationId;
                                objPermissionDetails.UserIds = objLocation.ServicesID;
                                objPermissionDetails.CreatedOn = DateTime.UtcNow;
                                #endregion

                                #region Administrator Mapping
                                AdminLocationMappingRepository obj_AdminLocationMappingRepository = new AdminLocationMappingRepository();
                                AdminLocationMapping obj_AdminLocationMapping = new AdminLocationMapping();
                                obj_AdminLocationMapping.LocationId = outLocationId;
                                obj_AdminLocationMapping.AdminUserId = objLocation.ManagerModel.Administrator;
                                obj_AdminLocationMapping.MappedBy = Convert.ToInt64(objLocation.CreatedBy);
                                obj_AdminLocationMapping.CreatedOn = DateTime.UtcNow;
                                obj_AdminLocationMapping.IsDeleted = false;
                                obj_AdminLocationMappingRepository.Add(obj_AdminLocationMapping);
                                obj_AdminLocationMappingRepository.SaveChanges();

                                objPermissionDetails.UserId = objLocation.ManagerModel.Administrator;
                                var AdminPremissionResult = _ICommonMethod.UpdateUserPermissions(objPermissionDetails);
                                //Added By Bhushan Dod on 07/06/2016 for bydefault setting when location created according to loc services.
                                SaveByDefaultWidgetSetting(ObjLocationMaster.LocationId, objLocation.ServicesID, objLocation.ManagerModel.Administrator);
                                #endregion

                                #region Manager User Record

                                long managerUSerId = 0;
                                if (objLocation.ManagerModel.ExistManagerID == 0)
                                {
                                    objLocation.ManagerModel.SubscriptionEmail = string.IsNullOrEmpty(objLocation.ManagerModel.SubscriptionEmail) ? objLocation.ManagerModel.UserEmail : objLocation.ManagerModel.SubscriptionEmail;
                                    objLocation.ManagerModel.FirstName = objLocation.ManagerModel.FirstName.ToTitleCase();
                                    objLocation.ManagerModel.LastName = objLocation.ManagerModel.LastName.ToTitleCase();
                                    AutoMapper.Mapper.CreateMap<UserModel, UserRegistration>();
                                    string dob = objLocation.ManagerModel.DOB;
                                    objLocation.ManagerModel.DOB = null;
                                    ObjManagerUser = AutoMapper.Mapper.Map(objLocation.ManagerModel, ObjManagerUser);
                                    if (!string.IsNullOrEmpty(dob))
                                    {
                                        ObjManagerUser.DOB = Convert.ToDateTime(dob);
                                    }
                                    ObjManagerUser.IsLoginActive = true; //if manager already exists then user will automatically veryfied. 
                                    ObjManagerUser.CreatedDate = objLocation.CreatedDate;
                                    ObjManagerUser.CreatedBy = Convert.ToInt64(ObjLocationMaster.CreatedBy);
                                    ObjManagerUser.UserType = Convert.ToInt64(UserType.Manager, CultureInfo.InvariantCulture);
                                    if (!string.IsNullOrEmpty(ObjManagerUser.Password))
                                    {
                                        ObjManagerUser.Password = Cryptography.GetEncryptedData(ObjManagerUser.Password, true);
                                    }
                                    if (objLocation.ManagerModel.ProfileImage != null && !string.IsNullOrEmpty(objLocation.ManagerModel.ProfileImage.FileName)) { ObjManagerUser.ProfileImage = objLocation.ManagerModel.ProfileImageFile; }

                                    ObjUserRepository.Add(ObjManagerUser);
                                    if (ObjManagerUser.UserId > 0)
                                    {
                                        objLocation.ManagerModel.UserId = managerUSerId = ObjManagerUser.UserId;
                                        objLocation.ManagerModel.Address.UserId = ObjManagerUser.UserId;
                                        objAddressManager.SaveAddress(objLocation.ManagerModel.Address);

                                        //Added By Bhushan Dod on 07/06/2016 for bydefault setting when location created according to loc services.
                                        SaveByDefaultWidgetSetting(ObjLocationMaster.LocationId, objLocation.ServicesID, ObjManagerUser.UserId);
                                    }

                                    objLocation.ManagerModel.UserEmail = ObjManagerUser.UserEmail;
                                    objLocation.ManagerModel.AlternateEmail = ObjManagerUser.AlternateEmail;
                                    objLocation.ManagerModel.SubscriptionEmail = ObjManagerUser.SubscriptionEmail;
                                    //objPermissionDetails.UserId = ObjManagerUser.UserId;
                                    //var ManagerPremissionResult = _ICommonMethod.UpdateUserPermissions(objPermissionDetails);
                                }
                                else // manager came from dropdown. 
                                {

                                    using (workorderEMSEntities Context = new workorderEMSEntities())
                                    {
                                        var ab = (from o in Context.UserRegistrations
                                                  where o.UserId == objLocation.ManagerModel.ExistManagerID
                                                  && o.IsDeleted == false
                                                  select o
                                                  ).FirstOrDefault();

                                        objLocation.ManagerModel.FirstName = ab.FirstName;
                                        objLocation.ManagerModel.LastName = ab.LastName;
                                        objLocation.ManagerModel.UserId = managerUSerId = ObjManagerUser.UserId;
                                        objLocation.ManagerModel.UserEmail = ab.UserEmail;
                                        objLocation.ManagerModel.Password = ab.Password;
                                        objLocation.ManagerModel.AlternateEmail = ab.AlternateEmail;
                                        objLocation.ManagerModel.SubscriptionEmail = ab.AlternateEmail;
                                    }
                                    managerUSerId = objLocation.ManagerModel.ExistManagerID;

                                    //objPermissionDetails.UserId = managerUSerId;
                                    //var ManagerPremissionResult = _ICommonMethod.UpdateUserPermissions(objPermissionDetails);

                                    //Added By Bhushan Dod on 07/06/2016 for bydefault setting when location created according to loc services.
                                    SaveByDefaultWidgetSetting(ObjLocationMaster.LocationId, objLocation.ServicesID, managerUSerId);
                                }

                                #region ManagerLocationMappingRepository

                                //ManagerLocationMappingRepository ObjManagerLocationMappingRepository;                                if (managerUSerId > 0)
                                {
                                    ManagerLocationMapping ObjManagerLocationMapping = new ManagerLocationMapping();
                                    ObjManagerLocationMapping.ManagerUserId = managerUSerId;
                                    ObjManagerLocationMapping.CreatedBy = objLocation.CreatedBy.HasValue ? objLocation.CreatedBy.Value : 3;
                                    ObjManagerLocationMapping.CreatedOn = objLocation.CreatedDate;
                                    ObjManagerLocationMapping.MappedBy = objLocation.CreatedBy.HasValue ? objLocation.CreatedBy.Value : 3;
                                    ObjManagerLocationMapping.LocationId = ObjLocationMaster.LocationId;
                                    ObjManagerLocationMappingRepository = new ManagerLocationMappingRepository();
                                    ObjManagerLocationMappingRepository.Add(ObjManagerLocationMapping);
                                    verificationManagerId = ObjManagerLocationMapping.ManagerLocationMappingId;
                                }

                                #endregion ManagerLocationMappingRepository

                                #endregion Manager User Record

                                #region Client User Record

                                long clientUSerId = 0;
                                //if (objLocation.ClientModel.Is == true)
                                //{
                                objLocation.ClientModel.FirstName = objLocation.ClientModel.FirstName.ToTitleCase();
                                objLocation.ClientModel.LastName = objLocation.ClientModel.LastName.ToTitleCase();
                                objLocation.ClientModel.SubscriptionEmail = string.IsNullOrEmpty(objLocation.ClientModel.SubscriptionEmail) ? objLocation.ClientModel.UserEmail : objLocation.ClientModel.SubscriptionEmail;
                                AutoMapper.Mapper.CreateMap<UserModel, UserRegistration>();
                                string DOB = objLocation.ClientModel.DOB;
                                objLocation.ClientModel.DOB = null;
                                ObjClientUser = AutoMapper.Mapper.Map(objLocation.ClientModel, ObjClientUser);
                                ObjClientUser.IsLoginActive = true;
                                if (string.IsNullOrEmpty(DOB))
                                {
                                    ObjClientUser.DOB = null; //commented by vijay sahu on 24 march 2015
                                }
                                else
                                { ObjClientUser.DOB = Convert.ToDateTime(DOB); }

                                //ObjClientUser.DOB = (string.IsNullOrEmpty(DOB) == true) ? (DateTime.Now) : (Convert.ToDateTime(DOB));

                                ObjClientUser.CreatedDate = objLocation.CreatedDate;
                                ObjClientUser.CreatedBy = Convert.ToInt64(ObjLocationMaster.CreatedBy);
                                ObjClientUser.UserType = Convert.ToInt64(UserType.Client, CultureInfo.InvariantCulture);
                                if (!string.IsNullOrEmpty(ObjClientUser.Password))
                                {
                                    ObjClientUser.Password = Cryptography.GetEncryptedData(ObjClientUser.Password, true);
                                }
                                if (objLocation.ClientModel.ProfileImage != null && !string.IsNullOrEmpty(objLocation.ClientModel.ProfileImage.FileName)) { ObjClientUser.ProfileImage = objLocation.ClientModel.ProfileImageFile; }

                                ObjUserRepository.Add(ObjClientUser);
                                if (ObjClientUser.UserId > 0)
                                {
                                    objLocation.ClientModel.UserId = clientUSerId = ObjClientUser.UserId;
                                    objLocation.ClientModel.Address.UserId = ObjClientUser.UserId;
                                    objAddressManager.SaveAddress(objLocation.ClientModel.Address);
                                }
                                //}
                                //else { clientUSerId = objLocation.ClientModel.UserId; }

                                //commented by vijay sahu on 20 may 2015
                                //objPermissionDetails.UserId = ObjClientUser.UserId;
                                //var ClientPremissionResult = _ICommonMethod.UpdateUserPermissions(objPermissionDetails);
                                //end comment
                                #endregion Client User Record

                                #region LocationClientMappingRepository

                                LocationClientMappingRepository ObjLocationClientMappingRepository;
                                if (managerUSerId > 0)
                                {
                                    LocationClientMapping _LocationClientMapping = new LocationClientMapping();
                                    _LocationClientMapping.LocationId = objLocation.LocationId;
                                    _LocationClientMapping.ClientUserId = ObjClientUser.UserId;
                                    _LocationClientMapping.CreatedBy = objLocation.CreatedBy.HasValue ? objLocation.CreatedBy.Value : 1;
                                    _LocationClientMapping.CreatedOn = objLocation.CreatedDate;
                                    _LocationClientMapping.MappedBy = objLocation.CreatedBy.HasValue ? objLocation.CreatedBy.Value : 1;
                                    _LocationClientMapping.LocationId = ObjLocationMaster.LocationId;
                                    _LocationClientMapping.IsPrimaryClient = true;
                                    ObjLocationClientMappingRepository = new LocationClientMappingRepository();
                                    ObjLocationClientMappingRepository.Add(_LocationClientMapping);
                                    verificationClientId = _LocationClientMapping.LocationClientMappingID;
                                }

                                #endregion ManagerLocationMappingRepository

                                #region Employee User Record
                                commonMethodManager = new CommonMethodManager();


                                InsertUpdateEmployee(objLocation.EmployeeListModel, false, objLocation.CreatedDate, objLocation.CreatedBy.Value, outLocationId, out outSendMail, false);
                                // In last parameter I am passing False bcz I dont want give permission of any rules by default

                                #endregion Client User Record

                                //test chanegs
                                TransScope.Complete();
                                //TransScope.Dispose();
                                return Result.Completed;
                            }
                            else
                            {
                                //TransScope.Dispose();
                                return Result.Failed;
                            }
                        }
                        else
                        {
                            UpdateLocation(objLocation, out ObjQrc);
                            if (!string.IsNullOrEmpty(objLocation.ServicesID))
                            {
                                SaveLocationServices(objLocation.LocationId, objLocation.ServicesID, true);
                                //Added By Bhushan Dod on 07/06/2016 for bydefault setting when location created according to loc services.
                                SaveByDefaultWidgetSetting(objLocation.LocationId, objLocation.ServicesID, objLocation.ModifiedBy);
                            }

                            #region Get All Premission List

                            string[] aServicesIDStr = objLocation.ServicesID.Split(',');
                            List<long> aServicesID = new List<long>();

                            for (int i = 0; i < aServicesIDStr.Length; i++)
                            {
                                aServicesID.Add(Convert.ToInt32(aServicesIDStr[i]));
                            }

                            PermissionDetailsRepository objDeta = new PermissionDetailsRepository();

                            objDeta.DeleteAll(x => !aServicesID.Contains(x.PermissionId) && x.LocationId == objLocation.LocationId);

                            objDeta.SaveChanges();


                            #endregion

                            //ObjLocation.Address.UserId = objUserModel.UserId;
                            //objAddressManager.SaveAdress(objUserModel.Address);
                            InsertUpdateEmployee(objLocation.EmployeeListModel, true, (objLocation.ModifiedDate.HasValue ? objLocation.ModifiedDate.Value : DateTime.UtcNow), (objLocation.ModifiedBy.HasValue ? objLocation.ModifiedBy.Value : 1), objLocation.LocationId, out outSendMail);

                            TransScope.Complete();
                            return Result.UpdatedSuccessfully;
                        }
                    }
                }
                else
                { return Result.DuplicateRecord; }
            }
            catch (Exception ex)
            {
                //Exception_B.Exception_B.exceptionHandel_Runtime(ex, "InsertUpdateEmployee", "Exception While creating location", objLocation);
                throw;
            }
        }

        /// <summary>InsertUpdateEmployee
        /// <CreatedBy>Roshan Rahood</CreatedBy>
        /// <CreatedDate>Dec-26-2014</CreatedDate>
        /// <CreatedFor>Insert Update Employee Details</CreatedFor>
        /// <param name="EmployeeListModel"></param>
        /// <param name="IsUpdate"></param>
        /// <param name="CreatedDate"></param>
        /// <param name="CreatedBy"></param>
        /// <param name="LocationId"></param>
        /// <param name="OutSendMail"></param>
        /// <returns></returns>
        public bool InsertUpdateEmployee(List<UserModel> employeeListModel, bool isUpdate, DateTime createdDate, long createdBy, long locationId, out bool outSendMail, bool wantToGiveLocationService = true)
        {
            bool _insertStatus = false;
            outSendMail = false;
            long clientQRCID = 0;
            long employeeUserId = 0;
            commonMethodManager = new CommonMethodManager();

            try
            {
                if (employeeListModel != null)
                {
                    foreach (var employee in employeeListModel)
                    {
                        if (!CheckDuplicateUser(employee.UserEmail.Trim(), employee.UserId, out clientQRCID)) { throw new Exception(CommonMessage.UserDuplicateEmail(employee.UserEmail.Trim(), "Employee")); }

                        if (employee.UserId == 0)
                        {
                            // Employee User Insert Mode
                            #region Employee User Insert Mode
                            employee.SubscriptionEmail = string.IsNullOrEmpty(employee.SubscriptionEmail) ? employee.UserEmail : employee.SubscriptionEmail;
                            ObjEmployeeUser = new UserRegistration();
                            employee.CreatedDate = DateTime.UtcNow;
                            //AutoMapper.Mapper.CreateMap<UserModel, UserRegistration>();
                            string DBO = employee.DOB;
                            employee.DOB = null;
                            //ObjEmployeeUser = 
                            employee.FirstName = employee.FirstName.ToTitleCase();
                            employee.LastName = employee.LastName.ToTitleCase();
                            AutoMapper.Mapper.DynamicMap(employee, ObjEmployeeUser);
                            if (!string.IsNullOrEmpty(DBO))
                            {
                                employee.DOB = DBO;

                                try
                                {
                                    ObjEmployeeUser.DOB = Convert.ToDateTime(DBO);
                                }
                                catch (Exception)
                                {
                                    ObjEmployeeUser.DOB = null;

                                }
                            }
                            if (!string.IsNullOrEmpty(employee.Password))
                            {
                                ObjEmployeeUser.Password = Cryptography.GetEncryptedData(employee.Password, true);
                            }
                            //ObjEmployeeUser.IsEmailVerify = true;
                            ObjEmployeeUser.IsLoginActive = true;
                            //ObjEmployeeUser.DOB =
                            ObjEmployeeUser.IdleTimeLimit = DateTime.UtcNow.SetTime(0, 30, 0, 0);//Added By Bhushan on 07/06/2015 for by deafult IDLE Time set
                            ObjEmployeeUser.CreatedDate = createdDate;
                            ObjEmployeeUser.CreatedBy = createdBy;
                            ObjEmployeeUser.UserType = Convert.ToInt64(UserType.Employee, CultureInfo.InvariantCulture);
                            ObjEmployeeUser.ProfileImage = employee.ProfileImageFile;
                            ObjUserRepository.Add(ObjEmployeeUser);


                            if (wantToGiveLocationService == true) // this condition was being added just to know that you want to give some default permissions or not 
                            {
                                #region Default Premission
                                PermissionDetailsModel objPermissionDetails = new PermissionDetailsModel();
                                var premission = _ICommonMethod.GetAllPermissions(0);//Get All premission List
                                string premissionIDs = "";
                                foreach (var pre in premission)
                                {
                                    premissionIDs = premissionIDs + pre.PermissionId + ",";
                                }
                                objPermissionDetails.CreatedBy = Convert.ToInt64(createdBy);
                                objPermissionDetails.LocationId = locationId;
                                objPermissionDetails.UserIds = premissionIDs;
                                objPermissionDetails.CreatedOn = DateTime.UtcNow;
                                #endregion

                                if (ObjEmployeeUser.UserId > 0)
                                {
                                    objPermissionDetails.UserId = ObjEmployeeUser.UserId;
                                    var EmployeePremissionResult = _ICommonMethod.UpdateUserPermissions(objPermissionDetails);
                                }
                            }


                            if (ObjEmployeeUser.UserId > 0)
                            {


                                //clientUSerId = ObjClientUser.UserId;
                                employeeUserId = ObjEmployeeUser.UserId;

                                employee.Address.UserId = employeeUserId;
                                objAddressManager.SaveAddress(employee.Address);

                                objEmployeeLocationMappingRepository = new EmployeeLocationMappingRepository();
                                objEmployeeMapping = new EmployeeLocationMapping();
                                if (!string.IsNullOrEmpty(employee.ProfileImageFile))
                                    ObjEmployeeUser.ProfileImage = employee.ProfileImageFile;
                                objEmployeeMapping.EmployeeUserId = employeeUserId;
                                objEmployeeMapping.LocationId = locationId;
                                objEmployeeMapping.CreatedBy = createdBy;
                                objEmployeeMapping.CreatedOn = createdDate;
                                objEmployeeMapping.IsDeleted = false;
                                objEmployeeLocationMappingRepository.Add(objEmployeeMapping);
                                employee.UserId = objEmployeeMapping.EmployeeUserId; // employee Id will assigned to this model.
                                outSendMail = true;
                            }
                            //}
                            //else { clientUSerId = ObjLocation.ClientModel.ExistManagerID; }

                            #endregion Employee User Insert Mode
                            // Employee User Insert Mode End
                        }
                        else
                        {
                            // Employee User Update Mode
                            #region Employee User Update Mode
                            ObjEmployeeUser = ObjUserRepository.GetSingleOrDefault(u => u.UserId == employee.UserId && u.IsDeleted == false);
                            if (!string.IsNullOrEmpty(employee.Password))//Password Section
                            {
                                employee.Password = Cryptography.GetEncryptedData(employee.Password, true);
                            }
                            //employee.Password = commonMethodManager.CreateRandomPassword();
                            employee.SubscriptionEmail = string.IsNullOrEmpty(employee.SubscriptionEmail) ? employee.UserEmail : employee.SubscriptionEmail;
                            ObjEmployeeUser.FirstName = employee.FirstName.ToTitleCase();
                            ObjEmployeeUser.LastName = employee.LastName.ToTitleCase();
                            if (!string.IsNullOrEmpty(employee.DOB))
                            {
                                ObjEmployeeUser.DOB = Convert.ToDateTime(employee.DOB);
                            }

                            //Code to Map User for Update Employee User
                            if (!string.IsNullOrEmpty(employee.ProfileImageFile))
                                ObjEmployeeUser.ProfileImage = employee.ProfileImageFile;
                            ObjEmployeeUser.ModifiedDate = createdDate;
                            ObjEmployeeUser.ModifiedBy = createdBy;
                            // ObjUserRepository.SaveChanges();
                            ObjUserRepository.Update(ObjEmployeeUser);


                            employee.Address.UserId = employee.UserId;
                            objAddressManager.SaveAddress(employee.Address);
                            // after Update Mapping;
                            //Code to Map User for Update Employee User End
                            #endregion Employee User Update Mode
                            // Employee User Update Mode End
                        }
                        _insertStatus = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "InsertUpdateEmployee", "Exception While creating location", employeeListModel);
                throw ex;
            }



            return _insertStatus;
        }

        public void UpdateLocation(LocationMasterModel objLocationMasterModel, out QRCModel qrcDetail)
        {
            try
            {

                UserRepository _UserRepository = new UserRepository();
                #region UpdateLocation
                var data = objLocationRepository.GetSingleOrDefault(l => l.LocationId == objLocationMasterModel.LocationId && l.IsDeleted == false);
                qrcDetail = GetQRCDetails((data.QRCID.HasValue) ? data.QRCID.Value : 0);
                if (data != null)
                {
                    //data.LocationId = objLocationMasterModel.LocationId;
                    data.LocationName = objLocationMasterModel.LocationName;
                    data.Description = objLocationMasterModel.Description;
                    data.Address1 = objLocationMasterModel.Address1;
                    data.Address2 = objLocationMasterModel.Address2;
                    data.City = objLocationMasterModel.City;
                    data.StateId = objLocationMasterModel.StateId;
                    data.CountryId = objLocationMasterModel.CountryId;
                    data.Mobile = objLocationMasterModel.Mobile;
                    data.PhoneNo = objLocationMasterModel.PhoneNo;
                    data.ZipCode = objLocationMasterModel.ZipCode;
                    objLocationRepository.SaveChanges();
                }
                #endregion

                #region Administrator Mapping
                AdminLocationMappingRepository obj_AdminLocationMappingRepository = new AdminLocationMappingRepository();


                AdminLocationMapping obj_AdminLocationMapping = obj_AdminLocationMappingRepository.GetAll(x => x.LocationId == objLocationMasterModel.LocationId && x.AdminUserId == objLocationMasterModel.ManagerModel.Administrator && x.IsDeleted == false).SingleOrDefault();
                if (obj_AdminLocationMapping == null)
                {
                    obj_AdminLocationMapping = new AdminLocationMapping();
                    obj_AdminLocationMapping.LocationId = objLocationMasterModel.LocationId;
                    obj_AdminLocationMapping.AdminUserId = objLocationMasterModel.ManagerModel.Administrator;
                    obj_AdminLocationMapping.MappedBy = Convert.ToInt64(objLocationMasterModel.CreatedBy);
                    obj_AdminLocationMapping.CreatedOn = DateTime.UtcNow;
                    obj_AdminLocationMapping.IsDeleted = false;
                    obj_AdminLocationMappingRepository.Add(obj_AdminLocationMapping);
                    obj_AdminLocationMappingRepository.SaveChanges();
                }
                else
                {
                    obj_AdminLocationMapping.LocationId = objLocationMasterModel.LocationId;
                    obj_AdminLocationMapping.AdminUserId = objLocationMasterModel.ManagerModel.Administrator;
                    obj_AdminLocationMapping.MappedBy = Convert.ToInt64(objLocationMasterModel.ModifiedBy);
                    obj_AdminLocationMapping.ModifiedOn = DateTime.UtcNow;
                    obj_AdminLocationMapping.IsDeleted = false;
                    obj_AdminLocationMappingRepository.SaveChanges();
                }
                #endregion

                #region Update Manager
                if (objLocationMasterModel.ManagerModel.IsExistingManager == true)
                {
                    if (objLocationMasterModel.ManagerModel.UserId == 0)
                    {
                        #region Default Premission
                        PermissionDetailsModel objPermissionDetails = new PermissionDetailsModel();
                        var premission = _ICommonMethod.GetAllPermissions(0);//Get All premission List
                        string premissionIDs = "";
                        foreach (var pre in premission)
                        {
                            premissionIDs = premissionIDs + pre.PermissionId + ",";
                        }
                        objPermissionDetails.CreatedBy = Convert.ToInt64(objLocationMasterModel.CreatedBy);
                        objPermissionDetails.UserIds = premissionIDs;
                        objPermissionDetails.CreatedOn = DateTime.UtcNow;
                        #endregion
                        _UserRepository = new UserRepository();
                        UserRegistration obj_UserRegistration = new UserRegistration();
                        if (!string.IsNullOrEmpty(objLocationMasterModel.ManagerModel.Password))
                        {
                            objLocationMasterModel.ManagerModel.Password = Cryptography.GetEncryptedData(objLocationMasterModel.ManagerModel.Password, true);
                        }

                        obj_UserRegistration.Password = objLocationMasterModel.ManagerModel.Password;
                        obj_UserRegistration.UserEmail = objLocationMasterModel.ManagerModel.UserEmail;
                        obj_UserRegistration.AlternateEmail = objLocationMasterModel.ManagerModel.AlternateEmail;
                        obj_UserRegistration.SubscriptionEmail = objLocationMasterModel.ManagerModel.UserEmail;
                        obj_UserRegistration.UserType = 2;//to Manager 
                        obj_UserRegistration.ProjectID = objLocationMasterModel.ManagerModel.ProjectID;
                        obj_UserRegistration.FirstName = objLocationMasterModel.ManagerModel.FirstName.ToTitleCase();
                        obj_UserRegistration.LastName = objLocationMasterModel.ManagerModel.LastName.ToTitleCase();
                        obj_UserRegistration.Gender = objLocationMasterModel.ManagerModel.Gender;
                        if (!string.IsNullOrEmpty(objLocationMasterModel.ManagerModel.DOB))
                        {
                            obj_UserRegistration.DOB = Convert.ToDateTime(objLocationMasterModel.ManagerModel.DOB);
                        }
                        //Added by bhushan dod on 30-Sep-2016 for While editing image if browse file button not selected by user then by default image is old one.
                        if (objLocationMasterModel.ManagerModel.ProfileImageFile != null)
                        {
                            obj_UserRegistration.ProfileImage = objLocationMasterModel.ManagerModel.ProfileImageFile;
                        }

                        obj_UserRegistration.IsLoginActive = true;
                        obj_UserRegistration.IsEmailVerify = true;
                        obj_UserRegistration.TimeZoneId = objLocationMasterModel.ManagerModel.TimeZoneId;
                        obj_UserRegistration.CreatedBy = Convert.ToInt64(objLocationMasterModel.ManagerModel.CreatedBy);
                        obj_UserRegistration.CreatedDate = DateTime.UtcNow;
                        obj_UserRegistration.ModifiedBy = objLocationMasterModel.ModifiedBy;
                        obj_UserRegistration.ModifiedDate = objLocationMasterModel.ManagerModel.ModifiedDate; //objLocationMasterModel.ManagerModel.ModifiedDate.Value.ToClientTimeZoneinDateTime();
                        obj_UserRegistration.IsDeleted = false;
                        obj_UserRegistration.DeletedBy = objLocationMasterModel.ManagerModel.DeletedBy;

                        obj_UserRegistration.DeletedDate = objLocationMasterModel.ManagerModel.DeletedDate; //objLocationMasterModel.ManagerModel.DeletedDate.Value.ToClientTimeZoneinDateTime();
                        obj_UserRegistration.VendorID = objLocationMasterModel.ManagerModel.VendorID;
                        obj_UserRegistration.BloodGroup = objLocationMasterModel.ManagerModel.BloodGroup;
                        obj_UserRegistration.EmployeeID = objLocationMasterModel.ManagerModel.EmployeeID;
                        obj_UserRegistration.JobTitle = objLocationMasterModel.ManagerModel.JobTitle;

                        _UserRepository.Add(obj_UserRegistration);
                        _UserRepository.SaveChanges();
                        objPermissionDetails.UserId = obj_UserRegistration.UserId;
                        var ManagerPremissionResult = _ICommonMethod.UpdateUserPermissions(objPermissionDetails);
                        //Address Sestion
                        objLocationMasterModel.ManagerModel.Address.UserId = obj_UserRegistration.UserId;
                        objLocationMasterModel.ManagerModel.Address.CreatedBy = Convert.ToInt64(objLocationMasterModel.ManagerModel.CreatedBy);
                        objLocationMasterModel.ManagerModel.Address.CreatedDate = DateTime.UtcNow;
                        if (objLocationMasterModel.ManagerModel.Address != null)
                        {
                            objAddressManager.SaveAddress(objLocationMasterModel.ManagerModel.Address);
                        }

                        ManagerLocationMappingRepository obj_ManagerLocationMappingRepository = new ManagerLocationMappingRepository();
                        ManagerLocationMapping obj_ManagerLocationMapping = new ManagerLocationMapping();
                        obj_ManagerLocationMapping.ManagerUserId = obj_UserRegistration.UserId;
                        obj_ManagerLocationMapping.LocationId = objLocationMasterModel.LocationId;
                        obj_ManagerLocationMapping.MappedBy = Convert.ToInt64(objLocationMasterModel.ModifiedBy);
                        obj_ManagerLocationMapping.CreatedBy = Convert.ToInt64(objLocationMasterModel.ModifiedBy);
                        obj_ManagerLocationMapping.CreatedOn = DateTime.UtcNow;
                        obj_ManagerLocationMapping.IsDeleted = false;
                        obj_ManagerLocationMappingRepository.Add(obj_ManagerLocationMapping);
                        obj_ManagerLocationMappingRepository.SaveChanges();
                    }
                }
                #endregion

                #region Update Client
                var dataClient = _UserRepository.GetSingleOrDefault(m => m.UserId == objLocationMasterModel.ClientModel.UserId && m.IsDeleted == false);
                if (dataClient != null)
                {
                    dataClient.UserEmail = objLocationMasterModel.ClientModel.UserEmail;
                    dataClient.FirstName = objLocationMasterModel.ClientModel.FirstName.ToTitleCase();
                    dataClient.LastName = objLocationMasterModel.ClientModel.LastName.ToTitleCase();
                    dataClient.Gender = objLocationMasterModel.ClientModel.Gender;
                    //Added by bhushan dod on 30-Sep-2016 for While editing image if browse file button not selected by user then by default image is old one.
                    if (objLocationMasterModel.ClientModel.ProfileImageFile != null)
                    {
                        dataClient.ProfileImage = objLocationMasterModel.ClientModel.ProfileImageFile;
                    }

                    if (!string.IsNullOrEmpty(objLocationMasterModel.ClientModel.DOB)) { dataClient.DOB = Convert.ToDateTime(objLocationMasterModel.ClientModel.DOB); }
                    _UserRepository.SaveChanges();
                }
                if (objLocationMasterModel.ClientModel.Address != null)
                {
                    //objLocationMasterModel.ClientModel.Address.UserId =objLocationMasterModel.ClientModel.UserId;
                    objAddressManager.SaveAddress(objLocationMasterModel.ClientModel.Address);
                }
                #endregion

                objLocationRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "UpdateLocation", "Exception While creating location", objLocationMasterModel);
                throw ex;
            }
        }

        /// <summary>SaveLocationServices
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-19-2014</CreatedOn>
        /// <CreatedFor>Save Location Services</CreatedFor>
        /// MOdified by vijay sahu on 9 march 2015
        /// Modified By Bhushan Dod on 07/06/2016 for bydefault widget added according to services. 
        /// </summary>
        /// <param name="LocationID"></param>
        /// <param name="ServicesID"></param>
        /// <param name="IsEdit"></param>
        /// <returns></returns>
        public bool SaveLocationServices(long locationId, string servicesId, bool isEdit)
        {
            LocationService _LocationService = null;
            bool statusflag = false;
            try
            {
                string[] aServicesID = servicesId.Split(',');
                ObjLocationServicesRepository = new LocationServicesRepository();
                if (isEdit) { ObjLocationServicesRepository.DeleteAll(p => p.LocationID == locationId); }

                for (int i = 0; i < aServicesID.Length; i++)
                {
                    if (aServicesID[i] != null && !string.IsNullOrEmpty(aServicesID[i]) && Convert.ToInt64(aServicesID[i], CultureInfo.InvariantCulture) > 0)
                    {
                        _LocationService = new LocationService();
                        _LocationService.LocationID = locationId;
                        _LocationService.ServiceId = Convert.ToInt64(aServicesID[i], CultureInfo.InvariantCulture);
                        _LocationService.CreatedBy = 1;//Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
                        _LocationService.CreatedOn = DateTime.UtcNow;
                        _LocationService.IsDeleted = false;
                        ObjLocationServicesRepository.Add(_LocationService);
                        statusflag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "SaveLocationServices", "Exception While creating location", locationId);
                throw ex;
            }
            return statusflag;
        }

        /// <summary>MapAdminForLocation
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Dec-10-2014</CreatedOn>
        /// <CreatedFor>Map Admin For Location</CreatedFor>
        /// </summary>
        /// <param name="_LocationID"></param>
        /// <param name="_AdminUserId"></param>
        /// <param name="IsDelete"></param>
        public Tuple<bool, int> MapAdminForLocation(long locationId, long adminUserId, long loginUser, bool? isDelete)
        {
            bool status = false;
            int result = 0;
            try
            {
                ObjAdminLocationMappingRepository = new AdminLocationMappingRepository();
                using (TransactionScope TranScope = new TransactionScope())
                {
                    if (isDelete.HasValue && isDelete == true)
                    {


                        int count2 = 0;
                        //var count = ObjManagerLocationMappingRepository.GetAll(TotalManager => TotalManager.ManagerUserId == managerUserId && TotalManager.IsDeleted == false).Count();

                        workorderEMSEntities context = new workorderEMSEntities();
                        {

                            count2 = (from o in context.AdminLocationMappings
                                      join ur in context.UserRegistrations
                                      on o.AdminUserId equals ur.UserId

                                      where
                                      ur.IsLoginActive == true
                                      && ur.IsEmailVerify == true
                                      && ur.IsDeleted == false
                                      && o.LocationId == locationId
                                      && o.IsDeleted == false
                                      select o.AdminUserId
                                          ).Count();
                            //count2 = (from o in context.ManagerLocationMappings
                            //          where
                            //          o.LocationId == locationId && o.IsDeleted == false
                            //          select o.ManagerUserId).Count();
                        }



                        if (count2 > 1)
                        {

                            List<AdminLocationMapping> ListMapEntity = ObjAdminLocationMappingRepository.GetAll(map => map.AdminUserId == adminUserId
                                && map.LocationId == locationId && map.IsDeleted == false).ToList();

                            if (ListMapEntity != null && ListMapEntity.Count > 0)
                            {
                                foreach (AdminLocationMapping MapEntity in ListMapEntity)
                                {
                                    //MapEntity.LocationId = _LocationID;
                                    //MapEntity.AdminUserId = _AdminUserId;
                                    MapEntity.IsDeleted = true;
                                    MapEntity.DeletedBy = loginUser;
                                    MapEntity.DeletedOn = DateTime.UtcNow;
                                    //ObjAdminLocationMappingRepository.Update(MapEntity);
                                    ObjAdminLocationMappingRepository.Delete(MapEntity);
                                }
                                TranScope.Complete(); status = true;
                            }
                        }
                        else
                        {
                            status = false;
                            result = 20001; // only one admin user exists for this location.
                        }
                    }
                    else
                    {
                        AdminLocationMapping MapEntity = ObjAdminLocationMappingRepository.GetSingleOrDefault(map => map.AdminUserId == adminUserId
                            && map.LocationId == locationId && map.IsDeleted == false);
                        if (MapEntity != null && MapEntity.AdminLocationMappingId > 0)
                        {
                            //throw new Exception("Admin already mapped with this location.");
                            status = true;
                        }
                        else
                        {
                            MapEntity = new AdminLocationMapping();
                            MapEntity.LocationId = locationId;
                            MapEntity.AdminUserId = adminUserId;
                            MapEntity.MappedBy = loginUser;
                            MapEntity.CreatedBy = loginUser;
                            MapEntity.CreatedOn = DateTime.UtcNow;
                            ObjAdminLocationMappingRepository.Add(MapEntity);
                            TranScope.Complete(); status = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "MapAdminForLocation", "Exception While creating location", locationId);
                throw ex;
            }

            Tuple<bool, int> tup = new Tuple<bool, int>(status, result);

            return tup;
        }

        /// <summary>MapAdminForLocation
        /// <CreatedBy>Vijay Sahu</CreatedBy>
        /// <CreatedOn>2015 march 11</CreatedOn>
        /// <CreatedFor>Map Employee For Location</CreatedFor>
        /// </summary>
        /// <param name="_LocationID"></param>
        /// <param name="_AdminUserId"></param>
        /// <param name="IsDelete"></param>
        public bool MapEmployeeForLocation(long locationId, long EmployeeUserId, long loginUser, string locationname, bool? isDelete)
        {
            bool status = false; try
            {
                objEmployeeLocationMappingRepository = new EmployeeLocationMappingRepository();
                using (TransactionScope TranScope = new TransactionScope())
                {
                    if (isDelete.HasValue && isDelete == true)
                    {
                        List<EmployeeLocationMapping> ListMapEntity = objEmployeeLocationMappingRepository.GetAll(map => map.EmployeeUserId == EmployeeUserId
                            && map.LocationId == locationId && map.IsDeleted == false).ToList();

                        if (ListMapEntity != null && ListMapEntity.Count > 0)
                        {
                            foreach (EmployeeLocationMapping MapEntity in ListMapEntity)
                            {
                                ServiceDARModel obj = new ServiceDARModel();
                                obj.CreatedBy = loginUser;
                                obj.CreatedOn = DateTime.UtcNow;
                                obj.LocationId = locationId;
                                obj.UserId = EmployeeUserId;
                                var dd = _IDARManager.UserLocationMappingDelete(obj, locationname, "Employee");// it will mentain the log 
                                var Id = MapEntity.EmployeeUserId;
                                var loca = MapEntity.LocationId;
                                MapEntity.IsDeleted = true;
                                MapEntity.DeletedBy = loginUser;
                                MapEntity.DeletedOn = DateTime.UtcNow;
                                objEmployeeLocationMappingRepository.Delete(MapEntity);
                            }
                            TranScope.Complete(); status = true;
                        }
                    }
                    else
                    {
                        EmployeeLocationMapping MapEntity = objEmployeeLocationMappingRepository.GetSingleOrDefault(map => map.EmployeeUserId == EmployeeUserId
                            && map.LocationId == locationId && map.IsDeleted == false);
                        if (MapEntity != null && MapEntity.EmployeeLocationMappingId > 0)
                        {
                            //throw new Exception("Admin already mapped with this location."); 
                            status = true;

                        }
                        else
                        {
                            MapEntity = new EmployeeLocationMapping();
                            MapEntity.LocationId = locationId;
                            MapEntity.EmployeeUserId = EmployeeUserId;
                            MapEntity.ModifiedBy = loginUser;
                            MapEntity.CreatedBy = loginUser;
                            MapEntity.CreatedOn = DateTime.UtcNow;
                            objEmployeeLocationMappingRepository.Add(MapEntity);
                            TranScope.Complete(); status = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool MapEmployeeForLocation(long locationId, long EmployeeUserId, long loginUser, bool? isDelete)", "Exception While creating location", locationId);
                throw ex;
            }
            return status;
        }

        /// <summary>
        /// Created by vijay sahu on 10 mach 2015 
        /// this function is used for mapping manager with location 
        /// </summary>
        /// <param name="locationId"></param>
        /// <param name="adminUserId"></param>
        /// <param name="loginUser"></param>
        /// <param name="isDelete"></param>
        /// <returns></returns>
        public Tuple<bool, int> MapManagerForLocation(long locationId, long managerUserId, long loginUser, bool? isDelete)
        {
            bool status = false;
            int result = 0;

            try
            {
                ObjManagerLocationMappingRepository = new ManagerLocationMappingRepository();

                using (TransactionScope TranScope = new TransactionScope())
                {
                    if (isDelete.HasValue && isDelete == true)
                    {
                        int count2 = 0;
                        //var count = ObjManagerLocationMappingRepository.GetAll(TotalManager => TotalManager.ManagerUserId == managerUserId && TotalManager.IsDeleted == false).Count();

                        workorderEMSEntities context = new workorderEMSEntities();
                        {
                            //count2 = (from o in context.ManagerLocationMappings
                            //          where
                            //          o.LocationId == locationId && o.IsDeleted == false
                            //          select o.ManagerUserId).Count();


                            count2 = (from o in context.ManagerLocationMappings
                                      join ur in context.UserRegistrations
                                      on o.ManagerUserId equals ur.UserId

                                      where
                                      ur.IsLoginActive == true
                                      && ur.IsEmailVerify == true
                                      && ur.IsDeleted == false
                                      && o.LocationId == locationId
                                      && o.IsDeleted == false
                                      select o.ManagerUserId
                                      ).Count();


                        }



                        if (count2 > 1)
                        {

                            List<ManagerLocationMapping> ListMapEntity = ObjManagerLocationMappingRepository.GetAll(map => map.ManagerUserId == managerUserId
                                && map.LocationId == locationId && map.IsDeleted == false).ToList();



                            if (ListMapEntity != null && ListMapEntity.Count > 0)
                            {
                                foreach (ManagerLocationMapping MapEntity in ListMapEntity)
                                {
                                    MapEntity.IsDeleted = true;
                                    MapEntity.DeletedBy = loginUser;
                                    MapEntity.DeletedOn = DateTime.UtcNow;
                                    //ObjManagerLocationMappingRepository.Update(MapEntity);
                                    //var dd = _IDARManager.UserLocationMappingDelete(obj, locationname, "Employee");// it will mentain the log 
                                    ObjManagerLocationMappingRepository.Delete(MapEntity);

                                }
                                TranScope.Complete(); status = true;
                            }
                        }
                        else
                        {
                            status = false;
                            result = 20001;
                        }
                    }
                    else
                    {
                        ManagerLocationMapping MapEntity = ObjManagerLocationMappingRepository.GetSingleOrDefault(map => map.ManagerUserId == managerUserId
                            && map.LocationId == locationId && map.IsDeleted == false);
                        if (MapEntity != null && MapEntity.ManagerLocationMappingId > 0)
                        {
                            status = true;
                            //throw new Exception("Manager already mapped with this location."); 
                        }
                        else
                        {
                            MapEntity = new ManagerLocationMapping();
                            MapEntity.LocationId = locationId;
                            MapEntity.ManagerUserId = managerUserId;
                            MapEntity.MappedBy = loginUser;
                            MapEntity.CreatedBy = loginUser;
                            MapEntity.CreatedOn = DateTime.UtcNow;
                            ObjManagerLocationMappingRepository.Add(MapEntity);
                            TranScope.Complete();
                            status = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool MapManagerForLocation(long locationId, long managerUserId, long loginUser, bool? isDelete)", "Exception While creating location", locationId);
                throw ex;
            }


            Tuple<bool, int> tup = new Tuple<bool, int>(status, result);

            return tup;

        }

        #region WorkRequestAssignment
        /// <summary>Get the Employee under the Location
        /// <CreatedBy>Gayatri Pal</CreatedBy>
        /// <CreatedOn>Dec-23-2014</CreatedOn>
        /// <CreatedFor>Get the Employee under the Location</CreatedFor>
        /// </summary>
        /// <param name="_LocationID"></param>
        /// <param name="_AdminUserId"></param>
        /// <param name="IsDelete"></param>
        public List<SelectListItem> GetLocationEmployee(long locationId)
        {
            EmployeeLocationMappingRepository objEmployeeLocationMappingRepository = new EmployeeLocationMappingRepository();
            List<SelectListItem> lstEmployee = new List<SelectListItem>();
            try
            {
                //var lst = objEmployeeLocationMappingRepository.GetAll(t => t.LocationId == locationId && t.IsDeleted == false && t.i).Select(x => x.UserRegistration).ToList();
                //lstEmployee = lst.Select(r => new SelectListItem()
                //{
                //    Value = Convert.ToString(r.UserId, CultureInfo.InvariantCulture),
                //    Text = r.FirstName + " " + r.LastName
                //}).ToList();
                return objEmployeeLocationMappingRepository.GetEmployeeByLocation(locationId).Select(r => new SelectListItem()
                {
                    Value = Convert.ToString(r.UserId, CultureInfo.InvariantCulture),
                    Text = r.FirstName.ToTitleCase() + " " + r.LastName.ToTitleCase()
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public WorkRequestAssignmentModel SaveWorkRequestAssignment(WorkRequestAssignmentModel objWorkRequestAssignmentModel)
        {
            string message = string.Empty;
            List<EmailToManagerModel> objEmailReturn = null;
            WorkRequestAssignment objWorkRequestAssignment = new WorkRequestAssignment();
            WorkRequestAssignmentRepository objWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
            try
            {
                if (objWorkRequestAssignmentModel.WorkRequestAssignmentID == 0)
                {
                    if (objWorkRequestAssignmentModel.CustomerName != null && objWorkRequestAssignmentModel.CustomerName != "")
                    {
                        objWorkRequestAssignmentModel.CustomerName = objWorkRequestAssignmentModel.CustomerName.ToTitleCase();
                    }
                    AutoMapper.Mapper.CreateMap<WorkRequestAssignmentModel, WorkRequestAssignment>();
                    objWorkRequestAssignment = AutoMapper.Mapper.Map(objWorkRequestAssignmentModel, objWorkRequestAssignment);
                    if (objWorkRequestAssignment.WorkRequestProjectType == 256) // May be wrong code.Comment by Bhu for while creating WO.
                    {
                        objWorkRequestAssignment.AssignByUserId = null;
                    }
                    else
                    {
                        objWorkRequestAssignment.AssignByUserId = objWorkRequestAssignment.CreatedBy;
                    }

                    if (objWorkRequestAssignmentModel.AssignedTimeInterval != null)
                    {
                        objWorkRequestAssignment.AssignedTime = Convert.ToDateTime(objWorkRequestAssignmentModel.AssignedTimeInterval);
                    }

                    if (objWorkRequestAssignmentModel.CrStartTime != null)
                    {
                        objWorkRequestAssignment.ConStartTime = Convert.ToDateTime(objWorkRequestAssignmentModel.CrStartTime).ToUniversalTime();

                    }
                    if (objWorkRequestAssignmentModel.WorkRequestProjectType == 279)
                    {
                        DateTime startDate = new DateTime(DateTime.Parse(objWorkRequestAssignmentModel.StartDate.Value.ToUniversalTime().ToString()).Year, DateTime.Parse(objWorkRequestAssignmentModel.StartDate.Value.ToUniversalTime().ToString()).Month, DateTime.Parse(objWorkRequestAssignmentModel.StartDate.Value.ToUniversalTime().ToString()).Day);
                        DateTime endDate = new DateTime(DateTime.Parse(objWorkRequestAssignmentModel.EndDate.Value.ToUniversalTime().ToString()).Year, DateTime.Parse(objWorkRequestAssignmentModel.EndDate.Value.ToUniversalTime().ToString()).Month, DateTime.Parse(objWorkRequestAssignmentModel.EndDate.Value.ToUniversalTime().ToString()).Day);
                        StringBuilder sb = new StringBuilder();

                        string dayName = "";
                        DayOfWeek dow;

                        var list1 = objWorkRequestAssignmentModel.WeekDayLst.Split(',').ToList();
                        foreach (var weekname in list1)
                        {
                            dayName = weekname;
                            Enum.TryParse(dayName, true, out dow);
                            for (DateTime runDate = startDate; runDate <= endDate; runDate = runDate.AddDays(1))
                            {
                                if (runDate.DayOfWeek == dow)
                                    sb.Append(runDate.ToUniversalTime().Date.ToShortDateString() + ',');
                            }
                        }
                        if (sb != null)
                        {
                            sb.Remove(sb.ToString().LastIndexOf(','), 1);
                        }
                        message = sb.ToString();
                    }
                    objWorkRequestAssignment.WeekDays = message;

                    objWorkRequestAssignment.WeekDaysName = objWorkRequestAssignmentModel.WeekDayLst;
                    objWorkRequestAssignmentRepository.Add(objWorkRequestAssignment);
                    objWorkRequestAssignment.WorkOrderCodeID = objWorkRequestAssignment.WorkRequestAssignmentID + 100;
                    objWorkRequestAssignmentRepository.SaveChanges();
                    AutoMapper.Mapper.CreateMap<WorkRequestAssignment, WorkRequestAssignmentModel>();

                    objWorkRequestAssignmentModel = AutoMapper.Mapper.Map(objWorkRequestAssignment, objWorkRequestAssignmentModel);
                    objWorkRequestAssignmentModel.Result = Result.Completed;

                    //if (objWorkRequestAssignmentModel.Result == Result.Completed && objWorkRequestAssignmentModel.WorkRequestProjectType == 256)
                    //{

                    //    objEmailReturn = new List<EmailToManagerModel>();
                    //    objEmailLogRepository = new EmailLogRepository();
                    //    objEmailReturn = objEmailLogRepository.SendEmailToEmployeeForFacilityRequest(objWorkRequestAssignmentModel.LocationID, objWorkRequestAssignmentModel.CreatedBy);
                    //    foreach (var item in objEmailReturn)
                    //    {
                    //        EmailHelper objEmailHelper = new EmailHelper();
                    //        message = PushNotificationMessages.NewFacilityRequest(objWorkRequestAssignmentModel.WorkOrderCode + objWorkRequestAssignmentModel.WorkOrderCodeID);
                    //        PushNotification.GCMAndroid(message, item.DeviceId, objWorkRequestAssignmentModel);
                    //    }
                    //} //Commented by Bhushan Dod for multiple time message sent to employee
                    return objWorkRequestAssignmentModel;
                }
                else
                {
                    if (objWorkRequestAssignmentModel.CrStartTime != null)
                    {
                        objWorkRequestAssignment.ConStartTime = Convert.ToDateTime(objWorkRequestAssignmentModel.CrStartTime).ToUniversalTime();
                    }
                    if (objWorkRequestAssignmentModel.StartDate != null && objWorkRequestAssignmentModel.EndDate != null)
                    {
                        DateTime startDate = new DateTime(DateTime.Parse(objWorkRequestAssignmentModel.StartDate.Value.ToUniversalTime().ToString()).Year, DateTime.Parse(objWorkRequestAssignmentModel.StartDate.Value.ToUniversalTime().ToString()).Month, DateTime.Parse(objWorkRequestAssignmentModel.StartDate.Value.ToUniversalTime().ToString()).Day);
                        DateTime endDate = new DateTime(DateTime.Parse(objWorkRequestAssignmentModel.EndDate.Value.ToUniversalTime().ToString()).Year, DateTime.Parse(objWorkRequestAssignmentModel.EndDate.Value.ToUniversalTime().ToString()).Month, DateTime.Parse(objWorkRequestAssignmentModel.EndDate.Value.ToUniversalTime().ToString()).Day);
                        StringBuilder sb = new StringBuilder();

                        string dayName = "";
                        DayOfWeek dow;

                        var list1 = objWorkRequestAssignmentModel.WeekDayLst.Split(',').ToList();
                        foreach (var weekname in list1)
                        {
                            dayName = weekname;
                            Enum.TryParse(dayName, true, out dow);
                            for (DateTime runDate = startDate; runDate <= endDate; runDate = runDate.AddDays(1))
                            {
                                if (runDate.DayOfWeek == dow)
                                    sb.Append(runDate.ToUniversalTime().Date.ToShortDateString() + ',');
                            }
                        }
                        if (sb != null)
                        {
                            sb.Remove(sb.ToString().LastIndexOf(','), 1);
                        }
                        message = sb.ToString();
                    }
                    objWorkRequestAssignment.WeekDays = message;
                    objWorkRequestAssignment.WeekDaysName = objWorkRequestAssignmentModel.WeekDayLst;
                    if (objWorkRequestAssignmentModel.CustomerName != null && objWorkRequestAssignmentModel.CustomerName != "")
                    {
                        objWorkRequestAssignmentModel.CustomerName = objWorkRequestAssignmentModel.CustomerName.ToTitleCase();
                    }
                    var workrequestData = objWorkRequestAssignmentRepository.GetAll(r => r.IsDeleted == false && r.WorkRequestAssignmentID == objWorkRequestAssignmentModel.WorkRequestAssignmentID).SingleOrDefault();
                    workrequestData.WorkRequestType = objWorkRequestAssignmentModel.WorkRequestType;
                    workrequestData.AssetID = objWorkRequestAssignmentModel.AssetID;
                    workrequestData.ProblemDesc = objWorkRequestAssignmentModel.ProblemDesc;
                    workrequestData.PriorityLevel = objWorkRequestAssignmentModel.PriorityLevel;
                    workrequestData.WorkRequestImage = objWorkRequestAssignmentModel.WorkRequestImage;
                    workrequestData.SafetyHazard = objWorkRequestAssignmentModel.SafetyHazard;
                    workrequestData.ProjectDesc = objWorkRequestAssignmentModel.ProjectDesc;
                    workrequestData.WorkRequestStatus = objWorkRequestAssignmentModel.WorkRequestStatus;
                    workrequestData.AssignToUserId = objWorkRequestAssignmentModel.AssignToUserId;
                    workrequestData.AssignByUserId = objWorkRequestAssignmentModel.AssignByUserId;
                    workrequestData.ModifiedBy = objWorkRequestAssignmentModel.ModifiedBy;
                    workrequestData.ModifiedDate = objWorkRequestAssignmentModel.ModifiedDate; // objWorkRequestAssignmentModel.ModifiedDate.Value.ToClientTimeZoneinDateTime();

                    workrequestData.CustomerName = objWorkRequestAssignmentModel.CustomerName;
                    workrequestData.CustomerContact = objWorkRequestAssignmentModel.CustomerContact;
                    workrequestData.VehicleColor = objWorkRequestAssignmentModel.VehicleColor;
                    workrequestData.VehicleMake = objWorkRequestAssignmentModel.VehicleMake;
                    workrequestData.VehicleModel = objWorkRequestAssignmentModel.VehicleModel;
                    workrequestData.DriverLicenseNo = objWorkRequestAssignmentModel.DriverLicenseNo;
                    workrequestData.VehicleYear = objWorkRequestAssignmentModel.VehicleYear;
                    workrequestData.CurrentLocation = objWorkRequestAssignmentModel.CurrentLocation;
                    workrequestData.Address = objWorkRequestAssignmentModel.Address;
                    workrequestData.StateId = objWorkRequestAssignmentModel.StateId;
                    workrequestData.ZipCode = objWorkRequestAssignmentModel.ZipCode;
                    workrequestData.City = objWorkRequestAssignmentModel.City;
                    workrequestData.LicensePlateNo = objWorkRequestAssignmentModel.LicensePlateNo;
                    if (objWorkRequestAssignmentModel.StartDate != null)
                    {
                        workrequestData.StartDate = objWorkRequestAssignmentModel.StartDate.Value.ToUniversalTime();
                    }
                    if (objWorkRequestAssignmentModel.EndDate != null)
                    {
                        workrequestData.EndDate = objWorkRequestAssignmentModel.EndDate.Value.ToUniversalTime();
                    }
                    if (objWorkRequestAssignmentModel.StartTime != null)
                    {
                        workrequestData.StartTime = objWorkRequestAssignmentModel.StartTime.Value.ToUniversalTime();
                    }
                    //workrequestData.StartDate = objWorkRequestAssignmentModel.StartDate.Value.ToUniversalTime();
                    //workrequestData.EndDate = objWorkRequestAssignmentModel.EndDate.Value.ToUniversalTime();
                    //workrequestData.StartTime = objWorkRequestAssignment.StartTime.Value.ToUniversalTime();
                    workrequestData.WeekDays = objWorkRequestAssignmentModel.WeekDays;
                    workrequestData.WeekDaysName = objWorkRequestAssignment.WeekDaysName;

                    if (!string.IsNullOrEmpty(objWorkRequestAssignmentModel.AssignedWorkOrderImage))
                    {
                        workrequestData.AssignedWorkOrderImage = objWorkRequestAssignmentModel.AssignedWorkOrderImage;
                    }
                    if (objWorkRequestAssignmentModel.AssignedTimeInterval != null)
                    {
                        workrequestData.AssignedTime = Convert.ToDateTime(objWorkRequestAssignmentModel.AssignedTimeInterval);
                        objWorkRequestAssignmentModel.AssignedTime = Convert.ToDateTime(objWorkRequestAssignmentModel.AssignedTimeInterval);
                    }
                    if (objWorkRequestAssignmentModel.WorkRequestProjectType != null)
                    {
                        workrequestData.WorkRequestProjectType = Convert.ToInt32(objWorkRequestAssignmentModel.WorkRequestProjectType, CultureInfo.InvariantCulture);
                    }
                    objWorkRequestAssignmentRepository.SaveChanges();
                    //objWorkRequestAssignmentModel = AutoMapper.Mapper.Map(objWorkRequestAssignment, objWorkRequestAssignmentModel);
                    objWorkRequestAssignmentModel.Result = Result.UpdatedSuccessfully;
                    objWorkRequestAssignmentModel.WorkRequestProjectType = workrequestData.WorkRequestProjectType;
                    objWorkRequestAssignmentModel.WorkOrderCode = workrequestData.WorkOrderCode;
                    objWorkRequestAssignmentModel.WorkOrderCodeID = workrequestData.WorkOrderCodeID;
                    return objWorkRequestAssignmentModel;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public WorkRequestAssignmentModel SaveWorkRequestAssignment(WorkRequestAssignmentModel objWorkRequestAssignmentModel)", "Exception While saving work order request.", objWorkRequestAssignmentModel);
                throw;
            }
        }

        public List<WorkRequestAssignmentModelList> GetAllWorkRequestAssignment(long? workRequestAssignmentId, long? requestedBy, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, long LocationID, long UserID, DateTime StartDate, DateTime EndDate, string filter, ObjectParameter totalRecords)
        {
            WorkRequestAssignmentRepository _WorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
            try
            {

                return _WorkRequestAssignmentRepository.GetAllWorkRequestAssignment(workRequestAssignmentId, requestedBy, operationName, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, LocationID, UserID, StartDate, EndDate, filter, totalRecords);
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<WorkRequestAssignmentModelList> GetAllWorkRequestAssignment(long? workRequestAssignmentId, long? requestedBy, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, long LocationID, long UserID, DateTime StartDate, DateTime EndDate, string filter, ObjectParameter totalRecords)", "fromC#", operationName);

                throw;
            }
        }

        public Result AssignedToWorkRequestAssignment(WorkRequestAssignmentModel workRequestAssignmentModel)
        {
            objWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
            try
            {
                if (workRequestAssignmentModel.WorkRequestAssignmentID > 0 && workRequestAssignmentModel.AssignToUserId > 0)
                {
                    var WorkRequestDetail = objWorkRequestAssignmentRepository.GetSingleOrDefault(t => t.WorkRequestAssignmentID == workRequestAssignmentModel.WorkRequestAssignmentID && t.IsDeleted == false);
                    WorkRequestDetail.ModifiedBy = workRequestAssignmentModel.ModifiedBy;
                    WorkRequestDetail.ModifiedDate = workRequestAssignmentModel.ModifiedDate; // workRequestAssignmentModel.ModifiedDate.Value.ToClientTimeZoneinDateTime();
                    WorkRequestDetail.IsDeleted = workRequestAssignmentModel.IsDeleted;
                    WorkRequestDetail.AssignToUserId = workRequestAssignmentModel.AssignToUserId;
                    WorkRequestDetail.AssignByUserId = workRequestAssignmentModel.AssignByUserId;
                    WorkRequestDetail.ProblemDesc = workRequestAssignmentModel.ProblemDesc;
                    WorkRequestDetail.ProjectDesc = workRequestAssignmentModel.ProjectDesc;
                    WorkRequestDetail.PriorityLevel = workRequestAssignmentModel.PriorityLevel;
                    if (workRequestAssignmentModel.AssignedTimeInterval != null)
                    {
                        WorkRequestDetail.AssignedTime = Convert.ToDateTime(workRequestAssignmentModel.AssignedTimeInterval);
                        workRequestAssignmentModel.AssignedTime = Convert.ToDateTime(workRequestAssignmentModel.AssignedTimeInterval);
                    }
                    //THis field specifically added by bhushan for while Work Order or special assign to employee send push to emp.
                    workRequestAssignmentModel.WorkOrderCodeForPush = WorkRequestDetail.WorkOrderCode + WorkRequestDetail.WorkOrderCodeID;
                    workRequestAssignmentModel.WorkRequestProjectType = WorkRequestDetail.WorkRequestProjectType;


                    objWorkRequestAssignmentRepository.SaveChanges();
                    return Result.Completed;
                }
                else
                    return Result.Failed;
                //}
                //  return Result.DuplicateRecord;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Created by vijay sahu on 15 may 2015
        /// This function is using for sending notification to all employee after creating facility request for customer.
        /// </summary>
        /// <returns></returns>
        public List<listForEmployeeDevice> sendNotificaitonToAllEmployee(long LocationId, long UserId)
        {
            using (workorderEMSEntities Context = new workorderEMSEntities())
            {
                try
                {
                    List<listForEmployeeDevice> a = (from pd in Context.PermissionDetails
                                                     join ur in Context.UserRegistrations
                                                            on pd.UserId equals ur.UserId
                                                     join elm in Context.EmployeeLocationMappings
                                                            on ur.UserId equals elm.EmployeeUserId

                                                     where ur.UserType == 3 // employee type
                                                     && ur.IsDeleted == false
                                                     && elm.LocationId == LocationId
                                                     && (pd.PermissionId == 4 || pd.PermissionId == 190) //very nice
                                                     && pd.LocationId == LocationId
                                                     select new listForEmployeeDevice
                                                     {
                                                         PermissionDetailId = pd.PermissionDetailId,
                                                         DeviceId = ur.DeviceId,
                                                         UserId = ur.UserId
                                                     }).Distinct().OrderBy(x => x.PermissionDetailId).ToList();
                    return a;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Created by Bhushan Dod on 30 June 2015
        /// This function is using for sending notification to all Manager after creating facility request not accepted by any mobile
        /// </summary>
        /// <returns></returns>
        public List<listForEmployeeDevice> send30SecFRNotificaitonToAllManager(long LocationId, long UserId)
        {

            using (workorderEMSEntities Context = new workorderEMSEntities())
            {
                try
                {
                    List<listForEmployeeDevice> a = (from pd in Context.PermissionDetails
                                                     join ur in Context.UserRegistrations
                                                            on pd.UserId equals ur.UserId
                                                     join elm in Context.ManagerLocationMappings
                                                            on ur.UserId equals elm.ManagerUserId
                                                     join lm in Context.LocationMasters
                                                            on elm.LocationId equals lm.LocationId

                                                     where ur.UserType == 2 // employee type
                                                     && ur.IsDeleted == false
                                                     && elm.LocationId == LocationId
                                                     && (pd.PermissionId == 4 || pd.PermissionId == 190) //very nice
                                                     && pd.LocationId == LocationId
                                                     select new listForEmployeeDevice
                                                     {
                                                         PermissionDetailId = pd.PermissionDetailId,
                                                         DeviceId = ur.DeviceId,
                                                         UserId = ur.UserId,
                                                         LocationName = lm.LocationName
                                                     }).Distinct().OrderBy(x => x.PermissionDetailId).ToList();
                    return a;
                }
                catch (Exception)
                {
                    return null;
                }
            }


        }

        /// <summary>
        /// Rendering data for HTML Form for eMaintenance
        /// </summary>
        /// <returns></returns>
        public WorkOrderEMS.Models.eMaintenance_M.WorkRequestAssignment_M GetDataForRendringHTML(long WorkRequestAssignmentRequestId)
        {
            WorkOrderEMS.Models.eMaintenance_M.WorkRequestAssignment_M objData = new WorkOrderEMS.Models.eMaintenance_M.WorkRequestAssignment_M();
            using (workorderEMSEntities Context = new workorderEMSEntities())
            {

                //objData = (from o in Context.WorkRequestAssignments
                //           where o.WorkRequestAssignmentID == WorkRequestAssignmentRequestId
                //           select o).FirstOrDefault();
                objData = (from o in Context.WorkRequestAssignments
                           join g in Context.GlobalCodes on o.FacilityRequestId equals g.GlobalCodeId
                           join ur in Context.UserRegistrations on o.AssignToUserId equals ur.UserId
                           where o.WorkRequestAssignmentID == WorkRequestAssignmentRequestId && ur.IsDeleted == false
                           select new WorkOrderEMS.Models.eMaintenance_M.WorkRequestAssignment_M
                           {
                               CustomerName = o.CustomerName,
                               LocationID = o.LocationID,
                               VehicleColor = o.VehicleColor,
                               VehicleMake = o.VehicleMake,
                               VehicleModel = o.VehicleModel,
                               VehicleYear = o.VehicleYear,
                               WorkRequestAssignmentID = o.WorkRequestAssignmentID,
                               Address = o.Address,
                               DriverLicenseNo = o.DriverLicenseNo,
                               CustomerContact = o.CustomerContact,
                               CurrentLocation = o.CurrentLocation,
                               LicensePlateNo = o.LicensePlateNo,
                               FacilityRequestId = o.FacilityRequestId,
                               FacilityRequestName = g.CodeName,
                               AssignedFirstName = ur.FirstName,
                               AssignedLastName = ur.LastName,
                               StartTime = o.StartTime,
                               EndTime = o.EndTime

                           }
                           ).FirstOrDefault();


            }

            return objData;
        }

        /// <summary>
        /// Created by vijay sahu on 19 april 2015
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SubmitSurveyForm(WorkOrderEMS.Models.eMaintenance_M.eMaintenanceSurvey_M obj)
        {

            try
            {
                EMaintenanceSurvey objEn = null;
                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    // Question 1
                    objEn = new EMaintenanceSurvey();
                    objEn.Answer = Convert.ToInt32(obj.ans1);
                    objEn.CreatedDate = DateTime.UtcNow;
                    objEn.IfNoThenDescription = "";
                    objEn.GlobalCodeId = 273;
                    objEn.SubmittedBy = obj.UserId;
                    objEn.WorkOrderAssignmentId = obj.WorkAssignmentId;
                    Context.EMaintenanceSurveys.Add(objEn);

                    // Question 2
                    objEn = new EMaintenanceSurvey();
                    objEn.Answer = Convert.ToInt32(obj.ans2);
                    objEn.CreatedDate = DateTime.UtcNow;
                    objEn.IfNoThenDescription = obj.Ques2Comment;
                    objEn.GlobalCodeId = 274;
                    objEn.SubmittedBy = obj.UserId;
                    objEn.WorkOrderAssignmentId = obj.WorkAssignmentId;
                    Context.EMaintenanceSurveys.Add(objEn);

                    // Question 3
                    objEn = new EMaintenanceSurvey();
                    objEn.Answer = Convert.ToInt32(obj.ans3);
                    objEn.CreatedDate = DateTime.UtcNow;
                    objEn.IfNoThenDescription = obj.Ques3Comment;
                    objEn.GlobalCodeId = 275;
                    objEn.SubmittedBy = obj.UserId;
                    objEn.WorkOrderAssignmentId = obj.WorkAssignmentId;
                    Context.EMaintenanceSurveys.Add(objEn);

                    // Question 4
                    objEn = new EMaintenanceSurvey();
                    objEn.Answer = Convert.ToInt32(obj.ans4);
                    objEn.CreatedDate = DateTime.UtcNow;
                    objEn.IfNoThenDescription = obj.Ques4Comment;
                    objEn.GlobalCodeId = 276;
                    objEn.SubmittedBy = obj.UserId;
                    objEn.WorkOrderAssignmentId = obj.WorkAssignmentId;
                    Context.EMaintenanceSurveys.Add(objEn);

                    // Question 5
                    objEn = new EMaintenanceSurvey();
                    objEn.Answer = Convert.ToInt32(obj.ans5);
                    objEn.CreatedDate = DateTime.UtcNow;
                    objEn.IfNoThenDescription = obj.Ques5Comment;
                    objEn.GlobalCodeId = 277;
                    objEn.SubmittedBy = obj.UserId;
                    objEn.WorkOrderAssignmentId = obj.WorkAssignmentId;
                    Context.EMaintenanceSurveys.Add(objEn);

                    // Question 6
                    objEn = new EMaintenanceSurvey();
                    objEn.Answer = Convert.ToInt32(obj.ans6);
                    objEn.CreatedDate = DateTime.UtcNow;
                    objEn.IfNoThenDescription = "";
                    objEn.GlobalCodeId = 278;
                    objEn.SubmittedBy = obj.UserId;
                    objEn.WorkOrderAssignmentId = obj.WorkAssignmentId;
                    Context.EMaintenanceSurveys.Add(objEn);
                    Context.SaveChanges();
                }
                return "Survey is successfully done, Thanks for your valuable feedback.";
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public string SubmitSurveyForm(WorkOrderEMS.Models.eMaintenance_M.eMaintenanceSurvey_M obj)", "fromC#", obj);
                return "Please try after some times";
            }
        }
        #endregion

        /// <summary>ManagerLocationApproval
        ///  <CreatedBY>Roshan Rahood</CreatedBY>
        ///  <CreatedOn>Dec-22-2014</CreatedOn>
        ///  <CreatedFor>Verification for the location by manager</CreatedFor>
        /// </summary>
        /// <param name="verificationManagerId"></param>
        /// <returns></returns>
        public bool ManagerLocationApproval(long verificationManagerId, long locationId)
        {
            bool _status = false;
            try
            {
                ObjManagerLocationMappingRepository = new ManagerLocationMappingRepository();
                ManagerLocationMapping ObjMappingManager = ObjManagerLocationMappingRepository.GetSingleOrDefault(map => map.ManagerLocationMappingId == verificationManagerId && map.IsDeleted == false);
                if (ObjMappingManager != null && ObjMappingManager.ManagerLocationMappingId > 0)
                {


                    ObjLocationRepository = new LocationRepository();
                    LocationMaster ObjLocationManager = objLocationRepository.GetSingleOrDefault(x => x.LocationId == locationId && x.IsDeleted == false && x.IsVerifiedByManager == false);
                    if (ObjLocationManager != null && ObjLocationManager.LocationId > 0)
                    {
                        _status = true;
                        ObjLocationManager.IsVerifiedByManager = true;
                        objLocationRepository.Update(ObjLocationManager);
                    }
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "ManagerLocationApproval(long verificationManagerId, long locationId)", "Exception While creating location", verificationManagerId);

                throw ex;
            }
            return _status;
        }

        /// <summary>ClientLocationApproval
        ///  <CreatedBY>Roshan Rahood</CreatedBY>
        ///  <CreatedOn>Dec-22-2014</CreatedOn>
        ///  <CreatedFor>Verification for the location by Client</CreatedFor>
        /// </summary>
        /// <param name="verificationClientId"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public bool ClientLocationApproval(long verificationClientId, long locationId)
        {
            bool _status = false;
            try
            {
                ObjLocationClientMappingRepository = new LocationClientMappingRepository();
                LocationClientMapping ObjMappingClient = ObjLocationClientMappingRepository.GetSingleOrDefault(map => map.ClientUserId == verificationClientId && map.IsDeleted == false);
                if (ObjMappingClient != null && ObjMappingClient.LocationClientMappingID > 0)
                {
                    ObjLocationRepository = new LocationRepository();
                    LocationMaster ObjLocationClient = objLocationRepository.GetSingleOrDefault(x => x.LocationId == locationId && x.IsDeleted == false && x.IsVerifiedByClient == false);
                    if (ObjLocationClient != null && ObjLocationClient.LocationId > 0)
                    {
                        _status = true;
                        ObjLocationClient.IsVerifiedByClient = true;
                        objLocationRepository.Update(ObjLocationClient);
                    }
                }

            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "ClientLocationApproval(long verificationClientId, long locationId)", "Exception While creating location", verificationClientId);


                throw ex;
            }
            return _status;

        }

        /// <summary>
        /// To Get the All Locations For SuperAdmin
        /// </summary>
        /// <returns></returns>
        public List<MasterLocationModel> GetSuperAdminUserLocation()
        {
            LocationMasterRepository obj_LocationMasterRepository = new LocationMasterRepository();
            return obj_LocationMasterRepository.GetAll(x => x.IsDeleted == false).Select(x => new MasterLocationModel()
            {
                LocationName = x.LocationName,
                LocationId = x.LocationId,
            }).ToList();
        }

        public long GetTotalManagerCount(string LoginUserType, long LocationID, long userID)
        {
            GlobalAdminRepository obj_GlobalAdminRepository = new GlobalAdminRepository();
            return obj_GlobalAdminRepository.GetTotalCountOfUsers(LoginUserType, LocationID, userID).Count();

        }
        //public void GetUnAssignedWorkOrder(long locationId) {
        //    WorkRequestRepository obj_WorkRequestRepository = new WorkRequestRepository();
        //    var a = obj_WorkRequestRepository.GetAll(x => x.IsDeleted == false && locationId != 0 ? x.LocationID == locationId : x.LocationID == x.LocationID).Select(x=>new WorkRequestModel(){
        //    })

        //}
        /// <summary>
        /// GET THE GLOBAL USER IN THE APPPLIACTAIONS
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>2015-03-16</CreatedDate>
        /// <returns></returns>
        public List<GlobalUserModel> GetApplicationGlobalAdmin()
        {
            try
            {
                GlobalAdminRepository obj_GlobalAdminRepository = new GlobalAdminRepository();
                return obj_GlobalAdminRepository.GetApplicationGlobalAdmin();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// created by vijay sahu on 24 march 2015
        /// </summary>
        /// <param name="locationName"></param>
        /// <returns></returns>
        public byte isEmailExists(string Email, string AlternateEmail)
        {



            byte result = 0;
            try
            {


                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    long re = 0;

                    if (Email != "")
                    {
                        re = (from o in Context.UserRegistrations
                              where o.UserEmail == Email.Trim()
                                     && o.IsDeleted == false
                              select o.UserId).FirstOrDefault();

                    }
                    else if (AlternateEmail != "")
                    {
                        re = (from o in Context.UserRegistrations
                              where o.AlternateEmail == AlternateEmail.Trim()
                                     && o.IsDeleted == false
                              select o.UserId).FirstOrDefault();

                    }

                    if (re > 0)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result = 0;

            }
            return result;
        }

        /// <summary>
        /// To Get Location Detail
        /// </summary>
        /// <param name="LocationID"></param>
        /// <returns></returns>
        public List<LocationDetailModel> LocationDetailByLocationID(long LocationID)
        {
            LocationRepository obj_LocationRepository = new LocationRepository();
            return obj_LocationRepository.LocationDetailByLocationID(LocationID);
        }
        /// <summary>
        /// Created by vijay sahu /// 3 april 2015
        /// this method is used for binding new generated workOrder Request push notification on the screen.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic WorkOrderDetailsForPushNotificaiton(string id, long userId, long userType)
        {
            dynamic abc = null;
            LoginManager objLoginManager = new LoginManager();
            try
            {
                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    List<long> objL = new List<long>();
                    if (userType == 1 || userType == 5)
                    {
                        var list = objLoginManager.GetGlobalandITAdminLocationList(userId);
                        foreach (var t in list)
                        {
                            objL.Add(t.LocationId);
                        }
                    }
                    if (userType == 6)
                    {
                        var list = objLoginManager.GetAdminUserLocationList(userId);
                        foreach (var t in list)
                        {
                            objL.Add(t.LocationId);
                        }
                    }

                    if (userType == 2)
                    {
                        var list = objLoginManager.GetManageAdminUserLocationList(userId);
                        foreach (var t in list)
                        {
                            objL.Add(t.LocationId);
                        }
                    }
                    abc = (from o in Context.WorkRequestAssignments
                           .Where(x => objL.Contains(x.LocationID))
                           orderby o.WorkRequestAssignmentID descending
                           select new
                           {
                               o.LocationID,
                               o.WorkRequestAssignmentID,
                               o.WorkOrderCode,
                               o.WorkOrderCodeID,
                               o.CreatedBy,
                               o.WorkRequestProjectType
                           }).FirstOrDefault();
                    if (abc.CreatedBy == userId) { abc = null; }
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public string WorkOrderDetailsForPushNotificaiton(string id)", "Exception while fetching latest record for workOrder ", "id-" + id + "userId-" + userId + "userType-" + userType);
            }
            return abc;
        }

        /// <summary>
        /// Created by  vijay sahu on 26 march 2015 worldCup
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public byte isEmployeeIdExists(string empId)
        {
            string n = "";
            try
            {
                using (workorderEMSEntities Context = new workorderEMSEntities())
                {


                    n = (from o in Context.UserRegistrations
                         where o.EmployeeID == empId
                         && o.IsDeleted != true
                         select o.UserEmail).FirstOrDefault();
                }


                if (n == "" || n == null)
                {
                    return 0;
                }
                else
                {
                    return 1;

                }
            }
            catch (Exception)
            {
                return 1;
            }
        }

        /// <summary>      
        /// Created By Bhushan Dod on 30/06/2015
        /// Description :- Convert function for single record bcoz CR assign to particular emp 
        /// </summary>
        /// <returns></returns>
        public listForEmployeeDevice sendNotificationContinuousRequestToEmployee(long LocationId, long? UserId, WorkRequestAssignmentModel obj)
        {
            using (workorderEMSEntities Context = new workorderEMSEntities())
            {
                try
                {
                    EmailHelper objEmailHelper = new EmailHelper();
                    objEmailLogRepository = new EmailLogRepository();

                    listForEmployeeDevice lst = (from ur in Context.UserRegistrations
                                                 join elm in Context.EmployeeLocationMappings
                                                        on ur.UserId equals elm.EmployeeUserId
                                                 join lm in Context.LocationMasters on LocationId equals lm.LocationId

                                                 where ur.UserType == 3 // employee type
                                                 && ur.IsDeleted == false
                                                 && ur.UserId == UserId
                                                 select new listForEmployeeDevice
                                                 {
                                                     DeviceId = ur.DeviceId,
                                                     UserId = ur.UserId,
                                                     UserEmail = ur.UserEmail,
                                                     LocationID = elm.LocationId,
                                                     LocationName = lm.LocationName,
                                                     UserName = !string.IsNullOrEmpty(ur.LastName) ? ur.FirstName + " " + ur.LastName : ur.FirstName,

                                                 }).FirstOrDefault();

                    if (obj.WorkRequestProjectType == 279)
                    {
                        objEmailHelper.MailType = "CONTINIOUSREQUEST";
                        objEmailHelper.WorkRequestAssignmentID = obj.WorkRequestAssignmentID;
                        objEmailHelper.WorkOrderCodeId = obj.WorkOrderCodeForPush;
                        //objEmailHelper.StartDate = obj.StartDate.ToString("MM'/'dd'/'yyyy");
                        if (obj.StartDate != null)
                            objEmailHelper.StartDate = obj.StartDate.ToString("MM'/'dd'/'yyyy");
                            //objEmailHelper.StartDate = obj.StartDate.Value.ToClientTimeZoneinDateTime().ToString("MM'/'dd'/'yyyy");                            
                            if (obj.EndDate != null)
                            objEmailHelper.EndDate = obj.EndDate.ToString("MM'/'dd'/'yyyy");
                           // objEmailHelper.EndDate = obj.EndDate.Value.ToClientTimeZoneinDateTime().ToString("MM'/'dd'/'yyyy");
                        objEmailHelper.WeekDays = obj.WeekDayLst;
                        //objEmailHelper.StartTime = obj.StartTime.ToString("hh:mm tt");
                        if (obj.CrStartTime != null)
                            //objEmailHelper.StartTime = obj.StartTime.Value.ToClientTimeZoneinDateTime().ToString("hh:mm tt");
                            //objEmailHelper.StartTime = obj.CrStartTime.ToString("hh:mm tt"); Convert.ToDateTime(obj.CrStartTime)
                            objEmailHelper.StartTime = Convert.ToDateTime(obj.CrStartTime).ToString("hh:mm tt");
                        objEmailHelper.AssignedTime = obj.AssignedTime.ToString("HH:mm") + " (hh:mm)";
                        objEmailHelper.ProjectDesc = obj.ProjectDesc;
                        objEmailHelper.UserName = lst.UserName;
                        objEmailHelper.emailid = lst.UserEmail;
                        objEmailHelper.LocationName = lst.LocationName;

                        bool IsSent = objEmailHelper.SendEmailWithTemplate();
                        if (lst.DeviceId != null)
                        {
                            PushNotification.GCMAndroid("Continuous Request", lst.DeviceId, objEmailHelper);
                        }
                        if (IsSent == true)
                        {
                            EmailLog objEmailog = new EmailLog();
                            try
                            {
                                objEmailog.CreatedBy = lst.UserId;
                                objEmailog.CreatedDate = DateTime.UtcNow;
                                objEmailog.DeletedBy = null;
                                objEmailog.DeletedOn = null;
                                objEmailog.LocationId = lst.LocationID;
                                objEmailog.ModifiedBy = null;
                                objEmailog.ModifiedOn = null;
                                objEmailog.SentBy = obj.CreatedBy;
                                objEmailog.SentEmail = lst.UserEmail;
                                objEmailog.Subject = objEmailHelper.Subject;
                                objEmailog.SentTo = lst.UserId;

                                objEmailLogRepository.SaveEmailLog(objEmailog);
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                    }
                    if (obj.WorkRequestProjectType == 128)
                    {
                        objEmailHelper.MailType = "WORKORDERREQUESTTOEMPLOYEE";
                        objEmailHelper.WorkRequestAssignmentID = obj.WorkRequestAssignmentID;
                        objEmailHelper.WorkOrderCodeId = obj.WorkOrderCodeForPush;
                        objEmailHelper.AssignedTime = obj.AssignedTime.ToString("HH:mm");
                        objEmailHelper.ProjectDesc = obj.ProblemDesc;
                        objEmailHelper.UserName = lst.UserName;
                        objEmailHelper.LocationName = lst.LocationName;

                        if (lst.DeviceId != null)
                        {
                            PushNotification.GCMAndroid("New work order " + obj.WorkOrderCode + " assigned to you at " + lst.LocationName, lst.DeviceId, objEmailHelper);
                        }
                    }
                    if (obj.WorkRequestProjectType == 129)
                    {
                        objEmailHelper.MailType = "SPECIALWORKORDERTOEMPLOYEE";
                        objEmailHelper.WorkRequestAssignmentID = obj.WorkRequestAssignmentID;
                        objEmailHelper.WorkOrderCodeId = obj.WorkOrderCodeForPush;
                        objEmailHelper.AssignedTime = obj.AssignedTime.ToString("HH:mm");
                        objEmailHelper.ProjectDesc = obj.ProjectDesc;
                        objEmailHelper.UserName = lst.UserName;
                        objEmailHelper.LocationName = lst.LocationName;

                        if (lst.DeviceId != null)
                        {
                            PushNotification.GCMAndroid("Special work order " + obj.WorkOrderCode + " assigned to you at " + lst.LocationName, lst.DeviceId, objEmailHelper);
                        }
                    }

                    return lst;
                }
                catch (Exception ex)
                {
                    WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "listForEmployeeDevice sendNotificationContinuousRequestToEmployee(long LocationId, long? UserId, WorkRequestAssignmentModel obj)", "from c# while login GlobalAdminmanager.cs", obj);
                    throw;
                }
            }
        }

        /// <summary>
        /// Created by vijay sahu on 2 may 2015
        /// check weather location code exists or not.
        /// </summary>
        /// <param name="locationCode"></param>
        /// <returns></returns>
        public byte isLocationCodeExists(string locationCode)
        {
            string n = "";
            try
            {
                using (workorderEMSEntities Context = new workorderEMSEntities())
                {


                    n = (from o in Context.LocationMasters
                         where o.Address2 == locationCode
                         && o.IsDeleted != true
                         select o.Address2).FirstOrDefault();
                }


                if (n == "" || n == null)
                {
                    return 0;
                }
                else
                {
                    return 1;

                }
            }
            catch (Exception)
            {
                return 1;
            }
        }

        /// <summary>
        /// Created By : Bhushan Dod
        /// Created Date : 07/17/2015
        /// Description : Push notification to the manager if Checked out equipment is not returned within 24 hours.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool EquipmentCheckOutStatus(long LocId, long userId, long userType)
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

                    if (userType == 2)
                    {
                        var list = objLoginManager.GetManageAdminUserLocationList(userId);
                        foreach (var t in list)
                        { objL.Add(t.LocationId); }
                    }
                    EmailHelper objEmailHelper = new EmailHelper();
                    DateTime time = DateTime.UtcNow;

                    var QrcDetails = Context.QRCMasters.Where(x => x.CheckOutStatus == true && objL.Contains(x.LocationId.Value)
                        //&& (x.LocationId == LocId)
                        //&& (time.Date == x.ModifiedDate.Value.AddDays(1).Date)
                        //  && (time.Date.TimeOfDay.Minutes == x.u.ModifiedDate.Value.TimeOfDay)
                        && x.QRCTYPE == 46 && x.ModifiedDate != null).ToList();//x.ModifiedDate != null apply due to checkout constraint on modified date when QRCSave not save modified by
                    if (QrcDetails.Count > 0)
                    {
                        objEmailReturn = objEmailLogRepository.SendEmailToManagerForItemMissingQRC(LocId, userId);
                        if (objEmailReturn.Count > 0)
                        {
                            foreach (var item in objEmailReturn)
                            {
                                foreach (var d in QrcDetails)
                                {
                                    objEmailHelper.MailType = "EQUIPMENTCHECKOUT";
                                    objEmailHelper.QrCodeId = d.QRCodeID;
                                    objEmailHelper.ManagerName = item.ManagerName;
                                    objEmailHelper.UserName = item.UserName;
                                    objEmailHelper.emailid = item.ManagerEmail;
                                    objEmailHelper.LocationName = item.LocationName;
                                    if (time.Date == d.ModifiedDate.Value.AddDays(1).Date && d.ModifiedDate.Value.ToShortTimeString() == time.ToShortTimeString())//d.ModifiedDate.Value.AddMinutes(-1).TimeOfDay == time.TimeOfDay)
                                    {
                                        if (item.DeviceId != null)
                                        {
                                            PushNotification.GCMAndroid("QRC Equipment " + d.QRCodeID + " does not check in by " + item.UserName + " from last 24hr at location" + item.LocationName, item.DeviceId, objEmailHelper);
                                        }
                                        result = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "bool EquipmentCheckOutStatus(long LocId, long userId, long userType)", "Exception while EquipmentCheckOutStatus ", LocId);
            }
            return result;
        }

        /// <summary>
        /// Created by vijay sahu /// 3 april 2015
        /// this method is used for binding new generated workOrder Request push notification on the screen.
        /// Modified By : Bhushan Dod for Work ORder shown according to location Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic WorkOrderDetailsForPushNotificaitonSignal(string id, long userId, long userType, long locID)
        {
            dynamic abc = null;
            LoginManager objLoginManager = new LoginManager();
            try
            {
                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    List<long> objL = new List<long>();
                    if (userType == 1 || userType == 5)
                    {
                        var list = objLoginManager.GetGlobalandITAdminLocationList(userId);
                        foreach (var t in list)
                        {
                            objL.Add(t.LocationId);
                        }
                    }
                    if (userType == 6)
                    {
                        var list = objLoginManager.GetAdminUserLocationList(userId);
                        foreach (var t in list)
                        {
                            objL.Add(t.LocationId);
                        }
                    }

                    if (userType == 2)
                    {
                        var list = objLoginManager.GetManageAdminUserLocationList(userId);
                        foreach (var t in list)
                        {
                            objL.Add(t.LocationId);
                        }
                    }
                    abc = objL.Contains(locID);
                    //Commented By Bhushan Do for SignalR 
                    //abc = (from o in Context.WorkRequestAssignments
                    //       .Where(x => objL.Contains(x.LocationID))
                    //       orderby o.WorkRequestAssignmentID descending
                    //       select new
                    //       {
                    //           o.LocationID,
                    //           o.WorkRequestAssignmentID,
                    //           o.WorkOrderCode,
                    //           o.WorkOrderCodeID,
                    //           o.CreatedBy,
                    //           o.WorkRequestProjectType
                    //       }).FirstOrDefault();
                    //if (abc.CreatedBy == userId) { abc = null; }
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public string WorkOrderDetailsForPushNotificaiton(string id)", "Exception while fetching latest record for workOrder ", "id-" + id + "userId-" + userId + "userType-" + userType);
            }
            return abc;
        }

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created On: 13/04/2016
        /// Retrieve all data -- server method called by client Javascript to send data from server to client
        /// </summary>
        public string GetDashboardHeadCount(long locationId, long UserId, DateTime? fromDate, DateTime? toDate, long usertype)
        {
            try
            {
                HubManager objHubManager = new HubManager();
                var data = objHubManager.GetDataFromQuery("sp_GetWebDashboardDetails", UserId, locationId, fromDate, toDate, usertype);
                return data;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "   public string GetDashboardHeadCount(long locationId,long UserId, DateTime? fromDate, DateTime? toDate)", "Exception while fetching latest record for workOrder ", "id-" + locationId + "userId-" + UserId);
                return null;
            }
        }

        /// <summary>
        /// Created By: Bhushan Dod
        /// Created On: 26/04/2016
        /// Retrieve all data -- server method called by client Javascript to send data from server to client
        /// </summary>
        public string GetWorkOrderforDashboard(long locationId, long UserId, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                HubManager objHubManager = new HubManager();
                var data = objHubManager.GetDataFromQueryForWO("sp_GetWorkOrderForDashboardDetails", UserId, locationId, fromDate, toDate);
                return data;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "   public string GetWorkOrderforDashboard(long locationId,long UserId, DateTime? fromDate, DateTime? toDate)", "Exception while fetching latest record for workOrder ", "id-" + locationId + "userId-" + UserId);
                return null;
            }
        }

        /// Get details of Employee scan qrc log
        /// </summary>
        /// <param name="LocationId,qrcName,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        /// 

        public List<ReportChart> QrcScannedDetails(long? LocationId, long qrcName, long userType, long userId, DateTime? fromDate, DateTime? toDate)
        {
            List<ReportChart> lstRoutine = new List<ReportChart>();
            workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();
            if (LocationId.Value > 0)
            {
                lstRoutine = _workorderEMSEntities.QRCScanLogs.Join(_workorderEMSEntities.QRCMasters, q => q.QRCID, u => u.QRCID, (q, u) => new { q, u }).
                        Where(x => ((LocationId == 0 ? null : LocationId) == null || x.q.LocationId == LocationId)
                                                   && x.q.QrcType == qrcName
                                                   && (x.q.CreatedOn >= fromDate && x.q.CreatedOn <= toDate)
                                                   && x.u.ClientTypeID == null
                                                   ).Select(x => new ReportChart()
                                                   {
                                                       ScanUserId = x.q.ScanUserId,
                                                       ScanUserName = x.q.UserRegistration.FirstName + " " + x.q.UserRegistration.LastName,
                                                       CreatedDate = x.q.CreatedOn,
                                                       QrcName = x.u.QRCName,
                                                       LocationName = x.q.LocationMaster.LocationName,
                                                       QrCodeId = x.u.QRCodeID
                                                   }).ToList<ReportChart>();
                var tmp = lstRoutine.Select(x => new ReportChart()
                {
                    ScanUserId = x.ScanUserId,
                    ScanUserName = x.ScanUserName,
                    StrCreatedDate = x.CreatedDate.ToClientTimeZone(true),
                    QrcName = x.QrcName,
                    LocationName = x.LocationName,
                    QrCodeId = x.QrCodeId
                }).ToList();
                return tmp;
            }
            else
            {
                LoginManager objLoginManager = new LoginManager();
                List<long> objL = new List<long>();
                if (userType == 1 || userType == 5)
                {
                    var list = objLoginManager.GetGlobalandITAdminLocationList(userId);
                    foreach (var t in list)
                    {
                        objL.Add(t.LocationId);
                    }
                }
                if (userType == 6)
                {
                    var list = objLoginManager.GetAdminUserLocationList(userId);
                    foreach (var t in list)
                    {
                        objL.Add(t.LocationId);
                    }
                }
                if (userType == 2)
                {
                    var list = objLoginManager.GetManageAdminUserLocationList(userId);
                    foreach (var t in list)
                    {
                        objL.Add(t.LocationId);
                    }
                }
                lstRoutine = _workorderEMSEntities.QRCScanLogs.Join(_workorderEMSEntities.QRCMasters, q => q.QRCID, u => u.QRCID, (q, u) => new { q, u }).
                       Where(x => objL.Contains(x.q.LocationId)
                                                 && x.q.QrcType == qrcName
                                                  && (x.q.CreatedOn >= fromDate && x.q.CreatedOn <= toDate)
                                                  && x.u.ClientTypeID == null
                                                  ).Select(x => new ReportChart()
                                                  {
                                                      ScanUserId = x.q.ScanUserId,
                                                      ScanUserName = x.q.UserRegistration.FirstName + " " + x.q.UserRegistration.LastName,
                                                      CreatedDate = x.q.CreatedOn,
                                                      QrcName = x.u.QRCName,
                                                      LocationName = x.q.LocationMaster.LocationName,
                                                      QrCodeId = x.u.QRCodeID
                                                  }).ToList<ReportChart>();
                var tmp = lstRoutine.Select(x => new ReportChart()
                {
                    ScanUserId = x.ScanUserId,
                    ScanUserName = x.ScanUserName,
                    StrCreatedDate = x.CreatedDate.ToClientTimeZone(true),
                    QrcName = x.QrcName,
                    LocationName = x.LocationName,
                    QrCodeId = x.QrCodeId
                }).ToList();
                return tmp;
            }
        }

        /// <summary>
        /// Created By : Bhushan Dod
        /// Created On : 06/07/2016
        /// To Save widget by default according to location service when location created
        /// </summary>
        /// <param name="locationId"></param>
        /// <param name="servicesId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool SaveByDefaultWidgetSetting(long locationId, string servicesId, long? UserId)
        {
            LocationService _LocationService = null;
            DashboardWidgetSettingManager objDashboardWidgetSettingManager = new DashboardWidgetSettingManager();
            bool statusflag = false;
            StringBuilder WidgetIds = new StringBuilder();
            try
            {
                string[] aServicesID = servicesId.Split(',');
                foreach (var serviceItem in aServicesID)
                {
                    if (serviceItem != null && !string.IsNullOrEmpty(serviceItem) && Convert.ToInt64(serviceItem, CultureInfo.InvariantCulture) > 0)
                    {
                        switch (Convert.ToInt64(serviceItem))
                        {
                            case 2://Manage Users
                                {
                                    WidgetIds.Append((long)DashboardWidget.ActiveUsers + ",");
                                    WidgetIds.Append((long)DashboardWidget.UserCount + ",");
                                    break;
                                }
                            case 3://eScanner
                                {
                                    WidgetIds.Append((long)DashboardWidget.QRCCount + ",");
                                    WidgetIds.Append((long)DashboardWidget.QRCScanned + ",");
                                    break;
                                }
                            case 4://eMaintenance
                                {
                                    WidgetIds.Append((long)DashboardWidget.ProjectProgress + ",");
                                    WidgetIds.Append((long)DashboardWidget.UnAssignedWorkOrder + ",");
                                    WidgetIds.Append((long)DashboardWidget.WorkOrderCount + ",");
                                    WidgetIds.Append((long)DashboardWidget.WorkOrderUser + ",");
                                    break;
                                }
                            case 5://GT Tracker
                                {
                                    //WidgetIds.Append((long)DashboardWidget.eCashInfraction + ",");
                                    //WidgetIds.Append((long)DashboardWidget.VendorsVehicleCount + ",");
                                    break;
                                }
                            case 6://Inventory Tracker
                                {
                                    break;
                                }
                            case 7://Rules
                                {
                                    break;
                                }
                            case 9://Daily Activity Reports
                                {
                                    WidgetIds.Append((long)DashboardWidget.DailyActivityReport + ",");
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                }

                objDashboardWidgetSettingManager.UpdateDashboardWidgets(UserId.Value, locationId, WidgetIds.ToString());
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "SaveByDefaultWidgetSetting", "Exception While creating location and widget setting", locationId);
                throw ex;
            }




            return statusflag;
        }

        /// <summary>
        /// Created By: Bhushan Dod
        /// Create Date: 05-Oct-2016
        /// This method is used to fetch the unverified user list according to location.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="locationId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="searchText"></param>
        /// <param name="myUserType"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public List<UserModelList> GetAllUnVerifiedUserList(long? userId, long locationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string searchText, string myUserType, out long totalRecords)
        {
            ObjUserRepository = new UserRepository();
            try
            {

                return ObjUserRepository.GetUnVerifiedUsers(userId, locationId, myUserType, pageIndex, numberOfRows, sortColumnName, sortOrderBy, searchText, out totalRecords);
            }
            catch (Exception)
            {
                throw;
            }

        }

 
    }
    public class loc
    {
        public string LocationId { get; set; }
    }
}
