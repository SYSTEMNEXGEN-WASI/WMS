using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM_V3.Controllers
{
    public class CustomerProfileController : Controller
    {
        //
        // GET: /CustomerProfile/
        
        public ActionResult Index()
        {

            return View();
        }
        //public ActionResult loaddata() 
        //{
            
        //}
        public ActionResult CustomerProfile()
        {
            return View();
        }
        public JsonResult Select_CustomerDetails()
        {
            string data = "";
            bool result = false;
            data = CusProfileMethods.Get_CustomerDetails();

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
	}
}