﻿using Core.CRM.ADO.ViewModel;
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
    public class VehicleMethods
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


        public static string Insert_VehicleMaster(VehicleVM model, string dealerCode)
        {
            string msg = "";
            try
            {

                if (sysfun.IsExist("BrandCode", model.BrandCode, "Vehicle", model.DealerCode, "and ProdCode = '" + model.ProdCode + "'and VersionCode = '" + model.VersionCode + "'") && model.VehicleCode == null)
                {
                    return msg = "Vehicle is already Exists";
                }


                if (model.VehicleCode == null || model.VehicleCode == "" || model.VehicleCode == "0")
                {
                    strAutoCode = sysfun.GetNewMaxIDWitOutDealerCode("Vehicle", "VehicleCode", 8, "");
                    //strAutoCode = sysfun.AutoGen("Vehicle", "VehicleCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), dealerCode);

                }
                else
                {
                    strAutoCode = model.VehicleCode;
                }

                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                   new SqlParameter("@BrandCode",SqlDbType.Char),//1
                                   new SqlParameter("@ProdCode",SqlDbType.VarChar),//2
                                   new SqlParameter("@VersionCode",SqlDbType.Char),//3
                                   new SqlParameter("@Color",SqlDbType.VarChar),//4
                                   new SqlParameter("@ColorCode",SqlDbType.Char),//5
                                   new SqlParameter("@ProdDesc",SqlDbType.VarChar),//6
                                   new SqlParameter("@ProdLongDesc",SqlDbType.VarChar),//7
                                   new SqlParameter("@Power",SqlDbType.VarChar),//8
                                   new SqlParameter("@CNGFlag",SqlDbType.Char),//9
                                   new SqlParameter("@CommPerc",SqlDbType.Float),//10
                                   new SqlParameter("@CommAmount",SqlDbType.Float),//11
                                   new SqlParameter("@UpdUser",SqlDbType.VarChar),//12
                                   new SqlParameter("@UpdTerm",SqlDbType.VarChar),//13
                                   new SqlParameter("@Version",SqlDbType.Char),//14
                                   new SqlParameter("@FFS",SqlDbType.Float),//15
                                   new SqlParameter("@PDI",SqlDbType.Float),//16
                                   new SqlParameter("@VehicleStatus",SqlDbType.VarChar),//17
                                   new SqlParameter("@CostPrice",SqlDbType.Float),//18
                                   new SqlParameter("@ProdTitle",SqlDbType.VarChar),//19
                                   new SqlParameter("@ReportingProdCode",SqlDbType.VarChar),//20
                                   new SqlParameter("@IsActive",SqlDbType.Char),//21
                                   new SqlParameter("@SalesAccountCode",SqlDbType.VarChar),//22
                                   new SqlParameter("@EffectiveStartDate",SqlDbType.DateTime),//23
                                   new SqlParameter("@EffectiveEndDate",SqlDbType.DateTime),//24
                                   new SqlParameter("@EnginePrefix",SqlDbType.VarChar),//25
                                   new SqlParameter("@SFS",SqlDbType.Int),//26
                                   new SqlParameter("@TFS",SqlDbType.Int),//27
                                   new SqlParameter("@VehTypeCode",SqlDbType.Char),//28
                                   new SqlParameter("@Seater",SqlDbType.TinyInt),//29
                                   new SqlParameter("@Compartment",SqlDbType.TinyInt),//30
                                   new SqlParameter("@VehiclePrefix",SqlDbType.VarChar),//31
                                   new SqlParameter("@Weight",SqlDbType.Int),//32
                                   new SqlParameter("@BuiltUnit",SqlDbType.Char),//33
                                   new SqlParameter("@WeightType",SqlDbType.VarChar),//34
                                   new SqlParameter("@MaximumVehicleWeight",SqlDbType.NVarChar),//35
                                   new SqlParameter("@MaximumWeightFrontAxle",SqlDbType.NVarChar),//36
                                   new SqlParameter("@MaximumWeightRearAxle",SqlDbType.NVarChar),//37
                                   new SqlParameter("@FrontWheel",SqlDbType.NVarChar),//38
                                   new SqlParameter("@RearWheel",SqlDbType.NVarChar),//39
                                   new SqlParameter("@WheelBase",SqlDbType.NVarChar),//40
                                   new SqlParameter("@SchedulePlan",SqlDbType.TinyInt),//41
                                   new SqlParameter("@FreeServiceCup",SqlDbType.TinyInt),//42
                                   new SqlParameter("@Remarks",SqlDbType.VarChar),//43
                                   new SqlParameter("@Pic",SqlDbType.VarBinary),//44
                                   new SqlParameter("@TransmissionType",SqlDbType.Char),//45
                                   new SqlParameter("@VehicleCode",SqlDbType.Char),//46
                                   new SqlParameter("@Segment",SqlDbType.Char),//47
                                   new SqlParameter("@MSPR",SqlDbType.Char),//48
                                   new SqlParameter("@VehicleCategory",SqlDbType.Char),//49
                                   new SqlParameter("@VehicleStock",SqlDbType.Char),//50
                                   new SqlParameter("@CostofSale",SqlDbType.Char),//51
                                   new SqlParameter("@Revenue",SqlDbType.Char),//52
                                   new SqlParameter("@OwnMoney",SqlDbType.Char),//53
                                   new SqlParameter("@DownMoney",SqlDbType.Char),//54
                                   new SqlParameter("@F4S",SqlDbType.Char),//55
                                   new SqlParameter("@F5S",SqlDbType.Char),//56
                               };

                param[0].Value = model.DealerCode;
                param[1].Value = model.BrandCode;
                param[2].Value = model.ProdCode;
                param[3].Value = model.VersionCode;
                param[4].Value = model.Color;
                param[5].Value = model.ColorCode;
                param[6].Value = model.ProdDesc;
                param[7].Value = model.ProdLongDesc;
                param[8].Value = model.Power;
                if (model.CNGFlag != "CNG")
                {
                    param[9].Value = "N";
                }
                else
                {
                    param[9].Value = "Y";
                }
                param[10].Value = Convert.ToDouble("0");
                param[11].Value = Convert.ToDouble("0");
                param[12].Value = AuthBase.UserId;
                param[13].Value = General.CurrentIP;
                param[14].Value = (object)DBNull.Value;

                if (model.FFS == null)
                    param[15].Value = Convert.ToDouble("0");
                else
                    param[15].Value = model.FFS;

                if (model.PDI == null)
                    param[16].Value = Convert.ToDouble("0");
                else
                    param[16].Value = model.PDI;

                param[17].Value = model.VehicleStatus;

                param[18].Value = Convert.ToDouble("0");
                param[19].Value = model.ProdTitle;
                param[20].Value = (object)DBNull.Value;
                param[21].Value = model.IsActive;
                param[22].Value = (object)DBNull.Value;
                param[23].Value = (object)DBNull.Value;
                param[24].Value = (object)DBNull.Value;
                param[25].Value = (object)DBNull.Value;

                if (model.SFS == null)
                    param[26].Value = Convert.ToDouble("0");
                else
                    param[26].Value = model.SFS;

                if (model.TFS == null)
                    param[27].Value = Convert.ToDouble("0");
                else
                    param[27].Value = model.TFS;

                param[28].Value = model.VehTypeCode;
                param[29].Value = Convert.ToByte(model.Seater == "" ? "0" : model.Seater);
                param[30].Value = Convert.ToByte(model.Compartment == "" ? "0" : model.Compartment);
                param[31].Value = model.VehiclePrefix;
                param[32].Value = Convert.ToInt64(model.Weight == "" ? "0" : model.Weight);

                param[33].Value =model.BuiltUnit ;
                param[34].Value = model.WeightType;
                param[35].Value = model.MaximumVehicleWeight;
                param[36].Value = model.MaximumWeightFrontAxle;
                param[37].Value = model.MaximumWeightRearAxle;
                param[38].Value = model.FrontWheel;
                param[39].Value = model.RearWheel;
                param[40].Value = model.WheelBase;



                param[41].Value = Convert.ToByte(model.SchedulePlan == "" ? "0" : model.SchedulePlan);
                param[42].Value = Convert.ToByte(model.FreeServiceCup == "Y" ? "1" : "0");

                param[43].Value = model.Remarks;
                param[44].Value = (object)DBNull.Value;
                param[45].Value = model.TransmissionType;
                param[46].Value = strAutoCode;
                param[47].Value = model.Segment;
                param[48].Value = model.MSPR;
                param[49].Value = model.VehicleCategory;
                param[50].Value = model.VehicleStock;
                param[51].Value = model.CostofSale;
                param[52].Value = model.Revenue;
                param[53].Value = model.OwnMoney;
                param[54].Value = model.DownMoney;
                param[55].Value = model.F4S;
                param[56].Value = model.F5S;


                if (sysfun.ExecuteSP_NonQuery("SP_Sales_Insert_Vehicle", param))
                {
                    IsSaved = true;
                    msg = "Completed";
                }


            }
            catch (Exception ex)
            {
                return ex.Message;
                //throw;
            }

            return msg;
        }

        public static bool Insert_VehicleTaxDetail(List<VehicleTaxDetailVM> model2, string dealerCode,ref string msg)
        {

            try
            {
                foreach (var item in model2)
                {
                    if (item.BrandCode.Trim() != "" || item.BrandCode.Trim() != null)
                    {
                        SqlParameter[] param2 = {
                                                new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                                new SqlParameter("@BrandCode",SqlDbType.Char),//1
                                                new SqlParameter("@ProdCode",SqlDbType.VarChar),//2
                                                new SqlParameter("@VersionCode",SqlDbType.Char),//3
                                                new SqlParameter("@ColorCode",SqlDbType.Char),//4
                                                new SqlParameter("@SalePrice",SqlDbType.Float),//5
                                                new SqlParameter("@Version",SqlDbType.Char),//6
                                                new SqlParameter("@VehicleStatus",SqlDbType.VarChar),//7
                                                new SqlParameter("@CostPrice",SqlDbType.Float),//8
                                                new SqlParameter("@WHTaxPerc",SqlDbType.Float),//9
                                                new SqlParameter("@WHTaxAmount",SqlDbType.Float),//10
                                                new SqlParameter("@FurTaxPer",SqlDbType.Float),//11
                                                new SqlParameter("@ExtraTaxPerc",SqlDbType.Float),//12
                                                new SqlParameter("@RegTaxPerc",SqlDbType.Float),//13
                                                new SqlParameter("@FederalTaxPerc",SqlDbType.Float),//14
                                                new SqlParameter("@FederalTaxAmount",SqlDbType.Float),//15
                                                new SqlParameter("@SEDTaxPerc",SqlDbType.Float),//16
                                                new SqlParameter("@SEDTaxAmount",SqlDbType.Float),//17
                                                new SqlParameter("@UpdUser",SqlDbType.VarChar),//18
                                                new SqlParameter("@UpdTerm",SqlDbType.VarChar),//19
                                                new SqlParameter("@IsActive",SqlDbType.Char),//20
                                                new SqlParameter("@VehicleCode",SqlDbType.Char),//21
                                                new SqlParameter("@RegType",SqlDbType.Char),//22
                                                new SqlParameter("@VehicleCategory",SqlDbType.Char),//22
                                                };

                        float WHTaxAmount = 0;

                        WHTaxAmount = (float.Parse(item.SalePrice) * float.Parse(item.WHTaxPerc) / 100);

                        param2[0].Value = item.DealerCode;
                        param2[1].Value = item.BrandCode;
                        param2[2].Value = item.ProdCode;
                        param2[3].Value = item.VersionCode;
                        param2[4].Value = item.ColorCode;
                        param2[5].Value = float.Parse(item.SalePrice);
                        param2[6].Value = (object)DBNull.Value;
                        param2[7].Value = item.VehicleStatus;
                        param2[8].Value = float.Parse(item.CostPrice);
                        param2[9].Value = float.Parse(item.WHTaxPerc);
                        param2[10].Value = WHTaxAmount;
                        param2[11].Value = (object)DBNull.Value;
                        param2[12].Value = float.Parse(item.ExtraTaxPerc);
                        param2[13].Value = float.Parse(item.RegTaxPerc);
                        param2[14].Value = (object)DBNull.Value;
                        param2[15].Value = (object)DBNull.Value;
                        param2[16].Value = (object)DBNull.Value;
                        param2[17].Value = (object)DBNull.Value;
                        param2[18].Value = AuthBase.UserId;
                        param2[19].Value = General.CurrentIP;
                        param2[20].Value = item.IsActive;
                        param2[21].Value = item.VehicleCode;
                        param2[22].Value = item.RegType;
                        param2[23].Value = item.VehicleCategory;
                        if (ObjTrans.BeginTransaction(ref Trans) == true)
                        {
                            sysfun.ExecuteSP_NonQuery("Sp_Insert_VehicleTaxDetail", param2, Trans);


                            IsSaved = true;
                        }

                    }
                    //count++;
                }



            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                msg = ex.Message;
            }
            
                ObjTrans.CommittTransaction(ref Trans);
            
            
            return IsSaved;
        }

        public static string Get_VehicleMasterData(string enquiryId, string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<VehicleVM> lst = new List<VehicleVM>();
            try
            {
                string sql = "Select * from Vehicle where DealerCode in( '" + dealerCode + "', 'COMON' )AND VehicleCode = '" + enquiryId + "'";
                dt = sysfun.GetData(sql);

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<VehicleVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        public static string Get_VehicleDetailData(string enquiryId, string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<VehicleTaxDetailVM> lst = new List<VehicleTaxDetailVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@VehicleCode",enquiryId)//0
									
									};


                dt = DataAccess.getDataTable("SP_Select_VehicleTaxDetail", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<VehicleTaxDetailVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        public static List<VehicleVM> GetVehicleModal(string dealerCode)
        {

            List<VehicleVM> lst = new List<VehicleVM>();
            try
            {
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_VehiclesForModal", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<VehicleVM>(dt);
                }


            }
            catch (Exception ex)
            {

                throw;
            }
            return lst;
        }

        public static string Get_VehicleCategory(string enquiryId, string dealerCode)
            {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();
            var Serializer = new JavaScriptSerializer();
            List<PrdouctVM> lst = new List<PrdouctVM>();
            try
            {
                string sql = "Select VehCategory from VehicleType where DealerCode in ('" + dealerCode + "','COMON') AND VehTypeCode = '" + enquiryId + "'";

                dt = sysfun.GetData(sql);

                json = dt.Rows[0]["VehCategory"].ToString();
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }


        public static List<VehicleVM> Get_AccDesc(string dealerCode)
        {
            DataTable dt = new DataTable();
            string json = "";
            var Serializer = new JavaScriptSerializer();
            // string TransType = "CSI";
            //string SaleType = "Cash";


            List<VehicleVM> lst = new List<VehicleVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@CompCode",dealerCode)//0
                                    // , new SqlParameter("@TransType",TransType)//0
                                    //, new SqlParameter("@SaleType",SaleType)//0
									};

                dt = DataAccess.getDataTable("SP_Select_AccountCode", sqlParam, General.GetFAMConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<VehicleVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }

        //abdussaboor //Delete Delete_ItemMaster
        public static bool Delete_VehicleMaster(VehicleVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                if (sysfun.IsExistwithoutDealer("ProdCode", model.ProdCode, "BookOrdVehDetail","And BrandCode='"+model.BrandCode+"' and VersionCode='"+model.VersionCode+"'"))
                {
                    IsSaved = false;
                    msg = "This Vehicle "+model.ProdDesc+" is  Exist in Booking Order !";
                    return IsSaved;
                }
                if (sysfun.IsExistwithoutDealer("ProdCode", model.ProdCode, "CustomerVehicle", "And BrandCode='" + model.BrandCode + "' and VersionCode='" + model.VersionCode + "'"))
                {
                    IsSaved = false;
                    msg = "This Vehicle " + model.ProdDesc + " is  Exist in Cutomer Vehicle !";
                    return IsSaved;
                }
                if (sysfun.IsExistwithoutDealer("ProdCode", model.ProdCode, "VehicleStock", "And BrandCode='" + model.BrandCode + "' and VersionCode='" + model.VersionCode + "'"))
                {
                    IsSaved = false;
                    msg = "This Vehicle " + model.ProdDesc + " is  Exist in Vehicle Stock  !";
                    return IsSaved;
                }

               
               
                //ASP.Net Start
                SqlParameter[] param = {

                                                new SqlParameter("@DealerCode",model.DealerCode),//0
                                                new SqlParameter("@BrandCode",model.BrandCode),//1
                                                new SqlParameter("@ProdCode",model.ProdCode),//2
                                                new SqlParameter("@VersionCode",model.VersionCode),//3
                                                new SqlParameter("@VehicleCode",model.VehicleCode),//3
                                   };
                //ASP.Net End
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));

                if (sysfun.ExecuteSP_NonQuery("SP_Delete_VehicleMaster", param))
                {
                    msg = "Record Delete Sucessfully";
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
        #region ------------------- Vehicle Return ---------------------

        public static List<VehicleReturnMasterVM> VehicleReturnModal(string dealerCode)
        {

            List<VehicleReturnMasterVM> lst = new List<VehicleReturnMasterVM>();
            try
            {
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_Select_VehicleReturn", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<VehicleReturnMasterVM>(dt);
                }


            }
            catch (Exception ex)
            {

                throw;
            }
            return lst;
        }

        public static List<VehicleStockVM> GetVehicleStockModal(string dealerCode)
        {

            List<VehicleStockVM> lst = new List<VehicleStockVM>();
            try
            {
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_ChassisNoForVehicleReturn", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<VehicleStockVM>(dt);
                }


            }
            catch (Exception ex)
            {

                throw;
            }
            return lst;
        }

        public static string GetDealerSupplierModal(string dealerCode, string type)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<DealerSupplierVM> lst = new List<DealerSupplierVM>();
            try
            {

                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };

                if (type == "OEM")
                {
                    dt = DataAccess.getDataTable("SP_VendorModal", sqlParam, General.GetBMSConString());

                }
                else
                {
                    dt = DataAccess.getDataTable("SP_DealerModal", sqlParam, General.GetBMSConString());
                }

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<DealerSupplierVM>(dt);
                }
                json = Serializer.Serialize(lst);

            }
            catch (Exception ex)
            {

                throw;
            }
            return json;
        }

        public static bool Insert_VehicleReturnMaster(VehicleReturnMasterVM model)
        {

            try
            {
                if (model.ReturnCode == "" || model.ReturnCode == null)
                {
                    strAutoCode = sysfun.AutoGen("VehicleReturnMaster", "ReturnCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"), model.DealerCode);
                }
                else
                {
                    strAutoCode = model.ReturnCode;
                }
                SqlParameter[] param = {
                                 new SqlParameter("@DealerCode",model.DealerCode),//0
								 new SqlParameter("@VendorCode",model.VendorCode),//1
								 new SqlParameter("@ReturnCode",strAutoCode),//2								 
								 new SqlParameter("@ReturnDate",sysfun.SaveDate(model.ReturnDate)),//3								 
								 new SqlParameter("@Remarks",model.Remarks),//4
								 new SqlParameter("@GatePass",model.GatePass),//5
								 new SqlParameter("@DelFlag",'N'),//6
								 new SqlParameter("@UpdUser",AuthBase.UserId),//7
								 new SqlParameter("@UpdDate",DateTime.Now),//8
								 new SqlParameter("@UpdTime",DateTime.Now),//9
								 new SqlParameter("@UpdTerm",General.CurrentIP),//10
								 new SqlParameter("@DealerCustomer",model.DealerCustomer),//11
								 new SqlParameter("@ReturnType",model.ReturnType),//12
								 new SqlParameter("@DocumentNo",model.DocumentNo),//13
                            };

                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    sysfun.ExecuteSP_NonQuery("SP_Inser_VehicleReturnMaster", param, Trans);

                    IsSaved = true;
                }

            }
            catch (Exception)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                throw;
            }

            return IsSaved;
        }

        public static bool Insert_VRDetail(List<VehicleReturnDetailVM> model2)
        {
            int count = 0;
            try
            {
                foreach (var item in model2)
                {
                    if (count >= 1 || item.BrandCode != null)
                    {
                        SqlParameter[] param2 = {
                                 new SqlParameter("@DealerCode",item.DealerCode),//0
								 new SqlParameter("@ReturnCode",strAutoCode),//1
                                 new SqlParameter("@Type",item.Type),//2
								 new SqlParameter("@BrandCode",item.BrandCode),//3								 
								 new SqlParameter("@ProdCode",item.ProdCode.Trim()),//4								 
								 new SqlParameter("@VersionCode",item.VersionCode),//5
								 new SqlParameter("@EngineNo",item.EngineNo.Trim()),//6
								 new SqlParameter("@ChasisNo",item.ChasisNo.Trim()),//7								 
								 new SqlParameter("@BookRefNo",item.BookRefNo),//8
								 new SqlParameter("@BookRefDate",item.BookRefDate == null ? DBNull.Value :sysfun.SaveDate(item.BookRefDate)),//9
                                 new SqlParameter("@CusCode",item.CusCode),//10
								 new SqlParameter("@BookingNo",item.BookingNo),//10
                                 new SqlParameter("@ColorCode",item.ColorCode),//12
								 
							};

                        if (sysfun.ExecuteSP_NonQuery("SP_Insert_VehicleReturnDetail", param2, Trans) == true)
                        {
                            string sql = "update VehicleStock set DeliveryFlag = 'Y' , InStockFlag = 'N' , OutDate = '" + General.CurrentDate + "' where" +
                                " BrandCode = '" + item.BrandCode + "' and ProdCode = '" + item.ProdCode + "' and VersionCode = '" + item.VersionCode + "' and EngineNo = '" + item.EngineNo + "'" +
                                " and ChasisNo = '" + item.ChasisNo + "' and DealerCode = '" + item.DealerCode + "'";

                            //if (sysfun.ExecuteQuery_NonQuery(sql, Trans))
                            //{

                            //}
                            //else
                            //{
                            //    ObjTrans.RollBackTransaction(ref Trans);
                            //    return false;
                            //}
                        }
                        else
                        {
                            ObjTrans.RollBackTransaction(ref Trans);
                            return false;
                        }
                    }
                    count++;
                }

                ObjTrans.CommittTransaction(ref Trans);
                IsSaved = true;
            }
            catch (Exception)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                throw;
            }

            return IsSaved;
        }

        public static string Get_VehicleReturnDetailData(string enquiryId, string dealerCode)
        {
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<VehicleReturnDetailVM> lst = new List<VehicleReturnDetailVM>();
            try
            {
                SqlParameter[] sqlParam = {
                                    new SqlParameter("@DealerCode",dealerCode),//1
									new SqlParameter("@ReturnCode",enquiryId)//0
									
									};

                dt = DataAccess.getDataTable("SP_Select_VehicleReturnDetail", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<VehicleReturnDetailVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        public static bool Delete_VehicleReturn_Record(string enquiryId, string dealerCode)
        {
            DataSet ds = new DataSet();

            SqlParameter[] param = {
                new SqlParameter("@DealerCode",dealerCode),
                new SqlParameter("@ReturnCode",enquiryId)
            };

            if (sysfun.ExecuteSP_NonQuery("sp_VehicleReturn_Delete", param))
            {
                IsDeleted = true;
            }
            else
            {
                IsDeleted = false;
            }

            return IsDeleted;
        }

        #endregion
    }
}
