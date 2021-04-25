using Core.CRM.ADO.ViewModel;
using Core.CRM.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Core.CRM.ADO
{
    public class DashboardMethods
    {
        public static List<SalesPersonPerformanceVM> Select_SalesPersonPerformance(string DealerCode)
        {
            List<SalesPersonPerformanceVM> lst = new List<SalesPersonPerformanceVM>();
            DataTable dt = new DataTable();

            try
            {
                SqlParameter[] param =  
                {
                    new SqlParameter("@EmpCode",AuthBase.EmpCode),
                    new SqlParameter("@DealerCode",DealerCode)
                };

                dt = DataAccess.getDataTable("Select_SalesPersonPerformance ", param, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<SalesPersonPerformanceVM>(dt);
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
