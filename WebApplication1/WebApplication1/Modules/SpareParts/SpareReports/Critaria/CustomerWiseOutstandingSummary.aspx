<%@ Page Title="" Language="C#" Theme="Theme1" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="CustomerWiseOutstandingSummary.aspx.cs" Inherits="DXBMS.Modules.Reports.CustomerWiseOutstandingSummary" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">



    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <div class="container">
        
            <fieldset>
        <legend><asp:Label runat="server" ID="lblHeading"></asp:Label></legend>
        <table width="100%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
            
            <tr >
                <td align="right" >
                    <asp:Label ID="lblCustomer" runat="server" Visible="false">Customer</asp:Label>
                </td>
                <td>
                    <asp:ImageButton runat="server" ID="imgLookup" ImageUrl="~/Images/LookupNew.png"  OnClick="btnLookup_Click" Visible="false" />
                    <asp:TextBox ID="txtCustomer" runat="server" Width="30%" Visible="false"></asp:TextBox><asp:TextBox ID="txtCustomerDesc" runat="server" Width="60%" Visible="true"></asp:TextBox>
                </td>
               
            </tr>
            <tr>                
                <td align="right" width="50%">
                    FromDate:
                </td>
                <td width="50%">
                    <asp:TextBox ID="txtFromDate" runat="server" Width="150px" Style="background-image: url(../../../../Images/Calendar_16x16.png);
                                           background-repeat: no-repeat; padding-left: 25px;" ></asp:TextBox>
                    <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" 
                        Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtFromDate">
                    </asp:CalendarExtender>
                </td>
                </tr>
            
        <tr>
                <td align="right">
                    ToDate:
                </td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" Width="150px" Style="background-image: url(../../../../Images/Calendar_16x16.png);
                                           background-repeat: no-repeat; padding-left: 25px;" ></asp:TextBox>
                    <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" 
                        Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtToDate">
                    </asp:CalendarExtender>
                </td>
            </tr>
            
        <tr>
            
            <td>
            </td>
            <td align="left" style="width: 15%">
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" />
                   <asp:Button ID="btnClear" runat="server" Text="Clear" onclick="btnClear_Click" />
            </td>
        </tr>
        </table>
    </fieldset>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
