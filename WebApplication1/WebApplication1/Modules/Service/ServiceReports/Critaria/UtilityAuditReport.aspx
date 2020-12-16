<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="UtilityAuditReport.aspx.cs" Inherits="DXBMS.Modules.Service.ServiceReports.Critaria.UtilityAuditReport" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style5 {
            width: 154px;
        }

        .auto-style6 {
            width: 124px;
        }

        .auto-style7 {
            margin-left: 138;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <div class="col-lg-12">
                        <fieldset>
                            <legend>Utility Audit Report</legend>
                            <div>

                                <table width="100%" style="border-collapse: separate; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">

                                    <tr>
                                        <td align="right" width="50%">Utility Audit</td>

                                        <td >
                                            <asp:DropDownList ID="ddlUtilityAudit" runat="server" Width="234px" OnSelectedIndexChanged="ddlUtilityAudit_SelectedIndexChanged">
                                            </asp:DropDownList>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="right">From Date</td>
                                        <td >
                                            <asp:TextBox ID="txtFromDate" runat="server" Height="21px" Style="background-image: url(../../../../Images/Calendar_16x16.png);
                            background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate" Format="dd-MM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                        

                                    </tr>

                                    <tr>
                                        <td align="right">To Date</td>    
                                        <td >
                                            <asp:TextBox ID="txtToDate" runat="server" Height="21px" Style="background-image: url(../../../../Images/Calendar_16x16.png);
                            background-repeat: no-repeat; padding-left: 25px; width: 100%;"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate" Format="dd-MM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnShow" runat="server" width="200px" Text="Show Detail" OnClick="btnShow_Click" />
                                        </td>                                        
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

