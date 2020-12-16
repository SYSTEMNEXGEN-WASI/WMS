using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DXBMS.Modules.SpareParts
{
    public partial class SparePartsLocations : System.Web.UI.Page
    {
        SysFunctions myFunc = new SysFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/Main/login.aspx");
              
            }
        }

      

        protected void txtLocCode_TextChanged(object sender, EventArgs e)
        {
            
            string lcode = txtLocCode.Text;

            if (lcode == "" || lcode=="00")
            {
                txtLocDesc.Text = "";
                txtLocDesc.Enabled = false;
                txtSubLocCode_TextChanged(txtSubLocCode, EventArgs.Empty);

                btnSaveMainLoc.Enabled = false;
                btnDeleteMain.Enabled = false;
                return;
            }
            if (myFunc.isLocCodeExist(lcode))
            {
                txtLocDesc.Enabled = true;
                txtLocDesc.Text = myFunc.GetLocDesc(lcode);
                btnSubLocLookup.Enabled = true;
                txtSubLocCode.Enabled = true;

                
                btnSaveMainLoc.Enabled = true;
                btnDeleteMain.Enabled = true;

            }
            else
            {

                txtLocDesc.Enabled = true;
                txtLocDesc.Text = "";
                txtLocDesc.Focus();
                
                btnSubLocLookup.Enabled = false;
                txtSubLocCode.Enabled = true;
                txtSubCodeDesc.Enabled = true;
                btnSaveSubLoc.Visible = true;
                btnSaveSubLoc.Enabled = true;

                btnSaveMainLoc.Enabled = true;
                btnDeleteMain.Enabled = false;
               

            }

            txtSubLocCode_TextChanged(txtSubLocCode, EventArgs.Empty);
        }

        protected void btnLocCodeLookup_Click(object sender, EventArgs e)
        {
            PopupControlLocCode.ShowOnPageLoad = true;
            GridViewLocCode.DataBind();
        }

        protected void btnLocCancel_Click(object sender, EventArgs e)
        {
            PopupControlLocCode.ShowOnPageLoad = false;
        }

        protected void btnLocSelect_Click(object sender, EventArgs e)
        {
            txtLocCode.Text = GridViewLocCode.GetRowValues(GridViewLocCode.FocusedRowIndex, "PartsLocCode").ToString();
            txtLocDesc.Text = GridViewLocCode.GetRowValues(GridViewLocCode.FocusedRowIndex, "PartsLocDesc").ToString();
            txtLocCode_TextChanged(txtLocCode, EventArgs.Empty);
            PopupControlLocCode.ShowOnPageLoad = false;
            
        }

        

        protected void txtSubLocCode_TextChanged(object sender, EventArgs e)
        {
            string lcode = txtSubLocCode.Text;
            if (lcode == "")
            {
                txtSubCodeDesc.Text = "";
                txtSubCodeDesc.Enabled = false;
                btnSaveSubLoc.Enabled = false;
                btnDeleteSubLoc.Enabled = false;
                FormLayoutDetail.Visible = false;
                btnSelectTransCode.Visible = false;
                GridViewTransLocs.Visible = false;
                return;
            }
            if (myFunc.isSubLocCodeExist(txtLocCode.Text, lcode))
            {
                txtSubCodeDesc.Enabled = true;
                txtSubCodeDesc.Text = myFunc.GetSubLocDesc(txtLocCode.Text, lcode);
                btnSaveSubLoc.Enabled = true;
                btnDeleteSubLoc.Enabled = true;
                txtTransLocCode.Text = txtLocCode.Text.ToUpper();
                txtTransSubLocCode.Text = txtSubLocCode.Text.ToUpper();
                FormLayoutDetail.Visible = true;
                GridViewTransLocs.Visible = true;

                SqlDataSourceTransLocations.SelectCommand = "select * from PartsTransLocation where PartsLocCode='" + txtLocCode.Text + "' and PartsSubLocCode='"+txtSubLocCode.Text+"'";
                SqlDataSourceTransLocations.Select(DataSourceSelectArguments.Empty);
                GridViewTransLocs.Visible = true;
                btnSelectTransCode.Visible = true;
                GridViewTransLocs.DataBind();

                btnCreateLocation.Enabled = true;
            }
            else
            {
                txtSubCodeDesc.Enabled = true;
                txtSubCodeDesc.Text = "";
                txtSubCodeDesc.Focus();
                btnSaveSubLoc.Enabled = true;
                btnDeleteSubLoc.Enabled = false;
                FormLayoutDetail.Visible = false;
                GridViewTransLocs.Visible = false;
                btnSelectTransCode.Visible = false;
            }
        }

        protected void btnSubLocLookup_Click(object sender, EventArgs e)
        {

            if (txtLocCode.Text.Trim() == "")
            {
                lblMSG.Text = "Please select main code";
                PopupControlMSG.ShowOnPageLoad = true;
                txtLocCode.Focus();
                return;
            }

            PopupControlSubLocLookup.ShowOnPageLoad = true;
            SqlDataSourceSubLoc.SelectCommand = "select * from PartsSubLocation where PartsLocCode='" + txtLocCode.Text + "'";
            SqlDataSourceSubLoc.Select(DataSourceSelectArguments.Empty);
            GridViewSubLoc.Visible = true;
            GridViewSubLoc.DataBind();
        }

        protected void btnSubLocSelect_Click(object sender, EventArgs e)
        {
            txtSubLocCode.Text = GridViewSubLoc.GetRowValues(GridViewSubLoc.FocusedRowIndex, "PartsSubLocCode").ToString();
            txtSubCodeDesc.Text = GridViewSubLoc.GetRowValues(GridViewSubLoc.FocusedRowIndex, "PartsSubLocDesc").ToString();
            txtSubLocCode_TextChanged(txtSubLocCode, EventArgs.Empty);
            PopupControlSubLocLookup.ShowOnPageLoad = false;
        }

        protected void btnSubLocCancel_Click(object sender, EventArgs e)
        {
            PopupControlSubLocLookup.ShowOnPageLoad = false;
        }

        protected void btnSaveSubLoc_Click(object sender, EventArgs e)
        {

            string sql = "";

            if (txtSubCodeDesc.Text == "")
                {
                    lblMSG.Text = "Please enter sub description";
                    PopupControlMSG.ShowOnPageLoad = true;
                    txtSubCodeDesc.Focus();
                    return;
                }

            
            string msg = "";
            // saving sub location code
            if (!myFunc.isSubLocCodeExist(txtLocCode.Text,txtSubLocCode.Text))
            {
                sql = "insert into PartsSubLocation (DealerCode,PartsLocCode, PartsSubLocCode, PartsSubLocDesc, UpdUser, UpdDate, UpdTime, UpdTerm)";
                sql += " Values('" + this.Session["DealerCode"].ToString() + "','" + txtLocCode.Text.Trim().ToUpper() + "','" + txtSubLocCode.Text.Trim().ToUpper() + "','" + txtSubCodeDesc.Text.Trim().ToUpper() + "','";
                sql += this.Session["UserName"].ToString() + "',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,'" + Environment.MachineName + "')";
                msg = "Saved";
            }
            else
            {
                sql = "update PartsSubLocation Set PartsSubLocDesc='" + txtSubCodeDesc.Text.Trim().ToUpper().ToUpper() + "' where PartsSubLocCode='" + txtSubLocCode.Text.Trim().ToUpper() + "' and PartsLocCode='" + txtLocCode.Text.Trim().ToUpper() + "'";
                msg = "Update";
            }

            myFunc.GetData(sql);

            lblMSG.Text = "Record Has Been "+msg;
            PopupControlMSG.ShowOnPageLoad = true;

            txtSubCodeDesc.Enabled = false;

            txtSubLocCode_TextChanged(txtSubLocCode, EventArgs.Empty);

        }

        protected void btnPopOk_Click(object sender, EventArgs e)
        {
            PopupControlMSG.ShowOnPageLoad = false;
        }

        protected void btnPopCancel_Click(object sender, EventArgs e)
        {
            PopupControlMSG.ShowOnPageLoad = false;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtLocCode.Text = "";
            txtSubLocCode.Text = "";
            txtTransCode.Text = "";
            txtTransSubLocCode.Text = "";
            txtTransLocCode.Text = "";

            txtLocCode_TextChanged(txtLocCode, EventArgs.Empty);
            btnDelete.Enabled = false;
            btnSave.Enabled = false;


        }

        protected void btnClearBottom_Click(object sender, EventArgs e)
        {
            if (txtTransCode.Text.Trim() == "")
            {
                lblMSG.Text = "Please select a location";
                PopupControlMSG.ShowOnPageLoad = true;
                return;
            }

            
            string tlcode = txtTransLocCode.Text.Trim() + txtTransSubLocCode.Text.Trim() + txtTransCode.Text.Trim();
            string sql = "update PartsTransLocation set PartsTransLocDesc='" + txtTransLocDesc.Text.Trim().ToUpper() + "' where PartsTransLocCode='" + tlcode + "'";
            myFunc.GetData(sql);

            lblMSG.Text = "Location has been updated";
            PopupControlMSG.ShowOnPageLoad = true;

            //txtTransCode.Text = "";
            //txtTransLocDesc.Text = "";
            txtTransLocDesc.Enabled = true;
            btnCreateLocation.Enabled=true;
            btnDeleteLocation.Enabled=true;
            btnSelectTransCode.Visible=true;

            GridViewTransLocs.Visible = true;
            SqlDataSourceTransLocations.SelectCommand = "select * from PartsTransLocation where PartsLocCode='" + txtTransLocCode.Text.ToUpper().Trim() + "' and PartsSubLocCode='" + txtTransSubLocCode.Text.Trim().ToUpper() + "'";
            SqlDataSourceTransLocations.Select(DataSourceSelectArguments.Empty);
            GridViewTransLocs.Visible = true;
            GridViewTransLocs.DataBind();
        }

        protected void txtTransCode_TextChanged(object sender, EventArgs e)
        {
            string tlcode = txtTransLocCode.Text.Trim() + txtTransSubLocCode.Text.Trim() + txtTransCode.Text.Trim();
            if (!myFunc.isTransLocCodeExist(tlcode))
            {
                txtTransLocDesc.Text = "";
                
                txtTransLocDesc.Enabled = true;
                txtTransLocDesc.Focus();
                btnSave.Enabled = true;
                btnDelete.Enabled = false;
                //btnSaveBottom.Enabled=true;
                //btnDeleteBottom.Enabled=false;

                GridViewTransLocs.Visible = true;
                SqlDataSourceTransLocations.SelectCommand = "select * from PartsTransLocation where PartsLocCode='" + txtLocCode.Text.Trim() + "' and PartsSubLocCode='" + txtSubLocCode.Text.Trim() + "'";
                SqlDataSourceTransLocations.Select(DataSourceSelectArguments.Empty);
                GridViewTransLocs.Visible = true;
                GridViewTransLocs.DataBind();
            }
            else
            {

                txtTransLocDesc.Text = myFunc.GetTransLocDesc(tlcode);
                txtTransLocDesc.Enabled = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = true;
                
                //btnSaveBottom.Enabled = false;
                //btnDeleteBottom.Enabled = true;

                GridViewTransLocs.Visible = true;
                SqlDataSourceTransLocations.SelectCommand = "select * from PartsTransLocation where PartsTransLocCode='" + tlcode + "'";
                SqlDataSourceTransLocations.SelectCommand = "select * from PartsTransLocation where PartsLocCode='" + txtLocCode.Text.Trim() + "' and PartsSubLocCode='" + txtSubLocCode.Text.Trim() + "'";
                SqlDataSourceTransLocations.Select(DataSourceSelectArguments.Empty);
                GridViewTransLocs.Visible = true;
                GridViewTransLocs.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtTransLocDesc.Text == "")
            {
                lblMSG.Text = "Please enter description";
                PopupControlMSG.ShowOnPageLoad = true;
                txtTransLocDesc.Focus();
                return;
            }

            // saving sub location code
            string sql = "insert into PartsTransLocation (DealerCode,PartsLocCode, PartsSubLocCode,PartsTransLocCode, PartsTransLocDesc, UpdUser, UpdDate, UpdTime, UpdTerm)";
            sql += " Values('" + this.Session["DealerCode"].ToString() + "','" + txtLocCode.Text + "','" + txtSubLocCode.Text + "','"+txtTransCode.Text+"','" + txtTransLocDesc.Text + "','";
            sql += this.Session["UserName"].ToString() + "',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,'" + Environment.MachineName + "')";

            myFunc.GetData(sql);

            lblMSG.Text = "Record Has Been Saved";
            PopupControlMSG.ShowOnPageLoad = true;

            txtSubCodeDesc.Enabled = false;

            
            txtTransCode_TextChanged(txtTransLocCode, EventArgs.Empty);
        }

        protected void btnSaveMainLoc_Click(object sender, EventArgs e)
        {

            if (txtLocDesc.Text.Trim() == "")
            {
                lblMSG.Text = "Please enter description";
                PopupControlMSG.ShowOnPageLoad = true;
                txtLocDesc.Focus();
                return;
            }
            
            
            // saving main location code
            string msg = "";
            string sql = "";
            if (!myFunc.isLocCodeExist(txtLocCode.Text.Trim()))
            {
                sql = "insert into PartsLocation (DealerCode, PartsLocCode, PartsLocDesc, UpdUser, UpdDate, UpdTime, UpdTerm)";
                sql += " Values('" + this.Session["DealerCode"].ToString() + "','" + txtLocCode.Text.ToUpper().Trim() + "','" + txtLocDesc.Text.ToUpper().Trim() + "','";
                sql += this.Session["UserName"].ToString() + "',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,'" + Environment.MachineName + "')";
                msg = "Saved";
            }
            else
            {
                sql = "update PartsLocation set PartsLocDesc='" + txtLocDesc.Text.ToUpper().Trim() + "' where PartsLocCode='" + txtLocCode.Text.Trim() + "'";
                msg = "Update";
            }

            myFunc.GetData(sql);
           

            lblMSG.Text = "Record Has Been "+msg;
            PopupControlMSG.ShowOnPageLoad = true;

            txtLocDesc.Enabled = false;
            txtLocCode_TextChanged(txtLocCode, EventArgs.Empty);

            
        }

        protected void btnDeleteMain_Click(object sender, EventArgs e)
        {
            // delete 

            if(myFunc.CodeExists("PartsSubLocation","PartsLocCode",txtLocCode.Text.Trim(),this.Session["DealerCode"].ToString()))
            {
                 lblMSG.Text = "Can't delete , this code exist in sub location .";
                 PopupControlMSG.ShowOnPageLoad = true;
                return;
            }
            string sql = "delete from PartsLocation where PartsLocCode='"+txtLocCode.Text.Trim()+"'";
            myFunc.GetData(sql);

            lblMSG.Text = "Location has been deleted";
            PopupControlMSG.ShowOnPageLoad = true;

            txtLocCode.Text ="";
            txtLocCode_TextChanged(txtLocCode,EventArgs.Empty);
        }

        protected void btnDeleteSubLoc_Click(object sender, EventArgs e)
        {
            // delete 

            if (myFunc.CodeExists("PartsTransLocation", "PartsSubLocCode", txtSubLocCode.Text, this.Session["DealerCode"].ToString()))
            {
                lblMSG.Text = "Can't delete , this code exist in trans location .";
                PopupControlMSG.ShowOnPageLoad = true;
                return;
            }
            string sql = "delete from PartsSubLocation where PartsSubLocCode='" + txtSubCodeDesc.Text + "'";
            myFunc.GetData(sql);

            lblMSG.Text = "Sub Location has been deleted";
            PopupControlMSG.ShowOnPageLoad = true;

            txtSubLocCode.Text = "";
            txtSubLocCode_TextChanged(txtSubLocCode, EventArgs.Empty);
        }

        protected void GridViewTransLocs_FocusedRowChanged(object sender, EventArgs e)
        {
            if (GridViewTransLocs.FocusedRowIndex < 0)
            {
                return;
            }
            if (GridViewTransLocs.Selection.Count == 0)
            {
                return;
            }
            string loctranscode = GridViewTransLocs.GetRowValues(GridViewTransLocs.FocusedRowIndex, "PartsTransLocCode").ToString();
            string locmain = GridViewTransLocs.GetRowValues(GridViewTransLocs.FocusedRowIndex, "PartsLocCode").ToString();
            string locsub = GridViewTransLocs.GetRowValues(GridViewTransLocs.FocusedRowIndex, "PartsSubLocCode").ToString();

            txtTransCode.Text = loctranscode.Replace(locmain + locsub, "");
        }

        protected void btnCreateLocation_Click(object sender, EventArgs e)
        {

            if (txtTransLocDesc.Text.Trim() == "")
            {
                lblMSG.Text = "Please enter description";
                PopupControlMSG.ShowOnPageLoad = true;
                txtTransLocDesc.Focus();
                return;
            }

            string lcode = txtTransLocCode.Text.Trim().ToUpper() + txtTransSubLocCode.Text.Trim().ToUpper();
            string sql = "select MAX(REPLACE(PartsTransLocCode,'" + lcode + "','')) as MV  from PartsTransLocation where LEFT(PartsTransLocCode,"+lcode.Length+")='" + lcode + "'";
            DataTable dt = myFunc.GetData(sql);
            int nno = 1;
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["MV"].ToString() != "")
                {
                    nno = Convert.ToInt16(dt.Rows[0]["MV"].ToString()) + 1;
                }
                else
                {
                    nno = 1;
                }
            }
            string NewCode = nno.ToString("000");

            // create new code
            sql = "INSERT INTO PartsTransLocation (DealerCode,PartsLocCode,PartsSubLocCode,PartsTransLocCode,PartsTransLocDesc,UpdUser,UpdDate,UpdTime,UpdTerm)";
            sql += " VALUES('" + this.Session["DealerCode"].ToString() + "','" + txtTransLocCode.Text + "','" + txtTransSubLocCode.Text + "','" + lcode + NewCode;
            sql += "','" + txtTransLocDesc.Text.ToUpper().Trim() + "','" + this.Session["UserName"].ToString() + "',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,'" + Environment.MachineName + "')";

            myFunc.GetData(sql);

            txtTransCode.Text = NewCode;

            txtTransCode_TextChanged(txtTransCode, EventArgs.Empty);
        }

        protected void ASPxCallbackPanel1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
           
        }

        protected void btnSelectTransCode_Click(object sender, EventArgs e)
        {
            string loctranscode = GridViewTransLocs.GetRowValues(GridViewTransLocs.FocusedRowIndex, "PartsTransLocCode").ToString();
            string locmain = GridViewTransLocs.GetRowValues(GridViewTransLocs.FocusedRowIndex, "PartsLocCode").ToString().Trim();
            string locsub = GridViewTransLocs.GetRowValues(GridViewTransLocs.FocusedRowIndex, "PartsSubLocCode").ToString().Trim();

            txtTransCode.Text = loctranscode.Replace(locmain + locsub, "");

            txtTransCode_TextChanged(txtTransCode, EventArgs.Empty);
            txtTransLocDesc.Enabled = true;

            btnDeleteLocation.Enabled = true;
        }

        protected void btnDeleteLocation_Click(object sender, EventArgs e)
        {
            // delete 

            //if (myFunc.CodeExists("PartsTransLocation", "PartsSubLocCode", txtSubLocCode.Text, this.Session["DealerCode"].ToString()))
            //{
            //    lblMSG.Text = "Can't delete , this code exist in trans location .";
            //    PopupControlMSG.ShowOnPageLoad = true;
            //    return;
            //}
            string ptcode = txtTransLocCode.Text.Trim() + txtTransSubLocCode.Text.Trim() + txtTransCode.Text.Trim();

            string sql = "delete from PartsTransLocation where PartsTransLocCode='" + ptcode + "'";
            myFunc.GetData(sql);

            lblMSG.Text = "Location has been deleted";
            PopupControlMSG.ShowOnPageLoad = true;

            txtTransCode.Text = "";
            txtTransCode_TextChanged(txtTransCode, EventArgs.Empty);
        }
        
    }
}