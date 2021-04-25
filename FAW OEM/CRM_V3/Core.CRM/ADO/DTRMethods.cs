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
   

    public class DTRMethods
    {
        static bool IsDeleted = false;
        static DataTable dt = new DataTable();
        static string max = string.Empty;
        static bool IsSaved = false;
        static SysFunction sysfun = new SysFunction();
        static SqlParameter[] nullSqlParam = null;
        SqlDataReader rder = null;
        public static string Get_DTRDetailData(string value)

        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<DTRVM> lst = new List<DTRVM>();
            try
            {
                SqlParameter[] sqlParam = {

                                        };

                dt = DataAccess.getDataTable("sp_GetFIRMaster_Approval", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<DTRVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        public static string Get_DTRDetailDataHCV(string value)

        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<DTRVM> lst = new List<DTRVM>();
            try
            {
                SqlParameter[] sqlParam = {

                                        };

                dt = DataAccess.getDataTable("sp_GetFIRMaster_ApprovalHCV", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<DTRVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        public static List<DTRVM> Get_DTRDetailImage(string DealerCode,string FIRCode )

        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            SqlDataReader rder = null;
            List<DTRVM> lst = new List<DTRVM>();
            try
            {
                SqlParameter[] sqlParam = {
                    new SqlParameter("@DealerCode",DealerCode),//1
                                                new SqlParameter("@JobCardCode",FIRCode)//1
                                        };

                //dt = DataAccess.getDataTable("sp_W2_FIR_ImageDetail_Select", sqlParam, General.GetBMSConString());
                //if (dt.Rows.Count > 0)
                //{
                //    lst = EnumerableExtension.ToList<DTRVM>(dt);
                //}
                if (sysfun.ExecuteSP("sp_W2_FIR_ImageDetail_Select", sqlParam, ref rder))
                {
                    while (rder.Read())
                    {
                        lst.Add(new DTRVM
                        {
                            FIRCode = rder["FIRCode"].ToString(),
                           /// Name = sdr["Name"].ToString(),
                            //ContentType = sdr["ContentType"].ToString(),
                            ImageData = (byte[])rder["ImageData"]
                        });
                    }


                }


                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return lst;
        }
        public static string Get_DTR_CloseLCV(string value)

        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<DTRVM> lst = new List<DTRVM>();
            try
            {
                SqlParameter[] sqlParam = {

                                        };

                dt = DataAccess.getDataTable("Get_DTR_Approved_OEM", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<DTRVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        public static string Get_DTR_CloseHCV(string value)

        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<DTRVM> lst = new List<DTRVM>();
            try
            {
                SqlParameter[] sqlParam = {

                                        };

                dt = DataAccess.getDataTable("Get_DTR_Approved_OEM_HCV", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<DTRVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        public static string Get_DTRDetail(string DealerCode, string PPRCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<DTRVM> lst = new List<DTRVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                                new SqlParameter("@DealerCode",DealerCode),//1
                                                new SqlParameter("@FIRCode",PPRCode)//1



                                        };

                dt = DataAccess.getDataTable("sp_GetFIRMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<DTRVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        public static string Get_DTRPartsData(string DealerCode, string PPRCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<DTRVM> lst = new List<DTRVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                                new SqlParameter("@DealerCode",DealerCode),
                                                new SqlParameter("@JobCardCode",PPRCode)//1
//1
                                             //1

                                        };

                dt = DataAccess.getDataTable("sp_W2_FIR_PartsDetail_Select", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<DTRVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        public static bool Insert_FIRMasterData(DTRVM model)
        {
            try
            {

                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
                                 new SqlParameter("@FIRCode",model.FIRCode),//1
                                 new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                 new SqlParameter("@UpdTerm",General.CurrentIP),
                                 new SqlParameter("@ApprovalDate",sysfun.SaveDate(model.ApprovalDate)),
                                 new SqlParameter("@ApprovalStatus",model.ApprovalStatus),//2								 
                                 new SqlParameter("@ApprovalRemarks",model.ApprovalRemarks),//6,
                             };
                SqlParameter[] param1 = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
                                 new SqlParameter("@FIRCode",model.FIRCode),//1
                                 new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                 new SqlParameter("@UpdTerm",General.CurrentIP),
                                 new SqlParameter("@ApprovalDate",sysfun.SaveDate(model.ApprovalDate)),
                                 new SqlParameter("@ApprovalStatus",model.ApprovalStatus),//2								 
                                 new SqlParameter("@ApprovalRemarks",model.ApprovalRemarks),//6,
                                 new SqlParameter("@AHFMParts",SysFunction.CustomCDBL(model.AHFMParts)),//6
                                 new SqlParameter("@AHFMLub",SysFunction.CustomCDBL(model.AHFMLub)),//6
                                 new SqlParameter("@AHFMSublet",SysFunction.CustomCDBL(model.AHFMSublet)),//6
                                 new SqlParameter("@AHFMLabor",SysFunction.CustomCDBL(model.AHFMLabor)),//6
                                 new SqlParameter("@AHFMTotal",SysFunction.CustomCDBL(model.AHFMTotal)),//6
                            };
                if (model.VehicleCategory == "HCV")
                {
                    if (sysfun.ExecuteSP_NonQuery("sp_2W_Service_Update_FIRMasterHCVApproval", param1))
                    {

                        IsSaved = true;

                    }

                }
                else
                {
                    if (sysfun.ExecuteSP_NonQuery("sp_2W_Service_Update_FIRMasterApproval", param))
                    {

                        IsSaved = true;

                    }


                }
                   


            }
            catch (Exception)
            {

                throw;
            }

            return IsSaved;
        }






    }
}
