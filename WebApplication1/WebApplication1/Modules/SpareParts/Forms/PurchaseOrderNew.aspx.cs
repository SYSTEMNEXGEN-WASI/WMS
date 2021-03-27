using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using DXBMS.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;

namespace DXBMS.Modules.SpareParts.Forms
{
    public partial class PurchaseOrderNew : System.Web.UI.Page
    {

        SysFunction sysFunc = new SysFunction();
        SecurityBll sec = new SecurityBll();
        SysFunctions sysFuncs = new SysFunctions();
        DataSet ds;
        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;
        bool search_result, search_item;
        DataTable PartsDT;
        double total;
        clsLookUp clslook = new clsLookUp();
       static int btnValue = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2541", "001"))
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

            if (ViewState["PurchaseOrd"] != null) PartsDT = (DataTable)ViewState["PurchaseOrd"];
            if (!IsPostBack)
            {
                if (Session["TransCode"] != null)
                {
                    createGrid();
                    lOADddl();
                    string leadId= Session["TransCode"].ToString();
                    ddlPONo.SelectedValue = leadId;

                    ddlPONo_SelectedIndexChanged(null, null);
                  

                }
                else
                {
                    lblMode.Text = "Add Mode";
                    createGrid();
                    lOADddl();
                }
                //sysFuncs.Load_CounterSalesItem_ddl(ddlChPartNo, Session["DealerCode"].ToString(), "P");
            }
            btnValue = 0;
            Session["TransCode"] = null;
            Session["LookUpData"] = string.Empty;
        }

        private void lOADddl()
        {
            ddlPartsSource.Items.Clear();
            ddlPartsSource.Items.Add(new ListItem("Select", "Select"));
            ddlPartsSource.Items.Add(new ListItem("Market Parts", "In - House"));
            ddlPartsSource.Items.Add(new ListItem("Local Parts", "LOCAL"));
            ddlPartsSource.Items.Add(new ListItem("CKD Parts", "C.K.D."));
            ddlPartsSource.Items.Add(new ListItem("Imported Parts", "Imported"));


            // DDLColums retrive
            string Where = " A.DealerCode in ('" + Session["DealerCode"].ToString() + "') ";

            string[] col = new string[] { "A.POno", "convert(varchar(10),A.PODate,105)", "A.DeliveryType" };
            sysFunc.GetMultiColumnsDDL(ddlPONo, col, "POMaster A", Where, "POno", "order by A.POno desc", false, false);

            string Where1 = " DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') ";

            string[] col1 = new string[] { "VendorCode", "VendorDesc" };
            sysFunc.GetMultiColumnsDDL(ddlVendor, col1, "Vendor", Where1, "VendorCode", "", false, false);

            string WhereQuery = "SStatus=1";
            string[] col3 = new string[] { "NName" };
            sysFunc.GetMultiColumnsDDL(ddlDeliveryType, col3, "PartsDeliveryTypes", WhereQuery, "NName", "", false, false);


            txtPODate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }

        //ddlDiscount
        private void loadDiscountDDL()
        {
            //string sqlquery = "Select DiscountText,DiscountPerc From DiscountMaster Where DealerCode='" + this.Session["DealerCode"].ToString() + "' And DiscountType='Purchase' ";
            //DataTable dsDiscount = new DataTable();
            //sysFunc.ExecuteQuery(sqlquery,ref);
            //ddlDiscountPerc.DataValueField = "DiscountPerc";
            //ddlDiscountPerc.DataTextField = "DiscountText";
            //ddlDiscountPerc.DataSource = dsDiscount;
            //ddlDiscountPerc.DataBind();
            //ddlDiscountPerc.SelectedIndex = 0;
        }

        private void createGrid()
        {
            PartsDT = new DataTable();
            PartsDT.Columns.Add(new DataColumn("POno", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("ItemCode", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("ItemDesc", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("UnitCode", typeof(string)));
            //PartsDT.Columns.Add(new DataColumn("PurchaseRate", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("Rate", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("POQty", typeof(string)));       // add
            PartsDT.Columns.Add(new DataColumn("Amount", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("DiscountPerc", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("Discount", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("GSTage", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("PFGSTPERC", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("ExTaxPerc", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("GSTAmount", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("PFGSTAmt", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("ExTaxAmt", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("NetAmount", typeof(string)));

            gv_purchaseOrder.DataSource = PartsDT; gv_purchaseOrder.DataBind(); ViewState["PurchaseOrd"] = PartsDT;
        }

        #region Master Validation

        public bool MasterValidation(TextBox[] textBoxes)
        {
            bool isValid = true;

            for (int i = 0; i < textBoxes.Length; i++)
            {
                if (textBoxes[i].Text.Trim() == "")
                {
                    isValid = false;
                    textBoxes[i].BorderColor = System.Drawing.Color.Red;
                    SetFocus(textBoxes[i]);

                    lblMessage.Text = "Error: Field(s) Marked With Red Steriks '*' Are Mendetory...";
                    break;
                }
            }
            return isValid;
        }
        #endregion

        private void Insert_in_PartsDT(DataRow rowPartsDT)
        {
            //txtAmount.Text = hfAmount.Value.ToString();
            // item description detail function
            //string ItemDesc = sysFuncs.GetStringValuesAgainstCodes("ItemCode", txtItemCode.Text, "ItemDesc", "Item",Session["DealerCode"].ToString());
            rowPartsDT["ItemCode"] = txtItemCode.Text;
            rowPartsDT["ItemDesc"] = txtItemDesc.Text;
            rowPartsDT["UnitCode"] = txtUnit.Text;
            rowPartsDT["Rate"] = txtRate.Text;
            rowPartsDT["POQty"] = txtQty.Text;
            //rowPartsDT["Rate"] = txtRate.Text;
            rowPartsDT["Amount"] = txtAmount.Text;
            rowPartsDT["DiscountPerc"] = txtDiscountPerc.Text == "" ? "0" : txtDiscountPerc.Text;
            rowPartsDT["Discount"] = txtDicountAmt.Text == "" ? "0" : txtDicountAmt.Text;
            rowPartsDT["GSTage"] = txtGSTPercent.Text.Trim() == "" ? "0" : txtGSTPercent.Text.Trim();
            rowPartsDT["PFGSTPERC"] = txtFurtherTaxPerc.Text.Trim() == "" ? "0" : txtFurtherTaxPerc.Text.Trim();
            rowPartsDT["ExTaxPerc"] = txtExTaxPerc.Text.Trim() == "" ? "0" : txtExTaxPerc.Text.Trim();
            rowPartsDT["GSTAmount"] = txtGSTAmt.Text == "" ? "0" : txtGSTAmt.Text;
            rowPartsDT["ExTaxAmt"] = txtExTaxAmt.Text == "" ? "0" : txtExTaxAmt.Text;
            rowPartsDT["NetAmount"] = txtNetAmount.Text;
            rowPartsDT["PFGSTAmt"] = txtFurtherTaxAmt.Text == "" ? "0" : txtFurtherTaxAmt.Text;
        }

        protected void btnADD_Click(object sender, EventArgs e)
        {
            PartsDT = (DataTable)ViewState["PurchaseOrd"];

            //if (ddlChPartNo.SelectedIndex == 0)
            //{
            //    sysFuncs.UserMsg(lblMessage, Color.Red, "Please Select Part First", txtQty);
            //    return;
            //}
            //else
            //{
            //    lblMessage.Text = string.Empty;
            //}

            if (txtQty.Text == "")
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please enter Quantity", txtQty);
                return;
            }

            if (txtFurtherTaxPerc.Text == "")
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please enter Further Tax %", txtFurtherTaxPerc);
                return;
            }
            if (txtExTaxPerc.Text == "")
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please enter Extra Tax %", txtQty);
                return;
            }
            if (txtExTaxPerc.Text == "")
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please enter Extra Tax %", txtExTaxPerc);
                return;
            }

            //
            TextBox[] textBoxes = { txtRate, txtQty };
            if (!MasterValidation(textBoxes)) return;
            else
            {
                search_result = false;
                foreach (DataRow rowPartsDT in PartsDT.Rows)
                {
                    if (rowPartsDT["ItemCode"].ToString().Trim() == ""
                        | rowPartsDT["ItemCode"].ToString().Trim() == txtItemCode.Text)
                    {
                        Insert_in_PartsDT(rowPartsDT);
                        search_result = true;
                    }
                }
                if (search_result == false)
                {
                    DataRow rowPartsDT = PartsDT.NewRow(); Insert_in_PartsDT(rowPartsDT); PartsDT.Rows.Add(rowPartsDT);
                }
                ViewState["PurchaseOrd"] = PartsDT; gv_purchaseOrder.DataSource = PartsDT; gv_purchaseOrder.DataBind();

                CalMainTotal();

                //sysFunc.ClearTextBoxes(Page);
                txtQty.Text = "";
                txtRate.Text = "";
                txtUnit.Text = "";
                txtAmount.Text = "";
                txtDiscountPerc.Text = "";
                txtDicountAmt.Text = "";
                txtPackingQty.Text = "";
                txtGSTAmt.Text = "";
                txtGSTPercent.Text = "";
                txtFurtherTaxPerc.Text = "";
                txtFurtherTaxAmt.Text = "";
                txtExTaxPerc.Text = "";
                txtExTaxAmt.Text = ""; txtNetAmount.Text = "";
                txtItemDesc.Text = "";
                txtItemCode.Text = "";
                txtPartItemNo_Parts.Text = "";
                //ddlChPartNo.SelectedIndex = 0;
            }
        }

        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPartsSource.Items.Clear();
            ddlPartsSource.Items.Add(new ListItem("Select", "Select"));
            ddlPartsSource.Items.Add(new ListItem("Market Parts", "In - House"));
            ddlPartsSource.Items.Add(new ListItem("Local Parts", "LOCAL"));
            ddlPartsSource.Items.Add(new ListItem("CKD Parts", "C.K.D."));
            ddlPartsSource.Items.Add(new ListItem("Imported Parts", "Imported"));
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
            decimal dDiscountPerc = 0;
            decimal dDiscount = 0;
            decimal dAmountInclGST = 0;
            decimal dWHTaxAmount = 0;
            decimal dNetAmount = 0;


            try
            {
                dQty = Convert.ToDecimal(txtQty.Text.Trim() == "" ? "0" : txtQty.Text.Trim());
                dRate = Convert.ToDecimal(txtRate.Text.Trim() == "" ? "0" : txtRate.Text.Trim());
                dGSTPerc = Convert.ToDecimal(txtGSTPercent.Text.Trim() == "" ? "0" : txtGSTPercent.Text.Trim());
                dFurTaxPerc = Convert.ToDecimal(txtFurtherTaxPerc.Text.Trim() == "" ? "0" : txtFurtherTaxPerc.Text.Trim());
                dExTaxPerc = Convert.ToDecimal(txtExTaxPerc.Text.Trim() == "" ? "0" : txtExTaxPerc.Text.Trim());
                dDiscountPerc = Convert.ToDecimal(txtDiscountPerc.Text.Trim() == "" ? "0" : txtDiscountPerc.Text.Trim());



                //dDiscountPerc = dRate * dDiscountPerc;
                dDiscount = dRate * (dDiscountPerc / 100);
                dAmount = (dQty * dRate) - dDiscount;                
                dGSTAmount = dAmount * (dGSTPerc / 100);
                dFurAmount = dAmount * (dFurTaxPerc / 100);
                dExAmount = dAmount * (dExTaxPerc / 100);
                dAmountInclGST = dAmount + dGSTAmount + dFurAmount + dExAmount;


                dNetAmount = dAmountInclGST + dWHTaxAmount;
                txtDicountAmt.Text = decimal.Round(dDiscount, 2).ToString();     //add
                txtAmount.Text = decimal.Round(dAmount, 2).ToString();
                txtGSTAmt.Text = decimal.Round(dGSTAmount, 2).ToString();
                txtFurtherTaxAmt.Text = decimal.Round(dFurAmount, 2).ToString();
                txtExTaxAmt.Text = decimal.Round(dExAmount, 2).ToString();
                txtNetAmount.Text = Convert.ToString(Decimal.Round(dNetAmount));


            }
            catch (Exception ex)
            {
                //ObjGeneral.UserMsg(LbErr, Color.Red, ex.Message, txtPONo);
                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);
            }
        }

        protected void ddlChPartNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "exec SP_PartQtyRateUnit_select '" + txtItemCode.Text + "' , '"+Session["DealerCode"].ToString()+"'";
            // string sql = "select PackingSize,UnitCode as UnitDesc,PurchaseRate from item where PartItemNo =  '" + ddlChPartNo.SelectedValue.Trim() + "'";
            //select ItemDesc,ItemCode,PackingSize,UnitCode,PurchaseRate,PGSTPERC,PFGSTPERC,ExTaxPerc,Liter from item where PartItemNo = '10-300'
            DataTable dt = new DataTable();
            dt = sysFunc.GetData(sql);

            txtRate.Text = dt.Rows[0]["PurchaseRate"].ToString();
            txtPackingQty.Text = dt.Rows[0]["PackingSize"].ToString();
            txtUnit.Text = sysFuncs.GetUnitDesc(dt.Rows[0]["UnitCode"].ToString());

            if (dt.Rows[0]["PGSTPERC"] != null && dt.Rows[0]["PGSTPERC"].ToString() != "")
            {
                txtGSTPercent.Text = dt.Rows[0]["PGSTPERC"].ToString();
                
            }
            else
            {
                txtGSTPercent.Text = "0";
            }

            if (dt.Rows[0]["PFGSTPERC"].ToString() != "" && dt.Rows[0]["PFGSTPERC"] != null)
            {
                txtFurtherTaxPerc.Text = dt.Rows[0]["PFGSTPERC"].ToString();
                
            }
            else
            {
                txtFurtherTaxPerc.Text = "0";
            }

            if (dt.Rows[0]["ExTaxPerc"].ToString() != "" && dt.Rows[0]["ExTaxPerc"] != null)
            {
                txtExTaxPerc.Text = dt.Rows[0]["ExTaxPerc"].ToString();
                
            }
            else
            {
                txtExTaxPerc.Text = "0";
            }

            txtQty.Focus();


        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2541", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            //if (txtPODate.Text == "")
            //{

            //    sysFuncs.UserMsg(lblMessage, Color.Red, "Please Select Date First", txtPODate);
            //    return;
            //}
            if (ddlVendor.SelectedIndex == 0)
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please Select Vendor", txtPODate);
                return;
            }            
            else if (ddlPartsSource.SelectedIndex == 0)
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please Select Source", txtPayment);
                return;
            }
            if (ddlDeliveryType.SelectedIndex == 0)
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please Enter ExtraCharges", txtPayment);
                return;
            }
            else
            {
                lblMessage.Text = string.Empty;
            }

            if (gv_purchaseOrder.Rows.Count == 0)
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please Enter Detail", txtPayment);
                return;
            }


            try
            {
                string max;
                if (ddlPONo.SelectedIndex == 0)
                {
                    max = sysFuncs.AutoGen("POMaster", "POno", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));

                }
                else
                {
                    if (!sec.UserRight("2541", "003"))
                    {
                        Response.Redirect("~/Test.aspx");
                    }
                    max = ddlPONo.SelectedValue.Trim();
                }



                SqlParameter[] param1 = {
                        new SqlParameter("@DealerCode", SqlDbType.Char, 5),//0
                        new SqlParameter("@POno", SqlDbType.Char, 10), //1
                        new SqlParameter("@PODate", SqlDbType.DateTime),//2
                        new SqlParameter("@VendorCode", SqlDbType.Char, 10),//3
                        new SqlParameter("@Remarks", SqlDbType.VarChar, 50),//4
                        new SqlParameter("@PaymentTerm", SqlDbType.VarChar,10), //5
                        new SqlParameter("@OtherCharges", SqlDbType.Decimal), //6                       
                        // new SqlParameter("@DelFlag", SqlDbType.Char, 1),
                        new SqlParameter("@TotalQty", SqlDbType.Decimal),   //7 
                        new SqlParameter("@TotalAmt", SqlDbType.Decimal), //8
                        new SqlParameter("@TotalGST", SqlDbType.Decimal),  //9
                        // new SqlParameter("@PostFlag", SqlDbType.Char, 1),                        
                        
                        new SqlParameter("@NetTotal", SqlDbType.Decimal),   //10
                        new SqlParameter("@UpdUser", SqlDbType.VarChar, 50),   //11              
                        new SqlParameter("@DeliveryType", SqlDbType.VarChar,80),   //12        
                        new SqlParameter("@POSource", SqlDbType.VarChar, 10), //13
                        new SqlParameter("@TotalPFGSTAmt", SqlDbType.Decimal), //14
                        new SqlParameter("@TotalExTaxAmt", SqlDbType.Decimal), //15
                  };

                param1[0].Value = Session["DealerCode"].ToString();
                param1[1].Value = max;
                param1[2].Value = sysFuncs.SaveDate(txtPODate.Text);
                param1[3].Value = ddlVendor.SelectedValue.ToString().Trim();
                param1[4].Value = txtRemarks.Text.Trim();
                param1[5].Value = txtPayment.Text.Trim();
                param1[6].Value = Convert.ToDecimal(txtOtherCharges.Text.Trim() == "" ? "0" : txtOtherCharges.Text.Trim());
                param1[7].Value = Convert.ToDecimal(txtTotalQty.Text.Trim() == "" ? "0" : txtTotalQty.Text.Trim());
                param1[8].Value = Convert.ToDecimal(txtGrossAmt.Text.Trim() == "" ? "0" : txtGrossAmt.Text.Trim());
                param1[9].Value = Convert.ToDecimal(txtTotalGSTAmt.Text.Trim() == "" ? "0" : txtTotalGSTAmt.Text.Trim());
                param1[10].Value = Convert.ToDecimal(txtTotalNetAmt.Text.Trim() == "" ? "0" : txtTotalNetAmt.Text.Trim());
                param1[11].Value = Session["UserName"].ToString();
                param1[12].Value = ddlDeliveryType.SelectedValue.ToString().Trim();
                param1[13].Value = ddlPartsSource.SelectedValue.ToString().Trim();
                param1[14].Value = Convert.ToDecimal(txtTotalFurtherTaxAmnt.Text.Trim() == "" ? "0" : txtTotalFurtherTaxAmnt.Text.Trim());
                param1[15].Value = Convert.ToDecimal(txtTotalExtraTaxAmnt.Text.Trim() == "" ? "0" : txtTotalExtraTaxAmnt.Text.Trim());
                // param1[13].Value = "ALI";

                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    if (sysFunc.ExecuteSP_NonQuery("SP_Spare_Insert_POMaster", param1,Trans))
                    {
                        SqlParameter[] param2 = {

                                    new SqlParameter("@DealerCode", SqlDbType.Char, 5),//0
                                    new SqlParameter("@POno", SqlDbType.Char, 10), //1
                                    new SqlParameter("@ItemCode",SqlDbType.Char,8), //2
                                    new SqlParameter("@POQty",SqlDbType.Float),  //3
                                    new SqlParameter("@Rate",SqlDbType.Float),  //4
                                    new SqlParameter("@Amount",SqlDbType.Float), //5
                                    new SqlParameter("@GSTage",SqlDbType.Float), //6
                                    new SqlParameter("@GSTAmount",SqlDbType.Float),  //7
                                    new SqlParameter("@NetAmount",SqlDbType.Float), //8     
                                    new SqlParameter("@PFGSTPERC",SqlDbType.Float), //9
                                    new SqlParameter("@PFGSTAmt",SqlDbType.Float), //10
                                    new SqlParameter("@ExTaxPerc",SqlDbType.Float), //11
                                    new SqlParameter("@ExTaxAmt",SqlDbType.Float), //12
                                    new SqlParameter("@Discount",SqlDbType.Float), //13
                                    new SqlParameter("@DiscountPerc",SqlDbType.Float), //14
                };

                        PartsDT = (DataTable)ViewState["PurchaseOrd"];
                        DataRow[] drr = PartsDT.Select();
                        for (int i = 0; i < drr.Length; i++)
                        {
                            if (PartsDT.Rows[i]["ItemCode"].ToString() != "")
                            {
                                param2[0].Value = Session["DealerCode"].ToString(); // 0
                                param2[1].Value = max;                               //1
                                param2[2].Value = PartsDT.Rows[i]["ItemCode"].ToString();
                                param2[3].Value = Convert.ToDecimal(PartsDT.Rows[i]["POQty"].ToString() == "" ? "0" : (PartsDT.Rows[i]["POQty"].ToString()));
                                param2[4].Value = Convert.ToDecimal(PartsDT.Rows[i]["Rate"].ToString() == "" ? "0" : (PartsDT.Rows[i]["Rate"].ToString()));
                                param2[5].Value = Convert.ToDecimal(PartsDT.Rows[i]["Amount"].ToString() == "" ? "0" : (PartsDT.Rows[i]["Amount"].ToString()));
                                param2[6].Value = Convert.ToDecimal(PartsDT.Rows[i]["GSTage"].ToString() == "" ? "0" : (PartsDT.Rows[i]["GSTage"].ToString()));
                                param2[7].Value = Convert.ToDecimal(PartsDT.Rows[i]["GSTAmount"].ToString() == "" ? "0" : (PartsDT.Rows[i]["GSTAmount"].ToString()));
                                param2[8].Value = Convert.ToDecimal(PartsDT.Rows[i]["NetAmount"].ToString() == "" ? "0" : (PartsDT.Rows[i]["NetAmount"].ToString()));
                                param2[9].Value = Convert.ToDecimal(PartsDT.Rows[i]["PFGSTPERC"].ToString() == "" ? "0" : (PartsDT.Rows[i]["PFGSTPERC"].ToString()));
                                param2[10].Value = Convert.ToDecimal(PartsDT.Rows[i]["PFGSTAmt"].ToString() == "" ? "0" : (PartsDT.Rows[i]["PFGSTAmt"].ToString()));
                                param2[11].Value = Convert.ToDecimal(PartsDT.Rows[i]["ExTaxPerc"].ToString() == "" ? "0" : (PartsDT.Rows[i]["ExTaxPerc"].ToString()));
                                param2[12].Value = Convert.ToDecimal(PartsDT.Rows[i]["ExTaxAmt"].ToString() == "" ? "0" : (PartsDT.Rows[i]["ExTaxAmt"].ToString()));
                                param2[13].Value = Convert.ToDecimal(PartsDT.Rows[i]["Discount"].ToString() == "" ? "0" : (PartsDT.Rows[i]["Discount"].ToString()));
                                param2[14].Value = Convert.ToDecimal(PartsDT.Rows[i]["DiscountPerc"].ToString() == "" ? "0" : (PartsDT.Rows[i]["DiscountPerc"].ToString()));


                            }

                            if (sysFunc.ExecuteSP_NonQuery("SP_Spare_Insert_PODetails", param2, Trans))
                            {

                            }
                            else
                            {
                                ObjTrans.RollBackTransaction(ref Trans);
                            }

                        }
                        ObjTrans.CommittTransaction(ref Trans);
                        createGrid();
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        lblMessage.Text = "New Data Inserted." + max;
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Record Saved,Updated Successfully: " + max + "')", true);
                        lOADddl();

                        gv_purchaseOrder.DataSource = null;
                        gv_purchaseOrder.DataBind();

                        ClearAll();

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);

                    }

                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);
            }
        }
        protected void txtGSTPercent_TextChanged(object sender, EventArgs e)
        {
            CalcNetAmount();
        }

        protected void btnCLear_Click(object sender, EventArgs e)
        {
            //ddlChPartNo.ClearSelection();
            lblMessage.Text = "";
            ClearAll();                     

            createGrid();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);

        }

        private void ClearAll()
        {
            ddlPONo.SelectedIndex = 0;
            ddlVendor.SelectedIndex = 0;
            ddlDeliveryType.SelectedIndex = 0;
            txtPayment.Text = string.Empty;
            ddlPartsSource.SelectedIndex = 0;
            txtRemarks.Text = "";
            txtTotalQty.Text = "0";
            txtTotalLitre.Text = "0";
            txtTotalGSTAmt.Text = "0";
            txtTotalFurtherTaxAmnt.Text = "0";
            txtTotalNetAmt.Text = "0";
            txtPackingQty.Text = "0";
            txtFurtherTaxAmt.Text = "0";
            txtExTaxAmt.Text = "0";
            txtNetAmount.Text = "0";
            txtGrossAmt.Text = "0";
            txtTotalExtraTaxAmnt.Text = "0";
            txtOtherCharges.Text = "0";
            txtOtherCharges.Enabled = true;
        }

        protected void txtFurtherTaxPerc_TextChanged(object sender, EventArgs e)
        {
            CalcNetAmount();
        }

        protected void txtExTaxPerc_TextChanged(object sender, EventArgs e)
        {
            CalcNetAmount();
        }


        private void CalMainTotal()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["PurchaseOrd"];

            decimal Quantity = 0, GrossAmount = 0, GSTAmount = 0, dExTaxAmount = 0, dFurTaxAmount = 0, NetAmount = 0 ; /* WHTAmount = 0, dLiters = 0*/
            try
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //Convert.ToDecimal(txtQty.Text.Trim() == "" ? "0" : txtQty.Text.Trim())
                        Quantity = Convert.ToDecimal(dt.Rows[i]["POQty"].ToString().Trim() == "" ? "0" : dt.Rows[i]["POQty"].ToString().Trim()) + Quantity;
                        GrossAmount = Convert.ToDecimal(dt.Rows[i]["Amount"].ToString().Trim() == "" ? "0" : dt.Rows[i]["Amount"].ToString().Trim()) + GrossAmount;
                        GSTAmount = Convert.ToDecimal(dt.Rows[i]["GSTAmount"].ToString().Trim() == "" ? "0" : dt.Rows[i]["GSTAmount"].ToString().Trim()) + GSTAmount;
                        dExTaxAmount = Convert.ToDecimal(dt.Rows[i]["ExTaxAmt"].ToString().Trim() == "" ? "0" : dt.Rows[i]["ExTaxAmt"].ToString().Trim()) + dExTaxAmount;
                        dFurTaxAmount = Convert.ToDecimal(dt.Rows[i]["PFGSTAmt"].ToString().Trim() == "" ? "0" : dt.Rows[i]["PFGSTAmt"].ToString().Trim()) + dFurTaxAmount;
                        NetAmount = Convert.ToDecimal(dt.Rows[i]["NetAmount"].ToString().Trim() == "" ? "0" : dt.Rows[i]["NetAmount"].ToString().Trim()) + NetAmount;
                    }

                    txtTotalQty.Text = Convert.ToString(Quantity);
                    txtGrossAmt.Text = Convert.ToString(GrossAmount);
                    txtTotalGSTAmt.Text = Convert.ToString(GSTAmount);
                    txtTotalFurtherTaxAmnt.Text = Convert.ToString(dFurTaxAmount);
                    txtTotalExtraTaxAmnt.Text = Convert.ToString(dExTaxAmount);
                    txtTotalNetAmt.Text = Convert.ToString(NetAmount);

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
                    txtTotalQty.Text = Convert.ToString(0);
                    txtGrossAmt.Text = Convert.ToString(0);
                    txtTotalGSTAmt.Text = Convert.ToString(0);
                    txtTotalFurtherTaxAmnt.Text = Convert.ToString(0);
                    txtTotalExtraTaxAmnt.Text = Convert.ToString(0);
                    txtNetAmount.Text = Convert.ToString(0);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ddlPONo_SelectedIndexChanged(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            SqlDataReader dr = null;
            lblMode.Text = "Edit Mode";
           // ddlPONo.SelectedValue = Session["TransCode"].ToString();
            // Data load in ddlchargoutNo
            SqlParameter[] param =
       {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                new SqlParameter("@POno",SqlDbType.Char,10), //1
            };
            param[0].Value = Session["DealerCode"].ToString(); // 0
            param[1].Value = ddlPONo.SelectedValue.Trim();

            sysFuncs.ExecuteSP("SP_POMaster_POSelect", param, ref dr);

            dt.Load(dr);  // Load

            // Showing Data in Text Field through DropDownLIST
            if (dt.Rows.Count > 0)
            {
                txtPODate.Text = dt.Rows[0]["PODate"].ToString().Trim();

                ddlVendor.SelectedValue = dt.Rows[0]["VendorCode"].ToString().Trim();


                ddlDeliveryType.SelectedValue = dt.Rows[0]["DeliveryType"].ToString().Trim();
                txtPayment.Text = dt.Rows[0]["PaymentTerm"].ToString().Trim();
                ddlPartsSource.SelectedValue = dt.Rows[0]["POSource"].ToString().Trim();
                txtRemarks.Text = dt.Rows[0]["Remarks"].ToString().Trim();
                txtGrossAmt.Text = dt.Rows[0]["TotalAmt"].ToString().Trim();
                txtTotalQty.Text = dt.Rows[0]["TotalQty"].ToString().Trim();
                txtTotalGSTAmt.Text = dt.Rows[0]["TotalGST"].ToString().Trim();
                txtTotalNetAmt.Text = dt.Rows[0]["NetTotal"].ToString().Trim();
                txtTotalExtraTaxAmnt.Text = dt.Rows[0]["TotalExTaxAmt"].ToString().Trim();
                txtOtherCharges.Text = dt.Rows[0]["OtherCharges"].ToString().Trim();
                txtTotalFurtherTaxAmnt.Text = dt.Rows[0]["TotalPFGSTAmt"].ToString().Trim();

            }
            dr.Close();
            DataSet dsParts = new DataSet();
            dsParts = sysFuncs.FillDataSet("SP_PODetail_POSelect", param);
            gv_purchaseOrder.DataSource = dsParts;
            gv_purchaseOrder.DataBind();

            ViewState["PurchaseOrd"] = dsParts.Tables[0];
        }



        protected void BtnRemove_PurchaseOrder_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsdelete = new DataSet();

            DataTable dt = new DataTable();

            try
            {
                dt = (DataTable)ViewState["PurchaseOrd"];

                ImageButton BtnLnk2 = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk2.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;
                String code = dt.Rows[rowGvIndex]["ItemCode"].ToString();
                decimal qty =    Convert.ToDecimal(dt.Rows[rowGvIndex]["POQty"].ToString());
                decimal gst =    Convert.ToDecimal(dt.Rows[rowGvIndex]["GSTAmount"].ToString().Trim() == "" ? "0" : dt.Rows[rowGvIndex]["GSTAmount"].ToString().Trim());
                decimal fgst =   Convert.ToDecimal(dt.Rows[rowGvIndex]["PFGSTAmt"].ToString().Trim() == "" ? "0" : dt.Rows[rowGvIndex]["PFGSTAmt"].ToString());
                decimal exTax =  Convert.ToDecimal(dt.Rows[rowGvIndex]["ExTaxAmt"].ToString().Trim() == "" ? "0" : dt.Rows[rowGvIndex]["ExTaxAmt"].ToString().Trim());
                decimal amt = Convert.ToDecimal(dt.Rows[rowGvIndex]["Amount"].ToString().Trim() == "" ? "0" : dt.Rows[rowGvIndex]["Amount"].ToString().Trim());
                decimal netAmt = Convert.ToDecimal(dt.Rows[rowGvIndex]["NetAmount"].ToString().Trim() == "" ? "0" : dt.Rows[rowGvIndex]["NetAmount"].ToString().Trim());

                decimal Quantity = 0, GrossAmount = 0, GSTAmount = 0, dExTaxAmount = 0, dFurTaxAmount = 0, NetAmount = 0 ;

                Quantity = Convert.ToDecimal(txtTotalQty.Text);
                GrossAmount = Convert.ToDecimal(txtGrossAmt.Text);
                GSTAmount = Convert.ToDecimal(txtTotalGSTAmt.Text);
                dExTaxAmount = Convert.ToDecimal(txtTotalExtraTaxAmnt.Text);
                dFurTaxAmount = Convert.ToDecimal(txtTotalFurtherTaxAmnt.Text);
                NetAmount = Convert.ToDecimal(txtTotalNetAmt.Text);


                txtTotalQty.Text = Convert.ToString(Quantity - qty);
                txtGrossAmt.Text = Convert.ToString(GrossAmount - amt);
                txtTotalGSTAmt.Text = Convert.ToString(GSTAmount - gst);
                txtTotalFurtherTaxAmnt.Text = Convert.ToString(dFurTaxAmount - fgst);
                txtTotalExtraTaxAmnt.Text = Convert.ToString(dExTaxAmount - exTax);
                txtTotalNetAmt.Text = Convert.ToString(NetAmount - netAmt);


                dt.Rows[rowGvIndex].Delete();
                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                gv_purchaseOrder.DataSource = dt;
                gv_purchaseOrder.DataBind();

                ViewState["PurchaseOrd"] = dt;

                if (ddlPONo.SelectedIndex != 0)
                {
                    if (ObjTrans.BeginTransaction(ref Trans) == true)
                    {
                        string sql = "delete from PODetail where ItemCode = '" + code.Trim() + "' and DealerCode = '"+Session["DealerCode"].ToString()+"'";
                        SqlDataReader reader = null;
                        sysFunc.ExecuteQuery(sql, ref reader);
                    }
                    ObjTrans.CommittTransaction(ref Trans);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;
            }
        }


        protected void gv_purchaseOrder_DataBound(object sender, EventArgs e)
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


        //
        protected void gv_purchaseOrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && !(gv_purchaseOrder.EditIndex == e.Row.RowIndex))
                {
                    e.Row.Cells[0].Enabled = true;
                }

                if (e.Row.RowType == DataControlRowType.DataRow && (gv_purchaseOrder.EditIndex == e.Row.RowIndex))
                {
                    //btn_remove.Enabled = false;

                    // DropDownList ddlbrand_code = (DropDownList)e.Row.FindControl("ddlbrand_code");
                    TextBox txtQuantityEdit = (TextBox)e.Row.FindControl("txtQuantityEdit");
                    //TextBox txtStateCode = (TextBox)e.Row.FindControl("txtStateCode");
                    //TextBox txtCountryDesc = (TextBox)e.Row.FindControl("txtStateDescription");

                    txtQuantityEdit.Text = Session["lblQuantity"].ToString();
                    //txtCountryDesc.Text = Session["lblCountryDesc"].ToString();
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


        //// FOR GRIDVIEW ROW EDITING
        protected void gv_purchaseOrder_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                DataTable dtEditing = new DataTable();

                Label lblQuantity = (Label)gv_purchaseOrder.Rows[e.NewEditIndex].FindControl("lblQuantity");


                Session["lblQuantity"] = lblQuantity.Text;

                gv_purchaseOrder.EditIndex = e.NewEditIndex;

                dtEditing = (DataTable)ViewState["PurchaseOrd"];
                gv_purchaseOrder.DataSource = dtEditing;
                gv_purchaseOrder.DataBind();
                ViewState["PurchaseOrd"] = dtEditing;

            }

            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.Message;

            }

        }

        protected void btnLookup_Click(object sender, EventArgs e)
        {
            ViewState["lookupid"] = 10; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_Parts_PO(imgLookup, ViewState["lookupid"].ToString(), "", "../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }

        // ROW CANCEL EDIT
        protected void gv_purchaseOrder_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //DataTable dt = new DataTable();
            //gv_WFPolicy.EditIndex = -1;
            //dt = (DataTable)Session["WFPolicyDS"];
            //gv_WFPolicy.DataSource = dt;
            //gv_WFPolicy.DataBind();
            DataSet ds = new DataSet();

            gv_purchaseOrder.EditIndex = -1;

            PartsDT = (DataTable)ViewState["PurchaseOrd"];

            gv_purchaseOrder.DataSource = PartsDT;

            gv_purchaseOrder.DataBind();
        }


        // RowUpdating
        protected void gv_purchaseOrder_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt_update = new DataTable();
            Label lblItemCode = (Label)gv_purchaseOrder.Rows[e.RowIndex].FindControl("lblItemCode");
            TextBox txtQuantityEdit = (TextBox)gv_purchaseOrder.Rows[e.RowIndex].FindControl("txtQuantityEdit");
            //Label lblQuantity = (Label)gv_purchaseOrder.Rows[e.RowIndex].FindControl("lblQuantity");
            Label lblRate = (Label)gv_purchaseOrder.Rows[e.RowIndex].FindControl("lblRate");
            Label lblAmount = (Label)gv_purchaseOrder.Rows[e.RowIndex].FindControl("lblAmount");
            Label lblDiscPercent = (Label)gv_purchaseOrder.Rows[e.RowIndex].FindControl("lblDiscPercent");
            Label lblGSTPErcent = (Label)gv_purchaseOrder.Rows[e.RowIndex].FindControl("lblGSTPErcent");
            Label lblFurTaxPercent = (Label)gv_purchaseOrder.Rows[e.RowIndex].FindControl("lblFurTaxPercent");
            Label lblExTaxPercent = (Label)gv_purchaseOrder.Rows[e.RowIndex].FindControl("lblExTaxPercent");
            Label lblNetAmount = (Label)gv_purchaseOrder.Rows[e.RowIndex].FindControl("lblNetAmount");

            Label lblGSTAmount = (Label)gv_purchaseOrder.Rows[e.RowIndex].FindControl("lblGSTAmount");
            Label lblFurTaxAmount = (Label)gv_purchaseOrder.Rows[e.RowIndex].FindControl("lblFurTaxAmount");
            Label lblExTaxAmount = (Label)gv_purchaseOrder.Rows[e.RowIndex].FindControl("lblExTaxAmount");

            string lblQuantity = Session["lblQuantity"].ToString();

            decimal Quantity = 0, GrossAmount = 0, GSTAmount = 0, ExTaxAmount = 0, FurTaxAmount = 0, NetAmount = 0;

            Quantity = Convert.ToDecimal(txtTotalQty.Text) - Convert.ToDecimal(lblQuantity);
            GrossAmount = Convert.ToDecimal(txtGrossAmt.Text) - Convert.ToDecimal(lblAmount.Text);
            GSTAmount = Convert.ToDecimal(txtTotalGSTAmt.Text) - Convert.ToDecimal(lblGSTAmount.Text == "" ? "0" : lblGSTAmount.Text);
            FurTaxAmount = Convert.ToDecimal(txtTotalFurtherTaxAmnt.Text) - Convert.ToDecimal(lblFurTaxAmount.Text == "" ? "0" : lblFurTaxAmount.Text);
            ExTaxAmount = Convert.ToDecimal(txtTotalExtraTaxAmnt.Text) - Convert.ToDecimal(lblExTaxAmount.Text == "" ? "0" : lblExTaxAmount.Text);
            NetAmount = Convert.ToDecimal(txtTotalNetAmt.Text) - Convert.ToDecimal(lblNetAmount.Text);

            if (txtQuantityEdit.Text.Trim() == string.Empty)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Quantity can not null or empty.";
                return;
            }
            

            Decimal qty = 0, rate = 0, amt = 0 ,disPerc = 0,disAmt = 0, gst = 0 , fgst = 0, extra = 0;
            Decimal gstAmt = 0, fgstAmt = 0, extraAmt = 0, total = 0;

            try
            {
                dt_update = (DataTable)ViewState["PurchaseOrd"];

                foreach (DataRow dr in dt_update.Rows)
                {
                    if (dr["ItemCode"].ToString() == lblItemCode.Text)
                    {
                        gst = Convert.ToDecimal(lblGSTPErcent.Text.Trim() == "" ? "0" : lblGSTPErcent.Text.Trim());
                        fgst = Convert.ToDecimal(lblFurTaxPercent.Text.Trim() == "" ? "0" : lblFurTaxPercent.Text.Trim());
                        extra = Convert.ToDecimal(lblExTaxPercent.Text.Trim() == "" ? "0" : lblExTaxPercent.Text.Trim());
                        disPerc = Convert.ToDecimal(lblDiscPercent.Text.Trim() == "" ? "0" : lblDiscPercent.Text.Trim());                        
                        qty = Convert.ToDecimal(txtQuantityEdit.Text.Trim());
                        rate = Convert.ToDecimal(lblRate.Text);

                        disAmt = rate * (disPerc / 100);
                        amt = qty * (rate - disAmt);
                        gstAmt = amt * (gst / 100);
                        fgstAmt = amt * (fgst / 100);
                        extraAmt = amt * (extra / 100);
                        total = amt + gstAmt + fgstAmt + extraAmt;

                        dr["POQty"] = txtQuantityEdit.Text.Trim();
                        dr["Amount"] = amt.ToString();
                        dr["GSTAmount"] = gstAmt;
                        dr["PFGSTAmt"] = fgstAmt;
                        dr["ExTaxAmt"] = extraAmt;
                        dr["NetAmount"] = total;
                    }
                }

                txtTotalQty.Text = Convert.ToString(Quantity + qty);
                txtGrossAmt.Text = Convert.ToString(GrossAmount + amt);
                txtTotalGSTAmt.Text = Convert.ToString(GSTAmount + gstAmt);
                txtTotalFurtherTaxAmnt.Text = Convert.ToString(FurTaxAmount + fgstAmt);
                txtTotalExtraTaxAmnt.Text = Convert.ToString(ExTaxAmount + extraAmt);
                txtTotalNetAmt.Text = Convert.ToString(NetAmount + total);

                gv_purchaseOrder.EditIndex = -1;

                gv_purchaseOrder.DataSource = dt_update;
                gv_purchaseOrder.DataBind();

                ViewState["PurchaseOrd"] = dt_update;

                if(ddlPONo.SelectedIndex != 0)
                {
                    if (ObjTrans.BeginTransaction(ref Trans) == true)
                    {
                        SqlDataReader reader = null;
                        string sql = "update PODetail set POQty = '" + txtQuantityEdit.Text + "'  WHERE PONo = '"+ddlPONo.SelectedValue+"' and ItemCode='" + lblItemCode.Text + "' and DealerCode = '"+Session["DealerCode"].ToString() +"' ";

                        sysFunc.ExecuteQuery(sql, ref reader);
                    }

                    ObjTrans.CommittTransaction(ref Trans);
                }

                
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                lblMessage.Visible = true;
                lblMessage.ForeColor = Color.Red;
                lblMessage.Text = ex.Message;
            }
        }

        protected void SelectedPartDetail(string item)
        {
            try
            {

                if (btnValue == 1)
                {
                    ddlPONo.SelectedValue = item;
                    ddlPONo_SelectedIndexChanged(null, null);

                }
                else
                {
                    DataTable dt = new DataTable();
                    dt = sysFuncs.GetData("select ItemDesc,PartItemNo from Item where DealerCode in('COMON','" + Session["DealerCode"].ToString() + "') And  ItemCode='" + item + "'");
                    txtItemCode.Text = item;
                    txtItemDesc.Text = dt.Rows[0]["ItemDesc"].ToString().Trim();
                    txtPartItemNo_Parts.Text = dt.Rows[0]["PartItemNo"].ToString().Trim();
                    ddlChPartNo_SelectedIndexChanged(null, null);
                }

            }
            catch (Exception ex)
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, ex.Message);
            }
        }

        protected void ddlPartsSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string sQuery = "SP_SelectPartsForPurchaseOrder '" + Session["DealerCode"].ToString() + "', '"+ddlPartsSource.SelectedValue+"'";

            if (sysFuncs.ExecuteQuery(sQuery, ref ds))
            {
                Session["Parts"] = ds;

            }
        }

        protected void txtQty_TextChanged(object sender, EventArgs e)
        {
            decimal rate = 0, qty = 0, amount = 0;
            decimal dGSTPerc = 0, dFurTaxPerc = 0, dExTaxPerc = 0;
            decimal GSTAmount = 0, FurAmount = 0, ExAmount = 0,NetAmt=0;

            qty = Convert.ToDecimal(txtQty.Text);
            rate = Convert.ToDecimal(txtRate.Text);
            amount = qty * rate;

            dGSTPerc = Convert.ToDecimal(txtGSTPercent.Text.Trim() == "" ? "0" : txtGSTPercent.Text.Trim());
            dFurTaxPerc = Convert.ToDecimal(txtFurtherTaxPerc.Text.Trim() == "" ? "0" : txtFurtherTaxPerc.Text.Trim());
            dExTaxPerc = Convert.ToDecimal(txtExTaxPerc.Text.Trim() == "" ? "0" : txtExTaxPerc.Text.Trim());

            GSTAmount = amount * (dGSTPerc / 100);
            FurAmount = amount * (dFurTaxPerc / 100);
            ExAmount = amount * (dExTaxPerc / 100);
            NetAmt = GSTAmount + FurAmount + ExAmount + amount;
            txtGSTAmt.Text = decimal.Round(GSTAmount, 2).ToString();
            txtFurtherTaxAmt.Text = decimal.Round(FurAmount, 2).ToString();
            txtExTaxAmt.Text = decimal.Round(ExAmount, 2).ToString();

            txtAmount.Text = decimal.Round(amount,2).ToString();
            txtNetAmount.Text = decimal.Round(NetAmt,2).ToString();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2541", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }
            Data.DSReports data = new Data.DSReports();
            ReportDocument RD = new ReportDocument();



            SqlParameter[] param =
        {
             new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
             new SqlParameter("@PONo",SqlDbType.Char,10), //1

        };

            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = ddlPONo.SelectedValue.Trim();

            SqlDataReader rder = null;

            // Store Procedure check and load
            if (sysFunc.ExecuteSP("SP_Spare_Get_POView", param, ref rder)) //Database SP
            {
                data.SP_Spare_Get_POView.Load(rder);                       // DataSet SP

            }
            // Load in crystal report
            //RD.Load(Server.MapPath("../PurchaseSaleReturnReport.rpt"));
            //RD.Load(Server.MapPath("../crtStockAdjustment.rpt"));
            RD.Load(Server.MapPath("../SpareReports/PurchaseOrderReport.rpt"));

            // mention paper size
            RD.PrintOptions.PaperSize = PaperSize.PaperA4;


            //RD.Load(Server.MapPath("../../SpareReports/rptInvoices.rpt"));
            //RD.Load(Server.MapPath("../rptJobHistory.rpt"));
            //RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + sysFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Phone1", "Dealer") + "'";
            //RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + sysFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Email", "Dealer") + "'";

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["FromCode"].Text = "'" + FDate + "'";
            //RD.DataDefinition.FormulaFields["ToCode"].Text = "'" + TDate + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Purchase Order" + "\"";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";

            RD.Database.Tables[0].SetDataSource(data);
            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "Report.pdf";
            string File = FilePath + FileName;

            PdfDocument outputDocument = new PdfDocument();
            Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            DirectoryInfo info = new DirectoryInfo(FilePath);
            if (!info.Exists)
            {
                info.Create();
            }

            string path = Path.Combine(FilePath, FileName);
            using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
            {
                stream.CopyTo(outputFileStream);
            }
            stream.Dispose(); stream.Close();
            RD.Dispose(); RD.Close();
            string URL = "../../../Download/PrintReport.aspx";

            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";

            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

            //// convert and show
            //string FilePath = Server.MapPath("~") + "\\Download\\";
            //string FileName = "PurchaseOrder" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            //string File = FilePath + FileName;

            //Session["RD"] = RD;

            //string URL;
            //URL = "../../../Download/rptViewerService.aspx?FileName=" + FileName;
            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1200,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }

        protected void btnClear2_Click(object sender, EventArgs e)
        {
            txtItemCode.Text = string.Empty;
            txtPartItemNo_Parts.Text = string.Empty;
            txtItemDesc.Text = string.Empty;
            txtPackingQty.Text = string.Empty;
            txtQty.Text = string.Empty;
            txtRate.Text = string.Empty;
            txtDiscountPerc.Text = string.Empty;
            txtDicountAmt.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtGSTPercent.Text = string.Empty;
            txtGSTAmt.Text = string.Empty;
            txtFurtherTaxPerc.Text = string.Empty;
            txtFurtherTaxAmt.Text = string.Empty;
            txtExTaxPerc.Text = string.Empty;
            txtExTaxAmt.Text = string.Empty;
            txtNetAmount.Text = string.Empty;
            txtUnit.Text = string.Empty;
        }

        protected void txtOtherCharges_TextChanged(object sender, EventArgs e)
        {
            decimal GrossAmt = Convert.ToDecimal(txtTotalNetAmt.Text.Trim() == "" ? "0" : txtTotalNetAmt.Text.Trim());
            decimal OtherCharges = Convert.ToDecimal(txtOtherCharges.Text.Trim() == "" ? "0" : txtOtherCharges.Text.Trim());

            txtTotalNetAmt.Text = decimal.Round(GrossAmt + OtherCharges,2).ToString();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2541", "004"))
            {
                Response.Redirect("~/Test.aspx");
            }
            DataSet dsdelete = new DataSet();

            if (sysFunc.CodeExists("DCMaster", "PONo", ddlPONo.SelectedValue, Session["DealerCode"].ToString(), ref dsdelete))
            {
                lblMessage.Text = "Cannot delete , Order Confirmation of this PO No has already made";
                return;
            }

            DataTable dt = new DataTable();
            string sql = "delete from PODetail where DealerCode = '" + this.Session["DealerCode"].ToString() + "' and PONo='" + ddlPONo.SelectedValue + "'";
            dt = sysFunc.GetData(sql);
            sql = "delete from POMaster where DealerCode = '" + this.Session["DealerCode"].ToString() + "' and PONo='" + ddlPONo.SelectedValue + "'";
            dt = sysFunc.GetData(sql);
            
            lblMessage.Text = "Purchase order deleted";

            //lblRedirect.Text = "Delete";
            btnCLear_Click(btnCLear, EventArgs.Empty);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Deletealert()", true);
        }

        protected void ImageButtonPO_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 11; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_PONumber(ImageButtonPO, ViewState["lookupid"].ToString(), "", "../../../");

            btnValue = 1;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void txtDiscountPerc_TextChanged(object sender, EventArgs e)
        {
            CalcNetAmount();
        }
    }
}