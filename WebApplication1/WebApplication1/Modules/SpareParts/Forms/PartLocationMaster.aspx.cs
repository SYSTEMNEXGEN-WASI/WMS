using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.ApplicationBlocks.Data;
using CrystalDecisions.Shared;
using CConn;
using DXBMS.Data;


namespace DXBMS.Modules.SpareParts.Forms
{
    public partial class PartLocationMaster : System.Web.UI.Page
    {
        MainBLL objMBLL = new MainBLL();
        Transaction ObjTrans = new Transaction();
        SysFunctions SysFunc = new SysFunctions();
        SecurityBll sec = new SecurityBll();
        //string LoginID = "", DealerCode = "", UserID = "", StrMachineIP = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserName"] == null)
            {
                Response.Redirect("~/login.aspx");

            }
            if (!sec.UserRight("2540", "001"))
            {
                Response.Redirect("~/Test.aspx");
            }

            //LoginID = SessionInformation.LoginID;
            //DealerCode = SessionInformation.DealerCode;
            //UserID = SessionInformation.UserName;
            //StrMachineIP = SessionInformation.MachineIP;
            if ( Page .IsPostBack == false)
            {
                Load_ddlLocCode();
            }

        }
        private void Load_ddlLocCode()
        {

            string WhereQuery = "DealerCode = '" + Session["DealerCode"].ToString() + "'";

            string[] Columns = new string[] { "PartsLocCode", "PartsLocDesc" };
            SysFunc.GetMultiColumnsDDL(ddlLocCode, Columns, "PartsLocation", WhereQuery, "PartsLocCode", "", false, false);

            //SqlParameter[] LocMaster_param = {                                            
            //    new SqlParameter("@DealerCode",SqlDbType.Char,5)};
            //LocMaster_param[0].Value = Session["DealerCode"].ToString();
            //objMBLL.FillDrp_SP(ddlLocCode, "sp_Load_PartLocCode", "PartsLocCode", "PartsLocDesc", LocMaster_param, true, "--Select--", false, "");
        }

        private void Load_ddlSubLocCode()
        {
            SqlParameter[] LocMaster_param = {                                            
                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                             new SqlParameter("@PartsLocCode",SqlDbType.Char,2)};
            LocMaster_param[0].Value = Session["DealerCode"].ToString();
            LocMaster_param[1].Value = ddlLocCode.SelectedValue.ToString();
            objMBLL.FillDrp_SP(ddlSubLocCode, "sp_Load_PartsSubLocation", "PartsSubLocCode", "PartsSubLocDesc", LocMaster_param, true, "--Select--", false, "");
        }
        

            


        private void GVSubLocFill()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = {
                                      new SqlParameter("@DealerCode",SqlDbType.Char),
                                      new SqlParameter("@PartsLocCode",SqlDbType.VarChar)
                                   };
                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = txtPartLocationCode.Text.Trim();

                ds = SysFunc.SP_Spare_Get_PartsSubLocation(param);

                dtg_PartLocation.DataSource = ds.Tables[0];
                dtg_PartLocation.DataBind();
            }
            catch (Exception ex)
            {
                LbErr.Text = ex.Message;
            }
        }
        private void GVTransLocFill()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = {
                                      new SqlParameter("@DealerCode",SqlDbType.Char),
                                      new SqlParameter("@PartsLocCode",SqlDbType.VarChar),
                                      new SqlParameter("@PartsSubLocCode",SqlDbType.VarChar)
                                      
                                   };
                param[0].Value = Session["DealerCode"].ToString();
                param[1].Value = txtPartLocationCode.Text.Trim();
                param[2].Value = txtSubLocationCode.Text.Trim();

                ds = SysFunc.SP_Spare_Get_PartsTransLocation(param);

                GVTransLocation.DataSource = ds.Tables[0];
                GVTransLocation.DataBind();

            }
            catch (Exception ex)
            {
                LbErr.Text = ex.Message;
            }
        }



       

        protected void BtnSavePartLocation_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2540", "004"))
            {
                Response.Redirect("~/Test.aspx");
            }
            string StrPartLocationCode = "", StrPartLocationDesc = "";
            TextBox[] textBoxes = { txtPartLocationDesc };
            if (!MasterValidation(textBoxes))
            {
                return;
            }
            try
            {
                
                if (txtPartLocationCode.Text.Trim() == string.Empty)
                {
                    StrPartLocationCode = SysFunc.GetNewMaxID("PartsLocation", "PartsLocCode", 2,Session ["DealerCode"].ToString());
                }
                else
                {
                    if (!sec.UserRight("2540", "003"))
                    {
                        Response.Redirect("~/Test.aspx");
                    }
                    StrPartLocationCode = txtPartLocationCode.Text.Trim();
                }

                StrPartLocationDesc = txtPartLocationDesc.Text.Trim();


                SqlParameter[] param = {
                                     new SqlParameter("@DealerCode",Session["DealerCode"].ToString()),
                                     new SqlParameter("@partsLocCode",StrPartLocationCode),
                                     new SqlParameter("@partsLocDesc",StrPartLocationDesc),
                                     new SqlParameter("@UpdUser",Session["UserName"].ToString()),
                                     new SqlParameter("@Updtem",GlobalVar.mUserIPAddress)
                                     
                                   };

                //ObjDLL.SP_Spare_Insert_partslocation(param);
                if (SysFunc.ExecuteSP_NonQuery("SP_Spare_Insert_partslocation", param))
                {
                    SysFunc.UserMsg(LbErr, Color.Green, "Location saved successfully Last Code is :" + StrPartLocationCode, txtPartLocationCode);
                    
                    Load_ddlLocCode();
                    ddlLocCode.SelectedValue = StrPartLocationCode;
                    ClearSubLoc();
                    txtAutoPartLocCode.Text = ddlLocCode.SelectedValue.ToString();
                    txtPartLocationCode.Text = ddlLocCode.SelectedValue.ToString();

                }
                else
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Location not save try again", txtPartLocationCode);
                }
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message, txtPartLocationCode);
            }
        }


        protected void BtnPartSubLocations_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2540", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            string StrPartLocationCode = "", StrPartsSubLocCode = "", StrPartsSubLocDec = "";
            TextBox[] textBoxes = { txtSubLocationCode, txtSubLocationDesc, txtPartLocationCode };
            if (!MasterValidation(textBoxes))
            {
                return;
            }
            try
            {

                if (ddlSubLocCode.Items.Count > 0)
                {
                    foreach (ListItem li in ddlSubLocCode.Items)
                    {
                        if (li.Value.Trim() == txtSubLocationCode.Text)
                        {
                            SysFunc.UserMsg(LbErr, Color.Red, "This location code already exist in the list please enter uniqe code.");
                            return;
                        }
                    }
                }
                StrPartLocationCode = txtPartLocationCode.Text.Trim();
                StrPartsSubLocCode = txtSubLocationCode.Text.Trim();
                StrPartsSubLocDec = txtSubLocationDesc.Text.Trim();


                SqlParameter[] param = {
                                     new SqlParameter("@DealerCode",Session["DealerCode"].ToString()),//0
                                     new SqlParameter("@partsLocCode",StrPartLocationCode),//1
                                     new SqlParameter("@PartsSubLocCode",StrPartsSubLocCode),//2
                                     new SqlParameter("@PartsSubLocDesc",StrPartsSubLocDec),//3
                                     new SqlParameter("@UpdUser",Session["UserName"].ToString()),//4
                                     new SqlParameter("@Updtem",GlobalVar.mUserIPAddress)//7
                                   };

                //ObjDLL.sp_Spare_Insert_Subpartslocation(param);
                if (SysFunc.ExecuteSP_NonQuery("sp_Spare_Insert_Subpartslocation", param))
                {
                    SysFunc.UserMsg(LbErr, Color.Green, "Sub Location saved successfully Last Code is :" + StrPartsSubLocCode, txtSubLocationCode);
                    Load_ddlSubLocCode();
                   
                            ddlSubLocCode.SelectedValue = txtSubLocationCode.Text.Trim();
                       
                    txtAutoPartsSubLocCode.Text = ddlSubLocCode.SelectedValue.ToString();
                    //GVSubLocFill();
                }
                else
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Sub Location not save try again", txtPartLocationCode);
                }

            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message, txtPartLocationCode);
            }
        }

        //protected void ImgLookupPartLocCode_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        ViewState["lookupid"] = 47;
        //        ViewState["ixd1"] = 1;
        //        ViewState["ixd2"] = 2;

        //        ObjLookup.LU_Get_PartsLocation(ImgLookupPartLocCode, ViewState["lookupid"].ToString(), "", "../../../../");

        //        LbErr.Text = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        LbErr.Text = ex.Message;

        //    }
        //}



        //protected void ImgLookupSubLocCode_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        ////        if (txtPartLocationCode.Text == "")
        ////        {
        ////            LbErr.Text = "Please enter part location";
        ////            return;
        ////        }
        ////        else
        ////        {
        ////            LbErr.Text = "";
        ////        }

        //        ViewState["lookupid"] = 50;
        //        ViewState["ixd1"] = 1;
        //        ViewState["ixd2"] = 2;

        //        ObjLookup.LU_Get_SubPartsLocation(ImgLookupSubLocCode, ViewState["lookupid"].ToString(), "", "../../../../", txtPartLocationCode.Text.Trim());

        //        LbErr.Text = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        LbErr.Text = ex.Message;

        //    }
        //}
        protected void BtnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton BtnLnk = (LinkButton)sender;
                GridViewRow rowGv = (GridViewRow)BtnLnk.Parent.Parent;
                int rowGvIndex = rowGv.RowIndex;

                Label LbPartsSubLocCode = (Label)BtnLnk.FindControl("LbPartsSubLocCode");
                txtSubLocationCode.Text = LbPartsSubLocCode.Text.Trim();

                Label LbPartsSubLocDesc = (Label)BtnLnk.FindControl("LbPartsSubLocDesc");
                txtSubLocationDesc.Text = LbPartsSubLocDesc.Text.Trim();

                txtAutoPartsSubLocCode.Text = LbPartsSubLocCode.Text.Trim();
                GVTransLocFill();
                txtPartsTransLocCode.Text = string.Empty;
                txtPartsTransLocDesc.Text = string.Empty;

            }
            catch (Exception ex)
            {
                LbErr.Text = ex.Message;
            }
        }
        //protected void ImgLookupTransLocCode_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        if (txtPartLocationCode.Text == "")
        //        {
        //            LbErr.Text = "Please enter part location";
        //            return;
        //        }
        //        else
        //        {
        //            LbErr.Text = "";
        //        }

        //        if (txtSubLocationCode.Text == "")
        //        {
        //            LbErr.Text = "Please enter sub part location";
        //            return;
        //        }
        //        else
        //        {
        //            LbErr.Text = "";
        //        }

        //        ViewState["lookupid"] = 51;
        //        ViewState["ixd1"] = 1;
        //        ViewState["ixd2"] = 3;
        //        ViewState["ixd3"] = 4;
        //        ObjLookup.LU_Get_TransPartsLocation(ImgLookupSubLocCode, ViewState["lookupid"].ToString(), "", "../../../../",
        //            txtPartLocationCode.Text.Trim(), txtSubLocationCode.Text.Trim());

        //        LbErr.Text = "";

        //    }
        //    catch (Exception ex)
        //    {
        //        LbErr.Text = ex.Message;
        //    }
        //}

        protected void BtnTransLca_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2540", "002"))
            {
                Response.Redirect("~/Test.aspx");
            }
            string StrPartLocationCode = "", StrPartsSubLocCode = "",
                StrPartsSubLocDec = "", StrPartsTransLocDesc = "", StrPartsTransLocCode = "";
            try
            {
                StrPartLocationCode = txtPartLocationCode.Text.Trim();
                StrPartsSubLocCode = txtSubLocationCode.Text.Trim();
                StrPartsSubLocDec = txtSubLocationDesc.Text.Trim();
                if (txtPartsTransLocCode.Text.Trim() == string.Empty)
                {
                    StrPartsTransLocCode = SysFunc.GetNewMaxID("PartsTransLocation", "Right(PartsTransLocCode, 3)", 3,Session["DealerCode"].ToString());
                }
                else
                {
                    if (!sec.UserRight("2540", "003"))
                    {
                        Response.Redirect("~/Test.aspx");
                    }
                    StrPartsTransLocCode = txtPartsTransLocCode.Text;
                }
                StrPartsTransLocCode = StrPartLocationCode + StrPartsSubLocCode + StrPartsTransLocCode;
                StrPartsTransLocDesc = txtPartsTransLocDesc.Text;





                #region Form Check
                if (StrPartLocationCode == "")
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Sub location code can not be blank");
                    
                    return;
                }
                else
                {
                    LbErr.Text = "";
                }

                if (StrPartsSubLocDec == "")
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Sub location description can not be blank");
                    return;
                }
                else
                {
                    LbErr.Text = "";
                }

                
                if (txtAutoPartsSubLocCode.Text.Trim() == string.Empty)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Sub location Code should not be blank");
                    
                    return;
                }
                else
                {
                    LbErr.Text = "";
                }

                if (txtAutoPartLocCode.Text.Trim() == string.Empty)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Part location Code should not be blank");
                    return;
                }
                     else
                {
                    LbErr.Text = "";
                }

               
                LbErr.Text = "";
                #endregion

                SqlParameter[] param = {
                                     new SqlParameter("@DealerCode",Session["DealerCode"].ToString()),//0
                                     new SqlParameter("@partsLocCode",StrPartLocationCode),//1
                                     new SqlParameter("@PartsSubLocCode",StrPartsSubLocCode),//2
                                     new SqlParameter("@PartsTransLocCode",StrPartsTransLocCode),//3
                                     new SqlParameter("@PartsTransLocDesc",StrPartsTransLocDesc),//4
                                     new SqlParameter("@UpdUser",Session["UserName"].ToString()),//5
                                     new SqlParameter("@UpdTerm",GlobalVar.mUserIPAddress)//8
                                   };
                SysFunc.SP_Spare_Insert_PartsTransLocation(param);

                GVTransLocFill();
                //ClearPartTransLoc();
                txtPartsTransLocCode.Text = string.Empty;
                txtPartsTransLocDesc.Text = string.Empty;
            }
            catch (Exception ex)
            {
                LbErr.Text = ex.Message;
            }
        }
        public bool MasterValidation(TextBox[] textBoxes)
        {
            bool isValid = true;

            for (int i = 0; i < textBoxes.Length; i++)
            {
                if (textBoxes[i].Text == "")
                {
                    isValid = false;
                    textBoxes[i].BorderColor = System.Drawing.Color.Red;
                    SetFocus(textBoxes[i]);
                    SysFunc.UserMsg(LbErr, Color.Red, "Location is not Selected", textBoxes[i]);
                    break;
                }
                else { textBoxes[i].BorderColor = System.Drawing.ColorTranslator.FromHtml("#1A6297"); }
            }
            return isValid;
        }
        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2540", "004"))
            {
                Response.Redirect("~/Test.aspx");
            }
            string StrPartsTransLocCode = "";
            bool Check = false;
            TextBox[] textBoxes = { txtAutoPartLocCode, txtAutoPartsSubLocCode, txtPartsTransLocCode };
            if (!MasterValidation(textBoxes))
            {
                return;
            }
            try
            {
                StrPartsTransLocCode = txtAutoPartLocCode.Text + txtAutoPartsSubLocCode.Text + txtPartsTransLocCode.Text;
                if (SysFunc.CodeExists("ItemStock", "PartsTransLocCode", StrPartsTransLocCode))
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Can not delete location its used in ItemStock", txtPartsTransLocCode);
                    return;
                }
                if (SysFunc.CodeExists("ItemStatus", "PartsTransLocCode", StrPartsTransLocCode))
                {

                    SysFunc.UserMsg(LbErr, Color.Red, "Can not delete location its used in ItemStatus", txtPartsTransLocCode);
                    return;
                }
                if (SysFunc.CodeExists("GRNDetail", "PartsTransLocCode", StrPartsTransLocCode))
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Can not delete location its used in GRNDetail", txtPartsTransLocCode);
                    return;
                }

                SqlParameter[] param = {
                                     new SqlParameter("@DealerCode",Session["DealerCode"].ToString()),//0
                                     new SqlParameter("@PartsTransLocCode",StrPartsTransLocCode)//1
                                    };

                Check = SysFunc.SP_Spare_Delete_PartsTransLocation(param);
                GVTransLocFill();
                txtPartsTransLocCode.Text = string.Empty;
                txtPartsTransLocDesc.Text = string.Empty;
            }
            catch (Exception ex)
            {
                
                SysFunc.UserMsg(LbErr, Color.Red, "Error :" + ex.Message);
            }
        }
        protected void BtnTransSelect_Click(object sender, EventArgs e)
        {
            if (txtAutoPartsSubLocCode.Text.Trim() == string.Empty)
            {
                SysFunc.UserMsg(LbErr, Color.Red, "Sub location Code should not be blank" );
                return;
            }
            if (txtAutoPartLocCode.Text.Trim() == string.Empty)
            {
                SysFunc.UserMsg(LbErr, Color.Red, "Part location Code should not be blank");
                return;
            }
            else
            {
                LbErr.Text = "";
            }
            LinkButton btn = sender as LinkButton;
            TableCell tc = btn.Parent as TableCell;
            GridViewRow gvr = tc.Parent as GridViewRow;

            txtPartsTransLocCode.Text = gvr.Cells[1].Text.Substring(gvr.Cells[1].Text.Length - 3);
            txtPartsTransLocDesc.Text = gvr.Cells[2].Text;

        }
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            ClearPartTransLoc();
            GVTransLocFill();
        }
        private void ClearPartTransLoc()
        {
            txtAutoPartLocCode.Text = string.Empty;
            txtAutoPartsSubLocCode.Text = string.Empty;
            txtPartsTransLocCode.Text = string.Empty;
            txtPartsTransLocDesc.Text = string.Empty;
        }

        protected void BtnDeletePartSubLocations_Click(object sender, EventArgs e)
        {
            if (!sec.UserRight("2540", "004"))
            {
                Response.Redirect("~/Test.aspx");
            }
            try
            {
                TextBox[] textBoxes = { txtSubLocationCode };
                if (!MasterValidation(textBoxes))
                {
                    return;
                }

                if (SysFunc.CodeExists("PartsTransLocation", "PartsSubLocCode", txtSubLocationCode.Text.Trim()))
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Can not delete location its used in PartsTransLocation", txtSubLocationCode);
                    return;
                }
                SqlParameter[] param = {
                                     new SqlParameter("@DealerCode",Session["DealerCode"].ToString()),//0
                                     new SqlParameter("@PartsSubLocCode",txtSubLocationCode.Text.Trim())//1
                                    };

                if (SysFunc.ExecuteSP_NonQuery("SP_Spare_Delete_PartsSubLocation", param))
                {
                    SysFunc.UserMsg(LbErr, Color.Green, "Sub Location Deleted successfully", txtSubLocationCode);
                    GVSubLocFill();
                    txtSubLocationCode.Text = string.Empty;
                    txtSubLocationDesc.Text = string.Empty;
                    txtAutoPartsSubLocCode.Text = string.Empty;
                }
                else
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Sub Location not Deleted try again", txtPartLocationCode);
                }
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message, txtPartLocationCode);
            }
        }
        protected void BtnDeletePartLocation_Click(object sender, EventArgs e)
        {

            try
            {
                if (ddlLocCode.SelectedIndex == 0)
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Select location first", null);
                    return;
                }

                if (SysFunc.CodeExists("PartsSubLocation", "PartsLocCode", txtPartLocationCode.Text.Trim()))
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Can not delete location its used in PartsSubLocation", txtPartsTransLocCode);
                    return;
                }
                SqlParameter[] param = {
                                     new SqlParameter("@DealerCode",Session["DealerCode"].ToString()),//0
                                     new SqlParameter("@PartsLocCode",txtPartLocationCode.Text.Trim())//1
                                    };

                if (SysFunc.ExecuteSP_NonQuery("SP_Spare_Delete_PartsLocation", param))
                {
                    SysFunc.UserMsg(LbErr, Color.Green, "Location Deleted successfully", txtPartLocationCode);
                    Load_ddlLocCode();
                    txtPartLocationCode.Text = string.Empty;
                    txtPartLocationDesc.Text = string.Empty;
                    txtAutoPartLocCode.Text = string.Empty;
                    ddlLocCode.SelectedIndex = 0;
                }
                else
                {
                    SysFunc.UserMsg(LbErr, Color.Red, "Location not Deleted try again", txtPartLocationCode);
                }
            }
            catch (Exception ex)
            {
                SysFunc.UserMsg(LbErr, Color.Red, ex.Message, txtPartLocationCode);
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            ddlLocCode.SelectedIndex = 0;
            ddlSubLocCode.Items.Clear();
            txtPartLocationCode.Text = string.Empty;
            txtPartLocationDesc.Text = string.Empty;
            txtSubLocationCode.Text = string.Empty;
            txtSubLocationDesc.Text = string.Empty;
            ClearPartTransLoc();
            LbErr.Text = string.Empty;
            //GVSubLocFill();
            GVTransLocFill();

        }
        private void ClearSubLoc()
        {
            Load_ddlSubLocCode();
            txtSubLocationCode.Text = string.Empty;
            txtSubLocationDesc.Text = string.Empty;
            ClearPartTransLoc();
            GVTransLocFill();
        }
        protected void txtPartLocationDesc_TextChanged(object sender, EventArgs e)
        {

        }

        protected void ddlLocCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLocCode.SelectedIndex == 0)
            {
                return;
            }
            txtPartLocationCode.Text = ddlLocCode.SelectedValue.ToString();

            string code = ddlLocCode.SelectedItem.Text;
            string[] split = code.Split('|');

            txtPartLocationDesc.Text = split[2].Trim();
            if(txtAutoPartLocCode.BorderColor == Color.Red)
            {
                txtAutoPartLocCode.BorderColor = Color.Black;
            }
            txtAutoPartLocCode.Text = ddlLocCode.SelectedValue.ToString();
            Load_ddlSubLocCode();
        }

        protected void ddlSubLocCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSubLocCode.SelectedIndex == 0)
            {
                return;
            }
            if (txtSubLocationCode.BorderColor == Color.Red)
            {
                txtSubLocationCode.BorderColor = Color.Black;
            }
            txtSubLocationCode.Text = ddlSubLocCode.SelectedValue.ToString();
            txtSubLocationDesc.Text = ddlSubLocCode.SelectedItem.Text;
            if (txtAutoPartsSubLocCode.BorderColor == Color.Red)
            {
                txtAutoPartsSubLocCode.BorderColor = Color.Black;
            }
            txtAutoPartsSubLocCode.Text = ddlSubLocCode.SelectedValue.ToString();
            GVTransLocFill();
        }

        protected void ddlPartTransLocCode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}