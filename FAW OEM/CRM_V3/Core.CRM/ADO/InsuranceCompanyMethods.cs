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
    public class InsuranceCompanyMethods
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

        public static bool Insert_InsuranceCompany(InsuranceCompanyVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            bool IsSaved = false;
            try
            {
                if (model.InsCompCode == "" || model.InsCompCode == null)
                {
                    max = sysfun.GetNewMaxID("InsuranceCompanies", "InsCompCode", 4, model.DealerCode);
                }
                else
                {
                    max = model.InsCompCode;
                }
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@InsCompCode",max),//1
                                   new SqlParameter("@InsCompDescription",model.InsCompDescription),//2
                                   new SqlParameter("@InsCompPhone1",model.InsCompPhone1),//3
                                   new SqlParameter("@InsCompFax",model.InsCompFax),//7
                                   new SqlParameter("@InsCompEmail",model.InsCompEmail),//8
                                   new SqlParameter("@InsCompCellNo",model.InsCompCellNo),//8
                                   new SqlParameter("@InsCompContactPerson",model.InsCompContactPerson),//9
                                   new SqlParameter("@InsCompAddress1",model.InsCompAddress1),//9
                                   new SqlParameter("@InsCompURL",model.InsCompURL),//9
                                   new SqlParameter("@InsCompStndrdDiscount",float.Parse(model.InsCompStndrdDiscount)),//9
                                   new SqlParameter("@SalesTaxRegNo",model.SalesTaxRegNo),//9
                                   new SqlParameter("@NTNNo",model.NTNNo),//9
                                   new SqlParameter("@UpdUser",AuthBase.UserId),
                                   new SqlParameter("@UpdTerm",General.CurrentIP),
                                   //new SqlParameter("@Lubricant",model.Lubricant),//8
                                   //new SqlParameter("@SortKey",sort),//6

                                 };

                if (sysfun.ExecuteSP_NonQuery("Sp_Insert_InsuranceCompany", param))
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
        public static bool Delete_InsuranceCompany(InsuranceCompanyVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            bool IsSaved = false;
            try
            {

                SqlParameter[] param = {

                                    new SqlParameter("@DealerCode",model.DealerCode),//0                                  
                                    new SqlParameter("@InsCompCode",model.InsCompCode),//1
                                    
                                   };
                //ASP.Net End
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));

                if (sysfun.ExecuteSP_NonQuery("SP_Delete_InsuranceCompanies", param))
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
