﻿using Core.CRM.ADO;
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
    public class JobCategoryController : Controller
    {
        // GET: JobCategory
        static string dealerCode = string.Empty;
        public ActionResult Index()
        {
            return View();
        }
        //job category_main /AbdusSaboor
        public ActionResult JobCategory()
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

            return View();

        }
        //ItemCategory/Select_JobCategoryDetails/AbdusSaboor
        [HttpGet]
        public JsonResult Select_JobCategoryDetails(string dealer)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = JobCategoryMethods.Get_JobCategory(dealer,ref data);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        //for Save Insert_JobCategory/AbdusSaboor
        public JsonResult Insert_JobCategory(JobCategoryVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = JobCategoryMethods.Insert_JobCategory(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
        //for delete Delete_JobCategory/AbdusSaboor
        public JsonResult Delete_JobCategory(JobCategoryVM objects)
        {

            bool result = false;
            string msg = "'Failed' to Delete Record...";

            result = JobCategoryMethods.Delete_JobCategory(objects, ref msg);

            if (result)
            {
                msg = "Record Deleted Successfully";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
    }
}