using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;


namespace WorkOrderEMS.Data
{
    public class LocationRepository : BaseRepository<LocationMaster>, ILocationRepository
    {
        workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();
        //Created by Gayatri Pal
        //on 26-Aug-2014
        //To get list of all location
        public List<LocationListModel> GetAllLocationList(int LocationID, string OperationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter paramTotalRecords)
        {
            try
            {
                List<LocationListModel> lstLocation = _workorderEMSEntities.SP_GetAllLocation(LocationID, OperationName, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, paramTotalRecords).Select(l =>
                    new LocationListModel()
                    {
                        LocationId = l.LocationId,
                        LocationName = l.LocationName,
                        Address1 = l.Address1,
                        Address2 = l.Address2,
                        City = l.City,
                        StateId = l.StateId,
                        StateName = l.StateName,
                        CountryId = l.CountryId,
                        CountryName = l.CountryName,
                        PhoneNo = l.phoneno,
                        Mobile = l.Mobile,
                        ZipCode = l.zipcode,
                        Description = l.description

                    }).ToList();
                return lstLocation;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>GetListAllLocation
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Dec-04-2014</CreatedOn>
        /// <CreatedFor>Get All Location List</CreatedFor>
        /// </summary>
        /// <param name="LocationID"></param>        
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <param name="paramTotalRecords"></param>
        /// <returns></returns>
        public List<ListLocationModel> GetListAllLocation(int? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter paramTotalRecords)
        {
            try
            {
                List<ListLocationModel> lstLocation = _workorderEMSEntities.ssp_GetAllLocation(LocationId, textSearch, pageIndex, sortColumnName, sortOrderBy, numberOfRows, paramTotalRecords).Select(l =>
                    new ListLocationModel()
                    {
                        LocationId = l.LocationId,
                        LocationName = l.LocationName,
                        Address = l.Address,
                        LocationCode = l.LocationCode,
                        City = l.City,
                        StateId = l.StateId,
                        State = l.State,
                        CountryId = l.CountryId,
                        Country = l.Country,
                        PhoneNo = l.phoneno,
                        Mobile = l.Mobile,
                        ZipCode = l.zipcode,
                        Description = l.description,
                        LocationAdministrator = l.LocationAdministrator,
                        LocationManager = l.LocationManager,
                        LocationEmployee = l.LocationEmployee

                    }).ToList();
                return lstLocation;
            }
            catch (Exception)
            { throw; }
        }

        public List<LocationListModel> GetLocationByAdminId(long AdminId)
        {
            try
            {
                List<LocationListModel> lstLocation = (from l in _workorderEMSEntities.LocationMasters
                                                       join la in _workorderEMSEntities.AdminLocationMappings on l.LocationId equals la.LocationId
                                                       where la.AdminUserId == AdminId
                                                       && la.IsDeleted == false
                                                       && l.IsDeleted == false
                                                       select l).Select(t => new LocationListModel()
                                                       {
                                                           LocationId = t.LocationId,
                                                           LocationName = t.LocationName
                                                       }).ToList();
                return lstLocation;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<UserLocations> GetUserLocations(long UserType, long UserID)
        {
            try
            {
                return _workorderEMSEntities.Proc_GetUserLocations(UserType, UserID).Select(x => new UserLocations()
                 {
                     LocationID = x.LocationID,
                     LocationName = x.LocationName,
                 }).ToList(); ;
            }
            catch (Exception ex)
            { throw ex; }

        }
        /// <summary>
        /// TO GET DETAIL OF LOCATION
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>3 April 2015</CreatedDate>
        /// <param name="LocationID"></param>
        /// <returns></returns>
        public List<LocationDetailModel> LocationDetailByLocationID(long LocationID)
        {
            return _workorderEMSEntities.SP_GetLocationDetailByLocationID(LocationID).Select(x => new LocationDetailModel()
            {
                LocationId = x.LocationId,
                LocationName = x.LocationName,
                Description = x.Description,
                Address1 = x.Address1,
                Address2 = x.Address2,
                City = x.City,
                StateId = x.StateId,
                CountryId = x.CountryId,
                Mobile = x.Mobile,
                PhoneNo = x.PhoneNo,
                ZipCode = x.ZipCode,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedDate = x.ModifiedDate,
                IsDeleted = x.IsDeleted,
                DeletedBy = x.DeletedBy,
                DeletedDate = x.DeletedDate,
                QRCID = x.QRCID,
                LocationType = x.LocationType,
                LocationSubType = x.LocationSubType,
                IsVerifiedByManager = x.IsVerifiedByManager,
                IsVerifiedByClient = x.IsVerifiedByClient,
                LocationSubTypeDesc = x.LocationSubTypeDesc,
                LocationCountry = x.LocationCountry,
                LocationState = x.LocationState,
                ClientState = x.ClientState,
                ClientCountry = x.ClientCountry,
                ClientName = x.ClientName,
                ClientImage = x.ClientImage,
                ClientDOB = Convert.ToDateTime(x.ClientDOB).ToString("MM/dd/yyyy"),
                ClientEmail = x.ClientEmail,
                isEmailVerified = x.IsEmailVerify

            }).ToList();
        }
    }
}
