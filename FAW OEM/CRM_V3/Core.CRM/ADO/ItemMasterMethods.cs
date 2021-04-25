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
    public class ItemMasterMethods
    {
        static OEMDealerVM response = new OEMDealerVM();
        static bool IsDeleted = false;
        static DataTable dt = new DataTable();
        static string max = string.Empty;
        static bool IsSaved = false;
        static SysFunction sysfun = new SysFunction();
        static SqlParameter[] nullSqlParam = null;
        static string strAutoCode = string.Empty;
        //abdussaboor //Get_Product
        public static List<ItemMasterVM> Get_Product(string DealerCode, string vehiclecategory)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<ItemMasterVM> lst = new List<ItemMasterVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                 new SqlParameter("@VehicleCategory",vehiclecategory),//0
                                        };

                dt = DataAccess.getDataTable("Get_Product", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ItemMasterVM>(dt);
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
        //abdussaboor //Get_ItemMaster
        public static List<ItemMasterVM> Get_ItemMaster(string DealerCode,string VehicleCategory)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<ItemMasterVM> lst = new List<ItemMasterVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                  new SqlParameter("@VehicleCategory",VehicleCategory),//0
                                        };

                dt = DataAccess.getDataTable("Get_ItemMaster", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ItemMasterVM>(dt);
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
        public static List<ItemMasterVM> Get_ItemMaster2(string DealerCode, string ItemCode)
        {
            string json = "";
            var Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<ItemMasterVM> lst = new List<ItemMasterVM>();
            try
            {
                SqlParameter[] sqlParam = {
                 new SqlParameter("@DealerCode",DealerCode),//0
                 new SqlParameter("@ItemCode",ItemCode),//1
                                        };

                dt = DataAccess.getDataTable("Get_ItemMasterDetail", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<ItemMasterVM>(dt);
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
        //abdussaboor //Delete Delete_ItemMaster
        public static bool Delete_ItemMaster(ItemMasterVM model, ref string msg)
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
                    msg = "Record Can Not Delete Because It's Exists In PO Form";
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
        //abdussaboor //Insert Insert_ItemMaster
        public static bool Insert_ItemMaster(ItemMasterVM model, ref string msg)
        {
            string strAutoCode = string.Empty;
            try
            {

                if (model.ItemCode == "" || model.ItemCode == null)
                {
                    max = sysfun.GetNewMaxIDWitOutDealerCode("Item", "ItemCode", 8,  "");
                    if (sysfun.IsExist("PartItemNo", model.PartItemNo, "Item", model.DealerCode))
                    {
                        IsSaved = false;
                        msg = "Part Item No Is Already Exist!";
                        return IsSaved;
                    }
                    if (sysfun.IsExist("ItemDesc", model.ItemDesc, "Item", model.DealerCode))
                    {
                        IsSaved = false;
                        msg = "Item Description Is Already Exist!";
                        return IsSaved;
                    }
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
                                    new SqlParameter("@PartsTransLocCode",model.PartsTransLocCode),//7
                                    new SqlParameter("@PurchaseRate",Math.Round(float.Parse((model.PurchaseRate== null ? "0" : model.PurchaseRate)),2)),//8
                                    new SqlParameter("@SaleRate",Math.Round(float.Parse((model.SaleRate== null ? "0" : model.SaleRate)),2)),//9
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
                                    new SqlParameter("@FGSTPerc",Convert.ToDecimal(model.FGSTPerc)),//31
                                    new SqlParameter("@GSTPerc",Convert.ToDecimal(model.GSTPerc)),//32
                                    new SqlParameter("@SalesExTaxPerc",Convert.ToDecimal(model.ExTaxPerc)),//33
                                    new SqlParameter("@Genuine","Y"),//34
                                    new SqlParameter("@Running","Y"),//35
                                    new SqlParameter("@PFGSTPERC",Convert.ToDecimal(model.PFGSTPERC)),//36
                                    new SqlParameter("@PGSTPERC",Convert.ToDecimal(model.PGSTPERC)),//37
                                    new SqlParameter("@ExTaxPerc",Convert.ToDecimal(model.ExTaxPerc)),//38
                                    new SqlParameter("@Active",model.Active),//39
                                    new SqlParameter("@InventoryItem",model.InventoryItem),//40
                                    new SqlParameter("@SaleItem",model.SaleItem),//41
                                    new SqlParameter("@COGS",model.COGS),//42
                                    new SqlParameter("@StockInAccount",model.StockInAccount),//43
                                    new SqlParameter("@RevenueAccount",model.RevenueAccount),//44
                                    new SqlParameter("@StockOutAccount",model.StockOutAccount),//45
                                    new SqlParameter("@SalePromotionPoint",model.SalePromotionAccount),//46
                                    new SqlParameter("@PurchasePromotionPoint",model.PurshasePromotionPoint),//47
                                    new SqlParameter("@UOS",model.UOS),//48
                                    new SqlParameter("@VehicleCategory",model.VehicleCategory),//49
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
        public static List<AccountVM> Get_AccDesc(string dealerCode)
        {
            DataTable dt = new DataTable();
            string json = "";
            var Serializer = new JavaScriptSerializer();
            // string TransType = "CSI";
            //string SaleType = "Cash";


            List<AccountVM> lst = new List<AccountVM>();
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
                    lst = EnumerableExtension.ToList<AccountVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {
                throw;
            }

            return lst;
        }
    }
}
