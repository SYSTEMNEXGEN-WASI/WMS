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
using Jquery_Mvc_Paging.Helper;
namespace CRM_V3.Controllers
{
    public class ItemMasterController : Controller
    {
        // GET: ItemMaster
        static string dealerCode = string.Empty;
        string DealerCode;
        //static var pagedData;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ItemMaster(int PageNumber = 1, int RowsPerPage = 20)
        {
            int skip = (RowsPerPage * (PageNumber - 1));

            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }


            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlItemCatDesc = new List<SelectListItem>();
            ddlItemCatDesc = GeneralMethods.GetDataFromSpWithDealerCode("Get_ItemCategory", dealerCode);
            ViewBag.ddlItemCatDesc = ddlItemCatDesc;

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlUnit = new List<SelectListItem>();
            ddlUnit = GeneralMethods.GetDataFromSpWithDealerCode("Select_Unit", dealerCode);
            ViewBag.ddlUnit = ddlUnit;

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlPartType = new List<SelectListItem>();
            ddlPartType = GeneralMethods.GetDataFromSpWithDealerCode("Get_Unit", dealerCode);
            ViewBag.ddlPartType = ddlPartType;

            dealerCode = Session["DealerCode"].ToString();
            List<SelectListItem> ddlAllDealer = new List<SelectListItem>();
            ddlAllDealer = GeneralMethods.GetDataFromSpWithDealerCode("Get_AllDealer", dealerCode);
            ViewBag.ddlAllDealer = ddlAllDealer;
            List<AccountVM> lstAccount = ItemMasterMethods.Get_AccDesc(dealerCode);
            ViewBag.Accounts = lstAccount;

            DealerCode = Session["DealerCode"].ToString();
           
            List<ItemMasterVM> ddlSMSTemp = new List<ItemMasterVM>();
            ddlSMSTemp = ItemMasterMethods.Get_ItemMaster(DealerCode, Session["VehicleCategory"].ToString());
            ViewBag.SMSTemp = ddlSMSTemp;


            var list = (List<ItemMasterVM>)Session["ItemList"];

            if (list == null)
            {
                list = ItemMasterMethods.Get_ItemMaster(DealerCode, Session["VehicleCategory"].ToString()).ToList();
            }
            var _dataList = new List<ItemMasterVM>();
            _dataList = list.Skip(skip).Take(RowsPerPage).ToList();


            ViewBag.Records = list.Count / RowsPerPage + 1;
            ViewBag.PageNumber = PageNumber;

            return View();

        }
        public ActionResult GetPaggedData(int pageNumber = 1, int pageSize = 10)
        {
            
            DealerCode = Session["DealerCode"].ToString();
           
            List<ItemMasterVM> listData = ItemMasterMethods.Get_ItemMaster(DealerCode, Session["VehicleCategory"].ToString());
            Session["ItemList"] = listData;
            var pagedData = Pagination.PagedResult(listData, pageNumber, pageSize);
            if (string.IsNullOrEmpty((string)Session["Search"]))
            {
               pagedData = Pagination.PagedResult(listData, pageNumber, pageSize);
            }
            else
            {
                List<ItemMasterVM> data = (List<ItemMasterVM>)Session["Item"];
                 pagedData = Pagination.PagedResult(data, pageNumber, pageSize);
               
            }
            Session["Search"] = "";
          

            return Json(pagedData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SearchModal(string value1)
        {
            DealerCode = Session["DealerCode"].ToString();
            List<ItemMasterVM> data= ItemMasterMethods.Get_ItemMaster(DealerCode, Session["VehicleCategory"].ToString());
           
            bool result = false;
            Session["Search"] = "Search";
            if (data.Count > 0)
            {
                data = data.Where(i => i.ItemCode.Contains(value1) || i.PartItemNo.Contains(value1) || i.ItemDesc.Contains(value1) || i.ItemCatDesc.Contains(value1)).ToList();
            }
            
            Session["Item"] = data;
            if (data.Count > 0)
            {
                result = true;
            }

            return PartialView("LookUp",data);
        }

        [HttpGet]
        public ActionResult Select_From_Modal(string value1)
        {

            List<ItemMasterVM> data = (List<ItemMasterVM>)Session["ItemList"];
            bool result = false;
            Session["Search"] = "Search";
            data = data.Where(i => i.ItemCode.Trim()==value1.ToString().Trim()).ToList();
            Session["Select_Item"] = data;
            if (data.Count > 0)
            {
                result = true;
            }

            return PartialView("LookUp", data);
        }


        //ItemMaster/Select_ItemMasterDetail/AbdusSaboor
        [HttpGet]
        public JsonResult Select_ItemMasterDetail()
        {
            List<ItemMasterVM> data;
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = ItemMasterMethods.Get_ItemMaster(DealerCode,Session["VehicleCategory"].ToString());
            Session["ItemList"] = data;
            if (data.Count > 0)
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        //ItemMaster/Select_ItemMasterDetail2/AbdusSaboor
        [HttpGet]
        public JsonResult Select_ItemMasterDetail2(string value1, string value2)
        {
            List<ItemMasterVM> data;
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = ItemMasterMethods.Get_ItemMaster2(value1, value2);

            if (data.Count > 0)
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        //ItemMaster/Select_ProductCodeDetail/AbdusSaboor
        [HttpGet]
        public JsonResult Select_ProductDetail()
        {
            List<ItemMasterVM> data;
            bool result = false;
            string DealerCode = Session["DealerCode"].ToString();
            data = ItemMasterMethods.Get_Product(DealerCode, Session["VehicleCategory"].ToString());
            Session["ItemList"] = data;
            if (data.Count > 0)
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        //for delete Delete_ItemMaster/AbdusSaboor
        public JsonResult Delete_ItemMaster(ItemMasterVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = ItemMasterMethods.Delete_ItemMaster(objects, ref msg);

            if (result)
            {
                msg = "Record Delete Successfully";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
        //for Save ItemMaster/AbdusSaboor
        public JsonResult Insert_ItemMaster(ItemMasterVM objects)
        {

            bool result = false;
            string msg = "Failed to save record..";

            result = ItemMasterMethods.Insert_ItemMaster(objects, ref msg);

            if (result)
            {
                msg = "Successfully Added";
            }
            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);

        }
    }
}