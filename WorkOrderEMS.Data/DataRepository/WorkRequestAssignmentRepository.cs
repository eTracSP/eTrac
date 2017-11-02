using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;

namespace WorkOrderEMS.Data.DataRepository
{
    public class WorkRequestAssignmentRepository : BaseRepository<WorkRequestAssignment>
    {
        workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();

        public List<WorkRequestAssignmentModelList> GetAllWorkRequestAssignment(long? WorkRequestAssignmentID, long? RequestedBy, string OperationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, long LocationID, long UserID, DateTime StartDate, DateTime EndDate, string Filter, ObjectParameter paramTotalRecords)
        {
            List<WorkRequestAssignmentModelList> lstWorkRequest = new List<WorkRequestAssignmentModelList>();
            try
            {
                lstWorkRequest = _workorderEMSEntities.SP_GetAllWorkRequestAssignmentByUsertype(WorkRequestAssignmentID, RequestedBy, OperationName, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, LocationID, UserID, StartDate, EndDate, Filter, paramTotalRecords).Select(r => new WorkRequestAssignmentModelList()
                {
                    WorkRequestAssignmentID = r.WorkRequestAssignmentID,
                    WorkRequestType = r.WorkRequestType,
                    WorkRequestTypeName = r.WorkRequestTypeName,
                    AssetID = r.AssetID,
                    LocationID = r.LocationID,
                    LocationName = r.LocationName,
                    ProblemDesc = r.ProblemDesc,
                    PriorityLevel = r.PriorityLevel,
                    PriorityLevelName = r.PriorityLevelName,
                    WorkRequestImage = r.WorkRequestImage,
                    SafetyHazard = r.SafetyHazard,
                    ProjectDesc = r.ProjectDesc,
                    WorkRequestStatus = Convert.ToInt64(r.WorkRequestStatus),
                    WorkRequestStatusName = r.WorkRequestStatusName,
                    RequestBy = r.RequestBy,
                    AssignToUserId = r.AssignToUserId,
                    AssignToUserName = r.AssignToUserName,
                    AssignByUserId = r.AssignByUserId,
                    Remarks = r.Remarks,
                    CreatedBy = r.CreatedBy,
                    CreationDate = r.CreatedDate.ToString("MM/dd/yyyy hh:mm tt"),
                    ModifiedBy = r.ModifiedBy,
                    ModifiedDate = r.ModifiedDate,
                    IsDeleted = r.IsDeleted,
                    DeletedBy = r.DeletedBy,
                    DeletedDate = r.DeletedDate,
                    WorkRequestProjectType = r.WorkRequestProjectType,
                    WorkRequestProjectTypeName = r.WorkRequestProjectTypeName,
                    AssignedWorkOrderImage = r.AssignedWorkOrderImage,
                    ProfileImage = r.ProfileImage,
                    CodeID = r.CodeID,
                    CreatedByProfile = r.CreatedByProfile,
                    CreatedByUserName = r.CreatedByUserName,
                    StartTime = r.StartTime != null ? Convert.ToDateTime(r.StartTime).ToString("hh:mm tt") : null,
                    EndTime = r.EndTime != null ? Convert.ToDateTime(r.EndTime).ToString("hh:mm tt") : null,
                    FacilityRequestType = r.FacilityRequestType,
                    DisclaimerForm = r.DisclaimerForm,
                    SurveyForm = r.SurveyForm,
                    AssignedTime = r.AssignedTime != null ? Convert.ToDateTime(r.AssignedTime).ToString("HH:mm") : null,
                    StartDate = r.StartDate != null ? r.StartDate.Value.ToString("MM'/'dd'/'yyyy") : null,
                    EndDate = r.EndDate != null ? r.EndDate.Value.ToString("MM'/'dd'/'yyyy") : null,
                    WeekDays = r.WeekDaysName,
                    CustomerName = r.CustomerName,
                    VehicleMake = r.VehicleMake,
                    VehicleModel = r.VehicleModel,
                    CustomerContact = r.CustomerContact,
                    VehicleYear = r.VehicleYear,
                    VehicleColor = r.VehicleColor,
                    DriverLicenseNo = r.DriverLicenseNo,
                    PauseStatus = r.PauseStatus,
                    TotalTime = r.TotalTimeTaken,
                    ConStartTime = r.ConStartTime != null ? Convert.ToDateTime(r.ConStartTime).ToString("hh:mm tt") : null
                    // AssetName = r.QRCName
                }).ToList();
                return lstWorkRequest;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ssp_GetAllTaskListByEmpID_Result> GetTaskListByEmpID(string ServiceAuthKey, long UserId, string FromDate, string ToDate, long LocationId, string TimeZoneName, long TimeZoneOffset, bool IsTimeZoneinDaylight)
        {
            List<ssp_GetAllTaskListByEmpID_Result> lstWorkAssigned = new List<ssp_GetAllTaskListByEmpID_Result>();
            try
            {
                DateTime _fromDate = (string.IsNullOrEmpty(FromDate) == false ? Convert.ToDateTime(FromDate, CultureInfo.InvariantCulture) : DateTime.UtcNow.Date);
                DateTime _toDate = (string.IsNullOrEmpty(ToDate) == false ? Convert.ToDateTime(ToDate, CultureInfo.InvariantCulture) : DateTime.UtcNow);
                //Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
                if (_fromDate != null && _toDate != null)
                {
                    if ((_fromDate.Date == _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM") || (_toDate.ToLongTimeString() == "12:00:00 AM"))
                    {
                        _toDate = _toDate.AddDays(1).AddSeconds(-1);
                    }
                    if ((_toDate.ToLongTimeString() == "12:00:00 AM") || (_toDate.ToLongTimeString() == "12:00:00 AM"))
                    {
                        _toDate = _toDate.AddDays(1).AddSeconds(-1);
                    }
                }
                _fromDate = Convert.ToDateTime(_fromDate.ToMobileClientTimeZone(TimeZoneName,TimeZoneOffset,IsTimeZoneinDaylight));
                _toDate = Convert.ToDateTime(_toDate.ToMobileClientTimeZone(TimeZoneName, TimeZoneOffset, IsTimeZoneinDaylight));
                lstWorkAssigned = _workorderEMSEntities.ssp_GetAllTaskListByEmpID(ServiceAuthKey, UserId, _fromDate, _toDate, LocationId).Select(t => new ssp_GetAllTaskListByEmpID_Result()
                {
                    WorkRequestAssignmentID = t.WorkRequestAssignmentID,
                    AssetID = t.AssetID,
                    AssetName = t.AssetName,
                    QRCName = t.QRCName,
                    ProblemDesc = t.ProblemDesc,
                    ProjectDesc = t.ProjectDesc,
                    WorkRequestProjectType = t.WorkRequestProjectType,
                    WorkRequestProjectTypeName = t.WorkRequestProjectTypeName,
                    WorkRequestStatus = t.WorkRequestStatus,
                    WorkRequestStatusName = t.WorkRequestStatusName,
                    WorkRequestType = t.WorkRequestType,
                    WorkRequestTypeCodeName = t.WorkRequestTypeCodeName,
                    LocationID = t.LocationID,
                    LocationName = t.LocationName,
                    PriorityLevel = t.PriorityLevel,
                    SafetyHazard = t.SafetyHazard,
                    AssignByUserId = t.AssignByUserId,
                    AssignByUserName = t.AssignByUserName,
                    RequestBy = t.RequestBy,
                    RequestByName = t.RequestByName,
                    UserType = t.UserType,
                    CreatedDate = t.CreatedDate,
                    WorkRequestImage = t.WorkRequestImage,
                    WorkOrderCode = t.WorkOrderCode,
                    WorkOrderCodeID = t.WorkOrderCodeID,
                    CurrentLocation = t.CurrentLocation,
                    CustomerContact = t.CustomerContact,
                    CustomerName = t.CustomerName,
                    DriverLicenseNo = t.DriverLicenseNo,
                    VehicleColor = t.VehicleColor,
                    VehicleMake = t.VehicleMake,
                    VehicleModel = t.VehicleModel,
                    VehicleYear = t.VehicleYear,
                    Address = t.Address,
                    LicensePlateNo = t.LicensePlateNo,
                    FacilityRequest = t.FacilityRequest
                }).ToList();

                return lstWorkAssigned;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ssp_GetContinuousTaskListByEmpID_Result> GetContinuousTaskListByEmpID(string ServiceAuthKey, long UserId, long LocationId)
        {
            List<ssp_GetContinuousTaskListByEmpID_Result> lstWorkAssigned = new List<ssp_GetContinuousTaskListByEmpID_Result>();
            try
            {
                lstWorkAssigned = _workorderEMSEntities.ssp_GetContinuousTaskListByEmpID(ServiceAuthKey, UserId, LocationId).ToList();
                return lstWorkAssigned;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ServiceResponseModel<ssp_UpdateWorkRequestStatus_Result> UpdateWorkRequestStatus(string ServiceAuthKey, long UserId, long WorkRequestAssignmentID, int WorkRequestStatus, int LocationID, int WorkRequestType, string message, string StartTime = null, string EndTime = null, string WorkStatusDesc = null)
        {
            ServiceResponseModel<ssp_UpdateWorkRequestStatus_Result> ObjServiceResponseModel = new ServiceResponseModel<ssp_UpdateWorkRequestStatus_Result>();
            try
            {
                //long? test=UserId;
                ObjServiceResponseModel.Data = _workorderEMSEntities.ssp_UpdateWorkRequestStatus(ServiceAuthKey, UserId, WorkRequestAssignmentID, WorkRequestStatus, WorkRequestType, LocationID, message, StartTime, EndTime, WorkStatusDesc)
                                                .Select(t => new ssp_UpdateWorkRequestStatus_Result()
                {
                    Response = t.Response,
                    ResponseMessage = t.ResponseMessage
                }).FirstOrDefault();
            }
            catch (Exception) { throw; }
            return ObjServiceResponseModel;
        }

        public List<ssp_GetTaskListByClient_Result> GetClientRequestedTaskList(string ServiceAuthKey, long UserId, string FromDate, string ToDate, long LocationId, string TimeZoneName, long TimeZoneOffset, bool IsTimeZoneinDaylight)
        {
            List<ssp_GetTaskListByClient_Result> lstWorkAssigned = new List<ssp_GetTaskListByClient_Result>();
            try
            {
                DateTime _fromDate = (string.IsNullOrEmpty(FromDate) == false ? Convert.ToDateTime(FromDate, CultureInfo.InvariantCulture) : DateTime.UtcNow.Date);
                DateTime _toDate = (string.IsNullOrEmpty(ToDate) == false ? Convert.ToDateTime(ToDate, CultureInfo.InvariantCulture) : DateTime.UtcNow);
                //Ankit done this due to if date is same like 8/12 8/12 we need to fetch data from 12 am to 11.59.59 pm.
                if (_fromDate != null && _toDate != null)
                {
                    if ((_fromDate.Date == _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM") || (_toDate.ToLongTimeString() == "12:00:00 AM"))
                    {
                        _toDate = _toDate.AddDays(1).AddSeconds(-1);
                    }
                    if ((_toDate.ToLongTimeString() == "12:00:00 AM") || (_toDate.ToLongTimeString() == "12:00:00 AM"))
                    {
                        _toDate = _toDate.AddDays(1).AddSeconds(-1);
                    }
                }
                _fromDate = Convert.ToDateTime(_fromDate.ToMobileClientTimeZone(TimeZoneName, TimeZoneOffset, IsTimeZoneinDaylight));
                _toDate = Convert.ToDateTime(_toDate.ToMobileClientTimeZone(TimeZoneName, TimeZoneOffset, IsTimeZoneinDaylight));

                lstWorkAssigned = _workorderEMSEntities.ssp_GetTaskListByClient(ServiceAuthKey, UserId, _fromDate, _toDate,LocationId).Select(t => new ssp_GetTaskListByClient_Result()
                {
                    WorkRequestAssignmentID = t.WorkRequestAssignmentID,
                    AssetID = t.AssetID,
                    AssetName = t.AssetName,
                    QRCName = t.QRCName,
                    ProblemDesc = t.ProblemDesc,
                    ProjectDesc = t.ProjectDesc,
                    WorkRequestProjectType = t.WorkRequestProjectType,
                    WorkRequestProjectTypeName = t.WorkRequestProjectTypeName,
                    WorkRequestStatus = t.WorkRequestStatus,
                    WorkRequestStatusName = t.WorkRequestStatusName,
                    WorkRequestType = t.WorkRequestType,
                    WorkRequestTypeCodeName = t.WorkRequestTypeCodeName,
                    LocationID = t.LocationID,
                    LocationName = t.LocationName,
                    PriorityLevel = t.PriorityLevel,
                    SafetyHazard = t.SafetyHazard,
                    AssignByUserId = t.AssignByUserId,
                    AssignByUserName = t.AssignByUserName,
                    RequestBy = t.RequestBy,
                    RequestByName = t.RequestByName,
                    CreatedDate = t.CreatedDate,
                    WorkOrderCode = t.WorkOrderCode,
                    WorkOrderCodeID = t.WorkOrderCodeID

                }).ToList();

                return lstWorkAssigned;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long SaveWorkOrderRequestQRCType(ServiceWorkAssignmentModel ObjServiceWorkAssignmentModel)
        {
            WorkRequestAssignment ObjWorkRequestAssignment = new WorkRequestAssignment();
            try
            {
                ObjWorkRequestAssignment.AssetID = ObjServiceWorkAssignmentModel.AssetID;
                ObjWorkRequestAssignment.AssignByUserId = ObjServiceWorkAssignmentModel.AssignByUserId;
                ObjWorkRequestAssignment.AssignToUserId = ObjServiceWorkAssignmentModel.AssignToUserId;
                ObjWorkRequestAssignment.CreatedBy = ObjServiceWorkAssignmentModel.RequestBy;
                ObjWorkRequestAssignment.CreatedDate = DateTime.UtcNow;
                ObjWorkRequestAssignment.DeletedBy = null;
                ObjWorkRequestAssignment.DeletedDate = null;
                ObjWorkRequestAssignment.IsDeleted = false;
                ObjWorkRequestAssignment.LocationID = ObjServiceWorkAssignmentModel.LocationID;
                ObjWorkRequestAssignment.ModifiedBy = null;
                ObjWorkRequestAssignment.ModifiedDate = null;
                ObjWorkRequestAssignment.PriorityLevel = ObjServiceWorkAssignmentModel.PriorityLevel;
                ObjWorkRequestAssignment.ProblemDesc = ObjServiceWorkAssignmentModel.ProblemDescription;
                ObjWorkRequestAssignment.Remarks = ObjServiceWorkAssignmentModel.Remarks;
                ObjWorkRequestAssignment.RequestBy = ObjServiceWorkAssignmentModel.RequestBy;
                ObjWorkRequestAssignment.SafetyHazard = ObjServiceWorkAssignmentModel.SafetyHazard;
                ObjWorkRequestAssignment.WorkRequestImage = ObjServiceWorkAssignmentModel.WorkRequestImage;
                ObjWorkRequestAssignment.AssignedWorkOrderImage = ObjServiceWorkAssignmentModel.WorkRequestImage;
                ObjWorkRequestAssignment.WorkRequestProjectType = ObjServiceWorkAssignmentModel.WorkRequestProjectType;
                ObjWorkRequestAssignment.WorkRequestStatus = ObjServiceWorkAssignmentModel.WorkRequestStatus;
                ObjWorkRequestAssignment.WorkRequestType = ObjServiceWorkAssignmentModel.WorkRequestType;
                ObjWorkRequestAssignment.ProjectDesc = ObjServiceWorkAssignmentModel.ProjectDescription;
                ObjWorkRequestAssignment.WorkOrderCode = ObjServiceWorkAssignmentModel.WorkrequestCode;

                Add(ObjWorkRequestAssignment);
                long WorkAssignID = ObjWorkRequestAssignment.WorkRequestAssignmentID;

                ObjWorkRequestAssignment.WorkOrderCodeID = WorkAssignID + 100;

                Update(ObjWorkRequestAssignment);
                return WorkAssignID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public List<WorkRequestAssignmentModelList> GetAllWorkRequestCreatedByClient() {
        //    var data=_workorderEMSEntities.WorkRequestAssignments
        //        .Join(_workorderEMSEntities.UserRegistrations,wr=>wr.CreatedBy,ur=>ur.UserId,(wr,ur)=> new{wr,ur})
        //        .Join(_workorderEMSEntities.GlobalCodes,z=>z.ur.UserType,gc=>gc.GlobalCodeId,(z,gc)=>new{z,gc})
        //        .Select(s=> new WorkRequestAssignmentModelList(){

        //            WorkRequestAssignmentID = s.z.wr.WorkRequestAssignmentID,
        //            WorkRequestType = s.z.wr.WorkRequestType,
        //            WorkRequestTypeName = s.z.wr.WorkRequestTypeName,
        //            AssetID = s.z.wr.AssetID,
        //            LocationID = s.z.wr.LocationID,
        //            LocationName = s.z.wr.LocationName,
        //            ProblemDesc = s.z.wr.ProblemDesc,
        //            PriorityLevel = s.z.wr.PriorityLevel,
        //            PriorityLevelName = s.z.wr.PriorityLevelName,
        //            WorkRequestImage = s.z.wr.WorkRequestImage,
        //            SafetyHazard = s.z.wr.SafetyHazard,
        //            ProjectDesc = s.z.wr.ProjectDesc,
        //            WorkRequestStatus = Convert.ToInt64(s.z.wr.WorkRequestStatus),
        //            WorkRequestStatusName = s.z.wr.WorkRequestStatusName,
        //            RequestBy = s.z.wr.RequestBy,
        //            AssignToUserId = s.z.wr.AssignToUserId,
        //            AssignToUserName = s.z.wr.AssignToUserName,
        //            AssignByUserId = s.z.wr.AssignToUserId,
        //            Remarks = s.z.wr.Remarks,
        //            CreatedBy = s.z.wr.CreatedBy,
        //            CreatedDate = s.z.wr.CreatedDate,
        //            ModifiedBy = s.z.wr.ModifiedBy,
        //            ModifiedDate = s.z.wr.ModifiedDate,
        //            IsDeleted = s.z.wr.IsDeleted,
        //            DeletedBy = s.z.wr.DeletedBy,
        //            DeletedDate = s.z.wr.DeletedDate,
        //            WorkRequestProjectType = s.z.wr.WorkRequestProjectType,
        //            WorkRequestProjectTypeName = s.z.wr.WorkRequestProjectTypeName,
        //            AssignedWorkOrderImage=s.z.wr.AssignedWorkOrderImage,
        //            ProfileImage=s.z.wr.ProfileImage,
        //             CodeID =s.z.wr.CodeID,

        //        })

        //}
        /// <summary>
        /// TO GET UN-ASSIGNED WORKODER CREATED BY CLIENT
        /// <Created By>Manoj Jaswal</Created>
        /// <CreatedDate>4/7/2015</CreatedDate>
        /// </summary>
        /// <param name="WorkRequestAssignmentID"></param>
        /// <param name="RequestedBy"></param>
        /// <param name="OperationName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <param name="LocationID"></param>
        /// <param name="UserID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="paramTotalRecords"></param>
        /// <returns></returns>
        public List<WorkRequestAssignmentModelList> GetAllWorkRequestCreatedByClient(long? WorkRequestAssignmentID, long? RequestedBy, string OperationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, long LocationID, long UserID, DateTime StartDate, DateTime EndDate, string filter, ObjectParameter paramTotalRecords)
        {
            try
            {
                var Data = _workorderEMSEntities.SP_GetAllWorkRequestCreatedByClient(WorkRequestAssignmentID, RequestedBy, OperationName, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, LocationID, UserID, StartDate, EndDate, filter, paramTotalRecords).Select(r => new WorkRequestAssignmentModelList()
                {
                    WorkRequestAssignmentID = r.WorkRequestAssignmentID,
                    WorkRequestType = r.WorkRequestType,
                    WorkRequestTypeName = r.WorkRequestTypeName,
                    AssetID = r.AssetID,
                    LocationID = r.LocationID,
                    LocationName = r.LocationName,
                    ProblemDesc = r.ProblemDesc,
                    PriorityLevel = r.PriorityLevel,
                    PriorityLevelName = r.PriorityLevelName,
                    WorkRequestImage = r.WorkRequestImage,
                    SafetyHazard = r.SafetyHazard,
                    ProjectDesc = r.ProjectDesc,
                    WorkRequestStatus = Convert.ToInt64(r.WorkRequestStatus),
                    WorkRequestStatusName = r.WorkRequestStatusName,
                    RequestBy = r.RequestBy,
                    AssignToUserId = r.AssignToUserId,
                    AssignToUserName = r.AssignToUserName,
                    AssignByUserId = r.AssignToUserId,
                    Remarks = r.Remarks,
                    CreatedBy = r.CreatedBy,
                    CreatedDate = r.CreatedDate,
                    ModifiedBy = r.ModifiedBy,
                    ModifiedDate = r.ModifiedDate,
                    IsDeleted = r.IsDeleted,
                    DeletedBy = r.DeletedBy,
                    DeletedDate = r.DeletedDate,
                    WorkRequestProjectType = r.WorkRequestProjectType,
                    WorkRequestProjectTypeName = r.WorkRequestProjectTypeName,
                    AssignedWorkOrderImage = r.AssignedWorkOrderImage,
                    ProfileImage = r.ProfileImage,
                    CodeID = r.CodeID,
                    StartTime = r.StartTime != null ? Convert.ToDateTime(r.StartTime).ToString("MM/dd/yyyy HH:mm:ss") : null,
                    EndTime = r.EndTime != null ? Convert.ToDateTime(r.EndTime).ToString("MM/dd/yyyy HH:mm:ss") : null,
                    AssignedTime = r.AssignedTime != null ? Convert.ToDateTime(r.AssignedTime).ToString("MM/dd/yyyy HH:mm:ss") : null,
                    PauseStatus = r.PauseStatus
                    // AssetName = r.QRCName
                }).ToList();
                return Data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void GetWorkRequestAssignedtoEmployee(long LocationId, string Type, long employeeId, string Filter)
        {
            var data = _workorderEMSEntities.SP_GetAllWorkOrderAssignedToEmployee(LocationId, Type, employeeId, Filter);
        }

        /// <summary>
        /// TO GET EMPLOYEE DASHBOARD COUNT
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        public List<EmployeeWorkAssignmentCountModel> GetEmployeeTotalWorkStatus(long UserId, long LocationId)
        {
            try
            {
                return _workorderEMSEntities.Proc_GetEmployeeTotalWork(UserId, LocationId).Select(x => new EmployeeWorkAssignmentCountModel()
                {
                    CodeName = x.CodeName,
                    Column1 = x.Total,
                }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
