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
    public class DeliveryOrderController : Controller
    {
        static string dealerCode = string.Empty;
        // GET: DeliveryOrder
        public ActionResult DOMain()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            List<SelectListItem> ddlRecNo = new List<SelectListItem>();
            ddlRecNo = VehReceiptMethods.GetRecNo(dealerCode);
            ViewBag.RecNo = ddlRecNo;

            List<SelectListItem> ddlDONo = new List<SelectListItem>();
            ddlDONo = DeliveryOrderMethods.GetDONo(dealerCode);
            ViewBag.DONo = ddlDONo;

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
            ddlVehLoc = VehReceiptMethods.GetVehLocation(dealerCode);
            ViewBag.VehLoc = ddlVehLoc;

            List<SelectListItem> ddlAssignTo = new List<SelectListItem>();
            ddlAssignTo = DeliveryOrderMethods.GetDealerEmployee(dealerCode);
            ViewBag.AssignTo = ddlAssignTo;

            List<SelectListItem> ddlVendor = new List<SelectListItem>();
            ddlVendor = GeneralMethods.GetVendor();
            ViewBag.Vendor = ddlVendor;

            return View();
        }

        [HttpGet]
        public JsonResult Select_DelChkList()
        {
            string data = "";
            bool result = false;
            data = DeliveryOrderMethods.Get_DelChkList();

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_DeliveryCheckList(string EnquiryId)
        {
            string data = "";
            bool result = false;
            data = DeliveryOrderMethods.Get_DeliveryChkList(EnquiryId);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Export(string EnquiryId, string DealerCode)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
        {
                new SqlParameter("@DealerCode",SqlDbType.Char),
                new SqlParameter("@DONo",SqlDbType.VarChar)

            };

            param[0].Value = DealerCode;
            param[1].Value = EnquiryId;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("SP_DeliveryOrderReport", param, ref rder))
            {
                data.SP_DeliveryOrderReport.Load(rder);

            }
            RD.Load(Server.MapPath("~/Reports/DeliveryOrderReport.rpt"));

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Delivery Order Report'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";


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
        public JsonResult Select_ChassisNo(string EnquiryId)
        {
            List<SelectListItem> data;
            bool result = false;
            
            data = DeliveryOrderMethods.GetChassisNo(EnquiryId,dealerCode);

            if (data.Count > 0)
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult Select_ProRecDetail(string EnquiryId)
        {
            string data;
            bool result = false;

            data = DeliveryOrderMethods.GetProRecDetail(EnquiryId, dealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult Select_DeliveryOrder(string EnquiryId)
        {
            string data = "";
            bool result = false;
            data = DeliveryOrderMethods.Get_DeliveryOrderData(EnquiryId, dealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Insert_DOMaster(VehicleDeliveryMasterVM DOMasterVM)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = DeliveryOrderMethods.Insert_DOMaster(DOMasterVM, dealerCode);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert_DODetail(List<VehicleDeliveryDetailVM> objects)
        {
            bool result = false;
            int count = 0;
            string msg = "Failed to save record..";


            foreach (var item in objects)
            {
                if (count >= 1 || item.BrandCode != null)
                {
                    result = DeliveryOrderMethods.Insert_DODetail(item,dealerCode);
                }
                count++;
            }

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert_VehChkList(string strCheckedValues)
        {
            bool result = false;
            int count = 0;
            string msg = "Failed to save record..";

            result = DeliveryOrderMethods.Insert_VehChkList(strCheckedValues, dealerCode);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete_DeliveryOrder(string EnquiryId)
        {
            bool result = false;

            string msg = "Vehicle is Delivered , Data can't be deleted";

            result = DeliveryOrderMethods.Delete_DeliveryOrder_Record(EnquiryId, dealerCode);

            if (result)
            {
                msg = "Successfully Deleted";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
    }
}