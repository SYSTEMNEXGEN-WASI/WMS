<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="Solution.aspx.cs" Inherits="DXBMS.Modules.SpareParts.Forms.Solution" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="Jquery/jquery.min.js"></script>
    <script src="js/jquery-3.2.1.min.js" type="text/javascript"></script>
    <link href="css/Customs.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style2 {
            width: 459px;
        }

        .auto-style8 {
            width: 374px;
        }

        .auto-style9 {
            width: 457px;
        }

        .auto-style10 {
            width: 269px;
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
                            <legend>Solution </legend>

                            <div class="table-responsive">


                                <table width="70%" style="border-collapse: separate; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">

                                    <tr>
                                        <%--<td class="auto-style10">ID</td>
                                        <td class="auto-style9"></td>--%>
                                        <td class="auto-style8">Solution Description</td>
                                        <td class="auto-style10">
                                            <asp:TextBox ID="txtSolutionsDesc" runat="server" Width="234px"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style8">Project </td>
                                        <td class="auto-style10">
                                            <asp:TextBox ID="txtProject" runat="server" Width="234px"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td class="auto-style8">
                                            <asp:Button ID="btnInsert" runat="server" Height="30px" Text="Insert" Width="200px" OnClick="btnInsert_Click" />
                                        </td>
                                        <td class="auto-style10" colspan="2">
                                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                        </td>

                                    </tr>

                                </table>



                            </div>

                            <!---- FOR GRIDVIEW -->

                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:GridView runat="server" ShowHeaderWhenEmpty="true" Width="100%" ID="gv_Solution" AutoGenerateColumns="false"
                                           
                                            AllowPaging="True">

                                            <%--
                                                OnDataBound="gv_WFRolls_DataBound"
                                            OnRowDataBound="gv_WFRolls_RowDataBound"
                                            OnRowEditing="gv_WFRolls_RowEditing"
                                            OnRowCancelingEdit="gv_WFRolls_RowCancelingEdit"
                                            OnRowUpdating="gv_WFRolls_RowUpdating"
                                            OnPageIndexChanging="gv_WFRolls_SelectedIndexChanged"                                             
         
                                            --%>


                                            <PagerStyle CssClass="gridview"></PagerStyle>


                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />

                                            <Columns>

                                                <%--<asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image"
                                                    UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>--%>

                                                <asp:TemplateField HeaderText="Solution ID ">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSolution_Code" runat="server" Text='<%# Bind("Solution_Code") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Solution Description">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtSolutionsDescEdit" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSolutionsDesc" runat="server" Text='<%# Bind("Solution_Desc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Project">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtProject" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProject" runat="server" Text='<%# Bind("Project") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Remove">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="BtnRemove_Solution" runat="server" ImageUrl="~/Images/Delete_16x16.png" OnClick="BtnRemove_Solution_Click" ToolTip="Remove" />
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

