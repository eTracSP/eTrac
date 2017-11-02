using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.Data
{
    public class DARRepository : BaseRepository<DARDetail>, IDAR
    {
        workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();

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
        public List<DARModelList> GetDARDetails(long? LoginUserId, long? locationId, long? userId, int? taskType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords, string fromDate = null, string toDate = null)
        {
            List<DARModelList> lstDARDetails = new List<DARModelList>();
            try
            {
                lstDARDetails = _workorderEMSEntities.sp_GetAllDARDetails(locationId, userId, LoginUserId, fromDate, toDate, taskType, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, totalRecords).Select(t =>
                    new DARModelList()
                    {
                        DARId = t.DARId,
                        LocationName = t.Location_Name,
                        EmployeeName = t.Employee_Name,
                        ActivityDetails = t.Activity_Details,
                        TaskType = t.TaskType,
                        CreatedOn = t.CreatedOn,
                        EndTimeImage = t.EndTimeImage,
                        StartTimeImage = t.StartTimeImage,
                        StartTime = t.StartTime,
                        EndTime = t.EndTime,
                        TaskTypeInt = t.TaskTypeInt,
                        Description = t.Description,
                        DisclaimerFormFile = t.DisclaimerFormFile
                    }).ToList();

                return lstDARDetails;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>Save DAR Details
        /// <CreatedFor>For Insert DAR details</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Feb-16-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceDARModel"></param>
        /// <returns>long DARId</returns> 
        public long SaveDARDetails(ServiceDARModel ObjServiceDARModel)
        {
            DARDetail ObjDARDetail = new DARDetail();
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

                //Added by Bhushan on 06/02/2017 for entime need to save for facilty end.This method call once facility done for DAR record separate service call.
                if (ObjServiceDARModel.EndTime != null && ObjServiceDARModel.TaskType == 319)
                {
                    ObjDARDetail.EndTime = DateTime.UtcNow;
                }

                Add(ObjDARDetail);
                long DARId = ObjDARDetail.DARId;

                return DARId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>Save DAR for Jump Start
        /// <CreatedFor>For Insert Jump Start and GT Tracker</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>March-16-2015</CreatedOn>
        /// </summary>
        /// <param name="objServiceDARModel"></param>
        /// <returns></returns> 
        public long InsertDARDetailsForTracking(ServiceDARModel ObjServiceDARModel)
        {
            DARDetail ObjDARDetail = new DARDetail();
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
                ObjDARDetail.Description = ObjServiceDARModel.Description;
                if (ObjServiceDARModel.FacilityRequest != true || ObjServiceDARModel.FacilityRequest == null)
                {//This Constraint for error occur if starttime is null
                 // ObjDARDetail.StartTime = Convert.ToDateTime(ObjServiceDARModel.StartTime).ToUniversalTime();
                    ObjDARDetail.StartTime = DateTime.UtcNow;
                }
                //Condition for while accpeting FR which means by default work has started.
                if (ObjServiceDARModel.FacilityRequest == true && ObjServiceDARModel.EndTime == null && ObjServiceDARModel.StartTime != null)
                {//This Constraitn for error occur if starttime is null
                 // ObjDARDetail.StartTime = Convert.ToDateTime(ObjServiceDARModel.StartTime).ToUniversalTime();
                    ObjDARDetail.StartTime = DateTime.UtcNow;
                }
                ObjDARDetail.StartTimeImage = ObjServiceDARModel.StartTimeImage;

                Add(ObjDARDetail);
                long DARId = ObjDARDetail.DARId;

                return DARId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>GetAllDARDetails
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedFor>Get All DAR Details</CreatedFor>
        /// <CreatedOn>March-05-2015</CreatedOn>
        /// <param name="UserID"></param>
        /// <param name="OperationName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<sp_GetAllDARDetailsForMobile_Result> GetALLDARDetailsForMobile(int? UserId, int? taskType, int? LocationId, DateTime? fromDate, DateTime? toDate)
        {
            List<sp_GetAllDARDetailsForMobile_Result> lstDARDetails = new List<sp_GetAllDARDetailsForMobile_Result>();
            try
            {
                lstDARDetails = _workorderEMSEntities.sp_GetAllDARDetailsForMobile(LocationId, UserId, fromDate, toDate, taskType).ToList();

                return lstDARDetails;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>Update the status of DAR jump start
        /// <CreatedFor>For Update Jump Start and GT Tracker</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>March-16-2015</CreatedOn>
        /// </summary>
        /// <param name="objServiceDARModel"></param>
        /// <returns></returns> 

        public ServiceResponseModel<ssp_UpdateDarTaskStatus_Result> UpdateDarTaskStatus(string ServiceAuthKey, long UserId, long DARId, string EndTime, long LocationId, string Description = null, string EndTimeImage = null)
        {
            ServiceResponseModel<ssp_UpdateDarTaskStatus_Result> ObjServiceResponseModel = new ServiceResponseModel<ssp_UpdateDarTaskStatus_Result>();
            try
            {
                //long? test=UserId;
                ObjServiceResponseModel.Data = _workorderEMSEntities.ssp_UpdateDarTaskStatus(ServiceAuthKey, UserId, DARId, Description, EndTime, EndTimeImage, LocationId)
                                                .Select(t => new ssp_UpdateDarTaskStatus_Result()
                                                {
                                                    Response = t.Response,
                                                    ResponseMessage = t.ResponseMessage
                                                }).FirstOrDefault();
            }
            catch (Exception) { throw; }
            return ObjServiceResponseModel;
        }

        /// <summary>Save Disclaimer DAR Details
        /// <CreatedFor>For Insert DAR details</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Feb-16-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceDARModel"></param>
        /// <returns>long DARId</returns> 
        public long SaveDisclaimerDAR(ServiceDisclaimerModel ObjServiceDARModel)
        {
            DARDetail ObjDARDetail = new DARDetail();
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
                ObjDARDetail.Address = ObjServiceDARModel.Address;
                ObjDARDetail.City = ObjServiceDARModel.City;
                ObjDARDetail.CurrentLocation = ObjServiceDARModel.CurrentLocation;
                ObjDARDetail.CustomerContact = ObjServiceDARModel.CustomerContact;
                ObjDARDetail.CustomerName = ObjServiceDARModel.CustomerName;
                ObjDARDetail.CustomerSignatureImage = ObjServiceDARModel.ImageCust;
                ObjDARDetail.DisclaimerFormFile = ObjServiceDARModel.DisclaimerFormFile;
                ObjDARDetail.DriverLicenseNo = ObjServiceDARModel.DriverLicenseNo;
                ObjDARDetail.EmpSignatureImage = ObjServiceDARModel.ImageEmp;
                ObjDARDetail.LicensePlateNo = ObjServiceDARModel.LicensePlateNo;
                ObjDARDetail.ProjectDesc = ObjServiceDARModel.ProjectDesc;
                ObjDARDetail.StateId = ObjServiceDARModel.StateName;
                ObjDARDetail.UserName= ObjServiceDARModel.UserName;
                ObjDARDetail.VehicleColor = ObjServiceDARModel.VehicleColor;
                ObjDARDetail.VehicleMake = ObjServiceDARModel.VehicleMake;
                ObjDARDetail.VehicleModel = ObjServiceDARModel.VehicleModel;
                ObjDARDetail.VehicleYear = ObjServiceDARModel.VehicleYear;
                ObjDARDetail.ZipCode = ObjServiceDARModel.ZipCode;
                ObjDARDetail.StartTime = DateTime.UtcNow;
                Add(ObjDARDetail);
                long DARId = ObjDARDetail.DARId;

                return DARId;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
