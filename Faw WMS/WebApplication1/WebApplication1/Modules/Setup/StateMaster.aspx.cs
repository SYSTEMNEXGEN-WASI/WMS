using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace DXBMS.Modules.Setup
{
    public partial class StateMaster : System.Web.UI.Page
    {


        SysFunction sysFunc = new SysFunction();
        DataSet ds;
        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;
        //SysFunctions myFunc = new SysFunctions();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!IsPostBack)
            {
                createGrid();
            }
        }

        // String connString = " Data Source=PAKARMY-007;Initial Catalog=BMS;Integrated Security=True ";


        protected void btnInsert_Click(object sender, EventArgs e)
        {
            //String Query = "insert into State(CountryCode,StateCode,StateDesc) values (@CountryCode,@StateCode,@StateDesc)";

            //using (SqlConnection conn = new SqlConnection(connString))
            //{
            //    conn.Open();

            //    SqlCommand cmd = new SqlCommand(Query, conn);

            //    cmd.Parameters.AddWithValue("@CountryCode", txtCountryCode.Text);
            //    cmd.Parameters.AddWithValue("@StateCode", txtStateCode.Text);
            //    cmd.Parameters.AddWithValue("@StateDesc", txtStateDescription.Text);

            //    cmd.ExecuteNonQuery();

            try
            {
                SqlParameter[] param =
           {
                new SqlParameter("@CountryCode",SqlDbType.Char,3),//0
                new SqlParameter("@StateCode",SqlDbType.Char,3), //1
                new SqlParameter("@StateDesc",SqlDbType.VarChar,50)//2               

            };


                param[0].Value = txtCountryCode.Text;
                param[1].Value = txtStateCode.Text;
                param[2].Value = txtStateDescription.Text.Trim();


                sysFunc.ExecuteSP_NonQuery("SP_InsertState", param);
                sysFunc.ClearTextBoxes(Page);

                createGrid();

            }
            catch (Exception ex)
            {
                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);
            }

        }



        private void createGrid()
        {

            String query = "select * from State";

            DataTable dt = new DataTable();

            dt = sysFunc.GetData(query);

            if (dt.Rows.Count > 0)
            {
                gv_state.DataSource = dt;
                gv_state.DataBind();
            }

            Session["StateDS"] = dt;

        }



        protected void gv_state_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gv_state_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }


        // REMOVE CODE
        protected void BtnRemove_State_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsdelete = new DataSet();

            DataTable dt = new DataTable();

            try
            {
                dt = (DataTable)Session["StateDS"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;

                String code = dt.Rows[rowGvIndex]["StateCode"].ToString();

                if (sysFunc.CodeExists("Customer", "StateCode", code, Session["DealerCode"].ToString(), ref dsdelete))
                {
                    lblMessage.Text = "Cannot delete , this State is already in use";
                    return;
                }

                dt.Rows[rowGvIndex].Delete();
                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                gv_state.DataSource = dt;
                gv_state.DataBind();

                Session["StateDS"] = dt;
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    //and CountryDesc = '" + txtStateDescription.Text + "' and VersionCode = '" + dsdelete.Tables[0].Rows[rowGvIndex]["Version"].ToString() + "'

                    string sql = "delete from State where StateCode = '" + code.Trim() + "' ";
                    SqlDataReader reader = null;
                    sysFunc.ExecuteQuery(sql, ref reader);
                }
                ObjTrans.CommittTransaction(ref Trans);
                lblMsg.Visible = true;
                lblMsg.Text = "Record Deleted";
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }

        }


        protected void gv_state_DataBound(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }
        }



        // EDIT ROWS IN GRIDVIEW
        protected void gv_state_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                //DataSet ds = new DataSet();

                DataTable dtEditing = new DataTable();

                Label lblCountryCode = (Label)gv_state.Rows[e.NewEditIndex].FindControl("lblCountryCode");
                //Label lblStateCode = (Label)gv_state.Rows[e.NewEditIndex].FindControl("lblStateCode");
                Label lblCountryDesc = (Label)gv_state.Rows[e.NewEditIndex].FindControl("lblCountryDesc");

                Session["lblCountryCode"] = lblCountryCode.Text;
                //Session["lblStateCode"] = lblStateCode.Text;
                Session["lblCountryDesc"] = lblCountryDesc.Text;

                gv_state.EditIndex = e.NewEditIndex;

                dtEditing = (DataTable)Session["stateDS"];
                Session["StateDS"] = dtEditing;
                gv_state.DataSource = dtEditing;

                gv_state.DataBind();
            }

            catch (Exception ex)
            {

                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;

            }

        }

        // ROW CANCEL EDIT
        protected void gv_state_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataTable dt = new DataTable();

            gv_state.EditIndex = -1;

            dt = (DataTable)Session["StateDS"];

            gv_state.DataSource = dt;

            gv_state.DataBind();

        }


        // ROW UPDATING
        protected void gv_state_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataTable dt_update = new DataTable();
                TextBox txtCountryCode = (TextBox)gv_state.Rows[e.RowIndex].FindControl("txtCountryCode");
                Label lblStateCode = (Label)gv_state.Rows[e.RowIndex].FindControl("lblStateCode");
                TextBox txtCountryDesc = (TextBox)gv_state.Rows[e.RowIndex].FindControl("txtStateDescription");

                // Check value in GridView TextBOX
                if (txtCountryCode.Text.Trim() == string.Empty)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Country Code can not null or empty.";
                    return;
                }

                //if (txtStateCode.Text.Trim() == string.Empty)
                //{
                //    lblMsg.Visible = true;
                //    lblMsg.Text = "State Code can not null or empty.";
                //    return;
                //}
                if (txtCountryDesc.Text == "")
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Country Description can not be null";
                    return;
                }

                dt_update = (DataTable)Session["StateDS"];

                //Uzair
                //DataRow dr =  ds_update.Tables[0].Rows[0];
                // Check in GridView

                foreach (DataRow dr in dt_update.Rows)
                {
                    if (dr["StateCode"].ToString() == lblStateCode.Text)
                    {
                        dr["CountryCode"] = txtCountryCode.Text.Trim();
                        //dr["StateCode"] = txtStateCode.Text.Trim();
                        dr["StateDesc"] = txtCountryDesc.Text.Trim();

                    }
                }

                gv_state.EditIndex = -1;

                gv_state.DataSource = dt_update;
                gv_state.DataBind();

                Session["StateDS"] = dt_update;

                // Updating in DataBase
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {

                    string sql = "update State set CountryCode = '" + txtCountryCode.Text + "', StateDesc = '" + txtCountryDesc.Text + "' WHERE StateCode='" + lblStateCode.Text + "'  ";
                    SqlDataReader reader = null;
                    //sysFunc.ExecuteQuery(sql, Trans);
                    sysFunc.ExecuteQuery(sql, ref reader);
                }
                ObjTrans.CommittTransaction(ref Trans);
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }
        }

    }
}