using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CConn;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Drawing;
using System.Text;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;

namespace DXBMS.Modules.Service.Forms
{
    public partial class Warranty : System.Web.UI.Page
    {
        SysFunction sysFunc = new SysFunction();
        SysFunctions sysFuncs = new SysFunctions();
        SecurityBll sec = new SecurityBll();
        Transaction ObjTrans = new Transaction();
        clsLookUp clslook = new clsLookUp();
        SqlTransaction Trans;
        DataTable dtParts,dtSublet;
        DataSet dsSublet, ds, dsParts,dsJobs, dsFIRImage;
        DataTable dtJobs,dt, ImageDT;
        string StrMCDMainDBCon = "";
        string leadId;
        static int btnValue = 0;
        decimal countPartsRecQty;
        double totParts, totlabor, totSublet;
        bool IsSaved;
        DropDownList ddlpartsource;
        private decimal Total = (decimal)0.0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2533", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (IsPostBack)
            {

                if (!string.IsNullOrEmpty(Session["LookUpData"].ToString()))
                {
                    SelectedPartDetail(Session["LookUpData"].ToString());
                }
            }
            if (!Page.IsPostBack)
            {

                Session["LookUpData"] = string.Empty;
                LoadEmptyGrid();
                LoadEmptyGridJobs();
                LoadEmptyGridSublet();
                Load_PPR();
                LoadDLLs();
            }
            dtParts = new DataTable();
            dsSublet = new DataSet();
            dtJobs = new DataTable();
            txtPPRDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Session["LookUpData"] ="";
            if (!IsPostBack)
            {
                if (Session["FIRCode"] != null)
                {
                    ViewState["FIRCode"] = leadId;
                    ddlPPRNo.SelectedValue = Session["FIRCode"].ToString();
                    ddlPPRNo_SelectedIndexChanged(null, null);

                }
                Session["FIRCode"] = null;
                Session["LookUpData"] = "";
            }



            //if (Session["LookUpData"] != null)
            //{
            //    //lookUptext(ViewState["lookupid"].ToString());
            //    if (ViewState["lookupid"].ToString() == "44") ViewJobCardData();
            //    //if (ViewState["lookupid"].ToString() == "48") ViewPartsData();
            //    if (ViewState["lookupid"].ToString() == "54") ViewData();
            //}
        }

        private void LoadDLLs()
        {
            ddlRoadCond.Items.Add(new ListItem("Select", "Select"));
            ddlRoadCond.Items.Add(new ListItem("Paved", "Paved"));
            ddlRoadCond.Items.Add(new ListItem("City", "City"));
            ddlRoadCond.Items.Add(new ListItem("Rural", "Rural"));

            //ddlAppSt.Items.Add(new ListItem("Select", "Select"));
            //ddlAppSt.Items.Add(new ListItem("Approved", "Approved"));
            //ddlAppSt.Items.Add(new ListItem("Reject", "Reject"));
            //ddlAppSt.Items.Add(new ListItem("Return", "Return"));

            string Where = "1=1";

            string[] Columns2 = new string[] { "DefectCode", "DefectDesc" };
            sysFunc.GetMultiColumnsDDL(ddlDefect, Columns2, "Defect"," DealerCode = 'COMON'", "DefectCode", "", false, false);

            string[] Columns1 = new string[] { "ComplainCode", "ComplainDesc" };
            sysFunc.GetMultiColumnsDDL(ddlComplains, Columns1, "ComplainType", "DealerCode='COMON'", "ComplainCode", "", false, false);

            string WhereQuery = "DealerCode = '" + Session["DealerCode"] + "'";

         
            string[] Columns = new string[] { "VendorCode", "VendorDesc" };
            sysFunc.GetMultiColumnsDDL(ddlVendor, Columns, "Vendor", WhereQuery, "VendorCode", "", false, false);

          
            string where = "DealerCode = '" + Session["DealerCode"].ToString() + "' and JobCardType = '008'";

            //string[] Columns3 = new string[] { "JobCardCode", "UserName", "RegNo" };
            //sysFunc.GetMultiColumnsDDL(ddljob_code, Columns3, "JobCardMaster", where, "JobCardCode", "", false, false);
        }

        public void Load_PPR() {
            string WhereQuery = "DealerCode = '" + Session["DealerCode"] + "'";
            string[] Columns0 = new string[] { "PPRCode", "Convert(varchar(10),PPRDate,105)", "UserName", "VehRegNo" };
            sysFunc.GetMultiColumnsDDL(ddlPPRNo, Columns0, "PPRMaster", WhereQuery, "PPRCode", "", false, false);

        }

        public void ViewData()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string status;
            string sql = "exec SP_Select_PPRMaster '" + Session["DealerCode"].ToString() + "' ,'" + ddlPPRNo.SelectedValue + "'";

            dt = sysFunc.GetData(sql);

            //ddljob_code.SelectedValue = dt.Rows[0]["JobCardCode"].ToString();
            txtJobCardDate.Text = dt.Rows[0]["PPRDate"].ToString(); //Convert.ToDateTime(sysFuncs.GetStringValuesAgainstCodes("JobCardCode", ddljob_code.SelectedValue, "RecDate", "JobCardMaster")).ToString("dd/MM/yyyy");
            txtPPRTotalAmount.Text = dt.Rows[0]["TotalAmount"].ToString();
            txtRegNo.Text = dt.Rows[0]["VehRegNo"].ToString();
            txtChasisNo.Text = dt.Rows[0]["VehChassisNo"].ToString();
            txtEngNo.Text = dt.Rows[0]["VehEngineNo"].ToString();
            txtVehModel.Text = dt.Rows[0]["VersionCode"].ToString(); 
            txtVersionDesc.Text= dt.Rows[0]["VersionDesc"].ToString();
            txtInvoiceNo.Text = dt.Rows[0]["InvoiceNo"].ToString();
            //txtInvDate.Text = Convert.ToDateTime(dt.Rows[0]["InvoiceDate"].ToString()).ToString("dd/MM/yyyy");
            txtInvDate.Text = dt.Rows[0]["InvoiceDate"].ToString();
            //txtDealerCode.Text = dt.Rows[0]["SaleDealerCode"].ToString();
            //txtDealerName.Text = dt.Rows[0]["SaleDealerName"].ToString();
            txtProdCode.Text = dt.Rows[0]["ProdCode"].ToString();
            txtProdDesc.Text= dt.Rows[0]["ProdDesc"].ToString();
            txtVehColor.Text = dt.Rows[0]["Color"].ToString();
            txtKM.Text = dt.Rows[0]["MeterReadingKM"].ToString();
            dtComplitionDate.Text = dt.Rows[0]["CompletionDate"].ToString(); //Convert.ToDateTime(Convert.ToDateTime(dt.Rows[0]["CompletionDate"].ToString()).ToString("dd/MM/yyyy").Substring(6, 4) + "/" + Convert.ToDateTime(dt.Rows[0]["CompletionDate"].ToString()).ToString("dd/MM/yyyy").Substring(3, 2) + "/" + Convert.ToDateTime(dt.Rows[0]["CompletionDate"].ToString()).ToString("dd/MM/yyyy").Substring(0, 2));   //Convert.ToDateTime(Convert.ToDateTime(gvr.Cells[4].Text).ToString("yyyy-MM-dd"));
            dtProblemDate.Text = sysFunc.GetDate(dt.Rows[0]["DateOfProblem"]);//Convert.ToDateTime(Convert.ToDateTime(dt.Rows[0]["DateOfProblem"].ToString()).ToString("dd/MM/yyyy").Substring(6, 4) + "/" + Convert.ToDateTime(dt.Rows[0]["DateOfProblem"].ToString()).ToString("dd/MM/yyyy").Substring(3, 2) + "/" + Convert.ToDateTime(dt.Rows[0]["DateOfProblem"].ToString()).ToString("dd/MM/yyyy").Substring(0, 2));   //Convert.ToDateTime(Convert.ToDateTime(gvr.Cells[4].Text).ToString("yyyy-MM-dd"));
            txtCustCode.Text = dt.Rows[0]["CusCode"].ToString();
            txtCustName.Text = dt.Rows[0]["UserName"].ToString();
            txtUserName.Text = dt.Rows[0]["UserName"].ToString().Trim();
            txtUserAddress.Text = dt.Rows[0]["UserAddress1"].ToString();
            txtDelvDate.Text = dt.Rows[0]["DelvDate"].ToString();
            txtPhoneNo.Text = dt.Rows[0]["UserPhoneNo"].ToString();
            ddlRoadCond.SelectedValue = dt.Rows[0]["RoadCondition"].ToString().Trim();
            txtJobCardNo.Text = dt.Rows[0]["JobCardCode"].ToString().Trim();
            txtDTRNo.Text = dt.Rows[0]["DTRNo"].ToString().Trim();
            txtVehicleCategory.Text = dt.Rows[0]["VehicleCategory"].ToString();
            if (dt.Rows[0]["PostFlag"].ToString() == "Y")
            {
                btnSave.Enabled = false;
                btnPost.Enabled = false;
                btnDelete.Enabled = false;
            }
            status = sysFuncs.GetStringValuesAgainstCodes("PPRCode", ddlPPRNo.SelectedValue.Trim(), "ApprovalStatus", "PPRMaster", Session["DealerCode"].ToString());
            if (dt.Rows[0]["PostFlag"].ToString() == "Y")
            {
                if (dt.Rows[0]["ApprovalFlag"].ToString() == "Y")
                {
                    if (status == "Approved")
                    {
                        btnSave.Enabled = false;
                        btnPost.Enabled = false;

                    }
                }
            }

           

            //txtAprovalNo.Text = dt.Rows[0]["ApprovalNo"].ToString();
            //txtComplainRemarks.Text = dt.Rows[0]["ComplainRemarks"].ToString();
            //txtTRDate.Text = dt.Rows[0]["TRDate"].ToString();
            //txtTRNo.Text = dt.Rows[0]["TRNo"].ToString();
            //if (dt.Rows[0]["ApprovalStatus"].ToString() == "")
            //{
            //    ddlAppSt.SelectedIndex = 0;
            //}
            //else
            //{
            //    ddlAppSt.SelectedValue = dt.Rows[0]["ApprovalStatus"].ToString().Trim();
            //}



            DataSet dsPPRDetail = new DataSet();
            SqlParameter[] param = {
                                            new SqlParameter("@DealerCode",SqlDbType. Char,5),
                                            new SqlParameter("@PPRCode",SqlDbType. Char,8),
                                       };
            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = ddlPPRNo.SelectedValue;


            dsPPRDetail = sysFuncs.FillDataSet("sp_2W_Service_Select_PPRDetail", param);

            ddlComplains.SelectedValue = dsPPRDetail.Tables[0].Rows[0]["ComplainCode"].ToString();
            txtCompletionDesc.Text = dsPPRDetail.Tables[0].Rows[0]["ComplaintDesc"].ToString();
         //   ddlDefect.SelectedValue = dsPPRDetail.Tables[0].Rows[0]["DefectCode"].ToString();
            txtLaborDesc.Text = dsPPRDetail.Tables[0].Rows[0]["LabourDesc"].ToString();
            txtLaborAmount.Text = dsPPRDetail.Tables[0].Rows[0]["LabourRate"].ToString();
            txtRootCauseFailure.Text = dsPPRDetail.Tables[0].Rows[0]["RootCauseFailure"].ToString();
            txtCorrectiveAction.Text = dsPPRDetail.Tables[0].Rows[0]["CorrectiveAction"].ToString();


            dsParts = new DataSet();
            SqlParameter[] param1 = {
                                            new SqlParameter("@DealerCode",SqlDbType. Char,5),
                                            new SqlParameter("@PPRCode",SqlDbType. Char,8),
                                       };
            param1[0].Value = Session["DealerCode"].ToString();
            param1[1].Value = ddlPPRNo.SelectedValue;

            sql = "SELECT VendorCode FROM PPRPartsDetail WHERE PPRCode = '" + ddlPPRNo.SelectedValue + "' AND DealerCode = '" + Session["DealerCode"].ToString() + "'";

            dt = sysFunc.GetData(sql);

            if (dt.Rows.Count > 0) {
                ddlVendor.SelectedValue = dt.Rows[0]["VendorCode"].ToString().Trim();
            }
            dsParts = sysFuncs.FillDataSet("sp_2W_Service_Select_PPRPartsDetail", param1);

            if (dsParts.Tables[0].Rows.Count > 0)
            {
                LoadGrid();
            }
      
            dsJobs = sysFuncs.FillDataSet("sp_2W_Service_Select_PPRWarrantyJobs", param1);
            if (dsJobs.Tables[0].Rows.Count > 0)
            {
                LoadGridJobs();
            }
            dsSublet = sysFuncs.FillDataSet("sp_2W_Service_Select_PPRWarrantySubletJobs", param1);
            if (dsSublet.Tables[0].Rows.Count > 0)
            {
                LoadSubletJobs();
            }
            

        }
        private void ViewJobCardData()
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = {
                                            new SqlParameter("@DealerCode",SqlDbType. Char,5),
                                            new SqlParameter("@JobCardCode",SqlDbType. Char,8),
                                       };
            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = txtJobCardNo.Text;
            ds = sysFuncs.FillDataSet("sp_2W_Service_Select_JobCardMaster", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtRegNo.Text = ds.Tables[0].Rows[0]["RegNo"].ToString();
                txtChasisNo.Text = ds.Tables[0].Rows[0]["ChassisNo"].ToString();
                txtEngNo.Text = ds.Tables[0].Rows[0]["EngineNo"].ToString();
                txtVehModel.Text = ds.Tables[0].Rows[0]["InvoiceDate"].ToString().Substring(6, 4);
                txtVehColor.Text = ds.Tables[0].Rows[0]["ColorCode"].ToString();
                txtInvDate.Text = ds.Tables[0].Rows[0]["InvoiceDate"].ToString();
                txtCustCode.Text = ds.Tables[0].Rows[0]["CusCode"].ToString();
                txtProdCode.Text = ds.Tables[0].Rows[0]["ProdCode"].ToString();
                txtInvoiceNo.Text = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();
                ds = new DataSet();
                //Uzair
                if (sysFuncs.CodeExists("Customer", "CusCode", txtCustCode.Text, ""))
                {
                    txtCustName.Text = ds.Tables[0].Rows[0]["CusDesc"].ToString();
                }


            }
        }

        private void LoadEmptyGridSublet()
        {
            try
            {
                ds = new DataSet();
                ds.Tables.Add("WarrantySubletLaborTable");
                ds.Tables["WarrantySubletLaborTable"].Columns.Add(new DataColumn("JobCode", typeof(string)));
                ds.Tables["WarrantySubletLaborTable"].Columns.Add(new DataColumn("JobDesc", typeof(string)));
                ds.Tables["WarrantySubletLaborTable"].Columns.Add(new DataColumn("SubletDate", typeof(string)));
                ds.Tables["WarrantySubletLaborTable"].Columns.Add(new DataColumn("SubletAmount", typeof(string)));
                ds.Tables["WarrantySubletLaborTable"].Columns.Add(new DataColumn("PST", typeof(string)));
                ds.Tables["WarrantySubletLaborTable"].Columns.Add(new DataColumn("PSTAmount", typeof(string)));
                //dtJobs.Columns.Add("Sno");
                //dtJobs.Columns.Add("LaborCode");
                //dtJobs.Columns.Add("LaborDesc");
                //dtJobs.Columns.Add("Rate");
                //ViewState["WarrantyLaborTable"] = dtJobs;
                grd_Sublet.DataSource = ds.Tables["WarrantySubletLaborTable"];
                grd_Sublet.DataBind();
                dtJobs = ds.Tables["WarrantySubletLaborTable"];
                Session["WarrantySubletLaborTable"] = dtJobs;
                Session["WarrantySubletLaborTable"] = ds;
            }
            catch (Exception ex)
            {

            }
        }
        private void LoadEmptyGridJobs()
        {
            try
            {
                ds = new DataSet();
                ds.Tables.Add("WarrantyLaborTable");
                ds.Tables["WarrantyLaborTable"].Columns.Add(new DataColumn("LaborCode", typeof(string)));
                ds.Tables["WarrantyLaborTable"].Columns.Add(new DataColumn("LaborDesc", typeof(string)));
                ds.Tables["WarrantyLaborTable"].Columns.Add(new DataColumn("Hr", typeof(string)));
                ds.Tables["WarrantyLaborTable"].Columns.Add(new DataColumn("Rate", typeof(string)));
                ds.Tables["WarrantyLaborTable"].Columns.Add(new DataColumn("TotalLabor", typeof(string)));
                ds.Tables["WarrantyLaborTable"].Columns.Add(new DataColumn("PSTPer", typeof(string)));
                ds.Tables["WarrantyLaborTable"].Columns.Add(new DataColumn("PSTAmount", typeof(string)));
                //dtJobs.Columns.Add("Sno");
                //dtJobs.Columns.Add("LaborCode");
                //dtJobs.Columns.Add("LaborDesc");
                //dtJobs.Columns.Add("Rate");
                //ViewState["WarrantyLaborTable"] = dtJobs;
                gvWLabor.DataSource = ds.Tables["WarrantyLaborTable"];
                gvWLabor.DataBind();
                dtJobs = ds.Tables["WarrantyLaborTable"];
                Session["WarrantyLaborTables"] = dtJobs;
                Session["WarrantyLaborTable"] = ds;
            }
            catch (Exception ex)
            {

            }
        }
        private void LoadEmptyGrid()
        {

            ds = new DataSet();

            ds.Tables.Add();

            ds.Tables[0].Columns.Add(new DataColumn("PartNo", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("ItemCode", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("ItemDesc", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("PartSourceCode", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Qty", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Price", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("GrossAmount", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("GSTPer", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("GSTAmount", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("HandlingPer", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("HandlingAmount", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("trackInvNo", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("ConsNo", typeof(string)));
            DataRow dr = ds.Tables[0].NewRow();
            ds.Tables[0].Rows.Add(dr);

            grdParts.DataSource = ds.Tables[0];
            grdParts.DataBind();

            Session["PartsDetailsDS"] = ds;
        }
        private void LoadGrid()
        {
            //dtParts.DefaultView.Sort = "S.No";
            grdParts.DataSource = dsParts;
            grdParts.DataBind();
            dt = dsParts.Tables[0];
            Session["PartsDetailsDS"] = dsParts.Tables[0];
            ViewState["PartsDataTable"] = dsParts;
            // Total = Convert.ToDecimal(dt.Compute("Sum(GrossAmount)", string.Empty)) + Convert.ToDecimal(dt.Compute("Sum(HandlingAmount)", string.Empty))+ Convert.ToDecimal(dt.Compute("Sum(GSTAmount)", string.Empty));
         //   txtPPRTotalAmount.Text = Total.ToString();
            //txtPartsTotal.Text = Total.ToString();
        }
        private void LoadGridJobs()
        {
            decimal total;

            gvWLabor.DataSource = dsJobs;
            gvWLabor.DataBind();
            dtJobs = dsJobs.Tables[0];
            Session["WarrantyLaborTables"] = dtJobs;
            //if (dtJobs.Rows[0]["TOtalLabor"].ToString() == null || dtJobs.Rows[0]["TOtalLabor"].ToString() == "" || dtJobs.Rows[0]["TOtalLabor"].ToString() == "0")
            //{
            //    total = 0;
            //}
            //else
            //{
            //    total = Convert.ToDecimal(dtJobs.Compute("Sum(TOtalLabor)", string.Empty))+ Convert.ToDecimal(dtJobs.Compute("Sum(PSTAmount)", string.Empty));
            //    txtTotalLab.Text = total.ToString();
            //}

            //Total = Total + total;
            //txtPPRTotalAmount.Text = Total.ToString();
           
        }
        private void LoadSubletJobs()
        {
            if (dsSublet == null)
            {
                return;
            }
            decimal total;
            grd_Sublet.DataSource = dsSublet;
            grd_Sublet.DataBind();
            dtSublet = dsSublet.Tables[0];
            Session["WarrantySubletLaborTable"] = dtSublet;
            ViewState["WarrantySubletLaborTable"] = dsSublet;
            if (dtSublet.Rows[0]["SubletAmount"].ToString() == null || dtSublet.Rows[0]["SubletAmount"].ToString() == "" || dtSublet.Rows[0]["SubletAmount"].ToString() == "0")
            {
                total = 0;
            }
            else
            {
                total = Convert.ToDecimal(dtSublet.Compute("Sum(SubletAmount)", string.Empty)) + Convert.ToDecimal(dtSublet.Compute("Sum(PSTAmount)", string.Empty));
                txtTotalSublet.Text = total.ToString();
            }

            Total = Total + total;
          //  txtPPRTotalAmount.Text = Total.ToString();
            txtTotalSublet.Text = Total.ToString();
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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2533", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (ddlPPRNo.SelectedIndex != 0)
            {
                if (!sec.UserRight("2533", "003"))
                {
                    Response.Redirect("~/Test.aspx");
                }
            }
          
            SqlTransaction Trans = null;
            bool Chek = false;
            try
            {
                //TextBox[] textBoxes = { txtJobCardNo, txtCompletionDesc };
                //if (!MasterValidation(textBoxes))
                //{
                //	return;
                //}
                if (txtJobCardNo.Text.Trim() == "")
                {
                    sysFuncs.UserMsg(lblMsg, Color.Red, "Jobcard No Can not be left blank ", null);
                    return;
                }
                if (txtCompletionDesc.Text.Trim() == "")
                {
                    sysFuncs.UserMsg(lblMsg, Color.Red, "Complain Description Can not be left blank ", null);
                    return;
                }
                if (sysFuncs.IsPosted("PPRMaster", "PPRCode", ddlPPRNo.SelectedValue, string.Empty))
                {
                    sysFuncs.UserMsg(lblMsg, Color.Red, "Can not edit PPR already posted", null);
                    return;
                }
                //if (ddlPPRNo.SelectedValue != "__/_____" && ddlPPRNo.SelectedValue != "")
                //{
                //	if (sysFuncs.IsExist("PPRCode", ddlPPRNo.SelectedValue, "PPRMaster", string.Empty) == false)
                //	{
                //                 lblMsg.Text = "Record dose not exist";
                //		//sysFuncs.UserMsg(lblMsg, Color.Red, "Record dose not exist", ddlPPRNo);
                //		return;
                //	}
                //}
                if (dtProblemDate.Text.Trim() == string.Empty)
                {
                    sysFuncs.UserMsg(lblMsg, Color.Red, "Problem date should not be left blank.", null);
                    dtProblemDate.Focus();
                    return;
                }
                if (dtComplitionDate.Text.Trim() == string.Empty)
                {
                    sysFuncs.UserMsg(lblMsg, Color.Red, "Completion date should not be left blank.", null);
                    dtComplitionDate.Focus();
                    return;
                }
                if (ddlComplains.SelectedIndex == 0)
                {
                    sysFuncs.UserMsg(lblMsg, Color.Red, "Complain Code should not be left blank.");
                    return;
                }
                if (ddlDefect.SelectedIndex == 0)
                {
                    sysFuncs.UserMsg(lblMsg, Color.Red, "Defect Code should not be left blank.");
                    return;
                }
                if (gvWLabor.Rows.Count == 0)
                {
                    sysFuncs.UserMsg(lblMsg, Color.Red, "Labor should not be left blank.");
                    return;
                }

                if (ObjTrans.BeginTransaction(ref Trans) == false)
                {
                    return;
                }
                if (InsertPPRMaster(Trans))
                {
                    Chek = true;
                    if (InsertPPRDetail(Trans))
                    {
                        Chek = true;
                    }
                    else
                    {
                        Chek = false;
                    }
                    if (Chek == true)
                    {
                        // dtParts = (DataTable)ViewState["PartsDataTable"];
                        
                        dtParts = (DataTable)Session["PartsDetailsDS"];

                        foreach (DataRow dtRow in dtParts.Rows)
                        {
                            if (InsertPPRPartsDetail(dtRow, Trans))
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
                    if (Chek == true)
                    {
                        //dtParts = (DataTable)ViewState["PartsDataTable"];
                       
                        dtJobs = (DataTable)Session["WarrantyLaborTables"];

                        foreach (DataRow dtRow in dtJobs.Rows)
                        {
                            if (InsertPPRWarrantyLabor(dtRow, Trans))
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
                    if (Chek == true)
                    {
                        //dtParts = (DataTable)ViewState["PartsDataTable"];

                        dtSublet = (DataTable)Session["WarrantySubletLaborTable"];

                        foreach (DataRow dtRow in dtSublet.Rows)
                        {
                            if (InsertPPRSubletLabor(dtRow, Trans))
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
                }
                else
                {
                    ObjTrans.RollBackTransaction(ref Trans);
                }
                if (Chek == true)
                {
                    ObjTrans.CommittTransaction(ref Trans);
                    lblMsg.Visible = true;
                    //sysFuncs.UserMsg(lblMsg, Color.Green, "Saved Successfully Last Code '" + ddlPPRNo.SelectedValue + "'");
                    ClearAll();
                    Load_PPR();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                //SendAlert("Error : " + ex.Message);
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }
        }
        private bool InsertPPRMaster(SqlTransaction Trans)
        {
            SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",SqlDbType. Char, 5),//0
									new SqlParameter("@PPRCode",SqlDbType.Char,8),//1
									new SqlParameter("@JobCardCode",SqlDbType.Char,8),//2
									new SqlParameter("@PPRDate",SqlDbType.DateTime),//3
									new SqlParameter("@CusCode",SqlDbType.Char,8),//4
									new SqlParameter("@UserName",SqlDbType.Char,75),//5
									new SqlParameter("@UserAddress1",SqlDbType.VarChar,100),//6
									new SqlParameter("@UserPhoneNo",SqlDbType.Char,50),//7
									new SqlParameter("@VehEngineNo",SqlDbType.VarChar,30),//8
									new SqlParameter("@VehChassisNo",SqlDbType.VarChar,30),//9
									new SqlParameter("@VehRegNo",SqlDbType.Char,15),//10
									new SqlParameter("@InvoiceDate",SqlDbType.DateTime),//11
									new SqlParameter("@MeterReadingKM",SqlDbType.Char,5),//12
									new SqlParameter("@DateOfProblem",SqlDbType.DateTime),//13
									new SqlParameter("@RoadCondition",SqlDbType.Char,10),//14
									new SqlParameter("@NoOfProblems",SqlDbType.Int),//15
									new SqlParameter("@CompletionDate",SqlDbType.DateTime),//16
									new SqlParameter("@PostFlag",SqlDbType.Char,1),//17
									new SqlParameter("@DelFlag",SqlDbType.Char,1),//18
									new SqlParameter("@UpdUser",SqlDbType.VarChar,50),//19
									//new SqlParameter("@UpdDate",SqlDbType.DateTime),//20
									//new SqlParameter("@UpdTime",SqlDbType.DateTime),//21
									new SqlParameter("@UpdTerm",SqlDbType.VarChar,50),//20
									new SqlParameter("@TransferStatus",SqlDbType.Char,1),//21
									new SqlParameter("@Status",SqlDbType.Char,1),//22
									new SqlParameter("@Remarks",SqlDbType.VarChar,200),//23
								  
									new SqlParameter("@TotalAmount",SqlDbType.Float),//24
									new SqlParameter("@SaleDealerCode",SqlDbType.Char,5),//25
									new SqlParameter("@SaleDealerName",SqlDbType.VarChar,100),//26
                                    new SqlParameter("@Picture1",SqlDbType.VarChar,100),//27
                                    new SqlParameter("@Picture2",SqlDbType.VarChar,100),//28
                                     new SqlParameter("@DTRNo",SqlDbType.Char,08),//28
                                      new SqlParameter("@ApprovalFlag",SqlDbType.Char,01),//28
                                        new SqlParameter("@TotalParts",SqlDbType.Float),//28

                                          new SqlParameter("@TotalSublet",SqlDbType.Float),//28

                                            new SqlParameter("@TotalLabor",SqlDbType.Float),//28

								};
            param[0].Value = Session["DealerCode"].ToString();
            if (ddlPPRNo.SelectedValue == "0" || ddlPPRNo.SelectedValue.ToString().Trim() == "__/_____")
            {
                param[1].Value = sysFuncs.AutoGen("PPRMaster", "PPRCode", DateTime.Now.ToString("dd-MM-yyyy"));
                //txtPPRNo.Text = param[1].Value.ToString();
                ViewState["PPRCode"] = param[1].Value.ToString();
            }
            else
            {
                param[1].Value = ddlPPRNo.SelectedValue;
                ViewState["PPRCode"] = ddlPPRNo.SelectedValue;
            }
            //param[1].Value =txtPPRNo.Text;
            param[2].Value = txtJobCardNo.Text;
            param[3].Value = sysFuncs.SaveDate(txtPPRDate.Text);
            param[4].Value = txtCustCode.Text;
            param[5].Value = txtUserName.Text;
            param[6].Value = txtUserAddress.Text;
            param[7].Value = txtPhoneNo.Text;
            param[8].Value = txtEngNo.Text;
            param[9].Value = txtChasisNo.Text;
            param[10].Value = txtRegNo.Text;
            param[11].Value = sysFuncs.SaveDate(txtInvDate.Text);
            param[12].Value = txtKM.Text;
            param[13].Value = sysFuncs.SaveDate(dtProblemDate.Text);
            param[14].Value = ddlRoadCond.SelectedValue;
            param[15].Value = 1;
            param[16].Value = sysFuncs.SaveDate(dtComplitionDate.Text);
            param[17].Value = "N";
            param[18].Value = "N";
            param[19].Value = Session["UserName"].ToString();
            //param[19].Value = ObjGenral.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"));
            //param[20].Value = ObjGenral.SaveTime(DateTime.Now.ToString("HH:MM"));
            param[20].Value = GlobalVar.mUserIPAddress;
            param[21].Value = "C";
            param[22].Value = "C";
            param[23].Value = txtCompletionDesc.Text;
            param[24].Value =float.Parse(lblTotalInvAmt.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblTotalInvAmt.Text.Replace("&nbsp;", "").Trim())+
                             float.Parse(txtTotalSublet.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : txtTotalSublet.Text.Replace("&nbsp;", "").Trim())+
                             float.Parse(txtTotalLab.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : txtTotalLab.Text.Replace("&nbsp;", "").Trim());
            param[25].Value = "";
            param[26].Value = "";
            param[27].Value = (Session["Picture1"] == null ? "" : Session["Picture1"].ToString());
            param[28].Value = (Session["Picture2"] == null ? "" : Session["Picture2"].ToString());
            param[29].Value = txtDTRNo.Text;
            param[30].Value = "N";
            param[31].Value = float.Parse(lblTotalInvAmt.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblTotalInvAmt.Text.Replace("&nbsp;", "").Trim());
            param[32].Value = float.Parse(txtTotalSublet.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : txtTotalSublet.Text.Replace("&nbsp;", "").Trim());
            param[33].Value = float.Parse(txtTotalLab.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : txtTotalLab.Text.Replace("&nbsp;", "").Trim());

            if (sysFuncs.ExecuteSP_NonQuery("sp_2W_Service_Insert_PPRMaster", param, Trans))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool InsertPPRDetail(SqlTransaction Trans)
        {
            SqlParameter[] param = {
                                     new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
									   new SqlParameter("@PPRRef",SqlDbType.Char,8),//1
									  new SqlParameter("@ComplainCode",SqlDbType.Char,3),//2
									  new SqlParameter("@DefectCode",SqlDbType.Char,4),//3
									 new SqlParameter("@ComplaintDesc",SqlDbType.VarChar,500),//4
									 new SqlParameter("@LabourRate",SqlDbType.Decimal),//5
									 new SqlParameter("@LabourDesc",SqlDbType.VarChar,50),//6
									 new SqlParameter("@RootCauseFailure",SqlDbType.VarChar,100),//7
									 new SqlParameter("@CorrectiveAction",SqlDbType.VarChar,100)//8
								};

            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = ViewState["PPRCode"].ToString();
            param[2].Value = ddlComplains.SelectedValue.ToString();
            param[3].Value = ddlDefect.SelectedValue.ToString();
            param[4].Value = txtCompletionDesc.Text;
            param[5].Value = 0;
            param[6].Value = txtLaborDesc.Text;
            param[7].Value = txtRootCauseFailure.Text;
            param[8].Value = txtRootCauseFailure.Text;
            //ObjGenral.GetStringValuesAgainstCodes("ComplainCode", txtComplainCode.Text, "ComplainDesc", "ComplainType");
            if (sysFuncs.ExecuteSP_NonQuery("sp_2W_Service_Insert_PPRDetail", param, Trans))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool InsertPPRPartsDetail(DataRow dtRow, SqlTransaction Trans)
        {

            //SqlParameter[] Inser_CustomerEstimate_Delete_param = {
            //                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
            //                                                    new SqlParameter("@PPRCode",SqlDbType.Char,8),

            //                                                };
            //Inser_CustomerEstimate_Delete_param[0].Value = Session["DealerCode"].ToString();
            //Inser_CustomerEstimate_Delete_param[1].Value = ddlPPRNo.SelectedValue.Trim();
            //if  (sysFuncs.ExecuteSP_NonQuery("sp_W2_PPRParts_Detail_Delete", Inser_CustomerEstimate_Delete_param, Trans))
            //{
                SqlParameter[] param = {
                                        new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
										new SqlParameter("@PPRCode",SqlDbType.Char,8),//1
										new SqlParameter("@PartsCode",SqlDbType.Char,8),//2
										new SqlParameter("@Quantity",SqlDbType.Float),//3
										new SqlParameter("@VendorCode",SqlDbType.Char,6),//4
										new SqlParameter("@UnitRate",SqlDbType.Float),//5
										new SqlParameter("@GSTAmount",SqlDbType.Float),//6
										new SqlParameter("@TotalAmount",SqlDbType.Float),//7
										new SqlParameter("@RecStatus",SqlDbType.Char,1),//8
										new SqlParameter("@Casual",SqlDbType.Char,1),//9
                                        new SqlParameter("@PartSourceCode",SqlDbType.VarChar),//10
                                        new SqlParameter("@HandlingParts",SqlDbType.Decimal),//11
                                        new SqlParameter("@HandlingCharges",SqlDbType.Decimal),//11
                                        new SqlParameter("@GSTPer",SqlDbType.Decimal),//11
                                        new SqlParameter("@trackInvNo",SqlDbType.VarChar),//11
                                          new SqlParameter("@ConsNo",SqlDbType.VarChar),//11
									   
								};

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ViewState["PPRCode"].ToString();
                param[2].Value = dtRow["ItemCode"].ToString().Trim();
                param[3].Value = dtRow["Qty"].ToString().Trim();
                param[4].Value = ddlVendor.SelectedValue;
                param[5].Value = dtRow["Price"].ToString().Trim();
                param[6].Value = dtRow["GSTAmount"].ToString().Trim();
                param[7].Value = dtRow["GrossAmount"].ToString().Trim();
                param[8].Value = "".Trim();
                param[9].Value = "";
                param[10].Value = dtRow["PartSourceCode"].ToString().Trim();


                //DataTable dt = sysFunc.GetData("Select HandlingCharges from Dealer where Dealercode='"+Session["DealerCode"].ToString()+"'");
                //if (dt.Rows.Count>0)
                //{

                //    decimal hand = Convert.ToDecimal(dt.Rows[0]["HandlingCharges"].ToString());
                //    hand = (hand * Convert.ToDecimal(dtRow["GrossAmount"].ToString())) / 100;
                //    param[11].Value = hand;
                //}
                if (dtRow["HandlingPer"].ToString().Trim() == "")
                {
                    param[11].Value = Convert.ToDecimal("0");

                }
                else
                {
                    param[11].Value = Convert.ToDecimal(dtRow["HandlingAmount"].ToString().Trim());

                }


                if (dtRow["HandlingPer"].ToString().Trim() == "")
                {
                    param[12].Value = Convert.ToDecimal("0");

                }
                else
                {
                    param[12].Value = Convert.ToDecimal(dtRow["HandlingPer"].ToString().Trim());

                }

                if (dtRow["GSTPer"].ToString().Trim() == "")
                {


                    param[13].Value = Convert.ToDecimal("0");
                }

                else
                {


                    param[13].Value = Convert.ToDecimal(dtRow["GSTPer"].ToString().Trim());

                }
                param[14].Value = dtRow["trackInvNo"].ToString().Trim();
                param[15].Value = dtRow["ConsNo"].ToString().Trim();

                if (sysFuncs.ExecuteSP_NonQuery("sp_2W_Service_Insert_PPRPartsDetail", param, Trans))
                {
                    IsSaved= true;
                }
                else
                {
                    IsSaved= false;
               // }
            }
            return IsSaved;
        }
        private bool InsertPPRWarrantyLabor(DataRow dtRow, SqlTransaction Trans)
        {
            SqlParameter[] param = {
                                        new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
										new SqlParameter("@PPRCode",SqlDbType.Char,8),//1
										new SqlParameter("@JobCardCode",SqlDbType.Char,8),//2
										new SqlParameter("@LaborCode",SqlDbType.VarChar,15),//3
										new SqlParameter("@Rate",SqlDbType.Float),//4
                                        new SqlParameter("@Hr",SqlDbType.Decimal),//4
                                        new SqlParameter("@TotalLabor",SqlDbType.Decimal),//4
                                         new SqlParameter("@PSTPer",SqlDbType.Decimal),//3
									    new SqlParameter("@PSTAmount",SqlDbType.Decimal),//3
									   
								};

            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = ViewState["PPRCode"].ToString();
            param[2].Value = txtJobCardNo.Text;
            param[3].Value = dtRow["LaborCode"].ToString().Trim();
            param[4].Value = Convert.ToDecimal(dtRow["Rate"].ToString().Trim());
            param[5].Value = Convert.ToDecimal(dtRow["Hr"].ToString().Trim());
            param[6].Value = Convert.ToDecimal(dtRow["TotalLabor"].ToString().Trim());
            DataTable dt = sysFunc.GetData("Select PST from Dealer where Dealercode='" + Session["DealerCode"].ToString() + "'");
            if (dt.Rows.Count > 0)
            {

                decimal hand = Convert.ToDecimal(dt.Rows[0]["PST"].ToString());
                param[7].Value = Convert.ToDecimal(dt.Rows[0]["PST"].ToString().Trim());
                hand = (hand * Convert.ToDecimal(dtRow["TotalLabor"].ToString())) / 100;
                param[8].Value = Convert.ToDecimal(dtRow["PSTAmount"].ToString().Trim());
            }
          



            if (sysFuncs.ExecuteSP_NonQuery("sp_2W_Service_Insert_WarrantyJobs", param, Trans))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool InsertPPRSubletLabor(DataRow dtRow, SqlTransaction Trans)
        {

            if(dtRow["SubletAmount"].ToString().Trim()=="" || dtRow["SubletAmount"].ToString().Trim() == null)
            {
                return true;

            }
            SqlParameter[] param = {
                                        new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
										new SqlParameter("@PPRCode",SqlDbType.Char,8),//1
										new SqlParameter("@JobCardCode",SqlDbType.Char,8),//2
										new SqlParameter("@Rate",SqlDbType.Float),//3
                                        new SqlParameter("@PSTPer",SqlDbType.Decimal),//3
									    new SqlParameter("@PSTAmount",SqlDbType.Decimal),//3
									    new SqlParameter("@JobCode",SqlDbType.VarChar,08),//3
								};

            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = ViewState["PPRCode"].ToString();
            param[2].Value = txtJobCardNo.Text;
            param[3].Value = float.Parse(dtRow["SubletAmount"].ToString().Trim());
            param[4].Value = Convert.ToDecimal(dtRow["PST"].ToString().Trim());
            param[5].Value = Convert.ToDecimal(dtRow["PSTAmount"].ToString().Trim());
            param[6].Value = dtRow["JobCode"].ToString().Trim();




            if (sysFuncs.ExecuteSP_NonQuery("sp_2W_Service_Insert_PPRSubletDetail", param, Trans))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2533", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }

            Data.DSReports data = new Data.DSReports();
            ReportDocument RD = new ReportDocument();
            string ccon = CConn.CConnection.GetConnectionString();

            SqlParameter[] param =
        {
            new SqlParameter("@DealerCode",SqlDbType.Char),//0
            new SqlParameter("@PPRCOde",SqlDbType.Char), //1
            
        };

            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = ddlPPRNo.SelectedValue;


            SqlDataReader rder = null;

            SysFunction sysFunc = new SysFunction();
            if (sysFunc.ExecuteSP("SP_PPR_Report", param, ref rder))
            {
                data.SP_PPR_Report.Load(rder);
                dt = data.SP_PPR_Report;
                sysFunc.ExecuteSP("sp_PPRLaborDetail_Print", param, ref rder);
                
                //ds = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(ccon, CommandType.Text, "sp_PPRLaborDetail_Print '" + this.Session["DealerCode"].ToString() + "','" + dt.Rows[0]["PPRCode"].ToString() + "'");
                data.sp_PPRLaborDetail_Print.Load(rder); 
                sysFunc.ExecuteSP("sp_2W_Service_Select_PPRWarrantySubletJobs", param, ref rder);
                data.sp_2W_Service_Select_PPRWarrantySubletJobs.Load(rder);

            }
            if (dt.Rows[0]["VehicleCategory"].ToString() == "HCV")
            {
                RD.Load(Server.MapPath("~\\Modules\\Service\\ServiceReports\\TRReportHCV.rpt"));

            }
            else {
                RD.Load(Server.MapPath("~\\Modules\\Service\\ServiceReports\\TRReport.rpt"));

            }



            RD.DataDefinition.FormulaFields["PPRCode"].Text = "'" + ddlPPRNo.SelectedValue + "'";
            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerFax"].Text = "'" + Session["DealerFax"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Warranty Report'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";

            RD.Database.Tables[0].SetDataSource(data);

            // convert and show
            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "TRReport" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;

            //RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            Session["RD"] = RD;

            string URL;
            URL = "../../../Download/rptViewerService.aspx?FileName=" + FileName;
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2533", "005"))
            {
                Response.Redirect("~/Test.aspx");
            }
            try
            {
                if (ddlPPRNo.SelectedValue.ToString().Trim() == "")
                {
                    sysFuncs.UserMsg(lblMsg, Color.Red, "Please Enter Valid PPR No", null);
                    return;
                }
                if (sysFuncs.IsPosted("PPRMaster", "PPRCode", ddlPPRNo.SelectedValue, string.Empty))
                {
                    sysFuncs.UserMsg(lblMsg, Color.Red, "Can not edit PPR already posted", null);
                    return;
                }

                SqlParameter[] param = {
                                        new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
										new SqlParameter("@PPRCode",SqlDbType. Char,8),//1
										new SqlParameter("@JobCardCode",SqlDbType .Char ,8),//2
									};
                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlPPRNo.SelectedValue;
                param[2].Value = txtJobCardNo.Text.Trim();
                if (sysFuncs.ExecuteSP_NonQuery("sp_2W_Service_PostPPR", param))
                {
                    sysFuncs.UserMsg(lblMsg, Color.Green, "Posted Successfully ", null);
                    ClearAll();
                    btnSave.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
                //SendAlert("Error: " + ex.Message);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //SqlTransaction Trans = null;
            bool Chek = false;
            try
            {
                //TextBox[] textBoxes = { txtPPRNo };
                //if (!MasterValidation(textBoxes))
                //{
                //	return;
                //}
                if (sysFuncs.IsPosted("PPRMaster", "PPRCode", ddlPPRNo.SelectedValue, string.Empty))
                {
                    sysFuncs.UserMsg(lblMsg, Color.Red, "Can not Delete PPR is posted", null);
                    return;
                }
                //if (ObjGenral.IsDelFlag("PPRMaster", "PPRCode", txtPPRNo.Text, string.Empty) == false)
                //{
                //    ObjGenral.UserMsg(lblMsg, Color.Red, "Record dose not exist", null);
                //    return;
                //}
                if (ddlPPRNo.SelectedValue != "__/_____")
                {
                    if (sysFuncs.IsExist("PPRCode", ddlPPRNo.SelectedValue, "PPRMaster", string.Empty) == false)
                    {
                        lblMsg.Text = "Record dose not exist";
                        //sysFuncs.UserMsg(lblMsg, Color.Red, "Record dose not exist", ddlPPRNo.SelectedValue);
                        return;
                    }
                }
                //if (ObjTrans.BeginTransaction(ref Trans) == false)
                //{
                //    return;
                //}
                SqlParameter[] param = {
                                        new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
										new SqlParameter("@PPRCode",SqlDbType. Char,8),//1
									};
                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlPPRNo.SelectedValue;
                if (sysFuncs.ExecuteSP_NonQuery("sp_2W_Service_DeletePPR", param))
                {

                    sysFuncs.UserMsg(lblMsg, Color.Green, "Deleted Successfully", null);
                    ClearAll();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Deletealert()", true);
                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
                //SendAlert("Error: " + ex.Message);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearAll();
            lblMsg.Text = string.Empty;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);
        }
        public void ClearAll()
        {
            dsSublet = (DataSet)ViewState["PPRSublet"];
            ClearMasterDetail();

            dtParts.Rows.Clear();
            if (dtJobs != null)
            {
                dtJobs.Rows.Clear();
                //LoadGridJobs();
            }
            LoadEmptyGrid();
            LoadEmptyGridJobs();
            LoadEmptyGridSublet();
            Image1.ImageUrl = "";
            Image2.ImageUrl = "";
            Session["Picture1"] = "";
            Session["Picture2"] = "";
        }
        public void ClearMasterDetail()
        {
            //txtPPRNo.Text = string.Empty;
            txtPPRDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtJobCardNo.Text = "";
            txtJobCardDate.Text = string.Empty;
            txtPPRTotalAmount.Text = string.Empty;
            txtRegNo.Text = string.Empty;
            txtChasisNo.Text = string.Empty;
            txtEngNo.Text = string.Empty;
            txtVehModel.Text = string.Empty;
            txtVehColor.Text = string.Empty;
            txtInvDate.Text = string.Empty;
            txtProdCode.Text = string.Empty;
            txtInvoiceNo.Text = string.Empty;
            //txtDealerCode.Text = string.Empty;
            //txtDealerName.Text = string.Empty;
            txtKM.Text = string.Empty;
            ddlComplains.SelectedIndex = 0;
            ddlDefect.SelectedIndex = 0;
            txtCompletionDesc.Text = string.Empty;
            //lblMsg.Text = string.Empty;
            txtCustCode.Text = string.Empty;
            txtCustName.Text = string.Empty;
            txtUserName.Text = string.Empty;
            txtUserAddress.Text = string.Empty;
            txtPhoneNo.Text = string.Empty;
            ddlRoadCond.SelectedIndex = 0;
            txtCorrectiveAction.Text = string.Empty;
            txtRootCauseFailure.Text = string.Empty;
            txtLaborDesc.Text = string.Empty;
            txtLaborAmount.Text = string.Empty;
            dtComplitionDate.Text = string.Empty;
            dtProblemDate.Text = string.Empty;
            ddlVendor.SelectedIndex = 0;
            ddlPPRNo.SelectedIndex = 0;
            txtDTRNo.Text = string.Empty;
        }


        private void cleadata()
        {
            sysFunc.Clearddl(Page);
            sysFunc.ClearTextBoxes(Page);
            LoadDLLs();
        }

        protected void grdParts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            //    {
            //        //DropDownList ddList = (DropDownList)e.Row.FindControl("ddlEditType");
            //        ////bind dropdown-list
            //        //DataTable dt = sysFunc.GetData("Select PartSourceDesc from SuppliedBy");
            //        //ddList.DataSource = dt;
            //        //ddList.DataTextField = "PartSourceDesc";
            //        //ddList.DataValueField = "PartSourceDesc";
            //        //ddList.DataBind();

            //        //DataRowView dr = e.Row.DataItem as DataRowView;
            //        //ddList.SelectedItem.Text = dr["category_name"].ToString();
            //       // ddList.SelectedValue = dr["PartSourceDesc"].ToString();
            //    }
            //}

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //    // if row type is DataRow, add ProductSales value to TotalSales
            //    Total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "GrossAmount"));
            //else if (e.Row.RowType == DataControlRowType.Footer)
            //    // If row type is footer, show calculated total value
            //    // Since this example uses sales in dollars, I formatted output as currency
            //    e.Row.Cells[1].Text = String.Format("{0:c}", Total);
            //e.Row.Cells[2].Visible = false;
            //e.Row.Cells[5].Visible = 
            //e.Row.Cells[6].Visible = 
          //  e.Row.Cells[11].Visible = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblRowPartAmount = (Label)e.Row.Cells[5].FindControl("Qty");
                countPartsRecQty = countPartsRecQty + int.Parse(lblRowPartAmount.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblRowPartAmount.Text.Replace("&nbsp;", "").Trim());
                Label lblRowPartTotal = (Label)e.Row.Cells[7].FindControl("Total");
                Label lblRowHandAmount = (Label)e.Row.Cells[10].FindControl("HandlingAmount");
                Label lblRowGSTAmount = (Label)e.Row.Cells[9].FindControl("GSTAmount");
               // countPartsRecQty = countPartsRecQty + Convert.ToDecimal(lblRowPartTotal.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblRowPartTotal.Text.Replace("&nbsp;", "").Trim());
                totParts = totParts + double.Parse(lblRowPartTotal.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblRowPartTotal.Text.Replace("&nbsp;", "").Trim())+ 
                    double.Parse(lblRowHandAmount.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblRowHandAmount.Text.Replace("&nbsp;", "").Trim())+
                   double.Parse(lblRowGSTAmount.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblRowGSTAmount.Text.Replace("&nbsp;", "").Trim());
                //countParts = countParts + Convert.ToDecimal(e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim());
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                totParts = 0;
                countPartsRecQty = 0;
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
                lblPartCount.Text = countPartsRecQty.ToString();
                lblTotalInvAmt.Text = totParts.ToString();
               
            }


        }

        protected void ddljob_code_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddljob_code.SelectedIndex == 0)
            //{
            //    cleadata();
            //    return;
            //}

            //btnSave.Enabled = false;

            //DataSet ds = new DataSet();
            //SqlParameter[] param = {
            //new SqlParameter("@DealerCode",SqlDbType. Char,5),
            //new SqlParameter("@JobCardCode",SqlDbType. Char,8),
            //                           };
            //param[0].Value = Session["DealerCode"].ToString();
            //param[1].Value = ddljob_code.SelectedValue;
            //ds = sysFuncs.FillDataSet("sp_2W_Service_Select_JobCardMaster_New", param);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    txtJobCardNo.Text = ddljob_code.SelectedValue;
            //    txtJobCardDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["RecDate"].ToString()).ToString();

            //    txtPPRTotalAmount.Text = Convert.ToString(Convert.ToDouble(ds.Tables[0].Rows[0]["JobsTotal"].ToString()) +
            //                     Convert.ToDouble(ds.Tables[0].Rows[0]["PartsTotal"].ToString()) +
            //                     Convert.ToDouble(ds.Tables[0].Rows[0]["LubPartsTotal"].ToString()) +
            //                     Convert.ToDouble(ds.Tables[0].Rows[0]["SubletTotal"].ToString()));
            //    txtRegNo.Text = ds.Tables[0].Rows[0]["RegNo"].ToString();
            //    txtChasisNo.Text = ds.Tables[0].Rows[0]["ChassisNo"].ToString();
            //    txtEngNo.Text = ds.Tables[0].Rows[0]["EngineNo"].ToString();
            //    txtVehModel.Text = ds.Tables[0].Rows[0]["InvoiceDate"].ToString().Substring(6, 4);
            //    txtVehColor.Text = ds.Tables[0].Rows[0]["ColorDesc"].ToString();
            //    txtInvDate.Text = ds.Tables[0].Rows[0]["InvoiceDate"].ToString();
            //    txtCustCode.Text = ds.Tables[0].Rows[0]["CusCode"].ToString();
            //    txtProdCode.Text = ds.Tables[0].Rows[0]["ProdCode"].ToString();
            //    txtInvoiceNo.Text = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();
            //    txtKM.Text = ds.Tables[0].Rows[0]["KM"].ToString();
            //    txtCustName.Text = ds.Tables[0].Rows[0]["CusDesc"].ToString();

            //    txtUserName.Text = ds.Tables[0].Rows[0]["CusDesc"].ToString();
            //    txtUserAddress.Text = ds.Tables[0].Rows[0]["Address1"].ToString();
            //    txtPhoneNo.Text = ds.Tables[0].Rows[0]["Phone1"].ToString();
            //    dtProblemDate.Text = ds.Tables[0].Rows[0]["ProblemDate"].ToString();
            //    txtCompletionDesc.Text = ds.Tables[0].Rows[0]["Problem"].ToString();
            //    txtRootCauseFailure.Text = ds.Tables[0].Rows[0]["CusComment"].ToString();
            //    txtCorrectiveAction.Text = ds.Tables[0].Rows[0]["CorrectiveAction"].ToString();
            //    txtCorrectiveAction.Text = ds.Tables[0].Rows[0][""].ToString();


            //    FillJobCardDetailGrid();

            // }


        }

        protected void btnPic1Upload_Click(object sender, EventArgs e)
        {

            if (Pic1Upload.HasFile)
            {
                HttpPostedFile postedFile = Pic1Upload.PostedFile;

                string imgName = Pic1Upload.FileName;
                string imgPath = "VehicleImages/" + imgName;

                int imgSize = Pic1Upload.PostedFile.ContentLength;

                string fileExtension = Path.GetExtension(imgName);

                if (fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".gif"
                    || fileExtension.ToLower() == ".png" || fileExtension.ToLower() == ".bmp")
                {
                    Pic1Upload.SaveAs(Server.MapPath("~/" + imgPath));
                    Session["Picture1"] = imgPath;
                    Image1.ImageUrl = "~/" + imgPath;
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "Only images (.jpg, .png) can be uploaded";
                }
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Please Upload The Image...";
            }

        }

        protected void btnPic2Upload_Click(object sender, EventArgs e)
        {

            if (Pic2Upload.HasFile)
            {
                HttpPostedFile postedFile = Pic2Upload.PostedFile;
                string imgName = Pic2Upload.FileName;
                string imgPath = "VehicleImages/" + imgName;

                int imgSize = Pic2Upload.PostedFile.ContentLength;

                System.Drawing.Image img = System.Drawing.Image.FromStream(Pic2Upload.PostedFile.InputStream);

                int imgWidth = img.Width;
                int imgHeight = img.Height;

                if (imgWidth <= 500 && imgHeight <= 500)
                {
                    string fileExtension = Path.GetExtension(imgName);

                    if (fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".gif"
                        || fileExtension.ToLower() == ".png" || fileExtension.ToLower() == ".bmp")
                    {
                        Pic2Upload.SaveAs(Server.MapPath("~/" + imgPath));
                        Session["Picture2"] = imgPath;
                        Image2.ImageUrl = "~/" + imgPath;
                    }
                    else
                    {
                        lblMsg.Visible = true;
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        lblMsg.Text = "Only images (.jpg, .png) can be uploaded";
                    }
                } else
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "Images less than 500 x 500 are acceptable";
                }


            } else
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Please Upload The Image...";
            }


        }



        protected void ImageButtonDTRNO_Click1(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 78; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_DTRForm(ImageButtonDTRNO, ViewState["lookupid"].ToString(), "", "../../../../");
            btnValue = 1;


            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }
        protected void IMGLabour_Click(object sender, ImageClickEventArgs e)
        {

            ViewState["lookupid"] = 79; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_WarrantyLabour(IMGLabour, ViewState["lookupid"].ToString(), "", "../../../../");
            btnValue = 2;


            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }
        protected void SelectedPartDetail(string item)
        {
            try
            {


                if (btnValue == 1)
                {
                    txtDTRNo.Text = item;
                    string sql = "Select ItemDesc from Item where ItemCode='" + item + "' and DealerCode in('" + Session["DealerCode"].ToString() + "','COMON')";

                    DataSet dt = new DataSet();
                    SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",Session["DealerCode"].ToString()),
                                    new
                                    SqlParameter("@FIRCode",item)
                               };

                    dt = sysFuncs.FillDataSet("sp_GetFIRMaster", param);
                    txtDTRNo.Text = item;
                    txtJobCardNo.Text = (dt.Tables[0].Rows[0]["RoNo"].ToString());
                    txtJobCardDate.Text = (dt.Tables[0].Rows[0]["RoDate"].ToString()).ToString();

                    txtPPRTotalAmount.Text = Convert.ToString(
                                     Convert.ToDouble(dt.Tables[0].Rows[0]["PartsTotal"].ToString()) +
                                     Convert.ToDouble(dt.Tables[0].Rows[0]["SubletTotal"].ToString()));
                    txtRegNo.Text = dt.Tables[0].Rows[0]["RegNo"].ToString();
                    txtChasisNo.Text = dt.Tables[0].Rows[0]["ChassisNo"].ToString();
                    txtEngNo.Text = dt.Tables[0].Rows[0]["EngineNo"].ToString();
                    txtVehModel.Text = dt.Tables[0].Rows[0]["VersionCode"].ToString();
                    txtVehColor.Text = dt.Tables[0].Rows[0]["ColorDesc"].ToString();
                    txtInvDate.Text = dt.Tables[0].Rows[0]["DeliveryDate"].ToString();
                    txtCustCode.Text = dt.Tables[0].Rows[0]["CusDesc"].ToString();
                    txtProdCode.Text = dt.Tables[0].Rows[0]["ProdCode"].ToString();
                    Session["ProdCode"]= dt.Tables[0].Rows[0]["ProdCode"].ToString();
                    txtInvoiceNo.Text = dt.Tables[0].Rows[0]["InvoiceNo"].ToString();
                    txtKM.Text = dt.Tables[0].Rows[0]["KM"].ToString();
                    txtCustName.Text = dt.Tables[0].Rows[0]["CusDesc"].ToString();
                    txtVehicleCategory.Text = dt.Tables[0].Rows[0]["VehicleCategory"].ToString();
                    txtUserName.Text = dt.Tables[0].Rows[0]["CusDesc"].ToString();
                    txtUserAddress.Text = dt.Tables[0].Rows[0]["address"].ToString();
                    txtPhoneNo.Text = dt.Tables[0].Rows[0]["Phone1"].ToString();
                    txtDelvDate.Text =dt.Tables[0].Rows[0]["DelvDate"].ToString();
                    dtProblemDate.Text = dt.Tables[0].Rows[0]["ProblemDate"].ToString();
                    txtCompletionDesc.Text = dt.Tables[0].Rows[0]["Problem"].ToString();
                    txtRootCauseFailure.Text = dt.Tables[0].Rows[0]["CusComment"].ToString();
                    txtCorrectiveAction.Text = dt.Tables[0].Rows[0]["CorrectiveAction"].ToString();
                    ddlDefect.SelectedValue = dt.Tables[0].Rows[0]["DefectCode"].ToString();
                    FillJobCardDetailGrid();
                   
                    //txtEngineNo.Text = dt.Tables[0].Rows[0]["EngineNo"].ToString();
                    //txtModelCode.Text = dt.Tables[0].Rows[0]["VersionCode"].ToString();
                    //txtRegNo.Text = dt.Tables[0].Rows[0]["RegNo"].ToString();
                    //txtColor.Text = dt.Tables[0].Rows[0]["Color"].ToString();
                    //txtCustName.Text = dt.Tables[0].Rows[0]["CusDesc"].ToString();
                    //txtAddress.Text = dt.Tables[0].Rows[0]["CusAddress"].ToString();
                    //txtPhnNo.Text = dt.Tables[0].Rows[0]["Phone"].ToString();
                    //txtRepairDate.Text = dt.Tables[0].Rows[0]["JobCardDate"].ToString();
                    //txtDelDate.Text = dt.Tables[0].Rows[0]["DelvDate"].ToString();
                    //txtKM.Text = dt.Tables[0].Rows[0]["KM"].ToString();
                    //txtWarantyBookNo.Text = dt.Tables[0].Rows[0]["WorkbookNo"].ToString();
                    //txtPartCost.Text = dt.Tables[0].Rows[0]["PartsTotal"].ToString();
                    //txtSubletCost.Text = dt.Tables[0].Rows[0]["SubletTotal"].ToString();
                    //txtLubCost.Text = dt.Tables[0].Rows[0]["LubPartsTotal"].ToString();
                    //txtLabourCost.Text = dt.Tables[0].Rows[0]["JobsTotal"].ToString();
                    //txtTotalCost.Text = Convert.ToString(Convert.ToDecimal(txtSubletCost.Text) + Convert.ToDecimal(txtPartCost.Text) + Convert.ToDecimal(txtLabourCost.Text) + Convert.ToDecimal(txtLubCost.Text));
                    //if (dt.Tables[0].Rows[0]["BuiltUnit"].ToString() == "CBU")
                    //{
                    //    RadioBtnCBU.Checked = true;
                    //}
                    //else
                    //{
                    //    RadioBtnCKD.Checked = true;
                    //}
                    //txtBodyType.Text = dt.Tables[0].Rows[0]["BodyType"].ToString();
                    //txtPayLoad.Text = dt.Tables[0].Rows[0]["PayLoad"].ToString();
                    //txtVehicleCategory.Text = dt.Tables[0].Rows[0]["VehicleCategory"].ToString();





                }
                else if (btnValue == 2)
                {

                    txtWLaborCode.Text = item;
                    string sql = "Select LaborDesc,Hr,Rate from WarrantyLabor where LaborCode='" + item + "' and DealerCode IN ('" + Session["DealerCode"].ToString() + "','COMON')";

                    DataTable dt = new DataTable();

                    dt = sysFunc.GetData(sql);

                    if (dt.Rows.Count > 0)
                    {
                        decimal hr, rate, total;
                        txtWLaborDesc.Text = dt.Rows[0]["LaborDesc"].ToString();
                        txtWAmount.Text = dt.Rows[0]["Rate"].ToString();
                        rate = Convert.ToDecimal(dt.Rows[0]["Rate"].ToString());
                        txtLaborHr.Text = dt.Rows[0]["Hr"].ToString();
                        if (dt.Rows[0]["Hr"].ToString()=="" || dt.Rows[0]["Hr"].ToString()==null) {
                            hr = 0;
                        }
                        hr = Convert.ToDecimal(dt.Rows[0]["Hr"].ToString());
                        
                        txtTotalLabor.Text = Convert.ToString(hr * rate);
                    }


                }
            }
            catch (Exception ex)
            {
                sysFunc.UserMsg(lblMsg, Color.Red, ex.Message);
            }
        }

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            DataSet ds = new DataSet();
            double DRemainingQty = 0.0, DRate = 0.0, DQty = 0.0;
            try
            {
                if (txtWLaborCode.Text == "")
                {
                    sysFunc.UserMsg(lblMsg, Color.Red, "Please Select Labour Job First");
                    return;
                }


               
                else
                {
                    AddNewRow();
                 
                }
              

            }
           
       
            catch (Exception ex)
            {
                sysFunc.UserMsg(lblMsg, Color.Red, ex.Message);

            }
}

        private void AddNewRow()
        {

            DataSet ds = new DataSet();
            ds = (DataSet)Session["WarrantyLaborTable"];
            DataRow row = ds.Tables["WarrantyLaborTable"].NewRow();
            DataTable dt = sysFunc.GetData("Select PST from Dealer where Dealercode='" + Session["DealerCode"].ToString() + "'");
            Decimal hand=0; 

            if (dt.Rows.Count > 0)
            {

                 hand = Convert.ToDecimal(dt.Rows[0]["PST"].ToString());
                row["PSTPer"] = hand.ToString() ;
                hand = (hand * Convert.ToDecimal(txtTotalLabor.Text.Trim())) / 100;
             
                row["PSTAmount"] = hand.ToString();

            }


            row["LaborCode"] = txtWLaborCode.Text.Trim();
            row["LaborDesc"] = txtWLaborDesc.Text.Trim();
            row["Hr"] = txtLaborHr.Text.Trim();
            row["Rate"] = txtWAmount.Text.Trim();
            row["TotalLabor"] = txtTotalLabor.Text.Trim();
            
            ds.Tables["WarrantyLaborTable"].Rows.Add(row);
            //ViewState["ObjPOds"] = ds;
            gvWLabor.DataSource = ds.Tables["WarrantyLaborTable"];
            gvWLabor.DataBind();
            Session["WarrantyLaborTable"] = ds;
            dtJobs = ds.Tables["WarrantyLaborTable"];
            txtTotalLab.Text= Convert.ToString(Convert.ToDecimal(txtTotalLabor.Text.Trim())+Convert.ToDecimal(hand.ToString()));
            Session["WarrantyLaborTables"] = ds.Tables["WarrantyLaborTable"];
            // Total = Convert.ToDecimal(dtJobs.Compute("Sum(TotalLabor)", string.Empty));
            Total = Convert.ToDecimal(txtPPRTotalAmount.Text) + Convert.ToDecimal(txtTotalLabor.Text)+hand;
        //   txtPPRTotalAmount.Text = Total.ToString();

        }

        protected void gvWLabor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //    // if row type is DataRow, add ProductSales value to TotalSales
            //    Total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalLabor"));
            //else if (e.Row.RowType == DataControlRowType.Footer)
            //    // If row type is footer, show calculated total value
            //    // Since this example uses sales in dollars, I formatted output as currency
            //    e.Row.Cells[1].Text = String.Format("{0:c}", Total);

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblRowPartAmount = (Label)e.Row.Cells[4].FindControl("LblTotalLabor");
               // Label lblRowRate = (Label)e.Row.Cells[9].FindControl("lblRowPartAmount");
                Label lblRowPSTAmount = (Label)e.Row.Cells[6].FindControl("LblPSTAmount");
                totlabor = totlabor + double.Parse(lblRowPartAmount.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblRowPartAmount.Text.Replace("&nbsp;", "").Trim())
                    + double.Parse(lblRowPSTAmount.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblRowPSTAmount.Text.Replace("&nbsp;", "").Trim());
                //totParts = totParts + double.Parse(e.Row.Cells[7].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[7].Text.Replace("&nbsp;", "").Trim());
                //countParts = countParts + int.Parse(e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim());
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                totParts = 0;
                countPartsRecQty =0;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {

                txtTotalLab.Text = totlabor.ToString();
            }

        }

        protected void grdParts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            DataTable dt = new DataTable();

            grdParts.EditIndex = -1;

            dt = (DataTable)Session["PartsDetailsDS"];

            grdParts.DataSource = dt;

            grdParts.DataBind();
        }

        protected void grdParts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
             DataTable dtEditing = new DataTable();

             Label lblPartSource = (Label)grdParts.Rows[e.NewEditIndex].FindControl("lblPartSource");
             Label lbltrackInv = (Label)grdParts.Rows[e.NewEditIndex].FindControl("tbttrackInv");
                Label lblConsNo = (Label)grdParts.Rows[e.NewEditIndex].FindControl("lblCons");
                Session["lbltrackInv"] = lbltrackInv.Text;
                Session["lblPartSource"] = lblPartSource.Text;
                Session["lblConsNo"] = lblConsNo.Text;
                
                grdParts.EditIndex = e.NewEditIndex;

            dtEditing = (DataTable)Session["PartsDetailsDS"];
            //Session["UnitCategoryDS"] = dtEditing;
            grdParts.DataSource = dtEditing;

            grdParts.DataBind();
            
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;

            }
        }

        protected void grdParts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt_update = new DataTable();
            Label PartNo = (Label)grdParts.Rows[e.RowIndex].FindControl("PartNo");
            Label lblPartSource = (Label)grdParts.Rows[e.RowIndex].FindControl("lblPartSource");
            DropDownList ddlEditType = (DropDownList)grdParts.Rows[e.RowIndex].FindControl("ddlEditType");
            TextBox txttrackInv = (TextBox)grdParts.Rows[e.RowIndex].FindControl("txtInvNo");
            Label lbltrackInv = (Label)grdParts.Rows[e.RowIndex].FindControl("txttrackInv");
            TextBox txtCons = (TextBox)grdParts.Rows[e.RowIndex].FindControl("txtConsNo");
            
            
            Label lblConsNo = (Label)grdParts.Rows[e.RowIndex].FindControl("txtCons");
            if (ddlEditType.Text.Trim() == string.Empty)
            {
           
                lblMsg.Visible = true;
                lblMsg.Text = " Parts Source can not null or empty.";
                return;
            }
            dt_update = (DataTable)Session["PartsDetailsDS"];

            foreach (DataRow dr in dt_update.Rows)
            {
                if (dr["PartNo"].ToString() == PartNo.Text)
                {
                    if (ddlEditType.Text.Trim() == "002") {
                        dr["HandlingAmount"] = "0";
                       // dr["trackInvNo"] = txttrackInv.Text;
                       // dr["ConsNo"] = txtCons.Text;
                    }

                        dr["PartSourceCode"] = ddlEditType.SelectedItem.ToString().Trim();
                        dr["trackInvNo"] = txttrackInv.Text;
                        dr["ConsNo"] = txtCons.Text;
                 }
            }

            grdParts.EditIndex = -1;

            grdParts.DataSource = dt_update;
            grdParts.DataBind();

            Session["PartsDetailsDS"] = dt_update;
        }

        protected void grd_Sublet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //    // if row type is DataRow, add ProductSales value to TotalSales
            //    Total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "SubletAmount"));
            //else if (e.Row.RowType == DataControlRowType.Footer)
            //    // If row type is footer, show calculated total value
            //    // Since this example uses sales in dollars, I formatted output as currency
            //    e.Row.Cells[1].Text = String.Format("{0:c}", Total);
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    Label lblRowPartAmount = (Label)e.Row.Cells[9].FindControl("LblTotalLabor");
            //    // Label lblRowRate = (Label)e.Row.Cells[9].FindControl("lblRowPartAmount");
            //    Label lblRowPSTAmount = (Label)e.Row.Cells[9].FindControl("LblPSTAmount");
            //    totlabor = totlabor + double.Parse(lblRowPartAmount.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblRowPartAmount.Text.Replace("&nbsp;", "").Trim())
            //        + double.Parse(lblRowPSTAmount.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblRowPSTAmount.Text.Replace("&nbsp;", "").Trim());
            //    //totParts = totParts + double.Parse(e.Row.Cells[7].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[7].Text.Replace("&nbsp;", "").Trim());
            //    //countParts = countParts + int.Parse(e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim());
            //}
            //else if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    totParts = 0;
            //    countPartsRecQty = 0;
            //}
            //else if (e.Row.RowType == DataControlRowType.Footer)
            //{

            //    txtTotalLab.Text = totlabor.ToString();
            //}

        }

        protected void grd_Sublet_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void grd_Sublet_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void grd_Sublet_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

       

        private void ClearPartInfo()
        {
            txtWLaborCode.Text = "";
            txtWLaborDesc.Text = "";
            txtWAmount.Text = "";
            txtLaborHr.Text = "";

        }

        protected void BtnClear1_Click(object sender, ImageClickEventArgs e)
        {
            ClearPartInfo();
        }

        private void FillJobCardDetailGrid()
        {
            DataSet dsPartsDetail;

            SqlParameter[] dsParam = {  new SqlParameter("@DealerCode",SqlDbType.Char,5),
									new SqlParameter("@JobCardCode",SqlDbType.Char,8)};
            dsParam[0].Value = Session["DealerCode"].ToString();
            dsParam[1].Value = txtJobCardNo.Text;
            SqlParameter[] dsParam2 = {  new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)};
            dsParam2[0].Value = Session["DealerCode"].ToString();
            dsParam2[1].Value = txtDTRNo.Text;
            dsPartsDetail = new DataSet(); dsPartsDetail = sysFuncs.FillDataSet("SP_PartsDetail_DTR", dsParam2);
            if (dsPartsDetail.Tables[0].Rows.Count == 0)
            { dsPartsDetail.Tables[0].Rows.Add(dsPartsDetail.Tables[0].NewRow()); }
            grdParts.DataSource = dsPartsDetail;
            grdParts.DataBind();
            dtParts = dsPartsDetail.Tables[0];
          //  Total = Convert.ToDecimal(dsPartsDetail.Tables[0].Compute("Sum(GrossAmount)", string.Empty)) + Convert.ToDecimal(dsPartsDetail.Tables[0].Compute("Sum(HandlingAmount)", string.Empty)) + Convert.ToDecimal(dsPartsDetail.Tables[0].Compute("Sum(GSTAmount)", string.Empty));
          //  txtPPRTotalAmount.Text = Total.ToString();
          //  txtPartsTotal.Text = Total.ToString();
            Session["PartsDetailsDS"] = dsPartsDetail.Tables[0];
            dsSublet = new DataSet(); dsSublet = sysFuncs.FillDataSet("SP_SubletDetail", dsParam);
            //if (dsSublet.Tables[0].Rows.Count == 0)
            //{ dsSublet.Tables[0].Rows.Add(dsSublet.Tables[0].NewRow()); }
          
            dsFIRImage = sysFuncs.FillDataSet("sp_W2_FIR_ImageDetail_Select", dsParam2);
            //  if (dsJobCardParts.Tables[0].Rows.Count == 0) dsJobCardParts.Tables[0].Rows.Add(dsJobCardParts.Tables[0].NewRow());
            ViewState["Image"] = dsFIRImage.Tables[0];
            ImgDataList.DataSource = dsFIRImage; ImgDataList.DataBind();
            ImageDT = (DataTable)ViewState["Image"];
            grd_Sublet.DataSource = dsSublet;
            grd_Sublet.DataBind();
            dtSublet = dsSublet.Tables[0];
            Session["WarrantySubletLaborTable"] = dsSublet.Tables[0];
            if(dtSublet.Rows.Count == 0)
            {
                Total = 0;
                

            }
            //if (dtSublet.Rows[0]["SubletAmount"].ToString() == null || dtSublet.Rows[0]["SubletAmount"].ToString() == "" || dtSublet.Rows[0]["SubletAmount"].ToString() == "0")
            //{
            //    Total = 0;
            //}
            else
            {
                Total = Convert.ToDecimal(dtSublet.Compute("Sum(SubletAmount)", string.Empty)) + Convert.ToDecimal(dtSublet.Compute("Sum(PSTAmount)", string.Empty));
                txtTotalSublet.Text = Total.ToString();
            }

            Total = Total + Convert.ToDecimal(txtPPRTotalAmount.Text);
           // txtPPRTotalAmount.Text = Total.ToString();


        }

        protected void ddlPPRNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPPRNo.SelectedIndex == 0)
            {
                cleadata();
                //Create_Grid();
                return;
            }

            //btnWarSave.Enabled = true;

            ViewData();
        }
        protected void lnkRemove_Click1(object sender, EventArgs e)
        {
            try
            {
                dtJobs = (DataTable)Session["WarrantyLaborTables"];
                dsJobs = (DataSet)Session["WarrantyLaborTable"];
                //ImageButton btn = sender as ImageButton;
                // TableCell tc = btn.Parent as TableCell;
                // GridViewRow gvr = tc.Parent as GridViewRow;
                // dtJobs.Rows.RemoveAt(gvr.RowIndex);
                //Load grid 
                dtJobs.Rows.Clear();
                dsJobs.Tables[0].Rows.Clear();
                gvWLabor.DataSource = dtJobs;
                Session["WarrantyLaborTables"] = dtJobs;
            //    Session["WarrantyLaborTable"] = dsJobs;
                gvWLabor.DataBind();
                   

            }
            catch (Exception ex)
            {
                sysFunc.UserMsg(lblMsg, Color.Red, ex.Message);
            }
        }
        //protected void btnWarSave_Click(object sender, EventArgs e)
        //{

        //    DateTime TRDate = DateTime.ParseExact(txtTRDate.Text, "dd-MM-yyyy", null);
        //    DateTime ApprovalDate = DateTime.ParseExact(txtAprovalDate.Text, "dd-MM-yyyy", null);

        //    try
        //    {

        //        if (ObjTrans.BeginTransaction(ref Trans) == true)
        //        {

        //            string sql = "update PPRMaster set TRNo = '" + txtTRNo.Text + "' , TRDate = '" + TRDate + "' , ApprovalNo = '" + txtAprovalNo.Text + "' , ApprovalDate = '" + ApprovalDate + "', ApprovalStatus = '" + ddlAppSt.SelectedValue.ToString().Trim() + "' , ComplainRemarks = '" + txtComplainRemarks.Text + "' where DealerCode = '" + Session["DealerCode"].ToString() + "' And PPRCode = '" + ddlPPRNo.SelectedValue.Trim() + "'";


        //            sysFuncs.ExecuteQuery(sql, Trans);
        //        }
        //        ObjTrans.CommittTransaction(ref Trans);

        //    }
        //    catch (Exception ex)
        //    {

        //        ObjTrans.RollBackTransaction(ref Trans);
        //        lblMsg.Visible = true;
        //        lblMsg.Text = ex.Message;
        //    }
        //    ClearWarrantyTab();

        //}

        //protected void btnWarClear_Click(object sender, EventArgs e)
        //{
        //    ClearWarrantyTab();
        //}

        //private void ClearWarrantyTab()
        //{
        //    txtAprovalDate.Text = string.Empty;
        //    txtAprovalNo.Text = string.Empty;
        //    txtComplainRemarks.Text = string.Empty;
        //    txtTRDate.Text = string.Empty;
        //    txtTRNo.Text = string.Empty;
        //}

    }
}