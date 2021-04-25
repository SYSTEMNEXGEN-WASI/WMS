using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class CityMasterVM
    {
        // For City Table
        public string DealerCode { get; set; }
        public string CityCode { get; set; }
        public string CityDesc { get; set; }
        public string UpdUser { get; set; }
        public string UpdDate { get; set; }
        public string UpdTime { get; set; }
        public string UpdTerm { get; set; }
        public string StateCode { get; set; }
        // For STATE Table
        public string CountryCode { get; set; }
        public string StateDesc { get; set; }
            

    }
}
