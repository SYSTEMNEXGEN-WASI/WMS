using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.Models;
using CRM_V3.Models;
using System.Configuration;

namespace CRM_V3.Controllers
{
  
    public class HomeController : Controller
    {
        SDBContext SContext = new SDBContext();
        SDDBContext DefinitionContext = new SDDBContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();  
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public bool Authenticate(string userName, string password)
        {
            bool value = false;
            var Authenticate = SContext.SecurityUsers.Where(s => s.LoginName == userName.Trim() && s.Password == password.Trim() && s.ActiveFlag == "Y").ToList();
            if (Authenticate.Count != 0)
            {
                string SBSAppCode = ConfigurationManager.AppSettings["SecurityAppCode"].ToString();
                foreach (var item in Authenticate)
                {
                    string uUserCode = item.UserCode.Trim();
                    string uUserCompanyCode = item.CompCode.Trim();
                    var SBSCompanyCode = (from a in DefinitionContext.Dealers where a.DealerCode == uUserCompanyCode select a.DealerCode).FirstOrDefault();
                    if (uUserCompanyCode == SBSCompanyCode)
                    {
                        var sCompanyApplications = (from a in SContext.CompanyApplications where a.CompanyCode == uUserCompanyCode && a.ActiveFlag == "Y" select a.ApplicationCode.Trim()).ToList();
                        if (sCompanyApplications.Contains(SBSAppCode))
                        {
                            string sSecUserApps = (from a in SContext.SecurityUserApplications where a.UserCode == uUserCode && a.ApplicationCode == SBSAppCode && a.AllowFlag == "Y" select a.ApplicationCode).FirstOrDefault();
                            if (!string.IsNullOrWhiteSpace(sSecUserApps))
                            {
                                System.Web.HttpContext.Current.Session["UserName"] = item.LoginName;
                                System.Web.HttpContext.Current.Session["UserCode"] = item.UserCode;
                                System.Web.HttpContext.Current.Session["CompanyCode"] = item.CompCode;
                                System.Web.HttpContext.Current.Session["CompanyDesc"] = DefinitionContext.Dealers.Where(x => x.DealerCode == item.CompCode).Select(x => x.DealerDesc).FirstOrDefault();
                                var UserRights = SContext.SecurityUserAccesses.Where(s => s.UserCode == item.UserCode && s.ApplicationCode == SBSAppCode).ToList();
                                System.Web.HttpContext.Current.Session["UserRights"] = UserRights;
                                List<SecurityUserAccess> ItemObj = new List<SecurityUserAccess>();
                                foreach (var items in UserRights)
                                {
                                    SecurityUserAccess master = new SecurityUserAccess();
                                    master.ID = items.ID;
                                    master.UserCode = items.UserCode;
                                    master.ApplicationCode = items.ApplicationCode;
                                    master.FormCode = items.FormCode;
                                    master.FormActionCode = items.FormActionCode;
                                    master.ActionValue = items.ActionValue;
                                    ItemObj.Add(master);
                                }
                                System.Web.HttpContext.Current.Session["UserRights"] = ItemObj;
                                value = true;
                            }
                        }
                    }
                    else
                    {
                        System.Web.HttpContext.Current.Session["LoginError"] = "User Name or Password Is Incorrect...!";
                    }

                }
            }
            return value;
        }

        [HttpPost]
        public ActionResult LoginRequest(string user , string pass)
        {
            SysFunction myFunc = new SysFunction();

            List<DealerInfoVM> result ;

            string msg = "Failed to save record..";

            //    result = GeneralMethods.LoginRequest(user,pass,ref msg);
            result = GeneralMethods.LoginRequestSecurity(user, pass, ref msg);

            if (result.Count > 0)
            {
                this.Session["UserID"] = result.FirstOrDefault().UserID;
                this.Session["DealerCode"] = result.FirstOrDefault().DealerCode;
                this.Session["UserName"] = myFunc.ActiveUserName(this.Session["UserID"].ToString(),Session["DealerCode"].ToString());

                this.Session["EmpName"] = result.FirstOrDefault().EmpName;
                this.Session["EmpCode"] = result.FirstOrDefault().EmpCode;

               
                this.Session["DealerDesc"] = result.FirstOrDefault().DealerDesc;
                this.Session["DealerAddress"] = result.FirstOrDefault().Address1 + ", " + result.FirstOrDefault().Address2 + ", " + result.FirstOrDefault().Address3;
                this.Session["DealerEmail"] = result.FirstOrDefault().Email;
                this.Session["DealerFax"] = result.FirstOrDefault().Fax;
                this.Session["DealerPhone"] = result.FirstOrDefault().Phone1 + "," + result.FirstOrDefault().Phone2;
                this.Session["DealerNTN"] = result.FirstOrDefault().NTN;
                this.Session["DealerSaleTaxNo"] = result.FirstOrDefault().SaleTaxNo;
                this.Session["Logo"] = result.FirstOrDefault().Logo;
                this.Session["VehicleCategory"] = result.FirstOrDefault().VehicleCategory;
                Authenticate(user, pass);
                return Json(new { result = "Redirect", url = Url.Action("Dashboard", "Home") });
                
            }
            return Json(new { result = "InvalidLogin"+msg }, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult Barchart()
        //{
        //    string data = "";
        //    bool result = false;

        //    data = DashboardMethods.BarChart();

        //    if (!string.IsNullOrEmpty(data))
        //    {
        //        result = true;
        //    }

        //    return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult Dashboard()
        {
            //string DealerCode = Session["DealerCode"].ToString();
            //Session["userId"] = "001";
            
            if(string.IsNullOrEmpty((string)Session["DealerCode"])){
                return RedirectToAction("Login","Home");
            }
            string DealerCode = Session["DealerCode"].ToString();
            List<SalesPersonPerformanceVM> model = new List<SalesPersonPerformanceVM>();
            model = DashboardMethods.Select_SalesPersonPerformance(DealerCode);
            //List<BarChartVM> model1 = new List<BarChartVM>();
            //ViewBag.model1 = DashboardMethods.BarChart();
           
            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Abandon();

            return RedirectToAction("Login","Home");
        }

    }
}