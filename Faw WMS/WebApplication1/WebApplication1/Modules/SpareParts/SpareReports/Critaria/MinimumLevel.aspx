<%@ Page Title="" Language="C#" Theme="Theme1" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="MinimumLevel.aspx.cs" Inherits="DXBMS.Modules.SpareParts.SpareReports.Critaria.MinimumLevel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
                <div style="width: 100%">    
                    <fieldset>
                        <legend>Minimum Level</legend>
                        <table style="width: 100%; border-collapse: separate; border-spacing: 5px;" class="table table-condensed table-bordered table-hover">
                            <tr>
                                <td width="20%" align="right" >
                                    <asp:Label ID="Label5" runat="server" Text="From Code"></asp:Label>
                                    <asp:ImageButton runat="server" ID="imgLookup" ImageUrl="~/Images/LookupNew.png" OnClick="imgLookup3_Click" />
                                </td>
                                <td style="width: 50%">
                                    <asp:TextBox ID="txtMLItemCode" runat="server" ReadOnly="true" Width="24%"></asp:TextBox>                                  
                                    <asp:TextBox ID="txtMLItemDesc" runat="server" ReadOnly="true" Width="75%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%" align="right">
                                    <asp:Button ID="Button1" runat="server" Text="Print" OnClick="btnReorderRpt" />
                                </td>
                                <td>
                                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                                </td>                               
                            </tr>
                        </table>
                    </fieldset>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
