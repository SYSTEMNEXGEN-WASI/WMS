using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.Globalization;
using CrystalDecisions.Shared;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;

namespace DXBMS.Modules.Reports
{
    public partial class StockBalanceReport : System.Web.UI.Page
    {
        SysFunctions myFunc = new SysFunctions();
        SecurityBll sec = new SecurityBll();

        SqlDataReader rder;
        ReportDocument rpt;
        ReportDocument crReportDocument;
        string ReportID = "";
        ReportDocument RD = new ReportDocument();
        
        
        string CCon = ConfigurationManager.ConnectionStrings["BMSConnectionString"].ToString();


        public double SGPPurchasesFromPSMC;
        public double OtherPurchases;
        public double LubricantPurchases;

        public double SGOPurchases;

        public double SGPPartsConsumption;
        public double SGPPartsSales;
        public double SGPPartsSalesAccess;
        public double OtherPartsConsumption;
        public double OtherPartsSales;
        public double LubricantConsumption;

        public double LubricantSales;
        public double SGOConsumption;
        public double SGOConsumptionLiter;
        public double SGOSales;

        public double SGOSalesLiter;
        public double SGPStock_Opening;
        public double OtherStock_Opening;
        public double LubricantStock_Opening;

        public double SGO_Opening;
        public double SGPStock_Closing;
        public double OtherStock_Closing;
        public double LubricantStock_Closing;

        public double SGO_Closing;
        public double TotalPurchaseReturn;

        public double TotalSalesPlusStockReturn;
        public double PRSGP;

        public double PROther;
        public double TotalSalesReturn;
        public double SRSGP;

        public double SROther;
        public double TotalStockReturn;
        public double StockReturnSGP;

        public double StockReturnOther;
        public double TotalChargeOut;
        public double ChargeOutSGP;
        public double ChargeOutOther;



        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");
            }
            if (!sec.UserRight("2579", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (!IsPostBack)
            {
                DateTime dt = DateTime.Today;

                txtMonth.Text = dt.ToString("MM/yyyy");

                string where = "DealerCode = '" + Session["DealerCode"].ToString() + "'";

                string[] col = { "VendorCode", "VendorDesc" };

                myFunc.GetMultiColumnsDDL(ddlVendor, col, "Vendor", where, "VendorCode", "", false, false);
            }

           
        }

        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtVendor.Text = ddlVendor.SelectedItem.Text;
            lblMessage.Text = "";
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2579", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (ddlVendor.SelectedIndex == 0)
            {
                lblMessage.Text = "Please select vendor";
                return;
            }

            string mn = txtMonth.Text.Substring(0, 2);
            string yr = txtMonth.Text.Substring(3, 4);

            string strdate = yr + "/" + mn + "/01";

            if (!myFunc.isDateTime(strdate))
            {
                lblMessage.Text = "Please enter valid month";
                txtMonth.Focus();
                return;
            }

            lblMessage.Text = "Creating Report";

            SqlDataReader reader = null;
            string sTemp = "", StrVendorCode = "", StrMonth = "", StrYear = "";

            StrMonth = mn;
            StrYear = yr;
            StrVendorCode = ddlVendor.SelectedValue;

            RD.Load(Server.MapPath("../rptStockBalanceNew.rpt"));

            RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
           // RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Stock Summary'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'User'";
            //RD.DataDefinition.FormulaFields["FromCode"].Text = "'For the Month " + MonthName(Mid(txtMonth._String, 1, 2)) + " " & Mid(txtMonth._String, 4, 7) & "'"
            RD.DataDefinition.FormulaFields["FromCode"].Text = "'For the Month " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(StrMonth)) + " " + StrYear + "'";

            if (myFunc.ExecuteQuery("Exec sp_StockBalance_TotalJobCards '" + Session["DealerCode"].ToString() + "','" + myFunc.SaveDate("01/" + StrMonth + "/" + StrYear) + "'", ref  reader) == true)

                while (reader.Read())
                {
                    sTemp = reader[0].ToString();
                }
            reader.Close();
            GetStockBalanceData(StrVendorCode, StrMonth, StrYear);

            RD.DataDefinition.FormulaFields["SGPPurchasesFromPSMC"].Text = "'" + SGPPurchasesFromPSMC.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["OtherPurchases"].Text = "'" + OtherPurchases.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["LubricantPurchases"].Text = "'" + LubricantPurchases.ToString("n") + "' ";
            //RD.DataDefinition.FormulaFields["SGOPurchases"].Text = "'" + SGOPurchases.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["SGPPartsConsumption"].Text = "'" + SGPPartsConsumption.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["SGPPartsSales"].Text = "'" + SGPPartsSales.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["SGPPartsSalesAccess"].Text = "'" + SGPPartsSalesAccess.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["OtherPartsConsumption"].Text = "'" + OtherPartsConsumption.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["OtherPartsSales"].Text = "'" + OtherPartsSales.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["LubricantConumptionAndSales"].Text = "'" + (LubricantConsumption + LubricantSales).ToString("n") + "' ";

            //RD.DataDefinition.FormulaFields["SGOConsumption"].Text = "'" + SGOConsumption.ToString("n") + "' ";
            //RD.DataDefinition.FormulaFields["SGOConsumptionLiter"].Text = "'" + SGOConsumptionLiter.ToString("n") + "' ";
            //RD.DataDefinition.FormulaFields["SGOSales"].Text = "'" + SGOSales.ToString("n") + "' ";
            //RD.DataDefinition.FormulaFields["SGOSalesLiter"].Text = "'" + SGOSalesLiter.ToString("n") + "' ";

            RD.DataDefinition.FormulaFields["SGPStock_Opening"].Text = "'" + SGPStock_Opening.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["OtherStock_Opening"].Text = "'" + OtherStock_Opening.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["LubricantStock_Opening"].Text = "'" + LubricantStock_Opening.ToString("n") + "' ";
            //RD.DataDefinition.FormulaFields["SGO_Opening"].Text = "'" + SGO_Opening.ToString("n") + "' ";

            RD.DataDefinition.FormulaFields["SGPStock_Closing"].Text = "'" + SGPStock_Closing.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["OtherStock_Closing"].Text = "'" + OtherStock_Closing.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["LubricantStock_Closing"].Text = "'" + LubricantStock_Closing.ToString("n") + "' ";
            //RD.DataDefinition.FormulaFields["SGO_Closing"].Text = "'" + SGO_Closing.ToString("n") + "' ";


            RD.DataDefinition.FormulaFields["TotalPurchaseReturn"].Text = "'" + TotalPurchaseReturn.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["TotalSalesReturnPlusStockReturn"].Text = "'" + TotalSalesPlusStockReturn.ToString("n") + "' ";

            RD.DataDefinition.FormulaFields["PROther"].Text = "'" + PROther.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["PRSGP"].Text = "'" + PRSGP.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["SRSGP"].Text = "'" + SRSGP.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["SROther"].Text = "'" + SROther.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["TotalSalesReturn"].Text = "'" + TotalSalesReturn.ToString("n") + "' ";

            RD.DataDefinition.FormulaFields["StockReturnSGP"].Text = "'" + StockReturnSGP.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["StockReturnOther"].Text = "'" + StockReturnOther.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["TotalStockReturn"].Text = "'" + TotalStockReturn.ToString("n") + "' ";

            RD.DataDefinition.FormulaFields["ChargeOutSGP"].Text = "'" + ChargeOutSGP.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["ChargeOutOther"].Text = "'" + ChargeOutOther.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["TotalChargeOut"].Text = "'" + TotalChargeOut.ToString("n") + "' ";
            RD.DataDefinition.FormulaFields["TotalJobcard"].Text = "'" + sTemp + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "Report.pdf";
            string File = FilePath + FileName;

            PdfDocument outputDocument = new PdfDocument();
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
            // convert and show
            //Session["RD"] = RD;

            //string FilePath = Server.MapPath("~") + "\\Download\\";
            //string FileName = "StockBalance" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            //string File = FilePath + FileName;

            //string URL = "../../../../Download/rptViewerService.aspx?ReportID=StockBalance";

            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

        }

        public void GetStockBalanceData(string VendorCode, string MMonth, string YYear)
        {
            string sOpeningDate = "", sClosingDate = "", sql_query = "";
            SqlDataReader reader = null;
            try
            {
                sOpeningDate = YYear + "/" + MMonth + "/" + "01";
                sClosingDate = DateTime.Parse(sOpeningDate).AddMonths(1).ToString("yyyy/MM/dd");

                sOpeningDate = sOpeningDate.Replace("/", "-");
                sClosingDate = sClosingDate.Replace("/", "-");

                sql_query = "Execute [sp_StockBalance_New] '" + Session["DealerCode"].ToString() + "', '" + VendorCode + "', '" + sOpeningDate + "', '" + sClosingDate + "' ";
                
                myFunc.ExecuteQuery(sql_query, ref reader);

                while (reader.Read())
                {
                    SGPPurchasesFromPSMC = Convert.ToDouble(reader["SGPPurchasesFromPSMC"]);

                    OtherPurchases = Convert.ToDouble(reader["OtherPurchases"]);
                    LubricantPurchases = Convert.ToDouble(reader["LubricantPurchases"]);
                    SGOPurchases = Convert.ToDouble(reader["SGOPurchases"]);
                    SGPPartsConsumption = Convert.ToDouble(reader["SGPPartsConsumption"]);
                    SGPPartsSales = Convert.ToDouble(reader["SGPPartsSales"]);
                    //SGPPartsSalesAccess = Convert.ToDouble(reader["SGPPartsSalesAccess"]);
                    OtherPartsConsumption = Convert.ToDouble(reader["OtherPartsConsumption"]);
                    OtherPartsSales = Convert.ToDouble(reader["OtherPartsSales"]);
                    LubricantConsumption = Convert.ToDouble(reader["LubricantConsumption"]);
                    LubricantSales = Convert.ToDouble(reader["LubricantSales"]);

                    SGOConsumption = Convert.ToDouble(reader["SGOConsumption"]);
                    SGOConsumptionLiter = Convert.ToDouble(reader["SGOConsumptionLiter"]);
                    SGOSales = Convert.ToDouble(reader["SGOSales"]);
                    SGOSalesLiter = Convert.ToDouble(reader["SGOSalesLiter"]);

                    SGPStock_Opening = Convert.ToDouble(reader["SGPStock_Opening"]);
                    OtherStock_Opening = Convert.ToDouble(reader["OtherStock_Opening"]);
                    LubricantStock_Opening = Convert.ToDouble(reader["LubricantStock_Opening"]);
                    SGO_Opening = Convert.ToDouble(reader["SGO_Opening"]);

                    SGPStock_Closing = Convert.ToDouble(reader["SGPStock_Closing"]);
                    OtherStock_Closing = Convert.ToDouble(reader["OtherStock_Closing"]);
                    LubricantStock_Closing = Convert.ToDouble(reader["LubricantStock_Closing"]);
                    SGO_Closing = Convert.ToDouble(reader["SGO_Closing"]);

                    /*
                    TotalPurchaseReturn = Convert.ToDouble(reader["PurchaseReturnTotal"]);
                    TotalSalesPlusStockReturn = Convert.ToDouble(reader["TotalSalesReturn"]);
                    TotalSalesPlusStockReturn = TotalSalesPlusStockReturn + Convert.ToDouble(reader["TotalStockReturn"]);

                    PRSGP = Convert.ToDouble(reader["PRSGP"]);
                    PROther = Convert.ToDouble(reader["PROther"]);
                    SRSGP = Convert.ToDouble(reader["SRSGP"]);
                    SROther = Convert.ToDouble(reader["SROther"]);
                    TotalSalesReturn = Convert.ToDouble(reader["TotalSalesReturn"]);

                    StockReturnSGP = Convert.ToDouble(reader["StockReturnSGP"]);
                    StockReturnOther = Convert.ToDouble(reader["StockReturnOther"]);
                    TotalStockReturn = Convert.ToDouble(reader["TotalStockReturn"]);

                    ChargeOutSGP = Convert.ToDouble(reader["ChargeOutSGP"]);
                    ChargeOutOther = Convert.ToDouble(reader["ChargeOutOther"]);
                    TotalChargeOut = Convert.ToDouble(reader["TotalChargeOut"]);
                    */
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}