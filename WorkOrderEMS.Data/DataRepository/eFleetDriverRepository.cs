using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.UserModels;

namespace WorkOrderEMS.Data.DataRepository
{
     public class eFleetDriverRepository : BaseRepository<eFleetDriver>
    {       
        workorderEMSEntities objworkorderEMSEntities = new workorderEMSEntities();
        //private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        //string EmployeeProfilePicPath = ConfigurationManager.AppSettings["ProfilePicPath"];
        /// <summary>
        /// Created By Ashwajit Bansod For getting all Coutries 
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Created By Ashwajit Bansod Sept-26-2017
        /// For Fetching Employee with Image path
        /// </summary>
        /// <returns></returns>
        //public List<EmployeeModel> GetAllEmployees()
        //{
        //    List<EmployeeModel> lstEmployee = new List<EmployeeModel>();
        //    try
        //    {
        //        lstEmployee = objworkorderEMSEntities.UserRegistrations.Where(d => d.UserType == 3 && d.IsDeleted == false).Select(c => new EmployeeModel()
        //        {
        //            FirstName = c.FirstName + " " + c.LastName,
        //            //LastName = c.LastName,
        //            UserId = c.UserId,
        //            ProfileImage = c.ProfileImage == null ? "" : HostingPrefix + EmployeeProfilePicPath.Replace("~", "") + c.ProfileImage,
        //            // ProfileImage = c.ProfileImage,
        //            UserType = c.UserType

        //        }).ToList();
        //        return lstEmployee;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        /// <summary>
        /// Created By Ashwajit Bansod For Getting All States
        /// </summary>
        /// <returns></returns>
        public List<StateModel> GetStateByCountryID()
        {
            List<StateModel> lstState = new List<StateModel>();
            try
            {
                lstState = objworkorderEMSEntities.MasterStates.Select(s => new StateModel()
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
    }
}
