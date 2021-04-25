using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace DXBMS.Modules.Service
{
    public partial class CustomerInvoice : System.Web.UI.Page
    {
        SysFunctions myFunc = new SysFunctions();

        MainBLL objMBLL = new MainBLL();
        ServiceBL objSBL = new ServiceBL();
        SysFunctions grl = new SysFunctions();
        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;
        //clsLookUp clslook = new clsLookUp();
        DataSet dsJobCardMaster, dsJobCardDetail, dsJobCardParts,dsJobCardConParts, dsJobCardLub, dsJobCardSublet, ds, dsCustomerInvoice;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/Main/login.aspx");

            }
            
            if (!Page.IsPostBack)
            {

                txtInvoiceDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                objMBLL.FillDrp_SP(ddlInsCo, "sp_2W_InsuranceCompanies_select", "InsCompCode", "InsCompDescription", null, true, "--Select--", false, "");
                Load_ddlInvoiceNo();
                Load_ddlJobCardCode();
                Load_ddlCusCode();
            }


            //if (Session["LookUpData"] != null)
            //{
            //    HFBlank.Value = null;
            //    HFRegNo.Value = null;
            //    HFEngineNo.Value = null;
            //    HFChassisNo.Value = null;
            //    HFCustomer.Value = null;
            //    HFJobCard.Value = null;
            //    lookUptext(ViewState["lookupid"].ToString());

            //    if (ViewState["lookupid"].ToString() == "4")
            //    {
            //        LoadMasterData("JobCard");

            //    }
            //    if (ViewState["lookupid"].ToString() == "32")
            //    {
            //        LoadMasterData("InvoiceNo");
            //    }
            //    Calculate_NetAmountForCustomer();
            //}



            ////Commented by akram
            //Calculate_GSTAmount_On_Percentage();
            ///////////////////////////
            // Calculate_LaborDiscountAmount_On_Percentage();
            //Calculate_LaborDiscountPercentage_On_Amount();

            //Calculate_PartDiscountAmount_On_Percentage();
            // Calculate_PartDiscountPercentage_On_Amount();
            //////Commented by akram
            //Calculate_SubTotalAmount();
            //////////////////////
        }
        private void Load_ddlInvoiceNo()
        {
            string WhereClause = " CI.DealerCode=JC.DealerCode " +
                 "And CI.JobCardCode = JC.JobCardCode " +
 "  AND CI.DealerCode = '" + Session["DealerCode"].ToString() + "'" +
 "And CI.Delflag<>'Y' ";
            string[] Columns = new string[] { "CI.InvoiceNo", "CONVERT(VARCHAR(10),CI.InvoiceDate,105)", "CI.JobCardCode", "JC.JobCardType" };
            myFunc.GetMultiColumnsDDL(ddlInvoiceNo, Columns, "CustomerInvoice CI ,  JobCardMaster  JC ", WhereClause, "InvoiceNo", " Order by CI.InvoiceNo Desc ", false, false);
        }
        private void Load_ddlCusCode()
        {
            string WhereClause = " DealerCode = '" + Session["DealerCode"].ToString() + "'" ;
 
            string[] Columns = new string[] { "cusCode","CusDesc+' '+FatherHusName", "Address1+' '+Address2+' '+Address3" ,"isnull(phone1,isnull(phone2,CellNo))" };
            myFunc.GetMultiColumnsDDL(ddlCustCode, Columns, "Customer ", WhereClause, "cusCode", " Order by CusCode Desc ", false, false);
        }
        private void Load_ddlJobCardCode()
        {
            if (ddlInvoiceNo.SelectedIndex == 0)
            {
                string WhereClause = "DealerCode ='" + Session["DealerCode"].ToString().Trim() + "'  And  Gatepass='' " +
            "And DelFlag='N' and Jobtypecode <> '001' " +
                    "and JobTypeCode <> '002' " +
                    "and JobCardType <> 'Warranty' " +
                    "and JobCardType <> 'Repeated JobCard' ";
                string[] Columns = new string[] { "JobCardCode", "RegNo", "UserName" };
                myFunc.GetMultiColumnsDDL(ddlJobCardCode, Columns, "JobCardMaster ", WhereClause, "JobCardCode", " order by JobCardCode DESC ", false, false);
            }
            else
            {
                string WhereClause = "DealerCode ='" + Session["DealerCode"].ToString().Trim() + "'  " +
            " And JobCardCode='" + HFJobCard.Value.Trim() + "'";
                string[] Columns = new string[] { "JobCardCode", "RegNo", "UserName" };
                myFunc.GetMultiColumnsDDL(ddlJobCardCode, Columns, "JobCardMaster ", WhereClause, "JobCardCode", " order by JobCardCode DESC ", false, false);
            }
        }
        
        private void InsuranceInvoiceEntryUpdate()
        {
            #region JobCard Master Param
            SqlParameter[] INV_param = {         
            /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),         /*1*/ new SqlParameter("@InvoiceNo",SqlDbType.Char,8),
           /*2*/ new SqlParameter("@InvoiceDate",SqlDbType.DateTime),       /*3*/ new SqlParameter("@JobCardCode",SqlDbType.Char,8),
           /*4*/ new SqlParameter("@CusCode",SqlDbType. Char,8),            /*5*/ new SqlParameter("@GrossAmount",SqlDbType.Decimal),
           /*6*/ new SqlParameter("@GSTPer",SqlDbType.Decimal),             /*7*/ new SqlParameter("@GSTAmount",SqlDbType.Decimal),
           /*8*/ new SqlParameter("@SubTotal",SqlDbType.Decimal),           /*9*/ new SqlParameter("@DiscountAmount",SqlDbType.Decimal),
           /*10*/ new SqlParameter("@DiscountPer",SqlDbType.VarChar,8),     /*11*/ new SqlParameter("@NetAmountCustomer",SqlDbType.Decimal),
           /*12*/ new SqlParameter("@NetAmountInsurance",SqlDbType.Decimal), /*13*/ new SqlParameter("@InsCode",SqlDbType. Char,4),
           /*14*/ new SqlParameter("@BranchCode",SqlDbType. Char,4),           /*15*/ new SqlParameter("@Gatepass",SqlDbType. Char,8),
           /*16*/ new SqlParameter("@PaidC",SqlDbType. Decimal),           /*17*/ new SqlParameter("@PaidI",SqlDbType. Decimal),
           /*18*/ new SqlParameter("@VOD",SqlDbType.Decimal),           /*19*/ new SqlParameter("@TotalDep",SqlDbType. Decimal),
           /*20*/ new SqlParameter("@SaleInvoice",SqlDbType.VarChar,50),           /*21*/ new SqlParameter("@DepSaleInvoice",SqlDbType.VarChar,50),
           /*22*/ new SqlParameter("@InvType",SqlDbType. Char,3),           /*23*/ new SqlParameter("@VoucherNo",SqlDbType. VarChar,50),
           /*24*/ new SqlParameter("@VoucherFlag",SqlDbType.Char,1),           /*25*/ new SqlParameter("@DelFlag ",SqlDbType.Char,1),
           /*26*/ new SqlParameter("@UpdUser",SqlDbType. VarChar,50),           /*27*/ new SqlParameter("@UpdDate",SqlDbType.DateTime),
           /*28*/ new SqlParameter("@UpdTime",SqlDbType. DateTime),           /*29*/ new SqlParameter("@UpdTerm",SqlDbType. VarChar,50),
           /*30*/ new SqlParameter("@PartsDiscount",SqlDbType. Decimal),           /*31*/ new SqlParameter("@PartsDiscountPer",SqlDbType.Decimal),
           /*32*/ new SqlParameter("@OtherDiscount",SqlDbType.Decimal),           /*33*/ new SqlParameter("@OtherDiscountPer",SqlDbType.Decimal),
           /*34*/ new SqlParameter("@OutKM",SqlDbType.VarChar,50),              /*35*/ new SqlParameter("@PSTPer",SqlDbType.Decimal), 
           /*36*/ new SqlParameter("@PSTAmount",SqlDbType.Decimal)
         };


            INV_param[0].Value = Session["DealerCode"].ToString();
            //INV_param[1].Value = Decission will take on Insert/U.pdate
            INV_param[2].Value = grl.SaveDate(txtInvoiceDate.Text); INV_param[3].Value = ddlJobCardCode.SelectedValue.ToString().Trim();
            INV_param[4].Value = ddlCustCode.SelectedValue.ToString().Trim(); INV_param[5].Value = txtGrossAmount.Text;
            INV_param[6].Value = txtGSTPercent.Text == "" ? "0" : txtGSTPercent.Text; INV_param[7].Value = txtGSTAmount.Text;
            INV_param[8].Value = txtSubTotal.Text;
            INV_param[9].Value = txtDiscLabor.Text == "" ? "0" : txtDiscLabor.Text.Trim(); //Value not found
            INV_param[10].Value = txtDiscLaborPercent.Text == "" ? "0" : txtDiscLaborPercent.Text.Trim(); //Value not found
            INV_param[11].Value = "0.00"; INV_param[12].Value = txtTotAmtInsComp.Text;
            INV_param[13].Value = ddlInsCo.SelectedValue.ToString(); INV_param[14].Value = ddlInsBranch.SelectedValue.ToString();
            INV_param[15].Value = ddlJobCardCode.SelectedValue.ToString().Trim(); //Value not found
            INV_param[16].Value = txtPaidCustomer.Text; INV_param[17].Value = txtPaidInsurance.Text;
            INV_param[18].Value = txtVEODAmount.Text; INV_param[19].Value = txtDepAmount.Text;
            INV_param[20].Value = ""; //Value not found
            INV_param[21].Value = ""; //Value not found
            INV_param[22].Value = "Ins"; //Value not found
            INV_param[23].Value = ""; //Value not found
            INV_param[24].Value = "N"; //Value not found
            INV_param[25].Value = "N";
            INV_param[26].Value = Session["UserName"].ToString();
            INV_param[27].Value = grl.SaveDate(DateTime.Now.ToString("dd/MM/yyyy")); INV_param[28].Value = grl.SaveTime(DateTime.Now.ToString("HH:mm"));
            INV_param[29].Value = Environment.MachineName; 
            INV_param[30].Value = (txtDiscountParts.Text.Trim() == "" ? "0" : txtDiscountParts.Text.Trim());
            INV_param[31].Value = (txtDiscPercentPart.Text.Trim() == "" ? "0" : txtDiscPercentPart.Text.Trim());
            INV_param[32].Value = txtDistOtherParts.Text;
            INV_param[33].Value = txtDistOtherPercent.Text; INV_param[34].Value = txtOutKM.Text;
            INV_param[35].Value = txtPSTPercent.Text == "" ? "0" : txtPSTPercent.Text;
            INV_param[36].Value = txtPSTAmount.Text;
            #endregion
            try
            {
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {

                    INV_param[1].Value = ddlInvoiceNo.SelectedValue.ToString().Trim();
                    if (myFunc.ExecuteSP_NonQuery("[sp_W2_CustomerInvoice_Update]", INV_param, Trans))
                    {
                        ObjTrans.CommittTransaction(ref Trans);
                    }
                }
            }
            catch (Exception)
            {
                ObjTrans.RollBackTransaction(ref Trans);
            }
        }
        private void InsuranceInvoiceEntry()
        {
            #region JobCard Master Param
            SqlParameter[] INV_param = {         
            /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),         /*1*/ new SqlParameter("@InvoiceNo",SqlDbType.Char,8),
           /*2*/ new SqlParameter("@InvoiceDate",SqlDbType.DateTime),       /*3*/ new SqlParameter("@JobCardCode",SqlDbType.Char,8),
           /*4*/ new SqlParameter("@CusCode",SqlDbType. Char,8),            /*5*/ new SqlParameter("@GrossAmount",SqlDbType.Decimal),
           /*6*/ new SqlParameter("@GSTPer",SqlDbType.Decimal),             /*7*/ new SqlParameter("@GSTAmount",SqlDbType.Decimal),
           /*8*/ new SqlParameter("@SubTotal",SqlDbType.Decimal),           /*9*/ new SqlParameter("@DiscountAmount",SqlDbType.Decimal),
           /*10*/ new SqlParameter("@DiscountPer",SqlDbType.VarChar,8),     /*11*/ new SqlParameter("@NetAmountCustomer",SqlDbType.Decimal),
           /*12*/ new SqlParameter("@NetAmountInsurance",SqlDbType.Decimal), /*13*/ new SqlParameter("@InsCode",SqlDbType. Char,4),
           /*14*/ new SqlParameter("@BranchCode",SqlDbType. Char,4),           /*15*/ new SqlParameter("@Gatepass",SqlDbType. Char,8),
           /*16*/ new SqlParameter("@PaidC",SqlDbType. Decimal),           /*17*/ new SqlParameter("@PaidI",SqlDbType. Decimal),
           /*18*/ new SqlParameter("@VOD",SqlDbType.Decimal),           /*19*/ new SqlParameter("@TotalDep",SqlDbType. Decimal),
           /*20*/ new SqlParameter("@SaleInvoice",SqlDbType.VarChar,50),           /*21*/ new SqlParameter("@DepSaleInvoice",SqlDbType.VarChar,50),
           /*22*/ new SqlParameter("@InvType",SqlDbType. Char,3),           /*23*/ new SqlParameter("@VoucherNo",SqlDbType. VarChar,50),
           /*24*/ new SqlParameter("@VoucherFlag",SqlDbType.Char,1),           /*25*/ new SqlParameter("@DelFlag ",SqlDbType.Char,1),
           /*26*/ new SqlParameter("@UpdUser",SqlDbType. VarChar,50),           /*27*/ new SqlParameter("@UpdDate",SqlDbType.DateTime),
           /*28*/ new SqlParameter("@UpdTime",SqlDbType. DateTime),           /*29*/ new SqlParameter("@UpdTerm",SqlDbType. VarChar,50),
           /*30*/ new SqlParameter("@PartsDiscount",SqlDbType. Decimal),           /*31*/ new SqlParameter("@PartsDiscountPer",SqlDbType.Decimal),
           /*32*/ new SqlParameter("@OtherDiscount",SqlDbType.Decimal),           /*33*/ new SqlParameter("@OtherDiscountPer",SqlDbType.Decimal),
           /*34*/ new SqlParameter("@OutKM",SqlDbType.VarChar,50),              /*35*/ new SqlParameter("@PSTPer",SqlDbType.Decimal), 
           /*36*/ new SqlParameter("@PSTAmount",SqlDbType.Decimal)
         };


            INV_param[0].Value = Session["DealerCode"].ToString();
            //INV_param[1].Value = Decission will take on Insert/U.pdate
            INV_param[2].Value = grl.SaveDate(txtInvoiceDate.Text); INV_param[3].Value = ddlJobCardCode.SelectedValue.ToString().Trim();
            INV_param[4].Value = ddlCustCode.SelectedValue.ToString().Trim(); INV_param[5].Value = txtGrossAmount.Text;
            INV_param[6].Value = txtGSTPercent.Text == "" ? "0" : txtGSTPercent.Text; INV_param[7].Value = txtGSTAmount.Text;
            INV_param[8].Value = txtSubTotal.Text;
            INV_param[9].Value = txtDiscLabor.Text == "" ? "0" : txtDiscLabor.Text.Trim(); //Value not found
            INV_param[10].Value = txtDiscLaborPercent.Text == "" ? "0" : txtDiscLaborPercent.Text.Trim(); //Value not found
            INV_param[11].Value = "0.00"; INV_param[12].Value = txtTotAmtInsComp.Text;
            INV_param[13].Value = ddlInsCo.SelectedValue.ToString(); INV_param[14].Value = ddlInsBranch.SelectedValue.ToString();
            INV_param[15].Value = ddlJobCardCode.SelectedValue.ToString().Trim(); //Value not found
            INV_param[16].Value = txtPaidCustomer.Text; INV_param[17].Value = txtPaidInsurance.Text;
            INV_param[18].Value = txtVEODAmount.Text; INV_param[19].Value = txtDepAmount.Text;
            INV_param[20].Value = ""; //Value not found
            INV_param[21].Value = ""; //Value not found
            INV_param[22].Value = "Ins"; //Value not found
            INV_param[23].Value = ""; //Value not found
            INV_param[24].Value = "N"; //Value not found
            INV_param[25].Value = "N";
            INV_param[26].Value = Session["UserName"].ToString();
            INV_param[27].Value = grl.SaveDate(DateTime.Now.ToString("dd/MM/yyyy")); INV_param[28].Value = grl.SaveTime(DateTime.Now.ToString("HH:mm"));
            INV_param[29].Value = Environment.MachineName; 
            INV_param[30].Value = (txtDiscountParts.Text.Trim() == "" ? "0" : txtDiscountParts.Text.Trim());
            INV_param[31].Value = (txtDiscPercentPart.Text.Trim() == "" ? "0" : txtDiscPercentPart.Text.Trim());
            INV_param[32].Value = txtDistOtherParts.Text;
            INV_param[33].Value = txtDistOtherPercent.Text; INV_param[34].Value = txtOutKM.Text;
            INV_param[35].Value = txtPSTPercent.Text == "" ? "0" : txtPSTPercent.Text;
            INV_param[36].Value = txtPSTAmount.Text;
            #endregion
            try
            {
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    string IQuery = "";
                    string strAutoInvoiceNo = string.Empty;
                    //string Decission = IsExist();
                    //if (Decission == "Insert")
                    //{
                    strAutoInvoiceNo = grl.AutoGen("UniqueInvoiceNo", "InvoiceNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                    INV_param[1].Value = strAutoInvoiceNo;
                    if (myFunc.ExecuteSP_NonQuery("[sp_W2_CustomerInvoice_Insert]", INV_param, Trans))
                    {
                        IQuery = "Update JobCardMaster set GatePass ='" + ddlJobCardCode.SelectedValue.ToString().Trim() +"'," +
                                 "DelvDate='" + grl.SaveDate(DateTime.Now.ToString("dd-MM-yyyy")) + "',DelvTime='" + grl.SaveTime(DateTime.Now.ToString("HH:mm")) + "' " +
                                 "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + ddlJobCardCode.SelectedValue.ToString().Trim() +"'";
                        //////////////////Generate MDN/////////////
                        //if (Create_NDM(txtJobCardNo.Text, "InvoiceCreated") == false)
                        //{
                        //    grl.UserMsg(lblMsg, Color.Red, "NDM Data Can not be inserted");
                        //    ObjTrans.RollBackTransaction(ref Trans);
                        //    return;
                        //}
                        ///////////////////////////
                        if (myFunc.ExecuteQuery(IQuery, Trans))
                        {
                            ///Update GST Invoice Table
                            if (double.Parse(txtGSTAmount.Text.Trim()) != 0)
                            {
                                SqlParameter[] GSTINV_param = { new SqlParameter("@DealerCode",SqlDbType.Char,5),new SqlParameter("@GSTInvNo",SqlDbType.Char,8),
                                                                new SqlParameter("@GSTInvDate",SqlDbType.DateTime),new SqlParameter("@RefNo",SqlDbType.Char,8) };
                                string GSTInvNo = grl.AutoGen("UniqueSalesTaxInvoice", "SalesTaxInvNO", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                                GSTINV_param[0].Value = Session["DealerCode"].ToString(); GSTINV_param[1].Value = GSTInvNo;
                                GSTINV_param[2].Value = grl.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
                                GSTINV_param[3].Value = strAutoInvoiceNo;
                                if (myFunc.ExecuteSP_NonQuery("[sp_W2_GSTInvoice_Insert]", GSTINV_param, Trans))
                                {
                                    IQuery = "Update CustomerInvoice set SaleInvoice='" + GSTInvNo + "' " +
                                             "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + ddlJobCardCode.SelectedValue.ToString().Trim() +"'";
                                    myFunc.ExecuteQuery(IQuery, Trans);
                                }
                            }

                            /////Update PST Invoice Table
                            if (grl.CodeExists("PSTInvoice", "RefNo", ddlJobCardCode.SelectedValue.ToString().Trim()) == false)
                            {

                                if (double.Parse(txtPSTAmount.Text.Trim()) != 0)
                                {
                                    SqlParameter[] PSTINV_param = { new SqlParameter("@DealerCode",SqlDbType.Char,5),new SqlParameter("@PSTInvNo",SqlDbType.Char,8),
                                                                new SqlParameter("@PSTInvDate",SqlDbType.DateTime),new SqlParameter("@RefNo",SqlDbType.Char,8) };
                                    string PSTInvNo = grl.AutoGen("PSTUniqueSalesTaxInvoice", "SSTSalesTaxInvNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                                    PSTINV_param[0].Value = Session["DealerCode"].ToString(); PSTINV_param[1].Value = PSTInvNo;
                                    PSTINV_param[2].Value = grl.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
                                    PSTINV_param[3].Value = strAutoInvoiceNo;
                                    if (myFunc.ExecuteSP_NonQuery("[sp_W2_PSTInvoice_Insert]", PSTINV_param, Trans))
                                    {
                                        IQuery = "Update CustomerInvoice set SSTSaleInvoice='" + PSTInvNo + "' " +
                                                 "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + ddlJobCardCode.SelectedValue.ToString().Trim() +"'";
                                        myFunc.ExecuteQuery(IQuery, Trans);
                                    }
                                }

                            }
                        }
                    }
                    else { 
                        //objMBLL.ShowMessageBox("Record not saved please try again"); 
                        myFunc.UserMsg(lblMsg, Color.Red, "Record not saved please try again");
                    }
                    //}
                    //else if (Decission == "Update")
                    //{
                    //    INV_param[1].Value = txtInvoiceNo.Text;

                    //    if (myFunc.ExecuteSP_NonQuery("[sp_W2_CustomerInvoice_Update]", INV_param, Trans))
                    //    {

                    //        if (grl.GetStringValuesAgainstCodes("InvoiceNo", txtInvoiceNo.Text.Trim(), "SaleInvoice", "CustomerInvoice") == "")
                    //        {
                    //            ///Update GST Invoice Table
                    //            if (double.Parse(txtGSTAmount.Text.Trim()) != 0)
                    //            {
                    //                SqlParameter[] GSTINV_param = { new SqlParameter("@DealerCode",SqlDbType.Char,5),new SqlParameter("@GSTInvNo",SqlDbType.Char,8),
                    //                                                new SqlParameter("@GSTInvDate",SqlDbType.DateTime),new SqlParameter("@RefNo",SqlDbType.Char,8) };
                    //                string GSTInvNo = grl.AutoGen("UniqueSalesTaxInvoice", "SalesTaxInvNO", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                    //                GSTINV_param[0].Value = Session["DealerCode"].ToString(); GSTINV_param[1].Value = GSTInvNo;
                    //                GSTINV_param[2].Value = grl.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
                    //                GSTINV_param[3].Value = txtInvoiceNo.Text.Trim();
                    //                if (myFunc.ExecuteSP_NonQuery("[sp_W2_GSTInvoice_Insert]", GSTINV_param, Trans))
                    //                {
                    //                    IQuery = "Update CustomerInvoice set SaleInvoice='" + GSTInvNo + " " +
                    //                             "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + txtJobCardNo.Text.Trim() + "'";
                    //                    myFunc.ExecuteQuery(IQuery, Trans);
                    //                }
                    //            }
                    //        }

                    //        if (grl.GetStringValuesAgainstCodes("InvoiceNo", txtInvoiceNo.Text.Trim(), "SSTSaleInvoice", "CustomerInvoice") == "")
                    //        {
                    //            ///Update PST Invoice Table
                    //            if (double.Parse(txtPSTAmount.Text.Trim()) != 0)
                    //            {
                    //                SqlParameter[] PSTINV_param = { new SqlParameter("@DealerCode",SqlDbType.Char,5),new SqlParameter("@PSTInvNo",SqlDbType.Char,8),
                    //                                                new SqlParameter("@PSTInvDate",SqlDbType.DateTime),new SqlParameter("@RefNo",SqlDbType.Char,8) };
                    //                string PSTInvNo = grl.AutoGen("PSTUniqueSalesTaxInvoice", "SSTSalesTaxInvNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                    //                PSTINV_param[0].Value = Session["DealerCode"].ToString(); PSTINV_param[1].Value = PSTInvNo;
                    //                PSTINV_param[2].Value = grl.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
                    //                PSTINV_param[3].Value = txtInvoiceNo.Text.Trim();
                    //                if (myFunc.ExecuteSP_NonQuery("[sp_W2_PSTInvoice_Insert]", PSTINV_param, Trans))
                    //                {
                    //                    IQuery = "Update CustomerInvoice set SSTSaleInvoice='" + PSTInvNo + "' " +
                    //                             "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + txtJobCardNo.Text.Trim() + "'";
                    //                    myFunc.ExecuteQuery(IQuery, Trans);
                    //                }
                    //            }
                    //        }
                    //        //IQuery = "Update JobCardMaster set SIRMaster='" + txtInvoiceNo.Text + "', GatePass ='" + txtInvoiceNo.Text + "',DelvDate='" + grl.SaveDate(DateTime.Now.ToString("yyyy/MM/dd")) + "',DelvTime='" + grl.SaveTime(DateTime.Now.ToString("HH:mm")) + "' Where DealerCode='" + Session["DealerCode"].ToString() + "'" +
                    //        //        " and JobCardCode='" + txtJobCardNo.Text + "'";
                    //        //if (myFunc.ExecuteQuery(IQuery, Trans))
                    //        //{

                    //        //}       

                    //    }
                    //    else { objMBLL.ShowMessageBox(SessionInformation.Update_Fail_Message, txtColorName); }
                    //}
                    ObjTrans.CommittTransaction(ref Trans);
                    //hdPrint.Value = "";
                    //this.ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ConfirmApproval();", true);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "return confirm('confirm?');", true);
                    // Page.ClientScript.RegisterStartupScript(this.GetType(), "myClick", "window.open('../../Service/ServiceReports/InvoiceNormalView.aspx?JobCardCode=" + txtJobCardNo.Text.Trim() + "&GSTAmt=" + txtGSTAmount.Text + "&SSTAmt=" + txtPSTAmount.Text + "','_blank'," + General.PopUpSize + ");", true);

                    //lblMsg.Visible = true;
                    ///grl.UserMsg(lblMsg, Color.Green, "Record Saved Successfully: " + txtInvoiceNo.Text, txtInvoiceNo);
                    myFunc.UserMsg(lblMsg, Color.Green, "Record Saveed Successfully: " + strAutoInvoiceNo);
                    //lblMSGPop.Text = "Record Saveed Successfully: " + txtInvoiceNo.Text;
                    //PopupControlMSG.ShowOnPageLoad = true;
                }
            }
            catch (Exception)
            {
                ObjTrans.RollBackTransaction(ref Trans);
            }
            //try
            //{
            //    if (ObjTrans.BeginTransaction(ref Trans) == true)
            //    {
            //        txtInvoiceNo.Text = grl.AutoGen("UniqueInvoiceNo", "InvoiceNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
            //        INV_param[1].Value = txtInvoiceNo.Text;
            //        if (myFunc.ExecuteSP_NonQuery("[sp_W2_CustomerInvoice_Insert]", INV_param, Trans))
            //        {
            //            ObjTrans.CommittTransaction(ref Trans);
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    ObjTrans.RollBackTransaction(ref Trans);
            //}
        }
        private void InvoiceEntry()
        {
            #region JobCard Master Param
            SqlParameter[] INV_param = {         
            /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),         /*1*/ new SqlParameter("@InvoiceNo",SqlDbType.Char,8),
           /*2*/ new SqlParameter("@InvoiceDate",SqlDbType.DateTime),       /*3*/ new SqlParameter("@JobCardCode",SqlDbType.Char,8),
           /*4*/ new SqlParameter("@CusCode",SqlDbType. Char,8),            /*5*/ new SqlParameter("@GrossAmount",SqlDbType.Decimal),
           /*6*/ new SqlParameter("@GSTPer",SqlDbType.Decimal),             /*7*/ new SqlParameter("@GSTAmount",SqlDbType.Decimal),
           /*8*/ new SqlParameter("@SubTotal",SqlDbType.Decimal),           /*9*/ new SqlParameter("@DiscountAmount",SqlDbType.Decimal),
           /*10*/ new SqlParameter("@DiscountPer",SqlDbType.VarChar,8),     /*11*/ new SqlParameter("@NetAmountCustomer",SqlDbType.Decimal),
           /*12*/ new SqlParameter("@NetAmountInsurance",SqlDbType.Decimal), /*13*/ new SqlParameter("@InsCode",SqlDbType. Char,4),
           /*14*/ new SqlParameter("@BranchCode",SqlDbType. Char,4),           /*15*/ new SqlParameter("@Gatepass",SqlDbType. Char,8),
           /*16*/ new SqlParameter("@PaidC",SqlDbType. Decimal),           /*17*/ new SqlParameter("@PaidI",SqlDbType. Decimal),
           /*18*/ new SqlParameter("@VOD",SqlDbType.Decimal),           /*19*/ new SqlParameter("@TotalDep",SqlDbType. Decimal),
           /*20*/ new SqlParameter("@SaleInvoice",SqlDbType.VarChar,50),           /*21*/ new SqlParameter("@DepSaleInvoice",SqlDbType.VarChar,50),
           /*22*/ new SqlParameter("@InvType",SqlDbType. Char,3),           /*23*/ new SqlParameter("@VoucherNo",SqlDbType. VarChar,50),
           /*24*/ new SqlParameter("@VoucherFlag",SqlDbType.Char,1),           /*25*/ new SqlParameter("@DelFlag ",SqlDbType.Char,1),
           /*26*/ new SqlParameter("@UpdUser",SqlDbType. VarChar,50),           /*27*/ new SqlParameter("@UpdDate",SqlDbType.DateTime),
           /*28*/ new SqlParameter("@UpdTime",SqlDbType. DateTime),           /*29*/ new SqlParameter("@UpdTerm",SqlDbType. VarChar,50),
           /*30*/ new SqlParameter("@PartsDiscount",SqlDbType. Decimal),           /*31*/ new SqlParameter("@PartsDiscountPer",SqlDbType.Decimal),
           /*32*/ new SqlParameter("@OtherDiscount",SqlDbType.Decimal),           /*33*/ new SqlParameter("@OtherDiscountPer",SqlDbType.Decimal),
           /*34*/ new SqlParameter("@OutKM",SqlDbType.VarChar,50),              /*35*/ new SqlParameter("@PSTPer",SqlDbType.Decimal),
           /*36*/ new SqlParameter("@PSTAmount",SqlDbType.Decimal),              /*37*/ new SqlParameter("@TaxType",SqlDbType.VarChar,10),
           /*38*/ new SqlParameter("@TRNumber",SqlDbType.Char,15),            /*39*/ new SqlParameter("@TRDate",SqlDbType.DateTime)
         };


            INV_param[0].Value = Session["DealerCode"].ToString();
            //INV_param[1].Value = Decission will take on Insert/U.pdate
            INV_param[2].Value = grl.SaveDate(txtInvoiceDate.Text); INV_param[3].Value = HFJobCard.Value.Trim();
            INV_param[4].Value = ddlCustCode .SelectedValue.ToString().Trim(); INV_param[5].Value = txtGrossAmount.Text;
            INV_param[6].Value = txtGSTPercent.Text == "" ? "0" : txtGSTPercent.Text; INV_param[7].Value = txtGSTAmount.Text;
            INV_param[8].Value = txtSubTotal.Text;
            INV_param[9].Value = txtDiscLabor.Text == "" ? "0" : txtDiscLabor.Text.Trim(); //Value not found
            INV_param[10].Value = txtDiscLaborPercent.Text == "" ? "0" : txtDiscLaborPercent.Text.Trim(); //Value not found
            INV_param[11].Value = txtTotalAmtCustomer.Text; INV_param[12].Value = txtTotalAmntInsurance.Text;
            INV_param[13].Value = ddlInsCo.SelectedValue.ToString().Trim(); INV_param[14].Value = ddlInsBranch.SelectedValue.ToString().Trim();
            INV_param[15].Value = ddlJobCardCode.SelectedValue.ToString().Trim(); //Value not found
            INV_param[16].Value = txtPaidCustomer.Text; INV_param[17].Value = txtPaidInsurance.Text;
            INV_param[18].Value = txtVEODAmount.Text; INV_param[19].Value = txtDipriAmount.Text;
            INV_param[20].Value = ""; //Value not found
            INV_param[21].Value = ""; //Value not found
            if (txtJobCardType.Text.Trim() == "Insurance")
            {
                INV_param[22].Value = "Dep"; //Value not found
            }
            else
            {
                INV_param[22].Value = "Inv";
            }
            INV_param[23].Value = ""; //Value not found
            INV_param[24].Value = "N"; //Value not found
            INV_param[25].Value = "N";
            INV_param[26].Value = Session["UserName"].ToString();
            INV_param[27].Value = grl.SaveDate(DateTime.Now.ToString("dd/MM/yyyy")); INV_param[28].Value = grl.SaveTime(DateTime.Now.ToString("HH:mm"));
            INV_param[29].Value = Environment.MachineName;
            INV_param[30].Value = (txtDiscountParts.Text.Trim() == "" ? "0" : txtDiscountParts.Text.Trim());
            INV_param[31].Value = (txtDiscPercentPart.Text.Trim() == "" ? "0" : txtDiscPercentPart.Text.Trim());
            INV_param[32].Value = txtDistOtherParts.Text;
            INV_param[33].Value = txtDistOtherPercent.Text; INV_param[34].Value = txtOutKM.Text;
            INV_param[35].Value = txtPSTPercent.Text == "" ? "0" : txtPSTPercent.Text;
            INV_param[36].Value = txtPSTAmount.Text;
            INV_param[37].Value = ddlTaxType.SelectedValue.ToString();
            INV_param[38].Value = txtTRNumber.Text;
            INV_param[39].Value = txtTRDate.Text.Trim() == "" ? (Object)DBNull.Value : myFunc.SaveDate(txtTRDate.Text);
            //INV_param[37].Value = txtPolicyNo.Text;
            #endregion
            try
            {
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    string IQuery = ""; 
                    string Decission = IsExist();
                    if (Decission == "Insert")
                    {
                        string NewInvNo = "";
                        NewInvNo = grl.AutoGen("UniqueInvoiceNo", "InvoiceNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                        INV_param[1].Value = NewInvNo;
                        if (myFunc.ExecuteSP_NonQuery("[sp_W2_CustomerInvoice_Insert]", INV_param, Trans))
                        {
                            IQuery = "Update JobCardMaster set GatePass ='" + ddlJobCardCode.SelectedValue.ToString().Trim() +"'," +
                                     "DelvDate='" + grl.SaveDate(DateTime.Now.ToString("dd-MM-yyyy")) + "',DelvTime='" + grl.SaveTime(DateTime.Now.ToString("HH:mm")) + "' " +
                                     "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + ddlJobCardCode.SelectedValue.ToString().Trim() +"'";
                            //////////////////Generate MDN/////////////
                            //if (Create_NDM(txtJobCardNo.Text, "InvoiceCreated") == false)
                            //{
                            //    grl.UserMsg(lblMsg, Color.Red, "NDM Data Can not be inserted");
                            //    ObjTrans.RollBackTransaction(ref Trans);
                            //    return;
                            //}
                            ///////////////////////////
                            if (myFunc.ExecuteQuery(IQuery, Trans))
                            {
                                ///Update GST Invoice Table
                                if (double.Parse(txtGSTAmount.Text.Trim()) != 0)
                                {
                                    SqlParameter[] GSTINV_param = { new SqlParameter("@DealerCode",SqlDbType.Char,5),new SqlParameter("@GSTInvNo",SqlDbType.Char,8),
                                                                new SqlParameter("@GSTInvDate",SqlDbType.DateTime),new SqlParameter("@RefNo",SqlDbType.Char,8) };
                                    string GSTInvNo = grl.AutoGen("UniqueSalesTaxInvoice", "SalesTaxInvNO", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                                    GSTINV_param[0].Value = Session["DealerCode"].ToString(); GSTINV_param[1].Value = GSTInvNo;
                                    GSTINV_param[2].Value = grl.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
                                    GSTINV_param[3].Value = NewInvNo.Trim();
                                    if (myFunc.ExecuteSP_NonQuery("[sp_W2_GSTInvoice_Insert]", GSTINV_param, Trans))
                                    {
                                        IQuery = "Update CustomerInvoice set SaleInvoice='" + GSTInvNo + "' " +
                                                 "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + ddlJobCardCode.SelectedValue.ToString().Trim() +"'";
                                        myFunc.ExecuteQuery(IQuery, Trans);
                                    }

                                    // save item tax into table
                                    SaveItemSalesTax(GSTInvNo, NewInvNo);

                                }

                                ///Update PST Invoice Table

                                if (double.Parse(txtPSTAmount.Text.Trim()) != 0)
                                {
                                    SqlParameter[] PSTINV_param = { new SqlParameter("@DealerCode",SqlDbType.Char,5),new SqlParameter("@PSTInvNo",SqlDbType.Char,8),
                                                                new SqlParameter("@PSTInvDate",SqlDbType.DateTime),new SqlParameter("@RefNo",SqlDbType.Char,8) };
                                    string PSTInvNo = grl.AutoGen("PSTUniqueSalesTaxInvoice", "SSTSalesTaxInvNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                                    PSTINV_param[0].Value = Session["DealerCode"].ToString(); PSTINV_param[1].Value = PSTInvNo;
                                    PSTINV_param[2].Value = grl.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
                                    PSTINV_param[3].Value = NewInvNo.Trim();
                                    if (myFunc.ExecuteSP_NonQuery("[sp_W2_PSTInvoice_Insert]", PSTINV_param, Trans))
                                    {
                                        IQuery = "Update CustomerInvoice set SSTSaleInvoice='" + PSTInvNo + "' " +
                                                 "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + ddlJobCardCode.SelectedValue.ToString().Trim() +"'";
                                        myFunc.ExecuteQuery(IQuery, Trans);
                                    }
                                }

                            }
                        }
                        else { myFunc.UserMsg(lblMsg,Color.Red,"Can't save"); }
                    }
                    else if (Decission == "Update")
                    {
                        INV_param[1].Value = ddlInvoiceNo.SelectedValue.ToString().Trim();

                        if (myFunc.ExecuteSP_NonQuery("[sp_W2_CustomerInvoice_Update]", INV_param, Trans))
                        {

                            if (grl.GetStringValuesAgainstCodes("InvoiceNo", ddlInvoiceNo.SelectedValue.ToString().Trim(), "SaleInvoice", "CustomerInvoice") == "")
                            {
                                ///Update GST Invoice Table
                                if (double.Parse(txtGSTAmount.Text.Trim()) != 0)
                                {
                                    SqlParameter[] GSTINV_param = { new SqlParameter("@DealerCode",SqlDbType.Char,5),new SqlParameter("@GSTInvNo",SqlDbType.Char,8),
                                                                new SqlParameter("@GSTInvDate",SqlDbType.DateTime),new SqlParameter("@RefNo",SqlDbType.Char,8) };
                                    string GSTInvNo = grl.AutoGen("UniqueSalesTaxInvoice", "SalesTaxInvNO", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                                    GSTINV_param[0].Value = Session["DealerCode"].ToString(); GSTINV_param[1].Value = GSTInvNo;
                                    GSTINV_param[2].Value = grl.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
                                    GSTINV_param[3].Value = ddlInvoiceNo.SelectedValue.ToString().Trim();
                                    if (myFunc.ExecuteSP_NonQuery("[sp_W2_GSTInvoice_Insert]", GSTINV_param, Trans))
                                    {
                                        IQuery = "Update CustomerInvoice set SaleInvoice='" + GSTInvNo + " " +
                                                 "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + ddlJobCardCode.SelectedValue.ToString().Trim() +"'";
                                        myFunc.ExecuteQuery(IQuery, Trans);
                                    }
                                }
                            }

                            if (grl.GetStringValuesAgainstCodes("InvoiceNo", ddlInvoiceNo.SelectedValue.ToString(), "SSTSaleInvoice", "CustomerInvoice") == "")
                            {
                                ///Update PST Invoice Table
                                if (double.Parse(txtPSTAmount.Text.Trim()) != 0)
                                {
                                    SqlParameter[] PSTINV_param = { new SqlParameter("@DealerCode",SqlDbType.Char,5),new SqlParameter("@PSTInvNo",SqlDbType.Char,8),
                                                                new SqlParameter("@PSTInvDate",SqlDbType.DateTime),new SqlParameter("@RefNo",SqlDbType.Char,8) };
                                    string PSTInvNo = grl.AutoGen("PSTUniqueSalesTaxInvoice", "SSTSalesTaxInvNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                                    PSTINV_param[0].Value = Session["DealerCode"].ToString(); PSTINV_param[1].Value = PSTInvNo;
                                    PSTINV_param[2].Value = grl.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
                                    PSTINV_param[3].Value = ddlInvoiceNo.SelectedValue.ToString().Trim();
                                    if (myFunc.ExecuteSP_NonQuery("[sp_W2_PSTInvoice_Insert]", PSTINV_param, Trans))
                                    {
                                        IQuery = "Update CustomerInvoice set SSTSaleInvoice='" + PSTInvNo + "' " +
                                                 "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + ddlJobCardCode.SelectedValue.ToString().Trim() +"'";
                                        myFunc.ExecuteQuery(IQuery, Trans);
                                    }
                                }
                            }
                            //IQuery = "Update JobCardMaster set SIRMaster='" + txtInvoiceNo.Text + "', GatePass ='" + txtInvoiceNo.Text + "',DelvDate='" + grl.SaveDate(DateTime.Now.ToString("yyyy/MM/dd")) + "',DelvTime='" + grl.SaveTime(DateTime.Now.ToString("HH:mm")) + "' Where DealerCode='" + Session["DealerCode"].ToString() + "'" +
                            //        " and JobCardCode='" + txtJobCardNo.Text + "'";
                            //if (myFunc.ExecuteQuery(IQuery, Trans))
                            //{

                            //}       

                        }
                        else { objMBLL.ShowMessageBox("Update Failed", txtColorName); }
                    }
                    ObjTrans.CommittTransaction(ref Trans);
                    //hdPrint.Value = "";
                    //this.ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ConfirmApproval();", true);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "return confirm('confirm?');", true);
                    // Page.ClientScript.RegisterStartupScript(this.GetType(), "myClick", "window.open('../../Service/ServiceReports/InvoiceNormalView.aspx?JobCardCode=" + txtJobCardNo.Text.Trim() + "&GSTAmt=" + txtGSTAmount.Text + "&SSTAmt=" + txtPSTAmount.Text + "','_blank'," + General.PopUpSize + ");", true);

                    //lblMsg.Visible = true;
                    myFunc.UserMsg(lblMsg, Color.Green, "Record Saved Successfully: ");

                    //lblMSGPop.Text = "Record Saved Successfully: " + txtInvoiceNo.Text;
                    //PopupControlMSG.ShowOnPageLoad = true;
                }
            }
            catch (Exception er)
            {
                lblMsg.Text = er.Message;
                ObjTrans.RollBackTransaction(ref Trans);
            }
        }

        private bool Create_NDM(string strJobCardCode, string Action)
        {
            SqlParameter[] MDNINV_param = { new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                                        new SqlParameter("@JobCardCode",SqlDbType.Char,8),//01
                                        new SqlParameter("@GatePassNo",SqlDbType.Char,8),//02
                                        new SqlParameter("@GatePassDate",SqlDbType.DateTime),//03
                                        new SqlParameter("@UpdUser",SqlDbType.VarChar,50),//04
                                        new SqlParameter("@Action",SqlDbType.VarChar,50)};//05
            MDNINV_param[0].Value = Session["DealerCode"].ToString();
            MDNINV_param[1].Value = strJobCardCode;
            MDNINV_param[2].Value = strJobCardCode;
            MDNINV_param[3].Value = grl.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"));
            MDNINV_param[4].Value = Session["UserName"].ToString();
            MDNINV_param[5].Value = Action;
            //if (myFunc.ExecuteSP_NonQuery("sp_CRM_PostSales_NextDueMaintenance_OnJobCardUpdate", MDNINV_param))
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return true;
        }

        private bool PostServiceFollowUp(string strJobCardCode, string Action)
        {
            SqlParameter[] MDNINV_param = { new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                                        new SqlParameter("@JobCardCode",SqlDbType.Char,8),//01
                                        new SqlParameter("@GatePassNo",SqlDbType.Char,8),//02
                                        new SqlParameter("@GatePassDate",SqlDbType.DateTime),//03
                                        new SqlParameter("@UpdUser",SqlDbType.VarChar,50),//04
                                        new SqlParameter("@Action",SqlDbType.VarChar,50)};//05
            MDNINV_param[0].Value = Session["DealerCode"].ToString();
            MDNINV_param[1].Value = strJobCardCode;
            MDNINV_param[2].Value = strJobCardCode;
            MDNINV_param[3].Value = grl.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"));
            MDNINV_param[4].Value = Session["UserName"].ToString();
            MDNINV_param[5].Value = Action;
            //if (myFunc.ExecuteSP_NonQuery("sp_CRM_Post_PostServiceFollowup_OnJobCardUpdate", MDNINV_param))
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return true;
        }

        private string IsExist()
        {
            string strAction = "No Action";
            string IQuery = " AND JobCardCode= '" + HFJobCard.Value.Trim() + "'";
            if (grl.IsExist("InvoiceNo", ddlInvoiceNo.SelectedValue.ToString().Trim(), "CustomerInvoice",Session["DealerCode"].ToString(), IQuery))
            { strAction = "Update"; }
            else { strAction = "Insert"; }

            return strAction;
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
                    lblMsg.Visible = true;
                    lblMsg.Text = "Error: Field(s) Marked With Red Steriks '*' Are Mendetory...";
                    break;
                }
                else { textBoxes[i].BorderColor = System.Drawing.ColorTranslator.FromHtml("#1A6297"); }
            }
            return isValid;
        }
        private void lookUptext(string lookUpID)
        {

            switch (lookUpID)
            {
                case "1":
                    // Customer | Customer Description
                    //GetLookUpsValue(ref txtCustomer, ref txtCustomerName, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ref HFCustomer);
                    break;
                case "4":
                    // JobCardNo
                   // GetLookUpsValue(ref txtJobCardNo, ref txtJobCardNo, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ref HFJobCard);
                    break;
                case "32":
                    // Invoice No
                    //GetLookUpsValue(ref txtInvoiceNo, ref txtJobCardNo, ref txtInvoiceDate, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ViewState["ixd3"].ToString());
                    break;
                default:
                    break;
            }
        }
        private void GetLookUpsValue(ref TextBox txt1, ref TextBox txt2, string ixd1, string ixd2, ref HiddenField hf)
        {


            if (Session["LookUpData"] != null)
            {
                GridViewRow Drow = (GridViewRow)Session["LookUpData"];
                txt1.Text = (txt1 != null ? Drow.Cells[Convert.ToInt32(ixd1)].Text.ToString() : "");
                txt2.Text = (txt2 != null ? Drow.Cells[Convert.ToInt32(ixd2)].Text.ToString() : "");

                hf.Value = txt1.Text;
                Session["LookUpData"] = null;
            }
        }


        private void GetLookUpsValue(ref TextBox txt1, ref TextBox txt2, ref TextBox txt3, string ixd1, string ixd2, string ixd3)
        {


            if (Session["LookUpData"] != null)
            {
                GridViewRow Drow = (GridViewRow)Session["LookUpData"];
                txt1.Text = (txt1 != null ? Drow.Cells[Convert.ToInt32(ixd1)].Text.ToString() : "");
                txt2.Text = (txt2 != null ? Drow.Cells[Convert.ToInt32(ixd2)].Text.ToString() : "");
                txt3.Text = (txt3 != null ? Drow.Cells[Convert.ToInt32(ixd3)].Text.ToString() : "");

                Session["LookUpData"] = null;
            }
        }

        private void FillJobCardDetailGrid()
        {
            SqlParameter[] dsParam = {  new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)};
            dsParam[0].Value = Session["DealerCode"].ToString();
            dsParam[1].Value = (HFJobCard.Value == "" ? null : HFJobCard.Value.Trim());

            dsJobCardDetail = new DataSet(); dsJobCardDetail = myFunc.FillDataSet("sp_W2_JobCard_Detail_Select", dsParam);
            if (dsJobCardDetail.Tables[0].Rows.Count == 0) dsJobCardDetail.Tables[0].Rows.Add(dsJobCardDetail.Tables[0].NewRow());
            gvJobCard.DataSource = dsJobCardDetail; gvJobCard.DataBind();

            dsJobCardParts = new DataSet(); dsJobCardParts = myFunc.FillDataSet("sp_W2_JobCard_PartsDetail_Select", dsParam);
            if (dsJobCardParts.Tables[0].Rows.Count == 0) dsJobCardParts.Tables[0].Rows.Add(dsJobCardParts.Tables[0].NewRow());
            gvJobCardParts.DataSource = dsJobCardParts; gvJobCardParts.DataBind();

            dsJobCardLub = new DataSet(); dsJobCardLub = myFunc.FillDataSet("sp_W2_JobCard_LubricanteDetail_Select", dsParam);
            if (dsJobCardLub.Tables[0].Rows.Count == 0) dsJobCardLub.Tables[0].Rows.Add(dsJobCardLub.Tables[0].NewRow());
            gvLubParts.DataSource = dsJobCardLub; gvLubParts.DataBind();

            dsJobCardSublet = new DataSet(); dsJobCardSublet = myFunc.FillDataSet("sp_W2_JobCard_Sublet_Select", dsParam);
            if (dsJobCardSublet.Tables[0].Rows.Count == 0) dsJobCardSublet.Tables[0].Rows.Add(dsJobCardSublet.Tables[0].NewRow());
            gvSublet.DataSource = dsJobCardSublet; gvSublet.DataBind();

            dsJobCardConParts = new DataSet(); dsJobCardConParts = myFunc.FillDataSet("sp_W2_JobCard_ConPartsDetail_Select", dsParam);
            if (dsJobCardConParts.Tables[0].Rows.Count == 0) dsJobCardConParts.Tables[0].Rows.Add(dsJobCardConParts.Tables[0].NewRow());
            gvJobCardConParts.DataSource = dsJobCardConParts; gvJobCardConParts.DataBind();
        }

        protected void LoadMasterData(string strClearCondition)
        {
            ClearMasterData(strClearCondition);
            if (strClearCondition == "InvoiceNo")
            {
                //txtJobCardNo.Text = myFunc.GetJobCardNoFromCustomerInvNo(txtInvoiceNo.Text.Trim());
                HFJobCard.Value = myFunc.GetJobCardNoFromCustomerInvNo(ddlInvoiceNo.SelectedValue.ToString().Trim());
                //lblJobCardNo.Text = HFJobCard.Value;
                HFCustomer.Value = myFunc.GetCustomerCodeByJobCardNo(HFJobCard.Value);
                ddlCustCode.SelectedValue = HFCustomer.Value;
                //ddlJobCardCode.Visible = false;
                ddlJobCardCode.Enabled = false;
                Load_ddlJobCardCode();
                //lblJobCardNo.Visible = true;
            }
            FillJobCardDetailGrid();


            SqlParameter[] dsMasterParam = { new SqlParameter("@DealerCode",SqlDbType.Char,5),  new SqlParameter("@JobCardCode",SqlDbType.Char,8),
                                         new SqlParameter("@RegNo",SqlDbType.VarChar,50),   new SqlParameter("@EngineNo",SqlDbType.VarChar,50),
                                         new SqlParameter("@ChassisNo",SqlDbType.VarChar,50),new SqlParameter("@CusCode",SqlDbType.VarChar,50)
        };
            dsMasterParam[0].Value = Session["DealerCode"].ToString();
            if (HFJobCard.Value=="" & txtRegNo.Text == "" & txtEngineNo.Text == null & txtChassisNo.Text == ""
                & HFCustomer.Value == "") dsMasterParam[1].Value = "0";
            else dsMasterParam[1].Value = (HFJobCard.Value.Trim() == "" ? null : HFJobCard.Value.Trim());
            dsMasterParam[2].Value = (txtRegNo.Text == "" ? null : txtRegNo.Text);
            dsMasterParam[3].Value = (txtEngineNo.Text == "" ? null : txtEngineNo.Text);
            dsMasterParam[4].Value = (txtChassisNo.Text == "" ? null : txtChassisNo.Text);
            dsMasterParam[5].Value = null;

            dsJobCardMaster = new DataSet(); dsJobCardMaster = myFunc.FillDataSet("sp_W2_JobCard_Master_Select", dsMasterParam);
            if (dsJobCardMaster.Tables[0].Rows.Count > 0)
            {
                txtInvoiceDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                ddlJobCardCode.SelectedValue= dsJobCardMaster.Tables[0].Rows[0]["JobCardCode"].ToString();
                txtChassisNo.Text = dsJobCardMaster.Tables[0].Rows[0]["ChassisNo"].ToString();
                txtProductCode.Text = dsJobCardMaster.Tables[0].Rows[0]["ProdCode"].ToString();
                txtJobCardType.Text = dsJobCardMaster.Tables[0].Rows[0]["JobTypeDesc"].ToString();

                ds = new DataSet(); grl.CodeExists("vehicle", "ProdCode='" + txtProductCode.Text.Trim() + "'", ref ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtProductName.Text = ds.Tables[0].Rows[0]["ProdDesc"].ToString();
                    txtColorName.Text = ds.Tables[0].Rows[0]["Color"].ToString();
                }
                else txtProductCode.Text = txtColorName.Text = "";

                txtRemarksUser.Text = dsJobCardMaster.Tables[0].Rows[0]["Remarks"].ToString();
                txtUserName.Text = dsJobCardMaster.Tables[0].Rows[0]["UserName"].ToString();

                txtJobType.Text = dsJobCardMaster.Tables[0].Rows[0]["JobTypeCode"].ToString().Trim();
                txtJobTypeDesc.Text = grl.GetStringValuesAgainstCodes("jobTypeCode", txtJobType.Text, "JobTypeDesc", "JobTypeMaster");

                txtOutKM.Text = dsJobCardMaster.Tables[0].Rows[0]["KM"].ToString().Trim();
                txtEngineNo.Text = dsJobCardMaster.Tables[0].Rows[0]["EngineNo"].ToString();
                txtRegNo.Text = dsJobCardMaster.Tables[0].Rows[0]["RegNo"].ToString();
                ddlCustCode.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["CusCode"].ToString();
                //txtCustomerName.Text = grl.GetStringValuesAgainstCodes("cusCode", ddlCustCode.SelectedValue.ToString().Trim(), "CusDesc+' '+FatherHusName", "Customer");
                txtColorName.Text = grl.GetStringValuesAgainstCodes("ColorCode", grl.GetStringValuesAgainstCodes("RegNo", txtRegNo.Text, "ColorCode", "CustomerVehicle"), "ColorDesc", "Color");

                txtGrossAmount.Text = CustomCDBL((Convert.ToDecimal(dsJobCardMaster.Tables[0].Rows[0]["JobsTotal"].ToString()) +
                                                Convert.ToDecimal(dsJobCardMaster.Tables[0].Rows[0]["PartsTotal"].ToString()) +
                                                Convert.ToDecimal(dsJobCardMaster.Tables[0].Rows[0]["SubletTotal"].ToString()) +
                                                Convert.ToDecimal(dsJobCardMaster.Tables[0].Rows[0]["LubPartsTotal"].ToString())).ToString()).ToString();

                txtJobsTotal.Text = Convert.ToDecimal(dsJobCardMaster.Tables[0].Rows[0]["JobsTotal"].ToString()).ToString("#########0");
                txtLubTotal.Text = Convert.ToDecimal(dsJobCardMaster.Tables[0].Rows[0]["LubPartsTotal"].ToString()).ToString("#########0");
                txtPartsTotal.Text = Convert.ToDecimal(dsJobCardMaster.Tables[0].Rows[0]["PartsTotal"].ToString()).ToString("#########0");
                txtSubletTotal.Text = Convert.ToDecimal(dsJobCardMaster.Tables[0].Rows[0]["SubletTotal"].ToString()).ToString("#########0");

                double GSTPercent = double.Parse(grl.GetStringValuesAgainstCodes("DealerCode", "AAAAA", "GST", "BaseRate"));
                txtGSTPercent.Text = GSTPercent.ToString();

                double totPartsLub = (txtLubTotal.Text == "" ? 0 : double.Parse(txtLubTotal.Text))
                                    + (txtPartsTotal.Text == "" ? 0 : double.Parse(txtPartsTotal.Text));
                txttotPartsLub.Text = totPartsLub.ToString();


                double totJobSublet = ((txtJobsTotal.Text == "" ? 0 : double.Parse(txtJobsTotal.Text))
                                        + (txtSubletTotal.Text == "" ? 0 : double.Parse(txtSubletTotal.Text)));
                txttotJobSublet.Text = totJobSublet.ToString();

                double GSTPer = txtGSTPercent.Text.Trim() == "" ? 0 : double.Parse(txtGSTPercent.Text);
                txtGSTAmount.Text = Math.Round(totPartsLub * (GSTPer / 100), 0).ToString();

                txtPSTPercent.Text = "0";
                //if (grl.IsExist("Region", "AOK", "Dealer", ""))
                //{
                //    txtPSTPercent.Text = grl.GetStringValuesAgainstCodes("DealerCode", SessionInformation .DealerCode , "PST", "Dealer");
                //}
                //else if (grl.IsExist("Region", "AOL", "Dealer", ""))
                //{
                //    txtPSTPercent.Text = grl.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "PST", "Dealer");
                //}
                txtPSTPercent.Text = grl.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "PST", "Dealer");

                double PSTPer = double.Parse(txtPSTPercent.Text);
                txtPSTAmount.Text = Math.Round(totJobSublet * (PSTPer / 100), 0).ToString();

                // Calculate Item wise Tax
                CalculateSalesTax();

                double ttx = Convert.ToDouble(txtTaxAmount.Text) + Convert.ToDouble(txtGSTAmount.Text);
                txtGSTAmount.Text = Math.Round(ttx, 0).ToString();

                txtSubTotal.Text = txtGrossAmount.Text == "" ? "0" :
                                                           (
                                                              double.Parse(txtGrossAmount.Text)
                                                           + (txtGSTAmount.Text == "" ? 0 : double.Parse(txtGSTAmount.Text))
                                                           + (txtPSTAmount.Text == "" ? 0 : double.Parse(txtPSTAmount.Text))
                                                           ).ToString();

                txtTotalAmtCustomer.Text = txtSubTotal.Text == "" ? "0" :
                                                                    (
                                                                      double.Parse(txtSubTotal.Text)
                                                                      + (txtDiscountParts.Text == "" ? 0 : double.Parse(txtDiscountParts.Text))
                                                                      + (txtDiscLabor.Text == "" ? 0 : double.Parse(txtDiscLabor.Text))
                                                                      ).ToString();

                //txtTotalAmtCustomer.Text = txtSubTotal.Text == "" ? "0" :
                //                                                    (
                //                                                      double.Parse(txtSubTotal.Text)
                //                                                      + (txtDiscountParts.Text == "" ? 0 : double.Parse(txtDiscountParts.Text))
                //                                                      + (txtDiscLabor.Text == "" ? 0 : double.Parse(txtDiscLabor.Text))
                //                                                      + (txtGSTAmount.Text == "" ? 0 : double.Parse(txtGSTAmount.Text))
                //                                                      ).ToString();


                txtPaidCustomer.Text = "0";
                // Some fields are not found due to calculation problem occured.
                //NetAmount Customer =(Convert.ToDecimal(txtGrossAmount.Text)) - Part Discount + GST Amount - Labor Discount
                txtTotalAmntInsurance.Text = "0"; 
                
                txtPaidInsurance.Text = "0"; txtVEODAmount.Text = "0";
                txtDipriAmount.Text = "0"; txtDistOtherParts.Text = "0";
                txtDistOtherPercent.Text = "0";            //txtOutKM.Text               = "0";


                SqlParameter[] dsParamInv = {  new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                           new SqlParameter("@InvoiceNo",SqlDbType.Char,8)};
                dsParamInv[0].Value = Session["DealerCode"].ToString();
                dsParamInv[1].Value = ddlInvoiceNo.SelectedValue.ToString().Trim();//(txtInvoiceNo.Text.Trim() == "" ? null : txtInvoiceNo.Text.Trim());
                dsCustomerInvoice = new DataSet();
                dsCustomerInvoice = myFunc.FillDataSet("sp_W2_CustomerInvoice_Select", dsParamInv);
                if (dsCustomerInvoice.Tables[0].Rows.Count > 0)
                {
                    txtInvoiceDate.Text = Convert.ToDateTime(dsCustomerInvoice.Tables[0].Rows[0]["InvoiceDate"].ToString()).ToString("dd-MM-yyyy");
                    txtGrossAmount.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["GrossAmount"].ToString()), 0).ToString();
                    txtGSTPercent.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["GSTPer"].ToString()), 0).ToString();
                    txtGSTAmount.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["GSTAmount"].ToString()), 0).ToString();
                    txtPSTPercent.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["SSTPer"].ToString()), 0).ToString();
                    txtPSTAmount.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["SSTAmount"].ToString()), 0).ToString();
                    //txtSubletTotal.Text = dsCustomerInvoice.Tables[0].Rows[0]["SubTotal"].ToString();
                    txtSubTotal.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["SubTotal"].ToString()), 0).ToString();
                    txtDiscLabor.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["DiscountAmount"].ToString()), 0).ToString();
                    txtDiscLaborPercent.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["DiscountPer"].ToString()), 0).ToString();
                    txtTotalAmtCustomer.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["NetAmountCustomer"].ToString()), 0).ToString();
                    txtDiscountParts.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["PartsDiscount"].ToString()), 0).ToString();
                    txtDiscPercentPart.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["PartsDiscountPer"].ToString()), 0).ToString();
                    txtTRNumber.Text = dsCustomerInvoice.Tables[0].Rows[0]["TRNumber"].ToString();
                    txtTRDate.Text = txtTRDate.Text.Trim() == "" ? null : Convert.ToDateTime(dsCustomerInvoice.Tables[0].Rows[0]["TRDate"].ToString()).ToString("dd-MM-yyyy");
                }
                //txtTotAmtInsComp.Text = ((txtSubTotal.Text == "" ? 0 : double.Parse(txtSubTotal.Text)) - (txtDepAmount.Text == "" ? 0 : double.Parse(txtDepAmount.Text))).ToString();
                if (txtJobCardType.Text == "Insurance")
                {
                    txtTotAmtInsComp.Text = ((txtSubTotal.Text == "" ? 0 : double.Parse(txtSubTotal.Text)) - (txtDepAmount.Text == "" ? 0 : double.Parse(txtDepAmount.Text))).ToString();
                    txtTotalAmtCustomer.Text = (Convert.ToDouble(txtTotalAmtCustomer.Text) - Convert.ToDouble(txtTotAmtInsComp.Text)).ToString();
                }
                if (txtJobCardType.Text == "Insurance")
                {
                    txtDiscLabor.ReadOnly = true;
                    txtDiscountParts.ReadOnly = true;
                    ddlInsCo.Enabled = true;
                    ddlInsBranch.Enabled = true;
                    txtDiscountParts.Enabled = false;
                    txtDiscLabor.Enabled = false;

                }
                else
                {
                    txtDiscLabor.ReadOnly = false;
                    txtDiscountParts.ReadOnly = false;
                    ddlInsCo.Enabled = false;
                    ddlInsBranch.Enabled = false;
                    txtDiscountParts.Enabled = true;
                    txtDiscLabor.Enabled = true;
                }
            }
        }

        private void CalculateSalesTax()
        {
            double totTax = 0;
            // Calculate PartItems Tax

            DataTable dt = dsJobCardParts.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string itmcod = dt.Rows[i]["ItemCode"].ToString();
                double stxpct = myFunc.GetSalesTaxPercentByItemCode(itmcod);
                
                int qty = dt.Rows[i]["Qty"].ToString()=="" ? 0 : Convert.ToInt16(dt.Rows[i]["Qty"].ToString());
                double prc = dt.Rows[i]["Price"].ToString()=="" ? 0 : Convert.ToDouble(dt.Rows[i]["Price"].ToString());
                double tot = qty * prc;

                double totamt = tot * stxpct / 100;

                totTax = totTax + totamt;
                
            }

            dt.Dispose();

            // Calculate PartItems con tax
            
            //dt = dsJobCardConParts.Tables[0];

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    string itmcod = dt.Rows[i]["ItemCode"].ToString();
            //    double stxpct = myFunc.GetSalesTaxPercentByItemCode(itmcod);
            //    int qty = dt.Rows[i]["Qty"].ToString() =="" ? 0 : Convert.ToInt16(dt.Rows[i]["Qty"].ToString());
            //    double prc = dt.Rows[i]["Price"].ToString() == "" ? 0 : Convert.ToDouble(dt.Rows[i]["Price"].ToString());
            //    double tot = qty * prc;

            //    double totamt = tot * stxpct / 100;

            //    totTax = totTax + totamt;

            //}

            //dt.Dispose();

            txtTaxAmount.Text = totTax.ToString("####0");

        }
        private void SaveItemSalesTax(string InvoiceNumber,string InvNo="")
        {
            double totTax = 0;
            // Calculate PartItems Tax
            SqlParameter[] dsParam = {  new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)};
            dsParam[0].Value = Session["DealerCode"].ToString();
            dsParam[1].Value = ddlJobCardCode.SelectedValue.ToString().Trim();

            dsJobCardParts = new DataSet(); 
            dsJobCardParts = myFunc.FillDataSet("sp_W2_JobCard_PartsDetail_Select", dsParam);


            DataTable dt = dsJobCardParts.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string itmcod = dt.Rows[i]["ItemCode"].ToString();
                double stxpct = myFunc.GetSalesTaxPercentByItemCode(itmcod);
                double ftxpct = myFunc.GetFurtherSalesTaxPercentByItemCode(itmcod);
                string itmsrc = myFunc.GetItemSourceByItemCode(itmcod);
                int qty = Convert.ToInt16(dt.Rows[i]["Qty"].ToString());
                double prc = Convert.ToDouble(dt.Rows[i]["Price"].ToString());
                double tot = qty * prc;

                double gstot = tot * stxpct / 100;
                double fstot = tot * ftxpct / 100;

                double totamt = (tot * stxpct / 100) + (tot * ftxpct / 100);

                string jcno = dt.Rows[i]["JobCardCode"].ToString();
                string ityp = dt.Rows[i]["Type"].ToString();

                
                string sql = "insert into ItemsTaxTrx (DealerCode,InvoiceNumber,JobCardCode,ItemType,SalesTaxNo,ItemCode,GSTPerc,FGSTPerc,SalesTaxAmount,InvoiceType,GSTAmount,FGSTAmount)";
                sql += " values('" + Session["DealerCode"].ToString() + "','" + InvNo + "','" + jcno + "','" + ityp + "','" + InvoiceNumber + "','" + itmcod + "','" + stxpct.ToString() + "','" + ftxpct.ToString() + "','" + totamt + "','J','" + gstot.ToString() + "','" + fstot.ToString() + "')";

                DataTable dtmp = myFunc.GetData(sql);
                dtmp.Dispose();
            }

            dt.Dispose();

            // Calculate PartItems con tax

            //dt = dsJobCardConParts.Tables[0];

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    string itmcod = dt.Rows[i]["ItemCode"].ToString();
            //    double stxpct = myFunc.GetSalesTaxPercentByItemCode(itmcod);
            //    int qty = Convert.ToInt16(dt.Rows[i]["Qty"].ToString());
            //    double prc = Convert.ToDouble(dt.Rows[i]["Price"].ToString());
            //    double tot = qty * prc;

            //    double totamt = tot * stxpct / 100;

            //    totTax = totTax + totamt;

            //    string sql = "insert into (DealerCode,SINumber,SalesTaxNo,ItemCode,SalesPercent,SalesTaxAmount,InvoiceType)";
            //    sql += " value('" + Session["DealerCode"].ToString() + "','" + InvoiceNumber + "','','" + itmcod + "','" + prc.ToString() + "','" + totamt + "','C')";

            //    DataTable dtmp = myFunc.GetData(sql);
            //    dtmp.Dispose();

            //}

            //dt.Dispose();

            //txtTaxAmount.Text = totTax.ToString("####0");

        }

        protected void imgbtnCustomer_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                ViewState["lookupid"] = 1; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2;
                //clslook.CustomerLookup(txtRegNo, ViewState["lookupid"].ToString(), "", "../../../");
            }
            catch (Exception ex) { throw ex; }

        }
        protected void imgbtnJobCard_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewState["lookupid"] = 4; ViewState["ixd1"] = 1; ViewState["ixd2"] = 1;
                //clslook.LU_Get_JobCardNo(ImageButton1, ViewState["lookupid"].ToString(), "", "../../../");
            }
            catch (Exception ex) { throw ex; }
        }
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewState["lookupid"] = 32; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
                //clslook.LU_Get_InvoiceNo(ImageButton1, ViewState["lookupid"].ToString(), "", "../../../");
            }
            catch (Exception ex) { throw ex; }
        }
        protected void gvSublet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Alternate || e.Row.RowState == DataControlRowState.Normal))
            {
                // e.Row.Cells[1].Text = grl.GetStringValuesAgainstCodes("VendorCode", e.Row.Cells[0].Text, "VendorDesc", "vendor");            
                // e.Row.Cells[2].Text = grl.GetStringValuesAgainstCodes("DefJobCode", e.Row.Cells[2].Text, "DefJobDesc", "defaultjob");            
                //e.Row.Cells[4].Text = Convert.ToDateTime(e.Row.Cells[4].Text).ToString("dd/MM/yyyy");
            }
        }




        public void Calculate_NetAmountForCustomer()
        {
            double dNetAmountForCustomer = 0;
            //If Normal & Club Suzuki
            dNetAmountForCustomer = CustomCDBL(txtGrossAmount.Text) + CustomCDBL(txtGSTAmount.Text) + CustomCDBL(txtPSTAmount.Text) - CustomCDBL(txtDiscountParts.Text) - CustomCDBL(txtDiscLabor.Text);
            txtTotalAmtCustomer.Text = CustomCDBLRoundFormat(dNetAmountForCustomer);
            if (txtJobCardType.Text == "Insurance")
            {
                txtTotalAmtCustomer.Text = (Convert.ToDouble(txtTotalAmtCustomer.Text) - Convert.ToDouble(txtTotAmtInsComp.Text)).ToString();
            }
        }

        public static double CustomCDBL(object txt)
        {
            if (object.ReferenceEquals(txt, DBNull.Value)) return 0;
            else if (txt == null) return 0;
            else if (string.IsNullOrEmpty(txt.ToString().Trim())) return 0;
            else return Math.Round(Convert.ToDouble(txt), 0);
        }

        public static string CustomCDBLRoundFormat(object txt)
        {
            if (object.ReferenceEquals(txt, DBNull.Value)) return "0";
            else if (txt == null) return "0";
            else if (string.IsNullOrEmpty(txt.ToString().Trim())) return "0";
            //else return Convert.ToDouble(txt).ToString("#,###,###,##0");
            else return Convert.ToDouble(txt).ToString();
        }

        protected void gvJobCard_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Alternate || e.Row.RowState == DataControlRowState.Normal))
            {
                //   e.Row.Cells[1].Text = grl.GetStringValuesAgainstCodes("DefJobCode", e.Row.Cells[0].Text, "DefJobDesc", "defaultjob");
                //select DefJobDesc from defaultjob where DefJobCode=00005
                //  e.Row.Cells[3].Text = grl.GetStringValuesAgainstCodes("EmpCode", e.Row.Cells[2].Text, "EmpName", "DealerEmp");
                //select Empcode,EmpName from dealeremp
            }
        }
        protected void gvJobCardParts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Alternate || e.Row.RowState == DataControlRowState.Normal))
            {
                double InsAmount = 0.00;
                if (e.Row.Cells[3].Text != "&nbsp;")
                {
                    txtDepAmount.Text = (Math.Round(InsAmount + Convert.ToDouble(e.Row.Cells[3].Text))).ToString();
                }

                //select DefJobDesc from defaultjob where DefJobCode=00005
            }
        }
        protected void gvLubParts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Alternate || e.Row.RowState == DataControlRowState.Normal))
            {
                //  e.Row.Cells[2].Text = grl.GetStringValuesAgainstCodes("PartItemNo", e.Row.Cells[1].Text, "ItemDesc", "Item");
                //select DefJobDesc from defaultjob where DefJobCode=00005
            }
        }
        
        //protected void txtJobCardNo_TextChanged(object sender, EventArgs e)
        //{
        //    HFJobCard.Value = txtJobCardNo.Text;
        //    LoadMasterData("JobCard");
        //    HFCustomer.Value = txtCustomer.Text;
        //    //txtInvoiceNo.ReadOnly = true;
        //}
        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {

        }
        protected void txtInvoiceNo_TextChanged(object sender, EventArgs e)
        {

        }
        protected void btnLoadData_Click(object sender, EventArgs e)
        {

        }
        protected void ClearMasterData(string strClearCondition)
        {
            txtInvoiceDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            if (strClearCondition == "JobCard")
            {
                ddlInvoiceNo.SelectedIndex = 0; ;
            }

            //txtJobCardNo.Text = "";

            txtChassisNo.Text = "";
            txtProductCode.Text = ""; txtEngineNo.Text = "";
            txtProductName.Text = ""; txtColorName.Text = "";

            txtJobType.Text = ""; txtJobTypeDesc.Text = "";

            txtOutKM.Text = ""; txtEngineNo.Text = ""; txtRegNo.Text = "";
             txtColorName.Text = ""; txtGrossAmount.Text = "";

            txtJobsTotal.Text = ""; txtLubTotal.Text = ""; txtPartsTotal.Text = ""; txtSubletTotal.Text = "";

            txtGSTPercent.Text = ""; txttotPartsLub.Text = ""; txttotJobSublet.Text = ""; txtGSTAmount.Text = "";
            txtPSTPercent.Text = ""; txtPSTAmount.Text = "";
            txtSubTotal.Text = ""; txtTotalAmtCustomer.Text = ""; txtPaidCustomer.Text = "";

            txtTotalAmntInsurance.Text = ""; 
            
            txtPaidInsurance.Text = ""; txtVEODAmount.Text = "";
            txtDipriAmount.Text = ""; txtDistOtherParts.Text = "";
            txtDistOtherPercent.Text = "";            //txtOutKM.Text               = "0";

            txtDiscountParts.Text = ""; txtDiscPercentPart.Text = "";
            txtDiscLabor.Text = ""; txtDiscLaborPercent.Text = "";
            txtRemarksUser.Text = ""; txtUserName.Text = "";
            ddlCustCode.SelectedIndex = 0;
        }
        //protected void btnClear_Click(object sender, EventArgs e)
        //{
            //txtInvoiceDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            //ddlInvoiceNo.SelectedIndex = 0;
            //ddlJobCardCode.SelectedIndex=0; txtChassisNo.Text = "";
            //txtProductCode.Text = ""; txtEngineNo.Text = "";
            //txtProductName.Text = ""; txtColorName.Text = "";

            //txtJobType.Text = ""; txtJobTypeDesc.Text = "";

            //txtOutKM.Text = ""; txtEngineNo.Text = ""; txtRegNo.Text = ""; 
            //txtCustomerName.Text = ""; txtColorName.Text = ""; txtGrossAmount.Text = "";

            //txtJobsTotal.Text = ""; txtLubTotal.Text = ""; txtPartsTotal.Text = ""; txtSubletTotal.Text = "";

            //txtGSTPercent.Text = ""; txttotPartsLub.Text = ""; txttotJobSublet.Text = ""; txtGSTAmount.Text = "";
            //txtPSTPercent.Text = ""; txtPSTAmount.Text = "";
            //txtSubTotal.Text = ""; txtTotalAmtCustomer.Text = ""; txtPaidCustomer.Text = "";

            //txtTotalAmntInsurance.Text = ""; txtInsuranceComp.Text = "";
            //txtBranch.Text = "";            //txtPaidCustomer.Text        = "0";
            //txtPaidInsurance.Text = ""; txtVEODAmount.Text = "";
            //txtDipriAmount.Text = ""; txtDistOtherParts.Text = "";
            //txtDistOtherPercent.Text = "";            //txtOutKM.Text               = "0";

            //txtDiscountParts.Text = ""; txtDiscPercentPart.Text = "";
            //txtDiscLabor.Text = ""; txtDiscLaborPercent.Text = "";
            //txtRemarksUser.Text = ""; txtUserName.Text = "";
            //txtTotAmtInsComp.Text = "";
            //txtDepAmount.Text = "";
            //ddlInsCo.SelectedIndex = 0;
            //if (ddlInsBranch.Items.Count > 0)
            //{
            //    ddlInsBranch.SelectedIndex = 0;
            //}
            //if (lblMsg.ForeColor != Color.Green)
            //{
            //    lblMsg.Text = "";
            //}
            //HFJobCard.Value = "";
            //HFChassisNo.Value = "";
            //HFCustomer.Value = "";
            //HFEngineNo.Value = "";
            //HFRegNo.Value = "";
            
            //FillJobCardDetailGrid();
            //ddlCustCode.SelectedIndex = 0;
            ////txtInvoiceNo.DataBind();
            ////txtJobCardNo.DataBind();

            ////txtInvoiceNo.ReadOnly = false;
            ////txtJobCardNo.ReadOnly = false;
            ////lblJobCardNo.Visible = false;
            ////txtJobCardNo.Visible = true;
            //ddlJobCardCode.Enabled = true;
            
        //}
        protected void txtRemarksUser_TextChanged(object sender, EventArgs e)
        {

        }
        protected void gvJobCard_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string URL = string.Empty;
            URL = "../../Service/ServiceReports/InvoiceNormalView.aspx?JobCardCode=" + ddlJobCardCode.SelectedValue.ToString().Trim() + "&GSTAmt=" + txtGSTAmount.Text + "&SSTAmt=" + txtPSTAmount.Text + "&InvType=" + txtJobCardType.Text + "&DepAmt=" + txtDepAmount + "&InsAmt=" + txtTotAmtInsComp.Text + "&InvRefNo=" + ddlInvoiceNo.SelectedValue.ToString().Trim();
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
            /////////////
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "myClick", "window.open('../../Service/ServiceReports/InvoiceNormalView.aspx?JobCardCode=" + txtJobCardNo.Text.Trim() + "&GSTAmt=" + txtGSTAmount.Text + "&SSTAmt=" + txtPSTAmount.Text + "','_blank'," + General.PopUpSize + ");", true);
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (ddlInvoiceNo.SelectedIndex == 0) {

                myFunc.UserMsg(lblMsg, Color.Red, "Invoice No should not be blank");
                return;
            }

            if (HFJobCard.Value == "") {

                myFunc.UserMsg(lblMsg, Color.Red, "JobCard code should not be blank");
                //lblMSGPop.Text = "JobCard code should not be blank ";
                //PopupControlMSG.ShowOnPageLoad = true;
                return;
            }

            //if (grl.CodeExists("CustomerInvoice", "InvoiceNo", txtInvoiceNo.Text) == false) {
            //    myFunc.UserMsg(lblMsg, Color.Red, "Invoice no. not exist");
            //    //lblMSGPop.Text = "Invoice no. not exist ";
            //    //PopupControlMSG.ShowOnPageLoad = true;
            //    return;

            //}

            if (grl.CodeExists("PaymentReceiptDetail", "InvoiceNo", ddlInvoiceNo.SelectedValue.ToString().Trim(), " and InvoiceType='Service'") == true)
            {

                myFunc.UserMsg(lblMsg, Color.Red, "Invoice No. can not be deleted, payment receipt has made");
                //lblMSGPop.Text ="Invoice No. can not be deleted, payment receipt has made" ;
                //PopupControlMSG.ShowOnPageLoad = true;
                return;
            }

            try
            {

                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    string IQuery = "Update CustomerInvoice set GatePass ='',DelFlag='Y'" +
                        //"DelvDate='"+ grl.SaveDate(DateTime.Now.ToString("dd-MM-yyyy")) +"',DelvTime='"+grl.SaveTime(DateTime.Now.ToString("HH:mm"))  +"' " +
                                     "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + HFJobCard.Value.Trim() + "'";
                    IQuery += " Update JobCardMaster Set gatePass='' Where DealerCode='" + Session["DealerCode"].ToString() + "' And JobCardCode='" + HFJobCard.Value.Trim() + "'";
                    myFunc.ExecuteQuery(IQuery, Trans);

                    //Call Method of NDM and PSF For Delete

                    if (Create_NDM(HFJobCard.Value.Trim(), "InvoiceDeleted") == false)
                    {
                        //grl.UserMsg(lblMsg, Color.Red, "NDM Data Can not be Deleted");
                        myFunc.UserMsg(lblMsg, Color.Red, "NDM Data Can not be Deleted");
                        //lblMSGPop.Text = "NDM Data Can not be Deleted";
                        //PopupControlMSG.ShowOnPageLoad = true;
                        ObjTrans.RollBackTransaction(ref Trans);

                        return;
                    }
                    if (PostServiceFollowUp(HFJobCard.Value.Trim(), "InvoiceDeleted") == false)
                    {
                        myFunc.UserMsg(lblMsg, Color.Red, "Post Service FollowUp Data Can not be Deleted");
                        //lblMSGPop.Text = "Post Service FollowUp Data Can not be Deleted";
                        //PopupControlMSG.ShowOnPageLoad = true;
                        ObjTrans.RollBackTransaction(ref Trans);
                        return;
                    }

                    string sqld = "delete from itemsTaxTrx Where DealerCode='" + Session["DealerCode"].ToString() + "' And JobCardCode='" + HFJobCard.Value.Trim() + "'";
                    myFunc.ExecuteQuery(sqld, Trans);

                    ObjTrans.CommittTransaction(ref Trans);
                    //lblMsg.Visible = true;
                    myFunc.UserMsg(lblMsg, Color.Green, "Deleted Successfully Inv. No: " +ddlInvoiceNo.SelectedValue.ToString());
                    Load_ddlInvoiceNo();

                    //lblMSGPop.Text = "Deleted Successfully Inv. No: " + txtInvoiceNo.Text;
                    //PopupControlMSG.ShowOnPageLoad = true;

                    btnClearForm_Click(null, null);
                }
            }
            catch (Exception) { ObjTrans.RollBackTransaction(ref Trans); }

        }
        protected void btnPost_Click(object sender, EventArgs e)
        {

        }
        protected void ddlInsCo_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlParameter[] param = { new SqlParameter("@InsCompCode", SqlDbType.Char, 5) };
            param[0].Value = ddlInsCo.SelectedValue;
            objMBLL.FillDrp_SP(ddlInsBranch, "sp_2W_Branch_select", "BranchCode", "BranchDesc", param, true, "--Select--", false, "");
        }

        protected void txtPSTPercent_TextChanged(object sender, EventArgs e)
        {

        }
        protected void txtPSTAmount_TextChanged(object sender, EventArgs e)
        {

        }
        protected void txtSubTotal_TextChanged(object sender, EventArgs e)
        {

        }

        //protected void btnPopOk_Click(object sender, EventArgs e)
        //{
        //    PopupControlMSG.ShowOnPageLoad = false;
        //}

        //protected void btnPopCancel_Click(object sender, EventArgs e)
        //{
        //    PopupControlMSG.ShowOnPageLoad = false;
        //}

        protected void txtCustomer_TextChanged(object sender, EventArgs e)
        {

        }

        //protected void txtInvoiceNo_TextChanged1(object sender, EventArgs e)
        //{
        //    LoadMasterData("InvoiceNo");
        //    //txtInvoiceNo.ReadOnly = true;
        //    txtJobCardNo.ReadOnly = true;
        //    txtCustomer.ReadOnly = true;

        //}

        //protected void btnPrint_Click1(object sender, EventArgs e)
        //{

        //}

        protected void ddlInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            LoadMasterData("InvoiceNo");
            //txtInvoiceNo.ReadOnly = true;
            //txtJobCardNo.ReadOnly = true;
            //txtCustomer.ReadOnly = true;
        }

        protected void ddlJobCardCode_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            HFJobCard.Value = ddlJobCardCode.SelectedValue.ToString();
            LoadMasterData("JobCard");
            HFCustomer.Value = ddlCustCode.SelectedValue.ToString();
            //txtInvoiceNo.ReadOnly = true;
        
        }

        protected void btnSaveInv_Click(object sender, EventArgs e)
        {
            TextBox txtJobCardNox = new TextBox();
            TextBox txtCustomerx = new TextBox();
            txtJobCardNox.Text = HFJobCard.Value;

            txtCustomerx.Text = HFCustomer.Value;
            TextBox[] textBoxes = { txtJobCardNox, txtCustomerx };
            string IQuery = string.Empty;
            if (!MasterValidation(textBoxes)) return;
            else
            {
                if (grl.CodeExists("JobCardPartsDetail", "JobCardCode", HFJobCard.Value.Trim(), Session["DealerCode"].ToString(), " and qty <> recqty"))
                {
                    //SendAlert(); return;
                    myFunc.UserMsg(lblMsg, Color.Red, "Please Isuue All Parts");
                    //lblMSGPop.Text = "Parts have not been issued yet";
                    //PopupControlMSG.ShowOnPageLoad = true;
                    return;
                }

                if (grl.CodeExists("JobCardLubricateDetail", "JobCardCode", HFJobCard.Value.Trim(), Session["DealerCode"].ToString(), " and qty <> recqty"))
                {
                    //SendAlert(); return;
                    myFunc.UserMsg(lblMsg, Color.Red, "Please Isuue All Lubricant");
                    //lblMSGPop.Text = "Lubricant parts have not been issued yet";
                    //PopupControlMSG.ShowOnPageLoad = true;
                    return;
                }
                int iTotalInvoice = 0;
                int iPartsTotal = Convert.ToInt32(txtPartsTotal.Text);
                int iJobTotal = Convert.ToInt32(txtJobsTotal.Text);
                int iLubsTotal = Convert.ToInt32(txtLubTotal.Text);
                int iSubTotal = Convert.ToInt32(txtSubletTotal.Text);
                int iGrossAmt = Convert.ToInt32(txtGrossAmount.Text);

                iTotalInvoice = iPartsTotal + iJobTotal + iLubsTotal + iSubTotal;
                if (iTotalInvoice != iGrossAmt)
                {
                    //SendAlert(); return;
                    myFunc.UserMsg(lblMsg, Color.Red, "Gross Amount is not correct");
                    //lblMSGPop.Text ="Gross Amount is not correct" ;
                    //PopupControlMSG.ShowOnPageLoad = true;
                    return;
                }
                if (txtJobCardType.Text == "Insurance")
                {
                    if (ddlInsCo.SelectedItem.Text == "--Select--")
                    {
                        //lblMsg.Visible = true;
                        //grl.UserMsg(lblMsg, Color.Red, );
                        //return;
                        //lblMSGPop.Text ="Select Insurance company" ;
                        //PopupControlMSG.ShowOnPageLoad = true;
                        myFunc.UserMsg(lblMsg, Color.Red, "Select Insurance company");

                        return;
                    }
                    else
                    {
                        lblMsg.Text = "";
                    }
                    if (ddlInsBranch.SelectedItem.Text == "--Select--" || ddlInsBranch.SelectedItem.Text == "")
                    {

                        myFunc.UserMsg(lblMsg, Color.Red, "Select Insurance company branch");
                        //lblMSGPop.Text = "Select Insurance company branch";
                        //PopupControlMSG.ShowOnPageLoad = true;
                        return;
                    }
                    else
                    {
                        lblMsg.Text = "";
                    }
                    if (grl.IsExist("JobCardCode", HFJobCard.Value.Trim(), "CustomerInvoice", " And InvType IN ('Ins','Dep')"))
                    {

                        myFunc.UserMsg(lblMsg, Color.Red, "insurance invoice can not be edit");
                        //lblMSGPop.Text ="insurance invoice can not be edit" ;
                        //PopupControlMSG.ShowOnPageLoad = true;
                        return;
                    }
                    else
                    {
                        lblMsg.Text = "";
                    }
                }

                //InvoiceEntry();
                if (txtJobCardType.Text.Trim() != "Insurance")
                {
                    InvoiceEntry();
                }
                else if (txtJobCardType.Text == "Insurance")
                {
                    if (Convert.ToDouble(txtDepAmount.Text) > 0)
                    {
                        InvoiceEntry();
                    }
                    if (Convert.ToDouble(txtTotAmtInsComp.Text) > 0)
                    {
                        InsuranceInvoiceEntry();
                    }
                    //if (double.Parse(txtPSTAmount.Text.Trim()) != 0)
                    //{
                    //    if (ObjTrans.BeginTransaction(ref Trans) == true)
                    //    {

                    //        SqlParameter[] PSTINV_param = { new SqlParameter("@DealerCode",SqlDbType.Char,5),new SqlParameter("@PSTInvNo",SqlDbType.Char,8),
                    //                                                new SqlParameter("@PSTInvDate",SqlDbType.DateTime),new SqlParameter("@RefNo",SqlDbType.Char,8) };
                    //        string PSTInvNo = grl.AutoGen("PSTUniqueSalesTaxInvoice", "SSTSalesTaxInvNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                    //        PSTINV_param[0].Value = Session["DealerCode"].ToString(); PSTINV_param[1].Value = PSTInvNo;
                    //        PSTINV_param[2].Value = grl.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
                    //        PSTINV_param[3].Value = txtInvoiceNo.Text.Trim();
                    //        if (myFunc.ExecuteSP_NonQuery("[sp_W2_PSTInvoice_Insert]", PSTINV_param, Trans))
                    //        {
                    //            IQuery = "Update CustomerInvoice set SSTSaleInvoice='" + PSTInvNo + "' " +
                    //                     "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + txtJobCardNo.Text.Trim() + "'";
                    //            myFunc.ExecuteQuery(IQuery, Trans);
                    //        }
                    //    }
                    //    else
                    //    {

                    //    }

                    //}
                }

                if (Create_NDM(HFJobCard.Value.Trim(), "InvoiceCreated") == false)
                {
                    grl.UserMsg(lblMsg, Color.Red, "NDM Data Can not be inserted");
                    ObjTrans.RollBackTransaction(ref Trans);
                    return;
                }

                if (PostServiceFollowUp(HFJobCard.Value.Trim(), "InvoiceCreated") == false)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Post Service FollowUp Data Can not be inserted");
                    ObjTrans.RollBackTransaction(ref Trans);
                    return;
                }
                btnClearForm_Click(null, null);
            }
        }

        protected void btnDeleteInv_Click(object sender, EventArgs e)
        {
            if (ddlInvoiceNo.SelectedIndex == 0)
            {

                myFunc.UserMsg(lblMsg, Color.Red, "Invoice No should not be blank");
                return;
            }

            if (HFJobCard.Value == "")
            {

                myFunc.UserMsg(lblMsg, Color.Red, "JobCard code should not be blank");
                //lblMSGPop.Text = "JobCard code should not be blank ";
                //PopupControlMSG.ShowOnPageLoad = true;
                return;
            }

            //if (grl.CodeExists("CustomerInvoice", "InvoiceNo", txtInvoiceNo.Text) == false) {
            //    myFunc.UserMsg(lblMsg, Color.Red, "Invoice no. not exist");
            //    //lblMSGPop.Text = "Invoice no. not exist ";
            //    //PopupControlMSG.ShowOnPageLoad = true;
            //    return;

            //}

            if (grl.CodeExists("PaymentReceiptDetail", "InvoiceNo", ddlInvoiceNo.SelectedValue.ToString().Trim(), " and InvoiceType='Service'") == true)
            {

                myFunc.UserMsg(lblMsg, Color.Red, "Invoice No. can not be deleted, payment receipt has made");
                //lblMSGPop.Text ="Invoice No. can not be deleted, payment receipt has made" ;
                //PopupControlMSG.ShowOnPageLoad = true;
                return;
            }

            try
            {

                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    string IQuery = "Update CustomerInvoice set GatePass ='',DelFlag='Y'" +
                        //"DelvDate='"+ grl.SaveDate(DateTime.Now.ToString("dd-MM-yyyy")) +"',DelvTime='"+grl.SaveTime(DateTime.Now.ToString("HH:mm"))  +"' " +
                                     "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + HFJobCard.Value.Trim() + "'";
                    IQuery += " Update JobCardMaster Set gatePass='' Where DealerCode='" + Session["DealerCode"].ToString() + "' And JobCardCode='" + HFJobCard.Value.Trim() + "'";
                    myFunc.ExecuteQuery(IQuery, Trans);

                    //Call Method of NDM and PSF For Delete

                    if (Create_NDM(HFJobCard.Value.Trim(), "InvoiceDeleted") == false)
                    {
                        //grl.UserMsg(lblMsg, Color.Red, "NDM Data Can not be Deleted");
                        myFunc.UserMsg(lblMsg, Color.Red, "NDM Data Can not be Deleted");
                        //lblMSGPop.Text = "NDM Data Can not be Deleted";
                        //PopupControlMSG.ShowOnPageLoad = true;
                        ObjTrans.RollBackTransaction(ref Trans);

                        return;
                    }
                    if (PostServiceFollowUp(HFJobCard.Value.Trim(), "InvoiceDeleted") == false)
                    {
                        myFunc.UserMsg(lblMsg, Color.Red, "Post Service FollowUp Data Can not be Deleted");
                        //lblMSGPop.Text = "Post Service FollowUp Data Can not be Deleted";
                        //PopupControlMSG.ShowOnPageLoad = true;
                        ObjTrans.RollBackTransaction(ref Trans);
                        return;
                    }

                    string sqld = "delete from itemsTaxTrx Where DealerCode='" + Session["DealerCode"].ToString() + "' And JobCardCode='" + HFJobCard.Value.Trim() + "'";
                    myFunc.ExecuteQuery(sqld, Trans);

                    ObjTrans.CommittTransaction(ref Trans);
                    //lblMsg.Visible = true;
                    myFunc.UserMsg(lblMsg, Color.Green, "Deleted Successfully Inv. No: " + ddlInvoiceNo.SelectedValue.ToString());
                    Load_ddlInvoiceNo();

                    //lblMSGPop.Text = "Deleted Successfully Inv. No: " + txtInvoiceNo.Text;
                    //PopupControlMSG.ShowOnPageLoad = true;

                    btnClearForm_Click(null, null);
                }
            }
            catch (Exception) { ObjTrans.RollBackTransaction(ref Trans); }
        }

        protected void btnClearForm_Click(object sender, EventArgs e)
        {
            
            //Response.Redirect("CustomerInvoice.aspx");
            txtTRNumber.Text = "";
            txtTRDate.Text = "";
            txtInvoiceDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            ddlInvoiceNo.SelectedIndex = 0;
            ddlJobCardCode.SelectedIndex = 0; txtChassisNo.Text = "";
            txtProductCode.Text = ""; txtEngineNo.Text = "";
            txtProductName.Text = ""; txtColorName.Text = "";

            txtJobType.Text = ""; txtJobTypeDesc.Text = "";

            txtOutKM.Text = ""; txtEngineNo.Text = ""; txtRegNo.Text = "";
             txtColorName.Text = ""; txtGrossAmount.Text = "";

            txtJobsTotal.Text = ""; txtLubTotal.Text = ""; txtPartsTotal.Text = ""; txtSubletTotal.Text = "";

            txtGSTPercent.Text = ""; txttotPartsLub.Text = ""; txttotJobSublet.Text = ""; txtGSTAmount.Text = "";
            txtPSTPercent.Text = ""; txtPSTAmount.Text = "";
            txtSubTotal.Text = ""; txtTotalAmtCustomer.Text = ""; txtPaidCustomer.Text = "";

            txtTotalAmntInsurance.Text = "";            //txtPaidCustomer.Text        = "0";
            txtPaidInsurance.Text = ""; txtVEODAmount.Text = "";
            txtDipriAmount.Text = ""; txtDistOtherParts.Text = "";
            txtDistOtherPercent.Text = "";            //txtOutKM.Text               = "0";

            txtDiscountParts.Text = ""; txtDiscPercentPart.Text = "";
            txtDiscLabor.Text = ""; txtDiscLaborPercent.Text = "";
            txtRemarksUser.Text = ""; txtUserName.Text = "";
            txtTotAmtInsComp.Text = "";
            txtDepAmount.Text = "";
            ddlInsCo.SelectedIndex = 0;
            if (ddlInsBranch.Items.Count > 0)
            {
                ddlInsBranch.SelectedIndex = 0;
            }
            if (lblMsg.ForeColor != Color.Green)
            {
                lblMsg.Text = "";
            }
            HFJobCard.Value = "";
            HFChassisNo.Value = "";
            HFCustomer.Value = "";
            HFEngineNo.Value = "";
            HFRegNo.Value = "";

            FillJobCardDetailGrid();
            ddlCustCode.SelectedIndex = 0;
            //txtInvoiceNo.DataBind();
            //txtJobCardNo.DataBind();

            //txtInvoiceNo.ReadOnly = false;
            //txtJobCardNo.ReadOnly = false;
            //lblJobCardNo.Visible = false;
            //txtJobCardNo.Visible = true;
            //sysFunc.Clearddl(Page);
           // ddlJobCardCode.Enabled = true;
            //Load_ddlJobCardCode();
            
        }

        protected void btnPrintInv_Click(object sender, EventArgs e)
        {
            if (double.Parse(txttotPartsLub.Text.Trim()) > 0 && double.Parse(txttotJobSublet.Text.Trim()) == 0)
            {
                Load_GSTInvoiceRpt();
            }
            else if (double.Parse(txttotJobSublet.Text.Trim()) > 0 && double.Parse(txttotPartsLub.Text.Trim()) == 0)
            {
                Load_PSTInvoiceRpt();
            }
            else if (double.Parse(txttotPartsLub.Text.Trim()) > 0 && double.Parse(txttotJobSublet.Text.Trim()) > 0)
            {
                Load_GSTInvoiceRpt();
                //Load_PSTInvoiceRpt();
            }
        }
        private void Load_PSTInvoiceRpt()
        {
            ReportDocument RD = new ReportDocument();
            Data.DSReports InvoiceDS = new Data.DSReports();

            DataSet ds = new DataSet();
            SqlParameter[] dsMasterParam = { new SqlParameter("@DealerCode",SqlDbType.Char,5),  
                                         
                                         new SqlParameter("@InvoiceNo",SqlDbType.Char,8),
        };
            dsMasterParam[0].Value = Session["DealerCode"].ToString();
            dsMasterParam[1].Value = ddlInvoiceNo.SelectedValue.ToString();
            ds = myFunc.FillDataSet("sp_CustomerInvoiceInfo_Select", dsMasterParam);

            //ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_CustomerInvoiceInfo_Select '" + ConfigurationManager.AppSettings["DealerCode"].ToString() + "','" + Request.QueryString["InvoiceNo"].ToString() + "'");
            InvoiceDS.sp_CustomerInvoiceInfo_Select.Load(ds.CreateDataReader());
            string strJCCode = ds.Tables[0].Rows[0]["JobCardCode"].ToString();

            SqlParameter[] dsJCParam = { new SqlParameter("@DealerCode",SqlDbType.Char,5),  
                                         
                                         new SqlParameter("@JobCardCode",SqlDbType.Char,8),
        };
            dsJCParam[0].Value = Session["DealerCode"].ToString();
            dsJCParam[1].Value = strJCCode;
            ds = myFunc.FillDataSet("sp_InvoiceJobs_Select", dsJCParam);
            //ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_InvoiceJobs_Select '" + ConfigurationManager.AppSettings["DealerCode"].ToString() + "','" + strJCCode + "'");
            InvoiceDS.sp_InvoiceJobs_Select.Load(ds.CreateDataReader());

            ds = myFunc.FillDataSet("sp_InvoiceSublet_Select", dsJCParam);
            //ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_InvoiceSublet_Select '" + ConfigurationManager.AppSettings["DealerCode"].ToString() + "','" + strJCCode + "'");
            InvoiceDS.sp_InvoiceSublet_Select.Load(ds.CreateDataReader());

            string FileRptPath = Server.MapPath("~/Modules/Service/ServiceReports/");
            RD.Load(Server.MapPath("~/Modules/Service/ServiceReports/rptSSTInvoice.rpt"));
            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'PROVISIONAL SALES TAX INVOICE'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";

            RD.SetDataSource(InvoiceDS.sp_CustomerInvoiceInfo_Select.DataSet);

            // create pdf 
            string FilePath = Server.MapPath("~/Download/");
            string FileName = "InvoicePST" + Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;
            //crReportDocument.SetDatabaseLogon("SDMS", "sdms161", "192.168.1.47", "SDMS");
            RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            string URL;
            URL = "../../../Download/OpenPdf.aspx?FileName=" + FileName;

            //URL = FilePath + "OpenPdf.aspx?FileName=" + FileName;
            //txtPartItemDesc.Text = URL;
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //URL = "../../../Download/OpenPdf.aspx?FileName=" + FileName; 
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
        private void Load_GSTInvoiceRpt()
        {
            ReportDocument RD = new ReportDocument();
            Data.DSReports InvoiceDS = new Data.DSReports();

            DataSet ds = new DataSet();
            SqlParameter[] dsMasterParam = { new SqlParameter("@DealerCode",SqlDbType.Char,5),  
                                         
                                         new SqlParameter("@InvoiceNo",SqlDbType.Char,8),
        };
            dsMasterParam[0].Value = Session["DealerCode"].ToString();
            dsMasterParam[1].Value = ddlInvoiceNo.SelectedValue.ToString();
            ds = myFunc.FillDataSet("sp_CustomerInvoiceInfo_Select", dsMasterParam);

            //ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_CustomerInvoiceInfo_Select '" + ConfigurationManager.AppSettings["DealerCode"].ToString() + "','" + Request.QueryString["InvoiceNo"].ToString() + "'");
            InvoiceDS.sp_CustomerInvoiceInfo_Select.Load(ds.CreateDataReader());
            string strJCCode = ds.Tables[0].Rows[0]["JobCardCode"].ToString();

            SqlParameter[] dsJCParam = { new SqlParameter("@DealerCode",SqlDbType.Char,5),  
                                         
                                         new SqlParameter("@JobCardCode",SqlDbType.Char,8),
        };
            dsJCParam[0].Value = Session["DealerCode"].ToString();
            dsJCParam[1].Value = strJCCode;
            ds = myFunc.FillDataSet("sp_GSTInvoiceParts_Select", dsJCParam);
            //ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_InvoiceJobs_Select '" + ConfigurationManager.AppSettings["DealerCode"].ToString() + "','" + strJCCode + "'");
            InvoiceDS.sp_GSTInvoiceParts_Select.Load(ds.CreateDataReader());


            string FileRptPath = Server.MapPath("~/Modules/Service/ServiceReports/");
            RD.Load(Server.MapPath("~/Modules/Service/ServiceReports/rptGSTInvoice.rpt"));
            //RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'PROVISIONAL SALES TAX INVOICE'";
            //RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";

            RD.SetDataSource(InvoiceDS.sp_CustomerInvoiceInfo_Select.DataSet);

            // create pdf 
            string FilePath = Server.MapPath("~/Download/");
            string FileName = "InvoiceGST" + Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;
            //crReportDocument.SetDatabaseLogon("SDMS", "sdms161", "192.168.1.47", "SDMS");
            RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            string URL;
            URL = "../../../Download/OpenPdf.aspx?FileName=" + FileName;

            //URL = FilePath + "OpenPdf.aspx?FileName=" + FileName;
            //txtPartItemDesc.Text = URL;
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //URL = "../../../Download/OpenPdf.aspx?FileName=" + FileName; 
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
       
    }
}