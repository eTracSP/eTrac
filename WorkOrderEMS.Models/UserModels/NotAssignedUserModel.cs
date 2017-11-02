using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.UserModels
{
    public class NotAssignedUserModel
    {
        public Nullable<long> RN { get; set; }
        public string CodeName { get; set; }
        public Nullable<long> GlobalCodeId { get; set; }
        public Nullable<long> UserId { get; set; }
        public string UserEmail { get; set; }
        public string Name { get; set; }
        public Nullable<long> Gender { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string ProfileImage { get; set; }
        public Nullable<bool> IsLoginActive { get; set; }
    }
}
