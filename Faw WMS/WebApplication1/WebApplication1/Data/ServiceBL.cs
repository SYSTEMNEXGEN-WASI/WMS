using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using DXBMS;
using System.Web.UI.WebControls;

public class ServiceBL
{
    
    static string StrCCon;
    public string TextBoxId { get; set; }
    public ServiceBL()
    {

        StrCCon = GlobalVar.mDataConnection;
    }

    public DataSet Fill_FTPGV(SqlParameter[] param)
    {
        DataSet ds = new DataSet();
        ds = SqlHelper.ExecuteDataset(StrCCon, CommandType.StoredProcedure, "Get_GVFTPDateWise", param);
        return ds;
    }

    #region Create DataTable Add Rows and returns as per given params.
    public DataTable AddBlankRows(string[] Cols, int CurrentRows, int ColToAdd)
    {
        DataTable dt = new DataTable();
        if (CurrentRows == 0)
        {
            for (int j = 0; j <= Cols.Length - 1; j++)
            {
                dt.Columns.Add(new DataColumn(Cols.GetValue(j).ToString(), typeof(string)));

            }
        }
        for (int i = 0; i <= ColToAdd - 1; i++)
        {
            try
            {
                if (dt.Rows[i] == null)
                {
                    dt.Rows.Add(new object[] { null });
                }
            }
            catch (Exception ex)
            {
                string errText = "There is no row at position " + i + ".";
                if (ex.Message.ToString() == errText)
                {
                    dt.Rows.Add(new object[] { null });
                }
            }
        }

        return dt;
    }
    #endregion
    #region Insert Into Table
    public bool InsertQuery(string SpName, params SqlParameter[] commandParameters)
    {
        int i = SqlHelper.ExecuteNonQuery(StrCCon, SpName, commandParameters);
        return (i == 1 ? true : false);
    }
    #endregion


    #region GetRECORDS from any Store Procedure.
    public DataSet GetRECORDS(string SpName, params SqlParameter[] commandParameters)
    {
        DataSet ds = new DataSet();
        ds = SqlHelper.ExecuteDataset(StrCCon, CommandType.StoredProcedure, SpName, commandParameters);
        return ds;
    }

    #endregion





}