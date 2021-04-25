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
    public class ItemCategoryController : Controller
    {
        static string dealerCode = string.Empty;
        // GET: ItemCategory
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ItemCategory()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlBrand = new List<SelectListItem>();
            ddlBrand = GeneralMethods.GetDataFromSpWithDealerCode("Select_Brand", dealerCode);
            ViewBag.ddlBrand = ddlBrand;

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            ViewBag.ddlAllDealer = ddlAllDealer;

            List<SelectListItem> ddlproduct = new List<SelectListItem>();
            ddlproduct = GeneralMethods.GetDataFromSp("Select_Product");
            ViewBag.ddlproduct = ddlproduct;

            List<SelectListItem> ddlversion = new List<SelectListItem>();
            ddlversion = GeneralMethods.GetDataFromSp("Select_Version");
            ViewBag.ddlversion = ddlversion;

            List<SelectListItem> ddlVendor = new List<SelectListItem>();
            ddlVendor = GeneralMethods.GetDataFromSp("Select_Vendor");
            ViewBag.Vendor = ddlVendor;


            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlBay = new List<SelectListItem>();
            ddlBay = GeneralMethods.GetDataFromSpWithDealerCode("Get_Bay", dealerCode);
            ViewBag.ddlBay = ddlBay;



            return View();

        }
        //ItemCategory/Select_ItemCategoryDetail/AbdusSaboor
        [HttpGet]
        public JsonResult Select_ItemCategoryDetail(string dealer)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = ItemCategoryMethods.Get_ItemCategory(dealer);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        //for Save Insert_ItemCategory/AbdusSaboor
        public JsonResult Insert_ItemCategory(ItemCategoryVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = ItemCategoryMethods.Insert_ItemCategory(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
        //for delete Delete_ItemCategory/AbdusSaboor
        public JsonResult Delete_ItemCategory(ItemCategoryVM objects)
        {

            bool result = false;
            string msg = "'Failed' to Delete Record...";

            result = ItemCategoryMethods.Delete_ItemCategory(objects, ref msg);

            if (result)
            {
                msg = "Record Deleted Successfully";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
    }
}