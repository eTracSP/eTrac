using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data;
using WorkOrderEMS.Models.UserModels;

namespace WorkOrderEMS.BusinessLogic
{
    public class EmployeeManager : IEmployeeManager
    {
        /// <summary>
        /// TO GET FULL DETAIL FULL DETAIL OF THE EMPLOYEE 
        /// </summary>
        /// <CreatedBy>Manoj jaswal</CreatedBy>
        /// <CreatedDate>2015-03-18</CreatedDate>
        /// <param name="Loc_ID"></param>
        /// <returns></returns>
        public List<EmployeeModel> GetEmployeeByLocDetailed(long Loc_ID)
        {
            EmployeeLocationMappingRepository obj_EmployeeLocationMappingRepository=new EmployeeLocationMappingRepository();
           return obj_EmployeeLocationMappingRepository.GetEmployeeByLocDetailed(Loc_ID).Select(x => new EmployeeModel()
            {
                UserId = Convert.ToInt64(x.UserId),
                Password = x.Password,
                UserEmail = x.UserEmail,
                AlternateEmail = x.AlternateEmail,
                SubscriptionEmail = x.SubscriptionEmail,
                UserType = Convert.ToInt64(x.UserType),
                ProjectID = x.ProjectID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Gender = x.Gender,
                DOB = Convert.ToDateTime(x.DOB).ToString("MM/dd/yy"),
                ProfileImage = x.ProfileImage,
                IsLoginActive = Convert.ToBoolean(x.IsLoginActive),
                IsEmailVerify = Convert.ToBoolean(x.IsEmailVerify),
                TimeZoneId = x.TimeZoneId,
                VendorID = x.VendorID,
                BloodGroup = x.BloodGroup,
                HiringDate = x.HiringDate,
                EmployeeCategory = x.EmployeeCategory,
                QRCID = x.QRCID,
                EmployeeID = x.EmployeeID,
                JobTitle = x.JobTitle,
                DeviceId = x.DeviceId,
                DeviceType = x.DeviceType,
                ServiceAuthKey = x.ServiceAuthKey,
                EmployeeLocationMappingId = x.EmployeeLocationMappingId,
                EmployeeUserId = x.EmployeeUserId,
                LocationId = x.LocationId,
                AddressMasterId = x.AddressMasterId,
                Address1 = x.Address1,
                Address2 = x.Address2,
                City = x.City,
                StateId = x.StateId,
                CountryId = x.CountryId,
                Mobile = x.Mobile,
                Phone = x.Phone,
                ZipCode = x.ZipCode,
                IsCurrentAddress = x.IsCurrentAddress,
                IsPermanent = x.IsPermanent,
            }).ToList();
        }
    }
}
