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
using System.Threading;
using System.IO;

namespace DXBMS.Modules.Service
{
    public partial class CustomerEstimate : System.Web.UI.Page
    {
        MainBLL objMBLL = new MainBLL();
        SysFunctions SysFunc = new SysFunctions();
        Transaction ObjTrans = new Transaction();
        SecurityBll sec = new SecurityBll();
        SqlTransaction Trans;
        //string apStr;
        bool search_result, search_item, IsSaved = false, IsDeleted = false;
        DataTable JobDT, LubDT, PartsDT, SubletDT, BoutDT, dt, ImageDT;
        DataSet dsJobCardDetail, dsJobCardParts, dsJobCardSublet, dsJobCardLub, ds, dsJobCardBoutParts, dsFIRImage;
        double totLabour, totParts, totlub;
        int countLabour, countParts, countlub, countlubRecQty, countPartsRecQty;
        //decimal deductAmount;
        clsLookUp clslook = new clsLookUp();
        string CustomerEstimateCode;
        static string Status;
        byte[] imgcontent;
        string p;
        double totsubletInv, totsubletPayable, totConParts, totBoutInv, totPayableBout, totEst, totJob;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2531", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }

            if (ViewState["Parts"] != null) PartsDT = (DataTable)ViewState["Parts"];
            if (ViewState["Job"] != null) JobDT = (DataTable)ViewState["Job"];
            if (ViewState["Lub"] != null) LubDT = (DataTable)ViewState["Lub"];
            if (ViewState["SubLet"] != null) SubletDT = (DataTable)ViewState["SubLet"];
            if (ViewState["Bout"] != null) BoutDT = (DataTable)ViewState["Bout"];
            if (ViewState["Image"] != null) ImageDT = (DataTable)ViewState["Image"];
            if (Page.IsPostBack)
            {

                if (!string.IsNullOrEmpty(Session["LookUpData"].ToString()))
                {
                    SelectedPartDetail(Session["LookUpData"].ToString());
                }
            }

            if (!IsPostBack)
            {

                ThreadStart childthreat = new ThreadStart(childthreadcall);
                Thread child = new Thread(childthreat);

                child.Start();


                setInitialDates();
                createJobDT();               //-----------------------------------JobCardDetail
                createLubDT();              //------------------------------------------PARTS            
                createPartsDT();
                createSubletDT();
                createImageDT();
                initializeDDLs(ddlDocType);           //-----------------------------------------JOB
                LoadBoutGrid();
                lblBranch.Visible = false;
                lblInsuranceComp.Visible = false;
                lblBranch.Visible = false;
                lblConveyer.Visible = false;
                ddlInsuranceComp.Visible = false;
                ddlInsBranch.Visible = false;
                txtConvyer.Visible = false;



                DataSet ds = new DataSet();
                DataSet ds_Schedule = new DataSet();
                SqlParameter[] JobCardMaster_param = {
                new SqlParameter("@DealerCode",SqlDbType.Char,5)};
                JobCardMaster_param[0].Value = Session["DealerCode"].ToString();
                //Fill Advisor Drop Down List
                objMBLL.FillDrp_SP(ddlAdvisor, "sp_Get_Advisor", "EmpCode", "EmpName", JobCardMaster_param, true, "--Select--", false, "");
                //Fill Insurance company Drop Down List
                objMBLL.FillDrp_SP(ddlInsuranceComp, "sp_2W_GetAllInsuranceCompany", "InsCompCode", "InsCompDescription", null, true, "--Select--", false, "");
                //Fill Insurance Branch Drop Down List
                //   objMBLL.FillDrp_SP(ddlInsBranch, "sp_2W_GetAllBranch", "BranchCode", "BranchDesc", null, true, "--Select--", false, "");
                //Fill CustomerEstimateCode Drop Down List
                //SysFunc.GetCustEstMultiColumnsDDL(ddlEstCode);
                Load_CustEstMultiColumnsDDL();
                // objMBLL.FillDrp_SP(ddlVendorCode, "sp_2W_Vendor_Select", "VendorCode", "VendorDesc", JobCardMaster_param, true, "--Select--", false, "");
                //Fill RegNo Drop Down List
                string[] Columns = new string[] { "RegNo", "EngineNo", "ChassisNo" };
                SysFunc.GetMultiColumnsDDL(ddlRegNo, Columns, "CustomerVehicle", "DealerCode ='" + Session["DealerCode"].ToString() + "' ", "RegNo", string.Empty, false, false);
                Load_VendorColumnsDDL();
                //Fill Parts Drop Down List
                Columns = new string[] { "ItemCode", "PartItemNo", "ItemDesc", "Source" };
                //SysFunc.GetMultiColumnsDDL(ddlParts, Columns, "Item", "DealerCode ='" + Session["DealerCode"].ToString() + "' And LubeFlag = 'N' ", "ItemCode", "Order by ItemDesc",false,false);
                //Fill Lubricates Drop Down List
                SysFunc.GetMultiColumnsDDL(ddllLubs, Columns, "Item", "DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') And LubeFlag = 'Y' ", "ItemCode", "Order by ItemDesc", false, false);
                SysFunc.ExecuteQuery("SELECT (DJ.DefJobDesc) ColName, DJ.DefJobCode ColValue FROM DefaultJob DJ LEFT OUTER JOIN JobCategory JC ON DJ.DealerCode = JC.DealerCode And DJ.JobCatCode = JC.JobCatCode Where DJ.DealerCode in ('" + Session["DealerCode"].ToString() + "','COMON') and DJ.VehicleCategory in ('" + Session["VehicleCategory"].ToString() + "','All')", ref ds);

                ddlJobs.DataSource = ds.Tables[0];
                ddlJobs.DataTextField = "ColName";
                ddlJobs.DataValueField = "ColValue";
                ddlJobs.DataBind();
            }
            Session["LookUpData"] = string.Empty;
        }
        private void Load_CustEstMultiColumnsDDL()
        {
            string[] Columns = new string[] { "CustomerEstimateCode", "Convert(Varchar(10),tdDate,105)", "RegNo", "UserName" };
            SysFunc.GetMultiColumnsDDL(ddlEstCode, Columns, "CustomerEstimateMaster", "DealerCode ='" + Session["DealerCode"].ToString() + "'  And DelFlag='N' ", "CustomerEstimateCode", string.Empty, false, false);

        }
        private void initializeDDLs(DropDownList ddl)
        {
            try
            {
                String[] col = new string[] { "DocChkListCode", "DocChkListDesc" };
                string where = "DealerCode in ('COMON','" + Session["DealerCode"].ToString() + "')";
                SysFunc.GetMultiColumnsDDL(ddl, col, "DocumentCheckList", where, "DocChkListCode", "", false, false);

            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }

        }
        private void Load_VendorColumnsDDL()
        {
            string[] Columns = new string[] { "VendorCode", "VendorDesc" };
            SysFunc.GetMultiColumnsDDL(ddlVendor, Columns, "Vendor", "DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') ", "VendorCode", string.Empty, false, false);

        }
        private void createImageDT()
        {
            ImageDT = new DataTable();

            ImageDT.Columns.Add(new DataColumn("ImageAddress", typeof(string)));
            ImageDT.Columns.Add(new DataColumn("ImageData", System.Type.GetType("System.Byte[]")));
            ImageDT.Columns.Add(new DataColumn("DocChkListCode", typeof(string)));
            ImageDT.Columns.Add(new DataColumn("DocChkListDesc", typeof(string)));

            //PartsDT.Rows.Add(new object[] { null, null, null, null, null, null, null, null, null,null });                        
            ImgGridView.DataSource = ImageDT; ImgGridView.DataBind(); ViewState["Image"] = ImageDT;
        }
        private void Insert_in_SubletDT(DataRow rowSubletDT)
        {
            //Insert_in_SubletDT(txtSubletVendor.Text, txtSubletVendor.Text, txtSubletJob.Text, txtSubletRemarks.Text,
            //txtSubletDate.Text, txtSubletIncAmnt.Text, txtSubletPayAmnt.Text, txtSubletJob.Text);

            rowSubletDT["VendorCode"] = ddlVendor.SelectedValue.ToString().Trim();
            rowSubletDT["Vendor"] = ddlVendor.SelectedItem.Text.Trim();
            //rowSubletDT["JobCode"] = ddlJobs_Sublet.SelectedValue.ToString().Trim(); 
            rowSubletDT["JobCode"] = txtSubletJobCode.Text.Trim();
            rowSubletDT["JobDescription"] = txtSubletJobDesc.Text.Trim();
            rowSubletDT["Remarks"] = txtSubletRemarks.Text.Trim().ToUpper();
            rowSubletDT["SubletDate"] = txtSubletDate.Text.Trim();
            rowSubletDT["SubletAmount"] = txtSubletIncAmnt.Text.Trim();
            rowSubletDT["PayableAmount"] = txtSubletPayAmnt.Text;
            //rowSubletDT["BayID"] = ddlBaySubletJob.SelectedValue.Trim();
            //if (TotalPaySublet.Text == "0" || TotalPaySublet==null)
            //{
            //    TotalPaySublet.Text = Convert.ToString(Convert.ToInt32(txtSubletPayAmnt.Text));
            //}
            //else
            //{
            //    TotalPaySublet.Text= Convert.ToString(Convert.ToInt32(TotalPaySublet.Text) + Convert.ToInt32(double.Parse(txtSubletPayAmnt.Text.Trim())));
            //}
            //if (TotalInvSublet.Text == "0" || TotalInvSublet == null)
            //{
            //    TotalInvSublet.Text = Convert.ToString(Convert.ToInt32(txtSubletIncAmnt.Text));
            //}
            //else
            //{
            //    TotalInvSublet.Text = Convert.ToString(Convert.ToInt32(TotalInvSublet.Text) + Convert.ToInt32(double.Parse(txtSubletIncAmnt.Text.Trim())));
            //}

            if (lblTotalAMount.Text != "")
            {
                lblTotalAMount.Text = Convert.ToString(Convert.ToInt32(lblTotalAMount.Text) + Convert.ToInt32(double.Parse(txtSubletIncAmnt.Text.Trim())));

            }
            else
            {
                lblTotalAMount.Text = Convert.ToString(Convert.ToInt32(double.Parse(txtSubletIncAmnt.Text.Trim())));
            }
        }
        protected void LoadMasterData()
        {
            SqlParameter[] dsParam = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                    new SqlParameter("@CustomerEstimateCode",SqlDbType.Char,8)
                                 };
            dsParam[0].Value = Session["DealerCode"].ToString();
            dsParam[1].Value = ddlEstCode.SelectedValue.ToString().Trim();
            txtExtno.Text = ddlEstCode.SelectedValue.ToString();


            DataSet dsJobCardMaster = new DataSet();
            DataSet Total = new DataSet();
            dsJobCardMaster = SysFunc.FillDataSet("sp_2W_CustomerEstimateMaster_Select", dsParam);
            Total = dsJobCardMaster;

            setVehcileInfo(dsJobCardMaster);
            setJobCardInfo(dsJobCardMaster);
            if (dsJobCardMaster.Tables[0].Rows[0]["ManEstimateNo"] == DBNull.Value)
            {
                txtManEstNo.Text = "";
            }
            else
            {
                txtManEstNo.Text = dsJobCardMaster.Tables[0].Rows[0]["ManEstimateNo"].ToString().Trim();
            }
            if (ddlInsuranceComp.SelectedItem.Text != "--Select--")
            {
                lblBranch.Visible = true;
                lblConveyer.Visible = true;
                lblConveyer.Visible = true;
                lblInsuranceComp.Visible = true;
                txtConvyer.Visible = true;
                ddlInsuranceComp.Visible = true;
                ddlInsBranch.Visible = true;
                txtDep.ReadOnly = false;
                txtDep.Text = "0";
                ddlJobCardType.Enabled = false;
            }
            else
            {
                txtDep.ReadOnly = true;
                txtDep.Text = "";
            }


            dsJobCardDetail = new DataSet();
            dsJobCardDetail = SysFunc.FillDataSet("sp_2W_CustomerEstimate_Detail_Select", dsParam);
            //if (dsJobCardDetail.Tables[0].Rows.Count == 0) dsJobCardDetail.Tables[0].Rows.Add(dsJobCardDetail.Tables[0].NewRow());
            ViewState["Job"] = dsJobCardDetail.Tables[0]; gvJobCard.DataSource = dsJobCardDetail; gvJobCard.DataBind();

            dsJobCardParts = new DataSet();
            dsJobCardParts = SysFunc.FillDataSet("sp_2W_CustomerEstimate_PartsDetail_Select", dsParam);
            //if (dsJobCardParts.Tables[0].Rows.Count == 0) dsJobCardParts.Tables[0].Rows.Add(dsJobCardParts.Tables[0].NewRow());
            ViewState["Parts"] = dsJobCardParts.Tables[0]; gvJobCardParts.DataSource = dsJobCardParts; gvJobCardParts.DataBind();

            dsJobCardLub = new DataSet();
            dsJobCardLub = SysFunc.FillDataSet("sp_2W_CustomerEstimate_LubricanteDetail_Select", dsParam);
            //if (dsJobCardLub.Tables[0].Rows.Count == 0) dsJobCardLub.Tables[0].Rows.Add(dsJobCardLub.Tables[0].NewRow());
            ViewState["Lub"] = dsJobCardLub.Tables[0]; gvLubParts.DataSource = dsJobCardLub; gvLubParts.DataBind();
            dsJobCardSublet = new DataSet();
            dsJobCardSublet = SysFunc.FillDataSet("sp_W2_Estimate_Sublet_Select", dsParam);
            //if (dsJobCardSublet.Tables[0].Rows.Count == 0) dsJobCardSublet.Tables[0].Rows.Add(dsJobCardSublet.Tables[0].NewRow());
            ViewState["SubLet"] = dsJobCardSublet.Tables[0]; gvSublet.DataSource = dsJobCardSublet; gvSublet.DataBind();

            dsJobCardBoutParts = new DataSet();
            dsJobCardBoutParts = SysFunc.FillDataSet("sp_W2_Estimate_BoutPartsDetail_Select", dsParam);
            //if (dsJobCardConParts.Tables[0].Rows.Count == 0) dsJobCardConParts.Tables[0].Rows.Add(dsJobCardConParts.Tables[0].NewRow());
            ViewState["BoutParts"] = dsJobCardBoutParts.Tables[0]; gvJobCardPartsBought.DataSource = dsJobCardBoutParts; gvJobCardPartsBought.DataBind();
            dsFIRImage = SysFunc.FillDataSet("sp_W2_Estimate_ImageDetail_Select", dsParam);
            //  if (dsJobCardParts.Tables[0].Rows.Count == 0) dsJobCardParts.Tables[0].Rows.Add(dsJobCardParts.Tables[0].NewRow());
            ViewState["Image"] = dsFIRImage.Tables[0];
            ImgGridView.DataSource = dsFIRImage; ImgGridView.DataBind();
            ImageDT = (DataTable)ViewState["Image"];
            Session["BoutPartsDataTables"] = dsJobCardBoutParts.Tables[0];
            ViewState["Bout"] = dsJobCardBoutParts.Tables[0];
            lblTotalAMount.Text = Convert.ToString(
                                 Convert.ToDecimal(Total.Tables[0].Rows[0]["JobsTotal"].ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : Total.Tables[0].Rows[0]["JobsTotal"].ToString().Replace("&nbsp;", "").Trim()) +
                                 Convert.ToDecimal(Total.Tables[0].Rows[0]["PartsTotal"].ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : Total.Tables[0].Rows[0]["PartsTotal"].ToString().Replace("&nbsp;", "").Trim()) +
                                 Convert.ToDecimal(Total.Tables[0].Rows[0]["LubsTotal"].ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : Total.Tables[0].Rows[0]["LubsTotal"].ToString().Replace("&nbsp;", "").Trim()) +
                                 Convert.ToDecimal(Total.Tables[0].Rows[0]["SubletTotal"].ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : Total.Tables[0].Rows[0]["SubletTotal"].ToString().Replace("&nbsp;", "").Trim()) +
                                 Convert.ToDecimal(Total.Tables[0].Rows[0]["BoutTotal"].ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : Total.Tables[0].Rows[0]["BoutTotal"].ToString().Replace("&nbsp;", "").Trim())
                                  );

        }

        private void setInitialDates()
        {
            txtCreateDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtSubletDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }

        private void ClearJobSubletTextBoxes()
        {
            TextBox[] txts = { txtSubletJobDesc, txtSubletPayAmnt, txtSubletRemarks, txtSubletIncAmnt };
            ClearTextBoxes(txts);
            ddlVendor.SelectedIndex = 0;
            //ddlJobs_Sublet.SelectedIndex = 0;
            txtSubletJobCode.Text = "";
            txtSubletJobDesc.Text = "";
            //   ddlBay.SelectedIndex = 0;

        }
        private void ClearBoutTextBoxes()
        {
            TextBox[] txts = { txtBAmt, txtBItemCode, txtBItemDesc, txtBPartNo, txtBPrice, txtBQty };
            ClearTextBoxes(txts);
            //  ddlVendor.SelectedIndex = 0;
            //ddlJobs_Sublet.SelectedIndex = 0;
            // txtSubletJobCode.Text = "";
            //  txtSubletJobDesc.Text = "";
            //   ddlBay.SelectedIndex = 0;

        }


        protected void LoadVehInfo()
        {
            DataSet dsVehInfo = new DataSet();
            SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",SqlDbType.VarChar,5),
                                    new SqlParameter("@RegNo",SqlDbType.VarChar,30)
                               };
            param[0].Value = Session["DealerCode"];
            param[1].Value = txtVehRegNo.Text.Trim().ToUpper();
            dsVehInfo = SysFunc.FillDataSet("sp_W2_CustomerVehicle_Select", param);
            if (dsVehInfo.Tables[0].Rows.Count > 0) setVehcileInfo(dsVehInfo);
        }

        private void setVehcileInfo(DataSet dsJobCardMaster)
        {
            if (dsJobCardMaster.Tables[0].Rows.Count > 0)
            {
                txtVehRegNo.Text = dsJobCardMaster.Tables[0].Rows[0]["RegNo"].ToString().Trim();
              
                txtChassisNo.Text = dsJobCardMaster.Tables[0].Rows[0]["ChassisNo"].ToString();
                txtEngineNo.Text = dsJobCardMaster.Tables[0].Rows[0]["EngineNo"].ToString();
                txtCustomer.Text = dsJobCardMaster.Tables[0].Rows[0]["CusCode"].ToString();
                txtCustomerDesc.Text = dsJobCardMaster.Tables[0].Rows[0]["CusDesc"].ToString();
                if (dsJobCardMaster.Tables[0].Columns.Contains("CustomerEstimateCode"))
                {
                    txtEndUser.Text = dsJobCardMaster.Tables[0].Rows[0]["CusCode"].ToString();
                    lblTotalAMount.Text = Convert.ToString(Convert.ToInt32(dsJobCardMaster.Tables[0].Rows[0]["JobsTotal"]) +
                                      Convert.ToInt32(dsJobCardMaster.Tables[0].Rows[0]["PartsTotal"]) +
                                      Convert.ToInt32(dsJobCardMaster.Tables[0].Rows[0]["LubsTotal"]) +
                                       Convert.ToInt32(dsJobCardMaster.Tables[0].Rows[0]["SubletTotal"]) +
                                        Convert.ToInt32(dsJobCardMaster.Tables[0].Rows[0]["BoutTotal"]));
                }
                else
                {
                    txtEndUser.Text = dsJobCardMaster.Tables[0].Rows[0]["EndUserCode"].ToString();
                }
                txtEndUserDesc.Text = dsJobCardMaster.Tables[0].Rows[0]["EndUserDesc"].ToString();
                txtBrand.Text = dsJobCardMaster.Tables[0].Rows[0]["BrandCode"].ToString();
                txtBrandDesc.Text = dsJobCardMaster.Tables[0].Rows[0]["BrandDesc"].ToString();
                txtProduct.Text = dsJobCardMaster.Tables[0].Rows[0]["ProdCode"].ToString();
                txtVersion.Text = dsJobCardMaster.Tables[0].Rows[0]["VersionCode"].ToString();
                txtVersionDesc.Text = dsJobCardMaster.Tables[0].Rows[0]["VersionDesc"].ToString();



                string InsCompCode = dsJobCardMaster.Tables[0].Rows[0]["InsCompCode"].ToString().Trim();
                string InsBranchCode = dsJobCardMaster.Tables[0].Rows[0]["InsBranchCode"].ToString().Trim();
                if (dsJobCardMaster.Tables[0].Rows[0]["Type"] == DBNull.Value)
                {
                    ddlJobCardType.SelectedIndex = 0;
                }
                else
                {
                    ddlJobCardType.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["Type"].ToString();
                }
                if (dsJobCardMaster.Tables[0].Columns.Contains("CustomerEstimateCode"))
                {

                    if (dsJobCardMaster.Tables[0].Rows[0]["InsCompCode"] != DBNull.Value)
                    {


                        ddlInsuranceComp.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["InsCompCode"].ToString().Trim();
                        ddlInsuranceComp_SelectedIndexChanged(null, null);
                        ddlInsBranch.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["InsBranchCode"].ToString().Trim();
                        txtConvyer.Text = dsJobCardMaster.Tables[0].Rows[0]["surveyor"].ToString().Trim();
                    }
                    else
                    {
                        ddlInsuranceComp_SelectedIndexChanged(null, null);
                        ddlJobCardType.SelectedIndex = 0;
                        ddlInsuranceComp.SelectedIndex = 0;
                        ddlInsBranch.SelectedIndex = 0;
                        txtConvyer.Text = string.Empty;
                    }
                    ddlAdvisor.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["TechCode"].ToString().Trim();
                    ddlJobCardType_SelectedIndexChanged(ddlJobCardType, null);
                }

                //Load Schedual Data in DDL

                SqlParameter[] param_Schedule = {
                                                                new SqlParameter("@ProdCode",SqlDbType.VarChar,10),
                                                                new SqlParameter("@VersionCode",SqlDbType.Char,3)
                                                              };
                param_Schedule[0].Value = txtProduct.Text.Trim();
                param_Schedule[1].Value = txtVersion.Text.Trim();
                objMBLL.FillDrp_SP(ddlSchedule, "sp_2W_MaintainenceSchedule_Select", "KM", "KM", param_Schedule, true, "--Select--", false, "");

            }


        }

        public void childthreadcall()
        {
            try
            {
                DataSet ds = new DataSet();
                string sQuery = "SP_SelectParts '" + Session["DealerCode"].ToString() + "', 'P'";

                if (SysFunc.ExecuteQuery(sQuery, ref ds))
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

        private void setJobCardInfo(DataSet dsJobCardMaster)
        {
            if (txtExtno.Text.Trim().Length != 0)
            {
                //lblJobCardTotal.Text = dsJobCardMaster.Tables[0].Rows[0]["JobsTotal"].ToString();
                //txtPartsTotal.Text = dsJobCardMaster.Tables[0].Rows[0]["PartsTotal"].ToString();
                //txtLubTotal.Text = dsJobCardMaster.Tables[0].Rows[0]["LubsTotal"].ToString();
                string TechCode = dsJobCardMaster.Tables[0].Rows[0]["TechCode"].ToString().Trim();
                ddlAdvisor.SelectedItem.Text = dsJobCardMaster.Tables[0].Rows[0]["EmpName"].ToString().Trim();


                string InsCompCode = dsJobCardMaster.Tables[0].Rows[0]["InsCompCode"].ToString().Trim();
                string InsBranchCode = dsJobCardMaster.Tables[0].Rows[0]["InsBranchCode"].ToString().Trim();
                if (dsJobCardMaster.Tables[0].Rows[0]["Type"] == DBNull.Value)
                {
                    ddlJobCardType.SelectedIndex = 0;
                }
                else
                {
                    ddlJobCardType.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["Type"].ToString();
                }
                if (ddlJobCardType.SelectedValue == "Insurance")
                {
                    if (dsJobCardMaster.Tables[0].Rows[0]["InsCompCode"] == DBNull.Value)
                    {
                        ddlInsuranceComp.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlInsuranceComp.SelectedItem.Text = dsJobCardMaster.Tables[0].Rows[0]["InsCompDescription"].ToString();
                    }
                    //if (dsJobCardMaster.Tables[0].Rows[0]["InsBranchCode"] == DBNull.Value)
                    //{
                    //    ddlInsuranceComp_SelectedIndexChanged(null, null);
                    //    ddlInsBranch.SelectedIndex = 0;
                    //}
                    //else
                    //{
                    //    ddlInsuranceComp_SelectedIndexChanged(null, null);
                    //    ddlInsBranch.SelectedItem.Text = dsJobCardMaster.Tables[0].Rows[0]["BranchDesc"].ToString();
                    //}
                }

                if (dsJobCardMaster.Tables[0].Rows[0]["surveyor"] == DBNull.Value)
                {
                    txtConvyer.Text = "";
                }
                else
                {
                    txtConvyer.Text = dsJobCardMaster.Tables[0].Rows[0]["surveyor"].ToString().Trim();
                }
                if (dsJobCardMaster.Tables[0].Rows[0]["ManEstimateNo"] == DBNull.Value)
                {
                    txtConvyer.Text = "";
                }
                else
                {
                    txtManEstNo.Text = dsJobCardMaster.Tables[0].Rows[0]["ManEstimateNo"].ToString().Trim();
                }
                if (dsJobCardMaster.Tables[0].Rows[0]["Schedule"].ToString() == "--Select")
                {
                    SqlParameter[] param_Schedule = {
                                                                new SqlParameter("@ProdCode",SqlDbType.VarChar,10),
                                                                new SqlParameter("@VersionCode",SqlDbType.Char,3)
                                                              };
                    param_Schedule[0].Value = txtProduct.Text.Trim();
                    param_Schedule[1].Value = txtVersion.Text.Trim();
                    //   objMBLL.FillDrp_SP(ddlSchedule, "sp_2W_MaintainenceSchedule_Select", "KM", "KM", param_Schedule, true, "--Select--", false, "");
                }
                else
                {
                    ddlSchedule.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["Schedule"].ToString().Trim();
                }
                txtRemarks.Text = dsJobCardMaster.Tables[0].Rows[0]["Remarks"].ToString().Trim();
                if (dsJobCardMaster.Tables[0].Rows[0]["ManEstimateNo"] == DBNull.Value)
                {
                    txtConvyer.Text = "";
                }
                else
                {
                    txtManEstNo.Text = dsJobCardMaster.Tables[0].Rows[0]["ManEstimateNo"].ToString().Trim();
                }
                SysFunc = new SysFunctions();
            }
        }
        private void LoadBoutGrid()
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

            Session["PartsDetailsDT"] = ds.Tables["PartsDetailsDS"];
            ViewState["Bout"] = ds.Tables["PartsDetailsDS"];
            ViewState["PartsDetailsDS"] = ds;
        }
        private void createJobDT()
        {
            JobDT = new DataTable();
            JobDT.Columns.Add(new DataColumn("ID", typeof(int)));
            JobDT.Columns.Add(new DataColumn("JobCode", typeof(string)));
            JobDT.Columns.Add(new DataColumn("DefJobDesc", typeof(string)));
            JobDT.Columns.Add(new DataColumn("JobRemarks", typeof(string)));
            JobDT.Columns.Add(new DataColumn("Amount", typeof(string)));
            JobDT.Columns.Add(new DataColumn("Type", typeof(string)));
            gvJobCard.DataSource = JobDT; gvJobCard.DataBind(); ViewState["Job"] = JobDT;
        }

        private void createLubDT()
        {
            LubDT = new DataTable();
            LubDT.Columns.Add(new DataColumn("ID", typeof(int)));
            LubDT.Columns.Add(new DataColumn("PartNo", typeof(string)));
            LubDT.Columns.Add(new DataColumn("PartsDesc", typeof(string)));
            LubDT.Columns.Add(new DataColumn("Dep", typeof(string)));
            LubDT.Columns.Add(new DataColumn("DepAmount", typeof(string)));
            LubDT.Columns.Add(new DataColumn("Qty", typeof(string)));
            LubDT.Columns.Add(new DataColumn("Price", typeof(string)));
            LubDT.Columns.Add(new DataColumn("Total", typeof(string)));
            LubDT.Columns.Add(new DataColumn("ItemCode", typeof(string)));
            LubDT.Columns.Add(new DataColumn("Type", typeof(string)));
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
            PartsDT.Columns.Add(new DataColumn("ItemCode", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("Type", typeof(string)));
            gvJobCardParts.DataSource = PartsDT; gvJobCardParts.DataBind(); ViewState["Parts"] = PartsDT;
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

                if (PartsDT.Rows.Count > 0)
                {
                    foreach (DataRow dr in PartsDT.Rows)
                    {

                        totParts = totParts + double.Parse(dr["Total"].ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : dr["Total"].ToString().Replace("&nbsp;", "").Trim());

                    }

                }
                if (JobDT.Rows.Count > 0)
                {
                    foreach (DataRow dr in JobDT.Rows)
                    {

                        totJob = totJob + double.Parse(dr["Amount"].ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : dr["Amount"].ToString().Replace("&nbsp;", "").Trim());

                    }

                }
                if (LubDT.Rows.Count > 0)
                {
                    foreach (DataRow dr in LubDT.Rows)
                    {

                        totlub = totlub + double.Parse(dr["Total"].ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : dr["Total"].ToString().Replace("&nbsp;", "").Trim());

                    }

                }
                if (SubletDT.Rows.Count > 0)
                {
                    foreach (DataRow dr in SubletDT.Rows)
                    {

                        totsubletInv = totsubletInv + double.Parse(dr["SubletAmount"].ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : dr["SubletAmount"].ToString().Replace("&nbsp;", "").Trim());

                    }

                }
                if (BoutDT.Rows.Count > 0)
                {
                    foreach (DataRow dr in BoutDT.Rows)
                    {

                        totBoutInv = totBoutInv + double.Parse(dr["InvoiceTotal"].ToString().Replace("&nbsp;", "").Trim() == "" ? "0" : dr["InvoiceTotal"].ToString().Replace("&nbsp;", "").Trim());

                    }

                }
                totEst = totJob + totlub + totsubletInv + totBoutInv + totParts;
                lblTotalAMount.Text = totEst.ToString();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2531", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (txtExtno.Text != "")
            {
                if (!sec.UserRight("2531", "003"))
                {
                    Response.Redirect("~/Test.aspx");
                }
            }

            int GVJobCard = gvJobCard.Rows.Count;
            int GVParts = gvJobCardParts.Rows.Count;
            int GVLub = gvLubParts.Rows.Count;

            if (Convert.ToDateTime(SysFunc.SaveDate(txtCreateDate.Text)).Date > Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd")).Date)
            //if (Convert.ToDateTime(txtCreateDate.Text).Date > Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd")).Date)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, "Date should not greater then current date");
                return;
            }

            TextBox[] textBoxes = {  txtVehRegNo,txtCustomer, txtEndUser, txtBrand, txtProduct, txtVersion ,
                                    txtChassisNo, txtEngineNo};
            if (!MasterValidation(textBoxes)) return;


            if (txtVehRegNo.Text == "")
            {
                SysFunc.UserMsg(lblMsg, Color.Red, "Error: RegNo should not be left blank");
                return;
            }
            if (ddlJobCardType.SelectedValue == "0")
            {
                SysFunc.UserMsg(lblMsg, Color.Red, "Error: Type should not be left blank");
                return;
            }
            string JobCard = SysFunc.GetStringValuesAgainstCodes("EstimateCode !='' and EstimateCode='" + txtExtno.Text + "'", "JobCardMaster", "JobCardCode");

            if (SysFunc.IsExist("JobCardCode", JobCard, "CustomerInvoice", Session["DealerCode"].ToString(), " And DelFlag='N' "))
            {
                SysFunc.UserMsg(lblMsg, Color.Red, "Error: Estimate Can Not Be Update becuase This Estimate is Already Used in Invoice");
                return;
            }
            if (ddlJobCardType.SelectedValue == "Insurance")
            {
                if (ddlInsuranceComp.SelectedItem.Text == "--Select--")
                {
                    SysFunc.UserMsg(lblMsg, Color.Red, "Error: Insurance Company should not be left blank");
                    return;

                }
                if (ddlInsBranch.SelectedItem.Text == "--Select--")
                {
                    SysFunc.UserMsg(lblMsg, Color.Red, "Error: Branch should not be left blank");
                    return;

                }
                if (txtConvyer.Text == "" || txtConvyer.Text == null)
                {
                    SysFunc.UserMsg(lblMsg, Color.Red, "Error: Surveyor should not be left blank");
                    return;

                }

            }


            if (ddlAdvisor.SelectedItem.Text == "--Select--")
            {
                SetFocus(ddlAdvisor);
                SysFunc.UserMsg(lblMsg, Color.Red, "Error: Advisor should not left Blank...");

            }

            else
            {
                if (GVJobCard > 0 | GVParts > 0 | GVLub > 0)
                {
                    if (CustomerEsTimate_Entry())
                    {
                        string[] Columns = new string[] { "CustomerEstimateCode", "RegNo", "UserName" };
                        SysFunc.GetMultiColumnsDDL(ddlEstCode, Columns, "CustomerEstimateMaster", "DealerCode ='" + Session["DealerCode"].ToString() + "'  And DelFlag='N'", "CustomerEstimateCode", "Order by CustomerEstimateCode Desc", false, false);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);
                    }

                }
                else
                {

                    SysFunc.UserMsg(lblMsg, Color.Red, "One of the detail must be selected !!!", txtQuantity);
                    return;
                }
            }
            btnSave.Enabled = true;
        }

        private bool CustomerEsTimate_Entry()
        {
            SqlParameter[] CustomerEstimate_param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
           /*1*/ new SqlParameter("@CustomerEstimateCode",SqlDbType.Char,8),
           /*2*/ new SqlParameter("@tdDate",SqlDbType.DateTime),
           /*3*/ new SqlParameter("@CusCode",SqlDbType.Char,8),
           /*4*/ new SqlParameter("@UserName",SqlDbType.Char,100),
           /*5*/ new SqlParameter("@BrandCode",SqlDbType.VarChar,15),
           /*6*/ new SqlParameter("@RegNo",SqlDbType.VarChar,25),
           /*7*/ new SqlParameter("@EngineNo",SqlDbType.VarChar,30),
           /*8*/ new SqlParameter("@ChassisNo",SqlDbType.VarChar,30),
           /*9*/ new SqlParameter("@ProdCode",SqlDbType.VarChar,20),
           /*10*/ new SqlParameter("@VersionCode",SqlDbType.Char,3),
           /*11*/ new SqlParameter("@JobsTotal",SqlDbType.Float),
           /*12*/ new SqlParameter("@PartsTotal",SqlDbType.Float),
           /*13*/ new SqlParameter("@LubsTotal",SqlDbType.Float),           
           /*14*/ new SqlParameter("@Remarks",SqlDbType.VarChar,50),
           /*15*/ new SqlParameter("@TechCode",SqlDbType.Char,3),
           /*16*/ new SqlParameter("@DelFlag",SqlDbType.Char,1),
           /*17*/ new SqlParameter("@UpdUser",SqlDbType.Char,50),
           /*18*/ new SqlParameter("@UpdDate",SqlDbType.DateTime),
           /*19*/ new SqlParameter("@UpdTime",SqlDbType.DateTime),
           /*20*/ new SqlParameter("@UpdTerm",SqlDbType.Char,50),
           /*21*/ new SqlParameter("@Schedule",SqlDbType.Char,8),
           /*22*/ new SqlParameter("@TransferStatus",SqlDbType.Char,1),
           /*23*/ new SqlParameter("@InsCompCode",SqlDbType.Char,4),
           /*24*/ new SqlParameter("@InsBranchCode",SqlDbType.Char,4),
           /*25*/ new SqlParameter("@surveyor",SqlDbType.VarChar,100),
           /*26*/ new SqlParameter("@SubletTotal",SqlDbType.Float),
           /*27*/ new SqlParameter("@BoutTotal",SqlDbType.Float),
           /*28*/ new SqlParameter("@Type",SqlDbType.VarChar,50),
           /*28*/ new SqlParameter("@ManEstNo",SqlDbType.VarChar,50),
                                                };
            CustomerEstimate_param[0].Value = Session["DealerCode"].ToString();
            //Auto Code Generation Decision on Insert 
            //for update CustomerEstimate

            Label lblFooterAmount = new Label();
            Label lblFooterPartAmount = new Label();
            Label lblFooterLubsAmount = new Label();
            if (gvJobCard.Rows.Count > 0) lblFooterAmount = (Label)gvJobCard.FooterRow.FindControl("lblFooterAmount"); else lblFooterAmount.Text = "0";

            if (gvJobCardParts.Rows.Count > 0) lblFooterPartAmount = (Label)gvJobCardParts.FooterRow.FindControl("lblFooterPartAmount"); else lblFooterPartAmount.Text = "0";
            if (gvLubParts.Rows.Count > 0) lblFooterLubsAmount = (Label)gvLubParts.FooterRow.FindControl("lblFooterLubsAmount"); else lblFooterLubsAmount.Text = "0";
            if (ddlEstCode.SelectedIndex == 0 || txtEstCode.Text == "" || txtEstCode.Text == "0")
            {

                if (ddlEstCode.SelectedIndex == 0)
                {
                    CustomerEstimateCode = SysFunc.AutoGen("CustomerEstimateMaster", "CustomerEstimateCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                }
                else
                {
                    CustomerEstimateCode = ddlEstCode.SelectedValue;
                }

                CustomerEstimate_param[1].Value = CustomerEstimateCode;
                CustomerEstimate_param[2].Value = SysFunc.SaveDate(txtCreateDate.Text);
                CustomerEstimate_param[3].Value = txtCustomer.Text.Trim();
                CustomerEstimate_param[4].Value = txtEndUserDesc.Text;
                CustomerEstimate_param[5].Value = txtBrand.Text.Trim();
                CustomerEstimate_param[6].Value = txtVehRegNo.Text.Trim().ToUpper();
                CustomerEstimate_param[7].Value = txtEngineNo.Text.Trim().ToUpper();
                CustomerEstimate_param[8].Value = txtChassisNo.Text.Trim().ToUpper();
                CustomerEstimate_param[9].Value = txtProduct.Text.Trim().ToUpper();
                CustomerEstimate_param[10].Value = txtVersion.Text.Trim().ToUpper();
                CustomerEstimate_param[11].Value = Convert.ToDecimal(lblFooterAmount.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblFooterAmount.Text.Replace("&nbsp;", "").Trim());
                CustomerEstimate_param[12].Value = Convert.ToDecimal(lblFooterPartAmount.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblFooterPartAmount.Text.Replace("&nbsp;", "").Trim());
                CustomerEstimate_param[13].Value = Convert.ToDecimal(lblFooterLubsAmount.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblFooterLubsAmount.Text.Replace("&nbsp;", "").Trim());
                CustomerEstimate_param[14].Value = txtRemarks.Text.Trim().ToUpper();
                CustomerEstimate_param[15].Value = ddlAdvisor.SelectedValue.ToString().Trim();
                CustomerEstimate_param[16].Value = "N";
                CustomerEstimate_param[17].Value = Session["UserName"].ToString();
                CustomerEstimate_param[18].Value = DateTime.Now;
                CustomerEstimate_param[19].Value = SysFunc.SaveTime(DateTime.Now.ToString("HH:mm"));
                CustomerEstimate_param[20].Value = GlobalVar.mUserIPAddress;
                if (ddlSchedule.SelectedItem.Text == "--Select--") CustomerEstimate_param[21].Value = ddlSchedule.SelectedValue.ToString();
                else CustomerEstimate_param[21].Value = DBNull.Value;
                CustomerEstimate_param[22].Value = "E";

                if (ddlJobCardType.SelectedValue == "Insurance")
                {
                    CustomerEstimate_param[23].Value = ddlInsuranceComp.SelectedValue.ToString();
                    CustomerEstimate_param[24].Value = ddlInsBranch.SelectedValue.ToString();
                    CustomerEstimate_param[25].Value = txtConvyer.Text.Trim();
                }
                else
                {
                    CustomerEstimate_param[23].Value = DBNull.Value;
                    CustomerEstimate_param[24].Value = DBNull.Value;
                    CustomerEstimate_param[25].Value = DBNull.Value;
                }
                CustomerEstimate_param[26].Value = TotalInvSublet.Text;
                CustomerEstimate_param[27].Value = lblTotalInvAmt.Text;
                CustomerEstimate_param[28].Value = ddlJobCardType.SelectedValue;
                CustomerEstimate_param[29].Value = txtManEstNo.Text;

            }
            else
            {
                //txtEstCode.Text = SysFunc.AutoGen_CustomerEstimate("CustomerEstimateMaster", "CustomerEstimateCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                CustomerEstimateCode = ddlEstCode.SelectedValue.ToString().Trim();
                CustomerEstimate_param[1].Value = CustomerEstimateCode;
                CustomerEstimate_param[2].Value = SysFunc.SaveDate(txtCreateDate.Text);
                CustomerEstimate_param[3].Value = txtCustomer.Text.Trim();
                CustomerEstimate_param[4].Value = txtEndUserDesc.Text;
                CustomerEstimate_param[5].Value = txtBrand.Text.Trim();
                CustomerEstimate_param[6].Value = txtVehRegNo.Text.Trim().ToUpper();
                CustomerEstimate_param[7].Value = txtEngineNo.Text.Trim().ToUpper();
                CustomerEstimate_param[8].Value = txtChassisNo.Text.Trim().ToUpper();
                CustomerEstimate_param[9].Value = txtProduct.Text.Trim().ToUpper();
                CustomerEstimate_param[10].Value = txtVersion.Text.Trim().ToUpper();
                CustomerEstimate_param[11].Value = lblFooterAmount.Text.Trim();
                CustomerEstimate_param[12].Value = lblFooterPartAmount.Text.Trim();
                CustomerEstimate_param[13].Value = lblFooterLubsAmount.Text.Trim();
                CustomerEstimate_param[14].Value = txtRemarks.Text.Trim().ToUpper();
                CustomerEstimate_param[15].Value = ddlAdvisor.SelectedValue.ToString().Trim();
                CustomerEstimate_param[16].Value = "N";
                CustomerEstimate_param[17].Value = Session["UserName"].ToString();
                CustomerEstimate_param[18].Value = DateTime.Now;
                CustomerEstimate_param[19].Value = SysFunc.SaveTime(DateTime.Now.ToString("HH:mm"));
                CustomerEstimate_param[20].Value = GlobalVar.mUserIPAddress;
                CustomerEstimate_param[21].Value = ddlSchedule.SelectedValue.ToString();
                CustomerEstimate_param[22].Value = "E";

                if (ddlJobCardType.SelectedValue == "Insurance")
                {
                    CustomerEstimate_param[23].Value = ddlInsuranceComp.SelectedValue.ToString();
                    CustomerEstimate_param[24].Value = ddlInsBranch.SelectedValue.ToString();
                    CustomerEstimate_param[25].Value = txtConvyer.Text.Trim();
                }
                else
                {
                    CustomerEstimate_param[23].Value = DBNull.Value;
                    CustomerEstimate_param[24].Value = DBNull.Value;
                    CustomerEstimate_param[25].Value = DBNull.Value;
                }
                CustomerEstimate_param[26].Value = TotalInvSublet.Text;
                CustomerEstimate_param[27].Value = lblTotalInvAmt.Text;
                CustomerEstimate_param[28].Value = ddlJobCardType.SelectedValue;
                CustomerEstimate_param[29].Value = txtManEstNo.Text;
            }
            try
            {
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    //For Update CustomerEstimate
                    if (ddlEstCode.SelectedIndex == 0)
                    {
                        if (SysFunc.ExecuteSP_NonQuery("[sp_2W_CustomerEstimate_Master_Insert]", CustomerEstimate_param, Trans))
                        {
                            //bool a = (d1 > 0 ? Inser_JobCardDetail() : false);
                            //bool a = (rowsInJobCardDetail(gvJobCard) == true ? Inser_CustomerEstimateDetail() : false);
                            if (gvJobCard.Rows.Count > 0) Inser_CustomerEstimateDetail();

                            //bool c = (d2 > 0 ? Inser_JobCardPartsDetail() : false);
                            //bool c = (rowInJobCardPartsDetail(gvJobCardParts) == true ? Inser_CustomerEstimePartsDetail() : false);
                            if (gvJobCardParts.Rows.Count > 0) Inser_CustomerEstimePartsDetail();
                            //bool d = (d3 > 0 ? Inser_JobCardLubricanteDetail() : false);
                            //bool d = (rowInJobCardLubricanteDetail(gvLubParts) == true ? Inser_CustomerEstimateLubricates_Detail() : false);
                            if (gvLubParts.Rows.Count > 0) Inser_CustomerEstimateLubricates_Detail();

                            if (gvSublet.Rows.Count > 0) Inser_JobCardSubletDetail();
                            if (gvJobCardPartsBought.Rows.Count > 0) Inser_JobCardBoutPartsDetail();
                            if (ImgGridView.Rows.Count > 0) Inser_EstimateImageDetail();

                            //bool b = (d4 > 0 ? Inser_JobCardSubletDetail() : false);
                        }

                        if (ObjTrans.CommittTransaction(ref Trans) == true)
                        {
                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Record Saved Successfully: " + CustomerEstimateCode + "')", true);
                            IsSaved = true;
                            clearAll();

                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);
                        }
                        else
                        {
                            ObjTrans.RollBackTransaction(ref Trans);
                            SysFunc.UserMsg(lblMsg, Color.Red, "Record nor saved Try again. Or contact to Admin: " + txtEstCode.Text, txtEstCode);
                            clearAll();

                        }

                    }
                    else
                    {
                        //Update Customer Estimate
                        CustomerEstimate_param[1].Value = ddlEstCode.SelectedValue.ToString();
                        if (SysFunc.ExecuteSP_NonQuery("[sp_Update_CustomerEstimate_Master]", CustomerEstimate_param, Trans))
                        {
                            if (gvJobCard.Rows.Count > 0) Inser_CustomerEstimateDetail();

                            //bool c = (d2 > 0 ? Inser_JobCardPartsDetail() : false);
                            //bool c = (rowInJobCardPartsDetail(gvJobCardParts) == true ? Inser_CustomerEstimePartsDetail() : false);
                            if (gvJobCardParts.Rows.Count > 0) Inser_CustomerEstimePartsDetail();
                            //bool d = (d3 > 0 ? Inser_JobCardLubricanteDetail() : false);
                            //bool d = (rowInJobCardLubricanteDetail(gvLubParts) == true ? Inser_CustomerEstimateLubricates_Detail() : false);
                            if (gvLubParts.Rows.Count > 0) Inser_CustomerEstimateLubricates_Detail();

                            if (gvSublet.Rows.Count > 0) Inser_JobCardSubletDetail();
                            if (gvJobCardPartsBought.Rows.Count > 0) Inser_JobCardBoutPartsDetail();
                            if (ImgGridView.Rows.Count > 0) Inser_EstimateImageDetail();
                            bool c;

                        }
                        else { SysFunc.UserMsg(lblMsg, Color.Red, "Update failed"); }


                        if (ObjTrans.CommittTransaction(ref Trans) == true)
                        {
                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Record Updated Successfully: " + CustomerEstimateCode + "')", true);
                            clearAll();
                            IsSaved = true;

                            // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Updatealert()", true);
                        }
                        else
                        {
                            ObjTrans.RollBackTransaction(ref Trans);
                            SysFunc.UserMsg(lblMsg, Color.Red, "Record not saved Try again. Or contact to Administrator: " + txtEstCode.Text, txtEstCode);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message, txtEstCode);
                ObjTrans.RollBackTransaction(ref Trans);
            }
            return IsSaved;

        }

        private bool Inser_CustomerEstimateDetail()
        {
            try
            {
                bool flag = false;
                SqlParameter[] Inser_CustomerEstimate_Delete_param = {
                                                                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                new SqlParameter("@CustomerEstimateCode",SqlDbType.Char,8)
                                                            };
                Inser_CustomerEstimate_Delete_param[0].Value = Session["DealerCode"].ToString();
                Inser_CustomerEstimate_Delete_param[1].Value = CustomerEstimateCode.Trim();
                SqlParameter[] JobCardDetail_Insert_param = {
                                                                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                new SqlParameter("@CustomerEstimateCode",SqlDbType.Char,8),
                                                                new SqlParameter("@JobCode",SqlDbType.VarChar,8),
                                                                new SqlParameter("@JobRemarks",SqlDbType.VarChar,200),
                                                                new SqlParameter("@Amount",SqlDbType.Float),
                                                                new SqlParameter("@Type",SqlDbType.Char,01)
                                                            };
                if (SysFunc.ExecuteSP_NonQuery("sp_W2_CustomerEstimate_Detail_Delete", Inser_CustomerEstimate_Delete_param, Trans))
                {
                    JobDT = (DataTable)ViewState["Job"];
                    DataRow[] drr = JobDT.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {
                        if (JobDT.Rows[i]["JobCode"].ToString() != ""
                            & JobDT.Rows[i]["Amount"].ToString() != "")
                        {
                            JobCardDetail_Insert_param[0].Value = Session["DealerCode"].ToString();
                            JobCardDetail_Insert_param[1].Value = CustomerEstimateCode.Trim();
                            JobCardDetail_Insert_param[2].Value = JobDT.Rows[i]["JobCode"].ToString();
                            JobCardDetail_Insert_param[3].Value = JobDT.Rows[i]["JobRemarks"].ToString();
                            JobCardDetail_Insert_param[4].Value = Convert.ToDecimal(JobDT.Rows[i]["Amount"].ToString());
                            JobCardDetail_Insert_param[5].Value = JobDT.Rows[i]["Type"].ToString();
                            SysFunc.ExecuteSP_NonQuery("sp_2W_CustomerEstimate_Detail_Insert", JobCardDetail_Insert_param, Trans);
                            flag = true;
                        }
                    }
                    // }
                }
                return flag;
            }
            catch (Exception ex) { throw ex; }
        }

        //Insert CustomerEstimeParts
        private bool Inser_CustomerEstimePartsDetail()
        {
            try
            {
                bool flag = false;
                SqlParameter[] CustomerEstimePartsDetail_Delete_param = {
                                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    new SqlParameter("@CustomerEstimateCode",SqlDbType.Char,8)
                                                                 };
                CustomerEstimePartsDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                CustomerEstimePartsDetail_Delete_param[1].Value = CustomerEstimateCode.Trim();

                SqlParameter[] CustomerEstimePartsDetail_Insert_param = {
                                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    new SqlParameter("@CustomerEstimateCode",SqlDbType.Char,8),
                                                                    new SqlParameter("@ItemCode",SqlDbType.VarChar,8),
                                                                    new SqlParameter("@PartNo",SqlDbType.Char,18),
                                                                    new SqlParameter("@Qty",SqlDbType.VarChar,10),
                                                                    new SqlParameter("@Price",SqlDbType.VarChar,50),
                                                                    new SqlParameter("@Dep",SqlDbType.VarChar,3),
                                                                    new SqlParameter("@Type",SqlDbType.Char,1)
                                                                 };

                if (SysFunc.ExecuteSP_NonQuery("sp_2W_CustomerEstimateParts_PartsDetail_Delete", CustomerEstimePartsDetail_Delete_param, Trans))
                {
                    PartsDT = (DataTable)ViewState["Parts"];
                    DataRow[] drr = PartsDT.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {
                        CustomerEstimePartsDetail_Insert_param[0].Value = Session["DealerCode"].ToString();
                        CustomerEstimePartsDetail_Insert_param[1].Value = CustomerEstimateCode.Trim();
                        if (PartsDT.Rows[i]["ItemCode"].ToString() != "")
                        {
                            CustomerEstimePartsDetail_Insert_param[2].Value = PartsDT.Rows[i]["ItemCode"].ToString(); //Item Code System Generted num. Get it from lookup
                            CustomerEstimePartsDetail_Insert_param[3].Value = PartsDT.Rows[i]["PartNo"].ToString();
                            CustomerEstimePartsDetail_Insert_param[4].Value = PartsDT.Rows[i]["Qty"].ToString();
                            CustomerEstimePartsDetail_Insert_param[5].Value = PartsDT.Rows[i]["Price"].ToString();
                            CustomerEstimePartsDetail_Insert_param[6].Value = PartsDT.Rows[i]["Dep"].ToString();
                            CustomerEstimePartsDetail_Insert_param[7].Value = PartsDT.Rows[i]["Type"].ToString();
                            SysFunc.ExecuteSP_NonQuery("sp_2W_CustomerEstimatePartsDetail_Insert", CustomerEstimePartsDetail_Insert_param, Trans);
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
            JobCardSubletDetail_Delete_param[1].Value = CustomerEstimateCode.Trim();
            //ddlEstCode.Text.Trim() == "" ? HFJobCard.Value.Trim() : ddlEstCode.Text.Trim();// ddlJobCardCode.SelectedValue.ToString().Trim();

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

            bool ans = SysFunc.ExecuteSP_NonQuery("sp_W2_Estimate_Sublet_Delete", JobCardSubletDetail_Delete_param, Trans);
            SubletDT = (DataTable)ViewState["SubLet"];
            DataRow[] drr = SubletDT.Select();
            for (int i = 0; i < drr.Length; i++)
            {
                JobCardSubletDetail_Insert_param[0].Value = Session["DealerCode"].ToString();
                JobCardSubletDetail_Insert_param[1].Value = CustomerEstimateCode.Trim();
                //ddlEstCode.Text.Trim() == "" ? HFJobCard.Value.Trim() : ddlEstCode.Text.Trim();// ddlJobCardCode.SelectedValue.ToString().Trim();

                if (SubletDT.Rows[i]["VendorCode"].ToString() != "")
                {
                    JobCardSubletDetail_Insert_param[2].Value = SubletDT.Rows[i]["VendorCode"].ToString();
                    JobCardSubletDetail_Insert_param[3].Value = SubletDT.Rows[i]["JobCode"].ToString();
                    JobCardSubletDetail_Insert_param[4].Value = SubletDT.Rows[i]["Remarks"].ToString();
                    JobCardSubletDetail_Insert_param[5].Value = SysFunc.SaveDate(SubletDT.Rows[i]["SubletDate"].ToString());
                    JobCardSubletDetail_Insert_param[6].Value = SubletDT.Rows[i]["SubletAmount"].ToString();
                    JobCardSubletDetail_Insert_param[7].Value = SubletDT.Rows[i]["PayableAmount"].ToString();
                    JobCardSubletDetail_Insert_param[8].Value = SubletDT.Rows[i]["BayID"].ToString();
                    SysFunc.ExecuteSP_NonQuery("sp_W2_Estimate_Sublet_Insert", JobCardSubletDetail_Insert_param, Trans);
                    flag = true;
                }
                // }
            }
            return flag;
        }

        private bool Inser_CustomerEstimateLubricates_Detail()
        {
            try
            {
                bool flag = false;
                SqlParameter[] CustomerEstimateLubricanteDetail_Delete_param = {
                                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    new SqlParameter("@CustomerEstimateCode",SqlDbType.Char,8)
                                                                  };
                CustomerEstimateLubricanteDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                CustomerEstimateLubricanteDetail_Delete_param[1].Value = CustomerEstimateCode.Trim();

                SqlParameter[] CustomerEstimateLubricanteDetail_Insert_param = {
                                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    new SqlParameter("@CustomerEstimateCode",SqlDbType.Char,8),
                                                                    new SqlParameter("@ItemCode",SqlDbType.VarChar,8),
                                                                    new SqlParameter("@PartNo",SqlDbType.Char,18),
                                                                    new SqlParameter("@Qty",SqlDbType.VarChar,10),
                                                                    new SqlParameter("@Price",SqlDbType.VarChar,50),
                                                                    new SqlParameter("@Dep",SqlDbType.VarChar,05),
                                                                    new SqlParameter("@Type",SqlDbType.Char,1),

                                                                  };

                if (SysFunc.ExecuteSP_NonQuery("sp_W2_CustomerEstimate_LubricanteDetail_Delete", CustomerEstimateLubricanteDetail_Delete_param, Trans))
                {
                    LubDT = (DataTable)ViewState["Lub"];
                    DataRow[] drr = LubDT.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {
                        CustomerEstimateLubricanteDetail_Insert_param[0].Value = Session["DealerCode"].ToString();
                        CustomerEstimateLubricanteDetail_Insert_param[1].Value = CustomerEstimateCode.Trim();
                        if (LubDT.Rows[i]["ItemCode"].ToString() != "")
                        {
                            CustomerEstimateLubricanteDetail_Insert_param[2].Value = LubDT.Rows[i]["ItemCode"].ToString(); ; //Item Code System Generted num. Get it from lookup
                            CustomerEstimateLubricanteDetail_Insert_param[3].Value = LubDT.Rows[i]["PartNo"].ToString();
                            CustomerEstimateLubricanteDetail_Insert_param[4].Value = LubDT.Rows[i]["Qty"].ToString();
                            CustomerEstimateLubricanteDetail_Insert_param[5].Value = LubDT.Rows[i]["Price"].ToString();
                            CustomerEstimateLubricanteDetail_Insert_param[6].Value = LubDT.Rows[i]["Dep"].ToString();
                            CustomerEstimateLubricanteDetail_Insert_param[7].Value = LubDT.Rows[i]["Type"].ToString();
                            if (SysFunc.ExecuteSP_NonQuery("sp_2W_CustomerEstimateLubricates_LubricanteDetail_Insert", CustomerEstimateLubricanteDetail_Insert_param, Trans))
                            {
                                flag = true;
                            }
                            else
                            {
                                flag = false;
                            }

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
                JobCardPartsDetail_Delete_param[1].Value = CustomerEstimateCode.Trim();
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

                if (SysFunc.ExecuteSP_NonQuery("sp_W2_Estimate_BoutPartsDetail_Delete", JobCardPartsDetail_Delete_param, Trans))
                {
                    BoutDT = (DataTable)Session["BoutPartsDataTables"];
                    DataRow[] drr = BoutDT.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {
                        JobCardPartsDetail_Insert_param[0].Value = Session["DealerCode"].ToString();
                        //JobCardPartsDetail_Insert_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                        JobCardPartsDetail_Insert_param[1].Value = CustomerEstimateCode.Trim();
                        //ddlEstCode.Text == "" ? HFJobCard.Value.Trim() : ddlEstCode.Text;
                        if (BoutDT.Rows[i]["ItemCode"].ToString() != "")
                        {

                            JobCardPartsDetail_Insert_param[2].Value = BoutDT.Rows[i]["ItemCode"].ToString(); //Item Code System Generted num. Get it from lookup
                            JobCardPartsDetail_Insert_param[3].Value = BoutDT.Rows[i]["PartNo"].ToString();
                            JobCardPartsDetail_Insert_param[4].Value = Convert.ToInt32(BoutDT.Rows[i]["Qty"].ToString());
                            JobCardPartsDetail_Insert_param[5].Value = Convert.ToDecimal(BoutDT.Rows[i]["Price"].ToString());
                            JobCardPartsDetail_Insert_param[6].Value = Convert.ToDecimal(BoutDT.Rows[i]["PurchaseTotal"].ToString());
                            JobCardPartsDetail_Insert_param[7].Value = Convert.ToDecimal(BoutDT.Rows[i]["InvRate"].ToString());//Type
                            JobCardPartsDetail_Insert_param[8].Value = Convert.ToDecimal(BoutDT.Rows[i]["InvoiceTotal"].ToString()); //Dep


                            SysFunc.ExecuteSP_NonQuery("sp_W2_Estimate_BoutPartsDetail_Insert", JobCardPartsDetail_Insert_param, Trans);
                            flag = true;
                        }
                    }
                }
                // }
                return flag;
            }
            catch (Exception ex) { throw ex; }

        }
        private bool rowsInJobCardDetail(GridView gvJobCard)
        {
            search_item = false;
            foreach (GridViewRow row in gvJobCard.Rows) { if (row.Cells[2].Text.Replace("&nbsp;", "").Trim() != "") search_item = true; }
            return search_item;
        }

        private bool rowInJobCardPartsDetail(GridView gvJobCardParts)
        {
            search_item = false;
            foreach (GridViewRow row in gvJobCardParts.Rows) { if (row.Cells[2].Text.Replace("&nbsp;", "").Trim() != "") search_item = true; }
            return search_item;
        }

        private bool rowInJobCardLubricanteDetail(GridView gvLubParts)
        {
            search_item = false;
            foreach (GridViewRow row in gvLubParts.Rows) { if (row.Cells[2].Text.Replace("&nbsp;", "").Trim() != "") search_item = true; }
            return search_item;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);
            clearAll();
        }

        protected void clearAll()
        {
            ddlEstCode.SelectedIndex = 0;
            txtExtno.Text = "";
            ddllLubs.SelectedIndex = 0;
            txtEstCode.Text = "";
            ddlJobCardType.SelectedIndex = 0;
            //ddlInsuranceComp.SelectedIndex = 0;
            //ddlInsBranch.SelectedIndex = 0;
            txtConvyer.Text = "";
            txtChassisNo.Text = "";
            txtEngineNo.Text = "";
            txtManEstNo.Text = "";
            txtCustomer.Text = "";
            txtCustomerDesc.Text = "";
            txtEndUser.Text = "";
            txtEndUserDesc.Text = "";
            txtBrand.Text = "";
            txtBrandDesc.Text = "";
            txtProduct.Text = "";
            txtVersion.Text = "";
            txtVehRegNo.Text = "";
            txtVersionDesc.Text = "";
            txtRemarks.Text = "";
            ddlAdvisor.SelectedIndex = 0;
            createLubDT();
            createPartsDT();
            createJobDT();
            createSubletDT();
            LoadBoutGrid();
            setInitialDates();
            ClearJobTextBoxes();
            ClearPartsTextBoxes();
            ClearLubricantsTextBoxes();
            ClearBoutTextBoxes();
            ClearJobSubletTextBoxes();
            lblMsg.Text = "";
            //txtPartTotalQuantity.Text = txtPartsTotal.Text =
            //lbltotLubQty.Text = txtLubTotal.Text =
            //.Text = txtJobsTotal.Text = "0";
            txtDep.Text = string.Empty;
            ddlRegNo.SelectedIndex = 0;
        }




        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2531", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }
            //string PDFFileName = MakeReport();

            //string URL = "../../../Download/OpenPdf.aspx?FileName=" + PDFFileName;
            string URL = "../../../Download/rptViewerService.aspx?ReportID=CustomerEstimate&CustEstimateCode=" + ddlEstCode.SelectedValue.ToString().Trim() + "&Type=" + ddlJobCardType.SelectedValue;
            //URL = FilePath + "OpenPdf.aspx?FileName=" + FileName;
            //txtPartItemDesc.Text = URL;
            string fullsysURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullsysURL, true);
        }
        protected void btnDelete_Click1(object sender, EventArgs e)
        {
            if (!sec.UserRight("2531", "004"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (ddlEstCode.SelectedIndex == 0)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, "Select Estimate code first. ");
                txtEstCode.Focus();
                return;
            }
            if (SysFunc.CodeExists("JobCardMaster", "EstimateCode", txtExtno.Text, Session["DealerCode"].ToString(), " And DelFlag='N'"))
            {
                SysFunc.UserMsg(lblMsg, Color.Red, "Estimate Code exist in Job Card ,it can not be deleted ");
                return;
            }

            if (Delete(txtExtno.Text) == false)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, "Customer Estimate code not deleted Try again");
                return;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Deletealert()", true);
                //   SysFunc.UserMsg(lblMsg, Color.Green, "Customer Estimate Code deleted successfully ");
                //SendAlert("Customer Estimate Code deleted successfully !!!");
                clearAll();
            }

        }


        //Delete Customer Estimate Code
        public bool Delete(string strCode)
        {
            //#BEGIN TRANSACTION
            SqlTransaction stDeleteJobCard = null;
            //#IF TRANSACTION COULD'NT BE STARTED RETURN FALSE
            if ((!ObjTrans.BeginTransaction(ref stDeleteJobCard)))
            {
                return false;
            }

            //1. Only CustomerEstimateMaster update value wiht DelFalg Y
            string sJobCardMasterUpdateQuery = "Update CustomerEstimateMaster Set DelFlag = 'Y',UpdUser = '" + Session["UserName"] + "',UpdDate = '" + SysFunc.SaveDate(DateTime.Now.ToString("dd/MM/yyyy")) + "',UpdTime = '" + DateTime.Now.ToString("HH:mm:ss") + "', UpdTerm = '" + GlobalVar.mUserIPAddress + "' Where CustomerEstimateCode='" + strCode + "' and DealerCode = '" + Session["DealerCode"] + "'";
            try
            {
                if ((!SysFunc.ExecuteQuery_NonQuery(sJobCardMasterUpdateQuery, stDeleteJobCard)))
                {
                    //IF THE step return false exit from the function
                    ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                SysFunc.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);

                return false;
            }

            //2. DELETE FROM CustomerEstimateDetail
            string sJobCardDetailDeleteQuery = "DELETE FROM CustomerEstimateDetail Where CustomerEstimateCode='" + strCode + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
            try
            {
                if ((!SysFunc.ExecuteQuery_NonQuery(sJobCardDetailDeleteQuery, stDeleteJobCard)))
                {
                    //IF THE step return false exit from the function
                    ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                SysFunc.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);

                return false;
            }

            //3. DELETE FROM CustomerEstimateParts
            string sJobCardPartDeleteQuery = "DELETE FROM CustomerEstimateParts Where CustomerEstimateCode='" + strCode + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
            try
            {
                if ((!SysFunc.ExecuteQuery_NonQuery(sJobCardPartDeleteQuery, stDeleteJobCard)))
                {
                    //IF THE step return false exit from the function
                    ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                SysFunc.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);

                return false;
            }

            //4.DELETE FROM CustomerEstimateLubricates
            string sJobCardLubDeleteQuery = "DELETE FROM CustomerEstimateLubricates Where CustomerEstimateCode='" + strCode + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
            try
            {
                if ((!SysFunc.ExecuteQuery_NonQuery(sJobCardLubDeleteQuery, stDeleteJobCard)))
                {
                    //IF THE step return false exit from the function
                    ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                SysFunc.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);

                return false;
            }
            //Delete Sublet
            //5. DELETE FROM CustomerEstimateParts
            string sEstimateSubletDeleteQuery = "DELETE FROM EstimateSublet Where EstimateCode='" + strCode + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
            try
            {
                if ((!SysFunc.ExecuteQuery_NonQuery(sEstimateSubletDeleteQuery, stDeleteJobCard)))
                {
                    //IF THE step return false exit from the function
                    ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                SysFunc.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);

                return false;
            }
            //6. DELETE FROM CustomerEstimateParts
            string sEstimateBoutDeleteQuery = "DELETE FROM EstimateBoutPartsDetail Where EstimateCode='" + strCode + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
            try
            {
                if ((!SysFunc.ExecuteQuery_NonQuery(sEstimateBoutDeleteQuery, stDeleteJobCard)))
                {
                    //IF THE step return false exit from the function
                    ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                SysFunc.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);

                return false;
            }
            //#COMMIT TRANSACTION
            ObjTrans.CommittTransaction(ref stDeleteJobCard);
            return true;
        }

        //protected void lnkRemove_Click2(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        LubDT = (DataTable)ViewState["Lub"];
        //        LinkButton btn = sender as LinkButton;
        //        TableCell tc = btn.Parent as TableCell;
        //        GridViewRow gvr = tc.Parent as GridViewRow;
        //        LubDT.Rows.RemoveAt(gvr.RowIndex);
        //        //Load grid 
        //        gvLubParts.DataSource = LubDT;
        //        LubDT.AcceptChanges();
        //        gvLubParts.DataBind();
        //        if (LubDT.Rows.Count == 0)
        //        {
        //            txtLubTotal.Text = "0";
        //            lbltotLubQty.Text = "0";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SysFunc.UserMsg(lblMsg, Color.Red, ex.Message, txtPartCode);
        //    }
        //}
        //protected void lnkRemove_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        PartsDT = (DataTable)ViewState["Parts"];
        //        LinkButton btn = sender as LinkButton;
        //        TableCell tc = btn.Parent as TableCell;
        //        GridViewRow gvr = tc.Parent as GridViewRow;
        //        PartsDT.Rows.RemoveAt(gvr.RowIndex);
        //        //Load grid 
        //        gvJobCardParts.DataSource = PartsDT;
        //        PartsDT.AcceptChanges();
        //        gvJobCardParts.DataBind();
        //        int sumParts = 0;

        //        if (PartsDT.Rows.Count <= 0)
        //        {
        //            txtPartsTotal.Text = "0";
        //            txtPartTotalQuantity.Text = "0";
        //        }
        //        else
        //        {
        //            foreach (DataRow dr in PartsDT.Rows)
        //            {
        //                sumParts = sumParts + Convert.ToInt32(dr["Qty"].ToString());
        //            }
        //            txtPartTotalQuantity.Text = sumParts.ToString();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        SysFunc.UserMsg(lblMsg, Color.Red, ex.Message, txtPartCode);
        //    }
        //}
        //protected void lnkRemove_Click1(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        JobDT = (DataTable)ViewState["Job"];
        //        LinkButton btn = sender as LinkButton;
        //        TableCell tc = btn.Parent as TableCell;
        //        GridViewRow gvr = tc.Parent as GridViewRow;
        //        JobDT.Rows.RemoveAt(gvr.RowIndex);
        //        //Load grid 
        //        gvJobCard.DataSource = JobDT;
        //        JobDT.AcceptChanges();
        //        gvJobCard.DataBind();
        //        if (JobDT.Rows.Count == 0)
        //        {
        //            txtJobsTotal.Text = "0";
        //            lblJobCardTotal.Text = "0";
        //        }
        //        else
        //        {
        //            txtJobsTotal.Text = JobDT.Rows.Count.ToString();

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        SysFunc.UserMsg(lblMsg, Color.Red, ex.Message, txtPartCode);
        //    }
        //}

        private void Insert_in_JobDT(DataRow rowJobDT)
        {
            rowJobDT["JobCode"] = txtJobCode.Text; rowJobDT["DefJobDesc"] = txtJobDesc.Text;
            rowJobDT["JobRemarks"] = "";
            rowJobDT["Amount"] = txtLabor.Text;

            rowJobDT["Type"] = ddlJobType.SelectedValue;

            if (lblTotalAMount.Text != "")
            {
                lblTotalAMount.Text = Convert.ToString(SysFunctions.CustomCDBL(lblTotalAMount.Text) + SysFunctions.CustomCDBL(txtLabor.Text));
            }
            else
            {
                lblTotalAMount.Text = Convert.ToString(SysFunctions.CustomCDBL(txtLabor.Text));
            }

        }

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
                    SysFunc.UserMsg(lblMsg, Color.Red, "Error: Field(s) Marked With Red Steriks '*' Are Mendetory...");
                    break;
                }
            }
            return isValid;
        }

        private void ClearJobTextBoxes()
        {
            TextBox[] txts = { txtLabor, txtJobDesc, txtJobCode };
            ClearTextBoxes(txts);
            ddlJobs.SelectedIndex = 0;
        }
        public void ClearTextBoxes(TextBox[] textBoxes)
        {
            for (int i = 0; i < textBoxes.Length; i++)
            { textBoxes[i].Text = string.Empty; }
        }

        //protected void btnJobCardRemove_Click(object sender, EventArgs e)
        //{
        //    JobDT = (DataTable)ViewState["Job"];
        //    JobDT.Rows.Remove(JobDT.Rows[ridx]);
        //    JobDT.AcceptChanges();
        //    ViewState["Job"] = JobDT; gvJobCard.DataSource = JobDT; gvJobCard.DataBind();
        //}



        private void ClearPartsTextBoxes()
        {
            TextBox[] txts = { txtQuantity, txtItemCode, txtItemDesc, txtPartPrice, txtItemCode, txtDep, txtItemNo };
            lblIssuedParts.Text = "";
            ClearTextBoxes(txts); txtPartsRecQuantity.Text = "0";
            //ddlParts.SelectedIndex = 0;
        }

        private void Insert_in_PartsDT(DataRow rowPartsDT)
        {
            rowPartsDT["PartNo"] = txtItemNo.Text.Trim(); rowPartsDT["PartsDesc"] = txtItemDesc.Text.Trim();
            if (ddlJobCardType.SelectedValue == "Insurance")
            {
                if (txtDep.Text.Trim() == string.Empty)
                {
                    txtDep.Text = "0";
                }
                rowPartsDT["Dep"] = txtDep.Text.Trim();
                rowPartsDT["DepAmount"] = (SysFunctions.CustomCDBL((SysFunctions.CustomCDBL(txtQuantity.Text.Trim()) * SysFunctions.CustomCDBL(txtPartPrice.Text.Trim()) / 100) * SysFunctions.CustomCDBL(txtDep.Text.Trim())));
            }
            else
            {
                rowPartsDT["Dep"] = "0";
                rowPartsDT["DepAmount"] = "0";
            }

            rowPartsDT["Qty"] = txtQuantity.Text.Trim(); rowPartsDT["Price"] = txtPartPrice.Text.Trim();
            rowPartsDT["Total"] = double.Parse(txtQuantity.Text.Trim()) * double.Parse(txtPartPrice.Text.Trim());
            rowPartsDT["ItemCode"] = txtItemCode.Text.Trim();

            if (lblTotalAMount.Text != "")
            {
                lblTotalAMount.Text = Convert.ToString(SysFunctions.CustomCDBL(lblTotalAMount.Text) + SysFunctions.CustomCDBL(double.Parse(txtQuantity.Text.Trim()) * double.Parse(txtPartPrice.Text.Trim())));
            }
            else
            {
                lblTotalAMount.Text = Convert.ToString(SysFunctions.CustomCDBL(SysFunctions.CustomCDBL(txtQuantity.Text.Trim()) * SysFunctions.CustomCDBL(txtPartPrice.Text.Trim())));
            }
            if (ddlJobCardType.SelectedValue == "Normal")
            {
                rowPartsDT["Type"] = "P";
            }
            else if (ddlJobCardType.SelectedValue == "Insurance")
            {
                if (ddlPartType.SelectedValue == "A")
                {
                    rowPartsDT["Type"] = "A";
                }
                else if (ddlPartType.SelectedValue == "S")
                {
                    rowPartsDT["Type"] = "S";
                }
            }

            else
            {
                rowPartsDT["Type"] = "P";
            }
        }





        private void ClearLubricantsTextBoxes()
        {
            TextBox[] txts = { txtLubPartCode, txtLubPartDesc, txtLubPrice, txtLubQuantity, txtItemcodeLub };
            ClearTextBoxes(txts);
            txtLubDep.Text = "0";
            txtLubRecQuantity.Text = "0";
            ddllLubs.SelectedIndex = 0;
            lblLubIssuedQty.Text = "0";
            lblLubIssuedQty.Visible = false;
        }

        private void Insert_in_LubPartsDT(DataRow rowLubDT)
        {
            rowLubDT["PartNo"] = txtLubPartCode.Text.Trim(); rowLubDT["PartsDesc"] = txtLubPartDesc.Text.Trim();
            if (ddlJobCardType.SelectedValue == "Insurance")
            {
                if (txtLubDep.Text.Trim() == string.Empty)
                {
                    txtLubDep.Text = "0";
                }
                rowLubDT["Dep"] = txtLubDep.Text.Trim();
                rowLubDT["DepAmount"] = (SysFunctions.CustomCDBL((SysFunctions.CustomCDBL(txtLubQuantity.Text.Trim()) * SysFunctions.CustomCDBL(txtLubPrice.Text.Trim()) / 100) * SysFunctions.CustomCDBL(txtLubDep.Text.Trim())));
            }
            else
            {
                rowLubDT["Dep"] = "0";
                rowLubDT["DepAmount"] = "0";
            }
            rowLubDT["Qty"] = txtLubQuantity.Text.Trim(); rowLubDT["Price"] = txtLubPrice.Text.Trim();
            rowLubDT["Total"] = double.Parse(txtLubQuantity.Text.Trim()) * double.Parse(txtLubPrice.Text.Trim());
            rowLubDT["ItemCode"] = ddllLubs.SelectedValue;
            if (lblTotalAMount.Text != "")
            {
                lblTotalAMount.Text = Convert.ToString(SysFunctions.CustomCDBL(lblTotalAMount.Text) + SysFunctions.CustomCDBL(SysFunctions.CustomCDBL(txtLubQuantity.Text.Trim()) * SysFunctions.CustomCDBL(txtLubPrice.Text.Trim())));
            }
            else
            {
                lblTotalAMount.Text = Convert.ToString(SysFunctions.CustomCDBL(SysFunctions.CustomCDBL(txtLubQuantity.Text.Trim()) * SysFunctions.CustomCDBL(txtLubPrice.Text.Trim())));
            }
            if (ddlJobCardType.SelectedValue == "Normal")
            {
                rowLubDT["Type"] = "L";
            }
            else if (ddlJobCardType.SelectedValue == "Insurance")
            {
                if (ddlLubType.SelectedValue == "A")
                {
                    rowLubDT["Type"] = "A";

                }
                else if (ddlLubType.SelectedValue == "S")
                {
                    rowLubDT["Type"] = "S";
                }

            }
            else
            {
                rowLubDT["Type"] = "L";
            }
        }

        protected void gvLubParts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //countlub = countlub + int.Parse(e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim());
                Label lblRowLubsAmount = (Label)e.Row.Cells[6].FindControl("lblRowLubsAmount");
                totlub = totlub + double.Parse(lblRowLubsAmount.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblRowLubsAmount.Text.Replace("&nbsp;", "").Trim());
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                totlub = 0; countlubRecQty = countlub = 0;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblFooterLubsAmount = (Label)e.Row.FindControl("lblFooterLubsAmount");
                lblFooterLubsAmount.Text = totlub.ToString();
                //lbltotLubQty.Text = countlub.ToString(); txtLubTotal.Text = totlub.ToString();
                //txttotLubRecQty.Text = countlubRecQty.ToString();
            }
        }
        //protected void gvLubParts_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ViewState["ridx"] = gvLubParts.SelectedRow.RowIndex;
        //        GridViewRow row = gvLubParts.Rows[gvLubParts.SelectedRow.RowIndex];
        //        ViewState["deductAmount"] = (Convert.ToDecimal(row.Cells[5].Text) * Convert.ToDecimal(row.Cells[7].Text)).ToString();
        //        txtItemcodeLub.Text = row.Cells[2].Text;
        //        ddllLubs.SelectedValue = txtItemcodeLub.Text;
        //        txtLubPartCode.Text = row.Cells[3].Text;
        //        txtLubPartDesc.Text = row.Cells[4].Text;
        //        txtLubQuantity.Text = row.Cells[5].Text;
        //        txtLubPrice.Text = row.Cells[6].Text;
        //    }
        //    catch (Exception ex) { throw ex; }
        //}
        protected void gvJobCardParts_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ViewState["ridx"] = gvJobCardParts.SelectedRow.RowIndex;
                GridViewRow row = gvJobCardParts.Rows[gvJobCardParts.SelectedRow.RowIndex];

                txtItemCode.Text = row.Cells[2].Text;
                txtItemNo.Text = row.Cells[3].Text;
                txtItemDesc.Text = row.Cells[4].Text;
                txtDep.Text = row.Cells[5].Text;
                txtPartsRecQuantity.Text = row.Cells[7].Text;
                txtQuantity.Text = row.Cells[7].Text;
                txtPartPrice.Text = row.Cells[8].Text;
                if (row.Cells[10].Text != "P")
                {
                    ddlPartType.SelectedValue = row.Cells[10].Text;
                }

                if (txtExtno.Text != "")
                {
                    if (SysFunc.IsExist("EstimateCode", txtExtno.Text, "JobCardMaster", Session["DealerCode"].ToString()))
                    {
                        string jobCardCOde = SysFunc.GetStringValuesAgainstCodes("EstimateCode", txtExtno.Text, "JobCardCode", "JobCardMaster", Session["DealerCode"].ToString());
                        SqlParameter[] dsParam = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                                 };

                        dsParam[0].Value = Session["DealerCode"].ToString();
                        dsParam[1].Value = jobCardCOde;

                        dsJobCardParts = new DataSet();
                        dsJobCardParts = SysFunc.FillDataSet("sp_W2_JobCard_PartsDetail_Select", dsParam);
                        if (dsJobCardParts.Tables[0].Rows.Count > 0)
                        {


                            foreach (DataRow dr in dsJobCardParts.Tables[0].Rows)
                            {
                                if (dr["ItemCode"].ToString() == txtItemCode.Text)
                                {
                                    lblIssuedParts.Text = "Issued Qty:" + dr["RecQty"].ToString();
                                }
                            }
                        }
                    }
                }
                ViewState["deductAmount"] = row.Cells[9].Text;
                PartsDT = (DataTable)ViewState["Parts"];
                //PartsDT.Rows.RemoveAt(gvJobCardParts.SelectedIndex);
                //gvJobCardParts.DataSource = PartsDT;
                //PartsDT.AcceptChanges();
                //gvJobCardParts.DataBind();


            }
            catch (Exception ex) { throw ex; }
        }
        protected void gvJobCardParts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblRowPartAmount = (Label)e.Row.Cells[9].FindControl("lblRowPartAmount");
                totParts = totParts + int.Parse(lblRowPartAmount.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblRowPartAmount.Text.Replace("&nbsp;", "").Trim());
                //totParts = totParts + double.Parse(e.Row.Cells[7].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[7].Text.Replace("&nbsp;", "").Trim());
                //countParts = countParts + int.Parse(e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim());
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                totParts = 0;
                countPartsRecQty = countParts = 0;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblFooterPartAmount = (Label)e.Row.FindControl("lblFooterPartAmount");
                lblFooterPartAmount.Text = totParts.ToString();
            }
        }
        //protected void gvJobCard_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ViewState["ridx"] = gvJobCard.SelectedRow.RowIndex;
        //        GridViewRow row = gvJobCard.Rows[gvJobCard.SelectedRow.RowIndex];
        //        ddlJobs.SelectedValue = row.Cells[2].Text;
        //        txtJobRemarks.Text = (row.Cells[4].Text != "&nbsp;" ? row.Cells[4].Text : "");
        //        txtLabor.Text = row.Cells[5].Text;
        //    }
        //    catch (Exception ex) { throw ex; }
        //}
        protected void gvJobCard_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblRowAmount = (Label)e.Row.Cells[4].FindControl("lblRowAmount");
                totLabour = totLabour + double.Parse(lblRowAmount.Text.Replace("&nbsp;", "") == "" ? "0" : lblRowAmount.Text);
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                countLabour = 0;
                totLabour = 0;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblFooterAmount = (Label)e.Row.FindControl("lblFooterAmount");
                lblFooterAmount.Text = totLabour.ToString();
                //lblJobCardTotal.Text = totLabour.ToString();
                //txt.Text = countLabour.ToString();
            }
        }

        protected void ddlJobCardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlJobCardType.SelectedValue == "Insurance")
            {
                lblBranch.Visible = true;
                lblInsuranceComp.Visible = true;
                lblBranch.Visible = true;
                lblConveyer.Visible = true;
                ddlInsuranceComp.Visible = true;
                ddlInsBranch.Visible = true;
                txtConvyer.Visible = true;
                ddlPartType.Visible = true;
                ddlLubType.Visible = true;
                ddlJobType.Visible = true;
            }
            else
            {
                lblBranch.Visible = false;
                lblInsuranceComp.Visible = false;
                lblBranch.Visible = false;
                lblConveyer.Visible = false;
                ddlInsuranceComp.Visible = false;
                ddlInsBranch.Visible = false;
                txtConvyer.Visible = false;
                ddlPartType.Visible = false;
                ddlLubType.Visible = false;
                ddlJobType.Visible = false;
            }

        }

        protected void ddlRegNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtVehRegNo.Text == "")
            {
                return;
            }
            LoadVehInfo();
        }

        //protected void ddlParts_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //if (ddlParts.SelectedItem.Text == "Select")
        //    //{
        //    //    return;
        //    //}
        //    DataSet ds = new DataSet();
        //  //  string strQuery = "Select " +
        //  //   "A.ItemCode, " +
        //  //   "A.ItemDesc, " +
        //  //   "A.UnitCode, " +
        //  //   "A.PartItemNo, " +
        //  //   "A.SaleRate, " +
        //  //   "IsNull(Sum(B.RecQty) + Sum(B.RetQty) - Sum(B.IssQty) - Sum(B.ChargeOutQty) - Sum(B.PurRetQty), 0) As [Quantity] " +
        //  //   "From    " +
        //  //   "Item A  " +
        //  //   "Left outer join ItemStock B    " +
        //  // "ON    " +
        //  // "A.ItemCode = B.ItemCode And    " +
        //  //"B.DealerCode = '" + Session["DealerCode"].ToString() + "' " +
        //  //"And A.ItemCode='" + ddlParts.SelectedValue.ToString().Trim() + "' " +
        //  //"Group by " +
        //  //"A.ItemCode, " +
        //  //   "A.ItemDesc, " +
        //  //   "A.UnitCode, " +
        //  //   "A.PartItemNo," +
        //  //   "A.SaleRate";
        //    string strQuery = "SELECT * FROM ITEM WHERE DealerCode='" + Session["DealerCode"] + "' And ItemCode= '" + txtPartCode.SelectedValue.ToString().Trim()+"'";
        //    SysFunc.ExecuteQuery(strQuery,ref ds);
        //    txtPartDesc.Text = ds.Tables[0].Rows[0]["ItemDesc"].ToString();
        //    txtPartCode.Text=ds.Tables[0].Rows[0]["PartItemNo"].ToString();
        //    txtPartPrice.Text = Convert.ToInt32 (ds.Tables[0].Rows[0]["SaleRate"]).ToString();
        //    txtItemcodeParts.Text = ds.Tables[0].Rows[0]["ItemCode"].ToString();
        //    //txtPartsRecQuantity.Text = ds.Tables[0].Rows[0]["Quantity"].ToString();
        //}

        protected void SelectedPartDetail(string item)
        {
            try
            { if (ViewState["lookupid"].ToString() == "10") {

                    DataTable dt = new DataTable();
                    dt = SysFunc.GetData("select ItemDesc,SaleRate,PartItemNo from Item where DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') And  ItemCode='" + item + "'");
                    txtItemCode.Text = item;
                    txtItemDesc.Text = dt.Rows[0]["ItemDesc"].ToString().Trim();
                    txtItemNo.Text = dt.Rows[0]["PartItemNo"].ToString().Trim();
                    txtPartPrice.Text = Convert.ToInt32(dt.Rows[0]["SaleRate"]).ToString();


                }
                else if (ViewState["lookupid"].ToString() == "21" && ViewState["job"].ToString() == "Sublet") {


                    ddlJobs_Sublet_SelectedIndexChanged(item);
                }
                else if (ViewState["lookupid"].ToString() == "3")
                {
                    txtVehRegNo.Text = item;
                   
                    ddlRegNo_SelectedIndexChanged(null, null);

                }
                else if (ViewState["lookupid"].ToString() == "82")
                {
                    decimal purchaserate = 0, tax;
                    dt = SysFunc.GetData("select ItemDesc,SaleRate,PartItemNo from Item where DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') And  ItemCode='" + item + "'");
                    txtBItemCode.Text = item;
                    txtBItemDesc.Text = dt.Rows[0]["ItemDesc"].ToString().Trim();
                    txtBPartNo.Text = dt.Rows[0]["PartItemNo"].ToString().Trim();
                    txtBPrice.Text = dt.Rows[0]["SaleRate"].ToString().Trim();
                    TabPanel2.Focus();
                    txtQuantity.Focus();
                }
                else if (ViewState["lookupid"].ToString() == "73")
                {
                    txtExtno.Text = item;
                    ddlEstCode.SelectedValue = item;
                    ddlEstCode_SelectedIndexChanged(null, null);

                }
                else if (ViewState["lookupid"].ToString() == "21" && ViewState["job"].ToString() == "Job")
                {
                    ddlJobs.SelectedValue = item;
                    try
                    {
                        if (ddlJobs.SelectedValue != "")
                        {
                            ddlJobs_SelectedIndexChanged(null, null);
                            DataTable dt = new DataTable();
                            dt = SysFunc.GetData("select DefJobDesc from DefaultJob where DealerCode IN('" + Session["DealerCode"].ToString() + "','COMON') And  DefJobCode='" + item + "'");
                            txtJobCode.Text = item;
                            txtJobDesc.Text = dt.Rows[0]["DefJobDesc"].ToString().Trim();
                            SetFocus(txtLabor);
                            // TabPanel1.Focus();
                        }

                    }
                    catch (Exception ex)
                    {
                        SysFunc.UserMsg(lblMsg, Color.Red, ex.Message);
                    }

                }

            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, ex.Message);
            }
        }

        protected void ddlJobs_Sublet_SelectedIndexChanged(string item)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = SysFunc.GetData("select DefJobDesc from DefaultJob where DealerCode IN('" + Session["DealerCode"].ToString() + "','COMON') And  DefJobCode='" + item + "'");
                txtSubletJobCode.Text = item;
                txtSubletJobDesc.Text = dt.Rows[0]["DefJobDesc"].ToString().Trim();
                txtSubletIncAmnt.Focus();
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, ex.Message);
            }
        }

        protected void ddllLubs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddllLubs.SelectedItem.Text == "Select")
            {
                return;
            }
            DataSet ds = new DataSet();
            SysFunc.ExecuteQuery("Select * From Item Where ItemCode='" + ddllLubs.SelectedValue.ToString().Trim() + "'", ref ds);
            txtLubPartDesc.Text = ds.Tables[0].Rows[0]["ItemDesc"].ToString();
            txtLubPartCode.Text = ds.Tables[0].Rows[0]["PartItemNo"].ToString();
            txtItemcodeLub.Text = ds.Tables[0].Rows[0]["ItemCode"].ToString();
            txtLubPrice.Text = Convert.ToInt32(ds.Tables[0].Rows[0]["SaleRate"]).ToString();
        }

        protected void btnAddSublet_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ddlVendor.SelectedIndex == 0)
                {
                    SysFunc.UserMsg(lblMsg, Color.Red, "Please Select Vendor");
                    return;
                }
                else
                {
                    lblMsg.Text = "";
                }
                if (txtSubletJobCode.Text == "")
                {
                    SysFunc.UserMsg(lblMsg, Color.Red, "Please Select Sublet job", txtCustomer);
                    return;
                }
                else
                {
                    lblMsg.Text = "";
                }
                if (txtSubletDate.Text.Trim() == string.Empty || txtSubletDate.Text.Trim() == "__-__-____")
                {
                    SysFunc.UserMsg(lblMsg, Color.Red, "Please Select Sublet Date", txtSubletDate);
                    return;
                }
                else
                {
                    lblMsg.Text = "";
                }
                if (txtSubletIncAmnt.Text.Trim() == string.Empty)
                {
                    SysFunc.UserMsg(lblMsg, Color.Red, "Invoice Amount is not be left be blank", txtSubletIncAmnt);
                    return;
                }
                else
                {
                    lblMsg.Text = "";
                }
                if (txtSubletPayAmnt.Text.Trim() == string.Empty)
                {
                    SysFunc.UserMsg(lblMsg, Color.Red, "Payable Amount is not be left be blank", txtSubletPayAmnt);
                    return;
                }
                else
                {
                    lblMsg.Text = "";
                }

                double invoiceAmt = Convert.ToDouble(txtSubletIncAmnt.Text.Trim());
                double paymentAmt = Convert.ToDouble(txtSubletPayAmnt.Text.Trim());

                if (paymentAmt > invoiceAmt)
                {
                    SysFunc.UserMsg(lblMsg, Color.Red, "Payable amount should not be greater than Invoice amount", txtSubletPayAmnt);
                    return;
                }
                else
                {
                    lblMsg.Text = "";
                }
                foreach (DataRow dr in JobDT.Rows)
                {
                    string JobCode = dr["JobCode"].ToString();
                    if (JobCode == txtSubletJobCode.Text)
                    {
                        lblMsg.Text = "Job Already exist in Job Tab";
                        lblMsg.ForeColor = Color.Red;
                        TabPanel4.Focus();
                        return;
                    }

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
                CalculateTotal();
                ClearJobSubletTextBoxes();
            }
            catch (Exception ex) { throw ex; }

        }

        protected void btnClearSublet_Click(object sender, ImageClickEventArgs e)
        {
            ClearJobSubletTextBoxes();
        }

        protected void gvSublet_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvSublet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //e.Row.Cells[9].Visible = false;
            //  e.Row.Cells[2].Visible = e.Row.Cells[4].Visible = false;

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
                TotalPaySublet.Text = Convert.ToString(totsubletPayable);
                TotalInvSublet.Text = Convert.ToString(totsubletInv);
            }

        }

        protected void ImgAddBought_Click(object sender, ImageClickEventArgs e)
        {
            if (txtBQty.Text == "")
            {
                txtBQty.Text = "0";
            }
            if (Convert.ToInt32(txtBQty.Text) == 0)
            {

                SysFunc.UserMsg(lblMsg, Color.Red, "Please enter Quantity", txtBQty);
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
            TextBox[] textBoxes = { txtBPrice, txtBQty };
            if (!MasterValidation(textBoxes)) return;
            else
            {
                foreach (DataRow dr in PartsDT.Rows)
                {
                    string itemdesc = dr["ItemCode"].ToString();
                    if (itemdesc == txtBItemCode.Text)
                    {
                        lblMsg.Text = "Part Already exist in Part Tab";
                        lblMsg.ForeColor = Color.Red;
                        TabPanel8.Focus();
                        return;
                    }

                }
                foreach (DataRow dr in BoutDT.Rows)
                {
                    string itemdesc = dr["ItemCode"].ToString();
                    if (itemdesc == txtBItemCode.Text)
                    {
                        lblMsg.Text = "BoughtOut  Already exist in Part Tab";
                        lblMsg.ForeColor = Color.Red;
                        TabPanel8.Focus();
                        return;
                    }

                }

                search_result = false;
                foreach (DataRow rowPartsDT in BoutDT.Rows)
                {
                    if (rowPartsDT["ItemCode"].ToString().Trim() == ""
                        | rowPartsDT["ItemCode"].ToString().Trim() == txtBItemCode.Text)
                    {
                        // Insert_in_BoutDT(rowPartsDT);

                        // lblMsg.Text = "Part Already Exist";
                        SysFunc.UserMsg(lblMsg, Color.Red, "Part Already Exist", txtBItemCode);
                        search_result = true;

                    }
                }
                if (search_result == false)
                {
                    DataRow rowPartsDT = PartsDT.NewRow(); Insert_in_BoutDT(rowPartsDT);
                    //PartsDT.Rows.Add(rowPartsDT);
                }
                //  ViewState["Parts"] = PartsDT; gvJobCardPartsBought.DataSource = PartsDT; gvJobCardPartsBought.DataBind();
                int Count = 0;
                double InvAmt = 0;
                foreach (DataRow dr in BoutDT.Rows)
                {
                    Count = Count + Convert.ToInt32(dr["Qty"]);
                    InvAmt = InvAmt + Convert.ToDouble(dr["InvoiceTotal"]);
                }
                // lblTotalInvAmt.Text = InvAmt.ToString();
                // lblBoutPartCount.Text = Count.ToString();

                //ClearPartsTextBoxes();
                CalculateTotal();
                ClearBoutTextBoxes();
            }

        }

        protected void btnClearBParts_Click(object sender, ImageClickEventArgs e)
        {
            ClearBoutTextBoxes();
        }

        protected void imgEstNo_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 73;
            clslook.LU_Get_CustomerEstimateNoForm(imgEstNo, ViewState["lookupid"].ToString(), "", "../../../");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void imgVehRegNo_Click(object sender, ImageClickEventArgs e)
        {

            ViewState["lookupid"] = 3; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            string sql = "Select RegNo , ChassisNo , EngineNo , C.CusDesc from CustomerVehicle A inner join Customer C on C.CusCode = A.CusCode and C.DealerCode = A.DealerCode";
            clslook.LU_Get_RecVeh(imgVehRegNo, ViewState["lookupid"].ToString(), "", sql, "../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void ddlInsuranceComp_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlParameter[] JobCardMaster_param = {
                new SqlParameter("@InsCompCode",SqlDbType.Char,4)};
            JobCardMaster_param[0].Value = ddlInsuranceComp.SelectedValue;

            //Fill Insurance Branch Drop Down List
            objMBLL.FillDrp_SP(ddlInsBranch, "sp_2W_GetAllBranch", "BranchCode", "BranchDesc", JobCardMaster_param, true, "--Select--", false, "");

        }

        protected void gvJobCard_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ViewState["ridx"] = gvJobCard.SelectedRow.RowIndex;
                GridViewRow row = gvJobCard.Rows[gvJobCard.SelectedRow.RowIndex];
                Label lblRowAmount = (Label)gvJobCard.Rows[gvJobCard.SelectedRow.RowIndex].FindControl("lblRowAmount");
                //ddlJobs_Labor.SelectedValue = row.Cells[2].Text.Trim();
                txtJobCode.Text = row.Cells[2].Text.Trim();
                txtJobDesc.Text = row.Cells[3].Text;
                string techCode = row.Cells[4].Text.Trim();
                //      ddlTechnicianEmpCode.SelectedValue = row.Cells[4].Text.Trim();
                //txtTechnician.Text = row.Cells[4].Text;
                //txtTechnicianDesc.Text = row.Cells[5].Text;
                //ddlJobCardType.Text = row.Cells[6].Text;
                // txtJobRemarks.Text = (row.Cells[7].Text != "&nbsp;" ? row.Cells[7].Text : "");
                txtLabor.Text = lblRowAmount.Text;
                // ddlTechnicianEmpCode.SelectedValue = techCode;
                //  ViewState["deductAmount"] = decimal.Parse(row.Cells[8].Text);
                JobDT = (DataTable)ViewState["Job"];
                JobDT.Rows.RemoveAt(gvJobCard.SelectedIndex);
                gvJobCard.DataSource = JobDT;
                JobDT.AcceptChanges();
                gvJobCard.DataBind();
                //if (JobDT.Rows.Count == 0)
                //{
                //    txtJobsTotal.Text = "0";
                //    lblJobCardTotal.Text = "0";
                //}
                //else
                //{
                //    txtJobsTotal.Text = JobDT.Rows.Count.ToString();

                //}
            }
            catch (Exception ex) { lblMsg.Text = ex.Message; }

        }

        protected void gvLubParts_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ViewState["ridx"] = gvLubParts.SelectedRow.RowIndex;
                GridViewRow row = gvLubParts.Rows[gvLubParts.SelectedRow.RowIndex];

                ddllLubs.SelectedValue = row.Cells[2].Text;
                txtLubPartCode.Text = row.Cells[3].Text;
                txtLubPartDesc.Text = row.Cells[4].Text;
                txtLubQuantity.Text = row.Cells[5].Text;
                txtLubDep.Text = row.Cells[6].Text;
                txtLubRecQuantity.Text = row.Cells[7].Text;
               
                txtLubPrice.Text = row.Cells[8].Text;
                if (row.Cells[10].Text != "L")
                {
                    ddlLubType.SelectedValue = row.Cells[10].Text;
                }

                if (txtExtno.Text != "")
                {
                    if (SysFunc.IsExist("EstimateCode", txtExtno.Text, "JobCardMaster", Session["DealerCode"].ToString()))
                    {
                        string jobCardCOde = SysFunc.GetStringValuesAgainstCodes("EstimateCode", txtExtno.Text, "JobCardCode", "JobCardMaster", Session["DealerCode"].ToString());
                        SqlParameter[] dsParam = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                                 };

                        dsParam[0].Value = Session["DealerCode"].ToString();
                        dsParam[1].Value = jobCardCOde;

                        dsJobCardLub = new DataSet();
                        dsJobCardLub = SysFunc.FillDataSet("sp_W2_JobCard_LubricanteDetail_Select", dsParam);
                        if (dsJobCardLub.Tables[0].Rows.Count > 0)
                        {


                            foreach (DataRow dr in dsJobCardLub.Tables[0].Rows)
                            {
                                if (dr["ItemCode"].ToString() == ddllLubs.SelectedValue)
                                {
                                    lblLubIssuedQty.Text = "Issued Qty:" + dr["RecQty"].ToString();
                                    lblLubIssuedQty.Visible = true;
                                }
                            }
                        }
                    }
                }
                ViewState["deductAmount"] = row.Cells[9].Text;
                PartsDT = (DataTable)ViewState["Parts"];
                //PartsDT.Rows.RemoveAt(gvJobCardParts.SelectedIndex);
                //gvJobCardParts.DataSource = PartsDT;
                //PartsDT.AcceptChanges();
                //gvJobCardParts.DataBind();


            }
            catch (Exception ex) { throw ex; }
        }
    
        protected void ImgJobLookUp_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 21; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_Job(ImgJobLookUp, ViewState["lookupid"].ToString(), "", "../../../");

            ViewState["job"] = "Job";

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void lnkImgRemove_Click(object sender, ImageClickEventArgs e)
        {
             
            try
            {
                ImageDT = (DataTable)ViewState["Image"];
                ImageButton btn = sender as ImageButton;
                TableCell tc = btn.Parent as TableCell;
                GridViewRow gvr = tc.Parent as GridViewRow;
                ImageDT.Rows.RemoveAt(gvr.RowIndex);
                //Load grid 
                ImgGridView.DataSource = ImageDT;
                ImageDT.AcceptChanges();
                ImgGridView.DataBind();
               
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, ex.Message, txtCustomer);
            }

        }
    

        private void Insert_in_BoutDT(DataRow rowJobDT)
        {

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
            //ViewState["ObjPOds"] = ds;
            gvJobCardPartsBought.DataSource = ds.Tables["PartsDetailsDS"];
            gvJobCardPartsBought.DataBind();
           
            ViewState["BoutPartsDataTable"] = ds;
            //dtJobs = ds.Tables["WarrantyLaborTable"];
            Session["BoutPartsDataTables"] = ds.Tables["PartsDetailsDS"];
            ViewState["Bout"] = ds.Tables["PartsDetailsDS"];
        }
        protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void ddlJobs_SelectedIndexChanged(object sender, EventArgs e)
        {
      if(txtProduct.Text==""  || txtProduct.Text==null ) {

        SysFunc.UserMsg(lblMsg, Color.Red, "First Select the Vehicle ", txtProduct);
            }
            else {
                string sql = "Select Amount From LaborDetail where DefJobCode='" + ddlJobs.SelectedValue + "' and ProdCode='" + txtProduct.Text + "' and DealerCode in('COMON','" + Session["DealerCode"].ToString() + "') ";

                dt = SysFunc.GetData(sql);
                if (dt.Rows.Count > 0)
                {
                    txtLabor.Text = dt.Rows[0]["Amount"].ToString();
                }
               
            }
           
        }

        protected void gvJobCardPartsBought_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvJobCardPartsBought_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                totBoutInv = totBoutInv + double.Parse(e.Row.Cells[9].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[9].Text.Replace("&nbsp;", "").Trim());
                totPayableBout = totPayableBout + double.Parse(e.Row.Cells[7].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[7].Text.Replace("&nbsp;", "").Trim());
                if(lblTotalAMount.Text==null || lblTotalAMount.Text == "")
                {
                    lblTotalAMount.Text = "0";
                        
                        
                }
                totEst = double.Parse(e.Row.Cells[9].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[9].Text.Replace("&nbsp;", "").Trim()) + double.Parse(lblTotalAMount.Text);
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                totPayableBout = totBoutInv = 0;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                lblBoutPartCount.Text = Convert.ToString(totPayableBout);
                lblTotalInvAmt.Text = Convert.ToString(totBoutInv);
                lblTotalAMount.Text= Convert.ToString(totEst);
            }

        }

        protected void lnkBoutRemove_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                BoutDT = (DataTable)ViewState["Bout"];
                ImageButton btn = sender as ImageButton;
                TableCell tc = btn.Parent as TableCell;
                GridViewRow gvr = tc.Parent as GridViewRow;
                BoutDT.Rows.RemoveAt(gvr.RowIndex);
                //Load grid 
                gvJobCardPartsBought.DataSource = BoutDT;
                BoutDT.AcceptChanges();
                gvJobCardPartsBought.DataBind();
                if (BoutDT.Rows.Count == 0)
                {
                    Session["BoutPartsDataTables"] = BoutDT;
                    ViewState["Bout"] = BoutDT;
                    lblBoutPartCount.Text = "0";
                    lblTotalInvAmt.Text = "0";
                }
                CalculateTotal();
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, ex.Message, txtCustomer);
            }

        }

        protected void ImgBoughtOutPart_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 82; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_Parts(imgLookup, ViewState["lookupid"].ToString(), "", "../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void txtBQty_TextChanged(object sender, EventArgs e)
        {
            double amt;
            amt = double.Parse(txtBQty.Text) * double.Parse(txtBPrice.Text);



            amt = (double.Parse(txtBPrice.Text) * 25) / 100;
            amt = amt + double.Parse(txtBPrice.Text);
            txtBAmt.Text = amt.ToString();
        }

        protected void btnClearParts_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void lnkRemove_Click(object sender, ImageClickEventArgs e)
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
                CalculateTotal();
                if (SubletDT.Rows.Count == 0)
                {
                    TotalPaySublet.Text = "0";
                    TotalInvSublet.Text = "0";
                }
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, ex.Message, txtCustomer);
            }
        }

        protected void imgJobSubLookUp_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 21; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_Job(imgJobSubLookUp, ViewState["lookupid"].ToString(), "", "../../../");

            ViewState["job"] = "Sublet";

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }

        protected void ddlEstCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlEstCode.SelectedIndex==0)
            {
                return;
            }
            LoadMasterData();
        }
        private string MakeReport()
        {
            ReportDocument  RD;
            //string strCriteria, rptTitle;
            //DateTime FromDate, ToDate;
            //SqlDataReader rder;
            SysFunctions myFunc = new SysFunctions();
            Data.DSReports DsRpt = new Data.DSReports();
            DataSet1 dsRpt1 = new DataSet1();
            
            RD = new ReportDocument();
            string CCon = CConnection.GetConnectionString();
            DataSet ds = new DataSet();

            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_CustomerEstimateMaster_Print '" + this.Session["DealerCode"].ToString() + "','" + ddlEstCode.SelectedValue.ToString ().Trim() + "'");
            DsRpt.sp_CustomerEstimateMaster_Print.Load(ds.CreateDataReader());

            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_CustomerEstimateDetail_Print '" + this.Session["DealerCode"].ToString() + "','" + ddlEstCode.SelectedValue.ToString().Trim() + "'");
            DsRpt.sp_CustomerEstimateDetail_Print.Load(ds.CreateDataReader());

            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_CustomerEstimateLubricates_Print '" + this.Session["DealerCode"].ToString() + "','" + ddlEstCode.SelectedValue.ToString().Trim() + "'");
            DsRpt.sp_CustomerEstimateLubricates_Print.Load(ds.CreateDataReader());

            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_CustomerEstimateParts_Print '" + this.Session["DealerCode"].ToString() + "','" + ddlEstCode.SelectedValue.ToString().Trim() + "'");
            DsRpt.sp_CustomerEstimateParts_Print.Load(ds.CreateDataReader());

            //ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_JobCardSubletDetail_Print'" + this.Session["DealerCode"].ToString() + "','" + ddlJobCardNo.Text.Trim() + "'");
            //DSReports.sp_JobCardSubletDetail_Print.Load(ds.CreateDataReader());

            //string FileRptPath = Server.MapPath("~/Modules/Service/ServiceReports/");


            RD.Load(Server.MapPath("~/Modules/Service/ServiceReports/rptCustomerEstimatePrint.rpt"));
            RD.OpenSubreport(Server.MapPath("~/Modules/Service/ServiceReports/rptCustomerEstimateDetail.rpt"));
            RD.OpenSubreport(Server.MapPath("~/Modules/Service/ServiceReports/rptCustomerEctimatePartsDetail.rpt"));
            RD.OpenSubreport(Server.MapPath("~/Modules/Service/ServiceReports/rptCustomerEstimateLubDetail.rpt"));

            RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'CUSTOMER ESTIMATE'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            // CrystalReportViewer1.ReportSource = rpt;
            RD.SetDataSource(DsRpt.sp_CustomerEstimateMaster_Print.DataSet);

            Session["RDService"] = RD;

            // prepare pdf and show

            // create pdf 
            string FilePath = Server.MapPath("~/Download/");
            string FileName = "CustomerEstimate" + Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;
            //crReportDocument.SetDatabaseLogon("SDMS", "sdms161", "192.168.1.47", "SDMS");
            RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);




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

        protected void BtnRemove_Click(object sender, ImageClickEventArgs e)
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
                CalculateTotal();
                //if (JobDT.Rows.Count == 0)
                //{
                //    txtJobsTotal.Text = "0";
                //    lblJobCardTotal.Text = "0";
                //}
                //else
                //{
                //    txtJobsTotal.Text = JobDT.Rows.Count.ToString();

                //}

            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, ex.Message, txtItemCode);
            }
        }

        protected void BtnRemove_Click1(object sender, ImageClickEventArgs e)
        {
             try
             {
                 PartsDT = (DataTable)ViewState["Parts"];
                 ImageButton btn = sender as ImageButton;
                 TableCell tc = btn.Parent as TableCell;
                 GridViewRow gvr = tc.Parent as GridViewRow;
                if (txtExtno.Text != "")
                {
                    if (SysFunc.IsExist("EstimateCode", txtExtno.Text, "JobCardMaster", Session["DealerCode"].ToString()))
                    {
                        string jobCardCOde = SysFunc.GetStringValuesAgainstCodes("EstimateCode", txtExtno.Text, "JobCardCode", "JobCardMaster", Session["DealerCode"].ToString());
                        SqlParameter[] dsParam = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                                 };

                        dsParam[0].Value = Session["DealerCode"].ToString();
                        dsParam[1].Value = jobCardCOde;

                        dsJobCardParts = new DataSet();
                        dsJobCardParts = SysFunc.FillDataSet("sp_W2_JobCard_PartsDetail_Select", dsParam);
                        if (dsJobCardParts.Tables[0].Rows.Count > 0)
                        {


                            foreach (DataRow dr in dsJobCardParts.Tables[0].Rows)
                            {
                                if(gvr.Cells[2].Text== dr["ItemCode"].ToString())
                                {
                                    if (SysFunctions.CustomCDBL(dr["RecQty"].ToString()) > 0)
                                    {
                                        lblIssuedParts.Text = "Issued Qty:" + dr["RecQty"].ToString();
                                        SysFunc.UserMsg(lblMsg, Color.Red, "You Can Not Remove Part");
                                        return;
                                    }
                                }
                              
                            }
                        }
                    }
                }
                PartsDT.Rows.RemoveAt(gvr.RowIndex);
                 //Load grid 
                 gvJobCardParts.DataSource = PartsDT;
                 PartsDT.AcceptChanges();
                 gvJobCardParts.DataBind();
                 int sumParts = 0;
                CalculateTotal();
                 //if (PartsDT.Rows.Count <= 0)
                 //{
                 //    txtPartsTotal.Text = "0";
                 //    txtPartTotalQuantity.Text = "0";
                 //}
                 //else
                 //{
                 //    foreach (DataRow dr in PartsDT.Rows)
                 //    {
                 //        sumParts = sumParts + Convert.ToInt32(dr["Qty"].ToString());
                 //    }
                 //    txtPartTotalQuantity.Text = sumParts.ToString();
                 //}

             }
             catch (Exception ex)
             {
                 SysFunc.UserMsg(lblMsg, Color.Red, ex.Message, txtItemCode);
             }
        }

        protected void BtnRemove_Click2(object sender, ImageClickEventArgs e)
        {
            try
            {
                LubDT = (DataTable)ViewState["Lub"];
                ImageButton btn = sender as ImageButton;
                TableCell tc = btn.Parent as TableCell;
                GridViewRow gvr = tc.Parent as GridViewRow;
                if (txtExtno.Text != "")
                {
                    if (SysFunc.IsExist("EstimateCode", txtExtno.Text, "JobCardMaster", Session["DealerCode"].ToString()))
                    {
                        string jobCardCOde = SysFunc.GetStringValuesAgainstCodes("EstimateCode", txtExtno.Text, "JobCardCode", "JobCardMaster", Session["DealerCode"].ToString());
                        SqlParameter[] dsParam = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                                 };

                        dsParam[0].Value = Session["DealerCode"].ToString();
                        dsParam[1].Value = jobCardCOde;

                        dsJobCardParts = new DataSet();
                        dsJobCardParts = SysFunc.FillDataSet("sp_W2_JobCard_LubricanteDetail_Select", dsParam);
                        if (dsJobCardParts.Tables[0].Rows.Count > 0)
                        {


                            foreach (DataRow dr in dsJobCardParts.Tables[0].Rows)
                            {
                                if(gvr.Cells[2].Text== dr["ItemCode"].ToString())
                                if (SysFunctions.CustomCDBL(dr["RecQty"].ToString())>0)
                                {
                                    lblLubIssuedQty.Text = "Issued Qty:" + dr["RecQty"].ToString();
                                    SysFunc.UserMsg(lblMsg, Color.Red, "You Can Remove Lube is Issued ");
                                    return;
                                }
                            }
                        }
                    }
                }
                LubDT.Rows.RemoveAt(gvr.RowIndex);
                //Load grid 
                gvLubParts.DataSource = LubDT;
                LubDT.AcceptChanges();
                gvLubParts.DataBind();
                CalculateTotal();
                //if (LubDT.Rows.Count == 0)
                //{
                //    txtLubTotal.Text = "0";
                //    lbltotLubQty.Text = "0";
                //}
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, ex.Message, txtItemCode);
            }
        }

        protected void BtnAdd_Click1(object sender, ImageClickEventArgs e)
        {
            TextBox[] textBoxes = { txtLabor };
            if (ddlJobs.SelectedItem.Text == "--Select--")
            {
                SysFunc.UserMsg(lblMsg, Color.Red, "Please select Job first !!");
                return;
            }
            if (!MasterValidation(textBoxes)) { return; }
           
            else
            {

                search_result = false;
                foreach (DataRow dr in SubletDT.Rows)
                {
                 string   JobCode = dr["JobCode"].ToString();
                    if (JobCode == ddlJobs.SelectedValue)
                    {
                        lblMsg.Text = "Job Already exist in Sublet Tab";
                        lblMsg.ForeColor = Color.Red;
                        TabPanel1.Focus();
                        return;
                    }

                }
                foreach (DataRow rowJobDT in JobDT.Rows)
                {
                    if (rowJobDT["JobCode"].ToString().Trim() == ""
                        | rowJobDT["JobCode"].ToString().Trim() == txtJobCode.Text)
                    {
                        Insert_in_JobDT(rowJobDT); search_result = true;
                    }
                }
                if (search_result == false)
                {
                    DataRow rowJobDT = JobDT.NewRow(); Insert_in_JobDT(rowJobDT); JobDT.Rows.Add(rowJobDT);
                }
                ViewState["Job"] = JobDT; gvJobCard.DataSource = JobDT; gvJobCard.DataBind();

                //txtJobsTotal.Text = JobDT.Rows.Count.ToString();
                CalculateTotal();
                ClearJobTextBoxes();
            }
        }

        protected void BtnClear1_Click1(object sender, ImageClickEventArgs e)
        {
            ClearJobTextBoxes();
        }

        protected void BtnAdd0_Click(object sender, ImageClickEventArgs e)
        {
            string jobCardCOde;

            if (txtQuantity.Text.Trim() == "" || txtQuantity.Text.Trim() == "0")
            {
                string script = "alert(\"Quantity should not left blank  !!!\");";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", script, true);
                return;
            }
            if (txtExtno.Text != "")
            {
                if (SysFunc.IsExist("EstimateCode", txtExtno.Text, "JobCardMaster", Session["DealerCode"].ToString())){
                    jobCardCOde = SysFunc.GetStringValuesAgainstCodes("EstimateCode", txtExtno.Text, "JobCardCode", "JobCardMaster", Session["DealerCode"].ToString());
                    SqlParameter[] dsParam = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                                 };

                    dsParam[0].Value = Session["DealerCode"].ToString();
                    dsParam[1].Value = jobCardCOde;

                    dsJobCardParts = new DataSet();
                    dsJobCardParts = SysFunc.FillDataSet("sp_W2_JobCard_PartsDetail_Select", dsParam);
                    if (dsJobCardParts.Tables[0].Rows.Count > 0)
                    {


                        foreach (DataRow dr in dsJobCardParts.Tables[0].Rows)
                        {
                            if (dr["ItemCode"].ToString() == txtItemCode.Text)
                            {
                                if (SysFunctions.CustomCDBL(txtQuantity.Text) < SysFunctions.CustomCDBL(dr["RecQty"].ToString()))
                                {
                                    SysFunc.UserMsg(lblImp, Color.Red, "You can not enter quantity less then issued quantity:"+ dr["RecQty"].ToString());
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            //lblMsg.Visible = false;
            TextBox[] textBoxes = { txtPartPrice, txtQuantity };
            if (!MasterValidation(textBoxes)) return;
            else
            {
                foreach (DataRow dr in BoutDT.Rows)
                {
                   string itemdesc = dr["ItemCode"].ToString();
                    if (itemdesc == txtItemCode.Text)
                    {
                        lblMsg.Text = "Part Already exist in Bought Out Tab";
                        lblMsg.ForeColor = Color.Red;
                        TabPanel2.Focus();
                        return;
                    }

                }

                search_result = false;
                foreach (DataRow rowPartsDT in PartsDT.Rows)
                {
                    if (rowPartsDT["ItemCode"].ToString().Trim() == ""
                        | rowPartsDT["ItemCode"].ToString().Trim() == txtItemCode.Text.Trim())
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
                CalculateTotal();
                ClearPartsTextBoxes();
            }
        }

        protected void BtnClear2_Click(object sender, ImageClickEventArgs e)
        {
            ClearPartsTextBoxes();
        }

        protected void btnLookup_Click(object sender, EventArgs e)
        {
            
            ViewState["lookupid"] = 10; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_Parts(imgLookup, ViewState["lookupid"].ToString(), "", "../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            
            TextBox[] textBoxes = { txtLubPartCode, txtLubPrice, txtLubQuantity };
            if (!MasterValidation(textBoxes)) return;
            else
            {
                if (txtExtno.Text != "")
                {
                    if (SysFunc.IsExist("EstimateCode", txtExtno.Text, "JobCardMaster", Session["DealerCode"].ToString()))
                    {
                       string jobCardCOde = SysFunc.GetStringValuesAgainstCodes("EstimateCode", txtExtno.Text, "JobCardCode", "JobCardMaster", Session["DealerCode"].ToString());
                        SqlParameter[] dsParam = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                    new SqlParameter("@JobCardCode",SqlDbType.Char,8)
                                 };

                        dsParam[0].Value = Session["DealerCode"].ToString();
                        dsParam[1].Value = jobCardCOde;

                        dsJobCardParts = new DataSet();
                        dsJobCardParts = SysFunc.FillDataSet("sp_W2_JobCard_LubricanteDetail_Select", dsParam);
                        if (dsJobCardParts.Tables[0].Rows.Count > 0)
                        {


                            foreach (DataRow dr in dsJobCardParts.Tables[0].Rows)
                            {
                                if (dr["ItemCode"].ToString() == ddllLubs.SelectedValue)
                                {
                                    if (SysFunctions.CustomCDBL(txtLubQuantity.Text) < SysFunctions.CustomCDBL(dr["RecQty"].ToString()))
                                    {
                                        SysFunc.UserMsg(lblImp, Color.Red, "You can not enter quantity less then issued quantity: "+ dr["RecQty"].ToString());
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }

                if (txtLubPrice.Text == "0")
                {
                    lblMsg.Text = "Zero price is not allowed";
                    lblMsg.ForeColor=Color.Red;
                    txtLubPrice.Focus();
                    txtLubPrice.ForeColor = Color.Red;
                    return;
                }
                search_result = false;
                foreach (DataRow rowLubDT in LubDT.Rows)
                {
                    if (rowLubDT["ItemCode"].ToString().Trim() == ""
                        | rowLubDT["ItemCode"].ToString().Trim() == ddllLubs.SelectedValue)
                    {
                        Insert_in_LubPartsDT(rowLubDT); search_result = true;
                    }
                }
                if (search_result == false)
                {
                    DataRow rowLubDT = LubDT.NewRow(); Insert_in_LubPartsDT(rowLubDT); LubDT.Rows.Add(rowLubDT);
                }
                ViewState["Lub"] = LubDT; gvLubParts.DataSource = LubDT; gvLubParts.DataBind();
                CalculateTotal();
                ClearLubricantsTextBoxes();
            }
        }

        protected void OnClick_btnUpload(object sender, EventArgs e)
        {
            if (PicUpload.HasFile)
            {
                if (ImgGridView.Rows.Count >= 10)
                {
                    SysFunc.UserMsg(lblMsg, Color.Red, "Max 10 images are allowed", null);
                    lblImgAddress.Focus();
                    return;
                }

                HttpPostedFile postedFile = PicUpload.PostedFile;

                string imgName = PicUpload.FileName;
                string imgPath = "EstimateImage/" + imgName;
                FileInfo fi = new FileInfo(imgName);

                int imgSize = PicUpload.PostedFile.ContentLength;
                if (imgSize >= 1000000)
                {
                    SysFunc.UserMsg(lblMsg, Color.Red, "Images should be less or equal to 1MB ", null);
                    lblImgAddress.Focus();


                    return;
                }
                // imgcontent =PicUpload.FileBytes;
                if (fi.Exists)
                {
                    SysFunc.UserMsg(lblMsg, Color.Red, "Image name already exist please change name!  ", null);
                    lblImgAddress.Focus();


                    return;
                }

                string fileExtension = Path.GetExtension(imgName);

                if (fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".gif"
                    || fileExtension.ToLower() == ".png" || fileExtension.ToLower() == ".bmp")
                {
                    PicUpload.SaveAs(Server.MapPath("../../../" + imgPath));
                    Session["Sketch"] = imgPath;
                    p = Session["Sketch"].ToString();
                    lblImgAddress.Text = imgPath;
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
                    ImgGridView.DataSource = ImageDT; ImgGridView.DataBind();
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
                    lblMsg.Text = "Only images (.jpg, .png, .gif and .bmp) can be uploaded";
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
            rowImageDT["DocChkListCode"] = ddlDocType.SelectedValue;
            rowImageDT["DocChkListDesc"] = ddlDocType.SelectedItem.Text;
            //if (lblTotalAMount.Text != "")
            //{
            //    lblTotalAMount.Text = Convert.ToString(Convert.ToInt32(lblTotalAMount.Text) + Convert.ToInt32(double.Parse(txtQuantity.Text.Trim()) * double.Parse(txtPartPrice.Text.Trim())));
            //}
            //else
            //{
            //    lblTotalAMount.Text = Convert.ToString(Convert.ToInt32(double.Parse(txtQuantity.Text.Trim()) * double.Parse(txtPartPrice.Text.Trim())));
            //}

        }
        private bool Inser_EstimateImageDetail()
        {
            try
            {
                bool flag = false;
                // if (gvJobCardParts.Rows.Count > 0)
                //{
                SqlParameter[] JobCardPartsDetail_Delete_param = {
                                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    new SqlParameter("@EstimateCode",SqlDbType.Char,8)
                                                                 };
                JobCardPartsDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                //JobCardPartsDetail_Delete_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                JobCardPartsDetail_Delete_param[1].Value = CustomerEstimateCode.Trim();
                SqlParameter[] JobCardPartsDetail_Insert_param = {
                                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    new SqlParameter("@EstimateCode",SqlDbType.Char,8),
                                                                     new SqlParameter("@ImagePath",SqlDbType.VarChar,100),
                                                                     new SqlParameter("@ImageData",SqlDbType.VarBinary),
                                                                     new SqlParameter("@DocChkListCode",SqlDbType.Char,5),
                                                                     new SqlParameter("@DocChkListDesc",SqlDbType.VarChar,50),

                                                                 };

                if (SysFunc.ExecuteSP_NonQuery("sp_W2_Estimate_ImageDetail_Delete", JobCardPartsDetail_Delete_param, Trans))
                {
                    ImageDT = (DataTable)ViewState["Image"];
                    DataRow[] drr = ImageDT.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {
                        byte[] imgbyte = (byte[])ImageDT.Rows[i]["ImageData"];
                        //  MemoryStream ms = new MemoryStream(imgbyte);
                        JobCardPartsDetail_Insert_param[0].Value = Session["DealerCode"].ToString();
                        //JobCardPartsDetail_Insert_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                        JobCardPartsDetail_Insert_param[1].Value = CustomerEstimateCode.Trim();

                        JobCardPartsDetail_Insert_param[2].Value = ImageDT.Rows[i]["ImageAddress"].ToString();
                        JobCardPartsDetail_Insert_param[3].Value = imgbyte;
                        JobCardPartsDetail_Insert_param[4].Value = ImageDT.Rows[i]["DocChkListCode"].ToString();
                        JobCardPartsDetail_Insert_param[5].Value = ImageDT.Rows[i]["DocChkListDesc"].ToString();
                        //Item Code System Generted num. Get it from lookup
                        SysFunc.ExecuteSP_NonQuery("sp_W2_Estimate_ImageDetail_Insert", JobCardPartsDetail_Insert_param, Trans);
                        flag = true;

                    }
                }
                // }
                return flag;
            }
            catch (Exception ex) { throw ex; }

        }
    }
}