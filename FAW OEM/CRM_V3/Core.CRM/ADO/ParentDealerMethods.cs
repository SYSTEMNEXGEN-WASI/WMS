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
    public class ParentDealerMethods
    {
        static OEMDealerVM response = new OEMDealerVM();
        static bool IsDeleted = false;
        static DataTable dt = new DataTable();
        static string max = string.Empty;
        static bool IsSaved = false;
        static SysFunction sysfun = new SysFunction();
        static SqlParameter[] nullSqlParam = null;
        static string strAutoCode = string.Empty;

        //abdussaboor //Select_CompanyDetails
        public static string Select_CompanyDetails(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<ParentDealerVM> lst = new List<ParentDealerVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_CompanyMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ParentDealerVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        //abdussaboor //Get_PaymentMode
        public static string Select_ParentDealerDetails(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<ParentDealerVM> lst = new List<ParentDealerVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_ParentDealer", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ParentDealerVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        //abdussaboor //Insert Insert_ParentDealer
        public static bool Insert_ParentDealer(ParentDealerVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                if (model.PDealerCode == "" || model.PDealerCode == null)
                {
                    max = model.CompanyCode+sysfun.GetNewMaxIDWitOutDealerCode("ParentDealer", "PDealerCode", 4, "Where CompanyCode='"+model.CompanyCode+"'");
                }
                else
                {
                    max = model.PDealerCode;

                }
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));
                SqlParameter[] param = {
                                   //new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@PDealerCode",max),//1
                                   new SqlParameter("@PDealerDesc",model.PDealerDesc),//3
                                   new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                   new SqlParameter("@UpdTerm",General.CurrentIP),//5
                                   new SqlParameter("@CompanyCode",model.CompanyCode),//7
                                   new SqlParameter("@Active",model.Active),//8
                                   //new SqlParameter("@SortKey",sort),//6
                                 };

                if (sysfun.ExecuteSP_NonQuery("SP_InsertPDealer", param))
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
        public static bool Delete_ParentDealer(ParentDealerVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {


                if (sysfun.IsExistwithoutDealer("ParentCode", model.PDealerCode, "Dealer"))
                {
                    IsSaved = false;
                    msg = "Record Can Not Delete Because It's Exists In 'Dealer' Form";
                    return IsSaved;
                }
                //ASP.Net Start
                SqlParameter[] param = {

                                    new SqlParameter("@PDealerCode",model.PDealerCode),//0                                  
                                    new SqlParameter("@CompanyCode",model.CompanyCode),//1
                                    
                                   };
                //ASP.Net End
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));

                if (sysfun.ExecuteSP_NonQuery("SP_Delete_PDealer", param))
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
