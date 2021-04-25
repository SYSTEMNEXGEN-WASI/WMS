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
    public class OEMMethods
    {
        static OEMDealerVM response = new OEMDealerVM();
        static bool IsDeleted = false;
        static DataTable dt = new DataTable();
        static string max = string.Empty;
        static bool IsSaved = false;
        static SysFunction sysfun = new SysFunction();
        static SqlParameter[] nullSqlParam = null;
        static string strAutoCode = string.Empty;


        public static string Get_DealerDetails(string PDealerCode, string PDealerDesc)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<OEMDealerVM> lst = new List<OEMDealerVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                        };

                dt = DataAccess.getDataTable("SP_selectDealerDetails", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<OEMDealerVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }


        public static string Get_DealerDetailsInsertion(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<OEMDealerVM> lst = new List<OEMDealerVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                                new SqlParameter("@DealerCode",DealerCode)//1

                                        };

                dt = DataAccess.getDataTable("SP_Select_dealerLocation_Insertion", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<OEMDealerVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }


        public static bool Delete_DealerDetailRecord(string value)
        {
            DataSet ds = new DataSet();

            SqlParameter[] param = {
                new SqlParameter("@PDealerCode",value),
            };

            if (sysfun.ExecuteSP_NonQuery("SP_DeleteDealerDetails", param))
            {
                IsDeleted = true;
            }
            else
            {
                IsDeleted = false;
            }

            return IsDeleted;
        }
        public static bool Insert_DealerDetail(OEMDealerVM model)
        {
            try
            {

                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
                                 new SqlParameter("@DealerDesc",model.PDealerDesc),//1
                                 new SqlParameter("@ContactPerson",model.ContactPerson),
                                 new SqlParameter("@Phone1",model.Phone1),//2								 
                                 new SqlParameter("@Phone2",model.Phone2),//2								 //2								 
                                 new SqlParameter("@Address1",model.Address1),//3								
                                 new SqlParameter("@CityCode",model.CityCode),//6
                                 new SqlParameter("@CountryCode",model.CountryCode),//7
                                 new SqlParameter("@StateCode",model.StateCode),//8
                                 new SqlParameter("@DealerCatCode",model.DealerCatCode),//9
                                 new SqlParameter("@DealershipTypeCode",model.DealershipTypeCode),//10
                                 new SqlParameter("@Email",model.Email),//11
                                 new SqlParameter("@NTN",model.NTN),//12
                                 new SqlParameter("@LicenseNo",model.LicenseNo),//13
                                 new SqlParameter("@SaleTaxNo",model.SaleTaxNo),
                                 new SqlParameter("@PST",model.PST),
                                 new SqlParameter("@FacilityCode",model.FacilityCode),
                                 new SqlParameter("@CurrencyCode",model.CurrencyCode),
                                 new SqlParameter("@DealerAdminId",model.DealerAdminId),
                                 new SqlParameter("@Active",model.Active),
                                 new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                 new SqlParameter("@UpdTerm",General.CurrentIP),
                                 new SqlParameter("@OperatedBy",model.OperatedBy)
                            };

                if (sysfun.ExecuteSP_NonQuery("SP_InsertDealerDetaildata", param))
                {

                    IsSaved = true;

                }

            }
            catch (Exception)
            {

                throw;
            }

            return IsSaved;

        }

        public static bool Insert_ParentDealer(OEMDealerVM model)
        {
            try
            {

                SqlParameter[] param = {
                                         new SqlParameter("@PDealerCode",model.PDealerCode),//0
                                         new SqlParameter("@PDealerDesc",model.PDealerDesc),//1
                                         new SqlParameter("@Active",model.Active),//1
                                         new SqlParameter("@UpdUser",AuthBase.UserId),//4
								         new SqlParameter("@UpdTerm",General.CurrentIP),//5

                                       };


                if (sysfun.ExecuteSP_NonQuery("SP_InsertDealerDetails", param))
                {

                    IsSaved = true;

                }

            }
            catch (Exception)
            {

                throw;
            }

            return IsSaved;
        }

        public static bool Insert_PPRCauseData(WarrentyApprovalVM objects)
        {
            throw new NotImplementedException();
        }

        public static string Get_DealerCatData(string dealerCode)
        {
            
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<OEMDealerVM> lst = new List<OEMDealerVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                     new SqlParameter("@DealerCode",dealerCode)
                                 };
                dt = DataAccess.getDataTable("SP_Select_dealerCat_Detail", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<OEMDealerVM>(dt);
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
            List<OEMDealerVM> lst = new List<OEMDealerVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                                new SqlParameter("@PDealerCode",PDealerCode)//1

                                        };

                dt = DataAccess.getDataTable("SP_Select_dealerLocation_Detail", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<OEMDealerVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }


        public static bool Insert_DealerCatDetail(OEMDealerVM model, string dealerCode)
        {
          

            try
            {
                if (model.DealerCatCode == "" || model.DealerCatCode == null)
                {
                    max = sysfun.GetNewMaxIDWitOutDealerCode("DealerCategory", "DealerCatCode", 3, "");

                    //strAutoCode = sysfun.AutoGen("DealerCategory", "DealerCatCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), dealerCode);
                }
                else
                {

                    max = model.DealerCatCode;
                    
                }

                SqlParameter[] param = {
                                         new SqlParameter("@DealerCode",dealerCode),//0                                         
                                         new SqlParameter("@DealerCatCode",max),//0
                                         new SqlParameter("@DealerCatDesc",model.DealerCatDesc),//1
                                         new SqlParameter("@InActive",model.InActive),//1
                                         new SqlParameter("@UpdUser",AuthBase.UserId),//4
								         new SqlParameter("@UpdTerm",General.CurrentIP),
                                       };


                if (sysfun.ExecuteSP_NonQuery("SP_InsertDealerCatDetails", param))
                {

                    IsSaved = true;

                }

            }
            catch (Exception)
            {

                throw;
            }

            return IsSaved;
        }

        //yousuf_jobcategory
        public static bool Insert_JobCategory(OEMJobCategoryVM model)
        {

            if (model.JobCatCode == "" || model.JobCatCode == null)
            {
                max = sysfun.GetNewMaxID("jobcategory1", "JobCatCode", 3, model.DealerCode);

                //strAutoCode = sysfun.AutoGen("DealerCategory", "DealerCatCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), dealerCode);
            }
            else
            {

                max = model.JobCatCode;

            }
            
            try
            {
              
                SqlParameter[] param = {
                                         new SqlParameter("@DealerCode",model.DealerCode),//0
                                         new SqlParameter("@JobCatCode",max),//1
                                         new SqlParameter("@JobCatDesc",model.JobCatDesc),//1
                                         new SqlParameter("@AccountCode",model.AccountCode),//1
                                         //new SqlParameter("@Active",model.Active),//1
                                         new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                         //new SqlParameter("@UpdDate",General.CurrentDate),
                                         new SqlParameter("@UpdTerm",General.CurrentIP),//5

                                       };


                if (sysfun.ExecuteSP_NonQuery("SP_InsertJobCategory", param))
                {

                    IsSaved = true;

                }

            }
            catch (Exception)
            {

                throw;
            }

            return IsSaved;
        }
        // Inesert Labor Detail
        //yousuf_jobcategory
        public static bool Insert_LaborDetail(OEM_LabourMaster model,ref string msg)
        {

            if (model.DefJobCode == "" || model.DefJobCode == null)
            {
                msg = "Please Select Job First";
                return false;
             //   max = sysfun.GetNewMaxID("jobcategory1", "JobCatCode", 3, model.DealerCode);

                //strAutoCode = sysfun.AutoGen("DealerCategory", "DealerCatCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), dealerCode);
            }
            else
            {

                max = model.DefJobCode;

            }

            try
            {

                SqlParameter[] param = {
                                           new SqlParameter("@DealerCode",model.DealerCode),//0
                                           new SqlParameter("@DefJobCode",max),//1
                                           new SqlParameter("@ProdCode",model.ProdCode),//2
                                           new SqlParameter("@VersionCode",model.VersionCode),//3
                                           new SqlParameter("@Amount",model.Amount),//4
                                           new SqlParameter("@UpdUser",AuthBase.UserId),//5
                                           new SqlParameter("@UpdTerm",General.CurrentIP),//6
                                           new SqlParameter("@ServiceTax",model.ServiceTax),//7
                                           new SqlParameter("@BrandCode",model.BrandCode),//8
                                           new SqlParameter("@StnTime",model.StandardTime)//9
                                       
                                       };


                if (sysfun.ExecuteSP_NonQuery("Sp_Insert_LaborDetail", param))
                {

                    IsSaved = true;

                }

            }
            catch (Exception)
            {

                throw;
            }

            return IsSaved;
        }
        //get_jobcategoryinto grid


        //Insert 
        public static bool Insert_JobMaster(OEM_LabourMaster model, ref string msg)
        {

           

            string strAutoCode = string.Empty;
            try
            {
                if (model.DefJobCode == "" || model.DefJobCode == null)
                {
                max= sysfun.GetNewMaxID("DefaultJob", "DefJobCode", 8,model.DealerCode);
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
                                   new SqlParameter("@LaborAccount",model.LabourAccount),//6
                                   new SqlParameter("@StandardTime",model.StandardTime),//7
                                   new SqlParameter("@BayCode",model.BayCode),
                                   new SqlParameter("@VehicleCategory",model.VehicleCategory)
                                 };

                


               if(sysfun.ExecuteSP_NonQuery("Sp_Insert_DefaultJob", param))
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

        public static bool Insert_JobCardMaster(OEMJobCategoryVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                if (model.JobTypeCode == "" || model.JobTypeCode == null)
                {
                    max = sysfun.GetNewMaxID("JobTypeMaster", "JobTypeCode", 3, model.DealerCode);
                }
                else
                {

                    max = model.JobTypeCode;

                }
                int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey",3, model.DealerCode));
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@JobTypeCode",max),//1
                                   new SqlParameter("@JobTypeDesc",model.JobTypeDesc),//3
                                   new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                   new SqlParameter("@UpdTerm",General.CurrentIP),//5
                                   new SqlParameter("@SortKey",sort),//6
                                   new SqlParameter("@Unchanged",model.Unchanged),//7
                                 };

                if (sysfun.ExecuteSP_NonQuery("Sp_Insert_JobCardType", param))
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

        //abdussaboor //Insert BankMaster
        public static bool Insert_BankMaster(OEMJobCategoryVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                if (model.BankCode == "" || model.BankCode == null)
                {
                    max = sysfun.GetNewMaxID("Bank", "DealerCode", 3, model.DealerCode);
                }
                else
                {

                    max = model.BankCode;

                }
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@BankCode",max),//1
                                   new SqlParameter("@BankDesc",model.BankDesc),//3
                                   new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                   new SqlParameter("@UpdTerm",General.CurrentIP),//5
                                   //new SqlParameter("@SortKey",sort),//6
                                   new SqlParameter("@BankStatus",model.BankStatus),//7
                                 };

                if (sysfun.ExecuteSP_NonQuery("SP_InsertBank", param))
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

        //abdussaboor //Insert CityMaster
        public static bool Insert_CityMaster(OEMJobCategoryVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                if (model.CityCode == "" || model.CityCode == null)
                {
                    max = sysfun.GetNewMaxID("City", "DealerCode", 3, model.CityCode);
                }
                else
                {

                    max = model.CityCode;

                }
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@CityCode",max),//1
                                   new SqlParameter("@CityDesc",model.CityDesc),//3
                                   new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                   new SqlParameter("@UpdTerm",General.CurrentIP),//5
                                   //new SqlParameter("@SortKey",sort),//6
                                   new SqlParameter("@StateCode",model.StateCode),//7
                                 };

                if (sysfun.ExecuteSP_NonQuery("SP_InsertCity", param))
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

        //abdussaboor //Insert StateMaster
        public static bool Insert_StateMaster(OEMJobCategoryVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                if (model.CountryCode == "" || model.CountryCode == null)
                {
                    max = sysfun.GetNewMaxID("State", "DealerCode", 3, model.CountryCode);
                }
                else
                {

                    max = model.CountryCode;

                }
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));
                SqlParameter[] param = {
                                   //new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@CountryCode",max),//1
                                   new SqlParameter("@StateCode",model.StateCode),//3
                                   //new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                   //new SqlParameter("@UpdTerm",General.CurrentIP),//5
                                   //new SqlParameter("@SortKey",sort),//6
                                   new SqlParameter("@StateDesc",model.StateDesc),//7
                                 };

                if (sysfun.ExecuteSP_NonQuery("SP_InsertState", param))
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

        //abdussaboor //Get_JobCategoryDetails
        public static string Get_JobCategoryDetails(string JobCatCode, string JobCatDesc)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<OEMJobCategoryVM> lst = new List<OEMJobCategoryVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                        };

                dt = DataAccess.getDataTable("SP_selectJobCategoryDetails", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<OEMJobCategoryVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        //abdussaboor //Get_JobDetails
        public static string Get_JobDetails(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<OEMJobCategoryVM> lst = new List<OEMJobCategoryVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_DefaultJob", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<OEMJobCategoryVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        //abdussaboor //Get_JobTypeMaster
        public static string Get_JobTypeMaster(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<OEMJobCategoryVM> lst = new List<OEMJobCategoryVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_JobTypeMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<OEMJobCategoryVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        //abdussaboor //Get_BankMaster
        public static string Get_BankMaster(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<OEMJobCategoryVM> lst = new List<OEMJobCategoryVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_BankMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<OEMJobCategoryVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        //abdussaboor //Get_CityMaster
        public static string Get_CityMaster(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<OEMJobCategoryVM> lst = new List<OEMJobCategoryVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_CityMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<OEMJobCategoryVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        //abdussaboor //Get_StateMaster
        public static string Get_StateMaster(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<OEMJobCategoryVM> lst = new List<OEMJobCategoryVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_StateMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<OEMJobCategoryVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        public static string Get_JobCategory(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<OEMJobCategoryVM> lst = new List<OEMJobCategoryVM>();
            
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_JobCategory", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<OEMJobCategoryVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }


        //delete Job Category record

        public static bool Delete_JobCategoryRecord(string value)
        {
            DataSet ds = new DataSet();

            SqlParameter[] param = {
                new SqlParameter("@JobCatCode",value),
            };

            //SP_DeleteDealerDetails
            if (sysfun.ExecuteSP_NonQuery("SP_DeleteJobCategoryDetails", param))
            {
                IsDeleted = true;
            }
            else
            {
                IsDeleted = false;
            }

            return IsDeleted;
        }
        public static bool Delete_LaborRecord(string value,string ProdCode,string DealerCode)
        {
            DataSet ds = new DataSet();

            SqlParameter[] param = {
                new SqlParameter("@DealerCode",value),
                  new SqlParameter("@ProdCode",ProdCode),
                    new SqlParameter("@DefJobCode",value),
            };

            //SP_DeleteDealerDetails
            if (sysfun.ExecuteSP_NonQuery("SP_DeleteLaborDetails", param))
            {
                IsDeleted = true;
            }
            else
            {
                IsDeleted = false;
            }

            return IsDeleted;
        }
        public static string Get_LabourDetail(string DealerCode, string PPRCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<OEM_LabourMaster> lst = new List<OEM_LabourMaster>();
            try
            {
                SqlParameter[] sqlParam = {
                                               new SqlParameter("@DealerCode",DealerCode),//1
                                               new SqlParameter("@JobCode",PPRCode)//1



                                       };

                dt = DataAccess.getDataTable("Get_LabourDetail", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<OEM_LabourMaster>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        //public static bool Insert_PPRCauseData(WarrentyApprovalVM objects)
        //{
        //    throw new NotImplementedException();
        //}


    }
}