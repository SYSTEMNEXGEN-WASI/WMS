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
    public class CityMasterMethods
    {
        static OEMDealerVM response = new OEMDealerVM();
        static bool IsDeleted = false;
        static DataTable dt = new DataTable();
        static string max = string.Empty;
        static bool IsSaved = false;
        static SysFunction sysfun = new SysFunction();
        static SqlParameter[] nullSqlParam = null;
        static string strAutoCode = string.Empty;

        //abdussaboor //Get_CityMaster
        public static string Get_CityMaster(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<CityMasterVM> lst = new List<CityMasterVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_CityMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<CityMasterVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        //abdussaboor //Get_State
        public static string Get_State(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<CityMasterVM> lst = new List<CityMasterVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_StateMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<CityMasterVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        //abdussaboor //Insert Insert_CityMaster
        public static bool Insert_CityMaster(CityMasterVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                //if (model.PayModeCode == "" || model.PayModeCode == null)
                //{
                //    max = sysfun.GetNewMaxID("JobCategory", "JobCatCode", 3, model.DealerCode);
                //}
                //else
                //{

                //    max = model.PayModeCode;

                //}
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@CityCode",model.CityCode),//1
                                   new SqlParameter("@CityDesc",model.CityDesc),//3
                                   new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                   new SqlParameter("@UpdTerm",General.CurrentIP),//5
                                   new SqlParameter("@StateCode",model.StateCode),//7
                                   //new SqlParameter("@SortKey",sort),//6
                                 };

                if (sysfun.ExecuteSP_NonQuery("SP_InsertCity", param))
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
        //abdussaboor //Delete Delete_CityMaster
        public static bool Delete_CityMaster(CityMasterVM model, ref string msg)
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

                                    new SqlParameter("@DealerCode",model.DealerCode),//0                                  
                                    new SqlParameter("@CityCode",model.CityCode),//1
                                    
                                   };
                //ASP.Net End
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));

                if (sysfun.ExecuteSP_NonQuery("SP_Delete_City", param))
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
