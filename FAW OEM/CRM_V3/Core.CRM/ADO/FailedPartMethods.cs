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
    

    public class FailedPartMethods
    {
        static bool IsSaved = false;
        static bool IsDeleted = false;
        static SqlParameter[] nullSqlParam = null;
        static DataTable dt = new DataTable();
        static string strAutoCode = string.Empty;
        static SysFunction sysfun = new SysFunction();
        static DateTime recDate = new DateTime();
       
        static SqlTransaction Trans;
        static int i = 1;
        static string stockdate;
        public static List<ItemVM> Get_Item(string dealerCode,string cat)
        {
            var Serializer = new JavaScriptSerializer();
            List<ItemVM> lst = new List<ItemVM>();
            try
            {
                SqlParameter[] sqlParam = {

                                    new SqlParameter("@DealerCode",dealerCode),//1
                                       new SqlParameter("@Category",cat)//1
									};

                dt = DataAccess.getDataTable("SP_ItemForOEM_Select", sqlParam, CConn.CConnection.GetConnectionString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ItemVM>(dt);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return lst;
        }
    }
}
