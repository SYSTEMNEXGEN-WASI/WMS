<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Root.master" CodeBehind="Default.aspx.cs" Inherits="DXBMS._Default" %>

<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
 <style type="text/css">
     .ClsDiv
     {
         width :100%;
         vertical-align:middle;
         text-align : center;
         font-size:15px;
         
     }
     .img
     {
         opacity:0.2; filter:alpha(opacity=100);
     }


 </style>   
    <div class="ClsDiv">

        <iframe width="800" height="600" src="https://app.powerbi.com/view?r=eyJrIjoiM2FmYzdjZDEtOTYzNS00MGFiLWJlNGMtZTFkZDEwMjA4NTUwIiwidCI6IjZkYmQxYWQ5LTZlYmQtNDJiNy04MTNkLTBkNmQyM2NmMzgwMyIsImMiOjl9" frameborder="0" allowFullScreen="true"></iframe>
        <%--<asp:Image ID="Image1" runat="server" ImageUrl="~/Images/HinoLog.jpg" Width ="700px" Height="700px" CssClass="img" />--%>
    </div>
    <div class="ClsDiv">
    <%--<asp:Image ID="Image2" runat="server" ImageUrl="~/Images/Hino_Loc.jpg" Width ="100%" Height="40px"  />--%>

    </div>
    <br />
    <%--<div class="ClsDiv">
        Z-3, Quaid-e-Azam Truck Stand, Main Hawksbay Road, Karachi-Pakistan.<br />
        Tel: +92-213-2373176-9, E-mail: info@hinohawksbay.com, Web.: www..hinohawksbay.com

    </div>--%>
   <%--<dx:ASPxImage ID="ASPxImage1" runat="server" ShowLoadingImage="true" 
        Height="100%" ImageUrl="~/Images/Hino.jpg" Width="100%">
    </dx:ASPxImage> --%>
   
    
</asp:Content>

