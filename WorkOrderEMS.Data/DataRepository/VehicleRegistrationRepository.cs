using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data
{
    public class VehicleRegistrationRepository : BaseRepository<VehicleRegistration>, IVehicleRegistration
    {
        workorderEMSEntities objworkorderEMSEntities = new workorderEMSEntities();

        /// <summary>GetVehicleDetailsById
        /// CreatedBy   :   Roshan Rahood
        /// CreatedOn   :   Jan-06-2015
        /// CreatedFor  :   Get the Vehicle details by Id.
        /// </summary>
        /// <returns></returns>
        public VehicleRegistrationModel GetVehicleDetailsById(long vehicleId)
        {
            try
            {
                VehicleRegistrationModel objVehicleRegistrationModel = objworkorderEMSEntities.VehicleRegistrations.Where(g => g.VehicleID == vehicleId && g.IsDeleted == false).OrderBy(code => code.VehicleID).Select(s => new VehicleRegistrationModel()
                {
                    VehicleIdentificationNo = s.VehicleIdentificationNo,
                    VehicleMake = s.VehicleMake,
                    VehicleType = s.VehicleType,
                    CreatedBy = s.CreatedBy,
                    IsApprovedByClient=s.IsApprovedByClient,
                    IsApprovedByManager=s.IsApprovedByManager,
                    VehicleID=vehicleId,
                    VehicleModel=s.VehicleModel,
                    VehicleTagNo=s.VehicleTagNo,
                    DriverName=s.DriverName,
                    LicenseNo=s.LicenseNo,
                    PermitTypePrice=s.PermitTypePrice,
                    VendorUserID=s.VendorUserID,
                    QRCID = s.QRCID
                }).SingleOrDefault();

                return objVehicleRegistrationModel;
            }
            catch (Exception)
            { throw; }

        }

        /// <summary>Get QrcId Details for GT-Tracker
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>GetQRCDeatilsByID For GT-Tracker Vechicle</CreatedFor>
        /// <CreatedOn>March-12-2015</CreatedOn>
        /// </summary>
        /// <param name="ObjServiceQRCElevatorModel"></param>
        /// <returns></returns>
        public sp_GetQrcDetailsForVehicle_Result GetQrcDetailsGtTracker(long QrcId,long UserId,long Locationid)
        {
            try
            {
                sp_GetQrcDetailsForVehicle_Result objServiceVehicleRegistration = new sp_GetQrcDetailsForVehicle_Result();
                objServiceVehicleRegistration = objworkorderEMSEntities.sp_GetQrcDetailsForVehicle(QrcId, UserId, Locationid).FirstOrDefault();

                return objServiceVehicleRegistration;
            }
            catch (Exception)
            { throw; }



        }
    }
}
