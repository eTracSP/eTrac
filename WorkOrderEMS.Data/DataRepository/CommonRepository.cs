using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;


namespace WorkOrderEMS.Data
{
    public class CommonRepository : ICommonRepository
    {
        workorderEMSEntities objworkorderEMSEntities = new workorderEMSEntities();


        //Created by Gayatri on 25-Aug-2014
        //To Get List OF All countries
        public List<CountryModel> GetAllcountries()
        {
            List<CountryModel> lstCountry = new List<CountryModel>();
            try
            {
                lstCountry = objworkorderEMSEntities.MasterCountries.Where(d => d.IsDeleted == false).Select(c => new CountryModel()
                {
                    CountryID = c.CountryID,
                    CountryName = c.CountryName,
                    CountryCode = c.CountryCode
                }).ToList();
                return lstCountry;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Created by Gayatri on 25-Aug-2014
        //To Get List OF All State by countryid
        public List<StateModel> GetStateByCountryID(long CountryID)
        {
            List<StateModel> lstState = new List<StateModel>();
            try
            {
                lstState = objworkorderEMSEntities.MasterStates.Where(st => st.CountryId == CountryID && st.IsDeleted == false).Select(s => new StateModel()
                    {
                        StateId = s.StateId,
                        StateName = s.StateName,
                        StateCode = s.StateCode,
                        FrStateId = s.StateId
                    }).ToList();
                return lstState;
            }
            catch (Exception)
            {
                throw;
            }
        }

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
        public List<GlobalCodeModel> GetGlobalCodeData(string category)
        {
            List<GlobalCodeModel> lstglobalcode = new List<GlobalCodeModel>();

            try
            {
                lstglobalcode = objworkorderEMSEntities.GlobalCodes.Where(g => g.Category == category && g.IsActive == true && g.IsDeleted == false).OrderBy(code => code.SortOrder).Select(s => new GlobalCodeModel()
                {
                    GlobalCodeId = s.GlobalCodeId,
                    CodeName = s.CodeName
                }).ToList();
                return lstglobalcode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>GetGlobalCodeFor
        /// CreatedBy   :   Nagendra Upwanshi
        /// CreatedOn   :   Sep-03-2014
        /// CreatedFor  :   Get Global Code For Categories in "InCategories"
        /// </summary>
        /// <param name="InCategories"></param>
        /// <returns></returns>
        public List<GlobalCodeModel> GetGlobalCodeFor(string[] InCategories)
        {
            List<GlobalCodeModel> lstglobalcode = new List<GlobalCodeModel>();
            try
            {
                lstglobalcode = objworkorderEMSEntities.GlobalCodes.Where(g => InCategories.Contains(g.Category) && g.IsActive == true && g.IsDeleted == false).OrderBy(code => code.SortOrder).Select(s => new GlobalCodeModel()
                {
                    GlobalCodeId = s.GlobalCodeId,
                    CodeName = s.CodeName,
                    Category = s.Category,
                    Description = s.Description,
                }).ToList();
                return lstglobalcode;
            }
            catch (Exception)
            { throw; }
        }

        public List<UserModel> GetEmployeeProject(long LocationID)
        {
            try
            {
                string usertype = Convert.ToString(UserType.Employee);
                List<UserModel> lstUser = (from u in objworkorderEMSEntities.UserRegistrations
                                           join gc in objworkorderEMSEntities.GlobalCodes on u.UserType equals gc.GlobalCodeId
                                           join elm in objworkorderEMSEntities.EmployeeLocationMappings on u.UserId equals elm.EmployeeUserId
                                           where gc.CodeName == usertype && u.IsDeleted == false
                                           && elm.LocationId == LocationID && u.IsDeleted == false
                                           select u).ToList().Select(u => new UserModel()
                                           {   
                                               UserId = u.UserId,
                                               FirstName = u.LastName != null ? u.FirstName + ' ' + u.LastName : u.FirstName
                                           }).ToList();
                return lstUser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>GetManagersBYLocationId
        /// <CreatedBy>Roshan Rahood</CreatedBy>
        /// <CreatedOn>05-Jan-2015</CreatedOn>
        /// <CreatedFor>To get the Manager details based on location Id</CreatedFor>
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public List<UserModel> GetManagersBYLocationId(long locationId)
        {
            try
            {
                List<UserModel> lstManagers = (from mlm in objworkorderEMSEntities.ManagerLocationMappings
                                               join ur in objworkorderEMSEntities.UserRegistrations on mlm.ManagerUserId equals ur.UserId
                                               where mlm.LocationId == locationId && ur.IsDeleted == false
                                               select ur).Select(u => new UserModel()
                                           {
                                              
                                               UserId = u.UserId,
                                               FirstName = !string.IsNullOrEmpty(u.LastName) ? u.FirstName + " " + u.LastName : u.FirstName,
                                               UserEmail = u.UserEmail,
                                               UserType = u.UserType
                                           }).ToList();
                return lstManagers;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>GetAdminBYLocationId
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>17-March-2015</CreatedOn>
        /// <CreatedFor>To get the admin details based on location Id</CreatedFor>
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public List<UserModel> GetAdminBYLocationId(long locationId)
        {
            try
            {
                List<UserModel> lstManagers = (from mlm in objworkorderEMSEntities.AdminLocationMappings
                                               join ur in objworkorderEMSEntities.UserRegistrations on mlm.AdminUserId equals ur.UserId
                                               where mlm.LocationId == locationId && ur.IsDeleted == false
                                               select ur).Select(u => new UserModel()
                                               {
                                                  
                                                   UserId = u.UserId,
                                                   FirstName = !string.IsNullOrEmpty(u.LastName) ? u.FirstName + " " + u.LastName : u.FirstName,
                                                   UserEmail = u.UserEmail,
                                                   UserType = u.UserType
                                               }).ToList();
                return lstManagers;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>GetClientsBYLocationId
        /// <CreatedBy>Roshan Rahood</CreatedBy>
        /// <CreatedOn>05-Jan-2015</CreatedOn>
        /// <CreatedFor>To get the Client details based on location Id</CreatedFor>
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public List<UserModel> GetClientsBYLocationId(long locationId)
        {
            try
            {

                List<UserModel> lstClients = (from lcm in objworkorderEMSEntities.LocationClientMappings
                                              join ur in objworkorderEMSEntities.UserRegistrations on lcm.ClientUserId equals ur.UserId
                                              where lcm.LocationId == locationId && ur.IsDeleted == false && ur.IsLoginActive == true
                                              select ur).Select(u => new UserModel()
                                           {
                                             
                                               UserId = u.UserId,
                                               FirstName = u.LastName != null ? u.FirstName + " " + u.LastName : u.FirstName,
                                               UserEmail = u.UserEmail,
                                               UserType = u.UserType
                                           }).ToList();
                return lstClients;
            }
            catch (Exception)
            {
                throw;
            }
        }



        //public List<ProjectMasterModel> GetNotAssgnProject(string Usertype, long ProjectId)
        //{
        //    List<ProjectMasterModel> lstProject = new List<ProjectMasterModel>();
        //    try
        //    {

        //        var exceptionList = (from p in objworkorderEMSEntities.ProjectMasters
        //                             join u in objworkorderEMSEntities.UserRegistrations on p.ProjectID equals u.ProjectID
        //                             join gcu in objworkorderEMSEntities.GlobalCodes on u.UserType equals gcu.GlobalCodeId
        //                             where gcu.CodeName == Usertype
        //                             select p.ProjectID);

        //        lstProject = (from p in objworkorderEMSEntities.ProjectMasters where !exceptionList.Contains(p.ProjectID) || p.ProjectID == ProjectId select p).ToList().Select(t => new ProjectMasterModel()
        //        {
        //            ProjectID = t.ProjectID,
        //            Location = t.ProjectName
        //        }).ToList();

        //        return lstProject;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        /// <summary>GetListAllLocation
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
        public List<EmailToUserModel> GetUsersToEmail(long LocationID, long? employeeId)
        {
            try
            {

                List<EmailToUserModel> lstEmailToUser = objworkorderEMSEntities.ssp_EmailDetails(LocationID, employeeId).Select(l =>
                    new EmailToUserModel()
                    {
                        LocationId = l.LocationId,
                        LocationName = l.LocationName,
                        Address1 = l.Address1,
                        Address2 = l.Address2,
                        CountryName = l.CountryName,
                        StateName = l.StateName,
                        City = l.City,
                        Mobile = l.Mobile,
                        PhoneNo = l.PhoneNo,
                        ZipCode = l.ZipCode,
                        FirstName = l.FirstName,
                        LastName = l.LastName,
                        UserEmail = l.UserEmail,
                        AlternareEmail = l.AlternateEmail,
                        SubscriptionEmail = l.SubscriptionEmail,
                        UserType = l.UserType
                    }).ToList();

                return lstEmailToUser;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>GetLocationDetailsById
        /// <CreatedBy>Roshan Rahood</CreatedBy>
        /// <CreatedOn>Jan-07-2015</CreatedOn>
        /// <CreatedFor>Get All Location List</CreatedFor>
        /// </summary>
        public LocationMasterModel GetLocationDetailsById(long locationId)
        {
            try
            {
                LocationMasterModel objlocationmaster = objworkorderEMSEntities.LocationMasters.Where(b => b.LocationId == locationId).Select(a => new LocationMasterModel
                {
                    LocationName = a.LocationName,
                    Address1 = a.Address1,
                    Address2 = a.Address2,
                    StateId = a.StateId,
                    City = a.City,
                    LocationId = a.LocationId

                }).SingleOrDefault();

                return objlocationmaster;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UserModel> GetEmployeeListByLocation(long location)
        {
            List<UserModel> employeelist = new List<UserModel>();
            try
            {
                //employeelist = (from ur in objworkorderEMSEntities.UserRegistrations
                //                join elm in objworkorderEMSEntities.EmployeeLocationMappings 
                //                         on ur.UserId equals elm.EmployeeUserId
                //                where elm.LocationId == location 
                //                      && elm.IsDeleted == false


                //  List<UserModel> lstClients = (from lcm in objworkorderEMSEntities.LocationClientMappings
                //                              join ur in objworkorderEMSEntities.UserRegistrations on lcm.ClientUserId equals ur.UserId
                //                              where lcm.LocationId == locationId
                //                              select ur).Select(u => new UserModel()
                //                           {
                //                               UserId = u.UserId,
                //                               FirstName = u.LastName != null ? u.FirstName + " " + u.LastName : u.FirstName,
                //                               UserEmail = u.UserEmail,
                //                               UserType = u.UserType
                //                           }).ToList();


            }
            catch (Exception)
            {
                throw;
            }
            return employeelist;
        }
        /// <summary>GetDetails of manager to send email if brake not functional in QRC Vehicle
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>feb-25-2015</CreatedOn>
        /// <CreatedFor>Get manager list according to email</CreatedFor>
        /// </summary>
        public List<ssp_SendEmailForBrakeNotFunctional_Result> SendEmailToManager(long BrakesWorkOrderId,long LocationId)
        {
            List<ssp_SendEmailForBrakeNotFunctional_Result> obj = new List<ssp_SendEmailForBrakeNotFunctional_Result>();
            try
            {
                obj = objworkorderEMSEntities.ssp_SendEmailForBrakeNotFunctional(BrakesWorkOrderId, LocationId).ToList();
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>Get Dashboard Details 
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>DashboardCount</CreatedFor>
        /// <CreatedOn>April-21-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCElevatorModel"></param>
        /// <returns></returns>
        public ServiceDashboardModel DashboardCountForMobile(long UserId, long LocationId)
        {
            ServiceDashboardModel obj = new ServiceDashboardModel();// sp_GetCountForDashboard_Result();
            try
            {
                obj = objworkorderEMSEntities.sp_GetCountForDashboard(UserId, LocationId).Select(t => new ServiceDashboardModel()
                {
                    DarCount = t.DarCount,
                    RuleViolationCount = t.RuleViolationCount,
                    WorkRequestCount = t.WorkRequestCount,
                    FacilityRequestCount = t.FacilityRequestCount,
                    FacilityRequestCountLocation = t.FacilityRequestCountLocation,
                    ContinuousRequestCount = t.ContinuesRequestCount
                }).SingleOrDefault();
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
