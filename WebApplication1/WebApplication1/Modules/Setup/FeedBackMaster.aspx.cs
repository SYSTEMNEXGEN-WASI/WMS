using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DXBMS.Modules.Setup
{
    public partial class FeedBackMaster : System.Web.UI.Page
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

            String query = "select * from FeedBackQuestions where DealerCode = '" + Session["DealerCode"].ToString() + "'";

            DataTable dt = new DataTable();

            dt = sysFunc.GetData(query);

            gv_Feedback.DataSource = dt;
            gv_Feedback.DataBind();

            //if (dt.Rows.Count > 0)
            //{
            //    gv_Feedback.DataSource = dt;
            //    gv_Feedback.DataBind();
            //}

            Session["FeedbackDS"] = dt;

        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            if (ddlControlType.SelectedIndex == 0)
            {
                lblMessage.Text = "Select Control Type First";
                return;
            }
            if (ddlServiceType.SelectedIndex == 0)
            {
                lblMessage.Text = "Select Service Type First";
                return;
            }

            try
            {
                string max = sysFunc.GetNewMaxID("FeedBackQuestions", "ParentQuestionCode", 3, Session["DealerCode"].ToString());

                SqlParameter[] param =
               {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                new SqlParameter("@ParentQuestionCode",SqlDbType.Char,3), //1
                new SqlParameter("@ParentQuestionDesc",SqlDbType.VarChar,100),//2
                new SqlParameter("@ControlType",SqlDbType.VarChar,50),//2
                new SqlParameter("@ServiceType",SqlDbType.VarChar,50),
                new SqlParameter("@QSequence",SqlDbType.Int,3),
                new SqlParameter("@InActive",SqlDbType.VarChar,1),
                new SqlParameter("@UpdUser",SqlDbType.VarChar,50), //3
                new SqlParameter("@UpdTerm",SqlDbType.VarChar,50),//4
            };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = max;
                param[2].Value = txtParentQuestionDesc.Text.Trim();
                param[3].Value = ddlControlType.SelectedValue.ToString().Trim();
                param[4].Value = ddlServiceType.SelectedValue.ToString().Trim();
                param[5].Value = txtSequence.Text.Trim();

                if (check.Checked)
                {
                    param[6].Value = "Y";
                }
                else
                {
                    param[6].Value = "N";
                }
                param[7].Value = Session["UserName"].ToString();
                param[8].Value = "1";

                sysFunc.ExecuteSP_NonQuery("SP_InsertFeedbackQuestion", param);
                sysFunc.ClearTextBoxes(Page);

                createGrid();
                lblMessage.Text = "Feedback Question Inserted.";
            }
            catch (Exception ex)
            {

                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);
            }

        }

        protected void BtnRemove_Feedback_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsdelete = new DataSet();

            DataTable dt = new DataTable();

            try
            {
                dt = (DataTable)Session["FeedbackDS"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex + gv_Feedback.PageIndex * gv_Feedback.PageSize;

                String code = dt.Rows[rowGvIndex]["ParentQuestionCode"].ToString();

                //if (sysFunc.CodeExists("Customer", "ParentQuestionCode", code, Session["DealerCode"].ToString(), ref dsdelete))
                //{
                //    lblMessage.Text = "Cannot be deleted , this City is already in use";
                //    return;
                //}

                dt.Rows[rowGvIndex].Delete();
                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                gv_Feedback.DataSource = dt;
                gv_Feedback.DataBind();

                Session["FeedbackDS"] = dt;
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    //and CountryDesc = '" + txtStateDescription.Text + "' and VersionCode = '" + dsdelete.Tables[0].Rows[rowGvIndex]["Version"].ToString() + "'

                    string sql = "delete from FeedBackQuestions where  DealerCode = '" + Session["DealerCode"].ToString() + "' ParentQuestionCode = '" + code.Trim() + "' ";
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


        protected void gv_Feedback_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {


                if (e.Row.RowType == DataControlRowType.DataRow && !(gv_Feedback.EditIndex == e.Row.RowIndex))
                {

                    e.Row.Cells[0].Enabled = true;
                }

                if (e.Row.RowType == DataControlRowType.DataRow && (gv_Feedback.EditIndex == e.Row.RowIndex))
                {
                    TextBox txtParentQuestionDesc = (TextBox)e.Row.FindControl("txtParentQuestionDescEdit");
                    TextBox txtSequence = (TextBox)e.Row.FindControl("txtSequenceEdit");
                    DropDownList ddlControlType = (DropDownList)e.Row.FindControl("ddlControlTypeEdit");
                    DropDownList ddlServiceType = (DropDownList)e.Row.FindControl("ddlServiceTypeEdit");

                    txtParentQuestionDesc.Text = Session["lblParentQuestionDesc"].ToString();
                    txtSequence.Text = Session["lblSequence"].ToString();
                    ddlControlType.Text = Session["lblControlType"].ToString();
                    ddlServiceType.Text = Session["lblServiceType"].ToString();
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


        protected void gv_Feedback_DataBound(object sender, EventArgs e)
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
        protected void gv_Feedback_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                //DataSet ds = new DataSet();

                DataTable dtEditing = new DataTable();

                //Label lblDealerCode = (Label)gv_city.Rows[e.NewEditIndex].FindControl("lblDealerCode");            
                // Label lblCityCode = (Label)gv_city.Rows[e.NewEditIndex].FindControl("lblCityCode");                 
                Label lblParentQuestionDesc = (Label)gv_Feedback.Rows[e.NewEditIndex].FindControl("lblParentQuestionDesc");
                Label lblControlType = (Label)gv_Feedback.Rows[e.NewEditIndex].FindControl("lblControlType");
                Label lblServiceType = (Label)gv_Feedback.Rows[e.NewEditIndex].FindControl("lblServiceType");
                Label lblSequence = (Label)gv_Feedback.Rows[e.NewEditIndex].FindControl("lblSequence");



                // Label lblUpdUser = (Label)gv_city.Rows[e.NewEditIndex].FindControl("lblUpdUser");
                //Label lblUpdTerm = (Label)gv_city.Rows[e.NewEditIndex].FindControl("lblUpdTerm");
                //Label lblStateCode = (Label)gv_Feedback.Rows[e.NewEditIndex].FindControl("lblParentQuestionCode");

                //Session["lblDealerCode"] = lblDealerCode.Text;            
                // Session["lblCityCode"] = lblCityCode.Text;
                Session["lblParentQuestionDesc"] = lblParentQuestionDesc.Text;
                Session["lblControlType"] = lblControlType.Text;
                Session["lblServiceType"] = lblServiceType.Text;
                Session["lblSequence"] = lblSequence.Text;

                // Session["lblUpdUser"] = lblUpdUser.Text;
                // Session["lblUpdTerm"] = lblUpdTerm.Text;

                gv_Feedback.EditIndex = e.NewEditIndex;

                dtEditing = (DataTable)Session["FeedbackDS"];
                Session["FeedbackDS"] = dtEditing;
                gv_Feedback.DataSource = dtEditing;

                gv_Feedback.DataBind();
            }

            catch (Exception ex)
            {

                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;

            }

        }



        // ROW CANCEL EDIT
        protected void gv_Feedback_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataTable dt = new DataTable();

            gv_Feedback.EditIndex = -1;

            dt = (DataTable)Session["FeedbackDS"];

            gv_Feedback.DataSource = dt;

            gv_Feedback.DataBind();

        }

        protected void gv_Feedback_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataTable dt_update = new DataTable();

                //Label lblDealerCode = (Label)gv_city.Rows[e.RowIndex].FindControl("lblDealerCode");
                Label lblParentQuestionCode = (Label)gv_Feedback.Rows[e.RowIndex].FindControl("lblParentQuestionCode");
                TextBox txtParentQuestionDesc = (TextBox)gv_Feedback.Rows[e.RowIndex].FindControl("txtParentQuestionDescEdit");     // GridView TextBox Field
                DropDownList ddlControlType = (DropDownList)gv_Feedback.Rows[e.RowIndex].FindControl("ddlControlTypeEdit");
                DropDownList ddlServiceType = (DropDownList)gv_Feedback.Rows[e.RowIndex].FindControl("ddlServiceTypeEdit");
                TextBox txtSequence = (TextBox)gv_Feedback.Rows[e.RowIndex].FindControl("txtSequenceEdit");
                //Label lblUpdUser = (Label)gv_city.Rows[e.RowIndex].FindControl("lblUpdUser");
                //Label lblUpdTerm = (Label)gv_city.Rows[e.RowIndex].FindControl("lblUpdTerm");

                // Check value in GridView TextBOX
                if (txtParentQuestionDesc.Text.Trim() == string.Empty)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Description can not null or empty.";
                    return;
                }


                if (txtParentQuestionDesc.Text == "")
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Description can not be null";
                    return;
                }

                dt_update = (DataTable)Session["FeedbackDS"];

                //Uzair
                //DataRow dr =  ds_update.Tables[0].Rows[0];
                // Check in GridView

                foreach (DataRow dr in dt_update.Rows)
                {
                    if (dr["ParentQuestionCode"].ToString() == lblParentQuestionCode.Text)
                    {
                        dr["ParentQuestionDesc"] = txtParentQuestionDesc.Text.Trim();
                        dr["ControlType"] = ddlControlType.Text.Trim();
                        dr["ServiceType"] = ddlServiceType.Text.Trim();
                        dr["QSequence"] = txtSequence.Text.Trim();

                    }
                }

                gv_Feedback.EditIndex = -1;

                gv_Feedback.DataSource = dt_update;
                gv_Feedback.DataBind();

                Session["FeedbackDS"] = dt_update;

                // Updating in DataBase
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {

                    string sql = "update FeedBackQuestions set ParentQuestionDesc = '" + txtParentQuestionDesc.Text + "' , ControlType = '" + ddlControlType.Text + "' , ServiceType = '" + ddlServiceType.Text + "' ,QSequence = '" + txtSequence.Text + "' WHERE  DealerCode = '" + Session["DealerCode"].ToString() + "' and ParentQuestionCode='" + lblParentQuestionCode.Text + "'  ";
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



        protected void gv_Feedback_SelectedIndexChanged(object sender, GridViewPageEventArgs e)
        {
            gv_Feedback.PageIndex = e.NewPageIndex;
            createGrid();
        }
    }
}