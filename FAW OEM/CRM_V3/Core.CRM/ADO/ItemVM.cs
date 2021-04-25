using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO
{
  public  class ItemVM
    {
        public string DealerCode { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public string ItemCatCode { get; set; }
        public string ItemCatDesc { get; set; }
        public string Quantity { get; set; }

        public string UpdUser { get; set; }
        public string UpdDate { get; set; }
        public string UpdTime { get; set; }
        public string UpdTerm { get; set; }
        public string SaleRate { get; set; }
        public string PurchaseRate { get; set; }
    }
}
