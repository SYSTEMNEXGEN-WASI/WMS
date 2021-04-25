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
    public class CustomerProfileDataController : Controller
    {
        static string dealerCode = string.Empty;



        //
        // GET: /CtomerProfileData/
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");

            }
            dealerCode = Session["DealerCode"].ToString();
            return View();
        }
        public ActionResult CustomerProfileData(string CusCode)
        {
            List<SelectListItem> ddlCusType = new List<SelectListItem>();

            ddlCusType = GeneralMethods.GetDataFromSpWithDealerCode("SP_SelectCustomerType", dealerCode);
            ViewBag.CusType = ddlCusType;
            


            List<SelectListItem> ddlCountry = new List<SelectListItem>();
            ddlCountry = GeneralMethods.GetDataFromSp("Select_Country");
            ViewBag.Country = ddlCountry;
            


            List<SelectListItem> ddlState = new List<SelectListItem>();
            ddlState = GeneralMethods.GetDataFromSp("Select_State");
            ViewBag.State = ddlState;
            

            List<SelectListItem> ddlCity = new List<SelectListItem>();
            ddlCity = GeneralMethods.GetDataFromSp("Select_City");
            ViewBag.City = ddlCity;

            ViewBag.Customer = CusCode;

            return View();
        }

        public JsonResult Select_CustomerDetail(string CusCode)
        {
            string data = "";
            bool result = false;
            data = CustomerProfileMethod.Get_CustomerProfileDetails(CusCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_CustomerDetails()
        {
            string data = "";
            bool result = false;
            data = CusProfileMethods.Get_CustomerDetails();

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Insert_CustomerDetail(CustomerProfileVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = CustomerProfileMethod.Insert_CustomerDetailData(objects);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        
    }
}