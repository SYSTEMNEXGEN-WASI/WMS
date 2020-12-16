using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.UI;

namespace DXBMS
{
    public class MainBLL
    {
        public DropDownList FillDrp_SP(DropDownList Drop, string QStr, string DropValue, string dropText,
       SqlParameter[] param, bool setTextAtTop, string SsetAsTextTop, bool setTextAtBottom, string setAsTextBottom)
        {
            /*  Fill Dropdown with SP and set defualt text  */
            try
            {
                DataSet ds = new DataSet();

                //  ds = Common.GetInstance(dbname).ExecuteDataSet(QStr);
                //  ds = Common.GetInstance(dbname).ExecuteDataSet(Common.GetNewSPCommand(QStr, ht, dbname));
                ds = SqlHelper.ExecuteDataset(CConn.CConnection.GetConnectionString(), CommandType.StoredProcedure, QStr, param);

                Drop.DataSource = ds.Tables[0];
                Drop.DataValueField = DropValue;
                Drop.DataTextField = dropText;
                Drop.DataBind();
                if (setTextAtTop == true)
                {
                    Drop.Items.Insert(0, SsetAsTextTop); Drop.SelectedIndex = 0;
                }
                if (setTextAtBottom == true)
                {
                    int i = 0;
                    i = Drop.Items.Count;
                    Drop.Items.Insert(i, setAsTextBottom);
                }
                //Drop.Items.Insert(0, " ");

                return Drop;
            }
            catch (Exception ex) { throw ex; }
        }
        public void FillDropDown(DropDownList ddlFill, string strQuery, string DataTextField, string DataValueField, string StartText)
        {
            //Fill document Type DropDown
            DataSet ds1 = new DataSet();
            ds1 = SqlHelper.ExecuteDataset(GlobalVar.mDataConnection, CommandType.Text, strQuery);
            ddlFill.DataSource = ds1.Tables[0];
            ddlFill.DataTextField = DataTextField;
            ddlFill.DataValueField = DataValueField;
            ddlFill.DataBind();
            if (StartText != string.Empty)
            {
                ddlFill.Items.Insert(0, new ListItem(StartText, String.Empty));
                ddlFill.SelectedIndex = 0;
            }
            ///////////////"--Select--"
        }

        public void ShowMessageBox(string msg, Control obj)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string message = msg;
            sb.Append("alert('" + message + "');");
            ScriptManager.RegisterClientScriptBlock(obj, obj.GetType(), "Prompt Message", sb.ToString(), true);
        }

        public void ShowMsgBoxConfirm(string msg, Control obj)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string message = msg;
            sb.Append("confirm('" + message + "');");
            ScriptManager.RegisterClientScriptBlock(obj, obj.GetType(), "Prompt Message", sb.ToString(), true);

        }

        internal object FillDataSet(string p, SqlParameter[] param)
        {
            throw new NotImplementedException();
        }

        internal bool ExecuteQuery(string sQuery, ref DataSet ds)
        {
            throw new NotImplementedException();
        }
    }
}