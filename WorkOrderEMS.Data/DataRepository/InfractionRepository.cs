using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using WorkOrderEMS.Data.EntityModel;
//using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data
{
    public class InfractionRepository : BaseRepository<Infraction>
    {
        workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();

        public long SaveGtTrackerInfraction(ServiceQrcModelGtTrackerModel ObjServiceQrcModelGtTrackerModel)
        {
            Infraction ObjInfraction = new Infraction();
            try
            {
                ObjInfraction.EnforcementImage = ObjServiceQrcModelGtTrackerModel.EnforcementImage;
                ObjInfraction.InfractionType = ObjServiceQrcModelGtTrackerModel.InfractionType;
                ObjInfraction.IsCheckIn = ObjServiceQrcModelGtTrackerModel.IsCheckIn;
                ObjInfraction.CreatedBy = ObjServiceQrcModelGtTrackerModel.UserId;
                ObjInfraction.CreatedOn = DateTime.UtcNow;
                ObjInfraction.DeletedBy = null;
                ObjInfraction.DeletedOn = null;
                ObjInfraction.IsDeleted = false;
                ObjInfraction.LocationId = ObjServiceQrcModelGtTrackerModel.LocationId;
                ObjInfraction.ModifiedBy = null;
                ObjInfraction.ModifiedOn = null;
                ObjInfraction.Operation = ObjServiceQrcModelGtTrackerModel.Operation;
                ObjInfraction.QRCID = ObjServiceQrcModelGtTrackerModel.QrcId;
                ObjInfraction.Remarks = ObjServiceQrcModelGtTrackerModel.Remarks;
                ObjInfraction.VehicleId = ObjServiceQrcModelGtTrackerModel.VehicleId;
                // ObjInfraction. = ObjServiceQrcModelGtTrackerModel.;

                Add(ObjInfraction);
                long InfractionID = ObjInfraction.InfractionId;

                return InfractionID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>Send Email while infraction 
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>Send Email to manager and client while infraction </CreatedFor>
        /// <CreatedOn>April-10-2015</CreatedOn>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        public List<InfractionEmail> SendInfractionEmail(long LocationId, long vehicleId)
        {
            List<InfractionEmail> objList = new List<InfractionEmail>();
            try
            {
                using (workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities())
                {

                    objList = (from pd in _workorderEMSEntities.PermissionDetails
                               join mlm in _workorderEMSEntities.ManagerLocationMappings
                                                                   on pd.UserId equals mlm.ManagerUserId
                               join lm in _workorderEMSEntities.LocationMasters
                                                                   on mlm.LocationId equals lm.LocationId
                               join urm in _workorderEMSEntities.UserRegistrations
                                                                   on pd.UserId equals urm.UserId
                               join qm in _workorderEMSEntities.QRCMasters
                                                                   on LocationId equals qm.LocationId
                               join vr in _workorderEMSEntities.VehicleRegistrations
                                                                   on qm.QRCID equals vr.QRCID

                               join lcm in _workorderEMSEntities.LocationClientMappings on lm.LocationId equals lcm.LocationId
                               join urc in _workorderEMSEntities.UserRegistrations on lcm.ClientUserId equals urc.UserId
                               join gcpermit in _workorderEMSEntities.GlobalCodes
                                                                   on vr.PermitType equals gcpermit.GlobalCodeId
                               where
                               qm.LocationId == LocationId
                               && urm.IsDeleted == false
                               && mlm.LocationId == LocationId
                               && lcm.LocationId == LocationId
                               && (pd.PermissionId == 5 || pd.PermissionId == 192) //GT-Tracker
                               && pd.LocationId == LocationId
                               && vr.VehicleID == vehicleId
                               && urm.IsDeleted == false
                               && urc.IsDeleted == false
                               select new InfractionEmail()
                               {
                                   ClientEmail = urc.UserEmail,
                                   ClientName = urc.FirstName + " " + urc.LastName,
                                   ClientUserId = urc.UserId,
                                   LocationName = lm.LocationName,
                                   ManagerEmail = urm.UserEmail,
                                   ManagerName = urm.FirstName + " " + urm.LastName,
                                   ManagerUserId = urm.UserId,
                                   DriverName = vr.DriverName,
                                   LicenseNo = vr.LicenseNo,
                                   PermitType = gcpermit.CodeName,
                                   VehicleIdentificationNo = vr.VehicleIdentificationNo,
                                   VehicleMake = vr.VehicleMake,
                                   VehicleModel = vr.VehicleModel,
                                   ManagerDeviceId = urm.DeviceId,
                                   LocationID = mlm.LocationId
                               }).Distinct().ToList();
                }

            }
            catch (Exception) { throw; }
            return objList;
        }

        /// <summary>Send Email while infraction 
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedFor>Send Email to manager and client while infraction </CreatedFor>
        /// <CreatedOn>April-10-2015</CreatedOn>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        public List<InfractionEmail> SendInfractionEmailClient(long LocationId)
        {
            List<InfractionEmail> objList = new List<InfractionEmail>();
            try
            {
                using (workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities())
                {

                    objList = (from lcm in _workorderEMSEntities.LocationClientMappings
                               join urc in _workorderEMSEntities.UserRegistrations
                                                                 on lcm.ClientUserId equals urc.UserId
                               join lm in _workorderEMSEntities.LocationMasters
                                                                   on lcm.LocationId equals lm.LocationId

                               //join qm in _workorderEMSEntities.QRCMasters
                               //                                    on LocationId equals qm.LocationId
                               //join vr in _workorderEMSEntities.VehicleRegistrations
                               //                                    on qm.QRCID equals vr.QRCID

                               //join lcm in _workorderEMSEntities.LocationClientMappings on qm.LocationId equals lcm.LocationId                                                             
                               //join urc in _workorderEMSEntities.UserRegistrations on lcm.ClientUserId equals urc.UserId
                               //join gcpermit in _workorderEMSEntities.GlobalCodes
                               //                                     on vr.PermitType equals gcpermit.GlobalCodeId
                               where
                                   // qm.LocationId == LocationId &&
                               urc.UserType == 4 // client type
                               && urc.IsDeleted == false
                                   //  && mlm.LocationId == LocationId                               
                               && lcm.LocationId == LocationId
                               select new InfractionEmail()
                               {
                                   ClientEmail = urc.UserEmail,
                                   ClientName = urc.FirstName + " " + urc.LastName,
                                   ClientUserId = urc.UserId,
                                   LocationName = lm.LocationName,
                                   //ManagerEmail = urc.UserEmail,
                                   //ManagerName = urc.FirstName + " " + urc.LastName,
                                   //ManagerUserId = urc.UserId,
                                   // DriverName = vr.DriverName,
                                   //LicenseNo = vr.LicenseNo,
                                   //  PermitType = gcpermit.CodeName,
                                   //  VehicleIdentificationNo = vr.VehicleIdentificationNo,
                                   // VehicleMake = vr.VehicleMake,
                                   // VehicleModel = vr.VehicleModel,
                                   //ManagerDeviceId = urc.DeviceId,
                                   LocationID = lcm.LocationId
                               }).Distinct().ToList();
                }

            }
            catch (Exception) { throw; }
            return objList;
        }
        /// <summary>Send Email while infraction 
        /// <CreatedBy>Roshan Rahood</CreatedBY>
        /// <CreatedFor>Send Email to client while infraction </CreatedFor>
        /// <CreatedOn>April-27-2015</CreatedOn>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        public List<InfractionEmail> SendInfractionDetailsToClientEmail(long LocationId)
        {
            List<InfractionEmail> objList = new List<InfractionEmail>();
            try
            {
                using (workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities())
                {

                    objList = (from vr in _workorderEMSEntities.VehicleRegistrations
                               join qm in _workorderEMSEntities.QRCMasters on vr.QRCID equals qm.QRCID
                               join lm in _workorderEMSEntities.LocationMasters on qm.LocationId equals lm.LocationId
                               join lcm in _workorderEMSEntities.LocationClientMappings on qm.LocationId equals lcm.LocationId
                               join mlm in _workorderEMSEntities.ManagerLocationMappings on qm.LocationId equals mlm.LocationId
                               join urm in _workorderEMSEntities.UserRegistrations on mlm.ManagerUserId equals urm.UserId
                               join urc in _workorderEMSEntities.UserRegistrations on lcm.ClientUserId equals urc.UserId
                               join gcpermit in _workorderEMSEntities.GlobalCodes on vr.PermitType equals gcpermit.GlobalCodeId
                               join inf in _workorderEMSEntities.Infractions on vr.VehicleID equals inf.VehicleId
                               join infdetails in _workorderEMSEntities.InfractionLevelDetails on inf.InfractionId equals infdetails.InfractionId
                               join gcinf in _workorderEMSEntities.GlobalCodes on inf.InfractionType equals gcinf.GlobalCodeId
                               where qm.LocationId == LocationId && urc.IsDeleted == false && urm.IsDeleted == false
                               select new InfractionEmail()
                               {
                                   ClientEmail = urc.UserEmail,
                                   ClientName = urc.FirstName + " " + urc.LastName,
                                   ClientUserId = urc.UserId,
                                   LocationName = lm.LocationName,
                                   ManagerEmail = urm.UserEmail,
                                   ManagerName = urm.FirstName + " " + urm.LastName,
                                   ManagerUserId = urm.UserId,
                                   DriverName = vr.DriverName,
                                   LicenseNo = vr.LicenseNo,
                                   PermitType = gcpermit.CodeName,
                                   VehicleIdentificationNo = vr.VehicleIdentificationNo,
                                   VehicleMake = vr.VehicleMake,
                                   VehicleModel = vr.VehicleModel,
                                   InfractionType = gcinf.CodeName,
                                   InfractionSubmittedOn = inf.CreatedOn.ToString(),
                                   InfractionLevelDetailId = infdetails.InfractionLevelDetailsId
                               }).ToList();
                }

            }
            catch (Exception)
            {
                throw;
            }
            return objList;
        }

        /// <summary>Send Email while infraction 
        /// <CreatedBy>Roshan Rahood</CreatedBY>
        /// <CreatedFor>Send Email to client while infraction </CreatedFor>
        /// <CreatedOn>April-27-2015</CreatedOn>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        public List<InfractionEmail> SendInfractionApprovalDetails(long LocationId, long infractionId)
        {
            List<InfractionEmail> objList = new List<InfractionEmail>();
            try
            {
                using (workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities())
                {

                    objList = (from inf in _workorderEMSEntities.Infractions
                               join infdetails in _workorderEMSEntities.InfractionLevelDetails on inf.InfractionId equals infdetails.InfractionId
                               join gcinf in _workorderEMSEntities.GlobalCodes on inf.InfractionType equals gcinf.GlobalCodeId
                               join vr in _workorderEMSEntities.VehicleRegistrations on inf.VehicleId equals vr.VehicleID
                               join urm in _workorderEMSEntities.UserRegistrations on inf.CreatedBy equals urm.UserId
                               join urc in _workorderEMSEntities.UserRegistrations on vr.VendorUserID equals urc.UserId
                               join lc in _workorderEMSEntities.LocationMasters on inf.LocationId equals lc.LocationId
                               join gc in _workorderEMSEntities.GlobalCodes on vr.PermitType equals gc.GlobalCodeId
                               where inf.LocationId == LocationId
                               where infdetails.InfractionId == infractionId && urm.IsDeleted == false && urc.IsDeleted == false
                               select new InfractionEmail()
                               {
                                   ClientEmail = urc.UserEmail,
                                   ClientName = urc.FirstName + " " + urc.LastName,
                                   ClientUserId = urc.UserId,
                                   LocationName = lc.LocationName,
                                   ManagerEmail = urm.UserEmail,
                                   ManagerName = urm.FirstName + " " + urm.LastName,
                                   ManagerUserId = urm.UserId,
                                   DriverName = vr.DriverName,
                                   LicenseNo = vr.LicenseNo,
                                   PermitType = gc.CodeName,
                                   VehicleIdentificationNo = vr.VehicleIdentificationNo,
                                   VehicleMake = vr.VehicleMake,
                                   VehicleModel = vr.VehicleModel,
                                   InfractionType = gcinf.CodeName,
                                   InfractionSubmittedOn = inf.CreatedOn.ToString(),
                                   InfractionLevelDetailId = infdetails.InfractionLevelDetailsId
                               }).ToList();
                }

            }
            catch (Exception)
            {
                throw;
            }
            return objList;
        }



        public List<InfractionList_M> GetAllInfractionDetails(long? infractionId, long? locationId, string operationType, int? statusType, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords)
        {
            try
            {
                List<InfractionList_M> infractionlist = _workorderEMSEntities.SP_GetInfractionDetails(infractionId, locationId, operationType, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, statusType, totalRecords).Select(q =>
                    new InfractionList_M()
                    {
                        InfractionId = q.InfractionId,
                        InfractionType = q.InfractionType,
                        EnforcementImage = (q.EnforcementImage == null || q.EnforcementImage.Trim() == "") ? "defaultImage.png" : q.EnforcementImage,
                        Operation = q.Operation,
                        Name = q.Name,
                        DriverName = q.DriverName,
                        DriverProfilePic = q.DriverProfilePic,
                        LicenseNo = q.LicenseNo
                    }).ToList();

                return infractionlist;
            }
            catch (Exception)
            { throw; }
            //*/
            //return new List<QRCListModel>();
        }


        public List<InfractionEmail> GetAllEmailInfractionDetails(long? infractionLevelDetailId, string emailType, long? locid)
        {
            try
            {
                List<InfractionEmail> objEmailInfractionlist = _workorderEMSEntities.SP_GetEmailDetailsInfraction(infractionLevelDetailId, emailType, locid).Select(q =>
                     new InfractionEmail()
                     {
                         InfractionLevelDetailId = q.InfractionLevelDetailId,
                         InfractionType = q.InfractionType,
                         ManagerName = q.Manager_Name,
                         ManagerEmail = q.ManagerEmail,
                         ManagerUserId = q.ManagerUserId.Value,
                         ClientEmail = q.ClientEmail,
                         ClientName = q.Client_Name,
                         ClientUserId = Convert.ToInt64(q.ClientUserId),
                         VendorEmail = q.VendorEmail,
                         VendorName = q.vendor_Name,
                         VendorUserId = q.vendoruserId,
                         VehicleIdentificationNo = q.VehicleIdentificationNo,
                         LicenseNo = q.LicenseNo,
                         DriverName = q.DriverName,
                         PermitType = q.PermitType,
                         VehicleMake = q.VehicleMake,
                         VehicleModel = q.VehicleModel,
                         InfractionSubmittedOn = Convert.ToString(q.InfractionSubmittedOn),
                         LocationName = q.locationName,
                         InfractionLevel = q.InfractionLevel,
                         Comment = q.Comment,
                         TimeSpan = q.TimeSpan,
                         LocationID = q.LocationId,
                         SubmittedUserId = q.SubmittedUserId,
                         SubmittedName = q.Submitted_Name,
                         SubmittedEmail = q.SubmittedEmail
                     }).ToList();

                return objEmailInfractionlist;
            }
            catch (Exception)
            { throw; }
            //*/
            //return new List<QRCListModel>();
        }
    }
}
