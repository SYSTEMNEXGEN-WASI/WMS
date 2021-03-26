using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DXBMS
{


    public partial class Root : System.Web.UI.MasterPage
    {

        SysFunction sysfun = new SysFunction();
        DataTable dt = new DataTable();
        SecurityBll sec = new SecurityBll();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["DealerCode"] != null)
            {
                lblDealerName.Text = Session["DealerDesc"].ToString();
                lblUserName.Text = "Wellcome - " + Session["UserName"].ToString();
                lblUser.Text = Session["UserName"].ToString();
                lblUser2.Text = Session["UserName"].ToString();
                if (!(Session["Image"] == null))
                {
                    ImgProfile.ImageUrl = "~\\" + Session["Image"].ToString();
                    ImgProfile2.ImageUrl = "~\\" + Session["ProfileImg"].ToString();
                }
                
                Imagelogo.ImageUrl = "~\\" + Session["Image"].ToString().Trim();
                lblCoDesc.Text = Session["DealerDesc"].ToString();
            }else
            {
                Response.Redirect("~/login.aspx");
            }

            if (sec.FormsVerification(Session["UserName"].ToString(), Session["DealerCode"].ToString(), Session["ApplicationCode"].ToString().Trim(), ref dt))
            {
                if (dt != null)
                {
                    Session["FormVerification"] = dt;
                    LinkButton[] txts = { lnkJobMaster, lnkCustInvoice };
                    DataRow[] drr = dt.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {
                        sysfun.checkForms(Page, dt.Rows[i]["FormName"].ToString());
                    }
                }

            }


        }

        protected void btnLogout_OnClick(object sender, EventArgs e)
        {
            Session.Abandon();
            
            Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Redirect("~/login.aspx");
        }

        protected void lnkHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected void lnkLogOut_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/login.aspx");
        }

        protected void lnkPurchaseOrder_Click(object sender, EventArgs e)
        {
            Session["MDHeading"] = "Purchase Order";
              Response.Redirect("~/Modules/SpareParts/Forms/ModelData.aspx");

        }

        protected void LnkDeliverChallan_Click(object sender, EventArgs e)
        {
            Session["MDHeading"] = "Order Confirmation";
            Response.Redirect("~/Modules/SpareParts/Forms/ModelData.aspx");

        }

      

        protected void LnkGRN_Click(object sender, EventArgs e)
        {
            Session["MDHeading"] = "Goods Receipt Note";
            Response.Redirect("~/Modules/SpareParts/Forms/ModelData.aspx");
        }

        protected void lnkPayInv_Click(object sender, EventArgs e)
        {
            Session["MDHeading"] = "Payable Invoice";
            Response.Redirect("~/Modules/SpareParts/Forms/ModelData.aspx");
        }

        protected void lnkPurchaseReturn_Click(object sender, EventArgs e)
        {
            Session["MDHeading"] = "Purchase Return";
            Response.Redirect("~/Modules/SpareParts/Forms/ModelData.aspx");
           
        }

        protected void lnkCountSaleInv_Click(object sender, EventArgs e)
        {
           
            Session["MDHeading"] = "Direct Sales";
            Response.Redirect("~/Modules/SpareParts/Forms/ModelData.aspx");
        }

        protected void lnkStockIssue_Click(object sender, EventArgs e)
        {
            
            Session["MDHeading"] = "Stock Issue";
            Response.Redirect("~/Modules/SpareParts/Forms/ModelData.aspx");
        }

       

        protected void lknStockAdj_Click(object sender, EventArgs e)
        {
            
            Session["MDHeading"] = "Stock Adjustmnet";
            Response.Redirect("~/Modules/SpareParts/Forms/ModelData.aspx");
        }
    }
}