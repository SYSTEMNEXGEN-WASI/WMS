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
    public class BrandMethods
    {
        static bool IsSaved = false;
        static bool IsDeleted = false;
        static SqlParameter[] nullSqlParam = null;
        static DataTable dt = new DataTable();
        static string strAutoCode = string.Empty;
        static SysFunction sysfun = new SysFunction();
        static DateTime recDate = new DateTime();
        static Transaction ObjTrans = new Transaction();
        static SqlTransaction Trans;
        static int i = 1;
        static OEMDealerVM response = new OEMDealerVM();
        static string max = string.Empty;

        //abdussaboor //BrandMethods/Select_BrandDetails
        public static string Select_BrandDetails(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<BrandVM> lst = new List<BrandVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_Brand", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<BrandVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        //abdussaboor //Insert Insert_Brand
        public static bool Insert_Brand(BrandVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                if (model.BrandCode == "" || model.BrandCode == null)
                {
                    max = sysfun.GetNewMaxIDWitOutDealerCode("Brand", "BrandCode", 3, "");
                }
                else
                {

                    max = model.BrandCode;

                }
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@BrandCode",max),//1
                                   new SqlParameter("@BrandDesc",model.BrandDesc),//3
                                   new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                   new SqlParameter("@UpdTerm",General.CurrentIP),//5
                                   //new SqlParameter("@SortKey",sort),//6
                                   //new SqlParameter("@VehicleCategory",model.VehicleCategory),//7
                                 };

                if (sysfun.ExecuteSP_NonQuery("SP_InsertBrand", param))
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
        //abdussaboor //Delete Delete_Brand
        public static bool Delete_Brand(BrandVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                

                if (sysfun.IsExist("BrandCode", model.BrandCode, "CustomerVehicle", model.DealerCode))
                {
                    IsSaved = false;
                    msg = "Record Can Not Delete Because It's Exists In 'CustomerVehicle' Form";
                    return IsSaved;
                }
                //ASP.Net Start
                SqlParameter[] param = {

                                    new SqlParameter("@DealerCode",model.DealerCode),//0                                  
                                    new SqlParameter("@BrandCode",model.BrandCode),//2
                                    new SqlParameter("@BrandDesc",model.BrandDesc),//3
                                    new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                    new SqlParameter("@UpdTerm", General.CurrentIP),//5
                                    
                                   };
                //ASP.Net End
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));

                if (sysfun.ExecuteSP_NonQuery("SP_Delete_Brand", param))
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
