using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Data.SqlClient;
using CConn;
using System.Text.RegularExpressions;

namespace DXBMS.Modules.Service.Forms
{
    public partial class CI : System.Web.UI.Page
    {
        SecurityBll sec = new SecurityBll();
        SysFunction sysfun = new SysFunction();
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        private static int PageSize = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2502", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            txtSearch.Attributes.Add("onchange", "setTimeout('__doPostBack(\\'" + txtSearch.ClientID.Replace("_", "$") + "\\',\\'\\')', 0);");
            hfTab.Value = Request.Form[hfTab.UniqueID];
            if (!Page.IsPostBack)
            {
                GetTotalCount();
                GetPageData(0, 10);


                //string sql = "Select_PendingInv '" + Session["DealerCode"].ToString() + "'   ";
                //string sql1 = "Select_closeJobCard '" + Session["DealerCode"].ToString() + "' ";
                //hfTab.Value = Request.Form[hfTab.UniqueID];



                //dt = sysfun.GetData(sql);
                //dt2 = sysfun.GetData(sql1);

                //GridView1.DataSource = dt;
                //GridView1.DataBind();
                //if (dt.Rows.Count > 0)
                //{
                //    GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
                //}


                //GridView2.DataSource = dt2;
                //GridView2.DataBind();
                //if (dt2.Rows.Count > 0)
                //{
                //    GridView2.HeaderRow.TableSection = TableRowSection.TableHeader;
                //}

                //DataBind();
                //BindDummyRow();

            }


        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string SearchString = txtSearch.Text;
        }
        public string HighlightText(string InputTxt)
        {
            string Search_Str = txtSearch.Text.ToString();
            // Setup the regular expression and add the Or operator.
            Regex RegExp = new Regex(Search_Str.Replace(" ", "|").Trim(), RegexOptions.IgnoreCase);
            // Highlight keywords by calling the 
            //delegate each time a keyword is found.
            return RegExp.Replace(InputTxt, new MatchEvaluator(ReplaceKeyWords));
            // Set the RegExp to null.
            RegExp = null;
        }
        public string ReplaceKeyWords(Match m)
        {
            return "<span class=highlight>" + m.Value + "</span>";
        }


        private void BindDummyRow()
        {
            DataTable dummy = new DataTable();
            dummy.Columns.Add("JobCardCode");
            dummy.Columns.Add("UserName");
            dummy.Columns.Add("RegNo");
            dummy.Columns.Add("EngineNo");
            dummy.Columns.Add("ChassisNo");
            dummy.Rows.Add();

            //GridView1.DataSource = dummy;
            //GridView1.DataBind();
            GridView2.DataSource = dummy;
            GridView2.DataBind();
        }

        //public void DataBind()
        //{
        //    GridView2.DataSource = dt2;
        //    GridView2.DataBind();
        //}

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!sec.UserRight("2532", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            GridViewRow row = GridView1.SelectedRow;
            ViewState["TransCode"] = GridView1.SelectedRow.Cells[1].Text;
            Session["TransCode"] = GridView1.SelectedRow.Cells[1].Text;
            Session["Mode"] = "Edit";

            Response.Redirect("~/Modules/Service/Forms/CustomerInvoice.aspx");

        }

        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!sec.UserRight("2532", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            GridViewRow row = GridView2.SelectedRow;
            ViewState["TransCode"] = GridView2.SelectedRow.Cells[1].Text;
            Session["TransCode"] = GridView2.SelectedRow.Cells[1].Text;
            Session["Mode"] = "Edit";

            Response.Redirect("~/Modules/Service/Forms/CustomerInvoice.aspx");

        }


        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                e.Row.ToolTip = "Click to Select";
                e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.GridView1, "Select$" + e.Row.RowIndex);
            }
        }

        protected void GridView2_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                e.Row.ToolTip = "Click to Select";
                e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.GridView2, "Select$" + e.Row.RowIndex);
            }
        }

        [WebMethod]
        public static string GetCustomers(string searchTerm, int pageIndex)
        {
            string query = "[GetClosedJobs_Pager]";
            SqlCommand cmd = new SqlCommand(query);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);
            cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
            cmd.Parameters.AddWithValue("@PageSize", PageSize);
            cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
            return GetData(cmd, pageIndex).GetXml();
        }

        [WebMethod]
        public static string GetOpenJobs(string searchTerm, int pageIndex)
        {
            string query = "[GetOpenJobs_Pager]";
            SqlCommand cmd = new SqlCommand(query);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);
            cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
            cmd.Parameters.AddWithValue("@PageSize", PageSize);
            cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
            return GetData(cmd, pageIndex).GetXml();
        }

        public static DataSet GetData(SqlCommand cmd, int pageIndex)
        {
            string strConnString = CConnection.GetConnectionString();
            using (SqlConnection con = new SqlConnection(strConnString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds, "JobCardMaster");
                        DataTable dt = new DataTable("Pager");
                        dt.Columns.Add("PageIndex");
                        dt.Columns.Add("PageSize");
                        dt.Columns.Add("RecordCount");
                        dt.Rows.Add();
                        dt.Rows[0]["PageIndex"] = pageIndex;
                        dt.Rows[0]["PageSize"] = PageSize;
                        dt.Rows[0]["RecordCount"] = cmd.Parameters["@RecordCount"].Value;
                        ds.Tables.Add(dt);
                        return ds;
                    }
                }
            }
        }

        protected void GetDealerCode(object sender, SqlDataSourceSelectingEventArgs e)
        {
          //  SqlDataSource2.SelectParameters.Add("@DealerCode", Session["DealerCode"].ToString());
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;

            GetPageData(e.NewPageIndex, 10);
        }
        private int GetTotalCount()

        {

            string sql = "Select Count(*) from CustomerInvoice where Delflag='N' and DealerCode='" + Session["DealerCode"].ToString() + "'";
            int count = 0;
            dt = new DataTable();
            dt = sysfun.GetData(sql);
            if (dt.Rows.Count > 0)
            {
                count = Convert.ToInt16(SysFunctions.CustomCDBL(dt.Rows[0][0].ToString()));
            }

            return count;

        }
        public void GetPageData(int current, int pagesize)

        {
            string sql = "Select_PendingInv '" + Session["DealerCode"].ToString() + "','"+ pagesize + "','"+current+"'   ";
            string sql1 = "Select_ClosedInv '" + Session["DealerCode"].ToString() + "','" + pagesize + "','" + current + "'   ";
            hfTab.Value = Request.Form[hfTab.UniqueID];



            dt = sysfun.GetData(sql);
            dt2 = sysfun.GetData(sql1);
            GridView2.DataSource = dt2;
            GridView2.DataBind();
            if (dt2.Rows.Count > 0)
            {
                GridView2.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            GridView1.DataSource = dt;
            GridView1.DataBind();
            if (dt.Rows.Count > 0)
            {
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            
    }
}
}
