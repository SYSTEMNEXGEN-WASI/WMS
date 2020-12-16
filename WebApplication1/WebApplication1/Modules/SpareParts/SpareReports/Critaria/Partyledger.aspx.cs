using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DXBMS.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
using System.Data;
using DXBMS;
using CrystalDecisions.Shared;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;
using System.Drawing;

namespace DXBMS.Modules.SpareParts.SpareReports.Critaria
{
    public partial class Partyledger : System.Web.UI.Page
    {
        SysFunctions sysFunc = new SysFunctions();
        SecurityBll sec = new SecurityBll();
        MainBLL ObjMain = new MainBLL();
        clsLookUp clslook = new clsLookUp();
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2575", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (Page.IsPostBack)
            {

                if (!string.IsNullOrEmpty((String)Session["LookUpData"]))
                {
                    SelectedPartDetail(Session["LookUpData"].ToString());
                }
            }
            if (!IsPostBack)
            {
                
                //LoadGRN_DDL();
               
                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            }
            Session["LookUpData"] = null;
        }
        protected void SelectedPartDetail(string item)
        {
            try
            {

                if (ViewState["lookupid"].ToString() == "1")
                {

                    txtCusCode.Text = item;
                    txtCusDesc.Text = sysFunc.GetStringValuesAgainstCodes("CusCode", item, "CusDesc", "Customer", Session["DealerCode"].ToString());
                }
              else if (ViewState["lookupid"].ToString() == "93")
                {
                    txtInsCompCode.Text = item;
                    txtInsCompDesc.Text = sysFunc.GetStringValuesAgainstCodes("InsCompCode='" + item + "'", "InsuranceCompanies", "InsCompDescription");

                }
                else if (ViewState["lookupid"].ToString() == "94")
                {
                    txtInsBrCode.Text = item;
                    txtInsBrDesc.Text = sysFunc.GetStringValuesAgainstCodes("BranchCode='" + item + "' and InsCompCode='" + txtInsCompCode.Text + "' ", "Branch", "BranchDesc");
                }
            }

            catch (Exception ex)
            {
                sysFunc.UserMsg(lblMsg, Color.Red, ex.Message);
            }
        }
        protected void imgCustomerCode_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 1;
            clslook.LU_Get_Customer(imgCustomerCode, ViewState["lookupid"].ToString(), "DealerCode = '" + Session["DealerCode"].ToString() + "'", "../../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }
        protected void imgLookup_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 93;
            Session["Comp"] = "Company";
            clslook.LU_Get_InsComp(imgLookup, ViewState["lookupid"].ToString(), "", "../../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void imgInsBr_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 94;
            Session["Comp"] = "Branch";
            clslook.LU_Get_InsCompBr(imgLookup, ViewState["lookupid"].ToString(), "", "../../../../", txtInsCompCode.Text);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }


        protected void Print(object sender, EventArgs e)
        {
            if (!sec.UserRight("2575", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }
            Data.DSReports data = new Data.DSReports();
            ReportDocument RD = new ReportDocument();
            SqlDataReader rder = null;
            DateTime fromDate = DateTime.ParseExact(txtFromDate.Text, "dd-MM-yyyy", null);
            DateTime toDate = DateTime.ParseExact(txtToDate.Text, "dd-MM-yyyy", null);
            if (ddlType.SelectedValue == "Cus")
            {

                SqlParameter[] param =
            {
            new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
            new SqlParameter("@FromDate",SqlDbType.DateTime), //1
            new SqlParameter("@ToDate",SqlDbType.DateTime), //2
            new SqlParameter("@CusCode",SqlDbType.VarChar)

        };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = sysFunc.SaveDate(txtFromDate.Text);
                param[2].Value = sysFunc.SaveDate(txtToDate.Text);
                if (txtCusCode.Text == "")
                    param[3].Value = "";
                else
                    param[3].Value = txtCusCode.Text;
               
                string sql = "exec SP_PartyLedger_Report '" + Session["DealerCode"].ToString() + "','" + sysFunc.SaveDate(txtFromDate.Text) + "','" + sysFunc.SaveDate(txtToDate.Text) + "','" + param[3].Value + "'";
                dt = sysFunc.GetData(sql);
                RD.Load(Server.MapPath("../partyLedgerReport.rpt"));

            }
            else
            {

                SqlParameter[] param =
            {
            new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
            new SqlParameter("@FromDate",SqlDbType.DateTime), //1
            new SqlParameter("@ToDate",SqlDbType.DateTime), //2
            new SqlParameter("@InsCompCode",SqlDbType.VarChar),
              new SqlParameter("@InsBrCode",SqlDbType.VarChar)

        };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = sysFunc.SaveDate(txtFromDate.Text);
                param[2].Value = sysFunc.SaveDate(txtToDate.Text);
                param[3].Value = txtInsCompCode.Text;
                param[4].Value = txtInsBrCode.Text;

                string sql = "exec SP_PartyLedger_ReportIns '" + Session["DealerCode"].ToString() + "','" + sysFunc.SaveDate(txtFromDate.Text) + "','" + sysFunc.SaveDate(txtToDate.Text) + "','" + param[3].Value + "','"+ param[4].Value + "'";
                dt = sysFunc.GetData(sql);
                RD.Load(Server.MapPath("../partyLedgerReportIns.rpt"));
            }


            //if (sysFunc.ExecuteSP("SP_PartyLedger_Report", param, ref rder))
            //{
            //    data.SP_PartyLedger_Report.Load(rder);

            //}
            //rder.Close();
          
            

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Party Ledger'";
            RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserName"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
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


            //// convert and show
            //string FilePath = Server.MapPath("~") + "\\Download\\";
            //string FileName = "crtPartyLedger" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            //string File = FilePath + FileName;
            //Session["RD"] = RD;
            ////RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            //string URL;
            //URL = "../../../../Download/rptViewerService.aspx?FileName=" + FileName;
            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1200,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);


        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlType.SelectedValue == "Ins")
            {
                lblInsComp.Visible = true;
                lblInsBrh.Visible = true;
                txtInsBrCode.Visible = true;
                txtInsBrDesc.Visible = true;
                txtInsCompCode.Visible = true;
                txtInsCompDesc.Visible = true;
            }
            else
            {
                lblInsComp.Visible = false;
                lblInsBrh.Visible = false;
                txtInsBrCode.Visible = false;
                txtInsBrDesc.Visible = false;
                txtInsCompCode.Visible = false;
                txtInsCompDesc.Visible = false;
            }
        }
    }
}