using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data.DataRepository
{
    public class eFleetPreventativeMaintenanceRepository : BaseRepository<eFleetPreventativeMaintenance>, IPreventativeMaintenanceRepository
    {
        workorderEMSEntities objworkorderEMSEntities = new workorderEMSEntities();
        /// <summary>
        /// Created By Ashwajit Bansod Dated 08/29/2017
        /// Get all the Vehicle number with vehicle ID from eFleetVehicle Table
        /// </summary>
        /// <returns></returns>
        public List<eFleetVehicleModel> GetVehicleNumber()
        {
            List<eFleetVehicleModel> lstvehicle = new List<eFleetVehicleModel>();
            try
            {
                lstvehicle = objworkorderEMSEntities.eFleetVehicles.Select(s => new eFleetVehicleModel()
                {
                    VehicleID = s.VehicleID,
                    QRCodeID = s.QRCodeID,
                    VehicleNumber = s.VehicleNumber
                }).ToList();
                return lstvehicle;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Created By Ashwajit Bansod Dated 08/30/2017
        /// for Fetching all the MeterValue which is belongs to miles 
        /// </summary>
        /// <returns></returns>
        public List<eFleetMeterModel> GetAllMeterValue()
        {
            List<eFleetMeterModel> lstmetervalue = new List<eFleetMeterModel>();
            try
            {
                var meterval = Convert.ToInt64(eFleetEnum.Miles);
                lstmetervalue = objworkorderEMSEntities.eFleetMeters.Where(a => a.Meter == meterval).Select(s => new eFleetMeterModel()
                {
                    ID = s.ID,
                    MeterValue = s.MeterValue,
                    Meter = s.Meter
                }).ToList();               
                lstmetervalue.OrderBy(x => x.ID).ToList();
                return lstmetervalue;
            }
            catch (Exception)
            {
                throw;
            }
        }       
        /// <summary>
        /// Created By Ashwajit Bansod Dated 08/29/2017
        /// For Fetching all the Meter List from GrobalCodes Table along with their CodeID
        /// </summary>
        /// <returns></returns>
        public List<GlobalCodeModel> GetAllMeterList()
        {
            List<GlobalCodeModel> lstmeter = new List<GlobalCodeModel>();
            try
            {
                lstmeter = objworkorderEMSEntities.GlobalCodes.Where(a => a.Category == "eFleetMeter").Select(s => new GlobalCodeModel()
                {
                    Category = s.Category,
                    GlobalCodeId = s.GlobalCodeId,
                    CodeName = s.CodeName
                }).ToList();
                return lstmeter;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Created By Ashwajit for Getting all Category
        /// </summary>
        /// <returns></returns>
        public List<GlobalCodeModelDDL> GetAllCategory()
        {
            var lstallcategory = new List<GlobalCodeModelDDL>();
            try
            {
                lstallcategory = objworkorderEMSEntities.GlobalCodes.Where(a => a.Category == "eFleetModuleCategory").Select(s => new GlobalCodeModelDDL()
                {
                    GlobalCodeId = s.GlobalCodeId,
                    CodeName = s.CodeName
                }).ToList();
                return lstallcategory;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
