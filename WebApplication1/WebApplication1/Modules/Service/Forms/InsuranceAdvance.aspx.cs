using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using PdfSharp.Pdf;

namespace DXBMS.Modules.Service.Forms
{
    public partial class InsuranceAdvance : System.Web.UI.Page
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
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

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
                LoadDDL_Bank();
                txtRefundDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtinstrument_date.Text = DateTime.Now.ToString("dd-MM-yyyy");

                LoadDDL_PaymentMode();
                objMBLL.FillDrp_SP(ddlInsCo, "sp_InsuranceCompanies_select", "InsCompCode", "InsCompDescription", null, true, "--Select--", false, "");
                /// RbtnType_SelectedIndexChanged(null, null);
            }

        }
        /// <summary>
        /// /Selected Part Detail 
        /// 
        /// </summary>
        ///////CLear Function 

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
            dsJobCardMaster = SysFunc.FillDataSet("sp_AdvanceInsurance_Detail", dsMasterParam);
            if (dsJobCardMaster.Tables[0].Rows.Count > 0)
            {
                
                txtVoucherNo.Text = dsJobCardMaster.Tables[0].Rows[0]["VoucherNo"].ToString();
               
                txtRefundDate.Text = dsJobCardMaster.Tables[0].Rows[0]["AdvanceDate"].ToString();
                txtins_amount.Text = dsJobCardMaster.Tables[0].Rows[0]["InsAmount"].ToString();
                ddlpayment_mode.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["PayModeCode"].ToString();
                txtinstrument_no.Text = dsJobCardMaster.Tables[0].Rows[0]["InsNo"].ToString();
                txtinstrument_date.Text = SysFuncs.GetDate(dsJobCardMaster.Tables[0].Rows[0]["InsDate"].ToString());
                ddlbank.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["BankCode"].ToString().Trim();
                txtbranch.Text = dsJobCardMaster.Tables[0].Rows[0]["Branch"].ToString();
                ddlInsCo.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["InsCompCode"].ToString().Trim();
                ddlInsCo_SelectedIndexChanged(null, null);
                ddlInsBranch.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["BranchCode"].ToString().Trim();
             
                btn_save.Enabled = false;

            }

        }
        public void ClearAll()
        {
        
            txtRefundCode.Text = "";
            /// txtRefundDate.Text = "";
            /// 
      
       
            txtins_amount.Text = "";
            LoadDDL_Bank();
            LoadDDL_PaymentMode();
         
            txtinstrument_no.Text = "";
            txtinstrument_date.Text = "";

         
       
         
        



        }


        private void LoadDDL_Bank()
        {
            string WhereQuery = "1=1";

            string[] Columns = new string[] { "BankCode", "BankDesc" };
            SysFunc.GetMultiColumnsDDL(ddlbank, Columns, "Bank", WhereQuery, "BankCode", "", false, false);
        }
      
        protected void ddlInsCo_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlParameter[] param = { new SqlParameter("@InsCompCode", SqlDbType.Char, 5) };
            param[0].Value = ddlInsCo.SelectedValue;
            objMBLL.FillDrp_SP(ddlInsBranch, "sp_2W_Branch_select", "BranchCode", "BranchDesc", param, true, "--Select--", false, "");
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

        //// Search Lookup
        protected void imgReceiptNo_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 97;
            clslook.LU_Get_InsAdvanceNo(imgReceiptNo, ViewState["lookupid"].ToString(), "DealerCode = '" + Session["DealerCode"].ToString() + "'", "../../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (ddlInsCo.SelectedIndex == 0)
            {
                SysFunc.UserMsgddl(success_add_item, Color.Red, "Please Select Insurance Co!", ddlInsCo);
                return;
            }
            if (ddlInsBranch.SelectedIndex == 0)
            {
                SysFunc.UserMsgddl(success_add_item, Color.Red, "Please Select Branch!", ddlInsBranch);
                return;
            }
            if (ddlpayment_mode.SelectedIndex == 0)
            {
                SysFunc.UserMsgddl(success_add_item, Color.Red, "Please Select Pay Mode!", ddlInsBranch);
                return;
            }
            if (ddlpayment_mode.SelectedValue == "Q")
            {
                if (ddlbank.SelectedIndex == 0)
                {
                    SysFunc.UserMsgddl(success_add_item, Color.Red, "Please Select Bank!", ddlbank);
                    return;
                }
                 
            }
            if (txtins_amount.Text == "")
            {
                SysFunc.UserMsgddl(success_add_item, Color.Red, "Please Select Pay Mode!", ddlInsBranch);
                return;
            }
            if (txtVoucherNo.Text != "")
            {
                SysFunc.UserMsg(success_add_item, Color.Red, "VoucherNo exist you can not Update Information!", txtVoucherNo);
                return;
            }
            if (Insert_PaymentRefund())
            {

            }
         

        }
        public bool Insert_PaymentRefund()
        {
            SqlParameter[] PmtRecMaster_param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),          /*1*/ new SqlParameter("@AdvanceID",SqlDbType.Char,8),
           /*2*/ new SqlParameter("@AdvanceDate",SqlDbType.DateTime),      
           /*3*/ new SqlParameter("@CusCode",SqlDbType.VarChar,8),           /*4*/ new SqlParameter("@InsCompCode",SqlDbType.Char,4),         
             /*5*/ new SqlParameter("@BranchCode",SqlDbType.Char,4),         
               /*6*/ new SqlParameter("@PayModeCode",SqlDbType.Char,1),
           /*7*/ new SqlParameter("@InsNo",SqlDbType.VarChar,15),          /*8*/ new SqlParameter("@InsDate",SqlDbType.DateTime),
           /*9*/ new SqlParameter("@AmountPaid",SqlDbType.Float),          /*10*/ new SqlParameter("@BankCode",SqlDbType.Char,5),
           /*11*/ new SqlParameter("@Branch",SqlDbType.VarChar,50),       
                 
           /*12*/ new SqlParameter("@DelFlag",SqlDbType.Char,1),
           /*13*/ new SqlParameter("@UpdUser",SqlDbType.Char,50),           /*14*/ new SqlParameter("@UpdTerm",SqlDbType.Char,50),          
           /*15*/ new SqlParameter("@VoucherNo",SqlDbType.VarChar,50),      /*16*/ new SqlParameter("@VoucherFlag",SqlDbType.Char,1),
             /*17*/ new SqlParameter("@Remarks",SqlDbType.VarChar,100),         /*18*/ new SqlParameter("@AdvanceBalanceAmount",SqlDbType.Float),






            };

            PmtRecMaster_param[0].Value = Session["DealerCode"].ToString();
            PmtRecMaster_param[2].Value = SysFuncs.SaveDate(txtRefundDate.Text); //EstimateCode        
            PmtRecMaster_param[3].Value = "";
            PmtRecMaster_param[4].Value = ddlInsCo.SelectedValue;

            PmtRecMaster_param[5].Value = ddlInsBranch.SelectedValue;




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
            PmtRecMaster_param[17].Value = txtRemarks.Text;
            PmtRecMaster_param[18].Value = txtins_amount.Text;



            try
            {
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    if (txtRefundCode.Text == "")
                    {
                        strReceiptNo = SysFunc.AutoGen("AdvancePayment", "AdvanceID", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                    }
                    else
                    {
                        strReceiptNo = txtRefundCode.Text;
                    }

                    PmtRecMaster_param[1].Value = strReceiptNo;
                    if (SysFuncs.ExecuteSP_NonQuery("[sp_W2_AdvancePayment_Master_Insert]", PmtRecMaster_param, Trans))
                    {
                       
                        ObjTrans.CommittTransaction(ref Trans);
                        ClearAll();

                        SysFuncs.UserMsg(success_add_item, Color.Green, "Record Saved Sucessfully against Advance No" + strReceiptNo);
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

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            ClearAll();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);
        }

        protected void btnGL_Click(object sender, EventArgs e)
        {
            string URL = "GL.aspx?CusInv=" + txtRefundCode.Text + "&Type=IA";
            string fullURL = "window.open('" + URL + "', '_blank', 'height=600,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }

        protected void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {


                if (SysFunc.IsExist("AdvanceReceiptNo", txtRefundCode.Text, "PaymentReceiptMaster", Session["DealerCode"].ToString(), " and InvoiceType='Insurance'")){
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
                if (SysFuncs.ExecuteSP_NonQuery("[sp_W2_InsuranceAdvance_Master_Delete]", PmtRecMaster_param))
                {
                    SysFunc.UpdateJV(Session["DealerCode"].ToString(), txtVoucherNo.Text);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Deletealert()", true);
                }

            }
            catch (Exception ex)
            {
                SysFuncs.UserMsg(success_add_item, Color.Red, ex.Message);
                return;
            }
        }

        protected void btn_print_Click(object sender, EventArgs e)
        {
            ReportDocument RD = new ReportDocument();
            DXBMS.Data.DSReports objDsReports = new Data.DSReports();

            DataTable dt = new DataTable();
            if (txtRefundCode.Text == "")
            {
                SysFuncs.UserMsg(success_add_item, Color.Red, "Please select Advance No.");
                return;
            }
            string sql = "exec sp_PaymentAdvanceIns_Print '" + Session["DealerCode"].ToString() + "','" + txtRefundCode.Text + "'";
            dt = SysFuncs.GetData(sql);
            objDsReports.sp_PaymentAdvanceIns_Print.Load(dt.CreateDataReader());


            RD.Load(Server.MapPath("~/Modules/Service/ServiceReports/rptAdvancePayment.rpt"));



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
    }
}