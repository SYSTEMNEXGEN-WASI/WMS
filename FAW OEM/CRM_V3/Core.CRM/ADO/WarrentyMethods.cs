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
   public class WarrentyMethods
    {
       static WarrentyApprovalVM responce = new WarrentyApprovalVM();
       static bool IsDeleted = false;
       static DataTable dt = new DataTable();
        static DataTable dp = new DataTable();
        static string max = string.Empty;
       static bool IsSaved = false;
       static SysFunction sysfun = new SysFunction();
       static SqlParameter[] nullSqlParam = null;
        static string sql,FailPart;
        static Transaction ObjTrans = new Transaction();
        static SqlTransaction Trans;

        public static string Get_WarrentyDetailData(string value)
        
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<WarrentyApprovalVM> lst = new List<WarrentyApprovalVM>();
            try
            {
                SqlParameter[] sqlParam = {

                                        };

                dt = DataAccess.getDataTable("SP_SelectPPRInformation", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<WarrentyApprovalVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        public static string Get_WarrentyDetailFailPart(string value)

        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<WarrentyApprovalVM> lst = new List<WarrentyApprovalVM>();
            try
            {
                SqlParameter[] sqlParam = {

                                        };

                dt = DataAccess.getDataTable("SP_SelectPPRInformationFailPart", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<WarrentyApprovalVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        public static string Get_WarrentyDetailFailPart_HCV(string value)

        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<WarrentyApprovalVM> lst = new List<WarrentyApprovalVM>();
            try
            {
                SqlParameter[] sqlParam = {

                                        };

                dt = DataAccess.getDataTable("SP_SelectPPRInformationFailPart_HCV", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<WarrentyApprovalVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        public static string Get_Warranty_CloseLCV(string value)

        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<WarrentyApprovalVM> lst = new List<WarrentyApprovalVM>();
            try
            {
                SqlParameter[] sqlParam = {

                                        };

                dt = DataAccess.getDataTable("Get_WR_Approved_OEM_LCV", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<WarrentyApprovalVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        public static string Get_Warranty_CloseHCV(string value)

        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<WarrentyApprovalVM> lst = new List<WarrentyApprovalVM>();
            try
            {
                SqlParameter[] sqlParam = {

                                        };

                dt = DataAccess.getDataTable("Get_WR_Approved_OEM_HCV", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<WarrentyApprovalVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        public static string Get_WarrentyDetailDataHCV(string value)

        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<WarrentyApprovalVM> lst = new List<WarrentyApprovalVM>();
            try
            {
                SqlParameter[] sqlParam = {

                                        };

                dt = DataAccess.getDataTable("SP_SelectPPRInformationHCV", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<WarrentyApprovalVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        public static string Get_VehicleWarrantyDetail(string DealerCode, string PPRCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<WarrentyApprovalVM> lst = new List<WarrentyApprovalVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                                new SqlParameter("@DealerCode",DealerCode),//1
                                                new SqlParameter("@PPRCode",PPRCode)//1



                                        };

                dt = DataAccess.getDataTable("SP_Select_PPRMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<WarrentyApprovalVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        public static string Get_VehicleWarrantyDetailHCV(string DealerCode, string PPRCode)
       {
           string json = "";
           var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
           List<WarrentyApprovalVM> lst = new List<WarrentyApprovalVM>();
           try
           {
               SqlParameter[] sqlParam = {
                                                new SqlParameter("@DealerCode",DealerCode),//1
                                                new SqlParameter("@PPRCode",PPRCode)//1



                                        };

               dt = DataAccess.getDataTable("SP_Select_PPRMaster", sqlParam, General.GetBMSConString());
               if (dt.Rows.Count > 0)
               {
                   lst = EnumerableExtension.ToList<WarrentyApprovalVM>(dt);
               }
               json = Serializer.Serialize(lst);
           }
           catch (Exception ex)
           {

               throw;
           }

           return json;
       }
       public static string Get_WRCauseDetail(string DealerCode, string PPRCode)
       {
           string json = "";
           var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
           List<WarrentyApprovalVM> lst = new List<WarrentyApprovalVM>();
           try
           {
               SqlParameter[] sqlParam = {
                                                new SqlParameter("@DealerCode",DealerCode),//1
                                                new SqlParameter("@PPRCode",PPRCode)//1



                                        };

               dt = DataAccess.getDataTable("sp_2W_Service_Select_PPRDetail", sqlParam, General.GetBMSConString());
               if (dt.Rows.Count > 0)
               {
                   lst = EnumerableExtension.ToList<WarrentyApprovalVM>(dt);
               }
               json = Serializer.Serialize(lst);
           }
           catch (Exception ex)
           {

               throw;
           }

           return json;
       }

       public static bool Insert_PPRCauseData(WarrentyApprovalVM model)
       {
           try
           {

               SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
                                 new SqlParameter("@PPRRef",model.PPRCode),//1
                                 new SqlParameter("@ComplainCode",model.ComplainCode),
                                 new SqlParameter("@DefectCode",model.DefectCode),//2								 
                                 new SqlParameter("@ComplaintDesc",model.ComplaintDesc),//2								 //2								 
                                 new SqlParameter("@LabourRate",model.LabourRate),//3								
                                 new SqlParameter("@LabourDesc",model.LabourDesc),//6
                                 new SqlParameter("@RootCauseFailure",model.RootCauseFailure),
                                 new SqlParameter("@CorrectiveAction",model.CorrectiveAction)
                                 //new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                 //new SqlParameter("@UpdTerm",General.CurrentIP)
                            };

               if (sysfun.ExecuteSP_NonQuery("sp_2W_Service_Insert_PPRDetail", param))
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
       public static bool Insert_PPRMasterData(WarrentyApprovalVM model,ref string msg)
       {
           try
           {
                sql = "Select FailedPartReceivedFlag from PPRPartsDetail where  DealerCode='" + model.DealerCode + "' and PPRCode='" + model.PPRCode + "'   ";
                dt = sysfun.GetData(sql);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rows in dt.Rows)
                    {
                        FailPart = rows["FailedPartReceivedFlag"].ToString();
                        // Delflag = dt.Rows[0]["DelFalg"].ToString();
                        if (FailPart == "" || FailPart == null)
                        {
                            msg = "Claim Can Not Be Approved Some Failed Part Remaining to Received!";
                            IsSaved = false;
                            return IsSaved;
                        }

                    }

                }

                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
                                 new SqlParameter("@PPRCode",model.PPRCode),//1
                                 //new SqlParameter("@JobCardCode",model.JobCardCode),
                                 //new SqlParameter("@PPRDate",model.PPRDate),//2								 
                                 //new SqlParameter("@CusCode",model.CusCode),//2								 //2								 
                                 //new SqlParameter("@UserName",model.UserName),//3								
                                 //new SqlParameter("@UserAddress1",model.UserAddress1),//6
                                 //new SqlParameter("@UserPhoneNo",model.UserPhoneNo),
                                 //new SqlParameter("@VehEngineNo",model.VehEngineNo),
                                 //new SqlParameter("@VehChassisNo",model.VehChassisNo),//0
                                 //new SqlParameter("@VehRegNo",model.VehRegNo),//1
                                 //new SqlParameter("@InvoiceDate",model.InvoiceDate),
                                 //new SqlParameter("@MeterReadingKM",model.MeterReadingKM),//2								 
                                 //new SqlParameter("@DateOfProblem",model.DateOfProblem),//2								 //2								 
                                 //new SqlParameter("@RoadCondition",model.RoadCondition),//3								
                                 //new SqlParameter("@NoOfProblems",model.NoOfProblems),//6
                                 //new SqlParameter("@CompletionDate",model.CompletionDate),
                                 //new SqlParameter("@PostFlag",model.PostFlag),
                                 //new SqlParameter("@DelFlag",model.DelFlag),//0
                                 new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                 new SqlParameter("@UpdTerm",General.CurrentIP),
                                 //new SqlParameter("@TransferStatus",model.TransferStatus),//1
                                 //new SqlParameter("@Status",model.Status),
                                 //new SqlParameter("@Remarks",model.Remarks),//2								 
                                 //new SqlParameter("@TotalAmount",model.TotalAmount),//2								 //2								 
                                 //new SqlParameter("@SaleDealerCode",model.SaleDealerCode),//3								
                                 //new SqlParameter("@SaleDealerName",model.SaleDealerName),
                                 new SqlParameter("@ApprovalNo",model.ApprovalNo),//1
                                 new SqlParameter("@ApprovalDate",model.ApprovalDate),
                                 new SqlParameter("@ApprovalStatus",model.ApprovalStatus),//2								 
                                 //new SqlParameter("@TRNo",model.TRNo),//2								 //2								 
                                 //new SqlParameter("@TRDate",model.TRDate),//3								
                                 new SqlParameter("@ComplainRemarks",model.ComplainRemarks),//6
                                 //new SqlParameter("@DTRNo",model.DTRNo),//6
                                 new SqlParameter("@ApprovalFlag",model.ApprovalFlag),//6
                                 new SqlParameter("@AHFMParts",SysFunction.CustomCDBL(model.AHFMPart)),//6
                                new SqlParameter("@AHFMSublet",SysFunction.CustomCDBL(model.AHFMSublet)),//6
                                 new SqlParameter("@AHFMLabor",SysFunction.CustomCDBL(model.AHFMLabor)),//6
                                 new SqlParameter("@AHFMTotal",SysFunction.CustomCDBL(model.AHFMTotal))//6
                            };

              
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    sysfun.ExecuteSP_NonQuery("sp_2W_Service_Update_pprMastersApproval", param, Trans);


                    IsSaved = true;
                }

            }
           catch (Exception ex)
           {

                // throw;
                msg = ex.Message;
           }

           return IsSaved;
       }
        public static bool Insert_PPRPart(List<WarrentyApprovalVM> model2, ref string msg)
        {
            int count = 0;

            DataTable dt = new DataTable();

            //dt = sysfun.GetData("Select BrandCode , ProdCode , VersionCode ,ColorCode, ColorDesc from VehicleSaleDetail where ChassisNo = '" + ChasisNo + "' or EngineNo = '" + EngineNo + "'", "BMS0517ConnectionString");

            try
            {
                foreach (var item in model2)
                {
                    if (item.DealerCode != null || item.DealerCode != null)
                    {
                        if (count >= 1 || item.PPRCode != null)
                        {

                            SqlParameter[] param2 = {
                                    new SqlParameter("@DealerCode",item.DealerCode),
                                    new SqlParameter("@PPRCode",item.PPRCode) ,
                                    new SqlParameter("@PartNo",item.PartNo) ,
                                    new SqlParameter("@ItemCode",item.PartsCode) ,
                                    new SqlParameter("@AHFMQty",item.AHFMQty) ,
                                    new SqlParameter("@TotalAmount",item.TotalAmount) ,
                                 
                            };


                            if (sysfun.ExecuteSP_NonQuery("sp_2W_Service_Update_pprpartApproval", param2, Trans) == true)
                            {
                                IsSaved = true;
                            }
                            else
                            {
                                ObjTrans.RollBackTransaction(ref Trans);
                                IsSaved = false;
                            }
                        }
                        count++;


                    }

                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                //throw;
                msg = ex.Message;
            }
            //ObjTrans.CommittTransaction(ref Trans);
            return IsSaved;
        }
        public static bool Insert_PPRLabor(List<WarrentyApprovalVM> model2, ref string msg)
        {
            int count = 0;

            DataTable dt = new DataTable();

            //dt = sysfun.GetData("Select BrandCode , ProdCode , VersionCode ,ColorCode, ColorDesc from VehicleSaleDetail where ChassisNo = '" + ChasisNo + "' or EngineNo = '" + EngineNo + "'", "BMS0517ConnectionString");

            try
            {
                foreach (var item in model2)
                {
                    if (item.DealerCode != null || item.DealerCode != null)
                    {
                        if (count >= 1 || item.PPRCode != null)
                        {

                            SqlParameter[] param2 = {
                                    new SqlParameter("@DealerCode",item.DealerCode),
                                    new SqlParameter("@PPRCode",item.PPRCode) ,
                                    new SqlParameter("@LaborCode",item.LaborCode) ,
                                    new SqlParameter("@AHFMRate",item.AHFMRate) ,
                                    new SqlParameter("@TotalLabor",item.TotalLabor) ,
                                 
                            };


                            if (sysfun.ExecuteSP_NonQuery("sp_2W_Service_Update_pprLaborApproval", param2, Trans) == true)
                            {
                                IsSaved = true;
                            }
                            else
                            {
                                ObjTrans.RollBackTransaction(ref Trans);
                                IsSaved = false;
                            }
                        }
                        count++;


                    }

                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                //throw;
                msg = ex.Message;
            }
           
            return IsSaved;
        }
        public static bool Insert_PPRSublet(List<WarrentyApprovalVM> model2, ref string msg)
        {
            int count = 0;

            DataTable dt = new DataTable();

            //dt = sysfun.GetData("Select BrandCode , ProdCode , VersionCode ,ColorCode, ColorDesc from VehicleSaleDetail where ChassisNo = '" + ChasisNo + "' or EngineNo = '" + EngineNo + "'", "BMS0517ConnectionString");

            try
            {
                if (model2 !=null)
                {


                    foreach (var item in model2)
                    {
                        if (item.DealerCode != null || item.DealerCode != null)
                        {
                            if (count >= 1 || item.PPRCode != null)
                            {

                                SqlParameter[] param2 = {
                                    new SqlParameter("@DealerCode",item.DealerCode),
                                    new SqlParameter("@PPRCode",item.PPRCode) ,
                                    new SqlParameter("@JobCode",item.JobCode) ,
                                    new SqlParameter("@AHFMRate",item.AHFMRate) ,

                            };


                                if (sysfun.ExecuteSP_NonQuery("sp_2W_Service_Update_pprSubletApproval", param2, Trans) == true)
                                {
                                    IsSaved = true;
                                }

                                else
                                {
                                    ObjTrans.RollBackTransaction(ref Trans);
                                    IsSaved = false;
                                }
                            }
                            count++;


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                //throw;
                msg = ex.Message;
            }
            ObjTrans.CommittTransaction(ref Trans);
            return IsSaved;
        }

        public static string Get_PPRLaborData(string DealerCode, string PPRCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<WarrentyApprovalVM> lst = new List<WarrentyApprovalVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                                new SqlParameter("@DealerCode",DealerCode),
                                                new SqlParameter("@PPRCode",PPRCode)//1
//1
                                             //1

                                        };

                dt = DataAccess.getDataTable("sp_2W_Service_Select_PPRWarrantyJobs", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<WarrentyApprovalVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        public static string Get_PPRSubletrData(string DealerCode, string PPRCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<WarrentyApprovalVM> lst = new List<WarrentyApprovalVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                                new SqlParameter("@DealerCode",DealerCode),
                                                new SqlParameter("@PPRCode",PPRCode)//1
//1
                                             //1

                                        };

                dt = DataAccess.getDataTable("sp_2W_Service_Select_PPRWarrantySubletJobs", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<WarrentyApprovalVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        public static string Get_PPRPartsData(string DealerCode, string PPRCode)
       {
           string json = "";
           var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
           List<WarrentyApprovalVM> lst = new List<WarrentyApprovalVM>();
           try
           {
               SqlParameter[] sqlParam = {
                                                new SqlParameter("@DealerCode",DealerCode),
                                                new SqlParameter("@PPRCode",PPRCode)//1
//1
                                             //1

                                        };

               dt = DataAccess.getDataTable("sp_2W_Service_Select_PPRPartsDetail", sqlParam, General.GetBMSConString());
               if (dt.Rows.Count > 0)
               {
                   lst = EnumerableExtension.ToList<WarrentyApprovalVM>(dt);
               }
               json = Serializer.Serialize(lst);
           }
           catch (Exception ex)
           {

               throw;
           }

           return json;
       }
        public static string Get_PPRFPartsData(string DealerCode, string PPRCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<WarrentyApprovalVM> lst = new List<WarrentyApprovalVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                                new SqlParameter("@DealerCode",DealerCode),
                                                new SqlParameter("@PPRCode",PPRCode)//1
//1
                                             //1

                                        };

                dp = DataAccess.getDataTable("sp_2W_Service_Select_PPRPartsDetail", sqlParam, General.GetBMSConString());
                if (dp.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<WarrentyApprovalVM>(dp);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        //public static bool Insert_PPRPartsData(WarrentyApprovalVM model)
        //{
        //    try
        //    {

        //        SqlParameter[] param = {
        //                          new SqlParameter("@DealerCode",model.DealerCode),//0
        //                          new SqlParameter("@DealerDesc",model.PPRCode),//1
        //                          new SqlParameter("@ContactPerson",model.PartsCode),
        //                          new SqlParameter("@Phone1",model.Quantity),//2								 
        //                          new SqlParameter("@Phone2",model.GSTAmount),//2								 //2								 
        //                          new SqlParameter("@Address1",model.TotalAmount),//3								
        //                          new SqlParameter("@CityCode",model.RecStatus),//6
        //                          new SqlParameter("@CountryCode",model.Casual),
        //                          new SqlParameter("@UpdUser",AuthBase.UserId),//4
        //                          new SqlParameter("@UpdTerm",General.CurrentIP)
        //                     };

        //        if (sysfun.ExecuteSP_NonQuery("sp_2W_Service_Insert_PPRPartsDetail", param))
        //        {

        //            IsSaved = true;

        //        }

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //    return IsSaved;
        //}

        public static bool Insert_PPRFailPart(WarrentyApprovalVM model,ref string msg)
        {
            try
            {

                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
                                 new SqlParameter("@PPRCode",model.PPRCode),//1
                                 new SqlParameter("@PartsNo",model.PartsCode),
                                 new SqlParameter("@FPRQTY",model.FPRQTY),//2								 
                                 new SqlParameter("@FPDate",sysfun.SaveDate(model.FPDate)),//2								 //2								 
                                 new SqlParameter("@FailedPartReceivedFlag",model.FailedPartReceivedFlag),//3								
                                 new SqlParameter("@FPRemarks",model.FPRemarks),
                            };

                if (sysfun.ExecuteSP_NonQuery("sp_2W_UpdateFailPart", param))
                {

                    IsSaved = true;

                }

            }
            catch (Exception ex)
            {

                
                msg = ex.Message;
            }

            return IsSaved;
        }

        public static bool Insert_PPRFailPartHCV(WarrentyApprovalVM model, ref string msg)
        {
            try
            {
               

                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
                                 new SqlParameter("@PPRCode",model.PPRCode),//1
                                 new SqlParameter("@PartsNo",model.PartsCode),
                                 new SqlParameter("@FPRQTY",model.FPRQTY),//2								 
                                 new SqlParameter("@FPDate",model.FPDate),//2								 //2								 
                                 new SqlParameter("@FailedPartReceivedFlag",model.FailedPartReceivedFlag),//3								
                                 new SqlParameter("@FPRemarks",model.FPRemarks),
                            };

                if (sysfun.ExecuteSP_NonQuery("sp_2W_UpdateFailPart", param))
                {

                    IsSaved = true;

                }

            }
            catch (Exception ex)
            {


                msg = ex.Message;
            }

            return IsSaved;
        }
    }
}
