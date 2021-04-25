using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class ParentDealerVM
    {
        //For ParentDealer Table
        public string DealerCode { get; set; }
        public string UpdUser { get; set; }
        public string UpdTime { get; set; }
        public string UpdTerm { get; set; }
        public string PDealerCode { get; set; }
        public string PDealerDesc { get; set; }
        public string Active { get; set; }

        //For Security Comapny Table
        public string CompanyCode { get; set; }
        public string Description { get; set; }
    }
}
