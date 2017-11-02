using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.BusinessLogic
{
    public interface IEfleetVehicle
    {
        eFleetVehicleModel SaveEfleetVehicle(eFleetVehicleModel objeFleetVehicleModel);

        ServiceResponseModel<UpdateEfleetInspectionTypeXML> SaveeFleetDamageTireInspectionDetails(eFleetDamageTireModel obj);
        ServiceResponseModel<UpdateEfleetInspectionTypeXML> SaveeFleetInteriorMileageInspectionDetails(eFleetInteriorMileageDriverModel obj);
        ServiceResponseModel<UpdateEfleetInspectionTypeXML> SaveeFleetEngineExteriorInspectionDetails(eFleetEngineExteriorModel obj);
        ServiceResponseModel<UpdateEfleetInspectionTypeXML> SaveeFleetEmergencyAccessoriesInspectionDetails(eFleetEmergencyAccessoriesModel obj);

        ServiceResponseModel<string> ChangingStatusOfInsection(ChangeInspectionStatusModel objChangeInspectionStatusModel);

        List<listForEmployeeDevice> SendeFleetCheckOutInNotificaitonToAllManager(long LocationId, long UserId);

        eFleetVehicleModel GetAllVehicleList(eFleetVehicleModel objeFleetVehicleList);
        eDetails GetListVehicleDetails(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType);
        eFleetVehicleModel GetVehicleDetailsById(long VehicleID);
        Result DeleteeFleetVehicle(long QRCID, long loggedInUserId, string location);
        List<GlobalCodeModel> GetAllFuelType();

        List<VehicleDetailsModel> GetAllVehicleListDetails(ServiceBaseModel obj);
        bool IsVehicleExist(string VehicleNumber);
    }
}
