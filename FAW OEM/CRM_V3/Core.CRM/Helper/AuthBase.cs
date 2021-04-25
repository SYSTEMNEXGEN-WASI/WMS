using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.CRM.Helper
{
    public class AuthBase
    {
       
        #region Admin Area Users

        public static int UserId
        {
            get
            {


                if (HttpContext.Current.Session["userId"] != null && HttpContext.Current.Session["userId"].ToString().Length > 0)

                    return Convert.ToInt32(HttpContext.Current.Session["userId"]);
                
                else
                    return 0;
            }
            set
            {
                HttpContext.Current.Session["userId"] = value;
            }
        }

        public static string EmpCode
        {
            get
            {


                if (HttpContext.Current.Session["userId"] != null && HttpContext.Current.Session["userId"].ToString().Length > 0)

                    return HttpContext.Current.Session["userId"].ToString();

                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session["userId"] = value;
            }
        }
        #endregion

        #region Agent
        public static int AgentId
        {
            get
            {
                if (HttpContext.Current.Session["agentId"] != null && HttpContext.Current.Session["agentId"].ToString().Length > 0)
                    return Convert.ToInt32(HttpContext.Current.Session["agentId"]);
                else
                    return 0;
            }
            set
            {
                HttpContext.Current.Session["agentId"] = value;
            }
        }
        #endregion
    }
}
