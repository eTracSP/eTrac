using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;
using WorkOrderEMS.Data.Interfaces;
using System.Data.Entity;

namespace WorkOrderEMS.Data.DataRepository
{
    public class EmailLogRepository : BaseRepository<EmailLog>, IEmailLogRepository
    {
        workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();

        public static bool InsertEntitiesNew(string TableName, List<EmailLog> objList)
        {
            bool status = false;
            try
            {
                workorderEMSEntities context = new workorderEMSEntities();
                var cs = context.Database.Connection.ConnectionString;

                System.Data.SqlClient.SqlBulkCopy bulkInsert = new System.Data.SqlClient.SqlBulkCopy(cs);
                bulkInsert.DestinationTableName = TableName;

                using (MyGenericDataReader<EmailLog> dataReader = new
                                 MyGenericDataReader<EmailLog>(objList))
                {
                    bulkInsert.WriteToServer(dataReader);
                    status = true;
                }
            }
            catch (Exception)
            {
                status = false;
                throw;
            }                        
            return status;
        }

        /// <summary>Save Email Log
        /// <CreatedBy>Bhushan Dod </CreatedBY>
        /// <CreatedOn>Feb-17-2015</CreatedOn>
        /// <CreatedFor> Save entry in EmailLog for LocationVerification</CreatedFor>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>bool</returns>
        public bool SaveEmailLog(EmailLog obj)
        {
            bool IsSave = false;
            try
            {               
                _workorderEMSEntities.EmailLogs.Add(obj);
                _workorderEMSEntities.SaveChanges();
                IsSave = true;
            }

            catch (Exception)
            {
                throw;
            }

            return IsSave;
        }

        public async Task<bool> SaveEmailLogAsync(EmailLog obj)
        {
            bool IsSave = false;
            try
            {
                _workorderEMSEntities.EmailLogs.Add(obj);
                await _workorderEMSEntities.SaveChangesAsync();
                IsSave = true;
            }

            catch (Exception)
            {
                throw;
            }

            return IsSave;
        }
        public List<EmailLogModel> GetAllEmailList(long? emailId, long? locationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords)
        {
            try
            {
                List<EmailLogModel> emailList = _workorderEMSEntities.SP_GetAllEmial(emailId, locationId, pageIndex, sortColumnName, sortOrderBy, numberOfRows, textSearch, totalRecords).Select(q =>
                    new EmailLogModel()
                    {
                        EmailLogId = q.EmailLogId,
                        SentByUser = q.sent_By_User,
                        SentEmail = q.SentEmail,
                        SentBy = q.SentBy,
                        SentTo = q.SentTo,
                        Subject = q.Subject,
                        CreatedDate = (q.CreatedDate).ToString(),

                    }).ToList();
                return emailList;
            }
            catch (Exception)
            { throw; }
            //*/
            //return new List<QRCListModel>();
        }

        /// <summary>
        /// Created by :Bhushan Dod 
        /// Created Date :05/26/2015
        /// Alert on screen on admin and manager if any employee idle for 30 min
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<EmailToManagerModel> SendEmailToManagerForItemMissing(long LocationId, long UserId)
        {
            List<EmailToManagerModel> obj = new List<EmailToManagerModel>();
            try
            {
                List<EmailToManagerModel> lstManagers = (from mlm in _workorderEMSEntities.ManagerLocationMappings
                                                         join ur in _workorderEMSEntities.UserRegistrations on mlm.ManagerUserId equals ur.UserId
                                                         join lm in _workorderEMSEntities.LocationMasters on LocationId equals lm.LocationId
                                                         join urUserId in _workorderEMSEntities.UserRegistrations on UserId equals urUserId.UserId
                                                         where mlm.LocationId == LocationId
                                                         && ur.IsDeleted == false
                                                         select new EmailToManagerModel
                                                         {

                                                             LocationID = lm.LocationId,
                                                             LocationName = lm.LocationName,
                                                             ManagerEmail = ur.UserEmail,
                                                             ManagerName = !string.IsNullOrEmpty(ur.LastName) ? ur.FirstName + " " + ur.LastName : ur.FirstName,
                                                             ManagerUserId = ur.UserId,
                                                             RequestBy = urUserId.UserId,
                                                             UserName = !string.IsNullOrEmpty(urUserId.LastName) ? urUserId.FirstName + " " + urUserId.LastName : urUserId.FirstName,
                                                             DeviceId = ur.DeviceId,
                                                             IdleTimeLimit = urUserId.IdleTimeLimit
                                                         }).ToList();
                return lstManagers;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>GetDetails of manager to send email if checking out->item missing->false in QRC Vehicle
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>feb-25-2015</CreatedOn>
        /// <CreatedFor>Get manager list according to email</CreatedFor>
        /// </summary>
        public List<EmailToManagerModel> SendEmailToManagerForItemMissingQRC(long LocationId, long UserId)
        {
            List<EmailToManagerModel> obj = new List<EmailToManagerModel>();
            try
            {
                List<EmailToManagerModel> lstManagers = (from pd in _workorderEMSEntities.PermissionDetails
                                                         join mlm in _workorderEMSEntities.ManagerLocationMappings on pd.UserId equals mlm.ManagerUserId
                                                         join ur in _workorderEMSEntities.UserRegistrations on mlm.ManagerUserId equals ur.UserId
                                                         join lm in _workorderEMSEntities.LocationMasters on LocationId equals lm.LocationId
                                                         join urUserId in _workorderEMSEntities.UserRegistrations on UserId equals urUserId.UserId
                                                         where mlm.LocationId == LocationId && ur.IsDeleted == false
                                                                && ur.IsDeleted == false
                                                                && (pd.PermissionId == 3 || pd.PermissionId == 191) //eScanner
                                                                 && pd.LocationId == LocationId
                                                         select new EmailToManagerModel
                                                         {
                                                             LocationID = lm.LocationId,
                                                             LocationName = lm.LocationName,
                                                             ManagerEmail = ur.UserEmail,
                                                             ManagerName = !string.IsNullOrEmpty(ur.LastName) ? ur.FirstName + " " + ur.LastName : ur.FirstName,
                                                             ManagerUserId = ur.UserId,
                                                             RequestBy = urUserId.UserId,
                                                             UserName = !string.IsNullOrEmpty(urUserId.LastName) ? urUserId.FirstName + " " + urUserId.LastName : urUserId.FirstName,
                                                             DeviceId = ur.DeviceId
                                                         }).Distinct().OrderBy(x => x.ManagerUserId).ToList();
                return lstManagers;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>GetDetails of manager to send email if checking out->item missing->false in QRC Vehicle
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>feb-25-2015</CreatedOn>
        /// <CreatedFor>Get manager list according to email</CreatedFor>
        /// </summary>
        public List<EmailToManagerModel> SendEmailToEmployeeForFacilityRequest(long LocationId, long userId)
        {
            List<EmailToManagerModel> obj = new List<EmailToManagerModel>();
            try
            {
                List<EmailToManagerModel> lstManagers = (from mlm in _workorderEMSEntities.EmployeeLocationMappings
                                                         join ur in _workorderEMSEntities.UserRegistrations on mlm.EmployeeUserId equals ur.UserId
                                                         join lm in _workorderEMSEntities.LocationMasters on LocationId equals lm.LocationId
                                                         join urUserId in _workorderEMSEntities.UserRegistrations on userId equals urUserId.UserId
                                                         where mlm.LocationId == LocationId && ur.IsDeleted == false
                                                         select new EmailToManagerModel
                                                         {
                                                             LocationID = lm.LocationId,
                                                             LocationName = lm.LocationName + "," + lm.City,
                                                             ManagerEmail = ur.UserEmail,
                                                             ManagerName = !string.IsNullOrEmpty(ur.LastName) ? ur.FirstName + " " + ur.LastName : ur.FirstName,
                                                             ManagerUserId = ur.UserId,
                                                             RequestBy = urUserId.UserId,
                                                             UserName = !string.IsNullOrEmpty(urUserId.LastName) ? urUserId.FirstName + " " + urUserId.LastName : urUserId.FirstName,
                                                             DeviceId = ur.DeviceId
                                                         }).ToList();

                //          select new{mlm,ur,lm,urUserId} ).Select(u => new EmailToManagerModel()
                //{
                //    LocationID = u.,
                //    LocationName = !string.IsNullOrEmpty(u.LastName) ? u.FirstName + " " + u.LastName : u.FirstName,
                //    ManagerEmail = u.UserEmail,
                //    ManagerName = u.UserType,
                //    ManagerUserId = ,
                //    ProblemDesc = ,
                //    RequestBy = ,
                //    UserName =
                //}).ToList();
                return lstManagers;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>GetDetails of manager to send email for cellphone if screen cracked,button not present etc
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>March-13-2015</CreatedOn>
        /// <CreatedFor>Get manager list according to email</CreatedFor>
        /// </summary>
        public List<ssp_SendEmailForCellPhone_Result> SendEmailToManagerForCellphone(long QrcId, long UserId)
        {
            List<ssp_SendEmailForCellPhone_Result> obj = new List<ssp_SendEmailForCellPhone_Result>();
            try
            {
                obj = _workorderEMSEntities.ssp_SendEmailForCellPhone(QrcId, UserId).ToList();
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>Get Details of manager to send email of eFleet PreTrip/PostTrip done by employee.
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <CreatedOn>Sep-22-2017</CreatedOn>
        /// <CreatedFor>Get manager list according to email</CreatedFor>
        /// </summary>
        public async Task<List<EmailToManagerModel>> SendEmailToManagerForeFleetInspection(long LocationId, long UserId)
        {
            List<EmailToManagerModel> obj = new List<EmailToManagerModel>();
            try
            {
                List<EmailToManagerModel> lstManagers =  (from pd in _workorderEMSEntities.PermissionDetails
                                                         join mlm in _workorderEMSEntities.ManagerLocationMappings on pd.UserId equals mlm.ManagerUserId
                                                         join ur in _workorderEMSEntities.UserRegistrations on mlm.ManagerUserId equals ur.UserId
                                                         join lm in _workorderEMSEntities.LocationMasters on LocationId equals lm.LocationId
                                                         join urUserId in _workorderEMSEntities.UserRegistrations on UserId equals urUserId.UserId
                                                         where mlm.LocationId == LocationId && ur.IsDeleted == false
                                                                && ur.IsDeleted == false
                                                                && (pd.PermissionId == 3 || pd.PermissionId == 191) //eFleet
                                                                 && pd.LocationId == LocationId
                                                         select new EmailToManagerModel
                                                         {
                                                             LocationID = lm.LocationId,
                                                             LocationName = lm.LocationName,
                                                             ManagerEmail = ur.UserEmail,
                                                             ManagerName = !string.IsNullOrEmpty(ur.LastName) ? ur.FirstName + " " + ur.LastName : ur.FirstName,
                                                             ManagerUserId = ur.UserId,
                                                             RequestBy = urUserId.UserId,
                                                             UserName = !string.IsNullOrEmpty(urUserId.LastName) ? urUserId.FirstName + " " + urUserId.LastName : urUserId.FirstName,
                                                             DeviceId = ur.DeviceId
                                                         }).Distinct().OrderBy(x => x.ManagerUserId).ToList();
                return lstManagers;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
