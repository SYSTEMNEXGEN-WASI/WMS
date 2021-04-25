using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;


namespace DXBMS.Modules.CustomerExperience
{
    public partial class NDM : System.Web.UI.Page
    {

        //MainBLL ObjMainBLL = new MainBLL();
        Transaction ObjTrans = new Transaction();
        //General ObjGenral = new General();
        //clsLookUp ObjLookUp = new clsLookUp();
        DataTable grdVehDT = new DataTable();
        //MainBLL objMBLL = new MainBLL();
        SqlTransaction Trans;

        SysFunction sysFunc = new SysFunction();
        SysFunctions myFunc = new SysFunctions();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["DealerCode"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!IsPostBack)
            {
                createLubDT();
            }
        }


        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {

            PopulateGrid();
           
        }

        private void PopulateGrid()
        {
            SqlParameter[] param = {
                                  new SqlParameter("@DealerCode",SqlDbType.Char),
                                  new SqlParameter("@FromDate",SqlDbType.DateTime),
                                                                    
                               };
            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = Calendar1.SelectedDate.ToString("yyyy-MM-dd");

            DataSet ds = new DataSet();
            ds = myFunc.FillDataSet("SP_NDM_LetterSendDate", param);
            if(ds.Tables[0].Rows.Count > 0)
            {
                grdNextDueMaintenance.DataSource = ds;
                grdNextDueMaintenance.DataBind();
                Session["NextDueMaintenance"] = ds;
            }           

            ds.Dispose();

            ds = myFunc.FillDataSet("SP_NDM_CallDate", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCallSend.DataSource = ds;
                gvCallSend.DataBind();
                Session["CallDate"] = ds;
            }
            

            ds.Dispose();

            ds = myFunc.FillDataSet("SP_NDM_SMSDate", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvSMSSend.DataSource = ds;
                gvSMSSend.DataBind();
                Session["SMSDate"] = ds;
            }
            

        }

        protected void gv_PSF_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            try
            {
                DataSet ds_update = new DataSet();

                TextBox txtLetterSentDate = (TextBox)grdNextDueMaintenance.Rows[e.RowIndex].FindControl("txtLetterSentDate");
                TextBox txtCallDate = (TextBox)grdNextDueMaintenance.Rows[e.RowIndex].FindControl("txtCallDate");
                TextBox txtSmsDate = (TextBox)grdNextDueMaintenance.Rows[e.RowIndex].FindControl("txtSmsDate");
                TextBox txtRemarks = (TextBox)grdNextDueMaintenance.Rows[e.RowIndex].FindControl("txtRemarks");
                

                Label JobCode = (Label)grdNextDueMaintenance.Rows[e.RowIndex].FindControl("lblJobCode");


                ds_update = (DataSet)Session["NextDueMaintenance"];

                //Uzair
                foreach (DataRow dr in ds_update.Tables[0].Rows)
                {
                    if (dr["JobCardCode"].ToString().Trim() == JobCode.Text.Trim())
                    {
                        dr["LetterSentDate"] = sysFunc.SaveDate(txtLetterSentDate.Text);

                        if (txtCallDate.Text == "")
                        {
                            dr["CalledDate10Day"] = null;
                        }
                        else
                        {
                            dr["CalledDate10Day"] = sysFunc.SaveDate(txtCallDate.Text);
                        }
                        if (txtSmsDate.Text == "")
                        {
                            dr["SMSSendDate"] = null;
                        }
                        else
                        {
                            dr["SMSSendDate"] = sysFunc.SaveDate(txtSmsDate.Text);
                        }
                        dr["Remarks2Day"] = txtRemarks.Text.Trim();
                    }

                }

                grdNextDueMaintenance.EditIndex = -1;

                grdNextDueMaintenance.DataSource = ds_update;
                grdNextDueMaintenance.DataBind();
                
                SqlParameter[] param = { 
                                                  new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                  new SqlParameter("@JObCardCode",SqlDbType.Char,8),
                                                  new SqlParameter("@LetterSentDate",SqlDbType.DateTime),
                                                  new SqlParameter("@CalledDate10Day",SqlDbType.DateTime),
                                                  new SqlParameter("@SMSSendDate",SqlDbType.DateTime),
                                                  new SqlParameter("@Remarks2Day",SqlDbType.VarChar,50)
                                              };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = JobCode.Text;
                if (txtLetterSentDate.Text == "")
                        {
                           param[2].Value = null;
                        }
                        else
                        {
                            param[2].Value = sysFunc.SaveDate(txtLetterSentDate.Text);
                        }
                if (txtCallDate.Text == "")
                        {
                            param[3].Value = null;
                        }
                        else
                        {
                            param[3].Value = sysFunc.SaveDate(txtCallDate.Text);
                        }
                if (txtSmsDate.Text == "")
                        {
                            param[4].Value = null;
                        }
                        else
                        {
                            param[4].Value = sysFunc.SaveDate(txtSmsDate.Text);
                        }

                param[5].Value = txtRemarks.Text.Trim();

                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    myFunc.ExecuteSP_NonQuery("[sp_SP_CRM_PostSales_NextDueMaintenance_Update]", param, Trans);
                    //string sql = "update CRM_PostSales_NextDueMaintenance set LetterSentDate = '" + sysFunc.SaveDate(txtLetterSentDate.Text) + "', Remarks2Day = '" + txtRemarks.Text + "' , CalledDate10Day ='" + sysFunc.SaveDate(txtCallDate.Text) + "'" +
                    //    ", SMSSendDate = '" + sysFunc.SaveDate(txtSmsDate.Text) + "' where DealerCode = '" + Session["DealerCode"].ToString() + "' and JobCardCode = '" + JobCode.Text + "' ";
                    //myFunc.ExecuteQuery(sql, Trans);
                }

                lblMessage.Visible = true;
                lblMessage.ForeColor = Color.Green;
                lblMessage.Text = "Record "+ JobCode.Text +" Updated";

                Session["NextDueMaintenance"] = ds_update;
                ObjTrans.CommittTransaction(ref Trans);
            }
            catch (Exception ex)
            {
                //ObjTrans.CommittTransaction(ref Trans);
                ObjTrans.RollBackTransaction(ref Trans);
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }

        }

        protected void gv_PSF_DataBound(object sender, EventArgs e)
        {
            try
            {
                //LoadGRN_DDL();
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }
        }

        protected void gv_PSF_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataSet ds = new DataSet();

            grdNextDueMaintenance.EditIndex = -1;

            ds = (DataSet)Session["NextDueMaintenance"];

            grdNextDueMaintenance.DataSource = ds.Tables[0];

            grdNextDueMaintenance.DataBind();

        }

        protected void gv_PSF_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                //ViewState["NextDueMaintenance"]
                Label lblLetrDate = (Label)grdNextDueMaintenance.Rows[e.NewEditIndex].FindControl("lblLetrDate");
                Label lblCallDate = (Label)grdNextDueMaintenance.Rows[e.NewEditIndex].FindControl("lblCallDate");
                Label lblSMSSendDate = (Label)grdNextDueMaintenance.Rows[e.NewEditIndex].FindControl("lblSMSSendDate");
                Label lblRemarks = (Label)grdNextDueMaintenance.Rows[e.NewEditIndex].FindControl("lblRemarks");

                Session["lblLetrDate"] = lblLetrDate.Text;
                Session["lblCallDate"] = lblCallDate.Text;
                Session["lblSMSSendDate"] = lblSMSSendDate.Text;
                Session["lblRemarks"] = lblRemarks.Text;

                grdNextDueMaintenance.EditIndex = e.NewEditIndex;

                // ds = ViewState["NextDueMaintenance"];

                ds = (DataSet)Session["NextDueMaintenance"];

                grdNextDueMaintenance.DataSource = ds.Tables[0];

                grdNextDueMaintenance.DataBind();
            }

            catch (Exception ex)
            {

                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;

            }

        }

        protected void gv_PSF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                ImageButton btn_remove = (ImageButton)e.Row.FindControl("BtnRemove_PSF");
                Label JobCode = (Label)e.Row.FindControl("lblJobCode");

                if (e.Row.RowType == DataControlRowType.DataRow && !(grdNextDueMaintenance.EditIndex == e.Row.RowIndex) && JobCode.Text == string.Empty)
                {

                    e.Row.Cells[0].Enabled = false;
                }


                if (e.Row.RowType == DataControlRowType.DataRow && (grdNextDueMaintenance.EditIndex == e.Row.RowIndex))
                {
                    TextBox txtLetterSentDate = (TextBox)e.Row.FindControl("txtLetterSentDate");
                    TextBox txtCallDate = (TextBox)e.Row.FindControl("txtCallDate");
                    TextBox txtSmsDate = (TextBox)e.Row.FindControl("txtSmsDate");
                    TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");


                    txtLetterSentDate.Text = Session["lblLetrDate"].ToString().Trim();
                    txtCallDate.Text = Session["lblCallDate"].ToString();
                    txtSmsDate.Text = Session["lblSMSSendDate"].ToString();
                    txtRemarks.Text = Session["lblRemarks"].ToString();

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

        protected void gvCallSend_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            try
            {
                DataSet ds_update = new DataSet();

                TextBox txtLetterSentDate = (TextBox)gvCallSend.Rows[e.RowIndex].FindControl("txtLetterSentDate");
                TextBox txtCallDate = (TextBox)gvCallSend.Rows[e.RowIndex].FindControl("txtCallDate");
                TextBox txtSmsDate = (TextBox)gvCallSend.Rows[e.RowIndex].FindControl("txtSmsDate");
                TextBox txtRemarks = (TextBox)gvCallSend.Rows[e.RowIndex].FindControl("txtRemarks");


                Label JobCode = (Label)gvCallSend.Rows[e.RowIndex].FindControl("lblJobCode");


                ds_update = (DataSet)Session["CallDate"];

                //Uzair
                foreach (DataRow dr in ds_update.Tables[0].Rows)
                {
                    if (dr["JobCardCode"].ToString().Trim() == JobCode.Text.Trim())
                    {
                        dr["LetterSentDate"] = sysFunc.SaveDate(txtLetterSentDate.Text);

                        if (txtCallDate.Text == "")
                        {
                            dr["CalledDate10Day"] = null;
                        }
                        else
                        {
                            dr["CalledDate10Day"] = sysFunc.SaveDate(txtCallDate.Text);
                        }
                        if (txtSmsDate.Text == "")
                        {
                            dr["SMSSendDate"] = null;
                        }
                        else
                        {
                            dr["SMSSendDate"] = sysFunc.SaveDate(txtSmsDate.Text);
                        }
                        dr["Remarks2Day"] = txtRemarks.Text.Trim();
                    }

                }

                gvCallSend.EditIndex = -1;

                gvCallSend.DataSource = ds_update;
                gvCallSend.DataBind();

                SqlParameter[] param = { 
                                                  new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                  new SqlParameter("@JObCardCode",SqlDbType.Char,8),
                                                  new SqlParameter("@LetterSentDate",SqlDbType.DateTime),
                                                  new SqlParameter("@CalledDate10Day",SqlDbType.DateTime),
                                                  new SqlParameter("@SMSSendDate",SqlDbType.DateTime),
                                                  new SqlParameter("@Remarks2Day",SqlDbType.VarChar,50)
                                              };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = JobCode.Text;
                if (txtLetterSentDate.Text == "")
                {
                    param[2].Value = null;
                }
                else
                {
                    param[2].Value = sysFunc.SaveDate(txtLetterSentDate.Text);
                }
                if (txtCallDate.Text == "")
                {
                    param[3].Value = null;
                }
                else
                {
                    param[3].Value = sysFunc.SaveDate(txtCallDate.Text);
                }
                if (txtSmsDate.Text == "")
                {
                    param[4].Value = null;
                }
                else
                {
                    param[4].Value = sysFunc.SaveDate(txtSmsDate.Text);
                }

                param[5].Value = txtRemarks.Text.Trim();

                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    myFunc.ExecuteSP_NonQuery("[sp_SP_CRM_PostSales_NextDueMaintenance_Update]", param, Trans);
                    //string sql = "update CRM_PostSales_NextDueMaintenance set LetterSentDate = '" + sysFunc.SaveDate(txtLetterSentDate.Text) + "', Remarks2Day = '" + txtRemarks.Text + "' , CalledDate10Day ='" + sysFunc.SaveDate(txtCallDate.Text) + "'" +
                    //    ", SMSSendDate = '" + sysFunc.SaveDate(txtSmsDate.Text) + "' where DealerCode = '" + Session["DealerCode"].ToString() + "' and JobCardCode = '" + JobCode.Text + "' ";
                    //myFunc.ExecuteQuery(sql, Trans);
                }

                lblMessage.Visible = true;
                lblMessage.ForeColor = Color.Green;
                lblMessage.Text = "Record " + JobCode.Text + " Updated";

                Session["CallDate"] = ds_update;
                ObjTrans.CommittTransaction(ref Trans);
            }
            catch (Exception ex)
            {
                //ObjTrans.CommittTransaction(ref Trans);
                ObjTrans.RollBackTransaction(ref Trans);
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }

        }

        protected void gvCallSend_DataBound(object sender, EventArgs e)
        {
            try
            {
                //LoadGRN_DDL();
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }
        }

        protected void gvCallSend_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataSet ds = new DataSet();

            gvCallSend.EditIndex = -1;

            ds = (DataSet)Session["CallDate"];

            gvCallSend.DataSource = ds.Tables[0];

            gvCallSend.DataBind();

        }

        protected void gvCallSend_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                //ViewState["NextDueMaintenance"]
                Label lblLetrDate = (Label)gvCallSend.Rows[e.NewEditIndex].FindControl("lblLetrDate");
                Label lblCallDate = (Label)gvCallSend.Rows[e.NewEditIndex].FindControl("lblCallDate");
                Label lblSMSSendDate = (Label)gvCallSend.Rows[e.NewEditIndex].FindControl("lblSMSSendDate");
                Label lblRemarks = (Label)gvCallSend.Rows[e.NewEditIndex].FindControl("lblRemarks");

                Session["lblLetrDate"] = lblLetrDate.Text;
                Session["lblCallDate"] = lblCallDate.Text;
                Session["lblSMSSendDate"] = lblSMSSendDate.Text;
                Session["lblRemarks"] = lblRemarks.Text;

                gvCallSend.EditIndex = e.NewEditIndex;

                // ds = ViewState["NextDueMaintenance"];

                ds = (DataSet)Session["CallDate"];

                gvCallSend.DataSource = ds.Tables[0];

                gvCallSend.DataBind();
            }

            catch (Exception ex)
            {

                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;

            }

        }

        protected void gvCallSend_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                ImageButton btn_remove = (ImageButton)e.Row.FindControl("BtnRemove_PSF");
                Label JobCode = (Label)e.Row.FindControl("lblJobCode");

                if (e.Row.RowType == DataControlRowType.DataRow && !(gvCallSend.EditIndex == e.Row.RowIndex) && JobCode.Text == string.Empty)
                {

                    e.Row.Cells[0].Enabled = false;
                }


                if (e.Row.RowType == DataControlRowType.DataRow && (gvCallSend.EditIndex == e.Row.RowIndex))
                {
                    TextBox txtLetterSentDate = (TextBox)e.Row.FindControl("txtLetterSentDate");
                    TextBox txtCallDate = (TextBox)e.Row.FindControl("txtCallDate");
                    TextBox txtSmsDate = (TextBox)e.Row.FindControl("txtSmsDate");
                    TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");


                    txtLetterSentDate.Text = Session["lblLetrDate"].ToString().Trim();
                    txtCallDate.Text = Session["lblCallDate"].ToString();
                    txtSmsDate.Text = Session["lblSMSSendDate"].ToString();
                    txtRemarks.Text = Session["lblRemarks"].ToString();

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

        protected void gvSMSSend_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            try
            {
                DataSet ds_update = new DataSet();

                TextBox txtLetterSentDate = (TextBox)gvSMSSend.Rows[e.RowIndex].FindControl("txtLetterSentDate");
                TextBox txtCallDate = (TextBox)gvSMSSend.Rows[e.RowIndex].FindControl("txtCallDate");
                TextBox txtSmsDate = (TextBox)gvSMSSend.Rows[e.RowIndex].FindControl("txtSmsDate");
                TextBox txtRemarks = (TextBox)gvSMSSend.Rows[e.RowIndex].FindControl("txtRemarks");


                Label JobCode = (Label)gvSMSSend.Rows[e.RowIndex].FindControl("lblJobCode");


                ds_update = (DataSet)Session["SMSDate"];

                //Uzair
                foreach (DataRow dr in ds_update.Tables[0].Rows)
                {
                    if (dr["JobCardCode"].ToString().Trim() == JobCode.Text.Trim())
                    {
                        dr["LetterSentDate"] = sysFunc.SaveDate(txtLetterSentDate.Text);

                        if (txtCallDate.Text == "")
                        {
                            dr["CalledDate10Day"] = null;
                        }
                        else
                        {
                            dr["CalledDate10Day"] = sysFunc.SaveDate(txtCallDate.Text);
                        }
                        if (txtSmsDate.Text == "")
                        {
                            dr["SMSSendDate"] = null;
                        }
                        else
                        {
                            dr["SMSSendDate"] = sysFunc.SaveDate(txtSmsDate.Text);
                        }
                        dr["Remarks2Day"] = txtRemarks.Text.Trim();
                    }

                }

                gvSMSSend.EditIndex = -1;

                gvSMSSend.DataSource = ds_update;
                gvSMSSend.DataBind();

                SqlParameter[] param = { 
                                                  new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                  new SqlParameter("@JObCardCode",SqlDbType.Char,8),
                                                  new SqlParameter("@LetterSentDate",SqlDbType.DateTime),
                                                  new SqlParameter("@CalledDate10Day",SqlDbType.DateTime),
                                                  new SqlParameter("@SMSSendDate",SqlDbType.DateTime),
                                                  new SqlParameter("@Remarks2Day",SqlDbType.VarChar,50)
                                              };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = JobCode.Text;
                if (txtLetterSentDate.Text == "")
                {
                    param[2].Value = null;
                }
                else
                {
                    param[2].Value = sysFunc.SaveDate(txtLetterSentDate.Text);
                }
                if (txtCallDate.Text == "")
                {
                    param[3].Value = null;
                }
                else
                {
                    param[3].Value = sysFunc.SaveDate(txtCallDate.Text);
                }
                if (txtSmsDate.Text == "")
                {
                    param[4].Value = null;
                }
                else
                {
                    param[4].Value = sysFunc.SaveDate(txtSmsDate.Text);
                }

                param[5].Value = txtRemarks.Text.Trim();

                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    myFunc.ExecuteSP_NonQuery("[sp_SP_CRM_PostSales_NextDueMaintenance_Update]", param, Trans);
                    //string sql = "update CRM_PostSales_NextDueMaintenance set LetterSentDate = '" + sysFunc.SaveDate(txtLetterSentDate.Text) + "', Remarks2Day = '" + txtRemarks.Text + "' , CalledDate10Day ='" + sysFunc.SaveDate(txtCallDate.Text) + "'" +
                    //    ", SMSSendDate = '" + sysFunc.SaveDate(txtSmsDate.Text) + "' where DealerCode = '" + Session["DealerCode"].ToString() + "' and JobCardCode = '" + JobCode.Text + "' ";
                    //myFunc.ExecuteQuery(sql, Trans);
                }

                lblMessage.Visible = true;
                lblMessage.ForeColor = Color.Green;
                lblMessage.Text = "Record " + JobCode.Text + " Updated";

                Session["SMSDate"] = ds_update;
                ObjTrans.CommittTransaction(ref Trans);
            }
            catch (Exception ex)
            {
                //ObjTrans.CommittTransaction(ref Trans);
                ObjTrans.RollBackTransaction(ref Trans);
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }

        }

        protected void gvSMSSend_DataBound(object sender, EventArgs e)
        {
            try
            {
                //LoadGRN_DDL();
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }
        }

        protected void gvSMSSend_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataSet ds = new DataSet();

            gvSMSSend.EditIndex = -1;

            ds = (DataSet)Session["SMSDate"];

            gvSMSSend.DataSource = ds.Tables[0];

            gvSMSSend.DataBind();

        }

        protected void gvSMSSend_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                //ViewState["NextDueMaintenance"]
                Label lblLetrDate = (Label)gvSMSSend.Rows[e.NewEditIndex].FindControl("lblLetrDate");
                Label lblCallDate = (Label)gvSMSSend.Rows[e.NewEditIndex].FindControl("lblCallDate");
                Label lblSMSSendDate = (Label)gvSMSSend.Rows[e.NewEditIndex].FindControl("lblSMSSendDate");
                Label lblRemarks = (Label)gvSMSSend.Rows[e.NewEditIndex].FindControl("lblRemarks");

                Session["lblLetrDate"] = lblLetrDate.Text;
                Session["lblCallDate"] = lblCallDate.Text;
                Session["lblSMSSendDate"] = lblSMSSendDate.Text;
                Session["lblRemarks"] = lblRemarks.Text;

                gvSMSSend.EditIndex = e.NewEditIndex;

                // ds = ViewState["NextDueMaintenance"];

                ds = (DataSet)Session["SMSDate"];

                gvSMSSend.DataSource = ds.Tables[0];

                gvSMSSend.DataBind();
            }

            catch (Exception ex)
            {

                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;

            }

        }

        protected void gvSMSSend_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                ImageButton btn_remove = (ImageButton)e.Row.FindControl("BtnRemove_PSF");
                Label JobCode = (Label)e.Row.FindControl("lblJobCode");

                if (e.Row.RowType == DataControlRowType.DataRow && !(gvSMSSend.EditIndex == e.Row.RowIndex) && JobCode.Text == string.Empty)
                {

                    e.Row.Cells[0].Enabled = false;
                }


                if (e.Row.RowType == DataControlRowType.DataRow && (gvSMSSend.EditIndex == e.Row.RowIndex))
                {
                    TextBox txtLetterSentDate = (TextBox)e.Row.FindControl("txtLetterSentDate");
                    TextBox txtCallDate = (TextBox)e.Row.FindControl("txtCallDate");
                    TextBox txtSmsDate = (TextBox)e.Row.FindControl("txtSmsDate");
                    TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");


                    txtLetterSentDate.Text = Session["lblLetrDate"].ToString().Trim();
                    txtCallDate.Text = Session["lblCallDate"].ToString();
                    txtSmsDate.Text = Session["lblSMSSendDate"].ToString();
                    txtRemarks.Text = Session["lblRemarks"].ToString();

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


        private void createLubDT()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            SysFunction sysfun = new SysFunction();

            string sql = "exec SP_CRM_PostSales_NextDueMaintenance_Select '" + Session["DealerCode"].ToString() + "'";

            ds = myFunc.GetData(sql, "CRM_PostSales_NextDueMaintenance");
            

            if (ds.Tables[0].Rows.Count > 0)
            {
                grdNextDueMaintenance.DataSource = ds.Tables[0]; grdNextDueMaintenance.DataBind();
            }

            Session["NextDueMaintenance"] = ds;
        }
    }
}