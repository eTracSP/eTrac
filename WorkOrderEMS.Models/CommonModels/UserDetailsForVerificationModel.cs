using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    /// <summary>
    /// <CreatedBy>UserDetailsForVerificationModel</CreatedBy>
    /// <CreatedOn>Dec-23-2014</CreatedOn>    
    /// </summary>
    public class UserDetailsForVerificationModel
    {
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public string EmailAddress { get; set; }
        public long UserType { get; set; }
        public string ClientFName { get; set; }
        public string ClientLName { get; set; }
        public long ClientId { get; set; }
        public long mappintId { get; set; }


    }
}
