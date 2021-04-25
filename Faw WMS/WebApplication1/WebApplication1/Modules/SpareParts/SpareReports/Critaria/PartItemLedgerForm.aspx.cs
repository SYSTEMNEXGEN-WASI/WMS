using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
using System.Data;
using CConn;
using CrystalDecisions.Shared;
using Microsoft.ApplicationBlocks.Data;
using System.Threading;
using System.Drawing;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;

namespace DXBMS.Modules.Reports
{
    public partial class PartItemLedgerForm : System.Web.UI.Page
    {
        static int btnValue = 0;
        SysFunctions myFunc = new SysFunctions();
        SecurityBll sec = new SecurityBll();
        DataSet ds = new DataSet();
        clsLookUp clslook = new clsLookUp();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2574", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }

            if (Page.IsPostBack)
            {

                if (!string.IsNullOrEmpty(Session["LookUpData"].ToString()))
                {
                    SelectedPartDetail(Session["LookUpData"].ToString());
                }
            }

            if (!IsPostBack)
            {
                //MainBLL ObjMainBLL = new MainBLL();
                //ObjMainBLL.FillDrp_SP(ddlFromCode, "sp_2W_Select_Item", "PartItemNo", "ItemDesc", null, true, "--Select--", false, "");

                //string where = "DealerCode = '" + Session["DealerCode"].ToString() + "'";

                //string[] col = { "PartItemNo", "ItemDesc", "ItemCode" };

                ThreadStart childthreat = new ThreadStart(childthreadcall);
                Thread child = new Thread(childthreat);

                child.Start();

                Session["LookUpData"] = string.Empty;

                //myFunc.GetMultiColumnsDDL(ddlFromCode, col, "Item", where, "PartItemNo", "", false, false);
                //myFunc.GetMultiColumnsDDL(ddlToCode, col, "Item", where, "PartItemNo", "", false, false);
                //ddlFromCode.DataSource = ObjMainBLL.FillDataSet("sp_2W_Select_Item", null).Tables[0];
                //ddlFromCode.DataValueField = "PartItemNo";
                //ddlFromCode.DataTextField = "ItemDesc";
                //ddlFromCode.DataBind();
                //ObjMainBLL.FillDrp_SP(ddlToCode, "sp_2W_Select_Item", "PartItemNo", "ItemDesc", null, true, "--Select--", false, "");
                //ObjMainBLL.ExecuteQuery("Select ItemCode,PartItemNo,ItemDesc From Item ", ref ds);

                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            }
        }

        public void childthreadcall()
        {
            try
            {

                string sQuery = "SP_SelectParts '" + Session["DealerCode"].ToString() + "', 'P'";

                if (myFunc.ExecuteQuery(sQuery, ref ds))
                {
                    Session["Parts"] = ds;

                }

            }
            catch (ThreadAbortException e)
            {

                //lblmessage.Text += "<br /> child thread - exception";

            }
            finally
            {
                //lblmessage.Text += "<br /> child thread - unable to catch the  exception";
            }
        }

        protected void btnLookup_Click(object sender, EventArgs e)
        {
            ViewState["lookupid"] = 10; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_Parts(imgLookup, ViewState["lookupid"].ToString(), "", "../../../../");

            btnValue = 1;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }

        protected void imgLookup2_Click(object sender, EventArgs e)
        {
            ViewState["lookupid"] = 10; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_Parts(imgLookup, ViewState["lookupid"].ToString(), "", "../../../../");

            btnValue = 2;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }

        protected void SelectedPartDetail(string item)
        {
            try
            {

                DataTable dt = new DataTable();
                dt = myFunc.GetData("select ItemDesc,PartItemNo from Item where DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') And  ItemCode='" + item + "'");

                if (btnValue == 1)
                {
                    txtFromItemCode.Text = item;
                    txtFromItemDesc.Text = dt.Rows[0]["ItemDesc"].ToString().Trim();
                    txtToItemCode.Text = item;
                    txtToItemDesc.Text = dt.Rows[0]["ItemDesc"].ToString().Trim();
                }
                else if (btnValue == 2)
                {
                    txtToItemCode.Text = item;
                    txtToItemDesc.Text = dt.Rows[0]["ItemDesc"].ToString().Trim();
                }


            }
            catch (Exception ex)
            {
                myFunc.UserMsg(lblMsg, Color.Red, ex.Message);
            }
        }

        //protected void ddlFromCode_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ddlToCode.SelectedValue = ddlFromCode.SelectedValue;
        //}

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2574", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (txtFromItemCode.Text == "")
            {
                //ObjGenral.UserMsg(lblMsg, Color.Red, "Please Select From Code ");
                //ddlFromCode.Focus();
                return;
            }
            if (txtToItemCode.Text == "")
            {
                //ObjGenral.UserMsg(lblMsg, Color.Red, "Please Select To Code ");
                //ddlToCode.Focus();
                return;
            }

            ReportDocument RD;
            ReportDocument crReportDocument;
            ReportDocument rpt;
            SqlDataReader rder;
            string ReportID = "";
            DXBMS.Data.DataSet1 dsrpt;
            SysFunctions ObjSysFunc = new SysFunctions();


            string FromDate = txtFromDate.Text.Substring(6, 4) + "/" + txtFromDate.Text.Substring(3, 2) + "/" + txtFromDate.Text.Substring(0, 2);
            string ToDate = txtToDate.Text.Substring(6, 4) + "/" + txtToDate.Text.Substring(3, 2) + "/" + txtToDate.Text.Substring(0, 2);
            string RptName = "PartItemLedger";


            RD = new ReportDocument();
           
            string StrToDateFrom = "", StrDateTo = "";
            RD = new ReportDocument();
            dsrpt = new DXBMS.Data.DataSet1();
            // PartItemLedger.sp_ItemStockLedger_select.Load(ds.CreateDataReader());
            DataSet ds = new DataSet();
            //DataSet ds1 = new DataSet();
            StrToDateFrom = FromDate;
            StrDateTo = ToDate;
            SqlParameter[] param = {
                                new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
                                new SqlParameter("@FromPartCode",SqlDbType.VarChar,50),//0
                                new SqlParameter("@ToPartCode",SqlDbType.VarChar,50),//0
                                new SqlParameter("@OpeningDate",SqlDbType.VarChar,10),//0
                                new SqlParameter("@ToDate",SqlDbType.VarChar,10),//0
                            };
            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = txtFromItemCode.Text;
            param[2].Value = txtToItemCode.Text;
            param[3].Value = Convert.ToDateTime(StrToDateFrom).ToString("yyyy-MM-dd");
            param[4].Value = Convert.ToDateTime(StrDateTo).ToString("yyyy-MM-dd");
            ObjSysFunc.ExecuteSP_NonQuery("sp_PartItemLedger", param);
            ds = SqlHelper.ExecuteDataset(CConnection.GetConnectionString(), CommandType.Text, "sp_tempItemStockLedger_Select '" + Session["DealerCode"].ToString() + "'");
            dsrpt.sp_tempItemStockLedger_Select.Load(ds.CreateDataReader());

            //RD.Load(base.Server.MapPath("rptItemStockLedger.rpt"));
            RD.Load(Server.MapPath("~/Modules/SpareParts/SpareReports/rptItemStockLedger.rpt"));

            //RD.Load(Server.MapPath("~/Modules/Reports/rptStockLedgerNew.rpt"));

           // //RD.Load(base.Server.MapPath("rptLedger.rpt"));
            RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserName"].ToString() + "'";
            RD.DataDefinition.FormulaFields["FromCode"].Text = "\"" + Convert.ToDateTime(FromDate).ToString("dd/MM/yyyy") + "\"";
            RD.DataDefinition.FormulaFields["ToCode"].Text = "\"" + Convert.ToDateTime(ToDate).ToString("dd/MM/yyyy") + "\"";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "ITEM STOCK LEDGER REPORT" + "\"";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.SetDataSource(dsrpt.sp_tempItemStockLedger_Select.DataSet);
            RD.SetDataSource(dsrpt);

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
            //string FileName = "PartItemLedger" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            //string File = FilePath + FileName;
            ////crReportDocument.SetDatabaseLogon("SDMS", "sdms161", "192.168.1.47", "SDMS");

            //// ConnectionInfo conninfo = new ConnectionInfo();
            //Session["RD"] = RD;
            ////RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);
            ////crReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            //string URL;
            //URL = "../../../../Download/rptViewerService.aspx?FileName=" + FileName;


            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";

            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtFromItemCode.Text = string.Empty;
            txtFromItemDesc.Text = string.Empty;
            txtToItemCode.Text = string.Empty;
            txtToItemDesc.Text = string.Empty;
        }

    }
}