using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Core.CRM.ADO
{
    public class CusProfileMethods
        
    {
        static CusProfileVM response = new CusProfileVM();
        static DataTable dt = new DataTable();
        static string strAutoCode = string.Empty;
        static string autoProspect_ID = string.Empty;
        static bool IsSaved = false;
        static SqlParameter[] nullSqlParam = null;
        public static string Get_CustomerDetails()
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<CusProfileVM> lst = new List<CusProfileVM>();
            try
            {
                SqlParameter[] sqlParam = {
                  

                                        };

                dt = DataAccess.getDataTable("SP_Select_CustomerDetailsMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<CusProfileVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
    }
}
