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
    public partial class JobCardDetailReport : System.Web.UI.Page
    {
        SysFunctions myFunc = new SysFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }

            DateTime mdtTo = DateTime.Today;
            DateTime mdtFrom = mdtTo.AddDays(-365);

            dtTo.Date = dtTo.Date;
            dtFrom.Date = dtFrom.Date;
        }

        protected void dtFrom_DateChanged(object sender, EventArgs e)
        {
            lblMSG.Text = "";
        }

        protected void dtTo_DateChanged(object sender, EventArgs e)
        {
            lblMSG.Text = "";
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
            //DataSet dsRpt = new DataSet();
            DXBMS.Data.DataSet1 objDsReports = new Data.DataSet1();



            SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                                   new SqlParameter("@FromDate",SqlDbType.DateTime),//0
                                   new SqlParameter("@ToDate",SqlDbType.DateTime),//0
                                   new SqlParameter("@Status",SqlDbType.VarChar,10)//0
                               };
            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = FromDate.ToString("yyyy-MM-dd");
            param[2].Value = ToDate.ToString("yyyy-MM-dd");
            param[2].Value = rbStatus.SelectedItem.Value.ToString();



            //if (myFunc.ExecuteSPDMIS("sp2W_Spare_InvoiceDetail", param, ref rder))
            //if(myFunc.ExecuteSP("sp2W_Spare_InvoiceDetail",param,ref rder))
            //{
            //    objDsReports.sp2W_Spare_InvoiceDetail.Load(rder);


            //}

            DataTable dt = new DataTable();
            string sql = "exec sp_JobCardDetailReport '" + Session["DealerCode"].ToString() + "','" + FromDate.ToString("yyyy-MM-dd") + "','" + ToDate.ToString("yyyy-MM-dd") + "','" + rbStatus.SelectedItem.Value.ToString() + "'";
            dt = myFunc.GetData(sql);
            RD.PrintOptions.PaperSize = PaperSize.PaperA4;


            //RD.Load(Server.MapPath("../../SpareReports/rptJobCardDateWise.rpt"));
            RD.Load(Server.MapPath("../rptJobCardDateWise.rpt"));
            //RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + myFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Phone1", "Dealer") + "'";
            //RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + myFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Email", "Dealer") + "'";

            //RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + myFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "DealerDesc", "Dealer") + "'";
            //RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + myFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Address1", "Dealer") + "" +
            //                                                               myFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Address2", "Dealer") + "" +
            //                                                               myFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Address3", "Dealer") + " (" +
            //                                                               myFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Phone1", "Dealer") + ") '";
            RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
            RD.DataDefinition.FormulaFields["FromCode"].Text = "'" + FDate + "'";
            RD.DataDefinition.FormulaFields["ToCode"].Text = "'" + TDate + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Jobcard Detail Report" + "\"";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.Database.Tables[0].SetDataSource(objDsReports);
            RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.Database.Tables[0].SetDataSource(dt);

            // convert and show
            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "JobCardDetail" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;

            RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            string URL;
            URL = "../../../../Download/OpenPdf.aspx?FileName=" + FileName;
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
    }
}