<%@ Page Title="" Language="C#" theme="Theme1" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="TransferOfLocation.aspx.cs" Inherits="DXBMS.Modules.SpareParts.Forms.TransferOfLocation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" Runat="Server">

    <script src="../../../js/jquery-3.2.1.min.js" type="text/javascript"></script>
        <script src="../../../js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.js" type="text/javascript"></script>
        <script src="../../../js/chosen.jquery.min.js" type="text/javascript"></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
            
            <div class="Heading">Transfer Of Location</div>

        <table width="100%"  style="border-collapse:separate; border-spacing:5px;" class="table table-condensed table-bordered table-hover">
            <tr>
                <td width="12%">Transfer Note No</td>
                <td width="48%">
                    <asp:DropDownList ID="ddltransfer_no" runat="server" Width="62%"></asp:DropDownList>
                    <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddltransfer_no" QueryPattern="Contains"></asp:ListSearchExtender>
                </td>
                <td width="10%">Date</td>
                <td>
                    <asp:TextBox ID="txtdate" runat="server" Style="background-image: url(../../../Images/Calendar_16x16.png); background-repeat: no-repeat; padding-left: 25px;"
                        Width="100%" placeholder="dd/mm/yyyy"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MM-yyyy" TargetControlID="txtdate"></asp:CalendarExtender>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtdate" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>Remarks</td>
                <td colspan="3">
                    <asp:TextBox ID="txtremarks" runat="server" Width="300px" TextMode="MultiLine" MaxLength="100" Wrap="true" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtremarks" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
        <fieldset>
            <legend>Details</legend>

            <table width="100%" >
                <tr>
                    <td>
                        <asp:GridView ID="GridView_Location" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" Width="100%"
                        ShowFooter= "true" OnRowEditing="GridView_Location_RowEditing" OnRowCancelingEdit="GridView_Location_RowCancelingEdit"
                            OnRowUpdating="GridView_Location_RowUpdating" OnRowDataBound="GridView_Location_RowDataBound" OnDataBound="GridView_Location_DataBound" >
                        <HeaderStyle CssClass="GridHeader" />
                        <FooterStyle CssClass="GridFooter" />
                            

                            <Columns>
                               <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image" 
                                   UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>
                            </Columns>

                            <Columns>
                                <asp:TemplateField HeaderText="Item No" Visible="false">
                                    <ItemTemplate >
                                        <asp:Label ID="lblItemNo" runat="server" Text='<%# Bind("ItemCode") %>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part Item No">
                                    <ItemTemplate >
                                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("PartItemNo") %>' ></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="BtnAdd_Item" runat="server" CausesValidation="false"
                                        ImageUrl="~/Images/Grid_Update.png" OnClick="BtnAdd_Item_Click" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part Description">
                                    <ItemTemplate >
                                        <asp:Label ID="lblPartDesc" runat="server" Text='<%# Bind("ItemDesc") %>' ></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlItemCode" runat="server" Width="100%" OnSelectedIndexChanged="ddlItemCode_SelectedIndexChanged"></asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="From Loc">
                                    <ItemTemplate >
                                        <asp:Label ID="lblFromLocCode" runat="server" Text='<%# Bind("FromLocCode") %>' ></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtFromLoc" runat="server" Width="100%" Enabled="false" placeholder="From Location"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="To Loc">
                                    <ItemTemplate >
                                        <asp:Label ID="lblToLocCode" runat="server" Text='<%# Bind("ToLocCode") %>' ></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlToLoc_code" runat="server" Width="100%"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlToLoc_code" runat="server" Width="100%"></asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty" >
                                    <ItemTemplate >
                                        <asp:Label ID="lblQty" runat="server" Text='<%# Bind("Qty") %>' ></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtQty" runat="server" Width="50px" MaxLength="2" placeholder="Qty"></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtQty" runat="server" Width="50px" MaxLength="2" placeholder="Qty"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                
                              <asp:TemplateField HeaderText="Remove" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate >
                                <asp:ImageButton ID="BtnRemove_loc" runat="server" CausesValidation="false"
                                ImageUrl="~/Images/Delete_16x16.png" ToolTip="Remove" OnClick="BtnRemove_loc_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </fieldset>
        <table width="100%">
        <tr>
            <td>
                <asp:Label ID="success_add_item" runat="server" Text="Item Added Succesfully" ForeColor="Green" Font-Bold="true" ></asp:Label>
            </td>
            <td style="text-align:center;">
                <asp:Button ID="btn_save" runat="server" Text="Save"  CausesValidation="True" OnClick="btn_save_Click" />
                <asp:Button ID="btn_clear" runat="server" Text="Clear" CausesValidation="false" OnClick="btn_clear_Click" />          
            </td>
        </tr>
    </table>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
