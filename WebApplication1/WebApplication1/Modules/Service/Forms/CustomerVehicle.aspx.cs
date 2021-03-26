using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace DXBMS.Modules.Service.Forms
{
    public partial class CustomerVehicle : System.Web.UI.Page
    {

        SysFunction sysFun = new SysFunction();
        SysFunctions sysFuns = new SysFunctions();
        SecurityBll sec = new SecurityBll();
        clsLookUp clslook = new clsLookUp();
        DataSet ds;
        string AutoGenCode;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2530", "001"))
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

                ViewState["CusCode"] = "";

                if (Session["CusCode"] != null)
                {
                    createLubDT();
                    LoadDDLs();

                    string leadId = Session["CusCode"].ToString();
                    ViewState["CusCode"] = leadId;
                    ddlCustomer.SelectedValue = ViewState["CusCode"].ToString();
                   
                    LoadData();

                }
                else
                {
                    txtChassisNo.Enabled = true;
                    txtEngineNo.Enabled = true;
                    txtRegNo.Enabled = true;
                    createLubDT();
                    LoadDDLs();
                }
            }
            Session["LookUpData"] = null;
            Session["CusCode"] = null;
        }
        public void LoadProductddl()
        {
            sysFun.FillDropDown(ddlProduct, "SELECT Distinct  ProdCode,ProdDesc FROM Vehicle Where DealerCode IN('" + Session["DealerCode"].ToString() + "', 'COMON') and VehicleCategory in('" + Session["VehicleCategory"].ToString() + "') and BrandCode = '" + ddlBrand.SelectedValue + "' ", "ProdDesc", "ProdCode", "Select");
        }

        private void LoadDDLs()
        {
            string where = "DealerCode IN ('COMON','"+Session["DealerCode"].ToString()+"')";
            
            string[] Columns = new string[] { "CusCode","CusDesc+' '+FatherHusName"," Address1","isnull(phone1,isnull(phone2,CellNo))" };
            sysFun.GetMultiColumnsDDL(ddlCustomer, Columns, "Customer", where, "CusCode", " Order by CusCode Desc ", false, false);

            string where1 = "DealerCode IN ('COMON','" + Session["DealerCode"].ToString() + "')";

            string[] Columns1 = new string[] { "CusCode", "CusDesc", " Address1", "isnull(phone1,isnull(phone2,CellNo))" };
            sysFun.GetMultiColumnsDDL(ddlAcctof, Columns1, "Customer", where1, "CusCode", " Order by CusCode Desc ", false, false);

            string where2 = "DealerCode IN ('COMON','" + Session["DealerCode"].ToString() + "')";

            string[] Columns2 = new string[] { "BrandCode" , "BrandDesc"};
            sysFun.GetMultiColumnsDDL(ddlBrand, Columns2, "Brand", where2, "BrandCode", "", false, false);

            //string where3 = "DealerCode IN ('AAAAA','" + Session["DealerCode"].ToString() + "')";

            //string[] Columns3 = new string[] { "ColorCode" , "ColorDesc" };
            //sysFun.GetMultiColumnsDDL(ddlColor, Columns3, "Color", where3, "ColorCode", "", false, false);

            sysFun.FillDropDown(ddlColor, "SELECT ColorCode,ColorDesc FROM Color Where DealerCode IN('AAAAA', 'COMON') ", "ColorDesc", "ColorCode", "Select");

            string where4 = "DealerCode = 'COMON'";

            string[] Columns4 = new string[] { "InsCompCode" , "InsCompDescription" };
            sysFun.GetMultiColumnsDDL(ddlInsCo, Columns4, "InsuranceCompanies", where4, "InsCompCode", "", false, false);            
           
        }

        private void createLubDT()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("CucCode", typeof(string)));
            dt.Columns.Add(new DataColumn("RegDate", typeof(string)));
            dt.Columns.Add(new DataColumn("RegNo", typeof(string)));
            dt.Columns.Add(new DataColumn("ChassisNo", typeof(string)));
            dt.Columns.Add(new DataColumn("EngineNo", typeof(string)));
            dt.Columns.Add(new DataColumn("Brand", typeof(string)));
            dt.Columns.Add(new DataColumn("Product", typeof(string)));
            dt.Columns.Add(new DataColumn("Color", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceNo", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
            dt.Columns.Add(new DataColumn("AccountOff", typeof(string)));
            //dt.Columns.Add(new DataColumn("EndUserCode", typeof(string)));


            gvCustVeh.DataSource = dt; gvCustVeh.DataBind(); ViewState["Lub"] = dt;
        }

        protected void ddlCustomer_OnSelectedIndexChanged(object sender, EventArgs e)
        {

            LoadData();
            
        }


        protected void txtRegNo_TextChanged(object sender, EventArgs e)
        {
            if (txtRegNo.Text == "")
            {
                return;
            }

            if (sysFun.IsExist("RegNo", txtRegNo.Text, "CustomerVehicle", Session["DealerCode"].ToString()))
            {
                Get_Data("RegNo");

                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = "Registration Number " + txtRegNo.Text + " already exist for a Customer";
                return;
            }

            if (sysFun.IsExistWithoutDealerCode("RegNo", txtRegNo.Text, "CustomerVehicle", ""))
            {
                Get_Data("RegNo");

                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = "Registration Number " + txtRegNo.Text + " already exist for a Customer";
                //PopupControlMSG.ShowOnPageLoad = true;
                return;
                //LoadVehicleRecord("RegNo", txtRegNo.Text);
            }
        }
        
        private void LoadData()
        {

            string ccode = ddlCustomer.SelectedValue;
            ddlAcctof.SelectedIndex = ddlCustomer.SelectedIndex;
            //txtCustomerName.Text = sysFuns.GetCustomerName(ccode, Session["DealerCode"].ToString());

            ddlAcctof.Enabled = true;
            txtRegDate.Enabled = true;
            txtRegNo.Enabled = true;
            ddlVersion.Enabled = true;
            txtEngineNo.Enabled = true;
            ddlProduct.Enabled = true;
            txtChassisNo.Enabled = true;
            ddlColor.Enabled = true;
            txtInvDate.Enabled = true;
            ddlInsCo.Enabled = true;
            txtWBNo.Enabled = true;
            txtPolicyNo.Enabled = true;
            ddlBrand.Enabled = true;
            btnDeleteInv.Enabled = true;
            btnSaveInv.Enabled = true;
            txtInvoiceNo.Enabled = true;
            txtDeliveryDate.Enabled = true;

            SqlParameter[] param = {
                                        new SqlParameter("@DealerCode",SqlDbType.VarChar,5), 
                                        new SqlParameter("@CusCode",SqlDbType.VarChar,50)
                                   };

            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = sysFun.GetNullString(ccode);
            gvCustVeh.DataSource = sysFuns.FillDataSetOrEmptyRow("sp_2W_Service_CusVeh_Select", param);
            gvCustVeh.DataBind();
            gvCustVeh.Enabled = true;
        }

        protected void ddlBrand_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //string where5 = "DealerCode = '"+ Session["DealerCode"].ToString() + "' and  BrandCode='" + ddlBrand.SelectedValue + "'";

            //string[] Columns5 = new string[] { "ProdCode", "ProdDesc" , "VersionCode" };
            //sysFun.GetMultiColumnsDDL(ddlProduct, Columns5, "Vehicle", where5, "ProdCode", "", false, false);
            txtProdDesc.Text = "";
            LoadProductddl();
            txtProdDesc.Text = ddlBrand.SelectedItem.Text;
           
        }
        public void loadVersionddl()
        {
            sysFun.FillDropDown(ddlVersion, "SELECT Distinct  V.VersionCode,V.VersionDesc FROM Versions V inner join Vehicle A on A.VersionCode=V.VersionCode and A.DealerCode =V.DealerCode  Where V.DealerCode IN('" + Session["DealerCode"].ToString() + "', 'COMON') and A.ProdCode='"+ddlProduct.SelectedValue+"' ", "VersionDesc", "VersionCode", "Select");
        }

        protected void ddlProduct_OnSelectedIndexChanged(object sender, EventArgs e)
        {

            loadVersionddl();
            txtProdDesc.Text = txtProdDesc.Text +"-"+ ddlProduct.SelectedItem.Text;

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
                    lblMsg.Visible = true;
                    lblMsg.Text = "Error: Field(s) Marked With Red Steriks '*' Are Mendetory...";
                    break;
                }
                else { textBoxes[i].BorderColor = System.Drawing.ColorTranslator.FromHtml("#1A6297"); }
            }
            return isValid;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2530", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }

            if (txtCusVehCode.Text !="")
            {
                if (!sec.UserRight("2530", "003"))
                {
                    Response.Redirect("~/Test.aspx");
                }
            }
            TextBox[] textBoxes = { txtCusCode, txtAccCode,txtChassisNo,txtEngineNo,txtRegNo };

            //if (sysFuns.CodeExists("CustomerEstimateMaster", "EngineNo", txtEngineNo.Text.Trim(), Session["DealerCode"].ToString()))
            //{
            //    lblMsg.Text = "Can not update this Engine number use in Estimate " + txtEngineNo.Text;
            //    lblMsg.ForeColor = Color.Red;
            //    txtEngineNo.Focus();

            //    return;
            //}
            //else
            //{
            //    lblMsg.Text = "";
            //}
            //if (sysFuns.CodeExists("CustomerEstimateMaster", "ChassisNo", txtChassisNo.Text.Trim(), Session["DealerCode"].ToString()))
            //{
            //    lblMsg.Text = "Can not update this Chassis number use in Estimate " + txtChassisNo.Text;
            //    lblMsg.ForeColor = Color.Red;
            //    txtChassisNo.Focus();
            //    return;
            //}
            //else
            //{
            //    lblMsg.Text = "";
            //}
            //if (sysFuns.CodeExists("JobCardMaster", "EngineNo", txtEngineNo.Text.Trim(), Session["DealerCode"].ToString(), " And GatePass='' "))
            //{
            //    lblMsg.Text = "Can not update this Engine number use in Job Card " + txtEngineNo.Text;
            //    lblMsg.ForeColor = Color.Red;
            //    txtEngineNo.Focus();

            //    return;
            //}
            //else
            //{
            //    lblMsg.Text = "";
            //}
            //if (sysFuns.CodeExists("JobCardMaster", "ChassisNo", txtChassisNo.Text.Trim(), Session["DealerCode"].ToString(), " And GatePass='' "))
            //{
            //    lblMsg.Text = "Can not update this Chassis number use in Job Card " + txtChassisNo.Text;
            //    lblMsg.ForeColor = Color.Red;
            //    txtChassisNo.Focus();
            //    return;
            //}
            //else
            //{
            //    lblMsg.Text = "";
            //}
           

            if (txtInvDate.Text != "" && txtRegDate.Text != "")
            {
                DateTime reg_date, invoice_date;

                invoice_date = DateTime.ParseExact(txtInvDate.Text, "dd-MM-yyyy", null);
                reg_date = DateTime.ParseExact(txtRegDate.Text, "dd-MM-yyyy", null);

                if (reg_date < invoice_date)
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = Color.Red;
                    lblMsg.Text = "Registration date should not be less then Invoice date";
                    return;
                }
            }
            if (ddlColor.SelectedIndex == 0)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = "Please Select the Color";
                return;
            }

            //TextBox[] textBoxes = { txtCustomerCode, txtEndUser, txtRegDate, txtRegNo, txtEngineNo, txtChassisNo, txtProduct, txtInvoiceNo, txtInvoiceDate };
            if (ddlCustomer.SelectedValue.Trim() == "" || ddlAcctof.SelectedValue.Trim() == "0" || ddlAcctof.SelectedIndex == 0 || txtChassisNo.Text.Trim() == "" || txtEngineNo.Text.Trim() == "")
            {
                sysFun.UserMsg(lblMsg, Color.Red, "Please Select the Customer First");
                return;
            }
            if (!MasterValidation(textBoxes)) return;
            else
            {
                /// Validate from SMIS Database of Engine number ///
                DataSet dsValidate = new DataSet();                

                SqlParameter[] param = {                                 
                new SqlParameter("@DealerCode",SqlDbType.VarChar,5), //0        
                new SqlParameter("@CusCode",SqlDbType.VarChar,8),//1	 		                
                new SqlParameter("@EndUserCode",SqlDbType.Char,8),//2			                                
                new SqlParameter("@CtdDate",SqlDbType.DateTime),//3			                                 
                new SqlParameter("@RegNo",SqlDbType.VarChar,15),//4			 
                new SqlParameter("@ChassisNo",SqlDbType.VarChar,30),//5			 
                new SqlParameter("@EngineNo",SqlDbType.VarChar,30),//6			 
                new SqlParameter("@BrandCode",SqlDbType.Char,3),//7			 
                new SqlParameter("@ProdCode",SqlDbType.VarChar,20),//8			 
                new SqlParameter("@VersionCode",SqlDbType.Char,3),//9			 
                new SqlParameter("@ColorCode",SqlDbType.Char,5),//10			    
                new SqlParameter("@InvoiceNo",SqlDbType.VarChar,25),//11	         
                new SqlParameter("@InvoiceDate",SqlDbType.DateTime),//12		     
                new SqlParameter("@InsCompCode",SqlDbType.Char,4),//13			 
                new SqlParameter("@InsPolicyNo",SqlDbType.VarChar,25),//14			 
                new SqlParameter("@LossNo",SqlDbType.VarChar,15),//15			 
                new SqlParameter("@MeterReading",SqlDbType.VarChar,10),//16
                new SqlParameter("@MonthReading",SqlDbType.VarChar,4),//17
                new SqlParameter("@Remarks",SqlDbType.VarChar,100),//18			 
                new SqlParameter("@UpdUser",SqlDbType.VarChar,30),//19		                 
		        new SqlParameter("@UpdTerm",SqlDbType.VarChar,30),//20
                new SqlParameter("@WorkbookNo",SqlDbType.VarChar,25),//21
                new SqlParameter("@DeliveryDate",SqlDbType.DateTime),//22
                new SqlParameter("@CusVehCode",SqlDbType.Char,8)//23
    };
                // 101 = Not Available
                // 102 = Not Understand

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlCustomer.SelectedValue.Trim();
                param[2].Value = ddlAcctof.SelectedValue.Trim();
                param[3].Value = sysFuns.SaveDate(txtRegDate.Text);
                param[4].Value = txtRegNo.Text.Trim().ToUpper().ToUpper();
                param[5].Value = txtChassisNo.Text.Trim().ToUpper();
                param[6].Value = txtEngineNo.Text.Trim().ToUpper();
                param[7].Value = ddlBrand.SelectedValue.ToString().Trim();
                param[8].Value = ddlProduct.SelectedValue;
                param[9].Value = ddlVersion.SelectedValue.Trim();
                param[10].Value = ddlColor.SelectedValue.ToString().Trim();
                param[11].Value = txtInvoiceNo.Text.Trim();
                param[12].Value = sysFuns.SaveDate(txtInvDate.Text);
                param[13].Value = ddlInsCo.SelectedValue.Trim();
                param[14].Value = txtPolicyNo.Text.Trim();
                param[15].Value = ""; // new SqlParameter("@LossNo",SqlDbType.VarChar,15),			 
                param[16].Value = ""; // new SqlParameter("@MeterReading",SqlDbType.VarChar,10),
                param[17].Value = ""; // new SqlParameter("@MonthReading",SqlDbType.VarChar,4),
                param[18].Value = ""; // new SqlParameter("@Remarks",SqlDbType.VarChar,100),			 
                param[19].Value = Session["UserName"].ToString().ToUpper();
                param[20].Value = Environment.MachineName;
                param[21].Value = txtWBNo.Text;
                param[22].Value = sysFuns.SaveDate(txtDeliveryDate.Text);

                //select * from brand where BrandCode=''
                //select * from version Where Version=''                
                //select * from customer where CusCode=''

                //select * from vehicle where BrandCode=='' and ProdCode='' and VersionCode='' and ColorCode                
                //select * from color where ColorCode = ''
                try
                {
                    if ((sysFun.IsExist("CusCode", ddlCustomer.SelectedValue.Trim(), "Customer", Session["DealerCode"].ToString()) == true))
                    {
                        if ((sysFun.IsExist("BrandCode", ddlBrand.SelectedValue.Trim(), "Brand", Session["DealerCode"].ToString()) == true))
                        {
                            if ((sysFun.IsExist("VersionCode", ddlVersion.SelectedValue, "Vehicle", Session["DealerCode"].ToString()) == true))
                            {
                                // apStr = " and  versioncode=" + txtVersion.Text + " and ProdCode='" + txtProduct.Text + "'";
                                if ((sysFun.IsExist("ColorCode", ddlColor.SelectedValue, "Color", Session["DealerCode"].ToString()) == true))
                                {

                                    //if (!sysFun.IsExist("RegNo", txtRegNo.Text, "CustomerVehicle", Session["DealerCode"].ToString())
                                    //    & !sysFun.IsExist("ChassisNo", txtChassisNo.Text, "CustomerVehicle", Session["DealerCode"].ToString())
                                    //    & !sysFun.IsExist("EngineNo", txtEngineNo.Text, "CustomerVehicle", Session["DealerCode"].ToString()))
                                    if(txtCusVehCode.Text == "")
                                    {
                                        if ((sysFun.IsExist("ChassisNo", txtChassisNo.Text, "CustomerVehicle", Session["DealerCode"].ToString()) == true))
                                        {
                                            lblMsg.Text = "Record Not Saved!"+txtChassisNo.Text+" is already exist ";
                                            return;
                                        }
                                        if ((sysFun.IsExist("EngineNo", txtEngineNo.Text, "CustomerVehicle", Session["DealerCode"].ToString()) == true))
                                        {
                                            lblMsg.Text = "Record Not Saved!" + txtEngineNo.Text + " is already exist ";
                                            return;
                                        }
                                        if ((sysFun.IsExist("RegNo", txtRegNo.Text, "CustomerVehicle", Session["DealerCode"].ToString()) == true))
                                        {
                                            lblMsg.Text = "Record Not Saved!" + txtRegNo.Text + " is already exist ";
                                            return;
                                        }
                                        AutoGenCode = sysFun.GetNewMaxID("CustomerVehicle", "CusVehCode", 8, Session["DealerCode"].ToString());
                                        param[23].Value = AutoGenCode;

                                        if (sysFun.ExecuteSP_NonQuery("sp_CustomerVehicle_Insert", param))
                                        {
                                            lblMsg.Text = "Record Saved!" + ddlVersion.SelectedValue;
                                            // ClearALL();
                                            ClearTextBoxs();
                                        }
                                        else
                                        {
                                            lblMsg.Text = "Record Not Saved!";
                                            //PopupControlMSG.ShowOnPageLoad = true;

                                        }
                                    }
                                    else
                                    {
                                        param[23].Value = txtCusVehCode.Text;
                                        string apStr = " and ChassisNo ='" + txtChassisNo.Text + "' and jobstatus = null and GatePass = null";
                                        if (sysFun.IsExist("ChassisNo", txtChassisNo.Text, "JobCardMaster", Session["DealerCode"].ToString())
                                            & sysFun.IsExist("EngineNo", txtEngineNo.Text, "JobCardMaster", Session["DealerCode"].ToString()))
                                        {
                                            if (sysFun.IsExist("ChassisNo", txtChassisNo.Text, "JobCardMaster", Session["DealerCode"].ToString(), apStr))
                                            {
                                                lblMsg.Text = "Record can not be updated";
                                                //PopupControlMSG.ShowOnPageLoad = true;
                                                return;
                                            }
                                            else
                                            {
                                                if (sysFun.ExecuteSP_NonQuery("sp_CustomerVehicle_Update", param))
                                                {
                                                    // objMBLL.ShowMessageBox("Record Updated", txtCustomerCode);
                                                    lblMsg.Text = "Record Updated";
                                                    //PopupControlMSG.ShowOnPageLoad = true;
                                                    //GetDataLoaded(true, false);
                                                    //ClearALL();
                                                    // LoadData()
                                                    ClearTextBoxs();
                                                }
                                                else
                                                {
                                                    lblMsg.Text = "Record Not updated";
                                                    //PopupControlMSG.ShowOnPageLoad = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (sysFun.ExecuteSP_NonQuery("sp_CustomerVehicle_Update", param))
                                            {
                                                //objMBLL.ShowMessageBox("Record Updated", txtCustomerCode);
                                                lblMsg.Text = "Record updated";
                                                //PopupControlMSG.ShowOnPageLoad = true;
                                                ClearTextBoxs();
                                                //GetDataLoaded(true, false);
                                               // LoadData();
                                            }
                                            else
                                            {
                                                lblMsg.Text = "Record can not be updated";
                                                //PopupControlMSG.ShowOnPageLoad = true;

                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //objMBLL.ShowMessageBox("'Version' not exist! ", txtCustomerCode);
                                lblMsg.Text = "Version not exist";
                                //PopupControlMSG.ShowOnPageLoad = true;

                            }
                        }
                        else
                        {
                            lblMsg.Text = "Product not exist!";
                            //PopupControlMSG.ShowOnPageLoad = true;

                        }
                    }
                    else
                    {
                        lblMsg.Text = "Brand not exist";
                        //PopupControlMSG.ShowOnPageLoad = true;

                    }
                }
                catch (Exception ex)
                {
                    //var LineNumber = new StackTrace(ex, true).GetFrame(0).GetFileLineNumber();
                    lblMsg.Text = "Error : " + ex.Message + "/" + ex.StackTrace.ToString() + "/";
                    //PopupControlMSG.ShowOnPageLoad = true;

                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2530", "004"))
            {
                Response.Redirect("~/Test.aspx");
            }

            if (txtEngineNo.Text.Trim().Length == 0 |
                txtChassisNo.Text.Trim().Length == 0)
            {

                //grl.UserMsg(lblMasg, Color.Red, "Select a record to delete.", txtCustomerCode);
                lblMsg.Text = "Select a record to delete.";
                //PopupControlMSG.ShowOnPageLoad = true;
                return;
            }
            if ((sysFun.IsExist("EngineNo", txtEngineNo.Text, "CustomerEstimateMaster", Session["DealerCode"].ToString()) == true)
               | (sysFun.IsExist("ChassisNo", txtChassisNo.Text, "CustomerEstimateMaster", Session["DealerCode"].ToString()) == true)
             )
            {
                //grl.UserMsg(lblMasg, Color.Red, "Information is used in Jobcard, You can't delete.", txtCustomerCode);
                lblMsg.Text = "Information is  used in Estimate , You can't delete.";
                //PopupControlMSG.ShowOnPageLoad = true;
                return;
            }

            if ((sysFun.IsExist("EngineNo", txtEngineNo.Text, "JobCardMaster", Session["DealerCode"].ToString()) == true)
                | (sysFun.IsExist("ChassisNo", txtChassisNo.Text, "JobCardMaster", Session["DealerCode"].ToString()) == true)
                | (sysFun.IsExist("ChassisNo", txtChassisNo.Text, "FIRMaster", Session["DealerCode"].ToString()) == true)
                | (sysFun.IsExist("EngineNo", txtEngineNo.Text, "FIRMaster", Session["DealerCode"].ToString()) == true)
                | (sysFun.IsExist("VehchassisNo", txtChassisNo.Text, "PPRMaster", Session["DealerCode"].ToString()) == true)
                | (sysFun.IsExist("VehEngineNo", txtEngineNo.Text, "PPRMaster", Session["DealerCode"].ToString()) == true))
            {
                //grl.UserMsg(lblMasg, Color.Red, "Information is used in Jobcard, You can't delete.", txtCustomerCode);
                lblMsg.Text = "Information is in  used , You can't delete.";
                //PopupControlMSG.ShowOnPageLoad = true;
                return;
            }
            else
            {
                SqlParameter[] param = {                                 
                new SqlParameter("@DealerCode",SqlDbType.VarChar,5),         
                new SqlParameter("@CusVehCode",SqlDbType.Char,8)
                };

                param[0].Value = Session["DealerCode"].ToString().Trim();
                param[1].Value = txtCusVehCode.Text;

                if (sysFun.ExecuteSP_NonQuery("sp_CustomerVehicle_Delete", param))
                {
                    ClearALL();
                    //grl.UserMsg(lblMasg, Color.Green, "Record Permenantly Deleted", txtCustomerCode);
                    lblMsg.Text = "Record Permenantly Deleted";
                    //PopupControlMSG.ShowOnPageLoad = true;
                    //GetDataLoaded(true, true);
                }
                else
                {
                    //grl.UserMsg(lblMasg, Color.Red, "Record Not Updated!", txtCustomerCode);
                    lblMsg.Text = "Record Not Deleted";
                    //PopupControlMSG.ShowOnPageLoad = true;
                }

            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearALL();
            
        }
        private void ClearTextBoxs()
        {
          //  ddlAcctof.SelectedIndex = 0;
          //  ddlCustomer.SelectedIndex = 0;
            txtRegDate.Text = txtRegNo.Text = "";
            ddlVersion.SelectedIndex = 0;
            //txtBrand.Text = txtBrandName.Text = "";

            if (ddlProduct.SelectedIndex > 0)
            {
                ddlProduct.SelectedIndex = 0;
            }

            txtChassisNo.Text = "";
        //    txtCusDesc.Text = "";
         //   txtCusCode.Text = "";
         //   txtAccDesc.Text = "";
          //  txtAccCode.Text = "";

            ddlColor.Text = ""; txtWBNo.Text = ""; txtInvDate.Text = "";
            ddlInsCo.SelectedIndex = 0;
            ddlBrand.SelectedIndex = 0;
            txtEngineNo.Text = "";
            //txtInsuranceName.Text = grl.GetStringValuesAgainstCodes("cusCode", txtCustomerCode.Text, "CusDesc+' '+FatherHusName", "Customer");  
            txtDeliveryDate.Text = "";
            txtPolicyNo.Text = "";
            txtInvoiceNo.Text = "";
            //FillBlankRows(1);
         
  
            LoadData();

            lblMsg.Text = "";
            Session["CusCode"] = null;
        }
        private void ClearALL()
        {
            ddlAcctof.SelectedIndex = 0;
            ddlCustomer.SelectedIndex = 0;
            txtRegDate.Text = txtRegNo.Text = "";
            //ddlVersion.SelectedIndex = 0;
            //txtBrand.Text = txtBrandName.Text = "";

            if(ddlProduct.SelectedIndex > 0)
            {
                ddlProduct.SelectedIndex = 0;
            }
            
            txtChassisNo.Text = "";
            txtCusDesc.Text = "";
            txtCusCode.Text = "";
            txtAccDesc.Text = "";
            txtAccCode.Text = "";
            
            ddlColor.Text = ""; txtWBNo.Text = ""; txtInvDate.Text = "";            
            ddlInsCo.SelectedIndex = 0;
            ddlBrand.SelectedIndex = 0;
            txtEngineNo.Text = "";
            //txtInsuranceName.Text = grl.GetStringValuesAgainstCodes("cusCode", txtCustomerCode.Text, "CusDesc+' '+FatherHusName", "Customer");  
            txtDeliveryDate.Text = "";
            txtPolicyNo.Text = "";
            txtInvoiceNo.Text = "";
            //FillBlankRows(1);
            ddlCustomer.Enabled = true;
            ddlAcctof.Enabled = false;
            txtRegDate.Enabled = false;
            txtRegNo.Enabled = false;
            ddlVersion.Enabled = false;
            txtEngineNo.Enabled = false;
            ddlProduct.Enabled = false;
            txtChassisNo.Enabled = false;
            ddlColor.Enabled = false;
            txtInvDate.Enabled = false;
            ddlInsCo.Enabled = false;
            txtWBNo.Enabled = false;
            txtPolicyNo.Enabled = false;
            ddlBrand.Enabled = false;
            txtCusVehCode.Text = "";
            btnSaveInv.Enabled = false;
            btnDeleteInv.Enabled = false;

            LoadData();

            lblMsg.Text = "";
            Session["CusCode"] = null;
        }

        protected void lnkSelect_Click1(object sender, EventArgs e)
        {
            LinkButton btnlnk = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)btnlnk.Parent.Parent;

            LinkButton lnk = (LinkButton)gvr.FindControl("lnkRegNo");

            sysFun.FillDropDown(ddlProduct, "SELECT rtrim(ltrim(ProdCode)) as ProdCode ,ProdDesc FROM Vehicle Where DealerCode IN('" + Session["DealerCode"].ToString() + "', 'COMON') and VehicleCategory='"+Session["VehicleCategory"].ToString()+"' ", "ProdDesc", "ProdCode", "Select");
            sysFun.FillDropDown(ddlVersion, "SELECT Distinct  VersionCode,VersionDesc FROM Versions Where DealerCode IN('" + Session["DealerCode"].ToString() + "', 'COMON') ", "VersionDesc", "VersionCode", "Select");
            if (lnk.Text == "")
            {
                //clearData();
                return;
            }
            try
            {
                ddlProduct.Enabled = true;

                DataTable dt = new DataTable();

                string sql = "select * from CustomerVehicle where RegNo = '" + lnk.Text + "'";
                dt = sysFun.GetData(sql);                
                

                if (dt.Rows[0]["InvoiceDate"].ToString().Trim() != "")
                {
                    DateTime InvoiceDate = (DateTime)dt.Rows[0]["InvoiceDate"];
                    txtInvDate.Text = InvoiceDate.ToString("dd-MM-yyyy");
                }

                if (dt.Rows[0]["tdDate"].ToString() != "")
                {
                    DateTime RegDate = (DateTime)dt.Rows[0]["tdDate"];
                    txtRegDate.Text = RegDate.ToString("dd-MM-yyyy");
                }

                if (dt.Rows[0]["DeliveryDate"].ToString() != "")
                {
                    DateTime DelDate = (DateTime)dt.Rows[0]["DeliveryDate"];
                    txtDeliveryDate.Text = DelDate.ToString("dd-MM-yyyy");
                }

                if (dt.Rows[0]["EndUserCode"].ToString() != "0" || dt.Rows[0]["EndUserCode"].ToString() != "")
                {
                    ddlAcctof.SelectedValue = dt.Rows[0]["EndUserCode"].ToString();
                    txtAccDesc.Text = sysFuns.GetStringValuesAgainstCodes("CusCode='" + dt.Rows[0]["EndUserCode"].ToString() + "' and DealerCode='" + Session["DealerCode"].ToString() + "'", "Customer", "CusDesc");

                }
                ddlBrand.SelectedValue = dt.Rows[0]["BrandCode"].ToString();
                //LoadProductddl();
                ddlProduct.SelectedValue = dt.Rows[0]["ProdCode"].ToString().Trim();
                //loadVersionddl();
                ddlVersion.SelectedValue = dt.Rows[0]["VersionCode"].ToString();
                txtRegNo.Text = dt.Rows[0]["RegNo"].ToString();
                txtEngineNo.Text = dt.Rows[0]["EngineNo"].ToString();
                txtChassisNo.Text = dt.Rows[0]["ChassisNo"].ToString();
                ddlColor.SelectedValue = dt.Rows[0]["ColorCode"].ToString();
                txtInvoiceNo.Text = dt.Rows[0]["InvoiceNo"].ToString();
                txtAccCode.Text = dt.Rows[0]["EndUserCode"].ToString();
                txtCusVehCode.Text = dt.Rows[0]["CusVehCode"].ToString();
                txtPolicyNo.Text = dt.Rows[0]["InsPolicyNo"].ToString();
                txtWBNo.Text = dt.Rows[0]["WorkbookNo"].ToString();
                txtCusCode.Text = dt.Rows[0]["CusCode"].ToString();
                txtCusDesc.Text= sysFuns.GetStringValuesAgainstCodes("CusCode='" + dt.Rows[0]["CusCode"].ToString() + "' and DealerCode='" + Session["DealerCode"].ToString() + "'", "Customer", "CusDesc");
                if (dt.Rows[0]["InsBranchCode"].ToString() != "")
                {
                    ddlInsCo.SelectedValue = dt.Rows[0]["InsBranchCode"].ToString();
                }
                else
                {
                    ddlInsCo.SelectedIndex = 0;
                }
                txtChassisNo.Enabled = false;
                txtEngineNo.Enabled = false;
                txtRegNo.Enabled = false;
                txtProdDesc.Text = ddlBrand.SelectedItem.Text + "-" + ddlProduct.SelectedItem.Text +"-"+ddlVersion.SelectedItem.Text;               

            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;

            }

        }
        public void Get_Data(string by)
        {
            try
            {
                sysFun.FillDropDown(ddlProduct, "SELECT rtrim(ltrim(ProdCode)) as ProdCode ,ProdDesc FROM Vehicle Where DealerCode IN('" + Session["DealerCode"].ToString() + "', 'COMON') and VehicleCategory='" + Session["VehicleCategory"].ToString() + "' ", "ProdDesc", "ProdCode", "Select");
                sysFun.FillDropDown(ddlVersion, "SELECT Distinct  VersionCode,VersionDesc FROM Versions Where DealerCode IN('" + Session["DealerCode"].ToString() + "', 'COMON') ", "VersionDesc", "VersionCode", "Select");
                ddlProduct.Enabled = true;

                DataTable dt = new DataTable();

                //  string sql = "select * from CustomerVehicle where RegNo = '" + txtRegNo.Text + "'";
                //  dt = sysFun.GetData(sql);
                SqlParameter[] param = {
                                        new SqlParameter("@RegNo",SqlDbType.VarChar,15),
                                        new SqlParameter("@ChassisNo",SqlDbType.VarChar,30),
                                         new SqlParameter("@EngineNo",SqlDbType.VarChar,30)
                                   };
                if(by=="RegNo")
                {
                    param[0].Value = txtRegNo.Text;
                    param[1].Value = null;
                    param[2].Value = null;
                }
                else if (by == "ChassisNo")
                {
                    param[0].Value = null;
                    param[1].Value = txtChassisNo.Text;
                    param[2].Value = null;
                }
                else
                {
                    param[0].Value = null;
                    param[1].Value = null;
                    param[2].Value = txtEngineNo.Text;
                }

                ds = sysFuns.FillDataSetOrEmptyRow("sp_CusVeh_ByReg_ByChassis_byEngine", param);
                dt = ds.Tables[0];


                if (dt.Rows[0]["InvoiceDate"].ToString().Trim() != "")
                {
                  //  DateTime InvoiceDate = (DateTime)dt.Rows[0]["InvoiceDate"];
                    txtInvDate.Text = dt.Rows[0]["InvoiceDate"].ToString();
                }

                if (dt.Rows[0]["tdDate"].ToString() != "")
                {
                   // DateTime RegDate = (DateTime)dt.Rows[0]["tdDate"];
                    txtRegDate.Text = dt.Rows[0]["tdDate"].ToString();
                }

                if (dt.Rows[0]["DeliveryDate"].ToString() != "")
                {
                   // DateTime DelDate = (DateTime)dt.Rows[0]["DeliveryDate"];
                    txtDeliveryDate.Text = dt.Rows[0]["DeliveryDate"].ToString();
                }

                if (dt.Rows[0]["EndUserCode"].ToString() != "0" || dt.Rows[0]["EndUserCode"].ToString() != "")
                {
                  //  ddlAcctof.SelectedValue = dt.Rows[0]["EndUserCode"].ToString();
                  //  txtAccDesc.Text = sysFuns.GetStringValuesAgainstCodes("CusCode='" + dt.Rows[0]["EndUserCode"].ToString() + "' and DealerCode='" + Session["DealerCode"].ToString() + "'", "Customer", "CusDesc");

                }
                ddlBrand.SelectedValue = dt.Rows[0]["BrandCode"].ToString();
                //LoadProductddl();
                ddlProduct.SelectedValue = dt.Rows[0]["ProdCode"].ToString().Trim();
                //loadVersionddl();
                ddlVersion.SelectedValue = dt.Rows[0]["VersionCode"].ToString();
                txtRegNo.Text = dt.Rows[0]["RegNo"].ToString();
                txtEngineNo.Text = dt.Rows[0]["EngineNo"].ToString();
                txtChassisNo.Text = dt.Rows[0]["ChassisNo"].ToString();
                ddlColor.SelectedValue = dt.Rows[0]["ColorCode"].ToString();
                txtInvoiceNo.Text = dt.Rows[0]["InvoiceNo"].ToString();
               // txtAccCode.Text = dt.Rows[0]["CusVehCode"].ToString();
               // txtCusVehCode.Text = dt.Rows[0]["CusVehCode"].ToString();
                txtPolicyNo.Text = dt.Rows[0]["InsPolicyNo"].ToString();
                txtWBNo.Text = dt.Rows[0]["WorkbookNo"].ToString();
              //  txtCusCode.Text = dt.Rows[0]["EndUserCode"].ToString();
               // txtCusDesc.Text = sysFuns.GetStringValuesAgainstCodes("CusCode='" + dt.Rows[0]["CusCode"].ToString() + "' and DealerCode='" + Session["DealerCode"].ToString() + "'", "Customer", "CusDesc");
                if (dt.Rows[0]["InsBranchCode"].ToString() != "")
                {
                    ddlInsCo.SelectedValue = dt.Rows[0]["InsBranchCode"].ToString();
                }
                else
                {
                    ddlInsCo.SelectedIndex = 0;
                }


            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;

            }
        }  

        protected void txtEngineNo_TextChanged(object sender, EventArgs e)
        {
            if (txtEngineNo.Text.Length < 4)
            {

                txtEngineNo.BorderColor = Color.Red;
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = "minimum input length should be 4 charcter ";
                return;
            }
            else
            {
                lblMsg.Text = "";
                txtEngineNo.BorderColor = Color.Black;
            }
            if (txtEngineNo.Text == "")
            {
                Get_Data("EngineNo");

                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = "Engine Number " + txtEngineNo.Text + " already exist for a Customer";
                return;
            }

            if (sysFun.IsExist("EngineNo", txtEngineNo.Text, "CustomerVehicle", Session["DealerCode"].ToString()))
            {
                Get_Data("EngineNo");

                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = "Engine Number " + txtEngineNo.Text + " already exist for a Customer";
                return;
            }

            if (sysFun.IsExistWithoutDealerCode("EngineNo", txtEngineNo.Text, "CustomerVehicle", ""))
            {
                Get_Data("EngineNo");

                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = "Engine Number " + txtEngineNo.Text + " already exist for a Customer";
                //PopupControlMSG.ShowOnPageLoad = true;
                return;
                //LoadVehicleRecord("RegNo", txtRegNo.Text);
            }
            else
                {
                    lblMsg.BorderColor = Color.Black;

                }
            }
        

        protected void txtChassisNo_TextChanged(object sender, EventArgs e)
        {
            if (txtChassisNo.Text.Length < 4)
            {

                txtChassisNo.BorderColor = Color.Red;
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = "minimum input length should be 4 charcter ";
                return;
            }
            else
            {
                lblMsg.Text = "";
                txtChassisNo.BorderColor = Color.Black;
            }
            if (txtChassisNo.Text == "")
            {
                Get_Data("ChassisNo");

                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = "Chassis Number " + txtChassisNo.Text + " already exist for a Customer";
                return;
            }

            if (sysFun.IsExist("ChassisNo", txtChassisNo.Text, "CustomerVehicle", Session["DealerCode"].ToString()))
            {
                Get_Data("ChassisNo");

                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = "Chassis Number " + txtChassisNo.Text + " already exist for a Customer";
                //PopupControlMSG.ShowOnPageLoad = true;
                return;
            }

            if (sysFun.IsExistWithoutDealerCode("ChassisNo", txtChassisNo.Text, "CustomerVehicle", ""))
            {
                Get_Data("ChassisNo");

                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = "Chassis Number " + txtChassisNo.Text + " already exist for a Customer";
                //PopupControlMSG.ShowOnPageLoad = true;
                return;
                //LoadVehicleRecord("RegNo", txtRegNo.Text);
            }
            else
            {
                lblMsg.BorderColor = Color.Black;

            }


        }

        protected void ddlVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtProdDesc.Text = txtProdDesc.Text + "-" + ddlVersion.SelectedItem.Text;
        }
        protected void imgCustomerCode_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 1;
            ViewState["State"] = "Customer";

            clslook.LU_Get_Customer(imgCustomerCode, ViewState["lookupid"].ToString(), "DealerCode = '" + Session["DealerCode"].ToString() + "'", "../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void ImgAccoff_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 1;
            ViewState["State"] = "Account";
            clslook.LU_Get_Customer(ImgAccoff, ViewState["lookupid"].ToString(), "DealerCode = '" + Session["DealerCode"].ToString() + "'", "../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }
        protected void SelectedPartDetail(string item)
        {
            try
            {

                if (ViewState["lookupid"].ToString() == "1" && ViewState["State"].ToString()== "Customer")
                {
                    txtCusCode.Text = item;
                    ddlCustomer.SelectedValue = item;
                    txtAccCode.Text = item;
                    ddlAcctof.SelectedValue = item;
                    txtCusDesc.Text = sysFuns.GetStringValuesAgainstCodes("CusCode='" + item + "' and DealerCode='" + Session["DealerCode"].ToString()+"'" , "Customer", "CusDesc");
                    txtAccDesc.Text = sysFuns.GetStringValuesAgainstCodes("CusCode='" + item + "' and DealerCode='" + Session["DealerCode"].ToString()+ "'", "Customer", "CusDesc");
                    ddlCustomer_OnSelectedIndexChanged(null, null);
                }
                else if(ViewState["lookupid"].ToString() == "1" && ViewState["State"].ToString() == "Account")
                {
                    txtAccCode.Text = item;
                    ddlAcctof.SelectedValue = item;
                    txtAccDesc.Text = sysFuns.GetStringValuesAgainstCodes("CusCode='" + item + "' and DealerCode='" + Session["DealerCode"].ToString() + "'", "Customer", "CusDesc");
                }
            }
            catch (Exception ex)
            {
                sysFun.UserMsg(lblMsg, Color.Red, ex.Message);
            }
        }

        //protected void imgProduct_Click(object sender, ImageClickEventArgs e)
        //{
        //    if (ddlBrand.SelectedIndex == 0)
        //    {
        //        lblMsg.Text = "Please Select the Brand First";
        //        lblMsg.ForeColor = Color.Red;
        //        return;
        //    }
        //    ViewState["lookupid"] = 26;
        //    ViewState["State"] = "Product";
        //    clslook.LU_Get_Product(imgProduct, ViewState["lookupid"].ToString(), "BrandCode='"+ddlBrand.SelectedValue+"' ", "../../../");

        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        //}
    }

    
}