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
    public class DealerLocationController : Controller
    {
        // GET: DealerLocation
        static string dealerCode = string.Empty;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DealerLocation()
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

            List<SelectListItem> ddlCity = new List<SelectListItem>();
            ddlCity = GeneralMethods.GetDataFromSp("Select_City");
            ViewBag.City = ddlCity;

            List<SelectListItem> ddlCountry = new List<SelectListItem>();
            ddlCountry = GeneralMethods.GetDataFromSp("Select_Country");
            ViewBag.Country = ddlCountry;

            List<SelectListItem> ddlState = new List<SelectListItem>();
            ddlState = GeneralMethods.GetDataFromSp("Select_State");
            ViewBag.State = ddlState;

            List<SelectListItem> ddlDealerCat = new List<SelectListItem>();
            ddlDealerCat = GeneralMethods.GetDataFromSp("Select_DealerCategory");
            ViewBag.DealerCat = ddlDealerCat;

            List<SelectListItem> ddlDealershipType = new List<SelectListItem>();
            ddlDealershipType = GeneralMethods.GetDataFromSp("Select_DealershipType");
            ViewBag.DealershipType = ddlDealershipType;

            List<SelectListItem> ddlFacility = new List<SelectListItem>();
            ddlFacility = GeneralMethods.GetDataFromSp("Select_Facility");
            ViewBag.Facility = ddlFacility;

            return View();

        }

        //PaymentMode/Select_PaymentModeDetails/AbdusSaboor
        [HttpGet]
        public JsonResult Select_Company_PDealerDetails(string dealer)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = DealerLocationMethods.Get_Company_PDealerDetails(dealer);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        //For ParentDealer
        [HttpGet]
        public JsonResult Select_PDealerDetails(string CompCode)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = DealerLocationMethods.Select_PDealerDetails(CompCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        //For DealerLocation
        [HttpGet]
        public JsonResult Select_Dealer(string CompCode)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = DealerLocationMethods.Select_Dealer(CompCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Select_DealerDetailsData(string value)
        {
            string data = "";
            bool result = false;
            data = DealerLocationMethods.Get_DealerDetailsData(value);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        //SAVE
        public JsonResult DealerDetailsInsertion(DealerLocationVM objects)
        {
            bool data = false;
            bool result = false;
            string msg = "Failed to save record..";
            data = DealerLocationMethods.SP_DealerDetailsInsertion(objects, ref msg);

            if (data)
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        //for delete Delete_DealerLocation/AbdusSaboor
        public JsonResult Delete_DealerLocation(DealerLocationVM objects)
        {

            bool result = false;
            string msg = "'Failed' to Delete Record...";

            result = DealerLocationMethods.Delete_DealerLocation(objects, ref msg);

            if (result)
            {
                msg = "Record Deleted Successfully";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
    }
}