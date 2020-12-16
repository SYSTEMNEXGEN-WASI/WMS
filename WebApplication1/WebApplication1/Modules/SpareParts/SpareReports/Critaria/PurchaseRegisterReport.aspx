<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="PurchaseRegisterReport.aspx.cs" Inherits="DXBMS.Modules.SpareParts.SpareReports.Critaria.PurchaseRegisterReport" %>


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
                    <div>
                        <fieldset>
                            <legend>Purchase Register Report 2</legend>
                            <div class="table-responsive">

                                <table width="100%" style="border-collapse: separate; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">

                                    <tr>
                                        <td width="50%" align="right">Vendor
                                        </td>

                                        <td >
                                            <asp:DropDownList ID="ddlVendor" runat="server" Width="234px" >
                                            </asp:DropDownList>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td  align="right">From Date</td>
                                        
                                        <td >
                                            <asp:TextBox ID="txtFromDate" runat="server" Height="21px" MaxLength="1" Style="background-image: url(/Images/Calendar_16x16.png); 
                                   background-repeat:no-repeat; padding-left:20px;"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate" Format="dd-MM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="right">To Date                                          
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtToDate" runat="server" Height="21px" MaxLength="1"  Style="background-image: url(/Images/Calendar_16x16.png); 
                                   background-repeat:no-repeat; padding-left:20px;"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate" Format="dd-MM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>

                                    <tr>
                                    <td>
                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnShow" runat="server" Height="30px" Text="Show Detail" Width="200px" OnClick="btnShow_Click" />
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
