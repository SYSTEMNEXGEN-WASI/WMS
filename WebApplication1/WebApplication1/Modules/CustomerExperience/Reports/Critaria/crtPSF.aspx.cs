using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DXBMS.Data;

namespace DXBMS.Modules.CustomerExperience.Reports
{
    public partial class crtPSF : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlStatus.Items.Add(new ListItem("All", "All"));
                ddlStatus.Items.Add(new ListItem("N/A", "N/A"));
                ddlStatus.Items.Add(new ListItem("Satisfied", "Yes"));
                ddlStatus.Items.Add(new ListItem("Unsatisfied", "No"));

                txtFromDate.Text = txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }



        protected void Print(object sender, EventArgs e)
        {
            Data.DSReports data = new Data.DSReports();
            ReportDocument RD = new ReportDocument();
            SysFunction sysFunc = new SysFunction();

            DateTime fromDate = DateTime.ParseExact(txtFromDate.Text, "yyyy-MM-dd", null);
            DateTime toDate = DateTime.ParseExact(txtToDate.Text, "yyyy-MM-dd", null);

            //string FDate = sysFunc.SaveDate(txtFromDate.Text).ToString();
            //string TDate = sysFunc.SaveDate(txtToDate.Text).ToString();

            SqlParameter[] param =
        {
            new SqlParameter("@DealerCode",SqlDbType.Char),//0
            new SqlParameter("@FromDate",SqlDbType.VarChar), //1
            new SqlParameter("@ToDate",SqlDbType.VarChar), //2
            new SqlParameter("@Satisfied",SqlDbType.Char)

        };

            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = fromDate;
            param[2].Value = toDate;
            param[3].Value = ddlStatus.SelectedValue.ToString().Trim();

            SqlDataReader rder = null;

            
            if (sysFunc.ExecuteSP("sp_PostServiceFollowup_Rpt", param, ref rder))
            {
                data.sp_PostServiceFollowup_Rpt.Load(rder);
               

            }
            RD.Load(Server.MapPath("../PSFReport.rpt"));


            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserID"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + " POST SERVICE FOLLOWUP SUMMARY" + "\"";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\Images\\havoline.png'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";



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