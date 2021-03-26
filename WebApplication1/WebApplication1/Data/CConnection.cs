using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
/// Summary description for CConnection
/// </summary>
/// 
namespace CConn
{
    public class CConnection
    {


        public CConnection()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static string GetSecurityConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["SecurityConnectionString"].ToString();
        }

        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings ["BMSConnectionString"].ToString();
        }

        public static string GetConnStringForAccount()
        {
            return ConfigurationManager.ConnectionStrings["FAMSConnectionString"].ToString();
        }

        public static string user()
        {
            return ConfigurationManager.AppSettings["user"].ToString();
        }
        public static string password()
        {
            return ConfigurationManager.AppSettings["password"].ToString();
        }

        public static string server()
        {
            return ConfigurationManager.AppSettings["server"].ToString();
        }
        public static string database()
        {
            return ConfigurationManager.AppSettings["database"].ToString();
        }

        

        public static string GetMCDMainDBConnectionString()
        {
            return ConfigurationManager.AppSettings["MCDMainDB"].ToString();
        }

        //public static string GetSMISConnectionString()
        //{
        //    return ConfigurationManager.AppSettings["SMIS"].ToString();
        //}


        /*
          public static string GetConnectionString(string connStr)
          {
              string conString="";
              if (connStr == "DMIS")
              {
                  conString = ConfigurationSettings.AppSettings["ConnectionString"];
              }
              if (connStr == "SECURITY")
              {
                  conString = ConfigurationSettings.AppSettings["AppSecurityDB"];
              }
              return conString;     
          }
         */
    }
}
