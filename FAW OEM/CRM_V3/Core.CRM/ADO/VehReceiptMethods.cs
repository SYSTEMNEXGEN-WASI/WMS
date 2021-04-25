using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Core.CRM.ADO
{
	public class VehReceiptMethods
	{
		static bool IsSaved = false;
		static bool IsDeleted = false;
		static SqlParameter[] nullSqlParam = null;
		static DataTable dt = new DataTable();
		static string strAutoCode = string.Empty;
		static SysFunction sysfun = new SysFunction();
		static DateTime recDate = new DateTime();



		public static List<SelectListItem> GetRecNo(string dealerCode)
		{
			List<SelectListItem> item = new List<SelectListItem>();
			
			List<StringNameValueClass> lst = new List<StringNameValueClass>();

			//string dealerCode = "00166";
			try
			{
				//var Serializer = new JavaScriptSerializer();
				SqlParameter[] sqlParam =
				{
					new SqlParameter("@DealerCode",dealerCode)
				};
				dt = DataAccess.getDataTable("SP_SelectRecNo", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
				}
				//json = Serializer.Serialize(lst);

				item = lst.Select(i => new SelectListItem()
				{
					Value = i.Id.ToString(),
					Text = i.Title
				}).ToList();

				item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

			}
			catch (Exception ex)
			{

				//throw;
			}
			return item;
		}

		public static List<SelectListItem> GetCustomers()
		{
			List<SelectListItem> item = new List<SelectListItem>();
			DataTable dt = new DataTable();
			string dealerCode = "00001";
			List<StringNameValueClass> lst = new List<StringNameValueClass>();
			try
			{
				//var Serializer = new JavaScriptSerializer();
				SqlParameter[] sqlParam =
				{
					new SqlParameter("@DealerCode",dealerCode)
				};
				dt = DataAccess.getDataTable("SP_Select_Customer", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
				}
				//json = Serializer.Serialize(lst);

				item = lst.Select(i => new SelectListItem()
				{
					Value = i.Id.ToString(),
					Text = i.Title
				}).ToList();

				item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

			}
			catch (Exception ex)
			{

				//throw;
			}
			return item;
		}

        public static List<SelectListItem> GetVehLocation( string dealerCode)
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_Select_VehLocation", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static bool Insert_ProdMaster(ProdReceiptVM model , string dealerCode)
		{
			
			try
			{
				if (model.RecNo=="0")
				{
                    strAutoCode = sysfun.AutoGen("ProdRecMaster", "RecNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"),dealerCode);
                    //strAutoCode = sysfun.GetNewMaxID("ProdRecMaster", "RecNo", 8, "00166");
					//recDate = DateTime.Parse( model.RecDate);
				}
				else
				{

					strAutoCode = model.RecNo;
					string rec = model.RecDate;
					recDate = DateTime.Parse(rec);

				}
				SqlParameter[] param = {
								 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@RecNo",strAutoCode),//1
								 new SqlParameter("@RecDate",model.RecDate),//2								 
								 new SqlParameter("@DocumentNo",model.DocumentNo),//3								 
								 new SqlParameter("@UpdUser",AuthBase.UserId),//4
								 new SqlParameter("@UpdTerm",General.CurrentIP),//5
								 new SqlParameter("@VehTypeCode",model.VehTypeCode),//6
								 new SqlParameter("@VehRecCode",model.VehRecCode),//7
								 new SqlParameter("@Segment",model.Segment.Trim()),//8
								 new SqlParameter("@Category",model.Category.Trim()),//9
								 new SqlParameter("@Usage",model.Usage.Trim()),//10
                                 new SqlParameter("@VendorCode",model.VendorCode.Trim())//10
								 
							};

				if (sysfun.ExecuteSP_NonQuery("SP_ProdRecMaster_Insert", param))
				{

						IsSaved = true;

				}
				//dt = DataAccess.getDataTable("SP_ProdRecMaster_Insert", param, General.GetBMSConString());
				//if (dt.Rows.Count > 0)
				//{

				//}
			}
			catch (Exception)
			{

				throw;
			}

			return IsSaved;
		}

		public static string Get_ProdRecMasterData(string enquiryId, string dealerCode)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<ProdReceiptVM> lst = new List<ProdReceiptVM>();
			try
			{
				SqlParameter[] sqlParam = {
									new SqlParameter("@EnquiryId",enquiryId),//0
									new SqlParameter("@DealerCode",dealerCode)//1
									};

				dt = DataAccess.getDataTable("SP_Select_ProdRecMaster", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<ProdReceiptVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;
		}

		public static bool Insert_ProdDetail(ProdReceiptDetailVM model2)
		{

			try
			{

				SqlParameter[] param2 = {
								 new SqlParameter("@DealerCode",model2.DealerCode),//0
								 new SqlParameter("@RecNo",strAutoCode),//1
								 new SqlParameter("@BrandCode",model2.BrandCode),//2								 
								 new SqlParameter("@ProdCode",model2.ProdCode.Trim()),//3								 
								 new SqlParameter("@VersionCode",model2.VersionCode.Trim()),//4
								 new SqlParameter("@ColorCode",model2.ColorCode.Trim()),//5
								 new SqlParameter("@EngineNo",model2.EngineNo.Trim()),//5
								 new SqlParameter("@ChasisNo",model2.ChasisNo.Trim()),//6
								 new SqlParameter("@StockType",model2.StockType.Trim()),
								 new SqlParameter("@InvoiceNo",model2.InvoiceNo.Trim()),//7
								 new SqlParameter("@InvoiceDate",model2.InvoiceDate),//8
								 new SqlParameter("@RegNo",model2.RegNo.Trim()),
								 new SqlParameter("@RegDate",model2.RegDate.Trim()),//9
								 new SqlParameter("@Insurance",model2.Insurance),//10
								 new SqlParameter("@Milage",model2.Milage),//11
								 new SqlParameter("@CusCode",model2.CusCode),//12
								 new SqlParameter("@BookingNo",model2.BookingNo),//13
								 new SqlParameter("@LocCode",model2.LocCode)//14
								 
							};

					if (sysfun.ExecuteSP_NonQuery("SP_ProdRecDetail_Insert", param2))
					{
						IsSaved = true;
					}

				//dt = DataAccess.getDataTable("SP_ProdRecMaster_Insert", param, General.GetBMSConString());
				//if (dt.Rows.Count > 0)
				//{

				//}
			}
			catch (Exception)
			{

				throw;
			}

			return IsSaved;
		}

		public static string Get_VehicleReceiptData(string enquiryId, string dealerCode)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<RequestVehicleReceiptVM> lst = new List<RequestVehicleReceiptVM>();
			try
			{
				SqlParameter[] sqlParam = {
									new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@RecNo",enquiryId)//0
									
									};

				dt = DataAccess.getDataTable("SP_VehicleReceipt_Select", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<RequestVehicleReceiptVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;
		}

		public static string Get_VehicleReceiptDetailData(string enquiryId, string dealerCode)
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<RequestVehicleReceiptVM> lst = new List<RequestVehicleReceiptVM>();
			try
			{
				SqlParameter[] sqlParam = {
									new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@RecNo",enquiryId)//0
									
									};

				dt = DataAccess.getDataTable("SP_Select_ProdRecDetail", sqlParam, General.GetBMSConString());
				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<RequestVehicleReceiptVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;
		}

		public static bool Delete_VehicleReceipt_Record(string enquiryId, string dealerCode)
		{
            DataSet ds = new DataSet();

            SqlParameter[] param = {
                new SqlParameter("@DealerCode",dealerCode),
            };
            if (sysfun.CodeExists("VehicleStock", "DeliveryFlag", "Y", "00166", ref ds))
            {
                return IsDeleted;
                
            }else
            {
                if (sysfun.ExecuteSP_NonQuery("SP_DeleteDealerDetails", param))
                {
                    IsDeleted = true;
                }
                else
                {
                    IsDeleted = false;
                }
            }

			return IsDeleted;
		}

        public static bool Delete_VehicleReceiptDetail_Record(string enquiryId, string dealerCode)
        {
            DataSet ds = new DataSet();

            SqlParameter[] param = {
                new SqlParameter("@DealerCode",dealerCode),
                new SqlParameter("@RecNo",enquiryId)
            };
            if (sysfun.CodeExists("VehicleStock", "DeliveryFlag", "Y", "00166", ref ds))
            {
                return IsDeleted;

            }
            else
            {
                if (sysfun.ExecuteSP_NonQuery("sp_VehicleReceiptDetail_Delete", param))
                {
                    IsDeleted = true;
                }
                else
                {
                    IsDeleted = false;
                }
            }

            return IsDeleted;
        }

    }
}
