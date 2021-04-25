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


namespace DXBMS.Modules.Service.Forms
{
	public partial class Approval : System.Web.UI.Page
	{
		SysFunction sysFunc = new SysFunction();
		SysFunctions sysFuncs = new SysFunctions();
		Transaction ObjTrans = new Transaction();
		SqlTransaction Trans;
		DataTable dtParts;
		DataSet dsSublet, ds, dsParts;
		DataTable dtJobs;
		string StrMCDMainDBCon = "";

		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.Session["UserName"] == null)
			{
				Response.Redirect("~/login.aspx");

			}
			dtParts = new DataTable();
			dsSublet = new DataSet();
			dtJobs = new DataTable();
			txtPPRDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

			TabContainer1.ActiveTabIndex = 0;

			if (Page.IsPostBack == false)
			{
				LoadEmptyGrid();
				LoadEmptyGridJobs();

				LoadDLLs();
			}
			//if (Session["LookUpData"] != null)
			//{
			//	//lookUptext(ViewState["lookupid"].ToString());
			//	if (ViewState["lookupid"].ToString() == "44") ViewJobCardData();
			//	//if (ViewState["lookupid"].ToString() == "48") ViewPartsData();
			//	if (ViewState["lookupid"].ToString() == "54") ViewData();
			//}
		}

		private void LoadDLLs()
		{
			ddlRoadCond.Items.Add(new ListItem("Select", "Select"));
			ddlRoadCond.Items.Add(new ListItem("Paved", "Paved"));
			ddlRoadCond.Items.Add(new ListItem("City", "City"));
			ddlRoadCond.Items.Add(new ListItem("Rural", "Rural"));

			ddlAppSt.Items.Add(new ListItem("Select", "Select"));
			ddlAppSt.Items.Add(new ListItem("Approved", "Approved"));
			ddlAppSt.Items.Add(new ListItem("Reject", "Reject"));
			ddlAppSt.Items.Add(new ListItem("Return", "Return"));

			string Where = "1=1";

			string[] Columns1 = new string[] { "ComplainCode", "ComplainDesc" };
			sysFunc.GetMultiColumnsDDL(ddlComplains, Columns1, "ComplainType", Where, "ComplainCode", "", false, false);

			string[] Columns2 = new string[] { "DefectCode", "DefectDesc" };
			sysFunc.GetMultiColumnsDDL(ddlDefect, Columns2, "Defect", Where, "DefectCode", "", false, false);

			string WhereQuery = "DealerCode = '" + Session["DealerCode"] + "'";

			string[] Columns0 = new string[] { "PPRCode", "PPRDate", "UserName", "VehRegNo" };
			sysFunc.GetMultiColumnsDDL(ddlPRNo, Columns0, "PPRMaster", WhereQuery, "PPRCode", "", false, false);

			string[] Columns = new string[] { "VendorCode", "VendorDesc" };
			sysFunc.GetMultiColumnsDDL(ddlVendor, Columns, "Vendor", WhereQuery, "VendorCode", "", false, false);

			

			//string where = "JobCardType = '008'";

			//string[] Columns3 = new string[] { "JobCardCode", "UserName", "RegNo" };
			//sysFunc.GetMultiColumnsDDL(ddljob_code, Columns3, "JobCardMaster", where, "JobCardCode", "", false, false);
		}

		public void ViewData()
		{
			DataSet ds = new DataSet();
			DataTable dt = new DataTable();

			string sql = "exec SP_Select_PPRMaster  '" + Session["DealerCode"].ToString() + "','" + ddlPRNo.SelectedValue + "'";

			dt = sysFunc.GetData(sql);

			txtJobCode.Text = dt.Rows[0]["JobCardCode"].ToString();
			txtJobCardDate.Text = dt.Rows[0]["PPRDate"].ToString(); //Convert.ToDateTime(sysFuncs.GetStringValuesAgainstCodes("JobCardCode", ddljob_code.SelectedValue, "RecDate", "JobCardMaster")).ToString("dd/MM/yyyy");
			txtPPRTotalAmount.Text = dt.Rows[0]["TotalAmount"].ToString();
			txtRegNo.Text = dt.Rows[0]["VehRegNo"].ToString();
			txtChasisNo.Text = dt.Rows[0]["VehChassisNo"].ToString();
			txtEngNo.Text = dt.Rows[0]["VehEngineNo"].ToString();
			txtVehModel.Text = Convert.ToDateTime(dt.Rows[0]["InvoiceDate"].ToString()).ToString("yyyy");
			txtInvoiceNo.Text = dt.Rows[0]["InvoiceNo"].ToString();
			txtInvDate.Text = Convert.ToDateTime(dt.Rows[0]["InvoiceDate"].ToString()).ToString("dd/MM/yyyy");
			//txtDealerCode.Text = dt.Rows[0]["SaleDealerCode"].ToString();
			//txtDealerName.Text = dt.Rows[0]["SaleDealerName"].ToString();
			txtProdCode.Text = dt.Rows[0]["ProdCode"].ToString();
			txtVehColor.Text = dt.Rows[0]["Color"].ToString();
			txtKM.Text = dt.Rows[0]["MeterReadingKM"].ToString();

			if(dt.Rows[0]["CompletionDate"].ToString() != "")
			dtComplitionDate.Text = sysFunc.GetDate(dt.Rows[0]["CompletionDate"]); //Convert.ToDateTime(Convert.ToDateTime(dt.Rows[0]["CompletionDate"].ToString()).ToString("dd/MM/yyyy").Substring(6, 4) + "/" + Convert.ToDateTime(dt.Rows[0]["CompletionDate"].ToString()).ToString("dd/MM/yyyy").Substring(3, 2) + "/" + Convert.ToDateTime(dt.Rows[0]["CompletionDate"].ToString()).ToString("dd/MM/yyyy").Substring(0, 2));   //Convert.ToDateTime(Convert.ToDateTime(gvr.Cells[4].Text).ToString("yyyy-MM-dd"));

			dtProblemDate.Text = sysFunc.GetDate(dt.Rows[0]["DateOfProblem"]);//Convert.ToDateTime(Convert.ToDateTime(dt.Rows[0]["DateOfProblem"].ToString()).ToString("dd/MM/yyyy").Substring(6, 4) + "/" + Convert.ToDateTime(dt.Rows[0]["DateOfProblem"].ToString()).ToString("dd/MM/yyyy").Substring(3, 2) + "/" + Convert.ToDateTime(dt.Rows[0]["DateOfProblem"].ToString()).ToString("dd/MM/yyyy").Substring(0, 2));   //Convert.ToDateTime(Convert.ToDateTime(gvr.Cells[4].Text).ToString("yyyy-MM-dd"));
			txtCustCode.Text = dt.Rows[0]["CusCode"].ToString();
			txtCustName.Text = dt.Rows[0]["UserName"].ToString();
			txtUserName.Text = dt.Rows[0]["UserName"].ToString().Trim();
			txtUserAddress.Text = dt.Rows[0]["UserAddress1"].ToString();
			txtPhoneNo.Text = dt.Rows[0]["UserPhoneNo"].ToString();
			ddlRoadCond.SelectedValue = dt.Rows[0]["RoadCondition"].ToString().Trim();
			txtAprovalDate.Text = dt.Rows[0]["ApprovalDate"].ToString();
			txtAprovalNo.Text = dt.Rows[0]["ApprovalNo"].ToString();
			txtComplainRemarks.Text = dt.Rows[0]["ComplainRemarks"].ToString();
			txtTRDate.Text = dt.Rows[0]["TRDate"].ToString();
			txtTRNo.Text = dt.Rows[0]["TRNo"].ToString();
			if (dt.Rows[0]["ApprovalStatus"].ToString() == "")
			{
				ddlAppSt.SelectedIndex = 0;
			}
			else
			{
				ddlAppSt.SelectedValue = dt.Rows[0]["ApprovalStatus"].ToString().Trim();
			}



			DataSet dsPPRDetail = new DataSet();
			SqlParameter[] param = {
											new SqlParameter("@DealerCode",SqlDbType. Char,5),
											new SqlParameter("@PPRCode",SqlDbType. Char,8),
									   };
			param[0].Value = Session["DealerCode"].ToString();
			param[1].Value = ddlPRNo.SelectedValue;


			dsPPRDetail = sysFuncs.FillDataSet("sp_2W_Service_Select_PPRDetail", param);

			ddlComplains.SelectedValue = dsPPRDetail.Tables[0].Rows[0]["ComplainCode"].ToString();
			txtCompletionDesc.Text = dsPPRDetail.Tables[0].Rows[0]["ComplaintDesc"].ToString();
			ddlDefect.SelectedValue = dsPPRDetail.Tables[0].Rows[0]["DefectCode"].ToString();
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
			param1[1].Value = ddlPRNo.SelectedValue;

			sql = "SELECT VendorCode FROM PPRPartsDetail WHERE PPRCode = '" + ddlPRNo.SelectedValue + "' AND DealerCode = '" + Session["DealerCode"].ToString() + "'";

			dt = sysFunc.GetData(sql);

			if (dt.Rows.Count > 0)
			{
				ddlVendor.SelectedValue = dt.Rows[0]["VendorCode"].ToString().Trim();
			}

			dsParts = sysFuncs.FillDataSet("sp_2W_Service_Select_PPRPartsDetail", param1);

			if (dsParts.Tables[0].Rows.Count > 0)
			{
				LoadGrid();
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
			param[1].Value = txtJobCode.Text;
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

		private void LoadEmptyGridJobs()
		{
			try
			{
				dtJobs.Columns.Add("Sno");
				dtJobs.Columns.Add("LaborCode");
				dtJobs.Columns.Add("LaborDesc");
				dtJobs.Columns.Add("Rate");
				ViewState["JobsDataTable"] = dtJobs;
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
			ds.Tables[0].Columns.Add(new DataColumn("Qty", typeof(string)));
			ds.Tables[0].Columns.Add(new DataColumn("Price", typeof(string)));
			ds.Tables[0].Columns.Add(new DataColumn("GrossAmount", typeof(string)));

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

			ViewState["PartsDataTable"] = dsParts;

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
				if (sysFuncs.IsPosted("PPRMaster", "PPRCode", ddlPRNo.SelectedValue, string.Empty))
				{
					sysFuncs.UserMsg(lblMsg, Color.Red, "Can not edit PPR already posted", null);
					return;
				}
				//if (ddlPRNo.SelectedValue != "__/_____" && ddlPRNo.SelectedValue != "")
				//{
				//	if (sysFuncs.IsExist("PPRCode", ddlPRNo.SelectedValue, "PPRMaster", string.Empty) == false)
				//	{
				//                 lblMsg.Text = "Record dose not exist";
				//		//sysFuncs.UserMsg(lblMsg, Color.Red, "Record dose not exist", ddlPRNo);
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
						//dtParts = (DataTable)ViewState["PartsDataTable"];
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
				}
				else
				{
					ObjTrans.RollBackTransaction(ref Trans);
				}
				if (Chek == true)
				{
					ObjTrans.CommittTransaction(ref Trans);
					lblMsg.Visible = true;
					sysFuncs.UserMsg(lblMsg, Color.Green, "Saved Successfully Last Code '" + ddlPRNo.SelectedValue + "'");
					ClearAll();
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


								};
			param[0].Value = Session["DealerCode"].ToString();
			if (ddlPRNo.SelectedValue == "0" || ddlPRNo.SelectedValue.ToString().Trim() == "__/_____")
			{
				param[1].Value = sysFuncs.AutoGen("PPRMaster", "PPRCode", DateTime.Now.ToString("dd-MM-yyyy"));
				//txtPPRNo.Text = param[1].Value.ToString();
				ViewState["PPRCode"] = param[1].Value.ToString();
			}
			else
			{
				param[1].Value = ddlPRNo.SelectedValue;
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
			param[24].Value = txtPPRTotalAmount.Text;
			param[25].Value = "";
			param[26].Value = "";
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
									  new SqlParameter("@ComplainCode",SqlDbType.Char,2),//2
									  new SqlParameter("@DefectCode",SqlDbType.Char,2),//3
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
			param[5].Value = txtLaborAmount.Text;
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
									   
								};

			param[0].Value = Session["DealerCode"].ToString();
			param[1].Value = ViewState["PPRCode"].ToString();
			param[2].Value = dtRow["PartNo"].ToString().Trim();
			param[3].Value = dtRow["Qty"].ToString().Trim();
			param[4].Value = ddlVendor.SelectedValue;
			param[5].Value = dtRow["Price"].ToString().Trim();
			param[6].Value = 0;
			param[7].Value = dtRow["GrossAmount"].ToString().Trim();
			param[8].Value = "".Trim();
			param[9].Value = "";



			if (sysFuncs.ExecuteSP_NonQuery("sp_2W_Service_Insert_PPRPartsDetail", param, Trans))
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

			Data.DSReports data = new Data.DSReports();
			ReportDocument RD = new ReportDocument();

			SqlParameter[] param =
		{
			new SqlParameter("@DealerCode",SqlDbType.Char),//0
			new SqlParameter("@PPRCOde",SqlDbType.Char), //1
			
		};

			param[0].Value = Session["DealerCode"].ToString();
			param[1].Value = ddlPRNo.SelectedValue;


			SqlDataReader rder = null;

			SysFunction sysFunc = new SysFunction();
			if (sysFunc.ExecuteSP("SP_PPR_Report", param, ref rder))
			{
				data.SP_PPR_Report.Load(rder);

			}
			RD.Load(Server.MapPath("~\\Modules\\Service\\ServiceReports\\TRReport.rpt"));



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
			Session["RD"] = RD;
			//RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

			string URL;
			URL = "../../../Download/rptViewerService.aspx?FileName=" + FileName;
			string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
			ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
	
		}

		protected void btnPost_Click(object sender, EventArgs e)
		{
			try
			{
				if (ddlPRNo.SelectedValue.ToString().Trim() == "")
				{
					sysFuncs.UserMsg(lblMsg, Color.Red, "Please Enter Valid PPR No", null);
					return;
				}
				if (sysFuncs.IsPosted("PPRMaster", "PPRCode", ddlPRNo.SelectedValue, string.Empty))
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
				param[1].Value = ddlPRNo.SelectedValue;
				param[2].Value = txtJobCardNo.Text.Trim();
				if (sysFuncs.ExecuteSP_NonQuery("sp_2W_Service_PostPPR", param))
				{
					sysFuncs.UserMsg(lblMsg, Color.Green, "Posted Successfully ", null);
					ClearAll();
				}
			}
			catch (Exception ex)
			{
				lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
				lblMsg.Text = ex.Message;
				//SendAlert("Error: " + ex.Message);
			}
		}


		protected void btnDelete_Click(object sender, EventArgs e)
		{
			//SqlTransaction Trans = null;
			if (ddlAppSt.SelectedValue == "Approved")
			{
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = "Cannot Delete Approved Status";
				return;
			}

			bool Chek = false;
			try
			{
				//TextBox[] textBoxes = { txtPPRNo };
				//if (!MasterValidation(textBoxes))
				//{
				//	return;
				//}
				if (sysFuncs.IsPosted("PPRMaster", "PPRCode", ddlPRNo.SelectedValue, string.Empty))
				{
					sysFuncs.UserMsg(lblMsg, Color.Red, "Can not Delete PPR is posted", null);
					return;
				}
				//if (ObjGenral.IsDelFlag("PPRMaster", "PPRCode", txtPPRNo.Text, string.Empty) == false)
				//{
				//    ObjGenral.UserMsg(lblMsg, Color.Red, "Record dose not exist", null);
				//    return;
				//}
				if (ddlPRNo.SelectedValue != "__/_____")
				{
					if (sysFuncs.IsExist("PPRCode", ddlPRNo.SelectedValue, "PPRMaster", string.Empty) == false)
					{
						lblMsg.Text = "Record dose not exist";
						//sysFuncs.UserMsg(lblMsg, Color.Red, "Record dose not exist", ddlPRNo.SelectedValue);
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
				param[1].Value = ddlPRNo.SelectedValue;
				if (sysFuncs.ExecuteSP_NonQuery("sp_2W_Service_DeletePPR", param))
				{

					sysFuncs.UserMsg(lblMsg, Color.Green, "Deleted Successfully", null);
					ClearAll();
				}
			}
			catch (Exception ex)
			{
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;
				//SendAlert("Error: " + ex.Message);
			}
		}

		protected void btnClear_Click(object sender, EventArgs e)
		{
			ClearAll();
			lblMsg.Text = string.Empty;
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
			LoadGrid();
			LoadEmptyGrid();

		}
		public void ClearMasterDetail()
		{
			//txtPPRNo.Text = string.Empty;
			txtPPRDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
			txtJobCode.Text = string.Empty;//ddljob_code.SelectedIndex = 0;
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
			ddlPRNo.SelectedIndex = 0;
		}


		private void cleadata()
		{
			sysFunc.Clearddl(Page);
			sysFunc.ClearTextBoxes(Page);
			LoadDLLs();
		}

		protected void grdParts_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Alternate || e.Row.RowState == DataControlRowState.Normal))
			{

			}
		}

		//protected void ddljob_code_SelectedIndexChanged(object sender, EventArgs e)
		//{
		//    if (ddljob_code.SelectedIndex == 0)
		//    {
		//        cleadata();
		//        return;
		//    }

		//    btnWarSave.Enabled = false;            

		//    DataSet ds = new DataSet();
		//    SqlParameter[] param = {
		//    new SqlParameter("@DealerCode",SqlDbType. Char,5),
		//    new SqlParameter("@JobCardCode",SqlDbType. Char,8),
		//                               };
		//    param[0].Value = Session["DealerCode"].ToString();
		//    param[1].Value = ddljob_code.SelectedValue;
		//    ds = sysFuncs.FillDataSet("sp_2W_Service_Select_JobCardMaster", param);
		//    if (ds.Tables[0].Rows.Count > 0)
		//    {
		//        txtJobCardNo.Text = ddljob_code.SelectedValue;
		//        txtJobCardDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["RecDate"].ToString()).ToString();

		//        txtPPRTotalAmount.Text = Convert.ToString(Convert.ToDouble(ds.Tables[0].Rows[0]["JobsTotal"].ToString()) +
		//                         Convert.ToDouble(ds.Tables[0].Rows[0]["PartsTotal"].ToString()) +
		//                         Convert.ToDouble(ds.Tables[0].Rows[0]["LubPartsTotal"].ToString()) +
		//                         Convert.ToDouble(ds.Tables[0].Rows[0]["SubletTotal"].ToString()));
		//        txtRegNo.Text = ds.Tables[0].Rows[0]["RegNo"].ToString();
		//        txtChasisNo.Text = ds.Tables[0].Rows[0]["ChassisNo"].ToString();
		//        txtEngNo.Text = ds.Tables[0].Rows[0]["EngineNo"].ToString();
		//        txtVehModel.Text = ds.Tables[0].Rows[0]["InvoiceDate"].ToString().Substring(6, 4);
		//        txtVehColor.Text = ds.Tables[0].Rows[0]["ColorDesc"].ToString();
		//        txtInvDate.Text = ds.Tables[0].Rows[0]["InvoiceDate"].ToString();
		//        txtCustCode.Text = ds.Tables[0].Rows[0]["CusCode"].ToString();
		//        txtProdCode.Text = ds.Tables[0].Rows[0]["ProdCode"].ToString();
		//        txtInvoiceNo.Text = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();
		//        txtKM.Text = ds.Tables[0].Rows[0]["KM"].ToString();
		//        txtCustName.Text = sysFuncs.GetStringValuesAgainstCodes("CusCode", txtCustCode.Text, "CusDesc", "Customer");

		//        txtUserName.Text = sysFuncs.GetStringValuesAgainstCodes("CusCode", txtCustCode.Text, "CusDesc", "Customer");
		//        txtUserAddress.Text = sysFuncs.GetStringValuesAgainstCodes("CusCode", txtCustCode.Text, "Address1", "Customer");
		//        txtPhoneNo.Text = sysFuncs.GetStringValuesAgainstCodes("CusCode", txtCustCode.Text, "Phone1", "Customer");

		//        FillJobCardDetailGrid();

		//    }


		//}

		//private void FillJobCardDetailGrid()
		//{
		//    DataSet dsPartsDetail;

		//    SqlParameter[] dsParam = {  new SqlParameter("@DealerCode",SqlDbType.Char,5),
		//                            new SqlParameter("@JobCardCode",SqlDbType.Char,8)};
		//    dsParam[0].Value = Session["DealerCode"].ToString();
		//    dsParam[1].Value = ddljob_code.SelectedValue;

		//    dsPartsDetail = new DataSet(); dsPartsDetail = sysFuncs.FillDataSet("SP_PartsDetail", dsParam);
		//    if (dsPartsDetail.Tables[0].Rows.Count == 0) dsPartsDetail.Tables[0].Rows.Add(dsPartsDetail.Tables[0].NewRow());
		//    grdParts.DataSource = dsPartsDetail; grdParts.DataBind();

		//    Session["PartsDetailsDS"] = dsPartsDetail.Tables[0];

		//}

		protected void ddlPRNo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ddlPRNo.SelectedIndex == 0)
			{
				cleadata();
				//Create_Grid();
				return;
			}

			btnWarSave.Enabled = true;

			ViewData();
		}
		protected void btnWarSave_Click(object sender, EventArgs e)
		{

			DateTime TRDate = DateTime.ParseExact(txtTRDate.Text, "dd-MM-yyyy", null);
			DateTime ApprovalDate = DateTime.ParseExact(txtAprovalDate.Text, "dd-MM-yyyy", null);

			try
			{

				if (ObjTrans.BeginTransaction(ref Trans) == true)
				{

					string sql = "update PPRMaster set TRNo = '" + txtTRNo.Text + "' , TRDate = '" + TRDate + "' , ApprovalNo = '" + txtAprovalNo.Text + "' , ApprovalDate = '" + ApprovalDate + "', ApprovalStatus = '" + ddlAppSt.SelectedValue.ToString().Trim() + "' , ComplainRemarks = '" + txtComplainRemarks.Text + "' where DealerCode = '" + Session["DealerCode"].ToString() + "' And PPRCode = '" + ddlPRNo.SelectedValue.Trim() + "'";


					sysFuncs.ExecuteQuery(sql, Trans);
				}
				ObjTrans.CommittTransaction(ref Trans);

			}
			catch (Exception ex)
			{

				ObjTrans.RollBackTransaction(ref Trans);
				
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;
			}
			ClearWarrantyTab();

		}

		protected void BtnRemove_Labor_Click(object sender, ImageClickEventArgs e)
		{
			DataSet dsdelete = new DataSet();

			DataTable dtDelete = new DataTable();

			try
			{
				dsdelete = (DataSet)ViewState["PartsDataTable"];

				ImageButton BtnLnk2 = (ImageButton)sender;
				GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
				int rowGvIndex = rowGv.RowIndex;
				if (dsdelete.Tables[0].Rows.Count == 1)
				{
					lblMsg.Text = "There Should be atleast 1 item";
					return;
				}
				else
				{
					dsdelete.Tables[0].Rows[rowGvIndex].Delete();
					dsdelete.Tables[0].AcceptChanges();
				}
				

				grdParts.DataSource = dsdelete;
				grdParts.DataBind();

				ViewState["PartsDataTable"] = dsdelete;
			}
			catch (Exception ex)
			{
				lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;
			}
		}

		protected void btnWarClear_Click(object sender, EventArgs e)
		{
			ClearWarrantyTab();
		}

		private void ClearWarrantyTab()
		{
			txtAprovalDate.Text = string.Empty;
			txtAprovalNo.Text = string.Empty;
			txtComplainRemarks.Text = string.Empty;
			txtTRDate.Text = string.Empty;
			txtTRNo.Text = string.Empty;
		}

	}
}