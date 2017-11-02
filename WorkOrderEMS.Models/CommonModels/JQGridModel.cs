using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.CommonModels
{
   public class JQGridModel<T>
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<T> rows { get; set; }
    }
}
