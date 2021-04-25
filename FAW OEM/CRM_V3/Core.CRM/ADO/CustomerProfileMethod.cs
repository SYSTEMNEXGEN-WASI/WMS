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

    public class CustomerProfileMethod
    {

        static CustomerProfileVM response = new CustomerProfileVM();
        static bool IsDeleted = false;
        static DataTable dt = new DataTable();
        static string max = string.Empty;
        static bool IsSaved = false;
        static SysFunction sysfun = new SysFunction();
        static SqlParameter[] nullSqlParam = null;

        public static string Get_CustomerProfileDetails(string CusCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<CustomerProfileVM> lst = new List<CustomerProfileVM>();
            try
            {
                SqlParameter[] sqlParam = {
                  									new SqlParameter("@CusCode",CusCode),  //0

                                                     
                                        };

                dt = DataAccess.getDataTable("SP_SelectCustomerDetail", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<CustomerProfileVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        public static string Get_CustomerDetails()
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<CusProfileVM> lst = new List<CusProfileVM>();
            try
            {
                SqlParameter[] sqlParam = {
                  

                                        };

                dt = DataAccess.getDataTable("SP_Select_CustomerDetails", sqlParam, General.GetBMSConString());
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



        public static bool Insert_CustomerDetailData(CustomerProfileVM model)
        {
            try
            {

                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
                                 new SqlParameter("@CusCode",model.CusCode),//1
                                 new SqlParameter("@CusTypeCode",model.CusTypeCode),
                                 new SqlParameter("@Title",model.Title),//2								 
                                 new SqlParameter("@CusDesc",model.CusDesc),//2								 //2								 
                                 new SqlParameter("@NIC",model.NIC),//3								
                                 new SqlParameter("@ContPerson",model.ContPerson),//6
                                 new SqlParameter("@Address1",model.Address1),
                                 new SqlParameter("@CountryCode",model.CountryCode),
                                 new SqlParameter("@StateCode",model.StateCode),//1
                                 new SqlParameter("@CityCode",model.CityCode),
                                 new SqlParameter("@Phone1",model.Phone1),//2								 
                                 new SqlParameter("@Phone2",model.Phone2),//2								 //2								 
                                 new SqlParameter("@CellNo",model.CellNo),//3								
                                 new SqlParameter("@FaxNo",model.FaxNo),//6
                                 new SqlParameter("@Email",model.Email),
                                 new SqlParameter("@URL",model.URL),//1
                                 new SqlParameter("@NTNno",model.NTNno),
                                 new SqlParameter("@SalesTaxRegNo",model.SalesTaxRegNo),//2								 
                                 new SqlParameter("@CreditDays",model.CreditDays),//2								 //2								 
                                 new SqlParameter("@CreditLimit",model.CreditLimit),//3								
                                 new SqlParameter("@Distance",model.Distance),//6
                                 new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                 new SqlParameter("@UpdTerm",General.CurrentIP)
                            };

                if (sysfun.ExecuteSP_NonQuery("sp_Sales_Insert_CustomerDetails", param))
                {

                    IsSaved = true;

                }

            }
            catch (Exception)
            {

                throw;
            }

            return IsSaved;
        }
    }
}
