<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="InputType.aspx.cs" Inherits="DXBMS.Modules.Setup.InputType" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="Jquery/jquery.min.js"></script>
    <script src="js/jquery-3.2.1.min.js" type="text/javascript"></script>
    <link href="css/Customs.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {
            width: 268435280px;
        }

        .auto-style2 {
            width: 820px;
        }
    </style>

    <%--<meta name="viewport" content="width=device-width, initial-scale=1.0" />--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="Server">
    <asp:UpdatePanel runat="server">

        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <div>
                        <fieldset>
                            <legend>Input Type </legend>

                            <div class="table-responsive">


                                <table width="70%" style="border-collapse: separate; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">

                                    <tr>
                                        <td class="auto-style80">From Date:</td>
                                        <td class="auto-style74">
                                            <asp:TextBox ID="txtfromdate" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;" Width="200px"></asp:TextBox>
                                            <asp:CalendarExtender runat="server" TargetControlID="txtfromdate" Format="dd-MM-yyyy"></asp:CalendarExtender>
                                        </td>
                                        <td class="auto-style80">To Date:</td>
                                        <td class="auto-style74">
                                            <asp:TextBox ID="txtToDate" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;" Width="200px"></asp:TextBox>
                                            <asp:CalendarExtender runat="server" TargetControlID="txtToDate" Format="dd-MM-yyyy"></asp:CalendarExtender>
                                        </td>

                                    </tr>

                                    <tr>
                                        <td class=" auto-style4" colspan="3">
                                            <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label></td>

                                        <td class="auto-style1">
                                            <asp:Button ID="btnCLear" runat="server" Height="30px" Text="Clear" Width="150px" />

                                            <asp:Button ID="btnPrint" runat="server" Height="30px" Text="Print" Width="150px" OnClick="btnPrint_Click" />
                                        </td>

                                    </tr>
                                </table>
                            </div>

                        </fieldset>
                    </div>
                </div>
            </div>

            </table>

        </ContentTemplate>

    </asp:UpdatePanel>


</asp:Content>


