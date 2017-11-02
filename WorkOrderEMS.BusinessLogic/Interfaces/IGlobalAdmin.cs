using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Web.Mvc;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;
using WorkOrderEMS.Models.SuperAdminModels;
using WorkOrderEMS.Models.UserModels;

namespace WorkOrderEMS.BusinessLogic
{
    public interface IGlobalAdmin
    {

        //Result SaveLocation(LocationMasterModel objLocationMasterModel, out long locationId);

        Result SaveLocation(LocationMasterModel objLocationMasterModel, out QRCModel qrcDetail);

        LocationMasterModel GetLocationById(long locationId);

        Result DeleteLocation(long locationId, DARModel objDAR);

        //Result SaveProject(ProjectMasterModel objProjectMasterModel, string ServicesID, out QRCModel QRCDetail);

        Result SaveManager(UserModel objUserModel, out long qrcId, bool isManagerRegistration);

        //Result SendInvitation(UserModel objUserModel, string UserType);
        Result SendInvitation(UserModel objUserModel);

        //UserModel GetManagerByID(long UserID, string OperationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter paramTotalRecords);
        UserModel GetManagerById(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, out long totalRecords);

        //Result AssignProject(UserModel objUserModel);

        //Result DeleteProject(long ProjectID);

        /// <summary>GetAllITAdministratorList
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
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
        List<UserModelList> GetAllITAdministratorList(long? userId, long locationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string searchText, string myUserType, out long totalRecords);
        //List<UserModelList> GetAllITAdministratorList(long? UserId, int? PageIndex, int? NumberOfRows, string SortColumnName, string SortOrderBy, string SearchText, long? myUserType);

        List<UserModelList> GetAllITAdministratorListForReport(long? userId, long locationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string searchText, string myUserType, out long totalRecords);

        /// <summary>GetAllLocationNew
        /// <CreatedOn>NOv-18-2014</CreatedOn>
        /// <CreatedFor>Get All New Location for SelectListItem</CreatedFor>
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// </summary>
        /// <returns></returns>
        List<LocationListModel> GetAllLocationNew();

        /// <summary>ProcessLocationSetup
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-19-2014</CreatedOn>
        /// <CreatedFor>Process Location Setup</CreatedFor>
        /// </summary>
        /// <param name="ObjLocationMasterModel"></param>
        /// <returns></returns>
        Result ProcessLocationSetup(LocationMasterModel objLocation, out long verificationManagerId, out long verificationClientId, out long outLocationId, out bool outSendMail);

        LocationMasterModel GetLocationDetailsByLocationId(long locationId);

        LocationMasterModel GetManagerByIdCode(long userId);

        List<LocationListModel> GetAllLocationList(int locationId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords);

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
        List<ListLocationModel> ListAllLocation(int? locationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords);



        /// <summary> GetLocationListAdministratorClient
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedFor>Get Location List Administrator or Client</CreatedFor>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="CalledForUserType"></param>
        /// <returns></returns>
        //List<UserModelList> GetLocationListAdministratorClient(string LocationId, UserType CalledForUserType = UserType.Administrator);


        /// <summary> GetLocationListAdministratorClient
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// Modified by vijay sahu on 9 march 2015
        /// <CreatedOn>Dec-10-2014</CreatedOn>
        /// <CreatedFor>Get Location List Administrator or Client</CreatedFor>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="CalledForUserType"></param>
        /// <returns></returns>
        List<UserModelList> GetLocationListAdministratorClient(string locationId, string UserType = "");


        /// <summary> get un assigned Administrator list for selected location.
        /// <CreatedBy>Vijay Sahu march 10 2015 </CreatedBy>
        /// <CreatedOn>Dec-10-2014</CreatedOn>
        /// <CreatedFor>Get Location List Administrator or Client</CreatedFor>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="CalledForUserType"></param>
        /// <returns></returns>
        List<SelectListItem> UnAssignedAdministratorId(string locationId, string UserType);



        /// <summary>MapAdminForLocation
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Dec-10-2014</CreatedOn>
        /// <CreatedFor>Map Admin For Location</CreatedFor>
        /// </summary>
        /// <param name="_LocationID"></param>
        /// <param name="_AdminUserId"></param>
        /// <param name="IsDelete"></param>
        Tuple<bool, int> MapAdminForLocation(long locationId, long adminUserId, long loginUser, bool? isDelete);


        /// <summary>MapEmployeeForLocation
        /// <CreatedBy>Created by vijay sahu</CreatedBy>
        /// <CreatedOn>Dec-10-2014</CreatedOn>
        /// <CreatedFor>Map Admin For Location</CreatedFor>
        /// </summary>
        /// <param name="_LocationID"></param>
        /// <param name="_AdminUserId"></param>
        /// <param name="IsDelete"></param>
        bool MapEmployeeForLocation(long locationId, long adminUserId, long loginUser, string locationname, bool? isDelete);



        /// <summary>Map manager with selected location 
        /// <CreatedBy>Vijay Sahu on 10 march 2015</CreatedBy>
        /// <CreatedOn>Dec-10-2014</CreatedOn>
        /// <CreatedFor>Map Admin For Location</CreatedFor>
        /// </summary>
        /// <param name="_LocationID"></param>
        /// <param name="_AdminUserId"></param>
        /// <param name="IsDelete"></param>
        Tuple<bool, int> MapManagerForLocation(long locationId, long adminUserId, long loginUser, bool? isDelete);



        List<SelectListItem> GetLocationEmployee(long locationId);
        WorkRequestAssignmentModel SaveWorkRequestAssignment(WorkRequestAssignmentModel objWorkRequestAssignmentModel);
        Result AssignedToWorkRequestAssignment(WorkRequestAssignmentModel workRequestAssignmentModel);

        /// <summary>Location verification by Manger
        /// <CreatedBy>Roshan Rahood</CreatedBy>
        /// <CreatedOn>Dec-22-2014</CreatedOn>
        /// <CreatedFor>Verify the Location by manger</CreatedFor>
        /// </summary>
        /// <param name="_LocationID"></param>
        /// <param name="_AdminUserId"></param>
        /// <param name="IsDelete"></param>
        bool ManagerLocationApproval(long verificationManagerId, long locationId);


        /// <summary>Location verification by client.
        /// <CreatedBy>Roshan Rahood</CreatedBy>
        /// <CreatedOn>Dec-22-2014</CreatedOn>
        /// <CreatedFor>Verify the Location by manger</CreatedFor>
        /// </summary>
        /// <param name="_LocationID"></param>
        /// <param name="_AdminUserId"></param>
        /// <param name="IsDelete"></param>
        bool ClientLocationApproval(long verificationClientId, long locationId);

        /// <summary>
        /// To Get the Location of the SuperAdmin
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>2015/2/23</CreatedDate>
        /// </summary>
        /// <returns></returns>
        List<MasterLocationModel> GetSuperAdminUserLocation();

        long GetTotalManagerCount(string LoginUserType, long LocationID, long userId);
        List<GlobalUserModel> GetApplicationGlobalAdmin();


        /// <summary>
        /// Created by vijay sahu on 24 march 2015
        /// To Check that location name is already exists or not. 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        byte isLocationNameExists(string locationName);

        /// <summary>
        /// Created by vijay sahu on 2 may 2015
        /// To Check that location code is already exists or not. 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        byte isLocationCodeExists(string locationCode);


        /// <summary>
        /// Created by vijay sahu on 24 march 2015
        /// To Check that email ID is already exists or not. 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        byte isEmailExists(string Email, string AlternateEmail);
        List<LocationDetailModel> LocationDetailByLocationID(long LocationID);
        dynamic WorkOrderDetailsForPushNotificaiton(string id, long userId, long userType);

        byte isEmployeeIdExists(string empId);


        List<listForEmployeeDevice> sendNotificaitonToAllEmployee(long LocationId, long UserId);

        listForEmployeeDevice sendNotificationContinuousRequestToEmployee(long LocationId, long? UserId, WorkRequestAssignmentModel obj);


        WorkOrderEMS.Models.eMaintenance_M.WorkRequestAssignment_M GetDataForRendringHTML(long WorkRequestAssignmentRequestId);

        string SubmitSurveyForm(WorkOrderEMS.Models.eMaintenance_M.eMaintenanceSurvey_M obj);

        bool EquipmentCheckOutStatus(long LocId, long userId, long userType);

        string GetDashboardHeadCount(long locationId, long UserId, DateTime? fromDate, DateTime? toDate, long usertype);

        string GetWorkOrderforDashboard(long locationId, long UserId, DateTime? fromDate, DateTime? toDate);

        List<ReportChart> QrcScannedDetails(long? LocationId, long qrcName, long userType, long userId, DateTime? fromDate, DateTime? toDate);

        bool SaveByDefaultWidgetSetting(long locationId, string servicesId, long? UserId);

        /// <summary>
        /// Created By: Bhushan Dod
        /// Create Date: 05-Oct-2016
        /// This method is used to fetch the unverified user list according to location.
        /// </summary>
        List<UserModelList> GetAllUnVerifiedUserList(long? userId, long locationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string searchText, string myUserType, out long totalRecords);
        List<UserModelList> GetAllUserListforDAR(long? userId, long locationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string searchText, string myUserType, out long totalRecords);
    }
}
