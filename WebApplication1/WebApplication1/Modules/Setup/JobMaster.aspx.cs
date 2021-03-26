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
    public partial class JobMaster : System.Web.UI.Page
    {

        SysFunction SysFunc = new SysFunction();
        SysFunctions sysFuncs = new SysFunctions();
        SecurityBll sec = new SecurityBll();
        DataSet ds;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!IsPostBack)
            {
                LoadDDL_JobCategory();
                LoadDDL_DefJobCode();
                LoadDDL_AccountCode();
                LoadDDL_BayCode();

                Create_Grid();
            }

           //sec.Check_Action(btnSave1,"View");
        //    sec.Check_Action(btnSave1, "Add");
           // sec.Check_Action(btnSave1, "Edit");
         //   sec.Check_Action(btnDelete0, "Delete");

            lblMsg.Visible = false;
        }
        protected void btnClear0_Click(object sender, EventArgs e)
        {
            cleadata();
            LoadDDL_JobCategory();
            Create_Grid();

        }

        private void LoadDDL_JobCategory()
        {
            string WhereQuery = "DealerCode in('" + Session["DealerCode"] + "','COMON')";

            string[] Columns = new string[] { "JobCatCode", "JobCatDesc" };
            SysFunc.GetMultiColumnsDDL(ddljob_category, Columns, "JobCategory", WhereQuery, "JobCatCode", "", false, false);
        }

        private void LoadDDL_AccountCode()
        {
            string WhereQuery = "DealerCode in('" + Session["DealerCode"] + "','COMON')";

            string[] Columns = new string[] { "AccountCode" };
            SysFunc.GetMultiColumnsDDL(ddlacount_code, Columns, "JobCategory", WhereQuery, "AccountCode", "", false, false);
        }

        private void LoadDDL_DefJobCode()
        {
            string WhereQuery = "DealerCode in(  '" + Session["DealerCode"] + "','COMON')";

            string[] Columns = new string[] { "DefJobCode", "DefJobDesc" };
            SysFunc.GetMultiColumnsDDL(ddljob_code, Columns, "DefaultJob", WhereQuery, "DefJobCode", "", false, false);
        }

        private void LoadDDL_BayCode()
        {
            string WhereQuery = "DealerCode = '" + Session["DealerCode"] + "'";

            string[] Columns = new string[] { "BayID", "BayDesc" };
            SysFunc.GetMultiColumnsDDL(ddlBayNo, Columns, "Bay", WhereQuery, "BayID", "", false, false);
        }

        private void Create_Grid()
        {
            ds = new DataSet();

            ds.Tables.Add();

            ds.Tables[0].Columns.Add(new DataColumn("DefJobCode", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("DefJobDesc", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("LaborAccount", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("StandardTime", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("BayCode", typeof(string)));


            //DataRow dr = ds.Tables[0].NewRow();
            //ds.Tables[0].Rows.Add(dr);

            gv_labor.DataSource = ds.Tables[0];
            gv_labor.DataBind();

            Session["LaborDS"] = ds;
        }

        private void cleadata()
        {
            //SysFunc.Clearddl(Page);
            SysFunc.ClearTextBoxes(Page);
            
            LoadDDL_DefJobCode();
            LoadDDL_AccountCode();
            LoadDDL_BayCode();
        }

        protected void BtnRemove_Labor_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsdelete = new DataSet();

            try
            {
                dsdelete = (DataSet)Session["LaborDS"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;

                dsdelete.Tables[0].Rows[rowGvIndex].Delete();
                dsdelete.Tables[0].AcceptChanges();
                if (dsdelete.Tables[0].Rows.Count == 0)
                {
                    DataRow dr = dsdelete.Tables[0].NewRow();
                    dsdelete.Tables[0].Rows.Add(dr);
                }

                gv_labor.DataSource = dsdelete.Tables[0];
                gv_labor.DataBind();

                Session["LaborDS"] = dsdelete;
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnSave1_Click(object sender, EventArgs e)
        {         
               
            
            string strAutoCode = string.Empty;
            try
            {
                if (ddljob_code.SelectedIndex == 0)
                {
                    strAutoCode = SysFunc.GetNewMaxID("DefaultJob", "DefJobCode", 8, Session["DealerCode"].ToString());
                }
                else
                {
                    strAutoCode = ddljob_code.SelectedItem.Value;

                }

                SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                    new SqlParameter("@JobCatCode",SqlDbType.Char),//1
                                    new SqlParameter("@DefJobCode",SqlDbType.Char),//2
                                    new SqlParameter("@DefJobDesc",SqlDbType.VarChar),//3
                                    new SqlParameter("@UpdUser",SqlDbType.VarChar),//4
                                    new SqlParameter("@UpdTerm",SqlDbType.VarChar),//5
                                    new SqlParameter("@LaborAccount",SqlDbType.Char),//6
                                    new SqlParameter("@StandardTime",SqlDbType.Char),//7
                                    new SqlParameter("@BayCode",SqlDbType.Char)
                                  };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddljob_category.SelectedValue;
                param[2].Value = strAutoCode;
                param[3].Value = txtjob_desc.Text;
                param[4].Value = Session["UserName"].ToString();
                param[5].Value = GlobalVar.mUserIPAddress;
                param[6].Value = ddlacount_code.SelectedValue;
                param[7].Value = txtStnTime.Text;
                param[8].Value = ddlBayNo.SelectedValue;


                SysFunc.ExecuteSP_NonQuery("Sp_Insert_DefaultJob", param);

                cleadata();

                lblMsg.Visible = true;
                //lblMsg.Text = "Labor " + strAutoCode + " Added Successfully";
                ddljob_category_SelectedIndexChanged(null, null);
                //Create_Grid();
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnDelete0_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "DELETE FROM DefaultJob WHERE  DealerCode in('" + Session["DealerCode"] + "','COMON') AND DefJobCode = '" + ddljob_code.SelectedValue + "'";

                SysFunc.ExecuteQuery_NonQuery(qry);

                lblMsg.Visible = true;
                lblMsg.Text = "Labor " + ddljob_code.SelectedValue + " Deleted Successfully";

                cleadata();
                LoadDDL_JobCategory();
                Create_Grid();
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }
        }

        protected void ddljob_code_SelectedIndexChanged(object sender, EventArgs e)
        {
          // sec.Check_Action(btnSave1,"Edit");

            if (ddljob_code.SelectedIndex == 0)
            {
                cleadata();
                LoadDDL_JobCategory();
                Create_Grid();
                return;
            }
            try
            {
                DataTable dt = new DataTable();



                string sql = "Select * from DefaultJob where  DealerCode in('" + Session["DealerCode"] + "','COMON') AND DefJobCode = '" + ddljob_code.SelectedItem.Value + "'";
                dt = SysFunc.GetData(sql);

                ddljob_category.SelectedValue = dt.Rows[0]["JobCatCode"].ToString();
                txtjob_desc.Text = dt.Rows[0]["DefJobDesc"].ToString();
                ddlacount_code.SelectedIndex = 0;
                txtStnTime.Text = dt.Rows[0]["StandardTime"].ToString();

                if (dt.Rows[0]["BayCode"].ToString() == "0" || dt.Rows[0]["BayCode"].ToString() == "" || dt.Rows[0]["BayCode"].ToString() == null)
                {
                    ddlBayNo.SelectedIndex = 0;
                }
                else
                {
                    //ddlBayNo.SelectedValue = dt.Rows[0]["BayCode"].ToString();
                }

                

            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }

        }

        protected void btnPrint(object sender, EventArgs e)
        {

        }




        protected void ddljob_category_SelectedIndexChanged(object sender, EventArgs e)
        {
            String sql = "select DefJobCode,DefJobDesc,LaborAccount,StandardTime,BayCode from DefaultJob where  DealerCode in('" + Session["DealerCode"] + "','COMON') and JobCatCode = '" + ddljob_category.SelectedValue + "'";

            SysFunctions sysFunc = new SysFunctions();


            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            
            sysFunc.ExecuteQuery(sql,ref ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gv_labor.DataSource = ds;
                gv_labor.DataBind();
                Session["JobsDS"] = ds;
            }
            else
            {
                Create_Grid();
            }

            Session["LaborDS"] = ds;           

        }

        protected void gv_labor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_labor.PageIndex = e.NewPageIndex;

            gv_labor.DataSource = (DataSet)Session["JobsDS"];
            gv_labor.DataBind();
            
        }

        protected void gv_labor_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gv_labor.Rows[gv_labor.SelectedRow.RowIndex];
            ddljob_code.SelectedValue = row.Cells[2].Text;
            
            txtjob_desc.Text = row.Cells[3].Text;
            ddlacount_code.SelectedValue = row.Cells[4].Text.Trim() == "&nbsp;" ? "0" : row.Cells[4].Text.Trim();
            txtStnTime.Text = row.Cells[5].Text;
            ddlBayNo.SelectedValue = row.Cells[6].Text.Trim() == "&nbsp;" ? "0" : row.Cells[6].Text;


           // sec.Check_Action(btnSave1,"Edit");
        }

       
    }
}