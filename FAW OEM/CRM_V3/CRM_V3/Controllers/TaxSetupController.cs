using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using CRM_V3.assets;
using Microsoft.Reporting.WebForms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
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
    public class TaxSetupController : Controller
    {
        // GET: TaxSetup
        static string dealerCode = string.Empty;
        SysFunction sysFunc = new SysFunction();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TaxSetup()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            ViewBag.ddlAllDealer = ddlAllDealer;

            List<SelectListItem> ddlTaxType = new List<SelectListItem>();
            ddlTaxType = GeneralMethods.GetDataFromSp("Get_TaxType");
            ViewBag.ddlTaxType = ddlTaxType;


            return View();

        }


        //PaymentMode/Select_TaxDetails/AbdusSaboor
        [HttpGet]
        public JsonResult Select_TaxDetails(string dealer)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = TaxSetupMethods.Get_TaxDetails(dealer);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        //for Save Insert_TaxSetup/AbdusSaboor
        public JsonResult Insert_TaxSetup(TaxSetupVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = TaxSetupMethods.Insert_TaxSetup(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
        //for delete Delete_TaxSetup/AbdusSaboor
        public JsonResult Delete_TaxSetup(TaxSetupVM objects)
        {

            bool result = false;
            string msg = "'Failed' to Delete Record...";

            result = TaxSetupMethods.Delete_TaxSetup(objects, ref msg);

            if (result)
            {
                msg = "Record Deleted Successfully";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
        //Report
        //[HttpPost]
        public ActionResult Export()
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
           {
               //new SqlParameter("@DealerCode",Session["DealerCode"].ToString()),
               new SqlParameter("@DealerCode","COMON"),//0
            
            };


            SqlDataReader rder = null;


            if (sysFunc.ExecuteSP("Get_TaxSteupMaster", param, ref rder))
            {
                //data.SP_selectjobcategorydetail.Load(rder);
                //data.SP_VehicleStock_Report.Load(rder);
                data.Get_TaxSteupMaster.Load(rder);
            }
            RD.Load(Server.MapPath("~/Reports/TaxReport.rpt"));

            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'Al Haj FAW'";// text istara khali ho tou istara likhe ge "''"
            RD.DataDefinition.FormulaFields["DealershipName"].Text = "'LCVAF'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Tax Detail'";
            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerFax"].Text = "''";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";



            RD.Database.Tables[0].SetDataSource(data);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "TaxReport.pdf");
            }
            catch
            {
                throw;
            }


        }
    }
}