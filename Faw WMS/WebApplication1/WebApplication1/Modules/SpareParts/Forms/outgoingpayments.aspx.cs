using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Design;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using DXBMS.Data;
using System.Drawing;

using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;

namespace DXBMS.Modules.SpareParts.Forms
{
    public partial class outgoingpayments : System.Web.UI.Page
    {
        SysFunctions SysFunc = new SysFunctions();
        MainBLL ObjMainBll = new MainBLL();
        DataTable dt = new DataTable();
        DataTable dttax = new DataTable();
        SecurityBll sec = new SecurityBll();
        DataSet dsInvoice;
        DataSet dsTax;
        Transaction obj_trans = new Transaction();
        double totInvoice, totOutstanding, totTaxAmt, totAdjAmt, totInsAmt, TotRefAmt, totAdvAmount;
        double RunningTotal = 0.00;
        double SumOfSelectedInvoice = 0.00, Count = 0.00;
        string advbalamount="";
        clsLookUp clslook = new clsLookUp();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2560", "001"))
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
                LoadDDL_Bank();
                txtpayment_date.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtinstrument_date.Text = DateTime.Now.ToString("dd-MM-yyyy");
                LoadDDL_Vendor();
                LoadDDL_PaymentMode();
                LoadDDL_PaymentHead();
                
                LoadDDL_PaymentNo();
                
                ddlcostcenter.Items.Add(new ListItem("Select", ""));
                
                ddlinclude_adv.Items.Add(new ListItem("Select", ""));

                create_InvoiveDS();
                create_TaxDetailsDS();
                
                RadioButtonList1.SelectedIndex = 1;
               
                //if (RadioButtonList1.SelectedIndex == 1)
                //{
                //    ddlvendor.SelectedIndex = 3;
                //}
            }



            RadioButtonList2.SelectedIndex = 0;

            success_add_item.Visible = false;

            ddlinclude_adv.Enabled = false;
        }

        protected void cbinlude_adv_CheckedChanged(object sender, EventArgs e)
        {
            if (cbinlude_adv.Checked == true)
            {
                ddlinclude_adv.Enabled = true;
                if (ddlvendor.SelectedIndex != 0)
                {
                    string WhereQuery = " DealerCode ='" + Session["DealerCode"].ToString() + "' " +
                    "And VendorCode='" + ddlvendor.SelectedValue.ToString().Trim() + "' " +
                    "And  IsAdvancePayment='Y' " +
                    "And  PaymentType='Advance Payment To Vendor' " +
                    "And InsAmount - AdvanceAdjustedAmount >= 1";
                    string[] Columns = new string[] { "PaymentNo", "CONVERT(VARCHAR(10),PaymentDate,105)", "'Paid=' + Convert(Varchar(20),InsAmount)", "'Adj=' +CONVERT(VARCHAR(10),AdvanceAdjustedAmount)", "'Bal='+CONVERT(VARCHAR(10),AdvanceBalanceAmount)" };
                    SysFunc.GetMultiColumnsDDL(ddlinclude_adv, Columns, "OutgoingPaymentMaster", WhereQuery, "PaymentNo", "Order by PaymentNo Desc", true, false);
                }
            }
            else
                ddlinclude_adv.Enabled = false;
        }

        private void LoadDDL_Bank()
        {
            string WhereQuery = "1=1";

            string[] Columns = new string[] { "BankCode", "BankDesc" };
            SysFunc.GetMultiColumnsDDL(ddlbank, Columns, "Bank", WhereQuery, "BankCode", "", false, false);
        }

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            SysFunc.Clearddl(Page);
            SysFunc.ClearTextBoxes(Page);
            create_InvoiveDS();
            create_TaxDetailsDS();

            LoadDDL_Invoice();
            txtpayment_date.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtinstrument_date.Text = DateTime.Now.ToString("dd-MM-yyyy");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);
        }

        private void LoadDDL_Vendor()
        {
            SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                     };
            param[0].Value = Session["DealerCode"].ToString();
            ObjMainBll.FillDrp_SP(ddlvendor, "sp_2W_GetAllvendors", "VendorCode", "VendorDesc", param, true, "Select", false, "");
            ddlvendor.SelectedIndex = 0;
        }

        private void LoadDDL_Customer()
        {
            string WhereQuery = "DealerCode = " + Session["DealerCode"];

            string[] Columns = new string[] { "CusCode", "CusDesc", "NIC" };
            SysFunc.GetMultiColumnsDDL(ddlvendor, Columns, "Customer", WhereQuery, "CusCode", "", false, false);
        }

        private void LoadDDL_PaymentMode()
        {
            string WhereQuery = "1=1";

            string[] Columns = new string[] { "PayModeCode", "PayModeDesc" };
            SysFunc.GetMultiColumnsDDL(ddlpayment_mode, Columns, "PaymentMode", WhereQuery, "PayModeCode", "", false, false);
            ddlpayment_mode.SelectedValue = "C";
        }

        private void LoadDDL_PaymentHead()
        {
            //string WhereQuery = "1=1";

            //string[] Columns = new string[] { "ReceiptHeadCode", "ReceiptHeadDesc" };
            //SysFunc.GetMultiColumnsDDL(ddlpayment_head, Columns, "TaxPaymentReceiptHead", WhereQuery, "ReceiptHeadCode", "", false, false);

            ObjMainBll.FillDropDown(ddlpayment_head, "SELECT TaxHead,TaxID FROM TaxType where Type='Payment'", "TaxHead", "TaxID", "Select");
        }

        private void LoadDDL_Invoice()
        {
            string WhereQuery = "DealerCode = '" + Session["DealerCode"] + "' " +
                 "AND (TotalIncTax - TotPayment) > 1" +
                 " And VendorCode = '" + ddlvendor.SelectedValue + "'";

            string[] Columns = new string[] { "PurInvNo", "CONVERT(Varchar(10),PurInvDate,105)", "'GRN No: '+GRNNo", "'PRNP No: '+VendorInvNo" };
            SysFunc.GetMultiColumnsDDL(ddlinvoice_no, Columns, "PurInvMaster", WhereQuery, "PurInvNo", "Order by PurInvNo Desc", false, false);
        }

        private void LoadDDL_PaymentNo()
        {
            string WhereQuery = "DealerCode = '" + Session["DealerCode"] + "' AND DelFlag = 'N' ";

            string[] Columns = new string[] { "PaymentNo", "PaymentType" };
            SysFunc.GetMultiColumnsDDL(ddlpayment_no, Columns, "OutgoingPaymentMaster", WhereQuery, "PaymentNo", "Order by PaymentNo Desc", false, false);
        }

        //private void ShowPaymentHeadData()
        //{
        //    try
        //    {
        //        DataTable dt_paymenthead = new DataTable();
        //        string query = "SELECT ReceiptHeadCode, ReceiptHeadDesc from TaxPaymentReceiptHead Where ReceiptHeadCode = " + ddlpayment_head.SelectedItem.Value;

        //        dt_paymenthead = SysFunc.GetData(query);

        //        if (dt_paymenthead.Rows.Count > 0)
        //        {
        //            GridView_payment.DataSource = dt_paymenthead;
        //            GridView_payment.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        success_add_item.Visible = true;
        //        success_add_item.Text = ex.Message;
        //    }
        //}

        private void create_InvoiveDS()
        {
            dsInvoice = new DataSet();
            dsInvoice.Tables.Add("InvoiceDetail");
            dsInvoice.Tables["InvoiceDetail"].Columns.Add(new DataColumn("GRNNo", typeof(string)));
            dsInvoice.Tables["InvoiceDetail"].Columns.Add(new DataColumn("RefInvNo", typeof(string)));
            dsInvoice.Tables["InvoiceDetail"].Columns.Add(new DataColumn("RefInvDate", typeof(string)));
            dsInvoice.Tables["InvoiceDetail"].Columns.Add(new DataColumn("NetTotal", typeof(string)));
            dsInvoice.Tables["InvoiceDetail"].Columns.Add(new DataColumn("Outstanding", typeof(string)));
            dsInvoice.Tables["InvoiceDetail"].Columns.Add(new DataColumn("Adjustment", typeof(string)));
            dsInvoice.Tables["InvoiceDetail"].Columns.Add(new DataColumn("Remaining", typeof(string)));

            GridView_Invoice.DataSource = dsInvoice.Tables["InvoiceDetail"];
            GridView_Invoice.DataBind();

            Session["InvoiceDS"] = dsInvoice;

        }

        private void create_ROW_Invoice()
        {
            try
            {
                DataTable dt_newrow = new DataTable();

                SqlDataReader dr_invoice = null;

                SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                    new SqlParameter("@GRNNo",SqlDbType.Char),//1
                                    new SqlParameter("@VendorCode", SqlDbType.Char)//2
                                   };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlinvoice_no.SelectedItem.Value;
                param[2].Value = ddlvendor.SelectedItem.Value;

                SysFunc.ExecuteSP("SP_Select_GRNMaster", param, ref dr_invoice);

                dt_newrow.Load(dr_invoice);

                DataSet ds = new DataSet();

                ds = (DataSet)Session["InvoiceDS"];

                
                DataRow row = ds.Tables["InvoiceDetail"].NewRow();
                //double adj_amt = double.Parse(txtadd_adjamount.Text == "" ? "0" : txtadd_adjamount.Text);
                //double sum = Convert.ToDouble(txttotal_netamt.Text) - adj_amt;

                row["GRNNo"] = dt_newrow.Rows[0]["GRNNo"].ToString();
                row["RefInvNo"] = dt_newrow.Rows[0]["RefInvNo"].ToString();
                row["RefInvDate"] = dt_newrow.Rows[0]["RefInvDate"].ToString();
                row["NetTotal"] = dt_newrow.Rows[0]["NetTotal"].ToString();
                row["Outstanding"] = dt_newrow.Rows[0]["Outstanding"].ToString();
                row["Adjustment"] = dt_newrow.Rows[0]["Adjustment"].ToString();//txtadd_adjamount.Text == "" ? "0" : txtadd_adjamount.Text;
                row["Remaining"] = dt_newrow.Rows[0]["Remaining"].ToString();//sum.ToString();

                ds.Tables["InvoiceDetail"].Rows.Add(row);

                GridView_Invoice.DataSource = ds.Tables["InvoiceDetail"];
                GridView_Invoice.DataBind();

                Session["InvoiceDS"] = ds;
            }
            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.Text = ex.Message;
            }

        }

        private void ShowInvoiceData()
        {
            try
            {

                DataSet ds = (DataSet)Session["InvoiceDS"];

                SqlDataReader dr = null;

                SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                    new SqlParameter("@GRNNo",SqlDbType.Char),//1
                                    new SqlParameter("@VendorCode", SqlDbType.Char)//2
                                   };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlinvoice_no.SelectedItem.Value;
                param[2].Value = ddlvendor.SelectedItem.Value;

                ds = SysFunc.FillDataSet("SP_Select_GRNMaster", param);
                //dt.Load(dr);
                
                if (ds.Tables[0].Rows.Count == 0)
                {
                    ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                }
                else
                {                

                    GridView_Invoice.DataSource = ds;
                    GridView_Invoice.DataBind();
                }


                Session["InvoiceDS"] = ds;
            }
            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.Text = ex.Message;
            }
        }

        protected void ddlpayment_head_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    dsTax = (DataSet)Session["TaxDS"];

            //    if (dsTax.Tables["TaxDetail"].Rows.Count > 0)
            //    {
            //        foreach (DataRow dtRow in dsTax.Tables["TaxDetail"].Rows)
            //        {
            //            if (ddlpayment_head.SelectedItem.Value == dtRow["ReceiptHeadCode"].ToString())
            //            {
            //                success_add_item.Visible = true;
            //                success_add_item.Text = "ReceiptHeadCode : " + ddlpayment_head.SelectedItem.Value + " Already Selected";

            //                return;
            //            }
            //        }
            //    }

            //    create_ROW_TaxDetails();    
            //}
            //catch (Exception ex)
            //{
            //    success_add_item.Visible = true;
            //    success_add_item.Text = ex.Message;
            //}
        }

        protected void ddlinvoice_no_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlinvoice_no.SelectedIndex == 0)
            {
                success_add_item.Visible = true;
                success_add_item.Text = "Select Invoice No ";
                txttotal_netamt.Text = "";
                return;
            }

            try
            {
                DataTable dt_newrow = new DataTable();

                SqlDataReader dr_invoice = null;

                SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                    new SqlParameter("@PurInvNo",SqlDbType.Char),//1
                                    new SqlParameter("@VendorCode", SqlDbType.Char)//2
                                   };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlinvoice_no.SelectedValue.ToString().Trim();
                param[2].Value = ddlvendor.SelectedItem.Value.ToString().Trim();

                SysFunc.ExecuteSP("SP_Select_GRNMaster", param, ref dr_invoice);

                dt_newrow.Load(dr_invoice);

                txttotal_netamt.Text = dt_newrow.Rows[0]["Outstanding"].ToString();

            }
            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.Text = ex.Message;
            }

        }

        protected void btn_delete_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2560", "004"))
            {
                Response.Redirect("~/Test.aspx");
            }
            SqlTransaction trans = null;
            bool check = false;
            if (ddlpayment_no.SelectedIndex > 0)
            {
                try
                {
                if (SysFunc.CheckVoucherPostFlag(Session["DealerCode"].ToString(), txtVoucherNo.Text))
                {
                    SysFunc.UserMsg(success_add_item, Color.Red, "Can't Edit or Delete the Voucher '" + txtVoucherNo.Text + "' is already Posted");
                    return;
                }
                    SysFunc.UpdateJV(Session["DealerCode"].ToString(), txtVoucherNo.Text);
                    string sql = "UPDATE OutgoingPaymentMaster SET DelFlag = 'Y'  where PaymentNo = '" + ddlpayment_no.SelectedItem.Value + "'";

                    SysFunc.ExecuteQuery_NonQuery(sql);

                    if (obj_trans.BeginTransaction(ref trans) == true)
                    {

                    }
                    else
                    {
                        return;
                    }

                    SqlParameter[] param = {
                                           new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                           new SqlParameter("@GRNNo",SqlDbType.Char),//1
                                           new SqlParameter("@RefInvNo",SqlDbType.VarChar),//2
                                           new SqlParameter("@VendorCode",SqlDbType.Char),//3
                                           new SqlParameter("@RecTotal",SqlDbType.Float),//4
                                       };

                    for (int i = 0; i < GridView_Invoice.Rows.Count; i++)
                    {
                        Label lblgrn_no = (Label)GridView_Invoice.Rows[i].FindControl("lblGRNNo");
                        Label lblInvNo = (Label)GridView_Invoice.Rows[i].FindControl("lblInvoiceNo");
                        Label lblAdjAmt = (Label)GridView_Invoice.Rows[i].FindControl("lblAdjustment");


                        param[0].Value = Session["DealerCode"].ToString();
                        param[1].Value = lblgrn_no.Text;
                        param[2].Value = lblInvNo.Text;
                        param[3].Value = ddlvendor.SelectedItem.Value;
                        param[4].Value = Convert.ToDouble(lblAdjAmt.Text);


                        SysFunc.ExecuteSP_NonQuery("SP_Update_GRNMaster_RecTotal", param, trans);

                        check = true;

                    }

                    string del_paymentdetail = "delete from OutgoingPaymentDetail where PaymentNo = '" + ddlpayment_no.SelectedItem.Value + "'" +
                                                "AND DealerCode = '" + Session["DealerCode"].ToString() + "' AND TransNo = '" + ddlinvoice_no.SelectedItem.Value + "'";

                    SysFunc.ExecuteQuery_NonQuery(del_paymentdetail);

                    string del_taxdetail = "delete from OutgoingPaymentTaxDetail where PaymentNo = '" + ddlpayment_no.SelectedItem.Value + "'" +
                                                "AND DealerCode = '" + Session["DealerCode"].ToString() + "'";

                    SysFunc.ExecuteQuery_NonQuery(del_taxdetail);

                    if (check == true)
                    {
                        obj_trans.CommittTransaction(ref trans);
                    }
                    else
                    {
                        obj_trans.RollBackTransaction(ref trans);
                        return;
                    }


                    LoadDDL_PaymentNo();

                    SysFunc.ClearTextBoxes(Page);
                    SysFunc.Clearddl(Page);

                    create_InvoiveDS();
                    create_TaxDetailsDS();

                    success_add_item.Visible = true;
                    success_add_item.Text = "Deleted Successfully";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Deletealert()", true);


                }
                catch (Exception ex)
                {
                    success_add_item.Visible = true;
                    success_add_item.Text = ex.Message;
                    obj_trans.RollBackTransaction(ref trans);
                }
            }
            else
            {
                success_add_item.Visible = true;
                success_add_item.Text = "Select Payment No. ";
            }
        }
        public void CalSubTotal()
        {
            double totTaxAmt = 0;
            foreach (GridViewRow row in GridView_payment.Rows)
            {
                Label chkSelect = (Label)row.FindControl("lblTaxAmount");

                totTaxAmt = totTaxAmt + SysFunctions.CustomCDBL(chkSelect.Text);

            }
          
            totInsAmt = SysFunctions.CustomCDBL(txtins_amount.Text);
            totAdvAmount = SysFunctions.CustomCDBL(txtadvajdusted_amt.Text);
            totTaxAmt = +totTaxAmt;
            txttotal_amount.Text = (totInsAmt + totTaxAmt + totAdvAmount).ToString();





        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2560", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            string strAutoCode = string.Empty;
            SqlTransaction trans = null;
           
            
            bool check = false;

            string inc_advance = "N";

            if (cbinlude_adv.Checked == true)
            {
                inc_advance = "Y";
                advbalamount= (SysFunc.GetStringValuesAgainstCodes("PaymentNo", ddlinclude_adv.SelectedValue.ToString().Trim(), "AdvanceBalanceAmount", "OutgoingPaymentMaster", "", Session["DealerCode"].ToString()));
            }

            if (ddlpayment_mode.SelectedValue == "C")
            {
                RequiredFieldValidator2.Enabled = false;

            }
            else
            {
                RequiredFieldValidator2.Enabled = true;
                if (ddlbank.SelectedIndex == 0)
                {
                    success_add_item.Visible = true;
                    success_add_item.ForeColor = System.Drawing.Color.Red;
                    success_add_item.Text = "Bank is mandatory.";
                    return;
                }
            }

            try
            {
                if (obj_trans.BeginTransaction(ref trans) == true)
                {

                }
                else
                {
                    return;
                }

                if (ddlpayment_no.SelectedIndex == 0)
                {
                    //strAutoCode = SysFunc.GetNewMaxID("OutgoingPaymentMaster", "PaymentNo", 8, Session["DealerCode"].ToString());
                    strAutoCode = SysFunc.AutoGen("OutgoingPaymentMaster", "PaymentNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                }
                else
                {
                    if (ddlpayment_no.SelectedIndex != 0)
                    {
                        if (!sec.UserRight("2560", "003"))
                        {
                            Response.Redirect("~/Test.aspx");
                        }
                        strAutoCode = ddlpayment_no.SelectedItem.Value;
                    }
                }

                SqlParameter[] param = {
                                new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                new SqlParameter("@PaymentType",SqlDbType.VarChar),//1
                                new SqlParameter("@PaymentNo",SqlDbType.Char),//2
                                new SqlParameter("@PaymentDate",SqlDbType.DateTime),//3
                                new SqlParameter("@InvoiceType",SqlDbType.VarChar),//4
                                new SqlParameter("@CusCode",SqlDbType.Char),//5
                                new SqlParameter("@VendorCode ", SqlDbType.Char),//6
                                new SqlParameter("@Remarks",SqlDbType.VarChar),//7
                                new SqlParameter("@CBCode",SqlDbType.Char),//8
                                new SqlParameter("@PayModeCode",SqlDbType.Char),//9
		                        new SqlParameter("@InsNo" ,SqlDbType.VarChar),//10
		                        new SqlParameter("@InsDate" ,SqlDbType.DateTime),//11
		                        new SqlParameter("@InsAmount",SqlDbType.Float),//12
		                        new SqlParameter("@BankCode",SqlDbType.Char),//13
		                        new SqlParameter("@Branch" ,SqlDbType.VarChar),//14
		                        new SqlParameter("@IsAdjustAdvance",SqlDbType.Char),//15
		                        new SqlParameter("@AdvancePaymentNo" , SqlDbType.Char),//16
		                        new SqlParameter("@AdvanceAmountAdjusted",SqlDbType.Float),//17
		                        new SqlParameter("@InvTotal",SqlDbType.Float),//18
                                new SqlParameter("@OUTSTTotal",SqlDbType.Float),//19
                                new SqlParameter("@InvAdjTotal",SqlDbType.Float),//20
                                new SqlParameter("@DelFlag",SqlDbType.Char),//21
                                new SqlParameter("@VoucherNo",SqlDbType.VarChar),//22
                                new SqlParameter("@VoucherFlag",SqlDbType.Char),//23
                                new SqlParameter("@IsAdvancePayment",SqlDbType.Char),//24
                                new SqlParameter("@AdvanceAmount",SqlDbType.Float),//25
                                new SqlParameter("@AdvanceAdjustedAmount",SqlDbType.Float),//26
                                new SqlParameter("@AdvanceBalanceAmount",SqlDbType.Float),//27
                                new SqlParameter("@UpdUser",SqlDbType.VarChar),//28
                                new SqlParameter("@UpdTerm",SqlDbType.VarChar),//29
                                new SqlParameter("@CSCode",SqlDbType.Char),//30
                                };


                //DateTime payDate = DateTime.ParseExact(txtpayment_date.Text, "dd-MM-yyyy", null);
                //DateTime instDate = DateTime.ParseExact(txtinstrument_date.Text,"dd-MM-yyyy",null);

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = RadioButtonList1.SelectedItem.Value;
                param[2].Value = strAutoCode;
                param[3].Value = SysFunc.SaveDate(txtpayment_date.Text);
                param[4].Value = RadioButtonList2.SelectedItem.Value;
                param[5].Value = "";
                param[6].Value = ddlvendor.SelectedItem.Value;
                param[7].Value = txtremarks.Text;
                param[8].Value = "0";
                param[9].Value = ddlpayment_mode.SelectedItem.Value;
                param[10].Value = txtinstrument_no.Text;
                param[11].Value = txtinstrument_date.Text.Trim () == "" ? (Object)DBNull.Value :  SysFunc.SaveDate(txtinstrument_date.Text);//instDate.ToString("dd-MM-yyyy");
                param[12].Value = Convert.ToDecimal(txtins_amount.Text.Trim() == "" ? "0" : txtins_amount.Text.Trim());
                param[13].Value = ddlbank.SelectedItem.Value;
                param[14].Value = txtbranch.Text;
                param[15].Value = inc_advance;
                param[16].Value = ddlinclude_adv.SelectedValue;
                param[17].Value = Convert.ToDecimal(txtadvajdusted_amt.Text.Trim() == "" ? "0" : txtadvajdusted_amt.Text.Trim());
                param[18].Value = 0;
                param[19].Value = 0;
                param[20].Value = Convert.ToDecimal(txttotal_amount.Text.Trim() == "" ? "0" : txttotal_amount.Text.Trim());
                param[21].Value = "N";
                param[22].Value = "";
                param[23].Value = "N";
                if (RadioButtonList1.SelectedValue == "Advance Payment To Vendor")
                {
                    param[24].Value = "Y";
                    param[25].Value = SysFunctions.CustomCDBL(txtins_amount.Text);
                    param[26].Value = SysFunctions.CustomCDBL("0");
                    param[27].Value = SysFunctions.CustomCDBL(txtins_amount.Text == "" ? "0" : txtins_amount.Text);
                }
                else if (cbinlude_adv.Checked && RadioButtonList1.SelectedValue != "Advance Payment To Vendor")
                {
                    param[24].Value = "N";
                    param[25].Value = advbalamount;
                    param[26].Value = double.Parse(txtadvajdusted_amt.Text == "" ? "0" : txtadvajdusted_amt.Text);
                    param[27].Value = SysFunctions.CustomCDBL(advbalamount) - SysFunctions.CustomCDBL(txtadvajdusted_amt.Text == "" ? "0" : txtadvajdusted_amt.Text);
                }
                else
                {
                    param[24].Value = "";
                    param[25].Value = 0;
                    param[26].Value = 0;
                    param[27].Value = 0;
                }
                
                param[28].Value = "A";
                param[29].Value = "A";
                param[30].Value = ddlcostcenter.SelectedItem.Value;


                if (SysFunc.ExecuteSP_NonQuery("Sp_Insert_OutgoingPaymentMaster", param, trans) == true)
                {
                    if (cbinlude_adv.Checked)
                    {
                        SqlParameter[] UpdateAdvance_param = {                                            
                                                            /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                            /*1*/ new SqlParameter("@ReceiptNo",SqlDbType.Char,8),
                                                            /*2*/ new SqlParameter("@AdvanceReceiptNo",SqlDbType.Char,8),
                                                            /*3*/ new SqlParameter("@AdvanceAdjustedAmount",SqlDbType.Float)
                                                                    };
                        UpdateAdvance_param[0].Value = Session["DealerCode"].ToString();
                        UpdateAdvance_param[1].Value = ddlinclude_adv.SelectedValue.ToString();
                        UpdateAdvance_param[2].Value = strAutoCode;
                        UpdateAdvance_param[3].Value = double.Parse(txtadvajdusted_amt.Text == "" ? "0" : txtadvajdusted_amt.Text);
                        SysFunc.ExecuteSP_NonQuery("[sp_OutgoingPaymentMaster_UpdateOnAdvance]", UpdateAdvance_param, trans);
                    }
                    SqlParameter[] param2 = {
                                        new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                        new SqlParameter("@PaymentType",SqlDbType.VarChar),//1
                                        new SqlParameter("@PaymentNo",SqlDbType.Char),//2
                                        new SqlParameter("@InvoiceType",SqlDbType.VarChar),//3
                                        new SqlParameter("@InvoiceNo",SqlDbType.Char),//4
                                        new SqlParameter("@InvoiceDate",SqlDbType.DateTime),//5
                                        new SqlParameter("@InvAmount",SqlDbType.Float),//6
                                        new SqlParameter("@OutStAmount",SqlDbType.Float),//7
                                        new SqlParameter("@AdjAmount",SqlDbType.Float),//8
                                        new SqlParameter("@TransNo",SqlDbType.Char),//9
                                    };

                    SqlParameter[] param4 = {
                                           new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                           //new SqlParameter("@VendorCode",SqlDbType.Char),//1
                                           //new SqlParameter("@GRNNo",SqlDbType.Char),//2
                                           new SqlParameter("@PurInvNo",SqlDbType.Char),//3
                                           new SqlParameter("@TotPayment",SqlDbType.Float),//4
                                        };

                    for (int i = 0; i < GridView_Invoice.Rows.Count; i++)
                    {
                        Label lblgrn_no = (Label)GridView_Invoice.Rows[i].FindControl("lblGRNNo");
                        Label lblInvNo = (Label)GridView_Invoice.Rows[i].FindControl("lblInvoiceNo");
                        Label lblInvDate = (Label)GridView_Invoice.Rows[i].FindControl("lblInvDate");
                        Label lblNetTotal = (Label)GridView_Invoice.Rows[i].FindControl("txtInvAmt");
                        Label lblOutSt = (Label)GridView_Invoice.Rows[i].FindControl("txtOutstanding");
                        Label lblAdjAmt = (Label)GridView_Invoice.Rows[i].FindControl("lblAdjustment");


                        param2[0].Value = Session["DealerCode"].ToString();
                        param2[1].Value = RadioButtonList1.SelectedItem.Value;
                        param2[2].Value = strAutoCode;
                        param2[3].Value = RadioButtonList2.SelectedItem.Value;
                        param2[4].Value = lblInvNo.Text;
                        param2[5].Value = SysFunc.SaveDate(lblInvDate.Text);
                        param2[6].Value = Convert.ToDouble(lblNetTotal.Text.Trim());
                        param2[7].Value = Convert.ToDouble(lblOutSt.Text.Trim());
                        param2[8].Value = Convert.ToDouble(lblAdjAmt.Text.Trim());
                        param2[9].Value = lblgrn_no.Text;

                        param4[0].Value = Session["DealerCode"].ToString();
                        //param4[1].Value = ddlvendor.SelectedItem.Value;
                        //param4[2].Value = lblgrn_no.Text;
                        param4[1].Value = lblInvNo.Text;
                        param4[2].Value = lblAdjAmt.Text;

                        SysFunc.ExecuteSP_NonQuery("Sp_Insert_OutgoingPaymentDetail", param2, trans);
                        SysFunc.ExecuteSP_NonQuery("Sp_Insert_GRNMaster_RecTotal", param4, trans);
                    }

                    SqlParameter[] param3 = {
                                                    new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                                    new SqlParameter("@PaymentNo",SqlDbType.Char),//1
                                                    new SqlParameter("@ReceiptHead",SqlDbType.Char),//2
                                                    new SqlParameter("@Amount",SqlDbType.Float),//3
                                                    new SqlParameter("@AccountCode",SqlDbType.Char),//4
                                                };
                    for (int j = 0; j < GridView_payment.Rows.Count; j++)
                    {

                        Label lblReceiptHead = (Label)GridView_payment.Rows[j].FindControl("lblReceiptHead");
                        Label lblTaxAmount = (Label)GridView_payment.Rows[j].FindControl("lblTaxAmount");
                        Label lblAccountCode = (Label)GridView_payment.Rows[j].FindControl("lblAccountCode");

                        param3[0].Value = Session["DealerCode"].ToString();
                        param3[1].Value = strAutoCode;
                        param3[2].Value = lblReceiptHead.Text;
                        param3[3].Value = Convert.ToDouble(lblTaxAmount.Text.Trim());
                        param3[4].Value = lblAccountCode.Text.Trim();

                        SysFunc.ExecuteSP_NonQuery("Sp_Insert_OutgoingPaymentTaxDetail", param3, trans);
                    }

                    check = true;

                }
                else
                {
                    check = false;
                    return;
                }

                if (check == true)
                {
                    obj_trans.CommittTransaction(ref trans);
                }
                else
                {
                    obj_trans.RollBackTransaction(ref trans);
                    return;
                }

                SysFunc.ClearTextBoxes(Page);
                SysFunc.Clearddl(Page);
                create_InvoiveDS();
                create_TaxDetailsDS();

                success_add_item.Visible = true;
                success_add_item.Text = "Payment No: " + strAutoCode + " Addedd Successfully";

                LoadDDL_PaymentNo();
                //LoadDDL_Invoice();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);
            }
            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.Text = ex.Message;
                obj_trans.RollBackTransaction(ref trans);
            }
        }

        double total_sum = 0.00;
        double total_Outstanding = 0.00;
        double total_Adjustment = 0.00;
        double total_Remaining = 0.00;

        protected void GridView_Invoice_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && !(GridView_Invoice.EditIndex == e.Row.RowIndex))
                {
                    Label NetTotal = (Label)e.Row.FindControl("txtInvAmt");
                    Label Outstanding = (Label)e.Row.FindControl("txtOutstanding");
                    Label Adjustment = (Label)e.Row.FindControl("lblAdjustment");

                    Label Remaining = (Label)e.Row.FindControl("txtRemaining");

                    total_sum += Convert.ToDouble(NetTotal.Text);
                    total_Outstanding += Convert.ToDouble(Outstanding.Text);
                    total_Adjustment += Convert.ToDouble(Adjustment.Text);
                    total_Remaining += Convert.ToDouble(Remaining.Text);
                }
                if (e.Row.RowType == DataControlRowType.DataRow && (GridView_Invoice.EditIndex == e.Row.RowIndex))
                {
                    Label NetTotal = (Label)e.Row.FindControl("txtInvAmt");
                    TextBox txtAdj = (TextBox)e.Row.FindControl("txtAdjustment");
                    Label Outstanding = (Label)e.Row.FindControl("txtOutstanding");
                    Label Adjustment = (Label)e.Row.FindControl("lblAdjustment");

                    Label Remaining = (Label)e.Row.FindControl("txtRemaining");

                    total_sum += Convert.ToDouble(NetTotal.Text);
                    total_Outstanding += Convert.ToDouble(Outstanding.Text);
                    total_Adjustment += Convert.ToDouble(txtAdj.Text);
                    total_Remaining += Convert.ToDouble(Remaining.Text);
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label total_amount = (Label)e.Row.FindControl("txttotal_amt");
                    Label total_outstanding = (Label)e.Row.FindControl("txttotalOut_amt");
                    Label total_adjustment = (Label)e.Row.FindControl("txttotalAdj_amt");
                    Label total_remaining = (Label)e.Row.FindControl("txttotalRem_amt");

                    total_amount.Text = total_sum.ToString();
                    total_outstanding.Text = total_Outstanding.ToString();
                    total_adjustment.Text = total_Adjustment.ToString();
                    total_remaining.Text = total_Remaining.ToString();

                }
            }
            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.Text = ex.Message;
            }

        }

        protected void BtnRemove_Click(object sender, ImageClickEventArgs e)
        {

            DataSet dsdelete_invoice = new DataSet();

            try
            {
                dsdelete_invoice = (DataSet)Session["InvoiceDS"];

                ImageButton BtnLnk = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;

                dsdelete_invoice.Tables[0].Rows[rowGvIndex].Delete();
                GridView_Invoice.DataSource = dsdelete_invoice.Tables[0];
                GridView_Invoice.DataBind();
                dsdelete_invoice.Tables[0].AcceptChanges();
                Session["InvoiceDS"] = dsdelete_invoice;
                CalOutStanding();
            }
            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.Text = ex.Message;
            }

        }

        protected void btn_fifo_Click(object sender, EventArgs e)
        {
            //if (ddlinvoice_no.SelectedIndex == 0)
            //{
            //    success_add_item.Visible = true;
            //    success_add_item.Text = "Select Invoice No ";
            //    txttotal_netamt.Text = "";
            //    return;
            //}
            if (txtins_amount.Text == "")
            {
                // success_add_item.Visible = true;
                //  success_add_item.Text = "Enter Instrument Amount ";
                //  return;
                txtins_amount.Text = "0";
            }
            try
            {
                Label lblAdj_Total_Footer = (Label)GridView_Invoice.FooterRow.Cells[6].FindControl("txttotalAdj_amt");
                Label txttotal_amt_Footer = (Label)GridView_Invoice.FooterRow.Cells[6].FindControl("txttotal_amt");
                //txttotal_amount.Text = Convert.ToString(Convert.ToDouble(txttotal_amount.Text.Trim() == "" ? "0" : txttotal_amount.Text.Trim()) + double.Parse(txtins_amount.Text == "" ? "0" : txtins_amount.Text));

                decimal RunningTotal = decimal.Parse(txttotal_amount.Text.Trim().Replace("&nbsp;", "") == "" ? "0" : txttotal_amount.Text.Trim());
                if (Convert.ToDecimal(txttotal_amount.Text) < Convert.ToDecimal(txtins_amount.Text))
                {
                    success_add_item.Visible = true;
                    success_add_item.Text = "Adjusted amount should not be greater then Total Amount. ";
                    return;
                }
                if (double.Parse(txttotal_amount.Text.Trim().Replace("&nbsp;", "") == "" ? "0" : txttotal_amount.Text.Trim()) < 1)
                {
                    success_add_item.Visible = true;
                   success_add_item.Text = "Total amount should not be zero. " + txttotal_amount.Text;

                    return;

                }

                

                Label lblNet_Total = (Label)GridView_Invoice.FooterRow.Cells[4].FindControl("txttotal_amt");
                Label lblOutstanding_Total = (Label)GridView_Invoice.FooterRow.Cells[5].FindControl("txttotalOut_amt");
                Label lblAdjusted_Total = (Label)GridView_Invoice.FooterRow.Cells[6].FindControl("txttotalAdj_amt");
                Label lblRemaining_Total = (Label)GridView_Invoice.FooterRow.Cells[6].FindControl("txttotalRem_amt");
                double dOutStAmt = 0; 
                double Count = 0.00;
                if (RunningTotal > decimal.Parse(lblNet_Total.Text.Trim()) && GridView_Invoice.Rows.Count < 1)
                {
                    success_add_item.Visible = true;
                    success_add_item.Text = "Adjusted amount not greater then Total Amount ";

                    return;

                }
                if (RunningTotal > 0)
                {

                    foreach (GridViewRow row in GridView_Invoice.Rows)
                    {
                        Label lblgrn_no = (Label)row.FindControl("lblGRNNo");
                        Label lblInvNo = (Label)row.FindControl("lblInvoiceNo");
                        Label lblInvDate = (Label)row.FindControl("lblInvDate");
                        Label lblNetTotal = (Label)row.FindControl("txtInvAmt");
                        Label lblOutSt = (Label)row.FindControl("txtOutstanding");
                        Label lblAdjAmt = (Label)row.FindControl("lblAdjustment");
                        Label lblRem = (Label)row.FindControl("txtRemaining");

                        if (RunningTotal > decimal.Parse(lblNetTotal.Text.Trim().Replace("&nbsp;", "") == "" ? "0" : lblNetTotal.Text.Trim()))
                        {

                            lblAdjAmt.Text = lblNetTotal.Text;

                            lblRem.Text = (double.Parse(lblOutSt.Text) - double.Parse(lblAdjAmt.Text)).ToString();


                            RunningTotal = RunningTotal - Convert.ToDecimal(lblOutSt.Text.Trim());
                            //dOutStAmt = dOutStAmt - double.Parse(lblNetTotal.Text.Trim());

                            //lblOutSt.Text = lblRem.Text;
                            //txttotAdj.Text = row.Cells[5].Text;

                            //txtAdj.Value = "t";

                            Count = Count + double.Parse(lblAdjAmt.Text.Trim().Replace("&nbsp;", "") == "" ? "0" : lblAdjAmt.Text.Trim());

                            lblAdj_Total_Footer.Text = Count.ToString();
                            lblOutstanding_Total.Text = lblOutSt.Text;
                            lblRemaining_Total.Text = lblRem.Text;
                            // gvPendingInvoice.FooterRow.Cells[5].Text = Count.ToString();

                        }

                        else
                        {

                            lblAdjAmt.Text = RunningTotal.ToString();
                            //lblAdjAmt.Text = dOutStAmt.ToString();

                            lblRem.Text = (double.Parse(lblOutSt.Text) - double.Parse(lblAdjAmt.Text)).ToString();

                            //   txttotAdj.Text = lblAdjAmt.Text;
                            RunningTotal = RunningTotal - Convert.ToDecimal(lblOutSt.Text.Trim());
                            // txtAdj.Value = "t";
                            // lblOutSt.Text = lblRem.Text;
                            Count = Count + double.Parse(lblAdjAmt.Text.Trim().Replace("&nbsp;", "") == "" ? "0" : lblAdjAmt.Text.Trim());

                            lblAdj_Total_Footer.Text = Count.ToString();
                            lblOutstanding_Total.Text = lblOutSt.Text;
                            lblRemaining_Total.Text = lblRem.Text;

                            //  gvPendingInvoice.FooterRow.Cells[5].Text = Count.ToString();

                            break;

                        }

                    }
                }

            }
            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.Text = ex.Message;
            }

        }

        protected void BtnRemove_Tax_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsdelete_tax = new DataSet();

            try
            {
                dsdelete_tax = (DataSet)Session["TaxDS"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;

                dsdelete_tax.Tables["TaxDetail"].Rows[rowGvIndex].Delete();
                GridView_payment.DataSource = dsdelete_tax.Tables["TaxDetail"];
                GridView_payment.DataBind();
                dsdelete_tax.Tables["TaxDetail"].AcceptChanges();
                Session["TaxDS"] = dsdelete_tax;
                CalSubTotal();
            }
            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.Text = ex.Message;
            }
        }

        double totaltax_amount = 0.00;

        protected void GridView_payment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //
                if (e.Row.RowType == DataControlRowType.DataRow && (GridView_payment.EditIndex == e.Row.RowIndex))
                {
                    TextBox txtTaxAmt = (TextBox)e.Row.FindControl("txtTaxAmount");

                    totaltax_amount += Convert.ToDouble(txtTaxAmt.Text);
                }

                if (e.Row.RowType == DataControlRowType.DataRow && !(GridView_payment.EditIndex == e.Row.RowIndex))
                {
                    Label TaxTotal = (Label)e.Row.FindControl("lblTaxAmount");
                    //TextBox txtTaxAmt = (TextBox)e.Row.FindControl("txtTaxAmount");

                    totaltax_amount += SysFunctions.CustomCDBL(TaxTotal.Text);
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label totaltax_amt = (Label)e.Row.FindControl("txttotaltax_amt");

                    totaltax_amt.Text = totaltax_amount.ToString();

                }
            }
            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.Text = ex.Message;
            }
        }

        private void create_TaxDetailsDS()
        {
            dsTax = new DataSet();
            dsTax.Tables.Add("TaxDetail");
            dsTax.Tables["TaxDetail"].Columns.Add(new DataColumn("TaxID", typeof(string)));
            dsTax.Tables["TaxDetail"].Columns.Add(new DataColumn("TaxHead", typeof(string)));
            dsTax.Tables["TaxDetail"].Columns.Add(new DataColumn("AccountCode", typeof(string)));
            dsTax.Tables["TaxDetail"].Columns.Add(new DataColumn("Amount", typeof(string)));

            GridView_payment.DataSource = dsTax.Tables["TaxDetail"];
            GridView_payment.DataBind();

            Session["TaxDS"] = dsTax;

        }

        private void create_ROW_TaxDetails()
        {
            try
            {
                DataTable dt_newrow = new DataTable();

                //SqlDataReader dr_tax = null;

                //SqlParameter[] param = {
                //                    new SqlParameter("@ReceiptHeadCode",SqlDbType.Char),//0
                //                   };

                //param[0].Value = ddlpayment_head.SelectedItem.Value;

                //SysFunc.ExecuteSP("SP_Select_PaymentReceiptHead", param, ref dr_tax);

                //dt_newrow.Load(dr_tax);

                DataSet ds = new DataSet();

                ds = (DataSet)Session["TaxDS"];

                DataRow row = ds.Tables["TaxDetail"].NewRow();

                //row["ReceiptHeadCode"] = dt_newrow.Rows[0]["ReceiptHeadCode"].ToString();
                //row["ReceiptHeadDesc"] = dt_newrow.Rows[0]["ReceiptHeadDesc"].ToString();


                row["TaxID"] = ddlpayment_head.SelectedValue;
                row["TaxHead"] = ddlpayment_head.SelectedItem;
                row["AccountCode"] = txtAccountCode.Text;
                row["Amount"] = txtpaymenttax_amount.Text.Trim();

                ds.Tables["TaxDetail"].Rows.Add(row);

                GridView_payment.DataSource = ds.Tables["TaxDetail"];
                GridView_payment.DataBind();

                Session["TaxDS"] = ds;
            }
            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.Text = ex.Message;
            }

        }
        protected void GridView_payment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            DataSet ds = new DataSet();

            GridView_payment.EditIndex = -1;

            ds = (DataSet)Session["TaxDS"];

            GridView_payment.DataSource = ds.Tables[0];

            GridView_payment.DataBind();

        }

        protected void GridView_payment_RowEditing(object sender, GridViewEditEventArgs e)
        {

            try
            {
                DataSet ds = new DataSet();

                GridView_payment.EditIndex = e.NewEditIndex;

                ds = (DataSet)Session["TaxDS"];

                GridView_payment.DataSource = ds.Tables[0];

                GridView_payment.DataBind();

                //FillDCGV();

            }

            catch (Exception ex)
            {

                success_add_item.Visible = true;
                success_add_item.Text = ex.Message;

            }

        }

        double tax = 0.00;

        protected void GridView_payment_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataSet dsupdate_tax = new DataSet();

            try
            {
                TextBox taxamt = (TextBox)GridView_payment.Rows[e.RowIndex].FindControl("txtTaxAmount");
                Label tax_amt = (Label)GridView_payment.Rows[e.RowIndex].FindControl("txttotaltax_amt");
                Label lblreceiptcode = (Label)GridView_payment.Rows[e.RowIndex].FindControl("lblReceiptHead");

                tax = Convert.ToDouble(taxamt.Text.Trim() == "" ? "0" : taxamt.Text.Trim());

                if (tax <= 0)
                {
                    success_add_item.Visible = true;
                    success_add_item.Text = "Tax Amount not Zero(0).";
                    return;
                }

                dsupdate_tax = (DataSet)Session["TaxDS"];

                foreach (DataRow dr in dsupdate_tax.Tables[0].Rows)
                {
                    if (dr["ReceiptHeadCode"].ToString() == lblreceiptcode.Text)
                    {
                        dr["Amount"] = tax;
                    }
                }

                GridView_payment.EditIndex = -1;

                GridView_payment.DataSource = dsupdate_tax.Tables[0];
                GridView_payment.DataBind();

                Session["TaxDS"] = dsupdate_tax;
                CalSubTotal();
            }

            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.Text = ex.Message;
            }

        }

        protected void GridView_Invoice_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();

                GridView_Invoice.EditIndex = e.NewEditIndex;

                ds = (DataSet)Session["InvoiceDS"];

                GridView_Invoice.DataSource = ds.Tables[0];

                GridView_Invoice.DataBind();

            }

            catch (Exception ex)
            {

                success_add_item.Visible = true;
                success_add_item.Text = ex.Message;

            }

        }

        protected void GridView_Invoice_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataSet ds = new DataSet();

            GridView_Invoice.EditIndex = -1;

            ds = (DataSet)Session["InvoiceDS"];

            GridView_Invoice.DataSource = ds.Tables[0];

            GridView_Invoice.DataBind();
        }

        protected void GridView_Invoice_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataSet dsupdate_invoice = new DataSet();

            double adjusted_amt = 0.0;

            try
            {
                Label lblGRNno = (Label)GridView_Invoice.Rows[e.RowIndex].FindControl("lblGRNNo");
                TextBox txtAdjustment = (TextBox)GridView_Invoice.Rows[e.RowIndex].FindControl("txtAdjustment");
                Label lblAdjustment = (Label)GridView_Invoice.Rows[e.RowIndex].FindControl("lblAdjustment");
                Label lblNetTotal = (Label)GridView_Invoice.Rows[e.RowIndex].FindControl("txtInvAmt");
                Label lblRemaining = (Label)GridView_Invoice.Rows[e.RowIndex].FindControl("txtRemaining");

                adjusted_amt = Convert.ToDouble(txtAdjustment.Text.Trim() == "" ? "0" : txtAdjustment.Text.Trim());

                if (adjusted_amt <= 0)
                {
                    success_add_item.Visible = true;
                    success_add_item.Text = "Adjusted Amount not Zero(0).";
                    return;
                }

                if (adjusted_amt > Convert.ToDouble(lblNetTotal.Text))
                {
                    success_add_item.Visible = true;
                    success_add_item.Text = "Adjusted Amount not greater than Total Amount : " + lblNetTotal.Text;
                    return;
                }

                dsupdate_invoice = (DataSet)Session["InvoiceDS"];

                foreach (DataRow dr in dsupdate_invoice.Tables[0].Rows)
                {
                    if (dr["GRNNo"].ToString() == lblGRNno.Text)
                    {
                        dr["Adjustment"] = adjusted_amt;
                        dr["Remaining"] = Convert.ToDouble(dr["NetTotal"].ToString()) - adjusted_amt;
                    }
                }

                GridView_Invoice.EditIndex = -1;

                GridView_Invoice.DataSource = dsupdate_invoice.Tables[0];
                GridView_Invoice.DataBind();
                CalOutStanding();

                Session["TaxDS"] = dsupdate_invoice;
            }

            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.Text = ex.Message;
            }
        }

        protected void ib_addtax_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlpayment_head.SelectedIndex == 0)
            {
                success_add_item.Visible = true;
                success_add_item.Text = "Select Payment Head ";
                return;
            }

            dsTax = new DataSet();

            try
            {


                //Label tax_amt ; // = (Label)GridView_payment.Rows[e.RowIndex].FindControl("txttotaltax_amt");
                tax = Convert.ToDouble(txtpaymenttax_amount.Text.Trim() == "" ? "0" : txtpaymenttax_amount.Text.Trim());

                if (tax <= 0)
                {
                    success_add_item.Visible = true;
                    success_add_item.Text = "Tax Amount not Zero(0).";
                    return;
                }

                dsTax = (DataSet)Session["TaxDS"];

                foreach (DataRow dr in dsTax.Tables["TaxDetail"].Rows)
                {
                    if (ddlpayment_head.SelectedItem.Value == dr["TaxHead"].ToString())
                    {
                        success_add_item.Visible = true;
                        success_add_item.Text = "ReceiptHeadCode : " + ddlpayment_head.SelectedItem.Value + " Already Selected";
                        return;
                    }

                }

                Session["TaxDS"] = dsTax;
                create_ROW_TaxDetails();
                CalSubTotal();
               // txttotal_amount.Text =  Convert.ToString(Convert.ToDouble(txttotal_amount.Text.Trim() == "" ? "0" : txttotal_amount.Text.Trim()) + tax);

            }

            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.Text = ex.Message;
            }
        }

        protected void Ibadd_invoice_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlinvoice_no.SelectedIndex == 0)
            {
                success_add_item.Visible = true;
                success_add_item.Text = "Select Invoice No.";
                return;
            }

            double adj_amt = double.Parse(txtadd_adjamount.Text == "" ? "0" : txtadd_adjamount.Text);

            try
            {
                //if (adj_amt == 0)
                //{
                //    success_add_item.Visible = true;
                //    success_add_item.Text = "Adjusted Amount could not be Zero(0) or Null ";
                //    return;
                //}

                //if (adj_amt > 0 && adj_amt <= Convert.ToDouble(txttotal_netamt.Text))
                //{

                dsInvoice = (DataSet)Session["InvoiceDS"];

                float netamt = float.Parse(txttotal_netamt.Text);
                double netamnt = Double.Parse(txttotal_netamt.Text);

                double sum = netamnt - adj_amt;
                //double sum = Int32.Parse(txttotal_netamt.Text.Trim(), System.Globalization.CultureInfo.InvariantCulture) - adj_amt;
                //double sum = Convert.ToDouble(txttotal_netamt.Text, System.Globalization.CultureInfo.InvariantCulture) - adj_amt;

                //if (dsInvoice.Tables["InvoiceDetail"].Rows.Count == 0)
                //{
                //    foreach (DataRow dtRow in dsInvoice.Tables["InvoiceDetail"].Rows)
                //    {
                //        dtRow["Adjustment"] = adj_amt;
                //        dtRow["Remaining"] = sum;

                //        return;
                //    }

                //}

                foreach (DataRow dtRow in dsInvoice.Tables["InvoiceDetail"].Rows)
                {
                    if (ddlinvoice_no.SelectedItem.Value == dtRow["RefInvNo"].ToString())
                    {
                        success_add_item.Visible = true;
                        success_add_item.Text = "GRN No : " + ddlinvoice_no.SelectedItem.Value + " Already Selected";

                        return;
                    }

                }

                //}
                //else
                //{
                //    success_add_item.Visible = true;
                //    success_add_item.Text = "Adjusted Amount not greater than Total Amount : " + txttotal_netamt.Text;
                //    return;
                //}


                Session["InvoiceDS"] = dsInvoice;
                create_ROW_Invoice();


                //Label total_adjusted_footer = (Label)GridView_Invoice.FooterRow.Cells[6].FindControl("txttotalAdj_amt");
                //txttotal_amount.Text = Convert.ToString(Convert.ToDouble(txttotal_amount.Text.Trim() == "" ? "0" : txttotal_amount.Text.Trim()) + 
                //                        Convert.ToDouble(txttotal_netamt.Text.Trim() == "" ? "0" : txttotal_netamt.Text.Trim()));
                CalSubTotal();
                CalOutStanding();
                clearInvoice();

            }
            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.Text = ex.Message;
            }
        }
        public void CalOutStanding()
        {
            SumOfSelectedInvoice = 0;
            foreach (GridViewRow row in GridView_Invoice.Rows)
            {
                Label chkSelect = (Label)row.FindControl("txtOutstanding");
               
                    SumOfSelectedInvoice = SumOfSelectedInvoice + SysFunctions.CustomCDBL(chkSelect.Text);
                
            }
            txttotOutStanding.Text = SumOfSelectedInvoice.ToString();

        }

        public void clearInvoice()
        {
            ddlinvoice_no.SelectedIndex = 0;
            txttotal_netamt.Text = "";
        }
        protected void btn_print_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2560", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }
            try
            {
                if (ddlpayment_no.SelectedIndex == 0)
                {
                    success_add_item.Visible = true;
                    success_add_item.Text = "Please select Payment No.";
                    return;
                }


                ReportDocument RD = new ReportDocument();

                Data.DSReports objDsReports = new Data.DSReports();
                //DataSet_BMS objDsReports = new DataSet_BMS();


                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();

                string sql = "exec SP_Select_OutgoingTaxDetailReport '" + Session["DealerCode"].ToString() + "','" + ddlpayment_no.SelectedValue.ToString().Trim() + "'";

                string sql2 = "exec SP_Select_OutgoingPaymentReport '" + Session["DealerCode"].ToString() + "','" + ddlpayment_no.SelectedValue.ToString().Trim() + "'";

                dt = SysFunc.GetData(sql2);
                RD.PrintOptions.PaperSize = PaperSize.PaperA4;
                dt2 = SysFunc.GetData(sql);

                objDsReports.OutgoingPaymentReport.Load(dt.CreateDataReader());
                objDsReports.OutgoingPaymentTaxDetail.Load(dt2.CreateDataReader());


                RD.Load(Server.MapPath("../SpareReports/PaymentToVendorReport.rpt"));
                RD.OpenSubreport("PaymentToVendorTaxDetail.rpt");

                RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerNTN"].Text = "'" + Session["DealerNTN"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerSaleTaxNo"].Text = "'" + Session["DealerSaleTaxNo"].ToString() + "'";


                RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Session["Terminal"].ToString() + "'";
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "PAYMENT TO VENDOR" + "\"";
                RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
                RD.DataDefinition.FormulaFields["UserName"].Text = "'" + Session["UserName"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["FromCode"].Text = "'" + FDate + "'";
                RD.DataDefinition.FormulaFields["CurrentDate"].Text = "'" + System.DateTime.Now.ToString() + "'";
                RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
                RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
                //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";

                RD.SetDataSource(objDsReports.OutgoingPaymentReport.DataSet);
                string FilePath = Server.MapPath("~") + "\\Download\\";
                string FileName = "Report.pdf";
                string File = FilePath + FileName;

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
                stream.Dispose(); stream.Close();
                string URL = "../../../Download/PrintReport.aspx";

                string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";

                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

                //RD.Database.Tables[0].SetDataSource(dt);

                //// convert and show
                //string FilePath = Server.MapPath("~") + "\\Download\\";
                //string FileName = "PaymentToVendorReport" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                //string File = FilePath + FileName;

                //Session["RD"] = RD;


                //string URL;
                //URL = "../../../Download/rptViewerService.aspx?FileName=" + FileName;
                //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
                //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
            }
            catch (Exception ex)    
            {
                success_add_item.Visible = true;
                success_add_item.Text = ex.Message;
            }
        }

        protected void ddlpayment_no_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlpayment_no.SelectedIndex == 0)
            {
                success_add_item.Visible = true;
                success_add_item.Text = "Select Payment No ";
                SysFunc.Clearddl(Page);
                SysFunc.ClearTextBoxes(Page);

                create_InvoiveDS();
                create_TaxDetailsDS();

                return;
            }

            create_InvoiveDS();
            create_TaxDetailsDS();

            try
            {
                DataTable dt_payment = new DataTable();
                SqlDataReader dr_payment = null;

                SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                    new SqlParameter("@PaymentNo",SqlDbType.Char),//1
                                 };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlpayment_no.SelectedItem.Value;


                //Sp_Insert_OutgoingPaymentToVendor

                SysFunc.ExecuteSP("Sp_Select_OutgoingPaymentToVendor", param, ref dr_payment);

                dt_payment.Load(dr_payment);

                RadioButtonList1.SelectedValue = dt_payment.Rows[0]["PaymentType"].ToString();
                RadioButtonList2.SelectedValue = dt_payment.Rows[0]["InvoiceType"].ToString();
                ddlvendor.SelectedValue = dt_payment.Rows[0]["VendorCode"].ToString();
                txtpayment_date.Text = dt_payment.Rows[0]["PaymentDate"].ToString();
                txtVoucherNo.Text = dt_payment.Rows[0]["VoucherNo"].ToString();
                txtremarks.Text = dt_payment.Rows[0]["Remarks"].ToString();
                // ddlcostcenter.SelectedValue = dt_payment.Rows[0]["CSCode"].ToString();
                // ddlpayment_account.SelectedValue = dt_payment.Rows[0]["CBCode"].ToString();
                ddlpayment_mode.SelectedValue = dt_payment.Rows[0]["PayModeCode"].ToString();
               
                if (dt_payment.Rows[0]["BankCode"].ToString().Trim() == "0")
                {
                    ddlbank.SelectedIndex = 0;
                }
                else
                {
                    ddlpayment_mode.SelectedValue = dt_payment.Rows[0]["PayModeCode"].ToString();
                    ddlbank.SelectedValue = dt_payment.Rows[0]["BankCode"].ToString();
                }
                txtbranch.Text = dt_payment.Rows[0]["Branch"].ToString();
                txtinstrument_date.Text = dt_payment.Rows[0]["InsDate"].ToString();
                txtinstrument_no.Text = dt_payment.Rows[0]["InsNo"].ToString();
                txtins_amount.Text = dt_payment.Rows[0]["InsAmount"].ToString();
                txttotal_amount.Text = dt_payment.Rows[0]["InvAdjTotal"].ToString();
                //Invoice 

                //ddlinvoice_no.SelectedValue = dt_payment.Rows[0]["TransNo"].ToString();
                //txttotal_netamt.Text = dt_payment.Rows[0]["InvAmount"].ToString();

                DataSet ds = new DataSet();

               // ds = (DataSet)Session["InvoiceDS"];

                //DataRow row = ds.Tables["InvoiceDetail"].NewRow();

                //double adj_amt = double.Parse(dt_payment.Rows[0]["AdjAmount"].ToString().Trim());
                //double sum = Convert.ToDouble(txttotal_netamt.Text) - adj_amt;



                //row["GRNNo"] = dt_payment.Rows[0]["TransNo"].ToString();
                //row["RefInvNo"] = dt_payment.Rows[0]["InvoiceNo"].ToString();
                //row["RefInvDate"] = dt_payment.Rows[0]["InvoiceDate"].ToString();
                //row["NetTotal"] = dt_payment.Rows[0]["InvAmount"].ToString();
                //row["Outstanding"] = dt_payment.Rows[0]["OutStAmount"].ToString();
                //row["Adjustment"] = dt_payment.Rows[0]["AdjAmount"].ToString();
                //row["Remaining"] = sum.ToString();

                //ds.Tables["InvoiceDetail"].Rows.Add(row);

               ds= SysFunc.FillDataSet("sp_OutgoingPaymentDetail_Select", param);
                
                GridView_Invoice.DataSource = ds.Tables[0];
                GridView_Invoice.DataBind();

                Session["InvoiceDS"] = ds;

                //Label lblAdj_Total = (Label)GridView_Invoice.FooterRow.Cells[6].FindControl("txttotalAdj_amt");
                //txttotal_amount.Text = lblAdj_Total.Text;
                
                // Tax

                //ddlpayment_head.SelectedValue = dt_payment.Rows[0]["ReceiptHead"].ToString();

                //DataSet ds_tax = (DataSet)Session["TaxDS"];
                //sp_OutgoingPaymentTaxDetail_Select
                //DataRow row2 = ds_tax.Tables["TaxDetail"].NewRow();
                //row2["ReceiptHeadCode"] = dt_payment.Rows[0]["ReceiptHead"].ToString();
                //row2["ReceiptHeadDesc"] = dt_payment.Rows[0]["ReceiptHeadDesc"].ToString();
                //row2["Amount"] = dt_payment.Rows[0]["Amount"].ToString();

                //ds_tax.Tables["TaxDetail"].Rows.Add(row2);
                ds = new DataSet();
                ds = SysFunc.FillDataSet("sp_OutgoingPaymentTaxDetail_Select", param);
                GridView_payment.DataSource = ds.Tables [0];
                GridView_payment.DataBind();

                Session["TaxDS"] = ds;

            }
            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.Text = ex.Message;
            }
        }

        protected void txtadvajdusted_amt_TextChanged(object sender, EventArgs e)
        {
            CalSubTotal();
        }

        protected void txtins_amount_TextChanged(object sender, EventArgs e)
        {
            CalSubTotal();
        }

        protected void ddlinclude_adv_SelectedIndexChanged(object sender, EventArgs e)
        {
            String advAmount = SysFunc.GetStringValuesAgainstCodes("VendorCode", ddlvendor.SelectedValue.Trim(), "AdvanceBalanceAmount", "OutgoingPaymentMaster", "And IsAdvancePayment='Y'  And InsAmount - AdvanceAdjustedAmount >= 1 and PaymentNo='" + ddlinclude_adv.SelectedValue.ToString()+"'", Session["DealerCode"].ToString());
            txtadvajdusted_amt.Enabled = true;
            txtadvajdusted_amt.Text = advAmount;
            CalSubTotal();
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedValue == "Refund Advance Reciept To Customer")
            {
                lbl_Vendor.Text = "Customer";
                LoadDDL_Customer();
            }
            else
            {
                LoadDDL_Vendor();
                lbl_Vendor.Text = "Vendor";
            }
        }

        protected void ddlvendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDDL_Invoice();
        }

        protected void ddlpayment_account_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void btnGL_Click(object sender, EventArgs e)
        {
            string URL = "../../Service/Forms/GL.aspx?CusInv=" + ddlpayment_no.SelectedValue + "&Type=PV";
            string fullURL = "window.open('" + URL + "', '_blank', 'height=600,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
            //string URL = "../../Service/Forms/GL.aspx?CusInv=" + ddlpayment_no.SelectedValue + "&Type=PV";
         
        }

        protected void ddlpayment_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string WhereQuery = "CompanyCode = '"+ Session["DealerCode"].ToString()+"'";

            if(ddlpayment_mode.SelectedValue == "C"){
                WhereQuery = WhereQuery + " and ACType = '" + ddlpayment_mode.SelectedValue + "'";
            }

            string[] Columns = new string[] { "CBCode", "CBDesc", "AccountCode" };
            SysFunc.GetMultiColumnsDDL(ddlpayment_account, Columns, "CashBankSetup", WhereQuery, "CBCode", "", false, false);
        }

        protected void imgLookup_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 75;
            clslook.LU_Get_AccountCode(imgLookup, ViewState["lookupid"].ToString(), "CompCode = '" + Session["DealerCode"].ToString() + "'", "../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void SelectedPartDetail(string item)
        {
            try
            {
                txtAccountCode.Text = item;
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(success_add_item, Color.Red, ex.Message);
            }
        }

        //protected void btn_pendinginv_Click(object sender, EventArgs e)
        //{
        //    ShowInvoiceData();
        //}
    }
}