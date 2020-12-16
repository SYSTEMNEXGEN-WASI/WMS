using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DXBMS;

namespace DXBMS.Modules.Setup
{
    public partial class CVO : System.Web.UI.Page
    {
        SysFunction sysFunc = new SysFunction();
        SysFunctions sysFuncs = new SysFunctions();
        DataSet ds;
        Transaction ObjTrans = new Transaction();
        clsLookUp clslook = new clsLookUp();
        DataTable dt = new DataTable();
        SqlTransaction Trans;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack)
            {
                if (!string.IsNullOrEmpty((string)Session["LookUpData"]))
                {

                    Set_values(Session["LookUpData"].ToString());

                }
            }

            if (!IsPostBack)
            {
                createGrid();
                txtCVODate.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
            }

            Session["LookUpData"] = string.Empty;
        }



        private void createGrid()
        {

            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("CVONo", typeof(string)));
            dt.Columns.Add(new DataColumn("CVODate", typeof(string)));
            dt.Columns.Add(new DataColumn("OwnerShipType", typeof(string)));
            dt.Columns.Add(new DataColumn("OldCusCode", typeof(string)));
            dt.Columns.Add(new DataColumn("NewCusCode", typeof(string)));
            dt.Columns.Add(new DataColumn("Remarks", typeof(string)));
            dt.Columns.Add(new DataColumn("UpdUser", typeof(string)));
            dt.Columns.Add(new DataColumn("UpdDate", typeof(string)));
            dt.Columns.Add(new DataColumn("UpdTime", typeof(string)));
            dt.Columns.Add(new DataColumn("UpdTerm", typeof(string)));

            gv_CVO.DataSource = dt;
            gv_CVO.DataBind();

            Session["CVODS"] = dt;

        }


        protected void gv_CVO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && !(gv_CVO.EditIndex == e.Row.RowIndex))
                {
                    e.Row.Cells[0].Enabled = true;
                }

                if (e.Row.RowType == DataControlRowType.DataRow && (gv_CVO.EditIndex == e.Row.RowIndex))
                {
                    TextBox txtreg = (TextBox)e.Row.FindControl("txtreg");

                    // txtreg.Text = Session["lblUnitDesc"].ToString();
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {

                }
            }
            catch (Exception ex)
            {
                // lblMessage.Visible = true;
                //lblMessage.Text = ex.Message;
            }
        }

        protected void gv_CVO_DataBound(object sender, EventArgs e)

        {
            try
            {

            }
            catch (Exception ex)
            {
                //lblMessage.Visible = true;
                // lblMessage.Text = ex.Message;
            }
        }
        protected void imgVehRegNo_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 3; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            string sql = "Select RegNo [Reg No], ChassisNo [Chassis No], EngineNo [Engine No], C.CusDesc [Customer] from CustomerVehicle A inner join Customer C on C.CusCode = A.CusCode and C.DealerCode = A.DealerCode";
            clslook.LU_Get_RecVeh(imgVehRegNo, ViewState["lookupid"].ToString(), "", sql, "../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void Set_values(string Item)
        {
            if (ViewState["lookupid"].ToString() == "3")
            {

                txtreg.Text = Item;
                string sql = "SP_Get_VechicleDataForCVO '" + Session["DealerCode"].ToString() + "','" + Item + "'";

                DataTable dt = new DataTable();

                dt = sysFunc.GetData(sql);

                if (dt.Rows.Count > 0)
                {
                    txtreg.Text = dt.Rows[0]["RegNo"].ToString();
                    txtChassisNo.Text = dt.Rows[0]["ChassisNo"].ToString();
                    txtEngineNo.Text = dt.Rows[0]["EngineNo"].ToString();
                    txtVehicle.Text = dt.Rows[0]["ProdCode"].ToString();
                    txtColor.Text = dt.Rows[0]["ColorDesc"].ToString();
                    txtOldcustomer.Text = dt.Rows[0]["CusDesc"].ToString();
                    txtOldCusCode.Text = dt.Rows[0]["CusCode"].ToString();

                }
                String query = "SP_Get_CVODataByRegNo '" + Session["DealerCode"].ToString() + "','" + txtreg.Text + "'";


                dt = sysFunc.GetData(query);

                if (dt.Rows.Count > 0)
                {
                    gv_CVO.DataSource = dt;
                    gv_CVO.DataBind();
                }

                Session["CVODS"] = dt;
            }
            else if (ViewState["lookupid"].ToString() == "1")
            {
                txtNewCusDesc.Text = sysFuncs.GetStringValuesAgainstCodes("CusCode", Item, "CusDesc", "Customer",  Session["DealerCode"].ToString());
                txtNewcustomer.Text = Item;
            }
            else if (ViewState["lookupid"].ToString() == "76")
            {
                txtCVONo.Text = Item;
                String query = "SP_Get_CVOData'" + Session["DealerCode"].ToString() + "','" + Item + "'";
                dt = sysFunc.GetData(query);

                if (dt.Rows.Count > 0)
                {
                    gv_CVO.DataSource = dt;
                    gv_CVO.DataBind();
                }

                Session["CVODS"] = dt;
            }

        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            if (txtreg.Text == "")
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Reg No. should not left blank", txtreg);
                return;
            }
            if (txtNewcustomer.Text == "")
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Customer should not left blank", txtreg);
                return;
            }
            if (txtRemarks.Text == "")
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Remarks should not left blank", txtreg);
                return;
            }


            DataTable dt = (DataTable)Session["CVODS"];

            try
            {
                string max = sysFuncs.GetNewMaxID("CVO", "CVONo", 8, Session["DealerCode"].ToString());
                SqlParameter[] param =
               {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                new SqlParameter("@CVONo",SqlDbType.Char,8), //1
                new SqlParameter("@CVODate",SqlDbType.DateTime),//2
                new SqlParameter("@RegNo",SqlDbType.VarChar,50),//3
                new SqlParameter("@ChassisNo",SqlDbType.VarChar,50),//4
                new SqlParameter("@EngineNo",SqlDbType.VarChar,50),//5
                new SqlParameter("@BrandCode",SqlDbType.Char,3),//6
                new SqlParameter("@ProdCode",SqlDbType.VarChar,10),//7
                new SqlParameter("@VersionCode",SqlDbType.Char,3),//8
                new SqlParameter("@ColorCode",SqlDbType.Char,5),//9
                new SqlParameter("@OwnerShipType",SqlDbType.VarChar,50),//10
                new SqlParameter("@OldCusCode",SqlDbType.VarChar,50),//11
                new SqlParameter("@NewCusCode",SqlDbType.VarChar,50),//12
                new SqlParameter("@Remarks",SqlDbType.VarChar,50),//13
                new SqlParameter("@UpdUser",SqlDbType.VarChar,50), //14
                new SqlParameter("@UpdDate",SqlDbType.DateTime), //15
                new SqlParameter("@UpdTime",SqlDbType.DateTime), //16
                new SqlParameter("@UpdTerm",SqlDbType.VarChar,50),// 17                             
            };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = max;
                param[2].Value = sysFunc.SaveDate(txtCVODate.Text);
                param[3].Value = txtreg.Text;
                param[4].Value = txtChassisNo.Text;
                param[5].Value = txtEngineNo.Text;
                param[6].Value = "";
                param[7].Value = txtVehicle.Text;
                param[8].Value = "";
                param[9].Value = "";
                param[10].Value = ddlownership.SelectedValue;
                param[11].Value = txtOldCusCode.Text;
                param[12].Value = txtNewcustomer.Text;
                param[13].Value = txtRemarks.Text;
                param[14].Value = Session["UserName"].ToString();
                param[15].Value = sysFunc.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"));
                param[16].Value = DateTime.Now;
                param[17].Value = "1";

                if (sysFunc.ExecuteSP_NonQuery("SP_Insert_CVO", param))
                {
                    //sysFunc.ClearTextBoxes(Page);
                    createGrid();
                    sysFuncs.UserMsg(lblMessage, Color.Green, "Data Inserted", txtreg);

                }
                else
                {
                    sysFuncs.UserMsg(lblMessage, Color.Green, "Data Not Inserted", txtreg);
                }

            }
            catch (Exception ex)
            {

                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);
            }
        }



        protected void imgNewCus_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 1; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            string sql = "Select A.CusCode [Customer] , A.CusDesc [Customer Description] , A.Address1 [Address] ,A.CellNo [Cell No] from Customer A ";
            clslook.LU_Get_RecVeh(imgVehRegNo, ViewState["lookupid"].ToString(), "", sql, "../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {

            Session["LookUpData"] = null;
            txtreg.Text = "";
            txtChassisNo.Text = "";
            txtColor.Text = "";
            txtEngineNo.Text = "";
            txtVehicle.Text = "";
            txtNewcustomer.Text = "";
            txtOldCusCode.Text = "";
            txtOldcustomer.Text = "";
            txtRemarks.Text = "";
            txtNewCusDesc.Text = "";
            txtCVONo.Text = "";
            createGrid();

        }

        protected void imgCVO_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 76; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            string sql = "Select A.CVONo [CVO No] , Format(A.CVODate,'dd-MM-yyyy') [CVO Date] , A.ChassisNo [Chassis No] ,B.CusDesc [Customer Description] from CVO A , Customer B";
            string append = "and A.DealerCode = B.DealerCode and A.NewCusCode = B.CusCode";
            clslook.LU_Get_RecVeh(imgVehRegNo, ViewState["lookupid"].ToString(), append, sql, "../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }
    }
}