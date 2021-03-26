using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
//using DevExpress.XtraGauges.Core.Model;
using System.Drawing;
using System.Web.UI.WebControls;
//using DevExpress.Web.ASPxEditors;
using CConn;
using System.IO;
using System.Web.UI;

namespace DXBMS
{
    public class SysFunctions
    {
        public bool CheckVoucherPostFlag(string dealercode,string voucher)
        {
            try
            {
                SqlDataAdapter dta = new SqlDataAdapter("Select Post from GVouMaster where CompCode = '" +dealercode + "' and VouchNo = '" + voucher + "'", CConnection.GetConnStringForAccount());

                DataTable dt = new DataTable();
                dta.Fill(dt);

                if (dt == null)
                {
                    return false;
                }

                if (dt.Rows.Count > 0 && dt.Rows[0]["Post"].ToString() == "Y")
                {

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public void UpdateJV(string CompanyCode, string delNo)
        {
            // string pcName = System.Net.Dns.GetHostEntry(HttpContext.Current.Request.ServerVariables["REMOTE_HOST"]).HostName.ToString();
            string connstring = CConnection.GetConnStringForAccount();
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                using (SqlCommand cmd = new SqlCommand("Sp_update", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conn.Open();
                    cmd.Parameters.AddWithValue("@CompCode", CompanyCode);
                    cmd.Parameters.AddWithValue("@VouchNo", delNo);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static double CustomCDBL(object txt)
        {
            if (object.ReferenceEquals(txt, DBNull.Value)) return 0;
            else if (txt == null) return 0;
            else if (string.IsNullOrEmpty(txt.ToString().Trim())) return 0;
            else return Math.Round(Convert.ToDouble(txt), 2);
        }
        public void GetConnection()
        {
            try
            {
                //SqlConnection con = new SqlConnection();
                if (GlobalVar.mDataConnection == null || GlobalVar.mCon == null)
                {
                    /// Changed by imran sohail ///
                    GlobalVar.mDataConnection = CConn.CConnection.GetConnectionString();// "Data Source=DBServer;Initial Catalog=BMS;User ID=sa;Password=;Max Pool Size=3000;Min Pool Size=5;";
                    //if (Environment.MachineName == "AZHARDELL")
                    //{
                    //    GlobalVar.mDataConnection = "Data Source=azhardell;Initial Catalog=BMS;User ID=sa;Password=100372;Max Pool Size=3000;Min Pool Size=5;MultipleActiveResultSets=true";
                    //}
                    /////////////////////////////////
                    SqlConnection cn = new SqlConnection();
                    cn.ConnectionString = GlobalVar.mDataConnection;
                    cn.Open();
                    GlobalVar.mCon = cn;
                }
                //return GlobalVar.mCon;
            }
            catch (Exception e)
            {
                GlobalVar.mErrorMessage = " Cretical connection Error cant continue ,please contact with concern person \n" + e.Message;
                GlobalVar.mCon = null;
            }
        }
        public DataTable GetData(string SQL)
        {
            try
            {
                if (GlobalVar.mDataConnection == null)
                {
                    GetConnection();
                }
                SqlDataAdapter dta = new SqlDataAdapter(SQL,CConnection.GetConnectionString().ToString());
                DataTable dt = new DataTable();
                dta.Fill(dt);

                return dt;
            }
            catch (Exception e)
            {
                GlobalVar.mErrorMessage = " Cretical connection Error cant continue ,please contact with concern person \n" + e.Message;
                return null;
            }


        }

        public DataSet GetData(string SQL, string Table)
        {
            try
            {
                if (GlobalVar.mCon == null)
                {
                    GetConnection();
                }
                SqlDataAdapter dta = new SqlDataAdapter(SQL, GlobalVar.mCon);
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                dta.Fill(ds,Table);

                return ds;
            }
            catch (Exception e)
            {
                GlobalVar.mErrorMessage = " Cretical connection Error cant continue ,please contact with concern person \n" + e.Message;
                return null;
            }


        }
        public bool isDateTime(string txtDate)
        {
            DateTime tempDate;

            return DateTime.TryParse(txtDate, out tempDate) ? true : false;
        }

        public object SaveDate(string txt)
        {
            object functionReturnValue = null;
            try
            {
                if (txt == "__/__/____" || string.IsNullOrEmpty(txt))
                {
                    functionReturnValue = DBNull.Value;
                }
                else
                {
                    DateTime DateTimeFor = Convert.ToDateTime(txt.Substring(6, 4) + "-" + txt.Substring(3, 2) + "-" + txt.Substring(0, 2)); //Convert.ToDateTime(txt, new System.Globalization.CultureInfo("ur-PK"));
                    functionReturnValue = DateTimeFor.ToString("yyyy-MM-dd");
                }
            }
            catch (Exception ex)
            {
                functionReturnValue = DBNull.Value; throw ex;
            }
            return functionReturnValue;
        }
        public object SaveTime(string txt)
        {
            object functionReturnValue = null;
            try
            {
                if (txt == "__:__" || string.IsNullOrEmpty(txt))
                {
                    functionReturnValue = DBNull.Value;
                }
                else
                {
                    DateTime DateTimeFor = Convert.ToDateTime(txt);
                    functionReturnValue = DateTimeFor.ToString("HH:mm");
                }
            }
            catch (Exception ex)
            {
                return "00:00";
            }
            return functionReturnValue;
        }

        public string ActiveUserName(string SessionUserID)
        {
            string usrname = "";
            DataTable dt = GetData("select * from SecurityUser where ID='" + SessionUserID + "'");
            if (dt.Rows.Count > 0)
            {
                usrname = dt.Rows[0]["UserName"].ToString();
            }
            dt.Dispose();
            return usrname;
        }
        public bool ExecuteSP(string SP_Name, SqlParameter[] param, ref SqlDataReader dr)
        {
            bool flag;

            try
            {
                GetConnection();
                
                dr = SqlHelper.ExecuteReader(CConnection.GetConnectionString(), SP_Name, param);
                if (dr.HasRows) flag = true; else flag = false;
            }
            catch (Exception ex) { throw ex; }
            return flag;


        }

        public DataSet FillDataSet(string SP_Name, SqlParameter[] param)
        {
            if (GlobalVar.mDataConnection == null && GlobalVar.mCon == null)
            {
                GetConnection();
            }
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(GlobalVar.mCon, CommandType.StoredProcedure, SP_Name, param);
            return ds;
        }
        public bool ExecuteQuery(string sQuery, ref SqlDataReader dr)
        {
            bool flag;
            try
            {
                GetConnection();

                dr = SqlHelper.ExecuteReader(CConnection.GetConnectionString(), CommandType.Text, sQuery);

                if (dr.HasRows) flag = true; else flag = false;
            }
            catch (Exception exception) { throw exception; }
            return flag;

        }
        public bool ExecuteQuery(string sQuery, ref DataSet ds)
        {
            // tell us later how to use 

            bool flag;
            ds = new DataSet();
            try
            {
                ds = SqlHelper.ExecuteDataset(CConnection.GetConnectionString(), CommandType.Text, sQuery);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return flag;
        }
        public bool ExecuteQuery(string sQuery, SqlTransaction Trans)
        {
            bool flag;
            try
            {
                int i = SqlHelper.ExecuteNonQuery(Trans, CommandType.Text, sQuery);
                if (i > 0)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            catch (Exception exception)
            {
                flag = false;
                throw exception;
            }
            return flag;
        }
        public bool ExecuteQuery(string sQuery, ref SqlDataReader dr, SqlTransaction trans)
        {
            bool flag;

            try
            {
                dr = SqlHelper.ExecuteReader(trans, CommandType.Text, sQuery);
                if (dr.HasRows)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }

            }
            catch (Exception exception)
            {
                throw exception;

            }
            return flag;
        }


        public bool ExecuteSPDMIS(string SP_Name, SqlParameter[] param, ref SqlDataReader dr)
        {
            bool flag;

            try
            {
                dr = SqlHelper.ExecuteReader(GlobalVar.mCon, SP_Name, param);
                if (dr.HasRows)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }

            }
            catch (Exception exception)
            {
                throw exception;

            }
            return flag;
        }


        public bool ExecuteSP_NonQuery(string SP_Name, SqlTransaction Trans)
        {
            bool flag;
            try
            {
                SqlHelper.ExecuteNonQuery(Trans, CommandType.Text, SP_Name);
                flag = true;
            }
            catch (Exception exception)
            {
                throw exception;
                flag = false;
            }
            return flag;
        }
        public bool ExecuteSP_NonQuery(string SP_Name, SqlParameter[] param)
        {
            bool flag;
            try
            {
                GetConnection();
                SqlHelper.ExecuteNonQuery(GlobalVar.mDataConnection, CommandType.StoredProcedure, SP_Name, param);
                flag = true;
            }
            catch (Exception exception)
            {
                flag = false;
                throw exception;
            }
            return flag;
        }
        public bool ExecuteSP_NonQuery(string SP_Name, SqlParameter[] param, SqlTransaction Trans)
        {
            bool flag;
            try
            {
                SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, SP_Name, param);
                flag = true;                
            }
            catch (Exception exception)
            {
                flag = false;
                throw exception;
            }
            return flag;
        }

        public bool ExecuteQuery_NonQuery(string strQuery, SqlTransaction Trans)
        {
            bool flag;
            try
            {
                SqlHelper.ExecuteNonQuery(Trans, CommandType.Text, strQuery);
                flag = true;
            }
            catch (Exception exception)
            {
                flag = false;
                throw exception;
            }
            return flag;
        }
        public bool ExecuteQuery_NonQuery(string strQuery)
        {
            bool flag;
            try
            {
                SqlHelper.ExecuteNonQuery(GlobalVar.mDataConnection, CommandType.Text, strQuery);
                flag = true;
            }
            catch (Exception exception)
            {
                flag = false;
                throw exception;
            }
            return flag;
        }
        public DataSet FillDataSetOrEmptyRow(string SP_Name, SqlParameter[] param)
        {
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(CConn.CConnection.GetConnectionString(), CommandType.StoredProcedure, SP_Name, param);
            if (ds.Tables[0].Rows.Count == 0) { ds.Tables[0].Rows.Add(ds.Tables[0].NewRow()); }
            return ds;

        }

        //General functions
        public bool CodeExists(string strTableNames, string strColumnCode, string strColumeValue,string WhereClause, string DealerCode, string Append="")
        {
            DataTable reader;
            try
            {
                //Create query to check in all tables one by one

                //foreach ( sTableName in strTableNames) {
                //string sQuery = "SELECT * FROM " + strTableNames + " WHERE " + strColumnCode + " = '" + strColumeValue + "' AND DealerCode = '" + SessionInformation.DealerCode + "'";
                string sQuery = "SELECT * FROM " + strTableNames + " WHERE " + strColumnCode + " = '" + strColumeValue + "' AND DealerCode = '" + DealerCode + "'";

                reader = GetData(sQuery);
                if (reader.Rows.Count>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false; throw ex;
            }
        }
        public bool CodeExists(string strTableNames, string strColumnCode, string strColumnValue)
        {
            try
            {
                //Create query to check in all tables one by one
                //string sTableName = null;
                //foreach ( sTableName in strTableNames) {
                string sQuery = "SELECT * FROM " + strTableNames;
                if (strColumnCode.Trim().Length > 0)
                {
                    sQuery += " WHERE " +strColumnCode+"='"+strColumnValue+"'";
                }
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.Text, sQuery);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //clsLogger.WriteLog(e.Message, e.Source);
                return false;
            }
        }
        public bool CodeExists(string strTableName, string strCodeColumn, string strCodeValue, string DealerCode, string Append = "")
        {
            //Step1: create query 
            // Append : and ColumeName = Condition;

            string selectQuery = "SELECT * FROM " + strTableName + " WHERE " + strCodeColumn + " = '" + strCodeValue + "' AND DealerCode = '" + DealerCode + "' "; //+ Append;
            if (Append.Trim() != "")
            {
                selectQuery += Append;
            }

            //Step2: Check wether code exists or not
            try
            {
                //Create reader
                SqlDataReader drCodeReader = null;

                if (ExecuteQuery(selectQuery, ref drCodeReader))
                {
                    //return false;
                    //Check the number of rows returned 
                    if ((drCodeReader.HasRows))
                    {
                        drCodeReader.Close();
                        return true;
                    }
                    else
                    {
                        drCodeReader.Close();
                        return false;
                    }
                }
                drCodeReader.Close();
                return false;
            }
            catch (Exception e)
            {
                //clsLogger.WriteLog(e.Message, e.Source);
                return false;
            }
        }
        public bool CodeExists(string strTableNames, string strColumnCode, string strColumeValue, ref DataSet ds, string DealerCode)
        {
            try
            {
                //Create query to check in all tables one by one
                //foreach ( sTableName in strTableNames) {
                string sQuery = "SELECT * FROM " + strTableNames + " WHERE " + strColumnCode + " = '" + strColumeValue + "' AND DealerCode = '" + DealerCode + "'";

                ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.Text, sQuery);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //clsLogger.WriteLog(e.Message, e.Source);
                throw ex;
            }
        }
        public bool CodeExists(string strTableNames, string WhereClause, ref DataSet ds)
        {
            try
            {
                //Create query to check in all tables one by one
                //string sTableName = null;
                //foreach ( sTableName in strTableNames) {
                string sQuery = "SELECT * FROM " + strTableNames;
                if (WhereClause.Trim().Length > 0)
                {
                    sQuery += " WHERE " + WhereClause;
                }
                ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.Text, sQuery);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //clsLogger.WriteLog(e.Message, e.Source);
                return false;
            }
        }
        


        public string GetServerTime()
        {

            DataTable dt = GetData("select CURRENT_TIMESTAMP as CDT");
            string cdt = dt.Rows[0]["CDT"].ToString();
            dt.Dispose();
            return cdt;

        }
        public string GetStringValuesAgainstCodes(string ItemCodeColumn, string ItemCode, string ValueItemColumn, string ItemTable, string dealerCode)
        {
            string functionReturnValue = string.Empty;
            SqlDataReader drItemVal = null;
            string strQuerry = null;
            strQuerry = " Select " + ValueItemColumn + " From [" + ItemTable + "] with(nolock) Where "
                        + ItemCodeColumn + " = '" + ItemCode + "' AND [DealerCode] IN ('" + dealerCode + "','AAAAA')";
            if (ExecuteQuery(strQuerry, ref drItemVal))
            {
                try
                {
                    if (drItemVal.HasRows)
                    {
                        drItemVal.Read();
                        string sTempItem = Convert.ToString(GetNullString(drItemVal[0]));
                        drItemVal.Close();
                        functionReturnValue = sTempItem;
                    }
                    else
                    {
                        drItemVal.Close();
                        return functionReturnValue;
                    }

                }
                catch (Exception ex)
                {
                    //ShowError();
                    drItemVal.Close();
                }
            }
            drItemVal.Close();
            return functionReturnValue;
        }

        public string GetStringValuesAgainstCodes1(string ItemCodeColumn, string ItemCode, string ValueItemColumn, string ItemTable)
        {
            string functionReturnValue = string.Empty;
            SqlDataReader drItemVal = null;
            string strQuerry = null;
            strQuerry = " Select " + ValueItemColumn + " From [" + ItemTable + "] with(nolock) Where " + ItemCodeColumn + " = "+ ItemCode +"  [DealerCode] IN ('" + GlobalVar.mDealerCode + "','AAAAA')";
            if (ExecuteQuery(strQuerry, ref drItemVal))
            {
                try
                {
                    if (drItemVal.HasRows)
                    {
                        drItemVal.Read();
                        string sTempItem = Convert.ToString(GetNullString(drItemVal[0]));
                        drItemVal.Close();
                        functionReturnValue = sTempItem;
                    }
                    else
                    {
                        drItemVal.Close();
                        return functionReturnValue;
                    }

                }
                catch (Exception ex)
                {
                    //ShowError();
                    drItemVal.Close();
                }
            }
            drItemVal.Close();
            return functionReturnValue;
        }
     
        public string GetStringValuesAgainstCodes(string WhereClause, string ItemTable, string ValueItemColomn)
        {
            string functionReturnValue = string.Empty;
            SqlDataReader drItemVal = null;
            string strQuerry = null;
            strQuerry = " Select " + ValueItemColomn + " From [" + ItemTable + "]  ";
            if (WhereClause.Length > 0)
            {
                strQuerry += "Where " + WhereClause;
            }
            if (ExecuteQuery(strQuerry, ref drItemVal))
            {
                try
                {
                    if (drItemVal.HasRows)
                    {
                        drItemVal.Read();
                        string sTempItem = Convert.ToString(GetNullString(drItemVal[0]));
                        drItemVal.Close();
                        functionReturnValue = sTempItem;
                    }
                    else
                    {
                        return functionReturnValue;
                    }

                }
                catch (Exception ex)
                {
                    //ShowError();
                    drItemVal.Close();
                }
            }
            return functionReturnValue;
        }
        public string GetStringValuesAgainstCodes(string ItemCodeColomn, string ItemCode, string ValueItemColomn, string ItemTable, string Append,string DealerCode)
        {
            string functionReturnValue = string.Empty;
            SqlDataReader drItemVal = null;
            string strQuerry = null;
            strQuerry = " Select " + ValueItemColomn + " From " + ItemTable + " Where " + ItemCodeColomn + " = '" + ItemCode + "' AND DealerCode IN( '" + DealerCode + "','COMON') " + Append;
            if (ExecuteQuery(strQuerry, ref drItemVal))
            {
                try
                {
                    if (drItemVal.HasRows)
                    {
                        drItemVal.Read();
                        string sTempItem = Convert.ToString(GetNullString(drItemVal[0]));
                        drItemVal.Close();
                        functionReturnValue = sTempItem;
                    }
                    else
                    {
                        drItemVal.Close();
                        return functionReturnValue;
                    }

                }
                catch (Exception ex)
                {
                    //ShowError();
                    drItemVal.Close();
                }
            }
            drItemVal.Close();
            return functionReturnValue;
        }

        public void UserMsg(System.Web.UI.WebControls.Label lblMsg, Color txtColor, string strMsg,TextBox txtfocus = null)
        {
            //var page = HttpContext.Current.CurrentHandler as Page;
            lblMsg.Text = strMsg;
            lblMsg .ForeColor = txtColor;
            //page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "ShowFailurMSG();", true);
            if (txtfocus != null)
            {
                txtfocus.Focus();
            }

        }
        public void UserMsgddl(System.Web.UI.WebControls.Label lblMsg, Color txtColor, string strMsg, DropDownList txtfocus = null)
        {
            //var page = HttpContext.Current.CurrentHandler as Page;
            lblMsg.Text = strMsg;
            lblMsg.ForeColor = txtColor;
            //page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "ShowFailurMSG();", true);
            if (txtfocus != null)
            {
                txtfocus.Focus();
            }

        }
        public string GetNullString(object txt)
        {
            try
            {
                if (object.ReferenceEquals(txt, DBNull.Value)) return string.Empty;
                else return Convert.ToString(txt).Trim();
            }
            catch (Exception) { return string.Empty; }
        }
     
        public bool IsExist(string strColumnName, string strValue, string strTableName,string DealerCode, string Append = "")
        {
            SqlDataReader dr = null;
            string strSql = null;
            strSql = "Select [" + strColumnName + "] from [" + strTableName + "] Where [" + strColumnName + "] = '" + strValue + "' AND DealerCode IN ('" + DealerCode + "','COMON') " + Append;
            try
            {
                //if (!dr.IsClosed)
                //{
                //    dr.Close();
                //}
                bool bFlag = ExecuteQuery(strSql, ref dr);
                if (!bFlag)
                {
                    dr.Close();
                    return false;
                }
                else
                {
                    if ((dr.HasRows))
                    {
                        dr.Close(); return true;
                    }
                    else
                    {
                        dr.Close(); return false;
                    }
                }
            }
            catch (SqlException ex)
            {
                //clsLogger.WriteLog(ex.Message, ex.Source);
                dr.Close();
                return false;
            }
        }
        

        public string GetExtraTaxPercent(string DealerCode)
        {
            string retVal="";
            string sql = "select * from BaseRate where DealerCode in ('"+DealerCode+"','AAAAA')";
            DataTable dt = GetData(sql);
            if (dt.Rows.Count > 0)
            {
                retVal = dt.Rows[0]["ExtraTax"].ToString();
            }
            return retVal;
        }
        public string GetFurtherTaxPercent(string DealerCode)
        {
            string retVal = "";
            string sql = "select * from BaseRate where DealerCode in ('" + DealerCode + "','AAAAA')";
            DataTable dt = GetData(sql);
            if (dt.Rows.Count > 0)
            {
                retVal = dt.Rows[0]["AddGST"].ToString();
            }
            return retVal;
        }

        public string GetNewMaxID(string sTableName, string sColumn, int NoOfChar, string DealerCode)
        {
            //#GET MAX ID FROM TABLE
            string sQuery = "SELECT MAX(" + sColumn + ") MAXID FROM " + sTableName + " WHERE DealerCode='" + DealerCode + "'";
            SqlDataReader drItemVal = null;
            string sNewVersion = "0";
            int iNewVersion;
            if (ExecuteQuery(sQuery, ref drItemVal))
            {
                try
                {
                    if (drItemVal.HasRows)
                    {
                        drItemVal.Read();
                        sNewVersion = GetNullString(drItemVal["MAXID"]);
                        drItemVal.Close();
                        if (sNewVersion == "")
                        {
                            sNewVersion = "0";
                            iNewVersion = Convert.ToInt32(sNewVersion) + 1;
                            sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
                        }
                        else
                        {
                            iNewVersion = Convert.ToInt32(sNewVersion) + 1;
                            sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
                        }
                        return sNewVersion;

                    }
                    else
                    {
                        iNewVersion = 0;
                        sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
                        return sNewVersion;
                    }

                }
                catch (Exception ex)
                {
                    //ShowError();
                    drItemVal.Close();
                }

            }
            return sNewVersion;
        }
        public string GetNewMaxIDWithOutDealer(string sTableName, string sColumn, int NoOfChar, string Append)
        {
            //#GET MAX ID FROM TABLE
            string sQuery = "SELECT MAX(" + sColumn + ") MAXID FROM " + sTableName + Append;
            SqlDataReader drItemVal = null;
            string sNewVersion = "0";
            int iNewVersion;
            if (ExecuteQuery(sQuery, ref drItemVal))
            {
                try
                {
                    if (drItemVal.HasRows)
                    {
                        drItemVal.Read();
                        sNewVersion = GetNullString(drItemVal["MAXID"]);
                        drItemVal.Close();
                        if (sNewVersion == "")
                        {
                            sNewVersion = "0";
                            iNewVersion = Convert.ToInt32(sNewVersion) + 1;
                            sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
                        }
                        else
                        {
                            iNewVersion = Convert.ToInt32(sNewVersion) + 1;
                            sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
                        }
                        return sNewVersion;

                    }
                    else
                    {
                        iNewVersion = 0;
                        sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
                        return sNewVersion;
                    }

                }
                catch (Exception ex)
                {
                    //ShowError();
                    drItemVal.Close();
                }

            }
            return sNewVersion;
        }
        public string GetNewMaxID(string sColumn, int NoOfChar, DataSet ds)
        {
            //#GET MAX ID FROM TABLE
            //string sQuery = "SELECT MAX(" + sColumn + ") MAXID FROM " + sTableName + " WHERE DealerCode='" + DEALER_CODE + "'";
            //SqlDataReader drItemVal = null;
            string sNewVersion = "0";
            int iNewVersion;
            //if (ObjMainBll.ExecuteQuery(sQuery, ref drItemVal))
            //{
            //int minAccountLevel = int.MaxValue;
            int maxID = int.MinValue;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int ID = Convert.ToInt32(dr[sColumn].ToString().Trim() == "" ? "0" : dr[sColumn].ToString().Trim());
                //minAccountLevel = Math.Min(minAccountLevel, accountLevel);
                maxID = Math.Max(maxID, ID);
            }
            //ds.Tables[0].Columns[sColumn].ColumnName = "MAXID";
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    sNewVersion = maxID.ToString();
                    //drItemVal.Close();
                    if (sNewVersion == "")
                    {
                        sNewVersion = "0";
                        iNewVersion = Convert.ToInt32(sNewVersion) + 1;
                        sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
                    }
                    else
                    {
                        iNewVersion = Convert.ToInt32(sNewVersion) + 1;
                        sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
                    }
                    return sNewVersion;

                }
                else
                {
                    iNewVersion = 0;
                    iNewVersion = iNewVersion + 1;
                    sNewVersion = Convert.ToString(iNewVersion).PadLeft(NoOfChar, '0');
                    return sNewVersion;
                }

            }
            catch (Exception ex)
            {
                //ShowError();
                //drItemVal.Close();
            }

            // }
            return sNewVersion;
        }

        public bool IsPosted(string strTableName, string strCodeName, string strCodeValue,string DealerCode, string Append = "")
        {
            SqlDataReader drdata = null;
            string Select_Qry = "SELECT * FROM " + strTableName + " WHERE " + strCodeName + " = '" + strCodeValue + "' AND DealerCode = '" + DealerCode + "'" + " and DelFlag = 'N' " + " and PostFlag = 'Y' " + Append;
            try
            {
                if (ExecuteQuery(Select_Qry, ref drdata) == true)
                {
                    if (drdata.HasRows)
                    {
                        if (drdata.IsClosed == false)
                            drdata.Close();
                        return true;
                    }
                    else
                    {
                        if (drdata.IsClosed == false)
                            drdata.Close();
                        return false;
                    }
                }
                else
                {
                    if (drdata.IsClosed == false)
                        drdata.Close();
                    //ShowError();
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (drdata.IsClosed == false)
                    drdata.Close();
                //clsLogger.WriteLog(ex.Message, ex.Source);
                //ShowError();
                return false;
            }

        }


        // Location Functions
        public bool isLocCodeExist(string LocCode)
        {
            bool retval = false;

            DataTable dt = GetData("select * from PartsLocation where PartsLocCode='"+LocCode+"'");
            if (dt.Rows.Count > 0)
            {
                retval = true;
            }
            dt.Dispose();
            return retval;
        }
        public string GetLocDesc(string LocCode)
        {
            string retval = "";
            DataTable dt = GetData("select * from PartsLocation where PartsLocCode='" + LocCode + "'");
            if (dt.Rows.Count > 0)
            {
                retval = dt.Rows[0]["PartsLocDesc"].ToString();
            }
            dt.Dispose();
            return retval;
        }

        public bool isSubLocCodeExist(string LocCode,string SubLocCode)
        {
            bool retval = false;

            DataTable dt = GetData("select * from PartsSubLocation where PartsSubLocCode='" + SubLocCode + "' and PartsLocCode='"+LocCode+"'");
            if (dt.Rows.Count > 0)
            {
                retval = true;
            }
            dt.Dispose();
            return retval;
        }
        public string GetSubLocDesc(string LocCode,string SubLocCode)
        {
            string retval = "";
            DataTable dt = GetData("select * from PartsSubLocation where PartsSubLocCode='" + SubLocCode + "' and PartsLocCode='" + LocCode + "'");
            if (dt.Rows.Count > 0)
            {
                retval = dt.Rows[0]["PartsSubLocDesc"].ToString();
            }
            dt.Dispose();
            return retval;
        }


        public bool isTransLocCodeExist(string TransLocCode)
        {
            bool retval = false;

            DataTable dt = GetData("select * from PartsTransLocation where PartsTransLocCode='" + TransLocCode + "'");
            if (dt.Rows.Count > 0)
            {
                retval = true;
            }
            dt.Dispose();
            return retval;
        }
        public string GetTransLocDesc(string TransLocCode)
        {
            string retval = "";
            DataTable dt = GetData("select * from PartsTransLocation where PartsTransLocCode='" + TransLocCode + "'");
            if (dt.Rows.Count > 0)
            {
                retval = dt.Rows[0]["PartsTransLocDesc"].ToString();
            }
            dt.Dispose();
            return retval;
        }



        //Inventroy functions
        public string GetUnitDesc(string UnitCode)
        {
            string retval = "";
            DataTable dt = GetData("select * from Unit where UnitCode='" + UnitCode + "'");
            if (dt.Rows.Count > 0)
            {
                retval = dt.Rows[0]["UnitDesc"].ToString();
            }
            dt.Dispose();
            return retval;
        }
        public string GetItemCodeByPartItemNo(string PartItemNo,string DealerCode)
        {
            string RetVal = "";

            DataTable dt = GetData("select ItemCode from Item where PartItemNo ='" + PartItemNo + "' and DealerCode in('COMON', '"+DealerCode+"')");

            if(dt.Rows.Count > 0)
            {
                RetVal = dt.Rows[0]["ItemCode"].ToString();
            }

            
            return RetVal;
        }
        public string GetItemPriceByPartItemNo(string PartItemNo)
        {
            string RetVal = "";

            DataTable dt = GetData("select SaleRate from Item where PartItemNo='" + PartItemNo + "'");

            RetVal = dt.Rows[0]["SaleRate"].ToString();
            return RetVal;
        }

        public string GetPartLocationByPartItemNo(string PartItemNo , string DealerCode)
        {
            string RetVal = "";

            string sql = "select PartsTransLocCode from Item where ItemCode ='" + PartItemNo + "' and DealerCode in('COMON', '"+DealerCode+"')";

            DataTable dt = GetData(sql);

            RetVal = dt.Rows[0]["PartsTransLocCode"].ToString();
            return RetVal;
        }
        public string GetPartItemNoByItemCode(string ItemCode)
        {
            string RetVal = "";

            DataTable dt = GetData("select PartItemNo from Item where ItemCode='" + ItemCode + "'");

            RetVal = dt.Rows[0]["PartItemNo"].ToString();
            return RetVal;
        }
        public string GetItemDescByItemCode(string ItemCode)
        {
            string RetVal = "";

            DataTable dt = GetData("select ItemDesc from Item where ItemCode='" + ItemCode + "'");

            RetVal = dt.Rows[0]["ItemDesc"].ToString();
            return RetVal;
        }
        public string GetItemSourceByItemCode(string ItemCode)
        {
            string RetVal = "";

            DataTable dt = GetData("select Source from Item where ItemCode='" + ItemCode + "'");

            RetVal = dt.Rows[0]["Source"].ToString();
            return RetVal;
        }
        public string GetUnitDescByPartItemNo(string PartItemNo)
        {
            string RetVal = "";

            DataTable dt = GetData("select u.UnitDesc from item i left outer join Unit u on i.UnitCode = u.UnitCode where i.PartItemNo='" + PartItemNo + "'");

            RetVal = dt.Rows[0]["UnitDesc"].ToString();
            return RetVal;
        }
        public double GetSalesTaxPercentByItemCode(string ItemCode)
        {
            double RetVal = 0;

            DataTable dt = GetData("select GSTPerc from Item where ItemCode='" + ItemCode + "'");
            if (dt.Rows.Count == 0)
            {
                return 0;
            }
            
            string gp = dt.Rows[0]["GSTPerc"].ToString();

            if (gp == "")
            {
                gp = "0";
            }
            RetVal = Convert.ToDouble(gp );
            return RetVal;
        }
        public double GetFurtherSalesTaxPercentByItemCode(string ItemCode)
        {
            double RetVal = 0;

            DataTable dt = GetData("select FGSTPerc from Item where ItemCode='" + ItemCode + "'");
            if (dt.Rows.Count == 0)
            {
                return 0;
            }

            string gp = dt.Rows[0]["FGSTPerc"].ToString();

            if (gp == "")
            {
                gp = "0";
            }
            RetVal = Convert.ToDouble(gp);
            return RetVal;
        }

        public string GetTransLocationByPartItemNo(string PartItemNo)
        {
            string RetVal = "";

            DataTable dt = GetData("select ItemCode,PartItemNo,i.PartsTransLocCode,l.PartsTransLocDesc from item i left outer join PartsTransLocation l on i.PartsTransLocCode = l.PartsTransLocCode where i.PartItemNo='" + PartItemNo + "'");

            RetVal = dt.Rows[0]["PartsTransLocDesc"].ToString();
            return RetVal;
        }
        public string GetTransLocationCodeByPartItemNo(string PartItemNo)
        {
            string RetVal = "";

            DataTable dt = GetData("select ItemCode,PartItemNo,i.PartsTransLocCode,l.PartsTransLocDesc from item i left outer join PartsTransLocation l on i.PartsTransLocCode = l.PartsTransLocCode where i.PartItemNo='" + PartItemNo + "'");

            RetVal = dt.Rows[0]["PartsTransLocCode"].ToString();
            return RetVal;
        }
        public string GetFirstTransLocationCode()
        {
            string RetVal = "";

            DataTable dt = GetData("select ItemCode,PartItemNo,i.PartsTransLocCode,l.PartsTransLocDesc from item i left outer join PartsTransLocation l on i.PartsTransLocCode = l.PartsTransLocCode ");
            if (dt.Rows.Count > 0)
            {
                RetVal = dt.Rows[0]["PartsTransLocCode"].ToString();
            }
            return RetVal;
        }


        //PO Function

        public string GetPOStatus(string PONo)
        {
            string RetVal = "";

            DataTable dt = GetData("select TransferStatus from POMaster where POno='"+PONo+"'");

            RetVal = dt.Rows[0]["TransferStatus"].ToString();
            return RetVal;
                

        }

        public string GetNewPONumber(string DealerID)
        {
            string RetVal = "";
            string mYear = Convert.ToDateTime( GetServerTime()).ToString("yy");
            string sql = "select MAX(POno) as MPONo from POMaster where DealerCode='" + DealerID + "' AND LEFT(POno,2)='"+mYear+"'";

            DataTable dt = new DataTable();

            dt = GetData(sql);
            if (dt.Rows.Count == 0)
            {
                RetVal = mYear + "000001";
            }
            else
            {
                string v=dt.Rows[0]["MPONo"].ToString();
                if (v == "")
                {
                    v = mYear + "000000";
                }

                int n = Convert.ToInt32(v) + 1;

                RetVal = n.ToString();
            }



            return RetVal;
        }

        public string GetItemRateFromPOByItemCode(string PONumber, string ItemCode)
        {
            string RetVal = "";
            string sql = "select Rate from PODetail where PONo='"+PONumber+"' and ItemCode='"+ItemCode+"'";

            DataTable dt = new DataTable();

            dt = GetData(sql);
            if (dt.Rows.Count == 0)
            {
                RetVal = "0";
            }
            else
            {
                RetVal = dt.Rows[0]["Rate"].ToString();
            }
            return RetVal;
        }
        public string GetItemGSTPercentFromPOByItemCode(string PONumber, string ItemCode)
        {
            string RetVal = "";
            string sql = "select GSTage from PODetail where PONo='" + PONumber + "' and ItemCode='" + ItemCode + "'";

            DataTable dt = new DataTable();

            dt = GetData(sql);
            if (dt.Rows.Count == 0)
            {
                RetVal = "0";
            }
            else
            {
                RetVal = dt.Rows[0]["GSTage"].ToString();
            }
            return RetVal;
        }


        //DC functions
        public bool isDCExist(string DCNumber,string DealerCode)
        {
            bool RetVal = false;

            DataTable dt = GetData("select DCNo from DCMaster where DCNo='" + DCNumber + "' and DealerCode = '"+DealerCode+"'");
            if (dt.Rows.Count > 0)
            {
                RetVal = true;
            }
            return RetVal;
        }
        public string GetNewDCNumber(string DealerID)
        {
            string RetVal = "";
            string mYear = Convert.ToDateTime(GetServerTime()).ToString("yy");
            string sql = "select MAX(DCno) as MDCNo from DCMaster where DealerCode='" + DealerID + "' AND LEFT(DCno,2)='" + mYear + "'";

            DataTable dt = new DataTable();

            dt = GetData(sql);
            if (dt.Rows.Count == 0)
            {
                RetVal = mYear + "000001";
            }
            else
            {
                string v = dt.Rows[0]["MDCNo"].ToString();
                if (v == "")
                {
                    v = mYear + "000000";
                }

                int n = Convert.ToInt32(v) + 1;

                RetVal = n.ToString();
            }



            return RetVal;
        }
        public DateTime GetDCDate(string DCNumber)
        {
            DateTime RetVal = DateTime.Now;
            DataTable dt = GetData("select DCDate from DCMaster where DCNo='" + DCNumber + "'");
            if (dt.Rows.Count > 0)
            {
                RetVal = Convert.ToDateTime(dt.Rows[0]["DCDate"].ToString());
            }

            return RetVal;
        }

        public string GetDCQty(string DCNumber, string ItemCode)
        {
            string retVal = "";

            DataTable dt = GetData("select DCQty from DCDetail where DCNo='"+DCNumber+"' and ItemCode='"+ItemCode+"'");
            if(dt.Rows.Count>0)
            {
                retVal = dt.Rows[0]["DCQty"].ToString();
            }


            return retVal;
        }

        //GRN function
        public string GetNewGRNNumber(string DealerID)
        {
            string RetVal = "";
            string mYear = Convert.ToDateTime(GetServerTime()).ToString("yy");
            string sql = "select MAX(GRNNo) as MGRNNo from GRNMaster where DealerCode='" + DealerID + "' AND LEFT(GRNNo,2)='" + mYear + "'";

            DataTable dt = new DataTable();

            dt = GetData(sql);
            if (dt.Rows.Count == 0)
            {
                RetVal = mYear + "000001";
            }
            else
            {
                string v = dt.Rows[0]["MGRNNo"].ToString();
                if (v == "")
                {
                    v = mYear + "000000";
                }

                int n = Convert.ToInt32(v) + 1;

                RetVal = n.ToString();
            }



            return RetVal;
        }

        public bool SP_Spare_Insert_GRNMaster(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Spare_Insert_GRNMaster", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SP_Spare_Insert_GRNDetail(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Spare_Insert_GRNDetail", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SP_Spare_Insert_GRNItemStock(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Spare_Insert_GRNItemStock", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool UpdateStockFlag(SqlTransaction Trans, string GRNNo, string DEALER_CODE)
        {
            string Update_GrnPost = "";
            Update_GrnPost = " Exec sp_PostGrnActivity '" + GRNNo + "','" + DEALER_CODE + "'";

            if (ExecuteSP_NonQuery(Update_GrnPost, Trans))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SP_Spare_update_PartLocation(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Spare_update_PartLocation", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet SP_Part_Get_GRNInfo(SqlParameter[] param)
        {
            //GRN 
            try
            {
                GetConnection();
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.StoredProcedure, "SP_Part_Get_GRNInfo", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SP_Spare_Update_GRNMaster(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Spare_Update_GRNMaster", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SP_Part_Update_UpdatePost(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Part_Update_UpdatePost", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DateTime GetGRNDate(string GRNNumber)
        {
            DateTime RetVal= DateTime.Now;
            DataTable dt = GetData("select GRNDate from GRNMaster where GRNNo='" + GRNNumber + "'");
            if (dt.Rows.Count > 0)
            {
                RetVal = Convert.ToDateTime(dt.Rows[0]["GRNDate"].ToString());
            }

            return RetVal;
        }

        public string GetGRNStatus(string GRNNo)
        {
            string RetVal = "";

            DataTable dt = GetData("select TransferStatus from GRNMaster where GRNNo='" + GRNNo + "'");

            RetVal = dt.Rows[0]["TransferStatus"].ToString();
            return RetVal;
        }


        //CSI function
        #region // Counter Sales //
        public string AutoGen(string Table, string Col, string txtDt)
        {
            string functionReturnValue = null;
            string Sql_Auto = "";
            SqlDataReader drAuto = null;
            string AutoNum = null;
            //string Sql_Auto = "Select Max(" + Col + ") as '" + Col + "' from " + Table + "" + " Where Left(" + Col + ",2) = '" + Strings.Right(txtDt, 2) + "'" + " Order By " + Col;
            Sql_Auto = "Select Max(" + Col + ") as '" + Col + "' from " + Table + " with(nolock) " + " Where DealerCode='" + GlobalVar.mDealerCode + "' And Left(" + Col + ",2) = '" + txtDt.Substring(txtDt.Length - 2) + "'" + " Order By " + Col;
            if (drAuto != null) if (drAuto.IsClosed == false) drAuto.Dispose();

            if (ExecuteQuery(Sql_Auto, ref drAuto) == true)
            {
                try
                {
                    drAuto.Read();
                    AutoNum = drAuto[Col].ToString();
                    if (AutoNum.Trim() != "")
                    {
                        AutoNum = AutoNum.Substring(AutoNum.Length - 5);
                        AutoNum = (Convert.ToInt32(AutoNum) + 1).ToString();
                        while (!(AutoNum.Length == 5))
                        {
                            AutoNum = "0" + AutoNum;
                        }
                        functionReturnValue = txtDt.Substring(txtDt.Length - 2) + "" + AutoNum;
                    }
                    else
                    {
                        functionReturnValue = txtDt.Substring(txtDt.Length - 2) + "00001";
                    }
                    drAuto.Close();
                }
                catch (Exception ex)
                {
                    functionReturnValue = txtDt.Substring(txtDt.Length - 2) + "00001";
                    drAuto.Close();
                }
            }
            else
            {
                functionReturnValue = txtDt.Substring(txtDt.Length - 2) + "00001";
            }
            return functionReturnValue;
        }

        public bool SP_Spare_Insert_CounterSaleMaster(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Spare_Insert_CounterSaleMaster", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool sp_PurInvMaster_Insert(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("sp_PurInvMaster_Insert", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SP_Spare_Insert_CounterSaleDetail(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Spare_Insert_CounterSaleDetail", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool sp_PurInvDetail_Insert(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("sp_PurInvDetail_Insert", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SP_Spare_Insert_SIRMaster(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Spare_Insert_SIRMaster", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SP_Spare_Insert_SIRDetail(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Spare_Insert_SIRDetail", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SP_Spare_Update_CounterSaleMaster(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Spare_Update_CounterSaleMaster", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SP_Spare_Update_UpdatePost(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Spare_Update_UpdatePost", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SP_Spare_update_CounterSaleGSTInvoice(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Spare_update_CounterSaleGSTInvoice", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet SP_Spare_Get_CounterSaleMaster(SqlParameter[] param)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.StoredProcedure, "SP_Spare_Get_CounterSaleMaster", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet SP_Spare_Get_CounterSaleDetail(SqlParameter[] param)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.StoredProcedure, "SP_Spare_Get_CounterSaleDetail", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string GetRecQtyFromSIR(string SINo, string DealerCode, string ItemCode)
        {
            string RetVal = "0";
            string sql = "select * from  SIRDetail where SIRNo ='" + SINo + "' and DealerCode='" + DealerCode + "' and ItemCode='" + ItemCode + "'";
            DataTable dt = GetData(sql);
            if (dt.Rows.Count > 0)
            {
                RetVal = dt.Rows[0]["RecQty"].ToString();
            }
            return RetVal;
        }

        #endregion

        public string GetCustomerName(string CustomerCode, string DealerCode)
        {
            string RetVal = "0";
            string sql = "select * from  Customer where CusCode ='" + CustomerCode + "' and DealerCode='" + DealerCode + "'";
            DataTable dt = GetData(sql);
            if (dt.Rows.Count > 0)
            {
                RetVal = dt.Rows[0]["CusDesc"].ToString();
            }
            return RetVal;
        }
        public bool IsAnyStockIssuedAgainstSIR(string sirID, string itemCode,string DealerCode)
        {
            string functionReturnValue = null;
            SqlDataReader drItemVal = null;
            SqlDataReader drItemVal2 = null;
            string strQuerry = null;
            //if (!drItemVal.IsClosed)
            //{
            //    drItemVal.Close();
            //}
            bool Check = false;
            strQuerry = "Select RecQty from SIRDetail Where SIRNo='" + sirID + "' and itemCode='" + itemCode + "' and  recQty>0 And DealerCode='" + DealerCode + "'";

            if (ExecuteQuery(strQuerry, ref drItemVal2))
            {
                try
                {
                    if (drItemVal2.HasRows)
                    {
                        drItemVal2.Read();
                        string sTempItem = Convert.ToString(GetNullString(drItemVal[0]));
                        drItemVal2.Close();
                        functionReturnValue = sTempItem;
                        Check = true;
                    }
                    else
                    {
                        Check = false;
                    }
                }
                catch (Exception ex)
                {
                    //ShowError();
                    drItemVal2.Close();
                }
            }
            drItemVal2.Close();
            return Check;
        }

        public bool IsIssued(string strTableName, string strCodeName, string strCodeValue,string DealerCode, string Append = "")
        {
            SqlDataReader drdata = null;
            string Select_Qry = " SELECT * FROM " + strTableName + " WHERE " + strCodeName + " = '" + strCodeValue +
                                "' AND DealerCode = '" + DealerCode + "'" + " and DelFlag = 'N' " + " and IssueFlag = 'Y' " + Append;
            try
            {
                if (ExecuteQuery(Select_Qry, ref drdata) == true)
                {
                    if (drdata.HasRows)
                    {
                        if (drdata.IsClosed == false)
                            drdata.Close();
                        return true;
                    }
                    else
                    {
                        if (drdata.IsClosed == false)
                            drdata.Close();
                        return false;
                    }
                }
                else
                {
                    if (drdata.IsClosed == false)
                        drdata.Close();
                    //ShowError();
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (drdata.IsClosed == false)
                    drdata.Close();
                //clsLogger.WriteLog(ex.Message, ex.Source);
                //ShowError();
                return false;
            }

        }

        public bool SP_Spare_Delete_CounterSaleMaster(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Spare_Delete_CounterSaleMaster", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SP_Spare_Delete_CounterSaleDetail(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Spare_Delete_CounterSaleDetail", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SP_Spare_Insert_GSTInvoice(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Spare_Insert_GSTInvoice", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //SIR functions
        #region // SIR  //

        public DataSet SP_Parts_Get_SIRDataGetForStockIssue(SqlParameter[] param)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.StoredProcedure, "SP_Parts_Get_SIRDataGetForStockIssue", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet SP_Parts_Get_StockReturn_Data(SqlParameter[] param)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.StoredProcedure, "Sp_2W_StockReturn_Select", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet Sp_2W_SRMaster_Select(SqlParameter[] param)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.StoredProcedure, "Sp_2W_SRMaster_Select", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet SP_Parts_Get_PurchaseReturn_Data(SqlParameter[] param)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.StoredProcedure, "Sp_2W_PRMaster_Select", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet ItemAvgCost(string strItemCode, string DEALER_CODE)
        {
            DataSet ds = new DataSet();
            string strSQL = "";
            try
            {
                strSQL = " Select isnull(WAR,0) as Cost " +
                         " from ItemStatus " +
                         " Where ItemCode='" + strItemCode + "' and DealerCode='" + DEALER_CODE + "'";
                ExecuteQuery(strSQL, ref ds);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet PartStock(string strItemCode, string DEALER_CODE, string PartsTransLocCode)
        {
            DataSet ds = new DataSet();

            string Update_GrnPost = "";
            Update_GrnPost = " Exec sp_PartStockInHand '" + DEALER_CODE + "','" + strItemCode + "','" + PartsTransLocCode+"'";

            ExecuteQuery(Update_GrnPost, ref ds);
            return ds;
        }



        public DataSet PartStockAtLoaction(string strItemCode, string DEALER_CODE, string strLocationCode)
        {
            DataSet ds = new DataSet();

            string Update_GrnPost = "";
            Update_GrnPost = " Exec sp_PartStockAtLocation1 '" + DEALER_CODE + "','" + strItemCode + "' , '" + strLocationCode + "'";

            ExecuteQuery(Update_GrnPost, ref ds);

            return ds;

        }
        public bool Sp_Sparts_Get_IsItemIssuedExta(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("Sp_Sparts_Get_IsItemIssuedExta", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SP_Parts_Insert_StockIssue(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Parts_Insert_StockIssue", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Sp_Parts_Insert_StockIssueDetail(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("Sp_Parts_Insert_StockIssueDetail", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet Get_Parts_Get_GetStockIssue(SqlParameter[] param)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.StoredProcedure, "Get_Parts_Get_GetStockIssue", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet SP_parts_Get_StockIssueDetail(SqlParameter[] param)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.StoredProcedure, "SP_parts_Get_StockIssueDetail", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateSIRDetail(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("UpdateSIRDetail", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateJobCardDetail(SqlTransaction Trans, SqlParameter[] param, string StrSIRNO, string StrPartItem,string DealerCode)
        {
            bool Check = true;
            string StrType = "";
            try
            {
                //General ObjGeneral = new General();

                StrType = GetStringValuesAgainstCodes("SIRNO", StrSIRNO, "[Type]", "SIRDetail", " and  ItemCode = " + StrPartItem + " ", Trans,DealerCode);
                if (StrType == "P" || StrType == "A" || StrType == "S")
                {
                    Check = ExecuteSP_NonQuery("SP_Parts_Get_UpdateJobCardDetail", param, Trans);
                }
                else if (StrType == "L")
                {
                    Check = ExecuteSP_NonQuery("SP_Spare_Get_JobCardLubricateDetail", param, Trans);
                }
                else if (StrType == "C")
                {
                    Check = ExecuteSP_NonQuery("SP_Parts_Get_JobCardConDetail", param, Trans);
                }


                if (Check == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetStringValuesAgainstCodes(string ItemCodeColomn, string ItemCode, string ValueItemColomn, string ItemTable, string Append, SqlTransaction Trans, string DealerCode)
        {
            string functionReturnValue = string.Empty;
            SqlDataReader drItemVal = null;
            string strQuerry = null;
            strQuerry = " Select " + ValueItemColomn + " From [" + ItemTable + "] Where " + ItemCodeColomn + " = '" + ItemCode + "' AND [DealerCode] = '" + DealerCode + "' " + Append;
            if (ExecuteQuery(strQuerry, ref drItemVal, Trans))
            {
                try
                {
                    if (drItemVal.HasRows)
                    {
                        drItemVal.Read();
                        string sTempItem = Convert.ToString(GetNullString(drItemVal[0]));
                        drItemVal.Close();
                        functionReturnValue = sTempItem;
                    }
                    else
                    {
                        return functionReturnValue;
                    }

                }
                catch (Exception ex)
                {
                    //ShowError();
                    drItemVal.Close();
                }
            }
            return functionReturnValue;
        }

        public bool ValuesAgainstCodes(string ItemCodeColomn, string ItemCode, string ValueItemColomn, string ItemTable, string Append, string DealerCode)
        {
            string functionReturnValue = string.Empty;
            SqlDataReader dr = null;
            string strQuerry = null;
            strQuerry = " Select " + ValueItemColomn + " From [" + ItemTable + "] Where " + ItemCodeColomn + " = '" + ItemCode + "' AND [DealerCode] = '" + DealerCode + "' " + Append;
            try
            {
                //if (!dr.IsClosed)
                //{
                //    dr.Close();
                //}
                bool bFlag = ExecuteQuery(strQuerry, ref dr);
                if (!bFlag)
                {
                    dr.Close();
                    return false;
                }
                else
                {
                    if ((dr.HasRows))
                    {
                        dr.Read();
                        string sTempItem = Convert.ToString(GetNullString(dr[0]));
                        if(sTempItem == "")
                        {
                            dr.Close();
                            return false;
                        }
                        dr.Close();
                        return true;
                    }
                    else
                    {
                        dr.Close(); return false;
                    }
                }
            }
            catch (SqlException ex)
            {
                //clsLogger.WriteLog(ex.Message, ex.Source);
                dr.Close();
                return false;
            }
        }

        public bool UpdateStock(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("Sp_Parts_Update_ItemStockIssue", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SP_Parts_Update_UpdateCounterSales(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Parts_Update_UpdateCounterSales", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool PartIssues_UpdatePost(SqlTransaction Trans, string StrIssueNO, string DEALER_CODE)
        {
            string Update_GrnPost = "";
            Update_GrnPost = " Exec sp_PostStockIssueActivity '" + StrIssueNO + "','" + DEALER_CODE + "'";

            if (ExecuteSP_NonQuery(Update_GrnPost, Trans))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

       // Report Function
        public DataSet SP_Spare_Get_rptGSTInvoiceCounterSale(SqlParameter[] param)
        {
            //CounterSale Reports
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.StoredProcedure, "SP_Spare_Get_rptGSTInvoiceCounterSale", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Customer Payment Reciept
        //public ASPxComboBox FillDrp_SP(ASPxComboBox Drop, string QStr, string DropValue, string dropText,
        //SqlParameter[] param, bool setTextAtTop, string SsetAsTextTop, bool setTextAtBottom, string setAsTextBottom)
        //{
        //    /*  Fill Dropdown with SP and set defualt text  */
        //    try
        //    {
        //        DataSet ds = new DataSet();

        //        //  ds = Common.GetInstance(dbname).ExecuteDataSet(QStr);
        //        //  ds = Common.GetInstance(dbname).ExecuteDataSet(Common.GetNewSPCommand(QStr, ht, dbname));
        //        ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.StoredProcedure, QStr, param);

        //        //Drop.DataSource = ds.Tables[0];
        //        //Drop.ValueField = DropValue;
        //        //Drop.TextField = dropText;
        //        //Drop.DataBind();
        //        if (setTextAtTop == true)
        //        {
        //            //Drop.Items.Insert(0, SsetAsTextTop); Drop.SelectedIndex = 0;
        //            ListEditItem itm = new ListEditItem();
        //            itm.Value = "0";
        //            itm.Text = SsetAsTextTop;
        //            Drop.Items.Add(itm); 
        //            Drop.SelectedIndex = 0;
                    
        //        }
                
        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            ListEditItem itm = new ListEditItem();
        //            itm.Value = ds.Tables[0].Rows[i][DropValue].ToString();
        //            itm.Text = ds.Tables[0].Rows[i][dropText].ToString();
        //            Drop.Items.Add(itm);
        //        }


        //        if (setTextAtBottom == true)
        //        {
        //            int i = 0;
        //            i = Drop.Items.Count;
        //            ListEditItem itm = new ListEditItem();
        //            itm.Value = i.ToString();
        //            itm.Text = setAsTextBottom;
        //            Drop.Items.Add(itm);

        //        }
        //        //Drop.Items.Insert(0, " ");

        //        return Drop;
        //    }
        //    catch (Exception ex) { throw ex; }
        //}

        public DataTable PendingPaymentReceipt(string PaymentReceiptType, string CustomerCode, string PaymentStatus, string ReceiptNo, string InvNo,String DealerCode)
        {
            string sQuery;
            if (PaymentStatus == "UNPAID")
            {
                if (PaymentReceiptType == "Service")
                {
                    sQuery = "Select '' [S NO],InvoiceNo [RefNo],JobCardCode,CONVERT(VARCHAR(10),InvoiceDate,105) [Ref Date],round((NetAmountCustomer - PaidC),2) OutStanding,round(NetAmountCustomer,2) [Ref Amount]," +
                        "'' Adjustment,'' Remaining from CustomerInvoice " +
                        " Where DealerCode = '" + DealerCode + "' " +
                        " AND CusCode = '" + CustomerCode + "' " +
                        " AND Round(NetAmountCustomer,2) - Round(isNULL(PaidC,0),2) > 1 " +
                        " AND DelFlag = 'N' " +
                        " AND (InvType = 'Dep' OR InvType = 'Inv') Order by InvoiceNo desc"; //And InvoiceNo='"+InvNo+"' ";
                }
                else if (PaymentReceiptType == "CountrSale")
                {
                    sQuery = "Select '' [S NO],SaleInvNO [RefNo],'' as JobCardCode,CONVERT(VARCHAR(10),SaleInvDate,105) [Ref Date],round(ISNULL(InvoiceAmount,2) - ISNULL(TotReceipt,0),2) OutStanding,round(InvoiceAmount,2) [Ref Amount]," +
                        "'' Adjustment,'' Remaining,Cast(1 as Bit) CheckVal from CounterSaleMaster " +
                        " Where DealerCode = '" + DealerCode + "' " +
                        " AND CusCode = '" + CustomerCode + "' " +
                        " AND DelFlag = 'N' " +
                        " AND Round(InvoiceAmount,2) - Round(isnull(TotReceipt,0),2) > 1 " +
                        " AND PostFlag = 'Y'  "
                        
                        ;
                }
                else if (PaymentReceiptType == "Insurance")
                {
                    sQuery = "Select '' [S NO],InvoiceNo [RefNo],JobCardCode,CONVERT(VARCHAR(10),InvoiceDate,105) [Ref Date],round((NetAmountInsurance - PaidI),2) OutStanding,round(NetAmountInsurance,2) [Ref Amount]," +
                        "'' Adjustment,'' Remaining from CustomerInvoice " +
                        " Where DealerCode = '" + DealerCode + "' " +
                        " AND CusCode = '" + CustomerCode + "' " +
                        " AND Round(NetAmountInsurance,2) - Round(isNULL(PaidI,0),2) > 1 " +
                        " AND DelFlag = 'N' " +
                        " AND (InvType = 'Ins') Order by InvoiceNo desc"; //And InvoiceNo='"+InvNo+"' ";
                }
                else if (PaymentReceiptType == "Dep")
                {
                    sQuery = "Select '' [S NO],InvoiceNo [RefNo],JobCardCode,CONVERT(VARCHAR(10),InvoiceDate,105) [Ref Date],round((NetAmountCustomer - PaidC),2) OutStanding,round(NetAmountCustomer,2) [Ref Amount]," +
                        "'' Adjustment,'' Remaining from CustomerInvoice " +
                        " Where DealerCode = '" + DealerCode + "' " +
                        " AND CusCode = '" + CustomerCode + "' " +
                        " AND Round(NetAmountCustomer,2) - Round(isNULL(PaidC,0),2) > 1 " +
                        " AND DelFlag = 'N' " +
                        " AND (InvType = 'Dep') Order by InvoiceNo desc"; //And InvoiceNo='"+InvNo+"' ";
                }
                else if (PaymentReceiptType == "FreeCopon")
                {
                    sQuery = "Select '' [S NO],InvoiceNo [RefNo],JobCardCode,CONVERT(VARCHAR(10),InvoiceDate,105) [Ref Date],round((NetAmountInsurance - PaidI),0) OutStanding,round(NetAmountInsurance,0) [Ref Amount]," +
                        "'' Adjustment,'' Remaining from CustomerInvoice " +
                        " Where DealerCode = '" + DealerCode + "' " +
                        " AND CusCode = '" + CustomerCode + "' " +
                        " AND Round(NetAmountInsurance,0) - Round(isNULL(PaidI,0),0) > 0 " +
                        " AND DelFlag = 'N' " +
                        " AND (InvType = 'Ins') Order by InvoiceNo desc"; //And InvoiceNo='"+InvNo+"' ";
                }
                else
                {
                    sQuery = "Select '' [S NO],ExpendCode [RefNo],'' as JobCardCode,CONVERT(VARCHAR(10),ExpendDate,105) [Ref Date],round(TotalCost,0) [Ref Amount], " +
                            " round(ISNULL(TotalCost,0) - ISNULL(TotReceipt,0),0) OutStanding,'' Adjustment,'' Remaining,Cast(1 as Bit) CheckVal from ExpenditureMaster " +
                            " Where DealerCode = '" + DealerCode + "' " +
                            " AND CusCode = '" + CustomerCode + "' " +
                             " AND DelFlag = 'N' " +
                             " AND Round(TotalCost,0) - Round(TotReceipt,0) > 0 ";
                }
            }
            else
            {
                if (PaymentReceiptType == "Service")
                {
                    sQuery = "Select '' [S NO],PRD.InvoiceNo [RefNo],CI.JobCardCode,CONVERT(VARCHAR(10),PRD.InvoiceDate,105) [Ref Date],round(PRD.InvAmount,2) [Ref Amount]," +
                        "AdjAmount Adjustment,isNull(OutstAmount,'0') OutStanding,isNull(OutstAmount,'0')-IsNull(AdjAmount,'0') as Remaining,Cast(1 as Bit) CheckVal " +
                        "from PaymentReceiptDetail PRD inner Join CustomerInvoice CI on CI.InvoiceNo=PRD.InvoiceNo and CI.DealerCode=PRD.DealerCode " +
                        "Where PRD.DealerCode = '" + DealerCode + "' " +
                        "AND PRD.ReceiptNo = '" + ReceiptNo + "' Order by PRD.InvoiceNo desc";
                }
              else  if (PaymentReceiptType == "Dep")
                {
                    sQuery = "Select '' [S NO],PRD.InvoiceNo [RefNo],CI.JobCardCode,CONVERT(VARCHAR(10),PRD.InvoiceDate,105) [Ref Date],round(PRD.InvAmount,2) [Ref Amount]," +
                        "AdjAmount Adjustment,isNull(OutstAmount,'0') OutStanding,isNull(OutstAmount,'0')-IsNull(AdjAmount,'0') as Remaining,Cast(1 as Bit) CheckVal " +
                        "from PaymentReceiptDetail PRD inner Join CustomerInvoice CI on CI.InvoiceNo=PRD.InvoiceNo and CI.DealerCode=PRD.DealerCode " +
                        "Where PRD.DealerCode = '" + DealerCode + "' " +
                        "AND PRD.ReceiptNo = '" + ReceiptNo + "' Order by PRD.InvoiceNo desc";
                }
                else if (PaymentReceiptType == "Insurance")
                {
                    sQuery = "Select '' [S NO],PRD.InvoiceNo [RefNo],CI.JobCardCode,CONVERT(VARCHAR(10),PRD.InvoiceDate,105) [Ref Date],round(PRD.InvAmount,2) [Ref Amount]," +
                        "AdjAmount Adjustment,isNull(OutstAmount,'0') OutStanding,isNull(OutstAmount,'0')-IsNull(AdjAmount,'0') as Remaining,Cast(1 as Bit) CheckVal " +
                        "from PaymentReceiptDetail PRD inner Join CustomerInvoice CI on CI.InvoiceNo=PRD.InvoiceNo and CI.DealerCode=PRD.DealerCode " +
                        "Where PRD.DealerCode = '" + DealerCode + "' " +
                        "AND PRD.ReceiptNo = '" + ReceiptNo + "' Order by PRD.InvoiceNo desc";
                }
                else {
                    sQuery = "Select ''[S NO],PRD.InvoiceNo[RefNo],CI.SaleInvNo AS 'JobCardCode',CONVERT(VARCHAR(10), PRD.InvoiceDate, 105)[Ref Date],round(PRD.InvAmount,2)[Ref Amount]," +
                        "AdjAmount Adjustment,isNull(OutstAmount,'0') OutStanding,isNull(OutstAmount,'0')-IsNull(AdjAmount,'0') as Remaining,Cast(1 as Bit) CheckVal " +
                        "from PaymentReceiptDetail PRD " +
                        "inner Join CounterSaleMaster CI on CI.SaleInvNo=PRD.InvoiceNo and CI.DealerCode=PRD.DealerCode" +
                        " Where PRD.DealerCode = '" + DealerCode + "' " +
                        "AND PRD.ReceiptNo = '" + ReceiptNo + "'";
}
                }

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.Text, sQuery);

            if (ds.Tables[0].Rows.Count == 0) ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            return ds.Tables[0];
        }
        public DataTable PendingPaymentOutgoing(string PaymentReceiptType, string CustomerCode, string PaymentStatus, string ReceiptNo, string InvNo, String DealerCode)
        {
            string sQuery="";
            if (PaymentStatus == "UNPAID")
            {
                if(PaymentReceiptType == "P/A Invoice Payment To Vendor")
                {
                    sQuery = "Select '' [S NO],PurInvNo [RefNo] " +
",GRNNo as JobCardCode,CONVERT(VARCHAR(10), PurInvDate, 105)[Ref Date] " +
",round((TotalIncTax - TotPayment), 2) OutStanding,round(TotalIncTax, 2)[Ref Amount] " +
",'' Adjustment,'' Remaining " +
"from PurInvMaster " +
"Where DealerCode = '"+DealerCode+"' " +
"AND VendorCode = '"+CustomerCode+"' " +
"AND Round(TotalIncTax,2) -Round(isNULL(TotPayment, 0), 2) > 1  AND DelFlag = 'N' Order by PurInvDate Asc";
                }
               



            }
            else
            {
                
            }

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.Text, sQuery);

            if (ds.Tables[0].Rows.Count == 0) ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            return ds.Tables[0];
        }

        //Job Card Functions
        public string GetBrandName(string BrandCode)
        {
            string BrandName = "";
            string sql = "select * from Brand where BrandCode='"+BrandCode+"'";
            DataTable dt = new DataTable();
            dt = GetData(sql);
            if (dt.Rows.Count > 0)
            {
                BrandName = dt.Rows[0]["BrandDesc"].ToString();
            }

            return BrandName;
        }

        //New Customer Vehicle
        public bool isRegNoExistInVehicle(string RegNo)
        {
            bool isexist = false;
            string sql = "select * from Vehicle where BrandCode='" + RegNo + "'";
            DataTable dt = new DataTable();
            dt = GetData(sql);
            if (dt.Rows.Count > 0)
            {
                isexist = true;
            }
            return isexist;
        }
        public string getRegNoByChassisNo(string ChassisNo)
        {
            string retval = "";
            string sql = "select * from CustomerVehicle where ChassisNo='" + ChassisNo + "'";
            DataTable dt = new DataTable();
            dt = GetData(sql);
            if (dt.Rows.Count > 0)
            {
                retval = dt.Rows[0]["RegNo"].ToString();
            }
            return retval;
        }
        public string getRegNoByEngineNo(string EngineNo)
        {
            string retval = "";
            string sql = "select * from CustomerVehicle where EngineNo='" + EngineNo + "'";
            DataTable dt = new DataTable();
            dt = GetData(sql);
            if (dt.Rows.Count > 0)
            {
                retval = dt.Rows[0]["RegNo"].ToString();
            }
            return retval;
        }


        // Customer Invoice
        public string GetJobCardNoFromCustomerInvNo(string CustomerInvoiceNumber,string DealerCode)
        {
            string JobCardNo = "";
            string sql = "select JobCardCode from CustomerInvoice where InvoiceNo='" + CustomerInvoiceNumber + "' and DealerCode = '"+DealerCode+"'";
            //Akram insert dealer code above line
            DataTable dt = new DataTable();
            dt = GetData(sql);
            if (dt.Rows.Count > 0)
            {
                JobCardNo = dt.Rows[0]["JobCardCode"].ToString();
            }

            return JobCardNo;
        }
        public string GetCustomerCodeByJobCardNo(string JobCardNo, string DealerCode)
        {
            string CusCode = "";
            string sql = "select CusCode from JobCardMaster where JobCardCode='" + JobCardNo + "' and DealerCode = '"+DealerCode+"'";
            DataTable dt = new DataTable();
            dt = GetData(sql);
            if (dt.Rows.Count > 0)
            {
                CusCode = dt.Rows[0]["CusCode"].ToString();
            }

            return CusCode;
        }
        public void GetMultiColumnsDDL(DropDownList ddlEmp, string[] Columns, string TableName, string WhereClause, string DataValueField, string Orderby , bool boolAddDistinct,bool boolGroupbyClause,string StrHavingClause="")
        {
            SqlDataReader Reader = null;
            string strSql = "WITH maxLen AS (SELECT ";
            int CountLen=0;
            foreach (string s in Columns)
            {
                strSql = strSql + "max(len(rtrim(" + s + "))) Len"+CountLen+",";

                CountLen++;
            }
            strSql = strSql.Remove(strSql.Length - 1, 1) + " ";
            strSql = strSql + "FROM " + TableName + " with(nolock)  ";
            if (WhereClause != string.Empty)
            {
                strSql = strSql + " Where " + WhereClause + ") ";
            }
            if(boolAddDistinct==true)
                strSql = strSql + "SELECT Distinct " + Columns[0].ToString() + ",";// +DataValueFieldColumns[0].ToString();
            else
                strSql = strSql + "SELECT " + Columns[0].ToString() + ",";// +DataValueFieldColumns[0].ToString();
            CountLen = 0;
            foreach (string s in Columns)
            {
                //if (Columns[0] != s)
                //{
                if (Columns[0] == s) //if (Columns[1] == s)
                {
                    strSql = strSql + " + ' | '  + (rtrim(" + s + ")  ";
                }
                else
                {
                    strSql = strSql + " + ' | '  + rtrim(" + s + ") ";
                }

                //1 + space((1+maxLen.Len" + CountLen + ")-len(rtrim(" + s + "))) 2 + space((1+maxLen.Len" + CountLen + ")-len(rtrim(" + s + ")))}
                CountLen++;

            }
            strSql = strSql + ") As Name from " + TableName + " with(nolock),maxLen  ";
            if (WhereClause != string.Empty)
            {
                strSql = strSql + " Where " + WhereClause + " ";
            }
            if (boolGroupbyClause == true)
            {
                string strGroupBy = "Group by ";
                CountLen=0;
                foreach (string s in Columns)
                {
                    strGroupBy = strGroupBy + s + "," + "maxLen.Len" + CountLen + ",";
                        CountLen++;
                }
                strGroupBy = strGroupBy.Substring(0, strGroupBy.Length - 1);
                strSql = strSql + strGroupBy + " ";
            }
            strSql = strSql + StrHavingClause + " ";
                strSql = strSql + Orderby;
            if (Reader != null && Reader.IsClosed == false) Reader.Close();
            ExecuteQuery(strSql, ref Reader);
            ddlEmp.Items.Clear();
            ListItem item = new ListItem();
            item.Text = "Select";
            item.Value = "0";
            ddlEmp.Items.Add(item);
            while (Reader.Read())
            {
                StringWriter myWriter = new StringWriter();
                HttpUtility.HtmlDecode(Reader["NAME"].ToString().Replace(" ", "&nbsp;"), myWriter);//ddlEmp.Items.Add(myWriter.ToString());
                item = new ListItem();
                item.Text = myWriter.ToString();
                item.Value = Reader[DataValueField].ToString();
                ddlEmp.Items.Add(item);
            }
            Reader.Close();
        }
//        public void GetCustEstMultiColumnsDDL(DropDownList ddlEmp)
//        {
//            SqlDataReader Reader = null;
//            string strSql = string .Empty ;
//            strSql = "WITH maxLen AS (SELECT max(len(rtrim(CustomerEstimateCode))) CustomerEstimateCodeLen, " +
//"max(len(rtrim(Convert(Varchar(10),tdDate,105)))) tdDateLen, " +
//"max(len(rtrim(RegNo))) RegNoLen, " +
//"max(len(rtrim(UserName))) UserNameLen " +
//"FROM CustomerEstimateMaster with(nolock) " +
//"Where DealerCode ='00001'  And DelFlag='N') " +
////"Select 'Code', "+
//// "+ ' | '  + 'CustomerEstimateCode' "+
////"+ ' | '  + 'tdDate' "+
////"+ ' | '  + 'RegNo' "+
////"+ ' | '  + 'UserName' As Name "+
//// "UNION " +
//"SELECT CustomerEstimateCode, " +
//"+ ' | '  + (rtrim(CustomerEstimateCode) + space((1+maxLen.CustomerEstimateCodeLen)-len(rtrim(CustomerEstimateCode)))  " +
//"+ ' | '  + rtrim(Convert(Varchar(10),tdDate,105)) + space((1+maxLen.tdDateLen)-len(rtrim(Convert(Varchar(10),tdDate,105)))) " +
//"+ ' | '  + rtrim(RegNo) + space((1+maxLen.RegNoLen)-len(rtrim(RegNo)))  " +
//"+ ' | '  + rtrim(UserName) + space((1+maxLen.UserNameLen)-len(rtrim(UserName))) ) As Name " +
//"from CustomerEstimateMaster with(nolock),maxLen   Where DealerCode ='00001'  And DelFlag='N' ";
//            if (Reader != null && Reader.IsClosed == false) Reader.Close();
//            ExecuteQuery(strSql, ref Reader);
//            ddlEmp.Items.Clear();
//            ListItem item = new ListItem();
//            item.Text = "Select";
//            item.Value = "0";
//            ddlEmp.Items.Add(item);
//            while (Reader.Read())
//            {
//                StringWriter myWriter = new StringWriter();
//                HttpUtility.HtmlDecode(Reader["NAME"].ToString().Replace(" ", "&nbsp;"), myWriter);//ddlEmp.Items.Add(myWriter.ToString());
//                item = new ListItem();
//                item.Text = myWriter.ToString();
//                item.Value = Reader["CustomerEstimateCode"].ToString();
//                ddlEmp.Items.Add(item);
//            }
//            Reader.Close();
//        }
        public void Load_CounterSalesItem_ddl(DropDownList ddlCSItem , string DealerCode,string strLubeFlag= "PL")
        {
            SqlDataReader Reader = null;
            string strSql = string.Empty;
            strSql = "WITH maxLen AS " +
                    "(SELECT max(len(rtrim(A.ItemCode))) Len0, " +
                    "max(len(rtrim(A.ItemDesc))) Len1,  " +
                    "max(len(rtrim(A.PartItemNo))) Len2  " +
                    "FROM Item A Left outer Join ItemStock B On A.ItemCode=B.ItemCode And A.DealerCode=B.DealerCode ";
            if (strLubeFlag == "PL")
                strSql = strSql +"Where  A.DealerCode='" + DealerCode + "' AND Source not in ('C.K.D.' , 'LOCAL')) ";
            else if (strLubeFlag == "P")
                strSql = strSql + "Where  A.DealerCode='" + DealerCode + "' And A.LubeFlag = 'N')  ";
            else if (strLubeFlag=="L")
                strSql = strSql + "Where  A.DealerCode='" + DealerCode + "' And A.LubeFlag = 'Y')  ";
            strSql = strSql + "SELECT A.ItemCode, + ' | '  + (rtrim(A.ItemCode)" +
            "+ ' | '  + rtrim(A.ItemDesc)  " +
            "+ ' | '  + rtrim(A.PartItemNo)  " +
            "+ ' | '  + rtrim(IsNull(isnull(Sum(B.INQty),0) + Sum(B.RecQty) + Sum(B.PurRetQty) + Sum(B.RetQty) - ((Sum(B.IssQty) + Sum(B.ChargeOutQty)) ), 0)) ) As Name  " +
            "from Item A Left outer Join ItemStock B On A.ItemCode=B.ItemCode And A.DealerCode=B.DealerCode ,maxLen  ";
            

            if (strLubeFlag == "PL")
                strSql = strSql + "Where  A.DealerCode='" + DealerCode + "' AND Source not in ('C.K.D.' , 'LOCAL') ";
            else if (strLubeFlag == "P")
                strSql = strSql + "Where  A.DealerCode='" + DealerCode + "' And A.LubeFlag = 'N'  ";
            else if (strLubeFlag == "L")
                strSql = strSql + "Where  A.DealerCode='" + DealerCode + "' And A.LubeFlag = 'Y'  ";
//"And A.LubeFlag = 'N'  " +
            strSql = strSql + " Group by A.ItemCode,maxLen.Len0,A.ItemDesc,maxLen.Len1,A.PartItemNo,maxLen.Len2 Order by A.PartItemNo ";

            if (Reader != null && Reader.IsClosed == false) Reader.Close();
            ExecuteQuery(strSql, ref Reader);
            ddlCSItem.Items.Clear();
            ListItem item = new ListItem();
            item.Text = "Select";
            item.Value = "0";
            ddlCSItem.Items.Add(item);
            while (Reader.Read())
            {
                StringWriter myWriter = new StringWriter();
                HttpUtility.HtmlDecode(Reader["NAME"].ToString().Replace(" ", "&nbsp;"), myWriter);//ddlEmp.Items.Add(myWriter.ToString());
                item = new ListItem();
                item.Text = myWriter.ToString();
                item.Value = Reader["ItemCode"].ToString();
                ddlCSItem.Items.Add(item);

            }
            Reader.Close();
        }
        
        public bool sp_W2_PRMaster_Insert(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("sp_2W_PRMaster_insert", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool sp_W2_PRDetail_Insert(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("sp_2W_PRDetail_Insert", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool sp_W2_GRNDetail_Update_PR(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("sp_2W_GRNDetail_Update_PR", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool sp_W2_GRNMaster_Update_PR(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("sp_2W_GRNMaster_Update_PR", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet Get_Parts_Get_PRDetail(SqlParameter[] param)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.StoredProcedure, "sp_W2_Get_PRDetail", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet SP_Spare_Get_PartLocation(SqlParameter[] param)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.StoredProcedure, "SP_Spare_Get_PartLocation", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SP_Spare_Delete_IssueMaster(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Spare_Delete_IssueMaster", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SP_Spare_Delete_IssueDetail(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Spare_Delete_IssueDetail", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet SP_Spare_Get_PartsSubLocation(SqlParameter[] param)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.StoredProcedure, "SP_Spare_Get_PartsSubLocation", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet SP_Spare_Get_PartsTransLocation(SqlParameter[] param)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.StoredProcedure, "SP_Spare_Get_PartsTransLocation", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SP_Spare_Insert_PartsTransLocation(SqlParameter[] param)
        {
            try
            {
                ExecuteSP_NonQuery( "SP_Spare_Insert_PartsTransLocation", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SP_Spare_Delete_PartsTransLocation(SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Spare_Delete_PartsTransLocation", param))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool sp_2W_SRMaster_Insert(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("sp_2W_SRMaster_Insert", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool sp_W2_SRDetail_Insert(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("sp_2W_SRDetail_Insert", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet sp_W2_Get_SRDetail(SqlParameter[] param)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(CConn.CConnection.GetConnectionString(), CommandType.StoredProcedure, "sp_W2_Get_SRDetail", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ClearTextBoxes(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                TextBox textBox = child as TextBox;
                
                
                if (textBox == null)
                    ClearTextBoxes(child);
                else
                    textBox.Text = string.Empty;
            }
        }
        public void Clearddl(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                DropDownList ddl = child as DropDownList;


                if (ddl == null)
                    Clearddl(child);
                else
                {
                    if (ddl.Items.Count > 0)
                    {
                        ddl.SelectedIndex = 0;
                    }
                }
            }
        }

        public DataSet SP_Spare_Get_DCOPInfo(SqlParameter[] param)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(CConn.CConnection.GetConnectionString(), CommandType.StoredProcedure, "SP_Spare_Get_DCOPInfo", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet SP_Part_DCGRNData(SqlParameter[] param)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(CConn.CConnection.GetConnectionString(), CommandType.StoredProcedure, "SP_Part_DCGRNData", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SP_Spare_Delete_GRNMaster(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Spare_Delete_GRNMaster", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SP_Spare_Delete_GRNDetail(SqlTransaction Trans, SqlParameter[] param)
        {
            try
            {
                if (ExecuteSP_NonQuery("SP_Spare_Delete_GRNDetail", param, Trans))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable PendingPaymentReceipt(string DealerCode,string PaymentReceiptType, string CustomerCode, string PaymentStatus, string ReceiptNo, string InvNo, string InvType = "C", string InsCode = "0000", string BranchCode = "0000")
        {
            string sQuery;
            if (PaymentStatus == "UNPAID")
            {
                if (PaymentReceiptType == "Service")
                {
                    if (InvType == "C")
                    {
                        sQuery = "Select '' [S NO],InvoiceNo [Ref No],CONVERT(VARCHAR(10),InvoiceDate,105) [Ref Date],round(NetAmountCustomer,0) [Ref Amount]," +
                            "round((NetAmountCustomer - PaidC),0) OutStanding,'' Adjustment,'' Remaining from CustomerInvoice " +
                            " Where DealerCode = '" + DealerCode + "' " +
                            " AND CusCode = '" + CustomerCode + "' " +
                            " AND Round(NetAmountCustomer,0) - Round(isNULL(PaidC,0),0) > 1 " +
                            " AND DelFlag = 'N' " +
                            " AND (InvType = 'Dep' OR InvType = 'Inv') Order by InvoiceNo desc"; //And InvoiceNo='"+InvNo+"' ";
                    }
                    else
                    {
                        sQuery = "Select '' [S NO],InvoiceNo [Ref No],CONVERT(VARCHAR(10),InvoiceDate,105) [Ref Date],round(NetAmountInsurance,0) [Ref Amount]," +
                            "round((NetAmountInsurance - PaidI),0) OutStanding,'' Adjustment,'' Remaining from CustomerInvoice " +
                            " Where DealerCode = '" + DealerCode + "' " +
                            " AND InsCode = '" + InsCode + "' And BranchCode='" + BranchCode + "' " +
                            " AND Round(NetAmountInsurance,0) - Round(isNULL(PaidI,0),0) > 1 " +
                            " AND DelFlag = 'N' " +
                            " AND InvType = 'Ins' Order by InvoiceNo desc";
                    }
                }
                else if (PaymentReceiptType == "CounterSale")
                {
                    sQuery = "Select '' [S NO],SaleInvNO [Ref No],CONVERT(VARCHAR(10),SaleInvDate,105) [Ref Date],round(InvoiceAmount,0) [Ref Amount]," +
                        "round(ISNULL(InvoiceAmount,0) - ISNULL(TotReceipt,0),0) OutStanding,'' Adjustment,'' Remaining,Cast(1 as Bit) CheckVal from CounterSaleMaster " +
                        " Where DealerCode = '" + DealerCode + "' " +
                        " AND CusCode = '" + CustomerCode + "' " +
                        " AND DelFlag = 'N' " +
                        " AND Round(InvoiceAmount,0) - Round(TotReceipt,0) > 1 " +
                        " AND PostFlag = 'Y'  ";
                }
                else
                {
                    sQuery = "Select '' [S NO],ExpendCode [Ref No],CONVERT(VARCHAR(10),ExpendDate,105) [Ref Date],round(TotalCost,0) [Ref Amount], " +
                            " round(ISNULL(TotalCost,0) - ISNULL(TotReceipt,0),0) OutStanding,'' Adjustment,'' Remaining,Cast(1 as Bit) CheckVal from ExpenditureMaster " +
                            " Where DealerCode = '" + DealerCode + "' " +
                            " AND CusCode = '" + CustomerCode + "' " +
                             " AND DelFlag = 'N' " +
                             " AND Round(TotalCost,0) - Round(TotReceipt,0) > 1 ";
                }
            }
            else
            {
                sQuery = "Select '' [S NO],InvoiceNo [Ref No],CONVERT(VARCHAR(10),InvoiceDate,105) [Ref Date],round(InvAmount,0) [Ref Amount]," +
                        "OutstAmount OutStanding,AdjAmount Adjustment,isNull(OutstAmount,'0')-IsNull(AdjAmount,'0') as Remaining,Cast(1 as Bit) CheckVal " +
                        "from PaymentReceiptDetail " +
                        "Where DealerCode = '" + DealerCode + "' " +
                        "AND ReceiptNo = '" + ReceiptNo + "' Order by InvoiceNo desc";
            }

            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(CConn.CConnection.GetConnectionString(), CommandType.Text, sQuery);

            if (ds.Tables[0].Rows.Count == 0) ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            return ds.Tables[0];
        }
        public DataSet Get_Parts_Get_StockRetDetail(SqlParameter[] param)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(CConn.CConnection.GetConnectionString(), CommandType.StoredProcedure, "sp_W2_Get_StockRetDetail", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}