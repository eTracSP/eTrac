using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class CountryModel
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string CountryCodeDigit { get; set; }
        public string CountryCode { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    }
    public class StateModel
    {
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public List<StateModel> Data { get; set; }
        public int FrStateId { get; set; }
    
    }
       
}
