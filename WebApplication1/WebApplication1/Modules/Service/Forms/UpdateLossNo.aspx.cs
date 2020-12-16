using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DXBMS.Modules.Service.Forms
{
    public partial class UpdateLossNo : System.Web.UI.Page
    {
        MainBLL objMBLL = new MainBLL();
        SysFunction SysFuncs = new SysFunction();
        SysFunctions SysFunc = new SysFunctions();
        Transaction ObjTrans = new Transaction();
        SecurityBll sec = new SecurityBll();
        SqlTransaction Trans;
        //string apStr;
    

 
        //decimal deductAmount;
        clsLookUp clslook = new clsLookUp();
        string CustomerEstimateCode;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2582", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }

            if (Page.IsPostBack)
            {

                if (!string.IsNullOrEmpty(Session["LookUpData"].ToString()))
                {
                    SelectedPartDetail(Session["LookUpData"].ToString());
                }
            }

            if (!IsPostBack)
            {

              //  ThreadStart childthreat = new ThreadStart(childthreadcall);
               // Thread child = new Thread(childthreat);

              //  child.Start();


                setInitialDates();
          
              }
            Session["LookUpData"] = string.Empty;

        }
        private void setInitialDates()
        {
            txtCreateDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            
        }

        protected void imgEstNo_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 73;
            clslook.LU_Get_CustomerEstimateNoForm(imgEstNo, ViewState["lookupid"].ToString(), "", "../../../");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void SelectedPartDetail(string item)
        {
            try
            {
             if (ViewState["lookupid"].ToString() == "73")
                {
                    txtExtno.Text = item;
                    txtLossNo.Text=SysFunc.GetStringValuesAgainstCodes("CustomerEstimateCode='" + txtExtno.Text + "' and DealerCode='"+Session["DealerCode"].ToString()+"'", "CustomerEstimateMaster", "LossNo");
                    string lossdate = SysFunc.GetStringValuesAgainstCodes("CustomerEstimateCode='" + txtExtno.Text + "' and DealerCode='" + Session["DealerCode"].ToString() + "'", "CustomerEstimateMaster", "LossDate");
                    if(lossdate !=null && lossdate != "")
                    {
                        txtCreateDate.Text = SysFuncs.GetDate(lossdate);
                    }
                    
                }
                }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, ex.Message);
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {

            txtLossNo.Text = "";
            txtExtno.Text = "";
            setInitialDates();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2582", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (txtExtno.Text != "")
            {
                if (!sec.UserRight("2582", "003"))
                {
                    Response.Redirect("~/Test.aspx");
                }
            }
          
            TextBox[] textBoxes = { txtExtno, txtLossNo };
            try
            {


                SqlParameter[] CustomerEstimate_param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
           /*1*/ new SqlParameter("@CustomerEstimateCode",SqlDbType.Char,8),
           /*2*/ new SqlParameter("@LossDate",SqlDbType.DateTime),
           /*3*/ new SqlParameter("@LossNo",SqlDbType.VarChar,50),
            };

                CustomerEstimate_param[0].Value = Session["DealerCode"].ToString();
                CustomerEstimate_param[1].Value = txtExtno.Text;
                CustomerEstimate_param[2].Value = SysFunc.SaveDate(txtCreateDate.Text);
                CustomerEstimate_param[3].Value = txtLossNo.Text.Trim();
                if (SysFunc.ExecuteSP_NonQuery("[sp_Update_CustomerEstimate_LossNo]", CustomerEstimate_param))
                {
                    SysFunc.UserMsg(lblMsg, Color.Green, "Record Saved Successfully: " + txtExtno.Text, txtExtno);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Updatealert()", true);
                }
                else
                {
                    SysFunc.UserMsg(lblMsg, Color.Green, "Record Not Saved Successfully: " + txtExtno.Text, txtExtno);
                 
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }
    }
}