<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="StateMaster.aspx.cs" Inherits="DXBMS.Modules.Setup.StateMaster" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        
        .mGrid { 
    width: 100%; 
    background-color: #fff; 
    margin: 5px 0 10px 0; 
    border: solid 1px #525252; 
    border-collapse:collapse; 
}
.mGrid td { 
    padding: 2px; 
    border: solid 1px #c1c1c1; 
    color: #717171; 
}
.mGrid th { 
    padding: 4px 2px; 
    color: #fff; 
    background: #424242 repeat-x top; 
    border-left: solid 1px #525252; 
    font-size: 0.9em; 
}
.mGrid .alt { background: #fcfcfc repeat-x top; }
.mGrid .pgr { background: #424242 repeat-x top; }
.mGrid .pgr table { margin: 5px 0; }
.mGrid .pgr td { 
    border-width: 0; 
    padding: 0 6px; 
    border-left: solid 1px #666; 
    font-weight: bold; 
    color: #fff; 
    line-height: 12px; 
 }   
.mGrid .pgr a { color: #666; text-decoration: none; }
.mGrid .pgr a:hover { color: #000; text-decoration: none; }

        .auto-style1 {
            width: 282px;
        }

        .auto-style4 {
            width: 336px;
        }

        .auto-style5 {
            width: 154px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">

        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <div class="col-lg-12">
                        <fieldset>
                            <legend>State </legend>

                            <div class="container">


                                <table width="70%" style="border-collapse: separate; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">
                                    <tr>
                                        <td class="auto-style1">Country Code</td>
                                        <td class="auto-style5">
                                            <asp:TextBox ID="txtCountryCode" runat="server" Height="21px" MaxLength="3" Width="300px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style4">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style1">State Code</td>
                                        <td class="auto-style5">
                                            <asp:TextBox ID="txtStateCode" runat="server" Height="21px" MaxLength="3" Width="300px"></asp:TextBox></td>
                                        <td class="auto-style4">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style1">State Description</td>
                                        <td class="auto-style5">
                                            <asp:TextBox ID="txtStateDescription" runat="server" Height="21px" MaxLength="50" Width="300px"></asp:TextBox></td>
                                        <td class="auto-style4">
                                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style1">
                                            <asp:Button ID="btnInsert" runat="server"  Text="Insert" OnClick="btnInsert_Click" />
                                        </td>
                                        <td class="auto-style5">
                                            &nbsp;</td>
                                        <td class="auto-style4">
                                            &nbsp;</td>
                                    </tr>

                                </table>


                            </div>

                            <!---- FOR GRIDVIEW -->

                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:GridView runat="server" ShowHeaderWhenEmpty="true" Width="100%" ID="gv_state" AutoGenerateColumns="false" 
                                            OnRowDataBound="gv_state_RowDataBound"
                                            OnDataBound="gv_state_DataBound" 
                                            OnRowEditing="gv_state_RowEditing" 
                                            OnRowCancelingEdit="gv_state_RowCancelingEdit" 
                                            OnRowUpdating="gv_state_RowUpdating" 
                                            CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                                            
                                            

                                            <Columns>
                                                <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image"
                                                    UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>
                                            </Columns>


                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />


                                            <Columns>

                                                <asp:TemplateField HeaderText="Country Code" ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCountryCode" runat="server" Text='<%# Bind("CountryCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtCountryCode" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="State Code" ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStateCode" runat="server" Text='<%# Bind("StateCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%--<EditItemTemplate>
                                                        <asp:TextBox ID="txtStateCode" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>--%>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="State Description" ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCountryDesc" runat="server" Text='<%# Bind("StateDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtStateDescription" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>



                                                <asp:TemplateField HeaderText="Remove" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="BtnRemove_State" runat="server"
                                                            ImageUrl="~/Images/Delete_16x16.png" OnClick="BtnRemove_State_Click"
                                                            ToolTip="Remove" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                            </Columns>

                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblMsg" runat="server" Text="Item Added Succesfully" ForeColor="Green" Visible="false" Font-Bold="true"></asp:Label>
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

