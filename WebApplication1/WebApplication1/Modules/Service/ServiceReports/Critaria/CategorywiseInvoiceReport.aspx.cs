using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;
using CConn;
using Microsoft.ApplicationBlocks.Data;
using System.Threading;
using System.IO;
using PdfSharp.Pdf;

namespace DXBMS.Modules.Service.ServiceReports.Critaria
{
    public partial class CategorywiseInvoiceReport : System.Web.UI.Page
    {
        SysFunctions myFunc = new SysFunctions();
        SysFunction sysfun = new SysFunction();
        SecurityBll sec = new SecurityBll();
        MainBLL objMBLL = new MainBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!IsPostBack)
            {
                dtTo.Text = dtFrom.Text = DateTime.Now.ToString("dd-MM-yyyy");
                loadJobTypeCategory();
                SqlParameter[] JobCardMaster_param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5)};
                JobCardMaster_param[0].Value = Session["DealerCode"].ToString();

                objMBLL.FillDrp_SP(ddlAdvisorCode, "sp_Get_Advisor", "EmpCode", "EmpName", JobCardMaster_param, true, "Select", false, "");
            }
        
        }
        public void loadJobTypeCategory()
        {
            objMBLL.FillDropDown(ddlJobCardTypeCategory, "Select  Distinct(JobNatureCatCode),JobNatureCatDesc from JobTypeCategory where DealerCode IN('" + Session["DealerCode"].ToString() + "', 'COMON') And Active='Y' ", "JobNatureCatDesc", "JobNatureCatCode", "Select");
        }
       
        protected void ddlJobCardTypeCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            objMBLL.FillDropDown(ddlJobCardTypeCode, "SELECT J.JobTypeCode,JobTypeDesc FROM JobTypeMaster J inner join JobTypeCategory" +
               " JTC on  J.JobTypeCode=JTC.JobTypeCode and JTC.DealerCode=J.DealerCode  "
               + " Where JTC.DealerCode IN('" + Session["DealerCode"].ToString() + "', 'COMON') and JTC.JobNatureCatCode='" + ddlJobCardTypeCategory.SelectedValue + "' and J.Active='Y' "
               + "order by SortKey Desc ", "JobTypeDesc", "JobTypeCode", "Select");

        }

        protected void ddlJobCardTypeCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSubCategory.Items.Clear();
            if (ddlJobCardTypeCode.SelectedValue == "006")
            {

                // txtLabor.Enabled = false;
                ddlSubCategory.Items.Clear();

                ddlSubCategory.Items.Add(new ListItem("Express", "Express"));
                ddlSubCategory.Items.Add(new ListItem("Standard", "Standard"));
            }
        }

        protected void btnShowReport_Click(object sender, EventArgs e)
        {
            //if (!sec.UserRight("2564", "006"))
            //{
            //    Response.Redirect("~/Test.aspx");
            //}
            if (dtFrom.Text == "" || dtTo.Text == "")
            {
                lblMSG.Text = "Please select dates";
                return;
            }
            SqlDataReader rder;
            ReportDocument RD = new ReportDocument();
            DateTime FromDate;
            DateTime ToDate;

            string FDate = myFunc.SaveDate(dtFrom.Text).ToString();
            string TDate = myFunc.SaveDate(dtTo.Text).ToString();

            FromDate = Convert.ToDateTime(FDate);
            ToDate = Convert.ToDateTime(TDate);

            //ReportDocument RD = new ReportDocument();
            //DataSet dsRpt = new DataSet();
            DXBMS.Data.DataSet1 objDsReports = new Data.DataSet1();



            SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                                   new SqlParameter("@FromDate",SqlDbType.DateTime),//0
                                   new SqlParameter("@ToDate",SqlDbType.DateTime),//0
                                   new SqlParameter("@CusCode",SqlDbType.Char,8),//0
                                    new SqlParameter("@EmpCode",SqlDbType.Char,06),//0
                                    new SqlParameter("@JobCardType",SqlDbType.Char,03),//0
                                    new SqlParameter("@JobCatNatureCode",SqlDbType.Char,03),//0
                                    new SqlParameter("@SubCategory",SqlDbType.VarChar,50)//0
                               };
            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = FromDate.ToString("yyyy-MM-dd");
            param[2].Value = ToDate.ToString("yyyy-MM-dd");
            param[3].Value = "";
            param[4].Value = ddlAdvisorCode.SelectedValue;
            param[5].Value = ddlJobCardTypeCode.SelectedValue;
            param[6].Value = ddlJobCardTypeCategory.SelectedValue;
            param[7].Value = ddlSubCategory.SelectedValue;

            string FroDate = sysfun.SaveDate(dtFrom.Text).ToString();
            string TooDate = sysfun.SaveDate(dtTo.Text).ToString();
            DataTable dt = new DataTable();
            string sql = "exec SP_vw_AfterSaleInvoiceDetail_New '" + Session["DealerCode"].ToString() + "','" + FroDate + "','" + TooDate + "','" + "" + "','" + ddlAdvisorCode.SelectedValue + "', '"+ddlJobCardTypeCode.SelectedValue+"' ,'"+ddlJobCardTypeCategory.SelectedValue+"' ,'"+ddlSubCategory.SelectedValue+"' ";
            dt = myFunc.GetData(sql);
            RD.Load(Server.MapPath("../../ServiceReports/rptAfterSaleInvoice.rpt"));
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "AFTER SALE INVOICE DETAIL" + "\"";

            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";

            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
          
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.Database.Tables[0].SetDataSource(objDsReports);
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

            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1500,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";

            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
    }
}