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
    public class StateMasterMethods
    {
        static OEMDealerVM response = new OEMDealerVM();
        static bool IsDeleted = false;
        static DataTable dt = new DataTable();
        static string max = string.Empty;
        static bool IsSaved = false;
        static SysFunction sysfun = new SysFunction();
        static SqlParameter[] nullSqlParam = null;
        static string strAutoCode = string.Empty;

        //abdussaboor //Get_StateMaster
        public static string Get_StateMaster(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<StateMasterVM> lst = new List<StateMasterVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_StateMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StateMasterVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        //abdussaboor //Insert StateMaster
        public static bool Insert_StateMaster(StateMasterVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                //if (model.StateCode == "" || model.StateCode == null)
                //{
                //    max = sysfun.GetNewMaxID("State", "DealerCode", 3, model.DealerCode);
                //}
                //else
                //{

                //    max = model.StateCode;

                //}
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));
                SqlParameter[] param = {
                                   //new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@CountryCode",model.CountryCode),//1
                                   new SqlParameter("@StateCode",model.StateCode),//3
                                   //new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                   //new SqlParameter("@UpdTerm",General.CurrentIP),//5
                                   //new SqlParameter("@SortKey",sort),//6
                                   new SqlParameter("@StateDesc",model.StateDesc),//7
                                 };

                if (sysfun.ExecuteSP_NonQuery("SP_InsertState", param))
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

        //abdussaboor //Delete Delete_StateMaster
        public static bool Delete_StateMaster(StateMasterVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {


                //if (sysfun.IsExist("PayModeCode", model.PayModeCode, "DefaultJob", model.DealerCode))
                //{
                //    IsSaved = false;
                //    msg = "Record Can Not Delete Because It's Exists In 'DefaultJob' Form";
                //    return IsSaved;
                //}
                //ASP.Net Start
                SqlParameter[] param = {

                                    new SqlParameter("@CountryCode",model.CountryCode),//0                                  
                                    new SqlParameter("@StateCode",model.StateCode),//1
                                    
                                   };
                //ASP.Net End
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));

                if (sysfun.ExecuteSP_NonQuery("SP_Delete_State", param))
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
