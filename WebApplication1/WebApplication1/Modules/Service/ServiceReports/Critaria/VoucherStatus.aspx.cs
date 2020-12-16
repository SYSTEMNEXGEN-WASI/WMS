using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DXBMS.Modules.Service.ServiceReports.Critaria
{
    public partial class VoucherStatus : System.Web.UI.Page
    {
        SysFunction sysfunc = new SysFunction();
        SecurityBll sec = new SecurityBll();
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["DealerCode"] == null)
            {
                Response.Redirect("~/login.aspx");
            }
            if (!IsPostBack)
            {
                txtFromDate.Text = txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            }
          //  txtFromDate.Text = txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");

        }
        public void GetData()
        {
            string FromDate = sysfunc.SaveDate(txtFromDate.Text).ToString();
           string  ToDate = sysfunc.SaveDate(txtToDate.Text).ToString();
            var Form = ddlForm.SelectedValue;
            string sqlquery = "SP_VoucherStatus_Pending '" + Session["DealerCode"].ToString() + "','"+ sysfunc.SaveDate(txtFromDate.Text) + "','" + sysfunc.SaveDate(txtToDate.Text) + "','" + Form + "'";
            dt = sysfunc.GetData(sqlquery);
            if (dt.Rows.Count > 0)
            {
                gvPendingVoucher.DataSource = dt;
                gvPendingVoucher.DataBind();
            }



        }

        protected void txtForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!sec.UserRight("2589", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            GetData();

        }

        protected void gvPendingVoucher_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPendingVoucher.PageIndex = e.NewPageIndex;
            GetData();
        }
    }
}