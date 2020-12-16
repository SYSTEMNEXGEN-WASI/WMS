using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using DXBMS;


namespace DXBMS.Modules.WorkFlow
{
    public partial class WorkFlowRolls : System.Web.UI.Page
    {
        SysFunction sysFunc = new SysFunction();
        SysFunctions sysFuncs = new SysFunctions();
        DataSet ds;
        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lOADddl();
                createGrid();
            }
        }

        private void lOADddl()
        {
            ddlActive.Items.Clear();
            ddlActive.Items.Add(new ListItem("Select", "Select"));
            ddlActive.Items.Add(new ListItem("Y", "Y"));
            ddlActive.Items.Add(new ListItem("N", "N"));

            txtValidFrom.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtValidTo.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            if (txtRollsDesc.Text == "")
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please Enter Rolls Description", txtRollsDesc);
                return;
            }
            else if (txtPriority.Text == "")
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please Enter Priority", txtPriority);
                return;
            }
            else if (txtValidFrom.Text == "")
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please Enter Valid From Date", txtValidFrom);
                return;
            }
            else if (txtValidTo.Text == "")
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please Enter Valid To Date", txtValidTo);
                return;
            }
            else
            {
                lblMessage.Text = string.Empty;
            }

            try
            {
                string max = sysFuncs.GetNewMaxID("Rolls", "RollsID", 5, Session["DealerCode"].ToString());
                // string max = sysFuncs.AutoGen("WorkFlowSetup", "WFCode", Session["DealerCode"].ToString(), DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                SqlParameter[] param =
               {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                new SqlParameter("@RollsID",SqlDbType.Char,5), //1
                new SqlParameter("@RollsDesc",SqlDbType.VarChar,50),//2
                new SqlParameter("@Priority",SqlDbType.VarChar,50), //3
                new SqlParameter("@ValidFrom",SqlDbType.DateTime),//4   
                new SqlParameter("@ValidTo",SqlDbType.DateTime),
                new SqlParameter("@Active",SqlDbType.Char,1),
            };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = max;
                param[2].Value = txtRollsDesc.Text.Trim();
                param[3].Value = txtPriority.Text.Trim();
                param[4].Value = sysFuncs.SaveDate(txtValidFrom.Text);
                param[5].Value = sysFuncs.SaveDate(txtValidTo.Text);
                param[6].Value = ddlActive.SelectedValue.ToString().Trim();

                sysFunc.ExecuteSP_NonQuery("SP_WorkFlowRolls_insert", param);
                sysFunc.ClearTextBoxes(Page);

                createGrid();
                //txtWorkFlowID.Text = max;
                sysFuncs.UserMsg(lblMessage, Color.Green, "Data Inserted", txtRollsDesc);

            }
            catch (Exception ex)
            {
                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);
            }
        }

        private void createGrid()
        {
            String query = "select * from Rolls";

            DataTable dt = new DataTable();

            dt = sysFunc.GetData(query);

            if (dt.Rows.Count > 0)
            {
                gv_WFRolls.DataSource = dt;
                gv_WFRolls.DataBind();
            }
            Session["WFRollsDS"] = dt;
        }

        // Remove Button Code
        protected void BtnRemove_WorkFlowRolls_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsdelete = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                dt = (DataTable)Session["WFRollsDS"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex + gv_WFRolls.PageIndex * gv_WFRolls.PageSize;

                String code = dt.Rows[rowGvIndex]["RollsID"].ToString();

                dt.Rows[rowGvIndex].Delete();
                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                gv_WFRolls.DataSource = dt;
                gv_WFRolls.DataBind();

                Session["WFRollsDS"] = dt;
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    string sql = "delete from Rolls where RollsID = '" + code.Trim() + "' ";
                    SqlDataReader reader = null;
                    sysFunc.ExecuteQuery(sql, ref reader);
                }
                ObjTrans.CommittTransaction(ref Trans);

                lblMessage.Text = "Record Deleted";
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }
        }

        //// DATABOUND FUNCTION
        protected void gv_WFRolls_DataBound(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }
        }

        //
        protected void gv_WFRolls_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && !(gv_WFRolls.EditIndex == e.Row.RowIndex))
                {
                    e.Row.Cells[0].Enabled = true;
                }

                if (e.Row.RowType == DataControlRowType.DataRow && (gv_WFRolls.EditIndex == e.Row.RowIndex))
                {
                    //btn_remove.Enabled = false;

                    // DropDownList ddlbrand_code = (DropDownList)e.Row.FindControl("ddlbrand_code");
                    TextBox txtRollsDesc = (TextBox)e.Row.FindControl("txtRollsDescEdit");
                    TextBox txtPriority = (TextBox)e.Row.FindControl("txtPriorityEdit");
                    //TextBox txtStateCode = (TextBox)e.Row.FindControl("txtStateCode");
                    //TextBox txtCountryDesc = (TextBox)e.Row.FindControl("txtStateDescription");

                    txtRollsDesc.Text = Session["lblRollsDesc"].ToString();
                    txtPriority.Text = Session["lblPriority"].ToString();
                    //txtCountryDesc.Text = Session["lblCountryDesc"].ToString();
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {

                }
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }
        }


        //// FOR GRIDVIEW ROW EDITING
        protected void gv_WFRolls_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                DataTable dtEditing = new DataTable();

                Label lblRollsDesc = (Label)gv_WFRolls.Rows[e.NewEditIndex].FindControl("lblRollsDesc");
                Label lblPriority = (Label)gv_WFRolls.Rows[e.NewEditIndex].FindControl("lblPriority");
                //Label lblStateCode = (Label)gv_city.Rows[e.NewEditIndex].FindControl("lblStateCode");

                Session["lblRollsDesc"] = lblRollsDesc.Text;
                Session["lblPriority"] = lblPriority.Text;
                //Session["lblStateCode"] = lblStateCode.Text;

                gv_WFRolls.EditIndex = e.NewEditIndex;

                dtEditing = (DataTable)Session["WFRollsDS"];
                Session["WFRollsDS"] = dtEditing;
                gv_WFRolls.DataSource = dtEditing;

                gv_WFRolls.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;

            }

        }


        // ROW CANCEL EDIT
        protected void gv_WFRolls_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataTable dt = new DataTable();

            gv_WFRolls.EditIndex = -1;

            dt = (DataTable)Session["WFRollsDS"];

            gv_WFRolls.DataSource = dt;

            gv_WFRolls.DataBind();

        }

        // FOR GRIDVIEW ROW UPDATING
        protected void gv_WFRolls_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataTable dt_update = new DataTable();

                //Label lblDealerCode = (Label)gv_city.Rows[e.RowIndex].FindControl("lblDealerCode");
                Label lblRollsID = (Label)gv_WFRolls.Rows[e.RowIndex].FindControl("lblRollsID");
                TextBox txtRollsDescEdit = (TextBox)gv_WFRolls.Rows[e.RowIndex].FindControl("txtRollsDescEdit");     // GridView TextBox Field
                TextBox txtPriorityEdit = (TextBox)gv_WFRolls.Rows[e.RowIndex].FindControl("txtPriorityEdit");
                Label lblValidFrom = (Label)gv_WFRolls.Rows[e.RowIndex].FindControl("lblValidFrom");
                Label lblValidTo = (Label)gv_WFRolls.Rows[e.RowIndex].FindControl("lblValidTo");

                // Check value in GridView TextBOX
                if (txtRollsDescEdit.Text.Trim() == string.Empty)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Rolls can not null or empty.";
                    return;
                }
                if (txtPriorityEdit.Text.Trim() == string.Empty)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Priority can not null or empty.";
                    return;
                }

                dt_update = (DataTable)Session["WFRollsDS"];

                //Uzair
                //DataRow dr =  ds_update.Tables[0].Rows[0];
                // Check in GridView

                foreach (DataRow dr in dt_update.Rows)
                {
                    if (dr["RollsID"].ToString() == lblRollsID.Text)
                    {
                        dr["RollsDesc"] = txtRollsDescEdit.Text.Trim();
                        dr["Priority"] = txtPriorityEdit.Text.Trim();
                    }
                }

                gv_WFRolls.EditIndex = -1;

                gv_WFRolls.DataSource = dt_update;
                gv_WFRolls.DataBind();

                Session["WFRollsDS"] = dt_update;

                // Updating in DataBase
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {

                    string sql = "update Rolls set RollsDesc = '" + txtRollsDescEdit.Text + "', Priority = '" + txtPriorityEdit.Text + "'  WHERE RollsID='" + lblRollsID.Text + "'  ";
                    SqlDataReader reader = null;
                    //sysFunc.ExecuteQuery(sql, Trans);
                    sysFunc.ExecuteQuery(sql, ref reader);
                }
                ObjTrans.CommittTransaction(ref Trans);
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }

        }


        protected void gv_WFRolls_SelectedIndexChanged(object sender, GridViewPageEventArgs e)
        {
            gv_WFRolls.PageIndex = e.NewPageIndex;
            createGrid();
        }
    }
}