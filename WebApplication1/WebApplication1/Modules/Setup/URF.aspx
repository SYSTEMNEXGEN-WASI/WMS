<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" Theme="Theme1" AutoEventWireup="true" CodeBehind="URF.aspx.cs" Inherits="DXBMS.Modules.Setup.URF" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="Server">
    <asp:UpdatePanel runat="Server">
        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <div>
                        <fieldset>
                            <legend>Update Registration Form</legend>
                            <div class="table-reponsive">

                                <div class="table-responsive">


                                    <table width="70px" style="border-collapse: separate; border-spacing: 5px;"
                                        class="table table-condensed table-bordered table-hover">
                                        <tr>
                                            <td class="auto-style1" style="display:none">URF  
                                            </td>
                                            <td class="auto-style6" style="display:none">
                                                <asp:TextBox ID="txtURFNo" ReadOnly="true" runat="server" Width="95%"></asp:TextBox>
                                                <%--<asp:DropDownList ID="ddlCvoNo" runat="server" AutoPostBack="True" Font-Names="monospace" Width="234px">
                                                        
                                                    </asp:DropDownList>--%>
                                            </td>
                                            <td class="auto-style1" style="display:none">URF-Date</td>
                                            <td style="display:none">
                                                <asp:TextBox ID="txtURFDate" runat="server" TabIndex="17" ReadOnly="True" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px; width: 95%;"></asp:TextBox>

                                                <asp:CalendarExtender ID="txtRecieptDate_CalendarExtender" runat="server"
                                                    Enabled="True" Format="dd-MM-yyyy" TargetControlID="txtURFDate" />
                                            </td>
                                            <td class="auto-style1">Chassis No &nbsp &nbsp &nbsp &nbsp
                                                    <asp:ImageButton runat="server" ID="imgVehChassisNo" ImageUrl="~/Images/LookupNew.png" OnClick="imgVehChassisNo_Click" />
                                            </td>
                                            <td class="auto-style6">

                                                <asp:TextBox ID="txtChassis" runat="server" ReadOnly="true" Height="21px" MaxLength="50" Width="95%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <%-- <td class="auto-style1">REG No</td>
                                                <td class="auto-style6">
                                                    <asp:TextBox ID="txtregNo" runat="server" ReadOnly="true" Height="21px" MaxLength="50" Width="95%"></asp:TextBox>
                                                </td>--%>
                                            <td class="auto-style1">Engine No</td>
                                            <td class="auto-style6">
                                                <asp:TextBox ID="txtEngineNo" runat="server" ReadOnly="true" Height="21px" MaxLength="50" Width="95%"></asp:TextBox>
                                            </td>
                                            <td class="auto-style1">Vehicle</td>
                                            <td class="auto-style6">
                                                <asp:TextBox ID="txtVehicle" runat="server" ReadOnly="true" Height="21px" MaxLength="50" Width="95%"></asp:TextBox>
                                            </td>
                                            <td class="auto-style1">REG &nbsp
                                                  
                                            </td>
                                            <td class="auto-style6">
                                                <asp:TextBox ID="txtNewreg" runat="server" Height="21px" MaxLength="50" Width="95%"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td class="auto-style1">Color</td>
                                            <td class="auto-style6">
                                                <asp:TextBox ID="txtColor" runat="server" ReadOnly="true" Height="21px" MaxLength="50" Width="95%"></asp:TextBox>
                                            </td>
                                            <%--<td class="auto-style1">Ownership Type</td>
                                                <td class="auto-style6">
                                                    <asp:DropDownList ID="ddlownership" runat="server" AutoPostBack="True" Font-Names="monospace" Width="95%">
                                                        <asp:ListItem Value="Customer">Customer</asp:ListItem>
                                                         <asp:ListItem Value="EndUser">End User</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>--%>
                                            <td class="auto-style1">Customer</td>
                                            <td class="auto-style6">
                                                <asp:TextBox ID="txtCusDesc" runat="server" ReadOnly="true" Width="95%"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>


                                            <td class="auto-style1" style="display:none">Remarks</td>
                                            <td class="auto-style6" style="display:none" colspan="3">
                                                <asp:TextBox ID="txtRemarks" runat="server" Height="21px" MaxLength="100" Width="98%"></asp:TextBox>
                                            </td>




                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:Button ID="btnInsert" runat="server" Height="30px" Width="48%" Text="Save" OnClick="btnInsert_Click" />


                                                <asp:Button ID="btnClear" runat="server" Height="30px" Width="48%" Text="Clear" OnClick="btnClear_Click" />

                                            </td>
                                        </tr>

                                    </table>

                                </div>
                                <!---- FOR GRIDVIEW -->
                                <table width="100%">
                                    <tr>

                                        <td>

                                            <asp:GridView ID="gv_URF" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" Width="100%"
                                                OnDataBound="gv_URF_DataBound"
                                                OnRowDataBound="gv_URF_RowDataBound"
                                                OnPageIndexChanging="gv_URF_PageIndexChanging"
                                                AllowPaging="True">
                                                <PagerStyle CssClass="gridview" />
                                                <HeaderStyle CssClass="GridHeader" />
                                                <FooterStyle CssClass="GridFooter" />
                                                <Columns>

                                                    <%-- <asp:TemplateField HeaderText="Serial no">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSerialno" runat="server" Text='<%# Bind("SerialNo") %>'></asp:Label>

                                                        </ItemTemplate>
                                                        <asp:ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="URF NO">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblURFNo" runat="server" Text='<%# Bind("URFNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="URF Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblURFDate" runat="server" Text='<%# Bind("URFDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText=" Chassis No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblChassisNo" runat="server" Text='<%# Bind("ChassisNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Reg No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblReg" runat="server" Text='<%# Bind("RegNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remarks">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Remarks") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>
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
