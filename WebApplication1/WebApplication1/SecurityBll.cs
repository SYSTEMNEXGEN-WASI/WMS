using CConn;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DXBMS.Model;

namespace DXBMS
{
    public class SecurityBll
    {
        static DataTable SecurityDT = new DataTable();
        static DataTable SecurityDT_Add = new DataTable();
        static DataTable SecurityDT_Edit = new DataTable();
        static DataTable SecurityDT_Delete = new DataTable();
        static DataTable SecurityDT_Post = new DataTable();
        static DataTable SecurityDT_Print = new DataTable();

        static DataTable dt = new DataTable();
        static string LoginName;
        static string CompanyCode;
        static string ApplicationCode;
        public bool LoginVerification(string username, string password, ref DataTable dt)
        {
            try
            {
              
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@LoginName",username),
                    new SqlParameter("@Password",password)
                };
                dt = DataAccess.getDataTable("SP_LoginVerification", sqlParam, CConnection.GetSecurityConnectionString());

                if (dt != null && dt.Rows.Count > 0)
                {

                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        public bool FormsVerification(string LoginName, string CompanyCode, string ApplicationCode, ref DataTable dt)
        {
            try
            {
                List<FormVerification> lst = new List<FormVerification>();
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@LoginName",LoginName),
                    new SqlParameter("@CompCode",CompanyCode),
                    new SqlParameter("@ApplicationCode",ApplicationCode)
                };
                dt = DataAccess.getDataTable("SP_FormsVerification", sqlParam, CConnection.GetSecurityConnectionString());

                if (dt != null && dt.Rows.Count > 0)
                {

                    lst = EnumerableExtension.ToList<FormVerification>(dt);
                    if (lst.Count > 0)
                    {
                        HttpContext.Current.Session["UserRights"] = lst;
                        SecurityDT = dt;
                    }
                  
                   // FormsActionVerification(LoginName, CompanyCode, ApplicationCode);
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        public bool FormsActionVerification(string LoginName, string CompanyCode, string ApplicationCode)
        {
            try
            {
                //var Serializer = new JavaScriptSerializer();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@LoginName",LoginName),
                    new SqlParameter("@CompCode",CompanyCode),
                    new SqlParameter("@ApplicationCode",ApplicationCode)
                };
                dt = DataAccess.getDataTable("SP_FormsActionVerification", sqlParam, CConnection.GetSecurityConnectionString());
                SqlParameter[] sqlParam1 =
               {
                    new SqlParameter("@LoginName",LoginName),
                    new SqlParameter("@CompCode",CompanyCode),
                    new SqlParameter("@ApplicationCode",ApplicationCode)
                };
                SecurityDT_Add = DataAccess.getDataTable("SP_FormsActionVerification_Add", sqlParam1, CConnection.GetSecurityConnectionString());
                SqlParameter[] sqlParam2 =
               {
                    new SqlParameter("@LoginName",LoginName),
                    new SqlParameter("@CompCode",CompanyCode),
                    new SqlParameter("@ApplicationCode",ApplicationCode)
                };
                SecurityDT_Edit = DataAccess.getDataTable("SP_FormsActionVerification_Edit", sqlParam2, CConnection.GetSecurityConnectionString());
                SqlParameter[] sqlParam3 =
               {
                    new SqlParameter("@LoginName",LoginName),
                    new SqlParameter("@CompCode",CompanyCode),
                    new SqlParameter("@ApplicationCode",ApplicationCode)
                };
                SecurityDT_Delete = DataAccess.getDataTable("SP_FormsActionVerification_Delete", sqlParam3, CConnection.GetSecurityConnectionString());
                SqlParameter[] sqlParam4 =
               {
                    new SqlParameter("@LoginName",LoginName),
                    new SqlParameter("@CompCode",CompanyCode),
                    new SqlParameter("@ApplicationCode",ApplicationCode)
                };
                SecurityDT_Post = DataAccess.getDataTable("SP_FormsActionVerification_Post", sqlParam4, CConnection.GetSecurityConnectionString());
                SqlParameter[] sqlParam5 =
               {
                    new SqlParameter("@LoginName",LoginName),
                    new SqlParameter("@CompCode",CompanyCode),
                    new SqlParameter("@ApplicationCode",ApplicationCode)
                };
                SecurityDT_Print = DataAccess.getDataTable("SP_FormsActionVerification_Print", sqlParam5, CConnection.GetSecurityConnectionString());

                if (dt != null && dt.Rows.Count > 0)
                {
                    //SecurityDT_Add = dt;
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }
        public void Check_Action_ImageButton(string Formname, ImageButton btn, string action)
        {
            if (SecurityDT != null)
            {
                DataRow[] drr = SecurityDT.Select();
                for (int i = 0; i < drr.Length; i++)
                {

                    if (SecurityDT_Add.Rows[i]["FormName"].ToString().ToUpper() == Formname.ToUpper() && SecurityDT.Rows[i]["FormActionName"].ToString().ToUpper() == action.ToUpper())
                    {
                        btn.Enabled = true;
                    }
                }
            }
        }
        public void Check_Action(string Formname, Button btn, string action)
        {
            if (action == "Add")
            {
                if (SecurityDT_Add != null)
                {
                    DataRow[] drr = SecurityDT_Add.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {

                        if (SecurityDT_Add.Rows[i]["FormName"].ToString().ToUpper() == Formname.ToUpper() && SecurityDT_Add.Rows[i]["FormActionName"].ToString().ToUpper() == action.ToUpper())
                        {
                            btn.Enabled = true;
                        }
                    }
                }
            }
            else if (action == "Edit")
            {
                if (SecurityDT_Edit != null)
                {
                    DataRow[] drr = SecurityDT_Edit.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {

                        if (SecurityDT_Edit.Rows[i]["FormName"].ToString().ToUpper() == Formname.ToUpper() && SecurityDT_Edit.Rows[i]["FormActionName"].ToString().ToUpper() == action.ToUpper())
                        {
                            btn.Enabled = true;
                        }
                    }
                }

            }
            else if (action == "Delete")
            {
                if (SecurityDT_Delete != null)
                {
                    DataRow[] drr = SecurityDT_Delete.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {

                        if (SecurityDT_Add.Rows[i]["FormName"].ToString().ToUpper() == Formname.ToUpper() && SecurityDT_Delete.Rows[i]["FormActionName"].ToString().ToUpper() == action.ToUpper())
                        {
                            btn.Enabled = true;
                        }
                    }
                }

            }
            else if (action == "Print")
            {
                if (SecurityDT_Print != null)
                {
                    DataRow[] drr = SecurityDT_Print.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {

                        if (SecurityDT_Add.Rows[i]["FormName"].ToString().ToUpper() == Formname.ToUpper() && SecurityDT_Print.Rows[i]["FormActionName"].ToString().ToUpper() == action.ToUpper())
                        {
                            btn.Enabled = true;
                        }
                    }
                }

            }
            else if (action == "Post")
            {
                if (SecurityDT_Post != null)
                {
                    DataRow[] drr = SecurityDT_Post.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {

                        if (SecurityDT_Add.Rows[i]["FormName"].ToString().ToUpper() == Formname.ToUpper() && SecurityDT_Post.Rows[i]["FormActionName"].ToString().ToUpper() == action.ToUpper())
                        {
                            btn.Enabled = true;
                        }
                    }
                }
            }
        }


          public bool UserRight(string FormCode, string ActionCode)
        {
            bool Access = false;
            if (HttpContext.Current.Session["UserRights"] != null)
            {
                var Rights = HttpContext.Current.Session["UserRights"] as List<FormVerification>;
                if (Rights.Count > 0)
                {
                    foreach (var i in Rights)
                    {
                        if (i.FormCode.Trim() == FormCode.Trim() && i.FormActionCode.Trim() == ActionCode.Trim() && i.ActionValue == "Y")
                        {
                            Access = true;
                            return Access;
                        }
                    }
                }
            }
            return Access;
        }


    

    }
}
