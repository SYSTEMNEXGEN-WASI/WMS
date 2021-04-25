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
    public class ReportsController : Controller
    {
        // GET: Reports

        static string dealerCode = string.Empty;
        SysFunction sysFunc = new SysFunction();
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public ActionResult VehHistory(string Id)
        {
            
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();
            if (Id == "LCV")
            {
                ViewBag.Heading = "LCV";
            }
            else
            {
                ViewBag.Heading = "HCV";

            }
            List<SelectListItem> ddlRegNo = new List<SelectListItem>();
            ddlRegNo = GeneralMethods.GetDataFromSpWithDealerCode("sp_RegNoForJobCardHistoryOEM", dealerCode);
            ViewBag.RegNo = ddlRegNo;
            return View();
        }
        //vehiclesstockreport
        public ActionResult VehicleStockReport()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }

            List<SelectListItem> ddlRegNo = new List<SelectListItem>();
            ddlRegNo = GeneralMethods.GetDataFromSpWithDealerCode("sp_RegNoForJobCardHistoryOEM", dealerCode);
            ViewBag.RegNo = ddlRegNo;
            return View();
        }
        public ActionResult WarrantyDetailReportHCV()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            List<SelectListItem> ddlDealers = new List<SelectListItem>();
            ddlDealers = GeneralMethods.GetDataFromSp("Get_DealersForOEM");
            ViewBag.Dealers = ddlDealers;
            return View();
        }
        public ActionResult TechnicalProblemReport()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            List<SelectListItem> ddlDealers = new List<SelectListItem>();
            ddlDealers = GeneralMethods.GetDataFromSp("Get_DealersForOEM");
            ViewBag.Dealers = ddlDealers;
            return View();
        }
        public ActionResult WarrantyDetailReportLCV()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            List<SelectListItem> ddlDealers = new List<SelectListItem>();
            ddlDealers = GeneralMethods.GetDataFromSp("Get_DealersForOEM");
            ViewBag.Dealers = ddlDealers;
            return View();
        }
        public ActionResult DTRDetailReportHCV()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            List<SelectListItem> ddlDealers = new List<SelectListItem>();
            ddlDealers = GeneralMethods.GetDataFromSp("Get_DealersForOEM");
            ViewBag.Dealers = ddlDealers;
            return View();
        }
        public ActionResult DTRDetailReportLCV()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            List<SelectListItem> ddlDealers = new List<SelectListItem>();
            ddlDealers = GeneralMethods.GetDataFromSp("Get_DealersForOEM");
            ViewBag.Dealers = ddlDealers;
            return View();
        }
        public ActionResult VehicleInvoiceReport()
        {
            List<SelectListItem> ddlDealers = new List<SelectListItem>();
            ddlDealers = GeneralMethods.GetDataFromSp("Get_DealersForOEM");
            ViewBag.Dealers = ddlDealers;

            return View();
        }

        public ActionResult JobCardHistoryReport()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            List<SelectListItem> ddlRegNo = new List<SelectListItem>();
            ddlRegNo = GeneralMethods.GetDataFromSpWithDealerCode("sp_RegNoForJobCardHistoryOEM", dealerCode);
            ViewBag.RegNo = ddlRegNo;

            return View();
        }

        [HttpGet]
        public JsonResult Select_ProRecDetail(string EnquiryId)
        {
            string data;
            bool result = false;

            data = GeneralMethods.ChassisEngionFromRegNo(EnquiryId, dealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Export()
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
           {
               //new SqlParameter("@DealerCode",Session["DealerCode"]),
            
            };


            SqlDataReader rder = null;


            if (sysFunc.ExecuteSP("SP_VehicleStock_From_OEM", param, ref rder))
            {
                //data.SP_selectjobcategorydetail.Load(rder);
                data.SP_VehicleStock_Report.Load(rder);
            }
            RD.Load(Server.MapPath("~/Reports/VehicleStockReport.rpt"));

            RD.DataDefinition.FormulaFields["CompanyName"].Text = "''";// text istara khali ho tou istara likhe ge "''"
            RD.DataDefinition.FormulaFields["DealershipName"].Text = "''";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Vehicle Stock Report'";
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
                return File(stream, "VSReport.pdf");
            }
            catch
            {
                throw;
            }


        }
        public ActionResult TechnicalExport(string Type, string dealerCode, string FromDate, string ToDate)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();
            SqlParameter[] param =
         {
               //new SqlParameter("@DealerCode",Session["DealerCode"]),
                        
             new SqlParameter("@DealerCode",SqlDbType.Char),//
            new SqlParameter("@FromDate",SqlDbType.DateTime), //1
            new SqlParameter("@ToDate",SqlDbType.DateTime), //2
             new SqlParameter("@VehicleCategory",SqlDbType.VarChar),

            };


            param[0].Value = dealerCode;
            param[1].Value = sysFunc.SaveDate(FromDate);
            param[2].Value = sysFunc.SaveDate(ToDate);
            param[3].Value = Type;




            SqlDataReader rder = null;


            if (sysFunc.ExecuteSP("SP_TechnicalProblem_Report", param, ref rder))
            {
                //data.SP_selectjobcategorydetail.Load(rder);
                data.EnforceConstraints = false;
                data.SP_TechnicalProblem_Report.Load(rder);
            }
            RD.Load(Server.MapPath("~/Reports/TechnicalProblem.rpt"));

            // RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            //  RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Model Wise Technical Problem Detail Report'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            //RD.DataDefinition.FormulaFields["Pic1"].Text = "'" + Server.MapPath("~") + dt.Rows[0]["item"].ToString() + "'";

            RD.Database.Tables[0].SetDataSource(data);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "VSReport.pdf");
            }
            catch
            {
                throw;
            }


        }
        public ActionResult DTRCVExport(string Type, string dealerCode, string FromDate, string ToDate)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();
            SqlParameter[] param =
         {
               //new SqlParameter("@DealerCode",Session["DealerCode"]),
                        
             new SqlParameter("@DealerCode",SqlDbType.Char),//
            new SqlParameter("@FromDate",SqlDbType.DateTime), //1
            new SqlParameter("@ToDate",SqlDbType.DateTime), //2
             new SqlParameter("@VehicleCategory",SqlDbType.VarChar),
            new SqlParameter("@ChassisNo",SqlDbType.VarChar), //2
            new SqlParameter("@EngineNo",SqlDbType.VarChar),
             new SqlParameter("@RegNo",SqlDbType.VarChar),
               new SqlParameter("@RptType",SqlDbType.VarChar),
            };


            param[0].Value = dealerCode;
            param[1].Value = sysFunc.SaveDate(FromDate);
            param[2].Value = sysFunc.SaveDate(ToDate);
            param[3].Value = Type;
            param[4].Value = "";
            param[5].Value = "";
            param[6].Value = "";
            param[7].Value = "DTR";



            SqlDataReader rder = null;

            
            if (sysFunc.ExecuteSP("SP_FIRDetail_WMS", param, ref rder))
            {
                //data.SP_selectjobcategorydetail.Load(rder);
                data.EnforceConstraints = false;
                data.sp_GetFIRMaster.Load(rder);
            }
            RD.Load(Server.MapPath("~/Reports/DTRDetail.rpt"));

            // RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            //  RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Dealer Technical Report'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            //RD.DataDefinition.FormulaFields["Pic1"].Text = "'" + Server.MapPath("~") + dt.Rows[0]["item"].ToString() + "'";

            RD.Database.Tables[0].SetDataSource(data);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "VSReport.pdf");
            }
            catch
            {
                throw;
            }


        }
        public ActionResult WRCVExport(string Type, string dealerCode, string FromDate, string ToDate)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
           {
               //new SqlParameter("@DealerCode",Session["DealerCode"]),
                        
            new SqlParameter("@DealerCode",SqlDbType.Char),//
            new SqlParameter("@FromDate",SqlDbType.VarChar), //1
            new SqlParameter("@ToDate",SqlDbType.VarChar), //2
            new SqlParameter("@VehicleCategory",SqlDbType.VarChar),
            new SqlParameter("@ChassisNo",SqlDbType.VarChar), //2
            new SqlParameter("@EngineNo",SqlDbType.VarChar),
            new SqlParameter("@RegNo",SqlDbType.VarChar),
            new SqlParameter("@RptType",SqlDbType.VarChar),
            };


            param[0].Value = dealerCode;
            param[1].Value = sysFunc.SaveDate(FromDate);
            param[2].Value = sysFunc.SaveDate(ToDate);
            param[3].Value = Type;
            param[4].Value = "";
            param[5].Value = "";
            param[6].Value = "";
            param[7].Value = "Warranty";



            SqlDataReader rder = null;


            if (sysFunc.ExecuteSP("SP_PPR_Detail_Report", param, ref rder))
            {
                //data.SP_selectjobcategorydetail.Load(rder);
                data.EnforceConstraints = false;
                data.SP_PPR_Detail_Report.Load(rder);
            }
            RD.Load(Server.MapPath("~/Reports/rptWarrantyDetailHCV.rpt"));

            // RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            //  RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Warranty Claim  Report'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            //RD.DataDefinition.FormulaFields["Pic1"].Text = "'" + Server.MapPath("~") + dt.Rows[0]["item"].ToString() + "'";

            RD.Database.Tables[0].SetDataSource(data);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "VSReport.pdf");
            }
            catch
            {
                throw;
            }


        }
        public ActionResult JobCardHistory(string RegNo, string ChassisNo,string EngineNo)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();
            SqlDataReader rder = null;

            DataTable dt = new DataTable();
            // string sql = "exec sp_JobcardHistory '" + Session["DealerCode"].ToString() + "','" + EngineNo + "','" + ChassisNo + "','" + RegNo + "'";
            //dt = sysFunc.GetData(sql);
            SqlParameter[] param =
          {
               new SqlParameter("@DealerCode",Session["DealerCode"].ToString()),
               new SqlParameter("@EngNo",EngineNo),
               new SqlParameter("@ChsNo",ChassisNo),
               new SqlParameter("@RegNo",RegNo),
             

            };
            if (sysFunc.ExecuteSP("sp_JobCardHistory", param, ref rder))
            {
                //data.SP_selectjobcategorydetail.Load(rder);
                data.EnforceConstraints = false;
                data.sp_JobCardHistory.Load(rder);
            }
            RD.PrintOptions.PaperSize = PaperSize.PaperA4;
            
            RD.Load(Server.MapPath("~/Reports/rptJobHistory.rpt"));

            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";

            RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";

            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Jobcard History" + "\"";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            
            RD.Database.Tables[0].SetDataSource(data);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "JobCardHistoryReport.pdf");
            }
            catch
            {
                throw;
            }
        }

        public ActionResult VIExport(string Type, string dealerCode, string FromDate, string ToDate)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();
            SqlDataReader rder = null;

            SqlParameter[] param =
            {
            new SqlParameter("@DealerCode",SqlDbType.Char),//
            new SqlParameter("@FromDate",SqlDbType.VarChar), //1
            new SqlParameter("@ToDate",SqlDbType.VarChar), //2
            new SqlParameter("@Type",SqlDbType.VarChar),

            };
            

                param[0].Value = dealerCode;
                param[1].Value = sysFunc.SaveDate(FromDate);
                param[2].Value = sysFunc.SaveDate(ToDate);
                param[3].Value = Type;

                sysFunc.ExecuteSP("SP_VehicleInvoice_Report_OEM", param, ref rder);
                data.EnforceConstraints = false;
                data.SP_VehicleStock_Report.Load(rder);
                RD.Load(Server.MapPath("~/Reports/VehicleStockReport.rpt"));
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Vehicle Invoice Detail'";

            

            try
            {

                RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
                RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
                RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
                //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + sysfunc.GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
                RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["Pic"].Text = "'F:\CRM\CRM_V3\CRM_V3\assets\images" + Session["Logo"] + "'";
                RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";

                RD.Database.Tables[0].SetDataSource(data);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "ProductReceiptReport.pdf");
            }
            catch
            {
                throw;
            }


        }
        public ActionResult VehicleHistory(string RegNo, string ChassisNo, string EngineNo,string type)
        {
            ReportDocument RD = new ReportDocument();
            DSReports data = new DSReports();
            SqlDataReader rder = null;

            DataTable dt = new DataTable();
            //string sql = "exec sp_JobcardHistory '" + Session["DealerCode"].ToString() + "','" + EngineNo + "','" + ChassisNo + "','" + RegNo + "'";
            //dt = sysFunc.GetData(sql);
            SqlParameter[] param1 =
     {
               //new SqlParameter("@DealerCode",Session["DealerCode"]),
                        
             new SqlParameter("@DealerCode",SqlDbType.Char,05),//
            new SqlParameter("@EngNo",SqlDbType.VarChar,30), //2
            new SqlParameter("@ChsNo",SqlDbType.VarChar,30),
             new SqlParameter("@RegNo",SqlDbType.VarChar,30),
                         };
            param1[0].Value = Session["DealerCode"].ToString();
            param1[1].Value = EngineNo;
            param1[2].Value = ChassisNo;
            param1[3].Value = RegNo;
            //if (sysFunc.ExecuteSP("sp_JobcardHistory", param1, ref rder))
            //{
            //    //data.SP_selectjobcategorydetail.Load(rder);
            //    data.EnforceConstraints = false;
            //    data.sp_JobCardHistory.Load(rder);
            //}
          
            SqlParameter[] param =
         {
               //new SqlParameter("@DealerCode",Session["DealerCode"]),
                        
             new SqlParameter("@DealerCode",SqlDbType.Char),//
            new SqlParameter("@FromDate",SqlDbType.DateTime), //1
            new SqlParameter("@ToDate",SqlDbType.DateTime), //2
             new SqlParameter("@VehicleCategory",SqlDbType.VarChar),
            new SqlParameter("@ChassisNo",SqlDbType.VarChar), //2
            new SqlParameter("@EngineNo",SqlDbType.VarChar),
             new SqlParameter("@RegNo",SqlDbType.VarChar),
               new SqlParameter("@RptType",SqlDbType.VarChar),
            };

           

            param[0].Value = "";
            param[1].Value = null;
            param[2].Value = null;
            param[3].Value = type;
            param[4].Value = ChassisNo;
            param[5].Value = EngineNo;
            param[6].Value = RegNo;
            param[7].Value = "VehicleHistory";
            if (sysFunc.ExecuteSP("SP_FIRDetail_WMS", param, ref rder))
            {

                data.EnforceConstraints = false;
                data.SP_FIRDetail_WMS.Load(rder);
                sysFunc.ExecuteSP("sp_JobcardHistory", param1, ref rder);
                data.EnforceConstraints = false;
                data.sp_JobCardHistory.Load(rder);
                sysFunc.ExecuteSP("SP_PPR_Detail_Report", param, ref rder);
                data.EnforceConstraints = false;
                data.SP_PPR_Detail_Report.Load(rder);
            }

          //  RD.PrintOptions.PaperSize = PaperSize.PaperA4;

            RD.Load(Server.MapPath("~/Reports/rptVehicleHistory.rpt"));

            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";

            RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";

            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Vehicle History" + "\"";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";

            RD.Database.Tables[0].SetDataSource(data);



            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "JobCardHistoryReport.pdf");
            }
            catch
            {
                throw;
            }
        }
    }
}