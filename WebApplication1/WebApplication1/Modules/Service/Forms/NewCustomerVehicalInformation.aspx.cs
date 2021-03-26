using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using DevExpress.Web.ASPxEditors;
using System.Diagnostics;

namespace DXBMS.Modules.Service
{
    public partial class NewCustomerVehicalInformation : System.Web.UI.Page
    {
        SysFunctions myFunc = new SysFunctions();
        string apStr, StrMCDMainDBCon;
        bool search_result;
        ServiceBL objSBL = new ServiceBL();
        //MainBLL objMBLL = new MainBLL();
        //General grl = new General();
        //clsLookUp clslook = new clsLookUp();
        DataSet ds, dsSub;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/Main/login.aspx");

            }
            if (!Page.IsPostBack)
            {
                cmbCustomerCode.Focus();
                //-------------------------------------SUBLET
                DataTable CustDT = new DataTable();
                CustDT.Columns.Add(new DataColumn("ID", typeof(int)));
                CustDT.Columns.Add(new DataColumn("CusCode", typeof(string)));
                CustDT.Columns.Add(new DataColumn("tdDate", typeof(string)));
                CustDT.Columns.Add(new DataColumn("RegNo", typeof(string)));
                CustDT.Columns.Add(new DataColumn("ChassisNo", typeof(string)));
                CustDT.Columns.Add(new DataColumn("EngineNo", typeof(string)));
                CustDT.Columns.Add(new DataColumn("BrandCode", typeof(string)));
                CustDT.Columns.Add(new DataColumn("ProdCode", typeof(string)));
                CustDT.Columns.Add(new DataColumn("VersionCode", typeof(string)));
                CustDT.Columns.Add(new DataColumn("ColorCode", typeof(string)));
                CustDT.Columns.Add(new DataColumn("InvoiceNo", typeof(string)));
                CustDT.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
                CustDT.Columns.Add(new DataColumn("InsCompCode", typeof(string)));
                CustDT.Columns.Add(new DataColumn("InsPolicyNo", typeof(string)));

                //SubletDT.Rows.Add(new object[] { null, null, null, null, null, null, null, null,null});            
                gvCVInfo.DataSource = CustDT;
                gvCVInfo.DataBind();



                cmbBrand =  myFunc.FillDrp_SP(cmbBrand, "sp_2W_Select_Brand", "BrandCode", "BrandDesc", null, true, "--Select--", false, "");


            }

            
        }

        protected void txtRegNo_ButtonClick(object source, DevExpress.Web.ASPxEditors.ButtonEditClickEventArgs e)
        {
            string sql = "SELECT Distinct cv.EngineNo AS [Engine No], cv.RegNo AS [Reg No], cv.ChassisNo AS [Chassis No],";
            sql += " c.CusDesc + ' ' + c.FatherHusName AS Customer FROM  CustomerVehicle cv INNER JOIN ";
            sql += " Customer c ON cv.CusCode = c.CusCode AND cv.DealerCode=c.DealerCode ";
            sql += " where cv.DealerCode IN ('AAAAA','"+Session["DealerCode"].ToString()+"') ";

           

            DataTable dt = new DataTable();
            dt = myFunc.GetData(sql);


        } 

      

        protected void txtEngNo_TextChanged(object sender, EventArgs e)
        {
            ds = new DataSet();
            DataTable dt = new DataTable();
            search_result = false;
            txtEngNo.ToolTip = "";
            try
            {
                if (txtEngNo.Text.Count() < 6)
                {
                    lblMSG.Text = "6 Characters must be enter in Engine number";
                    PopupControlMSG.ShowOnPageLoad = true;
                    return;
                }
                else
                {
                    lblMasg.Text = "";
                }
                SqlParameter[] paramData = {
                                        new SqlParameter("@EngineNo",SqlDbType.VarChar,5),
                                        new SqlParameter ("@Chk" ,SqlDbType.Char,1)
                                   };
                if (txtEngNo.Text == "")
                {
                    return;
                }
                else
                {
                    if (cmbBrand.Value.ToString().Trim() == "001")
                    {
                        txtChassisNo.Text = "";
                        cmbProdCode.Text = ""; txtVersion.Text = ""; cmbColorCode.Text = ""; txtInvoiceNo.Text = "";
                        dpInvoiceDate.Text = ""; txtProdDesc.Text = ""; txtColorName.Text = "";

                        if (myFunc.CodeExists("customervehicle", "EngineNo", txtEngNo.Text.Trim(), ref ds,Session["DealerCode"].ToString()))
                        {
                            txtRegNo.Text = ds.Tables[0].Rows[0]["RegNo"].ToString();
                            dpRegDate.Text = ds.Tables[0].Rows[0]["tdDate"].ToString();
                            txtChassisNo.Text = ds.Tables[0].Rows[0]["ChassisNo"].ToString();
                            cmbProdCode.Text = ds.Tables[0].Rows[0]["ProdCode"].ToString();
                            txtProdDesc.Text = myFunc.GetStringValuesAgainstCodes("ProdCode", cmbProdCode.Text, "ProdDesc", "Vehicle");
                            cmbColorCode.Text = ds.Tables[0].Rows[0]["ColorCode"].ToString();
                            txtColorName.Text = myFunc.GetStringValuesAgainstCodes("ProdCode", cmbProdCode.Text, "Color", "Vehicle");

                            FillProduct(ds);
                        }
                        else
                        {

                            paramData[0].Value = txtEngNo.Text;
                            paramData[1].Value = "Y";


                            //ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, "sp_2W_Get_NewVehicleInfo_SDMS", paramData);



                            dt = myFunc.GetData("select * from customervehicle where EngineNo='" + txtEngNo.Text + "'");

                            ds.Tables.Add(dt);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                txtChassisNo.Text = ds.Tables[0].Rows[0]["ChasisNo"].ToString();
                                cmbProdCode.Text = ds.Tables[0].Rows[0]["ProdCode"].ToString();
                                txtProdDesc.Text = ds.Tables[0].Rows[0]["ProdDesc"].ToString();
                                txtColorName.Text = ds.Tables[0].Rows[0]["ColorDesc"].ToString();
                                FillProduct(ds);
                                if (txtColorName.Text.Trim() == "NoColor")
                                {
                                    txtColorName.Text = "BLACK";
                                    cmbColorCode.Text = "00002";
                                }
                                else
                                {
                                    if (myFunc.CodeExists("Vehicle", "ProdCode='" + cmbProdCode.Text.Trim() + "' AND  Color='" + txtColorName.Text.Trim() + "'", ref ds))
                                        cmbColorCode.Text = ds.Tables[0].Rows[0]["ColorCode"].ToString();
                                }
                            }
                            else
                            {
                                lblMSG.Text = "Record Not found "+txtEngNo.Text;
                                PopupControlMSG.ShowOnPageLoad= true;
                                
                            }
                        }

                        if (search_result == false)
                        {
                            paramData[0].Value = txtEngNo.Text;
                            paramData[1].Value = "N";
                            //ds = SqlHelper.ExecuteDataset(this.StrMCDMainDBCon, "sp_2W_Get_NewVehicleInfo_SDMS", paramData);
                            dt = myFunc.GetData("select * from customervehicle where EngineNo='" + txtEngNo.Text + "'");
                            ds.Tables.Add(dt);
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                txtEngNo.ToolTip = txtEngNo.ToolTip + ds.Tables[0].Rows[i]["EngineNo"].ToString() + System.Environment.NewLine;
                            }
                            
                        }
                        txtEngNo.Focus();
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }

        protected void btnPopOk_Click(object sender, EventArgs e)
        {
            PopupControlMSG.ShowOnPageLoad = false;
        }

        protected void btnPopCancel_Click(object sender, EventArgs e)
        {
            PopupControlMSG.ShowOnPageLoad = false;
        }

        private void FillProduct(DataSet ds)
        {
            search_result = true;

            cmbBrand.Value = ds.Tables[0].Rows[0]["BrandCode"].ToString();
            //txtBrandName.Text = grl.GetStringValuesAgainstCodes("BrandCode=" + txtBrand.Text.Trim(), "Brand", "BrandDesc");
            txtVersion.Text = ds.Tables[0].Rows[0]["VersionCode"].ToString();
            txtInvoiceNo.Text = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();
            dpInvoiceDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["InvoiceDate"].ToString()).ToString("dd-MM-yyyy");
            if (dpInvoiceDate.Text.Contains("1900"))
            {
                dpInvoiceDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        protected void cmbCustomerCode_TextChanged(object sender, EventArgs e)
        {
            string ccode = cmbCustomerCode.Text;

            txtCustomerName.Text= myFunc.GetCustomerName(ccode, Session["DealerCode"].ToString());

            cmbEndUser.Enabled = true;
            dpRegDate.Enabled = true;
            txtRegNo.Enabled = true;
            txtVersion.Enabled = true;
            txtEngNo.Enabled = true;
            cmbProdCode.Enabled = true;
            txtChassisNo.Enabled = true;
            cmbColorCode.Enabled = true;
            dpInvoiceDate.Enabled = true;
            cmbInsCode.Enabled = true;
            txtInvoiceNo.Enabled = true;
            txtPolicyNo.Enabled = true;
            cmbBrand.Enabled = true;
            btnDelete.Enabled = false;
            btnSave.Enabled = true;

            SqlParameter[] param = {
                                        new SqlParameter("@DealerCode",SqlDbType.VarChar,5), 
                                        new SqlParameter("@CusCode",SqlDbType.VarChar,50)
                                   };

            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = myFunc.GetNullString(cmbCustomerCode.Text.Trim());
            gvCVInfo.DataSource = myFunc.FillDataSetOrEmptyRow("sp_2W_Service_CusVeh_Select", param);
            gvCVInfo.DataBind();
            gvCVInfo.Enabled = true;

            lblMasg.Text = "New";
        }

       

        protected void btnClear_Click(object sender, EventArgs e)
        {
            cmbCustomerCode.Text = txtCustomerName.Text = "";
            cmbEndUser.Text = txtEndUserName.Text = "";
            dpRegDate.Text = txtRegNo.Text = "";
            txtVersion.Text = txtEngNo.Text = "";
            //txtBrand.Text = txtBrandName.Text = "";
            cmbProdCode.Text = txtChassisNo.Text = "";
            txtProdDesc.Text = "";
            cmbColorCode.Text = ""; txtInvoiceNo.Text = ""; dpInvoiceDate.Text = "";
            txtColorName.Text = "";
            cmbInsCode.Text = "";
            cmbBrand.Text = "";
            //txtInsuranceName.Text = grl.GetStringValuesAgainstCodes("cusCode", txtCustomerCode.Text, "CusDesc+' '+FatherHusName", "Customer");  
            txtInsuranceName.Text = "";
            txtPolicyNo.Text = "";

            //FillBlankRows(1);
            cmbCustomerCode.Enabled = true;
            cmbEndUser.Enabled = false;
            dpRegDate.Enabled = false;
            txtRegNo.Enabled = false;
            txtVersion.Enabled = false;
            txtEngNo.Enabled = false;
            cmbProdCode.Enabled = false;
            txtChassisNo.Enabled = false;
            cmbColorCode.Enabled = false;
            dpInvoiceDate.Enabled = false;
            cmbInsCode.Enabled = false;
            txtInvoiceNo.Enabled = false;
            txtPolicyNo.Enabled = false;
            cmbBrand.Enabled = false;

            btnSave.Enabled = false;
            btnDelete.Enabled = false;

            gvCVInfo.DataSource = null;
            gvCVInfo.DataBind();
            gvCVInfo.Enabled = false;
            lblMasg.Text = "";
        }
        void FillBlankRows(int rows)
        {
            gvCVInfo.DataSource = objSBL.AddBlankRows(new string[] { "CusCode", "tdDate", "RegNo","ChassisNo",
            "EngineNo","BrandCode","ProdCode","VersionCode","ColorCode","InvoiceNo","InvoiceDate"}, 0, rows);
            gvCVInfo.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (myFunc.CodeExists("JobCardMaster", "EngineNo", txtEngNo.Text.Trim(), " And GatePass='' "))
            {
                lblMSG.Text = "Can not update this Engine number use in Job Card "+ txtEngNo.Text;
                PopupControlMSG.ShowOnPageLoad=true;
                return;
            }
            else
            {
                lblMasg.Text = "";
            }
            if (myFunc.CodeExists("JobCardMaster", "ChassisNo", txtChassisNo.Text.Trim(), " And GatePass='' "))
            {
                lblMSG.Text = "Can not update this Chassis number use in Job Card " + txtChassisNo.Text;
                PopupControlMSG.ShowOnPageLoad = true;
                return;
            }
            else
            {
                lblMasg.Text = "";
            }
            //TextBox[] textBoxes = { txtCustomerCode, txtEndUser, txtRegDate, txtRegNo, txtEngineNo, txtChassisNo, txtProduct, txtInvoiceNo, txtInvoiceDate };
            if (txtCustomerName.Text.Trim() == "" || txtEndUserName.Text.Trim() == "" || txtChassisNo.Text.Trim() == "" || txtEngNo.Text.Trim() == "")
            {
                return;
            }
            else
            {
                /// Validate from SMIS Database of Engine number ///
                DataSet dsValidate = new DataSet();
                DateTime InvDate = dpInvoiceDate.Date;

                if (cmbBrand.Value.ToString().Trim() == "001")
                {
                    //if (InvDate >= Convert.ToDateTime("2009-10-01"))
                    //{
                    //    SqlParameter[] paramData = {
                    //                    new SqlParameter("@EngineNo",SqlDbType.VarChar,5),
                    //                    new SqlParameter ("@Chk" ,SqlDbType.Char,1)
                    //               };
                    //    paramData[0].Value = txtEngNo.Text;
                    //    paramData[1].Value = "Y";

                    //    dsValidate = SqlHelper.ExecuteDataset(GlobalVar.mCon, "sp_2W_Get_NewVehicleInfo_SDMS", paramData);

                    //    if (dsValidate.Tables[0].Rows.Count > 0)
                    //    {
                    //        if (dsValidate.Tables[0].Rows[0]["ChasisNo"].ToString() != txtChassisNo.Text.Trim())
                    //        {
                    //            lblMSG.Text = "Enter valid Chassis No."+ txtChassisNo.Text;
                    //            PopupControlMSG.ShowOnPageLoad = true;
                                
                    //            return;
                    //        }
                    //        if (dsValidate.Tables[0].Rows[0]["ProdCode"].ToString() != cmbProdCode.Text.Trim())
                    //        {
                    //            lblMSG.Text = "Enter valid Product Code." + cmbProdCode.Text;
                    //            PopupControlMSG.ShowOnPageLoad = true;
                    //            return;
                    //        }
                    //        //if(txtInvoiceNo.Text .Trim () >
                    //        //if (dsValidate.Tables[0].Rows[0]["InvoiceNo"].ToString() != txtInvoiceNo.Text.Trim())
                    //        //{
                    //        //    grl.UserMsg(lblMasg, Color.Red, "Enter valid InvoiceNo.", txtInvoiceNo );
                    //        //    return;
                    //        //}
                    //        //if (Convert.ToDateTime (dsValidate.Tables[0].Rows[0]["InvoiceDate"].ToString ()).ToString ("dd/MM/yyyy") != InvDate.ToString ("dd/MM/yyyy"))
                    //        //{
                    //        //    grl.UserMsg(lblMasg, Color.Red, "Enter valid InvoiceDate.", txtInvoiceDate );
                    //        //    return;
                    //        //}

                    //        if (dsValidate.Tables[0].Rows[0]["VersionCode"].ToString() != txtVersion.Text)
                    //        {
                    //            lblMSG.Text = "Enter valid Version Code" + txtVersion.Text;
                    //            PopupControlMSG.ShowOnPageLoad = true;
                    //            return;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        lblMSG.Text = "Enter valid Engine No." + txtEngNo.Text;
                    //        PopupControlMSG.ShowOnPageLoad = true;
                    //        return;
                    //    }
                    //}
                }
                ////////////////////////////////////////////////
                SqlParameter[] param = {                                 
                new SqlParameter("@DealerCode",SqlDbType.VarChar,5), //0        
                new SqlParameter("@CusCode",SqlDbType.VarChar,8),//1	 		                
                new SqlParameter("@EndUserCode",SqlDbType.Char,8),//2			                                
                new SqlParameter("@CtdDate",SqlDbType.DateTime),//3			                                 
                new SqlParameter("@RegNo",SqlDbType.VarChar,15),//4			 
                new SqlParameter("@ChassisNo",SqlDbType.VarChar,30),//5			 
                new SqlParameter("@EngineNo",SqlDbType.VarChar,30),//6			 
                new SqlParameter("@BrandCode",SqlDbType.Char,3),//7			 
                new SqlParameter("@ProdCode",SqlDbType.VarChar,10),//8			 
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
		        new SqlParameter("@UpdTerm",SqlDbType.VarChar,30)//20
                
    };
                // 101 = Not Available
                // 102 = Not Understand

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = cmbCustomerCode.Text.Trim().ToUpper();
                param[2].Value = cmbEndUser.Text.Trim().ToUpper();
                param[3].Value = myFunc.SaveDate(dpRegDate.Text);
                param[4].Value = txtRegNo.Text.Trim().ToUpper().ToUpper();
                param[5].Value = txtChassisNo.Text.Trim().ToUpper();
                param[6].Value = txtEngNo.Text.Trim().ToUpper();
                param[7].Value = cmbBrand.Value.ToString().Trim();
                param[8].Value = cmbProdCode.Text.Trim();
                param[9].Value = txtVersion.Text.Trim();
                param[10].Value = cmbColorCode.Text.Trim();
                param[11].Value = txtInvoiceNo.Text.Trim();
                param[12].Value = myFunc.SaveDate(dpInvoiceDate.Text);
                param[13].Value = cmbInsCode.Text.Trim();
                param[14].Value = txtPolicyNo.Text.Trim();
                param[15].Value = "102"; // new SqlParameter("@LossNo",SqlDbType.VarChar,15),			 
                param[16].Value = "101"; // new SqlParameter("@MeterReading",SqlDbType.VarChar,10),
                param[17].Value = "101"; // new SqlParameter("@MonthReading",SqlDbType.VarChar,4),
                param[18].Value = "101"; // new SqlParameter("@Remarks",SqlDbType.VarChar,100),			 
                param[19].Value = Session["UserName"].ToString().ToUpper();
                param[20].Value = Environment.MachineName;

                //select * from brand where BrandCode=''
                //select * from version Where Version=''                
                //select * from customer where CusCode=''

                //select * from vehicle where BrandCode=='' and ProdCode='' and VersionCode='' and ColorCode                
                //select * from color where ColorCode = ''
                try
                {
                    if ((myFunc.IsExist("CusCode", cmbCustomerCode.Text, "Customer", Session["DealerCode"].ToString()) == true))
                    {
                        if ((myFunc.IsExist("BrandCode", cmbBrand.Value.ToString(), "Brand", Session["DealerCode"].ToString()) == true))
                        {
                            if ((myFunc.IsExist("VersionCode", txtVersion.Text, "Vehicle", Session["DealerCode"].ToString()) == true))
                            {
                                // apStr = " and  versioncode=" + txtVersion.Text + " and ProdCode='" + txtProduct.Text + "'";
                                if ((myFunc.IsExist("ColorCode", cmbColorCode.Text, "Vehicle", Session["DealerCode"].ToString()) == true))
                                {

                                    if (!myFunc.IsExist("RegNo", txtRegNo.Text, "CustomerVehicle", Session["DealerCode"].ToString())
                                        & !myFunc.IsExist("ChassisNo", txtChassisNo.Text, "CustomerVehicle", Session["DealerCode"].ToString())
                                        & !myFunc.IsExist("EngineNo", txtEngNo.Text, "CustomerVehicle", Session["DealerCode"].ToString()))
                                    {

                                        if (myFunc.ExecuteSP_NonQuery("sp_CustomerVehicle_Insert", param))
                                        {
                                            lblMSG.Text = "Record Saved!" + txtVersion.Text;
                                            PopupControlMSG.ShowOnPageLoad = true;
                                            GetDataLoaded(true, false);
                                            btnDelete.Enabled = true;
                                        }
                                        else
                                        {
                                            lblMSG.Text = "Record Not Saved!";
                                            PopupControlMSG.ShowOnPageLoad = true;

                                        }
                                    }
                                    else
                                    {
                                        apStr = " and ChassisNo ='" + txtChassisNo.Text + "' and jobstatus = null and GatePass = null";
                                        if (myFunc.IsExist("ChassisNo", txtChassisNo.Text, "JobCardMaster", Session["DealerCode"].ToString())
                                            & myFunc.IsExist("EngineNo", txtEngNo.Text, "JobCardMaster", Session["DealerCode"].ToString()))
                                        {
                                            if (myFunc.IsExist("ChassisNo", txtChassisNo.Text, "JobCardMaster", Session["DealerCode"].ToString(), apStr))
                                            {
                                                lblMSG.Text = "Record can not be updated";
                                                PopupControlMSG.ShowOnPageLoad = true;
                                                return;
                                            }
                                            else
                                            {
                                                if (myFunc.ExecuteSP_NonQuery("sp_CustomerVehicle_Update", param))
                                                {
                                                    // objMBLL.ShowMessageBox("Record Updated", txtCustomerCode);
                                                    lblMSG.Text = "Record Updated";
                                                    PopupControlMSG.ShowOnPageLoad = true;
                                                    GetDataLoaded(true, false);
                                                }
                                                else
                                                {
                                                    lblMSG.Text = "Record Not updated";
                                                    PopupControlMSG.ShowOnPageLoad = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (myFunc.ExecuteSP_NonQuery("sp_CustomerVehicle_Update", param))
                                            {
                                                //objMBLL.ShowMessageBox("Record Updated", txtCustomerCode);
                                                lblMSG.Text = "Record updated";
                                                PopupControlMSG.ShowOnPageLoad = true;

                                                GetDataLoaded(true, false);
                                            }
                                            else
                                            {
                                                lblMSG.Text = "Record can not be updated";
                                                PopupControlMSG.ShowOnPageLoad = true;

                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //objMBLL.ShowMessageBox("'Version' not exist! ", txtCustomerCode);
                                lblMSG.Text = "Version not exist";
                                PopupControlMSG.ShowOnPageLoad = true;

                            }
                        }
                        else
                        {
                            lblMSG.Text = "Product not exist!";
                            PopupControlMSG.ShowOnPageLoad = true;

                        }
                    }
                    else
                    {
                        lblMSG.Text = "Brand not exist";
                        PopupControlMSG.ShowOnPageLoad = true;

                    }
                }
                catch(Exception ex)
                {
                    var LineNumber = new StackTrace(ex, true).GetFrame(0).GetFileLineNumber();
                    lblMSG.Text = "Error : " + ex.Message + "/"+ ex.StackTrace.ToString()+"/"+LineNumber.ToString();
                    PopupControlMSG.ShowOnPageLoad = true;
                    
                }
            }
        }
        private void GetDataLoaded(bool GridYN, bool FieldsYN)
        {
            try
            {
                DataSet ds = new DataSet();
                if (FieldsYN)
                {
                    if (hfLookup.Value == "C")
                    {
                        SqlParameter[] paramData = {
                                        new SqlParameter("@DealerCode",SqlDbType.VarChar,5), 
                                        new SqlParameter("@ChassisNo",SqlDbType.VarChar,50)
                                   };
                        paramData[0].Value = Session["DealerCode"].ToString();
                        paramData[1].Value = myFunc.GetNullString(txtChassisNo.Text.Trim());

                        ds = myFunc.FillDataSetOrEmptyRow("sp_W2_CustomerVehicle_OnChasis_Select", paramData);
                    }
                    else if (hfLookup.Value == "E")
                    {
                        SqlParameter[] paramData = {
                                        new SqlParameter("@DealerCode",SqlDbType.VarChar,5), 
                                        new SqlParameter("@EngineNo",SqlDbType.VarChar,50)
                                   };
                        paramData[0].Value = Session["DealerCode"].ToString();
                        paramData[1].Value = myFunc.GetNullString(txtEngNo.Text.Trim());

                        ds = myFunc.FillDataSetOrEmptyRow("sp_W2_CustomerVehicle_OnEngine_Select", paramData);
                    }
                    else
                    {
                        SqlParameter[] paramData = {
                                        new SqlParameter("@DealerCode",SqlDbType.VarChar,5), 
                                        new SqlParameter("@RegNo",SqlDbType.VarChar,50)
                                   };
                        paramData[0].Value = Session["DealerCode"].ToString();
                        paramData[1].Value = myFunc.GetNullString(txtRegNo.Text.Trim());

                        ds = myFunc.FillDataSetOrEmptyRow("sp_W2_CustomerVehicle_Select", paramData);
                    }
                    cmbCustomerCode.Text = ds.Tables[0].Rows[0]["CusCode"].ToString();
                    txtCustomerName.Text = myFunc.GetStringValuesAgainstCodes("cusCode", cmbCustomerCode.Text, "CusDesc+' '+FatherHusName", "Customer");
                    cmbEndUser.Text = ds.Tables[0].Rows[0]["EndUserCode"].ToString();
                    txtEndUserName.Text = myFunc.GetStringValuesAgainstCodes("cusCode", cmbEndUser.Text, "CusDesc+' '+FatherHusName", "Customer");
                    if (ds.Tables[0].Rows[0]["tdDate"].ToString() != "") dpRegDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["tdDate"].ToString()).ToString("dd-MM-yyyy");
                    txtRegNo.Text = ds.Tables[0].Rows[0]["RegNo"].ToString();
                    txtVersion.Text = ds.Tables[0].Rows[0]["VersionCode"].ToString();
                    txtEngNo.Text = ds.Tables[0].Rows[0]["EngineNo"].ToString();
                    cmbBrand.Value = ds.Tables[0].Rows[0]["BrandCode"].ToString();
                    //txtBrandName.Text = grl.GetStringValuesAgainstCodes("BrandCode", txtBrand.Text, "BrandDesc", "Brand");
                    cmbProdCode.Text = ds.Tables[0].Rows[0]["ProdCode"].ToString();

                    dsSub = new DataSet(); 
                    myFunc.CodeExists("vehicle", "ProdCode='" + cmbProdCode.Text.Trim() + "'", ref dsSub);
                    if (dsSub.Tables[0].Rows.Count > 0)
                    {
                        txtProdDesc.Text = dsSub.Tables[0].Rows[0]["ProdDesc"].ToString();
                        cmbColorCode.Text = dsSub.Tables[0].Rows[0]["ColorCode"].ToString();
                        txtColorName.Text = dsSub.Tables[0].Rows[0]["Color"].ToString();
                    }
                    else txtProdDesc.Text = cmbColorCode.Text = txtColorName.Text = "";

                    txtChassisNo.Text = ds.Tables[0].Rows[0]["ChassisNo"].ToString();
                    cmbColorCode.Text = ds.Tables[0].Rows[0]["ColorCode"].ToString();
                    //txtColorName.Text = grl.GetStringValuesAgainstCodes("ColorCode", cmbColorCode.Text, "ColorDesc", "Color");
                    txtInvoiceNo.Text = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();
                    if (ds.Tables[0].Rows[0]["InvoiceDate"].ToString() != "") dpInvoiceDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["InvoiceDate"].ToString()).ToString("dd-MM-yyyy");
                    cmbInsCode.Text = ds.Tables[0].Rows[0]["InsCompCode"].ToString();
                    //txtInsuranceName.Text = grl.GetStringValuesAgainstCodes("cusCode", txtCustomerCode.Text, "CusDesc+' '+FatherHusName", "Customer");  
                    txtPolicyNo.Text = ds.Tables[0].Rows[0]["InsPolicyNo"].ToString();
                    ds = new DataSet();
                    if (myFunc.CodeExists("vehicle", "ProdCode", cmbProdCode.Text, ref ds,Session["DealerCode"].ToString()))
                    {
                        txtColorName.Text = ds.Tables[0].Rows[0]["Color"].ToString();
                        txtProdDesc.Text = ds.Tables[0].Rows[0]["ProdDesc"].ToString();
                    }


                }
                SqlParameter[] param = {
                                        new SqlParameter("@DealerCode",SqlDbType.VarChar,5), 
                                        new SqlParameter("@CusCode",SqlDbType.VarChar,50)
                                   };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = myFunc.GetNullString(cmbCustomerCode.Text.Trim());
                if (GridYN)
                {
                    //DataSet ds = objMBLL.FillDataSetOrEmptyRow("sp_W2_CustomerVehicle_Select", param);
                    gvCVInfo.DataSource = myFunc.FillDataSetOrEmptyRow("sp_2W_Service_CusVeh_Select", param);
                    gvCVInfo.DataBind();
                }

                lblMasg.Text = "Editing";
            }
            catch (Exception ex) { throw ex; }
        }

        protected void selectButton_Click(object sender, EventArgs e)
        {
           
        }

        protected void cmbBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlDataSourceProduct.SelectCommand = "select  ProdCode as VehicleCode,VersionCode as[Version],ProdDesc as [Description],ColorCode,Color from vehicle where DealerCode IN ('AAAAA','" + Session["DealerCode"].ToString() + "') and BrandCode='" + cmbBrand.Value + "'";
            SqlDataSourceProduct.Select(DataSourceSelectArguments.Empty);
            SqlDataSourceProduct.DataBind();

            cmbProdCode.DataBind();
            cmbProdCode.Text = "";
           // txtProdDesc.Text = myFunc.getProductDesc(cmbBrand.Text);
            //loading prod combo
            //if (cmbBrand.Value == "0")
            //{
            //    cmbProdCode.Items.Clear();
            //    cmbProdCode.Text = "";
            //    txtProdDesc.Text = "";
            //    return;

            //}



            //string sql = "select  ProdCode as VehicleCode,VersionCode as[Version],ProdDesc as [Description],ColorCode,Color from vehicle where DealerCode IN ('AAAAA','"+Session["DealerCode"].ToString()+"') and BrandCode='"+cmbBrand.Value+"'";

            //DataTable dt = myFunc.GetData(sql);

            //cmbProdCode.Items.Clear();
            //if (dt.Rows.Count == 0)
            //{
            //    return;
            //}
            ////Adding top item
            //ListEditItem itm = new ListEditItem();
            //itm.Value = "0";
            //itm.Text = "--Select--";
            //cmbProdCode.Items.Add(itm);
            //cmbProdCode.SelectedIndex = 0;

            //// Add ItemsItems

            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    ListEditItem itms = new ListEditItem();
            //    itms.Value = dt.Rows[i]["VehicleCode"].ToString();
            //    itm.Text = dt.Rows[i]["Description"].ToString();
            //    cmbProdCode.Items.Add(itm);
            //}
        }



        

        protected void LoadVehicleRecord(string FieldName = "", string FieldValue = "")
        {
            string Criteria = "";
            if (FieldName != "")
            {
                if (FieldName == "RegNo")
                {
                    Criteria = "RegNo = '" + FieldValue+"'";
                }
                else if(FieldName == "EngNo")
                {
                    Criteria = "EngineNo = '" + FieldValue+"'";
                }
                else if (FieldName == "ChassisNo")
                {
                    Criteria = "ChassisNo = '" + FieldValue + "'";
                }
            }

            string sql = "select * from CustomerVehicle where " + Criteria;

            DataTable dt = myFunc.GetData(sql);

            if (dt.Rows.Count == 0)
            {
                return;
            }

            cmbCustomerCode.Text = dt.Rows[0]["CusCode"].ToString();
            txtCustomerName.Text = myFunc.GetStringValuesAgainstCodes("cusCode", cmbCustomerCode.Text, "CusDesc+' '+FatherHusName", "Customer");
            cmbEndUser.Text = dt.Rows[0]["EndUserCode"].ToString();
            txtEndUserName.Text = myFunc.GetStringValuesAgainstCodes("cusCode", cmbEndUser.Text, "CusDesc+' '+FatherHusName", "Customer");
            if (dt.Rows[0]["tdDate"].ToString() != "") dpRegDate.Date = Convert.ToDateTime(dt.Rows[0]["tdDate"].ToString());
            txtRegNo.Text = dt.Rows[0]["RegNo"].ToString();
            txtVersion.Text = dt.Rows[0]["VersionCode"].ToString();
            txtEngNo.Text = dt.Rows[0]["EngineNo"].ToString();
            cmbBrand.Value = dt.Rows[0]["BrandCode"].ToString();
            //txtBrandName.Text = grl.GetStringValuesAgainstCodes("BrandCode", txtBrand.Text, "BrandDesc", "Brand");
            cmbProdCode.Text = dt.Rows[0]["ProdCode"].ToString();

            dsSub = new DataSet();
            myFunc.CodeExists("vehicle", "ProdCode='" + cmbProdCode.Text.Trim() + "'", ref dsSub);
            if (dsSub.Tables[0].Rows.Count > 0)
            {
                txtProdDesc.Text = dsSub.Tables[0].Rows[0]["ProdDesc"].ToString();
                cmbColorCode.Text = dsSub.Tables[0].Rows[0]["ColorCode"].ToString();
                txtColorName.Text = dsSub.Tables[0].Rows[0]["Color"].ToString();
            }
            else txtProdDesc.Text = cmbColorCode.Text = txtColorName.Text = "";

            txtChassisNo.Text = dt.Rows[0]["ChassisNo"].ToString();
            cmbColorCode.Text = dt.Rows[0]["ColorCode"].ToString();
            txtColorName.Text = myFunc.GetStringValuesAgainstCodes("ColorCode", cmbColorCode.Text, "ColorDesc", "Color");
            txtInvoiceNo.Text = dt.Rows[0]["InvoiceNo"].ToString();
            if (dt.Rows[0]["InvoiceDate"].ToString() != "")
            {
                dpInvoiceDate.Date = Convert.ToDateTime(dt.Rows[0]["InvoiceDate"].ToString());
            }
            cmbInsCode.Text = dt.Rows[0]["InsCompCode"].ToString();
            txtInsuranceName.Text = myFunc.GetStringValuesAgainstCodes("InsCompCode", cmbInsCode.Text, "InsCompDescription", "InsuranceCompanies","",Session["DealerCode"].ToString());  
            txtPolicyNo.Text = dt.Rows[0]["InsPolicyNo"].ToString();

            btnDelete.Enabled = true;
            //ds = new DataSet();
            //if (myFunc.CodeExists("vehicle", "ProdCode", cmbProdCode.Text, ref ds, Session["DealerCode"].ToString()))
            //{
            //    txtColorName.Text = ds.Tables[0].Rows[0]["Color"].ToString();
            //    txtProdDesc.Text = ds.Tables[0].Rows[0]["ProdDesc"].ToString();
            //}
            lblMasg.Text = "Editing";
        }

        

        protected void cmbEndUser_TextChanged1(object sender, EventArgs e)
        {
            string ccode = cmbEndUser.Text;

            txtEndUserName.Text = myFunc.GetCustomerName(ccode, Session["DealerCode"].ToString());
        }
        protected void txtRegNo_TextChanged(object sender, EventArgs e)
        {
            if (txtRegNo.Text == "")
            {
                return;
            }

            if (!myFunc.IsExist("RegNo", txtRegNo.Text, "CustomerVehicle", Session["DealerCode"].ToString()))
            {
                return;
            }

            if (myFunc.IsExist("RegNo", txtRegNo.Text, "CustomerVehicle", Session["DealerCode"].ToString(), " And CusCode='" + cmbCustomerCode.Text + "'"))
            {
                //lblMSG.Text = "Registration Number " + txtRegNo.Text + " already exist for a Customer";
                //PopupControlMSG.ShowOnPageLoad = true;
                //return;
                LoadVehicleRecord("RegNo", txtRegNo.Text);
            }

            

        }
        protected void txtEngNo_TextChanged1(object sender, EventArgs e)
        {
            if (txtEngNo.Text == "")
            {
                return;
            }

            if (!myFunc.IsExist("EngineNo", txtEngNo.Text, "CustomerVehicle", Session["DealerCode"].ToString()))
            {
                return;
            }

            if (myFunc.IsExist("RegNo", txtEngNo.Text, "CustomerVehicle", Session["DealerCode"].ToString(), " And CusCode='" + cmbCustomerCode.Text + "'"))
            {
                //lblMSG.Text = "Engine Number " + txtEngNo.Text + " already exist for a Customer";
                //PopupControlMSG.ShowOnPageLoad = true;
                //return;
                LoadVehicleRecord("EngNo", txtEngNo.Text);
            }

            
        }

        protected void txtChassisNo_TextChanged(object sender, EventArgs e)
        {
            if (txtChassisNo.Text == "")
            {
                return;
            }

            if (!myFunc.IsExist("ChassisNo", txtChassisNo.Text, "CustomerVehicle", Session["DealerCode"].ToString()))
            {
                return;
            }

            if (myFunc.IsExist("ChassisNo", txtEngNo.Text, "CustomerVehicle", Session["DealerCode"].ToString(), " And CusCode='" + cmbCustomerCode.Text + "'"))
            {
                //lblMSG.Text = "Chasis Number " + txtChassisNo.Text + " already exist for a Customer";
                //PopupControlMSG.ShowOnPageLoad = true;
                //return;
                LoadVehicleRecord("ChassisNo", txtChassisNo.Text);
            }

            
        }

        protected void cmbProdCode_ButtonClick(object source, DevExpress.Web.ASPxEditors.ButtonEditClickEventArgs e)
        {

        }

        protected void cmbProdCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtProdDesc.Text = cmbProdCode.Items[cmbProdCode.SelectedIndex].GetValue("Description").ToString();
            txtVersion.Text = cmbProdCode.Items[cmbProdCode.SelectedIndex].GetValue("Version").ToString();
            cmbColorCode.Text = cmbProdCode.Items[cmbProdCode.SelectedIndex].GetValue("ColorCode").ToString();
            txtColorName.Text = cmbProdCode.Items[cmbProdCode.SelectedIndex].GetValue("Color").ToString();
        }

        protected void cmbColorCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtColorName.Text = cmbColorCode.Items[cmbColorCode.SelectedIndex].GetValue("ColorDesc").ToString();
        }

        protected void cmbInsCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtInsuranceName.Text = cmbInsCode.Items[cmbInsCode.SelectedIndex].GetValue("InsCompDescription").ToString();
        }

        protected void cmbEndUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtEndUserName.Text = cmbEndUser.Items[cmbEndUser.SelectedIndex].GetValue("Description").ToString();
        }

        protected void btnDCGV_Click(object sender, EventArgs e)
        {
            cmbCustomerCode.Text = gvCVInfo.GetRowValues(gvCVInfo.FocusedRowIndex, "CusCode").ToString();

            txtEngNo.Text = gvCVInfo.GetRowValues(gvCVInfo.FocusedRowIndex, "EngineNo").ToString();

            LoadVehicleRecord("EngNo", txtEngNo.Text);
            lblMasg.Text = "Editing";
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtEngNo.Text.Trim().Length == 0 |
                txtChassisNo.Text.Trim().Length == 0)
            {

                //grl.UserMsg(lblMasg, Color.Red, "Select a record to delete.", txtCustomerCode);
                lblMSG.Text = "Select a record to delete.";
                PopupControlMSG.ShowOnPageLoad = true;
                return;
            }

            if ((myFunc.IsExist("EngineNo", txtEngNo.Text, "JobCardMaster",Session["DealerCode"].ToString()) == true)
                | (myFunc.IsExist("ChassisNo", txtChassisNo.Text, "JobCardMaster", Session["DealerCode"].ToString()) == true)
                | (myFunc.IsExist("VehchassisNo", txtChassisNo.Text, "PPRMaster", Session["DealerCode"].ToString()) == true)
                | (myFunc.IsExist("VehEngineNo", txtEngNo.Text, "PPRMaster", Session["DealerCode"].ToString()) == true))
            {
                //grl.UserMsg(lblMasg, Color.Red, "Information is used in Jobcard, You can't delete.", txtCustomerCode);
                lblMSG.Text = "Information is used in Jobcard, You can't delete.";
                PopupControlMSG.ShowOnPageLoad = true;
                return;
            }
            else
            {
                SqlParameter[] param = {                                 
                new SqlParameter("@DealerCode",SqlDbType.VarChar,5),         
                new SqlParameter("@ChassisNo",SqlDbType.VarChar,30),			 
                new SqlParameter("@EngineNo",SqlDbType.VarChar,30)};

                param[0].Value = Session["DealerCode"].ToString().Trim();
                param[1].Value = txtChassisNo.Text;
                param[2].Value = txtEngNo.Text;
                if (myFunc.ExecuteSP_NonQuery("sp_CustomerVehicle_Delete", param))
                {
                    btnClear_Click(btnClear,EventArgs.Empty);
                    //grl.UserMsg(lblMasg, Color.Green, "Record Permenantly Deleted", txtCustomerCode);
                    lblMSG.Text = "Record Permenantly Deleted";
                    PopupControlMSG.ShowOnPageLoad = true;
                    //GetDataLoaded(true, true);
                }
                else { 
                        //grl.UserMsg(lblMasg, Color.Red, "Record Not Updated!", txtCustomerCode);
                        lblMSG.Text = "Record Not Updated";
                        PopupControlMSG.ShowOnPageLoad = true;
                    }

            }
        }

        protected void btnCustomerLookup_Click(object sender, EventArgs e)
        {
            cmbCustomerCode_TextChanged(cmbCustomerCode, EventArgs.Empty);
        }

        
    }
}