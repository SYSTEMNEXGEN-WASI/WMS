using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CConn;
using Microsoft.ApplicationBlocks.Data;
using CrystalDecisions.Shared;
using DXBMS.Data;
using System.Threading;
using System.Windows.Forms;

using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;


namespace DXBMS.Modules.Service.ServiceReports.Critaria
{
    public partial class DTRDetail : System.Web.UI.Page
    {
        SysFunction sysfunc = new SysFunction();
        SysFunctions sysfuncs = new SysFunctions();
        SecurityBll sec = new SecurityBll();
        SysFunction myFunc = new SysFunction();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2584", "001"))
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
            if (!sec.UserRight("2584", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }

            if (dtFrom.Text == "" || dtTo.Text == "")
            {
                MessageBox.Show ("Please select dates","Message");
                return;
            }
            Data.DSReports data = new Data.DSReports();
            SqlDataReader rder = null;
            DataTable dt = new DataTable();
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
                                    new SqlParameter("@DealerCode",SqlDbType.Char),//
            new SqlParameter("@FromDate",SqlDbType.DateTime), //1
            new SqlParameter("@ToDate",SqlDbType.DateTime), //2
             new SqlParameter("@VehicleCategory",SqlDbType.VarChar,03),
            new SqlParameter("@ChassisNo",SqlDbType.VarChar), //2
            new SqlParameter("@EngineNo",SqlDbType.VarChar),
             new SqlParameter("@RegNo",SqlDbType.VarChar),
               new SqlParameter("@RptType",SqlDbType.VarChar),
                               };
            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = sysfunc.SaveDate(dtFrom.Text);
            param[2].Value = sysfunc.SaveDate(dtTo.Text);
            param[3].Value = rbStatus.SelectedItem.Value.ToString();
            param[4].Value = "";
            param[5].Value = "";
            param[6].Value = "";
            param[7].Value = "DTR";



            if (sysfunc.ExecuteSP("SP_FIRDetail_WMS", param, ref rder))
            {

                data.sp_GetFIRMaster.Load(rder);
                dt = data.sp_GetFIRMaster;
            }
           

                RD.Load(Server.MapPath("~/Modules/Service/ServiceReports/DTRDetail.rpt"));
          


           // RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
          //  RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Dealer Technical Report'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            //RD.DataDefinition.FormulaFields["Pic1"].Text = "'" + Server.MapPath("~") + dt.Rows[0]["item"].ToString() + "'";
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
            stream.Dispose();stream.Close();
            string URL = "../../../../Download/PrintReport.aspx";

            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";

            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

            // convert and show
         


            ////// convert and show
            ////string FilePath = Server.MapPath("~") + "\\Download\\";
            ////string FileName = "BillingReport" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            ////string File = FilePath + FileName;

            ////Session["RD"] = RD;

            ////string URL;
            ////URL = "../../../../Download/rptViewerService.aspx?FileName=" + FileName;
            ////string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000%,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ////ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);




        }
    }
}