<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" Theme="Theme1" AutoEventWireup="true" CodeBehind="DTRDetail.aspx.cs" Inherits="DXBMS.Modules.Service.ServiceReports.Critaria.DTRDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
       <%-- <script src="../../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../../js/chosen.jquery.min.js" type="text/javascript"></script>--%>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="container">
                    <div class="Heading">DTR  DETAIL REPORT</div>
                    <fieldset>
                        <table width="50%" style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
                            <tr>
                                <td align="right">Report of</td>
                                <td  align="right">
                                    <asp:RadioButtonList ID="rbStatus" runat="server" RepeatDirection="Horizontal" Width="100%" AutoPostBack="true">
                                        <asp:ListItem Selected="True" Text="All" Value="All" />
                                        <asp:ListItem Text="HCV" Value="HCV" />
                                        <asp:ListItem Text="LCV" Value="LCV" />
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
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
        
        <script type="text/javascript">
            function loadPDF() {
                window.open('GeneratePDF.aspx', '', 'scrollbars=no,menubar=no,height=600,width=800,resizable=yes,toolbar=no,location=no,status=no');
            }
</script>

</asp:Content>
