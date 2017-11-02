using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces.eFleet;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel.ApiModel;

namespace WorkOrderEMS.BusinessLogic.Managers.eFleet
{
    public class HoursOfServicesManager : IHoursOfServices
    {
        /// <summary>
        /// Created By: Ashwajit Bansod
        /// Created Date : Oct-25-2017
        /// Created for : Saving Hours of services
        /// </summary>
        /// <param name="objModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> InsertHoursOfServices(HoursOfServicesModel objModel)
        {
            var objReturnModel = new ServiceResponseModel<string>();
            try
            {
                var objHoursOfServicesRepository = new HoursOfServicesRepository();
                HoursOfService Obj = new HoursOfService();
                AutoMapper.Mapper.CreateMap<HoursOfServicesModel, HoursOfService>();                
                Obj = AutoMapper.Mapper.Map(objModel, Obj);
                Obj.CreatedBy = objModel.UserId;
                Obj.CreatedDate = DateTime.UtcNow;
                objHoursOfServicesRepository.Add(Obj);
                if (Obj.HoursID > 0)
                {
                    var Data = objHoursOfServicesRepository.GetAll(pm => pm.HoursID == objModel.HoursID && pm.IsDeleted == false).FirstOrDefault();
                    objReturnModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.InvariantCulture);
                    objReturnModel.Message = CommonMessage.Successful();
                }
                else
                {
                    objReturnModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.InvariantCulture);
                    objReturnModel.Message = CommonMessage.NoRecordMessage();
                }
            }
            catch (Exception ex)
            {
                WorkOrderEMS.BusinessLogic.Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ServiceResponseModel<string> InsertHoursOfServices(HoursOfServicesModel objModel)", "Exception while insert Hours Of Services", objModel);
                objReturnModel.Message = ex.Message;
                objReturnModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                objReturnModel.Data = null;
            }
            return objReturnModel;
        }
    }
}
