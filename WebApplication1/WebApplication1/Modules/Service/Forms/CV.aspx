<%@ Page Title="" Language="C#" Theme="Theme1" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="CV.aspx.cs" Inherits="DXBMS.Modules.Service.Forms.CV" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">


    <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
    <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="../../../Jquery/quicksearch.js" type="text/javascript"></script>
    <script src="../../../Jquery/jquery.tablesorter-2.0.3.js" type="text/javascript"></script>
    <link href="../../../css/styletable.css" rel="stylesheet" type="text/css" />
    <link href="../../../css/datatables.min.css" rel="stylesheet" type="text/css" />
    <script src="../../../js/datatables.min.js" type="text/javascript"></script>
    <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
        table
        {
            border: 1px solid #ccc;
        }
        table th
        {
            background-color: #F7F7F7;
            color: #333;
            font-weight: bold;
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
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    
        <ContentTemplate>
            <div class="container">
                <div class="Heading">Customer Vehicle</div>     
                <div class="row">
                    <div class="col-lg-12">
                        <div class="col-lg-6 lead" style="margin-top: 7px;">
                          <span class="fa-arrow-left fa"></span>
                        <asp:LinkButton ID="lnkBack" ForeColor="Black" runat="server" PostBackUrl="~/" Text="Back"></asp:LinkButton>
                         
                            <asp:Label ID="Label1" class="lead" runat="server" ></asp:Label></div>
                        <div class="col-lg-1" style="margin-top: 7px;  line-height: 30px; ">
                         
                         
                            <asp:TextBox class="table-responsive" Visible="false" Width="100%" ID="txtSearch" OnTextChanged="txtSearch_TextChanged" runat="server" />                            
                        </div>
                        <div class="col-lg-2" style="margin-top: 5px;">
                            <asp:Button ID="btnSearch" Text="Search" runat="server" Visible="false"/>
                        </div>
                        <div class="col-lg-2" style="text-align:right;margin-top: 10px;">
                            <asp:Label ID="Label2" class="lead" runat="server">Create New</asp:Label>
                        </div>
                        <div class="col-lg-1" style="left: -15px;margin-top: 5px;">
                            <asp:ImageButton ID="btnAddConsumablePart" runat="server" Height="40px" ImageUrl="~/Images/AddIMG.png"
                              Width="40px" style="cursor:pointer;" PostBackUrl="~/Modules/Service/Forms/CustomerVehicle.aspx" />
                        </div>   
                    </div>
                </div>  
                <fieldset>                  

                    <div class="tab-content">
                        <div role="tabpanel" class="tab-pane active" id="openjobcards">
                            <asp:GridView ID="GridView1" CssClass="CusVeh" Width="100%" runat="server" AutoGenerateColumns="False" DataKeyNames="cusCode"
                                 OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowCreated="GridView1_RowCreated"
                                   >   <%--PagerStyle-CssClass="bs-pagination" AllowPaging="true" PageSize="10" DataSourceID = "SqlDataSource3"--%>                                        
                                    <%--<HeaderStyle  CssClass="GridHeader"/>--%>
                                            <Columns>                                                
                                                <%--<asp:BoundField DataField="JobCardCode" HeaderText="JOB CARD" SortExpression="JobCardCode" ></asp:BoundField>\--%>
                                                <asp:BoundField DataField="cusCode" HeaderText="Customer Code" SortExpression="cusCode" />
                                                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer"></asp:BoundField>
                                                <asp:BoundField DataField="AccountOf" HeaderText="Account of" SortExpression="AccountOf"></asp:BoundField>
                                                <asp:BoundField DataField="CellNo" HeaderText="Cell No" SortExpression="CellNo"></asp:BoundField>
                                                <asp:BoundField DataField="RegNo" HeaderText="Reg No" SortExpression="RegNo"></asp:BoundField>
                                                <asp:BoundField DataField="ChassisNo" HeaderText="Chassis No" SortExpression="ChassisNo" ></asp:BoundField>
                                                <asp:BoundField DataField="EngineNo" HeaderText="Engine No" SortExpression="EngineNo" />
                                                <asp:CommandField ShowSelectButton="true" HeaderText="Select" ></asp:CommandField>                                            
                                            </Columns>                                            
                                        </asp:GridView>

                                        <div class="Pager1" style="text-align:center; margin-top: 10px;">
                                        </div>                                        
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>"
                                            SelectCommand ="Select A.cusCode, B.CusDesc as Customer,D.CusDesc as AccountOf,D.cellno as CellNo,A.RegNo,A.ChassisNo,A.EngineNo From CustomerVehicle A
                                             inner join Customer B On A.Dealercode = B.DealerCode and A.Cuscode = B.Cuscode inner join Customer D On A.Dealercode = D.DealerCode and A.Cuscode = D.Cuscode"
                                              >
                                            <%--SelectCommand="select cusCode,CusDesc, Address1 ,phone1  from Customer where DealerCode IN ('AAAAA',@DealerCode) Order by CusCode Desc"
                                                FilterExpression="cusCode like '%{0}%' or CusDesc like '%{1}%' or Address1 like '%{2}%' or phone1 like '%{3}%'">--%>
                                            
                                               <FilterParameters>
                                                   <asp:ControlParameter Name="Customer" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="AccountOf" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="CellNo" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="RegNo" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="ChassisNo" ControlID="txtSearch" PropertyName="Text" />     
                                                   <asp:ControlParameter Name="Engine" ControlID="txtSearch" PropertyName="Text" />                                                 
                                               </FilterParameters>
                                                     <SelectParameters>
                                                         <asp:SessionParameter DefaultValue="00001" Name="DealerCode" 
                                                             SessionField="DealerCode" />
                                                     </SelectParameters>
                                       </asp:SqlDataSource>                                        
                        </div>
                        <%--<div role="tabpanel" id="closejobcards" class="tab-pane active">
                            <asp:GridView ID="GridView2" CssClass="closeJC" Width="100%" runat="server" AutoGenerateColumns="False"                             
                                DataKeyNames="JobCardCode" DataSourceID="SqlDataSource2" OnSelectedIndexChanged="GridView2_SelectedIndexChanged" > <%-- PageSize="5"  --%>            
                                <%--AllowPaging="true" PagerStyle-CssClass="bs-pagination"--%>
                                            <%--<Columns>                                                
                                                <asp:BoundField DataField="JobCardCode" HeaderText="JOB CARD" SortExpression="JobCardCode" ></asp:BoundField>
                                                
                                               <%-- <asp:TemplateField HeaderText="User Name" SortExpression="UserName">
                                                    
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUserName" Text='<%# HighlightText(Eval("UserName").ToString()) %>' runat="server"
                                                            CssClass="TextField" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderText="Registration No" SortExpression="RegNo">
                                                    <ItemStyle Width="120px" HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRegNo" Text='<%# HighlightText(Eval("RegNo").ToString()) %>' runat="server"
                                                            CssClass="TextField" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Engine No" SortExpression="EngineNo">
                                                    <ItemStyle Width="120px" HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEngineNo" Text='<%# HighlightText(Eval("EngineNo").ToString()) %>' runat="server"
                                                            CssClass="TextField" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Chassis No" SortExpression="ChassisNo">
                                                    <ItemStyle Width="120px" HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblChassisNo" Text='<%# HighlightText(Eval("ChassisNo").ToString()) %>' runat="server"
                                                            CssClass="TextField" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                 <%--<asp:BoundField DataField="UserName" HeaderText="Name" SortExpression="UserName"></asp:BoundField>
                                                <asp:BoundField DataField="RegNo" HeaderText="Registration No" SortExpression="RegNo"></asp:BoundField>
                                                <asp:BoundField DataField="EngineNo" HeaderText="Engine No" SortExpression="EngineNo"></asp:BoundField>
                                                <asp:BoundField DataField="ChassisNo" HeaderText="Chassis No" SortExpression="ChassisNo"></asp:BoundField>
                                                <asp:CommandField ShowSelectButton="true" HeaderText="Edit" ></asp:CommandField> 
                                                
                                            </Columns>                                        
                                        </asp:GridView>
                                        <div class="Pager" style="text-align:center; margin-top: 10px;">
                                        </div>
                                         <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
                                            SelectCommand="SELECT JobCardCode,UserName,RegNo,EngineNo,ChassisNo FROM JobCardMaster AS L WHERE L.GatePass <> ''  Order by [JobCardCode] desc">
                                             <%--FilterExpression="UserName like '%{0}%' or RegNo like '%{1}%' or EngineNo like '%{2}%' or ChassisNo like '%{3}%'">
                                            
                                               <FilterParameters>
                                                   <asp:ControlParameter Name="UserName" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="RegNo" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="EngineNo" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="ChassisNo" ControlID="txtSearch" PropertyName="Text" />                                                   
                                               </FilterParameters>--%>
                                            <%--</asp:SqlDataSource>
                                        
                        </div>--%>
                    </div>
                </fieldset>                
            </div>
               <asp:HiddenField ID="hfTab" runat="server" />
        </ContentTemplate>
             <Triggers>
                
            </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript">
        $(document).ready(function () {
            //$("#GridView2").DataTable();
//            $(".closeJC").DataTable();
            $(".CusVeh").DataTable(
            {
                serverside: true,
              


            });
//            $(".dataTables_filter").find("input[type=search]").addClass("searchCSS");
        });
        
    </script>

</asp:Content>
