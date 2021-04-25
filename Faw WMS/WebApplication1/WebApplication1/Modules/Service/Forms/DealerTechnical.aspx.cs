using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CConn;
using Microsoft.ApplicationBlocks.Data;
using CrystalDecisions.Shared;
using DXBMS.Data;
using System.Threading;
using System.Windows.Forms;
using PdfSharp.Pdf;

namespace DXBMS.Modules.Service.Forms
{
    public partial class DealerTechnical : System.Web.UI.Page
    {
        SysFunction sysfunc = new SysFunction();
        SysFunctions sysfuncs = new SysFunctions();
        SecurityBll sec = new SecurityBll();
        Transaction ObjTrans = new Transaction();
        MainBLL objMBll = new MainBLL();
        SqlTransaction Trans;
        DataSet ds;
        clsLookUp clslook = new clsLookUp();
        static int btnValue = 0;
        static string Status;
        byte[] imgcontent;
        string p;
        DataTable JobDT, LubDT, SubletDT, PartsDT, ConPartsDT, BayDT, BoutDT,ImageDT;
        DataSet dsJobCardDetail, dsJobCardParts, dsJobCardLub, dsJobCardSublet, dsJobCardConParts, dsJobCardBay, dsJobCardBoutParts, dsFIRImage;
        bool search_result, search_item;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2538", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (ViewState["Parts"] != null) PartsDT = (DataTable)ViewState["Parts"];
            if (ViewState["Job"] != null) JobDT = (DataTable)ViewState["Job"];
            if (ViewState["Lub"] != null) LubDT = (DataTable)ViewState["Lub"];
            if (ViewState["SubLet"] != null) SubletDT = (DataTable)ViewState["SubLet"];
            if (ViewState["Image"] != null) ImageDT = (DataTable)ViewState["Image"];
            ViewState["ddlParts"] = ddlParts.SelectedValue.ToString().Trim();
            if (Page.IsPostBack)
            {
             
                if (!string.IsNullOrEmpty(Session["LookUpData"].ToString()))
                {
                    SelectedPartDetail(Session["LookUpData"].ToString());
                }
            }

            if (!IsPostBack)
            {

                if (Session["FIRCode"] != null)
                {
                    Session["LookUpData"] = string.Empty;
                    ThreadStart childthreat = new ThreadStart(Load_ddlParts);
                    Thread child = new Thread(childthreat);
                    Session["Sketch"] = "";
                    child.Start();
                    LoadDDL_JobCategory();
                    Load_ddlVehRegNo();
                    LoadDDL_City();
                    LoadDDL_FIRMaster();
                    createPartsDT();
                    createImageDT();
                    txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    string WhereClause = "DealerCode = '" + Session["DealerCode"].ToString() + "'";

                    string[] Columns = new string[] { "FIRCode", "ChassisNo", "RegNo", "EngineNo" };
                    sysfunc.GetMultiColumnsDDL(ddlDRNo, Columns, "FIRMaster ", WhereClause, "FIRCode", "", false, false);
                    string[] Columns2 = new string[] { "DefectCode", "DefectDesc" };
                    sysfunc.GetMultiColumnsCombo(ddlProblemDefect, Columns2, "Defect", "DealerCode in( '" + Session["DealerCode"].ToString() + "','COMON')", "DefectCode", "", false, false);
                    string leadId = Session["FIRCode"].ToString();
                    ViewState["FIRCode"] = leadId;
                    ddlDRNo.SelectedValue = ViewState["FIRCode"].ToString();
                    ddlDRNo_OnSelectedIndexChanged(null, null);



                }
                else
                {
                    Session["LookUpData"] = string.Empty;
                    ThreadStart childthreat = new ThreadStart(Load_ddlParts);
                    Thread child = new Thread(childthreat);
                    Session["Sketch"] = "";
                    child.Start();
                    LoadDDL_JobCategory();
                    Load_ddlVehRegNo();
                    LoadDDL_City();
                    LoadDDL_FIRMaster();
                    createPartsDT();
                    createSubletDT();            //-------------------------------------LUBRICANT
                    createJobDT();
                    createImageDT();


                    txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    string WhereClause = "DealerCode = '" + Session["DealerCode"].ToString() + "' And DelFlag='N'";

                    string[] Columns = new string[] { "FIRCode", "ChassisNo", "RegNo", "EngineNo" };
                    sysfunc.GetMultiColumnsDDL(ddlDRNo, Columns, "FIRMaster ", WhereClause, "FIRCode", "", false, false);
                    string[] Columns2 = new string[] { "DefectCode", "DefectDesc" };
                    sysfunc.GetMultiColumnsCombo(ddlProblemDefect, Columns2, "Defect", "DealerCode in( '" + Session["DealerCode"].ToString() + "','COMON')", "DefectCode", "", false, false);



                }
                Session["FIRCode"] = null;
            }

           






                //  Session["Sketch"] = "";
        }
        // Session["Sketch"] = "";

        private void createImageDT()
        {
            ImageDT = new DataTable();
           
            ImageDT.Columns.Add(new DataColumn("ImageAddress", typeof(string)));
            ImageDT.Columns.Add(new DataColumn("ImageData",System.Type.GetType("System.Byte[]")));

            //PartsDT.Rows.Add(new object[] { null, null, null, null, null, null, null, null, null,null });                        
            ImgDataList.DataSource = ImageDT; ImgDataList.DataBind(); ViewState["Image"] = ImageDT;
        }
        private void createPartsDT()
        {
            PartsDT = new DataTable();
            PartsDT.Columns.Add(new DataColumn("ID", typeof(int)));
            PartsDT.Columns.Add(new DataColumn("PartNo", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("PartsDesc", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("Dep", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("DepAmount", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("Qty", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("Price", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("Total", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("Type", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("ItemCode", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("RecQty", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("ConsNo", typeof(string)));
            //PartsDT.Rows.Add(new object[] { null, null, null, null, null, null, null, null, null,null });                        
            gvJobCardParts.DataSource = PartsDT; gvJobCardParts.DataBind(); ViewState["Parts"] = PartsDT;
        }
       
        private void createJobDT()
        {

            JobDT = new DataTable();
            JobDT.Columns.Add(new DataColumn("ID", typeof(int)));
            JobDT.Columns.Add(new DataColumn("JobCode", typeof(string)));
            JobDT.Columns.Add(new DataColumn("DefJobDesc", typeof(string)));
            JobDT.Columns.Add(new DataColumn("Bay", typeof(string)));
            JobDT.Columns.Add(new DataColumn("BayDesc", typeof(string)));
            JobDT.Columns.Add(new DataColumn("EmpCode", typeof(string)));
            JobDT.Columns.Add(new DataColumn("EmpName", typeof(string)));
            JobDT.Columns.Add(new DataColumn("Amount", typeof(string)));
            JobDT.Columns.Add(new DataColumn("Type", typeof(string)));
            //JobDT.Rows.Add(new object[] { null, null, null, null, null, null });
            //gvJobCard.DataSource = JobDT; gvJobCard.DataBind(); ViewState["Job"] = JobDT;

        }
        private void createSubletDT()
        {
            SubletDT = new DataTable();
            SubletDT.Columns.Add(new DataColumn("ID", typeof(int)));
            SubletDT.Columns.Add(new DataColumn("VendorCode", typeof(string)));
            SubletDT.Columns.Add(new DataColumn("Vendor", typeof(string)));
            SubletDT.Columns.Add(new DataColumn("JobDescription", typeof(string)));
            SubletDT.Columns.Add(new DataColumn("Remarks", typeof(string)));
            SubletDT.Columns.Add(new DataColumn("SubletDate", typeof(string)));
            SubletDT.Columns.Add(new DataColumn("SubletAmount", typeof(string)));
            SubletDT.Columns.Add(new DataColumn("PayableAmount", typeof(string)));
            SubletDT.Columns.Add(new DataColumn("JobCode", typeof(string)));
            SubletDT.Columns.Add(new DataColumn("BayID", typeof(string)));
            //SubletDT.Rows.Add(new object[] { null, null, null, null, null, null, null, null,null});  
            gvSublet.DataSource = SubletDT;
            gvSublet.DataBind();
            ViewState["SubLet"] = SubletDT;
        }
        private void Load_ddlParts()
        {
            string sQuery;
            //string[] Columns = new string[] { "PartItemNo", "ItemDesc" };
            //sysfunc.GetMultiColumnsDDL(ddlFailedPart, Columns, "Item", "DealerCode = '" + Session["DealerCode"].ToString() + "'", "PartItemNo", "", false, false);
            //sysfunc.FillDropDown(ddlPart1, "select PartItemNo , ItemDesc from Item where DealerCode = '" + Session["DealerCode"].ToString() + "' ", "ItemDesc", "PartItemNo", "Select");
            //sysfunc.FillDropDown(ddlPart2, "select PartItemNo , ItemDesc from Item where DealerCode = '" + Session["DealerCode"].ToString() + "' ", "ItemDesc", "PartItemNo", "Select");
            //sysfunc.FillDropDown(ddlPart3, "select PartItemNo , ItemDesc from Item where DealerCode = '" + Session["DealerCode"].ToString() + "' ", "ItemDesc", "PartItemNo", "Select");
            //sysfunc.FillDropDown(ddlPart4, "select PartItemNo , ItemDesc from Item where DealerCode = '" + Session["DealerCode"].ToString() + "' ", "ItemDesc", "PartItemNo", "Select");

            try
            {
                if(Session["Parts"] == null)
                {
                    sQuery = "SP_SelectParts '" + Session["DealerCode"].ToString() + "', 'P'";

                    if (sysfuncs.ExecuteQuery(sQuery, ref ds))
                    {
                        Session["Parts"] = ds;

                    }
                }
                

            }
            catch (ThreadAbortException e)
            {
                //lblmessage.Text += "<br /> child thread - exception";
            }
            finally
            {
                //lblmessage.Text += "<br /> child thread - unable to catch the  exception";
            }
        }
        private void LoadDDL_JobCategory()
        {
            string WhereQuery = " DealerCode in('" + Session["DealerCode"].ToString() + "','COMON')"; 

            string[] Columns = new string[] { "JobCatCode", "JobCatDesc" };
            sysfunc.GetMultiColumnsCombo(ddlsubject, Columns, "JobCategory", WhereQuery, "JobCatCode", "", false, false);
           
        }
        private void LoadDDL_FIRMaster()
        {
            string WhereClause = "DealerCode = '" + Session["DealerCode"].ToString() + "' And DelFlag='N'";
            string[] Columns = new string[] {"FIRCode", "ChassisNo", "RegNo", "EngineNo" };
                sysfunc.GetMultiColumnsDDL(ddlDRNo, Columns, "FIRMaster ", WhereClause, "FIRCode", "", false, false);

        }

        private void LoadDDL_City()
        {
            string WhereQuery = "DealerCode = 'COMON'";
            //string[] Columns = new string[] { "CityCode", "CityDesc" };
            //SysFunc.GetMultiColumnsDDL(ddlcity, Columns, "City", WhereQuery, "CityCode", "", false, false);
            sysfunc.FillDropDown(ddlcity, "Select LTrim(RTrim(CityCode)) as CityCode,CityDesc From City where " + WhereQuery, "CityDesc", "CityCode", "Select");
            sysfunc.FillDropDown(ddlCityFrom, "Select LTrim(RTrim(CityCode)) as CityCode,CityDesc From City where " + WhereQuery, "CityDesc", "CityCode", "Select");
            sysfunc.FillDropDown(ddlCityTo, "Select LTrim(RTrim(CityCode)) as CityCode,CityDesc From City where " + WhereQuery, "CityDesc", "CityCode", "Select");
            string[] Columns2 = new string[] { "DefectCode", "DefectDesc" };
            sysfunc.GetMultiColumnsCombo(ddlProblemDefect, Columns2, "Defect", WhereQuery, "DefectCode", "", false, false);
        }

        private void Load_ddlVehRegNo()
        {
            string WhereClause = "DealerCode IN ('" + Session["DealerCode"].ToString() + "','COMON')  ";

            string[] Columns = new string[] { "ChassisNo", "RegNo", "EngineNo" };
            sysfunc.GetMultiColumnsDDL(ddlVinNo, Columns, "CustomerVehicle ", WhereClause, "ChassisNo", "", false, false);
        }

        protected void btnClearJC_Click(object sender, EventArgs e)
        {
            clearAll();
        }

        private void clearAll()
        {
            sysfunc.ClearTextBoxes(Page);
            sysfunc.Clearddl(Page);
            //lblMsg.Text = "";
            Session["Sketch"] = "";
            createPartsDT();
            createImageDT();
            createSubletDT();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);
        }

        protected void btnSaveJC_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2538", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (!sec.UserRight("2538", "003"))
            {
                Response.Redirect("~/Test.aspx");
            }

            try
            {
                if (ddlVinNo.SelectedIndex == 0) {
                    lblMsg.Text = "Please select VIN No first";
                    return;
                }
                //if (txtFailedPart.Text.Trim() == "")
                //{
                //    lblMsg.Text = "Please select Failed part first";
                //    return;
                //}
                if (ImgDataList.Items.Count<=0)
                {
                    sysfunc.UserMsg(lblMsg, Color.Red, "Please Add Image At Least one! ");
                    return;
                }
                if (gvJobCardParts.Rows.Count <= 0)
                {
                    sysfunc.UserMsg(lblMsg, Color.Red, "Please Add Parts At Least one! ");
                    gvJobCardParts.Focus();
                    return;
                }
                System.Web.UI.WebControls.TextBox[] textBoxes = { txtRegNo , txtKM , txtSubject , txtEngineNo , txtInspDate };
                if (!MasterValidation(textBoxes)) return;
                else
                {
                    JobCardEntry();
                    LoadDDL_FIRMaster();
                }
            }
            catch (Exception ex)
            {

                sysfunc.UserMsg(lblMsg, Color.Red, ex.Message,txtSRNo);
            }
        }

        private void JobCardEntry()
        {   /////////////////

            if (txtSRNo.Text.Trim() == string.Empty)
            {
                txtSRNo.Text = sysfuncs.AutoGen("FIRMaster", "FIRCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
            }

            #region JobCard Master Param
            SqlParameter[] JobCardMaster_param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
           /*1*/ new SqlParameter("@FIRCode",SqlDbType.VarChar,8),
           /*2*/ new SqlParameter("@FIRDate",SqlDbType.DateTime),
           /*3*/ new SqlParameter("@Subject",SqlDbType.VarChar,100),
           /*4*/ new SqlParameter("@RegNo",SqlDbType.VarChar,15),
           /*5*/ new SqlParameter("@ChassisNo",SqlDbType.VarChar,30),
           /*6*/ new SqlParameter("@EngineNo",SqlDbType.VarChar,30),           
           /*7*/ new SqlParameter("@CusComment",SqlDbType.VarChar,400),
           /*8*/ new SqlParameter("@KM",SqlDbType.Char,8),
           /*9*/ new SqlParameter("@ProblemDate",SqlDbType.DateTime),
           /*10*/ new SqlParameter("@ItemCode",SqlDbType.Char,25),
           /*11*/ new SqlParameter("@VendorCode",SqlDbType.Char,6),
           /*12*/ new SqlParameter("@Defect",SqlDbType.VarChar,800),
           /*13*/ new SqlParameter("@Problem",SqlDbType.VarChar,1000),
           /*14*/ new SqlParameter("@CorrectiveAction",SqlDbType.VarChar,800),           
           /*15*/ new SqlParameter("@DelFlag",SqlDbType.Char,1),
           /*16*/ new SqlParameter("@UpdUser",SqlDbType.Char,50),           
           
           /*17*/ new SqlParameter("@UpdTerm",SqlDbType.Char,50),
           /*18*/ new SqlParameter("@Pic1",SqlDbType.VarChar,200),
           /*19*/ new SqlParameter("@Part1",SqlDbType.Char,8),
           /*20*/ new SqlParameter("@Part2",SqlDbType.Char,8),
           /*21*/ new SqlParameter("@Part3",SqlDbType.Char,8),
           /*22*/ new SqlParameter("@Part4",SqlDbType.Char,8),
           /*23*/ new SqlParameter("@InspDate",SqlDbType.DateTime),
           /*24*/ new SqlParameter("@Status",SqlDbType.VarChar,30),
           /*25*/ new SqlParameter("@Result",SqlDbType.VarChar,30),
           /*26*/ new SqlParameter("@RoNo",SqlDbType.VarChar,10),
           /*27*/ new SqlParameter("@RoDate",SqlDbType.DateTime),
           /*28*/ new SqlParameter("@PartAttach",SqlDbType.Char,1),
           /*29*/ new SqlParameter("@followupdate",SqlDbType.DateTime),
           /*30*/ new SqlParameter("@Request",SqlDbType.VarChar,200),
           /*31*/ new SqlParameter("@KeyNo",SqlDbType.VarChar,15),
           /*32*/ new SqlParameter("@SerialNo",SqlDbType.Char,8),
           /*33*/ new SqlParameter("@OperationalTO",SqlDbType.Char,8),
           /*34*/ new SqlParameter("@OperationalFrom",SqlDbType.Char,8),
           /*35*/ new SqlParameter("@BreakDownLoc",SqlDbType.Char,8),
            /*35*/ new SqlParameter("@JobCatCode",SqlDbType.Char,3),
             /*35*/ new SqlParameter("@DefectCode",SqlDbType.Char,4),

        };

            JobCardMaster_param[0].Value = Session["DealerCode"].ToString();

            JobCardMaster_param[2].Value = sysfunc.SaveDate(txtDate.Text.Trim());    
            
            JobCardMaster_param[3].Value = txtSubject.Text.Trim();

            JobCardMaster_param[4].Value = txtRegNo.Text.Trim().ToUpper();

            JobCardMaster_param[5].Value = ddlVinNo.SelectedValue.ToString().Trim();

            JobCardMaster_param[6].Value = txtEngineNo.Text.Trim();

            JobCardMaster_param[7].Value = txtCustComp.Text.Trim();

            JobCardMaster_param[8].Value = txtKM.Text.Trim().ToUpper();

            JobCardMaster_param[9].Value = sysfunc.SaveDate(txtFailureDate.Text);

            JobCardMaster_param[10].Value = txtFailedPart.Text.Trim();

            JobCardMaster_param[11].Value = Session["DealerCode"].ToString();
            if (RadioButton1.Checked)
            {
                JobCardMaster_param[12].Value = "Y";
            }
            else {
                 JobCardMaster_param[12].Value = "N";
            }

            JobCardMaster_param[13].Value = txtPrblmCause.Text.Trim();

            JobCardMaster_param[14].Value = txtCorAction.Text.Trim();

            JobCardMaster_param[15].Value = "N";            
            if (Session["UserName"] == null)
            {
                JobCardMaster_param[16].Value = "";
            }
            else
            {
                JobCardMaster_param[16].Value = Session["UserName"].ToString().Trim();
            }
            JobCardMaster_param[16].Value = Session["UserName"].ToString().Trim();
            JobCardMaster_param[17].Value = GlobalVar.mUserIPAddress;
            if (Session["Sketch"].ToString().Trim() == null || Session["Sketch"].ToString().Trim() == "" || Session["Sketch"].ToString() == null)
                JobCardMaster_param[18].Value = "";
            else
                JobCardMaster_param[18].Value = Session["Sketch"].ToString().Trim();

            JobCardMaster_param[19].Value = "";
            JobCardMaster_param[20].Value = "";
            JobCardMaster_param[21].Value = "";
            JobCardMaster_param[22].Value = "";
            JobCardMaster_param[23].Value = sysfunc.SaveDate(txtInspDate.Text.Trim());
            JobCardMaster_param[24].Value = ddlStatusC.SelectedItem.ToString().Trim();
            JobCardMaster_param[25].Value = ddlResultC.SelectedItem.ToString().Trim();
            JobCardMaster_param[26].Value = txtRoNo.Text.Trim();
            JobCardMaster_param[27].Value = sysfunc.SaveDate(txtRepairDate.Text.Trim());
          
            if (RadioButton3.Checked)
            {
                JobCardMaster_param[28].Value = "Y";
            }
            else
            {
                JobCardMaster_param[28].Value = "N";
            }
           
            JobCardMaster_param[29].Value = sysfunc.SaveDate(txtFollowUpDate.Text);
            JobCardMaster_param[30].Value = txtRequest.Text.Trim();
            JobCardMaster_param[31].Value = txtKeyNo.Text.Trim();
            JobCardMaster_param[32].Value = txtSerial.Text.Trim();
            JobCardMaster_param[33].Value = ddlcity.SelectedValue;
            JobCardMaster_param[34].Value = ddlCityTo.SelectedValue;
            JobCardMaster_param[35].Value = ddlCityFrom.SelectedValue;
            JobCardMaster_param[36].Value = ddlsubject.SelectedValue;
            JobCardMaster_param[37].Value = ddlProblemDefect.SelectedValue;
            #endregion

            try
            {
               
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {                    
                        DataSet dsJc = new DataSet();

                        if (ddlDRNo.SelectedIndex == 0)
                        {
                            JobCardMaster_param[1].Value = sysfuncs.AutoGen("FIRMaster", "FIRCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy")); //txtReferenceNo.Text;
                            HFJobCard.Value = JobCardMaster_param[1].Value.ToString();
                        if (sysfunc.ExecuteSP_NonQuery("[sp_FIRMaster_Insert]", JobCardMaster_param, Trans)) {

                            bool c = (rowInJobCardPartsDetail(gvJobCardParts) == true ? Inser_JobCardPartsDetail() : false);
                            bool d = (rowInFIRImageDetail(ImgDataList) == true ? Inser_FIRImageDetail() : false);


                        }
                        }
                        else
                        {

                            JobCardMaster_param[1].Value = ddlDRNo.SelectedValue.ToString().Trim();
                            HFJobCard.Value = JobCardMaster_param[1].Value.ToString();

                        if (sysfunc.ExecuteSP_NonQuery("[sp_UpdateFIRMasterFull]", JobCardMaster_param, Trans)) {

                            Inser_JobCardPartsDetail();
                            Inser_FIRImageDetail();

                        }
                        }
                     

                    if (ObjTrans.CommittTransaction(ref Trans) == true)
                    {
                        sysfunc.UserMsg(lblMsg, Color.Green, "Record Saved Successfully: " + HFJobCard.Value);
                        clearAll();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);
                    }
                    else
                    {
                        ObjTrans.RollBackTransaction(ref Trans);
                        sysfunc.UserMsg(lblMsg, Color.Red, "Record not saved Try again. Or contact to support team ");
                        clearAll();
                        // MessageBox.Show("Record not saved Try again. Or contact to support team ","Message");
                    }
                    clearAll();

                }
            }
            catch (Exception ex)
            {
                sysfunc.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message, txtSRNo);
                ObjTrans.RollBackTransaction(ref Trans);
            }
        }
        private bool rowInFIRImageDetail(DataList gvFIRImage)
        {
            search_item = false;
            if (ImgDataList.Items.Count > 0)
            {
                search_item = true;
            }
            //foreach (GridViewRow row in gvJobCardParts.Rows) { if (row.Cells[2].Text.Replace("&nbsp;", "").Trim() != "") search_item = true; }
            return search_item;
        }
        private bool rowInJobCardPartsDetail(GridView gvJobCardParts)
        {
            search_item = false;
            if (gvJobCardParts.Rows.Count > 0)
            {
                search_item = true;
            }
            //foreach (GridViewRow row in gvJobCardParts.Rows) { if (row.Cells[2].Text.Replace("&nbsp;", "").Trim() != "") search_item = true; }
            return search_item;
        }
        private bool Inser_JobCardPartsDetail()
        {
            try
            {
                bool flag = false;
                // if (gvJobCardParts.Rows.Count > 0)
                //{
                SqlParameter[] JobCardPartsDetail_Delete_param = {
                                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                                                                 };
                JobCardPartsDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                //JobCardPartsDetail_Delete_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                JobCardPartsDetail_Delete_param[1].Value = txtSRNo.Text;
                SqlParameter[] JobCardPartsDetail_Insert_param = {
                                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8),
                                                                    new SqlParameter("@ItemCode",SqlDbType.VarChar,8),
                                                                    new SqlParameter("@PartNo",SqlDbType.Char,18),
                                                                    new SqlParameter("@Qty",SqlDbType.VarChar,10),
                                                                    new SqlParameter("@Price",SqlDbType.VarChar,50),
                                                                    new SqlParameter("@RecQty",SqlDbType.VarChar,50),
                                                                    new SqlParameter("@Type",SqlDbType.Char,1),
                                                                    new SqlParameter("@Dep",SqlDbType.VarChar,5),
                                                                    new SqlParameter("@FIRCode",SqlDbType.Char,8),
                                                                    new SqlParameter("@ConsNo",SqlDbType.VarChar,50)
                                                                 };

                if (sysfunc.ExecuteSP_NonQuery("sp_W2_FIR_PartsDetail_Delete", JobCardPartsDetail_Delete_param, Trans))
                {
                    PartsDT = (DataTable)ViewState["Parts"];
                    DataRow[] drr = PartsDT.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {
                        JobCardPartsDetail_Insert_param[0].Value = Session["DealerCode"].ToString();
                        //JobCardPartsDetail_Insert_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                        JobCardPartsDetail_Insert_param[1].Value = txtRoNo.Text == "" ? HFJobCard.Value.Trim() : txtRoNo.Text;
                        if (PartsDT.Rows[i]["ItemCode"].ToString() != "")
                        {
                            JobCardPartsDetail_Insert_param[2].Value = PartsDT.Rows[i]["ItemCode"].ToString(); //Item Code System Generted num. Get it from lookup
                            JobCardPartsDetail_Insert_param[3].Value = PartsDT.Rows[i]["PartNo"].ToString();
                            JobCardPartsDetail_Insert_param[4].Value = PartsDT.Rows[i]["Qty"].ToString();
                            JobCardPartsDetail_Insert_param[5].Value = PartsDT.Rows[i]["Price"].ToString();
                            JobCardPartsDetail_Insert_param[6].Value = PartsDT.Rows[i]["RecQty"].ToString();
                            
                                JobCardPartsDetail_Insert_param[7].Value = "P"; //Type
                                JobCardPartsDetail_Insert_param[8].Value = "0"; //Dep
                            JobCardPartsDetail_Insert_param[9].Value = txtSRNo.Text;
                            JobCardPartsDetail_Insert_param[10].Value = PartsDT.Rows[i]["ConsNo"].ToString();
                            sysfunc.ExecuteSP_NonQuery("sp_W2_FIR_PartsDetail_Insert", JobCardPartsDetail_Insert_param, Trans);
                            flag = true;
                        }
                    }
                }
                // }
                return flag;
            }
            catch (Exception ex) { throw ex; }

        }
        private bool Inser_FIRImageDetail()
        {
            try
            {
                bool flag = false;
                // if (gvJobCardParts.Rows.Count > 0)
                //{
                SqlParameter[] JobCardPartsDetail_Delete_param = {
                                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    new SqlParameter("@FIRCode",SqlDbType.Char,8)
                                                                 };
                JobCardPartsDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                //JobCardPartsDetail_Delete_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                JobCardPartsDetail_Delete_param[1].Value = txtSRNo.Text;
                SqlParameter[] JobCardPartsDetail_Insert_param = {
                                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    new SqlParameter("@FIRCode",SqlDbType.Char,8),
                                                                     new SqlParameter("@JobCardCode",SqlDbType.Char,8),
                                                                    new SqlParameter("@ImagePath",SqlDbType.VarChar,100),
                                                                     new SqlParameter("@ImageData",SqlDbType.VarBinary),

                                                                 };

                if (sysfunc.ExecuteSP_NonQuery("sp_W2_FIR_ImageDetail_Delete", JobCardPartsDetail_Delete_param, Trans))
                {
                    ImageDT = (DataTable)ViewState["Image"];
                    DataRow[] drr = ImageDT.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {
                        byte[] imgbyte = (byte[])ImageDT.Rows[i]["ImageData"];
                      //  MemoryStream ms = new MemoryStream(imgbyte);
                        JobCardPartsDetail_Insert_param[0].Value = Session["DealerCode"].ToString();
                        //JobCardPartsDetail_Insert_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                        JobCardPartsDetail_Insert_param[1].Value = txtSRNo.Text == "" ? HFJobCard.Value.Trim() : txtSRNo.Text;
                        JobCardPartsDetail_Insert_param[2].Value = txtRoNo.Text == "" ? HFJobCard.Value.Trim() : txtRoNo.Text;
                        JobCardPartsDetail_Insert_param[3].Value = ImageDT.Rows[i]["ImageAddress"].ToString();
                        JobCardPartsDetail_Insert_param[4].Value= imgbyte;
                        //Item Code System Generted num. Get it from lookup
                        sysfunc.ExecuteSP_NonQuery("sp_W2_FIR_ImageDetail_Insert", JobCardPartsDetail_Insert_param, Trans);
                        flag = true;
                        
                    }
                }
                // }
                return flag;
            }
            catch (Exception ex) { throw ex; }

        }
        protected void OnClick_btnUpload(object sender, EventArgs e)
        {
            if (PicUpload.HasFile)
            {
                if (ImgDataList.Items.Count >= 10)
                {
                    sysfunc.UserMsg(lblMsg, Color.Red, "Max 10 images are allowed", null);
                    lblImgAddress.Focus();
                    return;
                }

                HttpPostedFile postedFile = PicUpload.PostedFile;
               
                string imgName = PicUpload.FileName;
                string imgPath = "DTRImage/" + imgName;
                FileInfo fi = new FileInfo(imgName);

                int imgSize = PicUpload.PostedFile.ContentLength;
                if (imgSize>= 1000000)
                {
                  sysfunc.UserMsg(lblMsg, Color.Red, "Images should be less or equal to 1MB ", null);
                    lblImgAddress.Focus();
                 
                  
                    return;
                }
                // imgcontent =PicUpload.FileBytes;
                if (fi.Exists)
                {
                    sysfunc.UserMsg(lblMsg, Color.Red, "Image name already exist please change name!  ", null);
                    lblImgAddress.Focus();


                    return;
                }

                string fileExtension = Path.GetExtension(imgName);

                if (fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".gif"||fileExtension.ToLower() == ".jpeg" 
                    || fileExtension.ToLower() == ".png" || fileExtension.ToLower() == ".bmp")
                {
                    PicUpload.SaveAs(Server.MapPath("../../../" + imgPath));
                    Session["Sketch"] = imgPath;
                    p = Session["Sketch"].ToString();
                    lblImgAddress .Text = imgPath;
                    Stream fs = PicUpload.PostedFile.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    imgcontent = br.ReadBytes((int)fs.Length);
                    search_result = false;
                    if (ImageDT.Rows.Count > 0)
                    {
                        foreach (DataRow rowimageDT in ImageDT.Rows)
                        {
                            if (rowimageDT["ImageAddress"].ToString().Trim() == ""
                                | rowimageDT["ImageAddress"].ToString().Trim() == imgPath)
                            {
                                Insert_in_ImageDT(rowimageDT); search_result = true;
                            }
                        }
                    }

                    if (search_result == false)
                    {
                        DataRow rowImageDT = ImageDT.NewRow(); Insert_in_ImageDT(rowImageDT); ImageDT.Rows.Add(rowImageDT);
                    }
                    ViewState["Image"] = ImageDT; 
                    ImgDataList.DataSource = ImageDT; ImgDataList.DataBind();
                    //string[] filePaths = Directory.GetFiles(Server.MapPath("~/TestImage/"));
                    //List<ListItem> files = new List<ListItem>();
                    //foreach (string filePath in filePaths)
                    //{
                    //    files.Add(new ListItem(Path.GetFileName(filePath), filePath));
                    //}
                    //Gridview1.DataSource = files;
                    //Gridview1.DataBind();
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "Only images (.jpg,.jpeg, .png, .gif and .bmp) can be uploaded";
                }
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Please Upload The Image...";
            }
           

        }
        private void Insert_in_ImageDT(DataRow rowImageDT)
        {
            rowImageDT["ImageAddress"] = lblImgAddress.Text.Trim();
            rowImageDT["ImageData"] = imgcontent;
            lblImgAddress.Text = "";
            //if (lblTotalAMount.Text != "")
            //{
            //    lblTotalAMount.Text = Convert.ToString(Convert.ToInt32(lblTotalAMount.Text) + Convert.ToInt32(double.Parse(txtQuantity.Text.Trim()) * double.Parse(txtPartPrice.Text.Trim())));
            //}
            //else
            //{
            //    lblTotalAMount.Text = Convert.ToString(Convert.ToInt32(double.Parse(txtQuantity.Text.Trim()) * double.Parse(txtPartPrice.Text.Trim())));
            //}

        }
        public bool MasterValidation(System.Web.UI.WebControls.TextBox[] textBoxes)
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

        protected void ddlDRNo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMasterData();            
        }

        protected void gvJobCardParts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvJobCardParts_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void lnkRemove_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void LoadMasterData()
        {
            if (ddlDRNo.SelectedIndex == 0)
            {
                clearAll();
                return;
            }

            SqlDataReader dr_emp = null;
            DataTable dt = new DataTable();
            string imagepath;
            string[] filePaths;
            SqlParameter[] dsParam = {                                                       
                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                    new SqlParameter("@FIRCode",SqlDbType.Char,8)
                                 };

            dsParam[0].Value = Session["DealerCode"].ToString();
            dsParam[1].Value = ddlDRNo.SelectedValue.Trim();

            sysfunc.ExecuteSP("sp_GetFIRMaster", dsParam, ref dr_emp);

            dt.Load(dr_emp);

            txtSRNo.Text = dt.Rows[0]["FIRCode"].ToString().Trim();
            txtDate.Text = dt.Rows[0]["FIRDate"].ToString().Trim();
            txtRegNo.Text = dt.Rows[0]["RegNo"].ToString().Trim();
            txtModelCode.Text = dt.Rows[0]["VersionCode"].ToString().Trim();
            txtEngineNo.Text = dt.Rows[0]["EngineNo"].ToString().Trim();
            ddlVinNo.SelectedValue = dt.Rows[0]["ChassisNo"].ToString().Trim();
            txtKM.Text = dt.Rows[0]["KM"].ToString().Trim();
            txtSubject.Text = dt.Rows[0]["Subject"].ToString().Trim();
            txtFailedPart.Text = dt.Rows[0]["ItemCode"].ToString();
            txtFailureDate.Text = dt.Rows[0]["ProblemDate"].ToString().Trim();
            txtInspDate.Text = dt.Rows[0]["InspectionDate"].ToString().Trim();
            txtPrblmCause.Text = dt.Rows[0]["Problem"].ToString().Trim();
            txtCorAction.Text = dt.Rows[0]["CorrectiveAction"].ToString().Trim();
            txtCustComp.Text = dt.Rows[0]["CusComment"].ToString().Trim();
            txtCustName.Text = dt.Rows[0]["CusDesc"].ToString().Trim();
            txtColor.Text = dt.Rows[0]["ColorDesc"].ToString().Trim();
            txtAddress.Text = dt.Rows[0]["address"].ToString().Trim();
            txtPhnNo.Text = dt.Rows[0]["Phone1"].ToString().Trim();
            txtWarantyBookNo.Text = dt.Rows[0]["InvoiceNo"].ToString().Trim();
            txtDelDate.Text = dt.Rows[0]["DeliveryDate"].ToString().Trim();
            ddlStatusC.SelectedItem.Text = dt.Rows[0]["Status"].ToString();
            ddlResultC.SelectedItem.Text = dt.Rows[0]["Result"].ToString();
            txtRoNo.Text = dt.Rows[0]["RoNo"].ToString();
            txtRepairDate.Text = dt.Rows[0]["RoDate"].ToString();
            txtRequest.Text = dt.Rows[0]["Request"].ToString();
            txtFollowUpDate.Text = dt.Rows[0]["followupdate"].ToString();
            txtKeyNo.Text= dt.Rows[0]["KeyNo"].ToString();
            txtSerial.Text = dt.Rows[0]["SerialNo"].ToString();
            if (dt.Rows[0]["OperationalTO"].ToString() == ""|| dt.Rows[0]["OperationalTO"].ToString() == null)
            {
                ddlCityTo.SelectedIndex = 0;

            }
            else {
                ddlCityTo.SelectedItem.Text = dt.Rows[0]["OperationalTO"].ToString();
            }

            if (dt.Rows[0]["OperationalFrom"].ToString() == "" || dt.Rows[0]["OperationalFrom"].ToString()==null)
            {
                ddlCityFrom.SelectedIndex = 0;

            }
            else {
                ddlCityFrom.SelectedItem.Text = dt.Rows[0]["OperationalFrom"].ToString();
            }
           
            if (dt.Rows[0]["BreakDownLoc"].ToString() == ""|| dt.Rows[0]["BreakDownLoc"].ToString()==null)
            {
                ddlcity.SelectedIndex = 0;

            }
            else
            {
                ddlcity.SelectedItem.Text = dt.Rows[0]["BreakDownLoc"].ToString();
            }
            if (dt.Rows[0]["BuiltUnit"].ToString() == "CBU")
            {
                RadioBtnCBU.Checked = true;
            }
            else {
                RadioBtnCKD.Checked = true;
            }
            txtBodyType.Text = dt.Rows[0]["BodyType"].ToString();
            txtPayLoad.Text = dt.Rows[0]["PayLoad"].ToString();
            txtVehicleCategory.Text = dt.Rows[0]["VehicleCategory"].ToString();
            txtPartCost.Text = dt.Rows[0]["PartsTotal"].ToString();
            txtSubletCost.Text = dt.Rows[0]["SubletTotal"].ToString();
            txtLubCost.Text = dt.Rows[0]["LubPartsTotal"].ToString();
            txtLabourCost.Text = dt.Rows[0]["JobsTotal"].ToString();
            txtTotalCost.Text = dt.Rows[0]["TotalPartsCost"].ToString();
             ddlsubject.SelectedValue= dt.Rows[0]["JobCatCode"].ToString().Trim();
            txtInvDate.Text = dt.Rows[0]["InvoiceDate"].ToString();
            txtContPerson.Text = dt.Rows[0]["ContPersonName"].ToString();
            txtContPerCell.Text = dt.Rows[0]["ContPersonCell"].ToString();
            Session["Sketch"]= dt.Rows[0]["Pic1"].ToString();
            //  ddlProblemDefect.SelectedValue = dt.Rows[0]["DefectCode"].ToString().Trim();
            Status = sysfuncs.GetStringValuesAgainstCodes("FIRCode", txtSRNo.Text, "ApprovalStatus", "FIRMaster", Session["DealerCode"].ToString());
            if (dt.Rows[0]["PostFlag"].ToString() == "Y")
            {
                if (dt.Rows[0]["ApprovalFlag"].ToString() == "Y")
                {

                    if (Status == "Approved")
                    {
                        btnSaveDTR.Enabled = false;
                        btnPost.Enabled = false;

                    }
                }
            }
           
            if(dt.Rows[0]["PostFlag"].ToString()=="Y")
            {
                RadioButton5.Checked=true;
                    btnPost.Enabled = false;
                    btnSaveDTR.Enabled = false;
            }
            txtPostDate.Text = dt.Rows[0]["PostDate"].ToString();
            SqlParameter[] dsParam1 = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                                 };
            dsParam1[0].Value = Session["DealerCode"].ToString();
            dsParam1[1].Value = txtSRNo.Text;
            dsJobCardParts = new DataSet();
            dsJobCardParts = sysfuncs.FillDataSet("sp_W2_FIR_PartsDetail_Select", dsParam1);
            //  if (dsJobCardParts.Tables[0].Rows.Count == 0) dsJobCardParts.Tables[0].Rows.Add(dsJobCardParts.Tables[0].NewRow());
            ViewState["Parts"] = dsJobCardParts.Tables[0]; gvJobCardParts.DataSource = dsJobCardParts; gvJobCardParts.DataBind();
            dsJobCardSublet = new DataSet();
            SqlParameter[] dsParam2 = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                                 };
            dsParam2[0].Value = Session["DealerCode"].ToString();
            dsParam2[1].Value = txtRoNo.Text; ;
            dsJobCardSublet = sysfuncs.FillDataSet("sp_W2_JobCard_Sublet_Select", dsParam2);
            // if (dsJobCardSublet.Tables[0].Rows.Count == 0) dsJobCardSublet.Tables[0].Rows.Add(dsJobCardSublet.Tables[0].NewRow());
            ViewState["SubLet"] = dsJobCardSublet.Tables[0]; gvSublet.DataSource = dsJobCardSublet; gvSublet.DataBind();
            dsFIRImage = sysfuncs.FillDataSet("sp_W2_FIR_ImageDetail_Select", dsParam1);
            //  if (dsJobCardParts.Tables[0].Rows.Count == 0) dsJobCardParts.Tables[0].Rows.Add(dsJobCardParts.Tables[0].NewRow());
            ViewState["Image"] = dsFIRImage.Tables[0];
            ImgDataList.DataSource = dsFIRImage; ImgDataList.DataBind();
            ImageDT = (DataTable)ViewState["Image"];
          // Image1.ImageUrl= "~/ImageHandler.ashx?id=" + txtSRNo.Text+"&Dealer="+Session["DealerCode"].ToString();



        }

        protected void lnkRemove_Click1(object sender, ImageClickEventArgs e)
        {

        }

        protected void lnkRemove_Click2(object sender, ImageClickEventArgs e)
        {

        }

        protected void gvJobCard_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvJobCard_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvSublet_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void ddlParts_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt;
            string sql = "Select * from JobCardPartsDetail b inner join Item a on a.ItemCode=b.ItemCode and a.DealerCode in ('COMON','"+Session["DealerCode"].ToString()+"') where b.JobCardCode='" + txtRoNo.Text + "' and b.ItemCode='" + ViewState["ddlParts"].ToString().Trim()+ "' and b.DealerCode='" + Session["DealerCode"].ToString() + "'";
            dt = sysfunc.GetData(sql);
            if (dt.Rows.Count > 0)
            {
                ddlParts.SelectedValue = ViewState["ddlParts"].ToString();
                txtPartCode.Text = dt.Rows[0]["PartNo"].ToString();
                txtPartDesc.Text = dt.Rows[0]["ItemDesc"].ToString();
                txtQuantity.Text = dt.Rows[0]["Qty"].ToString();
                txtPrice.Text = dt.Rows[0]["Price"].ToString();
                txtDep.Text = dt.Rows[0]["Dep"].ToString();
                txtIssueQty.Text= dt.Rows[0]["RecQty"].ToString();
            }
        }

        protected void grvImage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].CssClass = "verticaltext";

            }
        }

        protected void btnDeleteDTR_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2538", "004"))
            {
                Response.Redirect("~/Test.aspx");
            }
            try
            {
                //grl = new General();
                //public bool CodeExists(string strTableNames, string strColumnCode, string strColumeValue)
                //CodeExists(string strTableNames, string strColumnCode, string strColumeValue)

                System.Web.UI.WebControls.TextBox[] textBoxes = {txtCustName, txtSRNo, txtRoNo, txtEngineNo,
                                 };
                if (!MasterValidation(textBoxes))
                {
                    return;
                }


                if (sysfunc.IsExist("DTRNo", txtSRNo.Text, "PPRMaster", Session["DealerCode"].ToString()))
                {
                    sysfunc.UserMsg(lblMsg, Color.Red, "DTR can not delete is exist in Warranty Claim", txtSRNo);
                    return;
                }

                if (sysfunc.IsExist("FIRCode", txtSRNo.Text, "FIRMaster", Session["DealerCode"].ToString(), "And ApprovalFlag='Y'"))
                {
                    sysfunc.UserMsg(lblMsg, Color.Red, "DTR is Approved", txtSRNo);
                    return;
                }
                if (sysfunc.IsExist("FIRCode", txtSRNo.Text, "FIRMaster", Session["DealerCode"].ToString(), "And PostFlag='Y' And ApprovalFlag='Y'"))
                {
                    sysfunc.UserMsg(lblMsg, Color.Red, "DTR can not be Delete is Posted", txtSRNo);
                    return;
                }


                SqlParameter[] JobCard_Master_Delete_param = {
                                                                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                new SqlParameter("@FIRCode",SqlDbType.Char,8)
                                                              };
                JobCard_Master_Delete_param[0].Value = Session["DealerCode"].ToString();
                JobCard_Master_Delete_param[1].Value = txtSRNo.Text; // ddlJobCardCode.SelectedValue.ToString().Trim();
                if (sysfunc.ExecuteSP_NonQuery("sp_W2_FIR_Master_Delete", JobCard_Master_Delete_param))
                {

                    sysfunc.UserMsg(lblMsg, Color.Green, "Record Deleted Successfully: " + txtSRNo.Text, txtCustName); //ddlJobCardCode.SelectedValue.ToString().Trim()
                    clearAll();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Deletealert()", true);
                }
                else
                {
                    sysfunc.UserMsg(lblMsg, Color.Red, "Record not deleted: " + txtSRNo.Text, txtCustName); //ddlJobCardCode.SelectedValue.ToString().Trim()
                }

            }

            catch (Exception ex)
            {
                sysfunc.UserMsg(lblMsg, Color.Red, ex.Message, txtCustName);
            }
        }

        protected void BtnAdd0_Click(object sender, ImageClickEventArgs e)
        {
            if (txtQuantity.Text.Trim() == "" || txtQuantity.Text.Trim() == "0")
            {
                string script = "alert(\"Quantity should not left blank  !!!\");";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", script, true);
                return;
            }


            //lblMsg.Visible = false;
            System.Web.UI.WebControls.TextBox[] textBoxes = { txtPrice, txtQuantity };
            if (!MasterValidation(textBoxes)) return;
            else
            {
                search_result = false;
                foreach (DataRow rowPartsDT in PartsDT.Rows)
                {
                    if (rowPartsDT["ItemCode"].ToString().Trim() == ""
                        | rowPartsDT["ItemCode"].ToString().Trim() == ViewState["ddlParts"].ToString().Trim())
                    {
                        Insert_in_PartsDT(rowPartsDT); search_result = true;
                    }
                }
                if (search_result == false)
                {
                    DataRow rowPartsDT = PartsDT.NewRow(); Insert_in_PartsDT(rowPartsDT); PartsDT.Rows.Add(rowPartsDT);
                }
                ViewState["Parts"] = PartsDT; gvJobCardParts.DataSource = PartsDT; gvJobCardParts.DataBind();
                //int Count = 0;
                //foreach (DataRow dr in PartsDT.Rows)
                //{
                //    Count = Count + Convert.ToInt32(dr["Qty"]);
                //}
                //txtPartTotalQuantity.Text = Count.ToString();

            }
        }
        private void Insert_in_PartsDT(DataRow rowPartsDT)
        {
            rowPartsDT["PartNo"] = txtPartCode.Text.Trim(); rowPartsDT["PartsDesc"] = txtPartDesc.Text.Trim();
         
                rowPartsDT["Dep"] = "0";
                rowPartsDT["DepAmount"] = "0";
         

            rowPartsDT["Qty"] = txtQuantity.Text.Trim(); rowPartsDT["Price"] = txtPrice.Text.Trim();
            rowPartsDT["Total"] = double.Parse(txtQuantity.Text.Trim()) * double.Parse(txtPrice.Text.Trim());
            rowPartsDT["ItemCode"] = ViewState["ddlParts"].ToString().Trim();
            rowPartsDT["ConsNo"] = txtTrackNo.Text.Trim();

            //if (lblTotalAMount.Text != "")
            //{
            //    lblTotalAMount.Text = Convert.ToString(Convert.ToInt32(lblTotalAMount.Text) + Convert.ToInt32(double.Parse(txtQuantity.Text.Trim()) * double.Parse(txtPartPrice.Text.Trim())));
            //}
            //else
            //{
            //    lblTotalAMount.Text = Convert.ToString(Convert.ToInt32(double.Parse(txtQuantity.Text.Trim()) * double.Parse(txtPartPrice.Text.Trim())));
            //}

        }
        protected void gvSublet_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlVinNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet dsVehInfo = new DataSet();
            SqlParameter[] param = {                                 
                                    new SqlParameter("@DealerCode",SqlDbType.VarChar,5), 
                                    new SqlParameter("@ChassisNo",SqlDbType.VarChar,30)
                               };


            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = ddlVinNo.SelectedValue.ToString().Trim(); // ddlRegNo.Text.Trim();//(HFRegNo.Value == "" ? null : HFRegNo.Value);

            dsVehInfo = sysfuncs.FillDataSet("sp_W2_CustomerVehicleDTR_Select", param);
            if (dsVehInfo.Tables[0].Rows.Count > 0)
            {
                setVehcileInfo(dsVehInfo);
            }        

        }

        private void setVehcileInfo(DataSet dsVehInfo)
        {
            txtEngineNo.Text = dsVehInfo.Tables[0].Rows[0]["EngineNo"].ToString();
            txtModelCode.Text = dsVehInfo.Tables[0].Rows[0]["ProdDesc"].ToString();
            txtRegNo.Text = dsVehInfo.Tables[0].Rows[0]["RegNo"].ToString();
            txtColor.Text = dsVehInfo.Tables[0].Rows[0]["Color"].ToString();
            txtCustName.Text = dsVehInfo.Tables[0].Rows[0]["CusDesc"].ToString();
            txtAddress.Text = dsVehInfo.Tables[0].Rows[0]["CusAddress"].ToString();
            txtPhnNo.Text = dsVehInfo.Tables[0].Rows[0]["Phone"].ToString();
            txtWarantyBookNo.Text = dsVehInfo.Tables[0].Rows[0]["invoiceNo"].ToString();
            txtDelDate.Text = dsVehInfo.Tables[0].Rows[0]["InvoiceDate"].ToString();
        }


        protected void btnPrintJC_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2538", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }
            Data.DSReports data = new Data.DSReports();
            string CCon = CConnection.GetConnectionString();
            ReportDocument RD = new ReportDocument();
            SqlParameter[] param =
            {
                new SqlParameter("@DealerCode",SqlDbType.Char),//0
                new SqlParameter("@FIRCode",SqlDbType.VarChar)//1

            };

            param[0].Value = Session["DealerCode"].ToString();

            if (ddlDRNo.SelectedIndex == 0)
            {
                lblMsg.Text = "Please Select DRN No. first";
                return;
            }
            else
                param[1].Value = ddlDRNo.SelectedValue.ToString().Trim();

            SqlDataReader rder = null;
            DataTable dt = new DataTable();

            if (sysfunc.ExecuteSP("sp_FIR_Reports", param, ref rder))
            {
               
                data.sp_GetFIRMaster.Load(rder);
                dt = data.sp_GetFIRMaster;
                ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_FIRPartsDetail_Print'" + this.Session["DealerCode"].ToString() + "','" + ddlDRNo.SelectedValue.ToString().Trim() + "'");
            data.sp_FIRPartsDetail_Print.Load(ds.CreateDataReader());
            }
            if (txtVehicleCategory.Text == "HCV") {
                RD.Load(Server.MapPath("~/Modules/Service/ServiceReports/HCVDTR(New).rpt"));
            }
            else {

                RD.Load(Server.MapPath("~/Modules/Service/ServiceReports/rptDTR.rpt"));
            }
            

            RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Dealer Technical Report'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic1"].Text = "'" + Server.MapPath("~")+dt.Rows[0]["item"].ToString() + "'";
            RD.Database.Tables[0].SetDataSource(data);
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

            //// convert and show
            //string FilePath = Server.MapPath("~") + "\\Download\\";
            //string FileName = "BillingReport" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            //string File = FilePath + FileName;

            //Session["RD"] = RD;
            ////RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            //string URL;
            //URL = "../../../../Download/rptViewerService.aspx?FileName=" + FileName;
            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
        protected string SelectedPartDesc(string item)
        {
            try
            {
                DataTable dt = new DataTable();
                string sql = "Select ItemDesc from Item where ItemCode='" + dt.Rows[0]["Part1"].ToString().Trim() + "' and  DealerCode in('" + Session["DealerCode"].ToString() + "','COMON')";



                dt = sysfunc.GetData(sql);

                if (dt.Rows.Count > 0)
                {

                    item = dt.Rows[0]["ItemDesc"].ToString();

                }

            }
            catch (Exception ex)
            {
                sysfuncs.UserMsg(lblMsg, Color.Red, ex.Message);
            }
            return item;

        }
        protected void SelectedPartDetail(string item)
        {
            try
            {
                if (btnValue == 1)
                {
                    txtFailedPart.Text = item;
                    //string sql = "Select ItemDesc from Customer where ItemCode='" + item + "' and DealerCode='" + Session["DealerCode"].ToString() + "'";

                    //DataTable dt = new DataTable();

                    //dt = sysfunc.GetData(sql);

                    //if (dt.Rows.Count > 0)
                    //{

                    //    txtPart1Desc.Text = dt.Rows[0]["ItemDesc"].ToString();

                    //}

                }
               
                if (btnValue == 6)
                {
                    txtRoNo.Text = item;
                    string sql = "Select ItemDesc from Item where ItemCode='" + item + "' and  DealerCode in('" + Session["DealerCode"].ToString() + "','COMON')";

                    DataSet dt = new DataSet();
                    SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",Session["DealerCode"].ToString()),
                                    new SqlParameter("@jobCardCode",item)
                               };

                    dt = sysfuncs.FillDataSet("sp_W2_CustomerVehicleJobCard_Select", param);
                    ddlVinNo.Text = dt.Tables[0].Rows[0]["ChassisNo"].ToString();
                    txtEngineNo.Text = dt.Tables[0].Rows[0]["EngineNo"].ToString();
                    txtModelCode.Text = dt.Tables[0].Rows[0]["VersionCode"].ToString();
                    txtRegNo.Text = dt.Tables[0].Rows[0]["RegNo"].ToString();
                    txtColor.Text = dt.Tables[0].Rows[0]["Color"].ToString();
                    txtCustName.Text = dt.Tables[0].Rows[0]["CusDesc"].ToString();
                    txtAddress.Text = dt.Tables[0].Rows[0]["CusAddress"].ToString();
                    txtPhnNo.Text = dt.Tables[0].Rows[0]["Phone"].ToString();
                    txtRepairDate.Text = dt.Tables[0].Rows[0]["JobCardDate"].ToString();
                    txtDelDate.Text = dt.Tables[0].Rows[0]["DelvDate"].ToString();
                    txtKM.Text = dt.Tables[0].Rows[0]["KM"].ToString();
                    txtWarantyBookNo.Text = dt.Tables[0].Rows[0]["WorkbookNo"].ToString();
                    txtPartCost.Text = dt.Tables[0].Rows[0]["PartsTotal"].ToString();
                    txtSubletCost.Text = dt.Tables[0].Rows[0]["SubletTotal"].ToString();
                    txtLubCost.Text = dt.Tables[0].Rows[0]["LubPartsTotal"].ToString();
                    txtLabourCost.Text = dt.Tables[0].Rows[0]["JobsTotal"].ToString();
                    txtTotalCost.Text = Convert.ToString(Convert.ToDecimal(txtSubletCost.Text) + Convert.ToDecimal(txtPartCost.Text) + Convert.ToDecimal(txtLabourCost.Text) + Convert.ToDecimal(txtLubCost.Text));
                    if (dt.Tables[0].Rows[0]["BuiltUnit"].ToString() == "CBU")
                    {
                        RadioBtnCBU.Checked = true;
                    }
                    else
                    {
                        RadioBtnCKD.Checked = true;
                    }
                    txtBodyType.Text = dt.Tables[0].Rows[0]["BodyType"].ToString();
                    txtPayLoad.Text = dt.Tables[0].Rows[0]["PayLoad"].ToString();
                    txtVehicleCategory.Text = dt.Tables[0].Rows[0]["VehicleCategory"].ToString();
                    txtInvDate.Text= dt.Tables[0].Rows[0]["InvoiceDate"].ToString();
                    txtContPerson.Text = dt.Tables[0].Rows[0]["ContPersonName"].ToString();
                    txtContPerson.Text = dt.Tables[0].Rows[0]["ContPersonCell"].ToString();
                    SqlParameter[] dsParam = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                                 };

                    dsParam[0].Value = Session["DealerCode"].ToString();
                    dsParam[1].Value = item;
                    // (ViewState["JobCardCode"].ToString().Trim() == "" ? null : ViewState["JobCardCode"].ToString().Trim());
                    dsJobCardDetail = new DataSet();

                    dsJobCardDetail = sysfuncs.FillDataSet("sp_W2_JobCard_Detail_Select", dsParam);
                    //if (dsJobCardDetail.Tables[0].Rows.Count == 0) dsJobCardDetail.Tables[0].Rows.Add(dsJobCardDetail.Tables[0].NewRow());
                    //  ViewState["Job"] = dsJobCardDetail.Tables[0]; gvJobCard.DataSource = dsJobCardDetail; gvJobCard.DataBind();
                   
                    objMBll.FillDrp_SP(ddlParts, "sp_W2_JobCard_PartsDetail_DropDown", "ItemCode", "PartsDesc", dsParam, true, "--Select--", false, "");
                    dsJobCardParts = new DataSet();
                    dsJobCardParts = sysfuncs.FillDataSet("sp_W2_JobCard_PartsDetail_Select", dsParam);
                  //  if (dsJobCardParts.Tables[0].Rows.Count == 0) dsJobCardParts.Tables[0].Rows.Add(dsJobCardParts.Tables[0].NewRow());
                //    ViewState["Parts"] = dsJobCardParts.Tables[0];
                   // ViewState["ddlParts"] = ddlParts.SelectedValue;

                    //gvJobCardParts.DataSource = dsJobCardParts; gvJobCardParts.DataBind();

                    dsJobCardLub = new DataSet();
                    dsJobCardLub = sysfuncs.FillDataSet("sp_W2_JobCard_LubricanteDetail_Select", dsParam);
                //    if (dsJobCardLub.Tables[0].Rows.Count == 0) dsJobCardLub.Tables[0].Rows.Add(dsJobCardLub.Tables[0].NewRow());
                  //  ViewState["Lub"] = dsJobCardLub.Tables[0]; gvLubParts.DataSource = dsJobCardLub; gvLubParts.DataBind();

                    dsJobCardSublet = new DataSet();
                    dsJobCardSublet = sysfuncs.FillDataSet("sp_W2_JobCard_Sublet_Select", dsParam);
                   // if (dsJobCardSublet.Tables[0].Rows.Count == 0) dsJobCardSublet.Tables[0].Rows.Add(dsJobCardSublet.Tables[0].NewRow());
                    ViewState["SubLet"] = dsJobCardSublet.Tables[0]; gvSublet.DataSource = dsJobCardSublet; gvSublet.DataBind();


                }
            }
            catch (Exception ex)
            {
                sysfuncs.UserMsg(lblMsg, Color.Red, ex.Message);
            }
        }


        protected void btnLookup_Click(object sender, EventArgs e)
        {
            ViewState["lookupid"] = 10; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_Parts(imgLookup, ViewState["lookupid"].ToString(), "", "../../../");

            btnValue = 1;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }

        protected void imgLookup1_Click(object sender, EventArgs e)
        {
            ViewState["lookupid"] = 10; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_Parts(imgLookup, ViewState["lookupid"].ToString(), "", "../../../../");

            btnValue = 2;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }
        protected void imgLookup2_Click(object sender, EventArgs e)
        {
            ViewState["lookupid"] = 10; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_Parts(imgLookup, ViewState["lookupid"].ToString(), "", "../../../../");

            btnValue = 3;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }
        protected void imgLookup3_Click(object sender, EventArgs e)
        {
            ViewState["lookupid"] = 10; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_Parts(imgLookup, ViewState["lookupid"].ToString(), "", "../../../../");

            btnValue = 4;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }
        protected void imgLookup4_Click(object sender, EventArgs e)
        {
            ViewState["lookupid"] = 10; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_Parts(imgLookup, ViewState["lookupid"].ToString(), "", "../../../../");

            btnValue = 5;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }

       

        protected void ImageButtonRoNO_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 4; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_DTR_JobCardNo(imgLookup, ViewState["lookupid"].ToString(), "", "../../../../");

            btnValue = 6;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2538", "005"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (ddlDRNo.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select DTR No first";
                return;
            }
            else {
                if(ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    //and CountryDesc = '" + txtStateDescription.Text + "' and VersionCode = '" + dsdelete.Tables[0].Rows[rowGvIndex]["Version"].ToString() + "'

                    string sql = "Update FIRMaster Set PostFlag='Y',ApprovalFlag='N',PostDate='" + sysfunc.SaveDate(txtPostDate.Text) + "' where FIRCode='" + ddlDRNo.SelectedValue.ToString().Trim() + "' and DealerCode='" + Session["DealerCode"].ToString() + "'";
                    SqlDataReader reader = null;
                    sysfunc.ExecuteQuery(sql, ref reader);
                }
                ObjTrans.CommittTransaction(ref Trans);
                sysfunc.UserMsg(lblMsg, Color.Green, "DTR Post Successfully: YOu Can Check Status Later On Print: " + HFJobCard.Value);
                clearAll();
               
               // MessageBox.Show("DTR Post Successfully: YOu Can Check Status Later On Print", "Message");
            


            }
        }
    }
}