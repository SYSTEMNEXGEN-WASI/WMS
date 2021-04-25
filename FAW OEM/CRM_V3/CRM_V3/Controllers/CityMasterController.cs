using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using CRM_V3.assets;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace CRM_V3.Controllers
{
    public class CityMasterController : Controller
    {
        static string dealerCode = string.Empty;
        // GET: CityMaster
        public ActionResult Index()
        {
            return View();
        }

        //City Master /abdussaboor
        public ActionResult CityMaster()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            
            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            ViewBag.ddlAllDealer = ddlAllDealer;
            
            return View();

        }

        //for Get CityMasterDetail
        public JsonResult Select_CityMasterDetail(string dealer)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = CityMasterMethods.Get_CityMaster(dealer);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        //for Get CityMasterDetail
        public JsonResult Select_StateDetail(string dealer)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = CityMasterMethods.Get_State(DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        //for Save CityMaster
        public JsonResult Insert_CityMaster(CityMasterVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = CityMasterMethods.Insert_CityMaster(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
        
        //for Delete CityMaster
        public JsonResult Delete_CityMaster(CityMasterVM objects)
        {

            bool result = false;
            string msg = "Failed to Delete Record..";

            result = CityMasterMethods.Delete_CityMaster(objects, ref msg);

            if (result)
            {
                msg = "Record Deleted Sucessfully";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
    }
}