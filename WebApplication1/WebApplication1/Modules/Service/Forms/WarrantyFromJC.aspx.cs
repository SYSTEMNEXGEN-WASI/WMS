using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;

namespace DXBMS.Modules.Service.Forms
{
    public partial class WarrantyFromJC : System.Web.UI.Page
    {

        DataTable dttemp = new DataTable();
        MainBLL ObjMainBll = new MainBLL();
        SysFunctions ObjGenral = new SysFunctions();
        SecurityBll sec = new SecurityBll();
        SysFunction sysFunc = new SysFunction();
        clsLookUp ObjLookup = new clsLookUp();
        DataTable dttable = new DataTable();
        DataSet dsJENDEtail;
        Transaction ObjTrans = new Transaction();
        clsLookUp clslook = new clsLookUp();
        SqlTransaction Trans;
        DataSet ds;
        double TotalAmount = 0, GST = 0, PST = 0, Labor = 0, Part = 0,Handling=0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2583", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }

            if (Page.IsPostBack)
            {

                if (Session["LookUpData"]!=null)
                {
                    lookUptext(Session["LookUpData"].ToString());
                }
            }
            if (!IsPostBack)
            {
                txtBillDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                LoadEmptyGrid();
                LoadddlType();
                LoadddlBillingNo();
            }
          
            Session["LookUpData"] = null;


        }

        private void LoadddlBillingNo()
        {
            string WhereClause = "DealerCode = '" + Session["DealerCode"] + "' and DelFlag='N'";

            string[] Columns = new string[] { "JEnBillNo", "BillDate", "Remarks" };
            sysFunc.GetMultiColumnsDDL(ddlBillingNo, Columns, "JENBillMaster ", WhereClause, "JENBillNo", "", false, false);

        }
        private void LoadddlType()
        {
            ddlTypes.Items.Add(new ListItem("Select", "Select"));
            ddlTypes.Items.Add(new ListItem("WAP", "WAP"));
            ddlTypes.Items.Add(new ListItem("NAP", "NAP"));
          
        }



        private void LoadEmptyGrid()
        {
            try
            {
                dttable.Columns.Add("PPRCode");
                dttable.Columns.Add("PPRDate");
                dttable.Columns.Add("VehChassisNo");
                dttable.Columns.Add("ProdDesc");
                dttable.Columns.Add("SubPart");
                dttable.Columns.Add("HandlingAmount");
                dttable.Columns.Add("GSTAmount");
                dttable.Columns.Add("SubSublet");
                dttable.Columns.Add("PSTSubletAmount");
                dttable.Columns.Add("SubLabor");
                dttable.Columns.Add("PSTLaborAmount");
                dttable.Columns.Add("TotalParts");
                dttable.Columns.Add("TotalSublet");
                dttable.Columns.Add("TotalLabor");
                dttable.Columns.Add("TotalAmount");
                dttable.AcceptChanges();
                Session["BillingDetails"] = dttable;
                AddNewRow();

            }
            catch (Exception ex) { throw ex; }

        }

        private void AddNewRow()
        {
            try
            {
                dttemp = (DataTable)Session["BillingDetails"];
                DataRow dr;
                dr = dttemp.NewRow();
                dr["PPRCode"] = "";
                dr["PPRDate"] = "";
                dr["VehChassisNo"] = "";
                dr["ProdDesc"] = "";
                dr["TotalParts"] = "";
                dr["TotalSublet"] = "";
                dr["TotalLabor"] = "";
                dr["TotalAmount"] = "";
                dttemp.Rows.Add(dr);
                dttemp.AcceptChanges();
                Grdbillings.DataSource = dttemp;
                Grdbillings.DataBind();
            }
            catch (Exception ex) { throw ex; }
        }
        //protected void btnListJobCards_Click(object sender, EventArgs e)
        //{
        //    //Calculating PST Amount here 
        //    string RegionCode = ObjGenral.GetStringValuesAgainstCodes("DealerCode='" + Session["DealerCode"].ToString() + "'", "Dealer", "Region");
        //    //Calculating PST Amount here 

        //    if (RegionCode.Trim() == "AOK")
        //    {
        //        DataSet ds = new DataSet();
        //        ObjGenral.ExecuteQuery("Select * from Dealer", ref ds);
        //        //txtPerc.Text = ds.Tables[0].Rows[0]["PST"].ToString();

        //        //string BillNo; BillNo = txtBillingNo.Text.Trim().ToString();
        //        string FromDate = ObjGenral.SaveDate(txtFromDate.Text).ToString();
        //        string ToDate = ObjGenral.SaveDate(txtToDate.Text).ToString();


        //        if (ddlTypes.SelectedItem.Text == "--Select--")
        //        {
        //            lblMessage.Text = "Please select a type !!!";
        //        }
        //        else
        //        {
        //            lblMessage.Text = "";
        //            if (ddlTypes.SelectedItem.Text == "FFI")
        //            {
        //                SqlParameter[] param = {
        //                           new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
        //                           new SqlParameter("@billNo",SqlDbType.VarChar),//0
        //                           new SqlParameter("@FromDate",SqlDbType.DateTime),//0
        //                           new SqlParameter("@ToDate",SqlDbType.DateTime),//0
        //                       };
        //                param[0].Value = Session["DealerCode"].ToString();
        //                param[1].Value = BillNo.ToString();
        //                param[2].Value = FromDate;
        //                param[3].Value = ToDate;

        //                ds = ObjGenral.FillDataSet("sp_2W_Service_GetFFIBillingData", param);

        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    Grdbillings.DataSource = ds;
        //                    Grdbillings.DataBind();

        //                    if (ds.Tables[0].Rows.Count > 0)
        //                    {
        //                        foreach (GridViewRow gvr in Grdbillings.Rows)
        //                        {
        //                            TotalAmount = TotalAmount + Convert.ToDecimal(ds.Tables[0].Rows[0]["PaidC"].ToString());
        //                            CheckBox Chk = (CheckBox)gvr.Cells[0].FindControl("ChkBox");
        //                            Chk.Checked = true;
        //                            ChkBox.Checked = true;
        //                        }
        //                        txtTotalAmount.Text = TotalAmount.ToString();
        //                        //CalculatePSTPercentage();
        //                    }
        //                }
        //                else
        //                {
        //                    //txtPstAmt.Text = "";
        //                    //txtPerc.Text = "";
        //                    lblMessage.Text = "No Record Found !!!";
        //                }
        //            }
        //            else
        //            {
        //                //lblMessage.Text = "No Record Found !!!";
        //                LoadEmptyGrid();
        //            }

        //            if (ddlTypes.SelectedItem.Text == "SFI")
        //            {
        //                SqlParameter[] param = {
        //                           new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
        //                           new SqlParameter("@billNo",SqlDbType.VarChar),//0
        //                           new SqlParameter("@FromDate",SqlDbType.DateTime),//0
        //                           new SqlParameter("@ToDate",SqlDbType.DateTime),//0
        //                       };
        //                param[0].Value = Session["DealerCode"].ToString();
        //                param[1].Value = BillNo.ToString();
        //                param[2].Value = FromDate;
        //                param[3].Value = ToDate;

        //                ds = ObjGenral.FillDataSet("sp_2W_Service_GetSFIBillingData", param);
        //                Grdbillings.DataSource = ds;
        //                Grdbillings.DataBind();

        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    foreach (GridViewRow gvr in Grdbillings.Rows)
        //                    {
        //                        TotalAmount = TotalAmount + Convert.ToDecimal(ds.Tables[0].Rows[0]["FFI"].ToString());
        //                        CheckBox Chk = (CheckBox)gvr.Cells[0].FindControl("ChkBox");
        //                        Chk.Checked = true;
        //                        ChkBox.Checked = true;
        //                    }
        //                    txtTotalAmount.Text = TotalAmount.ToString();
        //                    CalculatePSTPercentage();
        //                }
        //            }
        //            else
        //            {
        //                //lblMessage.Text = "No Record Found !!!";
        //                //LoadEmptyGrid();
        //            }
        //        }
        //    }

        //    else
        //    {
        //        string BillNo; BillNo = txtBillingNo.Text.Trim().ToString();
        //        string FromDate = txtFromDate.Text;
        //        string ToDate = txtToDate.Text;


        //        if (ddlTypes.SelectedItem.Text == "--Select--")
        //        {
        //            lblMessage.Text = "Please select a type !!!";
        //        }
        //        else
        //        {
        //            lblMessage.Text = "";
        //            if (ddlTypes.SelectedItem.Text == "FFI")
        //            {
        //                SqlParameter[] param = {
        //                           new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
        //                           new SqlParameter("@billNo",SqlDbType.VarChar),//0
        //                           new SqlParameter("@FromDate",SqlDbType.DateTime),//0
        //                           new SqlParameter("@ToDate",SqlDbType.DateTime),//0
        //                       };
        //                param[0].Value = Session["DealerCode"].ToString();
        //                param[1].Value = BillNo.ToString();
        //                param[2].Value = ObjGenral.SaveDate(FromDate);
        //                param[3].Value = ObjGenral.SaveDate(ToDate);

        //                ds = ObjGenral.FillDataSet("sp_2W_Service_GetFFIBillingData", param);
        //                Grdbillings.DataSource = ds;
        //                Grdbillings.DataBind();

        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    foreach (GridViewRow gvr in Grdbillings.Rows)
        //                    {
        //                        TotalAmount = TotalAmount + Convert.ToDecimal(ds.Tables[0].Rows[0]["FFI"].ToString());
        //                        CheckBox Chk = (CheckBox)gvr.Cells[0].FindControl("ChkBox");
        //                        Chk.Checked = true;
        //                        ChkBox.Checked = true;
        //                    }
        //                    txtTotalAmount.Text = TotalAmount.ToString();
        //                    //CalculatePSTPercentage();
        //                    //txtPerc.Text = "0";
        //                    //txtPstAmt.Text = "0";
        //                }
        //            }
        //            else
        //            {
        //                //lblMessage.Text = "No Record Found !!!";
        //                LoadEmptyGrid();
        //            }

        //            if (ddlTypes.SelectedItem.Text == "SFI")
        //            {
        //                SqlParameter[] param = {
        //                           new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
        //                           new SqlParameter("@billNo",SqlDbType.VarChar),//0
        //                           new SqlParameter("@FromDate",SqlDbType.DateTime),//0
        //                           new SqlParameter("@ToDate",SqlDbType.DateTime),//0
        //                       };
        //                param[0].Value = Session["DealerCode"].ToString();
        //                param[1].Value = BillNo.ToString();
        //                param[2].Value = FromDate;
        //                param[3].Value = ToDate;

        //                ds = ObjGenral.FillDataSet("sp_2W_Service_GetSFIBillingData", param);
        //                Grdbillings.DataSource = ds;
        //                Grdbillings.DataBind();

        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    foreach (GridViewRow gvr in Grdbillings.Rows)
        //                    {
        //                        TotalAmount = TotalAmount + Convert.ToDecimal(ds.Tables[0].Rows[0]["FFI"].ToString());
        //                        CheckBox Chk = (CheckBox)gvr.Cells[0].FindControl("ChkBox");
        //                        Chk.Checked = true;
        //                        ChkBox.Checked = true;
        //                    }
        //                    txtTotalAmount.Text = TotalAmount.ToString();
        //                    //CalculatePSTPercentage();
        //                    //txtPerc.Text = "0";
        //                    //txtPstAmt.Text = "0";
        //                }
        //            }
        //            else
        //            {
        //                //lblMessage.Text = "No Record Found !!!";
        //                //LoadEmptyGrid();
        //            }
        //        }
        //    }
        //}

        private void CalculatePSTPercentage()
        {
            double percentage, TotalAmount;

            //percentage = Convert.ToDouble(txtPerc.Text.Trim());
            //TotalAmount = Convert.ToDouble(txtTotalAmount.Text.Trim());
            //txtPstAmt.Text = Convert.ToString((percentage / 100) * TotalAmount);
        }

        protected void ChkBoxHeader_CheckedChanged(object sender, EventArgs e)
        {

        }
        //protected void ChkBox_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (ChkBox.Checked)
        //    {
        //        foreach (GridViewRow gvr in Grdbillings.Rows)
        //        {
        //            TotalAmount = TotalAmount + Convert.ToDecimal(gvr.Cells[5].Text.Trim());
        //            CheckBox Chk = (CheckBox)gvr.Cells[0].FindControl("ChkBox");
        //            Chk.Checked = true;
        //        }
        //        txtTotalAmount.Text = TotalAmount.ToString();
        //        CalculatePSTPercentage();
        //    }
        //    else
        //    {
        //        foreach (GridViewRow gvr in Grdbillings.Rows)
        //        {
        //            CheckBox Chk = (CheckBox)gvr.Cells[0].FindControl("ChkBox");
        //            Chk.Checked = false;
        //            txtTotalAmount.Text = "0.00";
        //            //txtPstAmt.Text = "0.00";
        //        }
        //    }

        //}
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2583", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            try
            {
                if (ddlTypes.SelectedValue == "Select")
                {
                    sysFunc.UserMsg(lblMessage, Color.Red, "Please Select the Type...");
                    return;
                }
                int check = 0, qty = 0;
                foreach (GridViewRow row in Grdbillings.Rows)
                {
                    CheckBox chkSelect = (CheckBox)row.FindControl("ChkBox");
                    if (chkSelect.Checked)
                    {
                        check = check + 1;
                        qty = qty + 1;
                    }

                }
                if (check == 0)
                {
                    sysFunc.UserMsg(lblMessage, Color.Red, "Please Check at least one JobCard...");
                    return;
                }
                int grd = Grdbillings.Rows.Count;
                if (grd > 0)
                {
                    //Auto Generated Code//
                    DateTime dt = DateTime.Now;
                    string strYear = dt.Year.ToString();
                    string BillNo;
                    if (ddlBillingNo.SelectedValue.Trim() == "0")
                    {
                        BillNo = ObjGenral.AutoGen("JENBillMaster", "JENBillNo", DateTime.Now.ToString("dd-MM-yyyy"));
                    }
                    else
                    {
                        BillNo = ddlBillingNo.SelectedValue.Trim();
                    }
                    //Auto Generated Code//
                    string Remarks = txtRemarks.Text.Trim().ToString();
                    float BillRecAmount = 0;
                    string DelFlag = "N", PostFlag = "N";
                    string UpdUser = Session["UserName"].ToString();

                    //FFI PDI Master Insert here
                    SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
                                   new SqlParameter("@FPBillNo",SqlDbType.Char,8),//1
                                   new SqlParameter("@BillDate",SqlDbType.DateTime),//2 
                                   new SqlParameter("@Remarks",SqlDbType.VarChar,100),//3 
                                   new SqlParameter("@BillQty",SqlDbType.Int),//4 
                                   new SqlParameter("@BillAmount",SqlDbType.Float,15),//5 
                                   new SqlParameter("@BillRecAmount",SqlDbType.Float,15),//6 
                                   new SqlParameter("@DelFlag",SqlDbType.Char,1),//7 
                                   new SqlParameter("@PostFlag",SqlDbType.Char,1),//8 
                                   new SqlParameter("@UpdUser",SqlDbType.VarChar,5),//9 
                                   new SqlParameter("@UpdTerm",SqlDbType.VarChar,50),//10 
                                   new SqlParameter("@TransferStatus",SqlDbType.Char,1),//11 
                                   new SqlParameter("@VoucherNo",SqlDbType.VarChar,50),//12 
                                   new SqlParameter("@VoucherFlag",SqlDbType.Char,1),//13 
                                   new SqlParameter("@SoftwareVersion",SqlDbType.VarChar,25),//14 
                                   new SqlParameter("@BillType",SqlDbType.VarChar,5),//15 
                                   new SqlParameter("@VehType",SqlDbType.Char,04),//16 
                                   new SqlParameter("@CreditNo",SqlDbType.VarChar,50),//17 
                                   new SqlParameter("@GSTAmount",SqlDbType.Decimal,50),//18 
                                   new SqlParameter("@SSTAmount",SqlDbType.Decimal,50),//19 
                                   new SqlParameter("@HandlingAmt",SqlDbType.Decimal,50),//20 
                                   new SqlParameter("@PartTotal",SqlDbType.Decimal,50),//21 
                                   new SqlParameter("@LaborTotal",SqlDbType.Decimal,50),//22 
                                 
                               };
                    param[0].Value = Session["DealerCode"].ToString();
                    param[1].Value = BillNo;
                    param[2].Value = ObjGenral.SaveDate(txtBillDate.Text);
                    //param[2].Value = txtBillDate.Text;
                    param[3].Value = Remarks.ToUpper();
                    param[4].Value = Grdbillings.Rows.Count;
                    param[5].Value = SysFunctions.CustomCDBL(txtTotalAmount.Text);
                    param[6].Value = SysFunctions.CustomCDBL(BillRecAmount);
                    param[7].Value = DelFlag;
                    param[8].Value = PostFlag;
                    param[9].Value = Session["UserName"].ToString();
                    //param[10].Value = ObjGenral.SaveDate(DateTime.Now.ToString());
                    //param[11].Value = ObjGenral.SaveTime(DateTime.Now.ToString());
                    param[10].Value = GlobalVar.mUserIPAddress;
                    param[11].Value = "C";
                    param[12].Value = null;
                    param[13].Value = null;
                    param[14].Value = null;
                    param[15].Value = null;
                    param[16].Value = ddlTypes.SelectedValue.ToString();
                    param[17].Value = txtCreditNo.Text;
                    param[18].Value = txtGSTAmt.Text.Trim();
                    param[19].Value = txtLabPST.Text.Trim();
                    param[20].Value = SysFunctions.CustomCDBL(txtHandlingAmt.Text); txtLabPST.Text.Trim();
                    param[21].Value =  SysFunctions.CustomCDBL(txtPartAmount.Text); txtLabPST.Text.Trim();
                    param[22].Value = SysFunctions.CustomCDBL(txtLaborAmount.Text); txtLabPST.Text.Trim();
                    if (ObjTrans.BeginTransaction(ref Trans) == true)
                    {
                        if (ObjGenral.ExecuteSP_NonQuery("sp_Service_JENBillMaster_Insert_ForJC", param, Trans))
                        {
                            lblLastBillNo.Text = "Last Code Generated : " + " " + BillNo;
                        }
                    }

                   
                    else
                    {
                        lblMessage.Text = "Error";
                    }

                    //FFI PDI Master Insert here
                    //Insert Detail Data Here
                    foreach (GridViewRow gvr in Grdbillings.Rows)
                    {
                        string DealerCode = Session["DealerCode"].ToString();
                        string PPRCode = gvr.Cells[1].Text.Trim().ToString();
                        string BillNoDetail = BillNo;
                        // string Billtype = ddlTypes.SelectedValue.ToString();
                        string BillAmount = gvr.Cells[5].Text.Trim().ToString();

                        CheckBox Chk = (CheckBox)gvr.Cells[0].FindControl("ChkBox");

                        SqlParameter[] param1 = {
                                   new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
                                   new SqlParameter("@JENBillNo",SqlDbType.Char,8),//1
                                   new SqlParameter("@PPRCode",SqlDbType.Char,8),//2
                                   new SqlParameter("@PPRDate",SqlDbType.DateTime),//3 
                                   new SqlParameter("@ChassisNo",SqlDbType.VarChar,30),//4 
                                   new SqlParameter("@Parts",SqlDbType.Float),//5
                                   new SqlParameter("@Sublet",SqlDbType.Float),//6 
                                   new SqlParameter("@Labor",SqlDbType.Float),//7
                                   new SqlParameter("@TotalAmount",SqlDbType.Float),//8
                                   new SqlParameter("@GSTAmount",SqlDbType.Float),//9
                                   new SqlParameter("@PSTLaborAmount",SqlDbType.Float),//10
                                   new SqlParameter("@PSTSublet",SqlDbType.Float),//11
                                   new SqlParameter("@HandlingAmount",SqlDbType.Float),//12
                                   };

                        param1[0].Value = Session["DealerCode"].ToString();
                       param1[1].Value = BillNoDetail;
                        param1[2].Value = PPRCode;
                        param1[3].Value = sysFunc.SaveDate(gvr.Cells[2].Text.Trim().ToString());
                        param1[4].Value = gvr.Cells[4].Text.Trim().ToString();
                        param1[5].Value =  SysFunctions.CustomCDBL(gvr.Cells[12].Text);
                        param1[6].Value =  SysFunctions.CustomCDBL(gvr.Cells[13].Text);
                        param1[7].Value =  SysFunctions.CustomCDBL(gvr.Cells[14].Text);
                        param1[8].Value =  SysFunctions.CustomCDBL(gvr.Cells[15].Text);
                        param1[9].Value =  SysFunctions.CustomCDBL(gvr.Cells[7].Text);
                        param1[10].Value = SysFunctions.CustomCDBL(gvr.Cells[9].Text);
                        param1[11].Value = SysFunctions.CustomCDBL(gvr.Cells[11].Text);
                        param1[12].Value = SysFunctions.CustomCDBL(gvr.Cells[6].Text);
                        // param1[3].Value = Billtype;

                        if (Chk.Checked == true)
                        {
                            if (ObjGenral.ExecuteSP_NonQuery("sp_Service_JENBillDetail_Insert", param1, Trans))
                            {
                                lblMessage.Text = "Record Has been Inserted Sucessfully !!!";
                             

                            }
                           // ObjGenral.ExecuteSP_NonQuery("sp_Service_JENBillDetail_Insert", param1,Trans);
                        }
                       
                        //Insert Detail Data Here
                    }
                }
                else
                {
                    string script = "alert(\"User should select some Bills !!!\");";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", script, true);
                }

            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                lblMessage.Text = ex.Message;

            }
            ObjTrans.CommittTransaction(ref Trans);
            ClearAll();
        }

        //protected void btnPost_Click(object sender, EventArgs e)
        //{
        //    if (ObjGenral.IsPosted("FFIPDIBillMaster", "FPBillNo", txtBillingNo.Text.Trim(), string.Empty))
        //    {
        //        ObjGenral.UserMsg(lblMessage, Color.Red, "Already Posted cannot deleted !!!", null);
        //        return;
        //    }
        //    SqlParameter[] param = {
        //                           new SqlParameter("@FPBillNo",SqlDbType. Char,8),//0
        //                           new SqlParameter("@DealerCode", SqlDbType. Char,5)
        //                       };

        //    param[0].Value = txtBillingNo.Text.Trim().ToString();
        //    param[1].Value = Session["DealerCode"].ToString();
        //    if (ObjGenral.ExecuteSP_NonQuery("sp_2W_Service_FFIPDI_UpdatePostFlag", param))
        //    {
        //        ObjGenral.UserMsg(lblMessage, Color.Green, txtBillingNo.Text + " is posted successflly", null);
        //    }
        //    else
        //    {
        //        ObjGenral.UserMsg(lblMessage, Color.Red, txtBillingNo.Text + " is not posted try again or contact to Pak suzuki help desk", null);
        //    }
        //}
        protected void btnDelete_Click(object sender, EventArgs e)

        {
            if (!sec.UserRight("2583", "004"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (ddlBillingNo.SelectedValue == "")
            {
                ObjGenral.UserMsg(lblMessage, Color.Red, "Please select any record to delete", null);
                return;
            }
            else
            {

                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
                                   new SqlParameter("@FPBillNo",SqlDbType. Char,8),//0
                               };

                if (ObjGenral.IsPosted("JENBillMaster", "JENBillNo", ddlBillingNo.SelectedValue, string.Empty))
                {
                    ObjGenral.UserMsg(lblMessage, Color.Red, "Already Posted cannot deleted !!!", null);
                    return;
                }
                else
                {
                    //Akram
                    param[0].Value = Session["DealerCode"].ToString();
                    param[1].Value = ddlBillingNo.SelectedValue.Trim().ToString();
                    ObjGenral.ExecuteSP_NonQuery("sp_2W_Service_JENBill_Delete", param);
                    ObjGenral.UserMsg(lblMessage, Color.Red, "Record Has been deleted Sucessfully !!!", null);
                    ClearAll();
                    return;

                }
            }

        }
        protected void txtBillingNo_TextChanged(object sender, EventArgs e)
        {
            RetrieveMasterData();
            RetrieveDetailData();
        }


        private void RetrieveMasterData()
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
                                   new SqlParameter("@FPBillNo",SqlDbType. Char,8),//1
                               };

            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = txtBillingNo.Text;

            //ObjMainBll.ExecuteSP_NonQuery("sp_2W_Service_GetMasterData", param);
            ds = ObjGenral.FillDataSet("sp_2W_JenBill_GetMasterData", param);
            if (ds.Tables[0].Rows[0]["BillDate"].ToString() == "")
            {
                txtRemarks.Text = "";
            }
            else
            {
                txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
            }
            //txtCode.Text = ds.Tables[0].Rows[0]["SSTNo"].ToString();
            ddlTypes.SelectedValue = ds.Tables[0].Rows[0]["VehicleCategory"].ToString().Trim();
            txtBillDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["BillDate"].ToString()).ToString("dd/MM/yyyy");
            txtPSTCode.Text = ds.Tables[0].Rows[0]["SSTNo"].ToString();
            txtGSTCode.Text = ds.Tables[0].Rows[0]["GSTNo"].ToString();
            txtLabPST.Text = ds.Tables[0].Rows[0]["SSTAmount"].ToString();
            txtGSTAmt.Text = ds.Tables[0].Rows[0]["GSTAmount"].ToString();
            txtPartAmount.Text = ds.Tables[0].Rows[0]["PartTotal"].ToString();
            txtHandlingAmt.Text = ds.Tables[0].Rows[0]["HandlingAmt"].ToString();
            txtLaborAmount.Text = ds.Tables[0].Rows[0]["LaborTotal"].ToString();
            txtTotalAmount.Text = ds.Tables[0].Rows[0]["BillAmount"].ToString();
            //txtPerc.Text = ds.Tables[0].Rows[0]["SSTAge"].ToString();
            //txtPstAmt.Text = ds.Tables[0].Rows[0]["SSTAmount"].ToString();
            lblLastBillNo.Text = "";

        }

        private void RetrieveDetailData()
        {
           

            DataTable dt3 = (DataTable)Session["BillingDetails"];


            dt3.Rows.RemoveAt(0);
            SqlParameter[] param1 = {
                                            new SqlParameter("@DealerCode",SqlDbType. Char,5),
                                            new SqlParameter("@JENBillNo",SqlDbType. Char,8),
                                       };

            param1[0].Value = Session["DealerCode"].ToString();
            param1[1].Value = txtBillingNo.Text.Trim();
            dsJENDEtail = ObjGenral.FillDataSet("sp_2W_Service_JENDetail_JC", param1);

            if (dsJENDEtail.Tables[0].Rows.Count > 0)
            {
              

                Grdbillings.DataSource = dsJENDEtail.Tables[0];
                Grdbillings.DataBind();
            }
                foreach (GridViewRow gvr in Grdbillings.Rows)
            {
                CheckBox Chk = (CheckBox)gvr.Cells[0].FindControl("ChkBox");
                Chk.Checked = true;
                //ChkBox.Checked = true;
            }
        }

        private void ClearAll()
        {
            ddlBillingNo.SelectedIndex = 0;
            txtRemarks.Text = "";
            txtBillDate.Text = "";
            LoadEmptyGrid();
            txtTotalAmount.Text = "";
            //ChkBox.Checked = false;
            lblMessage.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
            //txtPstAmt.Text = "";
            //txtPerc.Text = "";
            //txtCode.Text = "";
          //  ddlTypes.SelectedIndex = 0;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }
        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool chkFlag = false;
            CheckBox chkSelectAll = (CheckBox)Grdbillings.HeaderRow.FindControl("chkSelectAll");
            if (chkSelectAll.Checked)
                chkFlag = true;

            foreach (GridViewRow dr in Grdbillings.Rows)
            {
                CheckBox chk = (CheckBox)dr.Cells[0].FindControl("ChkBox");
                chk.Checked = chkFlag;
            }


            CheckBox Chk = sender as CheckBox;
            TableCell tc = Chk.Parent as TableCell;
            GridViewRow gvr = tc.Parent as GridViewRow;
            CheckBox ChkBox = (CheckBox)gvr.FindControl("ChkBox");
            txtGSTAmt.Text = CalculateGST().ToString();
            txtLabPST.Text = CalculatePST().ToString();
            txtPartAmount.Text = CalculatePart().ToString();
            CalculateTotal();
        }
        protected void ChkBox_CheckedChanged1(object sender, EventArgs e)
        {

            CheckBox Chk = sender as CheckBox;
            TableCell tc = Chk.Parent as TableCell;
            GridViewRow gvr = tc.Parent as GridViewRow;
            CheckBox ChkBox = (CheckBox)gvr.FindControl("ChkBox");
            txtGSTAmt.Text = CalculateGST().ToString();
            txtLabPST.Text = CalculatePST().ToString();
            txtPartAmount.Text = CalculatePart().ToString();
            CalculateTotal();

        }

        protected void btnBillReport_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2583", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }
            Data.DSReports data = new Data.DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
        {
            new SqlParameter("@DealerCode",SqlDbType.Char),//0
            new SqlParameter("@BillingNo",SqlDbType.VarChar)//1

        };

            param[0].Value = Session["DealerCode"].ToString();

            if (txtBillingNo.Text == "")
            {
                lblMessage.Text = "Please Select Billing No. first";
                return;
            }
            else
                param[1].Value = txtBillingNo.Text;

            SqlDataReader rder = null;



            if (sysFunc.ExecuteSP("Sp_JENBillingRpt_JC", param, ref rder))
            {
                data.EnforceConstraints = false;
                data.Sp_JENBillingRpt.Load(rder);

            }
            RD.Load(Server.MapPath("~/Modules/Service/ServiceReports/rptClaimMemoJC.rpt"));


            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Warranty Claim Memo'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";

            RD.Database.Tables[0].SetDataSource(data);

            // convert and show
            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "BillingReport" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;
            Session["RD"] = RD;
            //RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            string URL;
            URL = "../../../../Download/rptViewerService.aspx?FileName=" + FileName;
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

        }

        protected void ImageButton1_Click1(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewState["lookupid"] = 99;
                ViewState["ixd1"] = 1;
                ViewState["ixd2"] = 1;
                //Uzair
                ObjLookup.LU_Get_JenBills(ImageButton1, ViewState["lookupid"].ToString(), " And DelFlag='N' ", "../../../");
                //LbErr.Text = "";

            }
            catch (Exception ex)
            {
                // LbErr.Text = ex.Message;
            }
        }
        private void GetLookUpsValue(ref TextBox txt1, ref TextBox txt2, string ixd1, string ixd2)
        {
            if (Session["LookUpData"] != null)
            {
                GridViewRow Drow = (GridViewRow)Session["LookUpData"];
                txt1.Text = (txt1 != null ? Drow.Cells[Convert.ToInt32(ixd1)].Text.ToString() : "");
                txt2.Text = (txt2 != null ? Drow.Cells[Convert.ToInt32(ixd2)].Text.ToString() : "");
                Session["LookUpData"] = null;
            }
        }
        private void lookUptext(string lookUpID)
        {
            txtBillingNo.Text = lookUpID;
            RetrieveMasterData();
            RetrieveDetailData();
        }

        protected void ddlType_IndexChanged(object sender, EventArgs e)
        {

            if (txtToDate.Text == "" || txtFromDate.Text == "")
            {
                lblMessage.Text = "Please Select Dates";
                return;
            }
            string sql = "";
            DataTable dt ;
            LoadEmptyGrid();

            lblMessage.Text = string.Empty;
            SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
                                   new SqlParameter("@Type",SqlDbType.Char,3),//0
                                   new SqlParameter("@FromDate",SqlDbType.DateTime),//0
                                   new SqlParameter("@ToDate",SqlDbType.DateTime),//0
                               };
            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = ddlTypes.SelectedValue;
            param[2].Value = sysFunc.SaveDate(txtFromDate.Text);
            param[3].Value = sysFunc.SaveDate(txtToDate.Text);

            // ds = ObjGenral.FillDataSet("sp_2W_Service_GetClaimBilling_JC", param);
            sql = "sp_2W_Service_GetClaimBilling_JC  '" + param[0].Value + "','" + param[1].Value + "','" + param[2].Value + "','" + param[3].Value + "'";
            dt = ObjGenral.GetData(sql);
            if (dt.Rows.Count > 0)
            {
                Grdbillings.DataSource = dt;
                Grdbillings.DataBind();
                Session["JENDetail"] = dt;

                if (dt.Rows.Count > 0)
                {
                    foreach (GridViewRow gvr in Grdbillings.Rows)
                    {
                        PST = TotalAmount + SysFunctions.CustomCDBL(dt.Rows[0]["HandlingAmount"].ToString()) +  SysFunctions.CustomCDBL(dt.Rows[0]["PSTSubletAmount"].ToString())+ SysFunctions.CustomCDBL(dt.Rows[0]["PSTLaborAmount"].ToString());
                        GST = TotalAmount  + SysFunctions.CustomCDBL(dt.Rows[0]["GSTAmount"].ToString());
                        TotalAmount = TotalAmount + SysFunctions.CustomCDBL(dt.Rows[0]["TotalAmount"].ToString());
                        CheckBox Chk = (CheckBox)gvr.Cells[0].FindControl("ChkBox");
                        Chk.Checked = true;
                        //ChkBox.Checked = true;
                    }
                    txtLabPST.Text = CalculatePST().ToString();
                    txtGSTAmt.Text = CalculateGST().ToString();
                    //txtTotalAmount.Text = TotalAmount.ToString();
                    CalculateTotal();
                    //CalculatePSTPercentage();
                }
            }
            else
            {
                //txtPstAmt.Text = "";
                //txtPerc.Text = "";
                lblMessage.Text = "No Record Found !!!";
            }
        }

        protected void ddlBillingNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            string sql = "Select Format(BillDate,'dd-MM-yyyy')BillDate,Remarks,BillAmount,GSTNo,GSTAmount,SSTNo,SSTAmount,PostFlag,CreditMemoNo from JENBillMaster where DealerCode='" + Session["DealerCode"].ToString() + "' And JENBillNo = '" + ddlBillingNo.SelectedValue + "' and DelFlag='N'";
            dt = sysFunc.GetData(sql);

            txtBillDate.Text = dt.Rows[0]["BillDate"].ToString();
            txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
            txtTotalAmount.Text = dt.Rows[0]["BillAmount"].ToString();
            txtGSTAmt.Text = dt.Rows[0]["GSTAmount"].ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : dt.Rows[0]["GSTAmount"].ToString().Replace("&nbsp;", "").Trim();
               
            txtLabPST.Text = dt.Rows[0]["SSTAmount"].ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : dt.Rows[0]["SSTAmount"].ToString().Replace("&nbsp;", "").Trim();
           
            txtGSTCode.Text = dt.Rows[0]["GSTNo"].ToString();
            txtPSTCode.Text = dt.Rows[0]["SSTNo"].ToString();
            txtCreditNo.Text = dt.Rows[0]["CreditMemoNo"].ToString();
            if (dt.Rows[0]["PostFlag"].ToString() == "Y")
            {
                btnSave.Enabled = false;
                btnPost.Enabled = false;
                btnDelete.Enabled = false;


            }
            //  ddlTypes.SelectedValue = dt.Rows[0]["BillType"].ToString();
            DataTable dt2 = new DataTable();

            string sql2 = "Select * from JENBillDetail where DealerCode='" + Session["DealerCode"].ToString() + "' And JENBillNo = '" + ddlBillingNo.SelectedValue + "'";
            dt2 = sysFunc.GetData(sql2);

            DataTable dt3 = (DataTable)Session["BillingDetails"];

           
               dt3.Rows.RemoveAt(0);
            SqlParameter[] param1 = {
                                            new SqlParameter("@DealerCode",SqlDbType. Char,5),
                                            new SqlParameter("@JENBillNo",SqlDbType. Char,8),
                                       };

            param1[0].Value = Session["DealerCode"].ToString();
            param1[1].Value = ddlBillingNo.SelectedValue;
            dsJENDEtail = ObjGenral.FillDataSet("sp_2W_Service_JENDetail", param1);

                if (dsJENDEtail.Tables[0].Rows.Count > 0)
                {
                ddlTypes.SelectedValue= dsJENDEtail.Tables[0].Rows[0]["VehicleCategory"].ToString();

                    Grdbillings.DataSource = dsJENDEtail.Tables[0];
                    Grdbillings.DataBind();
            
                
                //for (int j = 0; j < dt2.Rows.Count; j++)
                //{
                //    DataRow row = dt3.NewRow();

                //    row["JobCardCode"] = dt2.Rows[j]["JobCardCode"].ToString();
                //    row["ChassisNo"] = dt2.Rows[j]["ChassisNo"].ToString();
                //    row["JobTypeDesc"] = dt2.Rows[j]["JobTypeCode"].ToString();
                //    row["PaidC"] = dt2.Rows[j]["BillAmount"].ToString();

                //    dt3.Rows.Add(row);
                //}
            }
            else
            {
                lblMessage.Text = "No Record Found";
                LoadEmptyGrid();
                return;
            }

           // Grdbillings.DataSource = dt3;
           // Grdbillings.DataBind();

            Session["BillingDetails"] = dsJENDEtail.Tables[0];
        }

        protected void Grdbillings_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Alternate || e.Row.RowState == DataControlRowState.Normal))
            {
              //  Part = Part + SysFunctions.CustomCDBL(e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim());
               // GST = GST + SysFunctions.CustomCDBL(e.Row.Cells[7].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[7].Text.Replace("&nbsp;", "").Trim());


            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                Part = GST = 0;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
               // txtPartAmount.Text = Part.ToString();
                //txtGSTAmt.Text = GST.ToString();
            }
        }

        protected void txtHandlingAmt_TextChanged(object sender, EventArgs e)
        {
            txtLabPST.Text = CalculatePST().ToString();
            CalculateTotal();
        }

        protected void txtLaborAmount_TextChanged(object sender, EventArgs e)
        {
            txtLabPST.Text = CalculatePST().ToString();
            CalculateTotal();
        }


        public double CalculatePST()
        {
            PST = 0;
          string pst = ObjGenral.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "PST", "Dealer", Session["DealerCode"].ToString());
          return  PST = (SysFunctions.CustomCDBL(txtLaborAmount.Text)+SysFunctions.CustomCDBL(txtHandlingAmt.Text)) * (SysFunctions.CustomCDBL(pst) / 100);
            
        }
        public double CalculatePart()
        {
            Part = 0;
            foreach (GridViewRow gvr in Grdbillings.Rows)
            {
                CheckBox Chk = (CheckBox)gvr.Cells[0].FindControl("ChkBox");

                if (Chk.Checked)
                {
                    Part = Part + SysFunctions.CustomCDBL(gvr.Cells[5].Text);
                   
                }

            }
            txtPartAmount.Text = Part.ToString();
            return Part = SysFunctions.CustomCDBL(Part);

        }
        public double  CalculateGST()
        {
            CalculatePart();
            GST = 0;
            foreach (GridViewRow gvr in Grdbillings.Rows)
            {
                CheckBox Chk = (CheckBox)gvr.Cells[0].FindControl("ChkBox");

                if(Chk.Checked)
                {
                    GST = GST + SysFunctions.CustomCDBL(gvr.Cells[7].Text);
                  
                }
                
            }
          
            return GST =  SysFunctions.CustomCDBL(GST);

        }
    
        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2583", "005"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (txtLabPST.Text == "")
            {
                string script = "alert(\"PST Amount should not left blank !!!\");";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", script, true);
            }
            if (txtGSTAmt.Text == "")
            {
                string script = "alert(\"GST Amount should not left blank !!!\");";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", script, true);
            }
            if (PST > 0 || GST > 0)
            {
                double PST = SysFunctions.CustomCDBL(txtLabPST.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : txtLabPST.Text.Replace("&nbsp;", "").Trim());
                double GST = SysFunctions.CustomCDBL(txtGSTAmt.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : txtGSTAmt.Text.Replace("&nbsp;", "").Trim());
                string SSTNo,GSTNo;
               
                    if (PST > 0)
                    {
                        SSTNo = ObjGenral.AutoGen("PSTUniqueSalesTaxInvoice", "SSTSalesTaxInvNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                    }
                    else
                    {
                        SSTNo = "";
                    }
                    if (GST > 0)
                    {
                        GSTNo = ObjGenral.AutoGen("UniqueSalesTaxInvoice", "SalesTaxInvNO", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                    }
                    else
                    {
                        GSTNo = "";
                    }

                    SqlParameter[] param = {
                                           new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
                                           new SqlParameter("@FPBillNo",SqlDbType. Char,8),//1
                                           new SqlParameter("@SSTNo",SqlDbType.VarChar,8),//0
                                           new SqlParameter("@SSTDate",SqlDbType.DateTime),//1
                                             new SqlParameter("@SSTAmount",SqlDbType.Decimal),//1
                                           new SqlParameter("@GSTNo",SqlDbType.VarChar,8),//0
                                           new SqlParameter("@GSTDate",SqlDbType.DateTime),//1
                                            new SqlParameter("@GSTAmount",SqlDbType.Decimal),//1
                                       };

                    param[0].Value = Session["DealerCode"].ToString();
                    param[1].Value = ddlBillingNo.SelectedValue;
                    param[2].Value = SSTNo;
                    param[3].Value = DateTime.Now.ToString();
                    param[4].Value = Convert.ToDecimal(txtLabPST.Text);
                    param[5].Value = GSTNo;
                    param[6].Value = DateTime.Now.ToString();
                    param[7].Value = Convert.ToDecimal(txtGSTAmt.Text);

                    ObjGenral.ExecuteSP_NonQuery("sp_2W_Service_UpdateSSTNoDate_JENBillMaster", param);
                    lblPstCode.Text = "Pst Code " + SSTNo.ToString() + "  " + "has been generated sucessfully !!";
                    lblGstCode.Text = "Gst Code " + GSTNo.ToString() + "  " + "has been generated sucessfully !!";
                    ClearAll();

                }
                else
                {
                lblPstCode.Text = "PST and GST  amount Must be greater than zero cannot be generated !!!";
                }
            

        }
        
        public void CalculateTotal()
        {
            CalculatePST();
            TotalAmount = SysFunctions.CustomCDBL(txtLaborAmount.Text)+
                          SysFunctions.CustomCDBL(txtLabPST.Text)+
                          SysFunctions.CustomCDBL(txtHandlingAmt.Text)+
                          SysFunctions.CustomCDBL(txtPartAmount.Text)+
                          SysFunctions.CustomCDBL(txtGSTAmt.Text);
            txtTotalAmount.Text = TotalAmount.ToString();
        }


    }
}