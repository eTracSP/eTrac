using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces.eFleet;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models.ServiceModel;
using WorkOrderEMS.Models.ServiceModel.ApiModel;

namespace WorkOrderEMS.BusinessLogic.Managers.eFleet
{
     public class PassengerTrackingManeger : IEfleetPassengerTracking
    {
        public List<eFleetPassengerTrackingRouteModel> GetAllPassengerTrackingRouteDetails(ServiceBaseModel obj)
        {
            try
            {
                var db = new workorderEMSEntities();
                var Results = db.eFleetPassengerTrackingCounts.Where(a => a.IsDeleted == false).Select(a => new eFleetPassengerTrackingRouteModel()
                {
                    RouteID = a.RouteID,
                    ServiceType = a.GlobalCode.GlobalCodeId
                    //VehicleIdentificationNo = a.VehicleIdentificationNo,
                    //VehicleNumber = a.VehicleNumber
                }).ToList<eFleetPassengerTrackingRouteModel>();

                return Results;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<eFleetPassengerTrackingRouteModel> GetAllPassengerTrackingRouteDetails(ServiceBaseModel obj)", "Exception While Listing Route detail.", obj.UserId);
                throw;
            }
        }
    }
}
