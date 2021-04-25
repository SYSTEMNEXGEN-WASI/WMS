using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;

namespace DXBMS {
    public partial class RootMaster : System.Web.UI.MasterPage {
        protected void Page_Load(object sender, EventArgs e) {
            //cmbThemes.DataSource = DevExpress.Web.ASPxThemes.ThemesProviderEx.GetThemes();
            //cmbThemes.DataBind();
            
            //HttpCookie cTheme = Request.Cookies["theme"];
            //if (cTheme != null)
            //{
            //    cmbThemes.Text = cTheme.Value.ToString();
            //}
            //else
            //{
            //    cmbThemes.Text = "Default";
            //}

            ASPxSplitter1.GetPaneByName("Header").Size = ASPxWebControl.GlobalTheme == "Moderno" ? 95 : 83;
            ASPxSplitter1.GetPaneByName("Header").MinSize = ASPxWebControl.GlobalTheme == "Moderno" ? 95 : 83;
            ASPxLabel2.Text = DateTime.Now.Year + Server.HtmlDecode(" &copy; ");

            //string uid = "";
            //if (this.Session["UserID"] == null)
            //{
            //    MenuTop.Visible = false;
            //    btnLogOff.Visible = false;
            //    btnLogin.Visible = true;
                
            //}
            //else
            //{
            //    MenuTop.Visible = true;
            //    btnLogOff.Visible = false;
            //    btnLogin.Visible = false;
            //    btnLogOff.Text = "Log Off " + this.Session["UserName"].ToString();
            //}

            
            
        }


        //protected void btnLogOff_Click(object sender, EventArgs e)
        //{
        //    //fn.LogoffUser(this.Session["UserID"].ToString(), "Web :" + fn.GetLanIPAddress());
        //    Session.RemoveAll();
        //    Response.Redirect("~");
        //}

        //protected void btnLogin_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("~/Main/login.aspx");
        //}


      
    }
}