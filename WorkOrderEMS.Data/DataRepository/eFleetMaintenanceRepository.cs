using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data.DataRepository
{
    public class eFleetMaintenanceRepository : BaseRepository<eFleetMaintenance>, IMaintenanceRepository
    {       
            workorderEMSEntities objworkorderEMSEntities = new workorderEMSEntities();
            public List<eFleetVehicleModel> GetVehicleNumber(long LocationID)
            {
                List<eFleetVehicleModel> lstvehicle = new List<eFleetVehicleModel>();
                try
                {
                    lstvehicle = objworkorderEMSEntities.eFleetVehicles.Where(a => a.IsDeleted == false && a.LocationID == LocationID).Select(s => new eFleetVehicleModel()
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
            /// Created By Ashwajit Bansod : Dated Sept-20-2017
            /// for fetching Maintenance type from Global codes
            /// </summary>
            /// <returns></returns>
            public List<GlobalCodeModel> GetAllMaintenanceType()
            {
                List<GlobalCodeModel> lstmaintenanceType = new List<GlobalCodeModel>();
                try
                {
                    lstmaintenanceType = objworkorderEMSEntities.GlobalCodes.Where(a => a.Category == "eFleetMaintenanceType").Select(s => new GlobalCodeModel()
                    {
                        Category = s.Category,
                        GlobalCodeId = s.GlobalCodeId,
                        CodeName = s.CodeName
                    }).ToList();
                    return lstmaintenanceType;
                }
                catch (Exception)
                {
                    throw;
                }
            }
    }
}


