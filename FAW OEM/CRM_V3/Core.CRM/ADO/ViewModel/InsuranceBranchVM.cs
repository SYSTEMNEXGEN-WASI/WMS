using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class InsuranceBranchVM
    {
        // FOR TABLE Branch
        public string DealerCode { get; set; }
        public string InsCompCode { get; set; }
        public string BranchCode { get; set; }
        public string BranchDesc { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string URL { get; set; }
        public string AccountCode { get; set; }
        public string AdvanceReceipt { get; set; }
        public string ContName { get; set; }
        public string ContPhone { get; set; }

        // FOR TABLE INSURANCE COMPANIES
        public string InsCompDescription { get; set; }

    }
}
