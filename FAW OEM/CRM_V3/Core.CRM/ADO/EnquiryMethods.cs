using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Core.CRM.ADO
{
	public class EnquiryMethods
	{
        static SysFunction sysFunc = new SysFunction();
		static EnquiryMasterVM response = new EnquiryMasterVM();
		static DataTable dt = new DataTable();
		static string strAutoCode = string.Empty;
		static string autoProspect_ID = string.Empty;
		static bool IsSaved = false;
		static  SqlParameter[] nullSqlParam = null;

		public static string Insert_EnquiryMaster(EnquiryMasterVM model)
		{
			string leadid = "";
			string prospectid = "";
			if(model.Enquiry_ID == "" || model.Enquiry_ID == "0")
			{
				if (model.Interest == "New")
				{
					strAutoCode = "NE/";
				}
				else
				{
					strAutoCode = "US/";
				}
				string getNextEnquiryID = "declare @lastval varchar(14),@id int " +
										   "set @id = (select count(*) from CRM_EnquiryMaster) " +
										   "set @id=@id+1 " +
										   "if len(@id) = 1 " +
										   "set @lastval='" + strAutoCode + "'+cast(YEAR(getDate()) as varchar(10)) +'/'+'00000' " +
										   "if len(@id) = 2 " +
										   "set @lastval='" + strAutoCode + "'+cast(YEAR(getDate()) as varchar(10)) +'/'+'0000' " +
										   "if len(@id) = 3 " +
										   "set @lastval='" + strAutoCode + "'+cast(YEAR(getDate()) as varchar(10)) +'/'+'000' " +
										   "if len(@id) >= 4 " +
										   "set @lastval='" + strAutoCode + "'+cast(YEAR(getDate()) as varchar(10)) +'/'+'00' " +
										   "if len(@id) >= 5 " +
										   "set @lastval='" + strAutoCode + "'+cast(YEAR(getDate()) as varchar(10)) +'/'+'0' " +
										   "declare @i varchar(14) " +
										   "set @i = CAST(@id as varchar(14)) " +
										   "set @lastval = @lastval+@i " +
										   "select @lastval as enquiryID";

				dt = DataAccess.getDataTableByQuery(getNextEnquiryID, nullSqlParam, General.GetBMSConString());

				strAutoCode = dt.Rows[0]["enquiryID"].ToString();
			
			}
			else
			{
				strAutoCode = model.Enquiry_ID;
			}
		   
			if(model.ProspectID == "" || model.ProspectID == "0")
			{

                //string max = sysFunc.GetNewMaxID("DealerCategory", "DealerCatCode", 3, Session["DealerCode"].ToString());

			}
			else
			{
				autoProspect_ID = model.ProspectID;
			}

			try
			{
				//var Serializer = new JavaScriptSerializer();
				DateTime leadDate = DateTime.ParseExact(model.EnqDate,"MM/dd/yyyy", CultureInfo.InvariantCulture);
				SqlParameter[] sqlParam = 
				{
					new SqlParameter("@DealerCode",model.DealerCode),//0
					new SqlParameter("@Enquiry_ID",strAutoCode),//1
					new SqlParameter("@EnqDate",leadDate),//2
					new SqlParameter("@Interest",model.Interest),//3
					new SqlParameter("@ProspectID",autoProspect_ID),//4
					new SqlParameter("@EmpCode",model.EmpCode),//5
					new SqlParameter("@ProspectTypeID",model.ProspectTypeID),//6
					new SqlParameter("@CompanyDetail",model.CompanyDetail),//7
					new SqlParameter("@EnquiryMode",model.EnquiryModeID),//8
					new SqlParameter("@EnquirySource",model.EnquirySourceID),//9
					new SqlParameter("@Event_Campaign",model.Event),//10
					new SqlParameter("@EnquiryStatus",(object)DBNull.Value),//11
					new SqlParameter("@TestDriveGiven",(object)DBNull.Value),//12
					new SqlParameter("@CashFinanced",model.CashFinanced),//13
					new SqlParameter("@IsFinanced",model.IsFinanced),//14
					new SqlParameter("@FinancedThrough",(object)DBNull.Value),//15
					new SqlParameter("@FinancedDetail",(object)DBNull.Value),//16
					new SqlParameter("@FinancedBank",model.FinancedBank),//17
					new SqlParameter("@InsuranceThrough",(object)DBNull.Value),//18
					new SqlParameter("@InsuranceDetail",(object)DBNull.Value),//19
					new SqlParameter("@ProspectRequist",(object)DBNull.Value),//20
					new SqlParameter("@Remarks",model.Remarks),//21
					new SqlParameter("@NextFollowupDate",(object)DBNull.Value),//22
					new SqlParameter("@NextFollowupTime",(object)DBNull.Value),//23
					new SqlParameter("@ActionPlan",(object)DBNull.Value),//24
					new SqlParameter("@Purpose",(object)DBNull.Value),//25
					new SqlParameter("@LikelyPurchaseDate",(object)DBNull.Value),//26
					new SqlParameter("@CreatedBy",model.CreatedBy),//27
					new SqlParameter("@RefBy",(object)DBNull.Value),//28
					new SqlParameter("@RegMobile",(object)DBNull.Value),//29
					new SqlParameter("@FinanceAppliedDate",(object)DBNull.Value),//30
					new SqlParameter("@FinanceApprovedDate",(object)DBNull.Value),//31
					new SqlParameter("@ProspectLost",(object)DBNull.Value),//32
					new SqlParameter("@LostReason",(object)DBNull.Value),//33
					new SqlParameter("@LostByDealer",(object)DBNull.Value),//34
					new SqlParameter("@LostByModel",(object)DBNull.Value),//35
					new SqlParameter("@IsDeleted",model.IsDeleted),//36
					new SqlParameter("@IsMatured",model.IsMatured),//37
					new SqlParameter("@TransferStatus",(object)DBNull.Value),//38
					new SqlParameter("@EnquiryType",model.EnquiryTypeID),//39
					new SqlParameter("@Gridstate",false),//40
					new SqlParameter("@PMatured",(object)DBNull.Value),//41
					new SqlParameter("@PartiallyLost",false),//42
					new SqlParameter("@UpdUser",AuthBase.UserId),//43
					new SqlParameter("@UpdTerm",General.CurrentIP),//44
					new SqlParameter("@SoftwareVersion",(object)DBNull.Value),//45
					new SqlParameter("@Blocked",false),//46
					new SqlParameter("@Campaign",model.Campaign),//47
					new SqlParameter("@Replacement",(object)DBNull.Value),//48
					new SqlParameter("@Addtional",(object)DBNull.Value),//49
					new SqlParameter("@ExChange",(object)DBNull.Value),//50
					new SqlParameter("@Mobile",model.Mobile),//51
					new SqlParameter("@InvoiceDetail",model.InvoiceDetail),//52
					new SqlParameter("@VehicleSegments",model.SegmentID)//53
				};
				dt = DataAccess.getDataTable("SP_Insert_EnquiryMaster", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{
					
				}
				leadid = strAutoCode + "," + autoProspect_ID;
				IsSaved = true;
			}
			catch (Exception ex)
			{

				//throw;
			}
			return leadid;
		}

		public static bool Insert_ProspectDetail(ProspectDetailVM model)
		{

			try
			{
				DateTime dob = DateTime.ParseExact(model.DOB, "MM/dd/yyyy", CultureInfo.InvariantCulture);

				SqlParameter[] sqlParam = {
									 new SqlParameter("@ProspectID",autoProspect_ID),//0
									 new SqlParameter("@DealerCode",model.DealerCode),//1
									 new SqlParameter("@ProspectTitle",(object)DBNull.Value),//2
									 new SqlParameter("@Name",model.Name),//3
									 new SqlParameter("@FatherName",model.FatherName),//4
									 new SqlParameter("@ProspectType",model.ProspectType),//5
									 new SqlParameter("@NIC",(object)DBNull.Value),//6
									 new SqlParameter("@NTN",model.NTN),//7
									 new SqlParameter("@ResAddress",model.ResAddress),//8
									 new SqlParameter("@ResCityCode",model.ResCityCode),//9
									 new SqlParameter("@ResPhone",model.ResPhone),//10
									 new SqlParameter("@Mobile",model.Mobile),//11
									 new SqlParameter("@OfficeAddress",model.OfficeAddress),//12
									 new SqlParameter("@OffCityCode",model.OffCityCode),//13
									 new SqlParameter("@OffPhone",model.OffPhone),//14
									 new SqlParameter("@Fax",(object)DBNull.Value),//15
									 new SqlParameter("@Gender",(object)DBNull.Value),//16
									 new SqlParameter("@DOB",dob),//17
									 new SqlParameter("@WeddingAnniversary",(object)DBNull.Value),//18
									 new SqlParameter("@Profession",model.Profession),//19
									 new SqlParameter("@Designation",model.Designation),//20
									 new SqlParameter("@Hobbies",(object)DBNull.Value),//21
									 new SqlParameter("@Remarks",(object)DBNull.Value),//22
									 new SqlParameter("@Education",(object)DBNull.Value),//23
									 new SqlParameter("@Income",(object)DBNull.Value),//24
									 new SqlParameter("@Email",model.Email),//25
									 new SqlParameter("@Createdby",model.Createdby),//26
									 new SqlParameter("@IsDeleted",false),//27
									 new SqlParameter("@CusCode",(object)DBNull.Value),//28
									 new SqlParameter("@CompanyDetail",model.CompanyDetail),//29
									 new SqlParameter("@ContactPerson",model.ContactPerson),//30
									 new SqlParameter("@UpdUser",AuthBase.UserId),//31
									 new SqlParameter("@UpdTerm",General.CurrentIP),//32
									};

				dt = DataAccess.getDataTable("SP_Insert_Prospect", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{

				}

				IsSaved = true;
			}
			catch (Exception)
			{
				
				throw;
			}
			return IsSaved;
		}

		public static bool Insert_EnquiryDetail(EnquiryDetailVM model)
		{

			try
			{
				DateTime furhterdate = DateTime.ParseExact(model.FurtherDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
				SqlParameter[] sqlParam = {
									new SqlParameter("@Enquiry_ID",strAutoCode),//0
									new SqlParameter("@BrandCode",model.BrandCode),//1
									new SqlParameter("@ProdCode",model.ProdCode),//2
									new SqlParameter("@VersionCode",model.VersionCode),//3
									new SqlParameter("@ColorCode",model.ColorCode),//4
									new SqlParameter("@Qty",model.Qty),//5
									new SqlParameter("@RequiredDate",(object)DBNull.Value),//6
									new SqlParameter("@Remarks",model.Remarks),//7
									new SqlParameter("@Blocked",false),//8
									new SqlParameter("@BlockedDate",(object)DBNull.Value),//9
									new SqlParameter("@BlockedBy",(object)DBNull.Value),//10
									new SqlParameter("@PrimaryModel",(object)DBNull.Value),//11
									new SqlParameter("@StatusCode",model.StatusCode),//12
									new SqlParameter("@IsDeleted",false),//13
									new SqlParameter("@DealerCode",model.DealerCode),//14
									new SqlParameter("@FurtherContact",model.FurtherContact),//15
									new SqlParameter("@FurtherDate",furhterdate)//16
											
									};

				dt = DataAccess.getDataTable("SP_Insert_EnquiryDetail", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{

				}

				IsSaved = true;
			}
			catch (Exception)
			{

				throw;
			}
			return IsSaved;
		}

		public static bool Insert_QuickEnquiry(QuickEnquiryVM model)
		{

			try
			{
				strAutoCode = "NE/";

				string getNextEnquiryID = "declare @lastval varchar(14),@id int " +
												   "set @id = (select count(*) from CRM_EnquiryMaster) " +
												   "set @id=@id+1 " +
												   "if len(@id) = 1 " +
												   "set @lastval='" + strAutoCode + "'+cast(YEAR(getDate()) as varchar(10)) +'/'+'00000' " +
												   "if len(@id) = 2 " +
												   "set @lastval='" + strAutoCode + "'+cast(YEAR(getDate()) as varchar(10)) +'/'+'0000' " +
												   "if len(@id) = 3 " +
												   "set @lastval='" + strAutoCode + "'+cast(YEAR(getDate()) as varchar(10)) +'/'+'000' " +
												   "if len(@id) >= 4 " +
												   "set @lastval='" + strAutoCode + "'+cast(YEAR(getDate()) as varchar(10)) +'/'+'00' " +
												   "if len(@id) >= 5 " +
												   "set @lastval='" + strAutoCode + "'+cast(YEAR(getDate()) as varchar(10)) +'/'+'0' " +
												   "declare @i varchar(14) " +
												   "set @i = CAST(@id as varchar(14)) " +
												   "set @lastval = @lastval+@i " +
												   "select @lastval as enquiryID";

				dt = DataAccess.getDataTableByQuery(getNextEnquiryID, nullSqlParam, General.GetBMSConString());

				strAutoCode = dt.Rows[0]["enquiryID"].ToString();

				string getNewProsID = "declare @lastval varchar(19),@id int " +
											  "set @id = (select count(*) from CRM_Prospect) " +
											  "set @id=@id+1 " +
											  "if len(@id) = 1 " +
											  "set @lastval='CUST_ID/'+cast(YEAR(getDate()) as varchar(10)) +'/'+'00000' " +
											  "if len(@id) = 2 " +
											  "set @lastval='CUST_ID/'+cast(YEAR(getDate()) as varchar(10)) +'/'+'0000' " +
											  "if len(@id) = 3 " +
											  "set @lastval='CUST_ID/'+cast(YEAR(getDate()) as varchar(10)) +'/'+'000' " +
											  "if len(@id) >= 4 " +
											  "set @lastval='CUST_ID/'+cast(YEAR(getDate()) as varchar(10)) +'/'+'00' " +
											  "if len(@id) >= 5 " +
											  "set @lastval='CUST_ID/'+cast(YEAR(getDate()) as varchar(10)) +'/'+'0' " +
											  "declare @i varchar(19) " +
											  "set @i = CAST(@id as varchar(19)) " +
											  "set @lastval = @lastval+@i " +
											  "select @lastval as prospectID";

				dt = DataAccess.getDataTableByQuery(getNewProsID, nullSqlParam, General.GetBMSConString());
				autoProspect_ID = dt.Rows[0]["prospectID"].ToString();

				SqlParameter[] param = {
								 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@Enquiry_ID",strAutoCode),//1
								 new SqlParameter("@EnqDate",General.CurrentDate),//2
								 new SqlParameter("@ProspectID",autoProspect_ID),//3
								 new SqlParameter("@EmpCode",model.EmpCode),//4
								 new SqlParameter("@CashFinanced",model.CashFinanced),//5
								 new SqlParameter("@IsFinanced",model.IsFinanced),//6
								 new SqlParameter("@CreatedBy",model.CreatedBy),//7
								 new SqlParameter("@UpdUser",AuthBase.UserId),//8
								 new SqlParameter("@UpdTerm",General.CurrentIP),//9
								 new SqlParameter("@BrandCode",model.BrandCode),//10
								 new SqlParameter("@ProdCode",model.ProdCode),//11
								 new SqlParameter("@ColorCode",model.ColorCode),//12
								 new SqlParameter("@Qty",model.Qty),//13
								 new SqlParameter("@Name",model.Name),//14
								 new SqlParameter("@Mobile",model.Mobile)//15
							};
				dt = DataAccess.getDataTable("Insert_QuickEnquiryMaster", param, General.GetBMSConString());
				if(dt.Rows.Count > 0)
				{

				}
				IsSaved = true;
			}
			catch (Exception)
			{
				
				throw;
			}

			return IsSaved;
		}

		public static string Get_EnquiryMasterData(string enquiryId, string dealerCode)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<RequestEnquiryMasterVM> lst = new List<RequestEnquiryMasterVM>();
			try
			{
				SqlParameter[] sqlParam = {
									new SqlParameter("@EnquiryId",enquiryId),//0
									new SqlParameter("@DealerCode",dealerCode)//1
									};

				dt = DataAccess.getDataTable("Select_EnquiryMaster", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<RequestEnquiryMasterVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{
				
				throw;
			}

			return json;
		}

		public static string Get_EnquiryDetailData(string enquiryId, string dealerCode)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<RequestEnquiryDetailVM> lst = new List<RequestEnquiryDetailVM>();
			try
			{
				SqlParameter[] sqlParam = {
									new SqlParameter("@EnquiryId",enquiryId),//0
									new SqlParameter("@DealerCode",dealerCode)//1
									};

				dt = DataAccess.getDataTable("Select_EnquiryDetail", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<RequestEnquiryDetailVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;

		}

		public static bool Insert_FollowupDetail(FollowUpDetailVM model)
		{
			
			try
			{
				DateTime furhterdate = DateTime.ParseExact(model.NextFollowUpDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
				DateTime furhtertime = Convert.ToDateTime(model.NextFollowUpTime);
				string query = "declare @lastval varchar(50),@id int " +
								"set @id = (select count(*) from CRM_FollowUpDetail) " +
								"set @id=@id+1 " +
								"if len(@id) = 1 " +
								"set @lastval= '000000' " + 
								"if len(@id) = 2 " +
								"set @lastval= '00000' " +
								"if len(@id) = 3 " +
								"set @lastval= '0000' " + 
								"if len(@id) >= 4 " +
								"set @lastval= '000' " + 
								"if len(@id) >= 5  " +
								"set @lastval= '00' " +
								"if len(@id) >= 6  " +
								"set @lastval= '0' " +
								"declare @i varchar(50) " +
								"set @i = Convert(varchar(50),@id) " +
								"set @lastval = @lastval+@i " +
								"select @lastval as FollowupId";
				dt = DataAccess.getDataTableByQuery(query, nullSqlParam, General.GetBMSConString());
				model.FollowupId = dt.Rows[0]["FollowupId"].ToString();
				SqlParameter[] sqlParam = {
									new SqlParameter("@FollowupId",model.FollowupId),//0
									new SqlParameter("@DealerCode",model.DealerCode),//1
									new SqlParameter("@EnquiryID",model.EnquiryID),//2
									new SqlParameter("@PlanFollowupDate",(object)DBNull.Value),// 3
									new SqlParameter("@PlanFollowupTime",(object)DBNull.Value),// 4
									new SqlParameter("@PlanFollowupAction",(object)DBNull.Value),// 5
									new SqlParameter("@PlanFollowupPurpose",(object)DBNull.Value),// 6
									new SqlParameter("@PlanFollowupAssignTo",(object)DBNull.Value),// 7
									new SqlParameter("@Schedule",(object)DBNull.Value),// 8
									new SqlParameter("@ActualFollowupDate",(object)DBNull.Value),// 9
									new SqlParameter("@ActualFollowupTime",(object)DBNull.Value),// 10
									new SqlParameter("@ActualFollowupAction", (object)DBNull.Value), //11
									new SqlParameter("@ActualFollowupBy",(object)DBNull.Value), //12
									new SqlParameter("@AfterFollowupEnqStatus",(object)DBNull.Value),// 13
									new SqlParameter("@ActualFollowupRemarks",(object)DBNull.Value),// 14
									new SqlParameter("@IsLostEnq",model.IsLostEnq),//15
									new SqlParameter("@LostByDealer",(object)DBNull.Value), //16
									new SqlParameter("@LostByModel", (object)DBNull.Value), //17
									new SqlParameter("@LostReasonId", model.LostReasonId),//)18
									new SqlParameter("@NextFollowUpDate",furhterdate),//19
									new SqlParameter("@NextFollowUpTime",furhtertime.ToString("HH:mm")),//20
									new SqlParameter("@NextFollowUpActionPlan",model.NextFollowUpActionPlan),//21
									new SqlParameter("@NextFollowUpPurpose",model.NextFollowUpPurpose),//22
									new SqlParameter("@CreatedBy",General.GetCurrentUsername()),//23
									new SqlParameter("@IsDeleted", false), //24
									new SqlParameter("@TransferStatus", (object)DBNull.Value),//25
									new SqlParameter("@PartiallyLost", false),//26
									new SqlParameter("@LostDate", model.LostDate),//27
									new SqlParameter("@EmpCode", AuthBase.EmpCode),//28     
									new SqlParameter("@StatusTypeId",model.StatusTypeId)//29
									};

				dt = DataAccess.getDataTable("SP_Insert_CRM_FollowUpDetail", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{

				}

				IsSaved = true;
			}
			catch (Exception)
			{

			}
			return IsSaved;
		}

		public static string Get_FollowUpDetailData(string enquiryId)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<RequestFollowUpDetailVM> lst = new List<RequestFollowUpDetailVM>();
			try
			{
				SqlParameter[] sqlParam = {
									new SqlParameter("@EnquiryId",enquiryId),//0
									new SqlParameter("@EmpCode",AuthBase.EmpCode)//1
									};

				dt = DataAccess.getDataTable("Select_FollowUpDetail", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<RequestFollowUpDetailVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;

		}


        public static string Get_AlertFollowUpDetails()
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<EnquiryDetailVM> lst = new List<EnquiryDetailVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                               

                                        };

                dt = DataAccess.getDataTable("SP_SelectAlertFollowUpDetails", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<EnquiryDetailVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        public static string Get_AlertEnquiryDetails()
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<EnquiryDetailVM> lst = new List<EnquiryDetailVM>();
            try
            {
                SqlParameter[] sqlParam = {
                  

                                        };

                dt = DataAccess.getDataTable("SP_SelectAlertEnquiryDetails", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<EnquiryDetailVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
    }
}
