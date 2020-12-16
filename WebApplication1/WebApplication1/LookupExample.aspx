<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="LookupExample.aspx.cs" Inherits="DXBMS.LookupExample" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

    <div>
        <asp:TextBox ID="TextBox1" runat="server" Text="Test"></asp:TextBox>
        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        <asp:Button ID="btnLookup" runat="server" Text="Lookup" 
            onclick="btnLookup_Click" />
    </div>

</asp:Content>
