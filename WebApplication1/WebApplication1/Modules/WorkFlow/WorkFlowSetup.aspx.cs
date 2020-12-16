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
    public partial class WorkFlowSetup : System.Web.UI.Page
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
            if (txtWorkFlowDesc.Text == "")
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please Enter WorkFlow Description", txtWorkFlowDesc);
                return;
            }
            else
            {
                lblMessage.Text = string.Empty;
            }

            try
            {

                string max = sysFuncs.GetNewMaxID("WorkFlowSetup", "WFCode", 5, Session["DealerCode"].ToString());


                // string max = sysFuncs.AutoGen("WorkFlowSetup", "WFCode", Session["DealerCode"].ToString(), DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));

                SqlParameter[] param =
               {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                new SqlParameter("@WFCode",SqlDbType.Char,8), //1
                new SqlParameter("@WFDesc",SqlDbType.VarChar,50),//2
                new SqlParameter("@UpdUser",SqlDbType.VarChar,50), //3
                new SqlParameter("@UpdTerm",SqlDbType.VarChar,50),//4                              
            };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = max;
                param[2].Value = txtWorkFlowDesc.Text.Trim();
                param[3].Value = Session["UserName"].ToString();
                param[4].Value = "1";

                sysFunc.ExecuteSP_NonQuery("SP_insertWorkFlowSetup", param);
                sysFunc.ClearTextBoxes(Page);

                createGrid();
                txtWorkFlowID.Text = max;
                sysFuncs.UserMsg(lblMessage, Color.Green, "Data Inserted", txtWorkFlowDesc);

            }
            catch (Exception ex)
            {

                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);
            }
        }

        private void createGrid()
        {
            String query = "select * from WorkFlowSetup";

            DataTable dt = new DataTable();

            dt = sysFunc.GetData(query);

            if (dt.Rows.Count > 0)
            {
                gv_WorkFlow.DataSource = dt;
                gv_WorkFlow.DataBind();
            }


            Session["WorkFlowDS"] = dt;
        }


        // Remove Button Code
        protected void BtnRemove_WorkFlowSetup_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsdelete = new DataSet();

            DataTable dt = new DataTable();

            try
            {
                dt = (DataTable)Session["WorkFlowDS"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex + gv_WorkFlow.PageIndex * gv_WorkFlow.PageSize;

                String code = dt.Rows[rowGvIndex]["WFCode"].ToString();

                //if (sysFunc.CodeExists("WorkFlowSetup", "WFCode", code, Session["DealerCode"].ToString(), ref dsdelete))
                //{
                //    lblMessage.Text = "Cannot delete , this WorkFlow Code is already in use";
                //    return;
                //}

                dt.Rows[rowGvIndex].Delete();
                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                gv_WorkFlow.DataSource = dt;
                gv_WorkFlow.DataBind();

                Session["WorkFlowDS"] = dt;
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    //and CountryDesc = '" + txtStateDescription.Text + "' and VersionCode = '" + dsdelete.Tables[0].Rows[rowGvIndex]["Version"].ToString() + "'

                    string sql = "delete from WorkFlowSetup where WFCode = '" + code.Trim() + "' ";
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


        // DATABOUND FUNCTION
        protected void gv_WorkFlow_DataBound(object sender, EventArgs e)
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
        protected void gv_WorkFlow_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && !(gv_WorkFlow.EditIndex == e.Row.RowIndex))
                {
                    e.Row.Cells[0].Enabled = true;
                }

                if (e.Row.RowType == DataControlRowType.DataRow && (gv_WorkFlow.EditIndex == e.Row.RowIndex))
                {
                    //btn_remove.Enabled = false;

                    // DropDownList ddlbrand_code = (DropDownList)e.Row.FindControl("ddlbrand_code");
                    TextBox txtWorkFlowDescEdit = (TextBox)e.Row.FindControl("txtWorkFlowDescEdit");
                    //TextBox txtStateCode = (TextBox)e.Row.FindControl("txtStateCode");
                    //TextBox txtCountryDesc = (TextBox)e.Row.FindControl("txtStateDescription");

                    txtWorkFlowDescEdit.Text = Session["lblWorkFlowDesc"].ToString();
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
        protected void gv_WorkFlow_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                DataTable dtEditing = new DataTable();

                Label lblWorkFlowDesc = (Label)gv_WorkFlow.Rows[e.NewEditIndex].FindControl("lblWorkFlowDesc");

                //Label lblStateCode = (Label)gv_city.Rows[e.NewEditIndex].FindControl("lblStateCode");

                Session["lblWorkFlowDesc"] = lblWorkFlowDesc.Text;

                //Session["lblStateCode"] = lblStateCode.Text;

                gv_WorkFlow.EditIndex = e.NewEditIndex;

                dtEditing = (DataTable)Session["WorkFlowDS"];
                Session["WorkFlowDS"] = dtEditing;
                gv_WorkFlow.DataSource = dtEditing;

                gv_WorkFlow.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;

            }

        }


        // ROW CANCEL EDIT
        protected void gv_WorkFlow_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataTable dt = new DataTable();

            gv_WorkFlow.EditIndex = -1;

            dt = (DataTable)Session["WorkFlowDS"];

            gv_WorkFlow.DataSource = dt;

            gv_WorkFlow.DataBind();

        }

        // FOR GRIDVIEW ROW UPDATING
        protected void gv_WorkFlow_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataTable dt_update = new DataTable();

                //Label lblDealerCode = (Label)gv_city.Rows[e.RowIndex].FindControl("lblDealerCode");
                Label lblWorkFlowCode = (Label)gv_WorkFlow.Rows[e.RowIndex].FindControl("lblWorkFlowCode");
                TextBox txtWorkFlowDescEdit = (TextBox)gv_WorkFlow.Rows[e.RowIndex].FindControl("txtWorkFlowDescEdit");     // GridView TextBox Field


                // Check value in GridView TextBOX
                if (txtWorkFlowDescEdit.Text.Trim() == string.Empty)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Description can not null or empty.";
                    return;
                }


                dt_update = (DataTable)Session["WorkFlowDS"];

                //Uzair
                //DataRow dr =  ds_update.Tables[0].Rows[0];
                // Check in GridView

                foreach (DataRow dr in dt_update.Rows)
                {
                    if (dr["WFCode"].ToString() == lblWorkFlowCode.Text)
                    {
                        dr["WFDesc"] = txtWorkFlowDescEdit.Text.Trim();

                    }
                }

                gv_WorkFlow.EditIndex = -1;

                gv_WorkFlow.DataSource = dt_update;
                gv_WorkFlow.DataBind();

                Session["WFRollsDS"] = dt_update;

                // Updating in DataBase
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {

                    string sql = "update WorkFlowSetup set WFDesc = '" + txtWorkFlowDescEdit.Text + "'  WHERE WFCode='" + lblWorkFlowCode.Text + "'  ";
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


        protected void gv_WorkFlow_SelectedIndexChanged(object sender, GridViewPageEventArgs e)
        {
            gv_WorkFlow.PageIndex = e.NewPageIndex;
            createGrid();
        }

    }
}