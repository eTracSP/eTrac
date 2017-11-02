using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class LoginModel
    {
        public long UserID { get; set; }
        public long ProjectID { get; set; }
        public string EmployeeUserID { get; set; }
        //public long ClientUserID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public long UserType { get; set; }
        public string UserProfile { get; set; }
        public string ContactNo { get; set; }



        public string ProjectName { get; set; }
        public string ProjectLogo { get; set; }
        public string ProjectImage { get; set; }
    }
}
