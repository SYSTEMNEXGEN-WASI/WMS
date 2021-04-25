using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using CRM_V3.assets;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM_V3.Controllers
{
    public class VehicleReceiptController : Controller
    {
        static string DealerCode;
        // GET: VehicleReceipt
        public ActionResult Main()
        {

            if(string.IsNullOrEmpty((string)Session["DealerCode"])) {
                return RedirectToAction("Login", "Home");
            }
            DealerCode = Session["DealerCode"].ToString();

            List<SelectListItem> ddlRecNo = new List<SelectListItem>();
            ddlRecNo = VehReceiptMethods.GetRecNo(DealerCode);
            ViewBag.RecNo = ddlRecNo;

            List<SelectListItem> ddlVehType = new List<SelectListItem>();
            ddlVehType = GeneralMethods.GetVehicaleType();
            ViewBag.VehType = ddlVehType;

            List<SelectListItem> ddlRecType = new List<SelectListItem>();
            ddlRecType = GeneralMethods.GetReceiptType();
            ViewBag.RecType = ddlRecType;

            List<SelectListItem> ddlBrandCode = new List<SelectListItem>();
            ddlBrandCode = GeneralMethods.GetBrands();
            ViewBag.BrandCode = ddlBrandCode;

            List<SelectListItem> ddlProdCode = new List<SelectListItem>();
            ddlProdCode = GeneralMethods.GetProduct();
            ViewBag.ProdCode = ddlProdCode;

            List<SelectListItem> ddlColor = new List<SelectListItem>();
            ddlColor = GeneralMethods.GetColor(Session["VehicleCategory"].ToString());
            ViewBag.Color = ddlColor;

            List<SelectListItem> ddlCustomers = new List<SelectListItem>();
            ddlCustomers = VehReceiptMethods.GetCustomers();
            ViewBag.Customers = ddlCustomers;

            List<SelectListItem> ddlVehLoc = new List<SelectListItem>();
            ddlVehLoc = VehReceiptMethods.GetVehLocation(DealerCode);
            ViewBag.VehLoc = ddlVehLoc;

            List<SelectListItem> ddlVendor = new List<SelectListItem>();
            ddlVendor = GeneralMethods.GetVendor();
            ViewBag.Vendor = ddlVendor;


            return View();
        }


        public ActionResult Export(string EnquiryId, string DealerCode)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
        {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@EnquiryId",SqlDbType.VarChar)

            };

            
            param[0].Value = DealerCode;
            param[1].Value = EnquiryId;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("SP_Select_ProdRec_Print", param, ref rder))
            {
                data.SP_Select_ProdRec_Print.Load(rder);
                
            }
            RD.Load(Server.MapPath("~/Reports/VehicleReceipt.rpt"));

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Vehicle Receipt Report'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";


            RD.Database.Tables[0].SetDataSource(data);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "ProductReceiptReport.pdf");
            }
            catch
            {
                throw;
            }


        }


        [HttpGet]
        public JsonResult Select_BrandProduct(string EnquiryId, string DealerCode)
        {
            List<SelectListItem> data;
            bool result = false;
            data = EventMethods.Get_BrandProductData(EnquiryId, DealerCode);

            if (data.Count > 0)
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }


        public JsonResult Insert_ProdRecMaster(ProdReceiptVM ProdReceiptVM)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = VehReceiptMethods.Insert_ProdMaster(ProdReceiptVM , DealerCode);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete_ProdRecMaster(string EnquiryId, string DealerCode)
        {
            bool result = false;

            string msg = "Vehicle is Delivered , Data can't be deleted";

            result = VehReceiptMethods.Delete_VehicleReceipt_Record(EnquiryId, DealerCode);

            if (result)
            {
                msg = "Successfully Deleted";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete_ProdRecDetail(string EnquiryId, string DealerCode)
        {
            bool result = false;

            string msg = "Vehicle is Delivered , Data can't be deleted";

            result = VehReceiptMethods.Delete_VehicleReceiptDetail_Record(EnquiryId, DealerCode);

            if (result)
            {
                msg = "Successfully Deleted";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert_ProdRecDetail( List<ProdReceiptDetailVM> objects)
        {
            bool result = false;
            int count = 0;
            string msg = "Failed to save record..";


            foreach(var item in objects)
            {
                if(count >= 1 || item.BrandCode != null)
                {
                    result = VehReceiptMethods.Insert_ProdDetail(item);
                }
                count++;
            }       

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_VehicleReceipt(string EnquiryId)
        {
            string data = "";
            bool result = false;
            data = VehReceiptMethods.Get_VehicleReceiptData(EnquiryId, DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_VehicleReceiptDetail(string EnquiryId, string Code)
        {
            string data = "";
            bool result = false;
            data = VehReceiptMethods.Get_VehicleReceiptDetailData(EnquiryId, DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
    }
}