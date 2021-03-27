using CConn;
using Microsoft.ApplicationBlocks.Data;
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

namespace DXBMS.Modules.SpareParts.Forms
{
    public partial class CustProfile : System.Web.UI.Page
    {

        MainBLL ObjMainBLL = new MainBLL();
        Transaction ObjTrans = new Transaction();
        SysFunction SysFunc = new SysFunction();
        SysFunctions SysFuncs = new SysFunctions();
        SecurityBll sec = new SecurityBll();
        //clsLookUp ObjLookUp = new clsLookUp();
        DataSet ds = new DataSet();
        string AutoGenCode;
        string strchk = string.Empty;
        clsLookUp clslook = new clsLookUp();
        /// Security Code /////
        public void SecurityImplements()
        {

        }
        // Security Code ///////////////
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2505", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }

            if (Page.IsPostBack)
            {

                if (!string.IsNullOrEmpty((String) Session["LookUpData"]))
                {
                    SelectedPartDetail(Session["LookUpData"].ToString());
                }
            }

            if (!IsPostBack)
            {

                if (Session["CusCode"] != null)
                {
                    ThreadStart childthreat = new ThreadStart(childthreadcall);
                    Thread child = new Thread(childthreat);

                    child.Start();

                    LoadDDL_CustCode();
                    LoadDDL_Country();
                    LoadDDL_State();
                    LoadDDL_City();
                    LoadDDL_CustomerType();
                    ddlType.SelectedIndex = 1;
                    string leadId = Session["CusCode"].ToString();
                    ViewState["CusCode"] = leadId;
                    txtCusCode.Text= ViewState["CusCode"].ToString();
                    ddlcust_code.SelectedValue = ViewState["CusCode"].ToString();
                    ddlcust_code_SelectedIndexChanged(null,null);

                }
                else
                {
                    ThreadStart childthreat = new ThreadStart(childthreadcall);
                    Thread child = new Thread(childthreat);

                    child.Start();

                    LoadDDL_CustCode();
                    LoadDDL_Country();
                    LoadDDL_State();
                    LoadDDL_City();
                    LoadDDL_CustomerType();
                    ddlType.SelectedIndex = 1;
                }
            }

            lblMsg.Visible = false;
            //load lookup data
            //if (Session["LookUpData"] != null)
            //{
            //    GridViewRow Drow = (GridViewRow)Session["LookUpData"];
            //    ddlcust_code.SelectedItem.Value = Drow.Cells[1].Text;
            //    ViewData();
              Session["LookUpData"] = null;
            //}
            Session["CusCode"] = null;
        }

        private void LoadDDL_CustomerType()
        {
            string WhereQuery = "1=1";
            string[] Columns = new string[] { "CusTypeCode", "CusTypeDesc" };
            SysFunc.GetMultiColumnsDDL(ddlType, Columns, "CustomerType", WhereQuery, "CusTypeCode", "", false, false);
        }

        private void LoadDDL_CustCode()
        {
            string WhereQuery = "DealerCode =  '" + Session["DealerCode"] + "'";
            string[] Columns = new string[] { "CusCode", "CusDesc", "NIC" };
            SysFunc.GetMultiColumnsDDL(ddlcust_code, Columns, "Customer", WhereQuery, "CusCode", "Order by CusCode Desc", false, false);
        }

        private void LoadDDL_Country()
        {
            //string WhereQuery = "1=1";
            //string[] Columns = new string[] { "CountryCode", "CountryDesc" };
            //SysFunc.GetMultiColumnsDDL(ddlcountry, Columns, "Country", WhereQuery, "CountryCode", "", false, false);
            ObjMainBLL.FillDropDown(ddlcountry, "Select LTrim(RTrim(CountryCode)) as CountryCode,CountryDesc From Country ", "CountryDesc", "CountryCode", "Select");
        }

        private void LoadDDL_State()
        {
            string WhereQuery = "CountryCode = '" + ddlcountry.SelectedItem.Value + "'";
            //string[] Columns = new string[] { "LTrim(RTrim(StateCode)) as StateCode", "StateDesc" };
            //SysFunc.GetMultiColumnsDDL(ddlstates, Columns, "State", WhereQuery, "StateCode", "", false, false);
            ObjMainBLL.FillDropDown(ddlstates, "Select LTrim(RTrim(StateCode)) as StateCode,StateDesc From State where " + WhereQuery, "StateDesc", "StateCode", "Select");
        }

        private void LoadDDL_City()
        {
            string WhereQuery = "StateCode = '" + ddlstates.SelectedItem.Value + "'";
            //string[] Columns = new string[] { "CityCode", "CityDesc" };
            //SysFunc.GetMultiColumnsDDL(ddlcity, Columns, "City", WhereQuery, "CityCode", "", false, false);
            ObjMainBLL.FillDropDown(ddlcity, "Select LTrim(RTrim(CityCode)) as CityCode,CityDesc From City where " + WhereQuery, "CityDesc", "CityCode", "Select");
        }

        private void ViewData()
        {
            DataSet ds = new DataSet();

            try
            {
                if (ddlcust_code.SelectedIndex < 0)
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = Color.Red;
                    lblMsg.Text = "Select Customer Code";
                    return;
                }
                if (SysFunc.CodeExists("Customer", "CusCode", ddlcust_code.SelectedItem.Value, Session["DealerCode"].ToString(), ref ds))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtCustName.Text = ds.Tables[0].Rows[0]["CusDesc"].ToString();
                        txtAddress1.Text = ds.Tables[0].Rows[0]["Address1"].ToString();
                        txtNIC.Text = ds.Tables[0].Rows[0]["NIC"].ToString();
                        txtNTN.Text = ds.Tables[0].Rows[0]["NTNno"].ToString();
                        if (ds.Tables[0].Rows[0]["DOB"].ToString() != "") txtBirthDT.Text = SysFunc.GetDate(Convert.ToDateTime(ds.Tables[0].Rows[0]["DOB"].ToString()).ToString("yyyy/MM/dd"));
                        txtPhone1.Text = ds.Tables[0].Rows[0]["Phone1"].ToString();
                        txtPhone2.Text = ds.Tables[0].Rows[0]["Phone2"].ToString();
                        txtFaxNo.Text = ds.Tables[0].Rows[0]["FaxNo"].ToString();
                        txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                        txtURL.Text = ds.Tables[0].Rows[0]["URL"].ToString();
                        txtCreditTerm.Text = ds.Tables[0].Rows[0]["CreditDays"].ToString();
                        txtCreditLimt.Text = ds.Tables[0].Rows[0]["CreditLimit"].ToString();
                        txtContPerson.Text = ds.Tables[0].Rows[0]["ContPerson"].ToString();
                        txtSalesTax.Text = ds.Tables[0].Rows[0]["SalesTaxRegNo"].ToString();
                        txtFName.Text = ds.Tables[0].Rows[0]["FatherHusName"].ToString();
                        txtCellNo.Text = ds.Tables[0].Rows[0]["CellNo"].ToString();
                        txtDist.Text = ds.Tables[0].Rows[0]["Distance"].ToString();
                        if (ds.Tables[0].Rows[0]["CusTypeCode"].ToString().Trim() != "001" && ds.Tables[0].Rows[0]["CusTypeCode"].ToString().Trim() != "002")
                        {
                            ddlType.SelectedValue = "001";
                        }
                        else
                        {
                            ddlType.SelectedValue = ds.Tables[0].Rows[0]["CusTypeCode"].ToString().Trim();
                        }
                        

                        ddlcountry.SelectedValue = ds.Tables[0].Rows[0]["CountryCode"].ToString().Trim() == "" ? "PAK" : ds.Tables[0].Rows[0]["CountryCode"].ToString();
                        LoadDDL_State();
                        if (ds.Tables[0].Rows[0]["StateCode"].ToString().Trim() == "" || ds.Tables[0].Rows[0]["StateCode"].ToString() == null)
                        {
                            ddlstates.SelectedValue = "SN";
                        }
                        else
                        {
                            ddlstates.SelectedValue = ds.Tables[0].Rows[0]["StateCode"].ToString().Trim();
                        } 
                        LoadDDL_City();
                        if( ds.Tables[0].Rows[0]["CityCode"].ToString() == "" ||  ds.Tables[0].Rows[0]["CityCode"].ToString() == null)
                        {
                            ddlcity.SelectedValue =  "SLA";
                        }else{
                            ddlcity.SelectedValue = ds.Tables[0].Rows[0]["CityCode"].ToString().Trim();
                        }
                        ddlTitle.SelectedValue = ds.Tables[0].Rows[0]["Title"].ToString().Trim();

                        txtAccountCode.Text = ds.Tables[0].Rows[0]["AccountCode"].ToString().Trim();

                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);
            ClearAll();
            lblImp.Text ="(*) Mandatory";
            
        }

        private void ClearAll()
        {

            //if (strchk != "Y")
            //{
            //    ddlcust_code.SelectedIndex = 0;
            //}

            SysFunc.ClearTextBoxes(Page);
            SysFunc.ClearTextBoxesColor(Page);
            SysFunc.Clearddl(Page);
            Session["CusCode"] = null;
            ddlType.SelectedIndex = 1;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2505", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (txtCusCode.Text != "")
            {
                if (!sec.UserRight("2505", "003"))
                {
                    Response.Redirect("~/Test.aspx");
                }

            }
           
            try
            {
                if (ddlType.SelectedIndex == 0)
                {
                    lblImp.Text = "First Select Customer Type";
                    ddlType.BorderColor = Color.Red;
                    SysFunc.UserMsg(lblImp, Color.Red, "First Select Customer Type", txtCustName);
                    ddlType.Focus();
                    return;
                }
                if (ddlType.SelectedValue == "001" )
                {
                    //if (txtNIC.Text == "")
                    //{
                    //    lblImp.Text = "Please Enter the CNIC";
                    //    txtNIC.BorderColor = Color.Red;
                    //    SysFunc.UserMsg(lblImp, Color.Red, "Please Enter the CNIC No", txtNIC);
                    //    txtNIC.Focus();
                    //    return;
                    //}
                   
                }
                if (ddlType.SelectedValue == "002")
                {
                    if (txtNTN.Text == "")
                    {
                        lblImp.Text = "Please Enter the NTN";
                        txtNTN.BorderColor = Color.Red;
                        SysFunc.UserMsg(lblImp, Color.Red, "Please Enter the NTN No", txtNTN);
                        txtNTN.Focus();
                        return;
                    }

                }

                TextBox[] textBoxes = { txtCustName,txtCellNo };
                if (!MasterValidation(textBoxes))
                {
                    SysFunc.UserMsg(lblImp, Color.Red, "* Fields are mandatory");
                    return; }
               
                if (SysFunc.ValidateDate(txtBirthDT.Text.Trim()) == false)
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = Color.Red;
                    lblMsg.Text = "Enter Valid Date Of Birth";
                    txtBirthDT.Focus();
                    return;
                }
                else
                {
                    AutoGenCode = ddlcust_code.SelectedItem.Value;
                }
                if (txtNIC.Text == "_____-_______-_")
                {
                    txtNIC.Text = "";
                }
                else if (txtNIC.Text.Contains('_'))
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Enter Valid CNIC";
                    lblMsg.ForeColor = Color.Red;
                    txtNIC.Focus();
                    return;
                }
                if (txtSalesTax.Text == "__-__-____-___-__")
                {
                    txtSalesTax.Text = "";
                }
                if(txtEmail.Text !="")
                {
                    if (!txtEmail.Text.Contains('@'))
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Enter valid Email ";
                        lblMsg.ForeColor = Color.Red;
                        txtEmail.Focus();
                        return;
                    }

                }
                if (txtURL.Text != "")
                {
                    if (!txtURL.Text.Contains("WWWW"))
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Enter valid URL ";
                        lblMsg.ForeColor = Color.Red;
                        txtURL.Focus();
                        return;
                    }

                }
                //if (txtCellNo.Text != "")
                //{
                //    if (!txtCellNo.Text.Contains("92"))
                //    {
                //        lblMsg.Visible = true;
                //        lblMsg.Text = "Enter Number Sarting With 92 ";
                //        lblMsg.ForeColor = Color.Red;
                //        txtCellNo.Focus();
                //        return;
                //    }

                //}
                if (txtSalesTax.Text.Contains('_'))
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Enter valid sales tax registration number ";
                    lblMsg.ForeColor = Color.Red;
                    txtSalesTax.Focus();
                    return;
                }
                if (txtNTN.Text == "_______-_")
                {
                    txtNTN.Text = "";
                }
                else if (txtNTN.Text.Contains('_'))
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Enter Valid NTN";
                    lblMsg.ForeColor = Color.Red;
                    txtNTN.Focus();
                    return;
                }
                if (txtCellNo.Text == "____-_______")
                {
                    txtCellNo.Text = "";
                }
                else if (txtCellNo.Text.Contains('_'))
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Enter Valid Contact Number";
                    lblMsg.ForeColor = Color.Red;
                    txtCellNo.Focus();
                    return;
                }
                if (txtAddress1.Text != "")
                {
                    if (ddlcountry.SelectedIndex == 0)
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Please Select the Country";
                        lblMsg.ForeColor = Color.Red;
                        ddlcountry.Focus();
                        return;
                    }
                    if (ddlstates.SelectedIndex == 0)
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Please Select the State";
                        lblMsg.ForeColor = Color.Red;
                        ddlstates.Focus();
                        return;
                    }
                    if (ddlcity.SelectedIndex == 0)
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Please Select the City";
                        lblMsg.ForeColor = Color.Red;
                        ddlcity.Focus();
                        return;
                    }
                }
                //DataSet ds = new DataSet();
                //if (ObjGenral.CodeExists("Customer", "DealerCode='" + Session["DealerCode"] + "' And CusCode='" + ddlcust_code.SelectedItem.Value + "' And NIC='" + txtNIC.Text + "'", ref ds) == false)
                //{
                //    string chkCNIC = string.Empty;
                //    ds = new DataSet();
                //    SqlDataReader dr = null;
                //    ObjGenral.ExecuteQuery("Select * From Customer Where DealerCode='" + Session["DealerCode"] + "' And  CusCode<>'" + ddlcust_code.SelectedItem.Value + "'", ref dr);
                //    if (ds.Tables[0].Rows.Count != 0)
                //    {
                //        foreach (DataRow dtRow in ds.Tables[0].Rows)
                //        {
                //            if (dtRow["NIC"].ToString().Trim() != string.Empty)
                //            {
                //                if (dtRow["NIC"].ToString() == txtNIC.Text.Trim())
                //                {
                //                    lblMsg.Text = "CNIC Number is already exist";
                //                    chkCNIC = "Y";
                //                    break;
                //                }
                //            }
                //        }
                //        if (chkCNIC == "Y") return;
                //    }
                //}

                if (InsertCustomer())
                {
                    lblMsg.Visible = true;

                    if (ddlcust_code.SelectedIndex == 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);
                        //  lblMsg.Text = "Customer Added Successfully '" + AutoGenCode + "'";
                        //  string script = "window.onload = function(){ alert('";
                        //  script += lblMsg.Text;
                        //  script += "')};";
                        //  ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
                        // // lblMsg.Text = "Customer Added Successfully '" + AutoGenCode + "'";
                        ////  Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                      //  lblMsg.Text = "Customer Edited Successfully '" + AutoGenCode + "'";
                       
                        //  ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Updatealert()", true);
                       // Response.Redirect("CustProfile.aspx");
                         //Response.Redirect(Request.RawUrl);
                    }

                    
                    lblMsg.ForeColor = Color.Green;
                    ClearAll();
                    LoadDDL_CustCode();
                   // Response.Redirect("CustProfile.aspx");
                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;
            }
        }

        //protected void btnCustCodeLookup_Click(object sender, ImageClickEventArgs e)
        //{
        //    ViewState["lookupid"] = 1;
        //    ViewState["ixd1"] = 1;
        //    ViewState["ixd2"] = 1;
        //    ViewState["txtgroup"] = 2;
        //    //ObjLookUp.LU_Get_Customer(btnCustCodeLookup, ViewState["lookupid"].ToString(), string.Empty, "../../../");
        //}

        //void lookUptext(string lookUpID)
        //{
        //    switch (lookUpID)
        //    {
        //        case "1":
        //            GetLookUpsValue(ref txtCustCode, ref txtCustCode, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString());
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //void GetLookUpsValue(ref TextBox txt1, ref TextBox txt2, string ixd1, string ixd2)
        //{
        //    if (Session["LookUpData"] != null)
        //    {
        //        GridViewRow Drow = (GridViewRow)Session["LookUpData"];
        //        txt1.Text = (txt1 != null ? Drow.Cells[Convert.ToInt32(ixd1)].Text : "");
        //        txt2.Text = (txt2 != null ? Drow.Cells[Convert.ToInt32(ixd2)].Text : "");
        //        Session["LookUpData"] = null;
        //    }
        //}

        private bool InsertCustomer()
        {
            SqlParameter[] param = {
                               new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                               new SqlParameter("@CusCode",SqlDbType .Char ,8),//1
                               new SqlParameter("@CusDesc",SqlDbType .VarChar ,50),//2
                               new SqlParameter("@FatherHusName",SqlDbType .VarChar ,50),//3
                               new SqlParameter("@Address1",SqlDbType .VarChar ,50),//4
                               new SqlParameter("@Address2",SqlDbType .VarChar ,50),//5
                               new SqlParameter("@Address3",SqlDbType .VarChar ,50),//6
                               new SqlParameter("@ContPerson",SqlDbType .VarChar ,50),//7
                               new SqlParameter("@CreditDays",SqlDbType .Int ),//8
                               new SqlParameter("@CreditLimit",SqlDbType .Float ),//9
                               new SqlParameter("@NIC",SqlDbType .VarChar ,50),//10
                               new SqlParameter("@CusTypeCode",SqlDbType .VarChar ,8),//11
                               new SqlParameter("@DOB",SqlDbType.DateTime ),//12
                               new SqlParameter("@Phone1",SqlDbType .VarChar ,15),//13
                               new SqlParameter("@Phone2",SqlDbType .VarChar ,15),//14
                               new SqlParameter("@CellNo",SqlDbType .VarChar ,15),//15
                               new SqlParameter("@FaxNo",SqlDbType .VarChar ,15),//16
                               new SqlParameter("@Email",SqlDbType .VarChar ,50),//17
                               new SqlParameter("@URL",SqlDbType .VarChar ,50),//18
                               new SqlParameter("@NTNno",SqlDbType .VarChar ,15),//19
                               new SqlParameter("@AdvanceReceipt",SqlDbType .Float ),//20
                               new SqlParameter("@UpdUser",SqlDbType .VarChar ,50),//21
                               new SqlParameter("@UpdTerm",SqlDbType .VarChar ,50),//22
                               new SqlParameter("@SalesTaxRegNo",SqlDbType .VarChar ,30),//23
                               new SqlParameter("@Behavior",SqlDbType .Char ,6),//24
                               new SqlParameter("@Remarks",SqlDbType .VarChar ,200),//25
                               new SqlParameter("@Distance",SqlDbType.Int),//26
                               new SqlParameter("@CountryCode", SqlDbType.Char),//27
                               new SqlParameter("@CityCode", SqlDbType.Char),//28
                               new SqlParameter("@StateCode", SqlDbType.Char),//29
                               new SqlParameter("@Title", SqlDbType.VarChar , 3),//30
                               new SqlParameter("@AccountCode", SqlDbType.VarChar , 16),//31
                                //new SqlParameter("@Source", SqlDbType.VarChar , 15)//31
                               };

            param[0].Value = Session["DealerCode"];

            if (ddlcust_code.SelectedIndex == 0)
            {
                AutoGenCode = SysFunc.GetNewMaxID("Customer", "CusCode", 8, Session["DealerCode"].ToString());
                param[1].Value = AutoGenCode;
            }
            else param[1].Value = ddlcust_code.SelectedItem.Value;
            param[2].Value = txtCustName.Text.Trim().ToUpper();
            param[3].Value = txtFName.Text.Trim().ToUpper();
            param[4].Value = txtAddress1.Text.Trim().ToUpper();
            param[5].Value = (object)DBNull.Value;
            param[6].Value = (object)DBNull.Value;
            param[7].Value = txtContPerson.Text.Trim().ToUpper();
            if (txtCreditTerm.Text.Trim() == "")
            {
                txtCreditTerm.Text = "0";
                param[8].Value = txtCreditTerm.Text;
            }
            else param[8].Value = txtCreditTerm.Text;
            if (txtCreditLimt.Text.Trim() == "")
            {
                txtCreditLimt.Text = "0.00";
                param[9].Value = txtCreditLimt.Text;
            }
            else param[9].Value = txtCreditLimt.Text;
            param[10].Value = txtNIC.Text;
            param[11].Value = ddlType.SelectedValue.ToString().Trim();
            param[12].Value = SysFunc.SaveDate(txtBirthDT.Text);
            param[13].Value = txtPhone1.Text;
            param[14].Value = txtPhone2.Text;
            param[15].Value = txtCellNo.Text;
            param[16].Value = txtFaxNo.Text;
            param[17].Value = txtEmail.Text;
            param[18].Value = txtURL.Text;
            param[19].Value = txtNTN.Text;
            param[20].Value = DBNull.Value;
            param[21].Value = Session["UserName"];
            param[22].Value = GlobalVar.mUserIPAddress;
            param[23].Value = txtSalesTax.Text;
            param[24].Value = "GOOD";
            param[25].Value = DBNull.Value;
            if (txtDist.Text == "")
            {
                txtDist.Text = "0";
                param[26].Value = txtDist.Text;
            }
            else param[26].Value = txtDist.Text;

            param[27].Value = ddlcountry.SelectedItem.Value;
            param[28].Value = ddlcity.SelectedItem.Value;
            param[29].Value = ddlstates.SelectedItem.Value;
            param[30].Value = ddlTitle.SelectedValue.ToString().Trim();
            param[31].Value = txtAccountCode.Text;
            if (SysFunc.ExecuteSP_NonQuery("sp_Sales_Insert_Customer", param)) return true;
            else
                return false;
        }

        protected void btnDelete0_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2505", "004"))
            {
                Response.Redirect("~/Test.aspx");
            }
            try
            {
                //TextBox[] textBoxes = { ddlcust_code.SelectedItem.Value };
                //if (!MasterValidation(textBoxes)) return;

                if (SysFunc.IsExist("CusCode", ddlcust_code.SelectedItem.Value, "Customer", Session["DealerCode"].ToString(), "") == false)
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = Color.Red;
                    lblMsg.Text = "Customer Code dose not exist";
                    return;
                }
                if (SysFunc.IsExist("CusCode", ddlcust_code.SelectedItem.Value, "BookOrdMaster", Session["DealerCode"].ToString(), "") == true)
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = Color.Red;
                    lblMsg.Text = "Customer code exist in Booking order";
                    return;
                }

                // If Customer used in Invoiced name in booking order then should not be delete
                if (SysFunc.IsExist("DisplayCode", ddlcust_code.SelectedItem.Value.Trim(), "BookOrdMaster", Session["DealerCode"].ToString(), "") == true)
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = Color.Red;
                    lblMsg.Text = "Customer code use as inoviced name in Booking order";
                    return;
                }

                //If Customer has vehicle in Customer vheicle table then customer should not be delete
                //{
                if (SysFunc.IsExist("CusCode", ddlcust_code.SelectedItem.Value.Trim(), "CustomerVehicle", Session["DealerCode"].ToString(), "") == true)
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = Color.Red;
                    lblMsg.Text = "Customer has vehicle, so record can't be deleted";
                    return;
                }
                //}
                //If Customer used in Estimate then should not be delete
                //{
                if (SysFunc.IsExist("CusCode", ddlcust_code.SelectedItem.Value.Trim(), "CustomerEstimateMaster", Session["DealerCode"].ToString(), "") == true)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Customer code exist in Estimate , so record can't be deleted";
                    return;
                }

                //If Customer used in Jobcard then should not be delete
                //{
                if (SysFunc.IsExist("CusCode", ddlcust_code.SelectedItem.Value.Trim(), "JobCardMaster", Session["DealerCode"].ToString(), "") == true)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Customer code exist in jobcard, so record can't be deleted";
                    return;
                }
                //}
                //If Customer used in Counter then should not be delete
                //{
                if (SysFunc.IsExist("CusCode", ddlcust_code.SelectedItem.Value.Trim(), "CountersaleMaster", Session["DealerCode"].ToString(), "") == true)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Customer code exist in Counter Sale, so record can't be deleted";
                    return;
                }
                //}


                SqlParameter[] param = {
                               new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                               new SqlParameter("@CusCode",SqlDbType.Char,8),//1
                               };
                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlcust_code.SelectedItem.Value;

                if (SysFunc.ExecuteSP_NonQuery("sp_Sales_Delete_Customer", param))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Deletealert()", true);
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = Color.Green;
                  //  lblMsg.Text = "Customer Deleted successfly";
                    ClearAll();
                    LoadDDL_CustCode();
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = Color.Red;
                    lblMsg.Text = "Customer Not Deleted";
                }

            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;
            }
        }

        public bool MasterValidation(TextBox[] textBoxes)
        {
            bool isValid = true;
            for (int i = 0; i < textBoxes.Length; i++)
            {
                if (textBoxes[i].Text == "")
                {
                    isValid = false;
                    textBoxes[i].BorderColor = System.Drawing.Color.Red;
                    SetFocus(textBoxes[i]);
                    
                    break;
                }
                else { textBoxes[i].BorderColor = System.Drawing.ColorTranslator.FromHtml("#1A6297"); }
            }
            return isValid;
        }

        protected void ddlcust_code_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlcust_code.SelectedIndex == 0)
            {
                ClearAll();
            }
            else
            {
                ViewData();
               txtAccountDesc.Text= GetAccounttitle(txtAccountCode.Text);
            }
        }

        protected void ddlcountry_SelectedIndexChanged(object sender, EventArgs e)
        {

            LoadDDL_State();

        }
        protected void ddlstates_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDDL_City();
        }

        protected void imgLookup_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 75;
            clslook.LU_Get_AccountCode(imgLookup, ViewState["lookupid"].ToString(), "CompCode = '"+Session["DealerCode"].ToString()+"'", "../../");

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
               
                if (ViewState["lookupid"].ToString() == "1")
                {
                    txtCusCode.Text = item;
                    ddlcust_code.SelectedValue = item;
                    ddlcust_code_SelectedIndexChanged(null, null);
                }
                else
                {
                    txtAccountCode.Text = item;
                    txtAccountDesc.Text = GetAccounttitle(item);
                }
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, ex.Message);
            }
        }

        private string GetAccounttitle(string code)
        {
            try
            {
                string CCon = CConnection.GetConnStringForAccount();
                string value = "";

                SqlDataAdapter dta = new SqlDataAdapter("Select rtrim(A.DetailDesc) as AccountTitle from GDetail  A where A.CompCode = '" + Session["DealerCode"].ToString() + "' and  A.contacccode +'-'+  A.SubCode +'-'+  A.subsubcode +'-'+  A.loccode +'-'+  A.DetailCode = '" + code + "'", CCon);

                DataTable dt = new DataTable();
                dta.Fill(dt);

                if (dt.Rows.Count > 0 && dt != null)
                {
                    value = dt.Rows[0]["AccountTitle"].ToString();
                }


                return value;
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, ex.Message);
                return "";
            }

        }
        protected void txtCustName_TextChanged(object sender, EventArgs e)
        {
            
            if (ddlType.SelectedValue.ToString()=="002") {
                DataTable dt;
                string sql = "Select CusDesc From Customer ";
                dt = SysFunc.GetData(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    if (txtCustName.Text.ToUpper() == dr["CusDesc"].ToString().ToUpper())
                    {
                        lblMsg.Text = "This Customer is Already Exists";
                        SysFunc.UserMsg(lblMsg, Color.Red, "This Customer is Already Exists", txtCustName);
                        return ;
                    }
                }
           
            }
            txtCustName.BorderColor = Color.Blue;
            txtCustName.Focus();
            lblMsg.Text = "";
        }

        protected void txtNIC_TextChanged(object sender, EventArgs e)
        {
           
            if (ddlType.SelectedIndex == 0)
            {
                lblImp.Text = "First Select Customer Type";
                ddlType.BorderColor = Color.Red;
                SysFunc.UserMsg(lblImp, Color.Red, "First Select Customer Type", txtCustName);
                return;
            }
            if (ddlType.SelectedValue.ToString() == "001")
               
            {
                if (SysFunc.IsExist("NIC",txtNIC.Text,"Customer",Session["DealerCode"].ToString()))
                    {

                    DataSet ds = new DataSet();

                    try
                    {
                        
                        if (SysFunc.CodeExists("Customer", "NIC",txtNIC.Text, Session["DealerCode"].ToString(), ref ds))
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                ddlcust_code.SelectedValue= ds.Tables[0].Rows[0]["CusCode"].ToString();
                                txtCusCode.Text = ds.Tables[0].Rows[0]["CusCode"].ToString();
                                txtCustName.Text = ds.Tables[0].Rows[0]["CusDesc"].ToString();
                                txtAddress1.Text = ds.Tables[0].Rows[0]["Address1"].ToString();
                                txtNIC.Text = ds.Tables[0].Rows[0]["NIC"].ToString();
                                txtNTN.Text = ds.Tables[0].Rows[0]["NTNno"].ToString();
                                if (ds.Tables[0].Rows[0]["DOB"].ToString() != "") txtBirthDT.Text = SysFunc.GetDate(Convert.ToDateTime(ds.Tables[0].Rows[0]["DOB"].ToString()).ToString("yyyy/MM/dd"));
                                txtPhone1.Text = ds.Tables[0].Rows[0]["Phone1"].ToString();
                                txtPhone2.Text = ds.Tables[0].Rows[0]["Phone2"].ToString();
                                txtFaxNo.Text = ds.Tables[0].Rows[0]["FaxNo"].ToString();
                                txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                                txtURL.Text = ds.Tables[0].Rows[0]["URL"].ToString();
                                txtCreditTerm.Text = ds.Tables[0].Rows[0]["CreditDays"].ToString();
                                txtCreditLimt.Text = ds.Tables[0].Rows[0]["CreditLimit"].ToString();
                                txtContPerson.Text = ds.Tables[0].Rows[0]["ContPerson"].ToString();
                                txtSalesTax.Text = ds.Tables[0].Rows[0]["SalesTaxRegNo"].ToString();
                                txtFName.Text = ds.Tables[0].Rows[0]["FatherHusName"].ToString();
                                txtCellNo.Text = ds.Tables[0].Rows[0]["CellNo"].ToString();
                                txtDist.Text = ds.Tables[0].Rows[0]["Distance"].ToString();
                                if (ds.Tables[0].Rows[0]["CusTypeCode"].ToString().Trim() != "001" && ds.Tables[0].Rows[0]["CusTypeCode"].ToString().Trim() != "002")
                                {
                                    ddlType.SelectedValue = "001";
                                }
                                else
                                {
                                    ddlType.SelectedValue = ds.Tables[0].Rows[0]["CusTypeCode"].ToString().Trim();
                                }


                                ddlcountry.SelectedValue = ds.Tables[0].Rows[0]["CountryCode"].ToString().Trim() == "" ? "PAK" : ds.Tables[0].Rows[0]["CountryCode"].ToString();
                                LoadDDL_State();
                                if (ds.Tables[0].Rows[0]["StateCode"].ToString().Trim() == "" || ds.Tables[0].Rows[0]["StateCode"].ToString() == null)
                                {
                                    ddlstates.SelectedValue = "SN";
                                }
                                else
                                {
                                    ddlstates.SelectedValue = ds.Tables[0].Rows[0]["StateCode"].ToString().Trim();
                                }
                                LoadDDL_City();
                                if (ds.Tables[0].Rows[0]["CityCode"].ToString() == "" || ds.Tables[0].Rows[0]["CityCode"].ToString() == null)
                                {
                                    ddlcity.SelectedValue = "SLA";
                                }
                                else
                                {
                                    ddlcity.SelectedValue = ds.Tables[0].Rows[0]["CityCode"].ToString().Trim();
                                }
                                ddlTitle.SelectedValue = ds.Tables[0].Rows[0]["Title"].ToString().Trim();

                                txtAccountCode.Text = ds.Tables[0].Rows[0]["AccountCode"].ToString().Trim();

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMsg.Visible = true;
                        lblMsg.ForeColor = Color.Red;
                        lblMsg.Text = ex.Message;
                    }
                    lblImp.Text = "This Customer is Already Exists";
                        txtNIC.BorderColor = Color.Red;
                        SysFunc.UserMsg(lblImp, Color.Red, "This Customer is Already Exists", txtCustName);
                        return;
                    }
               else if (SysFunc.IsExistWithoutDealerCode("NIC", txtNIC.Text, "Customer",""))
                {

                    DataSet ds = new DataSet();

                    try
                    {

                        if (SysFunc.CodeExistsWitoutDealerCode("Customer", "NIC", txtNIC.Text, "", ref ds))
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                
                                txtCustName.Text = ds.Tables[0].Rows[0]["CusDesc"].ToString();
                                txtAddress1.Text = ds.Tables[0].Rows[0]["Address1"].ToString();
                                txtNIC.Text = ds.Tables[0].Rows[0]["NIC"].ToString();
                                txtNTN.Text = ds.Tables[0].Rows[0]["NTNno"].ToString();
                                if (ds.Tables[0].Rows[0]["DOB"].ToString() != "") txtBirthDT.Text = SysFunc.GetDate(Convert.ToDateTime(ds.Tables[0].Rows[0]["DOB"].ToString()).ToString("yyyy/MM/dd"));
                                txtPhone1.Text = ds.Tables[0].Rows[0]["Phone1"].ToString();
                                txtPhone2.Text = ds.Tables[0].Rows[0]["Phone2"].ToString();
                                txtFaxNo.Text = ds.Tables[0].Rows[0]["FaxNo"].ToString();
                                txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                                txtURL.Text = ds.Tables[0].Rows[0]["URL"].ToString();
                                txtCreditTerm.Text = ds.Tables[0].Rows[0]["CreditDays"].ToString();
                                txtCreditLimt.Text = ds.Tables[0].Rows[0]["CreditLimit"].ToString();
                                txtContPerson.Text = ds.Tables[0].Rows[0]["ContPerson"].ToString();
                                txtSalesTax.Text = ds.Tables[0].Rows[0]["SalesTaxRegNo"].ToString();
                                txtFName.Text = ds.Tables[0].Rows[0]["FatherHusName"].ToString();
                                txtCellNo.Text = ds.Tables[0].Rows[0]["CellNo"].ToString();
                                txtDist.Text = ds.Tables[0].Rows[0]["Distance"].ToString();
                                if (ds.Tables[0].Rows[0]["CusTypeCode"].ToString().Trim() != "001" && ds.Tables[0].Rows[0]["CusTypeCode"].ToString().Trim() != "002")
                                {
                                    ddlType.SelectedValue = "001";
                                }
                                else
                                {
                                    ddlType.SelectedValue = ds.Tables[0].Rows[0]["CusTypeCode"].ToString().Trim();
                                }


                                ddlcountry.SelectedValue = ds.Tables[0].Rows[0]["CountryCode"].ToString().Trim() == "" ? "PAK" : ds.Tables[0].Rows[0]["CountryCode"].ToString();
                                LoadDDL_State();
                                if (ds.Tables[0].Rows[0]["StateCode"].ToString().Trim() == "" || ds.Tables[0].Rows[0]["StateCode"].ToString() == null)
                                {
                                    ddlstates.SelectedValue = "SN";
                                }
                                else
                                {
                                    ddlstates.SelectedValue = ds.Tables[0].Rows[0]["StateCode"].ToString().Trim();
                                }
                                LoadDDL_City();
                                if (ds.Tables[0].Rows[0]["CityCode"].ToString() == "" || ds.Tables[0].Rows[0]["CityCode"].ToString() == null)
                                {
                                    ddlcity.SelectedValue = "SLA";
                                }
                                else
                                {
                                    ddlcity.SelectedValue = ds.Tables[0].Rows[0]["CityCode"].ToString().Trim();
                                }
                                ddlTitle.SelectedValue = ds.Tables[0].Rows[0]["Title"].ToString().Trim();

                                txtAccountCode.Text = ds.Tables[0].Rows[0]["AccountCode"].ToString().Trim();

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMsg.Visible = true;
                        lblMsg.ForeColor = Color.Red;
                        lblMsg.Text = ex.Message;
                    }
                    lblImp.Text = "This Customer is Already Exists in a Network";
                    txtNIC.BorderColor = Color.Red;
                    SysFunc.UserMsg(lblImp, Color.Red, "This Customer is Already Exists", txtCustName);
                    return;
                }

            }

            txtNIC.BorderColor = Color.Blue;
            txtNIC.Focus();
            lblMsg.Text = "";
        }

        protected void txtNTN_TextChanged(object sender, EventArgs e)
        {
           
            if (ddlType.SelectedIndex == 0)
            {
                lblImp.Text = "First Select Customer Type";
                txtNTN.BorderColor = Color.Red;
                SysFunc.UserMsg(lblImp, Color.Red, "This Customer is Already Exists", txtCustName);
                return;
            }
         if (ddlType.SelectedValue.ToString() == "002")
            {
                 if (SysFunc.IsExist("NIC", txtNIC.Text, "Customer", Session["DealerCode"].ToString()))
                    {

                        DataSet ds = new DataSet();

                        try
                        {

                            if (SysFunc.CodeExists("Customer", "NTNno", txtNTN.Text, Session["DealerCode"].ToString(), ref ds))
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                   ddlcust_code.SelectedValue=ds.Tables[0].Rows[0]["CusCode"].ToString();
                                    txtCusCode.Text = ds.Tables[0].Rows[0]["CusCode"].ToString();
                                    txtCustName.Text = ds.Tables[0].Rows[0]["CusDesc"].ToString();
                                    txtAddress1.Text = ds.Tables[0].Rows[0]["Address1"].ToString();
                                    txtNIC.Text = ds.Tables[0].Rows[0]["NIC"].ToString();
                                    txtNTN.Text = ds.Tables[0].Rows[0]["NTNno"].ToString();
                                    if (ds.Tables[0].Rows[0]["DOB"].ToString() != "") txtBirthDT.Text = SysFunc.GetDate(Convert.ToDateTime(ds.Tables[0].Rows[0]["DOB"].ToString()).ToString("yyyy/MM/dd"));
                                    txtPhone1.Text = ds.Tables[0].Rows[0]["Phone1"].ToString();
                                    txtPhone2.Text = ds.Tables[0].Rows[0]["Phone2"].ToString();
                                    txtFaxNo.Text = ds.Tables[0].Rows[0]["FaxNo"].ToString();
                                    txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                                    txtURL.Text = ds.Tables[0].Rows[0]["URL"].ToString();
                                    txtCreditTerm.Text = ds.Tables[0].Rows[0]["CreditDays"].ToString();
                                    txtCreditLimt.Text = ds.Tables[0].Rows[0]["CreditLimit"].ToString();
                                    txtContPerson.Text = ds.Tables[0].Rows[0]["ContPerson"].ToString();
                                    txtSalesTax.Text = ds.Tables[0].Rows[0]["SalesTaxRegNo"].ToString();
                                    txtFName.Text = ds.Tables[0].Rows[0]["FatherHusName"].ToString();
                                    txtCellNo.Text = ds.Tables[0].Rows[0]["CellNo"].ToString();
                                    txtDist.Text = ds.Tables[0].Rows[0]["Distance"].ToString();
                                    if (ds.Tables[0].Rows[0]["CusTypeCode"].ToString().Trim() != "001" && ds.Tables[0].Rows[0]["CusTypeCode"].ToString().Trim() != "002")
                                    {
                                        ddlType.SelectedValue = "001";
                                    }
                                    else
                                    {
                                        ddlType.SelectedValue = ds.Tables[0].Rows[0]["CusTypeCode"].ToString().Trim();
                                    }


                                    ddlcountry.SelectedValue = ds.Tables[0].Rows[0]["CountryCode"].ToString().Trim() == "" ? "PAK" : ds.Tables[0].Rows[0]["CountryCode"].ToString();
                                    LoadDDL_State();
                                    if (ds.Tables[0].Rows[0]["StateCode"].ToString().Trim() == "" || ds.Tables[0].Rows[0]["StateCode"].ToString() == null)
                                    {
                                        ddlstates.SelectedValue = "SN";
                                    }
                                    else
                                    {
                                        ddlstates.SelectedValue = ds.Tables[0].Rows[0]["StateCode"].ToString().Trim();
                                    }
                                    LoadDDL_City();
                                    if (ds.Tables[0].Rows[0]["CityCode"].ToString() == "" || ds.Tables[0].Rows[0]["CityCode"].ToString() == null)
                                    {
                                        ddlcity.SelectedValue = "SLA";
                                    }
                                    else
                                    {
                                        ddlcity.SelectedValue = ds.Tables[0].Rows[0]["CityCode"].ToString().Trim();
                                    }
                                    ddlTitle.SelectedValue = ds.Tables[0].Rows[0]["Title"].ToString().Trim();

                                    txtAccountCode.Text = ds.Tables[0].Rows[0]["AccountCode"].ToString().Trim();

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            lblMsg.Visible = true;
                            lblMsg.ForeColor = Color.Red;
                            lblMsg.Text = ex.Message;
                        }
                        lblImp.Text = "This Customer is Already Exists";
                        txtNTN.BorderColor = Color.Red;
                        SysFunc.UserMsg(lblImp, Color.Red, "This Customer is Already Exists", txtCustName);
                        return;
                    }

               else if (SysFunc.IsExistWithoutDealerCode("NTNno", txtNTN.Text, "Customer", ""))
                {

                    DataSet ds = new DataSet();

                    try
                    {

                        if (SysFunc.CodeExistsWitoutDealerCode("Customer", "NTNno", txtNTN.Text, "", ref ds))
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                               
                                txtCustName.Text = ds.Tables[0].Rows[0]["CusDesc"].ToString();
                                txtAddress1.Text = ds.Tables[0].Rows[0]["Address1"].ToString();
                                txtNIC.Text = ds.Tables[0].Rows[0]["NIC"].ToString();
                                txtNTN.Text = ds.Tables[0].Rows[0]["NTNno"].ToString();
                                if (ds.Tables[0].Rows[0]["DOB"].ToString() != "") txtBirthDT.Text = SysFunc.GetDate(Convert.ToDateTime(ds.Tables[0].Rows[0]["DOB"].ToString()).ToString("yyyy/MM/dd"));
                                txtPhone1.Text = ds.Tables[0].Rows[0]["Phone1"].ToString();
                                txtPhone2.Text = ds.Tables[0].Rows[0]["Phone2"].ToString();
                                txtFaxNo.Text = ds.Tables[0].Rows[0]["FaxNo"].ToString();
                                txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                                txtURL.Text = ds.Tables[0].Rows[0]["URL"].ToString();
                                txtCreditTerm.Text = ds.Tables[0].Rows[0]["CreditDays"].ToString();
                                txtCreditLimt.Text = ds.Tables[0].Rows[0]["CreditLimit"].ToString();
                                txtContPerson.Text = ds.Tables[0].Rows[0]["ContPerson"].ToString();
                                txtSalesTax.Text = ds.Tables[0].Rows[0]["SalesTaxRegNo"].ToString();
                                txtFName.Text = ds.Tables[0].Rows[0]["FatherHusName"].ToString();
                                txtCellNo.Text = ds.Tables[0].Rows[0]["CellNo"].ToString();
                                txtDist.Text = ds.Tables[0].Rows[0]["Distance"].ToString();
                                if (ds.Tables[0].Rows[0]["CusTypeCode"].ToString().Trim() != "001" && ds.Tables[0].Rows[0]["CusTypeCode"].ToString().Trim() != "002")
                                {
                                    ddlType.SelectedValue = "001";
                                }
                                else
                                {
                                    ddlType.SelectedValue = ds.Tables[0].Rows[0]["CusTypeCode"].ToString().Trim();
                                }


                                ddlcountry.SelectedValue = ds.Tables[0].Rows[0]["CountryCode"].ToString().Trim() == "" ? "PAK" : ds.Tables[0].Rows[0]["CountryCode"].ToString();
                                LoadDDL_State();
                                if (ds.Tables[0].Rows[0]["StateCode"].ToString().Trim() == "" || ds.Tables[0].Rows[0]["StateCode"].ToString() == null)
                                {
                                    ddlstates.SelectedValue = "SN";
                                }
                                else
                                {
                                    ddlstates.SelectedValue = ds.Tables[0].Rows[0]["StateCode"].ToString().Trim();
                                }
                                LoadDDL_City();
                                if (ds.Tables[0].Rows[0]["CityCode"].ToString() == "" || ds.Tables[0].Rows[0]["CityCode"].ToString() == null)
                                {
                                    ddlcity.SelectedValue = "SLA";
                                }
                                else
                                {
                                    ddlcity.SelectedValue = ds.Tables[0].Rows[0]["CityCode"].ToString().Trim();
                                }
                                ddlTitle.SelectedValue = ds.Tables[0].Rows[0]["Title"].ToString().Trim();

                                txtAccountCode.Text = ds.Tables[0].Rows[0]["AccountCode"].ToString().Trim();

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMsg.Visible = true;
                        lblMsg.ForeColor = Color.Red;
                        lblMsg.Text = ex.Message;
                    }
                    lblImp.Text = "This Customer is Already Exists";
                    txtNTN.BorderColor = Color.Red;
                    SysFunc.UserMsg(lblImp, Color.Red, "This Customer is Already Exists in a Network", txtCustName);
                    return;
                }



            }
            txtNTN.BorderColor = Color.Blue;
            txtNTN.Focus();
            lblMsg.Text = "";
        }

        protected void imgCustomerCode_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 1;
            clslook.LU_Get_Customer(imgCustomerCode, ViewState["lookupid"].ToString(), "DealerCode = '" + Session["DealerCode"].ToString() + "'", "../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void txtCellNo_TextChanged(object sender, EventArgs e)
        {
            txtCellNo.BorderColor = Color.Blue;
            txtCellNo.Focus();
            lblMsg.Text = "";
        }
    }
}