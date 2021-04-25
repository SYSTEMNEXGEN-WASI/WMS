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
  public  class MasterFormsOEM_Methods
    {
        static OEMDealerVM response = new OEMDealerVM();
        static bool IsDeleted = false;
        static DataTable dt = new DataTable();
        static string max = string.Empty;
        static bool IsSaved = false;
        static SysFunction sysfun = new SysFunction();
        static SqlParameter[] nullSqlParam = null;
        static string strAutoCode = string.Empty;


        //abdussaboor //Get_ItemMaster
        public static List<MasterFormsOEM_VM> Get_ItemMaster(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<MasterFormsOEM_VM> lst = new List<MasterFormsOEM_VM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_ItemMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<MasterFormsOEM_VM>(dt);
                }
              //  Serializer.MaxJsonLength = 50000000;
                //json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return lst;
        }
        
        //abdussaboor //Get_ItemMaster2
        public static List<MasterFormsOEM_VM> Get_ItemMaster2(string DealerCode,string ItemCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<MasterFormsOEM_VM> lst = new List<MasterFormsOEM_VM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                 new SqlParameter("@ItemCode",ItemCode),//1
                                        };

                dt = DataAccess.getDataTable("Get_ItemMasterDetail", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<MasterFormsOEM_VM>(dt);
                }
                //  Serializer.MaxJsonLength = 50000000;
                //json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return lst;
        }

        //abdussaboor //Insert Insert_ItemMaster
        public static bool Insert_ItemMaster(MasterFormsOEM_VM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                if(sysfun.IsExist("PartItemNo",model.PartItemNo,"Item",model.DealerCode)) {
                    IsSaved = false;
                    msg = "Part Item No Is Already Exist!";
                    return IsSaved;
                }
                if (model.ItemCode == "" || model.ItemCode == null)
                {
                    max = sysfun.GetNewMaxIDAppend("Item", "ItemCode", 8, model.DealerCode,"and PartItemNo='"+model.PartItemNo+"'");
                }
                else
                {

                    max = model.ItemCode;

                }
                //ASP.Net Start
                SqlParameter[] param = {

                                    new SqlParameter("@DealerCode",model.DealerCode),//0
                                    new SqlParameter("@ItemCatCode",model.ItemCatCode),//1
                                    new SqlParameter("@ItemCode",max),//2
                                    new SqlParameter("@PartItemNo",model.PartItemNo),//3
                                    new SqlParameter("@ItemDesc",model.ItemDesc),//4
                                    new SqlParameter("@UnitCode",model.UnitCode),//5
                                    new SqlParameter("@ProdCode",model.ProdCode),//6
                                    new SqlParameter("@PartsTransLocCode",""),//7
                                    new SqlParameter("@PurchaseRate",float.Parse(model.PurchaseRate)),//8
                                    new SqlParameter("@SaleRate",float.Parse(model.SaleRate)),//9
                                    new SqlParameter("@EssentialFlag","Y"),//10
                                    new SqlParameter("@Source",model.Source),//11
                                    new SqlParameter("@MinLevel",float.Parse(model.MinLevel)),//12
                                    new SqlParameter("@Reorder",float.Parse(model.Reorder)),//13
                                    new SqlParameter("@Warranty","N"),//14
                                    new SqlParameter("@WarrMonths",0),//15
                                    new SqlParameter("@WarrRemarks",""),//16
                                    new SqlParameter("@LubeFlag",model.LubeFlag),//17
                                    new SqlParameter("@AccountCode",""),//18
                                    new SqlParameter("@UpdUser",AuthBase.UserId),//19
                                    new SqlParameter("@UpdTerm", General.CurrentIP),//20
                                    new SqlParameter("@SEDPerc",0),//21
                                    new SqlParameter("@OHQTY",Convert.ToDecimal(0)),//22
                                    new SqlParameter("@OHStock",Convert.ToDecimal(0)),//23
                                    new SqlParameter("@War",Convert.ToDecimal(model.War)),//24
                                    new SqlParameter("@Critical","Y"),//25
                                    new SqlParameter("@PackingSize",Convert.ToInt32(model.PackingSize)),//26
                                    new SqlParameter("@Access",model.Access),//27
                                    new SqlParameter("@SGOFlag","N"),//28
                                    new SqlParameter("@Liter",model.Liter),//29
                                    new SqlParameter("@invType","N"),//30
                                    new SqlParameter("@FGSTPerc",Convert.ToDecimal(0)),//31
                                    new SqlParameter("@GSTPerc",Convert.ToDecimal(0)),//32
                                    new SqlParameter("@SalesExTaxPerc",0),//33
                                    new SqlParameter("@Genuine",""),//34
                                    new SqlParameter("@Running",""),//35
                                    new SqlParameter("@PFGSTPERC",Convert.ToDecimal(model.PFGSTPERC)),//36
                                    new SqlParameter("@PGSTPERC",Convert.ToDecimal(model.PGSTPERC)),//37
                                    new SqlParameter("@ExTaxPerc",Convert.ToDecimal(model.ExTaxPerc)),//38
                                    new SqlParameter("@Active",model.Active),//39
                                    new SqlParameter("@InventoryItem",model.InventoryItem),//40
                                    new SqlParameter("@SaleItem",model.SaleItem),//41
                                    new SqlParameter("@COGS",model.COGS),//42
                                    new SqlParameter("@StockInAccount",model.StockInAccount),//43
                                    new SqlParameter("@RevenueAccount",""),//44
                                    new SqlParameter("@StockOutAccount",model.StockOutAccount),//45
                                    new SqlParameter("@SalePromotionPoint",model.SalePromotionAccount),//46
                                    new SqlParameter("@PurchasePromotionPoint",model.PurshasePromotionPoint),//47
                                    new SqlParameter("@UOS",model.UOS),//48
                                   };
                //ASP.Net End
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));

                if (sysfun.ExecuteSP_NonQuery("Sp_Insert_Item", param))
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

        //abdussaboor //Delete Delete_ItemMaster
        public static bool Delete_ItemMaster(MasterFormsOEM_VM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                if (sysfun.IsExist("PartItemNo", model.ItemCode, "Item", model.DealerCode))
                {
                    IsSaved = false;
                    msg = "Part Item Number Is Already Exist!";
                    return IsSaved;
                }

                if (sysfun.IsExist("ItemCode", model.ItemCode, "PODetail", model.DealerCode))
                {
                    IsSaved = false;
                    msg= "Record Can Not Delete Because It's Exists In PO Form";
                    return IsSaved;
                }
                if (sysfun.IsExist("ItemCode", model.ItemCode, "JobCardPartsDetail", model.DealerCode))
                {
                    IsSaved = false;
                    msg = "Record Can Not Delete Because It's Exists In Job Card Form";
                    return IsSaved;
                    
                }
                if (sysfun.IsExist("ItemCode", model.ItemCode, "CounterSaleDetail", model.DealerCode))
                {
                    IsSaved = false;
                    msg = "Record Can Not Delete Because It's Exists In Counter Sales Form";
                    return IsSaved;
                }
                if (sysfun.IsExist("ItemCode", model.ItemCode, "ItemStock", model.DealerCode))
                {
                    IsSaved = false;
                    msg = "Record Can Not Delete Because It's Exists In Item Stock Form";
                    return IsSaved;
                }
                if (model.ItemCode == "" || model.ItemCode == null)
                {
                    max = sysfun.GetNewMaxIDAppend("Item", "ItemCode", 8, model.DealerCode, "and PartItemNo='" + model.PartItemNo + "'");
                }
                else
                {

                    max = model.ItemCode;

                }
                //ASP.Net Start
                SqlParameter[] param = {

                                    new SqlParameter("@DealerCode",model.DealerCode),//0                                  
                                    new SqlParameter("@ItemCode",model.ItemCode),//2
                                    new SqlParameter("@PartItemNo",model.PartItemNo),//3
                                    //new SqlParameter("@ItemDesc",model.ItemDesc),//4
                                    //new SqlParameter("@UnitCode",model.UnitCode),//5
                                    //new SqlParameter("@ProdCode",model.ProdCode),//6
                                    //new SqlParameter("@PartsTransLocCode",""),//7
                                    //new SqlParameter("@PurchaseRate",float.Parse(model.PurchaseRate)),//8
                                    //new SqlParameter("@SaleRate",float.Parse(model.SaleRate)),//9
                                    //new SqlParameter("@EssentialFlag","Y"),//10
                                    //new SqlParameter("@Source",model.Source),//11
                                    //new SqlParameter("@MinLevel",float.Parse(model.MinLevel)),//12
                                    //new SqlParameter("@Reorder",float.Parse(model.Reorder)),//13
                                    //new SqlParameter("@Warranty","N"),//14
                                    //new SqlParameter("@WarrMonths",0),//15
                                    //new SqlParameter("@WarrRemarks",""),//16
                                    //new SqlParameter("@LubeFlag",model.LubeFlag),//17
                                    //new SqlParameter("@AccountCode",""),//18
                                    new SqlParameter("@UpdUser",AuthBase.UserId),//19
                                    new SqlParameter("@UpdTerm", General.CurrentIP),//20
                                    //new SqlParameter("@SEDPerc",0),//21
                                    //new SqlParameter("@OHQTY",Convert.ToDecimal(0)),//22
                                    //new SqlParameter("@OHStock",Convert.ToDecimal(0)),//23
                                    //new SqlParameter("@War",Convert.ToDecimal(model.War)),//24
                                    //new SqlParameter("@Critical","Y"),//25
                                    //new SqlParameter("@PackingSize",Convert.ToInt32(model.PackingSize)),//26
                                    //new SqlParameter("@Access",model.Access),//27
                                    //new SqlParameter("@SGOFlag","N"),//28
                                    //new SqlParameter("@Liter",model.Liter),//29
                                    //new SqlParameter("@invType","N"),//30
                                    //new SqlParameter("@FGSTPerc",Convert.ToDecimal(0)),//31
                                    //new SqlParameter("@GSTPerc",Convert.ToDecimal(0)),//32
                                    //new SqlParameter("@SalesExTaxPerc",0),//33
                                    //new SqlParameter("@Genuine",""),//34
                                    //new SqlParameter("@Running",""),//35
                                    //new SqlParameter("@PFGSTPERC",Convert.ToDecimal(model.PFGSTPERC)),//36
                                    //new SqlParameter("@PGSTPERC",Convert.ToDecimal(model.PGSTPERC)),//37
                                    //new SqlParameter("@ExTaxPerc",Convert.ToDecimal(model.ExTaxPerc)),//38
                                    //new SqlParameter("@Active",model.Active),//39
                                    //new SqlParameter("@InventoryItem",model.InventoryItem),//40
                                    //new SqlParameter("@SaleItem",model.SaleItem),//41
                                    //new SqlParameter("@COGS",model.COGS),//42
                                    //new SqlParameter("@StockInAccount",model.StockInAccount),//43
                                    //new SqlParameter("@RevenueAccount",""),//44
                                    //new SqlParameter("@StockOutAccount",model.StockOutAccount),//45
                                    //new SqlParameter("@SalePromotionPoint",model.SalePromotionAccount),//46
                                    //new SqlParameter("@PurchasePromotionPoint",model.PurshasePromotionPoint),//47
                                    //new SqlParameter("@UOS",model.UOS),//48
                                   };
                //ASP.Net End
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));

                if (sysfun.ExecuteSP_NonQuery("SP_Delete_Item", param))
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

        //abdussaboor //Get_DesignationMaster
        public static string Get_DesignationMaster(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<MasterFormsOEM_VM> lst = new List<MasterFormsOEM_VM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_DesignationMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<MasterFormsOEM_VM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        //abdussaboor //Insert Insert_DesignationMaster
        public static bool Insert_DesignationMaster(MasterFormsOEM_VM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                if (model.DesigCode == "" || model.DesigCode == null)
                {
                    max = sysfun.GetNewMaxID("Designation", "DesigCode", 3, model.DealerCode);
                }
                else
                {

                    max = model.DesigCode;

                }
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@ItemCatCode",max),//1
                                   new SqlParameter("@ItemCatDesc",model.ItemCatDesc),//3
                                   new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                   new SqlParameter("@UpdTerm",General.CurrentIP),//5
                                   //new SqlParameter("@SortKey",sort),//6
                                   new SqlParameter("@VehicleCategory",model.VehicleCategory),//7
                                 };

                if (sysfun.ExecuteSP_NonQuery("SP_InsertDesig", param))
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

        //abdussaboor //Get_DepartmentMaster
        public static string Get_DepartmentMaster(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<MasterFormsOEM_VM> lst = new List<MasterFormsOEM_VM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_DepartmentMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<MasterFormsOEM_VM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        //abdussaboor //Insert Insert_DepartmentMaster
        public static bool Insert_DepartmentMaster(MasterFormsOEM_VM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                if (model.DeptCode == "" || model.DeptCode == null)
                {
                    max = sysfun.GetNewMaxID("Department", "DeptCode", 3, model.DealerCode);
                }
                else
                {

                    max = model.DeptCode;

                }
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@DeptCode",max),//1
                                   new SqlParameter("@DeptDesc",model.ItemCatDesc),//3
                                   new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                   new SqlParameter("@UpdTerm",General.CurrentIP),//5
                                   //new SqlParameter("@SortKey",sort),//6
                                   //new SqlParameter("@VehicleCategory",model.VehicleCategory),//7
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

        //abdussaboor //Get_PaymentmodeMaster
        public static string Get_PaymentmodeMaster(string DealerCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<MasterFormsOEM_VM> lst = new List<MasterFormsOEM_VM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                                        };

                dt = DataAccess.getDataTable("Get_PaymentModeMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<MasterFormsOEM_VM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                throw;
            }

            return json;
        }

        //abdussaboor //Insert_PaymentmodeMaster
        public static bool Insert_PaymentmodeMaster(MasterFormsOEM_VM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {
                if (model.PayModeCode == "" || model.PayModeCode == null)
                {
                    //max = sysfun.GetNewMaxID("PaymentMode", "PayModeCode", 3, model.PayModeCode);
                }
                else
                {

                    max = model.PayModeCode;

                }
                //int sort = Convert.ToInt16(sysfun.GetNewMaxID("JobTypeMaster", "SortKey", 3, model.DealerCode));
                SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",model.DealerCode),//0
                                   new SqlParameter("@PayModeCode",max),//1
                                   new SqlParameter("@PayModeDesc",model.PayModeDesc),//3
                                   new SqlParameter("@UpdUser",AuthBase.UserId),//4
                                   new SqlParameter("@UpdTerm",General.CurrentIP),//5
                                   //new SqlParameter("@SortKey",sort),//6
                                   new SqlParameter("@Version",model.VehicleCategory),//7
                                   new SqlParameter("@PayModeCodePSMC","")//8
                                 };

                if (sysfun.ExecuteSP_NonQuery("SP_InsertPayMode", param))
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


    }
}
