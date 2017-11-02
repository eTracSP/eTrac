using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data.DataRepository
{
    public class eFleetVehicleRepository : BaseRepository<eFleetVehicle>
    {
        workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();
        /// <summary>
        /// Created By Ashwajit Bansod for creating JQGrid Listing
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="operationName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public eFleetVehicleModel GetUserById(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch)
        {
            return null;
        }
        /// <summary>
        /// Created By Ashwajit Bansod for Getting all details according to vehicle ID
        /// date: 08/12/2017
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public eFleetVehicleModel GetVehicleDetailsById(long vehicleId)
        {
            try
            {
                eFleetVehicleModel objeFleetVehicleModel = _workorderEMSEntities.eFleetVehicles.Where(g => g.VehicleID == vehicleId && g.IsDeleted == false).OrderBy(code => code.VehicleID).Select(s => new eFleetVehicleModel()
                {
                    VehicleIdentificationNo = s.VehicleIdentificationNo,
                    VehicleID = s.VehicleID,
                    Make = s.Make,
                    Model = s.Model,
                    Year = s.Year,
                    ExpirationDate = s.ExpirationDate,
                    AttachmentOfInsurance = s.AttachmentOfInsurance,
                    AttachmentOfRegistration = s.AttachmentOfRegistration,
                    VehicleImage = s.VehicleImage,
                    GVWR = s.GVWR,
                    StorageAddress = s.StorageAddress,
                    DummyField = s.DummyField
                }).SingleOrDefault();

                return objeFleetVehicleModel;
            }
            catch (Exception)
            { throw; }

        }
        /// <summary>
        /// Created By Ashwajit Bansod For Fetching FuelType from Global Codes
        /// </summary>
        /// <returns></returns>
        public List<GlobalCodeModel> GetAllFuelType()
        {
            List<GlobalCodeModel> lstallfueltype = new List<GlobalCodeModel>();
            try
            {
                lstallfueltype = _workorderEMSEntities.GlobalCodes.Where(a => a.Category == "eFleetFuelType").Select(s => new GlobalCodeModel()
                {
                    Category = s.Category,
                    GlobalCodeId = s.GlobalCodeId,
                    CodeName = s.CodeName
                }).ToList();
                return lstallfueltype;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Craeted By Ashwajit Bansod Dated: 08/15/2017
        /// for deleting Vehicle
        /// </summary>
        /// <param name="VehicleId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public eFleetVehicleModel DeleteeFleetVehicle(long VehicleId, long loggedInUserId)
        {
            try
            {
                eFleetVehicleModel objeFleetVehicleModel = _workorderEMSEntities.eFleetVehicles.Where(g => g.VehicleID == VehicleId && g.IsDeleted == false).OrderBy(code => code.VehicleID).Select(s => new eFleetVehicleModel()
                {
                    VehicleIdentificationNo = s.VehicleIdentificationNo,
                    VehicleID = s.VehicleID,
                    Make = s.Make,
                    Model = s.Model,
                    Year = s.Year,
                    ExpirationDate = s.ExpirationDate,
                    AttachmentOfInsurance = s.AttachmentOfInsurance,
                    AttachmentOfRegistration = s.AttachmentOfRegistration,
                    VehicleImage = s.VehicleImage,
                    GVWR = s.GVWR,
                    StorageAddress = s.StorageAddress,

                }).SingleOrDefault();

                return objeFleetVehicleModel;
            }
            catch (Exception)
            { throw; }
        }
    }
}
