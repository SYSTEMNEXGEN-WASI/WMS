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
    public class LabourController : Controller
    {
        // GET: Labour
        static string dealerCode = string.Empty;
        public ActionResult Index()
        {
            return View();
        }
        //Labour/AbdusSaboor
        public ActionResult Labour()
        {

            //dealerCode = Session["DealerCode"].ToString();
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            ViewBag.ddlAllDealer = ddlAllDealer;

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlBrandCode = new List<SelectListItem>();
            ddlBrandCode = GeneralMethods.GetDataFromSPWithDealerCode("Select_Brand", dealerCode);
            ViewBag.ddlBrand = ddlBrandCode;

            List<SelectListItem> ddlProdCode = new List<SelectListItem>();
            //ddlProdCode = GeneralMethods.GetProduct();
            ViewBag.ProdCode = ddlProdCode;

            List<SelectListItem> ddlVendor = new List<SelectListItem>();
            ddlVendor = GeneralMethods.GetDataFromSp("Select_Vendor");
            ViewBag.Vendor = ddlVendor;


            return View();

        }
        [HttpGet]
        public JsonResult Select_BrandProduct(string EnquiryId, string DealerCode)
        {
            List<PrdouctVM> data;
            DealerCode = Session["DealerCode"].ToString();
            bool result = false;
            data = LabourMethods.Get_BrandProductData(EnquiryId, DealerCode, Session["VehicleCategory"].ToString());

            if (data.Count > 0)
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }
        //ItemCategory/Select_LabourDetails/AbdusSaboor
        [HttpGet]
        public JsonResult Select_LabourDetails(string dealer,string CatCode)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = LabourMethods.Get_Labour(dealer,CatCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_LabourDetails2(string dealer)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = LabourMethods.Get_LabourCat(dealer);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Select_labourGridData(string value1, string value2)
        {
            string data = "";
            bool result = false;
            data = LabourMethods.Get_LabourDetailGrid(value1, value2);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        //for Save Insert_Labour/AbdusSaboor
        public JsonResult Insert_Labour(LabourVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = LabourMethods.Insert_Labour(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
        //for delete Delete_Labour/AbdusSaboor
        public JsonResult Delete_Labour(LabourVM objects)
        {

            bool result = false;
            string msg = "'Failed' to Delete Record...";

            result = LabourMethods.Delete_Labour(objects, ref msg);

            if (result)
            {
                msg = "Record Deleted Successfully";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
    }
}