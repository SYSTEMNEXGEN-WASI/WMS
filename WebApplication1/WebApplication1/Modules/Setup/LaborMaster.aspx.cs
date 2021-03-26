using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DXBMS.Modules.Setup
{
    public partial class LaborMaster : System.Web.UI.Page
    {
        SysFunction SysFunc = new SysFunction();
        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;
        SysFunctions myFunc = new SysFunctions();
        DataSet ds;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!IsPostBack)
            {
                //LoadDDL_JobCategory();
                LoadDDL_DefJobCode();
                //LoadDDL_AccountCode();
                //LoadDDL_BayCode();

                Create_Grid();
            }


            lblMsg.Visible = false;
        }
        protected void btnClear0_Click(object sender, EventArgs e)
        {
            cleadata();
            //LoadDDL_JobCategory();
            LoadDDL_DefJobCode();
            //LoadDDL_AccountCode();
            Create_Grid();
        }

        //private void LoadDDL_JobCategory()
        //{
        //    string WhereQuery = "1=1";

        //    string[] Columns = new string[] { "JobCatCode", "JobCatDesc" };
        //    SysFunc.GetMultiColumnsDDL(ddljob_category, Columns, "JobCategory", WhereQuery, "JobCatCode", "", false, false);
        //}

        //private void LoadDDL_AccountCode()
        //{
        //    string WhereQuery = "1=1";

        //    string[] Columns = new string[] { "AccountCode" };
        //    SysFunc.GetMultiColumnsDDL(ddlacount_code, Columns, "JobCategory", WhereQuery, "AccountCode", "", false, false);
        //}

        private void LoadDDL_DefJobCode()
        {
            string WhereQuery = "DealerCode in ( '"+Session["DealerCode"].ToString()+"','COMON')";

            string[] Columns = new string[] { "DefJobCode", "DefJobDesc" };
            SysFunc.GetMultiColumnsDDL(ddljob_code, Columns, "DefaultJob", WhereQuery, "DefJobCode", "", false, false);
        }

        private void Create_Grid()
        {
            ds = new DataSet();

            ds.Tables.Add();

            ds.Tables[0].Columns.Add(new DataColumn("DefJobCode", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Made", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Product", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Version", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("ServiceTax", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Amount", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("StandardTime", typeof(string)));

            DataRow dr = ds.Tables[0].NewRow();
            ds.Tables[0].Rows.Add(dr);

            gv_labor.DataSource = ds.Tables[0];
            gv_labor.DataBind();

            Session["LaborDS"] = ds;
        }

        private void cleadata()
        {
            SysFunc.Clearddl(Page);
            SysFunc.ClearTextBoxes(Page);
            //LoadDDL_JobCategory();
            LoadDDL_DefJobCode();
            //LoadDDL_AccountCode();
        }

        private void create_row()
        {
            DropDownList ddlbrand_code = (DropDownList)gv_labor.FooterRow.FindControl("ddlbrand_code");
            DropDownList ddlprod_code = (DropDownList)gv_labor.FooterRow.FindControl("ddlprod_code");
            DropDownList ddlVersion_code = (DropDownList)gv_labor.FooterRow.FindControl("ddlVersion_code");
            TextBox txtTaxAmt = (TextBox)gv_labor.FooterRow.FindControl("txtTaxAmt");
            TextBox txtAmount = (TextBox)gv_labor.FooterRow.FindControl("txtAmount");
            TextBox txtStnTime = (TextBox)gv_labor.FooterRow.FindControl("txtStnTime");
            Label JobCode = (Label)gv_labor.FindControl("lblJobCode");



            if (ddlbrand_code.SelectedIndex == 0)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Select Made";
                return;
            }
            if (ddlprod_code.SelectedIndex == 0)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Select Product ";
                return;
            }
            if (ddlVersion_code.SelectedIndex == 0)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Select Version ";
                return;
            }
            if (txtTaxAmt.Text == "" || txtTaxAmt.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Tax Amount cannot be empty or Zer(0)";
                return;
            }
            if (txtAmount.Text == "" || txtAmount.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Amount cannot be empty or Zer(0)";
                return;
            }
            if (txtStnTime.Text == "" || txtStnTime.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Time can not be null";
                return;
            }

            try
            {
                DataTable dt_newrow = new DataTable();
                SysFunctions SysFuncs = new SysFunctions();
                ds = new DataSet();

                ds = (DataSet)Session["LaborDS"];

                DataRow row = ds.Tables[0].NewRow();

                row["DefJobCode"] = ddlbrand_code.SelectedItem.Value;
                row["Made"] = ddlbrand_code.SelectedItem.Text;
                row["Product"] = ddlprod_code.SelectedItem.Value;
                row["Version"] = ddlVersion_code.SelectedItem.Value;
                row["ServiceTax"] = txtTaxAmt.Text;
                row["Amount"] = txtAmount.Text;
                row["StandardTime"] = txtStnTime.Text;

                ds.Tables[0].Rows.Add(row);

                gv_labor.DataSource = ds.Tables[0];
                gv_labor.DataBind();

                Session["LaborDS"] = ds;
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }
        }

        protected void BtnRemove_Labor_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsdelete = new DataSet();

            try
            {
                dsdelete = (DataSet)Session["LaborDS"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;



                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {

                    string sql = "delete from LaborDetail where DealerCode = '" + Session["DealerCode"].ToString() + "' and DefJobCode = '" + ddljob_code.SelectedValue + "' and ProdCode = '" + dsdelete.Tables[0].Rows[rowGvIndex]["Product"].ToString() + "' and VersionCode = '" + dsdelete.Tables[0].Rows[rowGvIndex]["Version"].ToString() + "'";
                    myFunc.ExecuteQuery(sql, Trans);
                }

                dsdelete.Tables[0].Rows[rowGvIndex].Delete();
                dsdelete.Tables[0].AcceptChanges();
                if (dsdelete.Tables[0].Rows.Count == 0)
                {
                    DataRow dr = dsdelete.Tables[0].NewRow();
                    dsdelete.Tables[0].Rows.Add(dr);
                }

                

                gv_labor.DataSource = dsdelete.Tables[0];
                gv_labor.DataBind();

                Session["LaborDS"] = dsdelete;
                
                ObjTrans.CommittTransaction(ref Trans);
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }
        }

        protected void BtnAdd_Labor_Click(object sender, ImageClickEventArgs e)
        {
            ds = new DataSet();

            DropDownList ddlbrand_code = (DropDownList)gv_labor.FooterRow.FindControl("ddlbrand_code");
            DropDownList ddlprod_code = (DropDownList)gv_labor.FooterRow.FindControl("ddlprod_code");
            DropDownList ddlVersion_code = (DropDownList)gv_labor.FooterRow.FindControl("ddlVersion_code");
            TextBox txtTaxAmt = (TextBox)gv_labor.FooterRow.FindControl("txtTaxAmt");
            TextBox txtAmount = (TextBox)gv_labor.FooterRow.FindControl("txtAmount");
            TextBox txtStnTime = (TextBox)gv_labor.FooterRow.FindControl("txtStnTime");

            if (ddlbrand_code.SelectedIndex == 0)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Select Made";
                return;
            }
            if (ddlprod_code.SelectedIndex == 0)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Select Product ";
                return;
            }
            if (ddlVersion_code.SelectedIndex == 0)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Select Version ";
                return;
            }
            if (txtTaxAmt.Text.Trim() == "" )
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Tax Amount cannot be empty or Zer(0)";
                return;
            }
            if (txtAmount.Text == "" || txtAmount.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Amount cannot be empty or Zer(0)";
                return;
            }
            if (txtStnTime.Text == "" || txtStnTime.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Time can not be null";
                return;
            }

            try
            {

                ds = (DataSet)Session["LaborDS"];

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (ddlbrand_code.SelectedValue == dr["Made"].ToString() && ddlprod_code.SelectedValue.Trim() == dr["Product"].ToString() && ddlVersion_code.SelectedValue == dr["Version"].ToString())
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Brand Code: " + ddlbrand_code.SelectedValue + " Already Selected";
                        return;
                    }

                }

                if (ds.Tables[0].Rows[0]["DefJobCode"].ToString() == string.Empty)
                {
                    ds.Tables[0].Rows.RemoveAt(0);
                    create_row();
                }
                else
                {
                    create_row();
                }
                Session["LaborDS"] = ds;
                //Uzair
                string strAutoCode = string.Empty;

                if (ddljob_code.SelectedIndex == 0)
                {
                    strAutoCode = SysFunc.GetNewMaxID("DefaultJob", "DefJobCode", 5, Session["DealerCode"].ToString());
                }
                else
                {
                    strAutoCode = ddljob_code.SelectedItem.Value;

                }
                SqlParameter[] param2 = {
                                            new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                            new SqlParameter("@DefJobCode",SqlDbType.VarChar),//1
                                            new SqlParameter("@ProdCode",SqlDbType.VarChar),//2
                                            new SqlParameter("@VersionCode",SqlDbType.Char),//3
                                            new SqlParameter("@Amount",SqlDbType.Float),//4
                                            new SqlParameter("@UpdUser",SqlDbType.Char),//5
                                            new SqlParameter("@UpdTerm",SqlDbType.Char),//6
                                            new SqlParameter("@ServiceTax",SqlDbType.Decimal),//7
                                            new SqlParameter("@BrandCode",SqlDbType.VarChar),//8
                                            new SqlParameter("@StnTime",SqlDbType.Char)//9
                                      };

                param2[0].Value = Session["DealerCode"].ToString();
                param2[1].Value = strAutoCode;
                param2[2].Value = ddlprod_code.SelectedValue;
                param2[3].Value = ddlVersion_code.SelectedValue;
                param2[4].Value = txtAmount.Text;
                param2[5].Value = Session["UserName"].ToString();
                param2[6].Value = GlobalVar.mUserIPAddress;
                param2[7].Value = txtTaxAmt.Text;
                param2[8].Value = ddlbrand_code.SelectedValue;
                param2[9].Value = txtStnTime.Text;


                SysFunc.ExecuteSP_NonQuery("Sp_Insert_LaborDetail", param2);

                lblMsg.Visible = true;
                lblMsg.Text = "Labor " + strAutoCode + " Added Successfully";

            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }
        }

        protected void gv_labor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                ImageButton btn_remove = (ImageButton)e.Row.FindControl("BtnRemove_Labor");
                Label JobCode = (Label)e.Row.FindControl("lblJobCode");

                if (e.Row.RowType == DataControlRowType.DataRow && !(gv_labor.EditIndex == e.Row.RowIndex) && JobCode.Text == string.Empty)
                {
                    btn_remove.Enabled = false;
                    e.Row.Cells[0].Enabled = false;


                }

                if (e.Row.RowType == DataControlRowType.DataRow && (gv_labor.EditIndex == e.Row.RowIndex))
                {
                    btn_remove.Enabled = false;

                    DropDownList ddlbrand_code = (DropDownList)e.Row.FindControl("ddlbrand_code");
                    TextBox txtTaxAmt = (TextBox)e.Row.FindControl("txtTaxAmt");
                    TextBox txtAmount = (TextBox)e.Row.FindControl("txtAmount");
                    TextBox txtStnTime = (TextBox)e.Row.FindControl("txtStnTime");

                    string WhereQuery = "DealerCode = '" + Session["DealerCode"] + "'";
                    string[] Columns = new string[] { "BrandCode", "BrandDesc" };
                    SysFunc.GetMultiColumnsDDL(ddlbrand_code, Columns, "Brand", WhereQuery, "BrandCode", "", false, false);
                    if (Session["lblMade"] != null)
                    {
                        string made = Session["lblMade"].ToString();
                        String [] subMade = made.Split('|');
                        string trim = subMade[1];
                        ddlbrand_code.SelectedValue = trim.Trim();
                        //ddlEditLoc.Items.FindByValue(Session["lblLocationCode"].ToString()).Selected = true;
                    }

                    DropDownList ddlprod_code = (DropDownList)e.Row.FindControl("ddlprod_code");
                    DropDownList ddlVersion_code = (DropDownList)e.Row.FindControl("ddlVersion_code");

                    string WhereQuery2 = "BrandCode = '" + ddlbrand_code.SelectedValue + "' AND DealerCode = '" + Session["DealerCode"].ToString() + "'";

                    string[] Columns2 = new string[] { "ProdCode" };
                    SysFunc.GetMultiColumnsDDL(ddlprod_code, Columns2, "Vehicle", WhereQuery2, "ProdCode", "", false, false);

                    if (Session["lblProduct"] != null)
                    {
                        ddlprod_code.SelectedValue = Session["lblProduct"].ToString();

                    }

                    string[] Columns3 = new string[] { "VersionCode" };
                    SysFunc.GetMultiColumnsDDL(ddlVersion_code, Columns3, "Vehicle", WhereQuery2, "VersionCode", "", false, false);
                    if (Session["lblVersion"] != null)
                    {
                        ddlVersion_code.SelectedValue = Session["lblVersion"].ToString();

                    }

                    txtAmount.Text = Session["lblAmount"].ToString();
                    txtTaxAmt.Text = Session["lblTaxAmt"].ToString();
                    txtStnTime.Text = Session["lblStnTime"].ToString();

                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DropDownList ddlbrand_code = (DropDownList)e.Row.FindControl("ddlbrand_code");
                    TextBox tax = (TextBox)e.Row.FindControl("txtTaxAmt");
                    DataTable dt = new DataTable();

                    string WhereQuery = "DealerCode = '" + Session["DealerCode"] + "'";
                    string[] Columns = new string[] { "BrandCode" };
                    SysFunc.GetMultiColumnsDDL(ddlbrand_code, Columns, "Vehicle", WhereQuery, "BrandCode", "", true, false);

                    string sql = "SELECT PST FROM Dealer where DealerCode = '" + Session["DealerCode"].ToString() + "'";
                    dt = SysFunc.GetData(sql);

                    tax.Text = dt.Rows[0]["PST"].ToString();

                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }
        }

        protected void gv_labor_DataBound(object sender, EventArgs e)
        {
            try
            {
                LoadDDL_gv();
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }
        }

        private void LoadDDL_gv()
        {
            DropDownList ddlbrand_code = (DropDownList)gv_labor.FooterRow.FindControl("ddlbrand_code");

            string WhereQuery = "DealerCode = '" + Session["DealerCode"] + "'";
            string[] Columns = new string[] { "BrandCode", "BrandDesc" };
            SysFunc.GetMultiColumnsDDL(ddlbrand_code, Columns, "Brand", WhereQuery, "BrandCode", "", false, false);


            DropDownList ddlprod_code = (DropDownList)gv_labor.FooterRow.FindControl("ddlprod_code");
            DropDownList ddlVersion_code = (DropDownList)gv_labor.FooterRow.FindControl("ddlVersion_code");

            string WhereQuery2 = "BrandCode = '" + ddlbrand_code.SelectedValue + "' AND DealerCode = '" + Session["DealerCode"].ToString() + "'";

            string[] Columns2 = new string[] { "ProdCode" };
            SysFunc.GetMultiColumnsDDL(ddlprod_code, Columns2, "Vehicle", WhereQuery2, "ProdCode", "", false, false);

            string WhereQuery3 = "ProdCode = '" + ddlprod_code.SelectedValue + "' AND DealerCode = '" + Session["DealerCode"].ToString() + "'";

            string[] Columns3 = new string[] { "VersionCode" };
            SysFunc.GetMultiColumnsDDL(ddlVersion_code, Columns3, "Vehicle", WhereQuery3, "VersionCode", "", false, false);
        }

        protected void gv_labor_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                DataSet ds = new DataSet();

                Label lblMade = (Label)gv_labor.Rows[e.NewEditIndex].FindControl("lblMade");
                Label lblProduct = (Label)gv_labor.Rows[e.NewEditIndex].FindControl("lblProduct");
                Label lblVersion = (Label)gv_labor.Rows[e.NewEditIndex].FindControl("lblVersion");
                Label lblTaxAmt = (Label)gv_labor.Rows[e.NewEditIndex].FindControl("lblTaxAmt");
                Label lblAmount = (Label)gv_labor.Rows[e.NewEditIndex].FindControl("lblAmount");
                Label lblStnTime = (Label)gv_labor.Rows[e.NewEditIndex].FindControl("lblStnTime");

                Session["lblMade"] = lblMade.Text;
                Session["lblProduct"] = lblProduct.Text;
                Session["lblVersion"] = lblVersion.Text;
                Session["lblTaxAmt"] = lblTaxAmt.Text;
                Session["lblAmount"] = lblAmount.Text;
                Session["lblStnTime"] = lblStnTime.Text;

                gv_labor.EditIndex = e.NewEditIndex;

                ds = (DataSet)Session["LaborDS"];

                gv_labor.DataSource = ds.Tables[0];

                gv_labor.DataBind();
            }

            catch (Exception ex)
            {

                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;

            }

        }

        protected void gv_labor_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataSet ds = new DataSet();

            gv_labor.EditIndex = -1;

            ds = (DataSet)Session["LaborDS"];

            gv_labor.DataSource = ds.Tables[0];

            gv_labor.DataBind();

        }

        protected void gv_labor_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataSet ds_update = new DataSet();
                TextBox txtStnTime = (TextBox)gv_labor.Rows[e.RowIndex].FindControl("txtStnTime");
                TextBox txtAmount = (TextBox)gv_labor.Rows[e.RowIndex].FindControl("txtAmount");
                TextBox txtTaxAmt = (TextBox)gv_labor.Rows[e.RowIndex].FindControl("txtTaxAmt");
                DropDownList ddlbrand_code = (DropDownList)gv_labor.Rows[e.RowIndex].FindControl("ddlbrand_code");
                DropDownList ddlprod_code = (DropDownList)gv_labor.Rows[e.RowIndex].FindControl("ddlprod_code");
                DropDownList ddlVersion_code = (DropDownList)gv_labor.Rows[e.RowIndex].FindControl("ddlVersion_code");
                Label JobCode = (Label)gv_labor.Rows[e.RowIndex].FindControl("lblJobCode");

                double amt = double.Parse(txtAmount.Text.Trim() == "" ? "0" : txtAmount.Text.Trim());
                double tax = double.Parse(txtTaxAmt.Text.Trim() == "" ? "0" : txtTaxAmt.Text.Trim());


                if (ddlbrand_code.SelectedIndex == 0)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Select (Brand Code)";
                    return;
                }

                if (ddlprod_code.SelectedIndex == 0)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Select (Product Code)";
                    return;
                }

                if (tax < 0 || txtTaxAmt.Text.Trim() == string.Empty)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Service can not Zero(0) or empty.";
                    return;
                }

                if (amt <= 0 || txtAmount.Text.Trim() == string.Empty)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Amount can not Zero(0) or empty.";
                    return;
                }
                if (txtStnTime.Text == "" || txtStnTime.Text == "0")
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Time can not be null";
                    return;
                }

                ds_update = (DataSet)Session["LaborDS"];

                //Uzair
                //DataRow dr =  ds_update.Tables[0].Rows[0];
                foreach (DataRow dr in ds_update.Tables[0].Rows)
                {
                    if (dr["DefJobCode"].ToString() == JobCode.Text.Trim() && dr["Product"].ToString() == Session["lblProduct"].ToString() && dr["Version"].ToString() == Session["lblVersion"].ToString())
                    {
                        dr["Made"] = ddlbrand_code.SelectedItem;
                        dr["Product"] = ddlprod_code.SelectedValue;
                        dr["Version"] = ddlVersion_code.SelectedValue;
                        dr["ServiceTax"] = tax;
                        dr["Amount"] = amt;
                        dr["StandardTime"] = txtStnTime.Text;
                    }
                }

                gv_labor.EditIndex = -1;

                gv_labor.DataSource = ds_update.Tables[0];
                gv_labor.DataBind();

                Session["LaborDS"] = ds_update;

                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {

                    string sql = "update LaborDetail set BrandCode = '" + ddlbrand_code.SelectedValue + "', ProdCode = '" + ddlprod_code.SelectedValue + "', VersionCode = '" + ddlVersion_code.SelectedValue + "'," +
                "ServiceTax = '" + tax + "', Amount = '" + amt + "', StandardTime = '" + txtStnTime.Text + "', UpdUser = '" + Session["UserName"].ToString() + "',UpdTerm = '" + GlobalVar.mUserIPAddress + "', UpdDate ='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where DealerCode = '" + Session["DealerCode"].ToString() + "'" +
                "and DefJobCode = '" + JobCode.Text + "' and ProdCode = '" + Session["lblProduct"].ToString() + "' and VersionCode = '" + Session["lblVersion"].ToString() + "'";
                    myFunc.ExecuteQuery(sql, Trans);
                }
                ObjTrans.CommittTransaction(ref Trans);
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnSave1_Click(object sender, EventArgs e)
        {
            string strAutoCode = string.Empty;
            string prodCode, VerCode = string.Empty;
            try
            {
                if (ddljob_code.SelectedIndex == 0)
                {
                    strAutoCode = SysFunc.GetNewMaxID("DefaultJob", "DefJobCode", 5, Session["DealerCode"].ToString());
                }
                else
                {
                    strAutoCode = ddljob_code.SelectedItem.Value;

                }
                SqlParameter[] param2 = {
                                            new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                            new SqlParameter("@DefJobCode",SqlDbType.VarChar),//1
                                            new SqlParameter("@ProdCode",SqlDbType.VarChar),//2
                                            new SqlParameter("@VersionCode",SqlDbType.Char),//3
                                            new SqlParameter("@Amount",SqlDbType.Float),//4
                                            new SqlParameter("@UpdUser",SqlDbType.Char),//5
                                            new SqlParameter("@UpdTerm",SqlDbType.Char),//6
                                            new SqlParameter("@ServiceTax",SqlDbType.Decimal),//7
                                            new SqlParameter("@BrandCode",SqlDbType.VarChar),//8
                                            new SqlParameter("@StnTime",SqlDbType.Char)//9
                                      };

                for (int i = 0; i < gv_labor.Rows.Count; i++)
                {
                    Label brandcode = (Label)gv_labor.Rows[i].FindControl("lblMade");
                    Label prodcode = (Label)gv_labor.Rows[i].FindControl("lblProduct");
                    Label versioncode = (Label)gv_labor.Rows[i].FindControl("lblVersion");
                    Label amount = (Label)gv_labor.Rows[i].FindControl("lblAmount");
                    Label servicetax = (Label)gv_labor.Rows[i].FindControl("lblTaxAmt");
                    Label Stntime = (Label)gv_labor.Rows[i].FindControl("lblStnTime");

                    prodCode = prodcode.Text;
                    VerCode = versioncode.Text;

                    decimal stnTime = Convert.ToDecimal(servicetax.Text == "" ? "0.0" : servicetax.Text);

                    param2[0].Value = Session["DealerCode"].ToString();
                    param2[1].Value = strAutoCode;
                    param2[2].Value = prodCode;
                    param2[3].Value = VerCode;
                    param2[4].Value = amount.Text;
                    param2[5].Value = Session["UserName"].ToString();
                    param2[6].Value = GlobalVar.mUserIPAddress;
                    param2[7].Value = stnTime;
                    param2[8].Value = brandcode.Text;
                    param2[9].Value = Stntime.Text;


                    SysFunc.ExecuteSP_NonQuery("Sp_Insert_LaborDetail", param2);
                }

                cleadata();

                lblMsg.Visible = true;
                lblMsg.Text = "Labor " + strAutoCode + " Added Successfully";

                Create_Grid();



            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnDelete0_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "DELETE FROM LaborDetail WHERE DealerCode = '" + Session["DealerCode"].ToString() + "' AND DefJobCode = '" + ddljob_code.SelectedValue + "'";

                SysFunc.ExecuteQuery_NonQuery(qry);

                lblMsg.Visible = true;
                lblMsg.Text = "Labor " + ddljob_code.SelectedValue + " Deleted Successfully";

                cleadata();
                Create_Grid();
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }
        }

        protected void ddljob_code_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddljob_code.SelectedIndex == 0)
            {
                cleadata();
                Create_Grid();
                return;
            }
            try
            {
                DataTable dt = new DataTable();
                Create_Grid();


                string sql = "Select d.DefJobDesc, d.JobCatCode,j.JobCatDesc,d.LaborAccount from DefaultJob d left outer join JobCategory j on d.JobCatCode = j.JobCatCode and "
                    + " d.DealerCode = j.DealerCode where d.DealerCode in( '" + Session["DealerCode"].ToString() + "','COMON') AND d.DefJobCode = '" + ddljob_code.SelectedItem.Value + "'";

                dt = SysFunc.GetData(sql);

                txtJobCatDesc.Text = dt.Rows[0]["JobCatDesc"].ToString();
                txtJobCategory.Text = dt.Rows[0]["JobCatCode"].ToString();
                txtjob_desc.Text = dt.Rows[0]["DefJobDesc"].ToString();
                txtLaborAC.Text = dt.Rows[0]["LaborAccount"].ToString();
               

                DataTable dt2 = new DataTable();

                string sql2 = "Select A.*,B.BrandDesc from LaborDetail A, Brand B WHERE A.DealerCode in( '" + Session["DealerCode"].ToString() + "','COMON') AND DefJobCode = '" + ddljob_code.SelectedItem.Value + "' and A.BrandCode = B.BrandCode and A.DealerCode = B.DealerCode";
                dt2 = SysFunc.GetData(sql2);

                DataSet ds = new DataSet();


                ds = (DataSet)Session["LaborDS"];

                if (dt2.Rows.Count > 0)
                {
                    ds.Tables[0].Rows.RemoveAt(0);

                    for (int j = 0; j < dt2.Rows.Count; j++)
                    {
                        DataRow row = ds.Tables[0].NewRow();

                        row["DefJobCode"] = dt2.Rows[j]["DefJobCode"].ToString();
                        row["Made"] = "| " + dt2.Rows[j]["BrandCode"].ToString() +" | "+dt2.Rows[j]["BrandDesc"].ToString();
                        row["Product"] = dt2.Rows[j]["ProdCode"].ToString();
                        row["Version"] = dt2.Rows[j]["VersionCode"].ToString();
                        row["Amount"] = dt2.Rows[j]["Amount"].ToString();
                        row["ServiceTax"] = dt2.Rows[j]["ServiceTax"].ToString();
                        row["StandardTime"] = dt2.Rows[j]["StandardTime"].ToString();

                        ds.Tables[0].Rows.Add(row);
                    }
                }
                else
                {
                    Create_Grid();
                    return;
                }

                gv_labor.DataSource = ds.Tables[0];
                gv_labor.DataBind();

                Session["LaborDS"] = ds;

            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }

        }

        protected void ddlbrand_code_SelectedIndexChanged(object sender, EventArgs e)
        {

            DropDownList ddlbrand_code = (DropDownList)gv_labor.FooterRow.FindControl("ddlbrand_code");

            DropDownList ddlprod_code = (DropDownList)gv_labor.FooterRow.FindControl("ddlprod_code");

            string WhereQuery2 = "BrandCode = '" + ddlbrand_code.SelectedValue + "' AND DealerCode = '" + Session["DealerCode"].ToString() + "'";

            string[] Columns2 = new string[] { "ProdCode" };
            SysFunc.GetMultiColumnsDDL(ddlprod_code, Columns2, "Vehicle", WhereQuery2, "ProdCode", "", false, true);

            //DropDownList ddlprod_code = (DropDownList)gv_labor.FooterRow.FindControl("ddlprod_code");
            
        }

        protected void ddlbrand_code_Edit(object sender, EventArgs e)
        {
            DropDownList ddlbrand_code = (DropDownList)gv_labor.Rows[0].FindControl("ddlbrand_code");
            DropDownList ddlprod_code = (DropDownList)gv_labor.Rows[0].FindControl("ddlprod_code");
            //DropDownList ddlVersion_code = (DropDownList)gv_labor.Rows[0].FindControl("ddlVersion_code");
            

            string WhereQuery2 = "BrandCode = '" + ddlbrand_code.SelectedValue + "' AND DealerCode = '" + Session["DealerCode"].ToString() + "'";

            string[] Columns2 = new string[] { "ProdCode" };
            SysFunc.GetMultiColumnsDDL(ddlprod_code, Columns2, "Vehicle", WhereQuery2, "ProdCode", "", false, true);

            
        }

        protected void ddlprod_code_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlprod_code = (DropDownList)gv_labor.Rows[0].FindControl("ddlprod_code");
            DropDownList ddlVersion_code = (DropDownList)gv_labor.Rows[0].FindControl("ddlVersion_code");

            string WhereQuery3 = "ProdCode = '" + ddlprod_code.SelectedValue + "' AND DealerCode = '" + Session["DealerCode"].ToString() + "'";

            string[] Columns3 = new string[] { "VersionCode" };
            SysFunc.GetMultiColumnsDDL(ddlVersion_code, Columns3, "Vehicle", WhereQuery3, "VersionCode", "", false, false);
        }

        protected void ddlprod_code_Footer(object sender, EventArgs e)
        {
            DropDownList ddlVersion_code = (DropDownList)gv_labor.FooterRow.FindControl("ddlVersion_code");
            DropDownList ddlprod_code = (DropDownList)gv_labor.FooterRow.FindControl("ddlprod_code");

            string WhereQuery3 = "ProdCode = '" + ddlprod_code.SelectedValue + "' AND DealerCode = '" + Session["DealerCode"].ToString() + "'";

            string[] Columns3 = new string[] { "VersionCode" };
            SysFunc.GetMultiColumnsDDL(ddlVersion_code, Columns3, "Vehicle", WhereQuery3, "VersionCode", "", false, false);

        }
    }
}