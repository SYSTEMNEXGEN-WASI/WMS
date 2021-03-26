using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//Database Libraries
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DXBMS;

namespace DXBMS.Modules.Setup
{
    public partial class Unit : System.Web.UI.Page
    {
        SysFunction sysFunc = new SysFunction();
        SysFunctions sysFuncs = new SysFunctions();
        DataSet ds;
        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                createGrid();
            }
        }
        ///Insert Button
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            if (txtunitdec.Text == "")
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please Enter Unit  Description", txtunitdec);
                return;
            }
            else
            {
                lblMessage.Text = string.Empty;
            }

            DataTable dt = (DataTable)Session["UnitCategoryDS"];


            // Check value in GridView TextBOX
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["UnitDesc"].ToString().ToUpper() == txtunitdec.Text.ToUpper())
                {
                    sysFunc.UserMsg(lblMessage, Color.Red, "Alredy Exist");
                    return;
                }
            }



            try
            {
                string max = sysFuncs.GetNewMaxID("Unit", "UnitCode", 4, Session["DealerCode"].ToString());
                SqlParameter[] param =
               {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                new SqlParameter("@UnitCode",SqlDbType.Char,4), //1
                new SqlParameter("@UnitDesc",SqlDbType.VarChar,50),//2
                new SqlParameter("@UpdUser",SqlDbType.VarChar,50), //3
                new SqlParameter("@UpdDate",SqlDbType.DateTime), //4
                new SqlParameter("@UpdTime",SqlDbType.DateTime), //5
                new SqlParameter("@UpdTerm",SqlDbType.VarChar,50),//6                              
            };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = max;

                param[2].Value = txtunitdec.Text.ToUpper().Trim();
                param[3].Value = Session["UserName"].ToString();
                param[4].Value = sysFunc.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"));
                param[5].Value = DateTime.Now;
                param[6].Value = "1";

                sysFunc.ExecuteSP_NonQuery("SP_Insert_Unit", param);
                sysFunc.ClearTextBoxes(Page);

                createGrid();
                sysFuncs.UserMsg(lblMessage, Color.Green, "Data Inserted", txtunitdec);
                //lblMessage.Text = "New Unit Category Inserted.";
            }
            catch (Exception ex)
            {

                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);
            }
        }
        private void createGrid()
        {
            String query = "select * from Unit where DealerCode in( '" + Session["DealerCode"].ToString() + "','COMON') ";

            DataTable dt = new DataTable();

            dt = sysFunc.GetData(query);

            if (dt.Rows.Count > 0)
            {
                gv_Unit.DataSource = dt;
                gv_Unit.DataBind();
            }

            Session["UnitCategoryDS"] = dt;
        }

        // Remove Button Code
        protected void BtnRemove_Unit_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsdelete = new DataSet();

            DataTable dt = new DataTable();

            try
            {
                dt = (DataTable)Session["UnitCategoryDS"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex + gv_Unit.PageIndex * gv_Unit.PageSize;

                String code = dt.Rows[rowGvIndex]["UnitCode"].ToString();

                if (sysFunc.CodeExists("Item", "UnitCode", code, Session["DealerCode"].ToString(), ref dsdelete))
                {
                    lblMessage.Text = "Cannot delete , this Unit Code is already in use";
                    return;
                }

                dt.Rows[rowGvIndex].Delete();
                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                gv_Unit.DataSource = dt;
                gv_Unit.DataBind();

                Session["UnitCategoryDS"] = dt;
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    //and CountryDesc = '" + txtStateDescription.Text + "' and VersionCode = '" + dsdelete.Tables[0].Rows[rowGvIndex]["Version"].ToString() + "'

                    string sql = "delete from Unit where UnitCode = '" + code.Trim() + "' ";
                    SqlDataReader reader = null;
                    sysFunc.ExecuteQuery(sql, ref reader);
                }
                ObjTrans.CommittTransaction(ref Trans);

                lblMessage.Text = "Record Deleted";
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;

            }
        }

        protected void gv_Unit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && !(gv_Unit.EditIndex == e.Row.RowIndex))
                {
                    e.Row.Cells[0].Enabled = true;
                }

                if (e.Row.RowType == DataControlRowType.DataRow && (gv_Unit.EditIndex == e.Row.RowIndex))
                {
                    TextBox txtUnitDesc = (TextBox)e.Row.FindControl("txtUnitDesc");

                    txtUnitDesc.Text = Session["lblUnitDesc"].ToString();
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {

                }
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }
        }

        //// DATABOUND FUNCTION
        protected void gv_Unit_DataBound(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }
        }
        // FOR GRIDVIEW ROW EDITING
        protected void gv_Unit_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                DataTable dtEditing = new DataTable();

                Label lblUnitDesc = (Label)gv_Unit.Rows[e.NewEditIndex].FindControl("lblUnitDesc");
                //Label lblStateCode = (Label)gv_city.Rows[e.NewEditIndex].FindControl("lblStateCode");

                Session["lblUnitDesc"] = lblUnitDesc.Text;
                //Session["lblStateCode"] = lblStateCode.Text;

                gv_Unit.EditIndex = e.NewEditIndex;

                dtEditing = (DataTable)Session["UnitCategoryDS"];
                //Session["UnitCategoryDS"] = dtEditing;
                gv_Unit.DataSource = dtEditing;

                gv_Unit.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;

            }

        }

        // ROW CANCEL EDIT
        protected void gv_Unit_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataTable dt = new DataTable();

            gv_Unit.EditIndex = -1;

            dt = (DataTable)Session["UnitCategoryDS"];

            gv_Unit.DataSource = dt;

            gv_Unit.DataBind();

        }

        //// FOR GRIDVIEW ROW UPDATING
        protected void gv_Unit_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataTable dt_update = new DataTable();

                Label lblUnitCode = (Label)gv_Unit.Rows[e.RowIndex].FindControl("lblUnitCode");
                TextBox txtUnitDesc = (TextBox)gv_Unit.Rows[e.RowIndex].FindControl("txtUnitDesc");     // GridView TextBox Field


                // Check value in GridView TextBOX
                if (txtUnitDesc.Text.Trim() == string.Empty)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Category Code can not null or empty.";
                    return;
                }


                dt_update = (DataTable)Session["UnitCategoryDS"];

                //Uzair
                //DataRow dr =  ds_update.Tables[0].Rows[0];
                // Check in GridView

                foreach (DataRow dr in dt_update.Rows)
                {
                    if (dr["UnitCode"].ToString() == lblUnitCode.Text)
                    {
                        dr["UnitDesc"] = txtUnitDesc.Text.Trim();
                    }
                }

                gv_Unit.EditIndex = -1;

                gv_Unit.DataSource = dt_update;
                gv_Unit.DataBind();

                Session["UnitCategoryDS"] = dt_update;

                string sql = "update Unit set UnitDesc = '" + txtUnitDesc.Text + "' WHERE UnitCode='" + lblUnitCode.Text + "'  ";

                if (sysFunc.ExecuteQuery_NonQuery(sql))
                {

                    sysFunc.UserMsg(lblMessage, Color.Green, "Sucessfully Update");

                }

            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }

        }
        protected void gv_Unit_SelectedIndexChanged(object sender, GridViewPageEventArgs e)
        {
            gv_Unit.PageIndex = e.NewPageIndex;
            createGrid();
        }


        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtunitdec.Text = "";
            lblMessage.Text = "";
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {


            SysFunction sysFunc = new SysFunction();
            Data.DSReports data = new Data.DSReports();
            ReportDocument RD = new ReportDocument();



            SqlParameter[] param =
            {
            new SqlParameter("@DealerCode",SqlDbType.Char),//0
           

        };

            param[0].Value = Session["DealerCode"].ToString();


            SqlDataReader rder = null;

            // Store Procedure check and load
            if (sysFunc.ExecuteSP("SP_Report_Unit", param, ref rder)) //Database SP
            {
                data.SP_Report_Unit.Load(rder);                       // DataSet SP

            }
            // Load in crystal report
            //RD.Load(Server.MapPath("../PurchaseSaleReturnReport.rpt"));
            RD.Load(Server.MapPath("/Reports/rptUnitReport.rpt"));
            RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAdress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            // RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'UNIT'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            // RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            // mention paper size
            //RD.PrintOptions.PaperSize = PaperSize.PaperA4;
            RD.Database.Tables[0].SetDataSource(data);
            string FilePath = Server.MapPath("~/Download/");
            string FileName = "CounterSalesDetail" + Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;

            Session["RD"] = RD;
            //RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            string URL;
            URL = "../../../Download/rptViewerService.aspx?FileName=" + FileName;
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
    }
}