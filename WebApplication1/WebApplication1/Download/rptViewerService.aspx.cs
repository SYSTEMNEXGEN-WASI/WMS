using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Web;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
using CrystalDecisions.Shared;
using DXBMS.Data;
using CConn;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using PdfSharp.Pdf;
using System.IO;

namespace DXBMS.Modules.Service.ServiceReports
{
    public partial class rptViewerService : System.Web.UI.Page
    {
        ReportDocument rpt, crReportDocument;
        string strCriteria, rptTitle;
        DateTime FromDate, ToDate;
        SqlDataReader rder;
        SysFunctions myFunc = new SysFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/Main/login.aspx");
            }
            rpt = new ReportDocument();
            String StrReportID,Type;
            Type = Request.QueryString["Type"];
            StrReportID = Request.QueryString["ReportID"];
            if (StrReportID == "PaymentReceiptReport")
            {
                rpt.Load(Server.MapPath("rptPaymentReceiptPrint.rpt"));

                strCriteria = "{PaymentReceiptMaster.ReceiptNo}='" + Request.QueryString["ReceiptNo"] + "' " +
                               "AND {PaymentReceiptMaster.DealerCode}='" + Session["DealerCode"].ToString() + "' " +
                               "AND {PaymentReceiptMaster.DelFlag} = 'N' ";

                //rpt.RecordSelectionFormula = strCriteria;

                rpt.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                rpt.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";

                rpt.DataDefinition.FormulaFields["ReportTitle"].Text = "'Payment Receipt'";
                rpt.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
                rpt.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
                rpt.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'" + Session["DealerSaleTaxNo"].ToString() + "'";
                rpt.DataDefinition.FormulaFields["NTN"].Text = "'" + Session["DealerNTN"].ToString() + "'";
                //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + grl.GetStringValuesAgainstCodes("CusCode", SessionInformation.LoginID, "CellNo", "Customer") + "'";


                // CrystalReportViewer1.ReportSource = rpt;
            }
            if (StrReportID == "CustomerEstimate")
            {
                //ReportDocument RD;
                //string strCriteria, rptTitle;
                //DateTime FromDate, ToDate;
                //SqlDataReader rder;
                //CustEstimateCode

                SysFunctions myFunc = new SysFunctions();
                Data.DSReports DsRpt = new Data.DSReports();
                //DataSet1 dsRpt1 = new DataSet1();
                SqlDataReader rder = null;
                rpt = new ReportDocument();
                string CCon = CConnection.GetConnectionString();
                DataSet ds = new DataSet();
                SqlParameter[] param =
    {
            new SqlParameter("@DealerCode",SqlDbType.Char),//0
            new SqlParameter("@CustomerEstimateCode",SqlDbType.Char), //1
            
        };
                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = Request.QueryString["CustEstimateCode"].ToString();
                DsRpt.EnforceConstraints = false;
                ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_CustomerEstimateMaster_Print '" + this.Session["DealerCode"].ToString() + "','" + Request.QueryString["CustEstimateCode"].ToString() + "'");
                DsRpt.sp_CustomerEstimateMaster_Print.Load(ds.CreateDataReader());
                DsRpt.EnforceConstraints = false;
                ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_CustomerEstimateDetail_Print '" + this.Session["DealerCode"].ToString() + "','" + Request.QueryString["CustEstimateCode"].ToString() + "'");
                DsRpt.sp_CustomerEstimateDetail_Print.Load(ds.CreateDataReader());
                DsRpt.EnforceConstraints = false;
                ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_CustomerEstimateDetailSup_Print '" + this.Session["DealerCode"].ToString() + "','" + Request.QueryString["CustEstimateCode"].ToString() + "'");
                DsRpt.sp_CustomerEstimateDetailSup_Print.Load(ds.CreateDataReader());
                DsRpt.EnforceConstraints = false;
                ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_CustomerEstimateLubricates_Print '" + this.Session["DealerCode"].ToString() + "','" + Request.QueryString["CustEstimateCode"].ToString() + "'");
                DsRpt.sp_CustomerEstimateLubricates_Print.Load(ds.CreateDataReader());
                DsRpt.EnforceConstraints = false;
                ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_CustomerEstimateLubricatesSup_Print '" + this.Session["DealerCode"].ToString() + "','" + Request.QueryString["CustEstimateCode"].ToString() + "'");
                DsRpt.sp_CustomerEstimateLubricatesSup_Print.Load(ds.CreateDataReader());
                DsRpt.EnforceConstraints = false;
                ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_CustomerEstimateParts_Print '" + this.Session["DealerCode"].ToString() + "','" + Request.QueryString["CustEstimateCode"].ToString() + "'");
                DsRpt.sp_CustomerEstimateParts_Print.Load(ds.CreateDataReader());
                DsRpt.EnforceConstraints = false;
                ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_CustomerEstimatePartsSup_Print '" + this.Session["DealerCode"].ToString() + "','" + Request.QueryString["CustEstimateCode"].ToString() + "'");
                DsRpt.sp_CustomerEstimatePartsSup_Print.Load(ds.CreateDataReader());
                DsRpt.EnforceConstraints = false;
                myFunc.ExecuteSP("sp_CustomerEstimateBoutParts_Print", param, ref rder);
                DsRpt.sp_CustomerEstimateBoutParts_Print.Load(rder);
                DsRpt.EnforceConstraints = false;
                myFunc.ExecuteSP("sp_EstimateSubletDetail_Print", param, ref rder);
                DsRpt.sp_JobCardSubletDetail_Print.Load(rder);
             
                rpt.Load(Server.MapPath("~/Modules/Service/ServiceReports/rptCustomerEstimatePrint.rpt"));
             
                rpt.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                rpt.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
                rpt.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
                rpt.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
                if (Type == "Insurance")
                {
                    rpt.DataDefinition.FormulaFields["ReportTitle"].Text = "'Insurance Estimate'";
                }
                else
                {
                    rpt.DataDefinition.FormulaFields["ReportTitle"].Text = "'Customer Estimate'";
                }
               
                rpt.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
                rpt.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
                rpt.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
                rpt.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
                //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
                rpt.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
                rpt.Database.Tables[0].SetDataSource(DsRpt);
                //CrystalReportViewer1.ReportSource = rpt;
                //  rpt.SetDataSource(DsRpt.sp_CustomerEstimateMaster_Print.DataSet);

                Session["RDService"] = rpt;
            }
            else
            {
                rpt = (ReportDocument)Session["RD"];
            }

            //var connectionInfo = new ConnectionInfo();
            //connectionInfo.ServerName = "localhost";
            //connectionInfo.DatabaseName = "BMS";
            //connectionInfo.Password = "100372";
            //connectionInfo.UserID = "sa";
            //connectionInfo.Type = ConnectionInfoType.SQL;
            //connectionInfo.IntegratedSecurity = false;

            //for (int i = 0; i < CrystalReportViewer1.LogOnInfo.Count; i++)
            //{
            //    CrystalReportViewer1.LogOnInfo[i].ConnectionInfo = connectionInfo;
            //}
            //if (Environment.MachineName == "AZHARDELL")
            //{
            //    rpt.SetDatabaseLogon("sa", "100372", "localhost", "BMS");
            //}
            //else
            //{
            //    rpt.SetDatabaseLogon("sa", "tiger", "akramali-pc", "BMS");
            //}
            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "Report.pdf";
            string File = FilePath + FileName;

            PdfDocument outputDocument = new PdfDocument();
            Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
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
            Session["RDService"] = rpt;
            string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"1000px\" height=\"800px\">";
            embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
            embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
            embed += "</object>";
            ltEmbed.Text = string.Format(embed, ResolveUrl("~/Download/Report.pdf"));

            //CrystalReportViewer1.ReportSource = rpt;
            //CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
        }

        protected void btnPrin_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            crReportDocument = new ReportDocument();
            crReportDocument = (ReportDocument)Session["RDService"];
            string FilePath = Server.MapPath("~") + "Download\\";
            string FileName = Request.QueryString["ReportID"] + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;
            //crReportDocument.SetDatabaseLogon("SDMS", "sdms161", "192.168.1.47", "SDMS");
            //crReportDocument.SetDatabaseLogon("sa", "100372", "AZHARDELL", "BMS",true);

            crReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            string URL;
            URL = "../../../../Download/OpenPdf.aspx?FileName=" + FileName;

            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);



            //crReportDocument = new ReportDocument();
            //crReportDocument = (ReportDocument)Session["RDService"];
            //crReportDocument.SetDatabaseLogon("SDMS", "sdms161");
            //string FilePath = Server.MapPath("~") + "\\Download\\";
            //string FileName = Request.QueryString["ReportID"] + SessionInformation.DealerCode + DateTime.Now.ToString("dd-MM-yyyy") + ".pdf";
            //string File = FilePath + FileName;
            //crReportDocument.SetDatabaseLogon("SDMS", "sdms161", "192.168.1.47", "SDMS");
            //crReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            //string URL;
            //URL = "../../../Download/OpenPdf.aspx?FileName=" + FileName;
            ////URL = FilePath + "OpenPdf.aspx?FileName=" + FileName;

            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
        protected void btnNavigation_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            //if (txtPageNo.Text.Trim() == string.Empty)
            //{
            //    CrystalReportViewer1.ShowNthPage(Convert.ToInt32(txtPageNo.Text.Trim()));
            //}
        }
        //protected void btnLast_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    CrystalReportViewer1.ShowLastPage();
        //}
        //protected void btnFirst_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    CrystalReportViewer1.ShowFirstPage();
        //}
        //protected void btnPervious_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    CrystalReportViewer1.ShowPreviousPage();
        //}
        //protected void btnNext_Click1(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    CrystalReportViewer1.ShowNextPage();
        //}
        protected void btnExport_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            rpt = new ReportDocument();
            rpt = (ReportDocument)Session["RDService"];
            rpt.SetDatabaseLogon("SDMS", "sdms161", "192.168.1.47", "SDMS");
            rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, Request.QueryString["ReportID"].ToString());

        }
    }
}