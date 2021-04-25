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
using Core.CRM.Helper;
using CConn;
using System.Configuration;


namespace CRM_V3.Controllers
{
    public class DTRController : Controller
    {
        // GET: DTR
        SysFunction sysfun = new SysFunction();
      
       
       
        DataTable dt,imgdt;
        DataSet ds;
        string DealerCode = string.Empty;
        string CCon = CConn.CConnection.GetConnectionString().ToString();
        string id, dealer, close,imgprint;
        Byte[] img;
        [HttpGet]
        public JsonResult DTRRedirectLCV(string EnquiryId = "", string EnDealerCode = "", string EnCLose = "")
        {
          bool result=false;
            DealerCode = Session["DealerCode"].ToString();
            Session["id"]=EnquiryId;
            Session["dealer"]= EnDealerCode;

            Session["close"]  = EnCLose;

            return Json(new { Success = result }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult DTRRedirectHCV(string EnquiryId = "", string EnDealerCode = "", string EnCLose = "")
        {
            bool result = false;
            DealerCode = Session["DealerCode"].ToString();
            Session["id"] = EnquiryId;
            Session["dealer"] = EnDealerCode;

            Session["close"] = EnCLose;

            return Json(new { Success = result }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult DTR()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            DealerCode = Session["DealerCode"].ToString();
            ViewBag.UrlLeadId = Session["id"].ToString();
            ViewBag.UrlDealerCode = Session["dealer"].ToString();
            ViewBag.EnCLose = Session["close"].ToString();
          //  Session["DealerCode"] = "";
            Session["id"] ="";
            Session["dealer"] = "";
            return View();
        }

        public ActionResult DTRAppr_Reject_LCV()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            DealerCode = Session["DealerCode"].ToString();
            return View();
        }
        public ActionResult DTRAppr_Reject_HCV()
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            DealerCode = Session["DealerCode"].ToString();
            return View();
        }
        public ActionResult HCVDTR(string EnquiryId = "", string EnDealerCode = "", string EnCLose = "")
        {
            if (string.IsNullOrEmpty((string)Session["DealerCode"]))
            {
                return RedirectToAction("Login", "Home");
            }
            DealerCode = Session["DealerCode"].ToString();
            ViewBag.UrlLeadId = Session["id"].ToString();
            ViewBag.UrlDealerCode = Session["dealer"].ToString();
            ViewBag.EnCLose = Session["close"].ToString();
            Session["id"] = "";
            Session["dealer"] = "";
            return View();
        }
        public JsonResult Select_DTRDetailHCV(string value)
        {
            string data = "";
            bool result = false;
            data = DTRMethods.Get_DTRDetailDataHCV(value);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_GetImage(string value, string value1)
        {
            string data = "";
            List<DTRVM> lst;
            SqlDataReader rder = null;
            bool result = false;
            // lst = DTRMethods.Get_DTRDetailImage(value, value1);
            try
            {
                
                SqlParameter[] sqlParam = {
                    new SqlParameter("@DealerCode",value),//1
                    new SqlParameter("@JobCardCode",value1)//1
                                        };
                List<DTRVM> DTRlst= new List<DTRVM>();

                if (sysfun.ExecuteSP("sp_W2_FIR_ImageDetail_Select", sqlParam, ref rder))
                {

                    while (rder.Read())
                    {
                        byte[] myimg = (byte[])rder["ImageData"];
                        DTRlst.Add(new DTRVM
                        {
                            FIRCode = rder["FIRCode"].ToString(),
                            /// Name = sdr["Name"].ToString(),
                            //ContentType = sdr["ContentType"].ToString(),
                            Imagestring = "data:image/png;base64," + Convert.ToBase64String(myimg, 0, myimg.Length)
                        });
                   // Session["Image"] = "data:image/png;base64," + Convert.ToBase64String(myimg, 0, myimg.Length);
                  
                        
                    }

                  

                }
                var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                data = Serializer.Serialize(DTRlst);

                if (!string.IsNullOrEmpty(data))
                {
                    result = true;
                }



            }
            catch (Exception ex)
            {

               // throw;
            }
            //Session["DTRImages"] = lst1;
            //List<DTRVM> ExchangeCodelst = (List<DTRVM>)Session["DTRImages"];



            //DTRVM images =lst1.Find(a=>a.FIRCode==value1);
            ////lst.Where(a => a.FIRCode.Trim() == value1.Trim());
            ////lst.Find(p => p.FIRCode ==value1);
            //if (images != null)
            //{
            //    result = true;
            //    ViewBag.Base64String = "data:image/png;base64," + Convert.ToBase64String(images.ImageData, 0, images.ImageData.Length);
            //}

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_DTR_Close_LCV(string value)
        {
            string data = "";
            bool result = false;
            data = DTRMethods.Get_DTR_CloseLCV(value);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_DTR_Close_HCV(string value)
        {
            string data = "";
            bool result = false;
            data = DTRMethods.Get_DTR_CloseHCV(value);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_DTRDetail(string value)
        {
            string data = "";
            bool result = false;
            data = DTRMethods.Get_DTRDetailData(value);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_FIRRPartsData(string value, string value1)
        {
            string data = "";
            bool result = false;
            data = DTRMethods.Get_DTRPartsData(value, value1);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_DTRDetails(string value, string value1)
        {
            string data = "";
            bool result = false;
            data = DTRMethods.Get_DTRDetail(value, value1);

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Insert_FIRMaster(DTRVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = DTRMethods.Insert_FIRMasterData(objects);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        ///DTR Print For LCV and HCV 
        ///
        public ActionResult DTR_HCV(string dealerCode, string EnquiryId)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
           {
               //new SqlParameter("@DealerCode",Session["DealerCode"]),
                        
             new SqlParameter("@DealerCode",SqlDbType.Char),//0
             new SqlParameter("@FIRCode",SqlDbType.VarChar)//1
            };


            param[0].Value = dealerCode;
            param[1].Value = EnquiryId;
           


            SqlDataReader rder = null;


            if (sysfun.ExecuteSP("sp_FIR_Reports", param, ref rder))
            {
                data.EnforceConstraints = false;
                data.sp_GetFIRMaster.Load(rder);
                dt = data.sp_GetFIRMaster;
                SqlParameter[] param1 =
        {
             new SqlParameter("@DealerCode",SqlDbType.Char),//0
             new SqlParameter("@JobCardCode",SqlDbType.VarChar)//1
            };
                param1[0].Value = dealerCode;
                param1[1].Value = EnquiryId;
                ds = DataAccess.getDataSet("sp_FIRPartsDetail_Print", param1, CCon);
                 //   ExecuteDataset(CCon, CommandType.Text, "sp_FIRPartsDetail_Print'" +dealerCode + "','" + EnquiryId + "'");
                data.sp_FIRPartsDetail_Print.Load(ds.CreateDataReader());
            }

            RD.Load(Server.MapPath("~/Reports/HCVDTR(New).rpt"));


            RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Dealer Technical Report'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
           // RD.DataDefinition.FormulaFields["Pic1"].Text = "'" + Server.MapPath("~") + dt.Rows[0]["item"].ToString() + "'";

            RD.Database.Tables[0].SetDataSource(data);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "VSReport.pdf");
            }
            catch
            {
                throw;
            }


        }
        public ActionResult DTR_LCV(string dealerCode, string EnquiryId)
        {
            DSReports data = new DSReports();
            ReportDocument RD = new ReportDocument();
            string sql;
            SqlParameter[] param =
           {
               //new SqlParameter("@DealerCode",Session["DealerCode"]),
                        
             new SqlParameter("@DealerCode",SqlDbType.Char),//0
             new SqlParameter("@FIRCode",SqlDbType.VarChar)//1
            };


            param[0].Value = dealerCode;
            param[1].Value = EnquiryId;



            SqlDataReader rder = null;
            SqlParameter[] param1 =
          {          
             new SqlParameter("@DealerCode",SqlDbType.Char),//0
             new SqlParameter("@JobCardCode",SqlDbType.VarChar)//1
            };
            param1[0].Value = dealerCode;
            param1[1].Value = EnquiryId;
            if (sysfun.ExecuteSP("sp_FIR_Reports", param, ref rder))
            {
                data.EnforceConstraints = false;
                data.sp_GetFIRMaster.Load(rder);
                dt = data.sp_GetFIRMaster;
                sql = "Sp_FIRImage_Print '" + dealerCode + "','" + EnquiryId + "'";
                imgdt = sysfun.GetData(sql);
                if (imgdt.Rows.Count > 0)
                {
                   // img = Convert.ToByte(imgdt.Rows[0]["ImageData"]);
                    ///imgprint = "data:image/png;base64," + Convert.ToBase64String(imgdt.Rows[0]["ImageData"], 0, imgdt.Rows[0]["ImageData"].Length)
                }
                ds = DataAccess.getDataSet("sp_FIRPartsDetail_Print", param1, CCon);

                //   ExecuteDataset(CCon, CommandType.Text, "sp_FIRPartsDetail_Print'" +dealerCode + "','" + EnquiryId + "'");
                data.sp_FIRPartsDetail_Print.Load(ds.CreateDataReader());
            }

            RD.Load(Server.MapPath("~/Reports/rptDTR.rpt"));


            RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Dealer Technical Report'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
           // RD.DataDefinition.FormulaFields["Pic1"].Text = imgprint;

            RD.Database.Tables[0].SetDataSource(data);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "VSReport.pdf");
            }
            catch
            {
                throw;
            }


        }
    }
}