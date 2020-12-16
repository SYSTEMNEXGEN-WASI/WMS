<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="WorkFlowRolls.aspx.cs" Inherits="DXBMS.Modules.SpareParts.Forms.WorkFlowRolls" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <style type="text/css">
        .auto-style1 {
            width: 314px;
        }

        .auto-style2 {
            width: 459px;
        }

        .auto-style4 {
            width: 458px;
        }

        .auto-style6 {
            width: 309px;
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
                            <legend>Rolls </legend>

                            <div class="table-responsive">


                                <table width="70%" style="border-collapse: separate; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">

                                    <tr>
                                        <%--<td class="auto-style10">ID</td>
                                        <td class="auto-style9"></td>--%>
                                        <td class="auto-style15">Rolls Description</td>
                                        <td class="auto-style14">
                                            <asp:TextBox ID="txtRollsDesc" runat="server" Width="234px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style10">Priority </td>
                                        <td class="auto-style9">
                                            <asp:TextBox ID="txtPriority" runat="server" Width="234px"></asp:TextBox>
                                        </td>

                                    </tr>

                                    <tr>
                                        <td class="auto-style15">Valid From </td>
                                        <td class="auto-style14">
                                            <asp:TextBox ID="txtValidFrom" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;" Width="234px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtValidFrom" Format="dd-MM-yyyy"></asp:CalendarExtender>
                                        </td>
                                        <td class="auto-style15">Valid To </td>
                                        <td class="auto-style14">
                                            <asp:TextBox ID="txtValidTo" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;" Width="234px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtValidTo" Format="dd-MM-yyyy"></asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>

                                        <td class="auto-style15">Active</td>
                                        <td class="auto-style14" colspan="3">
                                            <asp:DropDownList ID="ddlActive" runat="server" Height="23px" Width="234px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="auto-style4" colspan="2">
                                            <asp:Button ID="btnInsert" runat="server" Height="30px" Text="Insert" Width="200px" OnClick="btnInsert_Click" />
                                        </td>
                                        <td class="auto-style2" colspan="2">
                                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                        </td>
                                    </tr>

                                </table>



                            </div>

                            <!---- FOR GRIDVIEW -->

                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:GridView runat="server" ShowHeaderWhenEmpty="true" Width="100%" ID="gv_WFRolls" AutoGenerateColumns="false"
                                            OnDataBound="gv_WFRolls_DataBound"
                                            OnRowDataBound="gv_WFRolls_RowDataBound"
                                             OnRowEditing="gv_WFRolls_RowEditing"
                                            OnRowCancelingEdit="gv_WFRolls_RowCancelingEdit"
                                            OnRowUpdating="gv_WFRolls_RowUpdating"
                                             OnPageIndexChanging="gv_WFRolls_SelectedIndexChanged"
                                            AllowPaging="True">

                                            <%--
                                               
                                                 
                                            
                                           
                                            
                                            --%>


                                            <PagerStyle CssClass="gridview"></PagerStyle>


                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />

                                            <Columns>

                                                <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image"
                                                    UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>

                                                <asp:TemplateField HeaderText="Rolls ID ">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRollsID" runat="server" Text='<%# Bind("RollsID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Rolls Description">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtRollsDescEdit" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRollsDesc" runat="server" Text='<%# Bind("RollsDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Rolls Priority">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtPriorityEdit" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPriority" runat="server" Text='<%# Bind("Priority") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                 <asp:TemplateField HeaderText="Valid From">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValidFrom" runat="server" Text='<%# Bind("ValidFrom") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Valid To">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValidTo" runat="server" Text='<%# Bind("ValidTo") %>'></asp:Label>
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
                                                        <asp:ImageButton ID="BtnRemove_WorkFlowRolls" runat="server" ImageUrl="~/Images/Delete_16x16.png" OnClick="BtnRemove_WorkFlowRolls_Click" ToolTip="Remove" />
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

