<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="Customer.aspx.cs" Inherits="DXBMS.Modules.BusinessPartner.Customer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="../../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../js/chosen.jquery.js" type="text/javascript"></script>
    <script src="../../js/chosen.jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="../../Jquery/quicksearch.js" type="text/javascript"></script>
    <script src="../../../Jquery/jquery.tablesorter-2.0.3.js" type="text/javascript"></script>
    <link href="../../../css/styletable.css" rel="stylesheet" type="text/css" />
    <link href="../../../css/datatables.min.css" rel="stylesheet" type="text/css" />
    <script src="../../../js/datatables.min.js" type="text/javascript"></script>
      <style type="text/css">
        body
        {
            font-family: Arial;            
        }
        table
        {
            border: 1px solid #ccc;
        }
        table th
        {
            background-color: #F7F7F7;
            color: #333;
            
        }
        table th, table td
        {
            padding: 5px;
            border-color: #ccc;
        }
        .Pager span
        {
            color: #333;
            background-color: #F7F7F7;
            font-weight: bold;
            text-align: center;
            display: inline-block;
            width: 20px;
            margin-right: 3px;
            line-height: 150%;
            border: 1px solid #ccc;
        }
        .Pager a
        {
            text-align: center;
            display: inline-block;
            width: 20px;
            border: 1px solid #ccc;
            color: #fff;
            color: #333;
            margin-right: 3px;
            line-height: 150%;
            text-decoration: none;
        }
        .highlight
        {
            background-color: #FFFFAF;
        }
        .searchCSS
        {
            height:30px;
            border-radius:5px;
        }
        .dataTables_wrapper
        {
            margin-top:10px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

    
  

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
                <div class="Heading">Customer Profile</div>     
                <div class="row">
                    <div class="col-lg-12">
                        <div class="col-lg-1 lead" style="margin-top: 7px;">
                          <span class="fa-arrow-left fa"></span>
                        <asp:LinkButton ID="lnkBack" ForeColor="Black" runat="server" PostBackUrl="~/" Text="Back"></asp:LinkButton>
                         
                            <asp:Label ID="Label1" class="lead" runat="server" Visible="false">Search:</asp:Label></div>
                        <div class="col-lg-6" style="margin-top: 7px; line-height: 30px;">
                            <asp:TextBox class="table-responsive" Visible="false" Width="100%" ID="txtSearch" runat="server" />                            
                        </div>
                        <div class="col-lg-2" style="margin-top: 5px;">
                            <asp:Button ID="btnSearch" Visible="false" Text="Search" runat="server"/>
                        </div>
                        <div class="col-lg-2" style="text-align:right;margin-top: 10px;">
                            <asp:Label ID="Label2" class="lead" runat="server">Create New</asp:Label>
                        </div>
                        <div class="col-lg-1" style="left: -15px;margin-top: 5px;">
                            <asp:ImageButton ID="btnAddConsumablePart" runat="server" Height="40px" ImageUrl="~/Images/AddIMG.png"
                              Width="40px" style="cursor:pointer;" PostBackUrl="~/Modules/BusinessPartner/CustProfile.aspx" />
                        </div>   
                    </div>
                </div>
                <fieldset>
                    <div class="tab-content">
                        <div role="tabpanel" class="tab-pane active" id="openjobcards">
                            <asp:GridView ID="GridView1" CssClass="CusVeh" Width="100%" runat="server" AutoGenerateColumns="False" DataKeyNames="cusCode"
                                 OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowCreated="GridView1_RowCreated" >   <%--DataSourceID = "SqlDataSource3"--%>
                                            <Columns>                              
                                                <asp:BoundField DataField="cusCode" HeaderText="Customer Code" SortExpression="cusCode" />
                                                <asp:BoundField DataField="CusDesc" HeaderText="Customer" SortExpression="Customer"></asp:BoundField>
                                                <asp:BoundField DataField="Address1" HeaderText="Address" SortExpression="Address1"></asp:BoundField>
                                                <asp:BoundField DataField="CellNo" HeaderText="Cell No" SortExpression="CellNo"></asp:BoundField>
                                                <asp:BoundField DataField="NIC" HeaderText="NIC No" SortExpression="NIC" />
                                                 <asp:BoundField DataField="NTNno" HeaderText="NTN No" SortExpression="NTNno" />
                                                <asp:CommandField ShowSelectButton="true" HeaderText="Select" ></asp:CommandField>                                            
                                            </Columns>                                            
                            </asp:GridView>                                        
                                        <%--<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>"
                                            SelectCommand ="Select CusCode , CusDesc , Address1 , CellNo , NIC from Customer C">                                            
                                               <FilterParameters>
                                                   <asp:ControlParameter Name="CusDesc" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="Address1" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="CellNo" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="NIC" ControlID="txtSearch" PropertyName="Text" />                                                 
                                               </FilterParameters>
                                               <SelectParameters>
                                                    <asp:SessionParameter DefaultValue="00001" Name="DealerCode" SessionField="DealerCode" />
                                               </SelectParameters>
                                       </asp:SqlDataSource>--%>                                        
                        </div>                       
                    </div>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
        <script type="text/javascript">
            $(document).ready(function () {
                
                $(".CusVeh").DataTable(
                {
                    "order": [[0, "desc"]]
                });
                
            });
        
    </script>
</asp:Content>
