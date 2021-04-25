<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="TaxType.aspx.cs" Inherits="DXBMS.Modules.Setup.TaxType" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="Server">

    <asp:UpdatePanel runat="Server">
        <ContentTemplate>
            <div class="container">
                
                    <div>
                        <fieldset>
                            <legend>Tax Type</legend>
                            <div class="table-reponsive">

                                <div class="table-responsive">


                                    <table width="70px" style="border-collapse: separate; border-spacing: 5px;"
                                        class="table table-condensed table-bordered table-hover">
                                        <tr>
                                            <td class="auto-style1">Tax ID  
                                            </td>
                                            <td class="auto-style6">
                                                <asp:TextBox ID="txtTaxId" runat="server" Width="95%"></asp:TextBox>

                                            </td>
                                            <td class="auto-style1">Tax Head</td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtTaxHead" runat="server" Height="21px" Width="95%"></asp:TextBox>
                                        </tr>
                                        <tr>

                                            <td class="auto-style1">Tax Percentage</td>
                                            <td class="auto-style6">
                                                <asp:TextBox ID="txtTaxPercent" runat="server" Height="21px" Width="95%"></asp:TextBox>
                                            </td>

                                            <%--<asp:TextBox ID="txtAmount" runat="server"  Height="21px" MaxLength="50" Width="97%"></asp:TextBox>--%>
                                              </td>
                                                <td class="auto-style1">Tax Applicable</td>
                                            <td class="auto-style6">
                                                <asp:DropDownList ID="ddlTaxApplicable" runat="server" AutoPostBack="True" Font-Names="monospace" Width="95%">
                                                    <asp:ListItem Value="Services">Services</asp:ListItem>
                                                    <asp:ListItem Value="Part">Part</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td class="auto-style1">Type</td>
                                            <td class="auto-style6">
                                                <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True" Font-Names="monospace" Width="95%">
                                                    <asp:ListItem Value="Payment">Payment</asp:ListItem>
                                                    <asp:ListItem Value="Receipt">Receipt</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>

                                            <td class="auto-style1">Active</td>
                                            <td class="auto-style6">
                                                <asp:CheckBox ID="CheckId" runat="server" Height="31px" Width="95%" />
                                        </tr>
                                        <tr>
                                            <td class="auto-style1">Acc Code &nbsp &nbsp &nbsp &nbsp
                                                     <asp:ImageButton runat="server" ID="imgTaxType" ImageUrl="~/Images/LookupNew.png" OnClick="imgTaxType_Click" />
                                            </td>
                                            <td class="auto-style6">
                                                <asp:TextBox ID="txtAccCode" runat="server" ReadOnly="true" Height="21px" Width="95%"></asp:TextBox>
                                            </td>
                                            <td class="auto-style1">Acc Desc</td>
                                            <td class="auto-style6">
                                                <asp:TextBox ID="TxtAccDesc" runat="server" ReadOnly="true" Height="21px" MaxLength="50" Width="97%"></asp:TextBox>

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

                                            <asp:GridView ID="gv_TaxType" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" Width="100%"
                                                OnDataBound="gv_TaxType_DataBound1"
                                                OnRowDataBound="gv_TaxType_RowDataBound"
                                                OnRowEditing="gv_TaxType_RowEditing"
                                                OnRowCancelingEdit="gv_TaxType_RowCancelingEdit"
                                                OnRowUpdating="gv_TaxType_RowUpdating"
                                                AllowPaging="True">
                                                <PagerStyle CssClass="gridview" />
                                                <HeaderStyle CssClass="GridHeader" />
                                                <FooterStyle CssClass="GridFooter" />
                                                <Columns>
                                                    <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image"
                                                        UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>
                                                    <asp:TemplateField HeaderText="Tax ID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTaxID" runat="server" Text='<%# Bind("TaxID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText=" Tax Head">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtTaaxHead" runat="server"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTaxHead" runat="server" Text='<%# Bind("TaxHead") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText=" TaxPer">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditPer" runat="server"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTaxPercent" runat="server" Text='<%# Bind("TaxPer") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tax Applicable">
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddlEditTaxApplicable" runat="server" AutoPostBack="True" Font-Names="monospace" Width="95%">
                                                                <asp:ListItem Value=""> </asp:ListItem>
                                                                <asp:ListItem Value="Services">Services</asp:ListItem>
                                                                <asp:ListItem Value="Part">Part</asp:ListItem>
                                                            </asp:DropDownList>

                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTaxApplicable" runat="server" Text='<%# Bind("TaxApplicable") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Type">
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddlEditType" runat="server" AutoPostBack="True" Font-Names="monospace" Width="95%">
                                                                <asp:ListItem Value=""> </asp:ListItem>
                                                                <asp:ListItem Value="Payment">Payment</asp:ListItem>
                                                                <asp:ListItem Value="Receipt">Receipt</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTaxType" runat="server" Text='<%# Bind("Type") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="AccCode">
                                                    <EditItemTemplate>
                                                      <asp:DropDownList ID="ddlAccCode" runat="server" AutoPostBack="True" Font-Names="monospace" Width="95%">

                                                      </asp:DropDownList>
                                                    </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAccCode" runat="server" Text='<%# Bind("AccCode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Active">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblActive" runat="server" Text='<%# Bind("Active") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remove">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="BtnRemove_TaxType" runat="server" ImageUrl="~/Images/Delete_16x16.png" OnClick="BtnRemove_TaxType_Click" OnClientClick="return confirm('Are you sure you want to delete ?')" ToolTip="Remove" />
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

        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>
