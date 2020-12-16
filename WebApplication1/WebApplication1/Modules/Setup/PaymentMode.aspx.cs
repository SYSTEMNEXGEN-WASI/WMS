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
    public partial class PaymentMode : System.Web.UI.Page
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
                createGrid();
        }

        //Btn Insert
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] param =
           {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                new SqlParameter("@PayModeCode",SqlDbType.Char,1), //1
                new SqlParameter("@PayModeDesc",SqlDbType.VarChar,50),//2
                new SqlParameter("@UpdUser",SqlDbType.VarChar,50), //3
                new SqlParameter("@UpdTerm",SqlDbType.VarChar,50),//4
                new SqlParameter("@Version",SqlDbType.Char,8),//5  
                new SqlParameter("@PayModeCodePSMC",SqlDbType.VarChar,50)//6
            };

                param[0].Value = "AAAAA";
                param[1].Value = txtPayModeCode.Text;
                param[2].Value = txtPayModeDesc.Text.Trim();
                param[3].Value = Session["UserName"].ToString();
                param[4].Value = "1";
                param[5].Value = txtVersion.Text;
                param[6].Value = txtPayModeCodePSMC.Text;


                sysFunc.ExecuteSP_NonQuery("SP_InsertPayMode", param);
                sysFunc.ClearTextBoxes(Page);

                createGrid();
                lblMessage.Text = "New PaymentMode Inserted.";
            }
            catch (Exception ex)
            {
                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);
            }
        }

        private void createGrid()
        {

            String query = "select * from PaymentMode";

            DataTable dt = new DataTable();

            dt = sysFunc.GetData(query);

            if (dt.Rows.Count > 0)
            {
                gv_paymode.DataSource = dt;
                gv_paymode.DataBind();
            }

            Session["PayModeDS"] = dt;

        }



        // BTN REMOVE
        protected void BtnRemove_PayMode_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsdelete = new DataSet();

            DataTable dt = new DataTable();

            try
            {
                dt = (DataTable)Session["PayModeDS"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;

                String code = dt.Rows[rowGvIndex]["PayModeCode"].ToString();

                if (sysFunc.CodeExists("PaymentReceiptMaster", "PayModeCode", code, Session["DealerCode"].ToString(), ref dsdelete))
                {
                    lblMessage.Text = "Cannot delete , this PaymentMode is already in use";
                    return;
                }
                if (sysFunc.CodeExists("OutgoingPaymentMaster", "PayModeCode", code, Session["DealerCode"].ToString(), ref dsdelete))
                {
                    lblMessage.Text = "Cannot delete , this PaymentMode is already in use";
                    return;
                }

                dt.Rows[rowGvIndex].Delete();
                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                gv_paymode.DataSource = dt;
                gv_paymode.DataBind();

                Session["PayModeDS"] = dt;
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    //and CountryDesc = '" + txtStateDescription.Text + "' and VersionCode = '" + dsdelete.Tables[0].Rows[rowGvIndex]["Version"].ToString() + "'

                    string sql = "delete from PaymentMode where PayModeCode = '" + code.Trim() + "' ";
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
        protected void gv_paymode_DataBound(object sender, EventArgs e)
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

        // ROW DATABOUND 
        protected void gv_paymode_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow && !(gv_paymode.EditIndex == e.Row.RowIndex))
                {

                    e.Row.Cells[0].Enabled = true;
                }

                if (e.Row.RowType == DataControlRowType.DataRow && (gv_paymode.EditIndex == e.Row.RowIndex))
                {
                    //btn_remove.Enabled = false;

                    TextBox txtPayModeDesc = (TextBox)e.Row.FindControl("txt_PayModeDesc");
                    //TextBox txtVersion = (TextBox)e.Row.FindControl("txtVersion");
                    //TextBox txtPayModeCodePSMC = (TextBox)e.Row.FindControl("txtPayModeCodePSMC");


                    txtPayModeDesc.Text = Session["lblPayModeDesc"].ToString();
                    //txtVersion.Text = Session["lblVersion"].ToString();
                    //txtPayModeCodePSMC.Text = Session["lblPayModeCodePSMC"].ToString();
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


        // FOR GRIDVIEW ROW EDITING
        protected void gv_paymode_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                //DataSet ds = new DataSet();

                DataTable dtEditing = new DataTable();


                Label lblPayModeDesc = (Label)gv_paymode.Rows[e.NewEditIndex].FindControl("lblPayModeDesc");
                // Label lblVersion = (Label)gv_paymode.Rows[e.NewEditIndex].FindControl("lblVersion");
                // Label lblPayModeCodePSMC = (Label)gv_paymode.Rows[e.NewEditIndex].FindControl("lblPayModeCodePSMC");


                Session["lblPayModeDesc"] = lblPayModeDesc.Text;
                //Session["lblVersion"] = lblVersion.Text;
                //Session["lblPayModeCodePSMC"] = lblPayModeCodePSMC.Text;


                gv_paymode.EditIndex = e.NewEditIndex;

                dtEditing = (DataTable)Session["PayModeDS"];
                Session["PayModeDS"] = dtEditing;
                gv_paymode.DataSource = dtEditing;

                gv_paymode.DataBind();
            }

            catch (Exception ex)
            {

                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;

            }

        }

        // ROW CANCEL EDIT
        protected void gv_paymode_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataTable dt = new DataTable();

            gv_paymode.EditIndex = -1;

            dt = (DataTable)Session["PayModeDS"];

            gv_paymode.DataSource = dt;

            gv_paymode.DataBind();

        }


        // FOR GRIDVIEW ROW UPDATING
        protected void gv_paymode_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataTable dt_update = new DataTable();

                //Label lblDealerCode = (Label)gv_city.Rows[e.RowIndex].FindControl("lblDealerCode");
                Label lblPayModeCode = (Label)gv_paymode.Rows[e.RowIndex].FindControl("lblPayModeCode");
                TextBox txtPayModeDesc = (TextBox)gv_paymode.Rows[e.RowIndex].FindControl("txt_PayModeDesc");     // GridView TextBox Field

                // Check value in GridView TextBOX
                if (txtPayModeDesc.Text.Trim() == string.Empty)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Pay Mode Desc can not null or empty.";
                    return;
                }

                dt_update = (DataTable)Session["PayModeDS"];

                //Uzair
                //DataRow dr =  ds_update.Tables[0].Rows[0];
                // Check in GridView

                foreach (DataRow dr in dt_update.Rows)
                {
                    if (dr["PayModeCode"].ToString() == lblPayModeCode.Text)
                    {
                        dr["PayModeDesc"] = txtPayModeDesc.Text.Trim();
                        dr["UpdTime"] = System.DateTime.Now.ToString();
                        //dr["PayModeCodePSMC"] = txtPayModeCodePSMC.Text.Trim();


                    }
                }

                gv_paymode.EditIndex = -1;

                gv_paymode.DataSource = dt_update;
                gv_paymode.DataBind();

                Session["PayModeDS"] = dt_update;

                // Updating in DataBase
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {

                    string sql = "update PaymentMode set PayModeDesc = '" + txtPayModeDesc.Text + "', UpdTime='" + System.DateTime.Now.ToString() + "' WHERE PayModeCode='" + lblPayModeCode.Text + "'  ";
                    SqlDataReader reader = null;
                    //sysFunc.ExecuteQuery(sql, Trans);
                    sysFunc.ExecuteQuery(sql, ref reader);
                }
                ObjTrans.CommittTransaction(ref Trans);
                lblMessage.Visible = true;
                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "Record Updated";
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }
        }


        protected void gv_paymode_SelectedIndexChanged(object sender, GridViewPageEventArgs e)
        {
            gv_paymode.PageIndex = e.NewPageIndex;
            createGrid();
        }
    }
}