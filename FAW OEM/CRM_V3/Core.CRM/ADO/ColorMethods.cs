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
    public class ColorMethods
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

        //abdussaboor //ColorMethods/Select_ColorDetails
        public static string Select_ColorDetails(string DealerCode,string cat)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<ColorVM> lst = new List<ColorVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                  new SqlParameter("@VehicleCategory",cat),//0
                                        };

                dt = DataAccess.getDataTable("Get_Color", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ColorVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        //abdussaboor //Insert Insert_Color
        public static bool Insert_Color(ColorVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                if (model.ColorCode == "" || model.ColorCode == null)
                {
                    max = sysfun.GetNewMaxIDWitOutDealerCode("Color", "ColorCode", 5,"");
                }
                else
                {

                    max = model.ColorCode;

                }
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@ColorCode",max),//1
                                   new SqlParameter("@ColorDesc",model.ColorDesc),//3
                                   new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                   new SqlParameter("@UpdTerm",General.CurrentIP),//5
                                   new SqlParameter("@VehicleCategory",model.VehicleCatory),//7
                                   //new SqlParameter("@SortKey",sort),//6
                                 };

                if (sysfun.ExecuteSP_NonQuery("SP_InsertColor", param))
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
        //abdussaboor //Delete Delete_Color
        public static bool Delete_Color(ColorVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {


                if (sysfun.IsExist("ColorCode", model.ColorCode, "Vehicle", model.DealerCode))
                {
                    IsSaved = false;
                    msg = "Record Can Not Delete Because It's Exists In 'Vehicle' Form";
                    return IsSaved;
                }
                if (sysfun.IsExist("ColorCode", model.ColorCode, "VehicleTaxDetail", model.DealerCode))
                {
                    IsSaved = false;
                    msg = "Record Can Not Delete Because It's Exists In 'VehicleTaxDetail' Form";
                    return IsSaved;
                }
                if (sysfun.IsExist("ColorCode", model.ColorCode, "CustomerVehicle", model.DealerCode))
                {
                    IsSaved = false;
                    msg = "Record Can Not Delete Because It's Exists In 'CustomerVehicle' Form";
                    return IsSaved;
                }
                
                //ASP.Net Start
                SqlParameter[] param = {

                                    new SqlParameter("@DealerCode",model.DealerCode),//0                                  
                                    new SqlParameter("@ColorCode",model.ColorCode),//2
                                    new SqlParameter("@ColorDesc",model.ColorDesc),//3
                                    new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                    new SqlParameter("@UpdTerm", General.CurrentIP),//5
                                    
                                   };
                //ASP.Net End
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));

                if (sysfun.ExecuteSP_NonQuery("SP_Delete_Color", param))
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
