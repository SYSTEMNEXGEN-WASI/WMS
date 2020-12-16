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
namespace DXBMS.Modules.SpareParts.Forms
{
    public partial class StockReturn : System.Web.UI.Page
    {
        SysFunctions SysFunc = new SysFunctions();
        Transaction ObjTrans = new Transaction();
        SecurityBll sec = new SecurityBll();
        MainBLL ObjMainBll = new MainBLL();

        DataSet dsGVMain = new DataSet();
        DataTable dt;
        decimal sumFooterRetQty = 0;
        decimal sumFooterAmount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2549", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            //if (Session["LookUpData"] != null)
            //{
            //    int intlookupid = Convert.ToInt32(ViewState["lookupid"]);
            //    if (txtRemarks.Text.Contains("&nbsp;"))
            //    {
            //        txtRemarks.Text = "";
            //    }
            //}
            if (!IsPostBack)
            {   
                if (txtRetDate.Text.Length == 0)
                {
                    txtRetDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                
                Load_ddlReturnNo();
                Load_ddlIssueNo();
                createPartsDT();
            }
        }
        private void createPartsDT()
        {
            dsGVMain = new DataSet();
            dsGVMain.Tables.Add();

            dsGVMain.Tables[0].Columns.Add(new DataColumn("ItemCode", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("PartItemNo", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("PartItemName", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("Unit", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("UnitCode", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("RemainingQty", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("ReturnQty", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("Amount", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("Rate", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("Location", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("LocationCode", typeof(string)));

            GvMain.DataSource = dsGVMain.Tables[0];
            GvMain.DataBind();
            ViewState["ObjPOds"] = dsGVMain;
        }
        private void Load_ddlIssueNo()
        {
            if (ddlStockReturnNo.SelectedIndex == 0)
            {
                string WhereClause = "A.DealerCode = '" + Session["DealerCode"].ToString() + "'  " +
                    "And A.DealerCode = B.DealerCode " +
     "And A.IssueNo = B.IssueNo  " +
     "AND A.DelFlag = 'N' " +
     "AND A.PostFlag = 'Y' " +
     "AND 	(   (A.JobCardNo <> '' And A.JobCardNo IN (Select JobCardCode From Jobcardmaster Where DelFlag = 'N' and Gatepass='' and DealerCode = '"+ Session["DealerCode"].ToString() + "' )) " +
     "Or (A.InvoiceNo <> '' And A.InvoiceNo IN (Select SaleInvNo From CounterSaleMaster WHere PostFlag = 'N' And DelFlag = 'N'  and DealerCode = '" + Session["DealerCode"].ToString() + "' ) )	) ";

                string[] Columns = new string[] { "A.IssueNo", "convert(varchar(10),A.IssueDate,105)", "A.ReqSource", "ISNULL(A.JobCardNo, A.InvoiceNo)" };
                SysFunc.GetMultiColumnsDDL(ddlIssueNo, Columns, "StockIssue A , StockIssueDetail B ", WhereClause, "IssueNo", " Order by A.IssueNo Desc ", true, true, "Having(sum(B.IssueQty) > sum(B.RetQty))");
                ddlIssueNo.Enabled = true;
            }
            else
            {
                string WhereClause = "DealerCode = '" + Session["DealerCode"].ToString() + "'  " +
                    "And ReturnNo='"+ddlStockReturnNo.SelectedValue.ToString().Trim()+"' " + 
                "and DelFlag = 'N' ";

                string[] Columns = new string[] { "ReturnNo", "convert(varchar(10),ReturnDate,105)", "ReqSource", "'JC:' + JobCardNo", "'Inv:' + InvoiceNo" };
                SysFunc.GetMultiColumnsDDL(ddlIssueNo, Columns, "StockReturn", WhereClause, "ReturnNo", " Order by ReturnNo Desc ", false, false);
                ddlIssueNo.SelectedIndex = 1;
                ddlIssueNo.Enabled = false;
            }
        }
        private void Load_ddlReturnNo()
        {
            string WhereClause = "DealerCode = '" + Session["DealerCode"].ToString() + "'  " +
                "and DelFlag = 'N' ";

            string[] Columns = new string[] { "ReturnNo", "convert(varchar(10),ReturnDate,105)", "ReqSource", "'JC:' + JobCardNo", "'Inv:' + InvoiceNo"};
            SysFunc.GetMultiColumnsDDL(ddlStockReturnNo, Columns, "StockReturn", WhereClause, "ReturnNo", " Order by ReturnNo Desc ", false, false);
        }
        private void Load_ddlPartsNo()
        {
            string WhereClause = "A.DealerCode = '" + Session["DealerCode"].ToString() + "'  " +
   "and a.ItemCode = b.ItemCode " +
   "And b.DealerCode in ('"+Session["DealerCode"].ToString()+"','COMON') " +
   "and a.IssueNo = '"+ddlIssueNo.SelectedValue.ToString().Trim()+"' " +
   "And b.PartItemNo not in ('') "+
   "And a.IssueQty-a.RetQty <> 0    ";

            string[] Columns = new string[] { "a.ItemCode", "b.PartItemNo", "b.ItemDesc" };
            SysFunc.GetMultiColumnsDDL(ddlPartsNo, Columns, "StockIssueDetail a,Item b ", WhereClause, "ItemCode", " Order by a.ItemCode ", false, false);
        }
        private void GetReturnData()
        {
            DataSet ds = new DataSet();
            try
            {
                Load_ddlIssueNo();
                if (ddlStockReturnNo.SelectedIndex == 0)
                {
                    CtlClear("F");
                    return;
                }
                SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char),
                                    new SqlParameter("@ReturnNO",SqlDbType.Char)
                                   };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlStockReturnNo.SelectedValue.ToString().Trim();
                ds = SysFunc.SP_Parts_Get_StockReturn_Data(param);

                txtRetDate.Text = ds.Tables[0].Rows[0]["ReturnDate"].ToString();
                txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();

                FillGV();

            }
            catch (Exception ex)
            {
                LbErr.Text = ex.Message;
            }
        }
        private void DisplayItem()
        {

            DataSet ds = new DataSet();
            try
            {

                SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char),
                                   new SqlParameter("@ItemCode",SqlDbType.Char),
                                   new SqlParameter("@IssueNo",SqlDbType.Char)
                                       };
                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlPartsNo.SelectedValue.ToString();
                param[2].Value = ddlIssueNo.SelectedValue.ToString();
                ds = SysFunc.FillDataSet("sp_StockReturn_Part_Select", param);

                txtPartItemDesc.Text = ds.Tables[0].Rows[0]["ItemDesc"].ToString();
                txtPartItemNo.Text = ds.Tables[0].Rows[0]["PartItemNo"].ToString();
                txtLocCode.Text = ds.Tables[0].Rows[0]["PartsTransLocCode"].ToString();
                txtLocDesc.Text = ds.Tables[0].Rows[0]["PartsTransLocDesc"].ToString();
                txtUnitCode.Text = ds.Tables[0].Rows[0]["UnitCode"].ToString();
                txtUnit.Text = ds.Tables[0].Rows[0]["UnitDesc"].ToString();
                txtIssueQty.Text = ds.Tables[0].Rows[0]["IssueQty"].ToString();
                txtIssueRate.Text = ds.Tables[0].Rows[0]["Rate"].ToString();
                txtRemaining.Text = (Convert.ToDouble(txtIssueQty.Text) - Convert.ToDouble(ds.Tables[0].Rows[0]["RetQty"].ToString())).ToString();


                ds = SysFunc.PartStock(ddlPartsNo.SelectedValue.ToString().Trim(), Session["DealerCode"].ToString(), txtLocCode.Text);
                txtTotalStock.Text = ds.Tables[0].Rows[0]["Balance"].ToString();
                ds = SysFunc.PartStockAtLoaction(ddlPartsNo.SelectedValue.ToString().Trim(), Session["DealerCode"].ToString(), txtLocCode.Text);
                txtStockLoc.Text = ds.Tables[0].Rows[0]["Balance"].ToString();
            }
            catch (Exception ex)
            {
                LbErr.Text = ex.Message;
            }

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
                    break;
                }
                else { textBoxes[i].BorderColor = System.Drawing.ColorTranslator.FromHtml("#1A6297"); }
            }
            return isValid;
        }
        private void Save_InsertMasterDetail(SqlTransaction Trans, string strIssueDate, string strReqSource, string strJobCardNO, string strInvoiceNo, ref bool Check, ref string StrReturnNo)
        {    
            string StrIssueDate = "";
            try
            {
                StrIssueDate = txtRetDate.Text;
                SqlParameter[] param = {
                                       new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                                       new SqlParameter("@ReturnNo",SqlDbType.Char,8),//1
                                       new SqlParameter("@ReturnDate",SqlDbType.DateTime),//2
                                       new SqlParameter("@IssueNo",SqlDbType.Char,8),//3
                                       new SqlParameter("@ReqSource",SqlDbType.VarChar,10),//4
                                       new SqlParameter("@JobCardNO",SqlDbType.Char,8),//5
                                       new SqlParameter("@InvoiceNo",SqlDbType.Char,8),//6
                                       new SqlParameter("@Remarks",SqlDbType.VarChar,70),//7
                                       new SqlParameter("@UpdUser",SqlDbType.VarChar,50),//8
                                       new SqlParameter("@UpdTerm",SqlDbType.VarChar,50),//9
                                   };
                param[0].Value = Session["DealerCode"].ToString();

                param[1].Value = SysFunc.AutoGen("StockReturn", "ReturnNo", txtRetDate.Text.Trim()); ;
                param[2].Value = SysFunc.SaveDate(txtRetDate.Text);
                param[3].Value = ddlIssueNo.SelectedValue.ToString().Trim();
                param[4].Value = strReqSource.Trim();
                param[5].Value = strJobCardNO.Trim();
                param[6].Value = strInvoiceNo.Trim();
                param[7].Value = txtRemarks.Text.Replace("'", " ");
                param[8].Value = Session["UserName"].ToString();
                param[9].Value = GlobalVar.mUserIPAddress;;

                if (SysFunc.ExecuteSP_NonQuery("sp_W2_StockReturne_Insert", param, Trans))
                {
                    StrReturnNo = param[1].Value.ToString();
                    for (int i = 0; i < GvMain.Rows.Count; i++)
                    {
                        SqlParameter[] param1 = {
                                        new SqlParameter("@DealerCode",SqlDbType.Char ),//0
                                        new SqlParameter("@ReturnNo",SqlDbType.Char,8),//1
                                        new SqlParameter("@ItemCode",SqlDbType.Char,8),//2
                                        new SqlParameter("@IssueNo",SqlDbType.Char,8),//3
                                        new SqlParameter("@PartsTransLocCode",SqlDbType.VarChar,7),//4
                                        new SqlParameter("@ReturnQty",SqlDbType.Float),//5
                                        new SqlParameter("@Rate",SqlDbType.Float),//6
                                        new SqlParameter("@Amount",SqlDbType.Float)//7
                                        };

                        Label LbItemCode = (Label)GvMain.Rows[i].FindControl("LbItemCode");
                        Label LbPartsTransLocCode = (Label)GvMain.Rows[i].FindControl("LbPartsTransLocCode");
                        HiddenField HDPartsTransLocCode = (HiddenField)GvMain.Rows[i].FindControl("HDPartsTransLocCode");

                        Label LbRetQty = (Label)GvMain.Rows[i].FindControl("LbRetQty");
                        Label LbAmount = (Label)GvMain.Rows[i].FindControl("LbAmount");
                        Label LbRate = (Label)GvMain.Rows[i].FindControl("LbRate");


                        param1[0].Value = Session["DealerCode"].ToString();
                        param1[1].Value = StrReturnNo;
                        param1[2].Value = LbItemCode.Text;
                        param1[3].Value = ddlIssueNo.SelectedValue.ToString().Trim();
                        param1[4].Value = HDPartsTransLocCode.Value;
                        param1[5].Value = LbRetQty.Text;
                        param1[6].Value = LbRate.Text;
                        param1[7].Value = LbAmount.Text;

                        if (SysFunc.ExecuteSP_NonQuery("sp_W2_StockReturneDetail_Insert", param1, Trans))
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
            string Update_Qry = " Exec sp_PostStockReturnActivity '" + StockRetNo + "','" + Session["DealerCode"].ToString() + "'";
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
        private void Posting(ref bool Check, string strIssueDate, string strReqSource, string strJobCardNO, string strInvoiceNo)
        {                                   
            SqlTransaction Trans = null;    
            Check = false;                  

            try
            {
                if (GvMain.Rows.Count == 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Details should not be left blank...");
                    return;
                }

                ///Transection Begin
                if (ObjTrans.BeginTransaction(ref Trans) == false)
                {
                    return;
                }
                string StrReturnNo = string.Empty;
                Save_InsertMasterDetail(Trans, strIssueDate, strReqSource, strJobCardNO, strInvoiceNo, ref Check, ref StrReturnNo);
                    if (Check == true)
                    {
                        ///Start Posting
                        foreach (GridViewRow gvr in GvMain.Rows)
                        {
                            Label LbItemCode = (Label)gvr.FindControl("LbItemCode");
                            Label LbQty = (Label)gvr.FindControl("LbRetQty");
                            Label LbTotAmt = (Label)gvr.FindControl("LbAmount");
                            HiddenField hdLocCode = (HiddenField)gvr.FindControl("HDPartsTransLocCode");

                            if (Update_SIR_Detail(ddlIssueNo.SelectedValue.ToString().Trim(), strJobCardNO, LbItemCode.Text.Trim(), Convert.ToDouble(LbQty.Text.Trim()), ref Trans) == false)
                            {
                                SysFunc.UserMsg(LbErr, Color.Red, "Can Not Post, Not found SIRDetail");
                                ObjTrans.RollBackTransaction(ref Trans);
                                Check = false;
                                return;
                            }
                            else
                            {
                                Check = true;
                            }
                            if (strReqSource.Trim() == "J")
                            {
                        
                            if (UpdateJobCardDetail(ddlIssueNo.SelectedValue.ToString().Trim(), strJobCardNO, LbItemCode.Text.Trim(), Convert.ToDouble(LbQty.Text.Trim()), ref Trans) == false)
                                {
                                    SysFunc.UserMsg(LbErr, Color.Red, "Can Not Post, Not found Job card");
                                    ObjTrans.RollBackTransaction(ref Trans);
                                    Check = false;
                                    return;
                                }
                                else
                                {
                                    Check = true;
                                }
                            }
                            if (UpdateStock(LbItemCode.Text.Trim(), hdLocCode.Value, Convert.ToDouble(LbQty.Text.Trim()), Convert.ToDouble(LbTotAmt.Text.Trim()), ref Trans) == false)
                            {
                                SysFunc.UserMsg(LbErr, Color.Red, "Can Not Post, Not found ItemStock");
                                ObjTrans.RollBackTransaction(ref Trans);
                                Check = false;
                                return;
                            }
                            else
                            {
                                Check = true;
                            }
                        }
                        if (strReqSource.Trim() == "C")
                        {
                            if (UpdateCounterSales(strInvoiceNo, ref Trans) == false)
                            {
                                SysFunc.UserMsg(LbErr, Color.Red, "Can Not Post, Not found ItemStock");
                                ObjTrans.RollBackTransaction(ref Trans);
                                Check = false;
                                return;
                            }
                            else
                            {
                                Check = true;
                            }
                        }
                        if (UpdatePost(StrReturnNo, ref Trans) == false)
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
                    SysFunc.UserMsg(LbErr, Color.Green, "Record Posted successfuly.  Return No.: " + StrReturnNo);
                    CtlClear("T");
                }

            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }
        public bool UpdateCounterSales(string InvoiceNO, ref SqlTransaction Trans)
        {
            //#/------Updating CounterSalesMaster
            string Update_Qry = string.Empty;
            string SIRNo = string.Empty;
            SIRNo = SysFunc.GetStringValuesAgainstCodes("InvoiceNo", InvoiceNO, "SIRNo", "SIRMaster", "And DelFlag = 'N' ",Session["DealerCode"].ToString());

            Update_Qry += " Update CounterSaleMaster Set CounterSaleMaster.IssueFlag = ";
            Update_Qry += " (";
            Update_Qry += " Select Case Count(X.SIRNo) When 0 Then 'Y' Else 'N' End From SIRDetail X ";
            Update_Qry += " Where X.SIRNo = '" + SIRNo + "' ";
            Update_Qry += " And X.DealerCode = '" + Session["DealerCode"].ToString() + "' ";
            Update_Qry += " And X.Quantity > X.RecQty ";
            Update_Qry += " ) ";
            Update_Qry += " Where ";
            Update_Qry += " SaleInvNo = '" + InvoiceNO + "' ";
            Update_Qry += " And DealerCode = '" + Session["DealerCode"].ToString() + "' ";
            Update_Qry += " And PostFlag = 'N' ";

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
                return false;
            }
        }
        public bool UpdateStock(string ItemCode, string LocCode, double IssueQty, double IssueVal, ref SqlTransaction Trans)
        {
            string Update_Qry = "Update ItemStock Set " + " RetQty = RetQty + " + IssueQty + "," + " RetVal = RetVal + " + IssueVal + "" + " Where ItemCode = '" + ItemCode + "'" + " AND PartsTransLocCode = '" + LocCode + "'" + " AND DealerCode = '" + Session["DealerCode"].ToString() + "'";
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

                return false;
            }
        }

        public bool Update_SIR_Detail(string IssueNO, string JobCardNO, string ItemCode, double IssueQty, ref SqlTransaction Trans)
        {
            string strSIRNo = string.Empty;
            string strSQL = string.Empty;
            bool result = false;
            try
            {
                strSIRNo = SysFunc.GetStringValuesAgainstCodes("IssueNO", IssueNO, "SIRNo", "StockIssue", "",Session["DealerCode"].ToString());
                dt = SysFunc.GetData("Select * From SIRDetail  Where SIRNo = '"+strSIRNo+"' AND DealerCode IN( '"+Session["DealerCode"].ToString()+"','COMON') And ItemCode='"+ItemCode+"' ");
                Session["strSIRNo"] = strSIRNo;
                strSQL = "Update SIRDetail SET RecQty = RecQty - " + IssueQty.ToString() + " Where DealerCode = '" + Session["DealerCode"].ToString() + "' And SIRNo = '" + strSIRNo + "' And ItemCode  = '" + ItemCode + "' ";
                result = SysFunc.ExecuteQuery(strSQL, Trans);


                strSQL = "Update StockIssueDetail SET RetQty = RetQty + " + IssueQty.ToString() + " Where DealerCode = '" + Session["DealerCode"].ToString() + "' And IssueNo = '" + IssueNO + "' And ItemCode  = '" + ItemCode + "' ";
                if (SysFunc.ExecuteQuery(strSQL, Trans) == true)
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

                return false;
            }

        }

        public bool UpdateJobCardDetail(string IssueNO, string JobCardNO, string ItemCode, double IssueQty, ref SqlTransaction Trans)
        {


            string strSIRNo = string.Empty;
            string StrType = string.Empty;
            try
            {
                strSIRNo = Session["strSIRNo"].ToString();
                // StrType = SysFunc.GetStringValuesAgainstCodes("SIRNo", strSIRNo, "Type", "SIRDetail", "And ItemCode='"+ItemCode+"'",Session["DealerCode"].ToString());
                if (dt.Rows.Count > 0)
                {
                    StrType = dt.Rows[0]["Type"].ToString();
                }
               
            }
            catch (Exception ex)
            {
                return false;
            }

            if (StrType == "P")
            {
                string Update_Qry = "Update JobCardPartsDetail Set " + " RecQty = RecQty - " + Convert.ToDouble(IssueQty) + "" + " Where JobCardCode = '" + JobCardNO + "'" + " and ItemCode = '" + ItemCode + "'" + "  " + " and DealerCode = '" + Session["DealerCode"].ToString() + "'";
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
                    return false;
                }
            }
            else if (StrType == "A")
            {
                string Update_Qry = "Update JobCardPartsDetail Set " + " RecQty = RecQty - " + Convert.ToDouble(IssueQty) + "" + " Where JobCardCode = '" + JobCardNO + "'" + " and ItemCode = '" + ItemCode + "'" + " AND Type = 'A'" + " and DealerCode = '" + Session["DealerCode"].ToString() + "'";
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
                    return false;
                }
                //Consumable Returning
            }
            else if (StrType == "C")
            {
                string Update_Qry = " Update JobCardConDetail Set " + " RecQty = RecQty - " + Convert.ToDouble(IssueQty) + " " + " Where JobCardCode = '" + JobCardNO + "' " + " and ItemCode = '" + ItemCode + "'" + " and Type = 'C'" + " and DealerCode = '" + Session["DealerCode"].ToString() + "' ";
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

                    return false;
                }
            }
            else if (StrType == "S")
            {
                string Update_Qry = "Update JobCardPartsDetail Set " + " RecQty = RecQty - " + Convert.ToDouble(IssueQty) + "" + " Where JobCardCode = '" + JobCardNO + "'" + " and ItemCode = '" + ItemCode + "'" + " AND Type = 'S'" + " and DealerCode = '" + Session["DealerCode"].ToString() + "'";
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

                    return false;
                }
            }
            else if (StrType == "L")
            {
                string Update_Qry = "Update JobCardLubricateDetail Set " + " RecQty = RecQty - " + Convert.ToDouble(IssueQty) + "" + " Where JobCardCode = '" + JobCardNO + "'" + " and ItemCode = '" + ItemCode + "'" + " and DealerCode = '" + Session["DealerCode"].ToString() + "'";
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
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void ClearMaster()
        {
            txtRemarks.Text = string.Empty;
        }
        private void CtlClear(string strChk)
        {
            DataSet ds = new DataSet();
            try
            {
                
                txtRetDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtRemarks.Text = string.Empty;
                txtPartItemDesc.Text = string.Empty;
                txtUnit.Text = string.Empty;
                txtUnitCode.Text = string.Empty;
                txtIssueQty.Text = string.Empty;
                txtRemaining.Text = string.Empty;
                txtTotalStock.Text = string.Empty;
                txtStockLoc.Text = string.Empty;
                txtRetQty.Text = string.Empty;
                txtTotalAmt.Text = string.Empty;
                txtIssueRate.Text = string.Empty;
                txtLocCode.Text = string.Empty;
                txtLocDesc.Text = string.Empty;

                ddlIssueNo.SelectedIndex = 0;

                ds = (DataSet)ViewState["ObjPOds"];
                ds.Tables[0].Rows.Clear();
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
                                      new SqlParameter("@ReturnNo",SqlDbType.Char,10)
                                   };
                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlStockReturnNo.SelectedValue.ToString().Trim();
                dsMain = SysFunc.Get_Parts_Get_StockRetDetail(param);
                GvMain.DataSource = dsMain.Tables[0];
                GvMain.DataBind();
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }

        private string GetPartLocation(string StrLocationCode)
        {
            DataSet ds = new DataSet();
            string StrLocationNM = "";
            try
            {
                SqlParameter[] param = {
                                     new SqlParameter("@PartsTransLocCode",SqlDbType.Char),
                                     new SqlParameter("@DealerCode",SqlDbType.Char)
                                   };
                param[0].Value = StrLocationCode;
                param[1].Value = Session["DealerCode"].ToString();

                ds = SysFunc.SP_Spare_Get_PartLocation(param);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    StrLocationNM = ds.Tables[0].Rows[0]["PartsTransLocDesc"].ToString();
                }

                return StrLocationNM;
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
                return "";
            }
        }
       private void ClearPartInfo()
        {
            ddlPartsNo.SelectedIndex = 0;
            txtPartItemDesc.Text = string.Empty;
            txtUnit.Text = string.Empty;
            txtIssueQty.Text = string.Empty;
            txtLocCode.Text = string.Empty;
            txtLocDesc.Text = string.Empty;
            txtRemaining.Text = string.Empty;
            txtTotalStock.Text = string.Empty;
            txtStockLoc.Text = string.Empty;
            txtRetQty.Text = string.Empty;
            txtTotalAmt.Text = string.Empty;
            txtIssueRate.Text = string.Empty;
        }
        protected void BtnClear1_Click(object sender, EventArgs e)
        {
            ClearPartInfo();
             ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);
        }
        protected void BtnAdd_Click(object sender, EventArgs e)
        {   
            DataSet ds = new DataSet();
            double DIssueQty = 0.0, DRetQty = 0.0,DRemainingQty=0.0;
            try
            {
                if (ddlStockReturnNo.SelectedIndex != 0)
                {   
                        SysFunc.UserMsg(LbErr, Color.Red, "The Return is already Saved; now you could not change/delete the record...");
                        return;
                }
                if (ddlPartsNo.SelectedIndex == 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Select a Part.");
                    ddlPartsNo.Focus();
                    return;
                }
                if (txtLocCode.Text.Trim() == string.Empty)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Location not available");
                    return;
                }
                DIssueQty = Convert.ToDouble(txtIssueQty.Text == "" ? "0" : txtIssueQty.Text);
                DRetQty = Convert.ToDouble(txtRetQty.Text == "" ? "0" : txtRetQty.Text);
                DRemainingQty = Convert.ToDouble(txtRemaining.Text.Trim() == "" ? "0" : txtRemaining.Text.Trim());
                if (DIssueQty <= 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Issue quantity should not be Zero (0)", txtRemaining);
                    return;
                }
                if (DRemainingQty <= 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Remaining quantity should not be Zero (0)", txtRemaining);
                    return;
                }
                if (DRetQty <= 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Returne quantity should not be Zero (0)", txtIssueQty);
                    return;
                }
                if (DRetQty > DRemainingQty)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Remaining quantity can not be greater than Return quantity", txtIssueQty);
                    return;
                }
                else
                {
                    LbErr.Text = "";
                }

                ds = (DataSet)ViewState["ObjPOds"];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dtRow in ds.Tables[0].Rows)
                    {
                        if (ddlPartsNo.SelectedValue.ToString().Trim() == dtRow["ItemCode"].ToString())
                        {
                            SysFunc.UserMsg(LbErr, Color.Red, "Item Code : '" + ddlPartsNo.SelectedValue.ToString() + "' is already exists.");
                            return;
                        }
                    }
                }
               AddNewRow();
               ClearDetail();
                
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);

            }

        }
        private void ClearDetail()
        {
            ddlPartsNo.SelectedIndex = 0;
            txtPartItemNo.Text = string.Empty;
            txtPartItemDesc.Text = string.Empty;
            txtUnit.Text = string.Empty;
            txtUnitCode.Text = string.Empty;
            txtLocCode.Text = string.Empty;
            txtLocDesc.Text = string.Empty;
            txtRemaining.Text = string.Empty;
            txtIssueQty.Text = string.Empty;
            txtTotalStock.Text = string.Empty;
            txtStockLoc.Text = string.Empty;
            txtRetQty.Text = string.Empty;
            txtTotalAmt.Text = string.Empty;
            txtIssueRate.Text = string.Empty;
        }
        private void AddNewRow()
        {
            DataSet ds = new DataSet();
            ds = (DataSet)ViewState["ObjPOds"];
            DataRow row = ds.Tables[0].NewRow();
            row["ItemCode"] = ddlPartsNo.SelectedValue.ToString().Trim();
            row["PartItemNo"] = txtPartItemNo.Text;
            row["PartItemName"] = txtPartItemDesc.Text.Trim();
            row["Unit"] = txtUnit.Text.Trim();
            row["UnitCode"] = txtUnitCode.Text.Trim();
            row["RemainingQty"] = txtRemaining.Text.Trim();
            row["ReturnQty"] = txtRetQty.Text.Trim();
            row["Amount"] = txtTotalAmt.Text.Trim();
            row["Rate"] = txtIssueRate.Text.Trim();
            row["Location"] = txtLocDesc.Text;
            row["LocationCode"] = txtLocCode.Text;
            ds.Tables[0].Rows.Add(row);
            GvMain.DataSource = ds.Tables[0];
            GvMain.DataBind();
            ViewState["ObjPOds"] = ds;
        }
        
        protected void BtnPrint_Click(object sender, EventArgs e)
        {

        }
        protected void btnPosting_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2549", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            try
            {
                
                if (ddlStockReturnNo.SelectedIndex != 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "The Return is already Saved; now you could not change/delete the record...");
                    return;
                }
                DataTable dtIssueData = new DataTable();
                dtIssueData=SysFunc.GetData("Select IssueDate,ReqSource,JobCardNO,InvoiceNo from StockIssue Where DealerCode='" + Session["DealerCode"].ToString() + "' AND IssueNo='" + ddlIssueNo.SelectedValue.ToString().Trim() + "'");
                string strIssueDate = Convert.ToDateTime(dtIssueData.Rows[0]["IssueDate"].ToString()).ToString("dd/MM/yyyy");
                string strReqSource = dtIssueData.Rows[0]["ReqSource"].ToString();
                string strJobCardNO = dtIssueData.Rows[0]["JobCardNO"].ToString();
                string strInvoiceNo = dtIssueData.Rows[0]["InvoiceNo"].ToString();
                if (Convert.ToDateTime(SysFunc.SaveDate(txtRetDate.Text)) < Convert.ToDateTime(SysFunc.SaveDate(strIssueDate)))
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Retrun date should not be less than issuance date...");
                    return;
                }
                else
                {
                    LbErr.Text = "";
                }
                bool chk = false;
                Posting(ref chk, strIssueDate, strReqSource, strJobCardNO, strInvoiceNo);
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

            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }
        protected void GvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataSet ds = new DataSet();
            
            
            if (e.Row.RowType == DataControlRowType.DataRow && !(GvMain.EditIndex == e.Row.RowIndex))
            {
                Session["dsParts"] = ds;
                string strTotals;
                decimal dtotalvalue;
                //Sum of Return Qty
                strTotals = ((Label)e.Row.FindControl("LbRetQty")).Text;
                dtotalvalue = Convert.ToDecimal(strTotals);
                sumFooterRetQty += dtotalvalue;

                //Sum of Total Amount
                strTotals = ((Label)e.Row.FindControl("LbAmount")).Text;
                dtotalvalue = Convert.ToDecimal(strTotals);
                sumFooterAmount += dtotalvalue;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //Set Sum of Return Qty in Footer
                Label lblFooterTotalQty = (Label)e.Row.FindControl("lblFooterTotalQty");
                lblFooterTotalQty.Text = sumFooterRetQty.ToString();

                //Set Sum of Amount in Footer
                Label lblFooterTotalAmount = (Label)e.Row.FindControl("lblFooterTotalAmount");
                lblFooterTotalAmount.Text = sumFooterAmount.ToString();
            }
        }

        protected void ddlIssueNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_ddlPartsNo();
        }

        protected void ddlPartsNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPartsNo.SelectedIndex == 0)
            {
                return;
            }
            DisplayItem();
        }

        protected void ddlStockReturnNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetReturnData();
        }

        protected void BtnRemovePart_Click(object sender, ImageClickEventArgs e)
        {
           
            DataSet dsDelete = new DataSet();
            try
            {
                if (ddlStockReturnNo.SelectedIndex != 0)
                {

                    SysFunc.UserMsg(LbErr, Color.Red, "The Return is already Saved; now you could not change/delete the record...");
                    return;
                }
                dsDelete = (DataSet)ViewState["ObjPOds"];

                ImageButton BtnLnk = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;

                dsDelete.Tables[0].Rows[rowGvIndex].Delete();
                GvMain.DataSource = dsDelete.Tables[0];
                GvMain.DataBind();
                dsDelete.Tables[0].AcceptChanges();
                LbErr.Text = "";
                ViewState["ObjPOds"] = dsDelete;
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        
        }

        protected void GvMain_RowEditing(object sender, GridViewEditEventArgs e)
        {
            DataSet ds = new DataSet();
            if (ddlStockReturnNo.SelectedIndex != 0)
            {

                SysFunc.UserMsg(LbErr, Color.Red, "The Return is already Saved; now you could not change/delete the record...");
                return;
            }
            GvMain.EditIndex = e.NewEditIndex;
            ds = (DataSet)ViewState["ObjPOds"];
            GvMain.DataSource = ds.Tables[0];
            GvMain.DataBind();

        }

        protected void GvMain_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataSet ds = new DataSet();
            double DQty = 0.0,DRate=0.0,DRemainingQty=0.0,DTotal=0.0;
            try
            {
                Label lblItemCode = (Label)GvMain.Rows[e.RowIndex].FindControl("lblEditItemCode");
                TextBox txtEditRetQty = (TextBox)GvMain.Rows[e.RowIndex].FindControl("txtEditRetQty");
                Label lblEditRate = (Label)GvMain.Rows[e.RowIndex].FindControl("lblEditRate");
                Label lblEditRemainingQty = (Label)GvMain.Rows[e.RowIndex].FindControl("lblEditRemainingQty");

                DQty = Convert.ToDouble(txtEditRetQty.Text.Trim() == "" ? "0" : txtEditRetQty.Text.Trim());
                DRate = Convert.ToDouble(lblEditRate.Text.Trim() == "" ? "0" : lblEditRate.Text.Trim());
                DRemainingQty = Convert.ToDouble(lblEditRemainingQty.Text.Trim() == "" ? "0" : lblEditRemainingQty.Text.Trim());

                if (DQty <= 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Return Quantity can not be zero(0)");
                    return;
                }
                if (DQty > DRemainingQty)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Return Quantity can not be greater then Remaining quantity");
                    return;
                }
                DTotal = DRate * DQty;
                ds = (DataSet)ViewState["ObjPOds"];
                foreach (DataRow dr in ds.Tables[0].Rows) // search whole table
                {
                    if (dr["ItemCode"].ToString() == lblItemCode.Text) // if id==2
                    {
                        dr["ReturnQty"] = DQty.ToString();
                        dr["Amount"] = DTotal.ToString();
                    }
                }
                GvMain.EditIndex = -1;
                GvMain.DataSource = ds.Tables[0];
                GvMain.DataBind();
                ViewState["ObjPOds"] = ds;
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }

        protected void GvMain_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataSet ds = new DataSet();
            GvMain.EditIndex = -1;
            ds = (DataSet)ViewState["ObjPOds"];
            GvMain.DataSource = ds.Tables[0];
            GvMain.DataBind();
        }

    }
}