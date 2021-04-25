using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using PdfSharp.Pdf;

namespace DXBMS.Modules.SpareParts.Forms
{
    public partial class PaymentRefund : System.Web.UI.Page
    {


        SysFunction SysFuncs = new SysFunction();
        SysFunctions SysFunc = new SysFunctions();
        MainBLL ObjMainBll = new MainBLL();
        DataTable dt = new DataTable();
        DataTable dttax = new DataTable();
        SecurityBll sec = new SecurityBll();
        DataSet dsAdv;
        DataSet dsTax;
        Transaction obj_trans = new Transaction();

       
        clsLookUp clslook = new clsLookUp();
        Transaction ObjTrans = new Transaction();
        //General grl = new General();
        SqlTransaction Trans;
        MainBLL objMBLL = new MainBLL();
        string strReceiptNo;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {

                if (!string.IsNullOrEmpty((String)Session["LookUpData"]))
                {
                    SelectedPartDetail(Session["LookUpData"].ToString());
                }
            }

            if (!IsPostBack)
            {
                LoadDDL_Bank();
                txtRefundDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtinstrument_date.Text = DateTime.Now.ToString("dd-MM-yyyy");

                LoadDDL_PaymentMode();

                RbtnType_SelectedIndexChanged(null, null);
            }
            Session["LookUpData"] = string.Empty;

        }
        protected void SelectedPartDetail(string item)
        {
            try
            {
                txtRefundCode.Text = item;
                GetRefundDetail(item);

                
            }
            catch (Exception ex)
            {
             //   SysFunc.UserMsg(, Color.Red, ex.Message);
            }
        }
        public void GetRefundDetail(string item)
        {


            SqlParameter[] dsMasterParam = { new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                             new SqlParameter("@RefundNo",SqlDbType.Char,8),
                                        
        };
            dsMasterParam[0].Value = Session["DealerCode"].ToString();
            dsMasterParam[1].Value = item;


         DataSet dsJobCardMaster = new DataSet();
            dsJobCardMaster = SysFunc.FillDataSet("sp_PaymentRefund_Detail", dsMasterParam);
            if (dsJobCardMaster.Tables[0].Rows.Count > 0)
            {
                if(dsJobCardMaster.Tables[0].Rows[0]["RefundType"].ToString() == "Customer"){
                    RbtnType.SelectedValue = "Customer";
                    lblType.Text = "Customer";
                }
                else{
                    RbtnType.SelectedValue = "Vendor";
                    lblType.Text = "Vendor";
                }
                txtCusVenCode.Text = dsJobCardMaster.Tables[0].Rows[0]["VoucherNo"].ToString();
                txtCusVenCode.Text = dsJobCardMaster.Tables[0].Rows[0]["CusCode"].ToString();
                txtCUSVen.Text = dsJobCardMaster.Tables[0].Rows[0]["CusDesc"].ToString();
                txtRefundDate.Text = dsJobCardMaster.Tables[0].Rows[0]["RDate"].ToString();
                txtins_amount.Text = dsJobCardMaster.Tables[0].Rows[0]["InsAmount"].ToString();
                ddlpayment_mode.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["PayModeCode"].ToString();
                txtinstrument_no.Text = dsJobCardMaster.Tables[0].Rows[0]["InsNo"].ToString();
                txtinstrument_date.Text = SysFuncs.GetDate(dsJobCardMaster.Tables[0].Rows[0]["InsDate"].ToString());
                ddlbank.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["BankCode"].ToString().Trim();
                txtbranch.Text = dsJobCardMaster.Tables[0].Rows[0]["Branch"].ToString();
                txtInvType.Text = dsJobCardMaster.Tables[0].Rows[0]["ReceiptType"].ToString();
                txtBalance.Text = dsJobCardMaster.Tables[0].Rows[0]["AdvanceBalanceAmount"].ToString();
                txtAdvReceiptNo.Text = dsJobCardMaster.Tables[0].Rows[0]["AdvanceCode"].ToString();
                ddlReceiptNo.Visible = false;
                txtAdvReceiptNo.Visible = true;
                btnAdjAmt.Enabled = false;
                btn_save.Enabled = false;

            }

        }
        public void ClearAll()
        {
            txtAdvReceiptNo.Visible = false;
            txtRefundCode.Text = "";
            /// txtRefundDate.Text = "";
            /// 
            txtCusVenCode.Text = "";
            txtCUSVen.Text = "";
            txtins_amount.Text = "";
            LoadDDL_Bank();
            LoadDDL_PaymentMode();
            LoadDDL_CustPayment();
            txtinstrument_no.Text="";
            txtinstrument_date.Text = "";
            txtBalance.Text = "";
            txtInvType.Text = "";
            RbtnType.SelectedIndex = 0;
            lblType.Text = "";



        }
        private void LoadDDL_Bank()
        {
            string WhereQuery = "1=1";

            string[] Columns = new string[] { "BankCode", "BankDesc" };
            SysFunc.GetMultiColumnsDDL(ddlbank, Columns, "Bank", WhereQuery, "BankCode", "", false, false);
        }
        private void LoadDDL_CustPayment()
        {
            string WhereQuery = " DealerCode ='" + Session["DealerCode"].ToString() + "' " +
              // "And CusCode='" + ddlCust.SelectedValue.ToString().Trim() + "' " +
               "And  IsAdjustAdvance='Y' " +
               "And AmountPaid - AdvanceAdjustedAmount >= 1";
            string[] Columns = new string[] { "ReceiptNo", "CONVERT(VARCHAR(10),ReceiptDate,105)", "'Paid=' + Convert(Varchar(20),amountpaid)", "'Adj=' +CONVERT(VARCHAR(10),AdvanceAdjustedAmount)", "'Bal='+CONVERT(VARCHAR(10),AdvanceBalanceAmount)" };
            SysFuncs.GetMultiColumnsDDL(ddlReceiptNo, Columns, "PaymentReceiptMaster", WhereQuery, "ReceiptNo", "Order by ReceiptNo Desc", true, false);

        }
        
        private void LoadDDL_PaymentMode()
        {
            string WhereQuery = "1=1";

            string[] Columns = new string[] { "PayModeCode", "PayModeDesc" };
            SysFunc.GetMultiColumnsDDL(ddlpayment_mode, Columns, "PaymentMode", WhereQuery, "PayModeCode", "", false, false);
        }
        protected void ddlpayment_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string WhereQuery = "CompanyCode = '" + Session["DealerCode"].ToString() + "'";

            if (ddlpayment_mode.SelectedValue == "C")
            {
                WhereQuery = WhereQuery + " and ACType = '" + ddlpayment_mode.SelectedValue + "'";
            }

            string[] Columns = new string[] { "CBCode", "CBDesc", "AccountCode" };
            SysFunc.GetMultiColumnsDDL(ddlpayment_account, Columns, "CashBankSetup", WhereQuery, "CBCode", "", false, false);
        }
        protected void ddlpayment_account_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        protected void RbtnType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RbtnType.SelectedValue == "Customer")
            {
                Load_CustomerReceipt();
            }
           

        }
        public void Load_CustomerReceipt()
        {
            string WhereQuery = " DealerCode ='" + Session["DealerCode"].ToString() + "' " +
            "And TransType='Advance' " +
            "And  IsAdjustAdvance='Y' " +
            "And AdvanceBalanceAmount >= 1";
            string[] Columns = new string[] { "ReceiptNo", "CONVERT(VARCHAR(10),ReceiptDate,105)", "'Bal='+CONVERT(VARCHAR(10),AdvanceBalanceAmount)" };
            SysFunc.GetMultiColumnsDDL(ddlReceiptNo, Columns, "PaymentReceiptMaster", WhereQuery, "ReceiptNo", "Order by ReceiptNo Desc", true, false);
        }
        public void Load_VendorReceipt()
        {
            string WhereQuery = " DealerCode ='" + Session["DealerCode"].ToString() + "' " +
            "And TransType='Customer' " +
            "And  IsAdjustAdvance='Y' " +
            "And AdvanceBalanceAmount >= 1";
            string[] Columns = new string[] { "ReceiptNo", "CONVERT(VARCHAR(10),ReceiptDate,105)", "'Bal='+CONVERT(VARCHAR(10),AdvanceBalanceAmount)" };
            SysFunc.GetMultiColumnsDDL(ddlReceiptNo, Columns, "PaymentReceiptMaster", WhereQuery, "ReceiptNo", "Order by ReceiptNo Desc", true, false);
        }
        protected void ddlReceiptNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RbtnType.SelectedValue == "Customer")
            {
                dt = SysFunc.GetData("SP_GEt_AdvancePaymentReceipt_refund '" + Session["DealerCode"] + "','" + ddlReceiptNo.SelectedValue + "'");
                if (dt.Rows.Count > 0)
                {
                    lblType.Text = "Customer";
                    txtCusVenCode.Text= dt.Rows[0]["CusCode"].ToString();
                    txtCUSVen.Text = dt.Rows[0]["CusDesc"].ToString();
                    txtInvType.Text = dt.Rows[0]["InvoiceType"].ToString();
                    txtBalance.Text = dt.Rows[0]["AdvanceBalanceAmount"].ToString();
                }

                
            }
            else
            {
                lblType.Text = "Vendor";
            }
        }

        protected void btnAdjAmt_Click(object sender, EventArgs e)
        {
            if (txtins_amount.Text == "")
            {
                SysFunc.UserMsg(success_add_item,Color.Red,"Please Enter Amount First!",txtins_amount);
            }
            double InsAmt, AdvBal;
            AdvBal = SysFunctions.CustomCDBL(txtBalance.Text);
            InsAmt = SysFunctions.CustomCDBL(txtins_amount.Text);
            AdvBal = SysFunctions.CustomCDBL(AdvBal - InsAmt);
            txtBalance.Text = AdvBal.ToString();
            Session["Adj"] = "Adj";
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2588", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
          
            if (ddlReceiptNo.SelectedIndex == 0)
            {
                SysFuncs.UserMsg(success_add_item, Color.Red, "Please Select  the  Receipt First!");
                return;
            }
            if (ddlpayment_mode.SelectedIndex == 0)
            {
                SysFuncs.UserMsg(success_add_item, Color.Red, "Please Select  the  Payment Mode First!");
                return;
            }
            else
            {
                if (txtRefundCode.Text != "")
                {
                    if (!sec.UserRight("2588", "003"))
                    {
                        Response.Redirect("~/Test.aspx");
                    }
                }
               
            }
            if (Session["Adj"].ToString() == "Adj")
            {
                Insert_PaymentRefund();
            }
            else
            {
                SysFuncs.UserMsg(success_add_item, Color.Red, "Please Adjust Amount  First!");
                return;
            }

        }

        public bool Insert_PaymentRefund()
        {
            SqlParameter[] PmtRecMaster_param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),          /*1*/ new SqlParameter("@RefundNo",SqlDbType.Char,8),
           /*2*/ new SqlParameter("@RefundDate",SqlDbType.DateTime),       /*3*/ new SqlParameter("@AdvanceCode",SqlDbType.Char,8),
           /*4*/ new SqlParameter("@CusCode",SqlDbType.VarChar,8),         
                 
           /*5*/ new SqlParameter("@Remarks",SqlDbType.Char,100),           /*6*/ new SqlParameter("@PayModeCode",SqlDbType.Char,1),
           /*7*/ new SqlParameter("@InsNo",SqlDbType.VarChar,15),          /*8*/ new SqlParameter("@InsDate",SqlDbType.DateTime),
           /*9*/ new SqlParameter("@AmountPaid",SqlDbType.Float),          /*10*/ new SqlParameter("@BankCode",SqlDbType.Char,5),
           /*11*/ new SqlParameter("@Branch",SqlDbType.VarChar,50),       
                 
           /*12*/ new SqlParameter("@DelFlag",SqlDbType.Char,1),
           /*13*/ new SqlParameter("@UpdUser",SqlDbType.Char,50),           /*14*/ new SqlParameter("@UpdTerm",SqlDbType.Char,50),          
           /*15*/ new SqlParameter("@VoucherNo",SqlDbType.VarChar,50),      /*16*/ new SqlParameter("@VoucherFlag",SqlDbType.Char,1),
           /*17*/ new SqlParameter("@TransType",SqlDbType.VarChar,30),        /*17*/ new SqlParameter("@RefundType",SqlDbType.VarChar,30),





            };

            PmtRecMaster_param[0].Value = Session["DealerCode"].ToString();
            PmtRecMaster_param[2].Value = SysFuncs.SaveDate(txtRefundDate.Text); //EstimateCode        
            PmtRecMaster_param[3].Value = ddlReceiptNo.SelectedItem.Value;
            PmtRecMaster_param[4].Value = txtCusVenCode.Text;
           
            PmtRecMaster_param[5].Value = "";
           
            
           
          
            PmtRecMaster_param[6].Value = ddlpayment_mode.SelectedItem.Value;
            PmtRecMaster_param[7].Value = txtinstrument_no.Text.Trim().ToUpper();

            PmtRecMaster_param[8].Value = SysFuncs.SaveDate(txtinstrument_date.Text);
            PmtRecMaster_param[9].Value = txtins_amount.Text.Trim().Replace("&nbsp;", "") == "" ? "0" : txtins_amount.Text.Trim();
            PmtRecMaster_param[10].Value = ddlbank.SelectedItem.Value;
            PmtRecMaster_param[11].Value = txtbranch.Text.Trim().Replace("&nbsp;", "");

            PmtRecMaster_param[12].Value = "N";

            PmtRecMaster_param[13].Value = Session["UserName"].ToString();
            PmtRecMaster_param[14].Value = GlobalVar.mUserIPAddress;

            PmtRecMaster_param[15].Value = "";
            PmtRecMaster_param[16].Value = "N";
           
            PmtRecMaster_param[17].Value = txtInvType.Text;
            if(RbtnType.SelectedValue=="Customer")
            {
                PmtRecMaster_param[18].Value = "Customer";

            }
            else
            {
                PmtRecMaster_param[18].Value = "Vendor";
            }
            
            try
            {
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    if (txtRefundCode.Text == "")
                    {
                        strReceiptNo = SysFunc.AutoGen("RefundPayment", "RefundCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                    }
                    else
                    {
                        strReceiptNo = txtRefundCode.Text;
                    }

                    PmtRecMaster_param[1].Value = strReceiptNo;
                    if (SysFuncs.ExecuteSP_NonQuery("[sp_W2_PaymentRefund_Master_Insert]", PmtRecMaster_param, Trans))
                    {
                        SqlParameter[] UpdateAdvance_param = {                                            
                                                            /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                            /*1*/ new SqlParameter("@ReceiptNo",SqlDbType.Char,8),
                                                            /*2*/ new SqlParameter("@AdvanceReceiptNo",SqlDbType.Char,8),
                                                            /*3*/ new SqlParameter("@AdvanceAdjustedAmount",SqlDbType.Float)
                                                                    };
                        UpdateAdvance_param[0].Value = Session["DealerCode"].ToString();
                        UpdateAdvance_param[1].Value = ddlReceiptNo.SelectedValue.ToString();
                        UpdateAdvance_param[2].Value = strReceiptNo;
                        UpdateAdvance_param[3].Value = double.Parse(txtins_amount.Text == "" ? "0" : txtins_amount.Text);
                        SysFuncs.ExecuteSP_NonQuery("[sp_PaymentRefund_UpdateOnAdvance]", UpdateAdvance_param, Trans);
                        ObjTrans.CommittTransaction(ref Trans);
                        ClearAll();
                        
                        SysFuncs.UserMsg(success_add_item, Color.Green, "Record Saved Sucessfully against Refund No" + strReceiptNo );
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);
                        return true;
                       
                    }
                    else
                    {
                        ObjTrans.RollBackTransaction(ref Trans);
                        return false;
                       
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                SysFuncs.UserMsg(success_add_item, Color.Red, ex.Message);
                return false;
            }




        }
        protected void imgReceiptNo_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 92;
            clslook.LU_Get_RefundNo(imgReceiptNo, ViewState["lookupid"].ToString(), "DealerCode = '" + Session["DealerCode"].ToString() + "'", "../../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {


                if (SysFunc.IsExist("RefundPayment", txtRefundCode.Text, "PaymentReceiptMaster", Session["DealerCode"].ToString())){
                    SysFuncs.UserMsg(success_add_item, Color.Red, "You Can Not Delete it exist in PaymentReceipt");
                    return;
                }
                if (SysFunc.CheckVoucherPostFlag(Session["DealerCode"].ToString(), txtVoucherNo.Text))
                {
                    SysFuncs.UserMsg(success_add_item, Color.Red, "Can't Edit or Delete the Voucher '" + txtVoucherNo.Text + "' is already Posted");
                    return;
                }

                SqlParameter[] PmtRecMaster_param = { /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                        /*1*/ new SqlParameter("@ReceiptNo",SqlDbType.Char,8)
                                                    };
                PmtRecMaster_param[0].Value = Session["DealerCode"].ToString();
                PmtRecMaster_param[1].Value = txtRefundCode.Text;
                if (SysFuncs.ExecuteSP_NonQuery("[sp_W2_PaymentRefund_Master_Delete]", PmtRecMaster_param))
                {
                    SysFunc.UpdateJV(Session["DealerCode"].ToString(), txtVoucherNo.Text);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Deletealert()", true);
                }

            }
            catch(Exception ex)
            {
                SysFuncs.UserMsg(success_add_item, Color.Red, ex.Message);
                return;
            }

            }

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            ClearAll();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);
        }

        protected void btn_print_Click(object sender, EventArgs e)
        {
            ReportDocument RD = new ReportDocument();
            DXBMS.Data.DSReports objDsReports = new Data.DSReports();

            DataTable dt = new DataTable();
            if (txtRefundCode.Text == "")
            {
                SysFuncs.UserMsg(success_add_item, Color.Red, "Please select Receipt No.");
                return;
            }
            string sql = "exec sp_PaymentRefund_Print '" + Session["DealerCode"].ToString() + "','" + txtRefundCode.Text + "'";
            dt = SysFuncs.GetData(sql);
            objDsReports.sp_PaymentRefund_Print.Load(dt.CreateDataReader());
        
              
            RD.Load(Server.MapPath("~/Modules/SpareParts/SpareReports/rptPaymentRefund.rpt"));



            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "Report.pdf";
            string File = FilePath + FileName;
            RD.SetDataSource(objDsReports);
            PdfDocument outputDocument = new PdfDocument();
            Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            DirectoryInfo info = new DirectoryInfo(FilePath);
            if (!info.Exists)
            {
                info.Create();
            }

            string path = Path.Combine(FilePath, FileName);
            using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
            {
                stream.CopyTo(outputFileStream);
            }

            string URL = "../../../Download/PrintReport.aspx";

            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";

            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);


            // convert and show



            Session["RD"] = RD;
     


        }

        protected void btnGL_Click(object sender, EventArgs e)
        {
            string URL = "../../Service/Forms/GL.aspx?CusInv=" + txtRefundCode.Text + "&Type=RP";
            Session["CusInv"] = txtRefundCode.Text;
            Session["Type"] = "RP";
            string fullURL = "window.open('" + URL + "', '_blank', 'height=600,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
    }
}