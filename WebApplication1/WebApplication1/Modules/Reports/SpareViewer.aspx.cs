using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
//using CConn;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Drawing;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.Configuration;
using DXBMS.Data;
using CConn;
namespace DXBMS.Modules.Reports
{
    
    public partial class SpareViewer : System.Web.UI.Page
    {
        ReportDocument RD;
        SqlDataReader rder;
        string ReportID = "";
        DataSet1 dsrpt;
        SysFunctions ObjSysFunc=new SysFunctions ();
       
        
        //string CCon = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            CrystalReportViewer1.DisplayGroupTree = false;
            RD = new ReportDocument();
            ReportID = Request.QueryString["ReportID"];
            if (ReportID == "PartItemLedger")
            {
                string StrToDateFrom = "", StrDateTo = "";
                RD = new ReportDocument();
                dsrpt = new DataSet1();
                // PartItemLedger.sp_ItemStockLedger_select.Load(ds.CreateDataReader());
                DataSet ds = new DataSet();
                //DataSet ds1 = new DataSet();
                StrToDateFrom = Request.QueryString["FromDate"];
                StrDateTo = Request.QueryString["ToDate"];
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
                                   new SqlParameter("@FromPartCode",SqlDbType.VarChar,50),//0
                                   new SqlParameter("@ToPartCode",SqlDbType.VarChar,50),//0
                                   new SqlParameter("@OpeningDate",SqlDbType.VarChar,10),//0
                                   new SqlParameter("@ToDate",SqlDbType.VarChar,10),//0
                               };
                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = Request.QueryString["FromCode"].ToString();
                param[2].Value = Request.QueryString["ToCode"].ToString();
                param[3].Value = Convert.ToDateTime(StrToDateFrom).ToString("yyyy-MM-dd");
                param[4].Value = Convert.ToDateTime(StrDateTo).ToString("yyyy-MM-dd");
                ObjSysFunc.ExecuteSP_NonQuery("sp_PartItemLedger", param);
                ds = SqlHelper.ExecuteDataset(CConnection.GetConnectionString(), CommandType.Text, "sp_tempItemStockLedger_Select '" + Session["DealerCode"].ToString() + "'");
                //dsrpt.sp_tempItemStockLedger_Select.Load(ds.CreateDataReader());

                RD.Load(base.Server.MapPath("rptItemStockLedger.rpt"));
                //RD.Load(base.Server.MapPath("rptLedger.rpt"));
                RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
                RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserName"].ToString() + "'";
                RD.DataDefinition.FormulaFields["FromCode"].Text = "\"" + Convert.ToDateTime(Request.QueryString["FromDate"].ToString()).ToString("dd/MM/yyyy") + "\"";
                RD.DataDefinition.FormulaFields["ToCode"].Text = "\"" + Convert.ToDateTime(Request.QueryString["ToDate"].ToString()).ToString("dd/MM/yyyy") + "\"";
                RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
                RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
                RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "ITEM STOCK LEDGER REPORT" + "\"";
                RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
              //  RD.SetDataSource(dsrpt.sp_tempItemStockLedger_Select.DataSet);
            }
            CrystalReportViewer1.ReportSource = RD;
        }
    }
}