<%@ Page Title="Stock Status Report" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="StandardPartStockStatusReportNew.aspx.cs" Inherits="DXBMS.Modules.SpareParts.SpareReports.Critaria.StandardPartStockStatusReportNew" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="container">
                    <div class="row">
                        <div class="col-lg-12">
                            <fieldset>
                                <legend>STANDARD PART STOCK STATUS REPORT</legend>
                                <table style="border-collapse: separate; width:100%; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">
                                    <tr>
                                        <td width="15%">
                                            <asp:Label runat="server" Text="Check All" style="font-weight: 700; font-style: normal; font-size: small;"></asp:Label>
                                            <asp:CheckBox ID="chkAllItem" runat="server" AutoPostBack="True" 
                                                Checked="True" CheckState="Checked" 
                                                OnCheckedChanged="chkAllItem_CheckedChanged" >
                                            </asp:CheckBox>
                                        </td>
                                        <td width="14%">
                                            <span>From Item No</span>
                                            <asp:ImageButton runat="server" ID="imgLookup" ImageUrl="~/Images/LookupNew.png" Enabled="false" OnClick="btnLookup_Click" />
                                            <%--<asp:DropDownList ID="ddlFromCode" width="100%" runat="server" AutoPostBack="True" Enabled="false"
                                                onselectedindexchanged="glFromItemPartNo_TextChanged"></asp:DropDownList>--%>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFromItemcode" runat="server" ReadOnly="true" Width="100%"></asp:TextBox>
                                        </td>
                                        <td width="15%">Part Description</td>
                                        <td>
                                            <asp:TextBox ID="txtPartItemDescFrom" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <span>To Item No</span>
                                            <asp:ImageButton runat="server" ID="imgLookup2" ImageUrl="~/Images/LookupNew.png" Enabled="false" OnClick="imgLookup2_Click" />
                                            <%--<asp:DropDownList ID="ddlToCode" width="100%" runat="server" AutoPostBack="True" Enabled="false"
                                             onselectedindexchanged="glToItemPartNo_TextChanged"></asp:DropDownList>--%>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtToItemcode" runat="server" ReadOnly="true" Width="100%"></asp:TextBox>
                                        </td>
                                        <td>Part Description</td>
                                        <td>
                                            <asp:TextBox ID="txtPartItemDescTo" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                             <asp:RadioButtonList ID="radioItemType" runat="server" 
                                                RepeatDirection="Horizontal" Width="100%" >
                                                <Items>
                                                    <asp:ListItem Text="Local" Value="Local" />
                                                    <asp:ListItem Text="C.K.D." Value="C.K.D." />
                                                    <asp:ListItem Text="Market Parts" Value="In - House" />
                                                    <asp:ListItem Text="Lubes" Value="Lubes" />
                                                    <asp:ListItem Text="HGO" Value="HGO" />
                                                    <asp:ListItem Text="All" Value="--Select--" Selected="True" />
                                                </Items>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblErr" runat="server" Text="" ForeColor="#669900"></asp:Label>
                                        </td>
                                        <td colspan="4">
                                            <asp:Button ID="btnPrintLocRpt" runat="server" Text="Stock Report Location Wise" style="Width:auto;" OnClick="btnPrintLocRpt_Click" ></asp:Button>
                                            <asp:Button ID="btnPrint" runat="server" Text="Stock Report" style="Width:auto;" OnClick="btnPrint_Click"></asp:Button>
                                            <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
