using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class JobCardHistoryVM
    {

        public string RegNo { get; set; }
		public string ChassisNo { get; set; }
		public string EngineNo { get; set; }
		public string CusCode { get; set; }
		public string CusDesc { get; set; }
		public string ProdCode { get; set; }
		public string VersionCode { get; set; }

        public string ProdTitle { get; set; }
    }
}
