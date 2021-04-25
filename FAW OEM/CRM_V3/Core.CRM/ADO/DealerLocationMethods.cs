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
    public class DealerLocationMethods
    {
        static OEMDealerVM response = new OEMDealerVM();
        static bool IsDeleted = false;
        static DataTable dt = new DataTable();
        static string max = string.Empty;
        static bool IsSaved = false;
        static SysFunction sysfun = new SysFunction();
        static SqlParameter[] nullSqlParam = null;
        static string strAutoCode = string.Empty;
        //abdussaboor //Get_Company_PDealerDetails
        public static string Get_Company_PDealerDetails(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<DealerLocationVM> lst = new List<DealerLocationVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_CompanyMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<DealerLocationVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        //For ParentDealer
        public static string Select_PDealerDetails(string CompCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<DealerLocationVM> lst = new List<DealerLocationVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@CompCode",CompCode),//0
                 //new SqlParameter("@VehicleCategory",VehicleCategory),//1
                 //new SqlParameter("@VehicleCategory",vehicle),//2
                                        };

                dt = DataAccess.getDataTable("Get_Comp__ParentDealer", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<DealerLocationVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        //For DealerLocation
        public static string Select_Dealer(string CompCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<DealerLocationVM> lst = new List<DealerLocationVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@CompCode",CompCode),//0
                 //new SqlParameter("@VehicleCategory",VehicleCategory),//1
                 //new SqlParameter("@VehicleCategory",vehicle),//2
                                        };

                dt = DataAccess.getDataTable("Get_Comp_Dealer", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<DealerLocationVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        public static string Get_DealerDetailsData(string PDealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<DealerLocationVM> lst = new List<DealerLocationVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                                new SqlParameter("@PDealerCode",PDealerCode)//1

                                        };

                dt = DataAccess.getDataTable("SP_Select_dealerLocation_Detail", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<DealerLocationVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        //abdussaboor //Insert Get_DealerDetailsInsertion
        public static bool SP_DealerDetailsInsertion(DealerLocationVM model, ref string msg)
        {
            
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<DealerLocationVM> lst = new List<DealerLocationVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                                new SqlParameter("@DealerCode",model.DealerCode),//1
                                                new SqlParameter("@DealerDesc",model.PDealerDesc),//1
                                                new SqlParameter("@Phone1",model.Phone1),//1
                                                new SqlParameter("@Phone2",model.Phone2),//1
                                                new SqlParameter("@ContactPerson",model.ContactPerson),//1
                                                new SqlParameter("@Address1",model.Address1),//1
                                                new SqlParameter("@CityCode",model.CityCode),//1
                                                new SqlParameter("@StateCode",model.StateCode),//1
                                                new SqlParameter("@CountryCode",model.CountryCode),//1
                                                new SqlParameter("@RegionCode",model.RegionCode),//1
                                                new SqlParameter("@DealerCatCode",model.DealerCatCode),//1
                                                new SqlParameter("@DealershipTypeCode",model.DealershipTypeCode),//1
                                                new SqlParameter("@Email",model.Email),//1
                                                new SqlParameter("@NTN",model.NTN),//1
                                                new SqlParameter("@LicenseNo",model.LicenseNo),//1
                                                new SqlParameter("@SaleTaxNo",model.SaleTaxNo),//1
                                                new SqlParameter("@PST",model.PST),//1'
                                                new SqlParameter("@FacilityCode",model.FacilityCode),//1
                                                new SqlParameter("@CurrencyCode",model.CurrencyCode),//1
                                                new SqlParameter("@DealerAdminId",model.DealerAdminId),//1
                                                new SqlParameter("@OperatedBy",model.OperatedBy),//1
                                                new SqlParameter("@HandlingCharges",model.HandlingCharges),//1
                                                new SqlParameter("@DealerRefCode",model.DealerRefCode),//1
                                                new SqlParameter("@VehicleCategory",model.VehicleCategory),//1
                                                new SqlParameter("@PartRequisition",model.PartRequisition)//1
                                               



                                        };
                if (sysfun.ExecuteSP_NonQuery("SP_Dealer_Insert", sqlParam))
                {
                    msg = "Sucessfully Insert";
                    IsSaved = true;

                }

               
               
               
            }
            catch (Exception ex)
            {
                IsSaved = false;
                msg = ex.Message;
               // throw;
            }

            return IsSaved;
        }

        //abdussaboor //Delete Delete_DealerLocation
        public static bool Delete_DealerLocation(DealerLocationVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                
                SqlParameter[] param = {

                                    new SqlParameter("@DealerCode",model.DealerCode),//0
                                    
                                   };
                //ASP.Net End
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));

                if (sysfun.ExecuteSP_NonQuery("SP_Delete_Dealer", param))
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
