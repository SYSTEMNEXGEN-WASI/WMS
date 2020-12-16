using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
using DXBMS;
using Microsoft.ApplicationBlocks.Data;
using CrystalDecisions.Shared;
using System.Threading;
using System.Drawing;

namespace DXBMS.Modules.Reports
{
    public partial class SalesReturnReport : System.Web.UI.Page
    {
        //spareBLL OBjDLL = new spareBLL();
        SqlDataReader rder;
        ReportDocument rpt;
        ReportDocument crReportDocument;
        string ReportID = "";
        ReportDocument RD = new ReportDocument();
        String RptName = "";
        String URL = "";
       // DSReports CounterSaleDetail;
       // DataSet1 SaleAndReturnDetail;
        SysFunction sysFun = new SysFunction();
        SysFunctions myFunc = new SysFunctions();
        SecurityBll sec = new SecurityBll();
        clsLookUp clslook = new clsLookUp();
        DataSet ds = new DataSet();

        static int btnValue = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2576", "001"))
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
                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");

                ThreadStart childthreat = new ThreadStart(childthreadcall);
                Thread child = new Thread(childthreat);

                child.Start();
                
                Session["LookUpData"] = string.Empty;

                // string where = "DealerCode = '" + Session["DealerCode"].ToString() + "'";

                //string[] col = { "PartItemNo", "ItemDesc", "ItemCode" };

                //myFunc.GetMultiColumnsDDL(ddlFromCode, col, "Item", where, "PartItemNo", "", false, false);
                //myFunc.GetMultiColumnsDDL(ddlToCode, col, "Item", where, "PartItemNo", "", false, false);
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
            if (!sec.UserRight("2576", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (txtFromDate.Text == "" || txtToDate.Text == "")
            {
                lblMessage.Text = "Please select date range";
                return;
            }

            if (!chkSelect.Checked)
            {
                string fitm = txtFromItemCode.Text;
                string titm = txtToItemCode.Text;
                if (fitm == "" || titm == "")
                {
                    lblMessage.Text = "Please select item range";
                    return;
                }
            }
            
            string ReportID = ReportID = "PartsSalesAndReturnDet";
            //string FromDate = dpFromDate.Text.Substring(6, 4) + "/" + dpFromDate.Text.Substring(3, 2) + "/" + dpFromDate.Text.Substring(0, 2);
            //string ToDate = dpToDate.Text.Substring(6, 4) + "/" + dpToDate.Text.Substring(3, 2) + "/" + dpToDate.Text.Substring(0, 2);

            //string FromDate = txtFromDate.Date.ToString("yyyy/MM/dd");
            //string ToDate = txtToDate.Date.ToString("yyyy/MM/dd");

            string FromDate = sysFun.SaveDate( txtFromDate.Text).ToString();
            string ToDate = sysFun.SaveDate( txtToDate.Text).ToString();

            //string FromItem = ddlFromCode.SelectedValue;
            //string ToItem = ddlToCode.SelectedValue;
            string FromItem = txtFromItemCode.Text;
            string ToItem = txtToItemCode.Text;

            string PDFFileName = MakeReport(ReportID, FromDate, ToDate,FromItem,ToItem, Server.MapPath("../"), Server.MapPath("~/Download/"));


            URL = "../../../../Download/rptViewerService.aspx?FileName=" + PDFFileName;
            //URL = FilePath + "OpenPdf.aspx?FileName=" + FileName;
            //txtPartItemDesc.Text = URL;
            string fullsysURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullsysURL, true);
        }

        public string MakeReport(string ReportID, string StrToDateFrom, string StrDateTo, string StrFromItem, string StrToItem, string rptPath = "", string pdfPath = "")
        {
            DataSet ds = new DataSet();
            ReportDocument RD = new ReportDocument();
            if (ReportID == "PartsSalesAndReturnDet")
            {

                //Iniatilized DataSet
                //SaleAndReturnDetail = new DataSet1();
                DataSet SalesAnReturnDetail = new DataSet();

                string cond = "";
                if (chkSelect.Checked)
                {
                    cond = "";
                }
                else
                {
                    
                    cond = ",'" + txtFromItemCode.Text.Trim() + "','" + txtToItemCode.Text.Trim() + "'";
                }
                    
                string exsp = "Sp_2W_PartSaleandReturnDetail_rpt '" + Session["DealerCode"].ToString() + "','" +  StrToDateFrom + "','" + StrDateTo + "'"+cond;

                //Bind Report to DataSet
                ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.Text, exsp);

                ds.Tables[0].TableName = "Sp_2W_PartSaleandReturnDetail_rpt";

                string url = rptPath + "rptPartSaleAndReturnDetail.rpt";
                RD.Load(url);
                RD.SetDataSource(ds);


                //Set the date format of report heading
                RD.DataDefinition.FormulaFields["FromCode"].Text = "\"" + Convert.ToDateTime(StrToDateFrom.ToString()).ToString("dd/MM/yyyy") + "\"";
                RD.DataDefinition.FormulaFields["ToCode"].Text = "\"" + Convert.ToDateTime(StrDateTo.ToString()).ToString("dd/MM/yyyy") + "\"";
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Part Sales And Return Detail Report" + "\"";
                //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
                RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
                //Set Report Heading
                RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
                RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
                //Set Report Bottom
                RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
                RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserName"].ToString() + "'";

            }
            if (ReportID == "CounterSalesDetail")
            {

                //Iniatilized DataSet
                //CounterSaleDetail = new DataSet1();
                // DataSet ds = new DataSet();
                DataSet CounterSaleDetal = new DataSet();

                string cond = "";
                if (chkSelect.Checked)
                {
                    cond = "";
                }
                else
                {

                    cond = ",'" + txtFromItemCode.Text.Trim() + "','" + txtToItemCode.Text.Trim() + "'";
                }

                string exsp = "Sp_2W_CounterSaleDetail_rpt '" + Session["DealerCode"].ToString() + "','" + StrToDateFrom + "','" + StrDateTo + "'" + cond;



                //Bind Report to DataSet
                ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.Text, exsp);
                //CounterSaleDetail.Sp_2W_CounterSaleDetail_rpt.Load(ds.CreateDataReader());

                string url = rptPath + "rptCounterSaleDetail.rpt";
                RD.Load(url);
                RD.SetDataSource(ds.Tables[0]);

                //Set the date format of report heading
                RD.DataDefinition.FormulaFields["FromCode"].Text = "\"" + Convert.ToDateTime(StrToDateFrom.ToString()).ToString("dd/MM/yyyy") + "\"";
                RD.DataDefinition.FormulaFields["ToCode"].Text = "\"" + Convert.ToDateTime(StrDateTo.ToString()).ToString("dd/MM/yyyy") + "\"";
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Counter Sales Detail Report" + "\"";


                //Set Report Heading
                RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
                //Set Report Bottom
                RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
                RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserName"].ToString() + "'";



            }
            RD.SetDataSource(ds);
            Session["RD"] = RD;
            // create pdf 
            string FilePath = pdfPath;
            string FileName = "Report.pdf";
            string File = FilePath + FileName;
            //crReportDocument.SetDatabaseLogon("SDMS", "sdms161", "192.168.1.47", "SDMS");
            //RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            //URL = "../Download/OpenPdf.aspx?FileName=" + FileName;


            return FileName;
        }

        //protected void btnPopOk_Click(object sender, EventArgs e)
        //{
        //    PopupControlMSG.ShowOnPageLoad = false;
        //}

        //protected void btnPopCancel_Click(object sender, EventArgs e)
        //{
        //    PopupControlMSG.ShowOnPageLoad = false;
        //}

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

        protected void chkAllItem_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelect.Checked)
            {
                //ddlFromCode.SelectedValue = "";
                //ddlToCode.SelectedValue = "";
                //ddlFromCode.Enabled = false;
                //ddlToCode.Enabled = false;
                imgLookup.Enabled = false;
                imgLookup2.Enabled = false;
            }
            else
            {
                //ddlFromCode.Enabled =   true;
                //ddlToCode.Enabled =     true;
                txtFromItemCode.Text = "";
                txtToItemCode.Text = "";
                imgLookup.Enabled = true;
                imgLookup2.Enabled = true;
            }
        }

        protected void SelectedPartDetail(string item)
        {
            try
            {

                //DataTable dt = new DataTable();
                //dt = myFunc.GetData("select ItemDesc,SaleRate,PartItemNo from Item where DealerCode='" + Session["DealerCode"].ToString() + "' And  ItemCode='" + item + "'");

                if (btnValue == 1)
                {
                    txtFromItemCode.Text = item;
                    //txtPartItemDescFrom.Text = dt.Rows[0]["ItemDesc"].ToString().Trim();
                }
                else
                {
                    txtToItemCode.Text = item;
                    //txtPartItemDescTo.Text = dt.Rows[0]["ItemDesc"].ToString().Trim();
                }


            }
            catch (Exception ex)
            {
                myFunc.UserMsg(lblMessage, Color.Red, ex.Message);
            }
        }

        //protected void glFromItem_TextChanged(object sender, EventArgs e)
        //{
        //    lblMessage.Text = "Selected Item "+ddlFromCode.SelectedValue;
        //    return;
        //}
    }
}