using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.UserModels;
using static WorkOrderEMS.Models.eFleetDriverModel;

namespace WorkOrderEMS.BusinessLogic
{
    public interface IDriverEfleet
    {
        eFleetDriverModel SaveEfleetDriver(eFleetDriverModel objeFleetDriverModel);
        List<CountryModel> GetAllcountries();
        //List<StateModel> GetStateByCountryId(long countryId);
        eFleetDriverModel GetDriverDetailsById(long DriverId);
        eFleetDriverModel GetAllDriverList(eFleetDriverModel objeFleetDriverList);
        List<StateModel> GetStateByCountryID();

        Result DeleteeFleetDriver(long driverId, long loggedInUserId, string location);
        eDriverDetails GetListDriverDetails(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType);
        List<EmployeeModel> GetAllEmployees(long LocationId, long DriverID);
        bool IsLicenseExist(string LicenseNumber);

    }
}
