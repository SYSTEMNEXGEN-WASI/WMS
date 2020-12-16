<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="AccCodeAsgmt.aspx.cs" Inherits="DXBMS.Modules.Setup.AccCodeAsgmt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
     <style>
        /* style sheet for tab*/
      
        .float{
            float : right;
        }

        /*.chosen-container.chosen-container-single {
    width: 300px !important; /* or any value that fits your needs 
}*/
        div#ctl00_Content_TabContainer1_TabPanel3_ddlAssPartStkLcl_chosen {        
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel3_ddlAssPartStkMkt_chosen {
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel3_ddlAssPartStkCKD_chosen {        
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel3_ddlAssLubStkMkt_chosen {
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel3_ddlAssPartStkImp_chosen {        
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel3_ddlAssLubStkLcl_chosen {
            width : 100% !important; 
        }

        

        div#ctl00_Content_TabContainer1_TabPanel4_ddlLiabPST_chosen {        
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel4_ddlLiabGST_chosen {
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel4_ddlLiabExtra_chosen {        
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel4_ddlLiabFur_chosen {
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel4_ddlLiabWithHold_chosen {
            width : 100% !important; 
        }
        

        div#ctl00_Content_TabContainer1_TabPanel1_ddlExPartCostMkt_chosen {        
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel1_ddlExPartCostCKD_chosen {
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel1_ddlExPartCostLcl_chosen {        
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel1_ddlExPartCostImp_chosen {        
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel1_ddlExLubCostMkt_chosen {
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel1_ddlExLubCostLcl_chosen {        
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel1_ddlExCashDis_chosen {
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel1_ddlExChargeOut_chosen {        
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel1_ddlExBadDate_chosen {
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel1_ddlExSvcChar_chosen {
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel1_ddlExOtherChar_chosen {
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel1_ddlExFreightChar_chosen {
            width : 100% !important; 
        }

        div#ctl00_Content_TabContainer1_TabPanel2_ddlInPartSaleMkt_chosen {        
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel2_ddlInPartSaleCKD_chosen {
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel2_ddlInPartSaleLcl_chosen {        
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel2_ddlInPartSaleImp_chosen {        
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel2_ddlInLubSaleMkt_chosen {
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel2_ddlInLubSaleLcl_chosen {        
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel2_ddlInSubIncome_chosen {
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel2_ddlInLabIncome_chosen {        
            width : 100% !important; 
        }
        div#ctl00_Content_TabContainer1_TabPanel2_ddlInWarantyLab_chosen {
            width : 100% !important; 
        }
      
        .TabStyle .ajax__tab_header
        {
            cursor: pointer;
            background-color: #f1f1f1;
            font-size: 14px;
            font-weight: bold;
            font-family: Arial, Helvetica, sans-serif;
            height: 36px;
            border-bottom: 1px solid #bebebe;
            width:100%;
        }
      
        .TabStyle .ajax__tab_active .ajax__tab_tab
        {
            border: 1px solid;
            border-color: #bebebe #bebebe #e1e1e1 #bebebe;
            background-color: #e1e1e1;
            padding: 10px;
            border-bottom: none;
        }
        .TabStyle .ajax__tab_active .ajax__tab_tab:hover
        {
            border: 1px solid;
            border-color: #bebebe #bebebe #e1e1e1 #bebebe;
            background-color: #e1e1e1;
            padding: 10px;
            border-bottom: none;
        }
      
        .TabStyle .ajax__tab_tab
        {
            border: 1px solid;
            border-color: #e1e1e1 #e1e1e1 #bebebe #e1e1e1;
            background-color: #f1f1f1;
            color: #777777;
            cursor: pointer;
            text-decoration: none;
            padding: 10px;
        }
        .TabStyle .ajax__tab_tab:hover
        {
            border: 1px solid;
            border-color: #bebebe #bebebe #e1e1e1 #bebebe;
            background-color: #e1e1e1;
            color: #777777;
            cursor: pointer;
            text-decoration: none;
            padding: 10px;
            border-bottom: none;
        }
        .TabStyle .ajax__tab_active .ajax__tab_tab, .TabStyle .ajax__tab_tab, .TabStyle .ajax__tab_header .ajax__tab_tab
        {
            margin: 0px 0px 0px 0px;
        }

        input.imgBackBtn {
            height: 45px;
            padding-right: 37px;
            margin-right: -35px;
        }
      
        .TabStyle .ajax__tab_body
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 10pt;
            border-top: 0;
            border: 1px solid #bebebe;
            border-top: none;
            padding: 5px;
            background-color: #e1e1e1;
            width:100%;
            padding-top: 15px;
        }
        .chosen-drop {
    width: 100% !important;
}
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">        
        <ContentTemplate>
            <div class="container">
                <fieldset class="field">
                <legend>Account Code Assignment</legend>                
                <ajaxToolkit:TabContainer ID="TabContainer1" runat="server"  CssClass="TabStyle" TabIndex="20" Width="100%" Style="direction: ltr; text-align: left; display: grid;">
                    <ajaxToolkit:TabPanel ID="TabPanel1" runat="server">
                        <HeaderTemplate>Expense</HeaderTemplate>
                        <ContentTemplate >                           
                                <table width="100%" class="table table-bordered table-hover">
                                    <tr>
                                        <td style="width:20%;">Parts Cost of Sales (Market)</td>
                                        <td style="width:30%;    white-space: nowrap;"><asp:DropDownList width="100%" runat="server" ID="ddlExPartCostMkt"></asp:DropDownList></td>
                                        <td style="width:20%;">Parts Cost of Sales (CKD)</td>
                                        <td style="width:30%;    white-space: nowrap;"><asp:DropDownList width="100%" runat="server" ID="ddlExPartCostCKD"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td >Parts Cost of Sales (Local)</td>
                                        <td style="white-space: nowrap;"><asp:DropDownList width="100%" runat="server" ID="ddlExPartCostLcl"></asp:DropDownList></td>
                                        <td >Parts Cost of Sales (Imported)</td>
                                        <td style="white-space: nowrap;"><asp:DropDownList width="100%" runat="server" ID="ddlExPartCostImp"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>Lubricant Cost of Sales (Market)</td>
                                        <td style="white-space: nowrap;"><asp:DropDownList width="100%" runat="server" ID="ddlExLubCostMkt"></asp:DropDownList></td>
                                        <td >Lubricant Cost of Sales (Local)</td>
                                        <td style="white-space: nowrap;"><asp:DropDownList width="100%" runat="server" ID="ddlExLubCostLcl"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>Lubricant Cost of Sales (C.K.D)</td>
                                        <td style="white-space: nowrap;"><asp:DropDownList width="100%" runat="server" ID="ddlExLubCostCKD"></asp:DropDownList></td>
                                        <td >Lubricant Cost of Sales (Imported)</td>
                                        <td style="white-space: nowrap;"><asp:DropDownList width="100%" runat="server" ID="ddlExLubCostImport"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap;">Cash Discount</td>
                                        <td style="white-space: nowrap;"><asp:DropDownList width="100%" runat="server" ID="ddlExCashDis"></asp:DropDownList></td>
                                        <td style="white-space: nowrap;">ChargeOut</td>
                                        <td style="white-space: nowrap;"><asp:DropDownList width="100%" runat="server" ID="ddlExChargeOut"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>bad dabt</td>
                                        <td><asp:DropDownList width="100%" runat="server" ID="ddlExBadDate"></asp:DropDownList></td>
                                        <td>Service Charges</td>
                                        <td><asp:DropDownList width="100%" runat="server" ID="ddlExSvcChar"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>Other Charges</td>
                                        <td><asp:DropDownList width="100%" runat="server" ID="ddlExOtherChar"></asp:DropDownList></td>
                                        <td>Freight Charges</td>
                                        <td><asp:DropDownList width="100%" runat="server" ID="ddlExFreightChar"></asp:DropDownList></td>
                                    </tr>
                                </table>
                         </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="TabPanel2" runat="server">
                        <HeaderTemplate>Income</HeaderTemplate>
                        <ContentTemplate>                            
                                <table width="100%" class="table table-bordered table-hover">
                                    <tr>
                                        <td style="width:20%;">Parts Sale Income (Market)</td>
                                        <td style="width:30%;"><asp:DropDownList width="100%" runat="server" ID="ddlInPartSaleMkt"></asp:DropDownList></td>
                                        <td style="width:20%;">Parts Sale Income (CKD)</td>
                                        <td style="width:30%;"><asp:DropDownList width="100%" runat="server" ID="ddlInPartSaleCKD"></asp:DropDownList></td>      
                                    </tr>
                                    <tr>                              
                                        <td>Parts Sale Income (Local)</td>
                                        <td><asp:DropDownList width="100%" runat="server" ID="ddlInPartSaleLcl"></asp:DropDownList></td>
                                        <td>Parts Sale Income (Imported)</td>     
                                        <td><asp:DropDownList width="100%" runat="server" ID="ddlInPartSaleImp"></asp:DropDownList></td>
                                    </tr>
                                    <tr>                          
                                        <td>Lubricant Sale Income (Market)</td>
                                        <td><asp:DropDownList width="100%" runat="server" ID="ddlInLubSaleMkt"></asp:DropDownList></td>   
                                        <td>Lubricant Sale Income (Local)</td>
                                        <td><asp:DropDownList width="100%" runat="server" ID="ddlInLubSaleLcl"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>Lubricant Sale Income (C.K.D)</td>
                                        <td style="white-space: nowrap;"><asp:DropDownList width="100%" runat="server" ID="ddlInLubSaleCKD"></asp:DropDownList></td>
                                        <td >Lubricant Sale Income (Imported)</td>
                                        <td style="white-space: nowrap;"><asp:DropDownList width="100%" runat="server" ID="ddlInLubSaleImport"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>Sublet Income</td>
                                        <td><asp:DropDownList width="100%" runat="server" ID="ddlInSubIncome"></asp:DropDownList></td>
                                        <td>Labour Income</td>
                                        <td><asp:DropDownList width="100%" runat="server" ID="ddlInLabIncome"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>Warranty Labour</td>
                                        <td><asp:DropDownList width="100%" runat="server" ID="ddlInWarantyLab"></asp:DropDownList></td>
                                         <td>PDI/FS </td>
                                        <td><asp:DropDownList width="100%" runat="server" ID="ddlInFSB"></asp:DropDownList></td>
                                    </tr>
                                </table>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="TabPanel3" runat="server">
                        <HeaderTemplate>Asset</HeaderTemplate>
                        <ContentTemplate>         
                            <table width="100%" class="table table-bordered table-hover">
                                    <tr>
                                        <td style="width:20%;">Parts Stock (Local)</td>
                                        <td style="width:30%;"><asp:DropDownList width="100%"  runat="server" ID="ddlAssPartStkLcl"></asp:DropDownList></td>
                                        <td style="width:20%;">Parts Stock (Market)</td>
                                        <td style="width:30%;"><asp:DropDownList width="100%" runat="server" ID="ddlAssPartStkMkt"></asp:DropDownList></td>     
                                    </tr>
                                    <tr>                              
                                        <td>Parts Stock (CKD)</td>
                                        <td><asp:DropDownList width="100%" runat="server" ID="ddlAssPartStkCKD"></asp:DropDownList></td>
                                        <td>Parts Stock (Imported)</td>
                                        <td><asp:DropDownList width="100%" runat="server" ID="ddlAssPartStkImp"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>Lubricant Stock (Local)</td>
                                        <td><asp:DropDownList width="100%" runat="server" ID="ddlAssLubStkLcl"></asp:DropDownList></td> 
                                        <td>Lubricant Stock (Market)</td>
                                        <td><asp:DropDownList width="100%" runat="server" ID="ddlAssLubStkMkt"></asp:DropDownList></td>   
                                     </tr>
                                     <tr>
                                        <td>Lubricant Stock (C.K.D)</td>
                                        <td style="white-space: nowrap;"><asp:DropDownList width="100%" runat="server" ID="ddlAssLubStkCKD"></asp:DropDownList></td>
                                        <td >Lubricant Stock (Imported)</td>
                                        <td style="white-space: nowrap;"><asp:DropDownList width="100%" runat="server" ID="ddlAssLubStkImport"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                     <td>
                                        OEM Account
                                        </td>
                                        
                                          <td><asp:DropDownList width="100%" runat="server" ID="ddlLiaOEMAcc"></asp:DropDownList></td> 
                                           <td>
                                        Advance Payment To Vendor
                                        </td>
                                        
                                          <td><asp:DropDownList width="100%" runat="server" ID="ddlAssAPTV"></asp:DropDownList></td> 
                                    </tr>
                            </table>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="TabPanel4" runat="server">
                        <HeaderTemplate>Liabalities</HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" class="table table-bordered table-hover">
                                    <tr>
                                        <td style="width:20%;">PST (Tax)</td>
                                        <td style="width:30%;"><asp:DropDownList width="100%" runat="server" ID="ddlLiabPST"></asp:DropDownList></td>
                                        <td style="width:20%;">GST (Tax)</td>
                                        <td style="width:30%;"><asp:DropDownList width="100%" runat="server" ID="ddlLiabGST"></asp:DropDownList></td>      
                                    </tr>
                                    <tr>                              
                                        <td>Extra (Tax)</td>
                                        <td><asp:DropDownList width="100%" runat="server" ID="ddlLiabExtra"></asp:DropDownList></td>                                    
                                        <td>Further (Tax)</td>
                                        <td><asp:DropDownList width="100%" runat="server" ID="ddlLiabFur"></asp:DropDownList></td></tr>
                                    <tr>                              
                                        <td>WithHolding</td>
                                        <td><asp:DropDownList width="100%" runat="server" ID="ddlLiabWithHold"></asp:DropDownList></td> 
                                        <td>Advance Payment Receipt</td>
                                        <td><asp:DropDownList width="100%" runat="server" ID="ddlLiaAPR"></asp:DropDownList></td> 
                                       
                                     </tr>
                                     <tr>
                                       <td>Advance Payment Insurance</td>
                                        <td><asp:DropDownList width="100%" runat="server" ID="ddlLiaIA"></asp:DropDownList></td> 
                                        
                                     </tr>
                            </table>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>                    
            </ajaxToolkit:TabContainer>
                </fieldset>
                <div class="row">
                    <div class="col-lg-12">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblMsg" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#339933"></asp:Label>                                
                                </td>
                                <td align="center">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Visible="true" OnClientClick="this.disabled = true; this.value = 'Please Wait';" UseSubmitBehavior="false"/>
                                    <asp:Button ID="btnLoad" runat="server" Text="Load" OnClick="btnLoad_Click" Visible="false"/>
                                </td>
                            </tr>                
                        </table>
                    </div>
                </div>
            </div>
            <script src="../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
            <script>
                $(document).ready(function () {
                    $('select').chosen({ width: '100%' });

                    $('.chosen-container.chosen-container-single').css({ "width": "100%" });
                });

                //$('select').chosen({ width: '100%' });

                //$('.chosen-container.chosen-container-single').css({ "width": "100%" });

                //$(".chosen-select").chosen({ width: '100%' /* desired width */ });
            </script>
        </ContentTemplate>  
              
    </asp:UpdatePanel>

</asp:Content>
