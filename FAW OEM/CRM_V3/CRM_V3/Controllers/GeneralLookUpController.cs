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
    public class GeneralLookUpController : Controller
    {
        // GET: GeneralLookUp
        string DealerCode;
        public ActionResult LookUp(int PageNumber = 1, int RowsPerPage = 20)
        {
            int skip = (RowsPerPage * (PageNumber - 1));

            if (Session["DealerCode"] == null || string.IsNullOrEmpty(Session["DealerCode"].ToString()))
            {
                return RedirectToAction("Login", "Home");
            }
            DealerCode = Session["DealerCode"].ToString();

            List<ItemMasterVM> lst = new List<ItemMasterVM>();
            DataTable dt = new DataTable();

            List<ItemMasterVM> ddlSMSTemp = new List<ItemMasterVM>();
            ddlSMSTemp = ItemMasterMethods.Get_ItemMaster(DealerCode, Session["VehicleCategory"].ToString());
            ViewBag.SMSTemp = ddlSMSTemp;


            var list = (List<ItemMasterVM>)Session["ItemList"];

            if (list == null)
            {
                list = ItemMasterMethods.Get_ItemMaster(DealerCode, Session["VehicleCategory"].ToString()).ToList();
            }

            //SqlParameter[] sqlParam =
            //    {
            //        new SqlParameter("@DealerCode",DealerCode)
            //    };
            //dt = DataAccess.getDataTable("SP_Select_CustomerModal", sqlParam, General.GetBMSConString());

            //if (dt.Rows.Count > 0)
            //{
            //    lst = EnumerableExtension.ToList<Customer>(dt);
            //}

            //ViewBag.Customer = lst;

            var _dataList = new List<ItemMasterVM>();
            _dataList = list.Skip(skip).Take(RowsPerPage).ToList();


            ViewBag.Records = list.Count / RowsPerPage + 1;
            ViewBag.PageNumber = PageNumber;

            //if (Request.IsAjaxRequest())
            //{
            //    return PartialView("CustomerModal", _dataList);
            //}


            return View();
           
        }
        public ActionResult GetPaggedData(int pageNumber = 1, int pageSize = 20)
        {
            DealerCode = Session["DealerCode"].ToString();
            List<ItemMasterVM> listData = ItemMasterMethods.Get_ItemMaster(DealerCode, Session["VehicleCategory"].ToString());
            var pagedData = Pagination.PagedResult(listData, pageNumber, pageSize);
            return Json(pagedData, JsonRequestBehavior.AllowGet);
        }
    }
}