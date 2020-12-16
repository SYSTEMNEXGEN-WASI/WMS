using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CConn;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.ApplicationBlocks.Data;
using PdfSharp.Pdf;

namespace DXBMS.Modules.Service.Forms
{
    public partial class GatePassTemp : System.Web.UI.Page
    {
        SysFunctions myFunc = new SysFunctions();
        SysFunction sysfun = new SysFunction();
        SecurityBll sec = new SecurityBll();
        Transaction ObjTrans = new Transaction();
        clsLookUp clslook = new clsLookUp();
        SqlTransaction Trans;
        DataTable Veh, Item;
        bool search_result, search_item;
        string Autostr;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2532", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }
            if (ViewState["Veh"] != null) Veh = (DataTable)ViewState["Veh"];
            if (ViewState["Item"] != null) Item = (DataTable)ViewState["Item"];

            if (Page.IsPostBack)
            {

                if (!string.IsNullOrEmpty(Session["LookUpData"].ToString()))
                {
                    SelectedPartDetail(Session["LookUpData"].ToString());
                }
            }
            if (!Page.IsPostBack)
            {
                setInitialDates();            //-------------------------------------Dates
                createVehicleDT();
            }
            Session["LookUpData"] = string.Empty;
        }
        private void setInitialDates()
        {
             txtGatePassDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtInDate.Text =txtOutDate.Text= DateTime.Now.ToString("dd-MM-yyyy");
            txtInTime.Text = txtOutTime.Text = DateTime.Now.ToString("HH:mm");
            txtInTime.Text = "00:00";
        }
        private void createVehicleDT()
        {
            Veh = new DataTable();
            Veh.Columns.Add(new DataColumn("ID", typeof(int)));
            Veh.Columns.Add(new DataColumn("RegNo", typeof(string)));
            Veh.Columns.Add(new DataColumn("ChassisNo", typeof(string)));
            Veh.Columns.Add(new DataColumn("EngineNo", typeof(string)));
            Veh.Columns.Add(new DataColumn("BrandCode", typeof(string)));
            Veh.Columns.Add(new DataColumn("BrandDesc", typeof(string)));
            Veh.Columns.Add(new DataColumn("ProdCode", typeof(string)));
            Veh.Columns.Add(new DataColumn("ProdDesc", typeof(string)));
            Veh.Columns.Add(new DataColumn("VersionCode", typeof(string)));
            Veh.Columns.Add(new DataColumn("VersionDesc", typeof(string)));
           
            //PartsDT.Rows.Add(new object[] { null, null, null, null, null, null, null, null, null,null });                        
            gvVehicle.DataSource = Veh; gvVehicle.DataBind(); ViewState["Veh"] = Veh;
        }
        protected void SelectedPartDetail(string item)
        {

            DataTable dt = new DataTable();
            try
            {
               
                 if (ViewState["lookupid"].ToString() == "3")
                {
                    txtRegNo.Text = item;
                    ddlVehRegNo_SelectedIndexChanged(txtRegNo, EventArgs.Empty);
                   


                }
               else if (ViewState["lookupid"].ToString() == "96")
                {
                    txtGatePassCode.Text = item;
                    GetGatePassDetail();



                }



            }
            catch (Exception ex)
            {
                sysfun.UserMsg(lblMessage, Color.Red, ex.Message);
            }
        }
        public void GetGatePassDetail()
        {
            DataSet dsGatePassInfo = new DataSet();
            SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",SqlDbType.VarChar,5),
                                    new SqlParameter("@GatePassCode",SqlDbType.Char,8)
                               };
            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = txtGatePassCode.Text;
            dsGatePassInfo = myFunc.FillDataSet("sp_W2_GatePass_Select", param);
            if (dsGatePassInfo.Tables[0].Rows.Count > 0)
            {
                setGatePassInfo(dsGatePassInfo);
            }
        
        DataSet dsVeh = new DataSet();
        dsVeh = myFunc.FillDataSet("sp_W2_GatePass_VehicleDetail_Select", param);
            //if (dsJobCardParts.Tables[0].Rows.Count == 0) dsJobCardParts.Tables[0].Rows.Add(dsJobCardParts.Tables[0].NewRow());
            ViewState["Veh"] = dsVeh.Tables[0]; gvVehicle.DataSource = dsVeh.Tables[0]; gvVehicle.DataBind();


        }

        protected void ddlVehRegNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet dsVehInfo = new DataSet();
            DataTable dt;
            SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",SqlDbType.VarChar,5),
                                    new SqlParameter("@RegNo",SqlDbType.VarChar,30)
                               };


            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = txtRegNo.Text.Trim(); // ddlVehRegNo.SelectedValue.ToString().Trim();//(HFRegNo.Value == "" ? null : HFRegNo.Value);

            dsVehInfo = myFunc.FillDataSet("sp_W2_CustomerVehicle_Select", param);
            if (dsVehInfo.Tables[0].Rows.Count > 0)
            {
                setVehcileInfo(dsVehInfo);
            }
          
        }
        private void setGatePassInfo(DataSet dsJobCardMaster)
        {
            DataTable dt;
            //ddlRegNo.Text = dsJobCardMaster.Tables[0].Rows[0]["RegNo"].ToString();
            txtGatePassCode.Text = dsJobCardMaster.Tables[0].Rows[0]["GatePassCode"].ToString();
            txtGatePassDate.Text = Convert.ToDateTime(dsJobCardMaster.Tables[0].Rows[0]["GatePassDate"]).ToString("dd/MM/yyyy");
            txtGatePassCode.Text = dsJobCardMaster.Tables[0].Rows[0]["GatePassCode"].ToString();
            txtRemarks.Text = dsJobCardMaster.Tables[0].Rows[0]["Remarks"].ToString();
            ddlModule.SelectedValue = dsJobCardMaster.Tables[0].Rows[0]["Module"].ToString();
            if (dsJobCardMaster.Tables[0].Rows[0]["Module"].ToString() == "Service")
            {
                txtTransCode.Text = dsJobCardMaster.Tables[0].Rows[0]["InvoiceNo"].ToString();
            }
            else
            {
                txtTransCode.Text = dsJobCardMaster.Tables[0].Rows[0]["TransCode"].ToString();
            }
            txtInDate.Text = Convert.ToDateTime(dsJobCardMaster.Tables[0].Rows[0]["InDate"]).ToString("dd/MM/yyyy");
            txtInTime.Text = Convert.ToDateTime(dsJobCardMaster.Tables[0].Rows[0]["InTime"]).ToString("HH:mm");
            txtOutDate.Text = Convert.ToDateTime(dsJobCardMaster.Tables[0].Rows[0]["OutDate"]).ToString("dd/MM/yyyy");
            txtOutTime.Text = Convert.ToDateTime(dsJobCardMaster.Tables[0].Rows[0]["OutTIme"]).ToString("HH:mm");
            if (ddlGType.SelectedValue == "N")
            {
                btnSave.Enabled = false;
            }   
         
        }
        private void setVehcileInfo(DataSet dsJobCardMaster)
        {
            DataTable dt;
            //ddlRegNo.Text = dsJobCardMaster.Tables[0].Rows[0]["RegNo"].ToString();
            txtChassisNo.Text = dsJobCardMaster.Tables[0].Rows[0]["ChassisNo"].ToString();
            txtEngineNo.Text = dsJobCardMaster.Tables[0].Rows[0]["EngineNo"].ToString();
            txtCusCode.Value = dsJobCardMaster.Tables[0].Rows[0]["CusCode"].ToString();
            txtCusDesc.Text = dsJobCardMaster.Tables[0].Rows[0]["Customer"].ToString();
          
          
            txtUserPhNo.Text = dsJobCardMaster.Tables[0].Rows[0]["Phone"].ToString();
            txtBrandCode.Value = dsJobCardMaster.Tables[0].Rows[0]["BrandCode"].ToString();
            txtBrandDesc.Text = dsJobCardMaster.Tables[0].Rows[0]["Brand"].ToString();
            txtProdCode.Value = dsJobCardMaster.Tables[0].Rows[0]["ProdCode"].ToString();
            txtVersionCode.Value = dsJobCardMaster.Tables[0].Rows[0]["VersionCode"].ToString();
            txtProdDesc.Text = dsJobCardMaster.Tables[0].Rows[0]["Product"].ToString();
            txtVersionDesc.Text = dsJobCardMaster.Tables[0].Rows[0]["Product"].ToString();
           
        }

        protected void imgVehRegNo_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 3; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            string sql = "Select RegNo as'Reg No' , ChassisNo as 'Chassis No' , EngineNo as 'Engine No' , C.CusDesc as 'Customer' from CustomerVehicle A inner join Customer C on C.CusCode = A.CusCode and C.DealerCode = A.DealerCode";
            clslook.LU_Get_RecVeh(imgVehRegNo, ViewState["lookupid"].ToString(), "", sql, "../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void BtnClear1_Click(object sender, ImageClickEventArgs e)
        {
            ClearVehicleTextBoxes();
        }
        private void ClearGatePassTextBoxes()
        {
            txtGatePassCode.Text = "";
            txtRemarks.Text = "";
            txtRegNo.Text = "";
            txtChassisNo.Text = "";
            txtEngineNo.Text = "";
            txtCusCode.Value = "";
            txtCusDesc.Text = "";
            txtUserPhNo.Text = "";
            txtBrandCode.Value = "";
            txtBrandDesc.Text = "";
            txtProdCode.Value = "";
            txtVersionCode.Value = "";
            txtProdDesc.Text = "";
            txtVersionDesc.Text = "";
            setInitialDates();
            createVehicleDT();


        }

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            if (txtRegNo.Text == "")
            {

                sysfun.UserMsg(lblMessage, Color.Red, "Please Select Vehicle First", txtRegNo);
                return;
            }
            search_result = false;
            foreach (DataRow rowVehDT in Veh.Rows)
            {
                if (rowVehDT["RegNo"].ToString().Trim() == ""
                    | rowVehDT["RegNo"].ToString().Trim() == txtRegNo.Text)
                {
                    Insert_in_PartsDT(rowVehDT);
                    search_result = true;
                }
            }
            if (search_result == false)
            {
                DataRow rowPartsDT = Veh.NewRow(); Insert_in_PartsDT(rowPartsDT); Veh.Rows.Add(rowPartsDT);
            }
            ViewState["Veh"] = Veh; gvVehicle.DataSource = Veh; gvVehicle.DataBind();
            ClearVehicleTextBoxes();
        }
        private void Insert_in_PartsDT(DataRow rowVehDT)
        {
            //DataTable dt = myFunc.GetData("select ItemDesc from Item where DealerCode='" + Session["DealerCode"].ToString() + "' And  ItemCode='" + ddlPartCode.SelectedValue.ToString() + "'");
            rowVehDT["RegNo"] = txtRegNo.Text.Trim();
            rowVehDT["ChassisNo"] = txtChassisNo.Text;
            rowVehDT["EngineNo"] = txtEngineNo.Text;
            rowVehDT["BrandCode"] = txtBrandCode.Value;
            rowVehDT["BrandDesc"] = txtBrandDesc.Text;
            rowVehDT["ProdCode"] = txtProdCode.Value;
            rowVehDT["ProdDesc"] = txtProdDesc.Text;
            rowVehDT["VersionCode"] = txtVersionCode.Value;
            rowVehDT["VersionDesc"] = txtVersionDesc.Text;
          
        }
       
        protected void BtnRemove_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                Veh = (DataTable)ViewState["Veh"];
                ImageButton btn = sender as ImageButton;
                TableCell tc = btn.Parent as TableCell;
                GridViewRow gvr = tc.Parent as GridViewRow;

                //Load grid 
                gvVehicle.DataSource = Veh;
                Veh.AcceptChanges();
                gvVehicle.DataBind();



            }



            catch (Exception ex)
            {
                sysfun.UserMsg(lblMessage, Color.Red, ex.Message, txtRegNo);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlGType.SelectedValue == "0")
                {
                    sysfun.UserMsg(lblMessage, Color.Red, "Please Select GatePass Type First!", txtRegNo);
                }
                if (ddlModule.SelectedValue == "0")
                {
                    sysfun.UserMsg(lblMessage, Color.Red, "Please Select Trans Type First!", txtRegNo);
                }
                if (txtGatePassCode.Text == "")
                {
                    Autostr = myFunc.AutoGen("GatePassTemp", "GatePassCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
                }
                else
                {
                    Autostr = txtGatePassCode.Text;
                }

                SqlParameter[] GatePass_param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5), 
           
           /*1*/ new SqlParameter("@GatePassCode",SqlDbType.Char,8),
           /*2*/ new SqlParameter("@GatePassDate",SqlDbType.DateTime),     
           /*4*/ new SqlParameter("@TransCode",SqlDbType.VarChar,8),       
           /*3*/ new SqlParameter("@GatePassType",SqlDbType.Char,1),         
           /*4*/ new SqlParameter("@Remarks",SqlDbType.Char,100),          
            /*6*/ new SqlParameter("@Module",SqlDbType.VarChar,50),            
           /*7*/ new SqlParameter("@UpdUser",SqlDbType.Char,50),
            /*8*/ new SqlParameter("@UpdTerm",SqlDbType.Char,50),
           /*9*/ new SqlParameter("@DelFlag",SqlDbType.Char,1),
           /*10*/ new SqlParameter("@InvoiceNo",SqlDbType.VarChar,8),
             /*11*/ new SqlParameter("@InDate",SqlDbType.DateTime),
               /*12*/ new SqlParameter("@InTime",SqlDbType.DateTime),
                 /*13*/ new SqlParameter("@OutDate",SqlDbType.DateTime),
               /*14*/ new SqlParameter("@OutTime",SqlDbType.DateTime),

            };

                GatePass_param[0].Value = Session["DealerCode"].ToString();
                GatePass_param[1].Value = Autostr;
                GatePass_param[2].Value = sysfun.SaveDate(txtGatePassDate.Text);
                GatePass_param[3].Value = "";
                GatePass_param[4].Value = ddlGType.SelectedValue;
                GatePass_param[5].Value = txtRemarks.Text;
                GatePass_param[6].Value = ddlModule.SelectedValue;
                GatePass_param[7].Value = Session["UserName"].ToString();
                GatePass_param[8].Value = GlobalVar.mUserIPAddress;
                GatePass_param[9].Value = "N";
                GatePass_param[10].Value = "";
                GatePass_param[11].Value = sysfun.SaveDate(txtInDate.Text);
                GatePass_param[12].Value = Convert.ToDateTime(txtInTime.Text).ToString("HH:mm");
                GatePass_param[13].Value = sysfun.SaveDate(txtOutDate.Text);
                GatePass_param[14].Value = Convert.ToDateTime(txtOutTime.Text).ToString("HH:mm"); ;

                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    if (txtGatePassCode.Text == "")
                    {
                        if (myFunc.ExecuteSP_NonQuery("[Sp_Insert_GatePassTemp_GPForm]", GatePass_param, Trans))
                        {
                            bool a = (rowsInVehicle(gvVehicle) == true ? Inser_JobCardDetail() : false);
                        }
                    }
                    else
                    {
                        if (myFunc.ExecuteSP_NonQuery("[Sp_Insert_GatePassTemp_GPForm]", GatePass_param, Trans))
                        {
                            bool a = (rowsInVehicle(gvVehicle) == true ? Inser_JobCardDetail() : false);
                        }
                    }
                }
                if (ObjTrans.CommittTransaction(ref Trans) == true)
                {
                    lblMessage.ForeColor = Color.Green;
                    if (txtGatePassCode.Text.Trim() == "") //ddlJobCardCode.SelectedIndex
                    {
                        lblMessage.Text = "Record Saved Successfully: " + Autostr;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);
                    }
                    else
                    {
                        lblMessage.Text = "Record Updated Successfully: " + Autostr;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Updatealert()", true);

                    }

                }
                else
                {
                    ObjTrans.RollBackTransaction(ref Trans);
                    sysfun.UserMsg(lblMessage, Color.Red, "Record not saved Try again. Or contact to support team ");
                }
                ClearGatePassTextBoxes();
            }
            catch (Exception ex)
            {
                sysfun.UserMsg(lblMessage, Color.Red, ex.Message, txtGatePassCode);
            }
        }
        private bool rowsInVehicle(GridView gvVehicle)
        {
            search_item = false;
            if (gvVehicle.Rows.Count > 0)
            {
                search_item = true;
            }
            //foreach (GridViewRow row in gvJobCard.Rows) { if (row.Cells[2].Text.Replace("&nbsp;", "").Trim() != "") search_item = true; }
            return search_item;
        }
        private bool Inser_JobCardDetail()
        {
            try
            {
                bool flag = false;
                //if (gvJobCard.Rows.Count > 0)
                //{
                SqlParameter[] GPVehicle_Delete_param = {
                                                                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                new SqlParameter("@GatePassCode",SqlDbType.Char,8)
                                                                //new SqlParameter("@JobCode",SqlDbType.VarChar,8)
                                                            };
                GPVehicle_Delete_param[0].Value = Session["DealerCode"].ToString();
                //JobCardDetail_Delete_param[1].Value = ddlJobCardCode.SelectedIndex == 0 ? HFJobCard.Value.Trim() : ddlJobCardCode.SelectedValue.ToString().Trim();
                GPVehicle_Delete_param[1].Value = txtGatePassCode.Text;
                SqlParameter[] GatePassVehDetail_Insert_param = {
                                                                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                new SqlParameter("@GatePassCode",SqlDbType.Char,8),
                                                                new SqlParameter("@ChassisNo",SqlDbType.VarChar,30),
                                                               
                                                            };


                if (myFunc.ExecuteSP_NonQuery("sp_W2_GatePassVehicle_Detail_Delete", GPVehicle_Delete_param, Trans))
                {
                    Veh = (DataTable)ViewState["Veh"];
                    DataRow[] drr = Veh.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {
                        if (Veh.Rows[i]["RegNo"].ToString() != ""
                            & Veh.Rows[i]["ChassisNo"].ToString() != ""
                            & Veh.Rows[i]["EngineNo"].ToString() != "")
                        {
                            GatePassVehDetail_Insert_param[0].Value = Session["DealerCode"].ToString();
                            GatePassVehDetail_Insert_param[1].Value = Autostr; //ddlJobCardCode.SelectedValue.ToString().Trim()
                            GatePassVehDetail_Insert_param[2].Value = Veh.Rows[i]["ChassisNo"].ToString();
                           
                            myFunc.ExecuteSP_NonQuery("sp_W2_GatePassVeh_Detail_Insert", GatePassVehDetail_Insert_param, Trans);
                            flag = true;
                        }
                    }
                    // }
                }
                return flag;
            }
            catch (Exception ex) { throw ex; }
        }

        protected void ImgGatePass_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["lookupid"] = 96; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_GatePass(ImgGatePass, ViewState["lookupid"].ToString(), "", "../../../");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            ReportDocument rpt, crReportDocument, RD;
            PdfDocument outputDocument = new PdfDocument();
            string strCriteria, rptTitle;
            DateTime FromDate, ToDate;
            SqlDataReader rder;
            SysFunctions myFunc = new SysFunctions();
            Data.DSReports DSReports = new Data.DSReports();


            RD = new ReportDocument();
            string CCon = CConnection.GetConnectionString();
            // PartItemLedger.sp_ItemStockLedger_select.Load(ds.CreateDataReader());
            DataSet ds = new DataSet();

            //if (txtTransCode.Text.Trim() != "")
            //{
            //    ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_GatePassMaster_Print '" + this.Session["DealerCode"].ToString() + "','" + txtGatePassCode.Text + "'");
            //}
            //else
            //{
            //    ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "sp_W2_GatePass_VehicleDetail_Select '" + this.Session["DealerCode"].ToString() + "','" + txtGatePassCode.Text + "'");
            //}


            // QRCodeEncoder encoder = new QRCodeEncoder();

            // Bitmap bi = encoder.Encode(ds.Tables[0].Rows[0]["DealerCode"].ToString() + "" + ds.Tables[0].Rows[0]["JobCardCode"].ToString());

            // bi.SetPixel(10, 10, Color.Red);

            //  bi.Save(Server.MapPath("~/Images/QrCode.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
            ds = SqlHelper.ExecuteDataset(CCon, CommandType.Text, "_sp_GatePassTempForm '" + this.Session["DealerCode"].ToString() + "','" + txtGatePassCode.Text + "'");
            DSReports.EnforceConstraints = false;
            DSReports._sp_GatePassTempForm.Load(ds.CreateDataReader());




            string FileRptPath = Server.MapPath("~/Modules/Service/ServiceReports/");


            RD.Load(Server.MapPath("~/Modules/Service/ServiceReports/rptGatePassForm.rpt"));



            RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Gate Pass'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";

            RD.DataDefinition.FormulaFields["QRCode"].Text = "'" + Server.MapPath("~") + "/Images/QrCode.jpg'";

            //DataTable dt = new DataTable();

            //string sql = "Select Logo from Dealer where DealerCode = '" + Session["DealerCode"].ToString() + "'";

            //dt = myFunc.GetData(sql);

            //RD.DataDefinition.FormulaFields["Logo"].Text = "'"+dt.Rows[0]["Logo"].ToString()+"'";
            //RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";


            // CrystalReportViewer1.ReportSource = rpt;
            RD.SetDataSource(DSReports.sp_JobCardMaster_Print.DataSet);
            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "Report.pdf";
            string File = FilePath + FileName;


            Stream stream = RD.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            DirectoryInfo info = new DirectoryInfo(FilePath);
            if (!info.Exists)
            {
                info.Create();
            }

            string path = Path.Combine(FilePath, FileName);
            using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
            {
                stream.CopyTo(outputFileStream);
            }
            stream.Dispose(); stream.Close();
            string URL = "../../../Download/PrintReport.aspx";

            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";

            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearGatePassTextBoxes();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);
        }

        private void ClearVehicleTextBoxes()
        {
            txtChassisNo.Text ="";
            txtEngineNo.Text = "";
            txtCusCode.Value = "";
            txtCusDesc.Text = "";
            txtRegNo.Text = "";

            txtUserPhNo.Text = "";
            txtBrandCode.Value = "";
            txtBrandDesc.Text = "";
            txtProdCode.Value = "";
            txtVersionCode.Value = "";
            txtProdDesc.Text = "";
            txtVersionDesc.Text = "";



        }
    }
}