using System;
using System.Collections.Generic;
using System.Linq;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class Common_B
    {
        public byte AssignRoleAndPermission(long userId, long userType, long locationId, long createdBy, string action)
        {
            byte result = 0;

            //this block is used for assigning roles to the users.. 
            {
                try
                {
                    if (locationId > 0)
                    {
                        using (workorderEMSEntities Context = new workorderEMSEntities())
                        {
                            var abc = Context.sp_permissionAssign(userId, userType, createdBy, locationId, "").FirstOrDefault();
                            if (abc.Result == 1 || abc.Result == 2)
                            {
                                result = 1;
                            }
                        }
                    }
                    else
                    {
                        Exception_B.Exception_B.exceptionHandel_Runtime(new Exception("we raising this exception in else condition because Here we getting locationid = 0 which is not acceptable, while assigning roles to user.."), "public byte AssignRoleAndPermission(long userId, long userType, long locationId, long createdBy, string action)", "This is not an Exception.", "UserId:-" + userId + ",userType:-" + userType + ",LocationId:-" + locationId);
                    }
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public byte AssignRoleAndPermission(long userId, long userType, long locationId, long createdBy, string action)", "Exception from c#", "UserId:-" + userId + "LocationId:-" + locationId);
                }
            }
            return result;
        }
   
        /// <summary>
        /// TO GET LOCATIONSERVICES BY LOCATIONID
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>2/4/2015</CreatedDate>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        public List<string> GetLocationServicesByLocationID(long LocationId, long UserType)
        {
            LocationServicesRepository obj_LocationServicesRepository = new LocationServicesRepository();
            return obj_LocationServicesRepository.GetLocationServicesByLocationID(LocationId, UserType);
        }



        /// <summary>
        /// Check vendor is already exists or not.
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public byte isVendorEmailExists(string Email)
        {

            byte result = 0;
            try
            {


                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    long re = 0;

                    if (Email != "")
                    {
                        re = (from o in Context.UserRegistrations
                              where o.UserEmail == Email.Trim()
                               && o.IsDeleted == false

                              select o.UserId).FirstOrDefault();

                    }

                    if (re > 0)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result = 0;

            }
            return result;

        }


    }


    public class SetGlobalPath
    {
        public string globalPath = System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"];
    }
}
