using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace DXBMS.Modules.SpareParts.Forms
{
    public partial class Solution : System.Web.UI.Page
    {

        SysFunction sysFunc = new SysFunction();
        SysFunctions sysFuncs = new SysFunctions();
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
            {
                createGrid();
            }
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            if (txtSolutionsDesc.Text == "")
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please Enter Description", txtSolutionsDesc);
                return;
            }
            else if (txtProject.Text == "")
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please Enter Project", txtProject);
                return;
            }
            else
            {
                lblMessage.Text = string.Empty;
            }

            try
            {
                string max = sysFuncs.GetNewMaxID("Solution", "Solution_Code", 4, Session["DealerCode"].ToString());
                // string max = sysFuncs.AutoGen("WorkFlowSetup", "WFCode", Session["DealerCode"].ToString(), DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                SqlParameter[] param =
           {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                new SqlParameter("@Solution_Code",SqlDbType.Char,4), //1
                new SqlParameter("@Solution_Desc",SqlDbType.VarChar,50),//2
                new SqlParameter("@Project",SqlDbType.VarChar,50), //3
                new SqlParameter("@UpdUser",SqlDbType.VarChar,50), //4
                new SqlParameter("@UpdTerm",SqlDbType.VarChar,50),//5 
            };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = max;
                param[2].Value = txtSolutionsDesc.Text.Trim();
                param[3].Value = txtProject.Text.Trim();
                param[4].Value = Session["UserName"].ToString();
                param[5].Value = "1";

                sysFunc.ExecuteSP_NonQuery("SP_Solution_insert", param);
                sysFunc.ClearTextBoxes(Page);

                createGrid();
                sysFuncs.UserMsg(lblMessage, Color.Green, "Data Inserted", txtSolutionsDesc);

            }
            catch (Exception ex)
            {
                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);
            }
        }

        //Grid
        private void createGrid()
        {
            String query = "select * from Solution";

            DataTable dt = new DataTable();

            dt = sysFunc.GetData(query);

            if (dt.Rows.Count > 0)
            {
                gv_Solution.DataSource = dt;
                gv_Solution.DataBind();
            }
            Session["SolutionDS"] = dt;
        }


        // Remove Button Code
        protected void BtnRemove_Solution_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsdelete = new DataSet();

            DataTable dt = new DataTable();

            try
            {
                dt = (DataTable)Session["SolutionDS"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex + gv_Solution.PageIndex * gv_Solution.PageSize;

                String code = dt.Rows[rowGvIndex]["Solution_Code"].ToString();


                dt.Rows[rowGvIndex].Delete();
                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                gv_Solution.DataSource = dt;
                gv_Solution.DataBind();

                Session["SolutionDS"] = dt;
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {

                    string sql = "delete from Solution where Solution_Code = '" + code.Trim() + "' ";
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