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
using CConn;

namespace Core.CRM.ADO
{
    public class ScheduleMethods
    {
        static OEMDealerVM response = new OEMDealerVM();
        static bool IsDeleted = false;
        static DataTable dt = new DataTable();
        static string max = string.Empty;
        static bool IsSaved = false;
        static SysFunction sysfun = new SysFunction();
        static SqlParameter[] nullSqlParam = null;
        static string strAutoCode = string.Empty;
        static string DealerCode = string.Empty;
        static Transaction ObjTrans = new Transaction();
        public static SqlTransaction Trans;
        //abdussaboor //Get_Schedule
        public static string Get_Schedule(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<ScheduleVM> lst = new List<ScheduleVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                   //new SqlParameter("@VehicleCategory",vehicle),//1
                                        };

                dt = DataAccess.getDataTable("Get_Schedule", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ScheduleVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        public static string Get_Schedule_Version(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<ScheduleVM> lst = new List<ScheduleVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                 //new SqlParameter("@VehicleCategory",vehicle),//2
                                        };

                dt = DataAccess.getDataTable("Get_Sch_Vehicle", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ScheduleVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        public static string Get_Schedule_Job(string ProdCode , string VehicleCategory)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<ScheduleVM> lst = new List<ScheduleVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@ProdCode",ProdCode),//0
                 new SqlParameter("@VehicleCategory",VehicleCategory),//1
                 //new SqlParameter("@VehicleCategory",vehicle),//2
                                        };

                dt = DataAccess.getDataTable("Get_Schedule_DefaultJob", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ScheduleVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        public static string Get_ScheduleParts(string DealerCode, string VehicleCategory)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<ScheduleVM> lst = new List<ScheduleVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                 new SqlParameter("@VehicleCategory",VehicleCategory),//1
                 //new SqlParameter("@VehicleCategory",vehicle),//2
                                        };

                dt = DataAccess.getDataTable("Get_ScheduleParts", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ScheduleVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        //Save Job Get 
        public static string Get_ScheduleJobSave(string DealerCode, string ScheduleCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<ScheduleVM> lst = new List<ScheduleVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                 new SqlParameter("@ScheduleCode",ScheduleCode),//1
                 //new SqlParameter("@VehicleCategory",vehicle),//2
                                        };

                dt = DataAccess.getDataTable("Get_ScheduleJobSave", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ScheduleVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        //Save Part Get 
        public static string Get_SchedulePartsSave(string DealerCode, string ScheduleCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<ScheduleVM> lst = new List<ScheduleVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                 new SqlParameter("@ScheduleCode",ScheduleCode),//1
                 //new SqlParameter("@VehicleCategory",vehicle),//2
                                        };

                dt = DataAccess.getDataTable("Get_SchedulePartsSave", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ScheduleVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        //Save Lubricant Get 
        public static string Get_ScheduleLubeSave(string DealerCode, string ScheduleCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<ScheduleVM> lst = new List<ScheduleVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                 new SqlParameter("@ScheduleCode",ScheduleCode),//1
                 //new SqlParameter("@VehicleCategory",vehicle),//2
                                        };

                dt = DataAccess.getDataTable("Get_ScheduleLubeSave", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ScheduleVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        public static string Get_ScheduleLub(string DealerCode, string VehicleCategory)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<ScheduleVM> lst = new List<ScheduleVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                 new SqlParameter("@VehicleCategory",VehicleCategory),//1
                 //new SqlParameter("@VehicleCategory",vehicle),//2
                                        };

                dt = DataAccess.getDataTable("Get_ScheduleLub", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ScheduleVM>(dt);
                }

                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }
        //public static bool Insert_Schedule(ScheduleVM model, ref string msg)
        //{

        //    try
        //    {
        //        if (model.ScheduleCode == ""|| model.ScheduleCode == null)
        //        {
        //            strAutoCode = sysfun.AutoGen("MaintainenceSchedule", "ScheduleCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), model.DealerCode);
        //            //strAutoCode = sysfun.GetNewMaxID("ProdRecMaster", "RecNo", 8, "00166");
        //            //recDate = DateTime.Parse( model.RecDate);
        //        }
        //        else
        //        {

        //            strAutoCode = model.ScheduleCode;
        //            //string rec = model.DeliveryDate;
        //            //recDate = DateTime.Parse(rec);

        //        }
        //        SqlParameter[] param = {
        //                         new SqlParameter("@DealerCode",model.ScheduleCode),//0
        // new SqlParameter("@DeliveryNo",strAutoCode),//1
        // //new SqlParameter("@DeliveryDate",model.DeliveryDate),//2								 
        // new SqlParameter("@BookRefNo",model.BookRefNo),//3								 
        // new SqlParameter("@UpdUser",AuthBase.UserId),//4
        // new SqlParameter("@UpdTerm",General.CurrentIP),//5
        //                         new SqlParameter("@Type",model.Type),//6
        //                         new SqlParameter("@ReceiverCode",model.ReceiverCode),//7
        //                         new SqlParameter("@Remarks",model.Remarks),//8
        //                         new SqlParameter("@Segment",model.Segment),//9
        //                         new SqlParameter("@Usage",model.Usage),//10
        //                         new SqlParameter("@CusCode",model.CusCode),//11
        //                         new SqlParameter("@VehTypeCode",model.VehTypeCode),//12
        //                         new SqlParameter("@CusContNo",model.CusContNo),//13
        // new SqlParameter("@EmpCode",model.EmpCode)
        //                    };

        //        if (sysfun.ExecuteSP_NonQuery("SP_VehicleDeliveryMaster_Insert", param))
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

        public static bool Insert_ScheduleMaster(ScheduleVM model, ref string msg)
        {

            if (model.ScheduleCode == "" || model.ScheduleCode == null)
            {
                DealerCode = model.DealerCode;
                strAutoCode = sysfun.AutoGen("MaintainenceSchedule", "ScheduleCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), model.DealerCode);
            }
            else
            {
                DealerCode = model.DealerCode;
                strAutoCode = model.ScheduleCode;
            }
            SqlParameter[] param = {                                            
           /*0*/ new SqlParameter("@DealerCode",DealerCode),
           /*1*/ new SqlParameter("@ProdCode",model.ProdCode),         
           /*2*/ new SqlParameter("@BrandCode",model.BrandCode),        
           /*3*/ new SqlParameter("@ScheduleCode",strAutoCode),
           /*4*/  new SqlParameter("@KM",model.KM),
           /*5*/  new SqlParameter("@SDays",model.SDays),
           /*6*/  new SqlParameter("@ReminderLetter",model.ReminderLetter),
           /*7*/ new SqlParameter("@JobsTotal",model.JobsTotal),
           /*8*/ new SqlParameter("@PartsTotal",model.PartsTotal),
           /*9*/ new SqlParameter("@LubsTotal",model.LubsTotal),       
           /*10*/ new SqlParameter("@UpdUser",AuthBase.UserId),
           /*11*/ //new SqlParameter("@UpdDate",model.UpdDate),
           /*12*/ //new SqlParameter("@UpdTime",model.UpdTime),
           /*13*/ new SqlParameter("@UpdTerm",General.CurrentIP),
           /*14*/ new SqlParameter("@CallAlert",model.CallAlert),
           /*15*/ new SqlParameter("@SmsAlert",model.SmsAlert),
           /*15*/ new SqlParameter("@VehicleCategory",model.VehicleCategory)
        };


            //param[0].Value = Session["DealerCode"].ToString();
            //Auto Code Generation Decision on Insert 
            //for update CustomerEstimate




            try
            {
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    //For New Schedule

                    if (sysfun.ExecuteSP_NonQuery("[sp_Schedule_Master_Insert]", param, Trans))
                    {
                        IsSaved = true;
                        //bool a = (d1 > 0 ? Inser_JobCardDetail() : false);
                        //bool a = (rowsInJobCardDetail(gvJobCard) == true ? Inser_CustomerEstimateDetail() : false);
                        // Insert_ScheduleJobDetail();

                        //bool c = (d2 > 0 ? Inser_JobCardPartsDetail() : false);
                        //bool c = (rowInJobCardPartsDetail(gvJobCardParts) == true ? Inser_CustomerEstimePartsDetail() : false);
                        // Insert_SchedulePartsDetail();
                        //bool d = (d3 > 0 ? Inser_JobCardLubricanteDetail() : false);
                        //bool d = (rowInJobCardLubricanteDetail(gvLubParts) == true ? Inser_CustomerEstimateLubricates_Detail() : false);
                        // Insert_ScheduleLubricates_Detail();

                        //bool b = (d4 > 0 ? Inser_JobCardSubletDetail() : false);
                    }

                    //if (ObjTrans.CommittTransaction(ref Trans) == true)
                    //{
                    //    IsSaved = true;
                    //    //Insert_ScheduleJobDetail();
                    //    //Insert_SchedulePartsDetail();
                    //    //Insert_ScheduleLubricates_Detail();
                    //}
                    //else
                    //{
                    //    ObjTrans.RollBackTransaction(ref Trans);
                    //    IsSaved = false;
                    //    //SysFunc.UserMsg(lblMsg, Color.Red, "Record not saved Try again." + txtSchedule.Text, txtSchedule);
                    //}


                
            
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                ObjTrans.RollBackTransaction(ref Trans);
            }
            return IsSaved;

        }

        public static bool Insert_ScheduleTabJobs(List<ScheduleVM> model, ref string msg)
        {
            try
            {
                bool flag = false;
                //SqlParameter[] Inser_CustomerEstimate_Delete_param = {
                //                                                new SqlParameter("@DealerCode",model.DealerCode),
                //                                                new SqlParameter("@ScheduleCode",model.ScheduleCode)
                //                                            };
                //Inser_CustomerEstimate_Delete_param[0].Value = model.DealerCode;
                //Inser_CustomerEstimate_Delete_param[1].Value = strAutoCode;
                //if (sysfun.ExecuteSP_NonQuery("sp_W2_ScheduleJob_Detail_Delete", Inser_CustomerEstimate_Delete_param, Trans))
                //{
                if (model != null)
                {
                    foreach (var item in model)
                    {
                        SqlParameter[] param = {
                                        /*0*/ new SqlParameter("@DealerCode",DealerCode),
                                       /*1*/ new SqlParameter("@ProdCode",item.ProdCode),         
                                       /*2*/ new SqlParameter("@BrandCode",item.BrandCode),        
                                       /*3*/ new SqlParameter("@KM",item.KM),                                       
                                       /*4*/ new SqlParameter("@SDays",item.SDays), 
                                       /*5*/ new SqlParameter("@JobCode",item.JobCode),
                                       /*6*/ new SqlParameter("@ScheduleCode",strAutoCode),
                                       /*7*/ new SqlParameter("@Labor",item.Labor),
                                             new SqlParameter("@DefJobDesc",item.DefJobDesc),
                                             new SqlParameter("@EmpCode",item.EmpCode)

                           };



                        sysfun.ExecuteSP_NonQuery("sp_ScheduleJob_Detail_Insert", param, Trans);
                        IsSaved = true;

                    }
                }
                }
                

         //   }
                             
        catch (Exception ex)
           {
              ObjTrans.RollBackTransaction(ref Trans);
              msg=ex.Message;
               }
            
            return IsSaved;
}

        public static bool Insert_ScheduleTabParts(List<ScheduleVM> model, ref string msg)
        {
            try
            {
                bool flag = false;
                //SqlParameter[] CustomerEstimePartsDetail_Delete_param = {
                //                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                //                                                    new SqlParameter("@ScheduleCode",SqlDbType.Char,8)
                //                                                 };
                //CustomerEstimePartsDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                //CustomerEstimePartsDetail_Delete_param[1].Value = ScheduleCode.Trim();
                if (model != null)
                {
                    foreach (var item in model)
                    {
                        SqlParameter[] param = {                                                       
                                           /*0*/ new SqlParameter("@DealerCode",item.DealerCode),
                                           /*1*/ new SqlParameter("@ProdCode",item.ProdCode),         
                                           /*2*/ new SqlParameter("@BrandCode",item.BrandCode),        
                                           /*3*/ new SqlParameter("@KM",item.KM),                                       
                                           /*4*/ new SqlParameter("@SDays",item.SDays),
                                           /*5*/ new SqlParameter("@ItemCode",item.ItemCode),
                                           /*6*/ new SqlParameter("@PartNo",item.PartNo),
                                           /*7*/ new SqlParameter("@Qty",item.Qty),
                                           /*8*/ new SqlParameter("@Price",item.Price),
                                           /*9*/ new SqlParameter("@ScheduleCode",strAutoCode),
                                                 new SqlParameter("@PartDesc",item.PartsDesc),
                                                 new SqlParameter("@Total",item.Total)

                                                                     };

                        sysfun.ExecuteSP_NonQuery("sp_SchedulePartsDetail_Insert", param, Trans);
                        flag = true;
                    }
                }
                 }
                
            
            catch (Exception ex) {
                ObjTrans.RollBackTransaction(ref Trans);
                msg = ex.Message;
            }
         
            return IsSaved;
        }

        public static bool Insert_ScheduleTabLub(List<ScheduleVM> model, ref string msg)
        {
            try
            {
                bool flag = false;
                //SqlParameter[] CustomerEstimateLubricanteDetail_Delete_param = {
                //                                                        new SqlParameter("@DealerCode",model.DealerCode),
                //                                                        new SqlParameter("@ScheduleCode",model.ScheduleCode)
                //                                                      };
                //CustomerEstimateLubricanteDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                //CustomerEstimateLubricanteDetail_Delete_param[1].Value = ScheduleCode.Trim();
                if (model != null)
                { 
                foreach (var item in model)
                {
                    SqlParameter[] param = {                                                       
                                           /*0*/ new SqlParameter("@DealerCode",item.DealerCode),
                                           /*1*/ new SqlParameter("@ProdCode",item.ProdCode),         
                                           /*2*/ new SqlParameter("@BrandCode",item.BrandCode),        
                                           /*3*/ new SqlParameter("@KM",item.KM),                                       
                                           /*4*/ new SqlParameter("@SDays",item.SDays),
                                           /*5*/ new SqlParameter("@ItemCode",item.ItemCode),
                                           /*6*/ new SqlParameter("@PartNo",item.PartNo),
                                           /*7*/ new SqlParameter("@Qty",item.Qty),
                                           /*8*/ new SqlParameter("@Price",item.Price),
                                           /*9*/ new SqlParameter("@ScheduleCode",strAutoCode),
                                          /*10*/ new SqlParameter("@PartsDesc",item.PartsDesc),
                                                 new SqlParameter("@Total",item.Total)

                                                                     };
                    sysfun.ExecuteSP_NonQuery("sp_ScheduleLubricantDetail_Insert", param, Trans);
                    flag = true;
                }
            }
                
            }
            catch (Exception ex) {
                ObjTrans.RollBackTransaction(ref Trans);
                msg =ex.Message;
            }
            ObjTrans.CommittTransaction(ref Trans);
            return IsSaved;

        }
    }
}
