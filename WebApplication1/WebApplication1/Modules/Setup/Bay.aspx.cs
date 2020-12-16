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
    public partial class Bay : System.Web.UI.Page
    {
        SysFunction sysFunc = new SysFunction();
        DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!IsPostBack)
            {
                Loadddl();
                Create_Grid();
            }
        }

        private void Loadddl()
        {
            string WhereClause = "DealerCode = '" + Session["DealerCode"] + "'";

            string[] Columns = new string[] { "BayID", "BayDesc", "ShopID" };
            sysFunc.GetMultiColumnsDDL(ddlBays, Columns, "Bay", WhereClause, "BayID", "", false, false);

            string[] Columns1 = new string[] { "ShopID", "ShopDesc" };
            sysFunc.GetMultiColumnsDDL(ddlShop, Columns1, "Shops", "DealerCode in ('" + Session["DealerCode"] + "','COMON')", "ShopID", "", false, false);
        }

        private void Create_Grid()
        {
            String sql = "select B.BayID,B.BayDesc,S.ShopDesc,B.Active from Bay B inner join Shops S on S.ShopID = B.ShopID and S.DealerCode in (B.DealerCode,'COMON') where B.DealerCode in( '" + Session["DealerCode"].ToString() + "')";

            SysFunction sysFunc = new SysFunction();

            dt = sysFunc.GetData(sql);
            if (dt.Rows.Count > 0)
            {
                gv_Bays.DataSource = dt;
                gv_Bays.DataBind();
            }
            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strAutoCode = string.Empty;
            if (Page.IsValid)
            {
                try
                {
                    if (ddlBays.SelectedIndex == 0)
                    {
                        strAutoCode = sysFunc.GetNewMaxID("Bay", "BayID", 4, Session["DealerCode"].ToString());
                    }else
                    {
                        strAutoCode = ddlBays.SelectedValue;
                    }
                   

                    SqlParameter[] param = {
                                        new SqlParameter("@BayID",SqlDbType.Char),//0
                                        new SqlParameter("@BayDesc",SqlDbType.VarChar),//1
                                        new SqlParameter("@ShopID",SqlDbType.Char),//2
                                        new SqlParameter("@UpdUser",SqlDbType.VarChar),//3
                                        new SqlParameter("@UpdTerm",SqlDbType.VarChar),//4
                                        new SqlParameter("@DealerCode",SqlDbType.Char),//5                                    
                                        new SqlParameter("@Active",SqlDbType.Char)//6
                                      };

                    param[0].Value = strAutoCode;
                    param[1].Value = txtBayDesc.Text;
                    param[2].Value = ddlShop.SelectedValue.Trim();
                    param[3].Value = Session["UserName"].ToString();
                    param[4].Value = GlobalVar.mUserIPAddress;
                    param[5].Value = Session["DealerCode"].ToString();
                    if (chkActive.Checked)
                        param[6].Value = "Y";
                    else
                        param[6].Value = "N";


                    sysFunc.ExecuteSP_NonQuery("Sp_Insert_Bay", param);

                    cleadata();

                    lblMsg.Visible = true;
                    //lblMsg.Text = "Labor " + strAutoCode + " Added Successfully";

                    Create_Grid();
                    Loadddl();
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = ex.Message;
                }
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Fill the Required Fields";
            }
        }
        private void cleadata()
        {
            sysFunc.ClearTextBoxes(Page);
            sysFunc.Clearddl(Page);
            lblMsg.Text = "";
            Create_Grid();
        }

        protected void ddlBays_SelectedIndexChanged(object sender, EventArgs e)
        {
            String sql = "select BayID,BayDesc,ShopID,Active from Bay where DealerCode = '" + Session["DealerCode"].ToString() + "' and BayID = '" + ddlBays.SelectedValue + "'";

            SysFunction sysFunc = new SysFunction();

            dt = sysFunc.GetData(sql);

            if (dt.Rows.Count > 0)
            {
                txtBayId.Text = dt.Rows[0]["BayID"].ToString();
                txtBayDesc.Text = dt.Rows[0]["BayDesc"].ToString();
                ddlShop.SelectedValue = dt.Rows[0]["ShopID"].ToString();
                if (dt.Rows[0]["Active"].ToString() == "Y")
                    chkActive.Checked = true;
                else
                    chkActive.Checked = false;
            }
            else
            {
                lblMsg.Text = "No Data Found!";
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            cleadata();
            
        }

        protected void gv_Bays_SelectedIndexChanged(object sender, GridViewPageEventArgs e)
        {
            gv_Bays.PageIndex = e.NewPageIndex;
            Create_Grid();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if(ddlBays.SelectedIndex == 0){

                lblMsg.Text = "Please Select Bay First";
                return;
            }

            string query = "select * from JobCardDetail where BayID = '" + ddlBays.SelectedValue.Trim() + "' and DealerCode in ('"+Session["DealerCode"].ToString()+"','AAAAA')";
            DataTable dt = new DataTable();
            dt = sysFunc.GetData(query);

            if (dt.Rows.Count > 0)
            {
                lblMsg.Text = "The Bay is in use";
                return;
            }

            try
            {
                string qry = "DELETE FROM Bay WHERE DealerCode in ('" + Session["DealerCode"].ToString() + "','AAAAA') AND BayID = '" + ddlBays.SelectedValue + "'";

                sysFunc.ExecuteQuery_NonQuery(qry);

                lblMsg.Visible = true;
                lblMsg.Text = "Labor " + ddlBays.SelectedValue + " Deleted Successfully";

                cleadata();
                Create_Grid();
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }
        }
    }
        
}