using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Threading;

namespace DXBMS.Modules.Service
{
    public partial class Schedule : System.Web.UI.Page
    {
        MainBLL objMBLL = new MainBLL();
        SysFunction SysFunc = new SysFunction();
        SysFunctions SysFuncs = new SysFunctions();
        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;
        DataSet ds = new DataSet();
        bool search_result, search_item;
        DataTable JobDT, LubDT, PartsDT;
        DataTable dsJobCardDetail, dsJobCardParts, dsJobCardLub;
        double totLabour, totParts, totlub;
        int countLabour, countParts, countlub, countlubRecQty, countPartsRecQty;
        string ScheduleCode;
        clsLookUp clslook = new clsLookUp();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }

            if (Page.IsPostBack)
            {

                if (!string.IsNullOrEmpty(Session["LookUpData"].ToString()))
                {
                    SelectedPartDetail(Session["LookUpData"].ToString());
                }
            }

            if (ViewState["Parts"] != null) PartsDT = (DataTable)ViewState["Parts"];
            if (ViewState["Job"] != null) JobDT = (DataTable)ViewState["Job"];
            if (ViewState["Lub"] != null) LubDT = (DataTable)ViewState["Lub"];

            TabName.Value = Request.Form[TabName.UniqueID];

            hfTab.Value = Request.Form[TabName.UniqueID];

            if(!IsPostBack){

                ThreadStart childthreat = new ThreadStart(childthreadcall);
                Thread child = new Thread(childthreat);

                child.Start();

                TabName.Value = Request.Form[TabName.UniqueID];

                createJobDT();               //-----------------------------------JobCardDetail
                createLubDT();              //------------------------------------PARTS            
                createPartsDT();           //-------------------------------------JOB

                string where = "DealerCode in('" + Session["DealerCode"].ToString() + "','COMON')";
                string[] columns1 = new string[] {"BrandCode","BrandDesc"};

                SysFunc.GetMultiColumnsDDL(ddlBrand,columns1,"Brand",where,"BrandCode","",false,false);

                string[] Columns = new string[] { "ItemCode", "PartItemNo", "ItemDesc", "Source" };
                //SysFunc.GetMultiColumnsDDL(ddlParts, Columns, "Item", "DealerCode ='" + Session["DealerCode"].ToString() + "' And LubeFlag = 'N' ", "ItemCode", "Order by ItemDesc", false, false);
                //Fill Lubricates Drop Down List
                SysFunc.GetMultiColumnsDDL(ddllLubs, Columns, "Item", "DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') And LubeFlag = 'Y' ", "ItemCode", "Order by ItemDesc", false, false);
                SysFuncs.ExecuteQuery("SELECT (DJ.DefJobDesc) ColName, DJ.DefJobCode ColValue FROM DefaultJob DJ LEFT OUTER JOIN JobCategory JC ON DJ.DealerCode = JC.DealerCode And DJ.JobCatCode = JC.JobCatCode Where DJ.DealerCode in( '" + Session["DealerCode"].ToString() + "','COMON')", ref ds);

                ddlJobs.DataSource = ds.Tables[0];
                ddlJobs.DataTextField = "ColName";
                ddlJobs.DataValueField = "ColValue";
                ddlJobs.DataBind();

                string[] Columns2 = new string[] { "ScheduleCode", "ProdCode", "BrandCode","KM" };
                SysFunc.GetMultiColumnsDDL(ddlSchedule, Columns2, "MaintainenceSchedule", "DealerCode ='" + Session["DealerCode"].ToString() + "'", "ScheduleCode", "", false, false);

                objMBLL.FillDropDown(ddlVehicleCode, "Select Distinct ProdCode, ProdDesc from Vehicle where DealerCode IN('" + Session["DealerCode"].ToString() + "', 'COMON') ", "ProdDesc", "ProdCode", "Select");
                ddlVehicleCode.Enabled = false;

                //string Where = " DealerCode ='" + Session["DealerCode"].ToString() + "' and DesigCode = '002' and leavingDate is null";
                //string[] Columns3 = new string[] { "EmpCode", "EmpName" };
                //SysFunc.GetMultiColumnsDDL(ddlTechnicianEmpCode, Columns3, "DealerEmp", Where , "EmpCode","",false, false);
                SysFunc.FillDropDown(ddlVersion, "SELECT Distinct  V.VersionCode,V.VersionDesc FROM Versions V inner join Vehicle A on A.VersionCode=V.VersionCode and A.DealerCode =V.DealerCode  Where V.DealerCode IN('" + Session["DealerCode"].ToString() + "', 'COMON')  ", "VersionDesc", "VersionCode", "Select");
                objMBLL.FillDropDown(ddlTechnicianEmpCode, "SELECT EmpCode,EmpName FROM DealerEmp Where DealerCode IN('" + Session["DealerCode"].ToString() + "', 'COMON') and DesigCode = '003' and leavingDate is null ", "EmpName", "EmpCode", "Select");


            }
            Session["LookUpData"] = string.Empty;
        }
        public void loadVersionddl()
        {
            SysFunc.FillDropDown(ddlVersion, "SELECT Distinct  V.VersionCode,V.VersionDesc FROM Versions V inner join Vehicle A on A.VersionCode=V.VersionCode and A.DealerCode =V.DealerCode  Where V.DealerCode IN('" + Session["DealerCode"].ToString() + "', 'COMON') and A.ProdCode='" + ddlVehicleCode.SelectedValue.Trim() + "' ", "VersionDesc", "VersionCode", "Select");
        }
        public void childthreadcall()
        {
            try
            {

                string sQuery = "SP_SelectParts '" + Session["DealerCode"].ToString() + "', 'P'";

                if (SysFuncs.ExecuteQuery(sQuery, ref ds))
                {
                    Session["Parts"] = ds;

                }

            }
            catch (ThreadAbortException e)
            {

                //lblmessage.Text += "<br /> child thread - exception";

            }
            finally
            {
                //lblmessage.Text += "<br /> child thread - unable to catch the  exception";
            }
        }

        protected void ddlRegNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBrand.SelectedIndex == 0)
            {
                return;
            }

            objMBLL.FillDropDown(ddlVehicleCode, "Select Distinct ProdCode, ProdDesc from Vehicle where BrandCode = '" + ddlBrand.SelectedValue + "' and DealerCode IN('" + Session["DealerCode"].ToString() + "', 'COMON') ", "ProdDesc", "ProdCode", "Select");
            ddlVehicleCode.Enabled = true;
            
        }

        private void createJobDT()
        {
            JobDT = new DataTable();
            JobDT.Columns.Add(new DataColumn("ID", typeof(int)));
            JobDT.Columns.Add(new DataColumn("JobCode", typeof(string)));
            JobDT.Columns.Add(new DataColumn("DefJobDesc", typeof(string)));
            JobDT.Columns.Add(new DataColumn("EmpCode", typeof(string)));
            JobDT.Columns.Add(new DataColumn("EmpName", typeof(string)));
            JobDT.Columns.Add(new DataColumn("Labor", typeof(string)));
            JobDT.Columns.Add(new DataColumn("Type", typeof(string)));
            gvJobCard.DataSource = JobDT; gvJobCard.DataBind(); ViewState["Job"] = JobDT;
        }

        private void createLubDT()
        {
            LubDT = new DataTable();
            LubDT.Columns.Add(new DataColumn("ID", typeof(int)));
            LubDT.Columns.Add(new DataColumn("PartNo", typeof(string)));
            LubDT.Columns.Add(new DataColumn("PartsDesc", typeof(string)));
            LubDT.Columns.Add(new DataColumn("Qty", typeof(string)));
            LubDT.Columns.Add(new DataColumn("Price", typeof(string)));
            LubDT.Columns.Add(new DataColumn("Total", typeof(string)));
            LubDT.Columns.Add(new DataColumn("ItemCode", typeof(string)));
            gvLubParts.DataSource = LubDT; gvLubParts.DataBind(); ViewState["Lub"] = LubDT;
        }

        private void createPartsDT()
        {
            PartsDT = new DataTable();
            PartsDT.Columns.Add(new DataColumn("ID", typeof(int)));
            PartsDT.Columns.Add(new DataColumn("PartNo", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("PartsDesc", typeof(string)));            
            PartsDT.Columns.Add(new DataColumn("Qty", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("Price", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("Total", typeof(string)));
            PartsDT.Columns.Add(new DataColumn("ItemCode", typeof(string)));
            gvJobCardParts.DataSource = PartsDT; gvJobCardParts.DataBind(); ViewState["Parts"] = PartsDT;
        }

        //protected void ddlParts_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlParts.SelectedItem.Text == "Select")
        //    {
        //        return;
        //    }
        //    DataSet ds = new DataSet();
        //    //  string strQuery = "Select " +
        //    //   "A.ItemCode, " +
        //    //   "A.ItemDesc, " +
        //    //   "A.UnitCode, " +
        //    //   "A.PartItemNo, " +
        //    //   "A.SaleRate, " +
        //    //   "IsNull(Sum(B.RecQty) + Sum(B.RetQty) - Sum(B.IssQty) - Sum(B.ChargeOutQty) - Sum(B.PurRetQty), 0) As [Quantity] " +
        //    //   "From    " +
        //    //   "Item A  " +
        //    //   "Left outer join ItemStock B    " +
        //    // "ON    " +
        //    // "A.ItemCode = B.ItemCode And    " +
        //    //"B.DealerCode = '" + Session["DealerCode"].ToString() + "' " +
        //    //"And A.ItemCode='" + ddlParts.SelectedValue.ToString().Trim() + "' " +
        //    //"Group by " +
        //    //"A.ItemCode, " +
        //    //   "A.ItemDesc, " +
        //    //   "A.UnitCode, " +
        //    //   "A.PartItemNo," +
        //    //   "A.SaleRate";
        //    string strQuery = "SELECT * FROM ITEM WHERE DealerCode='" + Session["DealerCode"] + "' And ItemCode= '" + ddlParts.SelectedValue.ToString().Trim() + "'";
        //    SysFuncs.ExecuteQuery(strQuery, ref ds);
        //    txtPartDesc.Text = ds.Tables[0].Rows[0]["ItemDesc"].ToString();
        //    txtPartCode.Text = ds.Tables[0].Rows[0]["PartItemNo"].ToString();
        //    txtPartPrice.Text = Convert.ToInt32(ds.Tables[0].Rows[0]["SaleRate"]).ToString();
        //    txtItemcodeParts.Text = ds.Tables[0].Rows[0]["ItemCode"].ToString();
        //    //txtPartsRecQuantity.Text = ds.Tables[0].Rows[0]["Quantity"].ToString();
        //}

        protected void BtnAdd0_Click(object sender, ImageClickEventArgs e)
        {

            if (txtQuantity.Text.Trim() == "" || txtQuantity.Text.Trim() == "0")
            {
                string script = "alert(\"Quantity should not left blank  !!!\");";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", script, true);
                return;
            }


            //lblMsg.Visible = false;
            TextBox[] textBoxes = { txtPartPrice, txtQuantity };
            if (!MasterValidation(textBoxes)) return;
            else
            {
                search_result = false;
                foreach (DataRow rowPartsDT in PartsDT.Rows)
                {
                    if (rowPartsDT["ItemCode"].ToString().Trim() == ""
                        | rowPartsDT["ItemCode"].ToString().Trim() == txtItemcodeParts.Text.Trim())
                    {
                        Insert_in_PartsDT(rowPartsDT); search_result = true;
                    }
                }
                if (search_result == false)
                {
                    DataRow rowPartsDT = PartsDT.NewRow(); Insert_in_PartsDT(rowPartsDT); PartsDT.Rows.Add(rowPartsDT);
                }
                ViewState["Parts"] = PartsDT; gvJobCardParts.DataSource = PartsDT; gvJobCardParts.DataBind();
                //int Count = 0;
                //foreach (DataRow dr in PartsDT.Rows)
                //{
                //    Count = Count + Convert.ToInt32(dr["Qty"]);
                //}
                //txtPartTotalQuantity.Text = Count.ToString();
                ClearPartsTextBoxes();
            }
        }

        private void ClearPartsTextBoxes()
        {
            TextBox[] txts = { txtQuantity, txtPartCode, txtPartDesc, txtPartPrice, txtItemcodeParts};
            ClearTextBoxes(txts); txtPartsRecQuantity.Text = "0";
            //ddlParts.SelectedIndex = 0;
        }

        public bool MasterValidation(TextBox[] textBoxes)
        {
            bool isValid = true;

            for (int i = 0; i < textBoxes.Length; i++)
            {
                if (textBoxes[i].Text.Trim() == "")
                {
                    isValid = false;
                    textBoxes[i].BorderColor = System.Drawing.Color.Red;
                    SetFocus(textBoxes[i]);
                    SysFunc.UserMsg(lblMsg, Color.Red, "Error: Field(s) Marked With Red Steriks '*' Are Mendetory...");
                    break;
                }
            }
            return isValid;
        }

        private void Insert_in_PartsDT(DataRow rowPartsDT)
        {
            rowPartsDT["PartNo"] = txtPartCode.Text.Trim(); rowPartsDT["PartsDesc"] = txtPartDesc.Text.Trim();            

            rowPartsDT["Qty"] = txtQuantity.Text.Trim(); rowPartsDT["Price"] = txtPartPrice.Text.Trim();
            rowPartsDT["Total"] = double.Parse(txtQuantity.Text.Trim()) * double.Parse(txtPartPrice.Text.Trim());
            rowPartsDT["ItemCode"] = txtItemcodeParts.Text.Trim();
        }

        public void ClearTextBoxes(TextBox[] textBoxes)
        {
            for (int i = 0; i < textBoxes.Length; i++)
            { textBoxes[i].Text = string.Empty; }
        }

        protected void BtnAdd_Click1(object sender, ImageClickEventArgs e)
        {
            TextBox[] textBoxes = { txtLabor };
            if (ddlJobs.SelectedItem.Text == "--Select--" && ddlTechnicianEmpCode.SelectedIndex == 0)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, "Please select Job first !!");
                return;
            }
            if (!MasterValidation(textBoxes)) { return; }
            else
            {
                search_result = false;
                foreach (DataRow rowJobDT in JobDT.Rows)
                {
                    if (rowJobDT["JobCode"].ToString().Trim() == ""
                        | rowJobDT["JobCode"].ToString().Trim() == ddlJobs.SelectedValue)
                    {
                        Insert_in_JobDT(rowJobDT); search_result = true;
                    }
                }
                if (search_result == false)
                {
                    DataRow rowJobDT = JobDT.NewRow(); Insert_in_JobDT(rowJobDT); JobDT.Rows.Add(rowJobDT);
                }
                ViewState["Job"] = JobDT; gvJobCard.DataSource = JobDT; gvJobCard.DataBind();

                //txtJobsTotal.Text = JobDT.Rows.Count.ToString();
                ClearJobTextBoxes();
            }
        }

        private void Insert_in_JobDT(DataRow rowJobDT)
        {
            rowJobDT["JobCode"] = ddlJobs.SelectedValue;
            rowJobDT["DefJobDesc"] = ddlJobs.SelectedItem.Text;
            rowJobDT["EmpCode"] = ddlTechnicianEmpCode.SelectedValue;
            rowJobDT["EmpName"] = ddlTechnicianEmpCode.SelectedItem; 
            rowJobDT["Labor"] = txtLabor.Text; 
        }

        private void ClearJobTextBoxes()
        {
            TextBox[] txts = { txtLabor };
            ClearTextBoxes(txts);
            ddlJobs.SelectedIndex = 0;
        }

        protected void BtnClear1_Click1(object sender, ImageClickEventArgs e)
        {
            ClearJobTextBoxes();
        }

        protected void gvJobCard_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblRowAmount = (Label)e.Row.Cells[4].FindControl("lblRowAmount");
                totLabour = totLabour + double.Parse(lblRowAmount.Text.Replace("&nbsp;", "") == "" ? "0" : lblRowAmount.Text);
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                countLabour = 0;
                totLabour = 0;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblFooterAmount = (Label)e.Row.FindControl("lblFooterAmount");
                lblFooterAmount.Text = totLabour.ToString();
                //lblJobCardTotal.Text = totLabour.ToString();
                //txtJobsTotal.Text = countLabour.ToString();
            }
        }

        protected void BtnRemove_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                JobDT = (DataTable)ViewState["Job"];
                ImageButton btn = sender as ImageButton;
                TableCell tc = btn.Parent as TableCell;
                GridViewRow gvr = tc.Parent as GridViewRow;
                JobDT.Rows.RemoveAt(gvr.RowIndex);
                //Load grid 
                gvJobCard.DataSource = JobDT;
                JobDT.AcceptChanges();
                gvJobCard.DataBind();
                //if (JobDT.Rows.Count == 0)
                //{
                //    txtJobsTotal.Text = "0";
                //    lblJobCardTotal.Text = "0";
                //}
                //else
                //{
                //    txtJobsTotal.Text = JobDT.Rows.Count.ToString();

                //}

            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, ex.Message, txtPartCode);
            }
        }

        protected void BtnClear2_Click(object sender, ImageClickEventArgs e)
        {
            ClearPartsTextBoxes();
        }

        protected void gvJobCardParts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblRowPartAmount = (Label)e.Row.Cells[7].FindControl("lblRowPartAmount");
                totParts = totParts + int.Parse(lblRowPartAmount.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblRowPartAmount.Text.Replace("&nbsp;", "").Trim());
                //totParts = totParts + double.Parse(e.Row.Cells[7].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[7].Text.Replace("&nbsp;", "").Trim());
                //countParts = countParts + int.Parse(e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[8].Text.Replace("&nbsp;", "").Trim());
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                totParts = 0;
                countPartsRecQty = countParts = 0;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblFooterPartAmount = (Label)e.Row.FindControl("lblFooterPartAmount");
                lblFooterPartAmount.Text = totParts.ToString();
            }
        }

        protected void BtnRemove_Click1(object sender, ImageClickEventArgs e)
        {
            try
            {
                PartsDT = (DataTable)ViewState["Parts"];
                ImageButton btn = sender as ImageButton;
                TableCell tc = btn.Parent as TableCell;
                GridViewRow gvr = tc.Parent as GridViewRow;
                PartsDT.Rows.RemoveAt(gvr.RowIndex);
                //Load grid 
                gvJobCardParts.DataSource = PartsDT;
                PartsDT.AcceptChanges();
                gvJobCardParts.DataBind();
                int sumParts = 0;               

            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, ex.Message, txtPartCode);
            }
        }

        protected void ddllLubs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddllLubs.SelectedItem.Text == "Select")
            {
                return;
            }
            DataSet ds = new DataSet();
            SysFuncs.ExecuteQuery("Select * From Item Where ItemCode='" + ddllLubs.SelectedValue.ToString().Trim() + "'", ref ds);
            txtLubPartDesc.Text = ds.Tables[0].Rows[0]["ItemDesc"].ToString();
            txtLubPartCode.Text = ds.Tables[0].Rows[0]["PartItemNo"].ToString();
            txtItemcodeLub.Text = ds.Tables[0].Rows[0]["ItemCode"].ToString();
            txtLubPrice.Text = Convert.ToInt32(ds.Tables[0].Rows[0]["SaleRate"]).ToString();
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {

            TextBox[] textBoxes = { txtLubPartCode, txtLubPrice, txtLubQuantity };
            if (!MasterValidation(textBoxes)) return;
            else
            {
                search_result = false;
                foreach (DataRow rowLubDT in LubDT.Rows)
                {
                    if (rowLubDT["ItemCode"].ToString().Trim() == ""
                        | rowLubDT["ItemCode"].ToString().Trim() == txtItemcodeLub.Text.Trim())
                    {
                        Insert_in_LubPartsDT(rowLubDT); search_result = true;
                    }
                }
                if (search_result == false)
                {
                    DataRow rowLubDT = LubDT.NewRow(); Insert_in_LubPartsDT(rowLubDT); LubDT.Rows.Add(rowLubDT);
                }
                ViewState["Lub"] = LubDT; gvLubParts.DataSource = LubDT; gvLubParts.DataBind();
                ClearLubricantsTextBoxes();
            }
        }

        private void Insert_in_LubPartsDT(DataRow rowLubDT)
        {
            rowLubDT["PartNo"] = txtLubPartCode.Text.Trim(); rowLubDT["PartsDesc"] = txtLubPartDesc.Text.Trim();
            rowLubDT["Qty"] = txtLubQuantity.Text.Trim(); rowLubDT["Price"] = txtLubPrice.Text.Trim();
            rowLubDT["Total"] = double.Parse(txtLubQuantity.Text.Trim()) * double.Parse(txtLubPrice.Text.Trim());
            rowLubDT["ItemCode"] = txtItemcodeLub.Text.Trim();
        }

        private void ClearLubricantsTextBoxes()
        {
            TextBox[] txts = { txtLubPartCode, txtLubPartDesc, txtLubPrice, txtLubQuantity, txtItemcodeLub };
            ClearTextBoxes(txts);
            txtLubRecQuantity.Text = "0";
            ddllLubs.SelectedIndex = 0;
        }

        protected void ImgBtnLubClear_Click(object sender, ImageClickEventArgs e)
        {
            ClearLubricantsTextBoxes();
        }

        protected void gvLubParts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //countlub = countlub + int.Parse(e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim() == "" ? "0" : e.Row.Cells[5].Text.Replace("&nbsp;", "").Trim());
                Label lblRowLubsAmount = (Label)e.Row.Cells[6].FindControl("lblRowLubsAmount");
                totlub = totlub + double.Parse(lblRowLubsAmount.Text.Replace("&nbsp;", "").Trim() == "" ? "0" : lblRowLubsAmount.Text.Replace("&nbsp;", "").Trim());
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                totlub = 0; countlubRecQty = countlub = 0;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblFooterLubsAmount = (Label)e.Row.FindControl("lblFooterLubsAmount");
                lblFooterLubsAmount.Text = totlub.ToString();
                //lbltotLubQty.Text = countlub.ToString(); txtLubTotal.Text = totlub.ToString();
                //txttotLubRecQty.Text = countlubRecQty.ToString();
            }
        }

        protected void BtnRemove_Click2(object sender, ImageClickEventArgs e)
        {
            try
            {
                LubDT = (DataTable)ViewState["Lub"];
                ImageButton btn = sender as ImageButton;
                TableCell tc = btn.Parent as TableCell;
                GridViewRow gvr = tc.Parent as GridViewRow;
                LubDT.Rows.RemoveAt(gvr.RowIndex);
                //Load grid 
                gvLubParts.DataSource = LubDT;
                LubDT.AcceptChanges();
                gvLubParts.DataBind();
                //if (LubDT.Rows.Count == 0)
                //{
                //    txtLubTotal.Text = "0";
                //    lbltotLubQty.Text = "0";
                //}
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, ex.Message, txtPartCode);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int GVJobCard = gvJobCard.Rows.Count;
            int GVParts = gvJobCardParts.Rows.Count;
            int GVLub = gvLubParts.Rows.Count;        

                if (GVJobCard > 0 | GVParts > 0 | GVLub > 0)
                {
                    CustomerEsTimate_Entry();
                    string[] Columns = new string[] { "ScheduleCode", "ProdCode", "BrandCode" };
                    SysFunc.GetMultiColumnsDDL(ddlSchedule, Columns, "MaintainenceSchedule", "DealerCode ='" + Session["DealerCode"].ToString() + "'", "ScheduleCode", "", false, false);
                    clearAll();
                    createJobDT();
                    createLubDT();
                    createPartsDT();

                }
                else
                {

                    SysFunc.UserMsg(lblMsg, Color.Red, "One of the detail must be selected !!!", txtQuantity);
                    return;
                }
            
        }

        private void CustomerEsTimate_Entry()
        {
            SqlParameter[] param = {                                            
           /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
           /*1*/ new SqlParameter("@ProdCode",SqlDbType.VarChar,20),         
           /*2*/ new SqlParameter("@BrandCode",SqlDbType.VarChar,3),        
           /*3*/ new SqlParameter("@ScheduleCode",SqlDbType.VarChar,8),
           /*4*/  new SqlParameter("@KM",SqlDbType.VarChar,10),
           /*5*/  new SqlParameter("@SDays",SqlDbType.VarChar,3),
           /*6*/  new SqlParameter("@ReminderLetter",SqlDbType.VarChar,6),
           /*7*/ new SqlParameter("@JobsTotal",SqlDbType.Float),
           /*8*/ new SqlParameter("@PartsTotal",SqlDbType.Float),
           /*9*/ new SqlParameter("@LubsTotal",SqlDbType.Float),       
           /*10*/ new SqlParameter("@UpdUser",SqlDbType.VarChar,50),
           /*11*/ new SqlParameter("@UpdDate",SqlDbType.DateTime),
           /*12*/ new SqlParameter("@UpdTime",SqlDbType.DateTime),
           /*13*/ new SqlParameter("@UpdTerm",SqlDbType.VarChar,50),
           /*14*/ new SqlParameter("@CallAlert",SqlDbType.VarChar,5),
           /*15*/ new SqlParameter("@SmsAlert",SqlDbType.VarChar,5),
           /*16*/ new SqlParameter("@VehicleCategory",SqlDbType.Char,3),
            /*16*/ new SqlParameter("@Version",SqlDbType.Char,3)
        };
            param[0].Value = Session["DealerCode"].ToString();
            //Auto Code Generation Decision on Insert 
            //for update CustomerEstimate

            Label lblFooterAmount = new Label();
            Label lblFooterPartAmount = new Label();
            Label lblFooterLubsAmount = new Label();
            if (gvJobCard.Rows.Count > 0) lblFooterAmount = (Label)gvJobCard.FooterRow.FindControl("lblFooterAmount"); else lblFooterAmount.Text = "0";

            if (gvJobCardParts.Rows.Count > 0) lblFooterPartAmount = (Label)gvJobCardParts.FooterRow.FindControl("lblFooterPartAmount"); else lblFooterPartAmount.Text = "0";
            if (gvLubParts.Rows.Count > 0) lblFooterLubsAmount = (Label)gvLubParts.FooterRow.FindControl("lblFooterLubsAmount"); else lblFooterLubsAmount.Text = "0";
            

            if(ddlSchedule.SelectedIndex == 0)
            {
                ScheduleCode = SysFuncs.AutoGen("MaintainenceSchedule", "ScheduleCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));
            }
            else
            {
                ScheduleCode = ddlSchedule.SelectedValue;
            }

                
                param[1].Value = ddlVehicleCode.SelectedValue.Trim();
                param[2].Value = ddlBrand.SelectedValue.Trim();
                param[3].Value = ScheduleCode;
                param[4].Value = txtScheduleKm.Text;
                param[5].Value = txtNextVisit.Text;
                param[6].Value = txtReminderLeter.Text;
                param[7].Value = lblFooterAmount.Text.Trim();
                param[8].Value = lblFooterPartAmount.Text.Trim();
                param[9].Value = lblFooterLubsAmount.Text.Trim();             
                param[10].Value = Session["UserName"].ToString();
                param[11].Value = DateTime.Now;
                param[12].Value = SysFunc.SaveTime(DateTime.Now.ToString("HH:mm"));
                param[13].Value = GlobalVar.mUserIPAddress;
                param[14].Value = txtCallAlert.Text;
                param[15].Value = txtSms.Text;
                param[16].Value = Session["VehicleCategory"].ToString();
            param[17].Value = ddlVersion.SelectedValue.Trim() ;


            try
            {
                if (ObjTrans.BeginTransaction(ref Trans) == true)
                {
                    //For New Schedule
                    if (txtSchedule.Text == "")
                    {
                        if (SysFunc.ExecuteSP_NonQuery("[sp_Schedule_Master_Insert]", param, Trans))
                        {
                            //bool a = (d1 > 0 ? Inser_JobCardDetail() : false);
                            //bool a = (rowsInJobCardDetail(gvJobCard) == true ? Inser_CustomerEstimateDetail() : false);
                            if (gvJobCard.Rows.Count > 0) Insert_ScheduleJobDetail();

                            //bool c = (d2 > 0 ? Inser_JobCardPartsDetail() : false);
                            //bool c = (rowInJobCardPartsDetail(gvJobCardParts) == true ? Inser_CustomerEstimePartsDetail() : false);
                            if (gvJobCardParts.Rows.Count > 0) Insert_SchedulePartsDetail();
                            //bool d = (d3 > 0 ? Inser_JobCardLubricanteDetail() : false);
                            //bool d = (rowInJobCardLubricanteDetail(gvLubParts) == true ? Inser_CustomerEstimateLubricates_Detail() : false);
                            if (gvLubParts.Rows.Count > 0) Insert_ScheduleLubricates_Detail();

                            //bool b = (d4 > 0 ? Inser_JobCardSubletDetail() : false);
                        }

                        if (ObjTrans.CommittTransaction(ref Trans) == true)
                        {

                            SysFunc.UserMsg(lblMsg, Color.Green, "Schedule '"+ScheduleCode+"'  Saved Successfully !!!");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);
                        }
                        else
                        {
                            ObjTrans.RollBackTransaction(ref Trans);
                            SysFunc.UserMsg(lblMsg, Color.Red, "Record not saved Try again." + txtSchedule.Text, txtSchedule);
                        }
                        clearAll();
                    }
                    else
                    {
                        //For Update Schedule
                        param[3].Value = txtSchedule.Text.Trim();
                        if (SysFunc.ExecuteSP_NonQuery("[sp_Update_Schedule_Master]", param, Trans))
                        {
                            Insert_ScheduleJobDetail();
                            Insert_SchedulePartsDetail();
                            Insert_ScheduleLubricates_Detail();
                            bool c;

                        }
                        else { SysFunc.UserMsg(lblMsg, Color.Red, "Update failed"); }


                        if (ObjTrans.CommittTransaction(ref Trans) == true)
                        {
                            clearAll();

                            SysFunc.UserMsg(lblMsg, Color.Green, "Schedule '" + ScheduleCode + "' Update Successfully: " + txtSchedule.Text, txtSchedule);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Updatealert()", true);
                        }
                        else
                        {
                            ObjTrans.RollBackTransaction(ref Trans);
                            SysFunc.UserMsg(lblMsg, Color.Red, "Record not saved Try again. " + txtSchedule.Text, txtSchedule);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message, txtSchedule);
                ObjTrans.RollBackTransaction(ref Trans);
            }


        }

        protected void ddlVehicleCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadVersionddl();
            ddlVersion.Enabled = true;

        }

        private bool Insert_ScheduleJobDetail()
        {
            try
            {
                bool flag = false;
                SqlParameter[] Inser_CustomerEstimate_Delete_param = {                                                       
                                                                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                new SqlParameter("@ScheduleCode",SqlDbType.Char,8)
                                                            };
                Inser_CustomerEstimate_Delete_param[0].Value = Session["DealerCode"].ToString();
                Inser_CustomerEstimate_Delete_param[1].Value = ScheduleCode.Trim();
                
                SqlParameter[] param = {
                                        /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                       /*1*/ new SqlParameter("@ProdCode",SqlDbType.VarChar,20),         
                                       /*2*/ new SqlParameter("@BrandCode",SqlDbType.VarChar,3),        
                                       /*3*/ new SqlParameter("@KM",SqlDbType.VarChar,10),                                       
                                       /*4*/ new SqlParameter("@SDays",SqlDbType.VarChar,3), 
                                       /*5*/ new SqlParameter("@JobCode",SqlDbType.VarChar,8),
                                       /*6*/ new SqlParameter("@ScheduleCode",SqlDbType.VarChar,8),
                                       /*7*/ new SqlParameter("@Labor",SqlDbType.Float),
                                             new SqlParameter("@DefJobDesc",SqlDbType.VarChar,50),
                                             new SqlParameter("@EmpCode",SqlDbType.Char,3),
                                             

                           };
                if (SysFunc.ExecuteSP_NonQuery("sp_W2_ScheduleJob_Detail_Delete", Inser_CustomerEstimate_Delete_param, Trans))
                {

                    JobDT = (DataTable)ViewState["Job"];
                    DataRow[] drr = JobDT.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {
                        if (JobDT.Rows[i]["JobCode"].ToString() != ""
                            & JobDT.Rows[i]["Labor"].ToString() != "")
                        {
                            param[0].Value = Session["DealerCode"].ToString();
                            param[1].Value = ddlVehicleCode.SelectedValue.Trim();
                            param[2].Value = ddlBrand.SelectedValue.Trim();
                            param[3].Value = txtScheduleKm.Text;
                            param[4].Value = txtNextVisit.Text;
                            param[5].Value = JobDT.Rows[i]["JobCode"].ToString();
                            param[6].Value = ScheduleCode;
                            param[7].Value = Convert.ToDecimal(JobDT.Rows[i]["Labor"].ToString());
                            param[8].Value = JobDT.Rows[i]["DefJobDesc"].ToString();
                            param[9].Value = JobDT.Rows[i]["EmpCode"].ToString();
                            SysFunc.ExecuteSP_NonQuery("sp_ScheduleJob_Detail_Insert", param, Trans);
                            flag = true;
                        }
                    }
                    // }
                }
                return flag;
            }
            catch (Exception ex) { throw ex; }
        }

        private bool Insert_SchedulePartsDetail()
        {
            try
            {
                bool flag = false;
                SqlParameter[] CustomerEstimePartsDetail_Delete_param = {                                                       
                                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    new SqlParameter("@ScheduleCode",SqlDbType.Char,8)
                                                                 };
                CustomerEstimePartsDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                CustomerEstimePartsDetail_Delete_param[1].Value = ScheduleCode.Trim();

                SqlParameter[] param = {                                                       
                                        /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                       /*1*/ new SqlParameter("@ProdCode",SqlDbType.VarChar,20),         
                                       /*2*/ new SqlParameter("@BrandCode",SqlDbType.VarChar,3),        
                                       /*3*/ new SqlParameter("@KM",SqlDbType.VarChar,10),                                       
                                       /*4*/ new SqlParameter("@SDays",SqlDbType.VarChar,3),
                                       /*5*/      new SqlParameter("@ItemCode",SqlDbType.Char,8),
                                       /*6*/      new SqlParameter("@PartNo",SqlDbType.VarChar,18),
                                       /*7*/      new SqlParameter("@Qty",SqlDbType.VarChar,10),
                                       /*8*/      new SqlParameter("@Price",SqlDbType.Float),
                                       /*9*/      new SqlParameter("@ScheduleCode",SqlDbType.VarChar,8),
                                             new SqlParameter("@PartDesc",SqlDbType.VarChar,50),
                                             new SqlParameter("@Total",SqlDbType.Float)
                                                                    
                                                                 };
                if (SysFunc.ExecuteSP_NonQuery("sp_2W_Schedule_PartsDetail_Delete", CustomerEstimePartsDetail_Delete_param, Trans))
                {
                    PartsDT = (DataTable)ViewState["Parts"];
                    DataRow[] drr = PartsDT.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {
                        param[0].Value = Session["DealerCode"].ToString();
                        param[1].Value = ddlVehicleCode.SelectedValue;
                        param[2].Value = ddlBrand.SelectedValue;
                        param[3].Value = txtScheduleKm.Text;
                        param[4].Value = txtNextVisit.Text;
                        param[9].Value = ScheduleCode.Trim();
                        if (PartsDT.Rows[i]["ItemCode"].ToString() != "")
                        {
                            param[5].Value = PartsDT.Rows[i]["ItemCode"].ToString(); //Item Code System Generted num. Get it from lookup
                            param[6].Value = PartsDT.Rows[i]["PartNo"].ToString();
                            param[7].Value = PartsDT.Rows[i]["Qty"].ToString();
                            param[8].Value = PartsDT.Rows[i]["Price"].ToString();
                            param[10].Value = PartsDT.Rows[i]["PartsDesc"].ToString();
                            param[11].Value = PartsDT.Rows[i]["Total"].ToString();
                            //CustomerEstimePartsDetail_Insert_param[6].Value = PartsDT.Rows[i]["Dep"].ToString();                            
                            SysFunc.ExecuteSP_NonQuery("sp_SchedulePartsDetail_Insert", param, Trans);
                            flag = true;
                        }
                    }
                }
                // }
                return flag;
            }
            catch (Exception ex) { throw ex; }
        }

        private bool Insert_ScheduleLubricates_Detail()
        {
            try
            {
                bool flag = false;
                SqlParameter[] CustomerEstimateLubricanteDetail_Delete_param = {                                                       
                                                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                    new SqlParameter("@ScheduleCode",SqlDbType.Char,8)
                                                                  };
                CustomerEstimateLubricanteDetail_Delete_param[0].Value = Session["DealerCode"].ToString();
                CustomerEstimateLubricanteDetail_Delete_param[1].Value = ScheduleCode.Trim();

                SqlParameter[] param = {                                                       
                                        /*0*/ new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                       /*1*/ new SqlParameter("@ProdCode",SqlDbType.VarChar,20),         
                                       /*2*/ new SqlParameter("@BrandCode",SqlDbType.VarChar,3),        
                                       /*3*/ new SqlParameter("@KM",SqlDbType.VarChar,10),                                       
                                       /*4*/ new SqlParameter("@SDays",SqlDbType.VarChar,3),
                                       /*5*/      new SqlParameter("@ItemCode",SqlDbType.Char,8),
                                       /*6*/      new SqlParameter("@PartNo",SqlDbType.VarChar,18),
                                       /*7*/      new SqlParameter("@Qty",SqlDbType.VarChar,10),
                                       /*8*/      new SqlParameter("@Price",SqlDbType.Float),
                                       /*9*/      new SqlParameter("@ScheduleCode",SqlDbType.VarChar,8),
                                       new SqlParameter("@PartsDesc",SqlDbType.VarChar,50),
                                             new SqlParameter("@Total",SqlDbType.Float)
                                                                    
                                                                 };

                if (SysFunc.ExecuteSP_NonQuery("sp_W2_ScheduleCode_LubricanteDetail_Delete", CustomerEstimateLubricanteDetail_Delete_param, Trans))
                {
                    LubDT = (DataTable)ViewState["Lub"];
                    DataRow[] drr = LubDT.Select();
                    for (int i = 0; i < drr.Length; i++)
                    {
                        param[0].Value = Session["DealerCode"].ToString();
                        param[1].Value = ddlVehicleCode.SelectedValue;
                        param[2].Value = ddlBrand.SelectedValue;
                        param[3].Value = txtScheduleKm.Text;
                        param[4].Value = txtNextVisit.Text;
                        param[9].Value = ScheduleCode.Trim();
                        if (LubDT.Rows[i]["ItemCode"].ToString() != "")
                        {
                           param[5].Value = LubDT.Rows[i]["ItemCode"].ToString(); ; //Item Code System Generted num. Get it from lookup
                           param[6].Value = LubDT.Rows[i]["PartNo"].ToString();
                           param[7].Value = LubDT.Rows[i]["Qty"].ToString();
                           param[8].Value = LubDT.Rows[i]["Price"].ToString();
                           param[10].Value = LubDT.Rows[i]["PartsDesc"].ToString();
                           param[11].Value = LubDT.Rows[i]["Total"].ToString();
                            if (SysFunc.ExecuteSP_NonQuery("sp_ScheduleLubricantDetail_Insert",param, Trans))
                            {
                                flag = true;
                            }
                            else
                            {
                                flag = false;
                            }

                        }
                    }
                }
                // }
                return flag;
            }
            catch (Exception ex) { throw ex; }

        }

        protected void clearAll()
        {
            ddlSchedule.SelectedIndex = 0;
            //ddlParts.SelectedIndex = 0;
            ddllLubs.SelectedIndex = 0;
            ddlBrand.SelectedIndex = 0;
            ddlVehicleCode.SelectedIndex = 0;
            txtScheduleKm.Text = string.Empty;
            txtNextVisit.Text = string.Empty;
            txtReminderLeter.Text = string.Empty;
            txtCallAlert.Text = string.Empty;
            txtSms.Text = string.Empty;
            txtSchedule.Text = string.Empty;
            ClearJobTextBoxes();
            ClearPartsTextBoxes();
            ClearLubricantsTextBoxes();
            //txtPartTotalQuantity.Text = txtPartsTotal.Text =
            //lbltotLubQty.Text = txtLubTotal.Text =
            //lblJobCardTotal.Text = txtJobsTotal.Text = "0";
                
            
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            clearAll();
            createJobDT();
            createLubDT();
            createPartsDT();
            btnDelete.Enabled = false;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);
        }

        protected void btnDelete_Click1(object sender, EventArgs e)
        {
            if (ddlSchedule.SelectedIndex == 0)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, "Select Scedule code first. ");
                txtSchedule.Focus();
                return;
            }            

            if (Delete(ddlSchedule.SelectedValue.ToString().Trim()) == false)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, "Schecule code not deleted Try again");
                return;
            }
            else
            {
                SysFunc.UserMsg(lblMsg, Color.Green, "Schedule Code '"+txtSchedule.Text+"' deleted successfully ");
                //SendAlert("Customer Estimate Code deleted successfully !!!");
                clearAll();
                createJobDT();
                createLubDT();
                createPartsDT();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Deletealert()", true);
            }
        }

        public bool Delete(string strCode)
        {
            //#BEGIN TRANSACTION
            SqlTransaction stDeleteJobCard = null;
            //#IF TRANSACTION COULD'NT BE STARTED RETURN FALSE
            if ((!ObjTrans.BeginTransaction(ref stDeleteJobCard)))
            {
                return false;
            }

            //1. Delete From Shedule Master
            string sJobCardMasterUpdateQuery = "Delete From MaintainenceSchedule where ScheduleCode = '" + strCode + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
            try
            {
                if ((!SysFuncs.ExecuteQuery_NonQuery(sJobCardMasterUpdateQuery, stDeleteJobCard)))
                {
                    //IF THE step return false exit from the function
                    ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                SysFunc.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);

                return false;
            }

            //2. DELETE FROM CustomerEstimateDetail
            string sJobCardDetailDeleteQuery = "DELETE FROM ScheduleJobsDetail Where ScheduleCode ='" + strCode + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
            try
            {
                if ((!SysFuncs.ExecuteQuery_NonQuery(sJobCardDetailDeleteQuery, stDeleteJobCard)))
                {
                    //IF THE step return false exit from the function
                    ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                SysFunc.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);

                return false;
            }

            //3. DELETE FROM CustomerEstimateParts
            string sJobCardPartDeleteQuery = "DELETE FROM SchedulePartsDetail Where ScheduleCode ='" + strCode + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
            try
            {
                if ((!SysFuncs.ExecuteQuery_NonQuery(sJobCardPartDeleteQuery, stDeleteJobCard)))
                {
                    //IF THE step return false exit from the function
                    ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                SysFunc.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);

                return false;
            }

            //4.DELETE FROM CustomerEstimateLubricates
            string sJobCardLubDeleteQuery = "DELETE FROM ScheduleLubsDetail Where ScheduleCode ='" + strCode + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
            try
            {
                if ((!SysFuncs.ExecuteQuery_NonQuery(sJobCardLubDeleteQuery, stDeleteJobCard)))
                {
                    //IF THE step return false exit from the function
                    ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ObjTrans.RollBackTransaction(ref stDeleteJobCard);
                SysFunc.UserMsg(lblMsg, Color.Red, "Error: " + ex.Message);

                return false;
            }

            //#COMMIT TRANSACTION
            ObjTrans.CommittTransaction(ref stDeleteJobCard);
            return true;
        }

        protected void ddlEstCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSchedule.SelectedIndex == 0)
            {
                btnDelete.Enabled = false;
                return;
                
            }
            ddlVehicleCode.Enabled = false;
            LoadMasterData();
            btnDelete.Enabled = true;
        }
        
        protected void LoadMasterData()
        {
            SqlParameter[] dsParam = {                                                       
                                    new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                    new SqlParameter("@CustomerEstimateCode",SqlDbType.Char,8)
                                 };
            dsParam[0].Value = Session["DealerCode"].ToString();
            dsParam[1].Value = ddlSchedule.SelectedValue.ToString();

            string sql = "Select * from MaintainenceSchedule where ScheduleCode = '" + ddlSchedule.SelectedValue + "' and DealerCode = '" + Session["DealerCode"].ToString() +"'";
            string sql1 = "Select A.*,B.EmpName from ScheduleJobsDetail A , DealerEmp B  where A.EmpCode = B.EmpCode and ScheduleCode = '" + ddlSchedule.SelectedValue + "' and A.DealerCode = '" + Session["DealerCode"].ToString() + "'";
            string sql2 = "Select * from SchedulePartsDetail where ScheduleCode = '" + ddlSchedule.SelectedValue + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";
            string sql3 = "Select * from ScheduleLubsDetail where ScheduleCode = '" + ddlSchedule.SelectedValue + "' and DealerCode = '" + Session["DealerCode"].ToString() + "'";

            DataTable dt = new DataTable();
            dt = SysFunc.GetData(sql);

            if (dt.Rows.Count == 0)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = "No Record Found";
                return;
            }

            txtSchedule.Text = dt.Rows[0]["ScheduleCode"].ToString();
            ddlBrand.SelectedValue = dt.Rows[0]["BrandCode"].ToString();
            objMBLL.FillDropDown(ddlVehicleCode, "Select Distinct ltrim(rtrim(ProdCode)) ProdCode, ProdDesc from Vehicle where BrandCode = '" + ddlBrand.SelectedValue + "' and DealerCode IN('" + Session["DealerCode"].ToString() + "', 'COMON') ", "ProdDesc", "ProdCode", "Select");
            ddlVehicleCode.Enabled = true;
            ddlVehicleCode.SelectedValue = dt.Rows[0]["ProdCode"].ToString();
            txtScheduleKm.Text = dt.Rows[0]["KM"].ToString();
            txtNextVisit.Text = dt.Rows[0]["SDays"].ToString();
            txtReminderLeter.Text = dt.Rows[0]["ReminderLetter"].ToString();
            txtCallAlert.Text = dt.Rows[0]["CallAlert"].ToString();
            txtSms.Text = dt.Rows[0]["SmsAlert"].ToString();
            ddlVersion.SelectedValue = dt.Rows[0]["VersionCode"].ToString();
            dsJobCardDetail = new DataTable();
            dsJobCardDetail = SysFuncs.GetData(sql1);
            //if (dsJobCardDetail.Tables[0].Rows.Count == 0) dsJobCardDetail.Tables[0].Rows.Add(dsJobCardDetail.Tables[0].NewRow());
            ViewState["Job"] = dsJobCardDetail; gvJobCard.DataSource = dsJobCardDetail; gvJobCard.DataBind();

            dsJobCardParts = new DataTable();
            dsJobCardParts = SysFuncs.GetData(sql2);
            //if (dsJobCardParts.Tables[0].Rows.Count == 0) dsJobCardParts.Tables[0].Rows.Add(dsJobCardParts.Tables[0].NewRow());
            ViewState["Parts"] = dsJobCardParts; gvJobCardParts.DataSource = dsJobCardParts; gvJobCardParts.DataBind();

            dsJobCardLub = new DataTable();
            dsJobCardLub = SysFuncs.GetData(sql3);
            //if (dsJobCardLub.Tables[0].Rows.Count == 0) dsJobCardLub.Tables[0].Rows.Add(dsJobCardLub.Tables[0].NewRow());
            ViewState["Lub"] = dsJobCardLub; gvLubParts.DataSource = dsJobCardLub; gvLubParts.DataBind();
        }

        protected void btnLookup_Click(object sender, EventArgs e)
        {
            ViewState["lookupid"] = 10; ViewState["ixd1"] = 1; ViewState["ixd2"] = 2; ViewState["ixd3"] = 3;
            clslook.LU_Get_Parts(imgLookup, ViewState["lookupid"].ToString(), "", "../../../");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Close Look Up Window First')", true);

        }

        protected void SelectedPartDetail(string item)
        {
            try
            {
                
                DataTable dt = new DataTable();
                dt = SysFunc.GetData("select ItemDesc,SaleRate,PartItemNo from Item where DealerCode in ('" + Session["DealerCode"].ToString() + "','COMON') And  ItemCode='" + item + "'");
                txtItemcodeParts.Text = item;
                txtPartDesc.Text = dt.Rows[0]["ItemDesc"].ToString().Trim();
                txtPartCode.Text = dt.Rows[0]["PartItemNo"].ToString().Trim();
                txtPartPrice.Text = dt.Rows[0]["SaleRate"].ToString().Trim();
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(lblMsg, Color.Red, ex.Message);
            }
        }

    }
}