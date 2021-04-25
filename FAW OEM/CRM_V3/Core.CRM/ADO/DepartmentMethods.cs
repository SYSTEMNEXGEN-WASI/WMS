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
    public class DepartmentMethods
    {

        static OEMDealerVM response = new OEMDealerVM();
        static bool IsDeleted = false;
        static DataTable dt = new DataTable();
        static string max = string.Empty;
        static bool IsSaved = false;
        static SysFunction sysfun = new SysFunction();
        static SqlParameter[] nullSqlParam = null;
        static string strAutoCode = string.Empty;
        //abdussaboor //Get_Deapartment
        public static string Get_Deapartment(string DealerCode ,ref string msg)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<DepartmentVM> lst = new List<DepartmentVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_DepartmentMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<DepartmentVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                msg = ex.Message;
            }

            return json;
        }
        //abdussaboor //Insert Insert_Department
        public static bool Insert_Department(DepartmentVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                if (model.DeptCode == "" || model.DeptCode == null)
                {
                    max = sysfun.GetNewMaxIDWitOutDealerCode("Department", "DeptCode", 3, "");
                }
                else
                {

                    max = model.DeptCode;

                }
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@DeptCode",max),//1
                                   new SqlParameter("@DeptDesc",model.DeptDesc),//3
                                   new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                   new SqlParameter("@UpdTerm",General.CurrentIP),//5
                                   new SqlParameter("@VehicleCategory",model.VehicleCategory),//7
                                   //new SqlParameter("@SortKey",sort),//6
                                 };

                if (sysfun.ExecuteSP_NonQuery("SP_InsertDept", param))
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
        //abdussaboor //Delete Delete_Department
        public static bool Delete_Department(DepartmentVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {


                if (sysfun.IsExist("DeptCode", model.DeptCode, "DealerEmp", model.DealerCode))
                {
                    IsSaved = false;
                    msg = "Record Can Not Delete Because It's Exists In 'DefaultJob' Form";
                    return IsSaved;
                }
                //ASP.Net Start
                SqlParameter[] param = {

                                    new SqlParameter("@DealerCode",model.DealerCode),//0                                  
                                    new SqlParameter("@DeptCode",model.DeptCode),//1
                                    
                                   };
                //ASP.Net End
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));

                if (sysfun.ExecuteSP_NonQuery("SP_Delete_Department", param))
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
