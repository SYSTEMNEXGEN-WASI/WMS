using CConn;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DXBMS.Modules.Setup
{
    public partial class AccCodeAsgmt : System.Web.UI.Page
    {
        clsLookUp clslook = new clsLookUp();
        MainBLL ObjMainBLL = new MainBLL();
        Transaction ObjTrans = new Transaction();
        SysFunction SysFunc = new SysFunction();
        SysFunctions SysFuncs = new SysFunctions();
        DataSet ds = new DataSet();
        static int id = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }           

            if (!IsPostBack)
            {
                //Expense
                initializeDDLs(ddlExPartCostMkt);
                initializeDDLs(ddlExPartCostCKD);
                initializeDDLs(ddlExPartCostImp);
                initializeDDLs(ddlExPartCostLcl);
                initializeDDLs(ddlExLubCostMkt);
                initializeDDLs(ddlExLubCostLcl);
                initializeDDLs(ddlExLubCostCKD);
                initializeDDLs(ddlExLubCostImport);
                initializeDDLs(ddlExCashDis);
                initializeDDLs(ddlExChargeOut);
                initializeDDLs(ddlExBadDate);
                initializeDDLs(ddlExSvcChar);
                initializeDDLs(ddlExFreightChar);
                initializeDDLs(ddlExOtherChar);
                //Sale
                initializeDDLs(ddlInPartSaleMkt);
                initializeDDLs(ddlInPartSaleCKD);
                initializeDDLs(ddlInPartSaleLcl);
                initializeDDLs(ddlInPartSaleImp);
                initializeDDLs(ddlInLubSaleMkt);
                initializeDDLs(ddlInLubSaleLcl);
                initializeDDLs(ddlInLubSaleCKD);
                initializeDDLs(ddlInLubSaleImport);
                initializeDDLs(ddlInSubIncome);
                initializeDDLs(ddlInLabIncome);
                initializeDDLs(ddlInWarantyLab);
                initializeDDLs(ddlInFSB);
                //Assets
                initializeDDLs(ddlAssPartStkLcl);
                initializeDDLs(ddlAssPartStkMkt);
                initializeDDLs(ddlAssPartStkCKD);
                initializeDDLs(ddlAssPartStkImp);
                initializeDDLs(ddlAssLubStkMkt);
                initializeDDLs(ddlAssLubStkLcl);
                initializeDDLs(ddlAssLubStkCKD);
                initializeDDLs(ddlAssLubStkImport);
                initializeDDLs(ddlAssAPTV);
                initializeDDLs(ddlWarrantyWAP);
                initializeDDLs(ddlWarrantyNAP);
                // Liabalities
                initializeDDLs(ddlLiabPST);
                initializeDDLs(ddlLiabGST);
                initializeDDLs(ddlLiabExtra);
                initializeDDLs(ddlLiabFur);
                initializeDDLs(ddlLiabWithHold);
                initializeDDLs(ddlLiaOEMAcc);
                initializeDDLs(ddlLiaAPR);
                initializeDDLs(ddlLiaIA);
                LoadDDLs();
            }

            //

            Session["LookUpData"] = "";
        }

        private void LoadDDLs()
        {
            DataTable dt = SysFunc.GetData("Select * from AccountCodeSetup where DealerCode = '" + Session["DealerCode"].ToString() + "'");
            if(dt.Rows.Count>0)
            { 
            ddlExPartCostMkt.SelectedValue = dt.Rows[0]["PartsCostofSales(Market)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["PartsCostofSales(Market)"].ToString().Trim();
            ddlExPartCostCKD.SelectedValue = dt.Rows[0]["PartsCostofSales(CKD)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["PartsCostofSales(CKD)"].ToString().Trim();
            ddlExPartCostLcl.SelectedValue = dt.Rows[0]["PartsCostofSales(Local)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["PartsCostofSales(Local)"].ToString().Trim();
            ddlExPartCostImp.SelectedValue = dt.Rows[0]["PartsCostofSales(Imported)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["PartsCostofSales(Imported)"].ToString().Trim();
            ddlExLubCostMkt.SelectedValue = dt.Rows[0]["LubricantCostofSales(Market)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["LubricantCostofSales(Market)"].ToString().Trim();
            ddlExLubCostLcl.SelectedValue = dt.Rows[0]["LubricantCostofSales(Local)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["LubricantCostofSales(Local)"].ToString().Trim();
            ddlExLubCostCKD.SelectedValue = dt.Rows[0]["LubricantCostofSales(CKD)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["LubricantCostofSales(CKD)"].ToString().Trim();
            ddlExLubCostImport.SelectedValue = dt.Rows[0]["LubricantCostofSales(Import)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["LubricantCostofSales(Import)"].ToString().Trim();
            ddlExCashDis.SelectedValue = dt.Rows[0]["CashDiscountTaken"].ToString().Trim() == "" ? "0" : dt.Rows[0]["CashDiscountTaken"].ToString().Trim();
            ddlExChargeOut.SelectedValue = dt.Rows[0]["ChargeOutAccount"].ToString().Trim() == "" ? "0" : dt.Rows[0]["ChargeOutAccount"].ToString().Trim();
            ddlExBadDate.SelectedValue = dt.Rows[0]["baddebts"].ToString().Trim() == "" ? "0" : dt.Rows[0]["baddebts"].ToString().Trim();
            ddlExSvcChar.SelectedValue = dt.Rows[0]["ServiceChargesAccount"].ToString().Trim() == "" ? "0" : dt.Rows[0]["ServiceChargesAccount"].ToString().Trim();
            ddlExOtherChar.SelectedValue = dt.Rows[0]["OtherChargesAccount"].ToString().Trim() == "" ? "0" : dt.Rows[0]["OtherChargesAccount"].ToString().Trim();
            ddlExFreightChar.SelectedValue = dt.Rows[0]["FreightChargesAccount"].ToString().Trim() == "" ? "0" : dt.Rows[0]["FreightChargesAccount"].ToString().Trim();

            ddlInPartSaleMkt.SelectedValue = dt.Rows[0]["PartsSaleIncome(Market)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["PartsSaleIncome(Market)"].ToString().Trim();
            ddlInPartSaleCKD.SelectedValue = dt.Rows[0]["PartsSaleIncome(CKD)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["PartsSaleIncome(CKD)"].ToString().Trim();
            ddlInPartSaleLcl.SelectedValue = dt.Rows[0]["PartsSaleIncome(Local)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["PartsSaleIncome(Local)"].ToString().Trim();
            ddlInPartSaleImp.SelectedValue = dt.Rows[0]["PartsSaleIncome(Imported)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["PartsSaleIncome(Imported)"].ToString().Trim();
            ddlInLubSaleMkt.SelectedValue = dt.Rows[0]["LubricnatSaleIncome(Market)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["LubricnatSaleIncome(Market)"].ToString().Trim();
            ddlInLubSaleLcl.SelectedValue = dt.Rows[0]["LubricnatSaleIncome(Local)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["LubricnatSaleIncome(Local)"].ToString().Trim();
            ddlInLubSaleCKD.SelectedValue = dt.Rows[0]["LubricnatSaleIncome(CKD)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["LubricnatSaleIncome(CKD)"].ToString().Trim();
            ddlInLubSaleImport.SelectedValue = dt.Rows[0]["LubricnatSaleIncome(Import)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["LubricnatSaleIncome(Import)"].ToString().Trim();
            ddlInSubIncome.SelectedValue = dt.Rows[0]["SubletIncome"].ToString().Trim() == "" ? "0" : dt.Rows[0]["SubletIncome"].ToString().Trim();
            ddlInLabIncome.SelectedValue = dt.Rows[0]["LabourIncome"].ToString().Trim() == "" ? "0" : dt.Rows[0]["LabourIncome"].ToString().Trim();
            ddlInWarantyLab.SelectedValue = dt.Rows[0]["WarrantyLabour"].ToString().Trim() == "" ? "0" : dt.Rows[0]["WarrantyLabour"].ToString().Trim();
            ddlInFSB.SelectedValue = dt.Rows[0]["PDIFFIAccount"].ToString().Trim() == "" ? "0" : dt.Rows[0]["PDIFFIAccount"].ToString().Trim();

            ddlAssPartStkLcl.SelectedValue = dt.Rows[0]["PartsStock(Local)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["PartsStock(Local)"].ToString().Trim();
            ddlAssPartStkMkt.SelectedValue = dt.Rows[0]["PartsStock(Market)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["PartsStock(Market)"].ToString().Trim();
            ddlAssPartStkCKD.SelectedValue = dt.Rows[0]["PartsStock(CKD)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["PartsStock(CKD)"].ToString().Trim();
            ddlAssPartStkImp.SelectedValue = dt.Rows[0]["PartsStock(Imported)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["PartsStock(Imported)"].ToString().Trim();
            ddlAssLubStkMkt.SelectedValue = dt.Rows[0]["LubricantStock(Market)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["LubricantStock(Market)"].ToString().Trim();
            ddlAssLubStkLcl.SelectedValue = dt.Rows[0]["LubricantStock(Local)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["LubricantStock(Local)"].ToString().Trim();
            ddlAssLubStkCKD.SelectedValue = dt.Rows[0]["LubricantStock(CKD)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["LubricantStock(CKD)"].ToString().Trim();
            ddlAssLubStkImport.SelectedValue = dt.Rows[0]["LubricantStock(Import)"].ToString().Trim() == "" ? "0" : dt.Rows[0]["LubricantStock(Import)"].ToString().Trim();
            ddlAssAPTV.SelectedValue = dt.Rows[0]["AdvancePaymentToVendor"].ToString().Trim() == "" ? "0" : dt.Rows[0]["AdvancePaymentToVendor"].ToString().Trim();
            ddlWarrantyWAP.SelectedValue = dt.Rows[0]["WarrantyWap"].ToString().Trim() == "" ? "0" : dt.Rows[0]["WarrantyWap"].ToString().Trim();
            ddlWarrantyNAP.SelectedValue = dt.Rows[0]["WarrantyNap"].ToString().Trim() == "" ? "0" : dt.Rows[0]["WarrantyNap"].ToString().Trim();



                ///Liabilities
                ddlLiabPST.SelectedValue = dt.Rows[0]["PSTAccount"].ToString().Trim() == "" ? "0" : dt.Rows[0]["PSTAccount"].ToString().Trim();
            ddlLiabGST.SelectedValue = dt.Rows[0]["GSTAccount"].ToString().Trim() == "" ? "0" : dt.Rows[0]["GSTAccount"].ToString().Trim();
            ddlLiabExtra.SelectedValue = dt.Rows[0]["ExtraTax"].ToString().Trim() == "" ? "0" : dt.Rows[0]["ExtraTax"].ToString().Trim();
            ddlLiabFur.SelectedValue = dt.Rows[0]["FurtherAccount"].ToString().Trim() == "" ? "0" : dt.Rows[0]["FurtherAccount"].ToString().Trim();
            ddlLiabWithHold.SelectedValue = dt.Rows[0]["WHTax5"].ToString().Trim() == "" ? "0" : dt.Rows[0]["WHTax5"].ToString().Trim();
            ddlLiaOEMAcc.SelectedValue = dt.Rows[0]["OEMAccount"].ToString().Trim() == "" ? "0" : dt.Rows[0]["OEMAccount"].ToString().Trim();
            ddlLiaAPR.SelectedValue = dt.Rows[0]["AdvanceReceiptToCustomer"].ToString().Trim() == "" ? "0" : dt.Rows[0]["AdvanceReceiptToCustomer"].ToString().Trim();
                ddlLiaIA.SelectedValue = dt.Rows[0]["AdvanceReceiptToInsurance"].ToString().Trim() == "" ? "0" : dt.Rows[0]["AdvanceReceiptToInsurance"].ToString().Trim();

            }

        }

        private void initializeDDLs(DropDownList ddl)
        {
            try
            {
                string CCon = CConnection.GetConnStringForAccount();

                SqlDataReader dr = SqlHelper.ExecuteReader(CCon, CommandType.Text, "Select A.contacccode +'-'+  A.SubCode +'-'+  A.subsubcode +'-'+  A.loccode +'-'+  A.DetailCode as AccountCode , A.contacccode +'-'+   A.SubCode +'-'+ A.subsubcode +'-'+ A.loccode +'-'+  A.DetailCode + ' | ' + rtrim(A.DetailDesc) as DetailDesc from GDetail  A where CompCode = '"+Session["DealerCode"].ToString()+"'");

                if (dr.HasRows)
                {
                    ListItem item = new ListItem();
                    item.Text = "Select";
                    item.Value = "0";

                    //AddInAllDDL(item);

                    ddl.Items.Add(item);
                    while (dr.Read())
                    {
                        StringWriter myWriter = new StringWriter();
                        HttpUtility.HtmlDecode(dr["DetailDesc"].ToString().Replace(" ", "&nbsp;"), myWriter);//ddlEmp.Items.Add(myWriter.ToString());
                        item = new ListItem();
                        item.Text = myWriter.ToString();
                        item.Value = dr["AccountCode"].ToString();
                        ddl.Items.Add(item);

                    }
                    dr.Close();
                }
            }
            catch(Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }
            
        }

        private void AddInAllDDL(ListItem item)
        {
            ddlExPartCostMkt.Items.Add(item);
            ddlExPartCostCKD.Items.Add(item);
            ddlExPartCostLcl.Items.Add(item);
            ddlExLubCostMkt.Items.Add(item);
            ddlExLubCostLcl.Items.Add(item);
            ddlExLubCostCKD.Items.Add(item);
            ddlExLubCostImport.Items.Add(item);
            ddlExCashDis.Items.Add(item);
            ddlExChargeOut.Items.Add(item);
            ddlExBadDate.Items.Add(item);
            ddlExSvcChar.Items.Add(item);
            ddlExOtherChar.Items.Add(item);
            ddlExFreightChar.Items.Add(item);

            ddlInPartSaleMkt.Items.Add(item);
            ddlInPartSaleCKD.Items.Add(item);
            ddlInPartSaleLcl.Items.Add(item);
            ddlInLubSaleMkt.Items.Add(item);
            ddlInLubSaleLcl.Items.Add(item);
            ddlInLubSaleCKD.Items.Add(item);
            ddlInLubSaleImport.Items.Add(item);
            ddlInSubIncome.Items.Add(item);
            ddlInLabIncome.Items.Add(item);
            ddlInWarantyLab.Items.Add(item);

            ddlAssPartStkLcl.Items.Add(item);
            ddlAssPartStkMkt.Items.Add(item);
            ddlAssPartStkCKD.Items.Add(item);
            ddlAssLubStkMkt.Items.Add(item);
            ddlAssLubStkLcl.Items.Add(item);
            ddlAssLubStkCKD.Items.Add(item);
            ddlAssLubStkImport.Items.Add(item);

            ddlLiabPST.Items.Add(item);
            ddlLiabGST.Items.Add(item);
            ddlLiabExtra.Items.Add(item);
            ddlLiabFur.Items.Add(item);
            ddlLiabWithHold.Items.Add(item);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
                try
                {

                    SqlParameter[] param = {
                                        new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                        new SqlParameter("@PartsCostofSalesMkt",SqlDbType.Char),//1
                                        new SqlParameter("@PartsCostofSalesCKD",SqlDbType.Char),//2
                                        new SqlParameter("@PartsCostofSaleslcl",SqlDbType.Char),//3
                                        new SqlParameter("@LubricantCostofSalesMkt",SqlDbType.Char),//4
                                        new SqlParameter("@LubricantCostofSalesLcl",SqlDbType.Char),//5
                                        new SqlParameter("@LubricantCostofSalesCKD",SqlDbType.Char),//6
                                        new SqlParameter("@LubricantCostofSalesImport",SqlDbType.Char),//7                                   
                                        new SqlParameter("@PartsCostofSalesImp",SqlDbType.Char),//8
                                        new SqlParameter("@baddebts",SqlDbType.Char),//9
                                        new SqlParameter("@GSTAccount",SqlDbType.Char),//10
                                        new SqlParameter("@DiscountAccount",SqlDbType.Char),//11
                                        new SqlParameter("@OtherChargesAccount",SqlDbType.Char),//12
                                        new SqlParameter("@PartsSaleIncomeMkt",SqlDbType.Char),//13
                                        new SqlParameter("@PartsSaleIncomeCKD",SqlDbType.Char),//14                                  
                                        new SqlParameter("@PartsSaleIncomeLcl",SqlDbType.Char),//15
                                        new SqlParameter("@PartsSaleIncomeImp",SqlDbType.Char),//16
                                        new SqlParameter("@LubricnatSaleIncomeMkt",SqlDbType.Char),//17
                                        new SqlParameter("@LubricnatSaleIncomeLcl",SqlDbType.Char),//18
                                        new SqlParameter("@LubricnatSaleIncomeCKD",SqlDbType.Char),//19
                                        new SqlParameter("@LubricnatSaleIncomeImport",SqlDbType.Char),//20
                                        new SqlParameter("@SubletIncome",SqlDbType.Char),//21
                                        new SqlParameter("@LabourIncome",SqlDbType.Char),//22
                                        new SqlParameter("@WarrantyLabour",SqlDbType.Char),//23                                   
                                        new SqlParameter("@PartsStockLcl",SqlDbType.Char),//24
                                        new SqlParameter("@PartsStockMkt",SqlDbType.Char),//25
                                        new SqlParameter("@PartsStockCKD",SqlDbType.Char),//26
                                        new SqlParameter("@PartsStockImp",SqlDbType.Char),//27
                                        new SqlParameter("@LubricantStockMkt",SqlDbType.Char),//28
                                        new SqlParameter("@LubricantStockLcl",SqlDbType.Char),//29
                                        new SqlParameter("@LubricantStockCKD",SqlDbType.Char),//30
                                        new SqlParameter("@LubricantStockImport",SqlDbType.Char),//31 
                                        new SqlParameter("@PDIFFIAccount",SqlDbType.Char),//32                                   
                                        new SqlParameter("@WarrantyAccount",SqlDbType.Char),//33
                                        new SqlParameter("@OEMAccount",SqlDbType.Char),//34
                                        new SqlParameter("@WHTax5",SqlDbType.Char),//35
                                        new SqlParameter("@WHTax35",SqlDbType.Char),//36
                                        new SqlParameter("@OtherTax",SqlDbType.Char),//37                                    
                                        new SqlParameter("@ExpenseAccount",SqlDbType.Char),//38
                                        new SqlParameter("@CashDiscountTaken",SqlDbType.Char),//39
                                        new SqlParameter("@OEMVendorCode",SqlDbType.Char),//40
                                        new SqlParameter("@VODAccount",SqlDbType.Char),//41
                                        new SqlParameter("@SEDAccount",SqlDbType.Char),//42
                                        new SqlParameter("@ChargeOutAccount",SqlDbType.Char),//43                                   
                                        new SqlParameter("@HandlingAccount",SqlDbType.Char),//44
                                        new SqlParameter("@PSTAccount",SqlDbType.Char),//45
                                        new SqlParameter("@FurtherAccount",SqlDbType.Char),//46
                                        new SqlParameter("@FreightChargesAccount",SqlDbType.Char),//47
                                        new SqlParameter("@ServiceChargesAccount",SqlDbType.Char),//48
                                        new SqlParameter("@ExtraTax",SqlDbType.Char),//49
                                        new SqlParameter("@AdvancePaymentToVendor",SqlDbType.Char),//50
                                        new SqlParameter("@AdvanceReceiptToCustomer",SqlDbType.Char),//51
                                         new SqlParameter("@AdvanceReceiptToInsurance",SqlDbType.Char),//52
                                           new SqlParameter("@WarrantyWap",SqlDbType.Char),//53
                                             new SqlParameter("@WarrantyNap",SqlDbType.Char),//54
                                      };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlExPartCostMkt.SelectedValue;
                param[2].Value = ddlExPartCostCKD.SelectedValue;
                param[3].Value = ddlExPartCostLcl.SelectedValue;
                param[4].Value = ddlExLubCostMkt.SelectedValue;
                param[5].Value = ddlExLubCostLcl.SelectedValue;
                param[6].Value = ddlExLubCostCKD.SelectedValue;
                param[7].Value = ddlExLubCostImport.SelectedValue;
                param[8].Value = ddlExPartCostImp.SelectedValue;
                param[9].Value = ddlExBadDate.SelectedValue;
                param[10].Value = ddlLiabGST.SelectedValue;
                param[11].Value = ddlExCashDis.SelectedValue;
                param[12].Value = ddlExOtherChar.SelectedValue;
                param[13].Value = ddlInPartSaleMkt.SelectedValue;
                param[14].Value = ddlInPartSaleCKD.SelectedValue;
                param[15].Value = ddlInPartSaleLcl.SelectedValue;
                param[16].Value = ddlInPartSaleImp.SelectedValue;
                param[17].Value = ddlInLubSaleMkt.SelectedValue;
                param[18].Value = ddlInLubSaleLcl.SelectedValue;
                param[19].Value = ddlInLubSaleCKD.SelectedValue;
                param[20].Value = ddlInLubSaleImport.SelectedValue;
                param[21].Value = ddlInSubIncome.SelectedValue;
                param[22].Value = ddlInLabIncome.SelectedValue;
                param[23].Value = ddlInWarantyLab.SelectedValue;
                param[24].Value = ddlAssPartStkLcl.SelectedValue;
                param[25].Value = ddlAssPartStkMkt.SelectedValue;
                param[26].Value = ddlAssPartStkCKD.SelectedValue;
                param[27].Value = ddlAssPartStkImp.SelectedValue;
                param[28].Value = ddlAssLubStkMkt.SelectedValue;
                param[29].Value = ddlAssLubStkLcl.SelectedValue;
                param[30].Value = ddlAssLubStkCKD.SelectedValue;
                param[31].Value = ddlAssLubStkImport.SelectedValue;
                param[32].Value = ddlInFSB.SelectedValue;
                param[33].Value = "";
                param[34].Value = ddlLiaOEMAcc.SelectedValue;
                param[35].Value = ddlLiabWithHold.SelectedValue;
                param[36].Value = "";
                param[37].Value = "";
                param[38].Value = "";
                param[39].Value = ddlExCashDis.SelectedValue;
                param[40].Value = "";
                param[41].Value = "";
                param[42].Value = "";
                param[43].Value = ddlExChargeOut.SelectedValue;
                param[44].Value = "";
                param[45].Value = ddlLiabPST.SelectedValue;
                param[46].Value = ddlLiabFur.SelectedValue;
                param[47].Value = ddlExFreightChar.SelectedValue;
                param[48].Value = ddlExSvcChar.SelectedValue;
                param[49].Value = ddlLiabExtra.SelectedValue;
                param[50].Value = ddlAssAPTV.SelectedValue;
                param[51].Value = ddlLiaAPR.SelectedValue;
                param[52].Value = ddlLiaIA.SelectedValue;
                param[53].Value = ddlWarrantyWAP.SelectedValue;
                param[54].Value = ddlWarrantyNAP.SelectedValue;
                SysFunc.ExecuteSP_NonQuery("SP_Insert_AccountCodeSetup", param);
                lblMsg.Visible = true;
                lblMsg.Text = "Account(s) Saved Successfully";

            }
            catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = ex.Message;
                }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            LoadDDLs();
        }
    }
}