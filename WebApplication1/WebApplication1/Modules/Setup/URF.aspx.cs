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
    public partial class URF : System.Web.UI.Page
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
                txtURFDate.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
            }
            Session["LookUpData"] = string.Empty;

        }

        protected void imgVehChassisNo_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 3; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            string sql = "Select  ChassisNo [Chassis No],RegNo [Reg No], EngineNo [Engine No], C.CusDesc [Customer] from CustomerVehicle A inner join Customer C on A.CusCode = C.CusCode and C.DealerCode = A.DealerCode";
            clslook.LU_Get_RecVeh(imgVehChassisNo, ViewState["lookupid"].ToString(), "", sql, "../../../");
        }
        protected void Set_values(string Item)
        {
            if (ViewState["lookupid"].ToString() == "3")
            {

                txtChassis.Text = Item;
                string sql = "SP_Get_VechicleDataForURF '" + Session["DealerCode"].ToString() + "','" + Item + "'";

                DataTable dt = new DataTable();

                dt = sysFunc.GetData(sql);

                if (dt.Rows.Count > 0)
                {
                    // txtreg.Text = dt.Rows[0]["RegNo"].ToString();
                    txtChassis.Text = dt.Rows[0]["ChassisNo"].ToString();
                    txtEngineNo.Text = dt.Rows[0]["EngineNo"].ToString();
                    txtVehicle.Text = dt.Rows[0]["ProdCode"].ToString();
                    txtColor.Text = dt.Rows[0]["ColorDesc"].ToString();
                    txtCusDesc.Text = dt.Rows[0]["CusDesc"].ToString();
                    //txtOldCusCode.Text = dt.Rows[0]["CusCode"].ToString();

                }
                //String query = "SP_Get_CVODataByRegNo '" + Session["DealerCode"].ToString() + "','" + Item + "'";


                //dt = sysFunc.GetData(query);

                //if (dt.Rows.Count > 0)
                //{
                //    gv_URF.DataSource = dt;
                //    gv_URF.DataBind();
                //}

                Session["URFDS"] = dt;
            }


        }




        protected void btnInsert_Click(object sender, EventArgs e)
        {
            if (txtChassis.Text == "")
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Chassis No. should not left blank", txtChassis);
                return;
            }
            if (txtNewreg.Text == "")
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Registration Feilds should not left blank", txtNewreg);
                return;
            }
            //if (txtRemarks.Text == "")
            //{
            //    sysFuncs.UserMsg(lblMessage, Color.Red, "Remarks should not left blank", txtRemarks);
            //    return;
            //}


            DataTable dt = (DataTable)Session["URFDS"];
            // Check value in GridView TextBOX
            foreach (DataRow d in dt.Rows)
            {
                if (d["RegNo"].ToString().ToUpper() == txtNewreg.Text.ToUpper())
                {
                    sysFunc.UserMsg(lblMessage, Color.Red, "Alredy Exist");
                    return;
                }
            }

            try
            {
                // string max = sysFuncs.GetNewMaxID("URF", "URFNo", 8, Session["DealerCode"].ToString());
             
                SqlParameter[] param =
               {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                new SqlParameter("@ChassisNo",SqlDbType.VarChar,30),//4
                new SqlParameter("@EngineNo",SqlDbType.VarChar,30),//5
                new SqlParameter("@RegNo",SqlDbType.Char,15),//3

                
                                     
            };
              



                param[0].Value = Session["DealerCode"].ToString();
              
                param[1].Value = txtChassis.Text;
                param[2].Value = txtEngineNo.Text;
                param[3].Value = txtNewreg.Text.Trim().ToUpper().ToUpper();


                //SP_Insert_URF
                if (sysFunc.ExecuteSP_NonQuery("sp_Update_URF", param))
                {
                    //sysFunc.ClearTextBoxes(Page);
                    createGrid();
                    sysFuncs.UserMsg(lblMessage, Color.Green, "Reg No Update", txtChassis);

                }
                else
                {
                    sysFuncs.UserMsg(lblMessage, Color.Green, "RegNo Not Update", txtChassis);
                }

               

            }
            catch (Exception ex)
            {

                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);
            }
        }
        private void createGrid()
        {
            string query = "Select URFNo, Format(URFDate,'dd-MM-yyyy') [URFDate], ChassisNo, RegNo, Remarks from URF where dealercode = '" + Session["DealerCode"].ToString() + "' ";

            DataTable dt = new DataTable();

            dt = sysFunc.GetData(query);

            if (dt != null && dt.Rows.Count > 0)
            {
                gv_URF.DataSource = dt;
                gv_URF.DataBind();
            }

            Session["URFDS"] = dt;
        }
        //private void createGrid()
        //{

        //    DataTable dt = new DataTable();

        //    dt.Columns.Add(new DataColumn("URFNo", typeof(string)));
        //    dt.Columns.Add(new DataColumn("URFDate", typeof(string)));
        //    dt.Columns.Add(new DataColumn("ChassisNo", typeof(string)));
        //    dt.Columns.Add(new DataColumn("CusCode", typeof(string)));
        //    dt.Columns.Add(new DataColumn("RegNo", typeof(string)));
        //    dt.Columns.Add(new DataColumn("Remarks", typeof(string)));
        //    dt.Columns.Add(new DataColumn("UpdUser", typeof(string)));
        //    dt.Columns.Add(new DataColumn("UpdDate", typeof(string)));
        //    dt.Columns.Add(new DataColumn("UpdTime", typeof(string)));
        //    dt.Columns.Add(new DataColumn("UpdTerm", typeof(string)));

        //    gv_URF.DataSource = dt;
        //    gv_URF.DataBind();

        //    Session["URFDS"] = dt;


        //}
        //protected void SelectedPartDetail(string item)
        //{

        //    DataTable dt = new DataTable();
        //    try
        //    {
        //            if (ddlJobCardTypeCode.SelectedValue == "012")
        //            {
        //                txtDep.ReadOnly = false;
        //            }
        //            else
        //            {
        //                txtDep.ReadOnly = true;

        //            }

        protected void gv_URF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && !(gv_URF.EditIndex == e.Row.RowIndex))
                {
                    e.Row.Cells[0].Enabled = true;
                }

                if (e.Row.RowType == DataControlRowType.DataRow && (gv_URF.EditIndex == e.Row.RowIndex))
                {
                    TextBox txtChassis = (TextBox)e.Row.FindControl("txtChassis");

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

        protected void gv_URF_DataBound(object sender, EventArgs e)

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




        protected void btnClear_Click(object sender, EventArgs e)
        {
            Session["LookUpData"] = null;
            txtChassis.Text = "";
            txtNewreg.Text = "";
            txtColor.Text = "";
            txtEngineNo.Text = "";
            txtVehicle.Text = "";
            txtCusDesc.Text = "";
            txtRemarks.Text = "";
            txtURFDate.Text = "";
            txtURFNo.Text = "";
            lblMessage.Text = "";
            createGrid();
        }

        protected void gv_URF_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_URF.PageIndex = e.NewPageIndex;
            createGrid();
        }
    }
}