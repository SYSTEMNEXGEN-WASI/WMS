using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class BankMasterVM
    {
        // For Bank Table
        public string DealerCode { get; set; }
        public string BankCode { get; set; }
        public string BankDesc { get; set; }
        public string UpdUser { get; set; }
        public string UpdDate { get; set; }
        public string UpdTime { get; set; }
        public string UpdTerm { get; set; }
        public string BankStatus { get; set; }
        public string OEMFinance { get; set; }
    }
}