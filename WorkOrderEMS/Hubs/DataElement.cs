using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkOrderEMS.Hubs
{
    //Created By: Bhushan Dod on 13/04/2016
    //This class for units of chart data for the dashboard applications
    public class DataElement
    {

        public string ElementName { get; set; }
        public string ElementDataJSON { get; set; }

        /// <summary>
        /// Created By : Bhushan on 13/04/2016
        /// Default class constructor declaration
        /// </summary>
        public DataElement()
            : this(null, null)
        {

        }

        /// <summary>
        /// Created By : Bhushan on 13/04/2016
        /// Default class constructor definition
        /// </summary>
        public DataElement(string name, string json)
        {
            ElementName = name;
            ElementDataJSON = json;
        }

    }
}