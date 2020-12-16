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
using Microsoft.ApplicationBlocks.Data;
using CrystalDecisions.Shared;
using CConn;
using DXBMS.Data;
using System.Threading;

namespace DXBMS.Modules.SpareParts.Forms
{
    public partial class ItemMaster : System.Web.UI.Page
    {
        DXBMS.SysFunctions SysFunc = new DXBMS.SysFunctions();
        SecurityBll sec = new SecurityBll();
        DataSet ds = new DataSet();
        clsLookUp clslook = new clsLookUp();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2539", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            //success_add_item.Visible = false;
            txtliter.Enabled = false;

            if (Page.IsPostBack)
            {

                if (!string.IsNullOrEmpty(Session["LookUpData"].ToString()))
                {
                    SelectedPartDetail(Session["LookUpData"].ToString());
                }
            }

            if (!IsPostBack)
            {

                ThreadStart childthreat = new ThreadStart(childthreadcall);
                Thread child = new Thread(childthreat);

                child.Start();

                Session["LookUpData"] = string.Empty;

                ddlpart_type.Items.Add(new ListItem("Select", ""));
                ddlpart_type.Items.Add(new ListItem("Lube", "Y"));
                ddlpart_type.Items.Add(new ListItem("Spare", "N"));

                ddlaccess.Items.Add(new ListItem("Select", ""));
                ddlaccess.Items.Add(new ListItem("Yes", "Y"));
                ddlaccess.Items.Add(new ListItem("No", "N"));

                ddlcogs.Items.Add(new ListItem("Select", ""));
                ddlrevenue.Items.Add(new ListItem("Select", ""));
                ddlstockin.Items.Add(new ListItem("Select", ""));
                ddlstockout.Items.Add(new ListItem("Select", ""));

                //ddllocation.Items.Add(new ListItem("Rack A Shelf1", "01A001"));
                //ddllocation.Items.Add(new ListItem("Rack A Shelf2", "01A002"));

                //LoadDDL_Item();
                LoadDDL_Unit();
                LoadDDL_ItemCategory();
                LoadDDL_Location();
                createItemDT();

            }

        }

        public void childthreadcall()
        {
            try
            {

                string sQuery = "SP_SelectParts '" + Session["DealerCode"].ToString() + "', 'P'";

                if (SysFunc.ExecuteQuery(sQuery, ref ds))
                {
                    Session["Parts"] = ds;

                }

            }
            catch (ThreadAbortException e)
            {

                //lblmessage.Text += "<br /> child thread - exception";

            }
            finally
            {
                //lblmessage.Text += "<br /> child thread - unable to catch the  exception";
            }
        }

        protected void SelectedPartDetail(string item)
        {
            try
            {

                DataTable dt = new DataTable();
                dt = SysFunc.GetData("select ItemDesc,PartItemNo from Item where DealerCode in ('" + Session["DealerCode"].ToString() + "','COMON') And  ItemCode='" + item + "'");
                
                txtItemCode.Text = item;
                ddlitemcode_SelectedIndexChanged(null, null);
                Session["LookUpData"] = string.Empty;
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, ex.Message);
            }
        }

        private void createItemDT()
        {
            DataSet ObjPOds = new DataSet();
            ObjPOds.Tables.Add("ItemStock");
            ObjPOds.Tables["ItemStock"].Columns.Add(new DataColumn("PartsTransLocCode", typeof(string)));
            ObjPOds.Tables["ItemStock"].Columns.Add(new DataColumn("PartsTransLocDesc", typeof(string)));
            ObjPOds.Tables["ItemStock"].Columns.Add(new DataColumn("ItemQty", typeof(string)));


            gridItemView.DataSource = ObjPOds.Tables["ItemStock"];
            gridItemView.DataBind();
            //ViewState["ObjPOds"] = ObjPOds;
        }
        private void LoadPartGrid()
        {
            SqlDataReader rder=null;
            DataTable dt = new DataTable();
            SqlParameter[] param = {
                                   new SqlParameter("@DealerCode",SqlDbType. Char,5),//0
                                   new SqlParameter("@Param",SqlDbType.VarChar,20),//0
                                   new SqlParameter("@FromPartItemNo",SqlDbType.VarChar,20),
                                   new SqlParameter("@ToPartItemNo",SqlDbType.VarChar,20)
                               };
            param[0].Value = this.Session["DealerCode"].ToString();
            param[1].Value = "--Select--";
            param[2].Value = txtpart_itemno.Text.Trim();
            param[3].Value = txtpart_itemno.Text.Trim();
            if (SysFunc.ExecuteSP("sp_PartsStockLocationSourceWise", param, ref rder))
            {
                dt.Load(rder);
                gridItemView.DataSource = dt;
                gridItemView.DataBind();
            }
            else
            {
                createItemDT();
            }
            
        }
        private void LoadDDL_Location()
        {
            string WhereQuery = "DealerCode = '" + Session["DealerCode"].ToString() + "'";

            string[] Columns = new string[] { "PartsTransLocCode", "PartsTransLocDesc" };
            SysFunc.GetMultiColumnsDDL(ddllocation, Columns, "PartsTransLocation", WhereQuery, "PartsTransLocCode", string.Empty, false, false);
        }
        private void LoadDDL_ItemCategory()
        {
            string WhereQuery = "DealerCode in ('" + Session["DealerCode"].ToString() + "','COMON')";

            string[] Columns = new string[] { "ItemCatCode", "ItemCatDesc" };
            SysFunc.GetMultiColumnsDDL(ddlitemcatcode, Columns, "ItemCategory", WhereQuery, "ItemCatCode", " Order by ItemCatCode Desc", false, false);
        }

        private void LoadDDL_Unit()
        {
            string WhereQuery = "DealerCode in ('" + Session["DealerCode"].ToString() + "','COMON')";

            string[] Columns = new string[] { "UnitCode", "UnitDesc" };
            SysFunc.GetMultiColumnsDDL(ddlunitcode, Columns, "Unit", WhereQuery, "UnitCode", " Order by UnitCode Desc", false, false);
        }

        protected void btnLookup_Click(object sender, EventArgs e)
        {
            ViewState["lookupid"] = 10; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_Parts_PO(imgLookup, ViewState["lookupid"].ToString(), "", "../../../../");
            

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }

        //private void LoadDDL_Item()
        //{
        //    string WhereQuery = "DealerCode = '" + Session["DealerCode"].ToString()+"'";

        //    string[] Columns = new string[] { "ItemCode","PartItemNo", "ItemDesc" };
        //    SysFunc.GetMultiColumnsDDL(ddlitemcode, Columns, "Item", WhereQuery, "ItemCode", " Order by ItemCode Desc", false, false);
        //}

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            SysFunc.ClearTextBoxes(Page);
            SysFunc.Clearddl(Page);
            //LoadDDL_Item();
            //LoadDDL_ItemCategory();
            //LoadDDL_Unit();
            cbactive.Checked = cbinventoryItem.Checked = cbsaleItem.Checked = false;
            RadioButtonList1.SelectedIndex = -1;
            createItemDT();
        }
        protected void btn_delete_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2539", "004"))
            {
                Response.Redirect("~/Test.aspx");
            }
           
            if (SysFunc.IsExist("ItemCode", txtItemCode.Text, "PODetail", Session["DealerCode"].ToString()))
            {
                SysFunc.UserMsg(lblMsg, Color.Red, "Item can not delete b/c its used in PO");
                return;
            }
            if (SysFunc.IsExist("ItemCode", txtItemCode.Text, "JobCardPartsDetail", Session["DealerCode"].ToString()))
            {
                SysFunc.UserMsg(lblMsg, Color.Red, "Item can not delete b/c its used in Job Card");
                return;
            }
            if (SysFunc.IsExist("ItemCode", txtItemCode.Text, "CounterSaleDetail", Session["DealerCode"].ToString()))
            {
                SysFunc.UserMsg(lblMsg, Color.Red, "Item can not delete b/c its used in Counter Sales");
                return;
            }
            if (SysFunc.IsExist("ItemCode", txtItemCode.Text, "ItemStock", Session["DealerCode"].ToString()))
            {
                SysFunc.UserMsg(lblMsg, Color.Red, "Item can not delete b/c its Exist in Stock");
                return;
            }
            if (txtItemCode.Text != "")
            {
                try
                {

                    string sql = "delete from Item where DealerCode='"+Session["DealerCode"].ToString()+"' And ItemCode = '" + txtItemCode.Text + "'";

                    SysFunc.ExecuteQuery_NonQuery(sql);

                    //LoadDDL_Item();
                    ///Clear Form controls///
                    SysFunc.ClearTextBoxes(Page);
                    SysFunc.Clearddl(Page);
                    /////////---////////
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert(' Deleted Successfully ');", true);
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('" + ex.Message + "');", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert(' Select System Ref. No. ');", true);
            }
        }
        protected void ddlitemcode_SelectedIndexChanged(object sender, EventArgs e)
        {

            
            try
            {
                DataTable dt = new DataTable();

                string sql = "select A.*,B.WAR as 'WARItemStatus' from Item A Left Outer join ItemStatus B On A.DealerCode in ('COMON',B.DealerCode) And A.ItemCode=B.ItemCode where A.DealerCode in ('" + Session["DealerCode"].ToString() + "','COMON') And A.ItemCode = '" + txtItemCode.Text + "'";
                dt = SysFunc.GetData(sql);

                string sale_Item = dt.Rows[0]["SaleItem"].ToString(),
                        inv_item = dt.Rows[0]["InventoryItem"].ToString(),
                        active = dt.Rows[0]["Active"].ToString();

                if (sale_Item != "")
                {
                    cbsaleItem.Checked = true;
                }
                if (inv_item != "")
                {
                    cbinventoryItem.Checked = true;
                }
                if (active != "")
                {
                    cbactive.Checked = true;
                }

                if (ddlpart_type.SelectedItem.Value == "Y")
                {
                    txtliter.Enabled = true;
                }
                else
                {
                    txtliter.Enabled = false;
                }

                string access = dt.Rows[0]["Access"].ToString();

                if (access == " " || access == "N")
                {
                    ddlaccess.SelectedValue = "N";
                }

                else
                {
                    ddlaccess.SelectedValue = "Y";
                }

                txtpart_itemno.Text = dt.Rows[0]["PartItemNo"].ToString();
                txtpart_desc.Text = dt.Rows[0]["ItemDesc"].ToString();
                //ddlunitcode.SelectedValue = dt.Rows[0]["UnitCode"].ToString();
                ddlitemcatcode.SelectedValue = dt.Rows[0]["ItemCatCode"].ToString();
                ddlpart_type.SelectedValue = dt.Rows[0]["LubeFlag"].ToString();
                if (dt.Rows[0]["PartsTransLocCode"].ToString() != string.Empty)
                    ddllocation.SelectedValue = dt.Rows[0]["PartsTransLocCode"].ToString();
                else ddllocation.SelectedIndex = 0; 
                txtpack_size.Text = dt.Rows[0]["PackingSize"].ToString();
                txtliter.Text = dt.Rows[0]["Liter"].ToString();
                txtproduct_code.Text = dt.Rows[0]["ProdCode"].ToString();
                RadioButtonList1.SelectedValue = dt.Rows[0]["Source"].ToString();
                txtmin_level.Text = dt.Rows[0]["MinLevel"].ToString();
                txtreorder_level.Text = dt.Rows[0]["Reorder"].ToString();
                txtwar.Text = dt.Rows[0]["WARItemStatus"].ToString();
                txtpurchase_amt.Text = dt.Rows[0]["PurchaseRate"].ToString();
                txtpromprod_pruchase.Text = dt.Rows[0]["PurchasePromotionPoint"].ToString();
                txtgst_purchase.Text = Convert.ToDecimal(dt.Rows[0]["PGSTPERC"].ToString() == "" ? "0" : dt.Rows[0]["PGSTPERC"].ToString()).ToString("##.##");
                txtfurther_purchase.Text = Convert.ToDecimal(dt.Rows[0]["PFGSTPERC"].ToString() == "" ? "0" : dt.Rows[0]["PFGSTPERC"].ToString()).ToString("##.##");
                txtextratax_purchase.Text = Convert.ToDecimal(dt.Rows[0]["ExTaxPerc"].ToString() == "" ? "0" : dt.Rows[0]["ExTaxPerc"].ToString()).ToString("##.##");
                txtsales_amt.Text = dt.Rows[0]["SaleRate"].ToString();
                txtpromprod_sales.Text = dt.Rows[0]["SalePromotionPoint"].ToString();
                txtgst_sales.Text = Convert.ToDecimal(dt.Rows[0]["GSTPerc"].ToString() == "" ? "0" : dt.Rows[0]["GSTPerc"].ToString()).ToString("##.##");
                txtfurther_sales.Text = Convert.ToDecimal(dt.Rows[0]["FGSTPerc"].ToString() == "" ? "0" : dt.Rows[0]["FGSTPerc"].ToString()).ToString("##.##");
                txtextratax_sales.Text = Convert.ToDecimal(dt.Rows[0]["SalesExTaxPerc"].ToString() == "" ? "0" : dt.Rows[0]["SalesExTaxPerc"].ToString()).ToString("##.##");
                ddlcogs.SelectedValue = dt.Rows[0]["COGS"].ToString().Trim();
                ddlstockin.SelectedValue = dt.Rows[0]["StockInAccount"].ToString().Trim();
                ddlrevenue.SelectedValue = dt.Rows[0]["RevenueAccount"].ToString().Trim();
                ddlstockout.SelectedValue = dt.Rows[0]["StockOutAccount"].ToString().Trim();

                LoadPartGrid();

                if (ddlpart_type.SelectedItem.Value == "Y")
                {
                    txtliter.Enabled = true;
                }
                else
                {
                    txtliter.Enabled = false;
                    txtliter.Text = "0";
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('" + ex.Message + "');", true);
            }
        }
        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2539", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (!sec.UserRight("2539", "003"))
            {
                Response.Redirect("~/Test.aspx");
            }
            string strAutoCode = string.Empty;

            string sale_item = "", inv_item = "", active = "";

            if (cbsaleItem.Checked == true)
            {
                sale_item = "Y";
            }
            if (cbinventoryItem.Checked == true)
            {
                inv_item = "Y";
            }
            if (cbactive.Checked == true)
            {
                active = "Y";
            }

            string source_radiobtn = RadioButtonList1.SelectedItem.Value;

            try
            {
                if (txtItemCode.Text == "")
                {
                    if(SysFunc.IsExist("PartItemNo",txtpart_itemno.Text.Trim(),"Item",Session["DealerCode"].ToString()))
                    {
                        SysFunc.UserMsg(lblMsg, Color.Red, "Part Item No is already exist");
                        return;
                    }
                    strAutoCode = SysFunc.GetNewMaxID("Item", "ItemCode", 8, Session["DealerCode"].ToString());
                }
                else
                {
                    strAutoCode = txtItemCode.Text;
                    //success_add_item.Text = "Item Updated Successfully";
                }

                SqlParameter[] param = {

                                    new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                    new SqlParameter("@ItemCatCode",SqlDbType.Char),//1
                                    new SqlParameter("@ItemCode",SqlDbType.Char),//2
                                    new SqlParameter("@PartItemNo",SqlDbType.VarChar),//3
                                    new SqlParameter("@ItemDesc",SqlDbType.VarChar),//4
                                    new SqlParameter("@UnitCode",SqlDbType.Char),//5
                                    new SqlParameter("@ProdCode",SqlDbType.VarChar),//6
                                    new SqlParameter("@PartsTransLocCode",SqlDbType.VarChar),//7
                                    new SqlParameter("@PurchaseRate",SqlDbType.Float),//8
                                    new SqlParameter("@SaleRate",SqlDbType.Float),//9
                                    new SqlParameter("@EssentialFlag",SqlDbType.Char),//10
                                    new SqlParameter("@Source",SqlDbType.VarChar),//11
                                    new SqlParameter("@MinLevel",SqlDbType.Float),//12
                                    new SqlParameter("@Reorder",SqlDbType.Float),//13
                                    new SqlParameter("@Warranty",SqlDbType.Char),//14
                                    new SqlParameter("@WarrMonths",SqlDbType.Float),//15
                                    new SqlParameter("@WarrRemarks",SqlDbType.VarChar),//16
                                    new SqlParameter("@LubeFlag",SqlDbType.Char),//17
                                    new SqlParameter("@AccountCode",SqlDbType.VarChar),//18
                                    new SqlParameter("@UpdUser",SqlDbType.VarChar),//19
                                    new SqlParameter("@UpdTerm",SqlDbType.VarChar),//20
                                    new SqlParameter("@SEDPerc",SqlDbType.Float),//21
                                    new SqlParameter("@OHQTY",SqlDbType.Decimal),//22
                                    new SqlParameter("@OHStock",SqlDbType.Decimal),//23
                                    new SqlParameter("@War",SqlDbType.Decimal),//24
                                    new SqlParameter("@Critical",SqlDbType.Char),//25
                                    new SqlParameter("@PackingSize",SqlDbType.Int),//26
                                    new SqlParameter("@Access",SqlDbType.Char),//27
                                    new SqlParameter("@SGOFlag",SqlDbType.Char),//28
                                    new SqlParameter("@Liter",SqlDbType.Float),//29
                                    new SqlParameter("@invType",SqlDbType.Char),//30
                                    new SqlParameter("@FGSTPerc",SqlDbType.Decimal),//31
                                    new SqlParameter("@GSTPerc",SqlDbType.Decimal),//32
                                    new SqlParameter("@SalesExTaxPerc",SqlDbType.Decimal),//33
                                    new SqlParameter("@Genuine",SqlDbType.Char),//34
                                    new SqlParameter("@Running",SqlDbType.Char),//35
                                    new SqlParameter("@PFGSTPERC",SqlDbType.Decimal),//36
                                    new SqlParameter("@PGSTPERC",SqlDbType.Decimal),//37
                                    new SqlParameter("@ExTaxPerc",SqlDbType.Decimal),//38
                                    new SqlParameter("@Active",SqlDbType.Char),//39
                                    new SqlParameter("@InventoryItem",SqlDbType.Char),//40
                                    new SqlParameter("@SaleItem",SqlDbType.Char),//41
                                    new SqlParameter("@COGS",SqlDbType.Char),//42
                                    new SqlParameter("@StockInAccount",SqlDbType.Char),//43
                                    new SqlParameter("@RevenueAccount",SqlDbType.Char),//44
                                    new SqlParameter("@StockOutAccount",SqlDbType.Char),//45
                                    new SqlParameter("@SalePromotionPoint",SqlDbType.Int),//46
                                    new SqlParameter("@PurchasePromotionPoint",SqlDbType.Int),//47
                                   };

                param[0].Value = Session["DealerCode"];
                param[1].Value = ddlitemcatcode.SelectedItem.Value;
                param[2].Value = strAutoCode;
                param[3].Value = txtpart_itemno.Text;
                param[4].Value = txtpart_desc.Text;
                param[5].Value = ddlunitcode.SelectedItem.Value;
                param[6].Value = txtproduct_code.Text;
                param[7].Value = ddllocation.SelectedValue;
                param[8].Value = txtpurchase_amt.Text;
                param[9].Value = txtsales_amt.Text;
                param[10].Value = "Y";
                param[11].Value = source_radiobtn;
                param[12].Value = txtmin_level.Text;
                param[13].Value = txtreorder_level.Text;
                param[14].Value = "N";
                param[15].Value = 0;
                param[16].Value = "";
                param[17].Value = ddlpart_type.SelectedItem.Value;
                param[18].Value = "";
                param[19].Value = "A";
                param[20].Value = "A";
                param[21].Value = 0;
                param[22].Value = 0.00000;
                param[23].Value = 0.00000;
                param[24].Value = 0.00000;
                param[25].Value = "Y";
                param[26].Value = Convert.ToInt32(txtpack_size.Text == "" ? "0" : txtpack_size.Text);
                param[27].Value = ddlaccess.SelectedItem.Value;
                param[28].Value = "N";
                param[29].Value = Convert.ToDecimal(txtliter.Text.Trim() == "" ? "0" : txtliter.Text.Trim());
                param[30].Value = "N";
                param[31].Value = Convert.ToDecimal(txtfurther_sales.Text.Trim() == "" ? "0" : txtfurther_sales.Text.Trim());
                param[32].Value = Convert.ToDecimal(txtgst_sales.Text.Trim() == "" ? "0" : txtgst_sales.Text.Trim());
                param[33].Value = Convert.ToDecimal(txtextratax_sales.Text.Trim() == "" ? "0" : txtextratax_sales.Text.Trim());
                param[34].Value = "";
                param[35].Value = "";
                param[36].Value = Convert.ToDecimal(txtfurther_purchase.Text.Trim() == "" ? "0" : txtfurther_purchase.Text.Trim());
                param[37].Value = Convert.ToDecimal(txtgst_purchase.Text.Trim() == "" ? "0" : txtgst_purchase.Text.Trim());
                param[38].Value = Convert.ToDecimal(txtextratax_purchase.Text.Trim() == "" ? "0" : txtextratax_purchase.Text.Trim());
                param[39].Value = active;
                param[40].Value = inv_item;
                param[41].Value = sale_item;
                param[42].Value = ddlcogs.SelectedItem.Value;
                param[43].Value = ddlstockin.SelectedItem.Value;
                param[44].Value = ddlrevenue.SelectedItem.Value;
                param[45].Value = ddlstockout.SelectedItem.Value;
                param[46].Value = Convert.ToInt32(txtpromprod_sales.Text == "" ? "0" : txtpromprod_sales.Text);
                param[47].Value = Convert.ToInt32(txtpromprod_pruchase.Text == "" ? "0" : txtpromprod_pruchase.Text);


                SysFunc.ExecuteSP_NonQuery("Sp_Insert_Item", param);
                ///Clear Form
                SysFunc.ClearTextBoxes(Page);
                SysFunc.Clearddl(Page);
                //////---////////

                SysFunc.UserMsg(lblMsg, Color.Green, "Item Saved Successfully");
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('Item Added Successfully' " + strAutoCode + " );", true);

                //LoadDDL_Item();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('" + ex.Message + "');", true);
            }
        }
        protected void ddlpart_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlpart_type.SelectedItem.Value == "Y")
            {
                txtliter.Enabled = true;
            }
            else
            {
                txtliter.Enabled = false;
                txtliter.Text = "0";
            }
        }

        protected void txtmin_level_TextChanged(object sender, EventArgs e)
        {

        }
    }
}