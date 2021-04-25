<%@ Page Title="" Language="C#"  MasterPageFile="~/Root.master" AutoEventWireup="true" Theme="Theme1" CodeBehind="Test.aspx.cs" Inherits="DXBMS.Test" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server" ng-app="app">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
                 
               <asp:Label  Font-Size="22px" ForeColor="Red" runat="server">Please Contact to Administrator :For User Right </asp:Label>
      
    


            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
