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
    public class AlertsMethods
    {
        static AlertVM response = new AlertVM();
        static bool IsDeleted = false;
        static DataTable dt = new DataTable();
        static string max = string.Empty;
        static bool IsSaved = false;
        static SysFunction sysfun = new SysFunction();
        static SqlParameter[] nullSqlParam = null;



        public static string Get_AlertEnquiryDetails()
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<EnquiryMethods> lst = new List<EnquiryMethods>();
            try
            {
                SqlParameter[] sqlParam = {
                                        };

                dt = DataAccess.getDataTable("SP_SelectAlertEnquiryDetails", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<EnquiryMethods>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        public static string Get_AlertFollowUpDetails()
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<EnquiryMethods> lst = new List<EnquiryMethods>();
            try
            {
                SqlParameter[] sqlParam = {
                                        };

                dt = DataAccess.getDataTable("SP_SelectAlertFollowUpDetails", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<EnquiryMethods>(dt);
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