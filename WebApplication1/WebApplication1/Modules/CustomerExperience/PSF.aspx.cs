using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CConn;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Drawing;
using System.Text;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

namespace DXBMS.Modules.CustomerExperience
{
    public partial class PSF : System.Web.UI.Page
    {

        //MainBLL ObjMainBLL = new MainBLL();
        Transaction ObjTrans = new Transaction();
        //General ObjGenral = new General();
        //clsLookUp ObjLookUp = new clsLookUp();
        DataTable grdVehDT = new DataTable();
        //MainBLL objMBLL = new MainBLL();
        SqlTransaction Trans;

        SysFunction sysFunc = new SysFunction();
        SysFunctions myFunc = new SysFunctions();

        DataTable grdPSF;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (Page.IsPostBack == false)
            {
                lblMessage.Visible = false;


                Create_Grid();
            }
        }

        private void PopulateGrid()
        {
            SqlParameter[] param = {
                                  new SqlParameter("@DealerCode",SqlDbType.Char),
                                  new SqlParameter("@FromDate",SqlDbType.DateTime),
                                                                    
                               };
            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = Calendar1.SelectedDate.ToString("yyyy-MM-dd");

            DataSet ds = new DataSet();
            ds = myFunc.FillDataSet("sp_2W_Post_PostServiceFollowup", param);
            grdPostServiceFollowup.DataSource = ds;
            grdPostServiceFollowup.DataBind();
            Session["PostServiceFollowup"] = ds;
            lbltotalCustomer.Text = ds.Tables[0].Rows.Count.ToString();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
        protected void ImgHistory_Click(object sender, ImageClickEventArgs e)
        {

        }
        protected void grdPostServiceFollowup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        void RecordStatus()
        {
            DataTable dttemp = new DataTable();
            try
            {
                dttemp = ViewState["PostServiceFollowup"] as DataTable;
                int i = 0, FollowupRequired = 0, FollowupCompleted = 0;
                foreach (DataRow dr in dttemp.Rows)
                {
                    string FollowupReq = dr["FollowupDate"].ToString();
                    string FollowupComp = dr["FollowupDate"].ToString();
                    Label lblJobCard = grdPostServiceFollowup.Rows[i].FindControl("lblJobCard") as Label;

                    if (FollowupReq == "")
                    {
                        lblJobCard.ForeColor = Color.Green;
                        FollowupRequired++;
                    }
                    else
                    {
                        lblJobCard.ForeColor = Color.Black;
                        FollowupCompleted++;
                    }
                    if (dr["Satisfied"].ToString().Trim() == "N/A")
                    {
                        lblJobCard.ForeColor = Color.Red;
                    }
                    i++;
                }
                lblFollowupRequired.Text = FollowupRequired.ToString();
                lblFollowupCompleted.Text = FollowupCompleted.ToString();
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }
        }

        protected void BtnRemove_PSF_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsdelete = new DataSet();

            try
            {
                dsdelete = (DataSet)Session["PostServiceFollowup"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;

                dsdelete.Tables[0].Rows[rowGvIndex].Delete();
                dsdelete.Tables[0].AcceptChanges();
                if (dsdelete.Tables[0].Rows.Count == 0)
                {
                    DataRow dr = dsdelete.Tables[0].NewRow();
                    dsdelete.Tables[0].Rows.Add(dr);
                }

                grdPostServiceFollowup.DataSource = dsdelete.Tables[0];
                grdPostServiceFollowup.DataBind();

                Session["PostServiceFollowUp"] = dsdelete;
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }
        }

        private void Create_Grid()
        {
            DataSet ds = new DataSet();

            ds.Tables.Add();

            ds.Tables[0].Columns.Add(new DataColumn("RegNo", typeof(string)));//3
            ds.Tables[0].Columns.Add(new DataColumn("JobCardCode", typeof(string)));//1
            ds.Tables[0].Columns.Add(new DataColumn("EndUserDesc", typeof(string)));//2
            ds.Tables[0].Columns.Add(new DataColumn("FollowUpDate", typeof(string)));//9
            ds.Tables[0].Columns.Add(new DataColumn("EndUserCellNo", typeof(string)));//7
            ds.Tables[0].Columns.Add(new DataColumn("KM", typeof(string)));//6
            ds.Tables[0].Columns.Add(new DataColumn("Schedule", typeof(string)));//4
            //ds.Tables[0].Columns.Add(new DataColumn("AmountPaid", typeof(string)));//8
            ds.Tables[0].Columns.Add(new DataColumn("DelvDate", typeof(string)));//5
            ds.Tables[0].Columns.Add(new DataColumn("Complain", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Remarks", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Satisfied", typeof(string)));//10

            DataRow dr = ds.Tables[0].NewRow();
            ds.Tables[0].Rows.Add(dr);

            grdPostServiceFollowup.DataSource = ds.Tables[0];
            grdPostServiceFollowup.DataBind();

            Session["PostServiceFollowup"] = ds;
        }

        private void LoadGRN_DDL(DropDownList ddl)
        {
            ddl.Items.Add(new ListItem("All", "All"));
            ddl.Items.Add(new ListItem("Not Available", "N/A"));
            ddl.Items.Add(new ListItem("Un-Satisfied", "No"));
            ddl.Items.Add(new ListItem("Satisfied", "Yes"));
        }

        protected void BtnAdd_PSF_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {

            PopulateGrid();
            RecordStatus();
            lblMessage.Text = "";
        }

        protected void gv_PSF_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataSet ds = new DataSet();

            grdPostServiceFollowup.EditIndex = -1;

            ds = (DataSet)Session["PostServiceFollowUp"];

            grdPostServiceFollowup.DataSource = ds.Tables[0];

            grdPostServiceFollowup.DataBind();

        }

        protected void gv_PSF_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                //ViewState["PostServiceFollowup"]
                Label lblComplain = (Label)grdPostServiceFollowup.Rows[e.NewEditIndex].FindControl("lblComplain");
                Label lblRemakrs = (Label)grdPostServiceFollowup.Rows[e.NewEditIndex].FindControl("lblRemarks");
                Label lblSatisfied = (Label)grdPostServiceFollowup.Rows[e.NewEditIndex].FindControl("lblSatisfied");

                Session["lblComplain"] = lblComplain.Text;
                Session["lblRemarks"] = lblRemakrs.Text;
                Session["lblSatisfied"] = lblSatisfied.Text;

                grdPostServiceFollowup.EditIndex = e.NewEditIndex;

                // ds = ViewState["PostServiceFollowup"];

                ds = (DataSet)Session["PostServiceFollowup"];

                grdPostServiceFollowup.DataSource = ds.Tables[0];

                grdPostServiceFollowup.DataBind();
            }

            catch (Exception ex)
            {

                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;

            }

        }


        protected void gv_PSF_DataBound(object sender, EventArgs e)
        {
            try
            {
                //LoadGRN_DDL();
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }
        }

        protected void gv_PSF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                ImageButton btn_remove = (ImageButton)e.Row.FindControl("BtnRemove_PSF");
                Label JobCode = (Label)e.Row.FindControl("lblJobCode");

                if (e.Row.RowType == DataControlRowType.DataRow && !(grdPostServiceFollowup.EditIndex == e.Row.RowIndex) && JobCode.Text == string.Empty)
                {

                    e.Row.Cells[0].Enabled = false;
                }


                if (e.Row.RowType == DataControlRowType.DataRow && (grdPostServiceFollowup.EditIndex == e.Row.RowIndex))
                {

                    DropDownList ddl_Satisfied = (DropDownList)e.Row.FindControl("ddl_Satisfied");
                    ddl_Satisfied.Items.Add(new ListItem("All", "All"));
                    ddl_Satisfied.Items.Add(new ListItem("Not Available", "N/A"));
                    ddl_Satisfied.Items.Add(new ListItem("Un-Satisfied", "No"));
                    ddl_Satisfied.Items.Add(new ListItem("Satisfied", "Yes"));

                    TextBox txtremarks = (TextBox)e.Row.FindControl("txtRemarks");
                    TextBox txtCompalin = (TextBox)e.Row.FindControl("txtComplain");


                    ddl_Satisfied.SelectedValue = Session["lblSatisfied"].ToString().Trim();
                    txtremarks.Text = Session["lblRemarks"].ToString();
                    txtCompalin.Text = Session["lblComplain"].ToString();



                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DropDownList ddl_Satisfied = (DropDownList)e.Row.FindControl("ddl_Satisfied");



                }
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }
        }


        protected void gv_PSF_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            try
            {
                DataSet ds_update = new DataSet();
                TextBox txtComplain = (TextBox)grdPostServiceFollowup.Rows[e.RowIndex].FindControl("txtComplain");
                TextBox txtRemakrs = (TextBox)grdPostServiceFollowup.Rows[e.RowIndex].FindControl("txtRemarks");
                DropDownList ddlSatisfied = (DropDownList)grdPostServiceFollowup.Rows[e.RowIndex].FindControl("ddl_Satisfied");

                Label JobCode = (Label)grdPostServiceFollowup.Rows[e.RowIndex].FindControl("lblJobCode");


                ds_update = (DataSet)Session["PostServiceFollowUp"];

                //Uzair
                foreach (DataRow dr in ds_update.Tables[0].Rows)
                {
                    if (dr["JobCardCode"].ToString().Trim() == JobCode.Text.Trim())
                    {
                        dr["Complain"] = txtComplain.Text;
                        dr["Remarks"] = txtRemakrs.Text;
                        dr["Satisfied"] = ddlSatisfied.SelectedValue;
                        dr["FollowUpDate"] = sysFunc.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"));
                    }

                }

                grdPostServiceFollowup.EditIndex = -1;

                grdPostServiceFollowup.DataSource = ds_update;
                grdPostServiceFollowup.DataBind();

                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {

                    string sql = "update CRM_Post_PostServiceFollowup set Satisfied = '" + ddlSatisfied.SelectedValue.ToString() + "', Remarks = '" + txtRemakrs.Text + "' , FollowUpDate ='" + sysFunc.SaveDate(DateTime.Now.ToString("dd-MM-yyyy")) + "'" +
                        ", Complain = '" + txtComplain.Text + "' where DealerCode = '" + Session["DealerCode"].ToString() + "' and JobCardCode = '" + JobCode.Text + "' ";
                    myFunc.ExecuteQuery(sql, Trans);

                }

                Session["PostServiceFollowUp"] = ds_update;
                ObjTrans.CommittTransaction(ref Trans);
            }
            catch (Exception ex)
            {
                //ObjTrans.CommittTransaction(ref Trans);
                ObjTrans.RollBackTransaction(ref Trans);
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }

        }
    }
}