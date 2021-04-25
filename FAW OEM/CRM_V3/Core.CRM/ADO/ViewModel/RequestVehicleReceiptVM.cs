﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class RequestVehicleReceiptVM
    {
        public string RecNo { get; set; }
        public string RecDate     { get; set; }
	    public string DocumentNo  { get; set; }
	    public string VehTypeCode { get; set; }
	    public string VehRecCode  { get; set; }
	    public string Segment     { get; set; }
	    public string Category    { get; set; }
	    public string Usage       { get; set; }
	    public string BrandCode   { get; set; }
        public string BrandDesc { get; set; }
        public string ProdCode    { get; set; }
        public string ProdDesc { get; set; }
        public string VersionCode { get; set; }
	    public string InvoiceNo   { get; set; }
	    public string InvoiceDate { get; set; }
	    public string RegNo       { get; set; }
	    public string RegDate     { get; set; }
	    public string ChasisNo    { get; set; }
	    public string EngineNo    { get; set; }
	    public string ColorCode   { get; set; }
        public string ColorDesc { get; set; }
        public string StockType   { get; set; }
	    public string Insurance   { get; set; }
	    public int? Milage { get; set; }
        public string CusCode { get; set; }
        public string CusDesc { get; set; }
        public string BookingNo { get; set; }
        public string LocCode { get; set; }
        public string LocDesc { get; set; }
        public string VendorCode { get; set; }
    }
}
