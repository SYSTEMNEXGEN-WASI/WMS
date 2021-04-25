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
    public class OEMController : Controller
    {
        static string dealerCode = string.Empty;


        // GET: OEM
        
        public ActionResult ParentDealer()
        {
            return View();
        }
        public ActionResult DealerLocation()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            List<SelectListItem> ddlCity = new List<SelectListItem>();
            ddlCity = GeneralMethods.GetDataFromSp("Select_City");
            ViewBag.City = ddlCity;

            List<SelectListItem> ddlCountry = new List<SelectListItem>();
            ddlCountry = GeneralMethods.GetDataFromSp("Select_Country");
            ViewBag.Country = ddlCountry;

            List<SelectListItem> ddlState = new List<SelectListItem>();
            ddlState = GeneralMethods.GetDataFromSp("Select_State");
            ViewBag.State = ddlState;

            List<SelectListItem> ddlDealerCat = new List<SelectListItem>();
            ddlDealerCat = GeneralMethods.GetDataFromSp("Select_DealerCategory");
            ViewBag.DealerCat = ddlDealerCat;

            List<SelectListItem> ddlDealershipType = new List<SelectListItem>();
            ddlDealershipType = GeneralMethods.GetDataFromSp("Select_DealershipType");
            ViewBag.DealershipType = ddlDealershipType;

            List<SelectListItem> ddlFacility = new List<SelectListItem>();
            ddlFacility = GeneralMethods.GetDataFromSp("Select_Facility");
            ViewBag.Facility = ddlFacility;

           



            return View();
        }

        public ActionResult DealerCat()
        {
             if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            dealerCode = Session["DealerCode"].ToString();

            return View();
        }
        public ActionResult DashBoardChart()
        {
            return View();
        }
        public ActionResult VehicleSalesDashBoard() 
        {
            return View();
        }

        public JsonResult Insert_DealerParent(OEMDealerVM objects)
        {   
            bool result = false;

            string msg = "Failed to save record..";

            result = OEMMethods.Insert_ParentDealer(objects );

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete_DealerDetails(string value)
        {
            bool result = false;

            string msg = "Data can't be deleted";

            result = OEMMethods.Delete_DealerDetailRecord (value);

            if (result)
            {
                msg = "Successfully Deleted";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Select_DealerDetail(string PDealerCode, string PDealerDesc)
        {
            string data = "";
            bool result = false;
            data = OEMMethods.Get_DealerDetails(PDealerCode, PDealerDesc);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Select_DealerDetailsData(string value)
        {
            string data = "";
            bool result = false;
            data = OEMMethods.Get_DealerDetailsData(value);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Select_DealerCatData()
        {
            

            string data = "";
            bool result = false;
            data = OEMMethods.Get_DealerCatData(dealerCode);
            if (!string.IsNullOrEmpty(data)) 
            {
                result = true;

            }
            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Select_DealerDetailsInsertion(string value)
        {
            string data = "";
            bool result = false;
            data = OEMMethods.Get_DealerDetailsInsertion(value);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Insert_DealerDetails(OEMDealerVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = OEMMethods.Insert_DealerDetail(objects);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Insert_DealerCatData(OEMDealerVM objects)
        {

            bool result = false;

            string msg = "Failed to save record..";

            result = OEMMethods.Insert_DealerCatDetail(objects,dealerCode);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        //job LaborMaster /abdussaboor

        public ActionResult LaborMaster()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlBrand = new List<SelectListItem>();
            ddlBrand = GeneralMethods.GetDataFromSpWithDealerCode("Select_Brand",dealerCode);
            ViewBag.ddlBrand = ddlBrand;

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            ViewBag.ddlAllDealer = ddlAllDealer;

            List<SelectListItem> ddlproduct = new List<SelectListItem>();
            ddlproduct = GeneralMethods.GetDataFromSp("Select_Product");
            ViewBag.ddlproduct = ddlproduct;

            List<SelectListItem> ddlversion = new List<SelectListItem>();
            ddlversion = GeneralMethods.GetDataFromSp("Select_Version");
            ViewBag.ddlversion = ddlversion;

            List<SelectListItem> ddlVendor = new List<SelectListItem>();
            ddlVendor = GeneralMethods.GetDataFromSp("Select_Vendor");
            ViewBag.Vendor = ddlVendor;
            return View();


        }
        //job Master /abdussaboor
        public ActionResult JobMaster()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlBrand = new List<SelectListItem>();
            ddlBrand = GeneralMethods.GetDataFromSpWithDealerCode("Select_Brand", dealerCode);
            ViewBag.ddlBrand = ddlBrand;

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            ViewBag.ddlAllDealer = ddlAllDealer;

            List<SelectListItem> ddlproduct = new List<SelectListItem>();
            ddlproduct = GeneralMethods.GetDataFromSp("Select_Product");
            ViewBag.ddlproduct = ddlproduct;

            List<SelectListItem> ddlversion = new List<SelectListItem>();
            ddlversion = GeneralMethods.GetDataFromSp("Select_Version");
            ViewBag.ddlversion = ddlversion;

            List<SelectListItem> ddlVendor = new List<SelectListItem>();
            ddlVendor = GeneralMethods.GetDataFromSp("Select_Vendor");
            ViewBag.Vendor = ddlVendor;


            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlBay = new List<SelectListItem>();
            ddlBay = GeneralMethods.GetDataFromSpWithDealerCode("Get_Bay", dealerCode);
            ViewBag.ddlBay = ddlBay;


            return View();


        }

        //job Card Type /abdussaboor
        public ActionResult JobCardType()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlBrand = new List<SelectListItem>();
            ddlBrand = GeneralMethods.GetDataFromSpWithDealerCode("Select_Brand", dealerCode);
            ViewBag.ddlBrand = ddlBrand;

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            ViewBag.ddlAllDealer = ddlAllDealer;

            List<SelectListItem> ddlproduct = new List<SelectListItem>();
            ddlproduct = GeneralMethods.GetDataFromSp("Select_Product");
            ViewBag.ddlproduct = ddlproduct;

            List<SelectListItem> ddlversion = new List<SelectListItem>();
            ddlversion = GeneralMethods.GetDataFromSp("Select_Version");
            ViewBag.ddlversion = ddlversion;

            List<SelectListItem> ddlVendor = new List<SelectListItem>();
            ddlVendor = GeneralMethods.GetDataFromSp("Select_Vendor");
            ViewBag.Vendor = ddlVendor;


            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlBay = new List<SelectListItem>();
            ddlBay = GeneralMethods.GetDataFromSpWithDealerCode("Get_Bay", dealerCode);
            ViewBag.ddlBay = ddlBay;


            return View();




        }

        //Bank Master /abdussaboor
        public ActionResult BankMaster()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlBrand = new List<SelectListItem>();
            ddlBrand = GeneralMethods.GetDataFromSpWithDealerCode("Select_Brand", dealerCode);
            ViewBag.ddlBrand = ddlBrand;

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            ViewBag.ddlAllDealer = ddlAllDealer;

            List<SelectListItem> ddlproduct = new List<SelectListItem>();
            ddlproduct = GeneralMethods.GetDataFromSp("Select_Product");
            ViewBag.ddlproduct = ddlproduct;

            List<SelectListItem> ddlversion = new List<SelectListItem>();
            ddlversion = GeneralMethods.GetDataFromSp("Select_Version");
            ViewBag.ddlversion = ddlversion;

            List<SelectListItem> ddlVendor = new List<SelectListItem>();
            ddlVendor = GeneralMethods.GetDataFromSp("Select_Vendor");
            ViewBag.Vendor = ddlVendor;


            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlBay = new List<SelectListItem>();
            ddlBay = GeneralMethods.GetDataFromSpWithDealerCode("Get_Bay", dealerCode);
            ViewBag.ddlBay = ddlBay;



            return View();

        }

        //City Master /abdussaboor
        public ActionResult CityMaster()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlBrand = new List<SelectListItem>();
            ddlBrand = GeneralMethods.GetDataFromSpWithDealerCode("Select_Brand", dealerCode);
            ViewBag.ddlBrand = ddlBrand;

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            ViewBag.ddlAllDealer = ddlAllDealer;

            List<SelectListItem> ddlproduct = new List<SelectListItem>();
            ddlproduct = GeneralMethods.GetDataFromSp("Select_Product");
            ViewBag.ddlproduct = ddlproduct;

            List<SelectListItem> ddlversion = new List<SelectListItem>();
            ddlversion = GeneralMethods.GetDataFromSp("Select_Version");
            ViewBag.ddlversion = ddlversion;

            List<SelectListItem> ddlVendor = new List<SelectListItem>();
            ddlVendor = GeneralMethods.GetDataFromSp("Select_Vendor");
            ViewBag.Vendor = ddlVendor;


            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlBay = new List<SelectListItem>();
            ddlBay = GeneralMethods.GetDataFromSpWithDealerCode("Get_Bay", dealerCode);
            ViewBag.ddlBay = ddlBay;



            return View();

        }

        //State Master /abdussaboor
        public ActionResult StateMaster()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlBrand = new List<SelectListItem>();
            ddlBrand = GeneralMethods.GetDataFromSpWithDealerCode("Select_Brand", dealerCode);
            ViewBag.ddlBrand = ddlBrand;

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            ViewBag.ddlAllDealer = ddlAllDealer;

            List<SelectListItem> ddlproduct = new List<SelectListItem>();
            ddlproduct = GeneralMethods.GetDataFromSp("Select_Product");
            ViewBag.ddlproduct = ddlproduct;

            List<SelectListItem> ddlversion = new List<SelectListItem>();
            ddlversion = GeneralMethods.GetDataFromSp("Select_Version");
            ViewBag.ddlversion = ddlversion;

            List<SelectListItem> ddlVendor = new List<SelectListItem>();
            ddlVendor = GeneralMethods.GetDataFromSp("Select_Vendor");
            ViewBag.Vendor = ddlVendor;


            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlBay = new List<SelectListItem>();
            ddlBay = GeneralMethods.GetDataFromSpWithDealerCode("Get_Bay", dealerCode);
            ViewBag.ddlBay = ddlBay;



            return View();

        }

        

        public JsonResult Select_DTRDetailHCV(string value1, string value2)
        {
            string data = "";
            bool result = false;
            data = OEMMethods.Get_LabourDetail(value1, value2);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Labour()
        {

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlBrand = new List<SelectListItem>();
            ddlBrand = GeneralMethods.GetDataFromSpWithDealerCode("Select_Brand", dealerCode);
            ViewBag.ddlBrand = ddlBrand;

            List<SelectListItem> ddlproduct = new List<SelectListItem>();
            ddlproduct = GeneralMethods.GetDataFromSp("Select_Product");
            ViewBag.ddlproduct = ddlproduct;

            List<SelectListItem> ddlversion = new List<SelectListItem>();
            ddlversion = GeneralMethods.GetDataFromSp("Select_Version");
            ViewBag.ddlversion = ddlversion;

            List<SelectListItem> ddlVendor = new List<SelectListItem>();
            ddlVendor = GeneralMethods.GetDataFromSp("Select_Vendor");
            ViewBag.Vendor = ddlVendor;
            return View();

        }

        //job category_main /yousuf

        public ActionResult JobCategory()
        {
            
            if(string.IsNullOrEmpty((string)Session["DealerCode"])){
                return RedirectToAction("Login","Home");
            }

            dealerCode = Session["DealerCode"].ToString();
            return View();

        }


      //job category insertion


        public JsonResult Insert_JobCategory(OEMJobCategoryVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = OEMMethods.Insert_JobCategory(objects);
            
            if(result){
                msg="Successfully Added";
            }
            return Json(new{Success=result,Message=msg},JsonRequestBehavior.AllowGet);

        }
        //Save Labor Detail
        public JsonResult Insert_LaborDetail(OEM_LabourMaster objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = OEMMethods.Insert_LaborDetail(objects,ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Insert_JobMaster(OEM_LabourMaster objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = OEMMethods.Insert_JobMaster(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
        //for JobCardMaster
        public JsonResult Insert_JobCardMaster(OEMJobCategoryVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = OEMMethods.Insert_JobCardMaster(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }

        //for Save BankMaster
        public JsonResult Insert_BankMaster(OEMJobCategoryVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = OEMMethods.Insert_BankMaster(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }

        //for Save CityMaster
        public JsonResult Insert_CityMaster(OEMJobCategoryVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = OEMMethods.Insert_CityMaster(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }

        //for Save StateMaster
        public JsonResult Insert_StateMaster(OEMJobCategoryVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = OEMMethods.Insert_StateMaster(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }


        //get jobcategory into details
        public JsonResult Select_JobCategoryDetails(string JobCatCode, string JobCatDesc)
        {
            string data = "";
            bool result = false;
            data = OEMMethods.Get_JobCategoryDetails(JobCatCode, JobCatDesc);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_JobDetails(string JobCatCode, string JobCatDesc)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = OEMMethods.Get_JobDetails(DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_JobTypeDetail(string JobCatCode, string JobCatDesc)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = OEMMethods.Get_JobTypeMaster(DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Select_BankMasterDetail(string JobCatCode, string JobCatDesc)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = OEMMethods.Get_BankMaster(DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Select_CityMasterDetail(string JobCatCode, string JobCatDesc)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = OEMMethods.Get_CityMaster(DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Select_StateMasterDetail(string JobCatCode, string JobCatDesc)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = OEMMethods.Get_StateMaster(DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult JobCategory(string JobCatCode, string JobCatDesc)
        {
            string data = "";
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = OEMMethods.Get_JobCategory(DealerCode);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }


        //delete job category record
        public JsonResult Delete_JobCategoryDetails(string value)
        {
            bool result = false;

            string msg = "Data can't be deleted";

            result = OEMMethods.Delete_JobCategoryRecord(value);

            if (result)
            {
                msg = "Successfully Deleted";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        //delete job category record
        public JsonResult Delete_LaborDetails(string value,string ProdCode,string DealerCode)
        {
            bool result = false;

            string msg = "Data can't be deleted";

            result = OEMMethods.Delete_LaborRecord(value,ProdCode,DealerCode);

            if (result)
            {
                msg = "Successfully Deleted";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        //jobcategoryreport
        public ActionResult Export()
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
        {
                //new SqlParameter("@JobCatCode",SqlDbType.Char),
                //new SqlParameter("@JobCatDesc",SqlDbType.VarChar)

            };

        //    param[0].Value = JobCatCode;
        //    param[1].Value = JobCatDesc;

            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("SP_selectJobCategoryDetails", param, ref rder))
            {
                data.SP_selectjobcategorydetail.Load(rder);

            }
            RD.Load(Server.MapPath("~/Reports/CrystalReport1.rpt"));


            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Event Report'";
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
                return File(stream, "testReport.pdf");
            }
            catch
            {
                throw;
            }


        }


    }
}
   