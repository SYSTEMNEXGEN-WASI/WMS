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
    public class JobController : Controller
    {
        // GET: Job
        static string dealerCode = string.Empty;
        public ActionResult Index()
        {
            return View();
        }

        //JobMaster /AbdusSaboor
        public ActionResult Job()
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
            List<SelectListItem> ddlBay = new List<SelectListItem>();
            ddlBay = GeneralMethods.GetDataFromSpWithDealerCode("Get_Bay", dealerCode);
            ViewBag.ddlBay = ddlBay;


            return View();

        }
        //ItemCategory/Select_JobDetails/AbdusSaboor
        [HttpGet]
        public JsonResult Select_JobDetails(string dealer)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = JobMethods.Get_Job(dealer,ref data);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_JobCatDetails(string dealer)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = JobMethods.Get_JobCat(dealer,ref data);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        //for Save Insert_Job/AbdusSaboor
        public JsonResult Insert_Job(JobVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = JobMethods.Insert_Job(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
        //for delete Delete_Job/AbdusSaboor
        public JsonResult Delete_Job(JobVM objects)
        {

            bool result = false;
            string msg = "'Failed' to Delete Record...";

            result = JobMethods.Delete_Job(objects, ref msg);

            if (result)
            {
                msg = "Record Deleted Successfully";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }

    }
}