﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM_V3.Controllers
{
    public class CRMController : Controller
    {
        //
        // GET: /CRM/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CRMDashBoard() 
        {
            return View();
        }
	}
}