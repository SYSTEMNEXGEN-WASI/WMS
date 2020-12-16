﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OpenPdf.aspx.cs" Inherits="DXBMS.Download.OpenPdf" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <table style="border-right: 1px">
        <tr>
            <td>
                <asp:ImageButton ID="btnPrin" runat="server" ImageUrl="~/Images/Print.png" OnClick="btnPrin_Click"
                    ToolTip="Print" target="_blank" />
            </td>
            <td>
                <asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/Images/Export.png" OnClick="btnExport_Click"
                    ToolTip="Export" />
            </td>
            <td>
                <asp:ImageButton ID="btnFirst" runat="server" ImageUrl="~/Images/First.png" OnClick="btnFirst_Click"
                    ToolTip="Go to Firt Page" />
            </td>
            <td>
                <asp:ImageButton ID="btnPervious" runat="server" ImageUrl="~/Images/Previous.png"
                    OnClick="btnPervious_Click" ToolTip="Go to Previous Page" />
            </td>
            <td>
                <asp:ImageButton ID="btnNext" runat="server" ImageUrl="~/Images/Next.png" OnClick="btnNext_Click1"
                    ToolTip="Go to Next Page" />
            </td>
            <td>
                <asp:ImageButton ID="btnLast" runat="server" ImageUrl="~/Images/Last.png" OnClick="btnLast_Click"
                    ToolTip="Go to Last Page" />
            </td>
            <td>
                <asp:ImageButton ID="btnNavigation" runat="server" ImageUrl="~/Images/Navigation.png"
                    OnClick="btnNavigation_Click" ToolTip="Page Navigation" />
            </td>
            <td>
                <asp:TextBox ID="txtPageNo" runat="server" Width="74px" MaxLength="6"></asp:TextBox>
                
            </td>
        </tr>
    </table>
    <div>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
    </div>
    </form>
</body>
</html>
