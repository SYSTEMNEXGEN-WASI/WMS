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
    public partial class CityMaster : System.Web.UI.Page
    {
        SysFunction sysFunc = new SysFunction();
        DataSet ds;
        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!IsPostBack)
            {
                createGrid();
                LoadGRN_DDL();
            }
        }

        private void LoadGRN_DDL()
        {
            string WhereQuery = "1=1";
            //    string[] Columns = new string[] { "RegNo", "App_Id" };
            //    sysFunc.GetMultiColumnsDDL(ddlRegNo, Columns, "CustomerVisit", WhereQuery, "RegNo", "", false, false);

            string[] col = new string[] { "StateCode", "StateDesc" };
            sysFunc.GetMultiColumnsDDL(ddState, col, "State", WhereQuery, "StateCode", "", false, false);
        }


        // INSERT CODE
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            if (ddState.SelectedIndex == 0)
            {
                lblMessage.Text = "Select State First";
                return;
            }
            try
            {
                SqlParameter[] param =
           {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                new SqlParameter("@CityCode",SqlDbType.Char,3), //1
                new SqlParameter("@CityDesc",SqlDbType.VarChar,50),//2
                new SqlParameter("@UpdUser",SqlDbType.VarChar,50), //3
                new SqlParameter("@UpdTerm",SqlDbType.VarChar,50),//4
                new SqlParameter("@StateCode",SqlDbType.Char,3)//5               
            };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = txtCityCode.Text;
                param[2].Value = txtCityDescription.Text.Trim();
                param[3].Value = Session["UserName"].ToString();
                param[4].Value = "1";
                param[5].Value = ddState.SelectedValue.ToString().Trim();

                sysFunc.ExecuteSP_NonQuery("SP_InsertCity", param);
                sysFunc.ClearTextBoxes(Page);

                createGrid();
                lblMessage.Text = "New City Inserted.";
            }
            catch (Exception ex)
            {

                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);
            }

        }

        // SHOWING DATA
        private void createGrid()
        {

            String query = "select * from City where DealerCode in ('" + Session["DealerCode"].ToString() + "','AAAAA')";

            DataTable dt = new DataTable();

            dt = sysFunc.GetData(query);

            if (dt.Rows.Count > 0)
            {
                gv_city.DataSource = dt;
                gv_city.DataBind();
            }

            Session["CityDS"] = dt;

        }


        // Remove Button Code
        protected void BtnRemove_City_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsdelete = new DataSet();

            DataTable dt = new DataTable();

            try
            {
                dt = (DataTable)Session["CityDS"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;

                String code = dt.Rows[rowGvIndex]["CityCode"].ToString();

                if (sysFunc.CodeExists("Customer", "CityCode", code, Session["DealerCode"].ToString(), ref dsdelete))
                {
                    lblMessage.Text = "Cannot delete , this City is already in use";
                    return;
                }

                dt.Rows[rowGvIndex].Delete();
                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                gv_city.DataSource = dt;
                gv_city.DataBind();

                Session["CityDS"] = dt;
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    //and CountryDesc = '" + txtStateDescription.Text + "' and VersionCode = '" + dsdelete.Tables[0].Rows[rowGvIndex]["Version"].ToString() + "'

                    string sql = "delete from City where DealerCode in ('" + Session["DealerCode"].ToString() + "','AAAAA') and CityCode = '" + code.Trim() + "' ";
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



        protected void gv_city_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && !(gv_city.EditIndex == e.Row.RowIndex))
                {

                    e.Row.Cells[0].Enabled = true;
                }

                if (e.Row.RowType == DataControlRowType.DataRow && (gv_city.EditIndex == e.Row.RowIndex))
                {
                    //btn_remove.Enabled = false;

                    // DropDownList ddlbrand_code = (DropDownList)e.Row.FindControl("ddlbrand_code");
                    TextBox txtCountryCode = (TextBox)e.Row.FindControl("txtCityDescription");
                    TextBox txtStateCode = (TextBox)e.Row.FindControl("txtStateCode");
                    //TextBox txtCountryDesc = (TextBox)e.Row.FindControl("txtStateDescription");

                    txtCountryCode.Text = Session["lblCityDesc"].ToString();
                    txtStateCode.Text = Session["lblStateCode"].ToString();
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

        // DATABOUND FUNCTION
        protected void gv_city_DataBound(object sender, EventArgs e)
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

        // FOR GRIDVIEW ROW EDITING
        protected void gv_city_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                //DataSet ds = new DataSet();

                DataTable dtEditing = new DataTable();
               
                Label lblCityDesc = (Label)gv_city.Rows[e.NewEditIndex].FindControl("lblCityDesc");
                Label lblStateCode = (Label)gv_city.Rows[e.NewEditIndex].FindControl("lblStateCode");

                Session["lblCityDesc"] = lblCityDesc.Text;
                Session["lblStateCode"] = lblStateCode.Text;

                gv_city.EditIndex = e.NewEditIndex;

                dtEditing = (DataTable)Session["CityDS"];
                Session["CityDS"] = dtEditing;
                gv_city.DataSource = dtEditing;

                gv_city.DataBind();
            }

            catch (Exception ex)
            {

                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;

            }

        }



        // ROW CANCEL EDIT
        protected void gv_city_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataTable dt = new DataTable();

            gv_city.EditIndex = -1;

            dt = (DataTable)Session["CityDS"];

            gv_city.DataSource = dt;

            gv_city.DataBind();

        }



        // FOR GRIDVIEW ROW UPDATING
        protected void gv_city_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataTable dt_update = new DataTable();

                Label lblCityCode = (Label)gv_city.Rows[e.RowIndex].FindControl("lblCityCode");
                TextBox txtCityDescription = (TextBox)gv_city.Rows[e.RowIndex].FindControl("txtCityDescription");     // GridView TextBox Field

                TextBox txtStateCode = (TextBox)gv_city.Rows[e.RowIndex].FindControl("txtStateCode");                     // GridView TextBox Field

                // Check value in GridView TextBOX
                if (txtCityDescription.Text.Trim() == string.Empty)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Country Code can not null or empty.";
                    return;
                }


                if (txtStateCode.Text == "")
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Country Description can not be null";
                    return;
                }

                dt_update = (DataTable)Session["CityDS"];

                //Uzair
                //DataRow dr =  ds_update.Tables[0].Rows[0];
                // Check in GridView

                foreach (DataRow dr in dt_update.Rows)
                {
                    if (dr["CityCode"].ToString() == lblCityCode.Text)
                    {
                        dr["CityDesc"] = txtCityDescription.Text.Trim();
                        //dr["StateCode"] = txtStateCode.Text.Trim();
                        dr["StateCode"] = txtStateCode.Text.Trim();

                    }
                }

                gv_city.EditIndex = -1;

                gv_city.DataSource = dt_update;
                gv_city.DataBind();

                Session["CityDS"] = dt_update;

                // Updating in DataBase
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {

                    string sql = "update City set CityDesc = '" + txtCityDescription.Text + "', StateCode = '" + txtStateCode.Text + "' WHERE DealerCode in ('"+Session["DealerCode"].ToString()+"','AAAAA') and CityCode='" + lblCityCode.Text + "'  ";
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



        protected void gv_city_SelectedIndexChanged(object sender, GridViewPageEventArgs e)
        {
            gv_city.PageIndex = e.NewPageIndex;
            createGrid();
        }
    }
}