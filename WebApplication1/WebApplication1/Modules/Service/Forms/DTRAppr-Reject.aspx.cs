using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DXBMS.Modules.Service.Forms
{

        
    public partial class DTRAppr_Reject : System.Web.UI.Page
    {
        SysFunction sysfun = new SysFunction();
        SecurityBll sec = new SecurityBll();
        DataTable dt,dt2,dt3;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (this.Session["UserName"] == null)
                {
                    Response.Redirect("~/login.aspx");

                }
                if (!sec.UserRight("2538", "001"))
                {
                    Response.Redirect("~/Test.aspx");
                }
                txtSearch.Attributes.Add("onchange", "setTimeout('__doPostBack(\\'" + txtSearch.ClientID.Replace("_", "$") + "\\',\\'\\')', 0);");
                hfTab.Value = Request.Form[hfTab.UniqueID];
                if (!Page.IsPostBack)
                {
                
                    string sql = "Get_DTRApp_Reject '" + Session["DealerCode"].ToString() + "'";
                    string sql1= "Get_DTR_Approved '" + Session["DealerCode"].ToString() + "'";
                    string sql2 = "Get_DTR_Post '" + Session["DealerCode"].ToString() + "'";
                    dt = sysfun.GetData(sql);
                    dt2 = sysfun.GetData(sql1);
                    dt3 = sysfun.GetData(sql2);
                    grvDTR.DataSource = dt;
                    grvDTR.DataBind();
                    if (dt.Rows.Count > 0)
                    {
                        grvDTR.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
                    grvCloseDTR.DataSource = dt2;
                    grvCloseDTR.DataBind();
                    if (dt2.Rows.Count > 0)
                    {
                        grvCloseDTR.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
                    grvPostDTR.DataSource = dt3;
                    grvPostDTR.DataBind();
                    if (dt3.Rows.Count > 0)
                    {
                        grvPostDTR.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
                }

            }
        }

        protected void grvDTR_RowCreated(object sender, GridViewRowEventArgs e)
        {
           
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                e.Row.ToolTip = "Click to edit content";
                e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.grvDTR, "Select$" + e.Row.RowIndex);
            }
        }

        protected void grvDTR_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["FIRCode"] = grvDTR.SelectedRow.Cells[0].Text;
            Session["FIRCode"] = grvDTR.SelectedValue;
            Response.Redirect("~/Modules/Service/Forms/DealerTechnical.aspx");

        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        protected void grvCloseDTR_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!sec.UserRight("2538", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            ViewState["FIRCode"] = grvCloseDTR.SelectedRow.Cells[0].Text;
            Session["FIRCode"] = grvCloseDTR.SelectedValue;
            Response.Redirect("~/Modules/Service/Forms/DealerTechnical.aspx");

        }

        protected void grvPostDTR_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!sec.UserRight("2538", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            ViewState["FIRCode"] = grvPostDTR.SelectedRow.Cells[0].Text;
            Session["FIRCode"] = grvPostDTR.SelectedValue;
            Response.Redirect("~/Modules/Service/Forms/DealerTechnical.aspx");

        }

        protected void grvPostDTR_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                e.Row.ToolTip = "Click to edit content";
                e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.grvPostDTR, "Select$" + e.Row.RowIndex);
            }

        }

        protected void grvCloseDTR_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                e.Row.ToolTip = "Click to edit content";
                e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.grvCloseDTR, "Select$" + e.Row.RowIndex);
            }

        }
    }
}