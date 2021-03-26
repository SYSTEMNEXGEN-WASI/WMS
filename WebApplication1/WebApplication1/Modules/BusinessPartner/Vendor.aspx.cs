using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.ApplicationBlocks.Data;
using CrystalDecisions.Shared;
using CConn;
using DXBMS.Data;

namespace DXBMS.Modules.BusinessPartner
{
	public partial class Vendor : System.Web.UI.Page
	{
		DXBMS.SysFunctions SysFunc = new DXBMS.SysFunctions();
        clsLookUp clslook = new clsLookUp();
        DataSet ds = new DataSet();
        SecurityBll sec = new SecurityBll();
        protected void Page_Load(object sender, EventArgs e)
		{
			if (this.Session["UserName"] == null)
			{
				Response.Redirect("~/login.aspx");

			}
            if (!sec.UserRight("2504", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }

            if (Page.IsPostBack)
            {

                if (!string.IsNullOrEmpty((String)Session["LookUpData"]))
                {
                    SelectedPartDetail(Session["LookUpData"].ToString());
                }
            }

            if (!IsPostBack)
			{
                ThreadStart childthreat = new ThreadStart(childthreadcall);
                Thread child = new Thread(childthreat);

                child.Start();
                LoadGRN_DDL();
				//ddlacount_code.Items.Add(new ListItem("Select", ""));
			}
            Session["LookUpData"] = "";

        }

		private void LoadGRN_DDL()
		{
			string WhereQuery = "DealerCode =  '"+Session["DealerCode"].ToString()+"' ";
			//"and a.DelFlag = 'N' " +
			//"and a.VendorCode = b.VendorCode";
			//        "Order by a.PRNo Desc";
			string[] Columns = new string[] { "VendorCode", "VendorDesc" };
			SysFunc.GetMultiColumnsDDL(ddlVendor, Columns, "Vendor", WhereQuery, "VendorCode", " Order by VendorCode Desc", false, false);
		}

		protected void btn_save_Click(object sender, EventArgs e)
		{
			string strAutoCode = string.Empty;
            if (!sec.UserRight("2504", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (txtVendorCode.Text != "")
            {
                if (!sec.UserRight("2504", "003"))
                {
                    Response.Redirect("~/Test.aspx");
                }
            }
            
            try
			{
				if (ddlVendor.SelectedIndex == 0)
				{
					strAutoCode = SysFunc.GetNewMaxIDWithOutDealer("Vendor", "VendorCode", 6, "");
				}
				else
				{
					strAutoCode = ddlVendor.SelectedItem.Value;

				}

				SqlParameter[] param = {
								new SqlParameter("@DealerCode",SqlDbType.Char),//0
								new SqlParameter("@VendorCode",SqlDbType.Char),//1
								new SqlParameter("@VendorDesc",SqlDbType.VarChar),//2
								new SqlParameter("@Add1",SqlDbType.VarChar),//3
								new SqlParameter("@Add2",SqlDbType.VarChar),//4
								new SqlParameter("@Add3",SqlDbType.VarChar),//5
								new SqlParameter("@ContPerson ", SqlDbType.VarChar),//6
								new SqlParameter("@Phone1",SqlDbType.VarChar),//7
								new SqlParameter("@Phone2",SqlDbType.VarChar),//8
								new SqlParameter("@Fax ",SqlDbType.VarChar),//9
								new SqlParameter("@Email" ,SqlDbType.VarChar),//10
								new SqlParameter("@URL" ,SqlDbType.VarChar),//11
								new SqlParameter("@GSTno ",SqlDbType.VarChar),//12
								new SqlParameter("@NTN ",SqlDbType.VarChar),//13
								new SqlParameter("@PaymentTerm" ,SqlDbType.Float),//14
								new SqlParameter("@AccountCode",SqlDbType.VarChar),//15
								new SqlParameter("@AdvanceGiven" , SqlDbType.Float),//16
								new SqlParameter("@UpdUser ",SqlDbType.VarChar),//17
								new SqlParameter("@UpdTerm ",SqlDbType.VarChar),//18
								new SqlParameter("@CreditLimit",SqlDbType.Float),//19
                                new SqlParameter("@Title",SqlDbType.VarChar) //20
								};

				param[0].Value = Session["DealerCode"].ToString();
				param[1].Value = strAutoCode;
				param[2].Value = txtvendor_name.Text;
				param[3].Value = txtaddress_1.Text;
				param[4].Value = txtaddress_2.Text;
				param[5].Value = txtaddress_3.Text;
				param[6].Value = txtcontact_person.Text;
				param[7].Value = txtphone_1.Text;
				param[8].Value = txtphone_2.Text;
				param[9].Value = txtfax_number.Text;
				param[10].Value = txtemail.Text;
				param[11].Value = txturl.Text;
				param[12].Value = txtgst_number.Text;
				param[13].Value = txtntn_number.Text;
				param[14].Value = txtpayment_terms.Text.Trim() == "" ? "0" : txtpayment_terms.Text.Trim();
                param[15].Value = txtAccountCode.Text;
				param[16].Value = 0;
				param[17].Value = Session["UserName"].ToString();
				param[18].Value = GlobalVar.mUserIPAddress;
				param[19].Value = (txtCreditLimit.Text.Trim() == "" ? "0" : txtCreditLimit.Text);
                param[20].Value = ddlTitle.SelectedValue;


				if (SysFunc.ExecuteSP_NonQuery("Sp_Insert_Vendor", param))
				{

                    if (ddlVendor.SelectedIndex == 0)
                    {
                        SysFunc.UserMsg(lblmsg, Color.Green, "Vendor Added Successfully: " + strAutoCode);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert("+ strAutoCode + ")", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Updatealert(" + strAutoCode + ")", true);

                    }
                    

				}
				else
				{
					SysFunc.UserMsg(lblmsg, Color.Red, "Vendor not save try again!");
					return;
				}

				SysFunc.ClearTextBoxes(Page);

				

				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Vendor Added Successfully' " + strAutoCode + " );", true);

				LoadGRN_DDL();

			}
			catch (Exception ex)
			{
				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('" + ex.Message + "');", true);
			}

		}

		protected void btn_delete_Click(object sender, EventArgs e)
		{
            if (!sec.UserRight("2504", "004"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (SysFunc.IsExist("VendorCode",ddlVendor.SelectedItem.Value,"POMaster",Session["DealerCode"].ToString()))
			{
				SysFunc.UserMsg(lblmsg,Color.Red,"Vendor Code is used in PO you can not Delete");
				return;
			}
			if (ddlVendor.SelectedIndex > 0)
			{
				try
				{

					string sql = "delete from Vendor where DealerCode='"+Session["DealerCode"].ToString()+"' And VendorCode = '" + ddlVendor.SelectedItem.Value + "'";

					SysFunc.ExecuteQuery_NonQuery(sql);

					LoadGRN_DDL();

					SysFunc.ClearTextBoxes(Page);

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Deletealert()", true);
                }
				catch (Exception ex)
				{
					ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('" + ex.Message + "');", true);
				}
			}
			else
			{
				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert(' Select Vendor Code');", true);
			}

		}
		protected void btn_clear_Click(object sender, EventArgs e)
		{
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);
            SysFunc.ClearTextBoxes(Page);
			ddlVendor.SelectedIndex = 0;
            ddlTitle.SelectedIndex = 0;
		}

		protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ddlVendor.SelectedIndex == 0)
			{
				SysFunc.ClearTextBoxes(Page);
				return;
			}
			try
			{
				DataTable dt = new DataTable();

				string sql = "select * from Vendor where DealerCode='"+Session["DealerCode"].ToString()+"' And  VendorCode = '" + ddlVendor.SelectedItem.Value + "'";
				dt = SysFunc.GetData(sql);

				txtvendor_name.Text = dt.Rows[0]["VendorDesc"].ToString();
				txtcontact_person.Text = dt.Rows[0]["ContPerson"].ToString();
				txtaddress_1.Text = dt.Rows[0]["Add1"].ToString();
				txtaddress_2.Text = dt.Rows[0]["Add2"].ToString();
				txtaddress_3.Text = dt.Rows[0]["Add3"].ToString();
				txtphone_1.Text = dt.Rows[0]["Phone1"].ToString();
				txtphone_2.Text = dt.Rows[0]["Phone2"].ToString();
				txtemail.Text = dt.Rows[0]["Email"].ToString();
				txtfax_number.Text = dt.Rows[0]["Fax"].ToString();
				txtgst_number.Text = dt.Rows[0]["GSTno"].ToString();
				txtntn_number.Text = dt.Rows[0]["NTN"].ToString();
				txturl.Text = dt.Rows[0]["URL"].ToString();
				txtpayment_terms.Text = dt.Rows[0]["PaymentTerm"].ToString();
				txtAccountCode.Text = dt.Rows[0]["AccountCode"].ToString();
				txtCreditLimit.Text = dt.Rows[0]["CreditLimit"].ToString();
                ddlTitle.SelectedValue = dt.Rows[0]["Title"].ToString();
                txtAccountDesc.Text = GetAccounttitle(dt.Rows[0]["AccountCode"].ToString());
            }
			catch (Exception ex)
			{
				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('" + ex.Message + "');", true);
			}

		}

        protected void imgLookup_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 75;
            clslook.LU_Get_AccountCode(imgLookup, ViewState["lookupid"].ToString(), "", "../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        public void childthreadcall()
        {
            try
            {
                string CCon = CConnection.GetConnStringForAccount();

                //string sQuery = "Select A.contacccode +  A.SubCode + A.subsubcode + A.loccode + A.DetailCode as AccountCode , A.DetailDesc from GDetail  A";

                ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "Select A.contacccode +'-'+  A.SubCode +'-'+  A.subsubcode +'-'+  A.loccode +'-'+  A.DetailCode as AccountCode , A.DetailDesc from GDetail  A");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["Parts"] = ds;
                }

            }
            catch (ThreadAbortException e)
            {

                //lblmessage.Text += "<br /> child thread - exception";

            }
            finally
            {
                //lblmessage.Text += "<br /> child thread - unable to catch the  exception";
            }
        }

        protected void SelectedPartDetail(string item)
        {
            try
            {
                if (ViewState["lookupid"].ToString() == "8")
                {
                    txtVendorCode.Text = item;
                    ddlVendor.SelectedValue = item;
                    ddlVendor_SelectedIndexChanged(null, null);
                }
                else
                {
                    txtAccountCode.Text = item;
                    txtAccountDesc.Text= GetAccounttitle(item); 

                }
               
                
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblmsg, Color.Red, ex.Message);
            }
        }
        private string GetAccounttitle(string code)
        {
            try
            {
                string CCon = CConnection.GetConnStringForAccount();
                string value = "";

                SqlDataAdapter dta = new SqlDataAdapter("Select rtrim(A.DetailDesc) as AccountTitle from GDetail  A where A.CompCode = '" + Session["DealerCode"].ToString() + "' and A.contacccode +'-'+  A.SubCode +'-'+  A.subsubcode +'-'+  A.loccode +'-'+  A.DetailCode = '" + code + "'", CCon);

                DataTable dt = new DataTable();
                dta.Fill(dt);

                if (dt == null)
                {
                    return value;
                }

                if (dt.Rows.Count > 0)
                {
                    value = dt.Rows[0]["AccountTitle"].ToString();
                }


                return value;
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblmsg, Color.Red, ex.Message);
                return "";
            }

        }
        protected void imgVendorCode_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 8;
            clslook.LU_Get_Vendor(imgVendorCode, ViewState["lookupid"].ToString(), "DealerCode = '" + Session["DealerCode"].ToString() + "'", "../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }
    }
}