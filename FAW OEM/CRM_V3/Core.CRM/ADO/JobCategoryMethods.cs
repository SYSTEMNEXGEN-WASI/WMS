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
    public class JobCategoryMethods
    {
        static OEMDealerVM response = new OEMDealerVM();
        static bool IsDeleted = false;
        static DataTable dt = new DataTable();
        static string max = string.Empty;
        static bool IsSaved = false;
        static SysFunction sysfun = new SysFunction();
        static SqlParameter[] nullSqlParam = null;
        static string strAutoCode = string.Empty;
        //abdussaboor //Get_JobCategory
        public static string Get_JobCategory(string DealerCode, ref string msg)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<JobCategoryVM> lst = new List<JobCategoryVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_JobCategory", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<JobCategoryVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                msg = ex.Message;
            }

            return json;
        }
        //abdussaboor //Insert Insert_JobCategory
        public static bool Insert_JobCategory(JobCategoryVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                if (model.JobCatCode == "" || model.JobCatCode == null)
                {
                    max = sysfun.GetNewMaxIDWitOutDealerCode("JobCategory", "JobCatCode", 3,"");
                }
                else
                {

                    max = model.JobCatCode;

                }
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@JobCatCode",max),//1
                                   new SqlParameter("@JobCatDesc",model.JobCatDesc),//3
                                   new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                   new SqlParameter("@UpdTerm",General.CurrentIP),//5
                                   new SqlParameter("@VehicleCategory",model.VehicleCategory),//7
                                   //new SqlParameter("@SortKey",sort),//6
                                 };

                if (sysfun.ExecuteSP_NonQuery("SP_InsertJobCategory", param))
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
        //abdussaboor //Delete Delete_JobCategory
        public static bool Delete_JobCategory(JobCategoryVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {


                if (sysfun.IsExist("JobCatCode", model.JobCatCode, "DefaultJob", model.DealerCode))
                {
                    IsSaved = false;
                    msg = "Record Can Not Delete Because It's Exists In 'Item' Form";
                    return IsSaved;
                }
                //ASP.Net Start
                SqlParameter[] param = {

                                    new SqlParameter("@DealerCode",model.DealerCode),//0                                  
                                    new SqlParameter("@JobCatCode",model.JobCatCode),//1
                                    
                                   };
                //ASP.Net End
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));

                if (sysfun.ExecuteSP_NonQuery("SP_Delete_JobCategory", param))
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
