using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace DXBMS.Modules.SpareParts.Forms
{
    public partial class PayableInvoice : System.Web.UI.Page
    {
        Transaction ObjTrans = new Transaction();
        SysFunctions SysFunc = new SysFunctions();
        SecurityBll sec = new SecurityBll();
        MainBLL ObjMain = new MainBLL();
        clsLookUp clslook = new clsLookUp();
        static int btnValue = 0;
        int TotalQantity = 0;
        int TotalPayableQty = 0;
        int TotalPaidQty = 0;
        decimal TotalDiscount = 0;
        decimal GrossTotal = 0;
        string checkboxOnOrOff = "Y";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2544", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (Page.IsPostBack)
            {

                if (!string.IsNullOrEmpty(Session["LookUpData"].ToString()))
                {
                    SelectedPartDetail(Session["LookUpData"].ToString());
                }
            }
            if (IsPostBack == false)
            {

                if (Session["TransCode"] != null)
                {
                    txtPayInvDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    ObjMain.FillDropDown(ddlVendor, "select VendorCode,VendorDesc from Vendor Where DealerCode in('" + Session["DealerCode"].ToString() + "','COMON')", "VendorDesc", "VendorCode", "Select");
                    createPartsDT();
                    Load_ddlPurInvNo();
                    string leadId = Session["TransCode"].ToString();
                    ddlPayInvNo.SelectedValue = leadId;

                    ddlPayInvNo_SelectedIndexChanged(null, null);


                }
                else
                {
                    lblMode.Text = "Add Mode";
                    txtPayInvDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    ObjMain.FillDropDown(ddlVendor, "select VendorCode,VendorDesc from Vendor Where DealerCode in('" + Session["DealerCode"].ToString() + "','COMON')", "VendorDesc", "VendorCode", "Select");
                    createPartsDT();
                    Load_ddlPurInvNo();
                }
            }
            Session["TransCode"] = null;
            Session["LookUpData"] = string.Empty;
        }
        private void LoadGRN_DDL(string strGRNNo)
        {
            if (ddlPayInvNo.SelectedIndex == 0)
            {
                string WhereQuery = "A.DealerCode =  '" + Session["DealerCode"].ToString() + "' " +
                    "and A.DCNo=B.DCNo And A.DealerCode=B.DealerCode " +
      "and A.DelFlag = 'N' " +
     "and A.VendorCode = '" + ddlVendor.SelectedValue.ToString() + "'" +
     "And A.GRNNo in(Select GRNNo From GRNDetail Where DealerCode ='" + Session["DealerCode"].ToString() + "' And AccQty <> PayablePaidQty)";
                //"Order by a.PRNo Desc";
                string[] Columns = new string[] { "A.GRNNo", "convert(varchar(20),A.GRnDate,105)", "'PO: '+A.PoNo", "'Vend DCNo: ' +B.VendorDCNo" };
                SysFunc.GetMultiColumnsDDL(ddlGRN, Columns, "GRNMaster A , DCMaster B", WhereQuery, "GRNNo", "Order by A.GRNNo Desc", false, false);
            }
            else
            {
                string WhereQuery = "A.DealerCode =  '" + Session["DealerCode"].ToString() + "' " +
                    "and A.DCNo=B.DCNo And A.DealerCode=B.DealerCode " +
      "and A.DelFlag = 'N' " +
     "and A.VendorCode = '" + ddlVendor.SelectedValue.ToString() + "'" +
     "And A.GRNNo ='" + strGRNNo + "'";
                //"Order by a.PRNo Desc";
                string[] Columns = new string[] { "A.GRNNo", "convert(varchar(20),A.GRnDate,105)", "'PO: '+A.PoNo", "'Vend DCNo: ' +B.VendorDCNo" };
                SysFunc.GetMultiColumnsDDL(ddlGRN, Columns, "GRNMaster A , DCMaster B", WhereQuery, "GRNNo", "Order by A.GRNNo Desc", false, false);
            }
        }
        private void Load_ddlPurInvNo()
        {
            string WhereClause = " DealerCode = '" + Session["DealerCode"].ToString() + "'" +
                 " and DelFlag = 'N' ";

            string[] Columns = new string[] { "PurInvNo", "convert(varchar(10),PurInvDate,105)", "'GRN: '+GRNNo", "'Vend DCNo: '+VendorDCNo" };
            SysFunc.GetMultiColumnsDDL(ddlPayInvNo, Columns, "PurInvMaster", WhereClause, "PurInvNo", " Order by PurInvNo Desc ", false, false);
        }

        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPayInvNo.SelectedIndex == 0)
            {
                LoadGRN_DDL(string.Empty);
            }
        }

        protected void ddlGRN_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                Load_Detail("sp_Select_GRNDetail_on_PayableInvForm", ddlGRN.SelectedValue.ToString().Trim(), "@GRNNo");
                CalcTotals();
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }

        }
        private void CalcTotals()
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = {

                                       new SqlParameter("@DealerCode",SqlDbType.Char),
                                       new SqlParameter("@GRNNo",SqlDbType.Char),
                                  };
            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = ddlGRN.SelectedValue.ToString().Trim();
            //Load_Detail("sp_Select_GRNDetail_on_PayableInvForm", ddlGRN.SelectedValue.ToString().Trim(), "@GRNNo");
            ds = SysFunc.FillDataSet("sp_Select_GRNDetail_on_PayableInvForm", param);
            decimal SumOfGSTAmt = 0;
            decimal SumOfFurTaxAmt = 0;
            decimal SumOfExTaxAmt = 0;
            foreach (GridViewRow gvr in gvPurInvDetail.Rows)
            {
                CheckBox chkSelect = (CheckBox)gvr.Cells[0].FindControl("chkSelect");
                Label lblGrossAmt = (Label)gvr.Cells[10].FindControl("lblGrossAmt");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (chkSelect.Checked == true)
                    {
                        if (dr["ItemCode"].ToString() == gvr.Cells[1].Text)
                        {
                            SumOfGSTAmt = SumOfGSTAmt + ((Convert.ToDecimal(lblGrossAmt.Text) * Convert.ToDecimal(dr["GSTPercent"].ToString())) / 100);
                            SumOfExTaxAmt = SumOfExTaxAmt + ((Convert.ToDecimal(lblGrossAmt.Text) * Convert.ToDecimal(dr["ExTaxPercent"].ToString())) / 100);
                            SumOfFurTaxAmt = SumOfFurTaxAmt + ((Convert.ToDecimal(lblGrossAmt.Text) * Convert.ToDecimal(dr["FurTaxPercent"].ToString())) / 100);
                        }
                    }
                }

            }
            lblGstPerc.Text = ds.Tables[0].Rows[0]["GSTPercent"].ToString() + "%";
            lblFurTaxPerc.Text = ds.Tables[0].Rows[0]["FurTaxPercent"].ToString() + "%";
            lblExTaxPerc.Text = ds.Tables[0].Rows[0]["ExTaxPercent"].ToString() + "%";
            txtGstAmt.Text = SumOfGSTAmt.ToString(("#.##"));
            txtExTaxAmt.Text = SumOfExTaxAmt.ToString(("#.##"));
            txtFurTaxAmt.Text = SumOfFurTaxAmt.ToString(("#.##"));
            Label lblFooterGrossAmt = (Label)gvPurInvDetail.FooterRow.Cells[10].FindControl("lblFooterGrossAmt");
            txtTotalAmt.Text = (SumOfGSTAmt + SumOfExTaxAmt + SumOfFurTaxAmt + Convert.ToDecimal(lblFooterGrossAmt.Text)).ToString();
        }
        private void Load_Detail(string sp_Name, string Code, string CodeParamName)
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = {

                                       new SqlParameter("@DealerCode",SqlDbType.Char),
                                       new SqlParameter(CodeParamName,SqlDbType.Char),
                                  };
            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = Code;
            ds = SysFunc.FillDataSet(sp_Name, param);
            gvPurInvDetail.DataSource = ds.Tables[0];
            gvPurInvDetail.DataBind();
            Session["dsParts"] = ds;

        }

        protected void gvPurInvDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkSelect = (CheckBox)e.Row.Cells[0].FindControl("chkSelect");
                TextBox txtPayableQty = (TextBox)e.Row.Cells[6].FindControl("txtPayableQty");
                Label lblDiscountAmt = (Label)e.Row.Cells[9].FindControl("lblDiscountAmt");
                Label lblGrossAmt = (Label)e.Row.Cells[10].FindControl("lblGrossAmt");
                if (checkboxOnOrOff == "Y") chkSelect.Checked = true;
                //if (txtPayableQty.Text.Trim() == string.Empty || txtPayableQty.Text.Trim() == "0")
                //{
                //    SysFunc.UserMsg(LbErr, Color.Red, "Paid Quantity can not be zero(0) or Empty");
                //    return;
                //}
                if (chkSelect.Checked == true)
                {
                    TotalQantity = TotalQantity + Convert.ToInt32(e.Row.Cells[4].Text);
                    TotalPayableQty = TotalPayableQty + Convert.ToInt32(e.Row.Cells[5].Text); //Convert.ToDecimal((e.Row.Cells[5].Text == "" ? "0" : e.Row.Cells[5].Text));
                    TotalPaidQty = TotalPaidQty + Convert.ToInt32(txtPayableQty.Text);
                    TotalDiscount = TotalDiscount + Convert.ToDecimal(lblDiscountAmt.Text.Trim()) * Convert.ToDecimal(txtPayableQty.Text.Trim());
                    GrossTotal = GrossTotal + Convert.ToDecimal(lblGrossAmt.Text.Trim());
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

                Label lblPayableQty = (Label)e.Row.Cells[6].FindControl("lblPayableQty");
                Label lblDicountTotal = (Label)e.Row.Cells[9].FindControl("lblDicountTotal");
                Label lblFooterGrossAmt = (Label)e.Row.Cells[10].FindControl("lblFooterGrossAmt");
                e.Row.Cells[4].Text = TotalQantity.ToString();
                e.Row.Cells[5].Text = TotalPayableQty.ToString();
                lblPayableQty.Text = TotalPaidQty.ToString();
                lblDicountTotal.Text = TotalDiscount.ToString();
                lblFooterGrossAmt.Text = GrossTotal.ToString();
                ViewState["TotalDiscount"] = TotalDiscount.ToString();
                ViewState["TotalAmt"] = GrossTotal.ToString();
                ViewState["TotalPaidQty"] = TotalPaidQty.ToString();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            createPartsDT();
            //SysFunc.ClearTextBoxes(Page.Parent);
            txtOrder.Text = string.Empty;
            txtPayInvDate.Text = string.Empty;
            txtPrincipalInvDate.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            txtExTaxAmt.Text = "0.00";
            txtFurTaxAmt.Text = "0.00";
            txtExTaxAmt.Text = "0.00";
            txtPaymentTerms.Text = string.Empty;
            lblExTaxPerc.Text = string.Empty;
            lblFurTaxPerc.Text = string.Empty;
            lblGstPerc.Text = string.Empty;
            ddlVendor.SelectedIndex = 0;
            txtGstAmt.Text = string.Empty;
            txtFurTaxAmt.Text = string.Empty;
            txtExTaxAmt.Text = string.Empty;
            txtTotalAmt.Text = string.Empty;


            ddlGRN.Items.Clear();

            Load_ddlPurInvNo();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);

        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2544", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            SqlTransaction Trans = null;
            bool Chek = false;
            bool chkSelectRow = false;
            string strPurInvNo = string.Empty;
            try
            {
                if (ddlPayInvNo.SelectedIndex > 0)
                {
                    if (!sec.UserRight("2544", "003"))
                    {
                        Response.Redirect("~/Test.aspx");
                    }
                    SysFunc.UserMsg(LbErr, Color.Red, "Update not allowd please delete and save again");
                    return;
                }
                if (ddlVendor.SelectedIndex == 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Select Vendor");
                    return;
                }

                if (ddlGRN.SelectedIndex == 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Select GRN Number.");
                    return;
                }
                if (txtPrincipalInvNo.Text.Trim() == string.Empty)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Principal Inv No can not be empty");
                    return;
                }
                if (txtPayInvDate.Text.Trim() == string.Empty)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Principal Inv Date can not be empty");
                    return;
                }
                if (txtOrder.Text.Trim() == string.Empty)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Order No can not be empty");
                    return;
                }
                foreach (GridViewRow gvr in gvPurInvDetail.Rows)
                {
                    CheckBox chkSelect = (CheckBox)gvr.Cells[0].FindControl("chkSelect");
                    //TextBox txtPayableQty = (TextBox)gvr.Cells[6].FindControl("txtPayableQty");
                    if (chkSelect.Checked == true)
                    {
                        chkSelectRow = true;
                        break;
                    }
                    else
                    {
                        chkSelectRow = false;
                    }

                }
                if (chkSelectRow == false)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Select Rows from list");
                    return;
                }
                foreach (GridViewRow gvr in gvPurInvDetail.Rows)
                {
                    CheckBox chkSelect = (CheckBox)gvr.Cells[0].FindControl("chkSelect");
                    TextBox txtPayableQty = (TextBox)gvr.Cells[6].FindControl("txtPayableQty");
                    decimal dtxtPayableQty = Convert.ToDecimal(txtPayableQty.Text == "" ? "0" : txtPayableQty.Text);
                    decimal dAccQty = Convert.ToDecimal(gvr.Cells[4].Text.Trim());
                    decimal dPaidQty = Convert.ToDecimal(gvr.Cells[5].Text.Trim());
                    decimal dRemainingQty = dAccQty - dPaidQty;
                    if (chkSelect.Checked == true && dtxtPayableQty == 0)
                    {
                        SysFunc.UserMsg(LbErr, Color.Red, "Payable quantity can not be Zero(0) or empty.");

                        return;
                    }
                    if (chkSelect.Checked == true && dtxtPayableQty > dRemainingQty)
                    {
                        chkSelect.Checked = false;
                        SysFunc.UserMsg(LbErr, Color.Red, "Remaining payable Qty is " + dRemainingQty + " you can not enter greater then Remaining Qty");
                        return;
                    }
                }

                if (ObjTrans.BeginTransaction(ref Trans) == false)
                {
                    return;
                }

                SqlParameter[] param =
                                    {
                                         new SqlParameter("@DealerCode",SqlDbType.Char, 5),//0
                                         new SqlParameter("@PurInvNo",SqlDbType.Char, 8),//1
                                         new SqlParameter("@PurInvDate",SqlDbType.DateTime),//2
                                         new SqlParameter("@VendorCode",SqlDbType.Char,6),//3
                                         new SqlParameter("@GRNNo",SqlDbType.Char, 8),//4
                                         new SqlParameter("@VendorInvNo",SqlDbType.VarChar, 20),//5
                                         new SqlParameter("@VendorInvDate",SqlDbType.DateTime),//6
                                         new SqlParameter("@OrderNo",SqlDbType.VarChar,20),//7
                                         new SqlParameter("@DiscountAmt",SqlDbType.Decimal),//8
                                         new SqlParameter("@Remarks",SqlDbType.VarChar,70),//9
                                         new SqlParameter("@TotalAmt",SqlDbType.Decimal),//10
                                         new SqlParameter("@TotPayment",SqlDbType.Decimal),//11
                                         new SqlParameter("@UpdUser",SqlDbType.VarChar),//12
                                         new SqlParameter("@UpdTerm",SqlDbType.VarChar),//13
                                         new SqlParameter("@TotPaidQty",SqlDbType.Int ),//14
                                         new SqlParameter("@GSTPercent",SqlDbType.Decimal),//15
                                         new SqlParameter("@GSTTotal",SqlDbType.Decimal),//16
                                         new SqlParameter("@ExTaxPercent",SqlDbType.Decimal),//17
                                         new SqlParameter("@ExTaxTotal",SqlDbType.Decimal),//18
                                         new SqlParameter("@FurTaxPercent",SqlDbType.Decimal),//19
                                         new SqlParameter("@FurTaxTotal",SqlDbType.Decimal),//20
                                         new SqlParameter("@TotalIncTax",SqlDbType.Decimal),//21
                                    };
                param[0].Value = Session["DealerCode"].ToString();

                if (ddlPayInvNo.SelectedIndex == 0)
                {
                    strPurInvNo = SysFunc.AutoGen("PurInvMaster", "PurInvNo", DateTime.Parse(SysFunc.SaveDate(txtPayInvDate.Text).ToString()).ToString("dd-MM-yyyy"));
                }
                else
                {
                    if (!sec.UserRight("2544", "003"))
                    {
                        Response.Redirect("~/Test.aspx");
                    }
                    strPurInvNo = ddlPayInvNo.SelectedValue;
                }
                param[1].Value = strPurInvNo.Trim();
                param[2].Value = SysFunc.SaveDate(txtPayInvDate.Text);
                param[3].Value = ddlVendor.SelectedValue.ToString().Trim();
                param[4].Value = ddlGRN.SelectedValue.ToString().Trim();
                param[5].Value = txtPrincipalInvNo.Text.Trim();//txtRemarks.Text.Trim().Replace(",", " ");
                param[6].Value = SysFunc.SaveDate(txtPrincipalInvDate.Text);//Convert.ToDecimal((lblFooterTotalAmt.Text == "" ? "0" : lblFooterTotalAmt.Text));
                param[7].Value = txtOrder.Text.Trim();
                param[8].Value = Convert.ToDecimal(ViewState["TotalDiscount"].ToString() == "" ? "0" : ViewState["TotalDiscount"].ToString());
                param[9].Value = txtRemarks.Text.Trim().Replace(",", " ");
                param[10].Value = Convert.ToDecimal(ViewState["TotalAmt"].ToString() == "" ? "0" : ViewState["TotalAmt"].ToString());
                param[11].Value = "0";//Convert.ToDecimal(ViewState["TotalAmt"].ToString() == "" ? "0" : ViewState["TotalAmt"].ToString());
                param[12].Value = Session["UserName"].ToString();
                param[13].Value = GlobalVar.mUserIPAddress.ToString();
                param[14].Value = Convert.ToDecimal(ViewState["TotalPaidQty"].ToString() == "" ? "0" : ViewState["TotalPaidQty"].ToString());
                param[15].Value = Convert.ToDecimal(lblGstPerc.Text.Trim().Replace("%", ""));
                param[16].Value = Convert.ToDecimal(txtGstAmt.Text.Trim() == "" ? "0" : txtGstAmt.Text.Trim());
                param[17].Value = Convert.ToDecimal(lblExTaxPerc.Text.Trim().Replace("%", ""));
                param[18].Value = Convert.ToDecimal(txtExTaxAmt.Text.Trim() == "" ? "0" : txtExTaxAmt.Text.Trim());
                param[19].Value = Convert.ToDecimal(lblFurTaxPerc.Text.Trim().Replace("%", ""));
                param[20].Value = Convert.ToDecimal(txtFurTaxAmt.Text.Trim() == "" ? "0" : txtFurTaxAmt.Text.Trim());
                param[21].Value = Convert.ToDecimal(txtTotalAmt.Text.Trim() == "" ? "0" : txtTotalAmt.Text.Trim());
                if (SysFunc.sp_PurInvMaster_Insert(Trans, param))
                {
                    foreach (GridViewRow gvr in gvPurInvDetail.Rows)
                    {
                        TextBox txtPayableQty = (TextBox)gvr.Cells[6].FindControl("txtPayableQty");
                        Label lblDiscountAmt = (Label)gvr.Cells[9].FindControl("lblDiscountAmt");
                        Label lblGrossAmt = (Label)gvr.Cells[10].FindControl("lblGrossAmt");
                        CheckBox chkSelect = (CheckBox)gvr.Cells[0].FindControl("chkSelect");
                        if (chkSelect.Checked)
                        {
                            SqlParameter[] param1 =
                      {
                         new SqlParameter("@DealerCode",SqlDbType.Char, 10),//0
                         new SqlParameter("@GRNNo",SqlDbType.Char, 8),//1
                         new SqlParameter("@PurInvNo",SqlDbType.Char, 8),//2
                         new SqlParameter("@ItemCode",SqlDbType.Char, 8),//3
                         new SqlParameter("@AccQty",SqlDbType.Decimal),//4
                         new SqlParameter("@paidQty",SqlDbType.Decimal),//5
                         new SqlParameter("@Rate",SqlDbType.Decimal),//6
                         new SqlParameter("@DiscountAmt",SqlDbType.Decimal),//7
                         new SqlParameter("@Amount",SqlDbType.Decimal),//8
                         new SqlParameter("@DiscountPerc",SqlDbType.Decimal),//9

                      };
                            param1[0].Value = Session["DealerCode"].ToString();
                            param1[1].Value = ddlGRN.SelectedValue.ToString();
                            param1[2].Value = strPurInvNo;
                            param1[3].Value = gvr.Cells[1].Text.Trim();
                            param1[4].Value = Convert.ToDecimal(gvr.Cells[4].Text.Trim() == "" ? "0" : gvr.Cells[4].Text.Trim());
                            param1[5].Value = Convert.ToDecimal(txtPayableQty.Text.Trim() == "" ? "0" : txtPayableQty.Text.Trim());
                            param1[6].Value = Convert.ToDecimal(gvr.Cells[7].Text.Trim() == "" ? "0" : gvr.Cells[7].Text.Trim());
                            param1[7].Value = Convert.ToDecimal(lblDiscountAmt.Text.Trim() == "" ? "0" : lblDiscountAmt.Text.Trim());
                            param1[8].Value = Convert.ToDecimal(lblGrossAmt.Text.Trim() == "" ? "0" : lblGrossAmt.Text.Trim());
                            param1[9].Value = Convert.ToDecimal(gvr.Cells[8].Text.Trim() == "" ? "0" : gvr.Cells[8].Text.Trim());
                            if (SysFunc.sp_PurInvDetail_Insert(Trans, param1))
                            {
                                Chek = true;
                            }
                            else
                            {
                                Chek = false;
                                break;
                            }


                        }
                    }
                    if (Chek == false)
                    {
                        ObjTrans.RollBackTransaction(ref Trans);
                        SysFunc.UserMsg(LbErr, Color.Red, "Recored not saved try again  ");
                    }
                    else
                    {
                        ObjTrans.CommittTransaction(ref Trans);
                        SysFunc.UserMsg(LbErr, Color.Green, "Saved Successfully Sales Invoice No. " + strPurInvNo);
                        btnClear_Click(BtnSave, EventArgs.Empty);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);
                    }
                }

            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }
        private void createPartsDT()
        {
            DataSet dsGVMain = new DataSet();
            dsGVMain.Tables.Add();
            dsGVMain.Tables[0].Columns.Add(new DataColumn("CheckBox", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("ItemCode", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("PartItemNo", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("ItemDesc", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("AccQty", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("PayableQty", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("PaidQty", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("Rate", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("DiscountPerc", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("DiscountAmt", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("Amount", typeof(string)));

            //DataRow dr = dsGVMain.Tables[0].NewRow();
            //dsGVMain.Tables[0].Rows.Add(dr);

            gvPurInvDetail.DataSource = dsGVMain.Tables[0];
            gvPurInvDetail.DataBind();
            Session["gvPurInvDetail"] = dsGVMain.Tables[0];
        }

        protected void ddlPayInvNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMode.Text = "Edit Mode";
            DataSet ds = new DataSet();
            SqlParameter[] param =
                      {
                         new SqlParameter("@DealerCode",SqlDbType.Char, 10),//0
                         new SqlParameter("@PurInvNo",SqlDbType.Char, 8),//1

                      };
            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = ddlPayInvNo.SelectedValue.ToString();
            ds = SysFunc.FillDataSet("sp_PurInvMaster_Select", param);
            ddlVendor.SelectedValue = ds.Tables[0].Rows[0]["VendorCode"].ToString().Trim();
            txtPayInvDate.Text = ds.Tables[0].Rows[0]["PurInvDate"].ToString().Trim();
            txtVoucherNo.Text = ds.Tables[0].Rows[0]["VoucherNo"].ToString().Trim();
            txtPrincipalInvNo.Text = ds.Tables[0].Rows[0]["VendorInvNo"].ToString().Trim();
            txtPrincipalInvDate.Text = ds.Tables[0].Rows[0]["VendorInvDate"].ToString().Trim();
            txtOrder.Text = ds.Tables[0].Rows[0]["OrderNo"].ToString().Trim();
            txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString().Trim();
            lblGstPerc.Text = ds.Tables[0].Rows[0]["GSTPercent"].ToString().Trim() + "%";
            txtGstAmt.Text = ds.Tables[0].Rows[0]["GSTTotal"].ToString().Trim();
            lblExTaxPerc.Text = ds.Tables[0].Rows[0]["ExTaxPercent"].ToString().Trim() + "%";
            txtExTaxAmt.Text = ds.Tables[0].Rows[0]["ExTaxTotal"].ToString().Trim();
            lblFurTaxPerc.Text = ds.Tables[0].Rows[0]["FurTaxPercent"].ToString().Trim() + "%";
            txtFurTaxAmt.Text = ds.Tables[0].Rows[0]["FurTaxTotal"].ToString().Trim();
            txtTotalAmt.Text = ds.Tables[0].Rows[0]["TotalIncTax"].ToString().Trim();


            //txtPayInvDate.Text = ds.Tables[0].Rows[0]["VendorCode"].ToString().Trim());

            Load_Detail("sp_PurInvDetail_Select", ddlPayInvNo.SelectedValue.ToString().Trim(), "@PurInvNo");
            LoadGRN_DDL(ds.Tables[0].Rows[0]["GRNNo"].ToString().Trim());
            ddlGRN.SelectedIndex = 1;
        }

        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox BtnLnk = (CheckBox)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;

                CheckBox chkSelect = (CheckBox)rowGv.Cells[0].FindControl("chkSelect");
                TextBox txtPayableQty = (TextBox)rowGv.Cells[6].FindControl("txtPayableQty");
                Label lblDiscountAmt = (Label)rowGv.Cells[9].FindControl("lblDiscountAmt");
                Label lblGrossAmt = (Label)rowGv.Cells[10].FindControl("lblGrossAmt");
                decimal dtxtPayableQty = Convert.ToDecimal(txtPayableQty.Text == "" ? "0" : txtPayableQty.Text);
                //decimal dAccQty = Convert.ToDecimal(rowGv.Cells[4].Text.Trim());
                //decimal dPaidQty = Convert.ToDecimal(rowGv.Cells[5].Text.Trim());
                //decimal dRemainingQty = dAccQty - dPaidQty;
                if (chkSelect.Checked == false)
                {
                    txtPayableQty.Text = "0";
                }
                if (chkSelect.Checked == true && dtxtPayableQty == 0)
                {
                    chkSelect.Checked = false;
                    SysFunc.UserMsg(LbErr, Color.Red, "Quantity can not be zero(0) or empty");
                    return;
                }
                //if (chkSelect.Checked == true && dtxtPayableQty > dRemainingQty)
                //{
                //    chkSelect.Checked = false;
                //    SysFunc.UserMsg(LbErr, Color.Red, "Remaining payable Qty is " + dRemainingQty + " you can not enter greater then Remaining Qty");
                //    return;
                //}
                //decimal dtxtPayableQty = Convert.ToDecimal(txtPayableQty.Text == "" ? "0" : txtPayableQty.Text);
                decimal dRate = Convert.ToDecimal(rowGv.Cells[7].Text.Trim() == "" ? "0" : rowGv.Cells[7].Text.Trim());


                decimal dDiscountPerc = Convert.ToDecimal(rowGv.Cells[8].Text.Trim() == "" ? "0" : rowGv.Cells[8].Text.Trim());
                decimal dDiscountAmt = (dRate * dDiscountPerc) / 100;
                lblDiscountAmt.Text = dDiscountAmt.ToString();
                decimal dTotalAmt = dtxtPayableQty * (dRate - dDiscountAmt);
                //decimal dGrossAmt = dTotalAmt - dDiscountAmt;
                lblGrossAmt.Text = dTotalAmt.ToString();

                decimal dQtyCount = 0;
                decimal dFooterDiscountAmt = 0;
                decimal dGrossAmt = 0;
                foreach (GridViewRow gvr in gvPurInvDetail.Rows)
                {
                    TextBox txtPayableQtyTotal = (TextBox)gvr.Cells[6].FindControl("txtPayableQty");
                    Label lblDiscountAmtTotal = (Label)gvr.Cells[9].FindControl("lblDiscountAmt");
                    Label lblGrossAmtTotal = (Label)gvr.Cells[10].FindControl("lblGrossAmt");
                    CheckBox chkSelectChk = (CheckBox)gvr.Cells[0].FindControl("chkSelect");
                    if (chkSelectChk.Checked == true)
                    {
                        dQtyCount = dQtyCount + Convert.ToDecimal(txtPayableQtyTotal.Text.Trim() == "" ? "0" : txtPayableQtyTotal.Text.Trim());
                        dFooterDiscountAmt = dFooterDiscountAmt + Convert.ToDecimal(lblDiscountAmtTotal.Text);
                        dGrossAmt = dGrossAmt + Convert.ToDecimal(lblGrossAmtTotal.Text);
                    }
                }
                Label lblPayableQty = (Label)gvPurInvDetail.FooterRow.FindControl("lblPayableQty");
                Label lblDicountTotal = (Label)gvPurInvDetail.FooterRow.FindControl("lblDicountTotal");
                Label lblFooterGrossAmt = (Label)gvPurInvDetail.FooterRow.FindControl("lblFooterGrossAmt");
                lblPayableQty.Text = dQtyCount.ToString();
                lblDicountTotal.Text = Convert.ToString(dFooterDiscountAmt * dQtyCount);
                ViewState["TotalDiscount"] = Convert.ToString(dFooterDiscountAmt * dQtyCount);
                lblFooterGrossAmt.Text = dGrossAmt.ToString();
                //Load_Detail("sp_Select_GRNDetail_on_PayableInvForm", ddlGRN.SelectedValue.ToString().Trim(), "@GRNNo");
                CalcTotals();
                //Label =(Label)gvPurInvDetail.FooterRow.FindControl("");

                //DataTable dt=new DataTable ();
                //dt = (DataTable)Session["gvPurInvDetail"];
                //dt = GetDataTable(gvPurInvDetail, dt);
                //gvPurInvDetail.DataSource = dt;
                //gvPurInvDetail.DataBind();





            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);

            }
            //}
            //private DataTable GetDataTable(GridView dtg,DataTable dt)
            //{
            //    //DataTable dt = new DataTable();

            //    // add the columns to the datatable            
            //    //if (dtg.HeaderRow != null)
            //    //{

            //    //    for (int i = 0; i < dtg.HeaderRow.Cells.Count; i++)
            //    //    {
            //    //        dt.Columns.Add(dtg.HeaderRow.Cells[i].Text);
            //    //    }
            //    //}

            //    //  add each of the data rows to the table
            //    foreach (GridViewRow row in dtg.Rows)
            //    {
            //        DataRow dr;
            //        dr = dt.NewRow();

            //        for (int i = 0; i < row.Cells.Count; i++)
            //        {
            //            dr[i] = row.Cells[i].Text.Replace(" ", "");
            //        }
            //        dt.Rows.Add(dr);
            //    }
            //    return dt;
            //}
        }

        protected void txtPayableQty_TextChanged(object sender, EventArgs e)
        {
            TextBox txtQty = (TextBox)sender;
            GridViewRow rowGv = (GridViewRow)txtQty.Parent.Parent;
            CheckBox chkSelect = (CheckBox)rowGv.Cells[0].FindControl("chkSelect");

            chkSelect.Checked = false;

        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2544", "004"))
            {
                Response.Redirect("~/Test.aspx");
            }
            try
            {
                if (SysFunc.CheckVoucherPostFlag(Session["DealerCode"].ToString(), txtVoucherNo.Text))
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Can't Edit or Delete the Voucher '" + txtVoucherNo.Text + "' is already Posted");
                    return;
                }
                SysFunc.UpdateJV(Session["DealerCode"].ToString(), txtVoucherNo.Text);
                string ItemCode = string.Empty;
                foreach (GridViewRow gvr in gvPurInvDetail.Rows)
                {
                    ItemCode = gvr.Cells[1].Text;
                    SqlParameter[] param =
                      {
                         new SqlParameter("@DealerCode",SqlDbType.Char, 10),//0
                         new SqlParameter("@PurInvNo",SqlDbType.Char, 8),//1
                         new SqlParameter("@GRNNo",SqlDbType.Char, 8),//2
                         new SqlParameter("@ItemCode",SqlDbType.Char, 8),//3
                      };
                    param[0].Value = Session["DealerCode"].ToString();
                    param[1].Value = ddlPayInvNo.SelectedValue.ToString();
                    param[2].Value = ddlGRN.SelectedValue.ToString().Trim();
                    param[3].Value = ItemCode;
                    if (SysFunc.ExecuteSP_NonQuery("sp_PurInvMasterDetail_Delete", param))
                    {
                        SysFunc.UserMsg(LbErr, Color.Green, "Deleted Successfully");
                        btnClear_Click(BtnDelete, EventArgs.Empty);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Deletealert()", true);
                    }
                    else
                    {
                        SysFunc.UserMsg(LbErr, Color.Red, "Record not delete Try again");

                    }
                }
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }

        }

        protected void btnGL_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2544", "005"))
            {
                Response.Redirect("~/Test.aspx");
            }
            string URL;
            URL = "../../Service/Forms/JV.aspx?CusInv=" + ddlPayInvNo.SelectedValue + "&Type=PI";
            string fullURL = "window.open('" + URL + "', '_blank', 'height=600,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }

        protected void imgPayInvLookup_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 82; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_PayableInvoiceNo(imgPayInvLookup, ViewState["lookupid"].ToString(), "", "../../../");

            btnValue = 2;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }
        protected void imgGRNLookup_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 38; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_GRNNumber(imgGRNLookup, ViewState["lookupid"].ToString(), "", "../../../","PurInv");

            btnValue = 1;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }
        protected void SelectedPartDetail(string item)
        {
            try
            {

                if (btnValue == 2)
                {

                    ddlPayInvNo.SelectedValue = item.ToString();
                    ddlPayInvNo_SelectedIndexChanged(null, null);

                }
               else if (btnValue == 1)
                {
                    if (ddlVendor.SelectedIndex == 0)
                    {
                        SysFunc.UserMsg(lblImp, Color.Red,"Please Select Vendor First");
                        return;
                    }
                    ddlGRN.SelectedValue = item.ToString();
                    ddlGRN_SelectedIndexChanged(null, null);

                }


            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblImp, Color.Red, ex.Message);
            }
        }
    }
    }
