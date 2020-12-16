<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="VoucherStatus.aspx.cs" Theme="Theme1" Inherits="DXBMS.Modules.Service.ServiceReports.Critaria.VoucherStatus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
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
<asp:UpdatePanel runat="server">
<ContentTemplate>
<div class="container">
<div class="Heading">
Pending Vouchers 
</div>
<div class="row">
<table width="100%" style="border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
<tr>
<td>
From Date
</td>
<td>
 <asp:TextBox ID="txtFromDate" runat="server" Style="background-image: url(../../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"  CausesValidation="false"></asp:TextBox>
    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd-MM-yyyy" TargetControlID="txtFromDate"/>
</td>
<td>
To Date
</td>
<td>
 <asp:TextBox ID="txtToDate" runat="server" Style="background-image: url(../../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"   CausesValidation="false"></asp:TextBox>
    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" Format="dd-MM-yyyy" TargetControlID="txtToDate"/>
</td>
<td>Form</td>
<td>
<asp:DropDownList runat="server" ID="ddlForm" OnSelectedIndexChanged="txtForm_SelectedIndexChanged" AutoPostBack="true" CausesValidation="false">
<asp:ListItem Value="All" >ALL</asp:ListItem>
<asp:ListItem Value="Invoice" >Invoice</asp:ListItem>
<asp:ListItem Value="DirectSale" >Direct Sale</asp:ListItem>
<asp:ListItem Value="Incomming Payment" >Incomming Payment</asp:ListItem>
<asp:ListItem Value="Outgoing Payment" >Outgoing Payment</asp:ListItem>
<asp:ListItem Value="Payable Invoice" >Payable Invoice</asp:ListItem>
<asp:ListItem Value="Free Service" >Free Service</asp:ListItem>
<asp:ListItem Value="AC Memo" >Warranty</asp:ListItem>
<asp:ListItem Value="Purchase Return" >Purchase Return</asp:ListItem>
<asp:ListItem Value="Sale Return" >Sale Return</asp:ListItem>
<asp:ListItem Value="Payment Refund" >Payment Refund</asp:ListItem>
</asp:DropDownList>
</td>
</tr>
<tr>
<asp:GridView ID="gvPendingVoucher" CssClass="PendVou" Width="100%" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" PageSize="15"
                          Font-Names="Arial" Font-Size="9pt" Style="text-align: left" PagerStyle-CssClass="pgr" OnPageIndexChanging="gvPendingVoucher_PageIndexChanging"
                          ShowHeaderWhenEmpty="True" >
<Columns>
<asp:BoundField DataField="DealerCode" HeaderText="Dealer" Visible="false"  />
<asp:BoundField DataField="TransCode" HeaderText="Transction No"  />
<asp:BoundField DataField="TransDate" HeaderText="Date"  />
<asp:BoundField DataField="VoucherNo" HeaderText="Voucher No"  />
<asp:BoundField DataField="Form" HeaderText="Form"  />
</Columns>
</asp:GridView>
</tr>
</table>


</div>
</div>
</ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
        $(document).ready(function () {
            //$("#GridView2").DataTable();
//            $(".closeJC").DataTable();
            $(".PendVou").DataTable(
            {
                serverside: true,
              


            });
//            $(".dataTables_filter").find("input[type=search]").addClass("searchCSS");
        });
        
    </script>
</asp:Content>
 