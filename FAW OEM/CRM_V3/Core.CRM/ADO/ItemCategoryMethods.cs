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
    public class ItemCategoryMethods
    {
        static OEMDealerVM response = new OEMDealerVM();
        static bool IsDeleted = false;
        static DataTable dt = new DataTable();
        static string max = string.Empty;
        static bool IsSaved = false;
        static SysFunction sysfun = new SysFunction();
        static SqlParameter[] nullSqlParam = null;
        static string strAutoCode = string.Empty;
        //abdussaboor //Get_ItemMaster
        public static string Get_ItemCategory(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<ItemCategoryVM> lst = new List<ItemCategoryVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_ItemCategoryOEM", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ItemCategoryVM>(dt);
                }
                 
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        //abdussaboor //Insert Insert_ItemCategory
        public static bool Insert_ItemCategory(ItemCategoryVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                if (model.ItemCatCode == "" || model.ItemCatCode == null)
                {
                    max = sysfun.GetNewMaxIDWitOutDealerCode("ItemCategory", "ItemCatCode", 4, "");
                }
                else
                {

                    max = model.ItemCatCode;

                }
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@ItemCatCode",max),//1
                                   new SqlParameter("@ItemCatDesc",model.ItemCatDesc),//3
                                   new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                   new SqlParameter("@UpdTerm",General.CurrentIP),//5
                                   new SqlParameter("@VehicleCategory",model.VehicleCategory),//7
                                   //new SqlParameter("@SortKey",sort),//6
                                 };

                if (sysfun.ExecuteSP_NonQuery("SP_InsertItemCategory", param))
                {
                    msg = "Sucessfully Insert";
                    IsSaved = true;
                }

            }
            catch (Exception ex)
            {
                IsSaved = false;
                msg = ex.Message;

            }

            return IsSaved;
        }
        //abdussaboor //Delete Delete_ItemCategory
        public static bool Delete_ItemCategory(ItemCategoryVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {


                if (sysfun.IsExist("ItemCatCode", model.ItemCatCode, "Item", model.DealerCode))
                {
                    IsSaved = false;
                    msg = "Record Can Not Delete Because It's Exists In 'Item' Form";
                    return IsSaved;
                }
                //ASP.Net Start
                SqlParameter[] param = {

                                    new SqlParameter("@DealerCode",model.DealerCode),//0                                  
                                    new SqlParameter("@ItemCatCode",model.ItemCatCode),//1
                                    
                                   };
                //ASP.Net End
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));

                if (sysfun.ExecuteSP_NonQuery("SP_Delete_ItemCategory", param))
                {
                    msg = "Record Deleted Sucessfully";
                    IsSaved = true;
                }

            }
            catch (Exception ex)
            {
                IsSaved = false;
                msg = ex.Message;

            }

            return IsSaved;
        }

    }
}
