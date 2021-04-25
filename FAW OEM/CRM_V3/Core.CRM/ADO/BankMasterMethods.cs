using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CRM.ADO
{
    public class BankMasterMethods
    {
        //static OEMDealerVM response = new OEMDealerVM();
        static bool IsDeleted = false;
        static DataTable dt = new DataTable();
        static string max = string.Empty;
        static bool IsSaved = false;
        static SysFunction sysfun = new SysFunction();
        static SqlParameter[] nullSqlParam = null;
        static string strAutoCode = string.Empty;

        //abdussaboor //Get_BankMaster
        public static string Get_BankMaster(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<BankMasterVM> lst = new List<BankMasterVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_BankMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<BankMasterVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return json;
        }

        //abdussaboor //Insert Insert_BankMaster
        public static bool Insert_BankMaster(BankMasterVM model, ref string msg)
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
                                   new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@BankCode",model.BankCode),//1
                                   new SqlParameter("@BankDesc",model.BankDesc),//3
                                   new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                   new SqlParameter("@UpdTerm",General.CurrentIP),//5
                                   //new SqlParameter("@SortKey",sort),//6
                                   new SqlParameter("@BankStatus",model.BankStatus),//7
                                 };

                if (sysfun.ExecuteSP_NonQuery("SP_InsertBank", param))
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

        //abdussaboor //Delete Delete_BankMaster
        public static bool Delete_BankMaster(BankMasterVM model, ref string msg)
        {
            //string strAutoCode = string.Empty;
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
                                    new SqlParameter("@BankCode",model.BankCode),//1
                                    
                                   };
                //ASP.Net End
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));

                if (sysfun.ExecuteSP_NonQuery("SP_Delete_Bank", param))
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
