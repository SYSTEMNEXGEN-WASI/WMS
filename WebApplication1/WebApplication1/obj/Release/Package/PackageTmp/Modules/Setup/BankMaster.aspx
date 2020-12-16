<%@ Page Title="" Theme="Theme1" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="BankMaster.aspx.cs" Inherits="DXBMS.Modules.Setup.BankMaster" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

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
        .auto-style6 {
            width: 282px;
            height: 41px;
        }
        .auto-style7 {
            width: 154px;
            height: 41px;
        }
        .auto-style8 {
            height: 41px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="Content" Runat="Server">

     <asp:UpdatePanel ID="UpdatePanel1" runat="server">

        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <div class="col-lg-12">
                        <fieldset>
                            <legend>Bank</legend>                           

                            <div >
                                <table width="100%" style="border-collapse: separate; border-spacing: 5px;"
                                    class="table table-condensed table-bordered table-hover">                                   
                                    <tr>
                                        <td class="auto-style1">Bank Code</td>
                                        <td class="auto-style5">                                          
                                            <asp:TextBox ID="txtBankCode" runat="server" Height="21px" MaxLength="4" Width="300px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBankCode" ErrorMessage="Enter Bank Code" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </td>
                                        </tr>
                                    <tr>
                                        <td class="auto-style6">Bank Description</td>
                                        <td class="auto-style7">
                                            <asp:TextBox ID="txtBankDesc" runat="server" Height="21px" MaxLength="50" Width="300px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style8">

                                            
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBankDesc" ErrorMessage="Enter Bank Description" ForeColor="Red"></asp:RequiredFieldValidator>

                                            
                                        </td>
                                    </tr>

                                       <tr>
                                        <td class="auto-style1">Bank Status</td>
                                        <td class="auto-style5">
                                            <%--<asp:TextBox ID="txtBankStatus" runat="server" Height="21px" MaxLength="3" Width="300px"></asp:TextBox>--%>
                                            <asp:DropDownList ID="ddlBankStatus" runat="server" CssClass="col-xs-offset-0" Height="16px" Width="196px">                                                
                                                <asp:ListItem Value="OLD">OLD</asp:ListItem>
                                                <asp:ListItem Value="NEW">NEW</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>

                                            
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlBankStatus" ErrorMessage="Enter Bank Status" ForeColor="Red"></asp:RequiredFieldValidator>

                                            
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                       
                                        <td class="auto-style4">
                                            <asp:Button ID="btnInsert" runat="server" Height="30px" Text="Insert" Width="200px" OnClick="btnInsert_Click" Visible="false" />
                                        </td>
                                        <td class="auto-style1" colspan="2">
                                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                        </td>
                                        
                                    </tr>                                    

                                </table>


                            </div>

                            <!---- FOR GRIDVIEW -->

                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:GridView runat="server" ShowHeaderWhenEmpty="true" Width="100%" ID="gv_bank" AutoGenerateColumns="false" 
                                         OnDataBound="gv_bank_DataBound"
                                            OnRowDataBound="gv_bank_RowDataBound" GridLines="None"
                                            OnRowEditing="gv_bank_RowEditing" 
                                            OnRowCancelingEdit="gv_bank_RowCancelingEdit"
                                            OnRowUpdating="gv_bank_RowUpdating" 
                                            CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                            OnPageIndexChanging="gv_bank_SelectedIndexChanged" 
                                                AllowPaging="True">

                                           <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" />

                                            <Columns>
                                                <asp:CommandField ShowEditButton="true" CancelImageUrl="~/Images/Grid_Cancel.png" ButtonType="Image"
                                                    UpdateImageUrl="~/Images/Grid_Update.png" EditImageUrl="~/Images/Grid_Edit.png" CausesValidation="false"></asp:CommandField>
                                                <asp:TemplateField HeaderText="Bank Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBankCode" runat="server" Text='<%# Bind("BankCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Bank Description">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtBankDesc" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBankDesc" runat="server" Text='<%# Bind("BankDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Bank Status">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtBankStatus" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBankStatus" runat="server" Text='<%# Bind("BankStatus") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                             
                                                <asp:TemplateField HeaderText="Remove">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="BtnRemove_Bank" runat="server" ImageUrl="~/Images/Delete_16x16.png" OnClick="BtnRemove_Bank_Click" ToolTip="Remove" CausesValidation= "false" />
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
