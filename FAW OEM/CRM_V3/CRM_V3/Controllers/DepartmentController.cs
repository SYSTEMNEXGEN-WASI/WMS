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
    public class DepartmentController : Controller
    {
        // GET: Department
        static string dealerCode = string.Empty;
        public ActionResult Index()
        {
            return View();
        }

        //job category_main /AbdusSaboor
        public ActionResult Department()
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
        //ItemCategory/Select_DepartmentDetails/AbdusSaboor
        [HttpGet]
        public JsonResult Select_DepartmentDetails(string dealer)
        {
            string msg = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            msg = DepartmentMethods.Get_Deapartment(dealer,ref msg);

            if (!string.IsNullOrEmpty(msg))
            {
                result = true;
            }

            return Json(new { Success = result, Response = msg }, JsonRequestBehavior.AllowGet);
        }
        //for Save Insert_Department/AbdusSaboor
        public JsonResult Insert_Department(DepartmentVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = DepartmentMethods.Insert_Department(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
        //for delete Delete_Department/AbdusSaboor
        public JsonResult Delete_Department(DepartmentVM objects)
        {

            bool result = false;
            string msg = "'Failed' to Delete Record...";

            result = DepartmentMethods.Delete_Department(objects, ref msg);

            if (result)
            {
                msg = "Record Deleted Successfully";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }


    }
}