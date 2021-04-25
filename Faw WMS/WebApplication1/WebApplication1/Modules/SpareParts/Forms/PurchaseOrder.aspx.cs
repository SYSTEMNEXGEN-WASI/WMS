using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DXBMS.App_Code.DAL;
using System.Data;
using System.Threading;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data.SqlClient;
using DXBMS.Data;

namespace DXBMS.Modules.Transactions
{
    public partial class PurchaseOrder : System.Web.UI.Page
    {
        SysFunctions myFunc = new SysFunctions();
        DataSet OPds = null;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/Main/login.aspx");

            }
            if (!IsPostBack)
            {
                btnClear_Click(btnClear, EventArgs.Empty);
                
                //ResetPOItemsDataSet();
                
                //LoadGrid(ddPONumber.Text,this.Session["DealerCode"].ToString());
            }
            //this.Session["DealerCode"] = Session["DealerCode"].ToString();
            ddPONumber.Text = this.Session["PONumber"].ToString();
            
        }

        private void ResetPOItemsDataSet()
        {
            OPds = new DataSet();
            DataTable POItems = new DataTable();
            POItems.Columns.Add("SNo", typeof(int));
            POItems.Columns.Add("ItemCode", typeof(string));
            POItems.Columns.Add("PartItemNo", typeof(string));
            POItems.Columns.Add("PartItemName", typeof(string));
            POItems.Columns.Add("Unit", typeof(string));
            POItems.Columns.Add("Quantity", typeof(decimal));
            POItems.Columns.Add("Rate", typeof(decimal));
            POItems.Columns.Add("Liter", typeof(decimal));
            POItems.Columns.Add("DiscountPerc", typeof(decimal));
            POItems.Columns.Add("Discount", typeof(decimal));
            POItems.Columns.Add("Amount", typeof(decimal));
            POItems.Columns.Add("GST", typeof(decimal));
            POItems.Columns.Add("GSTAmount", typeof(decimal));
            POItems.Columns.Add("PFGST", typeof(decimal));
            POItems.Columns.Add("PFGSTAmt", typeof(decimal));
            POItems.Columns.Add("ExTaxPerc", typeof(decimal));
            POItems.Columns.Add("ExTaxAmt", typeof(decimal));
            POItems.Columns.Add("WHTAge", typeof(decimal));
            POItems.Columns.Add("WHTAmount", typeof(decimal));
            POItems.Columns.Add("NetAmount", typeof(decimal));

            POItems.PrimaryKey = new DataColumn[] { POItems.Columns["PartItemNo"] };


            OPds.Tables.AddRange(new DataTable[] { POItems });
            Session["PODataSet"] = OPds;
        }

        private void LoadGrid(string PONumber, string DealerCode)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            //OPds = (DataSet)Session["DataSet"];
            OPds = (DataSet)this.Session["PODataSet"];
            string sql = "exec SP_Spare_Get_ItemDetails '"+PONumber+"','"+DealerCode+"'";

            dt = myFunc.GetData(sql);

            if (dt.Rows.Count > 0)
            {

                OPds.Tables[0].Clear();

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    DataRow row = OPds.Tables[0].NewRow();
                    row["Sno"] = OPds.Tables[0].Rows.Count + 1;
                    row["PartItemNo"] = dt.Rows[i]["PartItemNo"].ToString();
                    row["PartItemName"] = dt.Rows[i]["PartItemName"].ToString();
                    row["Unit"] = dt.Rows[i]["Unit"].ToString();
                    row["Quantity"] = dt.Rows[i]["Quantity"].ToString();
                    row["Rate"] = dt.Rows[i]["Rate"].ToString();
                    row["Amount"] = dt.Rows[i]["Amount"].ToString();
                    row["GST"] = dt.Rows[i]["GST"].ToString();
                    row["GSTAmount"] = dt.Rows[i]["GSTAmount"].ToString();
                    row["NetAmount"] = dt.Rows[i]["NetAmount"].ToString();
                    row["PFGST"] = dt.Rows[i]["PFGSTPERC"].ToString();
                    row["PFGSTAmt"] = dt.Rows[i]["PFGSTAmt"].ToString();
                    row["ExTaxPerc"] = dt.Rows[i]["ExTaxPerc"].ToString();
                    row["ExTaxAmt"] = dt.Rows[i]["ExTaxAmt"].ToString();
                    row["Discount"] = dt.Rows[i]["Discount"].ToString();
                    row["Liter"] = dt.Rows[i]["Liter"].ToString();
                    row["DiscountPerc"] = dt.Rows[i]["DiscountPerc"].ToString();
                    OPds.Tables[0].Rows.Add(row);
                }
            }

            
            GridViewItems.DataSourceID = null;
            if (OPds != null)
            {
                GridViewItems.DataSource = OPds.Tables[0];
                GridViewItems.DataBind();
               
            }
            else
            {
                GridViewItems.DataSource = null;
                GridViewItems.DataBind();
            }
            this.Session["PODataSet"] = OPds;
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            //Check for validation
            if (dpPODate.Text == "" || ddVendorName.Text=="" || ddDeliveryType.Text=="" || ddPartsSource.Text=="" )
            {
                lblMSG.Text = "Please enter PO Information";
                PopupControlMSG.ShowOnPageLoad = true;
                return;
            }
            
            if (dpPODate.Text == "")
            {
                lblMSG.Text = "Please select a purchase order date";
                PopupControlMSG.ShowOnPageLoad = true;
                return;
            }

            if (ddVendorName.Text == "")
            {
                lblMSG.Text = "Please select a vendor";
                PopupControlMSG.ShowOnPageLoad = true;
                return;
            }

            if (ddDeliveryType.Text == "")
            {
                lblMSG.Text = "Please select a delivery type";
                PopupControlMSG.ShowOnPageLoad = true;
                return;
            }

            //if (txtPaymentTerms.Text == "")
            //{
            //    lblMSG.Text = "Please select a payment terms";
            //    PopupControlMSG.ShowOnPageLoad = true;
            //    return;
            //}
            if (ddPartsSource.Text == "")
            {
                lblMSG.Text = "Please select parts source";
                PopupControlMSG.ShowOnPageLoad = true;
                return;
            }
            string strQuery=Session["PartSource"].ToString();// = "LOCAL";//ddPartsSource.SelectedItem.Value.ToString();
            loadDiscountDDL();
            //if (this.Session["DealerCode"].ToString() == "AAAAA")
            //{
            //    SqlDataSourcePOItems.SelectCommand = "SELECT A.ItemCode, A.ItemDesc, U.UnitDesc, A.PartItemNo, ISNULL((SUM(B.RecQty) + SUM(B.PurRetQty) + SUM(B.RetQty)) - (SUM(B.IssQty) + SUM(B.ChargeOutQty)), 0) AS BalQty FROM Item AS A LEFT OUTER JOIN ItemStock AS B ON A.ItemCode = B.ItemCode LEFT OUTER JOIN Unit AS U ON A.UnitCode = U.UnitCode WHERE A.Source='" + ddPartsSource.Value.ToString() + "' GROUP BY A.PartItemNo, A.ItemDesc, U.UnitDesc, A.ItemCode ";
            //}
            //else
            //{
            
               SqlDataSourcePOItems.SelectCommand = "SELECT A.ItemCode, A.ItemDesc, U.UnitDesc, A.PartItemNo, ISNULL((SUM(B.RecQty) + SUM(B.PurRetQty) + SUM(B.RetQty)) - (SUM(B.IssQty) + SUM(B.ChargeOutQty)), 0) AS BalQty FROM Item AS A LEFT OUTER JOIN ItemStock AS B ON A.ItemCode = B.ItemCode LEFT OUTER JOIN Unit AS U ON A.UnitCode = U.UnitCode WHERE A.Source='" + ddPartsSource.Value.ToString() + "' AND A.DealerCode='" + this.Session["DealerCode"].ToString() + "' GROUP BY A.PartItemNo, A.ItemDesc, U.UnitDesc, A.ItemCode ";
            ////}
            SqlDataSourcePOItems.Select(DataSourceSelectArguments.Empty);
              //ddPartItem.DataSource = SqlDataSourcePOItems;
               ddPartItem.DataBind();
            //DataSet ds = new DataSet();

            //////string query="SELECT A.ItemCode, A.ItemDesc, U.UnitDesc, A.PartItemNo, ISNULL((SUM(B.RecQty) + SUM(B.PurRetQty) + SUM(B.RetQty)) - (SUM(B.IssQty) + SUM(B.ChargeOutQty)), 0) AS BalQty FROM Item AS A LEFT OUTER JOIN ItemStock AS B ON A.ItemCode = B.ItemCode LEFT OUTER JOIN Unit AS U ON A.UnitCode = U.UnitCode WHERE A.Source='" + ddPartsSource.Value.ToString() + "' AND A.DealerCode='" + this.Session["DealerCode"].ToString() + "' GROUP BY A.PartItemNo, A.ItemDesc, U.UnitDesc, A.ItemCode ";
            //////myFunc.ExecuteQuery(query, ref ds);
            //////ddPartItem.DataSource = ds.Tables[0];
            //////ddPartItem.DataBind();
            ddPartItem.Text = "";
            //ddPartItem_TextChanged(ddPartItem, EventArgs.Empty);
            ddPartItem.Enabled = true;
            PopupControlItemInfo.HeaderText = "New Item";
            //PopupControlItemInfo.ShowOnPageLoad = true;
        }
        private void loadDiscountDDL()
        {
            string sqlquery = "Select DiscountText,DiscountPerc From DiscountMaster Where DealerCode='" + this.Session["DealerCode"].ToString() + "' And DiscountType='Purchase' ";
            DataSet dsDiscount = new DataSet();
            myFunc.ExecuteQuery(sqlquery, ref dsDiscount);
            ddlDiscount.ValueField = "DiscountPerc";
            ddlDiscount.TextField = "DiscountText";
            ddlDiscount.DataSource = dsDiscount.Tables[0];
            ddlDiscount.DataBind();
            ddlDiscount.SelectedIndex = 0;
        }
        protected void ddPartItem_TextChanged(object sender, EventArgs e)
        {
            
            
        }

        private void SelectPartItem()
        {

            //check item already exist
            if (ddPartItem.Text == "")
            {
                return;
            }

            if (PopupControlItemInfo.HeaderText != "Edit Item")
            {
                if (isItemExist(ddPartItem.Text.Trim()))
                {
                    lblMSG.Text = ddPartItem.Text + " already selected in this PO, please select other item";
                    PopupControlMSG.ShowOnPageLoad = true;
                    ddPartItem.Text = "";
                    return;
                }
            }


            DataTable dt = new DataTable();
            string sql = "select * from item where PartItemNo='" + ddPartItem.Text + "'";
            dt = myFunc.GetData(sql);

            if (dt.Rows.Count > 0)
            {

                txtItemDesc.Text = dt.Rows[0]["ItemDesc"].ToString();

                txtItemCode.Text = dt.Rows[0]["ItemCode"].ToString();

                txtPackQty.Text = dt.Rows[0]["PackingSize"].ToString();
                txtUnit.Text = myFunc.GetUnitDesc(dt.Rows[0]["UnitCode"].ToString());

                txtRate.Text = dt.Rows[0]["PurchaseRate"].ToString();
                //GSTPerc changed to PGSTPerc by Akram ali
                txtGSTPercent.Text = dt.Rows[0]["PGSTPERC"].ToString();
                txtFurtherTaxPerc.Text = dt.Rows[0]["PFGSTPERC"].ToString();
                txtExTaxPerc.Text = dt.Rows[0]["ExTaxPerc"].ToString();
                txtLiter.Text = dt.Rows[0]["Liter"].ToString();
            }
            dt.Dispose();

            txtQty.Text = "0";
            //txtGSTPercent.Text = "0";
            CalcNetAmount();
        }

        protected void btnCancelItem_Click(object sender, EventArgs e)
        {
            PopupControlItemInfo.ShowOnPageLoad = false;
        }

        protected void txtQty_TextChanged(object sender, EventArgs e)
        {
            CalcNetAmount();
        }


        private void CalcNetAmount()
        {
            decimal dQty = 0;
            decimal dRate = 0;
            decimal dAmount = 0;
            decimal dGSTPerc = 0;
            decimal dFurTaxPerc = 0;
            decimal dExTaxPerc = 0;
            decimal dGSTAmount = 0;
            decimal dFurAmount = 0;
            decimal dExAmount = 0;
            decimal dAmountInclGST = 0;
            decimal dDiscount = 0;
            //decimal dWHTaxPerc = 0;
            decimal dWHTaxAmount = 0;
            decimal dNetAmount = 0;
            //decimal dLiter = 0;

            try
            {
                dQty = Convert.ToDecimal(txtQty.Text.Trim() == "" ? "0" : txtQty.Text.Trim());
                dRate = Convert.ToDecimal(txtRate.Text.Trim() == "" ? "0" : txtRate.Text.Trim());
                dGSTPerc = Convert.ToDecimal(txtGSTPercent.Text.Trim() == "" ? "0" : txtGSTPercent.Text.Trim());
                dFurTaxPerc = Convert.ToDecimal(txtFurtherTaxPerc.Text.Trim() == "" ? "0" : txtFurtherTaxPerc.Text.Trim());
                dExTaxPerc = Convert.ToDecimal(txtExTaxPerc.Text.Trim() == "" ? "0" : txtExTaxPerc.Text.Trim());
                //dDiscount = Convert.ToDecimal(txtDiscount.Text.Trim() == "" ? "0" : txtDiscount.Text.Trim());
                //dLiter = Convert.ToDecimal(txtLiter.Text.Trim() == "" ? "0" : txtLiter.Text.Trim());
                //dWHTaxPerc = Convert.ToDecimal(txtWHTage.Text.Trim() == "" ? "0" : txtWHTage.Text.Trim());

                dDiscount=dRate * Convert.ToDecimal( ddlDiscount.SelectedItem.Value.ToString());
                dAmount = dQty * (dRate - dDiscount);
                dGSTAmount = dAmount * (dGSTPerc / 100);
                dFurAmount = dAmount * (dFurTaxPerc / 100);
                dExAmount = dAmount * (dExTaxPerc / 100);
                dAmountInclGST = dAmount + dGSTAmount + dFurAmount + dExAmount;
                

                //dWHTaxAmount = dAmountInclGST * (dWHTaxPerc / 100);
                dNetAmount = dAmountInclGST + dWHTaxAmount;
                txtDiscount.Text = decimal.Round(dDiscount, 2).ToString();
                txtAmount.Text = decimal.Round(dAmount, 2).ToString(); 
                txtGSTAmount.Text = decimal.Round( dGSTAmount,2).ToString();
                txtFurtherTaxAmt.Text = decimal.Round(dFurAmount, 2).ToString(); 
                txtExTaxAmt.Text = decimal.Round(dExAmount, 2).ToString();
                //txtWHTAmount.Text = Convert.ToString(dWHTaxAmount);
                txtTotalAmount.Text = Convert.ToString(Decimal.Round(dNetAmount));
                

            }
            catch (Exception ex)
            {
                //ObjGeneral.UserMsg(LbErr, Color.Red, ex.Message, txtPONo);
            }
        }

        protected void txtRate_TextChanged(object sender, EventArgs e)
        {
            CalcNetAmount();
        }

        protected void txtGSTPercent_TextChanged(object sender, EventArgs e)
        {
            CalcNetAmount();
        }

        protected void txtSaveItem_Click(object sender, EventArgs e)
        {

           // GridViewItems.DataSource
            if (txtQty.Text == "" || Convert.ToDecimal(txtQty.Text) == 0)
            {
                lblMSG.Text = "Please enter quantity";
                PopupControlMSG.ShowOnPageLoad = true;
                return;

            }
            if (ddPartItem.Text == "" && ddPartItem.Enabled)
            {
                lblMSG.Text = "Please select item";
                PopupControlMSG.ShowOnPageLoad = false;
                return;
            }

            if (!ddPartItem.Enabled)
            {
                ddPartItem.Text = GridViewItems.GetRowValues(GridViewItems.FocusedRowIndex, "PartItemNo").ToString();
            }
            if (PopupControlItemInfo.HeaderText == "Edit Item")
            {
                SaveEditedItem();
                return;
            }


            DataSet ds = new DataSet();
            
            //ds = (DataSet)ViewState["ObjPOds"];
            ds = (DataSet)this.Session["PODataSet"];


            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    if (ddPartItem.Text.Trim() == dtRow["PartItemNo"].ToString())
                    {
                        dtRow.Delete();
                        //ds.Tables[0].Rows[i].Delete();
                        ds.AcceptChanges();
                        break;
                    }
                }



                //Add Vehicle grid row when mor the on row exist

            }
            

            DataRow row = ds.Tables[0].NewRow();
            row["Sno"] = ds.Tables[0].Rows.Count + 1;
            row["PartItemNo"] = ddPartItem.Text.Trim();
            row["PartItemName"] = txtItemDesc.Text.Trim();
            row["Unit"] = txtUnit.Text.Trim();
            row["Quantity"] = txtQty.Text.Trim();
            row["Rate"] = txtRate.Text.Trim();
            row["Liter"] = txtLiter.Text.Trim();
            if (ddlDiscount.SelectedItem.Text.Trim() != "0") row["DiscountPerc"] = ddlDiscount.SelectedItem.Text.Substring(0,ddlDiscount.SelectedItem.Text.Trim().Length-1);
            else row["DiscountPerc"] = ddlDiscount.SelectedItem.Text;
            row["Discount"] = txtDiscount.Text.Trim();
            row["Amount"] = txtAmount.Text.Trim();
            row["GST"] = txtGSTPercent.Text.Trim();
            row["GSTAmount"] = txtGSTAmount.Text.Trim();
            row["PFGST"] = txtFurtherTaxPerc.Text.Trim();
            row["PFGSTAmt"] = txtFurtherTaxAmt.Text.Trim();
            row["ExTaxPerc"] = txtExTaxPerc.Text.Trim();
            row["ExTaxAmt"] = txtExTaxAmt.Text.Trim();
            row["NetAmount"] = txtTotalAmount.Text.Trim();

            ds.Tables[0].Rows.Add(row);


            this.Session["PODataSet"] = ds;

            //POItems.Columns.Add("ItemCode", typeof(string));
            //POItems.Columns.Add("PartItemNo", typeof(string));
            //POItems.Columns.Add("PartItemName", typeof(string));
            //POItems.Columns.Add("Unit", typeof(string));
            //POItems.Columns.Add("Quantity", typeof(decimal));
            //POItems.Columns.Add("Rate", typeof(decimal));
            //POItems.Columns.Add("Amount", typeof(decimal));
            //POItems.Columns.Add("GST", typeof(decimal));
            //POItems.Columns.Add("GSTAmount", typeof(decimal));
            //POItems.Columns.Add("WHTAge", typeof(decimal));
            //POItems.Columns.Add("WHTAmount", typeof(decimal));
            //POItems.Columns.Add("NetAmount", typeof(decimal));
            //POItems.PrimaryKey = new DataColumn[] { POItems.Columns["PartItemNo"] };


            GridViewItems.DataSource = ds.Tables[0];
            GridViewItems.DataBind();

            CalMainTotal();

            //PopupControlItemInfo.ShowOnPageLoad = false;

            lblStatus.Text = "Not Save";
            ddPartsSource.Enabled = false;
        }

        private void SaveEditedItem()
        {

            DataSet ds = new DataSet();

            //ds = (DataSet)ViewState["ObjPOds"];
            ds = (DataSet)this.Session["PODataSet"];

            DataRow row = ds.Tables[0].NewRow();
            row["Sno"] = ds.Tables[0].Rows.Count + 1;
            row["PartItemNo"] = GridViewItems.GetRowValues(GridViewItems.FocusedRowIndex, "PartItemNo").ToString();
            row["ItemCode"]= txtItemCode.Text;
            row["PartItemName"] = txtItemDesc.Text.Trim();
            row["Unit"] = txtUnit.Text.Trim();
            row["Quantity"] = txtQty.Text.Trim();
            row["Rate"] = txtRate.Text.Trim();
            row["Liter"] = txtLiter.Text.Trim();
            if (ddlDiscount.SelectedItem.Text.Trim() != "0") row["DiscountPerc"] = ddlDiscount.SelectedItem.Text.Substring(0,ddlDiscount.SelectedItem.Text.Trim().Length - 1);
            else row["DiscountPerc"] = ddlDiscount.SelectedItem.Text;
            row["Discount"] = txtDiscount.Text.Trim();
            row["Amount"] = txtAmount.Text.Trim();
            row["GST"] = txtGSTPercent.Text.Trim();
            row["GSTAmount"] = txtGSTAmount.Text.Trim();
            row["NetAmount"] = txtTotalAmount.Text.Trim();
            row["PFGST"] = txtFurtherTaxPerc.Text;
            row["PFGSTAmt"] = txtFurtherTaxAmt.Text;
            row["ExTaxPerc"] = txtExTaxPerc.Text;
            row["ExTaxAmt"] = txtExTaxAmt.Text;
            int rowIndex = 0;
            foreach (DataRow gvr in ds.Tables[0].Rows)
            {
                if (gvr["PartItemNo"].ToString() == row["PartItemNo"].ToString())
                {
                    rowIndex=ds.Tables[0].Rows.IndexOf(gvr);
                }
            }
            ds.Tables[0].Rows.RemoveAt(rowIndex);
            ds.Tables[0].AcceptChanges();
            ds.Tables[0].Rows.Add(row);


            this.Session["PODataSet"] = ds;

            //POItems.Columns.Add("ItemCode", typeof(string));
            //POItems.Columns.Add("PartItemNo", typeof(string));
            //POItems.Columns.Add("PartItemName", typeof(string));
            //POItems.Columns.Add("Unit", typeof(string));
            //POItems.Columns.Add("Quantity", typeof(decimal));
            //POItems.Columns.Add("Rate", typeof(decimal));
            //POItems.Columns.Add("Amount", typeof(decimal));
            //POItems.Columns.Add("GST", typeof(decimal));
            //POItems.Columns.Add("GSTAmount", typeof(decimal));
            //POItems.Columns.Add("WHTAge", typeof(decimal));
            //POItems.Columns.Add("WHTAmount", typeof(decimal));
            //POItems.Columns.Add("NetAmount", typeof(decimal));
            //POItems.PrimaryKey = new DataColumn[] { POItems.Columns["PartItemNo"] };


            GridViewItems.DataSource = ds.Tables[0];
            GridViewItems.DataBind();

            CalMainTotal();

            PopupControlItemInfo.ShowOnPageLoad = false;

            lblStatus.Text = "Not Save";
        }

        private void CalMainTotal()
        {
            DataSet ds = new DataSet();
            ds = (DataSet)this.Session["PODataSet"];

            DataRow row = ds.Tables[0].NewRow();

            decimal Quantity = 0, GrossAmount = 0, GSTAmount = 0, dExTaxAmount = 0, dFurTaxAmount = 0, WHTAmount = 0, NetAmount = 0, dLiters = 0;
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        //Convert.ToDecimal(txtQty.Text.Trim() == "" ? "0" : txtQty.Text.Trim())
                        Quantity = Convert.ToDecimal(ds.Tables[0].Rows[i]["Quantity"].ToString().Trim()== "" ? "0" : ds.Tables[0].Rows[i]["Quantity"].ToString().Trim()) + Quantity;
                        GrossAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["Amount"].ToString().Trim()== "" ? "0" : ds.Tables[0].Rows[i]["Amount"].ToString().Trim()) + GrossAmount;
                        GSTAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["GSTAmount"].ToString().Trim()== "" ? "0" : ds.Tables[0].Rows[i]["GSTAmount"].ToString().Trim()) + GSTAmount;
                        dExTaxAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["ExTaxAmt"].ToString().Trim()== "" ? "0" : ds.Tables[0].Rows[i]["ExTaxAmt"].ToString().Trim()) + dExTaxAmount;
                        dFurTaxAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["PFGSTAmt"].ToString().Trim() == "" ? "0" : ds.Tables[0].Rows[i]["PFGSTAmt"].ToString().Trim()) + dFurTaxAmount;
                        NetAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["NetAmount"].ToString().Trim() == "" ? "0" : ds.Tables[0].Rows[i]["NetAmount"].ToString().Trim()) + NetAmount;
                        dLiters = (Convert.ToDecimal(ds.Tables[0].Rows[i]["Liter"].ToString().Trim()== "" ? "0" : ds.Tables[0].Rows[i]["Liter"].ToString().Trim()) * Convert.ToDecimal(ds.Tables[0].Rows[i]["Quantity"].ToString().Trim()== "" ? "0" : ds.Tables[0].Rows[i]["Quantity"].ToString().Trim())) + dLiters;

                    }
                    txtTotalQTY.Text = Convert.ToString(Quantity);
                    txtGrossAmount.Text = Convert.ToString(GrossAmount);
                    txtTotalGST.Text = Convert.ToString(GSTAmount);
                    txtTotalFurtherTaxAmt.Text = Convert.ToString(dFurTaxAmount);
                    txtTotExTax.Text = Convert.ToString(dExTaxAmount);
                    txtTotalLtrs.Text = Convert.ToString(dLiters);
                    //txtTotalLtrs.Text=Convert.ToDecimal(txtPackQty.Text)
                    if (txtOtherCharges.Text == "")
                    {
                        txtNetAmount.Text = Convert.ToString(NetAmount);
                    }
                    else
                    {
                        txtNetAmount.Text = Convert.ToString(NetAmount - Convert.ToDecimal(txtOtherCharges.Text));
                    }
                }
                else
                {
                    txtTotalQTY.Text = Convert.ToString(0);
                    txtGrossAmount.Text = Convert.ToString(0);
                    txtTotalGST.Text = Convert.ToString(0);
                    txtTotalFurtherTaxAmt.Text = Convert.ToString(0);
                    txtTotExTax.Text = Convert.ToString(0);
                    txtNetAmount.Text = Convert.ToString(0);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void ddVendorName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddPartsSource.Items.Clear();
            //if (ddVendorName.Text.Trim() == "Other")
            //{
                ddPartsSource.Items.Add("Market Parts", "In - House");
            //}
            //else
            //{
                ddPartsSource.Items.Add("Local Parts", "LOCAL");
                ddPartsSource.Items.Add("CKD Parts", "C.K.D.");
            //}

            btnAddItem.Enabled = true;
            btnEditItem.Enabled = true;
            btnDeleteItem.Enabled = true;
            lblStatus.Text = "New";
            ddPONumber.Enabled = false;
            ddVendorName.Enabled = false;
            btnSave.Enabled = true;
        }

        protected void btnPopCancel_Click(object sender, EventArgs e)
        {
            lblRedirect.Text = "";
            PopupControlMSG.ShowOnPageLoad = false;
        }

        protected void btnPopOk_Click(object sender, EventArgs e)
        {
            lblRedirect.Text = "";
            PopupControlMSG.ShowOnPageLoad = false;
        }

        protected void Clear_Click(object sender, EventArgs e)
        {
            
        }

        protected void ddPONumber_TextChanged(object sender, EventArgs e)
        {

            this.Session["PONumber"] = ddPONumber.Text;
        }

        private void PONoChange()
        {
            if (ddPONumber.Text == "")
            {
                dpPODate.Date = Convert.ToDateTime(myFunc.GetServerTime());
                ddVendorName.Text = "";
                ddDeliveryType.Text = "";
                txtPaymentTerms.Text = "";
                ddPartsSource.Text = "";
                txtRemarks.Text = "";

                txtTotalQTY.Text = "0";
                txtTotalLtrs.Text = "0";
                txtTotalGST.Text = "0";
                txtNetAmount.Text = "0";
                txtGrossAmount.Text = "0";
                txtOtherCharges.Text = "0";
                LoadGrid(ddPONumber.Text, this.Session["DealerCode"].ToString());

                btnDelete.Enabled = false;
                btnPrint.Enabled = false;
                btnPost.Enabled = false;
                ddPONumber.Enabled = true;
                btnAddItem.Enabled = true;
                btnEditItem.Enabled = true;
                btnDeleteItem.Enabled = true;

                txtOtherCharges.Enabled = true;

                return;
            }
            this.Session["PONumber"] = ddPONumber.Text;
            DataTable dt = new DataTable();

            string sql = "SP_Spare_Get_POMaster '" + ddPONumber.Text + "','" + this.Session["DealerCode"].ToString() + "'";

            string postatus = myFunc.GetPOStatus(ddPONumber.Text.Trim());

            if (postatus == "C")
            {
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                btnPrint.Enabled = true;
                btnPost.Enabled = true;
                btnAddItem.Enabled = true;
                btnEditItem.Enabled = true;
                btnDeleteItem.Enabled = true;

                ddPONumber.Enabled = false;
                ddVendorName.Enabled = false;
                ddDeliveryType.Enabled = false;
                ddPartsSource.Enabled = false;
                txtPaymentTerms.Enabled = true;
                txtRemarks.Enabled = true;

                
                lblStatus.Text = "Edit";
            }
            else
            {


                ddPONumber.Enabled = false;
                btnAddItem.Enabled = false;
                btnEditItem.Enabled = false;
                btnDeleteItem.Enabled = false;

                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                btnPrint.Enabled = true;
                btnPost.Enabled = false;

                txtOtherCharges.Enabled = false;
                lblStatus.Text = "Posted";

                ddVendorName.Enabled = false;
                ddDeliveryType.Enabled = false;
                ddPartsSource.Enabled = false;
                txtPaymentTerms.Enabled = false;
                txtRemarks.Enabled = false;

            }

            dt = myFunc.GetData(sql);

            dpPODate.Date  = Convert.ToDateTime (myFunc.SaveDate( dt.Rows[0]["PODate"].ToString()));
            ddVendorName.Text = dt.Rows[0]["VendorDesc"].ToString();
            ddDeliveryType.Value = dt.Rows[0]["DeliveryType"].ToString();
            ddPartsSource.Text = dt.Rows[0]["POSource"].ToString();
            txtPaymentTerms.Text = dt.Rows[0]["PaymentTerm"].ToString();
            txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
            Session["PartSource"] = dt.Rows[0]["POSource"].ToString();
            //calculation
            txtTotalQTY.Text = dt.Rows[0]["TotalQty"].ToString();
            txtTotalLtrs.Text = dt.Rows[0]["TotalLiters"].ToString();
            txtTotalGST.Text = dt.Rows[0]["TotalGST"].ToString();
            txtTotalFurtherTaxAmt.Text = dt.Rows[0]["TotalPFGSTAmt"].ToString();
            txtTotExTax.Text = dt.Rows[0]["TotalExTaxAmt"].ToString();
            txtOtherCharges.Text = dt.Rows[0]["OtherCharges"].ToString();
            txtGrossAmount.Text = dt.Rows[0]["TotalAmt"].ToString();//Convert.ToDecimal(dt.Rows[0]["NetTotal"].ToString() - Convert.ToDecimal(dt.Rows[0]["OtherCharges"].ToString())).ToString();
            txtNetAmount.Text = dt.Rows[0]["NetTotal"].ToString();

            ResetPOItemsDataSet();
            //Load Items 
            LoadGrid(ddPONumber.Text, this.Session["DealerCode"].ToString());


        }
        

        protected void txtOtherCharges_TextChanged(object sender, EventArgs e)
        {
            if (txtNetAmount.Text == "")
            {
                txtNetAmount.Text = "0";
            }
            txtNetAmount.Text = Convert.ToString(Convert.ToDecimal(txtNetAmount.Text) - Convert.ToDecimal(txtOtherCharges.Text));
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            this.Session["PONumber"] = "";
            ddPONumber.Text = "";
            dpPODate.Date = Convert.ToDateTime( myFunc.GetServerTime());
            ddPONumber.Enabled=true;
            ddVendorName.Text = "";
            ddDeliveryType.Text = "";
            ddPartsSource.Text = "";
            txtPaymentTerms.Text = "";
            txtRemarks.Text = "";

            txtTotalQTY.Text = "0";
            txtTotalLtrs.Text = "0";
            txtTotalFurtherTaxAmt.Text = "0";
            txtTotExTax.Text = "0";
            txtTotalGST.Text = "0";
            txtNetAmount.Text = "0";
            txtGrossAmount.Text = "0";
            txtOtherCharges.Text = "0";
            ResetPOItemsDataSet();
            LoadGrid(ddPONumber.Text, this.Session["DealerCode"].ToString());
            lblStatus.Text = "";


            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            btnPrint.Enabled = false;
            btnPost.Enabled = false;
            ddPONumber.Enabled = true;
            btnAddItem.Enabled = false;
            btnEditItem.Enabled = false;
            btnDeleteItem.Enabled = false;

            txtOtherCharges.Enabled = true;

            ddVendorName.Enabled = true;
            ddDeliveryType.Enabled = true;
            ddPartsSource.Enabled = true;
            txtPaymentTerms.Enabled = true;
            txtRemarks.Enabled = true;
            ddlDiscount.SelectedIndex = 0;
            txtDiscount.Text = "0";
        }


        private bool isItemExist(string PartItemNumber)
        {
            bool RetVal = false;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            ds = (DataSet)this.Session["PODataSet"];
            dt = ds.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["PartItemNo"].ToString() == PartItemNumber)
                {
                    RetVal = true;
                    break;
                }
            }




            return RetVal;

        }

        protected void btnEditItem_Click(object sender, EventArgs e)
        {
            if (GridViewItems.FocusedRowIndex < 0)
            {
                lblMSG.Text = "Please select an item to edit";
                PopupControlMSG.ShowOnPageLoad = true;
                return;
            }
            loadDiscountDDL();
            PopupControlItemInfo.ShowOnPageLoad = true;
            PopupControlItemInfo.HeaderText = "Edit Item";

            //SqlDataSourcePOItems.SelectCommand = "SELECT A.ItemCode, A.ItemDesc, U.UnitDesc, A.PartItemNo, ISNULL((SUM(B.RecQty) + SUM(B.PurRetQty) + SUM(B.RetQty)) - (SUM(B.IssQty) + SUM(B.ChargeOutQty)), 0) AS BalQty FROM Item AS A LEFT OUTER JOIN ItemStock AS B ON A.ItemCode = B.ItemCode LEFT OUTER JOIN Unit AS U ON A.UnitCode = U.UnitCode WHERE A.Source='" + ddPartsSource.Value.ToString() + "' AND A.DealerCode='" + this.Session["DealerCode"].ToString() + "' GROUP BY A.PartItemNo, A.ItemDesc, U.UnitDesc, A.ItemCode ";
            //SqlDataSourcePOItems.Select(DataSourceSelectArguments.Empty);
            //ddPartsSource.DataBind();

            ddPartItem.Text = GridViewItems.GetRowValues(GridViewItems.FocusedRowIndex, "PartItemNo").ToString();
            //ddPartItem_TextChanged(ddPartItem, EventArgs.Empty);

            ddPartItem.Enabled = false;

            txtItemDesc.Text = GridViewItems.GetRowValues(GridViewItems.FocusedRowIndex, "PartItemName").ToString();
            txtItemCode.Text = GridViewItems.GetRowValues(GridViewItems.FocusedRowIndex, "ItemCode").ToString();
            txtQty.Text = GridViewItems.GetRowValues(GridViewItems.FocusedRowIndex, "Quantity").ToString();
            txtRate.Text = GridViewItems.GetRowValues(GridViewItems.FocusedRowIndex, "Rate").ToString();
            txtDiscount.Text = GridViewItems.GetRowValues(GridViewItems.FocusedRowIndex, "Discount").ToString();
            txtUnit.Text = GridViewItems.GetRowValues(GridViewItems.FocusedRowIndex, "Unit").ToString();
            txtLiter.Text = GridViewItems.GetRowValues(GridViewItems.FocusedRowIndex, "Liter").ToString();

            txtAmount.Text = GridViewItems.GetRowValues(GridViewItems.FocusedRowIndex, "Amount").ToString();
            txtGSTPercent.Text = GridViewItems.GetRowValues(GridViewItems.FocusedRowIndex, "GST").ToString();
            txtGSTAmount.Text = GridViewItems.GetRowValues(GridViewItems.FocusedRowIndex, "GSTAmount").ToString();
            txtTotalAmount.Text = GridViewItems.GetRowValues(GridViewItems.FocusedRowIndex, "NetAmount").ToString();
            txtFurtherTaxPerc.Text = GridViewItems.GetRowValues(GridViewItems.FocusedRowIndex, "PFGST").ToString();
            txtFurtherTaxAmt.Text = GridViewItems.GetRowValues(GridViewItems.FocusedRowIndex, "PFGSTAmt").ToString();
            txtExTaxPerc.Text = GridViewItems.GetRowValues(GridViewItems.FocusedRowIndex, "ExTaxPerc").ToString();
            txtExTaxAmt.Text = GridViewItems.GetRowValues(GridViewItems.FocusedRowIndex, "ExTaxAmt").ToString();

        }

        protected void btnDeleteItem_Click(object sender, EventArgs e)
        {
            if (GridViewItems.FocusedRowIndex < 0)
            {
                lblMSG.Text = "Please select an item to delete";
                PopupControlMSG.ShowOnPageLoad = true;
                return;
            }
            DataSet ds = new DataSet();

            //ds = (DataSet)ViewState["ObjPOds"];
            ds = (DataSet)this.Session["PODataSet"];

            string mPartItemNo = GridViewItems.GetRowValues(GridViewItems.FocusedRowIndex, "PartItemNo").ToString();

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    if (mPartItemNo == dtRow["PartItemNo"].ToString())
                    {
                        dtRow.Delete();
                        //ds.Tables[0].Rows[i].Delete();
                        ds.AcceptChanges();
                        break;
                    }
                }


            }
            this.Session["PODataSet"] = ds;
            GridViewItems.DataSource = ds.Tables[0];
            GridViewItems.DataBind();

            CalMainTotal();

            PopupControlItemInfo.ShowOnPageLoad = false;

            lblStatus.Text = "Not Save";

            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "";
                DataTable dt = new DataTable();
                // Save PO Header
                string mPONo = "";

                DataSet ds = (DataSet)this.Session["PODataSet"];
                if (Convert.ToDateTime(myFunc.SaveDate( dpPODate.Text)).Date > Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd")).Date)
                {
                    lblMSG.Text = "Date should not greater then current date.";
                    PopupControlMSG.ShowOnPageLoad = true;
                    return;
                    //myFunc.UserMsg(lblMSG, Color.Red, "Date should not greater then current date");
                    
                }
                if (ds.Tables[0].Rows.Count == 0)
                {
                    lblMSG.Text = "Without Items PO can't save";
                    PopupControlMSG.ShowOnPageLoad = true;
                    return;
                }

                if (ddPONumber.Text!="")
                {
                    mPONo = ddPONumber.Text;
                }
                else
                {
                    //Get New PO Number
                    mPONo = myFunc.GetNewPONumber(this.Session["DealerCode"].ToString());
                }

                if (txtPaymentTerms.Text == "")
                {
                    txtPaymentTerms.Text = "00";
                }
                if (ddPONumber.Text == "")
                {
                    //create record
                    sql = "INSERT INTO POMaster (DealerCode,PONo,PODate,VendorCode,Remarks,PaymentTerm,";
                    sql += "OtherCharges,TotalAmt,TotalQty,TotalGST,NetTotal,DelFlag,UpdUser,UpdDate,UpdTime,TransferStatus,DeliveryType,POSource,TotalPFGSTAmt,TotalExTaxAmt,TotalLiters) ";
                    sql += "VALUES ('" + this.Session["DealerCode"].ToString() + "','";
                    sql += mPONo + "','" + dpPODate.Date.ToString("yyyy/MM/dd") + "','" + ddVendorName.Value.ToString() + "','";
                    sql += txtRemarks.Text + "','" + txtPaymentTerms.Text + "','" + txtOtherCharges.Value.ToString() + "','" + txtGrossAmount.Value.ToString();
                    sql += "','"+txtTotalQTY.Text.ToString()+"','"+txtTotalGST.Value.ToString()+"','"+txtNetAmount.Value.ToString()+"','N','";
                    sql += this.Session["UserName"].ToString() + "',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,'C','" + ddDeliveryType.Value.ToString() + "','" + ddPartsSource.Value.ToString() + "','" + txtTotalFurtherTaxAmt.Text.Trim() + "','"+txtTotExTax.Text.Trim()+"','"+txtTotalLtrs.Text.Trim()+"')";

                    dt = myFunc.GetData(sql);
                }
                else
                {
                    //Update REcord
                    sql = "UPDATE POMaster SET VendorCode='" + ddVendorName.Value.ToString() + "',";
                    sql += "PODate='" + dpPODate.Date.ToString("yyyy/MM/dd") + "',";
                    sql += "Remarks='"+txtRemarks.Text + "',";
                    sql += "PaymentTerm='" + txtPaymentTerms.Text + "',";
                    sql += "OtherCharges='"+txtOtherCharges.Value.ToString()+"',";
                    sql += "TotalAmt='"+txtGrossAmount.Value.ToString()+"',";
                    sql += "TotalQty='"+txtTotalQTY.Value.ToString()+"',";
                    sql += "TotalGST='"+txtTotalGST.Value.ToString()+"',";
                    sql += "TotalPFGSTAmt='" + txtTotalFurtherTaxAmt.Value.ToString() + "',";
                    sql += "TotalExTaxAmt='" + txtTotExTax.Value.ToString() + "',";
                    sql += "NetTotal='"+txtNetAmount.Value.ToString()+"',";
                    sql += "UpdUser='"+this.Session["UserName"].ToString()+"',";
                    sql += "UpdDate=CURRENT_TIMESTAMP,UpdTime=CURRENT_TIMESTAMP,";
                    sql += "DeliveryType='"+ddDeliveryType.Value.ToString()+"',";
                    sql += "POSource='"+ddPartsSource.Value.ToString()+"',";
                    sql += "TotalLiters='" + txtTotalLtrs.Text.Trim() + "'";
                    sql += " where DealerCode='" + this.Session["DealerCode"].ToString() + "' and PONo='" + mPONo + "'";
                    dt = myFunc.GetData(sql);

                }


                // Saving Items
                // Delete existing ones
                sql = "delete from PODetail where DealerCode = '" + this.Session["DealerCode"].ToString() + "' and PONo='" + mPONo + "'";
                dt = myFunc.GetData(sql);

                //Save from memories
                //DataSet ds = new DataSet();
                //ds = (DataSet)this.Session["PODataSet"];
                string mItemCode = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dtRow in ds.Tables[0].Rows)
                    {
                        mItemCode = dtRow["PartItemNo"].ToString().Trim();
                        mItemCode = myFunc.GetItemCodeByPartItemNo(mItemCode);

                        sql = "INSERT INTO PODetail (DealerCode,PONo,ItemCode,POQty,Rate,Amount,GSTage,GSTAmount,NetAmount,RecQty,DB2PONo,PFGSTPERC,PFGSTAmt,ExTaxPerc,ExTaxAmt,Discount,DiscountPerc) ";
                        sql += "values('"+this.Session["DealerCode"].ToString()+"','"+mPONo+"','"+mItemCode+"','";
                        sql += dtRow["Quantity"].ToString()+"','"+dtRow["Rate"].ToString()+"','";
                        sql += dtRow["Amount"].ToString()+"','"+dtRow["GST"].ToString()+"','"+dtRow["GSTAmount"].ToString()+"','";
                        sql += dtRow["NetAmount"].ToString()+"','0.0','00000000','";
                        sql += dtRow["PFGST"].ToString() + "','" + dtRow["PFGSTAmt"].ToString() + "','" + dtRow["ExTaxPerc"].ToString() + "','" + dtRow["ExTaxAmt"].ToString() + "','" + dtRow["Discount"].ToString() + "','" + dtRow["DiscountPerc"].ToString() + "')"; 

                        dt = myFunc.GetData(sql);
                    }
                }

                lblMSG.Text = mPONo + " has been saved";
                PopupControlMSG.ShowOnPageLoad = true;

                btnClear_Click(btnClear, EventArgs.Empty);

                ddPONumber.Text = mPONo;
                PONoChange();
                //ddPONumber_TextChanged(ddPONumber, EventArgs.Empty);



                
            }
            catch (Exception ex)
            {
                lblMSG.Text = ex.Message;
                PopupControlMSG.ShowOnPageLoad = true;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string sql = "delete from PODetail where DealerCode = '" + this.Session["DealerCode"].ToString() + "' and PONo='" + ddPONumber.Text + "'";
            dt = myFunc.GetData(sql);
            sql = "delete from POMaster where DealerCode = '" + this.Session["DealerCode"].ToString() + "' and PONo='" + ddPONumber.Text + "'";
            dt = myFunc.GetData(sql);

            ddPONumber.GridView.DataBind();
            lblMSG.Text ="Purchase order deleted";
            //lblRedirect.Text = "Delete";
            PopupControlMSG.ShowOnPageLoad = true;
            btnClear_Click(btnClear, EventArgs.Empty);
        }

        private void DeletePO()
        {
            lblMSG.Text = "Are you sure to delete this Purchase Order?";
            PopupControlMSG.ShowOnPageLoad = true;
        }

        protected void ASPxCallbackPanel1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            ddPONumber_TextChanged(ddPONumber, EventArgs.Empty);
            //Thread.Sleep(13000);
            //DeletePO();
        }

        protected void ddPONumber_ButtonClick(object source, DevExpress.Web.ASPxEditors.ButtonEditClickEventArgs e)
        {
            
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {

            DataSet ds = (DataSet)this.Session["PODataSet"];
            if (ds.Tables[0].Rows.Count == 0)
            {
                lblMSG.Text = "Without Items PO can't post";
                PopupControlMSG.ShowOnPageLoad = true;
                return;
            }

            DataTable dt = new DataTable();
            string sql = "EXEC	SP_Spare_Update_transferOP '" + this.Session["DealerCode"].ToString() + "','" + ddPONumber.Text + "','"+ddVendorName.Text+"'";
            dt = myFunc.GetData(sql);
            ddPONumber.GridView.DataBind();
            lblMSG.Text = "Purchase order posted";
            //lblRedirect.Text = "Delete";
            PopupControlMSG.ShowOnPageLoad = true;
            btnClear_Click(btnClear, EventArgs.Empty);
        }

        protected void ddPONumber_ValueChanged(object sender, EventArgs e)
        {
           ddPONumber_TextChanged(ddPONumber, EventArgs.Empty);
        }

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            //LoadingPanel1.Visible = true;
            PONoChange();
            //LoadingPanel1.Visible = false;
        }

        protected void btnItemSelect_Click(object sender, EventArgs e)
        {
            SelectPartItem();
        }

        protected void ddDeliveryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddDeliveryType.Enabled = false;
        }

        protected void ddPartsSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["PartSource"] = ddPartsSource.SelectedItem.Value.ToString();
            
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
           // DXBMS.Data.DataSet1 objDsReports = new Data.DataSet1();
            
            DSReports data = new DSReports();
            
            ReportDocument RD = new ReportDocument();

            SqlParameter[] param =
        {
            new SqlParameter("@DealerCode",SqlDbType.Char),
            new SqlParameter("@PONO",SqlDbType.Char)

        };

            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = ddPONumber.Text;

            SqlDataReader rder = null;

            SysFunctions sysFunc = new SysFunctions();

            if (sysFunc.ExecuteSP("SP_Spare_Get_POView", param, ref rder))
            {
                data.SP_Spare_Get_POView.Load(rder);

            }

            RD.Load(Server.MapPath("../SpareReports/PurchaseOrderReport.rpt"));


            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'" + Session["DealerNTN"].ToString() + "'";
            RD.DataDefinition.FormulaFields["SaleTaxNo"].Text = "'" + Session["DealerSaleTaxNo"].ToString() + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["FromCode"].Text = "'" + FDate + "'";
            //RD.DataDefinition.FormulaFields["ToCode"].Text = "'" + TDate + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "PURCHASE ORDER" + "\"";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.Database.Tables[0].SetDataSource(data);

            // convert and show
            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "POReport" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;

            RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            string URL;
            URL = "../../../Download/OpenPDF.aspx?FileName=" + FileName;
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

        }

        protected void ddlDiscount_SelectedIndexChanged(object sender, EventArgs e)
        {
            //decimal dRate = 0, dDiscount=0;
            //dRate = Convert.ToDecimal(txtRate.Text.Trim() == "" ? "0" : txtRate.Text.Trim());
            //dDiscount = dRate * Convert.ToDecimal(ddlDiscount.SelectedItem.Value.ToString());
            //txtDiscount.Text = decimal.Round(dDiscount, 2).ToString(); 
            CalcNetAmount();

        }

        protected void txtFurtherTaxPerc_TextChanged(object sender, EventArgs e)
        {
            CalcNetAmount();
        }

        protected void txtExTaxPerc_TextChanged(object sender, EventArgs e)
        {
            CalcNetAmount();
        }

        protected void GridViewItems_PageIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            ds=(DataSet)Session["PODataSet"];
            GridViewItems.DataSource = ds.Tables[0];
            GridViewItems.DataBind();
            //btnf_Click(btnf, EventArgs.Empty);
        }

        

        
    }

    

}