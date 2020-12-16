using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Threading;
using System.IO;
using PdfSharp.Pdf;

namespace DXBMS.Modules.SpareParts.Forms
{
    public partial class CounterSalesInv : System.Web.UI.Page
    {
        Transaction ObjTrans = new Transaction();
        SysFunctions SysFunc = new SysFunctions();
        SecurityBll sec = new SecurityBll();
        MainBLL ObjMain = new MainBLL();
        DataSet dsGVMain = new DataSet();
        clsLookUp clslook = new clsLookUp();
        static int btnValue=0;
        decimal sumFooterGRNQty = 0;
        decimal SumFooterGSTAmt = 0;
        decimal SumFooterExAmt = 0;
        decimal SumFooterFurAmt = 0;
        decimal SumFooterDiscount = 0;
        decimal SumFooterTotalAmt = 0;
        decimal SumFooterNetAmount = 0;
        static string URL;
        SqlTransaction Trans;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2546", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            string strQuery = string.Empty;

            if (Page.IsPostBack)
            {

                if (!string.IsNullOrEmpty((string)Session["LookUpData"]))
                {
                    SelectedPartDetail(Session["LookUpData"].ToString());
                }
            }

            if (!IsPostBack)
            {
                if (Session["TransCode"] != null)
                {
                    ThreadStart childthreat = new ThreadStart(childthreadcall);
                    Thread child = new Thread(childthreat);

                    child.Start();

                    txtCounterDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    Load_ddlCustomer();
                    createPartsDT();
                    Load_ddlCounterNo();
                    //DropDownList ddlParts = (DropDownList)GvMain.FooterRow.FindControl("ddlParts");
                    //SysFunc.Load_CounterSalesItem_ddl(ddlParts, Session["DealerCode"].ToString());

                    strQuery = "SELECT LTrim(RTrim(DiscountText)) as 'DiscountText',DiscountPerc From DiscountMaster Where DealerCode='" + Session["DealerCode"].ToString() + "' And DiscountType='Sales'";
                    ObjMain.FillDropDown(ddlDiscount, strQuery, "DiscountText", "DiscountPerc", "Select");
                    string leadId = Session["TransCode"].ToString();
                    ddlCounterNo.SelectedValue = leadId;

                    ddlCounterNo_SelectedIndexChanged(null, null);


                }
                else
                {
                    lblMode.Text = "Add Mode";
                    ThreadStart childthreat = new ThreadStart(childthreadcall);
                    Thread child = new Thread(childthreat);

                    child.Start();

                    txtCounterDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    Load_ddlCustomer();
                    createPartsDT();
                    Load_ddlCounterNo();
                    //DropDownList ddlParts = (DropDownList)GvMain.FooterRow.FindControl("ddlParts");
                    //SysFunc.Load_CounterSalesItem_ddl(ddlParts, Session["DealerCode"].ToString());

                    strQuery = "SELECT LTrim(RTrim(DiscountText)) as 'DiscountText',DiscountPerc From DiscountMaster Where DealerCode='" + Session["DealerCode"].ToString() + "' And DiscountType='Sales'";
                    ObjMain.FillDropDown(ddlDiscount, strQuery, "DiscountText", "DiscountPerc", "Select");
                }
                

            }
            btnValue = 0;
            Session["TransCode"] = null;
            Session["LookUpData"] = string.Empty;
        }

        public void childthreadcall()
        {
            try
            {
                DataSet ds = new DataSet();
                string sQuery = "SP_SelectParts '" + Session["DealerCode"].ToString() + "', ''";

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

        private void Load_ddlCustomer()
        {
            string[] Columns = new string[] { "CusCode", "CusDesc", "NIC", "Phone1" };
            SysFunc.GetMultiColumnsDDL(ddlCustomer, Columns, "Customer", "DealerCode ='" + Session["DealerCode"].ToString() + "'", "CusCode", " Order by  CusCode Desc ", false, false);
        }
        private void Load_ddlCounterNo()
        {
            string WhereClause = " a.DealerCode = '" + Session["DealerCode"].ToString() + "'" +
                 "and a.DelFlag = 'N' " +
                 "and a.DealerCode = b.DealerCode " +
                 "and a.CusCode = b.CusCode ";
            string[] Columns = new string[] { "a.SaleInvNo", "convert(varchar(10),a.SaleInvDate,105)", "b.CusDesc", "Case When a.PostFlag='N' Then 'Not-Posted' Else 'Posted' End " };
            SysFunc.GetMultiColumnsDDL(ddlCounterNo, Columns, "CounterSaleMaster a,Customer b", WhereClause, "SaleInvNo", " Order by SaleInvNo Desc ", false, false);
        }
        private void createPartsDT()
        {
            dsGVMain = new DataSet();
            dsGVMain.Tables.Add();
            
            dsGVMain.Tables[0].Columns.Add(new DataColumn("ItemCode", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("PartItemNo", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("ItemDesc", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("UnitDesc", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("Qty", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("SaleRate", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("DiscountPerc", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("DiscountAmt", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("NetAmount", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("RecQty", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("GSTage", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("GSTAmount", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("SalesExTaxPerc", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("SalesExTaxAmt", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("FGSTPERC", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("FGSTAmt", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("TotalAmount", typeof(string)));
            DataRow dr = dsGVMain.Tables[0].NewRow();
            dsGVMain.Tables[0].Rows.Add(dr);

            GvMain.DataSource = dsGVMain.Tables[0];
            GvMain.DataBind();
            Session["dsParts"] = dsGVMain;
        }

        //protected void ddlParts_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        ds = (DataSet)Session["dsParts"];
        //        DropDownList ddlParts = (DropDownList)GvMain.FooterRow.FindControl("ddlParts");
        //        if (GvMain.Rows.Count > 0)
        //        {
        //            foreach (DataRow dtRow in ds.Tables["dtParts"].Rows)
        //            {
        //                if (ddlParts.SelectedValue.ToString() == dtRow["ItemCode"].ToString())
        //                {
        //                    SysFunc.UserMsg(LbErr, Color.Red, "Part No. " + dtRow["PartItemNo"].ToString() + " is already add in the list");
        //                    return;
        //                }
        //            }
        //        }
        //        if (ds.Tables["dtParts"].Rows[0]["ItemCode"].ToString() == String.Empty)
        //        {
        //            ds.Tables["dtParts"].Rows.RemoveAt(0);
        //            AddNewRow();
        //        }
        //        else
        //        {
        //            AddNewRow();
        //        }
        //        totalCalculation();
        //    }
        //    catch (Exception ex)
        //    {
        //        SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
        //    }
        //}
        
        private void AddNewRow()
        {
            SqlDataReader dr = null;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            //TextBox txtItemCode = (TextBox)GvMain.FooterRow.FindControl("txtItemCode");
            //TextBox txtFooterQty = (TextBox)GvMain.FooterRow.FindControl("txtFooterQty");
            //TextBox txtFooterRate = (TextBox)GvMain.FooterRow.FindControl("txtFooterRate");

            SqlParameter[] param = {new SqlParameter("@DealerCode",SqlDbType.Char,5 ),
                                               new SqlParameter("@ItemCode",SqlDbType.Char,8),
                                           };
            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = txtItemCode.Text;
            SysFunc.ExecuteSP("sp_CounterSalesItem_Select", param, ref dr);
            dt.Load(dr);
            
            ds = (DataSet)Session["dsParts"];
            DataRow row = ds.Tables[0].NewRow();

            row["ItemCode"] = dt.Rows[0]["ItemCode"].ToString();
            row["PartItemNo"] = dt.Rows[0]["PartItemNo"].ToString();
            row["ItemDesc"] = dt.Rows[0]["ItemDesc"].ToString();
            row["UnitDesc"] = dt.Rows[0]["UnitDesc"].ToString();
            row["Qty"] = txtQuantity.Text;
            if(txtPartPrice.Text.Trim() == "")
            {
                row["SaleRate"] = dt.Rows[0]["SaleRate"].ToString();
            }else
            {
                row["SaleRate"] = txtPartPrice.Text.Trim();
            }
            
            
            if (ddlDiscount.SelectedIndex == 0)
            {
                row["DiscountPerc"] = dt.Rows[0]["DiscountPerc"].ToString();
                row["DiscountAmt"] = dt.Rows[0]["DiscountAmt"].ToString();
            }
            else
            {

                decimal dDiscount = 0;
                decimal dRate = Convert.ToDecimal (dt.Rows[0]["SaleRate"].ToString());

                dDiscount = dRate * Convert.ToDecimal(ddlDiscount.SelectedItem.Value.ToString());
                row["DiscountAmt"] = dDiscount.ToString();
                row["DiscountPerc"] = ddlDiscount.SelectedItem.Text.Substring(0, ddlDiscount.SelectedItem.Text.Trim().Length - 1);
            }
            row["NetAmount"] = dt.Rows[0]["NetAmount"].ToString();
            row["RecQty"] = dt.Rows[0]["RecQty"].ToString();
            row["GSTage"] = dt.Rows[0]["GSTage"].ToString();
            row["GSTAmount"] = dt.Rows[0]["GSTAmount"].ToString();
            row["SalesExTaxPerc"] = dt.Rows[0]["SalesExTaxPerc"].ToString();
            row["SalesExTaxAmt"] = dt.Rows[0]["SalesExTaxAmt"].ToString();
            row["FGSTPERC"] = dt.Rows[0]["FGSTPERC"].ToString();
            row["FGSTAmt"] = dt.Rows[0]["FGSTAmt"].ToString();
            row["TotalAmount"] = dt.Rows[0]["TotalAmount"].ToString();

            ds.Tables[0].Rows.Add(row);
            GvMain.DataSource = ds.Tables[0];
            GvMain.DataBind();
            Session["dsParts"] = ds;
            ClacLiters();
            
            //////////////////////
        }

        protected void GvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //decimal Qty = 0;
            
            decimal NetAmount = 0;
            decimal dQty = 0,
                 dRate = 0,
                 dGSTPerc = 0,
                 dFurTaxPerc = 0,
                 dExTaxPerc = 0,
                 dGSTAmount = 0,
                 dFurAmount = 0,
                 dExAmount = 0,
                 dAmountInclGST = 0;
            
            if (e.Row.RowType == DataControlRowType.DataRow && (GvMain.EditIndex == e.Row.RowIndex))
            {
                ////Label lblEditItemCode = (Label)e.Row.FindControl("lblEditItemCode");
                //if (lblEditItemCode.Text != string.Empty)
                //{
                    //GvMain.Columns[0].Visible = true;
                    TextBox Rate = (TextBox)e.Row.FindControl("txtEditRate");
                    Decimal DiscountAmt = Convert.ToDecimal(((Label)e.Row.FindControl("lblEditDiscount")).Text);
                    TextBox txtEditQty = (TextBox)e.Row.FindControl("txtAcceptedQty");
                    Label lblNetAmount = (Label)e.Row.FindControl("lblEditNetAmount");

                    NetAmount = (Convert.ToDecimal(Rate.Text) - DiscountAmt) * Convert.ToDecimal(txtEditQty.Text);
                    lblNetAmount.Text = NetAmount.ToString();

                //}
                //else
                //{
                    //GvMain.Columns[0].Visible = false; 
                //}
                //Label lblEditQty=(TextBox)e.Row.FindControl("txtAcceptedQty");
                //Qty = Convert.ToDecimal(lblEditQty.Text);
            }
            if (e.Row.RowType == DataControlRowType.DataRow && !(GvMain.EditIndex == e.Row.RowIndex))
            {
                Label lblItemCode = (Label)e.Row.FindControl("lblItemCode");
                ImageButton BtnRemovePart = (ImageButton)e.Row.FindControl("BtnRemovePart");
                if (lblItemCode.Text != string.Empty)
                {
                    GvMain.Columns[0].Visible = true;
                    BtnRemovePart.Enabled = true;
                    Decimal Rate = Convert.ToDecimal(((Label)e.Row.FindControl("lblRate")).Text);
                    Decimal DiscountAmt = Convert.ToDecimal(((Label)e.Row.FindControl("lblDiscount")).Text);
                    Label lblQty = (Label)e.Row.FindControl("lblAccQty");
                    Label lblNetAmount = (Label)e.Row.FindControl("lblNetAmount");
                    NetAmount = (Rate - DiscountAmt) * Convert.ToDecimal(lblQty.Text);
                    lblNetAmount.Text = NetAmount.ToString();


                    Label LBGSTage = (Label)e.Row.FindControl("LBGSTage");
                    Label LbGSTAmount = (Label)e.Row.FindControl("LbGSTAmount");
                    Label lblExTaxPerc = (Label)e.Row.FindControl("lblExTaxPerc");
                    Label lblExTaxAmt = (Label)e.Row.FindControl("lblExTaxAmt");
                    Label lblFurTaxPerc = (Label)e.Row.FindControl("lblFurTaxPerc");
                    Label lblFurTaxAmt = (Label)e.Row.FindControl("lblFurTaxAmt");
                    Label LbAmountMain = (Label)e.Row.FindControl("LbAmountMain");

                    dGSTPerc = Convert.ToDecimal(LBGSTage.Text.Trim() == "" ? "0" : LBGSTage.Text.Trim());
                    dExTaxPerc = Convert.ToDecimal(lblExTaxPerc.Text.Trim() == "" ? "0" : lblExTaxPerc.Text.Trim());
                    dFurTaxPerc = Convert.ToDecimal(lblFurTaxPerc.Text.Trim() == "" ? "0" : lblFurTaxPerc.Text.Trim());

                    CalcNetAmount(ref dQty,
                 ref Rate,
                  NetAmount,
                 ref dGSTPerc,
                 ref dFurTaxPerc,
                 ref dExTaxPerc,
                 ref dGSTAmount,
                 ref dFurAmount,
                 ref dExAmount,
                 ref dAmountInclGST);

                    LbGSTAmount.Text = decimal.Round(dGSTAmount, 2).ToString();
                    lblExTaxAmt.Text = decimal.Round(dExAmount, 2).ToString();
                    lblFurTaxAmt.Text = decimal.Round(dFurAmount, 2).ToString();
                    LbAmountMain.Text = decimal.Round(dAmountInclGST, 2).ToString();
                    DataSet ds = new DataSet();
                    ds = (DataSet)Session["dsParts"];
                    foreach (DataRow dr in ds.Tables[0].Rows) // search whole table
                    {
                        if (dr["ItemCode"].ToString() == lblItemCode.Text) // if id==2
                        {
                            //dr["Qty"] = txtQty.Text;

                            //dr["NetAmount"] = decimal.Round(dAmount, 2).ToString();

                            dr["GSTAmount"] = decimal.Round(dGSTAmount, 2).ToString();
                            dr["FGSTAmt"] = decimal.Round(dFurAmount, 2).ToString();
                            dr["SalesExTaxAmt"] = decimal.Round(dExAmount, 2).ToString();
                            dr["TotalAmount"] = Convert.ToString(Decimal.Round(dAmountInclGST,2));

                        }
                    }
                    Session["dsParts"] = ds;
                    string strTotals;
                    decimal dtotalvalue;
                    decimal qty, discount , disAmt;
                    //Sum of GRN Qty
                    strTotals = ((Label)e.Row.FindControl("lblAccQty")).Text;
                    dtotalvalue = Convert.ToDecimal(strTotals);
                    sumFooterGRNQty += dtotalvalue;
                    qty = dtotalvalue;

                    //Sum of GST Amt
                    strTotals = ((Label)e.Row.FindControl("LbGSTAmount")).Text;
                    dtotalvalue = Convert.ToDecimal(strTotals);
                    SumFooterGSTAmt += dtotalvalue;

                    //Sum of Extra Tax Amt
                    strTotals = ((Label)e.Row.FindControl("lblExTaxAmt")).Text;
                    dtotalvalue = Convert.ToDecimal(strTotals);
                    SumFooterExAmt += dtotalvalue;

                    //Sum of Further Tax Amt
                    strTotals = ((Label)e.Row.FindControl("lblFurTaxAmt")).Text;
                    dtotalvalue = Convert.ToDecimal(strTotals);
                    SumFooterFurAmt += dtotalvalue;

                    //Sum of Discount Amt
                    strTotals = ((Label)e.Row.FindControl("lblDiscount")).Text;
                    dtotalvalue = Convert.ToDecimal(strTotals);
                    discount = dtotalvalue;

                    disAmt = qty * discount;

                    SumFooterDiscount += disAmt;

                    //Sum of Total Amt
                    strTotals = ((Label)e.Row.FindControl("LbAmountMain")).Text;
                    dtotalvalue = Convert.ToDecimal(strTotals);
                    SumFooterTotalAmt += dtotalvalue;

                    strTotals = ((Label)e.Row.FindControl("lblNetAmount")).Text;
                    dtotalvalue = Convert.ToDecimal(strTotals);
                    SumFooterNetAmount += dtotalvalue;

                }
                else
                {
                    GvMain.Columns[0].Visible = false;
                    BtnRemovePart.Enabled = false;
                }
        }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //Set Sum of GRN Qty in Footer
                Label lblTotalAccQty = (Label)e.Row.FindControl("lblTotalAccQty");
                lblTotalAccQty.Text = sumFooterGRNQty.ToString();

                //Set Sum of GST Amt in Footer
                Label lblTotalGSTAmt = (Label)e.Row.FindControl("lblTotalGSTAmt");
                lblTotalGSTAmt.Text = SumFooterGSTAmt.ToString();

                //Set Sum of Ex Amt in Footer
                Label lblTotalExAmt = (Label)e.Row.FindControl("lblTotalExAmt");
                lblTotalExAmt.Text = SumFooterExAmt.ToString();

                //Set Sum of Further Amt in Footer
                Label lblTotalFurAmt = (Label)e.Row.FindControl("lblTotalFurAmt");
                lblTotalFurAmt.Text = SumFooterFurAmt.ToString();

                //Set Sum of Discount in Footer
                Label lblTotalDiscount = (Label)e.Row.FindControl("lblTotalDiscount");
                lblTotalDiscount.Text = Convert.ToString(SumFooterDiscount);

                //Set Sum of Total Amount in Footer
                Label lblFooterTotalAmt = (Label)e.Row.FindControl("lblFooterTotalAmt");
                lblFooterTotalAmt.Text = SumFooterTotalAmt.ToString();

                //Set Sum of Net Amount in Footer
                Label lblFooterNetAmt = (Label)e.Row.FindControl("lblTotalNetAmount");
                lblFooterNetAmt.Text = SumFooterNetAmount.ToString();

                //DropDownList ddlParts = (DropDownList)e.Row.FindControl("ddlParts");
                //SysFunc.Load_CounterSalesItem_ddl(ddlParts, Session["DealerCode"].ToString());

            }


        }

        protected void GvMain_RowEditing(object sender, GridViewEditEventArgs e)
        {
            DataSet ds = new DataSet();

            GvMain.EditIndex = e.NewEditIndex;
            ds = (DataSet)Session["dsParts"];
            GvMain.DataSource = ds.Tables[0];
            GvMain.DataBind();

        }

        protected void GvMain_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
           
            DataSet ds = new DataSet();
            GvMain.EditIndex = -1;
            ds = (DataSet)Session["dsParts"];
            GvMain.DataSource = ds.Tables[0];
            GvMain.DataBind();
        }
        private void CalcNetAmount(

            ref decimal dQty,
            ref decimal dRate,
            decimal dAmount,
            ref decimal dGSTPerc,
            ref decimal dFurTaxPerc,
            ref decimal dExTaxPerc,
            ref decimal dGSTAmount,
            ref decimal dFurAmount,
            ref decimal dExAmount,
            ref decimal dAmountInclGST
            //ref decimal dWHTaxAmount,
            //ref decimal dNetAmount
        )
        {   
                //dAmount = dQty * (dRate - dDiscount);
                dGSTAmount = dAmount * (dGSTPerc / 100);
                dFurAmount = dAmount * (dFurTaxPerc / 100);
                dExAmount = dAmount * (dExTaxPerc / 100);
                dAmountInclGST = dAmount + dGSTAmount + dFurAmount + dExAmount;
                //dNetAmount = dAmountInclGST + dWHTaxAmount;
        }
        protected void GvMain_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            decimal dQty = 0,
                    dDCQty = 0,
                 dRate = 0,
                 dAmount = 0,
                 dGSTPerc = 0,
                 dFurTaxPerc = 0,
                 dExTaxPerc = 0,
                 dGSTAmount = 0,
                 dFurAmount = 0,
                 dExAmount = 0,
                 dAmountInclGST = 0;

            DataSet ds = new DataSet();
            try
            {

                //DropDownList ddlEditLocation = (DropDownList)GvMain.Rows[e.RowIndex].FindControl("ddlLocation");
                Label lblItemCode = (Label)GvMain.Rows[e.RowIndex].FindControl("lblEditItemCode");
                TextBox txtQty = (TextBox)GvMain.Rows[e.RowIndex].FindControl("txtAcceptedQty");
                TextBox lblEditRate = (TextBox)GvMain.Rows[e.RowIndex].FindControl("txtEditRate");
                
                //Label lblEditDCQty = (Label)GvMain.Rows[e.RowIndex].FindControl("lblEditDCQty");
                Label lblEditNetAmount = (Label)GvMain.Rows[e.RowIndex].FindControl("lblEditNetAmount");
                TextBox txtGSTPerc = (TextBox)GvMain.Rows[e.RowIndex].FindControl("txtGSTPerc");
                Label LbEditGSTAmount = (Label)GvMain.Rows[e.RowIndex].FindControl("LbEditGSTAmount");
                TextBox txtExTaxPerc = (TextBox)GvMain.Rows[e.RowIndex].FindControl("txtExTaxPerc");
                Label lblEditExTaxAmt = (Label)GvMain.Rows[e.RowIndex].FindControl("lblEditExTaxAmt");
                TextBox txtFurTaxPerc = (TextBox)GvMain.Rows[e.RowIndex].FindControl("txtFurTaxPerc");
                Label lblEditFurTaxAmt = (Label)GvMain.Rows[e.RowIndex].FindControl("lblEditFurTaxAmt");

                Label lblEditDiscountPerc = (Label)GvMain.Rows[e.RowIndex].FindControl("lblEditDiscountPerc");
                Label lblEditDiscount = (Label)GvMain.Rows[e.RowIndex].FindControl("lblEditDiscount");
                //Label LbEditAmountMain = (Label)GvMain.Rows[e.RowIndex].FindControl("lblEditDiscount");
                
                dQty = Convert.ToDecimal(txtQty.Text.Trim() == "" ? "0" : txtQty.Text.Trim());
                if (dQty == 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Quantity can not be zero(0)");
                    return;
                }
                //dDCQty = Convert.ToDecimal(lblEditDCQty.Text.Trim() == "" ? "0" : lblEditDCQty.Text.Trim());
                dRate = Convert.ToDecimal(lblEditRate.Text.Trim() == "" ? "0" : lblEditRate.Text.Trim());
                dGSTPerc = Convert.ToDecimal(txtGSTPerc.Text.Trim() == "" ? "0" : txtGSTPerc.Text.Trim());
                dFurTaxPerc = Convert.ToDecimal(txtFurTaxPerc.Text.Trim() == "" ? "0" : txtFurTaxPerc.Text.Trim());
                dExTaxPerc = Convert.ToDecimal(txtExTaxPerc.Text.Trim() == "" ? "0" : txtExTaxPerc.Text.Trim());
                dAmount = Convert.ToDecimal(lblEditNetAmount.Text.Trim() == "" ? "0" : lblEditNetAmount.Text.Trim());
                //dDiscount = Convert.ToDecimal(lblEditDiscount.Text.Trim() == "" ? "0" : lblEditDiscount.Text.Trim());

                CalcNetAmount(ref dQty,
             ref dRate,
              dAmount,
             ref dGSTPerc,
             ref dFurTaxPerc,
             ref dExTaxPerc,
             ref dGSTAmount,
             ref dFurAmount,
             ref dExAmount,
             ref dAmountInclGST);

                ds = (DataSet)Session["dsParts"];
                foreach (DataRow dr in ds.Tables[0].Rows) // search whole table
                {
                    if (dr["ItemCode"].ToString() == lblItemCode.Text) // if id==2
                    {
                        dr["Qty"] = txtQty.Text;

                        //dr["NetAmount"] = decimal.Round(dAmount, 2).ToString();
                        dr["SaleRate"] = decimal.Round(dRate, 2).ToString();
                        dr["GSTAmount"] = decimal.Round(dGSTAmount, 2).ToString();
                        dr["FGSTAmt"] = decimal.Round(dFurAmount, 2).ToString();
                        dr["SalesExTaxAmt"] = decimal.Round(dExAmount, 2).ToString();
                        dr["GSTage"] = txtGSTPerc.Text;
                        dr["SalesExTaxPerc"] = txtExTaxPerc.Text;
                        dr["FGSTPerc"] = txtFurTaxPerc.Text;
                        dr["DiscountPerc"] = lblEditDiscountPerc.Text;
                        dr["DiscountAmt"] = lblEditDiscount.Text;
                        dr["NetAmount"] = lblEditNetAmount.Text;
                        dr["TotalAmount"] = Convert.ToString(Decimal.Round(dAmountInclGST));

                    }
                }
                GvMain.EditIndex = -1;
                GvMain.DataSource = ds.Tables[0];
                GvMain.DataBind();
                Session["dsParts"] = ds;
                totalCalculation();
                ClacLiters();
                //totalCalculation();

            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }

        }

        protected void BtnRemovePart_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsDelete = new DataSet();
            try
            {   
                dsDelete = (DataSet)Session["dsParts"];
                ImageButton BtnLnk = (ImageButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;
                Label lblDCQty = (Label)GvMain.Rows[rowGv.RowIndex].FindControl("lblDCQty");
                if (Convert.ToInt32(lblDCQty.Text) > 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Stock issued against this part");
                    return;
                }
                dsDelete.Tables[0].Rows[rowGvIndex].Delete();
                dsDelete.Tables[0].AcceptChanges();
                if (dsDelete.Tables[0].Rows.Count == 0)
                {
                    DataRow dr = dsDelete.Tables[0].NewRow();
                    dsDelete.Tables[0].Rows.Add(dr);
                    //ddlDiscount.Enabled = true;
                }
                GvMain.DataSource = dsDelete.Tables[0];
                GvMain.DataBind();
                LbErr.Text = "";
                Session["dsParts"] = dsDelete;
                totalCalculation();
                ClacLiters();
                
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }
        private void totalCalculation()
        {
            decimal DGrossAmount = 0, DOtherCharges = 0, FrightCharges = 0;
            Label lblFooterTotalAmt = (Label)GvMain.FooterRow.FindControl("lblFooterTotalAmt");
                DGrossAmount = Convert.ToDecimal(lblFooterTotalAmt.Text.Trim() == "" ? "0" : lblFooterTotalAmt.Text.Trim());
                DOtherCharges = Convert.ToDecimal(txtOtherChargers.Text.Trim() == "" ? "0" : txtOtherChargers.Text.Trim());
            FrightCharges = Convert.ToDecimal(txtFreightCharges.Text.Trim() == "" ? "0" : txtFreightCharges.Text.Trim());
            txtNetAmount.Text = Convert.ToString(DGrossAmount + DOtherCharges + FrightCharges);
                GridNetAmount.Text = Convert.ToString(DGrossAmount);
        }

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //ddlDiscount.Enabled = false;
                DataSet ds = new DataSet();
                ds = (DataSet)Session["dsParts"];
             
                if (txtItemCode.Text == "")
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Select part first.");
                    return;
                }
                if (txtQuantity.Text.Trim() == string.Empty)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Quantity can not empty or zero(0)");
                    return;
                }
                if (Convert.ToDecimal(txtQuantity.Text.Trim()) <= 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Quantity can not empty or zero(0)");
                    return;
                }
                if (txtPartPrice.Text != "")
                {
                    if(Convert.ToDouble(txtPartPrice.Text.Trim()) <= 0)
                    {
                        SysFunc.UserMsg(LbErr, Color.Red, "Rate can not be less than or equal to zero(0)");
                        return;
                    }
                    
                }
                foreach (GridViewRow row in GvMain.Rows)
                {
                    Label lkn = (Label)row.FindControl("lblItemCode");
                    if (lkn.Text == txtItemCode.Text)
                    {
                        SysFunc.UserMsg(LbErr, Color.Red, "Part is already add in the list...");
                        return;
                    }

                }
                //if (GvMain.Rows.Count > 0)
                //{
                //    foreach (DataRow dtRow in ds.Tables[0].Rows)
                //    {
                //        if (txtItemCode.Text == dtRow["ItemCode"].ToString())
                //        {
                //            SysFunc.UserMsg(LbErr, Color.Red, "Part No. " + dtRow["PartItemNo"].ToString() + " is already add in the list");
                //            return;
                //        }
                //    }
                //}
                if (ds.Tables[0].Rows[0]["ItemCode"].ToString() == String.Empty)
                {
                    ds.Tables[0].Rows.RemoveAt(0);
                    AddNewRow();
                }
                else
                {
                    AddNewRow();
                }
                totalCalculation();

                clearFields();
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2546", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (ddlCounterNo.SelectedIndex != 0)
            {
                if (!sec.UserRight("2546", "003"))
                {
                    Response.Redirect("~/Test.aspx");
                }
            }

            if (txtCounterDate.Text.Trim() == string.Empty)
            {
                SysFunc.UserMsg(LbErr, Color.Red, "Counter Sale Date should not blank.");
                return;
            }
            if (Convert.ToDateTime(SysFunc.SaveDate(txtCounterDate.Text)).Date > Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd")).Date)
            {
                SysFunc.UserMsg(LbErr, Color.Red, "Counter sale Date should not greater then current date.");
                return;
            }
            if (ddlCustomer.SelectedIndex == 0)
            {
                SysFunc.UserMsg(LbErr, Color.Red, "Select customer before save ");
                return;
            }
            else LbErr.Text = "";
            if (ddlTaxType.SelectedIndex == 0)
            {
                SysFunc.UserMsg(LbErr, Color.Red, "Select Tax Type before save ");
                return;
            }
            else LbErr.Text = "";
            if (ddlPaymentType.SelectedIndex == 0)
            {
                SysFunc.UserMsg(LbErr, Color.Red, "Select Payment Type before save ");
                return;
            }
            else LbErr.Text = "";
            foreach (GridViewRow row in GvMain.Rows)
            {
                Label lkn = (Label)row.FindControl("lblItemCode");
                if (lkn.Text == "")
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Details should not be left blank...");
                    return;
                }

            }
        
            //else LbErr.Text = "";
            if (ddlCounterNo.SelectedIndex == 0)
            {
                Insert_MasterDetail();
                if (ddlPaymentType.SelectedValue == "Cash")
                {
                    if (ddlReceiptMode.SelectedValue == "Auto") {
                        string strReceiptNo = string.Empty;
                        PaymentReceiptEntry(ref strReceiptNo);
                        SysFunc.UserMsg(LbErr, Color.Green, LbErr.Text + "<br />" + " Payment Receipt no. " + strReceiptNo + " has been added successfully : ");
                    }
                   
                }
            }
            else
            {
                Update_MasterDetail();
            }

            ClearAll();
        }
        private void Insert_MasterDetail()
        {
            SqlTransaction Trans = null;
            bool Chek = false;
            try
            {
                if (ObjTrans.BeginTransaction(ref Trans) == false)
                {
                    return;
                }
                Label lblTotalGSTAmt = (Label)GvMain.FooterRow.FindControl("lblTotalGSTAmt");
                Label lblTotalExAmt = (Label)GvMain.FooterRow.FindControl("lblTotalExAmt");
                Label lblTotalFurAmt = (Label)GvMain.FooterRow.FindControl("lblTotalFurAmt");
                Label lblFooterTotalAmt = (Label)GvMain.FooterRow.FindControl("lblFooterTotalAmt");
                Label lblTotalDiscount = (Label)GvMain.FooterRow.FindControl("lblTotalDiscount");


                SqlParameter[] param = 
                                    {
                                         new SqlParameter("@DealerCode",SqlDbType.Char, 5),//0
                                         new SqlParameter("@SaleInvNo",SqlDbType.Char, 8),//1
                                         new SqlParameter("@SaleInvDate",SqlDbType.DateTime),//2
                                         new SqlParameter("@ReqNO",SqlDbType.Char),//3
                                         new SqlParameter("@CusCode",SqlDbType.Char, 8),//4
                                         new SqlParameter("@Remarks",SqlDbType.VarChar, 70),//5
                                         new SqlParameter("@Amount",SqlDbType.Decimal),//6
                                         new SqlParameter("@Discount",SqlDbType.Decimal),//7
                                         new SqlParameter("@OtherCharges",SqlDbType.Decimal),//8
                                         new SqlParameter("@FurtherAmount",SqlDbType.Decimal),//9
                                         new SqlParameter("@GSTAmount",SqlDbType.Decimal),//10
                                         new SqlParameter("@InvoiceAmount",SqlDbType.Decimal),//11
                                         new SqlParameter("@TotReceipt",SqlDbType.Decimal),//12
                                         new SqlParameter("@DelFlag",SqlDbType.Char, 1),//13
                                         new SqlParameter("@PostFlag",SqlDbType.Char, 1),//14
                                         new SqlParameter("@UpdUser",SqlDbType.VarChar),//15
                                         //new SqlParameter("@UpdDate",SqlDbType.DateTime),//16
                                         //new SqlParameter("@UpdTime",SqlDbType.DateTime),//17
                                         new SqlParameter("@UpdTerm",SqlDbType.VarChar),//16
                                         new SqlParameter("@TaxType",SqlDbType.VarChar),//17
                                         new SqlParameter("@ExTaxTotalAmount",SqlDbType.Decimal),//18
                                         new SqlParameter("@PaymentType",SqlDbType.VarChar, 20),//19
                                       new SqlParameter("@FreightCharges",SqlDbType.Float),//20
                                        new SqlParameter("@ReceiptMode",SqlDbType.VarChar),//21
                                    };


                param[0].Value = Session["DealerCode"].ToString();
                //txtCounterNo.Text = ObjGeneral.AutoGen("CounterSaleMaster", "SaleInvNo", txtCounterDate.Text.Trim());
                GlobalVar.mDealerCode = Session["DealerCode"].ToString();
                
                string txtCounterNo = SysFunc.AutoGen("UniqueInvoiceNo", "InvoiceNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                Session["CSINumber"] = txtCounterNo;
                txtRequisitionNo.Text = SysFunc.AutoGen("SIRMaster", "SIRno", txtCounterDate.Text.Trim());
                param[1].Value = txtCounterNo.Trim();
                param[2].Value = SysFunc.SaveDate(txtCounterDate.Text);
                param[3].Value = txtRequisitionNo.Text;
                param[4].Value = ddlCustomer.SelectedValue.ToString();
                param[5].Value = txtRemarks.Text.Trim().Replace(",", " ");
                param[6].Value = Convert.ToDecimal((lblFooterTotalAmt.Text == "" ? "0" : lblFooterTotalAmt.Text));
                param[7].Value = Convert.ToDecimal(lblTotalDiscount.Text.Trim() == "" ? "0" : lblTotalDiscount.Text.Trim());
                param[8].Value = Convert.ToDecimal(txtOtherChargers.Text.Trim() == "" ? "0" : txtOtherChargers.Text.Trim());
                param[9].Value = Convert.ToDecimal(lblTotalFurAmt.Text.Trim() == "" ? "0" : lblTotalFurAmt.Text.Trim());
                param[10].Value = Convert.ToDecimal(lblTotalGSTAmt.Text.Trim() == "" ? "0" : lblTotalGSTAmt.Text.Trim());
                param[11].Value = Convert.ToDecimal(txtNetAmount.Text.Trim() == "" ? "0" : txtNetAmount.Text.Trim());
                param[12].Value = 0.0;
                param[13].Value = "N";
                param[14].Value = "N";
                param[15].Value = Session["UserName"].ToString();
                //param[16].Value = ObjGeneral.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
                //param[17].Value = ObjGeneral.SaveTime(DateTime.Now.ToString("hh:mm"));
                param[16].Value = "";
                param[17].Value = ddlTaxType.SelectedItem.Text ;
                param[18].Value = Convert.ToDecimal(lblTotalExAmt.Text.Trim() == "" ? "0" : lblTotalExAmt.Text.Trim());
                param[19].Value = ddlPaymentType.SelectedItem.Text;
                param[20].Value = Convert.ToDecimal(txtFreightCharges.Text.Trim() == "" ? "0" : txtFreightCharges.Text.Trim());
                param[21].Value = ddlReceiptMode.SelectedItem.Text;
                if (SysFunc.SP_Spare_Insert_CounterSaleMaster(Trans, param))
                {
                    DataSet ds = (DataSet)Session["dsParts"];
                    DataTable dt = ds.Tables[0];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SqlParameter[] param1 = 
                      {
                         new SqlParameter("@DealerCode",SqlDbType.Char, 10),//0
                         new SqlParameter("@SaleInvNo",SqlDbType.Char, 10),//1
                         new SqlParameter("@ItemCode",SqlDbType.Char, 10),//2
                         new SqlParameter("@SaleQty",SqlDbType.Decimal),//3
                         new SqlParameter("@Rate",SqlDbType.Decimal),//4
                         new SqlParameter("@Amount",SqlDbType.Decimal),//5
                         new SqlParameter("@GSTPercent",SqlDbType.Decimal),//6
                         new SqlParameter("@GSTAmount",SqlDbType.Decimal),//7
                         new SqlParameter("@FurtherPercent",SqlDbType.Decimal),//8
                         new SqlParameter("@FurtherAmount",SqlDbType.Decimal),//9
                         new SqlParameter("@ExTaxPerc",SqlDbType.Decimal),//10
                         new SqlParameter("@ExTaxAmount",SqlDbType.Decimal),//11
                         new SqlParameter("@DiscountPerc",SqlDbType.Decimal),//12
                         new SqlParameter("@DiscountAmount",SqlDbType.Decimal),//13
                      };

                        //Label LbPartItemNo = (Label)GvMain.Rows[i].FindControl("LbPartItemNo");
                        //Label LbQuantity = (Label)GvMain.Rows[i].FindControl("LbQuantity");
                        //Label LbRate = (Label)GvMain.Rows[i].FindControl("LbRate");
                        //Label LbAmount = (Label)GvMain.Rows[i].FindControl("LbAmount");


                        param1[0].Value = Session["DealerCode"].ToString();
                        param1[1].Value = txtCounterNo;
                        param1[2].Value = dt.Rows[i]["ItemCode"].ToString();
                        param1[3].Value = Convert.ToDecimal(dt.Rows[i]["Qty"].ToString().Trim() == "" ? "0" : dt.Rows[i]["Qty"].ToString().Trim());
                        param1[4].Value = Convert.ToDecimal(dt.Rows[i]["SaleRate"].ToString().Trim() == "" ? "0" : dt.Rows[i]["SaleRate"].ToString().Trim());
                        param1[5].Value = Convert.ToDecimal(dt.Rows[i]["TotalAmount"].ToString().Trim() == "" ? "0" : dt.Rows[i]["TotalAmount"].ToString().Trim());
                        param1[6].Value = Convert.ToDecimal(dt.Rows[i]["GSTage"].ToString().Trim() == "" ? "0" : dt.Rows[i]["GSTage"].ToString().Trim());
                        param1[7].Value = Convert.ToDecimal(dt.Rows[i]["GSTAmount"].ToString().Trim() == "" ? "0" : dt.Rows[i]["GSTAmount"].ToString().Trim());
                        param1[8].Value = Convert.ToDecimal(dt.Rows[i]["FGSTPerc"].ToString().Trim() == "" ? "0" : dt.Rows[i]["FGSTPerc"].ToString().Trim());
                        param1[9].Value = Convert.ToDecimal(dt.Rows[i]["FGSTAmt"].ToString().Trim() == "" ? "0" : dt.Rows[i]["FGSTAmt"].ToString().Trim());
                        param1[10].Value = Convert.ToDecimal(dt.Rows[i]["SalesExTaxPerc"].ToString().Trim() == "" ? "0" : dt.Rows[i]["SalesExTaxPerc"].ToString().Trim());
                        param1[11].Value = Convert.ToDecimal(dt.Rows[i]["SalesExTaxAmt"].ToString().Trim() == "" ? "0" : dt.Rows[i]["SalesExTaxAmt"].ToString().Trim());
                        param1[12].Value = Convert.ToDecimal(dt.Rows[i]["DiscountPerc"].ToString().Trim() == "" ? "0" : dt.Rows[i]["DiscountPerc"].ToString().Trim());
                        param1[13].Value = Convert.ToDecimal(dt.Rows[i]["DiscountAmt"].ToString().Trim() == "" ? "0" : dt.Rows[i]["DiscountAmt"].ToString().Trim());
                        if (SysFunc.SP_Spare_Insert_CounterSaleDetail(Trans, param1))
                        {
                            Chek = true;
                        }
                        else
                        {
                            Chek = false;
                            break;
                        }
                    }

                    Chek = UpdateRequisition( Trans);

                    if (Chek == false)
                    {
                        ObjTrans.RollBackTransaction(ref Trans);
                        SysFunc.UserMsg(LbErr, Color.Red, "Recored not saved try again  ");
                    }
                    else
                    {
                        ObjTrans.CommittTransaction(ref Trans);
                        SysFunc.UserMsg(LbErr, Color.Green, "Saved Successfully Sales Invoice No. " + txtCounterNo);
                        Session["dsParts"] = null;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);
                    }
                }


            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                //ObjGeneral.UserMsg(LbErr, Color.Red, ex.Message, txtCounterNo);
            }

        }
        private bool UpdateRequisition(SqlTransaction Trans)
        {
            string StrWhere1 = "", StrLubeFlag = "", TType = "";
            bool Check = false;
            try
            {


                //if (Oper == "Add")
                //{

                SqlParameter[] param = 
                                    {
                                         new SqlParameter("@DealerCode",SqlDbType.Char, 5),//0
                                         new SqlParameter("@SIRNo",SqlDbType.Char, 8),//1
                                         new SqlParameter("@SIRDate",SqlDbType.DateTime),//2
                                         new SqlParameter("@Remarks",SqlDbType.VarChar),//3
                                         new SqlParameter("@RequsitionerCode",SqlDbType.Char, 8),//4
                                         new SqlParameter("@Requsitioner",SqlDbType.Char, 8),//5

                                         new SqlParameter("@InvoiceNO",SqlDbType.Char),//6
                                         new SqlParameter("@JobCardNO",SqlDbType.Char),//7
                                         new SqlParameter("@UpdUser",SqlDbType.VarChar),//8
                                         //new SqlParameter("@UpdDate",SqlDbType.DateTime),//9
                                         //new SqlParameter("@UpdTime",SqlDbType.DateTime),//10
                                         new SqlParameter("@UpdTerm",SqlDbType.VarChar)//11
                                    };


                param[0].Value = Session["DealerCode"].ToString();
                //txtCounterNo.Text = ObjGeneral.AutoGen("CounterSaleMaster", "SaleInvNo", txtCounterDate.Text.Trim());
                //txtRequisitionNo.Text = ObjGeneral.AutoGen("SIRMaster", "SIRno", txtCounterDate.Text.Trim());
                param[1].Value = txtRequisitionNo.Text;
                param[2].Value = SysFunc.SaveDate(txtCounterDate.Text);
                param[3].Value = txtRemarks.Text.Trim().Replace(",", " ");
                param[4].Value = "";
                param[5].Value = "C";
                param[6].Value = Session["CSINumber"].ToString();
                param[7].Value = null;
                param[8].Value = Session["UserName"].ToString();
                //param[9].Value = ObjGeneral.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
                //param[10].Value = ObjGeneral.SaveTime(DateTime.Now.ToString("hh:mm"));
                param[9].Value = GlobalVar.mUserIPAddress;


                if (SysFunc.SP_Spare_Insert_SIRMaster(Trans, param))
                {
                    DataSet ds = (DataSet)Session["dsParts"];
                    DataTable dt = ds.Tables[0];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SqlParameter[] param1 = 
                      {
                         new SqlParameter("@DealerCode",SqlDbType.Char, 10),//0
                         new SqlParameter("@SIRNo",SqlDbType.Char, 10),//1
                         new SqlParameter("@ItemCode",SqlDbType.Char, 10),//2
                         new SqlParameter("@Quantity",SqlDbType.Decimal),//3
                         
                         new SqlParameter("@RecQty",SqlDbType.Decimal),//4
                         new SqlParameter("@TType",SqlDbType.Char),//5
                      };

                        //Label LbPartItemNo = (Label)GvMain.Rows[i].FindControl("LbPartItemNo");
                        //Label LbQuantity = (Label)GvMain.Rows[i].FindControl("LbQuantity");
                        //Label LbRate = (Label)GvMain.Rows[i].FindControl("LbRate");
                        //Label LbAmount = (Label)GvMain.Rows[i].FindControl("LbAmount");
                        //Label LbRecQty = (Label)GvMain.Rows[i].FindControl("LbRecQty");

                        StrWhere1 = "ItemCode = '" + dt.Rows[i]["ItemCode"].ToString() + "' AND [DealerCode] = '" + Session["DealerCode"].ToString() + "'";
                        StrLubeFlag = SysFunc.GetStringValuesAgainstCodes(StrWhere1, "Item", "LubeFlag");

                        if (StrLubeFlag == "N")
                        {
                            TType = "P";
                        }
                        else
                        {
                            TType = "L";
                        }

                        param1[0].Value = Session["DealerCode"].ToString();
                        param1[1].Value = txtRequisitionNo.Text;
                        param1[2].Value = dt.Rows[i]["ItemCode"].ToString();
                        param1[3].Value = Convert.ToDecimal(dt.Rows[i]["Qty"].ToString().Trim() == "" ? "0" : dt.Rows[i]["Qty"].ToString().Trim());

                        param1[4].Value = dt.Rows[i]["RecQty"].ToString();
                        param1[5].Value = TType;


                        if (SysFunc.SP_Spare_Insert_SIRDetail(Trans, param1))
                        {
                            Check = true;
                        }
                        else
                        {
                            Check = false;
                            break;
                        }
                    }
                }
                return Check;

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            
            LbErr.Text = "";

            ClearAll();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);
        }

        private void ClearAll()
        {
            SysFunc.ClearTextBoxes(Page);
            SysFunc.Clearddl(Page);
            txtCounterDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            createPartsDT();
            Load_ddlCounterNo();
        }

        protected void btnPosting_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2546", "005"))
            {
                Response.Redirect("~/Test.aspx");
            }
            SqlTransaction Trans = null;
            bool Check = false;
            try
            {
                
                if (ObjTrans.BeginTransaction(ref Trans) == false)
                {
                    return;
                }
                DataSet ds = (DataSet)Session["dsParts"];
                DataTable dt = ds.Tables[0];

                if (ddlCounterNo.SelectedIndex == 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Select Counter No. before post");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Details should not be left blank...");
                    return;
                }
                
                if(SysFunc.IsPosted("CounterSaleMaster","SaleInvNo",ddlCounterNo.SelectedValue.ToString(),Session["DealerCode"].ToString()))
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "The Invoice is already posted; now you could not change/delete/Post the record...");
                    return;
                }
                //if (myFunc.CodeExists("CounterSaleMaster", "SaleInvNo", ddCounterSalesNo.Text, " and DelFlag ='N'", Session["DealerCode"].ToString(), "") == false)
                //{
                //    lblMSG.Text = "Invoice No. does not exist...";
                //    PopupControlMSG.ShowOnPageLoad = true;
                //    //LbErr.Text = "";
                //    //txtCounterNo.Focus();
                //    return;
                //}


                //if (SysFunc.IsPosted("CounterSaleMaster", "SaleInvNo", ddCounterSalesNo.Text, Session["DealerCode"].ToString()) == true)
                //{
                //    lblMSG.Text = "The Invoice is already posted; now you could not change/delete/Post the record...";
                //    PopupControlMSG.ShowOnPageLoad = true;
                //    //LbErr.Text = "";
                //    //txtCounterNo.Focus();
                //    return;
                //}


                //Check RecQuty and IssuQty is equal
                string StrReq = string.Empty;
                ds = new DataSet();
                StrReq = SysFunc.GetStringValuesAgainstCodes("SaleInvNo", ddlCounterNo.SelectedValue.ToString(), "ReqNO", "CounterSaleMaster",Session["DealerCode"].ToString());
                SysFunc.ExecuteQuery("Select * From SIRDetail Where Dealercode='" + Session["DealerCode"].ToString() + "' And SIRNo='" + StrReq + "'", ref ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(dr["Quantity"]) != Convert.ToInt32(dr["RecQty"]))
                        {
                            SysFunc.UserMsg(LbErr, Color.Red, "Stock not issue yet " + ddlCounterNo.SelectedValue.ToString());
                            //lblMSG.Text = "Stock not issue yet " + ddCounterSalesNo.Text;
                            
                            //ObjGeneral.UserMsg(LbErr, Color.Red, "Stock not issue yet", txtCounterNo);
                            return;
                        }
                    }
                }
                ///////

                //if (myFunc.IsIssued("CounterSaleMaster", "SaleInvNo", ddCounterSalesNo.Text, Session["DealerCode"].ToString()) == false)
                //{
                //    lblMSG.Text = "Issuance is not completed against the Invoice so the invoice could not be posted...";
                //    PopupControlMSG.ShowOnPageLoad = true;

                //    //ObjGeneral.UserMsg(LbErr, Color.Red, "");
                //    //txtCounterNo.Focus();
                //    return;
                //}


                if (Posting(Trans) == true)
                {
                    Check = true;
                }
                else
                {
                    Check = false;
                }


                if (Check == false)
                {
                    ObjTrans.RollBackTransaction(ref Trans);
                    SysFunc.UserMsg(LbErr, Color.Red, "Record not posted Please try again");
                }
                else
                {
                    ObjTrans.CommittTransaction(ref Trans);
                    SysFunc.UserMsg(LbErr, Color.Green, "Record posted successfully");
                    ClearAll();
                }

            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                SysFunc.UserMsg(LbErr, Color.Green, ex.Message);
            }
        }
        private bool Posting(SqlTransaction Trans)
        {
            double DGSTage = 0.0;
            //SqlTransaction Trans;
            bool Check = false;
                //if (ObjTrans.BeginTransaction(ref Trans) == false)
                //{
                //    return;
                //}

                if (UpdatePost(Trans, ddlCounterNo.SelectedValue.ToString()) == false)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Invoice information could not be Posted...");
                    Check = false;
                }
                else
                {
                    Label lblTotalGSTAmt = (Label)GvMain.FooterRow.FindControl("lblTotalGSTAmt");
                    DGSTage = Convert.ToDouble(lblTotalGSTAmt.Text == "" ? "0" : lblTotalGSTAmt.Text);
                    if (DGSTage > 0.0)
                    {
                        if (UpdateGSTInvoiceNO(Trans) == true)
                        {
                            Check = true;
                        }
                        else
                        {
                            Check = false;
                        }
                    }
                    else
                    {
                        Check = true;
                    }

                }

                return Check;
            
        }
        private bool UpdateGSTInvoiceNO(SqlTransaction Trans)
        {
            bool Check = false;
            
                txtGSTInvoice.Text = SysFunc.AutoGen("UniqueSalesTaxInvoice", "SalesTaxInvNo", txtCounterDate.Text);
                //txtGSTInvoice.Text = SysFunc.AutoGen("GSTInvoice", "GSTInvNo", txtCounterDate.Text);

                SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char),
                                    new SqlParameter("@GSTInvNo",SqlDbType.Char),
                                    new SqlParameter("@GSTInvDate",SqlDbType.DateTime),
                                    new SqlParameter("@RefNo",SqlDbType.Char)
                                    
                                   };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = txtGSTInvoice.Text.Trim();
                param[2].Value = SysFunc.SaveDate(txtCounterDate.Text);
                param[3].Value = ddlCounterNo.SelectedValue.ToString().ToString();

                if (SysFunc.SP_Spare_Insert_GSTInvoice(Trans, param) == true)
                {
                    Check = true;
                }
                else
                {
                    Check = false;
                }

                SqlParameter[] param1 = {
                                    new SqlParameter("@GSTInvoice",SqlDbType.Char),
                                    new SqlParameter("@CounterCode",SqlDbType.Char),
                                    new SqlParameter("@DEALER_CODE",SqlDbType.Char)
                                   };

                param1[0].Value = txtGSTInvoice.Text.Trim();
                param1[1].Value = ddlCounterNo.SelectedValue.ToString();
                param1[2].Value = Session["DealerCode"].ToString();

                if (Check == true)
                {
                    if (SysFunc.SP_Spare_update_CounterSaleGSTInvoice(Trans, param1) == true)
                    {
                        Check = true;
                    }
                    else
                    {
                        Check = false;
                    }
                }
                return Check;
            
        }
        private bool UpdatePost(SqlTransaction Trans, string CounterCode)
        {
            bool Check = false;
            
                SqlParameter[] param1 = {
                                    new SqlParameter("@CounterCode",SqlDbType.Char),
                                    new SqlParameter("@DEALER_CODE",SqlDbType.Char)
                                   };

                param1[0].Value = ddlCounterNo.SelectedValue.ToString();
                param1[1].Value = Session["DealerCode"].ToString();


                if (SysFunc.SP_Spare_Update_UpdatePost(Trans, param1) == true)
                {
                    Check = true;
                }
                else
                {
                    Check = true;
                }

                return Check;
            
        }
        private void Update_MasterDetail()
        {
            SqlTransaction Trans = null;
            bool Chek = false;
            try
            {
                if (ObjTrans.BeginTransaction(ref Trans) == false)
                {
                    return;
                }
                Label lblTotalGSTAmt = (Label)GvMain.FooterRow.FindControl("lblTotalGSTAmt");
                Label lblTotalExAmt = (Label)GvMain.FooterRow.FindControl("lblTotalExAmt");
                Label lblTotalFurAmt = (Label)GvMain.FooterRow.FindControl("lblTotalFurAmt");
                Label lblFooterTotalAmt = (Label)GvMain.FooterRow.FindControl("lblFooterTotalAmt");
                Label lblTotalDiscount = (Label)GvMain.FooterRow.FindControl("lblTotalDiscount");
                SqlParameter[] param = 
                                    {
                                         new SqlParameter("@DealerCode",SqlDbType.Char, 5),//0
                                         new SqlParameter("@SaleInvNo",SqlDbType.Char, 8),//1
                                         new SqlParameter("@SaleInvDate",SqlDbType.DateTime),//2
                                         new SqlParameter("@ReqNO",SqlDbType.Char),//3
                                         new SqlParameter("@CusCode",SqlDbType.Char, 8),//4
                                         new SqlParameter("@Remarks",SqlDbType.VarChar, 70),//5
                                         new SqlParameter("@Amount",SqlDbType.Decimal),//6
                                         new SqlParameter("@Discount",SqlDbType.Decimal),//7
                                         new SqlParameter("@OtherCharges",SqlDbType.Decimal),//8
                                         new SqlParameter("@FurtherAmount",SqlDbType.Decimal),//9
                                         new SqlParameter("@GSTAmount",SqlDbType.Decimal),//10
                                         new SqlParameter("@InvoiceAmount",SqlDbType.Decimal),//11
                                         new SqlParameter("@TotReceipt",SqlDbType.Decimal),//12
                                         new SqlParameter("@DelFlag",SqlDbType.Char, 1),//13
                                         new SqlParameter("@PostFlag",SqlDbType.Char, 1),//14
                                         new SqlParameter("@UpdUser",SqlDbType.VarChar),//15
                                         //new SqlParameter("@UpdDate",SqlDbType.DateTime),//16
                                         //new SqlParameter("@UpdTime",SqlDbType.DateTime),//17
                                         new SqlParameter("@UpdTerm",SqlDbType.VarChar),//16
                                         new SqlParameter("@TaxType",SqlDbType.VarChar),//17
                                         new SqlParameter("@ExTaxTotalAmount",SqlDbType.Decimal),//18
                                         new SqlParameter("@PaymentType",SqlDbType.VarChar, 20),//19
                                         new SqlParameter("@FreightCharges",SqlDbType.Float, 20),//19

                                    };
                param[0].Value = Session["DealerCode"].ToString();
                //txtCounterNo.Text = ObjGeneral.AutoGen("CounterSaleMaster", "SaleInvNo", txtCounterDate.Text.Trim());
                //txtRequisitionNo.Text = ObjGeneral.AutoGen("SIRMaster", "SIRno", txtCounterDate.Text.Trim());
                //Session["CSINumber"] = 
                param[1].Value = ddlCounterNo.SelectedValue.ToString();
                param[2].Value = SysFunc.SaveDate(txtCounterDate.Text);
                param[3].Value = txtRequisitionNo.Text;
                param[4].Value = ddlCustomer.SelectedValue.ToString();
                param[5].Value = txtRemarks.Text.Trim().Replace(",", " ");
                param[6].Value = Convert.ToDecimal((lblFooterTotalAmt.Text == "" ? "0" : lblFooterTotalAmt.Text));
                param[7].Value = Convert.ToDecimal(lblTotalDiscount.Text.Trim() == "" ? "0" : lblTotalDiscount.Text.Trim());
                param[8].Value = Convert.ToDecimal(txtOtherChargers.Text.Trim() == "" ? "0" : txtOtherChargers.Text.Trim());
                param[9].Value = Convert.ToDecimal(lblTotalFurAmt.Text.Trim() == "" ? "0" : lblTotalFurAmt.Text.Trim());
                param[10].Value = Convert.ToDecimal(lblTotalGSTAmt.Text.Trim() == "" ? "0" : lblTotalGSTAmt.Text.Trim());
                param[11].Value = Convert.ToDecimal(txtNetAmount.Text.Trim() == "" ? "0" : txtNetAmount.Text.Trim());
                param[12].Value = 0.0;
                param[13].Value = "N";
                param[14].Value = "N";
                param[15].Value = Session["UserName"].ToString();
                //param[16].Value = ObjGeneral.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
                //param[17].Value = ObjGeneral.SaveTime(DateTime.Now.ToString("hh:mm"));
                param[16].Value = "";
                param[17].Value = ddlTaxType.SelectedItem.Text;
                param[18].Value = Convert.ToDecimal(lblTotalExAmt.Text.Trim() == "" ? "0" : lblTotalExAmt.Text.Trim());
                param[19].Value = ddlPaymentType.SelectedItem.Text;
                param[20].Value = Convert.ToDecimal(txtFreightCharges.Text.Trim() == "" ? "0" : txtFreightCharges.Text.Trim());

                if (SysFunc.SP_Spare_Update_CounterSaleMaster(Trans, param))
                {
                    DataSet ds = (DataSet)Session["dsParts"];
                    DataTable dt = ds.Tables[0];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SqlParameter[] param1 = 
                      {
                         new SqlParameter("@DealerCode",SqlDbType.Char, 10),//0
                         new SqlParameter("@SaleInvNo",SqlDbType.Char, 10),//1
                         new SqlParameter("@ItemCode",SqlDbType.Char, 10),//2
                         new SqlParameter("@SaleQty",SqlDbType.Decimal),//3
                         new SqlParameter("@Rate",SqlDbType.Decimal),//4
                         new SqlParameter("@Amount",SqlDbType.Decimal),//5
                         new SqlParameter("@GSTPercent",SqlDbType.Decimal),//6
                         new SqlParameter("@GSTAmount",SqlDbType.Decimal),//7
                         new SqlParameter("@FurtherPercent",SqlDbType.Decimal),//8
                         new SqlParameter("@FurtherAmount",SqlDbType.Decimal),//9
                         new SqlParameter("@ExTaxPerc",SqlDbType.Decimal),//10
                         new SqlParameter("@ExTaxAmount",SqlDbType.Decimal),//11
                         new SqlParameter("@DiscountPerc",SqlDbType.Decimal),//12
                         new SqlParameter("@DiscountAmount",SqlDbType.Decimal),//13
                      };

                        //Label LbPartItemNo = (Label)GvMain.Rows[i].FindControl("LbPartItemNo");
                        //Label LbQuantity = (Label)GvMain.Rows[i].FindControl("LbQuantity");
                        //Label LbRate = (Label)GvMain.Rows[i].FindControl("LbRate");
                        //Label LbAmount = (Label)GvMain.Rows[i].FindControl("LbAmount");


                        param1[0].Value = Session["DealerCode"].ToString();
                        param1[1].Value = ddlCounterNo.SelectedValue.ToString();
                        param1[2].Value = dt.Rows[i]["ItemCode"].ToString();
                        param1[3].Value = Convert.ToDecimal(dt.Rows[i]["Qty"].ToString().Trim() == "" ? "0" : dt.Rows[i]["Qty"].ToString().Trim());
                        param1[4].Value = Convert.ToDecimal(dt.Rows[i]["SaleRate"].ToString().Trim() == "" ? "0" : dt.Rows[i]["SaleRate"].ToString().Trim());
                        param1[5].Value = Convert.ToDecimal(dt.Rows[i]["TotalAmount"].ToString().Trim() == "" ? "0" : dt.Rows[i]["TotalAmount"].ToString().Trim());
                        param1[6].Value = Convert.ToDecimal(dt.Rows[i]["GSTage"].ToString().Trim() == "" ? "0" : dt.Rows[i]["GSTage"].ToString().Trim());
                        param1[7].Value = Convert.ToDecimal(dt.Rows[i]["GSTAmount"].ToString().Trim() == "" ? "0" : dt.Rows[i]["GSTAmount"].ToString().Trim());
                        param1[8].Value = Convert.ToDecimal(dt.Rows[i]["FGSTPerc"].ToString().Trim() == "" ? "0" : dt.Rows[i]["FGSTPerc"].ToString().Trim());
                        param1[9].Value = Convert.ToDecimal(dt.Rows[i]["FGSTAmt"].ToString().Trim() == "" ? "0" : dt.Rows[i]["FGSTAmt"].ToString().Trim());
                        param1[10].Value = Convert.ToDecimal(dt.Rows[i]["SalesExTaxPerc"].ToString().Trim() == "" ? "0" : dt.Rows[i]["SalesExTaxPerc"].ToString().Trim());
                        param1[11].Value = Convert.ToDecimal(dt.Rows[i]["SalesExTaxAmt"].ToString().Trim() == "" ? "0" : dt.Rows[i]["SalesExTaxAmt"].ToString().Trim());
                        param1[12].Value = Convert.ToDecimal(dt.Rows[i]["DiscountPerc"].ToString().Trim() == "" ? "0" : dt.Rows[i]["DiscountPerc"].ToString().Trim());
                        param1[13].Value = Convert.ToDecimal(dt.Rows[i]["DiscountAmt"].ToString().Trim() == "" ? "0" : dt.Rows[i]["DiscountAmt"].ToString().Trim());


                        if (SysFunc.SP_Spare_Insert_CounterSaleDetail(Trans, param1))
                        {
                            Chek = true;
                        }
                        else
                        {
                            Chek = false;
                            break;
                        }
                    }

                    Chek = UpdateRequisition("Edit", Trans);
                    if (Chek == false)
                    {
                        ObjTrans.RollBackTransaction(ref Trans);
                        SysFunc.UserMsg(LbErr, Color.Red, "Recored not updated try again  ");
                    }
                    else
                    {
                        ObjTrans.CommittTransaction(ref Trans);
                        SysFunc.UserMsg(LbErr, Color.Green, "Sale Inv No. "+ddlCounterNo.SelectedValue.ToString()+" Updated Successfuly ");
                    }
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }

        }
        private bool UpdateRequisition(string Oper, SqlTransaction Trans)
        {
            string StrWhere1 = "", StrLubeFlag = "", TType = "";
            bool Check = false;
            try
            {


                //if (Oper == "Add")
                //{

                SqlParameter[] param = 
                                    {
                                         new SqlParameter("@DealerCode",SqlDbType.Char, 5),//0
                                         new SqlParameter("@SIRNo",SqlDbType.Char, 8),//1
                                         new SqlParameter("@SIRDate",SqlDbType.DateTime),//2
                                         new SqlParameter("@Remarks",SqlDbType.VarChar),//3
                                         new SqlParameter("@RequsitionerCode",SqlDbType.Char, 8),//4
                                         new SqlParameter("@Requsitioner",SqlDbType.Char, 8),//5

                                         new SqlParameter("@InvoiceNO",SqlDbType.Char),//6
                                         new SqlParameter("@JobCardNO",SqlDbType.Char),//7
                                         new SqlParameter("@UpdUser",SqlDbType.VarChar),//8
                                         //new SqlParameter("@UpdDate",SqlDbType.DateTime),//9
                                         //new SqlParameter("@UpdTime",SqlDbType.DateTime),//10
                                         new SqlParameter("@UpdTerm",SqlDbType.VarChar)//11
                                    };


                param[0].Value = Session["DealerCode"].ToString();
                //txtCounterNo.Text = ObjGeneral.AutoGen("CounterSaleMaster", "SaleInvNo", txtCounterDate.Text.Trim());
                //txtRequisitionNo.Text = ObjGeneral.AutoGen("SIRMaster", "SIRno", txtCounterDate.Text.Trim());
                param[1].Value = txtRequisitionNo.Text;
                param[2].Value = SysFunc.SaveDate(txtCounterDate.Text);
                param[3].Value = txtRemarks.Text.Trim().Replace(",", " ");
                param[4].Value = "";
                param[5].Value = "C";
                param[6].Value = ddlCounterNo.SelectedValue.ToString();
                param[7].Value = null;
                param[8].Value = Session["UserName"].ToString();
                //param[9].Value = ObjGeneral.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
                //param[10].Value = ObjGeneral.SaveTime(DateTime.Now.ToString("hh:mm"));
                param[9].Value = GlobalVar.mUserIPAddress;


                if (SysFunc.SP_Spare_Insert_SIRMaster(Trans, param))
                {
                    DataSet ds = (DataSet)Session["dsParts"];
                    DataTable dt = ds.Tables[0];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SqlParameter[] param1 = 
                      {
                         new SqlParameter("@DealerCode",SqlDbType.Char, 10),//0
                         new SqlParameter("@SIRNo",SqlDbType.Char, 10),//1
                         new SqlParameter("@ItemCode",SqlDbType.Char, 10),//2
                         new SqlParameter("@Quantity",SqlDbType.Decimal),//3
                         
                         new SqlParameter("@RecQty",SqlDbType.Decimal),//4
                         new SqlParameter("@TType",SqlDbType.Char),//5
                      };

                        //Label LbPartItemNo = (Label)GvMain.Rows[i].FindControl("LbPartItemNo");
                        //Label LbQuantity = (Label)GvMain.Rows[i].FindControl("LbQuantity");
                        //Label LbRate = (Label)GvMain.Rows[i].FindControl("LbRate");
                        //Label LbAmount = (Label)GvMain.Rows[i].FindControl("LbAmount");
                        //Label LbRecQty = (Label)GvMain.Rows[i].FindControl("LbRecQty");

                        StrWhere1 = "ItemCode = '" + dt.Rows[i]["ItemCode"].ToString() + "' AND [DealerCode] = '" + Session["DealerCode"].ToString() + "'";
                        StrLubeFlag = SysFunc.GetStringValuesAgainstCodes(StrWhere1, "Item", "LubeFlag");

                        if (StrLubeFlag == "N")
                        {
                            TType = "P";
                        }
                        else
                        {
                            TType = "L";
                        }

                        param1[0].Value = Session["DealerCode"].ToString();
                        param1[1].Value = txtRequisitionNo.Text;
                        param1[2].Value = dt.Rows[i]["ItemCode"].ToString();
                        param1[3].Value = Convert.ToDecimal(dt.Rows[i]["Qty"].ToString().Trim() == "" ? "0" : dt.Rows[i]["Qty"].ToString().Trim());

                        param1[4].Value = dt.Rows[i]["RecQty"].ToString();
                        param1[5].Value = TType;


                        if (SysFunc.SP_Spare_Insert_SIRDetail(Trans, param1))
                        {
                            Check = true;
                        }
                        else
                        {
                            Check = false;
                            break;
                        }
                    }
                }


                return Check;

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ddlCounterNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMode.Text = "Edit Mode";
            FillForm();
            ClacLiters();
        }
        private void FillForm()
        {
            DataSet dsForm = new DataSet();
            try
            {


                SqlParameter[] param = {
                new SqlParameter("@SaleInvNo",SqlDbType.Char,8),
                new SqlParameter("@DealerCode",SqlDbType.Char,5)
               
            };
                param[0].Value = ddlCounterNo.SelectedValue.ToString();
                param[1].Value = Session["DealerCode"].ToString();

                dsForm = SysFunc.SP_Spare_Get_CounterSaleMaster(param);

                if (dsForm.Tables[0].Rows.Count != 0)
                {
                    txtRequisitionNo.Text = dsForm.Tables[0].Rows[0]["ReqNo"].ToString();
                    ddlCustomer.SelectedValue = dsForm.Tables[0].Rows[0]["CusCode"].ToString();
                    txtCounterDate.Text = dsForm.Tables[0].Rows[0]["SaleInvDate"].ToString();
                    //txtCustomerDesc.Text = dsForm.Tables[0].Rows[0]["CusDesc"].ToString();
                    txtRemarks.Text = dsForm.Tables[0].Rows[0]["Remarks"].ToString();
                    ddlPaymentType.SelectedValue = dsForm.Tables[0].Rows[0]["PaymentType"].ToString().Trim();
                    ddlTaxType.SelectedValue = dsForm.Tables[0].Rows[0]["TaxType"].ToString().Trim();
                    txtOtherChargers.Text = dsForm.Tables[0].Rows[0]["OtherCharges"].ToString();
                    txtFreightCharges.Text = dsForm.Tables[0].Rows[0]["FreightCharges"].ToString();
                    GridNetAmount.Text = dsForm.Tables[0].Rows[0]["Amount"].ToString();
                    txtGSTInvoice.Text = dsForm.Tables[0].Rows[0]["SalesTaxInvNo"].ToString();
                    lblPostFlag.Text = dsForm.Tables[0].Rows[0]["PostFlag"].ToString();
                    txtVoucherNo.Text = dsForm.Tables[0].Rows[0]["VoucherNo"].ToString();
                    ddlReceiptMode.SelectedValue = dsForm.Tables[0].Rows[0]["ReceiptMode"].ToString();
                    //HDDeleted.Value = "1";

                    //ObjPOds.DataTable1.Rows.Clear();
                    FillGV();
                    //CalMainTotal();
                }
            }
            catch (Exception ex)
            {
               // ObjGeneral.UserMsg(LbErr, Color.Red, ex.Message, txtCounterNo);
            }
        }

        private void FillGV()
        {
            try
            {
                //DataSet dsMain = new DataSet();
                //DataSet ds = new DataSet();
               
           

                SqlParameter[] param = {
                new SqlParameter("@SaleInvNo",SqlDbType.Char,10) ,
                new SqlParameter("@dealerCode",SqlDbType.Char,5)
            };
                param[0].Value = ddlCounterNo.SelectedValue.ToString();
                param[1].Value = Session["DealerCode"].ToString();

                dsGVMain = SysFunc.SP_Spare_Get_CounterSaleDetail(param);
                GvMain.DataSource = dsGVMain.Tables[0];
                GvMain.DataBind();
                
                Session["dsParts"] = dsGVMain;
                totalCalculation();

                //ds = (DataSet)ViewState["ObjPOds"];
                //ds.Tables[0].Load(dsMain.CreateDataReader());
                }
            catch (Exception ex)
            {

                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2546", "004"))
            {
                Response.Redirect("~/Test.aspx");
            }
            DataSet dsPostPO = new DataSet();
            SqlTransaction Trans = null;
            bool Check = false;
            string SIRCheck = "";
            try
            {
                if (SysFunc.CheckVoucherPostFlag(Session["DealerCode"].ToString(), txtVoucherNo.Text))
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Can't Edit or Delete the Voucher '" + txtVoucherNo.Text + "' is already Posted");
                    return;
                }
             
                if (ObjTrans.BeginTransaction(ref Trans) == false)
                {
                    return;
                }
                DataSet ds = (DataSet)Session["dsParts"];
                DataTable dt = ds.Tables[0];

                if (ddlCounterNo.SelectedIndex == 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Select Counter No. before post");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Details should not be left blank...");
                    return;
                }

                //if (SysFunc.ValuesAgainstCodes("SaleInvNo", ddlCounterNo.SelectedValue, "VoucherNo", "CounterSaleMaster", "", Session["DealerCode"].ToString()))
                //{
                //    SysFunc.UserMsg(LbErr, Color.Red, "Voucher Exists can't delete the record.");
                //    return;
                //}
                if (SysFunc.CodeExists("PaymentReceiptDetail", "InvoiceNo", ddlCounterNo.SelectedValue.ToString().Trim(), " and InvoiceType='Service'") == true)
                {

                    SysFunc.UserMsg(LbErr, Color.Red, "Invoice No. can not be deleted, payment receipt has made");
                    //lblMSGPop.Text ="Invoice No. can not be deleted, payment receipt has made" ;
                    //PopupControlMSG.ShowOnPageLoad = true;
                    return;
                }

                if (SysFunc.IsPosted("CounterSaleMaster", "SaleInvNo", ddlCounterNo.SelectedValue.ToString(), Session["DealerCode"].ToString()))
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "The Invoice is already posted; now you could not change/delete/Post the record...");
                    return;
                }

                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                   
                   

                    if (SysFunc.IsAnyStockIssuedAgainstSIR(txtRequisitionNo.Text, dt.Rows[i]["ItemCode"].ToString(), Session["DealerCode"].ToString()))
                    {
                        SysFunc.UserMsg(LbErr, Color.Red, "Stock Issued Against This Counter Sales, Delete Not Allowed");

                        SIRCheck = "1";
                        break;
                    }
                    else
                    {
                        SIRCheck = "0";
                    }
                }

                if (SIRCheck == "1")
                {
                    return;
                }

             

                SqlParameter[] param2 = {
                                    new SqlParameter("@SaleInvNo",SqlDbType.Char),
                                    new SqlParameter("@DEALER_CODE",SqlDbType.Char)
                                   };

                param2[0].Value = ddlCounterNo.SelectedValue.ToString().Trim();
                param2[1].Value = Session["DealerCode"].ToString();

                if (SysFunc.SP_Spare_Delete_CounterSaleMaster(Trans, param2))
                {
                    string sql1 = "Update GatePassTemp Set DelFlag='Y' Where DealerCode='" + Session["DealerCode"].ToString() + "' And TransCode='" + ddlCounterNo.SelectedValue.Trim() + "' and Module='Spare Part'";
                    SysFunc.ExecuteQuery(sql1, Trans);
                    Check = true;
                }
                else
                {
                    Check = false;
                }

                if (Check == true)
                {

                    if (SysFunc.SP_Spare_Delete_CounterSaleDetail(Trans, param2))
                    {
                        Check = true;
                    }
                    else
                    {
                        Check = false;
                    }
                }

                if (Check == false)
                {
                    ObjTrans.RollBackTransaction(ref Trans);
                    SysFunc.UserMsg(LbErr,Color.Red, "Record not deleted");

                }
                else
                {
                    ObjTrans.CommittTransaction(ref Trans);
                    SysFunc.UpdateJV(Session["DealerCode"].ToString(), txtVoucherNo.Text);
                    SysFunc.UserMsg(LbErr, Color.Red, "Sales Invoice " + ddlCounterNo.SelectedValue.ToString() + " has been deleted");
                    ClearAll();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Deletealert()", true);
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }

        protected void BtnPrint_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2546", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }

            if (ddlCounterNo.SelectedIndex == 0)
            {
                SysFunc.UserMsg(LbErr, Color.Red, "Please select GRN No.");
                return;
            }
            ReportDocument RD = new ReportDocument();
            DXBMS.Data.DSReports objDsReports = new Data.DSReports();

            DataTable dt = new DataTable();
            string sql = "exec SP_Spare_Get_rptGSTInvoiceCounterSale '" + Session["DealerCode"].ToString() + "','" + ddlCounterNo.SelectedValue.ToString().Trim() + "'";
            dt = SysFunc.GetData(sql);
            objDsReports.SP_Spare_Get_rptGSTInvoiceCounterSale.Load(dt.CreateDataReader());
            RD.PrintOptions.PaperSize = PaperSize.PaperA4;
            RD.Load(Server.MapPath("../SpareReports/rptCounterSale.rpt"));
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
            if (lblPostFlag.Text.Trim() == "N") RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'" + "Counter Sale Invoice" + "'";
            else RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'" + "Invoice/Sales Tax Invoice" + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            //Invoice/Sales Tax Invoice ReportTitle
            RD.SetDataSource(objDsReports);
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
            string URL = "../../../Download/PrintReport.aspx";

            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";

            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

            ////string URL;
            ////URL = "../../../../Download/OpenPdf.aspx?FileName=" + FileName;

            //////URL = FilePath + "OpenPdf.aspx?FileName=" + FileName;
            //////txtPartItemDesc.Text = URL;
            ////string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ////URL = "../../../../Download/OpenPdf.aspx?FileName=" + FileName;
            ////ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);




            //if (ddlCounterNo.SelectedIndex == 0)
            //{
            //    SysFunc.UserMsg(LbErr, Color.Red, "Please select GRN No.");
            //    return;
            //}
            //ReportDocument RD = new ReportDocument();
            //DXBMS.Data.DSReports objDsReports = new Data.DSReports();

            //DataTable dt = new DataTable();
            //string sql = "exec SP_Spare_Get_rptGSTInvoiceCounterSale '" + Session["DealerCode"].ToString() + "','" + ddlCounterNo.SelectedValue.ToString().Trim() + "'";
            //dt = SysFunc.GetData(sql);
            //objDsReports.SP_Spare_Get_rptGSTInvoiceCounterSale.Load(dt.CreateDataReader());
            //RD.PrintOptions.PaperSize = PaperSize.PaperA4;


            ////RD.Load(Server.MapPath("../../SpareReports/rptPartsReceivedDetail.rpt"));
            //RD.Load(Server.MapPath("../SpareReports/rptCounterSale.rpt"));
            ////RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            ////RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            ////RD.DataDefinition.FormulaFields["NTN"].Text = "'" + Session["DealerNTN"].ToString() + "'";
            ////RD.DataDefinition.FormulaFields["SaleTaxNo"].Text = "'" + Session["DealerSaleTaxNo"].ToString() + "'";

            ////RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            ////RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";

            ////RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
            ////RD.DataDefinition.FormulaFields["FromCode"].Text = "'" + FDate + "'";
            ////RD.DataDefinition.FormulaFields["ToCode"].Text = "'" + TDate + "'";
            ////RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
            ////RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "GRN PRINT" + "\"";
            ////RD.Database.Tables[0].SetDataSource(objDsReports);
            ////RD.Database.Tables[0].SetDataSource(dt);
            //RD.SetDataSource(objDsReports);

            //// convert and show
            //string FilePath = Server.MapPath("~") + "\\Download\\";
            //string FileName = "CounterSaleInvoice'" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            //string File = FilePath + FileName;

            //RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            //string URL;
            //URL = "../../../Download/OpenPdf.aspx?FileName=" + FileName;
            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);


        }

        protected void btnLookup_Click(object sender, EventArgs e)
        {
            
            ViewState["lookupid"] = 10; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_Parts_PO(imgLookup, ViewState["lookupid"].ToString(), "", "../../../");

            ScriptManager.RegisterClientScriptBlock(imgLookup, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }

        protected void ddlDiscount_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            ds= (DataSet)Session["dsParts"];
            foreach (DataRow dr in ds.Tables[0].Rows) // search whole table
            {

                if (ddlDiscount.SelectedIndex == 0)
                {
                    dr["DiscountPerc"] = "0";
                    dr["DiscountAmt"] = "0";
                }
                else
                {
                    if (ds.Tables[0].Rows[0][1].ToString().Trim() != string.Empty)
                    {
                        decimal dDiscount = 0;
                        decimal dRate = Convert.ToDecimal(dr["SaleRate"].ToString());

                        dDiscount = dRate * Convert.ToDecimal(ddlDiscount.SelectedItem.Value.ToString());
                        dr["DiscountAmt"] = dDiscount.ToString();
                        dr["DiscountPerc"] = ddlDiscount.SelectedItem.Text.Substring(0, ddlDiscount.SelectedItem.Text.Trim().Length - 1);
                        dr.AcceptChanges();
                    }
                }

            }
            ds.AcceptChanges();
            Session["dsParts"] = ds;
            GvMain.DataSource = ds.Tables[0];
            GvMain.DataBind();
            totalCalculation();


        }

        protected void SelectedPartDetail(string item)
        {
            try
            {
                if (btnValue == 1)
                {
                    ddlCounterNo.SelectedValue = item;
                    ddlCounterNo_SelectedIndexChanged(null, null);
                }
                else if (btnValue == 2)
                {
                    ddlCustomer.SelectedValue = item;

                }
                else { 
                DataTable dt = new DataTable();
                dt = SysFunc.GetData("select ItemDesc,SaleRate,PartItemNo from Item where DealerCode in('COMON','" + Session["DealerCode"].ToString() + "') And  ItemCode='" + item + "'");
                txtItemCode.Text = item;
                txtItemDesc.Text = dt.Rows[0]["ItemDesc"].ToString().Trim();
                txtItemNo.Text = dt.Rows[0]["PartItemNo"].ToString().Trim();
                txtPartPrice.Text = dt.Rows[0]["SaleRate"].ToString().Trim();
            }
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }

        private void ClacLiters()
        {
            decimal DCalcLiter = 0;
            foreach (GridViewRow gvr in GvMain.Rows)
            {
                Label lblPartItemNo = (Label)gvr.Cells[2].FindControl("lblPartItemNo");
                Label LbUnitDesc = (Label)gvr.Cells[2].FindControl("LbUnitDesc");
                Label lblAccQty = (Label)gvr.Cells[4].FindControl("lblAccQty");
                if (LbUnitDesc.Text.Trim () == "LITRE")
                {
                    string strLiterFromItem = SysFunc.GetStringValuesAgainstCodes("DealerCode='" + Session["DealerCode"].ToString() + "' And PartItemNo='" + lblPartItemNo.Text.Trim() + "' ", "Item", "Liter");
                    decimal DConvertLiter = Convert.ToDecimal(strLiterFromItem == "" ? "0" : strLiterFromItem);
                    DCalcLiter = DCalcLiter + (DConvertLiter * Convert.ToDecimal(lblAccQty.Text.Trim()));

                }

            }
            txtTotalLiters.Text = DCalcLiter.ToString();
        }

        private void clearFields()
        {
            txtItemCode.Text = "";
            txtItemDesc.Text = "";
            txtItemNo.Text =   "";
            txtPartPrice.Text = "";
            txtQuantity.Text = "";
        }

        protected void btnGL_Click(object sender, EventArgs e)
        {
           

            if (ddlReceiptMode.SelectedValue == "Auto")
            {
                if (ddlPaymentType.Text.Trim() == "Cash")
                {
                    URL = "../../Service/Forms/GL.aspx?CusInv=" + ddlCounterNo.SelectedValue + "&Type=DS";
                }
            }
          
            else
            {
                URL = "../../Service/Forms/JV.aspx?CusInv=" + ddlCounterNo.SelectedValue + "&Type=DS";
            }            

            string fullURL = "window.open('" + URL + "', '_blank', 'height=600,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }


        private void PaymentReceiptEntry(ref string strReceiptNo)
        {

            try
            {
                if (Inert_PaymentReceiptMaster(ref strReceiptNo))
                {

                    SqlParameter[] PmtRecDetail_param = {                                            
                                                            /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                            /*1*/ new SqlParameter("@ReceiptNo",SqlDbType.Char,8),
                                                            /*2*/ new SqlParameter("@InvoiceType",SqlDbType.VarChar,20),
                                                            /*3*/ new SqlParameter("@InvoiceNo",SqlDbType.Char,8),
                                                            /*4*/ new SqlParameter("@InvoiceDate",SqlDbType.DateTime),
                                                            /*5*/ new SqlParameter("@InvAmount",SqlDbType.Decimal),
                                                            /*6*/ new SqlParameter("@OutStAmount",SqlDbType.Decimal),
                                                            /*7*/ new SqlParameter("@AdjAmount",SqlDbType.Decimal)
                                                        };

                    PmtRecDetail_param[0].Value = Session["DealerCode"].ToString();
                    PmtRecDetail_param[1].Value = strReceiptNo;
                    PmtRecDetail_param[2].Value = "CountrSale";
                    PmtRecDetail_param[3].Value = Session["CSINumber"].ToString();
                    PmtRecDetail_param[4].Value = SysFunc.SaveDate(txtCounterDate.Text);
                    PmtRecDetail_param[5].Value =SysFunctions.CustomCDBL( txtNetAmount.Text);
                    PmtRecDetail_param[6].Value = SysFunctions.CustomCDBL(txtNetAmount.Text);
                    PmtRecDetail_param[7].Value = SysFunctions.CustomCDBL(txtNetAmount.Text);


                    SysFunc.ExecuteSP_NonQuery("[sp_W2_PaymentReceipt_Detail_Insert]", PmtRecDetail_param, Trans);
                    //Updation Of invoice

                    //SqlParameter[] PmtRecTaxDetail_param = {                                            
                    //                                        /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
                    //                                        /*1*/ new SqlParameter("@ReceiptNo",SqlDbType.Char,8),
                    //                                        /*2*/ new SqlParameter("@ReceiptHead",SqlDbType.Char,50),
                    //                                        /*3*/ new SqlParameter("@AccountCode",SqlDbType.Char,1),
                    //                                        /*4*/ new SqlParameter("@Amount",SqlDbType.Decimal),
                    //                                        /*5*/ new SqlParameter("@TaxID",SqlDbType.Char,2),
                    //                                        /*6*/ new SqlParameter("@TaxPerc",SqlDbType.Decimal),
                    //                                       };

                    //PmtRecTaxDetail_param[0].Value = Session["DealerCode"].ToString();
                    //PmtRecTaxDetail_param[1].Value = strReceiptNo;
                    //foreach (GridViewRow row in gvTaxDetail.Rows)
                    //{
                    //    Label lblRecHead = (Label)row.FindControl("lblRecHead");
                    //    Label lblAmount = (Label)row.FindControl("lblAmount");
                    //    Label lblTaxID = (Label)row.FindControl("lblTaxID");
                    //    Label lblPercent = (Label)row.FindControl("lblPercent");
                    //    if (lblRecHead.Text != "")
                    //    {
                    //        PmtRecTaxDetail_param[2].Value = lblRecHead.Text;
                    //        PmtRecTaxDetail_param[3].Value = "";
                    //        PmtRecTaxDetail_param[4].Value = Convert.ToDecimal(lblAmount.Text.Trim());
                    //        PmtRecTaxDetail_param[5].Value = lblTaxID.Text.Trim();
                    //        PmtRecTaxDetail_param[6].Value = Convert.ToDecimal(lblPercent.Text.Trim());
                    //        SysFuncs.ExecuteSP_NonQuery("[sp_W2_PaymentReceipt_TaxDetail_Insert]", PmtRecTaxDetail_param, Trans);
                    //    }
                    //}


                }
                else
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Operation Failed! Record Not Saved.");
                    ObjTrans.RollBackTransaction(ref Trans);
                    return;
                }

                if (ObjTrans.CommittTransaction(ref Trans))
                {
                    string IQuery = "Update CounterSaleMaster set TotReceipt = '" + txtNetAmount.Text + "' " +
                      "Where DealerCode='" + Session["DealerCode"].ToString() + "' and SaleInvNo ='" + ddlCounterNo.SelectedValue + "'";

                    SysFunc.ExecuteQuery_NonQuery(IQuery);
                }
                LbErr.Visible = true;
                //myFunc.UserMsg(lblMsg, Color.Green, "Record Saved Successfully: " + strReceiptNo);

            }
            catch (Exception ex) { ObjTrans.RollBackTransaction(ref Trans); throw ex; }
        }
        private bool Inert_PaymentReceiptMaster(ref string strReceiptNo)
        {
            //PaymentReceiptEntry2(1);

            SqlParameter[] PmtRecMaster_param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),          /*1*/ new SqlParameter("@ReceiptNo",SqlDbType.Char,8),
           /*2*/ new SqlParameter("@ReceiptDate",SqlDbType.DateTime),       /*3*/ new SqlParameter("@InvoiceType",SqlDbType.VarChar,20),
           /*4*/ new SqlParameter("@CusCode",SqlDbType.VarChar,8),          /*5*/ new SqlParameter("@InsCompCode",SqlDbType.Char,4),
           /*6*/ new SqlParameter("@BranchCode",SqlDbType.Char,4),          /*7*/ new SqlParameter("@InsCusFlag",SqlDbType.Char,1),
           /*8*/ new SqlParameter("@Remarks",SqlDbType.Char,100),           /*9*/ new SqlParameter("@PayModeCode",SqlDbType.Char,1),
           /*10*/ new SqlParameter("@InsNo",SqlDbType.VarChar,15),          /*11*/ new SqlParameter("@InsDate",SqlDbType.DateTime),
           /*12*/ new SqlParameter("@AmountPaid",SqlDbType.Float),          /*13*/ new SqlParameter("@BankCode",SqlDbType.Char,5),
           /*14*/ new SqlParameter("@Branch",SqlDbType.VarChar,50),         /*15*/ new SqlParameter("@AdvanceAmount",SqlDbType.Float),
           /*16*/ new SqlParameter("@InvTotal",SqlDbType.Float),            /*17*/ new SqlParameter("@OutSTTotal",SqlDbType.Float),
           /*18*/ new SqlParameter("@InvAdjTotal",SqlDbType.Float),         /*19*/ new SqlParameter("@DelFlag",SqlDbType.Char,1),
           /*20*/ new SqlParameter("@UpdUser",SqlDbType.Char,50),           /*21*/ new SqlParameter("@UpdTerm",SqlDbType.Char,50),          
           /*22*/ new SqlParameter("@VoucherNo",SqlDbType.VarChar,50),      /*23*/ new SqlParameter("@VoucherFlag",SqlDbType.Char,1),
           /*24*/ new SqlParameter("@AdvancePaid",SqlDbType.Decimal),       /*25*/ new SqlParameter("@TransType",SqlDbType.VarChar,30),
           /*26*/ new SqlParameter("@IsAdjustAdvance",SqlDbType.Char,1),
           /*27*/ new SqlParameter("@AdvanceReceiptNo",SqlDbType.Char,8),
           /*28*/ new SqlParameter("@AdvanceAdjustedAmount",SqlDbType.Float),
           /*29*/ new SqlParameter("@AdvanceBalanceAmount",SqlDbType.Float),
            /*30*/ new SqlParameter("@DocumentNo",SqlDbType.VarChar,50),
            };

            PmtRecMaster_param[0].Value = Session["DealerCode"].ToString();
            PmtRecMaster_param[2].Value = SysFunc.SaveDate(txtCounterDate.Text); //EstimateCode        
            PmtRecMaster_param[3].Value = "CountrSale";
            PmtRecMaster_param[4].Value = ddlCustomer.SelectedValue;
            PmtRecMaster_param[5].Value = "";
            PmtRecMaster_param[6].Value = "";
            PmtRecMaster_param[7].Value = "N";
            PmtRecMaster_param[8].Value = txtRemarks.Text;
            PmtRecMaster_param[9].Value = "C";
            PmtRecMaster_param[10].Value = "";

            PmtRecMaster_param[11].Value = (object)DBNull.Value;
            PmtRecMaster_param[12].Value = txtNetAmount.Text;
            PmtRecMaster_param[13].Value = "";
            PmtRecMaster_param[14].Value = "";
            PmtRecMaster_param[15].Value = "0";
            PmtRecMaster_param[16].Value = SysFunctions.CustomCDBL(txtNetAmount.Text);
            PmtRecMaster_param[17].Value = SysFunctions.CustomCDBL(txtNetAmount.Text);
            PmtRecMaster_param[18].Value = SysFunctions.CustomCDBL(txtNetAmount.Text);
            PmtRecMaster_param[19].Value = "N";

            PmtRecMaster_param[20].Value = Session["UserName"].ToString();
            PmtRecMaster_param[21].Value = GlobalVar.mUserIPAddress;

            PmtRecMaster_param[22].Value = "";
            PmtRecMaster_param[23].Value = "N";
            PmtRecMaster_param[24].Value = "0";
            PmtRecMaster_param[25].Value = "Customer";

            PmtRecMaster_param[26].Value = "N";
            PmtRecMaster_param[27].Value = (object)DBNull.Value;
            PmtRecMaster_param[28].Value = (object)DBNull.Value;
            PmtRecMaster_param[29].Value = (object)DBNull.Value;
            PmtRecMaster_param[30].Value = (object)DBNull.Value;

            try
            {
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    strReceiptNo = SysFunc.AutoGen("PaymentReceiptMaster", "ReceiptNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                    PmtRecMaster_param[1].Value = strReceiptNo;
                    if (SysFunc.ExecuteSP_NonQuery("[sp_W2_PaymentReceipt_Master_Insert]", PmtRecMaster_param, Trans))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
                return false;
            }
        }

        protected void btnGatePass_Click(object sender, EventArgs e)
        {
            try
            {
                string oldgatepass,postflag;
                oldgatepass = SysFunc.GetStringValuesAgainstCodes("Module='Spare Part' and TransCode='" + ddlCounterNo.SelectedValue.ToString() + "'", "GatePassTemp", "GatePassCode");
                postflag = SysFunc.GetStringValuesAgainstCodes("SaleInvNo='" + ddlCounterNo.SelectedValue.ToString() + "'", "CounterSaleMaster", "PostFlag");
                if (ddlCounterNo.SelectedIndex == 0)
                {
                    lblImp.Text = "Please Select the Invoice First!";
                    lblImp.ForeColor = Color.Red;
                    return;
                }

                if (postflag=="N")
                {
                    lblImp.Text = "Please Post the Record First!";
                    lblImp.ForeColor = Color.Red;
                    return;

                }
                if (oldgatepass == null|| oldgatepass=="")
                {


                    SqlParameter[] GatePass_param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5), 
           
           /*1*/ new SqlParameter("@GatePassCode",SqlDbType.Char,8),
          // /*2*/ new SqlParameter("@GatePassDate",SqlDbType.DateTime),     
           /*4*/ new SqlParameter("@TransCode",SqlDbType.VarChar,8),       
           /*3*/ new SqlParameter("@GatePassType",SqlDbType.Char,1),         
           /*4*/ new SqlParameter("@Remarks",SqlDbType.Char,100),          
            /*6*/ new SqlParameter("@Module",SqlDbType.VarChar,50),            
           /*7*/ new SqlParameter("@UpdUser",SqlDbType.Char,50),
            /*9*/ new SqlParameter("@UpdTerm",SqlDbType.Char,50),
           /*8*/ new SqlParameter("@DelFlag",SqlDbType.Char,1),
           /*4*/ new SqlParameter("@InvoiceNo",SqlDbType.VarChar,8),

            };
                    string gatepass = SysFunc.AutoGen("GatePassTemp", "GatePassCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                    GatePass_param[0].Value = Session["DealerCode"].ToString();
                    GatePass_param[1].Value = gatepass;
                    GatePass_param[2].Value = ddlCounterNo.SelectedValue.ToString();
                    GatePass_param[3].Value = "N";
                    GatePass_param[4].Value = "";
                    GatePass_param[5].Value = "Spare Part";
                    GatePass_param[6].Value = Session["UserName"].ToString();
                    GatePass_param[7].Value = GlobalVar.mUserIPAddress;
                    GatePass_param[8].Value = "N";
                    GatePass_param[9].Value = ddlCounterNo.SelectedValue.ToString();
                    if (SysFunc.ExecuteSP_NonQuery("[Sp_Insert_GatePassTemp]", GatePass_param))
                    {
                        lblImp.Text = "Gate Created " + gatepass;
                        lblImp.ForeColor = Color.Green;
                        MakeGatePassReport(gatepass);
                    }
                }
                else
                {
                    MakeGatePassReport(oldgatepass);
                }
            }
            catch (Exception ex)
            {
                lblImp.Text = ex.Message;
            }
        }
        private void MakeGatePassReport(string Gatepass)
        {
            ReportDocument RD = new ReportDocument();
            DXBMS.Data.DSReports objDsReports = new Data.DSReports();

            DataTable dt = new DataTable();
            string sql = "exec SP_Spare_Get_rptGatePass '" + Session["DealerCode"].ToString() + "','" + Gatepass + "'";
            dt = SysFunc.GetData(sql);
            objDsReports.SP_Spare_Get_rptGatePass.Load(dt.CreateDataReader());
            RD.PrintOptions.PaperSize = PaperSize.PaperA4;
            RD.Load(Server.MapPath("../SpareReports/rptCounterSaleGatePass.rpt"));
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
            if (lblPostFlag.Text.Trim() == "N") RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'" + "Counter Sale Invoice" + "'";
            else RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'" + "Invoice/Sales Tax Invoice" + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            //Invoice/Sales Tax Invoice ReportTitle
            RD.SetDataSource(objDsReports);
            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "CounterSaleInvoice" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;
            //RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            Session["RD"] = RD;

            string URL;
            URL = "../../../Download/rptViewerService.aspx?FileName=" + FileName;

            //URL = FilePath + "OpenPdf.aspx?FileName=" + FileName;
            //txtPartItemDesc.Text = URL;
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //URL = "../../../Download/OpenPdf.aspx?FileName=" + FileName; 
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

        }

        protected void ImageButtonDS_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 42; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_CounterSalesNo(ImageButtonDS, ViewState["lookupid"].ToString(), "", "../../../");

            btnValue = 1;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void ImageButtonCustomer_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 1; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_Customer(ImageButtonCustomer, ViewState["lookupid"].ToString(), "", "../../../");

            btnValue = 2;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }
    }
}