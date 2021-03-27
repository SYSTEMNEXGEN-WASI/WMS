using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;

namespace DXBMS.Modules.Service.Forms
{
    public partial class FreeService : System.Web.UI.Page
    {
        DataTable dttemp = new DataTable();
        MainBLL ObjMainBll = new MainBLL();
        SysFunctions ObjGenral = new SysFunctions();
        SecurityBll sec = new SecurityBll();
        SysFunction sysFunc = new SysFunction();
        clsLookUp ObjLookup = new clsLookUp();
        DataTable dttable = new DataTable();
        float totalamount, totalGST, totalPST;
        DataSet ds;
        decimal TotalAmount = 0;
        string FormState = string.Empty;
        float SumOfPart = 0;
        float SumOfLabor = 0;
        double SumOfPST = 0;
        double SumOfGST = 0;
        double SumOf = 0, SumOfSubT=0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2535", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }

            if (!IsPostBack)
            {
                txtBillDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                LoadEmptyGrid();
                LoadddlType();
                LoadddlBillingNo();
                Session["LookUpData"] = string.Empty;
            }
            if (IsPostBack)
            {
                //if (Session["LookUpData"] != null)
                //{
                //    int intlookupid = Convert.ToInt32(ViewState["lookupid"]);
                //    lookUptext();


                //    //if (intlookupid == 57)
                //    //{
                //    //    ddlBillingNo.SelectedValue = Session["LookUpData"].ToString();
                //    //    ddlBillingNo_SelectedIndexChanged(null, null);
                //    //    // RetrieveMasterData();
                //    //    // RetrieveDetailData();
                //    //}
                //}
                if (!string.IsNullOrEmpty(Session["LookUpData"].ToString()))
                {
                    lookUptext(Session["LookUpData"].ToString());
                }
            }
           
            Session["LookUpData"] ="";
        }
        
        private void LoadddlBillingNo()
        {
            string WhereClause = "DealerCode = '" + Session["DealerCode"]+ "'";

            string[] Columns = new string[] { "FPBillNo","BillDate","Remarks" };
            sysFunc.GetMultiColumnsDDL(ddlBillingNo, Columns, "FFIPDIBillMaster ", WhereClause, "FPBillNo", "", false, false);
        
        }

        private void LoadddlType()
        {   ddlTypes.Items.Add(new ListItem("Select","Select"));
            ddlTypes.Items.Add(new ListItem("PDI", "002"));
            ddlTypes.Items.Add(new ListItem("FFS", "001"));
            ddlTypes.Items.Add(new ListItem("SFS", "014"));
            ddlTypes.Items.Add(new ListItem("TFS", "013"));

        }

        private void LoadEmptyGrid()
        {
            try
            {
                dttable.Columns.Add("JobCardCode");
                dttable.Columns.Add("RecDate");
                dttable.Columns.Add("ChassisNo");
                dttable.Columns.Add("JobTypeDesc");
                dttable.Columns.Add("PartsTotal");
                dttable.Columns.Add("GST");
                dttable.Columns.Add("GSTAmount");
                dttable.Columns.Add("Fur");
                dttable.Columns.Add("FurAmount");
                dttable.Columns.Add("LaborTotal");
                dttable.Columns.Add("PST");
                dttable.Columns.Add("PSTAmount");
                dttable.Columns.Add("PaidC");
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
                dr["JobCardCode"] = "";
                dr["RecDate"] = "";
                dr["ChassisNo"] = "";
                dr["JobTypeDesc"] = "";
                dr["PartsTotal"] = "";
                dr["GST"] = "";
                dr["GSTAmount"] = "";
                dr["Fur"] = "";
                dr["FurAmount"] = "";
                dr["LaborTotal"] = "";
                dr["PST"] = "";
                dr["PSTAmount"] = "";
                dr["PaidC"] = "";
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

        private double CalculatePSTPercentage()
        {
            foreach (GridViewRow row in Grdbillings.Rows)
            {
                CheckBox ChkBox = (CheckBox)row.FindControl("ChkBox");
                Label lblPSTAmt = (Label)row.FindControl("lblPSTAmt");
                if (ChkBox.Checked)
                {
                    SumOfPST = SumOfPST + SysFunctions.CustomCDBL(lblPSTAmt.Text);
                }
                
            }
            return SumOfPST;


        }
        private double CalculateTotal()
        {
            SumOf = 0;
            foreach (GridViewRow row in Grdbillings.Rows)
            {
                CheckBox ChkBox = (CheckBox)row.FindControl("ChkBox");
                Label lblLabor = (Label)row.FindControl("lblLabor");
                Label lblPSTAmt = (Label)row.FindControl("lblPSTAmt");
                Label lblGSTAmt = (Label)row.FindControl("lblGSTAmt");
                Label lblFurAmt = (Label)row.FindControl("lblFurAmt");
                Label lblPart = (Label)row.FindControl("lblPart");
                if (ChkBox.Checked)
                {
                    SumOf = SumOf + SysFunctions.CustomCDBL(lblPSTAmt.Text)
                        + SysFunctions.CustomCDBL(lblLabor.Text)
                                                + SysFunctions.CustomCDBL(lblPart.Text)
                                                                        + SysFunctions.CustomCDBL(lblGSTAmt.Text)
                                                                         + SysFunctions.CustomCDBL(lblFurAmt.Text)




                        ;
                }

            }
            return SumOf;


        }
        private double CalculateGSTPercentage()
        {
            foreach (GridViewRow row in Grdbillings.Rows)
            {
                CheckBox ChkBox = (CheckBox)row.FindControl("ChkBox");
                Label lblGSTAmt = (Label)row.FindControl("lblGSTAmt");
                Label lblFurAmt = (Label)row.FindControl("lblFurAmt");
              
                if (ChkBox.Checked)
                {
                    SumOfGST = SumOfGST + SysFunctions.CustomCDBL(lblGSTAmt.Text) + SysFunctions.CustomCDBL(lblFurAmt.Text);
                }

            }
            return SumOfPST;


        }
        private double CalculateSubTotal()
        {
            foreach (GridViewRow row in Grdbillings.Rows)
            {
                CheckBox ChkBox = (CheckBox)row.FindControl("ChkBox");
                Label lblLabor = (Label)row.FindControl("lblLabor");
                Label lblPart = (Label)row.FindControl("lblPart");

                if (ChkBox.Checked)
                {
                    SumOfSubT = SumOfSubT + SysFunctions.CustomCDBL(lblLabor.Text) + SysFunctions.CustomCDBL(lblPart.Text);
                }

            }
            return SumOfSubT;


        }

        protected void ChkBoxHeader_CheckedChanged(object sender, EventArgs e)
        {

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


                // if (chk.Checked) txtSelectedTotalRefAmount.Text = (Convert.ToDecimal(txtSelectedTotalRefAmount.Text.Trim() == "" ? "0" : txtSelectedTotalRefAmount.Text.Trim()) + Convert.ToDecimal(dr.Cells[5].Text)).ToString();
                //  else txtSelectedTotalRefAmount.Text = "0.00";
                // txtInstAmt.Text = txtSelectedTotalRefAmount.Text;

            }
         
   txtSubTotal.Text= CalculateSubTotal().ToString();
            txtTotalAmount.Text = CalculateTotal().ToString();
            txtLabPST.Text = CalculatePSTPercentage().ToString();
            txtGSTAmt.Text = CalculateGSTPercentage().ToString();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2535", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (ddlBillingNo.SelectedIndex != 0)
            {
                if (!sec.UserRight("2535", "003"))
                {
                    Response.Redirect("~/Test.aspx");
                }
            }
            if (ddlTypes.SelectedValue == "Select")
            {
                sysFunc.UserMsg(lblMessage, Color.Red, "Please Select the Type...");
                return;
            }
            int check=0,qty=0;
            foreach (GridViewRow row in Grdbillings.Rows)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("ChkBox");
                if (chkSelect.Checked)
                {
                    check=check+1;
                    qty = qty + 1;
                }

            }
            if(check==0)
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
                if (ddlBillingNo.SelectedValue.Trim() =="0"||ddlBillingNo.SelectedIndex==0)
                {
                    BillNo = ObjGenral.AutoGen("FFIPDIBillDetail", "FPBillNo", strYear);
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
                                   //new SqlParameter("@UpdDate",SqlDbType.DateTime),//10 
                                   //new SqlParameter("@UpdTime",SqlDbType.DateTime),//11 
                                   new SqlParameter("@UpdTerm",SqlDbType.VarChar,50),//12 
                                   new SqlParameter("@TransferStatus",SqlDbType.Char,1),//13 
                                   new SqlParameter("@VoucherNo",SqlDbType.VarChar,50),//14 
                                   new SqlParameter("@VoucherFlag",SqlDbType.Char,1),//15 
                                   new SqlParameter("@SoftwareVersion",SqlDbType.VarChar,25),//16 
                                   new SqlParameter("@BillType",SqlDbType.VarChar,5),//17 
                                   new SqlParameter("@SubTotal",SqlDbType.Float),//18 
                                   //new SqlParameter("@SSTAmount",SqlDbType.Decimal,18),//19 
                                   
                               };
                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = BillNo;
                param[2].Value = ObjGenral.SaveDate(txtBillDate.Text);
                //param[2].Value = txtBillDate.Text;
                param[3].Value = Remarks.ToUpper();
                param[4].Value = qty;
                param[5].Value = txtTotalAmount.Text.Trim().ToString();
                param[6].Value = BillRecAmount;
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
                param[15].Value = ddlTypes.SelectedValue.ToString();
                param[16].Value = txtSubTotal.Text.Trim();
                //param[17].Value = txtPstAmt.Text.Trim();

                if (ObjGenral.ExecuteSP_NonQuery("sp_Service_FFIPDIBillMaster_Insert", param))
                {
                    lblLastBillNo.Text = "Last Code Generated : " + " " + BillNo;
                   
                }
                else
                {
                    lblMessage.Text = "Error";
                }
                //FFI PDI Master Insert here
                //Insert Detail Data Here
                foreach (GridViewRow gvr in Grdbillings.Rows)

                {
                    Label lblPart = (Label)gvr.Cells[5].FindControl("lblPart");
                    Label lblLabor = (Label)gvr.Cells[10].FindControl("lblLabor");
                    Label lblGSTAmt = (Label)gvr.Cells[7].FindControl("lblGSTAmt");
                    Label lblPSTAmt = (Label)gvr.Cells[12].FindControl("lblPSTAmt");
                    Label lblFurAmt = (Label)gvr.Cells[9].FindControl("lblFurAmt");
                    Label lblGrossAmt = (Label)gvr.Cells[13].FindControl("lblGrossAmt");
                    string DealerCode = Session["DealerCode"].ToString();
                    string JobCardCode = gvr.Cells[1].Text.Trim().ToString();
                    string BillNoDetail = BillNo;
                    string Billtype = ddlTypes.SelectedValue.ToString();
                    float BillAmount = float.Parse(lblGrossAmt.Text.Trim().ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : lblGrossAmt.Text.Trim().ToString().Replace("&nbsp;", "").Trim());

                    CheckBox Chk = (CheckBox)gvr.Cells[0].FindControl("ChkBox");

                    SqlParameter[] param1 = {
                                   new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
                                   new SqlParameter("@FPBillNo",SqlDbType.Char,8),//1
                                   new SqlParameter("@JobCardCode",SqlDbType.Char,8),//2 
                                   new SqlParameter("@JobTypeCode",SqlDbType.Char,3),//3 
                                   new SqlParameter("@BillAmount",SqlDbType.Float,15),//4 
                                   new SqlParameter("@ChassisNo",SqlDbType.VarChar,30),//5
                                   new SqlParameter("@GST",SqlDbType.Float),//6
                                   new SqlParameter("@PST",SqlDbType.Float),//7
                                   new SqlParameter("@PartsTotal",SqlDbType.Float),//8
                                   new SqlParameter("@LaborTotal",SqlDbType.Float),//9
                                   new SqlParameter("@GSTAmount",SqlDbType.Float),//10
                                   new SqlParameter("@PSTAmount",SqlDbType.Float),//11
                                   new SqlParameter("@Fur",SqlDbType.Float),//12
                                   new SqlParameter("@FurAmount",SqlDbType.Float),//13
                                   };
                
                    param1[0].Value = Session["DealerCode"].ToString();
                    param1[1].Value = BillNoDetail;
                    param1[2].Value = JobCardCode;
                    param1[3].Value = Billtype;
                    param1[4].Value = BillAmount;
                    param1[5].Value = gvr.Cells[4].Text.Trim().ToString();
                    param1[6].Value = SysFunctions.CustomCDBL(gvr.Cells[6].Text.Trim().ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : gvr.Cells[6].Text.Trim().ToString().Replace("&nbsp;", "").Trim());
                    param1[7].Value = SysFunctions.CustomCDBL(gvr.Cells[11].Text.Trim().ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : gvr.Cells[11].Text.Trim().ToString().Replace("&nbsp;", "").Trim());
                    param1[8].Value = SysFunctions.CustomCDBL(lblPart.Text.ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : lblPart.Text.ToString().Replace("&nbsp;", "").Trim());
                    param1[9].Value = SysFunctions.CustomCDBL(lblLabor.Text.ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : lblLabor.Text.ToString().Replace("&nbsp;", "").Trim());

                    param1[10].Value = SysFunctions.CustomCDBL(lblGSTAmt.Text.ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : lblGSTAmt.Text.ToString().Replace("&nbsp;", "").Trim());
                    param1[11].Value = SysFunctions.CustomCDBL(lblPSTAmt.Text.ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : lblPSTAmt.Text.ToString().Replace("&nbsp;", "").Trim());
                    param1[12].Value = SysFunctions.CustomCDBL(gvr.Cells[8].Text.Trim().ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : gvr.Cells[8].Text.Trim().ToString().Replace("&nbsp;", "").Trim());
                    param1[13].Value = SysFunctions.CustomCDBL(lblFurAmt.Text.ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : lblFurAmt.Text.ToString().Replace("&nbsp;", "").Trim());
                    if (Chk.Checked == true)
                    {
                        
                        ObjGenral.ExecuteSP_NonQuery("sp_Service_FFIPDIBillDetail_Insert", param1);
                    }
                    lblMessage.Text = "Record Has been Inserted Sucessfully !!!";
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Record Saved,Updated Successfully: " + BillNo + "')", true);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);
                }
            }
            else
            {
                string script = "alert(\"User should select some Bills !!!\");";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", script, true);
            }
        }
  
        private void CalcTotals()
        {
            decimal SumOfPart = 0;
            decimal SumOfLabor = 0;
            decimal SumOfGSTAmt = 0;
            decimal SumOfFurTaxAmt = 0;
            decimal SumOfGrossAmt = 0;
            foreach (GridViewRow gvr in Grdbillings.Rows)
            {
                CheckBox chkSelect = (CheckBox)gvr.Cells[0].FindControl("chkSelect");
                Label lblPart = (Label)gvr.Cells[5].FindControl("lblPart");
                Label lblLabor = (Label)gvr.Cells[10].FindControl("lblLabor");
                Label lblGSTAmt = (Label)gvr.Cells[7].FindControl("lblGSTAmt");
                Label lblPSTAmt = (Label)gvr.Cells[12].FindControl("lblPSTAmt");
                Label lblFurAmt = (Label)gvr.Cells[9].FindControl("lblFurAmt");
                Label lblGrossAmt = (Label)gvr.Cells[13].FindControl("lblGrossAmt");
                SumOfPart = SumOfPart + Convert.ToDecimal(lblPart.Text);
                SumOfLabor = SumOfLabor + Convert.ToDecimal(lblLabor.Text);
                SumOfGSTAmt = SumOfGSTAmt + Convert.ToDecimal(lblGSTAmt.Text); ;
                SumOfFurTaxAmt = SumOfFurTaxAmt + Convert.ToDecimal(lblFurAmt.Text);
                SumOfGrossAmt = SumOfGrossAmt + Convert.ToDecimal(lblGrossAmt.Text);


            }

        }
        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2535", "005"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (txtGSTAmt.Text == "")
            {
                string script = "alert(\"GST Amount should not left blank !!!\");";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", script, true);
            }
            if (txtLabPST.Text == "")
            {
                string script = "alert(\"PST Amount should not left blank !!!\");";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", script, true);
            }
          
            else
            {
                double PST = double.Parse(txtLabPST.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : txtLabPST.Text.Replace("&nbsp;", "").Trim());
                double GST = double.Parse(txtGSTAmt.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : txtGSTAmt.Text.Replace("&nbsp;", "").Trim());
                string SSTNo, GSTNo;

                if (ObjGenral.CodeExists("PSTUniqueSalesTaxInvoice", "SSTSalesTaxInvNO", txtPSTCode.Text, ""))
                {
                    lblMessage.Text = "Already Code exists against this Dealer ";
                    if (ObjGenral.CodeExists("UniqueSalesTaxInvoice", "SalesTaxInvNO", txtGSTCode.Text, ""))
                    {
                        lblMessage.Text = "Already Code exists against this Dealer ";

                    }
                }
                else
                {
                     
                    if (PST > 0 || GST > 0)
                    {
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
                                           new SqlParameter("@SSTAmount",SqlDbType.Float),//1
                                           new SqlParameter("@GSTNo",SqlDbType.VarChar,8),//0
                                           new SqlParameter("@GSTDate",SqlDbType.DateTime),//1
                                           new SqlParameter("@GSTAmount",SqlDbType.Float),//1
                                       };

                        param[0].Value = Session["DealerCode"].ToString();
                        param[1].Value = ddlBillingNo.SelectedValue;
                        param[2].Value = SSTNo;
                        param[3].Value = DateTime.Now.ToString();
                        param[4].Value = SysFunctions.CustomCDBL(txtLabPST.Text);
                        param[5].Value = GSTNo;
                        param[6].Value = DateTime.Now.ToString();
                        param[7].Value = SysFunctions.CustomCDBL(txtGSTAmt.Text);

                        ObjGenral.ExecuteSP_NonQuery("sp_2W_Service_UpdateSSTNoDate_FFIBillMaster", param);
                        if (PST > 0)
                        {
                            lblPstCode.Text = "Pst Code " + SSTNo.ToString() + "  " + "has been generated sucessfully !!";
                        }
                        if (GST > 0)
                        {
                            lblGstCode.Text = "Gst Code " + GSTNo.ToString() + "  " + "has been generated sucessfully !!";
                        }
                       
                       
                        ClearAll();
                    }
                    else if (PST==0 && GST==0)
                    {
                        string sql = "Update FFIPDIBillMaster Set PostFlag='Y' where DealerCode='"+Session["DealerCode"].ToString()+ "' and FPBillNo='"+ddlBillingNo.SelectedValue+"'  ";
                        sysFunc.GetData(sql);
                        lblPstCode.Text = ""+ddlBillingNo.SelectedValue+" is Posted Sucessfully !!!";
                    }
                    else
                    {
                        lblPstCode.Text = "PST and GST  amount Must be greater than zero cannot be generated !!!";
                    }
                
                }
            }

        }




        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2535", "004"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (ddlBillingNo.SelectedValue == "")
            {
                ObjGenral.UserMsg(lblMessage, Color.Red, "Please select any record to delete", null);
                return;
            }
            if (txtBillingNo.Text == "")
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

                if (ObjGenral.IsPosted("FFIPDIBillMaster", "FPBillNo", ddlBillingNo.SelectedValue, string.Empty))
                {
                    ObjGenral.UserMsg(lblMessage, Color.Red, "Already Posted cannot deleted !!!", null);
                    return;
                }
                else
                {
                    //Akram
                    param[0].Value = Session["DealerCode"].ToString();
                    param[1].Value = ddlBillingNo.SelectedValue.Trim().ToString();
                    ObjGenral.ExecuteSP_NonQuery("sp_2W_Service_FFIPDIBill_Delete", param);
                    ObjGenral.UserMsg(lblMessage, Color.Red, "Record Has been deleted Sucessfully !!!", null);
                    ClearAll();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Deletealert()", true);
                    return;

                }
            }

        }
        protected void txtBillingNo_TextChanged(object sender, EventArgs e)
        {
            //FormState = "Edit";
            //RetrieveMasterData();
            //RetrieveDetailData();
            ddlBillingNo.SelectedValue = Session["LookUpData"].ToString();
            ddlBillingNo_SelectedIndexChanged(null, null);
        }


        private void RetrieveMasterData()
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
                                   new SqlParameter("@FPBillNo",SqlDbType. Char,8),//1
                               };

            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = ddlBillingNo.SelectedValue.Trim().ToString();

            //ObjMainBll.ExecuteSP_NonQuery("sp_2W_Service_GetMasterData", param);
            ds = ObjGenral.FillDataSet("sp_2W_Service_GetMasterData", param);
            if (ds.Tables[0].Rows[0]["BillDate"].ToString() == "")
            {
                txtRemarks.Text = "";
            }
            else
            {
                txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
            }
            //txtCode.Text = ds.Tables[0].Rows[0]["SSTNo"].ToString();
            ddlTypes.SelectedValue = ds.Tables[0].Rows[0]["BillType"].ToString();
            txtBillDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["BillDate"].ToString()).ToString("dd/MM/yyyy");
          //  txtTotalAmount.Text = ds.Tables[0].Rows[0]["BillAmount"].ToString();
            //txtPerc.Text = ds.Tables[0].Rows[0]["SSTAge"].ToString();
            //txtPstAmt.Text = ds.Tables[0].Rows[0]["SSTAmount"].ToString();
            lblLastBillNo.Text = "";

        }

        private void RetrieveDetailData()
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
                                   new SqlParameter("@FPBillNo",SqlDbType. Char,8),//1
                               };

            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = ddlBillingNo.SelectedValue.Trim().ToString();

            //ObjMainBll.ExecuteSP_NonQuery("sp_2W_Service_GetMasterData", param);
            ds = ObjGenral.FillDataSet("sp_2W_Service_GetDetailData", param);
            Grdbillings.DataSource = ds;
            Grdbillings.DataBind();

            foreach (GridViewRow gvr in Grdbillings.Rows)
            {
                CheckBox Chk = (CheckBox)gvr.Cells[0].FindControl("ChkBox");
               Chk.Checked = true;
                //Chk.Visible = false;
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
            ddlTypes.SelectedIndex = 0;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearAll();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);
        }
        protected void ChkBox_CheckedChanged1(object sender, EventArgs e)
        {

            CheckBox Chk = sender as CheckBox;
            TableCell tc = Chk.Parent as TableCell;
            GridViewRow gvr = tc.Parent as GridViewRow;
            CheckBox ChkBox = (CheckBox)gvr.FindControl("ChkBox");
            Label lblPart = (Label)gvr.Cells[5].FindControl("lblPart");
            Label lblLabor = (Label)gvr.Cells[10].FindControl("lblLabor");
            Label lblGSTAmt = (Label)gvr.Cells[7].FindControl("lblGSTAmt");
            Label lblPSTAmt = (Label)gvr.Cells[12].FindControl("lblPSTAmt");
            Label lblFurAmt = (Label)gvr.Cells[9].FindControl("lblFurAmt");
            Label lblGrossAmt = (Label)gvr.Cells[13].FindControl("lblGrossAmt");
      
            //Button Btn = (Button)gvr.FindControl("BtnGenerateCode");
            double CountBillAmount = 0;
            double PST = 0;
            double GST = 0;
           GST = CalculateGSTPercentage();
                PST = CalculatePSTPercentage();
                CountBillAmount = CalculateTotal();




            txtSubTotal.Text = CalculateSubTotal().ToString();
            txtLabPST.Text = PST.ToString();
            txtGSTAmt.Text = GST.ToString();
            txtTotalAmount.Text = CountBillAmount.ToString();
            CalculatePSTPercentage();

        }

        protected void btnBillReport_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2535", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }

            Data.DSReports data = new Data.DSReports();
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
        {
            new SqlParameter("@DealerCode",SqlDbType.Char),//0
            new SqlParameter("@FPBillNo",SqlDbType.Char)//1

        };

            param[0].Value = Session["DealerCode"].ToString();

            if (ddlBillingNo.SelectedIndex == 0)
            {
                lblMessage.Text = "Please Select Billing No. first";
                return;
            }
            else
                param[1].Value = ddlBillingNo.SelectedValue.ToString().Trim();

            SqlDataReader rder = null;


            ds = SqlHelper.ExecuteDataset(CConn.CConnection.GetConnectionString(), CommandType.Text, "Sp_BillingRpt'" + this.Session["DealerCode"].ToString() + "','" + ddlBillingNo.SelectedValue.ToString().Trim() + "'");
            data.Sp_BillingRpt.Load(ds.CreateDataReader());
            //if (sysFunc.ExecuteSP("Sp_BillingRpt", param, ref rder))
            //{
            //    //data.EnforceConstraints = false;
            //    data.Sp_BillingRpt.Load(rder);

            //}
             RD.Load(Server.MapPath("~/Modules/Service/ServiceReports/FreeServiceBill.rpt"));
             RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Free Services Billing'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString().ToUpper() + "'";
            RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString().ToUpper() + " '";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
          //  RD.SetDataSource(data);
           RD.Database.Tables[0].SetDataSource(data);

            // convert and show
            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "BillingReport" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;
            Session["RD"] = RD;
            //RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);
            RD.Dispose(); RD.Close();
            string URL;
            URL = "../../../../Download/rptViewerService.aspx?FileName=" + FileName;
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1500,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

        }

        protected void ImageButton1_Click1(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewState["lookupid"] = 57;
                ViewState["ixd1"] = 1;
                ViewState["ixd2"] = 1;
                //Uzair
                ObjLookup.LU_Get_FFIPDIBills(ImageButton1, ViewState["lookupid"].ToString(), " And DelFlag='N' ", "../../../");
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
            //Selection values display

            //switch (lookUpID)
            //{

            //    case 57:
            //        GetLookUpsValue(ref txtBillingNo, ref txtBillingNo, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString());
            //        break;

            //    default:
            //        break;
            //}
            btnClear_Click(null, null);
            LoadddlBillingNo();
            ddlBillingNo.SelectedValue = lookUpID; ;
            txtBillingNo.Text = Session["LookUpData"].ToString();
            ddlBillingNo_SelectedIndexChanged(null, null);
            ddlTypes.Enabled = false;
            btnSave.Enabled = false;

        }

        protected void ddlType_IndexChanged(object sender, EventArgs e)
        {

            if (txtToDate.Text == "" && txtFromDate.Text == "")
            {
                lblMessage.Text = "Please Select Dates";
                return;
            }

            LoadEmptyGrid();

            lblMessage.Text = string.Empty;
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
                                   new SqlParameter("@JobType",SqlDbType.VarChar),//0
                                   new SqlParameter("@FromDate",SqlDbType.DateTime),//0
                                   new SqlParameter("@ToDate",SqlDbType.DateTime),//0
                               };
                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlTypes.SelectedValue;
                param[2].Value =sysFunc.SaveDate(txtFromDate.Text);
                param[3].Value =sysFunc.SaveDate(txtToDate.Text);

                ds = ObjGenral.FillDataSet("sp_2W_Service_GetFFIBillingData_new", param);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Grdbillings.DataSource = ds;
                    Grdbillings.DataBind();

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (GridViewRow gvr in Grdbillings.Rows)
                        {
                            TotalAmount = TotalAmount + Convert.ToDecimal(ds.Tables[0].Rows[0]["PaidC"].ToString());
                            CheckBox Chk = (CheckBox)gvr.Cells[0].FindControl("ChkBox");
                          //  CheckBox ChkAll = (CheckBox)gvr.Cells[0].FindControl("chkSelectAll");
                            Chk.Checked = true;
                          //  ChkAll.Checked = true;
                         
                        }
                  
                     
                    }
                }
                else
                {
                  
                    lblMessage.Text = "No Record Found !!!";
                }
         
        }

        protected void ddlBillingNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            string sql = "Get_FFIPDIMaster'" + Session["DealerCode"].ToString() + "' ,'" + ddlBillingNo.SelectedValue + "'";
            dt = sysFunc.GetData(sql);

            txtBillDate.Text = dt.Rows[0]["BillDate"].ToString();
            txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
           // txtTotalAmount.Text = dt.Rows[0]["BillAmount"].ToString();
            Session["TotalAmount"] = dt.Rows[0]["BillAmount"].ToString();
            ddlTypes.SelectedValue = dt.Rows[0]["BillType"].ToString();
           Session["GSTAmount"] = dt.Rows[0]["GSTAmount"].ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : dt.Rows[0]["GSTAmount"].ToString().Replace("&nbsp;", "").Trim();
            txtSubTotal.Text= dt.Rows[0]["SubTotal"].ToString();
            Session["SSTAmount"] = dt.Rows[0]["SSTAmount"].ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : dt.Rows[0]["SSTAmount"].ToString().Replace("&nbsp;", "").Trim();

            txtGSTCode.Text = dt.Rows[0]["GSTNo"].ToString();
            txtPSTCode.Text = dt.Rows[0]["SSTNo"].ToString();
            if (dt.Rows[0]["PostFlag"].ToString() == "Y")
            {
                btnSave.Enabled = false;
                btnPost.Enabled = false;
                btnDelete.Enabled = false;


            }
            DataTable dt2 = new DataTable();

            string sql2 = "sp_FFIPDI_GetBillingData '" + Session["DealerCode"].ToString() + "' , '" + ddlBillingNo.SelectedValue + "'";
            dt2 = sysFunc.GetData(sql2);

            DataTable dt3 = (DataTable) Session["BillingDetails"];

            if (dt2.Rows.Count > 0)
            {
                dt3.Rows.RemoveAt(0);

                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    DataRow row = dt3.NewRow();

                    row["JobCardCode"] = dt2.Rows[j]["JobCardCode"].ToString();
                    row["RecDate"] = dt2.Rows[j]["RecDate"].ToString();
                    row["ChassisNo"] = dt2.Rows[j]["ChassisNo"].ToString();
                    row["JobTypeDesc"] = dt2.Rows[j]["JobTypeDesc"].ToString();
                    row["PartsTotal"] = dt2.Rows[j]["PartsTotal"].ToString();
                    row["GST"] = dt2.Rows[j]["GST"].ToString();
                    row["GSTAmount"] = dt2.Rows[j]["GSTAmount"].ToString();
                    row["Fur"] = dt2.Rows[j]["Fur"].ToString();
                    row["FurAmount"] = dt2.Rows[j]["FurAmount"].ToString();
                    row["LaborTotal"] = dt2.Rows[j]["LaborTotal"].ToString();
                    row["PST"] = dt2.Rows[j]["PST"].ToString();
                    row["PSTAmount"] = dt2.Rows[j]["PSTAmount"].ToString();
                    row["PaidC"] = dt2.Rows[j]["PaidC"].ToString();

                    dt3.Rows.Add(row);
                }
            }
            else
            {
                lblMessage.Text = "No Record Found";
                LoadEmptyGrid();
                return;
            }

            Grdbillings.DataSource = dt3;
            Grdbillings.DataBind();
            foreach (GridViewRow gvr in Grdbillings.Rows)
            {
                CheckBox Chk = (CheckBox)gvr.Cells[0].FindControl("ChkBox");
               // Chk.Checked = true;
               Chk.Visible = false;
            }
        
            Session["BillingDetails"] = dt3;
        }

        protected void btnGL_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2535", "005"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (txtBillingNo.Text == "")
            {
                ObjGenral.UserMsg(lblMessage, Color.Red, "Please Select Bill First !!!", null);
                return;
            }
            string Post=ObjGenral.GetStringValuesAgainstCodes("DealerCode='"+Session["DealerCode"].ToString()+"' And  FPBillNo='" + txtBillingNo.Text + "'", "FFIPDIBillMaster", "PostFlag");
            if (Post=="" || Post=="N")

            {
                ObjGenral.UserMsg(lblMessage, Color.Red, "Please  Post Bill First !!!", null);
                return;
            }
            
           

             string   URL = "JV.aspx?CusInv=" + txtBillingNo.Text + "&Type=FSB";
            
            
            string fullURL = "window.open('" + URL + "', '_blank', 'height=600,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }

        protected void Grdbillings_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Grdbillings_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //e.Row.Cells[2].Visible = false;
            //e.Row.Cells[5].Visible = 
            //e.Row.Cells[6].Visible = 
           // e.Row.Cells[11].Visible = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
               
                Label lblPart = (Label)e.Row.Cells[5].FindControl("lblPart");
                Label lblLabor = (Label)e.Row.Cells[10].FindControl("lblLabor");
                Label lblGSTAmt = (Label)e.Row.Cells[7].FindControl("lblGSTAmt");
                Label lblPSTAmt = (Label)e.Row.Cells[12].FindControl("lblPSTAmt");
                Label lblFurAmt = (Label)e.Row.Cells[9].FindControl("lblFurAmt");
                Label lblGrossAmt = (Label)e.Row.Cells[13].FindControl("lblGrossAmt");
                
                    SumOfPart = SumOfPart + float.Parse(lblPart.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblPart.Text.Replace("&nbsp;", "").Trim());
                    SumOfLabor = SumOfLabor + float.Parse(lblLabor.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblLabor.Text.Replace("&nbsp;", "").Trim());
                    totalamount = totalamount + float.Parse(lblGrossAmt.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblGrossAmt.Text.Replace("&nbsp;", "").Trim());
                    totalGST = totalGST + float.Parse(lblGSTAmt.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblGSTAmt.Text.Replace("&nbsp;", "").Trim());
                    totalPST = totalPST + float.Parse(lblPSTAmt.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblPSTAmt.Text.Replace("&nbsp;", "").Trim());
              
                }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                totalamount = 0;
                totalGST = totalPST =SumOfLabor=SumOfPart= 0;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                //if (txtQuantity.Text.Trim() == "")
                //{
                //    txtQuantity.Text = "0";
                //}
                ////if (txtPartTotalQuantity.Text.Trim () == "")
                ////{
                ////    txtPartTotalQuantity.Text = "0";
                ////}
                //txtPartTotalQuantity.Text = (Convert.ToInt32 (txtPartTotalQuantity.Text.Trim ()) + Convert.ToInt32 (txtQuantity.Text.Trim ())).ToString (); 
                Label lblPart = (Label)e.Row.Cells[5].FindControl("lblFootParts");
                Label lblLabor = (Label)e.Row.Cells[10].FindControl("lblFootLabor");
                Label lblGSTAmt = (Label)e.Row.Cells[7].FindControl("lblFootGSTAmt");
                Label lblPSTAmt = (Label)e.Row.Cells[12].FindControl("lblFootPSTAmt");
                Label lblFurAmt = (Label)e.Row.Cells[9].FindControl("lblFootFurAmt");
                Label lblGrossAmt = (Label)e.Row.Cells[13].FindControl("lblFootGrossAmt");
                lblPart.Text = SumOfPart.ToString();
                txtSubTotal.Text = (SumOfLabor+SumOfPart).ToString();
                lblGSTAmt.Text= totalGST.ToString();
                lblPSTAmt.Text = totalPST.ToString();
                lblGrossAmt.Text= totalamount.ToString();
                txtGSTAmt.Text = totalGST.ToString();
                txtLabPST.Text = totalPST.ToString();
                txtTotalAmount.Text = totalamount.ToString();
            }

        }
        //protected void btnGeoneratePST_Click(object sender, EventArgs e)
        //{
        //    if (txtPstAmt.Text == "")
        //    {
        //        string script = "alert(\"PST Amount should not left blank !!!\");";
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", script, true);
        //    }
        //    else
        //    {
        //        //double a = double.Parse(txtPstAmt.Text.Trim());
        //        string SSTNo;

        //        if (ObjGenral.CodeExists("PSTUniqueSalesTaxInvoice", "SSTSalesTaxInvNO", txtCode.Text, ""))
        //        {
        //            lblMessage.Text = "Already Code exists against this Dealer ";
        //        }
        //        else
        //        {
        //            if (a > 0)
        //            {
        //                SSTNo = ObjGenral.AutoGen("PSTUniqueSalesTaxInvoice", "SSTSalesTaxInvNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));

        //                SqlParameter[] param = {
        //                           new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
        //                           new SqlParameter("@FPBillNo",SqlDbType. Char,8),//1
        //                           new SqlParameter("@SSTNo",SqlDbType.VarChar,8),//0
        //                           new SqlParameter("@SSTDate",SqlDbType.DateTime),//1
        //                       };

        //                param[0].Value = Session["DealerCode"].ToString();
        //                param[1].Value = txtBillingNo.Text.Trim().ToString();
        //                param[2].Value = SSTNo;
        //                param[3].Value = DateTime.Now.ToString();

        //                ObjGenral.ExecuteSP_NonQuery("sp_2W_Service_UpdateSSTNoDate_FFiMaster", param);
        //                lblPstCode.Text = "Pst Code " + SSTNo.ToString() + "  " + "has been generated sucessfully !!";
        //                ClearAll();
        //            }
        //            else
        //            {
        //                lblPstCode.Text = "PST amount greater than zero cannot be generated !!!";
        //            }
        //        }
        //    }
        //}
     
          
            
           
        


    }
}