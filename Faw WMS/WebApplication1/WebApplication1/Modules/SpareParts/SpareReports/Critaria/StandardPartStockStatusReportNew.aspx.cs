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
using System.Drawing;
using System.Threading;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;

namespace DXBMS.Modules.SpareParts.SpareReports.Critaria
{
    public partial class StandardPartStockStatusReportNew : System.Web.UI.Page
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
        SecurityBll sec = new SecurityBll();
        DataSet ds = new DataSet();
        clsLookUp clslook = new clsLookUp();

        static int btnValue = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2578", "001"))
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
                ThreadStart childthreat = new ThreadStart(childthreadcall);
                Thread child = new Thread(childthreat);

                child.Start();

                //string where = "DealerCode = '" + Session["DealerCode"].ToString() + "'";

                //string[] col = { "PartItemNo", "ItemDesc", "ItemCode" };

                //myFunc.GetMultiColumnsDDL(ddlFromCode, col, "Item", where, "PartItemNo", "", false, false);
                //myFunc.GetMultiColumnsDDL(ddlToCode, col, "Item", where, "PartItemNo", "", false, false);
                Session["LookUpData"] = string.Empty;
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

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2578", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (!chkAllItem.Checked && (txtFromItemcode.Text == "" || txtToItemcode.Text == ""))
            {
                lblErr.Text = "Please select Item Range";
                return;
            }
            if (radioItemType.SelectedItem == null)
            {
                lblErr.Text = "Please select Item Type";
                return;
            }
            PrintReport("sp_PartsStockSourceWiseNew", "STOCK STATUS REPORT", "rptPartsStockStatusNew");
            
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

        protected void PrintReport(string sp_Name, string ReportTitle, string ReportName)
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
                                   new SqlParameter("@FromPartItemNo",SqlDbType.VarChar,20),
                                   new SqlParameter("@ToPartItemNo",SqlDbType.VarChar,20)
                               };
            param[0].Value = this.Session["DealerCode"].ToString();
            //param[1].Value = ReportIDStock;
            param[1].Value = ReportIDStock;
            if (!chkAllItem.Checked)
            {
                //param[2].Value = ddlFromCode.SelectedValue;
                //param[3].Value = ddlToCode.SelectedValue;

                param[2].Value = txtFromItemcode.Text;
                param[3].Value = txtToItemcode.Text;
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
            rder.Close();

            RD.PrintOptions.PaperSize = PaperSize.PaperA4;
            
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
            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            RD.Database.Tables[0].SetDataSource(objDSReports);
           

            Session["RD"] = RD;
            //RD.Database.Tables[0].SetDataSource(objDsReports);
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

            //crReportDocument = new ReportDocument();
            //crReportDocument = (ReportDocument)Session["RDSpare"];
            //string FilePath = Server.MapPath("~") + "\\Download\\";
            //string FileName = Request.QueryString["ReportID"] + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            //string File = FilePath + FileName;
            ////crReportDocument.SetDatabaseLogon("SDMS", "sdms161", "192.168.1.47", "SDMS");
            ////crReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            //string URL;
            //URL = "../../../../Download/rptViewerService.aspx?FileName=" + FileName;
            ////URL = FilePath + "OpenPdf.aspx?FileName=" + FileName;
            ////txtPartItemDesc.Text = URL;
            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }

        //protected void glFromItemPartNo_TextChanged(object sender, EventArgs e)
        //{
        //    lblErr.Text = "";
        //    if (ddlFromCode.SelectedValue == "")
        //    {
        //        txtPartItemDescFrom.Text = "";
        //        return;
        //    }
        //    txtPartItemDescFrom.Text = myFunc.GetItemDescByItemCode(myFunc.GetItemCodeByPartItemNo(ddlFromCode.SelectedValue));
        //}

        //protected void glToItemPartNo_TextChanged(object sender, EventArgs e)
        //{
        //    lblErr.Text = "";
        //    if (ddlToCode.SelectedValue == "")
        //    {
        //        txtPartItemDescTo.Text = "";
        //        return;
        //    }
        //    txtPartItemDescTo.Text = myFunc.GetItemDescByItemCode(myFunc.GetItemCodeByPartItemNo(ddlToCode.SelectedValue));
        //}

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtPartItemDescTo.Text = "";
            txtPartItemDescFrom.Text = "";
            txtFromItemcode.Text = "";
            txtToItemcode.Text = "";
            //ddlFromCode.SelectedIndex = 0;
            //ddlToCode.SelectedIndex = 0;
            // radioItemType.SelectedItem.Text = "All";
            chkAllItem.Checked = true;
            imgLookup.Enabled = false;
            imgLookup2.Enabled = false;
            //ddlFromCode.Enabled = false;
            //ddlToCode.Enabled = false;
            lblErr.Text = "";
        }

        protected void chkAllItem_CheckedChanged(object sender, EventArgs e)
        {
            lblErr.Text = "";
            if (chkAllItem.Checked)
            {
                //ddlFromCode.Enabled = false;
                //ddlToCode.Enabled = false;
                imgLookup.Enabled = false;
                imgLookup2.Enabled = false;

            }
            else
            {
                //ddlFromCode.Enabled = true;
                //ddlToCode.Enabled = true;
                txtPartItemDescTo.Text = "";
                txtPartItemDescFrom.Text = "";
                //ddlFromCode.SelectedIndex = 0;
                //ddlToCode.SelectedIndex = 0;
                imgLookup.Enabled = true;
                imgLookup2.Enabled = true;
            }
        }


        protected void SelectedPartDetail(string item)
        {
            try
            {

                DataTable dt = new DataTable();
                dt = myFunc.GetData("select ItemDesc,SaleRate,PartItemNo from Item where DealerCode in ('" + Session["DealerCode"].ToString() + "','COMON') And  ItemCode='" + item + "'");

                if(btnValue == 1)
                {
                    txtFromItemcode.Text = item;
                    txtPartItemDescFrom.Text = dt.Rows[0]["ItemDesc"].ToString().Trim();
                }else
                {
                    txtToItemcode.Text = item;
                    txtPartItemDescTo.Text = dt.Rows[0]["ItemDesc"].ToString().Trim();
                }
                
                
            }
            catch (Exception ex)
            {
                myFunc.UserMsg(lblErr, Color.Red, ex.Message);
            }
        }

        protected void btnPrintLocRpt_Click(object sender, EventArgs e)
        {
            PrintReport("sp_PartsStockLocationSourceWise", "STOCK LOCATION WISE REPORT", "rptPartsStockLocationWise");
        }
    }
}