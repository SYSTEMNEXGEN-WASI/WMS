using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using DXBMS;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace DXBMS.Modules.Setup
{
    public partial class FeedBackQuestion : System.Web.UI.Page
    {
        SysFunction sysFunc = new SysFunction();
        SysFunctions sysFuncs = new SysFunctions();
        DataSet ds;
        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;
        bool search_result, search_item;
        DataTable FeedBackDT;
        double total;
        int i = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["FeedBackDetail"] != null) FeedBackDT = (DataTable)Session["FeedBackDetail"];
            if (!IsPostBack)
            {
                //createGrid("");
                lOADddl();
                lOADddl2();

                //sysFuncs.Load_CounterSalesItem_ddl(ddlJobCardCode, Session["DealerCode"].ToString(), "P");
                
            }
        }

        private void createGrid(string where)
        {
            String query;
            if (ddlfeedbackNo.SelectedIndex == 0)
            {

                query = "select * from FeedBackQuestions " + where;
                i = 0;

            }
            else
            {
                query = "exec [SP_SelectFeedBackDetail] '" + Session["DealerCode"] + "','" + ddlfeedbackNo.SelectedValue + "'";
            }

            DataTable dtFeedBackQues = new DataTable();
            dtFeedBackQues = sysFunc.GetData(query);

            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("QSequence", typeof(string)));
            dt.Columns.Add(new DataColumn("ParentQuestionDesc", typeof(string)));
            dt.Columns.Add(new DataColumn("ParentQuestionCode", typeof(string)));
            dt.Columns.Add(new DataColumn("ControlType", typeof(string)));
            dt.Columns.Add(new DataColumn("ServiceType", typeof(string)));
            dt.Columns.Add(new DataColumn("Rating", typeof(string)));
            for (int j = 0; j < dtFeedBackQues.Rows.Count; j++)
            {
                dr = dt.NewRow();

                dr["QSequence"] = dtFeedBackQues.Rows[j]["QSequence"].ToString();
                dr["ParentQuestionDesc"] = dtFeedBackQues.Rows[j]["ParentQuestionDesc"].ToString();
                dr["ParentQuestionCode"] = dtFeedBackQues.Rows[j]["ParentQuestionCode"].ToString();
                dr["ControlType"] = dtFeedBackQues.Rows[j]["ControlType"].ToString();
                dr["ServiceType"] = dtFeedBackQues.Rows[j]["ServiceType"].ToString();

                dt.Rows.Add(dr);
            }
            Session["FeedbackQuestionsDS"] = dt;

            gv_FeedbackQuestions.DataSource = dt;
            gv_FeedbackQuestions.DataBind();


        }

        private void lOADddl()
        {

            string WhereQuery = "DealerCode= '" + Session["DealerCode"].ToString() + "' ";
            string[] col = new string[] { "JobCardCode", "UserName", "EngineNo", "ChassisNo" };
            sysFunc.GetMultiColumnsDDL(ddlJobCardCode, col, "JobCardMaster", WhereQuery, "JobCardCode", "", false, false);


            txtfeedbackDate.Text = DateTime.Now.ToString("dd-MM-yyyy");


        }
        private void lOADddl2()
        {
            string WhereQuery = "DealerCode= '" + Session["DealerCode"].ToString() + "' ";
            string[] col = new string[] { "FeedBackCode", "FeedBackDate", "ServiceType", "JobCardCode" };
            sysFunc.GetMultiColumnsDDL(ddlfeedbackNo, col, "FeedBackMaster", WhereQuery, "FeedBackCode", "", false, false);

        }


        protected void BtnSubmit_Click(object sender, EventArgs e)
        {


            try
            {
                string max;
                if (ddlfeedbackNo.SelectedIndex <= 0)
                {
                    max = sysFuncs.AutoGen("FeedBackMaster", "FeedBackCode", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("dd/MM/yyyy"));

                }
                else
                {
                    max = ddlfeedbackNo.SelectedValue.Trim();
                }


                SqlParameter[] param =
           {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                new SqlParameter("@FeedBackCode",SqlDbType.Char,8), //1
                new SqlParameter("@FeedBackDate",SqlDbType.DateTime),//2
                new SqlParameter("@JobcardCode",SqlDbType.Char,8),//2
                new SqlParameter("@CusCode",SqlDbType.Char,8),
                new SqlParameter("@ServiceType",SqlDbType.VarChar,50),
                new SqlParameter("@DelFlag",SqlDbType.Char,1),
                new SqlParameter("@UpdUser",SqlDbType.VarChar,50),
           };
                FeedBackDT = (DataTable)Session["FeedbackQuestionsDS"];
                DataRow[] drr = FeedBackDT.Select();

                param[0].Value = Session["DealerCode"].ToString(); // 0
                param[1].Value = max;
                param[2].Value = sysFuncs.SaveDate(txtfeedbackDate.Text);
                param[3].Value = ddlJobCardCode.SelectedValue;
                param[4].Value = txtCusCode.Text;
                param[5].Value = ddlServiceType.SelectedValue.ToString();
                param[6].Value = "N";
                param[7].Value = Session["UserName"];

                if (sysFunc.ExecuteSP_NonQuery("SP_SaveFeedbackQuestions", param))
                {
                    SqlParameter[] param2 = {

                                    new SqlParameter("@DealerCode", SqlDbType.Char, 5),//0
                                    new SqlParameter("@FeedBackCode",SqlDbType.Char,8), //1
                                    new SqlParameter("@JobcardCode",SqlDbType.Char,8),
                                    new SqlParameter("@ParentQuestionCode",SqlDbType.Char,8),  //3
                                    new SqlParameter("@Rating",SqlDbType.VarChar,20),  //4
                                    
                    };

                    FeedBackDT = (DataTable)Session["FeedbackQuestionsDS"];
                    for (int i = 0; i < drr.Length; i++)
                    {
                        //if (FeedBackDT.Rows[i]["ItemCode"].ToString() != "")
                        //{

                        //}
                        param2[0].Value = Session["DealerCode"].ToString(); // 0
                        param2[1].Value = max;                               //1
                        param2[2].Value = ddlJobCardCode.SelectedValue;
                        param2[3].Value = FeedBackDT.Rows[i]["ParentQuestionCode"].ToString();
                        param2[4].Value = FeedBackDT.Rows[i]["Rating"].ToString();


                        sysFunc.ExecuteSP_NonQuery("SP_InsertQuestionsDetail", param2);

                    }



                    //createGrid();
                    lblMessage.Text = "FeeBack Inserted.";
                    BtnClear_Click(null, null);
                    gv_FeedbackQuestions.DataSource = null;
                    gv_FeedbackQuestions.DataBind();
                }
            }
            catch (Exception ex)
            {

                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);
            }

        }
        protected void BtnClear_Click(object sender, EventArgs e)
        {

            ddlfeedbackNo.SelectedIndex = 0;
            ddlJobCardCode.SelectedIndex = 0;
            ddlServiceType.SelectedIndex = 0;
            txtJobCardType.Text = "";
            txtCustomerName.Text = "";
            txtProduct.Text = "";
            txtCustomerChassisNo.Text = "";
            txtRegNo.Text = "";

            gv_FeedbackQuestions.DataSource = null;
            gv_FeedbackQuestions.DataBind();
        }


        protected void BtnPrint_Click(object sender, EventArgs e)
        {
            Data.DSReports data = new Data.DSReports();
            ReportDocument RD = new ReportDocument();



            SqlParameter[] param =
            {
             new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
             new SqlParameter("@FeedBackCode",SqlDbType.Char,8), //1

        };

            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = ddlfeedbackNo.SelectedValue.Trim();

            SqlDataReader rder = null;

            // Stored Procedure check and load
            if (sysFunc.ExecuteSP("SP_FeedBackMaster_Print", param, ref rder)) //Database SP
            {
                data.SP_FeedBackMaster_Print.Load(rder);                       // DataSet SP

            }
            // Load in crystal report
            //RD.Load(Server.MapPath("../PurchaseSaleReturnReport.rpt"));
            //RD.Load(Server.MapPath("../crtStockAdjustment.rpt"));
            RD.Load(Server.MapPath("Reports/FeedBackReport.rpt"));

            // mention paper size
            RD.PrintOptions.PaperSize = PaperSize.PaperA4;


            ////RD.Load(Server.MapPath("../../SpareReports/rptInvoices.rpt"));
            //// RD.Load(Server.MapPath("../rptJobHistory.rpt"));
            ////RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + sysFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Phone1", "Dealer") + "'";
            //// RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + sysFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Email", "Dealer") + "'";

            //RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + sysFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "DealerDesc", "Dealer") + "'";
            //RD.DataDefinition.FormulaFields["DealerAddress"].Text = "'" + sysFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Address1", "Dealer") + "" +
            //                                                               sysFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Address2", "Dealer") + "" +
            //                                                               sysFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Address3", "Dealer") + " (" +
            //                                                               sysFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "Phone1", "Dealer") + ") '";
            //RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'" + sysFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "SaleTaxNo", "Dealer") + "'";
            //RD.DataDefinition.FormulaFields["NTN"].Text = "'" + sysFunc.GetStringValuesAgainstCodes("DealerCode", Session["DealerCode"].ToString(), "NTN", "Dealer") + "'";
            ////RD.DataDefinition.FormulaFields["UserID"].Text = "'" + Session["UserID"].ToString() + "'";
            ////RD.DataDefinition.FormulaFields["FromCode"].Text = "'" + FDate + "'";
            ////RD.DataDefinition.FormulaFields["ToCode"].Text = "'" + TDate + "'";
            //RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Environment.MachineName + "'";
            //RD.DataDefinition.FormulaFields["ReportTitle"].Text = "\"" + "STOCK ADJUSTMENT REPORT" + "\"";
            ////RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            ////RD.DataDefinition.FormulaFields["DealershipName"].Text = "'Authorised " + Session["ParentDesc"].ToString() + " Dealership'";
            //// RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";


            RD.Database.Tables[0].SetDataSource(data);

            // convert and show
            string FilePath = Server.MapPath("~") + "\\Download\\";
            string FileName = "FeedBackQuestions" + this.Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;

            RD.ExportToDisk(ExportFormatType.PortableDocFormat, File);

            string URL;
            URL = "../../Download/OpenPDF.aspx?FileName=" + FileName;
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);

        }


        #region Master Validation

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

                    lblMessage.Text = "Error: Field(s) Marked With Red Steriks '*' Are Mendetory...";
                    break;
                }
                //else { textBoxes[i].BorderColor = System.Drawing.ColorTranslator.FromHtml("#1A6297"); }
            }
            return isValid;
        }
        #endregion
        protected void BtnClear_Delete(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] FeedBack_Detail_Delete_param = {
                                                                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                                                new SqlParameter("@FeedBackCode",SqlDbType.Char,8)
                                                              };
                FeedBack_Detail_Delete_param[0].Value = Session["DealerCode"].ToString();
                FeedBack_Detail_Delete_param[1].Value = ddlfeedbackNo.SelectedValue.ToString().Trim();
                if (sysFunc.ExecuteSP_NonQuery("sp_Delete_FeedBackDetail", FeedBack_Detail_Delete_param))
                {

                    lblMessage.Text = "Record Deleted Successfully: ";
                    BtnClear_Click(null, null);
                }
                else
                {
                    lblMessage.Text = "Record not Deleted: ";
                }
                gv_FeedbackQuestions.DataSource = null;
                gv_FeedbackQuestions.DataBind();

            }

            catch (Exception ex)
            {
                sysFunc.UserMsg(lblMessage, System.Drawing.Color.Red, ex.Message);

            }
        }


        protected void gv_FeedbackQuestions_RowDataBound(object sender, GridViewRowEventArgs e)
        {


        }
        protected void gv_FeedbackQuestions_DataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dtFeedBackQues = (DataTable)Session["FeedbackQuestionsDS"];
            RadioButtonList rdButton = (RadioButtonList)e.Row.FindControl("rblYesOrNo");
            CheckBoxList chkBox = (CheckBoxList)e.Row.FindControl("cblYesOrNo");
            if (e.Row.RowType == DataControlRowType.Header)
            {
                i = 0;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (i < dtFeedBackQues.Rows.Count)
                {

                    if (dtFeedBackQues.Rows[i]["ControlType"].ToString() == "Option")
                    {
                        rdButton.Items.Clear();
                        rdButton.Items.Add(new ListItem("Yes", "Yes"));
                        rdButton.Items.Add(new ListItem("No", "No"));
                        i++;
                    }
                    else
                    //if (dtFeedBackQues.Rows[i]["ControlType"].ToString() == "Rating")
                    {
                        rdButton.Items.Clear();
                        rdButton.Items.Add(new ListItem("Excellent", "Excellent"));
                        rdButton.Items.Add(new ListItem("Good", "Good"));
                        rdButton.Items.Add(new ListItem("Average", "Average"));
                        rdButton.Items.Add(new ListItem("Poor", "Poor"));
                        rdButton.Items.Add(new ListItem("Very Poor", "Very Poor"));
                        i++;
                    }

                }



            }

        }

        protected void ddlJobCardCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            String Sql = "select b.JobTypeDesc,a.CusCode, a.UserName, a.ProdCode, a.ChassisNo, a.RegNo from JobCardMaster a INNER JOIN JobTypeMaster b ON b.JobTypeCode = a.JobCardType where a.DealerCode = '"+Session["DealerCode"].ToString()+"' and a.JobCardCode = '"+ddlJobCardCode.SelectedValue+"'";

            dt = sysFunc.GetData(Sql);

            txtJobCardType.Text = dt.Rows[0]["JobTypeDesc"].ToString().Trim();
            txtCusCode.Text = dt.Rows[0]["CusCode"].ToString().Trim();
            txtCustomerName.Text = dt.Rows[0]["UserName"].ToString().Trim();

            txtCustomerChassisNo.Text = dt.Rows[0]["ChassisNo"].ToString().Trim();

            txtRegNo.Text = dt.Rows[0]["RegNo"].ToString().Trim();

            txtProduct.Text = dt.Rows[0]["ProdCode"].ToString().Trim();


        }

        protected void ddlFeedBackCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            String Sql = "exec SP_SelectFeedBackCode '" + Session["DealerCode"] + "','" + ddlfeedbackNo.SelectedValue + "'";

            dt = sysFunc.GetData(Sql);

            txtfeedbackDate.Text = dt.Rows[0]["FeedBackDate"].ToString().Trim();

            ddlJobCardCode.SelectedValue = dt.Rows[0]["JobCardCode"].ToString();

            txtJobCardType.Text = dt.Rows[0]["JobTypeDesc"].ToString().Trim();
            txtCusCode.Text = dt.Rows[0]["CusCode"].ToString().Trim();
            txtCustomerName.Text = dt.Rows[0]["UserName"].ToString().Trim();

            txtCustomerChassisNo.Text = dt.Rows[0]["ChassisNo"].ToString().Trim();

            txtRegNo.Text = dt.Rows[0]["RegNo"].ToString().Trim();

            txtProduct.Text = dt.Rows[0]["ProdCode"].ToString().Trim();

            ddlServiceType.SelectedValue = dt.Rows[0]["ServiceType"].ToString();

            //createGrid(",FeedBackMaster where FeedBackCode= '" + ddlfeedbackNo.SelectedValue + "'");
            createGrid("");


        }


        protected void ddlServiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string where = "where ServiceType = '" + ddlServiceType.SelectedValue + "'";
            createGrid(where);
        }

        protected void rblYesOrNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["FeedbackQuestionsDS"];

            RadioButtonList rblYessOrNo = sender as RadioButtonList;
            TableCell tc = rblYessOrNo.Parent as TableCell;
            GridViewRow gvr = tc.Parent as GridViewRow;
            RadioButtonList radio = (RadioButtonList)gvr.Cells[2].FindControl("rblYesOrNo");
            Label lblParentQuestionCode = (Label)gvr.Cells[2].FindControl("lblParentQuestionCode");

            string value = radio.SelectedValue;

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ParentQuestionCode"].ToString() == lblParentQuestionCode.Text)
                {
                    dr["Rating"] = radio.SelectedValue;

                }
            }

            Session["FeedbackQuestionsDS"] = dt;

        }
    }
}