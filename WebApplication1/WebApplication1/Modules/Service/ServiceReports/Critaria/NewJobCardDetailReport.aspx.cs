using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
using System.Data;
using CrystalDecisions.Shared;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;


namespace DXBMS.Modules.Service.ServiceReports.Critaria
{
    public partial class NewJobCardDetailReport : System.Web.UI.Page
    {

        SysFunction myFunc = new SysFunction();
        SecurityBll sec= new SecurityBll();
        MainBLL objMBLL = new MainBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2564", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (!IsPostBack)
            {
                dtTo.Text = dtFrom.Text = DateTime.Now.ToString("dd-MM-yyyy");
                SqlParameter[] JobCardMaster_param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5)};
                JobCardMaster_param[0].Value = Session["DealerCode"].ToString();

                objMBLL.FillDrp_SP(ddlAdvisorCode, "sp_Get_Advisor", "EmpCode", "EmpName", JobCardMaster_param, true, "Select", false, "");
            }
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2564", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (dtFrom.Text == "" || dtTo.Text == "")
            {
                lblMSG.Text = "Please select dates";
                return;
            }
            SqlDataReader rder;
            ReportDocument RD = new ReportDocument();
            DateTime FromDate;
            DateTime ToDate;

            string FDate = myFunc.SaveDate(dtFrom.Text).ToString();
            string TDate = myFunc.SaveDate(dtTo.Text).ToString();

            FromDate = Convert.ToDateTime(FDate);
            ToDate = Convert.ToDateTime(TDate);

            //ReportDocument RD = new ReportDocument();
            //DataSet dsRpt = new DataSet();
            DXBMS.Data.DataSet1 objDsReports = new Data.DataSet1();



            SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                                   new SqlParameter("@FromDate",SqlDbType.DateTime),//0
                                   new SqlParameter("@ToDate",SqlDbType.DateTime),//0
                                   new SqlParameter("@Status",SqlDbType.VarChar,10),//0
                                    new SqlParameter("@EmpCode",SqlDbType.Char,06)//0
                               };
            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = FromDate.ToString("yyyy-MM-dd");
            param[2].Value = ToDate.ToString("yyyy-MM-dd");
            param[3].Value = rbStatus.SelectedItem.Value.ToString();
            param[4].Value = ddlAdvisorCode.SelectedValue;


            DataTable dt = new DataTable();
            string sql = "exec sp_JobCardDetailReport '" + Session["DealerCode"].ToString() + "','" + FromDate.ToString("yyyy-MM-dd") + "','" + ToDate.ToString("yyyy-MM-dd") + "','" + rbStatus.SelectedItem.Value.ToString() + "','" + ddlAdvisorCode.SelectedValue + "'";
            dt = myFunc.GetData(sql);
            RD.Load(Server.MapPath("../rptJobCardDateWise.rpt"));
            RD.OpenSubreport(Server.MapPath("../JobCardSummary.rpt"));
                                               
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
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Jobcard Detail Report" + "\"";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.Database.Tables[0].SetDataSource(objDsReports);
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            RD.SetDataSource(dt);
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


            //string FilePath = Server.MapPath("~") + "\\Download\\";
            //string FileName = "JobCardDetail" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            //string File = FilePath + FileName;

            //Session["RD"] = RD;



            //string URL = "../../../../Download/rptViewerService.aspx?ReportID="+ FileName;
            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1500,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
    }
}