using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.ApplicationBlocks.Data;
using CrystalDecisions.Shared;
using CConn;
using DXBMS.Data;
namespace DXBMS.Modules.BusinessPartner
{
    public partial class VendorMaster : System.Web.UI.Page
    {
        DXBMS.SysFunctions SysFunc = new DXBMS.SysFunctions();

        protected void Page_Load(object sender, EventArgs e)
        {
            
            lblmsg.Visible = false;
            if (!IsPostBack)
            {
                LoadGRN_DDL();
                ddlacount_code.Items.Add(new ListItem("Select", ""));
            }
        }

        private void LoadGRN_DDL()
        {
            string WhereQuery = "DealerCode =  '00001' ";
            //"and a.DelFlag = 'N' " +
            //"and a.VendorCode = b.VendorCode";
            //        "Order by a.PRNo Desc";
            string[] Columns = new string[] { "VendorCode", "VendorDesc" };
            SysFunc.GetMultiColumnsDDL(ddlVendor, Columns, "Vendor", WhereQuery, "VendorCode", " Order by VendorCode Desc", false, false);
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            string strAutoCode = string.Empty;
            try
            {
                if (ddlVendor.SelectedIndex == 0)
                {
                    strAutoCode = SysFunc.GetNewMaxID("Vendor", "VendorCode", 6, "00001");
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
                                };

                param[0].Value = "00001";
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
                param[14].Value = txtpayment_terms.Text;
                param[15].Value = ddlacount_code.SelectedItem.Value;
                param[16].Value = 0;
                param[17].Value = "A";
                param[18].Value = "PARTS";


                SysFunc.ExecuteSP_NonQuery("Sp_Insert_Vendor", param);

                SysFunc.ClearTextBoxes(Page);

                lblmsg.Visible = true;

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
            if (ddlVendor.SelectedIndex > 0)
            {
                try
                {

                    string sql = "delete from Vendor where VendorCode = '" + ddlVendor.SelectedItem.Value + "'";

                    SysFunc.ExecuteQuery_NonQuery(sql);

                    LoadGRN_DDL();

                    SysFunc.ClearTextBoxes(Page);

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert(' Deleted Successfully ');", true);
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
            SysFunc.ClearTextBoxes(Page);
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

                string sql = "select * from Vendor where VendorCode = '" + ddlVendor.SelectedItem.Value + "'";
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
                ddlacount_code.SelectedValue = dt.Rows[0]["AccountCode"].ToString();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('" + ex.Message + "');", true);
            }

        }
    }
}