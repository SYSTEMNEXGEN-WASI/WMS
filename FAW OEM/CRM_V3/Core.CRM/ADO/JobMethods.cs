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
    public class JobMethods
    {
        static OEMDealerVM response = new OEMDealerVM();
        static bool IsDeleted = false;
        static DataTable dt = new DataTable();
        static string max = string.Empty;
        static bool IsSaved = false;
        static SysFunction sysfun = new SysFunction();
        static SqlParameter[] nullSqlParam = null;
        static string strAutoCode = string.Empty;
        //abdussaboor //Get_Job
        public static string Get_Job(string DealerCode,ref string msg)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<JobVM> lst = new List<JobVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                   //new SqlParameter("@VehicleCategory",vehicle),//1
                                        };

                dt = DataAccess.getDataTable("Get_Job", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<JobVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                msg = ex.Message;
            }

            return json;
        }
        //abdussaboor //Get_JobCat
        public static string Get_JobCat(string DealerCode, ref string msg)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<JobVM> lst = new List<JobVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                 //new SqlParameter("@VehicleCategory",vehicle),//2
                                        };

                dt = DataAccess.getDataTable("Get_JobCat", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<JobVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                msg = ex.Message;
            }

            return json;
        }
        //abdussaboor //Insert Insert_Job
        public static bool Insert_Job(JobVM model, ref string msg)
        {

            string strAutoCode = string.Empty;
            try
            {
                if (model.DefJobCode == "" || model.DefJobCode == null)
                {
                    max = sysfun.GetNewMaxIDWitOutDealerCode("DefaultJob", "DefJobCode", 6,"");
                }
                else
                {

                    max = model.DefJobCode;

                }

                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@JobCatCode",model.JobCatCode),//1
                                   new SqlParameter("@DefJobCode",max),//2
                                   new SqlParameter("@DefJobDesc",model.DefJobDesc),//3
                                   new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                   new SqlParameter("@UpdTerm",General.CurrentIP),//5
                                   new SqlParameter("@LaborAccount",model.LaborAccount),//6
                                   new SqlParameter("@StandardTime",model.StandardTime),//7
                                   new SqlParameter("@BayCode",model.BayCode),
                                   new SqlParameter("@VehicleCategory",model.VehicleCategory)
                                 };




                if (sysfun.ExecuteSP_NonQuery("Sp_Insert_DefaultJob", param))
                {
                    msg = "Sucessfully Inert";
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
        //abdussaboor //Delete Delete_Job
        public static bool Delete_Job(JobVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {


                if (sysfun.IsExist("DefJobCode", model.DefJobCode, "LaborDetail", model.DealerCode))
                {
                    IsSaved = false;
                    msg = "Record Can Not Delete Because It's Exists In 'Labour Detail' Form";
                    return IsSaved;
                }
                //ASP.Net Start
                SqlParameter[] param = {

                                    new SqlParameter("@DealerCode",model.DealerCode),//0                                  
                                    new SqlParameter("@DefJobCode",model.DefJobCode),//1
                                    
                                   };
                //ASP.Net End
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));

                if (sysfun.ExecuteSP_NonQuery("SP_Delete_DefaultJob", param))
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
