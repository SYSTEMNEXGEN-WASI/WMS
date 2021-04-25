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


namespace DXBMS.Modules.Setup
{
    public partial class ItemCategory : System.Web.UI.Page
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
                createGrid();
            }
        }


        protected void btnInsert_Click(object sender, EventArgs e)
        {
            if (txtItemCatDesc.Text == "")
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please Enter Item Category Description", txtItemCatDesc);
                return;
            }
            else
            {
                lblMessage.Text = string.Empty;
            }

            try
            {
                string max = sysFuncs.GetNewMaxID("ItemCategory", "ItemCatCode", 4, Session["DealerCode"].ToString());
                SqlParameter[] param =
               {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                new SqlParameter("@ItemCatCode",SqlDbType.Char,4), //1
                new SqlParameter("@ItemCatDesc",SqlDbType.VarChar,50),//2
                new SqlParameter("@UpdUser",SqlDbType.VarChar,50), //3
                new SqlParameter("@UpdTerm",SqlDbType.VarChar,50),//4                              
            };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = max;
                //param[1].Value = txtItemCatCode.Text.Trim();
                param[2].Value = txtItemCatDesc.Text.Trim();
                param[3].Value = Session["UserName"].ToString();
                param[4].Value = "1";

                sysFunc.ExecuteSP_NonQuery("SP_InsertItemCategory", param);
                sysFunc.ClearTextBoxes(Page);

                createGrid();
                sysFuncs.UserMsg(lblMessage, Color.Green, "Data Inserted", txtItemCatDesc);
                //lblMessage.Text = "New Item Category Inserted.";
            }
            catch (Exception ex)
            {

                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);
            }
        }

        private void createGrid()
        {
            String query = "select * from ItemCategory";

            DataTable dt = new DataTable();

            dt = sysFunc.GetData(query);

            if (dt.Rows.Count > 0)
            {
                gv_ItemCat.DataSource = dt;
                gv_ItemCat.DataBind();
            }

            Session["ItemCategoryDS"] = dt;
        }

        // Remove Button Code
        protected void BtnRemove_ItemCategory_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsdelete = new DataSet();

            DataTable dt = new DataTable();

            try
            {
                dt = (DataTable)Session["ItemCategoryDS"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex + gv_ItemCat.PageIndex * gv_ItemCat.PageSize;

                String code = dt.Rows[rowGvIndex]["ItemCatCode"].ToString();

                if (sysFunc.CodeExists("Item", "ItemCatCode", code, Session["DealerCode"].ToString(), ref dsdelete))
                {
                    lblMessage.Text = "Cannot delete , this Item Category Code is already in use";
                    return;
                }

                dt.Rows[rowGvIndex].Delete();
                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                gv_ItemCat.DataSource = dt;
                gv_ItemCat.DataBind();

                Session["ItemCategoryDS"] = dt;
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    //and CountryDesc = '" + txtStateDescription.Text + "' and VersionCode = '" + dsdelete.Tables[0].Rows[rowGvIndex]["Version"].ToString() + "'

                    string sql = "delete from ItemCategory where ItemCatCode = '" + code.Trim() + "' ";
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




        protected void gv_ItemCat_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && !(gv_ItemCat.EditIndex == e.Row.RowIndex))
                {
                    e.Row.Cells[0].Enabled = true;
                }

                if (e.Row.RowType == DataControlRowType.DataRow && (gv_ItemCat.EditIndex == e.Row.RowIndex))
                {
                    //btn_remove.Enabled = false;

                    // DropDownList ddlbrand_code = (DropDownList)e.Row.FindControl("ddlbrand_code");
                    TextBox txtItemCatDesc = (TextBox)e.Row.FindControl("txtItemCatDescription");
                    //TextBox txtStateCode = (TextBox)e.Row.FindControl("txtStateCode");
                    //TextBox txtCountryDesc = (TextBox)e.Row.FindControl("txtStateDescription");

                    txtItemCatDesc.Text = Session["lblItemCatDesc"].ToString();
                    //txtStateCode.Text = Session["lblStateCode"].ToString();
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

        //// DATABOUND FUNCTION
        protected void gv_ItemCat_DataBound(object sender, EventArgs e)
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


        //// FOR GRIDVIEW ROW EDITING
        protected void gv_ItemCat_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                DataTable dtEditing = new DataTable();

                Label lblItemCatDesc = (Label)gv_ItemCat.Rows[e.NewEditIndex].FindControl("lblItemCatDesc");
                //Label lblStateCode = (Label)gv_city.Rows[e.NewEditIndex].FindControl("lblStateCode");

                Session["lblItemCatDesc"] = lblItemCatDesc.Text;
                //Session["lblStateCode"] = lblStateCode.Text;

                gv_ItemCat.EditIndex = e.NewEditIndex;

                dtEditing = (DataTable)Session["ItemCategoryDS"];
                Session["ItemCategoryDS"] = dtEditing;
                gv_ItemCat.DataSource = dtEditing;

                gv_ItemCat.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;

            }

        }



        // ROW CANCEL EDIT
        protected void gv_ItemCat_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataTable dt = new DataTable();

            gv_ItemCat.EditIndex = -1;

            dt = (DataTable)Session["ItemCategoryDS"];

            gv_ItemCat.DataSource = dt;

            gv_ItemCat.DataBind();

        }



        //// FOR GRIDVIEW ROW UPDATING
        protected void gv_ItemCat_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataTable dt_update = new DataTable();

                //Label lblDealerCode = (Label)gv_city.Rows[e.RowIndex].FindControl("lblDealerCode");
                Label lblItemCatCode = (Label)gv_ItemCat.Rows[e.RowIndex].FindControl("lblItemCatCode");
                TextBox txtItemCatDescription = (TextBox)gv_ItemCat.Rows[e.RowIndex].FindControl("txtItemCatDescription");     // GridView TextBox Field


                // Check value in GridView TextBOX
                if (txtItemCatDescription.Text.Trim() == string.Empty)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Category Code can not null or empty.";
                    return;
                }


                dt_update = (DataTable)Session["ItemCategoryDS"];

                //Uzair
                //DataRow dr =  ds_update.Tables[0].Rows[0];
                // Check in GridView

                foreach (DataRow dr in dt_update.Rows)
                {
                    if (dr["ItemCatCode"].ToString() == lblItemCatCode.Text)
                    {
                        dr["ItemCatDesc"] = txtItemCatDescription.Text.Trim();
                        //dr["StateCode"] = txtStateCode.Text.Trim();                    
                    }
                }

                gv_ItemCat.EditIndex = -1;

                gv_ItemCat.DataSource = dt_update;
                gv_ItemCat.DataBind();

                Session["ItemCategoryDS"] = dt_update;

                // Updating in DataBase
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {

                    string sql = "update ItemCategory set ItemCatDesc = '" + txtItemCatDescription.Text + "' WHERE ItemCatCode='" + lblItemCatCode.Text + "'  ";
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


        protected void gv_ItemCat_SelectedIndexChanged(object sender, GridViewPageEventArgs e)
        {
            gv_ItemCat.PageIndex = e.NewPageIndex;
            createGrid();
        }
    }
}