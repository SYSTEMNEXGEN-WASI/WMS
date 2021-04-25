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
    public class FailedPartDetailController : Controller
    {
        // GET: FailedPartDetail
        SysFunction sysfunc = new SysFunction();
        string DealerCode = string.Empty;
        public ActionResult FailPartDetailItemWise(string Id)
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            DealerCode = Session["DealerCode"].ToString();
            if (Id == "LCV")
            {
                ViewBag.Heading = "LCV";
            }
            else
            {
                ViewBag.Heading = "HCV";

            }
            List<SelectListItem> ddlDealers = new List<SelectListItem>();
            ddlDealers = GeneralMethods.GetDataFromSp("Get_DealersForOEM");
            ViewBag.Dealers = ddlDealers;
            List<SelectListItem> ddlProdCode = new List<SelectListItem>();
            ddlProdCode = GeneralMethods.GetProduct();
            ViewBag.ProdCode = ddlProdCode;
            return View();
        }
        public ActionResult TopDefect(string Id)
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            DealerCode = Session["DealerCode"].ToString();
            if (Id == "LCV")
            {
                ViewBag.Heading = "LCV";
            }
            else
            {
                ViewBag.Heading = "HCV";

            }
            List<SelectListItem> ddlDealers = new List<SelectListItem>();
            ddlDealers = GeneralMethods.GetDataFromSp("Get_DealersForOEM");
            ViewBag.Dealers = ddlDealers;
            List<SelectListItem> ddlProdCode = new List<SelectListItem>();
            ddlProdCode = GeneralMethods.GetProductCategory(Id);
            ViewBag.ProdCode = ddlProdCode;
            return View();
        }
        [HttpGet]
        public JsonResult Select_ItemProduct(string DealerCode,string cat)
        {
            List<ItemVM> data;
            DealerCode = Session["DealerCode"].ToString();
            bool result = false;
            // List<GetProductSpVM> ddlProduct = new List<GetProductSpVM>();
            data = FailedPartMethods.Get_Item(DealerCode,cat);
          //  Session["VehicleRecord"] = data;
            // ViewBag.ddlProduct = ddlProduct;
            //data = VehReceiptMethods.Get_BrandProductData(EnquiryId,DealerCode);

            if (data.Count > 0)
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult FPDetailExport(string dealerCode, string EnquiryId, string FromDate,string ToDate,string Type ,string ProdCode)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
           {
               //new SqlParameter("@DealerCode",Session["DealerCode"]),
                        
            new SqlParameter("@DealerCode",SqlDbType.Char),//
            new SqlParameter("@ItemCode",SqlDbType.VarChar), //1
             new SqlParameter("@FromDate",SqlDbType.VarChar), //1
            new SqlParameter("@ToDate",SqlDbType.VarChar), //2
            new SqlParameter("@VehicleCategory",SqlDbType.VarChar), //1
             new SqlParameter("@ProdCode",SqlDbType.VarChar,10), //1
            };


            param[0].Value = dealerCode;
            param[1].Value = EnquiryId;
            param[2].Value = sysfunc.SaveDate(FromDate);
            param[3].Value = sysfunc.SaveDate(ToDate);
            param[4].Value = Type;
            param[5].Value = ProdCode;



            SqlDataReader rder = null;


            if (sysfunc.ExecuteSP("SP_FailPartDetail_Report", param, ref rder))
            {
                //data.SP_selectjobcategorydetail.Load(rder);
                data.EnforceConstraints = false;
                data.SP_PPR_Detail_Report.Load(rder);
            }
            if(EnquiryId==""  && ProdCode=="0" || EnquiryId != "")
            {
                RD.Load(Server.MapPath("~/Reports/rptFailPartDetail.rpt"));
            }
           else if (EnquiryId == "" && ProdCode != "0")
            {
                RD.Load(Server.MapPath("~/Reports/rptFailPartProdDetail.rpt"));
            }

            // RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["FromDate"].Text = "'" + FromDate + "'";
            RD.DataDefinition.FormulaFields["ToDate"].Text = "'" + ToDate + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            //  RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'PRODUCT SUPPORT '";
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
        public ActionResult TopDefectExport(string dealerCode, string EnquiryId, string FromDate, string ToDate, string Type, string ProdCode)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
           {
               //new SqlParameter("@DealerCode",Session["DealerCode"]),
                        
            new SqlParameter("@DealerCode",SqlDbType.Char),//
            new SqlParameter("@ItemCode",SqlDbType.VarChar), //1
             new SqlParameter("@FromDate",SqlDbType.VarChar), //1
            new SqlParameter("@ToDate",SqlDbType.VarChar), //2
            new SqlParameter("@VehicleCategory",SqlDbType.VarChar), //1
             new SqlParameter("@ProdCode",SqlDbType.VarChar,10), //1
              new SqlParameter("@year",SqlDbType.Int), //1
            };


            param[0].Value = dealerCode;
            param[1].Value = EnquiryId;
            param[2].Value = "";
            param[3].Value = "";
            param[4].Value = Type;
            param[5].Value = ProdCode;
            param[6].Value = FromDate;


            SqlDataReader rder = null;


            if (sysfunc.ExecuteSP("SP_Defect_Report", param, ref rder))
            {
                //data.SP_selectjobcategorydetail.Load(rder);
                data.EnforceConstraints = false;
                data.SP_Defect_Report.Load(rder);
            }
            
                RD.Load(Server.MapPath("~/Reports/rptFailPartDefect.rpt"));
            

            // RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["FromDate"].Text = "'" + FromDate + "'";
            RD.DataDefinition.FormulaFields["ToDate"].Text = "'" + ToDate + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            //  RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Service Department '";
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
    }
}