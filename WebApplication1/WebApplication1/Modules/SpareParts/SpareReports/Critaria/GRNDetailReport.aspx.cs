using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using CrystalDecisions.Shared;

namespace DXBMS.Modules.Reports
{
    public partial class GRNDetailReport : System.Web.UI.Page
    {
        SysFunctions myFunc = new SysFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
        }
        

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            if (dtFrom.Text == "" || dtTo.Text == "")
            {
                lblMSG.Text = "Please select dates";
                return;
            }

            SqlDataReader rder;
            ReportDocument RD = new ReportDocument();
            DateTime FromDate;
            DateTime ToDate;

            string FDate = dtFrom.Date.ToString("yyyy/MM/dd");
            string TDate = dtTo.Date.ToString("yyyy/MM/dd");

            FromDate = Convert.ToDateTime(FDate);
            ToDate = Convert.ToDateTime(TDate);

            //ReportDocument RD = new ReportDocument();
            DataSet dsRpt = new DataSet();
            DXBMS.Data.DataSet1 objDsReports = new Data.DataSet1();


            SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
                                   new SqlParameter("@FromDate",SqlDbType.DateTime),//0
                                   new SqlParameter("@ToDate",SqlDbType.DateTime),//0
                               };
            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = FromDate.ToString("yyyy-MM-dd");
            param[2].Value = ToDate.ToString("yyyy-MM-dd");



            //if (myFunc.ExecuteSPDMIS("sp2W_Spare_InvoiceDetail", param, ref rder))
            //if(myFunc.ExecuteSP("sp2W_Spare_InvoiceDetail",param,ref rder))
            //{
            //    objDsReports.sp2W_Spare_InvoiceDetail.Load(rder);


            //}

            DataTable dt = new DataTable();
            string sql = "exec sp_GRNDetailList '" + Session["DealerCode"].ToString() + "','" + FromDate.ToString("yyyy-MM-dd") + "','" + ToDate.ToString("yyyy-MM-dd") + "'";
            dt = myFunc.GetData(sql);
            RD.PrintOptions.PaperSize = PaperSize.PaperA4;


            //RD.Load(Server.MapPath("../../SpareReports/rptPartsReceivedDetail.rpt"));
            RD.Load(Server.MapPath("../rptPartsReceivedDetail.rpt"));
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'" + Session["DealerNTN"].ToString() + "'";
            RD.DataDefinition.FormulaFields["SaleTaxNo"].Text = "'" + Session["DealerSaleTaxNo"].ToString() + "'";

            RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
                                                                           
            RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
            RD.DataDefinition.FormulaFields["FromCode"].Text = "'" + FDate + "'";
            RD.DataDefinition.FormulaFields["ToCode"].Text = "'" + TDate + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "GRN DETAIL REPORT" + "\"";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.Database.Tables[0].SetDataSource(objDsReports);
            RD.Database.Tables[0].SetDataSource(dt);

            // convert and show
            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "GRNDetial" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;

            RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            string URL;
            URL = "../../../../Download/OpenPdf.aspx?FileName=" + FileName;
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }

        protected void dtFrom_DateChanged(object sender, EventArgs e)
        {
            lblMSG.Text = "";
        }

        protected void dtTo_DateChanged(object sender, EventArgs e)
        {
            lblMSG.Text = "";
        }
    }
}