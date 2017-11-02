using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;

namespace WorkOrderEMS.BusinessLogic
{
    /// <summary>
    /// Interface for CommonManager
    /// </summary>
    /// <Createdby>Nagendra Upwanshi</Createdby>
    /// <CreatedDate>Aug-22-2014</CreatedDate>
    public interface ICommonMethod
    {
        /// <summary>UploadImage
        /// <Createdby>Nagendra Upwanshi</Createdby>
        /// <CreatedDate>Aug-22-2014</CreatedDate>
        /// </summary>        
        /// <param name="myFile"></param>
        /// <param name="path"></param>
        /// <param name="ImageName"></param>
        void UploadImage(HttpPostedFileBase myFile, string path, string imageName);

        /// <summary>IsEmailExist
        /// <Createdby>Nagendra Upwanshi</Createdby>
        /// <CreatedDate>Aug-22-2014</CreatedDate>
        /// <modifedby>Nagendra Upwanshi</modifedby>
        /// <modifiedOn>Oct-08-2014</modifiedOn>
        /// </summary>
        /// <param name="EmailId"></param>
        /// <returns></returns>
        bool IsEmailExist(string emailId);
        bool IsEmployeeIdExist(string employeeId);
        bool IsUserExists(string firstName, long userType);

        List<CountryModel> GetAllcountries();

        List<StateModel> GetStateByCountryId(long countryId);

        /// <summary>GetGlobalCodeData
        /// <CreatedBy>Gayatri</CreatedBy>
        /// <CreatedOn>28-Aug-2014</CreatedOn>
        /// <CreatedFor>To get the value from GlobalCode table</CreatedFor>
        /// <ModifiedBy>Nagendra Upwanshi</ModifiedBy>
        /// <ModifiedFor>apply filter isActive and Short Order orderrring</ModifiedFor>
        /// <ModifiedOn>Nov-13-2014</ModifiedOn>
        /// </summary>
        /// <param name="Category"></param>
        /// <returns></returns>
        List<GlobalCodeModel> GetGlobalCodeData(string category);

        /// <summary>GetGlobalCodeForName
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Oct-08-2014</CreatedOn>
        /// </summary>
        /// <param name="Category"></param>
        /// <param name="CodeName"></param>
        /// <returns></returns>
        long GetGlobalCodeForName(string category, string codeName);

        List<SelectListItem> GetGlobalCodeDataList(string category);
        List<LocationMasterModel> GetAllLocation();
        List<ServiceMasterModel> GetAllServices();
        //List<SelectListItem> GetNotAssgnProject(string UserType, long ProjectId);
        List<SelectListItem> GetWorkArea();
        List<SelectListItem> GetEmployeeProject(long projectId);
        List<SelectListItem> GetAllInventoryByProjectId(long projectId);
        //List<SelectListItem> GetAllAssetByWorkArea(long ProjectId, long WorkAreaId);
        //string GetAssetImage(long ProjectId, long AssetID);
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
        bool GenerateQRCode(string qrcName, long moduleId, long? projectCategoryId, long? projectId, long qrcDefaultSizeId, long qrcTypeId, string specialNotes, long? createdBy, out long qrcId);

        /// <summary>GetPlanByInsuranceCompanyId
        /// CreatedBy   :   Nagendra Upwanshi
        /// CreatedOn   :   Oct-14-2014
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        List<InsurancePlanModel> GetPlanByInsuranceCompanyId(long companyId);

        UserModel GetManagerByIdCode(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, out long totalRecords);

        /// <summary>
        /// Addded by vijay sahu on 16 june 2015
        /// </summary>       
        UserModel GetAdminByIdCode(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, out long totalRecords);
        QRCModel LoadInvitedUser(string usr);

        /// <summary>CreateRandomPassword()
        /// <CreatedBY>Nagendra Upwanshi</CreatedBY>
        /// <CreatedFor>Create Random Password</CreatedFor>
        /// <CreteadOn>Nov-18-2014</CreteadOn>
        /// </summary>
        /// <returns></returns>
        string CreateRandomPassword();

        List<GlobalCodeModel> GetUserTypeList(string category, long calledByUserType);

        /// <summary> User Details By Location</summary>
        /// <CreatedBY>Roshan Rahood</CreatedBY>
        /// <CreatedFor>Get the user details by location id</CreatedFor>
        /// <CreteadOn>Dec-23-2014</CreteadOn>
        /// <param name="locationId"></param>
        /// <returns></returns>
        UserDetailsForVerificationModel GetUserDetailsByLocationId(long locationId);

        /// <summary> Email To mail</summary>
        /// <CreatedBY>Roshan Rahood</CreatedBY>
        /// <CreatedFor>Get the user details by To mail</CreatedFor>
        /// <CreteadOn>Dec-24-2014</CreteadOn>
        /// <param name="locationId"></param>
        /// <returns></returns>
        List<EmailToUserModel> GetUsersToEmail(long locationId, long? employeeId);

        List<SelectListItem> GetAssetList(long LocationID);

        List<LocationListModel> GetLocationByAdminId(long? adminId);

        List<LocationListModel> GetLocationByManagerId(long managerId);

        List<LocationListModel> GetLocationByClientId(long clientId);

        List<LocationListModel> GetLocationByEmpId(long empId);

        List<UserModel> GetManagersBYLocationId(long locationId);

        List<UserModel> GetAdminBYLocationId(long locationId);

        List<UserModel> GetClientsBYLocationId(long locationId);

        LocationMasterModel GetLocationDetailsById(long locationId);

        Result SaveDAR(DARModel objDarModel);
        string GetAssetImageByQrcId(long qrcId);

        List<SelectListItem> GetLocationByManagerIdForDdl(long managerId);

        List<PermissionDetailsModel> GetAllPermissions(long locationId);

        bool UpdateUserPermissions(PermissionDetailsModel objPermissionDetailsModel);

        List<PermissionDetailsModel> GetAssignPermission(long userId, long LocationId);

        //bool DeleteUserPermission(long userId);

        /// <summary>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedFor>Email Status</CreatedFor>
        /// <CreatedDate>17-02-2015</CreatedDate>
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="body"></param>
        /// <param name="subject"></param>
        /// <param name="SentBy"></param>
        /// <param name="LocationID"></param>
        /// <returns></returns>

        bool EmailLog(long userId, long? sentTo, string emailId, string subject, long locationId);
   
        IsMapped isUserMappedWithLocation(long UserID, long LocationID);
        List<PermissionDetailsModel> GetPermissionsWithFilterByUserTypeLocationId(long locationId, long UserID);

        string GetGlobalCodeDetailById(long globalCodeId);

        bool ActivateNewUser(string usr);

        bool SendEmailJustforTesting(string email, string message);

        EmailToUserModel GetUserByID(long UserID);

        TimeZoneInfoModel GetTimeZoneInfo(string server);

        List<GlobalCodeModel> GetUserTypeListForUserRegistration(string category, long calledByUserType);

        List<PermissionDetailsModel> GetPermissionsWithUserType(long locationId, long userType);

        QRCModel LoadUnVerifiedInvitedUser(string usr);

        Result AssignLocationRoles(PermissionDetailsModel objPermissionDetailsModel, DARModel objDarModel, long createdBy);
    }

}
