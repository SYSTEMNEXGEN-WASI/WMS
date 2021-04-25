using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DXBMS.Modules.Setup
{
    public partial class CustomerTracking : System.Web.UI.Page
    {
        clsLookUp clslook = new clsLookUp();
        SysFunctions sysfuns = new SysFunctions();
        SysFunction sysfun = new SysFunction();
        DataTable dt = new DataTable();
        DataTable CusVehicleDT , JobCardDT,CounterSaleDT,PayRecDT,BalAmountDT;
        static string sQuery; 
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Page.IsPostBack)
            //{
            //    //if (Session["LookUpData"] != null)
            //    //{
            //    //    SelectedPartDetail(Session["LookUpData"].ToString());
            //    //}
            //}           

            if (!Page.IsPostBack)
            {
                CustomerVehicleGrid();
                JobCardGrid();
                CounterSaleGrid();
                PaymentReceiptGrid();
                BalancePayment();
            }
            
            //Session["LookUpData"] = string.Empty;
        }

        private void BalancePayment()
        {
            BalAmountDT = new DataTable();
            BalAmountDT.Columns.Add(new DataColumn("Customer", typeof(int)));
            BalAmountDT.Columns.Add(new DataColumn("InvoiceNo", typeof(int)));
            BalAmountDT.Columns.Add(new DataColumn("CustomerOustanding", typeof(string)));
            BalAmountDT.Columns.Add(new DataColumn("InsuranceOustanding", typeof(string)));
            BalAmountDT.Columns.Add(new DataColumn("SaleInvNo", typeof(string)));
            BalAmountDT.Columns.Add(new DataColumn("DirectSale", typeof(string)));

            gvBalanceAmount.DataSource = BalAmountDT; gvBalanceAmount.DataBind(); ViewState["BalAmt"] = CusVehicleDT;
        }

        private void CustomerVehicleGrid()
        {
            CusVehicleDT = new DataTable();
            CusVehicleDT.Columns.Add(new DataColumn("Customer", typeof(int)));
            CusVehicleDT.Columns.Add(new DataColumn("CellNo", typeof(string)));
            CusVehicleDT.Columns.Add(new DataColumn("Regno", typeof(string)));
            CusVehicleDT.Columns.Add(new DataColumn("ChassisNo", typeof(string)));
            CusVehicleDT.Columns.Add(new DataColumn("EngineNo", typeof(string)));
            CusVehicleDT.Columns.Add(new DataColumn("ProdCode", typeof(string)));

            gvCusVeh.DataSource = CusVehicleDT; gvCusVeh.DataBind(); ViewState["Cusveh"] = CusVehicleDT;
        }

        private void CounterSaleGrid()
        {
            CounterSaleDT = new DataTable();
            CounterSaleDT.Columns.Add(new DataColumn("SaleInvNo", typeof(int)));
            CounterSaleDT.Columns.Add(new DataColumn("CusDesc", typeof(string)));
            CounterSaleDT.Columns.Add(new DataColumn("TotReceipt", typeof(string)));
            CounterSaleDT.Columns.Add(new DataColumn("PaymentType", typeof(string)));

            gvCounterSale.DataSource = CounterSaleDT; gvCounterSale.DataBind(); ViewState["CounterSale"] = CounterSaleDT;
        }

        private void PaymentReceiptGrid()
        {
            PayRecDT = new DataTable();
            PayRecDT.Columns.Add(new DataColumn("ReceiptNo", typeof(int)));
            PayRecDT.Columns.Add(new DataColumn("InvoiceType", typeof(string)));
            PayRecDT.Columns.Add(new DataColumn("CusDesc", typeof(string)));
            PayRecDT.Columns.Add(new DataColumn("PayModeDesc", typeof(string)));
            PayRecDT.Columns.Add(new DataColumn("InvTotal", typeof(string)));

            gvPaymentReceipt.DataSource = PayRecDT; gvPaymentReceipt.DataBind(); ViewState["PaymentReceipt"] = PayRecDT;
        }
        private void JobCardGrid()
        {
            JobCardDT = new DataTable();
            JobCardDT.Columns.Add(new DataColumn("JobcardCode", typeof(int)));
            JobCardDT.Columns.Add(new DataColumn("JobTypeDesc", typeof(string)));
            JobCardDT.Columns.Add(new DataColumn("UserName", typeof(string)));
            JobCardDT.Columns.Add(new DataColumn("ChassisNo", typeof(string)));
            JobCardDT.Columns.Add(new DataColumn("EngineNo", typeof(string)));
            JobCardDT.Columns.Add(new DataColumn("JobTotal", typeof(string)));
            JobCardDT.Columns.Add(new DataColumn("ProdCode", typeof(string)));

            gvJobCard.DataSource = JobCardDT; gvJobCard.DataBind(); ViewState["JobCard"] = JobCardDT;
        }

        private void SelectedPartDetail(string item)
        {
            if(ViewState["lookupid"].ToString() == "1")
            {
                txtCustomer.Text = item;
            }
        }

        //protected void imgCustomer_Click(object sender, ImageClickEventArgs e)
        //{
        //    ViewState["lookupid"] = 1;
        //    clslook.LU_Get_Customer(imgCustomer, ViewState["lookupid"].ToString(), "", "../../../");
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        //}
        
        //protected void imgChasisNo_Click(object sender, ImageClickEventArgs e)
        //{
        //    ViewState["lookupid"] = 9;
        //    clslook.FQIinfo(imgChasisNo, ViewState["lookupid"].ToString(), "", "../../../");
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        //}

        protected void txtCellNo_TextChanged(object sender, EventArgs e)
        {
            
        }

        protected void txtCustomer_TextChanged(object sender, EventArgs e)
        {
            if (txtCustomer.Text == "" && txtCellNo.Text == "" && txtCNIC.Text == "" && txtNTN.Text == "" && txtChasisNo.Text == "" && txtEngineNo.Text == "" && txtRegno.Text == "" && txtProduct.Text == "")
            {
                sysfun.UserMsg(lblMsg, Color.Red, "Type Something For Search", txtCustomer);

                return;
            }


                SqlParameter[] param = {
                                            new SqlParameter("@DealerCode",SqlDbType.Char,5),    //0
                                            new SqlParameter("@CusCode",SqlDbType.VarChar,50),      //1
                                            new SqlParameter("@CellNo",SqlDbType.VarChar,15),    //2
                                            new SqlParameter("@NIC",SqlDbType.VarChar,15),       //3
                                            new SqlParameter("@ChassisNo",SqlDbType.VarChar,50), //4
                                            new SqlParameter("@EngineNo",SqlDbType.VarChar,50),  //5
                                            new SqlParameter("@Regno",SqlDbType.VarChar,20),     //6
                                            new SqlParameter("@Product",SqlDbType.VarChar,20)    //7
                                       };

            
            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = txtCustomer.Text.Trim();
            param[2].Value = txtCellNo.Text.Trim();
            param[3].Value = txtCNIC.Text.Trim();
            param[4].Value = txtChasisNo.Text.Trim();
            param[5].Value = txtEngineNo.Text.Trim();
            param[6].Value = txtRegno.Text.Trim();
            param[7].Value = txtProduct.Text.Trim();

            #region Comment
            //sQuery = "Select B.CusDesc Customer , B.CellNo ,A.ChassisNo , A.EngineNo , A.RegNo ,  V.ProdDesc ProdCode from Customer B " +
            //         "inner join CustomerVehicle A on A.CusCode = B.CusCode and A.DealerCode = B.DealerCode " +
            //         "inner join Vehicle V on A.DealerCode = V.DealerCode and A.ProdCode = V.ProdCode " +
            //         " where B.DealerCode = '" + Session["DealerCode"].ToString() + "' ";
            //if (txtCellNo.Text.Trim() != string.Empty)
            //{
            //    param[0].Value = txtCellNo.Text.Trim();
            //    //sQuery = sQuery + " and CellNo like '%" + txtCellNo.Text.Trim() + "%'";
            //}
            //else
            //{
            //    param[1].Value = "";
            //}

            //if (txtCustomer.Text.Trim() != string.Empty)
            //{
            //    //sQuery = sQuery + " and (B.CusDesc like  '%" + txtCustomer.Text.Trim() + "%' or B.CusCode like '%" + txtCustomer.Text.Trim() + "%' )";                
            //}

            //if (txtCNIC.Text.Trim() != string.Empty)
            //{
            //    //sQuery = sQuery + " and B.NIC like  '%" + txtCNIC.Text.Trim() + "%' ";
            //}

            //if (txtNTN.Text.Trim() != string.Empty)
            //{
            //    //sQuery = sQuery + " and B.NTNNo like  '%" + txtNTN.Text.Trim() + "%' ";
            //}

            //if (txtChasisNo.Text.Trim() != string.Empty)
            //{
            //    //sQuery = sQuery + " and A.ChassisNo like  '%" + txtChasisNo.Text.Trim() + "%' ";
            //}

            //if (txtEngineNo.Text.Trim() != string.Empty)
            //{
            //    //sQuery = sQuery + " and A.EngineNo like  '%" + txtEngineNo.Text.Trim() + "%' ";
            //}

            //if (txtRegno.Text.Trim() != string.Empty)
            //{
            //    //sQuery = sQuery + " and A.RegNo like  '%" + txtRegno.Text.Trim() + "%' ";
            //}

            //if (txtProduct.Text.Trim() != string.Empty)
            //{
            //    //sQuery = sQuery + " and (V.ProdDesc like  '%" + txtProduct.Text.Trim() + "%' or V.ProdCode like '%" + txtProduct.Text.Trim() + "%') ";
            //}
            #endregion

            DataSet ds = new DataSet();

            try
            {
                ds = sysfuns.FillDataSet("SP_CustomerVehicleSearch", param);
                gvCusVeh.DataSource = ds.Tables[0]; gvCusVeh.DataBind(); ViewState["Cusveh"] = ds.Tables[0];

                ds = sysfuns.FillDataSet("SP_JobCardSearch", param);
                gvJobCard.DataSource = ds.Tables[0]; gvJobCard.DataBind(); ViewState["JobCard"] = ds.Tables[0];

                if (txtChasisNo.Text == "" && txtEngineNo.Text == "" && txtRegno.Text == "" && txtProduct.Text == "")
                {
                    ds = sysfuns.FillDataSet("SP_CounterSaleSearch", param);
                    gvCounterSale.DataSource = ds.Tables[0]; gvCounterSale.DataBind(); ViewState["CounterSale"] = ds.Tables[0];

                    ds = sysfuns.FillDataSet("SP_PaymentReceiptSearch", param);
                    gvPaymentReceipt.DataSource = ds.Tables[0]; gvPaymentReceipt.DataBind(); ViewState["PaymentReceipt"] = ds.Tables[0];

                    ds = sysfuns.FillDataSet("SP_BalanceAmountSearch", param);
                    gvBalanceAmount.DataSource = ds.Tables[0]; gvBalanceAmount.DataBind(); ViewState["BalAmt"] = ds.Tables[0];

                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        return;
                    }

                    if (ds.Tables[0].Rows[0]["CustomerOustanding"].ToString() != "" && ds.Tables[0].Rows[0]["InsuranceOustanding"].ToString() != "" && ds.Tables[0].Rows[0]["DirectSale"].ToString() != "")
                    {
                        object sumCustOut, sumInsOut, sumDirectSale;
                        sumCustOut = ds.Tables[0].Compute("Sum(CustomerOustanding)", string.Empty);
                        sumInsOut = ds.Tables[0].Compute("Sum(InsuranceOustanding)", string.Empty);
                        sumDirectSale = ds.Tables[0].Compute("Sum(DirectSale)", string.Empty);

                        double sum = Convert.ToDouble(sumCustOut) + Convert.ToDouble(sumInsOut) + Convert.ToDouble(sumDirectSale);
                        lblCusOut.Text = sumCustOut.ToString();
                        lblInsOut.Text = sumInsOut.ToString();
                        lblDirectSales.Text = sumDirectSale.ToString();
                        lblTotalAmount.Text = sum.ToString();
                    }

                }

            }
            catch(Exception ex)
            {
                sysfun.UserMsg(lblMsg, Color.Red, ex.Message);
                return;
            }

            
            
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtCustomer.Text = "";
            txtCellNo.Text = "";
            txtCNIC.Text = "";
            txtNTN.Text = "";
            txtChasisNo.Text = "";
            txtEngineNo.Text = "";
            txtRegno.Text = "";
            txtProduct.Text = "";

            lblCusOut.Text = "";
            lblDirectSales.Text = "";
            lblInsOut.Text = "";
            lblTotalAmount.Text = "";
            lblMsg.Text = "";

            CustomerVehicleGrid();
            JobCardGrid();
            CounterSaleGrid();
            PaymentReceiptGrid();
            BalancePayment();
        }
    }
}