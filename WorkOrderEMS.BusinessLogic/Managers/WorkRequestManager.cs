using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Transactions;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class WorkRequestManager : IWorkRequestAssignment
    {
        DARRepository objDARRepository;
        UserRepository ObjUserRepository;
        WorkRequestAssignmentRepository ObjWorkRequestAssignmentRepository;
        EmailLogRepository objEmailLogRepository;
        string message = string.Empty;
        private readonly string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private readonly string ConstantImages = ConfigurationManager.AppSettings["ConstantImages"];
        //workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();

        CommonMethodManager ObjCommonMethodManager = new CommonMethodManager();

        /// <summary>Udate status of WorkRequest Assignment
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>UpdateTaskStatus</CreatedFor>
        /// <CreatedOn>Jan-16-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceWorkStatusModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> UpdateTaskStatus(ServiceWorkStatusModel obj)
        {
            ObjUserRepository = new UserRepository();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            ObjWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
            string message = string.Empty;
            try
            {
                // var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == ObjWorkStatusModel.ServiceAuthKey && x.UserId == ObjWorkStatusModel.UserId);

                if (
                    obj.WorkRequestAssignmentID != null &&
                    obj.WorkRequestStatus != null &&
                    obj.WorkRequestType != null
                    )
                {

                    //if (obj.WorkRequestStatus == 14 && obj.StartTime == null && obj.EndTime == null)
                    //{
                    //    message = DarMessage.DarUpdateTaskStatus(obj.UserName, obj.ClientUserName);
                    //}
                    if (obj.StartTime != null)
                    {
                        obj.StartTime = DateTime.UtcNow.ToString();
                    }
                    if (obj.EndTime != null)
                    {
                        obj.EndTime = DateTime.UtcNow.ToString();
                    }

                    var result = ObjWorkRequestAssignmentRepository.UpdateWorkRequestStatus(obj.ServiceAuthKey,
                                                                                            obj.UserId,
                                                                                            obj.WorkRequestAssignmentID,
                                                                                            obj.WorkRequestStatus,
                                                                                            obj.LocationID,
                                                                                            obj.WorkRequestType, obj.AcitivityDetails,
                                                                                            obj.StartTime, // Convert.ToDateTime(obj.StartTime).ToString("mm-dd-yyyy hh:mm:tt"),//
                                                                                            obj.EndTime,
                                                                                            obj.WorkStatusDesc);
                    ObjServiceResponseModel.Response = Convert.ToInt64(result.Data.Response, CultureInfo.InvariantCulture);
                    ObjServiceResponseModel.Message = (result.Data.ResponseMessage).ToString();//CommonMessage.MessageLogout();

                }
                else
                {

                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception)
            { throw; }

            return ObjServiceResponseModel;
        }

        /// <summary>Get task list by employee id
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedOn>Jan-12-2015</CreatedOn>
        /// <CreatedFor> GetTaskListByEmpID</CreatedFor>
        /// </summary>
        /// <param name="ServiceAuthKey"></param>
        /// <returns></returns>
        public List<ServiceWorkAssignmentModel> GetClientRequestedTaskList(string serviceAuthKey, long userId, string fromDate, string toDate, long locationId, string TimeZoneName, long TimeZoneOffset, bool IsTimeZoneinDaylight)
        {
            WorkRequestAssignmentRepository workRequestAssignmentRepository = new WorkRequestAssignmentRepository();
            try
            {
                List<ServiceWorkAssignmentModel> tasklist = workRequestAssignmentRepository.GetClientRequestedTaskList(serviceAuthKey, userId,
                                                           fromDate, toDate, locationId, TimeZoneName, TimeZoneOffset, IsTimeZoneinDaylight).Select(t => new ServiceWorkAssignmentModel()
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
                                                               CreatedDate = t.CreatedDate.ToString(),
                                                               WorkrequestCode = t.WorkOrderCode + t.WorkOrderCodeID.ToString()

                                                           }).ToList();
                // List<ServiceWorkAssignmentModel> tasklist = new List<ServiceWorkAssignmentModel>();
                return tasklist;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>Save Work Request Assignment
        /// <CreatedFor>For Insert QRC TrashCan work order issue request</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Jan-30-2015</CreatedOn>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns> 
        public ServiceWorkAssignmentModel SaveWorkOrderRequest(ServiceWorkAssignmentModel obj)
        {
            ObjUserRepository = new UserRepository();
            ObjWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();

            try
            {
                if (obj.AssetID != null && obj.LocationID != null)
                {
                    var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == obj.ServiceAuthKey && x.IsDeleted == false);

                    if (authuser != null && authuser.UserId > 0)
                    {
                        var result = ObjWorkRequestAssignmentRepository.SaveWorkOrderRequestQRCType(obj);
                        if (result != null || result > 0)
                        {
                            obj.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                            obj.WorkRequestAssignmentID = result;
                            obj.ResponseMessage = CommonMessage.SaveSuccessMessage();
                        }
                        else
                        {
                            obj.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                            obj.WorkRequestAssignmentID = result;
                            obj.ResponseMessage = CommonMessage.WrongParameterMessage();
                        }
                    }
                    else
                    {
                        obj.Response = Convert.ToInt32(ServiceResponse.InvalidSessionResponse, CultureInfo.CurrentCulture);
                        obj.ResponseMessage = CommonMessage.InvalidUser();
                    }
                }
                else
                {

                    obj.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    obj.ResponseMessage = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception)
            { throw; }
            return obj;
        }

        /// <summary>DeleteWorkRequest
        /// CreatedBy   :   Roshan Rahood
        /// CreatedOn   :   Feb 18 2015
        /// CreatedFor  :   Delete work request 
        /// </summary>
        /// <param name="VendorID"></param>
        /// <returns></returns>
        public Result DeleteWorkRequest(long workRequestId, long loggedInUserId, DARModel objDAR, string location)
        {
            Result result;
            try
            {
                if (workRequestId > 0)
                {
                    if (true)
                    {
                        ObjWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
                        var data = ObjWorkRequestAssignmentRepository.GetSingleOrDefault(v => v.WorkRequestAssignmentID == workRequestId && v.IsDeleted == false);
                        if (data != null)
                        {
                            data.IsDeleted = true;
                            data.DeletedBy = loggedInUserId;
                            data.DeletedDate = DateTime.UtcNow;
                            ObjWorkRequestAssignmentRepository.Update(data);

                            objDAR.ActivityDetails = DarMessage.DeleteWorkRequest(data.WorkOrderCode, location);
                            objDAR.TaskType = (long)TaskTypeCategory.WorkOrderDelete;

                            #region Save DAR
                            result = ObjCommonMethodManager.SaveDAR(objDAR);
                            #endregion Save DAR

                            return Result.Delete;
                        }

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
        /// TO GET WORK REQUEST ASSIGNMENT BY ID
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>1 April 2015</CreatedDate>
        /// <param name="WorkRequestAssignmentID"></param>
        /// <returns></returns>
        //public WorkRequestAssignmentModel GetWorkorderAssignmentByID(long WorkRequestAssignmentID)
        //{
        //    try
        //    {
        //        ObjWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
        //        var returndata = ObjWorkRequestAssignmentRepository.GetAll(v => v.WorkRequestAssignmentID == WorkRequestAssignmentID && v.IsDeleted == false).Select(x => new WorkRequestAssignmentModel()
        //        {
        //            WorkRequestAssignmentID = x.WorkRequestAssignmentID,
        //            WorkRequestType = x.WorkRequestType,
        //            AssetID = x.AssetID,
        //            LocationID = x.LocationID,
        //            ProblemDesc = x.ProblemDesc,
        //            PriorityLevel = x.PriorityLevel,
        //            WorkRequestImage = x.WorkRequestImage,
        //            SafetyHazard = x.SafetyHazard,
        //            ProjectDesc = x.ProjectDesc,
        //            RequestBy = x.RequestBy,
        //            AssignToUserId = x.AssignToUserId,
        //            AssignByUserId = x.AssignByUserId,
        //            Remarks = x.Remarks,
        //            CreatedBy = x.CreatedBy,
        //            CreatedDate = x.CreatedDate,//.ToClientTimeZoneinDateTime(),
        //            ModifiedBy = x.ModifiedBy,
        //            ModifiedDate = x.ModifiedDate,//.Value.ToClientTimeZoneinDateTime(),
        //            DeletedBy = x.DeletedBy,
        //            DeletedDate = x.DeletedDate,//.Value.ToClientTimeZoneinDateTime(),
        //            WorkRequestProjectType = x.WorkRequestProjectType,
        //            AssignedWorkOrderImage = x.AssignedWorkOrderImage,
        //            StartTime = x.StartTime,//.Value.ToClientTimeZoneinDateTime(),
        //            EndTime = x.EndTime,//.Value.ToClientTimeZoneinDateTime(),
        //            AssignedTime = x.AssignedTime,
        //            WorkStatusDesc = x.WorkStatusDesc,
        //            WorkOrderCode = x.WorkOrderCode,
        //            WorkOrderCodeID = x.WorkOrderCodeID,
        //            FacilityRequestId = Convert.ToInt64(x.FacilityRequestId),
        //            DisclaimerForm = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DisclaimerForm"], CultureInfo.InvariantCulture) + x.DisclaimerForm,
        //            SurveyForm = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SurveyForm"], CultureInfo.InvariantCulture) + x.SurveyForm,
        //            CustomerName = x.CustomerName,
        //            CustomerContact = x.CustomerContact,
        //            VehicleColor = x.VehicleColor,
        //            VehicleMake = x.VehicleMake,
        //            VehicleModel = x.VehicleModel,
        //            DriverLicenseNo = x.DriverLicenseNo,
        //            VehicleYear = x.VehicleYear,
        //            CurrentLocation = x.CurrentLocation,
        //            Address = x.Address,
        //            StateId = x.StateId,
        //            ZipCode = x.ZipCode,
        //            City = x.City,
        //            StartDate = x.StartDate,//.Value.ToClientTimeZoneinDateTime(),
        //            EndDate = x.EndDate,//.Value.ToClientTimeZoneinDateTime(),
        //            CrStartTime = x.StartTime.ToString(),//.Value.ToClientTimeZone(true),
        //            //WeekDayLst = x.WeekDays,
        //            WeekDayLst = x.WeekDaysName,
        //            LicensePlateNo = x.LicensePlateNo

        //        }).SingleOrDefault();


        //        returndata.CreatedDate = returndata.CreatedDate.ToClientTimeZoneinDateTime();
        //        if (returndata.ModifiedDate != null)
        //            returndata.ModifiedDate = returndata.ModifiedDate.Value.ToClientTimeZoneinDateTime();
        //        if (returndata.DeletedDate != null)
        //            returndata.DeletedDate = returndata.DeletedDate.Value.ToClientTimeZoneinDateTime();
        //        if (returndata.StartTime != null)
        //            returndata.StartTime = returndata.StartTime.Value.ToClientTimeZoneinDateTime();
        //        if (returndata.EndTime != null)
        //            returndata.EndTime = returndata.EndTime.Value.ToClientTimeZoneinDateTime();
        //        if (returndata.StartDate != null)
        //            returndata.StartDate = returndata.StartDate.Value.ToClientTimeZoneinDateTime();
        //        if (returndata.EndDate != null)
        //            returndata.EndDate = returndata.EndDate.Value.ToClientTimeZoneinDateTime();
        //        if (returndata.StartTime != null)
        //            returndata.CrStartTime = returndata.StartTime.Value.ToClientTimeZone(true);

        //        return returndata;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        public WorkRequestAssignmentModel GetWorkorderAssignmentByID(long WorkRequestAssignmentID)
        {
            try
            {
                ObjWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
                return ObjWorkRequestAssignmentRepository.GetAll(v => v.WorkRequestAssignmentID == WorkRequestAssignmentID && v.IsDeleted == false).Select(x => new WorkRequestAssignmentModel()
                {
                    WorkRequestAssignmentID = x.WorkRequestAssignmentID,
                    WorkRequestType = x.WorkRequestType,
                    AssetID = x.AssetID,
                    LocationID = x.LocationID,
                    ProblemDesc = x.ProblemDesc,
                    PriorityLevel = x.PriorityLevel,
                    WorkRequestImage = x.WorkRequestImage,
                    SafetyHazard = x.SafetyHazard,
                    ProjectDesc = x.ProjectDesc,
                    RequestBy = x.RequestBy,
                    AssignToUserId = x.AssignToUserId,
                    AssignByUserId = x.AssignByUserId,
                    Remarks = x.Remarks,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                    DeletedBy = x.DeletedBy,
                    DeletedDate = x.DeletedDate,
                    WorkRequestProjectType = x.WorkRequestProjectType,
                    AssignedWorkOrderImage = x.AssignedWorkOrderImage,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    AssignedTime = x.AssignedTime,
                    WorkStatusDesc = x.WorkStatusDesc,
                    WorkOrderCode = x.WorkOrderCode,
                    WorkOrderCodeID = x.WorkOrderCodeID,
                    FacilityRequestId = Convert.ToInt64(x.FacilityRequestId),
                    DisclaimerForm = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DisclaimerForm"], CultureInfo.InvariantCulture) + x.DisclaimerForm,
                    SurveyForm = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SurveyForm"], CultureInfo.InvariantCulture) + x.SurveyForm,
                    CustomerName = x.CustomerName,
                    CustomerContact = x.CustomerContact,
                    VehicleColor = x.VehicleColor,
                    VehicleMake = x.VehicleMake,
                    VehicleModel = x.VehicleModel,
                    DriverLicenseNo = x.DriverLicenseNo,
                    VehicleYear = x.VehicleYear,
                    CurrentLocation = x.CurrentLocation,
                    Address = x.Address,
                    StateId = x.StateId,
                    ZipCode = x.ZipCode,
                    City = x.City,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    //CrStartTime = x.StartTime.ToString("hh:mm tt"),
                    CrStartTime = (x.ConStartTime != null)? x.ConStartTime.Value.ToClientTimeZoneinDateTime().ToString("hh:mm tt") : x.ConStartTime.ToString("hh:mm tt"),
                    //WeekDayLst = x.WeekDays,
                    WeekDayLst = x.WeekDaysName,
                    LicensePlateNo = x.LicensePlateNo

                }).SingleOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// To Get WorkOrder Created by Client and Not Assigned
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
            WorkRequestAssignmentRepository obj_WorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
            return obj_WorkRequestAssignmentRepository.GetAllWorkRequestCreatedByClient(WorkRequestAssignmentID, RequestedBy, OperationName, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, LocationID, UserID, StartDate, EndDate, filter, paramTotalRecords).Select(r => new WorkRequestAssignmentModelList()
            {
                WorkRequestAssignmentID = 0,
                WorkRequestID = Cryptography.GetEncryptedData(r.WorkRequestAssignmentID.ToString(), true),
                WorkRequestType = r.WorkRequestType,
                WorkRequestTypeName = r.WorkRequestTypeName,
                AssetID = r.AssetID,
                LocationID = r.LocationID,
                LocationName = r.LocationName,
                ProblemDesc = (r.ProblemDesc == null || r.ProblemDesc.Trim() == "") ? r.ProjectDesc : r.ProblemDesc,//Added By Bhushan for Prob Desc for WO and Proj Desc for SP
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
                CreationDate = (r.CreatedDate != null) ? r.CreatedDate.Value.ToClientTimeZone(true) : null,
                ModifiedBy = r.ModifiedBy,
                ModifiedDate = (r.ModifiedDate != null) ? r.ModifiedDate.Value.ToClientTimeZoneinDateTime() : r.ModifiedDate,
                IsDeleted = r.IsDeleted,
                DeletedBy = r.DeletedBy,
                DeletedDate = (r.DeletedDate != null) ? r.DeletedDate.Value.ToClientTimeZoneinDateTime() : r.DeletedDate,
                WorkRequestProjectType = r.WorkRequestProjectType,
                WorkRequestProjectTypeName = r.WorkRequestProjectTypeName,
                AssignedWorkOrderImage = r.AssignedWorkOrderImage,
                ProfileImage = r.ProfileImage,
                CodeID = r.CodeID,
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                AssignedTime = r.AssignedTime,
                // AssetName = r.QRCName
            }).ToList();
        }
        /// <summary>
        /// TO ACCEPT WORKORDER BY CLIENT ITSELF
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>4-9-2015</CreatedDate>
        /// <param name="WorOrderID"></param>
        /// <param name="iUserId"></param>
        /// <returns></returns>
        public Result AcceptWorkOrderByEmployee(long WorOrderID, long iUserId)
        {
            try
            {
                using (TransactionScope TransScope = new TransactionScope())
                {
                    ObjWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
                    WorkRequestAssignment obj_WorkRequestAssignment = ObjWorkRequestAssignmentRepository.GetAll(x => x.WorkRequestAssignmentID == WorOrderID && x.IsDeleted == false).FirstOrDefault();
                    if (obj_WorkRequestAssignment.AssignToUserId == null)
                    {
                        obj_WorkRequestAssignment.AssignToUserId = iUserId;
                        obj_WorkRequestAssignment.ModifiedBy = iUserId;
                        obj_WorkRequestAssignment.ModifiedDate = DateTime.UtcNow;
                        ObjWorkRequestAssignmentRepository.SaveChanges();
                        TransScope.Complete();
                        return Result.Completed;
                    }
                    else { TransScope.Dispose(); return Result.DuplicateRecord; }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// TO COMPLETE WORKORDER BY CLIENT 
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>4-14-2015</CreatedDate>
        /// <param name="WorOrderID"></param>
        /// <param name="iUserId"></param>
        /// <returns></returns>
        public Result StartWorkOrderByEmployee(long WorOrderID, long iUserId, string StartTime)
        {
            try
            {
                using (TransactionScope TransScope = new TransactionScope())
                {
                    ObjWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
                    WorkRequestAssignment obj_WorkRequestAssignment = ObjWorkRequestAssignmentRepository.GetAll(x => x.WorkRequestAssignmentID == WorOrderID && x.IsDeleted == false).FirstOrDefault();
                    if (obj_WorkRequestAssignment != null)
                    {
                        obj_WorkRequestAssignment.WorkRequestStatus = 15; //TO SET IN PROGRESS STATUS OF ASSIGNMENT
                        obj_WorkRequestAssignment.StartTime = DateTime.UtcNow;//Convert.ToDateTime(StartTime);//Commented by Bhushan for incorrect datetime from browser date
                        obj_WorkRequestAssignment.ModifiedBy = iUserId;
                        obj_WorkRequestAssignment.ModifiedDate = DateTime.UtcNow;
                        ObjWorkRequestAssignmentRepository.SaveChanges();
                        TransScope.Complete();
                        return Result.UpdatedSuccessfully;
                    }
                    else { TransScope.Dispose(); return Result.DoesNotExist; }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// TO COMPLETE WORKORDER BY CLIENT 
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>4-14-2015</CreatedDate>
        /// <param name="WorOrderID"></param>
        /// <param name="iUserId"></param>
        /// <returns></returns>
        public Result CompleteWorkOrderByEmployee(long WorOrderID, long iUserId, string EndTime)
        {
            try
            {
                using (TransactionScope TransScope = new TransactionScope())
                {
                    ObjWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
                    WorkRequestAssignment obj_WorkRequestAssignment = ObjWorkRequestAssignmentRepository.GetAll(x => x.WorkRequestAssignmentID == WorOrderID && x.IsDeleted == false).FirstOrDefault();
                    if (obj_WorkRequestAssignment != null)
                    {
                        obj_WorkRequestAssignment.WorkRequestStatus = 16;  //TO SET COMPLETE STATUS OF ASSIGNMENT
                        obj_WorkRequestAssignment.EndTime = DateTime.UtcNow;//Convert.ToDateTime(EndTime);//Commented by Bhushan for incorrect datetime from browser date
                        obj_WorkRequestAssignment.ModifiedBy = iUserId;
                        obj_WorkRequestAssignment.ModifiedDate = DateTime.UtcNow;
                        ObjWorkRequestAssignmentRepository.SaveChanges();
                        TransScope.Complete();
                        return Result.UpdatedSuccessfully;
                    }
                    else { TransScope.Dispose(); return Result.DoesNotExist; }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// TO GET EMPLOYEE DSHBOARD COUNT
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>4-14-2015</CreatedDate>
        /// <param name="UserId"></param>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        public List<EmployeeWorkAssignmentCountModel> GetEmployeeTotalWorkStatus(long UserId, long LocationId)
        {
            ObjWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
            return ObjWorkRequestAssignmentRepository.GetEmployeeTotalWorkStatus(UserId, LocationId);
        }

        /// <summary>Update survey and disclaimer name
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>Disclaimer&Survey</CreatedFor>
        /// <CreatedOn>May-21-2015</CreatedOn>
        /// </summary>
        /// <param name="long WorkAssignmentID, string ImageName, string ImageNameEmp, string DisclaimerName, string SurveyName"></param>
        /// <returns></returns>
        public bool WorkFrSignature(long WorkAssignmentID, string ImageName, string ImageNameEmp, string DisclaimerName, string SurveyName, string SurveyEmailID)
        {
            bool status = false;
            try
            {
                ObjWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();

                var workDetails = ObjWorkRequestAssignmentRepository.GetSingleOrDefault(t => t.WorkRequestAssignmentID == WorkAssignmentID && t.IsDeleted == false);
                if (workDetails != null && workDetails.AssignToUserId != null && workDetails.StartTime != null
                        && DisclaimerName != null && DisclaimerName.Trim() != ""
                        && ImageName.Trim() != "" && ImageName != null
                        && ImageNameEmp.Trim() != "" && ImageNameEmp != null)
                {
                    workDetails.SignatureImage = ImageName;
                    workDetails.FREmployeeImage = ImageNameEmp;
                    workDetails.DisclaimerForm = DisclaimerName;
                    workDetails.WorkRequestStatus = 16;//Here hardcoded value due to after signature status should be complete
                    workDetails.EndTime = DateTime.UtcNow;
                    workDetails.FrDisclaimerStatus = false;
                    ObjWorkRequestAssignmentRepository.SaveChanges();
                    status = true;
                }
                if (workDetails != null && workDetails.SurveyForm == null && SurveyName != null && SurveyName.Trim() != "" && SurveyEmailID != null && SurveyEmailID.Trim() != "")
                {
                    workDetails.SurveyForm = SurveyName;
                    workDetails.SurveyEmailID = SurveyEmailID;
                    ObjWorkRequestAssignmentRepository.SaveChanges();
                    status = true;
                }
                return status;
            }
            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "bool WorkFrSignature(long WorkAssignmentID, string ImageName, string ImageNameEmp, string DisclaimerName, string SurveyName)", "While facility signature ", WorkAssignmentID);
                throw;
            }
        }

        /// <summary>Decline or accept facility decline  status
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>Decline Status</CreatedFor>
        /// <CreatedOn>May-21-2015</CreatedOn>
        /// </summary>
        /// <param name="long WorkAssignmentID, long UserId,bool IsDecline"></param>
        /// <returns></returns>
        public bool WorkFrIsDecline(long WorkAssignmentID, long UserId, bool IsDecline)
        {
            bool status = false;
            try
            {
                ObjWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();

                var workDetails = ObjWorkRequestAssignmentRepository.GetSingleOrDefault(t => t.WorkRequestAssignmentID == WorkAssignmentID && t.IsDeleted == false);

                if (workDetails != null && workDetails.WorkRequestAssignmentID > 0)
                {
                    workDetails.FrDisclaimerStatus = IsDecline;
                    workDetails.ModifiedBy = UserId;
                    workDetails.ModifiedDate = DateTime.UtcNow;
                    workDetails.WorkRequestStatus = 341;//Globalcode for Decline workstatus

                    ObjWorkRequestAssignmentRepository.SaveChanges();
                    status = true;
                }
                return status;
            }
            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, " bool WorkFrIsDecline(long WorkAssignmentID, long UserId,bool IsDecline)", "Decline/Accept of facility request", UserId);
                throw;
            }
        }

        /// <summary>Send Email to employee's email id
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>FeedbackEmailToEmployee</CreatedFor>
        /// <CreatedOn>May-21-2015</CreatedOn>
        /// </summary>
        /// <param name="objServiceQrcVehicleModel"></param>
        /// <returns></returns>
        public bool FeedbackEmailToEmployee(ServiceFedbackModel obj)
        {
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            EmailLog objEmailog;
            EmailLogRepository objEmailLogRepository = new EmailLogRepository();
            bool IsSent;
            try
            {

                EmailHelper objEmailHelper = new EmailHelper();
                objEmailHelper.emailid = obj.Email;
                objEmailHelper.UserName = obj.UserName;
                objEmailHelper.MailType = "SURVEYFEEDBACK";
                objEmailHelper.SentBy = obj.UserId;
                objEmailHelper.LocationID = obj.LocationId;
                objEmailHelper.WorkRequestAssignmentID = obj.WorkAssignmentID;

                string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"]);
                HostingPrefix = HostingPrefix + "eMaintenanceDisclaimer/SurveyFeedback?user=" + WebUtility.HtmlEncode(Cryptography.GetEncryptedData(obj.UserId.ToString(), true)) + "&work=" + WebUtility.HtmlEncode(Cryptography.GetEncryptedData(obj.WorkAssignmentID.ToString(), true)) + "&email=" + WebUtility.HtmlEncode(Cryptography.GetEncryptedData(obj.Email.ToString(), true));
                objEmailHelper.RegistrationLink = HostingPrefix;

                IsSent = objEmailHelper.SendEmailWithTemplate();
                if (IsSent == true)
                {
                    objEmailog = new EmailLog();
                    try
                    {
                        objEmailog.CreatedBy = obj.UserId;
                        objEmailog.CreatedDate = DateTime.UtcNow;
                        objEmailog.DeletedBy = null;
                        objEmailog.DeletedOn = null;
                        objEmailog.LocationId = obj.LocationId;
                        objEmailog.ModifiedBy = null;
                        objEmailog.ModifiedOn = null;
                        objEmailog.SentBy = obj.UserId;
                        objEmailog.SentEmail = obj.Email;
                        objEmailog.Subject = objEmailHelper.Subject;
                        objEmailog.SentTo = obj.UserId;
                        objEmailLogRepository.SaveEmailLog(objEmailog);
                    }
                    catch (Exception ex)
                    {
                        WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "bool FeedbackEmailToEmployee(ServiceFedbackModel obj)", "while sending survey", obj.UserId);
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return IsSent;
        }

        /// <summary>
        /// TO GET FAcility Request  BY ID
        /// </summary>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedDate>04 June 2015</CreatedDate>
        /// <param name="WorkRequestAssignmentID"></param>
        /// <returns></returns>
        public bool GetFacilityRequestByID(long WorkRequestAssignmentID, long LocationId, long UserId)
        {
            bool status = false;
            try
            {
                ObjWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
                GlobalAdminManager ObjManagerList = new GlobalAdminManager();
                WorkRequestAssignmentModel obj = new WorkRequestAssignmentModel();
                List<listForEmployeeDevice> objEmailReturn = new List<listForEmployeeDevice>();
                EmailHelper objEmailHelper = new EmailHelper();
                var t = ObjWorkRequestAssignmentRepository.GetAll(v => v.WorkRequestAssignmentID == WorkRequestAssignmentID && v.IsDeleted == false)
                                                          .FirstOrDefault();
                if (t.AssignToUserId == null)
                {
                    objEmailLogRepository = new EmailLogRepository();
                    objEmailReturn = ObjManagerList.send30SecFRNotificaitonToAllManager(LocationId, UserId);
                    objEmailHelper.WorkOrderCodeId = t.WorkOrderCode + t.WorkOrderCodeID.ToString();
                    objEmailHelper.FacilityRequestType = t.FacilityRequestId.ToString();
                    objEmailHelper.FacilityRequestType = ObjCommonMethodManager.GetGlobalCodeDetailById(Convert.ToInt64(objEmailHelper.FacilityRequestType));
                    objEmailHelper.FrCustomerName = t.CustomerName;
                    objEmailHelper.FrCurrentLocation = t.CurrentLocation;
                    objEmailHelper.FrDriverLicenseNo = t.DriverLicenseNo;
                    objEmailHelper.FrCustomerContact = t.CustomerContact;
                    objEmailHelper.FrVehicleYear = t.VehicleYear.ToString();
                    objEmailHelper.FrVehicleColor = t.VehicleColor;
                    objEmailHelper.FrVehicleMake = t.VehicleMake;
                    objEmailHelper.FrVehicleModel = t.VehicleModel;
                    objEmailHelper.FrAddress = t.Address;
                    objEmailHelper.MailType = "FACILITYREQUESTNOTACCEPTED";

                    foreach (var item in objEmailReturn)
                    {
                        objEmailHelper.LocationName = item.LocationName;
                        if (item.DeviceId != null && item.DeviceId.Trim() != "")
                        {
                            message = PushNotificationMessages.FacilityRequestIdle(item.LocationName, objEmailHelper.WorkOrderCodeId);
                            PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
                        }
                    }

                    status = true;
                }
                else
                {
                    status = false;
                }
                return status;
            }
            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "bool GetFacilityRequestByID(long WorkRequestAssignmentID, long LocationId, long UserId)", "while sending facility request not accepted", UserId);
                throw;
            }
        }

        /// <summary>
        /// Created by :Bhushan Dod 
        /// Created Date :09/06/2015
        /// Alert to employee for continuous request time start now
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CREmployeeAlert(long locationId, long userId, string TimeZoneName, long TimeZoneOffset, bool IsTimeZoneinDaylight)
        {
            bool result = false;
            using (workorderEMSEntities Context = new workorderEMSEntities())
            {
                try
                {
                    EmailHelper objEmailHelper = new EmailHelper();
                   var time = DateTime.UtcNow;//Convert.ToDateTime("2015-06-07 09:35:00.000"); //DateTime.Now;
                    var workDetails = Context.WorkRequestAssignments.Join(Context.UserRegistrations, u => u.AssignToUserId, s => s.UserId, (u, s) => new { u, s }).Where(x => x.u.AssignToUserId == userId
                                                                            && (time.Date >= x.u.StartDate && time.Date <= x.u.EndDate)
                                                                            && x.u.WorkRequestProjectType == 279
                                                                            && x.u.IsDeleted == false
                                                                            ).ToList();
                    foreach (var d in workDetails)
                    {
                        objEmailHelper.MailType = "CONTINIOUSREQUESTREMINDER";
                        objEmailHelper.WorkRequestAssignmentID = d.u.WorkRequestAssignmentID;
                        //objEmailHelper.StartDate = d.u.StartDate.ToString("MM'/'dd'/'yyyy");
                        if (d.u.StartDate != null)
                        {
                            //objEmailHelper.StartDate = d.u.StartDate.Value.ToMobileClientTimeZoneinDateTime(TimeZoneName,TimeZoneOffset,IsTimeZoneinDaylight).ToString("MM'/'dd'/'yyyy");
                            objEmailHelper.StartDate = d.u.StartDate.ToString("MM'/'dd'/'yyyy");
                        }
                        //objEmailHelper.EndDate = d.u.EndDate.ToString("MM'/'dd'/'yyyy");
                        if (d.u.EndDate != null)
                        {
                            objEmailHelper.EndDate = d.u.EndDate.ToString("MM'/'dd'/'yyyy");
                            //objEmailHelper.EndDate = d.u.EndDate.Value.ToMobileClientTimeZoneinDateTime(TimeZoneName, TimeZoneOffset, IsTimeZoneinDaylight).ToString("MM'/'dd'/'yyyy"); ;
                        }
                        objEmailHelper.WeekDays = d.u.WeekDaysName;
                        //objEmailHelper.StartTime = d.u.StartTime.ToString("hh:mm tt");
                        //objEmailHelper.AssignedTime = d.u.AssignedTime.ToString("HH:mm");
                        if (d.u.ConStartTime != null)
                        {
                            objEmailHelper.StartTime = d.u.ConStartTime.Value.GetClientDateTimeForMobileNow(TimeZoneName).ToString("hh:mm tt");
                            //objEmailHelper.StartTime = d.u.ConStartTime.Value.ToMobileClientTimeZoneinDateTime(TimeZoneName, TimeZoneOffset, IsTimeZoneinDaylight).ToString("hh:mm tt");
                        }
                        objEmailHelper.WorkOrderCodeId = d.u.WorkOrderCode + d.u.WorkOrderCodeID.ToString();
                        objEmailHelper.AssignedTime = d.u.AssignedTime.ToString("HH:mm");
                        objEmailHelper.ProjectDesc = d.u.ProjectDesc;
                        objEmailHelper.UserName = d.s.FirstName + ' ' + d.s.LastName;
                        objEmailHelper.emailid = d.s.UserEmail;
                        var dateList = d.u.WeekDays.Split(',').ToList();
                        foreach (var date in dateList)
                        {
                            if (d.u.ConStartTime != null)
                            {
                                if (date == time.ToShortDateString() && d.u.ConStartTime.Value.ToShortTimeString() == time.ToShortTimeString())
                                {
                                    if (d.s.DeviceId != null)
                                    {
                                        PushNotification.GCMAndroid("Your continuous task time arrived.", d.s.DeviceId, objEmailHelper);
                                    }
                                    result = true;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "dynamic CREmployeeAlert(long locationId, long userId)", "Send alert to employee for reminder of CR ", userId);
                    throw;
                }
            }
            return result;
        }

        public bool IsSurveySubmit(long WorkAssignmentID)
        {
            bool status = false;//by default status
            try
            {
                ObjWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();

                var workDetails = ObjWorkRequestAssignmentRepository.GetSingleOrDefault(t => t.WorkRequestAssignmentID == WorkAssignmentID && t.IsDeleted == false);
                if (workDetails != null && workDetails.SurveyForm != null && workDetails.SurveyForm.Trim() != "") //If exist in record need to save pdf that's why we are returning to false to savePDF.
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
                return status;
            }
            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "bool IsSurveySubmit(long WorkAssignmentID)", "from c# while WorkRequestmanager.cs", WorkAssignmentID);
                throw;
            }
        }

        /// <summary>Pause Resume Functionality of work order
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>Work Order Pause & Resume</CreatedFor>
        /// <CreatedOn>Oct-23-2015</CreatedOn>
        /// </summary>
        /// <param name="long WorkAssignmentID, long LocationID, long UserId, bool Status"></param>
        /// <returns></returns>
        public bool WorkOrderPauseResume(ServiceWorkOrderAcceptanceModel obj)
        {
            bool status = false;
            try
            {
                ObjWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
                ObjUserRepository = new UserRepository();

                var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == obj.ServiceAuthKey && x.IsDeleted == false);

                if (authuser != null && authuser.UserId > 0)
                {
                    var workDetails = ObjWorkRequestAssignmentRepository.GetSingleOrDefault(t => t.WorkRequestAssignmentID == obj.WorkRequestAssignmentID && t.LocationID == obj.LocationID && t.IsDeleted == false);

                    if (workDetails != null && workDetails.WorkRequestAssignmentID > 0)
                    {    //If first time not pause
                        if (workDetails.PauseStatus != null && workDetails.StartTime != null && workDetails.PauseTime != null)
                        {
                            // 329 - Pause , 330 - Resume
                            if (obj.Status == Convert.ToInt32(PauseResume.Pause))
                            {
                                workDetails.PauseStatus = obj.Status;
                                workDetails.PauseTime = DateTime.UtcNow;
                            }
                            else
                            {
                                workDetails.PauseStatus = obj.Status;//Resume
                                TimeSpan span = new TimeSpan();
                                //get the pause time
                                span = (DateTime.UtcNow - workDetails.PauseTime.Value);
                                //var t = String.Format("{0} days, {1} hours, {2} minutes, {3} seconds",
                                //    span.Days, span.Hours, span.Minutes, span.Seconds);
                                if (workDetails.TotalPauseTime == null)
                                {
                                    //Note : Default date bcoz we need to save day also.Scenario like if pause today and resume by tommorrow.
                                    //       Remind that when ever calculate TotalPauseTime if day is > 01 then minus 1 bcoz default set date day is 01. 
                                    DateTime dt = new DateTime(2015, 01, 01);
                                    //Here we save how much total time emp can pause
                                    workDetails.TotalPauseTime = dt.AddDays(span.Days).SetTime(span.Hours, span.Minutes, span.Seconds, span.Milliseconds); ;
                                }
                                else
                                {
                                    //Here we have added existing time + newly total time pause
                                    workDetails.TotalPauseTime = workDetails.TotalPauseTime.Value.AddDays(span.Days);
                                    workDetails.TotalPauseTime = workDetails.TotalPauseTime.Value.AddHours(span.Hours);
                                    workDetails.TotalPauseTime = workDetails.TotalPauseTime.Value.AddMinutes(span.Minutes);
                                    workDetails.TotalPauseTime = workDetails.TotalPauseTime.Value.AddSeconds(span.Seconds);
                                    workDetails.TotalPauseTime = workDetails.TotalPauseTime.Value.AddMilliseconds(span.Milliseconds);
                                }
                                workDetails.PauseTime = DateTime.UtcNow;
                            }
                        }
                        else
                        {
                            //If first time pause and TotalPauseTime is null
                            workDetails.PauseStatus = obj.Status;
                            workDetails.PauseTime = DateTime.UtcNow;//Employee when wo pause                      
                        }
                        workDetails.ModifiedBy = obj.UserId;
                        workDetails.ModifiedDate = DateTime.UtcNow;

                        ObjWorkRequestAssignmentRepository.SaveChanges();
                        status = true;
                    }

                    return status;
                }
                else
                {
                    return status;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ServiceWorkAssignmentModel UrgentWOAccpetedByEmployee(ServiceDARModel obj)
        {
            ObjUserRepository = new UserRepository();
            objDARRepository = new DARRepository();
            ServiceWorkAssignmentModel objServiceWorkAssignmentModel = new ServiceWorkAssignmentModel();
            try
            {
                if (obj.LocationId > 0 && obj.ServiceAuthKey != null)
                {
                    var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == obj.ServiceAuthKey && x.IsDeleted == false);

                    if (authuser != null && authuser.UserId > 0)
                    {
                        ObjWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
                        var workDetails = ObjWorkRequestAssignmentRepository.GetSingleOrDefault(t => t.WorkRequestAssignmentID == obj.WorkAssignmentID && t.LocationID == obj.LocationId && t.IsDeleted == false);
                        if (workDetails != null && workDetails.AssignToUserId == null)
                        {
                            workDetails.AssignToUserId = obj.UserId;
                            workDetails.AssignByUserId = obj.UserId;
                            workDetails.ModifiedBy = obj.UserId;
                            workDetails.ModifiedDate = DateTime.UtcNow;
                            ObjWorkRequestAssignmentRepository.SaveChanges();
                            obj.ActivityDetails = DarMessage.UrgentWOAcceptbyEmp(obj.UserName, obj.LocationName);

                            var result = objDARRepository.SaveDARDetails(obj);
                            if (result > 0)
                            {
                                //Modified by Bhushan on 31 March for after accpet all details of work request.
                                objServiceWorkAssignmentModel.WorkRequestAssignmentID = workDetails.WorkRequestAssignmentID;
                                objServiceWorkAssignmentModel.AssetID = workDetails.AssetID;
                                objServiceWorkAssignmentModel.ProblemDescription = workDetails.ProblemDesc;
                                objServiceWorkAssignmentModel.ProjectDescription = workDetails.ProjectDesc;
                                objServiceWorkAssignmentModel.WorkRequestProjectType = workDetails.WorkRequestProjectType;
                                objServiceWorkAssignmentModel.WorkrequestCode = workDetails.WorkOrderCode+ workDetails.WorkOrderCodeID;
                                objServiceWorkAssignmentModel.WorkRequestImage = (workDetails.AssignedWorkOrderImage != null)?ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/WorkRequest/" + workDetails.WorkRequestImage: HostingPrefix + ConstantImages.Replace("~", "") + "no-asset-pic.png";
                                objServiceWorkAssignmentModel.PriorityLevel = workDetails.PriorityLevel;
                                objServiceWorkAssignmentModel.WorkRequestStatus = workDetails.WorkRequestStatus;
                                objServiceWorkAssignmentModel.LocationID = workDetails.LocationID;
                                objServiceWorkAssignmentModel.PauseStatus = workDetails.PauseStatus;
                                objServiceWorkAssignmentModel.WorkRequestType = workDetails.WorkRequestType;
                                // obj = null;
                                objServiceWorkAssignmentModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                                //obj.DARId = result;
                                objServiceWorkAssignmentModel.ResponseMessage = CommonMessage.SaveSuccessMessage();
                            }
                            else
                            {
                                objServiceWorkAssignmentModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                                obj.DARId = result;
                                objServiceWorkAssignmentModel.ResponseMessage = CommonMessage.WrongParameterMessage();
                            }
                        }
                        else
                        {
                            objServiceWorkAssignmentModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                            objServiceWorkAssignmentModel.ResponseMessage = CommonMessage.AlreadyAcceptedUrgentWorkRequest();
                        }
                    }
                    else
                    {
                        objServiceWorkAssignmentModel.Response = Convert.ToInt32(ServiceResponse.InvalidSessionResponse, CultureInfo.CurrentCulture);
                        objServiceWorkAssignmentModel.ResponseMessage = CommonMessage.InvalidUser();
                    }
                }
                else
                {

                    objServiceWorkAssignmentModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    objServiceWorkAssignmentModel.ResponseMessage = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception)
            { throw; }
            return objServiceWorkAssignmentModel;
        }

        /// <summary>
        /// <CreatedBy>Ashwajit Bansod</CreatedBy>
        /// <CreatedOn>August-04-2017</CreatedOn>
        /// <CreatedFor>Checking for a Work order Assigned to User before deleting in List Form </CreatedFor>
        /// </summary>
        /// <returns></returns>
        public CheckWorkRequestforDeleteUser CheckingContinuousWorkRequestForUser(long userId)
        {
            CheckWorkRequestforDeleteUser objCheckWorkRequestforDeleteUser = new CheckWorkRequestforDeleteUser();
            using (workorderEMSEntities Context = new workorderEMSEntities())
            {
                try
                {
                    var time = DateTime.UtcNow;
                    //Added by ashwajit Bansod Fetching all the records of Continuous List as List
                    objCheckWorkRequestforDeleteUser.ContinuousList = Context.WorkRequestAssignments.Join(Context.UserRegistrations, u => u.AssignToUserId, s => s.UserId, (u, s) => new { u, s }).Where(x => x.u.AssignToUserId == userId
                                                                            && (time.Date >= x.u.StartDate && time.Date <= x.u.EndDate)
                                                                            && x.u.WorkRequestProjectType == 279
                                                                            && x.u.IsDeleted == false
                                                                            ).Select(a => new ServiceWorkAssignmentModel()
                                                                            {
                                                                                AssignToUserId = a.u.AssignToUserId,
                                                                                WorkOrderCodeID = a.u.WorkOrderCodeID,
                                                                                StartDate = a.u.StartDate.ToString(),
                                                                                EndDate = a.u.EndDate.ToString(),
                                                                                CreatedDate = a.u.CreatedDate.ToString(),
                                                                                ProjectDescription = a.u.ProjectDesc,
                                                                                WorkOrderCode = a.u.WorkOrderCode,
                                                                                LocationID = a.u.LocationID,
                                                                                WorkRequestProjectTypeName = a.u.GlobalCode.CodeName
                                                                            }).ToList();

                    objCheckWorkRequestforDeleteUser.NormalList = Context.WorkRequestAssignments.Join(Context.UserRegistrations, u => u.AssignToUserId, s => s.UserId, (u, s) => new { u, s }).Where(x => x.u.AssignToUserId == userId
                                                                                && (DbFunctions.TruncateTime(x.u.CreatedDate) == time.Date)
                                                                                && (x.u.WorkRequestStatus != 16)
                                                                                && (x.u.WorkRequestProjectType == 128 || x.u.WorkRequestProjectType == 129 || x.u.WorkRequestProjectType == 256)
                                                                                && x.u.IsDeleted == false
                                                                                ).Select(a => new ServiceWorkAssignmentModel()
                                                                                {
                                                                                    AssignToUserId = a.u.AssignToUserId,
                                                                                    CreatedDate = a.u.CreatedDate.ToString(),
                                                                                    AssetID = a.u.AssetID,
                                                                                    WorkRequestType = a.u.WorkRequestType,
                                                                                    WorkOrderCodeID = a.u.WorkOrderCodeID,
                                                                                    WorkRequestProjectType = a.u.WorkRequestProjectType,
                                                                                    ProblemDescription = a.u.ProblemDesc,
                                                                                    FacilityRequest = a.u.FacilityRequestId.ToString(),
                                                                                    CustomerName = a.u.CustomerName,
                                                                                    LocationID = a.u.LocationID,
                                                                                    WorkOrderCode = a.u.WorkOrderCode,
                                                                                    WorkRequestProjectTypeName = a.u.GlobalCode.CodeName
                                                                                }).ToList();
                }
                catch (Exception ex)
                {
                    WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "CheckWorkRequestforDeleteUser CheckingContinuousWorkRequestForUser(long userId)", "While deleting checking is user assigned work order or not", userId);
                    throw;
                }
            }
            return objCheckWorkRequestforDeleteUser;
        }
    }
}
