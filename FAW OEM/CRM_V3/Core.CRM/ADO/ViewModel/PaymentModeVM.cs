using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class PaymentModeVM
    {
        //For Schedule and Defult Jobs Table
        public string DealerCode { get; set; }
        public string PayModeCode { get; set; }
        public string PayModeDesc { get; set; }
        public string UpdUser { get; set; }
        public string UpdDate { get; set; }
        public string UpdTime { get; set; }
        public string UpdTerm { get; set; }
        public string Version { get; set; }
        public string PayModeCodeOEM { get; set; }
   
    }
}
