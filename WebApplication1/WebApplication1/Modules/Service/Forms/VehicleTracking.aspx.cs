using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DXBMS.Modules.Service.Forms
{
    public partial class VehicleTracking : System.Web.UI.Page
    {
        DataSet ds;
        SysFunction sysfun = new SysFunction();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!IsPostBack)
            {
                txtFromDate.Text = txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                Create_Grid();                
            }
        }



        private void Create_Grid()
        {
            ds = new DataSet();

            ds.Tables.Add();

            ds.Tables[0].Columns.Add(new DataColumn("JobCardCode", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("RegNo", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("ProdTitle", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("JobCardDate", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("PromiseDateTime", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("AdvisorName", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("WorkinProgress", typeof(string)));
            //ds.Tables[0].Columns.Add(new DataColumn("FinalInspection", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("ReadyForDelivery", typeof(string)));
            ds.Tables[0].Columns.Add(new DataColumn("Delayed", typeof(string)));
            //ds.Tables[0].Columns.Add(new DataColumn("NewDate", typeof(string)));

            DataRow dr = ds.Tables[0].NewRow();
            ds.Tables[0].Rows.Add(dr);

            gvVehicleTrack.DataSource = ds.Tables[0];
            gvVehicleTrack.DataBind();

            Session["LaborDS"] = ds;
        }

        protected void Button1_OnClick(object sender, EventArgs e)
        {
            int sec = Convert.ToInt32(DropDownList1.SelectedValue);

            Timer1.Interval = sec;
            Timer1.Enabled = true;
        }

        protected void Button2_OnClick(object sender, EventArgs e)
        {
            
            Timer1.Enabled = false;
        }

        protected void OnClick_btnCheck(object sender, EventArgs e)
        {
            string FDate = sysfun.SaveDate(txtFromDate.Text).ToString();
            string TDate = sysfun.SaveDate(txtToDate.Text).ToString();


            String sql = "exec sp_VTB_VehicleTrackingBoard'" + Session["DealerCode"].ToString() + "' , '" + FDate + "', '" + TDate + "'";

            SysFunction sysFunc = new SysFunction();


            DataTable dt = new DataTable();

            dt = sysFunc.GetData(sql);          

            if (dt != null)
            {
                gvVehicleTrack.DataSource = dt;
                gvVehicleTrack.DataBind();
            }
            
        }
    }
}