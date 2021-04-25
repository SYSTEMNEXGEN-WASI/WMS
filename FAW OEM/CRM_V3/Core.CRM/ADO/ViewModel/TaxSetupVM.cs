using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class TaxSetupVM
    {
        //For table TaxSetupMaster
        public string DealerCode { get; set; }
        public string TaxAppCode { get; set; }
        public string TaxAppDesc { get; set; }
        public string ChangeAble { get; set; }
        public string Applicable { get; set; }

        //For table TaxSetupDetail
        public string Part { get; set; }
        public string Sublet { get; set; }
        public string Labor { get; set; }
        public string BoughtOut { get; set; }
        public string TaxType { get; set; }
        public string GST { get; set; }
        public string FurTax { get; set; }
        public string ExtraTax { get; set; }
        public string PST { get; set; }
        public string Lubricant { get; set; }
        

         
    }
}
