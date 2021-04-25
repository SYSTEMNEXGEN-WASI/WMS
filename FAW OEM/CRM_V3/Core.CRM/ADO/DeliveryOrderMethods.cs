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
	public class DeliveryOrderMethods
	{
		static bool IsSaved = false;
		static bool IsDeleted = false;
		static SqlParameter[] nullSqlParam = null;
		static DataTable dt = new DataTable();
		static string strAutoCode = string.Empty;
		static SysFunction sysfun = new SysFunction();
		static DateTime recDate = new DateTime();

		public static List<SelectListItem> GetDONo(string dealerCode)
		{
			List<SelectListItem> item = new List<SelectListItem>();

			List<StringNameValueClass> lst = new List<StringNameValueClass>();

			try
			{
				//var Serializer = new JavaScriptSerializer();
				SqlParameter[] sqlParam =
				{
					new SqlParameter("@DealerCode",dealerCode)
				};
				dt = DataAccess.getDataTable("SP_SelectDONo", sqlParam, General.GetBMSConString());
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

		public static string Get_DelChkList()
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<DeliveryCheckListVM> lst = new List<DeliveryCheckListVM>();
			try
			{
				string sql = "Select A.DelChkListCode , A.DelChkListDesc , A.OptFlag from DelChkList A";

				dt = sysfun.GetData(sql);

				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<DeliveryCheckListVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;

		}


        public static List<SelectListItem> GetDealerEmployee(string DealerCode)
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",DealerCode)
                };
                dt = DataAccess.getDataTable("Select_DealerEmplpoyee", sqlParam, General.GetBMSConString());
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

        public static string Get_DeliveryChkList(string enquiryId)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<DeliveryCheckListVM> lst = new List<DeliveryCheckListVM>();
            try
            {
                string sql = "Select DelChkListCode from VehicleDelChkList where DeliveryNo = '" + enquiryId + "'";

                dt = sysfun.GetData(sql);

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<DeliveryCheckListVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;

        }


        public static List<SelectListItem> GetChassisNo(string vehType , string dealerCode)
		{
			List<SelectListItem> item = new List<SelectListItem>();
			DataTable dt = new DataTable();
			List<StringNameValueClass> lst = new List<StringNameValueClass>();
			try
			{
				//var Serializer = new JavaScriptSerializer();
				SqlParameter[] sqlParam =
				{
					new SqlParameter("@DealerCode",dealerCode),
					new SqlParameter("@VehType",vehType)
				};
				dt = DataAccess.getDataTable("SP_SelectChassisNo", sqlParam, General.GetBMSConString());
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

        public static bool Insert_DOMaster(VehicleDeliveryMasterVM model, string dealerCode)
        {

            try
            {
                if (model.DeliveryNo == "0")
                {
                    strAutoCode = sysfun.AutoGen("VehicleDeliveryMaster", "DeliveryNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), dealerCode);
                    //strAutoCode = sysfun.GetNewMaxID("ProdRecMaster", "RecNo", 8, "00166");
                    //recDate = DateTime.Parse( model.RecDate);
                }
                else
                {

                    strAutoCode = model.DeliveryNo;
                    string rec = model.DeliveryDate;
                    recDate = DateTime.Parse(rec);

                }
                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",dealerCode),//0
								 new SqlParameter("@DeliveryNo",strAutoCode),//1
								 new SqlParameter("@DeliveryDate",model.DeliveryDate),//2								 
								 new SqlParameter("@BookRefNo",model.BookRefNo),//3								 
								 new SqlParameter("@UpdUser",AuthBase.UserId),//4
								 new SqlParameter("@UpdTerm",General.CurrentIP),//5
                                 new SqlParameter("@Type",model.Type),//6
                                 new SqlParameter("@ReceiverCode",model.ReceiverCode),//7
                                 new SqlParameter("@Remarks",model.Remarks),//8
                                 new SqlParameter("@Segment",model.Segment),//9
                                 new SqlParameter("@Usage",model.Usage),//10
                                 new SqlParameter("@CusCode",model.CusCode),//11
                                 new SqlParameter("@VehTypeCode",model.VehTypeCode),//12
                                 new SqlParameter("@CusContNo",model.CusContNo),//13
								 new SqlParameter("@EmpCode",model.EmpCode)
                            };

                if (sysfun.ExecuteSP_NonQuery("SP_VehicleDeliveryMaster_Insert", param))
                {

                    IsSaved = true;

                }

            }
            catch (Exception)
            {

                throw;
            }

            return IsSaved;
        }

        public static bool Insert_DODetail(VehicleDeliveryDetailVM model2, string dealerCode)
        {

            try
            {
                SqlParameter[] param2 = {
                                 new SqlParameter("@DealerCode",dealerCode),//0
								 new SqlParameter("@DeliveryNo",strAutoCode),//1
								 new SqlParameter("@BrandCode",model2.BrandCode),//2								 
								 new SqlParameter("@ProdCode",model2.ProdCode.Trim()),//3								 
								 new SqlParameter("@VersionCode",model2.VersionCode.Trim()),//4
								 new SqlParameter("@ColorCode",model2.ColorCode.Trim()),//5
								 new SqlParameter("@EngineNo",model2.EngineNo.Trim()),//6
								 new SqlParameter("@ChasisNo",model2.ChasisNo.Trim()),//7
                                 new SqlParameter("@ProdRecNo",model2.ProdRecNo.Trim()),//8
								 new SqlParameter("@LocCode",model2.LocCode),//9
                                 new SqlParameter("@SaleType",model2.StockType),//10
                                 new SqlParameter("@BookingNo",model2.BookingNo),//11
                                 new SqlParameter("@DocNo",model2.DocumentNo)//12
								 
							};

                if (sysfun.ExecuteSP_NonQuery("SP_VehicleDeliveryDetail_Insert", param2))
                {
                    IsSaved = true;
                }

            }
            catch (Exception)
            {

                throw;
            }

            return IsSaved;
        }


        public static string Get_DeliveryOrderData(string enquiryId, string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<RequestDeliveryOrderVM> lst = new List<RequestDeliveryOrderVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@DONo",enquiryId)//0
									
									};

                dt = DataAccess.getDataTable("SP_Select_DeliveryOrder", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<RequestDeliveryOrderVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        public static bool Insert_VehChkList(string strCheckedValues, string dealerCode)
        {

            try
            {
                SqlParameter[] param2 = {
                                 new SqlParameter("@DealerCode",dealerCode),//0
								 new SqlParameter("@Delivery",strAutoCode),//1
								 new SqlParameter("@DelChkListCode",strCheckedValues)//2								 
								 
							};

                if (sysfun.ExecuteSP_NonQuery("SP_VehicleChkList_Insert", param2))
                {
                    IsSaved = true;
                }

            }
            catch (Exception)
            {

                throw;
            }

            return IsSaved;
        }

        public static bool Delete_DeliveryOrder_Record(string enquiryId, string dealerCode)
        {
            DataSet ds = new DataSet();

            SqlParameter[] param = {
                new SqlParameter("@DealerCode",dealerCode),
                new SqlParameter("@DONo",enquiryId)
            };
           
                if (sysfun.ExecuteSP_NonQuery("sp_DeliveryOrder_Delete", param))
                {
                    IsDeleted = true;
                }
                else
                {
                    IsDeleted = false;
                }
            

            return IsDeleted;
        }

        public static string GetProRecDetail(string chassisNo, string dealerCode)
		{
			string json = "";
			List<SelectListItem> item = new List<SelectListItem>();
		   
			var Serializer = new JavaScriptSerializer();
			List<JobCardHistoryVM> lst = new List<JobCardHistoryVM>();
			try
			{
				//var Serializer = new JavaScriptSerializer();
				SqlParameter[] sqlParam =
				{
					new SqlParameter("@DealerCode",dealerCode),
					new SqlParameter("@RegNo",chassisNo)
				};
				dt = DataAccess.getDataTable("sp_2W_LoadChassisEngineRegNo_RegNoParameter_History", sqlParam, General.GetBMSConString());

				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<JobCardHistoryVM>(dt);
				}
				json = Serializer.Serialize(lst);

			}
			catch (Exception ex)
			{

				//throw;
			}
			return json;
		}
	}
}
