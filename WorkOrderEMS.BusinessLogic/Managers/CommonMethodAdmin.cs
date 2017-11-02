using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class CommonMethodAdmin : ICommonMethodAdmin
    {
        AdminCommonRepository ObjAdminCommonRepository;
        ManagerLocationMappingRepository objManagerLocationMappingRepository;
        EmployeeLocationMappingRepository objEmployeeLocationMappingRepository;

        /// <summary>AssignLocationToAdminUser
        /// <CreatedOn>Nov-17-2014</CreatedOn>
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="AdminUserId"></param>
        public void AssignLocationToAdminUser(long locationId, long adminUserId)
        {
            try
            {
                if (locationId > 0 && adminUserId > 0)
                {
                    ObjAdminCommonRepository = new AdminCommonRepository();
                    AdminLocationMapping ObjAdminLocationMapping;
                    ObjAdminLocationMapping = ObjAdminCommonRepository.GetSingleOrDefault(mapp => mapp.LocationId == locationId && mapp.AdminUserId == adminUserId && mapp.IsDeleted == false);

                    if (ObjAdminLocationMapping != null)
                    {
                        ObjAdminLocationMapping = new AdminLocationMapping();
                        ObjAdminLocationMapping.LocationId = locationId;
                        ObjAdminLocationMapping.AdminUserId = adminUserId;
                        ObjAdminLocationMapping.MappedBy = adminUserId;
                        ObjAdminLocationMapping.CreatedBy = adminUserId;
                        ObjAdminLocationMapping.CreatedOn = DateTime.UtcNow;
                        ObjAdminCommonRepository.Add(ObjAdminLocationMapping);
                    }
                }
                else
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(new Exception("You are not allowed to assgin admin user to location 0. Means Location 0 is not allowed"), "public void AssignLocationToAdminUser(long locationId, long adminUserId)", "Exception from c#", "adminUserId:-" + adminUserId + "," + "locationId:-" + locationId);
                }
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>RemoveLocationForAdminUser
        /// <CreatedOn>Nov-17-2014</CreatedOn>
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="AdminUserId"></param>
        public void RemoveLocationForAdminUser(long locationId, long adminUserId)
        {
            try
            {
                ObjAdminCommonRepository = new AdminCommonRepository();
                ObjAdminCommonRepository.DeleteAll(mapp => mapp.LocationId == locationId && mapp.AdminUserId == adminUserId && mapp.IsDeleted == false);
            }
            catch (Exception)
            { throw; }
        }


        ///
        /// Created By Gayatri To bind the drop 
        /// down Location on QRC setup page
        ///
        public List<SelectListItem> GetLocationByAdminId(long adminId)
        {
            LocationRepository _LocationRepository = new LocationRepository();
            try
            {
                List<SelectListItem> lstlocation = _LocationRepository.GetLocationByAdminId(adminId).Select(t => new SelectListItem()
                {
                    Text = t.LocationName,
                    Value = Convert.ToString(t.LocationId, CultureInfo.InvariantCulture)
                }).ToList();
                return lstlocation;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UserModelList> GetManagerByAdminId(long adminId)
        {
            UserRepository _UserRepository = new UserRepository();
            try
            {
                return _UserRepository.GetManagerByAdminId(adminId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>AssignLocationToManagerUser
        /// <CreatedOn>Sep-13-2016</CreatedOn>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="AdminUserId"></param>
        public void AssignLocationToManagerUser(long locationId, long managerUserId)
        {
            try
            {
                if (locationId > 0 && managerUserId > 0)
                {
                    objManagerLocationMappingRepository = new ManagerLocationMappingRepository();
                    ManagerLocationMapping ObjManagerLocationMapping;
                    ObjManagerLocationMapping = objManagerLocationMappingRepository.GetSingleOrDefault(mapp => mapp.LocationId == locationId && mapp.ManagerUserId == managerUserId && mapp.IsDeleted == false);

                    if (ObjManagerLocationMapping != null)
                    {
                        ObjManagerLocationMapping = new ManagerLocationMapping();
                        ObjManagerLocationMapping.LocationId = locationId;
                        ObjManagerLocationMapping.ManagerUserId = managerUserId;
                        ObjManagerLocationMapping.MappedBy = managerUserId;
                        ObjManagerLocationMapping.CreatedBy = managerUserId;
                        ObjManagerLocationMapping.CreatedOn = DateTime.UtcNow;
                        objManagerLocationMappingRepository.Add(ObjManagerLocationMapping);
                    }
                }
                else
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(new Exception("You are not allowed to assgin admin user to location 0. Means Location 0 is not allowed"), "public void AssignLocationToAdminUser(long locationId, long adminUserId)", "Exception from c#", "adminUserId:-" + managerUserId + "," + "locationId:-" + locationId);
                }
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>AssignLocationToEmployeeUser
        /// <CreatedOn>Sep-14-2016</CreatedOn>
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="AdminUserId"></param>
        public void AssignLocationToEmployeeUser(long locationId, long empUserId)
        {
            try
            {
                if (locationId > 0 && empUserId > 0)
                {
                    objEmployeeLocationMappingRepository = new EmployeeLocationMappingRepository();
                    EmployeeLocationMapping ObjEmployeeLocationMapping;
                    ObjEmployeeLocationMapping = objEmployeeLocationMappingRepository.GetSingleOrDefault(mapp => mapp.LocationId == locationId && mapp.EmployeeUserId == empUserId && mapp.IsDeleted == false);

                    if (ObjEmployeeLocationMapping != null)
                    {
                        ObjEmployeeLocationMapping = new EmployeeLocationMapping();
                        ObjEmployeeLocationMapping.LocationId = locationId;
                        ObjEmployeeLocationMapping.EmployeeUserId = empUserId;
                        // ObjEmployeeLocationMapping. = empUserId;
                        ObjEmployeeLocationMapping.CreatedBy = empUserId;
                        ObjEmployeeLocationMapping.CreatedOn = DateTime.UtcNow;
                        objEmployeeLocationMappingRepository.Add(ObjEmployeeLocationMapping);
                    }
                }
                else
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(new Exception("You are not allowed to assgin employee user to location 0. Means Location 0 is not allowed"), "public void AssignLocationToAdminUser(long locationId, long adminUserId)", "Exception from c#", "adminUserId:-" + empUserId + "," + "locationId:-" + locationId);
                }
            }
            catch (Exception)
            { throw; }
        }
    }
}
