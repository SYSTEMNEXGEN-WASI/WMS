using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Web.ASPxGridView;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;



namespace DXBMS.Modules.Transactions
{
    public partial class CustomerPaymentReceipt : System.Web.UI.Page
    {
        SysFunctions myFunc = new SysFunctions();
        DataTable TaxDetailDT, dtPendingInvoice;
        DataSet ds, dsReceiptMaster, dsReceiptTaxDetail;
        bool search_result;
        int countInvoices;
        double totInvoice, totOutstanding, totTaxAmt, totAdjAmt, TotRefAmt;
        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/Main/login.aspx");
            }
            if (!IsPostBack)
            {
                dpInstDate.Text = dpReceiptDate.Text = System.DateTime.Now.Date.ToString("dd-MM-yyyy"); 
                createTaxDetail();
                cmbBank = myFunc.FillDrp_SP(cmbBank,"sp_2W_Select_Bank", "BankCode", "BankDesc", null,true, "--Select--", false, "");

                DataTable TaxDetailDT = (DataTable)ViewState["TaxDetail"];
                if (TaxDetailDT.Rows.Count == 0) TaxDetailDT.Rows.Add(TaxDetailDT.NewRow());
                gvTaxDetail.DataSource = TaxDetailDT; gvTaxDetail.DataBind(); 
            }

            gvPendingInvoice.DataSource = (DataTable) ViewState["DtUnPaid"];

            gvPendingInvoice.DataBind();
            gvPendingInvoice.KeyFieldName = "Ref No";
        }

        protected void btnSelectCustomer_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string sql = "select * from Customer where CusCode='" + gvlCustomerCode.Text.Trim() + "'";
            dt = myFunc.GetData(sql);

            txtCustomerName.Text = dt.Rows[0]["CusDesc"].ToString();

            dpReceiptDate.Date = DateTime.Today;
            dpInstDate.Date = DateTime.Today;
            rbPaymentReceiptType.Enabled = false;
            gvlReceiptNo.Enabled = false;
            dpReceiptDate.Enabled = false;

            string sinvno = gvlCustomerCode.GridView.GetRowValues(gvlCustomerCode.GridView.FocusedRowIndex, "Ref No").ToString();

            txtInvNo.Text = sinvno;

            cmbPaymentMode.Value = "C";

            cmbBank.SelectedIndex = -1;
            //LoadMasterData();
            MakeGrid();
            btnRemovePendingRow.Enabled = true;
            btnAutoAdj.Enabled = true;
            cmbBank.Value = "0";


            gvlCustomerCode.Enabled = true;
            txtRemarks.Enabled = true;
            cmbPaymentMode.Enabled = true;
            txtInstrumentNo.Enabled = true;
            txtInvNo.Enabled = false;
            txtInstAmt.Enabled = true;

            cmbBank.Enabled = true;
            txtBranch.Enabled = true;
            cmbReceiptHead.Enabled = true;
            txtTaxAmt.Enabled = true;

            rbPaymentReceiptType.Enabled = false;
            dpInstDate.Enabled = false;
            dpReceiptDate.Enabled = false;

            btnLoadAllPendingInv.Enabled = true;
            txtSrchInvNo.Enabled = true;
            btnAddInvToGrid.Enabled = true;



            txtInstAmt.Focus();
        }

        private void MakeGrid()
        {
            Session["PaymentStatus"] = "UNPAID";
            dtPendingInvoice = myFunc.PendingPaymentReceipt(rbPaymentReceiptType.Value.ToString(), gvlCustomerCode.Text, "UNPAID", gvlReceiptNo.Text, txtInvNo.Text, Session["DealerCode"].ToString());

            for (int i = 1; i < dtPendingInvoice.Rows.Count; i++)
            {
                dtPendingInvoice.Rows[i].Delete();
            }

            dtPendingInvoice.AcceptChanges();
            ViewState["DtUnPaid"] = dtPendingInvoice;

            gvPendingInvoice.DataSource = dtPendingInvoice;
            gvPendingInvoice.DataBind();
            gvPendingInvoice.KeyFieldName = "Ref No";

            if (gvPendingInvoice.Columns.Count > 0 && rbPaymentReceiptType.Value.ToString() == "CounterS")
            {
                if (gvPendingInvoice.Columns["CheckVal"] != null)
                {
                    gvPendingInvoice.Columns["CheckVal"].Visible = false;
                }
            }
        }

        private void createTaxDetail()
        {
            TaxDetailDT = new DataTable();
            TaxDetailDT.Columns.Add(new DataColumn("SNO", typeof(int)));
            TaxDetailDT.Columns.Add(new DataColumn("ReceiptHead", typeof(string)));
            TaxDetailDT.Columns.Add(new DataColumn("Amount", typeof(string)));

            gvTaxDetail.DataSource = TaxDetailDT; gvTaxDetail.DataBind();
            ViewState["TaxDetail"] = TaxDetailDT;
        }
        private void LoadMasterData()
        {
            //FillInvoiceDetailGrid();
            SqlParameter[] dsMasterPram = { new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                        new SqlParameter("@ReceiptNo",SqlDbType.Char,8), };
            dsMasterPram[0].Value = Session["DealerCode"].ToString();
            dsMasterPram[1].Value = (gvlReceiptNo.Text == "" ? null : gvlReceiptNo.Text);
            dsReceiptMaster = new DataSet();
            dsReceiptMaster = myFunc.FillDataSet("sp_W2_PaymentReceiptMaster_Select", dsMasterPram);
            if (dsReceiptMaster.Tables[0].Rows.Count > 0)
            {
                gvlCustomerCode.Text = dsReceiptMaster.Tables[0].Rows[0]["CusCode"].ToString();
                Session["CusCode"] = dsReceiptMaster.Tables[0].Rows[0]["CusCode"].ToString(); ;

                fillData();
                gvlCustomerCode.Text = dsReceiptMaster.Tables[0].Rows[0]["CusCode"].ToString();
                //dpReceiptDate.Date = Convert.ToDateTime( dsReceiptMaster.Tables[0].Rows[0]["ReceiptDate"].ToString());
                dpReceiptDate.Date =Convert.ToDateTime( myFunc.SaveDate( dsReceiptMaster.Tables[0].Rows[0]["ReceiptDate"].ToString()));
                rbPaymentReceiptType.Value = dsReceiptMaster.Tables[0].Rows[0]["InvoiceType"].ToString();
                txtRemarks.Text = dsReceiptMaster.Tables[0].Rows[0]["Remarks"].ToString();
                cmbPaymentMode.Value = dsReceiptMaster.Tables[0].Rows[0]["PayModeCode"].ToString();
                txtInstrumentNo.Text = dsReceiptMaster.Tables[0].Rows[0]["InsNo"].ToString();
                dpInstDate.Date = Convert.ToDateTime(myFunc.SaveDate( dsReceiptMaster.Tables[0].Rows[0]["InsDate"].ToString()));
                txtInstAmt.Text = dsReceiptMaster.Tables[0].Rows[0]["AmountPaid"].ToString();
                cmbBank.Value = dsReceiptMaster.Tables[0].Rows[0]["BankCode"].ToString().Trim();
                txtBranch.Text = dsReceiptMaster.Tables[0].Rows[0]["Branch"].ToString();
                //txttotInv.Text = dsReceiptMaster.Tables[0].Rows[0]["InvTotal"].ToString();
                //txttotOutstanding.Text = dsReceiptMaster.Tables[0].Rows[0]["OutSTTotal"].ToString();
                //txttotAdj.Text = dsReceiptMaster.Tables[0].Rows[0]["InvAdjTotal"].ToString();
                Session["PaymentStatus"] = "PAID";
                
                dtPendingInvoice = myFunc.PendingPaymentReceipt(rbPaymentReceiptType.Value.ToString(), gvlCustomerCode.Text, "PAID", gvlReceiptNo.Text, txtInvNo.Text,Session["DealerCode"].ToString());
                gvPendingInvoice.DataSource = dtPendingInvoice;
                gvPendingInvoice.DataBind();
                gvPendingInvoice.KeyFieldName = "Ref No";

                //if (gvPendingInvoice.Columns.Count > 8)
                //{
                //    gvPendingInvoice.Columns["Action"].Visible = false;
                //}
                //else
                //{
                if (gvPendingInvoice.Columns["CheckVal"] != null)
                {
                    gvPendingInvoice.Columns["CheckVal"].Visible = false;
                }
                //}
                

                Session["dtPendingInvoice"] = dtPendingInvoice;
                ViewState["DtUnPaid"] = dtPendingInvoice;

                txtTotAmount.Text = (double.Parse(txtInstAmt.Text.Trim() == "" ? "0" : txtInstAmt.Text.Trim())
                                    + double.Parse(txtTotTax.Text.Trim() == "" ? "0" : txtTotTax.Text.Trim())).ToString();

                btnDelete.Enabled = true;
                btnPrint.Enabled = true;

                gvlCustomerCode.Enabled = false;
                txtRemarks.Enabled = false;
                cmbPaymentMode.Enabled = false;
                txtInstrumentNo.Enabled = false;
                txtInvNo.Enabled = false;
                txtInstAmt.Enabled = false;

                cmbBank.Enabled = false;
                txtBranch.Enabled = false;
                cmbReceiptHead.Enabled = false;
                txtTaxAmt.Enabled = false;

                rbPaymentReceiptType.Enabled = false;
                dpInstDate.Enabled = false;
                dpReceiptDate.Enabled = false;

            }
            else
            {
                fillData();
                Session["PaymentStatus"] = "UNPAID";
                dtPendingInvoice = myFunc.PendingPaymentReceipt(rbPaymentReceiptType.Value.ToString(), gvlCustomerCode.Text, "UNPAID", gvlReceiptNo.Text, txtInvNo.Text,Session["DealerCode"].ToString());

                ViewState["DtUnPaid"] = dtPendingInvoice;
                UpdatePendInvGrid();

                try
                {

                    gvPendingInvoice.DataSource = dtPendingInvoice;
                    gvPendingInvoice.DataBind();
                    gvPendingInvoice.KeyFieldName = "Ref No";

                    if (gvPendingInvoice.Columns.Count > 0)
                    {
                        if (gvPendingInvoice.Columns["CheckVal"] != null)
                        {
                            gvPendingInvoice.Columns["CheckVal"].Visible = false;
                        }
                    }
                }
                catch(Exception c)
                {
                    string chk = c.Message;
                }

                

               

                //GridViewCommandColumn col = new GridViewCommandColumn();
                //col.Name = "Action";

                //gvPendingInvoice.Columns.Add(col);
                //col.ShowDeleteButton = true;

                
                
            }
        }
        private void UpdatePendInvGrid()
        {
            

            DataTable dt = (DataTable) ViewState["DtUnPaid"];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["S NO"] = (i+1).ToString();
                
            }

            ViewState["DtUnPaid"] = dt;
            //gvPendingInvoice.DataSource = dt;
            ////gvPendingInvoice.KeyFieldName = "Ref No";
            //gvPendingInvoice.DataBind();
           
        }
        private void fillData()
        {
            string CusCode=gvlCustomerCode.Text;
            if (CusCode == "")
            {
                CusCode = Session["CusCode"].ToString();
            }

            txtCustomerName.Text = myFunc.GetStringValuesAgainstCodes("CusCode", CusCode, "CusDesc", "Customer");
            SqlParameter[] dsTaxDetailPram = { new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                           new SqlParameter("@ReceiptNo",SqlDbType.Char,8) };
            dsTaxDetailPram[0].Value = Session["DealerCode"].ToString();
            dsTaxDetailPram[1].Value = (gvlReceiptNo.Text == "" ? null : gvlReceiptNo.Text);
            dsReceiptTaxDetail = new DataSet(); 
            dsReceiptTaxDetail = myFunc.FillDataSet("sp_W2_PaymentReceiptTaxDetail_Select", dsTaxDetailPram);
            if (dsReceiptTaxDetail.Tables[0].Rows.Count == 0) dsReceiptTaxDetail.Tables[0].Rows.Add(dsReceiptTaxDetail.Tables[0].NewRow());
            ViewState["TaxDetail"] = dsReceiptTaxDetail.Tables[0];
            gvTaxDetail.DataSource = dsReceiptTaxDetail; gvTaxDetail.DataBind();

            double tt = 0;
            for (int i = 0; i < dsReceiptTaxDetail.Tables[0].Rows.Count; i++)
            {
                tt = tt + double.Parse(dsReceiptTaxDetail.Tables[0].Rows[i]["Amount"].ToString() == "" ? "0" : dsReceiptTaxDetail.Tables[0].Rows[i]["Amount"].ToString());
            }

            txtTotTax.Text = tt.ToString();
        }
        private void ShowPendingSummary()
        {
            //gridView1.Columns["UnitsInStock"].Summary.Add(DevExpress.Data.SummaryItemType.Average, "UnitsInStock", "Avg={0:n2}");
            //gridView1.Columns["UnitsInStock"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "UnitsInStock", "Sum={0}");
            //GridColumnSummaryItem item = new GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Max, "UnitsInStock", "Max={0}");
            //gridView1.Columns["UnitsInStock"].Summary.Add(item);

           // GridColumnSummaryItem item = new GridColumnSummaryItem()
           // gvPendingInvoice.Columns["OutStanding"].

        }

        protected void gvPendingInvoice_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
        {
            String x = gvPendingInvoice.GetRowValues(gvPendingInvoice.FocusedRowIndex, "OutStanding").ToString();
        }

        protected void gvPendingInvoice_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            
        }

        protected void gvPendingInvoice_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            String x = gvPendingInvoice.GetRowValues(gvPendingInvoice.FocusedRowIndex, "OutStanding").ToString();

            //ColumnView View = sender as ColumnView;
            //View.SetRowCellValue(e.RowHandle, View.Columns["Type"], true);
            //View.SetRowCellValue(e.RowHandle, View.Columns["CreatedDate"], DateTime.Today);
            
        }

        protected void gvPendingInvoice_DataBinding(object sender, EventArgs e)
        {
           // String x = gvPendingInvoice.GetRowValues(gvPendingInvoice.FocusedRowIndex, "OutStanding").ToString();
        }

        protected void btnAutoAdj_Click(object sender, EventArgs e)
        {
            if ((myFunc.IsExist("ReceiptNo", gvlReceiptNo.Text, "paymentReceiptMaster", Session["DealerCode"].ToString()))
             & (gvlReceiptNo.Text.Trim().Length != 0))
            {
                lblMSG.Text = "Record already exists! "+gvlReceiptNo.Text;
                PopupControlMSG.ShowOnPageLoad = true;
               
                return;
            }


            txtTotAmount.Text = (double.Parse(txtTotTax.Text == "" ? "0" : txtTotTax.Text)
                                + double.Parse(txtInstAmt.Text == "" ? "0" : txtInstAmt.Text)).ToString();


            if (double.Parse(txtTotAmount.Text.Trim().Replace("&nbsp;", "") == "" ? "0" : txtTotAmount.Text.Trim()) < 1)
            {
                lblMSG.Text = "Total amount should not be zero. " + txtTotAmount.Text;
                PopupControlMSG.ShowOnPageLoad = true;

                return;
            }

            ///get summary value
            ///ASPxSummaryItem summaryItem = Grid.TotalSummary.First(i => i.Tag == "C2_Sum");
           // return Grid.GetTotalSummaryValue(summaryItem);

            ASPxSummaryItem sumOutStanding = gvPendingInvoice.TotalSummary.First(i => i.Tag == "OutStanding");

            double RunningTotal = double.Parse(txtTotAmount.Text.Trim().Replace("&nbsp;", "") == "" ? "0" : txtTotAmount.Text.Trim());
            double Count = 0.00;
            if (RunningTotal > double.Parse(gvPendingInvoice.GetTotalSummaryValue(sumOutStanding).ToString() == "" ? "0" : gvPendingInvoice.GetTotalSummaryValue(sumOutStanding).ToString()))
            {
                lblMSG.Text = "Adjusted amount not greater then Total Amount amount " + txtTotAmount.Text;
                PopupControlMSG.ShowOnPageLoad = true;
               
                return;
            }
            if (RunningTotal > 0)
            {

                DataTable dt = (DataTable) ViewState["DtUnPaid"];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //if (RunningTotal > double.Parse(row.Cells[4].Text.Trim().Replace("&nbsp;", "") == "" ? "0" : row.Cells[4].Text.Trim()))
                    //{
                    //    grl.UserMsg(lblMsg, Color.Red, "Adjusted amount not greater then Total Amount amount", txttotAmount);
                    //    return;
                    //}
                    if (RunningTotal >
                        double.Parse(dt.Rows[i]["OutStanding"].ToString().Trim() == "" ? "0" : dt.Rows[i]["OutStanding"].ToString().Trim()))
                    {
                        dt.Rows[i]["Adjustment"] = dt.Rows[i]["OutStanding"].ToString();
                        dt.Rows[i]["Remaining"] = (double.Parse(dt.Rows[i]["OutStanding"].ToString().Trim()) - double.Parse(dt.Rows[i]["Adjustment"].ToString().Trim())).ToString();
                        RunningTotal = RunningTotal - double.Parse(dt.Rows[i]["OutStanding"].ToString().Trim());
                        //txttotAdj.Text = dt.Rows[i]["OutStanding"].ToString().Trim();
                        txtAdj.Value = "t";
                        Count = Count + double.Parse(dt.Rows[i]["Adjustment"].ToString().Trim() == "" ? "0" : dt.Rows[i]["Adjustment"].ToString().Trim());
                        //gvPendingInvoice.FooterRow.Cells[5].Text = Count.ToString();
                    }

                    else
                    {
                        dt.Rows[i]["Adjustment"] = RunningTotal.ToString();
                        
                        dt.Rows[i]["Remaining"] = (double.Parse(dt.Rows[i]["OutStanding"].ToString().Trim() == "" ? "0" : dt.Rows[i]["OutStanding"].ToString().Trim()) - double.Parse(dt.Rows[i]["Adjustment"].ToString().Trim() == "" ? "0" : dt.Rows[i]["Adjustment"].ToString().Trim())).ToString();
                        //txttotAdj.Text = dt.Rows[i]["Adjustment"].ToString().Trim();
                        txtAdj.Value = "t";
                        Count = Count + double.Parse(dt.Rows[i]["Adjustment"].ToString().Trim() == "" ? "0" : dt.Rows[i]["Adjustment"].ToString().Trim());
                        //gvPendingInvoice.FooterRow.Cells[5].Text = Count.ToString();
                        break;

                    }

                    //if (RunningTotal > double.Parse(row.Cells[4].Text.Trim().Replace("&nbsp;", "") == ""?"0":row.Cells[4].Text.Trim()))
                    //{
                    //   RunningTotal= RunningTotal - double.Parse(row.Cells[4].Text.Trim().Replace("&nbsp;", "") == ""?"0":row.Cells[4].Text.Trim());
                    //}
                    //else

                }
                ViewState["DtUnPaid"] = dt;
                gvPendingInvoice.DataSource = dt;
                gvPendingInvoice.KeyFieldName = "Ref No";
                gvPendingInvoice.DataBind();

                if (rbPaymentReceiptType.SelectedItem.Value.ToString() == "CounterS")
                {
                    if (gvPendingInvoice.Columns["CheckVal"] != null)
                    {
                        gvPendingInvoice.Columns["CheckVal"].Visible = false;
                    }
                }


                btnSave.Enabled = true;
                btnDelete.Enabled = false;
                btnPrint.Enabled = true;

                btnAddTax.Enabled = true;
                btnTaxClear.Enabled = true;

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

        protected void btnRemovePendingRow_Click(object sender, EventArgs e)
        {
            if (gvPendingInvoice.FocusedRowIndex < 0)
            {
                lblMSG.Text = "Please select an invoice " ;
                PopupControlMSG.ShowOnPageLoad = true;
               
                return;
            }
            DataTable dt = (DataTable) ViewState["DtUnPaid"];

            if (dt.Rows.Count == 1)
            {
                lblMSG.Text = "One invoice must be in there ";
                PopupControlMSG.ShowOnPageLoad = true;
                return;
            }

            string isadj = gvPendingInvoice.GetRowValues(gvPendingInvoice.FocusedRowIndex, "Adjustment").ToString();

            if (isadj != "")
            {
                txtTotAmount.Text = "0";
            }

            int ridx = (int)(gvPendingInvoice.FocusedRowIndex);
            
            dt.Rows.Remove(dt.Rows[ridx]);
            dt.AcceptChanges();

            ViewState["DtUnPaid"] = dt;
            UpdatePendInvGrid();

            gvPendingInvoice.DataSource = (DataTable)ViewState["DtUnPaid"];
            gvPendingInvoice.DataBind();
        }

        protected void btnAddTax_Click(object sender, EventArgs e)
        {
           // TextBox[] textBoxes = { txtTaxAmt };
            //if (!MasterValidation(textBoxes)) return;
            if (txtCustomerName.Text == "") return;
            else
            {
                TaxDetailDT = (DataTable)ViewState["TaxDetail"];
                search_result = false;

                for (int i = 0; i < TaxDetailDT.Rows.Count; i++) 
                {
                    if (TaxDetailDT.Rows[i]["ReceiptHead"].ToString().Trim() == ""
                        | TaxDetailDT.Rows[i]["ReceiptHead"].ToString().Trim() == cmbReceiptHead.Text.Trim())
                    {
                        DataRow row = TaxDetailDT.Rows[i];
                        row["SNO"] = TaxDetailDT.Rows.Count + 1;
                        row["ReceiptHead"] = cmbReceiptHead.Text.Trim();
                        row["Amount"] = txtTaxAmt.Text.Trim();

                        //TaxDetailDT.Rows.Add(row);
                        search_result = true;
                    }
                }
                if (search_result == false)
                {
                    DataRow row = TaxDetailDT.NewRow();
                    row["SNO"] = TaxDetailDT.Rows.Count + 1;
                    row["ReceiptHead"] = cmbReceiptHead.Text.Trim();
                    row["Amount"] = txtTaxAmt.Text.Trim();
                    TaxDetailDT.Rows.Add(row);
                }

                double tt = 0;
                for (int i = 0; i < TaxDetailDT.Rows.Count; i++)
                {
                    tt = tt + double.Parse(TaxDetailDT.Rows[i]["Amount"].ToString());
                }

                txtTotTax.Text = tt.ToString();

                ViewState["Parts"] = TaxDetailDT; gvTaxDetail.DataSource = TaxDetailDT; gvTaxDetail.DataBind();
                txtTotAmount.Text = (double.Parse(txtInstAmt.Text == "" ? "0" : txtInstAmt.Text)
                                   + double.Parse(txtTotTax.Text == "" ? "0" : txtTotTax.Text)).ToString();
                txtAdj.Value = "f";

                btnAutoAdj_Click(btnAutoAdj, EventArgs.Empty);

                btnTaxRemove.Enabled = true;
            }
        }

        protected void clearAll()
        {
            gvlReceiptNo.Text = "";
            gvlReceiptNo.Enabled = true;

            rbPaymentReceiptType.SelectedIndex = 2;
            rbPaymentReceiptType.Enabled = true;
            gvlCustomerCode.Text = "";
            gvlCustomerCode.Enabled = true;
            txtCustomerName.Text = "";
            txtRemarks.Text = "";

            cmbPaymentMode.Value = "C"; 
            txtInstrumentNo.Text = ""; 
            txtInstAmt.Text = "";
            cmbBank.Value = "0"; 
            txtBranch.Text = "";

            //txttotInv.Text = ""; 
            //txttotOutstanding.Text = ""; 
            //txttotAdj.Text = "";
            txtTaxAmt.Text = ""; 
            txtTotAmount.Text = ""; 
            txtTotTax.Text = "";
            txtInvNo.Text = "";
            

            createTaxDetail();
            gvPendingInvoice.DataSource = null;
            gvPendingInvoice.DataBind();
            ViewState["DtUnPaid"] = null;

            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            btnPrint.Enabled = false;

            btnTaxClear.Enabled = false;
            btnTaxRemove.Enabled = false;
            btnAddTax.Enabled = false;

            btnAutoAdj.Enabled = false;
            btnRemovePendingRow.Enabled = false;

            btnLoadAllPendingInv.Enabled = false;
            txtSrchInvNo.Enabled = false;
            btnAddInvToGrid.Enabled = false;
            btnRemovePendingRow.Enabled=false;

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clearAll();
            dpInstDate.Date = dpReceiptDate.Date = System.DateTime.Now.Date;

            rbPaymentReceiptType_SelectedIndexChanged(rbPaymentReceiptType, EventArgs.Empty);
            //gvlCustomerCode.DataBind();
            gvlReceiptNo.DataBind();
        }

        protected void btnTaxClear_Click(object sender, EventArgs e)
        {
            txtTaxAmt.Text = "";
        }

        protected void btnTaxRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvTaxDetail.VisibleRowCount == 0)
                {
                    txtTotTax.Text = "";
                    return;
                }

                if (gvTaxDetail.FocusedRowIndex < 0)
                {
                    lblMSG.Text = "Please select an tax row ";
                    PopupControlMSG.ShowOnPageLoad = true;

                    return;
                }



                int ridx = (int)(gvTaxDetail.FocusedRowIndex);
                TaxDetailDT = (DataTable)ViewState["TaxDetail"];
                TaxDetailDT.Rows.Remove(TaxDetailDT.Rows[ridx]);
                TaxDetailDT.AcceptChanges();
                txtTaxAmt.Text = "";
                if (TaxDetailDT.Rows.Count == 0) TaxDetailDT.Rows.Add(TaxDetailDT.NewRow());
                ViewState["TaxDetail"] = TaxDetailDT; gvTaxDetail.DataSource = TaxDetailDT; gvTaxDetail.DataBind();


                double tt = 0;
                for (int i = 0; i < TaxDetailDT.Rows.Count; i++)
                {
                    tt = tt + double.Parse(TaxDetailDT.Rows[i]["Amount"].ToString());
                }

                txtTotTax.Text = tt.ToString();

                btnAutoAdj_Click(btnAutoAdj, EventArgs.Empty);
                if (TaxDetailDT.Rows.Count == 0)
                {
                    btnTaxRemove.Enabled = false;
                }
            }
            catch 
            {
                txtTotTax.Text = "";

                btnAutoAdj_Click(btnAutoAdj, EventArgs.Empty);
                return; 
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //Check rocord is already exist?
            if (gvlReceiptNo.Text.Trim().Length != 0)
            {
                //PaymentReceiptEntry2(1);
                if (myFunc.IsExist("ReceiptNo", gvlReceiptNo.Text, "paymentReceiptMaster",Session["DealerCode"].ToString()))
                //& (txtReceiptNo.Text.Trim().Length!=0))
                {
                    //PaymentReceiptEntry2(2);
                    lblMSG.Text = "Record already exist ";
                    PopupControlMSG.ShowOnPageLoad = true;
                    //.UserMsg(lblMsg, Color.Red, "Record already exists!", txtReceiptNo);
                    return;
                }
                //PaymentReceiptEntry2(3);
            }

            //Check textbox null or empty
            if(gvlCustomerCode.Text == "" || txtInstAmt.Text =="" || txtTotAmount.Text =="")
            {
                return;
            }

            //if (txttotAdj.Text.Trim() == "0")
            //{
            //    lblMSG.Text = "Please press Auto Adjest (FIFO) button ";
            //    PopupControlMSG.ShowOnPageLoad = true;

            //    //grl.UserMsg(lblMsg, Color.Red, "Please press Auto Adjest (FIFO) button", txttotAmount);
            //    return;
            //}


            ASPxSummaryItem sumOutStanding = gvPendingInvoice.TotalSummary.First(i => i.Tag == "OutStanding");
            
            if (double.Parse(txtTotAmount.Text.Trim()) > double.Parse(gvPendingInvoice.GetTotalSummaryValue(sumOutStanding).ToString() == "" ? "0" : gvPendingInvoice.GetTotalSummaryValue(sumOutStanding).ToString()))
            {
                lblMSG.Text = "Adjusted amount not greater then Total Amount amount ";
                PopupControlMSG.ShowOnPageLoad = true;

                //grl.UserMsg(lblMsg, Color.Red, "Adjusted amount not greater then Total Amount amount", txttotAmount);
                return;
            }

            //if((double .Parse (txttotAdj .Text )- double .Parse (txtTotTax .Text )) != double .Parse (txtInstAmt .Text ) )
            //{
            //    grl.UserMsg(lblMsg, Color.Red, "Adjusted amount not equal to Instrument Amount amount", txttotAmount);
            //    return;
            //}
            if (txtAdj.Value == "f")
            {
                lblMSG.Text = "Please press Auto (FIFO) ";
                PopupControlMSG.ShowOnPageLoad = true;

               // grl.UserMsg(lblMsg, Color.Red, "Please press Auto (FIFO) ", txttotAmount);
                return;
            }



            PaymentReceiptEntry();
        }

        private void PaymentReceiptEntry()
        {
            //PaymentReceiptEntry2(1);
            SqlParameter[] PmtRecMaster_param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),          /*1*/ new SqlParameter("@ReceiptNo",SqlDbType.Char,8),
           /*2*/ new SqlParameter("@ReceiptDate",SqlDbType.DateTime),       /*3*/ new SqlParameter("@InvoiceType",SqlDbType.VarChar,8),
           /*4*/ new SqlParameter("@CusCode",SqlDbType.VarChar,8),          /*5*/ new SqlParameter("@InsCompCode",SqlDbType.Char,1),
           /*6*/ new SqlParameter("@BranchCode",SqlDbType.Char,1),          /*7*/ new SqlParameter("@InsCusFlag",SqlDbType.Char,1),
           /*8*/ new SqlParameter("@Remarks",SqlDbType.Char,100),           /*9*/ new SqlParameter("@PayModeCode",SqlDbType.Char,1),
           /*10*/ new SqlParameter("@InsNo",SqlDbType.VarChar,15),          /*11*/ new SqlParameter("@InsDate",SqlDbType.DateTime),
           /*12*/ new SqlParameter("@AmountPaid",SqlDbType.Float),          /*13*/ new SqlParameter("@BankCode",SqlDbType.Char,5),
           /*14*/ new SqlParameter("@Branch",SqlDbType.VarChar,50),         /*15*/ new SqlParameter("@AdvanceAmount",SqlDbType.Float),
           /*16*/ new SqlParameter("@InvTotal",SqlDbType.Float),            /*17*/ new SqlParameter("@OutSTTotal",SqlDbType.Float),
           /*18*/ new SqlParameter("@InvAdjTotal",SqlDbType.Float),         /*19*/ new SqlParameter("@DelFlag",SqlDbType.Char,1),
           /*20*/ new SqlParameter("@UpdUser",SqlDbType.Char,50),           
           /*22*/ new SqlParameter("@UpdTerm",SqlDbType.Char,50),          
           /*24*/ new SqlParameter("@VoucherNo",SqlDbType.VarChar,50),      /*25*/ new SqlParameter("@VoucherFlag",SqlDbType.Char,1),
           /*26*/ new SqlParameter("@AdvancePaid",SqlDbType.Decimal)
            };

            PmtRecMaster_param[0].Value = Session["DealerCode"].ToString();
            //Auto Code Generation Decision on Insert OR Update
            //txtJobCardNo.Text = grl.AutoGen("JobCardMaster", "JobCardCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy")); 
            //JobCardMaster_param[1].Value = txtJobCardNo.Text;            
            PmtRecMaster_param[2].Value = myFunc.SaveDate(dpReceiptDate.Text); //EstimateCode        
            PmtRecMaster_param[3].Value = rbPaymentReceiptType.SelectedItem.Value;
            PmtRecMaster_param[4].Value = gvlCustomerCode.Text.Trim();
            PmtRecMaster_param[5].Value = ""; 
            PmtRecMaster_param[6].Value = ""; 
            PmtRecMaster_param[7].Value = "N";
            PmtRecMaster_param[8].Value = txtRemarks.Text.Trim().ToUpper();
            PmtRecMaster_param[9].Value = cmbPaymentMode.Value.ToString();
            PmtRecMaster_param[10].Value = txtInstrumentNo.Text.Trim().ToUpper();

            PmtRecMaster_param[11].Value = myFunc.SaveDate(dpInstDate.Text);
            PmtRecMaster_param[12].Value = txtInstAmt.Text.Trim().Replace("&nbsp;", "") == "" ? "0" : txtInstAmt.Text.Trim();
            PmtRecMaster_param[13].Value = cmbBank.Value.ToString();
            PmtRecMaster_param[14].Value = txtBranch.Text.Trim().Replace("&nbsp;", "");
            PmtRecMaster_param[15].Value = "0";

            ASPxSummaryItem sumOutStanding = gvPendingInvoice.TotalSummary.First(i => i.Tag == "OutStanding");
            ASPxSummaryItem sumAdjustment = gvPendingInvoice.TotalSummary.First(i => i.Tag == "Adjustment");
            ASPxSummaryItem sumRefAmount = gvPendingInvoice.TotalSummary.First(i => i.Tag == "RefAmount");

            PmtRecMaster_param[16].Value = double.Parse(gvPendingInvoice.GetTotalSummaryValue(sumRefAmount).ToString().Trim());
            PmtRecMaster_param[17].Value = double.Parse(gvPendingInvoice.GetTotalSummaryValue(sumOutStanding).ToString().Trim());
            PmtRecMaster_param[18].Value = double.Parse(gvPendingInvoice.GetTotalSummaryValue(sumAdjustment).ToString().Trim());
            PmtRecMaster_param[19].Value = "N";

            PmtRecMaster_param[20].Value = Session["UserName"].ToString();
            //PmtRecMaster_param[21].Value = grl.SaveDate(DateTime.Now.ToString("yyyy-MM-dd"));
            //PmtRecMaster_param[22].Value = grl.SaveTime(DateTime.Now.ToString("HH:mm"));
            PmtRecMaster_param[21].Value = Environment.MachineName.ToString();

            PmtRecMaster_param[22].Value = ""; 
            PmtRecMaster_param[23].Value = "N"; 
            PmtRecMaster_param[24].Value = "0";
            //PaymentReceiptEntry2(2);
            try
            {
                //ObjTrans.BeginTransaction(ref Trans);                   
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    
                    string newReceiptNo = myFunc.AutoGen("PaymentReceiptMaster", "ReceiptNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                    gvlReceiptNo.Text = newReceiptNo;
                    PmtRecMaster_param[1].Value = newReceiptNo;
                    if (myFunc.ExecuteSP_NonQuery("[sp_W2_PaymentReceipt_Master_Insert]", PmtRecMaster_param, Trans))
                    {

                        SqlParameter[] PmtRecDetail_param = {                                            
                                                            /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                            /*1*/ new SqlParameter("@ReceiptNo",SqlDbType.Char,8),
                                                            /*2*/ new SqlParameter("@InvoiceType",SqlDbType.Char,8),
                                                            /*3*/ new SqlParameter("@InvoiceNo",SqlDbType.Char,8),
                                                            /*4*/ new SqlParameter("@InvoiceDate",SqlDbType.DateTime),
                                                            /*5*/ new SqlParameter("@InvAmount",SqlDbType.Decimal),
                                                            /*6*/ new SqlParameter("@OutStAmount",SqlDbType.Decimal),
                                                            /*7*/ new SqlParameter("@AdjAmount",SqlDbType.Decimal)
                                                        };

                        PmtRecDetail_param[0].Value = Session["DealerCode"].ToString();
                        PmtRecDetail_param[1].Value = newReceiptNo;
                        PmtRecDetail_param[2].Value = rbPaymentReceiptType.SelectedItem.Value;

                        DataTable dt = (DataTable)ViewState["DtUnPaid"];
                        for(int i=0;i<dt.Rows.Count;i++)                       
                        {
                            PmtRecDetail_param[3].Value = dt.Rows[i]["Ref No"].ToString().Trim();
                            PmtRecDetail_param[4].Value = myFunc.SaveDate(dt.Rows[i]["Ref Date"].ToString().Trim());
                            PmtRecDetail_param[5].Value = dt.Rows[i]["Ref Amount"].ToString().Trim();
                            PmtRecDetail_param[6].Value = dt.Rows[i]["Outstanding"].ToString().Trim();
                            if (dt.Rows[i]["Adjustment"].ToString().Trim() == "")
                            {
                                PmtRecDetail_param[7].Value = "0";
                            }
                            else
                            {
                                PmtRecDetail_param[7].Value = dt.Rows[i]["Adjustment"].ToString().Trim();
                            }
                            myFunc.ExecuteSP_NonQuery("[sp_W2_PaymentReceipt_Detail_Insert]", PmtRecDetail_param, Trans);
                            if (rbPaymentReceiptType.Value.ToString() == "Service")
                            {
                                string IQuery = "Update dbo.CustomerInvoice set PaidC= paidC +'" + PmtRecDetail_param[7].Value + "' " +
                                      "Where DealerCode='" + Session["DealerCode"].ToString() + "' and InvoiceNo='" + PmtRecDetail_param[3].Value + "'";
                                myFunc.ExecuteQuery(IQuery, Trans);
                            }
                            else if (rbPaymentReceiptType.Value.ToString() == "CounterSales")
                            {
                                string IQuery = "Update CounterSaleMaster set TotReceipt= TotReceipt +'" + PmtRecDetail_param[7].Value + "' " +
                                     "Where DealerCode='" + Session["DealerCode"].ToString() + "' and SaleInvNo='" + PmtRecDetail_param[3].Value + "'";
                                myFunc.ExecuteQuery(IQuery, Trans);
                            }
                            else if (rbPaymentReceiptType.Value.ToString() == "Sales")
                            {
                                string IQuery = "Update ExpenditureMaster set TotReceipt= TotReceipt +'" + PmtRecDetail_param[7].Value + "' " +
                                     "Where DealerCode='" + Session["DealerCode"].ToString() + "' and ExpendCode='" + PmtRecDetail_param[3].Value + "'";
                                myFunc.ExecuteQuery(IQuery, Trans);
                            }
                        }

                        SqlParameter[] PmtRecTaxDetail_param = {                                            
                                                            /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                            /*1*/ new SqlParameter("@ReceiptNo",SqlDbType.Char,8),
                                                            /*2*/ new SqlParameter("@ReceiptHead",SqlDbType.Char,50),
                                                            /*3*/ new SqlParameter("@AccountCode",SqlDbType.Char,1),
                                                            /*4*/ new SqlParameter("@Amount",SqlDbType.Decimal),
                                                           };

                        PmtRecTaxDetail_param[0].Value = Session["DealerCode"].ToString();
                        PmtRecTaxDetail_param[1].Value = newReceiptNo;

                        dt = (DataTable)ViewState["TaxDetail"];
                        for(int i=0;i<dt.Rows.Count;i++)
                        {
                            if (dt.Rows[i]["ReceiptHead"].ToString().Trim() != "")
                            {
                                PmtRecTaxDetail_param[2].Value = dt.Rows[i]["ReceiptHead"].ToString();
                                PmtRecTaxDetail_param[3].Value = "";
                                PmtRecTaxDetail_param[4].Value = dt.Rows[i]["Amount"].ToString(); ;
                                myFunc.ExecuteSP_NonQuery("[sp_W2_PaymentReceipt_TaxDetail_Insert]", PmtRecTaxDetail_param, Trans);
                            }
                        }
                        //,AccountCode,Amount
                        //Auto Code Generation Decision on Insert OR Update
                        //txtJobCardNo.Text = grl.AutoGen("JobCardMaster", "JobCardCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy")); 
                        //JobCardMaster_param[1].Value = txtJobCardNo.Text;            
                    }
                    else 
                    {
                        lblMSG.Text = "Record not save!! ";
                        PopupControlMSG.ShowOnPageLoad = true;

                        
                    }
                    ObjTrans.CommittTransaction(ref Trans);
                    lblMSG.Text = "Record saved successfully: New Receipt No is : "+newReceiptNo;
                    PopupControlMSG.ShowOnPageLoad = true;

                    btnClear_Click(btnClear, EventArgs.Empty);
                }
            }
            catch (Exception ex) { ObjTrans.RollBackTransaction(ref Trans); throw ex; }
        }

        protected void btnSelectReceipt_Click(object sender, EventArgs e)
        {
            LoadMasterData();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //TextBox[] textBoxes = { txtCustomerCode, txtInstAmt, txttotAmount };
            if (txtCustomerName.Text=="") return;
            else
            {
                if ((myFunc.IsExist("ReceiptNo", gvlReceiptNo.Text.Trim(), "paymentReceiptMaster",Session["DealerCode"].ToString(),""))
                    & (gvlReceiptNo.Text.Trim().Length != 0))
                {

                    SqlParameter[] PmtRecMaster_param = { /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                        /*1*/ new SqlParameter("@ReceiptNo",SqlDbType.Char,8)           
                                                    };
                    PmtRecMaster_param[0].Value = Session["DealerCode"].ToString();
                    PmtRecMaster_param[1].Value = gvlReceiptNo.Text.Trim();

                    try
                    {
                        if (ObjTrans.BeginTransaction(ref Trans) == true)
                        {
                            if (myFunc.ExecuteSP_NonQuery("[sp_W2_PaymentReceipt_Master_Delete]", PmtRecMaster_param, Trans))
                            {
                                SqlParameter[] PmtRecDetail_param = {   /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    /*1*/ new SqlParameter("@ReceiptNo",SqlDbType.Char,8), };

                                PmtRecDetail_param[0].Value = Session["DealerCode"].ToString();
                                PmtRecDetail_param[1].Value = gvlReceiptNo.Text.Trim();

                                DataTable dt = new DataTable();

                                dt = (DataTable)ViewState["DtUnPaid"];
                                for(int i=0;i<dt.Rows.Count;i++)
                                {
                                    myFunc.ExecuteSP_NonQuery("[sp_W2_PaymentReceiptDetail_Delete]", PmtRecDetail_param, Trans);
                                    string IQuery = "Update dbo.CustomerInvoice set PaidC=PaidC - '" + dt.Rows[i]["Adjustment"].ToString() + "' " +
                                          "Where DealerCode='" + Session["DealerCode"].ToString() + "' and InvoiceNo='" + dt.Rows[i]["Ref No"].ToString() + "'";
                                    myFunc.ExecuteQuery(IQuery, Trans);
                                }

                                SqlParameter[] PmtRecTaxDetail_param = { /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    /*1*/ new SqlParameter("@ReceiptNo",SqlDbType.Char,8)};

                                PmtRecTaxDetail_param[0].Value = Session["DealerCode"].ToString();
                                PmtRecTaxDetail_param[1].Value = gvlReceiptNo.Text;
                                myFunc.ExecuteSP_NonQuery("[sp_W2_PaymentReceiptTaxDetail_Delete]", PmtRecTaxDetail_param, Trans);

                            }
                            else 
                            {
                                lblMSG.Text = "Deletion faild";
                                PopupControlMSG.ShowOnPageLoad = true;
                                
                            }
                            ObjTrans.CommittTransaction(ref Trans);

                            lblMSG.Text = "Record has been deleted";
                            PopupControlMSG.ShowOnPageLoad = true;
                            btnClear_Click(btnClear, EventArgs.Empty);
                        }
                    }
                    catch (Exception ex) { ObjTrans.RollBackTransaction(ref Trans); throw ex; }
                }
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {



            ReportDocument rpt, crReportDocument;
            string strCriteria, rptTitle;
            DateTime FromDate, ToDate;
            SqlDataReader rder;
            rpt = new ReportDocument();
            String StrReportID;

            rpt.Load(Server.MapPath("~/Modules/Service/ServiceReports/rptPaymentReceiptPrint.rpt"));

            strCriteria = "{PaymentReceiptMaster.ReceiptNo}='" + gvlReceiptNo.Text + "' " +
                           "AND {PaymentReceiptMaster.DealerCode}='" + Session["DealerCode"].ToString() + "' " +
                           "AND {PaymentReceiptMaster.DelFlag} = 'N' ";

            rpt.RecordSelectionFormula = strCriteria;

            rpt.DataDefinition.FormulaFields["DealerName"].Text = "'" + myFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "DealerDesc", "Dealer") + "'";
            rpt.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + myFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Address1", "Dealer") + "" +
                                                                           myFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Address2", "Dealer") + "" +
                                                                           myFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Address3", "Dealer") + " (" +
                                                                           myFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Phone1", "Dealer") + ") '";

            rpt.DataDefinition.FormulaFields["ReportTitle"].Text = "'Payment Receipt'";
            rpt.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
            rpt.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            rpt.DataDefinition.FormulaFields["NTN"].Text = "'" + myFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "NTN", "Dealer") + "'";
            rpt.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'" + myFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "SaleTaxNo", "Dealer") + "'";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + grl.GetStringValuesAgainstCodes("CusCode", SessionInformation.LoginID, "CellNo", "Customer") + "'";

            if (Environment.MachineName == "AZHARDELL")
            {
                rpt.SetDatabaseLogon("sa", "100372", ".", "BMS");
            }
            else
            {
                rpt.SetDatabaseLogon("sa", "", ".", "BMS");
            }

            Session["RDService"] = rpt;
            // CrystalReportViewer1.ReportSource = rpt;

            crReportDocument = new ReportDocument();
            crReportDocument = (ReportDocument)Session["RDService"];
            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "PaymentReceiptReport" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;


            //crReportDocument.SetDatabaseLogon("SDMS", "sdms161", "192.168.1.47", "SDMS");
            if (Environment.MachineName == "AZHARDELL")
            {
                crReportDocument.SetDatabaseLogon("sa", "100372", "AZHARDELL", "BMS", true);
            }
            else
            {
                crReportDocument.SetDatabaseLogon("sa", "", ".", "BMS", true);
            }

            //txtRemarks.Text = File;
            crReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            string URL;
            URL =  "../../Download/OpenPdf.aspx?FileName=" + FileName;
            
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

            
            //string URL = "../../Modules/Service/ServiceReports/rptViewerService.aspx?ReportID=PaymentReceiptReport&ReceiptNo=" + gvlReceiptNo.Text.Trim();
            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }

        protected void gvPendingInvoice_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {


            
            e.Cancel = true ;
            if (gvPendingInvoice.FocusedRowIndex < 0)
            {
                lblMSG.Text = "Please select an invoice ";
                PopupControlMSG.ShowOnPageLoad = true;

                return;
            }
            DataTable dt = (DataTable)ViewState["DtUnPaid"];

            if (dt.Rows.Count == 1)
            {
                //lblMSG.Text = "One invoice must be in there ";
                //PopupControlMSG.ShowOnPageLoad = true;
                return;
            }

            int ridx = (int)(gvPendingInvoice.FocusedRowIndex);

            dt.Rows.Remove(dt.Rows[ridx]);
            dt.AcceptChanges();

            ViewState["DtUnPaid"] = dt;
            UpdatePendInvGrid();
        }

        protected void btnLoadAllPendingInv_Click(object sender, EventArgs e)
        {
            LoadMasterData();
        }

        protected void btnAddInvToGrid_Click(object sender, EventArgs e)
        {
            DataTable sinv = new DataTable();
            DataTable pinv = new DataTable();
            string finv = "";
            Session["PaymentStatus"] = "UNPAID";
            sinv = myFunc.PendingPaymentReceipt(rbPaymentReceiptType.Value.ToString(), gvlCustomerCode.Text, "UNPAID", gvlReceiptNo.Text, txtInvNo.Text, Session["DealerCode"].ToString());

            //search in loaded table
            pinv = (DataTable) ViewState["DtUnPaid"];
            if (pinv != null)
            {
                for (int i = 0; i < pinv.Rows.Count; i++)
                {
                    if (pinv.Rows[i]["Ref No"].ToString().Trim() == txtSrchInvNo.Text.Trim())
                    {
                        lblMSG.Text = txtSrchInvNo.Text + " already exist in grid";
                        PopupControlMSG.ShowOnPageLoad = true;
                        return;
                    }
                }
            }

            // search in pending inv
            
            for (int i = 0; i < sinv.Rows.Count; i++)
            {
                if (sinv.Rows[i]["Ref No"].ToString().Trim() == txtSrchInvNo.Text.Trim())
                {
                    finv = i.ToString();
                }
            }
            if (finv == "")
            {
                lblMSG.Text = txtSrchInvNo.Text + " Invoice not found in pending invoices";
                PopupControlMSG.ShowOnPageLoad = true;
                return;
            }

            int f = Convert.ToInt16(finv);

            //if (pinv == null)
            //{
            //    //DataTable pinv = new DataTable();
            //    pinv.Columns.Add("S NO", typeof(int));
            //    pinv.Columns.Add("Ref No", typeof(string));
            //    pinv.Columns.Add("Ref Date", typeof(DateTime));
            //    pinv.Columns.Add("OutStanding", typeof(string));
            //    pinv.Columns.Add("Adjustment", typeof(string));
            //    pinv.Columns.Add("Remaining", typeof(decimal));
            //    pinv.Columns.Add("CheckVal", typeof(Boolean));
            //}


            DataRow row = pinv.NewRow();
            row["S NO"] = pinv.Rows.Count + 1;
            row["Ref No"] = sinv.Rows[f]["Ref No"].ToString();
            row["Ref Date"] =myFunc.SaveDate( sinv.Rows[f]["Ref Date"].ToString());
            row["Ref Amount"] = sinv.Rows[f]["Ref Amount"].ToString();
            row["OutStanding"] = sinv.Rows[f]["OutStanding"].ToString();
            row["Adjustment"] = sinv.Rows[f]["Adjustment"].ToString();
            row["Remaining"] = sinv.Rows[f]["Remaining"].ToString();
            row["CheckVal"] =Convert.ToBoolean( sinv.Rows[f]["CheckVal"].ToString());
            pinv.Rows.Add(row);


            ViewState["DtUnPaid"] = pinv;

            UpdatePendInvGrid();

            gvPendingInvoice.DataSource = (DataTable)ViewState["DtUnPaid"];
            gvPendingInvoice.DataBind();
            gvPendingInvoice.KeyFieldName = "Ref No";
            if (gvPendingInvoice.Columns.Count > 0)
            {
                if (gvPendingInvoice.Columns["CheckVal"] != null)
                {
                    gvPendingInvoice.Columns["CheckVal"].Visible = false;
                }
            }

            txtSrchInvNo.Text = "";
        }

        protected void rbPaymentReceiptType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pt = rbPaymentReceiptType.SelectedItem.Value.ToString();
            string sql = "";
            if (pt == "CounterS")
            {
                sql = "Select b.CusCode [Customer Code],b.CusDesc Customer,a.SaleInvNo [Ref No],ROUND(a.InvoiceAmount,0) - ROUND(a.TotReceipt,0) as 'Balance' ";
                sql += "from CounterSaleMaster a,Customer b ";
                sql += " WHERE a.DelFlag = 'N' ";
                sql += " AND a.DealerCode = b.DealerCode ";
                sql += " AND a.CusCode = b.CusCode ";
                sql += " AND ROUND(a.InvoiceAmount,0) > ROUND(a.TotReceipt,0) ";
                sql += "AND PostFlag = 'Y' AND A.InvoiceAmount - A.TotReceipt > 1";
            }
            //if (pt == "Service")
            else
            {
                sql = "Select CODEFIELD [Customer Code],CUSNAME Customer,INVNO [Ref No],Round(BALANCE,0) AS Balance FROM PendingServiceInvoices a";
	            sql += " where a.DealerCode in ('AAAAA','"+Session["DealerCode"].ToString()+"') ";
                sql += " AND LEN(CodeField) = 8 And Round(BALANCE,0)<>0.00 Order by CODEFIELD";
            }

            SqlDataSourceCustomerList.SelectCommand = sql;

            SqlDataSourceCustomerList.Select(DataSourceSelectArguments.Empty);
            gvlCustomerCode.DataSource = SqlDataSourceCustomerList;
            gvlCustomerCode.DataSourceID = "";
            gvlCustomerCode.DataBind();
            //SqlDataSourceAllStories.SelectCommand = sql;
            //SqlDataSourceAllStories.Select(DataSourceSelectArguments.Empty);
            //gvTodayStories.DataSource = SqlDataSourceAllStories;
            //gvTodayStories.DataSourceID = "";
            //gvTodayStories.DataBind();

        }
    }
}