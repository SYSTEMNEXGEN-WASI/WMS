using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class SalesPersonPerformanceVM
    {
        public int GenerateLeads { get; set; }
        public int Converted { get; set; }
        public int Lost { get; set; }
        public int FollowUp { get; set; }
        public int Task { get; set; }
        public int DeliveryOrder { get; set; }
        public int BookingOrder { get; set; }
        public int Stock { get; set; }
        public int Invoice { get; set; }
        public int PendingInvoice { get; set; }
        public int PendingPayment { get; set; }
        public int JobCardCount { get; set; }
        public int PartSales { get; set; }
        public int CountOfInvoice { get; set; }
        public int CashAmount { get; set; }
        public int OtherAmount { get; set; }
        public int SparePartsStock { get; set; }

        public int LocalStock { get; set; }

        public int OtherStock { get; set; }
        public int DTRCount { get; set; }
        public int WarrantyCount { get; set; }


    }

 
}
