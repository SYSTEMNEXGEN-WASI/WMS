﻿using System;
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

        public static string GetConnectionString()
        {
            return ConfigurationManager.AppSettings["DMISDB"].ToString();
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
