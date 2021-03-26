using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.Shared;

namespace DXBMS.Modules.Reports
{
    public partial class StandardPartStockStatusReport : System.Web.UI.Page
    {
        ReportDocument RD;
        SqlDataReader rder;
        string ReportID = "";
        ReportDocument crReportDocument;
        ReportDocument rpt;
        // DXBMS.App_Code.DAL.DSReports ds = new DXBMS.App_Code.DAL.DSReports();
        DataTable dt;
        string sql = "";
        SysFunctions myFunc = new SysFunctions();
        DataSet ds = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (!chkAllItem.Checked && (glToItemPartNo.Text == "" || glFromItemPartNo.Text==""))
            {
                lblErr.Text = "Please select Item Range";
                return;
            }
            if (radioItemType.SelectedItem == null)
            {
                lblErr.Text = "Please select Item Type";
                return;
            }
            PrintReport("sp_PartsStockSourceWiseNew", "STOCK STATUS WITH AVERAGE REPORT", "rptPartsStockStatusNew");
            //W:\Work\imran suhail\BMS\DXBMS\DXBMS\Modules\Criteria\POView.aspx
            //string URL = "../Criteria/POView.aspx?abc=" +radioItemType.SelectedItem.ToString() + "&ReportID=PartStock";
            //Response.Redirect(URL);
            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }

        protected void PrintReport(string sp_Name,string ReportTitle,string ReportName)
        {
            RD = new ReportDocument();
            ReportID = "PartStock";

            string ReportIDStock = "";
            //ReportIDStock = Request.QueryString["abc"];
            ReportIDStock = radioItemType.SelectedItem.Value.ToString();
            DataSet dsRpt = new DataSet();
            DataSetPartStockReport objDSReports = new DataSetPartStockReport();
            SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
                                   new SqlParameter("@Param",SqlDbType.VarChar,20),//0
                                   new SqlParameter("@@FromPartItemNo",SqlDbType.VarChar,20),
                                   new SqlParameter("@@ToPartItemNo",SqlDbType.VarChar,20)
                               };
            param[0].Value = this.Session["DealerCode"].ToString();
            //param[1].Value = ReportIDStock;
            param[1].Value = ReportIDStock;
            if (!chkAllItem.Checked)
            {
                param[2].Value = glFromItemPartNo.Text;
                param[3].Value = glToItemPartNo.Text;
            }
            else
            {
                param[2].Value = "";
                param[3].Value = "Z";
            }
            //ds = myFunc.ExecuteSP_NonQuery("sp_PartsStockSourceWiseNew", param);

            if (myFunc.ExecuteSP(sp_Name, param, ref rder))
            {
                objDSReports.sp_PartsStockSourceWiseNew.Load(rder);

            }
            else
            {
                lblErr.Text = "No record found!";
                return;
            }

            
            RD.PrintOptions.PaperSize = PaperSize.PaperA4;
            //RD.Load(Server.MapPath("~/Modules/Reports/SpareReports/rptPartsStockStatusNew.rpt"));
            //txtPartItemDesc.Text = Server.MapPath("~/Modules/Reports/SpareReports/rptPartsStockStatusNew.rpt");
            //return;
            RD.Load(Server.MapPath("../" + ReportName + ".rpt"));
            RD.DataDefinition.FormulaFields["UserID"].Text = "'" + this.Session["UserName"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + GlobalVar.mUserIPAddress + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'" + Session["DealerNTN"].ToString() + "'";
            RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'" + Session["DealerSaleTaxNo"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["FromDate"].Text = "'" + Request.QueryString["FrmDate"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["ToCode"].Text = "'" + Request.QueryString["ToDate"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + ReportTitle + "\"";
            RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"] .ToString()+ "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            RD.Database.Tables[0].SetDataSource(objDSReports);
            //RD.Database.Tables[0].SetDataSource(dt);
            //CrystalReportViewer1.ReportSource = RD;

            //CrystalReportViewer1.ReportSource = RD;
            ////CrystalReportViewer1.ShowFirstPage();
            //var connectionInfo = new ConnectionInfo();
            //connectionInfo.ServerName = "AZHARDELL";
            //connectionInfo.DatabaseName = "BMS";
            //connectionInfo.Password = "100372";
            //connectionInfo.UserID = "sa";
            //connectionInfo.Type = ConnectionInfoType.SQL;
            //connectionInfo.IntegratedSecurity = false;

            //for (int i = 0; i < CrystalReportViewer1.LogOnInfo.Count; i++)
            //{
            //    CrystalReportViewer1.LogOnInfo[i].ConnectionInfo = connectionInfo;
            //}

            Session["RDSpare"] = RD;
            //RD.Database.Tables[0].SetDataSource(objDsReports);


            crReportDocument = new ReportDocument();
            crReportDocument = (ReportDocument)Session["RDSpare"];
            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = Request.QueryString["ReportID"] + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;
            //crReportDocument.SetDatabaseLogon("SDMS", "sdms161", "192.168.1.47", "SDMS");
            crReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            string URL;
            URL = "../../../../Download/OpenPdf.aspx?FileName=" + FileName;
            //URL = FilePath + "OpenPdf.aspx?FileName=" + FileName;
            //txtPartItemDesc.Text = URL;
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }

        protected void glFromItemPartNo_TextChanged(object sender, EventArgs e)
        {
            lblErr.Text = "";
            if (glFromItemPartNo.Text=="")
            {
                txtPartItemDescFrom.Text = "";
                return;
            }
            txtPartItemDescFrom.Text= myFunc.GetItemDescByItemCode(myFunc.GetItemCodeByPartItemNo(glFromItemPartNo.Text));
        }

        protected void glToItemPartNo_TextChanged(object sender, EventArgs e)
        {
            lblErr.Text = "";
            if (glToItemPartNo.Text == "")
            {
                txtPartItemDescTo.Text = "";
                return;
            }
            txtPartItemDescTo.Text = myFunc.GetItemDescByItemCode(myFunc.GetItemCodeByPartItemNo(glToItemPartNo.Text));
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtPartItemDescTo.Text = "";
            txtPartItemDescFrom.Text = "";
            glToItemPartNo.Text = "";
            glFromItemPartNo.Text = "";
           // radioItemType.SelectedItem.Text = "All";
            chkAllItem.Checked = true;
            glFromItemPartNo.ClientEnabled = false;
            glToItemPartNo.ClientEnabled = false;
            lblErr.Text = "";
        }

        protected void chkAllItem_CheckedChanged(object sender, EventArgs e)
        {
            lblErr.Text = "";
            if (chkAllItem.Checked)
            {
                glFromItemPartNo.ClientEnabled = false;
                glToItemPartNo.ClientEnabled = false;

            }
            else
            {
                glFromItemPartNo.ClientEnabled = true;
                glToItemPartNo.ClientEnabled = true;
                txtPartItemDescTo.Text = "";
                txtPartItemDescFrom.Text = "";
                glToItemPartNo.Text = "";
                glFromItemPartNo.Text = "";
            }
        }

        protected void btnPrintLocRpt_Click(object sender, EventArgs e)
        {
            PrintReport("sp_PartsStockLocationSourceWise", "STOCK STATUS WITH PARTS LOCATION REPORT", "rptPartsStockLocationWise");
        }
    }
}