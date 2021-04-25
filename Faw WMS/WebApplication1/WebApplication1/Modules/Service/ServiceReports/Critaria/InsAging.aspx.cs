using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.ApplicationBlocks.Data;
using CrystalDecisions.Shared;
using CConn;
using DXBMS.Data;
using System.Drawing.Printing;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;

namespace DXBMS.Modules.Service.ServiceReports.Critaria
{
    public partial class InsAging : System.Web.UI.Page
    {
        static SysFunctions SysFunc = new SysFunctions();
        SecurityBll sec = new SecurityBll();
        clsLookUp clslook = new clsLookUp();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (Page.IsPostBack)
            {

                if (!string.IsNullOrEmpty((string)Session["LookUpData"]))
                {
                    SelectedPartDetail(Session["LookUpData"].ToString());
                }
            }
            if (!IsPostBack)
            {
                txtFromDate.Text = txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            }
            Session["LookUpData"] = string.Empty;
        }

        protected void SelectedPartDetail(string item)
        {

            if(Session["Comp"].ToString() == "Company")
            {
                txtInsCompCode.Text = item;
                txtInsCompDesc.Text=SysFunc.GetStringValuesAgainstCodes("InsCompCode='" + item + "'", "InsuranceCompanies", "InsCompDescription");

            }
            else
            {
                txtInsBrCode.Text = item;
                txtInsBrDesc.Text = SysFunc.GetStringValuesAgainstCodes("BranchCode='" + item + "' and InsCompCode='"+txtInsCompCode.Text+"' ", "Branch", "BranchDesc");
            }
           
        }
        protected void imgLookup_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 93;
            Session["Comp"] = "Company";
            clslook.LU_Get_InsComp(imgLookup, ViewState["lookupid"].ToString(), "", "../../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void imgInsBr_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 94;
            Session["Comp"] = "Branch";
            clslook.LU_Get_InsCompBr(imgLookup, ViewState["lookupid"].ToString(), "", "../../../../",txtInsCompCode.Text);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            ReportDocument RD = new ReportDocument();
            string FDate = SysFunc.SaveDate(txtFromDate.Text).ToString();
            string TDate = SysFunc.SaveDate(txtToDate.Text).ToString();
            DataSet dsRpt = new DataSet();
            DXBMS.Data.DSReports objDsReports = new Data.DSReports();


            DataTable dt = new DataTable();
            string sql;
           
            if (!sec.UserRight("2562", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }
            sql = "exec SP_InsAging_Select '" + Session["DealerCode"].ToString() + "','" + FDate + "','" + TDate + "', '" + txtInsCompCode.Text + "', '" + txtInsBrCode.Text + "'";
            RD.Load(Server.MapPath("../../ServiceReports/rptInsAging.rpt"));
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "INSURANCE AGING DETAIL" + "\"";

            dt = SysFunc.GetData(sql);

            RD.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;

            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";

            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\Images\\havoline.png'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            RD.Database.Tables[0].SetDataSource(dt);
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
            stream = null;

            string URL = "../../../../Download/PrintReport.aspx";

            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";

            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
            Session["RD"] = RD;



            //crviewer.ReportSource = RD;


            //convert and show
            //string FilePath = Server.MapPath("~") + "\\Download\\";
            //string FileName = "JobCardDetail" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            //string File = FilePath + FileName;

            //Session["RD"] = RD;

            //string URL = "../../../../Download/rptViewerService.aspx?ReportID=StockBalance";
            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=10200,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
    }
}