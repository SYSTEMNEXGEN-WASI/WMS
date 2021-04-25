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
using Core.CRM.Helper;

namespace CRM_V3.Controllers
{
    public class WarrantyController : Controller
    
    {
        SysFunction sysfunc = new SysFunction();
        string DealerCode = string.Empty;
        string CCon = CConn.CConnection.GetConnectionString().ToString();
        DataTable dt;
        DataSet ds;
        public ActionResult ACMemo(string Id)
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
        [HttpGet]
        public JsonResult WarrantyRedirectLCV(string EnquiryId = "", string EnDealerCode = "", string EnCLose = "")
        {
            bool result = false;
            DealerCode = Session["DealerCode"].ToString();
            Session["id"] = EnquiryId;
            Session["dealer"] = EnDealerCode;

            Session["close"] = EnCLose;

            return Json(new { Success = result }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult WarrantyRedirectHCV(string EnquiryId = "", string EnDealerCode = "", string EnCLose = "")
        {
            bool result = false;
            DealerCode = Session["DealerCode"].ToString();
            Session["id"] = EnquiryId;
            Session["dealer"] = EnDealerCode;

            Session["close"] = EnCLose;

            return Json(new { Success = result }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ApprovalHCV(string EnquiryId = "", string EnDealerCode = "", string EnCLose = "")
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            DealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlComplaint = new List<SelectListItem>();
            ddlComplaint = GeneralMethods.GetDataFromSp("Select_Complaint");
            ViewBag.Complaint = ddlComplaint;

            List<SelectListItem> ddlDefect = new List<SelectListItem>();
            ddlDefect = GeneralMethods.GetDataFromSp("Select_Defect");
            ViewBag.Defect = ddlDefect;

            List<SelectListItem> ddlVendor = new List<SelectListItem>();
            ddlVendor = GeneralMethods.GetDataFromSp("Select_Vendor");
            ViewBag.Vendor = ddlVendor;
            ViewBag.UrlLeadId =     Session["id"].ToString();
            ViewBag.UrlDealerCode = Session["dealer"].ToString();
            ViewBag.EnCLose =       Session["close"].ToString();
            Session["id"]="";
            Session["dealer"]="";
            Session["close"]="";

            return View();
        }
        //
        // GET: /Warranty/
        public ActionResult WarrantyAppr_Reject_LCV()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            DealerCode = Session["DealerCode"].ToString();
            return View();
        }
        public ActionResult WarrantyAppr_Reject_HCV()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            DealerCode = Session["DealerCode"].ToString();
            return View();
        }
        public ActionResult Approval(string EnquiryId = "", string EnDealerCode = "", string EnCLose = "") 
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            DealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlComplaint = new List<SelectListItem>();
            ddlComplaint = GeneralMethods.GetDataFromSp("Select_Complaint");
            ViewBag.Complaint = ddlComplaint;

            List<SelectListItem> ddlDefect = new List<SelectListItem>();
            ddlDefect = GeneralMethods.GetDataFromSp("Select_Defect");
            ViewBag.Defect = ddlDefect;

            List<SelectListItem> ddlVendor = new List<SelectListItem>();
            ddlVendor = GeneralMethods.GetDataFromSp("Select_Vendor");
            ViewBag.Vendor = ddlVendor;

            ViewBag.UrlLeadId = Session["id"].ToString();
            ViewBag.UrlDealerCode = Session["dealer"].ToString();
            ViewBag.EnCLose = Session["close"].ToString();
            Session["id"] = "";
            Session["dealer"] = "";
            return View();
        }
        public ActionResult WarrantyFailPart()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            DealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlComplaint = new List<SelectListItem>();
            ddlComplaint = GeneralMethods.GetDataFromSp("Select_Complaint");
            ViewBag.Complaint = ddlComplaint;

            List<SelectListItem> ddlDefect = new List<SelectListItem>();
            ddlDefect = GeneralMethods.GetDataFromSp("Select_Defect");
            ViewBag.Defect = ddlDefect;

            List<SelectListItem> ddlVendor = new List<SelectListItem>();
            ddlVendor = GeneralMethods.GetDataFromSp("Select_Vendor");
            ViewBag.Vendor = ddlVendor;

            return View();
        }
        public ActionResult WarrantyFailPartHCV()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            DealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlComplaint = new List<SelectListItem>();
            ddlComplaint = GeneralMethods.GetDataFromSp("Select_Complaint");
            ViewBag.Complaint = ddlComplaint;

            List<SelectListItem> ddlDefect = new List<SelectListItem>();
            ddlDefect = GeneralMethods.GetDataFromSp("Select_Defect");
            ViewBag.Defect = ddlDefect;

            List<SelectListItem> ddlVendor = new List<SelectListItem>();
            ddlVendor = GeneralMethods.GetDataFromSp("Select_Vendor");
            ViewBag.Vendor = ddlVendor;

            return View();
        }

        public JsonResult Select_Warranty_Close_LCV(string value)
        {
            string data = "";
            bool result = false;
            data = WarrentyMethods.Get_Warranty_CloseLCV(value);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_Warranty_Close_HCV(string value)
        {
            string data = "";
            bool result = false;
            data = WarrentyMethods.Get_Warranty_CloseHCV(value);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_WarrentyFailPart(string value)
        {
            string data = "";
            bool result = false;
            data = WarrentyMethods.Get_WarrentyDetailFailPart(value);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_WarrentyFailPart_HCV(string value)
        {
            string data = "";
            bool result = false;
            data = WarrentyMethods.Get_WarrentyDetailFailPart_HCV(value);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_WarrentyDetail(string value)
        {
           string data = "";
            bool result = false;
            data = WarrentyMethods.Get_WarrentyDetailData(value);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_WarrentyDetailHCV(string value)
        {
            string data = "";
            bool result = false;
            data = WarrentyMethods.Get_WarrentyDetailDataHCV(value);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Select_WRCauseDetail(string value, string value1)
        {
            string data = "";
            bool result = false;
            data = WarrentyMethods.Get_WRCauseDetail(value, value1);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_VehicleWarrantyDetail(string value, string value1)
        {
            string data = "";
            bool result = false;
            data = WarrentyMethods.Get_VehicleWarrantyDetail(value, value1);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Insert_PPRCause(WarrentyApprovalVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = WarrentyMethods.Insert_PPRCauseData(objects);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Insert_PPRMaster(WarrentyApprovalVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = WarrentyMethods.Insert_PPRMasterData(objects,ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Insert_PPRPart(List<WarrentyApprovalVM> objects)
        {
            bool result = false;
            string msg = "Failed to save record..";
            DealerCode = Session["DealerCode"].ToString();

           result = WarrentyMethods.Insert_PPRPart(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Insert_PPRLabor(List<WarrentyApprovalVM> objects)
        {
            bool result = false;
            string msg = "Failed to save record..";
            DealerCode = Session["DealerCode"].ToString();

            result = WarrentyMethods.Insert_PPRLabor(objects, ref msg);// 

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Insert_PPRSublet(List<WarrentyApprovalVM> objects)
        {
            bool result = false;
            string msg = "Failed to save record..";
            DealerCode = Session["DealerCode"].ToString();

            result = WarrentyMethods.Insert_PPRSublet(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult Insert_PPRParts(WarrentyApprovalVM objects)
        //{
        //    bool result = false;

        //    string msg = "Failed to save record..";

        //    result = WarrentyMethods.Insert_PPRPartsData(objects);

        //    if (result)
        //    {
        //        msg = "Successfully Added";
        //    }

        //    return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult Select_PPRPartsData(string value, string value1)
        {
            string data = "";
            bool result = false;
            data = WarrentyMethods.Get_PPRPartsData(value,value1);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_PPRFPartsData(string value, string value1)
        {
            string data = "";
            bool result = false;
            data = WarrentyMethods.Get_PPRFPartsData(value, value1);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Select_PPRLaborData(string value, string value1)
        {
            string data = "";
            bool result = false;
            data = WarrentyMethods.Get_PPRLaborData(value, value1);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_PPRSubletData(string value, string value1)
        {
            string data = "";
            bool result = false;
            data = WarrentyMethods.Get_PPRSubletrData(value, value1);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        //Fail Part
        public JsonResult Insert_FailPartStatus(WarrentyApprovalVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = WarrentyMethods.Insert_PPRFailPart(objects,ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Insert_FailPartStatusHCV(WarrentyApprovalVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = WarrentyMethods.Insert_PPRFailPartHCV(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FPLCVExport(string EnquiryId, string dealerCode)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
           {
               //new SqlParameter("@DealerCode",Session["DealerCode"]),
                        
            new SqlParameter("@DealerCode",SqlDbType.Char),//
            new SqlParameter("@PPRCode",SqlDbType.VarChar), //1
            new SqlParameter("@VehicleCategory",SqlDbType.VarChar), //1

            };


            param[0].Value = dealerCode;
            param[1].Value = EnquiryId;
            param[2].Value = "LCV";



            SqlDataReader rder = null;


            if (sysfunc.ExecuteSP("SP_PPR_FailPart_Report", param, ref rder))
            {
                //data.SP_selectjobcategorydetail.Load(rder);
                data.EnforceConstraints = false;
                data.SP_PPR_Detail_Report.Load(rder);
            }
            RD.Load(Server.MapPath("~/Reports/rptFailPart.rpt"));

            // RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            //  RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'PRODUCT SUPPORT DEPARTMENT'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["PIC"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
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

        public ActionResult FPHCVExport(string EnquiryId, string dealerCode)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
           {
               //new SqlParameter("@DealerCode",Session["DealerCode"]),
                        
            new SqlParameter("@DealerCode",SqlDbType.Char),//
            new SqlParameter("@PPRCode",SqlDbType.VarChar), //1
            new SqlParameter("@VehicleCategory",SqlDbType.VarChar), //1

            };


            param[0].Value = dealerCode;
            param[1].Value = EnquiryId;
            param[2].Value = "HCV";



            SqlDataReader rder = null;


            if (sysfunc.ExecuteSP("SP_PPR_FailPart_Report", param, ref rder))
            {
                //data.SP_selectjobcategorydetail.Load(rder);
                data.EnforceConstraints = false;
                data.SP_PPR_Detail_Report.Load(rder);
            }
            RD.Load(Server.MapPath("~/Reports/rptFailPart.rpt"));

            // RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            //  RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'PRODUCT SUPPORT DEPARTMENT'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["PIC"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
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
        public ActionResult ACMDetailExport(string dealerCode, string EnquiryId, string FromDate, string ToDate, string Type, string ProdCode)
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


            if (sysfunc.ExecuteSP("SP_ACMemoDetail_Report", param, ref rder))
            {
                //data.SP_selectjobcategorydetail.Load(rder);
                data.EnforceConstraints = false;
                data.SP_ACMemoDetail_Report.Load(rder);
            }
            if (EnquiryId == "" && ProdCode == "0" || EnquiryId != "")
            {
                RD.Load(Server.MapPath("~/Reports/rptFailPartDetail.rpt"));
            }
            else if (EnquiryId == "" && ProdCode != "0")
            {
                RD.Load(Server.MapPath("~/Reports/ACMemo.rpt"));
            }

            // RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["FromDate"].Text = "'" + FromDate + "'";
            RD.DataDefinition.FormulaFields["ToDate"].Text = "'" + ToDate + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            //  RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Approved Claim Memo(ACM) '";
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

    

    public ActionResult Warranty_LCV(string dealerCode, string EnquiryId)
    {
        DSReports data = new DSReports();
        ReportDocument RD = new ReportDocument();
            SysFunction sysFunc = new SysFunction();
            SqlDataReader rder = null;
        SqlParameter[] param =
       {
           new SqlParameter("@DealerCode",SqlDbType.Char),//0
             new SqlParameter("@PPRCode",SqlDbType.VarChar)//1
            };
        param[0].Value = dealerCode;
        param[1].Value = EnquiryId;

            if (sysFunc.ExecuteSP("SP_PPR_Report", param, ref rder))
            {
                data.SP_PPR_Report.Load(rder);
                dt = data.SP_PPR_Report;
                sysFunc.ExecuteSP("sp_PPRLaborDetail_Print", param, ref rder);

                //ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(ccon, CommandType.Text, "sp_PPRLaborDetail_Print '" + this.Session["DealerCode"].ToString() + "','" + dt.Rows[0]["PPRCode"].ToString() + "'");
                data.sp_PPRLaborDetail_Print.Load(rder);
                sysFunc.ExecuteSP("sp_2W_Service_Select_PPRWarrantySubletJobs", param, ref rder);
                data.sp_2W_Service_Select_PPRWarrantySubletJobs.Load(rder);

            }
       
          
        

       RD.Load(Server.MapPath("~/Reports/TRReport.rpt"));

            RD.DataDefinition.FormulaFields["PPRCode"].Text = "'" + EnquiryId + "'";
            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
          //  RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Warranty Report'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
        // RD.DataDefinition.FormulaFields["Pic1"].Text = "'" + Server.MapPath("~") + dt.Rows[0]["item"].ToString() + "'";

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

        public ActionResult Warranty_HCV(string dealerCode, string EnquiryId)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();
            SysFunction sysFunc = new SysFunction();
            SqlDataReader rder = null;
            SqlParameter[] param =
           {
           new SqlParameter("@DealerCode",SqlDbType.Char),//0
             new SqlParameter("@PPRCode",SqlDbType.VarChar)//1
            };
            param[0].Value = dealerCode;
            param[1].Value = EnquiryId;

            if (sysFunc.ExecuteSP("SP_PPR_Report", param, ref rder))
            {
                data.SP_PPR_Report.Load(rder);
                dt = data.SP_PPR_Report;
                sysFunc.ExecuteSP("sp_PPRLaborDetail_Print", param, ref rder);

                //ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(ccon, CommandType.Text, "sp_PPRLaborDetail_Print '" + this.Session["DealerCode"].ToString() + "','" + dt.Rows[0]["PPRCode"].ToString() + "'");
                data.sp_PPRLaborDetail_Print.Load(rder);
                sysFunc.ExecuteSP("sp_2W_Service_Select_PPRWarrantySubletJobs", param, ref rder);
                data.sp_2W_Service_Select_PPRWarrantySubletJobs.Load(rder);

            }




            RD.Load(Server.MapPath("~/Reports/TRReport.rpt"));

            RD.DataDefinition.FormulaFields["PPRCode"].Text = "'" + EnquiryId + "'";
            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            //  RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Warranty Report'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            // RD.DataDefinition.FormulaFields["Pic1"].Text = "'" + Server.MapPath("~") + dt.Rows[0]["item"].ToString() + "'";

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