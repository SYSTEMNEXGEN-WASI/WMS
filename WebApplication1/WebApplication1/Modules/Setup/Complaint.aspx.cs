using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DXBMS.Modules.Setup
{
    public partial class Complaint : System.Web.UI.Page
    {
        SysFunction sysFunc = new SysFunction();
        DataSet ds;
        SysFunctions sysFuncs = new SysFunctions();

        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Load_ddlComplaintby();
                createGrid();

            }
            //if (IsPostBack)
            //{
            //    ///btnClear_Click();
            //    sysFunc.ClearTextBoxes(Page);
            //    Image1.ImageUrl = "YourIageurl" + "?n=" + DateTime.Now.Second.ToString();
            //}
        }
        private void createGrid()
        {

            String query = "select TicketNo, Format(TicketDate,'dd-MM-yyyy') [TicketDate],ComplaintTypeCode,ComplaintDesc,status,StatusbyOEM,Priority,StatusRemarks from Complaint";

            DataTable dt = new DataTable();

            dt = sysFunc.GetData(query);

            if (dt.Rows.Count > 0)
            {
                gv_Complaint.DataSource = dt;
                gv_Complaint.DataBind();
            }

            Session["ComplaintDS"] = dt;

        }
        protected void btnInsert_Click(object sender, EventArgs e)
        {

            if (txtTicketDate.Text == "")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Ticket Date can not be left blank";
                return;
            }
            if (txtSubject.Text == "")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Subject can not be left blank";
                return;
            }
            if (ddlComplaintby.SelectedValue == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Complaint By can not be left blank";
                return;
            }

            if (txtComplaint.Text == "")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Complaint Description can not be left blank";
                return;
            }
            if (txtRemarks.Text == "")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Remarks can not be left blank";
                return;
            }

            if (ddlPriority.SelectedValue == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Select an option from Priority";
                return;
            }
            if (ddlType.SelectedValue == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Select an option from Type";
                return;
            }
            if (ddlStatus.SelectedValue == "0")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Select an option from Status";
                return;
            }
            try
            {
                string imgpath = Session["ImagePath"].ToString();
                string max = "";
                if (txtTicketNo.Text == "")
                {
                    max = sysFunc.GetNewMaxID("Complaint", "TicketNo", 5, Session["DealerCode"].ToString());

                }
                else
                {
                    max = txtTicketNo.Text;


                }




                SqlParameter[] param =
               {
                new SqlParameter("@DealerCode",SqlDbType.Char,5),//0
                new SqlParameter("@TicketNo",SqlDbType.Char,5), //1
                new SqlParameter("@TicketDate",SqlDbType.DateTime),//2
               // new SqlParameter("@Type",SqlDbType.VarChar,10),//3
                new SqlParameter("@ComplaintTypeCode",SqlDbType.Char,5),//3
                new SqlParameter("@ComplaintDesc",SqlDbType.VarChar,100),//4
                new SqlParameter("@ComplaintImg",SqlDbType.VarChar,50),//5
                new SqlParameter("@status",SqlDbType.VarChar,10),//6
                new SqlParameter("@StatusbyOEM",SqlDbType.VarChar,10),//7
                new SqlParameter("@StatusRemarks",SqlDbType.VarChar,50),//8
                 new SqlParameter("@Priority",SqlDbType.Char,10),//9
                new SqlParameter("@UpdUser",SqlDbType.VarChar,50), //10
                new SqlParameter("@UpdDate",SqlDbType.DateTime), //11
                new SqlParameter("@UpdTime",SqlDbType.DateTime), //12
                new SqlParameter("@UpdTerm",SqlDbType.VarChar,50),// 13
                new SqlParameter("@Subject",SqlDbType.VarChar,50),// 14
                new SqlParameter("@Complaintby",SqlDbType.Char,5),// 15
                 };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = max;
                param[2].Value = txtTicketDate.Text.Trim();
                param[3].Value = ddlType.SelectedValue.Trim();
                param[4].Value = txtComplaint.Text;
                param[5].Value = imgpath;
                param[6].Value = ddlStatus.SelectedValue.Trim();
                param[7].Value = txtStatusByOEM.Text.Trim();
                param[8].Value = txtRemarks.Text.Trim();
                param[9].Value = ddlPriority.SelectedValue.Trim();
                param[10].Value = Session["UserName"].ToString();
                param[11].Value = sysFunc.SaveDate(DateTime.Now.ToString("dd-MM-yyyy"));
                param[12].Value = DateTime.Now;
                param[13].Value = "1";
                param[14].Value = txtSubject.Text;
                param[15].Value = ddlComplaintby.SelectedValue.Trim();



                if (sysFunc.ExecuteSP_NonQuery("SP_Insert_Complaint", param))
                {


                    createGrid();

                    if (txtTicketNo.Text == "")
                    {
                        lblMsg.ForeColor = System.Drawing.Color.Green;

                        lblMsg.Text = "New Complaint Inserted.";
                    }
                    else
                    {
                        lblMsg.ForeColor = System.Drawing.Color.Green;

                        lblMsg.Text = "New Complaint Updated.";


                    }
                    sysFunc.ClearTextBoxes(Page);
                }
            }
            catch (Exception ex)
            {

                sysFunc.UserMsg(lblMsg, System.Drawing.Color.Red, ex.Message);
            }

        }





        protected void btnUpload_Click(object sender, EventArgs e)
        {
            HttpPostedFile postedFile = FileUpload1.PostedFile;

            Session["Image"] = postedFile;

            string imgName = FileUpload1.FileName;
            string imgPath = "Images/" + imgName;

            int imgSize = FileUpload1.PostedFile.ContentLength;

            string fileExtension = Path.GetExtension(imgName);

            if (fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".gif"
                || fileExtension.ToLower() == ".png" || fileExtension.ToLower() == ".bmp")
            {
                FileUpload1.SaveAs(Server.MapPath(imgPath));
                Image1.ImageUrl = "~/" + imgPath;
                Session["ImagePath"] = imgPath;
                Session["Image"] = imgPath;
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Only images (.jpg, .png, .gif and .bmp) can be uploaded";
            }

        }


        private bool IsImage(HttpPostedFile file)
        {
            //Checks for image type... you could also do filename extension checks and other things
            return ((file != null) && System.Text.RegularExpressions.Regex.IsMatch(file.ContentType, "image/\\S+") && (file.ContentLength > 0));
        }

        private void Load_ddlComplaintby()
        {
            string WhereClause = "DealerCode = '" + Session["DealerCode"] + "'";


            string[] Columns = new string[] { "EmpCode", "EmpName" };
            sysFuncs.GetMultiColumnsDDL(ddlComplaintby, Columns, "DealerEmp ", WhereClause, "EmpCode", " Order by EmpCode Desc ", false, false);
        }




        protected void btnClear_Click(object sender, EventArgs e)

        {
            txtSubject.Text = "";
            txtTicketNo.Text = "";
            txtTicketDate.Text = "";
            ddlComplaintby.SelectedIndex = 0;
            ddlType.SelectedIndex = 0;
            ddlPriority.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            txtStatusByOEM.Text = "";
            txtRemarks.Text = "";
            txtComplaint.Text = "";
            Image1.ImageUrl = "YourIageurl" + "?n=" + DateTime.Now.Second.ToString();
            lblMsg.Text = "";
        }

        protected void gv_Complaint_DataBound(object sender, EventArgs e)
        {

        }

        protected void gv_Complaint_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && !(gv_Complaint.EditIndex == e.Row.RowIndex))
                {
                    e.Row.Cells[0].Enabled = true;
                }

                if (e.Row.RowType == DataControlRowType.DataRow && (gv_Complaint.EditIndex == e.Row.RowIndex))
                {
                    TextBox txtTicketNo = (TextBox)e.Row.FindControl("TicketNo");

                    txtTicketNo.Text = Session["lblTicketNo"].ToString();


                    //DropDownList ddlEditType = (DropDownList)e.Row.FindControl("Type");
                    // ddlEditType.Text = Session["lblType"].ToString();

                    DropDownList ddlEditTaxApplicable = (DropDownList)e.Row.FindControl("Status");
                    ddlEditTaxApplicable.Text = Session["lblStatus"].ToString();

                    TextBox txtEditPer = (TextBox)e.Row.FindControl("StatusByOEM");

                    txtEditPer.Text = Session["lblStatusByOEM"].ToString();

                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {

                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }

        }

        protected void gv_Complaint_SelectedIndexChanged(object sender, EventArgs e)
        {

            GridViewRow row = gv_Complaint.SelectedRow;
            Label lblTicketNo = row.FindControl("lblTicketNo") as Label;
            Label lblPriority = row.FindControl("lblPriority") as Label;
            Label lblTicketDate = row.FindControl("lblTicketDate") as Label;
            Label lblType = row.FindControl("lblType") as Label;
            Label lblComplaint = row.FindControl("lblComplaint") as Label;
            Label lblStatus = row.FindControl("lblStatus") as Label;
            Label lblStatusByOEM = row.FindControl("lblStatusByOEM") as Label;
            Label lblRemarksByOEM = row.FindControl("lblRemarksByOEM") as Label;



            txtTicketNo.Text = lblTicketNo.Text;
            txtTicketDate.Text = lblTicketDate.Text;
            ddlType.SelectedItem.Text = lblType.Text;
            ddlPriority.SelectedItem.Text = lblPriority.Text;
            txtComplaint.Text = lblComplaint.Text;
            ddlStatus.SelectedItem.Text = lblStatus.Text;
            txtStatusByOEM.Text = lblStatusByOEM.Text;
            txtRemarks.Text = lblRemarksByOEM.Text;

        }

        protected void BtnRemove_Record_Click(object sender, ImageClickEventArgs e)
        {
        }

        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            DataSet dsdelete = new DataSet();

            DataTable dt = new DataTable();

            try
            {
                string sql = "Delete  from Complaint where  TicketNo='" + txtTicketNo.Text.Trim() + "'";
                sysFunc.ExecuteQuery_NonQuery(sql);
                createGrid();
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message;
            }


        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            SysFunction sysFunc = new SysFunction();
            Data.DSReports data = new Data.DSReports();
            ReportDocument RD = new ReportDocument();



            SqlParameter[] param =
            {
            new SqlParameter("@DealerCode",SqlDbType.Char),//0
           new SqlParameter("@TicketNo",SqlDbType.Char),//0

        };

            param[0].Value = Session["DealerCode"].ToString();
            param[1].Value = txtTicketNo.Text;

            SqlDataReader rder = null;

            // Store Procedure check and load
            if (sysFunc.ExecuteSP("SP_Report_Complaint", param, ref rder)) //Database SP
            {
                data.EnforceConstraints = false;
                data.SP_Report_Complaint.Load(rder);                       // DataSet SP

            }
            // Load in crystal report
            //RD.Load(Server.MapPath("../PurchaseSaleReturnReport.rpt"));
            RD.Load(Server.MapPath("/Reports/rptComplaint.rpt"));
            RD.DataDefinition.FormulaFields["DealerName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            RD.DataDefinition.FormulaFields["DealerAdress"].Text = "'" + Session["DealerAddress"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["DealerPhone"].Text = "'" + Session["DealerPhone"].ToString() + "'";
            // RD.DataDefinition.FormulaFields["DealerEmail"].Text = "'" + Session["DealerEmail"].ToString() + "'";
            RD.DataDefinition.FormulaFields["ReportTitle"].Text = "'Complain'";
            RD.DataDefinition.FormulaFields["Terminal"].Text = "'" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "'";
            RD.DataDefinition.FormulaFields["UserId"].Text = "'" + Session["UserName"].ToString() + "'";
            RD.DataDefinition.FormulaFields["NTN"].Text = "'N.T.N # " + Session["DealerNTN"].ToString() + "'";
            // RD.DataDefinition.FormulaFields["SalesTaxNo"].Text = "'Sales Tax No.  " + Session["DealerSaleTaxNo"].ToString() + " '";
            //rpt.DataDefinition.FormulaFields["UserCell"].Text = "'" + GetStringValuesAgainstCodes("CusCode", , "CellNo", "Customer") + "'";
            RD.DataDefinition.FormulaFields["CompanyName"].Text = "'" + Session["DealerDesc"].ToString() + "'";
            //RD.DataDefinition.FormulaFields["Pic"].Text = "'C:\\Users\\u_ahm\\OneDrive\\Documents\\Visual Studio 2010\\Projects\\WebApplication1\\WebApplication1\\" + Session["Logo"] + "'";
            RD.DataDefinition.FormulaFields["Pic"].Text = "'" + Server.MapPath("~") + Session["Logo"] + "'";
            // RD.DataDefinition.FormulaFields["ComplaintTypeCode"] = "'"+txtTicketNo.Text+"'";
            // mention paper size
            //RD.PrintOptions.PaperSize = PaperSize.PaperA4;
            RD.Database.Tables[0].SetDataSource(data);

            string FilePath = Server.MapPath("~/Download/");
            string FileName = "CounterSalesDetail" + Session["DealerCode"].ToString() + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            string File = FilePath + FileName;
            
            Session["RD"] = RD;

            string URL;
            URL = "../../../Download/rptViewerService.aspx?FileName=" + FileName;
            string fullURL = "window.open('" + URL + "', '_blank', 'height=800,width=1000,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no');";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
        }
    }
}