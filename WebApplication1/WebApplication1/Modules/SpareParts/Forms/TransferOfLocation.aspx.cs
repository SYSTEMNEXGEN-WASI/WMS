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

namespace DXBMS.Modules.SpareParts.Forms
{
    public partial class TransferOfLocation : System.Web.UI.Page
    {

        SysFunction SysFunc = new SysFunction();
        SecurityBll sec = new SecurityBll();
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        Transaction ObjTrans = new Transaction();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2551", "001"))
            {
                Response.Redirect("~/Test.aspx");
            } 
            if (!IsPostBack)
            {
                LoadDDL_TransLoc();

                create_grid();
            }

            success_add_item.Visible = false;

        }
        protected void btn_clear_Click(object sender, EventArgs e)
        {
            cleardata();
        }

        private void cleardata()
        {
            SysFunc.Clearddl(Page);
            SysFunc.ClearTextBoxes(Page);
            create_grid();
            success_add_item.Visible = false;
        }

        private void LoadDDL_TransLoc()
        {
            string WhereQuery = "DealerCode = '" + Session["DealerCode"] + "' AND DelFlag = 'N' ";

            string[] Columns = new string[] { "TransNo", "TransDate" };
            SysFunc.GetMultiColumnsDDL(ddltransfer_no, Columns, "PartsLocTransMaster", WhereQuery, "TransNo", "", false, false);
        }


        private void create_grid()
        {
            ds = new DataSet();
            ds.Tables.Add();
            ds.Tables[0].Columns.Add(new DataColumn("ItemCode", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("PartItemNo", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("ItemDesc", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("FromLocCode", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("ToLocCode", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Qty", typeof(string)));

            DataRow dr = ds.Tables[0].NewRow();
            ds.Tables[0].Rows.Add(dr);


            GridView_Location.DataSource = ds.Tables[0];
            GridView_Location.DataBind();

            Session["LocationDS"] = ds;
        }

        protected void BtnRemove_loc_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsdelete = new DataSet();

            Label itemcode = (Label)GridView_Location.FindControl("lblItemNo");

            dsdelete = (DataSet)Session["LocationDS"];

            try
            {


                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;

                dsdelete.Tables[0].Rows[rowGvIndex].Delete();
                dsdelete.Tables[0].AcceptChanges();
                if (dsdelete.Tables[0].Rows.Count == 0)
                {
                    DataRow dr = dsdelete.Tables[0].NewRow();
                    dsdelete.Tables[0].Rows.Add(dr);
                }


                GridView_Location.DataSource = dsdelete.Tables[0];
                GridView_Location.DataBind();

                Session["LocationDS"] = dsdelete;

            }
            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.ForeColor = Color.Red;
                success_add_item.Text = ex.Message;
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2551", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            string strAutoCode = string.Empty;
            SqlTransaction trans = null;
            bool check = false;

            try
            {
                if (ObjTrans.BeginTransaction(ref trans) == true)
                {

                }
                else
                {
                    return;
                }

                if (ddltransfer_no.SelectedIndex == 0)
                {
                    strAutoCode = SysFunc.GetNewMaxID("PartsLocTransMaster", "TransNo", 8, Session["DealerCode"].ToString());
                }
                else
                {
                    if (!sec.UserRight("2551", "003"))
                    {
                        Response.Redirect("~/Test.aspx");
                    }
                    strAutoCode = ddltransfer_no.SelectedItem.Value;

                }

                DateTime transDate = DateTime.ParseExact(txtdate.Text, "dd-MM-yyyy", null);
                DateTime postDate = DateTime.ParseExact(txtdate.Text, "dd-MM-yyyy", null);

                SqlParameter[] param = {
                                     new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                     new SqlParameter("@TransNo", SqlDbType.Char),//1
                                     new SqlParameter("@TransDate",SqlDbType.DateTime),//2
                                     new SqlParameter("@Remarks",SqlDbType.VarChar),//3
                                     new SqlParameter("@DelFlag",SqlDbType.Char),//4
                                     new SqlParameter("@UpdUser",SqlDbType.VarChar),//5
                                     new SqlParameter("@UpdTerm",SqlDbType.VarChar),//6
                                     new SqlParameter("@PostDate",SqlDbType.DateTime),//7

                                   };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = strAutoCode;
                param[2].Value = transDate;
                param[3].Value = txtremarks.Text;
                param[4].Value = "N";
                param[5].Value = Session["UserName"].ToString();
                param[6].Value = GlobalVar.mUserIPAddress;
                param[7].Value = postDate;

                if (SysFunc.ExecuteSP_NonQuery("Sp_Insert_PartsLocTranstMaster", param, trans) == true)
                {
                    SqlParameter[] param2 = {
                                          new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                          new SqlParameter("@TransNo",SqlDbType.Char),//1
                                          new SqlParameter("@ItemCode",SqlDbType.Char),//2
                                          new SqlParameter("@FromLocCode",SqlDbType.Char),//3
                                          new SqlParameter("@ToLocCode",SqlDbType.Char),//4
                                          new SqlParameter("@Qty",SqlDbType.Float),//5
                                       };

                    for (int j = 0; j < GridView_Location.Rows.Count; j++)
                    {

                        Label lblItemCode = (Label)GridView_Location.Rows[j].FindControl("lblItemNo");
                        Label lblFromLocCode = (Label)GridView_Location.Rows[j].FindControl("lblFromLocCode");
                        Label lblToLocCode = (Label)GridView_Location.Rows[j].FindControl("lblToLocCode");
                        Label lblQty = (Label)GridView_Location.Rows[j].FindControl("lblQty");

                        param2[0].Value = Session["DealerCode"].ToString();
                        param2[1].Value = strAutoCode;
                        param2[2].Value = lblItemCode.Text;
                        param2[3].Value = lblFromLocCode.Text;
                        param2[4].Value = lblToLocCode.Text;
                        param2[5].Value = lblQty.Text;

                        SysFunc.ExecuteSP_NonQuery("Sp_Insert_PartsLocTransDetail", param2, trans);
                        if (UpdateItemStock(lblItemCode.Text, lblFromLocCode.Text, lblToLocCode.Text, double.Parse(lblQty.Text), ref trans) == true)
                        {
                            check = true;
                        }
                        else
                        {
                            check = false;
                            return;

                        }
                    }

                    cleardata();

                    create_grid();

                    success_add_item.Visible = true;
                    success_add_item.ForeColor = Color.Red;
                    success_add_item.Text = "Location Transfered Successfully";

                    LoadDDL_TransLoc();

                }
                else
                {
                    check = false;
                    return;
                }

                if (check == true)
                {
                    ObjTrans.CommittTransaction(ref trans);
                }
                else
                {
                    ObjTrans.RollBackTransaction(ref trans);
                    return;
                }



            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref trans);
                success_add_item.Visible = true;
                success_add_item.ForeColor = Color.Red;
                success_add_item.Text = ex.Message;
            }
        }

        protected void GridView_Location_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();

                GridView_Location.EditIndex = e.NewEditIndex;

                ds = (DataSet)Session["LocationDS"];

                GridView_Location.DataSource = ds.Tables[0];

                GridView_Location.DataBind();
            }

            catch (Exception ex)
            {

                success_add_item.Visible = true;
                success_add_item.ForeColor = Color.Red;
                success_add_item.Text = ex.Message;

            }

        }

        protected void GridView_Location_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataSet ds = new DataSet();

            GridView_Location.EditIndex = -1;

            ds = (DataSet)Session["LocationDS"];

            GridView_Location.DataSource = ds.Tables[0];

            GridView_Location.DataBind();
        }

        protected void GridView_Location_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataSet ds_update = new DataSet();

                TextBox txtqty = (TextBox)GridView_Location.Rows[e.RowIndex].FindControl("txtQty");
                DropDownList ToLocCode = (DropDownList)GridView_Location.Rows[e.RowIndex].FindControl("ddlToLoc_code");
                Label itemcode = (Label)GridView_Location.Rows[e.RowIndex].FindControl("lblItemNo");

                double qty = double.Parse(txtqty.Text.Trim() == "" ? "0" : txtqty.Text.Trim());

                if (ToLocCode.SelectedIndex == 0)
                {
                    success_add_item.Visible = true;
                    success_add_item.Text = "Select (To Location)";
                    return;
                }

                if (qty <= 0 || txtqty.Text.Trim() == string.Empty)
                {
                    success_add_item.Visible = true;
                    success_add_item.Text = "Quantity can not Zero(0) or empty.";
                    return;
                }


                ds_update = (DataSet)Session["LocationDS"];

                foreach (DataRow dr in ds_update.Tables[0].Rows)
                {
                    if (dr["ItemCode"].ToString() == itemcode.Text)
                    {
                        dr["ToLocCode"] = ToLocCode.SelectedItem.Value;
                        dr["Qty"] = qty;
                    }
                }

                GridView_Location.EditIndex = -1;

                GridView_Location.DataSource = ds_update.Tables[0];
                GridView_Location.DataBind();

                Session["LocationDS"] = ds_update;


            }
            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.ForeColor = Color.Red;
                success_add_item.Text = ex.Message;
            }
        }

        protected void GridView_Location_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            try
            {
                ImageButton btn_remove = (ImageButton)e.Row.FindControl("BtnRemove_loc");
                Label Item_Code = (Label)e.Row.FindControl("lblItemNo");

                if (e.Row.RowType == DataControlRowType.DataRow && !(GridView_Location.EditIndex == e.Row.RowIndex) && Item_Code.Text == string.Empty)
                {
                    btn_remove.Enabled = false;
                    e.Row.Cells[0].Enabled = false;
                }

                if (e.Row.RowType == DataControlRowType.DataRow && (GridView_Location.EditIndex == e.Row.RowIndex))
                {
                    btn_remove.Enabled = false;
                    DropDownList ddlToLoc_code = (DropDownList)e.Row.FindControl("ddlToLoc_code");

                    string WhereQuery2 = "DealerCode = '" + Session["DealerCode"] + "'";

                    string[] Columns2 = new string[] { "PartsTransLocCode", "PartsTransLocDesc" };
                    SysFunc.GetMultiColumnsDDL(ddlToLoc_code, Columns2, "PartsTransLocation", WhereQuery2, "PartsTransLocCode", "", false, false);
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DropDownList ddlitem_code = (DropDownList)e.Row.FindControl("ddlItemCode");

                    Load_ItemLocation_ddl(ddlitem_code, Session["DealerCode"].ToString());

                    DropDownList ddlToLoc_code = (DropDownList)GridView_Location.FooterRow.FindControl("ddlToLoc_code");

                    string WhereQuery2 = "DealerCode = '" + Session["DealerCode"] + "'";

                    string[] Columns2 = new string[] { "PartsTransLocCode", "PartsTransLocDesc" };
                    SysFunc.GetMultiColumnsDDL(ddlToLoc_code, Columns2, "PartsTransLocation", WhereQuery2, "PartsTransLocCode", "", false, false);

                }
            }
            catch (Exception ex)
            {

                success_add_item.Visible = true;
                success_add_item.ForeColor = Color.Red;
                success_add_item.Text = ex.Message;
            }
        }

        protected void GridView_Location_DataBound(object sender, EventArgs e)
        {
            try
            {

                DropDownList ddlitem_code = (DropDownList)GridView_Location.FooterRow.FindControl("ddlItemCode");

                Load_ItemLocation_ddl(ddlitem_code, Session["DealerCode"].ToString());

                DropDownList ddlToLoc_code = (DropDownList)GridView_Location.FooterRow.FindControl("ddlToLoc_code");

                string WhereQuery2 = "DealerCode = '" + Session["DealerCode"] + "'";

                string[] Columns2 = new string[] { "PartsTransLocCode", "PartsTransLocDesc" };
                SysFunc.GetMultiColumnsDDL(ddlToLoc_code, Columns2, "PartsTransLocation", WhereQuery2, "PartsTransLocCode", "", false, false);

            }
            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.ForeColor = Color.Red;
                success_add_item.Text = ex.Message;
            }

        }

        private void create_row_location()
        {
            DropDownList ddlitem_code = (DropDownList)GridView_Location.FooterRow.FindControl("ddlItemCode");
            DropDownList ddlToLoc_code = (DropDownList)GridView_Location.FooterRow.FindControl("ddlToLoc_code");
            TextBox txtFromLoc = (TextBox)GridView_Location.FooterRow.FindControl("txtFromLoc");
            TextBox txtQty = (TextBox)GridView_Location.FooterRow.FindControl("txtQty");

            if (ddlitem_code.SelectedIndex == 0)
            {
                success_add_item.Visible = true;
                success_add_item.Text = "Select Part No.";
                return;
            }
            if (ddlToLoc_code.SelectedIndex == 0)
            {
                success_add_item.Visible = true;
                success_add_item.Text = "Select To Location ";
                return;
            }
            if (txtQty.Text == "" || txtQty.Text == "0")
            {
                success_add_item.Visible = true;
                success_add_item.Text = "Quantity cannot be empty or Zer(0)";
                return;
            }
            try
            {
                DataTable dt_newrow = new DataTable();

                string ItemCode, LocCode = "";
                ItemCode = ddlitem_code.SelectedValue.ToString().Substring(0, 8); //00000010,01B006 
                LocCode = ddlitem_code.SelectedValue.ToString().Substring(9, 6); //ddlitem_code.SelectedItem.Value.ToString().Length)

                string qry = "SELECT PartItemNo ,ItemDesc from Item Where ItemCode = '" + ItemCode + "' AND DealerCode in ('" + Session["DealerCode"] + "','COMON')";
                dt_newrow = SysFunc.GetData(qry);

                ds = new DataSet();

                ds = (DataSet)Session["LocationDS"];

                DataRow row = ds.Tables[0].NewRow();

                row["ItemCode"] = ItemCode;
                row["PartItemNo"] = dt_newrow.Rows[0]["PartItemNo"].ToString();
                row["ItemDesc"] = dt_newrow.Rows[0]["ItemDesc"].ToString();
                row["FromLocCode"] = LocCode;
                row["ToLocCode"] = ddlToLoc_code.SelectedItem.Value;
                row["Qty"] = txtQty.Text.Trim();

                ds.Tables[0].Rows.Add(row);

                GridView_Location.DataSource = ds.Tables[0];
                GridView_Location.DataBind();

                Session["LocationDS"] = ds;

            }
            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.ForeColor = Color.Red;
                success_add_item.Text = ex.Message;
            }

        }


        protected void BtnAdd_Item_Click(object sender, ImageClickEventArgs e)
        {
            ds = new DataSet();
            DropDownList ddlitem_code = (DropDownList)GridView_Location.FooterRow.FindControl("ddlItemCode");


            if (ddlitem_code.SelectedIndex == 0)
            {
                success_add_item.Visible = true;
                success_add_item.Text = "Select Part No.";
                return;
            }

            string partItem_No = ddlitem_code.SelectedItem.Value.Substring(0, 8);


            try
            {
                ds = (DataSet)Session["LocationDS"];

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (partItem_No == dr["ItemCode"].ToString())
                    {
                        success_add_item.Visible = true;
                        success_add_item.Text = "Part Item No : " + partItem_No + " Already Selected";
                        return;
                    }

                }
                if (ds.Tables[0].Rows[0]["ItemCode"].ToString() == String.Empty)
                {
                    ds.Tables[0].Rows.RemoveAt(0);
                    create_row_location();
                }
                else
                {
                    create_row_location();
                }

            }
            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.ForeColor = Color.Red;
                success_add_item.Text = ex.Message;
            }


        }

        protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlitem_code = (DropDownList)GridView_Location.FooterRow.FindControl("ddlItemCode");
            TextBox txtFromLoc = (TextBox)GridView_Location.FooterRow.FindControl("txtFromLoc");

            if (ddlitem_code.SelectedIndex == 0)
            {
                success_add_item.Visible = true;
                success_add_item.Text = "Select Part Item ";
                return;
            }
            try
            {
                DataTable dt = new DataTable();
                string LocCode = "";
                LocCode = ddlitem_code.SelectedValue.Substring(9, 6);

                txtFromLoc.Text = LocCode;

            }
            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.ForeColor = Color.Red;
                success_add_item.Text = ex.Message;
            }
        }

        public void Load_ItemLocation_ddl(DropDownList ddlCSItem, string DealerCode)
        {

            SqlDataReader Reader = null;
            string strSql = string.Empty;
            strSql = "WITH maxLen AS (SELECT  " +
            "max(len(rtrim(A.ItemCode))) Len0, " +
            "max(len(rtrim(A.ItemDesc))) Len1, " +
            "max(len(rtrim(A.PartItemNo))) Len2, " +
            "MAX(len(RTRIM(B.PartsTransLocCode))) Len3, " +
            "MAX(len(LTrim(RTrim(PL.PartsLocDesc)) +','+ LTrim(RTrim(PSL.PartsSubLocDesc)) +','+ LTrim(RTrim(PTL.PartsTransLocDesc)) )) Len4 " +
            "FROM Item A Inner Join ItemStock B " +
            "On A.ItemCode=B.ItemCode And B.DealerCode in ('"+Session["DealerCode"].ToString()+"','COMON') " +
            "inner join PartsTransLocation PTL " +
            "On B.PartsTransLocCode=PTL.PartsTransLocCode and B.DealerCode=PTL.DealerCode " +
            "Inner join  partslocation PL " +
            "On PTL.PartsLocCode=PL.PartsLocCode " +
            "And PTL.DealerCode=PL.DealerCode " +
            "Inner join PartsSubLocation PSL " +
            "On PTL.PartsSubLocCode=PSL.PartsSubLocCode " +
            "And PTL.DealerCode=PSL.DealerCode " +
            "Where  A.DealerCode in ('" + DealerCode + "','COMON')) " +
            "SELECT A.ItemCode + ',' + B.PartsTransLocCode As ItemCode, "
    + "+' | '+" + "(rtrim(A.ItemCode) + space((1+maxLen.Len0)-len(rtrim(A.ItemCode))) "
    + "+' | '+" + "rtrim(A.ItemDesc) + space((1+maxLen.Len1)-len(rtrim(A.ItemDesc))) "
    + "+' | '+" + "rtrim(A.PartItemNo) + space((1+maxLen.Len2)-len(rtrim(A.PartItemNo))) "
    + "+' | '+" + "rtrim(B.PartsTransLocCode) + space((1+maxLen.Len3)-len(rtrim(B.PartsTransLocCode))) "
    + "+' | '+" + "rtrim(LTrim(RTrim(PL.PartsLocDesc)) +','+ LTrim(RTrim(PSL.PartsSubLocDesc)) +','+ LTrim(RTrim(PTL.PartsTransLocDesc))) + space((1+maxLen.Len4)-len(rtrim(LTrim(RTrim(PL.PartsLocDesc)) +','+ LTrim(RTrim(PSL.PartsSubLocDesc)) +','+ LTrim(RTrim(PTL.PartsTransLocDesc)) ))) "
    + "+' | '+" + "rtrim(IsNull(Sum(B.RecQty) + Sum(B.PurRetQty) + Sum(B.RetQty) - ((Sum(B.IssQty) + Sum(B.ChargeOutQty)) ), 0)) ) As Name " +
    "from Item A Inner Join ItemStock B " +
    "On A.ItemCode=B.ItemCode And B.DealerCode in ('" + Session["DealerCode"].ToString() + "','COMON')" +
    "inner join PartsTransLocation PTL " +
    "On B.PartsTransLocCode=PTL.PartsTransLocCode and B.DealerCode=PTL.DealerCode " +
    "Inner join  partslocation PL " +
    "On PTL.PartsLocCode=PL.PartsLocCode " +
    "And PTL.DealerCode=PL.DealerCode " +
    "Inner join PartsSubLocation PSL " +
    "On PTL.PartsSubLocCode=PSL.PartsSubLocCode " +
    "And PTL.DealerCode=PSL.DealerCode,maxLen " +
    "Where  A.DealerCode in('" + DealerCode + "','COMON') " +
    "Group by A.ItemCode,maxLen.Len0,A.ItemDesc,maxLen.Len1,A.PartItemNo, " +
    "maxLen.Len2,B.PartsTransLocCode,maxLen.Len3,PTL.PartsTransLocDesc,PL.PartsLocDesc,PSL.PartsSubLocDesc,maxLen.Len4 ";

            if (Reader != null && Reader.IsClosed == false) Reader.Close();
            SysFunc.ExecuteQuery(strSql, ref Reader);
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


        public bool UpdateItemStock(string ItemCode, string FromLocCode, string ToLocCode, double Qty, ref SqlTransaction Trans)
        {
            string sQuery = String.Empty;


            if ((SysFunc.IsExist("ItemCode", ItemCode.Trim(), "ItemStock",
                    Session["DealerCode"].ToString(), " AND PartsTransLocCode = '" + ToLocCode.Trim() + "'") == true))
            {
                sQuery = ("UPDATE ItemStock SET RecQty = RecQty + "
                            + (Qty + (" WHERE LTrim(RTrim(PartsTransLocCode)) + LTrim(RTrim(ItemCode)) =\'"
                            + ((ToLocCode.Trim() + ItemCode.Trim()) + ("\' And DealerCode = \'"
                            + (Session["DealerCode"] + "\'"))))));
                if (!SysFunc.ExecuteQuery_NonQuery(sQuery))
                {
                    ObjTrans.RollBackTransaction(ref Trans);
                    return false;
                }

                sQuery = ("UPDATE ItemStock SET IssQty = IssQty + "
                            + (Qty + (" WHERE LTRIM(RTRIM(PartsTransLocCode))+LTRIM(RTRIM(ItemCode)) =\'"
                            + ((FromLocCode.Trim() + ItemCode.Trim()) + ("\' and DealerCode = \'"
                            + (Session["DealerCode"] + "\'"))))));
                if (!SysFunc.ExecuteQuery_NonQuery(sQuery))
                {
                    ObjTrans.RollBackTransaction(ref Trans);
                    return false;
                }

            }
            else
            {
                sQuery = ("INSERT INTO ItemStock (DealerCode,PartsTransLocCode,ItemCode,RecQty) VALUES (\'"
                            + (Session["DealerCode"] + ("\',\'"
                            + (ToLocCode.Trim() + ("\',\'"
                            + (ItemCode + ("\',"
                            + (Qty + ")"))))))));
                if (!SysFunc.ExecuteQuery_NonQuery(sQuery))
                {
                    ObjTrans.RollBackTransaction(ref Trans);
                    return false;
                }

                sQuery = ("UPDATE ItemStock SET IssQty = IssQty + "
                            + (Qty + (" WHERE LTRIM(RTRIM(PartsTransLocCode))+LTRIM(RTRIM(ItemCode)) =\'"
                            + ((FromLocCode.Trim() + ItemCode.Trim()) + ("\' and DealerCode = \'"
                            + (Session["DealerCode"] + "\'"))))));
                if (!SysFunc.ExecuteQuery_NonQuery(sQuery))
                {
                    ObjTrans.RollBackTransaction(ref Trans);
                    return false;
                }

            }

            return true;
        }

    }
}