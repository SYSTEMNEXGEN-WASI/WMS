using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DXBMS.Modules.BusinessPartner
{
    public partial class Customer : System.Web.UI.Page
    {
        SysFunction sysfun = new SysFunction();
        DataTable dt = new DataTable();
        SecurityBll sec = new SecurityBll();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (this.Session["UserName"] == null)
                {
                    Response.Redirect("~/login.aspx");

                }
                if (!sec.UserRight("2505", "001"))
                {
                    Response.Redirect("~/Test.aspx");
                }


                if (!Page.IsPostBack)
                {


                    string sql = "Select CusCode , CusDesc , Address1 , CellNo , NIC,NTNno from Customer C WHERE C.DealerCode = '" + Session["DealerCode"].ToString() + "' Order by CusCode Desc";

                    dt = sysfun.GetData(sql);
                    

                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    if (dt.Rows.Count > 0)
                    {
                        GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }

                    
                    //DataBind();
                    //BindDummyRow();

                }
                
            }

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!sec.UserRight("2505", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }


            ViewState["CusCode"] = GridView1.SelectedRow.Cells[0].Text;
            Session["CusCode"] = GridView1.SelectedValue;
            Response.Redirect("~/Modules/BusinessPartner/CustProfile.aspx");

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
    }
}