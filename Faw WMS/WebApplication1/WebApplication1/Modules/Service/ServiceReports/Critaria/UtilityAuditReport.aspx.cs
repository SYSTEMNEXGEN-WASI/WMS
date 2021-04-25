using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DXBMS.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Drawing;
using System.Data.SqlClient;
using CrystalDecisions.Shared;
using System.Data;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;

namespace DXBMS.Modules.Service.ServiceReports.Critaria
{
    public partial class UtilityAuditReport : System.Web.UI.Page
    {

        SysFunction sysFunc = new SysFunction();
        SysFunctions sysFuncs = new SysFunctions();
        Data.DSReports data = new Data.DSReports();
        SecurityBll sec = new SecurityBll();
        ReportDocument RD = new ReportDocument();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2567", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (!IsPostBack)
            {
                lOADddl();
                

            }

        }


        private void lOADddl()
        {
            ddlUtilityAudit.Items.Clear();
            ddlUtilityAudit.Items.Add(new ListItem("Select", "Select"));
            ddlUtilityAudit.Items.Add(new ListItem("Estimate", "Estimate"));
            ddlUtilityAudit.Items.Add(new ListItem("JobCard", "JobCard"));
            ddlUtilityAudit.Items.Add(new ListItem("CounterSale", "CounterSale"));
            ddlUtilityAudit.Items.Add(new ListItem("CustomerInvoice", "CustomerInvoice"));
            ddlUtilityAudit.Items.Add(new ListItem("Incomming Payment", "PayRec"));
            ddlUtilityAudit.Items.Add(new ListItem("GRN", "GRN"));
            ddlUtilityAudit.Items.Add(new ListItem("ChargeOut", "ChargeOut"));


            txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }


        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2567", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }
            try
            {
                if (txtFromDate.Text == "")
                {
                    sysFuncs.UserMsg(lblMessage, Color.Red, "Please Select From Date");
                    return;
                }
                else if (txtToDate.Text == "")
                {
                    sysFuncs.UserMsg(lblMessage, Color.Red, "Please Select To Date");
                    return;
                }

                if (ddlUtilityAudit.SelectedIndex == 0)
                {
                    sysFuncs.UserMsg(lblMessage, Color.Red, "Please Select Utility Audit Option");
                    return;
                }



                SqlParameter[] param =
            {
            new SqlParameter("@DealerCode",SqlDbType.Char),//0
            new SqlParameter("@FromDate",SqlDbType.VarChar), //1
            new SqlParameter("@ToDate",SqlDbType.VarChar) //2

        };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = sysFunc.SaveDate(txtFromDate.Text);
                param[2].Value = sysFunc.SaveDate(txtToDate.Text);

                SqlDataReader rder = null;

                if (ddlUtilityAudit.SelectedValue == "Estimate")
                {
                    if (sysFunc.ExecuteSP("SP_EstimateReport_pSelect", param, ref rder)) //Database SP
                    {
                        data.EnforceConstraints = false;
                        data.SP_JobCardMaster.Load(rder);                       // DataSet SP

                    }
                }
                if (ddlUtilityAudit.SelectedValue == "JobCard")
                {
                    if (sysFunc.ExecuteSP("SP_JobCardReport_pSelect", param, ref rder)) //Database SP
                    {
                        data.EnforceConstraints = false;
                        data.SP_JobCardMaster.Load(rder);                       // DataSet SP

                    }
                }
                else if (ddlUtilityAudit.SelectedValue == "CounterSale")
                {
                    if (sysFunc.ExecuteSP("SP_CounterSaleMaster_pSelect", param, ref rder)) //Database SP
                    {
                        data.SP_JobCardMaster.Load(rder);                       // DataSet SP

                    }
                }
                else if (ddlUtilityAudit.SelectedValue == "CustomerInvoice")
                {
                    if (sysFunc.ExecuteSP("SP_CustomerInvoice_pSelect", param, ref rder)) //Database SP
                    {
                        data.SP_JobCardMaster.Load(rder);                       // DataSet SP

                    }
                }
                else if (ddlUtilityAudit.SelectedValue == "PayRec")
                {
                    if (sysFunc.ExecuteSP("SP_CustomerInvoice_pSelect", param, ref rder)) //Database SP
                    {
                        data.SP_JobCardMaster.Load(rder);                       // DataSet SP

                    }
                }
                else if (ddlUtilityAudit.SelectedValue == "GRN")
                {
                    if (sysFunc.ExecuteSP("SP_GRNMaster_pSelect", param, ref rder)) //Database SP
                    {
                        data.SP_JobCardMaster.Load(rder);                       // DataSet SP

                    }
                }
                else if (ddlUtilityAudit.SelectedValue == "ChargeOut")
                {
                    if (sysFunc.ExecuteSP("SP_ChargeOutMaster_pSelect", param, ref rder)) //Database SP
                    {
                        data.SP_JobCardMaster.Load(rder);                       // DataSet SP

                    }
                }

                RD.Load(Server.MapPath("../crpUtilityAuditReport.rpt"));

                // mention paper size
                RD.PrintOptions.PaperSize = PaperSize.PaperA4;

                RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
                RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'" + Session["DealerSaleTaxNo"].ToString() + "'";
                RD.DataDefinition.FormulaFields["NTN"].Text = "'" + Session["DealerNTN"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["FromCode"].Text = "'" + FDate + "'";
                //RD.DataDefinition.FormulaFields["ToCode"].Text = "'" + TDate + "'";
                RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
                if (ddlUtilityAudit.SelectedValue == "Estimate")
                {
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Customer Estimate Report" + "\"";
                }
                if (ddlUtilityAudit.SelectedValue == "JobCard")
                {
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Job Card Report" + "\"";
                }
                else if (ddlUtilityAudit.SelectedValue == "CounterSale")
                {
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Counter Sale Report" + "\"";
                }
                else if (ddlUtilityAudit.SelectedValue == "CustomerInvoice")
                {
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Customer Invoice Report" + "\"";
                }
                else if (ddlUtilityAudit.SelectedValue == "CustomerInvoice")
                {
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Payment Receipt Report" + "\"";
                }
                else if (ddlUtilityAudit.SelectedValue == "GRN")
                {
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "GRN Report" + "\"";
                }
                else if (ddlUtilityAudit.SelectedValue == "ChargeOut")
                {
                    RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Stock Adjustment Report" + "\"";
                }

                //RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
                // RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";


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
                stream.Dispose(); stream.Close();
                string URL = "../../../../Download/PrintReport.aspx";

                string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";

                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
                //// convert and show
                //string FilePath = Server.MapPath("~") + "\\Download\\";
                //string FileName = "UtilityAuditReport" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                //string File = FilePath + FileName;


                //Session["RD"] = RD;

                //string URL;
                //URL = "../../../../Download/rptViewerService.aspx?ReportID=" + FileName;
                //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
                //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

            }
            catch (Exception ex)
            {

                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);
            }
        }

        protected void ddlUtilityAudit_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}