using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.ApplicationBlocks.Data;
using CrystalDecisions.Shared;
using CConn;
using DXBMS.Data;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;


namespace DXBMS.Modules.SpareParts.Forms
{
    public partial class SalesReturnForm : System.Web.UI.Page
    {
        
        Transaction ObjTrans = new Transaction();
        SysFunctions SysFunc = new SysFunctions();
        SecurityBll sec = new SecurityBll(); 
        DataSet ObjPOds;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2550", "001"))
            {
                Response.Redirect("~/Test.aspx");
            } 
            if (!IsPostBack)
            {
                //ViewState["ObjPOds"] = ObjPOds;
                createPartsDT();
                string[] Columns = new string[] { "CusCode", "CusDesc","NIC", "Phone1"};
                SysFunc.GetMultiColumnsDDL(ddlCustomer, Columns, "Customer", "DealerCode ='" + Session["DealerCode"].ToString() + "'", "CusCode", " Order by  CusDesc " ,false,false);
                Load_ddlSRNo();
                if (txtRetDate.Text.Length == 0)
                {
                    txtRetDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                rdCounterSale.Enabled = false;
                rdJobCard.Enabled = false;
                rdJobCard.Checked = false;
                rdCounterSale.Checked = false;
                
            }
        }
        private void Load_ddlSRNo()
        {
            string[] Columns = new string[] { "a.SRNo", "CONVERT(VARCHAR(10),a.SRDate,105)", "b.CusDesc" };
            SysFunc.GetMultiColumnsDDL(ddlSRNO, Columns, "SRMaster a ,  Customer b", " a.CusCode = b.CusCode and a.DealerCode = b.DealerCode And a.DealerCode ='" + Session["DealerCode"].ToString() + "'", "SRNo", " Order by a.SRNo Desc", false, false);
        }
        private void createPartsDT()
        {
            ObjPOds = new DataSet();
            ObjPOds.Tables.Add("SaleReturnTable");
            ObjPOds.Tables["SaleReturnTable"].Columns.Add(new DataColumn("Sno", typeof(string)));
            ObjPOds.Tables["SaleReturnTable"].Columns.Add(new DataColumn("ItemCode", typeof(string)));
            ObjPOds.Tables["SaleReturnTable"].Columns.Add(new DataColumn("PartItemNo", typeof(string)));
            ObjPOds.Tables["SaleReturnTable"].Columns.Add(new DataColumn("PartItemName", typeof(string)));
            ObjPOds.Tables["SaleReturnTable"].Columns.Add(new DataColumn("Unit", typeof(string)));
            ObjPOds.Tables["SaleReturnTable"].Columns.Add(new DataColumn("UnitCode", typeof(string)));
            ObjPOds.Tables["SaleReturnTable"].Columns.Add(new DataColumn("Rate", typeof(string)));
            ObjPOds.Tables["SaleReturnTable"].Columns.Add(new DataColumn("Qty", typeof(string)));
            ObjPOds.Tables["SaleReturnTable"].Columns.Add(new DataColumn("Amount", typeof(string)));
            ObjPOds.Tables["SaleReturnTable"].Columns.Add(new DataColumn("LocationCode", typeof(string)));
            ObjPOds.Tables["SaleReturnTable"].Columns.Add(new DataColumn("Location", typeof(string)));
            ObjPOds.Tables["SaleReturnTable"].Columns.Add(new DataColumn("AvgRate", typeof(string)));

            GvMain.DataSource = ObjPOds.Tables["SaleReturnTable"]; 
            GvMain.DataBind(); 
            ViewState["ObjPOds"] = ObjPOds;
        }
        
        private void GetSRData()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char),
                                    new SqlParameter("@SRNO",SqlDbType.Char)
                                   };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlSRNO.SelectedValue.ToString().Trim();
                ds = SysFunc.Sp_2W_SRMaster_Select(param);
                ddlCustomer.SelectedValue = ds.Tables[0].Rows[0]["CusCode"].ToString();
                //txtCusDesc.Text = ds.Tables[0].Rows[0]["CusDesc"].ToString();
                //ddlSaleInvNo.SelectedValue = ds.Tables[0].Rows[0]["SaleInvNo"].ToString();
                if (ds.Tables[0].Rows[0]["SaleType"].ToString() == rdCounterSale.Text)
                {
                    rdCounterSale.Checked = true;
                    Load_CounterSale();
                    ddlSaleInvNo.SelectedValue = ds.Tables[0].Rows[0]["SaleInvNo"].ToString();
                }
                else
                {

                    rdJobCard.Checked = true;
                    Load_ddlJobCard();
                    ddlSaleInvNo.SelectedValue = ds.Tables[0].Rows[0]["SaleInvNo"].ToString();
                }

                txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();

            }
            catch (Exception ex)
            {
                LbErr.Text = ex.Message;
            }
        }
        /// <summary>
        /// Will work after load JobCard or Invoice Data ddl
        /// </summary>
        private void DisplayItem()
        {
            string SIRNo = string.Empty;
            DataSet dsAvg = new DataSet();

            try
            {
                SqlDataReader dr=null;
                txtItemCode.Text = ddlParts.SelectedValue.ToString().Trim();
                if (rdCounterSale.Checked)
                {
                    SqlParameter[] param = {
                                        new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                        new SqlParameter("@IssueNo",SqlDbType.Char,8),
                                       new SqlParameter("@ItemCode",SqlDbType.Char,8),
                                           new SqlParameter("@SaleInvNo",SqlDbType.Char,8)};
                    param[0].Value = Session["DealerCode"].ToString();
                    param[1].Value = txtIssueNo.Text.Trim();
                    param[2].Value = txtItemCode.Text.Trim();
                    param[3].Value = ddlSaleInvNo.SelectedValue.ToString();
                    SysFunc.ExecuteSP("sp_Part_Info_SalesReturn_CS", param, ref dr);
                    if (dr.HasRows)
                    {
                        dr.Read();
                        txtLocCode.Text = dr["PartsTransLocCode"].ToString();
                        txtLocDesc.Text = dr["PartsTransLocDesc"].ToString();
                        txtRate.Text = dr["Rate"].ToString();
                        txtInvQuantity.Text = dr["IssueQty"].ToString();
                        txtUnitCode.Text = dr["UnitCode"].ToString();
                        txtUnit.Text = dr["UnitDesc"].ToString();
                        txtAvgCost.Text = dr["AvgCost"].ToString();
                        txtRemainingQty.Text = dr["Remaining_Qty"].ToString();
                    }
                    
                    
                }
                else if (rdJobCard.Checked)
                {
                    SqlParameter[] param = {
                                        new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                        new SqlParameter("@IssueNo",SqlDbType.Char,8),
                                       new SqlParameter("@ItemCode",SqlDbType.Char,8),
                                           new SqlParameter("@JobCardCode",SqlDbType.Char,8)};
                    param[0].Value = Session["DealerCode"].ToString();
                    param[1].Value = txtIssueNo.Text.Trim();
                    param[2].Value = txtItemCode.Text.Trim();
                    param[3].Value = ddlSaleInvNo.SelectedValue.ToString();
                    SysFunc.ExecuteSP("sp_Part_Info_SalesReturn_JC", param, ref dr);
                    if (dr.HasRows)
                    {
                        dr.Read();
                        txtLocCode.Text = dr["PartsTransLocCode"].ToString();
                        txtLocDesc.Text = dr["PartsTransLocDesc"].ToString();
                        txtRate.Text = dr["Price"].ToString();
                        txtInvQuantity.Text = dr["IssueQty"].ToString();
                        txtUnitCode.Text = dr["UnitCode"].ToString();
                        txtUnit.Text = dr["UnitDesc"].ToString();
                        txtAvgCost.Text = dr["AvgCost"].ToString();
                        txtRemainingQty.Text = dr["Remaining_Qty"].ToString();
                    }
                    
                   
                }
                
            }
            catch (Exception ex)
            {
                LbErr.Text = ex.Message;
            }

        }
        private void Save_InsertMasterDetail(SqlTransaction Trans, ref bool Check, ref string StrSRNo)
        {   
            try
            {
                SqlParameter[] param = {
                                       new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                                       new SqlParameter("@SRNo",SqlDbType.Char,8),//1
                                       new SqlParameter("@SRDate",SqlDbType.DateTime),//2
                                       new SqlParameter("@CusCode",SqlDbType.Char,8),//3
                                       new SqlParameter("@Remarks",SqlDbType.Text),//4
                                       new SqlParameter("@TotalQty",SqlDbType.Float),//5
                                       new SqlParameter("@TotalAmount",SqlDbType.Float),//6
                                       new SqlParameter("@DelFlag",SqlDbType.Char,1),//7
                                       new SqlParameter("@PostFlag",SqlDbType.Char,1),//8
                                       new SqlParameter("@UpdUser",SqlDbType.VarChar,50),//9
                                       new SqlParameter("@UpdTerm",SqlDbType.VarChar,50),//10
                                       new SqlParameter("@VoucherNo",SqlDbType.VarChar,50),//11
                                       new SqlParameter("@VoucherFlag",SqlDbType.Char,1),//12
                                       new SqlParameter("@SaleInvNo",SqlDbType.Char,8),//13
                                       new SqlParameter("@SaleType",SqlDbType.VarChar,15),//14
                                       

                                   };
                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = SysFunc.AutoGen("SRMaster", "SRNo", txtRetDate.Text.Trim());
                param[2].Value = SysFunc.SaveDate(txtRetDate.Text);
                param[3].Value = ddlCustomer.SelectedValue.ToString().Trim();
                param[4].Value = txtRemarks.Text.Trim();
                param[5].Value = txtTotalQty.Text.Trim();
                param[6].Value = txtTotalAmount.Text.Trim();
                param[10].Value = GlobalVar.mUserIPAddress;
                param[7].Value = "N";
                param[8].Value = "N";
                param[9].Value = Session["UserName"].ToString();
                param[11].Value = "";
                param[12].Value = "";
                param[13].Value = ddlSaleInvNo.SelectedValue.ToString();
                if (rdCounterSale.Checked)
                {
                    param[14].Value = rdCounterSale.Text;
                }
                else if (rdJobCard.Checked)
                {
                    param[14].Value = rdJobCard.Text;
                }


                if (SysFunc.sp_2W_SRMaster_Insert(Trans, param))
                {
                    StrSRNo = param[1].Value.ToString();
                    for (int i = 0; i < GvMain.Rows.Count; i++)
                    {
                        SqlParameter[] param1 = {
                                        new SqlParameter("@DealerCode",SqlDbType.Char ),//0
                                        new SqlParameter("@SRNo",SqlDbType.Char,8),//1
                                        new SqlParameter("@ItemCode",SqlDbType.Char,8),//2
                                        new SqlParameter("@PartsTransLocCode",SqlDbType.VarChar,7),//3
                                        new SqlParameter("@RetQty",SqlDbType.Float),//4
                                        new SqlParameter("@Rate",SqlDbType.Float),//5
                                        new SqlParameter("@Amount",SqlDbType.Float),//6
                                        new SqlParameter ("@AvgRate",SqlDbType .Float )//7
                                        };

                        Label LbItemCode = (Label)GvMain.Rows[i].FindControl("LbItemCode");
                        Label LbPartsTransLocCode = (Label)GvMain.Rows[i].FindControl("LbPartsTransLocCode");
                        HiddenField HDPartsTransLocCode = (HiddenField)GvMain.Rows[i].FindControl("HDPartsTransLocCode");

                        Label LbIssueQty = (Label)GvMain.Rows[i].FindControl("LbQty");
                        Label LbAmount = (Label)GvMain.Rows[i].FindControl("LbAmount");
                        Label LbRate = (Label)GvMain.Rows[i].FindControl("LbRate");
                        Label LbAvgRate = (Label)GvMain.Rows[i].FindControl("LbAvgCost");


                        param1[0].Value = Session["DealerCode"].ToString();
                        param1[1].Value = StrSRNo;
                        param1[2].Value = LbItemCode.Text;
                        param1[3].Value = HDPartsTransLocCode.Value;
                        param1[4].Value = LbIssueQty.Text;
                        param1[5].Value = LbRate.Text;
                        param1[6].Value = LbAmount.Text;
                        param1[7].Value = LbAvgRate.Text;

                        if (SysFunc.sp_W2_SRDetail_Insert(Trans, param1))
                        {
                            Check = true;
                        }
                        else
                        {
                            Check = false;
                            break;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LbErr.Text = ex.Message;
            }
        }
        
        public bool UpdatePost(string StockRetNo, ref SqlTransaction Trans)
        {

            string Update_Qry = " Exec sp_PostSalesReturnActivity '" + StockRetNo + "','" + Session["DealerCode"].ToString() + "'";
            try
            {
                if (SysFunc.ExecuteQuery(Update_Qry, Trans) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
                return false;
            }
        }

        private void Posting(ref bool Check)
        {
            SqlTransaction Trans = null;
            string strSQL = string.Empty;
            DataSet dsItemStock = new DataSet();
            string strMaster = string.Empty;
            Check = false;
            string strSrNo=string.Empty;

            try
            {
                if (GvMain.Rows.Count == 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Details should not be left blank...");
                    return;
                }
                else
                {
                    LbErr.Text = string.Empty;
                }
                ///Transection Begin
                if (ObjTrans.BeginTransaction(ref Trans) == false)
                {
                    return;
                }
                //if (ddl)
                //{
                Save_InsertMasterDetail(Trans, ref Check, ref strSrNo);
                    if (Check == true)
                    {
                        ///Start Posting
                        foreach (GridViewRow gvr in GvMain.Rows)
                        {
                            //'<<<<<<<< Update the ItemSotck >>>>>>>>>>>
                            bool NewFlag = false;

                            Label LbItemCode = (Label)gvr.FindControl("LbItemCode");
                            Label LbQty = (Label)gvr.FindControl("LbQty");
                            Label LbAmt = (Label)gvr.FindControl("LbAmount");
                            HiddenField hdLocCode = (HiddenField)gvr.FindControl("HDPartsTransLocCode");
                            strSQL = "SELECT * From ItemStock where ItemCode = '" + LbItemCode.Text + "' AND PartsTransLocCode = '" + hdLocCode.Value + "'";
                            if (SysFunc.ExecuteQuery(strSQL, ref dsItemStock) == false)
                            {
                                NewFlag = true;
                            }
                            if (NewFlag)
                            {
                                strSQL = "INSERT into ItemStock(DealerCode, PartsTransLocCode, ItemCode, RetQty, RetVal,SaleVal,SaleQty,RetSupp,Rejqty,IssVal,IssQty,RecVal,RecQty) values (" +
                                    " '" + Session["DealerCode"].ToString() + "'," +
                                    " '" + hdLocCode.Value + "'," +
                                    " '" + LbItemCode.Text + "'," +
                                    " '" + LbQty.Text.Trim() + "'," +
                                    " '" + LbAmt.Text + "',0,0,0,0,0,0,0,0) ";
                            }
                            else
                            {
                                strSQL = "UPDATE ItemStock Set RetQty = RetQty + " + LbQty.Text.Trim() + ", RetVal = RetVal + " + LbAmt.Text + " Where DealerCode = '" + Session["DealerCode"].ToString() + "' AND PartsTransLocCode = '" + hdLocCode.Value + "' And ItemCode = '" + LbItemCode.Text + "'";
                            }
                            if (SysFunc.ExecuteQuery_NonQuery(strSQL, Trans) == false)
                            {
                                SysFunc.UserMsg(LbErr, Color.Red, "Can Not Post, Not found ItemStock");
                                ObjTrans.RollBackTransaction(ref Trans);
                                Check = false;
                                return;
                            }
                           
                                strSQL = "UPDATE StockIssueDetail SET RetQty=" + LbQty .Text.Trim()+ " WHERE Dealercode='" + Session["DealerCode"].ToString() + "' And IssueNo='" + txtIssueNo.Text.Trim() + "' And ItemCode='"+ LbItemCode.Text.Trim() +"'";
                                if (SysFunc.ExecuteQuery_NonQuery(strSQL, Trans) == false)
                                {
                                    SysFunc.UserMsg(LbErr, Color.Red, "Can Not Post, Not found ItemStock");
                                    ObjTrans.RollBackTransaction(ref Trans);
                                    Check = false;
                                    return;
                                }
                            
                        }
                        //for adjust customer outstanding
                        if (rdCounterSale.Checked)
                        {
                            strMaster = "Update CounterSaleMaster set InvoiceAmount =  InvoiceAmount - '" + txtTotalAmount.Text + "'   Where saleinvno='" + ddlSaleInvNo.SelectedValue.ToString() + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
                            if (SysFunc.ExecuteQuery_NonQuery(strMaster, Trans) == false)
                            {
                                SysFunc.UserMsg(LbErr, Color.Red, "Can Not Post, Not found ItemStock");
                                ObjTrans.RollBackTransaction(ref Trans);
                                Check = false;
                                return;
                            }
                        }
                        if (rdJobCard.Checked)
                        {
                            if (SysFunc.CodeExists("CustomerInvoice", "JobcardCode", ddlSaleInvNo.SelectedValue.ToString(), " And DelFlag = 'N' and invType in ('Inv','Clb')") == true)
                            {
                                strMaster = "Update CustomerInvoice set NetAmountCustomer =  NetAmountCustomer - '" + txtTotalAmount.Text + "'   Where jobcardCode='" + ddlSaleInvNo.SelectedValue.ToString() + "' and DealerCode = '" + Session["DealerCode"].ToString() + "' and invType in ('Inv','Clb')";
                                if (SysFunc.ExecuteQuery_NonQuery(strMaster, Trans) == false)
                                {
                                    SysFunc.UserMsg(LbErr, Color.Red, "Can Not Post, Not found ItemStock");
                                    ObjTrans.RollBackTransaction(ref Trans);
                                    Check = false;
                                    return;
                                }
                            }
                        }
                        if (UpdatePost(strSrNo, ref Trans) == false)
                        {
                            SysFunc.UserMsg(LbErr, Color.Red, "UpdatePost Error");
                            ObjTrans.RollBackTransaction(ref Trans);
                            Check = false;
                            return;
                        }
                        else
                        {
                            Check = true;
                        }
                        
                    }

                
                if (Check == false)
                {
                    ObjTrans.RollBackTransaction(ref Trans);
                    SysFunc.UserMsg(LbErr, Color.Red, "Record can not save. Try again...");
                }
                else
                {
                    ObjTrans.CommittTransaction(ref Trans);
                    SysFunc.UserMsg(LbErr, Color.Green, "Record Posted successfuly.  Return No.: " + strSrNo);
                    Load_ddlSRNo();
                    CtlClear("T");
                }

            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }
        private void CtlClear(string strChk)
        {
            DataSet ds = new DataSet();
            try
            {
                ddlCustomer.Enabled = true;
                SysFunc. ClearTextBoxes(Page);
                ddlSRNO.SelectedIndex = 0;
                txtRetDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                ddlCustomer.SelectedIndex = 0;
                rdCounterSale.Checked = false;
                rdJobCard.Checked = false;
                rdCounterSale.Enabled = false;
                rdJobCard.Enabled = false;
                ddlSaleInvNo.Items.Clear();
                ddlParts.Items.Clear();
                ds = (DataSet)ViewState["ObjPOds"];
                ds.Tables["SaleReturnTable"].Rows.Clear();
                ViewState["ObjPOds"] = ds;
                GvMain.DataSource = ds;
                GvMain.DataBind();
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }
        
        private void FillGV()
        {

            DataSet dsMain = new DataSet();
            try
            {

                SqlParameter[] param = {    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                      new SqlParameter("@SRNo",SqlDbType.Char,10)
                                   };
                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlSRNO.SelectedValue.ToString();
                dsMain = SysFunc.sp_W2_Get_SRDetail(param);
                GvMain.DataSource = dsMain.Tables[0];
                GvMain.DataBind();
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }
        private void ClearPartInfo()
        {
            txtItemCode.Text = string.Empty;
            txtUnit.Text = string.Empty;
            txtLocCode.Text = string.Empty;
            txtLocDesc.Text = string.Empty;


        }
        private void ClearDetail()
        {
           
            txtItemCode.Text = string.Empty;
            txtUnit.Text = string.Empty;
            txtUnitCode.Text = string.Empty;
            txtLocCode.Text = string.Empty;
            txtLocDesc.Text = string.Empty;
            txtTotalAmt.Text = string.Empty;
            txtAvgCost.Text = string.Empty;
            txtRate.Text = string.Empty;
            txtQty.Text = string.Empty;
            txtInvQuantity.Text = string.Empty;
            ddlParts.SelectedIndex = 0;

            //txtAmount.Text = string.Empty;
        }
        private void AddNewRow()
        {
            DataSet ds = new DataSet();
            ds = (DataSet)ViewState["ObjPOds"];
            DataRow row = ds.Tables["SaleReturnTable"].NewRow();
            if (lblSNo.Text.Trim() != string.Empty)
            {
                row["Sno"] = lblSNo.Text;
            }
            else
            {
                row["Sno"] = SysFunc.GetNewMaxID("Sno", 3, ds);
            }
            
            row["Sno"] = ds.Tables["SaleReturnTable"].Rows.Count + 1;
            row["ItemCode"] = txtItemCode.Text.Trim();
            row["PartItemNo"] = SysFunc.GetPartItemNoByItemCode(ddlParts.SelectedValue.ToString());
            row["PartItemName"] = SysFunc.GetItemDescByItemCode(ddlParts.SelectedValue.ToString());
            row["Unit"] = txtUnit.Text.Trim();
            row["UnitCode"] = txtUnitCode.Text.Trim();

            row["Rate"] = txtRate.Text.Trim();
            row["Qty"] = txtQty.Text;
            row["Amount"] = txtTotalAmt.Text;
            row["Location"] = txtLocDesc.Text;
            row["LocationCode"] = txtLocCode.Text;
            row["AvgRate"] = txtAvgCost.Text;
            ds.Tables["SaleReturnTable"].Rows.Add(row);
            //ViewState["ObjPOds"] = ds;
            GvMain.DataSource = ds.Tables["SaleReturnTable"];
            GvMain.DataBind();
            ViewState["ObjPOds"] = ds;
        }
        
        protected void BtnPrint_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2550", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (ddlSRNO.SelectedIndex == 0)
            {
                SysFunc.UserMsg(LbErr, Color.Red, "Select Sales Return number first ");
                return;
            }
            else
            {
                LbErr.Text = "";
            }
            ReportDocument RD;

            Data.DSReports dsRpt1 = new Data.DSReports();

            RD = new ReportDocument();
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(CConnection.GetConnectionString(), CommandType.Text, "sp_SaleReturn_Report '" + Session["DealerCode"].ToString() + "', '" + ddlSRNO.SelectedValue.ToString().Trim() + "'");
            dsRpt1.sp_SaleReturn_Report.Load(ds.CreateDataReader());
            RD.Load(Server.MapPath("../SpareReports/rptSaleReturnReport.rpt"));

            RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["FromCode"].Text = "\"" + Convert.ToDateTime(FromDate).ToString("dd/MM/yyyy") + "\"";
            //RD.DataDefinition.FormulaFields["ToCode"].Text = "\"" + Convert.ToDateTime(ToDate).ToString("dd/MM/yyyy") + "\"";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "SALES RETURN" + "\"";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.SetDataSource(dsrpt.sp_tempItemStockLedger_Select.DataSet);
            RD.SetDataSource(dsRpt1);
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
            RD.Dispose(); RD.Close();
            string URL = "../../../Download/PrintReport.aspx";

            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";

            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

            //CrystalReportViewer1.ReportSource = RD;


            //Session["RDSpare"] = RD;
            //RD.Database.Tables[0].SetDataSource(objDsReports);


            // crReportDocument = new ReportDocument();
            //  crReportDocument = RD; // (ReportDocument)Session["RDSpare"];
            //string FilePath = Server.MapPath("~") + "\\Download\\";
            //string FileName = "SalesReturn" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            //string File = FilePath + FileName;

            //Session["RD"] = RD;

            //string URL;
            //URL = "../../../../Download/rptViewerService.aspx?FileName=" + FileName;

            ////URL = FilePath + "OpenPdf.aspx?FileName=" + FileName;
            ////txtPartItemDesc.Text = URL;
            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";

            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
        protected void btnPosting_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2550", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            try
            {
                if (GvMain.Rows.Count == 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "At least one part must be add...");
                    return;
                }
                else
                {
                    LbErr.Text = "";
                }
                if (ddlSRNO.SelectedIndex != 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "The Sales Return is already saved; now you could not change the record...");
                    return;

                }
                else
                {
                    LbErr.Text = "";
                }
                if (rdJobCard.Checked)
                {
                    
                    if (SysFunc.CodeExists("CustomerInvoice", "JobcardCode", ddlSaleInvNo.SelectedValue.ToString(), " And DelFlag = 'N' and InvType In ('Dep','Ins')") == true)
                    {
                        
                        SysFunc.UserMsg(LbErr, Color.Red, "Stock Return against Insurance Jobcard is not allowed...");
                        return;
                    }
                    if (SysFunc.CodeExists("CustomerInvoice", "JobcardCode", ddlSaleInvNo.SelectedValue.ToString(), " And DelFlag = 'N' and PaidC  > 1  ") == true)
                    {
                        SysFunc.UserMsg(LbErr, Color.Red, "Payment exist against this Customer Invoice. Please delete payment and make Sales Retrun..." );
                        return;
                    }
                }
                
                bool chk = false;
                Posting(ref chk);
                if (chk)
                {
                    CtlClear("T");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);
                }
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                CtlClear("T");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);
            }
            catch (Exception ex)
            {

                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }
        


        protected void BtnSelect_Click(object sender, EventArgs e)
        {
           
            try
            {
                if (ddlSRNO.SelectedIndex != 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Already sales returned do not modify ");
                    return;
                }
                LinkButton BtnLnk = (LinkButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;

                Label LbItemCode = (Label)BtnLnk.FindControl("LbItemCode");
                
                ddlParts.SelectedValue = LbItemCode.Text;

                HiddenField hdUnit = (HiddenField)BtnLnk.FindControl("hdUnitCode");
                txtUnitCode.Text = hdUnit.Value;

                DisplayItem();
                LbErr.Text = "";
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }

        }
        
        protected void GvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblAmt = (Label)e.Row.FindControl("LbAmount");//LbAmount
                Label lblQty = (Label)e.Row.FindControl("LbQty");
                double iAmt = 0, iQty = 0;
                iAmt = Convert.ToDouble(lblAmt.Text) + iAmt;
                txtTotalAmount.Text = iAmt.ToString();
                iQty = Convert.ToDouble(lblQty.Text) + iQty;
                txtTotalQty.Text = iQty.ToString();
            }
        }

        protected void rdCounterSale_CheckedChanged(object sender, EventArgs e)
        {
            Load_CounterSale();
        }
        private void Load_CounterSale()
        {
            if (rdCounterSale.Checked == false)
            {
                ddlSaleInvNo.Items.Clear();
            }
            else
            {
                string strWhereClause = "A.DealerCode = '" + Session["DealerCode"].ToString() + "' " +
"AND A.DelFlag = 'N' " +
"AND A.DealerCode = B.DealerCode " +
"AND A.IssueNo = B.IssueNo " +
"AND A.PostFlag = 'Y' " +
"And A.InvoiceNo <> '' " +
"And A.DealerCode=C.DealerCode " +
"And A.InvoiceNo=C.SaleInvNo " +
"And C.PostFlag='Y' " +
"And C.CusCode='" + ddlCustomer.SelectedValue.ToString() + "' ";
                string[] Columns = new string[] { "C.SaleInvNo", "CONVERT(VARCHAR(10),C.SaleInvDate,105)", "A.IssueNo", "CONVERT(VARCHAR(10),A.IssueDate,105)" };
                //SysFunc.GetMultiColumnsDDL(ddlSaleInvNo, Columns, "StockIssue A, StockIssueDetail B , CounterSaleMaster C", strWhereClause, "SaleInvNo", "Order by C.SaleInvNo Desc", false, true, "Having(sum(B.IssueQty) > sum(B.RetQty))");
                SysFunc.GetMultiColumnsDDL(ddlSaleInvNo, Columns, "StockIssue A, StockIssueDetail B , CounterSaleMaster C", strWhereClause, "SaleInvNo", "Order by C.SaleInvNo Desc", false, false, string.Empty);

                
            }
        }
        protected void rdJobCard_CheckedChanged(object sender, EventArgs e)
        {
          
            Load_ddlJobCard();
        }
        private void Load_ddlJobCard()
        {
            if (rdJobCard.Checked == false)
            {
                ddlSaleInvNo.Items.Clear();
            }
            else
            {
                string strWhereClause = "A.DealerCode = '" + Session["DealerCode"].ToString() + "' " +
"AND A.DelFlag = 'N' " +
"AND A.DealerCode = B.DealerCode " +
"AND A.IssueNo = B.IssueNo " +
"AND A.PostFlag = 'Y' " +
"And A.JobCardNO <> '' " +
"And A.DealerCode=C.DealerCode " +
"And A.JobCardNo=C.JobCardCode " +
"And C.GatePass<>'' " +
"And C.CusCode='" + ddlCustomer.SelectedValue.ToString() + "' ";
                string[] Columns = new string[] { "C.JobCardCode", "CONVERT(VARCHAR(10),C.RecDate,105)", "A.IssueNo", "CONVERT(VARCHAR(10),A.IssueDate,105)" };
                SysFunc.GetMultiColumnsDDL(ddlSaleInvNo, Columns, "StockIssue A, StockIssueDetail B , JobCardMaster C", strWhereClause, "JobCardCode", "Order by C.JobCardCode Desc", false, true, "Having(sum(B.IssueQty) > sum(B.RetQty))");
                
                
            }
        }
        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCustomer.SelectedIndex == 0)
            {
                rdCounterSale.Enabled = false;
                rdJobCard.Enabled = false;
                rdJobCard.Checked = false;
                rdCounterSale.Checked = false;
                ddlSaleInvNo.Items.Clear();
            }
            else
            {
                rdCounterSale.Enabled = true;
                rdJobCard.Enabled = true;
                rdJobCard.Checked = false;
                rdCounterSale.Checked = false;
                ddlSaleInvNo.Items.Clear();

            }
        }

        protected void ddlSaleInvNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string AppendInWhere = string.Empty;
                if (rdCounterSale.Checked)
                {
                   
                    AppendInWhere = "  a.ItemCode=b.ItemCode and b.DealerCode in (a.DealerCode,'COMON') and a.DealerCode = '" + Session["DealerCode"].ToString() + "' " +
                   "   and a.SaleInvNo = '" + ddlSaleInvNo.SelectedValue.ToString().Trim() + "' " +
                   "   And b.PartItemNo not in ('') ";
                        string[] Columns = new string[] { "a.ItemCode","b.PartItemNo","b.ItemDesc","b.UnitCode" };
                        SysFunc.GetMultiColumnsDDL(ddlParts, Columns, " countersaledetail a , Item b", AppendInWhere, "ItemCode", "Order by b.ItemDesc",false,false);
                }
                else if (rdJobCard.Checked)
                {

                    AppendInWhere = "  a.ItemCode=b.ItemCode and a.DealerCode in (a.DealerCode,'COMON') And a.DealerCode=c.DealerCode  And a.ItemCode=c.ItemCode And   a.DealerCode = '" + Session["DealerCode"].ToString() + "' " +
                   "   and a.JobCardCode = '" + ddlSaleInvNo.SelectedValue.ToString().Trim() + "' " +
                   "   And b.PartItemNo not in ('') "  ;

                    string[] Columns = new string[] { "a.ItemCode", "b.PartItemNo", "b.ItemDesc", "b.UnitCode" };
                    SysFunc.GetMultiColumnsDDL(ddlParts, Columns, " jobcardpartsdetail a , Item b , StockIssueDetail c", AppendInWhere, "ItemCode", "Order by b.ItemDesc", false, false);
                }
                txtIssueNo.Text = ddlSaleInvNo.SelectedItem.Text.Substring(ddlSaleInvNo.SelectedItem.Text.Length - 20, 8);
                ddlCustomer.Enabled = false;
                rdCounterSale.Enabled = false;
                rdJobCard.Enabled = false;

            }
            catch (Exception ex) { throw ex; }
        }

        protected void ddlParts_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayItem();
        }

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            DataSet ds = new DataSet();
            double DRemainingQty = 0.0, DRate = 0.0, DQty = 0.0;
            try
            {
                if (txtRemainingQty.Text.Contains("-"))
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Negative Remaining Quantity can not Alowed Please contact to IT Support  ");
                    return;
                }

                DQty = Convert.ToDouble(txtQty.Text == "" ? "0" : txtQty.Text);
                DRate = Convert.ToDouble(txtRate.Text == "" ? "0" : txtRate.Text);
                DRemainingQty = Convert.ToDouble(txtRemainingQty.Text == "" ? "0" : txtRemainingQty.Text);
                if (DRate <= 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Rate should not be Zero (0)", txtQty);
                    return;
                }
                if (DQty <= 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Return Quantity should not be Zero (0)", txtQty);
                    return;
                }
                if (DQty > DRemainingQty)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Returne quantity not greater then Remaining Quantity", txtQty);
                    return;
                }

                ds = (DataSet)ViewState["ObjPOds"];


                if (ds.Tables["SaleReturnTable"].Rows.Count > 0)
                {
                    foreach (DataRow dtRow in ds.Tables["SaleReturnTable"].Rows)
                    {
                        if (txtItemCode.Text.Trim() == dtRow["ItemCode"].ToString())
                        {
                            lblSNo.Text = dtRow["Sno"].ToString();
                        }
                    }

                    foreach (DataRow dtRow in ds.Tables["SaleReturnTable"].Rows)
                    {
                        //Add Vehicle grid row when mor the on row exist

                        if (lblSNo.Text.Trim() != string.Empty)
                        {
                            if (lblSNo.Text == dtRow["Sno"].ToString())
                            {
                                int index = ds.Tables["SaleReturnTable"].Rows.IndexOf(dtRow);
                                ds.Tables["SaleReturnTable"].Rows.RemoveAt(index);
                                AddNewRow();
                                lblSNo.Text = "";
                                ClearDetail();

                                return;
                            }
                        }
                    }
                    //Add vehicle grid row in epty grid
                    if (ds.Tables["SaleReturnTable"].Rows[0]["Sno"].ToString() == "")
                    {
                        ds.Tables["SaleReturnTable"].Rows.RemoveAt(0);
                        AddNewRow();
                        ClearDetail();
                    }
                    else
                    {
                        AddNewRow();
                        ClearDetail();
                    }
                }
                else
                {
                    AddNewRow();
                    ClearDetail();
                }
                lblSNo.Text = "";
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);

            }
        }

        protected void BtnClear1_Click(object sender, ImageClickEventArgs e)
        {
            ClearPartInfo();
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsDelete = new DataSet();
            try
            {
                if (ddlSRNO.SelectedIndex != 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Already sales returned do not change ");
                    return;
                }
                dsDelete = (DataSet)ViewState["ObjPOds"];

                ImageButton BtnLnk = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;

                dsDelete.Tables["SaleReturnTable"].Rows[rowGvIndex].Delete();
                GvMain.DataSource = dsDelete.Tables["SaleReturnTable"];
                GvMain.DataBind();
                dsDelete.Tables["SaleReturnTable"].AcceptChanges();
                LbErr.Text = "";
                ViewState["ObjPOds"] = dsDelete;
                //calculate total parts amount
                if (dsDelete.Tables["SaleReturnTable"].Rows.Count > 0)
                {
                    double iRate = 0;
                    foreach (DataRow dr in dsDelete.Tables["SaleReturnTable"].Rows)
                    {
                        iRate = iRate + Convert.ToDouble(dr["Amount"].ToString());
                    }
                    txtTotalAmt.Text = iRate.ToString();
                }
                else
                {
                    txtTotalAmt.Text = "0";
                }
                //////////////////////
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }

        protected void ddlSRNO_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetSRData();
            FillGV();
        }

        protected void btnGL_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2550", "005"))
            {
                Response.Redirect("~/Test.aspx");
            }
            string URL;

                URL = "../../Service/Forms/JV.aspx?CusInv=" + ddlSRNO.SelectedValue + "&Type=SR";
         

            string fullURL = "window.open('" + URL + "', '_blank', 'height=600,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
    }
}