using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Helpers
{
    public class JQGridResults
    {
        public int page;
        public int total;
        public int records;
        public JQGridRow[] rows;
    }

    public class JQGridRow
    {
        public object id;
        public string[] cell;
    }

}
