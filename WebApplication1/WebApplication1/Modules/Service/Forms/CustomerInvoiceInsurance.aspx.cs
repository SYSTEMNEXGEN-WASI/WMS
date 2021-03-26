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
using MessagingToolkit.QRCode.Codec;
using CConn;
using Microsoft.ApplicationBlocks.Data;


namespace DXBMS.Modules.Service
{
    public partial class CustomerInvoiceInsurance : System.Web.UI.Page
    {
        SysFunctions myFunc = new SysFunctions();
        SysFunction sysfun = new SysFunction();
        MainBLL objMBLL = new MainBLL();
        ServiceBL objSBL = new ServiceBL();
        SysFunctions grl = new SysFunctions();
        Transaction ObjTrans = new Transaction();
        clsLookUp clslook = new clsLookUp();
        LookUpGeneral Genlookcls = new LookUpGeneral();
        static int btnValue = 0;
        static string dicountlabor, discountpart;


        SqlTransaction Trans;
        //clsLookUp clslook = new clsLookUp();
        DataSet dsTaxDetail, dsJobCardMaster, dsJobCardDetail, dsJobCardParts, dsJobCardConParts, dsJobCardLub, dsJobCardSublet, ds, dsCustomerInvoice, dsJobCardBoutParts;
        Double Total = 0.0,totaltax=0,totalAmtwithTax=0, totalCustax = 0, totalCusAmtwithTax = 0;
        DataTable JobDT, LubDT, PartsDT, SubletDT, BoutDT, dt,CusTax,InsTax;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (Session["dsTaxDetail"] != null) dsTaxDetail = (DataSet)Session["dsTaxDetail"];
            if (Session["CusTax"] != null) CusTax = (DataTable)Session["CusTax"];
            if (Session["InsTax"] != null) CusTax = (DataTable)Session["InsTax"];
            if (IsPostBack)
            {
                Session["txtTotalAmount"] = txtTotalAmtCustomer.Text;
                if (!string.IsNullOrEmpty(Session["LookUpData"].ToString()))
                {
                    SelectedPartDetail(Session["LookUpData"].ToString());
                    Session["LookUpData"] = "";
                }
               // Session["LookUpData"] = "";
            }


            if (!Page.IsPostBack)
            {
               
                Session["LookUpData"] = string.Empty;
                txtInvoiceDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                objMBLL.FillDrp_SP(ddlInsCo, "sp_InsuranceCompanies_select", "InsCompCode", "InsCompDescription", null, true, "--Select--", false, "");
                Load_ddlInvoiceNo();
                Load_ddlJobCardCode();
                Load_ddlCusCode();
                createCusTax();

            }
            Session["LookUpData"] = string.Empty;
            ddlInvoiceNo.Enabled = false;
            ddlInvtype.Enabled = false;
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
                 "AND CI.DealerCode = '" + Session["DealerCode"].ToString() + "'" +
                 "And CI.Delflag<>'Y' ";
            string[] Columns = new string[] { "CI.InvoiceNo", "CONVERT(VARCHAR(10),CI.InvoiceDate,105)", "CI.JobCardCode", "JC.JobCardType" , "CI.InvType" };
            myFunc.GetMultiColumnsDDL(ddlInvoiceNo, Columns, "CustomerInvoice CI ,  JobCardMaster  JC ", WhereClause, "InvoiceNo", " Order by CI.InvoiceNo Desc ", false, false);
        }
        private void Load_ddlCusCode()
        {
            string WhereClause = " DealerCode = '" + Session["DealerCode"].ToString() + "'" ;
 
            string[] Columns = new string[] { "cusCode","CusDesc+' '+FatherHusName","isnull(phone1,isnull(phone2,CellNo))" };
            myFunc.GetMultiColumnsDDL(ddlCustCode, Columns, "Customer ", WhereClause, "cusCode", " Order by CusCode Desc ", false, false);
        }
        private void Load_ddlJobCardCode()
        {
            

            if (ddlInvoiceNo.SelectedIndex == 0)
            {

                //if (ddlInvtype.SelectedIndex == 0)
                //{
                //    string WhereClause1 = "A.DealerCode ='" + Session["DealerCode"].ToString().Trim() + "' " +
                //        //"and A.GatePass = ''"+
                //        " and A.DelFlag = 'N'" +                        
                //        " and A.JobCardCode not in (Select CI.JobCardCode from CustomerInvoice CI where CI.DealerCode = A.DealerCode and CI.JobCardCode = A.JobCardCode and CI.DelFlag = 'N' and CI.InvType = 'Dep')";

                //    string[] Columns1 = new string[] { "A.JobCardCode", "A.RegNo", "A.UserName" };
                //    myFunc.GetMultiColumnsDDL(ddlJobCardCode, Columns1, "JobCardMaster A  ", WhereClause1, "JobCardCode", " order by JobCardCode DESC ", false, false);

                //}else
                //{
                //    string WhereClause1 = "A.DealerCode ='" + Session["DealerCode"].ToString().Trim() + "' " +
                //         //" and A.GatePass = '' "+
                //         " and JobCardType = '012' " +
                //         " and A.DelFlag = 'N'" +
                //         " and A.JobCardCode not in (Select CI.JobCardCode from CustomerInvoice CI where CI.DealerCode = A.DealerCode and CI.JobCardCode = A.JobCardCode and CI.DelFlag = 'N' and CI.InvType = 'Ins')";
                //    //" and A.DealerCode = C.DealerCode " +
                //    //" and A.JobCardCode = C.JobCardCode";

                //    string[] Columns1 = new string[] { "A.JobCardCode", "A.RegNo", "A.UserName" };
                //    myFunc.GetMultiColumnsDDL(ddlJobCardCode, Columns1, "JobCardMaster A ", WhereClause1, "JobCardCode", " order by JobCardCode DESC ", false, false);
                //}

                string WhereClause = "DealerCode ='" + Session["DealerCode"].ToString().Trim() + "' " +
                    "And  Gatepass='' " +
                    "And DelFlag='N' and JobCardType <> '001' " +
                    "and JobCardType <> '002' ";
                //"and JobCardType <> 'Warranty' " +
                //"and JobCardType <> 'Repeated JobCard'";
                string[] Columns = new string[] { "JobCardCode", "RegNo", "UserName" };
                myFunc.GetMultiColumnsDDL(ddlJobCardCode, Columns, "JobCardMaster A ", WhereClause, "JobCardCode", " order by JobCardCode DESC ", false, false);
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
           /*36*/ new SqlParameter("@PSTAmount",SqlDbType.Decimal),             /*37*/ new SqlParameter("@TaxType",SqlDbType.VarChar,10),
           /*38*/ new SqlParameter("@TRNumber",SqlDbType.Char,15),            /*39*/ new SqlParameter("@TRDate",SqlDbType.DateTime)
         };


            INV_param[0].Value = Session["DealerCode"].ToString();
            //INV_param[1].Value = Decission will take on Insert/U.pdate
            INV_param[2].Value = grl.SaveDate(txtInvoiceDate.Text); INV_param[3].Value = txtJobCardCode.Text;
            INV_param[4].Value = ddlCustCode.SelectedValue.ToString().Trim(); INV_param[5].Value = txtGrossAmount.Text;
            INV_param[6].Value = txtGSTPercent.Text == "" ? "0" : txtGSTPercent.Text; INV_param[7].Value = txtGSTAmount.Text;
            INV_param[8].Value = txtSubTotal.Text;
            INV_param[9].Value = txtDiscLabor.Text == "" ? "0" : txtDiscLabor.Text.Trim(); //Value not found
            INV_param[10].Value = txtDiscLaborPercent.Text == "" ? "0" : txtDiscLaborPercent.Text.Trim(); //Value not found
            INV_param[11].Value = "0.00"; INV_param[12].Value = txtTotAmtInsComp.Text;
            INV_param[13].Value = ddlInsCo.SelectedIndex == 0 ? "" : ddlInsCo.SelectedValue;
            INV_param[14].Value = ddlInsBranch.SelectedValue.ToString();
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
            INV_param[37].Value = ddlTaxType.SelectedValue.ToString();
            INV_param[38].Value = txtTRNumber.Text.Trim();
            if(myFunc.SaveDate(txtTRDate.Text) == null){

            INV_param[39].Value = (Object)DBNull.Value;
            }else{
            INV_param[39].Value = myFunc.SaveDate(txtTRDate.Text);
            }
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
            SqlParameter[] IINV_param = {         
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
           /*36*/ new SqlParameter("@PSTAmount",SqlDbType.Decimal),             /*37*/ new SqlParameter("@TaxType",SqlDbType.VarChar,10),
           /*38*/ new SqlParameter("@TRNumber",SqlDbType.Char,15),            /*39*/ new SqlParameter("@TRDate",SqlDbType.DateTime)
         };


            IINV_param[0].Value = Session["DealerCode"].ToString();
            //INV_param[1].Value = Decission will take on Insert/U.pdate
            IINV_param[2].Value = grl.SaveDate(txtInvoiceDate.Text);
            IINV_param[3].Value = txtJobCardCode.Text;
            IINV_param[4].Value = ddlCustCode.SelectedValue.ToString().Trim();
            IINV_param[5].Value = CustomCDBL(txtGrossAmount.Text == "" ? "0" : txtGrossAmount.Text);
            IINV_param[6].Value = CustomCDBL(txtGSTPercent.Text == "" ? "0" : txtGSTPercent.Text);
            IINV_param[7].Value = CustomCDBL(txtGSTAmount.Text == "" ? "0" : txtGSTAmount.Text);
            IINV_param[8].Value = CustomCDBL(txtSubTotal.Text == "" ? "0" : txtSubTotal.Text);
            IINV_param[9].Value = CustomCDBL(txtDiscLabor.Text == "" ? "0" : txtDiscLabor.Text.Trim()); //Value not found
            IINV_param[10].Value = CustomCDBL(txtDiscLaborPercent.Text == "" ? "0" : txtDiscLaborPercent.Text.Trim()); //Value not found
            IINV_param[11].Value = "0.00";
            IINV_param[12].Value = CustomCDBL(txtTotAmtInsComp.Text);
            IINV_param[13].Value = ddlInsCo.SelectedIndex == 0 ? "" : ddlInsCo.SelectedValue;
            IINV_param[14].Value = ddlInsBranch.SelectedValue.ToString();
            IINV_param[15].Value = ddlJobCardCode.SelectedValue.ToString().Trim(); //Value not found
            IINV_param[16].Value = CustomCDBL(txtPaidCustomer.Text   == "" ? "0" : txtPaidCustomer.Text);
            IINV_param[17].Value = CustomCDBL(txtPaidInsurance.Text == "" ? "0" : txtPaidInsurance.Text);
            IINV_param[18].Value = CustomCDBL(txtVEODAmount.Text == "" ? "0" : txtVEODAmount.Text);
            IINV_param[19].Value = CustomCDBL(txtDepAmount.Text == "" ? "0" : txtDepAmount.Text);
            IINV_param[20].Value = ""; //Value not found
            IINV_param[21].Value = ""; //Value not found
            IINV_param[22].Value = "Ins"; //Value not found
            IINV_param[23].Value = ""; //Value not found
            IINV_param[24].Value = "N"; //Value not found
            IINV_param[25].Value = "N";
            IINV_param[26].Value = Session["UserName"].ToString();
            IINV_param[27].Value = grl.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
            IINV_param[28].Value = grl.SaveTime(DateTime.Now.ToString("HH:mm"));
            IINV_param[29].Value = Environment.MachineName; 
            IINV_param[30].Value = CustomCDBL((txtDiscountParts.Text.Trim() == "" ? "0" : txtDiscountParts.Text.Trim()));
            IINV_param[31].Value = CustomCDBL((txtDiscPercentPart.Text.Trim() == "" ? "0" : txtDiscPercentPart.Text.Trim()));
            IINV_param[32].Value = CustomCDBL(txtDistOtherParts.Text == "" ? "0" : txtDistOtherParts.Text);
            IINV_param[33].Value = CustomCDBL(txtDistOtherPercent.Text == "" ? "0" : txtDistOtherPercent.Text);
            IINV_param[34].Value = txtOutKM.Text;
            IINV_param[35].Value = CustomCDBL(txtPSTPercent.Text == "" ? "0" : txtPSTPercent.Text);
            IINV_param[36].Value = CustomCDBL(txtPSTAmount.Text == "" ? "0" : txtPSTAmount.Text);
            IINV_param[37].Value = ddlTaxType.SelectedValue.ToString();
            IINV_param[38].Value = txtTRNumber.Text.Trim();
            if (myFunc.SaveDate(txtTRDate.Text) == null)
            {

                IINV_param[39].Value = (Object)DBNull.Value;
            }
            else
            {
                IINV_param[39].Value = myFunc.SaveDate(txtTRDate.Text);
            }
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
                    IINV_param[1].Value = strAutoInvoiceNo;
                    if (myFunc.ExecuteSP_NonQuery("[sp_W2_CustomerInvoice_Insert]", IINV_param, Trans))
                    {
                        IQuery = "Update JobCardMaster set GatePass ='" + txtJobCardCode.Text.ToString().Trim() + "'," +
                                 "DelvDate='" + grl.SaveDate(DateTime.Now.ToString("dd-MM-yyyy")) + "',DelvTime='" + grl.SaveTime(DateTime.Now.ToString("HH:mm")) + "' " +
                                 "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + txtJobCardCode.Text + "'";
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
                                Session["GSTInvNo"] = GSTInvNo;
                                if (myFunc.ExecuteSP_NonQuery("[sp_W2_GSTInvoice_Insert]", GSTINV_param, Trans))
                                {
                                    //if (ddlInvtype.SelectedItem.Value == "Depriciation")
                                    //{
                                    //    IQuery = "Update CustomerInvoice set DepSaleInvoice='" + GSTInvNo + "' " +
                                    //         "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + ddlJobCardCode.SelectedValue.ToString().Trim() + "'";
                                    //}else
                                    //{
                                    IQuery = "Update CustomerInvoice set SaleInvoice='" + GSTInvNo + "' " +
                                         "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + txtJobCardCode.Text + "' and InvoiceNo = '" + strAutoInvoiceNo + "'";
                                    //}
                                    myFunc.ExecuteQuery(IQuery, Trans);
                                }
                            }

                            /////Update PST Invoice Table
                            if (grl.CodeExists("PSTInvoice", "RefNo", strAutoInvoiceNo) == false)
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
                                                 "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + txtJobCardCode.Text + "' and InvoiceNo = '" + strAutoInvoiceNo + "'";
                                        myFunc.ExecuteQuery(IQuery, Trans);
                                    }
                                }

                            }


                            if (double.Parse(txtGSTAmount.Text.Trim()) != 0)
                            {
                                SaveItemSalesTaxIns(Session["GSTInvNo"].ToString(), strAutoInvoiceNo);
                            }

                        }
                        else
                        {
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
            }
            catch (Exception ex)
            {

                myFunc.UserMsg(lblMsg, Color.Green, ex.Message);
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
            string NewInvNo = "";
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

            double totalAmtForCustomer = Convert.ToDouble(txtTotalAmtCustomer.Text) -
                (Convert.ToDouble(txtDiscountParts.Text.Trim() == "" ? "0" : txtDiscountParts.Text.Trim()) +
                    Convert.ToDouble(txtDiscLabor.Text == "" ? "0" : txtDiscLabor.Text.Trim()));

            INV_param[0].Value = Session["DealerCode"].ToString();
            //INV_param[1].Value = Decission will take on Insert/U.pdate
            INV_param[2].Value = grl.SaveDate(txtInvoiceDate.Text);
            INV_param[3].Value = HFJobCard.Value.Trim();
            INV_param[4].Value = ddlCustCode .SelectedValue.ToString().Trim();
            INV_param[5].Value = CustomCDBL(txtGrossAmount.Text);
            INV_param[6].Value = CustomCDBL(txtGSTPercent.Text == "" ? "0" : txtGSTPercent.Text);
            INV_param[7].Value = CustomCDBL(lblCusTaxAmt.Text);
            INV_param[8].Value = CustomCDBL(txtSubTotal.Text);
            INV_param[9].Value =  CustomCDBL(txtDiscLabor.Text == "" ? "0" : txtDiscLabor.Text.Trim()); //Value not found
            INV_param[10].Value = CustomCDBL(txtDiscLaborPercent.Text == "" ? "0" : txtDiscLaborPercent.Text.Trim()); //Value not found
            INV_param[11].Value = CustomCDBL(totalAmtForCustomer);
            INV_param[12].Value = CustomCDBL(txtTotalAmntInsurance.Text);
            INV_param[13].Value = ddlInsCo.SelectedIndex == 0 ? "" : ddlInsCo.SelectedValue;
            INV_param[14].Value = ddlInsBranch.SelectedValue.ToString().Trim();
            INV_param[15].Value = ddlJobCardCode.SelectedValue.ToString().Trim(); //Value not found
            INV_param[16].Value = CustomCDBL(txtPaidCustomer.Text);
            INV_param[17].Value = CustomCDBL(txtPaidInsurance.Text);
            INV_param[18].Value = CustomCDBL(txtVEODAmount.Text);
            INV_param[19].Value = CustomCDBL(txtDepAmount.Text == "" ? "0" : txtDepAmount.Text);
            INV_param[20].Value = ""; //Value not found
            INV_param[21].Value = ""; //Value not found
            INV_param[22].Value = "Dep"; //Value not found
            INV_param[23].Value = ""; //Value not found
            INV_param[24].Value = "N"; //Value not found
            INV_param[25].Value = "N";
            INV_param[26].Value = Session["UserName"].ToString();
            INV_param[27].Value = grl.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
            INV_param[28].Value = grl.SaveTime(DateTime.Now.ToString("HH:mm"));
            INV_param[29].Value = Environment.MachineName;
            INV_param[30].Value = CustomCDBL(txtDiscountParts.Text.Trim() == "" ? "0" : txtDiscountParts.Text.Trim());
            INV_param[31].Value = CustomCDBL(txtDiscPercentPart.Text.Trim() == "" ? "0" : txtDiscPercentPart.Text.Trim());
            INV_param[32].Value = CustomCDBL(txtDistOtherParts.Text);
            INV_param[33].Value = CustomCDBL(txtDistOtherPercent.Text);
            INV_param[34].Value = txtOutKM.Text;
            INV_param[35].Value = CustomCDBL(txtPSTPercent.Text == "" ? "0" : txtPSTPercent.Text);
            INV_param[36].Value = CustomCDBL(txtPSTAmount.Text);
            INV_param[37].Value = txtTaxType.Text.Trim() ;
            INV_param[38].Value = txtTRNumber.Text.Trim();
            if (txtTRDate.Text == "")
            {
                INV_param[39].Value = (Object)DBNull.Value;
            }
            else
            {
                INV_param[39].Value = myFunc.SaveDate(txtTRDate.Text);
            }
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

                        NewInvNo = grl.AutoGen("UniqueInvoiceNo", "InvoiceNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                        INV_param[1].Value = NewInvNo;

                        ViewState["InvoiceNo"] = NewInvNo;

                        if (myFunc.ExecuteSP_NonQuery("[sp_W2_CustomerInvoice_Insert]", INV_param, Trans))
                        {
                            IQuery = "Update JobCardMaster set GatePass ='" + txtJobCardCode.Text.Trim() + "'," +
                                     "DelvDate='" + grl.SaveDate(DateTime.Now.ToString("dd-MM-yyyy")) + "',DelvTime='" + grl.SaveTime(DateTime.Now.ToString("HH:mm")) + "' " +
                                     "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + txtJobCardCode.Text.Trim() + "'";
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
                                                 "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + txtJobCardCode.Text + "' and InvoiceNo = '" + NewInvNo + "'";
                                        myFunc.ExecuteQuery(IQuery, Trans);
                                    }

                                    // save item tax into table
                                    SaveItemSalesTax(GSTInvNo, NewInvNo);

                                }

                                ///Update PST Invoice Table
                                //Commented because Dep Customer has no Pst charges
                                //if (double.Parse(txtPSTAmount.Text.Trim()) != 0)
                                //{
                                //    SqlParameter[] PSTINV_param = {
                                //        new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                //        new SqlParameter("@PSTInvNo",SqlDbType.Char,8),
                                //        new SqlParameter("@PSTInvDate",SqlDbType.DateTime),
                                //        new SqlParameter("@RefNo",SqlDbType.Char,8)
                                //    };
                                //    string PSTInvNo = grl.AutoGen("PSTUniqueSalesTaxInvoice", "SSTSalesTaxInvNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));

                                //    PSTINV_param[0].Value = Session["DealerCode"].ToString();
                                //    PSTINV_param[1].Value = PSTInvNo;
                                //    PSTINV_param[2].Value = grl.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
                                //    PSTINV_param[3].Value = NewInvNo.Trim();

                                //    if (myFunc.ExecuteSP_NonQuery("[sp_W2_PSTInvoice_Insert]", PSTINV_param, Trans))
                                //    {
                                //        IQuery = "Update CustomerInvoice set SSTSaleInvoice='" + PSTInvNo + "' " +
                                //                 "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + txtJobCardCode.Text + "' and InvoiceNo = '" + NewInvNo + "'";
                                //        myFunc.ExecuteQuery(IQuery, Trans);
                                //    }
                                //}

                            }
                        }
                        else { myFunc.UserMsg(lblMsg, Color.Red, "Can't save"); }
                    }
                    else if (Decission == "Update")
                    {
                        INV_param[1].Value = ddlInvoiceNo.SelectedValue.ToString().Trim();

                        if (myFunc.ExecuteSP_NonQuery("[sp_W2_CustomerInvoice_Update]", INV_param, Trans))
                        {

                            if (grl.GetStringValuesAgainstCodes("InvoiceNo", ddlInvoiceNo.SelectedValue.ToString().Trim(), "SaleInvoice", "CustomerInvoice", Session["DealerCode"].ToString()) == "")
                            {
                                ///Update GST Invoice Table
                                if (double.Parse(txtGSTAmount.Text.Trim()) != 0)
                                {
                                    SqlParameter[] GSTINV_param = {
                                        new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                        new SqlParameter("@GSTInvNo",SqlDbType.Char,8),
                                        new SqlParameter("@GSTInvDate",SqlDbType.DateTime),
                                        new SqlParameter("@RefNo",SqlDbType.Char,8)
                                    };
                                    string GSTInvNo = grl.AutoGen("UniqueSalesTaxInvoice", "SalesTaxInvNO", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));

                                    GSTINV_param[0].Value = Session["DealerCode"].ToString();
                                    GSTINV_param[1].Value = GSTInvNo;
                                    GSTINV_param[2].Value = grl.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
                                    GSTINV_param[3].Value = ddlInvoiceNo.SelectedValue.ToString().Trim();

                                    if (myFunc.ExecuteSP_NonQuery("[sp_W2_GSTInvoice_Insert]", GSTINV_param, Trans))
                                    {
                                        IQuery = "Update CustomerInvoice set SaleInvoice='" + GSTInvNo + " " +
                                                 "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + ddlJobCardCode.SelectedValue.ToString().Trim() + "'";
                                        myFunc.ExecuteQuery(IQuery, Trans);
                                    }
                                }
                            }

                            if (grl.GetStringValuesAgainstCodes("InvoiceNo", ddlInvoiceNo.SelectedValue.ToString(), "SSTSaleInvoice", "CustomerInvoice", Session["DealerCode"].ToString()) == "")
                            {
                                ///Update PST Invoice Table
                                if (double.Parse(txtPSTAmount.Text.Trim()) != 0)
                                {
                                    SqlParameter[] PSTINV_param = {
                                        new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                        new SqlParameter("@PSTInvNo",SqlDbType.Char,8),
                                        new SqlParameter("@PSTInvDate",SqlDbType.DateTime),
                                        new SqlParameter("@RefNo",SqlDbType.Char,8)
                                    };

                                    string PSTInvNo = grl.AutoGen("PSTUniqueSalesTaxInvoice", "SSTSalesTaxInvNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));

                                    PSTINV_param[0].Value = Session["DealerCode"].ToString();
                                    PSTINV_param[1].Value = PSTInvNo;
                                    PSTINV_param[2].Value = grl.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
                                    PSTINV_param[3].Value = ddlInvoiceNo.SelectedValue.ToString().Trim();

                                    if (myFunc.ExecuteSP_NonQuery("[sp_W2_PSTInvoice_Insert]", PSTINV_param, Trans))
                                    {
                                        IQuery = "Update CustomerInvoice set SSTSaleInvoice='" + PSTInvNo + "' " +
                                                 "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode='" + ddlJobCardCode.SelectedValue.ToString().Trim() + "'";
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
                    if (ObjTrans.CommittTransaction(ref Trans))
                    {
                        HFJobCard.Value = "InvoiceCreated";
                    }
                    //hdPrint.Value = "";
                    //this.ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ConfirmApproval();", true);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "return confirm('confirm?');", true);
                    // Page.ClientScript.RegisterStartupScript(this.GetType(), "myClick", "window.open('../../Service/ServiceReports/InvoiceNormalView.aspx?JobCardCode=" + txtJobCardNo.Text.Trim() + "&GSTAmt=" + txtGSTAmount.Text + "&SSTAmt=" + txtPSTAmount.Text + "','_blank'," + General.PopUpSize + ");", true);

                    //lblMsg.Visible = true;
                    myFunc.UserMsg(lblMsg, Color.Green, "Record Saved Successfully: " + NewInvNo);

                    //lblMSGPop.Text = "Record Saved Successfully: " + txtInvoiceNo.Text;
                    //PopupControlMSG.ShowOnPageLoad = true;
                }
            }
            catch (Exception er)
            {

                myFunc.UserMsg(lblMsg, Color.Red, "Error: " + er.Message);
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
            if (myFunc.ExecuteSP_NonQuery("sp_CRM_PostSales_NextDueMaintenance_OnJobCardUpdate", MDNINV_param))
            {
                return true;
            }
            else
            {
                return false;
            }
            
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
            MDNINV_param[3].Value = grl.SaveDate(txtInvoiceDate .Text );
            MDNINV_param[4].Value = Session["UserName"].ToString();
            MDNINV_param[5].Value = Action;
            if (myFunc.ExecuteSP_NonQuery("sp_CRM_Post_PostServiceFollowup_OnJobCardUpdate", MDNINV_param))
            {
                return true;
            }
            else
            {
                return false;
            }
            //return true;
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

        private void FillJobCardDetailGrid(string strClearCondition)
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
            dsJobCardBoutParts = new DataSet();
            dsJobCardBoutParts = myFunc.FillDataSet("sp_W2_JobCard_BoutPartsDetail_Select", dsParam);
            //if (dsJobCardConParts.Tables[0].Rows.Count == 0) dsJobCardConParts.Tables[0].Rows.Add(dsJobCardConParts.Tables[0].NewRow());
            ViewState["BoutParts"] = dsJobCardBoutParts.Tables[0]; gvJobCardPartsBought.DataSource = dsJobCardBoutParts; gvJobCardPartsBought.DataBind();
            Session["BoutPartsDataTables"] = dsJobCardBoutParts.Tables[0];
            ViewState["Bout"] = dsJobCardBoutParts.Tables[0];
            FillTaxGrid(strClearCondition);
            
            
        }
        protected void FillTaxGrid(string strClearCondition)
        {
            SqlParameter[] dsParamInv = {  new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                           new SqlParameter("@InvoiceNo",SqlDbType.Char,8)};
            dsParamInv[0].Value = Session["DealerCode"].ToString();
            dsParamInv[1].Value = ddlInvoiceNo.SelectedValue.ToString().Trim();//(txtInvoiceNo.Text.Trim() == "" ? null : txtInvoiceNo.Text.Trim());
            dsCustomerInvoice = new DataSet();
            dsCustomerInvoice = myFunc.FillDataSet("sp_W2_CustomerInvoice_Select", dsParamInv);

            SqlParameter[] dsParams = {
                                        new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                        new SqlParameter("@JobCardCode",SqlDbType.Char,8),
                                       new SqlParameter("@InvType",SqlDbType.Char,10)
                                     };
            dsParams[0].Value = Session["DealerCode"].ToString();
            dsParams[1].Value = (HFJobCard.Value == "" ? null : HFJobCard.Value.Trim());
            if (strClearCondition == "InvoiceNo")
            {
                dsParams[2].Value = dsCustomerInvoice.Tables[0].Rows[0]["TaxType"].ToString();
            }
            else
            {
                dsParams[2].Value = txtTaxType.Text.Trim();
            }

            dsTaxDetail = new DataSet();
            dsTaxDetail = myFunc.FillDataSet("SP_TaxDetail_NEW1", dsParams);
            if (dsTaxDetail.Tables[0].Rows.Count == 0)
            {
                dsTaxDetail.Tables[0].Rows.Add(dsTaxDetail.Tables[0].NewRow());
               // createInsTax();
               // createInsTax();

                gvTax.DataSource = dsTaxDetail; gvTax.DataBind();
            }
            else
            {
                Session["dsTaxDetail"] = dsTaxDetail;
                object gst = dsTaxDetail.Tables[0].Compute("Sum(GSTAmount)", string.Empty);
                object fgst = dsTaxDetail.Tables[0].Compute("Sum(FGSTAmount)", string.Empty);
                object extax = dsTaxDetail.Tables[0].Compute("Sum(ExTaxAmount)", string.Empty);

                double temp = gst == DBNull.Value ? 0 : Convert.ToDouble(gst);
                double temp1 = fgst == DBNull.Value ? 0 : Convert.ToDouble(fgst);
                double temp2 = extax == DBNull.Value ? 0 : Convert.ToDouble(extax);

                //float temp = dsTaxDetail.Tables[0].AsEnumerable().Sum(x => x.Field<float>("GSTAmount"));
                //float temp1 = dsTaxDetail.Tables[0].AsEnumerable().Sum(x => x.Field<float>("FGSTAmount"));
                //float temp2 = dsTaxDetail.Tables[0].AsEnumerable().Sum(x => x.Field<float>("ExTaxAmount"));

                double taxTotal = temp + temp2 + temp1;

                if (taxTotal > 0)
                {
                    createInsTax();

                    InsInsTaxGVD();
                }
                //else
                //{
                //    dsTaxDetail.Clear();
                //    dsTaxDetail.Tables[0].Rows.Add(dsTaxDetail.Tables[0].NewRow());
                //    gvTax.DataSource = dsTaxDetail; gvTax.DataBind();
                //}
            }
            }
        protected void SelectedPartDetail(string item)
        {
            try
            {
                if (btnValue == 1)
                {
                    //LoadReportData();
                    txtJobCardCode.Text = item;
                    Session["total"] = "0";
                    HFJobCard.Value = item;
                    LoadMasterData("JobCard", item);
                    HFCustomer.Value = ddlCustCode.SelectedValue.ToString();
                  
                }
               else if (btnValue == 2)

                {
                   
                    ddlInvoiceNo.SelectedValue = item;
                    LoadReportData();
                    
                    LoadMasterDataInv("InvoiceNo", item);
                }
                else if(btnValue==3)
                     {
                    ddlCustCode.SelectedValue = item;
                }

            }
            catch (Exception ex)
            {
                sysfun.UserMsg(lblMsg, Color.Red, ex.Message);
            }
        }
        protected void LoadMasterData(string strClearCondition,string jobcard)
        {
            ClearMasterData(strClearCondition);
            if (strClearCondition == "InvoiceNo")
            {
                //txtJobCardNo.Text = myFunc.GetJobCardNoFromCustomerInvNo(txtInvoiceNo.Text.Trim());
                HFJobCard.Value = myFunc.GetJobCardNoFromCustomerInvNo(ddlInvoiceNo.SelectedValue.ToString().Trim() , Session["DealerCode"].ToString());
                //lblJobCardNo.Text = HFJobCard.Value;
                HFCustomer.Value = myFunc.GetCustomerCodeByJobCardNo(HFJobCard.Value , Session["DealerCode"].ToString());
                ddlCustCode.SelectedValue = HFCustomer.Value;
                ddlJobCardCode.Visible = false;
                ddlJobCardCode.Enabled = false;
                Load_ddlJobCardCode();
                txtJobCardCode.Visible = true;
            }
           


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
                if (strClearCondition == "InvoiceNo")
                {
                    txtJobCardCode.Text = dsJobCardMaster.Tables[0].Rows[0]["JobCardCode"].ToString();
                }
                txtChassisNo.Text = dsJobCardMaster.Tables[0].Rows[0]["ChassisNo"].ToString();
                txtProductCode.Text = dsJobCardMaster.Tables[0].Rows[0]["ProdCode"].ToString();
                txtJobCardType.Text = dsJobCardMaster.Tables[0].Rows[0]["JobTypeDesc"].ToString().Trim();

                ds = new DataSet(); grl.CodeExists("vehicle", "ProdCode='" + txtProductCode.Text.Trim() + "'", ref ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtProductName.Text = ds.Tables[0].Rows[0]["ProdDesc"].ToString();
                    txtColorName.Text = ds.Tables[0].Rows[0]["Color"].ToString();
                }
                else txtProductCode.Text = txtColorName.Text = "";

                txtRemarksUser.Text = dsJobCardMaster.Tables[0].Rows[0]["Remarks"].ToString();
                txtUserName.Text = dsJobCardMaster.Tables[0].Rows[0]["UserName"].ToString();
                txtBillPayMode.Text = dsJobCardMaster.Tables[0].Rows[0]["PaymentMode"].ToString();

                txtJobType.Text = dsJobCardMaster.Tables[0].Rows[0]["JobTypeCode"].ToString().Trim();
                txtJobTypeDesc.Text = dsJobCardMaster.Tables[0].Rows[0]["JobTypeDesc"].ToString().Trim();

                txtOutKM.Text = dsJobCardMaster.Tables[0].Rows[0]["KM"].ToString().Trim();
                txtEngineNo.Text = dsJobCardMaster.Tables[0].Rows[0]["EngineNo"].ToString();
                txtRegNo.Text = dsJobCardMaster.Tables[0].Rows[0]["RegNo"].ToString();
                if (strClearCondition == "InvoiceNo")
                {
                    ddlCustCode.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["InvoiceCustomer"].ToString();
                }
                else
                {
                    ddlCustCode.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["CusCode"].ToString();
                }
                
                //txtCustomerName.Text = grl.GetStringValuesAgainstCodes("cusCode", ddlCustCode.SelectedValue.ToString().Trim(), "CusDesc+' '+FatherHusName", "Customer");
                txtColorName.Text = dsJobCardMaster.Tables[0].Rows[0]["Color"].ToString();

                txtGrossAmount.Text = CustomCDBL((Convert.ToDouble(dsJobCardMaster.Tables[0].Rows[0]["JobsTotal"].ToString()) +
                                                Convert.ToDouble(dsJobCardMaster.Tables[0].Rows[0]["PartsTotal"].ToString()) +
                                                Convert.ToDouble(dsJobCardMaster.Tables[0].Rows[0]["BoutTotal"].ToString()) +
                                                Convert.ToDouble(dsJobCardMaster.Tables[0].Rows[0]["SubletTotal"].ToString()) +
                                                Convert.ToDouble(dsJobCardMaster.Tables[0].Rows[0]["LubPartsTotal"].ToString())).ToString()).ToString();

                txtJobsTotal.Text = Math.Round(Convert.ToDouble(dsJobCardMaster.Tables[0].Rows[0]["JobsTotal"].ToString()),2).ToString("");
                txtLubTotal.Text =   Math.Round(Convert.ToDouble(dsJobCardMaster.Tables[0].Rows[0]["LubPartsTotal"].ToString()),2).ToString("");
                txtPartsTotal.Text = Math.Round(Convert.ToDouble(dsJobCardMaster.Tables[0].Rows[0]["PartsTotal"].ToString()),2).ToString("");
                txtBoutTotal.Text=   Math.Round(Convert.ToDouble(dsJobCardMaster.Tables[0].Rows[0]["BoutTotal"].ToString()),2).ToString("");
                txtSubletTotal.Text =Math.Round(Convert.ToDouble(dsJobCardMaster.Tables[0].Rows[0]["SubletTotal"].ToString()),2).ToString("");
                hdnTaxCode.Value = dsJobCardMaster.Tables[0].Rows[0]["hdnTaxCode"].ToString();
                hdnTaxType.Value = dsJobCardMaster.Tables[0].Rows[0]["hdnTaxType"].ToString();
                txtTaxType.Text = dsJobCardMaster.Tables[0].Rows[0]["hdnTaxType"].ToString();

                //Calculate GST Uzair
                SqlParameter[] dsParamInv = {  new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                               new SqlParameter("@InvoiceNo",SqlDbType.Char,8)};
                dsParamInv[0].Value = Session["DealerCode"].ToString();
                dsParamInv[1].Value = ddlInvoiceNo.SelectedValue.ToString().Trim();//(txtInvoiceNo.Text.Trim() == "" ? null : txtInvoiceNo.Text.Trim());
                dsCustomerInvoice = new DataSet();
                dsCustomerInvoice = myFunc.FillDataSet("sp_W2_CustomerInvoice_Select", dsParamInv);

                SqlParameter[] dsParam = {  new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                            new SqlParameter("@JobCardCode",SqlDbType.Char,8),
                                            new SqlParameter("@InvType",SqlDbType.Char,10)
                                         };
                dsParam[0].Value = Session["DealerCode"].ToString();
                dsParam[1].Value = (HFJobCard.Value == "" ? null : HFJobCard.Value.Trim());
                if (strClearCondition == "InvoiceNo")
                {
                    dsParam[2].Value = dsCustomerInvoice.Tables[0].Rows[0]["TaxType"].ToString();
                }
                else
                {
                    dsParam[2].Value = txtTaxType.Text.Trim();
                }

                dsTaxDetail = myFunc.FillDataSet("SP_TaxDetail_NEW1", dsParam);
                FillJobCardDetailGrid(strClearCondition);
                InsCusTaxGVD();

                DataTable dt = InsTax;

                double taxAmount = 0,FGSTAmount=0,FTAmount=0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    taxAmount = taxAmount + CustomCDBL((Convert.ToDecimal(InsTax.Rows[i]["GSTAmount"]) +
                                                Convert.ToDecimal(InsTax.Rows[i]["FGSTAmount"]) +
                                                Convert.ToDecimal(InsTax.Rows[i]["ExTaxAmount"])));
                    FGSTAmount = FGSTAmount + CustomCDBL(Convert.ToDecimal(InsTax.Rows[i]["FGSTAmount"]));
                    FTAmount = FTAmount + CustomCDBL(Convert.ToDecimal(InsTax.Rows[i]["ExTaxAmount"]));

                }

                txtTaxAmount.Text = taxAmount.ToString();
                if (dsTaxDetail.Tables[0].Rows.Count > 0)
                {
                    txtGSTPercent.Text = InsTax.Rows[0]["GSTPerc"].ToString();
                    txtFGSTPerc.Text = InsTax.Rows[0]["FGSTPerc"].ToString();
                    txtFTPerc.Text = InsTax.Rows[0]["ExTaxPerc"].ToString();
                }
             
                txtFGSTAmount.Text = FGSTAmount.ToString();
                txtFTAmount.Text = FTAmount.ToString();
                //double GSTPercent = double.Parse(grl.GetStringValuesAgainstCodes1("DealerCode", Session["DealerCode"].ToString(), "GST", "BaseRate"));
                //txtGSTPercent.Text = GSTPercent.ToString();

                double totPartsLub = CustomCDBL(txtLubTotal.Text == "" ? 0 : double.Parse(txtLubTotal.Text))
                                    + CustomCDBL(txtPartsTotal.Text == "" ? 0 : double.Parse(txtPartsTotal.Text));
                txttotPartsLub.Text = totPartsLub.ToString();
               
                CalculateSubletPSTTax();
                

                double GSTPer = txtGSTPercent.Text.Trim() == "" ? 0 : double.Parse(txtGSTPercent.Text);
                txtGSTAmount.Text = Math.Round(totPartsLub * (GSTPer / 100), 2).ToString();

               

                // Calculate Item wise Tax
               // CalculateSalesTax();

               double ttx = Convert.ToDouble(txtTaxAmount.Text) ;
                txtGSTAmount.Text = Math.Round(ttx, 2).ToString();

                txtSubTotal.Text = txtGrossAmount.Text == "" ? "0" :
                                                           CustomCDBL(
                                                              double.Parse(txtGrossAmount.Text)
                                                           + (txtGSTAmount.Text == "" ? 0 : double.Parse(txtGSTAmount.Text))
                                                           + (txtPSTAmount.Text == "" ? 0 : double.Parse(txtPSTAmount.Text))
                                                            + (lblCusTaxAmt.Text == "" ? 0 : double.Parse(lblCusTaxAmt.Text))
                                                           ).ToString();

                txtTotalAmtCustomer.Text = txtSubTotal.Text == "" ? "0" :
                                                                    CustomCDBL(
                                                                      double.Parse(txtSubTotal.Text)
                                                                      + (txtDiscountParts.Text == "" ? 0 : double.Parse(txtDiscountParts.Text))
                                                                      + (txtDiscLabor.Text == "" ? 0 : double.Parse(txtDiscLabor.Text))
                                                                      ).ToString();

        
                txtDepAmount.Text = Total.ToString();

                txtPaidCustomer.Text = "0";
                // Some fields are not found due to calculation problem occured.
                //NetAmount Customer =(Convert.ToDecimal(txtGrossAmount.Text)) - Part Discount + GST Amount - Labor Discount
                txtTotalAmntInsurance.Text = "0"; 
                
                txtPaidInsurance.Text = "0"; txtVEODAmount.Text = "0";
                txtDipriAmount.Text = "0"; txtDistOtherParts.Text = "0";
                txtDistOtherPercent.Text = "0";            //txtOutKM.Text               = "0";


                
          
                //txtTotAmtInsComp.Text = ((txtSubTotal.Text == "" ? 0 : double.Parse(txtSubTotal.Text)) - (txtDepAmount.Text == "" ? 0 : double.Parse(txtDepAmount.Text))).ToString();
                if (txtJobCardType.Text == "INSURANCE" && strClearCondition == "JobCard")
                {
                   
                    double tax = 0;
                    taxAmount = 0;
                   double cusTaxAmt=0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        tax = CustomCDBL((Convert.ToDecimal(InsTax.Rows[i]["GSTPerc"]) +
                                                 Convert.ToDecimal(InsTax.Rows[i]["FGSTPerc"]) +
                                                  Convert.ToDecimal(InsTax.Rows[i]["ExTaxPerc"])));
                        taxAmount = taxAmount + CustomCDBL(Convert.ToDouble(InsTax.Rows[i]["DepAmount"]) * tax) / 100;
                    }
                    //taxAmount = (Convert.ToDouble(txtDepAmount.Text) * tax) / 100;
                    //txtTotAmtInsComp.Text = ((txtSubTotal.Text == "" ? 0 : double.Parse(txtSubTotal.Text)) - (txtDepAmount.Text == "" ? 0 : double.Parse(txtDepAmount.Text))).ToString();1
                    //txtTotalAmtCustomer.Text = (Convert.ToDouble(txtTotalAmtCustomer.Text) - Convert.ToDouble(txtTotAmtInsComp.Text)).ToString();1
                    //txtTotalAmtCustomer.Text = (Convert.ToDouble(txtDepAmount.Text) + Convert.ToDouble(txtTaxAmount.Text)).ToString();2
                    double totalAmtCustomer;
                    for (int i = 0; i < CusTax.Rows.Count; i++)
                    {
                        cusTaxAmt = cusTaxAmt+ CustomCDBL((Convert.ToDecimal(CusTax.Rows[i]["GSTAmount"]) +
                                                 Convert.ToDecimal(CusTax.Rows[i]["FGSTAmount"]) +
                                                  Convert.ToDecimal(CusTax.Rows[i]["ExTaxAmount"])));
                       
                    }
                    double cuslub = 0;
                    for (int i = 0; i < CusTax.Rows.Count; i++)
                    {
                        if (CusTax.Rows[i]["Type"].ToString() == "L")
                        {
                            cuslub = cuslub + CustomCDBL((Convert.ToDecimal(CusTax.Rows[i]["Price"])) *
                                                Convert.ToDecimal(CusTax.Rows[i]["Qty"]));
                        }



                    }


                    totalAmtCustomer = Math.Round(Convert.ToDouble(txtDepAmount.Text)+ cuslub + cusTaxAmt,2);

                    txtTotalAmtCustomer.Text = totalAmtCustomer.ToString();

                    txtTotAmtInsComp.Text = CustomCDBL((txtSubTotal.Text == "" ? 0 : double.Parse(txtSubTotal.Text))
                        
                        - CustomCDBL(totalAmtCustomer.ToString() == "" ? 0 : double.Parse(totalAmtCustomer.ToString()))
                        
                        ).ToString();

                }
                if (txtJobCardType.Text == "INSURANCE")
                {
                    
                    
                    lblTaxHeader.Text = "Insurance Company Taxes";
                   
                    txtDiscLabor.ReadOnly = true;
                    txtDiscountParts.ReadOnly = true;
                    ddlInsCo.Enabled = true;
                    ddlInsBranch.Enabled = true;
                    ds = new DataSet(); grl.CodeExists("CustomerEstimateMaster", "CustomerEstimateCode='" + dsJobCardMaster.Tables[0].Rows[0]["EstimateCode"].ToString() + "'", ref ds);
                    
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlInsCo.Text = ds.Tables[0].Rows[0]["InsCompCode"].ToString();

                        ddlInsCo_SelectedIndexChanged(ddlInsCo, null);

                        ddlInsBranch.Text = ds.Tables[0].Rows[0]["InsBranchCode"].ToString();
                    }else if (strClearCondition == "InvoiceNo")
                    {
                        ddlInsCo.Text = dsCustomerInvoice.Tables[0].Rows[0]["InsCode"].ToString();

                        ddlInsCo_SelectedIndexChanged(ddlInsCo, null);

                        ddlInsBranch.Text = dsCustomerInvoice.Tables[0].Rows[0]["BranchCode"].ToString();
                    }

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

        protected void LoadMasterDataInv(string strClearCondition, string jobcard)
        {
            ClearMasterData(strClearCondition);
            if (strClearCondition == "InvoiceNo")
            {
                //txtJobCardNo.Text = myFunc.GetJobCardNoFromCustomerInvNo(txtInvoiceNo.Text.Trim());
                HFJobCard.Value = myFunc.GetJobCardNoFromCustomerInvNo(ddlInvoiceNo.SelectedValue.ToString().Trim(), Session["DealerCode"].ToString());
                //lblJobCardNo.Text = HFJobCard.Value;
                HFCustomer.Value = myFunc.GetCustomerCodeByJobCardNo(HFJobCard.Value, Session["DealerCode"].ToString());
                ddlCustCode.SelectedValue = HFCustomer.Value;
                ddlJobCardCode.Visible = false;
                ddlJobCardCode.Enabled = false;
                Load_ddlJobCardCode();
                txtJobCardCode.Visible = true;
            }



            SqlParameter[] dsMasterParam = { new SqlParameter("@DealerCode",SqlDbType.Char,5),  new SqlParameter("@JobCardCode",SqlDbType.Char,8),
                                         new SqlParameter("@RegNo",SqlDbType.VarChar,50),   new SqlParameter("@EngineNo",SqlDbType.VarChar,50),
                                         new SqlParameter("@ChassisNo",SqlDbType.VarChar,50),new SqlParameter("@CusCode",SqlDbType.VarChar,50)
        };
            dsMasterParam[0].Value = Session["DealerCode"].ToString();
            if (HFJobCard.Value == "" & txtRegNo.Text == "" & txtEngineNo.Text == null & txtChassisNo.Text == ""
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
                if (strClearCondition == "InvoiceNo")
                {
                    txtJobCardCode.Text = dsJobCardMaster.Tables[0].Rows[0]["JobCardCode"].ToString();
                }
                txtChassisNo.Text = dsJobCardMaster.Tables[0].Rows[0]["ChassisNo"].ToString();
                txtProductCode.Text = dsJobCardMaster.Tables[0].Rows[0]["ProdCode"].ToString();
                txtJobCardType.Text = dsJobCardMaster.Tables[0].Rows[0]["JobTypeDesc"].ToString().Trim();

                ds = new DataSet(); grl.CodeExists("vehicle", "ProdCode='" + txtProductCode.Text.Trim() + "'", ref ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtProductName.Text = ds.Tables[0].Rows[0]["ProdDesc"].ToString();
                    txtColorName.Text = ds.Tables[0].Rows[0]["Color"].ToString();
                }
                else txtProductCode.Text = txtColorName.Text = "";

                txtRemarksUser.Text = dsJobCardMaster.Tables[0].Rows[0]["Remarks"].ToString();
                txtUserName.Text = dsJobCardMaster.Tables[0].Rows[0]["UserName"].ToString();
                txtBillPayMode.Text = dsJobCardMaster.Tables[0].Rows[0]["PaymentMode"].ToString();

                txtJobType.Text = dsJobCardMaster.Tables[0].Rows[0]["JobTypeCode"].ToString().Trim();
                txtJobTypeDesc.Text = dsJobCardMaster.Tables[0].Rows[0]["JobTypeDesc"].ToString().Trim();

                txtOutKM.Text = dsJobCardMaster.Tables[0].Rows[0]["KM"].ToString().Trim();
                txtEngineNo.Text = dsJobCardMaster.Tables[0].Rows[0]["EngineNo"].ToString();
                txtRegNo.Text = dsJobCardMaster.Tables[0].Rows[0]["RegNo"].ToString();
                if (strClearCondition == "InvoiceNo")
                {
                    ddlCustCode.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["InvoiceCustomer"].ToString();
                }
                else
                {
                    ddlCustCode.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["CusCode"].ToString();
                }

                //txtCustomerName.Text = grl.GetStringValuesAgainstCodes("cusCode", ddlCustCode.SelectedValue.ToString().Trim(), "CusDesc+' '+FatherHusName", "Customer");
                txtColorName.Text = dsJobCardMaster.Tables[0].Rows[0]["Color"].ToString();

                txtGrossAmount.Text = CustomCDBL((Convert.ToDouble(dsJobCardMaster.Tables[0].Rows[0]["JobsTotal"].ToString()) +
                                                Convert.ToDouble(dsJobCardMaster.Tables[0].Rows[0]["PartsTotal"].ToString()) +
                                                Convert.ToDouble(dsJobCardMaster.Tables[0].Rows[0]["BoutTotal"].ToString()) +
                                                Convert.ToDouble(dsJobCardMaster.Tables[0].Rows[0]["SubletTotal"].ToString()) +
                                                Convert.ToDouble(dsJobCardMaster.Tables[0].Rows[0]["LubPartsTotal"].ToString())).ToString()).ToString();

                txtJobsTotal.Text = Math.Round(Convert.ToDouble(dsJobCardMaster.Tables[0].Rows[0]["JobsTotal"].ToString()), 2).ToString("");
                txtLubTotal.Text = Math.Round(Convert.ToDouble(dsJobCardMaster.Tables[0].Rows[0]["LubPartsTotal"].ToString()), 2).ToString("");
                txtPartsTotal.Text = Math.Round(Convert.ToDouble(dsJobCardMaster.Tables[0].Rows[0]["PartsTotal"].ToString()), 2).ToString("");
                txtBoutTotal.Text = Math.Round(Convert.ToDouble(dsJobCardMaster.Tables[0].Rows[0]["BoutTotal"].ToString()), 2).ToString("");
                txtSubletTotal.Text = Math.Round(Convert.ToDouble(dsJobCardMaster.Tables[0].Rows[0]["SubletTotal"].ToString()), 2).ToString("");
                hdnTaxCode.Value = dsJobCardMaster.Tables[0].Rows[0]["hdnTaxCode"].ToString();
                hdnTaxType.Value = dsJobCardMaster.Tables[0].Rows[0]["hdnTaxType"].ToString();
                txtTaxType.Text = dsJobCardMaster.Tables[0].Rows[0]["hdnTaxType"].ToString();

                //Calculate GST Uzair
                SqlParameter[] dsParamInv = {  new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                               new SqlParameter("@InvoiceNo",SqlDbType.Char,8)};
                dsParamInv[0].Value = Session["DealerCode"].ToString();
                dsParamInv[1].Value = ddlInvoiceNo.SelectedValue.ToString().Trim();//(txtInvoiceNo.Text.Trim() == "" ? null : txtInvoiceNo.Text.Trim());
                dsCustomerInvoice = new DataSet();
                dsCustomerInvoice = myFunc.FillDataSet("sp_W2_CustomerInvoice_Select", dsParamInv);

                SqlParameter[] dsParam = {  new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                            new SqlParameter("@JobCardCode",SqlDbType.Char,8),
                                            new SqlParameter("@InvType",SqlDbType.Char,10)
                                         };
                dsParam[0].Value = Session["DealerCode"].ToString();
                dsParam[1].Value = (HFJobCard.Value == "" ? null : HFJobCard.Value.Trim());
                if (strClearCondition == "InvoiceNo")
                {
                    dsParam[2].Value = dsCustomerInvoice.Tables[0].Rows[0]["TaxType"].ToString();
                }
                else
                {
                    dsParam[2].Value = txtTaxType.Text.Trim();
                }

                dsTaxDetail = myFunc.FillDataSet("SP_TaxDetail_NEW1", dsParam);

                DataTable dt = dsTaxDetail.Tables[0];

                double taxAmount = 0, FGSTAmount = 0, FTAmount = 0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    taxAmount = taxAmount + CustomCDBL((Convert.ToDecimal(dsTaxDetail.Tables[0].Rows[i]["GSTAmount"]) +
                                                Convert.ToDecimal(dsTaxDetail.Tables[0].Rows[i]["FGSTAmount"]) +
                                                Convert.ToDecimal(dsTaxDetail.Tables[0].Rows[i]["ExTaxAmount"])));
                    FGSTAmount = FGSTAmount + CustomCDBL(Convert.ToDecimal(dsTaxDetail.Tables[0].Rows[i]["FGSTAmount"]));
                    FTAmount = FTAmount + CustomCDBL(Convert.ToDecimal(dsTaxDetail.Tables[0].Rows[i]["ExTaxAmount"]));

                }

                txtTaxAmount.Text = taxAmount.ToString();
                if (dsTaxDetail.Tables[0].Rows.Count > 0)
                {
                    txtGSTPercent.Text = dsTaxDetail.Tables[0].Rows[0]["GSTPerc"].ToString();
                    txtFGSTPerc.Text = dsTaxDetail.Tables[0].Rows[0]["FGSTPerc"].ToString();
                    txtFTPerc.Text = dsTaxDetail.Tables[0].Rows[0]["ExTaxPerc"].ToString();
                }

                txtFGSTAmount.Text = FGSTAmount.ToString();
                txtFTAmount.Text = FTAmount.ToString();
                //double GSTPercent = double.Parse(grl.GetStringValuesAgainstCodes1("DealerCode", Session["DealerCode"].ToString(), "GST", "BaseRate"));
                //txtGSTPercent.Text = GSTPercent.ToString();

                double totPartsLub = CustomCDBL(txtLubTotal.Text == "" ? 0 : double.Parse(txtLubTotal.Text))
                                    + CustomCDBL(txtPartsTotal.Text == "" ? 0 : double.Parse(txtPartsTotal.Text));
                txttotPartsLub.Text = totPartsLub.ToString();
                FillJobCardDetailGrid(strClearCondition);
                //dEP cUSTOMER gRID
                InsCusTaxGVD();
                CalculateSubletPSTTax();


                double GSTPer = txtGSTPercent.Text.Trim() == "" ? 0 : double.Parse(txtGSTPercent.Text);
                txtGSTAmount.Text = Math.Round(totPartsLub * (GSTPer / 100), 2).ToString();



                // Calculate Item wise Tax
                CalculateSalesTax();

                double ttx = Convert.ToDouble(txtTaxAmount.Text);
                txtGSTAmount.Text = Math.Round(ttx, 2).ToString();

                txtSubTotal.Text = txtGrossAmount.Text == "" ? "0" :
                                                           CustomCDBL(
                                                              double.Parse(txtGrossAmount.Text)
                                                           + (txtGSTAmount.Text == "" ? 0 : double.Parse(txtGSTAmount.Text))
                                                           + (txtPSTAmount.Text == "" ? 0 : double.Parse(txtPSTAmount.Text))
                                                           ).ToString();

                txtTotalAmtCustomer.Text = txtSubTotal.Text == "" ? "0" :
                                                                    CustomCDBL(
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

                //if (ddlInvtype.SelectedValue == "Depriciation")
                //{
                //    txtDepAmount.Text = Total.ToString();
                //}

                txtDepAmount.Text = Total.ToString();

                txtPaidCustomer.Text = "0";
                // Some fields are not found due to calculation problem occured.
                //NetAmount Customer =(Convert.ToDecimal(txtGrossAmount.Text)) - Part Discount + GST Amount - Labor Discount
                txtTotalAmntInsurance.Text = "0";

                txtPaidInsurance.Text = "0"; txtVEODAmount.Text = "0";
                txtDipriAmount.Text = "0"; txtDistOtherParts.Text = "0";
                txtDistOtherPercent.Text = "0";            //txtOutKM.Text               = "0";



                if (dsCustomerInvoice.Tables[0].Rows.Count > 0)
                {
                    txtInvoiceDate.Text = Convert.ToDateTime(dsCustomerInvoice.Tables[0].Rows[0]["InvoiceDate"].ToString()).ToString("dd-MM-yyyy");
                    txtGrossAmount.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["GrossAmount"].ToString()), 2).ToString();
                    txtGSTPercent.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["GSTPer"].ToString()), 2).ToString();
                    txtTaxAmount.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["GSTAmount"].ToString()), 2).ToString();
                    txtPSTPercent.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["SSTPer"].ToString()), 2).ToString();
                    txtPSTAmount.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["SSTAmount"].ToString()), 2).ToString();
                    txtVoucherNo.Text = dsCustomerInvoice.Tables[0].Rows[0]["VoucherNo"].ToString();
                    txtSubTotal.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["SubTotal"].ToString()), 2).ToString();
                    txtDiscLabor.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["DiscountAmount"].ToString()), 2).ToString();
                    txtDiscLaborPercent.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["DiscountPer"].ToString()), 2).ToString();
                    txtTotalAmtCustomer.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["NetAmountCustomer"].ToString()), 2).ToString();
                    txtDiscountParts.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["PartsDiscount"].ToString()), 2).ToString();
                    txtDiscPercentPart.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["PartsDiscountPer"].ToString()), 2).ToString();
                    txtTRNumber.Text = dsCustomerInvoice.Tables[0].Rows[0]["TRNumber"].ToString();
                    txtTotAmtInsComp.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["NetAmountInsurance"].ToString()), 2).ToString();
                    txtPaidCustomer.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["PaidC"].ToString()), 2).ToString();
                    txtPaidInsComp.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["PaidI"].ToString()), 2).ToString();
                    txtDepAmount.Text = Math.Round(Convert.ToDouble(dsCustomerInvoice.Tables[0].Rows[0]["TotalDep"].ToString()), 2).ToString();

                    if (dsCustomerInvoice.Tables[0].Rows[0]["TRDate"].ToString() != "")
                    {
                        txtTRDate.Text = Convert.ToDateTime(dsCustomerInvoice.Tables[0].Rows[0]["TRDate"].ToString()).ToString("dd-MM-yyyy");
                    }
                    txtPolicyNo.Text = dsCustomerInvoice.Tables[0].Rows[0]["PolicyNo"].ToString();
                    if (dsCustomerInvoice.Tables[0].Rows[0]["InvType"].ToString() == "Ins")
                    {
                        ddlInvtype.SelectedIndex = 1;
                    }
                    else
                    {
                        ddlInvtype.SelectedIndex = 0;
                    }
                }
                //txtTotAmtInsComp.Text = ((txtSubTotal.Text == "" ? 0 : double.Parse(txtSubTotal.Text)) - (txtDepAmount.Text == "" ? 0 : double.Parse(txtDepAmount.Text))).ToString();
                if (txtJobCardType.Text == "INSURANCE" && strClearCondition == "JobCard")
                {
                    
                    double tax = 0;
                    taxAmount = 0;
                    double cusTaxAmt = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        tax = CustomCDBL((Convert.ToDecimal(dsTaxDetail.Tables[0].Rows[i]["GSTPerc"]) +
                                                 Convert.ToDecimal(dsTaxDetail.Tables[0].Rows[i]["FGSTPerc"]) +
                                                  Convert.ToDecimal(dsTaxDetail.Tables[0].Rows[i]["ExTaxPerc"])));
                        taxAmount = taxAmount + CustomCDBL(Convert.ToDouble(dsTaxDetail.Tables[0].Rows[i]["DepAmount"]) * tax) / 100;
                    }
                    //taxAmount = (Convert.ToDouble(txtDepAmount.Text) * tax) / 100;
                    //txtTotAmtInsComp.Text = ((txtSubTotal.Text == "" ? 0 : double.Parse(txtSubTotal.Text)) - (txtDepAmount.Text == "" ? 0 : double.Parse(txtDepAmount.Text))).ToString();1
                    //txtTotalAmtCustomer.Text = (Convert.ToDouble(txtTotalAmtCustomer.Text) - Convert.ToDouble(txtTotAmtInsComp.Text)).ToString();1
                    //txtTotalAmtCustomer.Text = (Convert.ToDouble(txtDepAmount.Text) + Convert.ToDouble(txtTaxAmount.Text)).ToString();2
                    double totalAmtCustomer;
                    for (int i = 0; i < CusTax.Rows.Count; i++)
                    {
                        cusTaxAmt = cusTaxAmt + CustomCDBL((Convert.ToDecimal(CusTax.Rows[i]["GSTAmount"]) +
                                                 Convert.ToDecimal(CusTax.Rows[i]["FGSTAmount"]) +
                                                  Convert.ToDecimal(CusTax.Rows[i]["ExTaxAmount"])));

                    }
                    // Calculate Lubricant  Price 
                    double cuslub = 0;
                    for (int i = 0; i < CusTax.Rows.Count; i++)
                    {
                        if (CusTax.Rows[i]["Type"].ToString() == "L")
                        {
                            cuslub = cuslub + CustomCDBL((Convert.ToDecimal(CusTax.Rows[i]["Price"])) *
                                                Convert.ToDecimal(CusTax.Rows[i]["Qty"]));
                        }
                       
                                                 

                    }


                    totalAmtCustomer = Math.Round(Convert.ToDouble(txtDepAmount.Text) + cusTaxAmt+ cuslub, 2);

                    txtTotalAmtCustomer.Text = totalAmtCustomer.ToString();

                    txtTotAmtInsComp.Text = CustomCDBL((txtGrossAmount.Text == "" ? 0 : double.Parse(txtGrossAmount.Text) + Math.Round(txtGSTAmount.Text == "" ? 0 : double.Parse(txtGSTAmount.Text), 2) + Math.Round(txtPSTAmount.Text == "" ? 0 : double.Parse(txtPSTAmount.Text), 2)) - CustomCDBL(totalAmtCustomer.ToString() == "" ? 0 : double.Parse(totalAmtCustomer.ToString()))).ToString();

                }
                if (txtJobCardType.Text == "INSURANCE")
                {
                    if (dsTaxDetail.Tables[0].Rows.Count > 0)
                    {
                        createCusTax();
                        InsCusTaxGVD();
                    }

                    lblTaxHeader.Text = "Insurance Company Taxes";

                    txtDiscLabor.ReadOnly = true;
                    txtDiscountParts.ReadOnly = true;
                    ddlInsCo.Enabled = true;
                    ddlInsBranch.Enabled = true;
                    ds = new DataSet(); grl.CodeExists("CustomerEstimateMaster", "CustomerEstimateCode='" + dsJobCardMaster.Tables[0].Rows[0]["EstimateCode"].ToString() + "'", ref ds);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlInsCo.Text = ds.Tables[0].Rows[0]["InsCompCode"].ToString();

                        ddlInsCo_SelectedIndexChanged(ddlInsCo, null);

                        ddlInsBranch.Text = ds.Tables[0].Rows[0]["InsBranchCode"].ToString();
                    }
                    else if (strClearCondition == "InvoiceNo")
                    {
                        ddlInsCo.Text = dsCustomerInvoice.Tables[0].Rows[0]["InsCode"].ToString();

                        ddlInsCo_SelectedIndexChanged(ddlInsCo, null);

                        ddlInsBranch.Text = dsCustomerInvoice.Tables[0].Rows[0]["BranchCode"].ToString();
                    }

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
        /// <summary>
        ///  Customer Tax Grid 
        /// </summary>
        #region
        private void createCusTax()
        {
            CusTax = new DataTable();
            CusTax.Columns.Add(new DataColumn("ItemCode", typeof(string)));
            CusTax.Columns.Add(new DataColumn("PartNo", typeof(string)));
            CusTax.Columns.Add(new DataColumn("ItemDesc", typeof(string)));
            CusTax.Columns.Add(new DataColumn("Source", typeof(string)));
            CusTax.Columns.Add(new DataColumn("Qty", typeof(string)));
            CusTax.Columns.Add(new DataColumn("Price", typeof(string)));
            CusTax.Columns.Add(new DataColumn("GrossAmount", typeof(string)));
            CusTax.Columns.Add(new DataColumn("Type", typeof(string)));
            CusTax.Columns.Add(new DataColumn("Dep", typeof(string)));
            CusTax.Columns.Add(new DataColumn("DepAmount", typeof(string)));
            CusTax.Columns.Add(new DataColumn("GSTPerc", typeof(string)));
            CusTax.Columns.Add(new DataColumn("GSTAmount", typeof(string)));
            CusTax.Columns.Add(new DataColumn("FGSTPerc", typeof(string)));
            CusTax.Columns.Add(new DataColumn("FGSTAmount", typeof(string)));
            CusTax.Columns.Add(new DataColumn("ExTaxPerc", typeof(string)));
            CusTax.Columns.Add(new DataColumn("ExTaxAmount", typeof(string)));
            CusTax.Columns.Add(new DataColumn("JobCardCode", typeof(string)));
            //SubletDT.Rows.Add(new object[] { null, null, null, null, null, null, null, null,null});  
            InsCusGVD.DataSource = CusTax;
            InsCusGVD.DataBind();
            Session["CusTax"] = CusTax;
        }
        private void createInsTax()
        {
            InsTax = new DataTable();
            InsTax.Columns.Add(new DataColumn("ItemCode", typeof(string)));
            InsTax.Columns.Add(new DataColumn("PartNo", typeof(string)));
            InsTax.Columns.Add(new DataColumn("ItemDesc", typeof(string)));
            InsTax.Columns.Add(new DataColumn("Source", typeof(string)));
            InsTax.Columns.Add(new DataColumn("Qty", typeof(string)));
            InsTax.Columns.Add(new DataColumn("Price", typeof(string)));
            InsTax.Columns.Add(new DataColumn("GrossAmount", typeof(string)));
            InsTax.Columns.Add(new DataColumn("Type", typeof(string)));
            InsTax.Columns.Add(new DataColumn("Dep", typeof(string)));
            InsTax.Columns.Add(new DataColumn("DepAmount", typeof(string)));
            InsTax.Columns.Add(new DataColumn("GSTPerc", typeof(string)));
            InsTax.Columns.Add(new DataColumn("GSTAmount", typeof(string)));
            InsTax.Columns.Add(new DataColumn("FGSTPerc", typeof(string)));
            InsTax.Columns.Add(new DataColumn("FGSTAmount", typeof(string)));
            InsTax.Columns.Add(new DataColumn("ExTaxPerc", typeof(string)));
            InsTax.Columns.Add(new DataColumn("ExTaxAmount", typeof(string)));
            InsTax.Columns.Add(new DataColumn("JobCardCode", typeof(string)));
            //SubletDT.Rows.Add(new object[] { null, null, null, null, null, null, null, null,null});  
            gvTax.DataSource = InsTax;
            gvTax.DataBind();
            Session["InsTax"] = InsTax;
        }
        private void InsInsTaxGVD()
        {
            TabPanel8.Visible = true;
            string Dep;
            double depamt = 0, gst = 0, extax = 0, fgst = 0 ;
            try
            {
                if (dsTaxDetail.Tables[0].Rows.Count > 0)
                {
                    DataRow rowInsTaxDT = InsTax.NewRow();
                    DataRow[] drr = dsTaxDetail.Tables[0].Select();
                    for (int i = 0; i < drr.Length; i++)
                    {

                        if (dsTaxDetail.Tables[0].Rows[i]["Type"].ToString() != "L")
                        {
                            rowInsTaxDT["ItemCode"] = dsTaxDetail.Tables[0].Rows[i]["ItemCode"].ToString();
                            rowInsTaxDT["PartNo"] = dsTaxDetail.Tables[0].Rows[i]["PartNo"].ToString();
                            rowInsTaxDT["ItemDesc"] = dsTaxDetail.Tables[0].Rows[i]["ItemDesc"].ToString();
                            rowInsTaxDT["Source"] = dsTaxDetail.Tables[0].Rows[i]["Source"].ToString();
                            rowInsTaxDT["Qty"] = dsTaxDetail.Tables[0].Rows[i]["Qty"].ToString();
                            rowInsTaxDT["Price"] = dsTaxDetail.Tables[0].Rows[i]["Price"].ToString();
                            rowInsTaxDT["GrossAmount"] = dsTaxDetail.Tables[0].Rows[i]["GrossAmount"].ToString();
                            rowInsTaxDT["Type"] = dsTaxDetail.Tables[0].Rows[i]["Type"].ToString();
                            Dep = (100 - CustomCDBL(grl.GetStringValuesAgainstCodes("ItemCode='" + dsTaxDetail.Tables[0].Rows[i]["ItemCode"].ToString() + "' and JobCardCode='" + txtJobCardCode.Text + "'", "JobCardPartsDetail", "Dep"))).ToString();
                            depamt = CustomCDBL(((Convert.ToDouble(CustomCDBL(dsTaxDetail.Tables[0].Rows[i]["Price"].ToString())) * Convert.ToDouble(Dep)) / 100) * Convert.ToDouble(CustomCDBL(dsTaxDetail.Tables[0].Rows[i]["Qty"].ToString())));
                            rowInsTaxDT["Dep"] = Dep;
                            rowInsTaxDT["DepAmount"] = depamt;
                            rowInsTaxDT["GSTPerc"] = dsTaxDetail.Tables[0].Rows[i]["GSTPerc"].ToString();
                            rowInsTaxDT["GSTAmount"] = Math.Round((Convert.ToDecimal(depamt) * Convert.ToDecimal(CustomCDBL(dsTaxDetail.Tables[0].Rows[i]["GSTPerc"].ToString()))) / 100, 2);
                            rowInsTaxDT["FGSTPerc"] = dsTaxDetail.Tables[0].Rows[i]["FGSTPerc"].ToString();
                            rowInsTaxDT["FGSTAmount"] = Math.Round((Convert.ToDecimal(depamt) * Convert.ToDecimal(CustomCDBL(dsTaxDetail.Tables[0].Rows[i]["FGSTPerc"].ToString()))) / 100, 2);
                            rowInsTaxDT["ExTaxPerc"] = dsTaxDetail.Tables[0].Rows[i]["ExTaxPerc"].ToString();
                            rowInsTaxDT["ExTaxAmount"] = Math.Round((Convert.ToDecimal(depamt) * Convert.ToDecimal(CustomCDBL(dsTaxDetail.Tables[0].Rows[i]["ExTaxAmount"].ToString()))) / 100, 2);
                            rowInsTaxDT["JobCardCode"] = dsTaxDetail.Tables[0].Rows[i]["JobCardCode"].ToString();
                            InsTax.Rows.Add(rowInsTaxDT);
                            rowInsTaxDT = InsTax.NewRow();
                        }
                        }
                    
                    Session["InsTax"] = InsTax;
                    gvTax.DataSource = InsTax;
                    gvTax.DataBind();

                }
            }
            catch(Exception ex)
            {
                lblMsg.Text = ex.Message;
                lblMsg.ForeColor = Color.Red;
            }
           
        }
        // Inusurance Tax Grid
        private void InsCusTaxGVD()
        {
            TabPanel8.Visible = true;
            string Dep;
            double depamt = 0, gst = 0, extax = 0, fgst = 0;
            try
            {
                if (dsTaxDetail.Tables[0].Rows.Count > 0)
                {
                    DataRow rowCusTaxDT = CusTax.NewRow();
                    DataRow[] drr = dsTaxDetail.Tables[0].Select();
                    for (int i = 0; i < drr.Length; i++)
                    {


                        rowCusTaxDT["ItemCode"] = dsTaxDetail.Tables[0].Rows[i]["ItemCode"].ToString();
                        rowCusTaxDT["PartNo"] = dsTaxDetail.Tables[0].Rows[i]["PartNo"].ToString();
                        rowCusTaxDT["ItemDesc"] = dsTaxDetail.Tables[0].Rows[i]["ItemDesc"].ToString();
                        rowCusTaxDT["Source"] = dsTaxDetail.Tables[0].Rows[i]["Source"].ToString();
                        rowCusTaxDT["Qty"] = dsTaxDetail.Tables[0].Rows[i]["Qty"].ToString();
                        rowCusTaxDT["Price"] = dsTaxDetail.Tables[0].Rows[i]["Price"].ToString();
                        rowCusTaxDT["GrossAmount"] = dsTaxDetail.Tables[0].Rows[i]["GrossAmount"].ToString();
                        rowCusTaxDT["Type"] = dsTaxDetail.Tables[0].Rows[i]["Type"].ToString();
                        Dep = CustomCDBL(grl.GetStringValuesAgainstCodes("ItemCode='" + dsTaxDetail.Tables[0].Rows[i]["ItemCode"].ToString() + "' and JobCardCode='" + txtJobCardCode.Text + "'", "JobCardPartsDetail", "Dep")).ToString();
                        depamt = CustomCDBL(((Convert.ToDouble(CustomCDBL(dsTaxDetail.Tables[0].Rows[i]["Price"].ToString())) * Convert.ToDouble(Dep)) / 100) * Convert.ToDouble(CustomCDBL(dsTaxDetail.Tables[0].Rows[i]["Qty"].ToString())));
                        rowCusTaxDT["Dep"] = Dep;
                        if (dsTaxDetail.Tables[0].Rows[i]["Type"].ToString() == "L")
                        {
                            rowCusTaxDT["DepAmount"] = dsTaxDetail.Tables[0].Rows[i]["GrossAmount"].ToString();
                        }
                        else
                        {
                            rowCusTaxDT["DepAmount"] = depamt;
                        }
                      
                     
                        rowCusTaxDT["GSTPerc"] = dsTaxDetail.Tables[0].Rows[i]["GSTPerc"].ToString();
                        if(dsTaxDetail.Tables[0].Rows[i]["Type"].ToString()=="L")
                        {
                            rowCusTaxDT["GSTAmount"] = CustomCDBL(dsTaxDetail.Tables[0].Rows[i]["GSTAmount"].ToString());
                        }
                        else
                        {
                            rowCusTaxDT["GSTAmount"] = Math.Round((Convert.ToDecimal(depamt) * Convert.ToDecimal(CustomCDBL(dsTaxDetail.Tables[0].Rows[i]["GSTPerc"].ToString()))) / 100, 2);
                        }
                        if (dsTaxDetail.Tables[0].Rows[i]["Type"].ToString() == "L")
                        {
                            rowCusTaxDT["FGSTAmount"] = CustomCDBL(dsTaxDetail.Tables[0].Rows[i]["FGSTAmount"].ToString());
                        }
                        else
                        {
                            rowCusTaxDT["FGSTAmount"] = Math.Round((Convert.ToDecimal(depamt) * Convert.ToDecimal(CustomCDBL(dsTaxDetail.Tables[0].Rows[i]["FGSTPerc"].ToString()))) / 100, 2);
                        }
                        rowCusTaxDT["FGSTPerc"] = dsTaxDetail.Tables[0].Rows[i]["FGSTPerc"].ToString();
                        if (dsTaxDetail.Tables[0].Rows[i]["Type"].ToString() == "L")
                        {
                            rowCusTaxDT["FGSTAmount"] = CustomCDBL(dsTaxDetail.Tables[0].Rows[i]["FGSTPerc"].ToString());
                        }
                        else
                        {
                            rowCusTaxDT["FGSTAmount"] = Math.Round((Convert.ToDecimal(depamt) * Convert.ToDecimal(CustomCDBL(dsTaxDetail.Tables[0].Rows[i]["FGSTPerc"].ToString()))) / 100, 2);
                        }
                        rowCusTaxDT["ExTaxPerc"] = dsTaxDetail.Tables[0].Rows[i]["ExTaxPerc"].ToString();
                        if (dsTaxDetail.Tables[0].Rows[i]["Type"].ToString() == "L")
                        {
                            rowCusTaxDT["ExTaxAmount"] = CustomCDBL(dsTaxDetail.Tables[0].Rows[i]["ExTaxAmount"].ToString());
                        }
                        else
                        {
                            rowCusTaxDT["ExTaxAmount"] = Math.Round((Convert.ToDecimal(depamt) * Convert.ToDecimal(CustomCDBL(dsTaxDetail.Tables[0].Rows[i]["ExTaxPerc"].ToString()))) / 100, 2);
                        }
                       
                        rowCusTaxDT["JobCardCode"] = dsTaxDetail.Tables[0].Rows[i]["JobCardCode"].ToString();
                        CusTax.Rows.Add(rowCusTaxDT);
                        rowCusTaxDT = CusTax.NewRow();
                    }

                    Session["CusTax"] = CusTax;
                    InsCusGVD.DataSource = CusTax;
                    InsCusGVD.DataBind();

                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
                lblMsg.ForeColor = Color.Red;
            }

        }

        #endregion
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

          

        }
        private void CalculateSubletPSTTax()
        {
           
            double totJob = 0;
            double totSublet = 0;
            double totJobSublet = 0;
            double jobtotalfor_PST = 0;


            string Append = " and TaxAppCode='"+hdnTaxCode.Value+"' and TaxType='"+hdnTaxType.Value+"'";
            // Calculate Sublet+ Labor Tax
            txtPSTPercent.Text = "0";
            txtPSTPercent.Text = dsJobCardDetail.Tables[0].Rows[0]["PST"].ToString();
            double PSTPer =  double.Parse(txtPSTPercent.Text.ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : txtPSTPercent.Text.ToString().Replace("&nbsp;", "").Trim());
            if (grl.IsExist("Labor", "Y", "TaxSetUpDetail", "COMON", Append))
            {
                for (int i = 0; i < dsJobCardDetail.Tables[0].Rows.Count; i++)
                {
                    if (dsJobCardDetail.Tables[0].Rows[i]["Type"].ToString() != "F")
                    {
                        jobtotalfor_PST = CustomCDBL(jobtotalfor_PST + Convert.ToDouble(dsJobCardDetail.Tables[0].Rows[i]["Amount"].ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : dsJobCardDetail.Tables[0].Rows[i]["Amount"].ToString().Replace("&nbsp;", "").Trim()));
                    }

                }
            }
            else
            {
                txtPSTPercent.Text = "0";
                jobtotalfor_PST = 0;
            }
         
            
            totJobSublet = CustomCDBL((jobtotalfor_PST == 0 ? 0 :jobtotalfor_PST)
                                                 + (txtSubletTotal.Text == "" ? 0 : double.Parse(txtSubletTotal.Text)));

            if (grl.IsExist("Sublet", "Y", "TaxSetUpDetail", "COMON",Append)&& grl.IsExist("Labor", "Y", "TaxSetUpDetail", "COMON", Append))
            {
                totJobSublet = ((jobtotalfor_PST == 0 ? 0 : jobtotalfor_PST))
                                                 + (txtSubletTotal.Text == "" ? 0 : double.Parse(txtSubletTotal.Text));
                txtPSTAmount.Text = Math.Round(totJobSublet * (PSTPer / 100), 2).ToString();
            }
            else if (grl.IsExist("Sublet", "N", "TaxSetUpDetail", "COMON", Append) && grl.IsExist("Labor", "Y", "TaxSetUpDetail", "COMON", Append))
            {
                totJob = ((jobtotalfor_PST == 0 ? 0 : jobtotalfor_PST))
                                                 + (txtSubletTotal.Text == "" ? 0 : double.Parse("0"));
                txtPSTAmount.Text = Math.Round(totJob * (PSTPer / 100), 2).ToString();
            }
            else if (grl.IsExist("Sublet", "N", "TaxSetUpDetail", "COMON", Append) && grl.IsExist("Labor", "N", "TaxSetUpDetail", "COMON", Append))
            {
                totSublet = ((jobtotalfor_PST == 0 ? 0 : jobtotalfor_PST))
                                                + (txtSubletTotal.Text == "" ? 0 : double.Parse("0"));
                txtPSTAmount.Text = Math.Round(totSublet * (PSTPer / 100), 2).ToString();
            }


            txttotJobSublet.Text = totJobSublet.ToString();
         
          
           

           

           

        }
        private void SaveItemSalesTax(string InvoiceNumber, string InvNo = "")
        {
            double totTax = 0;
            // Calculate PartItems Tax
            SqlParameter[] dsParam = {  new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                        new SqlParameter("@JobCardCode",SqlDbType.Char,8),
                                        new SqlParameter("@InvType",SqlDbType.Char,15)
                                         };
            dsParam[0].Value = Session["DealerCode"].ToString();
            dsParam[1].Value = (HFJobCard.Value == "" ? null : HFJobCard.Value.Trim());
            dsParam[2].Value = txtTaxType.Text.Trim();


            DataSet dsTaxDetail = new DataSet();
            //dsTaxDetail = myFunc.FillDataSet("SP_TaxDetail_NEW1", dsParam);
            dsTaxDetail = (DataSet)Session["dsTaxDetail"];
            CusTax = (DataTable)Session["CusTax"];
            DataTable dt = dsTaxDetail.Tables[0];

            if (txtJobCardType.Text == "INSURANCE" && ddlInvtype.SelectedValue == "Depriciation")
            {
                for (int i = 0; i < CusTax.Rows.Count; i++)
                {

                    string ityp = CusTax.Rows[i]["Type"].ToString();
                    double gstPerc =   CustomCDBL(Convert.ToDouble(CusTax.Rows[i]["GSTPerc"]));
                    double fgstPerc =  CustomCDBL(Convert.ToDouble(CusTax.Rows[i]["FGSTPerc"]));
                    double exTaxPerc = CustomCDBL(Convert.ToDouble(CusTax.Rows[i]["ExTaxPerc"]));
                    double gstAmt =    CustomCDBL(Convert.ToDouble(CusTax.Rows[i]["GSTAmount"]));
                    double fgstAmt =   CustomCDBL(Convert.ToDouble(CusTax.Rows[i]["FGSTAmount"]));
                    double exTaxAmt =  CustomCDBL(Convert.ToDouble(CusTax.Rows[i]["ExTaxAmount"]));
                    double dep =       CustomCDBL(Convert.ToDouble(CusTax.Rows[i]["Dep"]));
                    double depAmt =    CustomCDBL(Convert.ToDouble(CusTax.Rows[i]["DepAmount"]));

                    string itmCode = CusTax.Rows[i]["ItemCode"].ToString();
                    double taxAmount = CustomCDBL(Convert.ToDouble(CusTax.Rows[i]["GrossAmount"]));
                    string jcno = CusTax.Rows[i]["JobCardCode"].ToString();

                    string sql = "insert into ItemsTaxTrx (DealerCode,SINumber,SalesTaxNo,ItemCode,GSTPerc,FGSTPerc,SalesTaxAmount,InvoiceType,InvoiceNumber,JobCardCode,ItemType,GSTAmount,FGSTAmount,ExGSTPerc,ExGSTAmount,Dep,DepAmount,BillType)";
                    sql += " values('" + Session["DealerCode"].ToString() + "','" + InvNo + "','" + InvoiceNumber + "','" + itmCode + "','" + gstPerc + "','" + fgstPerc + "','" + taxAmount + "','J','" + InvNo + "','" + jcno + "','" + ityp + "','" + gstAmt + "','" + fgstAmt + "','" + exTaxPerc + "','" + exTaxAmt + "','" + dep + "','" + depAmt + "','J')";

                    //string sql = "insert into ItemsTaxTrx (DealerCode,InvoiceNumber,JobCardCode,ItemType,SalesTaxNo,ItemCode,GSTPerc,FGSTPerc,SalesTaxAmount,InvoiceType,GSTAmount,FGSTAmount)";
                    //sql += " values('" + Session["DealerCode"].ToString() + "','" + InvNo + "','" + jcno + "','" + ityp + "','" + InvoiceNumber + "','" + itmcod + "','" + stxpct.ToString() + "','" + ftxpct.ToString() + "','" + totamt + "','J','" + gstot.ToString() + "','" + fstot.ToString() + "')";

                    DataTable dtmp = myFunc.GetData(sql);
                    dtmp.Dispose();
                }

                CusTax.Dispose();




            }
            else {

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string ityp = dt.Rows[i]["Type"].ToString();
                    double gstPerc =   CustomCDBL(Convert.ToDouble(dsTaxDetail.Tables[0].Rows[i]["GSTPerc"]));
                    double fgstPerc =  CustomCDBL(Convert.ToDouble(dsTaxDetail.Tables[0].Rows[i]["FGSTPerc"]));
                    double exTaxPerc = CustomCDBL(Convert.ToDouble(dsTaxDetail.Tables[0].Rows[i]["ExTaxPerc"]));
                    double gstAmt =    CustomCDBL(Convert.ToDouble(dsTaxDetail.Tables[0].Rows[i]["GSTAmount"]));
                    double fgstAmt =   CustomCDBL(Convert.ToDouble(dsTaxDetail.Tables[0].Rows[i]["FGSTAmount"]));
                    double exTaxAmt =  CustomCDBL(Convert.ToDouble(dsTaxDetail.Tables[0].Rows[i]["ExTaxAmount"]));
                    double dep =       CustomCDBL(Convert.ToDouble(dsTaxDetail.Tables[0].Rows[i]["Dep"]));
                    double depAmt =    CustomCDBL(Convert.ToDouble(dsTaxDetail.Tables[0].Rows[i]["DepAmount"]));

                    string itmCode = dt.Rows[i]["ItemCode"].ToString();
                    double taxAmount = CustomCDBL(Convert.ToDouble(dsTaxDetail.Tables[0].Rows[i]["GrossAmount"]));
                    string jcno = dt.Rows[i]["JobCardCode"].ToString();

                    string sql = "insert into ItemsTaxTrx (DealerCode,SINumber,SalesTaxNo,ItemCode,GSTPerc,FGSTPerc,SalesTaxAmount,InvoiceType,InvoiceNumber,JobCardCode,ItemType,GSTAmount,FGSTAmount,ExGSTPerc,ExGSTAmount,Dep,DepAmount,BillType)";
                    sql += " values('" + Session["DealerCode"].ToString() + "','" + InvNo + "','" + InvoiceNumber + "','" + itmCode + "','" + gstPerc + "','" + fgstPerc + "','" + taxAmount + "','J','" + InvNo + "','" + jcno + "','" + ityp + "','" + gstAmt + "','" + fgstAmt + "','" + exTaxPerc + "','" + exTaxAmt + "','" + dep + "','" + depAmt + "','J')";

                    //string sql = "insert into ItemsTaxTrx (DealerCode,InvoiceNumber,JobCardCode,ItemType,SalesTaxNo,ItemCode,GSTPerc,FGSTPerc,SalesTaxAmount,InvoiceType,GSTAmount,FGSTAmount)";
                    //sql += " values('" + Session["DealerCode"].ToString() + "','" + InvNo + "','" + jcno + "','" + ityp + "','" + InvoiceNumber + "','" + itmcod + "','" + stxpct.ToString() + "','" + ftxpct.ToString() + "','" + totamt + "','J','" + gstot.ToString() + "','" + fstot.ToString() + "')";

                    DataTable dtmp = myFunc.GetData(sql);
                    dtmp.Dispose();
                }

                dt.Dispose();
            }
        


        }
        private void SaveItemSalesTaxIns(string InvoiceNumber, string InvNo = "")
        {
            double totTax = 0;
            // Calculate PartItems Tax
            SqlParameter[] dsParam = {  new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                        new SqlParameter("@JobCardCode",SqlDbType.Char,8),
                                        new SqlParameter("@InvType",SqlDbType.Char,15)
                                         };
            dsParam[0].Value = Session["DealerCode"].ToString();
            dsParam[1].Value = (HFJobCard.Value == "" ? null : HFJobCard.Value.Trim());
            dsParam[2].Value = txtTaxType.Text.Trim();


            DataSet dsTaxDetail = new DataSet();
            //dsTaxDetail = myFunc.FillDataSet("SP_TaxDetail_NEW1", dsParam);
            DataTable InsTaxdetail  = (DataTable)Session["InsTax"];
            dt = InsTaxdetail;
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string ityp = dt.Rows[i]["Type"].ToString();
                    double gstPerc =   CustomCDBL(Convert.ToDouble(InsTaxdetail.Rows[i]["GSTPerc"]));
                    double fgstPerc =  CustomCDBL(Convert.ToDouble(InsTaxdetail.Rows[i]["FGSTPerc"]));
                    double exTaxPerc = CustomCDBL(Convert.ToDouble(InsTaxdetail.Rows[i]["ExTaxPerc"]));
                    double gstAmt =    CustomCDBL(Convert.ToDouble(InsTaxdetail.Rows[i]["GSTAmount"]));
                    double fgstAmt =   CustomCDBL(Convert.ToDouble(InsTaxdetail.Rows[i]["FGSTAmount"]));
                    double exTaxAmt =  CustomCDBL(Convert.ToDouble(InsTaxdetail.Rows[i]["ExTaxAmount"]));
                    double dep =       CustomCDBL(Convert.ToDouble(InsTaxdetail.Rows[i]["Dep"]));
                    double depAmt = CustomCDBL(Convert.ToDouble(InsTaxdetail.Rows[i]["DepAmount"]));

                    string itmCode = dt.Rows[i]["ItemCode"].ToString();
                    double taxAmount = CustomCDBL(Convert.ToDouble(InsTaxdetail.Rows[i]["GrossAmount"]));
                    string jcno = dt.Rows[i]["JobCardCode"].ToString();

                    string sql = "insert into ItemsTaxTrx (DealerCode,SINumber,SalesTaxNo,ItemCode,GSTPerc,FGSTPerc,SalesTaxAmount,InvoiceType,InvoiceNumber,JobCardCode,ItemType,GSTAmount,FGSTAmount,ExGSTPerc,ExGSTAmount,Dep,DepAmount,BillType)";
                    sql += " values('" + Session["DealerCode"].ToString() + "','" + InvNo + "','" + InvoiceNumber + "','" + itmCode + "','" + gstPerc + "','" + fgstPerc + "','" + taxAmount + "','J','" + InvNo + "','" + jcno + "','" + ityp + "','" + gstAmt + "','" + fgstAmt + "','" + exTaxPerc + "','" + exTaxAmt + "','" + dep + "','" + depAmt + "','J')";

                    //string sql = "insert into ItemsTaxTrx (DealerCode,InvoiceNumber,JobCardCode,ItemType,SalesTaxNo,ItemCode,GSTPerc,FGSTPerc,SalesTaxAmount,InvoiceType,GSTAmount,FGSTAmount)";
                    //sql += " values('" + Session["DealerCode"].ToString() + "','" + InvNo + "','" + jcno + "','" + ityp + "','" + InvoiceNumber + "','" + itmcod + "','" + stxpct.ToString() + "','" + ftxpct.ToString() + "','" + totamt + "','J','" + gstot.ToString() + "','" + fstot.ToString() + "')";

                    DataTable dtmp = myFunc.GetData(sql);
                    dtmp.Dispose();
                }

                dt.Dispose();
           
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

        protected void gvTax_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Alternate || e.Row.RowState == DataControlRowState.Normal))
            {
                if (txtJobCardType.Text == "INSURANCE")
                {
                    totaltax = totaltax + double.Parse(e.Row.Cells[11].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[11].Text.Replace("&nbsp;", "").Trim()) +
                                       double.Parse(e.Row.Cells[13].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[13].Text.Replace("&nbsp;", "").Trim()) +
                                       double.Parse(e.Row.Cells[15].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[15].Text.Replace("&nbsp;", "").Trim());
                    totalAmtwithTax = totalAmtwithTax  + double.Parse(e.Row.Cells[4].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[4].Text.Replace("&nbsp;", "").Trim()) *
                        double.Parse(e.Row.Cells[9].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[9].Text.Replace("&nbsp;", "").Trim());
                }
                else
                {
                    totaltax = totaltax + double.Parse(e.Row.Cells[11].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[11].Text.Replace("&nbsp;", "").Trim()) +
                                                           double.Parse(e.Row.Cells[13].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[13].Text.Replace("&nbsp;", "").Trim()) +
                                                           double.Parse(e.Row.Cells[15].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[15].Text.Replace("&nbsp;", "").Trim());
                    totalAmtwithTax = totalAmtwithTax  + double.Parse(e.Row.Cells[4].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[4].Text.Replace("&nbsp;", "").Trim()) *
                        double.Parse(e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim());
                }
               

            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                totaltax = totalAmtwithTax = 0;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                lblTotalTaxAmt.Text = Convert.ToString(totaltax);
                lblTotalWithTax.Text = Convert.ToString(totalAmtwithTax+totaltax);
                // txtJobCardTotal.Text = Convert.ToString(totEst);
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
            else return Math.Round(Convert.ToDouble(txt), 2);
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
                    Total = (Total + Math.Round(InsAmount + Convert.ToDouble(e.Row.Cells[3].Text),2));
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

            if (grl.ValuesAgainstCodes("InvoiceNo", ddlInvoiceNo.SelectedValue, "VoucherNo", "CustomerInvoice", "", Session["DealerCode"].ToString()))
            {
                grl.UserMsg(lblMsg, Color.Red, "Voucher Exists can't delete the record.");
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
                    string sql1 = "Update GatePassTemp Set DelFlag='Y' Where DealerCode='" + Session["DealerCode"].ToString() + "' And TransCode='" + HFJobCard.Value.Trim() + "' and Module='Service'";
                    myFunc.ExecuteQuery(sql1, Trans);

                    ObjTrans.CommittTransaction(ref Trans);
                    //lblMsg.Visible = true;
                    myFunc.UserMsg(lblMsg, Color.Green, "Deleted Successfully Inv. No: " +ddlInvoiceNo.SelectedValue.ToString());
                    Load_ddlInvoiceNo();

                    //lblMSGPop.Text = "Deleted Successfully Inv. No: " + txtInvoiceNo.Text;
                    //PopupControlMSG.ShowOnPageLoad = true;

                    ClearAll();
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
            
          //  LoadReportData();
           // LoadMasterData("InvoiceNo");
            //txtInvoiceNo.ReadOnly = true;
            //txtJobCardNo.ReadOnly = true;
            //txtCustomer.ReadOnly = true;
        }


        protected void ddlInvtype_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Load_ddlJobCardCode();
        
        }
        protected void ddlJobCardCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["total"] = "0";
            HFJobCard.Value = ddlJobCardCode.SelectedValue.ToString();
          ////  LoadMasterData("JobCard");
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
                    myFunc.UserMsg(lblMsg, Color.Red, "Please Isuue All Parts as per Job Card");
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
                if (grl.IsExist("JobCardCode", HFJobCard.Value.Trim(), "CustomerInvoice", Session["DealerCode"].ToString(), " And InvType = 'Inv' And DelFlag = 'N'"))
                {

                    myFunc.UserMsg(lblMsg, Color.Red, "Invoice already exist.");
                    return;
                }

                double iTotalInvoice = 0;
                double iPartsTotal = Math.Round(Convert.ToDouble(txtPartsTotal.Text),2);
                double iJobTotal =   Math.Round(Convert.ToDouble(txtJobsTotal.Text),2);
                double iLubsTotal =  Math.Round(Convert.ToDouble(txtLubTotal.Text),2);
                double iSubTotal =   Math.Round(Convert.ToDouble(txtSubletTotal.Text),2);
                double iGrossAmt =   Math.Round(Convert.ToDouble(txtGrossAmount.Text),2);
                double iBoutTotal =  Math.Round(Convert.ToDouble(txtBoutTotal.Text),2);

                iTotalInvoice = CustomCDBL(iPartsTotal + iJobTotal + iLubsTotal + iSubTotal+ iBoutTotal);
                if (iTotalInvoice != iGrossAmt)
                {
                    //SendAlert(); return;
                    myFunc.UserMsg(lblMsg, Color.Red, "Gross Amount is not correct");
                    //lblMSGPop.Text ="Gross Amount is not correct" ;
                    //PopupControlMSG.ShowOnPageLoad = true;
                    return;
                }
                if (txtJobCardType.Text == "INSURANCE")
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
                    //InvType IN ('Ins','Dep')
                    if (grl.IsExist("JobCardCode", HFJobCard.Value.Trim(), "CustomerInvoice", Session["DealerCode"].ToString()," And InvType = 'Ins'"))
                    {

                        myFunc.UserMsg(lblMsg, Color.Red, "insurance invoice can not be Save");
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
                if (txtJobCardType.Text.Trim() != "INSURANCE")
                {
                    InvoiceEntry();
                    if(txtBillPayMode.Text.Trim() == "Cash")
                    {
                        string strReceiptNo = string.Empty;
                        PaymentReceiptEntry(ref strReceiptNo);
                        myFunc.UserMsg(lblMsg, Color.Green, lblMsg.Text + "<br />" + " Payment Receipt no. " + strReceiptNo + " has been added successfully : ");
                    }
                }
                else if (txtJobCardType.Text == "INSURANCE" )
                {
                    InsuranceInvoiceEntry();
                    if (Convert.ToDouble(txtDepAmount.Text) > 0|| Convert.ToDouble(txtLubTotal.Text) > 0)
                    {
                        InvoiceEntry();
                    }
                }
                //else
                //{
                //    InsuranceInvoiceEntry();
                //    //if (Convert.ToDouble(txtTotAmtInsComp.Text) > 0)
                //    //{
                //    //    InsuranceInvoiceEntry();
                //    //}
                //}
                    //if (Convert.ToDouble(txtTotAmtInsComp.Text) > 0)
                    //{
                    //    InsuranceInvoiceEntry();
                    //}
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

                if (Create_NDM(ddlJobCardCode.SelectedValue.ToString(), "InvoiceCreated") == false)
                {
                    grl.UserMsg(lblMsg, Color.Red, "NDM Data Can not be inserted");
                    ObjTrans.RollBackTransaction(ref Trans);
                    return;
                }

                if (PostServiceFollowUp(ddlJobCardCode.SelectedValue .ToString (), "InvoiceCreated") == false)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Post Service FollowUp Data Can not be inserted");
                    ObjTrans.RollBackTransaction(ref Trans);
                    return;
                }

            ClearAll();
            Load_ddlJobCardCode();
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

            if (grl.CodeExists("CustomerInvoice", "InvoiceNo", ddlInvoiceNo.SelectedValue,Session["DealerCode"].ToString()," and VoucherFlag='Y'") == true)
            {
                myFunc.UserMsg(lblMsg, Color.Red, "Invoice no. can not Delete because its Voucher is Posted");
                //lblMSGPop.Text = "Invoice no. not exist ";
                //PopupControlMSG.ShowOnPageLoad = true;
                return;

            }

            if (grl.CodeExists("PaymentReceiptDetail", "InvoiceNo", ddlInvoiceNo.SelectedValue.ToString().Trim(), Session["DealerCode"].ToString()," and InvoiceType='Service'") == true && txtBillPayMode.Text.Trim() != "Cash")
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

                    if(txtBillPayMode.Text.Trim() == "Cash")
                    {
                        IQuery += " Update A set DelFlag = 'Y' From PaymentReceiptMaster A inner join PaymentReceiptDetail B on A.ReceiptNo = B.ReceiptNo " +
                                  " and A.DealerCode = B.DealerCode where A.DealerCode = '"+Session["DealerCode"].ToString()+"' and B.InvoiceNo = '"+ddlInvoiceNo.SelectedValue+"' ";

                        IQuery += " Delete From PaymentReceiptDetail where DealerCode = '" + Session["DealerCode"].ToString() + "' and InvoiceNo = '"+ddlInvoiceNo.SelectedValue+"'";
                    }

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

                    ClearAll();
                }
            }
            catch (Exception) { ObjTrans.RollBackTransaction(ref Trans); }
        }

        protected void lknDetailShow_Click(object sender, EventArgs e)
        {
            lknDetailShow.Visible = false;
            lknDetailHide.Visible = true;
            TabContainer1.Visible = true;
        }

        protected void lknDetailHide_Click(object sender, EventArgs e)
        {
            lknDetailShow.Visible = true;
            lknDetailHide.Visible = false;
            TabContainer1.Visible = false;
        }

        protected void InsCusGVD_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (txtJobCardType.Text == "INSURANCE")
                {
                    totalCustax = totalCustax + double.Parse(e.Row.Cells[11].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[11].Text.Replace("&nbsp;", "").Trim()) +
                                       double.Parse(e.Row.Cells[13].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[13].Text.Replace("&nbsp;", "").Trim()) +
                                       double.Parse(e.Row.Cells[15].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[15].Text.Replace("&nbsp;", "").Trim());
                    if (totalCustax > 0)
                    {


                        totalCusAmtwithTax = totalCusAmtwithTax + double.Parse(e.Row.Cells[4].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[4].Text.Replace("&nbsp;", "").Trim()) *
                            double.Parse(e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim());
                    }
                }
                else
                {
                    totaltax = totalCustax + double.Parse(e.Row.Cells[11].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[11].Text.Replace("&nbsp;", "").Trim()) +
                                                           double.Parse(e.Row.Cells[13].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[13].Text.Replace("&nbsp;", "").Trim()) +
                                                           double.Parse(e.Row.Cells[15].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[15].Text.Replace("&nbsp;", "").Trim());
                    totalCusAmtwithTax = totalCusAmtwithTax + double.Parse(e.Row.Cells[4].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[4].Text.Replace("&nbsp;", "").Trim()) *
                        double.Parse(e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim());
                }


            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                totalCustax = totalCusAmtwithTax = 0;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                lblCusTaxAmt.Text = Convert.ToString(totalCustax);
                lblCusTotalWithTax.Text = Convert.ToString(totalCusAmtwithTax+totalCustax);
                // txtJobCardTotal.Text = Convert.ToString(totEst);
            }

        }

        protected void imgCustomerCode_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 1;
            ViewState["State"] = "Customer";

            clslook.LU_Get_Customer(imgCustomerCode, ViewState["lookupid"].ToString(), "DealerCode = '" + Session["DealerCode"].ToString() + "'", "../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
            btnValue = 3;
        }
        // Gate Pass Work
       #region
        protected void btnGatePass_Click(object sender, EventArgs e)
        {
            try
            {
                string oldgatepass;
                oldgatepass= grl.GetStringValuesAgainstCodes("Module='Service' and TransCode='" + txtJobCardCode.Text + "'", "GatePassTemp", "GatePassCode");
                if (ddlInvoiceNo.SelectedIndex == 0)
                {
                    lblMsg.Text = "Please Select the Invoice First!";
                    lblMsg.ForeColor = Color.Red;
                    return;
                }

                //if (!sysfun.IsExist("InvoiceNo", ddlInvoiceNo.SelectedValue.ToString(), "PaymentReceiptDetail", Session["DealerCode"].ToString()))
                //{
                //    lblMsg.Text = "Create a Payment Receipt First!";
                //    lblMsg.ForeColor = Color.Red;
                //    return;

                //}
                if (oldgatepass == null||oldgatepass=="")
                {


                    SqlParameter[] GatePass_param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5), 
           
           /*1*/ new SqlParameter("@GatePassCode",SqlDbType.Char,8),
          // /*2*/ new SqlParameter("@GatePassDate",SqlDbType.DateTime),     
           /*4*/ new SqlParameter("@TransCode",SqlDbType.VarChar,8),       
           /*3*/ new SqlParameter("@GatePassType",SqlDbType.Char,1),         
           /*4*/ new SqlParameter("@Remarks",SqlDbType.Char,100),          
            /*6*/ new SqlParameter("@Module",SqlDbType.VarChar,50),            
           /*7*/ new SqlParameter("@UpdUser",SqlDbType.Char,50),
            /*9*/ new SqlParameter("@UpdTerm",SqlDbType.Char,50),
           /*8*/ new SqlParameter("@DelFlag",SqlDbType.Char,1),
           /*4*/ new SqlParameter("@InvoiceNo",SqlDbType.VarChar,8),

            };
                    string gatepass = myFunc.AutoGen("GatePassTemp", "GatePassCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                    GatePass_param[0].Value = Session["DealerCode"].ToString();
                    GatePass_param[1].Value = gatepass;
                    GatePass_param[2].Value = txtJobCardCode.Text;
                    GatePass_param[3].Value = "N";
                    GatePass_param[4].Value = "";
                    GatePass_param[5].Value = "Service";
                    GatePass_param[6].Value = Session["UserName"].ToString();
                    GatePass_param[7].Value = GlobalVar.mUserIPAddress;
                    GatePass_param[8].Value = "N";
                    GatePass_param[9].Value = ddlInvoiceNo.SelectedValue.ToString();
                    if (myFunc.ExecuteSP_NonQuery("[Sp_Insert_GatePassTemp]", GatePass_param))
                    {
                        lblMsg.Text = "Gate Created " + gatepass;
                        lblMsg.ForeColor = Color.Green;
                        MakeGatePassReport(gatepass);
                    }
                }
                else
                {
                    MakeGatePassReport(oldgatepass);
                }
            }
            catch (Exception ex){
                lblMsg.Text = ex.Message;
            }
        }
        private void MakeGatePassReport(string GatePass)
        {
            ReportDocument rpt, crReportDocument, RD;
            string strCriteria, rptTitle;
            DateTime FromDate, ToDate;
            SqlDataReader rder;
            SysFunctions myFunc = new SysFunctions();
            Data.DSReports DSReports = new Data.DSReports();


            RD = new ReportDocument();
            string CCon = CConnection.GetConnectionString();
            // PartItemLedger.sp_ItemStockLedger_select.Load(ds.CreateDataReader());
            DataSet ds = new DataSet();
            //SqlParameter[] param = {
            //                       new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
            //                       new SqlParameter("@JobCardCode",SqlDbType.Char,8),//0
            //                       };
            //param[0].Value = ConfigurationManager.AppSettings["DealerCode"].ToString();
            //param[1].Value = Request.QueryString["JobCardCode"].ToString();
            if (txtJobCardCode.Text.Trim() != "") //ddlJobCardCode.SelectedIndex
            {
                ViewState["JobCardCode"] = txtJobCardCode.Text.Trim(); // ddlJobCardCode.SelectedValue.ToString().Trim();
            }


            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_GatePassMaster_Print '" + this.Session["DealerCode"].ToString() + "','" + GatePass + "'");

            // QRCodeEncoder encoder = new QRCodeEncoder();

            // Bitmap bi = encoder.Encode(ds.Tables[0].Rows[0]["DealerCode"].ToString() + "" + ds.Tables[0].Rows[0]["JobCardCode"].ToString());

            // bi.SetPixel(10, 10, Color.Red);

            //  bi.Save(Server.MapPath("~/Images/QrCode.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);


            DSReports.sp_GatePassMaster_Print.Load(ds.CreateDataReader());


           

            string FileRptPath = Server.MapPath("~/Modules/Service/ServiceReports/");


            RD.Load(Server.MapPath("~/Modules/Service/ServiceReports/rptGatePass.rpt"));
           


            RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Gate Pass'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";

            RD.DataDefinition.FormulaFields["QRCode"].Text = "'" + Server.MapPath("~") + "/Images/QrCode.jpg'";

            //DataTable dt = new DataTable();

            //string sql = "Select Logo from Dealer where DealerCode = '" + Session["DealerCode"].ToString() + "'";

            //dt = myFunc.GetData(sql);

            //RD.DataDefinition.FormulaFields["Logo"].Text = "'"+dt.Rows[0]["Logo"].ToString()+"'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";


            // CrystalReportViewer1.ReportSource = rpt;
            RD.SetDataSource(DSReports.sp_JobCardMaster_Print.DataSet);

            Session["RD"] = RD;



            // prepare pdf and show

            // create pdf 
            string FilePath = Server.MapPath("~/Download/");
            string FileName = "CounterSalesDetail" + Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;
            //.SetDatabaseLogon("SDMS", "sdms161", "192.168.1.47", "SDMS");
            RD.ExportToDisk(ExportFormatType.Excel, File);

            string URL;
            URL = "../../../Download/rptViewerService.aspx?FileName=" + FileName;

            //URL = FilePath + "OpenPdf.aspx?FileName=" + FileName;
            //txtPartItemDesc.Text = URL;
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //URL = "../../../Download/OpenPdf.aspx?FileName=" + FileName; 
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

        }
        #endregion

        protected void btnGL_Click(object sender, EventArgs e)
        {
            string URL;

            if(txtBillPayMode.Text.Trim() == "Cash")
            {
                URL = "GL.aspx?CusInv=" + ddlInvoiceNo.SelectedValue + "&Type=PRI";
            }else
            {
                URL = "JV.aspx?CusInv=" + ddlInvoiceNo.SelectedValue + "&Type=CI";
            }
            
            string fullURL = "window.open('" + URL + "', '_blank', 'height=600,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }

        protected void btnClearForm_Click(object sender, EventArgs e)
        {

            ClearAll();
            lblMsg.Text = "";
        }

        public void ClearAll() {

            ddlInvtype.SelectedIndex = 0;
            ddlCustCode.SelectedIndex = 0;
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
            txtTaxAmount.Text = "";
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

            

            HFJobCard.Value = "";
            HFChassisNo.Value = "";
            HFCustomer.Value = "";
            HFEngineNo.Value = "";
            HFRegNo.Value = "";

            FillJobCardDetailGrid("");
            Load_ddlInvoiceNo();
            //txtInvoiceNo.DataBind();
            //txtJobCardNo.DataBind();

            //txtInvoiceNo.ReadOnly = false;
            //txtJobCardNo.ReadOnly = false;
            //lblJobCardNo.Visible = false;
            //txtJobCardNo.Visible = true;
            //sysFunc.Clearddl(Page);
            ddlJobCardCode.Visible = true;           
            txtJobCardCode.Visible = false;
            txtJobCardCode.Text = "";
            ddlJobCardCode.Enabled = true;
            Load_ddlJobCardCode();
            txtBillPayMode.Text = "";
            txtJobCardType.Text = "";
        }

        protected void ImageButtonInvNO_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 85; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_InvoiceNo(ImageButtonInvNO, ViewState["lookupid"].ToString(), "", "../../../../");

            btnValue = 2;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void ImageButtonRoNO_Click(object sender, ImageClickEventArgs e)
        {
         
            ViewState["lookupid"] = 4; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_JobCardNo_Invoice(ImageButtonRoNO, ViewState["lookupid"].ToString(), "", "../../../../");

           btnValue = 1;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

       
    }

        protected void btnPerInv_Click(object sender, EventArgs e)
        {
            string job, lub, parts, sublet;
            string strJCDecs;
            string strJCCode;
            discountpart = txtDiscountParts.Text;
            dicountlabor = txtDiscLabor.Text;
            //strJCDecs = ViewState["strJCDecs"].ToString();
            //strJCCode = ViewState["strJCCode"].ToString();
            //job = ViewState["job"].ToString();
            //lub = ViewState["lub"].ToString();
            //parts = ViewState["parts"].ToString();
            //sublet = ViewState["sublet"].ToString();

            if (RadioButton3.Checked)
            {
                if (txtJobCardType.Text == "INSURANCE")
                {
                    if (RadioButton1.Checked)
                    {
                        if (ddlInvtype.SelectedValue == "Depriciation")
                        {
                            Load_GSTInvoiceRpt("~\\Modules\\Service\\ServiceReports\\DepCust_GSTInvoiceReport.rpt");
                        }
                    }
                    else
                    {
                        Load_CombineInvoiceRptPerforma("~\\Modules\\Service\\ServiceReports\\CombineReportPerfor.rpt");
                    }
                }
                else
                {
                    Load_CombineInvoiceRptPerforma("~\\Modules\\Service\\ServiceReports\\CombineReportPerfor.rpt");
                }


            }

        }

        protected void btnPrintInv_Click(object sender, EventArgs e)
        {
            string job, lub, parts, sublet;
            string strJCDecs;
            string strJCCode;

            if (ddlInvoiceNo.SelectedIndex == 0)
            {
                sysfun.UserMsg(lblMsg,Color.Red,"First Select the Invoice");
                return;
            }
            strJCDecs = ViewState["strJCDecs"].ToString();
            strJCCode = ViewState["strJCCode"].ToString();
            job = ViewState["job"].ToString();
            lub = ViewState["lub"].ToString();
            parts = ViewState["parts"].ToString();
            sublet = ViewState["sublet"].ToString();

            if (RadioButton3.Checked)
            {
                //if(txtJobCardType.Text == "INSURANCE")
                //{
                    
                //    if (ddlInvtype.SelectedValue == "Depriciation")
                //    {
                //        Load_GSTInvoiceRpt("~\\Modules\\Service\\ServiceReports\\DepCust_GSTInvoiceReport.rpt");
                //    }
                //    else
                //    {
                //        Load_CombineInvoiceRpt("~\\Modules\\Service\\ServiceReports\\Ins_CombineReport.rpt");
                //    }
                //}
                if (ddlInvtype.SelectedValue == "Insurance")
                {
                    Load_CombineInvoiceRpt("~\\Modules\\Service\\ServiceReports\\Ins_CombineReport.rpt");
                }
                else
                {
                    Load_CombineInvoiceRptCustomer("~\\Modules\\Service\\ServiceReports\\InsCus_CombineReport.rpt");
                }

                
            }
            else
                if (strJCDecs == "FFS" || strJCDecs == "PDI" || strJCDecs == "TFS" || strJCDecs == "SFS")
            {


                if (double.Parse(parts) + double.Parse(lub) > 0 && double.Parse(sublet) + double.Parse(job) == 0)
                {
                    Load_GSTInvoiceRpt("~\\Modules\\Service\\ServiceReports\\FSK_GSTInvoiceReport.rpt");
                }
                else if (double.Parse(sublet) + double.Parse(job) > 0 && double.Parse(parts) + double.Parse(lub) == 0)
                {
                    Load_PSTInvoiceRpt("~\\Modules\\Service\\ServiceReports\\FSK_SSTInvoiceReport.rpt");
                }
                else if (RadioButton1.Checked)
                {
                    Load_GSTInvoiceRpt("~\\Modules\\Service\\ServiceReports\\FSK_GSTInvoiceReport.rpt");

                }
                else
                {
                    Load_PSTInvoiceRpt("~\\Modules\\Service\\ServiceReports\\FSK_SSTInvoiceReport.rpt");
                }

            }
            else if (strJCDecs == "WARRANTY")
            {
                if (double.Parse(parts) + double.Parse(lub) > 0 && double.Parse(sublet) + double.Parse(job) == 0)
                {
                    Load_GSTInvoiceRpt("~\\Modules\\Service\\ServiceReports\\TR_GSTInvoiceReport.rpt");
                }
                else if (double.Parse(sublet) + double.Parse(job) > 0 && double.Parse(parts) + double.Parse(lub) == 0)
                {
                    Load_PSTInvoiceRpt("~\\Modules\\Service\\ServiceReports\\TR_SSTInvoiceReport.rpt");
                }
                else if (RadioButton1.Checked)
                {
                    Load_GSTInvoiceRpt("~\\Modules\\Service\\ServiceReports\\TR_GSTInvoiceReport.rpt");

                }
                else
                {
                    Load_PSTInvoiceRpt("~\\Modules\\Service\\ServiceReports\\TR_SSTInvoiceReport.rpt");
                }
            }
            else if (strJCDecs == "INSURANCE")
            {
                if (ddlInvtype.SelectedValue == "Insurance")
                {
                    if (double.Parse(parts) + double.Parse(lub) > 0 && double.Parse(sublet) + double.Parse(job) == 0)
                    {
                        Load_GSTInvoiceRpt("~\\Modules\\Service\\ServiceReports\\Ins_GSTInvoiceReport.rpt");
                    }
                    else if (double.Parse(sublet) + double.Parse(job) > 0 && double.Parse(parts) + double.Parse(lub) == 0)
                    {
                        Load_PSTInvoiceRpt("~\\Modules\\Service\\ServiceReports\\Ins_SSTInvoiceReport.rpt");
                    }
                    else if (RadioButton1.Checked)
                    {
                        Load_GSTInvoiceRpt("~\\Modules\\Service\\ServiceReports\\Ins_GSTInvoiceReport.rpt");

                    }
                    else
                    {
                        Load_PSTInvoiceRpt("~\\Modules\\Service\\ServiceReports\\Ins_SSTInvoiceReport.rpt");
                    }

                }
                else
                {
                    Load_GSTInvoiceRpt("~\\Modules\\Service\\ServiceReports\\DepCust_GSTInvoiceReport.rpt");
                }
            }
            else
            {
                if (double.Parse(parts) + double.Parse(lub) > 0 && double.Parse(sublet) + double.Parse(job) == 0)
                {
                    Load_GSTInvoiceRpt("~\\Modules\\Service\\ServiceReports\\GRN_GSTInvoiceReport.rpt");
                }
                else if (double.Parse(sublet) + double.Parse(job) > 0 && double.Parse(parts) + double.Parse(lub) == 0)
                {
                    Load_PSTInvoiceRpt("~\\Modules\\Service\\ServiceReports\\GRN_SSTInvoiceReport.rpt");
                }
                else if (RadioButton1.Checked)
                {
                    Load_GSTInvoiceRpt("~\\Modules\\Service\\ServiceReports\\GRN_GSTInvoiceReport.rpt");

                }
                else
                {
                    Load_PSTInvoiceRpt("~\\Modules\\Service\\ServiceReports\\GRN_SSTInvoiceReport.rpt");
                }
            }


        }

        private void Load_CombineInvoiceRpt(string rpt)
        {
            ReportDocument RD = new ReportDocument();
            Data.DSReports data = new Data.DSReports();

            DataSet ds = new DataSet();

            string cCon = CConn.CConnection.GetConnectionString();

            string i = ddlInvoiceNo.SelectedValue.ToString();

            QRCodeEncoder encoder = new QRCodeEncoder();

            if (txtJobCardType.Text == "INSURANCE")
            {
                ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_CustomerInvoiceInfo_Select'" + this.Session["DealerCode"].ToString() + "','" + i + "'");
                data.sp_CustomerInvoiceInfo_Select.Load(ds.CreateDataReader());

              //  Bitmap bi = encoder.Encode(ds.Tables[0].Rows[0]["DealerCode"].ToString() + "" + ds.Tables[0].Rows[0]["InvoiceNo"].ToString().Trim() + "" + ds.Tables[0].Rows[0]["CusDesc"].ToString() + "" + ds.Tables[0].Rows[0]["NetAmountInsurance"].ToString());
            //    bi.Save(Server.MapPath("~/Images/QrCode.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "SP_CustomerInvoice_Combine'" + this.Session["DealerCode"].ToString() + "','" + i + "'");
                data.SP_CustomerInvoice_Combine.Load(ds.CreateDataReader());

              //  Bitmap bi = encoder.Encode(ds.Tables[0].Rows[0]["DealerCode"].ToString() + "" + ds.Tables[0].Rows[0]["InvoiceNo"].ToString().Trim() + "" + ds.Tables[0].Rows[0]["CusDesc"].ToString() + "" + ds.Tables[0].Rows[0]["NetAmountCustomer"].ToString());
               // bi.Save(Server.MapPath("~/Images/QrCode.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
            }



            string strJCCode = ds.Tables[0].Rows[0]["JobCardCode"].ToString();
            string InvType = ds.Tables[0].Rows[0]["InvType"].ToString();

            ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_JobCardDetail_Print '" + this.Session["DealerCode"].ToString() + "','" + strJCCode + "'");
            data.sp_JobCardDetail_Print.Load(ds.CreateDataReader());

            ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_JobCardLubDetail_Print '" +"" + "','" + strJCCode + "'");
            data.sp_JobCardLubDetail_Print.Load(ds.CreateDataReader());

            if (txtJobCardType.Text == "INSURANCE")
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
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString().ToUpper() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString().ToUpper() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString().ToUpper() + "'";
            RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString().ToUpper() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'COMBINE INVOICE'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString().ToUpper() + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString().ToUpper() + "'";
            RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString().ToUpper() + " '";
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
        private void Load_CombineInvoiceRptCustomer(string rpt)
        {
            ReportDocument RD = new ReportDocument();
            Data.DSReports data = new Data.DSReports();

            DataSet ds = new DataSet();

            string cCon = CConn.CConnection.GetConnectionString();

            string i = ddlInvoiceNo.SelectedValue.ToString();

            QRCodeEncoder encoder = new QRCodeEncoder();

            if (txtJobCardType.Text == "INSURANCE")
            {
                ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_CustomerInvoiceInfo_Select'" + this.Session["DealerCode"].ToString() + "','" + i + "'");
                data.sp_CustomerInvoiceInfo_Select.Load(ds.CreateDataReader());

                //  Bitmap bi = encoder.Encode(ds.Tables[0].Rows[0]["DealerCode"].ToString() + "" + ds.Tables[0].Rows[0]["InvoiceNo"].ToString().Trim() + "" + ds.Tables[0].Rows[0]["CusDesc"].ToString() + "" + ds.Tables[0].Rows[0]["NetAmountInsurance"].ToString());
                //    bi.Save(Server.MapPath("~/Images/QrCode.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "SP_CustomerInvoice_Combine'" + this.Session["DealerCode"].ToString() + "','" + i + "'");
                data.SP_CustomerInvoice_Combine.Load(ds.CreateDataReader());

                //  Bitmap bi = encoder.Encode(ds.Tables[0].Rows[0]["DealerCode"].ToString() + "" + ds.Tables[0].Rows[0]["InvoiceNo"].ToString().Trim() + "" + ds.Tables[0].Rows[0]["CusDesc"].ToString() + "" + ds.Tables[0].Rows[0]["NetAmountCustomer"].ToString());
                // bi.Save(Server.MapPath("~/Images/QrCode.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
            }



            string strJCCode = ds.Tables[0].Rows[0]["JobCardCode"].ToString();
            string InvType = ds.Tables[0].Rows[0]["InvType"].ToString();

            ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_JobCardDetail_Print '" + this.Session["DealerCode"].ToString() + "','" + strJCCode + "'");
            data.sp_JobCardDetail_Print.Load(ds.CreateDataReader());

            ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_JobCardLubDetail_Print '" + this.Session["DealerCode"].ToString() + "','" + strJCCode + "'");
            data.sp_JobCardLubDetail_Print.Load(ds.CreateDataReader());

            if (txtJobCardType.Text == "INSURANCE")
            {
                ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_InsDepCust_CombineInvoiceParts_Select'" + this.Session["DealerCode"].ToString() + "','" + strJCCode + "','" + InvType + "'"); //
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
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString().ToUpper() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString().ToUpper() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString().ToUpper() + "'";
            RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString().ToUpper() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'COMBINE INVOICE'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString().ToUpper() + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString().ToUpper() + "'";
            RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString().ToUpper() + " '";
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
        private void Load_CombineInvoiceRptPerforma(string rpt)
        {
            ReportDocument RD = new ReportDocument();
            Data.DSReports data = new Data.DSReports();

            DataSet ds = new DataSet();

            string cCon = CConn.CConnection.GetConnectionString();

            string i = ddlInvoiceNo.SelectedValue.ToString();

            QRCodeEncoder encoder = new QRCodeEncoder();

            //if (txtJobCardType.Text == "INSURANCE")
            //{
            //    ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_CustomerInvoiceInfo_Select'" + this.Session["DealerCode"].ToString() + "','" + i + "'");
            //    data.sp_CustomerInvoiceInfo_Select.Load(ds.CreateDataReader());

            //    Bitmap bi = encoder.Encode(ds.Tables[0].Rows[0]["DealerCode"].ToString() + "" + ds.Tables[0].Rows[0]["InvoiceNo"].ToString().Trim() + "" + ds.Tables[0].Rows[0]["CusDesc"].ToString() + "" + ds.Tables[0].Rows[0]["NetAmountInsurance"].ToString());
            //    bi.Save(Server.MapPath("~/Images/QrCode.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
            //}
            //else
            //{
            //    ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "SP_CustomerInvoice_Combine'" + this.Session["DealerCode"].ToString() + "','" + i + "'");
            //    data.SP_CustomerInvoice_Combine.Load(ds.CreateDataReader());

            //    Bitmap bi = encoder.Encode(ds.Tables[0].Rows[0]["DealerCode"].ToString() + "" + ds.Tables[0].Rows[0]["InvoiceNo"].ToString().Trim() + "" + ds.Tables[0].Rows[0]["CusDesc"].ToString() + "" + ds.Tables[0].Rows[0]["NetAmountCustomer"].ToString());
            //    bi.Save(Server.MapPath("~/Images/QrCode.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
            //}

            SqlDataReader rder = null;

            string strJCCode = txtJobCardCode.Text;
            string InvType = "Inv";
            SqlParameter[] dsMasterParam = { new SqlParameter("@DealerCode",SqlDbType.Char,5),  new SqlParameter("@JobCardCode",SqlDbType.Char,8),
                                         new SqlParameter("@RegNo",SqlDbType.VarChar,50),   new SqlParameter("@EngineNo",SqlDbType.VarChar,50),
                                         new SqlParameter("@ChassisNo",SqlDbType.VarChar,50),new SqlParameter("@CusCode",SqlDbType.VarChar,50)
        };
            dsMasterParam[0].Value = Session["DealerCode"].ToString();
            if (HFJobCard.Value == "" & txtRegNo.Text == "" & txtEngineNo.Text == null & txtChassisNo.Text == ""
                & HFCustomer.Value == "") dsMasterParam[1].Value = "0";
            else dsMasterParam[1].Value = (HFJobCard.Value.Trim() == "" ? null : HFJobCard.Value.Trim());
            dsMasterParam[2].Value = (txtRegNo.Text == "" ? null : txtRegNo.Text);
            dsMasterParam[3].Value = (txtEngineNo.Text == "" ? null : txtEngineNo.Text);
            dsMasterParam[4].Value = (txtChassisNo.Text == "" ? null : txtChassisNo.Text);
            dsMasterParam[5].Value = null;

            DataTable dsJobCardMaster = new DataTable(); 

            sysfun.ExecuteSP("sp_W2_JobCard_Master_Select", dsMasterParam, ref rder);
            dsJobCardMaster = data.sp_W2_JobCard_Master_Select;
            data.EnforceConstraints = false;
            data.sp_W2_JobCard_Master_Select.Load(rder);

            ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_JobCardDetail_Print '" + this.Session["DealerCode"].ToString() + "','" + strJCCode + "'");
            data.sp_JobCardDetail_Print.Load(ds.CreateDataReader());

            ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_JobCardButPartsDetail_Print '" + this.Session["DealerCode"].ToString() + "','" + strJCCode + "'");
            data.sp_JobCardLubDetail_Print.Load(ds.CreateDataReader());

            if (txtJobCardType.Text == "INSURANCE")
            {
                InvType = "Dep";
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
            RD.DataDefinition.FormulaFields["Customer"].Text = "'" + dsJobCardMaster.Rows[0]["Customer"].ToString() + "'";
            RD.DataDefinition.FormulaFields["KM"].Text = "'" + dsJobCardMaster.Rows[0]["KM"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Address"].Text = "'" + dsJobCardMaster.Rows[0]["Address1"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Phone"].Text = "'" + dsJobCardMaster.Rows[0]["Phone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Cell"].Text = "'" + dsJobCardMaster.Rows[0]["CellNo"].ToString() + "'";
            RD.DataDefinition.FormulaFields["NIC"].Text = "'" + dsJobCardMaster.Rows[0]["NIC"].ToString() + "'";
            RD.DataDefinition.FormulaFields["NTNno"].Text = "'" + dsJobCardMaster.Rows[0]["NTNno"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ContPersonName"].Text = "'"+ dsJobCardMaster.Rows[0]["ContPersonName"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ContPersonCell"].Text = "'" + dsJobCardMaster.Rows[0]["ContPersonCell"].ToString() + "'";
            RD.DataDefinition.FormulaFields["EngineNo"].Text =    "'"+txtEngineNo.Text+"'";
            RD.DataDefinition.FormulaFields["ChassisNo"].Text = "'" + txtChassisNo.Text + "'";
            RD.DataDefinition.FormulaFields["EndUser"].Text =     "'"+txtUserName.Text+"'";
            RD.DataDefinition.FormulaFields["RegNo"].Text =       "'"+txtRegNo.Text+"'";
            RD.DataDefinition.FormulaFields["ProdCode"].Text =    "'"+txtProductCode.Text+"'";
            RD.DataDefinition.FormulaFields["ProdName"].Text =    "'"+txtProductName.Text+"'";
            RD.DataDefinition.FormulaFields["JobCardCode"].Text = "'"+txtJobCardCode.Text+"'";
            RD.DataDefinition.FormulaFields["PaymentType"].Text = "'"+txtBillPayMode.Text+"'";
            RD.DataDefinition.FormulaFields["PSTAmountPerforma"].Text = "'" + txtPSTAmount.Text + ".00"+"'";
            RD.DataDefinition.FormulaFields["GSTAmountPerforma"].Text = "'" + txtGSTAmount.Text + ".00" + "'";
            RD.DataDefinition.FormulaFields["DiscountLabor"].Text ="'"+ txthdnLabors.Value + ".00" +"'";
            RD.DataDefinition.FormulaFields["DiscountPart"].Text = "'" + txthdnParts.Value + ".00" + "'";
            RD.DataDefinition.FormulaFields["DiscountPartPer"].Text = "'" + txtDiscLaborPercent.Text + "'";
            RD.DataDefinition.FormulaFields["DiscountPartPer"].Text = "'" + txtDiscPercentPart.Text + "'";
            RD.DataDefinition.FormulaFields["PSTPer"].Text = "'" + txtPSTPercent.Text + "'";
            RD.DataDefinition.FormulaFields["GSTPer"].Text = "'" + txtGSTPercent.Text + "'";
            RD.DataDefinition.FormulaFields["GrossAmountPerforma"].Text = "'" + txtGrossAmount.Text +".00"+ "'";
            RD.DataDefinition.FormulaFields["SubTotalPerforma"].Text = "'" + txtSubTotal.Text +".00"+ "'";
            RD.DataDefinition.FormulaFields["TotalNetPerforma"].Text = "'" + txtTotalAmtCustomer.Text + ".00"+ "'";
            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'COMBINE INVOICE'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
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
        private void Load_PSTInvoiceRpt(string rpt)
        {
            ReportDocument RD = new ReportDocument();
            Data.DSReports data = new Data.DSReports();

            DataSet ds = new DataSet();

            string cCon = CConn.CConnection.GetConnectionString();

            string i = ddlInvoiceNo.SelectedValue.ToString();

            ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_CustomerInvoiceInfo_Select'" + this.Session["DealerCode"].ToString() + "','" + i + "'");
            data.sp_CustomerInvoiceInfo_Select.Load(ds.CreateDataReader());

            string strJCCode = ds.Tables[0].Rows[0]["JobCardCode"].ToString();


            ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_InvoiceJobs_Select '" + this.Session["DealerCode"].ToString() + "','" + strJCCode + "'");
            data.sp_InvoiceJobs_Select.Load(ds.CreateDataReader());

            ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_InvoiceSublet_Select '" + this.Session["DealerCode"].ToString() + "','" + strJCCode + "'");
            data.sp_InvoiceSublet_Select.Load(ds.CreateDataReader());

            //RD.Load(Server.MapPath("~\\Reports\\GRN_SSTInvoiceReport.rpt"));
            RD.Load(Server.MapPath(rpt));
            

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'PROVINCIAL SALES TAX INVOICE'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";

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

        private void Load_GSTInvoiceRpt(string rpt)
        {
            Data.DSReports data = new Data.DSReports();
            ReportDocument RD = new ReportDocument();

            string cCon = CConn.CConnection.GetConnectionString();
            DataSet ds = new DataSet();

            string i = ddlInvoiceNo.SelectedValue.ToString();

            ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_CustomerInvoiceInfo_Select'" + this.Session["DealerCode"].ToString() + "','" + i + "'");
            data.sp_CustomerInvoiceInfo_Select.Load(ds.CreateDataReader());

            string strJCCode = ds.Tables[0].Rows[0]["JobCardCode"].ToString();

            if (txtJobCardType.Text == "INSURANCE")
            {
                ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_DepCust_GSTInvoiceParts_Select'" + this.Session["DealerCode"].ToString() + "','" + strJCCode + "','" + ds.Tables[0].Rows[0]["InvType"].ToString().Trim() + "'"); //
                data.sp_DepCust_GSTInvoiceParts_Select.Load(ds.CreateDataReader());
            }
            else
            {
                ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_GSTInvoiceParts_Select'" + this.Session["DealerCode"].ToString() + "','" + strJCCode + "'");
                data.sp_GSTInvoiceParts_Select.Load(ds.CreateDataReader());
            }
            //doc = new ReportDocument();
            //doc.Load(Server.MapPath("MR.rpt"));
            
            RD.Load(Server.MapPath(rpt));           


            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString() + "'";
            if(txtJobCardType.Text == "INSURANCE")
            {
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Depreciation Invoice'";
            }
            else
            {
                RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'GENERAL SALES TAX INVOICE'";
            }
           
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";

            RD.Database.Tables[0].SetDataSource(data);


            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "InvoiceGST" + Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;

            Session["RD"] = RD;
            //RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            string URL;
            URL = "../../../Download/rptViewerService.aspx?FileName=" + FileName;
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);


        }
      

        protected void LoadReportData()
        {
            string job, lub, parts, sublet;
            string strJCDecs;
            string strJCCode;

            Data.DSReports data = new Data.DSReports();


            DataSet ds = new DataSet();

            string cCon = CConn.CConnection.GetConnectionString();

            string i = ddlInvoiceNo.SelectedValue.ToString();

            ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_JobTypeDecs'" + this.Session["DealerCode"].ToString() + "','" + i + "'");
            data.sp_CustomerInvoiceInfo_Select.Load(ds.CreateDataReader());

            strJCDecs = ds.Tables[0].Rows[0]["JobTypeDesc"].ToString();

            ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(cCon, CommandType.Text, "sp_CustomerInvoiceInfo_Select'" + this.Session["DealerCode"].ToString() + "','" + i + "'");
            data.sp_CustomerInvoiceInfo_Select.Load(ds.CreateDataReader());

            strJCCode = ds.Tables[0].Rows[0]["JobCardCode"].ToString();

            DataTable dt = new DataTable();

            string sql = "select JobsTotal,PartsTotal,LubPartsTotal,SubletTotal from JobCardMaster where JobCardCode = '" + strJCCode + "' and DealerCode = '"+Session["DealerCode"].ToString()+"'";

            dt = myFunc.GetData(sql);



            job = Convert.ToDecimal(dt.Rows[0]["JobsTotal"].ToString()).ToString("#########0");
            lub = Convert.ToDecimal(dt.Rows[0]["LubPartsTotal"].ToString()).ToString("#########0");
            parts = Convert.ToDecimal(dt.Rows[0]["PartsTotal"].ToString()).ToString("#########0");
            sublet = Convert.ToDecimal(dt.Rows[0]["SubletTotal"].ToString()).ToString("#########0");

            if (double.Parse(parts) + double.Parse(lub) > 0 && double.Parse(sublet) + double.Parse(job) > 0)
            {
                lblMsg.Text = "Please Select GST/PST/Combine Report";

            }
            ViewState["strJCDecs"] = strJCDecs.Trim();
            ViewState["strJCCode"] = strJCCode;
            ViewState["job"] = job;
            ViewState["lub"] = lub;
            ViewState["parts"] = parts;
            ViewState["sublet"] = sublet;


        }
        private void PaymentReceiptEntry(ref string strReceiptNo)
        {

            try
            {
                if (Inert_PaymentReceiptMaster(ref strReceiptNo))
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
                    PmtRecDetail_param[2].Value = "Service";
                    PmtRecDetail_param[3].Value = ViewState["InvoiceNo"].ToString();
                    PmtRecDetail_param[4].Value = myFunc.SaveDate(txtInvoiceDate.Text);
                    PmtRecDetail_param[5].Value = txtTotalAmtCustomer.Text;
                    PmtRecDetail_param[6].Value = txtTotalAmtCustomer.Text;
                    PmtRecDetail_param[7].Value = txtTotalAmtCustomer.Text;


                    myFunc.ExecuteSP_NonQuery("[sp_W2_PaymentReceipt_Detail_Insert]", PmtRecDetail_param, Trans);
                            //Updation Of invoice
                            
                    //SqlParameter[] PmtRecTaxDetail_param = {                                            
                    //                                        /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
                    //                                        /*1*/ new SqlParameter("@ReceiptNo",SqlDbType.Char,8),
                    //                                        /*2*/ new SqlParameter("@ReceiptHead",SqlDbType.Char,50),
                    //                                        /*3*/ new SqlParameter("@AccountCode",SqlDbType.Char,1),
                    //                                        /*4*/ new SqlParameter("@Amount",SqlDbType.Decimal),
                    //                                        /*5*/ new SqlParameter("@TaxID",SqlDbType.Char,2),
                    //                                        /*6*/ new SqlParameter("@TaxPerc",SqlDbType.Decimal),
                    //                                       };

                    //PmtRecTaxDetail_param[0].Value = Session["DealerCode"].ToString();
                    //PmtRecTaxDetail_param[1].Value = strReceiptNo;
                    //foreach (GridViewRow row in gvTaxDetail.Rows)
                    //{
                    //    Label lblRecHead = (Label)row.FindControl("lblRecHead");
                    //    Label lblAmount = (Label)row.FindControl("lblAmount");
                    //    Label lblTaxID = (Label)row.FindControl("lblTaxID");
                    //    Label lblPercent = (Label)row.FindControl("lblPercent");
                    //    if (lblRecHead.Text != "")
                    //    {
                    //        PmtRecTaxDetail_param[2].Value = lblRecHead.Text;
                    //        PmtRecTaxDetail_param[3].Value = "";
                    //        PmtRecTaxDetail_param[4].Value = Convert.ToDecimal(lblAmount.Text.Trim());
                    //        PmtRecTaxDetail_param[5].Value = lblTaxID.Text.Trim();
                    //        PmtRecTaxDetail_param[6].Value = Convert.ToDecimal(lblPercent.Text.Trim());
                    //        SysFuncs.ExecuteSP_NonQuery("[sp_W2_PaymentReceipt_TaxDetail_Insert]", PmtRecTaxDetail_param, Trans);
                    //    }
                    //}
                    

                }
                else
                {
                    myFunc.UserMsg(lblMsg, Color.Red, "Operation Failed! Record Not Saved.");
                    ObjTrans.RollBackTransaction(ref Trans);
                    return;
                }

                if(ObjTrans.CommittTransaction(ref Trans))
                {
                    string IQuery = "Update dbo.CustomerInvoice set PaidC = '" + txtTotalAmtCustomer.Text + "' " +
                      "Where DealerCode='" + Session["DealerCode"].ToString() + "' and JobCardCode ='" + txtJobCardCode.Text + "'";

                    sysfun.ExecuteQuery_NonQuery(IQuery);
                }
                lblMsg.Visible = true;
                //myFunc.UserMsg(lblMsg, Color.Green, "Record Saved Successfully: " + strReceiptNo);

            }
            catch (Exception ex) { ObjTrans.RollBackTransaction(ref Trans); throw ex; }
        }
        private bool Inert_PaymentReceiptMaster(ref string strReceiptNo)
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
            PmtRecMaster_param[2].Value = myFunc.SaveDate(txtInvoiceDate.Text); //EstimateCode        
            PmtRecMaster_param[3].Value = "Service";
            PmtRecMaster_param[4].Value = ddlCustCode.SelectedValue;
            PmtRecMaster_param[5].Value = "";
            PmtRecMaster_param[6].Value = "";
            PmtRecMaster_param[7].Value = "N";
            PmtRecMaster_param[8].Value = txtRemarksUser.Text;
            PmtRecMaster_param[9].Value = "C";
            PmtRecMaster_param[10].Value = "";

            PmtRecMaster_param[11].Value = myFunc.SaveDate(txtInvoiceDate.Text);
            PmtRecMaster_param[12].Value = CustomCDBL(txtTotalAmtCustomer.Text);
            PmtRecMaster_param[13].Value = "";
            PmtRecMaster_param[14].Value = "";
            PmtRecMaster_param[15].Value = "0";
            PmtRecMaster_param[16].Value = CustomCDBL(txtTotalAmtCustomer.Text);
            PmtRecMaster_param[17].Value = CustomCDBL(txtTotalAmtCustomer.Text);
            PmtRecMaster_param[18].Value = CustomCDBL(txtTotalAmtCustomer.Text);
            PmtRecMaster_param[19].Value = "N";

            PmtRecMaster_param[20].Value = Session["UserName"].ToString();
            PmtRecMaster_param[21].Value = GlobalVar.mUserIPAddress;

            PmtRecMaster_param[22].Value = "";
            PmtRecMaster_param[23].Value = "N";
            PmtRecMaster_param[24].Value = "0";
            PmtRecMaster_param[25].Value = "Customer";
            
                PmtRecMaster_param[26].Value = "N";
                PmtRecMaster_param[27].Value = (object)DBNull.Value;
                PmtRecMaster_param[28].Value = (object)DBNull.Value;
                PmtRecMaster_param[29].Value = (object)DBNull.Value;
                PmtRecMaster_param[30].Value = (object)DBNull.Value;

            try
            {
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    strReceiptNo = myFunc.AutoGen("PaymentReceiptMaster", "ReceiptNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                    PmtRecMaster_param[1].Value = strReceiptNo;
                    if (myFunc.ExecuteSP_NonQuery("[sp_W2_PaymentReceipt_Master_Insert]", PmtRecMaster_param, Trans))
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
                myFunc.UserMsg(lblMsg, Color.Red, ex.Message);
                return false;
            }
        }
        protected void gvLubParts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
       
    }
}