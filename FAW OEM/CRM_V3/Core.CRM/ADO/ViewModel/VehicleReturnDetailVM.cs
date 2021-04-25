﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO.ViewModel
{
    public class VehicleReturnDetailVM
    {
        public string DealerCode { get; set; }
        public string ReturnCode { get; set; }
        public string Type { get; set; }
        public string BrandCode { get; set; }
        public string BrandDesc { get; set; }
        public string ProdCode { get; set; }
        public string ProdDesc { get; set; }
        public string VersionCode { get; set; }
        public string EngineNo { get; set; }
        public string ChasisNo { get; set; }
        public string BookRefNo { get; set; }
        public string BookRefDate { get; set; }
        public string CusCode { get; set; }
        public string CusDesc { get; set; }
        public string ColorCode { get; set; }
        public string ColorDesc { get; set; }
        public string BookingNo { get; set; }
    }

    public class VRDetailResponseModel
    {
        public VRDetailResponseModel()
        {
            VRDetailList = new List<VehicleReturnDetailVM>();
        }

        public List<VehicleReturnDetailVM> VRDetailList { get; set; }

    }
}
