using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;
using System.Threading;
using System.IO;
using System.Data.OleDb;

using PdfSharp.Pdf;

namespace DXBMS.Modules.SpareParts.Forms
{
    public partial class StockAdjustment : System.Web.UI.Page
    {

        SysFunction sysFunc = new SysFunction();
        SysFunctions sysFuncs = new SysFunctions();
        SecurityBll sec = new SecurityBll();
        DataSet ds;
        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;
        bool search_result, search_item;
        DataTable PartsDT;
        clsLookUp clslook = new clsLookUp();
        static int btnValue = 0;
        double TotalAmount=0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2548", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (ViewState["StockAdj"] != null) PartsDT = (DataTable)ViewState["StockAdj"];

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
                    ThreadStart childthreat = new ThreadStart(childthreadcall);
                    Thread child = new Thread(childthreat);

                    child.Start();
                    createGrid();
                    lOADddl();
                    string leadId = Session["TransCode"].ToString();
                    ddlChargeOutNo.SelectedValue = leadId;

                    ddlChargeOutNo_SelectedIndexChanged(null, null);


                }
                else
                {
                    lblMode.Text = "Add Mode";
                    ThreadStart childthreat = new ThreadStart(childthreadcall);
                    Thread child = new Thread(childthreat);

                    child.Start();
                    createGrid();
                    lOADddl();
                }
             

                //sysFuncs.Load_CounterSalesItem_ddl(ddlChPartNo, Session["DealerCode"].ToString(), "P");
            }
            btnValue = 0;
            Session["TransCode"] = null;
            Session["LookUpData"] = string.Empty;
            btnSubmit.Enabled = true;
        }


        public void childthreadcall()
        {
            try
            {

                string sQuery = "SP_SelectParts '" + Session["DealerCode"].ToString() + "', 'P'";

                if (sysFuncs.ExecuteQuery(sQuery, ref ds))
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

        private void createGrid()
        {
            PartsDT = new DataTable();
            //PartsDT.Columns.Add(new DataColumn("ChargeOutNo", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("ItemCode", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("ItemNo", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("ItemDesc", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("PartsTransLocCode", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("INQty", typeof(string)));       // add
            PartsDT.Columns.Add(new DataColumn("Amount", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("Rate", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("StockQty", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("ChargeOutQty", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("WAR", typeof(string)));

            gv_purchase.DataSource = PartsDT; gv_purchase.DataBind(); Session["StockDetail"] = PartsDT;

        }

        private void lOADddl()
        {
            
            ddlReason.Items.Clear();
            ddlReason.Items.Add(new ListItem("Select", "Select"));
            ddlReason.Items.Add(new ListItem("Opening Stock", "Opening Stock"));
            ddlReason.Items.Add(new ListItem("Borrow From Dealer", "Borrow From Dealer"));
            ddlReason.Items.Add(new ListItem("Borrow From Market", "Borrow From Market"));
            ddlReason.Items.Add(new ListItem("Audit Adjustment", "Audit Adjustment"));
            ddlReason.Items.Add(new ListItem("Cripling", "Crippling"));

            // DDLColums retrive
            string WhereQuery = "DealerCode= '" + Session["DealerCode"].ToString() + "' ";

            string[] col = new string[] { "ChargeOutNo" };
            sysFunc.GetMultiColumnsDDL(ddlChargeOutNo, col, "ChargeOutMaster", WhereQuery, "ChargeOutNo", "", false, false);

            string[] col1 = new string[] { "VendorCode", "VendorDesc" };
            sysFunc.GetMultiColumnsDDL(ddlVendor, col1, "Vendor", "DealerCode in ('"+Session["DealerCode"].ToString()+"','COMON')", "VendorCode", "", false, false);

            txtChargeOutDate.Text = DateTime.Now.ToString("dd-MM-yyyy");

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2548", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }

            try
            {

                if (ddlChargeOutNo.SelectedIndex != 0)
                {
                    sysFuncs.UserMsg(lblMessage, Color.Red, "This  record is already exist ", null);
                    return;
                }
                if (txtChargeOutDate.Text == "")
                {

                    sysFuncs.UserMsg(lblMessage, Color.Red, "Please Select Date First", txtChargeOutDate);
                    return;
                }
                else if (ddlReason.SelectedIndex == 0)
                {
                    sysFuncs.UserMsg(lblMessage, Color.Red, "Please Select Reason", txtChargeOutQty);
                    return;
                }               

                if (gv_purchase.Rows.Count == 0)
                {
                    sysFuncs.UserMsg(lblMessage, Color.Red, "Please Enter Detail", txtChargeOutQty);
                    return;
                }

                bool Check = false;
                

                txtAmount.Text = hfAmount.Value.ToString();
                // after insert , data index will retriv in ddl
                string max = sysFuncs.AutoGen("ChargeOutMaster", "ChargeOutNo", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));

                SqlParameter[] param =
           {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                new SqlParameter("@ChargeOutNo",SqlDbType.Char,8), //1
                new SqlParameter("@ChargeOutDate",SqlDbType.DateTime),//2
                new SqlParameter("@StockType",SqlDbType.Char,3), //3
                new SqlParameter("@Remarks",SqlDbType.VarChar,50),//4
                //new SqlParameter("@Amount",SqlDbType.Float),//5 
                new SqlParameter("@DelFlag",SqlDbType.Char,1),//6 
                new SqlParameter("@PostFlag",SqlDbType.Char,1),//7 
                new SqlParameter("@UpdUser",SqlDbType.VarChar,50), // 8
                //new SqlParameter("@UpdDate",SqlDbType.DateTime),
                //new SqlParameter("@UpdTime",SqlDbType.DateTime),
                new SqlParameter("@UpdTerm ",SqlDbType.VarChar,50), //9
               /* new SqlParameter("@PostDate",SqlDbType.DateTime),*/ 
                new SqlParameter("@Reason",SqlDbType.VarChar,50), // 10
                new SqlParameter("@VendorCode",SqlDbType.Char,6), // 11
                 new SqlParameter("@refDocno",SqlDbType.VarChar,50),  //12
                 new SqlParameter("@totalAmount",SqlDbType.Float),  //12


            };

                param[0].Value = Session["DealerCode"].ToString(); // 0
                param[1].Value = max;                               //1
                param[2].Value = sysFuncs.SaveDate(txtChargeOutDate.Text);      //2
                if (RadioIN.Checked == true)                        //3    
                {
                    param[3].Value = "IN";
                    param[10].Value = ddlVendor.SelectedValue.ToString().Trim();
                }
                else
                {
                    param[3].Value = "OUT";
                    param[10].Value = "";
                }

                param[4].Value = txtRemarks.Text.Trim();            // 4
                //param[5].Value = txtAmount.Text.Trim();             //5
                param[5].Value = "N";                               //6 
                param[6].Value = "N";                               //7    
                param[7].Value = Session["UserName"].ToString();    //8
                param[8].Value = "ALI";                             //9    
                /* param[8].Value = txtPostDate.Text.ToString(); */
                // 7
                param[9].Value = ddlReason.SelectedValue.ToString().Trim(); //10
                param[11].Value = txtRefDocno.Text.Trim();                  //12
                param[12].Value = SysFunctions.CustomCDBL(lblTotalAMount.Text);


                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    if (sysFunc.ExecuteSP_NonQuery("sp_W2_ChargeOutMaster_Insert", param, Trans))
                    {
                        SqlParameter[] param1 = {
                                                new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                                                new SqlParameter("@ChargeOutNo",SqlDbType.Char,8), //1
                                                new SqlParameter("@ItemCode",SqlDbType.Char,8), //2
                                                new SqlParameter("@PartsTransLocCode",SqlDbType.VarChar,8), //3
                                                new SqlParameter("@Rate",SqlDbType.Float),                  //4
                                                new SqlParameter("@Amount",SqlDbType.Float),                //5
                                                new SqlParameter("@StockQty",SqlDbType.Float),             //6 
                                                new SqlParameter("@ChargeOutQty",SqlDbType.Int),
                                                new SqlParameter("@INQty",SqlDbType.Int)
                                            };

                        PartsDT = (DataTable)Session["StockDetail"];
                        string itemCode = "";
                        DataRow[] drr = PartsDT.Select();
                        for (int i = 0; i < drr.Length; i++)
                        {
                            if(PartsDT.Rows[i]["ItemNo"].ToString().Trim() != "")
                            {
                                if (PartsDT.Rows[i]["ItemCode"].ToString() == "")
                                {
                                    itemCode = sysFuncs.GetItemCodeByPartItemNo(PartsDT.Rows[i]["ItemNo"].ToString(), Session["DealerCode"].ToString());
                                }
                                else if (sysFunc.CodeExists("Item", "ItemCode", PartsDT.Rows[i]["ItemCode"].ToString(), Session["DealerCode"].ToString()))
                                {

                                    itemCode = PartsDT.Rows[i]["ItemCode"].ToString();
                                }
                                if (itemCode != "")
                                {
                                    param1[0].Value = Session["DealerCode"].ToString(); // 0
                                    param1[1].Value = max;                               //1
                                    param1[2].Value = itemCode;
                                    param1[3].Value = PartsDT.Rows[i]["PartsTransLocCode"].ToString();
                                    param1[4].Value = SysFunctions.CustomCDBL(PartsDT.Rows[i]["Rate"].ToString() == "" ? "0" : (PartsDT.Rows[i]["Rate"].ToString()));
                                    param1[5].Value = SysFunctions.CustomCDBL(PartsDT.Rows[i]["Amount"].ToString() == "" ? "0" : (PartsDT.Rows[i]["Amount"].ToString()));
                                    param1[6].Value = SysFunctions.CustomCDBL(PartsDT.Rows[i]["StockQty"].ToString() == "" ? "0" : (PartsDT.Rows[i]["StockQty"].ToString()));
                                    if (RadioIN.Checked == true)
                                    {
                                        param1[7].Value = 0;
                                        param1[8].Value = SysFunctions.CustomCDBL(PartsDT.Rows[i]["INQty"].ToString() == "" ? "0" : (PartsDT.Rows[i]["INQty"].ToString()));
                                    }
                                    else
                                    {
                                        param1[7].Value = SysFunctions.CustomCDBL(PartsDT.Rows[i]["ChargeOutQty"].ToString() == "" ? "0" : (PartsDT.Rows[i]["ChargeOutQty"].ToString()));
                                        param1[8].Value = 0;
                                    }

                                    if (sysFunc.ExecuteSP_NonQuery("SP_ChargeOutDetail_Insert", param1, Trans))
                                    {
                                        //string ItemCode = PartsDT.Rows[i]["ItemCode"].ToString();
                                        string LocCode = PartsDT.Rows[i]["PartsTransLocCode"].ToString();
                                        double Amount = SysFunctions.CustomCDBL(PartsDT.Rows[i]["Amount"].ToString());
                                        double Qty;
                                        if (RadioIN.Checked == true)
                                        {
                                            Qty = SysFunctions.CustomCDBL(PartsDT.Rows[i]["INQty"].ToString() == "" ? "0" : PartsDT.Rows[i]["INQty"].ToString());
                                        }
                                        else
                                        {
                                            Qty = SysFunctions.CustomCDBL(PartsDT.Rows[i]["ChargeOutQty"].ToString() == "" ? "0" : PartsDT.Rows[i]["ChargeOutQty"].ToString());

                                        }


                                        //if (!sysFuncs.CodeExists("ItemStock", "ItemCode", itemCode, Session["DealerCode"].ToString()))
                                        //{

                                        SqlParameter[] param3 = {
                                                            new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                            new SqlParameter("@ItemCode",SqlDbType.Char,8),
                                                            new SqlParameter("@PartsTransLocCode",SqlDbType.VarChar,8),
                                                            new SqlParameter("@IssueQty",SqlDbType.Float),
                                                            new SqlParameter("@IssueValue",SqlDbType.Float),
                                                            new SqlParameter("@Type",SqlDbType.VarChar,3)
                                                        };
                                        //PartsDT = (DataTable)ViewState["StockAdj"];
                                        //DataRow[] drr = PartsDT.Select();

                                        if (itemCode != "")
                                        {
                                            param3[0].Value = Session["DealerCode"].ToString();
                                            param3[1].Value = itemCode;
                                            param3[2].Value = PartsDT.Rows[i]["PartsTransLocCode"].ToString();
                                            param3[3].Value = Qty;
                                            param3[4].Value = Qty * Amount;

                                            if (RadioIN.Checked == true)
                                            {
                                                param3[5].Value = "IN";
                                            }
                                            else
                                            {
                                                param3[5].Value = "OUT";
                                            }

                                        }


                                        if (!sysFunc.ExecuteSP_NonQuery("SP_ItemStock_Insert", param3, Trans))
                                        {
                                            ObjTrans.RollBackTransaction(ref Trans);
                                        }

                                        //}

                                        //if (UpdateStock(itemCode, LocCode, Qty, Amount, ref Trans) == true)
                                        //{
                                        //    Check = true;
                                        //}
                                        //else
                                        //{
                                        //    Check = false;
                                        //}                                   

                                    }
                                }
                                else
                                {
                                    ObjTrans.RollBackTransaction(ref Trans);
                                    sysFunc.UserMsg(lblMessage, Color.Red, "There is no Item of selected Part No " + PartsDT.Rows[i]["ItemNo"].ToString() + ".Pleasse enter Part Item in Item Table First.");
                                    return;
                                }

                                
                            }
                            
                        }

                        SqlParameter[] param2 = {
                                                    new SqlParameter("@CONo",SqlDbType.Char,8), //1
                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5)//0                        
                                                    };

                        param2[0].Value = max;
                        param2[1].Value = Session["DealerCode"].ToString(); // 0

                        if (RadioOUT.Checked == true)                        //3    
                        {
                            sysFunc.ExecuteSP_NonQuery("sp_PostChargeOutActivity", param2, Trans);
                        }
                        else if (RadioIN.Checked == true)
                        {
                            sysFunc.ExecuteSP_NonQuery("sp_PostINActivity", param2, Trans);
                        }
                        

                        if (ObjTrans.CommittTransaction(ref Trans) == true)
                        {
                            lblMessage.ForeColor = Color.Green;
                            lblMessage.Text = "Record Saved Successfully: " + max;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);

                        }
                        else
                        {
                            ObjTrans.RollBackTransaction(ref Trans);
                            sysFunc.UserMsg(lblMessage, Color.Red, "Record not saved Try again. Or contact to support team ");
                        }
                    }
                }

                RadioIN.Enabled = true;
                RadioOUT.Enabled = true;

                sysFunc.ClearTextBoxes(Page);

                createGrid();
                lOADddl();
                

            }
            catch (Exception ex)
            {
                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);
                ObjTrans.RollBackTransaction(ref Trans);
            }
        }

        public bool UpdateStock(string ItemCode, string LocCode, double IssueQty, double IssueVal, ref SqlTransaction Trans)
        {
            SqlDataReader dr = null;
            IssueVal = IssueQty * IssueVal;
            string Update_Qry;
            if (RadioIN.Checked == true)
            {
                Update_Qry = "Update ItemStock Set " + " INQty = isnull(INQty,0) + " + IssueQty + "," + " INVal = isnull(INVal,0) + " + IssueVal + "" + " Where ItemCode = '" + ItemCode + "'" + " AND PartsTransLocCode = '" + LocCode + "'" + " AND DealerCode = '" + Session["DealerCode"].ToString() + "'";
            }
            else
            {
                Update_Qry = "Update ItemStock Set " + " ChargeOutQty = isnull(ChargeOutQty,0) + " + IssueQty + "," + " ChargeOutVal = ChargeOutVal + " + IssueVal + "" + " Where ItemCode = '" + ItemCode + "'" + " AND PartsTransLocCode = '" + LocCode + "'" + " AND DealerCode = '" + Session["DealerCode"].ToString() + "'";
            }


            try
            {
                if (sysFuncs.ExecuteQuery_NonQuery(Update_Qry,Trans) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        protected void RadioIN_CheckedChanged(object sender, EventArgs e)
        {
            ddlVendor.Enabled = true;
            ddlReason.Items.Clear();
            ddlReason.Items.Add(new ListItem("Select", "Select"));
            ddlReason.Items.Add(new ListItem("Opening Stock", "Opening Stock"));
            ddlReason.Items.Add(new ListItem("Borrow From Dealer", "Borrow From Dealer"));
            ddlReason.Items.Add(new ListItem("Borrow From Market", "Borrow From Market"));
            ddlReason.Items.Add(new ListItem("Audit Adjustment", "Audit AdjustmentAudit Adjustment"));

            FileUpload1.Enabled = true;
            btnUpload.Enabled = true;
            txtRate.Enabled = true;
            txtRemarks.Text = "";
            txtRefDocno.Text = "";
        }

        protected void RadioOUT_CheckedChanged(object sender, EventArgs e)
        {
            ddlVendor.Enabled = false;
            ddlReason.Items.Clear();
            ddlReason.Items.Add(new ListItem("Select", "Select"));
            ddlReason.Items.Add(new ListItem("Damages", "Damages"));
            ddlReason.Items.Add(new ListItem("Dead item", "Dead item"));
            ddlReason.Items.Add(new ListItem("Audit Adjustment", "Audit Adjustment"));

            FileUpload1.Enabled = false;
            btnUpload.Enabled = false;

            txtRate.Enabled = false;
            txtRemarks.Text = "";
            ddlVendor.SelectedIndex = 0;
            txtRefDocno.Text = "";

        }

        protected void ddlChPartNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioIN.Checked == false)
            {
                string WhereQuery = "A.DealerCode= '" + Session["DealerCode"].ToString() + "'"
                + " and A.PartsTransLocCode = B.PartsTransLocCode"
                + " and A.DealerCode = B.DealerCode"
                + " and ItemCode = '" + txtItemCode.Text + "'";
                //string[] col = new string[] { "A.PartsTransLocCode", "A.ItemCode", "PartsTransLocDesc", "ItemQty" };
                //sysFunc.GetMultiColumnsDDL(ddlLocation, col, "vw_PartStockLocationWise A,PartsTransLocation B", WhereQuery, "PartsTransLocCode", "", false, false);

                string[] col = new string[] { "A.PartsTransLocCode", "A.ItemCode", "PartsTransLocDesc", "INQty" };
                sysFunc.GetMultiColumnsDDL(ddlLocation, col, "ItemStock A,PartsTransLocation B", WhereQuery, "PartsTransLocCode", "", false, false);
            }
            else
            {
                //string part = sysFuncs.GetPartLocationByPartItemNo(txtItemCode.Text , Session["DealerCode"].ToString());
                string WhereQuery = "A.DealerCode= '" + Session["DealerCode"].ToString() + "'"
                    //+ " and A.PartsTransLocCode = '"+part+"'"
                + "and A.PartsSubLocCode = C.PartsSubLocCode and A.PartsLocCode = B.PartsLocCode "
                + "and A.DealerCode = B.DealerCode and A.DealerCode = C.DealerCode";
                string[] col = new string[] { "A.PartsTransLocCode", " (Rtrim(B.PartsLocDesc) +' '+ Rtrim(C.PartsSubLocDesc) +' '+ Rtrim(A.PartsTransLocDesc))" };
                sysFunc.GetMultiColumnsDDL(ddlLocation, col, "PartsTransLocation A , PartsLocation B , PartsSubLocation C", WhereQuery, "PartsTransLocCode", "", false, false);

            }

        }

        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioIN.Checked == false)
            {
                string sql = "exec SP_LocationRateQtyWar_select '" + txtItemCode.Text + "' , '"+Session["DealerCode"].ToString()+"'";
                DataTable dt = new DataTable();
                dt = sysFunc.GetData(sql);

                txtRate.Text = dt.Rows[0]["PurchaseRate"].ToString();
                //txtStockQty.Text = dt.Rows[0]["ItemQty"].ToString();
                txtStockQty.Text = dt.Rows[0]["INQty"].ToString();
                txtWAR.Text = dt.Rows[0]["WAR"].ToString();
                //txtStockQty.Text = dt.Rows[0]["ItemQty"].ToString();
            }
            else
            {
                string sql = " SELECT  A.PurchaseRate , C.WAR ," 
                        +" rtrim(IsNull(isnull(Sum(B.INQty),0) + Sum(B.RecQty) + Sum(B.PurRetQty) + Sum(B.RetQty) - ((Sum(B.IssQty) + Sum(B.ChargeOutQty)) ), 0)) As Qty "
                        +" from Item A Left outer Join ItemStock B  On A.ItemCode=B.ItemCode And B.DealerCode in ('"+Session["DealerCode"].ToString()+"','COMON')"
                        + " inner join ItemStatus C on A.ItemCode = C.ItemCode and C.DealerCode  in ('" + Session["DealerCode"].ToString() + "','COMON')"
                        + " Where  A.DealerCode in ('" + Session["DealerCode"].ToString() + "','COMON') And A.LubeFlag = 'N' and A.ItemCode = '" + txtItemCode.Text + "'   Group By PurchaseRate , C.WAR";
                
                
                DataTable dt = new DataTable();
                dt = sysFunc.GetData(sql);
                if (dt.Rows.Count > 0)
                {
                    txtRate.Text = dt.Rows[0]["PurchaseRate"].ToString();
                    txtWAR.Text = dt.Rows[0]["WAR"].ToString();
                    txtStockQty.Text = dt.Rows[0]["Qty"].ToString() == "" ? "0" : dt.Rows[0]["Qty"].ToString();
                }
                else
                {
                    sql = " SELECT  A.PurchaseRate"
                       + " from Item A "
                       + " Where  A.DealerCode='" + Session["DealerCode"].ToString() + "' And A.LubeFlag = 'N' and A.ItemCode = '" + txtItemCode.Text + "'";


                    dt = sysFunc.GetData(sql);
                    if (dt.Rows.Count > 0)
                    {
                        txtRate.Text = dt.Rows[0]["PurchaseRate"].ToString();
                        txtWAR.Text = dt.Rows[0]["PurchaseRate"].ToString();
                        txtStockQty.Text = "0";
                    }
                }
            }
        }

        protected void BtnRemove_PurchaseReturn_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                PartsDT = (DataTable)Session["StockDetail"];
                ImageButton btn = sender as ImageButton;
                TableCell tc = btn.Parent as TableCell;
                GridViewRow gvr = tc.Parent as GridViewRow;
                

                double amount = Convert.ToDouble(PartsDT.Rows[gvr.RowIndex]["Amount"].ToString());
                PartsDT.Rows.RemoveAt(gvr.RowIndex);
                //Load grid 
                gv_purchase.DataSource = PartsDT;
                PartsDT.AcceptChanges();
                gv_purchase.DataBind();
               
                SumTOtalAMount();

            }
            catch (Exception ex)
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, ex.Message);
            }
        }

        protected void ddlChargeOutNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMode.Text = "Edit Mode";
            if (ddlChargeOutNo.SelectedIndex != 0)
            {
                DataTable dt = new DataTable();
                SqlDataReader dr = null;

                // Data load in ddlchargoutNo
                SqlParameter[] param =
           {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                new SqlParameter("@ChargeOutNo",SqlDbType.Char,8), //1

            };
                param[0].Value = Session["DealerCode"].ToString(); // 0
                param[1].Value = ddlChargeOutNo.SelectedValue.Trim();

                sysFuncs.ExecuteSP("SP_ChargeOutMaster_SASelect", param, ref dr);

                dt.Load(dr);  // Load

                // Showing Data in Text Field through DropDownLIST
                if (dt.Rows.Count > 0)
                {
                    txtChargeOutDate.Text = dt.Rows[0]["ChargeOutDate"].ToString().Trim();
                    if (dt.Rows[0]["StockType"].ToString().Trim() == "IN")
                    {
                        RadioOUT.Checked = false;
                        RadioIN.Checked = true;
                        RadioIN_CheckedChanged(null, null);     // Load this Function RADIO IN
                    }
                    else
                    {
                        RadioOUT.Checked = true;
                        RadioOUT_CheckedChanged(null, null);      // Load this Function RADIO OUT  
                    }
                    
                    ddlReason.SelectedValue = dt.Rows[0]["Reason"].ToString().Trim();
                    if(dt.Rows[0]["VendorCode"].ToString().Trim() == "" || dt.Rows[0]["VendorCode"].ToString().Trim() == null)
                    {

                    }
                    else
                    {
                        ddlVendor.SelectedValue = dt.Rows[0]["VendorCode"].ToString().Trim();
                    }
                   

                    txtRemarks.Text = dt.Rows[0]["Remarks"].ToString().Trim();

                    txtRefDocno.Text = dt.Rows[0]["RefDocno"].ToString().Trim();
                    lblTotalAMount.Text = dt.Rows[0]["TotalAmount"].ToString();

                    gv_purchase.DataSource = dt;

                    Session["StockDetail"] = dt;
                    gv_purchase.DataBind();
                    btnSubmit.Enabled = false;
                }
            }else
            {
                
                btnSubmit.Enabled = true;
            }

            
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            RadioIN.Enabled = true;
            FileUpload1.Enabled = true;
            btnUpload.Enabled = true;
            lblMessage.Text = "";

            ddlVendor.SelectedIndex = 0;
            ddlReason.SelectedIndex = 0;
            //ddlLocation.SelectedIndex = 0;
            ddlChargeOutNo.SelectedIndex = 0;
            txtRefDocno.Text = "";
            txtRemarks.Text = "";
            txtChargeOutQty.Text = "";
            lblTotalAMount.Text = "";
            createGrid();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);

        }

        protected void txtStockQty_TextChanged1(object sender, EventArgs e)
        {
            double tqtyt = Convert.ToDouble(txtChargeOutQty.Text);
            double tRate = Convert.ToDouble(txtRate.Text);
            txtAmount.Text = (tqtyt * tRate).ToString();
        }


        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2548", "006"))
            {
                Response.Redirect("~/Test.aspx");
            }

            Data.DSReports data = new Data.DSReports();
            ReportDocument RD = new ReportDocument();



            SqlParameter[] param =
        {
             new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
             new SqlParameter("@ChargeOutNo",SqlDbType.Char,8), //1

        };

            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = ddlChargeOutNo.SelectedValue.Trim();

            SqlDataReader rder = null;

            // Store Procedure check and load
            if (sysFunc.ExecuteSP("SP_ChargeOutMaster_Print", param, ref rder)) //Database SP
            {
                data.SP_ChargeOutMaster_Print.Load(rder);                       // DataSet SP

            }
            // Load in crystal report
            //RD.Load(Server.MapPath("../PurchaseSaleReturnReport.rpt"));
            //RD.Load(Server.MapPath("../crtStockAdjustment.rpt"));
            RD.Load(Server.MapPath("../SpareReports/crpStockAdjustment2.rpt"));

            // mention paper size
            RD.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;


            //RD.Load(Server.MapPath("../../SpareReports/rptInvoices.rpt"));
            // RD.Load(Server.MapPath("../rptJobHistory.rpt"));
            //RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + sysFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Phone1", "Dealer") + "'";
            // RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + sysFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Email", "Dealer") + "'";

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["FromCode"].Text = "'" + FDate + "'";
            //RD.DataDefinition.FormulaFields["ToCode"].Text = "'" + TDate + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Stock Adjustment" + "\"";
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
            string URL = "../../../Download/PrintReport.aspx";

            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";

            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

            // convert and show
            //string FilePath = Server.MapPath("~") + "\\Download\\";
            //string FileName = "StockAdjustment" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            //string File = FilePath + FileName;

            //Session["RD"] = RD;



            //string URL;
            //URL = "../../../Download/rptViewerService.aspx?FileName=" + FileName;
            //string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1200,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

        }

        private void Insert_in_PartsDT(DataRow rowPartsDT)
        {
            txtAmount.Text = hfAmount.Value.ToString();
            // item description detail function
            //string ItemDesc = sysFuncs.GetStringValuesAgainstCodes("ItemCode", ddlChPartNo.SelectedValue.Trim(), "ItemDesc", "Item",Session["DealerCode"].ToString());
            rowPartsDT["ItemCode"] = txtItemCode.Text;
            rowPartsDT["ItemNo"] = txtPartItemNo_Parts.Text;
            rowPartsDT["ItemDesc"] = txtItemDesc.Text;
            rowPartsDT["PartsTransLocCode"] = ddlLocation.SelectedValue.Trim();
            rowPartsDT["WAR"] = txtWAR.Text;
            if (RadioIN.Checked == true)
            {
                rowPartsDT["INQty"] = txtChargeOutQty.Text;
                rowPartsDT["ChargeOutQty"] = "0";
            }else
            {
                rowPartsDT["INQty"] = "0";
                rowPartsDT["ChargeOutQty"] = txtChargeOutQty.Text;
            }
                

            rowPartsDT["StockQty"] = txtStockQty.Text;
            rowPartsDT["Rate"] = txtRate.Text;
            rowPartsDT["Amount"] = txtAmount.Text;

            if (lblTotalAMount.Text != "")
            {
                lblTotalAMount.Text = Convert.ToString(Convert.ToDouble(lblTotalAMount.Text) + Convert.ToDouble(txtAmount.Text));
            }
            else
            {
                lblTotalAMount.Text = txtAmount.Text;
            }
        }

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
                //else { textBoxes[i].BorderColor = System.Drawing.ColorTranslator.FromHtml("#1A6297"); }
            }
            return isValid;
        }
        public void SumTOtalAMount()
        {
            foreach (GridViewRow row in gv_purchase.Rows)
            {
                Label lblgrn_no = (Label)row.FindControl("lblAmount");
                TotalAmount = TotalAmount + SysFunctions.CustomCDBL(lblgrn_no.Text);
            }
            lblTotalAMount.Text = TotalAmount.ToString();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {

            //if (ddlChPartNo.SelectedIndex == 0)
            //{

            //    sysFuncs.UserMsg(lblMessage, Color.Red, "Please Select Part First", txtChargeOutQty);
            //    return;
            //}
            //else
            //{
            //    lblMessage.Text = string.Empty;
            //}

            if (ddlLocation.SelectedIndex == 0)
            {

                sysFuncs.UserMsg(lblMessage, Color.Red, "Please Select Location", txtChargeOutQty);
                return;
            }


            if (txtChargeOutQty.Text == "")
            {
                sysFuncs.UserMsg(lblMessage, Color.Red, "Please enter Quantity", txtChargeOutQty);
                return;
            }
            if (RadioIN.Checked == false)
            {
                if (Convert.ToInt32(txtChargeOutQty.Text) > Convert.ToInt32(txtStockQty.Text))
                {

                    sysFuncs.UserMsg(lblMessage, Color.Red, "Issue quantity not less then receive quantity", txtChargeOutQty);
                    return;
                }
                else
                {
                    lblMessage.Text = "";
                }
            }

            PartsDT = (DataTable)Session["StockDetail"];

            //
            TextBox[] textBoxes = { txtRate, txtChargeOutQty };
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
                Session["StockDetail"] = PartsDT; gv_purchase.DataSource = PartsDT; gv_purchase.DataBind();

            }
            SumTOtalAMount();
            RadioIN.Enabled = false;
            RadioOUT.Enabled = false;
            //sysFunc.ClearTextBoxes(Page);
            txtChargeOutQty.Text = "";
            txtRate.Text = "";
            txtStockQty.Text = "";
            txtWAR.Text = "";
            txtAmount.Text = "";
            txtItemCode.Text = "";
            txtItemDesc.Text = "";
            txtPartItemNo_Parts.Text = "";
            //ddlChPartNo.SelectedIndex = 0;

        }

        protected void SelectedPartDetail(string item)
        {
            try
            {
                if (btnValue == 1)
                {
                    ddlChargeOutNo.SelectedValue = item;
                    ddlChargeOutNo_SelectedIndexChanged(null, null);
                }
                else { 

                DataTable dt = new DataTable();
                dt = sysFuncs.GetData("select ItemDesc,PartItemNo from Item where DealerCode in ('COMON','" + Session["DealerCode"].ToString() + "') And  ItemCode='" + item + "'");
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

        protected void gv_purchase_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    Label lblAmt=(Label)e.Row.FindControl("LbAmount");//LbAmount
            //    Label lblAvgCost = (Label)e.Row.FindControl("LbAvgCost");
            //    Label lblIssueQty = (Label)e.Row.FindControl("LbIssueQty");
            //    double DAvgCost = 0.0, DIssueQty = 0.0;
            //    DAvgCost = Convert.ToDouble(lblAvgCost.Text == "" ? "0" : lblAvgCost.Text);
            //    DIssueQty = Convert.ToDouble(lblIssueQty.Text == "" ? "0" : lblIssueQty.Text);
            //    lblAmt.Text = Convert.ToString(DAvgCost * DIssueQty);
            //}
        }

        protected void gv_purchase_DataBound(object sender, EventArgs e)
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

        protected void btnLookup_Click(object sender, EventArgs e)
        {
            ViewState["lookupid"] = 10; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_Parts_PO(imgLookup, ViewState["lookupid"].ToString(), "", "../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }

        protected void txtRate_TextChanged(object sender, EventArgs e)
        {
            //double tRate = Convert.ToDouble(txtRate.Text);
        }

        protected void gv_purchase_SelectedIndexChanged(object sender, GridViewPageEventArgs e)
        {
            gv_purchase.PageIndex = e.NewPageIndex;
            gv_purchase.DataSource = (DataTable) Session["StockDetail"];
            gv_purchase.DataBind();
        }

        protected void txtAmount_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtChargeOutDate_TextChanged(object sender, EventArgs e)
        {
            //txtChargeOutDate.Text= DateTime.Now.ToString("dd-MM-yyyy");
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //Coneection String by default empty  
            string ConStr = "";
            //Extantion of the file upload control saving into ext because   
            //there are two types of extation .xls and .xlsx of Excel   
            HttpPostedFile postedFile = FileUpload1.PostedFile;

            if (FileUpload1.HasFile == false)
            {
                sysFunc.UserMsg(lblMessage, Color.Red, "Choose file first");
                return;
            }

            string ext = Path.GetExtension(FileUpload1.FileName).ToLower();
            //getting the path of the file   
            string path = Server.MapPath("~/Download/" + FileUpload1.FileName);
            //saving the file inside the MyFolder of the server  
            FileUpload1.SaveAs(path);
            //Label1.Text = FileUpload1.FileName + "\'s Data showing into the GridView";
            //checking that extantion is .xls or .xlsx  

            if(ext.Trim() == ".xls" || ext.Trim() == ".xlsx")
            {

                try
                {
                    if (ext.Trim() == ".xls")
                    {
                        //connection string for that file which extantion is .xls  
                        ConStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (ext.Trim() == ".xlsx")
                    {
                        //connection string for that file which extantion is .xlsx  
                        ConStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    //making query  
                    string query = "SELECT * FROM [Sheet1$]";
                    //Providing connection  
                    OleDbConnection conn = new OleDbConnection(ConStr);
                    //checking that connection state is closed or not if closed the   
                    //open the connection  
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    //create command object  
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    // create a data adapter and get the data into dataadapter  
                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    DataSet ds = new DataSet();


                    //fill the Excel data to data set  
                    da.Fill(ds);
                    //set data source of the grid view  
                    DataTable table = ds.Tables[0];

                    object sumObject;
                    sumObject = table.Compute("Sum(Amount)", "");

                    //var result = table.AsEnumerable()
                    //   .Sum(x => Convert.ToInt32(x["Amount"]));


                    lblTotalAMount.Text = sumObject.ToString();

                    for (int i = ds.Tables[0].Rows.Count - 1; i >= 0; i--)
                    {
                        if (ds.Tables[0].Rows[i][0].ToString() == String.Empty)
                        {
                            ds.Tables[0].Rows.RemoveAt(i);
                        }
                    }

                    //foreach (DataRow row in ds.Tables[0].Rows)
                    //{
                    //    bool IsEmpty = false;
                    //    foreach (object obj in row.ItemArray)
                    //    {
                    //        if (String.IsNullOrEmpty(obj.ToString()))
                    //        {
                    //            IsEmpty = true;
                    //        }
                    //        else
                    //        {
                    //            IsEmpty = false;
                    //        }
                    //    }
                    //    if (IsEmpty)
                    //    {
                    //        ds.Tables[0].Rows.Remove(row);
                    //    }
                    //}

                    Session["StockDetail"] = ds.Tables[0];

                    gv_purchase.DataSource = ds.Tables[0];
                    //binding the gridview  
                    gv_purchase.DataBind();
                    //close the connection  w
                    conn.Close();
                }
                catch(Exception ex)
                {
                    sysFunc.UserMsg(lblMessage, Color.Red, ex.Message);
                    return;
                }
                
            }else
            {
                sysFunc.UserMsg(lblMessage, Color.Red, "Please Select the Excel sheet with extension .xls or .xlsx. This format is not suppoerted.");
                return;
            }
           
        }

        protected void btnGatePass_Click(object sender, EventArgs e)
        {
            try
            {
                string oldgatepass, postflag;
                oldgatepass = sysFuncs.GetStringValuesAgainstCodes("Module='Spare Part Stock Adj' and TransCode='" + ddlChargeOutNo.SelectedValue.ToString() + "'", "GatePassTemp", "GatePassCode");
                postflag = sysFuncs.GetStringValuesAgainstCodes("ChargeOutNo='" + ddlChargeOutNo.SelectedValue.ToString() + "'", "ChargeOutMaster", "PostFlag");
                if (ddlChargeOutNo.SelectedIndex == 0)
                {
                    lblImp.Text = "Please Select the Record First!";
                    lblImp.ForeColor = Color.Red;
                    return;
                }

                //if (postflag == "N")
                //{
                //    lblImp.Text = "Please Post the Record First!";
                //    lblImp.ForeColor = Color.Red;
                //    return;

                //}
                if (oldgatepass == null || oldgatepass == "")
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
                    string gatepass = sysFuncs.AutoGen("GatePassTemp", "GatePassCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                    GatePass_param[0].Value = Session["DealerCode"].ToString();
                    GatePass_param[1].Value = gatepass;
                    GatePass_param[2].Value = ddlChargeOutNo.SelectedValue.ToString();
                    if (RadioIN.Checked) GatePass_param[3].Value = "N";
                    else GatePass_param[3].Value = "O";
                    GatePass_param[4].Value = "";
                    GatePass_param[5].Value = "Spare Part Stock Adj";
                    GatePass_param[6].Value = Session["UserName"].ToString();
                    GatePass_param[7].Value = GlobalVar.mUserIPAddress;
                    GatePass_param[8].Value = "N";
                    GatePass_param[9].Value = ddlChargeOutNo.SelectedValue.ToString();
                    if (sysFuncs.ExecuteSP_NonQuery("[Sp_Insert_GatePassTemp]", GatePass_param))
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
            Data.DSReports data = new Data.DSReports();
            ReportDocument RD = new ReportDocument();



            SqlParameter[] param =
        {
             new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
             new SqlParameter("@ChargeOutNo",SqlDbType.Char,8), //1

        };

            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = Gatepass.Trim();

            SqlDataReader rder = null;

            // Store Procedure check and load
            if (sysFunc.ExecuteSP("SP_ChargeOutMaster_GatePass_Print", param, ref rder)) //Database SP
            {
                data.EnforceConstraints = false;
                data.SP_ChargeOutMaster_GatePass_Print.Load(rder);                       // DataSet SP

            }
            // Load in crystal report
            //RD.Load(Server.MapPath("../PurchaseSaleReturnReport.rpt"));
            //RD.Load(Server.MapPath("../crtStockAdjustment.rpt"));
            RD.Load(Server.MapPath("../SpareReports/crpStockAdjustmentGatePass.rpt"));

            // mention paper size
            RD.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;


            //RD.Load(Server.MapPath("../../SpareReports/rptInvoices.rpt"));
            // RD.Load(Server.MapPath("../rptJobHistory.rpt"));
            //RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + sysFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Phone1", "Dealer") + "'";
            // RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + sysFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Email", "Dealer") + "'";

            RD.DataDefinition.FormulaFields["DealerDesc"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["FromCode"].Text = "'" + FDate + "'";
            //RD.DataDefinition.FormulaFields["ToCode"].Text = "'" + TDate + "'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "Stock Adjustment Gate Pass" + "\"";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";

            RD.Database.Tables[0].SetDataSource(data);

            // convert and show
            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "StockAdjustment" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;

            Session["RD"] = RD;

            //RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            string URL;
            URL = "../../../Download/rptViewerService.aspx?FileName=" + FileName;
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1200,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

        }

        protected void imgSALookup_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 61; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_ChargeOut(imgSALookup, ViewState["lookupid"].ToString(), "", "../../../");

            btnValue = 1;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }

        protected void btnGL_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2548", "005"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (RadioOUT.Checked == true)
            {
                string URL;
                URL = "../../Service/Forms/JV.aspx?CusInv=" + ddlChargeOutNo.SelectedValue + "&Type=SA";
                string fullURL = "window.open('" + URL + "', '_blank', 'height=600,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
            }
            
        }

        protected void txtChargeOutQty_TextChanged(object sender, EventArgs e)
        {

        }
    }
}