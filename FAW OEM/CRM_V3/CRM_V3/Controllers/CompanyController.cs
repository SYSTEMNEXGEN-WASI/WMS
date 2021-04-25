using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using CRM_V3.assets;
using Microsoft.Reporting.WebForms;
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
    public class CompanyController : Controller
    {
        // GET: Company
        static string dealerCode = string.Empty;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Company()
        {

            dealerCode = Session["DealerCode"].ToString();
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            ViewBag.ddlAllDealer = ddlAllDealer;

            return View();

        }
        //PaymentMode/Select_CompanyDetails/AbdusSaboor
        [HttpGet]
        public JsonResult Select_CompanyDetails()
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = CompanyMethods.Get_Company();

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        //for Save Insert_Company/AbdusSaboor
        public JsonResult Insert_Company(CompanyVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = CompanyMethods.Insert_Company(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
        //for delete Delete_Company/AbdusSaboor
        public JsonResult Delete_Company(CompanyVM objects)
        {

            bool result = false;
            string msg = "'Failed' to Delete Record...";

            result = CompanyMethods.Delete_Company(objects, ref msg);

            if (result)
            {
                msg = "Record Deleted Successfully";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
    }
}