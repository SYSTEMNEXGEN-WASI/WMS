using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Core.CRM.ADO
{
	public class MasterMethods
	{
		static SysFunction sysfun = new SysFunction();
		static DataTable dt = new DataTable();
		
		static string strAutoCode = string.Empty;
		static string autoProspect_ID = string.Empty;
		static bool IsSaved = false;
		static SqlParameter[] nullSqlParam = null;

		public static string Get_VehicleTypeData()
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<VehicleTypeVM> lst = new List<VehicleTypeVM>();
			try
			{
				string sql = "Select V.VehTypeCode , V.VehTypeDesc from VehicleType V";

				dt = sysfun.GetData(sql);

				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<VehicleTypeVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;

		}

		public static string Get_VehRecTypeData()
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<VehReceiptTypoVM> lst = new List<VehReceiptTypoVM>();
			try
			{
				string sql = "Select A.VehRecCode , A.VehRecDesc from VehicleReceiptType A";

				dt = sysfun.GetData(sql);

				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<VehReceiptTypoVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;

		}

		public static string Get_DocumentTypeData()
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<DocumentCheckList> lst = new List<DocumentCheckList>();
			try
			{
				string sql = "Select A.DocChkListCode , A.DocChkListDesc from DocumentCheckList A";

				dt = sysfun.GetData(sql);

				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<DocumentCheckList>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;

		}

		public static string Get_LocationDetail()
		{
			string json = "";
			var Serializer = new JavaScriptSerializer();
			List<VehicleLocationVM> lst = new List<VehicleLocationVM>();
			try
			{
				string sql = "Select V.VehLocCode , V.VehLocDesc from VehicleLocation V";

				dt = sysfun.GetData(sql);

				if (dt.Rows.Count > 0)
				{
					lst = EnumerableExtension.ToList<VehicleLocationVM>(dt);
				}
				json = Serializer.Serialize(lst);
			}
			catch (Exception ex)
			{

				throw;
			}

			return json;

		}

		public static bool Insert_VehicleType(VehicleTypeVM model)
		{

			try
			{
				if (string.IsNullOrEmpty(model.VehTypeCode))
				{
					strAutoCode = sysfun.GetNewMaxID("VehicleType", "VehTypeCode", 5, "COMON");
				}
				else
				{
					strAutoCode = model.VehTypeCode;
				}
				

				SqlParameter[] param = {
								 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@VehTypeCode",strAutoCode),//1
								 new SqlParameter("@VehTypeDesc",model.VehTypeDesc),//2		 
								 new SqlParameter("@UpdUser",AuthBase.UserId),//3
								 new SqlParameter("@UpdTerm",General.CurrentIP)//4
								 
							};
				dt = DataAccess.getDataTable("Sp_Insert_VehicleType", param, General.GetBMSConString());
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

		public static bool Insert_VehReceiptType(VehReceiptTypoVM model)
		{

			try
			{

				if (string.IsNullOrEmpty(model.VehRecCode))
				{
					
					strAutoCode = sysfun.GetNewMaxID("VehicleReceiptType", "VehRecCode", 5, model.DealerCode);
				}
				else
				{
					strAutoCode = model.VehRecCode;
				}
				

				SqlParameter[] param = {
								 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@VehRecCode",strAutoCode),//1
								 new SqlParameter("@VehRecDesc",model.VehRecDesc),//2		 
								 new SqlParameter("@UpdUser",AuthBase.UserId),//3
								 new SqlParameter("@UpdTerm",General.CurrentIP)//4
								 
							};
				dt = DataAccess.getDataTable("Sp_Insert_VehicleReceiptType", param, General.GetBMSConString());
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

		public static bool Insert_DocumentType(DocumentCheckList model)
		{

			try
			{

				if (string.IsNullOrEmpty(model.DocChkListCode))
				{

					strAutoCode = sysfun.GetNewMaxID("DocumentCheckList", "DocChkListCode", 5, model.DealerCode);
				}
				else
				{
					strAutoCode = model.DocChkListCode;
				}
				

				SqlParameter[] param = {
								 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@DocChkListCode",strAutoCode),//1
								 new SqlParameter("@DocChkListDesc",model.DocChkListDesc),//2		 
								 new SqlParameter("@UpdUser",AuthBase.UserId),//3
								 new SqlParameter("@UpdTerm",General.CurrentIP)//4
								 
							};
				dt = DataAccess.getDataTable("Sp_Insert_DocumentCheckList", param, General.GetBMSConString());
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

		public static bool Insert_VehLocation(VehicleLocationVM model)
		{

			try
			{

				if (string.IsNullOrEmpty(model.VehLocCode))
				{

					strAutoCode = sysfun.GetNewMaxID("VehicleLocation", "VehLocCode", 5, model.DealerCode);
				}
				else
				{
					strAutoCode = model.VehLocCode;
				}
				

				SqlParameter[] param = {
								 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@VehLocCode",strAutoCode),//1
								 new SqlParameter("@VehLocDesc",model.VehLocDesc),//2		 
								 new SqlParameter("@UpdUser",AuthBase.UserId),//3
								 new SqlParameter("@UpdTerm",General.CurrentIP)//4
								 
							};
				dt = DataAccess.getDataTable("Sp_Insert_VehicleLocation", param, General.GetBMSConString());
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
	}
}
