using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DXBMS
{
    public partial class LookupExample : System.Web.UI.Page
    {
        clsLookUp clslook = new clsLookUp();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LookUpData"] != null)
            {

            }

        }
        protected void btnLookup_Click(object sender, EventArgs e)
        {
            ViewState["lookupid"] = 32; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_InvoiceNo(btnLookup, ViewState["lookupid"].ToString(), "", "");
        }
        private void lookUptext(string lookUpID)
        {

            switch (lookUpID)
            {
                case "1":
                    // Customer | Customer Description
                    GetLookUpsValue(ref TextBox1, ref TextBox2, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString());
                    break;

                default:
                    break;
            }
        }
        private void GetLookUpsValue(ref TextBox txt1, ref TextBox txt2, string ixd1, string ixd2)
        {


            if (Session["LookUpData"] != null)
            {
                GridViewRow Drow = (GridViewRow)Session["LookUpData"];
                txt1.Text = (txt1 != null ? Drow.Cells[Convert.ToInt32(ixd1)].Text.ToString() : "");
                txt2.Text = (txt2 != null ? Drow.Cells[Convert.ToInt32(ixd2)].Text.ToString() : "");

                //hf.Value = txt1.Text;
                Session["LookUpData"] = null;
            }
        }

    }
}