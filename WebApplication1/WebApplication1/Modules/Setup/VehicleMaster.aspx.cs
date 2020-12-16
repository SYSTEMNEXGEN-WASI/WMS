using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace DXBMS.Modules.Setup
{
    public partial class VehicleMaster : System.Web.UI.Page
    {

        SysFunction SysFunc = new SysFunction();
        DataSet ds;
        string fileName = string.Empty;
        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;
        SysFunctions myFunc = new SysFunctions();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (Page.IsPostBack == false)
            {
                ddlfuel_type.Items.Add(new ListItem("Petrol", "P"));
                ddlfuel_type.Items.Add(new ListItem("Diesel", "D"));
                ddlfuel_type.Items.Add(new ListItem("CNG", "C"));
                ddlfuel_type.Items.Add(new ListItem("LPG", "L"));

                Create_Grid();

                LoadDDL();

                ddltransmission_type.Items.Add(new ListItem("Auto", "Auto"));
                ddltransmission_type.Items.Add(new ListItem("Manual", "Manual"));

                ddlvehicle_status.Items.Add(new ListItem("New", "New"));
                ddlvehicle_status.Items.Add(new ListItem("Used", "Used"));


            }

            lblMsg.Visible = false;
            Page.Form.Attributes.Add("enctype", "multipart/form-data");

        }

        private void cleardata()
        {
            SysFunc.Clearddl(Page);
            SysFunc.ClearTextBoxes(Page);
            CheckBoxList1.ClearSelection();
            LoadDDL();
            Create_Grid();
        }

        private void LoadDDL()
        {
            

            //string WhereQuery = "1=1";

            //string[] Columns = new string[] { "VehicleCode", "BrandCode", "ProdCode" };
            //SysFunc.GetMultiColumnsDDL(ddlvehicle, Columns, "Vehicle", WhereQuery, "VehicleCode", "", false, false);

            LoadDDL_VehicleCode();
            LoadDDL_RegType();
            LoadDDL_VehicleType();
            LoadDDL_Brand();
        }

        private void Create_Grid()
        {
            ds = new DataSet();
            ds.Tables.Add();

            ds.Tables[0].Columns.Add(new DataColumn("VehicleCode", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Color", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("SalePrice", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("CostPrice", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("WHTax", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("FurTax", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("ExtraTax", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("RegType", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("RegTax", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("IsActive", typeof(string)));

            DataRow dr = ds.Tables[0].NewRow();
            ds.Tables[0].Rows.Add(dr);

            gv_vehicle.DataSource = ds.Tables[0];
            gv_vehicle.DataBind();

            Session["VehicleDS"] = ds;
        }

        private void LoadDDL_Brand()
        {
            string WhereQuery =  "DealerCode in( '" + Session["DealerCode"] + "','COMON')";

            string[] Columns = new string[] { "BrandCode", "BrandDesc" };
            SysFunc.GetMultiColumnsDDL(ddlmade, Columns, "Brand", WhereQuery, "BrandCode", "", false, false);
        }

        private void LoadDDL_VehicleCode()
        {
            string WhereQuery = "DealerCode in( '" + Session["DealerCode"] + "','COMON')";

            string[] Columns = new string[] { "VehicleCode", "BrandCode", "ProdCode" };
            SysFunc.GetMultiColumnsDDL(ddlvehicle, Columns, "Vehicle", WhereQuery, "VehicleCode", "order by VehicleCode desc", false, false);
        }

        private void LoadDDL_VehicleType()
        {
            string WhereQuery = "DealerCode in( '" + Session["DealerCode"] + "','COMON')";

            string[] Columns = new string[] { "VehTypeCode", "VehTypeDesc" };
            SysFunc.GetMultiColumnsDDL(ddlvehicle_type, Columns, "VehicleType", WhereQuery, "VehTypeCode", "", false, false);
        }

        protected void btnClear0_Click(object sender, EventArgs e)
        {
            cleardata();
        }

        protected void create_row()
        {
            DropDownList ddlcolorcode = (DropDownList)gv_vehicle.FooterRow.FindControl("ddlcolorcode");
            DropDownList ddlRegType = (DropDownList)gv_vehicle.FooterRow.FindControl("ddlRegType");
            TextBox txtSalePrice = (TextBox)gv_vehicle.FooterRow.FindControl("txtSalePrice");
            TextBox txtCostPrice = (TextBox)gv_vehicle.FooterRow.FindControl("txtCostPrice");
            TextBox txtWHTax = (TextBox)gv_vehicle.FooterRow.FindControl("txtWHTax");
            TextBox txtFurTax = (TextBox)gv_vehicle.FooterRow.FindControl("txtFurTax");
            TextBox txtExtraTax = (TextBox)gv_vehicle.FooterRow.FindControl("txtExtraTax");
            TextBox txtRegTax = (TextBox)gv_vehicle.FooterRow.FindControl("txtRegTax");
            CheckBox cbIsActive = (CheckBox)gv_vehicle.FooterRow.FindControl("cbIsActive");

            if (ddlcolorcode.SelectedIndex == 0)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Select Color";
                return;
            }
            if (txtSalePrice.Text == "" || txtSalePrice.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Sale Price cannot be empty or Zer(0)";
                return;
            }
            if (txtCostPrice.Text == "" || txtCostPrice.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Cost Price cannot be empty or Zer(0)";
                return;
            }
            if (txtWHTax.Text == "" || txtWHTax.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "WHTax cannot be empty or Zer(0)";
                return;
            }
            if (txtFurTax.Text == "" || txtFurTax.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Further Tax cannot be empty or Zer(0)";
                return;
            }
            if (txtExtraTax.Text == "" || txtExtraTax.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Extra Tax cannot be empty or Zer(0)";
                return;
            }
            if (txtRegTax.Text == "" || txtRegTax.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Register Tax cannot be empty or Zer(0)";
                return;
            }
            string IsActive = "N";
            if (cbIsActive.Checked == true)
            {
                IsActive = "Y";
            }

            try
            {

                DataTable dt_newrow = new DataTable();

                ds = new DataSet();

                ds = (DataSet)Session["VehicleDS"];

                DataRow row = ds.Tables[0].NewRow();

                row["VehicleCode"] = ddlcolorcode.SelectedValue;
                row["Color"] = ddlcolorcode.SelectedValue;
                row["SalePrice"] = txtSalePrice.Text;
                row["CostPrice"] = txtCostPrice.Text;
                row["WHTax"] = txtWHTax.Text;
                row["FurTax"] = txtWHTax.Text;
                row["ExtraTax"] = txtWHTax.Text;
                row["RegType"] = ddlRegType.SelectedValue;
                row["RegTax"] = txtRegTax.Text;
                row["IsActive"] = IsActive;

                ds.Tables[0].Rows.Add(row);

                gv_vehicle.DataSource = ds.Tables[0];
                gv_vehicle.DataBind();

                Session["VehicleDS"] = ds;

            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;
            }
        }

        protected void BtnRemove_Vehicle_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsdelete_tax = new DataSet();

            try
            {
                dsdelete_tax = (DataSet)Session["VehicleDS"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;

                dsdelete_tax.Tables[0].Rows[rowGvIndex].Delete();
                dsdelete_tax.Tables[0].AcceptChanges();

                if (dsdelete_tax.Tables[0].Rows.Count == 0)
                {
                    DataRow dr = dsdelete_tax.Tables[0].NewRow();
                    dsdelete_tax.Tables[0].Rows.Add(dr);
                }

                gv_vehicle.DataSource = dsdelete_tax.Tables[0];
                gv_vehicle.DataBind();

                Session["VehicleDS"] = dsdelete_tax;
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnSave1_Click(object sender, EventArgs e)
        {
            string strAutoCode = string.Empty;
            byte[] bytes = new byte[0];
            string isActive = string.Empty;

            if (CheckBoxList1.SelectedValue == "Active")
            {
                isActive = "Y";
            }
            if (CheckBoxList1.SelectedValue == "DummyVehicle")
            {
                isActive = "N";
            }
            if (Page.IsValid)
            {

                if (myFunc.IsExist("BrandCode", ddlmade.SelectedValue, "Vehicle", Session["DealerCode"].ToString(), "and ProdCode = '" + txtvehicle_code.Text + "'and VersionCode = '" + txtmodel_code.Text + "'") && ddlvehicle.SelectedIndex == 0)
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "Vehicle is already Exists";

                }
                else
                {
                    try
                    {

                        if (ddlvehicle.SelectedIndex == 0)
                        {
                            strAutoCode = SysFunc.GetNewMaxID("Vehicle", "VehicleCode", 8, Session["DealerCode"].ToString());
                        }
                        else
                        {
                            strAutoCode = ddlvehicle.SelectedItem.Value;
                        }

                        if (PicUpload.HasFile)
                        {
                            string extension = Path.GetExtension(PicUpload.FileName);
                            if (extension == ".jpg" || extension == ".bmp")
                            {
                                string filepath = Server.MapPath("~/VehicleImages/") + Guid.NewGuid() + PicUpload.PostedFile.FileName;
                                PicUpload.PostedFile.SaveAs(filepath);
                                FileStream fS = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                                BinaryReader br = new BinaryReader(fS);
                                bytes = br.ReadBytes(Convert.ToInt32(fS.Length));
                                fS.Close();
                                br.Close();
                                fS.Close();

                            }
                            else
                            {
                                lblMsg.Visible = true;
                                lblMsg.ForeColor = System.Drawing.Color.Red;
                                lblMsg.Text = "only JPG and BMP Images allowed";
                            }
                        }

                        SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                   new SqlParameter("@BrandCode",SqlDbType.Char),//1
                                   new SqlParameter("@ProdCode",SqlDbType.Char),//2
                                   new SqlParameter("@VersionCode",SqlDbType.Char),//3
                                   new SqlParameter("@Color",SqlDbType.VarChar),//4
                                   new SqlParameter("@ColorCode",SqlDbType.Char),//5
                                   new SqlParameter("@ProdDesc",SqlDbType.VarChar),//6
                                   new SqlParameter("@ProdLongDesc",SqlDbType.VarChar),//7
                                   new SqlParameter("@Power",SqlDbType.VarChar),//8
                                   new SqlParameter("@CNGFlag",SqlDbType.Char),//9
                                   new SqlParameter("@CommPerc",SqlDbType.Float),//10
                                   new SqlParameter("@CommAmount",SqlDbType.Float),//11
                                   new SqlParameter("@UpdUser",SqlDbType.VarChar),//12
                                   new SqlParameter("@UpdTerm",SqlDbType.VarChar),//13
                                   new SqlParameter("@Version",SqlDbType.Char),//14
                                   new SqlParameter("@FFS",SqlDbType.Float),//15
                                   new SqlParameter("@PDI",SqlDbType.Float),//16
                                   new SqlParameter("@VehicleStatus",SqlDbType.VarChar),//17
                                   new SqlParameter("@CostPrice",SqlDbType.Float),//18
                                   new SqlParameter("@ProdTitle",SqlDbType.VarChar),//19
                                   new SqlParameter("@ReportingProdCode",SqlDbType.VarChar),//20
                                   new SqlParameter("@IsActive",SqlDbType.Char),//21
                                   new SqlParameter("@SalesAccountCode",SqlDbType.VarChar),//22
                                   new SqlParameter("@EffectiveStartDate",SqlDbType.DateTime),//23
                                   new SqlParameter("@EffectiveEndDate",SqlDbType.DateTime),//24
                                   new SqlParameter("@EnginePrefix",SqlDbType.VarChar),//25
                                   new SqlParameter("@SFS",SqlDbType.Int),//26
                                   new SqlParameter("@TFS",SqlDbType.Int),//27
                                   new SqlParameter("@VehTypeCode",SqlDbType.Char),//28
                                   new SqlParameter("@Seater",SqlDbType.TinyInt),//29
                                   new SqlParameter("@Compartment",SqlDbType.TinyInt),//30
                                   new SqlParameter("@VehiclePrefix",SqlDbType.VarChar),//31
                                   new SqlParameter("@Weight",SqlDbType.Int),//32
                                   new SqlParameter("@SchedulePlan",SqlDbType.TinyInt),//33
                                   new SqlParameter("@FreeServiceCup",SqlDbType.TinyInt),//34
                                   new SqlParameter("@Remarks",SqlDbType.VarChar),//35
                                   new SqlParameter("@Pic",SqlDbType.VarBinary),//36
                                   new SqlParameter("@TransmissionType",SqlDbType.Char),//37
                                   new SqlParameter("@VehicleCode",SqlDbType.Char),//38 
                                   new SqlParameter("@VehicleCategory",SqlDbType.Char),//39
                                    new SqlParameter("@BodyType",SqlDbType.Char),//39
                                     new SqlParameter("@PayLoad",SqlDbType.Char),//39
                                      new SqlParameter("@BuiltUnit",SqlDbType.Char),//39

                               };

                        param[0].Value = Session["DealerCode"].ToString();
                        param[1].Value = ddlmade.SelectedItem.Value;
                        param[2].Value = txtvehicle_code.Text.ToUpper().Trim();
                        param[3].Value = txtmodel_code.Text;
                        param[4].Value = "Multi Color";
                        param[5].Value = "00005";
                        param[6].Value = txtshort_desc.Text.ToUpper().Trim();
                        param[7].Value = txtvehicle_desc.Text.ToUpper().Trim();
                        param[8].Value = txtengine_cap.Text + "CC";
                        param[9].Value = ddlfuel_type.SelectedItem.Value;
                        param[10].Value = Convert.ToDouble("0");
                        param[11].Value = Convert.ToDouble("0");
                        param[12].Value = Session["UserName"].ToString();
                        param[13].Value = GlobalVar.mUserIPAddress;
                        param[14].Value = (object)DBNull.Value;
                        param[15].Value = (object)DBNull.Value;
                        param[16].Value = Convert.ToDouble("0");
                        param[17].Value = ddlvehicle_status.SelectedValue;
                        param[18].Value = Convert.ToDouble("0");
                        param[19].Value = (object)DBNull.Value;
                        param[20].Value = (object)DBNull.Value;
                        param[21].Value = isActive;
                        param[22].Value = (object)DBNull.Value;
                        param[23].Value = (object)DBNull.Value;
                        param[24].Value = (object)DBNull.Value;
                        param[25].Value = (object)DBNull.Value;
                        param[26].Value = (object)DBNull.Value;
                        param[27].Value = (object)DBNull.Value;
                        param[28].Value = ddlvehicle_type.SelectedItem.Value;
                        param[29].Value = Convert.ToByte(txtseater.Text.Trim() == "" ? "0" : txtseater.Text.Trim());
                        param[30].Value = Convert.ToByte(txtNo_compartments.Text.Trim() == "" ? "0" : txtNo_compartments.Text.Trim());
                        param[31].Value = txtvehicle_prefix.Text;
                        param[32].Value = Convert.ToInt32(txtWeight.Text.Trim() == "" ? "0" : txtWeight.Text.Trim());
                        param[33].Value = Convert.ToByte(txtsch_plan.Text.Trim() == "" ? "0" : txtsch_plan.Text.Trim());
                        param[34].Value = Convert.ToByte(txtfree_service.Text.Trim() == "" ? "0" : txtfree_service.Text.Trim());
                        param[35].Value = txtremarks.Text;
                        param[36].Value = bytes;
                        param[37].Value = ddltransmission_type.SelectedValue;
                        param[38].Value = strAutoCode;
                        param[39].Value = txtVehCategory.Text;
                        param[40].Value = txtBodyType.Text;
                        param[41].Value = txtPayLoad.Text;
                        param[42].Value = ddlBuiltUnit.Text;


                        SysFunc.ExecuteSP_NonQuery("SP_Insert_Vehicle", param);
                        //{
                        //    SqlParameter[] param2 = {
                        //                        new SqlParameter("@DealerCode",SqlDbType.Char),//0
                        //                        new SqlParameter("@BrandCode",SqlDbType.Char),//1
                        //                        new SqlParameter("@ProdCode",SqlDbType.VarChar),//2
                        //                        new SqlParameter("@VersionCode",SqlDbType.Char),//3
                        //                        new SqlParameter("@ColorCode",SqlDbType.Char),//4
                        //                        new SqlParameter("@SalePrice",SqlDbType.Float),//5
                        //                        new SqlParameter("@Version",SqlDbType.Char),//6
                        //                        new SqlParameter("@VehicleStatus",SqlDbType.VarChar),//7
                        //                        new SqlParameter("@CostPrice",SqlDbType.Float),//8
                        //                        new SqlParameter("@WHTaxPerc",SqlDbType.Float),//9
                        //                        new SqlParameter("@WHTaxAmount",SqlDbType.Float),//10
                        //                        new SqlParameter("@FurTaxPer",SqlDbType.Float),//11
                        //                        new SqlParameter("@ExtraTaxPerc",SqlDbType.Float),//12
                        //                        new SqlParameter("@RegTaxPerc",SqlDbType.Float),//13
                        //                        new SqlParameter("@FederalTaxPerc",SqlDbType.Float),//14
                        //                        new SqlParameter("@FederalTaxAmount",SqlDbType.Float),//15
                        //                        new SqlParameter("@SEDTaxPerc",SqlDbType.Float),//16
                        //                        new SqlParameter("@SEDTaxAmount",SqlDbType.Float),//17
                        //                        new SqlParameter("@UpdUser",SqlDbType.VarChar),//18
                        //                        new SqlParameter("@UpdTerm",SqlDbType.VarChar),//19
                        //                        new SqlParameter("@IsActive",SqlDbType.Char),//20
                        //                        new SqlParameter("@VehicleCode",SqlDbType.Char),//21
                        //                        new SqlParameter("@RegType",SqlDbType.Char),//22
                        //                        };


                        //    for (int i = 0; i < gv_vehicle.Rows.Count; i++)
                        //    {
                        //        double WHTaxAmount = 0;

                        //        Label ColorCode = (Label)gv_vehicle.Rows[i].FindControl("lblColor");
                        //        Label SalePrice = (Label)gv_vehicle.Rows[i].FindControl("lblSalePrice");
                        //        Label CostPrice = (Label)gv_vehicle.Rows[i].FindControl("lblCostPrice");
                        //        Label WHTax = (Label)gv_vehicle.Rows[i].FindControl("lblWHTax");
                        //        Label FurTax = (Label)gv_vehicle.Rows[i].FindControl("lblFurTax");
                        //        Label ExtraTax = (Label)gv_vehicle.Rows[i].FindControl("lblExtraTax");
                        //        Label RegType = (Label)gv_vehicle.Rows[i].FindControl("lblRegType");
                        //        Label RegTax = (Label)gv_vehicle.Rows[i].FindControl("lblRegTax");
                        //        Label lblIsActive = (Label)gv_vehicle.Rows[i].FindControl("lblIsActive");

                        //        WHTaxAmount = (double.Parse(SalePrice.Text) * double.Parse(WHTax.Text));

                        //        param2[0].Value = Session["DealerCode"].ToString();
                        //        param2[1].Value = ddlmade.SelectedValue;
                        //        param2[2].Value = txtvehicle_code.Text;
                        //        param2[3].Value = txtmodel_code.Text;
                        //        param2[4].Value = ColorCode.Text;
                        //        param2[5].Value = SalePrice.Text.Trim();
                        //        param2[6].Value = (object)DBNull.Value;
                        //        param2[7].Value = ddlvehicle_status.SelectedValue;
                        //        param2[8].Value = CostPrice.Text.Trim();
                        //        param2[9].Value = WHTax.Text.Trim();
                        //        param2[10].Value = WHTaxAmount;
                        //        param2[11].Value = FurTax.Text.Trim();
                        //        param2[12].Value = ExtraTax.Text.Trim();
                        //        param2[13].Value = RegTax.Text.Trim();
                        //        param2[14].Value = (object)DBNull.Value;
                        //        param2[15].Value = (object)DBNull.Value;
                        //        param2[16].Value = (object)DBNull.Value;
                        //        param2[17].Value = (object)DBNull.Value;
                        //        param2[18].Value = Session["UserName"].ToString();
                        //        param2[19].Value = Session["Terminal"].ToString();
                        //        param2[20].Value = lblIsActive.Text;
                        //        param2[21].Value = strAutoCode;
                        //        param2[22].Value = RegType.Text;

                        //        SysFunc.ExecuteSP_NonQuery("Sp_Insert_VehicleTaxDetail", param2);
                        //    }

                        //}
                        //else
                        //{
                        //    lblMsg.Visible = true;
                        //    lblMsg.Text = "Vehicle Unsuccessfull";
                        //    return;
                        //}


                        if(ddlvehicle.SelectedIndex == 0)
                        {
                            lblMsg.Visible = true;
                            lblMsg.ForeColor = System.Drawing.Color.Green;
                            lblMsg.Text = "Vehicle " + strAutoCode + " Added Successfully";
                        }else
                        {
                            lblMsg.Visible = true;
                            lblMsg.ForeColor = System.Drawing.Color.Green;
                            lblMsg.Text = "Vehicle " + strAutoCode + " Updated Successfully";
                        }
                        

                        cleardata();
                        Create_Grid();


                    }
                    catch (Exception ex)
                    {
                        lblMsg.Visible = true;
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        lblMsg.Text = ex.Message;
                    }
                }                
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Validation Failed! Data not saved";
            }
        }

        protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (CheckBoxList1.SelectedIndex < 0)
            {
                ListItem chk1 = CheckBoxList1.Items.FindByValue("Active");
                ListItem chk2 = CheckBoxList1.Items.FindByValue("DummyVehicle");
                chk1.Enabled = true;
                chk2.Enabled = true;
            }
            if (CheckBoxList1.SelectedValue == "Active")
            {
                ListItem chk2 = CheckBoxList1.Items.FindByValue("DummyVehicle");
                chk2.Enabled = false;
            }
            if (CheckBoxList1.SelectedValue == "DummyVehicle")
            {
                ListItem chk2 = CheckBoxList1.Items.FindByValue("Active");
                chk2.Enabled = false;
            }

        }

        protected void gv_vehicle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Label lblVehicleCode = (Label)e.Row.FindControl("lblVehicleCode");
                ImageButton btn_remove = (ImageButton)e.Row.FindControl("BtnRemove_Vehicle");

                if (e.Row.RowType == DataControlRowType.DataRow && !(gv_vehicle.EditIndex == e.Row.RowIndex) && lblVehicleCode.Text == string.Empty)
                {
                    btn_remove.Enabled = false;
                    e.Row.Cells[0].Enabled = false;
                }

                if (e.Row.RowType == DataControlRowType.DataRow && (gv_vehicle.EditIndex == e.Row.RowIndex))
                {
                    btn_remove.Enabled = false;

                    DropDownList ddlcolorcode = (DropDownList)e.Row.FindControl("ddlcolorcode");
                    TextBox txtSalePrice = (TextBox)e.Row.FindControl("txtSalePrice");
                    TextBox txtCostPrice = (TextBox)e.Row.FindControl("txtCostPrice");
                    TextBox txtWHTax = (TextBox)e.Row.FindControl("txtWHTax");
                    TextBox txtFurTax = (TextBox)e.Row.FindControl("txtFurTax");
                    TextBox txtExtraTax = (TextBox)e.Row.FindControl("txtExtraTax");
                    TextBox txtRegTax = (TextBox)e.Row.FindControl("txtRegTax");
                    CheckBox cbIsActive = (CheckBox)e.Row.FindControl("cbIsActive");

                    string WhereQuery = "1=1";
                    string[] Columns = new string[] { "ColorCode" };
                    SysFunc.GetMultiColumnsDDL(ddlcolorcode, Columns, "Color", WhereQuery, "ColorCode", "", false, false);
                    if (Session["lblColor"] != null)
                    {
                        ddlcolorcode.SelectedValue = Session["lblColor"].ToString();
                    }
                    // if (!IsPostBack)
                    // {
                    DropDownList ddlRegType = (DropDownList)e.Row.FindControl("ddlRegType");

                    ddlRegType.Items.Add(new ListItem("Filer", "Filer"));
                    ddlRegType.Items.Add(new ListItem("Non-Filer", "Non-Filer"));
                    ddlRegType.Items.Add(new ListItem("Exempted", "Exempted"));

                    txtSalePrice.Text = Session["lblSaleprice"].ToString();
                    txtCostPrice.Text = Session["lblCostPrice"].ToString();
                    txtWHTax.Text = Session["lblWHTax"].ToString();
                    txtFurTax.Text = Session["lblFurTax"].ToString();
                    txtExtraTax.Text = Session["lblExtraTax"].ToString();
                    txtRegTax.Text = Session["lblRegTax"].ToString();
                    ddlRegType.SelectedValue = Session["lblRegType"].ToString();
                    if (Session["lblIsActive"].ToString().Trim() == "Y")
                    {
                        cbIsActive.Checked = true;
                    }


                    // }
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DropDownList ddlcolorcode = (DropDownList)e.Row.FindControl("ddlcolorcode");

                    string WhereQuery = "DealerCode in ('" + Session["DealerCode"] + "','COMON')";
                    string[] Columns = new string[] { "ColorCode", "ColorDesc" };
                    SysFunc.GetMultiColumnsDDL(ddlcolorcode, Columns, "Color", WhereQuery, "ColorCode", "", false, false);

                    DropDownList ddlRegType = (DropDownList)e.Row.FindControl("ddlRegType");

                    ddlRegType.Items.Add(new ListItem("Filer", "Filer"));
                    ddlRegType.Items.Add(new ListItem("Non-Filer", "Non-Filer"));
                    ddlRegType.Items.Add(new ListItem("Exempted", "Exempted"));
                }



            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;
            }

        }

        protected void gv_vehicle_DataBound(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlcolorcode = (DropDownList)gv_vehicle.FooterRow.FindControl("ddlcolorcode");

                string WhereQuery = "DealerCode in ('" + Session["DealerCode"] + "','AAAAA')";
                string[] Columns = new string[] { "ColorCode", "ColorDesc" };
                SysFunc.GetMultiColumnsDDL(ddlcolorcode, Columns, "Color", WhereQuery, "ColorCode", "", false, false);


            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;
            }
        }

        private void LoadDDL_RegType()
        {
            DropDownList ddlRegType = (DropDownList)gv_vehicle.FooterRow.FindControl("ddlRegType");
            ddlRegType.Items.Clear();
            ddlRegType.Items.Add(new ListItem("Filer", "Filer"));
            ddlRegType.Items.Add(new ListItem("Non-Filer", "Non-Filer"));
            ddlRegType.Items.Add(new ListItem("Exempted", "Exempted"));
        }

        protected void gv_vehicle_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                DataSet ds = new DataSet();

                Label lblColor = (Label)gv_vehicle.Rows[e.NewEditIndex].FindControl("lblColor");
                Label lblSalePrice = (Label)gv_vehicle.Rows[e.NewEditIndex].FindControl("lblSalePrice");
                Label lblCostPrice = (Label)gv_vehicle.Rows[e.NewEditIndex].FindControl("lblCostPrice");
                Label lblWHTax = (Label)gv_vehicle.Rows[e.NewEditIndex].FindControl("lblWHTax");
                Label lblFurTax = (Label)gv_vehicle.Rows[e.NewEditIndex].FindControl("lblFurTax");
                Label lblExtraTax = (Label)gv_vehicle.Rows[e.NewEditIndex].FindControl("lblExtraTax");
                Label lblRegType = (Label)gv_vehicle.Rows[e.NewEditIndex].FindControl("lblRegType");
                Label lblRegTax = (Label)gv_vehicle.Rows[e.NewEditIndex].FindControl("lblRegTax");
                Label lblIsActive = (Label)gv_vehicle.Rows[e.NewEditIndex].FindControl("lblIsActive");


                Session["lblColor"] = lblColor.Text;
                Session["lblSalePrice"] = lblSalePrice.Text;
                Session["lblCostPrice"] = lblCostPrice.Text;
                Session["lblWHTax"] = lblWHTax.Text;
                Session["lblFurTax"] = lblFurTax.Text;
                Session["lblExtraTax"] = lblExtraTax.Text;
                Session["lblRegType"] = lblRegType.Text;
                Session["lblRegTax"] = lblRegTax.Text;
                Session["lblIsActive"] = lblIsActive.Text;


                gv_vehicle.EditIndex = e.NewEditIndex;

                ds = (DataSet)Session["VehicleDS"];


                gv_vehicle.DataSource = ds.Tables[0];

                gv_vehicle.DataBind();
                Session["VehicleDS"] = ds;
            }

            catch (Exception ex)
            {

                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;

            }
        }

        protected void gv_vehicle_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            DataSet ds = new DataSet();

            gv_vehicle.EditIndex = -1;

            ds = (DataSet)Session["VehicleDS"];

            gv_vehicle.DataSource = ds.Tables[0];

            gv_vehicle.DataBind();
        }

        protected void gv_vehicle_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            DataSet ds_update = new DataSet();

            DropDownList ddlcolorcode = (DropDownList)gv_vehicle.Rows[e.RowIndex].FindControl("ddlcolorcode");
            DropDownList ddlRegType = (DropDownList)gv_vehicle.Rows[e.RowIndex].FindControl("ddlRegType");
            TextBox txtSalePrice = (TextBox)gv_vehicle.Rows[e.RowIndex].FindControl("txtSalePrice");
            TextBox txtCostPrice = (TextBox)gv_vehicle.Rows[e.RowIndex].FindControl("txtCostPrice");
            TextBox txtWHTax = (TextBox)gv_vehicle.Rows[e.RowIndex].FindControl("txtWHTax");
            TextBox txtFurTax = (TextBox)gv_vehicle.Rows[e.RowIndex].FindControl("txtFurTax");
            TextBox txtExtraTax = (TextBox)gv_vehicle.Rows[e.RowIndex].FindControl("txtExtraTax");
            TextBox txtRegTax = (TextBox)gv_vehicle.Rows[e.RowIndex].FindControl("txtRegTax");
            CheckBox cbIsActive = (CheckBox)gv_vehicle.Rows[e.RowIndex].FindControl("cbIsActive");

            if (ddlcolorcode.SelectedIndex == 0)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Select Color";
                return;
            }
            if (txtSalePrice.Text == "" || txtSalePrice.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Sale Price cannot be empty or Zer(0)";
                return;
            }
            if (txtCostPrice.Text == "" || txtCostPrice.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Cost Price cannot be empty or Zer(0)";
                return;
            }
            if (txtWHTax.Text == "" || txtWHTax.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "WHTax cannot be empty or Zer(0)";
                return;
            }
            if (txtFurTax.Text == "" || txtFurTax.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Further Tax cannot be empty or Zer(0)";
                return;
            }
            if (txtExtraTax.Text == "" || txtExtraTax.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Extra Tax cannot be empty or Zer(0)";
                return;
            }
            if (txtRegTax.Text == "" || txtRegTax.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Register Tax cannot be empty or Zer(0)";
                return;
            }
            string IsActive = "N";
            if (cbIsActive.Checked == true)
            {
                IsActive = "Y";
            }

            try
            {

                ds_update = (DataSet)Session["VehicleDS"];

                foreach (DataRow dr in ds_update.Tables[0].Rows)
                {

                    if (dr["Color"].ToString().Trim() == ddlcolorcode.SelectedValue)
                    {
                        dr["Color"] = ddlcolorcode.SelectedValue;
                        dr["SalePrice"] = txtSalePrice.Text.Trim();
                        dr["CostPrice"] = txtCostPrice.Text.Trim();
                        dr["WHTax"] = txtWHTax.Text.Trim();
                        dr["FurTax"] = txtFurTax.Text.Trim();
                        dr["ExtraTax"] = txtExtraTax.Text.Trim();
                        dr["RegType"] = ddlRegType.SelectedValue;
                        dr["RegTax"] = txtRegTax.Text.Trim();
                        if (cbIsActive.Checked == true)
                        {
                            dr["IsActive"] = "Y";
                        }
                        else
                        {
                            dr["IsActive"] = "N";
                        }

                    }


                }

                gv_vehicle.EditIndex = -1;

                gv_vehicle.DataSource = ds_update.Tables[0];
                gv_vehicle.DataBind();

                Session["VehicleDS"] = ds_update;

                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {

                    string sql = "update VehicleTaxDetail set ColorCode = '" + ddlcolorcode.SelectedValue + "', SalePrice = '" + txtSalePrice.Text.Trim() + "'" +
                        ",CostPrice = '" + txtCostPrice.Text.Trim() + "',WHTaxPerc = '" + txtWHTax.Text.Trim() + "',FurTaxPerc = '" + txtFurTax.Text.Trim() + "'" +
                        ",ExtraTaxPerc = '" + txtExtraTax.Text.Trim() + "',RegType = '" + ddlRegType.SelectedValue + "',RegTaxPerc = '" + txtRegTax.Text.Trim() + "'" +
                        "where ColorCode = '" + ddlcolorcode.SelectedValue + "' and VersionCode = '" + txtmodel_code.Text.Trim() + "' and ProdCode = '" + txtvehicle_code.Text.Trim() + "'" +
                        "and BrandCode = '" + ddlmade.SelectedValue + "' and DealerCode = '"+Session["DealerCode"].ToString()+"'";

                    myFunc.ExecuteQuery(sql, Trans);

                }

                ObjTrans.CommittTransaction(ref Trans);
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;
            }
        }

        protected void BtnAdd_Vehicle_Click(object sender, ImageClickEventArgs e)
        {
            ds = new DataSet();

            DropDownList ddlcolorcode = (DropDownList)gv_vehicle.FooterRow.FindControl("ddlcolorcode");
            DropDownList ddlRegType = (DropDownList)gv_vehicle.FooterRow.FindControl("ddlRegType");
            TextBox txtSalePrice = (TextBox)gv_vehicle.FooterRow.FindControl("txtSalePrice");
            TextBox txtCostPrice = (TextBox)gv_vehicle.FooterRow.FindControl("txtCostPrice");
            TextBox txtWHTax = (TextBox)gv_vehicle.FooterRow.FindControl("txtWHTax");
            TextBox txtFurTax = (TextBox)gv_vehicle.FooterRow.FindControl("txtFurTax");
            TextBox txtExtraTax = (TextBox)gv_vehicle.FooterRow.FindControl("txtExtraTax");
            TextBox txtRegTax = (TextBox)gv_vehicle.FooterRow.FindControl("txtRegTax");
            CheckBox cbIsActive = (CheckBox)gv_vehicle.FooterRow.FindControl("cbIsActive");

            if (ddlcolorcode.SelectedIndex == 0)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Select Color";
                return;
            }
            if (txtSalePrice.Text == "" || txtSalePrice.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Sale Price cannot be empty or Zer(0)";
                return;
            }
            if (txtCostPrice.Text == "" || txtCostPrice.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Cost Price cannot be empty or Zer(0)";
                return;
            }
            if (txtWHTax.Text == "" || txtWHTax.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "WHTax cannot be empty or Zer(0)";
                return;
            }
            if (txtFurTax.Text == "" || txtFurTax.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Further Tax cannot be empty or Zer(0)";
                return;
            }
            if (txtExtraTax.Text == "" || txtExtraTax.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Extra Tax cannot be empty or Zer(0)";
                return;
            }
            if (txtRegTax.Text == "" || txtRegTax.Text == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Register Tax cannot be empty or Zer(0)";
                return;
            }

            try
            {
                DataTable dt = new DataTable();

                //string sql = "Select * from VehicleTaxDetail where DealerCode = '" + Session["DealerCode"].ToString() + "' AND VehicleCode = '" + ddlvehicle.SelectedItem.Value + "'";
                //dt = SysFunc.GetData(sql);


                ds = (DataSet)Session["VehicleDS"];

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (ddlcolorcode.SelectedValue == dr["Color"].ToString())
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Color Code: " + ddlcolorcode.SelectedValue + " Already Selected";
                        return;
                    }
                }

                if (ds.Tables[0].Rows[0]["VehicleCode"].ToString() == string.Empty)
                {
                    ds.Tables[0].Rows.RemoveAt(0);
                    create_row();
                }
                else
                {
                    create_row();
                }

                Session["VehicleDS"] = ds;
                //Uzair

                string sql = "select VehicleCode from Vehicle where BrandCode = '" + ddlmade.SelectedValue + "' and ProdCode = '" + txtvehicle_code.Text + "'" +
                    "and VersionCode = '" + txtmodel_code.Text.Trim() + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
                dt = SysFunc.GetData(sql);

                SqlParameter[] param2 = {
                                                new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                                new SqlParameter("@BrandCode",SqlDbType.Char),//1
                                                new SqlParameter("@ProdCode",SqlDbType.VarChar),//2
                                                new SqlParameter("@VersionCode",SqlDbType.Char),//3
                                                new SqlParameter("@ColorCode",SqlDbType.Char),//4
                                                new SqlParameter("@SalePrice",SqlDbType.Float),//5
                                                new SqlParameter("@Version",SqlDbType.Char),//6
                                                new SqlParameter("@VehicleStatus",SqlDbType.VarChar),//7
                                                new SqlParameter("@CostPrice",SqlDbType.Float),//8
                                                new SqlParameter("@WHTaxPerc",SqlDbType.Float),//9
                                                new SqlParameter("@WHTaxAmount",SqlDbType.Float),//10
                                                new SqlParameter("@FurTaxPer",SqlDbType.Float),//11
                                                new SqlParameter("@ExtraTaxPerc",SqlDbType.Float),//12
                                                new SqlParameter("@RegTaxPerc",SqlDbType.Float),//13
                                                new SqlParameter("@FederalTaxPerc",SqlDbType.Float),//14
                                                new SqlParameter("@FederalTaxAmount",SqlDbType.Float),//15
                                                new SqlParameter("@SEDTaxPerc",SqlDbType.Float),//16
                                                new SqlParameter("@SEDTaxAmount",SqlDbType.Float),//17
                                                new SqlParameter("@UpdUser",SqlDbType.VarChar),//18
                                                new SqlParameter("@UpdTerm",SqlDbType.VarChar),//19
                                                new SqlParameter("@IsActive",SqlDbType.Char),//20
                                                new SqlParameter("@VehicleCode",SqlDbType.Char),//21
                                                new SqlParameter("@RegType",SqlDbType.Char),//22
                                                };

                double WHTaxAmount = 0;

                WHTaxAmount = (double.Parse(txtSalePrice.Text) * double.Parse(txtWHTax.Text));

                param2[0].Value = Session["DealerCode"].ToString();
                param2[1].Value = ddlmade.SelectedValue;
                param2[2].Value = txtvehicle_code.Text;
                param2[3].Value = txtmodel_code.Text;
                param2[4].Value = ddlcolorcode.SelectedValue;
                param2[5].Value = txtSalePrice.Text.Trim();
                param2[6].Value = (object)DBNull.Value;
                param2[7].Value = ddlvehicle_status.SelectedValue;
                param2[8].Value = txtCostPrice.Text.Trim();
                param2[9].Value = txtWHTax.Text.Trim();
                param2[10].Value = WHTaxAmount;
                param2[11].Value = txtFurTax.Text.Trim();
                param2[12].Value = txtExtraTax.Text.Trim();
                param2[13].Value = txtRegTax.Text.Trim();
                param2[14].Value = (object)DBNull.Value;
                param2[15].Value = (object)DBNull.Value;
                param2[16].Value = (object)DBNull.Value;
                param2[17].Value = (object)DBNull.Value;
                param2[18].Value = Session["UserName"].ToString();
                param2[19].Value = GlobalVar.mUserIPAddress;
                if (cbIsActive.Checked == false)
                {
                    param2[20].Value = "N";
                }
                else
                {
                    param2[20].Value = "Y";
                }

                param2[21].Value = dt.Rows[0]["VehicleCode"].ToString();
                param2[22].Value = ddlRegType.Text;

                SysFunc.ExecuteSP_NonQuery("Sp_Insert_VehicleTaxDetail", param2);
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;
            }
        }

        protected void ddlvehicle_id_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlvehicle.SelectedIndex == 0)
            {
                cleardata();
                Create_Grid();
                return;
            }

            try
            {
               
                Create_Grid();
                DataTable dt = new DataTable();

                string sql = "Select * from Vehicle where DealerCode in( '" + Session["DealerCode"].ToString() + "','COMON') AND VehicleCode = '" + ddlvehicle.SelectedItem.Value + "'";
                dt = SysFunc.GetData(sql);

                if (dt.Rows[0]["IsActive"].ToString() == "Y")
                {
                    CheckBoxList1.SelectedIndex = 0;
                }
                else
                {
                    CheckBoxList1.SelectedIndex = 1;
                }

                ddlmade.SelectedValue = dt.Rows[0]["BrandCode"].ToString();
                txtvehicle_code.Text = dt.Rows[0]["ProdCode"].ToString();
                txtmodel_code.Text = dt.Rows[0]["VersionCode"].ToString();
                txtvehicle_desc.Text = dt.Rows[0]["ProdLongDesc"].ToString();
                txtshort_desc.Text = dt.Rows[0]["ProdDesc"].ToString();
                ddlvehicle_type.SelectedValue = dt.Rows[0]["VehTypeCode"].ToString().Trim() == "" ? "0" : dt.Rows[0]["VehTypeCode"].ToString().Trim();
                txtseater.Text = dt.Rows[0]["Seater"].ToString();
                txtNo_compartments.Text = dt.Rows[0]["Compartment"].ToString();
                txtWeight.Text = dt.Rows[0]["Weight"].ToString();
                txtengine_cap.Text = dt.Rows[0]["Power"].ToString();
                txtvehicle_prefix.Text = ""; // dt.Rows[0]["VehiclePrefix"].ToString();
               // ddlfuel_type.SelectedValue = dt.Rows[0]["CNGFlag"].ToString();                
                txtsch_plan.Text = dt.Rows[0]["SchedulePlan"].ToString();
                txtfree_service.Text = dt.Rows[0]["FreeServiceCup"].ToString();
                txtVehCategory.Text = dt.Rows[0]["VehicleCategory"].ToString();
                ddlBuiltUnit.SelectedValue = dt.Rows[0]["BuiltUnit"].ToString();
                txtBodyType.Text = dt.Rows[0]["BodyType"].ToString();
                txtPayLoad.Text = dt.Rows[0]["PayLoad"].ToString();


                if (dt.Rows[0]["TransmissionType"].ToString().Trim() == "Auto")
                {
                    ddltransmission_type.SelectedIndex = 0;
                }
                else
                {
                    ddltransmission_type.SelectedIndex = 1;
                }
                txtremarks.Text = dt.Rows[0]["Remarks"].ToString();
                if (Request.QueryString["image"] != null)
                {
                    Byte[] bytes = (Byte[])dt.Rows[0]["Pic"];
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();
                }

                DataTable dt2 = new DataTable();

                string sql2 = "Select * from VehicleTaxDetail WHERE DealerCode in( '" + Session["DealerCode"].ToString() + "','COMON') AND VehicleCode = '" + ddlvehicle.SelectedItem.Value + "'";
                dt2 = SysFunc.GetData(sql2);

                
                    if (dt.Rows[0]["VehicleStatus"].ToString() == "New")
                    {
                        ddlvehicle_status.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlvehicle_status.SelectedIndex = 1;
                    }
                

                DataSet ds = new DataSet();


                ds = (DataSet)Session["VehicleDS"];

                if (dt2.Rows.Count > 0)
                {
                    ds.Tables[0].Rows.RemoveAt(0);

                    for (int j = 0; j < dt2.Rows.Count; j++)
                    {
                        DataRow row = ds.Tables[0].NewRow();

                        row["VehicleCode"] = dt2.Rows[j]["VehicleCode"].ToString();
                        row["Color"] = dt2.Rows[j]["ColorCode"].ToString();
                        row["SalePrice"] = dt2.Rows[j]["SalePrice"].ToString();
                        row["CostPrice"] = dt2.Rows[j]["CostPrice"].ToString();
                        row["WHTax"] = dt2.Rows[j]["WHTaxPerc"].ToString();
                        row["FurTax"] = dt2.Rows[j]["FurTaxPerc"].ToString();
                        row["ExtraTax"] = dt2.Rows[j]["ExtraTaxPerc"].ToString();
                        row["RegType"] = dt2.Rows[j]["RegType"].ToString();
                        row["RegTax"] = dt2.Rows[j]["RegTaxPerc"].ToString();
                        row["IsActive"] = dt2.Rows[j]["IsActive"].ToString();

                        ds.Tables[0].Rows.Add(row);
                    }
                }
                else
                {
                    Create_Grid();
                    return;
                }

                gv_vehicle.DataSource = ds.Tables[0];
                gv_vehicle.DataBind();

                Session["VehicleDS"] = ds;

            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = ex.Message;
            }
        }

        protected void txtvehicle_desc_TextChanged(object sender, EventArgs e)
        {
            string sql = "Select * from Vehicle where DealerCode = '"+Session["DealerCode"].ToString()+"' and BrandCode = '"+ddlmade.SelectedValue+"' and ProdDesc = '"+ txtvehicle_desc.Text+"'";
            DataTable dt = new DataTable();
            dt = SysFunc.GetData(sql);
            if (dt.Rows.Count > 0)
            {
                txtvehicle_desc.BorderColor = Color.Red;
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = "Vehicle description already exist";
                btnSave1.Enabled = false;
                return;

            }
            else
            {
                txtvehicle_desc.BorderColor = Color.Black;
                btnSave1.Enabled = true;
            }

        }

        protected void txtvehicle_code_TextChanged(object sender, EventArgs e)
        {
            
            if(ddlvehicle.SelectedValue != "0")
            {
                string sql = "Select VersionCode from Vehicle where DealerCode = '" + Session["DealerCode"].ToString() + "' and BrandCode = '" + ddlmade.SelectedValue + "' and ProdLongDesc = '" + txtvehicle_desc.Text + "'";
                DataTable dt = new DataTable();
                dt = SysFunc.GetData(sql);

                string version = dt.Rows[0]["VersionCode"].ToString();

                if (SysFunc.CodeExists("CustomerVehicle", "VersionCode", version, Session["DealerCode"].ToString(), ref ds))
                {
                    lblMsg.Visible = true;
                    SysFunc.UserMsg(lblMsg, Color.Red, "Model Code Cannot be Change , this Model is already in use in Customer Vehicle",txtmodel_code);
                    txtmodel_code.Text = version;
                    lblMsg.Focus(); 
                    //lblMsg.Text = "Record Cannot be deleted , this Version is already in use";
                    return;
                }
                if (SysFunc.CodeExists("JobCardMaster", "VersionCode", version, Session["DealerCode"].ToString(), ref ds))
                {
                    SysFunc.UserMsg(lblMsg, Color.Red, "Model Code Cannot be Change , this Model is already in use in JobCard",txtmodel_code);
                    txtmodel_code.Text = version;
                    lblMsg.Focus();
                    //lblMsg.Text = "Record Cannot be deleted , this Version is already in use";
                    return;
                }
            }
        }

        protected void ddlvehicle_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                string sql = "Select VehCategory from VehicleType where DealerCode = 'COMON' AND VehTypeCode = '" + ddlvehicle_type.SelectedValue + "'";

                dt = SysFunc.GetData(sql);

                txtVehCategory.Text = dt.Rows[0]["VehCategory"].ToString();
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, ex.Message, txtmodel_code);
                throw;
            }
        }
    }
}