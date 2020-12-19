using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

using System.IO;
using PdfSharp.Pdf;

namespace DXBMS.Modules.Service.Forms
{
    public partial class CustomerPaymentRec : System.Web.UI.Page
    {
        DataTable TaxDetailDT, dtPendingInvoice;
        DataSet ds, dsReceiptMaster, dsReceiptTaxDetail;
        clsLookUp clslook = new clsLookUp();
        Transaction ObjTrans = new Transaction();
        //General grl = new General();
        SqlTransaction Trans;
        MainBLL objMBLL = new MainBLL();
        SysFunctions SysFuncs = new SysFunctions();
        SecurityBll sec = new SecurityBll();
        SysFunction SysFunc = new SysFunction();
        bool search_result;
        int countInvoices;
        double totInvoice, totOutstanding, totTaxAmt, totAdjAmt, totInsAmt,TotRefAmt,totAdvAmount;
        double RunningTotal = 0.00;
        double SumOfSelectedInvoice = 0.00,Count=0.00;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2559", "001"))
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
                txtInstDate.Text = txtReceiptDate.Text = System.DateTime.Now.Date.ToString("dd-MM-yyyy");
                createTaxDetail();
                ds = new DataSet(); //grl = new General();
                search_result = SysFuncs.CodeExists("PaymentMode", "", ref ds);
                DDLPaymentMode.DataSource = ds.Tables[0].DefaultView; 
                DDLPaymentMode.DataTextField = "PayModeDesc"; 
                DDLPaymentMode.DataValueField = "payModeCode"; 
                DDLPaymentMode.DataBind();
                DDLPaymentMode.SelectedValue = "C";
                objMBLL.FillDrp_SP(ddlbank, "sp_2W_Select_Bank", "BankCode", "BankDesc", null, false, "--Select--", false, "");
                
                DataTable TaxDetailDT = (DataTable)ViewState["TaxDetail"];
                //if (TaxDetailDT.Rows.Count == 0) TaxDetailDT.Rows.Add(TaxDetailDT.NewRow());
                //gvTaxDetail.DataSource = TaxDetailDT; gvTaxDetail.DataBind();
                objMBLL.FillDrp_SP(ddlInsCo, "sp_InsuranceCompanies_select", "InsCompCode", "InsCompDescription", null, true, "--Select--", false, "");

                //lblInsComp.Visible = false;
                //lblInsBr.Visible = false;
                //ddlInsBranch.Visible = false;
                //ddlInsCo.Visible = false;
                createPartsDT();
                

            }
            Session["LookUpData"] = null;
            //if (Session["LookUpData"] != null)
            //{
            //    //lookUptext(ViewState["lookupid"].ToString());
            //    if (ViewState["lookupid"].ToString() == "56" | ViewState["lookupid"].ToString() == "55" | ViewState["lookupid"].ToString() == "74" | ViewState["lookupid"].ToString() == "75")
            //        LoadMasterData();
            //}
        }
        public void SelectedPartDetail(string item)
        {
            Load_ReceiptNo();
            Load_Customer();
            if (ViewState["lookupid"].ToString() == "95")
            {
                ddlCust.SelectedValue = item;
                ddlCust_SelectedIndexChanged(null,null);
                   
            }
           else if (ViewState["lookupid"].ToString() == "1")
            {
                ddlCust.SelectedValue = item;
                ddlCust_SelectedIndexChanged(null, null);

            }
            else
            {
                
                ddlReceptNo.SelectedValue = item;
                ddlReceptNo_SelectedIndexChanged(null, null);
            }
        }
        private void createPartsDT()
        {
            DataSet dsGVMain = new DataSet();
            dsGVMain.Tables.Add();

            dsGVMain.Tables[0].Columns.Add(new DataColumn("S NO", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("RefNo", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("JobCardCode", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("Ref Date", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("Ref Amount", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("OutStanding", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("Adjustment", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("Remaining", typeof(string)));

            //DataRow dr = dsGVMain.Tables[0].NewRow();
            //dsGVMain.Tables[0].Rows.Add(dr);

            gvPendingInvoice.DataSource = dsGVMain.Tables[0];
            gvPendingInvoice.DataBind();
            //Session["dsParts"] = dsGVMain;
        }
        private void fillData()
        {
            //txtCustomerName.Text = .GetStringValuesAgainstCodes("CusCode", txtCustomerCode.Text, "CusDesc", "Customer");
            SqlParameter[] dsTaxDetailPram = { new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                           new SqlParameter("@ReceiptNo",SqlDbType.Char,8) };
            dsTaxDetailPram[0].Value = Session["DealerCode"].ToString();
            dsTaxDetailPram[1].Value = (ddlReceptNo.SelectedValue.ToString().Trim() == "" ? null : ddlReceptNo.SelectedValue.ToString().Trim());
            dsReceiptTaxDetail = new DataSet(); 
            dsReceiptTaxDetail = SysFuncs.FillDataSet("sp_W2_PaymentReceiptTaxDetail_Select", dsTaxDetailPram);
            if (dsReceiptTaxDetail.Tables[0].Rows.Count == 0) dsReceiptTaxDetail.Tables[0].Rows.Add(dsReceiptTaxDetail.Tables[0].NewRow());
            ViewState["TaxDetail"] = dsReceiptTaxDetail.Tables[0];
            gvTaxDetail.DataSource = dsReceiptTaxDetail; gvTaxDetail.DataBind();
        }

        private void LoadMasterData(string InvType = "C")
        {
            //FillInvoiceDetailGrid();
            
            SqlParameter[] dsMasterPram = { new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                        new SqlParameter("@ReceiptNo",SqlDbType.Char,8), };
            dsMasterPram[0].Value = Session["DealerCode"].ToString();
            dsMasterPram[1].Value = (ddlReceptNo.SelectedValue.ToString().Trim() == "" ? null : ddlReceptNo.SelectedValue.ToString().Trim());
            dsReceiptMaster = new DataSet();
            dsReceiptMaster = SysFuncs.FillDataSet("sp_W2_PaymentReceiptMaster_Select", dsMasterPram);
            if (dsReceiptMaster.Tables[0].Rows.Count > 0)
            {
                fillData();
                Label lblTotalAmount = (Label)gvTaxDetail.FooterRow.FindControl("lblTotalAmount");
                ddlReceptNo.SelectedValue = dsReceiptMaster.Tables[0].Rows[0]["ReceiptNo"].ToString();
                ddlCust.Visible = false;
                txtCust.Visible = true;
                txtCusDesc.Visible = true;
                txtCust.Text= dsReceiptMaster.Tables[0].Rows[0]["CusCode"].ToString();
                txtCusDesc.Text=SysFuncs.GetStringValuesAgainstCodes("CusCode", txtCust.Text.ToString().Trim(), "CusDesc", "Customer", "", Session["DealerCode"].ToString());
                ddlCust.SelectedValue = dsReceiptMaster.Tables[0].Rows[0]["CusCode"].ToString();
                txtReceiptDate.Text = dsReceiptMaster.Tables[0].Rows[0]["ReceiptDate"].ToString();
                txtInvTotal.Text= dsReceiptMaster.Tables[0].Rows[0]["OutSTTotal"].ToString();
                txtVoucherNo.Text= dsReceiptMaster.Tables[0].Rows[0]["VoucherNo"].ToString();
                ddlTransType.SelectedValue = dsReceiptMaster.Tables[0].Rows[0]["TransType"].ToString();
                ddlTransType_SelectedIndexChanged(null,null);
                ddlReceptNo.SelectedValue = dsReceiptMaster.Tables[0].Rows[0]["ReceiptNo"].ToString();
                ddlPaymentReceiptType.SelectedValue = dsReceiptMaster.Tables[0].Rows[0]["InvoiceType"].ToString();
                txtRemarks.Text = dsReceiptMaster.Tables[0].Rows[0]["Remarks"].ToString();
               
                DDLPaymentMode.SelectedValue = dsReceiptMaster.Tables[0].Rows[0]["PayModeCode"].ToString();
                txtInstNo.Text = dsReceiptMaster.Tables[0].Rows[0]["InsNo"].ToString();
                txtInstDate.Text = dsReceiptMaster.Tables[0].Rows[0]["InsDate"].ToString();
                txtInstAmt.Text = dsReceiptMaster.Tables[0].Rows[0]["AmountPaid"].ToString();
                ddlbank.SelectedValue = dsReceiptMaster.Tables[0].Rows[0]["BankCode"].ToString().Trim();
                txtBranch.Text = dsReceiptMaster.Tables[0].Rows[0]["Branch"].ToString();
                //txttotInv.Text = dsReceiptMaster.Tables[0].Rows[0]["InvTotal"].ToString();
                txtDocNo.Text = dsReceiptMaster.Tables[0].Rows[0]["DocumentNo"].ToString();
                txttotAdj.Text = dsReceiptMaster.Tables[0].Rows[0]["InvAdjTotal"].ToString();

                string chkISADV = dsReceiptMaster.Tables[0].Rows[0]["IsAdjustAdvance"].ToString();
                if (chkISADV=="Y") {
                    chkAdvance_CheckedChanged(null, null);
                  //  ddlAdvance.SelectedValue= dsReceiptMaster.Tables[0].Rows[0]["AdvanceReceiptNo"].ToString();
                }
                txtAdvAmount.Text = dsReceiptMaster.Tables[0].Rows[0]["AdvanceAdjustedAmount"].ToString();
                if (dsReceiptMaster.Tables[0].Rows[0]["InvoiceType"].ToString() == "Insurance")
                {
                   // chkInsRec.Checked = true;
                    lblInsComp.Visible = true;
                    lblInsBr.Visible = true;
                    ddlInsBranch.Visible = true;
                    ddlInsCo.Visible = true;
                    objMBLL.FillDrp_SP(ddlInsCo, "sp_InsuranceCompanies_select", "InsCompCode", "InsCompDescription", null, true, "--Select--", false, "");
                    ddlInsCo.SelectedValue = dsReceiptMaster.Tables[0].Rows[0]["InsCompCode"].ToString();
                    ddlInsCo_SelectedIndexChanged(null, null);
                    ddlInsBranch.SelectedValue = dsReceiptMaster.Tables[0].Rows[0]["BranchCode"].ToString().Trim();

                }
                if (dsReceiptMaster.Tables[0].Rows[0]["ISAdjustAdvance"].ToString() == "Y")
                {
                    chkAdvance.Checked = true;
                }
                Session["PaymentStatus"] = "PAID";
                dtPendingInvoice = SysFuncs.PendingPaymentReceipt(ddlPaymentReceiptType.SelectedValue.ToString(), ddlCust.SelectedValue.ToString(), "PAID", ddlReceptNo.SelectedValue.ToString().Trim(), txtInvNo.Text, Session["DealerCode"].ToString());
                //dtPendingInvoice = SysFuncs.PendingPaymentReceipt(Session["DealerCode"].ToString(), ddlPaymentReceiptType.SelectedValue.ToString(), ddlCust.SelectedValue.ToString(), "PAID", ddlReceptNo.SelectedValue.ToString().Trim(), txtInvNo.Text);
                gvPendingInvoice.DataSource = dtPendingInvoice;
                gvPendingInvoice.DataBind();
                Session["dtPendingInvoice"] = dtPendingInvoice;
                if (chkAdvance.Checked & dsReceiptMaster.Tables[0].Rows[0]["TransType"].ToString()!="Advance")
                {
                    txttotAmount.Text = (double.Parse(txtInstAmt.Text.Trim() == "" ? "0" : txtInstAmt.Text.Trim())
                                    + double.Parse(lblTotalAmount.Text.Trim() == "" ? "0" : lblTotalAmount.Text.Trim())
                                    +double.Parse(txtAdvAmount.Text.Trim() == "" ? "0" : txtAdvAmount.Text.Trim())).ToString();
                }
                else
                {
                    txttotAmount.Text = (double.Parse(txtInstAmt.Text.Trim() == "" ? "0" : txtInstAmt.Text.Trim())
                                    + double.Parse(lblTotalAmount.Text.Trim() == "" ? "0" : lblTotalAmount.Text.Trim())).ToString();
                }
                
                ViewState["DtUnPaid"] = dtPendingInvoice;
            }
            else
            {
                fillData();
                Session["PaymentStatus"] = "UNPAID";
                if (InvType == "I")
                {
                    dtPendingInvoice = SysFuncs.PendingPaymentReceipt(Session["DealerCode"].ToString(), ddlPaymentReceiptType.SelectedValue.ToString(), ddlCust.SelectedValue.ToString(), "UNPAID", ddlReceptNo.SelectedValue.ToString().Trim(), txtInvNo.Text, "I", ddlInsCo.SelectedValue.ToString(), ddlInsBranch.SelectedValue.ToString());
                }
                else
                {
                    //dtPendingInvoice = SysFuncs.PendingPaymentReceipt(Session["DealerCode"].ToString(), ddlPaymentReceiptType.SelectedValue.ToString(), ddlCust.SelectedValue.ToString(), "UNPAID", txtReceiptNo.Text, txtInvNo.Text);
                    dtPendingInvoice = SysFuncs.PendingPaymentReceipt(ddlPaymentReceiptType.SelectedValue.ToString(), ddlCust.SelectedValue.ToString(), "UNPAID", ddlReceptNo.SelectedValue.ToString().Trim(), txtInvNo.Text, Session["DealerCode"].ToString());
                }
                ViewState["DtUnPaid"] = dtPendingInvoice;
                gvPendingInvoice.DataSource = dtPendingInvoice;
                gvPendingInvoice.DataBind();
            }
        }
        //void lookUptext(string lookUpID)
        //{
        //    switch (lookUpID)
        //    {
        //        case "55":
        //            GetLookUpsValue(ref txtCustomerCode, ref txtCustomerName, ref txtInvNo, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ViewState["ixd3"].ToString());
        //            break;
        //        case "74":
        //            GetLookUpsValue(ref txtCustomerCode, ref txtCustomerName, ref txtInvNo, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ViewState["ixd3"].ToString());
        //            break;
        //        case "75":
        //            GetLookUpsValue(ref txtCustomerCode, ref txtCustomerName, ref txtInvNo, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ViewState["ixd3"].ToString());
        //            break;
        //        case "56":
        //            GetLookUpsValue(ref txtReceiptNo, ref txtReceiptDate, ref txtCustomerCode, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ViewState["ixd3"].ToString());
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
        //void GetLookUpsValue(ref TextBox txt1, ref TextBox txt2, ref TextBox txt3, string ixd1, string ixd2, string ixd3)
        //{
        //    if (Session["LookUpData"] != null)
        //    {
        //        GridViewRow Drow = (GridViewRow)Session["LookUpData"];
        //        txt1.Text = (txt1 != null ? Drow.Cells[Convert.ToInt32(ixd1)].Text : "");
        //        txt2.Text = (txt2 != null ? Drow.Cells[Convert.ToInt32(ixd2)].Text : "");
        //        txt3.Text = (txt3 != null ? Drow.Cells[Convert.ToInt32(ixd3)].Text : "");
        //        Session["LookUpData"] = null;
        //    }
        //}
        private void createTaxDetail()
        {
            TaxDetailDT = new DataTable();
            TaxDetailDT.Columns.Add(new DataColumn("ReceiptHead", typeof(string)));
            TaxDetailDT.Columns.Add(new DataColumn("TaxPerc", typeof(string)));
            TaxDetailDT.Columns.Add(new DataColumn("Amount", typeof(string)));
            TaxDetailDT.Columns.Add(new DataColumn("TaxID", typeof(string)));
            if (TaxDetailDT.Rows.Count == 0) TaxDetailDT.Rows.Add(TaxDetailDT.NewRow());
            gvTaxDetail.DataSource = TaxDetailDT;
            gvTaxDetail.DataBind();
            ViewState["TaxDetail"] = TaxDetailDT;
        }
        //protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        string sQuery = string.Empty;
        //        string strWhereClause = string.Empty;

        //        sQuery = "Select Distinct ReceiptNo,dbo.getNewDate(ReceiptDate) As ReceiptDate,a.CusCode,CusDesc from PaymentReceiptMaster a,Customer b ";

        //        strWhereClause = " AND a.DelFlag = 'N'";
        //        strWhereClause += " AND a.DealerCode = b.DealerCode";
        //        strWhereClause += " AND a.CusCode = b.CusCode";

        //        ViewState["lookupid"] = 56;
        //        ViewState["ixd1"] = 1;
        //        ViewState["ixd2"] = 2;
        //        ViewState["ixd3"] = 3;
        //        ViewState["txtgroup"] = 1;

        //        clslook.LU_Get_ProdCode(ImageButton2, ViewState["lookupid"].ToString(), strWhereClause, sQuery, "../../../", "Order by ReceiptNo Desc");

        //    }
        //    catch (Exception ex) { throw ex; }
        //}
        //protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        string sQuery = string.Empty;
        //        string strWhereClause = string.Empty;
        //        string AppendOrderby = string.Empty;

        //        if (ddlPaymentReceiptType.SelectedItem.Value == "Service")
        //        {
        //            sQuery = "Select CODEFIELD [Customer Code],CUSNAME Customer,INVNO [Invoice No],Round(BALANCE,0) AS Balance FROM PendingServiceInvoices a";

        //            strWhereClause = " AND LEN(CodeField) = 8 And Round(BALANCE,0)<>0.00 ";
        //            AppendOrderby = "Order by CODEFIELD Desc";
        //            ViewState["lookupid"] = 55;
        //            ViewState["ixd1"] = 1;
        //            ViewState["ixd2"] = 2;
        //            ViewState["ixd3"] = 3;
        //            ViewState["txtgroup"] = 2;
        //        }
        //        else if (ddlPaymentReceiptType.SelectedItem.Value == "CounterSale")
        //        {
        //            sQuery = "Select b.CusCode [Customer Code],b.CusDesc Customer,a.SaleInvNo [Ref No],ROUND(a.InvoiceAmount,0) - ROUND(a.TotReceipt,0) as 'Balance' from CounterSaleMaster a,Customer b";

        //            strWhereClause = " AND a.DelFlag = 'N'";
        //            strWhereClause += " AND a.DealerCode = b.DealerCode";
        //            strWhereClause += " AND a.CusCode = b.CusCode";
        //            strWhereClause += " AND ROUND(a.InvoiceAmount,0) > ROUND(a.TotReceipt,0)";
        //            strWhereClause += " AND PostFlag = 'Y' AND A.InvoiceAmount - A.TotReceipt > 1 ";
        //            AppendOrderby = "Order by b.CusCode Desc";
        //            ViewState["lookupid"] = 74;
        //            ViewState["ixd1"] = 1;
        //            ViewState["ixd2"] = 2;
        //            ViewState["ixd3"] = 3;
        //            ViewState["txtgroup"] = 2;

        //        }
        //        else if (ddlPaymentReceiptType.SelectedItem.Value == "Sales")
        //        {
        //            sQuery = "Select b.CusCode [Customer Code],b.CusDesc Customer,a.ExpendCode [Ref No],ROUND(a.TotalCost,0) - ROUND(a.TotReceipt,0) as 'Balance' from ExpenditureMaster a,Customer b ";
        //            strWhereClause = "And  a.DelFlag = 'N' ";
        //            strWhereClause += "AND a.DealerCode = b.DealerCode ";
        //            strWhereClause += "AND a.CusCode = b.CusCode ";
        //            strWhereClause += "AND ROUND(a.TotalCost,0) > ROUND(a.TotReceipt,0) ";
        //            strWhereClause += "AND A.TotalCost - A.TotReceipt > 1";
        //            AppendOrderby = "Order by a.ExpendCode Desc";
        //            ViewState["lookupid"] = 75;
        //            ViewState["ixd1"] = 1;
        //            ViewState["ixd2"] = 2;
        //            ViewState["ixd3"] = 3;
        //            ViewState["txtgroup"] = 2;
        //        }
        //        clslook.LU_Get_ProdCode(ImageButton1, ViewState["lookupid"].ToString(), strWhereClause, sQuery, "../../../", AppendOrderby);
        //    }
        //    catch (Exception ex) { throw ex; }
        //}
        protected void gvPendingInvoice_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Width = 100;
            e.Row.Cells[1].Width = 60; e.Row.Cells[2].Width = 100;
            e.Row.Cells[3].Width = 100;
            e.Row.Cells[4].Width = e.Row.Cells[5].Width = e.Row.Cells[6].Width = e.Row.Cells[7].Width = 100;
            e.Row.Cells[4].HorizontalAlign = e.Row.Cells[5].HorizontalAlign = e.Row.Cells[6].HorizontalAlign = e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
        }

        protected void gvPendingInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                countInvoices = countInvoices + 1;
                e.Row.Cells[1].Text = countInvoices.ToString();
                TotRefAmt = TotRefAmt + double.Parse(e.Row.Cells[5].Text.Trim().Replace("&nbsp;", "") == "" ? "0" : e.Row.Cells[5].Text.Trim());
                totOutstanding = totOutstanding + double.Parse(e.Row.Cells[6].Text.Trim().Replace("&nbsp;", "") == "" ? "0" : e.Row.Cells[6].Text.Trim());
                totAdjAmt = totAdjAmt + double.Parse(e.Row.Cells[7].Text.Trim().Replace("&nbsp;", "") == "" ? "0" : e.Row.Cells[7].Text.Trim());

            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                countInvoices = 0;
                totInvoice = totOutstanding = totAdjAmt = 0;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = "Total";
                e.Row.Cells[5].Text = TotRefAmt.ToString();
                e.Row.Cells[6].Text = totOutstanding.ToString();
                e.Row.Cells[7].Text = totAdjAmt.ToString();
                e.Row.ForeColor = Color.White;
                e.Row.BackColor.Equals("#055a83");
            }
        }

        protected void gvTaxDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblAmount=(Label)e.Row.FindControl("lblAmount");
                totTaxAmt = totTaxAmt + double.Parse(lblAmount.Text.Trim().Replace("&nbsp;", "") == "" ? "0" : lblAmount.Text.Trim());
            }
            //else if (e.Row.RowType == DataControlRowType.Header) totTaxAmt = 0;
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                
                
                Label lblTotalAmount = (Label)e.Row.FindControl("lblTotalAmount");
                ImageButton BtnAdd = (ImageButton)e.Row.FindControl("BtnAdd");

                 if(ddlCust.SelectedIndex <= 0)
                {
                    BtnAdd.Enabled = false;
                }else
                {
                    BtnAdd.Enabled = true;
                }
                //txtTotTax.Text = totTaxAmt.ToString();
                lblTotalAmount.Text = totTaxAmt.ToString();
                //txttotAmount.Text = (totTaxAmt + double.Parse(txtInstAmt.Text.Trim().Replace("&nbsp;", "") == "" ? "0" : txtInstAmt.Text.Trim())).ToString();
                DropDownList ddlTaxType = (DropDownList)e.Row.FindControl("DDLFooterReceiptHead");
                LoadTaxType(ddlTaxType);
               
            }
           // CalSubTotal();
        }

        //protected void gvTaxDetail_RowCreated(object sender, GridViewRowEventArgs e)
        //{
        //    //e.Row.Cells[0].Width = 60;
        //    e.Row.Cells[0].Text = "S No"; e.Row.Cells[1].Width = 60;
        //    e.Row.Cells[1].Text = "Receipt Head"; e.Row.Cells[2].Width = 180;
        //    e.Row.Cells[2].Text = "Amount"; 
        //    e.Row.Cells[2].Width = 100; e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;

        //    if (e.Row.RowType == DataControlRowType.Header)
        //    {
        //        e.Row.Cells[0].Text = "S No";
        //        e.Row.Cells[1].Text = "Receipt Head"; 
        //        e.Row.Cells[2].Text = "Amount";
        //    }

        //}
        //protected void btnAdd_Click(object sender, EventArgs e)
        //{
        //    TextBox[] textBoxes = { txtTaxAmt };
        //    if (!MasterValidation(textBoxes)) return;
        //    else
        //    {
        //        TaxDetailDT = (DataTable)ViewState["TaxDetail"];
        //        search_result = false;
        //        foreach (DataRow rowTaxDetailDT in TaxDetailDT.Rows)
        //        {
        //            if (rowTaxDetailDT["ReceiptHead"].ToString().Trim() == ""
        //                | rowTaxDetailDT["ReceiptHead"].ToString().Trim() == DDLReceiptHead.SelectedItem.Text.Trim())
        //            {
        //                Insert_in_TaxDetailDT(rowTaxDetailDT);
        //                search_result = true;
        //            }
        //        }
        //        if (search_result == false)
        //        {
        //            DataRow rowTaxDetailDT = TaxDetailDT.NewRow();
        //            Insert_in_TaxDetailDT(rowTaxDetailDT);
        //            TaxDetailDT.Rows.Add(rowTaxDetailDT);
        //        }

        //        ViewState["Parts"] = TaxDetailDT; gvTaxDetail.DataSource = TaxDetailDT; gvTaxDetail.DataBind();
        //        txttotAmount.Text = (double.Parse(txtInstAmt.Text == "" ? "0" : txtInstAmt.Text)
        //                           + double.Parse(txtTotTax.Text == "" ? "0" : txtTotTax.Text)).ToString();
        //        txtAdj.Value = "f";
        //    }
        //}


        private void Insert_in_TaxDetailDT(DataRow rowTaxDetailDT)
        {
            //rowTaxDetailDT["SNO"] = TaxDetailDT.Rows.Count + 1;
            //foreach (GridViewRow gvr in gvPendingInvoice.Rows)
            //{

            //}
            TextBox txtFooterTaxAmt = (TextBox)gvTaxDetail.FooterRow.FindControl("txtFooterTaxAmt");
            DropDownList DDLFooterReceiptHead = (DropDownList)gvTaxDetail.FooterRow.FindControl("DDLFooterReceiptHead");
            TextBox txtFooterTaxPerc = (TextBox)gvTaxDetail.FooterRow.FindControl("txtFooterTaxPerc");
            rowTaxDetailDT["ReceiptHead"] = DDLFooterReceiptHead.SelectedItem.Text;
            rowTaxDetailDT["TaxPerc"] = txtFooterTaxPerc.Text.Trim();
            rowTaxDetailDT["Amount"] = txtFooterTaxAmt.Text.Trim();
            rowTaxDetailDT["TaxID"] = DDLFooterReceiptHead.SelectedValue.ToString();
        }

        [System.Web.Services.WebMethod]
        public bool MasterValidation(TextBox[] textBoxes)
        {
            bool isValid = true;

            for (int i = 0; i < textBoxes.Length; i++)
            {
                if (textBoxes[i].Text.Trim() == "")
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

        //protected void btnClear1_Click(object sender, EventArgs e)
        //{
        //    ClearTaxDetailTextBoxes();
        //}
        private void ClearTaxDetailTextBoxes()
        {
            TextBox txtFooterTaxAmt = (TextBox)gvTaxDetail.FooterRow.FindControl("txtFooterTaxAmt");
            //DropDownList DDLFooterReceiptHead = (DropDownList)gvTaxDetail.FooterRow.FindControl("DDLFooterReceiptHead");
            TextBox[] txts = { txtFooterTaxAmt };
            ClearTextBoxes(txts);
        }
        public void ClearTextBoxes(TextBox[] textBoxes)
        {
            for (int i = 0; i < textBoxes.Length; i++)
            { textBoxes[i].Text = string.Empty; }
        }
        //protected void btnRemove_Click(object sender, EventArgs e)
        //{
        //try
        //{
        //    int ridx = (int)(ViewState["ridx"]);
        //    TaxDetailDT = (DataTable)ViewState["TaxDetail"];
        //    TaxDetailDT.Rows.Remove(TaxDetailDT.Rows[ridx]);
        //    TaxDetailDT.AcceptChanges();
        //    ClearTaxDetailTextBoxes();
        //    if (TaxDetailDT.Rows.Count == 0) TaxDetailDT.Rows.Add(TaxDetailDT.NewRow());
        //    ViewState["TaxDetail"] = TaxDetailDT; gvTaxDetail.DataSource = TaxDetailDT; gvTaxDetail.DataBind();
        //}
        //catch { return; }

        //}
        //protected void gvTaxDetail_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ViewState["ridx"] = gvTaxDetail.SelectedRow.RowIndex;
        //        GridViewRow row = gvTaxDetail.Rows[gvTaxDetail.SelectedRow.RowIndex];
        //        DDLReceiptHead.SelectedValue = row.Cells[2].Text.Trim();
        //        txtTaxAmt.Text = row.Cells[3].Text.Trim();
        //    }
        //    catch (Exception ex) { throw ex; }

        //}
        protected void btnAutoAdj_Click(object sender, EventArgs e)
        {
            Label lblTotalAmount = (Label)gvTaxDetail.FooterRow.FindControl("lblTotalAmount");
            if (ddlReceptNo.SelectedIndex != 0)
            {
                SysFuncs.UserMsg(lblMsg, Color.Red, "Record already exists!");
                return;
            }
            if (SysFunctions.CustomCDBL(txttotAmount.Text) <1 && chkAdvance.Checked==false)
            {
                SysFuncs.UserMsg(lblMsg, Color.Red, "Receipt Amount Must be greater then Zero!");
                return;
            }
            if (gvPendingInvoice.Rows.Count > 0)
                {



                    double Receive = 0.00;
                    double Remaining = 0.00;
                    foreach (GridViewRow row in gvPendingInvoice.Rows)
                    {
                        CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                        if (chkSelect.Checked)
                        {
                            SumOfSelectedInvoice = SysFunctions.CustomCDBL(SumOfSelectedInvoice + SysFunctions.CustomCDBL(row.Cells[6].Text));
                        }
                    }
                RunningTotal = SumOfSelectedInvoice;

                   // txttotAmount.Text = SumOfSelectedInvoice.ToString();
                    Remaining = double.Parse(txtInstAmt.Text == "" ? "0" : txtInstAmt.Text);
                    if (double.Parse(txttotAmount.Text.Trim().Replace("&nbsp;", "") == "" ? "0" : txttotAmount.Text.Trim()) < 1)
                    {
                        SysFuncs.UserMsg(lblMsg, Color.Red, "Total amount should not be zero.", txttotAmount);
                        return;
                    }
                if (double.Parse(txttotAmount.Text.Trim().Replace("&nbsp;", "") == "" ? "0" : txttotAmount.Text.Trim()) > SumOfSelectedInvoice)
                {
                    SysFuncs.UserMsg(lblMsg, Color.Red, "Total amount should not be greater then Selected Invoice.", txttotAmount);
                    return;
                }
                else
                    {
                    RunningTotal = SysFunctions.CustomCDBL(txtInstAmt.Text);
                    AdjustAmount();
                        if (chkAdvance.Checked)
                        {
                            AdvanceAdjustAmount();
                        }
                 //   CalSubTotal();

                    }

                
            }
        }
        public double CaltotalInvAmount()
        {
            SumOfSelectedInvoice = 0;
            foreach (GridViewRow row in gvPendingInvoice.Rows)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect.Checked)
                {
                    SumOfSelectedInvoice = SumOfSelectedInvoice + SysFunctions.CustomCDBL(row.Cells[6].Text);
                }
            }
            return SumOfSelectedInvoice;
        }
        public void AdjustAmount()
        {
            Label lblTotalAmount = (Label)gvTaxDetail.FooterRow.FindControl("lblTotalAmount");
            RunningTotal = SysFunctions.CustomCDBL(txttotAmount.Text);

            ///Adjustment 
            ///
            foreach (GridViewRow row in gvPendingInvoice.Rows)
            {
               
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect.Checked)
                {

                    // double.Parse(row.Cells[5].Text.Trim().Replace("&nbsp;", "") == "" ? "0" : row.Cells[5].Text.Trim())
                    if (RunningTotal > SysFunctions.CustomCDBL(row.Cells[6].Text))
                    {
                        row.Cells[7].Text = row.Cells[6].Text;
                        row.Cells[8].Text = (SysFunctions.CustomCDBL(row.Cells[6].Text) - SysFunctions.CustomCDBL(row.Cells[7].Text)).ToString();
                        RunningTotal = RunningTotal - SysFunctions.CustomCDBL(row.Cells[6].Text.Trim());
                        txttotAdj.Text = txttotAmount.Text;
                            //(SumOfSelectedInvoice - double.Parse(lblTotalAmount.Text.Trim())).ToString();
                        txtAdj.Value = "t";
                        Count = Count + SysFunctions.CustomCDBL(row.Cells[6].Text.Trim().Replace("&nbsp;", "") == "" ? "0" : row.Cells[6].Text.Trim());
                        gvPendingInvoice.FooterRow.Cells[6].Text = Count.ToString();
                    }
                    else
                    {
                        row.Cells[7].Text = RunningTotal.ToString();
                        row.Cells[8].Text = (SysFunctions.CustomCDBL(row.Cells[6].Text) - SysFunctions.CustomCDBL(row.Cells[7].Text)).ToString();
                        txttotAdj.Text = txttotAmount.Text;
                            //(RunningTotal - double.Parse(lblTotalAmount.Text.Trim())).ToString();
                        txtAdj.Value = "t";
                        Count = Count + SysFunctions.CustomCDBL(row.Cells[6].Text.Trim().Replace("&nbsp;", "") == "" ? "0" : row.Cells[6].Text.Trim());
                        gvPendingInvoice.FooterRow.Cells[6].Text = Count.ToString();
                    }
                    

                    

                }
                
            }

        }

        public void AdvanceAdjustAmount()
        {

            //if (SysFunctions.CustomCDBL(txtAdvAmount.Text) > SysFunctions.CustomCDBL(txtInstAmt.Text))
            //{
            //    RunningTotal = SumOfSelectedInvoice;
            //    txtAdvAmount.Text = SumOfSelectedInvoice.ToString();
            //}
            //else
            //{
            //    RunningTotal = SysFunctions.CustomCDBL(txtInstAmt.Text) + SysFunctions.CustomCDBL(txtAdvAmount.Text);
            //}
            RunningTotal = SysFunctions.CustomCDBL(txttotAmount.Text);


            ///Adjustment 
            ///
            foreach (GridViewRow row in gvPendingInvoice.Rows)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect.Checked)
                {

                    // double.Parse(row.Cells[5].Text.Trim().Replace("&nbsp;", "") == "" ? "0" : row.Cells[5].Text.Trim())
                    if (RunningTotal > SysFunctions.CustomCDBL(row.Cells[6].Text))
                    {
                        row.Cells[7].Text = row.Cells[6].Text;
                        row.Cells[8].Text = (SysFunctions.CustomCDBL(row.Cells[6].Text) - SysFunctions.CustomCDBL(row.Cells[7].Text)).ToString();
                        RunningTotal = RunningTotal - SysFunctions.CustomCDBL(row.Cells[6].Text.Trim());
                    }
                    else
                    {
                        row.Cells[7].Text = RunningTotal.ToString();
                        row.Cells[8].Text = (SysFunctions.CustomCDBL(row.Cells[6].Text) - SysFunctions.CustomCDBL(row.Cells[7].Text)).ToString();
                    }




                }

            }
            
        }
        public void CalSubTotal()
        {
            Label lblTotalAmount = (Label)gvTaxDetail.FooterRow.FindControl("lblTotalAmount");
            totInsAmt = SysFunctions.CustomCDBL(txtInstAmt.Text);
            totAdvAmount = SysFunctions.CustomCDBL(txtAdvAmount.Text);
            totTaxAmt= SysFunctions.CustomCDBL(lblTotalAmount.Text);
            txttotAmount.Text = (totInsAmt + totTaxAmt + totAdvAmount).ToString();




            
        }
        public double InvAdjTotal()
        {
            Label lblTotalAmount = (Label)gvTaxDetail.FooterRow.FindControl("lblTotalAmount");
            foreach (GridViewRow row in gvPendingInvoice.Rows)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect.Checked)
                {
                    totAdjAmt = totAdjAmt + Convert.ToDouble(row.Cells[7].Text);
                }
            }
            return totAdjAmt = totAdjAmt-SysFunctions.CustomCDBL(lblTotalAmount.Text);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2559", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (ddlReceptNo.SelectedIndex != 0)
            {
                if (!sec.UserRight("2559", "003"))
                {
                    Response.Redirect("~/Test.aspx");
                }
            }
            
            double strAdvBalAmt = 0;
            double strAdvAmt = SysFunctions.CustomCDBL(txtAdvAmount.Text.Trim().Replace("&nbsp;", "") == "" ? "0" : txtAdvAmount.Text.Trim());
            if (ddlCust.SelectedIndex <= 0)
            {
                //PaymentReceiptEntry2(2);
                SysFuncs.UserMsg(lblMsg, Color.Red, "Please Select  the  Customer First!");
                return;
            }

            if (chkAdvance.Checked)
            {
             if(ddlAdvance.SelectedValue=="" || ddlAdvance.SelectedIndex == 0)
                {
                    SysFuncs.UserMsg(lblMsg, Color.Red, "Please Select Advance Receipt First!");
                }  
             if(txtAdvAmount.Text==""|| SysFunctions.CustomCDBL(txtAdvAmount.Text) <= 0)
                {
                    SysFuncs.UserMsg(lblMsg, Color.Red, "Please Enter Advance Amount First Or It Can not be Zero !");
                }
                if (ddlPaymentReceiptType.SelectedValue != "Insurance")
                {
                    strAdvBalAmt = SysFunctions.CustomCDBL(SysFuncs.GetStringValuesAgainstCodes("ReceiptNo", ddlAdvance.SelectedValue.ToString().Trim(), "AdvanceBalanceAmount", "PaymentReceiptMaster", "", Session["DealerCode"].ToString()));
                }
                else
                {
                    strAdvBalAmt = SysFunctions.CustomCDBL(SysFuncs.GetStringValuesAgainstCodes("AdvanceID", ddlAdvance.SelectedValue.ToString().Trim(), "AdvanceBalanceAmount", "AdvancePayment", "", Session["DealerCode"].ToString()));
                }
                    
               
            }

            //Check rocord is already exist?
            if(ddlReceptNo.SelectedIndex != 0)
            {
                    //PaymentReceiptEntry2(2);
                    SysFuncs.UserMsg(lblMsg, Color.Red, "Record already exists!");
                    return;
            }
            if (ddlPaymentReceiptType.SelectedValue == "Insurance")
            {
                if (ddlCust.SelectedIndex==0)
                {
                    SysFuncs.UserMsg(lblMsg, Color.Red, "Customer code dose not left blank");
                    return;
                }
                if (ddlInsCo.SelectedIndex == 0)
                {
                    SysFuncs.UserMsg(lblMsg, Color.Red, "Insurance Company  dose not left blank");
                    return;
                }
                if (ddlInsBranch.SelectedIndex == 0)
                {
                    SysFuncs.UserMsg(lblMsg, Color.Red, "Insurance Branch  dose not left blank");
                    return;
                }
            }
            //else
            //{
            //    if (ddlInsCo.SelectedIndex == 0)
            //    {
            //        SysFuncs.UserMsg(lblMsg, Color.Red, "Select Insurance company first.");
            //        return;
            //    }
            //    if (ddlInsBranch.SelectedIndex == 0)
            //    {
            //        SysFuncs.UserMsg(lblMsg, Color.Red, "Select Insurance Branch.");
            //        return;
            //    }
            //}
            //Check textbox null or empty
            if (ddlPaymentReceiptType.SelectedValue.ToString() != "Advance")
            {
                TextBox[] textBoxes = { txttotAmount };
                if (!MasterValidation(textBoxes)) return;
                if (txttotAdj.Text.Trim() == "0")
                {
                    SysFuncs.UserMsg(lblMsg, Color.Red, "Please press Auto Adjest (FIFO) button", txttotAmount);
                    return;
                }

                if (ddlTransType.SelectedValue != "Advance")
                {
                   // int strAdvAmt = Convert.ToInt32(txtAdvAmount.Text.Trim().Replace("&nbsp;", "") == "" ? "0" : txtAdvAmount.Text.Trim());
                    if (strAdvAmt > strAdvBalAmt)
                    {
                        SysFuncs.UserMsg(lblMsg, Color.Red, "Advance Amount can not be greater then Balance amount");
                        return;
                    }
                    if (double.Parse(txttotAdj.Text == "" ? "0" : txttotAdj.Text) > double.Parse(gvPendingInvoice.FooterRow.Cells[5].Text.Trim().Replace("&nbsp;", "") == "" ? "0" : gvPendingInvoice.FooterRow.Cells[5].Text.Trim()))
                    {
                        SysFuncs.UserMsg(lblMsg, Color.Red, "Adjusted amount not greater then Total Amount amount", txttotAmount);
                        return;
                    }
                

                if (txtAdj.Value == "f")
                {
                    SysFuncs.UserMsg(lblMsg, Color.Red, "Please press Auto (FIFO) ", txttotAmount);
                    return;
                }
                }
                txtAdvAmount.Text=txtAdvAmount.Text.Trim().Replace("&nbsp;", "") == "" ? "0" : txtAdvAmount.Text.Trim();
                if (chkAdvance.Checked && txtAdvAmount.Text == "0")
                {
                    SysFuncs.UserMsg(lblMsg, Color.Red, "Advance amount cant not be zero(0)");
                    return;
                }
               

                decimal dGridCeckedAdjAmount = 0;
                foreach (GridViewRow row in gvPendingInvoice.Rows)
                {
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                    dGridCeckedAdjAmount = Convert.ToDecimal(row.Cells[7].Text.Trim() == "&nbsp;" ? "0" : row.Cells[7].Text.Trim());
                    if (chkSelect.Checked)
                    {
                        if (dGridCeckedAdjAmount < 1)
                        {
                            SysFuncs.UserMsg(lblMsg, Color.Red, "Selected invoice adjusted amount can not be zero(0)");
                            return;
                        }
                    }
                    if (chkSelect.Checked == false)
                    {
                        if (dGridCeckedAdjAmount > 1)
                        {
                            SysFuncs.UserMsg(lblMsg, Color.Red, "Not Selected invoice adjusted amount can not be zero(0)");
                            return;
                        }
                    }

                }
                PaymentReceiptEntry(strAdvBalAmt);
            }
            if (ddlPaymentReceiptType.SelectedValue.ToString() == "Advance")
            {
                TextBox[] textBoxes = { txtInstAmt };
                if (!MasterValidation(textBoxes)) return;
                if (txtInstAmt.Text.Trim() == "0")
                {
                    SysFuncs.UserMsg(lblMsg, Color.Red, "Receipt Amount cannot be zero(0)", txtInstAmt);
                    return;
                }
                string strReceiptNo = string.Empty;
                if (Inert_PaymentReceiptMaster(ref strReceiptNo, strAdvBalAmt))
                {
                    ObjTrans.CommittTransaction(ref Trans);
                    SysFuncs.UserMsg(lblMsg, Color.Green, "Advance payment saved successfully");
                }
                else
                {
                    ObjTrans.RollBackTransaction(ref Trans);
                    SysFuncs.UserMsg(lblMsg, Color.Red, "Advance payment not saved");
                }
            }
            Load_ReceiptNo();
        }
        private bool Inert_PaymentReceiptMaster(ref string strReceiptNo, double strAdvBalAmt)
        {
            //PaymentReceiptEntry2(1);
            
            SqlParameter[] PmtRecMaster_param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),          /*1*/ new SqlParameter("@ReceiptNo",SqlDbType.Char,8),
           /*2*/ new SqlParameter("@ReceiptDate",SqlDbType.DateTime),       /*3*/ new SqlParameter("@InvoiceType",SqlDbType.VarChar,20),
           /*4*/ new SqlParameter("@CusCode",SqlDbType.VarChar,8),          /*5*/ new SqlParameter("@InsCompCode",SqlDbType.Char,4),
           /*6*/ new SqlParameter("@BranchCode",SqlDbType.Char,4),          /*7*/ new SqlParameter("@InsCusFlag",SqlDbType.Char,1),
           /*8*/ new SqlParameter("@Remarks",SqlDbType.Char,100),           /*9*/ new SqlParameter("@PayModeCode",SqlDbType.Char,1),
           /*10*/ new SqlParameter("@InsNo",SqlDbType.VarChar,15),          /*11*/ new SqlParameter("@InsDate",SqlDbType.DateTime),
           /*12*/ new SqlParameter("@AmountPaid",SqlDbType.Float),          /*13*/ new SqlParameter("@BankCode",SqlDbType.Char,5),
           /*14*/ new SqlParameter("@Branch",SqlDbType.VarChar,50),         /*15*/ new SqlParameter("@AdvanceAmount",SqlDbType.Float),
           /*16*/ new SqlParameter("@InvTotal",SqlDbType.Float),            /*17*/ new SqlParameter("@OutSTTotal",SqlDbType.Float),
           /*18*/ new SqlParameter("@InvAdjTotal",SqlDbType.Float),         /*19*/ new SqlParameter("@DelFlag",SqlDbType.Char,1),
           /*20*/ new SqlParameter("@UpdUser",SqlDbType.Char,50),           /*21*/ new SqlParameter("@UpdTerm",SqlDbType.Char,50),          
           /*22*/ new SqlParameter("@VoucherNo",SqlDbType.VarChar,50),      /*23*/ new SqlParameter("@VoucherFlag",SqlDbType.Char,1),
           /*24*/ new SqlParameter("@AdvancePaid",SqlDbType.Decimal),       /*25*/ new SqlParameter("@TransType",SqlDbType.VarChar,30),
           /*26*/ new SqlParameter("@IsAdjustAdvance",SqlDbType.Char,1),
           /*27*/ new SqlParameter("@AdvanceReceiptNo",SqlDbType.Char,8),
           /*28*/ new SqlParameter("@AdvanceAdjustedAmount",SqlDbType.Float),
           /*29*/ new SqlParameter("@AdvanceBalanceAmount",SqlDbType.Float),
           /*30*/ new SqlParameter("@DocumentNo",SqlDbType.VarChar,50)
            };

            PmtRecMaster_param[0].Value = Session["DealerCode"].ToString();
            PmtRecMaster_param[2].Value = SysFuncs.SaveDate(txtReceiptDate.Text); //EstimateCode        
            PmtRecMaster_param[3].Value = ddlPaymentReceiptType.SelectedItem.Value;
            PmtRecMaster_param[4].Value = ddlCust.SelectedValue.ToString();
            if (ddlPaymentReceiptType.SelectedValue=="Insurance")
            {
                PmtRecMaster_param[5].Value = ddlInsCo.SelectedValue;
                PmtRecMaster_param[6].Value = ddlInsBranch.SelectedValue;
            }
            else
            {
                PmtRecMaster_param[5].Value = "";
                PmtRecMaster_param[6].Value = "";
            }
            PmtRecMaster_param[7].Value = "N";
            PmtRecMaster_param[8].Value = txtRemarks.Text.Trim().ToUpper();
            PmtRecMaster_param[9].Value = DDLPaymentMode.SelectedItem.Value;
            PmtRecMaster_param[10].Value = txtInstNo.Text.Trim().ToUpper();

            PmtRecMaster_param[11].Value = SysFuncs.SaveDate(txtInstDate.Text);
            PmtRecMaster_param[12].Value = txtInstAmt.Text.Trim().Replace("&nbsp;", "") == "" ? "0" : txtInstAmt.Text.Trim();
            PmtRecMaster_param[13].Value = ddlbank.SelectedItem.Value;
            PmtRecMaster_param[14].Value = txtBranch.Text.Trim().Replace("&nbsp;", "");
            

            if (ddlTransType.SelectedValue == "Advance")
            {
                PmtRecMaster_param[15].Value = txttotAmount.Text;
                PmtRecMaster_param[16].Value = "0";
                PmtRecMaster_param[17].Value = "0";
                PmtRecMaster_param[18].Value = "0";
            }
            else
            {
                PmtRecMaster_param[16].Value = double.Parse(gvPendingInvoice.FooterRow.Cells[5].Text.Trim());
                PmtRecMaster_param[17].Value = double.Parse(gvPendingInvoice.FooterRow.Cells[6].Text.Trim());
               // PmtRecMaster_param[18].Value = double.Parse(gvPendingInvoice.FooterRow.Cells[7].Text.Trim());
                PmtRecMaster_param[18].Value = txttotAdj.Text;
            }
            PmtRecMaster_param[19].Value = "N";

            PmtRecMaster_param[20].Value = Session["UserName"].ToString();
            PmtRecMaster_param[21].Value = GlobalVar.mUserIPAddress;

            PmtRecMaster_param[22].Value = "";
            PmtRecMaster_param[23].Value = "N";
            PmtRecMaster_param[24].Value = "0";
            PmtRecMaster_param[25].Value = ddlTransType.SelectedValue.ToString();
            if (ddlTransType.SelectedValue == "Advance")
            {
                PmtRecMaster_param[26].Value = "Y";
                PmtRecMaster_param[27].Value = (object)DBNull.Value;
                PmtRecMaster_param[28].Value = double.Parse("0");
                PmtRecMaster_param[29].Value = double.Parse(txtInstAmt.Text == "" ? "0" : txtInstAmt.Text);
            }
            else if (chkAdvance.Checked && ddlTransType.SelectedValue != "Advance")
            {
                PmtRecMaster_param[26].Value = "Y";
                PmtRecMaster_param[27].Value = ddlAdvance.SelectedValue.ToString();
                PmtRecMaster_param[28].Value = double.Parse(txtAdvAmount.Text == "" ? "0" : txtAdvAmount.Text);
                PmtRecMaster_param[29].Value = strAdvBalAmt - double.Parse(txtAdvAmount.Text == "" ? "0" : txtAdvAmount.Text);
            }
            else
            {
                PmtRecMaster_param[26].Value = "N";
                PmtRecMaster_param[27].Value = (object)DBNull.Value;
                PmtRecMaster_param[28].Value = (object)DBNull.Value;
                PmtRecMaster_param[29].Value = (object)DBNull.Value;
            }
            PmtRecMaster_param[30].Value = txtDocNo.Text; ;

            try
            {
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    if(ddlReceptNo.SelectedIndex==0)
                    {
                        strReceiptNo = SysFuncs.AutoGen("PaymentReceiptMaster", "ReceiptNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                    }
                    else
                    {
                        strReceiptNo = ddlReceptNo.SelectedValue;
                    }
                    
                    PmtRecMaster_param[1].Value = strReceiptNo;
                    if (SysFuncs.ExecuteSP_NonQuery("[sp_W2_PaymentReceipt_Master_Insert]", PmtRecMaster_param, Trans))
                    {  
                        return true;
                    }
                    else
                    {
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
                SysFuncs.UserMsg(lblMsg, Color.Red, ex.Message);
                return false;
            }
        }
        private void PaymentReceiptEntry(double strAdvBalAmt)
        {
            
            try
            {
                string strReceiptNo = string.Empty;
                if (Inert_PaymentReceiptMaster(ref strReceiptNo,strAdvBalAmt))
                {

                    SqlParameter[] PmtRecDetail_param = {                                            
                                                            /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                            /*1*/ new SqlParameter("@ReceiptNo",SqlDbType.Char,8),
                                                            /*2*/ new SqlParameter("@InvoiceType",SqlDbType.VarChar,20),
                                                            /*3*/ new SqlParameter("@InvoiceNo",SqlDbType.Char,8),
                                                            /*4*/ new SqlParameter("@InvoiceDate",SqlDbType.DateTime),
                                                            /*5*/ new SqlParameter("@InvAmount",SqlDbType.Decimal),
                                                            /*6*/ new SqlParameter("@OutStAmount",SqlDbType.Decimal),
                                                            /*7*/ new SqlParameter("@AdjAmount",SqlDbType.Decimal)
                                                        };

                    PmtRecDetail_param[0].Value = Session["DealerCode"].ToString();
                    PmtRecDetail_param[1].Value = strReceiptNo;
                    PmtRecDetail_param[2].Value = ddlPaymentReceiptType.SelectedItem.Value;
                    foreach (GridViewRow row in gvPendingInvoice.Rows)
                    {
                        CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                        if (chkSelect.Checked)
                        {
                            LinkButton lkn = (LinkButton)row.FindControl("lnkInvoiceNo");
                            PmtRecDetail_param[3].Value = lkn.Text;
                            PmtRecDetail_param[4].Value = SysFuncs.SaveDate(row.Cells[4].Text.Trim());
                            PmtRecDetail_param[5].Value = row.Cells[5].Text.Trim();
                            PmtRecDetail_param[6].Value = row.Cells[6].Text.Trim();
                            if (row.Cells[7].Text.Trim() == "&nbsp;") PmtRecDetail_param[7].Value = "0";
                            else PmtRecDetail_param[7].Value = row.Cells[7].Text.Trim();
                            SysFuncs.ExecuteSP_NonQuery("[sp_W2_PaymentReceipt_Detail_Insert]", PmtRecDetail_param, Trans);
                            //Updation Of invoice
                            if (chkAdvance.Checked)
                            {
                                SqlParameter[] UpdateAdvance_param = {                                            
                                                            /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                            /*1*/ new SqlParameter("@ReceiptNo",SqlDbType.Char,8),
                                                            /*2*/ new SqlParameter("@AdvanceReceiptNo",SqlDbType.Char,8),
                                                            /*3*/ new SqlParameter("@AdvanceAdjustedAmount",SqlDbType.Float),
                                                            /*3*/ new SqlParameter("@AdvanceBalAmount",SqlDbType.Float)
                                                                    };
                                UpdateAdvance_param[0].Value = Session["DealerCode"].ToString();
                                UpdateAdvance_param[1].Value = ddlAdvance.SelectedValue.ToString();
                                UpdateAdvance_param[2].Value = strReceiptNo;
                                UpdateAdvance_param[3].Value = SysFunctions.CustomCDBL(txtAdvAmount.Text == "" ? "0" : txtAdvAmount.Text);
                                UpdateAdvance_param[4].Value =SysFunctions.CustomCDBL(strAdvBalAmt)-SysFunctions.CustomCDBL(txtAdvAmount.Text == "" ? "0" : txtAdvAmount.Text);
                                if (ddlPaymentReceiptType.SelectedValue != "Insurance")
                                {
                                    SysFuncs.ExecuteSP_NonQuery("[sp_PaymentReceiptMaster_UpdateOnAdvance]", UpdateAdvance_param, Trans);
                                }
                                else
                                {
                                    SysFuncs.ExecuteSP_NonQuery("[sp_AdvanceID_UpdateOnAdvance]", UpdateAdvance_param, Trans);
                                }
                                
                            }
                            if (ddlPaymentReceiptType.SelectedValue == "CountrSale")
                            {
                                string IQuery = "Update CounterSaleMaster set TotReceipt= TotReceipt +'" + PmtRecDetail_param[7].Value + "' " +
                                     "Where DealerCode='" + Session["DealerCode"].ToString() + "' and SaleInvNo='" + PmtRecDetail_param[3].Value + "'";
                                SysFuncs.ExecuteQuery(IQuery, Trans);
                            }
                            else if (ddlPaymentReceiptType.SelectedValue == "Sales")
                            {
                                string IQuery = "Update ExpenditureMaster set TotReceipt= TotReceipt +'" + PmtRecDetail_param[7].Value + "' " +
                                     "Where DealerCode='" + Session["DealerCode"].ToString() + "' and ExpendCode='" + PmtRecDetail_param[3].Value + "'";
                                SysFuncs.ExecuteQuery(IQuery, Trans);
                            }
                            else if (ddlPaymentReceiptType.SelectedValue == "Insurance")
                            {
                                string IQuery = "Update dbo.CustomerInvoice set PaidI= PaidI +'" + PmtRecDetail_param[7].Value + "' " +
                                          "Where DealerCode='" + Session["DealerCode"].ToString() + "' and InvoiceNo='" + PmtRecDetail_param[3].Value + "'";
                                SysFuncs.ExecuteQuery(IQuery, Trans);
                            }
                            else 
                            {
                                string IQuery;
                                if (ddlPaymentReceiptType.SelectedValue == "Insurance")
                                {
                                    IQuery = "Update dbo.CustomerInvoice set PaidI= PaidI +'" + PmtRecDetail_param[7].Value + "' " +
                                          "Where DealerCode='" + Session["DealerCode"].ToString() + "' and InvoiceNo='" + PmtRecDetail_param[3].Value + "'";
                                }
                                else
                                {
                                    IQuery = "Update dbo.CustomerInvoice set PaidC= paidC +'" + PmtRecDetail_param[7].Value + "' " +
                                          "Where DealerCode='" + Session["DealerCode"].ToString() + "' and InvoiceNo='" + PmtRecDetail_param[3].Value + "'";
                                }
                                SysFuncs.ExecuteQuery(IQuery, Trans);
                            }
                        }
                    }
                    SqlParameter[] PmtRecTaxDetail_param = {                                            
                                                            /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                            /*1*/ new SqlParameter("@ReceiptNo",SqlDbType.Char,8),
                                                            /*2*/ new SqlParameter("@ReceiptHead",SqlDbType.Char,50),
                                                            /*3*/ new SqlParameter("@AccountCode",SqlDbType.Char,1),
                                                            /*4*/ new SqlParameter("@Amount",SqlDbType.Float),
                                                            /*5*/ new SqlParameter("@TaxID",SqlDbType.Char,2),
                                                            /*6*/ new SqlParameter("@TaxPerc",SqlDbType.Decimal),
                                                           };

                    PmtRecTaxDetail_param[0].Value = Session["DealerCode"].ToString();
                    PmtRecTaxDetail_param[1].Value = strReceiptNo;
                    foreach (GridViewRow row in gvTaxDetail.Rows)
                    {
                        Label lblRecHead = (Label)row.FindControl("lblRecHead");
                        Label lblAmount = (Label)row.FindControl("lblAmount");
                        Label lblTaxID = (Label)row.FindControl("lblTaxID");
                            Label lblPercent = (Label)row.FindControl("lblPercent");
                        if (lblRecHead.Text != "")
                        {
                            PmtRecTaxDetail_param[2].Value = lblRecHead.Text;
                            PmtRecTaxDetail_param[3].Value = "";
                            PmtRecTaxDetail_param[4].Value = Math.Round(Convert.ToDecimal(lblAmount.Text.Trim()),2);
                            PmtRecTaxDetail_param[5].Value = lblTaxID.Text.Trim();
                            PmtRecTaxDetail_param[6].Value = Convert.ToDecimal(lblPercent.Text.Trim());
                            SysFuncs.ExecuteSP_NonQuery("[sp_W2_PaymentReceipt_TaxDetail_Insert]", PmtRecTaxDetail_param, Trans);
                        }
                    }

                }
                else { 
                    SysFuncs.UserMsg(lblMsg, Color.Red, "Operation Failed! Record Not Saved.");
                    ObjTrans.RollBackTransaction(ref Trans);
                    return;
                }
                    ObjTrans.CommittTransaction(ref Trans);
                    lblMsg.Visible = true;
                    SysFuncs.UserMsg(lblMsg, Color.Green, "Record Saved Successfully: " + strReceiptNo);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);
              //  clearAll();
                
            }
            catch (Exception ex) { ObjTrans.RollBackTransaction(ref Trans);
                lblMsg.Text= ex.Message;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Focus();
            }
        }


        protected void clearAll()
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);
            //txtReceiptNo.Text = "";
            //ddlPaymentReceiptType.SelectedValue = "Service";
            SysFuncs.ClearTextBoxes(Page);
            SysFuncs.Clearddl(Page);
            txtInvTotal.Text = "";
            txtVoucherNo.Text = "";
            chkAdvance.Checked = false;
            txtCust.Text = "";
            txtCusDesc.Text = "";
            ddlPaymentReceiptType.Items.Clear();
            RBLTransType.ClearSelection();
            DDLPaymentMode.SelectedValue = "C";  
            ddlbank.SelectedValue = "0"; 
            createPartsDT();
            createTaxDetail();
            txtInstDate.Text = txtReceiptDate.Text = System.DateTime.Now.Date.ToString("dd-MM-yyyy");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clearAll();
            ddlCust.Enabled = true;
            ddlCust.Visible = true;
            txtCust.Visible = false;
            txtCusDesc.Visible = false;
            imgCustomerCode.Enabled = false;

            txtInstDate.Text = txtReceiptDate.Text = System.DateTime.Now.Date.ToString("dd-MM-yyyy");
            lblMsg.Text = string.Empty;
        }
        protected void gvPendingInvoice_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2559", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }
            ReportDocument RD = new ReportDocument();
            DXBMS.Data.DSReports objDsReports = new Data.DSReports();

            DataTable dt = new DataTable();
            if (ddlReceptNo.SelectedIndex == 0)
            {
                SysFuncs.UserMsg(lblMsg, Color.Red, "Please select Receipt No.");
                return;
            }
            string sql = "exec sp_PaymentReceipt_Print '" + Session["DealerCode"].ToString() + "','" + ddlReceptNo.SelectedValue.ToString().Trim() + "'";
            dt = SysFuncs.GetData(sql);
            objDsReports.sp_PaymentReceipt_Print.Load(dt.CreateDataReader());
            sql = "exec sp_W2_PaymentReceiptTaxDetail_Select '" + Session["DealerCode"].ToString() + "','" + ddlReceptNo.SelectedValue.ToString().Trim() + "'";
            dt = SysFuncs.GetData(sql);
            objDsReports.sp_W2_PaymentReceiptTaxDetail_Select.Load(dt.CreateDataReader());

            RD.PrintOptions.PaperSize = PaperSize.PaperA4;
            if (ddlPaymentReceiptType.SelectedValue == "Insurance")
            {
               
                RD.Load(Server.MapPath("../ServiceReports/rptPaymentRecPrintIns.rpt"));

            }
            else
            {
                RD.Load(Server.MapPath("../ServiceReports/rptPaymentRecPrint.rpt"));
            }
         
            RD.OpenSubreport(Server.MapPath("../ServiceReports/rptPaymentReceiptTaxDetail.rpt"));
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
            //string FilePath =Server.MapPath( "../../../Download/");
            //string FilePath = Server.MapPath("~") + "\\Download\\";
            //string FileName = "PaymentRecPrint" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            //string File = FilePath + FileName;
            ////RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);
            //Session["RD"] = RD;


            //string URL;
            //URL = "../../../Download/rptViewerService.aspx?FileName=" + FileName;

            ////URL = FilePath + "OpenPdf.aspx?FileName=" + FileName;
            ////txtPartItemDesc.Text = URL;
            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ////URL = "../../../Download/OpenPdf.aspx?FileName=" + FileName; 
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
            /////////////////////////////////////////////////////////
            ////ReportDocument RD = new ReportDocument();
            ////DXBMS.Data.DSReports objDsReports = new Data.DSReports();

            ////DataTable dt = new DataTable();
            ////string sql = "exec sp_PaymentReceipt_Print '" + Session["DealerCode"].ToString() + "','" + ddlReceptNo.SelectedValue.ToString().Trim() + "'";
            ////dt = SysFuncs.GetData(sql);
            ////objDsReports.sp_PaymentReceipt_Print.Load(dt.CreateDataReader());
            ////RD.PrintOptions.PaperSize = PaperSize.PaperA4;

            ////RD.Load(Server.MapPath("../ServiceReports/rptPaymentReceiptPrint.rpt"));
            ////RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            ////RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            ////RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString() + "'";

            ////RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            ////RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";

            ////RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
            ////RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
            ////RD.SetDataSource(objDsReports);

            ////string FilePath = Server.MapPath("~") + "\\Download\\";

            ////string FileName = "CounterSaleInvoice" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            ////string File = FilePath + FileName;

            ////RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            ////string URL;
            ////URL = "../../../Download/OpenPdf.aspx?FileName=" + FileName;

            //////URL = FilePath + "OpenPdf.aspx?FileName=" + FileName;
            //////txtPartItemDesc.Text = URL;
            ////string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //////URL = "../../../Download/OpenPdf.aspx?FileName=" + FileName; 
            ////ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
            //------------------------------//----------------------------//--------------------------//


        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2559", "004"))
            {
                Response.Redirect("~/Test.aspx");
            }
            TextBox[] textBoxes = {  txtInstAmt, txttotAmount };

            if (SysFuncs.CheckVoucherPostFlag(Session["DealerCode"].ToString(), txtVoucherNo.Text))
            {
                SysFuncs.UserMsg(lblMsg, Color.Red, "Can't Edit or Delete the Voucher '" + txtVoucherNo.Text + "' is already Posted");
                return;
            }

            if (!MasterValidation(textBoxes)) return;
            else
            {
                //if ((SysFuncs.IsExist("ReceiptNo", txtReceiptNo.Text.Trim(), "paymentReceiptMaster",Session["DealerCode"].ToString()))
                //    & (txtReceiptNo.Text.Trim().Length != 0))
                //{
                SysFuncs.UpdateJV(Session["DealerCode"].ToString(), txtVoucherNo.Text);
                    SqlParameter[] PmtRecMaster_param = { /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                        /*1*/ new SqlParameter("@ReceiptNo",SqlDbType.Char,8)           
                                                    };
                    PmtRecMaster_param[0].Value = Session["DealerCode"].ToString();
                    PmtRecMaster_param[1].Value = ddlReceptNo.SelectedValue.ToString().Trim();

                    try
                    {
                    string link;
                    foreach (GridViewRow row in gvPendingInvoice.Rows)
                    {
                        LinkButton chkSelect = (LinkButton)row.FindControl("lnkInvoiceNo");
                        link = chkSelect.Text;


                    }
                    if (ObjTrans.BeginTransaction(ref Trans) == true)
                        {
                            if (SysFuncs.ExecuteSP_NonQuery("[sp_W2_PaymentReceipt_Master_Delete]", PmtRecMaster_param, Trans))
                            {
                                SqlParameter[] PmtRecDetail_param = {   /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    /*1*/ new SqlParameter("@ReceiptNo",SqlDbType.Char,8), };

                                PmtRecDetail_param[0].Value = Session["DealerCode"].ToString();
                                PmtRecDetail_param[1].Value = ddlReceptNo.SelectedValue.ToString().Trim();
                                foreach (GridViewRow row in gvPendingInvoice.Rows)
                                {
                                LinkButton chkSelect = (LinkButton)row.FindControl("lnkInvoiceNo");

                                SysFuncs.ExecuteSP_NonQuery("[sp_W2_PaymentReceiptDetail_Delete]", PmtRecDetail_param, Trans);
                                    string IQuery = "Update dbo.CustomerInvoice set PaidC=PaidC - '" + row.Cells[7].Text.Trim() + "' " +
                                          "Where DealerCode='" + Session["DealerCode"].ToString() + "' and InvoiceNo='" + chkSelect.Text + "'";
                                    SysFuncs.ExecuteQuery(IQuery, Trans);
                                }

                                SqlParameter[] PmtRecTaxDetail_param = { /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    /*1*/ new SqlParameter("@ReceiptNo",SqlDbType.Char,8)};

                                PmtRecTaxDetail_param[0].Value = Session["DealerCode"].ToString();
                                PmtRecTaxDetail_param[1].Value = ddlReceptNo.SelectedValue.ToString().Trim();
                                SysFuncs.ExecuteSP_NonQuery("[sp_W2_PaymentReceiptTaxDetail_Delete]", PmtRecTaxDetail_param, Trans);

                            }
                            else { SysFuncs.UserMsg(lblMsg,Color.Red,"Operation Failed! Record Not Deleted."); }
                            ObjTrans.CommittTransaction(ref Trans);

                            SysFuncs.UserMsg(lblMsg, Color.Green, "Record Deleted Successfully : " + ddlReceptNo.SelectedValue.ToString().Trim());
                        //   clearAll();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Deletealert()", true);
                    }
                    }
                    catch (Exception ex) { ObjTrans.RollBackTransaction(ref Trans); throw ex; }
                //}
            }
        }
        protected void txtInstAmt_TextChanged(object sender, EventArgs e)
        {
            CalSubTotal();

        }
        protected void lnkRemove_Click(object sender, EventArgs e)
        {

        }
        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            //LinkButton btn = sender as LinkButton;
            //TableCell tc = btn.Parent as TableCell;
            //GridViewRow gvr = tc.Parent as GridViewRow;
            //DataTable dt = new DataTable();
            //dt = (DataTable)ViewState["DtUnPaid"];


            //dt.Rows.RemoveAt(gvr.RowIndex);

            //gvPendingInvoice.DataSource = dt;
            //gvPendingInvoice.DataBind();
            //txtAdj.Value = "f";
            //ViewState["DtUnPaid"] = dt;

        }
       
        protected void ddlInsCo_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlParameter[] param = { new SqlParameter("@InsCompCode", SqlDbType.Char, 5) };
            param[0].Value = ddlInsCo.SelectedValue;
            objMBLL.FillDrp_SP(ddlInsBranch, "sp_2W_Branch_select", "BranchCode", "BranchDesc", param, true, "--Select--", false, "");
        }
        protected void chkInsRec_CheckedChanged(object sender, EventArgs e)
        {
           // if (ddlPaymentReceiptType.SelectedIndex == 1 || ddlPaymentReceiptType.SelectedIndex == 2)
            //{
            //    if (chkInsRec.Checked == true)
            //    {
            //        lblInsComp.Visible = true;
            //        lblInsBr.Visible = true;
            //        ddlInsBranch.Visible = true;
            //        ddlInsCo.Visible = true;
            //    }
            //    else
            //    {
            //        lblInsComp.Visible = false;
            //        lblInsBr.Visible = false;
            //        ddlInsBranch.Visible = false;
            //        ddlInsCo.Visible = false;
            //    }
           // }
        }

        protected void ddlInsBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPaymentReceiptType.SelectedIndex == 1)
            {
                if (ddlPaymentReceiptType.SelectedValue=="Insurance")
                {
                    LoadMasterData("I");
                }
            }
        }

        protected void BtnAdd_Click1(object sender, ImageClickEventArgs e)
        {
           TextBox txtFooterTaxAmt= (TextBox)gvTaxDetail.FooterRow.FindControl("txtFooterTaxAmt");
            TextBox txtFooterTaxPerc= (TextBox)gvTaxDetail.FooterRow.FindControl("txtFooterTaxPerc");
           DropDownList DDLFooterReceiptHead = (DropDownList)gvTaxDetail.FooterRow.FindControl("DDLFooterReceiptHead");
           Label lblTotalAmount = (Label)gvTaxDetail.FooterRow.FindControl("lblTotalAmount");
           
            TextBox[] textBoxes = { txtFooterTaxAmt,txtFooterTaxPerc };
            if (!MasterValidation(textBoxes)) return;
            if (DDLFooterReceiptHead.SelectedIndex == 0)
            {
                SysFuncs.UserMsg(lblMsg, Color.Red, "Please Select TaxType First");
                return;
            }
            if (Convert.ToDecimal(txtFooterTaxAmt.Text) == 0)
            {
                SysFuncs.UserMsg(lblMsg, Color.Red, "Tax Amount can not be zero(0)");
                return;
            }
                TaxDetailDT = (DataTable)ViewState["TaxDetail"];
                search_result = false;
                foreach (DataRow rowTaxDetailDT in TaxDetailDT.Rows)
                {
                    if (rowTaxDetailDT["ReceiptHead"].ToString().Trim() == ""
                        | rowTaxDetailDT["ReceiptHead"].ToString().Trim() == DDLFooterReceiptHead.SelectedItem.Text.Trim())
                    {
                        Insert_in_TaxDetailDT(rowTaxDetailDT);
                        search_result = true;
                    }
                }
                if (search_result == false)
                {
                    DataRow rowTaxDetailDT = TaxDetailDT.NewRow();
                    Insert_in_TaxDetailDT(rowTaxDetailDT);
                    TaxDetailDT.Rows.Add(rowTaxDetailDT);
                }

                ViewState["TaxDetail"] = TaxDetailDT; gvTaxDetail.DataSource = TaxDetailDT; gvTaxDetail.DataBind();
            CalSubTotal();
            txtAdj.Value = "f";
           
        }

        protected void BtnRemove_Click1(object sender, ImageClickEventArgs e)
        {
            if (gvPendingInvoice.Rows.Count == 0)
            {
                createPartsDT();
            }
            ImageButton btn = sender as ImageButton;
            TableCell tc = btn.Parent as TableCell;
            GridViewRow gvr = tc.Parent as GridViewRow;
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["DtUnPaid"];


            dt.Rows.RemoveAt(gvr.RowIndex);

            gvPendingInvoice.DataSource = dt;
            gvPendingInvoice.DataBind();
            txtAdj.Value = "f";
            ViewState["DtUnPaid"] = dt;
            
        }

        protected void BtnRemove_Click2(object sender, ImageClickEventArgs e)
        {
            try
            {

                //int ridx = (int)(ViewState["ridx"]);
                //TaxDetailDT = (DataTable)ViewState["TaxDetail"];
                //TaxDetailDT.Rows.Remove(TaxDetailDT.Rows[ridx]);
                //TaxDetailDT.AcceptChanges();
                ////ClearTaxDetailTextBoxes();
                //if (TaxDetailDT.Rows.Count == 0) TaxDetailDT.Rows.Add(TaxDetailDT.NewRow());
                //ViewState["TaxDetail"] = TaxDetailDT; gvTaxDetail.DataSource = TaxDetailDT; gvTaxDetail.DataBind();
                DataTable tdDelete = new DataTable();
                try
                {
                    tdDelete = (DataTable)ViewState["TaxDetail"];
                    ImageButton BtnLnk = (ImageButton)sender;
                    GridViewRow rowGv = (GridViewRow)BtnLnk.Parent.Parent;
                    int rowGvIndex = rowGv.RowIndex;
                    //Label lblDCQty = (Label)GvMain.Rows[rowGv.RowIndex].FindControl("lblDCQty");
                    //if (Convert.ToInt32(lblDCQty.Text) > 0)
                    //{
                    //    SysFuncs.UserMsg(LbErr, Color.Red, "Stock issued against this part");
                    //    return;
                    //}
                    tdDelete.Rows[rowGvIndex].Delete();
                    tdDelete.AcceptChanges();
                    if (tdDelete.Rows.Count == 0)
                    {
                        DataRow dr = tdDelete.NewRow();
                        tdDelete.Rows.Add(dr);
                        //ddlDiscount.Enabled = true;
                    }
                    gvTaxDetail.DataSource = tdDelete;
                    gvTaxDetail.DataBind();
                    //LbErr.Text = "";
                    ViewState["TaxDetail"] = tdDelete;
                    TaxDetailDT = tdDelete;
                    CalSubTotal();
                    //totalCalculation();

                }
                catch (Exception ex)
                {
                    SysFuncs.UserMsg(lblMsg, Color.Red, ex.Message);
                }
            }
            catch { return; }
        }
        private void Load_Customer()
        {
            string WhereClause = " DealerCode = '" + Session["DealerCode"].ToString() + "'";

            string[] Columns = new string[] { "cusCode", "CusDesc+' '+FatherHusName",  "isnull(phone1,isnull(phone2,CellNo))" };
            SysFunc.GetMultiColumnsDDL(ddlCust, Columns, "Customer ", WhereClause, "cusCode", " Order by CusCode Desc ", false, false);

            //            if (ddlPaymentReceiptType.SelectedValue == "DirectSale")
            //            {
            //                string WhereQuery = "A.DealerCode ='" + Session["DealerCode"].ToString() + "' " +
            //                "AND a.DelFlag = 'N' " +
            //                "AND a.DealerCode = b.DealerCode " +
            //                "AND a.CusCode = b.CusCode " +
            //                "AND ROUND(a.InvoiceAmount,0) > ROUND(a.TotReceipt,0) " +
            //                "AND PostFlag = 'Y' AND A.InvoiceAmount - A.TotReceipt > 1 ";
            //                //"Order by a.PRNo Desc";
            //                //string[] Columns = new string[] { "b.CusCode", "b.CusDesc", "a.SaleInvNo","ROUND(a.InvoiceAmount, 0) - ROUND(a.TotReceipt, 0)" };
            //                string[] Columns = new string[] { "b.CusCode", "b.CusDesc" };
            //                SysFuncs.GetMultiColumnsDDL(ddlCust, Columns, "CounterSaleMaster a,Customer b", WhereQuery, "CusCode", "Order by b.CusCode Desc", true, false);
            ////                
            //            }
            //            if (ddlPaymentReceiptType.SelectedValue == "DirectSaleAdj")
            //            {
            //                string WhereQuery = "A.DealerCode ='" + Session["DealerCode"].ToString() + "' " +
            //                "AND a.DelFlag = 'N' " +
            //                "AND a.DealerCode = b.DealerCode " +
            //                "AND a.CusCode = b.CusCode " +
            //                "AND ROUND(a.InvoiceAmount,0) > ROUND(a.TotReceipt,0) " +
            //                "AND PostFlag = 'Y' AND A.InvoiceAmount - A.TotReceipt > 1 ";
            //                //"Order by a.PRNo Desc";
            //                //string[] Columns = new string[] { "b.CusCode", "b.CusDesc", "a.SaleInvNo","ROUND(a.InvoiceAmount, 0) - ROUND(a.TotReceipt, 0)" };
            //                string[] Columns = new string[] { "b.CusCode", "b.CusDesc" };
            //                SysFuncs.GetMultiColumnsDDL(ddlCust, Columns, "CounterSaleMaster a,Customer b", WhereQuery, "CusCode", "Order by b.CusCode Desc", true, false);
            //                //                
            //            }
            //            if (ddlPaymentReceiptType.SelectedValue == "Service")
            //            {

            //                string WhereQuery = "A.DealerCode ='" + Session["DealerCode"].ToString() + "' " +
            //                "AND a.DelFlag = 'N' " +
            //                "AND a.DealerCode = b.DealerCode " +
            //                "AND a.CusCode = b.CusCode " +
            //                "AND ROUND(a.NetAmountCustomer,0) > ROUND(a.PaidC,0) " +
            //                "AND A.NetAmountCustomer - A.PaidC > 1 ";
            //                //"Order by a.PRNo Desc";
            //                //string[] Columns = new string[] { "b.CusCode", "b.CusDesc", "a.SaleInvNo","ROUND(a.InvoiceAmount, 0) - ROUND(a.TotReceipt, 0)" };
            //                string[] Columns = new string[] { "b.CusCode", "b.CusDesc" };
            //                SysFuncs.GetMultiColumnsDDL(ddlCust, Columns, "CustomerInvoice a,Customer b", WhereQuery, "CusCode", "Order by b.CusCode Desc", true, false);
            //                //                
            //            }
            //            if (ddlPaymentReceiptType.SelectedValue == "Insurance")
            //            {

            //                string WhereQuery = "A.DealerCode ='" + Session["DealerCode"].ToString() + "' " +
            //                "AND a.DelFlag = 'N' " +
            //                "AND a.DealerCode = b.DealerCode " +
            //                "AND a.CusCode = b.CusCode " +
            //                "AND ROUND(a.NetAmountCustomer,0) > ROUND(a.PaidC,0) " +
            //                "AND A.NetAmountCustomer - A.PaidC > 1 ";
            //                //"Order by a.PRNo Desc";
            //                //string[] Columns = new string[] { "b.CusCode", "b.CusDesc", "a.SaleInvNo","ROUND(a.InvoiceAmount, 0) - ROUND(a.TotReceipt, 0)" };
            //                string[] Columns = new string[] { "b.CusCode", "b.CusDesc" };
            //                SysFuncs.GetMultiColumnsDDL(ddlCust, Columns, "CustomerInvoice a,Customer b", WhereQuery, "CusCode", "Order by b.CusCode Desc", true, false);
            //                //                
            //            }
        }
        private void Load_ReceiptNo()
        {
            
                string WhereQuery = "A.DealerCode ='"+Session["DealerCode"] +"' " +  
            "AND a.DelFlag = 'N' AND a.DealerCode = b.DealerCode " +
           // "And TransType='"+RBLTransType.SelectedValue.ToString()+"'" +
            "AND a.CusCode = b.CusCode "; 
                string[] Columns = new string[] { "a.ReceiptNo", "Convert(Varchar(10),a.ReceiptDate,105)","b.CusDesc" };
                SysFuncs.GetMultiColumnsDDL(ddlReceptNo, Columns, "PaymentReceiptMaster a,Customer b", WhereQuery, "ReceiptNo", "Order by a.ReceiptNo Desc", false, false);
                                
            
    }
        protected void ddlCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadMasterData();
            //clearAll();

            //GridViewRow gvr = (GridViewRow)(sender as Control).Parent.Parent;
            ImageButton Btnadd = (ImageButton) gvTaxDetail.FooterRow.FindControl("Btnadd");
            if(ddlCust.SelectedIndex <= 0)
            {
                Btnadd.Enabled = false;
            }else
            {
                Btnadd.Enabled = true;
            }
            
        }

        protected void ddlAdvance_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPaymentReceiptType.SelectedValue == "Service"|| ddlPaymentReceiptType.SelectedValue == "CountrSale" || ddlPaymentReceiptType.SelectedValue == "Dep")
            {
                String advAmount = SysFuncs.GetStringValuesAgainstCodes("CusCode", ddlCust.SelectedValue.Trim(), "AdvanceBalanceAmount", "PaymentReceiptMaster", "And IsAdjustAdvance='Y' And AmountPaid - AdvanceAdjustedAmount >= 1 and ReceiptNo='" + ddlAdvance.SelectedValue + "' ", Session["DealerCode"].ToString());
                txtAdvAmount.Enabled = true;
                txtAdvAmount.Text = advAmount;

            }
            else if (ddlPaymentReceiptType.SelectedValue=="Insurance")
            {
                String advAmount = SysFuncs.GetStringValuesAgainstCodes("InsCompCode", ddlInsCo.SelectedValue.Trim(), "AdvanceBalanceAmount", "AdvancePayment", "And IsAdjustAdvance='Y' And InsAmount - AdvanceAdjustedAmount >= 1 and AdvanceID='" + ddlAdvance.SelectedValue + "' ", Session["DealerCode"].ToString());
                txtAdvAmount.Enabled = true;
                txtAdvAmount.Text = advAmount;
            }
          
            CalSubTotal();

        }

        

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool chkFlag = false;
            CheckBox chkSelectAll=(CheckBox)gvPendingInvoice.HeaderRow.FindControl("chkSelectAll");
            if (chkSelectAll.Checked)
                chkFlag = true;
            txtSelectedTotalRefAmount.Text = "0.00";
            foreach (GridViewRow dr in gvPendingInvoice.Rows)
            {
                CheckBox chk = (CheckBox)dr.Cells[0].FindControl("chkSelect");
                chk.Checked = chkFlag;

                
             // if (chk.Checked) txtSelectedTotalRefAmount.Text = (Convert.ToDecimal(txtSelectedTotalRefAmount.Text.Trim() == "" ? "0" : txtSelectedTotalRefAmount.Text.Trim()) + Convert.ToDecimal(dr.Cells[5].Text)).ToString();
              //  else txtSelectedTotalRefAmount.Text = "0.00";
               // txtInstAmt.Text = txtSelectedTotalRefAmount.Text;

            }
            txtInvTotal.Text = CaltotalInvAmount().ToString();
        }

        protected void ImgPendingInv_Click(object sender, ImageClickEventArgs e)
        {
            LoadMasterData();
        }

        protected void ddlReceptNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Uzair

            if (ddlReceptNo.SelectedIndex == 0)
            {
                clearAll();
            }
            ddlCust.Enabled = false;
            

            DataTable dt = new DataTable();

            string sql = "select b.CusCode,b.CusDesc from PaymentReceiptMaster a inner join Customer b on a.DealerCode ='" + Session["DealerCode"].ToString() + "' " +
                "AND a.ReceiptNo = '" + ddlReceptNo.SelectedValue + "' " +
                "AND a.CusCode = b.CusCode and a.DealerCode = b.DealerCode";

            dt = SysFuncs.GetData(sql);

            //ddlCust.SelectedValue = dt.Rows[0]["CusCode"].ToString();// +"|"+ dt.Rows[0]["CusDesc"].ToString();
             
            //string[] Columns = new string[] { "b.CusCode", "b.CusDesc" };
            //SysFuncs.GetMultiColumnsDDL(ddlCust, Columns, "PaymentReceiptMaster a,Customer b", WhereQuery, "CusCode", "Order by b.CusCode Desc", true, false);

            LoadMasterData();
        }

        protected void btnGL_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2559", "005"))
            {
                Response.Redirect("~/Test.aspx");
            }
            //if (SysFunctions.CustomCDBL(txtAdvAmount.Text) > 0)
            //{
            //    double InvAdjTOtal = SysFunctions.CustomCDBL(SysFuncs.GetStringValuesAgainstCodes("ReceiptNo", ddlReceptNo.SelectedValue, "InvAdjTotal", "PaymentReceiptMaster", Session["DealerCode"].ToString()));
            //    string AdvReceiptNo = SysFuncs.GetStringValuesAgainstCodes("ReceiptNo", ddlReceptNo.SelectedValue, "AdvanceReceiptNo", "PaymentReceiptMaster", Session["DealerCode"].ToString());
            //    double Advanceamt = SysFunctions.CustomCDBL(SysFuncs.GetStringValuesAgainstCodes("ReceiptNo", AdvReceiptNo, "AdvanceAdjustedAmount", "PaymentReceiptMaster", Session["DealerCode"].ToString()));
            //    if (InvAdjTOtal == Advanceamt)
            //    {
            //        SysFunc.UserMsg(lblMsg, Color.Red, "Voucher will not be generated,Because Advance Amount is equal to Adjustedamount");
            //        return;
            //    }
            //}
            if (SysFunctions.CustomCDBL(txtInstAmt.Text) == 0)
            {
                string URL = "JV.aspx?CusInv=" + ddlReceptNo.SelectedValue + "&Type=AP";
                string fullURL = "window.open('" + URL + "', '_blank', 'height=600,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
            }
            else
            {
                string URL = "GL.aspx?CusInv=" + ddlReceptNo.SelectedValue + "&Type=PR";
                string fullURL = "window.open('" + URL + "', '_blank', 'height=600,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
            }
            
        }

        protected void ddlTransType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTransType.SelectedValue == "Advance")
            {
                
                ddlPaymentReceiptType.Visible = true;
                ddlPaymentReceiptType.Items.Clear();
                imgCustomerCode.Enabled = true;

               // ddlPaymentReceiptType.Items.Add(new ListItem("Sales", "Sales"));
                ddlPaymentReceiptType.Items.Add(new ListItem("Service", "Service"));
                ddlPaymentReceiptType.Items.Add(new ListItem("Direct Sales", "CountrSale"));

            }
            else if (ddlTransType.SelectedValue == "Customer")
            {
               
                ddlPaymentReceiptType.Visible = true;
                ddlPaymentReceiptType.Items.Clear();
                ddlPaymentReceiptType.Items.Add(new ListItem("Sales Order", "SalesOrder"));
                ddlPaymentReceiptType.Items.Add(new ListItem("Cash Service", "Service"));
                ddlPaymentReceiptType.Items.Add(new ListItem("Insurance", "Insurance"));
                ddlPaymentReceiptType.Items.Add(new ListItem("Direct Sales", "CountrSale"));
                ddlPaymentReceiptType.Items.Add(new ListItem("Depreciation ", "Dep"));
                //Uzair
                string WhereQuery = "a.DealerCode = b.DealerCode and a.CusCode = b.CusCode and a.DealerCode ='" + Session["DealerCode"].ToString() + "' ";
                //"Order by a.PRNo Desc";
                //string[] Columns = new string[] { "b.CusCode", "b.CusDesc", "a.SaleInvNo","ROUND(a.InvoiceAmount, 0) - ROUND(a.TotReceipt, 0)" };
                string[] Columns = new string[] { "b.CusCode", "b.CusDesc" };
                SysFuncs.GetMultiColumnsDDL(ddlCust, Columns, "PaymentReceiptMaster a,Customer b", WhereQuery, "CusCode", "Order by b.CusCode Desc", true, false);

            }
            else if (ddlTransType.SelectedValue == "Principle")
            {
                
                ddlPaymentReceiptType.Visible = true;
                ddlPaymentReceiptType.Items.Clear();
                ddlPaymentReceiptType.Items.Add(new ListItem("Free Copon", "FreeCopon"));
                ddlPaymentReceiptType.Items.Add(new ListItem("Warranty", "Warranty"));
                ddlPaymentReceiptType.Items.Add(new ListItem("Insurance", "Insurance"));
                ddlPaymentReceiptType.Items.Add(new ListItem("Adjustment", "Adjustment"));

            }
            else if (ddlTransType.SelectedValue == "SaleReturn")
            {

                ddlPaymentReceiptType.Visible = true;
                ddlPaymentReceiptType.Items.Clear();
                ddlPaymentReceiptType.Items.Add(new ListItem("Customer Return", "CustomerReturn"));

            }
            Load_ReceiptNo();
        }

        protected void ddlPaymentReceiptType_SelectedIndexChanged(object sender, EventArgs e)
        {

            Load_Customer();
            if (ddlPaymentReceiptType.SelectedIndex == 0)
            {

               // chkInsRec.Checked = false;
//chkInsRec.Enabled = false;
                ddlInsCo.SelectedIndex = 0;
                if (ddlInsBranch.Items.Count > 0) ddlInsBranch.SelectedIndex = 0;
                lblInsComp.Visible = false;
                lblInsBr.Visible = false;
                ddlInsCo.Enabled = false;
                ddlInsBranch.Enabled = false;
            }
            else if (ddlPaymentReceiptType.SelectedValue=="Insurance")
            {
                ddlInsCo.Enabled = true;
                ddlInsBranch.Enabled = true;
                // chkInsRec.Enabled = true;
            }
            else
            {
                ddlInsCo.SelectedIndex = 0;
                if (ddlInsBranch.Items.Count > 0) ddlInsBranch.SelectedIndex = 0;
                lblInsComp.Visible = false;
                lblInsBr.Visible = false;
                ddlInsCo.Enabled = false;
                ddlInsBranch.Enabled = false;
            }
            imgCustomerCode.Enabled = true;
        }

        protected void DDLPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLPaymentMode.SelectedValue == "C")
            {
                ddlbank.Visible = false;
                txtBranch.Visible = false;
                txtInstDate.Visible = false;
                txtInstNo.Visible = false;
            }
            else
            {
                ddlbank.Visible = true;
                txtBranch.Visible = true;
                txtInstDate.Visible = true;
                txtInstNo.Visible = true;

            }
        }

        protected void imgCustomerCode_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlTransType.SelectedValue == "Advance")
            {
                ViewState["lookupid"] = 1;
                clslook.LU_Get_Customer(imgCustomerCode, ViewState["lookupid"].ToString(), "DealerCode = '" + Session["DealerCode"].ToString() + "'", "../../../");
            }
            else
            {
                ViewState["lookupid"] = 95;
                clslook.LU_Get_CustomerPayRec(imgCustomerCode, ViewState["lookupid"].ToString(), "DealerCode = '" + Session["DealerCode"].ToString() + "'", ddlPaymentReceiptType.SelectedValue, ddlInsCo.SelectedValue, "../../../");
            }
            

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void imgReceiptNo_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 91;
            clslook.LU_Get_ReceiptNo(imgCustomerCode, ViewState["lookupid"].ToString(), "DealerCode = '" + Session["DealerCode"].ToString() + "'", "../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void txtAdvAmount_TextChanged(object sender, EventArgs e)
        {
            CalSubTotal();
        }

        protected void gvTaxDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalSubTotal();
        }

        protected void lnkInvoiceNo_Click(object sender, EventArgs e)
        {
            LinkButton btnlnk = (LinkButton)sender;
            TableCell tc = btnlnk.Parent as TableCell;
            GridViewRow gvr = (GridViewRow)tc.Parent as GridViewRow;
          
            LinkButton lnk = (LinkButton)gvr.FindControl("lnkInvoiceNo");
            string JobCardCode = gvr.Cells[4].Text;
            Load_CombineInvoiceRpt("",lnk.Text,JobCardCode);
        }

        protected void RBLTransType_SelectedIndexChanged(object sender, EventArgs e)
        {
           
           
        }
        private void LoadTaxType(DropDownList ddl)
        {
            objMBLL.FillDropDown(ddl, "SELECT TaxHead,TaxID FROM TaxType Where Type='Receipt'", "TaxHead", "TaxID", "Select");
        }

        protected void chkAdvance_CheckedChanged(object sender, EventArgs e)
        {
            if(ddlPaymentReceiptType.SelectedValue== "Service"|| ddlPaymentReceiptType.SelectedValue == "CountrSale" || ddlPaymentReceiptType.SelectedValue == "Dep")
            {
                if (ddlCust.SelectedIndex != 0)
                {
                    string WhereQuery = " DealerCode ='" + Session["DealerCode"].ToString() + "' " +
                    "And CusCode='" + ddlCust.SelectedValue.ToString().Trim() + "' " +
                    "And  IsAdjustAdvance='Y' " +
                    "And  TransType='Advance' " +
                    "And   AdvanceBalanceAmount >= 1" +
                    "And   DelFlag<>'Y' ";
                    string[] Columns = new string[] { "ReceiptNo", "CONVERT(VARCHAR(10),ReceiptDate,105)", "'Paid=' + Convert(Varchar(20),amountpaid)", "'Adj=' +CONVERT(VARCHAR(10),AdvanceAdjustedAmount)", "'Bal='+CONVERT(VARCHAR(10),AdvanceBalanceAmount)" };
                    SysFuncs.GetMultiColumnsDDL(ddlAdvance, Columns, "PaymentReceiptMaster", WhereQuery, "ReceiptNo", "Order by ReceiptNo Desc", true, false);
                }

            }
            else
            {
                if (ddlInsCo.SelectedIndex != 0)
                {
                    if (ddlInsBranch.SelectedIndex != 0)
                    {
                        string WhereQuery = " DealerCode ='" + Session["DealerCode"].ToString() + "' " +
                    "And InsCompCode='" + ddlInsCo.SelectedValue.ToString().Trim() + "' " +
                     "And BranchCode='" + ddlInsBranch.SelectedValue.ToString().Trim() + "' " +
                    "And  IsAdjustAdvance='Y' " +

                    "And   AdvanceBalanceAmount >= 1" +
                    "And   DelFlag<>'Y' ";
                        string[] Columns = new string[] { "AdvanceID", "CONVERT(VARCHAR(10),AdvanceDate,105)", "'Paid=' + Convert(Varchar(20),InsAmount)", "'Adj=' +CONVERT(VARCHAR(10),AdvanceAdjustedAmount)", "'Bal='+CONVERT(VARCHAR(10),AdvanceBalanceAmount)" };
                        SysFuncs.GetMultiColumnsDDL(ddlAdvance, Columns, "AdvancePayment", WhereQuery, "AdvanceID", "Order by AdvanceID Desc", true, false);

                    }

                }

            }
         
          
        }

        protected void DDLFooterReceiptHead_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList DDLFooterReceiptHead= (DropDownList) gvTaxDetail.FooterRow.FindControl("DDLFooterReceiptHead");
            TextBox txtFooterTaxPerc = (TextBox)gvTaxDetail.FooterRow.FindControl("txtFooterTaxPerc");
            string strTaxPerc = SysFuncs.GetStringValuesAgainstCodes("TaxId", DDLFooterReceiptHead.SelectedValue.ToString().Trim(),"TaxPer", "TaxType", "", Session["DealerCode"].ToString());
            txtFooterTaxPerc.Text = strTaxPerc;

            //Calculate Tax Percentage
            decimal TotalInvAmount = Convert.ToDecimal(txtSelectedTotalRefAmount.Text.Trim() == "" ? "0" : txtSelectedTotalRefAmount.Text.Trim());
            decimal TaxPercent = Convert.ToDecimal(txtFooterTaxPerc.Text.Trim() == "" ? "0" : txtFooterTaxPerc.Text.Trim());


            var TotalTax = (TotalInvAmount * TaxPercent) / 100;
            TextBox txtFooterTaxAmt = (TextBox)gvTaxDetail.FooterRow.FindControl("txtFooterTaxAmt");
            txtFooterTaxAmt.Text = string.Format("{0:F2}", TotalTax);
            /////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            //txtFooterTaxPerc.Focus();
        }

        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                CheckBox Chk = sender as CheckBox;
                TableCell tc = Chk.Parent as TableCell;
                GridViewRow gvr = tc.Parent as GridViewRow;
                CheckBox chkSelect = (CheckBox)gvr.Cells[0].FindControl("chkSelect");

                //if (chkSelect.Checked) {
                //    if (SysFunctions.CustomCDBL(gvr.Cells[6].Text) < SysFunctions.CustomCDBL(txtAdvAmount.Text))
                //    {
                //        txtSelectedTotalRefAmount.Text = "0";
                //    }
                //    else
                //    {
                //        txtSelectedTotalRefAmount.Text = (Convert.ToDecimal(txtSelectedTotalRefAmount.Text.Trim() == "" ? "0" : txtSelectedTotalRefAmount.Text.Trim()) + Convert.ToDecimal(gvr.Cells[6].Text) - Convert.ToDecimal(txtAdvAmount.Text.Trim() == "" ? "0" : txtAdvAmount.Text.Trim())).ToString();
                //   }

                //    }
                //   else
                //txtSelectedTotalRefAmount.Text = (Convert.ToDecimal(txtSelectedTotalRefAmount.Text.Trim() == "" ? "0" : txtSelectedTotalRefAmount.Text.Trim()) + Convert.ToDecimal(gvr.Cells[6].Text)).ToString();
                // txtInstAmt.Text = txtSelectedTotalRefAmount.Text;
                //             txttotAmount.Text = txtSelectedTotalRefAmount.Text;
                txtInvTotal.Text = CaltotalInvAmount().ToString();
            }
            catch (Exception ex)
            {
                SysFuncs.UserMsg(lblMsg, Color.Red, ex.Message);
            }


        }

        protected void txtFooterTaxPerc_TextChanged(object sender, EventArgs e)
        {
            try{
            TextBox txtFooterTaxPerc=(TextBox)gvTaxDetail.FooterRow.FindControl("txtFooterTaxPerc");
                //txtSelectedTotalRefAmount
                if (gvPendingInvoice.Rows.Count > 0)
                {



                    foreach (GridViewRow row in gvPendingInvoice.Rows)
                    {
                        CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                        if (chkSelect.Checked)
                        {
                            SumOfSelectedInvoice = SumOfSelectedInvoice + Convert.ToDouble(row.Cells[6].Text);
                        }
                    }
                }
                    decimal TotalInvAmount= Convert.ToDecimal (SumOfSelectedInvoice == 0 ? 0 : SumOfSelectedInvoice);
            decimal TaxPercent=Convert.ToDecimal (txtFooterTaxPerc.Text .Trim() == "" ? "0" : txtFooterTaxPerc.Text .Trim());


            var TotalTax = (TotalInvAmount * TaxPercent) / 100;
           TextBox txtFooterTaxAmt=(TextBox)gvTaxDetail.FooterRow.FindControl("txtFooterTaxAmt");
           txtFooterTaxAmt.Text = string.Format("{0:F2}", TotalTax);
            }
            catch (Exception ex)
            {
                SysFuncs.UserMsg (lblMsg ,Color .Red,ex.Message );
            }
            //document.getElementById('<%=((TextBox)gvTaxDetail.FooterRow.FindControl("txtFooterTaxAmt")).ClientID %>').value = parseFloat(TotalTax).toFixed(2).toString();
        }

        protected void txtFooterTaxAmt_TextChanged(object sender, EventArgs e)
        {
            try{
             TextBox txtFooterTaxAmt=(TextBox)gvTaxDetail.FooterRow.FindControl("txtFooterTaxAmt");
                if (gvPendingInvoice.Rows.Count > 0)
                {



                    foreach (GridViewRow row in gvPendingInvoice.Rows)
                    {
                        CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                        if (chkSelect.Checked)
                        {
                            SumOfSelectedInvoice = SumOfSelectedInvoice + Convert.ToDouble(row.Cells[6].Text);
                        }
                    }
                }
                decimal TotalInvAmount= Convert.ToDecimal (SumOfSelectedInvoice == 0 ? 0 : SumOfSelectedInvoice);
            decimal DFooterTaxAmt=Convert.ToDecimal (txtFooterTaxAmt.Text .Trim() == "" ? "0" : txtFooterTaxAmt.Text .Trim());

           var TotalTax = (DFooterTaxAmt / TotalInvAmount) * 100;
           TextBox txtFooterTaxPerc = (TextBox)gvTaxDetail.FooterRow.FindControl("txtFooterTaxPerc");
           txtFooterTaxPerc.Text = string.Format("{0:F2}", TotalTax);
            }
            catch (Exception ex)
            {
                SysFuncs.UserMsg (lblMsg ,Color .Red,ex.Message );
            }
        }
        private void Load_CombineInvoiceRpt(string rpt,string Invoice,string JobCard)
        {
            ReportDocument RD = new ReportDocument();
            Data.DSReports data = new Data.DSReports();
            if (ddlPaymentReceiptType.SelectedValue == "Insurance")
            {
                rpt = "~\\Modules\\Service\\ServiceReports\\Ins_CombineReport.rpt";
            }
            else
            {
                rpt = "~\\Modules\\Service\\ServiceReports\\CombineReport.rpt";
            }
           
            DataSet ds = new DataSet();

            string cCon = CConn.CConnection.GetConnectionString();

            string i = Invoice;
            string jobtype=SysFuncs.GetStringValuesAgainstCodes("JobCardCode='" + JobCard + "' and DealerCode='"+Session["DealerCode"].ToString()+"'", "JobCardMaster", "JobCardType");
            //   QRCodeEncoder encoder = new QRCodeEncoder();

            if (ddlPaymentReceiptType.SelectedValue == "Insurance")
            {
                ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_CustomerInvoiceInfo_Select'" + this.Session["DealerCode"].ToString() + "','" + i + "'");
                data.sp_CustomerInvoiceInfo_Select.Load(ds.CreateDataReader());

                // Bitmap bi = encoder.Encode(ds.Tables[0].Rows[0]["DealerCode"].ToString() + "" + ds.Tables[0].Rows[0]["InvoiceNo"].ToString().Trim() + "" + ds.Tables[0].Rows[0]["CusDesc"].ToString() + "" + ds.Tables[0].Rows[0]["NetAmountInsurance"].ToString());
                // bi.Save(Server.MapPath("~/Images/QrCode.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "SP_CustomerInvoice_Combine'" + this.Session["DealerCode"].ToString() + "','" + i + "'");
                data.SP_CustomerInvoice_Combine.Load(ds.CreateDataReader());

                //  Bitmap bi = encoder.Encode(ds.Tables[0].Rows[0]["DealerCode"].ToString() + "" + ds.Tables[0].Rows[0]["InvoiceNo"].ToString().Trim() + "" + ds.Tables[0].Rows[0]["CusDesc"].ToString() + "" + ds.Tables[0].Rows[0]["NetAmountCustomer"].ToString());
                //  bi.Save(Server.MapPath("~/Images/QrCode.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
            }



            string strJCCode = ds.Tables[0].Rows[0]["JobCardCode"].ToString();
            string InvType = ds.Tables[0].Rows[0]["InvType"].ToString();

            ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_JobCardDetail_Print '" + this.Session["DealerCode"].ToString() + "','" + strJCCode + "'");
            data.sp_JobCardDetail_Print.Load(ds.CreateDataReader());

            ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_JobCardButPartsDetail_Print '" + this.Session["DealerCode"].ToString() + "','" + strJCCode + "'");
            data.sp_JobCardLubDetail_Print.Load(ds.CreateDataReader());

            if (ddlPaymentReceiptType.SelectedValue == "Insurance")
            {
                ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_DepCust_GSTInvoiceParts_Select'" + this.Session["DealerCode"].ToString() + "','" + strJCCode + "','" + InvType + "'"); //
                data.sp_DepCust_GSTInvoiceParts_Select.Load(ds.CreateDataReader());
            }
            else
            {
                ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_JobCardPartsDetail_Print'" + this.Session["DealerCode"].ToString() + "','" + strJCCode + "'");
                data.sp_JobCardPartsDetail_Print.Load(ds.CreateDataReader());
            }


            ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_JobCardSubletDetail_Print'" + this.Session["DealerCode"].ToString() + "','" + strJCCode + "'");
            data.sp_JobCardSubletDetail_Print.Load(ds.CreateDataReader());

            //RD.Load(Server.MapPath("~\\Reports\\GRN_SSTInvoiceReport.rpt"));
            RD.Load(Server.MapPath(rpt));


            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'COMBINE INVOICE'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            RD.DataDefinition.FormulaFields["QRCode"].Text = "'" + Server.MapPath("~") + "/Images/QrCode.jpg'";
            RD.Database.Tables[0].SetDataSource(data);

            // create pdf 
            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "InvoicePST" + Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;

            //RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);
            Session["RD"] = RD;

            string URL;
            URL = "../../../Download/rptViewerService.aspx?FileName=" + FileName;
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

        }
      

    }
}