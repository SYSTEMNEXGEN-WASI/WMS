using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DXBMS.Main
{
    public partial class login : System.Web.UI.Page
    {
        SysFunctions myFunc = new SysFunctions();
        SecurityBll sec = new SecurityBll();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //protected void btnSignIn_Click(object sender, EventArgs e)
        //{
        //    //DataTable dt = myFunc.GetData("select * from SecurityUser where UserName = '" + txtUserID.Text + "' and Password='" + txtPassword.Text + "'");
        //    try
        //    {
        //        DataTable dt = myFunc.GetData("select A.DealerCode,A.UserID,A.UserName ,A.ID,B.DealerDesc,B.Address1 ,B.Address2,B.Address3,B.Email,B.Fax,B.Phone1 ,B.NTN,B.Phone2,B.SaleTaxNo,B.VehicleCategory,B.Image,B.Logo,C.EmpCode,C.EmpName,C.Picture from SecurityUser A inner join DealerEmp C On A.DealerCode = C.DealerCode and C.EmpCode = A.EmpCode inner join Dealer B On A.DealerCode=B.DealerCode  where A.UserName = '" + txtUserID.Text + "' and A.Password='" + txtPassword.Text + "' And A.Active='Y'");
        //        if (dt == null)
        //        {
        //            Response.Redirect("~/ErrorPage.aspx");
        //        }

        //        if (dt.Rows.Count == 0)
        //        {
        //            //txtUserID. = "Invalid User ID or Password, Try again";
        //            lblMsg.Text = "Invalid User ID or Password, Try again";
        //        }
        //        else
        //        {
        //            this.Session["UserID"] = dt.Rows[0]["ID"].ToString();

        //            this.Session["UserName"] = dt.Rows[0]["UserName"].ToString();

        //            this.Session["EmpName"] = dt.Rows[0]["EmpName"].ToString();
        //            this.Session["EmpCode"] = dt.Rows[0]["EmpCode"].ToString();

        //            this.Session["DealerCode"] = dt.Rows[0]["DealerCode"].ToString();
        //            this.Session["DealerDesc"] = dt.Rows[0]["DealerDesc"].ToString();
        //            this.Session["DealerAddress"] = dt.Rows[0]["Address1"].ToString() + ", " + dt.Rows[0]["Address2"].ToString() + ", " + dt.Rows[0]["Address3"].ToString();
        //            this.Session["DealerEmail"] = dt.Rows[0]["Email"].ToString();
        //            this.Session["DealerFax"] = dt.Rows[0]["Fax"].ToString();
        //            this.Session["DealerPhone"] = dt.Rows[0]["Phone1"].ToString() + "," + dt.Rows[0]["Phone2"].ToString();
        //            this.Session["DealerNTN"] = dt.Rows[0]["NTN"].ToString();
        //            this.Session["DealerSaleTaxNo"] = dt.Rows[0]["SaleTaxNo"].ToString();
        //            this.Session["Image"] = dt.Rows[0]["Logo"].ToString();
        //            this.Session["Logo"] = dt.Rows[0]["Logo"].ToString();
        //            this.Session["ProfileImg"] = dt.Rows[0]["Picture"].ToString(); 
        //            this.Session["VehicleCategory"] = dt.Rows[0]["VehicleCategory"].ToString();
        //            GlobalVar.mUserIPAddress = Request.ServerVariables["REMOTE_ADDR"].ToString();
        //            GlobalVar.mDealerCode = dt.Rows[0]["DealerCode"].ToString();

        //            string BrandCode = "001";
        //            //ClsSessionInfo ObjLogin = new ClsSessionInfo(dt.Rows[0]["ID"].ToString(), txtPassword.Text, myFunc.ActiveUserName(this.Session["UserID"].ToString()), DateTime.Now.Date, Request.ServerVariables["REMOTE_ADDR"].ToString(), dt.Rows[0]["DealerCode"].ToString(), BrandCode);
        //            //Session["clsSessionInfo"] = ObjLogin;
        //            //myFunc.LoginUser(this.Session["UserID"].ToString(), "Web :" + myFunc.GetLanIPAddress());

        //            Response.Redirect("~/");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMsg.Text = "Error: " + ex.Message;
        //    }
        //}

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            DataTable dtLoginVerification = new DataTable();
            if (!sec.LoginVerification(txtUserID.Text, txtPassword.Text, ref dtLoginVerification))
            {


                lblMsg.Text = "Invalid User ID or Password, Try again or You are not Allowed to Login. Please contact to administrator";
                lblMsg.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                try
                {


                    // DataTable dt = myFunc.GetData("select A.DealerCode,A.UserID,A.UserName ,A.ID,B.DealerDesc,B.Address1 ,B.Address2,B.Address3,B.Email,B.Fax,B.Phone1 ,B.NTN,B.Phone2,B.SaleTaxNo,B.Image,B.Logo,C.EmpCode,C.EmpName from SecurityUser A inner join DealerEmp C On A.DealerCode = C.DealerCode and C.EmpCode = A.EmpCode inner join Dealer B On A.DealerCode=B.DealerCode  where A.UserName = '" + txtUserID.Text + "' and A.Password='" + txtPassword.Text + "' And A.Active='Y'");
                    DataTable dt = dtLoginVerification;
                    if (dt == null)
                    {
                        Response.Redirect("~/ErrorPage.aspx");
                    }

                    if (dt.Rows.Count == 0)
                    {
                        //txtUserID. = "Invalid User ID or Password, Try again";
                        lblMsg.Text = "Invalid User ID or Password, Try again";
                    }
                    else
                    {
                        this.Session["ApplicationCode"] = dtLoginVerification.Rows[0]["ApplicationCode"].ToString();

                        this.Session["UserID"] = dt.Rows[0]["ID"].ToString();

                        this.Session["UserName"] = dt.Rows[0]["UserName"].ToString();

                        this.Session["EmpName"] = dt.Rows[0]["EmpName"].ToString();
                        this.Session["EmpCode"] = dt.Rows[0]["EmpCode"].ToString();

                        this.Session["DealerCode"] = dt.Rows[0]["DealerCode"].ToString();
                        this.Session["DealerDesc"] = dt.Rows[0]["DealerDesc"].ToString();
                        this.Session["DealerAddress"] = dt.Rows[0]["Address1"].ToString() + ", " + dt.Rows[0]["Address2"].ToString() + ", " + dt.Rows[0]["Address3"].ToString();
                        this.Session["DealerEmail"] = dt.Rows[0]["Email"].ToString();
                        this.Session["DealerFax"] = dt.Rows[0]["Fax"].ToString();
                        this.Session["DealerPhone"] = dt.Rows[0]["Phone1"].ToString() + "," + dt.Rows[0]["Phone2"].ToString();
                        this.Session["DealerNTN"] = dt.Rows[0]["NTN"].ToString();
                        this.Session["DealerSaleTaxNo"] = dt.Rows[0]["SaleTaxNo"].ToString();
                        this.Session["Image"] = dt.Rows[0]["Logo"].ToString();
                        this.Session["Logo"] = dt.Rows[0]["Logo"].ToString();
                        this.Session["ProfileImg"] = dt.Rows[0]["Picture"].ToString();
                        GlobalVar.mUserIPAddress = Request.ServerVariables["REMOTE_ADDR"].ToString();
                        GlobalVar.mDealerCode = dt.Rows[0]["DealerCode"].ToString();
                        this.Session["VehicleCategory"] = dt.Rows[0]["VehicleCategory"].ToString();
                        GlobalVar.mUserIPAddress = Request.ServerVariables["REMOTE_ADDR"].ToString();
                        GlobalVar.mDealerCode = dt.Rows[0]["DealerCode"].ToString();
                        this.Session["Whatsapp"] = dt.Rows[0]["Whatsapp"].ToString();
                        this.Session["WhatsappNo"] = dt.Rows[0]["WhatsappNo"].ToString();
                        this.Session["ComplainNo"] = dt.Rows[0]["ComplainNo"].ToString();
                        string BrandCode = "001";
                        //ClsSessionInfo ObjLogin = new ClsSessionInfo(dt.Rows[0]["ID"].ToString(), txtPassword.Text, myFunc.ActiveUserName(this.Session["UserID"].ToString()), DateTime.Now.Date, Request.ServerVariables["REMOTE_ADDR"].ToString(), dt.Rows[0]["DealerCode"].ToString(), BrandCode);
                        //Session["clsSessionInfo"] = ObjLogin;
                        //myFunc.LoginUser(this.Session["UserID"].ToString(), "Web :" + myFunc.GetLanIPAddress());

                        Response.Redirect("~/");
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Text = "Error: " + ex.Message;
                }
            }
        }

    }
}