using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public class EmailDetailManager : IEmailDetail
    {

        EmailLogRepository objEmailLogRepository;
        CommonMethodManager objCommonMethodManager;

        public List<EmailLogModel> GetAllEmailList(long? emailId, long? locationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch, ObjectParameter totalRecords)
        {
            try
            {
                objEmailLogRepository = new EmailLogRepository();
                return objEmailLogRepository.GetAllEmailList(emailId, locationId, pageIndex, numberOfRows, sortColumnName, sortOrderBy, textSearch, totalRecords);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete the Rule.
        /// CreatedBy   :   Roshan Rahood
        /// CreatedOn   :   Feb 25 2015
        /// </summary>
        /// <param name="emailLogId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public Result DeleteEmail(long emailLogId, long loggedInUserId)
        {
            Result result;
            try
            {
                if (emailLogId > 0)
                {
                    if (true)
                    {
                        objEmailLogRepository = new EmailLogRepository();
                        objCommonMethodManager = new CommonMethodManager();

                        var data = objEmailLogRepository.GetSingleOrDefault(v => v.EmailLogId == emailLogId && v.IsDeleted == false);
                        data.IsDeleted = true;
                        data.DeletedBy = loggedInUserId;
                        data.DeletedOn = DateTime.UtcNow;
                        objEmailLogRepository.Update(data);

                        return Result.Delete;
                    }
                    else
                    { return Result.Failed; }
                }
                else { return Result.DoesNotExist; }
                return Result.Delete;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
