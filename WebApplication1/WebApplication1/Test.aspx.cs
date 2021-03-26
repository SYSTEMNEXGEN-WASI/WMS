using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DXBMS
{
    public partial class Test : System.Web.UI.Page
    {
        clsLookUp clslook = new clsLookUp();
        SysFunctions myFunc = new SysFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Session["LookUpData"].ToString()))
                {
                   
                }
            }
            Session["LookUpData"] = string.Empty;
        }

        protected void btnLookup_Click(object sender, EventArgs e)
        {
            DataTable dtSMSApplic = new DataTable();
            DataTable dtQuery = new DataTable();
            DataTable dtMessage = new DataTable();

            string DealerCode = "00249";

            string squery = "Select * from BMS_SMSApplicable where IsActive = 'Y'";
            dtSMSApplic = myFunc.GetData(squery);

            DataRow[] dr = dtSMSApplic.Select();

            for (int j = 0; j < dr.Length; j++)
            {
                string squery1 = dtSMSApplic.Rows[j]["squery"].ToString().Trim() + "and A.DealerCode = '"+ DealerCode + "'";
                dtQuery = myFunc.GetData(squery1);

                DataRow[] drr = dtQuery.Select();
                for (int i = 0; i < drr.Length; i++)
                {

                    SqlParameter[] param3 =
                                {
                                new SqlParameter("@DealerCode",SqlDbType.Char,5),
                                new SqlParameter("@DMSRefNo",SqlDbType.Char,8),
                                new SqlParameter("@CustomerCode",SqlDbType.VarChar,8),
                                new SqlParameter("@CustomerDes",SqlDbType.VarChar,50),
                                new SqlParameter("@Module",SqlDbType.VarChar,10),
                                new SqlParameter("@ModuleType",SqlDbType.VarChar,10),
                                new SqlParameter("@SMSApplicCode",SqlDbType.Char,5),
                            };


                    if (dtQuery.Rows[i]["DMSRefNo"].ToString() != "")
                    {
                        param3[0].Value = Session["DealerCode"].ToString();
                        param3[1].Value = dtQuery.Rows[i]["DMSRefNo"].ToString();
                        param3[2].Value = dtQuery.Rows[i]["CustomerCode"].ToString();
                        param3[3].Value = dtQuery.Rows[i]["CustomerDes"].ToString();
                        param3[4].Value = dtQuery.Rows[i]["Module"].ToString();
                        param3[5].Value = dtQuery.Rows[i]["ModuleType"].ToString();
                        param3[6].Value = dtSMSApplic.Rows[j]["SMSApplicCode"].ToString();

                        myFunc.ExecuteSP_NonQuery("SP_Insert_BMS_SMSTransMaster", param3);
                    }


                    
                }

                //string query3 = "exec SP_SMSMessage '" + Session["DealerCode"].ToString() + "' , '" + dtSMSApplic.Rows[j]["SMSApplicCode"].ToString().Trim() + "'";
                //string AutoNum = "";
                //dtMessage = myFunc.GetData(query3);

                //drr = dtMessage.Select();
                //for (int i = 0; i < drr.Length; i++)
                //{
                //    string username = "03214454247";
                //    string password = "m12345";
                //    string from = "MasterMotor";

                //    if (dtMessage.Rows[i]["CellNo"].ToString().StartsWith("0"))
                //    {
                //        AutoNum = "92" + dtMessage.Rows[i]["CellNo"].ToString().Substring(dtMessage.Rows[i]["CellNo"].ToString().Length - (dtMessage.Rows[i]["CellNo"].ToString().Length - 1));
                //    }


                //    string to = AutoNum;
                //    string message = dtMessage.Rows[i]["Message"].ToString();


                //    //string urlString = string.Format("http://weblogin.premiumsms.pk/sendsms_url.html?Username={0}&Password={1}&From={2}&To={3}&Message={4}", username, password, from, to, message);
                //    //Uri URI = new Uri(urlString);
                //    //WebClient wc = new WebClient();
                //    //string res = wc.DownloadString(URI);

                //    string qry = "Update BMS_SMSTransMaster set SMSText = '" + message + "' ,"+
                //        " SMSApplicCode = '" + dtSMSApplic.Rows[j]["SMSApplicCode"].ToString() + "' ,"+
                //        " SMSTo = '" + to + "', SMSFrom = '" + from + "' , "+
                //        " SendDate ='" + myFunc.SaveDate(DateTime.Now.ToString("dd/MM/yyyy")) + "',"+
                //        " where SMSTransID = '"+ dtMessage.Rows[i]["SMSTransID"].ToString() + "'";

                //    myFunc.ExecuteQuery_NonQuery(qry);
                //}
            }           
        }
    }
}