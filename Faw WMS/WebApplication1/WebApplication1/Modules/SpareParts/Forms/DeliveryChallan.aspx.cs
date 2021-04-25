using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
//using DevExpress.Web.ASPxGridView;
//using DevExpress.Web.ASPxClasses.Internal;
//using DevExpress.Web.ASPxEditors;
using System.Collections;
using System.Drawing;

namespace DXBMS.Modules.Transactions
{
    public partial class DeliveryChallan : System.Web.UI.Page
    {
        SysFunctions myFunc = new SysFunctions();
        DataSet DCds = null;
        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!IsPostBack)
            {
                btnClear_Click(btnClear, EventArgs.Empty);
                ResetDCItemsDataSet();
                LoadGrid(ddDCNumber.Text, ddPONumber.Text, this.Session["DealerCode"].ToString());

                
            }
            ddPONumber.Text = this.Session["PONumber"].ToString();
            if (ddDCNumber.Text == "")
            {
                ddDCNumber.Text = this.Session["DCNumber"].ToString();
            }
            //GridViewDC.SettingsEditing.BatchEditSettings.EditMode = (GridViewBatchEditMode)Enum.Parse(typeof(GridViewBatchEditMode), "Cell", true);
            //GridViewDC.SettingsEditing.BatchEditSettings.StartEditAction = (GridViewBatchStartEditAction)Enum.Parse(typeof(GridViewBatchStartEditAction), "Click", true);

            if (GridViewDC.IsEditing && Session["column"] != null)
            {
                string fieldName = Convert.ToString(Session["column"]);
                for (int i = 0; i < GridViewDC.Columns.Count; i++)
                {
                    GridViewDataColumn column = (GridViewDataColumn)GridViewDC.Columns[i];
                    if (column != null && column.FieldName != fieldName)
                        column.EditItemTemplate = new MyTemplate();
                }
            }

           
        }

        protected void btnPopOk_Click(object sender, EventArgs e)
        {
            PopupControlMSG.ShowOnPageLoad = false;
        }

        protected void btnPopCancel_Click(object sender, EventArgs e)
        {
            PopupControlMSG.ShowOnPageLoad = false;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            this.Session["PONumber"] = "";
            this.Session["DCNumber"] = "";
            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            ddPONumber.Text = "";
            ddPONumber.Enabled = true;
            ddPONumber.Visible = true;
            lblPONo.Visible = false;
            lblPONo.Text = "";

            dpPODate.Date = DateTime.Today;

            dpDCDate.Date = Convert.ToDateTime(myFunc.GetServerTime());
            ddDCNumber.Text = "";
            ddDCNumber.Enabled = true;

            lblDCStatus.Text = "";
            txtVendorCode.Text = "";
            txtVendorName.Text = "";
            txtVendorDCNo.Text = "";



            txtRemarks.Text = "";
            ResetDCItemsDataSet();
            LoadGrid("", "", "");

            string dcode = this.Session["DealerCode"].ToString();
            SqlDataSourceDCLookup.SelectCommand = "select  a.DCNo,  convert(varchar(12),a.DCDate,103) as DCDate , a.PONo,  b.VendorDesc,a.VendorDCNo  from DCMaster a,Vendor b where  a.VendorCode = b.VendorCode and a.DealerCode in ('" + dcode + "','AAAAA') and a.DelFlag = 'N' and a.VendorCode = b.VendorCode and a.DCNo Not IN  (Select DCNo from GRNMaster Where DealerCode in ('" + dcode + "','AAAAA') and Delflag = 'N') ";
            ddDCNumber.DataBind();
            ddPONumber.DataBind();
            lblDCStatus.Text = "";
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

            ErrorMsg.Text = "";


        }
        private void LoadGrid(string DCNumber,string PONumber, string DealerCode)
        {
            DataTable dtParts = new DataTable();
            DataSet ds = new DataSet();
            DataSet dsParts = new DataSet();
            SqlDataReader dr = null;
            string mPONo="";
            string mDCNo = "";
            if (PONumber == "")
            {
                mPONo = this.Session["PONumber"].ToString();
            }
            else
            {
                mPONo = PONumber;
            }
            //if (DCNumber == "")
            //{
            //    mDCNo = this.Session["DCNumber"].ToString();
            //}
            //else
            //{
            //    mDCNo = DCNumber;
            //}
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
                ds = (DataSet)this.Session["DCDataSet"];
                ds.Tables["DCItems"].Rows.Clear();
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
                        dsParts = myFunc.FillDataSet("SP_Spare_Get_PartPOInfo", paramPO_Parts);

                        DataRow row = ds.Tables["DCItems"].NewRow();
                        row["Sno"] = ds.Tables["DCItems"].Rows.Count + 1;

                        row["PartItemNo"] =myFunc.GetStringValuesAgainstCodes("ItemCode", paramPO_Parts[2].Value.ToString(), "PartItemNo", "Item");
                        row["PartItemName"] = myFunc.GetStringValuesAgainstCodes("ItemCode", paramPO_Parts[2].Value.ToString(), "ItemDesc", "Item");
                        row["unit"] = myFunc.GetStringValuesAgainstCodes("UnitCode='" + dRow["UnitCode"].ToString() + "'", "unit", "UnitDesc");
                        row["POQuantity"] = dsParts.Tables[0].Rows[0]["POQty"].ToString();
                        row["BalanceQuantity"] = dsParts.Tables[0].Rows[0]["BalanceQty"].ToString();
                        if (DCNumber == "")
                        {
                            row["DCQuantity"] = dsParts.Tables[0].Rows[0]["BalanceQty"].ToString();
                        }
                        else
                        {
                            row["DCQuantity"] = myFunc.GetDCQty(DCNumber,dRow["ItemCode"].ToString());
                        }

                        row["Selected"] = true;

                        ds.Tables["DCItems"].Rows.Add(row);

                    }

                    
                    //if (GridViewDC.VisibleRowCount > 0)
                    //{
                    //    CheckBox chkSelect;
                    //    TextBox txtDCQty;
                    //    Label lblBalQty;
                    //    foreach (GridViewRow gvr in GridViewDC.Rows)
                    //    {
                    //        chkSelect = (CheckBox)gvr.Cells[0].FindControl("ChkSelect");
                    //        txtDCQty = (TextBox)gvr.Cells[6].FindControl("txtDCQuantity");
                    //        lblBalQty = (Label)gvr.Cells[5].FindControl("LbBalanceQuantity");
                    //        chkSelect.Checked = true;
                    //        txtDCQty.Text = lblBalQty.Text;
                    //        //chkSelect.Enabled = false;
                    //    }
                    //}
                    //txtVendorNo.Text = dsVendorInfo.Tables[0].Rows[0]["vendorCode"].ToString();
                    //txtVendorDesc.Text = dsVendorInfo.Tables[0].Rows[0]["vendorDesc"].ToString();
                }
                Session["DCDataSet"] = ds;
                GridViewDC.DataSource = ds.Tables["DCItems"];
                GridViewDC.DataSourceID = null;
                GridViewDC.DataBind();

            }
            catch (Exception ex)
            {
                //ObjGeneral.UserMsg(LbErr, Color.Red, "Error: " + ex.Message, txtDCNo);
            }

        }

        protected void btnPOSelect_Click(object sender, EventArgs e)
        {
            string PONo = "";
            if (ddPONumber.Text == "")
            {
                PONo = lblPONo.Text;
            }
            else
            {
                PONo = ddPONumber.Text;
            }

            LoadPOInfo(PONo);
            ResetDCItemsDataSet();
            LoadGrid(ddDCNumber.Text, PONo, this.Session["DealerCode"].ToString());
            GridViewDC.Selection.SelectAll();

            ddDCNumber.Enabled = false;
            ddPONumber.Enabled = false;
            btnSave.Enabled = true;
            lblDCStatus.Text = "New";
        }
        private void LoadPOInfo(string PONo)
        {
            string mPONo = ddPONumber.Text;
            if (mPONo == "")
            {
                mPONo = this.Session["PONumber"].ToString();
            }
            DataTable dt = new DataTable();
            string sql = "select PO.POno,po.PODate,po.VendorCode,v.VendorDesc, po.VendorDCNo from POMaster PO left outer join Vendor V on po.VendorCode = v.VendorCode where PO.PONo='" + mPONo + "'";
            dt = myFunc.GetData(sql);
            if (dt.Rows.Count == 0)
            {
                return;
            }
            dpPODate.Date = Convert.ToDateTime(dt.Rows[0]["PODate"].ToString());
            txtVendorCode.Text = dt.Rows[0]["VendorCode"].ToString();
            txtVendorName.Text = dt.Rows[0]["VendorDesc"].ToString();
            //txtVendorDCNo.Text = dt.Rows[0]["VendorDCNo"].ToString();
            ///Commented by Akram
            //dpDCDate.Date = Convert.ToDateTime( myFunc.GetServerTime());
            ///Ended
            dt.Dispose();
        }

        protected void ddPONumber_TextChanged(object sender, EventArgs e)
        {
            this.Session["PONumber"] = ddPONumber.Text;
        }

        protected void GridViewDC_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            
        }

        protected void ASPxCallback1_Callback(object source, DevExpress.Web.ASPxCallback.CallbackEventArgs e)
        {
            string[] parameters = e.Parameter.Split('|');

            string row = parameters[0].ToString();
            string itemno = parameters[1].ToString();
            decimal balqty =Convert.ToDecimal( parameters[2].ToString());
            decimal dcqty =Convert.ToDecimal( parameters[3].ToString());

            DataSet ds = (DataSet)this.Session["DCDataSet"];

            DataTable dt = ds.Tables["DCItems"];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["PartItemNo"].ToString() == itemno)
                {
                    if (dcqty > balqty)
                    {
                        dt.Rows[i]["DCQuantity"] = "0";
                    }
                    else
                    {
                        dt.Rows[i]["DCQuantity"] = dcqty.ToString();
                    }

                    dt.AcceptChanges();
                    break;
                }
            }

            this.Session["DCDataSet"] = ds;
            GridViewDC.DataSource = ds;
            GridViewDC.DataBind();


            
            //ASPxGridView myg = GridViewDC;

            //string balqty = myg.GetRowValues(Convert.ToInt16(row),"BalanceQuantity").ToString();
            
                //GridViewDC.GetRowValues(GridViewDC.FocusedRowIndex, "DCQuantity").ToString();
            
            //lblMSG.Text = "Error";
            //PopupControlMSG.ShowOnPageLoad = true;
            //GridViewDC.CancelEdit();
        }

        protected void GridViewDC_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            e.Editor.Enabled = !e.Column.ReadOnly;
            if (e.Editor.Enabled)
            {
                ((ASPxTextBox)e.Editor).ClientSideEvents.KeyPress = "function(s,e) {OnEditorKeyPress(s, e);}";
            }
            //if (ASPxGridView1.IsNewRowEditing) return;
            //if (e.Column.FieldName == "DCQuantity")
            //{
            //    e.Editor.MinValue = 0;
            //    e.Editor.MaxValue = 5;
            //}
        }

        protected void GridViewDC_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            
            //string a = "";
            //CancelEditing(e);
        }

        protected void CancelEditing(CancelEventArgs e)
        {
            e.Cancel = true;
            GridViewDC.CancelEdit();
        }

        protected void GridViewDC_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            e.Cell.Attributes.Add("onclick", "onCellClick(" + e.VisibleIndex + ", '" + e.DataColumn.FieldName + "')");
        }

        protected void GridViewDC_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            ASPxGridView gridView = (ASPxGridView)sender;
            gridView.UpdateEdit();
            string[] data = e.Parameters.Split(new char[] { '|' });
            gridView.FocusedRowIndex = Convert.ToInt32(data[0]);
            for (int i = 0; i < gridView.Columns.Count; i++)
            {
                GridViewDataColumn column = (GridViewDataColumn)gridView.Columns[i];
                if (column != null)
                    if (column.FieldName != data[1])
                        column.EditItemTemplate = new MyTemplate();
                    else
                    {
                        Session["column"] = column.FieldName;
                        column.EditItemTemplate = null;
                    }
            }
            gridView.StartEdit(Convert.ToInt32(data[0]));
        }

        protected void GridViewDC_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.InlineEdit)
            {
                string fieldName = Session["column"] == null ? "" : Session["column"].ToString();
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    GridViewDataColumn column = ((GridViewDataColumn)((ASPxGridView)sender).VisibleColumns[i]);
                    if (column.FieldName != fieldName)
                        e.Row.Cells[i].Attributes.Add("onclick", "onCellClick(" + e.VisibleIndex + ", '" + column.FieldName + "')");
                }
            }
        }

        protected void GridViewDC_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {
            ASPxGridView gridView = (ASPxGridView)sender;
            for (int i = 0; i < gridView.Columns.Count; i++)
                if (gridView.Columns[i] is GridViewDataColumn)
                {
                    ((GridViewDataColumn)gridView.Columns[i]).EditItemTemplate = null;
                    Session["column"] = null;
                }
        }

        protected void GridViewDC_BatchUpdate1(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            GridViewDC.CancelEdit();
        }

        protected void GridViewDC_RowUpdating1(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            DataSet ds = (DataSet)Session["DCDataSet"];
            decimal dqty = Convert.ToDecimal( e.NewValues["DCQuantity"].ToString());
            decimal bqty = Convert.ToDecimal( e.NewValues["BalanceQuantity"].ToString());

            if (dqty > bqty || dqty<0)
            {
                ErrorMsg.Text = "Check quantity, you can't enter more than balance qty";
                ErrorMsg.Visible = true;
                e.Cancel = true;
                GridViewDC.DataSource = ds;
                GridViewDC.DataBind();
                return;
            }

            
            ASPxGridView gridView = (ASPxGridView)sender;
            DataTable dataTable = ds.Tables[0];
            DataRow row = dataTable.Rows.Find(e.Keys[0]);
            IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
                row[enumerator.Key.ToString()] = enumerator.Value;
            gridView.CancelEdit();
            e.Cancel = true;
            Session["DCDataSet"]=ds;
            GridViewDC.DataSource = ds;
            GridViewDC.DataBind();
            return;
            
            ////string test = GridViewDC.GetRowValues(GridViewDC.FocusedRowIndex,"DCQuantity").ToString();
            //GridViewDC.CancelEdit();
            //e.Cancel = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (Convert.ToDateTime(myFunc.SaveDate(dpDCDate.Text)).Date > Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd")).Date)
            {
                lblMSG.Text = "DC Date should not greater then current date.";
                PopupControlMSG.ShowOnPageLoad = true;
                return;
                //myFunc.UserMsg(lblMSG, Color.Red, "Date should not greater then current date");

            }

            if (Convert.ToDateTime(myFunc.SaveDate(dpDCDate.Text)).Date < Convert.ToDateTime(myFunc.SaveDate(dpPODate.Text)).Date)
            {
                lblMSG.Text = "DC Date should not less then PO Date";
                PopupControlMSG.ShowOnPageLoad = true;
                return;
            }
            if (txtVendorDCNo.Text == "")
            {
                lblMSG.Text = "Please enter vendor DC Number";
                //PopupControlMSG.ShowOnPageLoad = true;
                return;
            }

            string newDCNo = "";

            if (ddDCNumber.Text == "" && this.Session["DCNumber"].ToString()=="")
            {
                newDCNo = myFunc.GetNewDCNumber(this.Session["DealerCode"].ToString());
            }
            else
            {
                if (ddDCNumber.Text != "")
                {
                    newDCNo = ddDCNumber.Text;
                }
                else
                {
                    newDCNo = this.Session["DCNumber"].ToString();
                }
            }
            string sql = "";
            if (myFunc.isDCExist(newDCNo))
            {
                sql = "UPDATE DCMASTER SET VendorDCNo='" + txtVendorDCNo.Text + "',Remarks='" + txtRemarks.Text + "' where DCNo='" + newDCNo + "'";
            }
            else
            {

                sql = "INSERT INTO DCMASTER (DealerCode,DCNo,DCDate,PONo,VendorDCNo,VendorCode,Remarks,DelFlag,UpdUser,UpdDate,UpdTime)";
                sql += " VALUES ('" + this.Session["DealerCode"].ToString() + "','" + newDCNo + "','" + dpDCDate.Date.ToString("yyyy/MM/dd") + "','" + ddPONumber.Text + "','";
                sql += txtVendorDCNo.Text.Trim() + "','" + txtVendorCode.Text + "','" + txtRemarks.Text.Trim() + "','N','" + this.Session["UserID"].ToString() + "',";
                sql += "CURRENT_TIMESTAMP,CURRENT_TIMESTAMP)";
            }
            DataTable dt =  myFunc.GetData(sql);
             // save detail

            RemoveQTYFromPO(newDCNo,"-");
           sql = "delete from DCDetail where DCNo='" + newDCNo + "'";
           myFunc.GetData(sql);
           

            DataSet ds = (DataSet)Session["DCDataSet"];

            dt = ds.Tables[0];
            string ItemCode="";
            string slct = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                slct = dt.Rows[i]["Selected"].ToString();
                if (slct == "True")
                {
                    ItemCode = myFunc.GetItemCodeByPartItemNo(dt.Rows[i]["PartItemNo"].ToString());
                    sql = "INSERT INTO DCDetail (DealerCode, DCNo, ItemCode, PONo, POQty, BalQty, DCQty)";
                    sql += " VALUES('" + this.Session["DealerCode"].ToString() + "','" + newDCNo + "','" + ItemCode + "','" + ddPONumber.Text.Trim() + "','";
                    sql += dt.Rows[i]["POQuantity"].ToString() + "','" + dt.Rows[i]["BalanceQuantity"].ToString() + "','" + dt.Rows[i]["DCQuantity"].ToString() + "')";
                    myFunc.GetData(sql);
                }
            }
            RemoveQTYFromPO(newDCNo, "+");
            lblMSG.Text = newDCNo + " has been saved";
            PopupControlMSG.ShowOnPageLoad = true;
            btnClear_Click(btnClear, EventArgs.Empty);

            //ddDCNumber.Text = newDCNo;
            
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

        protected void btnDCSelect_Click(object sender, EventArgs e)
        {
            string sql = "select * from DCMaster where DealerCode='"+ Session["DealerCode"].ToString () +"' And DCNo='" + ddDCNumber.Text + "'";
            DataTable dt = myFunc.GetData(sql);
            if (dt.Rows.Count > 0)
            {
                dpDCDate.Date = Convert.ToDateTime(dt.Rows[0]["DCDate"].ToString());
                ddPONumber.Text = dt.Rows[0]["PONo"].ToString();
                ddPONumber.Visible = false;
                lblPONo.Visible = true;
                lblPONo.Text = dt.Rows[0]["PONo"].ToString();

                this.Session["PONumber"] = dt.Rows[0]["PONo"].ToString();
                txtVendorDCNo.Text = dt.Rows[0]["VendorDCNo"].ToString();
                txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();

                this.Session["DCNumber"] = ddDCNumber.Text;
                LoadPOInfo(lblPONo.Text);
                //btnPOSelect_Click(btnPOSelect, EventArgs.Empty);

                // loading details of DC
                DataSet ds = new DataSet();
                ds = (DataSet)this.Session["DCDataSet"];
                ds.Tables["DCItems"].Rows.Clear();

                dt = myFunc.GetData("select * from DCDetail where DealerCode='" + Session["DealerCode"].ToString() + "' And DCNo='" + ddDCNumber.Text + "'");

                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow dRow in dt.Rows)
                    {


                        DataRow row = ds.Tables["DCItems"].NewRow();
                        row["Sno"] = ds.Tables["DCItems"].Rows.Count + 1;

                        row["PartItemNo"] = myFunc.GetPartItemNoByItemCode(dRow["ItemCode"].ToString());
                        row["PartItemName"] = myFunc.GetItemDescByItemCode(dRow["ItemCode"].ToString());
                        row["unit"] = myFunc.GetUnitDescByPartItemNo(row["PartItemNo"].ToString());
                        row["POQuantity"] = dRow["POQty"].ToString();
                        row["BalanceQuantity"] = dRow["BalQty"].ToString();
                        row["DCQuantity"] = dRow["DCQty"].ToString();
                        row["Selected"] = true;

                        ds.Tables["DCItems"].Rows.Add(row);

                    }
                }

                Session["DCDataSet"] = ds;
                GridViewDC.DataSource = ds;
                GridViewDC.DataBind();

                btnDelete.Enabled = true;
                lblDCStatus.Text = "Edited";

            }
            else
            {
                lblMSG.Text= "DC number not found";
                return;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            RemoveQTYFromPO(this.Session["DCNumber"].ToString(), "-");

            string sql = "delete from DCDetail where DealerCode = '" + this.Session["DealerCode"].ToString() + "' and DCNo='" + this.Session["DCNumber"].ToString() + "'";
            dt = myFunc.GetData(sql);

            sql = "delete from DCMaster where DealerCode = '" + this.Session["DealerCode"].ToString() + "' and DCNo='" + this.Session["DCNumber"].ToString() + "'";
            dt = myFunc.GetData(sql);

            ddPONumber.GridView.DataBind();
            lblMSG.Text = "Delivery Challan deleted";
            //lblRedirect.Text = "Delete";
            PopupControlMSG.ShowOnPageLoad = true;
            btnClear_Click(btnClear, EventArgs.Empty);
        }

        protected void GridViewDC_SelectionChanged(object sender, EventArgs e)
        {
            string t= e.ToString();
        }

        protected void cbCheck_Load(object sender, EventArgs e)
        {
            ASPxCheckBox cb = sender as ASPxCheckBox;

            GridViewDataItemTemplateContainer container = cb.NamingContainer as GridViewDataItemTemplateContainer;
            cb.ClientInstanceName = String.Format("cbCheck{0}", container.VisibleIndex);
            cb.Checked = GridViewDC.Selection.IsRowSelected(container.VisibleIndex);

            cb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ grid.SelectRowOnPage ({0}, s.GetChecked()); }}", container.VisibleIndex);
        }

        protected void cbCheck_CheckedChanged(object sender, EventArgs e)
        {
            ASPxCheckBox cb = sender as ASPxCheckBox;
            GridViewDataItemTemplateContainer container = cb.NamingContainer as GridViewDataItemTemplateContainer;
            cb.ClientInstanceName = String.Format("cbCheck{0}", container.VisibleIndex);
            //cb.Checked = GridViewDC.Selection.IsRowSelected(container.VisibleIndex);
            string sno="";
            if (cb.Checked)
            {
                
            }
            sno = GridViewDC.GetRowValues(container.VisibleIndex, "PartItemNo").ToString();
            UpdateGrid(sno,cb.Checked);
            
            string t = ""; //GridViewDC.GetRowValues(GridViewDC.FocusedRowIndex, "Selected").ToString();


        }

        private void UpdateGrid(string PartItemNo, bool ChkVal)
        {
            DataSet ds = (DataSet)Session["DCDataSet"];
            ASPxGridView gridView = GridViewDC;
            DataTable dataTable = ds.Tables[0];
            DataRow row = dataTable.Rows.Find(PartItemNo);

            row["Selected"] = ChkVal;

            ds.Tables[0].AcceptChanges();

            //IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            //enumerator.Reset();
            //while (enumerator.MoveNext())
            //    row[enumerator.Key.ToString()] = enumerator.Value;
            
            Session["DCDataSet"] = ds;
            GridViewDC.DataSource = ds;
            GridViewDC.DataBind();
        }
    }
}