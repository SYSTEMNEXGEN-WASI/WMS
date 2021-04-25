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
    public class MasterFormsOEMController : Controller
    {
        static string dealerCode = string.Empty;
        // GET: MasterFormsOEM
        public ActionResult Index()
        {
            return View();
        }
        //PaymentMode Master /abdussaboor
        public ActionResult PaymentModeMaster()
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

        //PaymentModeMaster/Select_PaymentModeMasterDetail/AbdusSaboor
        public JsonResult Select_PaymentModeMasterDetail(string JobCatCode, string JobCatDesc)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = MasterFormsOEM_Methods.Get_PaymentmodeMaster(DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        //for Save PaymentModeMaster/AbdusSaboor
        public JsonResult Insert_PaymentModeMaster(MasterFormsOEM_VM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = MasterFormsOEM_Methods.Insert_PaymentmodeMaster(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }

        //VehicleMaster Master /abdussaboor
        public ActionResult VehicleMaster()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }


            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlItemCatDesc = new List<SelectListItem>();
            ddlItemCatDesc = GeneralMethods.GetDataFromSpWithDealerCode("Get_ItemCategory", dealerCode);
            ViewBag.ddlItemCatDesc = ddlItemCatDesc;

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlUnit = new List<SelectListItem>();
            ddlUnit = GeneralMethods.GetDataFromSpWithDealerCode("Get_Unit", dealerCode);
            ViewBag.ddlUnit = ddlUnit;

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlPartType = new List<SelectListItem>();
            ddlPartType = GeneralMethods.GetDataFromSpWithDealerCode("Get_Unit", dealerCode);
            ViewBag.ddlPartType = ddlPartType;

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            ViewBag.ddlAllDealer = ddlAllDealer;



            return View();

        }

        //VehicleMaster/Select_VehicleMasterDetail/AbdusSaboor
        public JsonResult Select_VehicleMasterDetail(string JobCatCode, string JobCatDesc)
        {
            List<MasterFormsOEM_VM> data;
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = MasterFormsOEM_Methods.Get_ItemMaster(DealerCode);

            if (data.Count > 0)
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        //for Save VehicleMaster/AbdusSaboor
        public JsonResult Insert_VehicleMaster(MasterFormsOEM_VM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = MasterFormsOEM_Methods.Insert_ItemMaster(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }

        //for delete Delete_VehicleMaster/AbdusSaboor
        public JsonResult Delete_VehicleMaster(MasterFormsOEM_VM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = MasterFormsOEM_Methods.Delete_ItemMaster(objects, ref msg);

            if (result)
            {
                msg = "Record Delete Successfully";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }

        //ItemMaster Master /abdussaboor
        public ActionResult ItemMaster()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }


            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlItemCatDesc = new List<SelectListItem>();
            ddlItemCatDesc = GeneralMethods.GetDataFromSpWithDealerCode("Get_ItemCategory", dealerCode);
            ViewBag.ddlItemCatDesc = ddlItemCatDesc;

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlUnit = new List<SelectListItem>();
            ddlUnit = GeneralMethods.GetDataFromSpWithDealerCode("Get_Unit", dealerCode);
            ViewBag.ddlUnit = ddlUnit;

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlPartType = new List<SelectListItem>();
            ddlPartType = GeneralMethods.GetDataFromSpWithDealerCode("Get_Unit", dealerCode);
            ViewBag.ddlPartType = ddlPartType;

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            ViewBag.ddlAllDealer = ddlAllDealer;



            return View();

        }

        //ItemMaster/Select_ItemMasterDetail/AbdusSaboor
        public JsonResult Select_ItemMasterDetail(string JobCatCode, string JobCatDesc)
        {
            List<MasterFormsOEM_VM> data ;
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = MasterFormsOEM_Methods.Get_ItemMaster(DealerCode);

            if (data.Count>0)
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        //for Save ItemMaster/AbdusSaboor
        public JsonResult Insert_ItemMaster(MasterFormsOEM_VM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = MasterFormsOEM_Methods.Insert_ItemMaster(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }

        //for delete Delete_ItemMaster/AbdusSaboor
        public JsonResult Delete_ItemMaster(MasterFormsOEM_VM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = MasterFormsOEM_Methods.Delete_ItemMaster(objects, ref msg);

            if (result)
            {
                msg = "Record Delete Successfully";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }

        //ItemCategory Master /abdussaboor
        public ActionResult ItemCategoryMaster()
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

        //ItemCategoryMaster/Select_ItemCategoryMasterDetail/AbdusSaboor
        [HttpGet]
        public JsonResult Select_ItemCategoryMasterDetail()
        {
            List<MasterFormsOEM_VM> data;
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = MasterFormsOEM_Methods.Get_ItemMaster(DealerCode);
            Session["ItemList"] = data;
            if (data.Count>0)
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        //ItemCategoryMaster/Select_ItemCategoryMasterDetail2/AbdusSaboor
        [HttpGet]
        public JsonResult Select_ItemCategoryMasterDetail2(string value1,string value2)
        {
            List<MasterFormsOEM_VM> data;
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = MasterFormsOEM_Methods.Get_ItemMaster2(value1,value2);

            if (data.Count > 0)
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        //for Save ItemCategoryMaster/AbdusSaboor
        public JsonResult Insert_ItemCategoryMaster(MasterFormsOEM_VM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = MasterFormsOEM_Methods.Insert_ItemMaster(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }

        //Designation Master /abdussaboor
        public ActionResult DesignationMaster()
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

        //ItemDesignationMaster/Select_DesignationMasterDetail/AbdusSaboor
        public JsonResult Select_DesignationMasterDetail(string JobCatCode, string JobCatDesc)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = MasterFormsOEM_Methods.Get_DesignationMaster(DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        //for Save ItemDesignationMaster/AbdusSaboor
        public JsonResult Insert_DesignationMaster(MasterFormsOEM_VM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = MasterFormsOEM_Methods.Insert_DesignationMaster(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }

        //Department Master /abdussaboor
        public ActionResult DepartmentMaster()
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

        //ItemDepartmentMaster/Select_DepartmentMasterDetail/AbdusSaboor
        public JsonResult Select_DepartmentMasterDetail(string JobCatCode, string JobCatDesc)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = MasterFormsOEM_Methods.Get_DepartmentMaster(DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        //for Save ItemDepartmentMaster/AbdusSaboor
        public JsonResult Insert_DepartmentMaster(MasterFormsOEM_VM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = MasterFormsOEM_Methods.Insert_DepartmentMaster(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }

    }
}