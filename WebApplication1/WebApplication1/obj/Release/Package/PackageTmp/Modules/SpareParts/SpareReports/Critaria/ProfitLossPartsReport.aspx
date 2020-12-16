<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" Theme="Theme1" AutoEventWireup="true" CodeBehind="ProfitLossPartsReport.aspx.cs" Inherits="DXBMS.Modules.Service.ServiceReports.Critaria.ProfitLossPartsReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="container">
                    <div class="Heading">Part Out Flow Report</div>
                    <fieldset>
                        <table width="50%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                            
                <tr> 
                    <td width="50%" align="right">
                    From Date
                        <%--<CR:CrystalReportViewer ID="crviewer" runat="server" AutoDataBind="true" />--%>
                    </td>
                    <td>
                        <asp:TextBox ID="dtFrom" runat="server" Style="background-image: url(../../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd-MM-yyyy" TargetControlID="dtFrom"/>
                    </td>
                   
                </tr>
                <tr>
                    <td align="right">
                    To Date
                    </td>
                    <td>
                    <asp:TextBox ID="dtTo" runat="server" Style="background-image: url(../../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" Format="dd-MM-yyyy" TargetControlID="dtTo"/>
                    </td>
                    
                </tr>
                <tr>
                <td><asp:Label ID="lblMSG" runat="server" Text="" ForeColor="Red" Font-Size="Large" Font-Bold="true"></asp:Label></td>
                <td><asp:Button ID="btnShowReport" runat="server" Text="Show Report" Width="110px"  onclick="btnShowReport_Click"/></td> <%--OnClientClick="Print()"--%>
                
                </tr>
            </table>
                    </fieldset>

                   
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
