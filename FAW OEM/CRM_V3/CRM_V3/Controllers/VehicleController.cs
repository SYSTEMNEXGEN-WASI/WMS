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
    public class VehicleController : Controller
    {
        static string dealerCode = string.Empty;
        // GET: Vehicle
        public ActionResult VehiclePrice()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            //List<SelectListItem> ddlVehicle = new List<SelectListItem>();
            //ddlVehicle = GeneralMethods.GetDataFromSPWithDealerCode("SP_Vehicles", dealerCode,"Y");
            //ViewBag.Vehicle = ddlVehicle;

            List<VehicleVM> lstVehicle = VehicleMethods.GetVehicleModal(dealerCode);
            ViewBag.Vehicles = lstVehicle;
            Session["lstVehicle"] = lstVehicle;

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            ViewBag.ddlAllDealer = ddlAllDealer;

            List<SelectListItem> ddlBrand = new List<SelectListItem>();
            ddlBrand = GeneralMethods.GetDataFromSPWithDealerCode("Select_Brand", dealerCode);
            ViewBag.Made = ddlBrand;
            List<SelectListItem> ddlVersion = new List<SelectListItem>();
            ddlVersion = GeneralMethods.GetDataFromSp("Select_Version");
            ViewBag.ddlVersion = ddlVersion;


            List<SelectListItem> ddlVehicleType = new List<SelectListItem>();
            ddlVehicleType = GeneralMethods.GetDataFromSPWithDealerCode("Select_VehicleType", dealerCode);
            ViewBag.VehicleType = ddlVehicleType;

            List<SelectListItem> ddlColor = new List<SelectListItem>();
            ddlColor = GeneralMethods.GetColor(Session["VehicleCategory"].ToString());
            ViewBag.Color = ddlColor;
            List<VehicleVM> Get_AccDesc = new List<VehicleVM>();
            Get_AccDesc = VehicleMethods.Get_AccDesc(dealerCode);
            ViewBag.Get_AccDesc = Get_AccDesc;


            return View();
        }
        public ActionResult Vehicle()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            //List<SelectListItem> ddlVehicle = new List<SelectListItem>();
            //ddlVehicle = GeneralMethods.GetDataFromSPWithDealerCode("SP_Vehicles", dealerCode,"Y");
            //ViewBag.Vehicle = ddlVehicle;

            List<SelectListItem> ddlVersion = new List<SelectListItem>();
            ddlVersion = GeneralMethods.GetDataFromSp("Select_Version");
            ViewBag.ddlVersion = ddlVersion;
            List<VehicleVM> lstVehicle = VehicleMethods.GetVehicleModal(dealerCode);
            ViewBag.Vehicles = lstVehicle;

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            ViewBag.ddlAllDealer = ddlAllDealer;

            List<SelectListItem> ddlBrand = new List<SelectListItem>();
            ddlBrand = GeneralMethods.GetDataFromSPWithDealerCode("Select_Brand", dealerCode);
            ViewBag.Made = ddlBrand;

            List<SelectListItem> ddlVehicleType = new List<SelectListItem>();
            ddlVehicleType = GeneralMethods.GetDataFromSPWithDealerCode("Select_VehicleType", dealerCode);
            ViewBag.VehicleType = ddlVehicleType;

            List<SelectListItem> ddlColor = new List<SelectListItem>();
            ddlColor = GeneralMethods.GetColor(Session["VehicleCategory"].ToString());
            ViewBag.Color = ddlColor;
            List<VehicleVM> Get_AccDesc = new List<VehicleVM>();
            Get_AccDesc = VehicleMethods.Get_AccDesc(dealerCode);
            ViewBag.Get_AccDesc = Get_AccDesc;


            return View();
        }

        //for delete Delete_Vehicle/AbdusSaboor
        public JsonResult Delete_VehicleMaster(VehicleVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = VehicleMethods.Delete_VehicleMaster(objects, ref msg);

            if (result)
            {
                msg = "Record Delete Successfully";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Insert_VehicleMaster(VehicleVM VehicleVM)
        {
            bool result = false;

            string msg = "Failed to save record..";

            msg = VehicleMethods.Insert_VehicleMaster(VehicleVM, Session["DealerCode"].ToString());

            if (msg == "Completed")
            {
                result = true;
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert_VehicleTaxDetail(List<VehicleTaxDetailVM> objects)
        {
            bool result = false;
            int count = 0;
            string msg = "Failed to save record..";


            //foreach(var item in objects)
            //{
            //    if(count >= 1 || item.BrandCode != null)
            //    {
            result = VehicleMethods.Insert_VehicleTaxDetail(objects, Session["DealerCode"].ToString(),ref msg);
            //    }
            //    count++;

            //}       

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_Vehicle(string EnquiryId)
        {
            string data = "";
            bool result = false;
            data = VehicleMethods.Get_VehicleMasterData(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_VehicleDetail(string EnquiryId, string Code)
        {
            string data = "";
            bool result = false;
            data = VehicleMethods.Get_VehicleDetailData(EnquiryId, Session["DealerCode"].ToString());

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_VehicleCategory(string EnquiryId, string DealerCode)
        {
            string data = "";
            bool result = false;
            data = VehicleMethods.Get_VehicleCategory(EnquiryId, DealerCode);

            if (data != "")
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }
    }
}