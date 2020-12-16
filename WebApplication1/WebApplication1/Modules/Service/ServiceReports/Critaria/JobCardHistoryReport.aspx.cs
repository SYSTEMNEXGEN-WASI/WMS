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

using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;

namespace DXBMS.Modules.Reports
{
    public partial class JobCardHistoryReport : System.Web.UI.Page
    {
        ReportDocument rpt;
        //General grl;
        SysFunctions myFunc = new SysFunctions();
        SecurityBll sec = new SecurityBll();
        string strCriteria;
        MainBLL objMBLL = new MainBLL();

        clsLookUp clslook = new clsLookUp();
      
        protected void Page_Load(object sender, EventArgs e)
        {


            if (this.Session["DealerCode"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2565", "001"))
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
                DataSet ds = new DataSet();
                SqlParameter[] JobCardMaster_param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5)};
                JobCardMaster_param[0].Value = Session["DealerCode"].ToString();
                objMBLL.FillDrp_SP(ddlRegNo, "sp_2W_LoadChassisEngineRegNo_History", "RegNo", "RegNo", JobCardMaster_param, true, "--Select--", false, "");
                objMBLL.FillDrp_SP(ddlEngineNo, "sp_2W_LoadChassisEngineRegNo_History", "EngineNo", "EngineNo", JobCardMaster_param, true, "--Select--", false, "");
                objMBLL.FillDrp_SP(ddlChassisNo, "sp_2W_LoadChassisEngineRegNo_History", "ChassisNo", "ChassisNo", JobCardMaster_param, true, "--Select--", false, "");
            }

            Session["LookUpData"] = null;
            Session["LookUpData"] = "";
        }

        protected void ddlRegNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string RegNo = ddlRegNo.SelectedValue.ToString();
            SqlParameter[] RegNo_param = {          
                new SqlParameter("@DealerCode",SqlDbType.Char,5),                      
           /*0*/ new SqlParameter("@RegNo",SqlDbType.VarChar,100)};
            RegNo_param[0].Value = Session["DealerCode"].ToString();
            RegNo_param[1].Value = RegNo;
            ds = myFunc.FillDataSet("sp_2W_LoadChassisEngineRegNo_RegNoParameter_History", RegNo_param);
            ddlEngineNo.SelectedValue = ds.Tables[0].Rows[0]["EngineNo"].ToString();
            ddlChassisNo.SelectedValue = ds.Tables[0].Rows[0]["ChassisNo"].ToString();
            txtCusCode.Text = ds.Tables[0].Rows[0]["CusCode"].ToString();
            txtCustomer.Text = ds.Tables[0].Rows[0]["CusDesc"].ToString().ToUpper();
            txtProduct.Text = ds.Tables[0].Rows[0]["ProdCode"].ToString();
            txtVersionCode.Text = ds.Tables[0].Rows[0]["VersionCode"].ToString();
            txtVerDesc.Text = ds.Tables[0].Rows[0]["ProdTitle"].ToString();

            lblMSG.Text = "";

        }
        protected void ddlEngineNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string EngineNo = ddlEngineNo.SelectedValue.ToString();
            SqlParameter[] EngineNo_param = {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),                                           
           /*0*/ new SqlParameter("@EngineNo",SqlDbType.VarChar,100)};
            EngineNo_param[0].Value = Session["DealerCode"].ToString();
            EngineNo_param[1].Value = EngineNo;
            ds = myFunc.FillDataSet("sp_2W_LoadChassisEngineRegNo_EngNoParameter_History", EngineNo_param);
            ddlRegNo.SelectedValue = ds.Tables[0].Rows[0]["RegNo"].ToString();
            ddlChassisNo.SelectedValue = ds.Tables[0].Rows[0]["ChassisNo"].ToString();
            txtCusCode.Text = ds.Tables[0].Rows[0]["CusCode"].ToString();
            txtCustomer.Text = ds.Tables[0].Rows[0]["CusDesc"].ToString().ToUpper();
            txtProduct.Text = ds.Tables[0].Rows[0]["ProdCode"].ToString();
            txtVersionCode.Text = ds.Tables[0].Rows[0]["VersionCode"].ToString();
            txtVerDesc.Text = ds.Tables[0].Rows[0]["ProdTitle"].ToString();

            lblMSG.Text = "";

        }
        protected void ddlChassisNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string ChassisNo = ddlChassisNo.SelectedValue.ToString();
            SqlParameter[] ChassisNo_param = {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),                                      
           /*0*/ new SqlParameter("@ChassisNo",SqlDbType.VarChar,100)};
            ChassisNo_param[0].Value = Session["DealerCode"].ToString();
            ChassisNo_param[0].Value = ChassisNo;
            ds = myFunc.FillDataSet("sp_2W_LoadChassisEngineChassisNo_Parameter_History", ChassisNo_param);
            ddlEngineNo.SelectedValue = ds.Tables[0].Rows[0]["EngineNo"].ToString();
            ddlRegNo.SelectedValue = ds.Tables[0].Rows[0]["RegNo"].ToString();
            txtCusCode.Text = ds.Tables[0].Rows[0]["CusCode"].ToString();
            txtCustomer.Text = ds.Tables[0].Rows[0]["CusDesc"].ToString().ToUpper();
            txtProduct.Text = ds.Tables[0].Rows[0]["ProdCode"].ToString();
            txtVersionCode.Text = ds.Tables[0].Rows[0]["VersionCode"].ToString();
            txtVerDesc.Text = ds.Tables[0].Rows[0]["ProdTitle"].ToString();

            lblMSG.Text = "";
        }

        protected void BtnHistory_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2565", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (ddlRegNo.SelectedIndex == 0 || ddlEngineNo.SelectedIndex == 0 || ddlChassisNo.SelectedIndex == 0)
            {
                lblMSG.Text = "Please select Engine Number, Chassis Number and Registration Number";
                return;
            }


            ReportDocument RD = new ReportDocument();
            DataSet dsRpt = new DataSet();
            DXBMS.Data.DataSet1 objDsReports = new Data.DataSet1();


            SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                   new SqlParameter("@EngNo",SqlDbType.VarChar,30),//1
                                   new SqlParameter("@ChasNo",SqlDbType.VarChar,30),//2
                                   new SqlParameter("@RegNo",SqlDbType.VarChar,15),//3
                               };
            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = ddlEngineNo.Text;
            param[2].Value = ddlChassisNo.Text;
            param[3].Value = ddlRegNo.Text;



            //if (myFunc.ExecuteSPDMIS("sp2W_Spare_InvoiceDetail", param, ref rder))
            //if(myFunc.ExecuteSP("sp2W_Spare_InvoiceDetail",param,ref rder))
            //{
            //    objDsReports.sp2W_Spare_InvoiceDetail.Load(rder);


            //}

            DataTable dt = new DataTable();
            string sql = "exec sp_JobcardHistory '" + Session["DealerCode"].ToString() + "','" + ddlEngineNo.Text +"','" + ddlChassisNo.Text + "','" + ddlRegNo.Text + "'";
            dt = myFunc.GetData(sql);
            RD.PrintOptions.PaperSize = PaperSize.PaperA4;


            //RD.Load(Server.MapPath("../../SpareReports/rptInvoices.rpt"));
            RD.Load(Server.MapPath("../rptJobHistory.rpt"));
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";

            RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'" + Session["DealerNTN"].ToString() + "'";
            RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'" + Session["DealerSaleTaxNo"].ToString() + " '";
            RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["FromCode"].Text = "'" + FDate + "'";
            //RD.DataDefinition.FormulaFields["ToCode"].Text = "'" + TDate + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Jobcard History" + "\"";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            //RD.Database.Tables[0].SetDataSource(objDsReports);
            RD.Database.Tables[0].SetDataSource(dt);
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
            //string FileName = "JobcardHistory" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            //string File = FilePath + FileName;



            //Session["RD"] = RD;

            //string URL;
            //URL = "../../../../Download/rptViewerService.aspx?FileName=" + FileName;
            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1200,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);





            //string URL = "../../Service/ServiceReports/rptViewerService.aspx?ReportID=History&EngineNo=" + ddlEngineNo.SelectedItem.Text;
            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }

        protected void imgLookup_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 3; 
            string sql = "Select RegNo [Reg No], ChassisNo [Chassis No], EngineNo [Engine No], C.CusDesc [Customer] from CustomerVehicle A inner join Customer C on C.CusCode = A.CusCode and C.DealerCode = A.DealerCode";
            clslook.LU_Get_RecVeh(imgLookup, ViewState["lookupid"].ToString(), "", sql, "../../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }


        protected void SelectedPartDetail(string item)
        {

            DataSet ds = new DataSet();
            string RegNo = ddlRegNo.SelectedValue.ToString();
            SqlParameter[] RegNo_param = {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),                      
           /*0*/ new SqlParameter("@RegNo",SqlDbType.VarChar,100)};
            RegNo_param[0].Value = Session["DealerCode"].ToString();
            RegNo_param[1].Value = item;
            ds = myFunc.FillDataSet("sp_2W_LoadChassisEngineRegNo_RegNoParameter_History", RegNo_param);
            ddlRegNo.SelectedValue = ds.Tables[0].Rows[0]["RegNo"].ToString();
            ddlEngineNo.SelectedValue = ds.Tables[0].Rows[0]["EngineNo"].ToString();
            ddlChassisNo.SelectedValue = ds.Tables[0].Rows[0]["ChassisNo"].ToString();
            txtCusCode.Text = ds.Tables[0].Rows[0]["CusCode"].ToString();
            txtCustomer.Text = ds.Tables[0].Rows[0]["CusDesc"].ToString().ToUpper();
            txtProduct.Text = ds.Tables[0].Rows[0]["ProdCode"].ToString();
            txtVersionCode.Text = ds.Tables[0].Rows[0]["VersionCode"].ToString();
            txtVerDesc.Text = ds.Tables[0].Rows[0]["ProdTitle"].ToString();
            lblMSG.Text = "";
        }
    }
}