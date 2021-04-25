using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Core.CRM.ADO
{
    public class UnitMethods
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
        //abdussaboor //UnitMethods/Select_UnitDetails
        public static string Select_UnitDetails(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<UnitVM> lst = new List<UnitVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_Unit", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<UnitVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        //abdussaboor //Insert Insert_Unit
        public static bool Insert_Unit(UnitVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                if (model.UnitCode == "" || model.UnitCode == null)
                {
                    max = sysfun.GetNewMaxIDWitOutDealerCode("Unit", "UnitCode", 4, "");
                }
                else
                {

                    max = model.UnitCode;

                }
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@UnitCode",max),//1
                                   new SqlParameter("@UnitDesc",model.UnitDesc),//3
                                   new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                   new SqlParameter("@UpdTerm",General.CurrentIP),//5
                                   //new SqlParameter("@SortKey",sort),//6
                                   //new SqlParameter("@VehicleCategory",model.VehicleCategory),//7
                                 };

                if (sysfun.ExecuteSP_NonQuery("SP_InsertUnit", param))
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
        //abdussaboor //Delete Delete_Unit
        public static bool Delete_Unit(UnitVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {


                if (sysfun.IsExist("UnitCode", model.UnitCode, "Item", model.DealerCode))
                {
                    IsSaved = false;
                    msg = "Record Can Not Delete Because It's Exists In 'Item' Form";
                    return IsSaved;
                }
                //ASP.Net Start
                SqlParameter[] param = {

                                    new SqlParameter("@DealerCode",model.DealerCode),//0                                  
                                    new SqlParameter("@UnitCode",model.UnitCode),//2
                                    new SqlParameter("@UnitDesc",model.UnitDesc),//3
                                    new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                    new SqlParameter("@UpdTerm", General.CurrentIP),//5
                                    
                                   };
                //ASP.Net End
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));

                if (sysfun.ExecuteSP_NonQuery("SP_Delete_Unit", param))
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
