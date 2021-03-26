<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="CVO.aspx.cs" Inherits="DXBMS.Modules.Setup.CVO" %>

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
                            <legend>CVO</legend>
                            <div class="table-reponsive">

                                <div class="table-responsive">


                                    <table width="70px" style="border-collapse: separate; border-spacing: 5px;"
                                        class="table table-condensed table-bordered table-hover">
                                        <tr>
                                            <td class="auto-style1">CVO  
                                                    <asp:ImageButton runat="server" ID="imgCVO" ImageUrl="~/Images/LookupNew.png" OnClick="imgCVO_Click" /></td>
                                            <td class="auto-style6">
                                                <asp:TextBox ID="txtCVONo" ReadOnly="true" runat="server" Width="95%"></asp:TextBox>
                                                <%--<asp:DropDownList ID="ddlCvoNo" runat="server" AutoPostBack="True" Font-Names="monospace" Width="234px">
                                                        
                                                    </asp:DropDownList>--%>
                                            </td>
                                            <td class="auto-style1">CVO-Date</td>
                                            <td>
                                                <asp:TextBox ID="txtCVODate" runat="server" TabIndex="17" ReadOnly="True" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px; width: 95%;"></asp:TextBox>

                                                <asp:CalendarExtender ID="txtRecieptDate_CalendarExtender" runat="server"
                                                    Enabled="True" Format="dd-MM-yyyy" TargetControlID="txtCVODate" />
                                            </td>
                                            <td class="auto-style1">Reg &nbsp &nbsp &nbsp &nbsp
                                                    <asp:ImageButton runat="server" ID="imgVehRegNo" ImageUrl="~/Images/LookupNew.png" OnClick="imgVehRegNo_Click" />
                                            </td>
                                            <td class="auto-style6">

                                                <asp:TextBox ID="txtreg" runat="server" ReadOnly="true" Height="21px" MaxLength="50" Width="95%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style1">Chassis No</td>
                                            <td class="auto-style6">
                                                <asp:TextBox ID="txtChassisNo" runat="server" ReadOnly="true" Height="21px" MaxLength="50" Width="95%"></asp:TextBox>
                                            </td>
                                            <td class="auto-style1">Engine No</td>
                                            <td class="auto-style6">
                                                <asp:TextBox ID="txtEngineNo" runat="server" ReadOnly="true" Height="21px" MaxLength="50" Width="95%"></asp:TextBox>
                                            </td>
                                            <td class="auto-style1">Vehicle</td>
                                            <td class="auto-style6">
                                                <asp:TextBox ID="txtVehicle" runat="server" ReadOnly="true" Height="21px" MaxLength="50" Width="95%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style1">Color</td>
                                            <td class="auto-style6">
                                                <asp:TextBox ID="txtColor" runat="server" ReadOnly="true" Height="21px" MaxLength="50" Width="95%"></asp:TextBox>
                                            </td>
                                            <td class="auto-style1">Ownership Type</td>
                                            <td class="auto-style6">
                                                <asp:DropDownList ID="ddlownership" runat="server" AutoPostBack="True" Font-Names="monospace" Width="95%">
                                                    <asp:ListItem Value="Customer">Customer</asp:ListItem>
                                                    <asp:ListItem Value="EndUser">End User</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td class="auto-style1">Old Customer</td>
                                            <td class="auto-style6">
                                                <asp:TextBox ID="txtOldCusCode" runat="server" ReadOnly="true" Width="22%"></asp:TextBox>
                                                <asp:TextBox ID="txtOldcustomer" runat="server" ReadOnly="true" Width="70%"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td class="auto-style1">New Customer &nbsp
                                                    <asp:ImageButton runat="server" ID="imgNewCus" ImageUrl="~/Images/LookupNew.png" OnClick="imgNewCus_Click" />
                                            </td>
                                            <td class="auto-style6">
                                                <asp:TextBox ID="txtNewcustomer" runat="server" ReadOnly="true" Height="21px" MaxLength="50" Width="22%"></asp:TextBox>
                                                <asp:TextBox ID="txtNewCusDesc" runat="server" ReadOnly="true" Height="21px" MaxLength="50" Width="70%"></asp:TextBox>
                                            </td>

                                            <td class="auto-style1">Remarks</td>
                                            <td class="auto-style6" colspan="3">
                                                <asp:TextBox ID="txtRemarks" runat="server" Height="21px" MaxLength="100" Width="97%"></asp:TextBox>
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

                                            <asp:GridView ID="gv_CVO" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" Width="100%"
                                                OnDataBound="gv_CVO_DataBound"
                                                OnRowDataBound="gv_CVO_RowDataBound">
                                                <%--        OnRowEditing="gv_Unit_RowEditing"
                                                OnRowCancelingEdit="gv_Unit_RowCancelingEdit"
                                                OnRowUpdating="gv_Unit_RowUpdating"
                                                OnPageIndexChanging="gv_Unit_SelectedIndexChanged"
                                                AllowPaging="True"--%>
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
                                                    <asp:TemplateField HeaderText="CVO NO">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCvoNo" runat="server" Text='<%# Bind("CVONo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="CVO Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCVODate" runat="server" Text='<%# Bind("CVODate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Ownership Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOwnertyp" runat="server" Text='<%# Bind("OwnerShipType") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Old Customer">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOldCustomer" runat="server" Text='<%# Bind("OldCusCode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="New Customer">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNewCust" runat="server" Text='<%# Bind("NewCusCode") %>'></asp:Label>
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
