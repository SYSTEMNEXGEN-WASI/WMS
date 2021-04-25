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
    public class CompanyMethods
    {
        static OEMDealerVM response = new OEMDealerVM();
        static bool IsDeleted = false;
        static DataTable dt = new DataTable();
        static string max = string.Empty;
        static bool IsSaved = false;
        static SysFunction sysfun = new SysFunction();
        static SqlParameter[] nullSqlParam = null;
        static string strAutoCode = string.Empty;
        //abdussaboor //Get_PaymentMode
        public static string Get_Company()
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<CompanyVM> lst = new List<CompanyVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 //new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_Company", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<CompanyVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        //abdussaboor //Insert Insert_PaymentMode
        public static bool Insert_Company(CompanyVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                //if (model.CompanyCode == "" || model.CompanyCode == null)
                //{
                //    max = sysfun.GetNewMaxID("SecurityCompany", "CompanyCode", 3, model.DealerCode);
                //}
                //else
                //{

                //    max = model.CompanyCode;

                //}
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));
                SqlParameter[] param = {
                                   //new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@CompanyCode",model.CompanyCode),//1
                                   new SqlParameter("@CompanyName",model.CompanyName),//3
                                   //new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                   //new SqlParameter("@UpdTerm",General.CurrentIP),//5
                                   new SqlParameter("@Description",model.Description),//7
                                   //new SqlParameter("@PayModeCodeOEM","C"),//8
                                   //new SqlParameter("@SortKey",sort),//6
                                 };

                if (sysfun.ExecuteSP_NonQuery("SP_InsertCompany", param))
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
        //abdussaboor //Delete Delete_PaymentMode
        public static bool Delete_Company(CompanyVM model, ref string msg)
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

                                    new SqlParameter("@CompanyCode",model.CompanyCode),//0                                  
                                    //new SqlParameter("@PayModeCode",model.PayModeCode),//1
                                    
                                   };
                //ASP.Net End
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));

                if (sysfun.ExecuteSP_NonQuery("SP_Delete_Company", param))
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
