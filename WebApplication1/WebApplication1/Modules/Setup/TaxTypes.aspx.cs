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
using CConn;
using System.Threading;
using Microsoft.ApplicationBlocks.Data;
using System.IO;

namespace DXBMS.Modules.Setup
{
    public partial class TaxTypes : System.Web.UI.Page
    {
        SysFunction sysFunc = new SysFunction();
        SysFunctions sysFuncs = new SysFunctions();
        DataSet ds;
        Transaction ObjTrans = new Transaction();
        clsLookUp clslook = new clsLookUp();
        DataTable dt = new DataTable();
        SqlTransaction Trans;
        DropDownList ddlAccCode;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                createGrid();
                initializeDDLs(ddlAccCode);

            }
            if (Page.IsPostBack)
            {

                if (!string.IsNullOrEmpty(Session["LookUpData"].ToString()))
                {
                    SelectedPartDetail(Session["LookUpData"].ToString());
                }
            }
            Session["LookUpData"] = string.Empty;

        }
        private void createGrid()
        {
            string query = "Select TaxID, TaxHead, TaxPer, TaxApplicable ,Type,AccCode,Active  from TaxType where dealercode = '" + "COMON" + "' ";

            DataTable dt = new DataTable();

            dt = sysFunc.GetData(query);

            if (dt.Rows.Count > 0)
            {
                gv_TaxType.DataSource = dt;
                gv_TaxType.DataBind();
            }

            Session["TaxTypeDS"] = dt;

        }


        private void initializeDDLs(DropDownList ddl)
        {
            try
            {
                string CCon = CConnection.GetConnStringForAccount();

                SqlDataReader dr = SqlHelper.ExecuteReader(CCon, CommandType.Text, "Select A.contacccode +'-'+  A.SubCode +'-'+  A.subsubcode +'-'+  A.loccode +'-'+  A.DetailCode as AccountCode , A.contacccode +'-'+   A.SubCode +'-'+ A.subsubcode +'-'+ A.loccode +'-'+  A.DetailCode + ' | ' + rtrim(A.DetailDesc) as DetailDesc from GDetail  A where CompCode = '" + Session["DealerCode"].ToString() + "'");

                if (dr.HasRows)
                {
                    ListItem item = new ListItem();
                    item.Text = "Select";
                    item.Value = "0";

                    //AddInAllDDL(item);

                    ddl.Items.Add(item);
                    while (dr.Read())
                    {
                        StringWriter myWriter = new StringWriter();
                        HttpUtility.HtmlDecode(dr["DetailDesc"].ToString().Replace(" ", "&nbsp;"), myWriter);//ddlEmp.Items.Add(myWriter.ToString());
                        item = new ListItem();
                        item.Text = myWriter.ToString();
                        item.Value = dr["AccountCode"].ToString();
                        ddl.Items.Add(item);

                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
               // lblMessage.Text = ex.Message;
            }

        }


        protected void imgTaxType_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 75;
            clslook.LU_Get_AccountCode(imgTaxType, ViewState["lookupid"].ToString(), "CompCode = '" + Session["DealerCode"].ToString() + "'", "../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }



        public void SelectedPartDetail(string item)
        {
            try
            {
                txtAccCode.Text = item;
                TxtAccDesc.Text = GetAccounttitle(item);


            }
            catch (Exception ex)
            {
                sysFunc.UserMsg(lblMessage, Color.Red, ex.Message);
            }
        }
        private string GetAccounttitle(string code)
        {
            try
            {
                string CCon = CConnection.GetConnStringForAccount();
                string value = "";

                SqlDataAdapter dta = new SqlDataAdapter("Select rtrim(A.DetailDesc) as AccountTitle from GDetail  A where A.CompCode = '" + Session["DealerCode"].ToString() + "' and  A.contacccode +'-'+  A.SubCode +'-'+  A.subsubcode +'-'+  A.loccode +'-'+  A.DetailCode = '" + code + "'", CCon);

                DataTable dt = new DataTable();
                dta.Fill(dt);

                if (dt.Rows.Count > 0 && dt != null)
                {
                    value = dt.Rows[0]["AccountTitle"].ToString();
                }


                return value;
            }
            catch (Exception ex)
            {
                sysFunc.UserMsg(lblMessage, Color.Red, ex.Message);
                return "";
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";

        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            if (txtTaxHead.Text == "")
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Tax Head should not left blank", txtTaxHead);
                return;
            }
            //if (txt.Text == "")
            //{
            //    sysFuncs.UserMsg(lblMessage, Color.Red, "Customer should not left blank", txtreg);
            //    return;
            //}
            //if (txtRemarks.Text == "")
            //{
            //    sysFuncs.UserMsg(lblMessage, Color.Red, "Remarks should not left blank", txtreg);
            //    return;
            //}


            DataTable dt = (DataTable)Session["TaxTypeDS"];

            try
            {
                string max = sysFuncs.GetNewMaxID("TaxType", "TaxID", 2, "COMON");
                SqlParameter[] param =
               {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                new SqlParameter("@TaxID",SqlDbType.Char,2), //1
                new SqlParameter("@TaxHead",SqlDbType.VarChar,50),//2
                new SqlParameter("@TaxPer",SqlDbType.Decimal),//3
                new SqlParameter("@Active",SqlDbType.Char,1),//4
                new SqlParameter("@Type",SqlDbType.VarChar,50),//5
                new SqlParameter("@TaxApplicable",SqlDbType.VarChar,20),//6
                new SqlParameter("@AccCode",SqlDbType.Char,16),//7
                new SqlParameter("@UpdUser",SqlDbType.VarChar,50), //8
                new SqlParameter("@UpdDate",SqlDbType.DateTime), //9
                new SqlParameter("@UpdTime",SqlDbType.DateTime), //10
                new SqlParameter("@UpdTerm",SqlDbType.VarChar,50),// 11                             
            };
                string activeCheck = "";
                if (CheckId.Checked)
                {
                    activeCheck = "Y";
                }
                else
                {
                    activeCheck = "N";
                }

                
                //  txtAccCode.Text = textAccCode;

                param[0].Value = "COMON";
                param[1].Value = max;
                param[2].Value = txtTaxHead.Text;
                param[3].Value = SysFunctions.CustomCDBL(txtTaxPercent.Text);
                param[4].Value = activeCheck;
                param[5].Value = ddlType.Text;
                param[6].Value = ddlTaxApplicable.Text;
                param[7].Value = txtAccCode.Text;
                param[8].Value = Session["UserName"].ToString();
                param[9].Value = sysFunc.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"));
                param[10].Value = DateTime.Now;
                param[11].Value = "1";


                if (sysFunc.ExecuteSP_NonQuery("SP_Insert_TaxType", param))
                {
                    sysFunc.ClearTextBoxes(Page);
                    createGrid();
                    sysFuncs.UserMsg(lblMessage, Color.Green, "Record " + max + " Insert Sucessfully", txtTaxId);

                }
                else
                {
                    sysFuncs.UserMsg(lblMessage, Color.Green, "Data Not Inserted", txtTaxId);
                }
            }
            catch (Exception ex)
            {
                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);
            }


        }
        protected void gv_Desig_SelectedIndexChanged(object sender, GridViewPageEventArgs e)
        {
            gv_TaxType.PageIndex = e.NewPageIndex;
            createGrid();
        }
        protected void gv_TaxType_DataBound1(object sender, EventArgs e)
        {

            try
            {

            }
            catch (Exception ex)
            {
                //lblMessage.Visible = true;
                // lblMessage.Text = ex.Message;
            }

        }

        protected void gv_TaxType_RowDataBound(object sender, GridViewRowEventArgs e)
        {

           
            if (e.Row.RowType == DataControlRowType.DataRow && (gv_TaxType.EditIndex == e.Row.RowIndex))
            {
                DropDownList ddlAccCode = (DropDownList)e.Row.FindControl("ddlAccCode");
                initializeDDLs(ddlAccCode);


               
                TextBox txtEditPer = (TextBox)e.Row.FindControl("txtEditPer");
                TextBox txtTaaxHead = (TextBox)e.Row.FindControl("txtTaaxHead");
                DropDownList ddlEditType = (DropDownList)e.Row.FindControl("ddlEditType");
                DropDownList ddlEditTaxApplicable = (DropDownList)e.Row.FindControl("ddlEditTaxApplicable");
             
                if (Session["lblAccountCode"].ToString() != "" && Session["lblAccountCode"].ToString() != null)
                {
                    ddlAccCode.SelectedValue = Session["lblAccountCode"].ToString();
                }
                txtTaaxHead.Text = Session["lblTaxHead"].ToString();
                txtEditPer.Text = Session["lblTaxPercent"].ToString();
                if (Session["lblTaxType"].ToString() != "" && Session["lblTaxType"].ToString() != null)
                {
                    ddlEditType.SelectedValue = Session["lblTaxType"].ToString();
                }
                if (Session["lblTaxApplicable"].ToString() != "" && Session["lblTaxApplicable"].ToString() != null)
                {
                    ddlTaxApplicable.SelectedValue = Session["lblTaxApplicable"].ToString();
                }
            }
            //try
            //{
            //    if (e.Row.RowType == DataControlRowType.DataRow && !(gv_TaxType.EditIndex == e.Row.RowIndex))
            //    {
            //        e.Row.Cells[0].Enabled = true;
            //    }

            //    if (e.Row.RowType == DataControlRowType.DataRow && (gv_TaxType.EditIndex == e.Row.RowIndex))
            //    {
            //        TextBox txtTaaxHead = (TextBox)e.Row.FindControl("txtTaaxHead");

            //        txtTaaxHead.Text = Session["lblTaxHead"].ToString();

            //        DropDownList ddlEditType = (DropDownList)e.Row.FindControl("ddlEditType");
            //        ddlEditType.Text = Session["lblTaxType"].ToString();

            //        DropDownList ddlEditTaxApplicable = (DropDownList)e.Row.FindControl("ddlEditTaxApplicable");
            //        ddlEditTaxApplicable.Text = Session["lblTaxApplicable"].ToString();

            //        TextBox txtEditPer = (TextBox)e.Row.FindControl("txtEditPer");

            //        txtEditPer.Text = Session["lblTaxPercent"].ToString();

            //    }

            //    if (e.Row.RowType == DataControlRowType.Footer)
            //    {

            //    }
            //}
            //catch (Exception ex)
            //{
            //    lblMessage.Visible = true;
            //    lblMessage.Text = ex.Message;
            //}
        }

        protected void gv_TaxType_RowEditing(object sender, GridViewEditEventArgs e)
        {

            try
            {
                DataTable dtEditing = new DataTable();

                Label lblTaxHead = (Label)gv_TaxType.Rows[e.NewEditIndex].FindControl("lblTaxHead");
                Label lblTaxType = (Label)gv_TaxType.Rows[e.NewEditIndex].FindControl("lblTaxType");
                Label lblTaxApplicable = (Label)gv_TaxType.Rows[e.NewEditIndex].FindControl("lblTaxApplicable");
                Label lblTaxPercent = (Label)gv_TaxType.Rows[e.NewEditIndex].FindControl("lblTaxPercent");
                Label lblAccountCode = (Label)gv_TaxType.Rows[e.NewEditIndex].FindControl("lblAccCode");
                
               
                Session["lblTaxHead"] = lblTaxHead.Text;
                Session["lblTaxType"] = lblTaxType.Text;
                Session["lblTaxApplicable"] = lblTaxApplicable.Text;
                Session["lblTaxPercent"] = lblTaxPercent.Text;
                Session["lblAccountCode"] = lblAccountCode.Text;

                

                gv_TaxType.EditIndex = e.NewEditIndex;

                dtEditing = (DataTable)Session["TaxTypeDS"];
                //Session["UnitCategoryDS"] = dtEditing;
                gv_TaxType.DataSource = dtEditing;

                gv_TaxType.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;

            }
        }

        protected void gv_TaxType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            DataTable dt = new DataTable();

            gv_TaxType.EditIndex = -1;

            dt = (DataTable)Session["TaxTypeDS"];

            gv_TaxType.DataSource = dt;

            gv_TaxType.DataBind();
        }

        protected void gv_TaxType_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataTable dt_update = new DataTable();
             
                Label lblTaxID = (Label)gv_TaxType.Rows[e.RowIndex].FindControl("lblTaxID");
                TextBox txtTaaxHead = (TextBox)gv_TaxType.Rows[e.RowIndex].FindControl("txtTaaxHead");     // GridView TextBox Field
                Label lblTaxType = (Label)gv_TaxType.Rows[e.RowIndex].FindControl("lblTaxType");
                DropDownList ddlEditType = (DropDownList)gv_TaxType.Rows[e.RowIndex].FindControl("ddlEditType");
                Label lblTaxApplicable = (Label)gv_TaxType.Rows[e.RowIndex].FindControl("lblTaxApplicable");
                DropDownList ddlEditTaxApplicable = (DropDownList)gv_TaxType.Rows[e.RowIndex].FindControl("ddlEditTaxApplicable");
                Label lblTaxPercent = (Label)gv_TaxType.Rows[e.RowIndex].FindControl("lblTaxPercent");
                TextBox txtEditPer = (TextBox)gv_TaxType.Rows[e.RowIndex].FindControl("txtEditPer");
                Label lblAccountCode = (Label)gv_TaxType.Rows[e.RowIndex].FindControl("lblAccountCode");
                DropDownList ddlAccCode = (DropDownList)gv_TaxType.Rows[e.RowIndex].FindControl("ddlAccCode");
               
                // Check value in GridView TextBOX
                if (txtTaaxHead.Text.Trim() == string.Empty)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Tax Head can not null or empty.";
                    return;
                }

                if (ddlEditType.Text.Trim() == string.Empty)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Tax Type can not null or empty.";
                    return;
                }
                if (ddlEditTaxApplicable.Text.Trim() == string.Empty)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Tax Applicable can not null or empty.";
                    return;
                }

                if (txtEditPer.Text.Trim() == string.Empty)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Tax Percent can not null or empty.";
                    return;
                }

                dt_update = (DataTable)Session["TaxTypeDS"];

                //Uzair
                //DataRow dr =  ds_update.Tables[0].Rows[0];
                // Check in GridView

                foreach (DataRow dr in dt_update.Rows)
                {
                    if (dr["TaxID"].ToString() == lblTaxID.Text)
                    {
                        dr["TaxHead"] = txtTaaxHead.Text.Trim();
                        dr["Type"] = ddlEditType.Text.Trim();
                        dr["TaxApplicable"] = ddlEditTaxApplicable.Text.Trim();
                        dr["TaxPer"] = txtEditPer.Text.Trim();
                        dr["AccCode"] = ddlAccCode.SelectedValue;

                    }
                }

                gv_TaxType.EditIndex = -1;

                gv_TaxType.DataSource = dt_update;
                gv_TaxType.DataBind();

                Session["TaxTypeDS"] = dt_update;

                string sql = "update TaxType set TaxHead = '" + txtTaaxHead.Text + "',Type='" + ddlEditType.SelectedValue + "',TaxApplicable='" + ddlEditTaxApplicable.SelectedValue + "',TaxPer='" + txtEditPer.Text + "',AccCode='"+ddlAccCode.SelectedValue+"'  WHERE TaxID='" + lblTaxID.Text + "'  ";

                if (sysFunc.ExecuteQuery_NonQuery(sql))
                {

                    sysFunc.UserMsg(lblMessage, Color.Green, "Sucessfully Update");

                }

            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }
        }

        protected void BtnRemove_TaxType_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsdelete = new DataSet();

            DataTable dt = new DataTable();

            try
            {
                dt = (DataTable)Session["TaxTypeDS"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex + gv_TaxType.PageIndex * gv_TaxType.PageSize;

                String code = dt.Rows[rowGvIndex]["TaxID"].ToString();
                if (sysFunc.CodeExists("PaymentReceiptTaxDetail", "TaxID", code, Session["DealerCode"].ToString(), ref dsdelete))
                {
                    lblMessage.Text = "Cannot delete , this Tax ID is already in use";
                    return;
                }

                if (sysFunc.CodeExists("OutgoingPaymentTaxDetail", "ReceiptHead", code, Session["DealerCode"].ToString(), ref dsdelete))
                {
                    lblMessage.Text = "Cannot delete , this Tax ID is already in use";
                    return;
                }

                dt.Rows[rowGvIndex].Delete();
                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                gv_TaxType.DataSource = dt;
                gv_TaxType.DataBind();

                Session["TaxTypeDS"] = dt;
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    //and CountryDesc = '" + txtStateDescription.Text + "' and VersionCode = '" + dsdelete.Tables[0].Rows[rowGvIndex]["Version"].ToString() + "'

                    string sql = "delete from TaxType where TaxID = '" + code.Trim() + "' ";
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
    }
}