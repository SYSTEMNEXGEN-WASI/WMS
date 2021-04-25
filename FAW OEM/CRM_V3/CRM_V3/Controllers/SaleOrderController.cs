using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM_V3.Controllers
{
    public class SaleOrderController : Controller
    {
        // GET: SaleOrder
        public ActionResult SaleOrder()
        {
            return View();
        }
        public ActionResult PartSalesDashBoard() 
        {
            return View();
        }
    }
}