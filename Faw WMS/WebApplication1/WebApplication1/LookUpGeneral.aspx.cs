using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DXBMS;
using System.Data.SqlClient;
using CConn;
using Microsoft.ApplicationBlocks.Data;

namespace DXBMS
{
    public partial class LookUpGeneral : System.Web.UI.Page
    {
        SysFunctions objMBL = new SysFunctions();
        //MainBLL objMBL = new MainBLL();
        static int LookUpPageId;
        DataTable JobDTearch = new DataTable("JobDTearch");
        //Hashtable htColValues = new Hashtable();
        string ColumnsName;
        string AppendWhere;
        string AppendInGroupBy;
        string AppendOrderby;
        static DataSet ds;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    ColumnsName = Session["ColumnsName"].ToString();
                    AppendWhere = Session["AppendInWhere"].ToString();

                    if (Request.QueryString.Count > 0)
                    {
                        LookUpPageId = int.Parse(Request.QueryString.Get("lpi").ToString());

                        //SqlParameter[] param = {
                        //        new SqlParameter("@LookUpId",SqlDbType.Int) };
                        //param[0].Value = LookUpPageId;
                        DataTable dt = new DataTable();

                        string sql = "exec sp_LookUp_Select '" + LookUpPageId + "'";

                        dt = objMBL.GetData(sql);

                        //rdblColumns.DataSource = objMBL.FillDataSet("sp_LookUp_Select", param);
                        rdblColumns.DataSource = dt;
                        rdblColumns.DataTextField = "DisplayText";
                        rdblColumns.DataValueField = "ColumnValue";
                        rdblColumns.DataBind();

                        if (rdblColumns.Items.Count != 0)
                        {
                            rdblColumns.ClearSelection();
                            rdblColumns.SelectedIndex = 0;
                        }
                        
                        getData();

                    }
                }

                txtSearch.Focus();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }

        }
        
        public void BindGridView(int pageNo)
        {
            int pageSize = 15;
            GDV.PageSize = pageSize;

            try
            {
                string sQuery = "sp_GetProductByCustomPaging '" + pageNo + "','" + pageSize + "','" + Session["DealerCode"].ToString() + "', 'P'";

                if (objMBL.ExecuteQuery(sQuery, ref ds))
                {
                    Session["Parts"] = ds;
                    GDV.VirtualItemCount = Convert.ToInt32(ds.Tables[1].Rows[0]["Total"]);
                    GDV.DataSource = ds.Tables[0];
                    GDV.DataBind();
                }


            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            finally
            {
            }
        }

        protected void txtSearch_TextChanged(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                btnSearch_Click(sender, e);
            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
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
        public void GetDSdata(DataTable dt, string field)
        {

            var results = from myRow in dt.AsEnumerable()
                          where myRow.Field<string>(field).ToUpper().StartsWith(txtSearch.Text.ToUpper())
                          select myRow;
            DataView view = results.AsDataView();
            GDV.DataSource = view;
            GDV.DataBind();
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

                if (LookUpPageId == 75)
                {
                    string CCon = CConnection.GetConnStringForAccount();

                    ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, sQuery);
                    GDV.AllowPaging = false;
                    GDV.PageSize = ds.Tables[0].Rows.Count;
                    //Session["Parts"] = ds;
                    //Session["SearchParts"] = ds;
                    GDV.DataSource = ds;
                    GDV.DataBind();
                }
                else
                {
                    if (objMBL.ExecuteQuery(sQuery, ref ds))
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


            }
        }
        protected void GDV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GDV.PageIndex = e.NewPageIndex;

            //if(Session["SearchParts"] != null)
            //{
            //    ds = (DataSet)Session["SearchParts"];
            //    GDV.AllowPaging = false;
            //    GDV.PageSize = ds.Tables[0].Rows.Count;
            //    GDV.DataSource = ds;
            //    GDV.DataBind();
            //}else
            //{
            //ds = (DataSet)Session["Parts"];
            //GDV.AllowPaging = true;
            //GDV.PageSize = 15;
            //GDV.DataSource = ds;
            //GDV.DataBind();
            //}
               getData();

            //getData();
            //BindGridView(e.NewPageIndex + 1);
            //getDataWithSP();
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
        protected void GDV_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GDV.Rows[GDV.SelectedRow.RowIndex];

            Session["LookUpData"] = GDV.SelectedRow.Cells[1].Text;

            Session["SearchParts"] = null;
            CloseWindow();
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
    }
}