﻿<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="JC.aspx.cs" Inherits="DXBMS.Modules.Service.Forms.JC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
    
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
        <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>
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
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="../../../Jquery/quicksearch.js" type="text/javascript"></script>
    <script src="../../../Jquery/jquery.tablesorter-2.0.3.js" type="text/javascript"></script>
    <link href="../../../css/styletable.css" rel="stylesheet" type="text/css" />
    <link href="../../../css/datatables.min.css" rel="stylesheet" type="text/css" />
    <script src="/js/datatables.min.js" type="text/javascript"></script>
   <%-- <script src="https://cdn.datatables.net/1.10.16/js/jquery.dataTables.min.js" type="text/javascript"></script>--%>
    
    <script type="text/javascript">
            
        function pageLoad() {
            var selectedTab = $("#<%=hfTab.ClientID%>");


            var tabId = selectedTab.val() != "" ? selectedTab.val() : "tab1";
            $('#dvTab a[href="#' + tabId + '"]').tab('show');
            $("#dvTab a").click(function () {
                selectedTab.val($(this).attr("href").substring(1));
            });
        }

        function pageLoad() {
            $('.bs-pagination td table').each(function (index, obj) {
                convertToPagination(obj)
            });
        }

        function convertToPagination(obj) {
            var liststring = '<ul class="pagination">';

            $(obj).find("tbody tr").each(function () {
                $(this).children().map(function () {
                    liststring = liststring + "<li>" + $(this).html() + "</li>";
                });
            });
            liststring = liststring + "</ul>";
            var list = $(liststring);
            list.find('span').parent().addClass('active');

            $(obj).replaceWith(list);
        }

    </script>
    

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    
        <ContentTemplate>
            <div class="container">
                <div class="Heading">JOB CARD</div>     
                <div class="row">
                    <div class="col-lg-12">
                        <div class="col-lg-1 lead" style="margin-top: 7px;">
                            <asp:Label class="lead" runat="server" Visible="false">Search:</asp:Label></div>
                        <div class="col-lg-5" style="margin-top: 7px; line-height: 30px;">
                            <asp:TextBox class="table-responsive" Visible="false" Width="100%" ID="txtSearch" OnTextChanged="txtSearch_TextChanged" runat="server" />                            
                        </div>
                        <div class="col-lg-2" style="margin-top: 5px;">
                            <asp:Button ID="btnSearch" Text="Search" runat="server" Visible="false"/>
                        </div>
                        <div class="col-lg-3" style="text-align:right;margin-top: 10px;">
                            <asp:Label class="lead" runat="server">Create New</asp:Label>
                        </div>
                        <div class="col-lg-1" style="left: -15px;margin-top: 5px;">
                            <asp:ImageButton ID="btnAddConsumablePart" runat="server" Height="40px" ImageUrl="~/Images/AddIMG.png"
                              Width="40px" style="cursor:pointer;" PostBackUrl="~/Modules/Service/Forms/JobCardNew.aspx" />
                        </div>   
                    </div>
                </div>  
                <div id="dvTab">
                    <ul class="nav nav-tabs" role="tablist" id="myTab">
                                        <li class="active"><a href="#openjobcards"  role="tab" data-toggle="tab">Open JobCards</a></li>
                                        <li ><a href="#closejobcards"  role="tab" data-toggle="tab">Closed JobCards</a></li>                                        
                                    </ul>

                    <div class="tab-content">
                        <div role="tabpanel" class="tab-pane active" id="openjobcards">
                            <asp:GridView ID="GridView1" CssClass="openJC" Width="100%" runat="server" AutoGenerateColumns="False" DataKeyNames="JobCardCode"
                                 OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowCreated="GridView1_RowCreated" >   
                                 <%--AllowPaging="true" PageSize="5"  DataSourceID = "SqlDataSource1" PagerStyle-CssClass="bs-pagination"--%>                                        

                                            <Columns>
                                                
                                                <%--<asp:BoundField DataField="JobCardCode" HeaderText="JOB CARD" SortExpression="JobCardCode" ></asp:BoundField>\--%>
                                                <asp:TemplateField HeaderText="Job Card" >
                                                       <ItemTemplate>
                                                           <asp:Label ID="lblJobCard" runat="server" Text='<%# Bind("JobCardCode") %>'></asp:Label>
                                                       </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="User Name" SortExpression="UserName">                                                    
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUserName" Text='<%# HighlightText(Eval("UserName").ToString()) %>' runat="server"
                                                            CssClass="TextField" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Registration #" SortExpression="RegNo">
                                                    <ItemStyle Width="120px" HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRegNo" Text='<%# HighlightText(Eval("RegNo").ToString()) %>' runat="server"
                                                            CssClass="TextField" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Engine #" SortExpression="EngineNo">
                                                    <ItemStyle Width="120px" HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEngineNo" Text='<%# HighlightText(Eval("EngineNo").ToString()) %>' runat="server"
                                                            CssClass="TextField" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Chassis #" SortExpression="ChassisNo">
                                                    <ItemStyle Width="120px" HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblChassisNo" Text='<%# HighlightText(Eval("ChassisNo").ToString()) %>' runat="server"
                                                            CssClass="TextField" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Job Card #" SortExpression="ChassisNo">
                                                    <ItemStyle Width="120px" HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblJobTypeDesc" Text='<%# HighlightText(Eval("JobTypeDesc").ToString()) %>' runat="server"
                                                            CssClass="TextField" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:BoundField DataField="UserName" HeaderText="Name" SortExpression="UserName"></asp:BoundField>
                                                <asp:BoundField DataField="RegNo" HeaderText="Registration #" SortExpression="RegNo"></asp:BoundField>
                                                <asp:BoundField DataField="EngineNo" HeaderText="Engine #" SortExpression="EngineNo"></asp:BoundField>
                                                <asp:BoundField DataField="ChassisNo" HeaderText="Chassis #" SortExpression="ChassisNo"></asp:BoundField>--%>
                                                <asp:CommandField ShowSelectButton="true" HeaderText="Edit" ></asp:CommandField>                                            
                                            </Columns>                                            
                                        </asp:GridView>

                                        <div class="Pager1" style="text-align:center; margin-top: 10px;">
                                        </div>
                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
                                            SelectCommand="SELECT JobCardCode,UserName,RegNo,EngineNo,ChassisNo,J.JobTypeDesc FROM JobCardMaster AS L Inner Join JobTypeMaster J on J.JobTypeCode=L.JobCardType WHERE L.DelFlag = 'N'  AND  L.GatePass = ''  Order by [JobCardCode] desc"
                                            FilterExpression="UserName like '%{0}%' or RegNo like '%{1}%' or EngineNo like '%{2}%' or ChassisNo like '%{3}%' or JobTypeDesc like '%{4}%'">
                                            <FilterParameters>
                                                   <asp:ControlParameter Name="UserName" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="RegNo" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="EngineNo" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="ChassisNo" ControlID="txtSearch" PropertyName="Text" />     
                                                     <asp:ControlParameter Name="Job Card " ControlID="txtSearch" PropertyName="Text" />                                               
                                               </FilterParameters>
                                        </asp:SqlDataSource>                                        
                        </div>
                        <div role="tabpanel" id="closejobcards" class="tab-pane">
                            <asp:GridView ID="GridView2" CssClass="closeJC" Width="100%" runat="server" AutoGenerateColumns="False"                             
                                DataKeyNames="JobCardCode"  OnSelectedIndexChanged="GridView2_SelectedIndexChanged" OnRowCreated="GridView2_RowCreated"> <%-- PageSize="5"  --%>              
                                <%--AllowPaging="true" PagerStyle-CssClass="bs-pagination" DataSourceID="SqlDataSource2"--%>
                                            <Columns>                                                
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

                                                <asp:BoundField DataField="UserName" HeaderText="Name" SortExpression="UserName"></asp:BoundField>
                                                <asp:BoundField DataField="RegNo" HeaderText="Registration No" SortExpression="RegNo"></asp:BoundField>
                                                <asp:BoundField DataField="EngineNo" HeaderText="Engine No" SortExpression="EngineNo"></asp:BoundField>
                                                <asp:BoundField DataField="ChassisNo" HeaderText="Chassis No" SortExpression="ChassisNo"></asp:BoundField>
                                                 <asp:BoundField DataField="JobTypeDesc" HeaderText="Job Card" SortExpression="JobTypeDesc"></asp:BoundField>
                                                <asp:CommandField ShowSelectButton="true" HeaderText="Select" ></asp:CommandField> 
                                                
                                            </Columns>                                        
                                        </asp:GridView>
                                        <div class="Pager" style="text-align:center; margin-top: 10px;">
                                        </div>
                                         <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" OnSelecting="GetDealerCode"
                                            SelectCommand="SELECT JobCardCode,UserName,RegNo,EngineNo,ChassisNo,J.JobTypeDesc  FROM JobCardMaster AS L Inner Join JobTypeMaster J on J.JobTypeCode=L.JobCardType  WHERE L.DealerCode = @DealerCode  and L.GatePass <> ''  Order by [JobCardCode] desc"
                                             FilterExpression="UserName like '%{0}%' or RegNo like '%{1}%' or EngineNo like '%{2}%' or ChassisNo like '%{3}%'or JobTypeDesc like '%{4}%'">
                                            
                                               <FilterParameters>
                                                   <asp:ControlParameter Name="UserName" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="RegNo" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="EngineNo" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="ChassisNo" ControlID="txtSearch" PropertyName="Text" />    
                                                   <asp:ControlParameter Name="Job Card " ControlID="txtSearch" PropertyName="Text" />                                                 
                                               </FilterParameters>

                                             <SelectParameters>
                                                  <asp:SessionParameter  Name="DealerCode" SessionField="DealerCode" DefaultValue="5" />
                                             </SelectParameters>
                                           </asp:SqlDataSource>
                                        
                        </div>
                    </div>
                </div>                
            </div>
               <asp:HiddenField ID="hfTab" runat="server" />
        </ContentTemplate>
             <Triggers>
                
            </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript">
        $(document).ready(function () {
            //$("#GridView2").DataTable();
            $(".closeJC").DataTable({
                "order": [[0, "desc"]]
            });
            $(".openJC").DataTable(
            {
                "order": [[0, "desc"]]
            });
            $(".dataTables_filter").find("input[type=search]").addClass("searchCSS");
        });
        
    </script>
</asp:Content>