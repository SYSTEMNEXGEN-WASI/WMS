using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DXBMS.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Drawing;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;


namespace DXBMS.Modules.SpareParts.SpareReports.Critaria
{
    public partial class PurchaseRegisterReport : System.Web.UI.Page
    {
        SysFunction sysFunc = new SysFunction();
        SysFunctions sysFuncs = new SysFunctions();
        Data.DSReports data = new Data.DSReports();
        ReportDocument RD = new ReportDocument();
        SecurityBll sec = new SecurityBll();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2577", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (!IsPostBack)
            {
                lOADddl();

            }

        }


        private void lOADddl()
        {
            ddlVendor.Items.Clear();
            ddlVendor.Items.Add(new ListItem("Select", "Select"));
            ddlVendor.Items.Add(new ListItem("All", "All"));
            ddlVendor.Items.Add(new ListItem("Balance Amount > 0", "Balance Amount > 0"));

            txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }


        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2577", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }
            try
            {
                if (txtFromDate.Text == "")
                {
                    sysFuncs.UserMsg(lblMessage, Color.Red, "Please Select From Date");
                    return;
                }
                else if (txtToDate.Text == "")
                {
                    sysFuncs.UserMsg(lblMessage, Color.Red, "Please Select To Date");
                    return;
                }

                if (ddlVendor.SelectedIndex == 0)
                {
                    sysFuncs.UserMsg(lblMessage, Color.Red, "Please Select Vendor Option");
                    return;
                }



                SqlParameter[] param =
            {
            new SqlParameter("@DealerCode",SqlDbType.Char),//0
            new SqlParameter("@FromDate",SqlDbType.VarChar), //1
            new SqlParameter("@ToDate",SqlDbType.VarChar) //2

        };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = sysFunc.SaveDate(txtFromDate.Text);
                param[2].Value = sysFunc.SaveDate(txtToDate.Text);

                SqlDataReader rder = null;

                //if (ddlUtilityAudit.SelectedValue == "JobCard")
                //{
                //    if (sysFunc.ExecuteSP("SP_JobCardReport_pSelect", param, ref rder)) //Database SP
                //    {
                //        data.SP_JobCardMaster.Load(rder);                       // DataSet SP

                //    }
                //}



                //Store Procedure check and load
                if (sysFunc.ExecuteSP("SP_PurchaseRegisterReport_pSelect", param, ref rder)) //Database SP
                {
                    data.SP_PurchaseRegisterReport_pSelect.Load(rder);                       // DataSet SP

                }
                // Load in crystal report
                //RD.Load(Server.MapPath("../PurchaseSaleReturnReport.rpt"));
                RD.Load(Server.MapPath("../crptPurchaseRegisterReport2.rpt"));

                // mention paper size
                RD.PrintOptions.PaperSize = PaperSize.PaperA4;


                //RD.Load(Server.MapPath("../../SpareReports/rptInvoices.rpt"));
                // RD.Load(Server.MapPath("../rptJobHistory.rpt"));
                //RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + sysFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Phone1", "Dealer") + "'";
                // RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + sysFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Email", "Dealer") + "'";
                RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
                RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'" + Session["DealerSaleTaxNo"].ToString() + "'";
                RD.DataDefinition.FormulaFields["NTN"].Text = "'" + Session["DealerNTN"].ToString() + "'";
                RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["FromCode"].Text = "'" + FDate + "'";
                //RD.DataDefinition.FormulaFields["ToCode"].Text = "'" + TDate + "'";
                RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Purchase Register Report" + "\"";
                RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
                //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
                RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";

                RD.Database.Tables[0].SetDataSource(data);
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


                //// convert and show
                //string FilePath = Server.MapPath("~") + "\\Download\\";
                //string FileName = "PurchaseRegisterReport" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                //string File = FilePath + FileName;

                //Session["RD"] = RD;


                //string URL;
                //URL = "../../../../Download/rptViewerService.aspx?FileName=" + FileName;
                //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1200,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
                //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

            }
            catch (Exception ex)
            {

                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);
            }
        }
    }
}