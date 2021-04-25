<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="POView.aspx.cs" Inherits="DXBMS.Modules.Criteria.POView" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <table style="border-right: 1px">
        <tr>
            <td>
                <asp:ImageButton ID="btnPrint" runat="server" ImageUrl="~/Images/Print.png" OnClick="btnPrint_Click"
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

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" onunload="CrystalReportViewer1_Unload" ToolPanelView="None" 
        Width="350px" />
    </asp:Content>
