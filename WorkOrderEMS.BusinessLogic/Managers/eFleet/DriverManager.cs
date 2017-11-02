using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.UserModels;
using static WorkOrderEMS.Models.eFleetDriverModel;

namespace WorkOrderEMS.BusinessLogic.Managers.eFleet
{
    public class DriverManager : IDriverEfleet
    {
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string ProfilePicPath = ConfigurationManager.AppSettings["ProfilePicPath"];//Image save path declared in webConfig File
        //string EmployeeProfilePicPath = ConfigurationManager.AppSettings["ProfilePicPath"];
        CommonMethodManager _ICommonMethod = new CommonMethodManager();
        /// <summary>
        /// Created by Ashwait Bansod for Saving data to database and also update data to database
        /// </summary>
        /// <param name="objeFleetDriverModel"></param>
        /// <returns></returns>
        public eFleetDriverModel SaveEfleetDriver(eFleetDriverModel objeFleetDriverModel)
        {
            try
            {
                var objLocationMaster = new LocationMaster();
                var objeFleetDriver = new eFleetDriver();
                var objeFleetDriverRepository = new eFleetDriverRepository();
                var objeTracLoginModel = new eTracLoginModel();

                if (objeFleetDriverModel.DriverID == 0)
                {
                    AutoMapper.Mapper.CreateMap<eFleetDriverModel, eFleetDriver>();
                    var objfleetDriverMapper = AutoMapper.Mapper.Map(objeFleetDriverModel, objeFleetDriver);
                    objeFleetDriverRepository.Add(objfleetDriverMapper);
                    //objeFleetDriver.QRCCodeID = objeFleetDriverModel.QRCCodeID + "EFD" + (objeFleetDriver.DriverID + 100).ToString();
                    objeFleetDriverRepository.SaveChanges();
                    objeFleetDriverModel.Result = Result.Completed;

                    if (objeFleetDriverModel.Result == Result.Completed)
                    {
                        #region Save DAR
                        DARModel objDAR = new DARModel();
                        objDAR.ActivityDetails = DarMessage.RegisterNeweFleetDriver(objeFleetDriverModel.LocationName);
                        objDAR.LocationId = objeFleetDriverModel.LocationID;
                        objDAR.UserId = objeFleetDriverModel.UserID;
                        objDAR.CreatedBy = objeFleetDriverModel.UserID;
                        objDAR.CreatedOn = DateTime.UtcNow;
                        objDAR.TaskType = (long)TaskTypeCategory.eFleetDriverSubmission;
                        Result result = _ICommonMethod.SaveDAR(objDAR);
                        #endregion Save DAR
                    }
                }
                //edit Data
                else
                {
                    var DriverData = objeFleetDriverRepository.GetAll(v => v.IsDeleted == false && v.DriverID == objeFleetDriverModel.DriverID).FirstOrDefault();
                    if (objeFleetDriverModel.DriverImageFile != null)
                    {
                    }
                    else
                    {
                        objeFleetDriverModel.DriverImage = DriverData.DriverImage;
                    }
                    //== null ? "" : HostingPrefix + ProfilePicPath.Replace("~", "") + DriverData.DriverImage;
                    AutoMapper.Mapper.CreateMap<eFleetDriverModel, eFleetDriver>();
                    var objfleetDriverMapper = AutoMapper.Mapper.Map(objeFleetDriverModel, DriverData);
                    objeFleetDriverRepository.SaveChanges();
                    objeFleetDriverModel.Result = Result.UpdatedSuccessfully;
                    if (objeFleetDriverModel.Result == Result.UpdatedSuccessfully)
                    {
                        #region Save DAR
                        DARModel objDAR = new DARModel();
                        objDAR.ActivityDetails = DarMessage.UpdateeFleetDriver(objeFleetDriverModel.LocationName);
                        objDAR.LocationId = objeFleetDriverModel.LocationID;
                        objDAR.UserId = objeFleetDriverModel.UserID;
                        objDAR.ModifiedBy = objeFleetDriverModel.UserID;
                        objDAR.ModifiedOn = DateTime.UtcNow;
                        objDAR.TaskType = (long)TaskTypeCategory.UpdateeFleetDriver;
                        Result result = _ICommonMethod.SaveDAR(objDAR);
                        #endregion Save DAR
                    }
                }
                return objeFleetDriverModel;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public eFleetDriverModel SaveEfleetDriver(eFleetDriverModel objeFleetDriverModel)", "Exception While saving Driver request.", objeFleetDriverModel);
                throw;
            }
        }

        // get Country List From Master County Table 
        public List<CountryModel> GetAllcountries()
        {
            try
            {

                eFleetDriverRepository objeFleetDriverRepository = new eFleetDriverRepository();
                return objeFleetDriverRepository.GetAllcountries();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<CountryModel> GetAllcountries()", "Exception While Getting All Countries.", null);
                throw;
            }
        }
        /// <summary>
        /// Created By Ashwajit Bansod Dated : Sept-26-2017
        /// For Fetching only employee records from UserRegistration 
        /// </summary>
        /// <returns></returns>
        public List<EmployeeModel> GetAllEmployees()
        {
            try
            {
                workorderEMSEntities objworkorderEMSEntities = new workorderEMSEntities();
                var objeFleetDriverRepository = new eFleetDriverRepository();
                List<EmployeeModel> lstEmployee = new List<EmployeeModel>();
                lstEmployee = objworkorderEMSEntities.UserRegistrations.Where(d => d.UserType == 3 && d.IsDeleted == false && d.JobTitle == "389").Select(c => new EmployeeModel()
                {
                    FirstName = c.FirstName + " " + c.LastName,
                    //LastName = c.LastName,
                    UserId = c.UserId,
                    ProfileImage = c.ProfileImage == null ? "" : HostingPrefix + ProfilePicPath.Replace("~", "") + c.ProfileImage,
                    // ProfileImage = c.ProfileImage,
                    UserType = c.UserType

                }).ToList();
                return lstEmployee;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<EmployeeModel> GetAllEmployees()", "Exception While Getting All Employee.", null);
                throw;
            }
        }
        //Get all State from MasterState Table
        public List<StateModel> GetStateByCountryID()
        {
            try
            {
                eFleetDriverRepository objeFleetDriverRepository = new eFleetDriverRepository();
                return objeFleetDriverRepository.GetStateByCountryID();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public List<StateModel> GetStateByCountryID()", "Exception While Getting All States.", null);
                throw;
            }
        }
        //Get Driver List Model
        public eFleetDriverModel GetAllDriverList(eFleetDriverModel objeFleetDriverList)
        {
            return objeFleetDriverList;
        }
        /// <summary>
        /// Added By Ashwajit Bansod
        /// For Geting all the Data from the database and and Put it inti JQ Grid list
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="locationId"></param>
        /// <param name="textSearch"></param>
        /// <param name="statusType"></param>
        /// <returns></returns>
        public eDriverDetails GetListDriverDetails(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)
        {
            try
            {
                var db = new workorderEMSEntities();
                var objeDriverDetails = new eDriverDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                var Results = db.eFleetDrivers.Where(a => a.IsDeleted == false).Select(a => new eFleetDriverModel()
                {
                    DriverID = a.DriverID,
                    DriverLicenseNo = a.DriverLicenseNo,
                    CDLType = a.CDLType,
                    EmployeeNameList = a.UserRegistration.FirstName + " " + a.UserRegistration.LastName,
                    CDLExpiration = a.CDLExpiration,
                    MedicleCardExpiration = a.MedicleCardExpiration,
                    MVRExpiration = a.MVRExpiration,
                    DriverImage = (from em in db.UserRegistrations where em.UserId == a.EmployeeName select em.ProfileImage == null ? "" : HostingPrefix + ProfilePicPath.Replace("~", "") + em.ProfileImage).FirstOrDefault(),
                });
                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                //if (sortOrderBy.ToLower() == "DESC")
                //{
                //    Results = Results.OrderByDescending(s => s.QRCodeID);
                //    Results = Results.Skip(pageindex * pageSize).Take(pageSize);
                //}
                //else
                //{
                Results = Results.OrderByDescending(s => s.DriverID);
                // Results = Results.Skip(pageindex * pageSize).Take(pageSize);
                //}
                objeDriverDetails.pageindex = pageindex;
                objeDriverDetails.total = totalPages;
                objeDriverDetails.records = totRecords;
                objeDriverDetails.rows = Results.ToList();
                return objeDriverDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public eDriverDetails GetListDriverDetails(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)", "Exception While Getting List of Driver.", null);
                throw;
            }
        }
        /// <summary>
        /// Added by Ashwajit Bansod 
        /// while editing a driver then getting all the data of that driver
        /// </summary>
        /// <param name="DriverId"></param>
        /// <returns></returns>
        public eFleetDriverModel GetDriverDetailsById(long DriverId)
        {
            try
            {
                var db = new workorderEMSEntities();
                var ObjeFleetDriverRepository = new eFleetDriverRepository();
                var editDriverDetails = new eFleetDriverModel();
                var objeFleetDriver = new eFleetDriver();
                var driverDetails = ObjeFleetDriverRepository.GetSingleOrDefault(u => u.DriverID == DriverId && u.IsDeleted == false);
                if (driverDetails.DriverID > 0)
                {
                    //editDriverDetails.Passwordforedit = driverDetails.Password;
                    AutoMapper.Mapper.CreateMap<eFleetDriver, eFleetDriverModel>();
                    var objfleetVehicleMapper = AutoMapper.Mapper.Map(driverDetails, editDriverDetails);
                    editDriverDetails.DriverImage = (from em in db.UserRegistrations where em.UserId == driverDetails.EmployeeName select em.ProfileImage == null ? "" : HostingPrefix + ProfilePicPath.Replace("~", "") + em.ProfileImage).FirstOrDefault();
                    //editDriverDetails.DriverImage = driverDetails.DriverImage == null ? "" : HostingPrefix + ProfilePicPath.Replace("~", "") + driverDetails.DriverImage;
                    // editDriverDetails.DOBForEdit = driverDetails.DOB;
                }
                return editDriverDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public eFleetDriverModel GetDriverDetailsById(long DriverId)", "Exception While Editing Driver.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By Ashwajit Bansod 
        /// for Deleting the Driver and set IsDeleted Field to 1
        /// </summary>
        /// <param name="driverId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public Result DeleteeFleetDriver(long driverId, long loggedInUserId, string location)
        {
            var objDAR = new DARModel();
            try
            {
                Result result;
                if (driverId > 0)
                {
                    eFleetDriverRepository objeFleetDriverRepository = new eFleetDriverRepository();
                    var data = objeFleetDriverRepository.GetSingleOrDefault(v => v.DriverID == driverId && v.IsDeleted == false);
                    if (data != null)
                    {
                        data.IsDeleted = true;
                        data.DeletedBy = Convert.ToInt32(loggedInUserId);
                        data.DeletedDate = DateTime.UtcNow;
                        objeFleetDriverRepository.Update(data);

                        objeFleetDriverRepository.SaveChanges();
                        //var objDAR = new DARModel();
                        objDAR.ActivityDetails = DarMessage.DeleteFleetDriver(location);
                        objDAR.TaskType = (long)TaskTypeCategory.DeleteeFleetDriver;

                        #region Save DAR
                        objDAR.LocationId = data.LocationID.Value;
                        objDAR.UserId = loggedInUserId;
                        objDAR.DeletedBy = data.DeletedBy;
                        objDAR.DeletedOn = DateTime.UtcNow;
                        result = _ICommonMethod.SaveDAR(objDAR);
                        #endregion Save DAR

                        return Result.Delete;
                    }
                }
                else
                {
                    return Result.DoesNotExist;
                }
                return Result.Delete;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public Result DeleteeFleetDriver(long driverId, long loggedInUserId)", "Exception While Deleting Driver.", null);
                throw;
            }
        }

    }
}
