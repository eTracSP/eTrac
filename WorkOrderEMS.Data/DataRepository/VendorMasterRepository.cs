using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;

namespace WorkOrderEMS.Data
{
    public class VendorMasterRepository : BaseRepository<VendorMaster>, IVendorMasterRepository
    {
        workorderEMSEntities _workorderEMSEntities;


        /// <summary>GetAllVendorList
        /// CreatedBy   :   Nagendra Upwanshi
        /// CreatedOn   :   Sep-22-2014
        /// CreatedFor  :   Get All Vendor List
        /// </summary>
        /// <param name="VendorID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<VendorListModel> GetAllVendorList(long? VendorID, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch)
        {
            try
            {
                _workorderEMSEntities = new workorderEMSEntities();
                //var ss = _workorderEMSEntities.ssp_GetVendorDetail(VendorID, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch);

                List<VendorListModel> VendorList = _workorderEMSEntities.ssp_GetVendorDetail(VendorID, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch).Select(v =>
                    new VendorListModel()
                    {
                        //EncryptVendor = Cryptography.GetEncryptedData(v.VendorID.ToString(), true),
                        ContactName = v.ContactName,
                        Address1 = v.Address1,
                        Address2 = v.Address2,
                        City = v.City,
                        Country = v.Country,
                        IndustryName = v.IndustryName,
                        JoinFrom = v.JoinFrom,
                        OrganizationName = v.OrganizationName,
                        RowNo = v.RowNo,
                        State = v.State,
                        TotalRows = v.TotalRows,
                        VendorEmail = v.VendorEmail,
                        VendorID = v.VendorID,
                        BuisnessNo = v.BusinessNo,
                        Mobile = v.MobilePOC,
                        ZipCode = v.ZipCode

                    }).ToList();
                return VendorList;
            }
            catch (Exception)
            { throw; }
            //*/
            //return new List<QRCListModel>();
        }



        /// <summary>GetVendorList
        /// CreatedBy   :   Nagendra Upwanshi
        /// CreatedOn   :   Sep-22-2014
        /// CreatedFor  :   Get All Vendor List
        /// </summary>
        /// <param name="VendorID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<VendorDetailModel> GetVendorList(long? vendorID, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, int? status, long? userId, long? locationId)
        {
            try
            {
                _workorderEMSEntities = new workorderEMSEntities();
                var ss = _workorderEMSEntities.ssp_GetVendorDetail(vendorID, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch);

                List<VendorDetailModel> VendorList = _workorderEMSEntities.ssp_GetRegisterVendorDetail(vendorID, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, status, locationId, userId).Select(v =>
                    new VendorDetailModel()
                    {
                        EncryptVendor = Cryptography.GetEncryptedData(v.VendorID.ToString(), true),
                        ContactName = v.ContactName,
                        //CompanyAddress = v.Address1 + " " + v.City + " " + v.State,
                        //BillingAddress = v.Address2 + " " + v.Country,
                        IndustryName = v.IndustryName,
                        JoinFrom = v.JoinFrom,
                        CompanyName = v.OrganizationName,
                        RowNo = v.RowNo,
                        TotalRows = v.TotalRows,
                        VendorEmail = v.VendorEmail,
                        VendorID = v.VendorID
                    }).ToList();
                return VendorList;
            }
            catch (Exception)
            { throw; }
            //*/
            //return new List<QRCListModel>();
        }

        #region vendor registration new

        /// <summary>GetAllRegisterVendorList
        /// CreatedBy   :   Nagendra Upwanshi
        /// CreatedOn   :   Sep-29-2014
        /// CreatedFor  :   Get All Vendor List
        /// </summary>
        /// <param name="VendorID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public List<VendorListModel> GetAllRegisterVendorList(long? VendorID, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, int? status = 244, long? userId = 0, long? locationId = 0)
        {
            try
            {
                _workorderEMSEntities = new workorderEMSEntities();
                //var ss = _workorderEMSEntities.ssp_GetRegisterVendorDetail(VendorID, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch);

                List<VendorListModel> VendorList = _workorderEMSEntities.ssp_GetRegisterVendorDetail(VendorID, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, status, locationId, userId).Select(v =>
                    new VendorListModel()
                    {
                        EncryptVendor = Cryptography.GetEncryptedData(v.VendorID.ToString(), true),
                        ContactName = v.ContactName,
                        Address1 = v.Address1,
                        Address2 = v.Address2,
                        City = v.City,
                        Country = v.Country,
                        IndustryName = v.IndustryName,
                        JoinFrom = v.JoinFrom,
                        OrganizationName = v.OrganizationName,
                        RowNo = v.RowNo,
                        State = v.State,
                        BuisnessNo = v.BusinessNo,
                        TotalRows = v.TotalRows,
                        VendorEmail = v.VendorEmail,
                        VendorID = v.VendorID,
                        DBA = v.DBA,
                        OwnerRegisterdAgent = v.OwnerRegisterdAgent,
                        MobilePOC = v.MobilePOC,
                        TaxIdNumber = v.TaxIdNumber,
                        PhoneNumber = v.Phone,
                        ZipCode = v.ZipCode,
                        Address21 = v.Address12,
                        City2 = v.City2,
                        State2 = v.State2,
                        ZipCode2 = v.zipcode2,
                        LocationName = v.LocationName
                    }).ToList();
                return VendorList;
            }
            catch (Exception)
            { throw; }
        }

        #endregion vendor registration new
        /// <summary>
        /// TO GET THE ALL VENDOR LIST FILTERED BY STATUS
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>18 April 2015</CreatedDate>
        /// <param name="pageIndex"></param>
        /// <param name="TotalRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="textSearch"></param>
        /// <param name="locationId"></param>
        /// <param name="userId"></param>
        /// <param name="Statusfilter"></param>
        /// <param name="paramTotalRecords"></param>
        /// <returns></returns>
        public List<AllVendorDetailModel> GetAllVendorList(int? pageIndex, int? TotalRows, string sortColumnName, string sortOrderBy, string textSearch, long locationId, long userId, string Statusfilter, ObjectParameter paramTotalRecords)
        {
            try
            {
                _workorderEMSEntities = new workorderEMSEntities();
                List<AllVendorDetailModel> obj_AllVendorDetailModel = _workorderEMSEntities.SSP_GetAllVendorListByFilter(pageIndex, sortColumnName, sortOrderBy, TotalRows, textSearch, locationId, userId, Statusfilter, paramTotalRecords).Select(x => new AllVendorDetailModel()
                {
                    VendorID = x.VendorID,
                    CompanyName = x.CompanyName,
                    DBA = x.DBA,
                    ContactName = x.ContactName,
                    ManagerPOC = x.ManagerPOC,
                    MobilePOC = x.MobilePOC,
                    BusinessNo = x.BusinessNo,
                    IndustryName = x.IndustryName,
                    CompanyLogo = x.CompanyLogo,
                    CompanySize = x.CompanySize,
                    JoinFrom = x.JoinFrom,
                    UserId = x.UserId,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    POCEmail = x.POCEmail,
                    OwnerRegisterdAgent = x.OwnerRegisterdAgent,
                    IsApprovedByManager = x.IsApprovedByManager,
                    IsApprovedByClient = x.IsApprovedByClient,
                    CodeName = x.CodeName,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserEmail = x.UserEmail

                }).ToList();
                return obj_AllVendorDetailModel;
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// TO GET SINGLE VENDOR DETAIL BY VENDOR ID
        /// </summary>
        /// <CreatedBy>Manoj Jaswal</CreatedBy>
        /// <CreatedDate>4/20/2015</CreatedDate>
        /// <param name="VendorId"></param>
        /// <returns></returns>
        public AllVendorDetailModel GetVendorDetailById(long VendorId)
        {
            try
            {
                _workorderEMSEntities = new workorderEMSEntities();
                return _workorderEMSEntities.VendorMasters.Join(_workorderEMSEntities.UserRegistrations, vm => vm.UserId, ur => ur.UserId, ((vm, ur) => new { vm, ur }))
                     .Where(a => a.vm.VendorID == VendorId && a.vm.IsDeleted == false)
                     .Select(z => new AllVendorDetailModel()
                     {
                         VendorID = z.vm.VendorID,
                         CompanyName = z.vm.CompanyName,
                         DBA = z.vm.DBA,
                         ContactName = z.vm.ContactName,
                         ManagerPOC = z.vm.ManagerPOC,
                         MobilePOC = z.vm.MobilePOC,
                         BusinessNo = z.vm.BusinessNo,
                         IndustryName = z.vm.IndustryName,
                         CompanyLogo = z.vm.CompanyLogo,
                         //CompanySize = Convert.ToInt64(z.vm.CompanySize),
                         JoinFrom = z.vm.JoinFrom,
                         UserId = z.vm.UserId,
                         CreatedBy = z.vm.CreatedBy,
                         CreatedDate = z.vm.CreatedDate,
                         POCEmail = z.vm.POCEmail,
                         OwnerRegisterdAgent = z.vm.OwnerRegisterdAgent,
                         IsApprovedByManager = z.vm.IsApprovedByManager,
                         IsApprovedByClient = z.vm.IsApprovedByClient,
                         FirstName = z.ur.FirstName,
                         LastName = z.ur.LastName,
                         UserEmail = z.ur.UserEmail,
                         TaxIdNumber = z.vm.TaxIdNumber

                     }).SingleOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// To Get Total Counts of vendor Detail
        /// </summary>
        /// <createdBy>Manoj Jaswal</createdBy>
        /// <CreatedDate>2015-20-4</CreatedDate>
        /// <param name="userId"></param>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        public List<EmployeeWorkAssignmentCountModel> GetTotalCountsOfVendor(long userId, long LocationId)
        {
            try
            {
                _workorderEMSEntities = new workorderEMSEntities();
                return _workorderEMSEntities.SSP_GetTotalVendorActivationCount(userId, LocationId).Select(x => new EmployeeWorkAssignmentCountModel()
                 {
                     CodeName = x.CodeName,
                     Column1 = x.Total,
                 }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Permit Type

        /// <summary>GetPermitTypeForMail
        /// CreatedBy   :   Kartik Bhave
        /// CreatedOn   :   Aug-14-2015
        /// CreatedFor  :   To Get permit details for mail.
        /// </summary>
        /// <param name="VendorID"></param>
        /// <returns></returns>
        public List<PermitTypeMailModel> GetPermitTypeForMail(int VendorID)
        {
            try
            {
                _workorderEMSEntities = new workorderEMSEntities();
                //var ss = _workorderEMSEntities.ssp_GetRegisterVendorDetail(VendorID, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch);

                List<PermitTypeMailModel> result = _workorderEMSEntities.spp_PermitTypeMailData(VendorID).Select(v =>
                    new PermitTypeMailModel()
                    {
                        c350 = v.c350,
                        c351 = v.c351,
                        c352 = v.c352,
                        c353 = v.c353,
                        c354 = v.c354,
                        c355 = v.c355,
                    }).ToList();
                return result;
            }
            catch (Exception)
            { throw; }
        }

        #endregion

        #region Permit Type For Dropdown and price

        /// <summary>GetPermitType
        /// CreatedBy   :   Kartik Bhave
        /// CreatedOn   :   Aug-14-2015
        /// CreatedFor  :   To Get permit details 
        /// </summary>
        /// <param name="VendorId"></param>
        /// <param name="Fields"></param>
        /// <param name="PermitTypeID"></param>
        /// <param name="VehicleTypeID"></param>
        /// <returns></returns>
        public List<PermitTypeDDModel> GetPermitType(int VendorID, string Fields, int PermitTypeID = 0, int VehicleTypeID = 0)
        {
            try
            {
                _workorderEMSEntities = new workorderEMSEntities();

                List<PermitTypeDDModel> result = _workorderEMSEntities.spp_PermitTypeVendorwise(VendorID, Fields, PermitTypeID, VehicleTypeID).Select(v =>
                    new PermitTypeDDModel()
                    {
                        id = v.id,
                        codename = v.codename
                    }).ToList();
                return result;
            }
            catch (Exception)
            { throw; }
        }

        #endregion


    }

}
