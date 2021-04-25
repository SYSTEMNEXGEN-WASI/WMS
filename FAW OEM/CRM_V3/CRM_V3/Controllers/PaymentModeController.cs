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
    public class PaymentModeController : Controller
    {
        // GET: PaymentMode
        static string dealerCode = string.Empty;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PaymentMode()
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

        //PaymentMode/Select_PaymentModeDetails/AbdusSaboor
        [HttpGet]
        public JsonResult Select_PaymentModeDetails(string dealer)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = PaymentModeMethods.Get_PaymentMode(dealer);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        //for Save Insert_PaymentMode/AbdusSaboor
        public JsonResult Insert_PaymentMode(PaymentModeVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = PaymentModeMethods.Insert_PaymentMode(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
        //for delete Delete_PaymentMode/AbdusSaboor
        public JsonResult Delete_PaymentMode(PaymentModeVM objects)
        {

            bool result = false;
            string msg = "'Failed' to Delete Record...";

            result = PaymentModeMethods.Delete_PaymentMode(objects, ref msg);

            if (result)
            {
                msg = "Record Deleted Successfully";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }


    }
}