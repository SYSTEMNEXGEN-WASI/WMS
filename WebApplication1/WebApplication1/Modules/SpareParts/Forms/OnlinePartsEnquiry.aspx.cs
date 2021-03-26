using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DXBMS.Modules.SpareParts.Forms
{
    public partial class OnlinePartsEnquiry : System.Web.UI.Page
    {

        SysFunction sysFunc = new SysFunction();
        SysFunctions sysFuncs = new SysFunctions();

        protected void Page_Load(object sender, EventArgs e)
        {

            createGrid();
        }

        private void createGrid()
        {
            DataTable PartsDT = new DataTable();
            PartsDT.Columns.Add("SNo", typeof(int));
            PartsDT.Columns.Add(new DataColumn("Dealer", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("PartNo", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("PartDescription", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("Source", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("Quantity", typeof(string)));

            gv_DC.DataSource = PartsDT; gv_DC.DataBind(); Session["DCTable"] = PartsDT;
        }
        

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            // Data load in ddlchargoutNo
            SqlParameter[] param =
       {
                new SqlParameter("@PartNo",SqlDbType.VarChar,50),//0
                new SqlParameter("@PartDesc",SqlDbType.VarChar,100), //1
                new SqlParameter("@Source",SqlDbType.VarChar,10),//2
                new SqlParameter("@qty",SqlDbType.Int), //3
            };
            param[0].Value = txtPartNo.Text;// 0
            param[1].Value = txtPartDesc.Text;
            param[2].Value = txtSource.Text; // 0
            param[3].Value = txtQty.Text;

            DataSet ds = new DataSet();
            ds = sysFuncs.FillDataSet("SP_OnlinePartCheck", param);
            gv_DC.DataSource = ds;
            gv_DC.DataBind();
        }
    }
}