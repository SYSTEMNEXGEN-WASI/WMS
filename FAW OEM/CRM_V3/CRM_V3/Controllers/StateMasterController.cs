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
    public class StateMasterController : Controller
    {
        // GET: StateMaster
        static string dealerCode = string.Empty;
        public ActionResult Index()
        {
            return View();
        }

        //State Master /abdussaboor
        public ActionResult StateMaster()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }

            List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            ViewBag.ddlAllDealer = ddlAllDealer;

            return View();

        }
        //Select/Get State /abdussaboor
        public JsonResult Select_StateMasterDetail(string dealer)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = StateMasterMethods.Get_StateMaster(dealer);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        //for Save StateMaster
        public JsonResult Insert_StateMaster(StateMasterVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = StateMasterMethods.Insert_StateMaster(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }

        //for delete Delete_StateMaster/AbdusSaboor
        public JsonResult Delete_StateMaster(StateMasterVM objects)
        {

            bool result = false;
            string msg = "'Failed' to Delete Record...";

            result = StateMasterMethods.Delete_StateMaster(objects, ref msg);

            if (result)
            {
                msg = "Record Deleted Successfully";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
    }
}