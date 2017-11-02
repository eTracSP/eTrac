using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class PushNotificationManager : IPushNotification
    {

        UserRepository ObjUserRepository = null;

        /// <summary>Push Notification
        /// <CreatedFor>Send Notification when manager login through mobile</CreatedFor>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>March-23-2015</CreatedOn>
        /// </summary>
        /// <param name="objServicePushModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> SendNotification(ServicePushModel obj)
        {
            ServiceResponseModel<string> ObjServiceResponseModel= new ServiceResponseModel<string>();
            ObjUserRepository = new UserRepository();
            string message = "Hello Folks ! This is testing message";
            try
            {
                var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == obj.ServiceAuthKey && x.IsDeleted == false);
                if (authuser != null && authuser.UserId > 0)
                {

                    if(obj.DeviceId !=null )
                    {
                        var result = PushNotification.GCMAndroid(message,obj.DeviceId,obj.ServiceAuthKey);
                        if(result== "success")
                        {
                            ObjServiceResponseModel.Message = CommonMessage.Successful();
                            ObjServiceResponseModel.Response =  Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);

                        }
                        else
                        {

                            ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                            ObjServiceResponseModel.Message = CommonMessage.InvalidEntry();
                        }
                    }
                    else
                    {

                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                    }
                }
                else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.InvalidSessionResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                    }
                
            }
            catch (Exception)
            { throw; }

            return ObjServiceResponseModel;
        }
        
    }
}
