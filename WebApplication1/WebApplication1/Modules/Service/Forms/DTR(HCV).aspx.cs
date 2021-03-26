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

namespace DXBMS.Modules.Service.Forms
{
    public partial class DTR_HCV_ : System.Web.UI.Page
    {
        SysFunction sysfunc = new SysFunction();
        SysFunctions sysfuncs = new SysFunctions();
        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;
        DataSet ds;
        clsLookUp clslook = new clsLookUp();
        static int btnValue = 0;
        string p;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

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
                Session["LookUpData"] = string.Empty;
                ThreadStart childthreat = new ThreadStart(Load_ddlParts);
                Thread child = new Thread(childthreat);

                child.Start();

                Load_ddlVehRegNo();

                txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                string WhereClause = "DealerCode = '" + Session["DealerCode"].ToString() + "'";

                string[] Columns = new string[] { "FIRCode", "ChassisNo", "RegNo", "EngineNo" };
                sysfunc.GetMultiColumnsDDL(ddlDRNo, Columns, "FIRMaster ", WhereClause, "FIRCode", "", false, false);
                //  Session["Sketch"] = "";
            }
            // Session["Sketch"] = "";

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
                if (Session["Parts"] == null)
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
            lblMsg.Text = "";
        }

        protected void btnSaveJC_Click(object sender, EventArgs e)
        {

            try
            {
                if (ddlVinNo.SelectedIndex == 0)
                {
                    lblMsg.Text = "Please select VIN No first";
                    return;
                }
                if (txtFailedPart.Text.Trim() == "")
                {
                    lblMsg.Text = "Please select Failed part first";
                    return;
                }

                TextBox[] textBoxes = { txtRegNo, txtKM, txtSubject, txtEngineNo, txtInspDate };
                if (!MasterValidation(textBoxes)) return;
                else
                {
                    JobCardEntry();
                }
            }
            catch (Exception ex)
            {

                sysfunc.UserMsg(lblMsg, Color.Red, ex.Message, txtSRNo);
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
           /*12*/ new SqlParameter("@Defect",SqlDbType.VarChar,100),
           /*13*/ new SqlParameter("@Problem",SqlDbType.VarChar,100),
           /*14*/ new SqlParameter("@CorrectiveAction",SqlDbType.VarChar,100),           
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
           /*31*/ new SqlParameter("@SerialNo",SqlDbType.Char,8)

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
            else
            {
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
            if (Session["Sketch"].ToString() == null || Session["Sketch"].ToString().Trim() == "" || Session["Sketch"] == null)
                JobCardMaster_param[18].Value = "";
            else
                JobCardMaster_param[18].Value = Session["Sketch"].ToString().Trim();

            JobCardMaster_param[19].Value = txtPart1.Text.Trim();
            JobCardMaster_param[20].Value = txtPart2.Text.Trim();
            JobCardMaster_param[21].Value = txtPart3.Text.Trim();
            JobCardMaster_param[22].Value = txtPart4.Text.Trim();
            JobCardMaster_param[23].Value = sysfunc.SaveDate(txtInspDate.Text.Trim());
            JobCardMaster_param[24].Value = ddlStatus.SelectedValue.Trim();
            JobCardMaster_param[25].Value = ddlResult.SelectedValue.Trim();
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
                        sysfunc.ExecuteSP_NonQuery("[sp_FIRMaster_Insert]", JobCardMaster_param, Trans);
                    }
                    else
                    {

                        JobCardMaster_param[1].Value = ddlDRNo.SelectedValue.ToString().Trim();
                        HFJobCard.Value = JobCardMaster_param[1].Value.ToString();

                        sysfunc.ExecuteSP_NonQuery("[sp_UpdateFIRMasterFull]", JobCardMaster_param, Trans);
                    }


                    if (ObjTrans.CommittTransaction(ref Trans) == true)
                    {
                        sysfunc.UserMsg(lblMsg, Color.Green, "Record Saved Successfully: " + HFJobCard.Value);
                    }
                    else
                    {
                        ObjTrans.RollBackTransaction(ref Trans);
                        sysfunc.UserMsg(lblMsg, Color.Red, "Record not saved Try again. Or contact to support team ");
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

        protected void ddlDRNo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMasterData();
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
            txtAddress.Text = dt.Rows[0]["address"].ToString().Trim();
            txtPhnNo.Text = dt.Rows[0]["Phone1"].ToString().Trim();
            txtWarantyBookNo.Text = dt.Rows[0]["InvoiceNo"].ToString().Trim();
            txtDelDate.Text = dt.Rows[0]["DeliveryDate"].ToString().Trim();
            txtPart1.Text = dt.Rows[0]["Part1"].ToString().Trim();
            txtPart1Desc.Text = dt.Rows[0]["Part1D"].ToString().Trim();
            txtPart2.Text = dt.Rows[0]["Part2"].ToString().Trim();
            txtPart2Desc.Text = dt.Rows[0]["Part2D"].ToString().Trim();
            txtPart3.Text = dt.Rows[0]["Part3"].ToString().Trim();
            txtPart3Desc.Text = dt.Rows[0]["Part3D"].ToString().Trim();
            txtPart4.Text = dt.Rows[0]["Part4"].ToString().Trim();
            txtPart4Desc.Text = dt.Rows[0]["Part4D"].ToString().Trim();
            ddlStatus.SelectedValue = dt.Rows[0]["Status"].ToString();
            ddlResult.SelectedValue = dt.Rows[0]["Result"].ToString();
            txtRoNo.Text = dt.Rows[0]["RoNo"].ToString();
            txtRepairDate.Text = dt.Rows[0]["RoDate"].ToString();
            txtRequest.Text = dt.Rows[0]["Request"].ToString();
            txtFollowUpDate.Text = dt.Rows[0]["followupdate"].ToString();
            txtKeyNo.Text = dt.Rows[0]["KeyNo"].ToString();
            txtSerial.Text = dt.Rows[0]["SerialNo"].ToString();

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

            Data.DSReports data = new Data.DSReports();

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
            }

            RD.Load(Server.MapPath("~/Modules/Service/ServiceReports/rptDTR.rpt"));

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
            RD.DataDefinition.FormulaFields["Pic1"].Text = "'" + Server.MapPath("~") + dt.Rows[0]["item"].ToString() + "'";
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
        protected string SelectedPartDesc(string item)
        {
            try
            {
                DataTable dt = new DataTable();
                string sql = "Select ItemDesc from Item where ItemCode='" + dt.Rows[0]["Part1"].ToString().Trim() + "' and DealerCode='" + Session["DealerCode"].ToString() + "'";



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
                if (btnValue == 2)
                {
                    txtPart1.Text = item;
                    string sql = "Select ItemDesc from Item where ItemCode='" + item + "' and DealerCode='" + Session["DealerCode"].ToString() + "'";

                    DataTable dt = new DataTable();

                    dt = sysfunc.GetData(sql);

                    if (dt.Rows.Count > 0)
                    {

                        txtPart1Desc.Text = dt.Rows[0]["ItemDesc"].ToString();

                    }
                }
                if (btnValue == 3)
                {
                    txtPart2.Text = item;
                    string sql = "Select ItemDesc from Item where ItemCode='" + item + "' and DealerCode='" + Session["DealerCode"].ToString() + "'";

                    DataTable dt = new DataTable();

                    dt = sysfunc.GetData(sql);

                    if (dt.Rows.Count > 0)
                    {

                        txtPart2Desc.Text = dt.Rows[0]["ItemDesc"].ToString();

                    }
                }
                if (btnValue == 4)
                {
                    txtPart3.Text = item;
                    string sql = "Select ItemDesc from Item where ItemCode='" + item + "' and DealerCode='" + Session["DealerCode"].ToString() + "'";

                    DataTable dt = new DataTable();

                    dt = sysfunc.GetData(sql);

                    if (dt.Rows.Count > 0)
                    {

                        txtPart3Desc.Text = dt.Rows[0]["ItemDesc"].ToString();

                    }
                }
                if (btnValue == 5)
                {
                    txtPart4.Text = item;
                    string sql = "Select ItemDesc from Item where ItemCode='" + item + "' and DealerCode='" + Session["DealerCode"].ToString() + "'";

                    DataTable dt = new DataTable();

                    dt = sysfunc.GetData(sql);

                    if (dt.Rows.Count > 0)
                    {

                        txtPart4Desc.Text = dt.Rows[0]["ItemDesc"].ToString();

                    }
                }
                if (btnValue == 6)
                {
                    txtRoNo.Text = item;
                    string sql = "Select ItemDesc from Item where ItemCode='" + item + "' and DealerCode='" + Session["DealerCode"].ToString() + "'";

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
            ViewState["lookupid"] = 10; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_JobCardNo(imgLookup, ViewState["lookupid"].ToString(), "", "../../../../");

            btnValue = 6;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (ddlDRNo.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select DTR No first";
                return;
            }
            else
            {
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    //and CountryDesc = '" + txtStateDescription.Text + "' and VersionCode = '" + dsdelete.Tables[0].Rows[rowGvIndex]["Version"].ToString() + "'

                    string sql = "Update FIRMaster Set PostFlag='Y',ApprovalFlag='N',PostDate='" + sysfunc.SaveDate(txtPostDate.Text) + "' where FIRCode='" + ddlDRNo.SelectedValue.ToString().Trim() + "' and DealerCode='" + Session["DealerCode"].ToString() + "'";
                    SqlDataReader reader = null;
                    sysfunc.ExecuteQuery(sql, ref reader);
                }
                ObjTrans.CommittTransaction(ref Trans);

                lblMsg.Text = "DTR Posted";
                clearAll();


            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            HttpPostedFile postedFile = PicUpload.PostedFile;

            string imgName = PicUpload.FileName;
            string imgPath = "Images/" + imgName;

            int imgSize = PicUpload.PostedFile.ContentLength;

            string fileExtension = Path.GetExtension(imgName);

            if (fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".gif"
                || fileExtension.ToLower() == ".png" || fileExtension.ToLower() == ".bmp")
            {
                PicUpload.SaveAs(Server.MapPath("../../../" + imgPath));
                Session["Sketch"] = imgPath;
                p = Session["Sketch"].ToString();
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Only images (.jpg, .png, .gif and .bmp) can be uploaded";
            }

        }
    }
}