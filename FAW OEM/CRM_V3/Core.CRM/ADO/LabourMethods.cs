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
    public class LabourMethods
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
        public static string Get_Labour(string DealerCode,string CatCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<LabourVM> lst = new List<LabourVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                 new SqlParameter("@CatCode",CatCode),//1
                                        };

                dt = DataAccess.getDataTable("Get_DefaultJob", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<LabourVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        //abdussaboor //Get_JobCat
        public static string Get_LabourCat(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<LabourVM> lst = new List<LabourVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                 //new SqlParameter("@VehicleCategory",vehicle),//2
                                        };

                dt = DataAccess.getDataTable("Get_JobCategory", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<LabourVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        public static string Get_LabourDetailGrid(string DealerCode, string PPRCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<LabourVM> lst = new List<LabourVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                               new SqlParameter("@DealerCode",DealerCode),//1
                                               new SqlParameter("@JobCode",PPRCode)//1



                                       };

                dt = DataAccess.getDataTable("Get_LabourDetail", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<LabourVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        //abdussaboor //Insert Insert_Job
        public static bool Insert_Labour(LabourVM model, ref string msg)
        {

            string strAutoCode = string.Empty;
            try
            {
                if (model.DefJobCode == "" || model.DefJobCode == null)
                {
                    max = sysfun.GetNewMaxID("DefaultJob", "DefJobCode", 8, model.DealerCode);
                }
                else
                {

                    max = model.DefJobCode;

                }

                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@DefJobCode",max),//1
                                   new SqlParameter("@BrandCode",model.BrandCode),//2
                                   new SqlParameter("@ProdCode",model.ProdCode),
                                   new SqlParameter("@VersionCode",model.VersionCode),//3
                                   new SqlParameter("@Amount",model.Amount),//4
                                   new SqlParameter("@UpdUser",AuthBase.UserId),//5
                                   new SqlParameter("@UpdTerm",General.CurrentIP),//6
                                   new SqlParameter("@ServiceTax",model.ServiceTax),//7
                                   new SqlParameter("@StnTime",model.StandardTime),//8
                                   new SqlParameter("@VehicleCategory",model.VehicleCategory)
                                 };




                if (sysfun.ExecuteSP_NonQuery("Sp_Insert_LaborDetail", param))
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
        //abdussaboor //Delete Delete_Labour
        public static bool Delete_Labour(LabourVM model, ref string msg)
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

                if (sysfun.ExecuteSP_NonQuery("SP_Delete_Labour", param))
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
        public static List<PrdouctVM> Get_BrandProductData(string enquiryId, string dealerCode, string VehicleCategory)
        {
            var Serializer = new JavaScriptSerializer();
            List<PrdouctVM> lst = new List<PrdouctVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@EnquiryId",enquiryId),//0
									new SqlParameter("@DealerCode",dealerCode),//1
                                    new SqlParameter("@VehicleCategory",VehicleCategory)//1
									};

                dt = DataAccess.getDataTable("SP_BrandProduct_Select", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<PrdouctVM>(dt);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return lst;
        }
    }
}
