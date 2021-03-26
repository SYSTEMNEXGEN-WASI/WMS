using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace DXBMS.Modules.Setup
{
    public partial class Brand : System.Web.UI.Page
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
            String query = "select * from brand where DealerCode in( '"+Session["DealerCode"].ToString()+"','COMON')";
            DataTable dt = new DataTable();
            dt = sysFunc.GetData(query);
            if (dt.Rows.Count > 0)
            {
                gv_Brand.DataSource = dt;
                gv_Brand.DataBind();
            }
            Session["BrandDS"] = dt;
        }
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                string max = sysFunc.GetNewMaxID("Brand", "BrandCode", 3, Session["DealerCode"].ToString());

                SqlParameter[] param =
                {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                new SqlParameter("@BrandCode",SqlDbType.Char,3),
                new SqlParameter("@BrandDesc",SqlDbType.VarChar,50),
                new SqlParameter("@UpdUser",SqlDbType.VarChar,50), //3
                new SqlParameter("@UpdTerm",SqlDbType.VarChar,50)//4
            };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = max;
                param[2].Value = txtBrandDesc.Text.Trim();
                param[3].Value = Session["UserName"].ToString();
                param[4].Value = "1";
                if (txtBrandDesc.Text != "")
                {
                    sysFunc.ExecuteSP_NonQuery("SP_InsertBrand", param);
                    sysFunc.ClearTextBoxes(Page);

                    createGrid();
                    lblMessage.ForeColor = Color.Green;
                    lblMessage.Text = "New Brand " + param[2].Value + " Inserted.";
                }

                else
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Brand Description can not be left empty";
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
                dt = (DataTable)Session["BrandDS"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex + gv_Brand.PageIndex * gv_Brand.PageSize;

                String code = dt.Rows[rowGvIndex]["BrandCode"].ToString();
                String Desc = dt.Rows[rowGvIndex]["BrandDesc"].ToString();

                if (sysFunc.CodeExists("Vehicle", "BrandCode", code, Session["DealerCode"].ToString(), ref dsdelete))
                {
                    lblMessage.Text = "Record Cannot be deleted , this Brand is already in use";
                    return;
                }
                if (sysFunc.CodeExists("Vehicletaxdetail", "BrandCode", code, Session["DealerCode"].ToString(), ref dsdelete))
                {
                    lblMessage.Text = "Record Cannot be deleted , this Brand is already in use";
                    return;
                }
                if (sysFunc.CodeExists("jobcardmaster", "BrandCode", code, Session["DealerCode"].ToString(), ref dsdelete))
                {
                    lblMessage.Text = "Record Cannot be deleted , this Brand is already in use";
                    return;
                }
                if (sysFunc.CodeExists("bookordvehdetail", "BrandCode", code, Session["DealerCode"].ToString(), ref dsdelete))
                {
                    lblMessage.Text = "Record Cannot be deleted , this Brand is already in use";
                    return;
                }
                if (sysFunc.CodeExists("customervehicle", "BrandCode", code, Session["DealerCode"].ToString(), ref dsdelete))
                {
                    lblMessage.Text = "Record Cannot be deleted , this Brand is already in use";
                    return;
                }

                dt.Rows[rowGvIndex].Delete();
                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                gv_Brand.DataSource = dt;
                gv_Brand.DataBind();

                Session["BrandDS"] = dt;
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    //and CountryDesc = '" + txtStateDescription.Text + "' and VersionCode = '" + dsdelete.Tables[0].Rows[rowGvIndex]["Version"].ToString() + "'

                    string sql = "delete from Brand where DealerCode = '" + Session["DealerCode"].ToString() + "' and BrandCode = '" + code.Trim() + "' ";
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

        protected void gv_Brand_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {



                if (e.Row.RowType == DataControlRowType.DataRow && !(gv_Brand.EditIndex == e.Row.RowIndex))
                {

                    e.Row.Cells[0].Enabled = true;
                }

                if (e.Row.RowType == DataControlRowType.DataRow && (gv_Brand.EditIndex == e.Row.RowIndex))
                {
                    //btn_remove.Enabled = false;

                    // DropDownList ddlbrand_code = (DropDownList)e.Row.FindControl("ddlbrand_code");
                    
                    TextBox txtBrandDesc = (TextBox)e.Row.FindControl("txtBrandDesc");
                    //TextBox txtCountryDesc = (TextBox)e.Row.FindControl("txtStateDescription");

                    //txtBrandCode.Text = Session["txtBrandCode"].ToString();
                    txtBrandDesc.Text = Session["lblBrandDesc"].ToString();
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
        protected void gv_Brand_DataBound(object sender, EventArgs e)
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
        protected void gv_Brand_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                DataTable dtEditing = new DataTable();

                Label lblBrandCode = (Label)gv_Brand.Rows[e.NewEditIndex].FindControl("lblBrandCode");
                Label lblBrandDesc = (Label)gv_Brand.Rows[e.NewEditIndex].FindControl("lblBrandDesc");

                Session["lblBrandCode"] = lblBrandCode.Text;
                Session["lblBrandDesc"] = lblBrandDesc.Text;

                gv_Brand.EditIndex = e.NewEditIndex;

                dtEditing = (DataTable)Session["BrandDS"];
                Session["BrandDS"] = dtEditing;
                gv_Brand.DataSource = dtEditing;

                gv_Brand.DataBind();
            }

            catch (Exception ex)
            {

                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;

            }

        }



        // ROW CANCEL EDIT
        protected void gv_Brand_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataTable dt = new DataTable();

            gv_Brand.EditIndex = -1;

            dt = (DataTable)Session["BrandDS"];

            gv_Brand.DataSource = dt;

            gv_Brand.DataBind();

        }



        // FOR GRIDVIEW ROW UPDATING
        protected void gv_Brand_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataTable dt_update = new DataTable();

                //Label lblDealerCode = (Label)gv_city.Rows[e.RowIndex].FindControl("lblDealerCode");
                Label lblBrandCode = (Label)gv_Brand.Rows[e.RowIndex].FindControl("lblBrandCode");
                TextBox txtBrandDesc = (TextBox)gv_Brand.Rows[e.RowIndex].FindControl("txtBrandDesc");     // GridView TextBox Fiel                     // GridView TextBox Field

                // Check value in GridView TextBOX
                if (txtBrandDesc.Text.Trim() == string.Empty)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Brand Description can not be left empty.";
                    return;
                }


                //if (lblBrandCode.Text == "")
                //{
                //    lblMessage.Visible = true;
                //    lblMessage.Text = "Brand Code can not be null";
                //    return;
                //}

                dt_update = (DataTable)Session["BrandDS"];

                //Uzair
                //DataRow dr =  ds_update.Tables[0].Rows[0];
                // Check in GridView

                foreach (DataRow dr in dt_update.Rows)
                {
                    if (dr["BrandCode"].ToString() == lblBrandCode.Text)
                    {
                        dr["BrandDesc"] = txtBrandDesc.Text.Trim();


                    }
                }

                gv_Brand.EditIndex = -1;

                gv_Brand.DataSource = dt_update;
                gv_Brand.DataBind();

                Session["BrandDS"] = dt_update;

                // Updating in DataBase
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {

                    string sql = "update Brand set BrandDesc = '" + txtBrandDesc.Text + "' WHERE DealerCode = '" + Session["DealerCode"].ToString() + "' and BrandCode='" + lblBrandCode.Text + "'  ";
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
        protected void gv_Brand_SelectedIndexChanged(object sender, GridViewPageEventArgs e)
        {
            gv_Brand.PageIndex = e.NewPageIndex;
            createGrid();
        }

        protected void gv_Brand_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gr = gv_Brand.SelectedRow;
                Label lbl = gr.FindControl("lblBrandCode") as Label;
                txtBrandCode.Text = lbl.Text;
                Label lbl1 = gr.FindControl("lblBrandDesc") as Label;
                txtBrandDesc.Text = lbl1.Text;

            }
            catch (Exception ex) { throw ex; }

        }
    }
}