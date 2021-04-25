using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DXBMS.App_Code.DAL;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

using System.Data;
using System.Data.SqlClient;

namespace DXBMS.Modules.Criteria
{
    public partial class POView : System.Web.UI.Page
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
        DataSet dsRpt = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/Main/login.aspx");

            }
            RD = new ReportDocument();
            ReportID = Request.QueryString["ReportID"];

            if (ReportID == "GSTInvoiceCounterSale")
            {
                SqlParameter[] param = {
                new SqlParameter("@SaleInvNo",SqlDbType.Char,10),
                new SqlParameter("@dealerCode",SqlDbType.Char,10)  
            };
                param[0].Value = Request.QueryString["CounterNo"];
                param[1].Value = Session["DealerCode"].ToString();

                dsRpt = myFunc.SP_Spare_Get_rptGSTInvoiceCounterSale(param);
                RD.Load(Server.MapPath("~/Modules/Reports/SpareReports/rptGSTInvoiceCounterSale.rpt"));
            }

            if (ReportID == "PartStock")
            {
                string ReportIDStock = "";
                ReportIDStock = Request.QueryString["abc"];
                //ReportDocument RD = new ReportDocument();
                DataSet dsRpt = new DataSet();
                //DataSet objDsReports = new DataSet();
                DataSetPartStockReport objDSReports = new DataSetPartStockReport();
               //DSReports objDsReports = new DSReports();
               //DSReports objDsReports = new DSReports();
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
                                   new SqlParameter("@Param",SqlDbType.VarChar,20),//0
                               };
                param[0].Value = this.Session["DealerCode"].ToString();
                //param[1].Value = ReportIDStock;
                param[1].Value = "--Select--";

                //ds = myFunc.ExecuteSP_NonQuery("sp_PartsStockSourceWiseNew", param);

                if (myFunc.ExecuteSP("sp_PartsStockSourceWiseNew", param, ref rder))
                {
                    objDSReports.sp_PartsStockSourceWiseNew.Load(rder);
                }

//                if (myFunc.ExecuteSP_NonQuery("sp_PartsStockSourceWiseNew", param))
//                {
//                    //objDsReports.sp_PartsStockSourceWiseNew.Load(rder);
//                    objDSReports.sp_PartsStockSourceWiseNew.Load(rder);
//                    //rder.Close();
//                    objDSReports = myFunc.FillDataSet("sp_PartsStockSourceWiseNew", param);
//                    objDsReports = myFunc.FillDataSet("sp_PartsStockSourceWiseNew", param);

//                    sql = "exec sp_PartsStockSourceWiseNew '00001','--Select--'";

//                    dt = myFunc.GetData(sql);

//                    int tr = dt.Rows.Count;

                    
////W:\Work\imran suhail\BMS\DXBMS\DXBMS\Modules\Reports\SpareReports\rptPartsStockStatusNew.rpt
////W:\Work\imran suhail\BMS\DXBMS\DXBMS\Modules\Criteria\POView.aspx
//                }
                RD.PrintOptions.PaperSize = PaperSize.PaperA4;
                RD.Load(Server.MapPath("~/Modules/Reports/SpareReports/rptPartsStockStatusNew.rpt"));
                RD.DataDefinition.FormulaFields["UserID"].Text = "'" + this.Session["UserName"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["FromDate"].Text = "'" + Request.QueryString["FrmDate"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["ToCode"].Text = "'" + Request.QueryString["ToDate"].ToString() + "'";
                RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "C.I Standard Report" + "\"";
                RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
                RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
                RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
                RD.Database.Tables[0].SetDataSource(objDSReports);
                //RD.Database.Tables[0].SetDataSource(dt);
                //CrystalReportViewer1.ReportSource = RD;
                //RD.Database.Tables[0].SetDataSource(objDsReports);

            }



            CrystalReportViewer1.ReportSource = RD;
            //CrystalReportViewer1.ShowFirstPage();
            var connectionInfo = new ConnectionInfo();
            connectionInfo.ServerName = "AZHARDELL";
            connectionInfo.DatabaseName = "BMS";
            connectionInfo.Password = "100372";
            connectionInfo.UserID = "sa";
            connectionInfo.Type = ConnectionInfoType.SQL;
            connectionInfo.IntegratedSecurity = false;

            for (int i = 0; i < CrystalReportViewer1.LogOnInfo.Count; i++)
            {
                CrystalReportViewer1.LogOnInfo[i].ConnectionInfo = connectionInfo;
            }

            Session["RDSpare"] = RD;
            
            //crReportDocument = new ReportDocument();
            //crReportDocument = (ReportDocument)Session["RDSpare"];

            //CrystalReportViewer1.ReportSource = crReportDocument;
        }

        protected void btnPrint_Click(object sender, ImageClickEventArgs e)
        {
            crReportDocument = new ReportDocument();
            crReportDocument = (ReportDocument)Session["RDSpare"];
            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = Request.QueryString["ReportID"] + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;
            //crReportDocument.SetDatabaseLogon("SDMS", "sdms161", "192.168.1.47", "SDMS");
            crReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            string URL;
            URL = "../../../../Download/OpenPdf.aspx?FileName=" + FileName;

            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }

        protected void btnExport_Click(object sender, ImageClickEventArgs e)
        {
            rpt = new ReportDocument();
            rpt = (ReportDocument)Session["RDSpare"];
            rpt.SetDatabaseLogon("SDMS", "sdms161", "192.168.1.47", "SDMS");
            rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, Request.QueryString["ReportID"].ToString());
        }

        protected void btnFirst_Click(object sender, ImageClickEventArgs e)
        {
            CrystalReportViewer1.ShowFirstPage();
        }

        protected void btnPervious_Click(object sender, ImageClickEventArgs e)
        {
            CrystalReportViewer1.ShowPreviousPage();
        }

        protected void btnNext_Click1(object sender, ImageClickEventArgs e)
        {
            CrystalReportViewer1.ShowNextPage();
        }

        protected void btnLast_Click(object sender, ImageClickEventArgs e)
        {
            CrystalReportViewer1.ShowLastPage();
        }

        protected void btnNavigation_Click(object sender, ImageClickEventArgs e)
        {
            CrystalReportViewer1.ShowNthPage(Convert.ToInt32(txtPageNo.Text));
        }

        protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
        {
            try
            {
                this.CrystalReportViewer1.Dispose();
                this.CrystalReportViewer1 = null;

                if ((RD != null))
                {
                    RD.Close();
                    RD.Dispose();

                }
                if ((rpt != null))
                {
                    rpt.Close();
                    rpt.Dispose();

                }
                if ((crReportDocument != null))
                {
                    crReportDocument.Close();
                    crReportDocument.Dispose();

                }
                GC.Collect();
            }
            catch (Exception ex)
            {
                //MainClass.Showmessage(ex.Message.ToString, Page.ClientScript);
            }
        }
    }
}