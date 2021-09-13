using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace DXBMS.Modules.CustomerExperience.Reports.Critaria
{
    public partial class AppoinmentDetailReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
        }

        protected void Print(object sender, EventArgs e)
        {
            Data.DSReports data = new Data.DSReports();
            ReportDocument RD = new ReportDocument();
            SysFunction sysFunc = new SysFunction();

            //DateTime fromDate = DateTime.ParseExact(txtFromDate.Text, "yyyy-MM-dd", null);
         //   DateTime toDate = DateTime.ParseExact(txtToDate.Text, "yyyy-MM-dd", null);

            //string FDate = sysFunc.SaveDate(txtFromDate.Text).ToString();
            //string TDate = sysFunc.SaveDate(txtToDate.Text).ToString();

            SqlParameter[] param =
        {
            new SqlParameter("@DealerCode",SqlDbType.Char),//0
            new SqlParameter("@FromDate",SqlDbType.VarChar), //1
            new SqlParameter("@ToDate",SqlDbType.VarChar), //2
         

        };

            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = sysFunc.SaveDate(txtFromDate.Text);
            param[2].Value = sysFunc.SaveDate(txtToDate.Text);
         

            SqlDataReader rder = null;


            if (sysFunc.ExecuteSP("sp_AppointMentDetailReport", param, ref rder))
            {
                data.sp_AppointMentDetailReport.Load(rder);


            }
            RD.Load(Server.MapPath("../AppointmentDetail.rpt"));


            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserID"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + " Appointment Summary" + "\"";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            RD.Database.Tables[0].SetDataSource(data);

            // convert and show
            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "crtPSF" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;
            Session["RD"] = RD;
            //RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            string URL;
            URL = "../../../../Download/rptViewerService.aspx?FileName=" + FileName;
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1200,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);


        }
    }
}