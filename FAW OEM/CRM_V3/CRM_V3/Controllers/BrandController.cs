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
    public class BrandController : Controller
    {
        static string dealerCode = string.Empty;
        // GET: Brand/AbdusSaboor
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Brand()
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
            List<SelectListItem> ddlDeptDesc = new List<SelectListItem>();
            ddlDeptDesc = GeneralMethods.GetDataFromSpWithDealerCode("Get_DepartmentMaster", dealerCode);
            ViewBag.ddlDeptDesc = ddlDeptDesc;


            return View();
        }

        //Get_BrandData/Select_BrandDetails/
        public JsonResult Select_BrandDetails(string dealer)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = BrandMethods.Select_BrandDetails(dealer);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        //for Save Insert_BrandMaster/AbdusSaboor
        public JsonResult Insert_BrandMaster(BrandVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = BrandMethods.Insert_Brand(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
        //for delete Delete_Brand/AbdusSaboor
        public JsonResult Delete_Brand(BrandVM objects)
        {

            bool result = false;
            string msg = "'Failed' to Delete Record...";

            result = BrandMethods.Delete_Brand(objects, ref msg);

            if (result)
            {
                msg = "Record Deleted Successfully";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }

    }
}