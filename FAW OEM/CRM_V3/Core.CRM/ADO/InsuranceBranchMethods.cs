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
    public class InsuranceBranchMethods
    {
        //static OEMDealerVM response = new OEMDealerVM();
        //static bool IsDeleted = false;
        static DataTable dt = new DataTable();
        static string max = string.Empty;
        //static bool IsSaved = false;
        static SysFunction sysfun = new SysFunction();
        //static SqlParameter[] nullSqlParam = null;
        static string strAutoCode = string.Empty;

        public static string Get_InsuranceCompany(string DealerCode, ref string msg)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<InsuranceCompanyVM> lst = new List<InsuranceCompanyVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_InsuranceCompany", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<InsuranceCompanyVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return json;
        }

        public static string Get_InsuranceBranch(string DealerCode,string InsComCode,ref string msg)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<InsuranceBranchVM> lst = new List<InsuranceBranchVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                 new SqlParameter("@InsComCode",InsComCode),//0
                };

                dt = DataAccess.getDataTable("Get_InsuranceBranch", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<InsuranceBranchVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return json;
        }

        public static bool Insert_InsuranceBranch(InsuranceBranchVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            bool IsSaved = false;
            try
            {
                if (model.BranchCode == "" || model.BranchCode == null)
                {
                    max = sysfun.GetNewMaxID("Branch", "BranchCode", 4, model.DealerCode);
                }
                else
                {
                    max = model.BranchCode;
                }
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@InsCompCode",model.InsCompCode),//1
                                   new SqlParameter("@BranchCode",max),//2
                                   new SqlParameter("@BranchDesc",model.BranchDesc),//3
                                   new SqlParameter("@Address",model.Address),//7
                                   new SqlParameter("@Phone",model.Phone),//8
                                   new SqlParameter("@Fax",model.Fax),//8
                                   new SqlParameter("@Email",model.Email),//9
                                   new SqlParameter("@URL",model.URL),//9
                                   new SqlParameter("@AccountCode",model.AccountCode),//9
                                   new SqlParameter("@AdvanceReceipt",float.Parse(model.AdvanceReceipt)),//9
                                   new SqlParameter("@ContName",model.ContName),//9
                                   new SqlParameter("@ContPhone",model.ContPhone),//9
                                   //new SqlParameter("@Lubricant",model.Lubricant),//8
                                   //new SqlParameter("@SortKey",sort),//6

                                 };

                if (sysfun.ExecuteSP_NonQuery("Sp_Insert_InsuranceBranch", param))
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

        //abdussaboor //Delete Delete_InvoiceRequest
        public static bool Delete_InsuranceBranch(InsuranceBranchVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            bool IsSaved = false;
            try
            {

                SqlParameter[] param = {

                                    new SqlParameter("@DealerCode",model.DealerCode),//0                                  
                                    new SqlParameter("@InsCompCode",model.InsCompCode),//1
                                    new SqlParameter("@BranchCode",model.BranchCode),//1
                                    
                                   };
                //ASP.Net End
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));

                if (sysfun.ExecuteSP_NonQuery("SP_Delete_InsuranceBranch", param))
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


        public static List<AccountVM> Get_AccDesc(string dealerCode)
        {
            DataTable dt = new DataTable();
            string json = "";
            var Serializer = new JavaScriptSerializer();
            // string TransType = "CSI";
            //string SaleType = "Cash";


            List<AccountVM> lst = new List<AccountVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@CompCode",dealerCode)//0
                                    // , new SqlParameter("@TransType",TransType)//0
                                    //, new SqlParameter("@SaleType",SaleType)//0
									};

                dt = DataAccess.getDataTable("SP_Select_AccountCode", sqlParam, General.GetFAMConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<AccountVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }
    }
}
