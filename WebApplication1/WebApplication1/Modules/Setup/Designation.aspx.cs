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
    public partial class Designation : System.Web.UI.Page
    {
        SysFunction sysFunc = new SysFunction();
        DataSet ds;
        SysFunctions sysFuncs = new SysFunctions();

        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lOADddl();
                createGrid();
                //LoadGRN_DDL();
            }
        }

        private void lOADddl()
        {
            ddlDeptCode.Items.Clear();
            ddlDeptCode.Items.Add(new ListItem("Select", "Select"));
            ddlDeptCode.Items.Add(new ListItem("General", "General"));
            ddlDeptCode.Items.Add(new ListItem("Extended", "Extended"));

            //// DDLColums retrive
            string WhereQuery = "DealerCode in ( '" + Session["DealerCode"].ToString() + "','COMON') ";

            string[] col2 = new string[] { "DeptCode", "DeptDesc" };
            sysFunc.GetMultiColumnsDDL(ddlDeptCode, col2, "Department", WhereQuery, "DeptCode", "", false, false);

        }


        protected void gv_Desig_SelectedIndexChanged(object sender, EventArgs e)
        {
            try

            {
                GridViewRow gr = gv_Desig.SelectedRow;
                Label lbl = gr.FindControl("lblDesigCode") as Label;
                txtDesigCode.Text = lbl.Text;
                Label lbl1 = gr.FindControl("lblDesigDesc") as Label;
                txtDesigDesc.Text = lbl1.Text;
                Label lbl2 = gr.FindControl("lblDeptCode") as Label;

                if(lbl2.Text.Trim() != "" && lbl2.Text != null) 
                    ddlDeptCode.SelectedValue = lbl2.Text;

            }
            catch (Exception ex) { throw ex; }

        }

        private void createGrid()
        {

            String query = "select a.DesigCode, a.DesigDesc, isnull(a.DeptCode,'') DeptCode, isnull((select b.DeptDesc from Department b where a.DeptCode = b.DeptCode and a.DealerCode = b.DealerCode),'') DeptDesc from  Designation a where a.DealerCode in('"+Session["DealerCode"].ToString()+"','COMON')";

            DataTable dt = new DataTable();

            dt = sysFunc.GetData(query);

            if (dt.Rows.Count > 0)
            {
                gv_Desig.DataSource = dt;
                gv_Desig.DataBind();
            }

            Session["DesigDS"] = dt;

        }
        protected void btnInsert_Click(object sender, EventArgs e)
        {

            try
            {
                string max = sysFunc.GetNewMaxID("Designation", "DesigCode", 3, Session["DealerCode"].ToString());

                SqlParameter[] param =
               {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                new SqlParameter("@DeptCode",SqlDbType.Char,3),//0
                new SqlParameter("@DesigCode",SqlDbType.Char,3), //1
                new SqlParameter("@DesigDesc",SqlDbType.VarChar,50),//2
                new SqlParameter("@UpdUser",SqlDbType.VarChar,50), //3
                new SqlParameter("@UpdTerm",SqlDbType.VarChar,50),//4
            };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlDeptCode.SelectedValue;
                param[2].Value = max;
                param[3].Value = txtDesigDesc.Text.Trim();
                param[4].Value = Session["UserName"].ToString();
                param[5].Value = "1";

                if (txtDesigDesc.Text != "")
                {
                    sysFunc.ExecuteSP_NonQuery("SP_InsertDesig", param);

                    sysFunc.ClearTextBoxes(Page);

                    createGrid();
                    lblMessage.ForeColor = Color.Green;

                    lblMessage.Text = "New Designation " + param[2].Value + " Inserted.";

                }

                else
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Designation Description can not be null or empty";
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
                dt = (DataTable)Session["DesigDS"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex + gv_Desig.PageIndex * gv_Desig.PageSize;

                String code = dt.Rows[rowGvIndex]["DesigCode"].ToString();
                String Desc = dt.Rows[rowGvIndex]["DesigDesc"].ToString();

                if (sysFunc.CodeExists("Dealeremp", "DesigCode", code, Session["DealerCode"].ToString(), ref dsdelete))
                {
                    lblMessage.Text = "Cannot delete , this Designation is already in use";
                    return;
                }

                dt.Rows[rowGvIndex].Delete();
                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                gv_Desig.DataSource = dt;
                gv_Desig.DataBind();

                Session["DesigDS"] = dt;
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    //and CountryDesc = '" + txtStateDescription.Text + "' and VersionCode = '" + dsdelete.Tables[0].Rows[rowGvIndex]["Version"].ToString() + "'

                    string sql = "delete from Designation where DealerCode = '" + Session["DealerCode"].ToString() + "' and DesigCode = '" + code.Trim() + "' ";
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

        protected void gv_Desig_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && !(gv_Desig.EditIndex == e.Row.RowIndex))
                {
                    e.Row.Cells[0].Enabled = true;
                }

                if (e.Row.RowType == DataControlRowType.DataRow && (gv_Desig.EditIndex == e.Row.RowIndex))
                {
                    //btn_remove.Enabled = false;

                    // DropDownList ddlbrand_code = (DropDownList)e.Row.FindControl("ddlbrand_code");
                    //TextBox txtDesigCode = (TextBox)e.Row.FindControl("txtDesigCode");
                    TextBox txtDesigDesc = (TextBox)e.Row.FindControl("txtDesigDesc");

                    //TextBox txtCountryDesc = (TextBox)e.Row.FindControl("txtStateDescription");

                    ////txtDesigCode.Text = Session["txtDesigCode"].ToString();
                    txtDesigDesc.Text = Session["txtDesigDesc"].ToString();
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
        protected void gv_Desig_DataBound(object sender, EventArgs e)
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
        protected void gv_Desig_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                DataTable dtEditing = new DataTable();

                Label lblDesigCode = (Label)gv_Desig.Rows[e.NewEditIndex].FindControl("lblDesigCode");
                Label lblDesigDesc = (Label)gv_Desig.Rows[e.NewEditIndex].FindControl("lblDesigDesc");

                Session["lblDesigCode"] = lblDesigCode.Text;
                Session["lblDesigDesc"] = lblDesigDesc.Text;

                gv_Desig.EditIndex = e.NewEditIndex;

                dtEditing = (DataTable)Session["DesigDS"];
                Session["DesigDS"] = dtEditing;
                gv_Desig.DataSource = dtEditing;

                gv_Desig.DataBind();
            }

            catch (Exception ex)
            {

                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;

            }

        }



        // ROW CANCEL EDIT
        protected void gv_Desig_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataTable dt = new DataTable();

            gv_Desig.EditIndex = -1;

            dt = (DataTable)Session["DesigDS"];

            gv_Desig.DataSource = dt;

            gv_Desig.DataBind();

        }



        // FOR GRIDVIEW ROW UPDATING
        protected void gv_Desig_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataTable dt_update = new DataTable();

                //Label lblDealerCode = (Label)gv_city.Rows[e.RowIndex].FindControl("lblDealerCode");
                Label lblDesigCode = (Label)gv_Desig.Rows[e.RowIndex].FindControl("lblDesigCode");
                TextBox txtDesigDesc = (TextBox)gv_Desig.Rows[e.RowIndex].FindControl("txtDesigDesc");     // GridView TextBox Fiel                     // GridView TextBox Field

                // Check value in GridView TextBOX
                if (txtDesigDesc.Text.Trim() == string.Empty)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Designation Description can not be null or empty.";
                    return;
                }


                if (lblDesigCode.Text == "")
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Designation Code can not be null";
                    return;
                }

                dt_update = (DataTable)Session["DesigDS"];

                //Uzair
                //DataRow dr =  ds_update.Tables[0].Rows[0];
                // Check in GridView

                foreach (DataRow dr in dt_update.Rows)
                {
                    if (dr["DesigCode"].ToString() == lblDesigCode.Text)
                    {
                        dr["DesigDesc"] = txtDesigDesc.Text.Trim();


                    }
                }

                gv_Desig.EditIndex = -1;

                gv_Desig.DataSource = dt_update;
                gv_Desig.DataBind();

                Session["DesigDS"] = dt_update;

                // Updating in DataBase
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {

                    string sql = "update Designation set DesigDesc = '" + txtDesigDesc.Text + "' WHERE DealerCode = '" + Session["DealerCode"].ToString() + "' and DesigCode='" + lblDesigCode.Text + "'  ";
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
        protected void gv_Desig_SelectedIndexChanged(object sender, GridViewPageEventArgs e)
        {
            gv_Desig.PageIndex = e.NewPageIndex;
            createGrid();
        }
    }
}