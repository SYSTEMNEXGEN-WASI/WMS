using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CrystalDecisions.Shared;
using DXBMS.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Threading;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;

namespace DXBMS.Modules.Reports
{
    public partial class CustomerWiseOutstandingSummary : System.Web.UI.Page
    {
        SysFunctions myFunc = new SysFunctions();
        SecurityBll sec = new SecurityBll();
        clsLookUp clslook = new clsLookUp();
        DataSet ds = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }

            if (IsPostBack)
            {
                if (!string.IsNullOrEmpty(Session["LookUpData"].ToString()))
                {
                    SelectedPartDetail(Session["LookUpData"].ToString());
                }                
            }

            if (Request.Params.Get("Name") != null)
            {
                ViewState["SparePart"] = Request.QueryString["Name"].ToString();

                if (ViewState["SparePart"].ToString() == "PTV")
                {
                    if (!sec.UserRight("2568", "001"))
                    {
                        Response.Redirect("~/Test.aspx");
                    }
                    lblHeading.Text = "Payment to Vendor Report";
                    lblCustomer.Visible = false;
                    txtCustomer.Visible = false;
                    imgLookup.Visible = false;
                }
                else if(ViewState["SparePart"].ToString() == "SA"){
                    if (!sec.UserRight("2569", "001"))
                    {
                        Response.Redirect("~/Test.aspx");
                    }
                    lblHeading.Text = "Stock Adjustment Detail Report";
                    lblCustomer.Visible = false;
                    txtCustomer.Visible = false;
                    imgLookup.Visible = false;
                }
                else if (ViewState["SparePart"].ToString() == "IL")
                {
                    if (!sec.UserRight("2571", "001"))
                    {
                        Response.Redirect("~/Test.aspx");
                    }
                    lblHeading.Text = "Invoices Detail Report";
                    lblCustomer.Visible = false;
                    txtCustomer.Visible = false;
                    imgLookup.Visible = false;
                    txtCustomerDesc.Visible = false;
                }
                else if (ViewState["SparePart"].ToString() == "IR")
                {
                    if (!sec.UserRight("2590", "001"))
                    {
                        Response.Redirect("~/Test.aspx");
                    }
                    lblHeading.Text = "Invoices Revenue Report";
                    lblCustomer.Visible = false;
                    txtCustomer.Visible = false;
                    imgLookup.Visible = false;
                    txtCustomerDesc.Visible = false;
                }
                else if (ViewState["SparePart"].ToString() == "DS")
                {
                    if (!sec.UserRight("2572", "001"))
                    {
                        Response.Redirect("~/Test.aspx");
                    }
                    lblHeading.Text = "Direct Sales Detail Report";
                    lblCustomer.Visible = false;
                    txtCustomer.Visible = false;
                    imgLookup.Visible = false;
                }
                else if (ViewState["SparePart"].ToString() == "GRN")
                {
                    if (!sec.UserRight("2573", "001"))
                    {
                        Response.Redirect("~/Test.aspx");
                    }
                    lblHeading.Text = "GRN Detail Report";
                    lblCustomer.Visible = false;
                    txtCustomer.Visible = false;
                    imgLookup.Visible = false;
                }
                else
                {
                    if (!sec.UserRight("2570", "001"))
                    {
                        Response.Redirect("~/Test.aspx");
                    }
                    lblHeading.Text = "Customer Outstanding Report";
                    lblCustomer.Visible = true;
                    txtCustomer.Visible = true;
                    imgLookup.Visible = true;
                }
            }
            if (!IsPostBack)
            {
                ThreadStart childthreat = new ThreadStart(childthreadcall);
                Thread child = new Thread(childthreat);

                child.Start();

                Session["LookUpData"] = string.Empty;
                setInitialDates();
            }
        }

        public void childthreadcall()
        {
            try
            {

                string sQuery = "SP_SelectCustomerForLookUp '" + Session["DealerCode"].ToString() + "'";

                if (myFunc.ExecuteQuery(sQuery, ref ds))
                {
                    Session["Parts"] = ds;

                }

            }
            catch (ThreadAbortException e)
            {

                //lblmessage.Text += "<br /> child thread - exception";

            }
            finally
            {
                //lblmessage.Text += "<br /> child thread - unable to catch the  exception";
            }
        }

        private void setInitialDates()
        {
            txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }

        protected void SelectedPartDetail(string item)
        {


            //DataTable dt = new DataTable();
            //dt = myFunc.GetData("select ItemDesc,PartItemNo from Item where DealerCode='" + Session["DealerCode"].ToString() + "' And  ItemCode='" + item + "'");
            txtCustomer.Text = item;
            string sql = "Select CusDesc from Customer where CusCode='"+item+"' and DealerCode='"+Session["DealerCode"].ToString()+"'";

            DataTable dt = new DataTable();

            dt = myFunc.GetData(sql);

            if (dt.Rows.Count > 0)
            {

                txtCustomerDesc.Text = dt.Rows[0]["CusDesc"].ToString();

            }



        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            SysFunction SysFunc = new SysFunction();
            string FDate = SysFunc.SaveDate(txtFromDate.Text).ToString();
            string TDate = SysFunc.SaveDate(txtToDate.Text).ToString();
            PdfDocument outputDocument = new PdfDocument();
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string extension = "FileName.pdf";
            desktopPath += @"\" + extension;


            SqlDataReader rder;
            ReportDocument RD = new ReportDocument();

            DataTable dt = new DataTable();
            string sql;


            DataSet dsRpt = new DataSet();
            DXBMS.Data.DSReports objDsReports = new Data.DSReports();


            if (ViewState["SparePart"].ToString() == "PTV")
            {
                if (!sec.UserRight("2568", "006"))
                {
                    Response.Redirect("~/Test.aspx");
                }
                sql = "exec SP_PaymenttoVendor_Select '" + Session["DealerCode"].ToString() + "','" + FDate + "','" + TDate + "'";
                RD.Load(Server.MapPath("~/Modules/SpareParts/SpareReports/PaymentToVendorDetailReport.rpt"));
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "PAYMENT TO VENDOR DETAIL" + "\"";
            }
            else if (ViewState["SparePart"].ToString() == "SA")
            {
                if (!sec.UserRight("2569", "006"))
                {
                    Response.Redirect("~/Test.aspx");
                }
                sql = "exec SP_StockAdjustment_DetailReport '" + Session["DealerCode"].ToString() + "','" + FDate + "','" + TDate + "'";
                RD.Load(Server.MapPath("~/Modules/SpareParts/SpareReports/crtStockAdjustment.rpt"));
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Stock Adjustment Detail Report" + "\"";
            }
            else if (ViewState["SparePart"].ToString() == "IL")
            {
                if (!sec.UserRight("2571", "006"))
                {
                    Response.Redirect("~/Test.aspx");
                }
                sql = "exec sp2W_Spare_InvoiceDetail '" + Session["DealerCode"].ToString() + "','" + FDate + "','" + TDate + "'";
                RD.Load(Server.MapPath("../rptInvoices.rpt"));
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Invoices Detail Report" + "\"";
            }
            else if (ViewState["SparePart"].ToString() == "IR")
            {
                if (!sec.UserRight("2590", "006"))
                {
                    Response.Redirect("~/Test.aspx");
                }
                sql = "exec sp2W_Revenue_InvoiceDetail '" + Session["DealerCode"].ToString() + "','" + FDate + "','" + TDate + "'";
                RD.Load(Server.MapPath("../rptInvoiceRevenue.rpt"));
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Invoices Revenue Detail Report" + "\"";
            }
            else if (ViewState["SparePart"].ToString() == "DS")
            {
                if (!sec.UserRight("2572", "006"))
                {
                    Response.Redirect("~/Test.aspx");
                }
                string cond = "";
                sql = "Sp_2W_CounterSaleDetail_rpt '" + Session["DealerCode"].ToString() + "','" + FDate + "','" + TDate + "'" + cond;
                RD.Load(Server.MapPath("../rptCounterSaleDetail.rpt"));
                RD.DataDefinition.FormulaFields["FromCode"].Text = "\"" + Convert.ToDateTime(FDate.ToString()).ToString("dd/MM/yyyy") + "\"";
                RD.DataDefinition.FormulaFields["ToCode"].Text = "\"" + Convert.ToDateTime(TDate.ToString()).ToString("dd/MM/yyyy") + "\"";
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Direct Sale Detail Report" + "\"";
            }
            else if (ViewState["SparePart"].ToString() == "GRN")
            {
                if (!sec.UserRight("2573", "006"))
                {
                    Response.Redirect("~/Test.aspx");
                }
                sql = "exec sp_GRNDetailList '" + Session["DealerCode"].ToString() + "','" + FDate + "','" + TDate + "'";
                RD.Load(Server.MapPath("../rptPartsReceivedDetail.rpt"));
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "GRN DETAIL REPORT" + "\"";
                RD.DataDefinition.FormulaFields["FromCode"].Text = "'" + FDate + "'";
                RD.DataDefinition.FormulaFields["ToCode"].Text = "'" + TDate + "'";
            }
            else
            {
                if (!sec.UserRight("2570", "006"))
                {
                    Response.Redirect("~/Test.aspx");
                }
                sql = "exec sp_2W_Spares_Aging'" + Session["DealerCode"].ToString() + "','" + FDate + "','" + TDate + "' ,'"+txtCustomer.Text+"'";
                RD.Load(Server.MapPath("../rptCustomerWiseOutstandingSummary.rpt"));
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Customer Wise OutStanding Summary Report" + "\"";
            }

            dt = SysFunc.GetData(sql);
             
            RD.PrintOptions.PaperSize = PaperSize.PaperA4;
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["FromCode"].Text = "'" + FDate + "'";
            //RD.DataDefinition.FormulaFields["ToCode"].Text = "'" + TDate + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
            
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            RD.Database.Tables[0].SetDataSource(dt);

            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "Report.pdf";
            string File = FilePath + FileName;

         
            Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            DirectoryInfo info = new DirectoryInfo(FilePath);
            if (!info.Exists)
            {
                info.Create();
            }

            string path = Path.Combine(FilePath, FileName);
            using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
            {
                stream.CopyTo(outputFileStream);
            }
            stream.Dispose(); stream.Close();
            string URL = "../../../../Download/PrintReport.aspx";

            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";

            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
            //RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            //string URL;
            //URL = Page.ResolveUrl("~/Download/rptViewerService.aspx?FileName=") + FileName;
            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);


            //string URL = "../Criteria/POView.aspx?CusWise=" + "&ReportID=" + RptName.ToString() + "&fromDate=" + FromDate + "&ToDate=" + ToDate;
            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
        protected void btnLookup_Click(object sender, EventArgs e)
        {
            ViewState["lookupid"] = 1; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.CustomerLookup(imgLookup, ViewState["lookupid"].ToString(), "", "../../../../");            

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtCustomerDesc.Text = "";
            txtCustomer.Text = "";
            txtFromDate.Text= DateTime.Now.ToString("dd-MM-yyyy");
            txtToDate.Text= DateTime.Now.ToString("dd-MM-yyyy");
        }
    }
}