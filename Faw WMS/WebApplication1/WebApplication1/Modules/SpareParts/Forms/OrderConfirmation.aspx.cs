using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;
namespace DXBMS.Modules.SpareParts.Forms
{
    public partial class OrderConfirmation : System.Web.UI.Page
    {


        SysFunctions myFunc = new SysFunctions();
        DataSet DCds = null;
        SysFunction sysFunc = new SysFunction();
        MainBLL objMBLL = new MainBLL();
        DataSet ds;
        Transaction ObjTrans = new Transaction();
        SecurityBll sec = new SecurityBll();
        SqlTransaction Trans;
        clsLookUp clslook = new clsLookUp();
        static int btnValue = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            txtDCDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtPODate.Text = DateTime.Now.ToString("dd-MM-yyyy");

            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2542", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }

            if (Page.IsPostBack)
            {

                if (!string.IsNullOrEmpty(Session["LookUpData"].ToString()))
                {
                    SelectedPartDetail(Session["LookUpData"].ToString());
                }
            }
            if (!IsPostBack)
            {
                if (Session["TransCode"] != null)
                {
                    createGrid();
                    lOADddl();
                    string leadId = Session["TransCode"].ToString();
                    ddlDCno.SelectedValue = leadId;

                    ddlDCno_SelectedIndexChanged(null, null);


                }
                else
                {
                    lblMode.Text = "Add Mode";
                    createGrid();
                    lOADddl();
                }
                
            }
            btnValue = 0;
            Session["TransCode"] = null;
            Session["LookUpData"] = string.Empty;
        }



        private void lOADddl()
        {
            // DDLColums retrive
            string[] col1 = new string[] { "A.DCNo", "convert(varchar(12),A.DCDate,103)", "A.PONo", "B.VendorDesc", "A.VendorDCNo" };      
            
            string Where = " A.VendorCode = B.VendorCode AND A.DealerCode= '" + Session["DealerCode"].ToString() + "'and a.DelFlag = 'N' "
	                       +" and a.VendorCode = b.VendorCode and B.DealerCode in(A.DealerCode,'COMON') "
                           + " and a.DCNo Not IN  (Select DCNo from GRNMaster Where DealerCode in ('" + Session["DealerCode"].ToString() + "','AAAAA') and Delflag = 'N') ";

            sysFunc.GetMultiColumnsDDL(ddlDCno, col1, "DCMaster A, Vendor B  ", Where, "DCNo", "ORDER BY A.DCNO DESC", false, false);

            string[] col = new string[] { "a.PONo", "convert(varchar(20), a.PODate, 103)", "c.VendorDesc","d.NName" };
            string Where2 = "a.VendorCode = c.VendorCode  and a.DealerCode = b.DealerCode and c.DealerCode in (a.DealerCode,'COMON') "
                 + "and a.PONo = b.PONo  "
				 +" and a.DeliveryType = d.NName"
                 + " and a.DealerCode in( '" + Session["DealerCode"].ToString() + "')"
				 +" and a.TransferStatus ='T'";
            sysFunc.GetMultiColumnsDDL(ddlPONo, col, "POMaster a,PODetail b,Vendor c,partsDeliveryTypes d", Where2, "PONo", "Order by a.PONo DESC", true, true, "Having sum(b.POQty) > sum(b.RecQty)");           
            
            //string WhereQuery = "1=1";
            //string[] col1 = new string[] { "VendorCode", "VendorDesc" };
            //sysFunc.GetMultiColumnsDDL(ddlVendor, col1, "Vendor", WhereQuery, "VendorCode", "", false, false);

            //string[] col3 = new string[] { "NName" };
            //sysFunc.GetMultiColumnsDDL(ddlDeliveryType, col3, "PartsDeliveryTypes", WhereQuery, "NName", "", false, false);

            
        }


        private void createGrid()
        {
            DataTable PartsDT = new DataTable();
            PartsDT.Columns.Add("SNo", typeof(int));
            PartsDT.Columns.Add(new DataColumn("Selected", typeof(bool)));
            PartsDT.Columns.Add(new DataColumn("PartItemNo", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("PartItemName", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("unit", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("POQuantity", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("BalanceQuantity", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("DCQuantity", typeof(string)));
            PartsDT.PrimaryKey = new DataColumn[] { PartsDT.Columns["PartItemNo"] };

            gv_DC.DataSource = PartsDT; gv_DC.DataBind(); Session["DCTable"] = PartsDT;

        }

        private void ResetDCItemsDataSet()
        {
            DCds = new DataSet();
            DataTable DCItems = new DataTable("DCItems");
            DCItems.Columns.Add("SNo", typeof(int));
            DCItems.Columns.Add("Selected", typeof(bool));
            DCItems.Columns.Add("ItemCode", typeof(string));
            DCItems.Columns.Add("PartItemNo", typeof(string));
            DCItems.Columns.Add("PartItemName", typeof(string));
            DCItems.Columns.Add("unit", typeof(string));
            DCItems.Columns.Add("POQuantity", typeof(decimal));
            DCItems.Columns.Add("BalanceQuantity", typeof(decimal));
            DCItems.Columns.Add("DCQuantity", typeof(decimal));
            DCItems.PrimaryKey = new DataColumn[] { DCItems.Columns["PartItemNo"] };

            DCds.Tables.AddRange(new DataTable[] { DCItems });
            Session["DCDataSet"] = DCds;


        }

        protected void ddlDCno_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMode.Text = "Edit Mode";
            string sql = "select * from DCMaster where DealerCode='" + Session["DealerCode"].ToString() + "' And DCNo='" + ddlDCno.SelectedValue + "'";
            DataTable dt = myFunc.GetData(sql);
            if (dt.Rows.Count > 0)
            {
                txtDCDate.Text = dt.Rows[0]["DCDate"].ToString();
                //ddlPONo.Text = dt.Rows[0]["PONo"].ToString();
                ddlPONo.Visible = false;
                lblPONo.Visible = true;
                lblPONo.Text = dt.Rows[0]["PONo"].ToString();

                this.Session["PONumber"] = dt.Rows[0]["PONo"].ToString();
                txtVendorDCn0.Text = dt.Rows[0]["VendorDCNo"].ToString();
                txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();

                this.Session["DCNumber"] = ddlDCno.SelectedValue;
                LoadPOInfo(lblPONo.Text);
                //btnPOSelect_Click(btnPOSelect, EventArgs.Empty);

                // loading details of DC
                DataTable dtTable = new DataTable();
                dtTable = (DataTable)this.Session["DCTable"];
                dtTable.Rows.Clear();

                dt = myFunc.GetData("select * from DCDetail where DealerCode='" + Session["DealerCode"].ToString() + "' And DCNo='" + ddlDCno.SelectedValue + "'");

                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow dRow in dt.Rows)
                    {
                        DataRow row = dtTable.NewRow();
                        row["Sno"] = dtTable.Rows.Count + 1;

                        row["PartItemNo"] = myFunc.GetPartItemNoByItemCode(dRow["ItemCode"].ToString());
                        row["PartItemName"] = myFunc.GetItemDescByItemCode(dRow["ItemCode"].ToString());
                        row["unit"] = myFunc.GetUnitDescByPartItemNo(row["PartItemNo"].ToString());
                        row["POQuantity"] = dRow["POQty"].ToString();
                        row["BalanceQuantity"] = dRow["BalQty"].ToString();
                        row["DCQuantity"] = dRow["DCQty"].ToString();
                        //row["Selected"] = true;
                        dtTable.Rows.Add(row);

                    }
                }

                Session["DCTable"] = dtTable;
                gv_DC.DataSource = dtTable;
                gv_DC.DataBind();

                txtStatus.Text = "OLD";
                //btnDelete.Enabled = true;

            }
            else
            {
                return;
            }
        }

        private void LoadGrid(string DCNumber, string PONumber, string DealerCode)
        {
            DataTable dtParts = new DataTable();
            DataTable dt = new DataTable();
            DataSet dsParts = new DataSet();
            SqlDataReader dr = null;
            string mPONo = "";
            string mDCNo = "";
            if (PONumber == "")
            {
                mPONo = Session["PONumber"].ToString();
            }
            else
            {
                mPONo = PONumber;
            }
            if (DCNumber == "")
            {
                mDCNo = Session["DCNumber"].ToString();
            }
            else
            {
                mDCNo = DCNumber;
            }
            try
            {
                SqlParameter[] param = {
                new SqlParameter("@DealerCode",SqlDbType.Char,10) ,
                new SqlParameter("@PONO",SqlDbType.Char,10)

            };

                param[0].Value = this.Session["DealerCode"].ToString();
                param[1].Value = mPONo;



                myFunc.ExecuteSP("Sp_2W_GetPO_ForDC", param, ref dr);
                dtParts.Load(dr);
                //ds = (DataSet)ViewState["ObjPOds"];
                dt = (DataTable)this.Session["DCTable"];
                dt.Rows.Clear();
                if (dtParts.Rows.Count != 0)
                {
                    foreach (DataRow dRow in dtParts.Rows)
                    {
                        SqlParameter[] paramPO_Parts ={
                        new SqlParameter ("@PONO",SqlDbType.Char,10),
                        new SqlParameter("@DealerCode",SqlDbType.Char,10),
                        new SqlParameter("@ItemCode",SqlDbType.Char,10)
                        };

                        paramPO_Parts[0].Value = mPONo;
                        paramPO_Parts[1].Value = this.Session["DealerCode"].ToString();
                        paramPO_Parts[2].Value = dRow["ItemCode"].ToString();
                        dsParts = myFunc.FillDataSet("SP_Spare_Get_PartPOInfo_New", paramPO_Parts);

                        DataRow row = dt.NewRow();
                        row["Sno"] = dt.Rows.Count + 1;

                        row["PartItemNo"] = dsParts.Tables[0].Rows[0]["PartItemNo"].ToString();
                        row["PartItemName"] = dsParts.Tables[0].Rows[0]["ItemDesc"].ToString();
                        row["unit"] = dsParts.Tables[0].Rows[0]["Unit"].ToString();
                        row["POQuantity"] = dsParts.Tables[0].Rows[0]["POQty"].ToString();
                        row["BalanceQuantity"] = dsParts.Tables[0].Rows[0]["BalanceQty"].ToString();
                        row["DCQuantity"] = dsParts.Tables[0].Rows[0]["BalanceQty"].ToString();                        

                        //row["Selected"] = true;

                        dt.Rows.Add(row);

                    }

                }
                Session["DCTable"] = dt;
                gv_DC.DataSource = dt;
                gv_DC.DataSourceID = null;
                gv_DC.DataBind();

            }
            catch (Exception ex)
            {
                //ObjGeneral.UserMsg(LbErr, Color.Red, "Error: " + ex.Message, txtDCNo);
            }

        }

        protected void ddlPONo_SelectedIndexChanged(object sender, EventArgs e)
        {

            string PONo = "";
            if (ddlPONo.SelectedValue == "")
            {
                PONo = lblPONo.Text;
            }
            else
            {
                PONo = ddlPONo.SelectedValue;
            }

            LoadPOInfo(PONo);

            LoadGrid(ddlDCno.SelectedValue, PONo, this.Session["DealerCode"].ToString());
            //GridViewDC.Selection.SelectAll();

            ddlDCno.Enabled = false;
            ddlPONo.Enabled = false;
            btnSave.Enabled = true;
            txtStatus.Text = "New";
        }

        ///
        private void LoadPOInfo(string PONo)
        {
            string mPONo = ddlPONo.SelectedValue;
            if (mPONo == "" || mPONo == "0")
            {
                mPONo = this.Session["PONumber"].ToString();
            }
            DataTable dt = new DataTable();
            string sql = "SP_PO_FOr_OrderConfrm '"+ Session["DealerCode"].ToString()+"','" + mPONo + "'";
            dt = myFunc.GetData(sql);
            if (dt.Rows.Count == 0)
            {
                return;
            }
            //dpPODate.Date = Convert.ToDateTime(dt.Rows[0]["PODate"].ToString());
            txtDCDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtVendor.Text = dt.Rows[0]["VendorCode"].ToString();
            txtVendorDesc.Text = dt.Rows[0]["VendorDesc"].ToString();
            //txtVendorDCn0.Text = dt.Rows[0]["VendorDCNo"].ToString();
            ///Commented by Akram
            //dpDCDate.Date = Convert.ToDateTime( myFunc.GetServerTime());
            ///Ended
            dt.Dispose();
        }


        protected void gv_DC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && !(gv_DC.EditIndex == e.Row.RowIndex))
                {                    
                    
                }

                if (e.Row.RowType == DataControlRowType.DataRow && (gv_DC.EditIndex == e.Row.RowIndex))
                {
                    //btn_remove.Enabled = false;
                   
                    TextBox txtDCQuantity = (TextBox)e.Row.FindControl("txtDCQuantity");  
                   
                    txtDCQuantity.Text = Session["lblDCQuantity"].ToString();
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

        // DATABOUND FUNCTION
        protected void gv_DC_DataBound(object sender, EventArgs e)
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
        protected void gv_DC_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;

                DataTable dtEditing = new DataTable();

                Label lblDCQuantity = (Label)gv_DC.Rows[e.NewEditIndex].FindControl("lblDCQuantity");
                Label lblBalQuantity = (Label)gv_DC.Rows[e.NewEditIndex].FindControl("lblBalQuantity");

                Session["lblDCQuantity"] = lblDCQuantity.Text;
                Session["lblBalQuantity"] = lblBalQuantity.Text;

                gv_DC.EditIndex = e.NewEditIndex;

                dtEditing = (DataTable)Session["DCTable"];
                Session["CityDS"] = dtEditing;
                gv_DC.DataSource = dtEditing;

                gv_DC.DataBind();
            }

            catch (Exception ex)
            {

                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;

            }

        }

        // ROW CANCEL EDIT
        protected void gv_DC_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DataTable dt = new DataTable();

            gv_DC.EditIndex = -1;

            dt = (DataTable)Session["DCTable"];

            gv_DC.DataSource = dt;

            gv_DC.DataBind();

            btnClear.Enabled = true;
            btnSave.Enabled = true;
            btnDelete.Enabled = true;

        }
        
        // FOR GRIDVIEW ROW UPDATING
        protected void gv_DC_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataTable dt_update = new DataTable();

                Label lblPartItemNo = (Label)gv_DC.Rows[e.RowIndex].FindControl("lblPartItemNo");
                Label lblBalQuantity = (Label)gv_DC.Rows[e.RowIndex].FindControl("lblBalQuantity");
                
                TextBox txtDCQuantity = (TextBox)gv_DC.Rows[e.RowIndex].FindControl("txtDCQuantity");     // GridView TextBox Field

                // Check value in GridView TextBOX
                int qty = Convert.ToInt32(txtDCQuantity.Text.Trim());
                int blncqty = Convert.ToInt32(lblBalQuantity.Text.Trim());
                if (txtDCQuantity.Text.Trim() == string.Empty || qty > blncqty)
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = Color.Red;
                    lblMessage.Text = "Quantity can not be null or greater than balance quantity.";
                    return;
                }

                dt_update = (DataTable)Session["DCTable"];

                //Uzair
                //DataRow dr =  ds_update.Tables[0].Rows[0];
                // Check in GridView

                foreach (DataRow dr in dt_update.Rows)
                {
                    if (dr["PartItemNo"].ToString() == lblPartItemNo.Text)
                    {
                        dr["DCQuantity"] = txtDCQuantity.Text.Trim();

                    }
                }

                gv_DC.EditIndex = -1;

                gv_DC.DataSource = dt_update;
                gv_DC.DataBind();

                Session["DCTable"] = dt_update;

                btnClear.Enabled = true;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
            }
            catch (Exception ex)
            {
                //ObjTrans.RollBackTransaction(ref Trans);
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }
        }

        public void btnSave_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2542", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }

            if (Convert.ToDateTime(myFunc.SaveDate(txtDCDate.Text)).Date > Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd")).Date)
            {            
                myFunc.UserMsg(lblMessage, Color.Red, "DC Date should not greater then current date.");
                return;
            }

            if (Convert.ToDateTime(myFunc.SaveDate(txtDCDate.Text)).Date < Convert.ToDateTime(myFunc.SaveDate(txtPODate.Text)).Date)
            {
                myFunc.UserMsg(lblMessage, Color.Red, "DC Date should not less then PO Date.");
                return;
            }
            if (txtVendorDCn0.Text == "")
            {
                myFunc.UserMsg(lblMessage, Color.Red, "Please enter vendor DC Number.");
                return;
            }

            if (gv_DC.Rows.Count == 0)
            {
                lblMessage.ForeColor = Color.Red;
                lblMessage.Text = "Please Select PO Number.";
                return;
            }

            string newDCNo = "";

            if (ddlDCno.SelectedValue == "0")
            {
                //newDCNo = myFunc.GetNewDCNumber(this.Session["DealerCode"].ToString());
                newDCNo = myFunc.AutoGen("DCMaster", "DCNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
            }
            else
            {
                if (ddlDCno.SelectedValue != "")
                {
                    if (!sec.UserRight("2542", "003"))
                    {
                        Response.Redirect("~/Test.aspx");
                    }
                    newDCNo = ddlDCno.SelectedValue;
                }
                else
                {
                    newDCNo = this.Session["DCNumber"].ToString();
                }
            }
            string sql = "";
            if (myFunc.isDCExist(newDCNo,Session["DealerCode"].ToString()))
            {

               
                sql = "UPDATE DCMaster SET VendorDCNo='" + txtVendorDCn0.Text + "',Remarks='" + txtRemarks.Text + "' where DCNo='" + newDCNo + "' and DealerCode = '"+Session["DealerCode"].ToString()+"'";
            }
            else
            {

                sql = "INSERT INTO DCMaster (DealerCode,DCNo,DCDate,PONo,VendorDCNo,VendorCode,Remarks,DelFlag,UpdUser,UpdDate,UpdTime)";
                sql += " VALUES ('" + this.Session["DealerCode"].ToString() + "','" + newDCNo + "','" + myFunc.SaveDate(txtDCDate.Text) + "','" + ddlPONo.SelectedValue + "','";
                sql += txtVendorDCn0.Text.Trim() + "','" + txtVendor.Text + "','" + txtRemarks.Text.Trim() + "','N','" + this.Session["UserID"].ToString() + "',";
                sql += "CURRENT_TIMESTAMP,CURRENT_TIMESTAMP)";
            }
            try
            {
                int slct = 0;

                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    myFunc.ExecuteQuery_NonQuery(sql, Trans);
                    // save detail

                    RemoveQTYFromPO(newDCNo, "-");
                    sql = "delete from DCDetail where DCNo ='" + newDCNo + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
                    myFunc.ExecuteQuery_NonQuery(sql, Trans);

                    DataTable dt = (DataTable)Session["DCTable"];
                    string ItemCode = "";

                    int i = 0;

                    foreach (GridViewRow row in gv_DC.Rows)
                    {
                        CheckBox chkSelect = (CheckBox)row.FindControl("cbSelect");

                        if (chkSelect.Checked)
                        {
                            ItemCode = myFunc.GetItemCodeByPartItemNo(dt.Rows[i]["PartItemNo"].ToString(),Session["DealerCode"].ToString());
                            sql = "INSERT INTO DCDetail (DealerCode, DCNo, ItemCode, PONo, POQty, BalQty, DCQty)";
                            sql += " VALUES('" + this.Session["DealerCode"].ToString() + "','" + newDCNo + "','" + ItemCode + "','" + ddlPONo.SelectedValue + "','";
                            sql += dt.Rows[i]["POQuantity"].ToString() + "','" + dt.Rows[i]["BalanceQuantity"].ToString() + "','" + dt.Rows[i]["DCQuantity"].ToString() + "')";
                            myFunc.ExecuteQuery_NonQuery(sql, Trans);
                            slct++;
                        }
                        i++;
                    }
                }
                else
                {
                    ObjTrans.RollBackTransaction(ref Trans);
                }
                //Commented By Uzair
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    slct = dt.Rows[i]["Selected"].ToString();
                //    if (slct == "True")
                //    {
                //        ItemCode = myFunc.GetItemCodeByPartItemNo(dt.Rows[i]["PartItemNo"].ToString());
                //        sql = "INSERT INTO DCDetail (DealerCode, DCNo, ItemCode, PONo, POQty, BalQty, DCQty)";
                //        sql += " VALUES('" + this.Session["DealerCode"].ToString() + "','" + newDCNo + "','" + ItemCode + "','" + ddlPONo.SelectedValue + "','";
                //        sql += dt.Rows[i]["POQuantity"].ToString() + "','" + dt.Rows[i]["BalanceQuantity"].ToString() + "','" + dt.Rows[i]["DCQuantity"].ToString() + "')";
                //        myFunc.GetData(sql);
                //    }
                //}
                if(slct > 0)
                {
                    ObjTrans.CommittTransaction(ref Trans);
                    RemoveQTYFromPO(newDCNo, "+");
                    lblMessage.ForeColor = Color.Green;
                    lblMessage.Text = newDCNo + " has been saved";
                    ClearAll();
                    lOADddl();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);
                }
                else
                {
                    ObjTrans.RollBackTransaction(ref Trans);
                    lblMessage.ForeColor = Color.Red;
                    lblMessage.Text = "At least one row should be selected";
                }
                
                
            }
            catch (Exception)
            {
                ObjTrans.RollBackTransaction(ref Trans);
            }
            //ddDCNumber.Text = newDCNo;

        }

        protected void BtnRemove_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsdelete = new DataSet();

            DataTable dt = new DataTable();

            try
            {
                dt = (DataTable)Session["DCTable"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;

                dt.Rows[rowGvIndex].Delete();
                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                gv_DC.DataSource = dt;
                gv_DC.DataBind();

                Session["DCTable"] = dt;
                
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }

        }

        private void RemoveQTYFromPO(string DCNumber, string UpdateSign)
        {
            string sql = "select * from DCDetail where DealerCode='" + Session["DealerCode"].ToString() + "' And DCNo='" + DCNumber + "'";
            DataTable dt = myFunc.GetData(sql);
            DataTable dt2 = new DataTable();
            if (dt.Rows.Count == 0)
            {
                return;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt2 = myFunc.GetData("update PODetail SET RecQty = RecQty " + UpdateSign + dt.Rows[i]["DCQty"].ToString() + ",DCQty=DCQty " + UpdateSign + dt.Rows[i]["DCQty"].ToString() + " where DealerCode='" + Session["DealerCode"].ToString() + "' And PONo='" + dt.Rows[i]["PONo"].ToString() + "' and ItemCode='" + dt.Rows[i]["ItemCode"].ToString() + "'");
            }

            dt.Dispose();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {


            ClearAll();

            lblMessage.Text = "";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);

        }

        private void ClearAll()
        {
            this.Session["PONumber"] = "";
            this.Session["DCNumber"] = "";
            btnSave.Enabled = false;
            //btnDelete.Enabled = false;
            ddlDCno.SelectedIndex = 0;
            ddlPONo.SelectedIndex = 0;
            ddlPONo.Enabled = true;
            ddlPONo.Visible = true;
            lblPONo.Visible = false;
            lblPONo.Text = "";

            txtPODate.Text = DateTime.Now.ToString("dd-MM-yyyy");

            txtDCDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            //ddDCNumber.Text = "";
            ddlDCno.Enabled = true;

            txtStatus.Text = "";
            txtVendor.Text = "";
            txtVendorDesc.Text = "";
            txtVendorDCn0.Text = "";



            txtRemarks.Text = "";
            LoadGrid("", "", "");



            string dcode = this.Session["DealerCode"].ToString();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2542", "004"))
            {
                Response.Redirect("~/Test.aspx");
            }
            DataTable dt = new DataTable();
            DataSet dsdelete = new DataSet();

            if (ddlDCno.SelectedIndex == 0)
            {
                lblMessage.Text = "Select DC no first";
                return;
            }

            if (sysFunc.CodeExists("GRNMaster", "DCNo", ddlDCno.SelectedValue, Session["DealerCode"].ToString(), ref dsdelete))
            {
                lblMessage.Text = "Cannot delete , GRN of this DC No has already made";
                return;
            }

            RemoveQTYFromPO(this.Session["DCNumber"].ToString(), "-");

            string sql = "delete from DCDetail where DealerCode = '" + this.Session["DealerCode"].ToString() + "' and DCNo='" + this.Session["DCNumber"].ToString() + "'";
            dt = myFunc.GetData(sql);

            sql = "update DCMaster set DelFlag = 'Y' where DealerCode = '" + this.Session["DealerCode"].ToString() + "' and DCNo='" + this.Session["DCNumber"].ToString() + "'";
            dt = myFunc.GetData(sql);

            lblMessage.Text = "Delivery Challan deleted";

            ClearAll();
            lOADddl();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Deletealert()", true);
        }

        protected void cbCheck_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox Chk = sender as CheckBox;
            TableCell tc = Chk.Parent as TableCell;
            GridViewRow gvr = tc.Parent as GridViewRow;
            CheckBox chkSelect = (CheckBox)gvr.Cells[0].FindControl("cbSelect");
            Label lblPartItemNo = (Label)gvr.Cells[1].FindControl("lblPartItemNo");
            string sno = "";
            if (chkSelect.Checked)
            {
                sno = lblPartItemNo.Text;
                chkSelect.Checked = true;
            }
            else
            {
                chkSelect.Checked = false;
            }
            //sno = GridViewDC.GetRowValues(gvr.VisibleIndex, "PartItemNo").ToString();
            UpdateGrid(sno, chkSelect.Checked);

            

        }

        private void UpdateGrid(string PartItemNo, bool ChkVal)
        {
            DataTable dataTable = (DataTable)Session["DCTable"];
            
            DataRow row = dataTable.Rows.Find(PartItemNo);

            row["Selected"] = ChkVal;

            dataTable.AcceptChanges();

            Session["DCTable"] = dataTable;
            gv_DC.DataSource = dataTable;
            gv_DC.DataBind();
        }

        protected void imgDCLookup_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 20; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_DCNumber(imgDCLookup, ViewState["lookupid"].ToString(), "", "../../../","DC");

            btnValue = 1;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }

        protected void imgPOLookup_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 11; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_PONumber(imgPOLookup, ViewState["lookupid"].ToString(), "", "../../../","DC");

            btnValue = 2;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }
        protected void SelectedPartDetail(string item)
        {
            try
            {
               lOADddl();
                if (btnValue == 2)
                {
                    
                    ddlPONo.SelectedValue = item.ToString();
                    ddlPONo_SelectedIndexChanged(null, null);

                }
                else
                {
                    ddlDCno.SelectedValue = item;
                    ddlDCno_SelectedIndexChanged(null, null);
                }

            }
            catch (Exception ex)
            {
                sysFunc.UserMsg(lblMessage, Color.Red, ex.Message);
            }
        }
    }
}