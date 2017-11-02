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
    public class eFleetVehicleIncidentRepository :BaseRepository<eFleetVehicleIncident>, IVehicleIncidentRepository
    {
        workorderEMSEntities objworkorderEMSEntities = new workorderEMSEntities();
        public List<StateModel> GetStateID()
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
        public List<eFleetVehicleModel> GetAllVehicleNumber()
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
    }
}
