using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace DXBMS.Modules.Reports
{
    public partial class ViewReport : System.Web.UI.Page
    {
        SysFunctions myFunc = new SysFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
//            if (this.Session["UserName"] == null)
//            {
//                string close = @"<script type='text/javascript'>
//                                window.returnValue = true;
//                                window.close();
//                                </script>";
//                base.Response.Write(close);
//                return;


//            }

            string ReportName = Request.QueryString["ReportName"].ToString();
            if (ReportName == "GSTInvoice")
            {
                GSTInvoice();
            }
           

        }

        private void GSTInvoice()
        {
            //string SINo = Request.QueryString["SINo"].ToString();

            //Reports.SpareReports.XtraReportGSTInvoice myReport = new SpareReports.XtraReportGSTInvoice();

            //DataTable dt = new DataTable();

            ////string sql = "select 'UserID' as UserID,'Terminal' as Terminal, DD.DealerDesc , 
            ////DD.Address1 + ' ' + DD.Address2 + ' ' + DD.Address3 as DealerAddress,    CSM.SaleInvNo as SaleInvNo,    
            ////convert(varchar(12),CSM.SaleInvDate,103) as SaleInvDate,    CC.CusDesc as CustomerName,    
            ////CC.Phone1 + ' , ' + CC.Phone1  as Phones,    CC.SalesTaxRegNo as SaleTaxRegNo,    CC.NTNNo as NTNNo,    
            ////CC.CellNo as CellNo,    CC.Address1 + ' ' + CC.Address2 + ' ' + CC.Address3 as CustomerAddress,    
            ////CSM.Remarks  as Remarks,    II.PartItemNo as PartItemNo,    II.ItemDesc as ItemDesc,    
            ////CSD.SaleQty as SaleQty,    CSD.Rate as Rate,    CSM.PostFlag as PostFlag,    CSM.GSTAmount as GSTAmount,    
            ////CSM.Amount as GrossAmount,    DD.SaleTaxNo as SaleTaxNo,    DD.NTN as NTNNo,    CSM.OtherCharges as OtherCharges,    
            ////CSM.Discount as Discount,    CSM.InvoiceAmount as InvoiceAmount,  CSM.SalesTaxInvNO as SalesTaxInvNo,  
            ////CC.Address1 as AddressLine1,   CC.Address2 as AddressLine2 ,  CSM.GSTAmount  as GSTTotalAmount,  
            ////CSM.SalesTaxInvNo as SalesTaxInvNo  
            ////from CounterSaleMaster as CSM    
            ////    inner join CounterSaleDetail as CSD on CSD.SaleInvNo = CSM.SaleInvNo     
            ////    inner join Customer as CC on CC.CusCode = CSM.CusCode and CC.DealerCode = CSM.DealerCode   
            ////    --inner join CustomerInvoice as CI on CI.CusCode = CSM.CusCode and CI.DealerCode = CSM.DealerCode   
            ////    inner join dealer as DD on DD.DealerCode = CSM.DealerCode     
            ////    inner join Item as II on II.ItemCode = CSD.ItemCode  ";

            ////Data.dsGSTInvoice dsRpt = new Data.dsGSTInvoice();
            
            //DataSet dsRpt = new DataSet();
            //SqlParameter[] param = {
            //            new SqlParameter("@SaleInvNo",SqlDbType.Char,10),
            //            new SqlParameter("@dealerCode",SqlDbType.Char,10)  
            //        };
            //param[0].Value = SINo;
            //param[1].Value = Session["DealerCode"].ToString();

            //dsRpt = myFunc.SP_Spare_Get_rptGSTInvoiceCounterSale(param);

            //myReport.DataSource = dsRpt.Tables[0];
            //myReport.DataMember = "";
            //myReport.DataAdapter = null;

            
            //rptViewr.Report = myReport;
        }
    }
}