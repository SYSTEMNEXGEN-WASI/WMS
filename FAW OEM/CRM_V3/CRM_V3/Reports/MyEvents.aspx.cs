using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM_V3.Reports
{
    public partial class MyEvents : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet rptSource = (DataSet)System.Web.HttpContext.Current.Session["rptSource"];
            ReportDocument RD = new ReportDocument();

            RD.Load(Server.MapPath("~/Reports/EventDetail.rpt"));

            RD.Database.Tables[0].SetDataSource(rptSource);

            CrystalReportViewer1.ReportSource = RD;
        }
    }
}