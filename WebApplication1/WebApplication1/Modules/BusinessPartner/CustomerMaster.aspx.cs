using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxEditors;
using System.Data;
using System.Data.SqlClient;

namespace DXBMS.Modules.Masters
{
    public partial class CustomerMaster : System.Web.UI.Page
    {
        SysFunctions myFunc = new SysFunctions();
        string AutoGenCode;
        string strchk = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            //btnClear_Click(btnClear, EventArgs.Empty);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ASPxEdit.ClearEditorsInContainer(FormLayoutCustomerMaster);
            btnDelete.Enabled = false;
            //btnSave.Enabled = false;
            ddCustomerCode.Enabled = true;
            //ddCustomerCode.DataBind();
            return;
            //ddCustomerLookup.Text = "";
            //cmbCustomerType.Text = "";
            //txtCustomerName.Text = "";
            //txtAddress1.Text = "";
            //txtAddress2.Text = "";
            //txtAddress3.Text = "";
            //txtNICNo.Text = "";
            //txtNTNNo.Text = "";
            //txtSaleTaxRegNo.Text = "";
            //txtDistance.Text = "";

            //txtPhoneNumber.Text = "";
            //txtCellNumber.Text = "";
            //txtFaxNumber.Text = "";
            //txtEmail.Text = "";
            //txtURL.Text = "";

            //spnCreditDays.Text = "";
            //txtCreditLimit.Text = "";
            //txtContactPerson.Text = "";
            //txtFatherHusband.Text = "";

            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                if (myFunc.CodeExists("Customer","DealerCode='"+ Session["DealerCode"].ToString()+"' And CusCode='"+ ddCustomerCode.Text +"' And NIC='" + txtNICNo.Text + "'",Session["DealerCode"].ToString())==false)
                
                {
                    string chkCNIC = string.Empty;
                    ds = new DataSet();
                    myFunc.ExecuteQuery("Select * From Customer Where DealerCode='" + Session["DealerCode"].ToString() + "' And  CusCode<>'" + ddCustomerCode.Text + "'", ref ds);
                        if (ds.Tables[0].Rows.Count != 0)
                    {
                        //if (ds.Tables[0].Rows[0]["NIC"].ToString().Trim() != string.Empty)
                        //{
                            foreach (DataRow dtRow in ds.Tables[0].Rows)
                            {
                                if (dtRow["NIC"].ToString().Trim() != string.Empty)
                                {
                                    if (dtRow["NIC"].ToString() == txtNICNo.Text.Trim())
                                    {
                                        //ObjGenral.UserMsg(lblMsg, Color.Red, "CNIC Number is already exist", txtNIC);
                                        lblMSG.Text = txtNICNo.Text + " CNIC Number is already exist";
                                        PopupControlMSG.ShowOnPageLoad = true;
                                        chkCNIC = "Y";
                                        break;
                                    }
                                }
                            }
                            if (chkCNIC == "Y")
                            {
                                return;
                            }
                       // }
                    }
                }

                if (InsertCustomer())
                {
                    if (ddCustomerCode.Text == "")
                    {
                        lblMSG.Text = "Customer add successfully (" + AutoGenCode + ")";
                    }
                    else
                    {
                        lblMSG.Text = "Customer information updated";
                    }
                    PopupControlMSG.ShowOnPageLoad = true;
                    btnClear_Click(btnClear, EventArgs.Empty);
                    //lblMsg.Text = "Customer Add Successfully '" + AutoGenCode + "'";
                    //lblMsg.ForeColor = Color.Green;
                    //ClearAll();
                }
             }
            catch (Exception ex)
            {
                SendAlert("Error:" + ex.ToString());
            }
        }

        private bool InsertCustomer()
        {
            SqlParameter[] param = {
                               new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                               new SqlParameter("@CusCode",SqlDbType .Char ,8),//1
                               new SqlParameter("@CusDesc",SqlDbType .VarChar ,50),//2
                               new SqlParameter("@FatherHusName",SqlDbType .VarChar ,50),//3
                               //new SqlParameter("@AuxCusCode
                               new SqlParameter("@Address1",SqlDbType .VarChar ,50),//4
                               new SqlParameter("@Address2",SqlDbType .VarChar ,50),//5
                               new SqlParameter("@Address3",SqlDbType .VarChar ,50),//6
                               new SqlParameter("@ContPerson",SqlDbType .VarChar ,50),//7
                               new SqlParameter("@CreditDays",SqlDbType .Int ),//8
                               new SqlParameter("@CreditLimit",SqlDbType .Float ),//9
                               new SqlParameter("@NIC",SqlDbType .VarChar ,50),//10
                               new SqlParameter("@CusTypeCode",SqlDbType .Char ,3),//11
                               new SqlParameter("@DOB",SqlDbType.DateTime ),//12
                               new SqlParameter("@Phone1",SqlDbType .VarChar ,15),//13
                               new SqlParameter("@Phone2",SqlDbType .VarChar ,15),//14
                               new SqlParameter("@CellNo",SqlDbType .VarChar ,15),//15
                               new SqlParameter("@FaxNo",SqlDbType .VarChar ,15),//16
                               new SqlParameter("@Email",SqlDbType .VarChar ,50),//17
                               new SqlParameter("@URL",SqlDbType .VarChar ,50),//18
                               new SqlParameter("@NTNno",SqlDbType .VarChar ,15),//19
                               new SqlParameter("@AdvanceReceipt",SqlDbType .Float ),//20
                               //new SqlParameter("@AccountCode
                               new SqlParameter("@UpdUser",SqlDbType .VarChar ,50),//21
                               //new SqlParameter("@UpdDate",SqlDbType .DateTime ),//22
                               new SqlParameter("@UpdTerm",SqlDbType .VarChar ,50),//23
                               new SqlParameter("@SalesTaxRegNo",SqlDbType .VarChar ,30),//24
                               new SqlParameter("@Behavior",SqlDbType .Char ,6),//25
                               new SqlParameter("@Remarks",SqlDbType .VarChar ,200),//26
                               new SqlParameter("@Distance",SqlDbType.Int )//27
                               };
            param[0].Value = Session["DealerCode"].ToString();
            if (ddCustomerCode.Text.Trim() == string.Empty)
            {
                AutoGenCode = myFunc.GetNewMaxID("Customer", "CusCode", 8,Session["DealerCode"].ToString());
                //txtCustCode.Text = AutoGenCode;
                
                param[1].Value = AutoGenCode;
            }
            else
            {
                param[1].Value = ddCustomerCode.Text;
            }

            param[2].Value = txtCustomerName.Text.Trim().ToUpper();
            param[3].Value = txtFatherHusband.Text.Trim().ToUpper();
            param[4].Value = txtAddress1.Text.Trim().ToUpper();
            param[5].Value = txtAddress2.Text.Trim().ToUpper();
            param[6].Value = txtAddress3.Text.Trim().ToUpper();
            param[7].Value = txtContactPerson.Text.Trim().ToUpper();
            if (spnCreditDays.Text.Trim() == "")
            {
                spnCreditDays.Text = "0";
                param[8].Value = spnCreditDays.Text;
            }
            else
            {
                param[8].Value = spnCreditDays.Text;
            }
            if (txtCreditLimit.Text.Trim() == "")
            {
                txtCreditLimit.Text = "0.00";
                param[9].Value = txtCreditLimit.Text;
            }
            else
            {
                param[9].Value = txtCreditLimit.Text;
            }
            param[10].Value = txtNICNo.Text;
            param[11].Value = cmbCustomerType.Text;
            if (dpDOB.Date.Year > 1900)
            {
                param[12].Value = dpDOB.Date;
            }
            param[13].Value = txtPhoneNumber.Text;
            param[14].Value = txtPhoneNumber2.Text;
            param[15].Value = txtCellNumber.Text;
            param[16].Value = txtFaxNumber.Text;
            param[17].Value = txtEmail.Text;
            param[18].Value = txtURL.Text;
            param[19].Value = txtNTNNo.Text;
            param[20].Value = DBNull.Value;
            param[21].Value = Session["UserName"].ToString();
            //param[22].Value = ObjGenral.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
            param[22].Value = GlobalVar.mUserIPAddress; // SessionInformation.MachineIP;
            param[23].Value = txtSaleTaxRegNo.Text;
            param[24].Value = "GOOD";
            param[25].Value = DBNull.Value;
            if (txtDistance.Text == "")
            {
                txtDistance.Text = "0";
                param[26].Value = txtDistance.Text;
            }
            else
            {
                param[26].Value = txtDistance.Text;
            }


            if (myFunc.ExecuteSP_NonQuery("sp_Sales_Insert_Customer", param))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        protected void cmbCustomerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void btnSelectCustomer_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            try
            {
                if (ddCustomerCode.Text == "")
                {
                    return;
                }
                if (myFunc.CodeExists("Customer", "CusCode", ddCustomerCode.Text,Session["DealerCode"].ToString()))
                {
                    string sql = "select * from Customer where CusCode='" + ddCustomerCode.Text.Trim() + "' and DealerCode='" + Session["DealerCode"].ToString()+"'";
                    dt = myFunc.GetData(sql);
                    if (dt.Rows.Count > 0)
                    {
                        btnDelete.Enabled = true;
                        //ddCustomerCode.Enabled = false;
                        txtCustomerName.Text = dt.Rows[0]["CusDesc"].ToString();                       
                        txtAddress1.Text = dt.Rows[0]["Address1"].ToString();
                        txtAddress2.Text = dt.Rows[0]["Address2"].ToString();
                        txtAddress3.Text = dt.Rows[0]["Address3"].ToString();
                        txtNICNo.Text = dt.Rows[0]["NIC"].ToString();
                        txtNTNNo.Text = dt.Rows[0]["NTNno"].ToString();
                        if (dt.Rows[0]["DOB"].ToString() != "")
                        {
                            dpDOB.Date = Convert.ToDateTime( dt.Rows[0]["DOB"].ToString() );
                            //dpDOB.Date = Convert.ToDateTime( dt.Rows[0]["DOB"].ToString() ).ToString("yyyy/MM/dd"));
                        }

                        txtPhoneNumber.Text = dt.Rows[0]["Phone1"].ToString();
                        txtPhoneNumber2.Text = dt.Rows[0]["Phone2"].ToString();
                        txtFaxNumber.Text = dt.Rows[0]["FaxNo"].ToString();
                        txtEmail.Text = dt.Rows[0]["Email"].ToString();
                        txtURL.Text = dt.Rows[0]["URL"].ToString();
                        spnCreditDays.Text = dt.Rows[0]["CreditDays"].ToString();
                        txtCreditLimit.Text = dt.Rows[0]["CreditLimit"].ToString();
                        txtContactPerson.Text = dt.Rows[0]["ContPerson"].ToString();
                        txtSaleTaxRegNo.Text = dt.Rows[0]["SalesTaxRegNo"].ToString();
                        txtFatherHusband.Text = dt.Rows[0]["FatherHusName"].ToString();
                        txtCellNumber.Text = dt.Rows[0]["CellNo"].ToString();
                        txtDistance.Text = dt.Rows[0]["Distance"].ToString();
                        cmbCustomerType.Text = dt.Rows[0]["CusTypeCode"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                SendAlert("Error:" + ex.ToString());
            }
        }
        public static void SendAlert(string sMessage)
        {
            sMessage = "alert('" + sMessage.Replace("'", @"\'").Replace("\n", @"\n") + "');";

            if (HttpContext.Current.CurrentHandler is Page)
            {
                Page p = (Page)HttpContext.Current.CurrentHandler;

                if (ScriptManager.GetCurrent(p) != null)
                {
                    ScriptManager.RegisterStartupScript(p, typeof(Page), "Message", sMessage, true);
                }
                else
                {
                    p.ClientScript.RegisterStartupScript(typeof(Page), "Message", sMessage, true);
                }
            }
        }

        protected void btnPopOk_Click(object sender, EventArgs e)
        {
            PopupControlMSG.ShowOnPageLoad = false;
        }

        protected void btnPopCancel_Click(object sender, EventArgs e)
        {
            PopupControlMSG.ShowOnPageLoad = false;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //TextBox[] textBoxes = { txtCustCode };
                //if (!MasterValidation(textBoxes))
                //{
                //    return;
                //}

                
                
                if (myFunc.CodeExists("BookOrdMaster", "CusCode", ddCustomerCode.Text.Trim()) == true)
                {
                    lblMSG.Text = ddCustomerCode.Text + "Customer code exist in Booking order";
                    PopupControlMSG.ShowOnPageLoad = true;
                    return;
                }

                // If Customer used in Invoiced name in booking order then should not be delete
                if (myFunc.CodeExists("BookOrdMaster", "DisplayCode", ddCustomerCode.Text.Trim()) == true)
                {
                    lblMSG.Text = ddCustomerCode.Text + "Customer code use as inoviced name in Booking order";
                    PopupControlMSG.ShowOnPageLoad = true;
                    return;
                }

                //If Customer has vehicle in Customer vheicle table then customer should not be delete
                {
                    if (myFunc.CodeExists("CustomerVehicle", "CusCode", ddCustomerCode.Text.Trim()) == true)
                    {
                        lblMSG.Text = ddCustomerCode.Text + "Customer has vehicle, so record can't be deleted";
                        PopupControlMSG.ShowOnPageLoad = true;
                        return;
                    }
                }

                //If Customer used in Jobcard then should not be delete
                {
                    if (myFunc.CodeExists("JobCardMaster", "CusCode", ddCustomerCode.Text.Trim()) == true)
                    {
                        lblMSG.Text = ddCustomerCode.Text + "Customer code exist in jobcard, so record can't be deleted";
                        PopupControlMSG.ShowOnPageLoad = true;
                        return;
                    }
                }

                //If Customer used in Counter then should not be delete
                {
                    if (myFunc.CodeExists("CountersaleMaster", "CusCode", ddCustomerCode.Text.Trim()) == true)
                    {
                        lblMSG.Text = ddCustomerCode.Text + "Customer code exist in Counter Sale, so record can't be deleted";
                        PopupControlMSG.ShowOnPageLoad = true;
                        return;
                    }
                }


                SqlParameter[] param = {
                               new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                               new SqlParameter("@CusCode",SqlDbType.Char,8),//1
                               };
                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddCustomerCode.Text.Trim();
                if (myFunc.ExecuteSP_NonQuery("sp_Sales_Delete_Customer", param))
                {
                    lblMSG.Text = ddCustomerCode.Text + "Customer deleted successfully";
                    PopupControlMSG.ShowOnPageLoad = true;
                    btnClear_Click(btnClear, EventArgs.Empty);
                }
                else
                {
                    lblMSG.Text = ddCustomerCode.Text + "Customer cannot delete";
                    PopupControlMSG.ShowOnPageLoad = true;                    
                }
            }
            catch (Exception ex)
            {
                SendAlert("Error:" + ex.ToString());
            }
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            //txtCustomerName.Text = txtCustomerName.Text.ToUpper();
        }

        
    }
}