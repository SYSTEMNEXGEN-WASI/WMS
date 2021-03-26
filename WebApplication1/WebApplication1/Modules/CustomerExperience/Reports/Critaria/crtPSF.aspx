<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="crtPSF.aspx.cs" Inherits="DXBMS.Modules.CustomerExperience.Reports.crtPSF" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" Runat="Server">
<script src="../../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
    <script src="../../../../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../../../js/chosen.jquery.js" type="text/javascript"></script>
    <script src="../../../../js/chosen.jquery.min.js" type="text/javascript"></script>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container" >
                
                        <fieldset>
       <legend>Post Service Follow Up Summary</legend>
    <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
        
        <tr>
            <td style="text-align:right;">Follow Up Status</td>
            <td>
                <asp:DropDownList ID="ddlStatus" runat="server" Width="38%"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width:50%; text-align:right">
                From Date  <font color="red"><b>*</b></font>
            </td>
            <td>
                <asp:TextBox ID="txtFromDate" runat="server" Style="background-image: url(/Images/Calendar_16x16.png);
                                   background-repeat:no-repeat; padding-left:20px;"></asp:TextBox>
                 <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFromDate" Format="yyyy-MM-dd"></asp:CalendarExtender>
                 <asp:Label ID="lblFormat" runat="server">Format (YYYY-MM-DD)</asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align:right;">
                To Date  <font color="red"><b>*</b></font>
            </td>
            <td>
                <asp:TextBox ID="txtToDate" runat="server" Style="background-image: url(/Images/Calendar_16x16.png);
                                   background-repeat:no-repeat; padding-left:20px;"></asp:TextBox>
                 <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate" Format="yyyy-MM-dd"></asp:CalendarExtender>
                 <asp:Label ID="lblToDate" runat="server">Format (YYYY-MM-DD)</asp:Label>
            </td>
        </tr>
        <tr>
            <td >
                
            </td>
            <td>
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="Print"/>
            </td>
        </tr>
    </table>

  </fieldset>
                
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
