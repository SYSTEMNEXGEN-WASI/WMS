using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;
using CConn;
using Microsoft.ApplicationBlocks.Data;
using System.Threading;
using System.IO;
using PdfSharp.Pdf;

namespace DXBMS.Modules.Service
{
    public partial class JobCardNew : System.Web.UI.Page
    {
        SysFunctions myFunc = new SysFunctions();
        SysFunction sysfun = new SysFunction();
        SecurityBll sec = new SecurityBll();
        MainBLL objMBLL = new MainBLL();
        ServiceBL objSBL = new ServiceBL();
        //General grl = new General();
        SysFunctions grl = new SysFunctions();
        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;
        string apStr, PartRequisition = "";
        DataTable JobDT, LubDT, SubletDT, PartsDT, ConPartsDT, BayDT, BoutDT;
        DataSet dsJobCardDetail, dsJobCardParts, dsJobCardLub, dsJobCardSublet, dsJobCardConParts, dsJobCardBay, dsJobCardBoutParts, ds;


        int updflag, countLabour, countConPartsRecQty, countConParts;
        decimal countlubRecQty, countlub, countParts, countPartsRecQty;
        string sQuery;
        double totLabour, totParts, totlub, totsubletInv, totsubletPayable, totConParts, totBoutInv, totPayableBout, totEst, totJob;

        bool search_result, search_item;
        clsLookUp clslook = new clsLookUp();

        int ridx, byPass = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2532", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (ViewState["Parts"] != null) PartsDT = (DataTable)ViewState["Parts"];
            if (ViewState["Job"] != null) JobDT = (DataTable)ViewState["Job"];
            if (ViewState["Bay"] != null) BayDT = (DataTable)ViewState["Bay"];
            if (ViewState["Lub"] != null) LubDT = (DataTable)ViewState["Lub"];
            if (ViewState["SubLet"] != null) SubletDT = (DataTable)ViewState["SubLet"];
            if (ViewState["ConParts"] != null) ConPartsDT = (DataTable)ViewState["ConParts"];
            if (ViewState["BoutParts"] != null) BoutDT = (DataTable)ViewState["BoutParts"];
            //d1 = (ViewState["d1"] != null ? Convert.ToDecimal(ViewState["d1"].ToString()) : 0);
            //d2 = (ViewState["d2"] != null ? Convert.ToDecimal(ViewState["d2"].ToString()) : 0);
            //d3 = (ViewState["d3"] != null ? Convert.ToDecimal(ViewState["d3"].ToString()) : 0);
            //d4 = (ViewState["d4"] != null ? Convert.ToDecimal(ViewState["d4"].ToString()) : 0);
            //ridx = (ViewState["ridx"] != null ? int.Parse(ViewState["ridx"].ToString()) : -1);
            //deductAmount = (ViewState["deductAmount"] != null ? decimal.Parse(ViewState["deductAmount"].ToString()) : 0);
            //Session["DealerCode"] = "00001";
            //GlobalVar.mDealerCode = "00001";
            //Session["UserName"] = "azhar";

            if (Page.IsPostBack)
            {

                if (!string.IsNullOrEmpty(Session["LookUpData"].ToString()))
                {
                    SelectedPartDetail(Session["LookUpData"].ToString());
                }
            }
            if (Page.IsPostBack == false)
            {
                LoadEmptyGrid();



            }


            if (!Page.IsPostBack)
            {

                ViewState["JobCardCode"] = "";


                if (Session["JobCardCode"] != null)
                {

                    ThreadStart childthreat = new ThreadStart(childthreadcall);
                    Thread child = new Thread(childthreat);

                    child.Start();
                    loadPaymentMode();

                    DataSet ds = new DataSet();
                    SqlParameter[] JobCardMaster_param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5)};
                    JobCardMaster_param[0].Value = Session["DealerCode"].ToString();

                    objMBLL.FillDrp_SP(ddlAdvisorCode, "sp_Get_Advisor", "EmpCode", "EmpName", JobCardMaster_param, true, "Select", false, "");

                    objMBLL.FillDrp_SP(ddlTechnicianEmpCode, "sp_2W_DealerEmp_Technician_Select", "EmpCode", "EmpName", JobCardMaster_param, true, "--Select--", false, "");


                    objMBLL.FillDrp_SP(ddlVendorCode, "sp_2W_Vendor_Select", "VendorCode", "VendorDesc", JobCardMaster_param, true, "--Select--", false, "");

                    objMBLL.FillDropDown(ddlJobCardTypeCode, "SELECT JobTypeCode,JobTypeDesc FROM JobTypeMaster Where DealerCode IN('" + Session["DealerCode"].ToString() + "', 'COMON') order by SortKey Desc ", "JobTypeDesc", "JobTypeCode", "Select");
                    string where = "Bay.ShopID = Shops.ShopID and Bay.DealerCode = '" + Session["DealerCode"].ToString() + "'";

                    String[] col = new string[] { "BayID", "BayDesc", "ShopDesc" };
                    myFunc.GetMultiColumnsDDL(ddlBay, col, "Bay,Shops", where, "BayID", "", false, false);

                    objMBLL.FillDropDown(ddlTaxType, "Select TM.TaxAppCode,TD.TaxType+'|'+TaxAppDesc as TaxDesc from TaxSetupMaster TM inner join TaxSetupDetail TD on TD.DealerCode=TM.DealerCode and TD.TaxAppCode = TM.TaxAppCode " +
                       " Where TM.DealerCode IN('" + Session["DealerCode"].ToString() + "', 'COMON')  ", "TaxDesc", "TaxAppCode", "Select");

                    txtDep.ReadOnly = true;
                    string leadId = Session["JobCardCode"].ToString();
                    Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
                    //ddlJobCardCode.SelectedValue = leadId + " ";
                    txtJobCardCode.Text = leadId;
                    ViewState["JobCardCode"] = leadId;
                    LoadMasterData();
                    // LoadEmptyGrid();
                }
                else
                {

                    ThreadStart childthreat = new ThreadStart(childthreadcall);
                    Thread child = new Thread(childthreat);

                    child.Start();



                    setInitialDates();            //-------------------------------------SUBLET
                    createSubletDT();            //-------------------------------------LUBRICANT
                    createLubDT();              //------------------------------------------PARTS            
                    createPartsDT();            //-----------------------------------------JOB
                    createBayDT();
                    createJobDT();
                    createConPartsDT();
                    loadPaymentMode();
                    //Load_ddlJobCardCode();
                    //Load_ddlVehRegNo();
                    //Load_ddlJobs(ddlJobs_Labor);
                    //Load_ddlJobs(ddlJobs_Sublet);
                    //Load_ddlEstNo();
                    //myFunc.Load_CounterSalesItem_ddl(ddlPartCode, Session["DealerCode"].ToString(), "P");
                    //myFunc.Load_CounterSalesItem_ddl(ddlLubCode, Session["DealerCode"].ToString(), "L");
                    //myFunc.Load_CounterSalesItem_ddl(ddlConsumableParts, Session["DealerCode"].ToString(), "PL");
                    //this.DataBind();
                    DataSet ds = new DataSet();
                    SqlParameter[] JobCardMaster_param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5)};
                    JobCardMaster_param[0].Value = Session["DealerCode"].ToString();
                    //myFunc.FillDrp_SP(ddlAdvisorCode, "sp_Get_Advisor", "EmpCode", "EmpName", JobCardMaster_param, true, "--Select--", false, "");
                    objMBLL.FillDrp_SP(ddlAdvisorCode, "sp_Get_Advisor", "EmpCode", "EmpName", JobCardMaster_param, true, "Select", false, "");
                    //objMBLL.FillDrp_SP(ddlAdvisor, "sp_Get_Advisor", "EmpCode", "EmpName", JobCardMaster_param, true, "--Select--", false, "");
                    objMBLL.FillDrp_SP(ddlTechnicianEmpCode, "sp_2W_DealerEmp_Technician_Select", "EmpCode", "EmpName", JobCardMaster_param, true, "Select", false, "");
                    //objMBLL.FillDrp_SP(ddlTechnician, "sp_2W_DealerEmp_Technician_Select", "EmpCode", "EmpName", JobCardMaster_param, true, "--Select--", false, "");
                    objMBLL.FillDrp_SP(ddlVendorCode, "sp_2W_Vendor_Select", "VendorCode", "VendorDesc", JobCardMaster_param, true, "--Select--", false, "");
                    objMBLL.FillDropDown(ddlJobCardTypeCode, "SELECT JobTypeCode,JobTypeDesc FROM JobTypeMaster Where DealerCode IN('" + Session["DealerCode"].ToString() + "', 'COMON') ", "JobTypeDesc", "JobTypeCode", "Select");
                    objMBLL.FillDropDown(ddlTaxType, "Select TM.TaxAppCode,TD.TaxType+'|'+TaxAppDesc as TaxDesc from TaxSetupMaster TM inner join TaxSetupDetail TD on TD.DealerCode=TM.DealerCode and TD.TaxAppCode = TM.TaxAppCode " +
                        " Where TM.DealerCode IN('" + Session["DealerCode"].ToString() + "', 'COMON')  ", "TaxDesc", "TaxAppCode", "Select");
                    txtDep.ReadOnly = true;
                    Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());

                    TabContainer1.ActiveTabIndex = 0;
                    //Create_Grid();

                    string where = "Bay.ShopID = Shops.ShopID and Bay.DealerCode = '" + Session["DealerCode"].ToString() + "'";
                    String[] col = new string[] { "BayID", "BayDesc", "ShopDesc" };
                    myFunc.GetMultiColumnsDDL(ddlBay, col, "Bay,Shops", where, "BayID", "", false, false);
                    //myFunc.GetMultiColumnsDDL(ddlBaySubletJob, col, "Bay,Shops", where, "BayID", "", false, false);
                }

                //ddlJobCardCode.SelectedValue = ViewState["JobCardCode"].ToString();
                txtJobCardCode.Text = ViewState["JobCardCode"].ToString();

            }

            Session["LookUpData"] = string.Empty;
            Session["JobCardCode"] = null;
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
        public void loadPaymentMode()
        {
            ddlPayMode.Items.Clear();
            ddlPayMode.Items.Add(new ListItem("Select", "Select"));
            ddlPayMode.Items.Add(new ListItem("Cash", "Cash"));
            ddlPayMode.Items.Add(new ListItem("Credit", "Credit"));
            ddlPayMode.Items.Add(new ListItem("Intra", "Intra"));
            ddlPayMode.Items.Add(new ListItem("Internal", "Internal"));
            ddlPayMode.Items.Add(new ListItem("Free of Cost", "FOC"));
            ddlPayMode.Items.Add(new ListItem("OEM", "OEM"));
            ddlPayMode.Items.Add(new ListItem("Good Will Warranty", "Good Will Warranty"));
        }
        public void childthreadcall()
        {
            try
            {

                sQuery = "SP_SelectParts '" + Session["DealerCode"].ToString() + "', 'P'";

                if (myFunc.ExecuteQuery(sQuery, ref ds))
                {
                    Session["Parts"] = ds;
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




        //private void Load_ddlEstNo()
        //{
        //    string WhereClause = "DealerCode = '" + Session["DealerCode"].ToString() + "' " +
        //        "And DelFlag='N'  ";

        //    string[] Columns = new string[] { "CustomerEstimateCode", "CONVERT(VARCHAR(10),tdDate,105)","RegNo", "UserName" };
        //    myFunc.GetMultiColumnsDDL(ddlEstNo, Columns, "CustomerEstimateMaster ", WhereClause, "CustomerEstimateCode", " Order by CustomerEstimateCode Desc ", false, false);
        //}

        //private void Load_ddlVehRegNo()
        //{
        //    string WhereClause = "DealerCode IN ('" + Session["DealerCode"].ToString() + "','COMON')  ";


        //    string[] Columns = new string[] { "RegNo", "ChassisNo", "EngineNo" };
        //    myFunc.GetMultiColumnsDDL(ddlVehRegNo, Columns, "CustomerVehicle ", WhereClause, "RegNo", "order by CusVehCode desc ", false, false);
        //}
        private void Load_ddlJobs(DropDownList ddlJobs)
        {
            string WhereClause = "DJ.DealerCode = JC.DealerCode " +
            "And DJ.JobCatCode = JC.JobCatCode " +
            "And DJ.DealerCode = '" + Session["DealerCode"].ToString() + "'";

            string[] Columns = new string[] { "DJ.DefJobCode", "DJ.DefJobDesc", "JC.JobCatDesc" };
            myFunc.GetMultiColumnsDDL(ddlJobs, Columns, "DefaultJob DJ, JobCategory JC ", WhereClause, "DefJobCode", "", false, false);
        }
        private void setInitialDates()
        {
            txtJobCardDate.Text = txtSubletDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtPromisedDate.Text = txtDeliveryDate.Text = txtDeliveryDate.Text = txtRecieptDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtDeliveryTime.Text = txtReciptTime.Text = DateTime.Now.ToString("HH:mm");
            txtPromistedTime.Text = "00:00";
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

        private void createBayDT()
        {
            BayDT = new DataTable();
            BayDT.Columns.Add(new DataColumn("ID", typeof(int)));
            BayDT.Columns.Add(new DataColumn("JobCardCode", typeof(string)));
            BayDT.Columns.Add(new DataColumn("EmpCode", typeof(string)));
            BayDT.Columns.Add(new DataColumn("EmpName", typeof(string)));
            BayDT.Columns.Add(new DataColumn("StdTime", typeof(string)));
            BayDT.Columns.Add(new DataColumn("StartTime", typeof(string)));
            BayDT.Columns.Add(new DataColumn("EndTime", typeof(string)));
            BayDT.Columns.Add(new DataColumn("TimeTaken", typeof(string)));
            BayDT.Columns.Add(new DataColumn("JobCode", typeof(string)));
            BayDT.Columns.Add(new DataColumn("DefJobDesc", typeof(string)));
            BayDT.Columns.Add(new DataColumn("BayCode", typeof(string)));
            BayDT.Columns.Add(new DataColumn("BayDesc", typeof(string)));

            grdBay.DataSource = BayDT;
            grdBay.DataBind();
            ViewState["Bay"] = BayDT;
        }
        private void createLubDT()
        {
            LubDT = new DataTable();
            LubDT.Columns.Add(new DataColumn("ID", typeof(int)));
            LubDT.Columns.Add(new DataColumn("PartNo", typeof(string)));
            LubDT.Columns.Add(new DataColumn("PartsDesc", typeof(string)));
            LubDT.Columns.Add(new DataColumn("Qty", typeof(string)));
            LubDT.Columns.Add(new DataColumn("Dep", typeof(string)));
            LubDT.Columns.Add(new DataColumn("DepAmount", typeof(string)));
            LubDT.Columns.Add(new DataColumn("Price", typeof(string)));
            LubDT.Columns.Add(new DataColumn("Total", typeof(string)));
            LubDT.Columns.Add(new DataColumn("ItemCode", typeof(string)));
            LubDT.Columns.Add(new DataColumn("RecQty", typeof(string)));
            LubDT.Columns.Add(new DataColumn("Type", typeof(string)));
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
            // PartsDT.Columns.Add(new DataColumn("Type", typeof(string)));
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
            gvJobCard.DataSource = JobDT; gvJobCard.DataBind(); ViewState["Job"] = JobDT;

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
            param[1].Value = txtVehRegNo.Text.Trim();
            // ddlVehRegNo.SelectedValue.ToString().Trim(); //(HFRegNo.Value == "" ? null : HFRegNo.Value);
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
            dsParam[1].Value = (ViewState["JobCardCode"].ToString().Trim() == "" ? null : ViewState["JobCardCode"].ToString().Trim());

            DataTable dtjc = new DataTable();
            string sqlj = "select * from JobCardMaster where JobCardCode='" + dsParam[1].Value.ToString() + "' and DealerCode='" + Session["DealerCode"].ToString() + "'";
            dtjc = myFunc.GetData(sqlj);
            if (dtjc.Rows.Count == 0)
            {

                btnClearJC_Click(btnClearJC, EventArgs.Empty);
                lblMsg.Text = "Error opening JobCard please contact concern person";
                return;

            }

            HFJobCard.Value = ViewState["JobCardCode"].ToString().Trim();
            HFRegNo.Value = dtjc.Rows[0]["RegNo"].ToString();
            HFEngineNo.Value = dtjc.Rows[0]["EngineNo"].ToString();
            HFChassisNo.Value = dtjc.Rows[0]["ChassisNo"].ToString();
            HFCustomer.Value = dtjc.Rows[0]["CusCode"].ToString();

            //ddlVehRegNo.SelectedValue = dtjc.Rows[0]["RegNo"].ToString();
            txtVehRegNo.Text = dtjc.Rows[0]["RegNo"].ToString();

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


            setVehcileInfo(dsJobCardMaster);
            setJobCardInfo(dsJobCardMaster);

            if (ddlJobCardTypeCode.SelectedValue == "012")
            {
                ddlJobCardTypeCode.Enabled = false;
                txtDep.ReadOnly = false;
                txtDep.Text = "0";
                btnUpdJobs.Visible = true;
                btnUpdBout.Visible = true;
                btnUpdLub.Visible = true;
                btnUpdSublet.Visible = true;
                btnUpdParts.Visible = true;
            }
            else
            {
                txtDep.ReadOnly = true;
                txtDep.Text = "";
            }

            dsJobCardDetail = new DataSet();
            dsJobCardDetail = myFunc.FillDataSet("sp_W2_JobCard_Detail_Select", dsParam);
            //if (dsJobCardDetail.Tables[0].Rows.Count == 0) dsJobCardDetail.Tables[0].Rows.Add(dsJobCardDetail.Tables[0].NewRow());
            ViewState["Job"] = dsJobCardDetail.Tables[0]; gvJobCard.DataSource = dsJobCardDetail; gvJobCard.DataBind();

            dsJobCardParts = new DataSet();
            dsJobCardParts = myFunc.FillDataSet("sp_W2_JobCard_PartsDetail_Select", dsParam);
            //if (dsJobCardParts.Tables[0].Rows.Count == 0) dsJobCardParts.Tables[0].Rows.Add(dsJobCardParts.Tables[0].NewRow());
            ViewState["Parts"] = dsJobCardParts.Tables[0]; gvJobCardParts.DataSource = dsJobCardParts; gvJobCardParts.DataBind();
            lknStock.Visible = true;
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

            dsJobCardBay = new DataSet();
            dsJobCardBay = myFunc.FillDataSet("sp_W2_JobCard_JobCardTech_Select", dsParam);
            ViewState["Bay"] = dsJobCardBay.Tables[0]; grdBay.DataSource = dsJobCardBay; grdBay.DataBind();

            dsJobCardBoutParts = new DataSet();
            dsJobCardBoutParts = myFunc.FillDataSet("sp_W2_JobCard_BoutPartsDetail_Select", dsParam);
            //if (dsJobCardConParts.Tables[0].Rows.Count == 0) dsJobCardConParts.Tables[0].Rows.Add(dsJobCardConParts.Tables[0].NewRow());
            ViewState["BoutParts"] = dsJobCardBoutParts.Tables[0]; gvJobCardPartsBought.DataSource = dsJobCardBoutParts; gvJobCardPartsBought.DataBind();
            Session["BoutPartsDataTables"] = dsJobCardBoutParts.Tables[0];


        }

        private void setVehcileInfo(DataSet dsJobCardMaster)
        {
            DataTable dt;
            //ddlRegNo.Text = dsJobCardMaster.Tables[0].Rows[0]["RegNo"].ToString();
            txtChassisNo.Text = dsJobCardMaster.Tables[0].Rows[0]["ChassisNo"].ToString();
            txtEngineNo.Text = dsJobCardMaster.Tables[0].Rows[0]["EngineNo"].ToString();
            txtCustomer.Text = dsJobCardMaster.Tables[0].Rows[0]["CusCode"].ToString();
            txtCustomerDesc.Text = dsJobCardMaster.Tables[0].Rows[0]["Customer"].ToString();
            txtEndUser.Text = dsJobCardMaster.Tables[0].Rows[0]["CusCode"].ToString();
            txtEndUserDesc.Text = dsJobCardMaster.Tables[0].Rows[0]["EndUser"].ToString();
            txtUserPhNo.Text = dsJobCardMaster.Tables[0].Rows[0]["Phone"].ToString();
            txtBrand.Text = dsJobCardMaster.Tables[0].Rows[0]["BrandCode"].ToString();
            txtBrandDesc.Text = dsJobCardMaster.Tables[0].Rows[0]["Brand"].ToString();
            txtProduct.Text = dsJobCardMaster.Tables[0].Rows[0]["ProdCode"].ToString();
            txtVersion.Text = dsJobCardMaster.Tables[0].Rows[0]["VersionCode"].ToString();
            txtCntPerName.Text = dsJobCardMaster.Tables[0].Rows[0]["ContPersonName"].ToString();
            txtCntPerCell.Text = dsJobCardMaster.Tables[0].Rows[0]["ContPersonCell"].ToString();
            txtVehicleCategory.Text = dsJobCardMaster.Tables[0].Rows[0]["VehicleCategory"].ToString();
            txtCusType.Text = dsJobCardMaster.Tables[0].Rows[0]["CusTypeDesc"].ToString();
            txtCusTypeCode.Text = dsJobCardMaster.Tables[0].Rows[0]["CusTypeCode"].ToString();
            dt = myFunc.GetData("Select C.CusDesc,C.CreditDays,C.CreditLimit ,isnull(round(isnull(CV.NetAmountCustomer,0)-isnull(CV.PaidC,0),0),0) as Outstanding from Customer C inner join CustomerInvoice CV on CV.DealerCode = C.DealerCode and CV.CusCode = C.CusCode where C.DealerCode='" + Session["DealerCode"].ToString() + "' and C.CusCode='" + txtCustomer.Text + "' "); //ddlEstNo.SelectedValue.ToString().Trim()
            if (dt.Rows.Count > 0) {
                txtCreditDays.Text = dt.Rows[0]["CreditDays"].ToString();
                txtCreditLimit.Text = dt.Rows[0]["CreditLimit"].ToString();
                txtOutStanding.Text = dt.Rows[0]["Outstanding"].ToString();
            }

            SqlParameter[] param_Schedule = {
                                                                new SqlParameter("@ProdCode",SqlDbType.VarChar,10),
                                                                new SqlParameter("@VersionCode",SqlDbType.Char,3)
                                                              };
            param_Schedule[0].Value = txtProduct.Text.Trim();
            param_Schedule[1].Value = txtVersion.Text.Trim();
            objMBLL.FillDrp_SP(ddlScheduleJC, "sp_2W_MaintainenceSchedule_Select", "ScheduleCode", "KM", param_Schedule, true, "Select", false, "");

            if (dsJobCardMaster.Tables[0].Columns.Contains("SIRMaster"))
            {
                txtSIRMasterId.Text = dsJobCardMaster.Tables[0].Rows[0]["SIRMaster"].ToString();
            }

            //txtVersionDesc.Text = grl.GetStringValuesAgainstCodes("VersionCode", txtVersion.Text, "ProdDesc", "Vehicle");
            txtVersionDesc.Text = dsJobCardMaster.Tables[0].Rows[0]["Product"].ToString();
            txtColor.Text = dsJobCardMaster.Tables[0].Rows[0]["Color"].ToString();
        }

        private void setJobCardInfo(DataSet dsJobCardMaster)
        {
            // txtGatePass.Text = ddlJobCardNo.Text.Trim();
            if (ViewState["JobCardCode"].ToString().Trim() != "")
            {
                ddlJobCardTypeCode.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["JobCardType"].ToString().Trim();
                if (dsJobCardMaster.Tables[0].Rows[0]["JobCardType"].ToString().Trim() == "002")
                {
                    lknPDI.Visible = true;
                }
                else if (dsJobCardMaster.Tables[0].Rows[0]["JobCardType"].ToString().Trim() == "008")
                {
                    ddlJobCardType_SelectedIndexChanged(null, null);
                }
                //txtJobType.Text = dsJobCardMaster.Tables[0].Rows[0]["JobTypeCode"].ToString();
                //txtJobTypeDesc.Text = grl.GetStringValuesAgainstCodes("jobTypeCode", txtJobType.Text, "JobTypeDesc", "JobTypeMaster");
                //ddlJobType.Value = dsJobCardMaster.Tables[0].Rows[0]["JobTypeCode"].ToString();
                txtRemarks.Text = dsJobCardMaster.Tables[0].Rows[0]["Remarks"].ToString().Trim();
                txtKM.Text = dsJobCardMaster.Tables[0].Rows[0]["KM"].ToString();

                txtRecieptDate.Text = Convert.ToDateTime(dsJobCardMaster.Tables[0].Rows[0]["RecDate"].ToString()).ToString("dd/MM/yyyy");//(grl.GetDate(dsJobCardMaster.Tables[0].Rows[0]["RecDate"].ToString())).ToString();
                txtReciptTime.Text = Convert.ToDateTime(dsJobCardMaster.Tables[0].Rows[0]["RecTime"].ToString()).ToString("HH:mm"); //grl.GetTime(dsJobCardMaster.Tables[0].Rows[0]["RecTime"].ToString()).ToString();
                txtDeliveryDate.Text = Convert.ToDateTime(dsJobCardMaster.Tables[0].Rows[0]["DelvDate"].ToString()).ToString("dd/MM/yyyy");//(grl.GetDate(dsJobCardMaster.Tables[0].Rows[0]["DelvDate"].ToString())).ToString();
                txtDeliveryTime.Text = Convert.ToDateTime(dsJobCardMaster.Tables[0].Rows[0]["DelvTime"].ToString()).ToString("HH:mm");// grl.GetTime(dsJobCardMaster.Tables[0].Rows[0]["DelvTime"].ToString()).ToString();
                txtPromistedTime.Text = dsJobCardMaster.Tables[0].Rows[0]["PromiseTime"].ToString() == "" ? "" : Convert.ToDateTime(dsJobCardMaster.Tables[0].Rows[0]["PromiseTime"].ToString()).ToString("HH:mm");
                ddlStatusJC.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["JobStatus"].ToString().Trim();
                string a = dsJobCardMaster.Tables[0].Rows[0]["promiseDate"].ToString().Trim();
                txtPromisedDate.Text = (a == "" ? DateTime.Now.ToString("dd-MM-yyyy") : Convert.ToDateTime(a).ToString("dd-MM-yyyy"));

                string b = dsJobCardMaster.Tables[0].Rows[0]["JobCardDate"].ToString().Trim();
                txtJobCardDate.Text = (b == "" ? DateTime.Now.ToString("dd-MM-yyyy") : Convert.ToDateTime(b).ToString("dd-MM-yyyy"));

                lblJobCardTotal.Text = dsJobCardMaster.Tables[0].Rows[0]["JobsTotal"].ToString().Trim();
                lblPartsTotal.Text = dsJobCardMaster.Tables[0].Rows[0]["PartsTotal"].ToString().Trim();
                txtLubTotal.Text = dsJobCardMaster.Tables[0].Rows[0]["LubPartsTotal"].ToString().Trim();
                //  txtSubletTotal.Text = dsJobCardMaster.Tables[0].Rows[0]["SubletTotal"].ToString().Trim();

                txtJobCardTotal.Text = Convert.ToString(Math.Floor(
                                                        Convert.ToDecimal(dsJobCardMaster.Tables[0].Rows[0]["JobsTotal"].ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : dsJobCardMaster.Tables[0].Rows[0]["JobsTotal"].ToString().Replace("&nbsp;", "").Trim()) +
                                                        Convert.ToDecimal(dsJobCardMaster.Tables[0].Rows[0]["PartsTotal"].ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : dsJobCardMaster.Tables[0].Rows[0]["PartsTotal"].ToString().Replace("&nbsp;", "").Trim()) +
                                                        Convert.ToDecimal(dsJobCardMaster.Tables[0].Rows[0]["LubPartsTotal"].ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : dsJobCardMaster.Tables[0].Rows[0]["LubPartsTotal"].ToString().Replace("&nbsp;", "").Trim()) +
                                                        Convert.ToDecimal(dsJobCardMaster.Tables[0].Rows[0]["SubletTotal"].ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : dsJobCardMaster.Tables[0].Rows[0]["SubletTotal"].ToString().Replace("&nbsp;", "").Trim()) +
                                                         Convert.ToDecimal(dsJobCardMaster.Tables[0].Rows[0]["BoutTotal"].ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : dsJobCardMaster.Tables[0].Rows[0]["BoutTotal"].ToString().Replace("&nbsp;", "").Trim())));

                ddlAdvisorCode.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["Advisor"].ToString().Trim();
                lblSIRMaster.Text = dsJobCardMaster.Tables[0].Rows[0]["SIRMaster"].ToString().Trim();
                txtvaluables.Text = dsJobCardMaster.Tables[0].Rows[0]["RecRemarks"].ToString().Trim();
                txtCouponNo.Text = dsJobCardMaster.Tables[0].Rows[0]["CouponNo"].ToString().Trim();
                //grl = new General();
                //txtAdvisorDesc.Text = grl.GetStringValuesAgainstCodes("EMPCODE", ddlAdvisor.SelectedItem .Text, "EMPNAME", "dealerEmp");
                txtGatePass.Text = dsJobCardMaster.Tables[0].Rows[0]["GatePass"].ToString().Trim();
                if (dsJobCardMaster.Tables[0].Rows[0]["Schedule"].ToString().Trim() != "")
                {
                    ddlScheduleJC.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["Schedule"].ToString().Trim();
                }
                if (dsJobCardMaster.Tables[0].Rows[0]["EstimateCode"].ToString().Trim() != "")
                {
                    txtExtno.Text = dsJobCardMaster.Tables[0].Rows[0]["EstimateCode"].ToString(); //ddlEstNo.SelectedValue 
                }
                ddlPayMode.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["PaymentMode"].ToString().Trim();
                ddlBillingType.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["Billing"].ToString().Trim();
                txtFuel.Text = dsJobCardMaster.Tables[0].Rows[0]["FuelSelection"].ToString().Trim();
                txtAvgKm.Text = dsJobCardMaster.Tables[0].Rows[0]["AvgKM"].ToString().Trim();
                ddlTaxType.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["hdnTaxCode"].ToString();
                hdnTaxCode.Value = dsJobCardMaster.Tables[0].Rows[0]["hdnTaxCode"].ToString();
                hdnTaxType.Value = dsJobCardMaster.Tables[0].Rows[0]["hdnTaxType"].ToString();
                // ddlTaxType.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["TaxAppCode"].ToString();
                txtTaxPolicyDesc.Text = dsJobCardMaster.Tables[0].Rows[0]["TaxAppDesc"].ToString();
                txtManJobCardNo.Text = dsJobCardMaster.Tables[0].Rows[0]["ManJobCardNo"].ToString();
                txtRepairSugest.Text = dsJobCardMaster.Tables[0].Rows[0]["RepairSuguestion"].ToString();
                txtCusRemarks.Text = dsJobCardMaster.Tables[0].Rows[0]["CusRemarks"].ToString();
                txtSIRMasterId.Text = dsJobCardMaster.Tables[0].Rows[0]["SIRMaster"].ToString();

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
            //if (ddlJobCardCode.SelectedIndex != 0)
            //{
            //    ddlJobCardCode.SelectedIndex = 0;
            //}
            txtJobCardCode.Text = "";
            ddlBay.SelectedIndex = 0;
            txtExtno.Text = "";
            //ddlJobCardNo.Text = "";
            //ddlJobCardNo.Enabled = true;
            txtGatePass.Text = "";
            txtChassisNo.Text = string.Empty;
            txtEngineNo.Text = string.Empty;
            //ddlRegNo.Text = "--Select--";
            //ddlChassisNo.Text = "--Select--";
            //ddlEngineNo.Text = "--Select--";
            txtUserPhNo.Text = "";
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
            txtCusRemarks.Text = "";
            txtRepairSugest.Text = "";
            txtManJobCardNo.Text = "";
            txtCouponNo.Text = "";
            ddlTaxType.SelectedIndex = 0;
            ddlStatusJC.SelectedIndex = 0;
            lblJobCardTotal.Text = "0";
            lblPartsTotal.Text = "0";
            txtLubTotal.Text = "0";
            txtSubletTotal.Text = "0";
            txtColor.Text = "";
            txtTaxPolicyCode.Text = "";
            txtTaxPolicyDesc.Text = "";
            txtVehicleCategory.Text = "";
            txtCreditDays.Text = "";
            txtCreditLimit.Text = "";
            txtBAmt.Text = "";
            hdnTaxType.Value = "";
            hdnTaxType.Value = "";
            txtCntPerCell.Text = "";
            txtCntPerName.Text = "";
            ddlAdvisorCode.SelectedIndex = 0;
            createSubletDT();
            createLubDT();
            createPartsDT();
            createJobDT();
            createConPartsDT();
            createBayDT();
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
            //ddlVehRegNo.SelectedIndex = 0;
            txtVehRegNo.Text = "";
            txtDep.Text = string.Empty;
            lblSIRMaster.Text = "";
            txtAvgKm.Text = "";
            txtFuel.Text = "";
            //ddlJobCardNo.Text = "";
            //ddlJobCardNo.DataBind();
            ///Load_ddlJobCardCode();
            txtvaluables.Text = "";
            ddlBay.SelectedIndex = 0;
            ddlJobCardTypeCode.SelectedIndex = 0;
            ddlPayMode.SelectedIndex = 0;
            ddlBillingType.SelectedIndex = 0;
            ddlAdvisorCode.SelectedIndex = 0;
            ddlStatusJC.SelectedIndex = 0;
            if (ddlScheduleJC.Items.Count > 1)
            {
                ddlScheduleJC.SelectedIndex = 0;
            }

            btnSaveJC.Enabled = true;

            txtRemarks.Text = "";
            txtKM.BorderColor = Color.Black;
            txtJobCardTotal.Text = "";

            Session["JobCardCode"] = null;
            TabContainer1.ActiveTabIndex = 0;
        }

        private void ClearJobTextBoxes()
        {
            TextBox[] txts = { txtJobRemarks, txtLabor };
            ClearTextBoxes(txts);
            //ddlTechnicianEmpCode.SelectedIndex = 0;
            //ddlJobs_Labor.SelectedIndex = 0;
            txtJobs_Labor.Text = "";
            txtJobsDesc.Text = "";
            ddlBay.SelectedIndex = 0;
            //txtSchedual.Text = "";
        }
        private void ClearBoutPartsTextBoxes()
        {
            TextBox[] txts = { txtBItemCode, txtBItemDesc, txtBPartNo, txtBPrice, txtBQty, txtBAmt };
            ClearTextBoxes(txts);
            //ddlTechnicianEmpCode.SelectedIndex = 0;
            //ddlJobs_Labor.SelectedIndex = 0;
            //txtJobs_Labor.Text = "";
            //txtJobsDesc.Text = "";
            //ddlBay.SelectedIndex = 0;
            //txtSchedual.Text = "";
        }

        private void ClearPartsTextBoxes()
        {
            TextBox[] txts = { txtQuantity, txtPartPrice, txtDep };
            ClearTextBoxes(txts);
            txtPartsRecQuantity.Text = "0";
            //ddlPartCode.SelectedIndex = 0;
            txtItemCode.Text = string.Empty;
            txtItemDesc.Text = string.Empty;
            txtPartItemNo_Parts.Text = string.Empty;
        }
        private void ClearBPartsTextBoxes()
        {
            TextBox[] txts = { txtBQty, txtBPrice, txtBPartNo, txtBPartNo, txtBItemDesc, txtBItemCode, txtBAmt };
            ClearTextBoxes(txts);
            //txtPartsRecQuantity.Text = "0";
            ////ddlPartCode.SelectedIndex = 0;
            //txtItemCode.Text = string.Empty;
            //txtItemDesc.Text = string.Empty;
            //txtPartItemNo_Parts.Text = string.Empty;
        }
        private void ClearLubricantsTextBoxes()
        {
            TextBox[] txts = { txtLubPartDesc, txtLubPrice, txtLubQuantity };
            ClearTextBoxes(txts);
            txtLubRecQuantity.Text = "0";
            //ddlLubCode.SelectedIndex = 0;
            txtLubCode.Text = "";
            txtLubPartDesc.Text = "";
        }
        private void ClearJobSubletTextBoxes()
        {
            TextBox[] txts = { txtSubletJobDesc, txtSubletPayAmnt, txtSubletRemarks, txtSubletIncAmnt };
            ClearTextBoxes(txts);
            ddlVendorCode.SelectedIndex = 0;
            //ddlJobs_Sublet.SelectedIndex = 0;
            txtSubletJobCode.Text = "";
            txtSubletJobDesc.Text = "";
            ddlBay.SelectedIndex = 0;

        }
        public void ClearTextBoxes(TextBox[] textBoxes)
        {
            for (int i = 0; i < textBoxes.Length; i++)
            { textBoxes[i].Text = string.Empty; }
        }
        private void ClearConPartsTextBoxes()
        {
            TextBox[] txts = { txtConQuantity, txtConPartDesc, txtConPartPrice, txtPartItemNoConParts };
            ClearTextBoxes(txts); txtConPartsRecQuantity.Text = "0";
            //ddlConsumableParts.SelectedIndex = 0;
            txtConPartCode.Text = "";
            txtConPartDesc.Text = "";
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

            PartRequisition = grl.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "PartRequisition", "Dealer", Session["DealerCode"].ToString());
            /////////////////
            if (gvJobCardParts.Rows.Count > 0 || gvLubParts.Rows.Count > 0)
            {
                if (PartRequisition == "DMS")
                {
                    if (txtSIRMasterId.Text.Trim() == string.Empty)
                    {
                        txtSIRMasterId.Text = grl.AutoGen("sirmaster", "SIRNO", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                    }
                    else
                    {
                        txtSIRMasterId.Text = "";
                    }

                }
                else if (PartRequisition == "ERP")
                {
                    txtSIRMasterId.Text = "";

                }



            }
            else
            {
                txtSIRMasterId.Text = "";
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
           /*10*/ new SqlParameter("@ProdCode",SqlDbType.VarChar,20),
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
            /*45*/ new SqlParameter("@AvgKm",SqlDbType.Char,4),
            /*46*/ new SqlParameter("@FuelSelection",SqlDbType.Char,4),
            /*47*/ new SqlParameter("@JobCardDate",SqlDbType.DateTime),
             /*47*/ new SqlParameter("@ContPersonName",SqlDbType.VarChar,50),
              /*47*/ new SqlParameter("@ContPersonCell",SqlDbType.Char,13),
               /*47*/ new SqlParameter("@VehicleCategory",SqlDbType.Char,3),
                 /*47*/ new SqlParameter("@BoutTotal",SqlDbType.Decimal),
                   /*47*/ new SqlParameter("@TaxAppCode",SqlDbType.Char,6),
                   /*47*/ new SqlParameter("@TaxType",SqlDbType.Char,15),
                     /*47*/ new SqlParameter("ManJobCardNo",SqlDbType.VarChar,50),
                       /*47*/ new SqlParameter("@RepairSuguestion",SqlDbType. VarChar,100),
                         /*47*/ new SqlParameter("@CusRemark",SqlDbType.VarChar,100),
                           /*47*/ new SqlParameter("@CouponNo",SqlDbType.VarChar,50),
                           /*47*/ new SqlParameter("@ScheduleCode",SqlDbType.Char,08),
        };



            JobCardMaster_param[0].Value = Session["DealerCode"].ToString();
            //Auto Code Generation Decision on Insert OR Update
            //ddlJobCardNo.Text = grl.AutoGen("JobCardMaster", "JobCardCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy")); 
            //JobCardMaster_param[1].Value = ddlJobCardNo.Text;         
            if (txtExtno.Text.Trim() == "") //ddlEstNo.SelectedIndex
            {
                JobCardMaster_param[2].Value = string.Empty;
            }
            else
            {
                JobCardMaster_param[2].Value = txtExtno.Text;//ddlEstNo.SelectedValue.ToString().Trim(); //EstimateCode        
            }

            JobCardMaster_param[3].Value = ddlJobCardTypeCode.SelectedValue.ToString().Trim();

            JobCardMaster_param[4].Value = txtCustomer.Text.Trim().ToUpper();

            JobCardMaster_param[5].Value = txtEndUserDesc.Text;

            JobCardMaster_param[6].Value = txtBrand.Text.Trim();

            //JobCardMaster_param[7].Value = ddlVehRegNo.SelectedValue.ToString(). Trim().ToUpper();
            JobCardMaster_param[7].Value = txtVehRegNo.Text.Trim().ToUpper();

            JobCardMaster_param[8].Value = txtEngineNo.Text.Trim().ToUpper();

            JobCardMaster_param[9].Value = txtChassisNo.Text.Trim().ToUpper();

            JobCardMaster_param[10].Value = txtProduct.Text.Trim().ToUpper();

            JobCardMaster_param[11].Value = txtVersion.Text.Trim().ToUpper();

            JobCardMaster_param[12].Value = grl.SaveDate(txtRecieptDate.Text);

            JobCardMaster_param[13].Value = Convert.ToDateTime(txtReciptTime.Text).ToString("HH:mm");//grl.SaveTime(txtReciptTime.Text);

            JobCardMaster_param[14].Value = grl.SaveDate(txtPromisedDate.Text);//grl.SaveDate(DatePicker1.Text).ToString();

            JobCardMaster_param[15].Value = Convert.ToDateTime(txtPromistedTime.Text).ToString("HH:mm"); //grl.SaveTime(txtPromistedTime.Text);

            JobCardMaster_param[16].Value = grl.SaveDate(txtRecieptDate.Text);//grl.SaveDate(txtDeliveryDate.Text.ToString());

            JobCardMaster_param[17].Value = Convert.ToDateTime(txtDeliveryTime.Text).ToString("HH:mm");//grl.SaveTime(txtDeliveryTime.Text);

            JobCardMaster_param[18].Value = "";//ddlJobType.SelectedItem.Value.ToString();

            JobCardMaster_param[19].Value = txtKM.Text.Trim();

            JobCardMaster_param[20].Value = string.Empty;// Updates when invoice will be saved. Value will be the jobcard No.        

            JobCardMaster_param[21].Value = txtRemarks.Text.ToUpper().Trim();

            JobCardMaster_param[22].Value = string.Empty; //101        

            JobCardMaster_param[23].Value = string.Empty; //101        

            if (ddlScheduleJC.SelectedIndex == 0)
            {
                JobCardMaster_param[24].Value = "";
            }
            else
            {
                JobCardMaster_param[24].Value = ddlScheduleJC.SelectedItem.ToString();
            }


            JobCardMaster_param[25].Value = string.Empty;// control should be visible false        

            JobCardMaster_param[26].Value = lblJobCardTotal.Text.Trim();

            JobCardMaster_param[27].Value = lblPartsTotal.Text.Trim();

            JobCardMaster_param[28].Value = txtLubTotal.Text;

            JobCardMaster_param[29].Value = txtSubletTotal.Text;

            JobCardMaster_param[30].Value = txtvaluables.Text.Trim();

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
            JobCardMaster_param[36].Value = GlobalVar.mUserIPAddress;

            JobCardMaster_param[37].Value = "C"; //         

            JobCardMaster_param[38].Value = txtEndUser.Text.Trim().ToUpper();

            JobCardMaster_param[39].Value = "0"; // @ConsumableTotal would not be saved for minar items like . angry papper,elphi,petrol. amount is not recieved from customer        

            JobCardMaster_param[40].Value = ddlStatusJC.SelectedValue.ToString();

            JobCardMaster_param[41].Value = ddlPayMode.SelectedValue.ToString().Trim();
            JobCardMaster_param[42].Value = "";
            JobCardMaster_param[43].Value = txtAvgKm.Text.Trim();
            JobCardMaster_param[44].Value = txtFuel.Text.Trim();
            JobCardMaster_param[45].Value = grl.SaveDate(txtJobCardDate.Text);
            JobCardMaster_param[46].Value = txtCntPerName.Text;
            JobCardMaster_param[47].Value = txtCntPerCell.Text;
            JobCardMaster_param[48].Value = txtVehicleCategory.Text;
            JobCardMaster_param[49].Value = lblBInvAmt.Text;

            JobCardMaster_param[50].Value = hdnTaxCode.Value;
            JobCardMaster_param[51].Value = hdnTaxType.Value;
            JobCardMaster_param[52].Value = txtManJobCardNo.Text;
            JobCardMaster_param[53].Value = txtRepairSugest.Text;
            JobCardMaster_param[54].Value = txtCusRemarks.Text;
            JobCardMaster_param[55].Value = txtCouponNo.Text;
            if (ddlScheduleJC.SelectedIndex == 0)
            {
                JobCardMaster_param[56].Value = "";
            }
            else
            {
                JobCardMaster_param[56].Value = ddlScheduleJC.SelectedValue.ToString();
            }
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

                        if (txtSIRMasterId.Text != "")
                        {
                            if (grl.IsExist("SIRMaster", txtSIRMasterId.Text.Trim(), "JobCardMaster", Session["DealerCode"].ToString()))
                            {

                                grl.UserMsg(lblMsg, Color.Red, "This SIRNo is Already Exist", txtSIRMasterId);
                                ObjTrans.RollBackTransaction(ref Trans);
                                return;
                            }
                        }


                        if (txtJobCardCode.Text == "")
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
                            bool f = (rowInJobCardBoutPartsDetail(gvJobCardConParts) == true ? Inser_JobCardBoutPartsDetail() : false);

                            if (gvJobCardParts.Rows.Count > 0 || gvLubParts.Rows.Count > 0)
                            {
                                if (PartRequisition == "DMS")
                                {
                                    Inser_SIR_Master_Detail();

                                }
                                else if (PartRequisition == "ERP")
                                {
                                    txtSIRMasterId.Text = "";

                                }



                            }
                            else
                            {
                                txtSIRMasterId.Text = "";
                            }






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
                            if (grl.CodeExists("JobCardMaster", "SIRMaster", txtSIRMasterId.Text.Trim(), "And JobCardCode <> '" + txtJobCardCode.Text.Trim() + "'", GlobalVar.mDealerCode)) //ddlJobCardCode.SelectedValue.ToString().Trim() 
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
                        if (ViewState["JobCardCode"].ToString().Trim() != "")
                        {
                            JobCardMaster_param[1].Value = ViewState["JobCardCode"].ToString().Trim();
                        }
                        else
                        {
                            JobCardMaster_param[1].Value = txtJobCardCode.Text.Trim();// ddlJobCardCode.SelectedValue.ToString().Trim();
                        }
                        if (myFunc.ExecuteSP_NonQuery("[sp_Update_JobcardMaster]", JobCardMaster_param, Trans))
                        {
                            Inser_JobCardDetail();
                            Inser_JobCardPartsDetail();
                            Inser_JobCardLubricanteDetail();
                            Inser_JobCardSubletDetail();
                            Inser_JobCardConPartsDetail();
                            Inser_JobCardBoutPartsDetail();
                            bool c;
                            bool d;
                            if (gvJobCardParts.Rows.Count > 0 || gvLubParts.Rows.Count > 0)
                            {
                                if (PartRequisition == "DMS")
                                {
                                    Inser_SIR_Master_Detail();

                                }
                                else if (PartRequisition == "ERP")
                                {
                                    txtSIRMasterId.Text = "";

                                }



                            }
                            else
                            {
                                txtSIRMasterId.Text = "";
                            }

                        }
                        else { objMBLL.ShowMessageBox("Update Fail", lblMsg); }
                    }

                    if (ObjTrans.CommittTransaction(ref Trans) == true)
                    {
                        lblMsg.ForeColor = Color.Green;
                        if (txtJobCardCode.Text.Trim() == "") //ddlJobCardCode.SelectedIndex
                        {
                            lblMsg.Text = "Record Saved Successfully: " + HFJobCard.Value;
                        }
                        else
                        {
                            lblMsg.Text = "Record Updated Successfully: " + HFJobCard.Value;
                        }

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
        private bool rowInJobCardBoutPartsDetail(GridView gvJobCardConParts)
        {
            search_item = false;
            if (gvJobCardPartsBought.Rows.Count > 0)
            {
                search_item = true;
            }
            //foreach (GridViewRow row in gvJobCardParts.Rows) { if (row.Cells[2].Text.Replace("&nbsp;", "").Trim() != "") search_item = true; }
            return search_item;
        }


        private void Insert_in_JobDT(DataRow rowJobDT)
        {
            //DataTable dt = myFunc.GetData("select DefJobDesc from DefaultJob where DealerCode IN('"+ Session["DealerCode"].ToString() +"','COMON') And  DefJobCode='" + ddlJobs_Labor.SelectedValue.ToString() + "'");
            DataTable dt = myFunc.GetData("select DefJobDesc from DefaultJob where DealerCode IN('" + Session["DealerCode"].ToString() + "','COMON') And  DefJobCode='" + txtJobs_Labor.Text + "'");
            //rowJobDT["JobCode"] = ddlJobs_Labor.SelectedValue.ToString().Trim();
            rowJobDT["JobCode"] = txtJobs_Labor.Text.Trim();
            rowJobDT["DefJobDesc"] = dt.Rows[0]["DefJobDesc"].ToString().Trim();
            rowJobDT["EmpCode"] = ddlTechnicianEmpCode.SelectedValue.ToString().Trim();
            rowJobDT["EmpName"] = ddlTechnicianEmpCode.SelectedItem.ToString().Trim();
            rowJobDT["Bay"] = ddlBay.SelectedValue.ToString().Trim();
            rowJobDT["BayDesc"] = ddlBay.SelectedItem.ToString().Trim();
            rowJobDT["Amount"] = txtLabor.Text;
            if (ddlJobCardTypeCode.SelectedValue == "001")
            {
                if (sysfun.IsExist("JobCode", txtJobs_Labor.Text, "FreeJobs", "COMON", "and VehicleCategory='" + Session["VehicleCategory"].ToString() + "'"))
                {
                    rowJobDT["Type"] = "F";
                }
                else
                {
                    rowJobDT["Type"] = "J";
                }

            }
            else if (ddlJobCardTypeCode.SelectedValue == "002")
            {
                if (sysfun.IsExist("JobCode", txtJobs_Labor.Text, "FreeJobs", "COMON", "and VehicleCategory='" + Session["VehicleCategory"].ToString() + "'"))
                {
                    rowJobDT["Type"] = "F";
                }
                else
                {
                    rowJobDT["Type"] = "J";
                }

            }
            else if (ddlJobCardTypeCode.SelectedValue == "013")
            {
                if (sysfun.IsExist("JobCode", txtJobs_Labor.Text, "FreeJobs", "COMON", "and VehicleCategory='" + Session["VehicleCategory"].ToString() + "'"))
                {
                    rowJobDT["Type"] = "F";
                }
                else
                {
                    rowJobDT["Type"] = "J";
                }
            }
            else if (ddlJobCardTypeCode.SelectedValue == "018")
            {
                if (sysfun.IsExist("JobCode", txtJobs_Labor.Text, "FreeJobs", "COMON", "and VehicleCategory='" + Session["VehicleCategory"].ToString() + "'"))
                {
                    rowJobDT["Type"] = "F";
                }
                else
                {
                    rowJobDT["Type"] = "J";
                }

            }
            else if (ddlJobCardTypeCode.SelectedValue == "019")
            {
                if (sysfun.IsExist("JobCode", txtJobs_Labor.Text, "FreeJobs", "COMON", "and VehicleCategory='" + Session["VehicleCategory"].ToString() + "'"))
                {
                    rowJobDT["Type"] = "F";
                }
                else
                {
                    rowJobDT["Type"] = "J";
                }

            }
            else if (ddlJobCardTypeCode.SelectedValue == "014")
            {
                if (sysfun.IsExist("JobCode", txtJobs_Labor.Text, "FreeJobs", "COMON", "and VehicleCategory='" + Session["VehicleCategory"].ToString() + "'"))
                {
                    rowJobDT["Type"] = "F";
                }
                else
                {
                    rowJobDT["Type"] = "J";
                }

            }
            else if (ddlJobCardTypeCode.SelectedValue == "012")
            {
                rowJobDT["Type"] = ddlJobType.SelectedValue;


            }
            else
            {
                rowJobDT["Type"] = "J";
            }


        }

        private void Insert_in_BayDT(DataRow rowBayDT)
        {
            rowBayDT["JobCardCode"] = txtJobCardCode.Text.Trim();
            //DataTable dt = myFunc.GetData("select DefJobDesc from DefaultJob where DealerCode IN('"+ Session["DealerCode"].ToString() +"','COMON') And  DefJobCode='" + ddlJobs_Labor.SelectedValue.ToString() + "'");
            DataTable dt = myFunc.GetData("select DefJobDesc , StandardTime from DefaultJob where DealerCode IN('" + Session["DealerCode"].ToString() + "','COMON') And  DefJobCode='" + txtJobs_Labor.Text + "'");
            //rowJobDT["JobCode"] = ddlJobs_Labor.SelectedValue.ToString().Trim();
            rowBayDT["JobCode"] = txtJobs_Labor.Text.Trim();
            rowBayDT["DefJobDesc"] = dt.Rows[0]["DefJobDesc"].ToString().Trim();
            rowBayDT["BayCode"] = ddlBay.SelectedValue.ToString().Trim();
            rowBayDT["BayDesc"] = ddlBay.SelectedItem.ToString().Trim();
            rowBayDT["EmpCode"] = ddlTechnicianEmpCode.SelectedItem.Value.ToString();
            rowBayDT["EmpName"] = ddlTechnicianEmpCode.SelectedItem.ToString();
            rowBayDT["StdTime"] = dt.Rows[0]["StandardTime"].ToString().Trim();
            rowBayDT["StartTime"] = DateTime.Now.ToString("hh:mm");
            rowBayDT["EndTime"] = "";
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
                        apStr = " and versioncode= '" + txtVersion.Text + "' and ProdCode='" + txtProduct.Text + "'";
                        if ((grl.IsExist("ProdCode", txtProduct.Text, "Vehicle", Session["DealerCode"].ToString(), apStr) == true ? true : false))
                        {
                            if (txtJobCardCode.Text.Trim() == "" && ViewState["JobCardCode"].ToString().Trim() == "") strAction = "Insert"; //ddlJobCardCode.SelectedIndex == 0
                            else
                            {
                                apStr = " and chassisno ='" + txtChassisNo.Text + "' and GatePass = null";
                                if (grl.IsExist("ChassisNo", txtChassisNo.Text, "JobCardMaster", Session["DealerCode"].ToString()))
                                {
                                    if (grl.IsExist("JobCardCode", txtJobCardCode.Text.Trim(), "JobCardMaster", Session["DealerCode"].ToString(), apStr)) //ddlJobCardCode.SelectedValue.ToString().Trim()
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
                //JobCardDetail_Delete_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                JobCardDetail_Delete_param[1].Value = txtJobCardCode.Text;
                SqlParameter[] JobCardDetail_Insert_param = {
                                                                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                new SqlParameter("@JobCardCode",SqlDbType.Char,8),
                                                                new SqlParameter("@JobCode",SqlDbType.VarChar,8),
                                                                new SqlParameter("@EmpCode",SqlDbType.Char,3),
                                                                new SqlParameter("@Amount",SqlDbType.Float),
                                                                new SqlParameter("@Type",SqlDbType.Char,1),
                                                                new SqlParameter("@BayCode",SqlDbType.Char,4)
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
                            JobCardDetail_Insert_param[1].Value = txtJobCardCode.Text.Trim() == "" ? HFJobCard.Value.Trim() : txtJobCardCode.Text; //ddlJobCardCode.SelectedValue.ToString().Trim()
                            JobCardDetail_Insert_param[2].Value = JobDT.Rows[i]["JobCode"].ToString();
                            JobCardDetail_Insert_param[3].Value = JobDT.Rows[i]["EmpCode"].ToString();
                            JobCardDetail_Insert_param[4].Value = Convert.ToDecimal(JobDT.Rows[i]["Amount"].ToString());
                            JobCardDetail_Insert_param[5].Value = JobDT.Rows[i]["Type"].ToString();
                            JobCardDetail_Insert_param[6].Value = JobDT.Rows[i]["Bay"].ToString();
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
                //JobCardPartsDetail_Delete_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                JobCardPartsDetail_Delete_param[1].Value = txtJobCardCode.Text;
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

                                                                 };

                if (myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_PartsDetail_Delete", JobCardPartsDetail_Delete_param, Trans))
                {
                    PartsDT = (DataTable)ViewState["Parts"];
                    DataRow[] drr = PartsDT.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {
                        JobCardPartsDetail_Insert_param[0].Value = Session["DealerCode"].ToString();
                        //JobCardPartsDetail_Insert_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                        JobCardPartsDetail_Insert_param[1].Value = txtJobCardCode.Text == "" ? HFJobCard.Value.Trim() : txtJobCardCode.Text;
                        if (PartsDT.Rows[i]["ItemCode"].ToString() != "")
                        {
                            JobCardPartsDetail_Insert_param[2].Value = PartsDT.Rows[i]["ItemCode"].ToString(); //Item Code System Generted num. Get it from lookup
                            JobCardPartsDetail_Insert_param[3].Value = PartsDT.Rows[i]["PartNo"].ToString();
                            JobCardPartsDetail_Insert_param[4].Value = PartsDT.Rows[i]["Qty"].ToString();
                            JobCardPartsDetail_Insert_param[5].Value = PartsDT.Rows[i]["Price"].ToString();
                            JobCardPartsDetail_Insert_param[6].Value = PartsDT.Rows[i]["RecQty"].ToString();
                            if (ddlJobCardTypeCode.SelectedItem.Text.Trim() == "INSURANCE")
                            {
                                JobCardPartsDetail_Insert_param[7].Value = PartsDT.Rows[i]["Type"].ToString(); //Type
                                JobCardPartsDetail_Insert_param[8].Value = PartsDT.Rows[i]["Dep"].ToString(); //Dep
                            }

                            else
                            {
                                JobCardPartsDetail_Insert_param[7].Value = PartsDT.Rows[i]["Type"].ToString(); //Type
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
        private bool Inser_JobCardBoutPartsDetail()
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
                JobCardPartsDetail_Delete_param[1].Value = txtJobCardCode.Text == "" ? HFJobCard.Value.Trim() : txtJobCardCode.Text;
                SqlParameter[] JobCardPartsDetail_Insert_param = {
                                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                                                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8),//1
                                                                    new SqlParameter("@ItemCode",SqlDbType.Char,8),//2
                                                                    new SqlParameter("@PartNo",SqlDbType.Char,18),//3
                                                                    new SqlParameter("@Qty",SqlDbType.Int),//4
                                                                    new SqlParameter("@PurchaseRate",SqlDbType.Decimal,(18)),//5
                                                                    new SqlParameter("@TotalPurchase",SqlDbType.Decimal,(18)),//6
                                                                    new SqlParameter("@InvRate",SqlDbType.Decimal,(18)),//7
                                                                    new SqlParameter("@TotalInvAmount",SqlDbType.Decimal,(18))//8
                                                                 };

                if (myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_BoutPartsDetail_Delete", JobCardPartsDetail_Delete_param, Trans))
                {
                    BoutDT = (DataTable)Session["BoutPartsDataTables"];
                    DataRow[] drr = BoutDT.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {
                        JobCardPartsDetail_Insert_param[0].Value = Session["DealerCode"].ToString();
                        //JobCardPartsDetail_Insert_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                        JobCardPartsDetail_Insert_param[1].Value = txtJobCardCode.Text == "" ? HFJobCard.Value.Trim() : txtJobCardCode.Text;
                        if (BoutDT.Rows[i]["ItemCode"].ToString() != "")
                        {

                            JobCardPartsDetail_Insert_param[2].Value = BoutDT.Rows[i]["ItemCode"].ToString(); //Item Code System Generted num. Get it from lookup
                            JobCardPartsDetail_Insert_param[3].Value = BoutDT.Rows[i]["PartNo"].ToString();
                            JobCardPartsDetail_Insert_param[4].Value = Convert.ToInt32(BoutDT.Rows[i]["Qty"].ToString());
                            JobCardPartsDetail_Insert_param[5].Value = Convert.ToDecimal(BoutDT.Rows[i]["Price"].ToString());
                            JobCardPartsDetail_Insert_param[6].Value = Convert.ToDecimal(BoutDT.Rows[i]["PurchaseTotal"].ToString());
                            JobCardPartsDetail_Insert_param[7].Value = Convert.ToDecimal(BoutDT.Rows[i]["InvRate"].ToString());//Type
                            JobCardPartsDetail_Insert_param[8].Value = Convert.ToDecimal(BoutDT.Rows[i]["InvoiceTotal"].ToString()); //Dep


                            myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_BoutPartsDetail_Insert", JobCardPartsDetail_Insert_param, Trans);
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
                //JobCardLubricanteDetail_Delete_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                JobCardLubricanteDetail_Delete_param[1].Value = txtJobCardCode.Text.Trim() == "" ? HFJobCard.Value.Trim() : txtJobCardCode.Text.Trim();

                SqlParameter[] JobCardLubricanteDetail_Insert_param = {
                                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8),
                                                                    new SqlParameter("@ItemCode",SqlDbType.VarChar,8),
                                                                    new SqlParameter("@PartNo",SqlDbType.Char,18),
                                                                    new SqlParameter("@Qty",SqlDbType.VarChar,10),
                                                                    new SqlParameter("@Price",SqlDbType.VarChar,50),
                                                                    new SqlParameter("@RecQty",SqlDbType.VarChar,50),
                                                                    new SqlParameter("@Type",SqlDbType.Char,01),
                                                                    new SqlParameter("@Dep",SqlDbType.VarChar,05)
                                                                  };

                if (myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_LubricanteDetail_Delete", JobCardLubricanteDetail_Delete_param, Trans))
                {
                    LubDT = (DataTable)ViewState["Lub"];
                    DataRow[] drr = LubDT.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {
                        JobCardLubricanteDetail_Insert_param[0].Value = Session["DealerCode"].ToString();
                        JobCardLubricanteDetail_Insert_param[1].Value = txtJobCardCode.Text.Trim() == "" ? HFJobCard.Value.Trim() : txtJobCardCode.Text.Trim();// ddlJobCardCode.SelectedValue.ToString().Trim();
                        if (LubDT.Rows[i]["ItemCode"].ToString() != "")
                        {
                            JobCardLubricanteDetail_Insert_param[2].Value = LubDT.Rows[i]["ItemCode"].ToString(); ; //Item Code System Generted num. Get it from lookup
                            JobCardLubricanteDetail_Insert_param[3].Value = LubDT.Rows[i]["PartNo"].ToString();
                            JobCardLubricanteDetail_Insert_param[4].Value = LubDT.Rows[i]["Qty"].ToString();
                            JobCardLubricanteDetail_Insert_param[5].Value = LubDT.Rows[i]["Price"].ToString();
                            JobCardLubricanteDetail_Insert_param[6].Value = LubDT.Rows[i]["RecQty"].ToString(); // 102 
                            JobCardLubricanteDetail_Insert_param[7].Value = LubDT.Rows[i]["Type"].ToString(); // 102 
                            JobCardLubricanteDetail_Insert_param[8].Value = LubDT.Rows[i]["Dep"].ToString(); // 102                        
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
            JobCardSubletDetail_Delete_param[1].Value = txtJobCardCode.Text.Trim() == "" ? HFJobCard.Value.Trim() : txtJobCardCode.Text.Trim();// ddlJobCardCode.SelectedValue.ToString().Trim();

            SqlParameter[] JobCardSubletDetail_Insert_param = {
                                                                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                new SqlParameter("@JobCardCode",SqlDbType.Char,8),
                                                                new SqlParameter("@VendorCode",SqlDbType.Char,6),
                                                                new SqlParameter("@JobCode",SqlDbType.Char,8),
                                                                new SqlParameter("@SubletRemarks",SqlDbType.VarChar,50),
                                                                new SqlParameter("@SubletDate",SqlDbType.DateTime),
                                                                new SqlParameter("@SubletAmount",SqlDbType.VarChar,50),
                                                                new SqlParameter("@PayableAmount",SqlDbType.VarChar,50),
                                                                new SqlParameter("@BayID",SqlDbType.Char,4)
                                                              };

            bool ans = myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_Sublet_Delete", JobCardSubletDetail_Delete_param, Trans);
            SubletDT = (DataTable)ViewState["SubLet"];
            DataRow[] drr = SubletDT.Select();
            for (int i = 0; i < drr.Length; i++)
            {
                JobCardSubletDetail_Insert_param[0].Value = Session["DealerCode"].ToString();
                JobCardSubletDetail_Insert_param[1].Value = txtJobCardCode.Text.Trim() == "" ? HFJobCard.Value.Trim() : txtJobCardCode.Text.Trim();// ddlJobCardCode.SelectedValue.ToString().Trim();

                if (SubletDT.Rows[i]["VendorCode"].ToString() != "")
                {
                    JobCardSubletDetail_Insert_param[2].Value = SubletDT.Rows[i]["VendorCode"].ToString();
                    JobCardSubletDetail_Insert_param[3].Value = SubletDT.Rows[i]["JobCode"].ToString();
                    JobCardSubletDetail_Insert_param[4].Value = SubletDT.Rows[i]["Remarks"].ToString();
                    JobCardSubletDetail_Insert_param[5].Value = grl.SaveDate(SubletDT.Rows[i]["SubletDate"].ToString());
                    JobCardSubletDetail_Insert_param[6].Value = SubletDT.Rows[i]["SubletAmount"].ToString();
                    JobCardSubletDetail_Insert_param[7].Value = SubletDT.Rows[i]["PayableAmount"].ToString();
                    JobCardSubletDetail_Insert_param[8].Value = SubletDT.Rows[i]["BayID"].ToString();
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
                SIRMaster_Insert_param[7].Value = txtJobCardCode.Text.Trim() == "" ? HFJobCard.Value.Trim() : txtJobCardCode.Text.Trim();// ddlJobCardCode.SelectedValue.ToString().Trim();
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
                SIRMaster_Insert_param[10].Value = GlobalVar.mUserIPAddress;


                if (myFunc.ExecuteSP_NonQuery("sp_W2_SIRMaster_Insert", SIRMaster_Insert_param, Trans))
                {
                    string jcno = txtJobCardCode.Text.Trim() == "" ? HFJobCard.Value.Trim() : txtJobCardCode.Text.Trim();// ddlJobCardCode.SelectedValue.ToString().Trim();
                    string strSIRNo = grl.GetStringValuesAgainstCodes("JobCardNO", jcno, "SIRNo", "SIRMaster", Session["DealerCode"].ToString());


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
                            SIRDetail_Insert_param[6].Value = 'P';
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
                //JobCardConPartsDetail_Delete_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                JobCardConPartsDetail_Delete_param[1].Value = txtJobCardCode.Text.Trim() == "" ? HFJobCard.Value.Trim() : txtJobCardCode.Text.Trim();// ddlJobCardCode.SelectedValue.ToString().Trim();

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
                        //JobCardConPartsDetail_Insert_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                        JobCardConPartsDetail_Insert_param[1].Value = txtJobCardCode.Text.Trim() == "" ? HFJobCard.Value.Trim() : txtJobCardCode.Text.Trim();// ddlJobCardCode.SelectedValue.ToString().Trim();
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
            //if (ddlPartCode.SelectedIndex == 0)
            //{

            //    grl.UserMsg(lblMsg, Color.Red, "Please enter Quantity", txtQuantity);
            //    return;
            //}
            //else
            //{
            //    lblMsg.Text = string.Empty;
            //}
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
            if (txtDep.Text != "" && SysFunction.CustomCDBL(txtDep.Text) > 0)
            {
                ddlJobCardTypeCode.SelectedValue = "012";
            }

            if (ddlJobCardTypeCode.SelectedItem.Text.Trim() == "INSURANCE")
            {
                if (txtDep.Text.Trim() == string.Empty)
                {
                    txtDep.Text = "0";
                }
                rowPartsDT["Dep"] = txtDep.Text.Trim();
                rowPartsDT["DepAmount"] = (Math.Round((SysFunctions.CustomCDBL(txtQuantity.Text.Trim()) * SysFunctions.CustomCDBL(txtPartPrice.Text.Trim()) / 100) * SysFunctions.CustomCDBL(txtDep.Text.Trim())));

            }
            else
            {
                rowPartsDT["Dep"] = "0";
                rowPartsDT["DepAmount"] = "0";
            }

            rowPartsDT["Qty"] = txtQuantity.Text.Trim();
            rowPartsDT["Price"] = txtPartPrice.Text.Trim();
            if (ddlJobCardTypeCode.SelectedValue == "001")
            {
                if (sysfun.IsExist("ItemCode", txtItemCode.Text, "FreeParts", "COMON", "and VehicleCategory='" + Session["VehicleCategory"].ToString() + "'"))
                {
                    rowPartsDT["Type"] = "F";
                }

            }
            else if (ddlJobCardTypeCode.SelectedValue == "013")
            {
                if (sysfun.IsExist("ItemCode", txtItemCode.Text, "FreeParts", "COMON", "and VehicleCategory='" + Session["VehicleCategory"].ToString() + "'"))
                {
                    rowPartsDT["Type"] = "F";
                }

            }
            else if (ddlJobCardTypeCode.SelectedValue == "018")
            {
                if (sysfun.IsExist("ItemCode", txtItemCode.Text, "FreeParts", "COMON", "and VehicleCategory='" + Session["VehicleCategory"].ToString() + "'"))
                {
                    rowPartsDT["Type"] = "F";
                }

            }
            else if (ddlJobCardTypeCode.SelectedValue == "019")
            {
                if (sysfun.IsExist("ItemCode", txtItemCode.Text, "FreeParts", "COMON", "and VehicleCategory='" + Session["VehicleCategory"].ToString() + "'"))
                {
                    rowPartsDT["Type"] = "F";
                }

            }
            else if (ddlJobCardTypeCode.SelectedValue == "012")
            {

                rowPartsDT["Type"] = lblPartType.Value;


            }
            else
            {
                rowPartsDT["Type"] = "P";
            }


            rowPartsDT["Total"] = double.Parse(txtQuantity.Text.Trim()) * double.Parse(txtPartPrice.Text.Trim());
            rowPartsDT["ItemCode"] = txtItemCode.Text;
            rowPartsDT["RecQty"] = txtPartsRecQuantity.Text.Trim();

        }
        protected void btnLubAddInGrid_Click(object sender, EventArgs e)
        {
            if (txtLubCode.Text == "") //ddlLubCode.SelectedIndex
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
                        | rowLubDT["ItemCode"].ToString().Trim() == txtLubCode.Text.Trim())
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
            rowLubDT["ItemCode"] = txtLubCode.Text.Trim();//ddlLubCode.SelectedValue.ToString().Trim();
            rowLubDT["RecQty"] = txtLubRecQuantity.Text.Trim();
            rowLubDT["Type"] = lblLubType.Value;
            if (ddlJobCardTypeCode.SelectedValue != "012")

            {
                rowLubDT["Dep"] = "0";
                rowLubDT["DepAmount"] = "0";

            }
            else
            {
                rowLubDT["Dep"] = txtLubDep.Text;
                rowLubDT["DepAmount"] = (Math.Round((SysFunctions.CustomCDBL(txtLubQuantity.Text.Trim()) * SysFunctions.CustomCDBL(txtLubPrice.Text.Trim()) / 100) * SysFunctions.CustomCDBL(txtLubDep.Text.Trim())));
            }



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
            //rowSubletDT["JobCode"] = ddlJobs_Sublet.SelectedValue.ToString().Trim(); 
            rowSubletDT["JobCode"] = txtSubletJobCode.Text.Trim();
            rowSubletDT["JobDescription"] = txtSubletJobDesc.Text.Trim();
            rowSubletDT["Remarks"] = txtSubletRemarks.Text.Trim().ToUpper();
            rowSubletDT["SubletDate"] = txtSubletDate.Text.Trim();
            rowSubletDT["SubletAmount"] = txtSubletIncAmnt.Text.Trim();
            rowSubletDT["PayableAmount"] = txtSubletPayAmnt.Text;
            //rowSubletDT["BayID"] = ddlBaySubletJob.SelectedValue.Trim();
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
        protected void CalculateTotal()
        {
            try
            {
                totEst = 0;
                totParts = 0;
                totJob = 0;
                totlub = 0;
                totsubletInv = 0;
                totBoutInv = 0;


                foreach (GridViewRow dr in gvJobCardParts.Rows)
                {

                    totParts = totParts + SysFunctions.CustomCDBL(dr.Cells[10].Text);

                }


                foreach (GridViewRow dr in gvJobCard.Rows)
                {

                    totJob = totJob + SysFunctions.CustomCDBL(dr.Cells[9].Text);

                }



                foreach (GridViewRow dr in gvLubParts.Rows)
                {

                    totlub = totlub + SysFunctions.CustomCDBL(dr.Cells[10].Text);


                }


                foreach (GridViewRow dr in gvSublet.Rows)
                {

                    totsubletInv = totsubletInv + SysFunctions.CustomCDBL(dr.Cells[7].Text);

                }



                foreach (GridViewRow dr in gvJobCardPartsBought.Rows)
                {

                    totBoutInv = totBoutInv + SysFunctions.CustomCDBL(dr.Cells[9].Text);

                }


                totEst = totJob + totlub + totsubletInv + totBoutInv + totParts;
                txtJobCardTotal.Text = totEst.ToString();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }


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
                //ddlJobs_Labor.SelectedValue = row.Cells[2].Text.Trim();
                txtJobs_Labor.Text = row.Cells[2].Text.Trim();
                txtJobsDesc.Text = row.Cells[3].Text;
                string techCode = row.Cells[4].Text.Trim();
                //      ddlTechnicianEmpCode.SelectedValue = row.Cells[4].Text.Trim();
                //txtTechnician.Text = row.Cells[4].Text;
                //txtTechnicianDesc.Text = row.Cells[5].Text;
                //ddlJobCardType.Text = row.Cells[6].Text;
                txtJobRemarks.Text = (row.Cells[7].Text != "&nbsp;" ? row.Cells[7].Text : "");
                txtLabor.Text = row.Cells[9].Text;
                ddlTechnicianEmpCode.SelectedValue = techCode;
                if (row.Cells[6].Text != null || row.Cells[6].Text != "")
                {
                    if (ddlJobCardTypeCode.SelectedValue == "012")
                    {
                        ddlJobType.Visible = true;
                    }

                    ddlJobType.SelectedValue = row.Cells[6].Text;
                }

                //  ViewState["deductAmount"] = decimal.Parse(row.Cells[8].Text);
                JobDT = (DataTable)ViewState["Job"];
                JobDT.Rows.RemoveAt(gvJobCard.SelectedIndex);
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
            catch (Exception ex) { lblMsg.Text = ex.Message; }

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
                //ddlPartCode.SelectedValue = row.Cells[2].Text;
                txtItemCode.Text = row.Cells[2].Text;
                txtPartItemNo_Parts.Text = row.Cells[3].Text.Trim();
                txtItemDesc.Text = row.Cells[4].Text;
                txtDep.Text = row.Cells[5].Text;
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
                //ddlLubCode.SelectedValue = row.Cells[2].Text;
                txtLubCode.Text = row.Cells[2].Text;
                txtPartItemNo_Lubs.Text = row.Cells[3].Text.Trim();
                txtLubPartDesc.Text = row.Cells[4].Text;
                txtLubRecQuantity.Text = row.Cells[5].Text;
                txtLubQuantity.Text = row.Cells[6].Text;
                txtLubDep.Text = row.Cells[7].Text;
                txtLubPrice.Text = row.Cells[9].Text;
                lblLubType.Value = row.Cells[11].Text;

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

                //ddlJobs_Sublet.SelectedValue = row.Cells[4].Text.Trim();
                txtSubletJobCode.Text = row.Cells[4].Text.Trim();
                txtSubletJobDesc.Text = row.Cells[5].Text;
                txtSubletDate.Text = row.Cells[6].Text;
                // txtSubletDate.Text = row.Cells[7].Text;
                txtSubletIncAmnt.Text = row.Cells[7].Text;
                txtSubletPayAmnt.Text = row.Cells[8].Text;
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
                countPartsRecQty = countPartsRecQty + Convert.ToDecimal(e.Row.Cells[7].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[7].Text.Replace("&nbsp;", "").Trim());
                totParts = totParts + double.Parse(e.Row.Cells[10].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[10].Text.Replace("&nbsp;", "").Trim());
                countParts = countParts + Convert.ToDecimal(e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim());
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
                countlubRecQty = Convert.ToDecimal(e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim());
                countlub = countlub + Convert.ToDecimal(e.Row.Cells[6].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[6].Text.Replace("&nbsp;", "").Trim());
                totlub = totlub + double.Parse(e.Row.Cells[10].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[10].Text.Replace("&nbsp;", "").Trim());
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
                totsubletInv = totsubletInv + double.Parse(e.Row.Cells[7].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[7].Text.Replace("&nbsp;", "").Trim());
                totsubletPayable = totsubletPayable + double.Parse(e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim());
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                totsubletInv = totsubletPayable = 0;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                txtSubletTotal.Text = Convert.ToString(totsubletInv);
                txtSubletPayableTotal.Text = Convert.ToString(totsubletPayable);
            }

        }
        protected void gvJobCard_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //e.Row.Cells[2].Visible = false;
            // e.Row.Cells[4].Visible = e.Row.Cells[6].Visible = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                totLabour = totLabour + double.Parse(e.Row.Cells[9].Text.Replace("&nbsp;", "") == "" ? "0" : e.Row.Cells[9].Text);
                countLabour = countLabour + e.Row.Cells[2].Text.Replace("&nbsp;", "") == "" ? 0 : 1;
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
                if (txtGatePass.Text.Length != 0)
                {
                    grl.UserMsg(lblMsg, Color.Red, "JobCard can not delete it has Gatpass", txtCustomer);
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
                            grl.UserMsg(lblMsg, Color.Red, "Lubricant issued this JobCard not Delete", txtCustomer);
                            return;
                        }
                    }
                }
                if (checkPartissued())
                {
                    grl.UserMsg(lblMsg, Color.Red, "Parts issued this JobCard not Delete", txtCustomer);
                    return;
                }
                if (checkLubissued())
                {
                    grl.UserMsg(lblMsg, Color.Red, "Lubricant issued this JobCard not Delete", txtCustomer);
                    return;
                }
                string InvoiceNo = grl.GetStringValuesAgainstCodes("JobCardCode='" + txtJobCardCode.Text + "'", "CustomerInvoice", "InvoiceNo");
                string GatePass = grl.GetStringValuesAgainstCodes("JobCardCode='" + txtJobCardCode.Text + "'", "JobCardMaster", "GatePass");
                if (InvoiceNo != "")
                {
                    grl.UserMsg(lblMsg, Color.Red, "Invoice Exist Against Jobcard ");
                    return;
                }
                if (GatePass != "")
                {
                    grl.UserMsg(lblMsg, Color.Red, "GatePass Exist Against Jobcard ");
                    return;
                }
                SqlParameter[] JobCard_Master_Delete_param = {
                                                                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                                                              };
                JobCard_Master_Delete_param[0].Value = Session["DealerCode"].ToString();
                JobCard_Master_Delete_param[1].Value = txtJobCardCode.Text; // ddlJobCardCode.SelectedValue.ToString().Trim();
                if (myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_Master_Delete", JobCard_Master_Delete_param))
                {

                    grl.UserMsg(lblMsg, Color.Green, "Record Deleted Successfully: " + txtJobCardCode.Text, txtCustomer); //ddlJobCardCode.SelectedValue.ToString().Trim()
                    clearAll();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Deletealert()", true);
                }
                else
                {
                    grl.UserMsg(lblMsg, Color.Red, "Record not deleted: " + txtJobCardCode.Text, txtCustomer); //ddlJobCardCode.SelectedValue.ToString().Trim()
                }

            }

            catch (Exception ex)
            {
                grl.UserMsg(lblMsg, Color.Red, ex.Message, txtCustomer);
            }
        }

        private bool checkPartissued()
        {
            bool Isissue = false;
            double Recqty;

            foreach (GridViewRow gvr in gvJobCardParts.Rows)
            {
                Recqty = SysFunctions.CustomCDBL(grl.GetStringValuesAgainstCodes("JobCardCode='" + txtJobCardCode.Text + "' and ItemCode='" + gvr.Cells[2].Text + "'", "JobCardPartsDetail", "RecQty"));

                if (Recqty > 0)
                {
                    Isissue = true;
                    grl.UserMsg(lblMsg, Color.Red, "Parts issued this JobCard not Delete", txtCustomer);
                    return Isissue;
                }

            }

            return Isissue;
        }
        private bool checkLubissued()
        {
            bool Isissue = false;
            double Recqty;

            foreach (GridViewRow gvr in gvLubParts.Rows)
            {
                Recqty = SysFunctions.CustomCDBL(grl.GetStringValuesAgainstCodes("JobCardCode='" + txtJobCardCode.Text + "' and ItemCode='" + gvr.Cells[2].Text + "'", "JobCardLubricateDetail", "RecQty"));

                if (Recqty > 0)
                {
                    Isissue = true;
                    grl.UserMsg(lblMsg, Color.Red, "Lube issued this JobCard not Delete", txtCustomer);
                    return Isissue;
                }

            }

            return Isissue;
        }
        private void MakePDIReport()
        {
            ReportDocument rpt, crReportDocument, RD;
            string strCriteria, rptTitle;
            DateTime FromDate, ToDate;
            SqlDataReader rder;
            SysFunctions myFunc = new SysFunctions();
            DXBMS.Data.DataSet1 DSReports = new DXBMS.Data.DataSet1();
            Data.DSReports datas = new Data.DSReports();

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


            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_JobCardMaster_Print '" + this.Session["DealerCode"].ToString() + "','" + ViewState["JobCardCode"].ToString().Trim() + "'");

            // QRCodeEncoder encoder = new QRCodeEncoder();

            //  Bitmap bi = encoder.Encode(ds.Tables[0].Rows[0]["DealerCode"].ToString() + "" + ds.Tables[0].Rows[0]["JobCardCode"].ToString());

            //  bi.SetPixel(10, 10, Color.Red);

            //   bi.Save(Server.MapPath("~/Images/QrCode.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
            datas.EnforceConstraints = false;

            datas.sp_JobCardMaster_Print.Load(ds.CreateDataReader());

            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "Print_JobCardPDICheckSheet'" + this.Session["VehicleCategory"].ToString() + "'");
            datas.Print_JobCardPDICheckSheet.Load(ds.CreateDataReader());

            //ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_JobCardButPartsDetail_Print'" + this.Session["DealerCode"].ToString() + "','" + ViewState["JobCardCode"].ToString().Trim() + "'");
            //DSReports.sp_JobCardLubDetail_Print.Load(ds.CreateDataReader());

            //ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_JobCardPartsDetail_Print'" + this.Session["DealerCode"].ToString() + "','" + ViewState["JobCardCode"].ToString().Trim() + "'");
            //DSReports.sp_JobCardPartsDetail_Print.Load(ds.CreateDataReader());

            //ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_JobCardSubletDetail_Print'" + this.Session["DealerCode"].ToString() + "','" + ViewState["JobCardCode"].ToString().Trim() + "'");
            //DSReports.sp_JobCardSubletDetail_Print.Load(ds.CreateDataReader());

            string FileRptPath = Server.MapPath("~/Modules/Service/ServiceReports/");


            RD.Load(Server.MapPath("~/Modules/Service/ServiceReports/rptJobCardPDI.rpt"));






            RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Pre Delivery Inspection & Service Checklist'";
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
            RD.SetDataSource(datas);

            Session["RD"] = RD;

            // prepare pdf and show

            // create pdf 
            string FilePath = Server.MapPath("~/Download/");
            string FileName = "CounterSalesDetail" + Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;
            //crReportDocument.SetDatabaseLogon("SDMS", "sdms161", "192.168.1.47", "SDMS");
            //RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            string URL;
            URL = "../../../Download/rptViewerService.aspx?FileName=" + FileName;

            //URL = FilePath + "OpenPdf.aspx?FileName=" + FileName;
            //txtPartItemDesc.Text = URL;
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //URL = "../../../Download/OpenPdf.aspx?FileName=" + FileName; 
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

        }
        private void MakeStockReport()
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
            if (txtJobCardCode.Text.Trim() != "") //ddlJobCardCode.SelectedIndex
            {
                ViewState["JobCardCode"] = txtJobCardCode.Text.Trim(); // ddlJobCardCode.SelectedValue.ToString().Trim();
            }


            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_JobCardMaster_Print '" + this.Session["DealerCode"].ToString() + "','" + ViewState["JobCardCode"].ToString().Trim() + "'");

            //  QRCodeEncoder encoder = new QRCodeEncoder();

            //  Bitmap bi = encoder.Encode(ds.Tables[0].Rows[0]["DealerCode"].ToString() + "" + ds.Tables[0].Rows[0]["JobCardCode"].ToString());

            ///  bi.SetPixel(10, 10, Color.Red);

            //  bi.Save(Server.MapPath("~/Images/QrCode.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);


            DSReports.sp_JobCardMaster_Print.Load(ds.CreateDataReader());

            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_JobCardDetail_Print'" + this.Session["DealerCode"].ToString() + "','" + ViewState["JobCardCode"].ToString().Trim() + "'");
            DSReports.sp_JobCardDetail_Print.Load(ds.CreateDataReader());

            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_JobCardButPartsDetail_Print'" + this.Session["DealerCode"].ToString() + "','" + ViewState["JobCardCode"].ToString().Trim() + "'");
            DSReports.sp_JobCardLubDetail_Print.Load(ds.CreateDataReader());

            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_JobCardPartsDetail_Print'" + this.Session["DealerCode"].ToString() + "','" + ViewState["JobCardCode"].ToString().Trim() + "'");
            DSReports.sp_JobCardPartsDetail_Print.Load(ds.CreateDataReader());

            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_JobCardSubletDetail_Print'" + this.Session["DealerCode"].ToString() + "','" + ViewState["JobCardCode"].ToString().Trim() + "'");
            DSReports.sp_JobCardSubletDetail_Print.Load(ds.CreateDataReader());

            string FileRptPath = Server.MapPath("~/Modules/Service/ServiceReports/");


            RD.Load(Server.MapPath("~/Modules/Service/ServiceReports/rptJobCardStock.rpt"));

            RD.OpenSubreport(Server.MapPath("~/Modules/Service/ServiceReports/JobCardPartsDetail.rpt"));




            RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Part Requisition'";
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
            //crReportDocument.SetDatabaseLogon("SDMS", "sdms161", "192.168.1.47", "SDMS");
            //RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            string URL;
            URL = "../../../Download/rptViewerService.aspx?FileName=" + FileName;

            //URL = FilePath + "OpenPdf.aspx?FileName=" + FileName;
            //txtPartItemDesc.Text = URL;
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //URL = "../../../Download/OpenPdf.aspx?FileName=" + FileName; 
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

        }
        private void MakeReport()
        {
            ReportDocument rpt, crReportDocument, RD;
            PdfDocument outputDocument = new PdfDocument();
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string extension = "FileName.pdf";
            desktopPath += @"\" + extension;
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
            if (txtJobCardCode.Text.Trim() != "") //ddlJobCardCode.SelectedIndex
            {
                ViewState["JobCardCode"] = txtJobCardCode.Text.Trim(); // ddlJobCardCode.SelectedValue.ToString().Trim();
            }


            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_JobCardMaster_Print '" + this.Session["DealerCode"].ToString() + "','" + ViewState["JobCardCode"].ToString().Trim() + "'");

            // QRCodeEncoder encoder = new QRCodeEncoder();

            // Bitmap bi = encoder.Encode(ds.Tables[0].Rows[0]["DealerCode"].ToString() + "" + ds.Tables[0].Rows[0]["JobCardCode"].ToString());

            // bi.SetPixel(10, 10, Color.Red);

            //  bi.Save(Server.MapPath("~/Images/QrCode.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);


            DSReports.sp_JobCardMaster_Print.Load(ds.CreateDataReader());

            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_JobCardDetail_Print'" + this.Session["DealerCode"].ToString() + "','" + ViewState["JobCardCode"].ToString().Trim() + "'");
            DSReports.sp_JobCardDetail_Print.Load(ds.CreateDataReader());

            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_JobCardButPartsDetail_Print'" + this.Session["DealerCode"].ToString() + "','" + ViewState["JobCardCode"].ToString().Trim() + "'");
            DSReports.sp_JobCardLubDetail_Print.Load(ds.CreateDataReader());

            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_JobCardPartsDetail_Print'" + this.Session["DealerCode"].ToString() + "','" + ViewState["JobCardCode"].ToString().Trim() + "'");
            DSReports.sp_JobCardPartsDetail_Print.Load(ds.CreateDataReader());

            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_JobCardSubletDetail_Print'" + this.Session["DealerCode"].ToString() + "','" + ViewState["JobCardCode"].ToString().Trim() + "'");
            DSReports.sp_JobCardSubletDetail_Print.Load(ds.CreateDataReader());

            string FileRptPath = Server.MapPath("~/Modules/Service/ServiceReports/");

            if (ddlJobCardTypeCode.SelectedItem.Text.Trim() == "Insurance".ToUpper())
            {
                RD.Load(Server.MapPath("~/Modules/Service/ServiceReports/rptJobCardIns.rpt"));
            }
            else
            {
                RD.Load(Server.MapPath("~/Modules/Service/ServiceReports/rptJobCard.rpt"));
            }

            RD.OpenSubreport(Server.MapPath("~/Modules/Service/ServiceReports/rptJobCardDetail.rpt"));
            RD.OpenSubreport(Server.MapPath("~/Modules/Service/ServiceReports/JobCardPartsDetail.rpt"));
            RD.OpenSubreport(Server.MapPath("~/Modules/Service/ServiceReports/rptJobCardLubDetail.rpt.rpt"));
            RD.OpenSubreport(Server.MapPath("~/Modules/Service/ServiceReports/rptJobCardSubletDetail.rpt"));


            RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString().ToUpper() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString().ToUpper() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString().ToUpper() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString().ToUpper() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'JOB CARD'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString().ToUpper() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString().ToUpper() + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString().ToUpper() + "'";
            RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString().ToUpper() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString().ToUpper() + "'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";

            RD.DataDefinition.FormulaFields["QRCode"].Text = "'" + Server.MapPath("~") + "/Images/QrCode.jpg'";



            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "Report.pdf";
            string File = FilePath + FileName;
            RD.SetDataSource(DSReports.sp_JobCardMaster_Print.DataSet);



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
            stream = null;

            string URL = "../../../Download/PrintReport.aspx";

            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";

            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);


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
            //dsParam[1].Value = ddlJobCardCode.SelectedValue.ToString().Trim();
            dsParam[1].Value = txtJobCardCode.Text.Trim();
            dsJobCardParts = new DataSet();
            if (gvJobCardParts.Rows.Count > 0 || gvLubParts.Rows.Count > 0)
            {
                if (PartRequisition == "DMS")
                {
                    if (grl.CodeExists("JobCardPartsDetail", "JobCardCode", txtJobCardCode.Text, Session["DealerCode"].ToString(), " and qty <> recqty"))
                    {
                        //SendAlert(); return;
                        myFunc.UserMsg(lblMsg, Color.Red, "Please Isuue All Parts as per Job Card");

                        return;
                    }

                    if (grl.CodeExists("JobCardLubricateDetail", "JobCardCode", txtJobCardCode.Text, Session["DealerCode"].ToString(), " and qty <> recqty"))
                    {
                        //SendAlert(); return;
                        myFunc.UserMsg(lblMsg, Color.Red, "Please Isuue All Lubricant");
                        // SendAlert("Cannot Post due to lube parts entered!"); return;
                        return;
                    }

                }

            }
            else if (PartRequisition == "ERP")
            {
                

            }



        
          
          

                   
            
            

            string strSql = "Update dbo.JobCardMaster set GatePass='" + txtJobCardCode.Text.Trim() +"'," + //ddlJobCardCode.SelectedValue.ToString().Trim() 
                            "DelvDate='" + grl.SaveDate(DateTime.Now.ToString("dd/MM/yyyy")) + "'," +
                            "TransferStatus='E' " +
                            "Where DealerCode='" + Session["DealerCode"].ToString() + "' AND JobCardCode='" + txtJobCardCode.Text.Trim() +"'"; //ddlJobCardCode.SelectedValue.ToString().Trim() 
            try
            {
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    if (myFunc.ExecuteQuery(strSql, Trans))
                    {
                        ObjTrans.CommittTransaction(ref Trans);

                        grl.UserMsg(lblMsg, Color.Green, "JobCard Posted successfully " + txtJobCardCode.Text); //ddlJobCardCode.SelectedValue.ToString().Trim());
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
        private bool Create_NDM(string strJobCardCode, string Action, string oldgatepass)
        {
            SqlParameter[] MDNINV_param = { new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                                        new SqlParameter("@JobCardCode",SqlDbType.Char,8),//01
                                        new SqlParameter("@GatePassNo",SqlDbType.Char,8),//02
                                        new SqlParameter("@GatePassDate",SqlDbType.DateTime),//03
                                        new SqlParameter("@UpdUser",SqlDbType.VarChar,50),//04
                                        new SqlParameter("@Action",SqlDbType.VarChar,50)};//05
            MDNINV_param[0].Value = Session["DealerCode"].ToString();
            MDNINV_param[1].Value = strJobCardCode;
            MDNINV_param[2].Value = oldgatepass;
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
        private bool PostServiceFollowUp(string strJobCardCode, string Action,string oldgatepass)
        {
            SqlParameter[] MDNINV_param = { new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                                        new SqlParameter("@JobCardCode",SqlDbType.Char,8),//01
                                        new SqlParameter("@GatePassNo",SqlDbType.Char,8),//02
                                        new SqlParameter("@GatePassDate",SqlDbType.DateTime),//03
                                        new SqlParameter("@UpdUser",SqlDbType.VarChar,50),//04
                                        new SqlParameter("@Action",SqlDbType.VarChar,50),//05
                                        new SqlParameter("@TransType", SqlDbType.VarChar, 50) };//07
            MDNINV_param[0].Value = Session["DealerCode"].ToString();
            MDNINV_param[1].Value = strJobCardCode;
            MDNINV_param[2].Value = oldgatepass;
            MDNINV_param[3].Value = grl.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"));
            MDNINV_param[4].Value = Session["UserName"].ToString();
            MDNINV_param[5].Value = Action;
            MDNINV_param[6].Value = "JobCard";
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
                    ImageButton btn = sender as ImageButton;
                    TableCell tc = btn.Parent as TableCell;
                    GridViewRow gvr = tc.Parent as GridViewRow;
                double    Recqty = SysFunctions.CustomCDBL(grl.GetStringValuesAgainstCodes("JobCardCode='" + txtJobCardCode.Text + "' and ItemCode='" + gvr.Cells[2].Text + "'", "JobCardPartsDetail", "RecQty"));
                    if (gvr.Cells[7].Text == "0" && Recqty==0)
                    {
                        PartsDT.Rows.RemoveAt(gvr.RowIndex);
                    }
                    else
                    {
                        
                        grl.UserMsg(lblMsg, Color.Red, "Part can not remove because Receiving Quantity is not zero(0)...");
                        return;
                    }
                    //if (checkPartissued())
                    //{
                    //    grl.UserMsg(lblMsg, Color.Red, "Part can not remove because Receiving Quantity is not zero(0)...");
                    //    return;
                    //}
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
                CalculateTotal();
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
                ImageButton btn = sender as ImageButton;
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
                CalculateTotal();

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
                    ImageButton btn = sender as ImageButton;
                    TableCell tc = btn.Parent as TableCell;
                    GridViewRow gvr = tc.Parent as GridViewRow;
                    double Recqty = SysFunctions.CustomCDBL(grl.GetStringValuesAgainstCodes("JobCardCode='" + txtJobCardCode.Text + "' and ItemCode='" + gvr.Cells[2].Text + "'", "JobCardLubricateDetail", "RecQty"));
                    if (gvr.Cells[5].Text == "0" && Recqty==0)
                    {
                        LubDT.Rows.RemoveAt(gvr.RowIndex);
                       
                    }
                    else
                    {
                        
                        grl.UserMsg(lblMsg, Color.Red, "Can not Remove because Receiving Quantity not zero(0)...");
                        return;
                    }
                    //if (checkLubissued())
                    //{
                    //    grl.UserMsg(lblMsg, Color.Red, "Can not Remove because Receiving Quantity not zero(0)...");
                    //    return;
                    //}

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
                CalculateTotal();
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
                ImageButton btn = sender as ImageButton;
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
                CalculateTotal();
            }
            catch (Exception ex)
            {
                grl.UserMsg(lblMsg, Color.Red, ex.Message, txtCustomer);
            }
        }


        
        protected void btnDelete_Click1(object sender, EventArgs e)
        {

            if (txtJobCardCode.Text.Trim()  == "") //ddlJobCardCode.SelectedIndex
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
            if (grl.CodeExists("JobEstimateMaster", "JobCardCode", txtJobCardCode.Text.Trim() , " And DelFlag='N'")) //ddlJobCardCode.SelectedValue.ToString().Trim()
            {
                grl.UserMsg(lblMsg, Color.Red, "Jobcard exist in Job Estimate ,it can not be deleted ");
                
                return;
            }
            //PPR made or not
            ///discuss
            if (grl.CodeExists("PPRMaster", "JobCardCode", txtJobCardCode.Text.Trim(), " And DelFlag = 'N'")) //ddlJobCardCode.SelectedValue.ToString().Trim()
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
                    if (drPart["DealerCode"].ToString() != "")
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
                    if (drLub["DealerCode"].ToString() != "")
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
            /////////////////////////////////////////////ddlJobCardCode.SelectedValue.ToString().Trim()
            if (Delete(txtJobCardCode.Text.Trim(), lblSIRMaster.Text) == false)
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
            string sJobCardMasterUpdateQuery = "Update JobCardMaster Set DelFlag = 'Y',UpdUser = '" + Session["UserName"].ToString() + "',UpdDate = '" + grl.SaveDate(DateTime.Now.ToString("dd/MM/yyyy")) + "',UpdTime = '" + DateTime.Now.ToString("HH:mm:ss") + "', UpdTerm = '" + GlobalVar.mUserIPAddress + "' Where JobCardCode='" + strCode + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
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
          
            if (ddlJobCardTypeCode.SelectedValue == "012")
            {
                txtDep.ReadOnly = false;
                ddlJobType.Visible = true;
               
                ddlJobType.Visible = true;

            }

            if (ddlJobCardTypeCode.SelectedValue == "002" )
            {
                TabContainer1.Tabs[2].Visible = false;
                TabContainer1.Tabs[3].Visible = false;
                TabContainer1.Tabs[4].Visible = false;
                TabContainer1.Tabs[5].Visible = false;
                TabContainer1.Tabs[6].Visible = false;
                TabContainer1.Tabs[7].Visible = false;
            }
           else if (ddlJobCardTypeCode.SelectedValue == "001")
            {
               
                TabContainer1.Tabs[4].Visible = false;
                TabContainer1.Tabs[5].Visible = false;
                TabContainer1.Tabs[6].Visible = false;
                TabContainer1.Tabs[7].Visible = false;
            }
           
            else
            {
                TabContainer1.Tabs[3].Visible = true;
                TabContainer1.Tabs[2].Visible = true;
                TabContainer1.Tabs[3].Visible = true;
                TabContainer1.Tabs[4].Visible = true;
                TabContainer1.Tabs[5].Visible = true;
                TabContainer1.Tabs[6].Visible = true;
                TabContainer1.Tabs[7].Visible = true;
            }
            // if (ddlJobCardTypeCode.SelectedValue == "008")
            //{
            //   // txtLabor.Enabled = false;
            //    ddlPayMode.Items.Clear();
            //    ddlPayMode.Items.Add(new ListItem("Select", "Select"));
            //    ddlPayMode.Items.Add(new ListItem("WAP", "WAP"));
            //    ddlPayMode.Items.Add(new ListItem("NAP", "NAP"));
            //}
            //else
            //{
            //    ddlPayMode.Items.Clear();
            //    ddlPayMode.Items.Add(new ListItem("Select", "Select"));
            //    ddlPayMode.Items.Add(new ListItem("Cash", "Cash"));
            //    ddlPayMode.Items.Add(new ListItem("Credit", "Credit"));
            //    ddlPayMode.Items.Add(new ListItem("Intra", "Intra"));
            //    ddlPayMode.Items.Add(new ListItem("Internal", "Internal"));
            //    ddlPayMode.Items.Add(new ListItem("Free of Cost", "FOC"));
            //    ddlPayMode.Items.Add(new ListItem("OEM", "OEM"));
            //    ddlPayMode.Items.Add(new ListItem("Good Will Warranty", "Good Will Warranty"));

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
            rowConPartsDT["ItemCode"] = txtConPartCode.Text.Trim(); //ddlConsumableParts.SelectedValue.ToString().Trim();
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
                    ImageButton btn = sender as ImageButton;
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
            CalculateTotal();
        }

        

        protected void gvJobCardConParts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
             e.Row.Cells[9].Visible = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                countConPartsRecQty = countConPartsRecQty + int.Parse(e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim());
                totParts = totParts + double.Parse(e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim());
                countParts = countParts + Convert.ToDecimal(e.Row.Cells[6].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[6].Text.Replace("&nbsp;", "").Trim());
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
                txtConPartCode.Text = row.Cells[2].Text.Trim();
                //ddlConsumableParts.SelectedValue = row.Cells[2].Text;
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
            if (txtKM.BorderColor == Color.Red)
            {
                txtKM.BorderColor = Color.Black;
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
            DataTable dt;
            SqlParameter[] param = {                                 
                                    new SqlParameter("@DealerCode",SqlDbType.VarChar,5), 
                                    new SqlParameter("@RegNo",SqlDbType.VarChar,30)
                               };


            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = txtVehRegNo.Text.Trim(); // ddlVehRegNo.SelectedValue.ToString().Trim();//(HFRegNo.Value == "" ? null : HFRegNo.Value);

            dsVehInfo = myFunc.FillDataSet("sp_W2_CustomerVehicle_Select", param);
            if (dsVehInfo.Tables[0].Rows.Count > 0)
            {
                setVehcileInfo(dsVehInfo);
            }
            //dt = myFunc.GetData("Select C.CusDesc,C.CreditDays,C.CreditLimit ,isnull(round(isnull(CV.NetAmountCustomer,0)-isnull(CV.PaidC,0),0),0) as Outstanding from Customer C inner join CustomerInvoice CV on CV.DealerCode = C.DealerCode and CV.CusCode = C.CusCode where CV.DealerCode='" + Session["DealerCode"].ToString() + "' and CV.CusCode='" + txtCustomer.Text + "' "); //ddlEstNo.SelectedValue.ToString().Trim()
            //txtCreditDays.Text = dt.Rows[0]["CreditDays"].ToString();
            //txtCreditLimit.Text = dt.Rows[0]["CreditLimit"].ToString();
            //txtOutStanding.Text = dt.Rows[0]["Outstanding"].ToString();

            ShowHistory();

            

            //PrepareNewJobCard();

            //if (ddlRegNo.BackColor == Color.Red)
            //{
            //    ddlRegNo.BackColor = Color.White;
            //}
        }

        protected void imgJobLookUp_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlJobCardTypeCode.SelectedValue == "012")
            {
                grl.UserMsg(lblMsg, Color.Red, "You Can Not Add Jobs in Insurance Case");
                txtJobs_Labor.Focus();
                return;
            }

            ViewState["lookupid"] = 21; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_Job(imgJobLookUp, ViewState["lookupid"].ToString(), "", "../../../");

            ViewState["job"] = "Labor";

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void imgVehRegNo_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 3; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            string sql = "Select RegNo as'Reg No' , ChassisNo as 'Chassis No' , EngineNo as 'Engine No' , C.CusDesc as 'Customer' from CustomerVehicle A inner join Customer C on C.CusCode = A.CusCode and C.DealerCode = A.DealerCode";
            clslook.LU_Get_RecVeh(imgVehRegNo, ViewState["lookupid"].ToString(),"", sql, "../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void imgJobSubLookUp_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 21; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_Job(imgJobSubLookUp, ViewState["lookupid"].ToString(), "", "../../../");

            ViewState["job"] = "Sublet";

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void imgJobCardCode_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 4; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_JobCardNo(imgJobCardCode, ViewState["lookupid"].ToString(), "", "../../../");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void imgEstNo_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 73;
            clslook.LU_Get_CustomerEstimateNoForm_JC(imgJobCardCode, ViewState["lookupid"].ToString(), "", "../../../");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void imgLubricant_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlJobCardTypeCode.SelectedValue == "012")
            {
                grl.UserMsg(lblMsg, Color.Red, "You Can Not Lube  in Insurance Case");
                txtLubCode.Focus();
                return;
            }
            ViewState["lookupid"] = 43; ViewState["DropDown"] = "Lubricant";
            clslook.LU_Get_Lub(imgLubricant, ViewState["lookupid"].ToString(), "", "../../../");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void ImgConPart_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 43; ViewState["DropDown"] = "ConsumeAble";
            clslook.LU_Get_ConsumeableParts(imgLubricant, ViewState["lookupid"].ToString(), "", "../../../");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }
       
        protected void imgTaxDetail_Click(object sender, ImageClickEventArgs e)
        {

            ViewState["lookupid"] = 94; ViewState["DropDown"] = "TaxPolicy";
            clslook.LU_Get_Tax_Policy(imgTaxDetail, ViewState["lookupid"].ToString(), "", "../../../");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }
        private void ShowHistory()
        {
            DataTable dt = new DataTable();
            DataSet dsHistory = new DataSet();

            //string query = "exec sp_2W_ChassisNo_EngineNo_History '"+Session["DealerCode"].ToString() +"' , '" + ddlVehRegNo.SelectedValue + "'";
            string query = "exec sp_2W_ChassisNo_EngineNo_History '" + Session["DealerCode"].ToString() + "' , '" + txtVehRegNo.Text + "'";

            dt = myFunc.GetData(query);

            SqlParameter[] param1 = {
                                    new SqlParameter("@EngNo",SqlDbType.VarChar,30),
                                    new SqlParameter("@ChsNo",SqlDbType.VarChar,30),
                                    new SqlParameter("@RegNo",SqlDbType.VarChar,30)
                               };

            if (dt.Rows.Count == 0) return;
            param1[0].Value = dt.Rows[0]["EngineNo"].ToString().Trim(); // ddlRegNo.Text.Trim();//(HFRegNo.Value == "" ? null : HFRegNo.Value);
            param1[1].Value = dt.Rows[0]["ChassisNo"].ToString().Trim();
            param1[2].Value = txtVehRegNo.Text.Trim();
            //param1[2].Value = ddlVehRegNo.SelectedValue.ToString().Trim();

            dsHistory = myFunc.FillDataSet("sp_JobHistory", param1);
            if (dsHistory.Tables[0].Rows.Count > 0)
            {
                gv_history.DataSource = dsHistory.Tables[0];
                gv_history.DataBind();
            }
        }        

        protected void btnSaveJC_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2532", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (txtJobCardCode.Text != "")
            {
                if (!sec.UserRight("2532", "003"))
                {
                    Response.Redirect("~/Test.aspx");
                }
            }
           
            try
            {
                if (txtVehRegNo.Text.Trim() == "")
                {
                    grl.UserMsg(lblMsg, Color.Red, "Select Vehicle Registration Number");
                    txtVehRegNo.BorderColor = Color.Red;
                    txtVehRegNo.Focus();

                    return;
                }
                else
                {
                    lblMsg.Text = string.Empty;
                    txtVehRegNo.BorderColor = Color.White;
                    txtVehRegNo.Focus();
                  //  sysfun.ClearTextBoxesColor(Page);
                }
                if (ddlJobCardTypeCode.SelectedIndex == 0)
                {
                    myFunc.UserMsg(lblMsg, Color.Red, "Select job card type first");
                    ddlJobCardTypeCode.BorderColor = Color.Red;
                  //  ddlJobCardTypeCode.Focus();
                    
                    return;
                }
                else
                {
                    lblMsg.Text = string.Empty;
                    ddlJobCardTypeCode.BorderColor = Color.White;
                    ddlJobCardTypeCode.Focus();
                 //   sysfun.ClearTextBoxesColor(Page);
                }
                if (ddlPayMode.SelectedIndex == 0)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Select Payment Mode");
                    ddlPayMode.BorderColor = Color.Red;
                    ddlPayMode.Focus();
                    return;
                }
                else
                {
                    lblMsg.Text = string.Empty;
                    ddlPayMode.BorderColor = Color.White;
                    ddlPayMode.Focus();
                 //   sysfun.ClearTextBoxesColor(Page);

                }
                if (ddlTaxType.SelectedIndex == 0)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Please select the tax policy");
                    ddlTaxType.BorderColor = Color.Red;
                    ddlTaxType.Focus();
                    return;
                }
                else
                {
                    lblMsg.Text = string.Empty;
                    ddlTaxType.BorderColor = Color.White;
                    ddlTaxType.Focus();
                    txtTaxPolicyDesc.BorderColor = Color.White;
                    txtTaxPolicyDesc.Focus();

                   // sysfun.ClearTextBoxesColor(Page);

                }

                if (ddlAdvisorCode.SelectedIndex == 0)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Please select Advisor", txtCustomer);
                    ddlAdvisorCode.Focus();
                    ddlAdvisorCode.BorderColor = Color.Red;
                    return;
                }
                else
                {
                    lblMsg.Text = string.Empty;
                    ddlAdvisorCode.BorderColor = Color.White;
                    ddlAdvisorCode.Focus();
                }

                DataTable dt;
                if (txtJobCardCode.Text.Trim() != "" || ViewState["JobCardCode"].ToString() != "")
                {
                    string sql = "Select GatePass from JobCardMaster where DealerCode = '"+Session["DealerCode"].ToString()+"' AND JobCardCode = '" + ViewState["JobCardCode"].ToString() + "'";
                   
                    SysFunction sysfun = new SysFunction();

                    dt = sysfun.GetData(sql);

                    if (dt.Rows[0]["GatePass"].ToString().Trim() != "")
                    {
                        lblMsg.Text = "JobCard is already Closed";
                        return;
                    }
                }

                double limit=0,crdt=0;
                int JCday,Cdays;
                dt = sysfun.GetData("Get_CreditLimtCustomer '" + Session["DealerCode"].ToString() + "','" + txtCustomer.Text + "' ");
                if (dt.Rows.Count > 0)
                {
                    JCday = Convert.ToInt32(SysFunctions.CustomCDBL(dt.Rows[0]["Limit"].ToString()));
                    Cdays = Convert.ToInt16(SysFunctions.CustomCDBL(txtCreditDays.Text));
                    if (txtJobCardTotal.Text == "")
                    {
                        limit = 0;
                        limit = limit + Convert.ToDouble(txtOutStanding.Text);
                        crdt = Convert.ToDouble(txtCreditLimit.Text);
                        if(limit != 0)
                        {
                            if (limit > crdt)
                            {
                                lblMsg.Text = "JobCard amount is exceeding its Credit Limit";
                                lblMsg.ForeColor = Color.Red;
                                txtCreditLimit.Focus();
                             //   return;
                            }
                            if (JCday > Cdays)
                            {
                                lblMsg.Text = "JobCard amount is exceeding its  Credit Days";
                                lblMsg.ForeColor = Color.Red;
                                txtCreditDays.Focus();
                             //   return;
                            }
                        }
                       

                    }
                    else
                    {
                        if (Convert.ToDouble(txtOutStanding.Text) == 0)
                        {
                            limit =   Convert.ToDouble(txtJobCardTotal.Text);
                        }
                        else
                        {
                            limit = limit  + Convert.ToDouble(txtJobCardTotal.Text);
                        }
                       
                        crdt = Convert.ToDouble(txtCreditLimit.Text);

                        if (limit > crdt)
                        {
                            lblMsg.Text = "Customer Amount is Exceeding More than its Credit Limit";
                            lblMsg.ForeColor = Color.Red;
                            txtOutStanding.Focus();
                            // return;
                        }
                        
                    }

                }
                //if (txtCntPerCell.Text != "")
                //{
                //    if (!txtCntPerCell.Text.Contains("92"))
                //    {
                //        lblMsg.Visible = true;
                //        lblMsg.Text = "Enter Number Sarting With 92 ";
                //        lblMsg.ForeColor = Color.Red;
                //        txtCntPerCell.Focus();
                //        return;
                //    }

                //}
    TextBox[] textBoxes = {  txtCustomer, txtEndUser, txtBrand, txtProduct, txtVersion ,
                                    txtReciptTime,txtKM,txtRecieptDate,txtPromistedTime,txtCntPerName,txtCntPerCell};
                if (!MasterValidation(textBoxes)) return;
                else
                {
                   
                    JobCardEntry();

                    //Load_ddlJobCardCode();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);
                }
            }
            catch (Exception ex)
            {

                grl.UserMsg(lblMsg, Color.Red, ex.Message, txtCustomer);
            }
        }

        protected void btnDeleteJC_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2532", "004"))
            {
                Response.Redirect("~/Test.aspx");
            }
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
                    if (gvr.Cells[8].Text != "&nbsp;")
                    {
                        int a = Convert.ToInt32(gvr.Cells[8].Text);
                        if ( a > 0 || a!=0)
                        {
                            grl.UserMsg(lblMsg, Color.Red, "Parts issued this JobCard not Delete", txtCustomer);
                            return;
                        }
                    }
                }
                foreach (GridViewRow gvr in gvLubParts.Rows)
                {
                    if (gvr.Cells[6].Text != "&nbsp;")
                    {
                        if (Convert.ToInt32(gvr.Cells[4].Text) > 0)
                        {
                            grl.UserMsg(lblMsg, Color.Red, "Lubricat issued this JobCard not Delete", txtCustomer);
                            return;
                        }
                    }
                }
                if (grl.CodeExists("CustomerInvoice", "JobCardCode", txtJobCardCode.Text, Session["DealerCode"].ToString(), " And DelFlag='N'"))
                {
                    grl.UserMsg(lblMsg, Color.Red, "JobCard Code exist in Invoice ,it can not be deleted ");
                    return;
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
                //JobCard_Master_Delete_param[1].Value = ddlJobCardCode.SelectedValue.ToString().Trim();
                JobCard_Master_Delete_param[1].Value = txtJobCardCode.Text.Trim();
                if (myFunc.ExecuteSP_NonQuery("sp_W2_JobCard_Master_Delete", JobCard_Master_Delete_param))
                {
                    
                    grl.UserMsg(lblMsg, Color.Green, "Record Deleted Successfully: " + txtJobCardCode.Text.Trim(), txtCustomer);
                    clearAll();
                }
                else
                {
                    grl.UserMsg(lblMsg, Color.Red, "Record not deleted: " + txtJobCardCode.Text.Trim(), txtCustomer);
                }

            }

            catch (Exception ex)
            {
                grl.UserMsg(lblMsg, Color.Red, ex.Message, txtCustomer);
            }
        }

        protected void btnClearJC_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            clearAll();
            TabContainer1.Tabs[0].Visible = true;
            TabContainer1.Tabs[1].Visible = true;
            TabContainer1.Tabs[2].Visible = true;
            TabContainer1.Tabs[3].Visible = true;
            TabContainer1.Tabs[4].Visible = true;
            TabContainer1.Tabs[5].Visible = true;
            TabContainer1.Tabs[6].Visible = true;
            TabContainer1.Tabs[7].Visible = true;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);

            //Response.Redirect("~/Modules/Service/Forms/JobCardNew.aspx");
        }

        protected void imgAddJobTech_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                SqlParameter[] Delete_param = {
                                  new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                  new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                               };
                Delete_param[0].Value = Session["DealerCode"].ToString();
                Delete_param[1].Value = txtJobCardCode.Text;

                SqlParameter[] Insert_param = {
                                                                new SqlParameter("@DealerCode",SqlDbType.Char,5),    //0
                                                                new SqlParameter("@JobCardCode",SqlDbType.Char,8),   //1
                                                                new SqlParameter("@JobCode",SqlDbType.VarChar,8),    //2
                                                                new SqlParameter("@StdTime",SqlDbType.VarChar,8),    //3
                                                                new SqlParameter("@StartTime",SqlDbType.VarChar,8),  //4
                                                                new SqlParameter("@EndTime",SqlDbType.VarChar,8),    //5
                                                                new SqlParameter("@EmpCode",SqlDbType.Char,3),       //6
                                                                new SqlParameter("@BayCode",SqlDbType.VarChar,5),    //7
                                                                new SqlParameter("@UpdDateTime",SqlDbType.DateTime), //8
                                                                new SqlParameter("@Terminal",SqlDbType.VarChar,10),  //9
                                                                new SqlParameter("@UpdUser",SqlDbType.VarChar,50)    //10
                                                            };


                if (myFunc.ExecuteSP_NonQuery("sp_W2_JobCardTech_Delete", Delete_param))
                {

                    foreach (GridViewRow gvr in grdBay.Rows)
                    {
                        //CheckBox chkSelect = (CheckBox)gvr.FindControl("ChkSelect");
                        Label lblJobCode = (Label)gvr.FindControl("lblJobCode");
                        Label lblBayCode = (Label)gvr.FindControl("lblBayCode");
                        Label lblEndTime = (Label)gvr.FindControl("lblEndTime");
                        Label lblStartTime = (Label)gvr.FindControl("lblStartTime");
                        Label lblStdTime = (Label)gvr.FindControl("lblStdTime");
                        Label lblEmpCode = (Label)gvr.FindControl("lblEmpCode");

                        //if (chkSelect.Checked)
                        //{
                            if (txtJobCardCode.Text.Trim() != ""
                            & lblJobCode.Text.Trim() != ""
                            & lblEmpCode.Text.Trim() != ""
                            & lblEndTime.Text.Trim() != "")
                            {
                                Insert_param[0].Value = Session["DealerCode"].ToString();
                                Insert_param[1].Value = txtJobCardCode.Text.Trim() == "" ? HFJobCard.Value.Trim() : txtJobCardCode.Text; //ddlJobCardCode.SelectedValue.ToString().Trim()
                                Insert_param[2].Value = lblJobCode.Text.Trim();
                                Insert_param[3].Value = lblStdTime.Text.Trim();
                                Insert_param[4].Value = lblStartTime.Text.Trim();
                                Insert_param[5].Value = lblEndTime.Text.Trim();
                                Insert_param[6].Value = lblEmpCode.Text.Trim();
                                Insert_param[7].Value = lblBayCode.Text.Trim();
                                Insert_param[8].Value = myFunc.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"));
                                Insert_param[9].Value = GlobalVar.mUserIPAddress;
                                Insert_param[10].Value = Session["UserName"].ToString();
                                
                                if(myFunc.ExecuteSP_NonQuery("sp_W2_JobCardTech_Insert", Insert_param))
                                {
                                    grl.UserMsg(lblMsg, Color.Green, "JobTech saved Successfully.");
                                }
                            }

                        //}
                    }

                    BayDT = (DataTable)ViewState["Bay"];
                    DataRow[] drr = BayDT.Select();
                    for (int i = 0; i< drr.Length; i++)
                    {
                        if (BayDT.Rows[i]["JobCardCode"].ToString() != ""
                            & BayDT.Rows[i]["JobCode"].ToString() != ""
                            & BayDT.Rows[i]["EmpCode"].ToString() != ""
                            & BayDT.Rows[i]["EndTime"].ToString() != "")
                        {
                            Insert_param[0].Value = Session["DealerCode"].ToString();
                            Insert_param[1].Value = txtJobCardCode.Text.Trim() == "" ? HFJobCard.Value.Trim() : txtJobCardCode.Text; //ddlJobCardCode.SelectedValue.ToString().Trim()
                            Insert_param[2].Value = BayDT.Rows[i]["JobCode"].ToString();
                            Insert_param[3].Value = BayDT.Rows[i]["StdTime"].ToString();
                            Insert_param[4].Value = BayDT.Rows[i]["StartTime"].ToString();
                            Insert_param[5].Value = BayDT.Rows[i]["EndTime"].ToString();
                            Insert_param[6].Value = BayDT.Rows[i]["EmpCode"].ToString();
                            Insert_param[7].Value = BayDT.Rows[i]["BayCode"].ToString();
                            Insert_param[8].Value = myFunc.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"));
                            Insert_param[9].Value = GlobalVar.mUserIPAddress;
                            Insert_param[10].Value = Session["UserName"].ToString();
                            myFunc.ExecuteSP_NonQuery("sp_W2_JobCardTech_Insert", Insert_param);
                        }
                    }
                }

                
            }
            catch (Exception ex) { throw ex; }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {

            if (txtJobCardCode.Text.Trim() != "")
            {
                SqlParameter[] dsParam = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                                 };

                dsParam[0].Value = Session["DealerCode"].ToString();
                dsParam[1].Value = (ViewState["JobCardCode"].ToString().Trim() == "" ? null : ViewState["JobCardCode"].ToString().Trim());

                dsJobCardBay = new DataSet();
                dsJobCardBay = myFunc.FillDataSet("sp_W2_JobCard_JobCardTech_Select", dsParam);
                ViewState["Bay"] = dsJobCardBay.Tables[0]; grdBay.DataSource = dsJobCardBay; grdBay.DataBind();
                
            }else
            {
                createBayDT();
            }
        }

        protected void ImgBoughtOutPart_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 10; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3; ViewState["Bout"] = "Bout"; ViewState["Partss"] = "";
            clslook.LU_Get_Parts(imgLookup, ViewState["lookupid"].ToString(), "", "../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }

        protected void txtBQty_TextChanged(object sender, EventArgs e)
        {
            double amt,rate ;
           
            amt = double.Parse(txtBQty.Text) * double.Parse(txtBPrice.Text);
           rate = SysFunctions.CustomCDBL(grl.GetStringValuesAgainstCodes("CusTypeCode='" + txtCusTypeCode.Text + "'", "CustomerType", "BoutRate"));


            amt = (double.Parse(txtBPrice.Text) * rate) / 100;
            amt = amt+ double.Parse(txtBPrice.Text) ;
            txtBAmt.Text = amt.ToString();
           

        }
        private void LoadEmptyGrid()
        {

            ds = new DataSet();

           // ds.Tables.Add();
            ds.Tables.Add("PartsDetailsDS");

            ds.Tables["PartsDetailsDS"].Columns.Add(new DataColumn("PartNo", typeof(string)));
            ds.Tables["PartsDetailsDS"].Columns.Add(new DataColumn("ItemCode", typeof(string)));
            ds.Tables["PartsDetailsDS"].Columns.Add(new DataColumn("ItemDesc", typeof(string)));
            ds.Tables["PartsDetailsDS"].Columns.Add(new DataColumn("Qty", typeof(string)));
            ds.Tables["PartsDetailsDS"].Columns.Add(new DataColumn("Price", typeof(string)));
            ds.Tables["PartsDetailsDS"].Columns.Add(new DataColumn("InvRate", typeof(string)));
            ds.Tables["PartsDetailsDS"].Columns.Add(new DataColumn("PurchaseTotal", typeof(string)));
            ds.Tables["PartsDetailsDS"].Columns.Add(new DataColumn("InvoiceTotal", typeof(string)));

            //DataRow dr = ds.Tables[0].NewRow();
            //ds.Tables[0].Rows.Add(dr);

            gvJobCardPartsBought.DataSource = ds.Tables["PartsDetailsDS"]; ;
            gvJobCardPartsBought.DataBind();
            Session["BoutPartsDataTables"]= ds.Tables["PartsDetailsDS"]; ;
            Session["PartsDetailsDT"] = ds.Tables["PartsDetailsDS"];
            ViewState["PartsDetailsDS"]= ds;
            ViewState["BoutParts"]=ds.Tables["PartsDetailsDS"];
        }
        private void LoadGrid()
        {
            //dtParts.DefaultView.Sort = "S.No";
            gvJobCardPartsBought.DataSource = ds;
            gvJobCardPartsBought.DataBind();

            Session["BoutPartsDetailsDS"] = ds.Tables[0];
            ViewState["BoutPartsDataTable"] = ds;

        }

        protected void lknStock_Click(object sender, EventArgs e)
        {
            MakeStockReport();
        }

        protected void lknPDI_Click(object sender, EventArgs e)
        {
            MakePDIReport();
        }

        protected void BClearParts_Click(object sender, ImageClickEventArgs e)
        {
            ClearBoutPartsTextBoxes();
        }

        protected void lnkBoutRemove_Click(object sender, ImageClickEventArgs e)
        {

            BoutDT = (DataTable)ViewState["BoutPartsDataTables"];
            ImageButton btn = sender as ImageButton;
            TableCell tc = btn.Parent as TableCell;
            GridViewRow gvr = tc.Parent as GridViewRow;
            BoutDT.Rows.RemoveAt(gvr.RowIndex);
            
            //Load grid 
            gvJobCardPartsBought.DataSource = BoutDT;
            BoutDT.AcceptChanges();
            gvJobCardPartsBought.DataBind();
            if (BoutDT.Rows.Count > 0)
            {

            }
            else
            {
                lblBInvAmt.Text = "0";
                lblBParts.Text = "0";
            }
            ViewState["BoutPartsDataTables"] = BoutDT;
            CalculateTotal();

        }

        protected void ImgAddBought_Click(object sender, ImageClickEventArgs e)
        {
            string itemdesc;
            if (txtBQty.Text == "")
            {
                txtBQty.Text = "0";
            }
            if (Convert.ToInt32(txtBQty.Text) == 0)
            {

                grl.UserMsg(lblMsg, Color.Red, "Please enter Quantity", txtQuantity);
                return;
            }
            
            //if (Convert.ToInt32(txtQuantity.Text) < Convert.ToInt32(txtPartsRecQuantity.Text))
            //{

            //    grl.UserMsg(lblMsg, Color.Red, "Issue quantity not less then receive quantity", txtQuantity);
            //    return;
            //}
            else
            {
                lblMsg.Text = "";
            }
            //
            foreach (DataRow dr in PartsDT.Rows)
            {
                itemdesc = dr["ItemCode"].ToString();
                if (itemdesc == txtBItemCode.Text)
                {
                    lblMsg.Text = "Part Already exist in Material Tab";
                    lblMsg.ForeColor = Color.Red;
                    TabPanel2.Focus();
                    return;
                }

            }
           

                TextBox[] textBoxes = { txtBPrice, txtBQty };
            if (!MasterValidation(textBoxes)) return;
            else
            {
                //search_result = false;
                //foreach (DataRow rowPartsDT in PartsDT.Rows)
                //{
                //    if (rowPartsDT["ItemCode"].ToString().Trim() == ""
                //        | rowPartsDT["ItemCode"].ToString().Trim() == txtItemCode.Text)
                //    {
                //        Insert_in_PartsDTB(rowPartsDT);
                //        search_result = true;
                //    }
                //}
                //if (search_result == false)
                //{
                //    DataRow rowPartsDT = PartsDT.NewRow(); Insert_in_PartsDTB(rowPartsDT); PartsDT.Rows.Add(rowPartsDT);
                //}
                //ViewState["Parts"] = PartsDT; gvJobCardPartsBought.DataSource = PartsDT; gvJobCardPartsBought.DataBind();
                //int Count = 0;
                //foreach (DataRow dr in PartsDT.Rows)
                //{
                //    Count = Count + Convert.ToInt32(dr["Qty"]);
                //}
                //txtPartTotalQuantity.Text = Count.ToString();
                //ClearPartsTextBoxes();
                DataSet ds = new DataSet();
                ds = (DataSet)ViewState["PartsDetailsDS"];
                DataRow row = ds.Tables["PartsDetailsDS"].NewRow();

                row["ItemCode"] = txtBItemCode.Text.Trim();
                row["ItemDesc"] = txtBItemDesc.Text.Trim();
                row["Price"] = txtBPrice.Text.Trim();
                row["PartNo"] = txtBPartNo.Text.Trim();
                row["Qty"] = txtBQty.Text.Trim();
                row["InvRate"] = txtBAmt.Text.Trim();
                row["PurchaseTotal"] = double.Parse(txtBPrice.Text) * double.Parse(txtBQty.Text);
                row["InvoiceTotal"] = double.Parse(txtBAmt.Text) * double.Parse(txtBQty.Text);

                ds.Tables["PartsDetailsDS"].Rows.Add(row);
                BoutDT = ds.Tables["PartsDetailsDS"];
                //ViewState["ObjPOds"] = ds;
                decimal Count = 0;
                decimal InvAmt = 0;
                foreach (DataRow dr in BoutDT.Rows)
                {
                    Count = Count + Convert.ToDecimal(dr["Qty"]);
                    InvAmt = InvAmt + Convert.ToDecimal(dr["InvoiceTotal"].ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : dr["InvoiceTotal"].ToString().Replace("&nbsp;", "").Trim());
                }

                gvJobCardPartsBought.DataSource = ds.Tables["PartsDetailsDS"];
                gvJobCardPartsBought.DataBind();
                ViewState["BoutPartsDataTable"] = ds;
                //dtJobs = ds.Tables["WarrantyLaborTable"];
                Session["BoutPartsDataTables"] = ds.Tables["PartsDetailsDS"];
                ViewState["BoutPartsDataTables"] = ds.Tables["PartsDetailsDS"];
                ViewState["BoutDT"]= ds.Tables["PartsDetailsDS"]; 
                ClearBoutPartsTextBoxes();
                CalculateTotal();

            }
        }

    
        protected void btnUpdJobs_Click(object sender, EventArgs e)
        {
            DataTable dt;
            createJobDT();
            dt = myFunc.GetData("select JobCode,JobRemarks,Labor,Type from CustomerEstimateDetail where DealerCode='" + Session["DealerCode"].ToString() + "' And  CustomerEstimateCode='" + txtExtno.Text + "'");
            foreach (DataRow dr in dt.Rows)
            {
                txtJobs_Labor.Text = dr["JobCode"].ToString().Trim();
                txtJobRemarks.Text = dr["JobRemarks"].ToString().Trim();
                txtLabor.Text = dr["Labor"].ToString().Trim();
                ddlTechnicianEmpCode.SelectedIndex = 1;
                ddlBay.SelectedIndex = 1;
                if (dr["Type"].ToString() != null)
                {
                    ddlJobType.SelectedValue = dr["Type"].ToString().Trim();
                }
                else
                {
                    ddlJobType.SelectedIndex =0;
                }
                byPass = 1;
                BtnAddJobs_Click(BtnAddJobs, null);
            }

        }

        protected void btnUpdParts_Click1(object sender, EventArgs e)
        {
           
            DataTable dt;
            createPartsDT();
            dt = myFunc.GetData("select C.ItemCode,C.Qty,C.Dep,I.ItemDesc,C.Type from CustomerEstimateParts C Inner Join Item I on I.ItemCode=C.ItemCode and I.DealerCode in ('COMON',C.DealerCode)  where C.DealerCode='" + Session["DealerCode"].ToString() + "' And  C.CustomerEstimateCode='" + txtExtno.Text + "'");
            foreach (DataRow dr in dt.Rows)
            {
                txtQuantity.Text = dr["Qty"].ToString().Trim();
                txtDep.Text = dr["Dep"].ToString().Trim();
                //  SelectedPartDetail(dr["ItemCode"].ToString().Trim());
                txtItemCode.Text = dr["ItemCode"].ToString().Trim();
                txtPartsRecQuantity.Text = SysFunctions.CustomCDBL(grl.GetStringValuesAgainstCodes("ItemCode='"+ dr["ItemCode"].ToString().Trim() + "' and JobCardCode='" + txtJobCardCode.Text + "'", "JobCardPartsDetail", "RecQTY")).ToString();
                lblPartType.Value= dr["Type"].ToString().Trim();
                ddlPartCode_SelectedIndexChanged(txtItemCode, EventArgs.Empty);
                byPass = 1;
                btnAddParts_Click(btnAddParts, null);
            }
            CalculateTotal();



        }

        protected void btnUpdLub_Click(object sender, EventArgs e)
        {
            DataTable dt;
            createLubDT();
            dt = myFunc.GetData("select ItemCode,Qty,Type,Dep from CustomerEstimateLubricates where DealerCode='" + Session["DealerCode"].ToString() + "' And  CustomerEstimateCode='" + txtExtno.Text + "'");
            foreach (DataRow dr in dt.Rows)
            {
                txtLubCode.Text = dr["ItemCode"].ToString().Trim(); //ddlLubCode.SelectedValue
                txtLubQuantity.Text = dr["Qty"].ToString().Trim();
                lblLubType.Value = dr["Type"].ToString().Trim();
                txtLubDep.Text = dr["Dep"].ToString().Trim();
                txtLubRecQuantity.Text = SysFunctions.CustomCDBL(grl.GetStringValuesAgainstCodes("ItemCode='" + dr["ItemCode"].ToString().Trim() + "' and JobCardCode='" + txtJobCardCode.Text + "'", "JobCardLubricateDetail", "RecQTY")).ToString();
                ddlLubCode_SelectedIndexChanged(txtLubCode, EventArgs.Empty);
                byPass = 1;
                btnAddLubs_Click(btnAddLubs, null);
            }
            CalculateTotal();

        }

        protected void btnUpdSublet_Click(object sender, EventArgs e)
        {
            SqlParameter[] dsParam = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                    new SqlParameter("@CustomerEstimateCode",SqlDbType.Char,8)
                                 };
            dsParam[0].Value = Session["DealerCode"].ToString();
            dsParam[1].Value = txtExtno.Text;
            dsJobCardSublet = new DataSet();
            dsJobCardSublet = grl.FillDataSet("sp_W2_Estimate_Sublet_Select", dsParam);
            //if (dsJobCardSublet.Tables[0].Rows.Count == 0) dsJobCardSublet.Tables[0].Rows.Add(dsJobCardSublet.Tables[0].NewRow());
            ViewState["SubLet"] = dsJobCardSublet.Tables[0]; gvSublet.DataSource = dsJobCardSublet; gvSublet.DataBind();

            CalculateTotal(); 


        }

        protected void btnUpdBout_Click(object sender, EventArgs e)
        {
            SqlParameter[] dsParam = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                    new SqlParameter("@CustomerEstimateCode",SqlDbType.Char,8)
                                 };
            dsParam[0].Value = Session["DealerCode"].ToString();
            dsParam[1].Value = txtExtno.Text;
            dsJobCardBoutParts = new DataSet();
            dsJobCardBoutParts = grl.FillDataSet("sp_W2_Estimate_BoutPartsDetail_Select", dsParam);
            //if (dsJobCardConParts.Tables[0].Rows.Count == 0) dsJobCardConParts.Tables[0].Rows.Add(dsJobCardConParts.Tables[0].NewRow());
            ViewState["BoutParts"] = dsJobCardBoutParts.Tables[0]; gvJobCardPartsBought.DataSource = dsJobCardBoutParts; gvJobCardPartsBought.DataBind();
            Session["BoutPartsDataTables"] = dsJobCardBoutParts.Tables[0];
            CalculateTotal();
        }

        protected void ddlTaxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] splt = ddlTaxType.SelectedItem.Text.Split('|');
            if (splt.Length > 0)
            {
                txtTaxPolicyDesc.Text = grl.GetStringValuesAgainstCodes("TaxAppCode='" + ddlTaxType.SelectedValue + "'", "TaxSetUpMaster", "TaxAppDesc");
                hdnTaxCode.Value = ddlTaxType.SelectedValue;
                hdnTaxType.Value = splt[0];

            }

        }

        protected void txtSubletPayAmnt_TextChanged(object sender, EventArgs e)
        {
            double amt, rate;

            amt = SysFunctions.CustomCDBL(txtSubletPayAmnt.Text);
            rate = SysFunctions.CustomCDBL(grl.GetStringValuesAgainstCodes("CusTypeCode='" + txtCusTypeCode.Text + "'", "CustomerType", "SubletRate"));


            amt = (SysFunctions.CustomCDBL(txtSubletPayAmnt.Text) * rate) / 100;
            amt = amt + SysFunctions.CustomCDBL(txtSubletPayAmnt.Text);
            txtSubletIncAmnt.Text = amt.ToString();
        }

        private void Insert_in_PartsDTB(DataRow rowPartsDT)
        {
            //DataTable dt = myFunc.GetData("select ItemDesc from Item where DealerCode='" + Session["DealerCode"].ToString() + "' And  ItemCode='" + ddlPartCode.SelectedValue.ToString() + "'");
            rowPartsDT["PartNo"] = txtBPartNo.Text.Trim();
            rowPartsDT["PartsDesc"] = txtBItemDesc.Text.Trim();
            if (txtDep.Text != "")
            {
                ddlJobCardTypeCode.SelectedValue = "012";
            }

            if (ddlJobCardTypeCode.SelectedItem.Text.Trim() == "INSURANCE")
            {
                //if (txtDep.Text.Trim() == string.Empty)
                //{
                //    txtDep.Text = "0";
                //}
                //rowPartsDT["Dep"] = txtDep.Text.Trim();
                //rowPartsDT["DepAmount"] = (Math.Round((double.Parse(txtQuantity.Text.Trim()) * double.Parse(txtPartPrice.Text.Trim()) / 100) * double.Parse(txtDep.Text.Trim())));
            }
            else
            {
                //rowPartsDT["Dep"] = "0";
                //rowPartsDT["DepAmount"] = "0";
            }

            rowPartsDT["Qty"] = txtQuantity.Text.Trim();
            rowPartsDT["Price"] = txtBPrice.Text.Trim();
            rowPartsDT["Total"] = double.Parse(txtBQty.Text.Trim()) * double.Parse(txtBPrice.Text.Trim())+double.Parse((txtBPrice.Text.Trim()))*(25/100);
            rowPartsDT["ItemCode"] = txtBItemCode.Text;
          
        }
        protected void gvJobCardPartsBought_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                totBoutInv = totBoutInv + double.Parse(e.Row.Cells[9].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[9].Text.Replace("&nbsp;", "").Trim());
                totPayableBout = totPayableBout + double.Parse(e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim());
                //if (txtJobCardTotal.Text == null || txtJobCardTotal.Text == "")
                //{
                //    txtJobCardTotal.Text = "0";


                //}
                //totEst = double.Parse(e.Row.Cells[9].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[9].Text.Replace("&nbsp;", "").Trim()) + double.Parse(txtJobCardTotal.Text);
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                totPayableBout = totBoutInv = 0;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                lblBParts.Text = Convert.ToString(totPayableBout);
                lblBInvAmt.Text = Convert.ToString(totBoutInv);
               // txtJobCardTotal.Text = Convert.ToString(totEst);
            }


        }

        protected void gvJobCardPartsBought_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ViewState["ridx"] = gvJobCardPartsBought.SelectedRow.RowIndex;
                GridViewRow row = gvJobCardPartsBought.Rows[gvJobCardPartsBought.SelectedRow.RowIndex];
                //txtConPartCode.Value = row.Cells[2].Text.Trim();
                txtBItemCode.Text = row.Cells[2].Text.Trim();
                //ddlConsumableParts.SelectedValue = row.Cells[2].Text;
                txtBPartNo.Text = row.Cells[3].Text.Trim();
                txtBItemDesc.Text = row.Cells[4].Text;
                txtBQty.Text = row.Cells[5].Text;
                txtBPrice.Text = row.Cells[6].Text;
                txtBAmt.Text = row.Cells[8].Text;
              //  ViewState["deductAmount"] = row.Cells[7].Text;

            }
            catch (Exception ex) { throw ex; }
        }

        protected void btnPostJC_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2532", "005"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (txtJobCardCode.Text.Trim() == "")
            {
                grl.UserMsg(lblMsg, Color.Red, "jobCardNo should not be left blank", txtCustomer);
                return;
            }
            if (ddlJobCardTypeCode.SelectedValue.ToString() != "001" )
            {
                if (ddlJobCardTypeCode.SelectedValue.ToString() != "002")
                {
                    if (ddlJobCardTypeCode.SelectedValue.ToString() != "007")
                    {
                       
                            if (ddlJobCardTypeCode.SelectedValue.ToString() != "013")
                            {
                                if (ddlJobCardTypeCode.SelectedValue.ToString() != "014")
                                {
                                    grl.UserMsg(lblMsg, Color.Red, "Posting not allow, please make customer invoice against this jobcard No", txtCustomer);
                                    return;
                                }
                            }
                        
                    }
                }
                
               
            }

            ds = new DataSet();
            grl.CodeExists("JobCardMaster", "JobCardCode='" + txtJobCardCode.Text.Trim() + "' AND DealerCode='" + Session["DealerCode"].ToString() + "'", ref ds);
            search_result = false;

            if (ddlJobCardTypeCode.SelectedValue == "008")
            {
               
                callPostingJobCard(); search_result = true;


            }
           
          
            /////////////////
            if (ds.Tables[0].Rows[0]["JobCardType"].ToString().Trim() == "007")
            {
                callPostingJobCard(); search_result = true;
            }
            else if (ds.Tables[0].Rows[0]["JobCardType"].ToString().Trim() == "001" || ds.Tables[0].Rows[0]["JobCardType"].ToString().Trim() == "013"|| ds.Tables[0].Rows[0]["JobCardType"].ToString().Trim() == "014")
            {
                callPostingJobCard(); search_result = true;
               
            }
            else if (ds.Tables[0].Rows[0]["JobCardType"].ToString().Trim() == "002")
            {
                callPostingJobCard(); search_result = true;
            }
            btnGatePass_Click(null,null);
           
            if (search_result == false) { SendAlert("Cannot post due to Job Type / Job Card Type!"); return; }
            clearAll();
        }

        protected void btnPrintJC_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2532", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }
            MakeReport();

            //string URL = "../../../Download/OpenPdf.aspx?FileName=" + PDFFileName;
            ////URL = FilePath + "OpenPdf.aspx?FileName=" + FileName;
            ////txtPartItemDesc.Text = URL;
            //string fullsysURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullsysURL, true);
        }

        protected void ddlJobs_SelectedIndexChanged(string item)
        {
            DataTable dt = new DataTable();

            //dt = myFunc.GetData("select Amount from LaborDetail where DealerCode='" + Session["DealerCode"].ToString() + "' And ProdCode='" + txtProduct.Text + "' And DefJobCode ='"+ddlJobs_Labor.SelectedValue+"' ");
            if (ddlJobCardTypeCode.SelectedValue == "002") {
                if (sysfun.IsExist("JobCode", item, "FreeJobs", "COMON", "and VehicleCategory='" + Session["VehicleCategory"].ToString() + "'"))
                {
                    dt = myFunc.GetData("select PDI as Amount from Vehicle where DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') And ProdCode='" + txtProduct.Text + "'  ");
                }
               
            }
            else if (ddlJobCardTypeCode.SelectedValue == "001")
            {
                if (sysfun.IsExist("JobCode", item, "FreeJobs", "COMON", "and VehicleCategory='" + Session["VehicleCategory"].ToString() + "'"))
                {
                    dt = myFunc.GetData("select FFS as Amount from Vehicle where DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') And ProdCode='" + txtProduct.Text + "'  ");
                }
               
            }
            else if (ddlJobCardTypeCode.SelectedValue == "014")
            {
                if (sysfun.IsExist("JobCode", item, "FreeJobs", "COMON", "and VehicleCategory='" + Session["VehicleCategory"].ToString() + "'"))
                {
                    dt = myFunc.GetData("select SFS as Amount from Vehicle where DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') And ProdCode='" + txtProduct.Text + "'  ");
                }
                
            }
            else if (ddlJobCardTypeCode.SelectedValue == "013")
            {
                if (sysfun.IsExist("JobCode", item, "FreeJobs", "COMON", "and VehicleCategory='" + Session["VehicleCategory"].ToString() + "'"))
                {
                    dt = myFunc.GetData("select TFS as Amount from Vehicle where DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') And ProdCode='" + txtProduct.Text + "'  ");
                }

                
            }
            else if (ddlJobCardTypeCode.SelectedValue == "013")
            {
                if (sysfun.IsExist("JobCode", item, "FreeJobs", "COMON", "and VehicleCategory='" + Session["VehicleCategory"].ToString() + "'"))
                {
                    dt = myFunc.GetData("select TFS as Amount from Vehicle where DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') And ProdCode='" + txtProduct.Text + "'  ");
                }
               
            }
            else if (ddlJobCardTypeCode.SelectedValue == "018")
            {
                if (sysfun.IsExist("JobCode", item, "FreeJobs", "COMON", "and VehicleCategory='" + Session["VehicleCategory"].ToString() + "'"))
                {
                    dt = myFunc.GetData("select F4S as Amount from Vehicle where DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') And ProdCode='" + txtProduct.Text + "'  ");
                }
                
            }
            else if (ddlJobCardTypeCode.SelectedValue == "019")
            {
                if (sysfun.IsExist("JobCode", item, "FreeJobs", "COMON", "and VehicleCategory='" + Session["VehicleCategory"].ToString() + "'"))
                {
                    dt = myFunc.GetData("select F5S as Amount from Vehicle where DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') And ProdCode='" + txtProduct.Text + "'  ");
                }
               
            }
            else
            {
                dt = myFunc.GetData("select Amount from LaborDetail where DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') And ProdCode='" + txtProduct.Text + "' And DefJobCode ='" + item + "' ");
            }
           
            txtLabor.Focus();
            if (dt.Rows.Count == 0)
            {
                return;
            }
            if(dt.Rows[0]["Amount"].ToString() != "")
                {
                    txtLabor.Text = dt.Rows[0]["Amount"].ToString();
                }
            txtJobs_Labor.Text = item;
        }

        protected void BtnAddJobs_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlJobCardTypeCode.SelectedValue == "007")
            {
                grl.UserMsg(lblMsg, Color.Red, "You Can Not Add Jobs in ComeBack Case");
                txtJobs_Labor.Focus();
                return;
            }
              
            
           
            if (txtJobs_Labor.Text == "")
            {
                txtJobs_Labor.BorderColor = Color.Red;
                grl.UserMsg(lblMsg, Color.Red, "Please select Labor");
                txtJobs_Labor.Focus();
                return;
            }
            else
            {
                lblMsg.Text = string.Empty;
            }
            //if (ddlJobCardTypeCode.SelectedValue=="008")
            //{
            //    txtJobs_Labor.BorderColor = Color.Red;
            //    grl.UserMsg(lblMsg, Color.Red, "Warranty Labor will be added at Warranty Claim");
            //    txtJobs_Labor.Focus();
            //    return;
            //}
            //else
            //{
            //    lblMsg.Text = string.Empty;
            //}

            if (ddlBay.SelectedIndex == 0)
            {
                grl.UserMsg(lblMsg, Color.Red, "Please select Bay");
                return;
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
                        | rowJobDT["JobCode"].ToString().Trim() == txtJobs_Labor.Text.Trim())
                    {
                        Insert_in_JobDT(rowJobDT); search_result = true;
                    }
                }
                if (search_result == false)
                {

                    DataRow rowJobDT = JobDT.NewRow(); Insert_in_JobDT(rowJobDT); JobDT.Rows.Add(rowJobDT);
                }
                ViewState["Job"] = JobDT; gvJobCard.DataSource = JobDT.DefaultView; gvJobCard.DataBind();                
                txtJobsTotal.Text = JobDT.Rows.Count.ToString();
                CalculateTotal();

                ClearJobTextBoxes();
            }
        }

        protected void BtnClearJobs_Click(object sender, ImageClickEventArgs e)
        {
            ClearJobTextBoxes();
        }

        protected void btnAddParts_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlJobCardTypeCode.SelectedValue == "007")
            {
                grl.UserMsg(lblMsg, Color.Red, "You Can Not Parts  in ComeBack Case");
                txtItemCode.Focus();
                return;
            }
           
                
            
           

            if (txtQuantity.Text == "")
            {
                txtQuantity.Text = "0";
            }
            if (txtQuantity.Text == "0")
            {
                
                grl.UserMsg(lblMsg, Color.Red, "Please enter Quantity", txtQuantity);
                return;
            }
            if (SysFunctions.CustomCDBL(txtQuantity.Text) < SysFunctions.CustomCDBL(txtPartsRecQuantity.Text))
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
                foreach (DataRow dr in BoutDT.Rows)
                {
                 string   itemdesc = dr["ItemCode"].ToString();
                    if (itemdesc == txtItemCode.Text)
                    {
                        lblMsg.Text = "Part Already exist in BoughtOut Tab";
                        lblMsg.ForeColor = Color.Red;
                        TabPanel2.Focus();
                        return;
                    }

                }
                search_result = false;
                foreach (DataRow rowPartsDT in PartsDT.Rows)
                {
                    if (rowPartsDT["ItemCode"].ToString().Trim() == ""
                        | rowPartsDT["ItemCode"].ToString().Trim() == txtItemCode.Text)
                    {
                        Insert_in_PartsDT(rowPartsDT);
                        search_result = true;
                    }
                }
                if (search_result == false)
                {
                    DataRow rowPartsDT = PartsDT.NewRow(); Insert_in_PartsDT(rowPartsDT); PartsDT.Rows.Add(rowPartsDT);
                }
                ViewState["Parts"] = PartsDT; gvJobCardParts.DataSource = PartsDT; gvJobCardParts.DataBind();
                decimal Count = 0;
                foreach (DataRow dr in PartsDT.Rows)
                {
                    Count = Count + Convert.ToDecimal(dr["Qty"]);
                }
                txtPartTotalQuantity.Text = Count.ToString();
                ClearPartsTextBoxes();
                CalculateTotal();

            }
        }

        protected void btnClearParts_Click(object sender, ImageClickEventArgs e)
        {
            ClearPartsTextBoxes();
        }

        protected void SelectedPartDetail(string item)
        {

            DataTable dt = new DataTable();
            try
            {
                if (ViewState["lookupid"].ToString() == "10" && ViewState["Partss"].ToString() == "Parts")
                {
                    if (ddlJobCardTypeCode.SelectedValue == "012")
                    {
                        txtDep.ReadOnly = false;
                    }
                    else
                    {
                        txtDep.ReadOnly = true;

                    }

                    dt = myFunc.GetData("select ItemDesc,SaleRate,PurchaseRate,PartItemNo from Item where DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') And  ItemCode='" + item + "'");
                    txtItemCode.Text = item;
                    txtItemDesc.Text = dt.Rows[0]["ItemDesc"].ToString().Trim();
                    if (ddlJobCardTypeCode.SelectedValue == "008")
                    {
                        txtPartPrice.Text = dt.Rows[0]["PurchaseRate"].ToString().Trim();
                    }
                    else
                    {
                        txtPartPrice.Text = dt.Rows[0]["SaleRate"].ToString().Trim();
                    }
                    txtPartItemNo_Parts.Text = dt.Rows[0]["PartItemNo"].ToString().Trim();
                    // txtPartPrice.Text = dt.Rows[0]["SaleRate"].ToString().Trim();
                   SetFocus(txtQuantity);
                }
                else if (ViewState["lookupid"].ToString() == "10" && ViewState["Bout"].ToString()== "Bout")
                {
                    decimal purchaserate = 0, tax;
                    dt = myFunc.GetData("select ItemDesc,SaleRate,PartItemNo from Item where DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') And  ItemCode='" + item + "'");
                    txtBItemCode.Text = item;
                    txtBItemDesc.Text = dt.Rows[0]["ItemDesc"].ToString().Trim();
                    txtBPartNo.Text = dt.Rows[0]["PartItemNo"].ToString().Trim();
                    txtBPrice.Text = dt.Rows[0]["SaleRate"].ToString().Trim();

                    SetFocus(txtBQty);
                }
                else if (ViewState["lookupid"].ToString() == "3")
                {
                    txtVehRegNo.Text = item;
                    ddlVehRegNo_SelectedIndexChanged(txtVehRegNo, EventArgs.Empty);
                    SetFocus(ddlJobCardTypeCode);
                   

                }
                else if (ViewState["lookupid"].ToString() == "4")
                {
                    txtJobCardCode.Text = item;
                    ddlJobCardCode_SelectedIndexChanged(txtJobCardCode, EventArgs.Empty);

                }
                else if (ViewState["lookupid"].ToString() == "73")
                {
                    txtExtno.Text = item;
                    ddlEstNo_SelectedIndexChanged(txtExtno, EventArgs.Empty);
                }
                else if (ViewState["lookupid"].ToString() == "43")
                {
                    if (ViewState["DropDown"].ToString() == "Lubricant")
                    {
                        txtLubCode.Text = item;
                        ddlLubCode_SelectedIndexChanged(txtLubCode, EventArgs.Empty);
                        SetFocus(txtLubQuantity);
                    }
                    else
                    {
                        txtConPartCode.Text = item;
                        ddlConsumableParts_SelectedIndexChanged(txtConPartCode, EventArgs.Empty);
                        SetFocus(txtConPartsRecQuantity);
                    }

                }
                else if (ViewState["lookupid"].ToString() == "94")
                {
                       txtTaxPolicyCode.Text = item;
                   
                    string[] splt = item.Split('|');
                    if (splt.Length > 0)
                    {
                        txtTaxPolicyDesc.Text = grl.GetStringValuesAgainstCodes("TaxAppCode='" + splt[0] + "'", "TaxSetUpMaster", "TaxAppDesc");
                        hdnTaxCode.Value = splt[0];
                        hdnTaxType.Value = splt[1];

                    }
                   




                }
                else
                {
                    if(ViewState["job"].ToString() == "Labor")
                    {
                        ddlJobs_SelectedIndexChanged(item);
                        dt = myFunc.GetData("select DefJobDesc from DefaultJob where DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') And  DefJobCode ='" + item + "'");
                        txtJobs_Labor.Text = item;
                        txtJobsDesc.Text = dt.Rows[0]["DefJobDesc"].ToString().Trim();
                        txtLabor.Focus();
                        SetFocus(txtLabor);
                        if (ddlJobCardTypeCode.SelectedValue == "008")
                        {
                            txtLabor.Text = "0";
                            txtLabor.Enabled = false;
                        }
                    }
                    else
                    {
                        ddlJobs_Sublet_SelectedIndexChanged(item);
                        //dt = myFunc.GetData("select DefJobDesc from DefaultJob where DealerCode='" + Session["DealerCode"].ToString() + "' And  DefJobCode ='" + item + "'");
                        //txtSubletJobCode.Text = item;
                        //txtSubletJobDesc.Text = dt.Rows[0]["DefJobDesc"].ToString().Trim();
                        SetFocus(txtSubletPayAmnt);
                    }
                    
                }
               
            }
            catch (Exception ex)
            {
                myFunc.UserMsg(lblMsg, Color.Red, ex.Message);
            }
        }

        protected void ddlPartCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlJobCardTypeCode.SelectedValue == "012")
                {
                    txtDep.ReadOnly = false;
                }
                else
                {
                    txtDep.ReadOnly = true;

                }
                DataTable dt = new DataTable();
                dt = myFunc.GetData("select ItemDesc,SaleRate,PurchaseRate,PartItemNo from Item where DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') And  ItemCode='" + txtItemCode.Text + "'");
                txtItemDesc.Text = dt.Rows[0]["ItemDesc"].ToString().Trim();
                if (ddlJobCardTypeCode.SelectedValue == "008")
                {
                    txtPartPrice.Text = dt.Rows[0]["PurchaseRate"].ToString().Trim();
                }
                else
                {
                    txtPartPrice.Text = dt.Rows[0]["SaleRate"].ToString().Trim();
                }
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
                DataTable dt = myFunc.GetData("select ItemDesc,SaleRate,PartItemNo from Item where DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') And  ItemCode='" + txtLubCode.Text + "'"); //ddlLubCode.SelectedValue.ToString()
                txtLubPartDesc.Text = dt.Rows[0]["ItemDesc"].ToString().Trim();
                txtPartItemNo_Lubs.Text = dt.Rows[0]["PartItemNo"].ToString().Trim();
                txtLubPrice.Text = dt.Rows[0]["SaleRate"].ToString().Trim();
                //if (ddlJobCardTypeCode.SelectedValue != "012")
                //{
                //    lblLubType.Value = "P";
                
                //}
                //else
                //{
                //    lblLubType.Value = "A";
                //}

                txtLubQuantity.Focus();
            }
            catch (Exception ex)
            {
                myFunc.UserMsg(lblMsg, Color.Red, ex.Message);
            }
        }

        protected void btnAddLubs_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlJobCardTypeCode.SelectedValue == "007")
            {
                grl.UserMsg(lblMsg, Color.Red, "You Can Not Parts Lube in ComeBack Case");
                txtLubCode.Focus();
                return;
            }
           
               
           

            if (txtLubCode.Text == "")
            {

                grl.UserMsg(lblMsg, Color.Red, "Please select Lube Item", txtQuantity);
                return;
            }
            if (txtLubPrice.Text == "0"|| txtLubPrice.Text == "")
            {

                grl.UserMsg(lblMsg, Color.Red, "Zero amount can not be add in grid", txtQuantity);
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
                        | rowLubDT["ItemCode"].ToString().Trim() == txtLubCode.Text)
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
                if (ddlJobCardTypeCode.SelectedValue == "007")
                {
                    grl.UserMsg(lblMsg, Color.Red, "You Can Not Add Sublet Jobs in ComeBack Case");
                    txtSubletJobCode.Focus();
                    return;
                }
                if (ddlJobCardTypeCode.SelectedValue == "012")
                {
                    grl.UserMsg(lblMsg, Color.Red, "You Can Not Add Sublet Jobs in Insurance Case");
                    txtSubletJobCode.Focus();
                    return;
                }
                if (ddlVendorCode.SelectedIndex == 0)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Please Select Vendor");
                    return;
                }
                else
                {
                    lblMsg.Text = "";
                }
                if (txtSubletJobCode.Text == "")
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
                    grl.UserMsg(lblMsg, Color.Red, "Payable Amount is not be left be blank", txtSubletPayAmnt);
                    return;
                }
                else
                {
                    lblMsg.Text = "";
                }

                double invoiceAmt = Convert.ToDouble(txtSubletIncAmnt.Text.Trim());
                double paymentAmt = Convert.ToDouble(txtSubletPayAmnt.Text.Trim());

                if(paymentAmt > invoiceAmt)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Payable amount should not be greater than Invoice amount", txtSubletPayAmnt);
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
                        | rowSubletDT["JobCode"].ToString().Trim() == txtSubletJobCode.Text.Trim())
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

        protected void ddlJobs_Sublet_SelectedIndexChanged(string item)
        {
            try
            { 
               DataTable dt = new DataTable();
               dt = myFunc.GetData("select DefJobDesc from DefaultJob where DealerCode IN('" + Session["DealerCode"].ToString() + "','COMON') And  DefJobCode='" + item + "'");
                txtSubletJobCode.Text = item;
                txtSubletJobDesc.Text = dt.Rows[0]["DefJobDesc"].ToString().Trim();
                dt = myFunc.GetData("select Amount from LaborDetail where DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') And ProdCode='" + txtProduct.Text + "' And DefJobCode ='" + item + "' ");
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Amount"].ToString() != "")
                    {
                        txtSubletPayAmnt.Text = dt.Rows[0]["Amount"].ToString();
                    }
                    double amt, rate;

                    amt = SysFunctions.CustomCDBL(txtSubletPayAmnt.Text);
                    rate = SysFunctions.CustomCDBL(grl.GetStringValuesAgainstCodes("CusTypeCode='" + txtCusTypeCode.Text + "'", "CustomerType", "SubletRate"));


                    amt = (SysFunctions.CustomCDBL(txtSubletPayAmnt.Text) * rate) / 100;
                    amt = amt + SysFunctions.CustomCDBL(txtSubletPayAmnt.Text);
                    txtSubletIncAmnt.Text = amt.ToString();
                }
                txtSubletIncAmnt.Focus();
            }
            catch (Exception ex)
            {
                myFunc.UserMsg(lblMsg, Color.Red, ex.Message);
            }
        }

        protected void ddlConsumableParts_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {   
                DataTable dt = new DataTable();
                dt = myFunc.GetData("select ItemDesc,SaleRate,PartItemNo from Item where DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') And  ItemCode='" + txtConPartCode.Text+ "'");
                txtConPartDesc.Text = dt.Rows[0]["ItemDesc"].ToString().Trim();
                txtPartItemNoConParts.Text = dt.Rows[0]["PartItemNo"].ToString().Trim();
                txtConPartPrice.Text = dt.Rows[0]["SaleRate"].ToString().Trim();
                txtConQuantity.Focus();
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
            if (Convert.ToDecimal(txtConQuantity.Text) == 0)
            {
                
                grl.UserMsg(lblMsg, Color.Red, "Please enter Quantity", txtConQuantity);
                return;
            }
            if (Convert.ToDecimal(txtConQuantity.Text) < Convert.ToDecimal(txtConPartsRecQuantity.Text))
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
                        | rowConPartsDT["ItemCode"].ToString().Trim() == txtConPartCode.Text.Trim())
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
            //if (ddlJobCardCode.SelectedIndex == 0)
            //{
            //    ddlJobCardCode.SelectedValue = ViewState["JobCardCode"].ToString();
            //}
            ViewState["JobCardCode"] = txtJobCardCode.Text.Trim();
            LoadMasterData();
            ShowHistory();
        }

        protected void ddlEstNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               // ddlJobCardTypeCode.SelectedItem.Text=grl.GetStringValuesAgainstCodes("EstimateCode='" + txtExtno.Text + "'", "CustomerEstimateMaster", "Type","",Session["DealerCode"].ToString());
               
                DataTable dt = new DataTable();
                DataTable total = new DataTable();
                dt = myFunc.GetData("select RegNo,isnull(JobsTotal,0)JobsTotal,isnull(PartsTotal,0)PartsTotal,isnull(SubletTotal,0)SubletTotal,isnull(LubsTotal,0)LubsTotal,isnull(BoutTotal,0)BoutTotal,Type from CustomerEstimateMaster where DealerCode='" + Session["DealerCode"].ToString() + "' And  CustomerEstimateCode='" + txtExtno.Text + "'"); //ddlEstNo.SelectedValue.ToString().Trim()
                txtVehRegNo.Text = dt.Rows[0]["RegNo"].ToString().Trim();
                ddlVehRegNo_SelectedIndexChanged(txtVehRegNo, EventArgs.Empty);
                if (dt.Rows[0]["Type"].ToString().Trim()== "Insurance")
                {
                    ddlJobCardTypeCode.SelectedValue = "012";
                }
                else
                {
                    ddlJobCardTypeCode.SelectedValue = "003";
                }
                

                total = dt;
                /////////Customer Credit
               
                 /////////////////////////////////////
                createPartsDT();
                dt = myFunc.GetData("select C.ItemCode,C.Qty,C.Dep,I.ItemDesc,C.Type from CustomerEstimateParts C Inner Join Item I on I.ItemCode=C.ItemCode and I.DealerCode in ('COMON',C.DealerCode)  where C.DealerCode='" + Session["DealerCode"].ToString() + "' And  C.CustomerEstimateCode='" + txtExtno.Text + "'");
                foreach (DataRow dr in dt.Rows)
                {
                    txtQuantity.Text = dr["Qty"].ToString().Trim();
                    txtDep.Text = dr["Dep"].ToString().Trim();
                  //  SelectedPartDetail(dr["ItemCode"].ToString().Trim());
                    txtItemCode.Text = dr["ItemCode"].ToString().Trim();
                    lblPartType.Value= dr["Type"].ToString().Trim();

                    ddlPartCode_SelectedIndexChanged(txtItemCode, EventArgs.Empty);
                    btnAddParts_Click(btnAddParts, null);
                }
                createLubDT();
                dt = myFunc.GetData("select ItemCode,Qty,Dep,Type from CustomerEstimateLubricates where DealerCode='" + Session["DealerCode"].ToString() + "' And  CustomerEstimateCode='" + txtExtno.Text + "'");
                foreach (DataRow dr in dt.Rows)
                {
                    txtLubCode.Text = dr["ItemCode"].ToString().Trim(); //ddlLubCode.SelectedValue
                    txtLubQuantity.Text = dr["Qty"].ToString().Trim();
                    lblLubType.Value = dr["Type"].ToString().Trim();
                    txtLubDep.Text = dr["Dep"].ToString().Trim();
                    ddlLubCode_SelectedIndexChanged(txtLubCode, EventArgs.Empty);
                    btnAddLubs_Click(btnAddLubs, null);
                }
                createJobDT();
                dt = myFunc.GetData("select JobCode,JobRemarks,Labor from CustomerEstimateDetail where DealerCode='" + Session["DealerCode"].ToString() + "' And  CustomerEstimateCode='" + txtExtno.Text + "'");
                foreach (DataRow dr in dt.Rows)
                {
                    txtJobs_Labor.Text = dr["JobCode"].ToString().Trim();
                    txtJobRemarks.Text = dr["JobRemarks"].ToString().Trim();
                    txtLabor.Text = dr["Labor"].ToString().Trim();
                    ddlTechnicianEmpCode.SelectedIndex = 1;
                    ddlBay.SelectedIndex = 1;
                    BtnAddJobs_Click(BtnAddJobs, null);
                }
                SqlParameter[] dsParam = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                    new SqlParameter("@CustomerEstimateCode",SqlDbType.Char,8)
                                 };
                dsParam[0].Value = Session["DealerCode"].ToString();
                dsParam[1].Value = txtExtno.Text;
                dsJobCardSublet = new DataSet();
                dsJobCardSublet = grl.FillDataSet("sp_W2_Estimate_Sublet_Select", dsParam);
                //if (dsJobCardSublet.Tables[0].Rows.Count == 0) dsJobCardSublet.Tables[0].Rows.Add(dsJobCardSublet.Tables[0].NewRow());
                ViewState["SubLet"] = dsJobCardSublet.Tables[0]; gvSublet.DataSource = dsJobCardSublet; gvSublet.DataBind();

                dsJobCardBoutParts = new DataSet();
                dsJobCardBoutParts = grl.FillDataSet("sp_W2_Estimate_BoutPartsDetail_Select", dsParam);
                //if (dsJobCardConParts.Tables[0].Rows.Count == 0) dsJobCardConParts.Tables[0].Rows.Add(dsJobCardConParts.Tables[0].NewRow());
                ViewState["BoutParts"] = dsJobCardBoutParts.Tables[0]; gvJobCardPartsBought.DataSource = dsJobCardBoutParts; gvJobCardPartsBought.DataBind();
                Session["BoutPartsDataTables"] = dsJobCardBoutParts.Tables[0];
                txtJobCardTotal.Text = Convert.ToString(Convert.ToInt32(total.Rows[0]["JobsTotal"]) +
                                 Convert.ToInt32(total.Rows[0]["PartsTotal"]) +
                                 Convert.ToInt32(total.Rows[0]["LubsTotal"]) +
                                 Convert.ToInt32(total.Rows[0]["SubletTotal"]) +
                                 Convert.ToInt32(total.Rows[0]["BoutTotal"]));
            }
            catch (Exception ex)
            {
                myFunc.UserMsg(lblMsg, Color.Red, ex.Message);
            }

        }

        protected void btnLookup_Click(object sender, EventArgs e)
        {
            if (ddlJobCardTypeCode.SelectedValue == "012")
            {
                grl.UserMsg(lblMsg, Color.Red, "You Can Not Parts  in Insurance Case");
                txtItemCode.Focus();
                return;
            }
            ViewState["lookupid"] = 10; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3; ViewState["Partss"] = "Parts";
            clslook.LU_Get_Parts(imgLookup, ViewState["lookupid"].ToString(), "", "../../../");
            
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }


        protected void ddlScheduleJC_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlScheduleJC.SelectedIndex == 0)
            {
                return;
            }
            ScheduleData();            
        }

        private void ScheduleData()
        {
            SqlParameter[] param = { new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                                        new SqlParameter("@ProdCode",SqlDbType.Char,20),//01
                                        new SqlParameter("@KM",SqlDbType.Char)//02                                        
                                    };


            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = txtProduct.Text.Trim();
            param[2].Value = ddlScheduleJC.SelectedValue.Trim();


            dsJobCardDetail = myFunc.FillDataSet("Sp_ScheduleJobDetail_Select", param);
            dsJobCardLub = myFunc.FillDataSet("Sp_ScheduleLubDetail_Select", param);
            dsJobCardParts = myFunc.FillDataSet("Sp_SchedulePartsDetail_Select", param);

            if (dsJobCardDetail.Tables[0].Rows.Count > 0)
            {
                ViewState["Job"] = dsJobCardDetail.Tables[0]; gvJobCard.DataSource = dsJobCardDetail; gvJobCard.DataBind();
            }

            if (dsJobCardLub.Tables[0].Rows.Count > 0)
            {
                ViewState["Lub"] = dsJobCardLub.Tables[0]; gvLubParts.DataSource = dsJobCardLub; gvLubParts.DataBind();
            }

            if (dsJobCardParts.Tables[0].Rows.Count > 0)
            {
                ViewState["Parts"] = dsJobCardParts.Tables[0]; gvJobCardParts.DataSource = dsJobCardParts; gvJobCardParts.DataBind();
            }
            CalculateTotal();
        }

        protected void gv_JobTech_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow && !(grdBay.EditIndex == e.Row.RowIndex))
                {
                    //e.Row.Cells[0].Enabled = false;
                }

                if (e.Row.RowType == DataControlRowType.DataRow && (grdBay.EditIndex == e.Row.RowIndex))
                {

                    TextBox txtStartTime = (TextBox)e.Row.FindControl("txtStartTime");
                    TextBox txtEndTime = (TextBox)e.Row.FindControl("txtEndTime");

                    txtStartTime.Text = Session["lblStartTime"].ToString();
                    txtEndTime.Text = Session["lblEndTime"].ToString();                   
                }               

            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;
            }

        }
        protected void gv_JobTech_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                DataTable dt = new DataTable();

                Label lblStartTime = (Label)grdBay.Rows[e.NewEditIndex].FindControl("lblStartTime");
                Label lblEndTime = (Label)grdBay.Rows[e.NewEditIndex].FindControl("lblEndTime");

                Session["lblStartTime"] = lblStartTime.Text;
                Session["lblEndTime"] = lblEndTime.Text;


                grdBay.EditIndex = e.NewEditIndex;

                dt = (DataTable)ViewState["Bay"];

                grdBay.DataSource = dt;
                grdBay.DataBind();
                ViewState["Bay"] = dt;
            }

            catch (Exception ex)
            {

                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;

            }
        }

        protected void gv_JobTech_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            DataTable dt = new DataTable();

            grdBay.EditIndex = -1;

            dt = (DataTable)ViewState["Bay"];

            grdBay.DataSource = dt;
            grdBay.DataBind();
        }

        protected void gv_JobTech_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox txtStartTime = (TextBox)grdBay.Rows[e.RowIndex].FindControl("txtStartTime");
            TextBox txtEndTime = (TextBox)grdBay.Rows[e.RowIndex].FindControl("txtEndTime");
            Label lblJobCode = (Label)grdBay.Rows[e.RowIndex].FindControl("lblJobCode");
            Label lblBayCode = (Label)grdBay.Rows[e.RowIndex].FindControl("lblBayCode");
            Label lblStdTime = (Label)grdBay.Rows[e.RowIndex].FindControl("lblStdTime");
            Label lblEmpCode = (Label)grdBay.Rows[e.RowIndex].FindControl("lblEmpCode");

            DataTable dt = new DataTable();

            dt = (DataTable)ViewState["Bay"];

            if (txtStartTime.Text.Contains("_"))
            {
                grl.UserMsg(lblMsg, Color.Green, "Plese set Start Time");
                return;
            }

            SqlParameter[] Insert_param = {
                                                                new SqlParameter("@DealerCode",SqlDbType.Char,5),    //0
                                                                new SqlParameter("@JobCardCode",SqlDbType.Char,8),   //1
                                                                new SqlParameter("@JobCode",SqlDbType.VarChar,8),    //2
                                                                new SqlParameter("@StdTime",SqlDbType.VarChar,8),    //3
                                                                new SqlParameter("@StartTime",SqlDbType.VarChar,8),  //4
                                                                new SqlParameter("@EndTime",SqlDbType.VarChar,8),    //5
                                                                new SqlParameter("@EmpCode",SqlDbType.Char,3),       //6
                                                                new SqlParameter("@BayCode",SqlDbType.VarChar,5),    //7
                                                                new SqlParameter("@UpdDateTime",SqlDbType.DateTime), //8
                                                                new SqlParameter("@Terminal",SqlDbType.VarChar,10),  //9
                                                                new SqlParameter("@UpdUser",SqlDbType.VarChar,50)    //10
                                                            };

            try
            {
                Insert_param[0].Value = Session["DealerCode"].ToString();
                Insert_param[1].Value = txtJobCardCode.Text.Trim() == "" ? HFJobCard.Value.Trim() : txtJobCardCode.Text; //ddlJobCardCode.SelectedValue.ToString().Trim()
                Insert_param[2].Value = lblJobCode.Text.Trim();
                Insert_param[3].Value = lblStdTime.Text.Trim();
                Insert_param[4].Value = txtStartTime.Text.Trim();
                Insert_param[5].Value = txtEndTime.Text.Trim();
                Insert_param[6].Value = lblEmpCode.Text.Trim();
                Insert_param[7].Value = lblBayCode.Text.Trim();
                Insert_param[8].Value = myFunc.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"));
                Insert_param[9].Value = GlobalVar.mUserIPAddress;
                Insert_param[10].Value = Session["UserName"].ToString();

                if (myFunc.ExecuteSP_NonQuery("sp_W2_JobCardTech_Insert", Insert_param))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["JobCode"].ToString() == lblJobCode.Text)
                        {
                            dr["StartTime"] = txtStartTime.Text.Trim();
                            dr["EndTime"] = txtEndTime.Text.Trim();
                            if (txtEndTime.Text.Contains("_") == false)
                            {
                                DateTime start = Convert.ToDateTime(txtStartTime.Text);
                                DateTime End = Convert.ToDateTime(txtEndTime.Text);

                                dr["TimeTaken"] = End.Subtract(start).TotalMinutes;
                            }

                        }
                    }
                    grdBay.EditIndex = -1;
                    grdBay.DataSource = dt;
                    grdBay.DataBind();
                    ViewState["Bay"] = dt;

                    grl.UserMsg(lblMsg, Color.Green, "JobTech saved Successfully.");
                }
            }
            catch(Exception ex)
            {
                grl.UserMsg(lblMsg, Color.Red, ex.Message);
            }
            
        }

        protected void gv_JobTech_DataBound(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }
        }

        ///Gatev Pass
        ///
           #region
        protected void btnGatePass_Click(object sender, EventArgs e)
        {
            try
            {
                string oldgatepass;
                oldgatepass = grl.GetStringValuesAgainstCodes("Module='Service/JC' and TransCode='" + txtJobCardCode.Text + "'", "GatePassTemp", "GatePassCode");
                if (txtJobCardCode.Text == "" || txtJobCardCode.Text == "0" || txtJobCardCode.Text == null)
                {
                    lblMsg.Text = "Please Select the JobCard First!";
                    lblMsg.ForeColor = Color.Red;
                    return;
                }

                //if (!sysfun.IsExist("InvoiceNo", ddlInvoiceNo.SelectedValue.ToString(), "PaymentReceiptDetail", Session["DealerCode"].ToString()))
                //{
                //    lblMsg.Text = "Create a Payment Receipt First!";
                //    lblMsg.ForeColor = Color.Red;
                //    return;

                //}
                if (oldgatepass == null || oldgatepass == "")
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
                    GatePass_param[5].Value = "Service/JC";
                    GatePass_param[6].Value = Session["UserName"].ToString();
                    GatePass_param[7].Value = GlobalVar.mUserIPAddress;
                    GatePass_param[8].Value = "N";
                    GatePass_param[9].Value = "";
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
                if (Create_NDM(txtJobCardCode.Text.Trim(), "InvoiceCreated", oldgatepass) == false)
                {
                    grl.UserMsg(lblMsg, Color.Red, "NDM Data Can not be inserted");
                    ObjTrans.RollBackTransaction(ref Trans);
                    return;
                }
                if (PostServiceFollowUp(txtJobCardCode.Text.Trim(), "JobCardPost", oldgatepass) == false)
                {
                    grl.UserMsg(lblMsg, Color.Red, "Post Service FollowUp Data Can not be inserted");
                    ObjTrans.RollBackTransaction(ref Trans);
                    return;
                }

            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }
        private void MakeGatePassReport(string GatePass)
        {
            ReportDocument rpt, crReportDocument, RD;
            PdfDocument outputDocument = new PdfDocument();
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
            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "Report.pdf";
            string File = FilePath + FileName;

         
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



            // prepare pdf and show

            // create pdf 
            //string FilePath = Server.MapPath("~/Download/");
            //string FileName = "CounterSalesDetail" + Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            //string File = FilePath + FileName;
            ////.SetDatabaseLogon("SDMS", "sdms161", "192.168.1.47", "SDMS");
            //RD.ExportToDisk(ExportFormatType.Excel, File);

            //string URL;
            //URL = "../../../Download/rptViewerService.aspx?FileName=" + FileName;

            ////URL = FilePath + "OpenPdf.aspx?FileName=" + FileName;
            ////txtPartItemDesc.Text = URL;
            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ////URL = "../../../Download/OpenPdf.aspx?FileName=" + FileName; 
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

        }
        #endregion

    }


}