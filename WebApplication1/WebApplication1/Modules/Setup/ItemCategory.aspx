<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="ItemCategory.aspx.cs" Inherits="DXBMS.Modules.Setup.ItemCategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

     <asp:UpdatePanel runat="server">

        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <div>
                        <fieldset>
                            <legend>Item Category </legend>

                            <div class="table-responsive">


                                <table width="70%" style="border-collapse: separate; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">
                                    <tr>
                                        <td class="auto-style1">Item Category Description</td>
                                        <td class="auto-style6">
                                            <asp:TextBox ID="txtItemCatDesc" runat="server" Height="21px" MaxLength="50" Width="300px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style2">
                                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>

                                        <td class="auto-style4" colspan="3">
                                            <asp:Button ID="btnInsert" runat="server" Height="30px" Text="Insert" Width="200px" OnClick="btnInsert_Click" Visible="false"  />
                                        </td>

                                    </tr>

                                </table>


                            </div>

                            <!---- FOR GRIDVIEW -->
                            <div class="row">
                                <div clas="col-lg-12">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:GridView runat="server" ShowHeaderWhenEmpty="true" Width="100%" ID="gv_ItemCat" AutoGenerateColumns="false"  CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" 
                                            OnDataBound="gv_ItemCat_DataBound"
                                            OnRowDataBound="gv_ItemCat_RowDataBound"
                                            OnRowEditing="gv_ItemCat_RowEditing"
                                            OnRowCancelingEdit="gv_ItemCat_RowCancelingEdit"
                                            OnRowUpdating="gv_ItemCat_RowUpdating"
                                            OnPageIndexChanging="gv_ItemCat_SelectedIndexChanged"
                                            AllowPaging="True">

                                          <%--  <PagerStyle CssClass="gridview"></PagerStyle>--%>


                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />

                                            <Columns>

                                                <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image"
                                                    UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>

                                                <asp:TemplateField  HeaderText="Item Category Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemCatCode" runat="server" Text='<%# Bind("ItemCatCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Category Description">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtItemCatDescription" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemCatDesc" runat="server" Text='<%# Bind("ItemCatDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Remove">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="BtnRemove_ItemCategory" runat="server" ImageUrl="~/Images/Delete_16x16.png" OnClick="BtnRemove_ItemCategory_Click" ToolTip="Remove" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>

                                        </asp:GridView>
                                    </td>
                                </tr>

                            </table>
                                </div>
                                </div>

                        </fieldset>

                    </div>
                </div>
            </div>

        </ContentTemplate>

    </asp:UpdatePanel>


</asp:Content>
