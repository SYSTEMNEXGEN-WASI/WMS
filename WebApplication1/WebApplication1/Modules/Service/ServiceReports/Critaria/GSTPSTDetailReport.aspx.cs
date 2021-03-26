using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

using System.IO;
using PdfSharp.Pdf;

namespace DXBMS.Modules.Service.ServiceReports.Critaria
{
    public partial class GSTPSTDetailReport : System.Web.UI.Page
    {
        SysFunction myFunc = new SysFunction();
        SecurityBll sec = new SecurityBll();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2585", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (!IsPostBack)
            {
                dtTo.Text = dtFrom.Text = DateTime.Now.ToString("dd-MM-yyyy");
            }

        }
        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2585", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }
            SqlDataReader rder;
            ReportDocument RD = new ReportDocument();
            DataTable dt = new DataTable();
            DateTime FromDate;
            DateTime ToDate;
            if (dtFrom.Text == "" || dtTo.Text == "")
            {
                lblMSG.Text = "Please select dates";
                return;
            }
            string FDate = myFunc.SaveDate(dtFrom.Text).ToString();
            string TDate = myFunc.SaveDate(dtTo.Text).ToString();

            FromDate = Convert.ToDateTime(FDate);
            ToDate = Convert.ToDateTime(TDate);
            DXBMS.Data.DataSet1 objDsReports = new Data.DataSet1();
            if (rbStatus.SelectedItem.Text == "GST")
            {
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                                   new SqlParameter("@FromDate",SqlDbType.DateTime),//0
                                   new SqlParameter("@ToDate",SqlDbType.DateTime),//0
                                  
                               };
                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = FromDate.ToString("yyyy-MM-dd");
                param[2].Value = ToDate.ToString("yyyy-MM-dd");
               string sql = "exec SP__Get_rptGSTDetailReport '" + Session["DealerCode"].ToString() + "','" + FromDate.ToString("yyyy-MM-dd") + "','" + ToDate.ToString("yyyy-MM-dd") + "'";
                dt = myFunc.GetData(sql);
                RD.PrintOptions.PaperSize = PaperSize.PaperA4;
                RD.Load(Server.MapPath("../GSTDetailReport.rpt"));
            }
            else if (rbStatus.SelectedItem.Text == "Combine")
            {
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                                   new SqlParameter("@FromDate",SqlDbType.DateTime),//0
                                   new SqlParameter("@ToDate",SqlDbType.DateTime),//0
                                  
                               };
                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = FromDate.ToString("yyyy-MM-dd");
                param[2].Value = ToDate.ToString("yyyy-MM-dd");
                string sql = "exec SP__Get_rptComDetailReport '" + Session["DealerCode"].ToString() + "','" + FromDate.ToString("yyyy-MM-dd") + "','" + ToDate.ToString("yyyy-MM-dd") + "'";
                dt = myFunc.GetData(sql);
                RD.PrintOptions.PaperSize = PaperSize.PaperA4;
                RD.Load(Server.MapPath("../ConbineTaxDetail.rpt"));

            }
            else
            {

                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                                   new SqlParameter("@FromDate",SqlDbType.DateTime),//0
                                   new SqlParameter("@ToDate",SqlDbType.DateTime),//0
                                  
                               };
                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = FromDate.ToString("yyyy-MM-dd");
                param[2].Value = ToDate.ToString("yyyy-MM-dd");
                
                string sql = "exec SP__Get_rptPSTDetailReport '" + Session["DealerCode"].ToString() + "','" + FromDate.ToString("yyyy-MM-dd") + "','" + ToDate.ToString("yyyy-MM-dd") + "'";
                dt = myFunc.GetData(sql);
                RD.PrintOptions.PaperSize = PaperSize.PaperA4;

                RD.Load(Server.MapPath("../PSTDetailReport.rpt"));
            }
            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString() + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
            RD.DataDefinition.FormulaFields["FromCode"].Text = "'" + FDate + "'";
            RD.DataDefinition.FormulaFields["ToCode"].Text = "'" + TDate + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
            if (rbStatus.SelectedItem.Text == "PST")
            {
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "PST Collection" + "\"";
            }
            else if (rbStatus.SelectedItem.Text == "Combine")
            {

                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Combine Tax Collection" + "\"";
            }

            else
            {
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "GST Collection" + "\"";
            }
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            RD.SetDataSource(dt);
      //      Session["RD"]=RD;
            //// convert and show
            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "Report.pdf";
            string File = FilePath + FileName;


            Session["RD"] = RD;

            string URL;
            URL = "../../../../Download/OpenPdf.aspx?ReportID=" + FileName;
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);




            //string FilePath = Server.MapPath("~") + "\\Download\\";
            //string FileName = "Report.pdf";
            //string File = FilePath + FileName;

            //PdfDocument outputDocument = new PdfDocument();
            //Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            //stream.Seek(0, SeekOrigin.Begin);
            //DirectoryInfo info = new DirectoryInfo(FilePath);
            //if (!info.Exists)
            //{
            //    info.Create();
            //}

            //string path = Path.Combine(FilePath, FileName);
            //using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
            //{
            //    stream.CopyTo(outputFileStream);
            //}
            //stream.Dispose(); stream.Close();
            //string URL = "../../../../Download/PrintReport.aspx";

            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";

            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

        }
    }
}