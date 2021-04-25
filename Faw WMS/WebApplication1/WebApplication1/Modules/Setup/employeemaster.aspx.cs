using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace DXBMS.Modules.Setup
{
    public partial class employeemaster : System.Web.UI.Page
    {

        SysFunction SysFunc = new SysFunction();
        DateTime join_date, leav_date, dob_date = System.DateTime.Now.Date;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!IsPostBack)
            {
                LoadDDL_DepartCode();
                LoadDDL_DesigCode();
                LoadDDL_EmpCode();
                Session["EmployeeImagePath"] = string.Empty;
            }
            
            success_add_item.Visible = false;
            success_add_item.ForeColor = Color.Red;
        }

        private void LoadDDL_DesigCode()
        {
            string WhereQuery = "SELECT DesigCode, DesigDesc FROM Designation WHERE DealerCode in  ('" + Session["DealerCode"] + "','COMON')";
            SysFunc.FillDropDown(ddldesig_code, WhereQuery, "DesigDesc", "DesigCode", "Select");
        }

        private void LoadDDL_DepartCode()
        {
            string WhereQuery = "SELECT DeptCode, DeptDesc FROM Department WHERE DealerCode in   ('" + Session["DealerCode"] + "','COMON')";
            SysFunc.FillDropDown(ddldep_code, WhereQuery, "DeptDesc", "DeptCode", "Select");
        }

        private void LoadDDL_EmpCode()
        {
            string WhereQuery = "DealerCode =  '" + Session["DealerCode"] + "'";
            string[] Columns = new string[] { "EmpCode", "EmpName", "CNIC" };
            SysFunc.GetMultiColumnsDDL(ddlEmployee, Columns, "DealerEmp", WhereQuery, "EmpCode", "", false, false);
        }

        private void cleardata()
        {
            SysFunc.Clearddl(Page);
            SysFunc.ClearTextBoxes(Page);
            success_add_item.Visible = false;
            Image1.ImageUrl = "~/Images/BlankProfile.jpg";
        }

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "reloads()", true);
            cleardata();
        }
        protected void btn_delete_Click(object sender, EventArgs e)
        {
            DataSet dsdelete = new DataSet();

            if (ddlEmployee.SelectedIndex > 0)
            {
                string code = ddlEmployee.SelectedValue;

                if (SysFunc.CodeExists("BookOrdMaster", "SalesPerson", code, Session["DealerCode"].ToString(), ref dsdelete))
                {
                    success_add_item.Text = "Cannot delete , this Employee is already in use";
                    return;
                }

                if (SysFunc.CodeExists("JobCardMaster", "Advisor", code, Session["DealerCode"].ToString(), ref dsdelete))
                {
                    success_add_item.Text = "Cannot delete , this Employee is already in use";
                    return;
                }

                if (SysFunc.CodeExists("JobCardDetail", "EmpCode", code, Session["DealerCode"].ToString(), ref dsdelete))
                {
                    success_add_item.Text = "Cannot delete , this Employee is already in use";
                    return;
                }

                if (SysFunc.CodeExists("StockIssueDetail", "EmpCode", code, Session["DealerCode"].ToString(), ref dsdelete))
                {
                    success_add_item.Text = "Cannot delete , this Employee is already in use";
                    return;
                }

                try
                {
                    string sql = "Delete From DealerEmp Where EmpCode = " + ddlEmployee.SelectedItem.Value + "AND DealerCode = '" + Session["DealerCode"] + "'";

                    SysFunc.ExecuteQuery_NonQuery(sql);

                    LoadDDL_EmpCode();
                    cleardata();

                    success_add_item.Visible = true;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Deletealert()", true);
                    success_add_item.Text = "Employee Deleted Successfully";
                }

                catch (Exception ex)
                {
                    success_add_item.Visible = true;
                    success_add_item.Text = ex.Message;
                }


            }
            else
            {
                success_add_item.Visible = true;
                success_add_item.ForeColor = Color.Red;
                success_add_item.Text = "Select Employee Code";
                return;
            }
        }
        protected void btn_save_Click(object sender, EventArgs e)
        {
            string strAutoCode = string.Empty;
            string active = "N";
            string leavedate = txtleaving_date.Text;

            if(txtEmpID.BorderColor == Color.Red)
            {
                success_add_item.Visible = true;
                success_add_item.Text = "This Employee No. is already in use";
                return;
            }

            if (cbActive.Checked == true)
            {
                active = "Y";
            }
            if (cbActive.Checked == false)
            {
                active = "N";
            }
            if (active == "Y" && txtleaving_date.Text.Trim() != string.Empty)
            {
                success_add_item.Visible = true;
                success_add_item.Text = "Remove Leaving Date If you are still Employee";
                return;
            }
            if (active == "N" && txtleaving_date.Text == "")
            {
                success_add_item.Visible = true;
                success_add_item.Text = "Enter Leaving Date";
                return;
            }

            //if(ddldep_code.SelectedValue == "001" && ddldesig_code.SelectedValue == "001")
            //{
            //    if(txtEmail.Text == "")
            //    {
            //        RegularExpressionValidator4.IsValid = false;
            //        RegularExpressionValidator4.Visible = true;
            //        RegularExpressionValidator4.ErrorMessage = "Email address is necessary";
            //        return;
            //    }else
            //    {
            //        RegularExpressionValidator4.Visible = false;
            //        RegularExpressionValidator4.ErrorMessage = "";
            //    }
            //}

            try
            {
                //join_date = Convert.ToDateTime(txtjoining_date.Text).Date;
                join_date = DateTime.ParseExact(txtjoining_date.Text, "dd-MM-yyyy", null);
                //dob_date = Convert.ToDateTime(txtdob.Text).Date;
                dob_date = DateTime.ParseExact(txtdob.Text, "dd-MM-yyyy", null);

                if (txtleaving_date.Text.Trim() != string.Empty)
                {
                    leav_date = DateTime.ParseExact(txtleaving_date.Text, "dd-MM-yyyy", null);
                }

                if (ddlEmployee.SelectedIndex == 0)
                {
                    strAutoCode = SysFunc.GetNewMaxID("DealerEmp", "EmpCode", 3, Session["DealerCode"].ToString());
                }
                else
                {
                    strAutoCode = ddlEmployee.SelectedItem.Value;

                }

                if (SysFunc.ValidateDate(txtdob.Text) == true)
                {
                    success_add_item.Visible = true;
                    success_add_item.Text = "Enter Valid DOB, not greater than current Date";
                    return;
                }
                if (SysFunc.ValidateDate(txtjoining_date.Text) == true)
                {
                    success_add_item.Visible = true;
                    success_add_item.Text = "Enter Valid Joining Date, not greater than current Date";
                    return;
                }

                if (SysFunc.ValidateDate(txtleaving_date.Text) == true && txtleaving_date.Text != string.Empty)
                {
                    success_add_item.Visible = true;
                    success_add_item.Text = "Enter Valid Leaving, not greater than current Date";
                    return;
                }
                if (dob_date >= join_date)
                {
                    success_add_item.Visible = true;
                    success_add_item.Text = "Enter Valid DOB, not greater than or equal to Joining Date";
                    return;
                }
                if (join_date >= leav_date && txtleaving_date.Text != string.Empty)
                {
                    success_add_item.Visible = true;
                    success_add_item.Text = "Joining Date not greater than or equal to Leaving Date";
                    return;
                }

                SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                    new SqlParameter("@EmpCode",SqlDbType.Char),//1
                                    new SqlParameter("@EmpID", SqlDbType.Char),//2
                                    new SqlParameter("@EmpName",SqlDbType.VarChar),//3
                                    new SqlParameter("@JoiningDate",SqlDbType.DateTime),//4
                                    new SqlParameter("@ContactNo",SqlDbType.VarChar),//5
                                    new SqlParameter("@DOB",SqlDbType.DateTime),//6
                                    new SqlParameter("@Experience",SqlDbType.TinyInt),//7
                                    new SqlParameter("@LeavingDate",SqlDbType.DateTime),//8
                                    new SqlParameter("@DesigCode",SqlDbType.Char),//9
                                    new SqlParameter("@DeptCode",SqlDbType.Char),//10
                                    new SqlParameter("@Remarks",SqlDbType.VarChar),//11
                                    new SqlParameter("@PrimaryTechCode",SqlDbType.Char),//12
                                    new SqlParameter("@UserName",SqlDbType.VarChar),//13
                                    new SqlParameter("@Password",SqlDbType.VarChar),//14
                                    new SqlParameter("@Role",SqlDbType.VarChar),//15
                                    new SqlParameter("@Active",SqlDbType.Char),//16
                                    new SqlParameter("@SecurityUserID",SqlDbType.Char),//17
                                    new SqlParameter("@CNIC",SqlDbType.Char),//18
                                    new SqlParameter("@Picture",SqlDbType.VarChar),//19
                                    new SqlParameter("@Email",SqlDbType.VarChar)//20
                                   };



                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = strAutoCode;
                param[2].Value = txtEmpID.Text;
                param[3].Value = txtEmp_name.Text;
                param[4].Value = join_date;
                param[5].Value = txtcontact_no.Text;
                param[6].Value = dob_date;
                param[7].Value = txtexp.Text;
                param[8].Value = SysFunc.SaveDate(txtleaving_date.Text);
                param[9].Value = ddldesig_code.SelectedItem.Value;
                param[10].Value = ddldep_code.SelectedItem.Value;
                param[11].Value = txtremarks.Text;
                param[12].Value = "";
                param[13].Value = "";
                param[14].Value = "";
                param[15].Value = "";
                param[16].Value = active;
                param[17].Value = string.Empty;
                param[18].Value = txtnic_no.Text.Trim();
                param[19].Value = Session["EmployeeImagePath"].ToString();
                param[20].Value = txtEmail.Text;

                SysFunc.ExecuteSP_NonQuery("Sp_Insert_DealerEmp", param);

                cleardata();
                LoadDDL_EmpCode();
                success_add_item.Visible = true;
                success_add_item.ForeColor = Color.Green;

                if (btn_save.Text == "Save")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Savealert()", true);
                }
                if (btn_save.Text == "Update")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "Updatealert()", true);
                }
            }
            catch (Exception ex)
            {
                success_add_item.Visible = true;
                
                success_add_item.Text = ex.Message;
            }

        }

        protected void txtEmpID_TextChanged(object sender, EventArgs e)
        {

            if (ddlEmployee.SelectedIndex == 0)
            {
                string sql = "Select * from DealerEmp D where D.EmpID = '" + txtEmpID.Text + "' and D.DealerCode = '" + Session["DealerCode"].ToString() + "'";
                DataTable dt = new DataTable();
                dt = SysFunc.GetData(sql);
                if (dt.Rows.Count > 0)
                {
                    txtEmpID.BorderColor = Color.Red;
                    success_add_item.Visible = true;
                    success_add_item.ForeColor = Color.Red;
                    success_add_item.Text = "This ID is already in use";
                    return;
                }
                else
                {
                    txtEmpID.BorderColor = Color.Black;

                }
            }else if(txtEmpID.Text.Length < 5){
                
                txtEmpID.BorderColor = Color.Red;
                success_add_item.Visible = true;
                success_add_item.ForeColor = Color.Red;
                success_add_item.Text = "ID must be 5 characters long.";
                return;
            }
            else
            {
                txtEmpID.BorderColor = Color.Black;
            }
        }

        protected void txtnic_no_TextChanged(object sender, EventArgs e)
        {
            if(txtnic_no.Text.Length < 13)
            {
                txtnic_no.BorderColor = Color.Red;
                success_add_item.Visible = true;
                success_add_item.ForeColor = Color.Red;
                success_add_item.Text = "Invalid CNIC No.";
                return;
            }
            else
            {
                txtnic_no.BorderColor = Color.Black;
            }
        }

        protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEmployee.SelectedIndex == 0)
            {
                cleardata();
                cbActive.Checked = false;
                return;
            }
            try
            {
                btn_save.Text = "Update";

                DataTable dt = new DataTable();

                SqlDataReader dr_emp = null;

                SqlParameter[] param = {
                                    new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                    new SqlParameter("@EmpCode",SqlDbType.Char),//1
                                   };

                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = ddlEmployee.SelectedItem.Value;

                SysFunc.ExecuteSP("SP_Select_DealerEmp", param, ref dr_emp);

                dt.Load(dr_emp);


                if (dt.Rows[0]["Active"].ToString() == "Y")
                {
                    cbActive.Checked = true;
                }
                else
                {
                    cbActive.Checked = false;
                }

                txtEmpID.Text = dt.Rows[0]["EmpID"].ToString().Trim();
                txtEmp_name.Text = dt.Rows[0]["EmpName"].ToString();
                txtdob.Text = dt.Rows[0]["DOB"].ToString();
                txtcontact_no.Text = dt.Rows[0]["ContactNo"].ToString();
                txtnic_no.Text = dt.Rows[0]["CNIC"].ToString();
                txtjoining_date.Text = dt.Rows[0]["JoiningDate"].ToString();
                txtleaving_date.Text = dt.Rows[0]["LeavingDate"].ToString();
                ddldesig_code.SelectedValue = dt.Rows[0]["DesigCode"].ToString();
                ddldep_code.SelectedValue = dt.Rows[0]["DeptCode"].ToString();
                txtexp.Text = dt.Rows[0]["Experience"].ToString();
                txtremarks.Text = dt.Rows[0]["Remarks"].ToString();
                txtEmail.Text = dt.Rows[0]["Email"].ToString();
                if (!string.IsNullOrEmpty(dt.Rows[0]["Picture"].ToString())) {
                Image1.ImageUrl = dt.Rows[0]["Picture"].ToString();
                }else
                {
                    Image1.ImageUrl = "~/Images/BlankProfile.jpg";
                }

            }
            catch (Exception ex)
            {
                success_add_item.Visible = true;
                success_add_item.ForeColor = Color.Red;
                success_add_item.Text = ex.Message;
            }
        }

        protected void OnClick_btnUpload(object sender, EventArgs e)
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
                FileUpload1.SaveAs(Server.MapPath("../../" + imgPath));
                Image1.ImageUrl = "~/" + imgPath;
                Session["EmployeeImagePath"] = Image1.ImageUrl;
            }
            else
            {
                success_add_item.Visible = true;
                success_add_item.ForeColor = System.Drawing.Color.Red;
                success_add_item.Text = "Only images (.jpg, .png, .gif and .bmp) can be uploaded";
            }

        }
    }
}