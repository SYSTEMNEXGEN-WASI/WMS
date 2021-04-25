using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM_V3.Models;
using System.Data.Entity.Validation;

namespace CRM_V3.Controllers
{
    public class CustomerController : Controller
    {
       
        public ActionResult Customer()
        {
            return View();
        }
            
    }
    }