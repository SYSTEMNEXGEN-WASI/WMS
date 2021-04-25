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
    public class VersionMethods
    {
        static bool IsSaved = false;
        static bool IsDeleted = false;
        static SqlParameter[] nullSqlParam = null;
        static DataTable dt = new DataTable();
        static string strAutoCode = string.Empty;
        static SysFunction sysfun = new SysFunction();
        static DateTime recDate = new DateTime();
        static Transaction ObjTrans = new Transaction();
        static SqlTransaction Trans;
        static int i = 1;
        static OEMDealerVM response = new OEMDealerVM();
        static string max = string.Empty;

        //abdussaboor //VersionMethods/Select_VersionDetails
        public static string Select_VersionDetails(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<VersionVM> lst = new List<VersionVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_Version", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<VersionVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        //abdussaboor //Insert Insert_Version
        public static bool Insert_Version(VersionVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                if (model.VersionCode == "" || model.VersionCode == null)
                {
                    max = sysfun.GetNewMaxIDWitOutDealerCode("Versions", "VersionCode", 3, "");
                }
                else
                {

                    max = model.VersionCode;

                }
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@VersionCode",max),//1
                                   new SqlParameter("@VersionDesc",model.VersionDesc),//3
                                   new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                   new SqlParameter("@UpdTerm",General.CurrentIP),//5
                                   //new SqlParameter("@SortKey",sort),//6
                                   //new SqlParameter("@VehicleCategory",model.VehicleCategory),//7
                                 };

                if (sysfun.ExecuteSP_NonQuery("SP_InsertVersion", param))
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
        //abdussaboor //Delete Delete_Version
        public static bool Delete_Version(VersionVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {


                if (sysfun.IsExist("VersionCode", model.VersionCode, "CustomerVehicle", model.DealerCode))
                {
                    IsSaved = false;
                    msg = "Record Can Not Delete Because It's Exists In 'CustomerVehicle' Form";
                    return IsSaved;
                }
                //ASP.Net Start
                SqlParameter[] param = {

                                    new SqlParameter("@DealerCode",model.DealerCode),//0                                  
                                    new SqlParameter("@VersionCode",model.VersionCode),//2
                                    new SqlParameter("@VersionDesc",model.VersionDesc),//3
                                    new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                    new SqlParameter("@UpdTerm", General.CurrentIP),//5
                                    
                                   };
                //ASP.Net End
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));

                if (sysfun.ExecuteSP_NonQuery("SP_Delete_Version", param))
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
