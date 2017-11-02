using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data
{
    public class VendorRepository : BaseRepository<VendorRegistration>, IVendorRepository
    {
        workorderEMSEntities _workorderEMSEntities;

        #region Comment proc call
        /*

        /// <summary>GetVendorDetails
        /// CreatedBy   :   Nagendra Upwanshi
        /// CreatedOn   :   Sep-05-2014
        /// CreatedFor  :   Get Vendor Details
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="textSearch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <returns></returns>
        public List<VendorModel> GetVendorDetails(long? UserID, string textSearch, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            List<VendorModel> lstVerifiedMnagaer = new List<VendorModel>();
            _workorderEMSEntities = new workorderEMSEntities();
            try
            {
                lstVerifiedMnagaer = _workorderEMSEntities.ssp_GetVendorDetails(UserID, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch).Select(t =>
                    new VendorModel()
                    {
                        VendorID = t.VendorID,
                        Address1 = t.Address1,
                        Address2 = t.Address2,
                        City = t.City,
                        CompanySize = t.CompanySize,
                        ContactName = t.ContactName,
                        CountryCodeDigit = t.CountryCodeDigit,
                        CountryId = t.CountryId,
                        CountryName = t.CountryName,
                        Fax = t.Fax,
                        IndustryName = t.IndustryName,
                        Mobile = t.Mobile,
                        OrganizationLogo = t.OrganizationLogo,
                        OrganizationName = t.OrganizationName,
                        Phone = t.Phone,
                        RowNo = t.RowNo,
                        RowNum = t.RowNum,
                        StateCode = t.StateCode,
                        StateId = t.StateId,
                        StateName = t.StateName,
                        TotalRows = t.TotalRows,
                        VendorEmail = t.VendorEmail,
                        VendorImage = t.VendorImage,
                        ZipCode = t.ZipCode
                    }).ToList();
                return lstVerifiedMnagaer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */
        #endregion Comment proc call


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
        public List<VendorDetailModel> GetVendorList(long? vendorID, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch)
        {
            try
            {
                _workorderEMSEntities = new workorderEMSEntities();
                var ss = _workorderEMSEntities.ssp_GetVendorDetail(vendorID, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch);

                List<VendorDetailModel> VendorList = _workorderEMSEntities.ssp_GetRegisterVendorDetail(vendorID, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch).Select(v =>
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
        public List<VendorListModel> GetAllRegisterVendorList(long? VendorID, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch)
        {
            try
            {
                _workorderEMSEntities = new workorderEMSEntities();
                //var ss = _workorderEMSEntities.ssp_GetRegisterVendorDetail(VendorID, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch);

                List<VendorListModel> VendorList = _workorderEMSEntities.ssp_GetRegisterVendorDetail(VendorID, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch).Select(v =>
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
                        TotalRows = v.TotalRows,
                        VendorEmail = v.VendorEmail,
                        VendorID = v.VendorID
                    }).ToList();
                return VendorList;
            }
            catch (Exception )
            { throw ; }
        }

        #endregion vendor registration new

    }
}
