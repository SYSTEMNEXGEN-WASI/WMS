<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" Theme="Theme1" AutoEventWireup="true" CodeBehind="WRAppr-Reject.aspx.cs" Inherits="DXBMS.Modules.Service.Forms.WRAppr_Reject" %>
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
            var selectedTab = $("#<%=WRfTab.ClientID%>");


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
                <div class="Heading">Warranty Claim</div>     
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
                              Width="40px" style="cursor:pointer;" PostBackUrl="~/Modules/Service/Forms/Warranty.aspx" />
                        </div>   
                    </div>
                </div>
                <div class="row">
                <div class="col-lg-12">
                <div class="col-lg-2" style="margin-top: 5px;">
                          From Date
                        </div>
                        <div class="col-lg-3" style="text-align:right;margin-top: 10px;">
                          <asp:TextBox ID="txtFromDate" TabIndex="10" runat="server" ReadOnly="true" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="txtFromDate_MaskedEditExtender" runat="server"
                                                ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="true" Mask="99-99-9999" TargetControlID="txtFromDate"
                                                UserDateFormat="DayMonthYear" />
                                            <ajaxToolkit:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server"
                                                Enabled="true" Format="dd-MM-yyyy" TargetControlID="txtFromDate" />
                        </div>
                           <div class="col-lg-2" style="margin-top: 5px;">
                          To Date
                        </div>
                        <div class="col-lg-3" style="text-align:right;margin-top: 10px;">
                          <asp:TextBox ID="txtToDate"  runat="server" ReadOnly="true" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px; width: 100%;" AutoPostBack="true"  OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="txtToDate_MaskedEditExtender" runat="server"
                                                ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="true" Mask="99-99-9999" TargetControlID="txtToDate"
                                                UserDateFormat="DayMonthYear" />
                                            <ajaxToolkit:CalendarExtender ID="txtToDate_CalendarExtender" runat="server"
                                                Enabled="true" Format="dd-MM-yyyy" TargetControlID="txtToDate" />
                        </div>
                </div>

                </div>
                <div id="dvTab">
                    <ul class="nav nav-tabs" role="tablist" id="myTab">
                                        <li class="active"><a href="#openjobcards"  role="tab" data-toggle="tab">Open WR</a></li>
                                         <li ><a href="#Postjobcards"  role="tab" data-toggle="tab">Post WR</a></li>          
                                        <li ><a href="#closejobcards"  role="tab" data-toggle="tab">Closed WR</a></li>                                        
                                    </ul>

                    <div class="tab-content">
                        <div role="tabpanel" class="tab-pane active" id="openjobcards">
                             <asp:GridView ID="grvDTR" CssClass="openJC" Width="100%" runat="server" AutoGenerateColumns="False" DataKeyNames="PPRCode"
                                 OnSelectedIndexChanged="grvDTR_SelectedIndexChanged" OnRowCreated="grvDTR_RowCreated" >   <%--DataSourceID = "SqlDataSource3"--%>
                                            <Columns>                              
                                                <asp:BoundField DataField="PPRCode" HeaderText="WR No" SortExpression="WR" />
                                                <asp:BoundField DataField="PPRDate" HeaderText="WR Date" SortExpression="WRDate" />
                                                 <asp:BoundField DataField="JobCardNo" HeaderText="JobCard No" SortExpression="JobCard"></asp:BoundField>
                                                <asp:BoundField DataField="CusDesc" HeaderText="Customer" SortExpression="Customer"></asp:BoundField>
                                                <asp:BoundField DataField="ChassisNo" HeaderText="Chassis No" SortExpression="ChassisNo"></asp:BoundField>
                                                <asp:BoundField DataField="EngineNo" HeaderText="Engine No" SortExpression="EngineNo"></asp:BoundField>
                                                <asp:CommandField ShowSelectButton="true" HeaderText="Select" ></asp:CommandField>                                            
                                            </Columns>                                            
                            </asp:GridView>  

                                        <div class="Pager1" style="text-align:center; margin-top: 10px;">
                                        </div>
                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
                                            SelectCommand="Select F.PPRCode,Format(F.PPRDate,'dd-MM-yyyy')PPRDate,F.JobCardNo as JobCardNo,Cus.CusDesc,F.ChassisNo,F.EngineNo from PPRMaster F Inner Join CustomerVehicle C on C.ChassisNo=F.ChassisNo and F.EngineNo=C.EngineNo and C.DealerCode=F.DealerCode  Inner Join Customer Cus on Cus.CusCode=C.CusCode and Cus.DealerCode=C.DealerCode Where F.DelFlag='N' "
                                            FilterExpression="FIRCode like '%{0}%' or FIRDate like '%{1}%' or JobCardNo like '%{2}%'or CusDesc like '%{3}%' or ChassisNo like '%{4}%' or EngineNo like '%{5}%'">
                                            <FilterParameters>
                                                   <asp:ControlParameter Name="PPRCode" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="PPRDate" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="JobCardNo" ControlID="txtSearch" PropertyName="Text" />
                                                     <asp:ControlParameter Name="CusDesc" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="ChassisNo" ControlID="txtSearch" PropertyName="Text" />    
                                                     <asp:ControlParameter Name="EngineNo" ControlID="txtSearch" PropertyName="Text" />    
                                                                                                    
                                               </FilterParameters>
                                        </asp:SqlDataSource>                                        
                        </div>
                         <div role="tabpanel" id="Postjobcards" class="tab-pane">
                           <asp:GridView ID="grvPostDTR" CssClass="PostJC" Width="100%" runat="server" AutoGenerateColumns="False" DataKeyNames="PPRCode"
                                 OnSelectedIndexChanged="grvPostDTR_SelectedIndexChanged" OnRowCreated="grvPostDTR_RowCreated" >   <%--DataSourceID = "SqlDataSource3"--%>
                                            <Columns>                              
                                                <asp:BoundField DataField="PPRCode" HeaderText="DTR No" SortExpression="WR No" />
                                                <asp:BoundField DataField="PPRDate" HeaderText="DTR Date" SortExpression="WR Date" />
                                                 <asp:BoundField DataField="JobCardNo" HeaderText="JobCard No" SortExpression="JobCard"></asp:BoundField>
                                                <asp:BoundField DataField="CusDesc" HeaderText="Customer" SortExpression="Customer"></asp:BoundField>
                                                <asp:BoundField DataField="ChassisNo" HeaderText="Chassis No" SortExpression="ChassisNo"></asp:BoundField>
                                                <asp:BoundField DataField="EngineNo" HeaderText="Engine No" SortExpression="EngineNo"></asp:BoundField>
                                                <asp:CommandField ShowSelectButton="true" HeaderText="Select" ></asp:CommandField>                                            
                                            </Columns>                                            
                            </asp:GridView>
                                 <div class="Pager1" style="text-align:center; margin-top: 10px;">
                                        </div>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
                                            SelectCommand="Select F.PPRCode,Format(F.PPRDate,'dd-MM-yyyy')PPRDate,F.RoNo as JobCardNo,Cus.CusDesc,F.ChassisNo,F.EngineNofrom PPRMaster F Inner Join CustomerVehicle C on C.ChassisNo=F.ChassisNo and F.EngineNo=C.EngineNo and C.DealerCode=F.DealerCode  Inner Join Customer Cus on Cus.CusCode=C.CusCode and Cus.DealerCode=C.DealerCode Where F.DelFlag='N' "
                                            FilterExpression="PPRCode like '%{0}%' or PPRDate like '%{1}%' or JobCardNo like '%{2}%'or CusDesc like '%{3}%' or ChassisNo like '%{4}%' or EngineNo like '%{5}%'">
                                            <FilterParameters>
                                                   <asp:ControlParameter Name="FIRCode" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="FIRDate" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="JobCardNo" ControlID="txtSearch" PropertyName="Text" />
                                                     <asp:ControlParameter Name="CusDesc" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="ChassisNo" ControlID="txtSearch" PropertyName="Text" />    
                                                     <asp:ControlParameter Name="EngineNo" ControlID="txtSearch" PropertyName="Text" />    
                                                                                                    
                                               </FilterParameters>
                                        </asp:SqlDataSource>         
                        </div>
                        <div role="tabpanel" id="closejobcards" class="tab-pane">
                           <asp:GridView ID="grvCloseDTR" CssClass="closeJC" Width="100%" runat="server" AutoGenerateColumns="False" DataKeyNames="PPRCode"
                                 OnSelectedIndexChanged="grvCloseDTR_SelectedIndexChanged" OnRowCreated="grvCloseDTR_RowCreated" >   <%--DataSourceID = "SqlDataSource3"--%>
                                            <Columns>                              
                                                <asp:BoundField DataField="PPRCode" HeaderText="WR No" SortExpression="DTR" />
                                                <asp:BoundField DataField="PPRDate" HeaderText="WR Date" SortExpression="DTRDate" />
                                                 <asp:BoundField DataField="JobCardNo" HeaderText="JobCard No" SortExpression="JobCard"></asp:BoundField>
                                                <asp:BoundField DataField="CusDesc" HeaderText="Customer" SortExpression="Customer"></asp:BoundField>
                                                <asp:BoundField DataField="ChassisNo" HeaderText="Chassis No" SortExpression="ChassisNo"></asp:BoundField>
                                                <asp:BoundField DataField="EngineNo" HeaderText="Engine No" SortExpression="EngineNo"></asp:BoundField>
                                                <asp:CommandField ShowSelectButton="true" HeaderText="Select" ></asp:CommandField>                                            
                                            </Columns>                                            
                            </asp:GridView>
                                 <div class="Pager1" style="text-align:center; margin-top: 10px;">
                                        </div>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:BMSConnectionString %>" 
                                            SelectCommand="Select F.PPRCode,Format(F.PPRDate,'dd-MM-yyyy')PPRDate,F.RoNo as JobCardNo,Cus.CusDesc,F.ChassisNo,F.EngineNo from PPRMaster F Inner Join CustomerVehicle C on C.ChassisNo=F.ChassisNo and F.EngineNo=C.EngineNo and C.DealerCode=F.DealerCode  Inner Join Customer Cus on Cus.CusCode=C.CusCode and Cus.DealerCode=C.DealerCode Where F.DelFlag='N' "
                                            FilterExpression="FIRCode like '%{0}%' or FIRDate like '%{1}%' or JobCardNo like '%{2}%'or CusDesc like '%{3}%' or ChassisNo like '%{4}%' or EngineNo like '%{5}%'">
                                            <FilterParameters>
                                                   <asp:ControlParameter Name="PPRCode" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="PPRDate" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="JobCardNo" ControlID="txtSearch" PropertyName="Text" />
                                                     <asp:ControlParameter Name="CusDesc" ControlID="txtSearch" PropertyName="Text" />
                                                   <asp:ControlParameter Name="ChassisNo" ControlID="txtSearch" PropertyName="Text" />    
                                                     <asp:ControlParameter Name="EngineNo" ControlID="txtSearch" PropertyName="Text" />    
                                                                                                    
                                               </FilterParameters>
                                        </asp:SqlDataSource>         
                        </div>
                    </div>
                </div>
            </div>
              <asp:HiddenField ID="WRfTab" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        $(document).ready(function () {
            //$("#GridView2").DataTable();
            $(".closeJC").DataTable();
            $(".PostJC").DataTable();
            $(".openJC").DataTable();
            $(".dataTables_filter").find("input[type=search]").addClass("searchCSS");
        });
        
    </script>
</asp:Content>
