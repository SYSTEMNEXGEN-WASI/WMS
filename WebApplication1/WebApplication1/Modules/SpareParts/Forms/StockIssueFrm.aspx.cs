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


namespace DXBMS.Modules.SpareParts.Forms
{
    public partial class StockIssueFrm : System.Web.UI.Page
    {
        Transaction ObjTrans = new Transaction();
        SysFunctions SysFunc = new SysFunctions();
        SecurityBll sec = new SecurityBll();
        MainBLL ObjMain = new MainBLL();
        DataSet dsGVMain = new DataSet();
        clsLookUp clslook = new clsLookUp();
        static int btnValue = 0;
        string FormState = string.Empty;
        string strIssueNo = string.Empty;
        string strTotalQty = string.Empty;
        string strTotalAmount = string.Empty;
       // string strSubmitBehavior = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2547", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (Page.IsPostBack)
            {

                if (!string.IsNullOrEmpty((string)Session["LookUpData"]))
                {
                    SelectedPartDetail(Session["LookUpData"].ToString());
                }
            }
            if (Page.IsPostBack == false)
            {

                if (Session["TransCode"] != null)
                {
                    txtIssueDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    Load_ddlSirNo();
                    createPartsDT();
                    Load_ddlCounterNo();
                    string leadId = Session["TransCode"].ToString();
                    ddlIssueNo.SelectedValue = leadId;

                    ddlIssueNo_SelectedIndexChanged(null, null);


                }
                else
                {
                    lblMode.Text = "Add Mode";
                    txtIssueDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    Load_ddlSirNo();
                    createPartsDT();
                    Load_ddlCounterNo();
                }
            }
            btnValue = 0;
            Session["TransCode"] = null;
            Session["LookUpData"] = string.Empty;


        }
        private void createPartsDT()
        {
            dsGVMain = new DataSet();
            dsGVMain.Tables.Add();

            dsGVMain.Tables[0].Columns.Add(new DataColumn("ItemCode", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("PartItemNo", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("ItemDesc", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("Location", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("Quantity", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("BalQty", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("OLStock", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("issueQty", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("AvgCost", typeof(string)));
            dsGVMain.Tables[0].Columns.Add(new DataColumn("Amount", typeof(string)));
            
            //DataRow dr = dsGVMain.Tables[0].NewRow();
            //dsGVMain.Tables[0].Rows.Add(dr);

            GvMain.DataSource = dsGVMain.Tables[0];
            GvMain.DataBind();
            Session["dsParts"] = dsGVMain;
        }
        private void Load_ddlCounterNo()
        {
            string WhereClause = " a.DealerCode = '" + Session["DealerCode"].ToString() + "' " +
 "and a.DelFlag = 'N' " +
 "and a.DealerCode = b.DealerCode And a.IssueNo = b.IssueNo "; 
            string[] Columns = new string[] { "a.IssueNo", "Convert(varchar(10),a.IssueDate,105)", "+'JC: '+IsNull(a.JobCardNo,'')", "+'CS: '+IsNull(a.InvoiceNo,'')" };
            SysFunc.GetMultiColumnsDDL(ddlIssueNo, Columns, "StockIssue a,StockIssueDetail b", WhereClause, "IssueNo", " Order by a.IssueNo Desc ", true, false);
        }
        private void Load_ddlSirNo()
        {
            SqlParameter[] param = 
                                    {
                                         new SqlParameter("@DealerCode",SqlDbType.Char, 5),//0
                                    };
            param[0].Value = Session["DealerCode"].ToString();
            ObjMain.FillDrp_SP(ddlSirNo, "sp_Select_SIRMaster_onStockIssue", "SIRNo", "name", param, true, "Select", false, string.Empty);
            
    //        string WhereClause = "a.DealerCode = b.DealerCode And "+
    //            " a.SIRNo = b.SIRNo " +
    //            "And a.DealerCode = c.DealerCode " +
    //            "AND a.InvoiceNo = c.SaleInvNo And "+
    //            " a.DealerCode = '" + Session["DealerCode"].ToString() + "' " +
    //             "And a.Requsitioner = 'C' " +
    //"AND a.InvoiceNo <> '' " +
    //"AND c.DelFlag = 'N' " +
    //"AND b.Quantity <> b.RecQty ";
    //        string FromClause = "SIRMaster a ,SIRDetail b,CounterSaleMaster c ";
 
    //        string[] Columns = new string[] { "a.SIRNo", "convert(varchar(10),a.SIRDate,105)", "IsNull(a.JobCardNo, '')", "a.InvoiceNO" };
    //        SysFunc.GetMultiColumnsDDL(ddlSirNo, Columns, FromClause, WhereClause, "SIRNo", " Order by SIRNo Desc ", true, false);
        }

        protected void ddlSirNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormState="New";
            
            GetDateSIR();
            AddNewRow();
        }
        private void AddNewRow()
        {
            SqlDataReader dr = null;
            //DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            //DropDownList ddlParts = (DropDownList)GvMain.FooterRow.FindControl("ddlParts");
            //TextBox txtFooterQty = (TextBox)GvMain.FooterRow.FindControl("txtFooterQty");

            SqlParameter[] param = {new SqlParameter("@DealerCode",SqlDbType.Char,5 ),
                                               new SqlParameter("@SIRNo",SqlDbType.Char,8),
                                           };
            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = ddlSirNo.SelectedValue.ToString();
            SysFunc.ExecuteSP("Sp_SirDetail_Items_Select", param, ref dr);
            if (dr.HasRows)
            {
                dt.Load(dr);
                GvMain.DataSource = dt;
                GvMain.DataBind();
            }
            else
            {
                createPartsDT();
            }
            //ds = (DataSet)Session["dsParts"];
            //DataRow row = ds.Tables[0].NewRow();
            //foreach (DataRow drTable in dt.Rows)
            //{
            //    ////Get OH stock
            //    //DataSet dsOHStock = new DataSet();
            //    //dsOHStock = SysFunc.PartStock(drTable["ItemCode"].ToString(), Session["DealerCode"].ToString());
            //    //decimal DTotalStock = Convert.ToDecimal(dsOHStock.Tables[0].Rows[0]["Balance"]);
            //    ///////////////////////////////////////////////////////
            //    //Get Avg Cost
            //    DataSet dsAvgCost = new DataSet();
            //    decimal DAvgCost = 0;
            //    dsAvgCost = SysFunc.ItemAvgCost(drTable["ItemCode"].ToString(), Session["DealerCode"].ToString());
            //    if (dsAvgCost.Tables[0].Rows.Count != 0)
            //    {
            //        DAvgCost = Convert.ToDecimal(dsAvgCost.Tables[0].Rows[0]["Cost"]);
            //    }
            //    //////////////////////////////////////////////////////

            //    row["ItemCode"] = drTable["ItemCode"].ToString();
            //    row["PartItemNo"] = drTable["PartItemNo"].ToString();
            //    row["ItemDesc"] = drTable["ItemDesc"].ToString();
            //    row["Location"] = drTable["Location"].ToString();
            //    row["Quantity"] = drTable["Quantity"].ToString();
            //    row["BalQty"] = drTable["BalQty"].ToString();
            //    //row["OHStock"] = "0";
            //    row["OLStock"] = "0";
            //    row["AvgCost"] = DAvgCost.ToString();
            //    row["Amount"] = "0";


            //    ds.Tables[0].Rows.Add(row);
            //}
            
            //GvMain.DataSource = dt;
            //GvMain.DataBind();
            //Session["dsParts"] = ds;

            //////////////////////
        }
        private void GetDateSIR()
        {
            DataSet dsSIR = new DataSet();
            try
            {
                if (ddlSirNo.SelectedIndex != 0)
                {
                    SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char),
                                    new SqlParameter("@SIRNO",SqlDbType.Char)
                                   };

                    param[0].Value = Session["DealerCode"].ToString();
                    param[1].Value = ddlSirNo.SelectedValue.ToString();
                    dsSIR = SysFunc.SP_Parts_Get_SIRDataGetForStockIssue(param);

                    if (Convert.ToString(dsSIR.Tables[0].Rows[0]["Requsitioner"]) == "J")
                    {
                        lblSource.Text = "Job Card #";
                        txtJC_CS_No.Text = dsSIR.Tables[0].Rows[0]["JobCardNO"].ToString();
                        
                    }
                    else if (Convert.ToString(dsSIR.Tables[0].Rows[0]["Requsitioner"]) == "C")
                    {
                        lblSource.Text = "Counter Sale #";
                        txtJC_CS_No.Text = dsSIR.Tables[0].Rows[0]["InvoiceNO"].ToString();
                    }
                    txtSource.Text = dsSIR.Tables[0].Rows[0]["Requsitioner"].ToString();
                    txtSIRDate.Text = dsSIR.Tables[0].Rows[0]["SIRDate"].ToString();
                    txtRemarks.Focus();
                }
            }
            catch (Exception ex)
            {
                LbErr.Text = ex.Message;
            }
        }
        private void Get_locationWisePartData(string StrItemCode,string strLocationCode,ref string strLocationQty)
        {
            double  DStockLoc = 0.0;
            DataSet ds2 = new DataSet();
            try
            {   
                    ds2 = SysFunc.PartStockAtLoaction(StrItemCode, Session["DealerCode"].ToString(),strLocationCode);
                    DStockLoc = Convert.ToDouble(ds2.Tables[0].Rows[0]["Balance"]);
                    strLocationQty = Convert.ToString(DStockLoc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void GvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && !(GvMain.EditIndex == e.Row.RowIndex))
            {
                SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char),
                                   new SqlParameter("@ItemCode",SqlDbType.Char)};
                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = e.Row.Cells[1].Text;
                DropDownList ddlLocation = (DropDownList)e.Row.FindControl("ddlLocation");
                ObjMain.FillDrp_SP(ddlLocation, "sp_2W_Location_Select", "PartsTransLocCode", "PartsTransLocDesc", param,true, "Select", false, "");
                Label LbPartsTransLocCode = (Label)e.Row.FindControl("LbPartsTransLocCode");
                Label lblIssueQty = (Label)e.Row.FindControl("lblIssueQty");
                TextBox txtIssueQty=(TextBox)e.Row.FindControl("txtIssueQty");
                //string strTotals;
                //decimal dtotalvalue;
                ////Sum of GRN Qty
                //strTotals = lblIssueQty.Text;
                //dtotalvalue = Convert.ToDecimal(strTotals);
                //sumFooterGRNQty += dtotalvalue;
                if (FormState == "New")
                {
                    LbPartsTransLocCode.Visible = false;
                    lblIssueQty.Visible=false;
                    ddlLocation.Visible=true;
                    txtIssueQty.Visible=true;
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[5].Visible = true;
                    e.Row.Cells[6].Visible = true;
                    e.Row.Cells[7].Visible = true;

                }
                else if (FormState == "Saved")
                {
                    LbPartsTransLocCode.Visible = true;
                    lblIssueQty.Visible = true;
                    ddlLocation.Visible = false;
                    txtIssueQty.Visible = false;
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (FormState == "New")
                {   
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[5].Visible = true;
                    e.Row.Cells[6].Visible = true;
                    e.Row.Cells[7].Visible = true;
                }
                else if (FormState == "Saved")
                {   
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (FormState == "New")
                {
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[5].Visible = true;
                    e.Row.Cells[6].Visible = true;
                    e.Row.Cells[7].Visible = true;
                }
                else if (FormState == "Saved")
                {
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    //Set Sum of issueQty in Footer
                    Label lblFooterIssueQty = (Label)e.Row.FindControl("lblFooterIssueQty");
                    lblFooterIssueQty.Text = strTotalQty;

                    Label lblTotalAmount = (Label)e.Row.FindControl("lblTotalAmount");
                    lblTotalAmount.Text = strTotalAmount;
                    
                }

            }
        }

        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddl = sender as DropDownList;
                TableCell tc = ddl.Parent as TableCell;
                GridViewRow gvr = tc.Parent as GridViewRow;

                string strItemCode = gvr.Cells[1].Text;
                string strLocationQty = string.Empty;
                DropDownList ddlLocation = (DropDownList)gvr.FindControl("ddlLocation");
                Get_locationWisePartData(strItemCode, ddl.SelectedValue.ToString(), ref strLocationQty);
                gvr.Cells[7].Text = strLocationQty;
                //Get Avg Cost
                DataSet dsAvgCost = new DataSet();
                decimal DAvgCost = 0;
                dsAvgCost = SysFunc.ItemAvgCost(strItemCode, Session["DealerCode"].ToString());
                Label lblAvgCost = (Label)gvr.FindControl("lblAvgCost");
                if (dsAvgCost.Tables[0].Rows.Count != 0)
                {
                    DAvgCost = Convert.ToDecimal(dsAvgCost.Tables[0].Rows[0]["Cost"]);
                    lblAvgCost.Text = DAvgCost.ToString();
                }
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            } 
        }

        protected void txtIssueQty_TextChanged(object sender, EventArgs e)
        {
            try
            {
                

                TextBox txt = sender as TextBox;
                TableCell tc = txt.Parent as TableCell;
                GridViewRow gvr = tc.Parent as GridViewRow;
                Label lblAvgCost = (Label)gvr.FindControl("lblAvgCost");
                TextBox txtIssueQty = (TextBox)gvr.FindControl("txtIssueQty");
                Label LbAmount = (Label)gvr.FindControl("LbAmount");

                decimal dAvgCost = Convert.ToDecimal(lblAvgCost.Text.Trim() == "" ? "0" : lblAvgCost.Text.Trim());
                decimal dIssueQty = Convert.ToDecimal(txtIssueQty.Text.Trim() == "" ? "0" : txtIssueQty.Text.Trim());

                LbAmount.Text = Convert.ToString(dAvgCost * dIssueQty);
                //strSubmitBehavior = "Y";
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }

        }

        protected void btnPosting_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2547", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            try
            {
                //if (txtSIRNO.Text.Trim() == string.Empty)
                //{
                //    ObjGeneral.UserMsg(LbErr, Color.Red, "SIRNo. should not be left blank", txtSIRNO);
                //    return;
                //}

                //for (int i = 0; i < GvMain.Rows.Count; i++)
                //{
                //    Label LbItemCode = (Label)GvMain.Rows[i].FindControl("LbItemCode");
                //    if (ObjGeneral.IsExist("SIRNo", txtSIRNO.Text, "SIRDetail", " And ItemCode='" + LbItemCode.Text + "' and  Quantity <= RecQty "))
                //    {
                //        ObjGeneral.UserMsg(LbErr, Color.Red, "Part No: " + LbItemCode.Text + " Is already issued", txtIssueNo);
                //        return;
                //    }
                //}
                // if(ObjGeneral.IsExist ("SIRNo",txtSIRNO.Text ,""
                //if (ObjGeneral.IsPosted("StockIssue", "IssueNo", txtIssueNo.Text) == true)
                //{
                //    ObjGeneral.UserMsg(LbErr, Color.Red, "The Issuance is already posted; now you could not change/delete/Post the record...", txtIssueNo);
                //    return;
                //}
                //else
                //{
                //    LbErr.Text = "";
                //}
                if (txtIssueDate.Text.Trim() == string.Empty)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Issue Date should not blank.");
                    return;
                }
                if (Convert.ToDateTime(SysFunc.SaveDate(txtIssueDate.Text)).Date > Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd")).Date)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Issue Date should not greater then current date.");
                    return;
                }
                if (Convert.ToDateTime(SysFunc.SaveDate(txtIssueDate.Text)).Date < Convert.ToDateTime(SysFunc.SaveDate(txtSIRDate.Text)).Date)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Issue Date should not less then SIR Date");
                    return;
                }
                //Add Griid checks
                double DBalQty = 0.0, DIssueQty = 0.0, DStockLoc = 0.0;
                foreach (GridViewRow gvr in GvMain.Rows)
                {
                    CheckBox chkSelect = (CheckBox)gvr.FindControl("ChkSelect");
                    if (chkSelect.Checked)
                    {
                        TextBox txtIssueQty = (TextBox)gvr.FindControl("txtIssueQty");
                        Label lblAvgCost = (Label)gvr.FindControl("lblAvgCost");
                        DropDownList ddlLocation = (DropDownList)gvr.FindControl("ddlLocation");

                        DIssueQty = Convert.ToDouble(txtIssueQty.Text == "" ? "0" : txtIssueQty.Text);
                        DBalQty = Convert.ToDouble(gvr.Cells[6].Text == "" ? "0" : gvr.Cells[6].Text);
                        DStockLoc = Convert.ToDouble(gvr.Cells[7].Text == "" ? "0" : gvr.Cells[7].Text);
                        if (DIssueQty <= 0)
                        {
                            SysFunc.UserMsg(LbErr, Color.Red, "Part Item No. "+gvr.Cells[2].Text+" Issue quantity should not be Zero (0)", txtIssueQty);
                            return;
                        }
                        else
                        {
                            LbErr.Text = string.Empty;
                        }
                        if (DIssueQty > DBalQty)
                        {
                            SysFunc.UserMsg(LbErr, Color.Red, "Part Item No. " + gvr.Cells[2].Text + " Issue quantity can not be more than remaining quantity", txtIssueQty);
                            return;
                        }
                        else
                        {
                            LbErr.Text = "";
                        }
                        if (DIssueQty > DStockLoc)
                        {
                            SysFunc.UserMsg(LbErr, Color.Red, "Part Item No. " + gvr.Cells[2].Text + " The stock has not available at this location please change location");
                            return;
                        }
                        else
                        {
                            LbErr.Text = "";
                        }
                        if (Convert.ToDecimal(lblAvgCost.Text) <= 0)
                        {
                            SysFunc.UserMsg(LbErr, Color.Red, "Part Item No. " + gvr.Cells[2].Text + " Average cost has not in correct format.");
                            return;
                        }
                        else
                        {
                            LbErr.Text = "";
                        }
                        if (ddlLocation.Items.Count == 0)
                        {
                            SysFunc.UserMsg(LbErr, Color.Red, "Part Item No. " + gvr.Cells[2].Text + " Location dose not exsit.");
                            return;
                        }
                        else
                        {
                            LbErr.Text = "";
                        }
                    }
                }
        
            
            

            
            
            
            
            //ds = (DataSet)ViewState["ObjPOds"];
            //if (ds.Tables[2].Rows.Count > 0)
            //{
            //    for (int i = 0; i < GvMain.Rows.Count; i++)
            //    {
            //        Label LbPartItemNo = (Label)GvMain.Rows[i].FindControl("LbItemCode");
            //        if (txtPartItem.Text == LbPartItemNo.Text)
            //        {
            //            ds.Tables[2].Rows[i]["IssueQty"] = txtIssueQty.Text;
            //            IntStatus = 1;
            //        }
            //    }
            //}
                /////////////////////
                if (ddlIssueNo.SelectedIndex > 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "The Issuance is already posted; now you could not change/delete/Post the record...");
                        return;
                }
                Posting();
                Load_ddlCounterNo();
                //CtlClear();
                //ViewState["ObjPOds"] = null;

            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }
        private void Posting()
        {
            SqlTransaction Trans = null;
            bool Check = false;
            string strChkSelect = string.Empty;
            try
            {
                foreach (GridViewRow gvr in GvMain.Rows)
                {
                    CheckBox chkSelect = (CheckBox)gvr.Cells[0].FindControl("ChkSelect");
                    if (chkSelect.Checked == true)
                    {
                        strChkSelect = "T";
                        break;
                    }
                    else
                    {
                        strChkSelect = "F";
                    }
                }
                if (strChkSelect == "F")
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Select Item in parts list");
                    return;
                }
                if (ObjTrans.BeginTransaction(ref Trans) == false)
                {
                    return;
                }
                // Issues Code Generation
                //if (txtIssueNo.Text.Trim() == "")
                //{
                //    if (Post_InsertMasterDetail(Trans))
                //    {
                //        Check = true;
                //    }
                //    else
                //    {
                //        Check = false;
                //    }
                //}
                //else
                //{
                //    Check = true;
                //}
                
                    Save_InsertMasterDetail(Trans, ref Check);
                //if (Check == true)
                //{
                    foreach (GridViewRow gvr in GvMain.Rows)
                    {
                        CheckBox chkSelect = (CheckBox)gvr.FindControl("ChkSelect");
                        if (chkSelect.Checked)
                        {
                            SqlParameter[] param = {
                                         new SqlParameter("@IssueQty",SqlDbType.Float),
                                         new SqlParameter("@SIRNo",SqlDbType.Char),
                                         new SqlParameter("@ItemCode",SqlDbType.Char),
                                         new SqlParameter("@DEALER_CODE",SqlDbType.Char)
                                       };

                            DropDownList ddlLocation = (DropDownList)gvr.FindControl("ddlLocation");
                            //HiddenField HDPartsTransLocCode = (HiddenField)GvMain.Rows[i].FindControl("HDPartsTransLocCode");

                            TextBox txtIssueQty = (TextBox)gvr.FindControl("txtIssueQty");
                            Label lblAvgCost = (Label)gvr.FindControl("lblAvgCost");
                            Label LbAmount = (Label)gvr.FindControl("LbAmount");


                            param[0].Value = txtIssueQty.Text.Trim();
                            param[1].Value = ddlSirNo.SelectedValue.ToString().Trim();
                            param[2].Value = gvr.Cells[1].Text.Trim();
                            param[3].Value = Session["DealerCode"].ToString().Trim();

                            if (SysFunc.UpdateSIRDetail(Trans, param))
                            {
                                SqlParameter[] param1 = { 
                                               new SqlParameter("@IssueQty",SqlDbType.Float),//0
                                               new SqlParameter("@JobCardCode",SqlDbType.Char ),//1
                                               new SqlParameter("@ItemCode",SqlDbType.Char),//2
                                               new SqlParameter("@DEALER_CODE",SqlDbType.Char)//3
                                            };
                                param1[0].Value = txtIssueQty.Text.Trim();
                                param1[1].Value = txtJC_CS_No.Text.Trim();
                                param1[2].Value = gvr.Cells[1].Text.Trim();
                                param1[3].Value = Session["DealerCode"].ToString().Trim();

                                if (txtSource.Text.Trim() == "J")
                                {
                                    Check = SysFunc.UpdateJobCardDetail(Trans, param1, ddlSirNo.SelectedValue.ToString(), gvr.Cells[1].Text,Session["DealerCode"].ToString());
                                    if (Check == false)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    Check = true;
                                }


                                if (Check == true)
                                {

                                    SqlParameter[] param2 = { 
                                               new SqlParameter("@IssueQty",SqlDbType.Float),//0
                                               new SqlParameter("@IssueVal",SqlDbType.Char ),//1
                                               new SqlParameter("@ItemCode",SqlDbType.Char),//2
                                               new SqlParameter("@LocationCode",SqlDbType.Char),//3
                                               new SqlParameter("@DEALER_CODE",SqlDbType.Char)//4
                                            };
                                    param2[0].Value = txtIssueQty.Text.Trim();
                                    param2[1].Value = LbAmount.Text.Trim();
                                    param2[2].Value = gvr.Cells[1].Text.Trim();
                                    //param2[3].Value = txtLocationCode.Text;
                                    param2[3].Value = ddlLocation.SelectedValue.ToString().Trim();
                                    param2[4].Value = Session["DealerCode"].ToString().Trim();

                                    if (SysFunc.UpdateStock(Trans, param2))
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

                        }
                    }


                //Check SIR RecQty 
                    foreach (GridViewRow gvr in GvMain.Rows)
                    {
                        CheckBox chkSelect = (CheckBox)gvr.FindControl("ChkSelect");
                        if (chkSelect.Checked)
                        {
                            SqlParameter[] param3 = { 
                                               new SqlParameter("@SIRNO",SqlDbType.Char),//0
                                               new SqlParameter("@DEALER_CODE",SqlDbType.Char ),//1
                                               new SqlParameter("@ItemCode",SqlDbType.Char),//2
                                               
                                            };



                            param3[0].Value = ddlSirNo.SelectedValue.ToString().Trim();
                            param3[1].Value = Session["DealerCode"].ToString();
                            param3[2].Value = gvr.Cells[1].Text.Trim();

                            if (SysFunc.Sp_Sparts_Get_IsItemIssuedExta(Trans, param3))
                            {
                                Check = true;
                            }
                            else
                            {
                                Check = false;
                                SysFunc.UserMsg(LbErr, Color.Red, "Issuance of Part [" + gvr.Cells[1].Text + "] is already completed, you cannot issue more quantity.");
                            }

                        }
                    }
                //Updating the CounterSalesMaster
                if (txtSource.Text.Trim() == "C")
                {
                    SqlParameter[] param4 = { 
                                               new SqlParameter("@SIRNO",SqlDbType.Char),//0
                                               new SqlParameter("@DEALER_CODE",SqlDbType.Char ),//1
                                               new SqlParameter("@InvoiceNO",SqlDbType.Char ),//2
                                            };

                    param4[0].Value = ddlSirNo.SelectedValue.ToString().Trim();
                    param4[1].Value = Session["DealerCode"].ToString();
                    param4[2].Value = txtJC_CS_No.Text.Trim();

                    if (SysFunc.SP_Parts_Update_UpdateCounterSales(Trans, param4))
                    {
                        Check = true;
                    }
                    else
                    {
                        Check = false;
                        SysFunc.UserMsg(LbErr, Color.Red, "Issuance information could not be Posted");
                    }
                }


                //Posting the Stock Issuance

                //


                if (SysFunc.PartIssues_UpdatePost(Trans, strIssueNo, Session["DealerCode"].ToString()))
                {
                    Check = true;
                }
                else
                {
                    Check = false;
                    SysFunc.UserMsg(LbErr, Color.Red, "Issuance information could not be Posted");
                }

                if (Check == false)
                {
                    ObjTrans.RollBackTransaction(ref Trans);
                    SysFunc.UserMsg(LbErr, Color.Red, "Record not save and post. Try again...");
                }
                else
                {
                    ObjTrans.CommittTransaction(ref Trans);
                    SysFunc.UserMsg(LbErr, Color.Green, "Record saved and post successfully. Last Code: " + strIssueNo);
                    //strSubmitBehavior = "Y";
                    System.Windows.Forms.MessageBox.Show("Record saved and post successfully. Last Code: " + strIssueNo, "Message");
                    btnClear_Click(btnClear, EventArgs.Empty);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);
                }
            }
            //}
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref Trans);
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }
        private void Save_InsertMasterDetail(SqlTransaction Trans, ref bool Check)
        {

            //Trans = null;
            //Check = false;

            string StrIssueDate = "";
            
            try
            {

                //if (GvMain.Rows.Count == 0)
                //{
                //    ObjGeneral.UserMsg(LbErr, Color.Red, "At least one part must be add...", txtPartItem);
                //    return;
                //}
                //else
                //{
                //    LbErr.Text = "";
                //}
                StrIssueDate = txtIssueDate.Text;

                int IssueDate = Convert.ToInt32(txtIssueDate.Text.Substring(6, 4) + txtIssueDate.Text.Substring(3, 2) + txtIssueDate.Text.Substring(0, 2));
                int SIRDate = Convert.ToInt32(txtSIRDate.Text.Substring(6, 4) + txtSIRDate.Text.Substring(3, 2) + txtSIRDate.Text.Substring(0, 2));

                if (txtSource.Text == "J")
                {
                    if (IssueDate < SIRDate)
                    {
                        SysFunc.UserMsg(LbErr, Color.Red, "Issuance date should not be less than jobcard date...", txtIssueDate);
                        return;
                    }
                    else
                    {
                        LbErr.Text = "";
                    }
                }
                if (txtSource.Text == "C")
                {
                    if (IssueDate < SIRDate)
                    {
                        SysFunc.UserMsg(LbErr, Color.Red, "Issuance date should not be less than counter sale date...", txtIssueDate);
                        return;
                    }
                    else
                    {
                        LbErr.Text = "";
                    }
                }

                //if (ObjTrans.BeginTransaction(ref Trans) == false)
                //{
                //    return;
                //}

                //if (ObjGeneral.IsPosted("StockIssue", "IssueNo", txtIssueNo.Text) == true)
                //{
                //    ObjGeneral.UserMsg(LbErr, Color.Red, "The Issuance is already posted; now you could not change/delete/Post the record...", txtIssueNo);
                //    return;
                //}
                //else
                //{
                //    LbErr.Text = "";
                //}

                SqlParameter[] param = {
                                       new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                       new SqlParameter("@IssueNo",SqlDbType.Char),//1
                                       new SqlParameter("@IssueDate",SqlDbType.DateTime),//2
                                       new SqlParameter("@SIRNo",SqlDbType.Char),//3
                                       new SqlParameter("@ReqSource",SqlDbType.VarChar),//4
                                       new SqlParameter("@JobCardNO",SqlDbType.Char),//5
                                       new SqlParameter("@InvoiceNo",SqlDbType.Char),//6
                                       new SqlParameter("@Remarks",SqlDbType.VarChar),//7
                                       new SqlParameter("@UpdUser",SqlDbType.VarChar),//8
                                       //new SqlParameter("@UpdDate",SqlDbType.DateTime),//9
                                       //new SqlParameter("@UpdTime",SqlDbType.DateTime),//10
                                       new SqlParameter("@UpdTerm",SqlDbType.VarChar),//11
                                       
                                       
                                   };
                param[0].Value = Session["DealerCode"].ToString();
                strIssueNo = SysFunc.AutoGen("StockIssue", "IssueNo", txtIssueDate.Text.Trim());
                param[1].Value = strIssueNo;
                param[2].Value = SysFunc.SaveDate(txtIssueDate.Text); ;
                param[3].Value = ddlSirNo.SelectedValue.ToString().Trim();
                param[4].Value = txtSource.Text;
                if (txtSource.Text == "J")
                {
                    param[5].Value = txtJC_CS_No.Text;
                    param[6].Value = (Object)DBNull.Value;
                }
                else if (txtSource.Text == "C")
                {
                    param[5].Value = (Object)DBNull.Value;
                    param[6].Value = txtJC_CS_No.Text; 
                }
                
                param[7].Value = txtRemarks.Text.Replace("'", " ");
                param[8].Value = Session["UserName"].ToString();
                //param[9].Value = ObjGeneral.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
                //param[10].Value = ObjGeneral.SaveDate(DateTime.Now.ToString("dd/MM/yyyy"));
                param[9].Value = GlobalVar.mUserIPAddress;

                if (SysFunc.SP_Parts_Insert_StockIssue(Trans, param))
                {
                    foreach (GridViewRow gvr in GvMain.Rows)
                    {
                        CheckBox chkSelect = (CheckBox)gvr.Cells[0].FindControl("ChkSelect");
                        if (chkSelect.Checked)
                        {
                            SqlParameter[] param1 = {
                                        new SqlParameter("@DealerCode",SqlDbType.Char ),//0
                                        new SqlParameter("@IssueNo",SqlDbType.Char),//1
                                        new SqlParameter("@ItemCode",SqlDbType.Char),//2
                                        new SqlParameter("@SIRNo",SqlDbType.Char),//3
                                        new SqlParameter("@PartsTransLocCode",SqlDbType.Char),//4
                                        new SqlParameter("@IssueQty",SqlDbType.Float),//5
                                        new SqlParameter("@Rate",SqlDbType.Float),//6
                                        new SqlParameter("@Amount",SqlDbType.Float)//7
                                        };

                            //Label LbItemCode = (Label)GvMain.Rows[i].FindControl("LbItemCode");
                            DropDownList ddlLocation = (DropDownList)gvr.FindControl("ddlLocation");
                            //HiddenField HDPartsTransLocCode = (HiddenField)GvMain.Rows[i].FindControl("HDPartsTransLocCode");

                            TextBox txtIssueQty = (TextBox)gvr.FindControl("txtIssueQty");
                            Label lblAvgCost = (Label)gvr.FindControl("lblAvgCost");
                            Label LbAmount = (Label)gvr.FindControl("LbAmount");



                            param1[0].Value = Session["DealerCode"].ToString();
                            param1[1].Value = strIssueNo;
                            param1[2].Value = gvr.Cells[1].Text.Trim();
                            param1[3].Value = ddlSirNo.SelectedValue.ToString();
                            param1[4].Value = ddlLocation.SelectedValue.ToString();
                            param1[5].Value = txtIssueQty.Text.Trim();
                            param1[6].Value = lblAvgCost.Text;
                            param1[7].Value = LbAmount.Text;

                            if (SysFunc.Sp_Parts_Insert_StockIssueDetail(Trans, param1))
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
                }
            }
            catch (Exception ex)
            {
                //ObjTrans.RollBackTransaction(ref Trans);
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            //if (strSubmitBehavior!="Y")
            //{
                SysFunc.ClearTextBoxes(Page);
                SysFunc.Clearddl(Page);
                txtIssueDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                createPartsDT();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);
            // Load_ddlSirNo();
            //}
            //strSubmitBehavior = string.Empty;
        }
        private void FillFormData()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = {    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                      new SqlParameter("@IssueNo",SqlDbType.Char,10)
                                   };
                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlIssueNo.SelectedValue.ToString().Trim();
                ds = SysFunc.Get_Parts_Get_GetStockIssue(param);

                txtIssuedSirNo.Text = ds.Tables[0].Rows[0]["SIRNo"].ToString();
                txtSIRDate.Text = ds.Tables[0].Rows[0]["SIRDate"].ToString();
                txtSource.Text = ds.Tables[0].Rows[0]["ReqSource"].ToString();
                if (txtSource.Text == "J")
                {
                    txtJC_CS_No.Text = ds.Tables[0].Rows[0]["JobCardNO"].ToString();
                    lblSource.Text = "Job Card No.";
                }
                else if (txtSource.Text == "C")
                {
                    txtJC_CS_No.Text = ds.Tables[0].Rows[0]["InvoiceNO"].ToString();
                    lblSource.Text = "Counter Sale No.";
                }
                txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();

                FillGV();

            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
            }
        }
            private void FillGV()
    {

        DataSet dsMain = new DataSet();
        try
        {

            SqlParameter[] param = {
                                       new SqlParameter ("@DealerCode",SqlDbType .Char),
                                       new SqlParameter("@IssueNo",SqlDbType.Char)
                                  };
            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = ddlIssueNo.SelectedValue.ToString().Trim();
            dsMain = SysFunc.SP_parts_Get_StockIssueDetail(param);
            GvMain.DataSource = dsMain.Tables[0];
            GvMain.DataBind();
        }
        catch (Exception ex)
        {
            SysFunc.UserMsg(LbErr, Color.Red, ex.Message);
        }
    }

        protected void ddlIssueNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMode.Text = "Edit Mode";
            SqlDataReader dr=null;
            DataTable dtTotals = new DataTable();
            FormState = "Saved";
            FillFormData();
            SqlParameter[] param = {
                                       new SqlParameter ("@DealerCode",SqlDbType .Char),
                                       new SqlParameter("@IssueNo",SqlDbType.Char)
                                  };
            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = ddlIssueNo.SelectedValue.ToString().Trim();
            SysFunc.ExecuteSP("sp_Get_Totals_StockIssueDetail", param, ref dr);
            dtTotals.Load(dr);
            strTotalQty=dtTotals.Rows[0]["TotalIssueQty"].ToString();
            strTotalAmount = dtTotals.Rows[0]["TotalAmount"].ToString();
            FillGV();
        }

        protected void ImageButtonSI_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 40; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_IssueNo(ImageButtonSI, ViewState["lookupid"].ToString(), "", "../../../");

            btnValue = 1;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }

        protected void ImageButtonSIR_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 39; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_SIRNumber(ImageButtonSIR, ViewState["lookupid"].ToString(), "", "../../../","SIR");

            btnValue = 2;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }
        protected void SelectedPartDetail(string item)
        {
            try
            {

                if (btnValue == 1)
                {

                    ddlIssueNo.SelectedValue = item.ToString();
                    ddlIssueNo_SelectedIndexChanged(null, null);

                }
                else
                {
                    ddlSirNo.SelectedValue = item;
                    ddlSirNo_SelectedIndexChanged(null, null);
                }

            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblImp, Color.Red, ex.Message);
            }
        }
    }
}