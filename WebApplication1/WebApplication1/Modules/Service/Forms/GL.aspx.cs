using CConn;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DXBMS.Modules.Service.Forms
{
    public partial class GL : System.Web.UI.Page
    {
        DataSet ds;
        SysFunction sysfun = new SysFunction();
        SysFunctions myFunc = new SysFunctions();
        string strAutoCode;
        MainBLL objMBLL = new MainBLL();
        ServiceBL objSBL = new ServiceBL();
        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;
        static string type ;
        static double totDebit = 0, totCredit = 0;
        string CCon = CConnection.GetConnStringForAccount();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!Page.IsPostBack)
            {
                //Create_Grid();
              

                if (Request.Params.Get("CusInv") != null)
                {
                    string leadId = Request.Params.Get("CusInv");
                    type = Request.Params.Get("Type");


                    initializeDDLs(ddlJournalNo);
                    //ddlJournalNo.SelectedIndex = 1;
                    //ddlJournalNo_SelectedIndexChanged(null, null);
                    if (type == "PR")
                    {
                        LoadPRGrid(leadId);
                    }
                    else if (type == "IA")
                    {
                        LoadIAGrid(leadId);
                    }
                    else if (type == "RP")
                    {
                        LoadRPGrid(leadId);
                    }
                    else if(type == "PRI")
                    {
                        LoadCIGrid(leadId);
                    }
                    else if (type == "DS")
                    {
                        LoadDSGrid(leadId);
                    }
                    else if (type == "FSB")
                    {
                        LoadFSBGrid(leadId);
                    }
                    else if (type == "PV")
                    {
                        LoadPVGrid(leadId);
                        
                    }

                    if (Session["VoucherNo"]==null || Session["VoucherNo"].ToString()==""){
                        txtVoucherDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                        txtVoucherNo.Text = GetNewVoucherNo("GVouMaster", "VouchNo", 3,"");
                    }
               

                    ViewState["ReceiptNo"] = leadId;

                    txtTotalDebit.Text = Math.Round((totDebit), 2).ToString();
                    txtTotalCredit.Text = Math.Round((totCredit), 2).ToString();

                    totCredit = totDebit = 0;
                }else
                {
                    string leadId = Request.Params.Get("CusInv");
                    type = Request.Params.Get("Type");

                    initializeDDLs(ddlJournalNo);
                    //ddlJournalNo.SelectedIndex = 1;
                    //ddlJournalNo_SelectedIndexChanged(null, null);
                    if (type == "PR")
                    {
                        LoadPRGrid(leadId);
                    }
                    else if (type == "RP")
                    {
                        LoadRPGrid(leadId);
                    }
                    else if (type == "PRI")
                    {
                        LoadCIGrid(leadId);
                    }
                    else if (type == "FSB")
                    {
                        LoadFSBGrid(leadId);
                    }
                    else if (type == "DS")
                    {
                        LoadDSGrid(leadId);
                    }
                    else if (type == "PV")
                    {
                        LoadPVGrid(leadId);
                       

                    }

                    if (Session["VoucherNo"] == null || Session["VoucherNo"].ToString() == "")
                    {
                        txtVoucherDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                        txtVoucherNo.Text = GetNewVoucherNo("GVouMaster", "VouchNo", 3,"");
                    }
                    ViewState["ReceiptNo"] = leadId;

                    txtTotalDebit.Text = Math.Round((totDebit), 2).ToString();
                    txtTotalCredit.Text = Math.Round((totCredit),2).ToString();

                    totCredit = totDebit = 0;
                }
            }
            Session["VoucherNo"] = null;
          
        }
        private void LoadFSBGrid(string leadId)
        {

            SqlParameter[] dsParamInv = {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                new SqlParameter("@BillNo",SqlDbType.Char,8)
            };

            dsParamInv[0].Value = Session["DealerCode"].ToString();
            dsParamInv[1].Value = leadId;

            totCredit = totDebit = 0;

            DataSet dsCustomerInvoice = new DataSet();
            dsCustomerInvoice = myFunc.FillDataSet("SP_FFIPDIMaster_JV", dsParamInv);

            if (dsCustomerInvoice.Tables[0].Rows.Count > 0)
            {
                string FPBillNo = dsCustomerInvoice.Tables[0].Rows[0]["FPBillNo"].ToString();
              
                double GSTAmount = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["GSTAmount"]), 2);
                double SubTotal = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["SubTotal"]), 2);

                double BillAmount = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["BillAmount"]), 2);
                double FurAMount = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["FurAMount"]), 2);
                double ExtraAmt = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["ExtraAmt"]), 2);
                double SSTAmount = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["SSTAmount"]), 2);


                string Naration = "Bill No : " + dsCustomerInvoice.Tables[0].Rows[0]["FPBillNo"].ToString().Trim();
                                 

                ViewState["RecPay"] = "Bill No : " + dsCustomerInvoice.Tables[0].Rows[0]["FPBillNo"].ToString().Trim();

                if (dsCustomerInvoice.Tables[0].Rows[0]["VoucherNo"].ToString().Trim() != "")
                {
                    lblText.Text = "Edit Mode";
                    txtVoucherNo.Text = dsCustomerInvoice.Tables[0].Rows[0]["VoucherNo"].ToString().Trim();
                    Session["VoucherNo"] = dsCustomerInvoice.Tables[0].Rows[0]["VoucherNo"].ToString().Trim();
                    txtVoucherDate.Text = GetVoucherDate(dsCustomerInvoice.Tables[0].Rows[0]["VoucherNo"].ToString().Trim());
                    txtVoucherDate.Enabled = false;
                    RBLTransType.Visible = false;
                }

                ds = new DataSet();

                ds.Tables.Add();

                ds.Tables[0].Columns.Add(new DataColumn("AccountCode", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("AccountTitle", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("Debit", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("Credit", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("Naration", typeof(string)));

                Session["JV"] = ds;

                //if (InvoiceAmount > 0)
                //{
                //    AddCustomerDebitAmount(InvoiceAmount, AccountCode, Customer, Naration);
                //}

                if (BillAmount > 0)
                {
                    string code = GetAccountCode("OEMAccount");
                    AddDebitAmount(BillAmount, code, Naration);
                }

                if (SubTotal > 0)
                {
                    string code = GetAccountCode("PDIFFIAccount");
                    AddCreditAmount(SubTotal, code, Naration);
                }

                

                if (GSTAmount > 0)
                {
                    string code = GetAccountCode("GSTAccount");
                    AddCreditAmount(GSTAmount, code, Naration);
                }

                if (SSTAmount > 0)
                {
                    string code = GetAccountCode("PSTAccount");
                    AddCreditAmount(SSTAmount, code, Naration);
                }
                if (FurAMount > 0)
                {
                    string code = GetAccountCode("FurtherAccount");
                    AddCreditAmount(FurAMount, code, Naration);
                }

                if (ExtraAmt > 0)
                {
                    string code = GetAccountCode("ExtraTax");
                    AddCreditAmount(ExtraAmt, code, Naration);
                }


            }
        }
        private void LoadDSGrid(string leadId)
        {

            SqlParameter[] dsParamInv = {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                new SqlParameter("@SaleInvNo",SqlDbType.Char,8)
            };

            dsParamInv[0].Value = Session["DealerCode"].ToString();
            dsParamInv[1].Value = leadId;

            totCredit = totDebit = 0;

            DataSet dsCustomerInvoice = new DataSet();
            dsCustomerInvoice = myFunc.FillDataSet("SP_CounterSaleMaster_JV", dsParamInv);

            if (dsCustomerInvoice.Tables[0].Rows.Count > 0)
            {
                string AccountCode = dsCustomerInvoice.Tables[0].Rows[0]["AccountCode"].ToString();
                string Customer = dsCustomerInvoice.Tables[0].Rows[0]["CusDesc"].ToString().Trim();
                double GSTAmount =      Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["GSTAmount"]),2);
                double Discount =       Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["Discount"]), 2);
                double FurtherAmount =  Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["FurtherAmount"]), 2);
                double OtherCharges =   Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["OtherCharges"]), 2);
                double InvoiceAmount =  Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["InvoiceAmount"]), 2);
                double Amount =         Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["Amount"]), 2);
                double FreightCharges = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["FreightCharges"]), 2);
                double pCkd =           Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["PC.K.D"]), 2);
                double pLocal =         Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["PLocal"]), 2);
                double pMarket =        Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["PMarket"]), 2);
                double pImport =        Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["PImport"]), 2);
                double lCkd =           Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["LC.K.D"]), 2);
                double lLocal =         Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["LLocal"]), 2);
                double lMarket =        Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["LMarket"]), 2);
                double lImport =        Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["LImport"]), 2);


                string Naration = "Sale Invoice : " + dsCustomerInvoice.Tables[0].Rows[0]["SaleInvNo"].ToString().Trim() + " | " +
                                  "Customer : " + dsCustomerInvoice.Tables[0].Rows[0]["CusDesc"].ToString().Trim();

                ViewState["RecPay"] = dsCustomerInvoice.Tables[0].Rows[0]["CusDesc"].ToString().Trim();

                if (dsCustomerInvoice.Tables[0].Rows[0]["VoucherNo"].ToString().Trim() != "")
                {
                    lblText.Text = "Edit Mode";
                    txtVoucherNo.Text = dsCustomerInvoice.Tables[0].Rows[0]["VoucherNo"].ToString().Trim();
                    Session["VoucherNo"] = dsCustomerInvoice.Tables[0].Rows[0]["VoucherNo"].ToString().Trim();
                    txtVoucherDate.Text = GetVoucherDate(dsCustomerInvoice.Tables[0].Rows[0]["VoucherNo"].ToString().Trim());
                    txtVoucherDate.Enabled = false;
                    RBLTransType.Visible = false;
                }

                ds = new DataSet();

                ds.Tables.Add();

                ds.Tables[0].Columns.Add(new DataColumn("AccountCode", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("AccountTitle", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("Debit", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("Credit", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("Naration", typeof(string)));

                Session["JV"] = ds;

                //if (InvoiceAmount > 0)
                //{
                //    AddCustomerDebitAmount(InvoiceAmount, AccountCode, Customer, Naration);
                //}

                if (InvoiceAmount > 0)
                {
                    AddDebitAmount(InvoiceAmount, "", Naration);
                }

                if (FurtherAmount > 0)
                {
                    string code = GetAccountCode("FurtherAccount");
                    AddCreditAmount(FurtherAmount, code, Naration);
                }

                if (OtherCharges > 0)
                {
                    string code = GetAccountCode("OtherChargesAccount");
                    AddCreditAmount(OtherCharges, code, Naration);
                }

                if (GSTAmount > 0)
                {
                    string code = GetAccountCode("GSTAccount");
                    AddCreditAmount(GSTAmount, code, Naration);
                }

                if (pCkd > 0)
                {
                    string code = GetAccountCode("PartsSaleIncome(CKD)");
                    AddCreditAmount(pCkd, code, Naration);
                }
                if (pLocal > 0)
                {
                    string code = GetAccountCode("PartsSaleIncome(Local)");
                    AddCreditAmount(pLocal, code, Naration);
                }
                if (pMarket > 0)
                {
                    string code = GetAccountCode("PartsSaleIncome(Market)");
                    AddCreditAmount(pMarket, code, Naration);
                }
                if (pImport > 0)
                {
                    string code = GetAccountCode("PartsSaleIncome(Imported)");
                    AddCreditAmount(pImport, code, Naration);
                }

                if (lLocal > 0)
                {
                    string code = GetAccountCode("LubricnatSaleIncome(Local)");
                    AddCreditAmount(lLocal, code, Naration);
                }
                if (lMarket > 0)
                {
                    string code = GetAccountCode("LubricnatSaleIncome(Market)");
                    AddCreditAmount(lMarket, code, Naration);
                }
                if (lCkd > 0)
                {
                    string code = GetAccountCode("LubricnatSaleIncome(CKD)");
                    AddCreditAmount(lCkd, code, Naration);
                }
                if (FreightCharges > 0)
                {
                    string code = GetAccountCode("FreightChargesAccount");
                    AddCreditAmount(FreightCharges, code, Naration);
                }
                if (Discount > 0)
                {
                    string code = GetAccountCode("CashDiscountTaken");
                    AddDebitAmount(Discount, code, Naration);
                }
            }
        }
        private void LoadCIGrid(string leadId)
        {
            SqlParameter[] dsParamInv = {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                new SqlParameter("@InvoiceNo",SqlDbType.Char,8)
            };

            dsParamInv[0].Value = Session["DealerCode"].ToString();
            dsParamInv[1].Value = leadId;

            totCredit = totDebit = 0;

            DataSet dsCustomerInvoice = new DataSet();
            dsCustomerInvoice = myFunc.FillDataSet("sp_W2_CustomerInvoice_JV", dsParamInv);

            

            if (dsCustomerInvoice == null)
            {
                return;
            }
            else
            if (dsCustomerInvoice.Tables[0].Rows.Count > 0)
            {
                string AccountCode = dsCustomerInvoice.Tables[0].Rows[0]["AccountCode"].ToString();
                string Customer = dsCustomerInvoice.Tables[0].Rows[0]["CusDesc"].ToString().Trim();
                double GrossAmount =     Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["GrossAmount"]),2);
                double GSTAmount =       Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["GSTAmount"]), 2);
                double PSTAmount =       Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["SSTAmount"]), 2);
                double SubTotal =        Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["SubTotal"]), 2);
                double DiscLabor =       Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["DiscountAmount"]), 2);
                double TotalAmtCustomer =Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["NetAmountCustomer"]), 2);
                double DiscountParts =   Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["PartsDiscount"]), 2);
                double JobsTotal =       Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["JobsTotal"]), 2);
                double LubTotal =        Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["LubPartsTotal"]), 2);
                double PartsTotal =      Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["PartsTotal"]), 2);
                double SubletTotal =     Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["SubletTotal"]), 2);
                double pCkd =            Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["PC.K.D"]), 2);
                double pLocal =          Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["PLocal"]), 2);
                double pMarket =         Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["PMarket"]), 2);
                double pImport =         Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["PImport"]), 2);
                double lCkd =            Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["LC.K.D"]),2);
                double lLocal =          Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["LLocal"]), 2);
                double lMarket =         Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["LMarket"]),2);
                double lImport =         Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["LImport"]), 2);
                double Sublet =          Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["Sublet"]), 2);

                string Naration = "Jobcard : " + dsCustomerInvoice.Tables[0].Rows[0]["JobCardCode"].ToString().Trim() + " | " +
                                  "Invoice : " + dsCustomerInvoice.Tables[0].Rows[0]["InvoiceNo"].ToString().Trim() + " | " +
                                  "Customer : " + dsCustomerInvoice.Tables[0].Rows[0]["CusDesc"].ToString().Trim();

                ViewState["RecPay"] = dsCustomerInvoice.Tables[0].Rows[0]["CusDesc"].ToString().Trim();

                double discount = DiscountParts + DiscLabor;

                if (dsCustomerInvoice.Tables[0].Rows[0]["VoucherNo"].ToString().Trim() != "")
                {
                    lblText.Text = "Edit Mode";
                    txtVoucherNo.Text = dsCustomerInvoice.Tables[0].Rows[0]["VoucherNo"].ToString().Trim();
                    txtVoucherDate.Text = GetVoucherDate(dsCustomerInvoice.Tables[0].Rows[0]["VoucherNo"].ToString().Trim());
                    Session["VoucherNo"] = dsCustomerInvoice.Tables[0].Rows[0]["VoucherNo"].ToString().Trim();
                    txtVoucherDate.Enabled = false;
                }

                ds = new DataSet();

                ds.Tables.Add();

                ds.Tables[0].Columns.Add(new DataColumn("AccountCode", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("AccountTitle", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("Debit", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("Credit", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("Naration", typeof(string)));

                Session["JV"] = ds;

                if (TotalAmtCustomer > 0)
                {
                    AddDebitAmount(TotalAmtCustomer, "", Naration);
                }

                //if (TotalAmtCustomer > 0)
                //{
                //    AddCustomerDebitAmount(TotalAmtCustomer, AccountCode, Customer, Naration);
                //}

                if (JobsTotal > 0)
                {
                    string code = GetAccountCode("LabourIncome");
                    AddCreditAmount(JobsTotal, code, Naration);
                }

                if (PSTAmount > 0)
                {
                    string code = GetAccountCode("PSTAccount");
                    AddCreditAmount(PSTAmount, code, Naration);
                }

                if (GSTAmount > 0)
                {
                    string code = GetAccountCode("GSTAccount");
                    AddCreditAmount(GSTAmount, code, Naration);
                }

                if (pCkd > 0)
                {
                    string code = GetAccountCode("PartsSaleIncome(CKD)");
                    AddCreditAmount(pCkd, code, Naration);
                }
                if (pLocal > 0)
                {
                    string code = GetAccountCode("PartsSaleIncome(Local)");
                    AddCreditAmount(pLocal, code, Naration);
                }
                if (pMarket > 0)
                {
                    string code = GetAccountCode("PartsSaleIncome(Market)");
                    AddCreditAmount(pMarket, code, Naration);
                }
                if (pImport > 0)
                {
                    string code = GetAccountCode("PartsSaleIncome(Imported)");
                    AddCreditAmount(pImport, code, Naration);
                }

                if (lLocal > 0)
                {
                    string code = GetAccountCode("LubricnatSaleIncome(Local)");
                    AddCreditAmount(lLocal, code, Naration);
                }
                if (lMarket > 0)
                {
                    string code = GetAccountCode("LubricnatSaleIncome(Market)");
                    AddCreditAmount(lMarket, code, Naration);
                }
                if (lImport > 0)
                {
                    string code = GetAccountCode("LubricnatSaleIncome(Import)");
                    AddCreditAmount(lImport, code, Naration);
                }
                if (lCkd > 0)
                {
                    string code = GetAccountCode("LubricnatSaleIncome(CKD)");
                    AddCreditAmount(lCkd, code, Naration);
                }

                if (SubletTotal > 0)
                {
                    string code = GetAccountCode("SubletIncome");
                    AddCreditAmount(SubletTotal, code, Naration);
                }
                //if (discount > 0)
                //{
                //    string code = GetAccountCode("CashDiscountTaken");
                //    AddDebitAmount(discount, code, Naration);
                //}
                RBLTransType.Enabled = false;
                ddlJournalNo.SelectedIndex = 1;
                ddlJournalNo_SelectedIndexChanged(null, null);
            }
        }

        private void LoadPVGrid(string leadId)
        {
            SqlParameter[] dsParamInv = {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                new SqlParameter("@ReceiptNo",SqlDbType.Char,8)
            };

            dsParamInv[0].Value = Session["DealerCode"].ToString();
            dsParamInv[1].Value = leadId;

            totCredit = totDebit = 0;

            DataSet ds0 = new DataSet();
            ds0 = myFunc.FillDataSet("sp_W2_PaymentToVendor_GL", dsParamInv);

            if (ds0.Tables[0].Rows.Count > 0)
            {
                string AccountCode = ds0.Tables[0].Rows[0]["AccountCode"].ToString();
                string Vendor = ds0.Tables[0].Rows[0]["VendorDesc"].ToString().Trim();
                double AmountPaid =  SysFunctions.CustomCDBL(ds0.Tables[0].Rows[0]["InsAmount"].ToString());
                double TotalAmtCustomer = SysFunctions.CustomCDBL(ds0.Tables[0].Rows[0]["InvAdjTotal"].ToString());
                double AdvanceAdjustedAmount = SysFunctions.CustomCDBL(ds0.Tables[0].Rows[0]["AdvanceAdjustedAmount"].ToString());
                double AdvanceAmount = SysFunctions.CustomCDBL(ds0.Tables[0].Rows[0]["AdvanceAmount"].ToString());
                double AdvTaxCount = SysFunctions.CustomCDBL(ds0.Tables[0].Rows[0]["AdvTaxCount"].ToString());
                string IsAdjustAdvance = ds0.Tables[0].Rows[0]["IsAdjustAdvance"].ToString();
                string IsAdvancePayment = ds0.Tables[0].Rows[0]["IsAdvancePayment"].ToString();
                string AdvancePaymentNo = ds0.Tables[0].Rows[0]["AdvancePaymentNo"].ToString();
                string AccountCode1 = "";
                //double cashDiscount = Convert.ToDouble(ds.Tables[0].Rows[0]["CashDiscount"]);
                //double ServiceCharges = Convert.ToDouble(ds.Tables[0].Rows[0]["ServiceCharges"]);
                //double INCOMETAX10 = Convert.ToDouble(ds.Tables[0].Rows[0]["INCOMETAX10%"]);
                //double INCOMETAX4 = Convert.ToDouble(ds.Tables[0].Rows[0]["INCOMETAX4.50%"]);
                //double WHT20 = Convert.ToDouble(ds.Tables[0].Rows[0]["WHT20%"]);
                //double OtherCharges = Convert.ToDouble(ds.Tables[0].Rows[0]["OtherCharges"]);
                string[] itemsToDisable = { "CR", "BR" };
                foreach (string item in itemsToDisable)
                {
                    var listItem = RBLTransType.Items.FindByValue(item);
                    if (listItem != null)
                        listItem.Enabled = false;
                }
                if (ds0.Tables[0].Rows[0]["PayModeCode"].ToString() != "C")
                {
                    txtChqNo.Text = ds0.Tables[0].Rows[0]["InsNo"].ToString();
                    txtChqDate.Text = ds0.Tables[0].Rows[0]["InsDate"].ToString();
                    RBLTransType.SelectedValue = "BP";
                    RBLTransType_SelectedIndexChanged(null, null);
                }
                else
                {
                    txtChqNo.Text = ds0.Tables[0].Rows[0]["InsNo"].ToString();
                    txtChqDate.Text = ds0.Tables[0].Rows[0]["InsDate"].ToString();
                    RBLTransType.SelectedValue = "CP";
                    RBLTransType_SelectedIndexChanged(null, null);
                }
                string Naration = "Payment No : " + ds0.Tables[0].Rows[0]["PaymentNo"].ToString().Trim() + " | " +
                                  "Vendor : " + ds0.Tables[0].Rows[0]["VendorDesc"].ToString().Trim() + " | " +
                                  "Purchase No : " + ds0.Tables[0].Rows[0]["OrderNo"].ToString().Trim();

                ViewState["RecPay"] = ds0.Tables[0].Rows[0]["VendorDesc"].ToString().Trim();

                if (ds0.Tables[0].Rows[0]["VoucherNo"].ToString().Trim() != "")
                {
                    lblText.Text = "Edit Mode";
                    txtVoucherNo.Text = ds0.Tables[0].Rows[0]["VoucherNo"].ToString().Trim();
                    Session["VoucherNo"] = ds0.Tables[0].Rows[0]["VoucherNo"].ToString().Trim();
                    txtVoucherDate.Text = GetVoucherDate(ds0.Tables[0].Rows[0]["VoucherNo"].ToString().Trim());
                    txtVoucherDate.Enabled = false;
                    RBLTransType.Visible = false;
                }

                ds = new DataSet();

                ds.Tables.Add();
               
                ds.Tables[0].Columns.Add(new DataColumn("AccountCode", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("AccountTitle", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("Debit", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("Credit", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("Naration", typeof(string)));

                Session["JV"] = ds;
                if(IsAdvancePayment=="Y" && IsAdjustAdvance=="N")
                {
                    if (AdvanceAmount > 0)
                    {
                        Naration = "Advance Payment No : " + ds0.Tables[0].Rows[0]["PaymentNo"].ToString().Trim() + " | " +
                              //   "Order No : " + ds0.Tables[0].Rows[0]["OrderNo"].ToString().Trim() + " | " +
                                 "Vendor : " + ds0.Tables[0].Rows[0]["VendorDesc"].ToString().Trim();
                        AddCreditAmount(AdvanceAmount, "", Naration);
                        string code = GetAccountCode("AdvancePaymentToVendor");
                        AddDebitAmount(AmountPaid, code, Naration+" | Receivable Account:"+code);

                    }
                }
                else if (IsAdjustAdvance == "Y")
                {
                    if (AmountPaid > 0)
                    {
                        AddCreditAmount(AmountPaid, "", Naration);
                    }
                    if (AdvanceAdjustedAmount > 0)
                    {
                        string code = GetAccountCode("AdvancePaymentToVendor");
                        AddCreditAmount(AdvanceAdjustedAmount, code, Naration);
                    }
                    if (AdvTaxCount > 0)
                    {
                        SqlParameter[] param1 = {
                        new SqlParameter("@DealerCode",SqlDbType.Char,5),
                        new SqlParameter("@PaymentNo",SqlDbType.Char,8)
                };

                        param1[0].Value = Session["DealerCode"].ToString();
                        param1[1].Value = leadId;

                        DataSet dsTaxDetails = myFunc.FillDataSet("sp_OutgoingPaymentTaxDetail_Select", param1);

                        for (int i = 0; i < dsTaxDetails.Tables[0].Rows.Count; i++)
                        {
                            AccountCode1 = dsTaxDetails.Tables[0].Rows[0]["AccountCode"].ToString();
                            string VehExpDesc = dsTaxDetails.Tables[0].Rows[0]["TaxHead"].ToString().Trim();
                            double Amount = Convert.ToDouble(dsTaxDetails.Tables[0].Rows[0]["Amount"]);


                            AddCreditAmount(Amount, AccountCode1, Naration);

                        }

                    }
                    if (TotalAmtCustomer > 0)
                    {
                        AddDebitAmount(TotalAmtCustomer, AccountCode, Naration);
                    }

                }

                else if(IsAdjustAdvance=="N" ||IsAdvancePayment=="N"){

                    if (AmountPaid > 0)
                    {
                        AddCreditAmount(AmountPaid, "", Naration);
                    }
                    if (AdvTaxCount > 0)
                    {
                        SqlParameter[] param = {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                new SqlParameter("@PaymentNo",SqlDbType.Char,8)
                };

                        param[0].Value = Session["DealerCode"].ToString();
                        param[1].Value = leadId;

                        DataSet dsTaxDetail = myFunc.FillDataSet("sp_OutgoingPaymentTaxDetail_Select", param);

                        for (int i = 0; i < dsTaxDetail.Tables[0].Rows.Count; i++)
                        {
                            AccountCode1 = dsTaxDetail.Tables[0].Rows[0]["AccountCode"].ToString();
                            string VehExpDesc = dsTaxDetail.Tables[0].Rows[0]["TaxHead"].ToString().Trim();
                            double Amount = Convert.ToDouble(dsTaxDetail.Tables[0].Rows[0]["Amount"]);


                            AddCreditAmount(Amount, AccountCode1, Naration);

                        }
                    }
                    if (TotalAmtCustomer > 0) 
                    {
                        AddCustomerDebitAmount(TotalAmtCustomer, AccountCode, Vendor, Naration);
                    }
                  

                }

             
              
            }
        }

        private void initializeDDLs(DropDownList ddl)
        {
            try
            {

                string sql = "Select A.contacccode +'-'+  A.SubCode +'-'+  A.subsubcode +'-'+  A.loccode +'-'+  A.DetailCode as AccountCode , A.contacccode +'-'+  A.SubCode +'-'+  A.subsubcode +'-'+  A.loccode +'-'+  A.DetailCode + ' | ' + rtrim(B.DetailDesc) + ' | ' + A.Booktype + ' | ' + A.BookNo DetailDesc  from GbookSetup  A" +
                    " inner join GDetail B " +
                    " on B.contacccode + '-' + B.SubCode + '-' + B.subsubcode + '-' + B.loccode + '-' + B.DetailCode = " +
                    " A.contacccode + '-' + A.SubCode + '-' + A.subsubcode + '-' + A.loccode + '-' + A.DetailCode " +
                    " and A.Compcode = B.CompCode ";

                ddl.Items.Clear();

                if(RBLTransType.SelectedValue == "BR" || RBLTransType.SelectedValue == "BP")
                {
                    sql = sql + " and A.BookType = 'B'";
                }else
                {
                    sql = sql + " and A.BookType = 'C'";
                }

                sql = sql + "where A.CompCode = '" + Session["DealerCode"].ToString() + "'";

                SqlDataReader dr = SqlHelper.ExecuteReader(CCon, CommandType.Text, sql);

                if (dr.HasRows)
                {
                    ListItem item = new ListItem();
                    item.Text = "Select";
                    item.Value = "0";

                    //AddInAllDDL(item);

                    ddl.Items.Add(item);
                    while (dr.Read())
                    {
                        StringWriter myWriter = new StringWriter();
                        HttpUtility.HtmlDecode(dr["DetailDesc"].ToString().Replace(" ", "&nbsp;"), myWriter);//ddlEmp.Items.Add(myWriter.ToString());
                        item = new ListItem();
                        item.Text = myWriter.ToString();
                        item.Value = dr["AccountCode"].ToString();
                        ddl.Items.Add(item);

                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                sysfun.UserMsg(lblMsg, Color.Red, ex.Message);
            }

        }

        private string GetAccountCode(string code)
        {
            DataTable dt = new DataTable();

            string value = "";

            dt = myFunc.GetData("Select [" + code + "] from AccountCodeSetup where DealerCode = '" + Session["DealerCode"].ToString() + "'");

            if(dt == null)
            {
                return value;
            }
            if (dt.Rows.Count > 0)
            {
                value = dt.Rows[0][code].ToString();
            }
            return value;
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
                sysfun.UserMsg(lblMsg, Color.Red, ex.Message);
                return "";
            }

        }

        private string GetVoucherDate(string code)
        {
            try
            {
                string CCon = CConnection.GetConnStringForAccount();
                string value = "";

                SqlDataAdapter dta = new SqlDataAdapter("Select VouchDate  from GVouMaster  A where A.CompCode = '" + Session["DealerCode"].ToString() + "' and VouchNo='"+code+"' ", CCon);

                DataTable dt = new DataTable();
                dta.Fill(dt);

                if (dt.Rows.Count > 0 && dt != null)
                {
                    value = Convert.ToDateTime(dt.Rows[0]["VouchDate"].ToString()).ToString("dd/MM/yyyy");
                }


                return value;
            }
            catch (Exception ex)
            {
                sysfun.UserMsg(lblMsg, Color.Red, ex.Message);
                return "";
            }

        }

        private bool IsExistVoucherNo(string code)
        {
            try
            {
                string CCon = CConnection.GetConnStringForAccount();
                
                bool Isexist=false;

                SqlDataAdapter dta = new SqlDataAdapter("Select *  from GVouMaster  A where A.CompCode = '" + Session["DealerCode"].ToString() + "' and VouchNo='" + code + "' ", CCon);

                DataTable dt = new DataTable();
                dta.Fill(dt);

                if (dt.Rows.Count > 0 && dt != null)
                {
                    Isexist = true;
                }


                return Isexist;
            }
            catch (Exception ex)
            {
                sysfun.UserMsg(lblMsg, Color.Red, ex.Message);
                return false;
            }

        }

        private string GetBookType(string code)
        {
            try
            {
                string CCon = CConnection.GetConnStringForAccount();
                string value = "";

                SqlDataAdapter dta = new SqlDataAdapter("Select A.Booktype from GbookSetup  A where A.CompCode = '"+Session["DealerCode"].ToString()+"' and A.contacccode +'-'+  A.SubCode +'-'+  A.subsubcode +'-'+  A.loccode +'-'+  A.DetailCode = '" + code + "'", CCon);

                DataTable dt = new DataTable();
                dta.Fill(dt);
                if(dt == null)
                {
                    return value;
                }
                else if (dt.Rows.Count > 0)
                {
                    value = dt.Rows[0]["Booktype"].ToString();
                }

                return value;
            }
            catch (Exception ex)
            {
                sysfun.UserMsg(lblMsg, Color.Red, ex.Message);
                return "";
            }

        }

        private string GetBookNo(string code)
        {
            try
            {
                string CCon = CConnection.GetConnStringForAccount();
                string value = "";

                SqlDataAdapter dta = new SqlDataAdapter("Select A.BookNo from GbookSetup  A  where A.CompCode = '"+Session["DealerCode"].ToString()+"' and A.contacccode +'-'+  A.SubCode +'-'+  A.subsubcode +'-'+  A.loccode +'-'+  A.DetailCode = '" + code + "'", CCon);

                DataTable dt = new DataTable();
                dta.Fill(dt);
                if (dt == null)
                {
                    return value;
                }
                else if (dt.Rows.Count > 0)
                {
                    value = dt.Rows[0]["BookNo"].ToString();
                }

                return value;
            }
            catch (Exception ex)
            {
                sysfun.UserMsg(lblMsg, Color.Red, ex.Message);
                return "";
            }

        }

        protected void btnYes_Click(object sender, EventArgs e)
        {

            ds = (DataSet)Session["JV"];
            DataRow[] drr = ds.Tables[0].Select();
            for (int i = 0; i < drr.Length; i++)
            {
                if (ds.Tables[0].Rows[i]["AccountCode"].ToString().Trim() == "" || ds.Tables[0].Rows[i]["AccountCode"].ToString().Trim() == "0")
                {
                    sysfun.UserMsg(lblMsg, Color.Red, "Please Select Account Code First");
                
                    return;
                }
            }

            if (RBLTransType.SelectedValue == "")
            {
                sysfun.UserMsg(lblMsg, Color.Red, "Please Select Book Type First");
                return;
            }
            if (ddlJournalNo.SelectedIndex == 0)
            {
                sysfun.UserMsg(lblMsg, Color.Red, "Please Select Journal No First");
                return;
            }
             if(CustomCDBL(txtTotalCredit.Text)!=CustomCDBL(txtTotalDebit.Text))
            {
                sysfun.UserMsg(lblMsg, Color.Red, "Debit and Credit Amount is not equal ");
                return;
            }
           


               

          
            try
            {
                if (IsExistVoucherNo(txtVoucherNo.Text))
                {
                    sysfun.UserMsg(lblMsg, Color.Red, "Can't  Post the Voucher it Already Exist");
                    return;
                }
                if (lblText.Text.Trim() == "Add Mode")
                {
                    strAutoCode = GetNewVoucherNo("GVouMaster", "VouchNo", 3, "");
                }
                else
                {
                    strAutoCode = txtVoucherNo.Text;
                }
                SqlParameter[] param = {

                                   new SqlParameter("@CompCode",SqlDbType.Char,5),         //0
                                   new SqlParameter("@Booktype",SqlDbType.VarChar,2),      //1
                                   new SqlParameter("@Journalno",SqlDbType.VarChar,4),     //2
                                   new SqlParameter("@VouchNo",SqlDbType.VarChar,50),      //3
                                   new SqlParameter("@SeqNo",SqlDbType.Int),               //4
                                   new SqlParameter("@ContAccCode",SqlDbType.Char,2),      //5
                                   new SqlParameter("@SubCode",SqlDbType.Char,2),          //6
                                   new SqlParameter("@SubSubCode",SqlDbType.Char,2),       //7
                                   new SqlParameter("@LocCode",SqlDbType.Char,2),          //8
                                   new SqlParameter("@DetailCode",SqlDbType.Char,4),       //9
                                   new SqlParameter("@VouchDate",SqlDbType.DateTime),      //10
                                   new SqlParameter("@RecPay",SqlDbType.VarChar,50),       //11
                                   new SqlParameter("@Narration01",SqlDbType.VarChar,200), //12
                                   new SqlParameter("@Narration02",SqlDbType.VarChar,200), //13
                                   new SqlParameter("@ChqBillNo",SqlDbType.VarChar,50),    //14
                                   new SqlParameter("@ChqBillDate",SqlDbType.DateTime),    //15
                                   new SqlParameter("@DebitAmt",SqlDbType.Float),          //16
                                   new SqlParameter("@CreditAmt",SqlDbType.Float),         //17
                                   new SqlParameter("@FYear",SqlDbType.DateTime),          //18
                                   new SqlParameter("@TYear",SqlDbType.DateTime),          //19
                                   new SqlParameter("@DelFlag",SqlDbType.Char,1),          //20
                                   new SqlParameter("@BookNo",SqlDbType.Char,2),           //21
                                   new SqlParameter("@AutoAcc",SqlDbType.VarChar,500),     //22
                                   new SqlParameter("@Post",SqlDbType.Char,1),             //23
                                   new SqlParameter("@Source",SqlDbType.VarChar,50),       //24
                                   new SqlParameter("@AddUser",SqlDbType.Char,50),         //25
                                   new SqlParameter("@AddDate",SqlDbType.DateTime),        //26
                                   new SqlParameter("@AddTime",SqlDbType.DateTime),        //27
                                   new SqlParameter("@AddTerm",SqlDbType.VarChar,50),      //28
                                   new SqlParameter("@CSCode",SqlDbType.Char,8),           //29
                                };

                for (int i = 0; i < drr.Length; i++)
                {

                    string accountcode = ds.Tables[0].Rows[i]["AccountCode"].ToString();

                    string contAccCode = accountcode.Substring(0, 2);
                    string subCode = accountcode.Substring(3, 2);
                    string subSubCode = accountcode.Substring(6, 2);
                    string locCode = accountcode.Substring(9, 2);
                    string detailCode = accountcode.Substring(12, 4);

                    string CCon = CConnection.GetConnStringForAccount();
                    string value = "";

                    SqlDataAdapter dta = new SqlDataAdapter("Select Post from GVouMaster where  CompCode='" + Session["DealerCode"].ToString() + "' and VouchNo='" + txtVoucherNo.Text + "' and SeqNo='" + i + 1 + "'  ", CCon);

                    DataTable dt = new DataTable();
                    dta.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                      string  Postflag = dt.Rows[0]["Post"].ToString();
                        // Delflag = dt.Rows[0]["DelFalg"].ToString();
                        if (Postflag == "Y")
                        {
                            sysfun.UserMsg(lblMsg, Color.Red, "Voucher Can't Be Edit or Delete. . .! It is Already Posted. ");

                            return;
                        }



                    }







                    param[0].Value = Session["DealerCode"].ToString();
                    param[1].Value = RBLTransType.SelectedValue;
                    param[2].Value = "";
                    param[3].Value = strAutoCode;
                    param[4].Value = i + 1;
                    param[5].Value = contAccCode;
                    param[6].Value = subCode;
                    param[7].Value = subSubCode;
                    param[8].Value = locCode;
                    param[9].Value = detailCode;
                    param[10].Value = sysfun.SaveDate(txtVoucherDate.Text);
                    param[11].Value = ViewState["RecPay"].ToString();
                    param[12].Value = ds.Tables[0].Rows[i]["Naration"].ToString();
                    param[13].Value = "";
                    param[14].Value = txtChqNo.Text;
                    param[15].Value = sysfun.SaveDate(txtChqDate.Text);

                    if (ds.Tables[0].Rows[i]["Debit"].ToString() != "0.0")
                    {
                        param[16].Value = Convert.ToDouble(ds.Tables[0].Rows[i]["Debit"].ToString());
                    }
                    else
                    {
                        param[16].Value = 0;
                    }

                    if (ds.Tables[0].Rows[i]["Credit"].ToString() != "0.0")
                    {
                        param[17].Value = Convert.ToDouble(ds.Tables[0].Rows[i]["Credit"].ToString());
                    }
                    else
                    {
                        param[17].Value = 0;
                    }
              
                  SqlDataAdapter dta1 = new SqlDataAdapter("Select FYear,TYear from FiscalYear where FYear<='" + sysfun.SaveDate(txtVoucherDate.Text) + "' and TYear>='" + sysfun.SaveDate(txtVoucherDate.Text) + "'  and FinalClose='N' ", CCon);

                    DataTable dt1 = new DataTable();
                    dta1.Fill(dt1);
                 
                    if (dt.Rows.Count > 0 ||dt !=null)
                    {
                        param[18].Value = dt1.Rows[0]["FYear"].ToString();
                        param[19].Value = dt1.Rows[0]["TYear"].ToString();

                    }
                    else
                    {
                        lblMsg.Text = "Voucher Can't be Save! Please Check Fiscal Year Date ";
                        return ;

                    }
                   
                    param[20].Value = "N";
                    param[21].Value = GetBookNo(accountcode);
                    param[22].Value = accountcode;
                    param[23].Value = "N";
                    param[24].Value = type;
                    param[25].Value = Session["UserName"].ToString();
                    param[26].Value = sysfun.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"));
                    param[27].Value = DateTime.Now;
                    param[28].Value = GlobalVar.mUserIPAddress;
                    param[29].Value = "00/00000";


                    SqlHelper.ExecuteNonQuery(CCon, CommandType.StoredProcedure, "sp_GVouMaster_Insert", param);
                }

                string IQuery;

                if(type == "PR")
                {
                    IQuery = "Update PaymentReceiptMaster set VoucherNo ='" + strAutoCode + "' , VoucherFlag = 'Y' " +
                                             "Where DealerCode='" + Session["DealerCode"].ToString() + "' and ReceiptNo ='" + ViewState["ReceiptNo"].ToString() + "'";
                }
                else if (type == "PRI")
                {
                    IQuery = "Update CustomerInvoice set VoucherNo ='" + strAutoCode + "' , VoucherFlag = 'Y' " +
                                             "Where DealerCode='" + Session["DealerCode"].ToString() + "' and InvoiceNo ='" + ViewState["ReceiptNo"].ToString() + "'";
                }
                else if (type == "DS")
                {
                    IQuery = "Update CounterSaleMaster set VoucherNo ='" + strAutoCode + "' , VoucherFlag = 'Y' " +
                                             "Where DealerCode='" + Session["DealerCode"].ToString() + "' and SaleInvNo ='" + ViewState["ReceiptNo"].ToString() + "'";
                }
                else if (type == "RP")
                {
                    IQuery = "Update RefundPayment set VoucherNo ='" + strAutoCode + "' , VoucherFlag = 'Y' " +
                                             "Where DealerCode='" + Session["DealerCode"].ToString() + "' and RefundCode ='" + ViewState["ReceiptNo"].ToString() + "'";
                }
                else
                {
                    IQuery = "Update OutgoingPaymentMaster set VoucherNo ='" + strAutoCode + "' , VoucherFlag = 'Y' " +
                                             "Where DealerCode='" + Session["DealerCode"].ToString() + "' and PaymentNo ='" + ViewState["ReceiptNo"].ToString() + "'";
                }


                if (sysfun.ExecuteQuery_NonQuery(IQuery))
                {
                    strAutoCode = "";
                    sysfun.UserMsg(lblMsg, Color.Green, "Voucher Generated");
                }

            }
            catch (Exception ex) { throw ex; }
        }

        public string GetNewVoucherNo(string sTableName, string sColumn, int NoOfChar,string VDate)
        {

            string CCon = CConnection.GetConnStringForAccount();
            string dateTime = "";
            if (VDate == "")
            {
                dateTime = SaveDateVouch(txtVoucherDate.Text).ToString();
            }
            else
            {
                dateTime = SaveDateVouch(VDate).ToString();
            }
            string Vtype=RBLTransType.SelectedValue;
            //string dateTime = "17-07-01";
            string vouch =  Vtype+ "-";
            string vouch_No = vouch + dateTime;

            //#GET MAX ID FROM TABLE
            string sQuery = "SELECT MAX(" + sColumn + ") MAXID FROM " + sTableName+ " where CompCode = '"+Session["DealerCode"].ToString() + "' and left(VouchNo,11)='"+vouch_No+"'";

            SqlDataReader drItemVal = SqlHelper.ExecuteReader(CCon, CommandType.Text, sQuery);

            //SqlDataReader drItemVal = null;
            string sNewVersion = "0";
            int iNewVersion;
            string date = DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("yyyy-MM-dd");
            string subDate = date.Substring(2);
            try
            {
                if (drItemVal.HasRows)
                {
                    drItemVal.Read();
                    sNewVersion = myFunc.GetNullString(drItemVal["MAXID"]);
                    drItemVal.Close();

                    if (sNewVersion == "")
                    {
                        sNewVersion = "0";
                        iNewVersion = Convert.ToInt32(sNewVersion) + 1;
                        sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
                    }
                    else
                    {
                        string sub = sNewVersion.Substring(12);
                        iNewVersion = Convert.ToInt32(sub) + 1;
                        sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
                    }
                    return RBLTransType.SelectedValue + "-" + dateTime + "-" + sNewVersion;

                }
                else
                {
                    iNewVersion = 0;
                    sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
                    return RBLTransType.SelectedValue + "-" + dateTime + "-" + sNewVersion;
                }

            }
            catch (Exception ex)
            {
                //ShowError();
                drItemVal.Close();
            }

            return sNewVersion;
        }
        public object SaveDateVouch(string txt)
        {
            object functionReturnValue = null;
            try
            {
                if (txt == "__/__/____" || string.IsNullOrEmpty(txt))
                {
                    functionReturnValue = DBNull.Value;
                }
                else
                {
                    DateTime DateTimeFor = Convert.ToDateTime(txt.Substring(6, 4) + "-" + txt.Substring(3, 2) + "-" + txt.Substring(0, 2)); //Convert.ToDateTime(txt, new System.Globalization.CultureInfo("ur-PK"));
                    functionReturnValue = DateTimeFor.ToString("yy-MM-dd");
                }
            }
            catch (Exception ex)
            {
                functionReturnValue = DBNull.Value; throw ex;
            }
            return functionReturnValue;
        }
        private void LoadPRGrid(string CusInv)
        {
            SqlParameter[] dsParamInv = {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                new SqlParameter("@ReceiptNo",SqlDbType.Char,8)
            };

            dsParamInv[0].Value = Session["DealerCode"].ToString();
            dsParamInv[1].Value = CusInv;

            totCredit = totDebit = 0;
            string[] itemsToDisable = { "CP", "BP" };
            foreach (string item in itemsToDisable)
            {
                var listItem = RBLTransType.Items.FindByValue(item);
                if (listItem != null)
                    listItem.Enabled = false;
            }
            DataSet ds0 = new DataSet();
            ds0 = myFunc.FillDataSet("sp_W2_PaymentReceipt_GL", dsParamInv);

            if (ds0.Tables[0].Rows.Count > 0)
            {
               
                string AccountCode = ds0.Tables[0].Rows[0]["AccountCode"].ToString();
                string IsAdvAdj = ds0.Tables[0].Rows[0]["IsAdjustAdvance"].ToString();
                double AmountPaid = Convert.ToDouble(ds0.Tables[0].Rows[0]["AmountPaid"]);              
                double TotalAmtCustomer = Convert.ToDouble(ds0.Tables[0].Rows[0]["NetAmountCustomer"]);
                double AdvanceAmount = Convert.ToDouble(ds0.Tables[0].Rows[0]["AdvanceAmount"]);
                double AdvanceAdjustedAmount= Convert.ToDouble(ds0.Tables[0].Rows[0]["AdvanceAdjustedAmount"]);
                double cashDiscount =   Math.Round(Convert.ToDouble(ds0.Tables[0].Rows[0]["CashDiscount"]),2);
                double ServiceCharges = Math.Round(Convert.ToDouble(ds0.Tables[0].Rows[0]["ServiceCharges"]), 2);
                double INCOMETAX10 =    Math.Round(Convert.ToDouble(ds0.Tables[0].Rows[0]["INCOMETAX10%"]), 2);
                double INCOMETAX4 =     Math.Round(Convert.ToDouble(ds0.Tables[0].Rows[0]["INCOMETAX4.50%"]), 2);
                double WHT20 =          Math.Round(Convert.ToDouble(ds0.Tables[0].Rows[0]["WHT20%"]), 2);
                double OtherCharges =   Math.Round(Convert.ToDouble(ds0.Tables[0].Rows[0]["OtherCharges"]), 2);
                double WHT3 = Math.Round(Convert.ToDouble(ds0.Tables[0].Rows[0]["WHT3.5%"]), 2);
                if (ds0.Tables[0].Rows[0]["PayModeCode"].ToString() != "C")
                {
                    txtChqNo.Text = ds0.Tables[0].Rows[0]["InsNo"].ToString();
                    txtChqDate.Text = ds0.Tables[0].Rows[0]["InsDate"].ToString();
                    RBLTransType.SelectedValue = "BR";
                    
                    RBLTransType_SelectedIndexChanged(null, null);
                }
               else { 
                   // txtChqNo.Text = ds.Tables[0].Rows[0]["InsNo"].ToString();
                    //txtChqDate.Text = ds.Tables[0].Rows[0]["InsDate"].ToString();
                    RBLTransType.SelectedValue = "CR";
                    RBLTransType_SelectedIndexChanged(null, null);
                }

                string Naration = "Receipt : " + ds0.Tables[0].Rows[0]["ReceiptNo"].ToString().Trim() + " | " +
                                  "Customer : " + ds0.Tables[0].Rows[0]["CusDesc"].ToString().Trim() + " | " +
                                  "Trans Type : " + ds0.Tables[0].Rows[0]["TransType"].ToString().Trim() + " | " +
                                   "Inv Type : " + ds0.Tables[0].Rows[0]["InvoiceType"].ToString().Trim() + " | " +
                                  "Invoice No : " + ds0.Tables[0].Rows[0]["InvoicesNo"].ToString().Trim();

                ViewState["RecPay"] = ds0.Tables[0].Rows[0]["CusDesc"].ToString().Trim();

                if (ds0.Tables[0].Rows[0]["VoucherNo"].ToString().Trim() != "")
                {
                    lblText.Text = "Edit Mode";
                    txtVoucherNo.Text = ds0.Tables[0].Rows[0]["VoucherNo"].ToString().Trim();
                    txtVoucherDate.Text = GetVoucherDate(ds0.Tables[0].Rows[0]["VoucherNo"].ToString().Trim());
                    Session["VoucherNo"] = ds0.Tables[0].Rows[0]["VoucherNo"].ToString().Trim();
                    txtVoucherDate.Enabled = false;
                    RBLTransType.Visible = false;
                }

                ds = new DataSet();

                ds.Tables.Add();

                ds.Tables[0].Columns.Add(new DataColumn("AccountCode", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("AccountTitle", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("Debit", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("Credit", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("Naration", typeof(string)));

                Session["JV"] = ds;

                

                
               
                if (ds0.Tables[0].Rows[0]["TransType"].ToString()== "Advance")
                {
                    if (AdvanceAmount > 0)
                    {
                        string code;
                        if (ds0.Tables[0].Rows[0]["InvoiceType"].ToString() == "Insurance")
                        {
                            code = GetAccountCode("AdvanceReceiptToInsurance");
                        }
                        else
                        {
                            code = GetAccountCode("AdvanceReceiptToCustomer");
                        }
                        AddDebitAmount(AdvanceAmount, "", "Advance "+Naration);
                        AddCreditAmount(AdvanceAmount, code, "Advance " + Naration);
                    }
                }
              else if (ds0.Tables[0].Rows[0]["IsAdjustAdvance"].ToString() == "Y")
                {
                    if (AmountPaid > 0)
                    {
                        AddDebitAmount(AmountPaid, "", Naration);
                    }
                    if (AdvanceAdjustedAmount > 0)
                    {
                        string code;
                        if (ds0.Tables[0].Rows[0]["InvoiceType"].ToString() == "Insurance")
                        {
                            code = GetAccountCode("AdvanceReceiptToInsurance");
                        }
                        else
                        {
                            code = GetAccountCode("AdvanceReceiptToCustomer");
                        }
                        AddDebitAmount(AdvanceAdjustedAmount, code, "Advance " + Naration);

                    }

                }
                else
                    {
                    if (AmountPaid > 0)
                    {
                        AddDebitAmount(AmountPaid, "", Naration);
                    }
                        
                    }
                if (ds0.Tables[0].Rows.Count > 0)
                {
                   
                    if (CustomCDBL(ds0.Tables[0].Rows[0]["AdvReceiptCount"].ToString()) > 0)
                    {
                        for (int i = 0; i < ds0.Tables[0].Rows.Count; i++)
                        {
                            AddDebitAmount(CustomCDBL(ds0.Tables[0].Rows[i]["Amount"].ToString()), ds0.Tables[0].Rows[i]["ReceiptAcc"].ToString(), Naration);

                        }
                        //if (AdvanceAdjustedAmount > 0)
                        //{
                        //    string code = GetAccountCode("AdvanceReceiptToCustomer");
                        //    AddDebitAmount(AdvanceAdjustedAmount, code, "Advance " + Naration);

                        //}
                        if (TotalAmtCustomer > 0)
                        {
                            string code = GetAccountCode(AccountCode);
                            AddCreditAmount(TotalAmtCustomer, AccountCode, Naration);
                        }

                    }
                    else
                    {
                       

                        if (TotalAmtCustomer > 0)
                        {
                            string code = GetAccountCode(AccountCode);
                            AddCreditAmount(TotalAmtCustomer, AccountCode, Naration);
                        }
                    }

                }

                #region
                //if (cashDiscount > 0)
                //{
                //    AddDebitAmount(cashDiscount, "DiscountAccount", Naration);
                //}

                //if (ServiceCharges > 0)
                //{
                //    AddDebitAmount(ServiceCharges, "OtherTax", Naration);
                //}

                //if (INCOMETAX10 > 0)
                //{
                //    AddDebitAmount(INCOMETAX10, "", Naration);
                //}
                //if (INCOMETAX4 > 0)
                //{
                //    AddDebitAmount(INCOMETAX4, "", Naration);
                //}
                //if (OtherCharges > 0)
                //{
                //    AddDebitAmount(OtherCharges, "OtherChargesAccount", Naration);
                //}

                //if (WHT20 > 0)
                //{
                //    AddDebitAmount(WHT20, "", Naration);
                //}
                //if (WHT3 > 0)
                //{
                //    AddDebitAmount(WHT3, "", Naration);
                //}
                #endregion
               


                
            }
        }
        private void LoadRPGrid(string CusInv)
        {
            SqlParameter[] dsParamInv = {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                new SqlParameter("@RefundNo",SqlDbType.Char,8)
            };

            dsParamInv[0].Value = Session["DealerCode"].ToString();
            dsParamInv[1].Value = CusInv;

            totCredit = totDebit = 0;

            DataSet ds0 = new DataSet();
            ds0 = myFunc.FillDataSet("sp_W2_PaymentRefund_GL", dsParamInv);

            if (ds0.Tables[0].Rows.Count > 0)
            {

                string AccountCode = ds0.Tables[0].Rows[0]["AccountCode"].ToString();
                double AmountPaid = Convert.ToDouble(ds0.Tables[0].Rows[0]["InsAmount"]);
               
                if (ds0.Tables[0].Rows[0]["PayModeCode"].ToString() != "C")
                {
                    txtChqNo.Text = ds0.Tables[0].Rows[0]["InsNo"].ToString();
                    txtChqDate.Text = ds0.Tables[0].Rows[0]["InsDate"].ToString();
                    RBLTransType.SelectedValue = "BP";
                    RBLTransType_SelectedIndexChanged(null, null);
                }
               else { 
                    // txtChqNo.Text = ds.Tables[0].Rows[0]["InsNo"].ToString();
                    //txtChqDate.Text = ds.Tables[0].Rows[0]["InsDate"].ToString();
                    RBLTransType.SelectedValue = "CP";
                    RBLTransType_SelectedIndexChanged(null, null);
                }

                string Naration = "Refund No : " + ds0.Tables[0].Rows[0]["RefundCode"].ToString() + " | " +
                    "Customer : " + ds0.Tables[0].Rows[0]["CusDesc"].ToString() + " | " +
                    "Advance Receipt : " + ds0.Tables[0].Rows[0]["AdvanceCode"].ToString();
                                  

                ViewState["RecPay"] = ds0.Tables[0].Rows[0]["CusDesc"].ToString().Trim();

                if (ds0.Tables[0].Rows[0]["VoucherNo"].ToString().Trim() != "")
                {
                    lblText.Text = "Edit Mode";
                    txtVoucherNo.Text = ds0.Tables[0].Rows[0]["VoucherNo"].ToString().Trim();
                    txtVoucherDate.Text = GetVoucherDate(ds0.Tables[0].Rows[0]["VoucherNo"].ToString().Trim());
                    Session["VoucherNo"] = ds0.Tables[0].Rows[0]["VoucherNo"].ToString().Trim();
                    txtVoucherDate.Enabled = false;
                    RBLTransType.Visible = false;
                }

                ds = new DataSet();

                ds.Tables.Add();

                ds.Tables[0].Columns.Add(new DataColumn("AccountCode", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("AccountTitle", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("Debit", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("Credit", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("Naration", typeof(string)));

                Session["JV"] = ds;

                if (AmountPaid > 0)
                {
                    AddCreditAmount(AmountPaid, "", Naration);
                }
                if (AmountPaid > 0)
                {
                    string code = GetAccountCode("AdvanceReceiptToCustomer");
                    AddDebitAmount(AmountPaid, code, "Advance " + Naration);
                }
              
               
               
               
            }
        }

        private void LoadIAGrid(string CusInv)
        {
            SqlParameter[] dsParamInv = {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                new SqlParameter("@RefundNo",SqlDbType.Char,8)
            };

            dsParamInv[0].Value = Session["DealerCode"].ToString();
            dsParamInv[1].Value = CusInv;

            totCredit = totDebit = 0;

            DataSet ds0 = new DataSet();
            ds0 = myFunc.FillDataSet("sp_W2_InsuranceAdvance_GL", dsParamInv);

            if (ds0.Tables[0].Rows.Count > 0)
            {

                string AccountCode = ds0.Tables[0].Rows[0]["AccountCode"].ToString();
                double AmountPaid = Convert.ToDouble(ds0.Tables[0].Rows[0]["InsAmount"]);

                if (ds0.Tables[0].Rows[0]["PayModeCode"].ToString() != "C")
                {
                    txtChqNo.Text = ds0.Tables[0].Rows[0]["InsNo"].ToString();
                    txtChqDate.Text = ds0.Tables[0].Rows[0]["InsDate"].ToString();
                    RBLTransType.SelectedValue = "BR";
                    RBLTransType_SelectedIndexChanged(null, null);
                }
               else { 
                    // txtChqNo.Text = ds.Tables[0].Rows[0]["InsNo"].ToString();
                    //txtChqDate.Text = ds.Tables[0].Rows[0]["InsDate"].ToString();
                    RBLTransType.SelectedValue = "CR";
                    RBLTransType_SelectedIndexChanged(null, null);
                }

                string Naration = "Advance No : " + ds0.Tables[0].Rows[0]["AdvanceID"].ToString() + " | " +
                    "Insurance Company : " + ds0.Tables[0].Rows[0]["CusDesc"].ToString() + " | " 
                   ;


                ViewState["RecPay"] = ds0.Tables[0].Rows[0]["CusDesc"].ToString().Trim();

                if (ds0.Tables[0].Rows[0]["VoucherNo"].ToString().Trim() != "")
                {
                    lblText.Text = "Edit Mode";
                    txtVoucherNo.Text = ds0.Tables[0].Rows[0]["VoucherNo"].ToString().Trim();
                    txtVoucherDate.Text = GetVoucherDate(ds0.Tables[0].Rows[0]["VoucherNo"].ToString().Trim());
                    Session["VoucherNo"] = ds0.Tables[0].Rows[0]["VoucherNo"].ToString().Trim();
                    txtVoucherDate.Enabled = false;
                    RBLTransType.Visible = false;
                }

                ds = new DataSet();

                ds.Tables.Add();

                ds.Tables[0].Columns.Add(new DataColumn("AccountCode", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("AccountTitle", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("Debit", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("Credit", typeof(string)));
                ds.Tables[0].Columns.Add(new DataColumn("Naration", typeof(string)));

                Session["JV"] = ds;
                if (AmountPaid > 0)
                {
                    AddDebitAmount(AmountPaid, "", Naration);
                }
                if (AmountPaid > 0)
                {
                    string code = GetAccountCode("AdvanceReceiptToInsurance");
                    AddCreditAmount(AmountPaid, code, "Advance " + Naration);
                }

               





            }
        }

        private void AddCreditAmount(double amount, string Account, string Naration)
        {
            ds = (DataSet)Session["JV"];
            
            DataRow dr = ds.Tables[0].NewRow();
            dr["AccountCode"] = Account;
            dr["AccountTitle"] = GetAccounttitle(Account);
            dr["Debit"] = "0.0";
            dr["Credit"] = amount;
            dr["Naration"] = Naration;

            totCredit = totCredit + amount;

            ds.Tables[0].Rows.Add(dr);

            gvJV.DataSource = ds.Tables[0];
            gvJV.DataBind();

            Session["JV"] = ds;
        }

        protected void ddlJournalNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ds = (DataSet)Session["JV"];
            
            
            string Code = ddlJournalNo.SelectedValue;
            ds.Tables[0].Rows[0]["AccountCode"] = Code;
            ds.Tables[0].Rows[0]["AccountTitle"] = GetAccounttitle(Code);

            gvJV.DataSource = ds.Tables[0];
            gvJV.DataBind();

            Session["JV"] = ds;
        }

        protected void RBLTransType_SelectedIndexChanged(object sender, EventArgs e)
        {
            initializeDDLs(ddlJournalNo);
            txtVoucherNo.Text = GetNewVoucherNo("GVouMaster", "VouchNo", 3,"");
        }

        private void AddCustomerDebitAmount(double amount, string code, string Customer, string Naration)
        {
            ds = (DataSet)Session["JV"];

            DataRow dr = ds.Tables[0].NewRow();
            dr["AccountCode"] = code;
            dr["AccountTitle"] = Customer;
            dr["Debit"] = amount;
            dr["Credit"] = "0.0";
            dr["Naration"] = Naration;

            totDebit = totDebit + amount;

            ds.Tables[0].Rows.Add(dr);

            gvJV.DataSource = ds.Tables[0];
            gvJV.DataBind();

            Session["JV"] = ds;
        }
        private void AddDebitAmount(double amount, string account, string Naration)
        {
            ds = (DataSet)Session["JV"];

            DataRow dr = ds.Tables[0].NewRow();
            string code = GetAccountCode(account);
            if (code == "" || code == null)
            {
                code = account;
            }
            dr["AccountCode"] = code;
            dr["AccountTitle"] = GetAccounttitle(code);
            dr["Debit"] = amount;
            dr["Credit"] = "0.0";
            dr["Naration"] = Naration+"||"+account;
            totDebit = totDebit + amount;

            ds.Tables[0].Rows.Add(dr);

            gvJV.DataSource = ds.Tables[0];
            gvJV.DataBind();

            Session["JV"] = ds;
        }

        protected void txtVoucherDate_TextChanged(object sender, EventArgs e)
        {
            txtVoucherNo.Text= GetNewVoucherNo("GVouMaster", "VouchNo", 3, txtVoucherDate.Text);
        }

        public static double CustomCDBL(object txt)
        {
            if (object.ReferenceEquals(txt, DBNull.Value)) return 0;
            else if (txt == null) return 0;
            else if (string.IsNullOrEmpty(txt.ToString().Trim())) return 0;
            else return Math.Round(Convert.ToDouble(txt), 2);
        }

    }
}