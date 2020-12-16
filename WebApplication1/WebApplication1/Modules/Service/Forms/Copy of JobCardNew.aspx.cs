using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Web.SessionState;
using CrystalDecisions.CrystalReports.Engine;
using CConn;
using Microsoft.ApplicationBlocks.Data;
using CrystalDecisions.Shared;


namespace DXBMS.Modules.Service
{
    public partial class JobCardNew : System.Web.UI.Page
    {
        SysFunctions myFunc = new SysFunctions();
        MainBLL objMBLL = new MainBLL();
        ServiceBL objSBL = new ServiceBL();
        //General grl = new General();
        SysFunctions grl = new SysFunctions();
        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;
        string apStr;
        DataTable JobDT, LubDT, SubletDT, PartsDT,ConPartsDT;
        DataSet dsJobCardDetail, dsJobCardParts, dsJobCardLub, dsJobCardSublet,dsJobCardConParts, ds;
        //clsLookUp clslook = new clsLookUp();
        decimal d1, d2, d3, d4;
        int ridx, updflag, countLabour, countParts, countlub, countlubRecQty, countPartsRecQty, countConPartsRecQty,countConParts;

        double totLabour, totParts, totlub, totsubletInv, totsubletPayable, totConParts;
        decimal deductAmount;
        bool search_result, search_item;

        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/Main/login.aspx");

            }
            if (ViewState["Parts"] != null) PartsDT = (DataTable)ViewState["Parts"];
            if (ViewState["Job"] != null) JobDT = (DataTable)ViewState["Job"];
            if (ViewState["Lub"] != null) LubDT = (DataTable)ViewState["Lub"];
            if (ViewState["SubLet"] != null) SubletDT = (DataTable)ViewState["SubLet"];
            if (ViewState["ConParts"] != null) ConPartsDT = (DataTable)ViewState["ConParts"];

            d1 = (ViewState["d1"] != null ? Convert.ToDecimal(ViewState["d1"].ToString()) : 0);
            d2 = (ViewState["d2"] != null ? Convert.ToDecimal(ViewState["d2"].ToString()) : 0);
            d3 = (ViewState["d3"] != null ? Convert.ToDecimal(ViewState["d3"].ToString()) : 0);
            d4 = (ViewState["d4"] != null ? Convert.ToDecimal(ViewState["d4"].ToString()) : 0);
            ridx = (ViewState["ridx"] != null ? int.Parse(ViewState["ridx"].ToString()) : -1);
            //deductAmount = (ViewState["deductAmount"] != null ? decimal.Parse(ViewState["deductAmount"].ToString()) : 0);
            //Session["DealerCode"] = "00001";
            //GlobalVar.mDealerCode = "00001";
            //Session["UserName"] = "azhar";
            if (!Page.IsPostBack)
            {
                setInitialDates();            //-------------------------------------SUBLET
                createSubletDT();            //-------------------------------------LUBRICANT
                createLubDT();              //------------------------------------------PARTS            
                createPartsDT();            //-----------------------------------------JOB
                createJobDT();
                createConPartsDT();
                Load_ddlJobCardCode();
                Load_ddlVehRegNo();
                Load_ddlJobs(ddlJobs_Labor);
                Load_ddlJobs(ddlJobs_Sublet);
                Load_ddlEstNo();
                myFunc.Load_CounterSalesItem_ddl(ddlPartCode,Session["DealerCode"].ToString(),"P");
                myFunc.Load_CounterSalesItem_ddl(ddlLubCode, Session["DealerCode"].ToString(), "L");
                myFunc.Load_CounterSalesItem_ddl(ddlConsumableParts, Session["DealerCode"].ToString(), "P");
                this.DataBind();
                DataSet ds = new DataSet();
                SqlParameter[] JobCardMaster_param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5)};
                JobCardMaster_param[0].Value = Session["DealerCode"].ToString();
                //myFunc.FillDrp_SP(ddlAdvisorCode, "sp_Get_Advisor", "EmpCode", "EmpName", JobCardMaster_param, true, "--Select--", false, "");
                objMBLL.FillDrp_SP(ddlAdvisorCode, "sp_Get_Advisor", "EmpCode", "EmpName", JobCardMaster_param, true, "Select", false, "");
                //objMBLL.FillDrp_SP(ddlAdvisor, "sp_Get_Advisor", "EmpCode", "EmpName", JobCardMaster_param, true, "--Select--", false, "");
                objMBLL.FillDrp_SP(ddlTechnicianEmpCode, "sp_2W_DealerEmp_Technician_Select", "EmpCode", "EmpName", JobCardMaster_param, true, "--Select--", false, "");
                //objMBLL.FillDrp_SP(ddlTechnician, "sp_2W_DealerEmp_Technician_Select", "EmpCode", "EmpName", JobCardMaster_param, true, "--Select--", false, "");
                objMBLL.FillDrp_SP(ddlVendorCode, "sp_2W_Vendor_Select", "VendorCode", "VendorDesc", null, true, "--Select--", false, "");
                objMBLL.FillDropDown(ddlJobCardTypeCode, "SELECT JobTypeCode,JobTypeDesc FROM JobTypeMaster Where DealerCode IN('" + Session["DealerCode"].ToString() + "', 'COMON') ", "JobTypeDesc", "JobTypeCode", "Select");
                //objMBLL.FillDrp_SP(ddlVendor, "sp_2W_Vendor_Select", "VendorCode", "VendorDesc", null, true, "--Select--", false, "");
                //objMBLL.FillDrp_SP(ddlSchedule , "sp_2W_MaintainenceSchedule_Select", "KM", "Prod", null, true, "--Select--", false, "");
                //myFunc.ExecuteQuery("select (JobTypeCode + ' --- ' +  JobTypeDesc)  ColName,JobTypeCode ColValue from JobTypeMaster ", ref ds);
                //ddlJobType.DataSource = ds.Tables[0];
                //ddlJobType.DataTextField = "ColName";
                //ddlJobType.DataValueField = "ColValue";
                //ddlJobType.DataBind();

                txtDep.ReadOnly = true;
                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());

                //ddlJobCardNo.Items.Add("New");
                //ddlJobCardNo.Text = "New";

                //ddlJobCardType.Items.Add("--Select--");
                //ddlJobCardType.Text = "--Select--";
                //ddlPaymentMode.Items.Add("--Select--");
                //ddlPaymentMode.Text = "--Select--";
                //ddlBilling.Items.Add("--Select--");
                //ddlBilling.Text = "--Select--";

                //ddlEngineNo.Items.Add("--Select--");
                //ddlEngineNo.Text = "--Select--";
                //ddlChassisNo.Items.Add("--Select--");
                //ddlChassisNo.Text = "--Select--";
                //ddlRegNo.Items.Add("--Select--");
                //ddlRegNo.Text = "--Select--";
                //TabContainer1.Enabled = false;
                TabContainer1.ActiveTabIndex = 0;

            }
            //if (Session["LookUpData"] != null)
            //{
            //    HFBlank.Value = HFRegNo.Value = HFEngineNo.Value = HFChassisNo.Value = HFCustomer.Value = HFJobCard.Value = null;
            //    lookUptext(ViewState["lookupid"].ToString());

            //    if (ViewState["lookupid"].ToString() == "4" | ViewState["lookupid"].ToString() == "1") LoadMasterData();

            //    if (ViewState["lookupid"].ToString() == "3")
            //    {
            //        LoadVehInfo();
            //    }
            //    if (ViewState["lookupid"].ToString() == "10")
            //    {
            //        if (ddlJobCardTypeCode.SelectedItem.Text == "Insurance")
            //        {
            //            txtDep.ReadOnly = false;
            //            txtDep.Text = "0";
            //        }
            //        else
            //        {
            //            txtDep.ReadOnly = true;
            //            txtDep.Text = "";
            //        }
            //    }
            //}

            //if (Session["SubletJobDesc"] != null)
            //{
            //    txtSubletJobDesc.Text = Session["SubletJobDesc"].ToString();
            //}
        }


        private void Load_ddlJobCardCode()
        {
            string WhereClause = "DealerCode = '" + Session["DealerCode"].ToString() + "'  " +
                "And DelFlag='N' and GatePass=''  ";

            string[] Columns = new string[] { "JobCardCode", "RegNo", "UserName" };
            myFunc.GetMultiColumnsDDL(ddlJobCardCode, Columns, "JobCardMaster ", WhereClause, "JobCardCode", " Order by JobCardCode Desc ", false, false);
        }
        private void Load_ddlEstNo()
        {
            string WhereClause = "DealerCode = '" + Session["DealerCode"].ToString() + "'  " +
                "And DelFlag='N'  ";

            string[] Columns = new string[] { "CustomerEstimateCode", "CONVERT(VARCHAR(10),tdDate,105)","RegNo", "UserName" };
            myFunc.GetMultiColumnsDDL(ddlEstNo, Columns, "CustomerEstimateMaster ", WhereClause, "CustomerEstimateCode", " Order by CustomerEstimateCode Desc ", false, false);
        }
        
        private void Load_ddlVehRegNo()
        {
            string WhereClause = "DealerCode IN ('" + Session["DealerCode"].ToString() + "','COMON')  ";
                

            string[] Columns = new string[] { "RegNo", "ChassisNo", "EngineNo" };
            myFunc.GetMultiColumnsDDL(ddlVehRegNo, Columns, "CustomerVehicle ", WhereClause, "RegNo", "", false, false);
        }
        private void Load_ddlJobs(DropDownList ddlJobs)
        {
            string WhereClause = "DJ.DealerCode = JC.DealerCode " +
"And DJ.JobCatCode = JC.JobCatCode";

            string[] Columns = new string[] { "DJ.DefJobCode", "DJ.DefJobDesc", "JC.JobCatDesc" };
            myFunc.GetMultiColumnsDDL(ddlJobs, Columns, "DefaultJob DJ, JobCategory JC ", WhereClause, "DefJobCode", "", false, false);
        }
        private void setInitialDates()
        {
            txtPromisedDate.Text = txtDeliveryDate.Text = txtDeliveryDate.Text = txtRecieptDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtPromistedTime.Text = txtDeliveryTime.Text = txtReciptTime.Text = DateTime.Now.ToString("HH:mm");
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
            //SubletDT.Rows.Add(new object[] { null, null, null, null, null, null, null, null,null});  
            gvSublet.DataSource = SubletDT;
            gvSublet.DataBind();
            ViewState["SubLet"] = SubletDT;
        }
        private void createLubDT()
        {
            LubDT = new DataTable();
            LubDT.Columns.Add(new DataColumn("ID", typeof(int)));
            LubDT.Columns.Add(new DataColumn("PartNo", typeof(string)));
            LubDT.Columns.Add(new DataColumn("PartsDesc", typeof(string)));
            LubDT.Columns.Add(new DataColumn("Qty", typeof(string)));
            LubDT.Columns.Add(new DataColumn("Price", typeof(string)));
            LubDT.Columns.Add(new DataColumn("Total", typeof(string)));
            LubDT.Columns.Add(new DataColumn("ItemCode", typeof(string)));
            LubDT.Columns.Add(new DataColumn("RecQty", typeof(string)));
            //LubDT.Columns.Add(new DataColumn("Type", typeof(string)));

            //LubDT.Rows.Add(new object[] { null, null, null, null, null, null,null });
            gvLubParts.DataSource = LubDT; gvLubParts.DataBind(); ViewState["Lub"] = LubDT;
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
            //PartsDT.Rows.Add(new object[] { null, null, null, null, null, null, null, null, null,null });                        
            gvJobCardParts.DataSource = PartsDT; gvJobCardParts.DataBind(); ViewState["Parts"] = PartsDT;
        }
        private void createJobDT()
        {

            //  SELECT Distinct 1 as ID,a.[DealerCode]
            //,[JobCardCode]
            //,[JobCode]
            //,a.[EmpCode]
            //,[Amount]
            //,[Type]
            //,[JobRemarks]
            //,EmpName
            //,DefJobDesc



            JobDT = new DataTable();
            JobDT.Columns.Add(new DataColumn("ID", typeof(int)));
            JobDT.Columns.Add(new DataColumn("JobCode", typeof(string)));
            JobDT.Columns.Add(new DataColumn("DefJobDesc", typeof(string)));
            JobDT.Columns.Add(new DataColumn("JobRemarks", typeof(string)));
            JobDT.Columns.Add(new DataColumn("EmpCode", typeof(string)));
            JobDT.Columns.Add(new DataColumn("EmpName", typeof(string)));
            JobDT.Columns.Add(new DataColumn("Amount", typeof(string)));
            JobDT.Columns.Add(new DataColumn("Type", typeof(string)));
            //JobDT.Rows.Add(new object[] { null, null, null, null, null, null });
            gvJobCard.DataSource = JobDT; gvJobCard.DataBind(); ViewState["Job"] = JobDT;


            //r["ID"] = JobDT.Rows.Count + 1;
            //dr["JobCode"] = job;
            //dr["JobDesc"] = jobDesc;
            //dr["JobRemarks"] = jobremarks;
            //dr["EmpCode"] = technician;
            //dr["EmpName"] = EmpName;
            //dr["Amount"] = amount;
            //dr["Type"] = type;

        }
        private void createConPartsDT()
        {
            ConPartsDT = new DataTable();
            ConPartsDT.Columns.Add(new DataColumn("ID", typeof(int)));
            ConPartsDT.Columns.Add(new DataColumn("PartNo", typeof(string)));
            ConPartsDT.Columns.Add(new DataColumn("PartsDesc", typeof(string)));
            ConPartsDT.Columns.Add(new DataColumn("Dep", typeof(string)));
            ConPartsDT.Columns.Add(new DataColumn("DepAmount", typeof(string)));
            ConPartsDT.Columns.Add(new DataColumn("Qty", typeof(string)));
            ConPartsDT.Columns.Add(new DataColumn("Price", typeof(string)));
            ConPartsDT.Columns.Add(new DataColumn("Total", typeof(string)));
            ConPartsDT.Columns.Add(new DataColumn("Type", typeof(string)));
            ConPartsDT.Columns.Add(new DataColumn("ItemCode", typeof(string)));
            ConPartsDT.Columns.Add(new DataColumn("RecQty", typeof(string)));
            //PartsDT.Rows.Add(new object[] { null, null, null, null, null, null, null, null, null,null });                        
            gvJobCardConParts.DataSource = ConPartsDT;
            gvJobCardConParts.DataBind();
            ViewState["ConParts"] = ConPartsDT;
        }


        private void lookUptext(string lookUpID)
        {

            //switch (lookUpID)
            //{
            //    case "1":
            //        // Customer | Customer Description
            //        if (ViewState["txtgroup"].ToString() == "1") { GetLookUpsValue(ref txtCustomer, ref txtCustomerDesc, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ref HFCustomer); } else if (ViewState["txtgroup"].ToString() == "2") { GetLookUpsValue(ref txtEndUser, ref txtEndUserDesc, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ref HFBlank); }
            //        break;
            //    case "2":
            //        // End User | End User Description
            //        GetLookUpsValue(ref txtEndUser, ref txtEndUserDesc, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ref HFBlank);
            //        break;
            //    case "3":
            //        // Chassis No | RegNo | Engine No
            //        if (ViewState["txtgroup"].ToString() == "1") { GetLookUpsValue(ref txtRegNo, ref txtEngineNo, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ref HFChassisNo); } else if (ViewState["txtgroup"].ToString() == "2") { GetLookUpsValue(ref txtEngineNo, ref txtEngineNo, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ref HFEngineNo); } else if (ViewState["txtgroup"].ToString() == "3") { GetLookUpsValue(ref txtRegNo, ref txtRegNo, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ref HFRegNo); }
            //        break;
            //    case "4":
            //        // JobCardNo
            //        GetLookUpsValue(ref txtJobCardNo, ref txtJobCardNo, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ref HFJobCard);
            //        break;
            //    case "10":
            //        if (ViewState["txtgroup"].ToString() == "1")
            //        {
            //            GetLookUpsValue(ref txtItemcodeParts, ref txtPartCode, ref txtPartDesc, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ViewState["ixd3"].ToString());
            //            string whereclause = " itemcode = '" + txtItemcodeParts.Text + "' and DealerCode IN('AAAAA','" + Session["DealerCode"].ToString() + "')";
            //            txtPartPrice.Text = grl.GetStringValuesAgainstCodes(whereclause, "Item", "SaleRate");
            //        }
            //        else if (ViewState["txtgroup"].ToString() == "2")
            //        {
            //            GetLookUpsValue(ref txtItemcodeLub, ref txtLubPartCode, ref txtLubPartDesc, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ViewState["ixd3"].ToString());
            //            string whereclause = " itemcode = '" + txtItemcodeLub.Text + "' and DealerCode IN('AAAAA','" + Session["DealerCode"].ToString() + "')";
            //            txtLubPrice.Text = grl.GetStringValuesAgainstCodes(whereclause, "Item", "SaleRate");
            //        }
            //        break;
            //    //case "17" :
            //    //    // JobCard Type | JobCart Description
            //    //    GetLookUpsValue(ref txtJobType, ref txtJobTypeDesc, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ref HFBlank);
            //    //    break;
            //    case "18":
            //        // Schedule KM | Product | Version
            //        GetLookUpsValue(ref txtSchedual, ref txtSchedual, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ref HFBlank);
            //        break;
            //    //case "19":
            //    //    // Advisor | Advisor Description
            //    //    // if (ViewState["txtgroup"].ToString() == "1") { GetLookUpsValue(ref txtAdvisor, ref txtAdvisorDesc, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ref HFBlank); }
            //    //    if (ViewState["txtgroup"].ToString() == "2")
            //    //    {
            //    //        GetLookUpsValue(ref txtTechnician, ref txtTechnicianDesc, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ref HFBlank);
            //    //        string whereclause = " DefJobCode ='" + txtJob.Text + "' and ProdCode='" + txtProduct.Text + "' and VersionCode = '" + txtVersion.Text + "' and DealerCode IN('AAAAA','" + Session["DealerCode"].ToString() + "')";
            //    //        txtLabor.Text = grl.GetStringValuesAgainstCodes(whereclause, "WithOutWarrantyRate", "WithOutWarrantyRate");
            //    //    }
            //    //    break;
            //    case "21":
            //        // Job | Job Description
            //        if (ViewState["txtgroup"].ToString() == "1")
            //        {
            //            GetLookUpsValue(ref txtJob, ref txtJobDesc, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ref HFBlank);
            //            string whereclause = " DefJobCode ='" + txtJob.Text + "' and ProdCode='" + txtProduct.Text + "' and VersionCode = '" + txtVersion.Text + "' and DealerCode IN('AAAAA','" + Session["DealerCode"].ToString() + "')";
            //            txtLabor.Text = grl.GetStringValuesAgainstCodes(whereclause, "WithOutWarrantyRate", "WithOutWarrantyRate");
            //        }
            //        else if (ViewState["txtgroup"].ToString() == "2")
            //        {
            //            GetLookUpsValue(ref txtSubletJob, ref txtSubletJobDesc, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ref HFBlank);
            //            string whereclause = " DefJobCode ='" + txtJob.Text + "' and ProdCode='" + txtProduct.Text + "' and VersionCode = '" + txtVersion.Text + "' and DealerCode IN('AAAAA','" + Session["DealerCode"].ToString() + "')";
            //            txtLabor.Text = grl.GetStringValuesAgainstCodes(whereclause, "WithOutWarrantyRate", "WithOutWarrantyRate");
            //        }
            //        break;
            //    //case "8":
            //    //    // Vendor | Vendor Description
            //    //    GetLookUpsValue(ref txtSubletVendor, ref txtSubletVendorDesc, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ref HFBlank);
            //    //    break;
            //    //case "23":
            //    //    // Color | Color Description                
            //    //    GetLookUpsValue(ref txtSubletVendor, ref txtSubletVendorDesc, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ref HFBlank);
            //    //    break;
            //    case "":
            //        // Schedule KM | Product | Version
            //        GetLookUpsValue(ref txtSchedual, ref txtSchedual, ViewState["ixd1"].ToString(), ViewState["ixd2"].ToString(), ref HFBlank);
            //        break;



            //    default:
            //        break;
            //}
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

        protected void LoadVehInfo()
        {
            DataSet dsVehInfo = new DataSet();
            SqlParameter[] param = {                                 
                                    new SqlParameter("@DealerCode",SqlDbType.VarChar,5), 
                                    new SqlParameter("@RegNo",SqlDbType.VarChar,30)
                               };


            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = ddlVehRegNo.SelectedValue.ToString().Trim(); // ddlRegNo.Text.Trim();//(HFRegNo.Value == "" ? null : HFRegNo.Value);
            //if (HFJobCard.Value == "" & HFRegNo.Value == "" & HFEngineNo.Value == "" & HFChassisNo.Value == ""
            //    & HFCustomer.Value == "") param[1].Value = "0";
            //else dsMasterParam[1].Value = HFChassisNo.Value;
            ////param[1].Value = (HFChassisNo.Value == "" ? null : HFChassisNo.Value); 
            //param[2].Value = (HFEngineNo.Value == "" ? null : HFEngineNo.Value);
            //param[3].Value = null; param[4].Value = null; param[5].Value = null;
            //param[6].Value = (HFRegNo.Value == "" ? null : HFRegNo.Value);
            //param[6].Value = null;           

            dsVehInfo = myFunc.FillDataSet("sp_W2_CustomerVehicle_Select", param);
            if (dsVehInfo.Tables[0].Rows.Count > 0) setVehcileInfo(dsVehInfo);
        }
        protected void LoadMasterData()
        {
            SqlParameter[] dsParam = {                                                       
                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                                 };

            dsParam[0].Value = Session["DealerCode"].ToString();
            dsParam[1].Value = (ddlJobCardCode.SelectedValue.ToString().Trim() == "" ? null : ddlJobCardCode.SelectedValue.ToString().Trim());

            DataTable dtjc = new DataTable();
            string sqlj = "select * from JobCardMaster where JobCardCode='" + dsParam[1].Value.ToString() + "' and DealerCode='" + Session["DealerCode"].ToString()+"'";
            dtjc = myFunc.GetData(sqlj);
            if (dtjc.Rows.Count == 0)
            {
                
                btnClearJC_Click(btnClearJC, EventArgs.Empty);
                lblMsg.Text = "Error opening JobCard please contact concenrn person";
                return;
                
            }

            HFJobCard.Value = ddlJobCardCode.SelectedValue.ToString().Trim();
            HFRegNo.Value = dtjc.Rows[0]["RegNo"].ToString();
            HFEngineNo.Value = dtjc.Rows[0]["EngineNo"].ToString();
            HFChassisNo.Value = dtjc.Rows[0]["ChassisNo"].ToString();
            HFCustomer.Value = dtjc.Rows[0]["CusCode"].ToString();

            ddlVehRegNo.SelectedValue = dtjc.Rows[0]["RegNo"].ToString();
            

            dtjc.Dispose();

            SqlParameter[] dsMasterParam = {                                                       
                                            new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                            new SqlParameter("@JobCardCode",SqlDbType.Char,8),
                                            new SqlParameter("@RegNo",SqlDbType.VarChar,50),
                                            new SqlParameter("@EngineNo",SqlDbType.VarChar,50),
                                            new SqlParameter("@ChassisNo",SqlDbType.VarChar,50),
                                            new SqlParameter("@CusCode",SqlDbType.VarChar,50)
                                       };

            dsMasterParam[0].Value = Session["DealerCode"].ToString();
            dsMasterParam[1].Value = (HFJobCard.Value == "" ? null : HFJobCard.Value);
            dsMasterParam[2].Value = (HFRegNo.Value == "" ? null : HFRegNo.Value);
            dsMasterParam[3].Value = (HFEngineNo.Value == "" ? null : HFEngineNo.Value);
            if (HFJobCard.Value == "" & HFRegNo.Value == "" & HFEngineNo.Value == "" & HFChassisNo.Value == ""
                & HFCustomer.Value == "") dsMasterParam[4].Value = "0";
            else dsMasterParam[4].Value = HFChassisNo.Value;

            dsMasterParam[5].Value = (HFCustomer.Value == "" ? null : HFCustomer.Value);

            DataSet dsJobCardMaster = new DataSet();
            dsJobCardMaster = myFunc.FillDataSet("sp_W2_JobCard_Master_Select", dsMasterParam);
            if (ddlJobCardTypeCode.SelectedItem.Text.Trim() == "Insurance")
            {
                txtDep.ReadOnly = false;
                txtDep.Text = "0";
            }
            else
            {
                txtDep.ReadOnly = true;
                txtDep.Text = "";
            }

            setVehcileInfo(dsJobCardMaster);
            setJobCardInfo(dsJobCardMaster);


            dsJobCardDetail = new DataSet();
            dsJobCardDetail = myFunc.FillDataSet("sp_W2_JobCard_Detail_Select", dsParam);
            //if (dsJobCardDetail.Tables[0].Rows.Count == 0) dsJobCardDetail.Tables[0].Rows.Add(dsJobCardDetail.Tables[0].NewRow());
            ViewState["Job"] = dsJobCardDetail.Tables[0]; gvJobCard.DataSource = dsJobCardDetail; gvJobCard.DataBind();

            dsJobCardParts = new DataSet();
            dsJobCardParts = myFunc.FillDataSet("sp_W2_JobCard_PartsDetail_Select", dsParam);
            //if (dsJobCardParts.Tables[0].Rows.Count == 0) dsJobCardParts.Tables[0].Rows.Add(dsJobCardParts.Tables[0].NewRow());
            ViewState["Parts"] = dsJobCardParts.Tables[0]; gvJobCardParts.DataSource = dsJobCardParts; gvJobCardParts.DataBind();

            dsJobCardLub = new DataSet();
            dsJobCardLub = myFunc.FillDataSet("sp_W2_JobCard_LubricanteDetail_Select", dsParam);
            //if (dsJobCardLub.Tables[0].Rows.Count == 0) dsJobCardLub.Tables[0].Rows.Add(dsJobCardLub.Tables[0].NewRow());
            ViewState["Lub"] = dsJobCardLub.Tables[0]; gvLubParts.DataSource = dsJobCardLub; gvLubParts.DataBind();

            dsJobCardSublet = new DataSet();
            dsJobCardSublet = myFunc.FillDataSet("sp_W2_JobCard_Sublet_Select", dsParam);
            //if (dsJobCardSublet.Tables[0].Rows.Count == 0) dsJobCardSublet.Tables[0].Rows.Add(dsJobCardSublet.Tables[0].NewRow());
            ViewState["SubLet"] = dsJobCardSublet.Tables[0]; gvSublet.DataSource = dsJobCardSublet; gvSublet.DataBind();

            dsJobCardConParts = new DataSet();
            dsJobCardConParts = myFunc.FillDataSet("sp_W2_JobCard_ConPartsDetail_Select", dsParam);
            //if (dsJobCardConParts.Tables[0].Rows.Count == 0) dsJobCardConParts.Tables[0].Rows.Add(dsJobCardConParts.Tables[0].NewRow());
            ViewState["ConParts"] = dsJobCardConParts.Tables[0]; gvJobCardConParts.DataSource = dsJobCardConParts; gvJobCardConParts.DataBind();
        }

        private void setVehcileInfo(DataSet dsJobCardMaster)
        {
            //ddlRegNo.Text = dsJobCardMaster.Tables[0].Rows[0]["RegNo"].ToString();
            txtChassisNo.Text = dsJobCardMaster.Tables[0].Rows[0]["ChassisNo"].ToString();
            txtEngineNo.Text = dsJobCardMaster.Tables[0].Rows[0]["EngineNo"].ToString();
            txtCustomer.Text = dsJobCardMaster.Tables[0].Rows[0]["CusCode"].ToString();
            txtCustomerDesc.Text = grl.GetStringValuesAgainstCodes("cusCode", txtCustomer.Text, "CusDesc+' '+FatherHusName", "Customer");
            txtEndUser.Text = dsJobCardMaster.Tables[0].Rows[0]["CusCode"].ToString();
            txtEndUserDesc.Text = grl.GetStringValuesAgainstCodes("cusCode", txtEndUser.Text, "CusDesc+' '+FatherHusName", "Customer");
            txtBrand.Text = dsJobCardMaster.Tables[0].Rows[0]["BrandCode"].ToString();
            txtBrandDesc.Text = grl.GetStringValuesAgainstCodes("BrandCode", txtBrand.Text, "BrandDesc", "Brand");
            txtProduct.Text = dsJobCardMaster.Tables[0].Rows[0]["ProdCode"].ToString();
            txtVersion.Text = dsJobCardMaster.Tables[0].Rows[0]["VersionCode"].ToString();
            SqlParameter[] param_Schedule = {                                                       
                                                                new SqlParameter("@ProdCode",SqlDbType.VarChar,10),
                                                                new SqlParameter("@VersionCode",SqlDbType.Char,3)
                                                              };
            param_Schedule[0].Value = txtProduct.Text.Trim();
            param_Schedule[1].Value = txtVersion.Text.Trim();
            objMBLL.FillDrp_SP(ddlScheduleJC, "sp_2W_MaintainenceSchedule_Select", "KM", "KM", param_Schedule, true, "Select", false, "");
            
            if (dsJobCardMaster.Tables[0].Columns.Contains("SIRMaster"))
            {
                txtSIRMasterId.Text = dsJobCardMaster.Tables[0].Rows[0]["SIRMaster"].ToString();
            }

            //txtVersionDesc.Text = grl.GetStringValuesAgainstCodes("VersionCode", txtVersion.Text, "ProdDesc", "Vehicle");
            txtVersionDesc.Text = grl.GetStringValuesAgainstCodes("ProdCode", txtProduct.Text, "ProdDesc", "Vehicle");
            txtColor.Text = grl.GetStringValuesAgainstCodes("ColorCode", grl.GetStringValuesAgainstCodes("RegNo", ddlVehRegNo.SelectedValue.ToString().Trim(), "ColorCode", "CustomerVehicle"), "ColorDesc", "Color");
        }

        private void setJobCardInfo(DataSet dsJobCardMaster)
        {
            // txtGatePass.Text = ddlJobCardNo.Text.Trim();
            if (ddlJobCardCode.SelectedIndex != 0)
            {
                ddlJobCardTypeCode.SelectedValue= dsJobCardMaster.Tables[0].Rows[0]["JobCardType"].ToString().Trim();
                //txtJobType.Text = dsJobCardMaster.Tables[0].Rows[0]["JobTypeCode"].ToString();
                //txtJobTypeDesc.Text = grl.GetStringValuesAgainstCodes("jobTypeCode", txtJobType.Text, "JobTypeDesc", "JobTypeMaster");
                //ddlJobType.Value = dsJobCardMaster.Tables[0].Rows[0]["JobTypeCode"].ToString();
                txtRemarks.Text = dsJobCardMaster.Tables[0].Rows[0]["Remarks"].ToString();
                txtKM.Text = dsJobCardMaster.Tables[0].Rows[0]["KM"].ToString();

                txtRecieptDate.Text = Convert.ToDateTime(dsJobCardMaster.Tables[0].Rows[0]["RecDate"].ToString()).ToString("dd/MM/yyyy");//(grl.GetDate(dsJobCardMaster.Tables[0].Rows[0]["RecDate"].ToString())).ToString();
                txtReciptTime.Text = Convert.ToDateTime(dsJobCardMaster.Tables[0].Rows[0]["RecTime"].ToString()).ToString("HH:mm"); //grl.GetTime(dsJobCardMaster.Tables[0].Rows[0]["RecTime"].ToString()).ToString();
                txtDeliveryDate.Text = Convert.ToDateTime(dsJobCardMaster.Tables[0].Rows[0]["DelvDate"].ToString()).ToString("dd/MM/yyyy");//(grl.GetDate(dsJobCardMaster.Tables[0].Rows[0]["DelvDate"].ToString())).ToString();
                txtDeliveryTime.Text = Convert.ToDateTime(dsJobCardMaster.Tables[0].Rows[0]["DelvTime"].ToString()).ToString("HH:mm");// grl.GetTime(dsJobCardMaster.Tables[0].Rows[0]["DelvTime"].ToString()).ToString();
                txtPromistedTime.Text = dsJobCardMaster.Tables[0].Rows[0]["PromiseTime"].ToString() == "" ? "" : Convert.ToDateTime(dsJobCardMaster.Tables[0].Rows[0]["PromiseTime"].ToString()).ToString("HH:mm");
                ddlStatusJC.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["JobStatus"].ToString().Trim();
                string a = dsJobCardMaster.Tables[0].Rows[0]["promiseDate"].ToString().Trim();
                txtPromisedDate.Text = (a == "" ? DateTime.Now.ToString("dd-MM-yyyy") : Convert.ToDateTime(a).ToString("dd-MM-yyyy"));

                lblJobCardTotal.Text = dsJobCardMaster.Tables[0].Rows[0]["JobsTotal"].ToString().Trim();
                lblPartsTotal.Text = dsJobCardMaster.Tables[0].Rows[0]["PartsTotal"].ToString().Trim();
                txtLubTotal.Text = dsJobCardMaster.Tables[0].Rows[0]["LubPartsTotal"].ToString().Trim();
                txtSubletTotal.Text = dsJobCardMaster.Tables[0].Rows[0]["SubletTotal"].ToString().Trim();
                ddlAdvisorCode.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["Advisor"].ToString().Trim();
                lblSIRMaster.Text = dsJobCardMaster.Tables[0].Rows[0]["SIRMaster"].ToString().Trim();
                //grl = new General();
                //txtAdvisorDesc.Text = grl.GetStringValuesAgainstCodes("EMPCODE", ddlAdvisor.SelectedItem .Text, "EMPNAME", "dealerEmp");
                txtGatePass.Text = dsJobCardMaster.Tables[0].Rows[0]["GatePass"].ToString().Trim();
                if (dsJobCardMaster.Tables[0].Rows[0]["Schedule"].ToString().Trim() != "")
                {
                    ddlScheduleJC.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["Schedule"].ToString().Trim();
                }

                ddlPayMode.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["PaymentMode"].ToString().Trim();
                ddlBillingType.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["Billing"].ToString().Trim();

            }
            //txtGatePass.Text = dsJobCardMaster.Tables[0].Rows[0]["GatePass"].ToString();

            //txtColor.Text = grl.GetStringValuesAgainstCodes("ColorCode", grl.GetStringValuesAgainstCodes("RegNo", ddlRegNo.Text, "ColorCode", "CustomerVehicle"), "ColorDesc", "Color");
        }

        private bool JobCard_PDI_FFI()
        {
            search_result = false;
            string dv = ddlJobTypeCode.Text;

            if (ddlJobTypeCode.SelectedValue.ToString().Trim() == "001"
               | ddlJobTypeCode.SelectedValue.ToString().Trim() == "002")
            {
                gvJobCardParts.DataSource = (DataTable)ViewState["Parts"]; gvJobCardParts.DataBind();
                if (rowInJobCardPartsDetail(gvJobCardParts) == true) search_result = true;

                gvLubParts.DataSource = (DataTable)ViewState["Lub"]; gvLubParts.DataBind();
                if (rowInJobCardLubricanteDetail(gvLubParts) == true) search_result = true;
            }
            return search_result;
        }

        protected void clearAll()
    {

        //ddlJobCardNo.Text = "";
        //ddlJobCardNo.Enabled = true;
        txtGatePass.Text = "";
        txtChassisNo.Text = string.Empty;
        txtEngineNo.Text = string.Empty;
        //ddlRegNo.Text = "--Select--";
        //ddlChassisNo.Text = "--Select--";
        //ddlEngineNo.Text = "--Select--";

        //ddlJobCardType.SelectedIndex = 0;
        //txtChassisNo.Text = "";
        //txtEngineNo.Text = "";
        txtCustomer.Text = "";
        txtCustomerDesc.Text = "";
        txtEndUser.Text = "";
        txtEndUserDesc.Text = "";
        txtBrand.Text = "";
        txtBrandDesc.Text = "";
        txtProduct.Text = "";
        txtVersion.Text = "";
        txtVersionDesc.Text = "";
        txtRemarks.Text = "";
        txtKM.Text = "";
        txtCouponNo.Text = "";
        ddlStatusJC.SelectedIndex = 0;
        lblJobCardTotal.Text = "0";
        lblPartsTotal.Text = "0";
        txtLubTotal.Text = "0";
        txtSubletTotal.Text = "0";
        txtColor.Text = "";
        ddlAdvisorCode.SelectedIndex = 0;
        createSubletDT();
        createLubDT();
        createPartsDT();
        createJobDT();
        createConPartsDT();
        txtSIRMasterId.Text = string.Empty;
        setInitialDates();
        ClearJobTextBoxes();
        ClearPartsTextBoxes();
        ClearLubricantsTextBoxes();
        ClearJobSubletTextBoxes();
        ClearConPartsTextBoxes();
        txtPartTotalQuantity.Text = lblPartsTotal.Text =
        lbltotLubQty.Text = txtLubTotal.Text =
        txtSubletTotal.Text = txtSubletPayableTotal.Text = "0";
        lblJobCardTotal.Text = txtJobsTotal.Text = "0";
        ddlVehRegNo.SelectedIndex = 0;
        txtDep.Text = string.Empty;
        lblSIRMaster.Text = "";

        //ddlJobCardNo.Text = "";
        //ddlJobCardNo.DataBind();
        ///Load_ddlJobCardCode();
        
        
        ddlJobCardTypeCode.SelectedIndex = 0;
        ddlPayMode.SelectedIndex = 0;
        ddlBillingType.SelectedIndex=0;
        ddlAdvisorCode.SelectedIndex = 0;
        ddlStatusJC.SelectedIndex = 0;
        if (ddlScheduleJC.Items.Count > 1)
        {
            ddlScheduleJC.SelectedIndex = 0;
        }
        

        //lblMsg.Text="";

        //btnSaveJC.Enabled = false;
        //btnDeleteJC.Enabled = false;
        //btnPostJC.Enabled = false;
        //btnPrintJC.Enabled = false;

        
        


        //ddlJobCardType.BackColor = Color.White;
        //ddlPaymentMode.BackColor = Color.White;
        //ddlBilling.BackColor = Color.White;
        //ddlEngineNo.BackColor = Color.White;
        //ddlRegNo.BackColor = Color.White;
        //ddlJobCardType.BackColor = Color.White;
        //ddlAdvisor.BackColor = Color.White;
        //ddlChassisNo.BackColor = Color.White;
        txtKM.BackColor = Color.White;

    }

        private void ClearJobTextBoxes()
        {
            TextBox[] txts = {     txtJobRemarks, txtLabor };
            ClearTextBoxes(txts);
            ddlTechnicianEmpCode.SelectedIndex = 0;
            ddlJobs_Labor.SelectedIndex = 0;
            //txtSchedual.Text = "";
        }

        private void ClearPartsTextBoxes()
        {
            TextBox[] txts = { txtQuantity,   txtPartPrice,  txtDep };
            ClearTextBoxes(txts); 
            txtPartsRecQuantity.Text = "0";
            ddlPartCode.SelectedIndex = 0;
        }
        private void ClearLubricantsTextBoxes()
        {
            TextBox[] txts = { txtLubPartDesc, txtLubPrice, txtLubQuantity};
            ClearTextBoxes(txts);
            txtLubRecQuantity.Text = "0";
            ddlLubCode.SelectedIndex = 0;
        }
        private void ClearJobSubletTextBoxes()
        {
            TextBox[] txts = { txtSubletJobDesc,  txtSubletPayAmnt, txtSubletRemarks, txtSubletIncAmnt, txtSubletDate };
            ClearTextBoxes(txts);
            ddlVendorCode.SelectedIndex = 0;
            ddlJobs_Sublet.SelectedIndex = 0;

        }
        public void ClearTextBoxes(TextBox[] textBoxes)
        {
            for (int i = 0; i < textBoxes.Length; i++)
            { textBoxes[i].Text = string.Empty; }
        }
        private void ClearConPartsTextBoxes()
        {
            TextBox[] txts = { txtConQuantity, txtConPartDesc, txtConPartPrice,txtPartItemNoConParts };
            ClearTextBoxes(txts); txtConPartsRecQuantity.Text = "0";
            ddlConsumableParts.SelectedIndex = 0;

        }
        

        #region Master Validation

        public bool MasterValidation(TextBox[] textBoxes)
        {
            bool isValid = true;

            for (int i = 0; i < textBoxes.Length; i++)
            {
                if (textBoxes[i].Text.Trim() == "")
                {
                    isValid = false;
                    textBoxes[i].BorderColor = System.Drawing.Color.Red;
                    SetFocus(textBoxes[i]);
                    
                    lblMsg.Text = "Error: Field(s) Marked With Red Steriks '*' Are Mendetory...";
                    break;
                }
                //else { textBoxes[i].BorderColor = System.Drawing.ColorTranslator.FromHtml("#1A6297"); }
            }
            return isValid;
        }
        #endregion

        private void JobCardEntry()
        {


            /////////////////

            if (txtSIRMasterId.Text.Trim() == string.Empty)
            {
                txtSIRMasterId.Text = grl.AutoGen("sirmaster", "SIRNO", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
            }

            #region JobCard Master Param
            SqlParameter[] JobCardMaster_param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
           /*1*/ new SqlParameter("@JobCardCode",SqlDbType.Char,8),
           /*2*/ new SqlParameter("@EstimateCode",SqlDbType.Char,8),
           /*3*/ new SqlParameter("@JobCardType",SqlDbType.VarChar,50),
           /*4*/ new SqlParameter("@CusCode",SqlDbType.Char,8),
           /*5*/ new SqlParameter("@UserName",SqlDbType.Char,100),
           /*6*/ new SqlParameter("@BrandCode",SqlDbType.VarChar,15),
           /*7*/ new SqlParameter("@RegNo",SqlDbType.VarChar,25),
           /*8*/ new SqlParameter("@EngineNo",SqlDbType.VarChar,30),
           /*9*/ new SqlParameter("@ChassisNo",SqlDbType.VarChar,30),
           /*10*/ new SqlParameter("@ProdCode",SqlDbType.VarChar,10),
           /*11*/ new SqlParameter("@VersionCode",SqlDbType.Char,3),
           /*12*/ new SqlParameter("@RecDate",SqlDbType.DateTime),
           /*13*/ new SqlParameter("@RecTime",SqlDbType.DateTime),
           /*14*/ new SqlParameter("@PromiseDate",SqlDbType.DateTime),
           /*15*/ new SqlParameter("@PromiseTime",SqlDbType.DateTime),
           /*16*/ new SqlParameter("@DelvDate",SqlDbType.DateTime),
           /*17*/ new SqlParameter("@DelvTime",SqlDbType.DateTime),
           /*18*/ new SqlParameter("@JobTypeCode",SqlDbType.Char,3),
           /*19*/ new SqlParameter("@KM",SqlDbType.Char,8),
           /*20*/ new SqlParameter("@GatePass",SqlDbType.Char,10),
           /*21*/ new SqlParameter("@Remarks",SqlDbType.Char,200),
           /*22*/ new SqlParameter("@RecFuel",SqlDbType.VarChar,10),           
           /*23*/ new SqlParameter("@RetFuel",SqlDbType.VarChar,10),           
           /*24*/ new SqlParameter("@Schedule",SqlDbType.Char,8),
           /*25*/ new SqlParameter("@sMonth",SqlDbType.Char,3),
           /*26*/ new SqlParameter("@JobsTotal",SqlDbType.Decimal),  
           /*27*/ new SqlParameter("@PartsTotal",SqlDbType.Decimal),  
           /*28*/ new SqlParameter("@LubPartsTotal",SqlDbType.Decimal),  
           /*29*/ new SqlParameter("@SubletTotal",SqlDbType.Decimal),  
           /*30*/ new SqlParameter("@RecRemarks",SqlDbType.VarChar,100),
           /*31*/ new SqlParameter("@RetRemarks",SqlDbType.VarChar,100),
           /*32*/ new SqlParameter("@Advisor",SqlDbType.Char,3),
           /*33*/ new SqlParameter("@SIRMaster",SqlDbType.Char,8),
           /*34*/ new SqlParameter("@DelFlag",SqlDbType.Char,1),
           /*35*/ new SqlParameter("@UpdUser",SqlDbType.Char,50),           
           ///*36*/ new SqlParameter("@UpdDate",SqlDbType.DateTime),
           ///*37*/ new SqlParameter("@UpdTime",SqlDbType.DateTime),
           /*38*/ new SqlParameter("@UpdTerm",SqlDbType.Char,50),
           /*39*/ new SqlParameter("@TransferStatus",SqlDbType.Char,1),
           /*40*/ new SqlParameter("@EndUserCode",SqlDbType.Char,8),
           /*41*/ new SqlParameter("@ConsumableTotal",SqlDbType.Decimal),  
           /*42*/ new SqlParameter("@jobstatus",SqlDbType.VarChar,10),  
           /*43/41*/ new SqlParameter("@PaymentMode",SqlDbType.VarChar,50),  
           /*44/42*/ new SqlParameter("@Billing",SqlDbType.VarChar,50),  
        };



            JobCardMaster_param[0].Value = Session["DealerCode"].ToString();
            //Auto Code Generation Decision on Insert OR Update
            //ddlJobCardNo.Text = grl.AutoGen("JobCardMaster", "JobCardCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy")); 
            //JobCardMaster_param[1].Value = ddlJobCardNo.Text;         
            if (ddlEstNo.SelectedIndex == 0)
            {
                JobCardMaster_param[2].Value = string.Empty;
            }
            else
            {
                JobCardMaster_param[2].Value = ddlEstNo.SelectedValue.ToString().Trim(); //EstimateCode        
            }

            JobCardMaster_param[3].Value = ddlJobCardTypeCode.SelectedValue.ToString().Trim();

            JobCardMaster_param[4].Value = txtCustomer.Text.Trim().ToUpper();

            JobCardMaster_param[5].Value = txtEndUserDesc.Text;

            JobCardMaster_param[6].Value = txtBrand.Text.Trim();

            JobCardMaster_param[7].Value = ddlVehRegNo.SelectedValue.ToString(). Trim().ToUpper();

            JobCardMaster_param[8].Value = txtEngineNo.Text.Trim().ToUpper();

            JobCardMaster_param[9].Value = txtChassisNo.Text.Trim().ToUpper();

            JobCardMaster_param[10].Value = txtProduct.Text.Trim().ToUpper();

            JobCardMaster_param[11].Value = txtVersion.Text.Trim().ToUpper();

            JobCardMaster_param[12].Value = grl.SaveDate(txtRecieptDate.Text);

            JobCardMaster_param[13].Value = Convert.ToDateTime(txtReciptTime.Text).ToString("HH:mm");//grl.SaveTime(txtReciptTime.Text);

            JobCardMaster_param[14].Value = grl.SaveDate(txtRecieptDate.Text);//grl.SaveDate(DatePicker1.Text).ToString();

            JobCardMaster_param[15].Value = Convert.ToDateTime(txtPromistedTime.Text).ToString("HH:mm"); //grl.SaveTime(txtPromistedTime.Text);

            JobCardMaster_param[16].Value = grl.SaveDate(txtRecieptDate.Text);//grl.SaveDate(txtDeliveryDate.Text.ToString());

            JobCardMaster_param[17].Value = Convert.ToDateTime(txtDeliveryTime.Text).ToString("HH:mm");//grl.SaveTime(txtDeliveryTime.Text);

            JobCardMaster_param[18].Value = "";//ddlJobType.SelectedItem.Value.ToString();

            JobCardMaster_param[19].Value = txtKM.Text.Trim();

            JobCardMaster_param[20].Value = string.Empty;// Updates when invoice will be saved. Value will be the jobcard No.        

            JobCardMaster_param[21].Value = txtRemarks.Text.Trim().ToUpper();

            JobCardMaster_param[22].Value = string.Empty; //101        

            JobCardMaster_param[23].Value = string.Empty; //101        

            if (ddlScheduleJC.SelectedIndex == 0)
            {
                JobCardMaster_param[24].Value = "";
            }
            else
            {
                JobCardMaster_param[24].Value = ddlScheduleJC.SelectedValue.ToString();
            }


            JobCardMaster_param[25].Value = string.Empty;// control should be visible false        

            JobCardMaster_param[26].Value = lblJobCardTotal.Text.Trim();

            JobCardMaster_param[27].Value = lblPartsTotal.Text.Trim();

            JobCardMaster_param[28].Value = txtLubTotal.Text;

            JobCardMaster_param[29].Value = txtSubletTotal.Text;

            JobCardMaster_param[30].Value = txtRemarks.Text.Trim().ToUpper();

            JobCardMaster_param[31].Value = string.Empty; //        

            JobCardMaster_param[32].Value = ddlAdvisorCode.SelectedValue.ToString().Trim();

            JobCardMaster_param[33].Value = txtSIRMasterId.Text;//(ViewState["SIRMasterCode"] == null ? string.Empty : ViewState["SIRMasterCode"].ToString()); // @SIRMaster | Incase of Parts and Lubricants sirmastercode will be auto generated. otherwise blank        

            JobCardMaster_param[34].Value = "N";
            if (Session["UserName"] == null)
            {
                JobCardMaster_param[35].Value = "";
            }
            else
            {
                JobCardMaster_param[35].Value = Session["UserName"].ToString();
            }

            //JobCardMaster_param[36].Value = grl.SaveDate(DateTime.Now.ToString("yyyy-MM-dd"));
            //JobCardMaster_param[37].Value = grl.SaveTime(DateTime.Now.ToString("HH:mm"));
            JobCardMaster_param[36].Value = Environment.MachineName;

            JobCardMaster_param[37].Value = "C"; //         

            JobCardMaster_param[38].Value = txtEndUser.Text.Trim().ToUpper();

            JobCardMaster_param[39].Value = "0"; // @ConsumableTotal would not be saved for minar items like . angry papper,elphi,petrol. amount is not recieved from customer        

            JobCardMaster_param[40].Value = ddlStatusJC.SelectedValue.ToString();

            JobCardMaster_param[41].Value = ddlPayMode.SelectedValue.ToString();
            JobCardMaster_param[42].Value = ddlBillingType.SelectedValue.ToString();

            #endregion


            try
            {
                //ObjTrans.BeginTransaction(ref Trans);


                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {

                    if (IsExist() == "Insert"
                        & IsExist() != "No Action")
                    //if ("Insert" == "Insert")
                    {
                        DataSet dsJc = new DataSet();

                        //if (grl.CodeExists("JobCardMaster", "EngineNo='" + txtEngineNo.Text + "' And RecDate='" + grl.SaveDate(txtRecieptDate.Text) + "' And JobCardType='" + ddlJobCardTypeCode.SelectedValue.ToString().Trim() + "' And DelFlag='N' And DealerCode='" + Session["DealerCode"].ToString() + "' ", ref dsJc))
                        //{
                        //    if (dsJc.Tables[0].Rows[0]["GatePass"].ToString().Trim() == "")
                        //    {
                        //        
                        //        grl.UserMsg(lblMsg, Color.Red, "JobCard No. " + dsJc.Tables[0].Rows[0]["JobCardCode"].ToString().Trim() + " already Opend on same Engine No. and same date please ues existing jobcard   ", txtSIRMasterId);
                        //        return;
                        //    }
                        //}
                        //if (grl.CodeExists("JobCardMaster", "EngineNo='" + txtEngineNo.Text + "' And JobCardType='" + ddlJobCardTypeCode.SelectedValue.ToString().Trim() + "' And DelFlag='N' And DealerCode='" + Session["DealerCode"].ToString() + "'  And GatePass=''", ref dsJc))
                        //{
                        //    
                        //    grl.UserMsg(lblMsg, Color.Red, "JobCard No. " + dsJc.Tables[0].Rows[0]["JobCardCode"].ToString().Trim() + " already exist on same Engine No. Please close existing jobcard  ", txtSIRMasterId);
                        //    return;
                        //}
                        if (grl.IsExist("SIRMaster", txtSIRMasterId.Text.Trim(), "JobCardMaster",Session["DealerCode"].ToString()))
                        {
                            
                            grl.UserMsg(lblMsg, Color.Red, "This SIRNo is Already Exist", txtSIRMasterId);
                            ObjTrans.RollBackTransaction(ref Trans);
                            return;
                        }
                        //ddlJobCardNo.Text = grl.AutoGen("JobCardMaster", "JobCardCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                        //JobCardMaster_param[1].Value = ddlJobCardNo.Text;
                        if (ddlJobCardCode.SelectedIndex==0)
                        {
                            JobCardMaster_param[1].Value = grl.AutoGen("JobCardMaster", "JobCardCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy")); //txtReferenceNo.Text;
                            //ddlJobCardNo.Text = JobCardMaster_param[1].Value.ToString();
                            HFJobCard.Value = JobCardMaster_param[1].Value.ToString(); 
                        }

                        if (myFunc.ExecuteSP_NonQuery("[sp_W2_JobCard_Master_Insert]", JobCardMaster_param, Trans))
                        {
                            //bool a = (d1 > 0 ? Inser_JobCardDetail() : false);
                            bool a = (rowsInJobCardDetail(gvJobCard) == true ? Inser_JobCardDetail() : false);
                            //bool c = (d2 > 0 ? Inser_JobCardPartsDetail() : false);
                            bool c = (rowInJobCardPartsDetail(gvJobCardParts) == true ? Inser_JobCardPartsDetail() : false);
                            //bool d = (d3 > 0 ? Inser_JobCardLubricanteDetail() : false);
                            bool d = (rowInJobCardLubricanteDetail(gvLubParts) == true ? Inser_JobCardLubricanteDetail() : false);
                            //bool b = (d4 > 0 ? Inser_JobCardSubletDetail() : false);
                            bool b = (rowInJobCardSubletDetail(gvSublet) == true ? Inser_JobCardSubletDetail() : false);
                            bool e = (rowInJobCardConPartsDetail(gvJobCardConParts) == true ? Inser_JobCardConPartsDetail() : false);
                            //if (c == true || d == true)
                            //{
                            Inser_SIR_Master_Detail();
                            //}
                        }
                        else { objMBLL.ShowMessageBox("Saving Faild", txtCustomer); }
                    }
                    else
                    {
                        //Jobcard no exist if delfalg='N'
                        //Gate <> ''
                        //txtchassisno. engine 
                        if (txtSIRMasterId.Text.Trim() != string.Empty)
                        {
                            if (grl.CodeExists("JobCardMaster", "SIRMaster", txtSIRMasterId.Text.Trim(), "And JobCardCode <> '" + ddlJobCardCode.SelectedValue.ToString().Trim() + "'", GlobalVar.mDealerCode))
                            {
                                
                                grl.UserMsg(lblMsg, Color.Red, "SIRNo already used in another JobCard", txtCustomer);
                                return;
                            }
                        }
                        //if (ddlJobCardCode.SelectedIndex != 0)
                        //{
                        //    if (grl.IsExist("JobCardCode", ddlJobCardNo.Text, "JobCardMaster",Session["DealerCode"].ToString(), "And DelFlag='N' And GatePass<>''"))
                        //    {
                        //        
                        //        grl.UserMsg(lblMsg, Color.Red, "JobCard dose not Exists", txtCustomer);
                        //        return;
                        //    }
                        //}
                        JobCardMaster_param[1].Value = ddlJobCardCode.SelectedValue.ToString().Trim();

                        if (myFunc.ExecuteSP_NonQuery("[sp_Update_JobcardMaster]", JobCardMaster_param, Trans))
                        {
                            Inser_JobCardDetail();
                            Inser_JobCardPartsDetail();
                            Inser_JobCardLubricanteDetail();
                            Inser_JobCardSubletDetail();
                            Inser_JobCardConPartsDetail();
                            bool c;
                            bool d;
                            Inser_SIR_Master_Detail();
                        }
                        else { objMBLL.ShowMessageBox("Update Fail", lblMsg); }
                    }
                    
                    if (ObjTrans.CommittTransaction(ref Trans) == true)
                    {
                        grl.UserMsg(lblMsg, Color.Green, "Record Saved Successfully: " + HFJobCard.Value);
                    }
                    else
                    {
                        ObjTrans.RollBackTransaction(ref Trans);
                        grl.UserMsg(lblMsg, Color.Red, "Record not saved Try again. Or contact to support team ");
                    }
                    clearAll();

                }
            }
            catch (Exception ex)
            {
                grl.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message, txtCustomer);
                ObjTrans.RollBackTransaction(ref Trans);
            }
        }

        private bool rowsInJobCardDetail(GridView gvJobCard)
        {
            search_item = false;
            if (gvJobCard.Rows.Count > 0)
            {
                search_item = true;
            }
            //foreach (GridViewRow row in gvJobCard.Rows) { if (row.Cells[2].Text.Replace("&nbsp;", "").Trim() != "") search_item = true; }
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
        private bool rowInJobCardLubricanteDetail(GridView gvLubParts)
        {
            search_item = false;
            if (gvLubParts.Rows.Count > 0)
            {
                search_item = true;
            }
            //foreach (GridViewRow row in gvLubParts.Rows) { if (row.Cells[2].Text.Replace("&nbsp;", "").Trim() != "") search_item = true; }
            return search_item;
        }
        private bool rowInJobCardSubletDetail(GridView gvSublet)
        {
            search_item = false;
            if (gvSublet.Rows.Count > 0)
            {
                search_item = true;
            }
            //foreach (GridViewRow row in gvSublet.Rows) { if (row.Cells[2].Text.Replace("&nbsp;", "").Trim() != "") search_item = true; }
            return search_item;
        }
         private bool rowInJobCardConPartsDetail(GridView gvJobCardConParts)
        {
            search_item = false;
            if (gvJobCardConParts.Rows.Count > 0)
            {
                search_item = true;
            }
            //foreach (GridViewRow row in gvJobCardParts.Rows) { if (row.Cells[2].Text.Replace("&nbsp;", "").Trim() != "") search_item = true; }
            return search_item;
        }

        

        private void Insert_in_JobDT(DataRow rowJobDT)
        {
            DataTable dt = myFunc.GetData("select DefJobDesc from DefaultJob where DealerCode IN('"+ Session["DealerCode"].ToString() +"','COMON') And  DefJobCode='" + ddlJobs_Labor.SelectedValue.ToString() + "'");

            rowJobDT["JobCode"] = ddlJobs_Labor.SelectedValue.ToString().Trim();
            rowJobDT["DefJobDesc"] = dt.Rows[0]["DefJobDesc"].ToString().Trim();
            rowJobDT["JobRemarks"] = txtJobRemarks.Text.Trim().ToUpper();
            rowJobDT["EmpCode"] = ddlTechnicianEmpCode.SelectedItem.Value.ToString();
            rowJobDT["EmpName"] = ddlTechnicianEmpCode.SelectedItem.ToString();
            rowJobDT["Amount"] = txtLabor.Text;
            rowJobDT["Type"] = "J";
        }

        private string IsExist()
        {
            string strAction = "No Action";
            if ((grl.IsExist("BrandCode", txtBrand.Text, "Brand", Session["DealerCode"].ToString()) == true ? true : false))
            {
                if ((grl.IsExist("ProdCode", txtProduct.Text, "Vehicle", Session["DealerCode"].ToString()) == true ? true : false))
                {
                    if ((grl.IsExist("VersionCode", txtVersion.Text, "Vehicle", Session["DealerCode"].ToString()) == true ? true : false))
                    {
                        apStr = " and versioncode=" + txtVersion.Text + " and ProdCode='" + txtProduct.Text + "'";
                        if ((grl.IsExist("ProdCode", txtProduct.Text, "Vehicle",Session["DealerCode"].ToString(), apStr) == true ? true : false))
                        {
                            if (ddlJobCardCode.SelectedIndex == 0) strAction = "Insert";
                            else
                            {
                                apStr = " and chassisno ='" + txtChassisNo.Text + "' and GatePass = null";
                                if (grl.IsExist("ChassisNo", txtChassisNo.Text, "JobCardMaster", Session["DealerCode"].ToString()))
                                {
                                    if (grl.IsExist("JobCardCode", ddlJobCardCode.SelectedValue.ToString().Trim(), "JobCardMaster",Session["DealerCode"].ToString(), apStr))
                                    {
                                        objMBLL.ShowMessageBox("Update Failed", txtCustomer);
                                        strAction = "No Action";
                                    }
                                    else strAction = "Update";
                                }
                                else strAction = "Update";
                            }
                        }
                    }
                    else objMBLL.ShowMessageBox("'Version' not exist! ", txtCustomer);
                }
                else objMBLL.ShowMessageBox("'Product' not exist! ", txtCustomer);
            }
            else objMBLL.ShowMessageBox("'Brand' not exist! ", txtCustomer);

            return strAction;
        }

        private bool Inser_JobCardDetail()
        {
            try
            {
                bool flag = false;
                //if (gvJobCard.Rows.Count > 0)
                //{
                SqlParameter[] JobCardDetail_Delete_param = {                                                       
                                                                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                                                                //new SqlParameter("@JobCode",SqlDbType.VarChar,8)
                                                            };
                JobCardDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                JobCardDetail_Delete_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                //JobCardDetail_Delete_param[2].Value = txtJob.Text;
                SqlParameter[] JobCardDetail_Insert_param = {                                                       
                                                                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                new SqlParameter("@JobCardCode",SqlDbType.Char,8),
                                                                new SqlParameter("@JobCode",SqlDbType.VarChar,8),
                                                                new SqlParameter("@EmpCode",SqlDbType.Char,3),
                                                                new SqlParameter("@Amount",SqlDbType.Float),
                                                                new SqlParameter("@Type",SqlDbType.Char,1),
                                                                new SqlParameter("@JobRemarks",SqlDbType.VarChar,200)
                                                            };










                if (myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_Detail_Delete", JobCardDetail_Delete_param, Trans))
                {
                    JobDT = (DataTable)ViewState["Job"];
                    DataRow[] drr = JobDT.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {
                        if (JobDT.Rows[i]["JobCode"].ToString() != ""
                            & JobDT.Rows[i]["EmpCode"].ToString() != ""
                            & JobDT.Rows[i]["Amount"].ToString() != "")
                        {
                            JobCardDetail_Insert_param[0].Value = Session["DealerCode"].ToString();
                            JobCardDetail_Insert_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                            JobCardDetail_Insert_param[2].Value = JobDT.Rows[i]["JobCode"].ToString();
                            JobCardDetail_Insert_param[3].Value = JobDT.Rows[i]["EmpCode"].ToString();
                            JobCardDetail_Insert_param[4].Value = Convert.ToDecimal(JobDT.Rows[i]["Amount"].ToString());
                            JobCardDetail_Insert_param[5].Value = JobDT.Rows[i]["Type"].ToString();
                            JobCardDetail_Insert_param[6].Value = JobDT.Rows[i]["JobRemarks"].ToString();
                            myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_Detail_Insert", JobCardDetail_Insert_param, Trans);
                            flag = true;
                        }
                    }
                    // }
                }
                return flag;
            }
            catch (Exception ex) { throw ex; }
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
                JobCardPartsDetail_Delete_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();

                SqlParameter[] JobCardPartsDetail_Insert_param = {                                                       
                                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8),
                                                                    new SqlParameter("@ItemCode",SqlDbType.VarChar,8),
                                                                    new SqlParameter("@PartNo",SqlDbType.Char,18),
                                                                    new SqlParameter("@Qty",SqlDbType.VarChar,10),
                                                                    new SqlParameter("@Price",SqlDbType.VarChar,50),
                                                                    new SqlParameter("@RecQty",SqlDbType.VarChar,50),
                                                                    new SqlParameter("@Type",SqlDbType.Char,1),
                                                                    new SqlParameter("@Dep",SqlDbType.VarChar,5)
                                                                 };

                if (myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_PartsDetail_Delete", JobCardPartsDetail_Delete_param, Trans))
                {
                    PartsDT = (DataTable)ViewState["Parts"];
                    DataRow[] drr = PartsDT.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {
                        JobCardPartsDetail_Insert_param[0].Value = Session["DealerCode"].ToString();
                        JobCardPartsDetail_Insert_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                        if (PartsDT.Rows[i]["ItemCode"].ToString() != "")
                        {
                            JobCardPartsDetail_Insert_param[2].Value = PartsDT.Rows[i]["ItemCode"].ToString(); //Item Code System Generted num. Get it from lookup
                            JobCardPartsDetail_Insert_param[3].Value = PartsDT.Rows[i]["PartNo"].ToString();
                            JobCardPartsDetail_Insert_param[4].Value = PartsDT.Rows[i]["Qty"].ToString();
                            JobCardPartsDetail_Insert_param[5].Value = PartsDT.Rows[i]["Price"].ToString();
                            JobCardPartsDetail_Insert_param[6].Value = PartsDT.Rows[i]["RecQty"].ToString();
                            if (ddlJobCardTypeCode.SelectedItem.Text == "Insurance")
                            {
                                JobCardPartsDetail_Insert_param[7].Value = "A"; //Type
                                JobCardPartsDetail_Insert_param[8].Value = PartsDT.Rows[i]["Dep"].ToString(); //Dep
                            }
                            else
                            {
                                JobCardPartsDetail_Insert_param[7].Value = "P"; //Type
                                JobCardPartsDetail_Insert_param[8].Value = "0"; //Dep
                            }
                            myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_PartsDetail_Insert", JobCardPartsDetail_Insert_param, Trans);
                            flag = true;
                        }
                    }
                }
                // }
                return flag;
            }
            catch (Exception ex) { throw ex; }

        }
        private bool Inser_JobCardLubricanteDetail()
        {
            try
            {
                bool flag = false;
                // if (gvLubParts.Rows.Count > 0)
                //{

                SqlParameter[] JobCardLubricanteDetail_Delete_param = {                                                       
                                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                                                                  };
                JobCardLubricanteDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                JobCardLubricanteDetail_Delete_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();

                SqlParameter[] JobCardLubricanteDetail_Insert_param = {                                                       
                                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8),
                                                                    new SqlParameter("@ItemCode",SqlDbType.VarChar,8),
                                                                    new SqlParameter("@PartNo",SqlDbType.Char,18),
                                                                    new SqlParameter("@Qty",SqlDbType.VarChar,10),
                                                                    new SqlParameter("@Price",SqlDbType.VarChar,50),
                                                                    new SqlParameter("@RecQty",SqlDbType.VarChar,50)
                                                                  };

                if (myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_LubricanteDetail_Delete", JobCardLubricanteDetail_Delete_param, Trans))
                {
                    LubDT = (DataTable)ViewState["Lub"];
                    DataRow[] drr = LubDT.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {
                        JobCardLubricanteDetail_Insert_param[0].Value = Session["DealerCode"].ToString();
                        JobCardLubricanteDetail_Insert_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                        if (LubDT.Rows[i]["ItemCode"].ToString() != "")
                        {
                            JobCardLubricanteDetail_Insert_param[2].Value = LubDT.Rows[i]["ItemCode"].ToString(); ; //Item Code System Generted num. Get it from lookup
                            JobCardLubricanteDetail_Insert_param[3].Value = LubDT.Rows[i]["PartNo"].ToString();
                            JobCardLubricanteDetail_Insert_param[4].Value = LubDT.Rows[i]["Qty"].ToString();
                            JobCardLubricanteDetail_Insert_param[5].Value = LubDT.Rows[i]["Price"].ToString();
                            JobCardLubricanteDetail_Insert_param[6].Value = LubDT.Rows[i]["RecQty"].ToString(); // 102                        
                            //JobCardLubricanteDetail_Insert_param[7].Value = "L"; //Type
                            //JobCardLubricanteDetail_Insert_param[8].Value = "0"; //Dep
                            myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_LubricanteDetail_Insert", JobCardLubricanteDetail_Insert_param, Trans);
                            flag = true;
                        }
                    }
                }
                // }
                return flag;
            }
            catch (Exception ex) { throw ex; }

        }
        private bool Inser_JobCardSubletDetail()
        {
            bool flag = false;
            //  if (gvSublet.Rows.Count > 0)
            //  {

            SqlParameter[] JobCardSubletDetail_Delete_param = {                                                       
                                                                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                                                              };
            JobCardSubletDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
            JobCardSubletDetail_Delete_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();

            SqlParameter[] JobCardSubletDetail_Insert_param = {                                                       
                                                                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                new SqlParameter("@JobCardCode",SqlDbType.Char,8),
                                                                new SqlParameter("@VendorCode",SqlDbType.Char,6),
                                                                new SqlParameter("@JobCode",SqlDbType.Char,8),
                                                                new SqlParameter("@SubletRemarks",SqlDbType.VarChar,50),
                                                                new SqlParameter("@SubletDate",SqlDbType.DateTime),
                                                                new SqlParameter("@SubletAmount",SqlDbType.VarChar,50),
                                                                new SqlParameter("@PayableAmount",SqlDbType.VarChar,50)
                                                              };

            bool ans = myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_Sublet_Delete", JobCardSubletDetail_Delete_param, Trans);
            SubletDT = (DataTable)ViewState["SubLet"];
            DataRow[] drr = SubletDT.Select();
            for (int i = 0; i < drr.Length; i++)
            {
                JobCardSubletDetail_Insert_param[0].Value = Session["DealerCode"].ToString();
                JobCardSubletDetail_Insert_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                if (SubletDT.Rows[i]["VendorCode"].ToString() != "")
                {
                    JobCardSubletDetail_Insert_param[2].Value = SubletDT.Rows[i]["VendorCode"].ToString();
                    JobCardSubletDetail_Insert_param[3].Value = SubletDT.Rows[i]["JobCode"].ToString();
                    JobCardSubletDetail_Insert_param[4].Value = SubletDT.Rows[i]["Remarks"].ToString();
                    JobCardSubletDetail_Insert_param[5].Value = grl.SaveDate(SubletDT.Rows[i]["SubletDate"].ToString());
                    JobCardSubletDetail_Insert_param[6].Value = SubletDT.Rows[i]["SubletAmount"].ToString();
                    JobCardSubletDetail_Insert_param[7].Value = SubletDT.Rows[i]["PayableAmount"].ToString();
                    myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_Sublet_Insert", JobCardSubletDetail_Insert_param, Trans);
                    flag = true;
                }
                // }
            }
            return flag;
        }
        private bool Inser_SIR_Master_Detail()
        {
            bool flag = false;
            try
            {


                SqlParameter[] SIRMaster_Insert_param = {                         
                              
                /*0*/  new SqlParameter("@DealerCode",SqlDbType.Char,5),
                /*1*/  new SqlParameter("@SIRNo",SqlDbType.Char,8),
                /*2*/  new SqlParameter("@SIRDate",SqlDbType.DateTime),
                /*3*/  new SqlParameter("@Remarks",SqlDbType.VarChar,50),
                /*4*/  new SqlParameter("@RequsitionerCode",SqlDbType.Char,5),
                /*5*/  new SqlParameter("@Requsitioner",SqlDbType.Char,1),
                /*6*/  new SqlParameter("@InvoiceNO",SqlDbType.Char,8),
                /*7*/  new SqlParameter("@JobCardNO",SqlDbType.Char,8),
                /*8*/  new SqlParameter("@DelFlag",SqlDbType.Char,1),
                /*9*/  new SqlParameter("@UpdUser",SqlDbType.VarChar,50),
                ///*10*/ new SqlParameter("@UpdDate",SqlDbType.DateTime),
                ///*11*/ new SqlParameter("@UpdTime",SqlDbType.DateTime), 
                /*12*/ new SqlParameter("@UpdTerm",SqlDbType.VarChar,50)};
                string SirNo = txtSIRMasterId.Text.Trim();//grl.AutoGen("sirmaster", "SIRNO", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                SIRMaster_Insert_param[0].Value = Session["DealerCode"].ToString();
                SIRMaster_Insert_param[1].Value = SirNo;
                SIRMaster_Insert_param[2].Value = grl.SaveDate(txtRecieptDate.Text);
                SIRMaster_Insert_param[3].Value = txtRemarks.Text;
                SIRMaster_Insert_param[4].Value = ""; // 101
                SIRMaster_Insert_param[5].Value = "J"; //101
                SIRMaster_Insert_param[6].Value = ""; //101
                SIRMaster_Insert_param[7].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                SIRMaster_Insert_param[8].Value = "N";
                if (Session["UserName"] == null)
                {
                    SIRMaster_Insert_param[9].Value = "";
                }
                else
                {
                    SIRMaster_Insert_param[9].Value = Session["UserName"].ToString();
                }
                //SIRMaster_Insert_param[10].Value = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
                //SIRMaster_Insert_param[11].Value = Convert.ToDateTime(DateTime.Now.ToString("HH:mm"));
                SIRMaster_Insert_param[10].Value = Environment.MachineName;


                if (myFunc.ExecuteSP_NonQuery("sp_W2_SIRMaster_Insert", SIRMaster_Insert_param, Trans))
                {
                    string jcno = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                    string strSIRNo = grl.GetStringValuesAgainstCodes("JobCardNO", jcno, "SIRNo", "SIRMaster");


                    SqlParameter[] SIRDetail_Insert_param = {                                                       
                                                            /*0*/  new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                            /*1*/  new SqlParameter("@SIRNo",SqlDbType.Char,8),                
                                                            /*2*/  new SqlParameter("@ItemCode",SqlDbType.Char,8),
                                                            /*3*/  new SqlParameter("@Quantity",SqlDbType.Float),
                                                            /*4*/  new SqlParameter("@Remarks",SqlDbType.VarChar,50),
                                                            /*5*/  new SqlParameter("@RecQty",SqlDbType.Float),
                                                            /*6*/  new SqlParameter("@Type",SqlDbType.Char,1)};
                    //-------------------------------------Parts Item for SIR Detail

                    /////Delete Parts SIRDetail
                    SqlParameter[] SIRDetail_Delete_param = {                                                       
                                                            /*0*/  new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                         /*1*/  new SqlParameter("@SIRNo",SqlDbType.Char,8),
                                                        /*6*/  new SqlParameter("@Type",SqlDbType.Char,1)};
                    SIRDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                    if (strSIRNo.Trim() == "")
                    {
                        SIRDetail_Delete_param[1].Value = SirNo;
                    }
                    else
                    {
                        SIRDetail_Delete_param[1].Value = strSIRNo;
                    }
                    SIRDetail_Delete_param[2].Value = "P";
                    myFunc.ExecuteSP_NonQuery("sp_2W_SIRDetailDelete", SIRDetail_Delete_param, Trans);
                    ////////////////
                    //////////Delete SIRDetail for Lubs
                    SIRDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                    if (strSIRNo.Trim() == "")
                    {
                        SIRDetail_Delete_param[1].Value = SirNo;
                    }
                    else
                    {
                        SIRDetail_Delete_param[1].Value = strSIRNo;
                    }
                    SIRDetail_Delete_param[2].Value = "L";
                    myFunc.ExecuteSP_NonQuery("sp_2W_SIRDetailDelete", SIRDetail_Delete_param, Trans);
                    /////////////
                    DataRow[] drParts = PartsDT.Select();
                    for (int i = 0; i < drParts.Length; i++)
                    {
                        if (PartsDT.Rows[i]["ItemCode"].ToString() != "")
                        {
                            SIRDetail_Insert_param[0].Value = Session["DealerCode"].ToString();
                            if (strSIRNo.Trim() == "")
                            {
                                SIRDetail_Insert_param[1].Value = SirNo;
                            }
                            else
                            {
                                SIRDetail_Insert_param[1].Value = strSIRNo;
                            }
                            SIRDetail_Insert_param[2].Value = PartsDT.Rows[i]["ItemCode"].ToString();
                            SIRDetail_Insert_param[3].Value = float.Parse(PartsDT.Rows[i]["Qty"].ToString());
                            //SIRDetail_Insert_param[4].Value = txtRemarks.Text;  // 101
                            SIRDetail_Insert_param[4].Value = "";  // 101
                            SIRDetail_Insert_param[5].Value = float.Parse(PartsDT.Rows[i]["RecQty"].ToString());
                            SIRDetail_Insert_param[6].Value = PartsDT.Rows[i]["Type"].ToString();
                            myFunc.ExecuteSP_NonQuery("sp_W2_SIRDetail_Insert", SIRDetail_Insert_param, Trans);
                            flag = true;
                        }
                    }

                    //-------------------------------------Lubricants Item for SIR Detail

                    DataRow[] drLub = LubDT.Select();
                    for (int i = 0; i < drLub.Length; i++)
                    {
                        if (LubDT.Rows[i]["ItemCode"].ToString().Trim() != "")
                        {
                            SIRDetail_Insert_param[0].Value = Session["DealerCode"].ToString();
                            if (strSIRNo.Trim() == "")
                            {
                                SIRDetail_Insert_param[1].Value = SirNo;
                            }
                            else
                            {
                                SIRDetail_Insert_param[1].Value = strSIRNo;
                            }
                            SIRDetail_Insert_param[2].Value = LubDT.Rows[i]["ItemCode"].ToString();
                            SIRDetail_Insert_param[3].Value = float.Parse(LubDT.Rows[i]["Qty"].ToString());
                            //SIRDetail_Insert_param[4].Value = txtRemarks.Text;  // 101
                            SIRDetail_Insert_param[4].Value = "";  // 101
                            SIRDetail_Insert_param[5].Value = float.Parse(LubDT.Rows[i]["RecQty"].ToString());
                            SIRDetail_Insert_param[6].Value = 'L';
                            myFunc.ExecuteSP_NonQuery("sp_W2_SIRDetail_Insert", SIRDetail_Insert_param, Trans);
                            flag = true;
                        }
                    }
                    flag = true;
                }
                return flag;
            }
            catch (Exception ex)
            {
                flag = false;
                grl.UserMsg(lblMsg, Color.Red, "Error: sp_W2_SIRMaster_Insert " + ex.Message, txtCustomer);
                return flag;
            }
        }

        private bool Inser_JobCardConPartsDetail()
        {
            try
            {
                bool flag = false;
                // if (gvJobCardParts.Rows.Count > 0)
                //{
                SqlParameter[] JobCardConPartsDetail_Delete_param = {                                                       
                                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                                                                 };
                JobCardConPartsDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                JobCardConPartsDetail_Delete_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();

                SqlParameter[] JobCardConPartsDetail_Insert_param = {                                                       
                                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8),
                                                                    new SqlParameter("@ItemCode",SqlDbType.VarChar,8),
                                                                    new SqlParameter("@PartNo",SqlDbType.Char,18),
                                                                    new SqlParameter("@Qty",SqlDbType.VarChar,10),
                                                                    new SqlParameter("@Price",SqlDbType.VarChar,50),
                                                                    new SqlParameter("@RecQty",SqlDbType.VarChar,50),
                                                                    new SqlParameter("@Type",SqlDbType.Char,1)
                                                                 };

                if (myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_ConPartsDetail_Delete", JobCardConPartsDetail_Delete_param, Trans))
                {
                    ConPartsDT = (DataTable)ViewState["ConParts"];
                    DataRow[] drr = ConPartsDT.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {
                        JobCardConPartsDetail_Insert_param[0].Value = Session["DealerCode"].ToString();
                        JobCardConPartsDetail_Insert_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                        if (ConPartsDT.Rows[i]["ItemCode"].ToString() != "")
                        {
                            JobCardConPartsDetail_Insert_param[2].Value = ConPartsDT.Rows[i]["ItemCode"].ToString(); //Item Code System Generted num. Get it from lookup
                            JobCardConPartsDetail_Insert_param[3].Value = ConPartsDT.Rows[i]["PartNo"].ToString();
                            JobCardConPartsDetail_Insert_param[4].Value = ConPartsDT.Rows[i]["Qty"].ToString();
                            JobCardConPartsDetail_Insert_param[5].Value = ConPartsDT.Rows[i]["Price"].ToString();
                            JobCardConPartsDetail_Insert_param[6].Value = ConPartsDT.Rows[i]["RecQty"].ToString();
                            if (ddlJobCardTypeCode.SelectedItem.Text == "Insurance")
                            {
                                JobCardConPartsDetail_Insert_param[7].Value = "A"; //Type
                                //JobCardConPartsDetail_Insert_param[8].Value = ConPartsDT.Rows[i]["Dep"].ToString(); //Dep
                            }
                            else
                            {
                                JobCardConPartsDetail_Insert_param[7].Value = "P"; //Type
                                //JobCardConPartsDetail_Insert_param[8].Value = "0"; //Dep
                            }
                            myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_ConPartsDetail_Insert", JobCardConPartsDetail_Insert_param, Trans);
                            flag = true;
                        }
                    }
                }
                // }
                return flag;
            }
            catch (Exception ex) { throw ex; }

        }

        protected int GetRowForEntry(DataTable dt)
        {
            int retAns = 0;
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                try
                {
                    if (dt.Rows[i] != null)
                    {
                        if (dt.Rows[i]["ID"].ToString() == "")
                        {
                            retAns = i;
                            break;
                        }
                    }
                }
                catch (Exception ex) { retAns = 0; throw ex; }
            }
            return retAns;
        }

        protected void btnPartsAddInGrid_Click(object sender, EventArgs e)
        {
            if (ddlPartCode.SelectedIndex == 0)
            {
                
                grl.UserMsg(lblMsg, Color.Red, "Please enter Quantity", txtQuantity);
                return;
            }
            else
            {
                lblMsg.Text = string.Empty;
            }
            if (txtQuantity.Text == "")
            {
                txtQuantity.Text = "0";
            }
            if (Convert.ToInt32(txtQuantity.Text) ==0)
            {
                
                grl.UserMsg(lblMsg, Color.Red, "Please enter Quantity", txtQuantity);
                return;
            }
            if (Convert.ToInt32(txtQuantity.Text) < Convert.ToInt32(txtPartsRecQuantity.Text))
            {
                
                grl.UserMsg(lblMsg, Color.Red, "Issue quantity not less then receive quantity", txtQuantity);
                return;
            }
            else
            {
                lblMsg.Text = "";
            }
            //
            TextBox[] textBoxes = {  txtPartPrice, txtQuantity };
            if (!MasterValidation(textBoxes)) return;
            else
            {
                search_result = false;
                foreach (DataRow rowPartsDT in PartsDT.Rows)
                {
                    //if (rowPartsDT["ItemCode"].ToString().Trim() == ""
                    //    | rowPartsDT["ItemCode"].ToString().Trim() == txtItemcodeParts.Text.Trim())
                    //{
                        Insert_in_PartsDT(rowPartsDT); search_result = true;
                    //}
                }
                if (search_result == false)
                {
                    DataRow rowPartsDT = PartsDT.NewRow(); Insert_in_PartsDT(rowPartsDT); PartsDT.Rows.Add(rowPartsDT);
                }
                ViewState["Parts"] = PartsDT; gvJobCardParts.DataSource = PartsDT; gvJobCardParts.DataBind();
                int Count = 0;
                foreach (DataRow dr in PartsDT.Rows)
                {
                    Count = Count + Convert.ToInt32(dr["Qty"]);
                }
                txtPartTotalQuantity.Text = Count.ToString();
                ClearPartsTextBoxes();

            }
        }
        private void Insert_in_PartsDT(DataRow rowPartsDT)
        {
            //DataTable dt = myFunc.GetData("select ItemDesc from Item where DealerCode='" + Session["DealerCode"].ToString() + "' And  ItemCode='" + ddlPartCode.SelectedValue.ToString() + "'");
            rowPartsDT["PartNo"] = txtPartItemNo_Parts.Text.Trim();
            rowPartsDT["PartsDesc"] = txtItemDesc.Text.Trim();
            if (ddlJobCardTypeCode.SelectedItem.Text == "Insurance")
            {
                if (txtDep.Text.Trim() == string.Empty)
                {
                    txtDep.Text = "0";
                }
                rowPartsDT["Dep"] = txtDep.Text.Trim();
                rowPartsDT["DepAmount"] = (Math.Round((double.Parse(txtQuantity.Text.Trim()) * double.Parse(txtPartPrice.Text.Trim()) / 100) * double.Parse(txtDep.Text.Trim())));
            }
            else
            {
                rowPartsDT["Dep"] = "0";
                rowPartsDT["DepAmount"] = "0";
            }

            rowPartsDT["Qty"] = txtQuantity.Text.Trim(); rowPartsDT["Price"] = txtPartPrice.Text.Trim();
            rowPartsDT["Type"] = "P";
            rowPartsDT["Total"] = double.Parse(txtQuantity.Text.Trim()) * double.Parse(txtPartPrice.Text.Trim());
            rowPartsDT["ItemCode"] = ddlPartCode.SelectedValue.ToString().Trim();
            rowPartsDT["RecQty"] = txtPartsRecQuantity.Text.Trim();
        }
        protected void btnLubAddInGrid_Click(object sender, EventArgs e)
        {
            if(ddlLubCode.SelectedIndex==0)
            {
                
                grl.UserMsg(lblMsg, Color.Red, "Please select Lube Item", txtQuantity);
                return;
            }
            
            TextBox[] textBoxes = { txtLubPrice, txtLubQuantity };
            if (!MasterValidation(textBoxes)) return;
            else
            {
                search_result = false;
                foreach (DataRow rowLubDT in LubDT.Rows)
                {
                    if (rowLubDT["ItemCode"].ToString().Trim() == ""
                        | rowLubDT["ItemCode"].ToString().Trim() == ddlLubCode.SelectedValue.ToString().Trim())
                    {
                        Insert_in_LubPartsDT(rowLubDT); search_result = true;
                    }
                }
                if (search_result == false)
                {
                    DataRow rowLubDT = LubDT.NewRow(); Insert_in_LubPartsDT(rowLubDT); LubDT.Rows.Add(rowLubDT);
                }
                ViewState["Lub"] = LubDT; gvLubParts.DataSource = LubDT; gvLubParts.DataBind();
                ClearLubricantsTextBoxes();
            }
        }
        private void Insert_in_LubPartsDT(DataRow rowLubDT)
        {
            rowLubDT["PartNo"] = txtPartItemNo_Lubs.Text.Trim();
            rowLubDT["PartsDesc"] = txtLubPartDesc.Text.Trim();
            rowLubDT["Qty"] = txtLubQuantity.Text.Trim(); rowLubDT["Price"] = txtLubPrice.Text.Trim();
            rowLubDT["Total"] = double.Parse(txtLubQuantity.Text.Trim()) * double.Parse(txtLubPrice.Text.Trim());
            rowLubDT["ItemCode"] = ddlLubCode.SelectedValue.ToString().Trim();
            rowLubDT["RecQty"] = txtLubRecQuantity.Text.Trim();
        }
        //protected void btnSubletAddinGrid_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (ddlVendorCode.SelectedIndex == 0)
        //        {
        //            grl.UserMsg(lblMsg, Color.Red, "Please Select Vendor");
        //            return;
        //        }
        //        else
        //        {
        //            lblMsg.Text = "";
        //        }
        //        if (ddlJobs_Sublet.SelectedIndex==0)
        //        {
        //            grl.UserMsg(lblMsg, Color.Red, "Please Select Sublet job", txtCustomer);
        //            return;
        //        }
        //        else
        //        {
        //            lblMsg.Text = "";
        //        }
        //        if (txtSubletDate.Text.Trim() == string.Empty || txtSubletDate.Text.Trim() == "__-__-____")
        //        {
        //            grl.UserMsg(lblMsg, Color.Red, "Please Select Sublet Date", txtSubletDate);
        //            return;
        //        }
        //        else
        //        {
        //            lblMsg.Text = "";
        //        }
        //        if (txtSubletIncAmnt.Text.Trim() == string.Empty)
        //        {
        //            grl.UserMsg(lblMsg, Color.Red, "Invoice Amount is not be left be blank", txtSubletIncAmnt);
        //            return;
        //        }
        //        else
        //        {
        //            lblMsg.Text = "";
        //        }
        //        if (txtSubletPayAmnt.Text.Trim() == string.Empty)
        //        {
        //            grl.UserMsg(lblMsg, Color.Red, "Paid Amount is not be left be blank", txtSubletPayAmnt);
        //            return;
        //        }
        //        else
        //        {
        //            lblMsg.Text = "";
        //        }
        //        

        //        search_result = false;
        //        foreach (DataRow rowSubletDT in SubletDT.Rows)
        //        {
        //            if (rowSubletDT["JobCode"].ToString().Trim() == ""
        //                | rowSubletDT["JobCode"].ToString().Trim() == ddlJobs_Sublet.SelectedValue.ToString().Trim())
        //            {
        //                Insert_in_SubletDT(rowSubletDT); search_result = true;
        //            }
        //        }
        //        if (search_result == false)
        //        {
        //            DataRow rowSubletDT = SubletDT.NewRow(); Insert_in_SubletDT(rowSubletDT); SubletDT.Rows.Add(rowSubletDT);
        //        }
        //        ViewState["SubLet"] = SubletDT; gvSublet.DataSource = SubletDT; gvSublet.DataBind();
        //        ClearJobSubletTextBoxes();
        //    }
        //    catch (Exception ex) { throw ex; }
        //}



        private void Insert_in_SubletDT(DataRow rowSubletDT)
        {
            //Insert_in_SubletDT(txtSubletVendor.Text, txtSubletVendor.Text, txtSubletJob.Text, txtSubletRemarks.Text,
            //txtSubletDate.Text, txtSubletIncAmnt.Text, txtSubletPayAmnt.Text, txtSubletJob.Text);

            rowSubletDT["VendorCode"] = ddlVendorCode.SelectedValue.ToString().Trim();
            rowSubletDT["Vendor"] = ddlVendorCode.SelectedItem.Text.Trim();
            rowSubletDT["JobCode"] = ddlJobs_Sublet.SelectedValue.ToString().Trim(); 
            rowSubletDT["JobDescription"] = txtSubletJobDesc.Text.Trim();
            rowSubletDT["Remarks"] = txtSubletRemarks.Text.Trim().ToUpper(); 
            rowSubletDT["SubletDate"] = txtSubletDate.Text.Trim();
            rowSubletDT["SubletAmount"] = txtSubletIncAmnt.Text.Trim(); 
            rowSubletDT["PayableAmount"] = txtSubletPayAmnt.Text;
        }


        private bool CheckRecInGrid(string ColumnName, string ColumnValue, string DTable, bool showMessage)
        {

            string SearchIt = ColumnName + " = '" + ColumnValue + "'";
            DataRow[] drp = JobDT.Select();
            switch (DTable)
            {
                case "Job": drp = JobDT.Select(SearchIt); break;
                case "Part": drp = PartsDT.Select(SearchIt); break;
                case "Lub": drp = LubDT.Select(SearchIt); break;
                case "Sublet": drp = SubletDT.Select(SearchIt); break;
                default: break;
            }
            if (drp.Length > 0)
            {
                updflag = (ViewState["ridx"] == null ? -1 : (int)ViewState["ridx"]); return true;
            }
            else { return false; }
        }

        
        protected void Button13_Click(object sender, EventArgs e)
        {

        }
        protected void gvJobCard_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ViewState["ridx"] = gvJobCard.SelectedRow.RowIndex;
                GridViewRow row = gvJobCard.Rows[gvJobCard.SelectedRow.RowIndex];
                ddlJobs_Labor.SelectedValue = row.Cells[2].Text.Trim();
                //txtJobDesc.Text = row.Cells[3].Text;

                ddlTechnicianEmpCode.SelectedValue = row.Cells[4].Text;
                //txtTechnician.Text = row.Cells[4].Text;
                //txtTechnicianDesc.Text = row.Cells[5].Text;
                //ddlJobCardType.Text = row.Cells[6].Text;
                txtJobRemarks.Text = (row.Cells[7].Text != "&nbsp;" ? row.Cells[7].Text : "");
                txtLabor.Text = row.Cells[8].Text;
                //  ViewState["deductAmount"] = decimal.Parse(row.Cells[8].Text);
            }
            catch (Exception ex) { throw ex; }

        }
        protected void gvJobCardParts_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ViewState["ridx"] = gvJobCardParts.SelectedRow.RowIndex;
            //GridViewRow row = gvJobCardParts.Rows[gvJobCardParts.SelectedRow.RowIndex];
            //ViewState["deductAmount"]  = row.Cells[1].Text;
            //txtPartCode.Text  = row.Cells[2].Text;
            //txtPartPrice.Text = row.Cells[7].Text;  
            //txtQuantity.Text  = row.Cells[6].Text;  
            try
            {
                ViewState["ridx"] = gvJobCardParts.SelectedRow.RowIndex;
                GridViewRow row = gvJobCardParts.Rows[gvJobCardParts.SelectedRow.RowIndex];
                ddlPartCode.SelectedValue = row.Cells[2].Text;
                txtPartItemNo_Parts.Text = row.Cells[3].Text.Trim();
                txtItemDesc.Text = row.Cells[4].Text;
                txtPartsRecQuantity.Text = row.Cells[7].Text;
                txtQuantity.Text = row.Cells[8].Text;
                txtPartPrice.Text = row.Cells[9].Text;
                ViewState["deductAmount"] = row.Cells[9].Text;

            }
            catch (Exception ex) { throw ex; }
        }
        protected void gvLubParts_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ViewState["ridx"] = gvLubParts.SelectedRow.RowIndex;
                GridViewRow row = gvLubParts.Rows[gvLubParts.SelectedRow.RowIndex];
                ViewState["deductAmount"] = (Convert.ToDecimal(row.Cells[5].Text) * Convert.ToDecimal(row.Cells[7].Text)).ToString();
                ddlLubCode.SelectedValue = row.Cells[2].Text;
                txtPartItemNo_Lubs.Text = row.Cells[3].Text.Trim();
                txtLubPartDesc.Text = row.Cells[4].Text;
                txtLubRecQuantity.Text = row.Cells[5].Text;
                txtLubQuantity.Text = row.Cells[6].Text;
                txtLubPrice.Text = row.Cells[7].Text;
            }
            catch (Exception ex) { throw ex; }
        }
        protected void gvSublet_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ViewState["ridx"] = gvSublet.SelectedRow.RowIndex;
                GridViewRow row = gvSublet.Rows[gvSublet.SelectedRow.RowIndex];
                //ViewState["deductAmount"] = row.Cells[7].Text;
                ddlVendorCode.SelectedValue = row.Cells[2].Text;
                ddlVendorCode.SelectedItem.Text = row.Cells[3].Text;

                ddlJobs_Sublet.SelectedValue = row.Cells[4].Text.Trim();
                txtSubletJobDesc.Text = row.Cells[5].Text;
                txtSubletRemarks.Text = row.Cells[6].Text;
                txtSubletDate.Text = row.Cells[7].Text;
                txtSubletIncAmnt.Text = row.Cells[8].Text;
                txtSubletPayAmnt.Text = row.Cells[9].Text;
            }
            catch (Exception ex) { throw ex; }


            //<asp:BoundField DataField="VendorCode" HeaderText="Vendor Code">
            //                        <ItemStyle Width="60px" />
            //                        </asp:BoundField>
            //                        <asp:BoundField DataField="Vendor" HeaderText="Vendor" />
            //                        <asp:BoundField DataField="JobCode" HeaderText="JobCode">
            //                        <ItemStyle Width="60px" />
            //                        </asp:BoundField>
            //                        <asp:BoundField DataField="JobDescription" HeaderText="Job Description" />
            //                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
            //                        <asp:BoundField DataField="SubletDate" HeaderText="Date" >
            //                        <ItemStyle Width="80px" />
            //                        </asp:BoundField>
            //                        <asp:BoundField DataField="SubletAmount" HeaderText="Invoiced Amt">
            //                        <ItemStyle Width="60px" />
            //                        </asp:BoundField>
            //                        <asp:BoundField DataField="PayableAmount" HeaderText="Payable Amt">
            //                        <ItemStyle Width="60px" />
            //                        </asp:BoundField>


        }

        
        
        



        protected void btnJobCardRemove_Click(object sender, EventArgs e)
        {
            //DataRow drr = JobDT.Rows[ridx]; // .Select("ID= " + Convert.ToInt32(ridx));
            //for (int i = 0; i < drr.Length; i++)
            //{
            JobDT = (DataTable)ViewState["Job"];
            JobDT.Rows.Remove(JobDT.Rows[ridx]);
            JobDT.AcceptChanges();
            ClearJobSubletTextBoxes();
            ViewState["Job"] = JobDT; gvJobCard.DataSource = JobDT; gvJobCard.DataBind();
        }
        protected void btnPartsRemove_Click(object sender, EventArgs e)
        {
            
            if ((ViewState["InvNo"] == null ? "" : ViewState["InvNo"].ToString()) == "")
            {
                PartsDT = (DataTable)ViewState["Parts"];
                PartsDT.Rows.Remove(PartsDT.Rows[ridx]);
                PartsDT.AcceptChanges();
                ClearPartsTextBoxes();
                ViewState["Parts"] = PartsDT; gvJobCardParts.DataSource = PartsDT; gvJobCardParts.DataBind();
            }
            else { lblMsg.Text = "Record can not be deleted after Invoice generated..."; }
        }
        protected void btnSubletRemove_Click(object sender, EventArgs e)
        {
            //DataRow[] drr = SubletDT.Select("ID= " + Convert.ToInt32(ridx));
            //for (int i = 0; i < drr.Length; i++)
            //{
            SubletDT = (DataTable)ViewState["SubLet"];
            SubletDT.Rows.Remove(SubletDT.Rows[ridx]);
            SubletDT.AcceptChanges();
            //txtSubletTotal.Text = (Convert.ToDecimal(txtSubletTotal.Text) - decimal.Parse(ViewState["deductAmount"].ToString())).ToString();
            //ViewState["d4"] = (ViewState["d4"] != null ? Convert.ToDecimal(ViewState["d4"].ToString()) : 0) - 1;
            //ViewState["deductAmount"] = 0;
            ClearJobSubletTextBoxes();
            //}
            ViewState["SubLet"] = SubletDT; gvSublet.DataSource = SubletDT; gvSublet.DataBind();

        }
        protected void txtLubRemove_Click(object sender, EventArgs e)
        {
            
            TextBox[] textBoxes = { txtLubPrice, txtLubQuantity };
            if (!MasterValidation(textBoxes)) return;
            else
            {

                if ((ViewState["InvNo"] == null ? "" : ViewState["InvNo"].ToString()) == "")
                {
                    //DataRow[] drr = LubDT.Select("RecQty < 1 and PartNo = " + txtLubPartCode.Text);
                    //for (int i = 0; i < drr.Length; i++)
                    //{
                    LubDT = (DataTable)ViewState["Lub"];
                    LubDT.Rows.Remove(LubDT.Rows[ridx]);
                    LubDT.AcceptChanges();
                    //txtLubTotal.Text = (Convert.ToDecimal(txtLubTotal.Text) - decimal.Parse(ViewState["deductAmount"].ToString())).ToString();
                    //  ViewState["d3"] = (ViewState["d3"] != null ? Convert.ToDecimal(ViewState["d3"].ToString()) : 0) - 1;
                    // ViewState["deductAmount"] = 0;
                    ClearLubricantsTextBoxes();
                    //    }
                    ViewState["Lub"] = LubDT; gvLubParts.DataSource = LubDT; gvLubParts.DataBind();
                }
                else { lblMsg.Text = "Record can not be deleted after Invoice generated..."; }
            }
        }

        #region    Image Button
        protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewState["lookupid"] = 1; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["txtgroup"] = 1;
                //clslook.CustomerLookup(txtRegNo, ViewState["lookupid"].ToString(), "", "../../../");
            }
            catch (Exception ex) { throw ex; }
        }
        //protected void ImageButton16_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        ViewState["lookupid"] = 17; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2;
        //        clslook.LU_Get_JobType(ImageButton16, ViewState["lookupid"].ToString(), "", "../../../");
        //    }   catch (Exception ex) { throw ex; }
        //}
        protected void ImageButton4_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewState["lookupid"] = 2; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2;
                //clslook.CustomerLookup(ImageButton4, ViewState["lookupid"].ToString(), "", "../../../");
            }
            catch (Exception ex) { throw ex; }
        }
        protected void ImageButton6_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                
                ViewState["lookupid"] = 3; ViewState["ixd1"] = 3; ViewState["ixd2"] = 2; ViewState["txtgroup"] = 1;
                //clslook.RegChassisEngine(txtChassisNo, ViewState["lookupid"].ToString(), "", "../../../");
            }
            catch (Exception ex) { throw ex; }
        }
        protected void ImageButton7_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewState["lookupid"] = 3; ViewState["txtgroup"] = 1; ViewState["ixd1"] = 2; ViewState["ixd2"] = 1;
                //clslook.RegChassisEngine(ddlEngineNo, ViewState["lookupid"].ToString(), "", "../../../");
            }
            catch (Exception ex) { throw ex; }
        }
        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewState["lookupid"] = 3; ViewState["txtgroup"] = 3; ViewState["ixd1"] = 2; ViewState["ixd2"] = 2;
                //clslook.RegChassisEngine(txtRegNo, ViewState["lookupid"].ToString(), "", "../../../");
            }
            catch (Exception ex) { throw ex; }

        }
        protected void ImageButton1_Click1(object sender, ImageClickEventArgs e)
        {

            Session["AppendWhere"] = "";
            Session["TableName"] = "JobCardMaster";
            Session["ColsName"] = @"JobCardCode as [Job Card],RegNo as [Reg No.],UserName as [Customer],JobCardType as [Job Card Type]";
            //lookupid = 4;
            //ViewState["ixd1"].ToString() = 1; ViewState["ixd2"].ToString() = 1;
            ShowLookUp();


        }
        protected void ImageButton12_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewState["lookupid"] = 10; ViewState["txtgroup"] = 2; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
               // clslook.LU_Get_Lubs(ImageButton1, ViewState["lookupid"].ToString(), "", "../../../");
            }
            catch (Exception ex) { throw ex; }
        }
        protected void ImageButton17_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewState["lookupid"] = 10; ViewState["txtgroup"] = 1; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
                //clslook.LU_Get_Parts(ImageButton1, ViewState["lookupid"].ToString(), "", "../../../");
            }
            catch (Exception ex) { throw ex; }
        }
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                
                ViewState["lookupid"] = 4; ViewState["ixd1"] = 1; ViewState["ixd2"] = 1;
                //clslook.LU_Get_JobCardNoForm(ImageButton1, ViewState["lookupid"].ToString(), "", "../../../");
            }
            catch (Exception ex) { throw ex; }
        }
        protected void ImageButton8_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewState["lookupid"] = 18; ViewState["ixd1"] = 1; ViewState["ixd2"] = 1;
                //clslook.LU_Get_ScheduleKM(ImageButton8, ViewState["lookupid"].ToString(), "", "../../../");
            }
            catch (Exception ex) { throw ex; }

        }
        //protected void ImageButton9_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        ViewState["lookupid"] = 19; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["txtgroup"] = 1;
        //        clslook.LU_Get_Advisor(ImageButton9, ViewState["lookupid"].ToString(), "", "../../../");
        //    }
        //    catch (Exception ex) { throw ex; }
        //}
        protected void ImageButton10_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewState["lookupid"] = 21; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["txtgroup"] = 1;
                //clslook.LU_Get_Job(ImageButton10, ViewState["lookupid"].ToString(), "", "../../../");
            }
            catch (Exception ex) { throw ex; }
        }
        //protected void ImageButton11_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        ViewState["lookupid"] = 19; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["txtgroup"] = 2;
        //        clslook.LU_Get_Technician(ImageButton11, ViewState["lookupid"].ToString(), "", "../../../");
        //    }
        //    catch (Exception ex) { throw ex; }
        //}
        protected void ImageButton17_Click1(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewState["lookupid"] = 10; ViewState["ixd1"] = 1; ViewState["ixd2"] = 4; ViewState["ixd3"] = 2; ViewState["txtgroup"] = 1;
                //clslook.LU_Get_Parts(, ViewState["lookupid"].ToString(), "", "../../../");
            }
            catch (Exception ex) { throw ex; }
        }
        protected void ImageButton15_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewState["lookupid"] = 21; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["txtgroup"] = 2;
                //clslook.LU_Get_Job(ImageButton10, ViewState["lookupid"].ToString(), "", "../../../");
            }
            catch (Exception ex) { throw ex; }

        }
        //protected void ImageButton13_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        ViewState["lookupid"] = 8; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["txtgroup"] = 2;
        //        clslook.LU_Get_Vendors(ImageButton10, ViewState["lookupid"].ToString(), "", "../../../");
        //    }
        //    catch (Exception ex) { throw ex; }

        //}
        #endregion

        protected void ShowLookUp()
        {

        }

        

        protected void gvJobCardParts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //e.Row.Cells[2].Visible = false;
            //e.Row.Cells[5].Visible = 
            //e.Row.Cells[6].Visible = 
            e.Row.Cells[11].Visible = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                countPartsRecQty = countPartsRecQty + int.Parse(e.Row.Cells[7].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[7].Text.Replace("&nbsp;", "").Trim());
                totParts = totParts + double.Parse(e.Row.Cells[10].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[10].Text.Replace("&nbsp;", "").Trim());
                countParts = countParts + int.Parse(e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim());
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                totParts = 0;
                countPartsRecQty = countParts = 0;
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
                lblPartsTotal.Text = totParts.ToString();
                txtPartTotalQuantity.Text = countParts.ToString();
                txttotPartsRecQty.Text = countPartsRecQty.ToString();
            }
        }
        protected void gvLubParts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //e.Row.Cells[2].Visible = false;  

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                countlubRecQty = int.Parse(e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim());
                countlub = countlub + int.Parse(e.Row.Cells[6].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[6].Text.Replace("&nbsp;", "").Trim());
                totlub = totlub + double.Parse(e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim());
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                totlub = 0; countlubRecQty = countlub = 0;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                lbltotLubQty.Text = countlub.ToString(); txtLubTotal.Text = totlub.ToString();
                txttotLubRecQty.Text = countlubRecQty.ToString();
            }

        }
        protected void gvSublet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //e.Row.Cells[9].Visible = false;
            e.Row.Cells[2].Visible = e.Row.Cells[4].Visible = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                totsubletInv = totsubletInv + double.Parse(e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim());
                totsubletPayable = totsubletPayable + double.Parse(e.Row.Cells[9].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[9].Text.Replace("&nbsp;", "").Trim());
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                totsubletInv = totsubletPayable = 0;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                txtSubletTotal.Text = totsubletInv.ToString(); txtSubletPayableTotal.Text = totsubletPayable.ToString();
            }
        }
        protected void gvJobCard_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //e.Row.Cells[2].Visible = false;
            // e.Row.Cells[4].Visible = e.Row.Cells[6].Visible = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                totLabour = totLabour + double.Parse(e.Row.Cells[8].Text.Replace("&nbsp;", "") == "" ? "0" : e.Row.Cells[8].Text);
                //countLabour = countLabour + e.Row.Cells[2].Text.Replace("&nbsp;", "") == "" ? 0 : 1;
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                countLabour = 0;
                totLabour = 0;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                lblJobCardTotal.Text = totLabour.ToString();
                txtJobsTotal.Text = countLabour.ToString();
            }
        }

        private void fillTextBox(string query)
        {
            DataSet ds = new DataSet(); myFunc.ExecuteQuery("", ref ds);

        }
        protected void btnLoadData_Click(object sender, EventArgs e)
        {
            LoadMasterData();
        }

        protected void LinkButton11_Click(object sender, EventArgs e)
        {

        }
        //protected void btnClear_Click(object sender, EventArgs e)
        //{
        //    clearAll();
        //}

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //grl = new General();
                //public bool CodeExists(string strTableNames, string strColumnCode, string strColumeValue)
                //CodeExists(string strTableNames, string strColumnCode, string strColumeValue)

                TextBox[] textBoxes = {txtCustomer, txtEndUser, txtBrand, txtProduct, txtVersion, 
                                txtReciptTime,txtKM,txtRecieptDate };
                if (!MasterValidation(textBoxes))
                {
                    return;
                }
                if (ddlAdvisorCode.SelectedIndex == 0)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Please select advisor", txtCustomer);
                    return;
                }
                
                //if (grl.IsExist("JobCardCode", ddlJobCardNo.Text, "JobCardMaster", " And GatePass<>''"))
                //{
                //    grl.UserMsg(lblMsg, Color.Red, "JobCard can not delete is has Gatpass", txtCustomer);
                //    return;
                //}
                if (txtGatePass.Text.Length !=0)
                {
                    grl.UserMsg(lblMsg, Color.Red, "JobCard can not delete is has Gatpass", txtCustomer);
                    return;
                }

                foreach (GridViewRow gvr in gvJobCardParts.Rows)
                {
                    if (gvr.Cells[6].Text != "&nbsp;")
                    {
                        if (Convert.ToInt32(gvr.Cells[6].Text) > 0)
                        {
                            grl.UserMsg(lblMsg, Color.Red, "Parts issued this JobCard not Delete", txtCustomer);
                            return;
                        }
                    }
                }
                foreach (GridViewRow gvr in gvLubParts.Rows)
                {
                    if (gvr.Cells[4].Text != "&nbsp;")
                    {
                        if (Convert.ToInt32(gvr.Cells[4].Text) > 0)
                        {
                            grl.UserMsg(lblMsg, Color.Red, "Lubricat issued this JobCard not Delete", txtCustomer);
                            return;
                        }
                    }
                }
                //SqlParameter[] JobCardDetail_Delete_param = {                                                       
                //                                                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                //                                                new SqlParameter("@JobCardCode",SqlDbType.Char,8),
                //                                            };
                //JobCardDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                //JobCardDetail_Delete_param[1].Value = ddlJobCardNo.Text;
                //myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_Detail_Delete", JobCardDetail_Delete_param, Trans);


                //SqlParameter[] JobCardPartsDetail_Delete_param = {                                                       
                //                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                //                                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                //                                                 };
                //JobCardPartsDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                //JobCardPartsDetail_Delete_param[1].Value = ddlJobCardNo.Text;
                //myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_PartsDetail_Delete", JobCardPartsDetail_Delete_param, Trans);

                //SqlParameter[] JobCardLubricanteDetail_Delete_param = {                                                       
                //                                                        new SqlParameter("@DealerCode",SqlDbType.Char,5),
                //                                                        new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                //                                                     };
                //JobCardLubricanteDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                //JobCardLubricanteDetail_Delete_param[1].Value = ddlJobCardNo.Text;
                //myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_LubricanteDetail_Delete", JobCardLubricanteDetail_Delete_param, Trans);    


                //SqlParameter[] JobCardSubletDetail_Delete_param = {                                                       
                //                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                //                                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                //                                                 };
                //JobCardSubletDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                //JobCardSubletDetail_Delete_param[1].Value = ddlJobCardNo.Text;
                //myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_Sublet_Delete", JobCardSubletDetail_Delete_param, Trans);    

                SqlParameter[] JobCard_Master_Delete_param = {                                                       
                                                                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                                                              };
                JobCard_Master_Delete_param[0].Value = Session["DealerCode"].ToString();
                JobCard_Master_Delete_param[1].Value = ddlJobCardCode.SelectedValue.ToString().Trim();
                if (myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_Master_Delete", JobCard_Master_Delete_param))
                {
                    
                    grl.UserMsg(lblMsg, Color.Green, "Record Deleted Successfully: " + ddlJobCardCode.SelectedValue.ToString().Trim(), txtCustomer);
                    clearAll();
                }
                else
                {
                    grl.UserMsg(lblMsg, Color.Red, "Record not deleted: " + ddlJobCardCode.SelectedValue.ToString().Trim(), txtCustomer);
                }

            }

            catch (Exception ex)
            {
                grl.UserMsg(lblMsg, Color.Red, ex.Message, txtCustomer);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            ////Page.ClientScript.RegisterStartupScript(this.GetType(), "myClick", "window.open('../../Service/ServiceReports/rptJobCardView.aspx?JobCardCode=" + ddlJobCardNo.Text.Trim() + "','ShowReport','scrollbars=yes,menubar=no,height=800,width=900,resizable=yes,toolbar=no,location=no,status=no');", true);
            //string URL = string.Empty;
            //URL = "../../Service/ServiceReports/rptViewerService.aspx?ReportID=JobCard&JobCardCode=" + ddlJobCardNo.Text.Trim();
            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);


            string PDFFileName = MakeReport();

            //string URL = "../../Download/OpenPdf.aspx?FileName=" + PDFFileName;
            ////URL = FilePath + "OpenPdf.aspx?FileName=" + FileName;
            ////txtPartItemDesc.Text = URL;
            //string fullsysURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullsysURL, true);
        }
        private string MakeReport()
        {
            ReportDocument rpt, crReportDocument, RD;
            string strCriteria, rptTitle;
            DateTime FromDate, ToDate;
            SqlDataReader rder;
            SysFunctions myFunc = new SysFunctions();
            DXBMS.Data.DataSet1 DSReports = new DXBMS.Data.DataSet1();


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
            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_JobCardMaster_Print '" + this.Session["DealerCode"].ToString() + "','" + ddlJobCardCode.SelectedValue.ToString().Trim() + "'");
            DSReports.sp_JobCardMaster_Print.Load(ds.CreateDataReader());

            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_JobCardDetail_Print'" + this.Session["DealerCode"].ToString() + "','" + ddlJobCardCode.SelectedValue.ToString().Trim() + "'");
            DSReports.sp_JobCardDetail_Print.Load(ds.CreateDataReader());

            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_JobCardLubDetail_Print'" + this.Session["DealerCode"].ToString() + "','" + ddlJobCardCode.SelectedValue.ToString().Trim() + "'");
            DSReports.sp_JobCardLubDetail_Print.Load(ds.CreateDataReader());

            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_JobCardPartsDetail_Print'" + this.Session["DealerCode"].ToString() + "','" + ddlJobCardCode.SelectedValue.ToString().Trim() + "'");
            DSReports.sp_JobCardPartsDetail_Print.Load(ds.CreateDataReader());

            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_JobCardSubletDetail_Print'" + this.Session["DealerCode"].ToString() + "','" + ddlJobCardCode.SelectedValue.ToString().Trim() + "'");
            DSReports.sp_JobCardSubletDetail_Print.Load(ds.CreateDataReader());

            string FileRptPath = Server.MapPath("~/Modules/Service/ServiceReports/");


            RD.Load(Server.MapPath("~/Modules/Service/ServiceReports/rptJobCard.rpt"));
            RD.OpenSubreport(Server.MapPath("~/Modules/Service/ServiceReports/rptJobCardDetail.rpt"));
            RD.OpenSubreport(Server.MapPath("~/Modules/Service/ServiceReports/JobCardPartsDetail.rpt"));
            RD.OpenSubreport(Server.MapPath("~/Modules/Service/ServiceReports/rptJobCardLubDetail.rpt.rpt"));
            RD.OpenSubreport(Server.MapPath("~/Modules/Service/ServiceReports/rptJobCardSubletDetail.rpt"));


            RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'JOB CARD'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
                

            // CrystalReportViewer1.ReportSource = rpt;
            RD.SetDataSource(DSReports.sp_JobCardMaster_Print.DataSet);

            Session["RDService"] = RD;

            // prepare pdf and show

            // create pdf 
            string FilePath = Server.MapPath("~/Download/");
            string FileName = "CounterSalesDetail" + Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
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


            //crReportDocument = new ReportDocument();
            //crReportDocument = (ReportDocument)Session["RDService"];

            ////CrystalReportViewer1.ReportSource = crReportDocument;
            ////return;

            //string FilePath = Server.MapPath("~/Download/");
            //string FileName = "JobCard" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            //string File = FilePath + FileName;
            ////Response.Write(File);
            ////crReportDocument.SetDatabaseLogon("SDMS", "sdms161", "192.168.1.47", "SDMS");
            //crReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, File);
            //RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            return FileName;
        }

        //protected void btnPost_Click(object sender, EventArgs e)
        //{
            //if (ddlJobCardCode.SelectedIndex == 0)
            //{
            //    grl.UserMsg(lblMsg, Color.Red, "jobCardNo should not be left blank", txtCustomer);
            //    return;
            //}
            //if (ddlJobType.SelectedItem.Value.ToString() != "001" && ddlJobType.SelectedItem.Value.ToString() != "002")
            //{
            //    
            //    grl.UserMsg(lblMsg, Color.Red, "Posting not allow, please make customer invoine against this jobcard No", txtCustomer);
            //    return;
            //}

            //ds = new DataSet();
            //grl.CodeExists("JobCardMaster", "JobCardCode='" + ddlJobCardCode.SelectedValue.ToString().Trim() + "' AND DealerCode='" + Session["DealerCode"].ToString() + "'", ref ds);
            //search_result = false;

            //if (ddlJobCardTypeCode.SelectedItem.Text == "Warranty")
            //{
            //    //double  dJCTotal  = 0.00;
            //    //string Query = 
            //    //if()
            //    //{

            //    //}
            //    if (grl.CodeExists("JobCardPartsDetail", "JobCardCode", ddlJobCardCode.SelectedValue.ToString().Trim(), " and qty <> recqty"))
            //    {
            //        SendAlert("Parts have not been issued yet"); return;
            //    }

            //    if (grl.CodeExists("JobCardLubricateDetail", "JobCardCode", ddlJobCardCode.SelectedValue.ToString().Trim(), " and qty <> recqty"))
            //    {
            //        SendAlert("Lubricant parts have not been issued yet"); return;
            //    }
            //}
            ////Check RecQuty and IssuQty is equal
            //string StrSIRNo = string.Empty;
            //DataSet dsSIR = new DataSet();
            //StrSIRNo = grl.GetStringValuesAgainstCodes("JobCardCode", ddlJobCardCode.SelectedValue.ToString().Trim(), "SIRMaster", "JobCardMaster");
            //myFunc.ExecuteQuery("Select * From SIRDetail Where Dealercode='" + Session["DealerCode"].ToString() + "' And SIRNo='" + StrSIRNo + "'", ref dsSIR);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow dr in dsSIR.Tables[0].Rows)
            //    {
            //        if (Convert.ToInt32(dr["Quantity"]) != Convert.ToInt32(dr["RecQty"]))
            //        {
            //            
            //            grl.UserMsg(lblMsg, Color.Red, "Stock not issue yet", txtCustomer);
            //            return;
            //        }
            //    }
            //}
            ///////////////////
            //if (ds.Tables[0].Rows[0]["JobCardType"].ToString().Trim() == "Repeated JobCard")
            //{
            //    callPostingJobCard(); search_result = true;
            //}
            //else if (ds.Tables[0].Rows[0]["JobTypeCode"].ToString().Trim() == "001")
            //{
            //    callPostingJobCard(); search_result = true;
            //    if (Create_NDM(ddlJobCardCode.SelectedValue.ToString().Trim(), "InvoiceCreated") == false)
            //    {
            //        grl.UserMsg(lblMsg, Color.Red, "NDM Data Can not be inserted");
            //        ObjTrans.RollBackTransaction(ref Trans);
            //        return;
            //    }
            //    if (PostServiceFollowUp(ddlJobCardCode.SelectedValue.ToString().Trim(), "InvoiceCreated") == false)
            //    {
            //        grl.UserMsg(lblMsg, Color.Red, "Post Service FollowUp Data Can not be inserted");
            //        ObjTrans.RollBackTransaction(ref Trans);
            //        return;
            //    }
            //}
            //else if (ds.Tables[0].Rows[0]["JobTypeCode"].ToString().Trim() == "002")
            //{
            //    callPostingJobCard(); search_result = true;

            //    if (Create_NDM(ddlJobCardCode.SelectedValue.ToString().Trim(), "InvoiceCreated") == false)
            //    {
            //        grl.UserMsg(lblMsg, Color.Red, "NDM Data Can not be inserted");
            //        ObjTrans.RollBackTransaction(ref Trans);
            //        return;
            //    }
            //    if (PostServiceFollowUp(ddlJobCardCode.SelectedValue.ToString().Trim(), "InvoiceCreated") == false)
            //    {
            //        grl.UserMsg(lblMsg, Color.Red, "Post Service FollowUp Data Can not be inserted");
            //        ObjTrans.RollBackTransaction(ref Trans);
            //        return;
            //    }
            //}

            //if (search_result == false) { SendAlert("Cannot post due to Job Type / Job Card Type!"); return; }
            //clearAll();
        //}

        private void callPostingJobCard()
        {
            SqlParameter[] dsParam = {                                                       
                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                                 };

            dsParam[0].Value = Session["DealerCode"].ToString();
            dsParam[1].Value = ddlJobCardCode.SelectedValue.ToString().Trim();

            dsJobCardParts = new DataSet();
            dsJobCardParts = myFunc.FillDataSet("sp_W2_JobCard_PartsDetail_Select", dsParam);
            if (dsJobCardParts.Tables[0].Rows.Count > 0) { SendAlert("Cannot Post due to parts entered!"); return; }

            dsJobCardLub = new DataSet();
            dsJobCardLub = myFunc.FillDataSet("sp_W2_JobCard_LubricanteDetail_Select", dsParam);
            if (dsJobCardLub.Tables[0].Rows.Count > 0) { SendAlert("Cannot Post due to lube parts entered!"); return; }

            string strSql = "Update dbo.JobCardMaster set GatePass='" + ddlJobCardCode.SelectedValue.ToString().Trim() +"'," +
                            "DelvDate='" + grl.SaveDate(DateTime.Now.ToString("dd/MM/yyyy")) + "'," +
                            "TransferStatus='E' " +
                            "Where DealerCode='" + Session["DealerCode"].ToString() + "' AND JobCardCode='" + ddlJobCardCode.SelectedValue.ToString().Trim() +"'";
            try
            {
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    if (myFunc.ExecuteQuery(strSql, Trans))
                    {
                        ObjTrans.CommittTransaction(ref Trans);
                        
                        grl.UserMsg(lblMsg, Color.Green, "JobCard Posted successfully " + ddlJobCardCode.SelectedValue.ToString().Trim());
                    }
                    else
                    {
                        ObjTrans.RollBackTransaction(ref Trans);
                        
                        grl.UserMsg(lblMsg, Color.Red, "JobCard dose not Post");
                    }
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                
                grl.UserMsg(lblMsg, Color.Red, ex.Message, txtCustomer);
            }

        }

        //Method for Next due Maintenance Posting on FFI and PDI jobcards.
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

        //Method For POsting PSF Data 
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
            if (myFunc.ExecuteSP_NonQuery("sp_CRM_Post_PostServiceFollowup_OnJobCardUpdate", MDNINV_param))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        protected void txtLabor_TextChanged(object sender, EventArgs e)
        {

        }
        protected void Button6_Click(object sender, EventArgs e)
        {

        }
        protected void LinkButton2_Click(object sender, EventArgs e)
        {

        }
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //if (Session["CheckRefresh"].ToString() == ViewState["CheckRefresh"].ToString())
        //        //    Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
        //        //else return;

        //        //if (JobCard_PDI_FFI())
        //        //{
        //        //    SendAlert("Job card for PDI, FFI cannot save parts or lubricant"); return;
        //        //}

        //        if (ddlJobCardTypeCode.SelectedItem.Text == "SFI"
        //            & double.Parse(lblJobCardTotal.Text == "" ? "0" : lblJobCardTotal.Text) != 0)
        //        {
        //            SendAlert("Labour Amount is not allowed for SFI job card!"); return;
        //        }
        //        if (ddlAdvisor.SelectedItem.Text.Trim() == "--Select--")
        //        {
        //            grl.UserMsg(lblMsg, Color.Red, "Please select Advisor", txtCustomer);
                    
        //            ddlAdvisor.BackColor = Color.Red;
        //            return;
        //        }
        //        if (ddlJobCardTypeCode.SelectedIndex == 0)
        //        {
        //            myFunc.UserMsg(lblMsg, Color.Red, "Select job card type first");
        //            ddlJobCardTypeCode.BackColor = Color.Red;
        //            return;
        //        }
        //        else
        //        {
        //            lblMsg.Text = string.Empty;
        //            ddlJobCardTypeCode.BackColor = Color.White;
        //        }
        //        if (ddlPayMode.SelectedIndex == 0)
        //        {
        //            grl.UserMsg(lblMsg, Color.Red, "Select Payment Mode");
        //            ddlPayMode.BackColor = Color.Red;
        //            return;
        //        }
        //        else
        //        {
        //            lblMsg.Text = string.Empty;
        //            ddlPayMode.BackColor = Color.White;
                    
        //        }
        //        if (ddlBillingType.SelectedIndex == 0)
        //        {
        //            grl.UserMsg(lblMsg, Color.Red, "Select Billing type");
        //            ddlBillingType.BackColor = Color.Red;
        //            return;
        //        }
        //        else
        //        {
        //            lblMsg.Text = string.Empty;
        //            ddlBillingType.BackColor = Color.White;
        //        }
        //        if(ddlVehRegNo .SelectedIndex ==0)
        //        {
        //            grl.UserMsg(lblMsg, Color.Red, "Select VehicleRegistration Number");
        //            ddlVehRegNo.BackColor = Color.Red;
        //            return;
        //        }
        //        else
        //        {
        //            lblMsg.Text = string.Empty;
        //            ddlBillingType.BackColor = Color.White;
        //        }
        //              if(ddlJobType.Text == "--Select--" || ddlAdvisor.Text == "--Select--")
        //        {
        //            grl.UserMsg(lblMsg, Color.Red, "Not all required field entered", txtCustomer);

        //            ddlPayMode.BackColor = Color.Red;
                    
        //            ddlAdvisor.BackColor = Color.Red;
                    
        //            txtKM.BackColor = Color.Red;

        //            return;
        //        }

        //        TextBox[] textBoxes = {  txtCustomer, txtEndUser, txtBrand, txtProduct, txtVersion ,
        //                            txtReciptTime,txtKM,txtRecieptDate};
        //        if (!MasterValidation(textBoxes)) return;
        //        else
        //        {
        //            // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Key", "var a=confirm('Test');if (a==true) {alert('Ok') ;} else{alert('Cancel')}", true);
        //            JobCardEntry();
        //            Load_ddlJobCardCode();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        grl.UserMsg(lblMsg, Color.Red, ex.Message, txtCustomer);
        //    }
        //}

        //private void DisableControls()
        //{
        //    btnSaveJC.Enabled = false;

        //    btnClearJC.Enabled = false;
        //    btnPrintJC.Enabled = false;
        //    btnPostJC.Enabled = false;
        //    btnAddinGrid.Enabled = false;
        //    txtClear.Enabled = false;
        //}
        //private void EnableControls()
        //{
        //    btnSaveJC.Enabled = true;

        //    btnClearJC.Enabled = true;
        //    btnPrintJC.Enabled = true;
        //    btnPostJC.Enabled = true;
        //    btnAddinGrid.Enabled = true;
        //    txtClear.Enabled = true;
        //}

        protected void lnkRemove_Click(object sender, EventArgs e)
        {
            try
            {

                if ((ViewState["InvNo"] == null ? "" : ViewState["InvNo"].ToString()) == "")
                {
                    PartsDT = (DataTable)ViewState["Parts"];
                    LinkButton btn = sender as LinkButton;
                    TableCell tc = btn.Parent as TableCell;
                    GridViewRow gvr = tc.Parent as GridViewRow;
                    if (gvr.Cells[7].Text == "0")
                    {
                        PartsDT.Rows.RemoveAt(gvr.RowIndex);
                    }
                    else
                    {
                        
                        grl.UserMsg(lblMsg, Color.Red, "Part can not remove because Receiving Quantity is not zero(0)...");
                        return;
                    }
                    //Load grid 
                    gvJobCardParts.DataSource = PartsDT;
                    PartsDT.AcceptChanges();
                    gvJobCardParts.DataBind();
                    //txtPartsTotal.Text = (Convert.ToDouble(gvr.Cells[10].Text.Trim()) - Convert.ToDouble(txtPartsTotal.Text.Trim())).ToString();
                    int sumParts = 0;

                    if (PartsDT.Rows.Count <= 0)
                    {
                        lblPartsTotal.Text = "0";
                        txtPartTotalQuantity.Text = "0";
                    }
                    else
                    {
                        foreach (DataRow dr in PartsDT.Rows)
                        {
                            sumParts = sumParts + Convert.ToInt32(dr["Qty"].ToString());
                        }
                        txtPartTotalQuantity.Text = sumParts.ToString();
                    }
                }
                else
                {
                    
                    grl.UserMsg(lblMsg, Color.Red, "Record can not be deleted after Invoice generated...");
                }
            }
            catch (Exception ex)
            {
                grl.UserMsg(lblMsg, Color.Red, ex.Message, txtCustomerDesc);
            }
        }
        protected void lnkRemove_Click1(object sender, EventArgs e)
        {
            try
            {
                JobDT = (DataTable)ViewState["Job"];
                LinkButton btn = sender as LinkButton;
                TableCell tc = btn.Parent as TableCell;
                GridViewRow gvr = tc.Parent as GridViewRow;
                JobDT.Rows.RemoveAt(gvr.RowIndex);
                //Load grid 
                gvJobCard.DataSource = JobDT;
                JobDT.AcceptChanges();
                gvJobCard.DataBind();
                if (JobDT.Rows.Count == 0)
                {
                    txtJobsTotal.Text = "0";
                    lblJobCardTotal.Text = "0";
                }
                else
                {
                    txtJobsTotal.Text = JobDT.Rows.Count.ToString();

                }

            }
            catch (Exception ex)
            {
                grl.UserMsg(lblMsg, Color.Red, ex.Message, txtCustomer);
            }
        }
        protected void lnkRemove_Click2(object sender, EventArgs e)
        {
            try
            {
                if ((ViewState["InvNo"] == null ? "" : ViewState["InvNo"].ToString()) == "")
                {

                    LubDT = (DataTable)ViewState["Lub"];
                    LinkButton btn = sender as LinkButton;
                    TableCell tc = btn.Parent as TableCell;
                    GridViewRow gvr = tc.Parent as GridViewRow;
                    if (gvr.Cells[5].Text == "0")
                    {
                        LubDT.Rows.RemoveAt(gvr.RowIndex);
                    }
                    else
                    {
                        
                        grl.UserMsg(lblMsg, Color.Red, "Can not Remove because Receiving Quantity not zero(0)...");
                        return;
                    }
                    //Load grid 
                    gvLubParts.DataSource = LubDT;
                    LubDT.AcceptChanges();
                    gvLubParts.DataBind();
                    if (LubDT.Rows.Count == 0)
                    {
                        txtLubTotal.Text = "0";
                        lbltotLubQty.Text = "0";
                    }
                }
                else
                {
                    
                    grl.UserMsg(lblMsg, Color.Red, "Record can not be deleted after Invoice generated...");
                }
            }
            catch (Exception ex)
            {
                grl.UserMsg(lblMsg, Color.Red, ex.Message, txtCustomer);
            }
        }
        protected void lnkRemove_Click3(object sender, EventArgs e)
        {
            try
            {
                SubletDT = (DataTable)ViewState["SubLet"];
                LinkButton btn = sender as LinkButton;
                TableCell tc = btn.Parent as TableCell;
                GridViewRow gvr = tc.Parent as GridViewRow;
                SubletDT.Rows.RemoveAt(gvr.RowIndex);
                //Load grid 
                gvSublet.DataSource = SubletDT;
                SubletDT.AcceptChanges();
                gvSublet.DataBind();
                if (SubletDT.Rows.Count == 0)
                {
                    txtSubletTotal.Text = "0";
                    txtSubletPayableTotal.Text = "0";
                }
            }
            catch (Exception ex)
            {
                grl.UserMsg(lblMsg, Color.Red, ex.Message, txtCustomer);
            }
        }


        
        protected void btnDelete_Click1(object sender, EventArgs e)
        {

            if (ddlJobCardCode.SelectedIndex  == 0)
            {
                grl.UserMsg(lblMsg, Color.Red, "Jobcard code should not be left blank!!!");
                
                return;
            }
            //if (grl.CodeExists("JobCardMaster", "JobCardCode", ddlJobCardNo.Text) == false)
            //{
            //    grl.UserMsg(lblMsg, Color.Red, "Job card code not exist");
            //    
            //    return;
            //}
            //////Discuss
            //if (grl.CodeExists("JobCardMaster", "JobCardCode", ddlJobCardNo.Text, " And TransferStatus='E'"))
            //{
            //    grl.UserMsg(lblMsg, Color.Red, "Jobcard can not be deleted,because it has been Transferred to PSMC ");
            //    
            //    return;
            //}
            // Check TransferStatus is not C then should not be deleted
            if (grl.CodeExists("JobEstimateMaster", "JobCardCode", ddlJobCardCode.SelectedValue.ToString().Trim(), " And DelFlag='N'"))
            {
                grl.UserMsg(lblMsg, Color.Red, "Jobcard exist in Job Estimate ,it can not be deleted ");
                
                return;
            }
            //PPR made or not
            ///discuss
            if (grl.CodeExists("PPRMaster", "JobCardCode", ddlJobCardCode.SelectedValue.ToString().Trim(), " And DelFlag = 'N'"))
            {
                grl.UserMsg(lblMsg, Color.Red, "Jobcard exist in PPR ,it can not be deleted ");
                
                return;
            }
            //if (grl.IsExist("JobCardNo", ddlJobCardNo.Text, "StockIssue"))
            //{
            //    grl.UserMsg(lblMsg, Color.Red, "Stock Issued ,it can not be deleted ");
            //    
            //    return;
            //}
            //Check Parts data item stock quantity not issued


            if (!string.IsNullOrEmpty(lblSIRMaster.Text))
            {
                short i = 0;
                short j = 0;
                short k = 0;
                //for (i = 0; i <= gvJobCardParts.Rows.Count - 1; i++)
                //{

                foreach (DataRow drPart in PartsDT.Rows)
                {
                    if (drPart["DealerCode"] != "")
                    {
                        txtPartsRecQuantity.Text = GetSIRParts(lblSIRMaster.Text, drPart["ItemCode"].ToString());
                        if (string.IsNullOrEmpty(txtPartsRecQuantity.Text) == false && Convert.ToInt32(txtPartsRecQuantity.Text) > 0)
                        {
                            grl.UserMsg(lblMsg, Color.Red, "Item can not be deleted, because some Parts item quantity has been issued!!! ", txtCustomer);
                            
                            return;
                        }
                    }
                }
                //for (i = 0; i <= SuppPartsGrid.Items.Count - 1; i++)
                //{
                //    lblSupPartBal.Text = GetSupSIRParts(lblSIRMaster.Text, SuppPartsGrid.Items(i).SubItems.Item(0).Text);
                //    if (lblSupPartBal.Text > 0)
                //    {
                //        MessageBox.Show("Item can not be deleted, because some Supplementary Parts item quantity has been issued!!! ", MESSAGE_BOX_HEADING, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        txtCode.Select();
                //        return;
                //    }
                //}

                foreach (DataRow drLub in LubDT.Rows)
                {
                    if (drLub["DealerCode"] != "")
                    {
                        txtLubRecQuantity.Text = GetSIRLubs(lblSIRMaster.Text, drLub["ItemCode"].ToString());
                        if (txtLubRecQuantity.Text == "")
                        {
                            txtLubRecQuantity.Text = "0";
                        }
                        if (Convert.ToInt32(txtLubRecQuantity.Text) > 0)
                        {
                            grl.UserMsg(lblMsg, Color.Red, "Item can not be deleted, because some lubricants item quantity has been issued!!! ", txtCustomer);
                            
                            return;
                        }
                    }
                }

                //for (k = 0; k <= PartsGridCon.Items.Count - 1; k++)
                //{
                //    lblPartBalCon.Text = GetSIRCon(lblSIRMaster.Text, PartsGridCon.Items(k).SubItems.Item(0).Text);
                //    if (lblPartBalCon.Text > 0)
                //    {
                //        MessageBox.Show("Item can not be deleted, because some Consumable item quantity has been issued!!! ", MESSAGE_BOX_HEADING, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        txtPartCodeCon.Select();
                //        return;
                //    }
                //}



            }
            else
            {
                grl.UserMsg(lblMsg, Color.Red, "Stock issued information could not found ,because it has been Transferred to PSMC ");
                
                return;
            }
            /////////////////////////////////////////////
            if (Delete(ddlJobCardCode.SelectedValue.ToString().Trim(), lblSIRMaster.Text) == false)
            {
                grl.UserMsg(lblMsg, Color.Red, "Job Card code not deleted ");
                
                return;
            }
            else
            {
                grl.UserMsg(lblMsg, Color.Green, "Job Card deleted successfully ");
                
                clearAll();
            }
        }
        public bool Delete(string strCode, string strSIRMaster)
        {
            //#BEGIN TRANSACTION
            SqlTransaction stDeleteJobCard = null;
            //#IF TRANSACTION COULD'NT BE STARTED RETURN FALSE
            if ((!ObjTrans.BeginTransaction(ref stDeleteJobCard)))
            {
                return false;
            }

            //1. Only JobCardMaster update value wiht DelFalg Y
            string sJobCardMasterUpdateQuery = "Update JobCardMaster Set DelFlag = 'Y',UpdUser = '" + Session["UserName"].ToString() + "',UpdDate = '" + grl.SaveDate(DateTime.Now.ToString("dd/MM/yyyy")) + "',UpdTime = '" + DateTime.Now.ToString("HH:mm:ss") + "', UpdTerm = '" + Environment.MachineName + "' Where JobCardCode='" + strCode + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
            try
            {
                if ((!myFunc.ExecuteQuery_NonQuery(sJobCardMasterUpdateQuery, stDeleteJobCard)))
                {
                    //IF THE step return false exit from the function
                    ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                grl.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);
                
                return false;
            }

            //2. DELETE FROM JobCardDetail
            string sJobCardDetailDeleteQuery = "DELETE FROM JobCardDetail Where JobCardCode='" + strCode + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
            try
            {
                if ((!myFunc.ExecuteQuery_NonQuery(sJobCardDetailDeleteQuery, stDeleteJobCard)))
                {
                    //IF THE step return false exit from the function
                    ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                grl.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);
                
                return false;
            }

            //3. DELETE FROM JobCardPartsDetail
            string sJobCardPartDeleteQuery = "DELETE FROM JobCardPartsDetail Where JobCardCode='" + strCode + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
            try
            {
                if ((!myFunc.ExecuteQuery_NonQuery(sJobCardPartDeleteQuery, stDeleteJobCard)))
                {
                    //IF THE step return false exit from the function
                    ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                grl.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);
                
                return false;
            }

            //4.DELETE FROM JobCardLubricateDetail
            string sJobCardLubDeleteQuery = "DELETE FROM JobCardLubricateDetail Where JobCardCode='" + strCode + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
            try
            {
                if ((!myFunc.ExecuteQuery_NonQuery(sJobCardLubDeleteQuery, stDeleteJobCard)))
                {
                    //IF THE step return false exit from the function
                    ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                grl.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);
                
                return false;
            }
            //5.DELETE FROM JobCardSubletDetail
            string sJobCardSubletDeleteQuery = "DELETE FROM JobCardSublet Where JobCardCode='" + strCode + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
            try
            {
                if ((!myFunc.ExecuteQuery_NonQuery(sJobCardLubDeleteQuery, stDeleteJobCard)))
                {
                    //IF THE step return false exit from the function
                    ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                grl.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);
                
                return false;
            }
            //Special Case: Update SIRMaster
            string sSIRMasterUpdateQuery = "Update SIRMaster Set DelFlag = 'Y' Where SIRNo='" + strSIRMaster + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
            try
            {
                if ((!myFunc.ExecuteQuery_NonQuery(sSIRMasterUpdateQuery, stDeleteJobCard)))
                {
                    //IF THE step return false exit from the function
                    ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                grl.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);
                
                return false;
            }


            //Special Case for SIDetail
            string sSIRDetailQuery = "DELETE FROM SIRDetail Where SIRNO='" + strSIRMaster + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
            try
            {
                if ((!myFunc.ExecuteQuery_NonQuery(sSIRDetailQuery, stDeleteJobCard)))
                {
                    //IF THE step return false exit from the function
                    ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                grl.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);
                
                return false;
            }


            ////5. DELETE FROM JobCardRequiredDetail
            //string sJobCardReqDeleteQuery = "DELETE FROM JobCardReqDetail Where JobCardCode='" + strCode + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
            //try
            //{
            //    if ((!myFunc.ExecuteQuery(sJobCardReqDeleteQuery, stDeleteJobCard)))
            //    {
            //        //IF THE step return false exit from the function
            //        ObjTrans.RollBackTransaction(ref stDeleteJobCard);
            //        return false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ObjTrans.RollBackTransaction(ref stDeleteJobCard);
            //    grl.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);
            //    return false;
            //}

            ////6. DELETE FROM JobCardRecChlist
            //string sJobCardRecDeleteQuery = "DELETE FROM JobCardRecChkList Where JobCardCode='" + strCode + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
            //try
            //{
            //    if ((!myFunc.ExecuteQuery(sJobCardRecDeleteQuery, stDeleteJobCard)))
            //    {
            //        //IF THE step return false exit from the function
            //        ObjTrans.RollBackTransaction(ref stDeleteJobCard);
            //        return false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ObjTrans.RollBackTransaction(ref stDeleteJobCard);
            //    grl.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);
            //    return false;
            //}

            ////7. DELETE FROM JobCardReturnChlist
            //string sJobCardRetDeleteQuery = "DELETE FROM JobCardRetChkList Where JobCardCode='" + strCode + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
            //try
            //{
            //    if ((!objMBLL.ExecuteQuery(sJobCardRetDeleteQuery, stDeleteJobCard)))
            //    {
            //        //IF THE step return false exit from the function
            //        ObjTrans.RollBackTransaction(ref stDeleteJobCard);
            //        return false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ObjTrans.RollBackTransaction(ref stDeleteJobCard);
            //    grl.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);
            //    return false;
            //}

            ////8 Delete form JobCardTech

            //string sJobCardTechDeleteQuery = "DELETE FROM JobCardTech Where JobCardCode='" + strCode + "' and DealerCode = '" + DEALER_CODE + "'";
            //try
            //{
            //    if ((!daDataAccess.ExecuteQuery(sJobCardTechDeleteQuery, null, stDeleteJobCard)))
            //    {
            //        //IF THE step return false exit from the function
            //        daDataAccess.RollBackTransaction(stDeleteJobCard);
            //        return false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    daDataAccess.RollBackTransaction(stDeleteJobCard);
            //    clsLogger.WriteLog(ex.Message, ex.Source);
            //    return false;
            //}

            ////9. Only JobCardMaster update value wiht DelFalg Y
            //string sJobCardClubSuzukiUpdateQuery = "Update ClubGSTInvoice Set DelFlag = 'Y' Where JobCardCode='" + strCode + "' and DealerCode = '" + DEALER_CODE + "'";
            //try
            //{
            //    if ((!daDataAccess.ExecuteQuery(sJobCardClubSuzukiUpdateQuery, null, stDeleteJobCard)))
            //    {
            //        //IF THE step return false exit from the function
            //        daDataAccess.RollBackTransaction(stDeleteJobCard);
            //        return false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    daDataAccess.RollBackTransaction(stDeleteJobCard);
            //    clsLogger.WriteLog(ex.Message, ex.Source);
            //    return false;
            //}

            ////10. DELETE FROM JobCardConDetail
            //string sJobCardPartConDeleteQuery = "DELETE FROM JobCardConDetail Where JobCardCode='" + strCode + "' and DealerCode = '" + DEALER_CODE + "'";
            //try
            //{
            //    if ((!daDataAccess.ExecuteQuery(sJobCardPartConDeleteQuery, null, stDeleteJobCard)))
            //    {
            //        //IF THE step return false exit from the function
            //        daDataAccess.RollBackTransaction(stDeleteJobCard);
            //        return false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    daDataAccess.RollBackTransaction(stDeleteJobCard);
            //    clsLogger.WriteLog(ex.Message, ex.Source);
            //    return false;
            //}


            //#COMMIT TRANSACTION
            ObjTrans.CommittTransaction(ref stDeleteJobCard);

            //For CRM
            //Update_CRM_PostSales_NextDueMaintenance(strCode, "InvoiceDeleted");
            //Update_CRM_Post_PostServiceFollowup(strCode, "InvoiceDeleted");

            return true;


        }
        public string GetSIRLubs(string SIRNo, string strItemCode)
        {
            string functionReturnValue = null;

            SqlDataReader drParts = null;

            if (GetSIRLubsDetail(ref drParts, SIRNo, strItemCode) == true)
            {
                try
                {
                    functionReturnValue = drParts["RecQty"].ToString();
                    drParts.Close();
                }
                catch (Exception ex)
                {
                    drParts.Close();
                    grl.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);
                }
            }
            return functionReturnValue;

        }
        public bool GetSIRLubsDetail(ref SqlDataReader drParts, string strSIRNo, string strItemCode)
        {
            //Step1: create query
            string selectQuery = "SELECT * FROM SIRDetail WHERE SIRNo = '" + strSIRNo + "' AND ItemCode='" + strItemCode + "' AND DealerCode = '" + Session["DealerCode"].ToString() + "' and Type='L'";

            //Step2: execute query
            try
            {
                //if (drParts == null)
                //{
                    
                //    drParts.Close();
                //}
                bool bResult = myFunc.ExecuteQuery(selectQuery, ref drParts);

                if ((bResult))
                {
                    //Check number of records
                    if ((drParts.HasRows))
                    {
                        //Open the reader to read if it can be read
                        return drParts.Read();
                    }
                    else
                    {
                        drParts.Close();
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                grl.UserMsg(lblMsg, Color.Red, "Error: " + e.Message);
                drParts.Close();
                return false;
            }
        }
        public string GetSIRParts(string SIRNo, string strItemCode)
        {
            string functionReturnValue = null;
            //clsJobCard ObjJobCard = new clsJobCard();
            SqlDataReader drParts = null;
            if (GetSIRPartsDetail(ref drParts, SIRNo, strItemCode) == true)
            {
                try
                {
                    functionReturnValue = drParts["RecQty"].ToString();
                    drParts.Close();
                }
                catch (Exception ex)
                {
                    drParts.Close();
                    grl.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);
                }
            }
            return functionReturnValue;
        }
        public bool GetSIRPartsDetail(ref SqlDataReader drParts, string strSIRNo, string strItemCode)
        {
            //Step1: create query
            string selectQuery = "SELECT * FROM SIRDetail WHERE SIRNo = '" + strSIRNo + "' AND ItemCode='" + strItemCode + "' AND DealerCode = '" + Session["DealerCode"].ToString() + "' and Type in ('A','P')";

            //Step2: execute query
            try
            {
                bool bResult = myFunc.ExecuteQuery(selectQuery, ref drParts);
                if ((bResult))
                {
                    //Check number of records
                    if ((drParts.HasRows))
                    {
                        //Open the reader to read if it can be read
                        return drParts.Read();
                    }
                    else
                    {
                        drParts.Close();
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                grl.UserMsg(lblMsg, Color.Red, "Error: " + e.Message);
                drParts.Close();
                return false;
            }
        }
        public static void SendAlert(string sMessage)
        {
            sMessage = "alert('" + sMessage.Replace("'", @"\'").Replace("\n", @"\n") + "');";

            if (HttpContext.Current.CurrentHandler is Page)
            {
                Page p = (Page)HttpContext.Current.CurrentHandler;

                if (ScriptManager.GetCurrent(p) != null)
                {
                    ScriptManager.RegisterStartupScript(p, typeof(Page), "Message", sMessage, true);
                }
                else
                {
                    p.ClientScript.RegisterStartupScript(typeof(Page), "Message", sMessage, true);
                }
            }
        }

        // New Codes
        protected void PrepareNewJobCard()
        {
            //ddlJobCardNo.Text = "";
            //ddlJobCardNo.Enabled = false;
            btnSaveJC.Enabled = true;
            btnDeleteJC.Enabled = false;
            btnPostJC.Enabled = false;
            btnPrintJC.Enabled = false;
            //TabContainer1.Enabled = true;

            
        }
        protected void ddlJobCardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PrepareNewJobCard();
            //if (ddlJobCardType.BackColor == Color.Red)
            //{
            //    ddlJobCardType.BackColor = Color.White;
            //}
        }

        

        //protected void ddlBilling_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    PrepareNewJobCard();
        //    if (ddlBilling.BackColor == Color.Red)
        //    {
        //        ddlBilling.BackColor = Color.White;
        //    }
        //}

        protected void ddlRegNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void ddlChassisNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void ddlEngineNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void txtJob_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        //protected void ddlJobCardNo_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    LoadMasterData();
        //    btnSaveJC.Enabled = true;
        //    btnDeleteJC.Enabled = true;
        //    btnPostJC.Enabled = true;
        //    btnPrintJC.Enabled = true;
        //    //TabContainer1.Enabled = true;
        //}

        //protected void txtPartCode_TextChanged(object sender, EventArgs e)
        //{
        //    if (txtPartCode.Text == "")
        //    {
        //        return;
        //    }
        //    txtPartDesc.Text = myFunc.GetItemDescByItemCode(myFunc.GetItemCodeByPartItemNo(txtPartCode.Text));
        //    txtItemcodeParts.Text = myFunc.GetItemCodeByPartItemNo(txtPartCode.Text);
        //    txtPartPrice.Text = myFunc.GetItemPriceByPartItemNo(txtPartCode.Text);
        //}

        //protected void txtLubPartCode_TextChanged(object sender, EventArgs e)
        //{
        //    txtLubPartDesc.Text = myFunc.GetItemDescByItemCode(myFunc.GetItemCodeByPartItemNo(txtLubPartCode.Text));
        //    txtItemcodeLub.Text = myFunc.GetItemCodeByPartItemNo(txtLubPartCode.Text);
        //    txtLubPrice.Text = myFunc.GetItemPriceByPartItemNo(txtLubPartCode.Text);
        //}

        //protected void txtSubletJob_TextChanged(object sender, EventArgs e)
        //{
        //    DataTable dt = myFunc.GetData("select DefJobDesc from DefaultJob where DefJobCode='"+txtSubletJob.Text+"'");
        //    txtSubletJobDesc.Text = dt.Rows[0]["DefJobDesc"].ToString();
        //   // Session["SubletJobDesc"] = dt.Rows[0]["DefJobDesc"].ToString();
        //    dt.Dispose();
        //}

       

        

        
        private void Insert_in_ConPartsDT(DataRow rowConPartsDT)
        {

            rowConPartsDT["PartNo"] = txtPartItemNoConParts.Text.Trim(); rowConPartsDT["PartsDesc"] = txtConPartDesc.Text.Trim();
            

            rowConPartsDT["Qty"] = txtConQuantity.Text.Trim(); rowConPartsDT["Price"] = txtConPartPrice.Text.Trim();
            rowConPartsDT["Type"] = "P";
            rowConPartsDT["Total"] = double.Parse(txtConQuantity.Text.Trim()) * double.Parse(txtConPartPrice.Text.Trim());
            rowConPartsDT["ItemCode"] = ddlConsumableParts.SelectedValue.ToString().Trim();
            rowConPartsDT["RecQty"] = txtConPartsRecQuantity.Text.Trim();
        }

        protected void btnConPartsClear_Click(object sender, EventArgs e)
        {
            ClearConPartsTextBoxes();
        }

        protected void lnkRemove_Click4(object sender, EventArgs e)
        {
            try
            {

                if ((ViewState["InvNo"] == null ? "" : ViewState["InvNo"].ToString()) == "")
                {
                    ConPartsDT = (DataTable)ViewState["ConParts"];
                    LinkButton btn = sender as LinkButton;
                    TableCell tc = btn.Parent as TableCell;
                    GridViewRow gvr = tc.Parent as GridViewRow;
                    if (gvr.Cells[5].Text == "0")
                    {
                        ConPartsDT.Rows.RemoveAt(gvr.RowIndex);
                    }
                    else
                    {
                        
                        grl.UserMsg(lblMsg, Color.Red, "Part can not remove because Receiving Quantity is not zero(0)...");
                        return;
                    }
                    //Load grid 
                    gvJobCardConParts.DataSource = ConPartsDT;
                    ConPartsDT.AcceptChanges();
                    gvJobCardConParts.DataBind();
                    //txtPartsTotal.Text = (Convert.ToDouble(gvr.Cells[10].Text.Trim()) - Convert.ToDouble(txtPartsTotal.Text.Trim())).ToString();
                    int sumParts = 0;

                    if (PartsDT.Rows.Count <= 0)
                    {
                        txtConPartsTotal.Text = "0";
                        txtConPartTotalQuantity.Text = "0";
                    }
                    else
                    {
                        foreach (DataRow dr in PartsDT.Rows)
                        {
                            sumParts = sumParts + Convert.ToInt32(dr["Qty"].ToString());
                        }
                        txtConPartTotalQuantity.Text = sumParts.ToString();
                    }
                }
                else
                {
                    
                    grl.UserMsg(lblMsg, Color.Red, "Record can not be deleted after Invoice generated...");
                }
            }
            catch (Exception ex)
            {
                grl.UserMsg(lblMsg, Color.Red, ex.Message, txtCustomerDesc);
            }
        }

        

        protected void gvJobCardConParts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
             e.Row.Cells[9].Visible = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                countConPartsRecQty = countConPartsRecQty + int.Parse(e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim());
                totParts = totParts + double.Parse(e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim());
                countParts = countParts + int.Parse(e.Row.Cells[6].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[6].Text.Replace("&nbsp;", "").Trim());
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                totConParts = 0;
                countConPartsRecQty = countConParts = 0;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                txtConPartsTotal.Text = totParts.ToString();
                txttotConPartsRecQty.Text = countConPartsRecQty.ToString();
                txtConPartTotalQuantity.Text = countParts.ToString();
            }
        }

        protected void gvJobCardConParts_SelectedIndexChanged(object sender, EventArgs e)
        {
           try
            {
                ViewState["ridx"] = gvJobCardConParts.SelectedRow.RowIndex;
                GridViewRow row = gvJobCardConParts.Rows[gvJobCardConParts.SelectedRow.RowIndex];
                //txtConPartCode.Value = row.Cells[2].Text.Trim();
                //txtConPartCode.Text = row.Cells[2].Text.Trim();
                ddlConsumableParts.SelectedValue = row.Cells[2].Text;
                txtPartItemNoConParts.Text= row.Cells[3].Text.Trim();
                txtConPartDesc.Text = row.Cells[4].Text;
                txtConPartsRecQuantity.Text = row.Cells[5].Text;
                txtConQuantity.Text = row.Cells[6].Text;
                txtConPartPrice.Text = row.Cells[7].Text;
                ViewState["deductAmount"] = row.Cells[7].Text;

            }
            catch (Exception ex) { throw ex; }
        }

        protected void btn(object sender, EventArgs e)
        {

        }

        protected void ddlJobCartType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlJobCardType_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }

        protected void dlBilling_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //protected void txtJobNew_TextChanged(object sender, EventArgs e)
        //{
        //    DataTable dt = myFunc.GetData("select DefJobDesc from DefaultJob where DefJobCode='" + ddlJobs.SelectedValue.ToString() + "'");
        //    txtJobDesc.Text = dt.Rows[0]["DefJobDesc"].ToString();

        //    dt.Dispose();
        //}

        protected void txtColor_TextChanged(object sender, EventArgs e)
        {
            
        }

        protected void txtKM_TextChanged(object sender, EventArgs e)
        {
            if (txtKM.BackColor == Color.Red)
            {
                txtKM.BackColor = Color.White;
            }
        }

        //protected void ddlAdvisor_TextChanged(object sender, EventArgs e)
        //{
        //    if (ddlAdvisor.BackColor == Color.Red)
        //    {
        //        ddlAdvisor.BackColor = Color.White;
        //    }
        //}

        //protected void ddlRegNo_TextChanged(object sender, EventArgs e)
        //{
            //DataSet dsVehInfo = new DataSet();
            //SqlParameter[] param = {                                 
            //                        new SqlParameter("@DealerCode",SqlDbType.VarChar,5), 
            //                        new SqlParameter("@RegNo",SqlDbType.VarChar,30)
            //                   };


            //param[0].Value = Session["DealerCode"].ToString();
            //param[1].Value = ddlVehRegNo.SelectedValue.ToString().Trim(); // ddlRegNo.Text.Trim();//(HFRegNo.Value == "" ? null : HFRegNo.Value);

            //dsVehInfo = myFunc.FillDataSet("sp_W2_CustomerVehicle_Select", param);
            //if (dsVehInfo.Tables[0].Rows.Count > 0)
            //{
            //    setVehcileInfo(dsVehInfo);
            //}
            //PrepareNewJobCard();

            //if (ddlRegNo.BackColor == Color.Red)
            //{
            //    ddlRegNo.BackColor = Color.White;
            //}
        //}

        //protected void ddlChassisNo_TextChanged(object sender, EventArgs e)
        //{
        //    DataSet dsVehInfo = new DataSet();
        //    SqlParameter[] param = {                                 
        //                            new SqlParameter("@DealerCode",SqlDbType.VarChar,5), 
        //                            new SqlParameter("@RegNo",SqlDbType.VarChar,30)
        //                       };

        //    string regno = myFunc.getRegNoByChassisNo(txtChassisNo.Text.Trim());
        //    param[0].Value = Session["DealerCode"].ToString();
        //    param[1].Value = regno.Trim(); // ddlRegNo.Text.Trim();//(HFRegNo.Value == "" ? null : HFRegNo.Value);

        //    dsVehInfo = myFunc.FillDataSet("sp_W2_CustomerVehicle_Select", param);
        //    if (dsVehInfo.Tables[0].Rows.Count > 0)
        //    {
        //        setVehcileInfo(dsVehInfo);
        //    }
        //    PrepareNewJobCard();

        //    if (ddlChassisNo.BackColor == Color.Red)
        //    {
        //        ddlChassisNo.BackColor = Color.White;
        //    }
        //}

        //protected void ddlEngineNo_TextChanged(object sender, EventArgs e)
        //{
        //    DataSet dsVehInfo = new DataSet();
        //    SqlParameter[] param = {                                 
        //                            new SqlParameter("@DealerCode",SqlDbType.VarChar,5), 
        //                            new SqlParameter("@RegNo",SqlDbType.VarChar,30)
        //                       };

        //    string regno = myFunc.getRegNoByEngineNo(txtEngineNo.Text.Trim());
        //    param[0].Value = Session["DealerCode"].ToString();
        //    param[1].Value = regno.Trim(); // ddlRegNo.Text.Trim();//(HFRegNo.Value == "" ? null : HFRegNo.Value);

        //    dsVehInfo = myFunc.FillDataSet("sp_W2_CustomerVehicle_Select", param);
        //    if (dsVehInfo.Tables[0].Rows.Count > 0)
        //    {
        //        setVehcileInfo(dsVehInfo);
        //    }
        //    PrepareNewJobCard();

        //    if (ddlEngineNo.BackColor == Color.Red)
        //    {
        //        ddlEngineNo.BackColor = Color.White;
        //    }
        //}

        protected void ddlVehRegNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet dsVehInfo = new DataSet();
            SqlParameter[] param = {                                 
                                    new SqlParameter("@DealerCode",SqlDbType.VarChar,5), 
                                    new SqlParameter("@RegNo",SqlDbType.VarChar,30)
                               };


            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = ddlVehRegNo.SelectedValue.ToString().Trim(); // ddlRegNo.Text.Trim();//(HFRegNo.Value == "" ? null : HFRegNo.Value);

            dsVehInfo = myFunc.FillDataSet("sp_W2_CustomerVehicle_Select", param);
            if (dsVehInfo.Tables[0].Rows.Count > 0)
            {
                setVehcileInfo(dsVehInfo);
            }
            //PrepareNewJobCard();

            //if (ddlRegNo.BackColor == Color.Red)
            //{
            //    ddlRegNo.BackColor = Color.White;
            //}
        }

        protected void btnSaveJC_Click(object sender, EventArgs e)
        {
            try
            {
                //if (Session["CheckRefresh"].ToString() == ViewState["CheckRefresh"].ToString())
                //    Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
                //else return;

                //if (JobCard_PDI_FFI())
                //{
                //    SendAlert("Job card for PDI, FFI cannot save parts or lubricant"); return;
                //}

                if (ddlJobCardTypeCode.SelectedItem.Text == "SFI"
                    & double.Parse(lblJobCardTotal.Text == "" ? "0" : lblJobCardTotal.Text) != 0)
                {
                    SendAlert("Labour Amount is not allowed for SFI job card!"); return;
                }
                if (ddlAdvisorCode.SelectedIndex == 0)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Please select Advisor", txtCustomer);

                    ddlAdvisorCode.BackColor = Color.Red;
                    return;
                }
                else
                {
                    lblMsg.Text = string.Empty;
                    ddlAdvisorCode.BackColor = Color.White;
                }
                if (ddlJobCardTypeCode.SelectedIndex == 0)
                {
                    myFunc.UserMsg(lblMsg, Color.Red, "Select job card type first");
                    ddlJobCardTypeCode.BackColor = Color.Red;
                    return;
                }
                else
                {
                    lblMsg.Text = string.Empty;
                    ddlJobCardTypeCode.BackColor = Color.White;
                }
                if (ddlPayMode.SelectedIndex == 0)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Select Payment Mode");
                    ddlPayMode.BackColor = Color.Red;
                    return;
                }
                else
                {
                    lblMsg.Text = string.Empty;
                    ddlPayMode.BackColor = Color.White;

                }
                if (ddlBillingType.SelectedIndex == 0)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Select Billing type");
                    ddlBillingType.BackColor = Color.Red;
                    return;
                }
                else
                {
                    lblMsg.Text = string.Empty;
                    ddlBillingType.BackColor = Color.White;
                }
                if (ddlVehRegNo.SelectedIndex == 0)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Select Vehicle Registration Number");
                    ddlVehRegNo.BackColor = Color.Red;
                    return;
                }
                else
                {
                    lblMsg.Text = string.Empty;
                    ddlVehRegNo.BackColor = Color.White;
                }
                if (ddlJobTypeCode.SelectedIndex == 0)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Select JobType Number");
                    ddlJobTypeCode.BackColor = Color.Red;
                    return;
                }
                else
                {
                    lblMsg.Text = string.Empty;
                    ddlJobTypeCode.BackColor = Color.White;
                }
                if (ddlAdvisorCode.SelectedIndex == 0)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Select Advisor ");
                    ddlAdvisorCode.BackColor = Color.Red;
                    return;
                }
                else
                {
                    lblMsg.Text = string.Empty;
                    ddlAdvisorCode.BackColor = Color.White;
                }

                TextBox[] textBoxes = {  txtCustomer, txtEndUser, txtBrand, txtProduct, txtVersion ,
                                    txtReciptTime,txtKM,txtRecieptDate};
                if (!MasterValidation(textBoxes)) return;
                else
                {
                    // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Key", "var a=confirm('Test');if (a==true) {alert('Ok') ;} else{alert('Cancel')}", true);
                    JobCardEntry();
                    Load_ddlJobCardCode();
                }
            }
            catch (Exception ex)
            {

                grl.UserMsg(lblMsg, Color.Red, ex.Message, txtCustomer);
            }
        }

        protected void btnDeleteJC_Click(object sender, EventArgs e)
        {
            try
            {
                //grl = new General();
                //public bool CodeExists(string strTableNames, string strColumnCode, string strColumeValue)
                //CodeExists(string strTableNames, string strColumnCode, string strColumeValue)

                TextBox[] textBoxes = {txtCustomer, txtEndUser, txtBrand, txtProduct, txtVersion, 
                                txtReciptTime,txtKM,txtRecieptDate };
                if (!MasterValidation(textBoxes))
                {
                    return;
                }
                if (ddlAdvisorCode.SelectedIndex == 0)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Please select advisor", txtCustomer);
                    return;
                }

                //if (grl.IsExist("JobCardCode", ddlJobCardNo.Text, "JobCardMaster", " And GatePass<>''"))
                //{
                //    grl.UserMsg(lblMsg, Color.Red, "JobCard can not delete is has Gatpass", txtCustomer);
                //    return;
                //}
                if (txtGatePass.Text.Length != 0)
                {
                    grl.UserMsg(lblMsg, Color.Red, "JobCard can not delete is has Gatpass", txtCustomer);
                    return;
                }

                foreach (GridViewRow gvr in gvJobCardParts.Rows)
                {
                    if (gvr.Cells[6].Text != "&nbsp;")
                    {
                        if (Convert.ToInt32(gvr.Cells[6].Text) > 0)
                        {
                            grl.UserMsg(lblMsg, Color.Red, "Parts issued this JobCard not Delete", txtCustomer);
                            return;
                        }
                    }
                }
                foreach (GridViewRow gvr in gvLubParts.Rows)
                {
                    if (gvr.Cells[4].Text != "&nbsp;")
                    {
                        if (Convert.ToInt32(gvr.Cells[4].Text) > 0)
                        {
                            grl.UserMsg(lblMsg, Color.Red, "Lubricat issued this JobCard not Delete", txtCustomer);
                            return;
                        }
                    }
                }
                //SqlParameter[] JobCardDetail_Delete_param = {                                                       
                //                                                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                //                                                new SqlParameter("@JobCardCode",SqlDbType.Char,8),
                //                                            };
                //JobCardDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                //JobCardDetail_Delete_param[1].Value = ddlJobCardNo.Text;
                //myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_Detail_Delete", JobCardDetail_Delete_param, Trans);


                //SqlParameter[] JobCardPartsDetail_Delete_param = {                                                       
                //                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                //                                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                //                                                 };
                //JobCardPartsDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                //JobCardPartsDetail_Delete_param[1].Value = ddlJobCardNo.Text;
                //myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_PartsDetail_Delete", JobCardPartsDetail_Delete_param, Trans);

                //SqlParameter[] JobCardLubricanteDetail_Delete_param = {                                                       
                //                                                        new SqlParameter("@DealerCode",SqlDbType.Char,5),
                //                                                        new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                //                                                     };
                //JobCardLubricanteDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                //JobCardLubricanteDetail_Delete_param[1].Value = ddlJobCardNo.Text;
                //myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_LubricanteDetail_Delete", JobCardLubricanteDetail_Delete_param, Trans);    


                //SqlParameter[] JobCardSubletDetail_Delete_param = {                                                       
                //                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                //                                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                //                                                 };
                //JobCardSubletDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                //JobCardSubletDetail_Delete_param[1].Value = ddlJobCardNo.Text;
                //myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_Sublet_Delete", JobCardSubletDetail_Delete_param, Trans);    

                SqlParameter[] JobCard_Master_Delete_param = {                                                       
                                                                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                                                              };
                JobCard_Master_Delete_param[0].Value = Session["DealerCode"].ToString();
                JobCard_Master_Delete_param[1].Value = ddlJobCardCode.SelectedValue.ToString().Trim();
                if (myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_Master_Delete", JobCard_Master_Delete_param))
                {
                    
                    grl.UserMsg(lblMsg, Color.Green, "Record Deleted Successfully: " + ddlJobCardCode.SelectedValue.ToString().Trim(), txtCustomer);
                    clearAll();
                }
                else
                {
                    grl.UserMsg(lblMsg, Color.Red, "Record not deleted: " + ddlJobCardCode.SelectedValue.ToString().Trim(), txtCustomer);
                }

            }

            catch (Exception ex)
            {
                grl.UserMsg(lblMsg, Color.Red, ex.Message, txtCustomer);
            }
        }

        protected void btnClearJC_Click(object sender, EventArgs e)
        {
            clearAll();
        }

        protected void btnPostJC_Click(object sender, EventArgs e)
        {
            if (ddlJobCardCode.SelectedIndex == 0)
            {
                grl.UserMsg(lblMsg, Color.Red, "jobCardNo should not be left blank", txtCustomer);
                return;
            }
            if (ddlJobTypeCode.SelectedValue.ToString() != "001" && ddlJobTypeCode.SelectedValue.ToString() != "002")
            {
                
                grl.UserMsg(lblMsg, Color.Red, "Posting not allow, please make customer invoine against this jobcard No", txtCustomer);
                return;
            }

            ds = new DataSet();
            grl.CodeExists("JobCardMaster", "JobCardCode='" + ddlJobCardCode.SelectedValue.ToString().Trim() + "' AND DealerCode='" + Session["DealerCode"].ToString() + "'", ref ds);
            search_result = false;

            if (ddlJobCardTypeCode.SelectedItem.Text == "Warranty")
            {
                //double  dJCTotal  = 0.00;
                //string Query = 
                //if()
                //{

                //}
                if (grl.CodeExists("JobCardPartsDetail", "JobCardCode", ddlJobCardCode.SelectedValue.ToString().Trim(), " and qty <> recqty"))
                {
                    SendAlert("Parts have not been issued yet"); return;
                }

                if (grl.CodeExists("JobCardLubricateDetail", "JobCardCode", ddlJobCardCode.SelectedValue.ToString().Trim(), " and qty <> recqty"))
                {
                    SendAlert("Lubricant parts have not been issued yet"); return;
                }
            }
            //Check RecQuty and IssuQty is equal
            string StrSIRNo = string.Empty;
            DataSet dsSIR = new DataSet();
            StrSIRNo = grl.GetStringValuesAgainstCodes("JobCardCode", ddlJobCardCode.SelectedValue.ToString().Trim(), "SIRMaster", "JobCardMaster");
            myFunc.ExecuteQuery("Select * From SIRDetail Where Dealercode='" + Session["DealerCode"].ToString() + "' And SIRNo='" + StrSIRNo + "'", ref dsSIR);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsSIR.Tables[0].Rows)
                {
                    if (Convert.ToInt32(dr["Quantity"]) != Convert.ToInt32(dr["RecQty"]))
                    {
                        
                        grl.UserMsg(lblMsg, Color.Red, "Stock not issue yet", txtCustomer);
                        return;
                    }
                }
            }
            /////////////////
            if (ds.Tables[0].Rows[0]["JobCardType"].ToString().Trim() == "Repeated JobCard")
            {
                callPostingJobCard(); search_result = true;
            }
            else if (ds.Tables[0].Rows[0]["JobTypeCode"].ToString().Trim() == "001")
            {
                callPostingJobCard(); search_result = true;
                if (Create_NDM(ddlJobCardCode.SelectedValue.ToString().Trim(), "InvoiceCreated") == false)
                {
                    grl.UserMsg(lblMsg, Color.Red, "NDM Data Can not be inserted");
                    ObjTrans.RollBackTransaction(ref Trans);
                    return;
                }
                if (PostServiceFollowUp(ddlJobCardCode.SelectedValue.ToString().Trim(), "InvoiceCreated") == false)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Post Service FollowUp Data Can not be inserted");
                    ObjTrans.RollBackTransaction(ref Trans);
                    return;
                }
            }
            else if (ds.Tables[0].Rows[0]["JobTypeCode"].ToString().Trim() == "002")
            {
                callPostingJobCard(); search_result = true;

                if (Create_NDM(ddlJobCardCode.SelectedValue.ToString().Trim(), "InvoiceCreated") == false)
                {
                    grl.UserMsg(lblMsg, Color.Red, "NDM Data Can not be inserted");
                    ObjTrans.RollBackTransaction(ref Trans);
                    return;
                }
                if (PostServiceFollowUp(ddlJobCardCode.SelectedValue.ToString().Trim(), "InvoiceCreated") == false)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Post Service FollowUp Data Can not be inserted");
                    ObjTrans.RollBackTransaction(ref Trans);
                    return;
                }
            }

            if (search_result == false) { SendAlert("Cannot post due to Job Type / Job Card Type!"); return; }
            clearAll();
        }

        protected void btnPrintJC_Click(object sender, EventArgs e)
        {
            string PDFFileName = MakeReport();

            string URL = "../../../Download/OpenPdf.aspx?FileName=" + PDFFileName;
            //URL = FilePath + "OpenPdf.aspx?FileName=" + FileName;
            //txtPartItemDesc.Text = URL;
            string fullsysURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullsysURL, true);
        }

        protected void ddlJobs_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = myFunc.GetData("select Amount from LaborDetail where DealerCode='" + Session["DealerCode"].ToString() + "' And ProdCode='" + txtProduct.Text + "' And DefJobCode ='"+ddlJobs_Labor.SelectedValue+"' ");
            if (dt.Rows[0]["Amount"].ToString() != "")
            {
                txtLabor.Text = dt.Rows[0]["Amount"].ToString();
            }       
            
        }

        protected void BtnAddJobs_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlJobs_Labor.SelectedIndex == 0)
            {
                
                grl.UserMsg(lblMsg, Color.Red, "Please select job");
                return;
            }
            else
            {
                lblMsg.Text = string.Empty;
            }
            TextBox[] textBoxes = { txtLabor };
            if (ddlTechnicianEmpCode.SelectedIndex == 0)
            {
                grl.UserMsg(lblMsg, Color.Red, "Please select Technician Person");
                return;
            }
            if (!MasterValidation(textBoxes)) { return; }
            else
            {
                search_result = false;
                foreach (DataRow rowJobDT in JobDT.Rows)
                {
                    if (rowJobDT["JobCode"].ToString().Trim() == ""
                        | rowJobDT["JobCode"].ToString().Trim() == ddlJobs_Labor.SelectedValue.ToString().Trim())
                    {
                        Insert_in_JobDT(rowJobDT); search_result = true;
                    }
                }
                if (search_result == false)
                {
                    DataRow rowJobDT = JobDT.NewRow(); Insert_in_JobDT(rowJobDT); JobDT.Rows.Add(rowJobDT);
                }
                ViewState["Job"] = JobDT; gvJobCard.DataSource = JobDT; gvJobCard.DataBind();

                txtJobsTotal.Text = JobDT.Rows.Count.ToString();
                ClearJobTextBoxes();
            }
        }

        protected void BtnClearJobs_Click(object sender, ImageClickEventArgs e)
        {
            ClearJobTextBoxes();
        }

        protected void btnAddParts_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlPartCode.SelectedIndex == 0)
            {
                
                grl.UserMsg(lblMsg, Color.Red, "Please enter Quantity", txtQuantity);
                return;
            }
            else
            {
                lblMsg.Text = string.Empty;
            }
            if (txtQuantity.Text == "")
            {
                txtQuantity.Text = "0";
            }
            if (Convert.ToInt32(txtQuantity.Text) == 0)
            {
                
                grl.UserMsg(lblMsg, Color.Red, "Please enter Quantity", txtQuantity);
                return;
            }
            if (Convert.ToInt32(txtQuantity.Text) < Convert.ToInt32(txtPartsRecQuantity.Text))
            {
                
                grl.UserMsg(lblMsg, Color.Red, "Issue quantity not less then receive quantity", txtQuantity);
                return;
            }
            else
            {
                lblMsg.Text = "";
            }
            //
            TextBox[] textBoxes = { txtPartPrice, txtQuantity };
            if (!MasterValidation(textBoxes)) return;
            else
            {
                search_result = false;
                foreach (DataRow rowPartsDT in PartsDT.Rows)
                {
                    if (rowPartsDT["ItemCode"].ToString().Trim() == ""
                        | rowPartsDT["ItemCode"].ToString().Trim() ==ddlPartCode.SelectedValue.ToString().Trim())
                    {
                    Insert_in_PartsDT(rowPartsDT); search_result = true;
                    }
                }
                if (search_result == false)
                {
                    DataRow rowPartsDT = PartsDT.NewRow(); Insert_in_PartsDT(rowPartsDT); PartsDT.Rows.Add(rowPartsDT);
                }
                ViewState["Parts"] = PartsDT; gvJobCardParts.DataSource = PartsDT; gvJobCardParts.DataBind();
                int Count = 0;
                foreach (DataRow dr in PartsDT.Rows)
                {
                    Count = Count + Convert.ToInt32(dr["Qty"]);
                }
                txtPartTotalQuantity.Text = Count.ToString();
                ClearPartsTextBoxes();

            }
        }

        protected void btnClearParts_Click(object sender, ImageClickEventArgs e)
        {
            ClearPartsTextBoxes();
        }

        protected void ddlPartCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlJobCardTypeCode.SelectedValue == "Insurance")
                {
                    txtDep.ReadOnly = false;
                    txtDep.Text = "0";
                }
                else
                {
                    txtDep.ReadOnly = true;
                    txtDep.Text = "";
                }
                DataTable dt = new DataTable();
                dt = myFunc.GetData("select ItemDesc,SaleRate,PartItemNo from Item where DealerCode='" + Session["DealerCode"].ToString() + "' And  ItemCode='" + ddlPartCode.SelectedValue.ToString() + "'");
                txtItemDesc.Text = dt.Rows[0]["ItemDesc"].ToString().Trim();
                txtPartItemNo_Parts.Text = dt.Rows[0]["PartItemNo"].ToString().Trim();
                txtPartPrice.Text = dt.Rows[0]["SaleRate"].ToString().Trim();
            }
            catch (Exception ex)
            {
                myFunc.UserMsg(lblMsg, Color.Red, ex.Message);
            }
        }

        protected void ddlLubCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = myFunc.GetData("select ItemDesc,SaleRate,PartItemNo from Item where DealerCode='" + Session["DealerCode"].ToString() + "' And  ItemCode='" + ddlLubCode.SelectedValue.ToString() + "'");
                txtLubPartDesc.Text = dt.Rows[0]["ItemDesc"].ToString().Trim();
                txtPartItemNo_Lubs.Text = dt.Rows[0]["PartItemNo"].ToString().Trim();
                txtLubPrice.Text = dt.Rows[0]["SaleRate"].ToString().Trim();
            }
            catch (Exception ex)
            {
                myFunc.UserMsg(lblMsg, Color.Red, ex.Message);
            }
        }

        protected void btnAddLubs_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlLubCode.SelectedIndex == 0)
            {
                
                grl.UserMsg(lblMsg, Color.Red, "Please select Lube Item", txtQuantity);
                return;
            }
            
            TextBox[] textBoxes = { txtLubPrice, txtLubQuantity };
            if (!MasterValidation(textBoxes)) return;
            else
            {
                search_result = false;
                foreach (DataRow rowLubDT in LubDT.Rows)
                {
                    if (rowLubDT["ItemCode"].ToString().Trim() == ""
                        | rowLubDT["ItemCode"].ToString().Trim() == ddlLubCode.SelectedValue.ToString().Trim())
                    {
                        Insert_in_LubPartsDT(rowLubDT); search_result = true;
                    }
                }
                if (search_result == false)
                {
                    DataRow rowLubDT = LubDT.NewRow(); Insert_in_LubPartsDT(rowLubDT); LubDT.Rows.Add(rowLubDT);
                }
                ViewState["Lub"] = LubDT; gvLubParts.DataSource = LubDT; gvLubParts.DataBind();
                ClearLubricantsTextBoxes();
            }
        }

        protected void btnClearLubs_Click(object sender, ImageClickEventArgs e)
        {
            ClearLubricantsTextBoxes();
        }

        protected void btnAddSublet_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ddlVendorCode.SelectedIndex == 0)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Please Select Vendor");
                    return;
                }
                else
                {
                    lblMsg.Text = "";
                }
                if (ddlJobs_Sublet.SelectedIndex == 0)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Please Select Sublet job", txtCustomer);
                    return;
                }
                else
                {
                    lblMsg.Text = "";
                }
                if (txtSubletDate.Text.Trim() == string.Empty || txtSubletDate.Text.Trim() == "__-__-____")
                {
                    grl.UserMsg(lblMsg, Color.Red, "Please Select Sublet Date", txtSubletDate);
                    return;
                }
                else
                {
                    lblMsg.Text = "";
                }
                if (txtSubletIncAmnt.Text.Trim() == string.Empty)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Invoice Amount is not be left be blank", txtSubletIncAmnt);
                    return;
                }
                else
                {
                    lblMsg.Text = "";
                }
                if (txtSubletPayAmnt.Text.Trim() == string.Empty)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Paid Amount is not be left be blank", txtSubletPayAmnt);
                    return;
                }
                else
                {
                    lblMsg.Text = "";
                }
                

                search_result = false;
                foreach (DataRow rowSubletDT in SubletDT.Rows)
                {
                    if (rowSubletDT["JobCode"].ToString().Trim() == ""
                        | rowSubletDT["JobCode"].ToString().Trim() == ddlJobs_Sublet.SelectedValue.ToString().Trim())
                    {
                        Insert_in_SubletDT(rowSubletDT); search_result = true;
                    }
                }
                if (search_result == false)
                {
                    DataRow rowSubletDT = SubletDT.NewRow(); Insert_in_SubletDT(rowSubletDT); SubletDT.Rows.Add(rowSubletDT);
                }
                ViewState["SubLet"] = SubletDT; gvSublet.DataSource = SubletDT; gvSublet.DataBind();
                ClearJobSubletTextBoxes();
            }
            catch (Exception ex) { throw ex; }
        }

        protected void btnClearSublet_Click(object sender, ImageClickEventArgs e)
        {
            ClearJobSubletTextBoxes();
        }

        protected void ddlJobs_Sublet_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
               dt = myFunc.GetData("select DefJobDesc from DefaultJob where DealerCode IN('" + Session["DealerCode"].ToString() + "','COMON') And  DefJobCode='" + ddlJobs_Sublet.SelectedValue.ToString() + "'");
               txtSubletJobDesc.Text = dt.Rows[0]["DefJobDesc"].ToString().Trim();

        }

        protected void ddlConsumableParts_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {   
                DataTable dt = new DataTable();
                dt = myFunc.GetData("select ItemDesc,SaleRate,PartItemNo from Item where DealerCode='" + Session["DealerCode"].ToString() + "' And  ItemCode='" + ddlConsumableParts.SelectedValue.ToString() + "'");
                txtConPartDesc.Text = dt.Rows[0]["ItemDesc"].ToString().Trim();
                txtPartItemNoConParts.Text = dt.Rows[0]["PartItemNo"].ToString().Trim();
                txtConPartPrice.Text = dt.Rows[0]["SaleRate"].ToString().Trim();
            }
            catch (Exception ex)
            {
                myFunc.UserMsg(lblMsg, Color.Red, ex.Message);
            }
        }

        protected void btnAddConsumablePart_Click(object sender, ImageClickEventArgs e)
        {
            if (txtConPartDesc.Text == "")
            {
                
                grl.UserMsg(lblMsg, Color.Red, "Please select item", txtConPartDesc);
                return;
            }
            if (txtConQuantity.Text == "")
            {
                txtConQuantity.Text = "0";
            }
            if (Convert.ToInt32(txtConQuantity.Text) == 0)
            {
                
                grl.UserMsg(lblMsg, Color.Red, "Please enter Quantity", txtConQuantity);
                return;
            }
            if (Convert.ToInt32(txtConQuantity.Text) < Convert.ToInt32(txtConPartsRecQuantity.Text))
            {
                
                grl.UserMsg(lblMsg, Color.Red, "Issue quantity not less then receive quantity", txtConQuantity);
                return;
            }
            else
            {
                lblMsg.Text = "";
            }
            
            TextBox[] textBoxes = { txtConPartPrice, txtConQuantity };
            if (!MasterValidation(textBoxes)) return;
            else
            {
                search_result = false;
                foreach (DataRow rowConPartsDT in ConPartsDT.Rows)
                {
                    if (rowConPartsDT["ItemCode"].ToString().Trim() == ""
                        | rowConPartsDT["ItemCode"].ToString().Trim() == ddlConsumableParts.SelectedValue.ToString().Trim())
                    {
                        Insert_in_ConPartsDT(rowConPartsDT); search_result = true;
                    }
                }
                if (search_result == false)
                {
                    DataRow rowConPartsDT = ConPartsDT.NewRow(); Insert_in_ConPartsDT(rowConPartsDT); ConPartsDT.Rows.Add(rowConPartsDT);
                }
                ViewState["ConParts"] = ConPartsDT; gvJobCardConParts.DataSource = ConPartsDT; gvJobCardConParts.DataBind();
                int Count = 0;
                foreach (DataRow dr in ConPartsDT.Rows)
                {
                    Count = Count + Convert.ToInt32(dr["Qty"]);
                }
                txtConPartTotalQuantity.Text = Count.ToString();
                ClearConPartsTextBoxes();

            }
        }

        protected void btnClearConsumablePart_Click(object sender, ImageClickEventArgs e)
        {
            ClearConPartsTextBoxes();
        }

        protected void ddlJobCardCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMasterData();
        }

        protected void ddlEstNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = myFunc.GetData("select RegNo from CustomerEstimateMaster where DealerCode='" + Session["DealerCode"].ToString() + "' And  CustomerEstimateCode='" + ddlEstNo.SelectedValue.ToString().Trim() + "'");
                ddlVehRegNo.SelectedValue = dt.Rows[0]["RegNo"].ToString().Trim();
                ddlVehRegNo_SelectedIndexChanged(ddlVehRegNo, EventArgs.Empty);

                /////////////////////////////////////
                createPartsDT();
                dt = myFunc.GetData("select ItemCode,Qty from CustomerEstimateParts where DealerCode='" + Session["DealerCode"].ToString() + "' And  CustomerEstimateCode='" + ddlEstNo.SelectedValue.ToString().Trim() + "'");
                foreach (DataRow dr in dt.Rows)
                {
                    ddlPartCode.SelectedValue = dr["ItemCode"].ToString().Trim();
                    txtQuantity.Text = dr["Qty"].ToString().Trim();
                    ddlPartCode_SelectedIndexChanged(ddlPartCode, EventArgs.Empty);
                    btnAddParts_Click(btnAddParts, null);
                }
                createLubDT();
                dt = myFunc.GetData("select ItemCode,Qty from CustomerEstimateLubricates where DealerCode='" + Session["DealerCode"].ToString() + "' And  CustomerEstimateCode='" + ddlEstNo.SelectedValue.ToString().Trim() + "'");
                foreach (DataRow dr in dt.Rows)
                {
                    ddlLubCode.SelectedValue = dr["ItemCode"].ToString().Trim();
                    txtLubQuantity.Text = dr["Qty"].ToString().Trim();
                    ddlLubCode_SelectedIndexChanged(ddlLubCode, EventArgs.Empty);
                    btnAddLubs_Click(btnAddLubs, null);
                }
                createJobDT();
                dt = myFunc.GetData("select JobCode,JobRemarks,Labor from CustomerEstimateDetail where DealerCode='" + Session["DealerCode"].ToString() + "' And  CustomerEstimateCode='" + ddlEstNo.SelectedValue.ToString().Trim() + "'");
                foreach (DataRow dr in dt.Rows)
                {
                    ddlJobs_Labor.SelectedValue = dr["JobCode"].ToString().Trim();
                    txtJobRemarks.Text = dr["JobRemarks"].ToString().Trim();
                    txtLabor.Text = dr["Labor"].ToString().Trim();
                    ddlTechnicianEmpCode.SelectedIndex = 1;
                    BtnAddJobs_Click(BtnAddJobs, null);
                }
            }
            catch (Exception ex)
            {
                myFunc.UserMsg(lblMsg, Color.Red, ex.Message);
            }

        }



        

        

        

        

        

    }

        
}