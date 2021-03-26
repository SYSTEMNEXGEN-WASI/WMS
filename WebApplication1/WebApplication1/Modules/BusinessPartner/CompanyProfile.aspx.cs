using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DXBMS;
using System.IO;

namespace DXBMS.Modules.BusinessPartner
{
    public partial class CompanyProfile : System.Web.UI.Page
    {
        SysFunctions SysFunc = new SysFunctions();

        MainBLL objMBLL = new MainBLL();
        ServiceBL objSBL = new ServiceBL();
        //SysFunctions grl = new SysFunctions();
        Transaction ObjTrans = new Transaction();
        SecurityBll sec = new SecurityBll();
        SqlTransaction Trans;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2503", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (!IsPostBack)
            {
                ViewData();
                Session["ImagePath"] = string.Empty;
            }
            FileUpload1.Attributes["onchange"] = "UploadFile(this)";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (!sec.UserRight("2503", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (!sec.UserRight("2503", "003"))
            {
                Response.Redirect("~/Test.aspx");
            }


            try
            {
                if (txtEmail1.Text != "")
                {
                    if (!txtEmail1.Text.Contains('@'))
                    {
                        lblmsg.Visible = true;
                        lblmsg.Text = "Enter Valid Email Address";
                        lblmsg.ForeColor = Color.Red;
                        txtEmail1.Focus();
                        return;
                    }
                }
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    string sql = "UPDATE Dealer SET [DealerDesc] = '" + txtDealerName.Text.Trim() + "' ,[Address1] = '" + txtAddress1.Text + "',[Address2] ='" + txtAddress2.Text + "',[Address3] ='" + txtAddress3.Text + "' " +
              ",[SaleTaxNo] = '" + txtTaxReg.Text + "',[Phone1] = '" + txtPhone1.Text + "',[Phone2] = '" + txtPhone2.Text + "',[Email] = '" + txtEmail1.Text + "',[Fax] = '" + txtFax.Text + "',[NTN] = '" + txtNTN.Text + "'"+
              ",[UpdUser] = '" + Session["UserName"].ToString() + "',[UpdDate] = '" + SysFunc.SaveDate(DateTime.Now.ToString("dd/MM/yyyy")) + "',[UpdTerm] = '" + GlobalVar.mUserIPAddress + "'" +
              ",[PST] = '" + txtPSTPerc.Text + "',[HandlingCharges] = '" + Convert.ToDecimal(txtHandling.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : txtHandling.Text.Replace("&nbsp;", "").Trim()) + "' ,[CreditLimit] = '" + txtCreditLimit.Text + "' ,[Logo] = '" + Session["Logo"].ToString() + "'  WHERE DealerCode = '" + Session["DealerCode"].ToString() + "'";

                    SysFunc.ExecuteQuery(sql, Trans);

                }

                ObjTrans.CommittTransaction(ref Trans);
                lblmsg.Visible = true;
                lblmsg.ForeColor = System.Drawing.Color.Green;
                lblmsg.Text = "Profile Updated";
                
            }
            catch (Exception ex)
            {
                //ObjTrans.CommittTransaction(ref Trans);
                ObjTrans.RollBackTransaction(ref Trans);
                lblmsg.Visible = true;
                lblmsg.ForeColor = System.Drawing.Color.Red;
                lblmsg.Text = ex.Message;
            }
        }


        protected void txtDealerCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void ViewData()
        {
            DataTable dt = new DataTable();

            try
            {
                string sql = "Select * from Dealer where DealerCode = '" + Session["DealerCode"].ToString().Trim() + "'";
                dt = SysFunc.GetData(sql);

                txtDealerCode.Text = dt.Rows[0]["DealerCode"].ToString();
                txtDealerName.Text = dt.Rows[0]["DealerDesc"].ToString();
                txtAddress1.Text = dt.Rows[0]["Address1"].ToString();
                txtAddress2.Text = dt.Rows[0]["Address2"].ToString();
                txtAddress3.Text = dt.Rows[0]["Address3"].ToString();
                txtPhone1.Text = dt.Rows[0]["Phone1"].ToString();
                txtPhone2.Text = dt.Rows[0]["Phone2"].ToString();
                txtFax.Text = dt.Rows[0]["Fax"].ToString();
                txtTaxReg.Text = dt.Rows[0]["SaleTaxNo"].ToString();
                txtEmail1.Text = dt.Rows[0]["Email"].ToString();
                txtNTN.Text = dt.Rows[0]["NTN"].ToString();
                txtCreditLimit.Text = dt.Rows[0]["CreditLimit"].ToString();
                txtPSTPerc.Text = dt.Rows[0]["PST"].ToString();
                txtHandling.Text = dt.Rows[0]["HandlingCharges"].ToString();
                txtOpAs.Text= dt.Rows[0]["OperatedBy"].ToString();
                txtDealerRefC.Text = dt.Rows[0]["DealerRefCode"].ToString();
                txtVehicleCategory.Text = dt.Rows[0]["VehicleCategory"].ToString();
                if (dt.Rows[0]["Logo"].ToString() == "")
                {
                    Image1.ImageUrl = "~/Images/BlankProfile.jpg";
                }
                else
                {
                    Image1.ImageUrl =  "../../"+dt.Rows[0]["Logo"].ToString();
                }
            }
            catch (Exception ex)
            {
                lblmsg.Visible = true;
                lblmsg.ForeColor = System.Drawing.Color.Red;
                lblmsg.Text = ex.Message;
            }

        }

        protected void OnClick_btnUpload(object sender, EventArgs e)
        {
            HttpPostedFile postedFile = FileUpload1.PostedFile;

            Session["Image"] = postedFile;

            string imgName = FileUpload1.FileName;
            string imgPath = "Images/" + imgName;
            
            int imgSize = FileUpload1.PostedFile.ContentLength;

            string fileExtension = Path.GetExtension(imgName);

            if (fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".gif"
                || fileExtension.ToLower() == ".png" || fileExtension.ToLower() == ".bmp")
            {
                FileUpload1.SaveAs(Server.MapPath("../../"+imgPath));
                Image1.ImageUrl = "~/"+ imgPath;
                Session["ImagePath"] = imgPath;
                Session["Image"] = imgPath;
            }
            else {
                lblmsg.Visible = true;
                lblmsg.ForeColor = System.Drawing.Color.Red;
                lblmsg.Text = "Only images (.jpg, .png, .gif and .bmp) can be uploaded";
            }

        }

    }
}
