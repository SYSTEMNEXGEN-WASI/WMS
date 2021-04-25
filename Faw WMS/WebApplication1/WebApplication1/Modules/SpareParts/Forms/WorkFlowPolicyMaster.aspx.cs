using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace DXBMS.Modules.SpareParts.Forms
{
    public partial class WorkFlowPolicyMaster : System.Web.UI.Page
    {

        SysFunction sysFunc = new SysFunction();
        SysFunctions sysFuncs = new SysFunctions();
        DataSet ds;
        DataTable PartsDT;
        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (ViewState["WFPolicyDS"] != null) PartsDT = (DataTable)ViewState["WFPolicyDS"];

            if (!IsPostBack)
            {
                lOADddl();
                Create_Grid();
            }
        }

        private void lOADddl()
        {
            ddlPolicyType.Items.Clear();
            ddlPolicyType.Items.Add(new ListItem("Select", "Select"));
            ddlPolicyType.Items.Add(new ListItem("General", "General"));
            ddlPolicyType.Items.Add(new ListItem("Extended", "Extended"));

            ddlStatus.Items.Clear();
            ddlStatus.Items.Add(new ListItem("Select", "Select"));
            ddlStatus.Items.Add(new ListItem("Active", "Active"));
            ddlStatus.Items.Add(new ListItem("De-Active", "De-Active"));

            //// DDLColums retrive
            string WhereQuery = "DealerCode= '" + Session["DealerCode"].ToString() + "' ";

            string[] col1 = new string[] { "Solution_Code", "Solution_Desc" };
            sysFunc.GetMultiColumnsDDL(ddlSolution, col1, "Solution", WhereQuery, "Solution_Code", "", false, false);

            string[] col2 = new string[] { "WFPolicyMasterCode", "WFPolicyMasterDesc" };
            sysFunc.GetMultiColumnsDDL(ddlWFPolicyMasterCode, col2, "WFPolicyMaster", WhereQuery, "WFPolicyMasterCode", "", false, false);

            txtStartDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtEndDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }

        private void cleardata()
        {
            sysFunc.Clearddl(Page);
            sysFunc.ClearTextBoxes(Page);
            Create_Grid();
        }

        protected void btnClear0_Click(object sender, EventArgs e)
        {
            cleardata();
        }
        private void Create_Grid()
        {
            ds = new DataSet();
            ds.Tables.Add();

            ds.Tables[0].Columns.Add(new DataColumn("WFPolicyMasterCode", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("ApprovalCode", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("RollsID", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Priority", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("ApprovalRoll", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("BatchNo", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Status", typeof(string)));

            DataRow dr = ds.Tables[0].NewRow();
            ds.Tables[0].Rows.Add(dr);

            gv_WFPolicy.DataSource = ds.Tables[0];
            gv_WFPolicy.DataBind();

            Session["WFPolicyDS"] = ds;
        }

        protected void create_row()
        {
            DropDownList ddlApprover = (DropDownList)gv_WFPolicy.FooterRow.FindControl("ddlApprover");
            //DropDownList ddlRollsDesc = (DropDownList)gv_WFPolicy.FooterRow.FindControl("ddlRollsDesc");
            DropDownList ddlPriority = (DropDownList)gv_WFPolicy.FooterRow.FindControl("ddlPriority");
            DropDownList ddlRole = (DropDownList)gv_WFPolicy.FooterRow.FindControl("ddlRole");

            TextBox txtEmployee = (TextBox)gv_WFPolicy.FooterRow.FindControl("txtEmployee");
            TextBox txtStatus = (TextBox)gv_WFPolicy.FooterRow.FindControl("txtStatus");

            try
            {

                DataTable dt_newrow = new DataTable();
                ds = new DataSet();
                ds = (DataSet)Session["WFPolicyDS"];
                DataRow row = ds.Tables[0].NewRow();

                row["ApprovalCode"] = ddlApprover.SelectedValue;
                // row["RollsID"] = ddlRollsDesc.SelectedValue;
                row["Priority"] = ddlPriority.SelectedValue;
                row["ApprovalRoll"] = ddlRole.SelectedValue;
                row["BatchNo"] = txtEmployee.Text;
                row["Status"] = txtStatus.Text;


                ds.Tables[0].Rows.Add(row);

                gv_WFPolicy.DataSource = ds.Tables[0];
                gv_WFPolicy.DataBind();

                Session["WFPolicyDS"] = ds;

            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////

        // Grid ADD
        protected void BtnAdd_WFPolicy_Click(object sender, ImageClickEventArgs e)
        {

            ds = new DataSet();

            //TextBox txtApprover = (TextBox)gv_WFPolicy.FooterRow.FindControl("txtApprover");
            //TextBox txtPriority = (TextBox)gv_WFPolicy.FooterRow.FindControl("txtPriority");
            //Label lblApprover = (Label)gv_WFPolicy.FooterRow.FindControl("lblApprover");
            DropDownList ddlApprover = (DropDownList)gv_WFPolicy.FooterRow.FindControl("ddlApprover");
            //DropDownList ddlRollsDesc = (DropDownList)gv_WFPolicy.FooterRow.FindControl("ddlRollsDesc");
            DropDownList ddlPriority = (DropDownList)gv_WFPolicy.FooterRow.FindControl("ddlPriority");
            DropDownList ddlRole = (DropDownList)gv_WFPolicy.FooterRow.FindControl("ddlRole");

            //DropDownList ddlPriority = (DropDownList)gv_WFPolicy.FooterRow.FindControl("ddlPriority");
            //TextBox txtRole = (TextBox)gv_WFPolicy.FooterRow.FindControl("txtRole");
            TextBox txtEmployee = (TextBox)gv_WFPolicy.FooterRow.FindControl("txtEmployee");
            TextBox txtStatus = (TextBox)gv_WFPolicy.FooterRow.FindControl("txtStatus");


            if (ddlPriority.SelectedIndex == 0)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Select Priority";
                return;
            }
            if (ddlRole.SelectedIndex == 0)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Select Role";
                return;
            }

            if (txtEmployee.Text == "" || txtEmployee.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Employee cannot be empty ";
                return;
            }
            if (txtStatus.Text == "" || txtStatus.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Status cannot be empty ";
                return;
            }

            try
            {
                DataTable dt = new DataTable();
                //string sql = "Select * from WFPolicyDetail where DealerCode = '" + Session["DealerCode"].ToString() + "' ";
                //dt = sysFunc.GetData(sql);


                ds = (DataSet)Session["WFPolicyDS"];

                //foreach (DataRow dr in ds.Tables[0].Rows)
                //{
                //    if (ddlcolorcode.SelectedValue == dr["Color"].ToString())
                //    {
                //        lblMsg.Visible = true;
                //        lblMsg.Text = "Color Code: " + ddlcolorcode.SelectedValue + " Already Selected";
                //        return;
                //    }
                //}

                if (ds.Tables[0].Rows[0]["ApprovalCode"].ToString() == string.Empty)
                {
                    ds.Tables[0].Rows.RemoveAt(0);
                    create_row();
                }
                else
                {
                    create_row();
                }

                Session["WFPolicyDS"] = ds;

            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;
            }
        }



        protected void btnSave1_Click(object sender, EventArgs e)
        {
            if (ddlSolution.SelectedIndex == 0)
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please Select Solution", txtComment);
                return;
            }
            else if (ddlPolicyType.SelectedIndex == 0)
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please Select PolicyType", txtComment);
                return;
            }
            else if (ddlStatus.SelectedIndex == 0)
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please Select Status", txtComment);
                return;
            }
            else if (txtComment.Text == "")
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please Enter Comment", txtComment);
                return;
            }
            else
            {
                lblMessage.Text = string.Empty;
            }


            try
            {
                SqlParameter[] param1 = {

                                    new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                    new SqlParameter("@WFPolicyMasterID",SqlDbType.Char),//1
                                    new SqlParameter("@WFPolicyMasterCode",SqlDbType.Char),//2 WFPolicyMasterDesc
                                    new SqlParameter("@WFPolicyMasterDesc",SqlDbType.VarChar), //3
                                    new SqlParameter("@Solution_Code",SqlDbType.Char),//4
                                    new SqlParameter("@PolicyType",SqlDbType.VarChar),//5
                                    new SqlParameter("@PolicyStartDate",SqlDbType.DateTime),//6
                                    new SqlParameter("@PolicyEndDate",SqlDbType.DateTime), //7
                                    new SqlParameter("@Status",SqlDbType.VarChar),  //8
                                    new SqlParameter("@Comments",SqlDbType.VarChar),  //9
                                    new SqlParameter("@DelFlag",SqlDbType.Char),   //10
                                    new SqlParameter("@UpdUser",SqlDbType.VarChar),//11
                                    new SqlParameter("@UpdTerm",SqlDbType.VarChar),//12                                   
                                    
           };

                string max2;

                string max1 = sysFuncs.GetNewMaxID("WFPolicyMaster", "WFPolicyMasterID", 4, Session["DealerCode"].ToString());
                if (ddlWFPolicyMasterCode.SelectedIndex == 0)
                {
                    max2 = sysFuncs.AutoGen("WFPolicyMaster", "WFPolicyMasterCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                }
                else
                {
                    max2 = ddlWFPolicyMasterCode.SelectedValue;
                }


                param1[0].Value = Session["DealerCode"].ToString();
                param1[1].Value = max1;
                param1[2].Value = max2;
                param1[3].Value = txtPolicyDesc.Text;
                param1[4].Value = ddlSolution.SelectedValue;
                param1[5].Value = ddlPolicyType.SelectedValue;
                param1[6].Value = txtStartDate.Text.Trim();
                param1[7].Value = txtEndDate.Text.Trim();
                param1[8].Value = ddlStatus.SelectedValue;
                param1[9].Value = txtComment.Text.Trim();
                param1[10].Value = 'N';
                param1[11].Value = Session["UserName"].ToString();
                param1[12].Value = "1";

                if (sysFunc.ExecuteSP_NonQuery("SP_WFPolicyMaster_insert", param1))
                {
                    SqlParameter[] param2 = {

                                    new SqlParameter("@DealerCode",SqlDbType.Char),         //0
                                    new SqlParameter("@WFPolicyMasterID",SqlDbType.Char),   //1
                                    new SqlParameter("@WFPolicyMasterCode",SqlDbType.Char), //2
                                    new SqlParameter("@ApprovalCode",SqlDbType.Char),       //3
                                    new SqlParameter("@Priority",SqlDbType.Int),            //4
                                    new SqlParameter("@ApprovalRoll",SqlDbType.VarChar),    //5
                                    new SqlParameter("@BatchNo",SqlDbType.Char),            //6
                                   // new SqlParameter("@IsMandatory",SqlDbType.VarChar),     //7
                                    new SqlParameter("@Status",SqlDbType.VarChar),          //7



           };

                    //string max3 = sysFuncs.GetNewMaxID("WFPolicyDetail", "ApprovalCode", 10, Session["DealerCode"].ToString());

                    PartsDT = (DataTable)ViewState["WFPolicyDS"];
                    DataSet ds = (DataSet)Session["WFPolicyDS"];
                    DataRow[] drr = ds.Tables[0].Select();

                    for (int j = 0; j < drr.Length; j++)
                    {
                        if (ds.Tables[0].Rows[j]["ApprovalCode"].ToString() != "")
                        {
                            param2[0].Value = Session["DealerCode"].ToString();
                            param2[1].Value = max1;
                            param2[2].Value = max2;
                            param2[3].Value = ds.Tables[0].Rows[j]["ApprovalCode"].ToString();
                            param2[4].Value = ds.Tables[0].Rows[j]["Priority"].ToString();
                            param2[5].Value = ds.Tables[0].Rows[j]["ApprovalRoll"].ToString();
                            param2[6].Value = ds.Tables[0].Rows[j]["BatchNo"].ToString();
                            param2[7].Value = ds.Tables[0].Rows[j]["Status"].ToString();
                        }

                        sysFunc.ExecuteSP_NonQuery("SP_WFPolicyDetail_insert", param2);
                        sysFuncs.UserMsg(lblMsg, Color.Green, "Data Inserted", txtComment);
                    }
                }
            }

            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;
            }
        }


        // ddlWFPolicyMasterCode
        protected void ddlWFPolicyMasterCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            SqlDataReader dr = null;

            // Data load in ddlchargoutNo
            SqlParameter[] param =
       {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                new SqlParameter("@WFPolicyMasterCode",SqlDbType.Char,8), //1

            };
            param[0].Value = Session["DealerCode"].ToString(); // 0
            param[1].Value = ddlWFPolicyMasterCode.SelectedValue.Trim();

            ds = sysFuncs.FillDataSet("SP_WFPolicyMaster_WFSelect", param);


            // Showing Data in Text Field through DropDownLIST
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtPolicyDesc.Text = ds.Tables[0].Rows[0]["WFPolicyMasterDesc"].ToString().Trim();
                ddlSolution.SelectedValue = ds.Tables[0].Rows[0]["Solution_Code"].ToString().Trim();
                ddlPolicyType.SelectedValue = ds.Tables[0].Rows[0]["PolicyType"].ToString().Trim();
                ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["Status"].ToString().Trim();

                txtStartDate.Text = ds.Tables[0].Rows[0]["PolicyStartDate"].ToString().Trim();

                txtEndDate.Text = ds.Tables[0].Rows[0]["PolicyEndDate"].ToString().Trim();

                txtComment.Text = ds.Tables[0].Rows[0]["Comments"].ToString().Trim();

                gv_WFPolicy.DataSource = ds.Tables[0];
                gv_WFPolicy.DataBind();

            }
            else
            {
                Create_Grid();
                return;
            }

            Session["WFPolicyDS"] = ds;
        }


        protected void BtnRemove_WFPolicy_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsdelete = new DataSet();

            try
            {
                dsdelete = (DataSet)Session["WFPolicyDS"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;
                //String code = dsdelete.Tables[0].Rows[rowGvIndex]["ApprovalCode"].ToString();
                String code = dsdelete.Tables[0].Rows[rowGvIndex]["ApprovalCode"].ToString();
                dsdelete.Tables[0].Rows[rowGvIndex].Delete();
                dsdelete.Tables[0].AcceptChanges();
                if (dsdelete.Tables[0].Rows.Count == 0)
                {
                    DataRow dr = dsdelete.Tables[0].NewRow();
                    dsdelete.Tables[0].Rows.Add(dr);
                }

                gv_WFPolicy.DataSource = dsdelete.Tables[0];
                gv_WFPolicy.DataBind();

                Session["WFPolicyDS"] = dsdelete;
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    string sql = "delete from WFPolicyDetail where ApprovalCode = '" + code.Trim() + "' ";
                    SqlDataReader reader = null;
                    sysFunc.ExecuteQuery(sql, ref reader);
                }
                ObjTrans.CommittTransaction(ref Trans);
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }
        }


        protected void gv_WFPolicy_DataBound(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;
            }
        }


        protected void gv_WFPolicy_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Label lblApprover = (Label)e.Row.FindControl("lblApprover");
                ImageButton btn_remove = (ImageButton)e.Row.FindControl("BtnRemove_WFPolicy");

                if (e.Row.RowType == DataControlRowType.DataRow && !(gv_WFPolicy.EditIndex == e.Row.RowIndex))
                {
                    //btn_remove.Enabled = false;
                    //e.Row.Cells[0].Enabled = false;
                }

                if (e.Row.RowType == DataControlRowType.DataRow && (gv_WFPolicy.EditIndex == e.Row.RowIndex))
                {

                    //btn_remove.Enabled = false;
                    DropDownList ddlPriorityEdit = (DropDownList)e.Row.FindControl("ddlPriorityEdit");
                    DropDownList ddlRoleEdit = (DropDownList)e.Row.FindControl("ddlRoleEdit");
                    //TextBox txtRole = (TextBox)e.Row.FindControl("txtRoleEdit");
                    TextBox txtStatus = (TextBox)e.Row.FindControl("txtStatusEdit");

                    // txtRole.Text = Session["lblRole"].ToString();


                    string WhereQuery = "1=1";
                    string[] Col4 = new string[] { "Priority" };
                    sysFunc.GetMultiColumnsDDL(ddlPriorityEdit, Col4, "Rolls", WhereQuery, "Priority", "", false, false);

                    string[] Col6 = new string[] { "RollsDesc" };
                    sysFunc.GetMultiColumnsDDL(ddlRoleEdit, Col6, "Rolls", WhereQuery, "RollsDesc", "", false, false);

                    ddlPriorityEdit.SelectedValue = Session["lblPriority"].ToString();
                    ddlRoleEdit.SelectedValue = Session["lblRole"].ToString();
                    txtStatus.Text = Session["lblStatus"].ToString();
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DropDownList ddlPriority = (DropDownList)e.Row.FindControl("ddlPriority");
                    // DropDownList ddlRollsDesc = (DropDownList)e.Row.FindControl("ddlRollsDesc");
                    DropDownList ddlRole = (DropDownList)e.Row.FindControl("ddlRole");
                    DropDownList ddlApprover = (DropDownList)e.Row.FindControl("ddlApprover");

                    string WhereQuery = "1=1";
                    string[] Col4 = new string[] { "Priority" };
                    sysFunc.GetMultiColumnsDDL(ddlPriority, Col4, "Rolls", WhereQuery, "Priority", "", false, false);

                    string[] Col5 = new string[] { "RollsDesc" };
                    sysFunc.GetMultiColumnsDDL(ddlApprover, Col5, "Rolls", WhereQuery, "RollsDesc", "", false, false);

                    string[] Col6 = new string[] { "RollsDesc" };
                    sysFunc.GetMultiColumnsDDL(ddlRole, Col6, "Rolls", WhereQuery, "RollsDesc", "", false, false);
                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;
            }
        }


        //// FOR GRIDVIEW ROW EDITING
        protected void gv_WFPolicy_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                Label lblPriority = (Label)gv_WFPolicy.Rows[e.NewEditIndex].FindControl("lblPriority");
                Label lblRole = (Label)gv_WFPolicy.Rows[e.NewEditIndex].FindControl("lblRole");
                Label lblStatus = (Label)gv_WFPolicy.Rows[e.NewEditIndex].FindControl("lblStatus");

                Session["lblPriority"] = lblPriority.Text;
                Session["lblRole"] = lblRole.Text;
                Session["lblStatus"] = lblStatus.Text;

                gv_WFPolicy.EditIndex = e.NewEditIndex;

                ds = (DataSet)Session["WFPolicyDS"];

                gv_WFPolicy.DataSource = ds.Tables[0];

                gv_WFPolicy.DataBind();
                Session["WFPolicyDS"] = ds;
            }

            catch (Exception ex)
            {

                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;

            }
        }


        // ROW CANCEL EDIT
        protected void gv_WFPolicy_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //DataTable dt = new DataTable();
            //gv_WFPolicy.EditIndex = -1;
            //dt = (DataTable)Session["WFPolicyDS"];
            //gv_WFPolicy.DataSource = dt;
            //gv_WFPolicy.DataBind();
            DataSet ds = new DataSet();

            gv_WFPolicy.EditIndex = -1;

            ds = (DataSet)Session["WFPolicyDS"];

            gv_WFPolicy.DataSource = ds.Tables[0];

            gv_WFPolicy.DataBind();
        }

        // RowUpdating
        protected void gv_WFPolicy_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataSet ds_update = new DataSet();

            Label lblApprover = (Label)gv_WFPolicy.Rows[e.RowIndex].FindControl("lblApprover");
            //Label lblPriority = (Label)gv_WFPolicy.Rows[e.RowIndex].FindControl("lblPriority");

            DropDownList ddlPriorityEdit = (DropDownList)gv_WFPolicy.Rows[e.RowIndex].FindControl("ddlPriorityEdit");
            //TextBox txtRole = (TextBox)gv_WFPolicy.Rows[e.RowIndex].FindControl("txtRoleEdit");

            DropDownList ddlRoleEdit = (DropDownList)gv_WFPolicy.Rows[e.RowIndex].FindControl("ddlRoleEdit");
            Label lblEmployee = (Label)gv_WFPolicy.Rows[e.RowIndex].FindControl("lblEmployee");
            TextBox txtStatus = (TextBox)gv_WFPolicy.Rows[e.RowIndex].FindControl("txtStatusEdit");

            try
            {
                ds_update = (DataSet)Session["WFPolicyDS"];

                foreach (DataRow dr in ds_update.Tables[0].Rows)
                {
                    if (dr["ApprovalCode"].ToString() == lblApprover.Text)
                    {
                        dr["Priority"] = ddlPriorityEdit.SelectedValue;
                        dr["ApprovalRoll"] = ddlRoleEdit.SelectedValue;
                        dr["Status"] = txtStatus.Text.Trim();
                    }
                }

                gv_WFPolicy.EditIndex = -1;

                gv_WFPolicy.DataSource = ds_update.Tables[0];
                gv_WFPolicy.DataBind();

                Session["WFPolicyDS"] = ds_update;

                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    SqlDataReader reader = null;
                    string sql = "update WFPolicyDetail set Priority = '" + ddlPriorityEdit.SelectedValue + "', ApprovalRoll = '" + ddlRoleEdit.SelectedValue + "', Status = '" + txtStatus.Text + "'  WHERE ApprovalCode='" + lblApprover.Text + "'  ";

                    sysFunc.ExecuteQuery(sql, ref reader);
                }

                ObjTrans.CommittTransaction(ref Trans);
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;
            }
        }

    }
}