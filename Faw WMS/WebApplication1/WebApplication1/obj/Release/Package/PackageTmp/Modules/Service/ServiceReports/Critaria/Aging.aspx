<%@ Page Title="" Language="C#" Theme="Theme1" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="Aging.aspx.cs" Inherits="DXBMS.Modules.Service.ServiceReports.Critaria.Aging" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

            <script src="../../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../../js/chosen.jquery.min.js" type="text/javascript"></script>       


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
            <div class="Heading">AGING REPORT</div>
            <fieldset>
            <table width="50%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                
                <tr> 
                    <td width="50%" align="right">
                    From Date
                        
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd-MM-yyyy" TargetControlID="txtFromDate"/>
                    </td>
                   
                </tr>
                <tr>
                    <td align="right">
                    To Date
                    </td>
                    <td>
                    <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" Format="dd-MM-yyyy" TargetControlID="txtToDate"/>
                    </td>
                    
                </tr>
                <tr>
                <td></td>
                <td>
                    <asp:Button ID="btnPrint" runat="server" Text="Show Report" Width="110px" OnClientClick="Print()" onclick="btnPrint_Click"/>
                </td>               
                </tr>
            </table>
            </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
