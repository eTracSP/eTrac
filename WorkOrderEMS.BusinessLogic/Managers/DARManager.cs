using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class DARManager : IDARManager
    {
        DARRepository objDARRepository;
        UserRepository ObjUserRepository;
        WorkRequestAssignmentRepository objWorkRequestAssignmentRepository;
        WorkRequestAssignmentModel objWork;
        /// <summary>GetDARDetails
        /// <CreatedBy>Roshan Rahood</CreatedBy>
        /// <CreatedFor>Get DAR Details List</CreatedFor>
        /// <CreatedOn>Nov-14-2014</CreatedOn>
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="PageIndex"></param>
        /// <param name="NumberOfRows"></param>
        /// <param name="SortColumnName"></param>
        /// <param name="SortOrderBy"></param>
        /// <param name="SearchText"></param>
        /// <returns></returns>
        public List<DARModelList> GetDARDetails(long? LoginUserId, long? locationId, long? userId, int? taskType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords, string fromDate = null, string toDate = null)
        {
            try
            {
                objDARRepository = new DARRepository();
                return objDARRepository.GetDARDetails(LoginUserId, locationId, userId, taskType, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, totalRecords, fromDate, toDate);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>Save DAR for QRC Type
        /// <CreatedFor>For Insert QRC Type</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Feb-13-2015</CreatedOn>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns> 
        public ServiceDARModel SaveDARDetails(ServiceDARModel obj)
        {
            ObjUserRepository = new UserRepository();
            objDARRepository = new DARRepository();
            try
            {
                if (obj.LocationId > 0 && obj.ServiceAuthKey != null)
                {
                    var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == obj.ServiceAuthKey && x.IsDeleted == false);

                    if (authuser != null && authuser.UserId > 0 && obj.TaskType != 280)
                    {
                        //obj.ActivityDetails = DarMessage.QrcVehicleCleaning(obj.UserName);
                        var result = objDARRepository.SaveDARDetails(obj);
                        if (result != null && result > 0)
                        {
                            obj.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                            obj.DARId = result;
                            obj.ResponseMessage = CommonMessage.SaveSuccessMessage();
                        }
                        else
                        {
                            obj.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                            obj.DARId = result;
                            obj.ResponseMessage = CommonMessage.WrongParameterMessage();
                        }
                    }
                    else if (authuser != null && authuser.UserId > 0 && obj.TaskType == 280)
                    {
                        obj.ActivityDetails = DarMessage.ShiftEnd(obj.UserName);
                        var result = objDARRepository.SaveDARDetails(obj);

                        if (result != null && result > 0)
                        {
                            var data = ObjUserRepository.GetSingleOrDefault(v => v.UserId == obj.UserId && v.IsDeleted == false);
                            if (data != null)
                            {
                                data.ModifiedBy = obj.UserId;
                                data.ModifiedDate = DateTime.UtcNow;
                                data.IdleTimeLimit = Convert.ToDateTime("00:30");
                                ObjUserRepository.Update(data);

                                obj.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                                obj.DARId = result;
                                obj.ResponseMessage = CommonMessage.SaveSuccessMessage();
                            }

                        }
                        else
                        {
                            obj.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                            obj.DARId = result;
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

        /// <summary>Save DAR for Jump Start
        /// <CreatedFor>For Insert Jump Start and GT Tracker</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>March-16-2015</CreatedOn>
        /// </summary>
        /// <param name="objServiceDARModel"></param>
        /// <returns></returns> 
        public ServiceDARModel SaveDARDetailsForTracking(ServiceDARModel obj)
        {
            ObjUserRepository = new UserRepository();
            objDARRepository = new DARRepository();
            try
            {
                if (obj.LocationId > 0 && obj.ServiceAuthKey != null)
                {
                    var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == obj.ServiceAuthKey && x.IsDeleted == false);

                    if (authuser != null && authuser.UserId > 0)
                    {
                        //THIS SECTION FOR ACCEPTING THE FACILITY REQUEST
                        if (obj.FacilityRequest == true)
                        {
                            objWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
                            objWork = new WorkRequestAssignmentModel();

                            var workDetails = objWorkRequestAssignmentRepository.GetSingleOrDefault(t => t.WorkRequestAssignmentID == obj.WorkAssignmentID && t.LocationID == obj.LocationId && t.IsDeleted == false);
                            if (workDetails != null && workDetails.AssignToUserId == null && workDetails.StartTime == null)
                            {
                                workDetails.AssignToUserId = obj.UserId;
                                workDetails.AssignByUserId = obj.UserId;
                                workDetails.StartTime = DateTime.UtcNow;
                                workDetails.ModifiedBy = obj.UserId;
                                workDetails.ModifiedDate = DateTime.UtcNow;
                                workDetails.WorkRequestStatus = 15;
                                objWorkRequestAssignmentRepository.SaveChanges();
                                obj.ActivityDetails = DarMessage.FacilityRequestAccept(obj.UserName, obj.LocationName);
                                obj.StartTime = DateTime.UtcNow.ToString();

                                var result = objDARRepository.InsertDARDetailsForTracking(obj);
                                if (result != null && result > 0)
                                {
                                    // obj = null;
                                    obj.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                                    obj.DARId = result;
                                    obj.ResponseMessage = CommonMessage.SaveSuccessMessage();
                                }
                                else
                                {
                                    obj.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                                    obj.DARId = result;
                                    obj.ResponseMessage = CommonMessage.WrongParameterMessage();
                                }
                            }
                            else
                            {
                                obj.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                                obj.ResponseMessage = CommonMessage.AlreadyAcceptedFacilityRequest();
                            }
                        }
                        //THIS SECTION FOR DAR ENTRY FOR CUSTOMER CALL
                        if (obj.FacilityRequest == false && obj.StartTime != null && obj.EndTime != null && obj.TaskType == (long)DARTASKTYPECATEGORY.CustomerCall)
                        {
                            if (obj.StartTime != null)
                            {
                                obj.StartTime = DateTime.UtcNow.ToString();
                            }
                            if (obj.EndTime != null)
                            {
                                obj.EndTime = DateTime.UtcNow.ToString();
                            }
                            obj.ActivityDetails = DarMessage.DarCustomerCall(obj.UserName, obj.LocationName, obj.Description);
                            var result = objDARRepository.InsertDARDetailsForTracking(obj);
                            if (result != null && result > 0)
                            {
                                // obj = null;
                                obj.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                                obj.DARId = result;
                                obj.ResponseMessage = CommonMessage.SaveSuccessMessage();
                            }
                            else
                            {
                                obj.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                                obj.DARId = result;
                                obj.ResponseMessage = CommonMessage.WrongParameterMessage();
                            }
                        }
                        //THIS SECTION FOR DAR ENTRY JUMP START AND END TIME
                        if (obj.FacilityRequest == false && obj.StartTime != null && obj.TaskType == (long)DARTASKTYPECATEGORY.DARType)
                        {
                            if (obj.StartTime != null)
                            {
                                obj.StartTime = DateTime.UtcNow.ToString();
                            }

                            obj.ActivityDetails = DarMessage.DarJumpStartEndTimeStatus(obj.UserName, obj.LocationName, obj.Description);
                            var result = objDARRepository.InsertDARDetailsForTracking(obj);
                            if (result != null && result > 0)
                            {
                                // obj = null;
                                obj.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                                obj.DARId = result;
                                obj.ResponseMessage = CommonMessage.SaveSuccessMessage();
                            }
                            else
                            {
                                obj.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                                obj.DARId = result;
                                obj.ResponseMessage = CommonMessage.WrongParameterMessage();
                            }
                        }
                        //THIS SECTION FOR DAR ENTRY FOR 18 TO 33 CODE
                        if (obj.FacilityRequest == false && obj.StartTime != null && obj.TaskType == (int)DARTASKTYPECATEGORY.GateRepair
                            || obj.TaskType == (int)DARTASKTYPECATEGORY.CustomerVehicleLocate
                            || obj.TaskType == (int)DARTASKTYPECATEGORY.CustomerJumpStart
                            || obj.TaskType == (int)DARTASKTYPECATEGORY.Customertireinflation
                            || obj.TaskType == (int)DARTASKTYPECATEGORY.CustomerAssistance
                            || obj.TaskType == (int)DARTASKTYPECATEGORY.WorkBreak
                            || obj.TaskType == (int)DARTASKTYPECATEGORY.SpecialProject
                            || obj.TaskType == (int)DARTASKTYPECATEGORY.RoutineChecks
                            || obj.TaskType == (int)DARTASKTYPECATEGORY.SpaceCount
                            || obj.TaskType == (int)DARTASKTYPECATEGORY.LicensePlateInventory
                            || obj.TaskType == (int)DARTASKTYPECATEGORY.Emergency
                            || obj.TaskType == (int)DARTASKTYPECATEGORY.Facilitycleaning
                            || obj.TaskType == (int)DARTASKTYPECATEGORY.FacilitySpillResponse
                            || obj.TaskType == (int)DARTASKTYPECATEGORY.SnowRemoval
                            || obj.TaskType == (int)DARTASKTYPECATEGORY.TicketSpitterRepair
                            || obj.TaskType == (int)DARTASKTYPECATEGORY.MiscellaneousEvent)
                        {
                            if (obj.StartTime != null)
                            {
                                obj.StartTime = DateTime.UtcNow.ToString();
                            }

                            //obj.ActivityDetails = DarMessage.DarJumpStartEndTimeStatus(obj.UserName, obj.LocationName, obj.Description);
                            var result = objDARRepository.InsertDARDetailsForTracking(obj);
                            if (result != null && result > 0)
                            {
                                // obj = null;
                                obj.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                                obj.DARId = result;
                                obj.ResponseMessage = CommonMessage.SaveSuccessMessage();
                            }
                            else
                            {
                                obj.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                                obj.DARId = result;
                                obj.ResponseMessage = CommonMessage.WrongParameterMessage();
                            }
                        }
                        //THIS SECTION FOR EMPLOYEE STARTED CR FOR THE DAY AND IF STARTDATE = workDetails.StartDate then update WorkRequestStatus
                        if (obj.FacilityRequest == false && obj.StartTime != null && obj.EndTime == null && obj.TaskType == (long)DARTASKTYPECATEGORY.ContinuousRequestEnd)
                        {
                            if (obj.StartTime != null)
                            {
                                obj.StartTime = DateTime.UtcNow.ToString();
                            }

                            objWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
                            objWork = new WorkRequestAssignmentModel();

                            var workDetails = objWorkRequestAssignmentRepository.GetSingleOrDefault(t => t.WorkRequestAssignmentID == obj.WorkAssignmentID && t.LocationID == obj.LocationId && t.IsDeleted == false);

                            if (workDetails != null && workDetails.StartDate != null)
                            {
                                //Not sure about DateTimeNow bcoz if in future change UTC then StartDate save according to UTC
                                if (DateTime.UtcNow.ToShortDateString() == workDetails.StartDate.Value.ToShortDateString())
                                {
                                    workDetails.WorkRequestStatus = 15;
                                    workDetails.StartTime = DateTime.UtcNow;
                                    workDetails.ModifiedBy = obj.UserId;
                                    workDetails.ModifiedDate = DateTime.UtcNow; ;
                                    objWorkRequestAssignmentRepository.SaveChanges();
                                }

                                obj.ActivityDetails = DarMessage.CRTaskStart(obj.UserName, obj.LocationName);
                                var result = objDARRepository.InsertDARDetailsForTracking(obj);
                                if (result != null && result > 0)
                                {
                                    // obj = null;
                                    obj.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                                    obj.DARId = result;
                                    obj.ResponseMessage = CommonMessage.SaveSuccessMessage();
                                }
                                else
                                {
                                    obj.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                                    obj.DARId = result;
                                    obj.ResponseMessage = CommonMessage.WrongParameterMessage();
                                }
                            }
                            else
                            {
                                obj.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                                obj.ResponseMessage = CommonMessage.AlreadyAcceptedFacilityRequest();
                            }
                        }
                        //THIS SECTION FOR EMPLOYEE ENDED CR FOR THE DAY AND IF ENDDATE = workDetails.EndDate then update WorkRequestStatus
                        if (obj.FacilityRequest == false && obj.EndTime != null && obj.TaskType == (long)DARTASKTYPECATEGORY.ContinuousRequestEnd)
                        {
                            if (obj.EndTime != null)
                            {
                                obj.EndTime = DateTime.UtcNow.ToString();
                            }

                            objWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
                            objWork = new WorkRequestAssignmentModel();

                            var workDetails = objWorkRequestAssignmentRepository.GetSingleOrDefault(t => t.WorkRequestAssignmentID == obj.WorkAssignmentID && t.LocationID == obj.LocationId && t.IsDeleted == false);

                            if (workDetails != null && workDetails.EndDate != null)
                            {
                                //Not sure about DateTimeNow bcoz if in future change UTC then StartDate save according to UTC
                                if (DateTime.UtcNow.ToShortDateString() == workDetails.EndDate.Value.ToShortDateString())
                                {
                                    workDetails.WorkRequestStatus = 16;
                                    workDetails.EndTime = DateTime.UtcNow;
                                    workDetails.ModifiedBy = obj.UserId;
                                    workDetails.ModifiedDate = DateTime.UtcNow; ;
                                    objWorkRequestAssignmentRepository.SaveChanges();
                                }

                                obj.ActivityDetails = DarMessage.CRTaskEnd(obj.UserName, obj.LocationName);
                                var result = objDARRepository.InsertDARDetailsForTracking(obj);
                                if (result != null && result > 0)
                                {
                                    // obj = null;
                                    obj.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                                    obj.DARId = result;
                                    obj.ResponseMessage = CommonMessage.SaveSuccessMessage();
                                }
                                else
                                {
                                    obj.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                                    obj.DARId = result;
                                    obj.ResponseMessage = CommonMessage.WrongParameterMessage();
                                }
                            }
                            else
                            {
                                obj.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                                obj.ResponseMessage = CommonMessage.AlreadyAcceptedFacilityRequest();
                            }
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

        /// <summary>Save the Log while deleting mapping 
        /// <CreatedFor>For Insert QRC Type</CreatedFor>
        /// <CreatedBy>Vijay sahu</CreatedBy>
        /// <CreatedOn>march-11-2015</CreatedOn>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns> 
        public ServiceDARModel UserLocationMappingDelete(ServiceDARModel obj, string locationname, string userType)
        {


            ObjUserRepository = new UserRepository();
            objDARRepository = new DARRepository();
            try
            {
                string createdByName = "";
                if (obj.LocationId > 0)
                {

                    using (workorderEMSEntities objContext = new workorderEMSEntities())
                    {


                        obj.UserName = (from o in objContext.UserRegistrations
                                        where o.UserId == obj.UserId && o.IsDeleted == false
                                        select o.FirstName + "" + o.LastName).FirstOrDefault();


                        createdByName = (from o in objContext.UserRegistrations
                                         where o.UserId == obj.CreatedBy && o.IsDeleted == false
                                         select o.FirstName + "" + o.LastName).FirstOrDefault();

                    }



                    obj.ActivityDetails = DarMessage.DeleteLocationMapping(obj.UserName, locationname, userType, createdByName); // this will generate an message 
                    var result = objDARRepository.SaveDARDetails(obj);
                    if (result != null || result > 0)
                    {
                        obj.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        obj.DARId = result;
                        obj.ResponseMessage = CommonMessage.SaveSuccessMessage();
                    }
                    else
                    {
                        obj.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                        obj.DARId = result;
                        obj.ResponseMessage = CommonMessage.WrongParameterMessage();
                    }

                }
                else
                {

                    obj.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    obj.ResponseMessage = CommonMessage.WrongParameterMessage();
                }
            }
            catch (Exception ex)
            {

                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ServiceDARModel UserLocationMappingDelete(ServiceDARModel obj, string locationname, string userType)", "Exception While creating location", obj);
                throw ex;
            }
            return obj;
        }

        /// <summary>Get task list by employee id
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedOn>Jan-12-2015</CreatedOn>
        /// <CreatedFor> GetTaskListByEmpID</CreatedFor>
        /// </summary>
        /// <param name="ServiceAuthKey"></param>
        /// <returns></returns>
        public ServiceResponseModel<List<ServiceDARListModel>> GetAllDARDetails(ServiceDARListModel obj)
        {
            DARRepository _DARRepository = new DARRepository();
            ServiceResponseModel<List<ServiceDARListModel>> lstDAR = new ServiceResponseModel<List<ServiceDARListModel>>();
            try
            {
                ObjUserRepository = new UserRepository();
                var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == obj.ServiceAuthKey && x.IsDeleted == false);
                if (authuser != null && authuser.UserId > 0)
                {
                    if (string.IsNullOrEmpty(obj.FromDate) || string.IsNullOrEmpty(obj.ToDate))
                    {
                        if (string.IsNullOrEmpty(obj.FromDate))
                            obj.FromDate = DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                        if (string.IsNullOrEmpty(obj.ToDate))
                            obj.ToDate = DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    }
                    var result = _DARRepository.GetALLDARDetailsForMobile(obj.UserId, obj.TaskType, obj.LocationId,
                                                            Convert.ToDateTime(obj.FromDate, CultureInfo.InvariantCulture),
                                                            Convert.ToDateTime(obj.ToDate, CultureInfo.InvariantCulture)).Select(t => new ServiceDARListModel()
                                                            {
                                                                DARId = t.DARId,
                                                                Activity_Details = t.Activity_Details,
                                                                CreatedOn = t.CreatedOn,
                                                                Employee_Name = t.Employee_Name,
                                                                Location_Name = t.Location_Name,
                                                                TaskTypeDetails = t.TaskTypeDetails
                                                            }).ToList();
                    if (result != null || result.Count > 0)
                    {
                        lstDAR.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        lstDAR.Data = result;
                        lstDAR.Message = CommonMessage.Successful();
                    }
                    else
                    {
                        lstDAR.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                        lstDAR.Message = CommonMessage.NoRecordMessage();
                    }
                }
                else
                {

                    lstDAR.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    lstDAR.Message = CommonMessage.InvalidUser();
                }


                return lstDAR;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DARModel GetDARById(long? darId)
        {
            DARDetail objDARModel = new DARDetail();
            DARModel objDARModelList = new DARModel();
            objDARRepository = new DARRepository();
            try
            {
                objDARModel = objDARRepository.GetSingleOrDefault(x => x.DARId == darId && x.IsDeleted == false);
                if (objDARModel != null)
                {
                    objDARModelList.DARId = objDARModel.DARId;
                    objDARModelList.ActivityDetails = objDARModel.ActivityDetails;
                    objDARModelList.CreatedOn = objDARModel.CreatedOn.ToClientTimeZoneinDateTime();
                }

                return objDARModelList;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public Result UpdateDAR(DARModel objDARModel)
        {
            DARDetail objDARDetail;
            try
            {
                objDARRepository = new DARRepository();
                objDARDetail = new DARDetail();
                if (objDARModel.DARId > 0)
                {
                    objDARDetail = objDARRepository.GetAll(x => x.IsDeleted == false &&
                        x.DARId == objDARModel.DARId).FirstOrDefault();

                    if (objDARDetail != null)
                    {
                        objDARDetail.ActivityDetails = objDARModel.ActivityDetails;
                        objDARRepository.Update(objDARDetail);

                        return Result.Completed;
                    }
                    else
                    {
                        return Result.Failed;
                    }
                }
                return Result.Failed;
            }
            catch (Exception)
            {
                throw;

            }
        }

        /// <summary>Update the status of DAR jump start
        /// <CreatedFor>For Update Jump Start and GT Tracker</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>March-16-2015</CreatedOn>
        /// </summary>
        /// <param name="objServiceDARModel"></param>
        /// <returns></returns> 
        public ServiceResponseModel<string> UpdateDarTaskStatus(ServiceDARModel obj)
        {
            ObjUserRepository = new UserRepository();
            ServiceResponseModel<string> ObjServiceResponseModel = new ServiceResponseModel<string>();
            objDARRepository = new DARRepository();
            string message = string.Empty;
            try
            {
                // var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == ObjWorkStatusModel.ServiceAuthKey && x.UserId == ObjWorkStatusModel.UserId);
                if (obj.DARId != 0 && obj.EndTime != null && obj.EndTime.Trim() != "")
                {
                    if (obj.EndTime != null)
                    {
                        obj.EndTime = DateTime.UtcNow.ToString();
                    }
                    var result = objDARRepository.UpdateDarTaskStatus(obj.ServiceAuthKey, obj.UserId, obj.DARId, obj.EndTime, obj.LocationId, obj.Description, obj.EndTimeImage);
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

        /// <summary>Save Disclaimer DAR 
        /// <CreatedFor>For Insert QRC Type</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Feb-13-2015</CreatedOn>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns> 
        public ServiceDisclaimerModel SaveDisclaimerDARDetails(ServiceDisclaimerModel obj)
        {
            ObjUserRepository = new UserRepository();
            objDARRepository = new DARRepository();
            try
            {
                if (obj.LocationId > 0 && obj.ServiceAuthKey != null)
                {
                    var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == obj.ServiceAuthKey && x.IsDeleted == false);
                    if (authuser != null && authuser.UserId > 0 && obj.TaskType > 0)
                    {
                        var result = objDARRepository.SaveDisclaimerDAR(obj);
                        if (result > 0)
                        {
                            obj.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                            obj.DARId = result;
                            obj.ResponseMessage = CommonMessage.SaveSuccessMessage();
                        }
                        else
                        {
                            obj.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                            obj.DARId = result;
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

        public ServiceDARModel UpdateEndTimeDAR(ServiceDARModel obj)
        {
            DARDetail objDARDetail;
            ObjUserRepository = new UserRepository();
            objDARRepository = new DARRepository();
            try
            {
                var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == obj.ServiceAuthKey && x.IsDeleted == false);
                if (authuser != null && authuser.UserId > 0)
                {
                    objDARDetail = objDARRepository.GetAll(x => x.IsDeleted == false && x.DARId == obj.DARId && x.LocationId == obj.LocationId).FirstOrDefault();
                    if (objDARDetail != null)
                    {
                        objDARDetail.EndTime = DateTime.UtcNow;
                        objDARRepository.Update(objDARDetail);
                        obj.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        obj.ResponseMessage = CommonMessage.SaveSuccessMessage();
                    }
                    else
                    {
                        obj.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                        obj.ResponseMessage = CommonMessage.NoRecordMessage();
                    }
                }
                else
                {
                    obj.Response = Convert.ToInt32(ServiceResponse.InvalidSessionResponse, CultureInfo.CurrentCulture);
                    obj.ResponseMessage = CommonMessage.InvalidUser();
                }

            }
            catch (Exception)
            {
                throw;
            }
            return obj;
        }

        public DARModelList GetDARDetailsById(long? darId)
        {
            DARModelList objDARModel = new DARModelList();

            objDARRepository = new DARRepository();
            try
            {
                var darDetail = objDARRepository.GetSingleOrDefault(x => x.DARId == darId && x.IsDeleted == false);
                if (objDARModel != null)
                {
                    objDARModel.DisclaimerFormFile = darDetail.DisclaimerFormFile;
                    objDARModel.DARId = darDetail.DARId;
                    objDARModel.ActivityDetails = darDetail.ActivityDetails;
                    objDARModel.CreatedOn = darDetail.CreatedOn.ToString();

                    return objDARModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>Save eFleet DAR Details
        /// <CreatedFor>For Insert eFleet DAR details</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Sept-05-2017s</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceDARModel"></param>
        /// <returns>long DARId</returns> 
        public long SaveeFleetDAR(ServiceDARModel ObjServiceDARModel)
        {
            var ObjDARDetail = new DARDetail();
            var objDARRepository = new DARRepository();
            try
            {
                ObjDARDetail.ActivityDetails = ObjServiceDARModel.ActivityDetails;
                ObjDARDetail.LocationId = ObjServiceDARModel.LocationId;
                ObjDARDetail.TaskType = ObjServiceDARModel.TaskType;
                ObjDARDetail.CreatedBy = ObjServiceDARModel.UserId;
                ObjDARDetail.CreatedOn = DateTime.UtcNow;
                ObjDARDetail.DeletedBy = null;
                ObjDARDetail.DeletedOn = null;
                ObjDARDetail.IsDeleted = false;
                ObjDARDetail.IsManual = false;
                ObjDARDetail.ModifiedBy = null;
                ObjDARDetail.ModifiedOn = null;
                ObjDARDetail.UserId = ObjServiceDARModel.UserId;

                //Added by Bhushan on 09/05/2017 for start time of fueling is going to start now.
                if (ObjServiceDARModel.StartTime != null && ObjServiceDARModel.TaskType == Convert.ToInt64(eFleetEnum.Fueling))
                {
                    ObjDARDetail.StartTime = DateTime.UtcNow;
                }

                objDARRepository.Add(ObjDARDetail);
                long DARId = ObjDARDetail.DARId;

                return DARId;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "long SaveeFleetDAR(ServiceDARModel ObjServiceDARModel)", "Exception While saving DAR for eFlee", ObjServiceDARModel.UserId);
                throw;
            }

        }
    }
}

