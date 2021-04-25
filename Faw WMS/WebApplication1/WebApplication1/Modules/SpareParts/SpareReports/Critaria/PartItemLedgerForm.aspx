<%@ Page Language="C#" AutoEventWireup="true" Theme="Theme1" CodeBehind="PartItemLedgerForm.aspx.cs" MasterPageFile="~/Root.master" Inherits="DXBMS.Modules.Reports.PartItemLedgerForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>



<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
                <div style="width: 100%">
                    <fieldset>
                        <legend>Item Stock Ledger Report</legend>

                        <table style="width: 100%; border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                            <tr>
                                <td width="10%">
                                    <asp:Label ID="Label4" runat="server" Text="From Code"></asp:Label>
                                    <asp:ImageButton runat="server" ID="imgLookup" ImageUrl="~/Images/LookupNew.png" OnClick="btnLookup_Click" />
                                </td>
                                <td style="width: 40%">
                                    <asp:TextBox ID="txtFromItemCode" runat="server" ReadOnly="true" Width="24%"></asp:TextBox>
                                   
                                    <asp:TextBox ID="txtFromItemDesc" runat="server" ReadOnly="true" Width="75%"></asp:TextBox>
                                </td>
                                <td width="10%">
                                    <asp:Label ID="Label1" runat="server" Text="To Code"></asp:Label>
                                    <asp:ImageButton runat="server" ID="imgLookup2" ImageUrl="~/Images/LookupNew.png" OnClick="imgLookup2_Click" />
                                </td>
                                <td style="width: 50%">
                                    <asp:TextBox ID="txtToItemCode" runat="server" ReadOnly="true" Width="24%"></asp:TextBox>
                                   
                                    <asp:TextBox ID="txtToItemDesc" runat="server" ReadOnly="true" Width="75%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="From Date"></asp:Label>
                                </td>
                                <td>

                                    <asp:TextBox ID="txtFromDate" runat="server" Style="background-image: url(../../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px; width: 96px"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" Enabled="True" Format="dd-MM-yyyy" TargetControlID="txtFromDate"></asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="To Date"></asp:Label></td>
                                <td>

                                    <asp:TextBox ID="txtToDate" runat="server" Style="background-image: url(../../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px; width: 96px"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Enabled="True" Format="dd-MM-yyyy" TargetControlID="txtToDate"></asp:CalendarExtender>
                                </td>

                            </tr>

                            <tr>

                                <td colspan="2">
                                    <asp:Label ID="lblMsg" runat="server"></asp:Label></td>
                                <td colspan="2" align="left">

                                    <asp:Button ID="btnPrint" runat="server" Text="Print"
                                        OnClick="btnPrint_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Clear" OnClick="btnCancel_Click" />
                                </td>
                            </tr>

                        </table>
                    </fieldset>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
--%>