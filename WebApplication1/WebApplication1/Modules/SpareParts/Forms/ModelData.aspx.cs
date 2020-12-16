using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DXBMS.Modules.SpareParts.Forms
{
    public partial class ModelData : System.Web.UI.Page
    {
        SysFunction sysfun = new SysFunction();
        SysFunctions sysfuns = new SysFunctions();
        DataTable dt = new DataTable(); static int LookUpPageId;
        DataTable JobDTearch = new DataTable("JobDTearch");
        //Hashtable htColValues = new Hashtable();
        string ColumnsName;
        string AppendWhere;
        string AppendInGroupBy;
        string AppendOrderby;
        static DataSet ds;
        string query = string.Empty;
        string query1 = string.Empty;
        string AppendInWhere=string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (this.Session["UserName"] == null)
                {
                    Response.Redirect("~/login.aspx");

                }
                if (this.Session["MDHeading"] == null)
                {
                    lblHeading.Text = "Model Data";

                }
                else if (this.Session["MDHeading"].ToString() == "Purchase Order")
                {
                    lblHeading.Text = "Purchase Order";

                }
                else if (this.Session["MDHeading"].ToString() == "Order Confirmation")
                {
                    lblHeading.Text = "Order Confirmation";

                }
                else if (this.Session["MDHeading"].ToString() == "Goods Receipt Note")
                {
                    lblHeading.Text = "Goods Receipt Note";

                }
                else if (this.Session["MDHeading"].ToString() == "Payable Invoice")
                {
                    lblHeading.Text = "Payable Invoice";

                }
                else if (this.Session["MDHeading"].ToString() == "Purchase Return")
                {
                    lblHeading.Text = "Purchase Return";

                }
                else if (this.Session["MDHeading"].ToString() == "Direct Sales")
                {
                    lblHeading.Text = "Direct Sales";

                }
                else if (this.Session["MDHeading"].ToString() == "Stock Issue")
                {
                    lblHeading.Text = "Stock Issue";


                }
                else if (this.Session["MDHeading"].ToString() == "Stock Adjustmnet")
                {
                    lblHeading.Text = "Stock Adjustmnet";

                }

                if (!Page.IsPostBack)
                {
                    string sql="";
                   
                    if (this.Session["MDHeading"].ToString() == "Purchase Order")
                    {
                        LookUpPageId =11;


                      

                        //sql = "Select POno as 'Purchase Order No',Format(PODate,'dd-MM-yyyy') as 'Purchase Order Date',VendorDCNo as 'Vendor DC No',DeliveryType FROM POMaster P " +
                        //  "INNER JOIN Vendor V ON V.VendorCode = P.VendorCode and V.DealerCode in (P.DealerCode,'COMON') " +
                        //  "Where P.DealerCode='" + Session["DealerCode"].ToString() + "' and DelFlag = 'N'" +
                        // " Order by POno Desc ";
                        query += " Select  " + Environment.NewLine;
                        query += " a.POno as 'Purchase Order', " + Environment.NewLine;
                        query += " Format(a.PODate,'dd-MM-yyyy') as 'Date', " + Environment.NewLine;
                        query += " b.VendorDesc as 'Vendor', " + Environment.NewLine;
                        query += " a.DeliveryType   " + Environment.NewLine;
                        query += " from POMaster a,Vendor b  " + Environment.NewLine;
                        //Where
                        query1 += "   a.DealerCode = '" + Session["DealerCode"].ToString() + "' " + Environment.NewLine;
                        query1 += " and a.DelFlag = 'N' " + Environment.NewLine;
                        query1 += " and a.VendorCode = b.VendorCode " + Environment.NewLine;
                        query1 += " and b.DealerCode in (a.DealerCode,'COMON') " + Environment.NewLine;
                        //  query1 += " and a.PONo Like '%/%' " + Environment.NewLine;

                        AppendOrderby = "Order by a.POno Desc";
                      
                        AppendInWhere = query1;
                      
                        //dt = sysfun.GetData(sql);


                        //GDV.DataSource = dt;

                        //GDV.DataBind();
                        //if (dt.Rows.Count > 0)
                        //{
                        //    GDV.HeaderRow.TableSection = TableRowSection.TableHeader;
                        //}

                    }
                    if (this.Session["MDHeading"].ToString() == "Order Confirmation")
                    {
                        //     sql = "Select DCNo as 'DC No',Format(DCDate, 'dd-MM-yyyy') as 'DC Date',PONo as 'Purchase Order',V.VendorDesc as 'Vendor' "+
                        //"FROM DCMaster P "+
                        //"INNER JOIN Vendor V ON V.VendorCode = P.VendorCode "+
                        //"and V.DealerCode in (P.DealerCode,'COMON') "+
                        //"Where P.DealerCode = '"+Session["DealerCode"].ToString()+ "'  and P.DCNo Not IN  (Select DCNo from GRNMaster Where DealerCode in ('" + Session["DealerCode"].ToString() + "','COMON')) " +
                        //"and DelFlag = 'N' Order by DCno Desc";

                        //    dt = sysfun.GetData(sql);


                        //    GDV.DataSource = dt;

                        //    GDV.DataBind();
                        //    if (dt.Rows.Count > 0 || dt !=null)
                        //    {
                        //        GDV.HeaderRow.TableSection = TableRowSection.TableHeader;
                        //    }
                        LookUpPageId = 20;


                       
                        //sql = "Select POno as 'Purchase Order No',Format(PODate,'dd-MM-yyyy') as 'Purchase Order Date',VendorDCNo as 'Vendor DC No',DeliveryType FROM POMaster P " +
                        //  "INNER JOIN Vendor V ON V.VendorCode = P.VendorCode and V.DealerCode in (P.DealerCode,'COMON') " +
                        //  "Where P.DealerCode='" + Session["DealerCode"].ToString() + "' and DelFlag = 'N'" +
                        // " Order by POno Desc ";
                        query += " Select  " + Environment.NewLine;
                        query += " a.DCNo as 'DC No', " + Environment.NewLine;
                        query += " convert(varchar(12),a.DCDate,103) as 'DC Date' , " + Environment.NewLine;
                        query += " a.PONo, " + Environment.NewLine;
                        query += " b.VendorDesc as 'Vendor'  " + Environment.NewLine;
                        query += " from DCMaster a,Vendor b " + Environment.NewLine;
                        //Where
                        query1 += " a.VendorCode = b.VendorCode  " + Environment.NewLine;
                        query1 += " and a.DealerCode = '" + Session["DealerCode"].ToString() + "' " + Environment.NewLine;
                        query1 += " and a.DelFlag = 'N' " + Environment.NewLine;
                        //query1 += " and a.DealerCode = b.DealerCode  " + Environment.NewLine;
                        query1 += " and a.VendorCode = b.VendorCode  " + Environment.NewLine;
                        query1 += " and b.DealerCode in (a.DealerCode,'COMON') " + Environment.NewLine;
                        query1 += " and a.DCNo Not IN  " + Environment.NewLine;
                        query1 += " (Select DCNo from GRNMaster Where DealerCode = '" + Session["DealerCode"].ToString() + "'and Delflag = 'N')  " + Environment.NewLine;
                        //  query1 += " and a.DCNo Like '%/%' " + Environment.NewLine;

                        AppendOrderby = "Order by a.DCNo Desc";
                        AppendInWhere = query1;
                       


                    }
                    if (this.Session["MDHeading"].ToString() == "Goods Receipt Note")
                    {
                        //     sql = "Select GRNNo as 'GRN No',Format(GRNDate,'dd-MM-yyyy') as 'GRN Date',DCNo as 'Order Cornfirmation',V.VendorDesc as 'Vendor' " +
                        //"FROM GRNMaster P " +
                        //"INNER JOIN Vendor V ON V.VendorCode = P.VendorCode " +
                        //"and V.DealerCode in (P.DealerCode,'COMON') " +
                        //"Where P.DealerCode = '" + Session["DealerCode"].ToString() + "' and DelFlag = 'N' Order by GRNno Desc";

                        //    dt = sysfun.GetData(sql);


                        //    GDV.DataSource = dt;

                        //    GDV.DataBind();
                        //    if (dt.Rows.Count > 0)
                        //    {
                        //        GDV.HeaderRow.TableSection = TableRowSection.TableHeader;
                        //    }
                        LookUpPageId = 38;
                        query += "  Select a.GRNNo as 'GRN No',convert(varchar(20),a.GRnDate,103) as 'GRN Date',PoNo as 'PO No' ,a.DCNo as 'DC NO',b.VendorDesc as 'Vendor'    " + Environment.NewLine;
                        query += "  from GRNMaster a,Vendor b   " + Environment.NewLine;
                        //Where
                        query1 += "  a.DealerCode =  '" + Session["DealerCode"].ToString() + "'  " + Environment.NewLine;
                        query1 += "  and a.DelFlag = 'N'  " + Environment.NewLine;
                        query1 += "  and b.DealerCode in (a.DealerCode,'COMON')   " + Environment.NewLine;
                        query1 += " and a.VendorCode = b.VendorCode   " + Environment.NewLine;

                        AppendOrderby = "Order by a.GRNNo Desc";
                        AppendInWhere = query1;
                       

                    }
                    if (this.Session["MDHeading"].ToString() == "Payable Invoice")
                    {
                        //     sql = "Select PurInvNo as 'Payable Invoice',Format(PurInvDate, 'dd-MM-yyyy') as 'Payable Date',GRNNo as 'GRN No',VendorDCNo as 'Vendor DC',V.VendorDesc as 'Vendor' "+
                        //"FROM PurInvMaster P "+
                        //"INNER JOIN Vendor V ON V.VendorCode = P.VendorCode "+
                        //"and V.DealerCode in (P.DealerCode,'COMON') "+
                        //"Where P.DealerCode = '" + Session["DealerCode"].ToString() + "' and DelFlag = 'N'  Order by PurInvNo Desc";

                        //    dt = sysfun.GetData(sql);


                        //    GDV.DataSource = dt;

                        //    GDV.DataBind();
                        //    if (dt.Rows.Count > 0)
                        //    {
                        //        GDV.HeaderRow.TableSection = TableRowSection.TableHeader;
                        //    }

                        LookUpPageId = 82;
                        query += "Select PurInvNo as 'Inv No' ,Format(PurInvDate,'dd-MM-yyyy') as 'Inv Date' ,GRNNo as 'GRN No' ,VendorDCNo  as 'Vendor DC' ,VendorInvNo as 'Vendor Inv No' " +
                          "From PurInvMaster";
                        query1 += " DealerCode = '" + Session["DealerCode"].ToString() + "'" +
                          "And Delflag='N' "; 

                        AppendOrderby = "Order by PurInvNo Desc ";
                        AppendInWhere = query1;
                    }
                    if (this.Session["MDHeading"].ToString() == "Purchase Return")
                    {
                        // sql = "Select PRNo as 'Purchase Return',Format(PRDate, 'dd-MM-yyyy') as 'PR Date',GRNNo as 'GRN No',V.VendorDesc as 'Vendor' "+
                        //             ",Case when  P.PostFlag = 'N' then 'Not Posted' else 'Posted' end as 'Post' "+
                        //             "FROM PRMaster P "+
                        //             "INNER JOIN Vendor V ON V.VendorCode = P.VendorCode "+
                        //             "and V.DealerCode in (P.DealerCode,'COMON') "+
                        //             "Where P.DealerCode = '" + Session["DealerCode"].ToString() + "' and DelFlag = 'N'  Order by PRNo Desc";

                        //dt = sysfun.GetData(sql);


                        //GDV.DataSource = dt;

                        //GDV.DataBind();
                        //if (dt.Rows.Count > 0)
                        //{
                        //    GDV.HeaderRow.TableSection = TableRowSection.TableHeader;
                        //}
                        LookUpPageId = 69;
                        query += " Select a.PRNo as 'PR No', Format(a.PRDate,'dd-MM-yyyy') as 'PR Date', b.VendorDesc as 'Vendor' from  PRMaster a Inner Join Vendor b On  a.VendorCode = b.VendorCode and b.DealerCode in(a.DealerCode,'COMON') " + Environment.NewLine;
                       
                        //Where
                        query1 += "     a.DealerCode = '" + Session["DealerCode"].ToString() + "'  " + Environment.NewLine;
                        query1 += " and a.DelFlag = 'N' " + Environment.NewLine;
                        
                        AppendInWhere = query1;
                        AppendOrderby = "Order by a.PRNo Desc";


                    }
                    if (this.Session["MDHeading"].ToString() == "Direct Sales")
                    {
                        // sql = " Select SaleInvNo as 'Sale Inv No',Format(SaleInvDate, 'dd-MM-yyyy') as 'Sale Inv Date',ReqNO as 'Requisition',V.CusDesc as 'Customer' " +
                        //            ",Case when  P.PostFlag = 'N' then 'Not Posted' else 'Posted' end as 'Post' " +
                        //            "FROM CounterSaleMaster P " +
                        //            "INNER JOIN Customer V ON V.CusCode = P.CusCode " +
                        //            "and V.DealerCode in (P.DealerCode) " +
                        //            "Where P.DealerCode = '" + Session["DealerCode"].ToString() + "' and DelFlag = 'N'  Order by SaleInvNo Desc";

                        //dt = sysfun.GetData(sql);


                        //GDV.DataSource = dt;

                        //GDV.DataBind();
                        //if (dt.Rows.Count > 0)
                        //{
                        //    GDV.HeaderRow.TableSection = TableRowSection.TableHeader;
                        //}
                        LookUpPageId = 42;
                        query += " Select  " + Environment.NewLine;
                        query += " a.SaleInvNo as 'Sale Inv No.', " + Environment.NewLine;
                        query += "convert(varchar(20),a.SaleInvDate,103) as 'Sale Inv Date' , " + Environment.NewLine;
                        query += " b.CusDesc as 'Customer', " + Environment.NewLine;
                        query += " a.PostFlag " + Environment.NewLine;
                        query += " from CounterSaleMaster a,Customer b  " + Environment.NewLine;
                        //Where
                        query1 += " a.DealerCode = '" + Session["DealerCode"].ToString() + "' " + Environment.NewLine;
                        query1 += " and a.DelFlag = 'N' " + Environment.NewLine;
                        query1 += " and a.DealerCode = b.DealerCode " + Environment.NewLine;
                        query1 += " and a.CusCode = b.CusCode " + Environment.NewLine;
                        AppendOrderby = "Order by SaleInvNo Desc";
                        AppendInWhere = query1;
                    }
                    if (this.Session["MDHeading"].ToString() == "Stock Issue")
                    {
                        // sql = "Select Distinct a.IssueNo, Convert(varchar(10),a.IssueDate,105) 'Issue Date', +IsNull(a.JobCardNo,'')'JC: ',IsNull(a.InvoiceNo,'') 'CS: ' "+
                        //  "from StockIssue a ,StockIssueDetail b "+
                        //  "Where a.DealerCode = '" + Session["DealerCode"].ToString() + "' " +
                        //  "and a.DelFlag = 'N' "+
                        //  "and a.DealerCode = b.DealerCode And a.IssueNo = b.IssueNo "+
                        //  "Order by IssueNo Desc";

                        //dt = sysfun.GetData(sql);


                        //GDV.DataSource = dt;

                        //GDV.DataBind();
                        //if (dt.Rows.Count > 0)
                        //{
                        //    GDV.HeaderRow.TableSection = TableRowSection.TableHeader;
                        //}
                        LookUpPageId = 40;
                        query += " Select  " + Environment.NewLine;
                        query += " Distinct a.IssueNo as 'Issue No', " + Environment.NewLine;
                        query += " convert(varchar(20),a.IssueDate,103) as 'Issue Date', " + Environment.NewLine;
                        query += " JobCardNo, " + Environment.NewLine;
                        query += " InvoiceNo " + Environment.NewLine;

                        query += " from StockIssue a,StockIssueDetail b " + Environment.NewLine;
                        //Where
                        query1 += "     a.DealerCode = '" + Session["DealerCode"].ToString() + "'  " + Environment.NewLine;
                        query1 += " and a.DelFlag = 'N' " + Environment.NewLine;
                        query1 += " and a.DealerCode = b.DealerCode And a.IssueNo = b.IssueNo " + Environment.NewLine;
                        AppendOrderby = "Order by a.IssueNo Desc";
                       
                        AppendInWhere = query1;

                    }
                    if (this.Session["MDHeading"].ToString() == "Stock Adjustmnet")
                    {
                        // sql = "Select ChargeOutNo as 'Stock Adj',Format(ChargeOutDate,'dd-MM-yyyy') as 'Stock Date',StockType as 'Type',Remarks "+
                        //              "FROM ChargeOutMaster P "+
                        //              "Where P.DealerCode = '" + Session["DealerCode"].ToString() + "' and DelFlag = 'N'  Order by ChargeOutNo Desc ";

                        //dt = sysfun.GetData(sql);


                        //GDV.DataSource = dt;

                        //GDV.DataBind();
                        //if (dt.Rows.Count > 0)
                        //{
                        //    GDV.HeaderRow.TableSection = TableRowSection.TableHeader;
                        //}

                        LookUpPageId = 61;
                        query += " Select distinct  ChargeOutNo as 'Charge Out No',Format(ChargeOutDate,'dd-MM-yyyy') as 'ChargeOut Date', Remarks from ChargeOutMaster  " + Environment.NewLine;
                        //query += " convert(varchar(20),a.IssueDate,103) as IssueDate, " + Environment.NewLine;
                        //query += " JobCardNo, " + Environment.NewLine;
                        //query += " InvoiceNo " + Environment.NewLine;

                        //query += " from StockIssue a,StockIssueDetail b " + Environment.NewLine;
                        //Where
                        query1 += "     DealerCode = '" + Session["DealerCode"].ToString() + "'  " + Environment.NewLine;
                        query1 += " and DelFlag = 'N' " + Environment.NewLine;
                        AppendOrderby = "Order by ChargeOutNo Desc";
                        //query1 += " and a.DealerCode = b.DealerCode And a.IssueNo = b.IssueNo " + Environment.NewLine;

                        AppendInWhere = query1;

                    }

                  

                    sql = "exec sp_LookUp_Select '" + LookUpPageId + "'";

                    dt = sysfun.GetData(sql);


                    rdblColumns.DataSource = dt;
                    rdblColumns.DataTextField = "DisplayText";
                    rdblColumns.DataValueField = "ColumnValue";
                    rdblColumns.DataBind();

                    if (rdblColumns.Items.Count != 0)
                    {
                        rdblColumns.ClearSelection();
                        rdblColumns.SelectedIndex = 0;
                    }
                    HttpContext.Current.Session["ColumnsName"] = query;
                    HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
                    HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
                    getData();

                    //DataBind();
                    //BindDummyRow();

                }

            }

        }
        protected void getData()
        {
            ColumnsName = Session["ColumnsName"].ToString();
            AppendWhere = Session["AppendInWhere"].ToString();
            if (Session["AppendOrderby"] != null) AppendOrderby = Session["AppendOrderby"].ToString();
            if (Request.QueryString["Grp"] != null) AppendInGroupBy = Request.QueryString["Grp"].ToString();
            string sQuery;
            if (LookUpPageId.ToString() != string.Empty)
            {
                clsLookUp clp = new clsLookUp();
                if (AppendInGroupBy != string.Empty)
                {
                    if (txtSearch.Text != string.Empty)
                    {
                        if (AppendWhere != string.Empty)
                        {
                            sQuery = ColumnsName + " Where " + AppendWhere + " and " +
                                rdblColumns.SelectedValue.ToString() + " like '%" + txtSearch.Text.Trim() + "%' " + AppendInGroupBy;
                        }
                        else
                        {
                            sQuery = ColumnsName + " Where " +
                                rdblColumns.SelectedValue.ToString() + " like '%" + txtSearch.Text.Trim() + "%' " + AppendInGroupBy;
                        }
                    }
                    else
                    {
                        if (AppendWhere != string.Empty) sQuery = ColumnsName + " Where " + AppendWhere + " " + AppendInGroupBy; else sQuery = ColumnsName + " " + AppendInGroupBy;
                    }
                }
                else
                {
                    if (txtSearch.Text != string.Empty)
                    {
                        if (AppendWhere != string.Empty)
                        {
                            sQuery = ColumnsName + " Where " + AppendWhere + " and " +
                                rdblColumns.SelectedValue.ToString() + " like '%" + txtSearch.Text.Trim() + "%'";
                        }
                        else
                        {
                            sQuery = ColumnsName + " Where " +
                                rdblColumns.SelectedValue.ToString() + " like '%" + txtSearch.Text.Trim() + "%'";
                        }
                    }
                    else
                    {
                        if (AppendWhere != string.Empty) sQuery = ColumnsName + " Where " + AppendWhere; else sQuery = ColumnsName;
                    }
                }
                if (AppendOrderby != string.Empty) sQuery = sQuery + " " + AppendOrderby;

          
                if (txtSearch.Text == string.Empty)
                {
                    if (sysfuns.ExecuteQuery(sQuery, ref ds))
                    {
                        GDV.AllowPaging = true;
                        GDV.PageSize = 15;
                        //Session["Parts"] = ds;
                        Session["SearchParts"] = ds;
                        GDV.DataSource = ds;
                        GDV.DataBind();

                        //GDV.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
                }
                else
                {
                    if (sysfuns.ExecuteQuery(sQuery, ref ds))
                    {
                        GDV.AllowPaging = false;
                        //GDV.PageSize = 15;
                        //Session["Parts"] = ds;
                        Session["SearchParts"] = ds;
                        GDV.DataSource = ds;
                        GDV.DataBind();

                        //GDV.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
                }


            }
        }
        protected void GDV_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GDV.Rows[GDV.SelectedRow.RowIndex];
            if (this.Session["MDHeading"].ToString() == "Purchase Order")
            {
                ViewState["TransCode"] = GDV.SelectedRow.Cells[1].Text;
                Session["TransCode"] = GDV.SelectedRow.Cells[1].Text;
                Session["Mode"] = "Edit";
                Response.Redirect("~/Modules/SpareParts/Forms/PurchaseOrderNew.aspx");
                
            }
            if (this.Session["MDHeading"].ToString() == "Order Confirmation")
            {
                ViewState["TransCode"] = GDV.SelectedRow.Cells[1].Text;
                Session["TransCode"] = GDV.SelectedRow.Cells[1].Text;
                Session["Mode"] = "Edit";
                Response.Redirect("~/Modules/SpareParts/Forms/OrderConfirmation.aspx");
               
            }
            if (this.Session["MDHeading"].ToString() == "Goods Receipt Note")
            {
                ViewState["TransCode"] = GDV.SelectedRow.Cells[1].Text;
                Session["TransCode"] = GDV.SelectedRow.Cells[1].Text;
                Session["Mode"] = "Edit";
                Response.Redirect("~/Modules/SpareParts/Forms/GRNForm.aspx");

            }
            if (this.Session["MDHeading"].ToString() == "Payable Invoice")
            {
                ViewState["TransCode"] = GDV.SelectedRow.Cells[1].Text;
                Session["TransCode"] = GDV.SelectedRow.Cells[1].Text;
                Session["Mode"] = "Edit";
                Response.Redirect("~/Modules/SpareParts/Forms/PayableInvoice.aspx");

            }
            if (this.Session["MDHeading"].ToString() == "Purchase Return")
            {
                ViewState["TransCode"] = GDV.SelectedRow.Cells[1].Text;
                Session["TransCode"] = GDV.SelectedRow.Cells[1].Text;
                Session["Mode"] = "Edit";
                Response.Redirect("~/Modules/SpareParts/Forms/PurchaseReturn.aspx");

            }
            if (this.Session["MDHeading"].ToString() == "Direct Sales")
            {
                ViewState["TransCode"] = GDV.SelectedRow.Cells[1].Text;
                Session["TransCode"] = GDV.SelectedRow.Cells[1].Text;
                Session["Mode"] = "Edit";
                Response.Redirect("~/Modules/SpareParts/Forms/CounterSalesInv.aspx");

            }
            if (this.Session["MDHeading"].ToString() == "Stock Issue")
            {
                ViewState["TransCode"] = GDV.SelectedRow.Cells[1].Text;
                Session["TransCode"] = GDV.SelectedRow.Cells[1].Text;
                Session["Mode"] = "Edit";
                Response.Redirect("~/Modules/SpareParts/Forms/StockIssueFrm.aspx");

            }
            if (this.Session["MDHeading"].ToString() == "Stock Adjustmnet")
            {
                ViewState["TransCode"] = GDV.SelectedRow.Cells[1].Text;
                Session["TransCode"] = GDV.SelectedRow.Cells[1].Text;
                Session["Mode"] = "Edit";
                Response.Redirect("~/Modules/SpareParts/Forms/StockAdjustment.aspx");

            }

        }

        protected void GDV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GDV.PageIndex = e.NewPageIndex;
            getData();


        }
        protected void  // ERROR: Handles clauses are not supported in C#
    GDV_ColumnHeaderSorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                string sortExpression = e.SortExpression;
                if (SortDirection == e.SortDirection)
                {
                    SortDirection = SortDirection.Descending;
                    SortRecords(sortExpression, "Desc");
                    SortDirection = SortDirection.Descending;
                }
                else
                {
                    SortDirection = SortDirection.Ascending;
                    SortRecords(sortExpression, "Asc");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        #region "Prpoerties"
        public SortDirection SortDirection
        {
            get
            {
                if (Session["SortDirection"] == null) Session["SortDirection"] = SortDirection.Ascending;
                return (SortDirection)Session["SortDirection"];
            }
            set { Session["SortDirection"] = value; }
        }
        private void CloseWindow()
        {
            try
            {
                string closeWindow = "<script language='javascript' type='text/javascript'> window.opener.document.forms[0].submit(); window.close(); </script>";
                Response.Write(closeWindow);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                GC.Collect();
            }
        }
        private void SortRecords(string sortExpression, string direction)
        {
            try
            {
                DataView dv = new DataView();
                dv = new DataView(JobDTearch);
                if (dv.Count > 0)
                {
                    if (string.IsNullOrEmpty(sortExpression))
                    {
                        sortExpression = JobDTearch.Columns[1].ToString();
                        dv.Sort = sortExpression;
                    }
                    else
                    {
                        dv.Sort = sortExpression + " " + direction;
                        GDV.DataSource = dv;
                    }
                }
                else GDV.DataSource = null;
                GDV.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region "Functioins"
        #endregion

        protected void btnAdd_Click(object sender, ImageClickEventArgs e)
        {
            if (this.Session["MDHeading"].ToString() == "Purchase Order")
            {
                ViewState["TransCode"] = null;
                Session["TransCode"] = null;
                Session["Mode"] = "Add Mode";
                Response.Redirect("~/Modules/SpareParts/Forms/PurchaseOrderNew.aspx");

            }
            if (this.Session["MDHeading"].ToString() == "Order Confirmation")
            {
                ViewState["TransCode"] = null;
                Session["TransCode"] = null;
                Session["Mode"] = "Add Mode";
                Response.Redirect("~/Modules/SpareParts/Forms/OrderConfirmation.aspx");

            }
            if (this.Session["MDHeading"].ToString() == "Goods Receipt Note")
            {
                ViewState["TransCode"] = null;
                Session["TransCode"] = null;
                Session["Mode"] = "Add Mode";
                Response.Redirect("~/Modules/SpareParts/Forms/GRNForm.aspx");

            }
            if (this.Session["MDHeading"].ToString() == "Payable Invoice")
            {
                ViewState["TransCode"] = null;
                Session["TransCode"] = null;
                Session["Mode"] = "Add Mode";
                Response.Redirect("~/Modules/SpareParts/Forms/PayableInvoice.aspx");

            }
            if (this.Session["MDHeading"].ToString() == "Purchase Return")
            {
                ViewState["TransCode"] = null;
                Session["TransCode"] = null;
                Session["Mode"] = "Add Mode";
                Response.Redirect("~/Modules/SpareParts/Forms/PurchaseReturn.aspx");

            }
            if (this.Session["MDHeading"].ToString() == "Direct Sales")
            {
                ViewState["TransCode"] = null;
                Session["TransCode"] = null;
                Session["Mode"] = "Add Mode";
                Response.Redirect("~/Modules/SpareParts/Forms/CounterSalesInv.aspx");

            }
            if (this.Session["MDHeading"].ToString() == "Stock Issue")
            {
                ViewState["TransCode"] = null;
                Session["TransCode"] = null;
                Session["Mode"] = "Add Mode";
                Response.Redirect("~/Modules/SpareParts/Forms/StockIssueFrm.aspx");

            }
            if (this.Session["MDHeading"].ToString() == "Stock Adjustmnet")
            {
                ViewState["TransCode"] = null;
                Session["TransCode"] = null;
                Session["Mode"] = "Add Mode";
                Response.Redirect("~/Modules/SpareParts/Forms/StockAdjustment.aspx");

            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtSearch.Text = "";

                if (LookUpPageId == 10)
                {
                    Session["SearchParts"] = null;
                    GDV.AllowPaging = true;
                    GDV.PageSize = 15;
                    GDV.DataSource = (DataSet)Session["Parts"];
                    GDV.DataBind();
                }
                else
                {
                    getData();
                }




            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSearch.Text.Trim() != "")
                {
                    getData();
                }
                else
                {
                    Session["SearchParts"] = null;
                    GDV.AllowPaging = true;
                    GDV.PageSize = 15;
                    GDV.DataSource = (DataSet)Session["Parts"];
                    GDV.DataBind();
                }

            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }
    }
}