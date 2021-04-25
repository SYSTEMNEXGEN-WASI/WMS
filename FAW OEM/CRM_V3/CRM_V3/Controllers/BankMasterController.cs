using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using CRM_V3.assets;
using Microsoft.Reporting.WebForms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM_V3.Controllers
{
    public class BankMasterController : Controller
    {
        // GET: BankMaster
        static string dealerCode = string.Empty;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BankMaster()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            ViewBag.ddlAllDealer = ddlAllDealer;

            List<SelectListItem> ddlTaxType = new List<SelectListItem>();
            ddlTaxType = GeneralMethods.GetDataFromSp("Get_TaxType");
            ViewBag.ddlTaxType = ddlTaxType;


            return View();

        }

        public JsonResult Select_BankMasterDetail(string dealer)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = BankMasterMethods.Get_BankMaster(dealer);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        //for Save BankMaster
        public JsonResult Insert_BankMaster(BankMasterVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = BankMasterMethods.Insert_BankMaster(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }

        //for delete BankMaster/AbdusSaboor
        public JsonResult Delete_BankMaster(BankMasterVM objects)
        {

            bool result = false;
            string msg = "'Failed' to Delete Record...";

            result = BankMasterMethods.Delete_BankMaster(objects, ref msg);

            if (result)
            {
                msg = "Record Deleted Successfully";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
    }
}