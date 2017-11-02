using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;


namespace WorkOrderEMS.Data.Interfaces
{
    interface ICommonRepository
    {
        List<CountryModel> GetAllcountries();
        List<StateModel> GetStateByCountryID(long CountryID);

        /// <summary>GetGlobalCodeFor
        /// CreatedBy   :   Nagendra Upwanshi
        /// CreatedOn   :   Sep-03-2014
        /// CreatedFor  :   Get Global Code For Categories in "InCategories"
        /// </summary>
        /// <param name="InCategories"></param>
        /// <returns></returns>
        List<GlobalCodeModel> GetGlobalCodeFor(string[] InCategories);

        List<UserModel> GetManagersBYLocationId(long locationId);

        List<UserModel> GetClientsBYLocationId(long locationId);

        List<EmailToUserModel> GetUsersToEmail(long LocationID, long ? employeeId);

        LocationMasterModel GetLocationDetailsById(long locationId);

       
    }
}
