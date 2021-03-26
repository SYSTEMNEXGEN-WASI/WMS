using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DXBMS.Modules.Setup
{
    public partial class Department : System.Web.UI.Page
    {
        SysFunction sysFunc = new SysFunction();
        DataSet ds;
        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                createGrid();
                //LoadGRN_DDL();
            }
        }
        protected void gv_Dept_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gr = gv_Dept.SelectedRow;
                Label lbl = gr.FindControl("lblDepartmentCode") as Label;
                txtDepartmentCode.Text = lbl.Text;
                Label lbl1 = gr.FindControl("lblDeptDesc") as Label;
                txtDepartmentDesc.Text = lbl1.Text;

            }
            catch (Exception ex) { throw ex; }

        }

        private void createGrid()
        {

            String query = "select * from Department where DealerCode in( '" + Session["DealerCode"].ToString() + "','COMON')";

            DataTable dt = new DataTable();

            dt = sysFunc.GetData(query);

            if (dt.Rows.Count > 0)
            {
                gv_Dept.DataSource = dt;
                gv_Dept.DataBind();
            }

            Session["DeptDS"] = dt;

        }
        protected void btnInsert_Click(object sender, EventArgs e)
        {

            try
            {
                string max = sysFunc.GetNewMaxID("Department", "DeptCode", 3, Session["DealerCode"].ToString());

                SqlParameter[] param =
               {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                new SqlParameter("@DeptCode",SqlDbType.Char,3), //1
                new SqlParameter("@DeptDesc",SqlDbType.VarChar,50),//2
                new SqlParameter("@UpdUser",SqlDbType.VarChar,50), //3
                new SqlParameter("@UpdTerm",SqlDbType.VarChar,50),//4
            };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = max;
                param[2].Value = txtDepartmentDesc.Text.Trim();
                param[3].Value = Session["UserName"].ToString();
                param[4].Value = "1";
                if (txtDepartmentDesc.Text != "")
                {
                    sysFunc.ExecuteSP_NonQuery("SP_InsertDept", param);
                    sysFunc.ClearTextBoxes(Page);

                    createGrid();
                    lblMessage.ForeColor = Color.Green;

                    lblMessage.Text = "New Department " + param[2].Value + " Inserted.";
                }

                else
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Designation Description should not be left blank";
                    return;

                }


            }
            catch (Exception ex)
            {

                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);
            }

        }
        protected void BtnRemove_City_Click(object sender, ImageClickEventArgs e)

        {
            DataSet dsdelete = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                dt = (DataTable)Session["DeptDS"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex + gv_Dept.PageIndex * gv_Dept.PageSize;

                String code = dt.Rows[rowGvIndex]["DeptCode"].ToString();
                String Desc = dt.Rows[rowGvIndex]["DeptDesc"].ToString();

                if (sysFunc.CodeExists("Dealeremp", "DeptCode", code, Session["DealerCode"].ToString(), ref dsdelete))
                {
                    lblMessage.Text = "Cannot delete , this Department is already in use";
                    return;
                }

                dt.Rows[rowGvIndex].Delete();
                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                gv_Dept.DataSource = dt;
                gv_Dept.DataBind();

                Session["DeptDS"] = dt;
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    //and CountryDesc = '" + txtStateDescription.Text + "' and VersionCode = '" + dsdelete.Tables[0].Rows[rowGvIndex]["Version"].ToString() + "'

                    string sql = "delete from Department where DealerCode = '"+Session["DealerCode"].ToString()+"' and DeptCode = '" + code.Trim() + "' ";
                    SqlDataReader reader = null;
                    sysFunc.ExecuteQuery(sql, ref reader);
                }
                ObjTrans.CommittTransaction(ref Trans);

                lblMessage.Text = "Record " + Desc.Trim() + " Deleted";
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }

        }

        protected void gv_Dept_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {



                if (e.Row.RowType == DataControlRowType.DataRow && !(gv_Dept.EditIndex == e.Row.RowIndex))
                {

                    e.Row.Cells[0].Enabled = true;
                }

                if (e.Row.RowType == DataControlRowType.DataRow && (gv_Dept.EditIndex == e.Row.RowIndex))
                {
                    //btn_remove.Enabled = false;

                    // DropDownList ddlbrand_code = (DropDownList)e.Row.FindControl("ddlbrand_code");
                    //TextBox txtDeptCode = (TextBox)e.Row.FindControl("txtDepertmentCode");
                    TextBox txtDeptDesc = (TextBox)e.Row.FindControl("txtDeptDesc");
                    //TextBox txtCountryDesc = (TextBox)e.Row.FindControl("txtStateDescription");

                    //txtDeptCode.Text = Session["lblDepartmentCode"].ToString();
                    txtDeptDesc.Text = Session["lblDeptDesc"].ToString();
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
        protected void gv_Dept_DataBound(object sender, EventArgs e)
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
        protected void gv_Dept_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                DataTable dtEditing = new DataTable();

                Label lblDeptCode = (Label)gv_Dept.Rows[e.NewEditIndex].FindControl("lblDepartmentCode");
                Label lblDeptDesc = (Label)gv_Dept.Rows[e.NewEditIndex].FindControl("lblDeptDesc");

                Session["lblDepartmentCode"] = lblDeptCode.Text;
                Session["lblDeptDesc"] = lblDeptDesc.Text;

                gv_Dept.EditIndex = e.NewEditIndex;

                dtEditing = (DataTable)Session["DeptDS"];
                Session["DeptDS"] = dtEditing;
                gv_Dept.DataSource = dtEditing;

                gv_Dept.DataBind();
            }

            catch (Exception ex)
            {

                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;

            }

        }



        // ROW CANCEL EDIT
        protected void gv_Dept_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataTable dt = new DataTable();

            gv_Dept.EditIndex = -1;

            dt = (DataTable)Session["DeptDS"];

            gv_Dept.DataSource = dt;

            gv_Dept.DataBind();

        }



        // FOR GRIDVIEW ROW UPDATING
        protected void gv_Dept_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataTable dt_update = new DataTable();

                //Label lblDealerCode = (Label)gv_city.Rows[e.RowIndex].FindControl("lblDealerCode");
                Label lblDepartmentCode = (Label)gv_Dept.Rows[e.RowIndex].FindControl("lblDepartmentCode");
                TextBox txtDeptDesc = (TextBox)gv_Dept.Rows[e.RowIndex].FindControl("txtDeptDesc");     // GridView TextBox Fiel                     // GridView TextBox Field

                // Check value in GridView TextBOX
                if (txtDeptDesc.Text.Trim() == string.Empty)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Department Description can not be null or empty.";
                    return;
                }


                if (lblDepartmentCode.Text == "")
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Country Code can not be null";
                    return;
                }

                dt_update = (DataTable)Session["DeptDS"];

                //Uzair
                //DataRow dr =  ds_update.Tables[0].Rows[0];
                // Check in GridView

                foreach (DataRow dr in dt_update.Rows)
                {
                    if (dr["DeptCode"].ToString() == lblDepartmentCode.Text)
                    {
                        dr["DeptDesc"] = txtDeptDesc.Text.Trim();


                    }
                }

                gv_Dept.EditIndex = -1;

                gv_Dept.DataSource = dt_update;
                gv_Dept.DataBind();

                Session["DeptDS"] = dt_update;

                // Updating in DataBase
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {

                    string sql = "update Department set DeptDesc = '" + txtDeptDesc.Text + "' WHERE DealerCode = '" + Session["DealerCode"].ToString() + "' and DeptCode='" + lblDepartmentCode.Text + "'  ";
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

        protected void gv_Dept_SelectedIndexChanged(object sender, GridViewPageEventArgs e)
        {
            gv_Dept.PageIndex = e.NewPageIndex;
            createGrid();
        }
    }
}