using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text;


/// <summary>
/// Summary description for clsLookUp

/// </summary>
public class clsLookUp 
{
    //ClsSessionInfo cls = new ClsSessionInfo();
    public clsLookUp()
    {
       
    }
    public void ShowLookUp(Control obj, string lookupid, string RootURL)
    {     
       
        StringBuilder builder = new StringBuilder();
        builder.Append("window.open('" + RootURL + "LookUp.aspx?lpi=" + lookupid + "','OpenWindow','width=800, height=600, top=80, left=150, toolbar=0, menubar=0, location=1, status=1, scrollbars=1, resizable=0');");        
        
        base.GetType();
        ScriptManager.RegisterClientScriptBlock(obj, obj.GetType(), "LookUP", builder.ToString(), true);
    }

    public void ShowLookUpforParts(Control obj, string lookupid, string RootURL, string AppendInGroupBy)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        StringBuilder builder = new StringBuilder();
        builder.Append("window.open('" + RootURL + "LookUp.aspx?lpi=" + lookupid + "&Grp=" + AppendInGroupBy + "','OpenWindow','width=800, height=600, top=80, left=150, toolbar=0, menubar=0, location=1, status=1, scrollbars=1, resizable=0');");

        base.GetType();
        ScriptManager.RegisterClientScriptBlock(obj, obj.GetType(), "LookUP", builder.ToString(), true);
    }


    public void LU_Get_VehicleCode(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA// Get vehicle code
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["ColumnsName"] = "Select ProdCode,VersionCode,ProdDesc from Vehicle";
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_VehicleStock(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA// Get vehicle code
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "Select EngineNo,ChasisNo,ProdCode,BookingNo from VehicleStock";
        AppendInWhere = "DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    #region // Muhammad Javed // Lookup Methods for SERVICE MODULE
    //MJ// Get Customer 
    public void CustomerLookup(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["ColumnsName"] = "select cusCode as Code,CusDesc+' '+FatherHusName as [Description], Address1+' '+Address2+' '+Address3 as [Address],isnull(phone1,isnull(phone2,CellNo)) as Phone from Customer";
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        AppendOrderby = "Order by CusCode Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }

    //MJ// JobType | JobTypeDesc
    public void LU_Get_JobType(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select JobTypeCode as [Job Type Code],JobTypeDesc as [Job Type Description] from JobTypeMaster";
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') ";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }

    //MJ// Vendor | Vendor Desription
    public void LU_Get_Vendor(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select VendorCode as [Vendor Code],VendorDesc as [Vendor Desription] from vendor";
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') ";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    //MJ// Schedule KM | Product | Version
    public void LU_Get_ScheduleKM(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select KM as [Schedual KM],ProdCode as [Product],Version from MaintainenceSchedule";
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') ";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }

    //MJ// Get RegNo,ChassisNo and EngineNo    
    public void RegChassisEngine(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        string SelectClause = " Select z.EngineNo [Engine No],z.RegNo [Reg No],z.ChassisNo [Chassis No],z.Customer," +
                      "convert(varchar(10),Cast(z.dt as datetime),105) UpdDate from  " +
                      "( " +
                      "SELECT Distinct cv.EngineNo, " +
                      "cv.RegNo, " +
                      "cv.ChassisNo, " +
                      "c.CusDesc + ' ' + c.FatherHusName AS Customer , " +
                      "convert(varchar(10),cv.UpdDate,111) dt, " +
                      "cv.DealerCode " +
                      "FROM  CustomerVehicle cv " +
                      "INNER JOIN Customer c " +
                      "ON cv.CusCode = c.CusCode " +
                      "AND cv.DealerCode=c.DealerCode  " +
                      ") z ";
        AppendInWhere ="z.DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "')";
        AppendOrderby = " order by z.dt desc";
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }

    //MJ// JobCard Code
    public void LU_Get_JobCardNo(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {

        //string SelectClause = "SELECT J.JobCardCode, J.RegNo, J.UserName, JT.JobTypeCode " +
        //                "    FROM JobCardMaster J LEFT OUTER JOIN " +
        //                "    JobTypeMaster JT ON J.JobTypeCode = JT.JobTypeCode and J.Gatepass='' ";
        string AppendOrderby;
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string SelectClause = "SELECT J.JobCardCode, J.RegNo, J.UserName " +
                        "    FROM JobCardMaster J LEFT OUTER JOIN " +
                        "    JobTypeMaster JT ON J.JobTypeCode = JT.JobTypeCode  ";

        //Append in where clause
        AppendInWhere = " J.DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "')  And  J.Gatepass='' And DelFlag='N' and J.Jobtypecode <> '001' and J.JobTypeCode <> '002' and J.JobCardType <> 'Warranty' and J.JobCardType <> 'Repeated JobCard'  ";
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        AppendOrderby = "Order by J.JobCardCode Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_JobCardNoForm(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {

        //string SelectClause = "SELECT J.JobCardCode, J.RegNo, J.UserName, JT.JobTypeCode " +
        //                "    FROM JobCardMaster J LEFT OUTER JOIN " +
        //                "    JobTypeMaster JT ON J.JobTypeCode = JT.JobTypeCode and J.Gatepass='' ";
        string AppendOrderby;
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string SelectClause = "SELECT J.JobCardCode, J.RegNo, J.UserName " +
                        "    FROM JobCardMaster J LEFT OUTER JOIN " +
                        "    JobTypeMaster JT ON J.JobTypeCode = JT.JobTypeCode  ";

        //Append in where clause
        AppendInWhere = " J.DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "')  And DelFlag='N' and J.GatePass=''  ";
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        AppendOrderby = "Order by J.JobCardCode Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }

    //MJ// Job

    public void LU_Get_CustomerEstimateNoForm(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {

        //string SelectClause = "SELECT J.JobCardCode, J.RegNo, J.UserName, JT.JobTypeCode " +
        //                "    FROM JobCardMaster J LEFT OUTER JOIN " +
        //                "    JobTypeMaster JT ON J.JobTypeCode = JT.JobTypeCode and J.Gatepass='' ";
        string AppendOrderby;
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        //string SelectClause = "SELECT CE.CustomerEstimateCode, CE.RegNo, CE.UserName " +
          //              "    FROM CustomerEstimateMaster CE INNER JOIN " +
            //            "    CustomerEstimateDetail CED ON CE.CustomerEstimateCode = CED.CustomerEstimateCode  ";
        //Append in where clause
        string SelectClause = "SELECT CE.CustomerEstimateCode, CE.RegNo, CE.UserName " +
                        "    FROM CustomerEstimateMaster CE ";

        AppendInWhere = " CE.DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "')  And DelFlag='N'";
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        AppendOrderby = "Order by CE.CustomerEstimateCode Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }

    public void LU_Get_Job(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string SelectClause = "SELECT DJ.DefJobCode as [Job Code], DJ.DefJobDesc as [Job Description], JC.JobCatDesc as [Job Category] FROM DefaultJob DJ LEFT OUTER JOIN " +
                               " JobCategory JC ON DJ.DealerCode = JC.DealerCode And DJ.JobCatCode = JC.JobCatCode ";
                       
        //Append in where clause
        AppendInWhere = " DJ.DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') ";//order by JobcatDesc";
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }



    //MJ// BrandCode | BrandDescription
    public void LU_Get_Brand(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select BrandCode as [Brand Code],BrandDesc as[Brand Description] from brand";
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "')";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL); 
    }




    //MJ// ColorCode | ColorDescription
    public void LU_Get_Color(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select ColorCode as [Color Code],ColorDesc as [Color Description] from Color";
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "')";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }


    //MJ// ProdCode | Version | Description
    public void LU_Get_Product(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select  ProdCode as[Vehicle Code],VersionCode as[Version],ProdDesc as [Description],ColorCode,Color from vehicle";
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "')";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    
    //MJ// Invoice No
    public void LU_Get_InvoiceNo(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        string AppendOrderby;
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string SelectClause = " SELECT  CI.InvoiceNo AS [Invoice No], CI.JobCardCode AS [JobCard Code]," +
                        "    dbo.getNewDate(CI.InvoiceDate) AS [Invoice Date], DJ.DefJobDesc AS [JobCard Type]  " +
                        "    FROM CustomerInvoice AS CI LEFT OUTER JOIN  DefaultJob AS DJ ON CI.JobCardCode = DJ.DefJobCode ";
        //Append in where clause
        AppendInWhere = "  CI.DealerCode IN '00001' And Delflag<>'Y'";
        AppendOrderby = "Order by CI.InvoiceNo Desc";
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }



    //MJ// Part | Part Description
    public void LU_Get_Parts(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string AppendInGroupBy= string.Empty;
        string SelectClause = "   Select    " +
                        "    A.ItemCode as [Item Code],    " +
                        "    A.ItemDesc as [Part Name],   " +
                        "    A.UnitCode as [Unit Code],   " +
                        "    A.PartItemNo as [Part Item No.],    " +
                        "    IsNull(Sum(B.RecQty) + Sum(B.RetQty) - Sum(B.IssQty) - Sum(B.ChargeOutQty) - Sum(B.PurRetQty), 0) As [Quantity]    " +
                        "    From    " +
                        "    Item A   " +
                        "    Left outer join ItemStock B    " +
        "    ON    " +
        "    A.ItemCode = B.ItemCode And    " +
        "   B.DealerCode = '" + HttpContext.Current.Session["DealerCode"].ToString() + "'   ";


        //Append in where clause
        AppendInWhere = "   A.LubeFlag = 'N' ";
       
           AppendInGroupBy= "   Group By   " +
        "   A.PartItemNo,    " +
        "   A.ItemDesc,   " +
        "   A.UnitCode,   " +
        "   A.ItemCode   ";


        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        //HttpContext.Current.Session["AppendInGroupBy"] = AppendInGroupBy;
        ShowLookUpforParts(obj, lookupid, RootURL, AppendInGroupBy);
    }
    public void LU_Get_Parts_ChargOut(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string AppendInGroupBy = string.Empty;
        string SelectClause = "   Select * From( Select " +
                        "    A.ItemCode,    " +
                        "    A.ItemDesc,   " +
                        "    A.UnitCode,   " +
                        "    A.PartItemNo,    " +
                        "    IsNull(Sum(B.RecQty) + Sum(B.RetQty) - Sum(B.IssQty) - Sum(B.ChargeOutQty) - Sum(B.PurRetQty), 0) As [Quantity]    " +
                        "    From    " +
                        "    Item A   " +
                        "    Left outer join ItemStock B    " +
        "    ON    " +
        "    A.ItemCode = B.ItemCode And    " +
        "   B.DealerCode = '" + HttpContext.Current.Session["DealerCode"].ToString() + "' inner join PartsTransLocation C On B.DealerCode=C.DealerCode  And B.PartsTransLocCode=C.PartsTransLocCode    " +
       
        "   Group By A.PartItemNo, A.ItemDesc,A.UnitCode, A.ItemCode) z ";

        
        //Append in where clause
        AppendInWhere = "   z.Quantity > 0 ";
        AppendInGroupBy = "";
     //   AppendInGroupBy = "   Group By   " +
     //"   A.PartItemNo,    " +
     //"   A.ItemDesc,   " +
     //"   A.UnitCode,   " +
     //"   A.ItemCode   ";


        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        //HttpContext.Current.Session["AppendInGroupBy"] = AppendInGroupBy;
        ShowLookUpforParts(obj, lookupid, RootURL, AppendInGroupBy);
    }
    public void LU_Get_Parts_StockRet(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string SelectClause = "   Select a.ItemCode,b.PartItemNo,b.ItemDesc,b.UnitCode " +
                        "    from StockIssueDetail a,Item b   ";
       
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        //HttpContext.Current.Session["AppendInGroupBy"] = AppendInGroupBy;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_Parts_SaleRet(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string SelectClause = "   Select a.ItemCode,b.PartItemNo,b.ItemDesc,b.UnitCode " +
                        "    from countersaledetail a inner join Item b On a.ItemCode=b.ItemCode  ";

        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        //HttpContext.Current.Session["AppendInGroupBy"] = AppendInGroupBy;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_Parts_SaleRet_JC(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string SelectClause = "   Select a.ItemCode,b.PartItemNo,b.ItemDesc,b.UnitCode " +
                        "    from jobcardpartsdetail a inner join Item b On a.ItemCode=b.ItemCode  " +
                        "    inner join StockIssueDetail c " +
                        "     On a.DealerCode=c.DealerCode " +
                        "        And a.ItemCode=c.ItemCode " ;

        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        //HttpContext.Current.Session["AppendInGroupBy"] = AppendInGroupBy;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_Parts_PR(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string SelectClause = "   Select a.ItemCode,b.PartItemNo,b.ItemDesc,b.UnitCode " +
                        "    from GRNDetail a,Item b   ";

        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        //HttpContext.Current.Session["AppendInGroupBy"] = AppendInGroupBy;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_Lubs(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string SelectClause = "   Select    " +
                        "    A.ItemCode as [Item Code],    " +
                        "    A.PartItemNo as [Part Item No.],    " +
                        "    A.ItemDesc as [Part Name],   " +
                        "    A.SaleRate,"+
                        "    IsNull(Sum(B.RecQty) + Sum(B.RetQty) - Sum(B.IssQty) - Sum(B.ChargeOutQty) - Sum(B.PurRetQty), 0) As [Quantity]    " +
                        "    From    " +
                        "    Item A   " +
                        "    Left outer join ItemStock B    " +
        "    ON    " +
        "    A.ItemCode = B.ItemCode And    " +
        "   B.DealerCode = '" + HttpContext.Current.Session["DealerCode"] + "'   ";


        //Append in where clause
        AppendInWhere = "   A.LubeFlag = 'Y' " +
                        "   Group By   " +
                        "   A.PartItemNo,    " +
                        "   A.ItemDesc,   " +
                        "   A.UnitCode,   " +
                        "   A.ItemCode,   "+
                        "   A.SaleRate ";


        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    //MJ// Lub | Lub Description
    public void LU_Get_Lub(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string SelectClause = "   Select    " +
                        "    A.ItemCode as [Item Code],    " +
                        "    A.PartItemNo as [Part Item No.],    " +
                        "    A.ItemDesc as [Part Name],   " +
                        "    IsNull(Sum(B.RecQty) + Sum(B.RetQty) - Sum(B.IssQty) - Sum(B.ChargeOutQty) - Sum(B.PurRetQty), 0) As [Quantity]    " +
                        "    From    " +
                        "    Item A,   " +
                        "    ItemStock B   ";

        //Append in where clause
        AppendInWhere = "   A.DealerCode *= B.DealerCode   " +
                        "   And A.ItemCode *= B.ItemCode   " +
                        "   And A.LubeFlag = 'Y' " +
                        "   Group By   " +
                        "   A.PartItemNo,    " +
                        "   A.ItemDesc,   " +
                        "   A.UnitCode,   " +
                        "   A.ItemCode   ";


        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }





    //MJ// Get End User
    public void EndUser(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "cusCode as Code,CusDesc+' '+FatherHusName as [Description], Address1+' '+Address2+' '+Address3 as [Address],isnull(phone1,isnull(phone2,CellNo)) as Phone";
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }

    #endregion

    #region // Muhammad Farhan//
    public void LU_Get_Vendors(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        /* Muhammad Farhan  */
        /* For Vendor  */
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        try
        {
            HttpContext.Current.Session["ColumnsName"] = "Select VendorCode,VendorDesc from Vendor ";
            AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            ShowLookUp(obj, lookupid, RootURL);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public void LU_Get_DCNumber(Control obj, string lookupid, string AppendInWhere, string RootURL, string FormName)
    {
        /* Muhammad Farhan  */
        /* For Part Item  */
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        try
        {
            string query = string.Empty;
            string query1 = string.Empty;
            string AppendOrderby;
            if (FormName == "DC")
            {
                query += " Select  " + Environment.NewLine;
                query += " a.DCNo, " + Environment.NewLine;
                query += " convert(varchar(12),a.DCDate,103) as DCDate , " + Environment.NewLine;
                query += " a.PONo, " + Environment.NewLine;
                query += " b.VendorDesc  " + Environment.NewLine;
                query += " from DCMaster a,Vendor b " + Environment.NewLine;
                //Where
                query1 += " a.VendorCode = b.VendorCode  " + Environment.NewLine;
                query1 += " and a.DealerCode = '" + HttpContext.Current.Session["DealerCode"].ToString() + "'  " + Environment.NewLine;
                query1 += " and a.DelFlag = 'N' " + Environment.NewLine;
                //query1 += " and a.DealerCode = b.DealerCode  " + Environment.NewLine;
                query1 += " and a.VendorCode = b.VendorCode  " + Environment.NewLine;
                query1 += " and a.DCNo Not IN  " + Environment.NewLine;
                query1 += " (Select DCNo from GRNMaster Where DealerCode = '" + HttpContext.Current.Session["DealerCode"].ToString() + "'and Delflag = 'N')  " + Environment.NewLine;    
                query1 += " and a.DCNo Like '%/%' " + Environment.NewLine;
              

            }
            else if (FormName == "GRN")
            {
                query += " SELECT  a.DCNo, " + Environment.NewLine;
                query += " 	a.DCDate, " + Environment.NewLine;
                query += " 	a.PONo, " + Environment.NewLine;
                query += " 	b.VendorDesc " + Environment.NewLine;
                query += "  FROM DCMaster a, " + Environment.NewLine;
                query += "  Vendor b " + Environment.NewLine;
                //where 
                query1 += " a.DealerCode = '" + HttpContext.Current.Session["DealerCode"].ToString() + "' " + Environment.NewLine;
                query1 += " AND a.DelFlag = 'N' " + Environment.NewLine;
                //query += " 	and a.DealerCode = b.DealerCode " + Environment.NewLine;
                query1 += " AND a.VendorCode = b.VendorCode " + Environment.NewLine;
                query1 += " AND a.DCNo NOT IN ( " + Environment.NewLine;
                query1 += "     	SELECT DCNo " + Environment.NewLine;
                query1 += " 		FROM GRNMaster " + Environment.NewLine;
                query1 += " 		WHERE DealerCode = '" + HttpContext.Current.Session["DealerCode"].ToString() + "' " + Environment.NewLine;
                query1 += " 		AND Delflag = 'N' " + Environment.NewLine;
                query1 += " 		) " + Environment.NewLine;
                query1 += " 	AND a.DCNo LIKE '%/%' " + Environment.NewLine;
               

            }
            AppendOrderby = "Order by a.DCNo Desc";
            HttpContext.Current.Session["ColumnsName"] = query;
            AppendInWhere = query1;
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
            ShowLookUp(obj, lookupid, RootURL);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void LU_Get_BillNoDetail(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA// Get vehicle code
        HttpContext.Current.Session["ColumnsName"] = "Select JENBillNo,BillDate,BillAmount from JenBillMaster";
        AppendInWhere = "DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
 
    public void LU_Get_PONumber(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        /* Muhammad Farhan  */
        /* For Part Item  */

        string query = string.Empty;
        string query1 = string.Empty;
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        try
        {
            query += " Select  " + Environment.NewLine;
            query += " a.POno, " + Environment.NewLine;
            query += " a.PODate, " + Environment.NewLine;
            query += " b.VendorDesc, " + Environment.NewLine;
            query += " d.NName   " + Environment.NewLine;
            query += " from POMaster a,Vendor b,partsDeliveryTypes d  " + Environment.NewLine;
            //Where
            query1 += "   a.DealerCode = '" + HttpContext.Current.Session["DealerCode"].ToString() + "' " + Environment.NewLine;
            query1 += " and a.DelFlag = 'N' " + Environment.NewLine;
            query1 += " and a.VendorCode = b.VendorCode " + Environment.NewLine;
            query1 += " and a.deliverytype = d.sno " + Environment.NewLine;
            query1 += " and a.PONo Like '%/%' " + Environment.NewLine;
            
            AppendOrderby = "Order by a.POno Desc";
            HttpContext.Current.Session["ColumnsName"] = query;
            AppendInWhere = query1;
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
            ShowLookUp(obj, lookupid, RootURL);
        }
        catch (Exception ex)
        {

            throw ex;
        }
      
    }
    public void LU_Get_PONumber(Control obj, string lookupid, string AppendInWhere, string RootURL, string FormName)
    {
        /* Muhammad Farhan  */
        /* For Part Item  */

        try
        {
            string query = string.Empty;
            string query1 = string.Empty;
            HttpContext.Current.Session["AppendOrderby"] = string.Empty;
            string AppendOrderby = string.Empty;
            if (FormName == "PO")
            {
                query += " Select  " + Environment.NewLine;
                query += " a.POno, " + Environment.NewLine;
                query += " convert(varchar(20),a.PODate,103) as PODate, " + Environment.NewLine;
                query += " c.VendorDesc, " + Environment.NewLine;
                //query += " a.deliverytype,  " + Environment.NewLine;
                query += " d.NName  " + Environment.NewLine;
                query += " from  POMaster a,PODetail b,Vendor c,partsDeliveryTypes d   " + Environment.NewLine;
                //Where
                query1 += " a.VendorCode = c.VendorCode  " + Environment.NewLine;
                query1 += " and a.DealerCode = b.DealerCode " + Environment.NewLine;
                //query1 += " and a.DealerCode = c.DealerCode " + Environment.NewLine;
                query1 += " and a.PONo = b.PONo  and a.PONo Like '%/%' " + Environment.NewLine;
                query1 += " and a.DealerCode = '" + HttpContext.Current.Session["DealerCode"].ToString() + "' " + Environment.NewLine;
                query1 += " and a.deliverytype = d.sno  " + Environment.NewLine;
                query1 += " Group By a.PONo,a.PODate,c.VendorDesc, a.deliveryType, d.NName  " + Environment.NewLine;
                query1 += " Having sum(b.POQty) > sum(b.RecQty) " + Environment.NewLine;
               
                
            }
            else if (FormName == "DC")
            {
                query += " Select  " + Environment.NewLine;
                query += " a.POno, " + Environment.NewLine;
                query += " a.PODate, " + Environment.NewLine;
                query += " b.VendorDesc, " + Environment.NewLine;
                query += " a.deliverytype  " + Environment.NewLine;
                query += " from POMaster a,Vendor b " + Environment.NewLine;
                //Where
                query1 += " a.VendorCode = b.VendorCode  " + Environment.NewLine;
                query1 += " and a.DealerCode = '" + HttpContext.Current.Session["DealerCode"].ToString() + "'  " + Environment.NewLine;
                query1 += " and a.DelFlag = 'N' " + Environment.NewLine;
                query1 += " and a.transferStatus <> 'C'  " + Environment.NewLine;
                query1 += " and a.PONo Like '%/%' " + Environment.NewLine;
            }

            AppendOrderby = "Order by a.POno Desc";
            HttpContext.Current.Session["ColumnsName"] = query;
            AppendInWhere = query1;
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
            ShowLookUp(obj, lookupid, RootURL);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public void LU_Get_PartItem_PO(Control obj, string lookupid, string AppendInWhere, string RootURL,
   string FormName, string POCode, string DCCode, string PartItemNo,string PartSource)
    {
        /* Muhammad Farhan  */
        /* For Part Item  */
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string AppendInGroupBy = string.Empty;
        try
        {
            string query = string.Empty;
            string query1 = string.Empty;

            if (FormName == "PO")
            {
                query += "   Select    " + Environment.NewLine;
                query += "    A.ItemCode,    " + Environment.NewLine;
                query += "    A.ItemDesc,    " + Environment.NewLine;
                query += "    A.UnitCode ,   " + Environment.NewLine;
                query += "    A.PartItemNo ,   " + Environment.NewLine;
                query += "    IsNull(Sum(B.RecQty) + Sum(B.PurRetQty) + Sum(B.RetQty) - ((Sum(B.IssQty) + Sum(B.ChargeOutQty)) ), 0) As BalQty    " + Environment.NewLine;
                query += "    From Item A     " + Environment.NewLine;
                query += "    Left Outer Join ItemStock B   " + Environment.NewLine;
                query += "    ON A.ItemCode=B.ItemCode   " + Environment.NewLine;
                query += "    And B.DealerCode='" + HttpContext.Current.Session["DealerCode"].ToString() + "' " + Environment.NewLine;
                //where
                //query1 += "   A.DealerCode *= B.DealerCode   " + Environment.NewLine;
                //query1 += "   And A.ItemCode *= B.ItemCode   " + Environment.NewLine;
                //query1 += "   And A.LubeFlag = 'N' " + Environment.NewLine;


                AppendInGroupBy = "  Group By A.PartItemNo, A.ItemDesc,A.UnitCode,A.ItemCode ";

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void LU_Get_PartItem(Control obj, string lookupid, string AppendInWhere, string RootURL,
    string FormName, string POCode, string DCCode, string PartItemNo)
    {
        /* Muhammad Farhan  */
        /* For Part Item  */
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string AppendInGroupBy = string.Empty;
        try
        {
            string query = string.Empty;
            string query1 = string.Empty;

            if (FormName == "PO")
            {
                query += "   Select    " + Environment.NewLine;
                query += "    A.ItemCode,    " + Environment.NewLine;
                query += "    A.ItemDesc,    " + Environment.NewLine;
                query += "    A.UnitCode ,   " + Environment.NewLine;
                query += "    A.PartItemNo ,   " + Environment.NewLine;
                query += "    IsNull(Sum(B.RecQty) + Sum(B.PurRetQty) + Sum(B.RetQty) - ((Sum(B.IssQty) + Sum(B.ChargeOutQty)) ), 0) As BalQty    " + Environment.NewLine;
                query += "    From Item A     " + Environment.NewLine;
                query += "    Left Outer Join ItemStock B   " + Environment.NewLine;
                query += "    ON A.ItemCode=B.ItemCode   " + Environment.NewLine;
                query += "    And B.DealerCode='" + HttpContext.Current.Session["DealerCode"].ToString() + "' " + Environment.NewLine; 
                //where
                query1 +=    AppendInWhere + Environment.NewLine;
                //query1 += "   And A.ItemCode *= B.ItemCode   " + Environment.NewLine;
                //query1 += "   And A.LubeFlag = 'N' " + Environment.NewLine;


                AppendInGroupBy = "  Group By A.PartItemNo, A.ItemDesc,A.UnitCode,A.ItemCode ";
                
            }
            else if (FormName == "DC")
            {


                query += " Select b.ItemCode,b.ItemDesc   " + Environment.NewLine;
                query += " from PODetail a,Item b " + Environment.NewLine;
                //Where
                query1 += " a.DealerCode = '" + HttpContext.Current.Session["DealerCode"].ToString() + "' " + Environment.NewLine;
                query1 += " and a.ItemCode = b.ItemCode " + Environment.NewLine;
                query1 += " and a.PONo = '" + POCode + "' " + Environment.NewLine;
                query1 += " and a.POQty > a.RecQty " + Environment.NewLine;
                query1 += " And b.ItemCode not in (" + PartItemNo + ") " + Environment.NewLine;

                query1 += " Union " + Environment.NewLine;

                query1 += " Select b.ItemCode,b.ItemDesc   " + Environment.NewLine;
                query1 += " from DCDetail a,Item b " + Environment.NewLine;
                query1 += " Where  " + Environment.NewLine;
                query1 += " a.ItemCode = b.ItemCode " + Environment.NewLine;
                query1 += " and a.DCNo = '" + DCCode + "' " + Environment.NewLine;
                query1 += " And b.ItemCode not in (" + PartItemNo + ") " + Environment.NewLine;
                query1 += " And a.DealerCode='" + HttpContext.Current.Session["DealerCode"].ToString() + "' " + Environment.NewLine; 
            }
            else if (FormName == "SI")
            {

                query += "  Select a.ItemCode,I.ItemDesc,u.unitdesc     " + Environment.NewLine;
                query += "  from SIRDetail a,Item I,unit u    " + Environment.NewLine;
                //Where
                query1 += "  a.DealerCode =  '" + DCCode + "'  " + Environment.NewLine;
                //query1 += "  and a.DealerCode = b.DealerCode   " + Environment.NewLine;
                //query1 += "  and a.ItemCode = b.ItemCode   " + Environment.NewLine;
                query1 += "  and a.SIRNo = '" + POCode + "'  " + Environment.NewLine;
                query1 += "  and a.Quantity >= a.RecQty    " + Environment.NewLine;
                query1 += "  and a.ItemCode = I.ItemCode   " + Environment.NewLine;
                query1 += "  and I.ItemCatCode = u.UnitCode      " + Environment.NewLine;
                query1 += "  And a.ItemCode not in (" + PartItemNo + ") " + Environment.NewLine;
            }
            HttpContext.Current.Session["ColumnsName"] = query;
            AppendInWhere = query1;
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            ShowLookUpforParts(obj, lookupid, RootURL, AppendInGroupBy);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void LU_Get_PartItemLocation(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        /* Muhammad Farhan  */
        /* For Part Item Location  */
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        try
        {
            HttpContext.Current.Session["ColumnsName"] = "Select PartsTransLocCode,PartsTransLocDesc From PartsTransLocation  ";
            AppendInWhere = " DealerCode IN ('" + HttpContext.Current.Session["DealerCode"].ToString() + "','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            ShowLookUp(obj, lookupid, RootURL);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public void LU_Get_GRNNumber(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        /* Muhammad Farhan  */
        /* For GRN Number */
        string query = string.Empty;
        string query1 = string.Empty;
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        try
        {
            

            query += "  Select a.GRNNo,convert(varchar(20),a.GRnDate,103) as GRnDate,PoNo ,a.DCNo,b.VendorDesc    " + Environment.NewLine;
            query += "  from GRNMaster a,Vendor b   " + Environment.NewLine;
            //Where
            query1 += "  a.DealerCode =  '" + HttpContext.Current.Session["DealerCode"].ToString() + "'  " + Environment.NewLine;
            query1 += "  and a.DelFlag = 'N'  " + Environment.NewLine;
            //query1 += "  and a.DealerCode = b.DealerCode   " + Environment.NewLine;
            query1 += " and a.VendorCode = b.VendorCode and a.GRNNo Like '%/%'  " + Environment.NewLine;

            AppendOrderby = "Order by a.GRNNo Desc";
            HttpContext.Current.Session["ColumnsName"] = query;
            AppendInWhere = query1;
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
            ShowLookUp(obj, lookupid, RootURL);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void LU_Get_SIRNumber(Control obj, string lookupid, string AppendInWhere, string RootURL, string FormName)
    {
        /* Muhammad Farhan  */
        /* For  SIR Number  */
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        try
        {
            string query = string.Empty;
            string query1 = string.Empty;
            if (FormName == "SIR")
            {

                query += " SELECT DISTINCT a.SIRNo, " + Environment.NewLine;
                query += " convert(varchar(20),a.SIRDate,103) as SIRDate, " + Environment.NewLine;
                query += " 	a.JobCardNO, " + Environment.NewLine;
                query += " 	a.InvoiceNO " + Environment.NewLine;
                query += " FROM SIRMaster a " + Environment.NewLine;
                query += " INNER JOIN SIRDetail b " + Environment.NewLine;
                query += " 	ON a.DealerCode = b.DealerCode " + Environment.NewLine;
                query += " 		AND a.SIRNo = b.SIRNo " + Environment.NewLine;
                query += " INNER JOIN jobcardmaster C " + Environment.NewLine;
                query += " 	ON a.dealercode = c.dealercode " + Environment.NewLine;
                query += " 		AND a.jobcardno = c.jobcardcode " + Environment.NewLine;
                query += " 		AND c.gatepass = '' " + Environment.NewLine;
                //where 
                query1 += "  a.Requsitioner = 'J' " + Environment.NewLine;
                query1 += " 	AND a.JobCardNo <> '' " + Environment.NewLine;
                query1 += " 	AND c.DelFlag = 'N' " + Environment.NewLine;
                query1 += " 	AND b.Quantity <> b.RecQty " + Environment.NewLine;
                query1 += "     AND a.DealerCode = '" + HttpContext.Current.Session["DealerCode"].ToString() + "' " + Environment.NewLine;
                query1 += " UNION ALL " + Environment.NewLine;
                query1 += " SELECT DISTINCT a.SIRNo, " + Environment.NewLine;
                query1 += " 	convert(varchar(20),a.SIRDate,103) as SIRDate, " + Environment.NewLine;
                query1 += " 	IsNull(a.JobCardNo, '') AS JobCardNo, " + Environment.NewLine;
                query1 += " 	a.InvoiceNO " + Environment.NewLine;
                query1 += " FROM SIRMaster a " + Environment.NewLine;
                query1 += " INNER JOIN SIRDetail b " + Environment.NewLine;
                query1 += " 	ON a.DealerCode = b.DealerCode " + Environment.NewLine;
                query1 += " 		AND a.SIRNo = b.SIRNo " + Environment.NewLine;
                query1 += " INNER JOIN CounterSaleMaster c " + Environment.NewLine;
                query1 += " 	ON a.DealerCode = c.DealerCode " + Environment.NewLine;
                query1 += " 		AND a.InvoiceNo = c.SaleInvNo " + Environment.NewLine;
                query1 += " where a.DealerCode = '" + HttpContext.Current.Session["DealerCode"].ToString()  + "' and a.Requsitioner = 'C' And 	a.InvoiceNo <> '' And c.PostFlag = 'N' And c.DelFlag = 'N' And b.Quantity <> b.RecQty" + Environment.NewLine;
                query1 += " order by 1 desc " + Environment.NewLine;

            }
            else if (FormName == "GRN")
            {
                

            }

            HttpContext.Current.Session["ColumnsName"] = query;
            AppendInWhere = query1;
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            ShowLookUp(obj, lookupid, RootURL);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public void LU_Get_IssueNo(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        /* Muhammad Farhan  */
        /* For Issue Number */

        string query = string.Empty;
        string query1 = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        try
        {
            query += " Select  " + Environment.NewLine;
            query += " Distinct a.IssueNo, " + Environment.NewLine;
            query += " convert(varchar(20),a.IssueDate,103) as IssueDate, " + Environment.NewLine;
            query += " JobCardNo, " + Environment.NewLine;
            query += " InvoiceNo " + Environment.NewLine;

            query += " from StockIssue a,StockIssueDetail b " + Environment.NewLine;
            //Where
            query1 += "     a.DealerCode = '" + HttpContext.Current.Session["DealerCode"].ToString() + "'  " + Environment.NewLine;
            query1 += " and a.DelFlag = 'N' " + Environment.NewLine;
            query1 += " and a.DealerCode = b.DealerCode And a.IssueNo = b.IssueNo " + Environment.NewLine;
            AppendOrderby = "Order by a.IssueNo Desc";
            HttpContext.Current.Session["ColumnsName"] = query;
            AppendInWhere = query1;
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;

            
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
            ShowLookUp(obj, lookupid, RootURL);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void LU_Get_ChargeOut(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        /* Muhammad Farhan  */
        /* For Issue Number */

        string query = string.Empty;
        string query1 = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        try
        {
            //query += " Select  " + Environment.NewLine;
            query += " Select distinct  ChargeOutNo,ChargeOutDate,Remarks from ChargeOutMaster  " + Environment.NewLine;
            //query += " convert(varchar(20),a.IssueDate,103) as IssueDate, " + Environment.NewLine;
            //query += " JobCardNo, " + Environment.NewLine;
            //query += " InvoiceNo " + Environment.NewLine;

            //query += " from StockIssue a,StockIssueDetail b " + Environment.NewLine;
            //Where
            query1 += "     DealerCode = '" + HttpContext.Current.Session["DealerCode"].ToString() + "'  " + Environment.NewLine;
            query1 += " and DelFlag = 'N' " + Environment.NewLine;
            AppendOrderby = "Order by ChargeOutNo Desc";
            //query1 += " and a.DealerCode = b.DealerCode And a.IssueNo = b.IssueNo " + Environment.NewLine;

            HttpContext.Current.Session["ColumnsName"] = query;
            AppendInWhere = query1;
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
            ShowLookUp(obj, lookupid, RootURL);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void LU_Get_StockReturn(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        /* Muhammad Farhan  */
        /* For Issue Number */

        string query = string.Empty;
        string query1 = string.Empty;
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        try
        {
            //query += " Select  " + Environment.NewLine;
            query += " Select distinct  a.ReturnNo,a.ReturnDate, JobCardNO, InvoiceNo from StockReturn a Inner Join StockReturnDetail b On a.DealerCode = b.DealerCode And a.ReturnNo = b.ReturnNo " + Environment.NewLine;
            //query += " convert(varchar(20),a.IssueDate,103) as IssueDate, " + Environment.NewLine;
            //query += " JobCardNo, " + Environment.NewLine;
            //query += " InvoiceNo " + Environment.NewLine;

            //query += " from StockIssue a,StockIssueDetail b " + Environment.NewLine;
            //Where
            query1 += "     a.DealerCode = '" + HttpContext.Current.Session["DealerCode"].ToString() + "'  " + Environment.NewLine;
            query1 += " and a.DelFlag = 'N' " + Environment.NewLine;
            //query1 += " and a.DealerCode = b.DealerCode And a.IssueNo = b.IssueNo " + Environment.NewLine;
            AppendOrderby = "Order by a.ReturnNo Desc";
            HttpContext.Current.Session["ColumnsName"] = query;
            AppendInWhere = query1;
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
            ShowLookUp(obj, lookupid, RootURL);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
   
    public void LU_Get_SalesReturn(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        /* Akram Ali  */
        /* For Sales Return Code */

        string query = string.Empty;
        string query1 = string.Empty;
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        try
        {
            query += " Select a.SRNo, a.SRDate, b.CusDesc from  SRMaster a inner join  Customer b On a.DealerCode = b.DealerCode  AND a.CusCode = b.CusCode " + Environment.NewLine;
            query1 += " a.DealerCode = '" + HttpContext.Current.Session["DealerCode"].ToString() + "'  " + Environment.NewLine;
            query1 += " and a.DelFlag = 'N' " + Environment.NewLine;
            AppendOrderby = "Order by a.SRNo Desc";
            HttpContext.Current.Session["ColumnsName"] = query;
            AppendInWhere = query1;
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
            ShowLookUp(obj, lookupid, RootURL);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void LU_Get_PurchaseRet(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        /* Akram Ali  */
        /* For Purchase Return */

        string query = string.Empty;
        string query1 = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        try
        {
            //query += " Select  " + Environment.NewLine;
            query += " Select a.PRNo, a.PRDate, b.VendorDesc from  PRMaster a Inner Join Vendor b On  a.VendorCode = b.VendorCode " + Environment.NewLine;
            //query += " convert(varchar(20),a.IssueDate,103) as IssueDate, " + Environment.NewLine;
            //query += " JobCardNo, " + Environment.NewLine;
            //query += " InvoiceNo " + Environment.NewLine;

            //query += " from StockIssue a,StockIssueDetail b " + Environment.NewLine;
            //Where
            query1 += "     a.DealerCode = '" + HttpContext.Current.Session["DealerCode"].ToString() + "'  " + Environment.NewLine;
            query1 += " and a.DelFlag = 'N' " + Environment.NewLine;
            AppendOrderby = "Order by a.PRNo Desc";
            //query1 += " and a.DealerCode = b.DealerCode And a.IssueNo = b.IssueNo " + Environment.NewLine;

            HttpContext.Current.Session["ColumnsName"] = query;
            AppendInWhere = query1;
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
            ShowLookUp(obj, lookupid, RootURL);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
     public void LU_Get_GRN(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        /* Muhammad Farhan  */
        /* For Issue Number */

        string query = string.Empty;
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        try
        {
            query += " Select A.GRNNo, convert(varchar(20),A.GRNDate,103) As GRNDate, B.VendorDesc From GRNMaster A, Vendor B " + Environment.NewLine;
            AppendOrderby = "Order by A.GRNNo Desc";
            HttpContext.Current.Session["ColumnsName"] = query;
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
            ShowLookUp(obj, lookupid, RootURL);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void LU_Get_Issue_StockRet(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        /* Muhammad Farhan  */
        /* For Issue Number */

        string query = string.Empty;
        string query1 = string.Empty;
        string QueryGroupBy = string .Empty ;
        string AppendOrderby = string.Empty;
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        try
        {
            query = " Select  " + Environment.NewLine;
            query += " Distinct A.IssueNo, A.IssueDate,A.ReqSource, A.JobCardNo, A.InvoiceNo  " + Environment.NewLine;
            query += " From  StockIssue A Inner Join StockIssueDetail B " + Environment.NewLine;
            query += " On 	A.DealerCode = B.DealerCode And A.IssueNo = B.IssueNo  " + Environment.NewLine;
            //query += "  " + Environment.NewLine;

            //query += " from StockIssue a,StockIssueDetail b " + Environment.NewLine;
            //Where
            query1 = "     A.DealerCode = '" + HttpContext.Current.Session["DealerCode"].ToString() + "'  " + Environment.NewLine;
            query1 += " AND A.DelFlag = 'N' " + Environment.NewLine;
            query1 += " AND A.PostFlag = 'Y' " + Environment.NewLine;
            query1 += " AND 	(   (A.JobCardNo <> '' And A.JobCardNo IN (Select JobCardCode From Jobcardmaster Where DelFlag = 'N' and Gatepass='' )) " + Environment.NewLine;
            query1 += " Or (A.InvoiceNo <> '' And A.InvoiceNo IN (Select SaleInvNo From CounterSaleMaster WHere PostFlag = 'N' And DelFlag = 'N') )	)  " + Environment.NewLine;

            //QueryGroupBy = " Group By  	A.IssueNo, 	A.IssueDate, A.ReqSource,	A.JobCardNo,       A.InvoiceNo Having(sum(B.IssueQty) > sum(B.RetQty)) " + Environment.NewLine;
            QueryGroupBy = "   Group By   " +
        "   A.IssueNo,    " +
        "   A.IssueDate,   " +
        "   A.ReqSource,   " +
        "   A.JobCardNo,   " +
        "   A.InvoiceNo   Having(sum(B.IssueQty) > sum(B.RetQty))";
            AppendOrderby = "Order by A.IssueNo Desc";
            HttpContext.Current.Session["ColumnsName"] = query;
            AppendInWhere = query1;
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
            ShowLookUpforParts(obj, lookupid, RootURL, QueryGroupBy);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

        public void LU_Get_CounterSalesNo(Control obj, string lookupid, string AppendInWhere, string RootURL)
        {
            /* Muhammad Farhan  */
            /* For counter sales  */

            string query = string.Empty;
            string query1 = string.Empty;
            HttpContext.Current.Session["AppendOrderby"] = string.Empty;
            string AppendOrderby;
            try
            {
                //,,, 
                query += " Select  " + Environment.NewLine;
                query += " a.SaleInvNo, " + Environment.NewLine;
                query += "convert(varchar(20),a.SaleInvDate,103) as SaleInvDate , " + Environment.NewLine;
                query += " b.CusDesc, " + Environment.NewLine;
                query += " a.PostFlag " + Environment.NewLine;
                query += " from CounterSaleMaster a,Customer b  " + Environment.NewLine;
                //Where
                query1 += " a.DealerCode = '" + HttpContext.Current.Session["DealerCode"].ToString() + "' " + Environment.NewLine;
                query1 += " and a.DelFlag = 'N' " + Environment.NewLine;
                query1 += " and a.DealerCode = b.DealerCode " + Environment.NewLine;
                query1 += " and a.CusCode = b.CusCode " + Environment.NewLine;
                AppendOrderby = "Order by SaleInvNo Desc";

                HttpContext.Current.Session["ColumnsName"] = query;
                AppendInWhere = query1;
                HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
                HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
                ShowLookUp(obj, lookupid, RootURL);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public void LU_Get_PartsLocation(Control obj, string lookupid, string AppendInWhere, string RootURL)
        {
            /* Muhammad Farhan  */
            /* For Vendor  */
            string query = string.Empty;
            string query1 = string.Empty;
            HttpContext.Current.Session["AppendOrderby"] = string.Empty;
            try
            {

                query += "  select PartsLocCode, PartsLocDesc  " + Environment.NewLine;
                query += "   from PartsLocation    " + Environment.NewLine;
                //Where
                query1 += "  DealerCode =  '" + HttpContext.Current.Session["DealerCode"].ToString() + "'  " + Environment.NewLine;

                HttpContext.Current.Session["ColumnsName"] = query;
                AppendInWhere = query1;
                HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
                ShowLookUp(obj, lookupid, RootURL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void LU_Get_SubPartsLocation(Control obj, string lookupid, string AppendInWhere, string RootURL, string txtpartLocCode)
        {
            /* Muhammad Farhan  */
            /* For Sub Part Location  */
            string query = string.Empty;
            string query1 = string.Empty;
            HttpContext.Current.Session["AppendOrderby"] = string.Empty;
            try
            {

                query += "  select PartsSubLocCode, PartsSubLocDesc   " + Environment.NewLine;
                query += "   from PartsSubLocation   " + Environment.NewLine;
                //Where
                query1 += "  DealerCode =  '" + HttpContext.Current.Session["DealerCode"].ToString() + "'  " + Environment.NewLine;
                query1 += "  AND PartsLocCode = '" + txtpartLocCode + "'  " + Environment.NewLine;
                
                HttpContext.Current.Session["ColumnsName"] = query;
                AppendInWhere = query1;
                HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
                ShowLookUp(obj, lookupid, RootURL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LU_Get_TransPartsLocation(Control obj, string lookupid, string AppendInWhere, string RootURL, string txtpartLocCode, string PartsSubLocCode)
        {
            /* Muhammad Farhan  */
            /* For Transaction Part Location  */
            string query = string.Empty;
            string query1 = string.Empty;
            HttpContext.Current.Session["AppendOrderby"] = string.Empty;
            try
            {

                query += "  select PartsLocCode,PartsSubLocCode,Right(PartsTransLocCode, 3) as PartsTransLocCode, PartsTransLocDesc   " + Environment.NewLine;
                query += "   from PartsTransLocation   " + Environment.NewLine;
                //Where
                query1 += "  DealerCode =  '" + HttpContext.Current.Session["DealerCode"].ToString() + "'  " + Environment.NewLine;
                query1 += "  AND PartsLocCode = '" + txtpartLocCode + "'  " + Environment.NewLine;
                query1 += "  AND PartsSubLocCode = '" + PartsSubLocCode + "'  " + Environment.NewLine;

                HttpContext.Current.Session["ColumnsName"] = query;
                AppendInWhere = query1;
                HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
                ShowLookUp(obj, lookupid, RootURL);



                //LookUp.Sql = "select Right(PartsTransLocCode, 3) as PartsTransLocCode, PartsTransLocDesc from PartsTransLocation"
                //LookUp.Append = "AND PartsSubLocCode = '" & txtPartsSubLocCode._String.Trim & "' AND PartsLocCode = '" & txtPartsLocCode._String.Trim & "' AND DealerCode = '" & DEALER_CODE & "'"
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    
    #endregion

    #region // Akram Ali//
        public void LU_Get_TransPartsLocationNew(Control obj, string lookupid, string AppendInWhere, string RootURL)
        {
            /* Muhammad Farhan */
            /* For Transaction Part Location */
            HttpContext.Current.Session["AppendOrderby"] = string.Empty;
            string query = string.Empty;
            string query1 = string.Empty;
            try
            {
                query += " select PartsTransLocCode, PartsTransLocDesc " + Environment.NewLine;
                query += " from PartsTransLocation " + Environment.NewLine;
                //Where
                query1 += " DealerCode = '" + HttpContext.Current.Session["DealerCode"].ToString() + "' " + Environment.NewLine;
                //query1 += " AND PartsLocCode = '" + txtpartLocCode + "' " + Environment.NewLine;
                //query1 += " AND PartsSubLocCode = '" + PartsSubLocCode + "' " + Environment.NewLine;

                HttpContext.Current.Session["ColumnsName"] = query;
                AppendInWhere = query1;
                HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
                ShowLookUp(obj, lookupid, RootURL);

                //LookUp.Sql = "select Right(PartsTransLocCode, 3) as PartsTransLocCode, PartsTransLocDesc from PartsTransLocation"
                //LookUp.Append = "AND PartsSubLocCode = '" & txtPartsSubLocCode._String.Trim & "' AND PartsLocCode = '" & txtPartsLocCode._String.Trim & "' AND DealerCode = '" & DEALER_CODE & "'"
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LU_Get_FFIPDIBills(Control obj, string lookupid, string AppendInWhere, string RootURL)
        {
            /* Farhan Raees  */
            /* FFI/PDI Billing   */
            HttpContext.Current.Session["AppendOrderby"] = string.Empty;
            try
            {
                string AppendOrderby;
                HttpContext.Current.Session["ColumnsName"] = "Select FPBillNo, dbo.getNewDate(BillDate) As BillDate, Remarks, BillAmount, SSTNo,SSTDate From FFIPDIBillMaster ";
                AppendInWhere = " DealerCode IN ('" + HttpContext.Current.Session["DealerCode"].ToString() + "','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
                AppendOrderby = "Order by FPBillNo Desc";
                HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
                HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
                ShowLookUp(obj, lookupid, RootURL);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    //public void LU_Get_VehicleCode(Control obj, string lookupid, string AppendInWhere,string RootURL)
    //{
    //    //AA// Get vehicle code
    //    Session["AppendOrderby"] = string.Empty;
    //    HttpContext.Current.Session["ColumnsName"] = "Select ProdCode,VersionCode,ProdDesc from Vehicle";
    //    AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
    //    HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
    //    ShowLookUp(obj, lookupid,RootURL);
    //}

        public void LU_Get_ExpMaster(Control obj, string lookupid, string AppendInWhere, string RootURL)
        {
            //AA// Get vehicle code
            HttpContext.Current.Session["AppendOrderby"] = string.Empty;
            string AppendOrderby;
            HttpContext.Current.Session["ColumnsName"] = "Select EM.ExpendCode,EM.ExpendDate,EM.ChasisNo,V.DisplayName From ExpenditureMaster EM Inner Join VehicleStock V ON EM.DealerCode=V.DealerCode and EM.ChasisNo=V.ChasisNo";
            AppendInWhere = "EM.DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
            AppendOrderby = "Order by EM.ExpendCode Desc";
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
            ShowLookUp(obj, lookupid, RootURL);
        }

    public void CustomerVehLookup(Control obj, string lookupid, string AppendInWhere,string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select CusCode As 'Customer Code',NIC, Phone1 as Phone,CusDesc As 'Name' from Customer";
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid,RootURL);
    }
    public void SaleInvNoLookup(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "Select a.SaleInvNo,a.SaleInvDate,b.CusDesc, a.PostFlag from CounterSaleMaster a inner join Customer b On a.DealerCode=b.DealerCode And a.CusCode = b.CusCode ";
        AppendInWhere = " a.DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void SRJobCardNoLookup(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "Select a.JobCardCode,a.RecDate,b.CusDesc, a.RegNo from JobCardMaster a inner join Customer b On a.DealerCode = b.DealerCode And a.CusCode = b.CusCode ";
        AppendInWhere = " a.DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void CustomerVehLookupForSalesRet(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select CusCode, CusDesc from Customer ";
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    //public void LU_Get_DealerEmp(Control obj, string lookupid, string AppendInWhere,string RootURL)
    //{
    //    //AA//Get dealer Emp
    //    HttpContext.Current.Session["ColumnsName"] = "Select EmpCode As 'Code',EmpName As 'Name' from DealerEmp";
    //    AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
    //    HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
    //    ShowLookUp(obj, lookupid,RootURL);
    //}
    public void LU_Get_BookingRef(Control obj, string lookupid, string AppendInWhere,string RootURL)
    {
        //AA//
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["ColumnsName"] = "Select BO.BookRefNo,Cus.CusDesc,dbo.getNewDate(BO.TentativeDate) As TentativeDate,BO.DisplayName From BookOrdMaster BO inner Join Customer Cus On BO.CusCode=Cus.CusCode And BO.DealerCode=Cus.DealerCode ";
        AppendInWhere = " BO.DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        AppendOrderby = "Order by BO.BookRefNo Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid,RootURL);
    }

    public void LU_Get_AllocationVeh(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Allocation Lookup
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "SELECT A.RecNo,A.EngineNo,A.ChasisNo,isnull(B.ERPProdCode,A.ProdCode) as [ERPProdCode],isnull(B.ERPVersionCode,A.VersionCode) as [ERPVersionCode]  FROM ProdRecDetail A Left Outer Join VersionMapping B  On A.ProdCode=B.ProdCode and A.VersionCode=B.VersionCode   ";
        AppendInWhere = " A.DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }

   

    public void LU_Get_BookingReceipt(Control obj, string lookupid, string AppendInWhere,string RootURL)
    {
        //AA//Get Booing Receipt Lookup
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["ColumnsName"] = "Select ReceiptNo, dbo.getNewDate(ReceiptDate) As ReceiptDate, ReceiptAmount FROM BookingReceipt ";
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere ;
        AppendOrderby = "Order by ReceiptNo Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_JobCardData(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Job Card Data Lookup
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select JobCardCode, Convert(varchar(10) ,RecDate,103) As RecDate, RegNo From JobCardMaster ";
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_ComplainData(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Job Card Data Lookup
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "Select ComplainCode, ComplainDesc from ComplainType ";
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_DefectData(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Defect Data Lookup
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select DefectCode, DefectDesc from Defect ";
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    
    public void LU_Get_PartsData(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Defect Data Lookup
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "SELECT ItemCode, PartItemNo, ItemDesc FROM Item ";
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    //public void LU_Get_VendorData(Control obj, string lookupid, string AppendInWhere, string RootURL)
    //{
    //    //AA//Get Defect Data Lookup
    //    HttpContext.Current.Session["ColumnsName"] = "SELECT Vendorcode,VendorDesc FROM Vendor ";
    //    AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
    //    HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
    //    ShowLookUp(obj, lookupid, RootURL);
    //}
    public void LU_Get_PPRData(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Defect Data Lookup
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["ColumnsName"] = "select PPRCode, Convert(char(2),datepart(dd,PPRDate)) + '/' + convert(char(2),datepart(mm, PPRDate)) + '/' + convert(char(4),datepart(yyyy,PPrDate)) as Date, VehRegNo,JobCardCode from pprmaster ";
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        AppendOrderby = "Order by PPRCode Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_WarrantyLaborData(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Defect Data Lookup
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select  LaborCode,LaborDesc,ProdCode,Rate From WarrantyLabor ";
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    //public void LU_Get_PaymentMode(Control obj, string lookupid, string AppendInWhere,string RootURL)
    //{
    //    //AA//Get Instrument Type Lookup
    //    HttpContext.Current.Session["ColumnsName"] = "Select PayModeCode,PayModeDesc  from PaymentMode ";
        
    //    HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
    //    ShowLookUp(obj, lookupid,RootURL);
    //}
    //public void LU_Get_Bank(Control obj, string lookupid, string AppendInWhere,string RootURL)
    //{
    //    //AA//Get Instrument Type Lookup
    //    HttpContext.Current.Session["ColumnsName"] = "Select BankCode,BankDesc  from Bank ";

    //    HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
    //    ShowLookUp(obj, lookupid,RootURL);
    //}
    
    //public void LU_Get_City(Control obj, string lookupid, string AppendInWhere,string RootURL)
    //{
    //    AA//Get Instrument Type Lookup
    //    HttpContext.Current.Session["ColumnsName"] = "Select CityCode,CityDesc  from City ";

    //    HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
    //    ShowLookUp(obj, lookupid,RootURL);
    //}
    public void LU_Get_BOMst(Control obj, string lookupid, string AppendInWhere,string RootURL)
    {
        //AA//Get Booing Receipt Lookup
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["ColumnsName"] = "Select BO.BookRefNo,dbo.getNewDate(BO.BookRefDate) As BookRefDate,Cus.CusDesc From BookOrdMaster BO Inner Join Customer Cus On BO.DealerCode=Cus.DealerCode And BO.CusCode=Cus.CusCode ";
        AppendInWhere = " BO.DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere ;
        AppendOrderby = "Order by BO.BookRefNo Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid,RootURL);
    }
    public void LU_Get_Customer(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Customer Lookup
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["ColumnsName"] = "Select CusCode,CusDesc From Customer ";
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere ;
        AppendOrderby = "Order by CusCode Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }

    public void LU_Get_Dealer(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Dealer Lookup
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "Select DealerCode,DealerDesc From Dealer ";
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }

    public void LU_Get_BookingVehRec(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Booking No Lookup on VehicleReceipt Form
        string sQuery =string.Empty ;
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;

        sQuery = "Select M.BookingNo , M.BookRefNo,  IsNull(Sum(D.Qty - D.RecQty), 0) RemainingQuantity ";
        sQuery+="from BookOrdMaster M ";
        sQuery+="Inner Join BookOrdVehDetail D ";
        sQuery+="on M.DealerCode = D.DealerCode ";
        sQuery+="And M.BookRefNo = D.BookRefNo ";
        sQuery+="AND (Isnull(D.Qty, 0) > Isnull(D.RecQty, 0)) ";
        //sQuery+="and M.DealerCode = '"+ HttpContext.Current.Session["DealerCode"].ToString() +"' ";
        //sQuery+="and RTrim(LTrim(isnull(M.BookingNo,'')))<>'' ";
        //sQuery+="Group by  M.BookRefNo,M.BookingNo ";


        HttpContext.Current.Session["ColumnsName"] = sQuery;
        AppendInWhere = " M.DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_PordRecMst(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Prod Rec Master Lookup
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string sQuery = string.Empty;
         string AppendOrderby;
         sQuery = "Select A.RecNo, dbo.getNewDate(A.RecDate) As RecDate, B.ChasisNo, C.CusDesc ";
        sQuery += " From  ProdRecMaster A, ";
        sQuery += "ProdRecDetail B, ";
        sQuery += " Customer      C ";
        HttpContext.Current.Session["ColumnsName"] = sQuery;
        AppendInWhere = " A.DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        AppendOrderby = "Order by A.RecNo Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
        
    }
    public void LU_Get_RecVeh(Control obj, string lookupid, string AppendInWhere, string sQuery, string RootURL)
    {
        //AA//
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = sQuery;
        AppendInWhere = " V.DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_ProdCode(Control obj, string lookupid, string AppendInWhere, string sQuery, string RootURL, string AppendOrderby = null)
    {
        //AA//
        //string AppendOrderby;
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = sQuery;
        AppendInWhere = " A.DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        //AppendOrderby = "Order by a.DeliveryNo Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
        
    }
    public void LU_Get_Location(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string sQuery = "Select LocCode, LocDesc FROM Location";
        HttpContext.Current.Session["ColumnsName"] = sQuery;
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_GatePass(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//
        string sQuery = string.Empty;
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        sQuery = "Select distinct GatepassNoPSMC,ProductCode,VersionCode,ChassisNo from vehicledelivery ";
        HttpContext.Current.Session["ColumnsName"] = sQuery;
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_DO_Receiver (Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//
        string sQuery = string.Empty;
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        sQuery = "Select ReceiverCode,ReceiverDesc, ReceiverNIC,ReceiverAdd1  from Receiver ";
        HttpContext.Current.Session["ColumnsName"] = sQuery;
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    //public void LU_Get_DO_Reason(Control obj, string lookupid, string AppendInWhere, string RootURL)
    //{
    //    //AA//
    //    string sQuery = string.Empty;
    //    Session["AppendOrderby"] = string.Empty;
    //    sQuery = "Select DONotCode,DONotReason from DOReason ";
    //    HttpContext.Current.Session["ColumnsName"] = sQuery;
    //    AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
    //    HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
    //    ShowLookUp(obj, lookupid, RootURL);
    //}
    #endregion

    #region //Taha Zubair//

    public void FQIinfo(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string SelectClause = " SELECT cv.RegNo AS [Reg No], cv.EngineNo AS [Engine No], cv.ChassisNo AS [Chassis No],cv.CusCode As [Customer Code]," +
        "c.CusDesc + ' ' + c.FatherHusName AS Customer, cv.ProdCode AS [Prod Code], cv.VersionCode AS Version, cv.INVOICEDATE AS [Invoice Date] FROM  CustomerVehicle cv LEFT OUTER JOIN " +
        "Customer c ON cv.CusCode = c.CusCode and cv.DealerCode=c.DealerCode";
        AppendInWhere = "cv.DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "')";
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }

    public void PartNo_Desc(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "Select" +
                        "    ItemCode as [Item Code]," +
                        "    PartItemNo as [Part Item No.]," +
                        "    ItemDesc as [Part Name]" +
                        "    From" +
                        "    Item ";
                      
        AppendInWhere = " DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') ";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = "ORDER BY ItemCode asc";
        ShowLookUp(obj, lookupid, RootURL);
    }

    public void GetFromFIRMaster(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        HttpContext.Current.Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        string SelectClause = "Select FIRCode,dbo.getNewDate(FIRDate) As FIRDate,RegNo,ChassisNo,EngineNo From FIRMaster";        
        
        AppendInWhere = "DelFlag = 'N' AND DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "')";
        AppendOrderby = "Order by FIRCode Desc";
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }

    #endregion

    public void LU_Get_PPR_For_Approval(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA// Get vehicle code
        HttpContext.Current.Session["ColumnsName"] = "Select PPRM.DealerCode,PPRM.DealerCode + '  ' +	'|' + '  '+ D.DealerDesc As DealerDesc,	PPRM.PPRDate,PPRM.PPRCode,PPRM.TotalAmount As PPRAmount,PPRM.VehChassisNo From PPRMaster PPRM Inner join Dealer D ON PPRM.DealerCode=D.DealerCode";
        AppendInWhere = "PPRM.DealerCode IN ('AAAAA','" + HttpContext.Current.Session["DealerCode"].ToString() + "') " + AppendInWhere;
        AppendInWhere += " and PPRM.PostFlag='Y' and PPRM.TransferStatus='E' and PPRM.Status='X'";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }

    public void LU_Get_PPR_For_Approval_DealerCode(Control obj, string lookupid, string AppendInWhere, string DealerCode, string RootURL)
    {
        //AA// Get vehicle code
        HttpContext.Current.Session["ColumnsName"] = "Select PPRM.DealerCode,PPRM.DealerCode + '  ' +	'|' + '  '+ D.DealerDesc As DealerDesc,	PPRM.PPRDate,PPRM.PPRCode,PPRM.TotalAmount As PPRAmount,PPRM.VehChassisNo From PPRMaster PPRM Inner join Dealer D ON PPRM.DealerCode=D.DealerCode";
        AppendInWhere = "PPRM.DealerCode IN ('AAAAA','" + DealerCode.ToString() + "') " + AppendInWhere;
        AppendInWhere += " and PPRM.PostFlag='Y' and PPRM.TransferStatus='E' and PPRM.Status='X'";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }

}