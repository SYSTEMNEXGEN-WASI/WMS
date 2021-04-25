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
    public class GeneralMethods
    
    {
        public static List<SelectListItem> GetDataFromSPWithDealerCode(string sp, string DealerCode)
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt;
            List<StringNameValueClass> lst = new List<StringNameValueClass>();

            try
            {
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",DealerCode)
               };
                dt = DataAccess.getDataTable(sp, sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                //item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                throw;
            }
            return item;
        }
        public static List<SelectListItem> GetDataFromSp(string sp)
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {

                   
                };
                dt = DataAccess.getDataTable(sp, sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString().ToUpper(),
                    Text = i.Title.ToUpper()
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "All", Text = "All" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }


        public static List<SelectListItem> GetDataFromSpWithDealerCode(string sp, string dealerCode)
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {
                                        new SqlParameter("@DealerCode", dealerCode)
                   
                };
                dt = DataAccess.getDataTable(sp, sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetLeadId()
        {
            List<SelectListItem> itemLeadId = new List<SelectListItem>();
            DataTable dt = new DataTable();
            string dealerCode = "00166";
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("Select_EnquiryId", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                itemLeadId = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                itemLeadId.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return itemLeadId;
        }

        public static List<SelectListItem> GetLeadType()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            string dealerCode = "00166";
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("Select_ProspectType", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetLeadMode()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            string dealerCode = "00166";
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("Select_EnquiryMode", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetLeadSource()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            string dealerCode = "00166";
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("Select_EnquirySource", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetVehicaleType()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            SysFunction sysfun = new SysFunction();

            string dealerCode = "COMMON";
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("Select_VehicleType", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetReceiptType()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            string dealerCode = "COMMON";
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("Select_ReceiptType", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetDealerEmployee()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            string dealerCode = "00166";
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("Select_DealerEmp", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetVehicleSegments()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {
                };
                dt = DataAccess.getDataTable("Select_VehicleSegments", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetBank()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {
                };
                dt = DataAccess.getDataTable("Select_Bank", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetCustomers()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            string dealerCode = "00166";
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("Select_Customer", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetCountry()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {
                   
                };
                dt = DataAccess.getDataTable("Select_Country", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetCity()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {
                   
                };
                dt = DataAccess.getDataTable("Select_City", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetBrands()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {
                   
                };
                dt = DataAccess.getDataTable("Select_Brand", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetVendor()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {

                };
                dt = DataAccess.getDataTable("Select_Vendor", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetProduct( )
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {
                    // new SqlParameter("@VehicleCategory",Id)

                };
                dt = DataAccess.getDataTable("Select_Product", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }
        public static List<SelectListItem> GetProductCategory(string Id)
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {

                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                     new SqlParameter("@VehicleCategory",Id)

                };
                dt = DataAccess.getDataTable("Select_Product_Category", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetColor(string Cat)
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {
                     new SqlParameter("@VehicleCategory",Cat),

                };
                dt = DataAccess.getDataTable("Select_Color", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetStatus()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {
                   
                };
                dt = DataAccess.getDataTable("Select_Status", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetTaskType()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {
                   
                };
                dt = DataAccess.getDataTable("Select_TaskType", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetStatusType()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {
                   
                };
                dt = DataAccess.getDataTable("Select_StatusType", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetLostReason()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {
                   
                };
                dt = DataAccess.getDataTable("Select_LostReason", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetSubjects()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {
                   
                };
                dt = DataAccess.getDataTable("Select_Subjects", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetTaskId()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {
                   
                };
                dt = DataAccess.getDataTable("Select_TaskId", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetLeadSourceID()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            List<StringNameValueClass> lst = new List<StringNameValueClass>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam = 
                {
                   
                };
                dt = DataAccess.getDataTable("Select_LeadSource", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }

        public static List<SelectListItem> GetEvent()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            List<StringNameValueClass> lst = new List<StringNameValueClass>();

            string dealerCode = "00166";
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode)
                };
                dt = DataAccess.getDataTable("SP_SelectEvent", sqlParam, General.GetBMSConString());
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<StringNameValueClass>(dt);
                }
                //json = Serializer.Serialize(lst);

                item = lst.Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Title
                }).ToList();

                item.Insert(0, new SelectListItem() { Value = "0", Text = "Select" });

            }
            catch (Exception ex)
            {

                //throw;
            }
            return item;
        }
        public static List<DealerInfoVM> LoginRequestSecurity(string user, string pass, ref string msg)
        {
            SysFunction myFunc = new SysFunction();
            SecurityBll sec = new SecurityBll();
            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<DealerInfoVM> lst = new List<DealerInfoVM>();
            DataTable dtLoginVerification = new DataTable();
            if (!sec.LoginVerification(user, pass, ref dtLoginVerification))
            {


                msg = "Invalid User ID or Password, Try again or You are not Allowed to Login. Please contact to administrator";
                //msg.ForeColor = System.Drawing.Color.Red;
            }
            else
            {


                try
                {
                    DataTable dt = dtLoginVerification;
                    if (dt == null)
                    {
                        msg = "Invalid Connection Please Contact to Administrator";
                        return lst;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        lst = EnumerableExtension.ToList<DealerInfoVM>(dt);
                        json = Serializer.Serialize(lst);
                    }

                }
                catch (Exception ex)
                {

                    throw;
                }


            }
            return lst;
        }

        public static List<DealerInfoVM> LoginRequest(string user , string pass ,ref string msg)
        {
            SysFunction myFunc = new SysFunction();        

            string json = "";
            var Serializer = new JavaScriptSerializer();
            List<DealerInfoVM> lst = new List<DealerInfoVM>();
            try
            {
                DataTable dt = myFunc.GetData("select A.DealerCode,A.UserID ,A.UserName,A.ID,B.DealerDesc,B.Address1 ,B.Address2,B.Address3,B.Email,B.Fax,B.Phone1 ,B.NTN,B.Phone2,B.SaleTaxNo,B.Image,B.Logo,B.VehicleCategory,C.EmpCode,C.EmpName from SecurityUser A inner join DealerEmp C On A.DealerCode = C.DealerCode and C.EmpCode = A.EmpCode inner join Dealer B On A.DealerCode=B.DealerCode  where A.UserName = '" + user + "' and A.Password='" + pass + "' And A.Active='Y'");
                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<DealerInfoVM>(dt);
                }
                json = Serializer.Serialize(lst);
            }
            catch (Exception ex)
            {

                msg = ex.Message;
            }

            return lst;

        }

        public static string GetDealerCode()
        {
            throw new NotImplementedException();
        }

        public static string ChassisEngionFromRegNo(string RegNo, string dealerCode)
        {
            string json = "";
            List<SelectListItem> item = new List<SelectListItem>();
            DataTable dt = new DataTable();
            var Serializer = new JavaScriptSerializer();
            List<JobCardHistoryVM> lst = new List<JobCardHistoryVM>();
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@DealerCode",dealerCode),
                    new SqlParameter("@RegNo",RegNo)
                };
                dt = DataAccess.getDataTable("sp_2W_LoadChassisEngineRegNo_RegNoParameter_History", sqlParam, General.GetBMSConString());

                if (dt.Rows.Count > 0)
                {
                    lst = EnumerableExtension.ToList<JobCardHistoryVM>(dt);
                }
                json = Serializer.Serialize(lst);

            }
            catch (Exception ex)
            {

                //throw;
            }
            return json;
        }


    }
}
