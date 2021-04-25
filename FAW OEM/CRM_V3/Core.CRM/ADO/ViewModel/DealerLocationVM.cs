using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class DealerLocationVM
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

        //For Dealer Table
        public string DealerDesc { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string SaleTaxNo { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string RegionCode { get; set; }
        public string NTN { get; set; }
        public string AreaOfficeCode { get; set; }
        public string CityCode { get; set; }
        public string PST { get; set; }
        public string CreditLimit { get; set; }
        public string ParentDesc { get; set; }
        public string ParentCode { get; set; }
        public string Image { get; set; }
        public string StateCode { get; set; }
        public string CountryCode { get; set; }
        public string LicenseNo { get; set; }
        public string FacilityCode { get; set; }
        public string ContactPerson { get; set; }
        public string CurrencyCode { get; set; }
        public string DealerCatCode { get; set; }
        public string DealershipTypeCode { get; set; }
        public string DealerAdminId { get; set; }
        public string OperatedBy { get; set; }
        public string HandlingCharges { get; set; }
        public string VehicleCategory { get; set; }
        public string DealerRefCode { get; set; }
        public string PartRequisition { get; set; }


    }
}
