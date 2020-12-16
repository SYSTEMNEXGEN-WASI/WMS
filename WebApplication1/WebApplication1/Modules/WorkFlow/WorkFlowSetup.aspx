<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="WorkFlowSetup.aspx.cs" Inherits="DXBMS.Modules.WorkFlow.WorkFlowSetup" %>
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
                            <legend>Work Flow Setup </legend>

                            <div class="table-responsive">


                                <table width="70%" style="border-collapse: separate; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">
                                    <tr>
                                        <td class="auto-style1">ID</td>
                                        <td class="auto-style6">
                                            <asp:TextBox ID="txtWorkFlowID" runat="server" ReadOnly="True" Height="21px" MaxLength="50" Width="300px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style1">WorkFlow Name</td>
                                        <td class="auto-style6">
                                            <asp:TextBox ID="txtWorkFlowDesc" runat="server" Height="21px" MaxLength="50" Width="300px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style2">
                                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="auto-style4" colspan="3">
                                            <asp:Button ID="btnInsert" runat="server" Height="30px" Text="Insert" Width="200px" OnClick="btnInsert_Click" />
                                        </td>
                                    </tr>

                                </table>


                            </div>

                            <!---- FOR GRIDVIEW -->

                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:GridView runat="server" ShowHeaderWhenEmpty="true" Width="100%" ID="gv_WorkFlow" AutoGenerateColumns="false"
                                            OnDataBound="gv_WorkFlow_DataBound"
                                            OnRowDataBound="gv_WorkFlow_RowDataBound"
                                            OnRowEditing="gv_WorkFlow_RowEditing"
                                            OnRowCancelingEdit="gv_WorkFlow_RowCancelingEdit"
                                            OnRowUpdating="gv_WorkFlow_RowUpdating"
                                            
                                            OnPageIndexChanging="gv_WorkFlow_SelectedIndexChanged"
                                            AllowPaging="True">

                                            <%--
                                                 OnDataBound="gv_ItemCat_DataBound"
                                            OnRowDataBound="gv_ItemCat_RowDataBound"
                                            OnRowEditing="gv_ItemCat_RowEditing"
                                            OnRowCancelingEdit="gv_ItemCat_RowCancelingEdit"
                                            OnRowUpdating="gv_ItemCat_RowUpdating"--%>


                                            <PagerStyle CssClass="gridview"></PagerStyle>


                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />

                                            <Columns>

                                                <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image"
                                                    UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>

                                                <asp:TemplateField HeaderText="WorkFlow Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWorkFlowCode" runat="server" Text='<%# Bind("WFCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="WorkFlow Description">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtWorkFlowDescEdit" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWorkFlowDesc" runat="server" Text='<%# Bind("WFDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Remove">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="BtnRemove_WorkFlowSetup" runat="server" ImageUrl="~/Images/Delete_16x16.png" OnClick="BtnRemove_WorkFlowSetup_Click" ToolTip="Remove" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>

                                        </asp:GridView>
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
