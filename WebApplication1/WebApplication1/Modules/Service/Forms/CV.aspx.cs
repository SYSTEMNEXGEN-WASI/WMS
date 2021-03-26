using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DXBMS.Modules.Service.Forms
{
    public partial class CV : System.Web.UI.Page
    {
        SysFunction sysfun = new SysFunction();
        SecurityBll sec = new SecurityBll();
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2530", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            txtSearch.Attributes.Add("onchange", "setTimeout('__doPostBack(\\'" + txtSearch.ClientID.Replace("_", "$") + "\\',\\'\\')', 0);");
            if (!Page.IsPostBack)
            {
                string sql = "Select  A.cusCode, B.CusDesc as Customer,A.EndUserCode,B.CusDesc as AccountOf,B.cellno as CellNo,A.RegNo,A.ChassisNo,A.EngineNo From CustomerVehicle A" +
                             " inner join Customer B On A.Dealercode = B.DealerCode" +
                             " and A.Cuscode = B.Cuscode where A.DealerCode = '" + Session["DealerCode"].ToString() + "' order by A.CusCode Desc";

                dt = sysfun.GetData(sql);


                GridView1.DataSource = dt;
                GridView1.DataBind();
                if (dt.Rows.Count > 0)
                {
                    GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
                }else
                {
                    Label1.ForeColor = System.Drawing.Color.Red;
                    Label1.Text = "There is no data in the Customer Vehicle";
                }

            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string SearchString = txtSearch.Text;
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!sec.UserRight("2530", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }

            //GridViewRow row = GridView1.SelectedRow;
            //Label lbl = row.FindControl("lblJobCard") as Label;
            //ViewState["CusCode"] = lbl.Text;
            //string lead = GridView1.SelectedValue.ToString();
            ViewState["CusCode"] = GridView1.SelectedRow.Cells[0].Text;
            Session["CusCode"] = GridView1.SelectedValue;
            Response.Redirect("~/Modules/Service/Forms/CustomerVehicle.aspx");

        }

        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {

            //ViewState["JobCardCode"] = GridView2.SelectedRow.Cells[0].Text;
            //Response.Redirect("~/Modules/Service/Forms/JobCardNew.aspx?JobCardCode=" + GridView2.SelectedValue);

        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                e.Row.ToolTip = "Click to edit content";
                e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.GridView1, "Select$" + e.Row.RowIndex);
            }
        }

        //protected void GridView2_RowCreated(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';";
        //        e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";
        //        e.Row.ToolTip = "Click to edit content";
        //        e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.GridView2, "Select$" + e.Row.RowIndex);
        //    }
        //}
    }
}