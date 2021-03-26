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
    public partial class PurchaseReturn : System.Web.UI.Page
    {
        MainBLL objMBLL = new MainBLL();
        SysFunctions SysFunc = new SysFunctions();
        Transaction ObjTrans = new Transaction();
        SecurityBll sec = new SecurityBll();
        clsLookUp clslook = new clsLookUp();
        static int btnValue = 0;
        DataSet ObjPOds;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2545", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }

            if (Session["LookUpData"] != null)
            {
                int intlookupid = Convert.ToInt32(ViewState["lookupid"]);
                //lookUptext(intlookupid);
                if (intlookupid == 69)
                {
                    ddlPR.SelectedValue = Session["LookUpData"].ToString();
                    ddlPR_SelectedIndexChanged(null, null);
                   // GetReturnData();
                   // FillGV();

                }
                if (intlookupid == 63)
                {
                    DisplayItem();
                    txtIssueQty.Focus();
                }

                if (txtRemarks.Text.Contains("&nbsp;"))
                {
                    txtRemarks.Text = "";
                }
            }



            

            if (!IsPostBack)
            {
                if (Session["TransCode"] != null)
                {
                    SqlParameter[] param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5)};
                    param[0].Value = Session["DealerCode"].ToString();
                    //query += " Select a.PRNo, a.PRDate, b.VendorDesc from  PRMaster a Inner Join Vendor b On  a.VendorCode = b.VendorCode " + Environment.NewLine;
                    LoadPR_DDL();
                    objMBLL.FillDrp_SP(ddlVendor, "sp_2w_Spare_POVendor_Select", "VendorCode", "VendorDesc", param, true, "", false, "");
                    createPartsDT();

                    if (txtRetDate.Text.Length == 0)
                    {
                        txtRetDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    }

                    string leadId = Session["TransCode"].ToString();
                    ddlPR.SelectedValue = leadId;

                    ddlPR_SelectedIndexChanged(null, null);


                }
                else
                {
                    lblMode.Text = "Add Mode";
                    SqlParameter[] param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5)};
                    param[0].Value = Session["DealerCode"].ToString();
                    //query += " Select a.PRNo, a.PRDate, b.VendorDesc from  PRMaster a Inner Join Vendor b On  a.VendorCode = b.VendorCode " + Environment.NewLine;
                    LoadPR_DDL();
                    objMBLL.FillDrp_SP(ddlVendor, "sp_2w_Spare_POVendor_Select", "VendorCode", "VendorDesc", param, true, "", false, "");
                    createPartsDT();

                    if (txtRetDate.Text.Length == 0)
                    {
                        txtRetDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    }

                }

                //Fill RegNo Drop Down List
                //string[] Columns = new string[] { "a.ItemCode", "b.PartItemNo", "b.ItemDesc", "b.UnitCode" };
                //SysFunc.GetMultiColumnsDDL(ddlPR, Columns, "GRNDetail a,Item b", "DealerCode ='" + Session["DealerCode"].ToString() + "' ", "RegNo");

            }
            btnValue = 0;
            Session["TransCode"] = null;
            Session["LookUpData"] = string.Empty;
        }
        private void LoadPR_DDL()
        {
            string WhereQuery = "b.DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') " +
                 " and a.DelFlag = 'N' " +
                 " and a.VendorCode = b.VendorCode ";
            //"Order by a.PRNo Desc";
            string[] Columns = new string[] { "a.PRNo", "a.PRDate", "b.VendorDesc" };
            SysFunc.GetMultiColumnsDDL(ddlPR, Columns, "PRMaster a, Vendor b", WhereQuery, "PRNo", "Order by a.PRNo Desc",false,false);
        }
        private void createPartsDT()
        {
            ObjPOds = new DataSet();
            ObjPOds.Tables.Add("PurchaseReturn");
            ObjPOds.Tables["PurchaseReturn"].Columns.Add(new DataColumn("Sno", typeof(string)));
            ObjPOds.Tables["PurchaseReturn"].Columns.Add(new DataColumn("ItemCode", typeof(string)));
            ObjPOds.Tables["PurchaseReturn"].Columns.Add(new DataColumn("PartItemNo", typeof(string)));
            ObjPOds.Tables["PurchaseReturn"].Columns.Add(new DataColumn("PartItemName", typeof(string)));
            ObjPOds.Tables["PurchaseReturn"].Columns.Add(new DataColumn("Unit", typeof(string)));
            ObjPOds.Tables["PurchaseReturn"].Columns.Add(new DataColumn("IssueQty", typeof(string)));
            ObjPOds.Tables["PurchaseReturn"].Columns.Add(new DataColumn("Amount", typeof(string)));
            ObjPOds.Tables["PurchaseReturn"].Columns.Add(new DataColumn("Location", typeof(string)));
            ObjPOds.Tables["PurchaseReturn"].Columns.Add(new DataColumn("LocationCode", typeof(string)));
            ObjPOds.Tables["PurchaseReturn"].Columns.Add(new DataColumn("UnitCode", typeof(string)));
            ObjPOds.Tables["PurchaseReturn"].Columns.Add(new DataColumn("Rate", typeof(string)));
            ObjPOds.Tables["PurchaseReturn"].Columns.Add(new DataColumn("AvgRate", typeof(string)));
            ObjPOds.Tables["PurchaseReturn"].Columns.Add(new DataColumn("GSTAmount", typeof(string)));
            ObjPOds.Tables["PurchaseReturn"].Columns.Add(new DataColumn("FurTaxAmt", typeof(string)));
            ObjPOds.Tables["PurchaseReturn"].Columns.Add(new DataColumn("ExTaxAmt", typeof(string)));

            GvMain.DataSource = ObjPOds.Tables["PurchaseReturn"]; GvMain.DataBind(); ViewState["ObjPOds"] = ObjPOds;
        }

        #region // Functions //



        //private void lookUptext(int lookUpID)
        //{
        //    //Selection values display

        //    switch (lookUpID)
        //    {
        //        case 63:
        //            GetLookUpsValue(ref txtPartItemNo, ref txtPartItemDesc, ref txtUnitCode, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ViewState["ixd3"].ToString());
        //            break;

        //        case 67:

        //            GetLookUpsValue(ref txtGRNNo, ref txtGRNNo, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString());
        //            break;
        //        case 69:

        //            GetLookUpsValue(ref txtReturnNo, ref txtRetDate, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString());
        //            break;
        //        default:
        //            break;
        //    }
        //}

        ////private void GetLookUpsValue(ref TextBox txt1, ref TextBox txt2, ref TextBox txt3, string ixd1, string ixd2, string ixd3)
        ////{
        ////    if (Session["LookUpData"] != null)
        ////    {
        ////        GridViewRow Drow = (GridViewRow)Session["LookUpData"];
        ////        txt1.Text = (txt1 != null ? Drow.Cells[Convert.ToInt32(ixd1)].Text.ToString() : "");
        ////        txt2.Text = (txt2 != null ? Drow.Cells[Convert.ToInt32(ixd2)].Text.ToString() : "");
        ////        txt3.Text = (txt3 != null ? Drow.Cells[Convert.ToInt32(ixd3)].Text.ToString() : "");
        ////        Session["LookUpData"] = null;
        ////    }
        ////}
        private void GetReturnData()
        {
            DataSet ds = new DataSet();
            try
            {


                SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char),
                                    new SqlParameter("@ReturnNO",SqlDbType.Char)
                                   };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlPR.SelectedValue.ToString();
                ds = SysFunc.FillDataSet("Sp_2W_PRMaster_Select", param);//ObjDLL.SP_Parts_Get_PurchaseReturn_Data(param);
                DateTime Retdate = Convert.ToDateTime(ds.Tables[0].Rows[0]["PRDate"].ToString());
                
                ddlVendor.SelectedValue = ds.Tables[0].Rows[0]["VendorCode"].ToString();
                LoadGRN_DDL();
                ddlGRNNo.SelectedValue = ds.Tables[0].Rows[0]["GRNNo"].ToString();
                txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                txtTotalAmount.Text = ds.Tables[0].Rows[0]["TotalAmount"].ToString();
                txtTotalQty.Text = ds.Tables[0].Rows[0]["TotalQty"].ToString();
                txtRemarks.Focus();
                txtRetDate.Text = Retdate.ToString("dd-MM-yyyy");
                LoadParts_DDL();
            }
            catch (Exception ex)
            {
                LbErr.Text = ex.Message;
            }
        }
        private void DisplayItem()
        {

            DataSet ds = new DataSet();
            DataSet dsItemInfo = new DataSet();
            try
            {
                //txtPartItem.Text = ObjGeneral.GetStringValuesAgainstCodes("ItemCode", txtItemCode.Text.Trim(), "PartItemNo", "Item");
                SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                   new SqlParameter("@ItemCode",SqlDbType.Char,8),
                                       new SqlParameter("@GRNNo",SqlDbType.Char,8)};

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = txtItemCode.Text.Trim();
                param[2].Value = ddlGRNNo.SelectedValue.ToString ().Trim();
                dsItemInfo = SysFunc.FillDataSet("sp_Item_Select", param);

                txtPartItem.Text = dsItemInfo.Tables[0].Rows[0]["PartItemNo"].ToString();
                txtPartItemDesc.Text = dsItemInfo.Tables[0].Rows[0]["ItemDesc"].ToString(); 
                txtLocCode.Text = dsItemInfo.Tables[0].Rows[0]["PartsTransLocCode"].ToString();//ObjGeneral.GetStringValuesAgainstCodes("GRNNO", txtGRNNo.Text, "PartsTransLocCode", "GRNDetail", " and ItemCode='" + txtPartItemNo.Text + "'");
                txtLocDesc.Text = dsItemInfo.Tables[0].Rows[0]["PartsTransLocDesc"].ToString();//GetPartLocation(txtLocCode.Text);
                txtUnit.Text = dsItemInfo.Tables[0].Rows[0]["UnitDesc"].ToString();//ObjGeneral.GetStringValuesAgainstCodes("UnitCode", txtUnitCode.Text.Trim(), "UnitDesc", "Unit");
                txtIssueQty.Text = dsItemInfo.Tables[0].Rows[0]["AccQty"].ToString();//ObjGeneral.GetStringValuesAgainstCodes("ItemCode", txtPartItemNo.Text, "AccQty", "GRNDetail", "And GRNNo = '" + txtGRNNo.Text + "' ");
                txtRemaining.Text = dsItemInfo.Tables[0].Rows[0]["RemainingQty"].ToString(); //ObjGeneral.GetStringValuesAgainstCodes("ItemCode", txtPartItemNo.Text, "AccQty-RetQty", "GRNDetail", "And GRNNo = '" + txtGRNNo.Text + "' ");
                txtRate.Text = dsItemInfo.Tables[0].Rows[0]["ExclRate"].ToString();//ObjGeneral.GetStringValuesAgainstCodes("ItemCode", txtPartItemNo.Text, "ExclRate", "GRNDetail", "And GRNNo = '" + txtGRNNo.Text + "' ");
                txtGstAmount.Text = dsItemInfo.Tables[0].Rows[0]["GSTAmount"].ToString();
                txtFGSTAmount.Text = dsItemInfo.Tables[0].Rows[0]["FurTaxAmt"].ToString();
                txtExTaxAmount.Text = dsItemInfo.Tables[0].Rows[0]["ExTaxAmt"].ToString();
                txtGstPerc.Text = dsItemInfo.Tables[0].Rows[0]["GSTPercent"].ToString() +"%";
                txtFGSTPerc.Text = dsItemInfo.Tables[0].Rows[0]["FurTaxPercent"].ToString() + "%";
                txtExTaxPerc.Text = dsItemInfo.Tables[0].Rows[0]["ExTaxPercent"].ToString() + "%";
                
                SqlParameter[] paramds = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                   new SqlParameter("@ItemCode",SqlDbType.Char,8),
                                         new SqlParameter("@PartsTransLocCode",SqlDbType.VarChar,10)};
                paramds[0].Value = Session["DealerCode"].ToString();
                paramds[1].Value = txtItemCode.Text.Trim();
                paramds[2].Value = txtLocCode.Text.Trim();

                ds = SysFunc.FillDataSet("sp_PartStockInHand", paramds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtTotalStock.Text = ds.Tables[0].Rows[0]["Balance"].ToString();
                    txtStockLoc.Text = ds.Tables[0].Rows[0]["ItemLocBalance"].ToString();
                }

                //ds = ObjDLL.PartStockAtLoaction(txtPartItemNo.Text, SessionInformation.DealerCode, txtLocCode.Text);
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    txtStockLoc.Text = ds.Tables[0].Rows[0]["Balance"].ToString();
                //}
                //ds = ObjDLL.ItemAvgCost(txtPartItemNo.Text, SessionInformation.DealerCode);
                ds = SysFunc.ItemAvgCost(txtItemCode.Text, Session["DealerCode"].ToString());
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtAvgCost.Text = ds.Tables[0].Rows[0]["Cost"].ToString();
                }

                //txtRate.Text = ObjGeneral.GetStringValuesAgainstCodes("ItemCode", txtPartItemNo.Text, "ExclRate", "GRNDetail", "And GRNNo = '" + txtGRNNo.Text + "' ");

            }
            catch (Exception ex)
            {
                LbErr.Text = ex.Message;
            }

        }

        private void Get_locationWisePartData()
        {

            double DStockLoc = 0.0;
            DataSet ds2 = new DataSet();
            try
            {
                if (txtPartItem.Text != "")
                {
                    ds2 = SysFunc.PartStockAtLoaction(txtItemCode.Text, Session["DealerCode"].ToString(), txtLocCode.Text);
                    DStockLoc = Convert.ToDouble(ds2.Tables[0].Rows[0]["Balance"]);
                    //txtBalanceQty.Text = Convert.ToString(Convert.ToInt32(DStockLoc));
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
        private void Save_InsertMasterDetail(SqlTransaction Trans, ref bool Check, ref string PRNO)
        {

            //SqlTransaction Trans = null;
            //Check = false;
            //if (ObjTrans.BeginTransaction(ref Trans) == false)
            //{
            //    return;
            //}
            string StrIssueDate = "";
            try
            {


                StrIssueDate = txtRetDate.Text;
                //|| txtReturnNo.Text != "__/_____"
                if (ddlPR.SelectedIndex != 0)
                {
                    if ((SysFunc.CodeExists("PRMaster", "RPNO", ddlPR.SelectedValue.ToString(), " and DelFlag = 'N'")) == false)
                    {
                        SysFunc.UserMsg(LbErr, Color.Red, "Purchase Return Reference No. does not exist...", txtPartItem);
                        return;
                    }
                    if ((SysFunc.IsPosted("PRMaster", "RPNO", ddlPR.SelectedValue.ToString(), Session["DealerCode"].ToString())) == true) //. IsPosted("ChargeOutMaster", "ChargeOutNo", txtChargeOutNo._String) = True Then
                    {
                        SysFunc.UserMsg(LbErr, Color.Red, "The Purchase Return Document is already posted; now you could not change/delete/Post the record...", txtPartItem);
                        return;
                    }
                }
                SqlParameter[] param = {
                                       new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                                       new SqlParameter("@PRNo",SqlDbType.Char,8),//1
                                       new SqlParameter("@PRDate",SqlDbType.DateTime),//2
                                       new SqlParameter("@VendorCode",SqlDbType.Char,6),//3
                                       new SqlParameter("@Remarks",SqlDbType.Text),//4
                                       new SqlParameter("@TotalAmount",SqlDbType.Float),//5
                                       new SqlParameter("@TotalQty",SqlDbType.Float),//6
                                       new SqlParameter("@UpdUser",SqlDbType.VarChar,50),//7
                                       new SqlParameter("@UpdTerm",SqlDbType.VarChar,50),//8
                                       new SqlParameter("@GRNno",SqlDbType.Char,8),//9
                                   };
                param[0].Value = Session["DealerCode"].ToString();

                param[1].Value = SysFunc.AutoGen("PRMaster", "PRNo", txtRetDate.Text.Trim()); ;
                param[2].Value = SysFunc.SaveDate(txtRetDate.Text);
                param[3].Value = ddlVendor.SelectedValue.ToString();
                param[4].Value = txtRemarks.Text;
                param[5].Value = txtTotalAmount.Text;
                param[6].Value = txtTotalQty.Text.Trim();
                param[7].Value = Session["UserName"].ToString();
                param[8].Value = GlobalVar.mUserIPAddress;
                param[9].Value = ddlGRNNo.SelectedValue.ToString();

                if (SysFunc.sp_W2_PRMaster_Insert(Trans, param))
                {
                     PRNO = param[1].Value.ToString();
                    //txtReturnNo.Text = param[1].Value.ToString();
                    for (int i = 0; i < GvMain.Rows.Count; i++)
                    {
                        SqlParameter[] param1 = {
                                        new SqlParameter("@DealerCode",SqlDbType.Char ),//0
                                        new SqlParameter("@PRNo",SqlDbType.Char,8),//1
                                        new SqlParameter("@ItemCode",SqlDbType.Char,8),//2
                                        new SqlParameter("@PartsTransLocCode",SqlDbType.VarChar,7),//3
                                        new SqlParameter("@PurRetQty",SqlDbType.Float),//4
                                        new SqlParameter("@Rate",SqlDbType.Float),//5
                                        new SqlParameter("@Amount",SqlDbType.Float),//6
                                        new SqlParameter("@AvgRate",SqlDbType.Float),//7
                                        new SqlParameter("@GSTPercent",SqlDbType.Float),//8
                                        new SqlParameter("@GSTAmount",SqlDbType.Float),//9
                                        new SqlParameter("@ExTaxPercent",SqlDbType.Float),//10
                                        new SqlParameter("@ExTaxAmt",SqlDbType.Float),//11
                                        new SqlParameter("@FurTaxPercent",SqlDbType.Float),//12
                                        new SqlParameter("@FurTaxAmt",SqlDbType.Float),//13
                                        };

                        Label LbItemCode = (Label)GvMain.Rows[i].FindControl("LbItemCode");
                        Label LbPartsTransLocCode = (Label)GvMain.Rows[i].FindControl("LbPartsTransLocCode");
                        HiddenField HDPartsTransLocCode = (HiddenField)GvMain.Rows[i].FindControl("HDPartsTransLocCode");

                        Label LbIssueQty = (Label)GvMain.Rows[i].FindControl("LbRetQty");
                        //Label LbAvgCost = (Label)GvMain.Rows[i].FindControl("LbAvgCost");
                        Label LbAmount = (Label)GvMain.Rows[i].FindControl("LbAmount");
                        Label LbRate = (Label)GvMain.Rows[i].FindControl("LbRate");
                        Label LbAvgCost = (Label)GvMain.Rows[i].FindControl("LbAvgCost");
                        Label lblGstAMount = (Label)GvMain.Rows[i].FindControl("lblGstAMount");
                        Label lblFGSTAMount = (Label)GvMain.Rows[i].FindControl("lblFGSTAMount");
                        Label lblExTaxAMount = (Label)GvMain.Rows[i].FindControl("lblExTaxAMount");
                        if (LbAvgCost.Text.Trim() == "") {
                            LbAvgCost.Text = "0";

                        }
                        
                        param1[0].Value = Session["DealerCode"].ToString();
                        param1[1].Value = PRNO;
                        param1[2].Value = LbItemCode.Text;
                        //param1[3].Value = txtGRNNo.Text;
                        param1[3].Value = HDPartsTransLocCode.Value;
                        param1[4].Value = double.Parse(LbIssueQty.Text);
                        param1[5].Value = double.Parse(LbRate.Text);
                        param1[6].Value = double.Parse(LbAmount.Text);
                        param1[7].Value = double.Parse(LbAvgCost.Text);
                        param1[8].Value = double.Parse(ViewState["GstPerc"].ToString() == "" ? "0" : ViewState["GstPerc"].ToString());
                        param1[9].Value = double.Parse(lblGstAMount.Text == "" ? "0" : lblGstAMount.Text);
                        param1[10].Value = double.Parse(ViewState["ExTaxperc"].ToString() == "" ? "0" : ViewState["ExTaxperc"].ToString());
                        param1[11].Value = double.Parse(lblExTaxAMount.Text == "" ? "0" : lblExTaxAMount.Text);
                        param1[12].Value = double.Parse(ViewState["FGSTPerc"].ToString() == "" ? "0" : ViewState["FGSTPerc"].ToString());
                        param1[13].Value = double.Parse(lblFGSTAMount.Text == "" ? "0" : lblFGSTAMount.Text);
                        
                        
                        if(SysFunc.ExecuteSP_NonQuery("sp_2W_PRDetail_Insert", param1, Trans))
                        {

                            Check = true;
                        }
                        else
                        {
                            Check = false;
                            break;
                        }
                    }

                    //if (Check == false)
                    //{
                    //    ObjTrans.RollBackTransaction(ref Trans);
                    //    //SysFunc.UserMsg(LbErr, Color.Red, "Record can not save. Try again...");
                    //}
                    //else
                    //{
                    //    ObjTrans.CommittTransaction(ref Trans);
                    //    //SysFunc.UserMsg(LbErr, Color.Green, "Record saved successfuly. Last code: " + ddlPR.SelectedValue.ToString());
                    //    //CtlClear("T");
                    //}

                }



            }
            catch (Exception ex)
            {
                //ObjTrans.RollBackTransaction(ref Trans);
                LbErr.Text = ex.Message;
            }
        }
        private bool Post_InsertMasterDetail(SqlTransaction Trans)
        {
            string PRNO = string.Empty;
            bool Check = false;
            try
            {
                SqlParameter[] param = {
                                       new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                       new SqlParameter("@IssueNo",SqlDbType.Char),//1
                                       new SqlParameter("@IssueDate",SqlDbType.DateTime),//2
                                       new SqlParameter("@SIRNo",SqlDbType.Char),//3
                                       new SqlParameter("@ReqSource",SqlDbType.VarChar),//4
                                       new SqlParameter("@JobCardNO",SqlDbType.Char),//5
                                       new SqlParameter("@InvoiceNo",SqlDbType.Char),//6
                                       new SqlParameter("@Remarks",SqlDbType.VarChar),//7
                                       new SqlParameter("@UpdUser",SqlDbType.VarChar),//8
                                       new SqlParameter("@UpdDate",SqlDbType.DateTime),//9
                                       new SqlParameter("@UpdTime",SqlDbType.DateTime),//10
                                       new SqlParameter("@UpdTerm",SqlDbType.VarChar),//11
                                       
                                       
                                   };
                param[0].Value = Session["DealerCode"].ToString();
                PRNO = SysFunc.AutoGen("StockIssue", "IssueNo", txtRetDate.Text.Trim());
                param[1].Value = PRNO;
                param[2].Value = SysFunc.SaveDate(txtRetDate.Text); ;
                //param[3].Value = txtSIRNO.Text;
                //param[4].Value = txtSource.Text;
                // param[5].Value = txtCardNo.Text;
                //param[6].Value = txtInvoiceNo.Text;
                param[7].Value = txtRemarks.Text.Replace("'", " ");
                param[8].Value = Session["UserName"].ToString();
                param[9].Value = SysFunc.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
                param[10].Value = SysFunc.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
                param[11].Value = "";

                if (SysFunc.SP_Parts_Insert_StockIssue(Trans, param))
                {
                    for (int i = 0; i < GvMain.Rows.Count; i++)
                    {
                        SqlParameter[] param1 = {
                                        new SqlParameter("@DealerCode",SqlDbType.Char ),//0
                                        new SqlParameter("@IssueNo",SqlDbType.Char),//1
                                        new SqlParameter("@ItemCode",SqlDbType.Char),//2
                                        new SqlParameter("@SIRNo",SqlDbType.Char),//3
                                        new SqlParameter("@PartsTransLocCode",SqlDbType.Char),//4
                                        new SqlParameter("@IssueQty",SqlDbType.Float),//5
                                        new SqlParameter("@Rate",SqlDbType.Float),//6
                                        new SqlParameter("@Amount",SqlDbType.Float)//7
                                        };

                        Label LbItemCode = (Label)GvMain.Rows[i].FindControl("LbItemCode");
                        Label LbPartsTransLocCode = (Label)GvMain.Rows[i].FindControl("LbPartsTransLocCode");
                        Label LbIssueQty = (Label)GvMain.Rows[i].FindControl("LbIssueQty");
                        Label LbAvgCost = (Label)GvMain.Rows[i].FindControl("LbAvgCost");
                        Label LbAmount = (Label)GvMain.Rows[i].FindControl("LbAmount");

                        param1[0].Value = Session["DealerCode"].ToString();
                        param1[1].Value = PRNO;
                        param1[2].Value = LbItemCode.Text;
                        //param1[3].Value = txtSIRNO.Text;
                        param1[4].Value = LbPartsTransLocCode.Text;
                        param1[5].Value = LbIssueQty.Text;
                        param1[6].Value = LbAvgCost.Text;
                        param1[7].Value = LbAmount.Text;

                        if (SysFunc.Sp_Parts_Insert_StockIssueDetail(Trans, param1))
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
                return Check;
            }


            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
                return false;
            }

        }
        public bool IsPartOutOfStockAtLocation(string strItemCode, string strLocationCode, double dblQuantity = 0)
        {
            SqlDataReader drData = null;
            string Select_Qry = null;
            double stockqty = 0;

            Select_Qry = "Exec sp_PartStockAtLocation '" + Session["DealerCode"].ToString() + "', '" + strItemCode + "', '" + strLocationCode + "' ";

            try
            {
                if (SysFunc.ExecuteQuery(Select_Qry, ref drData) == true)
                {
                    if (drData.HasRows)
                    {
                        drData.Read();
                        stockqty = Convert.ToDouble(drData["Balance"]);
                    }
                    else
                    {
                        stockqty = 0;
                    }
                }

                if (stockqty >= dblQuantity && stockqty > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
                return false;
            }
            finally
            {
                if ((drData == null) == false)
                {
                    if (drData.IsClosed == false)
                        drData.Close();
                }
            }
        }

        //public bool UpdateStock(string ItemCode, string LocCode, double ChargeOutQty, double ChargeOutVal, ref SqlTransaction Trans)
        //{
        //    string Update_Qry = "Update ItemStock Set ";
        //    Update_Qry += " ChargeOutQty = ChargeOutQty + " + ChargeOutQty.ToString() + ",";
        //    Update_Qry += " ChargeOutVal = ChargeOutVal + " + ChargeOutVal.ToString() + "";
        //    Update_Qry += " Where ItemCode = '" + ItemCode + "'";
        //    Update_Qry += " AND PartsTransLocCode = '" + LocCode + "'";
        //    Update_Qry += " AND DealerCode = '" + SessionInformation.DealerCode + "'";
        //    try
        //    {
        //        if ((ObjMainBll.ExecuteQuery(Update_Qry, Trans) == true))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGeneral.UserMsg(LbErr, Color.Red, ex.Message);
        //        return false;
        //    }
        //}
        public bool ValidateWithGRN(string strPRNo, string strGRNNo)
        {
            SqlDataReader drData = null;
            string Select_Qry = null;


            Select_Qry = "Exec sp_2W_VarifyGRN '" + Session["DealerCode"].ToString() + "', '" + strPRNo + "', '" + strGRNNo + "' ";

            try
            {
                if (SysFunc.ExecuteQuery(Select_Qry, ref drData) == true)
                {
                    if (drData.HasRows)
                    {
                        drData.Read();
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
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
                return false;
            }
            finally
            {
                if ((drData == null) == false)
                {
                    if (drData.IsClosed == false)
                        drData.Close();
                }
            }
        }
        //public bool ValidateStock2(string strItemCode,) {
        //    int i = 0;
        //    //DataRow drPart = new DataRow();
        //    //string ItemCode = String.Empty;
        //    //string PartsTransLocCode = String.Empty;
        //    //double RequiredQty = 0;
        //    try {
        //        //foreach (DataRow drPart in dtPRDetail.Rows) {
        //            //ItemCode = DrPRDetail["ItemCode"].ToString();
        //            //PartsTransLocCode = DrPRDetail["PartsTransLocCode"].ToString().Trim();
        //            //RequiredQty = double.Parse(DrPRDetail["IssueQty"].ToString());
        //            if (!IsPartHaveQtyAtLocation(ItemCode, PartsTransLocCode, RequiredQty))
        //            {

        //                /////ShowItemStock(ItemCode, PartsTransLocCode);
        //                return false;
        //            }
        //        //}
        //        return true;
        //    }
        //    catch (Exception ex) {
        //        ObjGeneral.UserMsg(LbErr, Color.Red, ex.Message, txtReturnNo);
        //        return false;
        //    }
        //}
        private bool IsPartHaveQtyAtLocation(string strItemCode, string strLocationCode, double RequiredQty = 0)
        {
            SqlDataReader drData = null;
            // Warning!!! Optional parameters not supported
            string Select_Qry;
            double StockQty = 0;
            Select_Qry = "Exec sp_PartStockAtLocation '" + Session["DealerCode"].ToString() + "', '" + strItemCode + "','" + strLocationCode + "' ";
            try
            {
                if (SysFunc.ExecuteQuery(Select_Qry, ref drData) == true)
                {
                    if (drData.HasRows)
                    {
                        drData.Read();
                        StockQty = Convert.ToDouble(drData["Balance"]);
                    }
                    else
                    {
                        StockQty = 0;
                    }
                }
                if (((StockQty >= RequiredQty)
                            && (StockQty > 0)))
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
            finally
            {
                if (((drData == null)
                            == false))
                {
                    if ((drData.IsClosed == false))
                    {
                        drData.Close();
                    }
                }
            }
        }
        public bool UpdatePost(string StockRetNo, ref SqlTransaction Trans)
        {

            //Dim Update_Qry As String = " Update ChargeOutMaster Set PostFlag = 'Y' , PostDate = GetDate() " _
            //                        & " Where ChargeOutNo = '" & ChargeOutNo & "' " _
            //                        & " And DealerCode = '" & DEALER_CODE & "' "

            string Update_Qry = " Exec sp_PostPurchaseReturnActivity '" + StockRetNo + "','" + Session["DealerCode"].ToString() + "'";


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
            Check = false;
            string PRNO=string.Empty;
            try
            {
                if (GvMain.Rows.Count == 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Details should not be left blank...");
                    return;
                }

                if (ObjTrans.BeginTransaction(ref Trans) == false)
                {
                    return;
                }
                if (ddlPR.SelectedIndex == 0)
                {
                    Save_InsertMasterDetail(Trans, ref Check, ref PRNO);
                    //Trans = null;
                    
                    if (Check == true)
                    {
                        ///Start Post
                        foreach (GridViewRow gvr in GvMain.Rows)
                        {
                            Label LbItemCode = (Label)gvr.FindControl("LbItemCode");
                            Label LbQty = (Label)gvr.FindControl("LbRetQty");
                            Label LbRate = (Label)gvr.FindControl("LbRate");
                            HiddenField hdLocCode = (HiddenField)gvr.FindControl("HDPartsTransLocCode");

                            //if (ValidateWithGRN(txtReturnNo.Text, txtGRNNo.Text) == false)
                            //{
                            //    ObjTrans.RollBackTransaction(ref Trans);
                            //    ObjGeneral.UserMsg(LbErr, Color.Red, "Can Not Post, GRN Quantity not match", txtReturnNo);
                            //    Check = false;
                            //    return;
                            //}
                            //else
                            //{
                            //    Check = true;
                            //}
                            SqlParameter[] param = {new SqlParameter("@DealerCode",SqlDbType.Char,5 ),
                                               new SqlParameter("@GRNNo",SqlDbType.Char,8),
                                               new SqlParameter("@ItemCode",SqlDbType.Char,8),
                                               new SqlParameter("@Rate",SqlDbType.Float),
                                               new SqlParameter("@PurRetQty",SqlDbType.Float),
                                           };

                            param[0].Value = Session["DealerCode"].ToString();
                            param[1].Value = ddlGRNNo.SelectedValue.ToString().Trim();
                            param[2].Value = LbItemCode.Text;
                            param[3].Value = LbRate.Text;
                            param[4].Value = LbQty.Text;
                            if (SysFunc.sp_W2_GRNDetail_Update_PR(Trans, param) == false)
                            {
                                ObjTrans.RollBackTransaction(ref Trans);
                                SysFunc.UserMsg(LbErr, Color.Red, "Can Not Post, GRNDetail Quantity not Update");
                                Check = false;
                                ddlPR.SelectedIndex = 0;
                                return;
                            }
                            else
                            {
                                Check = true;
                            }
                            if (SysFunc.sp_W2_GRNMaster_Update_PR(Trans, param) == false)
                            {
                                ObjTrans.RollBackTransaction(ref Trans);
                                SysFunc.UserMsg(LbErr, Color.Red, "Can Not Post, GRNMaster Quantity not Update");
                                Check = false;
                                ddlPR.SelectedIndex = 0;
                                return;
                            }
                            else
                            {
                                Check = true;
                            }

                            if (IsPartHaveQtyAtLocation(LbItemCode.Text, hdLocCode.Value, Convert.ToDouble(LbQty.Text)) == false)
                            {
                                ObjTrans.RollBackTransaction(ref Trans);
                                LbErr.Text = "Can Not Post, No Quantity at Location";
                                SysFunc.UserMsg(LbErr, Color.Red, "Can Not Post, No Quantity at Location");
                                Check = false;
                                ddlPR.SelectedIndex = 0;
                                return;
                                /////ShowItemStock(ItemCode, PartsTransLocCode);
                            }
                            else
                            {
                                Check = true;
                            }
                        }
                        if (UpdatePost(PRNO, ref Trans) == false)
                        {
                            SysFunc.UserMsg(LbErr, Color.Red, "Can Not Post, Not found PRMaster");
                            ObjTrans.RollBackTransaction(ref Trans);
                            Check = false;
                            ddlPR.SelectedIndex = 0;
                            return;
                        }
                        else
                        {
                            Check = true;
                        }
                        
                        foreach (GridViewRow gvr in GvMain.Rows)
                        {
                            Label LbItemCode = (Label)gvr.FindControl("LbItemCode");
                            HiddenField hdLocCode = (HiddenField)gvr.FindControl("HDPartsTransLocCode");
                            Label LbQty = (Label)gvr.FindControl("LbRetQty");
                            Label LbVal = (Label)gvr.FindControl("LbAmount");
                            if (UpdateStock(LbItemCode.Text, hdLocCode.Value, Convert.ToDouble(LbQty.Text), Convert.ToDouble(LbVal.Text), ref Trans))
                            {
                                Check = true;
                            }
                            else
                            {
                                Check = false;
                            }
                        }
                    }
                }
                //else
                //{
                //    //if ((ObjGeneral.IsPosted("StockReturn", "ReturnNo", txtReturnNo.Text)) == true) //. IsPosted("ChargeOutMaster", "ChargeOutNo", txtChargeOutNo._String) = True Then
                //    //{
                //    //    ObjGeneral.UserMsg(LbErr, Color.Red, "The Charge Out Document is already posted; now you could not change/delete/Post the record...", txtPartItem);
                //    //    return;
                //    //}

                //    ///Start Post
                //    foreach (GridViewRow gvr in GvMain.Rows)
                //    {
                //        Label LbItemCode = (Label)gvr.FindControl("LbItemCode");
                //        Label LbQty = (Label)gvr.FindControl("LbRetQty");
                //        Label LbRate = (Label)gvr.FindControl("LbRate");
                //        HiddenField hdLocCode = (HiddenField)gvr.FindControl("HDPartsTransLocCode");

                //        //if (ValidateWithGRN(txtReturnNo.Text, txtGRNNo.Text) == false)
                //        //{
                //        //    ObjTrans.RollBackTransaction(ref Trans);
                //        //    ObjGeneral.UserMsg(LbErr, Color.Red, "Can Not Post, GRN Quantity not match", txtReturnNo);
                //        //    Check = false;
                //        //    return;
                //        //}
                //        //else
                //        //{
                //        //    Check = true;
                //        //}
                //        SqlParameter[] param = {new SqlParameter("@DealerCode",SqlDbType.Char,5 ),
                //                               new SqlParameter("@GRNNo",SqlDbType.Char,8),
                //                               new SqlParameter("@ItemCode",SqlDbType.Char,8),
                //                               new SqlParameter("@Rate",SqlDbType.Float),
                //                               new SqlParameter("@PurRetQty",SqlDbType.Float),
                //                           };

                //        param[0].Value = Session["DealerCode"].ToString();
                //        param[1].Value = ddlGRNNo.SelectedValue.ToString().Trim();
                //        param[2].Value = LbItemCode.Text;
                //        param[3].Value = LbRate.Text;
                //        param[4].Value = LbQty.Text;
                //        if (SysFunc.sp_W2_GRNDetail_Update_PR(Trans, param) == false)
                //        {
                //            ObjTrans.RollBackTransaction(ref Trans);
                //            SysFunc.UserMsg(LbErr, Color.Red, "Can Not Post, GRNDetail Quantity not Update");
                //            Check = false;
                //            return;
                //        }
                //        else
                //        {
                //            Check = true;
                //        }
                //        if (SysFunc.sp_W2_GRNMaster_Update_PR(Trans, param) == false)
                //        {
                //            ObjTrans.RollBackTransaction(ref Trans);
                //            SysFunc.UserMsg(LbErr, Color.Red, "Can Not Post, GRNMaster Quantity not Update");
                //            Check = false;
                //            return;
                //        }
                //        else
                //        {
                //            Check = true;
                //        }

                //        if (IsPartHaveQtyAtLocation(LbItemCode.Text, hdLocCode.Value, Convert.ToDouble(LbQty.Text)) == false)
                //        {
                //            ObjTrans.RollBackTransaction(ref Trans);
                //            SysFunc.UserMsg(LbErr, Color.Red, "Can not post, No quantity at location");
                //            Check = false;
                //            return;
                //            /////ShowItemStock(ItemCode, PartsTransLocCode);
                //        }
                //        else
                //        {
                //            Check = true;
                //        }
                //    }
                //    if (UpdatePost(ddlPR.SelectedValue.ToString(), ref Trans) == false)
                //    {
                //        SysFunc.UserMsg(LbErr, Color.Red, "Can Not Post, Not found ItemStock");
                //        ObjTrans.RollBackTransaction(ref Trans);
                //        Check = false;
                //        return;
                //    }
                //    else
                //    {
                //        Check = true;
                //    }
                //    //
                //    foreach (GridViewRow gvr in GvMain.Rows)
                //    {
                //        Label LbItemCode = (Label)gvr.FindControl("LbItemCode");
                //        HiddenField hdLocCode = (HiddenField)gvr.FindControl("HDPartsTransLocCode");
                //        Label LbQty = (Label)gvr.FindControl("LbRetQty");
                //        Label LbVal = (Label)gvr.FindControl("LbAmount");
                //        if (UpdateStock(LbItemCode.Text, hdLocCode.Value, Convert.ToDouble(LbQty.Text), Convert.ToDouble(LbVal.Text), ref Trans))
                //        {
                //            Check = true;
                //        }
                //        else
                //        {
                //            Check = false;
                //        }
                //    }
                //}
                
                if (Check == false)
                {
                    ObjTrans.RollBackTransaction(ref Trans);
                    SysFunc.UserMsg(LbErr, Color.Red, "Record can not save. Try again...");
                }
                else
                {

                    ObjTrans.CommittTransaction(ref Trans);
                    SysFunc.UserMsg(LbErr, Color.Green, "Record Posted successfuly.  Return No.: " + PRNO);
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
            SIRNo = SysFunc.GetStringValuesAgainstCodes("InvoiceNo", InvoiceNO, "SIRNo", "SIRMaster", "And DelFlag = 'N' ", Session["DealerCode"].ToString());

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
        public bool IsItemReturnedExta(string IssueNo, string ItemCode, ref SqlTransaction Trans)
        {

            string select_query = string.Empty;
            SqlDataReader rd = default(SqlDataReader);
            string strSIRNo = string.Empty;

            try
            {
                strSIRNo = SysFunc.GetStringValuesAgainstCodes("IssueNO", IssueNo, "SIRNo", "StockIssue",Session["DealerCode"].ToString());
                select_query = "Select ItemCode From SIRDetail with(NoLock) Where RecQty > 0 And SIRNo = '" + strSIRNo + "' And DealerCode = '" + Session["DealerCode"].ToString() + "' And ItemCode = '" + ItemCode + "' ";
                if (SysFunc.ExecuteQuery(select_query, ref  rd) == true)
                {
                    if (rd.HasRows)
                    {
                        rd.Read();
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
                return false;
            }
            finally
            {
                if ((rd == null) == false)
                {
                    if (rd.IsClosed == false)
                        rd.Close();
                }
            }
        }

        public bool UpdateStock(string ItemCode, string LocCode, double IssueQty, double IssueVal, ref SqlTransaction Trans)
        {
            string Update_Qry = "Update ItemStock Set " + " PurRetQty = PurRetQty + " + IssueQty + "," + " PurRetVal = PurRetVal + " + IssueVal + "" + " Where ItemCode = '" + ItemCode + "'" + " AND PartsTransLocCode = '" + LocCode + "'" + " AND DealerCode = '" + Session["DealerCode"].ToString() + "'";
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
                strSIRNo = SysFunc.GetStringValuesAgainstCodes("IssueNO", IssueNO, "SIRNo", "StockIssue", "", Session["DealerCode"].ToString());
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
                strSIRNo = SysFunc.GetStringValuesAgainstCodes("IssueNO", IssueNO, "SIRNo", "StockIssue", "", Session["DealerCode"].ToString());
                StrType = SysFunc.GetStringValuesAgainstCodes("SIRNo", strSIRNo, "Type", "SIRDetail", "And ItemCode='" + ItemCode + "'", Session["DealerCode"].ToString());
            }
            catch (Exception ex)
            {
                return false;
            }

            if (StrType == "P")
            {
                string Update_Qry = "Update JobCardPartsDetail Set " + " RecQty = RecQty - " + Convert.ToDouble(IssueQty) + "" + " Where JobCardCode = '" + JobCardNO + "'" + " and ItemCode = '" + ItemCode + "'" + " AND Type = 'P'" + " and DealerCode = '" + Session["DealerCode"].ToString() + "'";
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
                //Dim Update_Qry As String = "Update JobCardLubricateDetail Set " _
                // & " RecQty = RecQty + " & Val(IssueQty) & "" _
                // & " Where JobCardCode = '" & JobCardNO & "'" _
                // & " and ItemCode = '" & ItemCode & "'" _
                // & " and DealerCode = '" & DEALER_CODE & "'"

                //Modified by Yasir Godil (18-05-2006) 

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
            ddlPR.SelectedIndex = 0;
            txtRetDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            
            //txtIssueDate.Text = string.Empty;
            //txtSource.Text = string.Empty;
            //txtJCNo.Text = string.Empty;
            //txtInvoiceNo.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            ddlGRNNo.Items.Clear();
        }
        private void CtlClear(string strChk)
        {
            DataSet ds = new DataSet();
            try
            {
                ddlPR.SelectedIndex = 0;
                txtRetDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                if (strChk == "T")
                {
                    ddlVendor.SelectedIndex = 0;
                    ddlGRNNo.Items.Clear();
                    ddlParts.Items.Clear();
                    //txtIssueDate.Text = string.Empty;
                    //txtSource.Text = string.Empty;
                }
                //txtJCNo.Text = string.Empty;
                //txtInvoiceNo.Text = string.Empty;
                txtRemarks.Text = string.Empty;
                txtPartItem.Text = string.Empty;
                txtPartItemDesc.Text = string.Empty;
                txtUnit.Text = string.Empty;
                txtUnitCode.Text = string.Empty;
                txtIssueQty.Text = string.Empty;
                txtRemaining.Text = string.Empty;
                txtTotalStock.Text = string.Empty;
                txtStockLoc.Text = string.Empty;
                txtRetQty.Text = string.Empty;
                txtTotalAmt.Text = string.Empty;
                txtTotalAmount.Text = string.Empty;
                txtTotalQty.Text = string.Empty;
                //txtIssueRate.Text = string.Empty;
                txtLocCode.Text = string.Empty;
                txtLocDesc.Text = string.Empty;
                createPartsDT();
                LbErr.Text = string.Empty;
                txtGstPerc.Text = string.Empty;
                txtGstAmount.Text = string.Empty;
                txtFGSTAmount.Text = string.Empty;
                txtFGSTPerc.Text = string.Empty;
                txtExTaxAmount.Text = string.Empty;
                txtExTaxPerc.Text = string.Empty;
                //GvMain.DataSource = null;
                //GvMain.DataBind();

                //ds = (DataSet)ViewState["ObjPOds"];
                //ds.Tables["PurchaseReturn"].Rows.Clear();
                //ViewState["ObjPOds"] = ds;

            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }

        //private void FillFormData()
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlParameter[] param = {    new SqlParameter("@DealerCode",SqlDbType.Char,5),
        //                              new SqlParameter("@ChargeOutNo",SqlDbType.Char,10)
        //                           };
        //        param[0].Value = Session["DealerCode"].ToString();
        //        param[1].Value = txtReturnNo.Text.Trim();
        //        ds = SysFunc.Get_Parts_Get_ChargeOutDetail(param);

        //        //txtSIRNO.Text = ds.Tables[0].Rows[0]["SIRNo"].ToString();
        //        //txtSIRDate.Text = ds.Tables[0].Rows[0]["SIRDate"].ToString();
        //        //txtSource.Text = ds.Tables[0].Rows[0]["ReqSource"].ToString();
        //        //txtCardNo.Text = ds.Tables[0].Rows[0]["JobCardNO"].ToString();
        //        //txtInvoiceNo.Text = ds.Tables[0].Rows[0]["InvoiceNO"].ToString();
        //        //txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();

        //        FillGV();

        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGeneral.UserMsg(LbErr, Color.Red, ex.Message, txtReturnNo);
        //    }
        //}


        private void FillGV()
        {

            DataSet dsMain = new DataSet();
            try
            {

                SqlParameter[] param = {    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                      new SqlParameter("@ReturnNo",SqlDbType.Char,10)
                                   };
                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlPR.SelectedValue.ToString();
                dsMain = SysFunc.Get_Parts_Get_PRDetail(param);
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


        #endregion

        //protected void BtnVendorLookup_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        ViewState["lookupid"] = 39;
        //        ViewState["ixd1"] = 1;
        //        ViewState["ixd2"] = 2;
        //        ViewState["ixd3"] = 3;
        //        ViewState["ixd4"] = 4;

        //        //            ObjLookup.LU_Get_SIRNumber(BtnVendorLookup, ViewState["lookupid"].ToString(), "", "../../../../", "SIR");
        //        LbErr.Text = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGeneral.UserMsg(LbErr, Color.Red, ex.Message, txtReturnNo);

        //    }
        //}
        //private void GetLookUpsValue(ref TextBox txt1, ref TextBox txt2, string ixd1, string ixd2)
        //{
        //    if (Session["LookUpData"] != null)
        //    {
        //        GridViewRow Drow = (GridViewRow)Session["LookUpData"];
        //        txt1.Text = (txt1 != null ? Drow.Cells[Convert.ToInt32(ixd1)].Text.ToString() : "");
        //        txt2.Text = (txt2 != null ? Drow.Cells[Convert.ToInt32(ixd2)].Text.ToString() : "");

        //        Session["LookUpData"] = null;
        //    }
        //}



        //protected void txtQty_TextChanged(object sender, EventArgs e)
        //{
        //    //try
        //    //{

        //    //}
        //    //catch (Exception ex)
        //    //{

        //    //    LbErr.Text = ex.Message;
        //    //}

        //}
        //protected void txtRate_TextChanged(object sender, EventArgs e)
        //{
        //    //try
        //    //{

        //    //}
        //    //catch (Exception ex)
        //    //{

        //    //    LbErr.Text = ex.Message;
        //    //}
        //}
        //protected void txtGST_TextChanged(object sender, EventArgs e)
        //{
        //    //try
        //    //{


        //    //}
        //    //catch (Exception ex)
        //    //{

        //    //    LbErr.Text = ex.Message;
        //    //}
        //}
        //protected void txtAmount_TextChanged(object sender, EventArgs e)
        //{
        //    //try
        //    //{

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    LbErr.Text = ex.Message;
        //    //}
        //}
        //protected void txtWHT_TextChanged(object sender, EventArgs e)
        //{
        //    //try
        //    //{

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    LbErr.Text = ex.Message;
        //    //}
        //}
        private void ClearPartInfo()
        {
            txtPartItem.Text = string.Empty;
            txtPartItemDesc.Text = string.Empty;
            txtUnit.Text = string.Empty;
            txtUnitCode.Text = string.Empty;
            txtRemaining.Text = string.Empty;
            txtRetQty.Text = string.Empty;
            txtTotalAmt.Text = string.Empty;
            txtRate.Text = string.Empty;
            txtAvgCost.Text=string.Empty;
           // txtTotalQty.Text = string.Empty;
            //txtReqQty.Text = string.Empty;
            //txtBalQty.Text = string.Empty;
            txtTotalStock.Text = string.Empty;
            txtStockLoc.Text = string.Empty;
            //txtAvgCost.Text = string.Empty;
            txtIssueQty.Text = string.Empty;
            //txtAmount.Text = string.Empty;
            txtLocCode.Text = string.Empty;
            txtLocDesc.Text = string.Empty;
            txtGstPerc.Text = string.Empty;
            txtGstAmount.Text = string.Empty;
            txtFGSTAmount.Text = string.Empty;
            txtFGSTPerc.Text = string.Empty;
            txtExTaxAmount.Text = string.Empty;
            txtExTaxPerc.Text = string.Empty;


        }
        //protected void BtnClear1_Click(object sender, EventArgs e)
        //{
        //    ClearPartInfo();
        //}
        //protected void BtnAdd_Click(object sender, EventArgs e)
        //{
        //    int IntStatus = 0;
        //    DataSet ds = new DataSet();
        //    double DBalQty = 0.0, DIssueQty = 0.0, DRetQty = 0.0, DStockLoc = 0.0;
        //    try
        //    {
        //        if (ddlPR.SelectedIndex != 0)
        //        {
        //            if (SysFunc.IsPosted("StockReturn", "ReturnNO", ddlPR.SelectedValue.ToString(), Session["DealerCode"].ToString()) == true)
        //            {
        //                SysFunc.UserMsg(LbErr, Color.Red, "The Return is already posted; now you could not change/delete/Post the record...");
        //                return;
        //            }
        //        }
        //        if (txtLocCode.Text.Trim() == string.Empty)
        //        {
        //            SysFunc.UserMsg(LbErr, Color.Red, "Location not available");
        //            return;
        //        }
        //        DIssueQty = Convert.ToDouble(txtIssueQty.Text == "" ? "0" : txtIssueQty.Text);
        //        DRetQty = Convert.ToDouble(txtRetQty.Text == "" ? "0" : txtRetQty.Text);
        //        if (DRetQty <= 0)
        //        {
        //            SysFunc.UserMsg(LbErr, Color.Red, "Returne quantity should not be Zero (0)", txtIssueQty);
        //            return;
        //        }
        //        //if (txtAmount.Text == string.Empty)
        //        //{
        //        //    ObjGeneral.UserMsg(LbErr, Color.Red, "Part Item Amount Should not be left blank", txtIssueQty);
        //        //    return;
        //        //}
        //        if (DRetQty > DIssueQty)
        //        {
        //            SysFunc.UserMsg(LbErr, Color.Red, "Issue quantity can not be greater than remaining quantity", txtIssueQty);
        //            return;
        //        }
        //        else
        //        {
        //            LbErr.Text = "";
        //        }
        //        //if (IsPartOutOfStockAtLocation(txtPartItem.Text, txtLocCode.Text, Convert.ToDouble(txtIssueQty.Text)))
        //        //{
        //        //    ObjGeneral.UserMsg(LbErr, Color.Red, "Part Item Code. " + txtPartItem.Text + "is not on selected location");
        //        //    return;
        //        //}

        //        ds = (DataSet)ViewState["ObjPOds"];


        //        if (ds.Tables["PurchaseReturn"].Rows.Count > 0)
        //        {
        //            foreach (DataRow dtRow in ds.Tables["PurchaseReturn"].Rows)
        //            {
        //                if (txtItemCode.Text.Trim() == dtRow["ItemCode"].ToString())
        //                {
        //                    lblSNo.Text = dtRow["Sno"].ToString();
        //                }
        //            }

        //            foreach (DataRow dtRow in ds.Tables["PurchaseReturn"].Rows)
        //            {
        //                //Add Vehicle grid row when mor the on row exist

        //                if (lblSNo.Text.Trim() != string.Empty)
        //                {
        //                    if (lblSNo.Text == dtRow["Sno"].ToString())
        //                    {
        //                        int index = ds.Tables["PurchaseReturn"].Rows.IndexOf(dtRow);
        //                        ds.Tables["PurchaseReturn"].Rows.RemoveAt(index);
        //                        AddNewRow();

        //                        //LoadBOGrid();
        //                        //CalcAmount();
        //                        lblSNo.Text = "";
        //                        ClearDetail();

        //                        return;
        //                    }
        //                }
        //            }
        //            //Add vehicle grid row in epty grid
        //            if (ds.Tables["PurchaseReturn"].Rows[0]["Sno"].ToString() == "")
        //            {
        //                ds.Tables["PurchaseReturn"].Rows.RemoveAt(0);
        //                AddNewRow();
        //                ClearDetail();
        //            }
        //            else
        //            {
        //                AddNewRow();
        //                ClearDetail();
        //            }
        //        }
        //        else
        //        {
        //            AddNewRow();
        //            ClearDetail();
        //        }
        //        //LoadVehicleGrid();
        //        //LoadBOGrid();
        //        lblSNo.Text = "";
        //        //ViewState["ObjPOds"] = ds;


        //    }
        //    catch (Exception ex)
        //    {
        //        SysFunc.UserMsg(LbErr, Color.Red, ex.Message);

        //    }

        //}
        private void ClearDetail()
        {
            txtItemCode.Text = string.Empty;
            txtPartItem.Text = string.Empty;
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
            txtRate.Text = string.Empty;
            txtAvgCost.Text = string.Empty;
            ddlParts.SelectedIndex = 0;
            txtGstPerc.Text = string.Empty;
            txtGstAmount.Text = string.Empty;
            txtFGSTAmount.Text = string.Empty;
            txtFGSTPerc.Text = string.Empty;
            txtExTaxAmount.Text = string.Empty;
            txtExTaxPerc.Text = string.Empty;
        }
        private void AddNewRow()
        {
            DataSet ds = new DataSet();
            ds = (DataSet)ViewState["ObjPOds"];
            DataRow row = ds.Tables["PurchaseReturn"].NewRow();
            if (lblSNo.Text.Trim() != string.Empty)
            {
                row["Sno"] = lblSNo.Text;
            }
            else
            {
                row["Sno"] = SysFunc.GetNewMaxID("Sno", 3, ds);
            }

            double DAvgCost = 0.0, DIssueQty = 0.0 , total = 0.0;
            
            DAvgCost = Convert.ToDouble(txtRate.Text == "" ? "0" : txtRate.Text);
                DIssueQty = Convert.ToDouble(txtRetQty.Text == "" ? "0" : txtRetQty.Text);
                total = Convert.ToDouble(DAvgCost * DIssueQty);

                txtGstAmount.Text = Convert.ToString((total * Convert.ToDouble(ViewState["GstPerc"].ToString() == "" ? "0" : ViewState["GstPerc"].ToString())) / 100);
                txtFGSTAmount.Text = Convert.ToString((total * Convert.ToDouble(ViewState["FGSTPerc"].ToString() == "" ? "0" : ViewState["FGSTPerc"].ToString())) / 100);
                txtExTaxAmount.Text = Convert.ToString((total * Convert.ToDouble(ViewState["ExTaxperc"].ToString() == "" ? "0" : ViewState["ExTaxperc"].ToString())) / 100);


            

            row["Sno"] = ds.Tables["PurchaseReturn"].Rows.Count + 1;
            row["ItemCode"] = txtItemCode.Text.Trim();
            row["PartItemNo"] = txtPartItem.Text.Trim();
            row["PartItemName"] = txtPartItemDesc.Text.Trim();
            row["Unit"] = txtUnit.Text.Trim();
            row["UnitCode"] = txtUnitCode.Text.Trim();
            row["IssueQty"] = txtRetQty.Text.Trim();
            row["Amount"] = txtTotalAmt.Text.Trim();
            row["Rate"] = txtRate.Text.Trim();
            row["Location"] = txtLocDesc.Text;
            row["LocationCode"] = txtLocCode.Text;
            row["AvgRate"] = txtAvgCost.Text;
            row["GSTAmount"] = txtGstAmount.Text;
            row["FurTaxAmt"] = txtFGSTAmount.Text;
            row["ExTaxAmt"] = txtExTaxAmount.Text;
            ds.Tables["PurchaseReturn"].Rows.Add(row);
            //ViewState["ObjPOds"] = ds;
            GvMain.DataSource = ds.Tables["PurchaseReturn"];
            GvMain.DataBind();
            ViewState["ObjPOds"] = ds;
            //calculate total parts amount
            if (ds.Tables["PurchaseReturn"].Rows.Count > 0)
            {
                double iRate = 0;
                double iQty = 0;
                foreach (DataRow dr in ds.Tables["PurchaseReturn"].Rows)
                {
                    iRate = iRate + Convert.ToDouble(dr["Amount"].ToString());
                    iQty = iQty + Convert.ToDouble(dr["IssueQty"].ToString());
                }
                txtTotalAmount.Text = iRate.ToString();
                txtTotalQty.Text = iQty.ToString();
            }
            //////////////////////
        }
        //protected void BtnRemove_Click(object sender, EventArgs e)
        //{
        //    DataSet dsDelete = new DataSet();
        //    try
        //    {
        //        dsDelete = (DataSet)ViewState["ObjPOds"];

        //        LinkButton BtnLnk = (LinkButton)sender;
        //        GridViewRow rowGv = (GridViewRow)BtnLnk.Parent.Parent;
        //        int rowGvIndex = rowGv.RowIndex;

        //        dsDelete.Tables["PurchaseReturn"].Rows[rowGvIndex].Delete();
        //        GvMain.DataSource = dsDelete.Tables["PurchaseReturn"];
        //        GvMain.DataBind();
        //        dsDelete.Tables["PurchaseReturn"].AcceptChanges();
        //        LbErr.Text = "";
        //        ViewState["ObjPOds"] = dsDelete;
        //        //calculate total parts amount
        //        if (dsDelete.Tables["PurchaseReturn"].Rows.Count > 0)
        //        {

        //            double iRate = 0;
        //            double iQty = 0;
        //            foreach (DataRow dr in dsDelete.Tables["PurchaseReturn"].Rows)
        //            {
        //                iRate = iRate + Convert.ToDouble(dr["Amount"].ToString());
        //                iQty = iQty + Convert.ToDouble(dr["IssueQty"].ToString());
        //            }
        //            txtTotalAmount.Text = iRate.ToString();
        //            txtTotalQty.Text = iQty.ToString();
        //        }
        //        else
        //        {
        //            txtTotalAmt.Text = "0";
        //        }
        //        //////////////////////
        //    }
        //    catch (Exception ex)
        //    {
        //        SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
        //    }
        //}
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2545", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            SqlTransaction Trans = null;
            try
            {
                bool Check = false;

                //if (txtSIRNO .Text .Trim ()== string .Empty )
                //{
                //    ObjGeneral.UserMsg(LbErr, Color.Red, "SIRNO. should not be blank ", txtSIRNO);
                //    return;
                //}
                if (ddlPR .SelectedIndex != 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Edit not allowed", txtPartItem);
                    return;
                }
                else
                {
                    LbErr.Text = "";
                }
                if (GvMain.Rows.Count == 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "At least one part must be add...", txtPartItem);
                    return;
                }
                else
                {
                    LbErr.Text = "";
                }
                if (ObjTrans.BeginTransaction(ref Trans) == false)
                {
                    return;
                }
                string PRNO = ddlPR.Text.Trim();
                Save_InsertMasterDetail(Trans, ref Check,ref PRNO);
                if (Check == false)
                {
                    ObjTrans.RollBackTransaction(ref Trans);
                    SysFunc.UserMsg(LbErr, Color.Red, "Record can not save. Try again...");
                }
                else
                {
                    ObjTrans.CommittTransaction(ref Trans);
                    SysFunc.UserMsg(LbErr, Color.Green, "Record saved successfuly. Last code: " + ddlPR.SelectedValue.ToString());
                    CtlClear("T");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }
        protected void BtnPrint_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2545", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }

            if (ddlPR.SelectedIndex == 0)
            {
                SysFunc.UserMsg(LbErr, Color.Red, "Select Purchase Returne number first ");
                return;
            }
            else
            {
                LbErr.Text = "";
            }
            ReportDocument RD;

            Data.DSReports dsRpt1 = new Data.DSReports();
            
            RD = new ReportDocument();

            //string StrToDateFrom = "", StrDateTo = "";
            
            //dsrpt = new DXBMS.Data.DataSet1TableAdapters();
            // PartItemLedger.sp_ItemStockLedger_select.Load(ds.CreateDataReader());
            DataSet ds = new DataSet();
            
            
            //SqlParameter[] param = {
            //                    new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
            //                    new SqlParameter("@PRNo",SqlDbType.VarChar,8),//1
            //                };
            //param[0].Value = Session["DealerCode"].ToString();
            //param[1].Value = ddlPR.SelectedValue.ToString();

            //SysFunc.ExecuteSP_NonQuery("sp_PurchaseReturn_Report", param);
            ds = SqlHelper.ExecuteDataset(CConnection.GetConnectionString(), CommandType.Text, "sp_PurchaseReturn_Report '" + Session["DealerCode"].ToString() + "', '"+ddlPR.SelectedValue.ToString().Trim()+"'");
           dsRpt1.sp_PurchaseReturn_Report .Load(ds.CreateDataReader());
            
            


            RD.Load(Server.MapPath("../SpareReports/rptPurchaseReturnReport.rpt"));

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
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "PURCHASE RETURN" + "\"";
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


            //string FilePath = Server.MapPath("~") + "\\Download\\";
            //string FileName = "PurchaseReturn" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            //string File = FilePath + FileName;


            //Session["RD"] = RD;

            //string URL;
            //URL = "../../../Download/rptViewerService.aspx?FileName=" + FileName;

            ////URL = FilePath + "OpenPdf.aspx?FileName=" + FileName;
            ////txtPartItemDesc.Text = URL;
            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ////URL = "../../../Download/OpenPdf.aspx?FileName=" + FileName; 
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

            //// Page.ClientScript.RegisterStartupScript(this.GetType(), "myClick", "window.open('../../Criteria/POView.aspx?ReportID=ChargeOut&ChargeOutNO=" + StrChargOutNo + "','ShowReport','scrollbars=yes,menubar=no,height=800,width=900,resizable=no,toolbar=no,location=no,status=no');", true);
            //string URL = "../SpareReports/Criteria/POView.aspx?ReportID=ChargeOut&ChargeOutNO=" + StrChargOutNo;
            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
        protected void btnPosting_Click(object sender, EventArgs e)
        {
            try
            {
                //if(txtSIRNO .Text .Trim() ==string .Empty )
                //{
                //    ObjGeneral.UserMsg(LbErr ,Color .Red ,"SIRNo. should not be left blank",txtSIRNO );
                //    return;
                //}
                //if (SysFunc.IsPosted("PRMaster", "PRNo", ddlPR.SelectedValue.ToString(), Session["DealerCode"].ToString()) == true)
                //{
                //    SysFunc.UserMsg(LbErr, Color.Red, "The Purchase Return is already Saved; now you could not change/delete/Post the record...");
                //    return;
                //}
                //else
                //{
                //    LbErr.Text = "";
                //}
                if (ddlPR.SelectedIndex != 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "The Purchase Return is already Saved; now you could not change/delete/Post the record...");
                        return;
                }
                //if (Convert.ToDateTime(ObjGeneral.SaveDate(txtRetDate.Text)) < Convert.ToDateTime(ObjGeneral.SaveDate(txtIssueDate.Text)))
                //{
                //    ObjGeneral.UserMsg(LbErr, Color.Red, "Retrun date should not be less than issuance date...", txtIssueDate);
                //    return;
                //}
                //else
                //{
                //    LbErr.Text = "";
                //}
                bool chk = false;
                Posting(ref chk);
                if (chk)
                {
                    CtlClear("T");
                    LoadPR_DDL();
                }
                //ViewState["ObjPOds"] = null;

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
        protected void BtnPartItem_Click(object sender, ImageClickEventArgs e)
        {
            ////string StrSIRNO = "", MainPartItemNo = "'0'";
            //if (ddlGRNNo.SelectedIndex == 0)
            //{
            //    SysFunc.UserMsg(LbErr, Color.Red, "GRNNo can not be left blank.");
            //    return;
            //}
            //try
            //{
            //    string AppendInWhere = string.Empty;
            //    AppendInWhere = "   a.DealerCode = '" + Session["DealerCode"].ToString() + "' " +
            //   "   and a.ItemCode = b.ItemCode " +
            //   "   and a.GRNNO = '" + txtGRNNo.Text + "' " +
            //   "   And (A.AccQty - A.RetQty) > 0 " +
            //   "   And b.PartItemNo not in ('') ";
            //    ViewState["lookupid"] = 63; ViewState["ixd1"] = 1; ViewState["ixd2"] = 3; ViewState["ixd3"] = 4; ViewState["txtgroup"] = 1;
            //    //ObjLookup.LU_Get_Parts_PR(BtnPartItem, ViewState["lookupid"].ToString(), AppendInWhere, "../../../../");

            //}
            //catch (Exception ex) { throw ex; }
        }



        protected void BtnPartLocation_Click(object sender, ImageClickEventArgs e)
        {

            //try
            //{

            //    ViewState["lookupid"] = 33;
            //    ViewState["ixd1"] = 1;
            //    ViewState["ixd2"] = 2;
            //    ViewState["ixd3"] = 3;



            //    ObjLookup.LU_Get_PartItemLocation(BtnPartLocation, ViewState["lookupid"].ToString(), "", "../../../../");



            //    LbErr.Text = "";
            //}
            //catch (Exception ex)
            //{
            //    ObjGeneral.UserMsg(LbErr, Color.Red, ex.Message, txtReturnNo);
            //}
        }
        //protected void txtIssueQty_TextChanged(object sender, EventArgs e)
        //{
        //    double DAvgCost = 0.0, DIssueQty = 0.0;
        //    try
        //    {
        //        DAvgCost = Convert.ToDouble(txtAvgCost.Text == "" ? "0" : txtAvgCost.Text);
        //        DIssueQty = Convert.ToDouble(txtIssueQty.Text == "" ? "0" : txtIssueQty.Text);

        //        txtAmount.Text = Convert.ToString(DAvgCost * DIssueQty);
        //        txtIssueQty.Focus();
        //    }
        //    catch (Exception ex)
        //    {
        //        LbErr.Text = ex.Message;
        //    }
        //}
        protected void ImgLookupIssueNo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewState["lookupid"] = 69;
                ViewState["ixd1"] = 1;
                ViewState["ixd2"] = 2;


                //ObjLookup.LU_Get_PurchaseRet(ImgLookupIssueNo, ViewState["lookupid"].ToString(), "", "../../../../");

                LbErr.Text = "";


            }
            catch (Exception ex)
            {
                //ObjGeneral.UserMsg(LbErr, Color.Red, ex.Message, txtReturnNo);
            }
        }


        //protected void BtnSelect_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        LinkButton BtnLnk = (LinkButton)sender;
        //        GridViewRow rowGv = (GridViewRow)BtnLnk.Parent.Parent;
        //        int rowGvIndex = rowGv.RowIndex;

        //        Label LbItemCode = (Label)BtnLnk.FindControl("LbItemCode");
        //        txtItemCode.Text = LbItemCode.Text.Trim();

        //        Label LbPartDesc = (Label)BtnLnk.FindControl("LbPartDesc");
        //        txtPartItemDesc.Text = LbPartDesc.Text.Trim();

        //        HiddenField hdUnit = (HiddenField)BtnLnk.FindControl("hdUnitCode");
        //        txtUnitCode.Text = hdUnit.Value;

        //        DisplayItem();

        //        //HiddenField HDPartsTransLocCode = (HiddenField)BtnLnk.FindControl("HDPartsTransLocCode");
        //        //txtLocCode.Text = HDPartsTransLocCode.Value;

        //        //Label LbIssueQty = (Label)BtnLnk.FindControl("LbIssueQty");
        //        //txtRetQty.Text = LbIssueQty.Text.Trim();

        //        //Label LbAmount = (Label)BtnLnk.FindControl("LbAmount");
        //        ////txtAmount.Text = "";// LbAmount.Text.Trim();

        //        //Label lbRate = (Label)BtnLnk.FindControl("LbRate");
        //        //txtIssueRate.Text = lbRate.Text.Trim();

        //        //Label LbDCQuantity = (Label)BtnLnk.FindControl("LbDCQuantity");
        //        //txtDCQuantity.Text = LbDCQuantity.Text.Trim();
        //        LbErr.Text = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
        //    }

        //}
        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2545", "004"))
            {
                Response.Redirect("~/Test.aspx");
            }
            //ObjGeneral.UserMsg(LbErr, Color.Red, "Delete should not allowed", txtSIRNO);
            return;
            SqlTransaction Trans = null;
            bool Check = false;
            try
            {

                if (ObjTrans.BeginTransaction(ref Trans) == false)
                {
                    return;
                }

                //  POSTING
                if (ddlPR.SelectedIndex==0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "IssueNo. Should not be left blank");
                    return;
                }
                if (SysFunc.IsPosted("StockIssue", "IssueNO", ddlPR.SelectedValue.ToString(), Session["DealerCode"].ToString()) == true)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "The Issuance is already posted; now you could not change/delete/Post the record...");
                    return;
                }
                else
                {
                    LbErr.Text = "";
                }
                if (SysFunc.CodeExists("StockIssue", "IssueNO", ddlPR.SelectedValue.ToString(), " and DelFlag = 'N'") == true)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Issue No. does not exist...");
                    return;
                }
                else
                {
                    LbErr.Text = "";
                }

                if (SysFunc.CodeExists("StockReturn", "IssueNO", ddlPR .SelectedValue.ToString(), " And DelFlag = 'N'") == true)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Unable to Edit/Delete/Post the record; because it is used in Stock Return ...");
                    return;
                }
                else
                {
                    LbErr.Text = "";
                }

                SqlParameter[] param = {
                                       
                                       new SqlParameter("@IssueNo",SqlDbType.Char),
                                       new SqlParameter("@DEALER_CODE",SqlDbType.Char)
                                        };

                param[0].Value = ddlPR.SelectedValue .ToString();
                param[1].Value = Session["DealerCode"].ToString();

                if (SysFunc.SP_Spare_Delete_IssueMaster(Trans, param) == true)
                {
                    Check = true;
                }
                else
                {
                    Check = false;
                }
                if (Check == true)
                {
                    if (SysFunc.SP_Spare_Delete_IssueDetail(Trans, param) == true)
                    {
                        Check = true;
                    }
                    else
                    {
                        Check = false;
                    }
                }

                if (Check == false)
                {
                    ObjTrans.RollBackTransaction(ref Trans);
                    SysFunc.UserMsg(LbErr, Color.Red, "Record not Delete. try again...");
                }
                else
                {
                    ObjTrans.CommittTransaction(ref Trans);
                    SysFunc.UserMsg(LbErr, Color.Green, "Record Deleted successfully.");

                    CtlClear("T");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Deletealert()", true);
                }


            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }


        //protected void txtRetQty_OnTextChanged(object sender, EventArgs e)
        //{
        //    double DAvgCost = 0.0, DIssueQty = 0.0 , total = 0.0;
        //    try
        //    {
        //        DAvgCost = Convert.ToDouble(txtRate.Text == "" ? "0" : txtRate.Text);
        //        DIssueQty = Convert.ToDouble(txtRetQty.Text == "" ? "0" : txtRetQty.Text);
        //        total = Convert.ToDouble(DAvgCost * DIssueQty);

        //        txtGstAmount.Text = Convert.ToString((total * Convert.ToDouble(txtGstPerc.Text == "" ? "0" : txtGstPerc.Text)) / 100);
        //        txtFGSTAmount.Text = Convert.ToString((total * Convert.ToDouble(txtFGSTAmount.Text == "" ? "0" : txtFGSTAmount.Text)) / 100);
        //        txtExTaxAmount.Text = Convert.ToString((total * Convert.ToDouble(txtExTaxAmount.Text == "" ? "0" : txtExTaxAmount.Text)) / 100);



        //        txtTotalAmt.Text = total.ToString();
        //        txtIssueQty.Focus();
        //    }
        //    catch (Exception ex)
        //    {
                
        //    }
        //}

        protected void GvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    Label lblAmt=(Label)e.Row.FindControl("LbAmount");//LbAmount
            //    Label lblAvgCost = (Label)e.Row.FindControl("LbAvgCost");
            //    Label lblIssueQty = (Label)e.Row.FindControl("LbIssueQty");
            //    double DAvgCost = 0.0, DIssueQty = 0.0;
            //    DAvgCost = Convert.ToDouble(lblAvgCost.Text == "" ? "0" : lblAvgCost.Text);
            //    DIssueQty = Convert.ToDouble(lblIssueQty.Text == "" ? "0" : lblIssueQty.Text);
            //    lblAmt.Text = Convert.ToString(DAvgCost * DIssueQty);
            //}
        }
        protected void ddlLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            Get_locationWisePartData();
        }



        protected void ImgIssueNO_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlVendor.SelectedItem.Text.Trim() == "")
            {
                SysFunc.UserMsg(LbErr, Color.Red, "Select Vendor.");

                return;
            }
            try
            {
                ViewState["lookupid"] = 67;
                ViewState["ixd1"] = 1;
                ViewState["ixd2"] = 1;

                string strWhereClause;
                strWhereClause = " A.DealerCode = '" + Session["DealerCode"].ToString() + "' " +
                    " And A.DelFlag = 'N' " +
                    " And A.PostFlag = 'Y' " +
                    " And A.StockFlag = 'Y' " +
                    " And A.VendorCode = B.VendorCode " +
                    " And A.vendorcode = '" + ddlVendor.SelectedValue.ToString() + "' " +
                    " And A.GrnNo not In (Select distinct PurInvDetail.GrnNo from PurInvDetail) ";
                //ObjLookup.LU_Get_GRN(ImgIssueNO, ViewState["lookupid"].ToString(), strWhereClause, "../../../../");

                LbErr.Text = "";


            }
            catch (Exception ex)
            {
                //ObjGeneral.UserMsg(LbErr, Color.Red, ex.Message, txtReturnNo);
            }

        }

        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGRN_DDL();
        }
        private void LoadGRN_DDL()
    {
            string strWhereClause;
            strWhereClause = " DealerCode = '" + Session["DealerCode"].ToString() + "' " +
                " And DelFlag = 'N' " +
                " And PostFlag = 'Y' " +
                " And StockFlag = 'Y' " +
                " And vendorcode = '" + ddlVendor.SelectedValue.ToString() + "' " +
                " And GrnNo not In (Select GRNNO From GRNDetail Where PartsTransLocCode = '') ";
                //" And GrnNo not In (Select distinct PurInvDetail.GrnNo from PurInvDetail) ";
            string[] Columns = new string[] { "GRNNo", "Convert(Varchar(10),GRNDate,105)" };
            SysFunc.GetMultiColumnsDDL(ddlGRNNo, Columns, "GRNMaster", strWhereClause, "GRNNo","Order by GRNNo Desc",false,false);
    }
        private void LoadParts_DDL()
        {
            string AppendInWhere = string.Empty;
            AppendInWhere = "   a.DealerCode = '" + Session["DealerCode"].ToString() + "' " +
           "   and a.ItemCode = b.ItemCode " +
           "   and a.GRNNO = '" + ddlGRNNo.SelectedValue.ToString() + "' " +
           "   And (A.AccQty - A.RetQty) > 0 " +
           "   And b.PartItemNo not in ('') ";
            string[] Columns = new string[] { "a.ItemCode", "b.PartItemNo", "b.ItemDesc", "b.UnitCode" };
            SysFunc.GetMultiColumnsDDL(ddlParts, Columns, "GRNDetail a,Item b", AppendInWhere, "ItemCode","Order by b.ItemDesc",false,false);
        }

        protected void ddlGRNNo_SelectedIndexChanged(object sender, EventArgs e)
        {

            LoadParts_DDL();   
        }

        protected void ddlParts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlParts.SelectedIndex == 0)
            {
                ClearPartInfo();
                return;
            }
            txtItemCode.Text = ddlParts.SelectedValue.ToString().Trim();
            txtRetQty.Text = string.Empty;
            DisplayItem();
        }

        protected void ddlPR_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMode.Text = "Edit Mode";
            if (ddlPR.SelectedIndex == 0)
            {
                return;
            }
            else
            {   
                GetReturnData();
                FillGV();
            }
            
        }

        protected void BtnRemove1_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsDelete = new DataSet();
            try
            {
                
                dsDelete = (DataSet)ViewState["ObjPOds"];

                ImageButton BtnLnk = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;

                dsDelete.Tables["PurchaseReturn"].Rows[rowGvIndex].Delete();
                GvMain.DataSource = dsDelete.Tables["PurchaseReturn"];
                GvMain.DataBind();
                dsDelete.Tables["PurchaseReturn"].AcceptChanges();
                LbErr.Text = "";
                ViewState["ObjPOds"] = dsDelete;
                //calculate total parts amount
                if (dsDelete.Tables["PurchaseReturn"].Rows.Count > 0)
                {

                    double iRate = 0;
                    double iQty = 0;
                    foreach (DataRow dr in dsDelete.Tables["PurchaseReturn"].Rows)
                    {
                        iRate = iRate + Convert.ToDouble(dr["Amount"].ToString());
                        iQty = iQty + Convert.ToDouble(dr["IssueQty"].ToString());
                    }
                    txtTotalAmount.Text = iRate.ToString();
                    txtTotalQty.Text = iQty.ToString();
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

        protected void BtnAdd_Click1(object sender, ImageClickEventArgs e)
        {
            
            DataSet ds = new DataSet();
            double  DIssueQty = 0.0, DRetQty = 0.0;
            try
            {
                if (ddlPR.SelectedIndex != 0)
                {
                    if (SysFunc.IsPosted("StockReturn", "ReturnNO", ddlPR.SelectedValue.ToString(), Session["DealerCode"].ToString()) == true)
                    {
                        SysFunc.UserMsg(LbErr, Color.Red, "The Return is already posted; now you could not change/delete/Post the record...");
                        return;
                    }
                }
                if (txtLocCode.Text.Trim() == string.Empty)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Location not available");
                    return;
                }
                if(txtIssueQty.Text.Contains("-"))
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Negative GRN Quantity can not Alowed Please contact to IT Support  ");
                    return;
                }
                if (txtRemaining.Text.Contains("-"))
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Negative Remaining Quantity can not Alowed Please contact to IT Support  ");
                    return;
                }
                if (txtTotalStock.Text.Contains("-"))
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Negative Total Stock Quantity can not Alowed Please contact to IT Support  ");
                    return;
                }
                if (txtStockLoc.Text.Contains("-"))
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Negative Stock in Location Quantity can not Alowed Please contact to IT Support  ");
                    return;
                }
                
                DIssueQty = Convert.ToDouble(txtIssueQty.Text == "" ? "0" : txtIssueQty.Text);
                DRetQty = Convert.ToDouble(txtRetQty.Text == "" ? "0" : txtRetQty.Text);
                if (DRetQty <= 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Returne quantity should not be Zero (0)", txtIssueQty);
                    return;
                }
                //if (txtAmount.Text == string.Empty)
                //{
                //    ObjGeneral.UserMsg(LbErr, Color.Red, "Part Item Amount Should not be left blank", txtIssueQty);
                //    return;
                //}
                if (DRetQty > DIssueQty)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Issue quantity can not be greater than remaining quantity", txtIssueQty);
                    return;
                }
                else
                {
                    LbErr.Text = "";
                }
                //if (IsPartOutOfStockAtLocation(txtPartItem.Text, txtLocCode.Text, Convert.ToDouble(txtIssueQty.Text)))
                //{
                //    ObjGeneral.UserMsg(LbErr, Color.Red, "Part Item Code. " + txtPartItem.Text + "is not on selected location");
                //    return;
                //}
                string input = txtGstPerc.Text;
                string sub;
                if (input.Length > 0) {
                 sub = input.Substring(0, input.Length - 1);
                    ViewState["GstPerc"] = sub;
                }
                else{
                    sub = "0";
                    ViewState["GstPerc"] = sub;
                }
               

                string input1 = txtFGSTPerc.Text;
                string sub1;
                if (input1.Length > 0)
                {

                     sub1 = input1.Substring(0, input1.Length - 1);
                    ViewState["FGSTPerc"] = sub1;
                }
                else
                {
                    sub1 = "0";
                    ViewState["FGSTPerc"] = sub1;
                }

                string input2 = txtExTaxPerc.Text;
                string sub2;
                if (input2.Length > 0)
                {

                   sub2 = input2.Substring(0, input2.Length - 1);
                    ViewState["ExTaxperc"] = sub2;
                }
                else
                {
                    sub2 = "0";
                    ViewState["ExTaxperc"] = sub2;
                }



                ds = (DataSet)ViewState["ObjPOds"];


                if (ds.Tables["PurchaseReturn"].Rows.Count > 0)
                {
                    foreach (DataRow dtRow in ds.Tables["PurchaseReturn"].Rows)
                    {
                        if (txtItemCode.Text.Trim() == dtRow["ItemCode"].ToString())
                        {
                            lblSNo.Text = dtRow["Sno"].ToString();
                        }
                    }

                    foreach (DataRow dtRow in ds.Tables["PurchaseReturn"].Rows)
                    {
                        //Add Vehicle grid row when mor the on row exist

                        if (lblSNo.Text.Trim() != string.Empty)
                        {
                            if (lblSNo.Text == dtRow["Sno"].ToString())
                            {
                                int index = ds.Tables["PurchaseReturn"].Rows.IndexOf(dtRow);
                                ds.Tables["PurchaseReturn"].Rows.RemoveAt(index);
                                AddNewRow();

                                //LoadBOGrid();
                                //CalcAmount();
                                lblSNo.Text = "";
                                ClearDetail();

                                return;
                            }
                        }
                    }
                    //Add vehicle grid row in epty grid
                    if (ds.Tables["PurchaseReturn"].Rows[0]["Sno"].ToString() == "")
                    {
                        ds.Tables["PurchaseReturn"].Rows.RemoveAt(0);
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
                //LoadVehicleGrid();
                //LoadBOGrid();
                lblSNo.Text = "";
                //ViewState["ObjPOds"] = ds;


            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);

            }
        }

        protected void BtnClear1_Click1(object sender, ImageClickEventArgs e)
        {
            ClearPartInfo();
        }

        protected void btnGL_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2545", "005"))
            {
                Response.Redirect("~/Test.aspx");
            }
            string URL;

            URL = "../../Service/Forms/JV.aspx?CusInv=" + ddlPR.SelectedValue + "&Type=PR";


            string fullURL = "window.open('" + URL + "', '_blank', 'height=600,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }

       

        protected void ImageButtonPR_Click(object sender, ImageClickEventArgs e)
        {

            ViewState["lookupid"] = 69; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_PurchaseRet(ImageButtonPR, ViewState["lookupid"].ToString(), "", "../../../");



            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }
    }
}