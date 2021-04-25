using CConn;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

namespace DXBMS.Modules.Setup
{
    
    public partial class ImportItem : System.Web.UI.Page
    {
        SysFunction sysFunc = new SysFunction();
        DataSet ds;
        Transaction ObjTrans = new Transaction();
        SqlTransaction Trans;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (this.Session["DealerCode"] == null)
            {
                Response.Redirect("~/login.aspx");

            }

            if (!IsPostBack)
            {
                //ThreadStart childthreat = new ThreadStart(BindGridview);
                //Thread child = new Thread(childthreat);

                //child.Start();
               BindGridview();


            }
        }

        private void BindGridview()
        {
           
            DataTable dt = new DataTable();

            dt = sysFunc.GetData("SP_Select_Items '"+Session["DealerCode"].ToString()+"'");

            if(dt == null)
            {
                return;
            }

            if (dt.Rows.Count > 0)
            {
                grdItem.DataSource = dt;
                grdItem.DataBind();
                grdItem.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            Session["BankDS"] = dt;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //Coneection String by default empty  
            string ConStr = "";
            //Extantion of the file upload control saving into ext because   
            //there are two types of extation .xls and .xlsx of Excel   
            HttpPostedFile postedFile = FileUpload1.PostedFile;

            if (FileUpload1.HasFile == false)
            {
                sysFunc.UserMsg(lblMessage, Color.Red, "Choose file first");
                return;
            }

            string ext = Path.GetExtension(FileUpload1.FileName).ToLower();
            //getting the path of the file   
            string path = Server.MapPath("~/Download/" + FileUpload1.FileName);
            //saving the file inside the MyFolder of the server  
            FileUpload1.SaveAs(path);
            //Label1.Text = FileUpload1.FileName + "\'s Data showing into the GridView";
            //checking that extantion is .xls or .xlsx  

            if (ext.Trim() == ".xls" || ext.Trim() == ".xlsx")
            {
                try
                {
                    if (ext.Trim() == ".xls")
                    {
                        //connection string for that file which extantion is .xls  
                        ConStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (ext.Trim() == ".xlsx")
                    {
                        //connection string for that file which extantion is .xlsx  
                        ConStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    //making query  
                    string query = "SELECT * FROM [Sheet1$]";
                    //Providing connection  
                    OleDbConnection conn = new OleDbConnection(ConStr);
                    //checking that connection state is closed or not if closed the   
                    //open the connection  
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    //create command object  
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    // create a data adapter and get the data into dataadapter  
                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    DataSet ds = new DataSet();


                    //fill the Excel data to data set  
                    da.Fill(ds);
                    //set data source of the grid view  

                    Session["ImportItem"] = ds.Tables[0];

                    grdItem.DataSource = ds.Tables[0];
                    grdItem.DataBind();
                    grdItem.HeaderRow.TableSection = TableRowSection.TableHeader;
                    //close the connection  
                    conn.Close();
                    //string script = "window.onload = function() { ShowProgress(); };";
                    //ClientScript.RegisterStartupScript(this.GetType(), "ShowProgress", script, true);

                }
                catch (Exception ex)
                {
                    sysFunc.UserMsg(lblMessage, Color.Red, ex.Message);
                    return;
                }                
            }
            else
            {
                sysFunc.UserMsg(lblMessage, Color.Red, "Please Select the Excel sheet with extension .xls or .xlsx. This format is not suppoerted.");
                return;
            }
            //Response.Redirect("~/Modules/Setup/ImportItem.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            try
            {
                DataTable dt = (DataTable)Session["ImportItem"];

                SqlParameter[] param = {

                                    new SqlParameter("@DealerCode",SqlDbType.Char),//0
                                    new SqlParameter("@ItemCatCode",SqlDbType.Char),//1
                                    new SqlParameter("@ItemCode",SqlDbType.Char),//2
                                    new SqlParameter("@PartItemNo",SqlDbType.VarChar),//3
                                    new SqlParameter("@ItemDesc",SqlDbType.VarChar),//4
                                    new SqlParameter("@UnitCode",SqlDbType.Char),//5
                                    new SqlParameter("@ProdCode",SqlDbType.VarChar),//6
                                    new SqlParameter("@PartsTransLocCode",SqlDbType.VarChar),//7
                                    new SqlParameter("@PurchaseRate",SqlDbType.Float),//8
                                    new SqlParameter("@SaleRate",SqlDbType.Float),//9
                                    new SqlParameter("@EssentialFlag",SqlDbType.Char),//10
                                    new SqlParameter("@Source",SqlDbType.VarChar),//11
                                    new SqlParameter("@MinLevel",SqlDbType.Float),//12
                                    new SqlParameter("@Reorder",SqlDbType.Float),//13
                                    new SqlParameter("@Warranty",SqlDbType.Char),//14
                                    new SqlParameter("@WarrMonths",SqlDbType.Float),//15
                                    new SqlParameter("@WarrRemarks",SqlDbType.VarChar),//16
                                    new SqlParameter("@LubeFlag",SqlDbType.Char),//17
                                    new SqlParameter("@AccountCode",SqlDbType.VarChar),//18
                                    new SqlParameter("@UpdUser",SqlDbType.VarChar),//19
                                    new SqlParameter("@UpdTerm",SqlDbType.VarChar),//20
                                    new SqlParameter("@SEDPerc",SqlDbType.Float),//21
                                    new SqlParameter("@OHQTY",SqlDbType.Decimal),//22
                                    new SqlParameter("@OHStock",SqlDbType.Decimal),//23
                                    new SqlParameter("@War",SqlDbType.Decimal),//24
                                    new SqlParameter("@Critical",SqlDbType.Char),//25
                                    new SqlParameter("@PackingSize",SqlDbType.Int),//26
                                    new SqlParameter("@Access",SqlDbType.Char),//27
                                    new SqlParameter("@SGOFlag",SqlDbType.Char),//28
                                    new SqlParameter("@Liter",SqlDbType.Float),//29
                                    new SqlParameter("@invType",SqlDbType.Char),//30
                                    new SqlParameter("@FGSTPerc",SqlDbType.Decimal),//31
                                    new SqlParameter("@GSTPerc",SqlDbType.Decimal),//32
                                    new SqlParameter("@SalesExTaxPerc",SqlDbType.Decimal),//33
                                    new SqlParameter("@Genuine",SqlDbType.Char),//34
                                    new SqlParameter("@Running",SqlDbType.Char),//35
                                    new SqlParameter("@PFGSTPERC",SqlDbType.Decimal),//36
                                    new SqlParameter("@PGSTPERC",SqlDbType.Decimal),//37
                                    new SqlParameter("@ExTaxPerc",SqlDbType.Decimal),//38
                                    new SqlParameter("@Active",SqlDbType.Char),//39
                                    new SqlParameter("@InventoryItem",SqlDbType.Char),//40
                                    new SqlParameter("@SaleItem",SqlDbType.Char),//41
                                    new SqlParameter("@COGS",SqlDbType.Char),//42
                                    new SqlParameter("@StockInAccount",SqlDbType.Char),//43
                                    new SqlParameter("@RevenueAccount",SqlDbType.Char),//44
                                    new SqlParameter("@StockOutAccount",SqlDbType.Char),//45
                                    new SqlParameter("@SalePromotionPoint",SqlDbType.Int),//46
                                    new SqlParameter("@PurchasePromotionPoint",SqlDbType.Int),//47
                                   };

                DataRow[] drr = dt.Select();

                for (int i = 0; i < drr.Length; i++)
                {
                    param[0].Value = dt.Rows[i]["DealerCode"].ToString();
                    param[1].Value = dt.Rows[i]["ItemCatCode"].ToString();
                    param[2].Value = dt.Rows[i]["ItemCode"].ToString();
                    param[3].Value = dt.Rows[i]["PartItemNo"].ToString();
                    param[4].Value = dt.Rows[i]["ItemDesc"].ToString();
                    param[5].Value = dt.Rows[i]["UnitCode"].ToString();
                    param[6].Value = dt.Rows[i]["ProdCode"].ToString();
                    param[7].Value = dt.Rows[i]["PartsTransLocCode"].ToString();
                    param[8].Value = dt.Rows[i]["PurchaseRate"].ToString();
                    param[9].Value = dt.Rows[i]["SaleRate"].ToString();
                    param[10].Value = "Y";
                    param[11].Value = dt.Rows[i]["Source"].ToString();
                    param[12].Value = dt.Rows[i]["MinLevel"].ToString();
                    param[13].Value = dt.Rows[i]["Reorder"].ToString();
                    param[14].Value = "N";
                    param[15].Value = 0;
                    param[16].Value = "";
                    param[17].Value = dt.Rows[i]["LubeFlag"].ToString();
                    param[18].Value = "";
                    param[19].Value = dt.Rows[i]["UpdUser"].ToString();
                    param[20].Value = dt.Rows[i]["UpdTerm"].ToString();
                    param[21].Value = 0;
                    param[22].Value = 0.00000;
                    param[23].Value = 0.00000;
                    param[24].Value = 0.00000;
                    param[25].Value = "Y";
                    param[26].Value = Convert.ToInt32(dt.Rows[i]["PackingSize"].ToString() == "" ? "0" : dt.Rows[i]["PackingSize"].ToString());
                    param[27].Value = dt.Rows[i]["Access"].ToString();
                    param[28].Value = "N";
                    param[29].Value = Convert.ToDecimal(dt.Rows[i]["Liter"].ToString() == "" ? "0" : dt.Rows[i]["Liter"].ToString());
                    param[30].Value = "N";
                    param[31].Value = Convert.ToDecimal(dt.Rows[i]["FGSTPerc"].ToString().Trim() == "" ? "0" : dt.Rows[i]["FGSTPerc"].ToString());
                    param[32].Value = Convert.ToDecimal(dt.Rows[i]["GSTPerc"].ToString().Trim() == "" ? "0" : dt.Rows[i]["GSTPerc"].ToString());
                    param[33].Value = Convert.ToDecimal(dt.Rows[i]["SalesExTaxPerc"].ToString().Trim() == "" ? "0" : dt.Rows[i]["Liter"].ToString());
                    param[34].Value = "";
                    param[35].Value = "";
                    param[36].Value = Convert.ToDecimal(dt.Rows[i]["PFGSTPERC"].ToString().Trim() == "" ? "0" : dt.Rows[i]["PFGSTPERC"].ToString().Trim());
                    param[37].Value = Convert.ToDecimal(dt.Rows[i]["PGSTPERC"].ToString().Trim() == "" ? "0" : dt.Rows[i]["PGSTPERC"].ToString().Trim());
                    param[38].Value = Convert.ToDecimal(dt.Rows[i]["ExTaxPerc"].ToString().Trim() == "" ? "0" : dt.Rows[i]["ExTaxPerc"].ToString().Trim());
                    param[39].Value = dt.Rows[i]["Active"].ToString();
                    param[40].Value = dt.Rows[i]["InventoryItem"].ToString();
                    param[41].Value = dt.Rows[i]["SaleItem"].ToString();
                    param[42].Value = dt.Rows[i]["COGS"].ToString();
                    param[43].Value = dt.Rows[i]["StockInAccount"].ToString();
                    param[44].Value = dt.Rows[i]["RevenueAccount"].ToString();
                    param[45].Value = dt.Rows[i]["StockOutAccount"].ToString();
                    param[46].Value = Convert.ToInt32(dt.Rows[i]["SalePromotionPoint"].ToString().Trim() == "" ? "0" : dt.Rows[i]["SalePromotionPoint"].ToString().Trim());
                    param[47].Value = Convert.ToInt32(dt.Rows[i]["PurchasePromotionPoint"].ToString().Trim() == "" ? "0" : dt.Rows[i]["PurchasePromotionPoint"].ToString().Trim());

                    sysFunc.ExecuteSP_NonQuery("Sp_Insert_Item", param);
                    //if(sysFunc.ExecuteSP_NonQuery("Sp_Insert_Item", param))
                    //{
                    //    sysFunc.UserMsg(lblMessage, Color.Green, "Inserted " + i + 1 + "/" + drr.Length);
                    //}

                }

                //BindGridview();

                sysFunc.UserMsg(lblMessage, Color.Green, "Item Saved Successfully");
                

            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myalert", "alert('" + ex.Message + "');", true);
                sysFunc.UserMsg(lblMessage, Color.Red, ex.Message);
            }
        }

        protected void Download_Click(object sender, EventArgs e)
        {
            //  lblMessage.Text = "~/Demo/" + fuResume.FileName.ToString();
            //if (lblresume.Text != string.Empty)
            //{
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            string filePath = "Download/Book1";

            string FilePath = Server.MapPath("~") + "Download\\";
            string FileName = "Book1"+ ".xlsx";
            string File = FilePath + FileName;
            response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment;filename=Book1.xlsx");
            byte[] data = req.DownloadData(File);
           // response.BinaryWrite(data);
           // response.WriteFile(File);
            response.TransmitFile(File);
            response.End();



            //filePath = "~/Download/Book1";
            //Response.ContentType = "xlsx";
            //Response.AddHeader("Content-Disposition", "attachment;filename=\"Book1\"");
            //Response.TransmitFile(Server.MapPath(filePath));
            //Response.End();
        }
        
    }
}