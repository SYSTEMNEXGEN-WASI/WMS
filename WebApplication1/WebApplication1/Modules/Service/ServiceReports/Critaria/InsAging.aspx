<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" Theme="Theme1" CodeBehind="InsAging.aspx.cs" Inherits="DXBMS.Modules.Service.ServiceReports.Critaria.InsAging" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

        <script src="../../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../../js/chosen.jquery.min.js" type="text/javascript"></script>
        


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
            <div class="Heading">Insurance Company Aging </div>
            <fieldset>
            <table width="50%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                <tr id="CustomerRow">
                    <td align="right">Ins Company 
                        <asp:ImageButton runat="server" ID="imgLookup" ImageUrl="~/Images/LookupNew.png" OnClick="imgLookup_Click" />
                    </td>
                    <td>
                       <asp:TextBox runat="server" ID="txtInsCompCode" ReadOnly="true" SkinID="ROTextBox" Width="25%"></asp:TextBox>
                       <asp:TextBox runat="server" ID="txtInsCompDesc" ReadOnly="true" SkinID="ROTextBox" Width="65%"></asp:TextBox>
                    </td>
                </tr>
                <tr id="InsRow">
                    <td align="right">Ins Branch 
                        <asp:ImageButton runat="server" ID="imgInsBr" ImageUrl="~/Images/LookupNew.png" OnClick="imgInsBr_Click" />
                    </td>
                    <td>
                       <asp:TextBox runat="server" ID="txtInsBrCode" ReadOnly="true" SkinID="ROTextBox" Width="25%"></asp:TextBox>
                       <asp:TextBox runat="server" ID="txtInsBrDesc" ReadOnly="true" SkinID="ROTextBox" Width="65%"></asp:TextBox>
                    </td>
                </tr>
                <tr> 
                    <td width="50%" align="right">
                    From Date
                        <%--<CR:CrystalReportViewer ID="crviewer" runat="server" AutoDataBind="true" />--%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" Style="background-image: url(../../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd-MM-yyyy" TargetControlID="txtFromDate"/>
                    </td>
                   
                </tr>
                <tr>
                    <td align="right">
                    To Date
                    </td>
                    <td>
                    <asp:TextBox ID="txtToDate" runat="server" Style="background-image: url(../../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" Format="dd-MM-yyyy" TargetControlID="txtToDate"/>
                    </td>
                    
                </tr>
                <tr>
                <td></td>
                <td><asp:Button ID="btnPrint" runat="server" Text="Show Report" Width="110px" onclick="btnPrint_Click"/></td> <%----%>
                
                </tr>
            </table>
            </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    

</asp:Content>
