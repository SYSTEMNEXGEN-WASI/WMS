using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using CRM_V3.assets;
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
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CRM_V3.Controllers
{
    public class InsuranceCompanyController : Controller
    {
        // GET: InsuranceCompany
        static string dealerCode = string.Empty;
        SysFunction sysFunc = new SysFunction();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InsuranceCompany()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            ViewBag.ddlAllDealer = ddlAllDealer;
            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAllEmp = new List<SelectListItem>();
            ddlAllEmp = GeneralMethods.GetDataFromSp("Get_AllEmp");
            ViewBag.ddlAllEmp = ddlAllEmp;



            return View();

        }

        [HttpGet]
        public JsonResult Select_InsuranceCompany(string dealer)
        {
            string data = "";
            bool result = false;
            string msg = "Failed to Get Data..";
            string DealerCode = Session["DealerCode"].ToString();
            data = InsuranceCompanyMethods.Get_InsuranceCompany(dealer, ref msg);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Insert_InsuranceCompany(InsuranceCompanyVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = InsuranceCompanyMethods.Insert_InsuranceCompany(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
        
        //for delete 
        public JsonResult Delete_InsuranceCompany(InsuranceCompanyVM objects)
        {

            bool result = false;
            string msg = "'Failed' to Delete Record...";

            result = InsuranceCompanyMethods.Delete_InsuranceCompany(objects, ref msg);

            if (result)
            {
                msg = "Record Deleted Successfully";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
    }
}