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
    public class VersionController : Controller
    {
        static string dealerCode = string.Empty;
        // GET: Version/ABDUS SABOOR
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Version()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            ////List<SelectListItem> ddlVehicle = new List<SelectListItem>();
            ////ddlVehicle = GeneralMethods.GetDataFromSPWithDealerCode("SP_Vehicles", dealerCode,"Y");
            ////ViewBag.Vehicle = ddlVehicle;

            //List<VersionVM> lstVehicle = VersionMethods.GetVehicleModal(dealerCode);
            //ViewBag.Vehicles = lstVehicle;

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            ViewBag.ddlAllDealer = ddlAllDealer;

            //List<SelectListItem> ddlBrand = new List<SelectListItem>();
            //ddlBrand = GeneralMethods.GetDataFromSPWithDealerCode("Select_Brand", dealerCode);
            //ViewBag.Made = ddlBrand;

            //List<SelectListItem> ddlVehicleType = new List<SelectListItem>();
            //ddlVehicleType = GeneralMethods.GetDataFromSPWithDealerCode("Select_VehicleType", dealerCode);
            //ViewBag.VehicleType = ddlVehicleType;

            //List<SelectListItem> ddlColor = new List<SelectListItem>();
            //ddlColor = GeneralMethods.GetColor();
            //ViewBag.Color = ddlColor;
            //List<VehicleVM> Get_AccDesc = new List<VehicleVM>();
            //Get_AccDesc = VehicleMethods.Get_AccDesc(dealerCode);
            //ViewBag.Get_AccDesc = Get_AccDesc;


            return View();
        }

        //Get_VersionData/Select_VersionDetails/
        public JsonResult Select_VersionDetails(string dealer)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = VersionMethods.Select_VersionDetails(dealer);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        //for Save Insert_VersionMaster/AbdusSaboor
        public JsonResult Insert_VersionMaster(VersionVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = VersionMethods.Insert_Version(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
        //for delete Delete_Version/AbdusSaboor
        public JsonResult Delete_Version(VersionVM objects)
        {

            bool result = false;
            string msg = "'Failed' to Delete Record...";

            result = VersionMethods.Delete_Version(objects, ref msg);

            if (result)
            {
                msg = "Record Deleted Successfully";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
    }
}