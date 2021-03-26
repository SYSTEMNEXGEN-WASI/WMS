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
public class clsLookUp : PageBase 
{
    //ClsSessionInfo cls = new ClsSessionInfo();
    public clsLookUp()
	{       
        
	}
    public void ShowLookUp(Control obj, string lookupid, string RootURL)
    {     
       
        StringBuilder builder = new StringBuilder();
        if(lookupid == "10")
        {
            builder.Append("window.open('" + RootURL + "LookUP.aspx?lpi=" + lookupid + "','OpenWindow','width=800, height=600, top=80, left=150, toolbar=0, menubar=0, location=1, status=1, scrollbars=1, resizable=0');");
            base.GetType();
            ScriptManager.RegisterClientScriptBlock(obj, obj.GetType(), "LookUP", builder.ToString(), true);
        } else
        {
            builder.Append("window.open('" + RootURL + "LookUpGeneral.aspx?lpi=" + lookupid + "','OpenWindow','width=800, height=600, top=80, left=150, toolbar=0, menubar=0, location=1, status=1, scrollbars=1, resizable=0');");
            base.GetType();
            ScriptManager.RegisterClientScriptBlock(obj, obj.GetType(), "LookUpGeneral", builder.ToString(), true);
        }
        
    }

    public void ShowLookUpforParts(Control obj, string lookupid, string RootURL, string AppendInGroupBy)
    {
        Session["AppendOrderby"] = string.Empty;
        StringBuilder builder = new StringBuilder();
        if (lookupid == "10")
        {
            builder.Append("window.open('" + RootURL + "LookUP.aspx?lpi=" + lookupid + "&Grp=" + AppendInGroupBy + "','OpenWindow','width=1000, height=500, top=80, left=150, toolbar=0, menubar=0, location=1, status=1, scrollbars=1, resizable=0');");
            base.GetType();
            ScriptManager.RegisterClientScriptBlock(obj, obj.GetType(), "LookUP", builder.ToString(), true);
        } else
        {
            builder.Append("window.open('" + RootURL + "LookUpGeneral.aspx?lpi=" + lookupid + "&Grp=" + AppendInGroupBy + "','OpenWindow','width=1000, height=500, top=80, left=150, toolbar=0, menubar=0, location=1, status=1, scrollbars=1, resizable=0');");
            base.GetType();
            ScriptManager.RegisterClientScriptBlock(obj, obj.GetType(), "LookUpGeneral", builder.ToString(), true);
        }
    }


    public void LU_Get_VehicleCode(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA// Get vehicle code
        Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["ColumnsName"] = "Select ProdCode,VersionCode,ProdDesc from Vehicle";
        AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_VehicleStock(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA// Get vehicle code
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "Select EngineNo,ChasisNo,ProdCode,BookingNo from VehicleStock";
        AppendInWhere = "DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    #region // Muhammad Javed // Lookup Methods for SERVICE MODULE
    //MJ// Get Customer 
    public void CustomerLookup(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["ColumnsName"] = "select cusCode as Code,CusDesc+' '+FatherHusName as [Description], Address1+' '+Address2+' '+Address3 as [Address],isnull(phone1,isnull(phone2,CellNo)) as Phone from Customer";
        AppendInWhere = " DealerCode IN ('AAAAA','" + Session["DealerCode"].ToString() + "') " + AppendInWhere;
        AppendOrderby = "Order by CusCode Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }

    //MJ// JobType | JobTypeDesc
    public void LU_Get_JobType(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select JobTypeCode as [Job Type Code],JobTypeDesc as [Job Type Description] from JobTypeMaster";
        AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') ";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }

    //MJ// Vendor | Vendor Desription
    public void LU_Get_Vendor(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select VendorCode as [Vendor Code],VendorDesc as [Vendor Desription] from vendor";
        AppendInWhere = " DealerCode IN ('AAAAA','" + Session["DealerCode"].ToString() + "') ";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    //MJ// Schedule KM | Product | Version
    public void LU_Get_ScheduleKM(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select KM as [Schedual KM],ProdCode as [Product],Version from MaintainenceSchedule";
        AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') ";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }

    //MJ// Get RegNo,ChassisNo and EngineNo    
    public void RegChassisEngine(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
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
        AppendInWhere ="z.DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "')";
        AppendOrderby = " order by z.dt desc";
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }

    //MJ// JobCard Code

    /// <summary>
    //
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="lookupid"></param>
    /// <param name="AppendInWhere"></param>
    /// <param name="RootURL"></param>
    ///
    /// 
    /// Invoice Job Card
   
    public void LU_Get_JobCardNo_Invoice(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {

        //string SelectClause = "SELECT J.JobCardCode, J.RegNo, J.UserName, JT.JobTypeCode " +
        //                "    FROM JobCardMaster J LEFT OUTER JOIN " +
        //                "    JobTypeMaster JT ON J.JobTypeCode = JT.JobTypeCode and J.Gatepass='' ";
        string AppendOrderby;
    Session["AppendOrderby"] = string.Empty;
        string SelectClause = "SELECT J.JobCardCode as [Job Card No], J.RegNo[Reg No], J.UserName [Customer] , JT.JobTypeDesc as 'JobCard Type' " +
                        "    FROM JobCardMaster J LEFT OUTER JOIN " +
                        "    JobTypeMaster JT ON J.JobCardType = JT.JobTypeCode  ";

        //Append in where clause and J.JobCardType <> '008'
        AppendInWhere = " J.DealerCode IN ('AAAAA','" + Session["DealerCode"].ToString() + "')  And  J.Gatepass='' And DelFlag='N' "+
            "and J.JobCardType <> '001' and J.JobCardType <> '002' " +
            "and J.JobCardType <> '007' " +
            "and J.JobCardType <> '013' "+
            "and J.JobCardType <> '018' " +
            "and J.JobCardType <> '019' " +
            "and J.JobCardType <> '014' ";
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        AppendOrderby = "Order by J.JobCardCode Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUpforParts(obj, lookupid, RootURL, AppendOrderby);
}
public void LU_Get_JobCardNo(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {

        //string SelectClause = "SELECT J.JobCardCode, J.RegNo, J.UserName, JT.JobTypeCode " +
        //                "    FROM JobCardMaster J LEFT OUTER JOIN " +
        //                "    JobTypeMaster JT ON J.JobTypeCode = JT.JobTypeCode and J.Gatepass='' ";
        string AppendOrderby;
        Session["AppendOrderby"] = string.Empty;
        string SelectClause = "SELECT J.JobCardCode as [Job Card No], J.RegNo[Reg No],J.CusCode [CustomerNo], J.UserName [Customer],J.ManJobCardNo [Manual JobCard No],JT.JobTypeDesc [JobCard Type]" +
                        "    FROM JobCardMaster J LEFT OUTER JOIN " +
                        "    JobTypeMaster JT ON JT.JobTypeCode = J.JobCardType  ";

        //Append in where clause
        AppendInWhere = " J.DealerCode IN ('AAAAA','" + Session["DealerCode"].ToString() + "')  And  J.Gatepass='' And DelFlag='N' and J.Jobtypecode <> '001' and J.JobTypeCode <> '002' and J.JobCardType <> 'Warranty' and J.JobCardType <> 'Repeated JobCard'  ";
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        AppendOrderby = "Order by J.JobCardCode Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUpforParts(obj, lookupid, RootURL, AppendOrderby);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="lookupid"></param>
    /// <param name="AppendInWhere"></param>
    /// <param name="RootURL"></param>
    /// JobCard Selection At DTR
    public void LU_Get_DTR_JobCardNo(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {

        //string SelectClause = "SELECT J.JobCardCode, J.RegNo, J.UserName, JT.JobTypeCode " +
        //                "    FROM JobCardMaster J LEFT OUTER JOIN " +
        //                "    JobTypeMaster JT ON J.JobTypeCode = JT.JobTypeCode and J.Gatepass='' ";
        string AppendOrderby;
        Session["AppendOrderby"] = string.Empty;
        string SelectClause = "SELECT J.JobCardCode as [Job Card No], J.RegNo[Reg No], J.UserName [Customer]" +
                        "    FROM JobCardMaster J LEFT OUTER JOIN " +
                        "    JobTypeMaster JT ON J.JobTypeCode = JT.JobTypeCode  "+
                        "LEFT OUTER JOIN " +
                        "    FIRPartsDetail FP ON FP.JobCardCode = J.JobCardCode and  "+
                        "J.DealerCode=FP.DealerCode";

        //Append in where clause
        AppendInWhere = " J.DealerCode IN ('COMON','" + Session["DealerCode"].ToString() + "')  And  J.Gatepass='' And J.DelFlag='N' and J.JobCardType = '008' and J.JobCardType <> 'Repeated JobCard'"+
            "and (Select COUNT(JobCardCode) from JobCardPartsDetail where J.JobCardCode=JobCardCode and DealerCode=J.DealerCode)>(Select COUNT(JobCardCode) from FIRPartsDetail where FP.JobCardCode=JobCardCode and DealerCode=FP.DealerCode)";
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        AppendOrderby = "Order by J.JobCardCode Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUpforParts(obj, lookupid, RootURL, AppendOrderby);
    }
    /// <summary>
    /// Gate Pass
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="lookupid"></param>
    /// <param name="AppendInWhere"></param>
    /// <param name="RootURL"></param>
    /// 
    public void LU_Get_GatePass(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {

        //string SelectClause = "SELECT J.JobCardCode, J.RegNo, J.UserName, JT.JobTypeCode " +
        //                "    FROM JobCardMaster J LEFT OUTER JOIN " +
        //                "    JobTypeMaster JT ON J.JobTypeCode = JT.JobTypeCode and J.Gatepass='' ";
        string AppendOrderby;
        Session["AppendOrderby"] = string.Empty;
        string SelectClause = "Select "+
"GatePassCode as 'GatePass No'"+
",FORMAT(GatePassDate, 'dd-MM-yyyy') as GatePassDate"+
",case when GatePassType = 'N' then 'Non Returnable'"+
" else 'Returnable' end GatePassType"+
" ,case when Module = 'Service' then 'Invoice'"+
" when Module = 'Service/JC' then 'Job Card'"+
"when Module = 'Spare Part' then 'Counter Sale Part'"+
" else 'Stock Adj Part' end 'Trans Type'"+
" from GatePassTemp ";

        //Append in where clause
        AppendInWhere = " DealerCode IN ('AAAAA','" + Session["DealerCode"].ToString() + "')   And DelFlag='N'   ";
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        AppendOrderby = "Order by GatePassCode Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUpforParts(obj, lookupid, RootURL, AppendOrderby);
    }
    public void LU_Get_JobCardNoForm(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {

        //string SelectClause = "SELECT J.JobCardCode, J.RegNo, J.UserName, JT.JobTypeCode " +
        //                "    FROM JobCardMaster J LEFT OUTER JOIN " +
        //                "    JobTypeMaster JT ON J.JobTypeCode = JT.JobTypeCode and J.Gatepass='' ";
        string AppendOrderby;
        Session["AppendOrderby"] = string.Empty;
        string SelectClause = "SELECT J.JobCardCode, J.RegNo, J.UserName " +
                        "    FROM JobCardMaster J LEFT OUTER JOIN " +
                        "    JobTypeMaster JT ON J.JobTypeCode = JT.JobTypeCode  ";

        //Append in where clause
        AppendInWhere = " J.DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "')  And DelFlag='N' and J.GatePass=''  ";
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        AppendOrderby = "Order by J.JobCardCode Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }
    //DTR
    public void LU_Get_DTRForm(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {

        //string SelectClause = "SELECT J.JobCardCode, J.RegNo, J.UserName, JT.JobTypeCode " +
        //                "    FROM JobCardMaster J LEFT OUTER JOIN " +
        //                "    JobTypeMaster JT ON J.JobTypeCode = JT.JobTypeCode and J.Gatepass='' ";
        string AppendOrderby;
        Session["AppendOrderby"] = string.Empty;
        string SelectClause = "SELECT F.FIRCode as DTRNo,J.JobCardCode, J.RegNo, J.UserName     FROM FIRMaster F Inner join JobCardMaster J on F.RoNo = J.JobCardCode and F.DealerCode=J.DealerCode LEFT OUTER JOIN JobTypeMaster JT ON J.JobTypeCode = JT.JobTypeCode ";

        //Append in where clause
        AppendInWhere = " J.DealerCode IN ('COMON','" + Session["DealerCode"].ToString() + "')  And J.DelFlag='N' and J.GatePass='' and F.DelFlag='N' and F.ApprovalFlag='Y'  ";
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        AppendOrderby = "Order by J.JobCardCode Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }
    //WarrantyLabour
    public void LU_Get_WarrantyLabour(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {

        //string SelectClause = "SELECT J.JobCardCode, J.RegNo, J.UserName, JT.JobTypeCode " +
        //                "    FROM JobCardMaster J LEFT OUTER JOIN " +
        //                "    JobTypeMaster JT ON J.JobTypeCode = JT.JobTypeCode and J.Gatepass='' ";
        string AppendOrderby;
        Session["AppendOrderby"] = string.Empty;
        string SelectClause = "SELECT LaborCode as OperationalCode,LaborDesc as OperationalName, Hr as TimeHr,Rate as OperationalRate  FROM WarrantyLabor";

        //Append in where clause
        AppendInWhere = " DealerCode IN ('COMON','" + Session["DealerCode"].ToString() + "') and ProdCode='"+Session["ProdCode"].ToString()+"' ";
        AppendOrderby = "   Group By   " +
 "   A.OperationalCode,    " +
 "   A.OperationalName,   " +
  "   A.TimeHr,   " +
 "   A.OperationalRate,   " ;
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        AppendOrderby = "";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUpforParts(obj, lookupid, RootURL, AppendOrderby);
    }

    //MJ// Job

    public void LU_Get_CustomerEstimateNoForm(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {

        //string SelectClause = "SELECT J.JobCardCode, J.RegNo, J.UserName, JT.JobTypeCode " +
        //                "    FROM JobCardMaster J LEFT OUTER JOIN " +
        //                "    JobTypeMaster JT ON J.JobTypeCode = JT.JobTypeCode and J.Gatepass='' ";
        string AppendOrderby;
        Session["AppendOrderby"] = string.Empty;
        //string SelectClause = "SELECT CE.CustomerEstimateCode, CE.RegNo, CE.UserName " +
          //              "    FROM CustomerEstimateMaster CE INNER JOIN " +
            //            "    CustomerEstimateDetail CED ON CE.CustomerEstimateCode = CED.CustomerEstimateCode  ";
        //Append in where clause
        string SelectClause = "SELECT CE.CustomerEstimateCode as 'Customer Estimate', CE.RegNo as 'Reg No', CE.UserName as 'User Name',CE.Type as 'Type'  " +
                        "    FROM CustomerEstimateMaster CE ";

        AppendInWhere = " CE.DealerCode IN ('AAAAA','" + Session["DealerCode"].ToString() + "')  And DelFlag='N'";
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        AppendOrderby = "Order by CE.CustomerEstimateCode Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_CustomerEstimateNoForm_JC(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {

        //string SelectClause = "SELECT J.JobCardCode, J.RegNo, J.UserName, JT.JobTypeCode " +
        //                "    FROM JobCardMaster J LEFT OUTER JOIN " +
        //                "    JobTypeMaster JT ON J.JobTypeCode = JT.JobTypeCode and J.Gatepass='' ";
        string AppendOrderby;
        Session["AppendOrderby"] = string.Empty;
        //string SelectClause = "SELECT CE.CustomerEstimateCode, CE.RegNo, CE.UserName " +
        //              "    FROM CustomerEstimateMaster CE INNER JOIN " +
        //            "    CustomerEstimateDetail CED ON CE.CustomerEstimateCode = CED.CustomerEstimateCode  ";
        //Append in where clause
        string SelectClause = "SELECT CE.CustomerEstimateCode as 'Customer Estimate', CE.RegNo as 'Reg No', CE.UserName as 'User Name',CE.Type as 'Type'  " +
                        "    FROM CustomerEstimateMaster CE ";

        AppendInWhere = " CE.DealerCode IN ('AAAAA','" + Session["DealerCode"].ToString() + "')  And DelFlag='N' and CE.CustomerEstimateCode not in (Select J.EstimateCode from JobCardMaster J where J.EstimateCode=CE.CustomerEstimateCode and J.DealerCode=CE.DealerCode )";
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        AppendOrderby = "Order by CE.CustomerEstimateCode Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }

    public void LU_Get_Job(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        string SelectClause = "SELECT DJ.DefJobCode as [Job Code], DJ.DefJobDesc as [Job Description], JC.JobCatDesc as [Job Category] FROM DefaultJob DJ LEFT OUTER JOIN " +
                               " JobCategory JC ON DJ.DealerCode = JC.DealerCode And DJ.JobCatCode = JC.JobCatCode  ";
                               
                       
        //Append in where clause
        AppendInWhere = " DJ.DealerCode IN ('COMON','" + Session["DealerCode"].ToString() + "') and DJ.VehicleCategory = '"+Session["VehicleCategory"].ToString()+"'";//order by JobcatDesc";
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }



    //MJ// BrandCode | BrandDescription
    public void LU_Get_Brand(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select BrandCode as [Brand Code],BrandDesc as[Brand Description] from brand";
        AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "')";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL); 
    }



    //MJ// ColorCode | ColorDescription
    public void LU_Get_Color(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select ColorCode as [Color Code],ColorDesc as [Color Description] from Color";
        AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "')";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    //Version
    public void LU_Get_Version(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select VersionCode as[Version] from vehicle";
        AppendInWhere = " DealerCode IN ('AAAAA','" + Session["DealerCode"].ToString() + "')";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }

    //MJ// ProdCode | Version | Description
    public void LU_Get_Product(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "Select ProdCode as Code,ProdDesc as Description,VersionDesc"+
       "from Vehicle P"+
       "inner join Versions V on V.VersionCode = P.VersionCode and V.DealerCode = P.DealerCode";
        AppendInWhere =AppendInWhere+ "And  DealerCode IN ('COMON','" + SessionInformation.DealerCode.ToString() + "')";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    
    //MJ// Invoice No
    public void LU_Get_InvoiceNo(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        string AppendOrderby;
        Session["AppendOrderby"] = string.Empty;
        string SelectClause = " SELECT  CI.InvoiceNo AS [Invoice No], CI.JobCardCode AS [JobCard Code]," +
                        "    dbo.getNewDate(CI.InvoiceDate) AS [Invoice Date], " +
                        "    CI.CusCode as 'CustomerNo',C.CusDesc as 'Customer', JC.RegNo  ," +
                        "    case when CI.InvType = 'Ins' then 'Insurance' when CI.InvType = 'Dep' then 'Insurance Depreciation' else 'Customer' end as 'Type' " +
                        "    FROM CustomerInvoice AS CI inner join JobCardMaster JC on  JC.DealerCode=CI.DealerCode and JC.JobCardCode=CI.JobCardCode" +
                        " inner join Customer C on  C.DealerCode=CI.DealerCode and CI.CusCode=C.CusCode ";
                       
        //Append in where -, DJ.DefJobDesc AS [JobCard Type] -- LEFT OUTER JOIN  DefaultJob AS DJ ON CI.JobCardCode = DJ.DefJobCode - clause
        // AppendInWhere = "  CI.DealerCode = '" + Session["DealerCode"].ToString() +"' And Delflag<>'Y'";
        AppendInWhere = " CI.DealerCode=JC.DealerCode " +
             "And CI.JobCardCode = JC.JobCardCode " +
             "AND CI.DealerCode = '" + Session["DealerCode"].ToString() + "'" +
             "And CI.Delflag<>'Y'  ";
        AppendOrderby = "Order by CI.InvoiceNo Desc";
      //  AppendOrderby = "";

        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUpforParts(obj, lookupid, RootURL, AppendOrderby);
    }
    public void LU_Get_PayableInvoiceNo(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        string AppendOrderby;
        Session["AppendOrderby"] = string.Empty;
        string SelectClause = " Select PurInvNo as 'Inv No' ,Format(PurInvDate,'dd-MM-yyyy') as 'Inv Date' ,GRNNo as 'GRN No' ,VendorDCNo  as 'Vendor DC' ,VendorInvNo as 'Vendor Inv No' "+
            "From PurInvMaster";
       
        AppendInWhere = 
             
             " DealerCode = '" + Session["DealerCode"].ToString() + "'" +
             "And Delflag='N' Order by PurInvNo Desc ";
        // AppendOrderby = "Order by CI.InvoiceNo Desc";
        AppendOrderby = "";

        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }



    //MJ// Part | Part Description
    public void LU_Get_Parts(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        string AppendInGroupBy= string.Empty;
        string SelectClause = "   Select    " +
                        "    A.ItemCode as [Item Code],    " +
                        "    A.PartItemNo as [Part No.],    " +
                        "    A.ItemDesc as [Part Description],   " +
                        "    IsNull(Sum(B.RecQty) + Sum(B.RetQty) + Sum(B.INQty) - Sum(B.IssQty) - Sum(B.ChargeOutQty) - Sum(B.PurRetQty), 0) As [Quantity] " +
                        "    ,A.ProdCode as [Product]  " +

                        "   ,A.Source " +
                        "    From    " +
                        "    Item A   " +
                        "    Left outer join ItemStock B    " +
        "    ON    " +
        "    A.ItemCode = B.ItemCode And A.DealerCode='COMON' And  " +
        "   B.DealerCode in( '" + Session["DealerCode"].ToString() + "') ";
        


        //Append in where clause      "   ,A.PurchaseRate as [Purchase Rate] " +
       // "   ,A.SaleRate as [Sale Rate]" +
AppendInWhere = " A.DealerCode in('"+ Session["DealerCode"].ToString() + "','COMON') and  A.LubeFlag = 'N' and VehicleCategory='"+Session["VehicleCategory"].ToString() +"' ";

        AppendInGroupBy = "   Group By   " +
     "   A.PartItemNo,    " +
     "   A.ItemDesc,   " +
     "   A.UnitCode,   " +
     "   A.ItemCode,   " +
     "   A.ProdCode,   " +
     "   A.PurchaseRate, " +
     "   A.SaleRate, " +
     "   A.Source";


        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        //HttpContext.Current.Session["AppendInGroupBy"] = AppendInGroupBy;
        ShowLookUpforParts(obj, lookupid, RootURL, AppendInGroupBy);
    }

    public void LU_Get_Parts_PO(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        string AppendInGroupBy = string.Empty;
        string SelectClause = "   Select    " +
                        "    A.ItemCode as [Item Code],    " +
                        "    A.PartItemNo as [Item No.],    " +
                        "    A.ItemDesc as [Item Description],   " +
                        "    IsNull(Sum(B.RecQty) + Sum(B.RetQty) + Sum(B.INQty) - Sum(B.IssQty) - Sum(B.ChargeOutQty) - Sum(B.PurRetQty), 0) As [Quantity]    " +
                        "    ,A.ProdCode as [Product]  " +
                        "   ,A.PurchaseRate as [Purchase Rate] " +
                        "   ,A.SaleRate as [Sale Rate]" +
                        "   ,A.Source " +
                         "   ,A.PartsTransLocCode as [Item Location] " +
                        "    From    " +
                        "    Item A   " +
                        "    Left outer join ItemStock B    " +
        "    ON    " +
        "    A.ItemCode = B.ItemCode And    " +
        "   B.DealerCode in('COMON', '" + Session["DealerCode"].ToString() + "')   ";


        //Append in where clause
        AppendInWhere = " A.DealerCode in('COMON' ,'" + Session["DealerCode"].ToString() + "')   and A.VehicleCategory = '"+Session["VehicleCategory"].ToString()+"'"; 

        AppendInGroupBy = "   Group By   " +
     "   A.PartItemNo,    " +
     "   A.ItemDesc,   " +
     "   A.UnitCode,   " +
     "   A.ItemCode,   " +
     "   A.ProdCode,   " +
     "   A.PurchaseRate, " +
     "   A.SaleRate, " +
     "   A.Source,"+
     "   A.PartsTransLocCode ";


        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        //HttpContext.Current.Session["AppendInGroupBy"] = AppendInGroupBy;
        ShowLookUpforParts(obj, lookupid, RootURL, AppendInGroupBy);
    }

    public void LU_Get_Parts_ChargOut(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        string AppendInGroupBy = string.Empty;
        string SelectClause = "   Select * From( Select " +
                        "    A.ItemCode as 'Item Code',    " +
                        "    A.PartItemNo as 'Item No',    " +
                        "    A.ItemDesc as 'Item Description',   " +
                        "    IsNull(Sum(B.RecQty) + Sum(B.RetQty) + Sum(B.INQty) - Sum(B.IssQty) - Sum(B.ChargeOutQty) - Sum(B.PurRetQty), 0) As [Quantity]    " +
                        "    From    " +
                        "    Item A   " +
                        "    Left outer join ItemStock B    " +
        "    ON    " +
        "    A.ItemCode = B.ItemCode And    " +
        "   B.DealerCode = '" + SessionInformation.DealerCode + "' inner join PartsTransLocation C On B.DealerCode=C.DealerCode  And B.PartsTransLocCode=C.PartsTransLocCode    " +
       
        "   Group By A.PartItemNo, A.ItemDesc,A.UnitCode, A.ItemCode ";

        
        //Append in where clause
        AppendInWhere = "   z.Quantity > 0 ";
      //  AppendInGroupBy = "";
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
        Session["AppendOrderby"] = string.Empty;
        string SelectClause = "   Select a.ItemCode,b.PartItemNo,b.ItemDesc,b.UnitCode " +
                        "    from StockIssueDetail a,Item b   ";
       
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        //HttpContext.Current.Session["AppendInGroupBy"] = AppendInGroupBy;
        ShowLookUp(obj, lookupid, RootURL);
    }

    public void LU_Get_AccountCode(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        string SelectClause = "Select A.contacccode +'-'+  A.SubCode +'-'+ A.subsubcode +'-'+ A.loccode +'-'+ A.DetailCode as AccountCode , A.DetailDesc from GDetail  A ";

        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        //HttpContext.Current.Session["AppendInGroupBy"] = AppendInGroupBy;
        ShowLookUp(obj, lookupid, RootURL);
    }

    public void LU_Get_Parts_SaleRet(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        string SelectClause = "   Select a.ItemCode,b.PartItemNo,b.ItemDesc,b.UnitCode " +
                        "    from countersaledetail a inner join Item b On a.ItemCode=b.ItemCode  ";

        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        //HttpContext.Current.Session["AppendInGroupBy"] = AppendInGroupBy;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_Parts_SaleRet_JC(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
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
        Session["AppendOrderby"] = string.Empty;
        string SelectClause = "   Select a.ItemCode,b.PartItemNo,b.ItemDesc,b.UnitCode " +
                        "    from GRNDetail a,Item b   ";

        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        //HttpContext.Current.Session["AppendInGroupBy"] = AppendInGroupBy;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_Lubs(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        string SelectClause = "   Select    " +
                        "    A.ItemCode as [Item Code],    " +
                        "    A.PartItemNo as [Part Item No.],    " +
                        "    A.ItemDesc as [Part Name],   " +
                        "    A.SaleRate,"+
                        "    IsNull(Sum(B.RecQty) + Sum(B.RetQty) + Sum(B.INQty) - Sum(B.IssQty) - Sum(B.ChargeOutQty) - Sum(B.PurRetQty), 0) As [Quantity]    " +
                        "    From    " +
                        "    Item A   " +
                        "    Left outer join ItemStock B    " +
        "    ON    " +
        "    A.ItemCode = B.ItemCode And    " +
        "   B.DealerCode = '" + SessionInformation.DealerCode + "'   ";


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
        Session["AppendOrderby"] = string.Empty;
        string AppendInGroupBy = string.Empty;
        string SelectClause = "   Select    " +
                        "    A.ItemCode as [Item Code],    " +
                        "    A.PartItemNo as [Part Item No.],    " +
                        "    A.ItemDesc as [Part Name],   " +
                        "    IsNull(Sum(B.RecQty) + Sum(B.RetQty) + Sum(B.INQty) - Sum(B.IssQty) - Sum(B.ChargeOutQty) - Sum(B.PurRetQty), 0) As [Quantity]    " +
                        "  From Item A  Left outer join ItemStock B    " + "    ON    " +
        "    A.ItemCode = B.ItemCode And A.DealerCode='COMON' And  " +
        "   B.DealerCode in( '" + Session["DealerCode"].ToString() + "') ";

        //Append in where clause
        AppendInWhere = " A.DealerCode in('" + Session["DealerCode"].ToString() + "','COMON') and  A.LubeFlag = 'Y' and VehicleCategory='" + Session["VehicleCategory"].ToString() + "' ";
        AppendInGroupBy = "   Group By   " +
        "   A.PartItemNo,    " +
        "   A.ItemDesc,   " +
        "   A.UnitCode,   " +
        "   A.ItemCode   ";


        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUpforParts(obj, lookupid, RootURL,AppendInGroupBy);
    }


    public void LU_Get_ConsumeableParts(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        string AppendInGroupBy = string.Empty;
        string SelectClause = "   Select    " +
                        "    A.ItemCode as [Item Code],    " +
                        "    A.PartItemNo as [Part Item No.],    " +
                        "    A.ItemDesc as [Part Name],   " +
                        "    IsNull(Sum(B.RecQty) + Sum(B.RetQty) + Sum(B.INQty) - Sum(B.IssQty) - Sum(B.ChargeOutQty) - Sum(B.PurRetQty), 0) As [Quantity]    " +
                        "    From  Item A  Left outer Join  ItemStock B  " +
                        "    On A.ItemCode = B.ItemCode " +
                        "    And A.DealerCode = B.DealerCode in( '" + Session["DealerCode"].ToString() + "' ,'COMON') ";
        //Append in where clause
        AppendInWhere = "   A.Source not in ('C.K.D.' , 'LOCAL')" +
                        "   And A.DealerCode in( '" + Session["DealerCode"].ToString() + "' ,'COMON')";

        AppendInGroupBy = "   Group By   " +
        "   A.PartItemNo,    " +
        "   A.ItemDesc,   " +
        "   A.UnitCode,   " +
        "   A.ItemCode   ";


        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUpforParts(obj, lookupid, RootURL, AppendInGroupBy);
    }


    //MJ// Get End User
    public void EndUser(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "cusCode as Code,CusDesc+' '+FatherHusName as [Description], Address1+' '+Address2+' '+Address3 as [Address],isnull(phone1,isnull(phone2,CellNo)) as Phone";
        AppendInWhere = " DealerCode IN ('COMON','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }


    #endregion
    #region  
    public void LU_Get_Tax_Policy(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        string SelectClause = "   Select T.TaxAppCode+'|'+TD.TaxType,T.TaxAppDesc from TaxSetupMaster T inner join TaxSetupDetail TD on TD.TaxAppCode=T.TaxAppCode     ";

        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        //HttpContext.Current.Session["AppendInGroupBy"] = AppendInGroupBy;
        ShowLookUp(obj, lookupid, RootURL);
    }

    #endregion

    #region // Muhammad Farhan//
    public void LU_Get_Vendors(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        /* Muhammad Farhan  */
        /* For Vendor  */
        Session["AppendOrderby"] = string.Empty;
        try
        {
            HttpContext.Current.Session["ColumnsName"] = "Select VendorCode,VendorDesc from Vendor ";
            AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
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
        Session["AppendOrderby"] = string.Empty;
        try
        {
            string query = string.Empty;
            string query1 = string.Empty;
            string AppendOrderby;
            if (FormName == "DC")
            {
                query += " Select  " + Environment.NewLine;
                query += " a.DCNo as 'DC No', " + Environment.NewLine;
                query += " convert(varchar(12),a.DCDate,103) as 'DC Date' , " + Environment.NewLine;
                query += " a.PONo, " + Environment.NewLine;
                query += " b.VendorDesc as 'Vendor'  " + Environment.NewLine;
                query += " from DCMaster a,Vendor b " + Environment.NewLine;
                //Where
                query1 += " a.VendorCode = b.VendorCode  " + Environment.NewLine;
                query1 += " and a.DealerCode = '" + Session["DealerCode"].ToString() + "' " + Environment.NewLine;
                query1 += " and a.DelFlag = 'N' " + Environment.NewLine;
                //query1 += " and a.DealerCode = b.DealerCode  " + Environment.NewLine;
                query1 += " and a.VendorCode = b.VendorCode  " + Environment.NewLine;
                query1 += " and b.DealerCode in (a.DealerCode,'COMON') " + Environment.NewLine;
                query1 += " and a.DCNo Not IN  " + Environment.NewLine;
                query1 += " (Select DCNo from GRNMaster Where DealerCode = '" + Session["DealerCode"].ToString() + "'and Delflag = 'N')  " + Environment.NewLine;    
              //  query1 += " and a.DCNo Like '%/%' " + Environment.NewLine;
              

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
                query1 += "  a.DealerCode = '" + Session["DealerCode"].ToString() + "' " + Environment.NewLine;
                query1 += " AND a.DelFlag = 'N' " + Environment.NewLine;
                //query += " 	and a.DealerCode = b.DealerCode " + Environment.NewLine;
                query1 += " AND a.VendorCode = b.VendorCode " + Environment.NewLine;
                query1 += " AND a.DCNo NOT IN ( " + Environment.NewLine;
                query1 += "     	SELECT DCNo " + Environment.NewLine;
                query1 += " 		FROM GRNMaster " + Environment.NewLine;
                query1 += " 		WHERE DealerCode = '" + Session["DealerCode"].ToString()  + "' " + Environment.NewLine;
                query1 += " 		AND Delflag = 'N' " + Environment.NewLine;
                query1 += " 		) " + Environment.NewLine;
             
               

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
        AppendInWhere = "DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
 
    public void LU_Get_PONumber(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        /* Muhammad Farhan  */
        /* For Part Item  */

        string query = string.Empty;
        string query1 = string.Empty;
        Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        try
        {
            query += " Select  " + Environment.NewLine;
            query += " a.POno as 'Purchase Order', " + Environment.NewLine;
            query += " Format(a.PODate,'dd-MM-yyyy') as 'Date', " + Environment.NewLine;
            query += " b.VendorDesc as 'Vendor', " + Environment.NewLine;
            query += " a.DeliveryType   " + Environment.NewLine;
            query += " from POMaster a,Vendor b  " + Environment.NewLine;
            //Where
            query1 += "   a.DealerCode = '" + Session["DealerCode"].ToString() + "' " + Environment.NewLine;
            query1 += " and a.DelFlag = 'N' " + Environment.NewLine;
            query1 += " and a.VendorCode = b.VendorCode " + Environment.NewLine;
            query1 += " and b.DealerCode in (a.DealerCode,'COMON') " + Environment.NewLine;
          //  query1 += " and a.PONo Like '%/%' " + Environment.NewLine;
            
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
            Session["AppendOrderby"] = string.Empty;
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
                query1 += " and a.DealerCode = '" + SessionInformation.DealerCode + "' " + Environment.NewLine;
                query1 += " and a.deliverytype = d.sno  " + Environment.NewLine;
                query1 += " Group By a.PONo,a.PODate,c.VendorDesc, a.deliveryType, d.NName  " + Environment.NewLine;
                query1 += " Having sum(b.POQty) > sum(b.RecQty) " + Environment.NewLine;
               
                
            }
            else if (FormName == "DC")
            {
                query += " Select  " + Environment.NewLine;
                query += " a.POno as 'Purchase Order', " + Environment.NewLine;
                query += " Format(a.PODate,'dd-MM-yyyy') as 'Date', " + Environment.NewLine;
                query += " b.VendorDesc as 'Vendor', " + Environment.NewLine;
                query += " a.Deliverytype  " + Environment.NewLine;
                query += " from POMaster a,Vendor b,PoDetail c " + Environment.NewLine;
                //Where
                query1 += "   a.DealerCode = '" + Session["DealerCode"].ToString() + "' " + Environment.NewLine;
                query1 += " and a.PONo =c.PONo " + Environment.NewLine;
                query1 += " and a.DealerCode = c.DealerCode " + Environment.NewLine;
                query1 += " and a.DelFlag = 'N' " + Environment.NewLine;
                query1 += " and a.VendorCode = b.VendorCode " + Environment.NewLine;
                query1 += " and b.DealerCode in (a.DealerCode,'COMON') " + Environment.NewLine;
                query1 += " and a.transferStatus ='T'  " + Environment.NewLine;
                query1 += " Group By a.PONo,a.PODate,b.VendorDesc, a.deliveryType  " + Environment.NewLine;
                query1 += " Having sum(c.POQty) > sum(c.RecQty) " + Environment.NewLine; 
                //  query1 += " and a.PONo Like '%/%' " + Environment.NewLine;
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
        Session["AppendOrderby"] = string.Empty;
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
                query += "    And B.DealerCode='" + SessionInformation.DealerCode + "' " + Environment.NewLine;
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
        Session["AppendOrderby"] = string.Empty;
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
                query += "    And B.DealerCode='" + SessionInformation.DealerCode + "' " + Environment.NewLine; 
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
                query1 += " a.DealerCode = '" + SessionInformation.DealerCode + "' " + Environment.NewLine;
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
                query1 += " And a.DealerCode='" + SessionInformation.DealerCode + "' " + Environment.NewLine; 
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
        Session["AppendOrderby"] = string.Empty;
        try
        {
            HttpContext.Current.Session["ColumnsName"] = "Select PartsTransLocCode,PartsTransLocDesc From PartsTransLocation  ";
            AppendInWhere = " DealerCode IN ('" + SessionInformation.DealerCode + "','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
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
        Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        try
        {
            

            query += "  Select a.GRNNo as 'GRN No',convert(varchar(20),a.GRnDate,103) as 'GRN Date',PoNo as 'PO No' ,a.DCNo as 'DC NO',b.VendorDesc as 'Vendor'    " + Environment.NewLine;
            query += "  from GRNMaster a,Vendor b   " + Environment.NewLine;
            //Where
            query1 += "  a.DealerCode =  '" + Session["DealerCode"].ToString() + "'  " + Environment.NewLine;
            query1 += "  and a.DelFlag = 'N'  " + Environment.NewLine;
            query1 += "  and b.DealerCode in (a.DealerCode,'COMON')   " + Environment.NewLine;
            query1 += " and a.VendorCode = b.VendorCode   " + Environment.NewLine;

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
    public void LU_Get_GRNNumber(Control obj, string lookupid, string AppendInWhere, string RootURL, string FormName)
    {
        /* Muhammad Farhan  */
        /* For GRN Number */
        string query = string.Empty;
        string query1 = string.Empty;
        Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        try
        {

            if (FormName == "PurInv")
            {
                query += "  Select a.GRNNo as 'GRN No',convert(varchar(20),a.GRnDate,103) as 'GRN Date',PoNo as 'PO No' ,a.DCNo as 'DC NO',b.VendorDesc as 'Vendor'    " + Environment.NewLine;
                query += "  from GRNMaster a,Vendor b   " + Environment.NewLine;
                //Where
                query1 += "  a.DealerCode =  '" + Session["DealerCode"].ToString() + "'  " + Environment.NewLine;
                query1 += "  and a.DelFlag = 'N'  " + Environment.NewLine;
                query1 += "  and b.DealerCode in (a.DealerCode,'COMON')   " + Environment.NewLine;
                query1 += " and a.VendorCode = b.VendorCode   " + Environment.NewLine;
                query1 += " and a.GRNNo Not IN  " + Environment.NewLine;
                query1 += " (Select GRNNo from PurInvMaster Where DealerCode = '" + Session["DealerCode"].ToString() + "'and Delflag = 'N')  " + Environment.NewLine;
                AppendOrderby = "Order by a.GRNNo Desc";
                HttpContext.Current.Session["ColumnsName"] = query;
                AppendInWhere = query1;
                HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
                HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
                ShowLookUp(obj, lookupid, RootURL);
            }
            else
            {
                query += "  Select a.GRNNo as 'GRN No',convert(varchar(20),a.GRnDate,103) as 'GRN Date',PoNo as 'PO No' ,a.DCNo as 'DC NO',b.VendorDesc as 'Vendor'    " + Environment.NewLine;
                query += "  from GRNMaster a,Vendor b   " + Environment.NewLine;
                //Where
                query1 += "  a.DealerCode =  '" + Session["DealerCode"].ToString() + "'  " + Environment.NewLine;
                query1 += "  and a.DelFlag = 'N'  " + Environment.NewLine;
                query1 += "  and b.DealerCode in (a.DealerCode,'COMON')   " + Environment.NewLine;
                query1 += " and a.VendorCode = b.VendorCode   " + Environment.NewLine;
            }
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
        Session["AppendOrderby"] = string.Empty;
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
                query1 += "     AND a.DealerCode = '" + Session["DealerCode"].ToString() + "' " + Environment.NewLine;
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
                query1 += " where a.DealerCode = '" + Session["DealerCode"].ToString() + "' and a.Requsitioner = 'C' And 	a.InvoiceNo <> '' And c.PostFlag = 'N' And c.DelFlag = 'N' And b.Quantity <> b.RecQty" + Environment.NewLine;
                query1 += " order by 1 desc " + Environment.NewLine;

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
        Session["AppendOrderby"] = string.Empty;
        try
        {
            query += " Select  " + Environment.NewLine;
            query += " Distinct a.IssueNo as 'Issue No', " + Environment.NewLine;
            query += " convert(varchar(20),a.IssueDate,103) as 'Issue Date', " + Environment.NewLine;
            query += " JobCardNo, " + Environment.NewLine;
            query += " InvoiceNo " + Environment.NewLine;

            query += " from StockIssue a,StockIssueDetail b " + Environment.NewLine;
            //Where
            query1 += "     a.DealerCode = '" + Session["DealerCode"].ToString() + "'  " + Environment.NewLine;
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
        Session["AppendOrderby"] = string.Empty;
        try
        {
            //query += " Select  " + Environment.NewLine;
            query += " Select distinct  ChargeOutNo as 'Charge Out No',Format(ChargeOutDate,'dd-MM-yyyy') as 'ChargeOut Date', Remarks from ChargeOutMaster  " + Environment.NewLine;
            //query += " convert(varchar(20),a.IssueDate,103) as IssueDate, " + Environment.NewLine;
            //query += " JobCardNo, " + Environment.NewLine;
            //query += " InvoiceNo " + Environment.NewLine;

            //query += " from StockIssue a,StockIssueDetail b " + Environment.NewLine;
            //Where
            query1 += "     DealerCode = '" + Session["DealerCode"].ToString() + "'  " + Environment.NewLine;
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
        Session["AppendOrderby"] = string.Empty;
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
            query1 += "     a.DealerCode = '" + SessionInformation.DealerCode + "'  " + Environment.NewLine;
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
        Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        try
        {
            query += " Select a.SRNo, a.SRDate, b.CusDesc from  SRMaster a inner join  Customer b On a.DealerCode = b.DealerCode  AND a.CusCode = b.CusCode " + Environment.NewLine;
            query1 += " a.DealerCode = '" + SessionInformation.DealerCode + "'  " + Environment.NewLine;
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
        Session["AppendOrderby"] = string.Empty;
        try
        {
            //query += " Select  " + Environment.NewLine;
            query += " Select a.PRNo as 'PR No', Format(a.PRDate,'dd-MM-yyyy') as 'PR Date', b.VendorDesc as 'Vendor' from  PRMaster a Inner Join Vendor b On  a.VendorCode = b.VendorCode and b.DealerCode in(a.DealerCode,'COMON') " + Environment.NewLine;
            //query += " convert(varchar(20),a.IssueDate,103) as IssueDate, " + Environment.NewLine;
            //query += " JobCardNo, " + Environment.NewLine;
            //query += " InvoiceNo " + Environment.NewLine;

            //query += " from StockIssue a,StockIssueDetail b " + Environment.NewLine;
            //Where
            query1 += "     a.DealerCode = '" + Session["DealerCode"].ToString() + "'  " + Environment.NewLine;
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
        Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        try
        {
            query += " Select A.GRNNo as 'GRN No', convert(varchar(20),A.GRNDate,103) As 'GRN Date', B.VendorDesc as 'Vendor' From GRNMaster A, Vendor B " + Environment.NewLine;
            
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
        Session["AppendOrderby"] = string.Empty;
        try
        {
            query = " Select  " + Environment.NewLine;
            query += " Distinct A.IssueNo, A.IssueDate,A.ReqSource, A.JobCardNo, A.InvoiceNo  " + Environment.NewLine;
            query += " From  StockIssue A Inner Join StockIssueDetail B " + Environment.NewLine;
            query += " On 	A.DealerCode = B.DealerCode And A.IssueNo = B.IssueNo  " + Environment.NewLine;
            //query += "  " + Environment.NewLine;

            //query += " from StockIssue a,StockIssueDetail b " + Environment.NewLine;
            //Where
            query1 = "     A.DealerCode = '" + SessionInformation.DealerCode + "'  " + Environment.NewLine;
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
            Session["AppendOrderby"] = string.Empty;
            string AppendOrderby;
            try
            {
                //,,, 
                query += " Select  " + Environment.NewLine;
                query += " a.SaleInvNo as 'Sale Inv No.', " + Environment.NewLine;
                query += "convert(varchar(20),a.SaleInvDate,103) as 'Sale Inv Date' , " + Environment.NewLine;
                query += " b.CusDesc as 'Customer', " + Environment.NewLine;
                query += " a.PostFlag " + Environment.NewLine;
                query += " from CounterSaleMaster a,Customer b  " + Environment.NewLine;
                //Where
                query1 += " a.DealerCode = '" + Session["DealerCode"].ToString() + "' " + Environment.NewLine;
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
            Session["AppendOrderby"] = string.Empty;
            try
            {

                query += "  select PartsLocCode, PartsLocDesc  " + Environment.NewLine;
                query += "   from PartsLocation    " + Environment.NewLine;
                //Where
                query1 += "  DealerCode =  '" + SessionInformation.DealerCode + "'  " + Environment.NewLine;

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
            Session["AppendOrderby"] = string.Empty;
            try
            {

                query += "  select PartsSubLocCode, PartsSubLocDesc   " + Environment.NewLine;
                query += "   from PartsSubLocation   " + Environment.NewLine;
                //Where
                query1 += "  DealerCode =  '" + SessionInformation.DealerCode + "'  " + Environment.NewLine;
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
            Session["AppendOrderby"] = string.Empty;
            try
            {

                query += "  select PartsLocCode,PartsSubLocCode,Right(PartsTransLocCode, 3) as PartsTransLocCode, PartsTransLocDesc   " + Environment.NewLine;
                query += "   from PartsTransLocation   " + Environment.NewLine;
                //Where
                query1 += "  DealerCode =  '" + SessionInformation.DealerCode + "'  " + Environment.NewLine;
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
            Session["AppendOrderby"] = string.Empty;
            string query = string.Empty;
            string query1 = string.Empty;
            try
            {
                query += " select PartsTransLocCode, PartsTransLocDesc " + Environment.NewLine;
                query += " from PartsTransLocation " + Environment.NewLine;
                //Where
                query1 += " DealerCode = '" + SessionInformation.DealerCode + "' " + Environment.NewLine;
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
            Session["AppendOrderby"] = string.Empty;
            try
            {
                string AppendOrderby;
                HttpContext.Current.Session["ColumnsName"] = "Select FPBillNo, dbo.getNewDate(BillDate) As BillDate, Remarks, BillAmount,"+
                "Case When PostFlag='Y' then 'Posted' else 'Not Posted' end as Post "+
                "From FFIPDIBillMaster ";
                AppendInWhere = " DealerCode IN ('" + Session["DealerCode"].ToString() + "','') " + AppendInWhere;
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


    public void LU_Get_JenBills(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        /* Farhan Raees  */
        /* FFI/PDI Billing   */
        Session["AppendOrderby"] = string.Empty;
        try
        {
            string AppendOrderby;
            HttpContext.Current.Session["ColumnsName"] = "Select JenBillNo as 'Bill No', dbo.getNewDate(BillDate) As BillDate, Remarks, BillAmount," +
            "Case When PostFlag='Y' then 'Posted' else 'Not Posted' end as Post " +
            "From JenBillMaster ";
            AppendInWhere = " DealerCode IN ('" + Session["DealerCode"].ToString() + "','') " + AppendInWhere;
            AppendOrderby = "Order by JenBillNo Desc";
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
    //    AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
    //    HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
    //    ShowLookUp(obj, lookupid,RootURL);
    //}

    public void LU_Get_ExpMaster(Control obj, string lookupid, string AppendInWhere, string RootURL)
        {
            //AA// Get vehicle code
            Session["AppendOrderby"] = string.Empty;
            string AppendOrderby;
            HttpContext.Current.Session["ColumnsName"] = "Select EM.ExpendCode,EM.ExpendDate,EM.ChasisNo,V.DisplayName From ExpenditureMaster EM Inner Join VehicleStock V ON EM.DealerCode=V.DealerCode and EM.ChasisNo=V.ChasisNo";
            AppendInWhere = "EM.DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
            AppendOrderby = "Order by EM.ExpendCode Desc";
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
            ShowLookUp(obj, lookupid, RootURL);
        }

    public void CustomerVehLookup(Control obj, string lookupid, string AppendInWhere,string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select CusCode As 'Customer Code',NIC, Phone1 as Phone,CusDesc As 'Name' from Customer";
        AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid,RootURL);
    }
    public void SaleInvNoLookup(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "Select a.SaleInvNo,a.SaleInvDate,b.CusDesc, a.PostFlag from CounterSaleMaster a inner join Customer b On a.DealerCode=b.DealerCode And a.CusCode = b.CusCode ";
        AppendInWhere = " a.DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void SRJobCardNoLookup(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "Select a.JobCardCode,a.RecDate,b.CusDesc, a.RegNo from JobCardMaster a inner join Customer b On a.DealerCode = b.DealerCode And a.CusCode = b.CusCode ";
        AppendInWhere = " a.DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void CustomerVehLookupForSalesRet(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select CusCode, CusDesc from Customer ";
        AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    //public void LU_Get_DealerEmp(Control obj, string lookupid, string AppendInWhere,string RootURL)
    //{
    //    //AA//Get dealer Emp
    //    HttpContext.Current.Session["ColumnsName"] = "Select EmpCode As 'Code',EmpName As 'Name' from DealerEmp";
    //    AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
    //    HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
    //    ShowLookUp(obj, lookupid,RootURL);
    //}
    public void LU_Get_BookingRef(Control obj, string lookupid, string AppendInWhere,string RootURL)
    {
        //AA//
        Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["ColumnsName"] = "Select BO.BookRefNo,Cus.CusDesc,dbo.getNewDate(BO.TentativeDate) As TentativeDate,BO.DisplayName From BookOrdMaster BO inner Join Customer Cus On BO.CusCode=Cus.CusCode And BO.DealerCode=Cus.DealerCode ";
        AppendInWhere = " BO.DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
        AppendOrderby = "Order by BO.BookRefNo Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid,RootURL);
    }

    public void LU_Get_AllocationVeh(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Allocation Lookup
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "SELECT A.RecNo,A.EngineNo,A.ChasisNo,isnull(B.ERPProdCode,A.ProdCode) as [ERPProdCode],isnull(B.ERPVersionCode,A.VersionCode) as [ERPVersionCode]  FROM ProdRecDetail A Left Outer Join VersionMapping B  On A.ProdCode=B.ProdCode and A.VersionCode=B.VersionCode   ";
        AppendInWhere = " A.DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }

   

    public void LU_Get_BookingReceipt(Control obj, string lookupid, string AppendInWhere,string RootURL)
    {
        //AA//Get Booing Receipt Lookup
        Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["ColumnsName"] = "Select ReceiptNo, dbo.getNewDate(ReceiptDate) As ReceiptDate, ReceiptAmount FROM BookingReceipt ";
        AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere ;
        AppendOrderby = "Order by ReceiptNo Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_JobCardData(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Job Card Data Lookup
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select JobCardCode, Convert(varchar(10) ,RecDate,103) As RecDate, RegNo From JobCardMaster ";
        AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_ComplainData(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Job Card Data Lookup
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "Select ComplainCode, ComplainDesc from ComplainType ";
        AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_DefectData(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Defect Data Lookup
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select DefectCode, DefectDesc from Defect ";
        AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    
    public void LU_Get_PartsData(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Defect Data Lookup
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "SELECT ItemCode, PartItemNo, ItemDesc FROM Item ";
        AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    //public void LU_Get_VendorData(Control obj, string lookupid, string AppendInWhere, string RootURL)
    //{
    //    //AA//Get Defect Data Lookup
    //    HttpContext.Current.Session["ColumnsName"] = "SELECT Vendorcode,VendorDesc FROM Vendor ";
    //    AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
    //    HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
    //    ShowLookUp(obj, lookupid, RootURL);
    //}
    public void LU_Get_PPRData(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Defect Data Lookup
        Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["ColumnsName"] = "select PPRCode, Convert(char(2),datepart(dd,PPRDate)) + '/' + convert(char(2),datepart(mm, PPRDate)) + '/' + convert(char(4),datepart(yyyy,PPrDate)) as Date, VehRegNo,JobCardCode from pprmaster ";
        AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
        AppendOrderby = "Order by PPRCode Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_WarrantyLaborData(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Defect Data Lookup
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "select  LaborCode,LaborDesc,ProdCode,Rate From WarrantyLabor ";
        AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
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
        Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["ColumnsName"] = "Select BO.BookRefNo,dbo.getNewDate(BO.BookRefDate) As BookRefDate,Cus.CusDesc From BookOrdMaster BO Inner Join Customer Cus On BO.DealerCode=Cus.DealerCode And BO.CusCode=Cus.CusCode ";
        AppendInWhere = " BO.DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere ;
        AppendOrderby = "Order by BO.BookRefNo Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid,RootURL);
    }
    public void LU_Get_Customer(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Customer Lookup
        Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["ColumnsName"] = "Select  CusCode as 'Customer Code',CusDesc as Customer From Customer ";
        AppendInWhere = " DealerCode IN ('AAAAA','" + Session["DealerCode"].ToString() + "') "  ;
        AppendOrderby = "Order by CusCode Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_InsComp(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Customer Lookup
        Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["ColumnsName"] = "Select InsCompCode as 'Insurance Company Code',InsCompDescription as 'Company' From InsuranceCompanies ";
        AppendInWhere = " DealerCode IN ('COMON','" + Session["DealerCode"].ToString() + "') ";
        AppendOrderby = "Order by InsCompCode Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_InsCompBr(Control obj, string lookupid, string AppendInWhere, string RootURL,string InsCompCode)
    {
        //AA//Get Customer Lookup
        Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["ColumnsName"] = "Select BranchCode as 'Branch Code',BranchDesc as Branch From Branch ";
        AppendInWhere = " DealerCode IN ('COMON','" + Session["DealerCode"].ToString() + "') and InsCompCode='"+InsCompCode+"' ";
        AppendOrderby = "Order by BranchCode Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_Customer(Control obj, string lookupid, string AppendInWhere,string Type,string InsComp, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        //AA//Get Customer Lookup
        if (Type== "Insurance")
        {
            HttpContext.Current.Session["ColumnsName"] = "Select  Distinct C.CusCode as 'Customer Code',C.CusDesc as Customer,JC.RegNo as RegNo,JC.ChassisNo,JC.EngineNo  From Customer C inner join CustomerInvoice CI on  CI.CusCode=C.CusCode and CI.DealerCode=C.DealerCode  "+
                "inner Join JobCardMaster JC on JC.JobCardCode=CI.JobCardCode and JC.DealerCode=CI.DealerCode ";
            AppendInWhere = " C.DealerCode IN ('AAAAA','" + Session["DealerCode"].ToString() + "') and CI.InvType='Ins' and CI.InsCode='"+InsComp+"' ";
            AppendOrderby = "Order by C.CusCode Desc";
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        }
        else if(Type== "Dep")
        {
            HttpContext.Current.Session["ColumnsName"] = "Select  Distinct C.CusCode as 'Customer Code',C.CusDesc as Customer,JC.RegNo as RegNo,JC.ChassisNo,JC.EngineNo  From Customer C inner join CustomerInvoice CI on  CI.CusCode=C.CusCode and CI.DealerCode=C.DealerCode  " +
                "inner Join JobCardMaster JC on JC.JobCardCode=CI.JobCardCode and JC.DealerCode=CI.DealerCode ";
            AppendInWhere = " CI.DealerCode IN ('AAAAA','" + Session["DealerCode"].ToString() + "') and CI.InvType='Dep' ";
            AppendOrderby = "Order by C.CusCode Desc";
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        }
        else if (Type == "Service")
        {
            HttpContext.Current.Session["ColumnsName"] = "Select  Distinct C.CusCode as 'Customer Code',C.CusDesc as Customer,JC.RegNo as RegNo,JC.ChassisNo,JC.EngineNo  From Customer C inner join CustomerInvoice CI on  CI.CusCode=C.CusCode and CI.DealerCode=C.DealerCode  " +
               "inner Join JobCardMaster JC on JC.JobCardCode=CI.JobCardCode and JC.DealerCode=CI.DealerCode ";
            AppendInWhere = " CI.DealerCode IN ('AAAAA','" + Session["DealerCode"].ToString() + "') and CI.InvType='Inv'  "+
                "AND ROUND(CI.NetAmountCustomer,0) > ROUND(CI.PaidC,0) " +
                   "AND CI.NetAmountCustomer - CI.PaidC > 1 ";
            AppendOrderby = "Order by C.CusCode Desc";
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        }
        else
        {
            HttpContext.Current.Session["ColumnsName"] = "Select CusCode as 'Customer Code',CusDesc as Customer from Customer   ";
            AppendInWhere = " DealerCode IN ('AAAAA','" + Session["DealerCode"].ToString() + "')  ";
            AppendOrderby = "Order by CusCode Desc";
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        }
       
       
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_CustomerPayRec(Control obj, string lookupid, string AppendInWhere, string Type, string InsComp, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        //AA//Get Customer Lookup
        if (Type == "Insurance")
        {
            HttpContext.Current.Session["ColumnsName"] = "Select  Distinct C.CusCode as 'Customer Code',C.CusDesc as Customer,JC.RegNo as RegNo,JC.ChassisNo,CI.NetAmountInsurance-CI.PaidI as Outstanding  From Customer C inner join CustomerInvoice CI on  CI.CusCode=C.CusCode and CI.DealerCode=C.DealerCode  " +
                "inner Join JobCardMaster JC on JC.JobCardCode=CI.JobCardCode and JC.DealerCode=CI.DealerCode ";
            AppendInWhere = " C.DealerCode IN ('AAAAA','" + Session["DealerCode"].ToString() + "') and CI.InvType='Ins' and CI.InsCode='" + InsComp + "' and CI.DelFlag='N' and ROUND(NetAmountInsurance, 2) - ROUND(ISNULL(PaidI, 0), 2) > 1 ";
            AppendOrderby = "Order by C.CusCode Desc";
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        }
        else if (Type == "Dep")
        {
            HttpContext.Current.Session["ColumnsName"] = "Select  Distinct C.CusCode as 'Customer Code',C.CusDesc as Customer,JC.RegNo as RegNo,JC.ChassisNo,CI.NetAmountCustomer-CI.PaidC as Outstanding  From Customer C inner join CustomerInvoice CI on  CI.CusCode=C.CusCode and CI.DealerCode=C.DealerCode  " +
                "inner Join JobCardMaster JC on JC.JobCardCode=CI.JobCardCode and JC.DealerCode=CI.DealerCode ";
            AppendInWhere = " CI.DealerCode IN ('AAAAA','" + Session["DealerCode"].ToString() + "') and CI.InvType='Dep'and ROUND(NetAmountCustomer, 2) - ROUND(ISNULL(PaidC, 0), 2) > 1 and CI.DelFlag='N' ";
            AppendOrderby = "Order by C.CusCode Desc";
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        }
        else if (Type == "Service")
        {
            HttpContext.Current.Session["ColumnsName"] = "Select  Distinct C.CusCode as 'Customer Code',C.CusDesc as Customer,JC.RegNo as RegNo,JC.ChassisNo,CI.NetAmountCustomer-CI.PaidC as Outstanding  From Customer C inner join CustomerInvoice CI on  CI.CusCode=C.CusCode and CI.DealerCode=C.DealerCode  " +
               "inner Join JobCardMaster JC on JC.JobCardCode=CI.JobCardCode and JC.DealerCode=CI.DealerCode ";
            AppendInWhere = " CI.DealerCode IN ('AAAAA','" + Session["DealerCode"].ToString() + "') and CI.InvType='Inv' and ROUND(NetAmountCustomer, 2) - ROUND(ISNULL(PaidC, 0), 2) > 1 and CI.DelFlag='N'  " +
                "AND ROUND(CI.NetAmountCustomer,2) > ROUND(CI.PaidC,2) " +
                   "AND CI.NetAmountCustomer - CI.PaidC > 1 ";
            AppendOrderby = "Order by C.CusCode Desc";
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        }
        else if (Type == "CountrSale")
        {
            HttpContext.Current.Session["ColumnsName"] = "Select  C.CusCode ,C.CusDesc,isnull(CV.RegNo,'')RegNo,isnull(CV.ChassisNo,'')ChassisNo from CounterSaleMaster CI   " +
               "inner join Customer C on C.CusCode=CI.CusCode and C.DealerCode=CI.DealerCode Left Join CustomerVehicle CV on CV.CusCode =C.CusCode and CV.DealerCode=C.DealerCode";
            AppendInWhere = " CI.DealerCode IN ('AAAAA','" + Session["DealerCode"].ToString() + "')  and ROUND(InvoiceAmount, 2) - ROUND(ISNULL(TotReceipt, 0), 2) > 1 and CI.DelFlag='N' and CI.PostFlag='Y'  " +
                "AND ROUND(CI.InvoiceAmount,2) > ROUND(CI.TotReceipt,2) " +
                   "AND CI.InvoiceAmount - CI.TotReceipt > 1 ";
            AppendOrderby = "Order by C.CusCode Desc";
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        }
        else
        {
            HttpContext.Current.Session["ColumnsName"] = "Select CusCode as 'Customer Code',CusDesc as Customer from Customer   ";
            AppendInWhere = " DealerCode IN ('AAAAA','" + Session["DealerCode"].ToString() + "')  ";
            AppendOrderby = "Order by CusCode Desc";
            HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
            HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        }


        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_ReceiptNo(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Customer Lookup
        Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["ColumnsName"] = "Select " +
              "R.ReceiptNo,"+
              "Format(R.ReceiptDate, 'dd-MM-yyyy')ReceiptDate," +
              "C.CusDesc as Customer," +
              "R.TransType," +
              "R.InvoiceType," +
              "Case when R.TransType='Advance' then R.AdvanceAmount else R.InvAdjTotal end as 'Total Amount' "+
              "from PaymentReceiptMaster R " +
              "inner join Customer C on C.CusCode = R.CusCode and C.DealerCode = R.DealerCode";
        AppendInWhere = " R.DealerCode IN ('" + Session["DealerCode"].ToString() + "') and R.DelFlag='N' and R.InvoiceType in ('Insurance','CountrSale','Service','Dep') ";
        AppendOrderby = "order by ReceiptNo desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_RefundNo(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Customer Lookup
        Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["ColumnsName"] = "Select " +
              "R.RefundCode as 'Refund No'," +
              "Format(R.RefundDate, 'dd-MM-yyyy')RefundtDate," +
               "R.AdvanceCode as 'Advance Receipt No'," +
             // "R.ReceiptType," +
              "R.RefundType " +
              // "R.AmountPaid as 'Total Amount' " +
              "from RefundPayment R ";
              //"inner join Customer C on C.CusCode = R.CusCode and C.DealerCode = R.DealerCode";
        AppendInWhere = " R.DealerCode IN ('" + Session["DealerCode"].ToString() + "') and R.DelFlag='N' ";
        AppendOrderby = "order by RefundCode desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_InsAdvanceNo(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Customer Lookup
        Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        HttpContext.Current.Session["ColumnsName"] = "Select " +
              "R.AdvanceID as 'Advance No'," +
              "Format(R.AdvanceDate, 'dd-MM-yyyy')AdvanceDate," +
               "I.InsCompDescription as 'Insurance Company'," +
               "B.BranchDesc as 'Branch' " +
               "from AdvancePayment R " +
               "inner join InsuranceCompanies I on I.InsCompCode=R.InsCompCode and I.DealerCode in ('COMON','"+Session["DealerCode"].ToString()+"')"+
               "inner join Branch B on B.BranchCode=R.BranchCode and B.InsCompCode=R.InsCompCode and B.DealerCode in ('COMON','" + Session["DealerCode"].ToString() + "')"; ;
      
        AppendInWhere = " R.DealerCode IN ('" + Session["DealerCode"].ToString() + "') and R.DelFlag='N' ";
        AppendOrderby = "order by AdvanceID desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
    }

    public void LU_Get_Dealer(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Dealer Lookup
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "Select DealerCode,DealerDesc From Dealer ";
        AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }

    public void LU_Get_BookingVehRec(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Booking No Lookup on VehicleReceipt Form
        string sQuery =string.Empty ;
        Session["AppendOrderby"] = string.Empty;

        sQuery = "Select M.BookingNo , M.BookRefNo,  IsNull(Sum(D.Qty - D.RecQty), 0) RemainingQuantity ";
        sQuery+="from BookOrdMaster M ";
        sQuery+="Inner Join BookOrdVehDetail D ";
        sQuery+="on M.DealerCode = D.DealerCode ";
        sQuery+="And M.BookRefNo = D.BookRefNo ";
        sQuery+="AND (Isnull(D.Qty, 0) > Isnull(D.RecQty, 0)) ";
        //sQuery+="and M.DealerCode = '"+ SessionInformation.DealerCode +"' ";
        //sQuery+="and RTrim(LTrim(isnull(M.BookingNo,'')))<>'' ";
        //sQuery+="Group by  M.BookRefNo,M.BookingNo ";


        HttpContext.Current.Session["ColumnsName"] = sQuery;
        AppendInWhere = " M.DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_PordRecMst(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//Get Prod Rec Master Lookup
        Session["AppendOrderby"] = string.Empty;
        string sQuery = string.Empty;
         string AppendOrderby;
         sQuery = "Select A.RecNo, dbo.getNewDate(A.RecDate) As RecDate, B.ChasisNo, C.CusDesc ";
        sQuery += " From  ProdRecMaster A, ";
        sQuery += "ProdRecDetail B, ";
        sQuery += " Customer      C ";
        HttpContext.Current.Session["ColumnsName"] = sQuery;
        AppendInWhere = " A.DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
        AppendOrderby = "Order by A.RecNo Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
        
    }
    public void LU_Get_RecVeh(Control obj, string lookupid, string AppendInWhere, string sQuery, string RootURL)
    {
        //AA//
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = sQuery;
        AppendInWhere = " A.DealerCode IN ('COMON','" + Session["DealerCode"].ToString() + "') " + AppendInWhere ;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = " ";
        ShowLookUp(obj, lookupid, RootURL);
    }
    public void LU_Get_ProdCode(Control obj, string lookupid, string AppendInWhere, string sQuery, string RootURL, string AppendOrderby = null)
    {
        //AA//
        //string AppendOrderby;
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = sQuery;
        AppendInWhere = " A.DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
        //AppendOrderby = "Order by a.DeliveryNo Desc";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = AppendOrderby;
        ShowLookUp(obj, lookupid, RootURL);
        
    }
    public void LU_Get_Location(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//
        Session["AppendOrderby"] = string.Empty;
        string sQuery = "Select LocCode, LocDesc FROM Location";
        HttpContext.Current.Session["ColumnsName"] = sQuery;
        AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }
    //public void LU_Get_GatePass(Control obj, string lookupid, string AppendInWhere, string RootURL)
    //{
    //    //AA//
    //    string sQuery = string.Empty;
    //    Session["AppendOrderby"] = string.Empty;
    //    sQuery = "Select distinct GatepassNoPSMC,ProductCode,VersionCode,ChassisNo from vehicledelivery ";
    //    HttpContext.Current.Session["ColumnsName"] = sQuery;
    //    AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
    //    HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
    //    ShowLookUp(obj, lookupid, RootURL);
    //}
    public void LU_Get_DO_Receiver (Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        //AA//
        string sQuery = string.Empty;
        Session["AppendOrderby"] = string.Empty;
        sQuery = "Select ReceiverCode,ReceiverDesc, ReceiverNIC,ReceiverAdd1  from Receiver ";
        HttpContext.Current.Session["ColumnsName"] = sQuery;
        AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
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
    //    AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
    //    HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
    //    ShowLookUp(obj, lookupid, RootURL);
    //}
    #endregion

    #region //Taha Zubair//

    public void FQIinfo(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        string SelectClause = " SELECT cv.RegNo AS [Reg No], cv.EngineNo AS [Engine No], cv.ChassisNo AS [Chassis No],cv.CusCode As [Customer Code]," +
        "c.CusDesc + ' ' + c.FatherHusName AS Customer, cv.ProdCode AS [Prod Code], cv.VersionCode AS Version, cv.INVOICEDATE AS [Invoice Date] FROM  CustomerVehicle cv LEFT OUTER JOIN " +
        "Customer c ON cv.CusCode = c.CusCode and cv.DealerCode=c.DealerCode";
        AppendInWhere = "cv.DealerCode IN ('AAAAA','" + Session["DealerCode"].ToString() + "')";
        HttpContext.Current.Session["ColumnsName"] = SelectClause;
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        ShowLookUp(obj, lookupid, RootURL);
    }

    public void PartNo_Desc(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        HttpContext.Current.Session["ColumnsName"] = "Select" +
                        "    ItemCode as [Item Code]," +
                        "    PartItemNo as [Part Item No.]," +
                        "    ItemDesc as [Part Name]" +
                        "    From" +
                        "    Item ";
                      
        AppendInWhere = " DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') ";
        HttpContext.Current.Session["AppendInWhere"] = AppendInWhere;
        HttpContext.Current.Session["AppendOrderby"] = "ORDER BY ItemCode asc";
        ShowLookUp(obj, lookupid, RootURL);
    }

    public void GetFromFIRMaster(Control obj, string lookupid, string AppendInWhere, string RootURL)
    {
        Session["AppendOrderby"] = string.Empty;
        string AppendOrderby;
        string SelectClause = "Select FIRCode,dbo.getNewDate(FIRDate) As FIRDate,RegNo,ChassisNo,EngineNo From FIRMaster";        
        
        AppendInWhere = "DelFlag = 'N' AND DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "')";
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
        AppendInWhere = "PPRM.DealerCode IN ('AAAAA','" + SessionInformation.DealerCode.ToString() + "') " + AppendInWhere;
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