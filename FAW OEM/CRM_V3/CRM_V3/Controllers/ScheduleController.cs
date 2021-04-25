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
    public class ScheduleController : Controller
    {
        // GET: Schedule
        static string dealerCode = string.Empty;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Schedule()
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
            List<SelectListItem> ddlBrand = new List<SelectListItem>();
            ddlBrand = GeneralMethods.GetDataFromSpWithDealerCode("Select_Brand", dealerCode);
            ViewBag.ddlBrand = ddlBrand;

            List<SelectListItem> ddlproduct = new List<SelectListItem>();
            ddlproduct = GeneralMethods.GetDataFromSp("Select_Product");
            ViewBag.ddlproduct = ddlproduct;

            List<SelectListItem> ddlversion = new List<SelectListItem>();
            ddlversion = GeneralMethods.GetDataFromSp("Select_Version");
            ViewBag.ddlversion = ddlversion;

            List<SelectListItem> ddlVendor = new List<SelectListItem>();
            ddlVendor = GeneralMethods.GetDataFromSp("Select_Vendor");
            ViewBag.Vendor = ddlVendor;


            return View();

        }
        //ItemCategory/Select_ScheduleDetails/AbdusSaboor
        [HttpGet]
        public JsonResult Select_ScheduleDetails(string dealer)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = ScheduleMethods.Get_Schedule(dealer);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Select_ScheduleDetails2(string dealer)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = ScheduleMethods.Get_Schedule_Version(dealer);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Select_ScheduleDetails3(string ProdCode, string VehicleCategory)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = ScheduleMethods.Get_Schedule_Job(ProdCode,VehicleCategory);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Select_SchedulePartsDetails(string dealer, string VehicleCategory)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = ScheduleMethods.Get_ScheduleParts(dealer, VehicleCategory);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Save Job Get
        /// </summary>
        /// <param name="dealer"></param>
        /// <param name="ScheduleCode"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Select_SaveJobDetails(string dealer, string ScheduleCode)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = ScheduleMethods.Get_ScheduleJobSave(dealer, ScheduleCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Save Part Get
        /// </summary>
        /// <param name="dealer"></param>
        /// <param name="ScheduleCode"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Select_SavePartsDetails(string dealer, string ScheduleCode)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = ScheduleMethods.Get_SchedulePartsSave(dealer, ScheduleCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Save Lubricant Get
        /// </summary>
        /// <param name="dealer"></param>
        /// <param name="ScheduleCode"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Select_SaveLubeDetails(string dealer, string ScheduleCode)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = ScheduleMethods.Get_ScheduleLubeSave(dealer, ScheduleCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Select_ScheduleLubDetails(string dealer, string VehicleCategory)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = ScheduleMethods.Get_ScheduleLub(dealer, VehicleCategory);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Insert_ScheduleMaster(ScheduleVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = ScheduleMethods.Insert_ScheduleMaster(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Insert_ScheduleTabJobs(List<ScheduleVM> objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = ScheduleMethods.Insert_ScheduleTabJobs(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Insert_ScheduleTabParts(List<ScheduleVM> objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = ScheduleMethods.Insert_ScheduleTabParts(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Insert_ScheduleTabLub(List<ScheduleVM> objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = ScheduleMethods.Insert_ScheduleTabLub(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        #region

        #endregion


    }
}