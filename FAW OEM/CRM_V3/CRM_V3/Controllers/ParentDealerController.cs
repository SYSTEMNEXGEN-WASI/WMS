using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using CRM_V3.assets;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
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
    public class ParentDealerController : Controller
    {
        // GET: ParentDealer
        static string dealerCode = string.Empty;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ParentDealer()
        {

           
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            ViewBag.ddlAllDealer = ddlAllDealer;

            List<SelectListItem> ddlAllCompany = new List<SelectListItem>();
            ddlAllCompany = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllCompany", dealerCode);
            ViewBag.ddlAllCompany = ddlAllCompany;

            //List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            //ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            //ViewBag.ddlAllDealer = ddlAllDealer;

            //List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            //ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            //ViewBag.ddlAllDealer = ddlAllDealer;

            //List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            //ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            //ViewBag.ddlAllDealer = ddlAllDealer;

            //List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            //ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            //ViewBag.ddlAllDealer = ddlAllDealer;

            //List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            //ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            //ViewBag.ddlAllDealer = ddlAllDealer;

            //List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            //ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            //ViewBag.ddlAllDealer = ddlAllDealer;

            //List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            //ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            //ViewBag.ddlAllDealer = ddlAllDealer;
            return View();

        }

        //PaymentMode/Select_CompanyDetails/AbdusSaboor
        [HttpGet]
        public JsonResult Select_CompanyDetails(string dealer)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = ParentDealerMethods.Select_CompanyDetails(dealer);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        //PaymentMode/Select_ParentDealerDetails/AbdusSaboor
        [HttpGet]
        public JsonResult Select_ParentDealerDetails(string dealer)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = ParentDealerMethods.Select_ParentDealerDetails(dealer);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        //for Save Insert_ParentDealer/AbdusSaboor
        public JsonResult Insert_ParentDealer(ParentDealerVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = ParentDealerMethods.Insert_ParentDealer(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }

        //for delete Delete_ParentDealer/AbdusSaboor
        public JsonResult Delete_ParentDealer(ParentDealerVM objects)
        {

            bool result = false;
            string msg = "'Failed' to Delete Record...";

            result = ParentDealerMethods.Delete_ParentDealer(objects, ref msg);

            if (result)
            {
                msg = "Record Deleted Successfully";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
    }
}