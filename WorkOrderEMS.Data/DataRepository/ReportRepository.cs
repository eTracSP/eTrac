using System;
using System.Collections.Generic;
using System.Linq;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data
{

    public class ReportRepository : IReportRepository
    {
        workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();

        public List<CleaningModel> NoCleaningDone()
        {
            List<CleaningModel> lstCleaning = new List<CleaningModel>();
            try
            {
                lstCleaning = (from o in _workorderEMSEntities.QRCMasters
                               join g in _workorderEMSEntities.GlobalCodes on o.QRCTYPE equals g.GlobalCodeId
                               where g.Category == "QRCTYPE" && o.QRCTYPE == 38
                               // group c by new { y=dt.Year, m=dt.Month,d=dt.Day, c.CommonItemID, o.ItemName,c.QRCID} into resultsSet
                               group o by new { o.QRCName, o.QRCID, g.CodeName } into resultsSet
                               orderby resultsSet.Key.QRCName
                               select new
                               {
                                   ItemName = resultsSet.Key.QRCName,
                                   CleaningCount = resultsSet.Count(),
                                   QRCID = resultsSet.Key.QRCID,
                                   QRCType = resultsSet.Key.CodeName
                               }
                                   ).ToList().Select(C => new CleaningModel
                                   {
                                       ItemName = C.ItemName,
                                       NoCleaning = C.CleaningCount,
                                       QRCID = C.QRCID,
                                       QRCType = C.QRCType
                                   }).ToList();
                return lstCleaning;
            }
            catch (Exception)
            {
                throw;
            }

        }

        //public List<TrashData> GetTrashData()
        //{
        //    List<TrashData> lstTrashData = new List<TrashData>();
        //    try
        //    {
        //        lstTrashData = (from o in _workorderEMSEntities.QRCMasters 
        //                        join gcQRCType in _workorderEMSEntities.GlobalCodes on o.QRCTYPE equals gcQRCType.GlobalCodeId
        //                        join gcVolumeLevel in _workorderEMSEntities.GlobalCodes on o.RCTrashVolumeLevel equals gcVolumeLevel.GlobalCodeId
        //                        where gcQRCType.Category == "QRCTYPE" && gcQRCType.CodeName == "Trash Can"
        //                        select new { ItemName = o.ItemName, TrashLevel = gcVolumeLevel.CodeName }
        //                             ).ToList().Select(r => new TrashData
        //                             {
        //                                 TrashName = r.ItemName,
        //                                 TrashLevel = r.TrashLevel == "Full" ?"100":r.TrashLevel
        //                             }).ToList();
        //        return lstTrashData;
        //    }
        //    catch (Exception )
        //    {                
        //        throw ;
        //    }
        //}

        //Created by Gayatri Pal
        //Creted on 03-Oct-2014
        //To get the work Order ISsued For the Project

        public List<WorkOrderIssueedModel> GetWorkOrderIssued(long ProjectId)
        {
            List<WorkOrderIssueedModel> lstWorkOrder = new List<WorkOrderIssueedModel>();
            try
            {
                lstWorkOrder = (from wo in _workorderEMSEntities.WorkRequestAssignments
                                //join wr in _workorderEMSEntities.WorkRequests on wo.WorkRequestID equals wr.WorkRequestID
                                join u in _workorderEMSEntities.UserRegistrations on wo.AssignToUserId equals u.UserId
                                join ur in _workorderEMSEntities.UserRegistrations on wo.RequestBy equals ur.UserId
                                join gc in _workorderEMSEntities.GlobalCodes on wo.PriorityLevel equals gc.GlobalCodeId
                                where wo.LocationID == ProjectId && u.IsDeleted == false && ur.IsDeleted == false
                                select new { TaskName = wo.Remarks, TaskPriority = gc.CodeName, AssignedToUser = u.FirstName + " " + u.LastName, RequestedBy = ur.FirstName + " " + ur.LastName, CreatedDate = wo.CreatedDate }
                                 ).ToList().Select(r => new WorkOrderIssueedModel()
                                 {
                                     TaskName = r.TaskName,
                                     TaskPriority = r.TaskPriority,
                                     AssignedToUser = r.AssignedToUser,
                                     RequestedBy = r.RequestedBy,
                                     IssuedDate = r.CreatedDate.ToShortDateString(),
                                 }).ToList();

                return lstWorkOrder;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>Get report on the number of work orders issued from each location
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedOn>March-09-2015</CreatedOn>
        /// <CreatedFor> GetWorkOrderIssued</CreatedFor>
        /// </summary>
        /// <param name="LocationID"></param>
        ///   /// <param name="FromDate"></param>
        /// <returns></returns>
        public List<WorkOrderIssueedModel> GetWorkOrderIssuedForLocation(long? LocationID, long? UserId, long LoginUserId, string FromDate, string ToDate, string WorkRequestProjectType, string textSearch)
        {
            List<WorkOrderIssueedModel> lstWorkOrder = new List<WorkOrderIssueedModel>();
            try
            {
                if (FromDate == null || FromDate.Trim() == "") FromDate = null;
                if (ToDate == null || ToDate.Trim() == "") ToDate = null;
                if (UserId == 0) UserId = null;

                lstWorkOrder = _workorderEMSEntities.sp_GetWorkOrderIssuedForLocation(LocationID, UserId, LoginUserId, FromDate, ToDate, WorkRequestProjectType, textSearch).Select(r => new WorkOrderIssueedModel()
                {
                    AssignBy = r.AssignBy,
                    AssignTo = r.AssignTo,
                    CreatedDate = r.CreatedDate,
                    PriorityLevel = r.PriorityLevel,
                    ProblemDesc = r.ProblemDesc,
                    ProjectDesc = r.ProjectDesc,
                    RequestBy = r.RequestBy,
                    WorkRequestStatus = r.WorkRequestStatus,
                    WorkRequestType = r.WorkRequestType,
                    CodeID = r.CodeID

                }).ToList();


                return lstWorkOrder;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>Get report on the number of work orders issued from each location
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedOn>March-10-2015</CreatedOn>
        /// <CreatedFor> GetWorkOrderIssuedAssignedForLocationItem</CreatedFor>
        /// </summary>
        /// <param name="LocationID"></param>
        ///   /// <param name="FromDate"></param>
        /// <returns></returns>
        public List<WorkOrderIssueedModel> GetWorkOrderIssuedForAssignedLocationItem(long LoginUserID, long? LocationID, string FromDate, string ToDate, long? QrcId, long? ReqType, int? WorkRequestProjectType, long? safetyHuzzard, long? priorityLevel, long? userId, string textSearch)
        {
            List<WorkOrderIssueedModel> lstWorkOrder = new List<WorkOrderIssueedModel>();
            try
            {
                if (QrcId == 0) { QrcId = null; }
                if (ReqType == 0) { ReqType = null; }
                if (WorkRequestProjectType == 0) { WorkRequestProjectType = null; }
                if (safetyHuzzard == 3) { safetyHuzzard = null; }
                if (priorityLevel == 0) { priorityLevel = null; }
                if (userId == 0) { userId = null; }
                lstWorkOrder = _workorderEMSEntities.sp_GetWorkOrderIssuedForAssignedLocationItem(LoginUserID, LocationID, FromDate, ToDate, QrcId, ReqType, WorkRequestProjectType, safetyHuzzard, priorityLevel, userId, textSearch).Select(r => new WorkOrderIssueedModel()
                {
                    QRCName = r.QRCName,
                    QRCTYPE1 = r.QRCTYPE,
                    AssignBy = r.AssignBy,
                    AssignTo = r.AssignTo,
                    CreatedDate = r.CreatedDate,
                    PriorityLevel = r.PriorityLevel,
                    ProblemDesc = r.ProblemDesc,
                    ProjectDesc = r.ProjectDesc,
                    RequestBy = r.RequestBy,
                    WorkRequestStatus = r.WorkRequestStatus,
                    WorkRequestType = r.WorkRequestType,
                    CodeID = r.CodeID
                }).ToList();


                return lstWorkOrder;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>Get report on the number of work orders issued from each location
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedOn>March-09-2015</CreatedOn>
        /// <CreatedFor> GetWorkOrderIssued</CreatedFor>
        /// </summary>
        /// <param name="LocationID"></param>
        ///   /// <param name="FromDate"></param>
        /// <returns></returns>
        public List<WorkOrderIssueedModel> GetWorkOrderIssuedListFixedTime(long LoginUserID, long? LocationID, string FromDate, string ToDate, int? WorkRequestProjectType, string textSearch)
        {
            List<WorkOrderIssueedModel> lstWorkOrder = new List<WorkOrderIssueedModel>();
            try
            {
                if (WorkRequestProjectType == 0)
                {
                    WorkRequestProjectType = null;
                }
                lstWorkOrder = _workorderEMSEntities.sp_GetWorkOrderTimeIssuedFixed(LoginUserID,LocationID, FromDate, ToDate, WorkRequestProjectType, textSearch).Select(r => new WorkOrderIssueedModel()
                {
                    AssignBy = r.AssignBy,
                    AssignTo = r.AssignTo,
                    CreatedDate = r.CreatedDate,
                    PriorityLevel = r.PriorityLevel,
                    ProblemDesc = r.ProblemDesc,
                    ProjectDesc = r.ProjectDesc,
                    RequestBy = r.RequestBy,
                    WorkRequestType = r.WorkRequestType,
                    StartTime = r.StartTime.ToString(),
                    EndTime = r.EndTime.ToString(),
                    FixedTime = r.FixedTime,
                    CodeID = r.CodeID
                }).ToList();

                return lstWorkOrder;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>Get report on Get Expiration Date for warabty and insurance
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedOn>May-04-2015</CreatedOn>
        /// <CreatedFor> GetExpirationDate</CreatedFor>
        /// </summary>
        /// <param name="LocationID,ExpirationType"></param>
        ///   /// <param name="FromDate"></param>
        /// <returns></returns>
        public List<QRCModel> GetExpirationDate(long? LocationID, int ExpirationType)
        {
            List<QRCModel> lstQRCList = new List<QRCModel>();
            try
            {

                lstQRCList = _workorderEMSEntities.sp_GetQrcForExpirationDate(LocationID, ExpirationType).Select(r => new QRCModel()
                {
                    QRCodeID = r.QrcId,
                    QRCName = r.QRCName,
                    QRCTYPECaption = r.QrcType,
                    VendorName = r.VendorName,
                    WExpDate = r.ExpiryDate,
                    UserName = r.CreatedBy,
                    CreatedOn = r.CreatedDate
                }).ToList();


                return lstQRCList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>Get report on the number of work orders In progress for each location
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedOn>June-06-2015</CreatedOn>
        /// <CreatedFor> GetWorkOrderInProgress</CreatedFor>
        /// </summary>
        /// <param name="LocationID"></param>
        ///   /// <param name="FromDate"></param>
        /// <returns></returns>
        public List<WorkOrderIssueedModel> GetWorkOrderInProgressForLocation(long LoginUserID, long? LocationID, string FromDate, string ToDate, int? WorkRequestProjectType, long? UserId, string textSearch)
        {
            List<WorkOrderIssueedModel> lstWorkOrder = new List<WorkOrderIssueedModel>();
            try
            {
                if (WorkRequestProjectType == 0)
                {
                    WorkRequestProjectType = null;
                }
                if (UserId == 0)
                {
                    UserId = null;
                }
                lstWorkOrder = _workorderEMSEntities.sp_GetWorkOrderInProgressForLocation(LoginUserID,LocationID, FromDate, ToDate, WorkRequestProjectType, UserId, textSearch).Select(r => new WorkOrderIssueedModel()
                {
                    AssignBy = r.AssignBy,
                    AssignTo = r.AssignTo,
                    CreatedDate = r.CreatedDate,
                    PriorityLevel = r.PriorityLevel,
                    ProblemDesc = r.ProblemDesc,
                    ProjectDesc = r.ProjectDesc,
                    RequestBy = r.RequestBy,
                    //WorkRequestStatus = r.WorkRequestStatus,
                    WorkRequestType = r.WorkRequestType,
                    CodeID = r.CodeID

                }).ToList();


                return lstWorkOrder;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>Get report on Work Order & Special Project Completions: (Work Orders accepted and completed)
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedOn>June-16-2015</CreatedOn>
        /// <CreatedFor> GetWorkOrderAcceptedandCompleted</CreatedFor>
        /// </summary>
        /// <param name="LocationID"></param>
        ///   /// <param name="FromDate"></param>
        /// <returns></returns>
        //public List<WorkOrderIssueedModel> GetWorkOrderAcceptedandCompleted(long? LocationID, string FromDate, string ToDate, int? WorkRequestProjectType, long? PriorityLevel, long? UserId, string textSearch)
        //{
        //    List<WorkOrderIssueedModel> lstWorkOrder = new List<WorkOrderIssueedModel>();
        //    try
        //    {
        //        if (WorkRequestProjectType == 0)
        //        {
        //            WorkRequestProjectType = null;
        //        }
        //        if (UserId == 0)
        //        {
        //            UserId = null;
        //        }
        //        if (PriorityLevel == 0) { PriorityLevel = null; }
        //        lstWorkOrder = _workorderEMSEntities.sp_GetWorkOrderAcceptedAndCompletedForMissedTime(LocationID, FromDate, ToDate, WorkRequestProjectType, PriorityLevel, UserId, textSearch).Select(r => new WorkOrderIssueedModel()
        //        {
        //            //AssignBy = r.AssignBy,
        //            AssignTo = r.AssignTo,
        //            CreatedDateTM = r.CreatedDate,
        //            PriorityLevel = r.PriorityLevel,
        //            ProblemDesc = r.ProblemDesc,
        //            ProjectDesc = r.ProjectDesc,
        //            RequestBy = r.RequestBy,
        //            //WorkRequestType = r.WorkRequestType,
        //            StartTime = r.StartTime.ToString(),
        //            EndTime = r.EndTime.ToString(),
        //            FixedTime = r.TimeFrame,
        //            MissedTime = r.MissedTime,
        //            AssignedTime = r.AssignedTime,
        //            CodeID = r.CodeID

        //        }).ToList();


        //        return lstWorkOrder;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>Modified for performance issue - Get report on Work Order & Special Project Completions: (Work Orders accepted and completed)
        ///// <CreatedBy>Bhushan Dod</CreatedBY>
        ///// <CreatedOn>June-16-2015</CreatedOn>
        ///// <CreatedFor> GetWorkOrderAcceptedandCompleted</CreatedFor>
        ///// </summary>Original method above
        ///// <param name="LocationID"></param>
        /////   /// <param name="FromDate"></param>
        ///// <returns></returns>
        public List<sp_GetWorkOrderAcceptedAndCompletedForMissedTime_Result> GetWorkOrderAcceptedandCompleted(long? LoginUserID, long? LocationID, string FromDate, string ToDate, int? WorkRequestProjectType, long? PriorityLevel, long? UserId, string textSearch)
        {
            try
            {
                if (WorkRequestProjectType == 0)
                {
                    WorkRequestProjectType = null;
                }
                if (UserId == 0)
                {
                    UserId = null;
                }
                if (PriorityLevel == 0) { PriorityLevel = null; }
                var tt = _workorderEMSEntities.sp_GetWorkOrderAcceptedAndCompletedForMissedTime(LoginUserID,LocationID, FromDate, ToDate, WorkRequestProjectType, PriorityLevel, UserId, textSearch).ToList();
                return tt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
