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
    public partial class BankMaster : System.Web.UI.Page
    {

        SysFunction sysFunc = new SysFunction();
        DataSet ds;
        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["DealerCode"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!IsPostBack)
            {
                createGrid();
                // LoadGRN_DDL();
            }
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {

            try
            {
                SqlParameter[] param =
           {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                new SqlParameter("@BankCode",SqlDbType.Char,5), //1
                new SqlParameter("@BankDesc",SqlDbType.VarChar,50),//2
                new SqlParameter("@UpdUser",SqlDbType.VarChar,50), //3
                new SqlParameter("@UpdTerm",SqlDbType.VarChar,50),//4
                new SqlParameter("@BankStatus",SqlDbType.VarChar,25),//5
                //new SqlParameter("@SuzukiFinance",SqlDbType.Bit,1)//6  
                
            };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = txtBankCode.Text;
                param[2].Value = txtBankDesc.Text.Trim();
                param[3].Value = Session["UserName"].ToString();
                param[4].Value = Environment.MachineName;
                // param[5].Value = ddState.SelectedValue.ToString().Trim();
                param[5].Value = ddlBankStatus.SelectedValue.ToString().Trim(); ;

                sysFunc.ExecuteSP_NonQuery("SP_InsertBank", param);
                sysFunc.ClearTextBoxes(Page);

                createGrid();
                lblMessage.Text = "New BANK Data Inserted.";
            }
            catch (Exception ex)
            {

                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);
            }


        }

        private void createGrid()
        {

            String query = "select * from Bank where DealerCode in ('"+Session["DealerCode"].ToString()+"','COMON')";

            DataTable dt = new DataTable();

            dt = sysFunc.GetData(query);

            if (dt.Rows.Count > 0)
            {
                gv_bank.DataSource = dt;
                gv_bank.DataBind();
            }

            Session["BankDS"] = dt;

        }


        protected void gv_bank_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow && !(gv_bank.EditIndex == e.Row.RowIndex))
                {

                    e.Row.Cells[0].Enabled = true;
                }

                if (e.Row.RowType == DataControlRowType.DataRow && (gv_bank.EditIndex == e.Row.RowIndex))
                {
                    //btn_remove.Enabled = false;

                    // DropDownList ddlbrand_code = (DropDownList)e.Row.FindControl("ddlbrand_code");
                    TextBox txtBankCode = (TextBox)e.Row.FindControl("txtBankCode");
                    TextBox txtBankDesc = (TextBox)e.Row.FindControl("txtBankDesc");
                    //TextBox txtBankStatus= (TextBox)e.Row.FindControl("txtBankStatus");
                    DropDownList ddlBankStatus = (DropDownList)e.Row.FindControl("ddlBankStatus");
                    //TextBox txtCountryDesc = (TextBox)e.Row.FindControl("txtStateDescription");

                    txtBankCode.Text = Session["lblBankCode"].ToString();
                    txtBankDesc.Text = Session["lblBankDesc"].ToString();
                    ddlBankStatus.Text = Session["lblBankStatus"].ToString();
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



        // Remove Button Code
        protected void BtnRemove_Bank_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsdelete = new DataSet();

            DataTable dt = new DataTable();

            try
            {
                dt = (DataTable)Session["BankDS"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;

                String code = dt.Rows[rowGvIndex]["BankCode"].ToString();

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

                gv_bank.DataSource = dt;
                gv_bank.DataBind();

                Session["BankDS"] = dt;
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    //and CountryDesc = '" + txtStateDescription.Text + "' and VersionCode = '" + dsdelete.Tables[0].Rows[rowGvIndex]["Version"].ToString() + "'

                    string sql = "delete from Bank where DealerCode in ('" + Session["DealerCode"].ToString() + "','AAAAA') and BankCode = '" + code.Trim() + "' ";
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
        protected void gv_bank_DataBound(object sender, EventArgs e)
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
        protected void gv_bank_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                //DataSet ds = new DataSet();

                DataTable dtEditing = new DataTable();

                //Label lblDealerCode = (Label)gv_city.Rows[e.NewEditIndex].FindControl("lblDealerCode");            
                // Label lblCityCode = (Label)gv_city.Rows[e.NewEditIndex].FindControl("lblCityCode");                 
                Label lblBankDesc = (Label)gv_bank.Rows[e.NewEditIndex].FindControl("lblBankDesc");
                // Label lblUpdUser = (Label)gv_city.Rows[e.NewEditIndex].FindControl("lblUpdUser");
                //Label lblUpdTerm = (Label)gv_city.Rows[e.NewEditIndex].FindControl("lblUpdTerm");
                Label lblBankStatus = (Label)gv_bank.Rows[e.NewEditIndex].FindControl("lblBankStatus");

                //Session["lblDealerCode"] = lblDealerCode.Text;            
                // Session["lblCityCode"] = lblCityCode.Text;
                Session["lblBankDesc"] = lblBankDesc.Text;
                // Session["lblUpdUser"] = lblUpdUser.Text;
                // Session["lblUpdTerm"] = lblUpdTerm.Text;
                Session["lblBankStatus"] = lblBankStatus.Text;

                gv_bank.EditIndex = e.NewEditIndex;

                dtEditing = (DataTable)Session["BankDS"];
                Session["BankDS"] = dtEditing;
                gv_bank.DataSource = dtEditing;

                gv_bank.DataBind();
            }

            catch (Exception ex)
            {

                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;

            }

        }


        // ROW CANCEL EDIT
        protected void gv_bank_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataTable dt = new DataTable();

            gv_bank.EditIndex = -1;

            dt = (DataTable)Session["BankDS"];

            gv_bank.DataSource = dt;

            gv_bank.DataBind();

        }


        // FOR GRIDVIEW ROW UPDATING
        protected void gv_bank_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataTable dt_update = new DataTable();

                //Label lblDealerCode = (Label)gv_city.Rows[e.RowIndex].FindControl("lblDealerCode");
                Label lblBankCode = (Label)gv_bank.Rows[e.RowIndex].FindControl("lblBankCode");
                TextBox txtBankDesc = (TextBox)gv_bank.Rows[e.RowIndex].FindControl("txtBankDesc");     // GridView TextBox Field

                //Label lblUpdUser = (Label)gv_city.Rows[e.RowIndex].FindControl("lblUpdUser");
                //Label lblUpdTerm = (Label)gv_city.Rows[e.RowIndex].FindControl("lblUpdTerm");
                TextBox txtBankStatus = (TextBox)gv_bank.Rows[e.RowIndex].FindControl("txtBankStatus");                     // GridView TextBox Field

                // Check value in GridView TextBOX
                if (txtBankDesc.Text.Trim() == string.Empty)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Bank Description can not null or empty.";
                    return;
                }


                if (txtBankStatus.Text == "")
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Bank Status can not be null";
                    return;
                }

                dt_update = (DataTable)Session["BankDS"];

                //Uzair
                //DataRow dr =  ds_update.Tables[0].Rows[0];
                // Check in GridView

                foreach (DataRow dr in dt_update.Rows)
                {
                    if (dr["BankCode"].ToString() == lblBankCode.Text)
                    {
                        dr["BankDesc"] = txtBankDesc.Text.Trim();
                        //dr["StateCode"] = txtStateCode.Text.Trim();
                        dr["BankStatus"] = txtBankStatus.Text.Trim();

                    }
                }

                gv_bank.EditIndex = -1;

                gv_bank.DataSource = dt_update;
                gv_bank.DataBind();

                Session["BankDS"] = dt_update;

                // Updating in DataBase
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {

                    string sql = "update Bank set BankDesc = '" + txtBankDesc.Text + "', BankStatus = '" + txtBankStatus.Text + "' WHERE DealerCode in ('" + Session["DealerCode"].ToString() + "','AAAAA') and BankCode='" + lblBankCode.Text + "'  ";
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

        protected void gv_bank_SelectedIndexChanged(object sender, GridViewPageEventArgs e)
        {
            gv_bank.PageIndex = e.NewPageIndex;
            createGrid();
        }
    }
}