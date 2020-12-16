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
    public partial class JobCategory : System.Web.UI.Page
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
            }
        }
        private void createGrid()
        {
            String query = "select * from jobcategory where DealerCode = '" + Session["DealerCode"].ToString() + "'";
            DataTable dt = new DataTable();
            dt = sysFunc.GetData(query);
            if (dt.Rows.Count > 0)
            {
                gv_JobCat.DataSource = dt;
                gv_JobCat.DataBind();
            }
            Session["JobCatDS"] = dt;
        }
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                string max = sysFunc.GetNewMaxID("jobcategory", "JobCatCode", 3, Session["DealerCode"].ToString());

                SqlParameter[] param =
                {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                new SqlParameter("@JobCatCode",SqlDbType.Char,3),
                new SqlParameter("@JobCatDesc",SqlDbType.VarChar,50),
                new SqlParameter("@UpdUser",SqlDbType.VarChar,50), //3
                new SqlParameter("@UpdTerm",SqlDbType.VarChar,50)//4
            };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = max;
                param[2].Value = txtJobCatDesc.Text.Trim();
                param[3].Value = Session["UserName"].ToString();
                param[4].Value = "1";
                if (txtJobCatDesc.Text != "")
                {
                    sysFunc.ExecuteSP_NonQuery("SP_InsertJobCat", param);
                    sysFunc.ClearTextBoxes(Page);

                    createGrid();
                    lblMessage.ForeColor = Color.Green;
                    lblMessage.Text = "New Job Category " + param[2].Value + " Inserted.";
                }

                else
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Job Category Description can not be null or empty";
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
                dt = (DataTable)Session["JobCatDS"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex + gv_JobCat.PageIndex * gv_JobCat.PageSize;

                String code = dt.Rows[rowGvIndex]["JobCatCode"].ToString();
                String Desc = dt.Rows[rowGvIndex]["JobCatDesc"].ToString();

                if (sysFunc.CodeExists("defaultjob", "JobCatCode", code, Session["DealerCode"].ToString(), ref dsdelete))
                {
                    lblMessage.Text = "Record Cannot be deleted , this Job Category is already in use";
                    return;
                }

                dt.Rows[rowGvIndex].Delete();
                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                gv_JobCat.DataSource = dt;
                gv_JobCat.DataBind();

                Session["JobCatDS"] = dt;
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    //and CountryDesc = '" + txtStateDescription.Text + "' and VersionCode = '" + dsdelete.Tables[0].Rows[rowGvIndex]["Version"].ToString() + "'

                    string sql = "delete from JobCategory where  DealerCode = '" + Session["DealerCode"].ToString() + "' and JobCatCode = '" + code.Trim() + "' ";
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

        protected void gv_JobCat_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {



                if (e.Row.RowType == DataControlRowType.DataRow && !(gv_JobCat.EditIndex == e.Row.RowIndex))
                {

                    e.Row.Cells[0].Enabled = true;
                }

                if (e.Row.RowType == DataControlRowType.DataRow && (gv_JobCat.EditIndex == e.Row.RowIndex))
                {
                    //btn_remove.Enabled = false;

                    // DropDownList ddlbrand_code = (DropDownList)e.Row.FindControl("ddlbrand_code");
                    TextBox txtJobCatCode = (TextBox)e.Row.FindControl("txtJobCatCode");
                    TextBox txtJobCatDesc = (TextBox)e.Row.FindControl("txtJobCatDesc");
                    //TextBox txtCountryDesc = (TextBox)e.Row.FindControl("txtStateDescription");

                    txtJobCatCode.Text = Session["txtJobCatCode"].ToString();
                    txtJobCatDesc.Text = Session["txtJobCatDesc"].ToString();
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
        protected void gv_JobCat_DataBound(object sender, EventArgs e)
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
        protected void gv_JobCat_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                DataTable dtEditing = new DataTable();

                Label lblJobCatCode = (Label)gv_JobCat.Rows[e.NewEditIndex].FindControl("lblJobCatCode");
                Label lblJobCatDesc = (Label)gv_JobCat.Rows[e.NewEditIndex].FindControl("lblJobCatDesc");

                Session["lblJobCatCode"] = lblJobCatCode.Text;
                Session["lblJobCatDesc"] = lblJobCatDesc.Text;

                gv_JobCat.EditIndex = e.NewEditIndex;

                dtEditing = (DataTable)Session["JobCatDS"];
                Session["JobCatDS"] = dtEditing;
                gv_JobCat.DataSource = dtEditing;

                gv_JobCat.DataBind();
            }

            catch (Exception ex)
            {

                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;

            }

        }



        // ROW CANCEL EDIT
        protected void gv_JobCat_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataTable dt = new DataTable();

            gv_JobCat.EditIndex = -1;

            dt = (DataTable)Session["JobCatDS"];

            gv_JobCat.DataSource = dt;

            gv_JobCat.DataBind();

        }



        // FOR GRIDVIEW ROW UPDATING
        protected void gv_JobCat_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataTable dt_update = new DataTable();

                //Label lblDealerCode = (Label)gv_city.Rows[e.RowIndex].FindControl("lblDealerCode");
                Label lblJobCatCode = (Label)gv_JobCat.Rows[e.RowIndex].FindControl("lblJobCatCode");
                TextBox txtJobCatDesc = (TextBox)gv_JobCat.Rows[e.RowIndex].FindControl("txtJobCatDesc");     // GridView TextBox Fiel                     // GridView TextBox Field

                // Check value in GridView TextBOX
                if (txtJobCatDesc.Text.Trim() == string.Empty)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "JobCat Description can not be null or empty.";
                    return;
                }


                if (lblJobCatCode.Text == "")
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "JobCat Code can not be null";
                    return;
                }

                dt_update = (DataTable)Session["JobCatDS"];

                //Uzair
                //DataRow dr =  ds_update.Tables[0].Rows[0];
                // Check in GridView

                foreach (DataRow dr in dt_update.Rows)
                {
                    if (dr["JobCatCode"].ToString() == lblJobCatCode.Text)
                    {
                        dr["JobCatDesc"] = txtJobCatDesc.Text.Trim();


                    }
                }

                gv_JobCat.EditIndex = -1;

                gv_JobCat.DataSource = dt_update;
                gv_JobCat.DataBind();

                Session["JobCatDS"] = dt_update;

                // Updating in DataBase
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {

                    string sql = "update JobCategory set JobCatDesc = '" + txtJobCatDesc.Text + "' WHERE DealerCode = '" + Session["DealerCode"].ToString() + "' and JobCatCode='" + lblJobCatCode.Text + "'  ";
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
        protected void gv_JobCat_SelectedIndexChanged(object sender, GridViewPageEventArgs e)
        {
            gv_JobCat.PageIndex = e.NewPageIndex;
            createGrid();
        }

        protected void gv_JobCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gr = gv_JobCat.SelectedRow;
                Label lbl = gr.FindControl("lblJobCatCode") as Label;
                txtJobCatCode.Text = lbl.Text;
                Label lbl1 = gr.FindControl("lblJobCatDesc") as Label;
                txtJobCatDesc.Text = lbl1.Text;

            }
            catch (Exception ex) { throw ex; }

        }
    }
}