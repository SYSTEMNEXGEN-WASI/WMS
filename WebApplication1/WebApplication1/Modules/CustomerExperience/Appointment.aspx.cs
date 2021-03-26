using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace DXBMS.Modules.CustomerExperience
{
    public partial class Appointment : System.Web.UI.Page
    {
        SysFunction sysFunc = new SysFunction();
        DataSet ds;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!IsPostBack)
            {

                createGrid();
                lOADddl();
                LoadGRN_DDL();
            }

            txtVisitDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void LoadGRN_DDL()
        {
            string WhereQuery = "1=1";
            //    string[] Columns = new string[] { "RegNo", "App_Id" };
            //    sysFunc.GetMultiColumnsDDL(ddlRegNo, Columns, "CustomerVisit", WhereQuery, "RegNo", "", false, false);

            string[] col = new string[] { "ProdTitle" };
            sysFunc.GetMultiColumnsDDL(ddlVehicleModel, col, "Vehicle", WhereQuery, "ProdTitle", "", false, false);
        }

        private void lOADddl()
        {
            ddlPriority.Items.Add(new ListItem("Select", "Select"));
            ddlPriority.Items.Add(new ListItem("High", "High"));
            ddlPriority.Items.Add(new ListItem("Medium", "Medium"));
            ddlPriority.Items.Add(new ListItem("Low", "Low"));

            ddlTimeSlot.Items.Add(new ListItem("Select", "Select"));
            ddlTimeSlot.Items.Add(new ListItem("9am to 11am", "9am to 11am"));
            ddlTimeSlot.Items.Add(new ListItem("11am to 2pm", "11am to 2pm"));
            ddlTimeSlot.Items.Add(new ListItem("2pm to 4pm", "2pm to 4pm"));

            ddlpurpose.Items.Add(new ListItem("Select", "Select"));
            ddlpurpose.Items.Add(new ListItem("Schedule", "Schedule"));
            ddlpurpose.Items.Add(new ListItem("Repair", "Repair"));
            ddlpurpose.Items.Add(new ListItem("Waranty", "Waranty"));

        }

        private void clearData()
        {
            sysFunc.Clearddl(Page);
            sysFunc.ClearTextBoxes(Page);
            createGrid();

        }

        public void addVisitor(object sender, EventArgs e)
        {
            string autoCode = string.Empty;

            try
            {
                //  if(ddlRegNo.SelectedIndex == 0)
                //   {
                //     clearData();
                //   return;
                //}


                SqlParameter[] param =
            {
                new SqlParameter("@DealerCode",SqlDbType.VarChar),//0
                new SqlParameter("@RegNo",SqlDbType.VarChar), //1
                new SqlParameter("@VisitDate",SqlDbType.DateTime),//2
                new SqlParameter("@CustName",SqlDbType.VarChar), //3
                new SqlParameter("@Contact",SqlDbType.VarChar), //4
                new SqlParameter("@TimeSlot",SqlDbType.VarChar), //5
                new SqlParameter("@Purpose",SqlDbType.VarChar), //6
                new SqlParameter("@Priority",SqlDbType.VarChar),//7
                new SqlParameter("@VehicleModel",SqlDbType.VarChar), //8
                new SqlParameter("@Date",SqlDbType.DateTime) //9

            };

                DateTime visitDate = DateTime.ParseExact(txtVisitDate.Text, "dd-MM-yyyy", null);
                DateTime Date = DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", null);

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = txtRegNo.Text;
                param[2].Value = visitDate;
                param[3].Value = txtCustName.Text;
                param[4].Value = txtContactNo.Text;
                param[5].Value = ddlTimeSlot.Text;
                param[6].Value = ddlpurpose.Text;
                param[7].Value = ddlPriority.Text;
                param[8].Value = ddlVehicleModel.Text;
                param[9].Value = Date;

                sysFunc.ExecuteSP_NonQuery("Sp_Insert_CustomerVisit", param);
                clearData();
                LoadGRN_DDL();
                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Green, "Customer visit added Successfully");
            }
            catch (Exception ex)
            {
                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);

                throw;
            }
        }

        protected void clearFields(object sender, EventArgs e)
        {
            clearData();
            gv_visit.DataSource = null;
            gv_visit.DataBind();
        }

        private void createGrid()
        {
            ds = new DataSet();
            ds.Tables.Add();

            ds.Tables[0].Columns.Add(new DataColumn("VisitDate", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("RegNo", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("CustName", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Contact", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("TimeSlot", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("purpose", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Priority", typeof(string)));

            //DataRow dr = ds.Tables[0].NewRow();
            //ds.Tables[0].Rows.Add(dr);

            gv_visit.DataSource = ds.Tables[0];
            gv_visit.DataBind();

            Session["VisitDS"] = ds;

        }


        //protected void btnView_Click(object sender, EventArgs e)
        //{
        //    if(txtVisitDate.Text.Trim() == string.Empty)
        //    {
        //        sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, "Visit date is empty");
        //        return;
        //    }

        //    DateTime visitDate = DateTime.ParseExact(txtVisitDate.Text, "dd-MM-yyyy", null);
        //    String sql = "select VisitDate,RegNo,CustName,Contact,TimeSlot,Purpose,[Priority] from CustomerVisit where VisitDate = '"+ visitDate + "'";

        //    DataTable dt = new DataTable();

        //    dt = sysFunc.GetData(sql);

        //    if(dt.Rows.Count > 0) { 
        //    gv_visit.DataSource = dt;
        //    gv_visit.DataBind();
        //    }
        //    else
        //    {
        //      lblMessage.Text = "No Data Found!";
        //    }

        //}

        protected void lnkRegNo_Click(object sender, EventArgs e)
        {
            LinkButton btnlnk = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)btnlnk.Parent.Parent;

            LinkButton lnk = (LinkButton)gvr.FindControl("lnkRegNo");

            if (lnk.Text == "")
            {
                clearData();
                return;
            }
            try
            {
                DataTable dt = new DataTable();

                string sql = "select * from CustomerVisit where RegNo = '" + lnk.Text + "'";
                dt = sysFunc.GetData(sql);

                DateTime visitDate = (DateTime)dt.Rows[0]["VisitDate"];

                txtRegNo.Text = dt.Rows[0]["RegNo"].ToString();
                txtVisitDate.Text = visitDate.ToString("dd-MM-yyyy");
                txtCustName.Text = dt.Rows[0]["CustName"].ToString();
                txtContactNo.Text = dt.Rows[0]["Contact"].ToString();
                ddlTimeSlot.SelectedValue = dt.Rows[0]["TimeSlot"].ToString();
                ddlpurpose.SelectedValue = dt.Rows[0]["Purpose"].ToString();
                ddlPriority.SelectedValue = dt.Rows[0]["Priority"].ToString();

            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;

            }

        }


        //protected void btnPrint_Click(object sender, EventArgs e)
        //{
        //    dsReports data = new dsReports();
        //    ReportDocument RD = new ReportDocument();

        //    DateTime visitDate = DateTime.ParseExact(txtVisitDate.Text, "dd-MM-yyyy", null);

        //    SqlParameter[] param =
        //    {
        //        new SqlParameter("@DealerCode",SqlDbType.Char),
        //        new SqlParameter("@VisitDate",SqlDbType.VarChar)

        //    };

        //    param[0].Value = Session["DealerCode"].ToString();
        //    param[1].Value = visitDate;

        //    SqlDataReader rder = null;

        //    SysFunction sysFunc = new SysFunction();
        //    if (sysFunc.ExecuteSP("SP_VisitorsList", param, ref rder))
        //    {
        //        data.SP_VisitorsList.Load(rder);

        //    }

        //    RD.Load(Server.MapPath("\\Reports\\VisitorsReport.rpt"));

        //    RD.Database.Tables[0].SetDataSource(data);

        //    // convert and show
        //    string FilePath = Server.MapPath("~") + "\\Download\\";
        //    string FileName = "VisitorsReport" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
        //    string File = FilePath + FileName;

        //    RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

        //    string URL;
        //    URL = "../../Download/OpenPDF.aspx?FileName=" + FileName;
        //    string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
        //    ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

        //}

        //protected void Test(object sender, EventArgs e)
        //{

        //    dsReports data = new dsReports();
        //    ReportDocument RD = new ReportDocument();

        //    SqlParameter[] param =
        //    {
        //        new SqlParameter("@DealerCode",SqlDbType.Char),
        //        new SqlParameter("@PONO",SqlDbType.Char)

        //    };

        //    param[0].Value = Session["DealerCode"].ToString();
        //    param[1].Value = "17000008";

        //    SqlDataReader rder = null;

        //    SysFunction sysFunc = new SysFunction();
        //    if (sysFunc.ExecuteSP("SP_Spare_Get_POView", param, ref rder))
        //    {
        //        data.SP_Spare_Get_POView.Load(rder);

        //    }

        //    RD.Load(Server.MapPath("\\Reports\\PurchaseOrderReport.rpt"));

        //    RD.Database.Tables[0].SetDataSource(data);

        //    // convert and show
        //    string FilePath = Server.MapPath("~") + "\\Download\\";
        //    string FileName = "POReport" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
        //    string File = FilePath + FileName;

        //    RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

        //    string URL;
        //    URL = "../../Download/OpenPDF.aspx?FileName=" + FileName;
        //    string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
        //    ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);


        //}

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {

            lblMessage.Text = Calendar1.SelectedDate.ToString("dd-MM-yyyy");
            DateTime date = DateTime.ParseExact(lblMessage.Text, "dd-MM-yyyy", null);

            String sql = "select App_Id,convert(Varchar(10),VisitDate,105) as VisitDate,RegNo,VehicleModel,CustName,Contact,TimeSlot,Purpose,[Priority] from CustomerVisit where VisitDate = '" + date + "'";

            DataTable dt = new DataTable();

            dt = sysFunc.GetData(sql);

            if (dt.Rows.Count > 0)
            {
                gv_visit.DataSource = dt;
                gv_visit.DataBind();
            }
            else
            {
                lblMessage.Text = "No Data Found!";
            }
        }
    }
}