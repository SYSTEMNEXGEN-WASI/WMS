using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
using CrystalDecisions.Shared;
using CConn;

namespace DXBMS.Download
{
    public partial class OpenPdf : System.Web.UI.Page
    {

        ReportDocument rpt, crReportDocument;
        string strCriteria, rptTitle;
        DateTime FromDate, ToDate;
        SqlDataReader rder;
        SysFunctions myFunc = new SysFunctions();
        public string FilePath;
        public string File;
        string user = "";
        string password = "";
        string server = "";
        string database = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (this.Session["UserName"] == null)
            //{
            //    Response.Redirect("~/Main/login.aspx");

            //}
            //FilePath = Server.MapPath("~/Download/");
            //File = FilePath + Request.QueryString["FileName"];
            //string DelFile = Request.QueryString["FileName"];
            //DelFile = DelFile.Substring(0, DelFile.Length - 14);
            //WebClient User = new WebClient();
            //Byte[] FileBuffer = User.DownloadData(File);

            //if (FileBuffer != null)
            //{

            //    Response.ContentType = "application/pdf";

            //    Response.AddHeader("content-length", FileBuffer.Length.ToString());

            //    Response.BinaryWrite(FileBuffer);
            //}
            //DelFile = FilePath + DelFile + DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy") + ".pdf";
            //if (Directory.Exists(FilePath))
            //{
            //    System.IO.File.Delete(DelFile);
            //}

            //if (!IsPostBack)
            //{
            //    FilePath = Server.MapPath("~/Download/");
            //    File = FilePath + Request.QueryString["FileName"];
            //    System.Net.WebClient client = new System.Net.WebClient();
            //    Byte[] buffer = client.DownloadData(File);

            //    if (buffer != null)
            //    {
            //        Response.ContentType = "application/pdf";
            //        Response.AddHeader("content-length", buffer.Length.ToString());
            //        Response.BinaryWrite(buffer);
            //    }
            //}
            //user = CConnection.user();
            //password = CConnection.password();
            //server = CConnection.server();
            //database = CConnection.database();

            ReportDocument RD = (ReportDocument) Session["RD"];

            //FilePath = Server.MapPath("~/Download/");
            //File = Request.QueryString["FileName"];
            //RD.SetDatabaseLogon(user, password, server, database);
            //CrystalReportViewer1.ReportSource = RD;
            
            //CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

            ////ExportOptions CrExportOptions;
            //System.IO.Stream oStream = null;
            //byte[] byteArray = null;
            //oStream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            //byteArray = new byte[oStream.Length];
            //oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));
            CrystalReportViewer1.ReportSource = RD;
        //    Response.ClearContent();
          //  Response.ClearHeaders();
         //   Response.ContentType = "application/pdf";
          //  Response.BinaryWrite(byteArray);
           // Response.Flush();
           // Response.Close();

          //  RD.Close();
          //  RD.Dispose();

            //Response.Buffer = false;
            //Response.ClearContent();
            //Response.ClearHeaders();

            //Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            //stream.Seek(0, SeekOrigin.Begin);


            Session["ReportSource"] = File;

            //try
            //{
            //    ExportOptions CrExportOptions;
            //    DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            //    PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
            //    CrDiskFileDestinationOptions.DiskFileName = "Downloads";
            //    CrExportOptions = RD.ExportOptions;
            //    {
            //        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            //        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            //        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
            //        CrExportOptions.FormatOptions = CrFormatTypeOptions;
            //    }
            //    RD.Export();
            //}
            //catch (Exception ex)
            //{
            //    ex.ToString();
            //}



        }



        protected void btnPrin_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            //crReportDocument = new ReportDocument();
            //crReportDocument = (ReportDocument)Session["RDService"];
            //string FilePath = Server.MapPath("~") + "Download\\";
            //string FileName = Request.QueryString["ReportID"] + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            //string File = FilePath + FileName;
            ////crReportDocument.SetDatabaseLogon("SDMS", "sdms161", "192.168.1.47", "SDMS");
            ////crReportDocument.SetDatabaseLogon("sa", "100372", "AZHARDELL", "BMS",true);

            //crReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            //string URL;
            //URL = "../../../../Download/OpenPdf.aspx?FileName=" + FileName;

            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);



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
        protected void btnLast_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            rpt = (ReportDocument)Session["RD"];
            rpt.SetDatabaseLogon(user, password, server, database);
            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.ShowLastPage();
        }
        protected void btnFirst_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            
            rpt = (ReportDocument)Session["RD"];
            rpt.SetDatabaseLogon(user, password, server, database);
            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.ShowFirstPage();
        }
        protected void btnPervious_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            rpt = (ReportDocument)Session["RD"];
            rpt.SetDatabaseLogon(user, password, server, database);
            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.ShowPreviousPage();
        }
        protected void btnNext_Click1(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            rpt = (ReportDocument)Session["RD"];
            rpt.SetDatabaseLogon(user, password, server, database);
            CrystalReportViewer1.ReportSource = rpt;
            CrystalReportViewer1.ShowNextPage();
        }
        protected void btnExport_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            rpt = new ReportDocument();
            rpt = (ReportDocument)Session["RD"];
            rpt.SetDatabaseLogon(user, password, server, database);
            rpt.ExportToDisk(ExportFormatType.PortableDocFormat, File);

        }
    }
}