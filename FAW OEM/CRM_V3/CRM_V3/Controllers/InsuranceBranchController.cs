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
    public class InsuranceBranchController : Controller
    {
        // GET: InsuranceBranch
        static string dealerCode = string.Empty;
        SysFunction sysFunc = new SysFunction();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InsuranceBranch()
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
            List<AccountVM> lstAccount = InsuranceBranchMethods.Get_AccDesc(dealerCode);
            ViewBag.Accounts = lstAccount;


            return View();

        }

        [HttpGet]
        public JsonResult Select_InsuranceCompany(string dealer)
        {
            string data = "";
            bool result = false;
            string msg = "Failed to Get Data..";
            string DealerCode = Session["DealerCode"].ToString();
            data = InsuranceBranchMethods.Get_InsuranceCompany(dealer, ref msg);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_InsuranceBranch(string dealer,string InsComCode)
        {
            string data = "";
            bool result = false;
            string msg = "Failed to Get Data..";
            string DealerCode = Session["DealerCode"].ToString();
            data = InsuranceBranchMethods.Get_InsuranceBranch(dealer,InsComCode,ref msg);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Insert_InsuranceBranch(InsuranceBranchVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = InsuranceBranchMethods.Insert_InsuranceBranch(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }

        //for delete 
        public JsonResult Delete_InsuranceBranch(InsuranceBranchVM objects)
        {

            bool result = false;
            string msg = "'Failed' to Delete Record...";

            result = InsuranceBranchMethods.Delete_InsuranceBranch(objects, ref msg);

            if (result)
            {
                msg = "Record Deleted Successfully";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }

    }
}