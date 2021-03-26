using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Web;

namespace DXBMS.Modules.Reports.SpareReports
{
    public partial class XtraReportGSTInvoice : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReportGSTInvoice()
        {
            InitializeComponent();
        }

        private void xrLabelUserID_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            xrLabelUserID.Text = HttpContext.Current.Session["UserName"].ToString();
        }

    }
}
