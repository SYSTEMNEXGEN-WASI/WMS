using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Core.CRM.ADO.ViewModel
{
   public class OEMJobCategoryVM
    {

public string DealerCode  { get; set; }
public string JobCatCode  { get; set; }
[Required]
[StringLength(100, ErrorMessage = "invalid already exist", MinimumLength = 3)]

        public string JobDesc { get; set; }
        public string JobCode { get; set; }
        public string JobTypeCode { get; set; }
        public string JobTypeDesc { get; set; }
        public string Unchanged { get; set; }
        public string JobCatDesc  { get; set; }
        public string AccountCode { get; set; }
        public string UpdUser     { get; set; }
        public string UpdDate     { get; set; }
        public string UpdTerm     { get; set; }
        public string BankCode { get; set; }
        public string BankDesc { get; set; }
        public string UpdTime { get; set; }
        public string BankStatus { get; set; }
        public string OEMFinance { get; set; }
        public string CityCode { get; set; }
        public string CityDesc { get; set; }
        public string StateCode { get; set; }
        public string CountryCode { get; set; }
        public string StateDesc { get; set; }
        



    }
}
