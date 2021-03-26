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

namespace DXBMS.Modules.SpareParts.Forms
{
    public partial class GRNForm : System.Web.UI.Page
    {
        
        Transaction ObjTrans = new Transaction();
        SysFunctions SysFunc = new SysFunctions();
        SecurityBll sec = new SecurityBll();
        MainBLL ObjMain = new MainBLL();
        clsLookUp clslook = new clsLookUp();
        static int btnValue = 0;
        decimal sumFooterGRNQty = 0;
        decimal SumFooterGSTAmt = 0;
        decimal SumFooterExAmt = 0;
        decimal SumFooterFurAmt = 0;
        decimal SumFooterDiscount = 0;
        decimal SumFooterTotalAmt = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2543", "001"))
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
            if (!IsPostBack)
            {
                if (txtGRNDate.Text.Length == 0) txtGRNDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                if (Session["TransCode"] != null)
                {
                    LoadGRN_DDL();
                    LoadDC_DDL();
                    string leadId = Session["TransCode"].ToString();
                    ddlGRNNo.SelectedValue = leadId;

                    ddlGRNNo_SelectedIndexChanged(null, null);


                }
                else
                {
                    lblMode.Text = "Add Mode";
                    LoadGRN_DDL();
                    LoadDC_DDL();
                }
              

            }

            //if (Session["LookUpData"] != null)
            //{
            //    int intlookupid = Convert.ToInt32(ViewState["lookupid"]);
            //    //lookUptext(intlookupid);
            //    //lookUpLocation(intlookupid);

            //    if (intlookupid == 20)
            //    {
            //        Get_OPInfo();
            //        FillDCGV();
            //        totalCalculation();
            //    }

            //    if (intlookupid == 38)
            //    {
            //        Get_GRNInfo();
            //        FillGV();
            //        totalCalculation();
            //        HDDelete.Value = "1";
            //    }
            //}
            btnValue = 0;
            Session["TransCode"] = null;
            Session["LookUpData"] = string.Empty;
        }

        private void LoadDC_DDL()
        {
            string WhereQuery = "a.VendorCode = b.VendorCode " +
 "And b.DealerCode in (b.DealerCode,'COMON')  " +
 "and a.DealerCode = '"+ Session["DealerCode"].ToString() +"'" +
 "and a.DelFlag = 'N' " +
 "and a.DCNo Not IN  " +
 "(Select DCNo from GRNMaster Where DealerCode = '" + Session["DealerCode"].ToString() + "'and Delflag = 'N') "; 
            //"Order by a.PRNo Desc";
            string[] Columns = new string[] { "a.DCNo", "VendorDCNO", "convert(varchar(12),a.DCDate,105)", "b.VendorDesc", "a.PONo" };
            SysFunc.GetMultiColumnsDDL(ddlDCNo, Columns, "DCMaster a,Vendor b", WhereQuery, "DCNo", "Order by a.DCNo Desc", false, false);
        }
        private void LoadGRN_DDL()
        {
            string WhereQuery = "a.DealerCode =  '" + Session["DealerCode"].ToString() + "' " +
  "and a.DelFlag = 'N' " +
 "and a.VendorCode = b.VendorCode and b.DealerCode in (a.DealerCode,'COMON')";
            //"Order by a.PRNo Desc";
            string[] Columns = new string[] { "a.GRNNo", "convert(varchar(20),a.GRnDate,105)", "a.PoNo", "a.DCNo", "b.VendorDesc" };
            SysFunc.GetMultiColumnsDDL(ddlGRNNo, Columns, "GRNMaster a,Vendor b", WhereQuery, "GRNNo", "Order by a.GRNNo Desc", false, false);
        }
        #region // ********Funtions********* //


        //private void lookUptext(int lookUpID)
        //{
        //    //Selection values display

        //    switch (lookUpID)
        //    {
        //        case 20:
        //            GetLookUpsValue(ref txtDCNo, ref txtDCDate,
        //                ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString());
        //            break;
        //        case 38:
        //            GetLookUpsValue(ref txtGRNNo, ref txtGRNDate,
        //                ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString());
        //            break;

        //        default:
        //            break;
        //    }
        //}
        //private void lookUpLocation(int lookUpID)
        //{
        //    int GVIndex = 0;

        //    switch (lookUpID)
        //    {
        //        case 33:
        //            GVIndex = Convert.ToInt32(HDGVIndex.Value);

        //            TextBox txtLocationCode = (TextBox)GvMain.Rows[GVIndex].FindControl("txtLocationCode");
        //            TextBox txtLocationName = (TextBox)GvMain.Rows[GVIndex].FindControl("txtLocationName");

        //            HDGVIndex.Value = "";
        //            GetLookUpsValue(ref txtLocationCode, ref txtLocationName, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString());
        //            break;
        //        default:
        //            break;
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
        private void Get_OPInfo()
        {
            DataSet dsOP = new DataSet();
            try
            {
                SqlParameter[] param = {
                                       new SqlParameter("@DCNO",SqlDbType.Char),
                                       new SqlParameter("@DealerCode",SqlDbType.Char)
                                  };
                param[0].Value = ddlDCNo.SelectedValue.ToString();
                param[1].Value = Session["DealerCode"].ToString();

                dsOP = SysFunc.SP_Spare_Get_DCOPInfo(param);

                if (dsOP.Tables[0].Rows.Count != 0)
                {
                    txtPONO.Text = dsOP.Tables[0].Rows[0]["PONO"].ToString();
                    txtPODate.Text = dsOP.Tables[0].Rows[0]["PODate"].ToString();
                    txtDCDate.Text = dsOP.Tables[0].Rows[0]["DCDate"].ToString();
                    txtPOType.Text = dsOP.Tables[0].Rows[0]["DeliveryType"].ToString();
                    txtVendorCode.Text = dsOP.Tables[0].Rows[0]["VendorCode"].ToString();
                    txtVendorDesc.Text = dsOP.Tables[0].Rows[0]["VendorDesc"].ToString();
                    txtOtherChargers.Text = dsOP.Tables[0].Rows[0]["OtherCharges"].ToString();
                    txtRemarks.Focus();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void FillGV()
        {

            DataSet dsMain = new DataSet();
            try
            {

                SqlParameter[] param = {
                                       new SqlParameter("@GRNNO",SqlDbType.Char),
                                       new SqlParameter("@DealerCode",SqlDbType.Char)
                                  };

                param[0].Value = ddlGRNNo.SelectedValue.ToString();
                param[1].Value = Session["DealerCode"].ToString();

                dsMain = SysFunc.SP_Part_DCGRNData(param);
                GvMain.DataSource = dsMain.Tables[0];
                GvMain.DataBind();
                Session["dsDCGrid"] = dsMain;

            }
            catch (Exception ex)
            {
                LbErr.Text = ex.Message;
            }
        }
        private void FillDCGV()
        {

            DataSet dsMain = new DataSet();
            try
            {

                SqlParameter[] param = {
                                       new SqlParameter("@DealerCode",SqlDbType.Char),
                                       new SqlParameter("@DCNO",SqlDbType.Char),
                                       
                                  };


                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlDCNo.SelectedValue.ToString();

                dsMain = SysFunc.FillDataSet("Sp_2W_Get_DCGridData", param);
                GvMain.DataSource = dsMain.Tables[0];
                GvMain.DataBind();
                Session["dsDCGrid"] = dsMain;
                

            }
            catch (Exception ex)
            {
                LbErr.Text = ex.Message;
            }
        }
        private void totalCalculation()
        {
            decimal DGrossAmount = 0, DOtherCharges = 0, FrightCharges = 0;
            try
            {
                //for (int i = 0; i < GvMain.Rows.Count; i++)
                //{
                //    TextBox txtRate = (TextBox)GvMain.Rows[i].FindControl("txtRate");

                //    TextBox txtAcceptedQty = (TextBox)GvMain.Rows[i].FindControl("txtAcceptedQty");
                //    DAcceptedQty = Convert.ToDouble(txtAcceptedQty.Text);

                //    DRate = Convert.ToDouble(txtRate.Text);
                //    //DmainRate += DRate * DmainRate;
                //    DmainRate += DRate * DAcceptedQty;



                //    Label LbGSTage = (Label)GvMain.Rows[i].FindControl("LbGSTAmount");
                //    DGST = Convert.ToDouble(LbGSTage.Text);
                //    DMainGST = DGST + DMainGST;


                //    Label LbSEDAmount = (Label)GvMain.Rows[i].FindControl("LbSEDAmount");
                //    DWHT = Convert.ToDouble(LbSEDAmount.Text);
                //    DMainWHT = DWHT + DMainWHT;

                //    //  TextBox txtAcceptedQty = (TextBox)GvMain.Rows[i].FindControl("txtAcceptedQty");
                //    //  DAcceptedQty = Convert.ToDouble(txtAcceptedQty.Text);
                //    DMainAcceptedQty = DAcceptedQty + DMainAcceptedQty;

                //    Label LbAmountMain = (Label)GvMain.Rows[i].FindControl("LbAmountMain");


                //    Label LbGSTAmount = (Label)GvMain.Rows[i].FindControl("LbGSTAmount");

                //    DGSTAmount = Convert.ToDouble(LbGSTAmount.Text);



                //}
                //txtGrossAmount.Text = Convert.ToString(DmainRate);
                //txtGSTAmount.Text = Convert.ToString(DMainGST);
                //txtWHTAmount.Text = Convert.ToString(DMainWHT);
                Label lblFooterTotalAmt = (Label)GvMain.FooterRow.FindControl("lblFooterTotalAmt");
                DGrossAmount = Convert.ToDecimal(lblFooterTotalAmt.Text.Trim() == "" ? "0" : lblFooterTotalAmt.Text.Trim());
                DOtherCharges = Convert.ToDecimal(txtOtherChargers.Text.Trim() == "" ? "0" : txtOtherChargers.Text.Trim());
                FrightCharges = Convert.ToDecimal(txtFreightCharges.Text.Trim() == "" ? "0" : txtFreightCharges.Text.Trim());
                txtNetAmount.Text = Convert.ToString(DGrossAmount + DOtherCharges + FrightCharges);
                GridNetAmount.Text = Convert.ToString(DGrossAmount);

                //txtTotalAcceptedQty.Text = Convert.ToString(DMainAcceptedQty);

            }
            catch (Exception ex)
            {
                LbErr.Text = ex.Message;
            }
        }

        private bool Insert_MasterDetail()
        {
            //SqlTransaction Trans = null;
            //bool check = false;
            //try
            //{
            //    if (ObjTrans.BeginTransaction(ref Trans) == false)
            //    {
            //        return false;
            //    }

            //    SqlParameter[] param = {
            //                        new SqlParameter("@DealerCode",SqlDbType.Char),//10
            //                        new SqlParameter("@GRNNo",SqlDbType.Char),//1
            //                        new SqlParameter("@GRNDate",SqlDbType.DateTime),//2
            //                        new SqlParameter("@DCNo",SqlDbType.Char),//3
            //                        new SqlParameter("@PONo",SqlDbType.Char),//4
            //                        new SqlParameter("@VendorCode",SqlDbType.Char),//5
            //                        new SqlParameter("@Remarks",SqlDbType.Char),//6
            //                        new SqlParameter("@RejLocCode",SqlDbType.Char),//7
            //                        new SqlParameter("@OtherCharges",SqlDbType.Float),//8
            //                        new SqlParameter("@TotalGSTAmount",SqlDbType.Float),//9
            //                        new SqlParameter("@TotalSEDAmount",SqlDbType.Float),//10
            //                        new SqlParameter("@NetTotal",SqlDbType.Float),//11
            //                        new SqlParameter("@PostFlag",SqlDbType.Char),//12
            //                        new SqlParameter("@UpdUser",SqlDbType.Char),//13
            //                        //new SqlParameter("@UpdDate",SqlDbType.DateTime),//14
            //                        //new SqlParameter("@UpdTime",SqlDbType.DateTime),//15
            //                        new SqlParameter("@UpdTerm",SqlDbType.VarChar),//16
            //                        new SqlParameter("@Transferstatus",SqlDbType.Char)//17
            //                       };

            //    param[0].Value = Session["DealerCode"].ToString();
            //    txtGRNNo.Text = SysFunc.AutoGen("GRNMaster", "GRNNO", txtGRNDate.Text.Trim());
            //    param[1].Value = txtGRNNo.Text;
            //    param[2].Value = SysFunc.SaveDate(txtGRNDate.Text);
            //    param[3].Value = ddlDCNo.SelectedValue.ToString().Trim();
            //    param[4].Value = txtPONO.Text.Trim();
            //    param[5].Value = txtVendorCode.Text.Trim();
            //    param[6].Value = txtRemarks.Text.Replace("'", " ").Trim();
            //    param[7].Value = "";
            //    param[8].Value = Convert.ToDecimal(txtOtherChargers.Text.Trim() == "" ? "0" : txtOtherChargers.Text.Trim());
            //    param[9].Value = Convert.ToDecimal(txtGSTAmount.Text.Trim() == "" ? "0" : txtGSTAmount.Text.Trim());
            //    param[10].Value = Convert.ToDecimal(txtWHTAmount.Text.Trim() == "" ? "0" : txtWHTAmount.Text.Trim());
            //    param[11].Value = Convert.ToDecimal(txtNetAmount.Text.Trim() == "" ? "0" : txtNetAmount.Text.Trim());
            //    param[12].Value = "N";
            //    param[13].Value = Session["UserName"].ToString();
            //    //param[14].Value = ObjGeneral.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
            //    //param[15].Value = ObjGeneral.SaveTime(DateTime.Now.ToString("hh:mm"));
            //    param[14].Value = "";
            //    param[15].Value = "C";

            //    if (SysFunc.SP_Spare_Insert_GRNMaster(Trans, param))
            //    {
            //        for (int i = 0; i < GvMain.Rows.Count; i++)
            //        {

            //            SqlParameter[] param1 = {
            //                                 new SqlParameter("@DealerCode",SqlDbType.Char),//0
            //                                 new SqlParameter("@GRNNo",SqlDbType.Char),//1
            //                                 new SqlParameter("@ItemCode",SqlDbType.Char),//2
            //                                 new SqlParameter("@PartsTransLocCode",SqlDbType.Char),//3
            //                                 new SqlParameter("@DCQty",SqlDbType.Float),//4
            //                                 new SqlParameter("@AccQty",SqlDbType.Float),//5
            //                                 new SqlParameter("@RejQty",SqlDbType.Float),//6
            //                                 new SqlParameter("@Rate",SqlDbType.Float),//7
            //                                 new SqlParameter("@Amount",SqlDbType.Float),//8
            //                                 new SqlParameter("@GSTPercent",SqlDbType.Float),//9
            //                                 new SqlParameter("@GSTAmount",SqlDbType.Float),//10
            //                                 new SqlParameter("@SEDage",SqlDbType.Float),//11
            //                                 new SqlParameter("@SEDAmount",SqlDbType.Float),//12
            //                                 new SqlParameter("@NetAmount",SqlDbType.Float)//13
            //                              };

            //            Label LbPartNo = (Label)GvMain.Rows[i].FindControl("LbPartNo");
            //            TextBox txtLocationCode = (TextBox)GvMain.Rows[i].FindControl("txtLocationCode");
            //            Label LbDCQty1 = (Label)GvMain.Rows[i].FindControl("LbDCQty1");
            //            TextBox txtAcceptedQty = (TextBox)GvMain.Rows[i].FindControl("txtAcceptedQty");
            //            TextBox txtRate = (TextBox)GvMain.Rows[i].FindControl("txtRate");
            //            Label LBGSTage = (Label)GvMain.Rows[i].FindControl("LBGSTage");

            //            Label LbGSTAmount = (Label)GvMain.Rows[i].FindControl("LbGSTAmount");
            //            Label LbWHTage = (Label)GvMain.Rows[i].FindControl("LbWHTage");
            //            Label LbSEDAmount = (Label)GvMain.Rows[i].FindControl("LbSEDAmount");
            //            Label LbAmount = (Label)GvMain.Rows[i].FindControl("LbAmountMain");




            //            param1[0].Value = Session["DealerCode"].ToString();
            //            param1[1].Value = txtGRNNo.Text.Trim();
            //            param1[2].Value = LbPartNo.Text.Trim();
            //            param1[3].Value = txtLocationCode.Text.Trim();
            //            param1[4].Value = Convert.ToDecimal(LbDCQty1.Text.Trim() == "" ? "0" : LbDCQty1.Text.Trim());
            //            param1[5].Value = Convert.ToDecimal(txtAcceptedQty.Text.Trim() == "" ? "0" : txtAcceptedQty.Text.Trim());
            //            param1[6].Value = 0.0;
            //            param1[7].Value = Convert.ToDecimal(txtRate.Text.Trim() == "" ? "0" : txtRate.Text.Trim());
            //            param1[8].Value = Convert.ToDecimal(LbAmount.Text.Trim() == "" ? "0" : LbAmount.Text.Trim());
            //            param1[9].Value = Convert.ToDecimal(LBGSTage.Text.Trim() == "" ? "0" : LBGSTage.Text.Trim());
            //            param1[10].Value = Convert.ToDecimal(LbGSTAmount.Text.Trim() == "" ? "0" : LbGSTAmount.Text.Trim());
            //            param1[11].Value = Convert.ToDecimal(LbWHTage.Text.Trim() == "" ? "0" : LbWHTage.Text.Trim());
            //            param1[12].Value = Convert.ToDecimal(LbSEDAmount.Text.Trim() == "" ? "0" : LbSEDAmount.Text.Trim());
            //            param1[13].Value = Convert.ToDecimal(txtAcceptedQty.Text.Trim() == "" ? "0" : txtAcceptedQty.Text.Trim()) * Convert.ToDecimal(txtRate.Text.Trim() == "" ? "0" : txtRate.Text.Trim());



            //            if (SysFunc.SP_Spare_Insert_GRNDetail(Trans, param1)) // MasterDetail 
            //            {
            //                check = true;
            //            }
            //            else
            //            {
            //                check = false;
            //                break;
            //            }
            //        }

            //        if (check == false)
            //        {
            //            ObjTrans.RollBackTransaction(ref Trans);
            //            SysFunc.UserMsg(LbErr, Color.Red, "Record not save please try again", txtGRNNo);
            //        }
            //        else
            //        {
            //            ObjTrans.CommittTransaction(ref Trans);
            //            SysFunc.UserMsg(LbErr, Color.Green, "Recored Saved Successfully. Last Code: " + txtGRNNo.Text, txtGRNNo);
            //        }
            //    }
            //    return check;
            //}
            //catch (Exception ex)
            //{
            //    SysFunc.UserMsg(LbErr, Color.Red, ex.Message, txtGRNNo);
            //    ObjTrans.RollBackTransaction(ref Trans);
            //    throw ex;
            //}
            return false;
        }
        private bool Insert_MasterDetailMain()
        {
            //SqlTransaction Trans = null;
            //bool check = false;
            //try
            //{
            //    if (ObjTrans.BeginTransaction(ref Trans) == false)
            //    {
            //        return false;
            //    }

            //    SqlParameter[] param = {
            //                        new SqlParameter("@DealerCode",SqlDbType.Char),//10
            //                        new SqlParameter("@GRNNo",SqlDbType.Char),//1
            //                        new SqlParameter("@GRNDate",SqlDbType.DateTime),//2
            //                        new SqlParameter("@DCNo",SqlDbType.Char),//3
            //                        new SqlParameter("@PONo",SqlDbType.Char),//4
            //                        new SqlParameter("@VendorCode",SqlDbType.Char),//5
            //                        new SqlParameter("@Remarks",SqlDbType.Char),//6
            //                        new SqlParameter("@RejLocCode",SqlDbType.Char),//7
            //                        new SqlParameter("@OtherCharges",SqlDbType.Float),//8
            //                        new SqlParameter("@TotalGSTAmount",SqlDbType.Float),//9
            //                        new SqlParameter("@TotalSEDAmount",SqlDbType.Float),//10
            //                        new SqlParameter("@NetTotal",SqlDbType.Float),//11
            //                        new SqlParameter("@PostFlag",SqlDbType.Char),//12
                                    
            //                        new SqlParameter("@UpdUser",SqlDbType.Char),//13
            //                        new SqlParameter("@UpdDate",SqlDbType.DateTime),//14
            //                        new SqlParameter("@UpdTime",SqlDbType.DateTime),//15
            //                        new SqlParameter("@UpdTerm",SqlDbType.VarChar),//16
            //                        new SqlParameter("@Transferstatus",SqlDbType.Char)//17
            //                       };

            //    param[0].Value = Session["DealerCode"].ToString();
            //    txtGRNNo.Text = SysFunc.AutoGen("GRNMaster", "GRNNO", txtGRNDate.Text.Trim());
            //    param[1].Value = txtGRNNo.Text;
            //    param[2].Value = SysFunc.SaveDate(txtGRNDate.Text);
            //    param[3].Value = ddlDCNo.SelectedValue.ToString().Trim();
            //    param[4].Value = txtPONO.Text.Trim();
            //    param[5].Value = txtVendorCode.Text.Trim();
            //    param[6].Value = txtRemarks.Text.Replace("'", " ").Trim();
            //    param[7].Value = "";
            //    param[8].Value = Convert.ToDecimal(txtOtherChargers.Text.Trim() == "" ? "0" : txtOtherChargers.Text.Trim());
            //    param[9].Value = Convert.ToDecimal(txtGSTAmount.Text.Trim() == "" ? "0" : txtGSTAmount.Text.Trim());
            //    param[10].Value = Convert.ToDecimal(txtWHTAmount.Text.Trim() == "" ? "0" : txtWHTAmount.Text.Trim());
            //    param[11].Value = Convert.ToDecimal(txtNetAmount.Text.Trim() == "" ? "0" : txtNetAmount.Text.Trim());
            //    param[12].Value = "N";
            //    param[13].Value = Session["UserName"].ToString();
            //    param[14].Value = SysFunc.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
            //    param[15].Value = SysFunc.SaveTime(DateTime.Now.ToString("hh:mm"));
            //    param[16].Value = "";
            //    param[17].Value = "C";

            //    if (SysFunc.SP_Spare_Insert_GRNMaster(Trans, param))
            //    {
            //        for (int i = 0; i < GvMain.Rows.Count; i++)
            //        {

            //            SqlParameter[] param1 = {
            //                                 new SqlParameter("@DealerCode",SqlDbType.Char),//0
            //                                 new SqlParameter("@GRNNo",SqlDbType.Char),//1
            //                                 new SqlParameter("@ItemCode",SqlDbType.Char),//2
            //                                 new SqlParameter("@PartsTransLocCode",SqlDbType.Char),//3
            //                                 new SqlParameter("@DCQty",SqlDbType.Float),//4
            //                                 new SqlParameter("@AccQty",SqlDbType.Float),//5
            //                                 new SqlParameter("@RejQty",SqlDbType.Float),//6
            //                                 new SqlParameter("@Rate",SqlDbType.Float),//7
            //                                 new SqlParameter("@Amount",SqlDbType.Float),//8
            //                                 new SqlParameter("@GSTPercent",SqlDbType.Float),//9
            //                                 new SqlParameter("@GSTAmount",SqlDbType.Float),//10
            //                                 new SqlParameter("@SEDage",SqlDbType.Float),//11
            //                                 new SqlParameter("@SEDAmount",SqlDbType.Float),//12
            //                                 new SqlParameter("@NetAmount",SqlDbType.Float)//13
            //                              };

            //            Label LbPartNo = (Label)GvMain.Rows[i].FindControl("LbPartNo");
            //            TextBox txtLocationCode = (TextBox)GvMain.Rows[i].FindControl("txtLocationCode");
            //            Label LbDCQty1 = (Label)GvMain.Rows[i].FindControl("LbDCQty1");
            //            TextBox txtAcceptedQty = (TextBox)GvMain.Rows[i].FindControl("txtAcceptedQty");
            //            TextBox txtRate = (TextBox)GvMain.Rows[i].FindControl("txtRate");
            //            Label LBGSTage = (Label)GvMain.Rows[i].FindControl("LBGSTage");

            //            Label LbGSTAmount = (Label)GvMain.Rows[i].FindControl("LbGSTAmount");
            //            Label LbWHTage = (Label)GvMain.Rows[i].FindControl("LbWHTage");
            //            Label LbSEDAmount = (Label)GvMain.Rows[i].FindControl("LbSEDAmount");




            //            param1[0].Value = Session["DealerCode"].ToString();
            //            param1[1].Value = txtGRNNo.Text.Trim();
            //            param1[2].Value = LbPartNo.Text.Trim();
            //            param1[3].Value = txtLocationCode.Text.Trim();
            //            param1[4].Value = Convert.ToDecimal(LbDCQty1.Text.Trim() == "" ? "0" : LbDCQty1.Text.Trim());
            //            param1[5].Value = Convert.ToDecimal(txtAcceptedQty.Text.Trim() == "" ? "0" : txtAcceptedQty.Text.Trim());
            //            param1[6].Value = 0.0;
            //            param1[7].Value = Convert.ToDecimal(txtRate.Text.Trim() == "" ? "0" : txtRate.Text.Trim());
            //            param1[8].Value = Convert.ToDecimal(txtRate.Text.Trim() == "" ? "0" : txtRate.Text.Trim());
            //            param1[9].Value = Convert.ToDecimal(LBGSTage.Text.Trim() == "" ? "0" : LBGSTage.Text.Trim());
            //            param1[10].Value = Convert.ToDecimal(LbGSTAmount.Text.Trim() == "" ? "0" : LbGSTAmount.Text.Trim());
            //            param1[11].Value = Convert.ToDecimal(LbWHTage.Text.Trim() == "" ? "0" : LbWHTage.Text.Trim());
            //            param1[12].Value = Convert.ToDecimal(LbSEDAmount.Text.Trim() == "" ? "0" : LbSEDAmount.Text.Trim());
            //            param1[13].Value = 100;//Convert.ToDecimal(txtAcceptedQty.Text.Trim() == "" ? "0" : txtAcceptedQty.Text.Trim());



            //            if (SysFunc.SP_Spare_Insert_GRNDetail(Trans, param1) == true)
            //            {
            //                check = true;
            //            }
            //            else
            //            {
            //                check = false;
            //                break;
            //            }
            //        }
            //    }
            //    return check;
            //}
            //catch (Exception ex)
            //{

            //    ObjTrans.RollBackTransaction(ref Trans);
            //    throw ex;
            //}
            return false;
        }
        private void InsertItemStockMain(string chkSave)
        {
            SqlTransaction Trans = null;
            bool check = false;
            string GRNCode=string.Empty;
            try
            {
                for (int k = 0; k < GvMain.Rows.Count; k++)
                {
                    Label lblLocationCode = (Label)GvMain.Rows[k].FindControl("lblLocationCode");
                    if (lblLocationCode.Text.Trim() == string.Empty)
                    {
                        SysFunc.UserMsg(LbErr,Color.Red,"Part location Can not be empty");
                        return;
                    }
                }
                
                if (ObjTrans.BeginTransaction(ref Trans) == false) return;
                if (chkSave == "Y")
                {
                    //foreach (GridViewRow dr in GvMain.Rows)
                    //{
                    //    TextBox GVtxtLocCode = (TextBox)dr.FindControl("txtLocationCode");
                    //    if (GVtxtLocCode.Text.Trim() == "")
                    //    {
                    //        SysFunc.UserMsg(LbErr, Color.Red, "Location Code dose not be left blank...", GVtxtLocCode);
                    //        return;
                    //    }
                    //}
                    //Set Sum of GRN Qty in Footer

                    Label lblTotalAccQty = (Label)GvMain.FooterRow.FindControl("lblTotalAccQty");
                    

                    //Set Sum of GST Amt in Footer
                    Label lblTotalGSTAmt = (Label)GvMain.FooterRow.FindControl("lblTotalGSTAmt");
                    

                    //Set Sum of Ex Amt in Footer
                    Label lblTotalExAmt = (Label)GvMain.FooterRow.FindControl("lblTotalExAmt");
                    

                    //Set Sum of Further Amt in Footer
                    Label lblTotalFurAmt = (Label)GvMain.FooterRow.FindControl("lblTotalFurAmt");
                    

                    //Set Sum of Discount in Footer
                    Label lblTotalDiscount = (Label)GvMain.FooterRow.FindControl("lblTotalDiscount");
                    

                    Label lblFooterTotalAmt = (Label)GvMain.FooterRow.FindControl("lblFooterTotalAmt");

                    SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                    new SqlParameter("@GRNNo",SqlDbType.Char),//1
                                    new SqlParameter("@GRNDate",SqlDbType.DateTime),//2
                                    new SqlParameter("@DCNo",SqlDbType.Char),//3
                                    new SqlParameter("@PONo",SqlDbType.Char),//4
                                    new SqlParameter("@VendorCode",SqlDbType.Char),//5
                                    new SqlParameter("@Remarks",SqlDbType.Char),//6
                                    new SqlParameter("@RejLocCode",SqlDbType.Char),//7
                                    new SqlParameter("@OtherCharges",SqlDbType.Float),//8
                                    new SqlParameter("@TotalGSTAmount",SqlDbType.Float),//9
                                    new SqlParameter("@TotalSEDAmount",SqlDbType.Float),//10
                                    new SqlParameter("@FurtherTaxAmount",SqlDbType.Float),//11
                                    new SqlParameter("@ExtraTaxAmount",SqlDbType.Float),//12
                                    new SqlParameter("@NetTotal",SqlDbType.Float),//13
                                    new SqlParameter("@PostFlag",SqlDbType.Char),//14
                                    new SqlParameter("@UpdUser",SqlDbType.Char),//15
                                    //new SqlParameter("@UpdDate",SqlDbType.DateTime),//
                                    //new SqlParameter("@UpdTime",SqlDbType.DateTime),//
                                    new SqlParameter("@UpdTerm",SqlDbType.VarChar),//16
                                    new SqlParameter("@Transferstatus",SqlDbType.Char),//17
                                    new SqlParameter("@RefInvNo",SqlDbType.VarChar),//18
                                    new SqlParameter("@RefInvDate",SqlDbType.DateTime),//19
                                    new SqlParameter("@DocumentNo",SqlDbType.VarChar),//20
                                    new SqlParameter("@TotalDiscount",SqlDbType.Float),//21
                                    new SqlParameter("@FreightCharges",SqlDbType.Float),//21
                                   };

                    param[0].Value = Session["DealerCode"].ToString();
                    GRNCode = SysFunc.AutoGen("GRNMaster", "GRNNO", txtGRNDate.Text.Trim());
                    param[1].Value = GRNCode;
                    param[2].Value = SysFunc.SaveDate(txtGRNDate.Text);
                    param[3].Value = ddlDCNo.SelectedValue.ToString().Trim();
                    param[4].Value = txtPONO.Text.Trim();
                    param[5].Value = txtVendorCode.Text.Trim();
                    param[6].Value = txtRemarks.Text.Replace("'", " ").Trim();
                    param[7].Value = "";
                    param[8].Value = Convert.ToDecimal(txtOtherChargers.Text.Trim() == "" ? "0" : txtOtherChargers.Text.Trim());
                    param[9].Value = Convert.ToDecimal(lblTotalGSTAmt.Text.Trim() == "" ? "0" : lblTotalGSTAmt.Text.Trim());
                    param[10].Value = Convert.ToDecimal("0");
                    param[11].Value = Convert.ToDecimal(lblTotalFurAmt.Text.Trim() == "" ? "0" : lblTotalFurAmt.Text.Trim());
                    param[12].Value = Convert.ToDecimal(lblTotalExAmt.Text.Trim() == "" ? "0" : lblTotalExAmt.Text.Trim());

                    param[13].Value = Convert.ToDecimal(txtNetAmount.Text.Trim() == "" ? "0" : txtNetAmount.Text.Trim());
                    param[14].Value = "N";
                    param[15].Value = Session["UserName"].ToString();
                    param[16].Value = GlobalVar.mUserIPAddress;
                    param[17].Value = "C";
                    param[18].Value = txtInvRefNo.Text.Trim();
                    param[19].Value = SysFunc.SaveDate(txtInvDate.Text);
                    param[20].Value = txtDocNo.Text.Trim();
                    param[21].Value = Convert.ToDecimal(lblTotalDiscount.Text.Trim() == "" ? "0" : lblTotalDiscount.Text.Trim());
                    param[22].Value = Convert.ToDecimal(txtFreightCharges.Text.Trim() == "" ? "0" : txtFreightCharges.Text.Trim());

                    if (SysFunc.SP_Spare_Insert_GRNMaster(Trans, param))  // GRN master 
                    {
                        for (int i = 0; i < GvMain.Rows.Count; i++)
                        {

                            SqlParameter[] param1 = {
                                             new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                             new SqlParameter("@GRNNo",SqlDbType.Char),//1
                                             new SqlParameter("@ItemCode",SqlDbType.Char),//2
                                             new SqlParameter("@PartsTransLocCode",SqlDbType.Char),//3
                                             new SqlParameter("@DCQty",SqlDbType.Float),//4
                                             new SqlParameter("@AccQty",SqlDbType.Float),//5
                                             new SqlParameter("@RejQty",SqlDbType.Float),//6
                                             new SqlParameter("@Rate",SqlDbType.Float),//7
                                             new SqlParameter("@Amount",SqlDbType.Float),//8
                                             new SqlParameter("@GSTPercent",SqlDbType.Float),//9
                                             new SqlParameter("@GSTAmount",SqlDbType.Float),//10
                                             new SqlParameter("@SEDage",SqlDbType.Float),//11
                                             new SqlParameter("@SEDAmount",SqlDbType.Float),//12
                                             new SqlParameter("@NetAmount",SqlDbType.Float),//13
                                             new SqlParameter("@DiscountAmt",SqlDbType.Float),//14
                                             new SqlParameter("@ExTaxPercent",SqlDbType.Float),//15
                                             new SqlParameter("@ExTaxAmt",SqlDbType.Float),//16
                                             new SqlParameter("@FurTaxPercent",SqlDbType.Float),//17
                                             new SqlParameter("@FurTaxAmt",SqlDbType.Float),//18
                                             new SqlParameter("@DiscountPerc",SqlDbType.Float)//19
                                          };

                            Label lblItemCode = (Label)GvMain.Rows[i].FindControl("lblItemCode");
                            Label lblLocationCode = (Label)GvMain.Rows[i].FindControl("lblLocationCode");
                            Label lblDCQty = (Label)GvMain.Rows[i].FindControl("lblDCQty");
                            Label lblAccQty = (Label)GvMain.Rows[i].FindControl("lblAccQty");
                            Label lblRate = (Label)GvMain.Rows[i].FindControl("lblRate");
                            Label LBGSTage = (Label)GvMain.Rows[i].FindControl("LBGSTage");
                            Label LbGSTAmount = (Label)GvMain.Rows[i].FindControl("LbGSTAmount");
                            Label lblExTaxPerc = (Label)GvMain.Rows[i].FindControl("lblExTaxPerc");
                            Label lblExTaxAmt = (Label)GvMain.Rows[i].FindControl("lblExTaxAmt");
                            Label lblFurTaxPerc = (Label)GvMain.Rows[i].FindControl("lblFurTaxPerc");
                            Label lblFurTaxAmt = (Label)GvMain.Rows[i].FindControl("lblFurTaxAmt");
                            Label lblDiscount = (Label)GvMain.Rows[i].FindControl("lblDiscount");
                            Label LbAmountMain = (Label)GvMain.Rows[i].FindControl("LbAmountMain");
                            Label lblDiscountPerc = (Label)GvMain.Rows[i].FindControl("lblDiscountPerc");
                            decimal dQty = 0, dRate, dDiscount=0;
                            dQty = Convert.ToDecimal(lblAccQty.Text.Trim() == "" ? "0" : lblAccQty.Text.Trim());
                            dRate = Convert.ToDecimal(lblRate.Text.Trim() == "" ? "0" : lblRate.Text.Trim());
                            dDiscount = Convert.ToDecimal(lblDiscount.Text.Trim() == "" ? "0" : lblDiscount.Text.Trim());

                            param1[0].Value = Session["DealerCode"].ToString();
                            param1[1].Value = GRNCode;
                            param1[2].Value = lblItemCode.Text.Trim();
                            param1[3].Value = lblLocationCode.Text.Trim();
                            param1[4].Value = Convert.ToDecimal(lblDCQty.Text.Trim() == "" ? "0" : lblDCQty.Text.Trim());
                            param1[5].Value = dQty;
                            param1[6].Value = 0.0;
                            param1[7].Value = dRate;
                            param1[8].Value = (dRate - dDiscount) * dQty;
                            param1[9].Value = Convert.ToDecimal(LBGSTage.Text.Trim() == "" ? "0" : LBGSTage.Text.Trim());
                            param1[10].Value = Convert.ToDecimal(LbGSTAmount.Text.Trim() == "" ? "0" : LbGSTAmount.Text.Trim());
                            param1[11].Value = 0;
                            param1[12].Value = 0;
                            param1[13].Value = Convert.ToDecimal(LbAmountMain.Text.Trim() == "" ? "0" : LbAmountMain.Text.Trim());
                            param1[14].Value = dDiscount;
                            param1[15].Value = Convert.ToDecimal(lblExTaxPerc.Text.Trim() == "" ? "0" : lblExTaxPerc.Text.Trim());
                            param1[16].Value = Convert.ToDecimal(lblExTaxAmt.Text.Trim() == "" ? "0" : lblExTaxAmt.Text.Trim());
                            param1[17].Value = Convert.ToDecimal(lblFurTaxPerc.Text.Trim() == "" ? "0" : lblFurTaxPerc.Text.Trim());
                            param1[18].Value = Convert.ToDecimal(lblFurTaxAmt.Text.Trim() == "" ? "0" : lblFurTaxAmt.Text.Trim());
                            param1[19].Value = Convert.ToDecimal(lblDiscountPerc.Text.Trim() == "" ? "0" : lblDiscountPerc.Text.Trim());

                            if (SysFunc.SP_Spare_Insert_GRNDetail(Trans, param1)) check = true;
                            else
                            {
                                check = false;
                                break;
                            }
                        }
                    }
                }
                if (check == true) // details 2
                {
                    //if (chkSave == "N") check = true;

                    for (int k = 0; k < GvMain.Rows.Count; k++)
                    {
                        SqlParameter[] param2 = {
                                             new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                             new SqlParameter("@PartsTransLocCode",SqlDbType.Char),//1
                                             new SqlParameter("@ItemCode",SqlDbType.Char),//2
                                             new SqlParameter("@RecQty",SqlDbType.Char),//3
                                             new SqlParameter("@Rate",SqlDbType.Char),//4
                                       };

                        Label lblItemCode = (Label)GvMain.Rows[k].FindControl("lblItemCode");
                        Label lblLocationCode = (Label)GvMain.Rows[k].FindControl("lblLocationCode");
                        Label lblAccQty = (Label)GvMain.Rows[k].FindControl("lblAccQty");
                        Label lblRate = (Label)GvMain.Rows[k].FindControl("lblRate");

                        param2[0].Value = Session["DealerCode"].ToString();
                        param2[1].Value = lblLocationCode.Text.Trim();
                        param2[2].Value = lblItemCode.Text.Trim();
                        param2[3].Value = Convert.ToDecimal(lblAccQty.Text.Trim() == "" ? "0" : lblAccQty.Text.Trim());
                        param2[4].Value = Convert.ToDecimal(lblRate.Text.Trim() == "" ? "0" : lblRate.Text.Trim());
                        if (SysFunc.SP_Spare_Insert_GRNItemStock(Trans, param2)) check = true;
                        else
                        {
                            check = false;
                            break;
                        }
                    }

                    if (SysFunc.UpdateStockFlag(Trans, GRNCode, Session["DealerCode"].ToString())) check = true; else check = false;
                    for (int l = 0; l < GvMain.Rows.Count; l++)
                    {
                        SqlParameter[] param3 = {
                                               new SqlParameter("@ItemCode",SqlDbType.Char),
                                               new SqlParameter("@DealerCode",SqlDbType.Char),
                                               new SqlParameter("@PartsTransLocCode",SqlDbType.Char)
                                            };

                        Label lblItemCode = (Label)GvMain.Rows[l].FindControl("lblItemCode");
                        Label lblLocationCode = (Label)GvMain.Rows[l].FindControl("lblLocationCode");

                        param3[0].Value = lblItemCode.Text.Trim();
                        param3[1].Value = Session["DealerCode"].ToString();
                        param3[2].Value = lblLocationCode.Text.Trim();

                        if (SysFunc.SP_Spare_update_PartLocation(Trans, param3)) check = true; else check = false;
                    }
                }


                if (check == false)
                {
                    ObjTrans.RollBackTransaction(ref Trans);
                    SysFunc.UserMsg(LbErr, Color.Red, "Record not save and post please try again");
                }
                else
                {
                    ObjTrans.CommittTransaction(ref Trans);
                    SysFunc.UserMsg(LbErr, Color.Green, "Record Saved and post successfully. Last Code:" + GRNCode);
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Record Saved,Updated Successfully: " +GRNCode + "')", true);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);
                }
            }


            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                SysFunc.UserMsg(LbErr, Color.Red, "Error: " + ex.Message);
            }
        }
        //private void InsertItemStock()
        //{
        //    SqlTransaction Trans = null;
        //    bool check = false;
        //    try
        //    {
        //        if (Insert_MasterDetailMain())
        //        {
        //            for (int i = 0; i < GvMain.Rows.Count; i++)
        //            {
        //                SqlParameter[] param = {
        //                                     new SqlParameter("@DealerCode",SqlDbType.Char),//0
        //                                     new SqlParameter("@PartsTransLocCode",SqlDbType.Char),//1
        //                                     new SqlParameter("@ItemCode",SqlDbType.Char),//2
        //                                     new SqlParameter("@RecQty",SqlDbType.Char),//3
        //                                     new SqlParameter("@Rate",SqlDbType.Char),//4
        //                               };

        //                Label LbPartNo = (Label)GvMain.Rows[i].FindControl("LbPartNo");
        //                TextBox txtLocationCode = (TextBox)GvMain.Rows[i].FindControl("txtLocationCode");
        //                TextBox txtAcceptedQty = (TextBox)GvMain.Rows[i].FindControl("txtAcceptedQty");
        //                TextBox txtRate = (TextBox)GvMain.Rows[i].FindControl("txtRate");

        //                param[0].Value = Session["DealerCode"].ToString();
        //                param[1].Value = txtLocationCode.Text.Trim();
        //                param[2].Value = LbPartNo.Text.Trim();
        //                param[3].Value = Convert.ToDecimal(txtAcceptedQty.Text.Trim() == "" ? "0" : txtAcceptedQty.Text.Trim());
        //                param[4].Value = Convert.ToDecimal(txtRate.Text.Trim() == "" ? "0" : txtRate.Text.Trim());
        //                if (SysFunc.SP_Spare_Insert_GRNItemStock(Trans, param)) check = true;
        //                else
        //                {
        //                    check = false;
        //                    break;
        //                }

        //            }

        //            if (SysFunc.UpdateStockFlag(Trans, txtGRNNo.Text, Session["DealerCode"].ToString())) check = true; else check = false;
        //        }
        //        else check = false;

        //        if (check == false) ObjTrans.RollBackTransaction(ref Trans); else ObjTrans.CommittTransaction(ref Trans);
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjTrans.RollBackTransaction(ref Trans);
        //        throw ex;
        //    }
        //}
        private void Get_GRNInfo()
        {

            DataSet dsGRN = new DataSet();
            try
            {
                SqlParameter[] param = {
                                       new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                       new SqlParameter("@GRNNO",SqlDbType.Char,10)
                                  };
                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlGRNNo.SelectedValue.ToString();
                dsGRN = SysFunc.SP_Part_Get_GRNInfo(param);

                txtPONO.Text = dsGRN.Tables[0].Rows[0]["PONO"].ToString();
                txtPOType.Text = dsGRN.Tables[0].Rows[0]["NName"].ToString();
                //ddlDCNo.SelectedValue = dsGRN.Tables[0].Rows[0]["DCNo"].ToString();
                txtDCDate.Text = dsGRN.Tables[0].Rows[0]["DCDate"].ToString();
                txtVendorCode.Text = dsGRN.Tables[0].Rows[0]["VendorCode"].ToString();
                txtVendorDesc.Text = dsGRN.Tables[0].Rows[0]["VendorDesc"].ToString();
                txtRemarks.Text = dsGRN.Tables[0].Rows[0]["Remarks"].ToString();
                txtPODate.Text = dsGRN.Tables[0].Rows[0]["PODate"].ToString();
                txtInvRefNo.Text = dsGRN.Tables[0].Rows[0]["RefInvNo"].ToString();
                txtInvDate.Text = dsGRN.Tables[0].Rows[0]["RefInvDate"].ToString();
                txtDocNo.Text = dsGRN.Tables[0].Rows[0]["DocumentNo"].ToString();
                txtOtherChargers.Text = dsGRN.Tables[0].Rows[0]["OtherCharges"].ToString();
                txtFreightCharges.Text = dsGRN.Tables[0].Rows[0]["FreightCharges"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CtlClear()
        {
            try
            {
                ddlGRNNo.SelectedIndex = 0;
                if (txtGRNDate.Text.Length == 0) txtGRNDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtPOType.Text = "";
                ddlDCNo.SelectedIndex = 0;
                txtDCDate.Text = string.Empty;
                txtPONO.Text = "";
                txtVendorCode.Text = "";
                txtVendorDesc.Text = "";
                txtRemarks.Text = "";
                GvMain.DataSource = null;
                GvMain.DataBind();
                txtOtherChargers.Text = "";
                txtFreightCharges.Text = "";
                txtNetAmount.Text = "";
                txtInvRefNo.Text = string.Empty;
                txtInvDate.Text = string.Empty;
                txtPODate.Text = string.Empty;
                txtDocNo.Text = string.Empty;
                ddlDCNo.Enabled = true;
                GridNetAmount.Text = string.Empty;
            }
            catch (Exception ex)
            {
                LbErr.Text = ex.Message;
            }
        }
        private void CalculateShiftedAmount()
        {
            double Factor = 0.0, DTotal = 0.0, DOtherChargers = 0.0, TotalPartPrice = 0.0, TotalShiftValue = 0.0, DAcceptedQty = 0.0;
            try
            {
                //Factor = (Convert.ToDouble(txtOtherChargers.Text)/);
                DTotal = Convert.ToDouble(txtNetAmount.Text);
                DOtherChargers = Convert.ToDouble(txtOtherChargers.Text == "" ? "0" : txtOtherChargers.Text);
                Factor = ((DOtherChargers / ((DTotal == 0 ? 1 : DTotal) - DOtherChargers)) * 100);


                for (int i = 0; i < GvMain.Rows.Count; i++)
                {
                    Label LbAmountMain = (Label)GvMain.Rows[i].FindControl("LbAmountMain");
                    TextBox txtAcceptedQty = (TextBox)GvMain.Rows[i].FindControl("txtAcceptedQty");
                    TextBox txtRate = (TextBox)GvMain.Rows[i].FindControl("txtRate");

                    TotalPartPrice = Convert.ToDouble(LbAmountMain.Text == "" ? "0" : LbAmountMain.Text);
                    TotalShiftValue = TotalPartPrice + TotalPartPrice * Factor / 100;
                    DAcceptedQty = Convert.ToDouble(txtAcceptedQty.Text == "" ? "0" : txtAcceptedQty.Text);

                    if (DAcceptedQty != 0) txtRate.Text = Convert.ToString(TotalShiftValue / DAcceptedQty); else txtRate.Text = "0.0";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void Update_MasterDetail()
        {
            //SqlTransaction Trans = null;
            //bool Check = false;
            //try
            //{
            //    if (ObjTrans.BeginTransaction(ref Trans) == false) return;
            //    SqlParameter[] param = {
            //                        new SqlParameter("@DealerCode",SqlDbType.Char),//1
            //                        new SqlParameter("@GRNNo",SqlDbType.Char),//2
            //                        new SqlParameter("@Remarks",SqlDbType.Char),//3
            //                        new SqlParameter("@RejLocCode",SqlDbType.Char),//4
            //                        new SqlParameter("@OtherCharges",SqlDbType.Float),//5
            //                        new SqlParameter("@TotalGSTAmount",SqlDbType.Float),//6
            //                        new SqlParameter("@TotalSEDAmount",SqlDbType.Float),//7
            //                        new SqlParameter("@NetTotal",SqlDbType.Float),//8
            //                        new SqlParameter("@PostFlag",SqlDbType.Char),//9
            //                        new SqlParameter("@UpdUser",SqlDbType.Char),//10
            //                        //new SqlParameter("@UpdDate",SqlDbType.DateTime),//11
            //                        //new SqlParameter("@UpdTime",SqlDbType.DateTime),//12
            //                        new SqlParameter("@UpdTerm",SqlDbType.VarChar),//13
            //                        new SqlParameter("@Transferstatus",SqlDbType.Char)//14
            //                       };

            //    param[0].Value = Session["DealerCode"].ToString();
            //    param[1].Value = txtGRNNo.Text;
            //    param[2].Value = txtRemarks.Text.Replace("'", " ").Trim();
            //    param[3].Value = "";
            //    param[4].Value = Convert.ToDecimal(txtOtherChargers.Text.Trim() == "" ? "0" : txtOtherChargers.Text.Trim());
            //    param[5].Value = Convert.ToDecimal(txtGSTAmount.Text.Trim() == "" ? "0" : txtGSTAmount.Text.Trim());
            //    param[6].Value = Convert.ToDecimal(txtWHTAmount.Text.Trim() == "" ? "0" : txtWHTAmount.Text.Trim());
            //    param[7].Value = Convert.ToDecimal(txtNetAmount.Text.Trim() == "" ? "0" : txtNetAmount.Text.Trim());
            //    param[8].Value = "N";
            //    param[9].Value = Session["UserName"].ToString();
            //    //param[10].Value = ObjGeneral.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
            //    //param[11].Value = ObjGeneral.SaveTime(DateTime.Now.ToString("hh:mm"));
            //    param[10].Value = "";
            //    param[11].Value = "C";

            //    if (SysFunc.SP_Spare_Update_GRNMaster(Trans, param) == true)
            //    {

            //        for (int i = 0; i < GvMain.Rows.Count; i++)
            //        {

            //            SqlParameter[] param1 = {
            //                                 new SqlParameter("@DealerCode",SqlDbType.Char),//0
            //                                 new SqlParameter("@GRNNo",SqlDbType.Char),//1
            //                                 new SqlParameter("@ItemCode",SqlDbType.Char),//2
            //                                 new SqlParameter("@PartsTransLocCode",SqlDbType.Char),//3
            //                                 new SqlParameter("@DCQty",SqlDbType.Float),//4
            //                                 new SqlParameter("@AccQty",SqlDbType.Float),//5
            //                                 new SqlParameter("@RejQty",SqlDbType.Float),//6
            //                                 new SqlParameter("@Rate",SqlDbType.Float),//7
            //                                 new SqlParameter("@Amount",SqlDbType.Float),//8
            //                                 new SqlParameter("@GSTPercent",SqlDbType.Float),//9
            //                                 new SqlParameter("@GSTAmount",SqlDbType.Float),//10
            //                                 new SqlParameter("@SEDage",SqlDbType.Float),//11
            //                                 new SqlParameter("@SEDAmount",SqlDbType.Float),//12
            //                                 new SqlParameter("@NetAmount",SqlDbType.Float)//13
            //                              };

            //            Label LbPartNo = (Label)GvMain.Rows[i].FindControl("LbPartNo");
            //            TextBox txtLocationCode = (TextBox)GvMain.Rows[i].FindControl("txtLocationCode");
            //            Label LbDCQty1 = (Label)GvMain.Rows[i].FindControl("LbDCQty1");
            //            TextBox txtAcceptedQty = (TextBox)GvMain.Rows[i].FindControl("txtAcceptedQty");
            //            TextBox txtRate = (TextBox)GvMain.Rows[i].FindControl("txtRate");
            //            Label LBGSTage = (Label)GvMain.Rows[i].FindControl("LBGSTage");

            //            Label LbGSTAmount = (Label)GvMain.Rows[i].FindControl("LbGSTAmount");
            //            Label LbWHTage = (Label)GvMain.Rows[i].FindControl("LbWHTage");
            //            Label LbSEDAmount = (Label)GvMain.Rows[i].FindControl("LbSEDAmount");




            //            param1[0].Value = Session["DealerCode"].ToString();
            //            param1[1].Value = txtGRNNo.Text.Trim();
            //            param1[2].Value = LbPartNo.Text.Trim();
            //            param1[3].Value = txtLocationCode.Text.Trim();
            //            param1[4].Value = Convert.ToDecimal(LbDCQty1.Text.Trim() == "" ? "0" : LbDCQty1.Text.Trim());
            //            param1[5].Value = Convert.ToDecimal(txtAcceptedQty.Text.Trim() == "" ? "0" : txtAcceptedQty.Text.Trim());
            //            param1[6].Value = 0.0;
            //            param1[7].Value = Convert.ToDecimal(txtRate.Text.Trim() == "" ? "0" : txtRate.Text.Trim());
            //            param1[8].Value = Convert.ToDecimal(txtRate.Text.Trim() == "" ? "0" : txtRate.Text.Trim());
            //            param1[9].Value = Convert.ToDecimal(LBGSTage.Text.Trim() == "" ? "0" : LBGSTage.Text.Trim());
            //            param1[10].Value = Convert.ToDecimal(LbGSTAmount.Text.Trim() == "" ? "0" : LbGSTAmount.Text.Trim());
            //            param1[11].Value = Convert.ToDecimal(LbWHTage.Text.Trim() == "" ? "0" : LbWHTage.Text.Trim());
            //            param1[12].Value = Convert.ToDecimal(LbSEDAmount.Text.Trim() == "" ? "0" : LbSEDAmount.Text.Trim());
            //            param1[13].Value = 100;//Convert.ToDecimal(txtAcceptedQty.Text.Trim() == "" ? "0" : txtAcceptedQty.Text.Trim());



            //            if (SysFunc.SP_Spare_Insert_GRNDetail(Trans, param1) == true) Check = true;
            //            else
            //            {
            //                Check = false;
            //                break;
            //            }
            //        }

            //    }
            //    else Check = false;

            //    //return Check;

            //    if (Check == false)
            //    {
            //        ObjTrans.RollBackTransaction(ref Trans);
            //        SysFunc.UserMsg(LbErr, Color.Red, "Not Saved Successfully", txtGRNNo);
            //    }
            //    else
            //    {
            //        ObjTrans.CommittTransaction(ref Trans);
            //        SysFunc.UserMsg(LbErr, Color.Red, "Saved Successfully " + txtGRNNo.Text, txtGRNNo);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    SysFunc.UserMsg(LbErr, Color.Red, ex.Message, txtGRNNo);
            //    ObjTrans.RollBackTransaction(ref Trans);

            //    throw ex;
            //}
        }


        //public bool UpdatePost(SqlTransaction Trans)
        //{
        //    bool Check;
        //    try
        //    {
        //        SqlParameter[] param = {
        //                              new SqlParameter("@GRNNo",SqlDbType.Char),
        //                              new SqlParameter("@DEALER_CODE",SqlDbType.Char),
        //                           };

        //        param[0].Value = txtGRNNo.Text.Trim();
        //        param[1].Value = Session["DealerCode"].ToString();

        //        Check = SysFunc.SP_Part_Update_UpdatePost(Trans, param);

        //        return Check;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        #endregion

        //protected void ImgLookupGRN_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        ViewState["lookupid"] = 38;
        //        ViewState["ixd1"] = 1;
        //        ViewState["ixd2"] = 2;
        //        ViewState["ixd3"] = 3;

        //        //ObjLookup.LU_Get_GRNNumber(ImgLookupDC, ViewState["lookupid"].ToString(), "", "../../../../");
        //        LbErr.Text = "";

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //protected void ImgLookupDC_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        ViewState["lookupid"] = 20;
        //        ViewState["ixd1"] = 1;
        //        ViewState["ixd2"] = 2;
        //        ViewState["ixd3"] = 3;

        //        //ObjLookup.LU_Get_DCNumber(ImgLookupDC, ViewState["lookupid"].ToString(), "", "../../../../", "DC");
        //        LbErr.Text = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        LbErr.Text = ex.Message;
        //    }

        //}
        protected void GvMain_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                Label lblDocType = (Label)GvMain.Rows[e.NewEditIndex].FindControl("lblLocationCode");
                if (lblDocType.Text.Trim() != string.Empty)
                {
                    Session["lblLocationCode"] = lblDocType.Text;
                }
                GvMain.EditIndex = e.NewEditIndex;
                ds = (DataSet)Session["dsDCGrid"];
                GvMain.DataSource = ds.Tables[0];
                GvMain.DataBind();
                //FillDCGV();
            }
            catch (Exception ex)
            {
                LbErr.Text = ex.Message;
            }
        }
        protected void GvMain_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataSet ds = new DataSet();
            GvMain.EditIndex = -1;
            ds = (DataSet)Session["dsDCGrid"];
            GvMain.DataSource = ds.Tables[0];
            GvMain.DataBind();

            //FillDCGV();
        }
        //protected void ImgLookupLocation_Click(object sender, ImageClickEventArgs e)
        //{
        //    int RowGvIndex = 0;
        //    try
        //    {

        //        ViewState["lookupid"] = 33;
        //        ViewState["ixd1"] = 1;
        //        ViewState["ixd2"] = 2;

        //        ImageButton BtnLnk = (ImageButton)sender;
        //        GridViewRow RowGv = (GridViewRow)BtnLnk.Parent.Parent;
        //        RowGvIndex = RowGv.RowIndex;
        //        HDGVIndex.Value = Convert.ToString(RowGvIndex);

        //        //SysFunc.LU_Get_PartItemLocation(ImgLookupDC, ViewState["lookupid"].ToString(), "", "../../../../");



        //        LbErr.Text = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        LbErr.Text = ex.Message;
        //    }
        //}
        protected void BtnRemove_Click(object sender, EventArgs e)
        {
            DataSet dsDelete = new DataSet();
            try
            {
                dsDelete = (DataSet)ViewState["ObjPOds"];

                LinkButton BtnLnk = (LinkButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;

                dsDelete.Tables[0].Rows[rowGvIndex].Delete();
                GvMain.DataSource = dsDelete.Tables[0];
                GvMain.DataBind();
                LbErr.Text = "";
            }
            catch (Exception ex)
            {
                LbErr.Text = ex.Message;
            }
        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2543", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (ddlGRNNo.SelectedIndex != 0)
            {
                if (!sec.UserRight("2543", "003"))
                {
                    Response.Redirect("~/Test.aspx");
                }
            }
          
            try
            {

                //for (int i = 0; i < GvMain.Rows.Count; i++)
                //{
                //    TextBox txtAcceptedQty = (TextBox)GvMain.Rows[i].FindControl("txtAcceptedQty");
                //    if (txtAcceptedQty.Text == "")
                //    {
                //        SysFunc.UserMsg(LbErr, Color.Red, "Accepted Qty. should not be left blank...", txtAcceptedQty);
                //        return;
                //    }
                //}

                //if (SysFunc.IsPosted("GRNMaster", "GRNNo", txtGRNNo.Text, "") == true)
                //{
                //    SysFunc.UserMsg(LbErr, Color.Red, "The GRN is already posted; now you could not change/delete/Post the record...", txtGRNNo);
                //    //LbErr.Text = "The GRN is already posted; now you could not change/delete/Post the record...";
                //    return;
                //}
                //else
                //{
                //    LbErr.Text = "";
                //}
                //foreach (GridViewRow dr in GvMain.Rows)
                //{
                //    TextBox GVtxtLocCode = (TextBox)dr.FindControl("txtLocationCode");
                //    if (GVtxtLocCode.Text.Trim() == "")
                //    {
                //        SysFunc.UserMsg(LbErr, Color.Red, "Location Code dose not be left blank...", GVtxtLocCode);
                //        return;
                //    }
                //}

                //if (HDDelete.Value == "1")
                //{
                //    Update_MasterDetail();
                //    HDDelete.Value = "0";
                //}
                //else 
                Insert_MasterDetail();

                CtlClear();
            }
            catch (Exception ex)
            {
                LbErr.Text = ex.Message;
            }
        }
        protected void btnPosting_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2543", "005"))
            {
                Response.Redirect("~/Test.aspx");
            }
            //  bool bStockFlag;
            try
            {
                LbErr.Text = "";
                //if (txtGRNNo.Text.Trim() != "")
                //{
                //    if (SysFunc.IsExist("GRNNo", txtGRNNo.Text.Trim(), "GRNMaster", "And DelFlag='N'") == false)
                //    {
                //        SysFunc.UserMsg(LbErr, Color.Red, "DCNo Should not be left blank");
                //        return;
                //    }
                //}
                if (ddlGRNNo.SelectedIndex != 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "The GRN is already posted; now you could not change/delete/Post the record...");
                    return;
                }
                if (Convert.ToDateTime(SysFunc.SaveDate(txtGRNDate.Text)).Date > Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd")).Date)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "GRN Date should not greater then current date.");
                    return;
                }
                if (Convert.ToDateTime(SysFunc.SaveDate(txtGRNDate.Text)).Date < Convert.ToDateTime(SysFunc.SaveDate(txtDCDate.Text)).Date)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "GRN Date should not less then DC Date");
                    return;
                }
                if (ddlDCNo.SelectedIndex==0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "DCNo Should not be left blank");
                    return;
                }
                if (txtInvDate.Text.Trim() == string.Empty)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Invoice Date Should not be left blank");
                    return; 
                }
                if (txtInvRefNo.Text.Trim() == string.Empty)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Invoice Number Should not be left blank");
                    return;
                }
                //CalculateShiftedAmount();
                //for (int i = 0; i < GvMain.Rows.Count; i++)
                //{
                //    TextBox txtAcceptedQty = (TextBox)GvMain.Rows[i].FindControl("txtAcceptedQty");
                //    if (txtAcceptedQty.Text == "")
                //    {
                //        SysFunc.UserMsg(LbErr, Color.Red, "Accepted Qty. should not be left blank...", txtAcceptedQty);
                //        return;
                //    }
                //    else LbErr.Text = "";
                //}

                //if (SysFunc.IsPosted("GRNMaster", "GRNNo", txtGRNNo.Text, "") == true)
                //{
                //    SysFunc.UserMsg(LbErr, Color.Red, "The GRN is already posted; now you could not change/delete/Post the record...", txtGRNNo);
                //    return;
                //}
                //else LbErr.Text = "";

                //if (txtGRNNo.Text.Trim() != string.Empty)
                //{
                //    if (SysFunc.CodeExists("GRNMaster", "GRNNO", txtGRNNo.Text.Trim())) InsertItemStockMain("N");
                //    else
                //    {
                //        SysFunc.UserMsg(LbErr, Color.Red, "GRN number dose not exists.", txtGRNNo);
                //        return;
                //    }
                //}
                //else 
                InsertItemStockMain("Y");
                CtlClear();
                LoadGRN_DDL();
                LoadDC_DDL();
            }
            catch (Exception ex)
            {
                LbErr.Text = ex.Message;
            }
        }
        protected void BtnPrint_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2543", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }
            try
            {
                if (ddlGRNNo.SelectedIndex == 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Please select GRN No.");
                    return;
                }

                
                ReportDocument RD = new ReportDocument();
                
                //ReportDocument RD = new ReportDocument();
                //DataSet dsRpt = new DataSet();
                DXBMS.Data.DataSet1 objDsReports = new Data.DataSet1();


                //SqlParameter[] param = {
                //                   new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
                //                   new SqlParameter("@GRNNo",SqlDbType.Char,8),//1
                //               };
                //param[0].Value = Session["DealerCode"].ToString();
                //param[1].Value = ddlGRNNo.SelectedValue.ToString().Trim();
               



                //if (myFunc.ExecuteSPDMIS("sp2W_Spare_InvoiceDetail", param, ref rder))
                //if(myFunc.ExecuteSP("sp2W_Spare_InvoiceDetail",param,ref rder))
                //{
                //    objDsReports.sp2W_Spare_InvoiceDetail.Load(rder);


                //}

                DataTable dt = new DataTable();
                string sql = "exec Sp_Spare_Get_GRNReport '" + Session["DealerCode"].ToString() + "','" + ddlGRNNo.SelectedValue.ToString().Trim() + "'";
                dt = SysFunc.GetData(sql);
                RD.PrintOptions.PaperSize = PaperSize.PaperA4;


                //RD.Load(Server.MapPath("../../SpareReports/rptPartsReceivedDetail.rpt"));
                RD.Load(Server.MapPath("../SpareReports/rptGRNPrint.rpt"));
                RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
                RD.DataDefinition.FormulaFields["NTN"].Text = "'" + Session["DealerNTN"].ToString() + "'";
                RD.DataDefinition.FormulaFields["SaleTaxNo"].Text = "'" + Session["DealerSaleTaxNo"].ToString() + "'";

                RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";

                RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["FromCode"].Text = "'" + FDate + "'";
                //RD.DataDefinition.FormulaFields["ToCode"].Text = "'" + TDate + "'";
                RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "GOOD RECEIPT NOTE (GRN)" + "\"";
                RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
                //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
                RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
                //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
                //RD.Database.Tables[0].SetDataSource(objDsReports);
                RD.Database.Tables[0].SetDataSource(dt);


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
                // convert and show
                ////string FilePath = Server.MapPath("~") + "\\Download\\";
                ////string FileName = "GRNPrint'" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                ////string File = FilePath + FileName;

                ////RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

                ////string URL;
                ////URL = "../../../Download/OpenPdf.aspx?FileName=" + FileName;
                ////string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
                ////ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
                ///////////////////////////////////

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            CtlClear();
            LbErr.Text = "";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);
        }
        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2543", "004"))
            {
                Response.Redirect("~/Test.aspx");
            }
            //SysFunc.UserMsg(LbErr, Color.Red, "Delete should not be allowed");
            //return;
            //DataSet dsPostPO = new DataSet();
            //SqlTransaction Trans = null;
            //bool Check = false;
            //try
            //{
            //    if (ObjTrans.BeginTransaction(ref Trans) == false) return;
            //    if (SysFunc.CodeExists("GRNMaster", "GRNNo", txtGRNNo.Text.Trim(), " and DelFlag = 'N'") == true)
            //    {
            //        SysFunc.UserMsg(LbErr, Color.Red, "GRN No. does not exist...", txtGRNNo);
            //        return;
            //    }
            //    else LbErr.Text = "";
            //    if (SysFunc.IsPosted("GRNMaster", "GRNNo", txtGRNNo.Text,Session["DealerCode"].ToString()) == true)
            //    {
            //        SysFunc.UserMsg(LbErr, Color.Red, "The GRN is already posted; now you could not change/delete/Post the record...", txtGRNNo);
            //        return;
            //    }
            //    else LbErr.Text = "";
            //    SqlParameter[] param2 = {
            //                        new SqlParameter("@GRNNo",SqlDbType.Char),
            //                        new SqlParameter("@DEALER_CODE",SqlDbType.Char)
            //                       };

            //    param2[0].Value = txtGRNNo.Text;
            //    param2[1].Value = Session["DealerCode"].ToString();

            //    if (SysFunc.SP_Spare_Delete_GRNMaster(Trans, param2)) Check = true; else Check = false;
            //    if (Check == true)
            //    {
            //        if (SysFunc.SP_Spare_Delete_GRNDetail(Trans, param2)) Check = true; else Check = false;
            //    }

            //    if (Check == false)
            //    {
            //        ObjTrans.RollBackTransaction(ref Trans);
            //        SysFunc.UserMsg(LbErr, Color.Red, "Record not delete please try again", txtGRNNo);
            //    }
            //    else
            //    {
            //        ObjTrans.CommittTransaction(ref Trans);
            //        SysFunc.UserMsg(LbErr, Color.Green, "Record deleted successfully", txtGRNNo);
            //    }
            //    CtlClear();
            //}
            //catch (Exception ex)
            //{
            //    ObjTrans.RollBackTransaction(ref Trans);
            //    throw ex;
            //}
        }

        protected void GvMain_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && (GvMain.EditIndex == e.Row.RowIndex))
                {
                    if (ddlGRNNo.SelectedValue == "" || ddlGRNNo.SelectedIndex == 0)
                    {


                        DropDownList ddlEditLoc = (DropDownList)e.Row.FindControl("ddlLocation");
                        string queryLocation = "select A.PartsTransLocCode,LTrim(RTrim(B.PartsLocDesc)) +','+ LTrim(RTrim(C.PartsSubLocDesc)) +','+ LTrim(RTrim(A.PartsTransLocDesc))  As PartsTransLocDesc " +
        "from PartsTransLocation A " +
        "Inner join  partslocation B " +
        "On A.PartsLocCode=B.PartsLocCode " +
        "And A.DealerCode=B.DealerCode " +
        "Inner join PartsSubLocation C " +
        "On A.PartsSubLocCode=C.PartsSubLocCode " +
        "And A.DealerCode=C.DealerCode " +
        "Where A.DealerCode='" + Session["DealerCode"].ToString() + "' ";

                        ObjMain.FillDropDown(ddlEditLoc, queryLocation, "PartsTransLocDesc", "PartsTransLocCode", "Select");
                        if (Session["lblLocationCode"] != null)
                        {
                            ddlEditLoc.SelectedValue = Session["lblLocationCode"].ToString();
                            //ddlEditLoc.Items.FindByValue(Session["lblLocationCode"].ToString()).Selected = true;
                        }
                    }
                    else
                    {
                        DropDownList ddlEditLoc = (DropDownList)e.Row.FindControl("ddlLocation");
                        string queryLocation = "select A.PartsTransLocCode,LTrim(RTrim(B.PartsLocDesc)) +','+ LTrim(RTrim(C.PartsSubLocDesc)) +','+ LTrim(RTrim(A.PartsTransLocDesc))  As PartsTransLocDesc " +
     "from PartsTransLocation A " +
     "Inner join  partslocation B " +
     "On A.PartsLocCode=B.PartsLocCode " +
     "And A.DealerCode=B.DealerCode " +
     "Inner join PartsSubLocation C " +
     "On A.PartsSubLocCode=C.PartsSubLocCode " +
     "And A.DealerCode=C.DealerCode " +
     "Where A.DealerCode='" + Session["DealerCode"].ToString() + "' ";

                        ObjMain.FillDropDown(ddlEditLoc, queryLocation, "PartsTransLocDesc", "PartsTransLocCode", "Select");
                        if (Session["lblLocationCode"] != null)
                        {
                            ddlEditLoc.SelectedValue = Session["lblLocationCode"].ToString();
                            //ddlEditLoc.Items.FindByValue(Session["lblLocationCode"].ToString()).Selected = true;
                        }
                        TextBox txtGrnQty = (TextBox)e.Row.FindControl("txtAcceptedQty");
                        ddlEditLoc.Enabled = false;
                        txtGrnQty.Enabled = false;

                    }
                   
                }
                if (e.Row.RowType == DataControlRowType.DataRow && !(GvMain.EditIndex == e.Row.RowIndex))
                {
                    string strTotals;
                    decimal dtotalvalue;
                    //Sum of GRN Qty
                    strTotals = ((Label)e.Row.FindControl("lblAccQty")).Text;
                    dtotalvalue = Convert.ToDecimal(strTotals);
                    sumFooterGRNQty += dtotalvalue;

                    //Sum of GST Amt
                    strTotals = ((Label)e.Row.FindControl("LbGSTAmount")).Text;
                    dtotalvalue = Convert.ToDecimal(strTotals);
                    SumFooterGSTAmt += dtotalvalue;

                    //Sum of Extra Tax Amt
                    strTotals = ((Label)e.Row.FindControl("lblExTaxAmt")).Text;
                    dtotalvalue = Convert.ToDecimal(strTotals);
                    SumFooterExAmt += dtotalvalue;

                    //Sum of Further Tax Amt
                    strTotals = ((Label)e.Row.FindControl("lblFurTaxAmt")).Text;
                    dtotalvalue = Convert.ToDecimal(strTotals);
                    SumFooterFurAmt += dtotalvalue;

                    //Sum of Discount Amt
                    strTotals = ((Label)e.Row.FindControl("lblDiscount")).Text;
                    dtotalvalue = Convert.ToDecimal(strTotals);
                    SumFooterDiscount += dtotalvalue;

                    ////Sum of Discount Amt
                    //strTotals = ((Label)e.Row.FindControl("lblDiscount")).Text;
                    //dtotalvalue = Convert.ToDecimal(strTotals);
                    //SumFooterDiscount += dtotalvalue;

                    //Sum of Net Amt
                    strTotals = ((Label)e.Row.FindControl("LbAmountMain")).Text;
                    dtotalvalue = Convert.ToDecimal(strTotals);
                    SumFooterTotalAmt += dtotalvalue;


                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    //Set Sum of GRN Qty in Footer
                    Label lblTotalAccQty = (Label)e.Row.FindControl("lblTotalAccQty");
                    lblTotalAccQty.Text = sumFooterGRNQty.ToString();

                    //Set Sum of GST Amt in Footer
                    Label lblTotalGSTAmt = (Label)e.Row.FindControl("lblTotalGSTAmt");
                    lblTotalGSTAmt.Text = SumFooterGSTAmt.ToString();

                    //Set Sum of Ex Amt in Footer
                    Label lblTotalExAmt = (Label)e.Row.FindControl("lblTotalExAmt");
                    lblTotalExAmt.Text = SumFooterExAmt.ToString();

                    //Set Sum of Further Amt in Footer
                    Label lblTotalFurAmt = (Label)e.Row.FindControl("lblTotalFurAmt");
                    lblTotalFurAmt.Text = SumFooterFurAmt.ToString();

                    //Set Sum of Discount in Footer
                    Label lblTotalDiscount = (Label)e.Row.FindControl("lblTotalDiscount");
                    lblTotalDiscount.Text = SumFooterDiscount.ToString();

                    //Set Sum of Net Amount in Footer
                    Label lblFooterTotalAmt = (Label)e.Row.FindControl("lblFooterTotalAmt");
                    lblFooterTotalAmt.Text = SumFooterTotalAmt.ToString();


                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //TextBox GvtxtAccQty = (TextBox)e.Row.FindControl("txtAcceptedQty");
                    //if (GvtxtAccQty.Text.Trim() == string.Empty)
                    //{
                    //    SysFunc.UserMsg(LbErr, Color.Red, "Accepted Quantity Can Not be left blank", txtGRNNo);
                    //    return;
                    //}
                    //if (Convert.ToDecimal(GvtxtAccQty.Text.Trim()) == 0)
                    //{
                    //    SysFunc.UserMsg(LbErr, Color.Red, "Accepted Quantity Can Not be zero(0)", txtGRNNo);
                    //    return;
                    //}
                }
            }
            catch (Exception ex)
            {
                LbErr.Text = ex.Message;
            }
        }

        protected void ddlDCNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            GvMain.Columns[0].Visible = true; 
            Get_OPInfo();
            FillDCGV();
            totalCalculation();
        }

        protected void GvMain_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            decimal dQty=0 ,
                dDCQty = 0,
             dRate=0 ,
             dAmount=0,
             dGSTPerc=0,
             dFurTaxPerc=0,
             dExTaxPerc=0,
             dGSTAmount=0,
             dFurAmount=0,
             dExAmount=0,
             dAmountInclGST=0,
             dDiscount=0,
             dWHTaxAmount=0,
             dNetAmount=0;

                DataSet ds = new DataSet();
            try
            {
                
                DropDownList ddlEditLocation = (DropDownList)GvMain.Rows[e.RowIndex].FindControl("ddlLocation");
                Label lblItemCode = (Label)GvMain.Rows[e.RowIndex].FindControl("lblEditItemCode");
                TextBox txtQty = (TextBox)GvMain.Rows[e.RowIndex].FindControl("txtAcceptedQty");
                TextBox txtEditRate = (TextBox)GvMain.Rows[e.RowIndex].FindControl("txtEditRate");
                Label lblEditDCQty = (Label)GvMain.Rows[e.RowIndex].FindControl("lblEditDCQty");
                Label lblEditGSTage = (Label)GvMain.Rows[e.RowIndex].FindControl("lblEditGSTage");
                Label LbEditGSTAmount = (Label)GvMain.Rows[e.RowIndex].FindControl("LbEditGSTAmount");
                Label lblEditExTaxPerc = (Label)GvMain.Rows[e.RowIndex].FindControl("lblEditExTaxPerc");
                Label lblEditExTaxAmt = (Label)GvMain.Rows[e.RowIndex].FindControl("lblEditExTaxAmt");
                Label lblEditFurTaxPerc = (Label)GvMain.Rows[e.RowIndex].FindControl("lblEditFurTaxPerc");
                Label lblEditFurTaxAmt = (Label)GvMain.Rows[e.RowIndex].FindControl("lblEditFurTaxAmt");
                Label lblEditDiscount = (Label)GvMain.Rows[e.RowIndex].FindControl("lblEditDiscount");
                Label LbEditAmountMain = (Label)GvMain.Rows[e.RowIndex].FindControl("lblEditDiscount");
                
                dQty = Convert.ToDecimal(txtQty.Text.Trim() == "" ? "0" : txtQty.Text.Trim());
                dDCQty = Convert.ToDecimal(lblEditDCQty.Text.Trim() == "" ? "0" : lblEditDCQty.Text.Trim());
                dRate = Convert.ToDecimal(txtEditRate.Text.Trim() == "" ? "0" : txtEditRate.Text.Trim());
                dGSTPerc = Convert.ToDecimal(lblEditGSTage.Text.Trim() == "" ? "0" : lblEditGSTage.Text.Trim());
                dFurTaxPerc = Convert.ToDecimal(lblEditFurTaxPerc.Text.Trim() == "" ? "0" : lblEditFurTaxPerc.Text.Trim());
                dExTaxPerc = Convert.ToDecimal(lblEditExTaxPerc.Text.Trim() == "" ? "0" : lblEditExTaxPerc.Text.Trim());
                dDiscount = Convert.ToDecimal(lblEditDiscount.Text.Trim() == "" ? "0" : lblEditDiscount.Text.Trim());
                
                if (dQty <= 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "GRN Quantity not Zero(0).");
                    return;
                }
                if (dQty > dDCQty)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "GRN Quantity not greater then DC quantity.");
                    return;
                }
                if(ddlEditLocation.SelectedIndex==0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Select a location.");
                    return;
                }

                CalcNetAmount(ref dQty ,
             ref dRate,
             ref dAmount,
             ref dGSTPerc,
             ref dFurTaxPerc,
             ref dExTaxPerc,
             ref dGSTAmount,
             ref dFurAmount,
             ref dExAmount,
             ref dAmountInclGST,
             ref dDiscount,
             ref dWHTaxAmount,
             ref dNetAmount);
                
                ds = (DataSet)Session["dsDCGrid"];
                foreach (DataRow dr in ds.Tables[0].Rows) // search whole table
                {
                    if (dr["ItemCode"].ToString() == lblItemCode.Text) // if id==2
                    {
                        dr["AccQty"] = txtQty.Text; //change the name
                        dr["Rate"] = txtEditRate.Text; //change the name
                        dr["PartsTransLocCode"] = ddlEditLocation.SelectedValue.ToString().Trim();
                        dr["PartsTransLocDesc"] = ddlEditLocation.SelectedItem.Text;

                        //dr["NetAmount"] = decimal.Round(dAmount, 2).ToString();
                        dr["GSTAmount"] = decimal.Round(dGSTAmount, 2).ToString();
                        dr["PFGSTAmt"] = decimal.Round(dFurAmount, 2).ToString();
                        dr["ExTaxAmt"] = decimal.Round(dExAmount, 2).ToString();

                        dr["NetAmount"] = Convert.ToString(Decimal.Round(dNetAmount));
                        
                    }
                }
                GvMain.EditIndex = -1;
                GvMain.DataSource = ds.Tables[0];
                GvMain.DataBind();
                Session["dsDCGrid"] = ds;
                totalCalculation();
                
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }

        }
        private void CalcNetAmount(
        
            ref decimal dQty ,
            ref decimal dRate ,
            ref decimal dAmount,
            ref decimal dGSTPerc,
            ref decimal dFurTaxPerc,
            ref decimal dExTaxPerc,
            ref decimal dGSTAmount,
            ref decimal dFurAmount,
            ref decimal dExAmount,
            ref decimal dAmountInclGST,
            ref decimal dDiscount,
            ref decimal dWHTaxAmount,
            ref decimal dNetAmount
        )
            
    {
            try
            {
                //dQty = Convert.ToDecimal(txtQty.Text.Trim() == "" ? "0" : txtQty.Text.Trim());
                //dRate = Convert.ToDecimal(txtRate.Text.Trim() == "" ? "0" : txtRate.Text.Trim());
                //dGSTPerc = Convert.ToDecimal(txtGSTPercent.Text.Trim() == "" ? "0" : txtGSTPercent.Text.Trim());
                //dFurTaxPerc = Convert.ToDecimal(txtFurtherTaxPerc.Text.Trim() == "" ? "0" : txtFurtherTaxPerc.Text.Trim());
                //dExTaxPerc = Convert.ToDecimal(txtExTaxPerc.Text.Trim() == "" ? "0" : txtExTaxPerc.Text.Trim());

                ////dDiscount = Convert.ToDecimal(txtDiscount.Text.Trim() == "" ? "0" : txtDiscount.Text.Trim());
                ////dLiter = Convert.ToDecimal(txtLiter.Text.Trim() == "" ? "0" : txtLiter.Text.Trim());
                ////dWHTaxPerc = Convert.ToDecimal(txtWHTage.Text.Trim() == "" ? "0" : txtWHTage.Text.Trim());

                //Discount calculation
                //dDiscount = dRate * Convert.ToDecimal(ddlDiscount.SelectedItem.Value.ToString());
                dAmount = dQty * (dRate - dDiscount);
                dGSTAmount = dAmount * (dGSTPerc / 100);
                dFurAmount = dAmount * (dFurTaxPerc / 100);
                dExAmount = dAmount * (dExTaxPerc / 100);
                dAmountInclGST = dAmount + dGSTAmount + dFurAmount + dExAmount;


                //dWHTaxAmount = dAmountInclGST * (dWHTaxPerc / 100);
                dNetAmount = dAmountInclGST + dWHTaxAmount;
                //txtDiscount.Text = decimal.Round(dDiscount, 2).ToString();
                //txtAmount.Text = decimal.Round(dAmount, 2).ToString();
                //txtGSTAmount.Text = decimal.Round(dGSTAmount, 2).ToString();
                //txtFurtherTaxAmt.Text = decimal.Round(dFurAmount, 2).ToString();
                //txtExTaxAmt.Text = decimal.Round(dExAmount, 2).ToString();
                ////txtWHTAmount.Text = Convert.ToString(dWHTaxAmount);
                //txtTotalAmount.Text = Convert.ToString(Decimal.Round(dNetAmount));


            }
            catch (Exception ex)
            {
                //ObjGeneral.UserMsg(LbErr, Color.Red, ex.Message, txtPONo);
            }
        }

        protected void ddlGRNNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGRNNo.SelectedIndex == 0)
            {
                CtlClear();

            }
            else
            {
                lblMode.Text = "Edit Mode";
                ddlDCNo.SelectedIndex = 0;
                ddlDCNo.Enabled = false;
              //  GvMain.Columns[0].Visible = false;

                Get_GRNInfo();
                if (SysFunc.IsExist("PostFlag", "Y", "GRNMaster", Session["DealerCode"].ToString(), " and  GRNNo='" + ddlGRNNo.SelectedValue + "'"))
                {
                    btnPosting.Enabled = false;
                    btnPost.Enabled = false;
                    GvMain.Columns[0].Visible = false;
                }
                else
                {
                    btnPosting.Enabled = false;
                }
                FillGV();
                totalCalculation();
            }
        }

        protected void imgGRNLookup_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 38; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_GRNNumber(imgGRNLookup, ViewState["lookupid"].ToString(), "", "../../../");

            btnValue = 1;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void imgDCLookup_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 20; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_DCNumber(imgDCLookup, ViewState["lookupid"].ToString(), "", "../../../", "GRN");

            btnValue = 2;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }
        protected void SelectedPartDetail(string item)
        {
            try
            {
               
                if (btnValue == 2)
                {

                    ddlDCNo.SelectedValue = item.ToString();
                    ddlDCNo_SelectedIndexChanged(null, null);

                }
                else
                {
                    ddlGRNNo.SelectedValue = item;
                    ddlGRNNo_SelectedIndexChanged(null, null);
                }

            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblImp, Color.Red, ex.Message);
            }
        }

        protected void btnPost_Click(object sender, EventArgs e)

        {
            SqlTransaction Trans = null;
            try
            {
                if (ddlGRNNo.SelectedValue == "")
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Please Select GRN First");
                    return;
                }
                if (SysFunc.GetStringValuesAgainstCodes("GRNNo", ddlGRNNo.SelectedValue, "PostFlag", "GRNMaster", Session["DealerCode"].ToString()) == "Y")
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "This GRN is Already Posted!");
                    return;

                }
                bool check=false;
              
                if (ObjTrans.BeginTransaction(ref Trans) == false)
                { return; }
                else
                {
                    SqlParameter[] GRN_Delete = {
                    new SqlParameter("@DealerCode",SqlDbType.Char),//0
                    new SqlParameter("@GRNNo",SqlDbType.Char),//1
                };
                    GRN_Delete[0].Value = Session["DealerCode"].ToString();
                    GRN_Delete[1].Value = ddlGRNNo.SelectedValue;
                    if (SysFunc.ExecuteSP_NonQuery("SP_Delete_GRNDEtail", GRN_Delete, Trans))
                    {
                        check = true;

                    }
                    if (check == true)
                    {
                    for (int i = 0; i < GvMain.Rows.Count; i++)
                    {

                        SqlParameter[] param1 = {
                                             new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                             new SqlParameter("@GRNNo",SqlDbType.Char),//1
                                             new SqlParameter("@ItemCode",SqlDbType.Char),//2
                                             new SqlParameter("@PartsTransLocCode",SqlDbType.Char),//3
                                             new SqlParameter("@DCQty",SqlDbType.Float),//4
                                             new SqlParameter("@AccQty",SqlDbType.Float),//5
                                             new SqlParameter("@RejQty",SqlDbType.Float),//6
                                             new SqlParameter("@Rate",SqlDbType.Float),//7
                                             new SqlParameter("@Amount",SqlDbType.Float),//8
                                             new SqlParameter("@GSTPercent",SqlDbType.Float),//9
                                             new SqlParameter("@GSTAmount",SqlDbType.Float),//10
                                             new SqlParameter("@SEDage",SqlDbType.Float),//11
                                             new SqlParameter("@SEDAmount",SqlDbType.Float),//12
                                             new SqlParameter("@NetAmount",SqlDbType.Float),//13
                                             new SqlParameter("@DiscountAmt",SqlDbType.Float),//14
                                             new SqlParameter("@ExTaxPercent",SqlDbType.Float),//15
                                             new SqlParameter("@ExTaxAmt",SqlDbType.Float),//16
                                             new SqlParameter("@FurTaxPercent",SqlDbType.Float),//17
                                             new SqlParameter("@FurTaxAmt",SqlDbType.Float),//18
                                             new SqlParameter("@DiscountPerc",SqlDbType.Float)//19
                                          };

                        Label lblItemCode = (Label)GvMain.Rows[i].FindControl("lblItemCode");
                        Label lblLocationCode = (Label)GvMain.Rows[i].FindControl("lblLocationCode");
                        Label lblDCQty = (Label)GvMain.Rows[i].FindControl("lblDCQty");
                        Label lblAccQty = (Label)GvMain.Rows[i].FindControl("lblAccQty");
                        Label lblRate = (Label)GvMain.Rows[i].FindControl("lblRate");
                        Label LBGSTage = (Label)GvMain.Rows[i].FindControl("LBGSTage");
                        Label LbGSTAmount = (Label)GvMain.Rows[i].FindControl("LbGSTAmount");
                        Label lblExTaxPerc = (Label)GvMain.Rows[i].FindControl("lblExTaxPerc");
                        Label lblExTaxAmt = (Label)GvMain.Rows[i].FindControl("lblExTaxAmt");
                        Label lblFurTaxPerc = (Label)GvMain.Rows[i].FindControl("lblFurTaxPerc");
                        Label lblFurTaxAmt = (Label)GvMain.Rows[i].FindControl("lblFurTaxAmt");
                        Label lblDiscount = (Label)GvMain.Rows[i].FindControl("lblDiscount");
                        Label LbAmountMain = (Label)GvMain.Rows[i].FindControl("LbAmountMain");
                        Label lblDiscountPerc = (Label)GvMain.Rows[i].FindControl("lblDiscountPerc");
                        decimal dQty = 0, dRate, dDiscount = 0;
                        dQty = Convert.ToDecimal(lblAccQty.Text.Trim() == "" ? "0" : lblAccQty.Text.Trim());
                        dRate = Convert.ToDecimal(lblRate.Text.Trim() == "" ? "0" : lblRate.Text.Trim());
                        dDiscount = Convert.ToDecimal(lblDiscount.Text.Trim() == "" ? "0" : lblDiscount.Text.Trim());

                        param1[0].Value = Session["DealerCode"].ToString();
                        param1[1].Value = ddlGRNNo.SelectedValue;
                        param1[2].Value = lblItemCode.Text.Trim();
                        param1[3].Value = lblLocationCode.Text.Trim();
                        param1[4].Value = Convert.ToDecimal(lblDCQty.Text.Trim() == "" ? "0" : lblDCQty.Text.Trim());
                        param1[5].Value = dQty;
                        param1[6].Value = 0.0;
                        param1[7].Value = dRate;
                        param1[8].Value = (dRate - dDiscount) * dQty;
                        param1[9].Value = Convert.ToDecimal(LBGSTage.Text.Trim() == "" ? "0" : LBGSTage.Text.Trim());
                        param1[10].Value = Convert.ToDecimal(LbGSTAmount.Text.Trim() == "" ? "0" : LbGSTAmount.Text.Trim());
                        param1[11].Value = 0;
                        param1[12].Value = 0;
                        param1[13].Value = Convert.ToDecimal(LbAmountMain.Text.Trim() == "" ? "0" : LbAmountMain.Text.Trim());
                        param1[14].Value = dDiscount;
                        param1[15].Value = Convert.ToDecimal(lblExTaxPerc.Text.Trim() == "" ? "0" : lblExTaxPerc.Text.Trim());
                        param1[16].Value = Convert.ToDecimal(lblExTaxAmt.Text.Trim() == "" ? "0" : lblExTaxAmt.Text.Trim());
                        param1[17].Value = Convert.ToDecimal(lblFurTaxPerc.Text.Trim() == "" ? "0" : lblFurTaxPerc.Text.Trim());
                        param1[18].Value = Convert.ToDecimal(lblFurTaxAmt.Text.Trim() == "" ? "0" : lblFurTaxAmt.Text.Trim());
                        param1[19].Value = Convert.ToDecimal(lblDiscountPerc.Text.Trim() == "" ? "0" : lblDiscountPerc.Text.Trim());


                        if (SysFunc.ExecuteSP_NonQuery("SP_Spare_Insert_GRNDetail", param1, Trans))
                        {
                                check = true;

                        }
                        else
                            {
                                check = false;
                                break;
                            }

                    }

                }
                    if (check == true)
                    {
                        Label lblTotalAccQty = (Label)GvMain.FooterRow.FindControl("lblTotalAccQty");


                        //Set Sum of GST Amt in Footer
                        Label lblTotalGSTAmt = (Label)GvMain.FooterRow.FindControl("lblTotalGSTAmt");


                        //Set Sum of Ex Amt in Footer
                        Label lblTotalExAmt = (Label)GvMain.FooterRow.FindControl("lblTotalExAmt");


                        //Set Sum of Further Amt in Footer
                        Label lblTotalFurAmt = (Label)GvMain.FooterRow.FindControl("lblTotalFurAmt");


                        //Set Sum of Discount in Footer
                        Label lblTotalDiscount = (Label)GvMain.FooterRow.FindControl("lblTotalDiscount");


                        Label lblFooterTotalAmt = (Label)GvMain.FooterRow.FindControl("lblFooterTotalAmt");
                        string Sql = "Update GRNMaster Set PostFlag='Y',NetTotal='"+txtNetAmount.Text+"',TotalGSTAmount='"+lblTotalGSTAmt .Text+ "',FurtherTaxAmount='"+lblTotalFurAmt.Text+ "',ExtraTaxAmount='"+lblTotalExAmt.Text+ "',TotalDiscount='"+lblTotalDiscount.Text+"'  where GRNNo='" + ddlGRNNo.SelectedValue + "' and DealerCode='" + Session["DealerCode"].ToString() + "'";
                        if (SysFunc.ExecuteQuery_NonQuery(Sql, Trans))
                        {
                            ObjTrans.CommittTransaction(ref Trans);
                            SysFunc.UserMsg(LbErr, Color.Green, "Record Saved and post successfully. Last Code:" + ddlGRNNo.SelectedValue);
                            
                        }
                    }
                    else
                    {
                        ObjTrans.RollBackTransaction(ref Trans);
                        SysFunc.UserMsg(LbErr, Color.Red, "Record not save and post please try again");
                    }

                }
                CtlClear();
                LoadGRN_DDL();
                LoadDC_DDL();
            }
            catch(Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                SysFunc.UserMsg(LbErr, Color.Red, "Error: " + ex.Message);
            }

            }
        }
}