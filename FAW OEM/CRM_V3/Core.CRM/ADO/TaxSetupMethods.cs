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
    public class TaxSetupMethods
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
        public static string Get_TaxDetails(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<TaxSetupVM> lst = new List<TaxSetupVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_TaxSteupMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<TaxSetupVM>(dt);
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
        public static bool Insert_TaxSetup(TaxSetupVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                if (model.TaxAppCode == "" || model.TaxAppCode == null)
                {
                    max = sysfun.GetNewMaxID("TaxSetupMaster", "TaxAppCode", 6, model.DealerCode);
                }
                else
                {
                    max = model.TaxAppCode;
                }
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@TaxAppCode",max),//1
                                   new SqlParameter("@TaxAppDesc",model.TaxAppDesc),//3
                                   new SqlParameter("@Applicable",model.Applicable),//4
                                   new SqlParameter("@ChangeAble",model.ChangeAble),//5
                                   new SqlParameter("@Part",model.Part),//7
                                   new SqlParameter("@Sublet",model.Sublet),//8
                                   new SqlParameter("@Labor",model.Labor),//8
                                   new SqlParameter("@Boughtout",model.BoughtOut),//8
                                   new SqlParameter("@TaxType",model.TaxType),//8
                                   new SqlParameter("@GST",float.Parse(model.GST)),//8
                                   new SqlParameter("@FurTax",float.Parse(model.FurTax)),//8
                                   new SqlParameter("@ExtraTax",float.Parse(model.ExtraTax)),//8
                                   new SqlParameter("@PST",float.Parse(model.PST)),//8
                                   new SqlParameter("@Lubricant",model.Lubricant),//8
                                   //new SqlParameter("@SortKey",sort),//6
                                 };

                if (sysfun.ExecuteSP_NonQuery("Sp_Insert_Tax", param))
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
        //abdussaboor //Delete Delete_TaxSetup
        public static bool Delete_TaxSetup(TaxSetupVM model, ref string msg)
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

                                    new SqlParameter("@DealerCode",model.DealerCode),//0                                  
                                    new SqlParameter("@TaxAppCode",model.TaxAppCode),//1
                                    
                                   };
                //ASP.Net End
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));

                if (sysfun.ExecuteSP_NonQuery("SP_Delete_Tax", param))
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